using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DevExpress.Xpf.Grid;
using DevExpress.XtraReports.UI;
using ERP_BL.Databases;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;
using ERP_BL.Config;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using ERP_BL.ExchangeRates;

namespace ZAS_ERP
{
    public static class ExtensionMethods
    {
        private const UInt32 FLASHW_STOP = 0; //Stop flashing. The system restores the window to its original state.        private const UInt32 FLASHW_CAPTION = 1; //Flash the window caption.        
        private const UInt32 FLASHW_TRAY = 2; //Flash the taskbar button.        
        private const UInt32 FLASHW_ALL = 3; //Flash both the window caption and taskbar button.        
        private const UInt32 FLASHW_TIMER = 4; //Flash continuously, until the FLASHW_STOP flag is set.        
        private const UInt32 FLASHW_TIMERNOFG = 12; //Flash continuously until the window comes to the foreground.  


        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public UInt32 cbSize; //The size of the structure in bytes.            
            public IntPtr hwnd; //A Handle to the Window to be Flashed. The window can be either opened or minimized.


            public UInt32 dwFlags; //The Flash Status.            
            public UInt32 uCount; // number of times to flash the window            
            public UInt32 dwTimeout; //The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.        
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);



        public static void FlashWindow(this Window win, UInt32 count = UInt32.MaxValue)
        {
            //Don't flash if the window is active            
            if (win.IsActive) return;
            WindowInteropHelper h = new WindowInteropHelper(win);
            FLASHWINFO info = new FLASHWINFO
            {
                hwnd = h.Handle,
                dwFlags = FLASHW_ALL | FLASHW_TIMER,
                uCount = count,
                dwTimeout = 0
            };

            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            FlashWindowEx(ref info);
        }

        public static void StopFlashingWindow(this Window win)
        {
            WindowInteropHelper h = new WindowInteropHelper(win);
            FLASHWINFO info = new FLASHWINFO();
            info.hwnd = h.Handle;
            info.cbSize = Convert.ToUInt32(Marshal.SizeOf(info));
            info.dwFlags = FLASHW_STOP;
            info.uCount = UInt32.MaxValue;
            info.dwTimeout = 0;
            FlashWindowEx(ref info);
        }
    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand MarkReadUnread = new RoutedUICommand
            (
                "Read",
                "Read",
                typeof(SYSTEM_STATIC),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.M,  ModifierKeys.Alt )
                }
            );
        public static readonly RoutedUICommand ReplytoComment = new RoutedUICommand
            (
                "Reply",
                "ReplytoComment",
                typeof(SYSTEM_STATIC),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.R, ModifierKeys.Alt)
                }
            );

        public static readonly RoutedUICommand ChangeFlag = new RoutedUICommand
            (
                "FlagChange",
                "FlagChange",
                typeof(SYSTEM_STATIC),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.R, ModifierKeys.Alt)
                }
            );
        //Define more commands here, just like the one above
    }
    public static class SYSTEM_STATIC
    {
        public static string server;
        public static User currentUser;
        public static List<Permission> AllowedPermissions;
        public static List<ERP_BL.EmailIntegration.Email> userEmails;
        public static bool isLoadingPermissions;
        public static List<Role> currentUserRoles;
        public static List<ExchangeRateGroup> exchangeRateGroups;
        public static bool LoggedInByPowerUser;
        public static string gridTitle;
        private static object parallel; //Not used
        public static List<Product> allProducts;
        public static List<CostSheetField> costSheetFields;
        public static List<cmbitem> statusSources;
        public static List<cmbitem> currencySources;
        public static List<cmbitem> warrantySource;
        public static List<cmbitem> paymentTermSource;
        public static List<cmbitem> incoTermSource;
        public static List<cmbitem> vendorPaymentStatusSource;
        public static List<cmbitem> billTypesSource;

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
        public static List<Department> LoadCurrentUserDepartments()
        {
            DepartmentRepo departmentRepo = new DepartmentRepo();
            List<Department> userDepartments = new List<Department>();

            if (MainWindow.currentUserid == 0)
            {
                return departmentRepo.GetDepartments();
            }
            else
            {
                return SYSTEM_STATIC.currentUser.employee.departments;
            }

        }

        public static void LoadOutlookEmails()
        {

            try
            {
                userEmails = new List<ERP_BL.EmailIntegration.Email>();
                var outlookApp = new Microsoft.Office.Interop.Outlook.Application();
                var outlookNamespace = outlookApp.GetNamespace("MAPI");
                outlookNamespace.Logon("", "", Missing.Value, Missing.Value);
                Microsoft.Office.Interop.Outlook.Folder inbox = outlookNamespace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderInbox) as Microsoft.Office.Interop.Outlook.Folder;
                Microsoft.Office.Interop.Outlook.Folder sentItems = outlookNamespace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderSentMail) as Microsoft.Office.Interop.Outlook.Folder;
                userEmails.AddRange(GetFilteredEmails(inbox, true));
                userEmails.AddRange(GetFilteredEmails(sentItems,true));

                try
                {
                    Microsoft.Office.Interop.Outlook.Folder publicFolder = outlookNamespace.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olPublicFoldersAllPublicFolders) as Microsoft.Office.Interop.Outlook.Folder;
                    userEmails.AddRange(GetFilteredEmails(publicFolder,  true));
                }
                catch { /* Public folders might not exist, ignore */ }

            }
            catch (Exception ex)
            {
            }
        }
        public static List<ERP_BL.EmailIntegration.Email> GetFilteredEmails(Microsoft.Office.Interop.Outlook.Folder folder, bool recursive = false)
        {
            List<ERP_BL.EmailIntegration.Email> results = new List<ERP_BL.EmailIntegration.Email>();
            try
            {
                Microsoft.Office.Interop.Outlook.Items items = folder.Items;
                foreach (object item in items)
                {
                    if (item is Microsoft.Office.Interop.Outlook.MailItem mail)
                    {
                        results.Add(new ERP_BL.EmailIntegration.Email
                        {
                            From = mail.SenderName,
                            To = mail.To,
                            Subject = mail.Subject,
                            Date = mail.ReceivedTime,
                            BodyHtml = mail.HTMLBody,
                            isUnRead = mail.UnRead
                        });

                        Marshal.ReleaseComObject(mail);
                    }
                }

                if (recursive)
                {
                    foreach (Microsoft.Office.Interop.Outlook.Folder subFolder in folder.Folders)
                    {
                        results.AddRange(GetFilteredEmails(subFolder, true));
                        Marshal.ReleaseComObject(subFolder);
                    }
                }
                Marshal.ReleaseComObject(items);
                return results;

            }
            catch (Exception ex)
            {
                return results;
                // Handle/log individual folder errors if needed
            }

        }

        public static List<Company> LoadCurrentUserCompanies()
        {
            List<Company> userCompanies = new List<Company>(); 

            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();
                return cont.GetCompanies();
            }
            else
            {
                userCompanies.AddRange(  SYSTEM_STATIC.currentUser.employee.Companies);
                userCompanies.AddRange(SYSTEM_STATIC.currentUser.employee.AdminBillCompanies);
                userCompanies.AddRange(SYSTEM_STATIC.currentUser.employee.TaskCompanies);
                var finalCompanies = userCompanies.GroupBy(x => x.Id).Select(y => y.FirstOrDefault());
                return finalCompanies.ToList(); 
            }


        }
        public static List<string> loadinquirytypes()
        {
            Config config = new Config();
            List<string> InquiryTypes = config.getInquiryType();




            return InquiryTypes;

        }
        public static List<string> loadPurchaseOrdertypes()
        {
            Config config = new Config();
            List<string> InquiryTypes = config.getPurchaseOrderTypes();
            return InquiryTypes;

        }
        public static List<cmbitem> loadCurrencies()
        {

            CurrencyRepo currencyRepo = new CurrencyRepo();
            List<Currency> currencies = currencyRepo.getAll();
            List<cmbitem> cmbitems = new List<cmbitem>();
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            foreach (Currency cur in currencies)
            {

                //cmbCurrency.Items.Add(new cmbitem() { name = cur.CurrencyName, id = cur.Id });
                cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
            }

            return cmbitems;

        }
        public static List<cmbitem> loadTargetTypes()
        {

            DepartmentRepo repo = new DepartmentRepo();
            var Types = repo.getallTargetType();
            List<cmbitem> cmbitems = new List<cmbitem>();
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            foreach (var cur in Types)
            {

                //cmbCurrency.Items.Add(new cmbitem() { name = cur.CurrencyName, id = cur.Id });
                cmbitems.Add(new cmbitem() { name = cur.Type, id = cur.Id });
            }

            return cmbitems;

        }
        /// <summary>
        ///Convert String To Memory Stream 
        /// </summary>
        /// <param name="memoryStream"> String</param>
        /// <returns></returns>
        public static MemoryStream ConvertToMemoryStream(string memoryStream)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(memoryStream);
            MemoryStream stream = new MemoryStream(byteArray);
            //MemoryStream Stream = new MemoryStream();
            //Stream.Position = 0;
            //StreamReader reader = new StreamReader(memoryStream);
            //memoryStream = reader.ReadToEnd();
            return stream;
        }
        /// <summary>
        /// Convert Memory Stream to String 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public static string ConvertToString(MemoryStream memoryStream)
        {
            //StreamWriter writer = new StreamWriter(memoryStream);
            //writer.Write(Layoutstream);
            //writer.Flush();
            string Layoutstream = "";
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(memoryStream);
            return Layoutstream = reader.ReadToEnd();

        }
        /// <summary>
        /// Set The Settings of Grid Control in a Window Based on Current User Settings
        /// </summary>
        /// <param name="window">Instance of a Window object</param>
        public static void SetUserSettingOfCurrentWindow(GridControl gridControl)
        {
            //foreach (object o in LogicalTreeHelper.GetChildren(window))
            //{
            //foreach (GridControl gridControl in FindVisualChildren<GridControl>(depObj))
            {

                //DependencyObject o = VisualTreeHelper.GetChild(depObj, i);
                //if (o is GridControl)
                //{
                //GridControl gridControl = (GridControl)o;
                if (MainWindow.currentUserid == 0)
                    return;

                EmployeeRepo employeeRepo = new EmployeeRepo();
                UserSettings userSetting = new UserSettings();
                userSetting = employeeRepo.GetUserSettingsByUser(MainWindow.currentUserid, gridControl.Name);
                if (userSetting != null)
                {


                    gridControl.RestoreLayoutFromStream(ConvertToMemoryStream(userSetting.settingValue));
                    SystemLog.LogInfo(gridControl.Parent.GetType(), "Settings restored for current window Grid Name = " + gridControl.Name);
                }
                //}

                //do something

            }
            ////foreach (object o in LogicalTreeHelper.GetChildren(window))
            ////{
            ////foreach (GridControl gridControl in FindVisualChildren<GridControl>(depObj))
            //{

            //    //DependencyObject o = VisualTreeHelper.GetChild(depObj, i);
            //    //if (o is GridControl)
            //    //{
            //    //GridControl gridControl = (GridControl)o;
            //    if (MainWindow.currentUserid == 0)
            //        return;

            //    //EmployeeRepo employeeRepo = new EmployeeRepo();
            //    //UserSettings userSetting = new UserSettings();
            //    //userSetting = employeeRepo.GetUserSettingsByUser(MainWindow.currentUserid, gridControl.Name);
            //     var userSetting=currentUser.userSettings.FirstOrDefault(x => x.settingkey == gridControl.Name);


            //    if (userSetting != null)
            //    {


            //        gridControl.RestoreLayoutFromStream(ConvertToMemoryStream(userSetting.settingValue));
            //        SystemLog.LogInfo(gridControl.Parent.GetType(), "Settings restored for current window Grid Name = " + gridControl.Name);
            //    }
            //    //}

            //    //do something

            //}
        }
        /// <summary>
        /// Get Layout from database and load in designer
        /// </summary>
        /// <param name="report"></param>
        public static void SetLayoutOfCurrentReport(XtraReport report)
        {
            //foreach (object o in LogicalTreeHelper.GetChildren(window))
            //{
            //foreach (GridControl gridControl in FindVisualChildren<GridControl>(depObj))
            {

                //DependencyObject o = VisualTreeHelper.GetChild(depObj, i);
                //if (o is GridControl)
                //{
                //GridControl gridControl = (GridControl)o;
                if (MainWindow.currentUserid == 0)
                    return;

                EmployeeRepo employeeRepo = new EmployeeRepo();
                UserSettings userSetting = new UserSettings();
                userSetting = employeeRepo.GetUserSettingsByUser(MainWindow.currentUserid, report.Name);
                if (userSetting != null)
                {


                    report.LoadLayoutFromXml(ConvertToMemoryStream(userSetting.settingValue));
                    SystemLog.LogInfo(report.Parent.GetType(), "Formated restored for Report Name = " + report.Name);

                }
                //}

                //do something

            }
        }
        internal static List<AchivedTarget> GetAchivedTargetByUser(int Year, string field)
        {
            DepartmentRepo repo = new DepartmentRepo();
            List<AchivedTarget> achivedTargets = new List<AchivedTarget>();
            SaleOrderRepo orderRepo = new SaleOrderRepo();
            ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
            EmployeeRepo employeeRepo = new EmployeeRepo();
            employee = employeeRepo.GetEmployee(currentUser.employee.EmpId);
            foreach (var company in employee.Companies)
                foreach (var department in employee.departments)
                {
                    var targets = repo.getTargets(department.Id, company.Id, Year);

                    if (targets == null || targets.Count == 0)
                        continue;
                    List<SaleOrder> saleOrders = new List<SaleOrder>();
                    saleOrders = orderRepo.getDepartmentalSOByYear(department.Id, company.Id, Year);
                    double[] arr = new double[13];
                    var propertyName = field;
                    for (int i = 0; i < 12; i++)
                    {
                        //var name = item == null ? null : typeof(SaleOrder).GetProperty(field).GetValue(item);
                        arr[i] = saleOrders.Where(x => x.CreationDate.Value.Month == i + 1).Sum(x => Convert.ToDouble(x.GetPropertyValue(propertyName))/* .GetType().GetProperty(propertyName).GetValue(x)*/);
                        arr[12] += arr[i];
                    }
                    foreach (var target in targets)
                    {
                        foreach (var award in target.TargetAwards)
                        {
                            AchivedTarget achivedAward = new AchivedTarget();
                            achivedAward.company = target.Company.CompanyName;
                            achivedAward.department = target.Department.parentDepartment != null ? target.Department.parentDepartment.DeptName + " " + target.Department.DeptName : target.Department.DeptName;
                            achivedAward.currency = target.Currency.CurrencyName;
                            achivedAward.year = target.Year;
                            achivedAward.frequency = target.Type.Frequency;
                            achivedAward.Target = target;
                            achivedAward.target = award.target;
                            achivedAward.Target.Type.HierarchicalIndex = award.Target.Type.HierarchicalIndex;

                            achivedAward.Month = award.Month;
                            achivedAward.name = award.name;
                            achivedAward.IndviualAward = award.IndviualAward;
                            achivedAward.TotalAward = award.TotalAward;
                            achivedAward.NoOfEmployees = award.NoOfEmployees;
                            switch (award.name)
                            {
                                case "January":
                                    achivedAward.achivedTarget = arr[0];
                                    break;

                                case "Feburary":
                                    achivedAward.achivedTarget = arr[1];
                                    break;
                                case "March":
                                    achivedAward.achivedTarget = arr[2];
                                    break;
                                case "April":
                                    achivedAward.achivedTarget = arr[3];
                                    break;
                                case "May":
                                    achivedAward.achivedTarget = arr[4];
                                    break;
                                case "June":
                                    achivedAward.achivedTarget = arr[5];
                                    break;
                                case "July":
                                    achivedAward.achivedTarget = arr[6];
                                    break;
                                case "August":
                                    achivedAward.achivedTarget = arr[7];
                                    break;
                                case "September":
                                    achivedAward.achivedTarget = arr[8];
                                    break;
                                case "October":
                                    achivedAward.achivedTarget = arr[9];
                                    break;
                                case "November":
                                    achivedAward.achivedTarget = arr[10];
                                    break;
                                case "December":
                                    achivedAward.achivedTarget = arr[11];
                                    break;

                                case "Total":
                                    achivedAward.achivedTarget = arr[12];
                                    break;

                            }
                            if (achivedAward.target <= achivedAward.achivedTarget)
                            {
                                var targ = achivedTargets.Find(x => x.name == achivedAward.name);
                                if (targ != null)
                                {
                                    if (targ.Target.Type.HierarchicalIndex < achivedAward.Target.Type.HierarchicalIndex)
                                    {
                                        achivedTargets.Remove(targ);
                                    }
                                }
                                achivedTargets.Add(achivedAward);
                            }

                        }
                    }
                }

            return achivedTargets;
        }
        internal static List<AchivedTarget> GetAllAchivedTarget(int Year, string field)
        {
            DepartmentRepo repo = new DepartmentRepo();
            List<AchivedTarget> achivedTargets = new List<AchivedTarget>();

            var targets = repo.getActiveTargetsForUserandYear(currentUser.id, Year);

            if (targets == null || targets.Count == 0)
                return achivedTargets;
            SaleOrderRepo orderRepo = new SaleOrderRepo();

            foreach (var target in targets)
            {
                List<SaleOrder> saleOrders = new List<SaleOrder>();
                saleOrders = orderRepo.getDepartmentalSOByYear((int)target.departmentId, (int)target.companyId, Year);
                double[] arr = new double[13];
                //var idName = "Id";
                //var idValue = 1;

                //var param = System.Linq.Expressions.Expression.Parameter(typeof(SaleOrder));
                //var condition =
                //    System.Linq.Expressions.Expression.Lambda<Func<SaleOrder, bool>>(
                //        System.Linq.Expressions.Expression.Equal(
                //            System.Linq.Expressions.Expression.Property(param, idName),
                //            System.Linq.Expressions.Expression.Constant(idValue, typeof(int))
                //        ),
                //        param
                //    ).Compile(); // for LINQ to SQl/Entities skip Compile() call

                //var item = saleOrders.SingleOrDefault(condition);
                var propertyName = field;
                for (int i = 0; i < 12; i++)
                {
                    //var name = item == null ? null : typeof(SaleOrder).GetProperty(field).GetValue(item);
                    arr[i] = saleOrders.Where(x => x.CreationDate.Value.Month == i + 1).Sum(x => Convert.ToDouble(x.GetPropertyValue(propertyName))/* .GetType().GetProperty(propertyName).GetValue(x)*/);
                    arr[12] += arr[i];
                }
                foreach (var award in target.TargetAwards)
                {
                    AchivedTarget achivedAward = new AchivedTarget();
                    achivedAward.company = target.Company.CompanyName;
                    achivedAward.department = target.Department.parentDepartment != null ? target.Department.parentDepartment.DeptName + " " + target.Department.DeptName : target.Department.DeptName;
                    achivedAward.currency = target.Currency.CurrencyName;
                    achivedAward.year = target.Year;
                    achivedAward.frequency = target.Type.Frequency;
                    achivedAward.Target = target;
                    achivedAward.target = award.target;

                    achivedAward.Month = award.Month;
                    achivedAward.name = award.name;
                    achivedAward.IndviualAward = award.IndviualAward;
                    achivedAward.TotalAward = award.TotalAward;
                    achivedAward.NoOfEmployees = award.NoOfEmployees;
                    switch (award.name)
                    {
                        case "January":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                            {
                                achivedAward.achivedTarget = arr[0];

                            }
                            else
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 1).Sum(x => x.target);
                            break;

                        case "Feburary":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[1];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 2).Sum(x => x.target);

                                for (int i = 0; i <= 1; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "March":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[2];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 3).Sum(x => x.target);

                                for (int i = 0; i <= 2; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "April":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[3];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 4).Sum(x => x.target);

                                for (int i = 0; i <= 3; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "May":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[4];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 5).Sum(x => x.target);

                                for (int i = 0; i <= 4; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "June":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[5];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 6).Sum(x => x.target);

                                for (int i = 0; i <= 5; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "July":

                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[6];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 7).Sum(x => x.target);

                                for (int i = 0; i <= 6; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "August":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[7];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 8).Sum(x => x.target);

                                for (int i = 0; i <= 7; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "September":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[8];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 9).Sum(x => x.target);

                                for (int i = 0; i <= 8; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "October":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[9];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 10).Sum(x => x.target);

                                for (int i = 0; i <= 9; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "November":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[10];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 11).Sum(x => x.target);

                                for (int i = 0; i <= 10; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;
                        case "December":
                            if (achivedAward.frequency == ERP_BL.Enums.TargetFrequency.Monthly)
                                achivedAward.achivedTarget = arr[11];
                            else
                            {
                                achivedAward.target = target.TargetAwards.Where(x => x.Month <= 12).Sum(x => x.target);

                                for (int i = 0; i <= 11; i++)
                                    achivedAward.achivedTarget += arr[i];
                            }
                            break;

                            //case "Total":
                            //    achivedAward.achivedTarget = arr[12];
                            //    break;

                    }
                    if (award.name != "Total")
                        achivedTargets.Add(achivedAward);
                }
            }
            return achivedTargets;
        }
        internal static List<AchivedTarget> GetAchivedTarget(int departmentId, int companyId, int Year, string field)
        {
            DepartmentRepo repo = new DepartmentRepo();
            List<AchivedTarget> achivedTargets = new List<AchivedTarget>();

            var targets = repo.getTargets(departmentId, companyId, Year);

            if (targets == null || targets.Count == 0)
                return achivedTargets;
            SaleOrderRepo orderRepo = new SaleOrderRepo();
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            saleOrders = orderRepo.getDepartmentalSOByYear(departmentId, companyId, Year);
            double[] arr = new double[13];
            //var idName = "Id";
            //var idValue = 1;

            //var param = System.Linq.Expressions.Expression.Parameter(typeof(SaleOrder));
            //var condition =
            //    System.Linq.Expressions.Expression.Lambda<Func<SaleOrder, bool>>(
            //        System.Linq.Expressions.Expression.Equal(
            //            System.Linq.Expressions.Expression.Property(param, idName),
            //            System.Linq.Expressions.Expression.Constant(idValue, typeof(int))
            //        ),
            //        param
            //    ).Compile(); // for LINQ to SQl/Entities skip Compile() call

            //var item = saleOrders.SingleOrDefault(condition);
            var propertyName = field;
            for (int i = 0; i < 12; i++)
            {
                //var name = item == null ? null : typeof(SaleOrder).GetProperty(field).GetValue(item);
                arr[i] = saleOrders.Where(x => x.CreationDate.Value.Month == i + 1).Sum(x => Convert.ToDouble(x.GetPropertyValue(propertyName))/* .GetType().GetProperty(propertyName).GetValue(x)*/);
                arr[12] += arr[i];
            }
            foreach (var target in targets)
            {
                foreach (var award in target.TargetAwards)
                {
                    AchivedTarget achivedAward = new AchivedTarget();
                    achivedAward.company = target.Company.CompanyName;
                    achivedAward.department = target.Department.parentDepartment != null ? target.Department.parentDepartment.DeptName + " " + target.Department.DeptName : target.Department.DeptName;
                    achivedAward.currency = target.Currency.CurrencyName;
                    achivedAward.year = target.Year;
                    achivedAward.frequency = target.Type.Frequency;
                    achivedAward.Target = target;
                    achivedAward.target = award.target;

                    achivedAward.Month = award.Month;
                    achivedAward.name = award.name;
                    achivedAward.IndviualAward = award.IndviualAward;
                    achivedAward.TotalAward = award.TotalAward;
                    achivedAward.NoOfEmployees = award.NoOfEmployees;
                    switch (award.name)
                    {
                        case "January":
                            achivedAward.achivedTarget = arr[0];
                            break;

                        case "Feburary":
                            achivedAward.achivedTarget = arr[1];
                            break;
                        case "March":
                            achivedAward.achivedTarget = arr[2];
                            break;
                        case "April":
                            achivedAward.achivedTarget = arr[3];
                            break;
                        case "May":
                            achivedAward.achivedTarget = arr[4];
                            break;
                        case "June":
                            achivedAward.achivedTarget = arr[5];
                            break;
                        case "July":
                            achivedAward.achivedTarget = arr[6];
                            break;
                        case "August":
                            achivedAward.achivedTarget = arr[7];
                            break;
                        case "September":
                            achivedAward.achivedTarget = arr[8];
                            break;
                        case "October":
                            achivedAward.achivedTarget = arr[9];
                            break;
                        case "November":
                            achivedAward.achivedTarget = arr[10];
                            break;
                        case "December":
                            achivedAward.achivedTarget = arr[11];
                            break;

                            //case "Total":
                            //    achivedAward.achivedTarget = arr[12];
                            //    break;

                    }
                    if (award.name != "Total")
                        achivedTargets.Add(achivedAward);
                }
            }
            return achivedTargets;
        }



        /// <summary>
        /// Save Report design to database
        /// </summary>
        /// <param name="window">Instance of Window</param>
        public static void SaveLayoutForCurrentReport(XtraReport report)
        {
            try
            {
                //foreach (object o in VisualTreeHelper.GetChild(depObj))
                //{
                ////for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)

                ////{

                ////    DependencyObject o = VisualTreeHelper.GetChild(depObj, i);

                ////    if (o is GridControl)
                ////    {
                //foreach (GridControl gridControl in FindVisualChildren<GridControl>(depObj))
                {
                    // do something with tb here

                    if (MainWindow.currentUserid == 0)
                        return;
                    EmployeeRepo employeeRepo = new EmployeeRepo();
                    UserSettings userSetting = new UserSettings();
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    report.SaveLayoutToXml(memoryStream);
                    userSetting.userId = MainWindow.currentUserid;
                    userSetting.settingkey = report.Name;
                    //using(StreamReader sr = new StreamReader(stream)) {
                    //// Read the report from the stream to a string variable.
                    //string s = sr.ReadToEnd();
                    userSetting.settingValue = ConvertToString(memoryStream).Trim();
                    userSetting.lastModified = DateTime.Now;
                    employeeRepo.SaveUserSetting(userSetting);
                    SystemLog.LogInfo(report.Parent.GetType(), "Formated Saved for Report Name = " + report.Name);



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Get Layout from database and load in designer
        /// </summary>
        /// <param name="report"></param>
        public static void SetReport(XtraReport report)
        {



            //if (MainWindow.currentUserid == 0)
            //    return;

            ReportRepo reportRepo = new ReportRepo();
            Report reporter = new Report();
            reporter = reportRepo.GetReportByName(report.Name);
            if (reporter != null)
            {


                report.LoadLayoutFromXml(ConvertToMemoryStream(reporter.ReportDesign));
            }

        }
        /// <summary>
        /// Save Report design to database
        /// </summary>
        /// <param name="window">Instance of Window</param>
        public static void SaveReport(int GroupId, String title, XtraReport report)
        {
            //if (MainWindow.currentUserid == 0)
            //    return;
            ReportRepo reportRepo = new ReportRepo();
            Report reporter = new Report();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            report.SaveLayoutToXml(memoryStream);
            //reporter.userId = MainWindow.currentUserid;
            reporter.ReportName = title;
            if (GroupId != 0)
                reporter.groupId = GroupId;
            //using(StreamReader sr = new StreamReader(stream)) {
            //// Read the report from the stream to a string variable.
            //string s = sr.ReadToEnd();
            reporter.ReportDesign = ConvertToString(memoryStream).Trim();
            reporter.lastModified = DateTime.Now;
            reportRepo.SaveReport(reporter);

        }

        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        public static string GenerateSHA512String(string inputString)
        {
            //MessageBox.Show("3");

            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
            MessageBox.Show("4");

        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        /// <summary>
        /// Save The User Settings for Current User and Current Window
        /// </summary>
        /// <param name="window">Instance of Window</param>
        public static void SaveUserSettingForCurrentWindow(GridControl gridControl)
        {
            //foreach (object o in VisualTreeHelper.GetChild(depObj))
            //{
            ////for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)

            ////{

            ////    DependencyObject o = VisualTreeHelper.GetChild(depObj, i);

            ////    if (o is GridControl)
            ////    {
            //foreach (GridControl gridControl in FindVisualChildren<GridControl>(depObj))
            {
                // do something with tb here

                if (MainWindow.currentUserid == 0)
                    return;
                
                EmployeeRepo employeeRepo = new EmployeeRepo();
                UserSettings userSetting = new UserSettings();
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                

                gridControl.SaveLayoutToStream(memoryStream);
                userSetting.userId = MainWindow.currentUserid;
                userSetting.settingkey = gridControl.Name;
                userSetting.settingValue = ConvertToString(memoryStream).Trim();
                userSetting.lastModified = DateTime.Now;
                employeeRepo.SaveUserSetting(userSetting);
                SystemLog.LogInfo(gridControl.Parent.GetType(), "Settings Saved for current window and Grid Name= " + gridControl.Name);
            }
        }
        /// <summary>
        /// Save The unboundReport Based on Current User Settings
        /// </summary>
        /// <param name="gridCon">Instance of a gridControl</param>
        /// <param name="ReportName">Report Name</param>
        public static void SaveUnBoundReport(GridControl gridControl, String ReportName, int type)
        {

            if (MainWindow.currentUserid == 0)
                return;
            ReportRepo reportRepo = new ReportRepo();
            UnBoundReport unBoundReport = new UnBoundReport();
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            unBoundReport.userId = MainWindow.currentUserid;
            unBoundReport.reportName = ReportName;
            unBoundReport.template = ConvertToString(memoryStream).Trim();
            unBoundReport.lastModified = DateTime.Now;
            unBoundReport.reportType = type;
            reportRepo.SaveUnBoundReport(unBoundReport);

            SystemLog.LogInfo(gridControl.Parent.GetType(), "Formate Saved for Unbound Report Name = " + gridControl.Name);
        }
        /// <summary>
        /// get The unboundReport Based on Current User Settings
        /// </summary>
        /// <param name="gridCon">Instance of a gridControl</param>
        /// <param name="ReportName">Report Name</param>
        public static void getUnboundReport(GridControl gridCon, String ReportName)
        {
            {

                if (MainWindow.currentUserid == 0)
                    return;

                ReportRepo reportRepo = new ReportRepo();
                UnBoundReport unBoundReport = new UnBoundReport();
                unBoundReport = reportRepo.GetUnBoundReportByUser(MainWindow.currentUserid, ReportName);
                if (unBoundReport != null)
                {
                    gridCon.RestoreLayoutFromStream(ConvertToMemoryStream(unBoundReport.template));
                    //SystemLog.LogInfo(gridCon.Parent.GetType(), "Formate restored for Unbound Report Name = " + gridCon.Name);
                }

            }
        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static void PopulateTransactionPanel()
        {
            SYSTEM_STATIC.statusSources = new List<cmbitem>();

            System.Threading.Thread th = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List Of Inquiries") != null)
                {

                    cmbitem treeItem1 = new cmbitem() { name = "Inquiries" };
                    cmbitem treeItema = new cmbitem() { name = "Inquiries(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (InquiryStatus status in inquiryrepo.getAllActiveStatus().OrderBy(x => x.Status).ToList())
                   foreach(var status in new InquiryRepo().getAllActiveStatus().OrderBy(x => x.Status).ToList())
                    {
                        Console.Write("1");
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new InquiryRepo().getinquiriessCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Inquiries" });
                    };
                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Inquiries(Closed)" };

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inquiry Statuses") != null))
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (InquiryStatus status in inquiryrepo.getAllInactiveStatus().OrderBy(x => x.Status).ToList())
                        foreach(var status in new InquiryRepo().getAllInactiveStatus().OrderBy(x => x.Status).ToList())
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new InquiryRepo().getinquiriessCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", bcolor = status.backcolor, description = "Inquiries" });
                        };

                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }


                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });

            th.Start();


            System.Threading.Thread th2 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List Of Offers") != null)
                {
                    OfferRepo offerRepo = new OfferRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Offers" };

                    cmbitem treeItema = new cmbitem() { name = "Offers(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (OfferStatus status in offerRepo.getAllActiveStatus().OrderBy(x => x.Status).ToList())
                    foreach(var  status in offerRepo.getAllActiveStatus().OrderBy(x => x.Status).ToList())
                    {
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new OfferRepo().getOffersCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Offers" });
                    };
                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Offers(Closed)" };

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Offer Statuses") != null))
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (OfferStatus status in offerRepo.getAllInactiveStatus().OrderBy(x => x.Status).ToList())
                        foreach(var status in offerRepo.getAllInactiveStatus().OrderBy(x => x.Status).ToList())
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new OfferRepo().getOffersCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", bcolor = status.backcolor, description = "Offers" });
                        };

                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }

                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });

            th2.Start();

            System.Threading.Thread th3 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
                {
                    SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Sale Orders" };
                    cmbitem treeItema = new cmbitem() { name = "Sale Orders(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (SaleOrderStatus status in saleOrderRepo.getAllActiveSaleOrderStatus().OrderBy(x => x.Status).ToList())
                    foreach(var status in saleOrderRepo.getAllActiveSaleOrderStatus().OrderBy(x => x.Status).ToList())
                    {
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new SaleOrderRepo().getsaleOrdersCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Sale Orders" });
                    };

                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Sale Orders(Closed)" };

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Order Statuses") != null))
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (SaleOrderStatus status in saleOrderRepo.getAllInActiveSaleOrderStatus().OrderBy(x => x.Status).ToList())
                        foreach(var status in saleOrderRepo.getAllInActiveSaleOrderStatus().OrderBy(x => x.Status).ToList())
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new SaleOrderRepo().getsaleOrdersCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString() + " )", bcolor = status.backcolor, description = "Sale Orders" });
                        };

                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }

                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });

            th3.Start();
            System.Threading.Thread th17 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List Of ModuleContracts") != null)
                {
                    ModuleContractRepo moduleContractRepo = new ModuleContractRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Module Contracts" };
                    cmbitem treeItema = new cmbitem() { name = "Module Contracts(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (SaleOrderStatus status in saleOrderRepo.getAllActiveSaleOrderStatus().OrderBy(x => x.Status).ToList())
                    foreach (var status in moduleContractRepo.getAllActiveStatus().OrderBy(x => x.Status).ToList())
                    {
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new ModuleContractRepo().getModuleContractsCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Module Contract" });
                    };

                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Module Contracts(Closed)" };

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive ModuleContracts Statuses") != null)
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (SaleOrderStatus status in saleOrderRepo.getAllInActiveSaleOrderStatus().OrderBy(x => x.Status).ToList())
                        foreach (var status in moduleContractRepo.getAllInactiveStatus().OrderBy(x => x.Status).ToList())
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new ModuleContractRepo().getModuleContractsCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString() + " )", bcolor = status.backcolor, description = "Module Contract" });
                        };

                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }

                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });
            th17.Start();

            System.Threading.Thread th4 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Sale Invoices") != null)
                {
                    SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Sale Invoices" };
                    cmbitem treeItema = new cmbitem() { name = "Sale Invoices(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (SaleInvoiceStatus status in saleInvoiceRepo.getAllActiveSaleInvoiceStatus().OrderBy(x => x.Status).ToList())
                    foreach(var status in saleInvoiceRepo.getAllActiveSaleInvoiceStatus().OrderBy(x => x.Status).ToList())
                    {
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new SaleInvoiceRepo().getsaleInvoicesCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Sale Invoices" });
                    };
                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Sale Invoices(Closed)" };

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Invoice Statuses") != null))
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (SaleInvoiceStatus status in saleInvoiceRepo.getAllInActiveSaleInvoiceStatus().OrderBy(x => x.Status).ToList())
                        foreach(var status in saleInvoiceRepo.getAllInActiveSaleInvoiceStatus().OrderBy(x => x.Status).ToList())
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new SaleInvoiceRepo().getsaleInvoicesCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString() + " )", bcolor = status.backcolor, description = "Sale Invoices" });
                        };

                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }
                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });

            th4.Start();

            System.Threading.Thread th5 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Memorandum Sales") != null)
                {
                    MemorandumSaleRepo memorandumSaleRepo = new MemorandumSaleRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Memorandum Sales" };
                    cmbitem treeItema = new cmbitem() { name = "Memorandum Sales(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (MemorandumSaleStatus status in memorandumSaleRepo.getAllActiveMemorandumSaleStatus().OrderBy(x => x.Status).ToList())
                    foreach(var status in memorandumSaleRepo.getAllActiveMemorandumSaleStatus().OrderBy(x => x.Status).ToList())
                    {
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new MemorandumSaleRepo().getmemorandumSalesCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Memorandum Sales" });
                    };

                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Memorandum Sales(Closed)" };

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Memorandum Sale Statuses") != null))
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (MemorandumSaleStatus status in memorandumSaleRepo.getAllInActiveMemorandumSaleStatus().OrderBy(x => x.Status).ToList())
                        foreach(var status in memorandumSaleRepo.getAllInActiveMemorandumSaleStatus().OrderBy(x => x.Status).ToList())
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new MemorandumSaleRepo().getmemorandumSalesCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString() + " )", bcolor = status.backcolor, description = "Memorandum Sales" });
                        };

                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }

                    SYSTEM_STATIC.statusSources.Add(treeItem1);

                }
            });

            th5.Start();

            System.Threading.Thread th6 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Purchase Orders") != null)
                {
                    PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Purchase Orders" };
                    cmbitem treeItema = new cmbitem() { name = "Purchase Orders(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (PurchaseOrderStatus status in purchaseOrderRepo.getAllActivePurchaseOrderStatus().OrderBy(x => x.Status).ToList())
                    foreach(var status in purchaseOrderRepo.getAllActivePurchaseOrderStatus().OrderBy(x => x.Status).ToList())
                    {
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = new PurchaseOrderRepo().getpurchaseOrdersCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString(), isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Purchase Orders" });
                    };
                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Purchase Orders(Closed)" };

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Order Statuses") != null))
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (PurchaseOrderStatus status in purchaseOrderRepo.getAllInActivePurchaseOrderStatus().OrderBy(x => x.Status).ToList())
                        foreach(var status in purchaseOrderRepo.getAllInActivePurchaseOrderStatus().OrderBy(x => x.Status).ToList())
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = new PurchaseOrderRepo().getpurchaseOrdersCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString(), bcolor = status.backcolor, description = "Purchase Orders" });
                        };
                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }

                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });

            th6.Start();

            System.Threading.Thread th7 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Bills") != null)
                {
                    BillRepo billRepo = new BillRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Bills" };
                    cmbitem treeItema = new cmbitem() { name = "Bills(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (BillStatus status in billRepo.getAllActiveBillStatus().OrderBy(x => x.Status).ToList())
                    foreach(var status in billRepo.getAllActiveBillStatus().OrderBy(x => x.Status).ToList())
                    {
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = new BillRepo().getbillsCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString(), isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Bills" });
                    };

                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Bills(Closed)" };

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Bill Statuses") != null))
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (BillStatus status in billRepo.getAllInActiveBillStatus().OrderBy(x => x.Status).ToList())
                        foreach(var status in billRepo.getAllInActiveBillStatus().OrderBy(x => x.Status).ToList())
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = new BillRepo().getbillsCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString(), bcolor = status.backcolor, description = "Bills" });
                        };

                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }

                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });

            th7.Start();

            System.Threading.Thread th8 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Bills(Pending for Apprvoal)") != null)
                {
                    BillRepo billRepo = new BillRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Bills(Pending for Approval)" };
                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });

            th8.Start();

            System.Threading.Thread th9 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Purchase Invoices") != null)
                {
                    PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Purchase Invoices" };
                    cmbitem treeItema = new cmbitem() { name = "Purchase Invoices(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (PurchaseInvoiceStatus status in purchaseInvoiceRepo.getAllActivePurchaseInvoiceStatus().OrderBy(x => x.Status).ToList())
                    foreach(var status in purchaseInvoiceRepo.getAllActivePurchaseInvoiceStatus().OrderBy(x => x.Status).ToList())
                    {
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new PurchaseInvoiceRepo().getpurchaseInvoicesCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Purchase Invoices" });
                    };
                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Purchase Invoices(Closed)" };

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Invoice Statuses") != null))
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (PurchaseInvoiceStatus status in purchaseInvoiceRepo.getAllInActivePurchaseInvoiceStatus().OrderBy(x => x.Status).ToList())
                        foreach (var status in purchaseInvoiceRepo.getAllInActivePurchaseInvoiceStatus().OrderBy(x => x.Status).ToList()) 
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new PurchaseInvoiceRepo().getpurchaseInvoicesCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString() + " )", bcolor = status.backcolor, description = "Purchase Invoices" });
                        };
                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }

                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });

            th9.Start();
            System.Threading.Thread th10 = new System.Threading.Thread(() =>
            {
                CurrencyRepo currencyRepo = new CurrencyRepo();
                List<Currency> currencies = currencyRepo.getAll();
                currencies = currencies.Where(x => x.isVoid != true).ToList();
                List<cmbitem> cmbitems = new List<cmbitem>();
                foreach(var currecny in  currencies)
                {
                    cmbitems.Add(new cmbitem() { name = currecny.CurrencyName + "(" + currecny.Abbrivation + " " + currecny.Symbol + ")", id = currecny.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
                currencySources = cmbitems;
            });
            th10.Start();
            System.Threading.Thread th11 = new System.Threading.Thread(() =>
            {
                List<Warranty> warrantys = new List<Warranty>();
                ProcurementRepo repo = new ProcurementRepo();
                warrantys = repo.GetActiveWarranties();
                List<cmbitem> cmbitems = new List<cmbitem>();
               
                foreach( var warranty in warrantys)
                {
                    cmbitems.Add(new cmbitem() { name = warranty.name, id = warranty.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
                warrantySource = cmbitems;
            });
            th11.Start();
            System.Threading.Thread th12 = new System.Threading.Thread(() =>
            {
                List<cmbitem> cmbitems = new List<cmbitem>();
                try
                {
                    PaymentTermRepo TermRepo = new PaymentTermRepo();
                    List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
                    paymentTerms = TermRepo.getAll();
                    
                   
                    foreach( var paymentTerm in paymentTerms)
                    {
                        cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
                    }
                    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
                }
                catch(Exception ex) { }
                paymentTermSource = cmbitems;
            });
            th12.Start();
            System.Threading.Thread th13 = new System.Threading.Thread(() =>
            {
                IncotermRepo termRepo = new IncotermRepo();
                List<Incoterm> incoterms = new List<Incoterm>();
                incoterms = termRepo.getAll();
                List<cmbitem> cmbitems = new List<cmbitem>();
               
                foreach( var incoterm in incoterms)
                {
                    cmbitems.Add(new cmbitem() { name = incoterm.term, id = incoterm.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
                incoTermSource = cmbitems;
            });
            th13.Start();
            System.Threading.Thread th14 = new System.Threading.Thread(() =>
            {
                VendorRepo vendorRepo = new VendorRepo();
                List<VendorPaymentStatus> VendorPaymentStatuss = new List<VendorPaymentStatus>();
                VendorPaymentStatuss = vendorRepo.getAllActiveVendorPaymentStatus();
                List<cmbitem> cmbitems = new List<cmbitem>();
                
                foreach(var status  in VendorPaymentStatuss)
                {
                    cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, fcolor = "#FF000000" });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
                vendorPaymentStatusSource = cmbitems;
            });
            th14.Start();
            System.Threading.Thread th15 = new System.Threading.Thread(() =>
            {
                BillRepo billRepo = new BillRepo();
                var BillTypes = billRepo.getActiveBillTypes();
                List<cmbitem> cmbitems = new List<cmbitem>();
                
                foreach( var type in BillTypes) 
                {
                    cmbitems.Add(new cmbitem() { name = type.billType, id = type.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
                billTypesSource = cmbitems;
            });
            System.Threading.Thread th16 = new System.Threading.Thread(() =>
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Inventory Adjustments") != null)
                {
                    AdjustmentRepo adjustmentRepo = new AdjustmentRepo();
                    cmbitem treeItem1 = new cmbitem() { name = "Inventory Adjustment" };
                    cmbitem treeItema = new cmbitem() { name = "Inventory Adjustment(Open)" };
                    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                    //foreach (PurchaseInvoiceStatus status in purchaseInvoiceRepo.getAllActivePurchaseInvoiceStatus().OrderBy(x => x.Status).ToList())
                    foreach (var status in adjustmentRepo.getAllActiveInventoryAdjustmentStatus().OrderBy(x => x.Status).ToList())
                    {
                        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new AdjustmentRepo().getInventoryAdjustmentCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString().ToString() + " )", isActive = (bool)status.isActive, bcolor = status.backcolor, description = "Inventory Adjustment" });
                    };
                    treeItema.Items = cmbItemsa;
                    treeItem1.Items.Add(treeItema);
                    cmbitem treeItemb = new cmbitem() { name = "Inventory Adjustment(Closed)" };

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inventory Adjustment Statuses") != null)
                    {
                        ICollection<cmbitem> cmbItems = new List<cmbitem>();
                        //foreach (PurchaseInvoiceStatus status in purchaseInvoiceRepo.getAllInActivePurchaseInvoiceStatus().OrderBy(x => x.Status).ToList())
                        foreach (var status in adjustmentRepo.getAllInActiveInventoryAdjustmentStatus().OrderBy(x => x.Status).ToList())
                        {
                            cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, fcolor = "( " + new AdjustmentRepo().getInventoryAdjustmentCountByStatusId(SYSTEM_STATIC.currentUser.id, status.Id).ToString() + " )", bcolor = status.backcolor, description = "Inventory Adjustment" });
                        };
                        treeItemb.Items = cmbItems;
                        treeItem1.Items.Add(treeItemb);

                    }

                    SYSTEM_STATIC.statusSources.Add(treeItem1);
                }
            });

            th16.Start();

            th15.Start();

            




            th.Join();
            th2.Join();
            th3.Join();
            th17.Join();
            th4.Join();
            th5.Join();
            th6.Join();
            th7.Join();
            th8.Join();
            th9.Join();
            th10.Join();
            th11.Join();
            th12.Join();
            th13.Join();
            th14.Join();
            th15.Join();
            th16.Join();

            //treeViewTransactions.ItemsSource = TreeItems;
        }

        public static List<Product> GetItemsForCurrentUser()
        {
            try
            {

                ProductRepo productRepo = new ProductRepo();


                if (MainWindow.currentUserid == 0)
                {
                    if (allProducts == null || allProducts.Count() == 0)
                        allProducts = productRepo.getAll();

                    return allProducts;

                }
                if (allProducts == null || allProducts.Count() == 0)
                    allProducts = productRepo.getAllUserProducts(MainWindow.currentUserid);

                return allProducts;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //SystemLog.LogError(Type.GetType(Sys),ex.ToString());
                return new List<Product>();
            }
        }
       
        public static List<CostSheetField> GetCostSheetFields()
        {
            try
            {

                ProductRepo productRepo = new ProductRepo();


                //if (MainWindow.currentUserid == 0)
                //{
                //    if (allProducts == null || allProducts.Count() == 0)
                //        allProducts = productRepo.getAll();

                //    return allProducts;

                //}
                if (costSheetFields == null || costSheetFields.Count() == 0)
                    costSheetFields = productRepo.getAllUserCostSheetFields();
                return costSheetFields;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //SystemLog.LogError(Type.GetType(Sys),ex.ToString());
                return new List<CostSheetField>();
            }
        }
        public static List<cmbitem> GetActiveAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveTaskAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveTaskAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveMemoAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveMemoAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveSOAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveSOAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }
        public static List<cmbitem> GetActiveSTLAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveSTLAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }
        public static List<cmbitem> GetActivePaymentAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActivePaymentAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveRewardAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveRewardAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }
        public static List<cmbitem> GetActivePOAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActivePOAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }
        public static List<cmbitem> GetActiveSIAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveSIAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }
        public static List<cmbitem> GetActiveVBAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveVBAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }
        public static List<cmbitem> GetActiveSRAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveSRAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }
        public static List<cmbitem> GetActivePIAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActivePIAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }
        public static List<cmbitem> GetActiveABAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveABAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }


        public static List<cmbitem> GetActiveVehicleExpensesAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveVehicleExpensesAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveIBTAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveIBTAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }
        public static List<cmbitem> GetActiveOfferAttachmentCategories()
        {
            AttachmentsRepo repo = new AttachmentsRepo();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (var attach in repo.getActiveOfferAttachmentCategories())
            {
                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });
            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            return cmbitems;
        } 
        public static List<cmbitem> GetActiveModuletContractAttachmentCategories()
        {
            AttachmentsRepo repo = new AttachmentsRepo();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (var attach in repo.getActiveModuleContractrAttachmentCategories())
            {
                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });
            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            return cmbitems;
        }
        public static List<cmbitem> GetActiveInquiryAttachmentCategories()
        {
            AttachmentsRepo repo = new AttachmentsRepo();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (var attach in repo.getActiveInquiryAttachmentCategories())
            {
                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });
            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            return cmbitems;
        }

        public static List<cmbitem> GetActiveLAAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveLAAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveTravelingRecordAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveTravelingRecordAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveDocumentAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveDocumentAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveAssetAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveAssetAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveTenantAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveTenantAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveRentalContractAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveRentalContractAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveRentalOrderAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveRentalOrderAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveRentalInvoiceAttachmentCategories()
        {



            AttachmentsRepo repo = new AttachmentsRepo();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (var attach in repo.getActiveRentalInvoiceAttachmentCategories())
            {

                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            return cmbitems;

        }

        public static List<cmbitem> GetActiveProcurementProductsAttachmentCategories()
        {
            AttachmentsRepo repo = new AttachmentsRepo();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (var attach in repo.getActiveProcurementProductsAttachmentCategories())
            {
                cmbitems.Add(new cmbitem() { name = attach.Name, id = attach.Id });
            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            return cmbitems;
        }

        public static List<cmbitem> GetSoFields()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "Budgeted Margin(O.C)", description = "margin" });
            cmbitems.Add(new cmbitem() { name = "Budgeted Margin(M.E)", description = "BudgetedMargininBase" });
            cmbitems.Add(new cmbitem() { name = "Budgeted Margin(S.E)", description = "SalesBudgetedMargin" });
            cmbitems.Add(new cmbitem() { name = "Revised Margin(O.C)", description = "RevisedMargin" });
            cmbitems.Add(new cmbitem() { name = "Revised Margin(M.E)", description = "RevisedMargininBase" });
            cmbitems.Add(new cmbitem() { name = "Revised Margin(S.E)", description = "SalesRevisedMargin" });
            cmbitems.Add(new cmbitem() { name = "Actual Margin(O.C)", description = "ActualMargin" });
            cmbitems.Add(new cmbitem() { name = "Actual Margin(M.E)", description = "ActualMargininBase" });
            cmbitems.Add(new cmbitem() { name = "Actual Margin(S.E)", description = "SalesActualMargin" });
            cmbitems.Add(new cmbitem() { name = "S.O FOB Amount(O.C)", description = "totalFOBValue" });
            cmbitems.Add(new cmbitem() { name = "S.O Amount(O.C)", description = "totalCFRValue" });
            //cmbitems.Add(new cmbitem() { name = "S.O Amount(S.E)", description = "totalBaseFOBValue" });
            cmbitems.Add(new cmbitem() { name = "S.O Amount(M.E)", description = "totalBaseCFRValue" });
            cmbitems.Add(new cmbitem() { name = "S.O Amount(S.E)", description = "SoAmountSER" });
            cmbitems.Add(new cmbitem() { name = "S.O Amount(PER)", description = "SoAmountPER" });
            cmbitems.Add(new cmbitem() { name = "Commision(M.E)", description = "commisioninBase" });
            cmbitems.Add(new cmbitem() { name = "Commision(O.C)", description = "commision" });
            cmbitems.Add(new cmbitem() { name = "Total Quantity", description = "TotalQuantity" });
            cmbitems.Add(new cmbitem() { name = "BM Gross Profit (SE)", description = "BMgrossProfitSE" });
            cmbitems.Add(new cmbitem() { name = "BM Gross Profit (ME)", description = "BMgrossProfitME" });

            return cmbitems;
        }


        public static List<cmbitem> GetTargetFields()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "Budgeted Margin(O.C)", description = "margin" });
            cmbitems.Add(new cmbitem() { name = "Budgeted Margin(M.E)", description = "BudgetedMargininBase" });
            cmbitems.Add(new cmbitem() { name = "Budgeted Margin(S.E)", description = "SalesBudgetedMargin" });
            cmbitems.Add(new cmbitem() { name = "Revised Margin(O.C)", description = "RevisedMargin" });
            cmbitems.Add(new cmbitem() { name = "Revised Margin(M.E)", description = "RevisedMargininBase" });
            cmbitems.Add(new cmbitem() { name = "Revised Margin(S.E)", description = "SalesRevisedMargin" });
            cmbitems.Add(new cmbitem() { name = "Actual Margin(O.C)", description = "ActualMargin" });
            cmbitems.Add(new cmbitem() { name = "Actual Margin(M.E)", description = "ActualMargininBase" });
            cmbitems.Add(new cmbitem() { name = "Actual Margin(S.E)", description = "SalesActualMargin" });
            cmbitems.Add(new cmbitem() { name = "S.O FOB Amount(O.C)", description = "totalFOBValue" });
            cmbitems.Add(new cmbitem() { name = "S.O Amount(O.C)", description = "totalCFRValue" });
            //cmbitems.Add(new cmbitem() { name = "S.O Amount(S.E)", description = "totalBaseFOBValue" });
            cmbitems.Add(new cmbitem() { name = "S.O Amount(M.E)", description = "totalBaseCFRValue" });
            cmbitems.Add(new cmbitem() { name = "S.O Amount(S.E)", description = "SoAmountSER" });
            cmbitems.Add(new cmbitem() { name = "S.O Amount(PER)", description = "SoAmountPER" });
            cmbitems.Add(new cmbitem() { name = "Commision(M.E)", description = "commisioninBase" });
            cmbitems.Add(new cmbitem() { name = "Commision(O.C)", description = "commision" });
            cmbitems.Add(new cmbitem() { name = "Step Target Points", description = "TaskPoints" });

            cmbitems.Add(new cmbitem() { name = "BM Gross Profit (SE)", description = "BMgrossProfitSE" });
            cmbitems.Add(new cmbitem() { name = "BM Gross Profit (ME)", description = "BMgrossProfitME" });

            return cmbitems;
        }


        public static List<TreeItem> GetAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TreeItem> GetSTLAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveSTLAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TargetAward> TargetListforMonths()
        {
            List<TargetAward> targetList = new List<TargetAward>();
            targetList.Add(new TargetAward() { name = "January", Month = 1 });
            targetList.Add(new TargetAward() { name = "Feburary", Month = 2 });
            targetList.Add(new TargetAward() { name = "March", Month = 3 });
            targetList.Add(new TargetAward() { name = "April", Month = 4 });
            targetList.Add(new TargetAward() { name = "May", Month = 5 });
            targetList.Add(new TargetAward() { name = "June", Month = 6 });
            targetList.Add(new TargetAward() { name = "July", Month = 7 });
            targetList.Add(new TargetAward() { name = "August", Month = 8 });
            targetList.Add(new TargetAward() { name = "September", Month = 9 });
            targetList.Add(new TargetAward() { name = "October", Month = 10 });
            targetList.Add(new TargetAward() { name = "November", Month = 11 });
            targetList.Add(new TargetAward() { name = "December", Month = 12 });
            //targetList.Add(new TargetAward() { name = "Extra" });
            //targetList.Add(new TargetList() { name = "Total" });

            return targetList;
        }

        public static List<TreeItem> GetDocumentAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveDocumentAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

        public static List<TreeItem> GetAssetAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveAssetAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

        public static List<TreeItem> GetTenantAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveTenantAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

        public static List<TreeItem> GetRentalContractAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveRentalContractAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

        public static List<TreeItem> GetRentalOrderAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveRentalOrderAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }


        public static List<TreeItem> GetRentalInvoiceAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveRentalInvoiceAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

        public static List<TreeItem> GetSOAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveSOAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TreeItem> GetSIAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveSIAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TreeItem> GetPIAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActivePIAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

        public static List<TreeItem> GetPOAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActivePOAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TreeItem> GetSRAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveSRAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TreeItem> GetVBAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveVBAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TreeItem> GetPaymentAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActivePaymentAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TreeItem> GetAdminBillAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveABAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TreeItem> GetOfferAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveOfferAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

        public static List<TreeItem> GetModuleContractAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveModuleContractrAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

        public static List<TreeItem> GetIBTAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveIBTAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }
        public static List<TreeItem> GetIBCTAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveIBCTAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

        public static List<TreeItem> GetInquiryAttachmentsListByCategory(int TransactionId, ERP_BL.Enums.TransactionItemType type)
        {
            // HierarchicalDataTemplate logic for CmbItem template is used before commit # 399 
            List<TreeItem> AttachmentByCategory = new List<TreeItem>();
            AttachmentsRepo repo = new AttachmentsRepo();
            List<AttachmentCategory> attachCat = new List<AttachmentCategory>();
            attachCat = repo.getActiveInquiryAttachmentCategories();
            if (attachCat != null)
            {
                foreach (var attach in attachCat)
                {

                    TreeItem Category = new TreeItem();

                    //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                    Category = new TreeItem() { name = attach.Name, /*id = attach.Id, description = attach.description*/ };
                    //attachCat.Remove(attach);
                    List<Attachment> attachments = new List<Attachment>();
                    attachments = repo.getAttachmentOrderDsc(attach.Id, TransactionId, type);
                    List<cmbitem> AttachmentsList = new List<cmbitem>();

                    foreach (var attachment in attachments)
                    {
                        AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                    }
                    Category.Items = AttachmentsList;
                    AttachmentByCategory.Add(Category);
                }


            }
            else
            {
                List<Attachment> attachments = new List<Attachment>();
                attachments = repo.getAttachmentOrderDsc(TransactionId, type);
                List<cmbitem> AttachmentsList = new List<cmbitem>();

                foreach (var attachment in attachments)
                {
                    AttachmentsList.Add(new cmbitem { id = attachment.Id, name = attachment.fileName, description = attachment.fileServerAdress, bcolor = attachment.lastOpendate.ToShortDateString(), fcolor = attachment.currentStatus.ToString() });
                }
                TreeItem Category = new TreeItem();

                //if (AttachmentByCategory.Find(x=>x.id ==attach.Id)==null)
                Category = new TreeItem() { name = "Other", /*id = attach.Id, description = attach.description*/ };
                Category.Items = AttachmentsList;
                AttachmentByCategory.Add(Category);
            }
            return AttachmentByCategory;
        }

    }
    public class AchivedTarget : TargetAward
    {
        public double achivedTarget { get; set; }
        public int year { get; set; }
        public ERP_BL.Enums.TargetFrequency frequency { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public string currency { get; set; }
        public bool isactive { get; set; }

    }
    public class MyConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "Yes";
            else
                return "NO";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TargetList
    {
        public string name { get; set; }
        public double Target { get; set; }
        public double IndviualAward { get; set; }
        public int NoOfEmployees { get; set; }

        public double TotalAward { get; set; }


    }
    public class cmbitem
    {
        public string name { get; set; }
        public int id { get; set; }
        public string description { get; set; }
        public string bcolor { get; set; }
        public string fcolor { get; set; }
        public bool isActive { get; set; }
        public int parentId { get; set; }
        public ICollection<cmbitem> Items { get; set; }
        public cmbitem()
        {
            Items = new List<cmbitem>();
        }

    }
    public class TreeItem
    {
        public string name { get; set; }
        //public List<TreeItemLevel1> TreeItems { get; set; }
        public ICollection<cmbitem> Items { get; set; }
        public TreeItem()
        {
            Items = new List<cmbitem>();
        }
    }
    public class TreeItemLevel1
    {
        public string name { get; set; }

        public List<cmbitem> Items { get; set; }
        public TreeItemLevel1()
        {
            Items = new List<cmbitem>();
        }
    }
    public class CurrentUserSetting
    {
        //public MemoryStream Stream { get; set; }
        //public string Layoutstream { get; set; }

        //MemoryStreamConverter()
        //{
        //    Stream = new MemoryStream();
        //    Layoutstream = "";
        //}

        /// <summary>
        /// Convert Memory Stream to String 
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        public string converttostring(MemoryStream memoryStream)
        {
            //StreamWriter writer = new StreamWriter(memoryStream);
            //writer.Write(Layoutstream);
            //writer.Flush();
            string Layoutstream = "";
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(memoryStream);
            return Layoutstream = reader.ReadToEnd();

        }
        /// <summary>
        ///Convert String To Memory Stream 
        /// </summary>
        /// <param name="memoryStream"> String</param>
        /// <returns></returns>
        public MemoryStream convertToMemoryStream(string memoryStream)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(memoryStream);
            MemoryStream stream = new MemoryStream(byteArray);
            //MemoryStream Stream = new MemoryStream();
            //Stream.Position = 0;
            //StreamReader reader = new StreamReader(memoryStream);
            //memoryStream = reader.ReadToEnd();
            return stream;
        }
        public void SetUserSettingOfCurrentWindow(System.Windows.Window window)
        {
            foreach (object o in LogicalTreeHelper.GetChildren(window))
            {

                if (o is GridControl)
                {
                    //GridControl gridControl = (GridControl)o;
                    EmployeeRepo employeeRepo = new EmployeeRepo();
                    UserSettings userSetting = new UserSettings();
                    userSetting = employeeRepo.GetUserSettingsByUser(MainWindow.currentUserid, ((GridControl)o).Name);
                    if (userSetting != null)
                    {


                        ((GridControl)o).RestoreLayoutFromStream(convertToMemoryStream(userSetting.settingValue));
                    }
                }

                //do something

            }
        }
    }
    public class VisualDocumentPaginator : DocumentPaginator
    {
        Size m_PageSize;
        Size m_Margin;
        DocumentPaginator m_Paginator = null;
        int m_PageCount;
        Size m_ContentSize;
        ContainerVisual m_PageContent;
        ContainerVisual m_SmallerPage;
        ContainerVisual m_SmallerPageContainer;
        ContainerVisual m_NewPage;

        public VisualDocumentPaginator(DocumentPaginator paginator,
               Size pageSize, Size margin)
        {
            m_PageSize = pageSize;
            m_Margin = margin;
            m_Paginator = paginator;
            m_ContentSize = new Size(pageSize.Width - 2 * margin.Width,
                                     pageSize.Height - 2 * margin.Height);
            m_PageCount = (int)Math.Ceiling(m_Paginator.PageSize.Height /
                                            m_ContentSize.Height);
            m_Paginator.PageSize = m_ContentSize;
            m_PageContent = new ContainerVisual();
            m_SmallerPage = new ContainerVisual();
            m_NewPage = new ContainerVisual();
            m_SmallerPageContainer = new ContainerVisual();
        }

        Rect Move(Rect rect)
        {
            if (rect.IsEmpty)
            {
                return rect;
            }
            else
            {
                return new Rect(rect.Left + m_Margin.Width,
                                rect.Top + m_Margin.Height,
                                rect.Width, rect.Height);
            }
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            m_PageContent.Children.Clear();
            m_SmallerPage.Children.Clear();
            m_NewPage.Children.Clear();
            m_SmallerPageContainer.Children.Clear();
            DrawingVisual title = new DrawingVisual();
            using (DrawingContext ctx = title.RenderOpen())
            {
                FontFamily font = new FontFamily("Times New Roman");
                Typeface typeface =
                  new Typeface(font, FontStyles.Normal,
                               FontWeights.Bold, FontStretches.Normal);
                FormattedText text = new FormattedText("Page " +
                    (pageNumber + 1) + " of " + m_PageCount,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface, 14, Brushes.Black);
                ctx.DrawText(text, new Point(0, 0));
            }

            DocumentPage page = m_Paginator.GetPage(0);
            m_PageContent.Children.Add(page.Visual);
            RectangleGeometry clip = new RectangleGeometry(
              new Rect(0, m_ContentSize.Height * pageNumber,
                       m_ContentSize.Width, m_ContentSize.Height));
            m_PageContent.Clip = clip;
            m_PageContent.Transform =
              new TranslateTransform(0, -m_ContentSize.Height * pageNumber);
            m_SmallerPage.Children.Add(m_PageContent);
            m_SmallerPage.Transform = new ScaleTransform(0.95, 0.95);
            m_SmallerPageContainer.Children.Add(m_SmallerPage);
            m_SmallerPageContainer.Transform = new TranslateTransform(0, 24);
            m_NewPage.Children.Add(title);
            m_NewPage.Children.Add(m_SmallerPageContainer);
            m_NewPage.Transform =
                      new TranslateTransform(m_Margin.Width, m_Margin.Height);
            return new DocumentPage(m_NewPage, m_PageSize,
                       Move(page.BleedBox), Move(page.ContentBox));
        }

        public override bool IsPageCountValid
        {
            get
            {
                return true;
            }
        }

        public override int PageCount
        {
            get
            {
                return m_PageCount;
            }
        }

        public override Size PageSize
        {
            get
            {
                return m_Paginator.PageSize;
            }
            set
            {
                m_Paginator.PageSize = value;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                if (m_Paginator != null)
                    return m_Paginator.Source;
                return null;
            }
        }
    }
    public class GenericEnum<T>
    {
        private T genericMemberVariable;

        public GenericEnum(T value)
        {
            genericMemberVariable = value;
        }


        public T genericProperty { get; set; }
    }
}


