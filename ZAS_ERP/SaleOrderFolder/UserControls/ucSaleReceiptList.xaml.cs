using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Reportss;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.RentalReceipts;
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucSaleReceiptList.xaml
    /// </summary>
    public partial class ucSaleReceiptList : UserControl
    {
        public int receiptListFlag = 0;
        public UcListWindow receipt_register_win = new UcListWindow();
        SalesReceiptRepo repo = new SalesReceiptRepo();
       
        static SalesReceiptStatus statusChanged = new SalesReceiptStatus();
        List<SalesReceipt> salesReceipts = new List<SalesReceipt>();
        int banktransactionFlag = 0;
        public int ApprovalCount { get; set; }
        public int ReApprovalCount { get; set; }
        public int VoidCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }
        public int statusId=0;
        public int AllActive=0;
        public int treeStatusId =0;
        public string labelHeader { get; set; }

        public ucSaleReceiptList()
        {
            InitializeComponent();
            receipt_register_win.Closing += ReceiptRegister_Window_Closing;
        }
        public ucSaleReceiptList(SalesReceiptStatus status)
        {
            statusChanged = status;
          
        }
        private void ReceiptRegister_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            receiptListFlag = 0;
            e.Cancel = false;
        }
        private void GrdSaleReceiptList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateReceipt();
        }
        public void Update_SaleReceipt(int transactionId)
        {
            try
            {
                CurrencyRepo currencyRepo = new CurrencyRepo();
                CompanyRepo compRepo = new CompanyRepo();

                DepartmentRepo deptRepo = new DepartmentRepo();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                {
                    ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                    updateSaleReceiptObj.saveEditFlag = 1;
                    var saleReceiptListByGroupId = repo.getReceiptsByGroupId(transactionId);
                    var saleReceipt = repo.GetSalesReceipt(saleReceiptListByGroupId[0].Id);
                    if (saleReceipt == null)
                    {
                        return;
                    }
                    if (saleReceipt.saleReceiptStatus != null)
                    {
                        var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                        updateSaleReceiptObj.selectedStatus = status;

                        if (status.isActive == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") == null)
                        {
                            DXMessageBox.Show("Permission required to Edit or View Closed Receipts!");
                            return;
                        }
                    }

                   
                    updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
                    updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                    updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                    updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                    updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                    updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                    updateSaleReceiptObj.receiptId = saleReceipt.Id;

                    if (saleReceipt.CreditedDate != null)
                        updateSaleReceiptObj.dateEditcreditedDate.EditValue = (DateTime)saleReceipt.CreditedDate;

                    if (saleReceipt.DepositedDate != null)
                        updateSaleReceiptObj.datDepositedDate.EditValue = (DateTime)saleReceipt.DepositedDate;

                    if (saleReceipt.InstrumentDate != null)
                        updateSaleReceiptObj.datInstrumentDate.EditValue = (DateTime)saleReceipt.InstrumentDate;

                    if (saleReceipt.InstrumentNo != null)
                        updateSaleReceiptObj.txtInstrumentNo.Text = saleReceipt.InstrumentNo;

                   
                    updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
                    updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                    updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                    updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                    updateSaleReceiptObj.enterReceiptWindowFlag = true;

                    updateSaleReceiptObj.enter_receipt_win.Show();
                    //Load_Receipts();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                    return;
                }
            }
            catch
            {

            }

        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdSaleReceiptList);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            dateFrom.EditValue = DateTime.Now.AddMonths(-2);
            dateTo.EditValue = DateTime.Now;
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdSaleReceiptList);
            this.DataContext = this;
            LoadReceiptsCounts();
            Load_Receipts();
       
        }
        public void LoadReceiptsCounts()
        {
            repo = new SalesReceiptRepo();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleReceipt List") != null)
                {
                    ApprovalCount = repo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                    {
                        ApprovalCount = repo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = repo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleReceipts List") != null)
                {
                    ReApprovalCount = repo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                    {
                        ReApprovalCount = repo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = repo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleReceipts") != null)
                {
                    //mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = repo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = repo.getVoidRegisterAdministratorCount();
                    }
                }
                else
                {
                    //mbtnVoid.Visibility = Visibility.Collapsed;
                }


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Sale Receipts") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                    {
                        ApproveunapprovedCount = repo.getReceiptRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApproveunapprovedCount = repo.getReceiptRegisterCountOWn(MainWindow.currentUserid);
                    }
                }
                else
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                ApprovalCount = repo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = repo.getReceiptRegisterAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleReceipt List") != null)
                {
                    ClosingCount = repo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                    {
                        ClosingCount = repo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = repo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }

            else
            {
                ClosingCount = repo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            LoadOrdersByDate();
            LoadReceiptsCounts();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSaleReceiptList);
          
        }

        public void Load_Receipts()
        {
            repo = new SalesReceiptRepo();
            salesReceipts = new List<SalesReceipt>();
            salesReceipts = repo.GetAllSalesReceipFirst(MainWindow.currentUserid);
            grdSaleReceiptList.ItemsSource =salesReceipts;
            lblHeading.Text = "Sale Receipts(Open)";
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSaleReceiptList);
        }


        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            banktransactionFlag = 0;
            try
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();
                NotificationsRepo notificationsRepo = new NotificationsRepo();


                SalesReceipt SR = new SalesReceipt();
                UsersRepo usersRepo = new UsersRepo();
                var selectedRow = grdSaleReceiptList.SelectedItem as SalesReceipt;
                var Id = selectedRow.Id;
                if (Id != 0)
                {
                    SR = repo.GetSalesReceipt(Id);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null) ? true : false)
                {
                    if (selectedRow != null)
                    {
                        var receipt = repo.GetSalesReceipt(Id);
                        var previous_status = receipt.saleReceiptStatus.Status;
                        var receiptList = repo.getReceiptsByGroupId(receipt.transactionGroupId);
                        var totalSum = receiptList.Sum(x => x.CollectionAmount);

                        if (receiptList.Count > 0 && receiptList[0].isApproved == false)
                        {
                            if (MessageBox.Show("Sale Receipt is Under Approval, Do you want to Approve?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null))
                                {
                                    receiptList.ForEach(z => z.isApproved = true);
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission required to Approve the Sale Receipt!");
                                    return;
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Sale Receipt cannot be Closed without Approval!", "Alert", MessageBoxButton.OK, MessageBoxImage.Hand);
                                return;
                            }
                        }
                        else if (receiptList.Count > 0 && receiptList[0].isReApproved == false)
                        {
                            if (MessageBox.Show("Sale Receipt is Under ReApproval, Do you want to ReApprove?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question) == MessageBoxResult.Yes)
                            {
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null))
                                {
                                    receiptList.ForEach(z => z.isReApproved = true);
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission required to ReApprove the Sale Receipt!");
                                    return;
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Sale Receipt cannot be Closed without Approval!", "Alert", MessageBoxButton.OK, MessageBoxImage.Hand);
                                return;
                            }
                        }


                        statusChanged = null;
                        ucFrmDirectClose ucFrmDirectClose = new ucFrmDirectClose();
                        if (receipt.saleReceiptStatus != null)
                        {
                            ucFrmDirectClose.statusName.Text = receipt.saleReceiptStatus.Status;

                            var brush = new BrushConverter();
                            ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(receipt.saleReceiptStatus.backcolor);
                        }

                        ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                        ucFrmDirectClose.directCloseWin.Width = 450;
                        ucFrmDirectClose.directCloseWin.Height = 650;
                        ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                        ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        ucFrmDirectClose.directCloseWin.ShowDialog();
                        double totalValue = 0;
                        if (statusChanged.Id != 0)
                        {
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Sale Receipt has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (SR.department != null && SR.department.Id != 0 && SR.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                {
                                    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(SR.department.Id, SR.company.Id), salesReceipts[0].transactionGroupId, TransactionItemType.Sale_Receipt);
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
                            }
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null)
                            {

                                foreach (var _receipt in receiptList)
                                {
                                    _receipt.PendingForClosing = false;
                                    _receipt.stage = TransactionStage.Closed.ToString();
                                    _receipt.saleReceiptStatus = statusChanged;
                                    _receipt.LastStatusChangeDate = System.DateTime.Now;
                                    _receipt.ClosingDate = System.DateTime.Now;
                                    totalValue = totalValue + _receipt.CollectionAmount;
                                    //List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                                    //if (banktransactionFlag == 0)
                                    //{
                                    //    var receiptAccount = repo.GetAccount(_receipt.AccountId);
                                    //    if (receiptAccount.COA_accountId != null)
                                    //    {
                                    //        JournalTransaction bankTransaction = new JournalTransaction()
                                    //        {
                                    //            accountId = receiptAccount.COA_accountId,
                                    //            coaTransactionsType = coaTransactionsType.SaleReceipt,
                                    //            creationDate = _receipt.CreationDate,
                                    //            debit = Convert.ToDouble(totalSum),
                                    //            credit = 0,
                                    //            //MER = 1,
                                    //            userId = _receipt.user_Id,
                                    //            SaleReceiptId = _receipt.Id,
                                    //            transactionRefno = _receipt.ReceiptRefNo,
                                    //            deptId = _receipt.department.Id,
                                    //        };
                                    //        journalTransactions.Add(bankTransaction);
                                    //        banktransactionFlag = 1;
                                    //    }
                                    //}
                                    //if (_receipt.receiptDeductions.Count != 0)
                                    //{
                                    //    foreach (var receiptDeduction in _receipt.receiptDeductions)
                                    //    {
                                    //        if (receiptDeduction.Amount != 0)
                                    //        {
                                    //            if (receiptDeduction.deduction.chartofAccountId != null)
                                    //            {
                                    //                JournalTransaction deductionTransaction = new JournalTransaction();

                                    //                deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                    //                deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //                deductionTransaction.creationDate = _receipt.CreationDate;
                                    //                deductionTransaction.debit = receiptDeduction.Amount;
                                    //                deductionTransaction.credit = 0;
                                    //                //deductionTransaction.MER = 1;
                                    //                //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                    //                deductionTransaction.userId = _receipt.user_Id;
                                    //                deductionTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //                deductionTransaction.SaleReceiptId = _receipt.Id;
                                    //                deductionTransaction.deptId = _receipt.department.Id;
                                    //                journalTransactions.Add(deductionTransaction);
                                    //            }
                                    //        }
                                    //    }

                                    //}
                                    //if (_receipt.department.chartofAccountId != null)
                                    //{
                                    //    JournalTransaction receivableTransaction = new JournalTransaction();
                                    //    receivableTransaction.accountId = _receipt.department.chartofAccountId;
                                    //    receivableTransaction.deptId = _receipt.department.Id;

                                    //    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //    receivableTransaction.creationDate = _receipt.CreationDate;
                                    //    receivableTransaction.debit = 0;
                                    //    receivableTransaction.credit = Math.Round(receipt.CollectionAmount, 2);
                                    //    //receivableTransaction.MER = 1;
                                    //    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    //    receivableTransaction.userId = _receipt.user_Id;
                                    //    receivableTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //    receivableTransaction.SaleReceiptId = _receipt.Id;
                                    //    receivableTransaction.deptId = _receipt.department.Id;
                                    //    journalTransactions.Add(receivableTransaction);
                                    //}
                                    //_receipt.journalTransactions = journalTransactions;
                                    repo.updateSalesReceiptForDirectClose(_receipt);
                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);

                                if (previous_status != statusChanged.Status)
                                {
                                    //Adding signature (comment)



                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Receipt having Collection Ammount: " + totalValue.ToString() + " (" + receipt.Currency.Abbrivation.ToString() + ")" + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (receiptList.Count != 0)
                                    {
                                        procurementRepo.Add(receiptList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }
                                //Load_Receipts();
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                            {
                                totalValue = 0;
                                foreach (var _receipt in receiptList)
                                {
                                    _receipt.saleReceiptStatus = statusChanged;
                                    _receipt.stage = TransactionStage.AwaitingApproval.ToString();
                                    _receipt.LastStatusChangeDate = System.DateTime.Now;
                                    _receipt.ClosingDate = System.DateTime.Now;
                                    if (_receipt.PendingForClosing == null)
                                    {
                                        _receipt.PendingForClosing = true;
                                    }
                                    totalValue = totalValue + _receipt.CollectionAmount;

                                    
                                    //List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                                    //if (banktransactionFlag == 0)
                                    //{
                                    //    var receiptAccount = repo.GetAccount(_receipt.AccountId);
                                    //    if (receiptAccount.COA_accountId != null)
                                    //    {
                                    //        JournalTransaction bankTransaction = new JournalTransaction()
                                    //        {
                                    //            accountId = receiptAccount.COA_accountId,
                                    //            coaTransactionsType = coaTransactionsType.SaleReceipt,
                                    //            creationDate = _receipt.CreationDate,
                                    //            debit = Convert.ToDouble(totalSum),
                                    //            credit = 0,
                                    //            //MER = 1,
                                    //            userId = _receipt.user_Id,
                                    //            SaleReceiptId = _receipt.Id,
                                    //            transactionRefno = _receipt.ReceiptRefNo,
                                    //            deptId = _receipt.department.Id,
                                    //        };
                                    //        journalTransactions.Add(bankTransaction);
                                    //        banktransactionFlag = 1;
                                    //    }
                                    //}
                                    //if (_receipt.receiptDeductions.Count != 0)
                                    //{
                                    //    foreach (var receiptDeduction in _receipt.receiptDeductions)
                                    //    {
                                    //        if (receiptDeduction.Amount != 0)
                                    //        {
                                    //            if (receiptDeduction.deduction.chartofAccountId != null)
                                    //            {
                                    //                JournalTransaction deductionTransaction = new JournalTransaction();

                                    //                deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                    //                deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //                deductionTransaction.creationDate = _receipt.CreationDate;
                                    //                deductionTransaction.debit = receiptDeduction.Amount;
                                    //                deductionTransaction.credit = 0;
                                    //                //deductionTransaction.MER = 1;
                                    //                //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                    //                deductionTransaction.userId = _receipt.user_Id;
                                    //                deductionTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //                deductionTransaction.SaleReceiptId = _receipt.Id;
                                    //                deductionTransaction.deptId = _receipt.department.Id;
                                    //                journalTransactions.Add(deductionTransaction);
                                    //            }
                                    //        }
                                    //    }

                                    //}
                                    //if (_receipt.department.chartofAccountId != null)
                                    //{
                                    //    JournalTransaction receivableTransaction = new JournalTransaction();
                                    //    receivableTransaction.accountId = _receipt.department.chartofAccountId;
                                    //    receivableTransaction.deptId = _receipt.department.Id;

                                    //    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //    receivableTransaction.creationDate = _receipt.CreationDate;
                                    //    receivableTransaction.debit = 0;
                                    //    receivableTransaction.credit = Math.Round(receipt.CollectionAmount, 2);
                                    //    //receivableTransaction.MER = 1;
                                    //    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    //    receivableTransaction.userId = _receipt.user_Id;
                                    //    receivableTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //    receivableTransaction.SaleReceiptId = _receipt.Id;
                                    //    receivableTransaction.deptId = _receipt.department.Id;
                                    //    journalTransactions.Add(receivableTransaction);
                                    //}
                                    //_receipt.journalTransactions = journalTransactions;
                                    repo.updateSalesReceiptForDirectClose(_receipt);
                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);

                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Receipt having value: " + totalValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (receiptList.Count != 0)
                                    {
                                        procurementRepo.Add(receiptList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                            {
                                totalValue = 0;

                                foreach (var _receipt in receiptList)
                                {
                                    _receipt.saleReceiptStatus = statusChanged;
                                    _receipt.stage = TransactionStage.AwaitingApproval.ToString();
                                    _receipt.LastStatusChangeDate = System.DateTime.Now;
                                    _receipt.ClosingDate = System.DateTime.Now;
                                    if (_receipt.PendingForClosing != true)
                                    {
                                        _receipt.PendingForClosing = true;
                                    }
                                    totalValue = totalValue + _receipt.CollectionAmount;
                                    //List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                                    //if (banktransactionFlag == 0)
                                    //{
                                    //    var receiptAccount = repo.GetAccount(_receipt.AccountId);
                                    //    if (receiptAccount.COA_accountId != null)
                                    //    {
                                    //        JournalTransaction bankTransaction = new JournalTransaction()
                                    //        {
                                    //            accountId = receiptAccount.COA_accountId,
                                    //            coaTransactionsType = coaTransactionsType.SaleReceipt,
                                    //            creationDate = _receipt.CreationDate,
                                    //            debit = Convert.ToDouble(totalSum),
                                    //            credit = 0,
                                    //            //MER = 1,
                                    //            userId = _receipt.user_Id,
                                    //            SaleReceiptId = _receipt.Id,
                                    //            transactionRefno = _receipt.ReceiptRefNo,
                                    //            deptId = _receipt.department.Id,
                                    //        };
                                    //        journalTransactions.Add(bankTransaction);
                                    //        banktransactionFlag = 1;
                                    //    }
                                    //}
                                    //if (_receipt.receiptDeductions.Count != 0)
                                    //{
                                    //    foreach (var receiptDeduction in _receipt.receiptDeductions)
                                    //    {
                                    //        if (receiptDeduction.Amount != 0)
                                    //        {
                                    //            if (receiptDeduction.deduction.chartofAccountId != null)
                                    //            {
                                    //                JournalTransaction deductionTransaction = new JournalTransaction();

                                    //                deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                    //                deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //                deductionTransaction.creationDate = _receipt.CreationDate;
                                    //                deductionTransaction.debit = receiptDeduction.Amount;
                                    //                deductionTransaction.credit = 0;
                                    //                //deductionTransaction.MER = 1;
                                    //                //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                    //                deductionTransaction.userId = _receipt.user_Id;
                                    //                deductionTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //                deductionTransaction.SaleReceiptId = _receipt.Id;
                                    //                deductionTransaction.deptId = _receipt.department.Id;
                                    //                journalTransactions.Add(deductionTransaction);
                                    //            }
                                    //        }
                                    //    }

                                    //}
                                    //if (_receipt.department.chartofAccountId != null)
                                    //{
                                    //    JournalTransaction receivableTransaction = new JournalTransaction();
                                    //    receivableTransaction.accountId = _receipt.department.chartofAccountId;
                                    //    receivableTransaction.deptId = _receipt.department.Id;

                                    //    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //    receivableTransaction.creationDate = _receipt.CreationDate;
                                    //    receivableTransaction.debit = 0;
                                    //    receivableTransaction.credit = Math.Round(receipt.CollectionAmount, 2);
                                    //    //receivableTransaction.MER = 1;
                                    //    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    //    receivableTransaction.userId = _receipt.user_Id;
                                    //    receivableTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //    receivableTransaction.SaleReceiptId = _receipt.Id;
                                    //    receivableTransaction.deptId = _receipt.department.Id;
                                    //    journalTransactions.Add(receivableTransaction);
                                    //}
                                    //_receipt.journalTransactions = journalTransactions;
                                    repo.updateSalesReceiptForDirectClose(_receipt);
                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Receipt having value: " + totalValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (receiptList.Count != 0)
                                    {
                                        procurementRepo.Add(receiptList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                totalValue = 0;

                                foreach (var _receipt in receiptList)
                                {
                                    _receipt.saleReceiptStatus = statusChanged;
                                    _receipt.stage = TransactionStage.AwaitingFirstReview.ToString();
                                    _receipt.PendingForClosing = true;
                                    _receipt.LastStatusChangeDate = System.DateTime.Now;
                                    _receipt.ClosingDate = System.DateTime.Now;
                                    usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                                    totalValue = totalValue + _receipt.CollectionAmount;
                                    //List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
                                    //if (banktransactionFlag == 0)
                                    //{
                                    //    var receiptAccount = repo.GetAccount(_receipt.AccountId);
                                    //    if (receiptAccount.COA_accountId != null)
                                    //    {
                                    //        JournalTransaction bankTransaction = new JournalTransaction()
                                    //        {
                                    //            accountId = receiptAccount.COA_accountId,
                                    //            coaTransactionsType = coaTransactionsType.SaleReceipt,
                                    //            creationDate = _receipt.CreationDate,
                                    //            debit = Convert.ToDouble(totalSum),
                                    //            credit = 0,
                                    //            //MER = 1,
                                    //            userId = _receipt.user_Id,
                                    //            SaleReceiptId = _receipt.Id,
                                    //            transactionRefno = _receipt.ReceiptRefNo,
                                    //            deptId = _receipt.department.Id,
                                    //        };
                                    //        journalTransactions.Add(bankTransaction);
                                    //        banktransactionFlag = 1;
                                    //    }
                                    //}
                                    //if (_receipt.receiptDeductions.Count != 0)
                                    //{
                                    //    foreach (var receiptDeduction in _receipt.receiptDeductions)
                                    //    {
                                    //        if (receiptDeduction.Amount != 0)
                                    //        {
                                    //            if (receiptDeduction.deduction.chartofAccountId != null)
                                    //            {
                                    //                JournalTransaction deductionTransaction = new JournalTransaction();

                                    //                deductionTransaction.accountId = receiptDeduction.deduction.chartofAccountId;
                                    //                deductionTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //                deductionTransaction.creationDate = _receipt.CreationDate;
                                    //                deductionTransaction.debit = receiptDeduction.Amount;
                                    //                deductionTransaction.credit = 0;
                                    //                //deductionTransaction.MER = 1;
                                    //                //deductionTransaction.AmountMER = receiptDeduction.Amount * 1;
                                    //                deductionTransaction.userId = _receipt.user_Id;
                                    //                deductionTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //                deductionTransaction.SaleReceiptId = _receipt.Id;
                                    //                deductionTransaction.deptId = _receipt.department.Id;
                                    //                journalTransactions.Add(deductionTransaction);
                                    //            }
                                    //        }
                                    //    }

                                    //}
                                    //if (_receipt.department.chartofAccountId != null)
                                    //{
                                    //    JournalTransaction receivableTransaction = new JournalTransaction();
                                    //    receivableTransaction.accountId = _receipt.department.chartofAccountId;
                                    //    receivableTransaction.deptId = _receipt.department.Id;

                                    //    receivableTransaction.coaTransactionsType = coaTransactionsType.SaleReceipt;
                                    //    receivableTransaction.creationDate = _receipt.CreationDate;
                                    //    receivableTransaction.debit = 0;
                                    //    receivableTransaction.credit = Math.Round(receipt.CollectionAmount, 2);
                                    //    //receivableTransaction.MER = 1;
                                    //    //receivableTransaction.AmountMER = Math.Round(_item.TotalAmount, 2) * 1;
                                    //    receivableTransaction.userId = _receipt.user_Id;
                                    //    receivableTransaction.transactionRefno = _receipt.ReceiptRefNo;
                                    //    receivableTransaction.SaleReceiptId = _receipt.Id;
                                    //    receivableTransaction.deptId = _receipt.department.Id;
                                    //    journalTransactions.Add(receivableTransaction);
                                    //}
                                    //_receipt.journalTransactions = journalTransactions;
                                    repo.updateSalesReceiptForDirectClose(_receipt);
                                }

                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Receipt having value: " + totalValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (receiptList.Count != 0)
                                    {
                                        procurementRepo.Add(receiptList[0].transactionGroupId, TransactionItemType.Sale_Receipt, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Receipt #" + SR.SystemRefNo, SR.transactionGroupId, TransactionItemType.Sale_Receipt, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }
                            }
                            DXMessageBox.Show("Sale Receipt status changed to InActive (" + statusChanged.Status + ")");
                        }
                    }
                }
                else
                {
                    DXMessageBox.Show("You are not Allowed to Close Sale Receipt Directly.");
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            AllActive = 7;
            statusId = 0;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleReceipt List") != null)
            {
                List<SalesReceipt> saleReceipts = new List<SalesReceipt>();

                lblHeading.Text = "(Pending for Closing) Sale Receipts";
                if (MainWindow.currentUserid != 0)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleReceipt List") != null)
                    {
                        saleReceipts = repo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                        {
                            saleReceipts = repo.getAllPendingForClosingFirst(MainWindow.currentUserid);
                        }
                        else
                        {
                            saleReceipts = repo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);
                        }
                    }
                }
                grdSaleReceiptList.ItemsSource = saleReceipts;
            }
            else
            {
                DXMessageBox.Show("Permission required to View Pending for closing Sale Receipts!!");
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            AllActive = 9;
            statusId = 0;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleReceipts") != null)
            {
                UsersRepo usersRepo = new UsersRepo();
                List<SalesReceipt> saleReceipts = new List<SalesReceipt>();

                lblHeading.Text = "Void Sale Receipts";
                if (MainWindow.currentUserid != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleReceipts") != null)
                    {
                        saleReceipts = repo.getVoidRegisterFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        saleReceipts = repo.getVoidRegisterOwnFirst(MainWindow.currentUserid);
                    }
                }
                else
                {
                    saleReceipts = repo.getVoidRegisterAdministratorFirst();
                }
               

                grdSaleReceiptList.ItemsSource = saleReceipts;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Sale Receipts!!");
            }
        }

     

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (grdSaleReceiptList.GetFocusedRow() != null)
                {
                    var selectedRow = grdSaleReceiptList.SelectedItem as SalesReceipt; ;
                    var recept = repo.GetSalesReceipt(selectedRow.Id);
                    var receipts = repo.getReceiptsByGroupId(recept.transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (recept.isApproved != true)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null) ? true : false)
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].isApproved = true;
                                    receipts[i].stage = TransactionStage.Approved.ToString();
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, recept.transactionGroupId, 11, frmInputBox.comment);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                            {
                                if (recept.isApproved == null)
                                {
                                    for (int i = 0; i < receipts.Count; i++)
                                    {
                                        receipts[i].isApproved = false;
                                        receipts[i].stage = TransactionStage.Approved.ToString();
                                    }

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, recept.transactionGroupId, 11, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                            {
                                if (recept.isApproved == null)
                                {
                                    for (int i = 0; i < receipts.Count; i++)
                                    {
                                        receipts[i].isApproved = false;
                                    }

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, recept.transactionGroupId, 11, frmInputBox.comment);
                            }
                            else
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].isApproved = false;
                                }
                            }

                            repo.updateSalesReceipt(receipts);

                            MessageBox.Show("SaleReceipts are Approved (" + recept.transactionGroupId + ")");
                            SystemLog.LogInfo(this.GetType(), "SaleReceipt is Approved (" + recept.transactionGroupId + ")");
                        }
                        else
                        {
                            MessageBox.Show("You are not Allowed to Approve SaleReceipt Directly");
                            SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve SaleReceipt Directly user id=(" + MainWindow.currentUserid + ")");
                            return;
                        }

                    }
                    else if (recept.isReApproved == false)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null) ? true : false)
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].isReApproved = true;
                                    receipts[i].stage = TransactionStage.Approved.ToString();

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, recept.transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].stage = TransactionStage.Approved.ToString();
                                    if (receipts[i].isReApproved == null)
                                    {
                                        receipts[i].isReApproved = false;
                                    }
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, recept.transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null)
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (receipts[i].isReApproved == null)
                                    {
                                        receipts[i].isReApproved = false;
                                    }
                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, recept.transactionGroupId, (int)TransactionItemType.Sale_Receipt, frmInputBox.comment);
                            }
                            else
                            {
                                for (int i = 0; i < receipts.Count; i++)
                                {
                                    receipts[i].isReApproved = false;
                                }
                            }


                            repo.updateSalesReceipt(receipts);

                            MessageBox.Show("SaleReceipts are Approved (" + recept.transactionGroupId + ")");
                            SystemLog.LogInfo(this.GetType(), "SaleReceipt is Approved (" + recept.transactionGroupId + ")");
                        }
                        else
                        {
                            MessageBox.Show("You are not Allowed to Re-Approve SaleReceipt Directly");
                            SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve SaleReceipt Directly user id=(" + MainWindow.currentUserid + ")");
                            return;
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
            AllActive = 6;
            statusId = 0;
            repo = new SalesReceiptRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleReceipt List") != null)
            {
                List<SalesReceipt> saleReceipts = new List<SalesReceipt>();
                if (MainWindow.currentUserid != 0)
                {
                    lblHeading.Text = "Pending For Approval Sale Receipts";
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleReceipt List") != null)
                    {
                        //List<SaleReceipts> saleReceipts = new List<SaleReceipts>();
                        saleReceipts = repo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid); //To be continue
                                                                                                            //grdSaleReceiptList.ItemsSource = saleReceipts;
                       

                        grdSaleReceiptList.ItemsSource = saleReceipts;
                    }
                    else
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                        {
                            saleReceipts = repo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                        }

                        else
                        {
                            saleReceipts = repo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);

                        }
                    }

                    lblHeading.Text = "Pending For Approval Sale Receipts";
                    

                    grdSaleReceiptList.ItemsSource = saleReceipts;
                }
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Sale Receipts!!");
            }



        }


        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            AllActive = 8;
            statusId = 0;
            UsersRepo usersRepo = new UsersRepo();
            List<SalesReceipt> saleReceipts = new List<SalesReceipt>();

            lblHeading.Text = "Sale Receipt Register";
            if (MainWindow.currentUserid != 0)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                {
                    saleReceipts = repo.getSaleReceiptRegisterFirst(MainWindow.currentUserid);
                }

                else
                {
                 
                    saleReceipts = repo.getSaleReceiptRegisterFirst(MainWindow.currentUserid);

                }
            }

            else
            {
                saleReceipts = repo.getSaleReceiptRegisterAdministratorFirst();
            }
            grdSaleReceiptList.ItemsSource = saleReceipts;
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            AllActive = 5;
            statusId = 0;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleReceipts List") != null)
            {
                UsersRepo usersRepo = new UsersRepo();
                List<SalesReceipt> saleReceipts = new List<SalesReceipt>();
                lblHeading.Text = "Pending for ReApprovals SaleReceipts";
                if (MainWindow.currentUserid != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleReceipts List") != null)
                    {
                        saleReceipts = repo.getAllPendingForReApprovalDepartmentalFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                        {
                            saleReceipts = repo.getAllPendingForReApprovalFirst(MainWindow.currentUserid);
                        }
                        else
                        {
                            saleReceipts = repo.getAllPendingForReApprovalOwnFirst(MainWindow.currentUserid);
                        }
                    }
                }
                grdSaleReceiptList.ItemsSource = saleReceipts;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Sale Receipts!!");
            }
        }

        private void MbtnViewDeductions_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Sale Receipts Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
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
                        ReportLogic.SaveGridReport(grdSaleReceiptList, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Sale Receipts Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
            {
                Window moduleWin = new Window();

                ucSaleReceiptSelectModule frmModuleSelect = new ucSaleReceiptSelectModule();
                //ucFrmPayments frmBillPayments = new ucFrmPayments();

                moduleWin.Content = frmModuleSelect;

                //enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //enterPaymentWin.WindowState = WindowState.Maximized;

                moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                moduleWin.Width = 400;
                moduleWin.Height = 250;
                moduleWin.ResizeMode = ResizeMode.CanMinimize;
                moduleWin.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Sale Receipt!");
            }
        }

        private void UpdateReceipt()
        {
            var selectedRow = grdSaleReceiptList.SelectedItem as SalesReceipt;

            if (selectedRow.receiptType == ReceiptType.Rental_Receipt)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Receipts") != null)
                {
                    ucFrmRentalReceiptAdd frmRentalReceipt = new ucFrmRentalReceiptAdd();
                    //paymentRepo = new PaymentRepo();
                    //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                    Window frmPiPaymentWindow = new Window();
                    if (selectedRow.saleReceiptStatus.isActive == false)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                        {
                            frmRentalReceipt.editFlag = true;
                            frmRentalReceipt.groupId = selectedRow.transactionGroupId;
                            frmRentalReceipt.receiptId = selectedRow.Id;
                            frmPiPaymentWindow.Content = frmRentalReceipt;
                            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                            frmPiPaymentWindow.Title = "Rental Receipts";
                            frmPiPaymentWindow.Show();
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Receipts!");
                            return;
                        }
                    }
                    else
                    {
                        frmRentalReceipt.editFlag = true;
                        frmRentalReceipt.groupId = selectedRow.transactionGroupId;
                        frmRentalReceipt.receiptId = selectedRow.Id;
                        frmPiPaymentWindow.Content = frmRentalReceipt;
                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                        frmPiPaymentWindow.Title = "Rental Receipts";
                        frmPiPaymentWindow.Show();
                    }

                }
                else
                {
                    DXMessageBox.Show("Permission required to View Rental Receipts!");
                }
                return;
            }

            if (selectedRow.receiptType == ReceiptType.Customer_Credits)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Customer Credit Receipts") != null)
                {
                    ucFrmCustomerCreditReceipt frmLAreceipt = new ucFrmCustomerCreditReceipt();
                    //paymentRepo = new PaymentRepo();
                    //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                    Window frmPiPaymentWindow = new Window();
                    if (selectedRow.saleReceiptStatus.isActive == false)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                        {
                            frmLAreceipt.editFlag = true;
                            frmLAreceipt.groupId = selectedRow.transactionGroupId;
                            frmLAreceipt.receiptId = selectedRow.Id;
                            frmPiPaymentWindow.Content = frmLAreceipt;
                            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                            frmPiPaymentWindow.Title = "Customer Credit Receipts";
                            frmPiPaymentWindow.Show();
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Receipts!");
                            return;
                        }
                    }
                    else
                    {
                        frmLAreceipt.editFlag = true;
                        frmLAreceipt.groupId = selectedRow.transactionGroupId;
                        frmLAreceipt.receiptId = selectedRow.Id;
                        frmPiPaymentWindow.Content = frmLAreceipt;
                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                        frmPiPaymentWindow.Title = "Customer Credit Receipts";
                        frmPiPaymentWindow.Show();
                    }

                }
                else
                {
                    DXMessageBox.Show("Permission required to View Customer Credit Receipts!");
                }
                return;
            }

            if (selectedRow.receiptType == ReceiptType.Direct_Receipt)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Direct Receipts") != null)
                {
                    if(selectedRow.payment != null)
                    {
                        ucFrmDirectReceiptPayment frmLAreceipt = new ucFrmDirectReceiptPayment();
                        //paymentRepo = new PaymentRepo();
                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                        Window frmPiPaymentWindow = new Window();
                        if (selectedRow.saleReceiptStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                            {
                                frmLAreceipt.editFlag = true;
                                frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                frmPiPaymentWindow.Content = frmLAreceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Direct Receipts";
                                frmPiPaymentWindow.Show();
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Direct Receipts!");
                                return;
                            }
                        }
                        else
                        {
                            frmLAreceipt.editFlag = true;
                            frmLAreceipt.groupId = selectedRow.transactionGroupId;
                            frmPiPaymentWindow.Content = frmLAreceipt;
                            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                            frmPiPaymentWindow.Title = "Direct Receipts";
                            frmPiPaymentWindow.Show();
                        }
                    }
                    else
                    {
                        ucFrmDirectSaleReceipt frmLAreceipt = new ucFrmDirectSaleReceipt();
                        //paymentRepo = new PaymentRepo();
                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                        Window frmPiPaymentWindow = new Window();
                        if (selectedRow.saleReceiptStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                            {
                                frmLAreceipt.editFlag = true;
                                frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                frmPiPaymentWindow.Content = frmLAreceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Direct Receipts";
                                frmPiPaymentWindow.Show();
                            }
                            else
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Direct Receipts!");
                                return;
                            }
                        }
                        else
                        {
                            frmLAreceipt.editFlag = true;
                            frmLAreceipt.groupId = selectedRow.transactionGroupId;
                            frmPiPaymentWindow.Content = frmLAreceipt;
                            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                            frmPiPaymentWindow.Title = "Direct Receipts";
                            frmPiPaymentWindow.Show();
                        }
                    }
                }
                else
                {
                    DXMessageBox.Show("Permission required to View Direct Receipts!");
                }
                return;
            }

            if (selectedRow.receiptType == ReceiptType.Loans_Advances)
            {
                switch (selectedRow.loansAdvance.advanceTemplate)
                {
                    case LoansAdvanceTemplate.Loan:
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                        {
                            ucFrmCompanyLoanSaleReceipt frmLAreceipt = new ucFrmCompanyLoanSaleReceipt();
                            //paymentRepo = new PaymentRepo();
                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                            Window frmPiPaymentWindow = new Window();
                            if (selectedRow.saleReceiptStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Sale Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                    return;
                                }
                            }
                            else
                            {
                                frmLAreceipt.editFlag = true;
                                frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                frmPiPaymentWindow.Content = frmLAreceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Sale Receipts";
                                frmPiPaymentWindow.Show();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                        }
                        break;
                    default:
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                        {
                            ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
                            //paymentRepo = new PaymentRepo();
                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                            Window frmPiPaymentWindow = new Window();
                            if (selectedRow.saleReceiptStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Sale Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                    return;
                                }
                            }
                            else
                            {
                                frmLAreceipt.editFlag = true;
                                frmLAreceipt.groupId = selectedRow.transactionGroupId;
                                frmPiPaymentWindow.Content = frmLAreceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Sale Receipts";
                                frmPiPaymentWindow.Show();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                        }
                        break;
                }
                
                return;
            }
            repo = new SalesReceiptRepo();
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                {
                    ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                    repo = new SalesReceiptRepo();
                    updateSaleReceiptObj.saveEditFlag = 1;
                    
                    if (selectedRow == null)
                    {
                        return;
                    }
                    var saleReceipt = repo.GetSalesReceipt(selectedRow.Id);
                    if (saleReceipt == null)
                    {
                        return;
                    }

                    if (saleReceipt.saleReceiptStatus != null)
                    {
                        var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                        updateSaleReceiptObj.selectedStatus = status;
                        if (status.isActive == false && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") == null))
                        {
                            DXMessageBox.Show("Permission required to View Closed Receipts!");
                            return;
                        }
                    }

                    for (int i = 0; i <= (int)ERP_BL.Enums.ReceiptType.Sales_Department; i++)
                    {
                        //if (((ERP_BL.Enums.ReceiptType)i).ToString() == selectedRow.receiptType)
                        //{
                        //    index = i;
                        //    break;
                        //    //break;
                        //}
                    }
                    updateSaleReceiptObj.dateEditcreationDate.EditValue = saleReceipt.CreationDate;
                    updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                    updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                    updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                    updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                    updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                    updateSaleReceiptObj.receiptId = saleReceipt.Id;
                    updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;


                    //updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                    ////ucFrmAddAccount obj = new ucFrmAddAccount();
                    //updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                    //updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                    //updateSaleReceiptObj.enter_receipt_win.Show();

                    updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
                    updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                    updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                    updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                    updateSaleReceiptObj.enterReceiptWindowFlag = true;


                    if (saleReceipt.CreditedDate != null)
                        updateSaleReceiptObj.dateEditcreditedDate.EditValue = (DateTime)saleReceipt.CreditedDate;

                    if (saleReceipt.DepositedDate != null)
                        updateSaleReceiptObj.datDepositedDate.EditValue = (DateTime)saleReceipt.DepositedDate;

                    if (saleReceipt.InstrumentDate != null)
                        updateSaleReceiptObj.datInstrumentDate.EditValue = (DateTime)saleReceipt.InstrumentDate;

                    if (saleReceipt.InstrumentNo != null)
                        updateSaleReceiptObj.txtInstrumentNo.Text = saleReceipt.InstrumentNo;

                    if (saleReceipt.principal != null)
                    {
                        PrincipalRepo prinRepo = new PrincipalRepo();
                        var principal = prinRepo.get(saleReceipt.principal.Id);
                    }

                    updateSaleReceiptObj.enter_receipt_win.Show();
                    //Load_Receipts();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                    return;
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateReceipt();
        }

        private void MbtnDeductions_Click(object sender, RoutedEventArgs e)
        {
            //List<DeductionAmount> dedAmounts = new List<DeductionAmount>();
            //var selectedRow = (AllSaleReceipts)grdSaleReceiptList.SelectedItem;
            //var saleReceipt = repo.GetSalesReceipt(selectedRow.SerialNo);

            //Window window = new Window();
            //GridControl grdCntrlDeduction = new GridControl();

            //GridColumn Title = new GridColumn();
            //Title.Header = "Title";
            //Title.FieldName = "Title";
            //Title.IsSmart = true;
            //Title.AllowEditing = DevExpress.Utils.DefaultBoolean.False;


            //GridColumn Amount = new GridColumn();
            //Amount.Header = "Amount";
            //Amount.FieldName = "Amount";
            //Amount.IsSmart = true;
            //Amount.AllowEditing = DevExpress.Utils.DefaultBoolean.False;

            //grdCntrlDeduction.Columns.Add(Title);
            //grdCntrlDeduction.Columns.Add(Amount);

            //if (saleReceipt.receiptDeductions != null)
            //{
            //    var deductions = saleReceipt.receiptDeductions;
            //    foreach (var _deduction in deductions)
            //    {
            //        DeductionAmount ded = new DeductionAmount();
            //        ded.Title = _deduction.deduction.title;
            //        ded.Amount = _deduction.Amount;
            //        dedAmounts.Add(ded);
            //    }
            //}

            //grdCntrlDeduction.ItemsSource = dedAmounts;
            //window.Title = "Deductions";
            //window.Width = 350;
            //window.Height = 250;
            //window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //window.ResizeMode = ResizeMode.NoResize;
            //window.Content = grdCntrlDeduction;

            //window.ShowDialog();
        }

        private void GrdSaleReceiptList_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                var receipt = grdSaleReceiptList.GetRowByListIndex(e.ListSourceRowIndex) as SalesReceipt;
                switch (e.Column.FieldName)
                {
                    case "departmentLevel1":
                        
                        if (receipt != null)
                        {
                            Department node = new Department()/*receipt.department*/;
                            switch (receipt.receiptType)
                            {
                                case ReceiptType.Customer_Credits:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Direct_Receipt:
                                    if (receipt.payment != null)
                                    {
                                        switch (receipt.payment.transactionType)
                                        {
                                            case PaymentTransactionType.Admin_Bills:
                                                node = receipt.payment.adminBill.department;
                                                break;
                                            case PaymentTransactionType.Loans_Advances:
                                                node = receipt.payment.loansAdvance.department;
                                                break;
                                            case PaymentTransactionType.Purchase_Invoice:
                                                node = receipt.payment.purchaseInvoice.department;
                                                break;
                                            case PaymentTransactionType.Target_Reward:
                                                break;
                                            case PaymentTransactionType.Vendor_Bills:
                                                node = receipt.payment.Bill.department;
                                                break;
                                        }
                                    }
                                    else
                                        node = receipt.department;
                                    break;
                                case ReceiptType.Loans_Advances:
                                    node = receipt.loansAdvance?.department;
                                    break;
                                case ReceiptType.Sales_Customer:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Sales_Department:
                                    node = receipt.saleInvoice?.department;
                                    break;
                            }
                            var deptList = new List<Department>();
                            

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        deptList.Add(node);
                                        node = node.parentDepartment;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        deptList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    deptList.Add(node);
                                    break;
                                }

                            }
                            deptList.Reverse();
                            e.Value = deptList[0].DeptName;
                        }
                        break;
                    case "departmentLevel2":
                        if (receipt != null)
                        {
                            Department node = new Department()/*receipt.department*/;
                            switch (receipt.receiptType)
                            {
                                case ReceiptType.Customer_Credits:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Direct_Receipt:
                                    if (receipt.payment != null)
                                    {
                                        switch (receipt.payment.transactionType)
                                        {
                                            case PaymentTransactionType.Admin_Bills:
                                                node = receipt.payment.adminBill.department;
                                                break;
                                            case PaymentTransactionType.Loans_Advances:
                                                node = receipt.payment.loansAdvance.department;
                                                break;
                                            case PaymentTransactionType.Purchase_Invoice:
                                                node = receipt.payment.purchaseInvoice.department;
                                                break;
                                            case PaymentTransactionType.Target_Reward:
                                                break;
                                            case PaymentTransactionType.Vendor_Bills:
                                                node = receipt.payment.Bill.department;
                                                break;
                                        }
                                    }
                                    else
                                        node = receipt.department;
                                    break;
                                case ReceiptType.Loans_Advances:
                                    node = receipt.loansAdvance?.department;
                                    break;
                                case ReceiptType.Sales_Customer:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Sales_Department:
                                    node = receipt.saleInvoice?.department;
                                    break;
                            }
                            var deptList = new List<Department>();

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        deptList.Add(node);
                                        node = node.parentDepartment;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        deptList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    deptList.Add(node);
                                    break;
                                }

                            }

                            deptList.Reverse();

                            switch (deptList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = deptList[0].DeptName;
                                    break;
                                case 2:
                                    e.Value = deptList[1].DeptName;
                                    break;
                                case 3:
                                    e.Value = deptList[1].DeptName;
                                    break;
                                case 4:
                                    e.Value = deptList[1].DeptName;
                                    break;
                                case 5:
                                    e.Value = deptList[1].DeptName;
                                    break;
                            }


                        }
                        break;
                    case "departmentLevel3":
                        if (receipt != null)
                        {
                            var deptList = new List<Department>();
                            Department node = new Department()/*receipt.department*/;
                            switch (receipt.receiptType)
                            {
                                case ReceiptType.Customer_Credits:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Direct_Receipt:
                                    if (receipt.payment != null)
                                    {
                                        switch (receipt.payment.transactionType)
                                        {
                                            case PaymentTransactionType.Admin_Bills:
                                                node = receipt.payment.adminBill.department;
                                                break;
                                            case PaymentTransactionType.Loans_Advances:
                                                node = receipt.payment.loansAdvance.department;
                                                break;
                                            case PaymentTransactionType.Purchase_Invoice:
                                                node = receipt.payment.purchaseInvoice.department;
                                                break;
                                            case PaymentTransactionType.Target_Reward:
                                                break;
                                            case PaymentTransactionType.Vendor_Bills:
                                                node = receipt.payment.Bill.department;
                                                break;
                                        }
                                    }
                                    else
                                        node = receipt.department;
                                    break;
                                case ReceiptType.Loans_Advances:
                                    node = receipt.loansAdvance?.department;
                                    break;
                                case ReceiptType.Sales_Customer:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Sales_Department:
                                    node = receipt.saleInvoice?.department;
                                    break;
                            }

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        deptList.Add(node);
                                        node = node.parentDepartment;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        deptList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    deptList.Add(node);
                                    break;
                                }

                            }
                            deptList.Reverse();

                            switch (deptList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = deptList[0].DeptName;
                                    break;
                                case 2:
                                    e.Value = deptList[1].DeptName;
                                    break;
                                case 3:
                                    e.Value = deptList[2].DeptName;
                                    break;
                                case 4:
                                    e.Value = deptList[2].DeptName;
                                    break;
                                case 5:
                                    e.Value = deptList[2].DeptName;
                                    break;
                            }


                        }
                        break;
                    case "departmentLevel4":
                        if (receipt != null)
                        {
                            var deptList = new List<Department>();
                            Department node = new Department()/*receipt.department*/;
                            switch (receipt.receiptType)
                            {
                                case ReceiptType.Customer_Credits:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Direct_Receipt:
                                    if (receipt.payment != null)
                                    {
                                        switch (receipt.payment.transactionType)
                                        {
                                            case PaymentTransactionType.Admin_Bills:
                                                node = receipt.payment.adminBill.department;
                                                break;
                                            case PaymentTransactionType.Loans_Advances:
                                                node = receipt.payment.loansAdvance.department;
                                                break;
                                            case PaymentTransactionType.Purchase_Invoice:
                                                node = receipt.payment.purchaseInvoice.department;
                                                break;
                                            case PaymentTransactionType.Target_Reward:
                                                break;
                                            case PaymentTransactionType.Vendor_Bills:
                                                node = receipt.payment.Bill.department;
                                                break;
                                        }
                                    }
                                    else
                                        node = receipt.department;
                                    break;
                                case ReceiptType.Loans_Advances:
                                    node = receipt.loansAdvance?.department;
                                    break;
                                case ReceiptType.Sales_Customer:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Sales_Department:
                                    node = receipt.saleInvoice?.department;
                                    break;
                            }

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and tranverse to its parent
                                        deptList.Add(node);
                                        node = node.parentDepartment;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        deptList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    deptList.Add(node);
                                    break;
                                }

                            }
                            deptList.Reverse();

                            switch (deptList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = deptList[0].DeptName;
                                    break;
                                case 2:
                                    e.Value = deptList[1].DeptName;
                                    break;
                                case 3:
                                    e.Value = deptList[2].DeptName;
                                    break;
                                case 4:
                                    e.Value = deptList[3].DeptName;
                                    break;
                                case 5:
                                    e.Value = deptList[3].DeptName;
                                    break;
                            }

                        }
                        break;
                    case "departmentLevel5":
                        if (receipt != null)
                        {
                            var deptList = new List<Department>();
                            Department node = new Department()/*receipt.department*/;
                            switch (receipt.receiptType)
                            {
                                case ReceiptType.Customer_Credits:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Direct_Receipt:
                                    if (receipt.payment != null)
                                    {
                                        switch (receipt.payment.transactionType)
                                        {
                                            case PaymentTransactionType.Admin_Bills:
                                                node = receipt.payment.adminBill.department;
                                                break;
                                            case PaymentTransactionType.Loans_Advances:
                                                node = receipt.payment.loansAdvance.department;
                                                break;
                                            case PaymentTransactionType.Purchase_Invoice:
                                                node = receipt.payment.purchaseInvoice.department;
                                                break;
                                            case PaymentTransactionType.Target_Reward:
                                                break;
                                            case PaymentTransactionType.Vendor_Bills:
                                                node = receipt.payment.Bill.department;
                                                break;
                                        }
                                    }
                                    else
                                        node = receipt.department;
                                    break;
                                case ReceiptType.Loans_Advances:
                                    node = receipt.loansAdvance?.department;
                                    break;
                                case ReceiptType.Sales_Customer:
                                    node = receipt.saleInvoice?.department;
                                    break;
                                case ReceiptType.Sales_Department:
                                    node = receipt.saleInvoice?.department;
                                    break;
                            }

                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and tranverse to its parent
                                        deptList.Add(node);
                                        node = node.parentDepartment;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        deptList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    deptList.Add(node);
                                    break;
                                }

                            }
                            deptList.Reverse();

                            switch (deptList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = deptList[0].DeptName;
                                    break;
                                case 2:
                                    e.Value = deptList[1].DeptName;
                                    break;
                                case 3:
                                    e.Value = deptList[2].DeptName;
                                    break;
                                case 4:
                                    e.Value = deptList[3].DeptName;
                                    break;
                                case 5:
                                    e.Value = deptList[4].DeptName;
                                    break;
                            }

                        }
                        break;


                    case "Customerr":
                        if (receipt != null)
                        {
                            if (receipt.Customer != null && receipt.Customer.company != null)
                            {
                                e.Value = receipt.Customer.company.CompanyName;
                            }
                            else if (receipt.saleInvoice != null && receipt.saleInvoice.customerCompany != null && receipt.saleInvoice.customerCompany.company != null)
                            {
                                e.Value = receipt.saleInvoice.customerCompany.company.CompanyName;
                            }
                        }
                        break;

                    case "Deductionss":
                        if (receipt != null)
                        {
                            var receiptDeductions = receipt.receiptDeductions;
                            if (receiptDeductions != null && receiptDeductions.Count != 0)
                            {
                                e.Value = Convert.ToDecimal( receiptDeductions.Sum(x => x.Amount));
                            }
                            else
                                e.Value = 0;
                        }
                        break;
                    case "VAT":

                        if (receipt != null)
                        {
                            Decimal taxAmount = 0;

                            if (receipt.ReceiptDeductionTaxes != null && receipt.ReceiptDeductionTaxes.Count != 0)
                            {

                                taxAmount = taxAmount + Convert.ToDecimal(receipt.ReceiptDeductionTaxes.Sum(x => x.Amount));
                            }
                            if (receipt.ReceiptBankTaxes != null && receipt.ReceiptBankTaxes.Count != 0)
                            {

                                taxAmount = taxAmount + Convert.ToDecimal(receipt.ReceiptBankTaxes.Sum(x => x.Amount));
                            }

                            e.Value = taxAmount;
                        }


                        break;
                    case "bankchargess":

                        if (receipt != null)
                        {


                            if (receipt.bankCharges != null && receipt.bankCharges.Count != 0)
                            {

                                e.Value = Convert.ToDecimal(receipt.bankCharges.Sum(x => x.Amount));
                            }
                            else
                                e.Value = 0;
                        }


                        break;
                    case "CreditedAmount":
                        if (e.GetListSourceFieldValue("Id") != null)
                        {
                            var recpt = grdSaleReceiptList.GetRowByListIndex(e.ListSourceRowIndex) as SalesReceipt;
                            if (recpt != null && recpt.receiptType != ReceiptType.Direct_Receipt)
                            {


                                var collectionAmount = e.GetListSourceFieldValue("CollectionAmount");
                                var _receipt = grdSaleReceiptList.GetRowByListIndex(e.ListSourceRowIndex) as SalesReceipt;
                                List<ReceiptDeduction> receiptDeductions = _receipt.receiptDeductions;
                                if (receiptDeductions != null && receiptDeductions.Count != 0)
                                {

                                    var totalDeductions = receiptDeductions.Sum(x => x.Amount);
                                    var VAT = _receipt.ReceiptDeductionTaxes.Sum(x => x.Amount);
                                    e.Value = Convert.ToDecimal(collectionAmount) - Convert.ToDecimal(totalDeductions) - Convert.ToDecimal(VAT);
                                }
                                else
                                {
                                    e.Value = Convert.ToDecimal(collectionAmount);
                                }
                            }
                            else
                            {
                                e.Value = recpt.CollectionAmount;
                            }
                        }
                        break;
                    case "RemainingAmount":
                        if (e.GetListSourceFieldValue("Id") != null)
                        {
                            var saleInvoice = e.GetListSourceFieldValue("saleInvoice") as SaleInvoice;

                            //List<ReceiptDeduction> receiptDeductions = (e.GetListSourceFieldValue("receiptDeductions")) as List<ReceiptDeduction>;
                            if (saleInvoice != null)
                            {
                                if (saleInvoice.salesReceipts != null && saleInvoice.salesReceipts.Count != 0)
                                {
                                    var reciepts = saleInvoice.salesReceipts;
                                    var result = reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                                    e.Value = saleInvoice.totalInvoiceAmount - result;

                                }
                                else
                                {
                                    e.Value = Convert.ToDecimal(saleInvoice.totalInvoiceAmount);
                                }
                            }

                        }
                        break;
                }
            }
        }

        private void BtnDateFilter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (dateFrom.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateFrom.Focus();

                return;
            }
            else
                if (dateTo.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateTo.Focus();

                return;

            }
            else
            {

                LoadOrdersByDate();
                //RemoveSourceObjects();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });

            }
        }
        private void LoadOrdersByDate()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
          

            if (lblHeading.Text == "Sale Orders(Open)" || lblHeading.Text == "Sale Orders(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                   
                }
                else if (AllActive == 1)
                {
                    salesReceipts = repo.GetAllSalesReceiptOpenAndClosed((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    statusId = 0;
                    AllActive = 1;

                }
                else if (AllActive == 2)
                {
                    salesReceipts = repo.getAllActiveandUnapprovedReceipts((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    statusId = 0;
                    AllActive = 2;
                }
                else if (AllActive == 3)
                {
                    salesReceipts = repo.getAllInActiveandUnapprovedReceipts((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    statusId = 0;
                    AllActive = 3;

                }
                if (AllActive == 4)
                {
                    if (treeStatusId != 0)
                    {
                        salesReceipts = repo.getAllSaleReceiptsbyStatusId((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid, treeStatusId);
                        statusId = 0;
                        AllActive = 4;
                    }
                }
                else
                if (AllActive == 5)
                {
                    AllActive = 5;
                    statusId = 0;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleReceipts List") != null)
                    {
                        UsersRepo usersRepo = new UsersRepo();
                        lblHeading.Text = "Pending for ReApprovals SaleReceipts";
                        if (MainWindow.currentUserid != 0)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) SaleReceipts List") != null)
                            {
                                salesReceipts = repo.getAllPendingForReApprovalDepartmental((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleReceipt without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                                {
                                    salesReceipts = repo.getAllPendingForReApproval((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                                }
                                else
                                {
                                    salesReceipts = repo.getAllPendingForReApprovalOwn((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Permission Required to View Pending For Re-Approval Sale Receipts!!");
                    }
                }
                else
                if (AllActive == 6)
                {

                    AllActive = 6;
                    statusId = 0;
                    repo = new SalesReceiptRepo();
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleReceipt List") != null)
                    {
                        if (MainWindow.currentUserid != 0)
                        {
                            lblHeading.Text = "Pending For Approval Sale Receipts";
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleReceipt List") != null)
                            {
                                //List<SaleReceipts> saleReceipts = new List<SaleReceipts>();
                                salesReceipts = repo.getAllPendingForApprovalDepartmental((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid); //To be continue
                                                                                                                         //grdSaleReceiptList.ItemsSource = saleReceipts;


                            }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                                {
                                    salesReceipts = repo.getAllPendingForApproval((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                                }

                                else
                                {
                                    salesReceipts = repo.getAllPendingForApprovalOwn((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                                }
                            }

                            lblHeading.Text = "Pending For Approval Sale Receipts";


                        }
                    }
                    else
                    {
                        MessageBox.Show("Permission Required to view Pending for Approval Sale Receipts!!");
                    }

                }
                else  
                if (AllActive == 7)
                {
                    AllActive = 7;
                    statusId = 0;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleReceipt List") != null)
                    {

                        lblHeading.Text = "(Pending for Closing) Sale Receipts";
                        if (MainWindow.currentUserid != 0)
                        {

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleReceipt List") != null)
                            {
                                salesReceipts = repo.getAllPendingForClosingDepartmental((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                                {
                                    salesReceipts = repo.getAllPendingForClosing((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                                }
                                else
                                {
                                    salesReceipts = repo.getAllPendingForClosingOwn((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                                }
                            }
                        }
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to View Pending for closing Sale Receipts!!");
                    }

                }
                else  
                if (AllActive == 8)
                {

                    AllActive = 8;
                    statusId = 0;
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Sale Receipt Register";
                    if (MainWindow.currentUserid != 0)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleReceipt") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleReceipt") != null)
                        {
                            salesReceipts = repo.getSaleReceiptRegister((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            salesReceipts = repo.getSaleReceiptRegister((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                    }
                    else
                    {
                        salesReceipts = repo.getSaleReceiptRegisterAdministrator((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                    }
                }
                else
                if (AllActive == 9)
                {
                    AllActive = 9;
                    statusId = 0;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleReceipts") != null)
                    {
                        UsersRepo usersRepo = new UsersRepo();

                        lblHeading.Text = "Void Sale Receipts";
                        if (MainWindow.currentUserid != 0)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleReceipts") != null)
                            {
                                salesReceipts = repo.getVoidRegister((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                salesReceipts = repo.getVoidRegisterOwn((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                        }
                        else
                        {
                            salesReceipts = repo.getVoidRegisterAdministrator((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Permission Required to view Void Sale Receipts!!");
                    }

                }
            }
            grdSaleReceiptList.ItemsSource = salesReceipts.Distinct().ToList();
            grdSaleReceiptList.RefreshData();
        }

        private void cmbxDepartmentFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbxDepartmentTo.SelectedIndex = -1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cmbxDepartmentTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > cmbxDepartmentTo.SelectedIndex)
                {
                    cmbxDepartmentTo.SelectedIndex = -1;
                    DXMessageBox.Show("Please select greater department!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnDepartmentFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > -1 && cmbxDepartmentTo.SelectedIndex > -1)
                {
                    switch (cmbxDepartmentFrom.SelectedIndex)
                    {
                        case 0:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 0:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 1:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 1:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {

                                case 1:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 2:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 3:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 4:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    DXMessageBox.Show("Please select Department Levels to Apply filter!");
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }
    }
 
}
