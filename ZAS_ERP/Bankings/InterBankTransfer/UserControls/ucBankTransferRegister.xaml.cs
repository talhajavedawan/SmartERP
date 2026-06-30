using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements;
using ERP_BL.Procurements.InterBankTransfers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Bankings.UserControls
{
    /// <summary>
    /// Interaction logic for ucBankTransferRegister.xaml
    /// </summary>
    public partial class ucBankTransferRegister : UserControl
    {
        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
        static InterBankTransferStatus statusChanged = new InterBankTransferStatus();

        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;
        InterBankTransferStatus oldStatus = new InterBankTransferStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucBankTransferRegister()
        {
            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
                {
                    ApprovalCount = bankTransRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    {
                        ApprovalCount = bankTransRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = bankTransRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                {
                    ReApprovalCount = bankTransRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    {
                        ReApprovalCount = bankTransRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = bankTransRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inter-Bank Transfer") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = bankTransRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = bankTransRepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = bankTransRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
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
                ApprovalCount = bankTransRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = bankTransRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inter-Bank Transfer List") != null)
                {
                    ClosingCount = bankTransRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    {
                        ClosingCount = bankTransRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = bankTransRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }

                }
            }


            else
            {
                ClosingCount = bankTransRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        public ucBankTransferRegister(InterBankTransferStatus status)
        {
            statusChanged = status;
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadInterBankTransferData();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlInterBankTransfer);
        }

        private void LoadInterBankTransferData()
        {
            //RefreshData();
            SetColumnsVisibility();
        }

        public void SetColumnsVisibility()
        {
            //grdInterBankTransfer.Columns["Id"].Visible = false;
            //grdInterBankTransfer.Columns["company"].Visible = false;
            //grdInterBankTransfer.Columns["department"].Visible = false;
            //grdInterBankTransfer.Columns["employee"].Visible = false;
            //grdInterBankTransfer.Columns["currency"].Visible = false;
            //grdInterBankTransfer.Columns["BankFrom"].Visible = false;
            //grdInterBankTransfer.Columns["AccountFrom"].Visible = false;
            //grdInterBankTransfer.Columns["BankTo"].Visible = false;
            //grdInterBankTransfer.Columns["AccountTo"].Visible = false;
            //grdInterBankTransfer.Columns["tranferMethod"].Visible = false;
        }

       


        public void update_interBankTransfer(int transaction_id)
        {
            ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
            CompanyRepo compRepo = new CompanyRepo();
            bankTransRepo = new InterBankTransRepo();

            //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
            var bankTransfer = bankTransRepo.GetInterBankTransfer(transaction_id); //grdInterBankTransfer.SelectedItem as InterBankTransfer;
            ucFrmBankTransfer.bankTransferId = transaction_id;

            if (bankTransfer.interBankTransStatus.isActive == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null )
                {
                    ucFrmBankTransfer.editFlag = true;

                    ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                    ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                    ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                    ucFrmBankTransfer.frmBankTranfer.ShowDialog();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View InActive Inter-Bank Transfer!");
                    return;
                }
            }
            else
            {
                ucFrmBankTransfer.editFlag = true;

                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                ucFrmBankTransfer.frmBankTranfer.ShowDialog();
            }
        }

        
        private void RefreshData()
        {
            ucBankTransferRegister ucBankTransfer = new ucBankTransferRegister();
            this.DataContext = ucBankTransfer;
            lblHeading.Text = "Inter-Bank Transfers(Open)";
            bankTransRepo = new InterBankTransRepo();
            grdCntrlInterBankTransfer.ItemsSource = bankTransRepo.GetAllInterBankTransfers(SYSTEM_STATIC.currentUser.id);
            SetColumnsVisibility();
            //SystemLogic.SetUserSettingOfCurrentWindow(grdCntrlInterBankTransfer);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlInterBankTransfer);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer") != null)
            {
                var selectedBankTransfer = grdCntrlInterBankTransfer.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterBankTransfer;
                UsersRepo usersRepo = new UsersRepo();

                if (selectedBankTransfer != null)
                {
                    bankTransRepo = new InterBankTransRepo();

                    var selectedRow = grdCntrlInterBankTransfer.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterBankTransfer;
                    ERP_BL.Procurements.InterBankTransfers.InterBankTransfer bankTransfer = new ERP_BL.Procurements.InterBankTransfers.InterBankTransfer();
                    bankTransfer = bankTransRepo.GetInterBankTransfer(selectedRow.Id);

                    //List<ProcurementProduct> products = new List<ProcurementProduct>();
                    //foreach (var _prod in bankTransfer.products)
                    //{
                    //    products.Add(_prod);
                    //}

                    if (bankTransfer.isApproved == false)
                    {
                        MessageBox.Show("Transaction should be approved before closing!");
                        return;
                    }
                    var previous_status = bankTransfer.interBankTransStatus.Status;


                    statusChanged = null;
                    ucFrmDirectClose ucFrmDirectClose = new ucFrmDirectClose();

                    if (bankTransfer.interBankTransStatus != null)
                    {
                        ucFrmDirectClose.statusName.Text = bankTransfer.interBankTransStatus.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(bankTransfer.interBankTransStatus.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null)
                        {
                            bankTransfer.PendingForClosing = false;
                            bankTransfer.stage = TransactionStage.Approved.ToString();
                            bankTransfer.statusId = statusChanged.Id;
                            bankTransfer.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer.ClosingDate = System.DateTime.Now;

                            bankTransRepo.approveInterBankTransfer(bankTransfer);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                        {
                            bankTransfer.statusId = statusChanged.Id;
                            bankTransfer.stage = TransactionStage.AwaitingApproval.ToString();
                            bankTransfer.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer.ClosingDate = System.DateTime.Now;
                            if (bankTransfer.PendingForClosing == null)
                            {
                                bankTransfer.PendingForClosing = true;
                            }
                            bankTransRepo.approveInterBankTransfer(bankTransfer);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null)
                        {
                            bankTransfer.statusId = statusChanged.Id;
                            bankTransfer.stage = TransactionStage.AwaitingApproval.ToString();
                            bankTransfer.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer.ClosingDate = System.DateTime.Now;
                            if (bankTransfer.PendingForClosing != true)
                            {
                                bankTransfer.PendingForClosing = true;

                            }
                            bankTransRepo.approveInterBankTransfer(bankTransfer);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                        }
                        else
                        {
                            bankTransfer.statusId = statusChanged.Id;
                            bankTransfer.stage = TransactionStage.AwaitingFirstReview.ToString();
                            bankTransfer.PendingForClosing = true;
                            bankTransfer.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer.ClosingDate = System.DateTime.Now;
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                            bankTransRepo.approveInterBankTransfer(bankTransfer);
                        }
                        //Adding signature (comment)

                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Bank Tranfer has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (selectedRow.department != null && selectedRow.department.Id != 0 && selectedRow.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(selectedRow.department.Id, selectedRow.company.Id), bankTransfer.Id, TransactionItemType.InterBank_Transfer);
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }
                            string oldStat = "";
                            if (oldStatus != null)
                            {
                                oldStat = oldStatus.Status;
                            }
                            string newStat = statusChanged.Status;
                            string symbolCurr = "";
                            if (selectedRow.currency != null)
                            {
                                symbolCurr = selectedRow.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Internal BankTransfer (Amount OC) having value: " + selectedRow.AmountOC.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(selectedRow.Id, TransactionItemType.InterBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);

                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in InterBank Transfer #" + selectedRow.SystemRefNo, selectedRow.Id, TransactionItemType.InterBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in InterBank Transfer #" + selectedRow.SystemRefNo, selectedRow.Id, TransactionItemType.InterBank_Transfer, comment.Comment, user.id, "New Comment ", null);
                                }
                            }

                        }
                        MessageBox.Show("Inter-Bank Transfer status changed to InActive (" + statusChanged.Status + ")");
                    }
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Close Inter-Bank Transfer!");
                return;
            }
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedBankTransfer = grdCntrlInterBankTransfer.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterBankTransfer;
                UsersRepo usersRepo = new UsersRepo();
                if (grdCntrlInterBankTransfer.GetFocusedRow() != null)
                {
                    bankTransRepo = new InterBankTransRepo();

                    var selectedRow = grdCntrlInterBankTransfer.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterBankTransfer;
                    ERP_BL.Procurements.InterBankTransfers.InterBankTransfer bankTransfer = new ERP_BL.Procurements.InterBankTransfers.InterBankTransfer();
                    bankTransfer = bankTransRepo.GetInterBankTransfer(selectedRow.Id);

                    //List<ProcurementProduct> products = new List<ProcurementProduct>();
                    //foreach (var _prod in bankTransfer.products)
                    //{
                    //    products.Add(_prod);
                    //}

                    if (bankTransfer.isApproved != true)
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null) ? true : false)
                            {
                                bankTransfer.isApproved = true;
                                bankTransfer.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, bankTransfer.Id, 13, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                            {
                                if (bankTransfer.isApproved == null)
                                {
                                    bankTransfer.isApproved = false;
                                    bankTransfer.stage = TransactionStage.Approved.ToString();
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, bankTransfer.Id, 13, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter - Bank Transfer") != null)
                            {
                                if (bankTransfer.isApproved == null)
                                {
                                    bankTransfer.isApproved = false;
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, bankTransfer.Id, 11, frmInputBox.comment);
                            }
                            else
                            {
                                bankTransfer.isApproved = false;
                            }
                            bankTransRepo.approveInterBankTransfer(bankTransfer);
                        }
                        else
                        {
                            MessageBox.Show("You are not Allowed to Approve Inter-Bank Transfer Directly");
                            SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Inter-Bank Transfer Directly user id=(" + MainWindow.currentUserid + ")");
                            return;
                        }
                    }
                    else if (bankTransfer.isReApproved == false)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Inter-Bank Transfer") != null) ? true : false)
                            {
                                bankTransfer.isReApproved = true;
                                bankTransfer.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, bankTransfer.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                            {
                                bankTransfer.stage = TransactionStage.Approved.ToString();
                                if (bankTransfer.isReApproved == null)
                                {
                                    bankTransfer.isReApproved = false;
                                }


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, bankTransfer.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null)
                            {
                                bankTransfer.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (bankTransfer.isReApproved == null)
                                {
                                    bankTransfer.isReApproved = false;
                                }


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, bankTransfer.Id, (int)TransactionItemType.InterBank_Transfer, frmInputBox.comment);
                            }
                            else
                            {
                                bankTransfer.isReApproved = false;
                            }



                            bankTransRepo.approveInterBankTransfer(bankTransfer);

                            MessageBox.Show("Transaction is Re-Approved (" + bankTransfer.Id + ")");
                            SystemLog.LogInfo(this.GetType(), "Transaction is Re-Approved (" + bankTransfer.Id + ")");
                        }
                        else
                        {
                            MessageBox.Show("You are not Allowed to Re-Approve Inter-Bank Transfer Directly");
                            SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Inter-Bank Transfer Directly user id=(" + MainWindow.currentUserid + ")");
                            return;
                        }


                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }




        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
            {
                var interBankTransfers = bankTransRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                lblHeading.Text = "(Pending For Approval) Inter-Bank Transfers";
                grdCntrlInterBankTransfer.ItemsSource = interBankTransfers;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Loans!");
            }
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
            {
                //_usersRepo usersRepo = new _usersRepo();
                //List<InterBankTransfer> inte = new List<InterBankTransfer>();
                lblHeading.Text = "Pending for ReApprovals Inter-Bank Transfers";
                //if (MainWindow.currentUserid != 0)
                //{
                //    if (SystemLogic.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                //    {
                var interBankTransfers = bankTransRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                //}
                //else
                //{
                //    if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                //    {
                //        saleReceipts = bankTransRepo.getAllPendingForReApproval(MainWindow.currentUserid);
                //    }

                //    else
                //    {
                //        saleReceipts = bankTransRepo.getAllPendingForReApprovalOwn(MainWindow.currentUserid);

                //    }
                //}
                //}

                //else
                //{
                //saleReceipts = bankTransRepo.getAllPendingForAdministrator();
                //}

                //GetAllSaleReceipts recepts = new GetAllSaleReceipts(saleReceipts);
                grdCntrlInterBankTransfer.ItemsSource = interBankTransfers;

            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Inter-Bank Transfers!!");
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inter-Bank Transfer List") != null)
            {

                var interBankTransfers = bankTransRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                lblHeading.Text = "(Pending for Closing) Inter-Bank Tranfers";
                grdCntrlInterBankTransfer.ItemsSource = interBankTransfers;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Inter-Bank Transfer!");
            }
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
            {
                UsersRepo usersRepo = new UsersRepo();

                List<ERP_BL.Procurements.InterBankTransfers.InterBankTransfer> bankTransList = new List<ERP_BL.Procurements.InterBankTransfers.InterBankTransfer>();
                lblHeading.Text = "Inter-Bank Transfer Register";
                if (MainWindow.currentUserid != 0)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    {
                        bankTransList = bankTransRepo.getInterBankTransferRegister(MainWindow.currentUserid);
                    }

                    else
                    {
                        bankTransList = bankTransRepo.getInterBankTransferRegister(MainWindow.currentUserid);
                    }
                }

                else
                {
                    bankTransList = bankTransRepo.getInterBankTransferRegisterAdministrator();
                }
                grdCntrlInterBankTransfer.ItemsSource = bankTransList;
            }
            else
            {
                MessageBox.Show("Permission required to View list of Inter-Bank Transfer!");
            }

        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inter-Bank Transfer") != null)
            {
                List<ERP_BL.Procurements.InterBankTransfers.InterBankTransfer> bankTransList = new List<ERP_BL.Procurements.InterBankTransfers.InterBankTransfer>();
                lblHeading.Text = "Void Inter-Bank Transfer";
                if (MainWindow.currentUserid != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inter-Bank Transfer") != null)
                    {
                        bankTransList = bankTransRepo.getVoidRegister(MainWindow.currentUserid);

                    }

                    else
                    {
                        bankTransList = bankTransRepo.getVoidRegisterOwn(MainWindow.currentUserid);
                        //saleInvoices = saleInvoicerepo.getVoidRegisterOwn(MainWindow.currentUserid);
                    }
                }
                else
                {
                    bankTransList = bankTransRepo.getVoidRegisterAdministrator();
                }

                //grdsaleInvoice.ItemsSource = saleInvoices;


                grdCntrlInterBankTransfer.ItemsSource = bankTransList;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Sale Receipts!!");
            }
        }

        private void GrdInterBankTransfer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                {

                    var selectedBankTransfer = grdCntrlInterBankTransfer.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterBankTransfer;

                    if (selectedBankTransfer != null)
                    {
                        ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                        CompanyRepo compRepo = new CompanyRepo();
                        bankTransRepo = new InterBankTransRepo();

                        //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                        ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                        if (selectedBankTransfer.interBankTransStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                            {
                                ucFrmBankTransfer.editFlag = true;

                                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                                ucFrmBankTransfer.frmBankTranfer.Show();
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                return;
                            }
                        }
                        else
                        {
                            ucFrmBankTransfer.editFlag = true;

                            ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                            //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                            //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                            ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                            ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                            //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                            //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                            ucFrmBankTransfer.frmBankTranfer.Show();
                        }


                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Inter-Bank Transfers Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblHeading.Text);
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdCntrlInterBankTransfer, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Inter-Bank Transfer Reports!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();

                ucFrmBankTransfer.editFlag = false;
                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                ucFrmBankTransfer.frmBankTranfer.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfer!");
                return;
            }
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                {

                    var selectedBankTransfer = grdCntrlInterBankTransfer.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterBankTransfer;

                    if (selectedBankTransfer != null)
                    {
                        ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                        CompanyRepo compRepo = new CompanyRepo();
                        bankTransRepo = new InterBankTransRepo();

                        //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                        ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                        if (selectedBankTransfer.interBankTransStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null )
                            {
                                ucFrmBankTransfer.editFlag = true;

                                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                ucFrmBankTransfer.frmBankTranfer.Show();
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                return;
                            }
                        }
                        else
                        {
                            ucFrmBankTransfer.editFlag = true;

                            ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                            //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                            //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                            ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                            //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                            //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                            ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                            ucFrmBankTransfer.frmBankTranfer.Show();
                        }


                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GrdCntrlInterBankTransfer_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdCntrlInterBankTransfer.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Procurements.InterBankTransfers.InterBankTransfer;
            switch (e.Column.FieldName)
            {
                case "Open":
                    var transferStatus = e.GetListSourceFieldValue("interBankTransStatus") as InterBankTransferStatus;
                    var flag = transferStatus.isActive;

                    if (flag == true)
                        e.Value = "Yes";
                    else
                        e.Value = "No";
                    break;
                case "BankChargesFrom":
                    if (row.bankChargesFrom != null)
                        e.Value = row.bankChargesFrom.Sum(x => x.Amount);
                    break;
                case "VATFrom":
                    if (row.InterBankTransferVATfrom != null)
                        e.Value = row.InterBankTransferVATfrom.Sum(x => x.Amount);
                    break;
                case "BankChargesTo":
                    if (row.bankChargesTo != null)
                        e.Value = row.bankChargesTo.Sum(x => x.Amount);
                    break;
                case "VATTo":
                    if (row.InterBankTransferVATto != null)
                        e.Value = row.InterBankTransferVATto.Sum(x => x.Amount);
                    break;
            }
        }
    }
}
