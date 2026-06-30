using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.HR;
using System;
using System.Collections.Generic;
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
using ZAS_ERP.Leaves;

namespace ZAS_ERP.HR
{
    /// <summary>
    /// Interaction logic for grdLeavesRegister.xaml
    /// </summary>
    public partial class grdLeavesRegister : UserControl
    {
        HrRepo hrRepo = new HrRepo();
        public int ApprovalCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int RegisterCount { get; set; } = 0;
        bool inputFormCloseFlag = false;
        public grdLeavesRegister()
        {
            InitializeComponent();
        }

        private void GrdEmployeeLeaves_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void GrdEmployeeLeaves_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void BtnNewLeave_Click(object sender, RoutedEventArgs e)
        {
            frmLeaveApplication frm = new frmLeaveApplication();
            Window win = new Window();
            frm.isAdmin = true;
            win.Content = frm;
            win.Title = "Add Leave";

            win.ShowDialog();
        }

        public void EditLeave()
        {
            try
            {
                frmLeaveApplication frm = new frmLeaveApplication();
                Window win = new Window();
                frm.isAdmin = true;
                frm.isEdit = true;
                if (grdLeaveRegisters.SelectedItem != null)
                {
                    var leave = (LeaveApplication)grdLeaveRegisters.SelectedItem;
                    //frm.selectedEmployee = leave.employee;
                    //frm.app = leave;
                    //frm.isAppr = (bool)leave.isApproved;
                    //frm.leaveType = leave.leave.LeaveType;
                    //frm.isHalf = (bool)leave.isHalf;
                    frm.leaveId = leave.Id;
                }



                win.Content = frm;
                win.Title = "Edit Employee Leave";

                win.ShowDialog();
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

        private void GrdLeaveRegisters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditLeave();
        }

        private void GrdLeaveRegisters_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
           
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdLeaveRegisters);
            DXMessageBox.Show("Layout is Saved successfully for this Register", "Successfully", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Leave Register") != null)
            {
                UpdateLeaveAppGrid();

                CountAdminVM vm = new CountAdminVM();
                this.DataContext = vm;
                //grdLeavesRecord.RefreshData();
                //menuGrid = new Menu();
                lblEmpLeaves.Text = "Leaves";
            }
            else
            {
                DXMessageBox.Show("You are not allowed to View Leave Register", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Stop);

            }
            

        }
        public void UpdateLeaveAppGrid()
        {
            // MessageBox.Show("clicked");
            var user = SYSTEM_STATIC.currentUser;
            if (user != null)
            {
                var LeaveLists = hrRepo.GetAllLeaveApplicationsMonth(DateTime.Now);
                grdLeaveRegisters.ItemsSource = LeaveLists;
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeaveRegisters);

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

                if (grdLeaveRegisters.GetFocusedRow() != null)
                {
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (LeaveApplication)grdLeaveRegisters.GetFocusedRow();
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
                                leave.leave.isApproved = true;
                                leave.stage = TransactionStage.Approved;

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
                                    leave.leave.isApproved = false;
                                    leave.stage = TransactionStage.AwaitingApproval;

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
                                    leave.leave.isApproved = false;
                                    leave.stage = TransactionStage.AwaitingApproval;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();

                                usersRepo.Add(TransactionInfo.Reviewed, leave.Id, 15, frmInputBox.comment);

                            }

                            else
                            {
                                leave.isApproved = false;
                                leave.leave.isApproved = false;
                                leave.stage = TransactionStage.AwaitingApproval;


                            }

                        if (inputFormCloseFlag == false)
                        {
                            hrRepo.UpdateLeaveApplication(leave);
                            MessageBox.Show("Leave is Approved (" + leave.Id + ")");
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
                            MessageBox.Show("Leave is Re-Approved (" + leave.Id + ")");
                            SystemLog.LogInfo(this.GetType(), "Leave is Re-Approved (" + leave.Id + ")");
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
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Leave List") != null)
                    {
                        //  var pendingApprovalAssets = assetRepo.GetAllPendingForApprovalAssets();
                        var voidLeaves = hrRepo.GetPendingForApprovalLeave();
                        if (voidLeaves != null)
                        {
                            grdLeaveRegisters.ItemsSource = voidLeaves;
                            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeaveRegisters);
                        }

                        ////grdSaleReceiptList.ItemsSource = saleReceipts;
                        //GetAllSaleReceipts obj = new GetAllSaleReceipts(saleReceipts);

                        //grdSaleReceiptList.ItemsSource = obj.SaleReceiptList;
                    }
                    else
                    {
                        grdLeaveRegisters.ItemsSource = null;
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
                //int empId = 0;
                //var user = SystemLogic.currentUser;
                //if (user != null)
                //{
                //    empId = user.employee.EmpId;
                //}

                if (MainWindow.currentUserid != 0)
                {
                    lblEmpLeaves.Text = "Leaves (Void)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Leaves") != null)
                    {
                        var voidLeaves = hrRepo.GetVoidLeave();
                        grdLeaveRegisters.ItemsSource = voidLeaves;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeaveRegisters);

                    }
                    else
                    {
                        grdLeaveRegisters.ItemsSource = null;

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
                //int empId = 0;
                //var user = SystemLogic.currentUser;
                //if (user != null)
                //{
                //    empId = user.employee.EmpId;
                //}

                if (MainWindow.currentUserid != 0)
                {
                    lblEmpLeaves.Text = "Leaves (Pending For Reapprovals)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Leave List") != null)
                    {
                        var pendingReapprovalLeaves = hrRepo.GetPendingForReapprovalLeave();
                        grdLeaveRegisters.ItemsSource = pendingReapprovalLeaves;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeaveRegisters);



                    }
                    else
                    {
                        grdLeaveRegisters.ItemsSource = null;

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //int empId = 0;
                //var user = SystemLogic.currentUser;
                //if (user != null)
                //{
                //    empId = user.employee.EmpId;
                //}
                if (MainWindow.currentUserid != 0)
                {
                    lblEmpLeaves.Text = "Leaves (Register)";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View All Leave Register") != null)
                    {
                        var leavesRegister = hrRepo.GetAllLeavesRegister();
                        grdLeaveRegisters.ItemsSource = leavesRegister;
                        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdLeaveRegisters);



                    }
                    else
                    {
                        grdLeaveRegisters.ItemsSource = null;

                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CountAdminVM vm = new CountAdminVM();
            this.DataContext = vm;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add leave application (Admin)") != null)
            {
                btnNewLeave.IsEnabled = true;
            }
            else
            {
                btnNewLeave.IsEnabled = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Leave application (Admin)") != null)
            {
                btnEditLeave.IsEnabled = true;
            }
            else
            {
                btnEditLeave.IsEnabled = false;
            }
          
        }
    }

    public class CountAdminVM
    {
        HrRepo hrRepo = new HrRepo();
        public int ApprovalCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int RegisterCount { get; set; } = 0;

        public CountAdminVM()
        {
            hrRepo = new HrRepo();
            var user = SYSTEM_STATIC.currentUser;
            if (user == null)
            {
                return;
            }
            var empId = user.employee.EmpId;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Leave List") != null)
            {
                ApprovalCount = hrRepo.getAllPendingForApprovalAdminCount();
            }


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Leaves") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Vehicles") != null)
            {
                VoidCount = hrRepo.getVoidRegisterAdministratorCount();
            }



            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View All Leave Register") != null)
            {
                ApproveunapprovedCount = hrRepo.getRegisterAdministratorCount();
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Leave List") != null)
            {
                ClosingCount = hrRepo.getAllPendingForClosingAdministratorCount();
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Leave List") != null)
            {
                ReApprovalCount = hrRepo.getAllPendingForReApprovalAdminCount();
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View All Leave Register") != null)
            {
                RegisterCount = hrRepo.getAllLeavesForRegister();
            }

        }
    }
}
