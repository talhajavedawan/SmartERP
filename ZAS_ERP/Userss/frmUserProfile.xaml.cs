using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.HR;
using Microsoft.Win32;
using ZAS_ERP.Employee;
using ZAS_ERP.Employeess;
using ZAS_ERP.Leaves;
using Brush = System.Windows.Media.Brush;
using Image = System.Windows.Controls.Image;

namespace ZAS_ERP.Userss
{
    /// <summary>
    /// Interaction logic for frmUserProfile.xaml
    /// </summary>
    public partial class frmUserProfile : Window
    {

        ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        public int ApprovalCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int RegisterCount { get; set; } = 0;
        bool inputFormCloseFlag = false;
        List<User> UsersForComments = new List<User>();
        UsersRepo userRepo = new UsersRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();


        public frmUserProfile()
        {
            InitializeComponent();
            var currentYear = DateTime.Now.Year;
            for (int i = 1990; i <= currentYear; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
                cmbYears.Items.Add(item);
                cmbYears.SelectedItem = item;
            }
            CountVM countVm = new CountVM();
            this.DataContext = countVm;
        }
        CurrencyRepo CurrencyRepo = new CurrencyRepo();
        SalesExchangeRate SalesExchangeRate = new SalesExchangeRate();
        UsersRepo rolesRepo = new UsersRepo();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
        SaleOrderRepo SaleOrderRepo = new SaleOrderRepo();
        List<SaleOrder> saleOrders = new List<SaleOrder>();
        public double TargetValue = 0;
        public double AchivedTarget = 0;
        public double SumAchivedTarget = 0;
        public double TotalCommission = 0;
        public Collection<DataPoint> Data { get; private set; }
        HrRepo hrRepo = new HrRepo();

        public class DataPoint
        {
            public string Argument { get; private set; }
            public double Value { get; private set; }
            public DataPoint(string argument, double value)
            {
                Argument = argument;
                Value = value;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            //Permissions
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Summary") != null)
            {
                tabSummary.Visibility = Visibility.Visible;

            }
            else
            {
                tabSummary.Visibility = Visibility.Hidden;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Teams") != null)
            {
                tabTeam.IsEnabled = true;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Permissions") != null)
            {
                tabPermissions.IsEnabled = true;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Leaves") != null)
            {
                tabLeaves.IsEnabled = true;

            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Edit Button in Profile") != null)
            {
                 btnEditInfo.IsEnabled = true;
            }
            
           if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View CLosed Year Leave Application") != null)
            {
                cmbYears.IsEnabled = true;
            }


            Data = new Collection<DataPoint>();

            SYSTEM_STATIC.AllowedPermissions = SYSTEM_STATIC.AllowedPermissions
   .GroupBy(p => p.Id)
   .Select(g => g.First())
   .ToList();
            grdPermissions.ItemsSource = SYSTEM_STATIC.AllowedPermissions.Distinct();
            var user = SYSTEM_STATIC.currentUser;
            if (user != null)
            {
                var userFullName = user.employee.person.FName + " " + user.employee.person.LName;
                lblUserName.Text = userFullName;
                if(user.employee.Desig != null)
                lblDesignation.Text = user.employee.Desig.Title;
            }

            //lblUserName.Text = MainWindow.currentUserName;
            try
            {
                grdemployee.ItemsSource = employeeRepo.GetTeamMembersbyUserId(SYSTEM_STATIC.currentUser.id, SYSTEM_STATIC.currentUser.employeeId);

            }
            catch { }
            employee = employeeRepo.GetEmployeeForProfile(SYSTEM_STATIC.currentUser.employeeId); /*SystemLogic.currentUser.employee*/;
           
            saleOrders = SaleOrderRepo.getAllAllcatedtoEmployee(SYSTEM_STATIC.currentUser.employeeId);
            lbltotalAmountSaleOrders.Text = saleOrders.Sum(x => x.totalCFRValue).ToString();
            lbltotalSaleOrders.Text = saleOrders.Count().ToString();
            lbltotalClosedSaleOrders.Text = saleOrders.Where(x => x.saleOrderStatus.isActive == false).Count().ToString();
            lbltotalPendingSaleOrders.Text = SaleOrderRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid).ToString();
            TotalCommission = Convert.ToDouble(saleOrders.Sum(x => x.commision));
            //Data = new Collection<DataPoint> {
            //            new DataPoint ("Bikes", 142345),
            //            new DataPoint ("Accessories", 266344),
            //            new DataPoint ("Components", 631359),
            //            new DataPoint ("Clothing", 120007)
            //};

            if (employee.SalesTarget != null)
                foreach (Company company in employee.Companies)
                {
                    double sumCompanyMargin=0;
                    foreach (SaleOrder saleOrder in saleOrders)
                    {
                        if (saleOrder.company_Id == company.Id)
                            if (employee.SalesTarget.CurrencyId == saleOrder.currency_Id)
                            {
                                SumAchivedTarget += Convert.ToDouble(saleOrder.ActualMargin);
                                sumCompanyMargin += Convert.ToDouble(saleOrder.ActualMargin);
                            }
                            else if (employee.SalesTarget.CurrencyId == saleOrder.company.CurrencyId)
                            {
                                SumAchivedTarget += Convert.ToDouble(saleOrder.SalesActualMargin);
                                sumCompanyMargin += Convert.ToDouble(saleOrder.SalesActualMargin);
                            }
                            else
                            {
                                SalesExchangeRate = CurrencyRepo.getsalesexchangerate(company.Id, employee.SalesTarget.CurrencyId);
                                if (SalesExchangeRate != null)
                                {
                                    SalesExchangeRate ExchangeRate = new SalesExchangeRate();
                                    ExchangeRate = CurrencyRepo.getsalesexchangerate(company.Id, saleOrder.currency_Id);
                                    decimal marginexchangeRate = 1;
                                    if(ExchangeRate!=null)
                                    marginexchangeRate = ExchangeRate.exchangerate;
                                    //decimal margin = Convert.ToDecimal(txtActualMargin.Text);
                                    SumAchivedTarget += Convert.ToDouble((SalesExchangeRate.exchangerate * saleOrder.ActualMargin)/ marginexchangeRate);
                                    sumCompanyMargin += Convert.ToDouble((SalesExchangeRate.exchangerate * saleOrder.ActualMargin) / marginexchangeRate);
                                    //txtSaleAMargin.Text = (marginexchangeRate * margin).ToString();
                                }
                                else
                                {
                                    SumAchivedTarget += Convert.ToDouble(saleOrder.ActualMargin);
                                    sumCompanyMargin += Convert.ToDouble(saleOrder.ActualMargin);
                                }
                            }
                    }
                    SumAchivedTarget= Math.Round(SumAchivedTarget, 2);
                    sumCompanyMargin= Math.Round(sumCompanyMargin, 2);
                    Data.Add(new DataPoint (company.CompanyName,sumCompanyMargin));
                }
            series.DataSource = Data;
            if (employee.SalesTarget != null)
            {
                TargetValue = employee.SalesTarget.TargetAmount;

            }
            lbltotalCommission.Text = TotalCommission.ToString();
            lblTargetValue.Text = TargetValue.ToString();
            pbarTarget.Maximum = TargetValue;
            AchivedTarget = SumAchivedTarget/*Convert.ToDouble( saleOrders.Sum(x=>x.ActualMargin))*/;
            lbltotalActualValue.Text = AchivedTarget.ToString();
            pbarTarget.Value = AchivedTarget;
            txtPbarValue.Text = AchivedTarget.ToString() + "/" + TargetValue.ToString();

            //Permission to access edit info button

           
            
            
                //Loadd all employee data to profile
                LoadEmployeeInfo();
        }


        public void LoadEmployeeInfo()
        {
            try
            {
                //ERP_BL.Databases.Employee employee = employeeRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId); /*SystemLogic.currentUser.employee*/;
                if (employee != null)
                {
                    //EmployeeRepo repo = new EmployeeRepo();
                    //var user = repo.GetEmployee(72);
                    var byteImg = employee.person.Photo;
                    if (byteImg != null)
                    {
                        var image = GetBitmapImageFromByteArray(byteImg);
                        img.Source = image;
                    }

                    //Personal Info Groupbox
                    if (employee.person != null)
                    {
                        lblEmpId.Text = employee.EmployeeId;
                        lblFname.Text = employee.person.FName;
                        lblLname.Text = employee.person.LName;
                        lblFatherName.Text = employee.person.FatherName;
                        lblCnic.Text = employee.person.CNIC;
                        lblGender.Text = employee.person.Gender.ToString();
                        lblDob.Text = employee.person.DOB.ToString();
                        lblMaritalStatus.Text = employee.MaritalStatus;
                        lblPassport.Text = employee.person.PassportNo;
                        lblBloodGrp.Text = employee.person.BloodGroup;
                        if (employee.Desig != null)
                        {
                            lblDesig.Text = employee.Desig.Title;
                        }

                        if (employee.empFunction != null)
                        {
                            lblFunction.Text = employee.empFunction.Title;

                        }

                        //Permanent Address Groupbox Data
                        if (employee.address != null)
                        {
                            lblAddLine1.Text = employee.address.Line1;
                            lblAddLine2.Text = employee.address.Line2;
                            lblCity.Text = employee.address.City;
                            lblState.Text = employee.address.State;
                            lblCountry.Text = employee.address.Country;
                            lblZip.Text = employee.address.Zip.ToString();

                        }

                        /// Postal Address Groupbox Data
                        if (employee.address2 != null)
                        {
                            lblAddLine1_2.Text = employee.address2.Line1;
                            lblAddLine2_2.Text = employee.address2.Line2;
                            lblCity_2.Text = employee.address2.City;
                            lblState_2.Text = employee.address2.State;
                            lblCountry_2.Text = employee.address2.Country;
                            lblZip_2.Text = employee.address2.Zip.ToString();

                        }


                        ///  Contact Groupbox Data
                        if (employee.contact != null)
                        {
                            lblPhone.Text = employee.contact.ContactNo;
                            lblFax.Text = employee.contact.Fax;
                            lblEmail.Text = employee.contact.Email;
                            lblSm1.Text = employee.contact.SMLink1;
                            lblSm2.Text = employee.contact.SMLink2;
                            lblSm3.Text = employee.contact.SMLink3;
                            lblOffSkype.Text = employee.contact.OfficialSkype;
                            lblOffTeams.Text = employee.contact.OfficialTeams;
                            lblPerSkype.Text = employee.contact.PersonalSkype;
                            lblPerTeams.Text = employee.contact.PersonalTeams;

                        }

                        ///  Employment Info Groupbox Data
                        if (employee != null)
                        {
                            lblJobTitle.Text = employee.DesignationTitle;
                            if (employee.Supervisor != null)
                                lblSupervisor.Text = employee.Supervisor.person.FName + " " + employee.Supervisor.person.LName;
                            lblHireDate.Text = employee.HireDate.ToString();
                            lblJoinDate.Text = employee.JoinDate.ToString();
                            lblJD.Text = employee.JobDescription;
                            txtEmpBio.Text = employee.empDescription;

                        }
                    }


                }

                //lblname.Text = employee.person.FName + " " + employee.person.LName;

                //lblfather.Text = employee.person.FatherName;
                //lblemail.Text = employee.contact.Email;
                //lbljoinedDate.Text = employee.JoinDate.ToShortDateString();
                //lblmainPhone.Text = employee.contact.ContactNo;
                //lblwebsite.Text = employee.contact.Website;
                //lbldateofb.Text = employee.person.DOB.ToShortDateString();
                //lblcnic.Text = employee.person.CNIC;
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            



        }

        private void Grdemployee_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

            if (e.Column.FieldName == "Roles")

            {
                var user1 = grdemployee.GetRow(e.ListSourceRowIndex) as User;
                if (user1.Roles != null)
                {

                    //departments = user1.employee.departments; //*/e.GetListSourceFieldValue("employee.departments") as List<Department>;
                    string rolesstr = "";
                    if (user1.Roles != null)
                        foreach (var role in user1.Roles)
                        {
                            
                                rolesstr+= role.Name+ " , ";
                        }
                    e.Value = rolesstr;
                }

            }
        }

        private void BtnEditInfo_Click(object sender, RoutedEventArgs e)
        {
            EmployeeRepo rep = new EmployeeRepo();

            var user = SYSTEM_STATIC.currentUser;
            if (user != null)
            {
               var userId =  user.employee.EmpId;
                var emp = rep.GetEmployee(userId);
                frmEmployeeAdd frm = new frmEmployeeAdd();
                frm.isEdit = true;
                frm.isProfile = true;
                frm.editEmpId = userId;


                frm.ShowDialog();

            }

        }

        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public byte[] GetByteArrayFromBitmapImage(BitmapImage imageC)
        {
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
       

    private void ImageEditLoadToolButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            var mbResult = DXMessageBox.Show("Do you want to Save this picture?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbResult == MessageBoxResult.Yes)
            {
                EmployeeRepo rep = new EmployeeRepo();
                var user = SYSTEM_STATIC.currentUser;

                var bmImg = (BitmapImage)img.Source;
                //var byteImg =  getJPGFromImageControl(imgUser.Source as BitmapImage);
                if (bmImg != null)
                {
                    var byteImg = GetByteArrayFromBitmapImage(bmImg);



                    if (user != null)
                    {
                        //var img =  imgUser.Source;
                        /// var a = ImageToByteArray(imgUser);
                        rep.AddUserImage(byteImg, user);
                    }

                    MessageBox.Show("Image saved successfully");
                }
                else
                {
                    if (user != null)
                    {
                        rep.ClearUserImage(user);
                    }
                    MessageBox.Show("Image is Cleared!");

                }
            }
            else if (mbResult == MessageBoxResult.No)
            {
                return;
            }
            
        }

        private void BtnDelImage_Click(object sender, RoutedEventArgs e)
        {
           var mbResult = DXMessageBox.Show("Are you sure to want to delete the picture?","Confirmation",MessageBoxButton.YesNo,MessageBoxImage.Question);

            if (mbResult == MessageBoxResult.Yes)
            {
                img.Source = null;
            }
            else if (mbResult == MessageBoxResult.No)
            {
                return;
            }
        }

        private void ImageEditLoadToolButton_Click_1(object sender, RoutedEventArgs e)
        {
          
        }



        private void BtnBrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;* *.bmp;";
            // DialogResult result = openFileDialog.ShowDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                var filePath = System.IO.Path.GetFullPath(openFileDialog.FileName);
                var bmImg = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                img.Source = bmImg;
           } 
        }

        private void GrdLeavesRecord_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditLeave();
        }

        private void GrdLeavesRecord_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void BtnApplyLeave_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            frmLeaveApplication frm = new frmLeaveApplication();
            win.Title = "Leave Application Form";
            win.Content = frm;
            win.ShowDialog();
        
         
        }

        private void BtnNewLeave_Click(object sender, RoutedEventArgs e)
        {

            Window win = new Window();
            frmLeaveApplication frm = new frmLeaveApplication();
            frm.isEdit = false;
            win.Title = "Leave Application Form";
            win.Content = frm;
            win.ShowDialog();
            UpdateLeaveAppGrid();
        }

        public void EditLeave()
        {
            try
            {
                Window win = new Window();
                frmLeaveApplication frm = new frmLeaveApplication();

                if (grdLeavesRecord.SelectedItem != null)
                {
                    var item = (LeaveApplication)grdLeavesRecord.SelectedItem;
                    if (item != null)
                    {
                        var leaveId = item.Id;
                        frm.leaveId = leaveId;
                        frm.isEdit = true;
                        win.Title = "Leave Application Form";
                        win.Content = frm;
                        win.ShowDialog();
                        //UpdateLeaveAppGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnEditLeave_Click(object sender, RoutedEventArgs e)
        {

            EditLeave();
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdLeavesRecord);
            DXMessageBox.Show("Layout is Saved successfully for this Register", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //frmUserProfile uc = new frmUserProfile();
            //uc.LoadCount();
            //uc.UpdateLeaveAppGrid();
            //uc.grdLeavesRecord
            //this.Content = uc;

            UpdateLeaveAppGrid();
            LoadCount();

            CountVM vm = new CountVM();
            this.DataContext = vm;
            //grdLeavesRecord.RefreshData();
            //menuGrid = new Menu();
            lblEmpLeaves.Text = "Leaves";
        }

        public void LoadCount()
        {
            hrRepo = new HrRepo();
            var user = SYSTEM_STATIC.currentUser;
            if (user == null)
            {
                return;
            }
            var empId = user.employee.EmpId;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) own Leave List") != null)
            {
                ApprovalCount = hrRepo.getAllPendingForApprovalOwnCount(empId);
            }


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void own Leaves") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Vehicles") != null)
            {
                VoidCount = hrRepo.getVoidRegisterOwnCount(empId);
            }



            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View own Leave Register") != null)
            {
                ApproveunapprovedCount = hrRepo.getRegisterOwnCount(empId);
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) own Leave List") != null)
            {
                ClosingCount = hrRepo.getAllPendingForClosingOwnCount(empId);
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval)  own Leave Listl") != null)
            {
                ReApprovalCount = hrRepo.getAllPendingForReApprovalOwnCount(empId);
            }

        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            inputFormCloseFlag = false;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Leave without Approval") != null)
            {
                LeaveApplication leave = new LeaveApplication();
                HrRepo hrRepo = new HrRepo();
                UsersRepo usersRepo = new UsersRepo();

                if (grdLeavesRecord.GetFocusedRow() != null)
                {
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (LeaveApplication)grdLeavesRecord.GetFocusedRow();
                    if (selectedRow == null)
                    { return; }
                    if (selectedRow.Id != 0)
                    {
                        leave = hrRepo.GetLeaveApplication(selectedRow.Id);

                    }
                    else
                    { return; }

                    
                    if (leave.isApproved == false)
                    {
                        if (leave.isApproved != true)

                            if (
                                ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Leave without Approval") != null
                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Leave") != null)
                                ) ? true : false)
                            {
                                leave.isApproved = true;
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, leave.Id, 15, frmInputBox.comment);


                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Leave") != null)
                            {
                                if (leave.isApproved == null)
                                {
                                    leave.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, leave.Id, 15, frmInputBox.comment);
                            }

                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Leave") != null)
                            {
                                if (leave.isApproved == null)
                                {
                                    leave.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();

                                usersRepo.Add(TransactionInfo.Reviewed, leave.Id, 15, frmInputBox.comment);

                            }

                            else
                            {
                                leave.isApproved = false;
                            }

                        if (inputFormCloseFlag == false)
                        {
                            hrRepo.UpdateLeaveApplication(leave);
                            MessageBox.Show("Leave is Approved (" + leave.Id+ ")");
                            SystemLog.LogInfo(this.GetType(), "Leave is Approved (" + leave.Id + ")");
                        }
                    }
                    else if (leave.isReApproved == false)
                    {
                        if (leave.isReApproved != true)

                            if (
                                ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Leave without ReApproval") != null
                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for added Leave") != null)

                                ) ? true : false)
                            {
                                leave.isReApproved = true;
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, leave.Id, 15, frmInputBox.comment);


                            }


                            else
                            {
                                leave.isReApproved = false;
                            }

                        if (inputFormCloseFlag == false)
                        {
                            hrRepo.UpdateLeaveApplication(leave);
                            MessageBox.Show("Leave is Re-Approved (" + leave.Id+ ")");
                            SystemLog.LogInfo(this.GetType(), "Leave is Re-Approved (" + leave.Id+ ")");
                        }


                    }
                }

            }
            else
            {
                DXMessageBox.Show("You are not Allowed to Approve Leave Directly.", "Unauthorized Access", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void InputForm_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            //receiptListFlag = 0;

            var approve = MessageBox.Show("Do you want to Approve?", "Approval", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (approve == MessageBoxResult.Yes)
            {
                inputFormCloseFlag = false;
            }
            else
            {
                inputFormCloseFlag = true;
            }

            e.Cancel = false;
        }
        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int empId = 0;
                var user = SYSTEM_STATIC.currentUser;
                if (user != null)
                {
                    empId = user.employee.EmpId;
                }
                if (MainWindow.currentUserid != 0)
                {
                    lblEmpLeaves.Text = "Leaves (Pending for Approval)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) own Leave List") != null)
                    {
                        //  var pendingApprovalAssets = assetRepo.GetAllPendingForApprovalAssets();
                        var voidLeaves = hrRepo.GetPendingForApprovalLeave(empId);
                        if (voidLeaves != null)
                        {
                            grdLeavesRecord.ItemsSource = voidLeaves;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeavesRecord);
                        }
                        
                        ////grdSaleReceiptList.ItemsSource = saleReceipts;
                        //GetAllSaleReceipts obj = new GetAllSaleReceipts(saleReceipts);

                        //grdSaleReceiptList.ItemsSource = obj.SaleReceiptList;
                    }
                    else
                    {
                        grdLeavesRecord.ItemsSource = null;
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int empId = 0;
                var user = SYSTEM_STATIC.currentUser;
                if (user != null)
                {
                    empId = user.employee.EmpId;
                }

                if (MainWindow.currentUserid != 0)
                {
                    lblEmpLeaves.Text = "Leaves (Void)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void own Leaves") != null)
                    {
                        var voidLeaves = hrRepo.GetVoidLeave(empId);
                        grdLeavesRecord.ItemsSource = voidLeaves;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeavesRecord);

                    }
                    else
                    {
                        grdLeavesRecord.ItemsSource = null;

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int empId = 0;
                var user = SYSTEM_STATIC.currentUser;
                if (user != null)
                {
                    empId = user.employee.EmpId;
                }

                if (MainWindow.currentUserid != 0)
                {
                    lblEmpLeaves.Text = "Leaves (Pending For Reapprovals)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval)  own Leave List") != null)
                    {
                        var pendingReapprovalLeaves = hrRepo.GetPendingForReapprovalLeave(empId);
                        grdLeavesRecord.ItemsSource = pendingReapprovalLeaves;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeavesRecord);



                    }
                    else
                    {
                        grdLeavesRecord.ItemsSource = null;

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void UpdateLeaveAppGrid()
        {
            // MessageBox.Show("clicked");
            var user = SYSTEM_STATIC.currentUser;
            if (user != null)
            {
                var empLists = hrRepo.GetEmployeeLeaveApplications(user.employee.EmpId);
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Approved Leave Application Register") != null)
                {
                    lblEmpLeaves.Text = "Leaves";
                    grdLeavesRecord.ItemsSource = empLists;
                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeavesRecord);
                }
                else
                {
                    grdLeavesRecord.ItemsSource = null;
                    //DXMessageBox.Show("You are not allowed to view approved applications","Permission Denied",MessageBoxButton.OK,MessageBoxImage.Warning);
                }

            }
        }
        private void DXTabControl_SelectionChanged(object sender, TabControlSelectionChangedEventArgs e)
        {

            if (tabApp.SelectedItem == tabLeaveApp)
            {
                UpdateLeaveAppGrid();
                LoadCount();
            }
        }

        private void MbtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int empId = 0;
                var user = SYSTEM_STATIC.currentUser;
                if (user != null)
                {
                    empId = user.employee.EmpId;
                }
                if (MainWindow.currentUserid != 0)
                {
                    lblEmpLeaves.Text = "Leaves (Register)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View own Leave Register") != null)
                    {
                        var leavesRegister = hrRepo.GetAllLeavesRegister(empId);
                        grdLeavesRecord.ItemsSource = leavesRegister;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeavesRecord);



                    }
                    else
                    {
                        grdLeavesRecord.ItemsSource = null;

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbYears.SelectedItem != null)
                {
                    var cmb = (ComboBoxItem)cmbYears.SelectedItem;
                    var item = cmb.Content ;
                    var user = SYSTEM_STATIC.currentUser;
                    if (user != null)
                    {
                        var yearStr = item;
                    var year = Convert.ToInt32(yearStr);

                    var lst = hrRepo.GetEmployeeLeaveApplications(user.employee.EmpId, year);
                        lblEmpLeaves.Text = "leaves" + " (" + year.ToString() + " )";
                        if (lst == null)
                        {
                            grdLeavesRecord.ItemsSource = null;
                            grdLeavesRecord.RefreshData();

                        }
                        
                        else
                        {
                            grdLeavesRecord.ItemsSource = lst;
                            grdLeavesRecord.RefreshData();

                        }
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void CmbYears_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                //var user = SystemLogic.currentUser;
                //if (user != null)
                //{
                //    if (cmbYears.SelectedItem != null)
                //    //  { emp.person.BloodGroup = cmbBloodGroup.Text; }

                //    //  var item = cmbYears.SelectedItem.Text;
                //    // if (item != null)
                //    {
                //        var item = cmbYears.Text;
                //        if (item != "Year")
                //        {
                //            var year = Convert.ToInt32(item);
                //            var lst = hrRepo.GetEmployeeLeaveApplications(user.employee.EmpId, year);
                //            lblEmpLeaves.Text = "leaves" + " (" + year.ToString() + " )";
                //            grdLeavesRecord.ItemsSource = lst;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SaveEmpBio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = SYSTEM_STATIC.currentUser;
                if (user != null)
                {
                    var empId = user.employee.EmpId;
                    var emp = employeeRepo.GetEmployee(empId);
                    if (emp != null)
                    {
                        emp.empDescription = txtEmpBio.Text;
                        employeeRepo.updateEmployee(emp);
                        DXMessageBox.Show("Successfully Added Bio","Successfull",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                    }
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }

        }
        public void GenerateUsersForComments()
        {
            frmDepartmentSelect frm = new frmDepartmentSelect();
            frm.ShowDialog();
            if (frm.department != null)
            {
                var dept = frm.department;
                var users = userRepo.getusersByDepartment(dept.Id);
                if (users != null)
                {
                    UsersForComments = users;

                    //if (isProfile == true)
                    //{
                    UsersForComments.Add(SYSTEM_STATIC.currentUser);
                    //}
                    //else
                    //{
                    //    if (!String.IsNullOrEmpty(EmployeeIdStr.Text))
                    //    {
                    //        var user = empRepo.GetUserFromEmployee(Convert.ToInt32(EmployeeIdStr.Text));
                    //        if (user != null)
                    //        {
                    //            UsersForComments.Add(user);

                    //        }
                    //    }
                    //}
                }
            }
            //frm.Dispose();
        }
        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            
            GenerateUsersForComments();
            //department = cmbxDepartments.SelectedItem as Department;
            //if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
            //{
            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersForComments, TransactionItemType.Employee);
            inputBox.ShowDialog();

            var user = SYSTEM_STATIC.currentUser;
            if (user != null)
            {
                var EmployeeIdStr = user.employee.EmpId.ToString();
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && EmployeeIdStr != "")
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var _user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Employee #" + EmployeeIdStr, Convert.ToInt32(EmployeeIdStr), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Employee #" + EmployeeIdStr, Convert.ToInt32(EmployeeIdStr), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var _user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Employee #" + EmployeeIdStr, Convert.ToInt32(EmployeeIdStr), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Employee #" + EmployeeIdStr, Convert.ToInt32(EmployeeIdStr), TransactionItemType.Employee, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                    }

                    procurementRepo.Add(Convert.ToInt32(EmployeeIdStr), TransactionItemType.Employee, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");
                    loadcomments();
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
            //}
            //else
            //{
            // frmInputBox inputBox = new frmInputBox();
            //inputBox.ShowDialog();
            //}

            //if (EmployeeIdStr.Text != "")
            //{
              
              //  }
                //else if (EmployeeIdStr.Text == "")
                //{
                //    DXMessageBox.Show("Kindly save Employee first to add a comment!");
                //}

            }
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {

            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }
        public void loadcomments()
        {

            try
            {
                var user = SYSTEM_STATIC.currentUser;
                if (user != null)
                {
                    var EmployeeIdStr = user.employee.EmpId.ToString();
                    if (EmployeeIdStr != "")
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        List<CommentLog> comments = new List<CommentLog>();
                        comments = procurementRepo.getcommentslogAsc(Convert.ToInt32(EmployeeIdStr), TransactionItemType.Employee);
                        grdCommentss.ItemsSource = comments;
                    }
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void TabLeaves_Loaded(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Leave") != null)
            {
                btnNewLeave.IsEnabled = true;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Leave") != null)
            {
                btnEditLeave.IsEnabled = true;
            }
           

        }

        private void GrdLeavesRecord_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnAddNewLeave_Click(object sender, RoutedEventArgs e)
        {

        }

       
    }

    public class CountVM
    {
        HrRepo hrRepo = new HrRepo();
        public int ApprovalCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int RegisterCount { get; set; } = 0;

        public CountVM()
        {
            hrRepo = new HrRepo();
            var user = SYSTEM_STATIC.currentUser;
            if (user == null)
            {
                return;
            }
            var empId = user.employee.EmpId;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) own Leave List") != null)
            {
                ApprovalCount = hrRepo.getAllPendingForApprovalOwnCount(empId);
            }


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void own Leaves") != null)
            {
                VoidCount = hrRepo.getVoidRegisterOwnCount(empId);
            }



            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View own Leave Register") != null)
            {
                ApproveunapprovedCount = hrRepo.getRegisterOwnCount(empId);
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Leave List") != null)
            {
                ClosingCount = hrRepo.getAllPendingForClosingOwnCount(empId);
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval)  own Leave List") != null)
            {
                ReApprovalCount = hrRepo.getAllPendingForReApprovalOwnCount(empId);
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View All Leave Register") != null)
            {
                RegisterCount = hrRepo.getAllLeavesForRegister(empId);
            }

        }
    }
   
}
