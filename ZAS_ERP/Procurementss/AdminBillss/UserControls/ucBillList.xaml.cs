using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
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
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for usBillList.xaml
    /// </summary>
    public partial class ucBillList : UserControl
    {
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        AdminBillsRepo billsRepo = new AdminBillsRepo();
        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();

        static AdminBillStatus statusChanged = new AdminBillStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();

        bool isFullyPaid = true;
        public ucBillList()
        {
            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Admin Bill List") != null)
                {
                    ApprovalCount = billsRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Admin Bill") != null)
                    {
                        ApprovalCount = billsRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = billsRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Admin Bills List") != null)
                {
                    ReApprovalCount = billsRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Admin Bill") != null)
                    {
                        ReApprovalCount = billsRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = billsRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Admin Bills") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = billsRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = billsRepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = billsRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
                    //}
                    //else
                    //{
                    //    ApproveunapprovedCount = bankTransRepo.getBankTransferRegisterCountOWn(MainWindow.currentUserid);
                    //}
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                ApprovalCount = billsRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = billsRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Admin Bill List") != null)
                {
                    ClosingCount = billsRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Admin Bill") != null)
                    {
                        ClosingCount = billsRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = billsRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }

                }
            }


            else
            {
                ClosingCount = billsRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var billsList = billsRepo.GetAllOpenBills(SYSTEM_STATIC.currentUser.id);
            grdBillRegister.ItemsSource = billsList;
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdBillRegister);
            SetColumnsVisibility();
        }

        private void SetColumnsVisibility()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Summary Memo for Admin Bills") == null)
            {
                grdBillRegister.Columns["hasSummary"].Visible = false;
                grdBillRegister.Columns["hasSummary"].ShowInColumnChooser = false;
                grdBillRegister.Columns["managementSummary.SummaryName"].Visible = false;
                grdBillRegister.Columns["managementSummary.SummaryName"].ShowInColumnChooser = false;
                grdBillRegister.Columns["SummaryMemo"].Visible = false;
                grdBillRegister.Columns["SummaryMemo"].ShowInColumnChooser = false;
            }
        }

        private void GrdBillsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateBillWindow();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill") != null)
            {
                frmBillAdd = new ucFrmBillAdd();
                Window frmBill = new Window();
                frmBill.WindowState = WindowState.Maximized;
                frmBill.Title = "Enter Bills";
                frmBillAdd.editFlag = false;
                frmBill.Content = frmBillAdd;
                frmBill.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Bill!");
            }
               
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateBillWindow();
        }

        private void UpdateBillWindow()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill") != null)
            {
                frmBillAdd = new ucFrmBillAdd();
                Window frmBill = new Window();
                frmBill.WindowState = WindowState.Maximized;
                frmBill.Title = "Update Bills";

                var selectedRow = grdBillRegister.SelectedItem as AdminBill;

                if (selectedRow != null)
                {
                    //billsRepo = new AdminBillsRepo();
                    //frmBillAdd.bills = new List<AdminBill>();
                    //frmBillAdd.bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                    frmBillAdd.editFlag = true;
                    frmBillAdd.groupId = selectedRow.transactionGroupId;
                    frmBillAdd.isProgressiveCost = selectedRow.isProgressiveCost;
                    frmBillAdd.billTypes = selectedRow.billTypes;
                    frmBill.Content = frmBillAdd;
                    frmBill.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Existing Bill!");
            }
                
        }


        public void update_bills(int transaction_id)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill") != null)
            {
                frmBillAdd = new ucFrmBillAdd();
                Window frmBill = new Window();
                frmBill.WindowState = WindowState.Maximized;
                frmBill.Title = "Update Bills";

                
                    billsRepo = new AdminBillsRepo();
                    frmBillAdd.bills = new List<AdminBill>();
                    frmBillAdd.bills = billsRepo.GetBillsByGroupId(transaction_id);
                    frmBillAdd.editFlag = true;
                    frmBillAdd.groupId = transaction_id;
                    frmBill.Content = frmBillAdd;
                    frmBill.Show();
                
            }
            else
            {
                DXMessageBox.Show("Permission required to View Existing Bill!");
            }
        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Admin Bills Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblHeading.Text);
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null  && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdBillRegister, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Admin Bills Reports!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            
            ucBillList billList = new ucBillList();
            this.DataContext = billList;
            billsRepo = new AdminBillsRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
            {
                var billsList = billsRepo.GetAllOpenBills(SYSTEM_STATIC.currentUser.id);
                grdBillRegister.ItemsSource = billsList;
            }
            else
            {
                grdBillRegister.ItemsSource = null;
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdBillRegister);
            lblHeading.Text = "Admin Bills(Open)";
            SetColumnsVisibility();
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdBillRegister);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            billsRepo = new AdminBillsRepo();
            var selectedRow = (AdminBill)grdBillRegister.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.BillStatus.Status;
                var bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                var totalAmountOC = bills.Sum(x=>x.AmountOC);
                if(bills != null && bills.Count > 0)
                {
                    if (bills[0].isApproved == false)
                    {
                        DXMessageBox.Show("Bills are pending for approval!");
                        return;
                    }

                    foreach (var _billl in bills)
                    {


                        if (_billl.Payments == null || _billl.Payments.Count == 0)
                        {
                            //DXMessageBox.Show("Bills are not Fully Paid yet!");
                            isFullyPaid = false;
                            break;
                        }
                        else if ((_billl.Payments.Sum(x => x.DebitedAmount) - _billl.AmountOC) != 0)
                        {
                            //DXMessageBox.Show("Bills are not Fully Paid yet!");
                            isFullyPaid = false;
                            break;
                        }

                    }

                    if (isFullyPaid == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bills without Payments") == null)
                    {
                        DXMessageBox.Show("Permission required to close Unpaid Admin Bills!");
                        return;
                    }

                    if (isFullyPaid == false)
                    {
                        MessageBoxResult result = DevExpress.Xpf.Core.DXMessageBox.Show("This Admin Bill is not Paid Yet, Do you want to Close it?", "Bill is Unpaid", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.No)
                        {
                            return;
                        }
                    }

                    statusChanged = null;
                    ucFrmBillDirectClose ucFrmDirectClose = new ucFrmBillDirectClose();
                    if (selectedRow.BillStatus != null)
                    {
                        ucFrmDirectClose.statusName.Text = selectedRow.BillStatus.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(selectedRow.BillStatus.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bill without Approval") != null)
                        {
                            for (int i = 0; i < bills.Count; i++)
                            {
                                bills[i].PendingForClosing = false;
                                bills[i].stage = TransactionStage.Closed.ToString();
                                bills[i].statusId = statusChanged.Id;
                                bills[i].LastStatusChangeDate = System.DateTime.Now;
                                bills[i].ClosingDate = System.DateTime.Now;


                            }
                            billsRepo.ApproveBill(bills);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Admin Bill having system ref #: " + bills[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + totalAmountOC,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (bills.Count != 0)
                                {
                                    procurementRepo.Add(bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {
                            for (int i = 0; i < bills.Count; i++)
                            {
                                bills[i].PendingForClosing = true;
                                bills[i].stage = TransactionStage.AwaitingApproval.ToString();
                                bills[i].statusId = statusChanged.Id;
                                bills[i].LastStatusChangeDate = System.DateTime.Now;
                                bills[i].ClosingDate = System.DateTime.Now;


                            }
                            billsRepo.ApproveBill(bills);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Admin Bill having system ref #: " + bills[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + totalAmountOC,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (bills.Count != 0)
                                {
                                    procurementRepo.Add(bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
        }

        public ucBillList(AdminBillStatus status)
        {
            statusChanged = status;
            //InitializeComponent();
            //receipt_register_win.Closing += ReceiptRegister_Window_Closing;
            //grdSaleReceiptList.Columns["SerialNo"].Visible = false;
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (grdBillRegister.GetFocusedRow() != null)
                {
                    billsRepo = new AdminBillsRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (AdminBill)grdBillRegister.GetFocusedRow();
                    var bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if(bills != null && bills.Count > 0)
                    {
                        if (bills[0].isApproved != true)
                        {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null) ? true : false)
                                {
                                    for (int i = 0; i < bills.Count; i++)
                                    {
                                        bills[i].isApproved = true;
                                        bills[i].stage = TransactionStage.Approved.ToString();
                                    }
                                    //receipt.isApproved = true;

                                    //receipt.stage = TransactionStage.Approved.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, 17, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                billsRepo.ApproveBill(bills);

                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Admin Bills are Approved (" + selectedRow.transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Admin Bill is Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Admin Bill Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Admin Bill Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (bills[0].isReApproved == false)
                        {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Admin Bill") != null) ? true : false)
                                {
                                    for (int i = 0; i < bills.Count; i++)
                                    {
                                        bills[i].isReApproved = true;
                                        bills[i].stage = TransactionStage.Approved.ToString();

                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                billsRepo.ApproveBill(bills);
                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Admin Bills are Approved (" + selectedRow.transactionGroupId + ")");
                                    SystemLog.LogInfo(this.GetType(), "Admin Bill is Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Admin Bill Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Admin Bill Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                    }
                    //Load_Receipts();
                }
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Admin Bill List") != null)
            {
                billsRepo = new AdminBillsRepo();
                lblHeading.Text = "Pending for Approval Admin Bills";
                var billsList = billsRepo.GetAllPendingForApprovalBills(SYSTEM_STATIC.currentUser.id);
                grdBillRegister.ItemsSource = billsList;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Admin Bills!!");
            }
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Admin Bills List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Admin Bills";
                var adminBills = billsRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                grdBillRegister.ItemsSource = adminBills;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Admin Bills!!");
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Admin Bill List") != null)
            {

                var interBankTransfers = billsRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Admin Bills";
                grdBillRegister.ItemsSource = interBankTransfers;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Admin Bills!");
            }
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
            {
                billsRepo = new AdminBillsRepo();
                var billsList = billsRepo.GetAllBills(SYSTEM_STATIC.currentUser.id);
                grdBillRegister.ItemsSource = billsList;
                lblHeading.Text = "Admin Bill Register";
            }
            else
            {
                MessageBox.Show("Permission required to View list of Admin Bills!");
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Admin Bills") != null)
            {
                lblHeading.Text = "Void Admin Bills";
               
                var adminBills = billsRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                grdBillRegister.ItemsSource = adminBills;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Admin Bills!!");
            }
        }

        private void GrdBillsList_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            //var bill=e.Source.CurrentItem as AdminBill ;
            var bill = grdBillRegister.GetRowByListIndex(e.ListSourceRowIndex) as AdminBill;

            if (e.IsGetData)
            {
                switch (e.Column.FieldName)
                {
                    case "Stage":
                        if (bill != null)
                        {
                            if (bill.isVoid == true)
                            {
                                e.Value = "Void";
                            }
                            else if (bill.isReApproved == false)
                            {
                                e.Value = "Under Re-Approval";
                            }
                            else if (bill.isApproved == true && bill.stage == "Closed")
                            {
                                e.Value = "Closed";
                            }
                            else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
                            {
                                e.Value = "Closed";
                            }
                            else if (bill.isApproved == true && bill.PendingForClosing == true)
                            {
                                e.Value = "Under Closing";
                            }
                            else if (bill.isApproved == true)
                            {
                                e.Value = "Approved";
                            }
                            else if (bill.isApproved == false)
                            {
                                e.Value = "Under Approval";
                            }
                            else if (bill.PendingForClosing == true)
                            {
                                e.Value = "Under Closing";
                            }
                        }
                        break;

                    case "Closed":
                        if(bill.Payments.Count > 0 && bill.Payments.Where(x=>x.Status != null && x.Status.isActive == true).Count() == 0)
                        {
                            e.Value = "Closed";
                        }
                        else
                        {
                            e.Value = "Open";
                        }
                        break;
                    case "PayeeName":
                        if (e.GetListSourceFieldValue("payee_Id") != null)
                        {
                            var payee_id = Convert.ToInt32(e.GetListSourceFieldValue("payee_Id"));
                            if (payee_id > 0)
                            {
                                var payee = billsRepo.GetPayeeForBillRegister(payee_id);
                                string payeeName;
                                payeeName = payee.PayeeName;
                                while (payee.ParentId != null)
                                {
                                    payee = billsRepo.GetPayeeForBillRegister((int)payee.ParentId);
                                    payeeName = payee.PayeeName + " > " + payeeName;
                                }
                                e.Value = payeeName;
                            }
                        }
                        break;

                    case "paidAmount":
                        if (bill.Payments.Count != 0)
                        {
                            var pymnts = bill.Payments.Where(x => x.isVoid != true).ToList();
                            e.Value = pymnts.Sum(x => x.DebitedAmount);
                        }
                        break;

                    case "balanceAmount":
                        if (bill.Payments.Count != 0)
                        {
                            var pymntss = bill.Payments.Where(x => x.isVoid != true).ToList();
                            var amountPaid = pymntss.Sum(x => x.DebitedAmount);
                            e.Value = bill.AmountOC - amountPaid;
                        }
                        break;

                    case "percBalanceAmount":
                        var finalPayments = bill.Payments.Where(x => x.isVoid != true).ToList();
                        var paidAmount = finalPayments.Sum(x => x.DebitedAmount);
                        var percent = (((paidAmount) / bill.AmountOC) * 100);
                        e.Value = Math.Round(percent, 2);
                        break;

                    case "BillCreator":
                        if (bill.Creator != null && bill.Creator.person != null)
                        {
                            var userName = bill.Creator.person.FName + " " + bill.Creator.person.LName;
                            e.Value = userName;
                        }
                        break;
                }
            }
        }

        public void GridControlSetUserSettings()
        {
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdBillRegister);
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdBillRegister.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdpurchaseOrderr.View.ShowPrintPreview(this);
        }

        //private void GrdBillsList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        //{
        //    if(e.Column.FieldName == "payee_Id")
        //    {
        //        var payee_id = Convert.ToInt32(e.GetListSourceFieldValue("payee_Id"));
        //    }
        //}
    }
}
