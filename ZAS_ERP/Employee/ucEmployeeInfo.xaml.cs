using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.Employeess;

namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for ucEmployeeInfo.xaml
    /// </summary>
    public partial class ucEmployeeInfo : UserControl
    {
        public int ApprovalCount { get; set; }
        public int ClosingCount { get; set; }
        public int VoidCount { get; set; }
        public int ApproveunapprovedCount { get; set; }
        public int ReApprovalCount { get; set; }


        public static int empid;
        EmployeeRepo employeeRepo = new EmployeeRepo();
        bool inputFormCloseFlag = false;
        static EmployeeWorkingStatus statusChanged = new EmployeeWorkingStatus();



        public ucEmployeeInfo()
        {
            this.DataContext = this;
            InitializeComponent();
            LoadCount();
        }

        public ucEmployeeInfo(EmployeeWorkingStatus status)
        {
            statusChanged = status;
        }

        private void GrdEmployeeRegisters_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try { }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            EmployeeRepo rep = new EmployeeRepo();

            var row = (ERP_BL.Databases.Employee)grdEmployeeRegisters.GetFocusedRow() ;

            if (row != null)
            {
                var emp = rep.GetEmployee(row.EmpId);
                if (e.Column.Header == "Supervisor" && e.IsGetData)

                {
                     e.Value = row.Supervisor.person.FName;
                }

            }
           // var row = grdEmployeeRegisters.GetRowByListIndex(e.ListSourceRowIndex) as Target;
           
        }

        public void Update_Employee(int transactionId)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Employee") != null)
                {
                    frmEmployeeAdd frmEmployee = new frmEmployeeAdd();
                    frmEmployee.isEdit = true;       
         
                    var employee = employeeRepo.GetEmployee(transactionId);
                    
                    if (employee == null)
                    {
                        return;
                    }

                    frmEmployee.isEdit = true;
                    frmEmployee.isProfile = false;
                    frmEmployee.editEmpId = transactionId;
                    frmEmployee.ShowDialog();

                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Employee!","Permission Denied",MessageBoxButton.OK,MessageBoxImage.Stop);
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SaveGriddata_Click(object sender, RoutedEventArgs e)
        {

        }
        public List<ERP_BL.Databases.Employee> GetAllEmployees()
        {
            EmployeeRepo repo = new EmployeeRepo();
         var emp =   repo.GetAllEmployeesForRegister();
            return emp;
        }


        public void AddNewEmployee()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Employee") != null)
            {
                try
                {
                    frmEmployeeAdd frmemployee = new frmEmployeeAdd();
                    frmemployee.isEdit = false;
                    //frmemployee.Owner = obj;
                    frmemployee.ShowDialog();
                    //obj.loademployeegrid();
                }
                catch (Exception ex)
                {

                    DXMessageBox.Show(ex.Message);
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Add new Employee!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }
          

        }

        public void EditEmployee()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Employee") != null)
            {
                if (grdEmployeeRegisters.SelectedItem != null)
                {

                    if (grdEmployeeRegisters.GetFocusedRowCellValue(grdEmployeeRegisters.Columns.GetColumnByFieldName("EmpId")) != null)
                    {

                        empid = (int)grdEmployeeRegisters.GetFocusedRowCellValue(grdEmployeeRegisters.Columns.GetColumnByFieldName("EmpId"));

                        //MessageBox.Show(empid.ToString());
                    }
                    frmEmployeeAdd frmemployee = new frmEmployeeAdd();
                    // frmemployee.Owner = this;
                    frmemployee.isEdit = true;
                    frmemployee.isProfile = false;
                    frmemployee.editEmpId = empid;
                    frmemployee.ShowDialog();
                    //  var item = (ERP_BL.Databases.Employee)grdEmployeeRegisters.SelectedItem;



                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Employee!", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

        }
        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.currentUserid != 0)
                {
                    lblfaRegister.Text = "Employees(Void)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Employee") != null)
                    {
                        var voidAssets = employeeRepo.GetVoidEmployees();
                        grdEmployeeRegisters.ItemsSource = voidAssets;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdEmployeeRegisters);

                    }
                    else
                    {
                        grdEmployeeRegisters.ItemsSource = null;

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdEmployeeRegisters);
            DXMessageBox.Show("Layout is Saved successfully for this Register","Successfully",MessageBoxButton.OK,MessageBoxImage.Information);
        }

        
        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if  (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Employee") != null
                       || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Employee without Approval") != null)
          
                {

                    ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
                    var selectedRow = (ERP_BL.Databases.Employee)grdEmployeeRegisters.GetFocusedRow();

                    if (selectedRow == null)
                    { return; }

                    if (selectedRow.EmpId != 0)
                    {
                        employee = employeeRepo.GetEmployee(selectedRow.EmpId);

                    }

                    ucFrmDirectCloseEmp ucFrmDirectClose = new ucFrmDirectCloseEmp();
                    if (employee != null)
                    {
                        if (employee.employeeStatus != null)
                        {
                            ucFrmDirectClose.statusName.Text = employee.employeeStatus.Status;

                            var brush = new BrushConverter();
                            ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(employee.employeeStatus.backcolor);
                        }
                    }



                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                    ucFrmDirectClose.directCloseWin.Width = 395;
                    ucFrmDirectClose.directCloseWin.Height = 277;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    //receipt.saleReceiptStatus = statusChanged;
                    if (statusChanged.Id != 0)
                    {
                        if (employee.employeeApproval.isApproved == false 
                            || employee.employeeApproval.isReApproved == false 
                            || employee.employeeApproval.isVoid==true)
                        {
                            DXMessageBox.Show("You cannot close the Employee in this state", "Un-authorize to Close", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        else
                        {
                            employee.employeeStatus = statusChanged;

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Employee") != null
                               || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Employee without Approval") != null)
                            {
                                employee.employeeApproval.stage = TransactionStage.Closed.ToString();
                                employee.employeeApproval.PendingForClosing = false;
                                employee.isActive = false;
                                employeeRepo.updateEmployee(employee);
                                DXMessageBox.Show("Employee is Closed successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                            }

                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Employee") != null)

                            {

                                //asset.stage = TransactionStage. .ToString();
                                employee.isActive = false;

                                employeeRepo.updateEmployee(employee);
                                if (employee.employeeApproval.PendingForClosing == null)
                                {
                                    employee.employeeApproval.PendingForClosing = true;

                                }
                                //employee.isActive = false;

                                employeeRepo.updateEmployee(employee);

                                DXMessageBox.Show("Employee is Closed but is in 'Pending for Closing'", "Successfull", MessageBoxButton.OK, MessageBoxImage.Exclamation);


                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Employee") != null)

                            {
                                employee.isActive = false;

                                employeeRepo.updateEmployee(employee);
                                if (employee.employeeApproval.PendingForClosing == null)
                                {
                                    employee.employeeApproval.PendingForClosing = true;

                                }
                                //employee.isActive = false;

                                employeeRepo.updateEmployee(employee);
                                DXMessageBox.Show("Employee is Closed but is in 'Pending for Closing'", "Successfull", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                            }
                            else
                            {
                                DXMessageBox.Show("You do not have permission to directly close the Employee, Your request to close is in 'Pending for Closing'", "Un-authorize to Close", MessageBoxButton.OK, MessageBoxImage.Information);
                                employee.employeeApproval.PendingForClosing = true;


                                employeeRepo.updateEmployee(employee);

                            }
                        }
                        
                    }
                }
                else
                {
                    DXMessageBox.Show("You are not Allowed to Close Employee ", "Unauthorized Access", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            
            inputFormCloseFlag = false;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Employee without Approval") != null)
            {
                ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
                UsersRepo usersRepo = new UsersRepo();

                
                    if (grdEmployeeRegisters.GetFocusedRow() != null)
                    {
                        //SalesReceipt receipt = new SalesReceipt();
                        var selectedRow = (ERP_BL.Databases.Employee)grdEmployeeRegisters.GetFocusedRow();
                        if (selectedRow == null)
                        { return; }
                        if (selectedRow.EmpId != 0)
                        {
                            employee = employeeRepo.GetEmployee(selectedRow.EmpId);

                        }
                        else
                        { return; }

                        if (employee.employeeApproval == null)
                        {
                            return;
                        }
                    if (employee.employeeApproval.isApproved == false)
                    {
                        if (employee.employeeApproval.isApproved != true)

                            if (
                                ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Employee without Approval") != null
                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Employee") != null)

                                ) ? true : false)
                            {
                                employee.employeeApproval.isApproved = true;
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, employee.EmpId, 14, frmInputBox.comment);


                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Employee") != null)
                            {
                                if (employee.employeeApproval.isApproved == null)
                                {
                                    employee.employeeApproval.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, employee.EmpId, 14, frmInputBox.comment);
                            }

                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Employee") != null)
                            {
                                if (employee.employeeApproval.isApproved == null)
                                {
                                    employee.employeeApproval.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();

                                usersRepo.Add(TransactionInfo.Reviewed, employee.EmpId, 14, frmInputBox.comment);

                            }

                            else
                            {
                                employee.employeeApproval.isApproved = false;
                            }

                        if (inputFormCloseFlag == false)
                        {
                            employeeRepo.updateEmployee(employee);
                            MessageBox.Show("Employee is Approved (" + employee.EmpId + ")");
                            SystemLog.LogInfo(this.GetType(), "Employee is Approved (" + employee.EmpId + ")");
                        }
                    }
                    else if (employee.employeeApproval.isReApproved == false)
                    {
                        if (employee.employeeApproval.isReApproved != true)

                            if (
                                ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employee without ReApproval") != null
                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Employee") != null)

                                ) ? true : false)
                            {
                                employee.employeeApproval.isReApproved = true;
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, employee.EmpId, 14, frmInputBox.comment);


                            }


                            else
                            {
                                employee.employeeApproval.isReApproved = false;
                            }

                        if (inputFormCloseFlag == false)
                        {
                            employeeRepo.updateEmployee(employee);
                            MessageBox.Show("Employee is Re-Approved (" + employee.EmpId + ")");
                            SystemLog.LogInfo(this.GetType(), "Employee is Re-Approved (" + employee.EmpId + ")");
                        }


                    }
                }
                
            }
            else
            {
                DXMessageBox.Show("You are not Allowed to Approve Employee Directly.", "Unauthorized Access", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (MainWindow.currentUserid != 0)
            //    {
            //        lblfaRegister.Text = "Employees (Pending for Approval)";
            //        if (SystemLogic.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Employee List") != null)
            //        {
            //            //  var pendingApprovalAssets = assetRepo.GetAllPendingForApprovalAssets();
            //            var pendingApprovalAssets = employeeRepo.GetPendingForApprovalEmp();
            //            if (pendingApprovalAssets != null)
            //            {
            //                grdEmployeeRegisters.ItemsSource = pendingApprovalAssets;
            //                SystemLogic.SetUserSettingOfCurrentWindow(grdEmployeeRegisters);
            //            }



            //            ////grdSaleReceiptList.ItemsSource = saleReceipts;
            //            //GetAllSaleReceipts obj = new GetAllSaleReceipts(saleReceipts);

            //            //grdSaleReceiptList.ItemsSource = obj.SaleReceiptList;
            //        }
            //        else
            //        {
            //            grdEmployeeRegisters.ItemsSource = null;
            //        }

            //    }
            //}
            //catch(Exception ex) { MessageBox.Show(ex.Message); }
            loadingGif.Visibility = Visibility.Visible;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += DoWorkPending;
            worker.RunWorkerCompleted += DoWorkPendingCompleted;
            worker.RunWorkerAsync();
        }
        private void DoWorkPending(object o, DoWorkEventArgs args)
        {
            Task.Delay(1000).Wait();  // Pretend to work
        }

        private void DoWorkPendingCompleted(object o, RunWorkerCompletedEventArgs args)
        {

            try
            {
                if (MainWindow.currentUserid != 0)
                {
                    lblfaRegister.Text = "Employees (Pending for Approval)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Employee List") != null)
                    {
                        //  var pendingApprovalAssets = assetRepo.GetAllPendingForApprovalAssets();
                        var pendingApprovalAssets = employeeRepo.GetPendingForApprovalEmp();
                        if (pendingApprovalAssets != null)
                        {
                            grdEmployeeRegisters.ItemsSource = pendingApprovalAssets;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdEmployeeRegisters);
                        }



                        ////grdSaleReceiptList.ItemsSource = saleReceipts;
                        //GetAllSaleReceipts obj = new GetAllSaleReceipts(saleReceipts);

                        //grdSaleReceiptList.ItemsSource = obj.SaleReceiptList;
                    }
                    else
                    {
                        grdEmployeeRegisters.ItemsSource = null;
                    }

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            loadingGif.Visibility = Visibility.Hidden;
        }
        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {        
            //ucEmployeeInfo uc = new ucEmployeeInfo();
            //uc.LoadCount();
            //uc.loadData();
            //this.Content = uc;
            ////loadData();
            ////LoadCount(sender, e);
            loadingGif.Visibility = Visibility.Visible;
           
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Refresh;
            worker.RunWorkerCompleted += RefreshCompleted;
            worker.RunWorkerAsync();
        }

        private void Refresh(object o, DoWorkEventArgs args)
        {
            Task.Delay(1000).Wait();  // Pretend to work
        }

        private void RefreshCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            ucEmployeeInfo uc = new ucEmployeeInfo();
            uc.LoadCount();
            uc.loadData();
            this.Content = uc;
            //loadData();
            //LoadCount(sender, e);

            loadingGif.Visibility = Visibility.Hidden;
        }

        public void loadData()
        {
            var empLst = employeeRepo.GetAllEmployeesForRegister();
            if (empLst != null)
            {
                grdEmployeeRegisters.ItemsSource = empLst;                
            }

            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdEmployeeRegisters);
            lblfaRegister.Text = "Employees";
           
        }
        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.currentUserid != 0)
                {
                    lblfaRegister.Text = "Employees(Pending For Closing)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Employee List") != null)
                    {
                        var pendingClosingEmp = employeeRepo.GetPendingForClosingEmp();
                        grdEmployeeRegisters.ItemsSource = pendingClosingEmp;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdEmployeeRegisters);



                    }
                    else
                    {
                        grdEmployeeRegisters.ItemsSource = null;

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnNewEmp_Click(object sender, RoutedEventArgs e)
        {
            AddNewEmployee();
        }

        private void BtnEditEmp_Click(object sender, RoutedEventArgs e)
        {
            EditEmployee();
        }

        private void GrdEmployeeRegisters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditEmployee();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadCount();
        }

        public void LoadCount()
        {
            employeeRepo = new EmployeeRepo();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Employee List") != null)
                {
                    ApprovalCount = employeeRepo.getAllPendingForApprovalAdminCount();
                }
               

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Employee") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Vehicles") != null)
                {
                        VoidCount = employeeRepo.getVoidRegisterAdministratorCount();
                }
                


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View (Employee Register)") != null)
                {
                        ApproveunapprovedCount = employeeRepo.getRegisterAdministratorCount();
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Employee List") != null)
                {
                         ClosingCount = employeeRepo.getAllPendingForClosingAdministratorCount();
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Employee List") != null)
                {
                         ReApprovalCount= employeeRepo.getAllPendingForReApprovalAdminCount();
                }

     
            //frmEmployeeCenter.refreshGrid();
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.currentUserid != 0)
                {
                    lblfaRegister.Text = "Employees(Pending For Reapprovals)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Employee List") != null)
                    {
                        var pendingReapprovalEmp = employeeRepo.GetPendingForReapprovalEmp();
                        grdEmployeeRegisters.ItemsSource = pendingReapprovalEmp;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdEmployeeRegisters);



                    }
                    else
                    {
                        grdEmployeeRegisters.ItemsSource = null;

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainWindow.currentUserid != 0)
                {
                    lblfaRegister.Text = "Employees(Registers)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View (Employee Register)") != null)
                    {
                        var approvedUnapprovedEmp = employeeRepo.GetAllEmployees();
                        grdEmployeeRegisters.ItemsSource = approvedUnapprovedEmp;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdEmployeeRegisters);



                    }
                    else
                    {
                        grdEmployeeRegisters.ItemsSource = null;
                        DXMessageBox.Show("You are not allowed to view All employee Register","Unauthorize",MessageBoxButton.OK,MessageBoxImage.Stop);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
