using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.ToDoTasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.ChartofAccounts.ViewModels;
using ZAS_ERP.ChartofAccounts.Windows;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.ToDoTasks.TargetRewardss;

namespace ZAS_ERP.ChartofAccounts
{
    public class CoaLogicClass
    {
        public List<ChartofAccount> accountsList = new List<ChartofAccount>();
        public List<JournalTransaction> jvList = new List<JournalTransaction>();
        public List<JournalTransaction> transactions = new List<JournalTransaction>();
        public JournalEntries myParent = null;
        public string logTransactionRef = null;
        public string logEntryNo = null;
        int editTransactions = 0;
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        JournalEntryRepo journalTransactionRepo = new JournalEntryRepo();
        JournalVoucherRepo JournalVoucherRepo = new JournalVoucherRepo();
        private InterBankTransRepo bankTransRepo;
        CompanyRepo compRepo = new CompanyRepo();
        DepartmentRepo deptRepo = new DepartmentRepo();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        PaymentRepo paymentRepo = new PaymentRepo();


        public CoaLogicClass()
        {
        }
        public CoaLogicClass(List<ChartofAccount> internalAccountsList)
        {

            accountsList.Clear();
            accountsList = internalAccountsList;
        }


        //public void GetAllTransactions()
        //{
        //    if (transactions.Count != 0)
        //    {
        //        foreach (var item in transactions)
        //        {
        //            LedgerViewModel ledger = new LedgerViewModel();
        //            ledger.Id = item.Id;
        //            ledger.chartofAccount = item.account;
        //            ledger.coaTransactionsType = item.coaTransactionsType;
        //            ledger.creationDate = item.creationDate;
        //            ledger.credit = item.credit;
        //            ledger.debit = item.debit;
        //            ledger.memo = item.memo;
        //            double cred_amount = item.credit;
        //            double ded = item.debit;
        //            double total = ded - cred_amount;
        //            ledger.Balance = total;
        //            if (item.journalVoucher != null)
        //            {
        //                //if (item.MER != null)
        //                //{
        //                //    ledger.MER = (double)item.MER;
        //                //    ledger.AmountMER = (double)item.AmountMER;
        //                //    ledger.Company = item.journalVoucher.company;
        //                //}
        //            }
        //            else
        //                if (item.InterBank != null)
        //            {
        //                //if (item.MER != null)
        //                //{
        //                //    ledger.MER = (double)item.MER;
        //                //    ledger.AmountMER = (double)item.AmountMER;
        //                //    ledger.Company = item.InterBank.company;
        //                //}

        //            }
        //            else
        //                if (item.SaleInvoice != null)
        //            {
        //                //if (item.MER != null)
        //                //{
        //                //    ledger.MER = (double)item.MER;
        //                //    ledger.AmountMER = (double)item.AmountMER;
        //                //    ledger.Company = item.SaleInvoice.company;
        //                //}

        //            }

        //            journalTransactions.Add(ledger);
        //        }
        //    }
        //}

        string accountType;
        public string GetAccountType(COA_AccountType type)
        {
            if (type == COA_AccountType.Income)
            {
                
                accountType = "Income";
            }
            else
                  if (type == COA_AccountType.Expense)
            {
                accountType = "Expense";
            }
            else
                  if (type == COA_AccountType.Fixed_Asset)
            {
                accountType = "Fixed Assets";
            }
            else
                  if (type == COA_AccountType.Bank)
            {
                accountType = "Bank";
            }
            if (type == COA_AccountType.Loan)
            {
                accountType = "Loan";
            }
            else
                  if (type == COA_AccountType.Credit_Card)
            {
                accountType = "Credit Card";
            }
            if (type == COA_AccountType.Equity)
            {
                accountType = "Equity";
            }
            else
                  if (type == COA_AccountType.Account_Receivable)
            {
                accountType = "Account Receivable";
            }
            else
                  if (type == COA_AccountType.Other_Income)
            {
                accountType = "Other Income";
            }
            else
                  if (type == COA_AccountType.Other_Expense)
            {
                accountType = "Other Expense";
            }
            else
                  if (type == COA_AccountType.Accounts_Payable)
            {
                accountType = "Account Payable";
            }
            else
                  if (type == COA_AccountType.Other_Current_Asset)
            {
                accountType = "Other Current Asset";
            }
            else
                  if (type == COA_AccountType.Other_Current_Liability)
            {
                accountType = "Other Current Liability";
            }
            else
                  if (type == COA_AccountType.Cost_of_Goods_Sold)
            {
                accountType = "Cost of Goods Sold";
            }
            else
                  if (type == COA_AccountType.Other_Asset)
            {
                accountType = "Other Asset";
            }
            else
                  if (type == COA_AccountType.Longterm_Liability)
            {
                accountType = "Long term Liability";
            }

            return accountType;
        }

        internal void AddJournalList(IList list, DateTime postingDate, string transactionRefNo, string EntryNumber)
        {
            var myWindow = Window.GetWindow(myParent);
            foreach (JournalTransaction item in list)
            {
                if (item.Id == 0)
                {
                    //item.postingDate = postingDate;
                    //item.transactionRefno = transactionRefNo;
                    //item.entryNumber = EntryNumber;
                    //item.coaTransactionsType = coaTransactionsType.JV;
                    item.accountId = item.account.Id;
                    item.account = null;
                    item.userId = SYSTEM_STATIC.currentUser.id;
                    item.creationDate = DateTime.Now;
                    jvList.Add(item);
                }
            }
            journalTransactionRepo.AddJournalTransaction(jvList);
            DXMessageBox.Show("List has been Added successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            //SystemLog.LogInfoJV(this.GetType(), "List Added Succesfully refrence No= " + logTransactionRef + " EntryNo=" + logEntryNo);

        }

        public void AddAccount(ChartofAccount Account)
        {
            if (Account != null)
            {
                repo.AddAccount(Account);
            }
        }
        public void UpdateAccount(ChartofAccount Account)
        {
            
            repo.UpdateAccount(Account);
        }

        internal void BindUserId(IList list, DateTime postingDate, string transactionRefNo, string EntryNumber)
        {
            var myWindow = Window.GetWindow(myParent);
            foreach (JournalTransaction item in list)
            {
                if (item.Id == 0)
                {
                    item.accountId = item.account.Id;
                    item.account = null;
                    item.userId = SYSTEM_STATIC.currentUser.id;
                    item.creationDate = DateTime.Now;
                    jvList.Add(item);
                }
            }
        }
        internal void ApproveJV()
        {
            var myWindow = Window.GetWindow(myParent);
            foreach (JournalTransaction item in jvList)
            {
                //item.stage = TransactionStage.Approved.ToString();
                //item.isApproved = true;
                //item.ApprovedDate = System.DateTime.Now;
            }
        }
        internal void AddWithoutApprovalJV()
        {
            foreach (JournalTransaction item in jvList)
            {
                //item.stage = TransactionStage.Approved.ToString();
                //item.isApproved = true;
                //item.ApprovedDate = System.DateTime.Now;
            }
        }
        internal void AddWithApprovals()
        {
            foreach (JournalTransaction item in jvList)
            {
                //item.stage = TransactionStage.AwaitingFirstReview.ToString();
                //item.isApproved = false;
            }
        }
        public void AddTransaction()
        {
            journalTransactionRepo.AddJV(jvList);
        }
        internal void AddVoucher(IList list, JournalVoucher voucher)
        {
            JournalVoucherRepo repo = new JournalVoucherRepo();
            List<JournalTransaction> transactions = new List<JournalTransaction>();
            transactions.AddRange(GetVoucherTransactions(list, voucher));
            voucher.journalTransactions = transactions;
            voucher.statusId = voucher.JournalVoucherStatus.Id;
            voucher.user = null;
            voucher.JournalVoucherStatus = null;
            voucher.Currency = null;
            
            repo.AddJournalVoucher(voucher);
            DXMessageBox.Show("Journal Voucher has been saved successfully,Congratulations", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        internal void UpdateVoucher(IList list, JournalVoucher voucher)
        {
            JournalVoucherRepo repo = new JournalVoucherRepo();
            List<JournalTransaction> transactions = new List<JournalTransaction>();
            transactions.AddRange(GetUpdatedVoucherTransactions(list, voucher));
            voucher.journalTransactions = transactions;
            voucher.statusId = voucher.JournalVoucherStatus.Id;
            voucher.user = null;
            voucher.JournalVoucherStatus = null;
            repo.UpdateVoucher(voucher);
            DXMessageBox.Show("Journal Voucher Updated Succesfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        internal void UpdateVoucher(JournalVoucher voucher)
        {
            JournalVoucherRepo repo = new JournalVoucherRepo();
            JournalTransaction trans = new JournalTransaction();
            List<JournalTransaction> transactions = new List<JournalTransaction>();
            foreach (JournalTransaction transaction in voucher.journalTransactions)
            {
                trans = new JournalTransaction();
                trans.accountId = transaction.account.Id;
                trans.coaTransactionsType = coaTransactionsType.JV;
                trans.creationDate = voucher.postingDate;
                trans.credit = transaction.credit;
                trans.debit = transaction.debit;
                trans.isAdjustment = transaction.isAdjustment;
                trans.memo = transaction.memo;
                trans.transactionRefno = transaction.transactionRefno;
                trans.userId = SYSTEM_STATIC.currentUser.id;
                transactions.Add(trans);
            }
            //repo.UpdateVoucher(voucher,transactions);
        }


        public List<JournalTransaction> GetVoucherTransactions(IList list, JournalVoucher voucher)
        {
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();


            foreach (JournalTransaction transaction in list)
            {
                double total = 0;
                if (
                    transaction.account.accountType == COA_AccountType.Loan ||
                    transaction.account.accountType == COA_AccountType.Credit_Card ||
                    transaction.account.accountType == COA_AccountType.Equity ||
                    transaction.account.accountType == COA_AccountType.Accounts_Payable ||
                    transaction.account.accountType == COA_AccountType.Longterm_Liability ||
                    transaction.account.accountType == COA_AccountType.Other_Current_Liability ||
                    transaction.account.accountType == COA_AccountType.Longterm_Liability 
                    )
                {
                    total = Math.Round((transaction.credit - transaction.debit), 2);
                }
                else
                {
                    total = Math.Round((transaction.debit - transaction.credit), 2);
                }
                JournalTransaction trans = new JournalTransaction();
                trans.accountId = transaction.account.Id;
                //trans.account = transaction.account;
                trans.coaTransactionsType = coaTransactionsType.JV;
                trans.creationDate = voucher.postingDate;
                trans.credit = transaction.credit;
                trans.debit = transaction.debit;
                trans.isAdjustment = transaction.isAdjustment;
                trans.memo = transaction.memo;
                trans.transactionRefno = transaction.transactionRefno;
                trans.userId = SYSTEM_STATIC.currentUser.id;
                trans.MER = Math.Round(Convert.ToDouble(voucher.MER), 2);
                trans.companyId = voucher.company_Id;
                trans.currencyId = voucher.currencyId;
                trans.total = total;
                //trans.MER = transaction.MER;
                trans.deptId = voucher.dept_Id;
                if (transaction.debit != 0)
                {
                    //trans.AmountMER = transaction.debit * transaction.MER;
                }
                else
                if (transaction.credit != 0)
                {
                    //trans.AmountMER = -transaction.credit * transaction.MER;
                }
                if (transaction.account.JournalTransactions.Count == 0)
                    transaction.account.isOpeningBalance = true;
                journalTransactions.Add(trans);
            }
            return journalTransactions;
        }


        public List<JournalTransaction> GetUpdatedVoucherTransactions(IList list, JournalVoucher voucher)
        {
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();


            foreach (JournalTransaction transaction in list)
            {
                double total= 0;

                if (
                    transaction.account.accountType == COA_AccountType.Loan ||
                    transaction.account.accountType == COA_AccountType.Credit_Card ||
                    transaction.account.accountType == COA_AccountType.Equity ||
                    transaction.account.accountType == COA_AccountType.Accounts_Payable ||
                    transaction.account.accountType == COA_AccountType.Longterm_Liability ||
                    transaction.account.accountType == COA_AccountType.Other_Current_Liability ||
                    transaction.account.accountType == COA_AccountType.Longterm_Liability
                    )
                {
                    total = Math.Round((transaction.credit - transaction.debit), 2);
                }
                else
                {
                    total = Math.Round((transaction.debit - transaction.credit), 2);
                }

                JournalTransaction trans = new JournalTransaction();
                trans.Id = transaction.Id;
                trans.accountId = transaction.account.Id;
                //trans.account = transaction.account;
                trans.coaTransactionsType = coaTransactionsType.JV;
                trans.creationDate = voucher.postingDate;
                trans.reconcilationType = transaction.reconcilationType;
                trans.credit = transaction.credit;
                trans.debit = transaction.debit;
                trans.isAdjustment = transaction.isAdjustment;
                trans.memo = transaction.memo;
                trans.deptId = voucher.dept_Id; 
                trans.transactionRefno = transaction.transactionRefno;
                trans.userId = SYSTEM_STATIC.currentUser.id;
                trans.reconcilationDate = transaction.reconcilationDate;
                trans.isReconciled = transaction.isReconciled;
                if (transaction.account.JournalTransactions.Count == 0)
                    transaction.account.isOpeningBalance = true;
                trans.journalVoucher_id = voucher.Id;
                trans.MER = Math.Round(Convert.ToDouble(voucher.MER), 2);
                trans.companyId = voucher.company_Id;
                trans.currencyId = voucher.currencyId;
                trans.total = total;
                journalTransactions.Add(trans);
            }
           
            return journalTransactions;
        }
        public void PopulateSaleInvoice(TransactionItemType type, int saleInvoiceId)
        {
            if (saleInvoiceId != 0)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(type, saleInvoiceId);
                procurmentPanel.Show();

            }
        }
        public void PopulatePurchaseInvoice(TransactionItemType type, int pourchaseInvoiceId)
        {


            if (pourchaseInvoiceId!=0)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Invoice, pourchaseInvoiceId);
                procurmentPanel.Show();

            }
        }
        public void PopulateBill(TransactionItemType type, int bill_Id)
        {
            if (bill_Id != 0)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Bill, bill_Id);
                procurmentPanel.Show();

            }
        }
        public void PopulateSaleSaleReceipt(int saleReceiptId)
        {
            if (saleReceiptId != 0)
            {
                try
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                    {
                        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                        SalesReceiptRepo repo = new SalesReceiptRepo();

                        updateSaleReceiptObj.saveEditFlag = 1;
                        var saleReceipt = repo.GetSalesReceipt(saleReceiptId);
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

                        
                        updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
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
        }
        public void PopulateAdminBills(int adminBillId)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill") != null)
            {
                AdminBillsRepo billsRepo = new AdminBillsRepo();
                ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                Window frmBill = new Window();
                frmBill.WindowState = WindowState.Maximized;
                frmBill.Title = "Update Bills";
                var selectedRow = billsRepo.GetBill(adminBillId);
                if (selectedRow != null)
                {
                    frmBillAdd.bills = new List<AdminBill>();
                    frmBillAdd.bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                    frmBillAdd.editFlag = true;
                    frmBillAdd.groupId = selectedRow.transactionGroupId;
                    frmBill.Content = frmBillAdd;
                    frmBill.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Existing Bill!");
            }
        }

      
        public void PopulateJV(JournalTransaction transaction)
        {
            try
            {
                JournalVoucherRepo repo = new JournalVoucherRepo();
                //var voucher = repo.GetVoucherbyId(transaction.journalVoucher.Id);
                var journalWin = new JournalEntries(transaction.journalVoucher);
                journalWin.Show();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        public void PopulateInterBankTransfer(JournalTransaction transaction)
        {
            try
            {
                InterBankTransRepo repo = new InterBankTransRepo();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                {

                    var selectedBankTransfer = transaction.InterBank;

                    if (selectedBankTransfer != null)
                    {
                        ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                        CompanyRepo compRepo = new CompanyRepo();
                        bankTransRepo = new InterBankTransRepo();

                        //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                        ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdInterBankTransfer.SelectedItem as InterBankTransfer;
                        var bankTransfer = bankTransRepo.GetInterBankTransfer(selectedBankTransfer.Id);

                        if (bankTransfer.interBankTransStatus.isActive == false)
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
        public void PopulatePayments(Payment selectedPayment)
        {
            if (selectedPayment != null)
            {
                CompanyRepo compRepo = new CompanyRepo();
                switch (selectedPayment.transactionType)
                {
                    case PaymentTransactionType.Admin_Bills:

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") != null)
                        {
                            ucFrmPayments frmPayments = new ucFrmPayments();

                            paymentRepo = new PaymentRepo();

                           // frmPayments = new ucFrmPayments();
                            var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                            
                                if (pymnt.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = pymnt.transactionGroupId;
                                        frmPayments.frmPaymentWindow.Content = frmPayments;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPayments.frmPaymentWindow.Title = "Payments";
                                        frmPayments.frmPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPayments.editFlag = true;
                                    frmPayments.groupId = selectedPayment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }
                            
                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Admin Bill Payments!");
                        }
                        break;
                    case PaymentTransactionType.Vendor_Bills:

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") != null)
                        {
                            ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd();

                            paymentRepo = new PaymentRepo();
                            var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                            if (pymnt != null)
                            {
                                if (pymnt.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmBillPayments.editFlag = true;
                                        frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                        frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;

                                        frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                        frmBillPayments.frmBillPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmBillPayments.editFlag = true;
                                    frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                    frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;
                                    frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                    frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                    frmBillPayments.frmBillPaymentWindow.Show();
                                }
                            }
                        }
                        break;
                    case PaymentTransactionType.Purchase_Invoice:

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Payment") != null)
                        {
                            ucFrmPInvoicePaymentAdd frmPIpayment = new ucFrmPInvoicePaymentAdd();

                            paymentRepo = new PaymentRepo();
                            var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                            if (pymnt != null)
                            {
                                if (pymnt.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPIpayment.editFlag = true;
                                        frmPIpayment.groupId = pymnt.transactionGroupId;
                                        frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;

                                        frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmPIpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPIpayment.editFlag = true;
                                    frmPIpayment.groupId = pymnt.transactionGroupId;
                                    frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                    frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmPIpayment.frmPiPaymentWindow.Show();
                                }
                            }
                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                        }


                        break;
                    case PaymentTransactionType.Loans_Advances:

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                        {
                            ucFrmLoansAdvancePaymentAdd frmPIpayment = new ucFrmLoansAdvancePaymentAdd();

                            paymentRepo = new PaymentRepo();
                            var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId); 

                            if (pymnt != null)
                            {
                                if (pymnt.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPIpayment.editFlag = true;
                                        frmPIpayment.groupId = pymnt.transactionGroupId;
                                        frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;

                                        frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmPIpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPIpayment.editFlag = true;
                                    frmPIpayment.groupId = pymnt.transactionGroupId;
                                    frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                    frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmPIpayment.frmPiPaymentWindow.Show();
                                }
                            }
                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                        }


                        break;
                }


            }
        }
        public void PopulateTargetReward(JournalTransaction transaction)
        {
            ToDoTaskRepo toDoTaskRepo = new ToDoTaskRepo();

            var reward = toDoTaskRepo.GetTargetReward((int)transaction.TargetRewardId);
            if (reward != null)
            {
                if (reward.isApplied == false)
                {
                    ucFrmBasicTargetRewards ucFrmTarget = new ucFrmBasicTargetRewards();
                    ucFrmTarget.rewardId = reward.Id;

                    DXWindow win = new DXWindow();
                    win.WindowState = WindowState.Maximized;
                    win.Content = ucFrmTarget;
                    win.Title = "Target Reward";
                    win.Show();
                }
                else if (reward.isApplied == true)
                {
                    ucFrmTargetRewardAdd ucFrmTarget = new ucFrmTargetRewardAdd();
                    ucFrmTarget.rewardId = reward.Id;
                    DXWindow win = new DXWindow();
                    win.WindowState = WindowState.Maximized;
                    win.Content = ucFrmTarget;
                    win.Title = "Target Reward";
                    win.Show();
                }
            }
        }
        public void PopulateInterCompanyTransfer(JournalTransaction transaction)
        {
          
                InterCompanyBankTransferRepo transferRepo = new InterCompanyBankTransferRepo();
                try
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                    {
                        var selectedBankTransfer = transferRepo.GetInterCompanyBankTransfer((int)transaction.InterCompanyId);

                        if (selectedBankTransfer != null)
                        {
                            ucBankTransferInterCompany ucFrmBankTransfer = new ucBankTransferInterCompany();
                            CompanyRepo compRepo = new CompanyRepo();
                            

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
        public void PopulatePayment(JournalTransaction transaction)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                {

                    if (transaction.InterCompanyId != null)
                    {

                        ucBankTransferInterCompany ucFrmBankTransfer = new ucBankTransferInterCompany();
                        CompanyRepo compRepo = new CompanyRepo();
                        InterCompanyBankTransferRepo transferRepo = new InterCompanyBankTransferRepo();

                        //ucFrmBankTransfer.bankTransfer = new ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer();
                        var bankTransfer = transferRepo.GetInterCompanyBankTransfer((int)transaction.InterCompanyId);

                        if (bankTransfer.interBankTransStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                            {
                                ucFrmBankTransfer.editFlag = true;
                                ucFrmBankTransfer.bankTransferId = transaction.InterCompanyId.Value;
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
    }
}
