using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.InterBankTransfers;
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
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.Procurementss;

namespace ZAS_ERP.Bankings.InterCompanyBankTransfer
{
    /// <summary>
    /// Interaction logic for ucInterCompBankTransRegister.xaml
    /// </summary>
    public partial class ucInterCompBankTransRegister : UserControl
    {

        

        InterCompanyBankTransferRepo transferRepo = new InterCompanyBankTransferRepo();
        static InterBankTransferStatus statusChanged = new InterBankTransferStatus();
        InterBankTransferStatus oldStatus = new InterBankTransferStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();



        ERP_BL.Procurements.InterBankTransfers.InterBankTransfer bankTransferRepo = new ERP_BL.Procurements.InterBankTransfers.InterBankTransfer();


        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        public ucInterCompBankTransRegister()
        {
            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
                {
                    ApprovalCount = transferRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    {
                        ApprovalCount = transferRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = transferRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                {
                    ReApprovalCount = transferRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    {
                        ReApprovalCount = transferRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = transferRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inter-Bank Transfer") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = transferRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = transferRepo.getVoidRegisterAdministratorCount();

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
                    ApproveunapprovedCount = transferRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
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
                ApprovalCount = transferRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = transferRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inter-Bank Transfer List") != null)
                {
                    ClosingCount = transferRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    {
                        ClosingCount = transferRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = transferRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }

                }
            }


            else
            {
                ClosingCount = transferRepo.getAllPendingForClosingAdministratorCount();
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
            
            //loadInterCompanyBankData();
            //var transactionList = transferRepo.getAllActiveandTransactions(SystemLogic.currentUser.id);
            //grdCntrlInterCompanyBankTransfers.ItemsSource = transactionList;
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlInterCompanyBankTransfers);
        }
        public void loadInterCompanyBankData()
        {

        }

        public ucInterCompBankTransRegister(InterBankTransferStatus status) 
        {
            statusChanged = status;
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {

            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Company-Bank Transfer") != null)
            {

                ucBankTransferInterCompany ucBankTransfer = new ucBankTransferInterCompany();

                ucBankTransfer.editFlag = false;
                ucBankTransfer.frmBankTranfer.Content = ucBankTransfer;
                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                ucBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                
                ucBankTransfer.frmBankTranfer.MinWidth = 500;
                ucBankTransfer.frmBankTranfer.Title = "Inter-Company-Bank Transfer";
                ucBankTransfer.frmBankTranfer.ShowDialog();
            }
            //else
            //{
            //    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfer!");
            //    return;
            //}

        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                {
                    var selectedBankTransfer = grdCntrlInterCompanyBankTransfers.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer;
                    if (selectedBankTransfer != null)
                    {
                        ucBankTransferInterCompany ucBankTransfer = new ucBankTransferInterCompany();
                        CompanyRepo compRepo = new CompanyRepo();
                        transferRepo = new InterCompanyBankTransferRepo();
                        {
                            ucBankTransfer.editFlag = true;
                            ucBankTransfer.bankTransferId = selectedBankTransfer.Id;
                            ucBankTransfer.frmBankTranfer.Content = ucBankTransfer;
                            ucBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;

                            ucBankTransfer.frmBankTranfer.Title = "Inter-Company-Bank Transfer";
                            ucBankTransfer.frmBankTranfer.Show();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshData();
        }
        private void RefreshData()
        {
            ucInterCompBankTransRegister ucInterComp = new ucInterCompBankTransRegister();

            this.DataContext = ucInterComp;
            lblHeading.Text = "Inter-Bank Transfers(Open)";
            transferRepo = new InterCompanyBankTransferRepo();
            grdCntrlInterCompanyBankTransfers.ItemsSource = transferRepo.getAllActiveandTransactions(SYSTEM_STATIC.currentUser.id);
            SetColumnsVisibility();
        }
        public void SetColumnsVisibility()
        {

        }

        public void update_interCompanyBankTransfer(int transaction_id)
        {
            ucBankTransferInterCompany ucBankTransfer = new ucBankTransferInterCompany();
            CompanyRepo compRepo = new CompanyRepo();
            transferRepo = new InterCompanyBankTransferRepo();

            var bankTransfer = transferRepo.GetInterCompanyBankTransfer(transaction_id);

            if (bankTransfer.interBankTransStatus.isActive == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                {
                    ucBankTransfer.editFlag = true;
                    ucBankTransfer.bankTransferId = transaction_id;
                    ucBankTransfer.frmBankTranfer.Content = ucBankTransfer;
                    ucBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;

                    ucBankTransfer.frmBankTranfer.Title = "Inter-Company-Bank Transfer";
                    ucBankTransfer.frmBankTranfer.ShowDialog();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View InActive Inter-Bank Transfer!");
                    return;
                }
            }
            else
            {
                ucBankTransfer.editFlag = true;
                ucBankTransfer.frmBankTranfer.Content = ucBankTransfer;
                ucBankTransfer.bankTransferId = transaction_id;
                ucBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                ucBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                ucBankTransfer.frmBankTranfer.ShowDialog();
            }


        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {


        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlInterCompanyBankTransfers);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer") != null)
            {
                var selectedBankTransfer = grdCntrlInterCompanyBankTransfers.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer;
                UsersRepo usersRepo = new UsersRepo();

                if (selectedBankTransfer != null)
                {
                    transferRepo = new InterCompanyBankTransferRepo();
                   // bankTransRepo = new InterBankTransRepo();

                    var selectedRow = grdCntrlInterCompanyBankTransfers.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer;

                    ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer bankTransfer = new ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer();
                     
                    bankTransfer = transferRepo.GetInterCompanyBankTransfer(selectedRow.Id);

                    List<ProcurementProduct> products = new List<ProcurementProduct>();
                    foreach (var _prod in bankTransfer.products)
                    {
                        products.Add(_prod);
                    }

                    if (bankTransfer.isApproved == false)
                    {
                        MessageBox.Show("Transaction should be approved before closing!");
                        return;
                    }
                    var previous_status = bankTransfer.interBankTransStatus.Status;


                    statusChanged = null;
                    ucFrmInterCompanyDirectClose ucFrmDirectClose = new ucFrmInterCompanyDirectClose();

                    if (bankTransfer.interBankTransStatus != null)
                    {
                        ucFrmDirectClose.statusName.Text = bankTransfer.interBankTransStatus.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(bankTransfer.interBankTransStatus.backcolor);
                    }

                    ucFrmDirectClose.IBTflag = false;

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
                            bankTransfer.interBankTransStatus = statusChanged;
                            bankTransfer.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer.ClosingDate = System.DateTime.Now;

                            transferRepo.approveInterBankTransfer(bankTransfer, products);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                        {
                            bankTransfer.interBankTransStatus = statusChanged;
                            bankTransfer.stage = TransactionStage.AwaitingApproval.ToString();
                            bankTransfer.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer.ClosingDate = System.DateTime.Now;
                            if (bankTransfer.PendingForClosing == null)
                            {
                                bankTransfer.PendingForClosing = true;
                            }
                            transferRepo.approveInterBankTransfer(bankTransfer, products);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null)
                        {
                            bankTransfer.interBankTransStatus = statusChanged;
                            bankTransfer.stage = TransactionStage.AwaitingApproval.ToString();
                            bankTransfer.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer.ClosingDate = System.DateTime.Now;
                            if (bankTransfer.PendingForClosing != true)
                            {
                                bankTransfer.PendingForClosing = true;

                            }
                            transferRepo.approveInterBankTransfer(bankTransfer, products);

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
                        }
                        else
                        {
                            bankTransfer.interBankTransStatus = statusChanged;
                            bankTransfer.stage = TransactionStage.AwaitingFirstReview.ToString();
                            bankTransfer.PendingForClosing = true;
                            bankTransfer.LastStatusChangeDate = System.DateTime.Now;
                            bankTransfer.ClosingDate = System.DateTime.Now;
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
                            transferRepo.approveInterBankTransfer(bankTransfer, products);
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
                            if (selectedRow.departmentFrom != null && selectedRow.departmentFrom.Id != 0 && selectedRow.companyFrom?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(selectedRow.departmentFrom.Id, selectedRow.companyFrom.Id), bankTransfer.Id, TransactionItemType.InterCompanyBank_Transfer);
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
                            procurementRepo.Add(selectedRow.Id, TransactionItemType.InterCompanyBank_Transfer, comment, SYSTEM_STATIC.currentUser.employeeId);

                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in InterBank Transfer #" + selectedRow.SystemRefNo, selectedRow.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in InterBank Transfer #" + selectedRow.SystemRefNo, selectedRow.Id, TransactionItemType.InterCompanyBank_Transfer, comment.Comment, user.id, "New Comment ", null);
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
                var selectedBankTransfer = grdCntrlInterCompanyBankTransfers.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer;
                UsersRepo usersRepo = new UsersRepo();
                if (grdCntrlInterCompanyBankTransfers.GetFocusedRow() != null)
                {
                    transferRepo = new InterCompanyBankTransferRepo();

                    var selectedRow = grdCntrlInterCompanyBankTransfers.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer;
                    ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer bankTransfer = new ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer();

                   // InterBankTransfer bankTransfer = new InterBankTransfer();
                    bankTransfer = transferRepo.GetInterCompanyBankTransfer(selectedRow.Id);

                    List<ProcurementProduct> products = new List<ProcurementProduct>();
                    foreach (var _prod in bankTransfer.products)
                    {
                        products.Add(_prod);
                    }

                    if (bankTransfer.isApproved != true)
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null) ? true : false)
                            {
                                bankTransfer.isApproved = true;
                                bankTransfer.stage = TransactionStage.Approved.ToString();
                                    bankTransfer.ApprovedDate = DateTime.Now;

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, bankTransfer.Id, 18, frmInputBox.comment);
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
                                usersRepo.Add(TransactionInfo.Reviewed, bankTransfer.Id, 18, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter - Bank Transfer") != null)
                            {
                                if (bankTransfer.isApproved == null)
                                {
                                    bankTransfer.isApproved = false;
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, bankTransfer.Id, 18, frmInputBox.comment);
                            }
                            else
                            {
                                bankTransfer.isApproved = false;
                            }
                            transferRepo.approveInterBankTransfer(bankTransfer, products);
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
                                bankTransfer.ApprovedDate = DateTime.Now;

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, bankTransfer.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
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
                                usersRepo.Add(TransactionInfo.Reviewed, bankTransfer.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
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
                                usersRepo.Add(TransactionInfo.Reviewed, bankTransfer.Id, (int)TransactionItemType.InterCompanyBank_Transfer, frmInputBox.comment);
                            }
                            else
                            {
                                bankTransfer.isReApproved = false;
                            }
                            transferRepo.approveInterBankTransfer(bankTransfer, products);

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

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
            {
                UsersRepo usersRepo = new UsersRepo();

                List<ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer> bankTransList = new List<ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer>();
                lblHeading.Text = "Inter-Bank Transfer Register";
                if (MainWindow.currentUserid != 0)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    {
                        bankTransList = transferRepo.getInterBankTransferRegister(MainWindow.currentUserid);
                    }

                    else
                    {
                        bankTransList = transferRepo.getInterBankTransferRegister(MainWindow.currentUserid);
                    }
                }

                else
                {
                    bankTransList = transferRepo.getInterBankTransferRegister(MainWindow.currentUserid);
                }
                grdCntrlInterCompanyBankTransfers.ItemsSource = bankTransList;
            }
            else
            {
                MessageBox.Show("Permission required to View list of Inter-Bank Transfer!");
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inter-Bank Transfer List") != null)
            {

                var interBankTransfers = transferRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                lblHeading.Text = "(Pending for Closing) Inter-Bank Tranfers";
                grdCntrlInterCompanyBankTransfers.ItemsSource = interBankTransfers;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Inter-Bank Transfer!");
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
            {
                var interBankTransfers = transferRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                lblHeading.Text = "(Pending For Approval) Inter-Bank Transfers";
                grdCntrlInterCompanyBankTransfers.ItemsSource = interBankTransfers;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Sale Receipts!!");
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inter-Bank Transfer") != null)
            {
                List<ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer> bankTransList = new List<ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer>();
                lblHeading.Text = "Void Inter-Bank Transfer";
                if (MainWindow.currentUserid != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Inter-Bank Transfer") != null)
                    {
                        bankTransList = transferRepo.getVoidRegister(MainWindow.currentUserid);
                    }

                    else
                    {
                        bankTransList = transferRepo.getVoidRegister(MainWindow.currentUserid);
                    }
                }
                else
                {
                    bankTransList = transferRepo.getVoidRegister(MainWindow.currentUserid);
                }

                grdCntrlInterCompanyBankTransfers.ItemsSource = bankTransList;
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
                    var selectedBankTransfer = grdCntrlInterCompanyBankTransfers.SelectedItem as ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer;

                    if (selectedBankTransfer != null)
                    {
                        ucBankTransferInterCompany ucFrmBankTransfer = new ucBankTransferInterCompany();
                        CompanyRepo compRepo = new CompanyRepo();
                        transferRepo = new InterCompanyBankTransferRepo();

                        if (selectedBankTransfer.interBankTransStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                            {
                                ucFrmBankTransfer.editFlag = true;
                                ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id;
                                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
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
                            ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id;
                            ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
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

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Inter-Bank Transfers";
                var interBankTransfers = transferRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                grdCntrlInterCompanyBankTransfers.ItemsSource = interBankTransfers;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Inter-Bank Transfers!!");
            }
        }
    }

}
