using DevExpress.Xpf.Core;
using ERP_BL.Bankings;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Bankings.Loans.UserControls
{
    /// <summary>
    /// Interaction logic for ucLoansList.xaml
    /// </summary>
    public partial class ucLoansList : UserControl
    {
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        static LoansStatus statusChanged = new LoansStatus();
        LoansRepo loansRepo = new LoansRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucLoansList()
        {
            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Loans List") != null)
                {
                    ApprovalCount = loansRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Loans") != null)
                    {
                        ApprovalCount = loansRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = loansRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Loans List") != null)
                {
                    ReApprovalCount = loansRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Loans") != null)
                    {
                        ReApprovalCount = loansRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = loansRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Loans") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = loansRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = loansRepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Loans") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = loansRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
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
                ApprovalCount = loansRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = loansRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Loans List") != null)
                {
                    ClosingCount = loansRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Loans") != null)
                    {
                        ClosingCount = loansRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = loansRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }

                }
            }


            else
            {
                ClosingCount = loansRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        public ucLoansList(LoansStatus status)
        {
            statusChanged = status;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblHeading.Text = "Loans (Open)";
            grdCntrlLoans.ItemsSource= loansRepo.GetAllLoans(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlLoans);
        }

        private void GrdCntrlLoans_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = grdCntrlLoans.SelectedItem as ERP_BL.Bankings.Loans;

            ucFrmLoans frmLoans = new ucFrmLoans();
            frmLoans.groupId = selectedItem.transactionGroupId;
            frmLoans.editFlag = true;
            DXWindow win = new DXWindow();
            win.Content = frmLoans;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans") != null)
            {
                ucFrmLoans ucFrmLoans = new ucFrmLoans();
                DXWindow win = new DXWindow();

                ucFrmLoans.editFlag = false;
                win.Content = ucFrmLoans;
                win.WindowState = WindowState.Maximized;
                win.MinHeight = 800;
                win.MinWidth = 800;
                win.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Loans!");
                return;
            }
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grdCntrlLoans.SelectedItem as ERP_BL.Bankings.Loans;

            ucFrmLoans frmLoans = new ucFrmLoans();
            frmLoans.groupId = selectedItem.transactionGroupId ;
            frmLoans.editFlag = true;
            DXWindow win = new DXWindow();
            win.Content = frmLoans;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Loans") != null)
            {
                List<ERP_BL.Bankings.Loans> loansList = new List<ERP_BL.Bankings.Loans>();
                lblHeading.Text = "Void Loans";
                if (MainWindow.currentUserid != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Loans") != null)
                    {
                        loansList = loansRepo.getVoidRegister(MainWindow.currentUserid);
                    }

                    else
                    {
                        loansList = loansRepo.getVoidRegisterOwn(MainWindow.currentUserid);
                    }
                }
                else
                {
                    loansList = loansRepo.getVoidRegisterAdministrator();
                }

                //grdsaleInvoice.ItemsSource = saleInvoices;


                grdCntrlLoans.ItemsSource = loansList;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Loans!!");
            }
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlLoans);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ucLoansList loansList = new ucLoansList();
            this.DataContext = loansList;
            loansRepo = new LoansRepo();
            lblHeading.Text = "Loans (Open)";
            grdCntrlLoans.ItemsSource = loansRepo.GetAllLoans(SYSTEM_STATIC.currentUser.id);
        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            var selectedRow = (ERP_BL.Bankings.Loans)grdCntrlLoans.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var loans = loansRepo.GetAllLoansByGroupId(selectedRow.transactionGroupId);
                //var totalAmountOC = bills.Sum(x => x.AmountOC);
                if (loans != null && loans.Count > 0)
                {
                    if (loans[0].isApproved == false)
                    {
                        DXMessageBox.Show("Loans are pending for approval!");
                        return;
                    }

                    
                    statusChanged = null;
                    ucFrmLoanDirectClose ucFrmDirectClose = new ucFrmLoanDirectClose();
                    if (selectedRow.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = selectedRow.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(selectedRow.Status.backcolor);
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Loans without Approval") != null)
                        {
                            for (int i = 0; i < loans.Count; i++)
                            {
                                loans[i].PendingForClosing = false;
                                loans[i].stage = TransactionStage.Closed.ToString();
                                loans[i].statusId = statusChanged.Id;
                                loans[i].LastStatusChangeDate = System.DateTime.Now;
                                loans[i].ClosingDate = System.DateTime.Now;


                            }
                            loansRepo.UpdateLoans(loans);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Loans, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Loans having system ref #: " + loans[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nSystem Ref:" + loans[0].SystemRefNo,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (loans.Count != 0)
                                {
                                    procurementRepo.Add(loans[0].transactionGroupId, TransactionItemType.Loans, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {
                            for (int i = 0; i < loans.Count; i++)
                            {
                                loans[i].PendingForClosing = true;
                                loans[i].stage = TransactionStage.AwaitingApproval.ToString();
                                loans[i].statusId = statusChanged.Id;
                                loans[i].LastStatusChangeDate = System.DateTime.Now;
                                loans[i].ClosingDate = System.DateTime.Now;


                            }
                            loansRepo.UpdateLoans(loans);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Loans, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Loans having system ref #: " + loans[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nSystem Ref:" + loans[0].SystemRefNo,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (loans.Count != 0)
                                {
                                    procurementRepo.Add(loans[0].transactionGroupId, TransactionItemType.Loans, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans #" + loans[0].SystemRefNo, loans[0].transactionGroupId, TransactionItemType.Loans, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (grdCntrlLoans.GetFocusedRow() != null)
                {
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (ERP_BL.Bankings.Loans)grdCntrlLoans.GetFocusedRow();
                    var loans = loansRepo.GetAllLoansByGroupId(selectedRow.transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (loans != null && loans.Count > 0)
                    {
                        if (loans[0].isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Loans") != null) ? true : false)
                            {
                                for (int i = 0; i < loans.Count; i++)
                                {
                                    loans[i].isApproved = true;
                                    loans[i].stage = TransactionStage.Approved.ToString();
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, (int)TransactionItemType.Loans, frmInputBox.comment);

                                loansRepo.UpdateLoans(loans);

                                MessageBox.Show("Loans are Approved (" + selectedRow.transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Loans are Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Loans Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (loans[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Loans") != null) ? true : false)
                            {
                                for (int i = 0; i < loans.Count; i++)
                                {
                                    loans[i].isReApproved = true;
                                    loans[i].stage = TransactionStage.Approved.ToString();

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, (int)TransactionItemType.Loans, frmInputBox.comment);

                                loansRepo.UpdateLoans(loans);

                                MessageBox.Show("Loans are Approved (" + selectedRow.transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Loans are Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Loans Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans Directly user id=(" + MainWindow.currentUserid + ")");
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Loans List") != null)
            {
                var interBankTransfers = loansRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                lblHeading.Text = "(Pending For Approval) Loans";
                grdCntrlLoans.ItemsSource = interBankTransfers;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Loans!!");
            }
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Loans List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Loans";
                var loans = loansRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);
                grdCntrlLoans.ItemsSource = loans;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Loans!");
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Loans List") != null)
            {

                var loans = loansRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                lblHeading.Text = "(Pending for Closing) Loans";
                grdCntrlLoans.ItemsSource = loans;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Loans!");
            }
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Loans") != null)
            {
                List<ERP_BL.Bankings.Loans> loansList = new List<ERP_BL.Bankings.Loans>();
                lblHeading.Text = "Loans Register";
                if (MainWindow.currentUserid != 0)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Loans") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Loans") != null)
                    {
                        loansList = loansRepo.getInterBankTransferRegister(MainWindow.currentUserid);
                    }

                    else
                    {
                        loansList = loansRepo.getInterBankTransferRegister(MainWindow.currentUserid);
                    }
                }

                else
                {
                    loansList = loansRepo.getInterBankTransferRegisterAdministrator();
                }
                grdCntrlLoans.ItemsSource = loansList;
            }
            else
            {
                MessageBox.Show("Permission required to View list of Loans!");
            }
        }
    }
}
