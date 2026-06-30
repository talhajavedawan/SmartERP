using DevExpress.Xpf.Core;
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
using System.Windows.Shapes;
using ERP_BL.Procurements.InterBankTransfers;
using ZAS_ERP.Procurementss;
using ERP_BL.ExchangeRates;
using ERP_BL.Tax;
using DevExpress.Office.Utils;
using DevExpress.CodeParser;
using Microsoft.Win32;
using System.Diagnostics;
using ERP_BL.Payments;
using ERP_BL.ChartofAccounts;
using ZAS_ERP.Bankings.UserControls;
using ZAS_ERP.Payments.UserControls.CompanyLoanPayments;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.Windows;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ERP_BL.Procurements;
using ZAS_ERP.Bankings;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ERP_BL.Payments;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.AdminBills;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;

namespace ZAS_ERP.Bankings.STL
{
    /// <summary>
    /// Interaction logic for winSTLAdd.xaml
    /// </summary>
    public partial class winSTLAdd : DXWindow
    {
        SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
        public static int stlId;

        public int paymentGroupId=0;
        public double paymentOC { get; set; }
        public Currency paymentCurrency { get; set; }

        public static bool editFlag { get; set; }
        public ERP_BL.Procurements.InterBankTransfers.STL stl = new ERP_BL.Procurements.InterBankTransfers.STL();
        public STLStatus checkStatus = new STLStatus();
        STLStatus oldStatus = new STLStatus();

        STLRepo stlRepo = new STLRepo();
        List<STLSettlement> settlements = new List<STLSettlement>();
        List<ReversalSettlement> reversalSettlements = new List<ReversalSettlement>();
        List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
        PaymentRepo paymentRepo = new PaymentRepo();
        Payment payment= new Payment();
        UsersRepo _usersRepo = new UsersRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        List<STLStatus> STLStatuses = new List<STLStatus>();

        public winSTLAdd()
        {
            InitializeComponent();
        }

        public winSTLAdd(bool _editFlag, int _paymentGroupId)
        {
            InitializeComponent();
            editFlag = _editFlag;
            paymentGroupId = _paymentGroupId;
            payment = paymentRepo.GetPaymentByGroupId(_paymentGroupId);
            settlements = new List<STLSettlement>();
            grdCntrlSettlementList.ItemsSource = settlements;
            reversalSettlements = new List<ReversalSettlement>();
            grdCntrlMarginReversalList.ItemsSource = reversalSettlements;

        }
        public winSTLAdd(int _paymentGroupId, double paymentOC, Currency paymentCurrency)
        {
            InitializeComponent();
            payment = paymentRepo.GetPaymentByGroupId(_paymentGroupId);
            paymentGroupId = _paymentGroupId;
            this.paymentOC = paymentOC;
            this.paymentCurrency = paymentCurrency;
            txtCreator.Text = SYSTEM_STATIC.currentUser.userName;
            datCreationDate.EditValue = DateTime.Now;
            settlements = new List<STLSettlement>();
            grdCntrlSettlementList.ItemsSource = settlements;
            reversalSettlements = new List<ReversalSettlement>();
            grdCntrlMarginReversalList.ItemsSource = reversalSettlements;

        }
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCompanies();
            LoadCurrencies();
            LoadInterests();
            LoadMarginPerc();
            loadSTLStatus();
            LoadSTLData();
            GellAllOrdersTracking();
        }
        public void LoadInterests()
        {
            TaxRepo taxRepo = new TaxRepo();
            lookupSTLInterest.ItemsSource = taxRepo.getAllInterests();
        }
        public void LoadMarginPerc()
        {
            TaxRepo taxRepo = new TaxRepo();
            lookupCashMarginPerc.ItemsSource = taxRepo.getAllMarginPerc();
        }
        public void LoadSTLData()
        {
            grdCntrlSettlementList.ItemsSource = settlements;
            grdCntrlMarginReversalList.ItemsSource = reversalSettlements;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void STL") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void STL") != null)
            {
                btnSetVoid.Visibility = Visibility.Visible;
            }
            else
            {
                btnSetVoid.Visibility = Visibility.Collapsed;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit GL-Posting Date") != null)
            {
                datGLPostingDate.IsReadOnly = false;
            }
            if (editFlag==false)
            {
                txtPaymentAmountOC.Text = paymentOC.ToString();
                lookupPaymentCurrency.EditValue = paymentCurrency.CurrencyName.ToString();
                datCreationDate.EditValue = DateTime.Now;
                datGLPostingDate.EditValue = DateTime.Now;
                if(payment!=null)
                {
                    txtSalesRef.Text = payment.PaymentRefNo;
                }
                if (payment.company_Id != null)
                {
                    lookupCompany.Text = payment.company.CompanyName;
                }
                if (payment.departments.Count == 1)
                {
                    lookupDepartment.Text = payment.departments[0].DeptName;
                }
                if (payment.currency_Id!=null)
                {
                    lookupPaymentCurrency.Text = payment.currency.CurrencyName;
                }
                btnPushCredits.IsEnabled = true;
                btnPushCredits.IsEnabled = true;
            }
            else
            {
                if (paymentGroupId != 0)
                {
                    stl = stlRepo.GetbyGroupId(paymentGroupId);
                }
                else
                {
                    stl = stlRepo.Get(stl.Id);
                }
                if (stl.Settlements.Count==0)
                {
                    txtBalSettAmountSTL.Text = stl.stlPaymentAmountOC.ToString();
                }
                grdCntrlSettlementList.ItemsSource = stl.Settlements;
                grdCntrlMarginReversalList.ItemsSource = stl.ReversalSettlements;
                checkStatus = stl.stlStatus;

                var creditJournalTransactions = stl.journalTransactions.Where(x => x.credit != 0).ToList();
                var debitJournalTransactions = stl.journalTransactions.Where(x => x.debit != 0).ToList();
                if (creditJournalTransactions.Count > 0)
                {
                    btnPushCredits.IsChecked = true;
                }
                if (debitJournalTransactions.Count > 0)
                {
                    btnPushDebits.IsChecked = true;
                }

                if (stl.isVoid == true)
                {
                    //grdVoid.Visibility = Visibility.Visible;
                    //txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                else if (stl.isReApproved == false)
                {
                    //lblStage.Text = "Under Re-Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (stl.isApproved == true && stl.stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (stl.isApproved == true && stl.stlStatus.isActive == false && stl.PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (stl.isApproved == true && stl.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (stl.isApproved == true)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (stl.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (stl.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                lookupCompany.Text = stl.company.CompanyName;

                if(stl.cashMarginCrAccount_Id!=null)
                {
                    lookupCashMarginCrAccount.Text = stl.cashMarginCrAccount.AccountNo;
                }
                lookupDepartment.Text = stl.department.DeptName;
                datCreationDate.EditValue = stl.CreationDate;
                txtCreator.Text = stl.user.userName;
                lookupPaymentBank.Text = stl.paymentBank.BankName;
                lookupPaymnetAccount.Text = stl.paymentAccount.AccountNo;
                lookupPaymentCurrency.Text = stl.paymentCurrency.CurrencyName;
                txtPaymentAmountOC.Text = stl.paymentAmountOC.ToString();
                cmbSTLStatus.Text = stl.stlStatus.Status;


                datSTLPaymentDate.EditValue = stl.stlPaymentDate;
                txtCashCT.Text = stl.creditTenureNo.ToString();
                txtCashECT.Text = stl.extendedCreditTenureNo.ToString();
                lookupSTLBank.Text = stl.stlBank.BankName;
                lookupSTLAccount.Text = stl.stlAccount.AccountNo;
                lookupSTLCurrency.Text = stl.stlCurrency.CurrencyName;
                txtSettAmountSTL.Text = stl.settlmentAmount.ToString();
               
                if(stl.GLPostingDate!=null)
                {
                    datGLPostingDate.EditValue = stl.GLPostingDate;
                }
                if(!string.IsNullOrEmpty(stl.salesReferenceNo))
                {
                    txtSalesRef.Text = stl.salesReferenceNo;
                }
                lookupCashMarginBank.Text = stl.cashMarginbank.BankName;
                lookupCashMarginDrAccount.Text = stl.cashMarginDrAccount.AccountNo;
                txtCashMarginAmount.Text = stl.cashMarginAmount.ToString();

                txtSettAmountSTL.Text = stl.settlmentAmount.ToString();
                txtBalSettAmountSTL.Text = stl.settlmentBalance.ToString();
                txtCashReversal.Text = stl.marginReversal.ToString();
                lookupInterestCurrency.Text = stl.interestAmountCurrency.CurrencyName;
                if (stl.paymentMaturityDate != null)
                {
                    datPaymentMaturityDate.EditValue = stl.paymentMaturityDate;
                }
                if (!string.IsNullOrEmpty(stl.stlRef))
                {
                    txtstlRef.Text = stl.stlRef;
                }
                if (stl.extendedCreditTenure != null)
                {
                    datExtendedDate.EditValue = stl.extendedCreditTenure;
                }
                if (stl.stlStatus != null)
                {
                    var disAbleStatus = STLStatuses.FirstOrDefault(x => x.Id == stl.stlStatus.Id);
                    if (disAbleStatus == null)
                    {
                        loadSTLStatus(stl.stlStatus);
                    }
                }

                foreach (cmbitem cmbitem in cmbSTLStatus.Items)
                {
                    if (cmbitem.name == stl.stlStatus.Status)
                    {
                        cmbSTLStatus.SelectedItem = cmbitem;
                        break;
                    }
                }
                if(stl.interest!=null)
                {
                    lookupSTLInterest.Text = stl.interest.Name.ToString();
                }
                if (stl.cashMarginPerc != null)
                {
                    lookupCashMarginPerc.Text = stl.cashMarginPerc.Name.ToString();
                }
                if (stl.cashMarginCurrency_Id!=null)
                {
                    lookupCashMarginCurrency.Text = stl.cashMarginCurrency.CurrencyName;
                }
                txtPaymentAmountSTL.Text = stl.stlPaymentAmountOC.ToString();
            }

        }
        public void LoadCurrencies()
        {
            CurrencyRepo currecnyRepo = new CurrencyRepo();
            lookupSTLCurrency.ItemsSource= currecnyRepo.getAll();
            lookupPaymentCurrency.ItemsSource = currecnyRepo.getAll();
            lookupInterestCurrency.ItemsSource = currecnyRepo.getAll();
            lookupCashMarginCurrency.ItemsSource = currecnyRepo.getAll();
        }
        public void LoadCompanies()
        {
          lookupCompany.ItemsSource=  SYSTEM_STATIC.LoadCurrentUserCompanies();
        }
        public void LoadDepartments(Company _company)
        {
            lookupDepartment.ItemsSource = _company.departments;
            lookupCustomer.ItemsSource = _company.Customers;
            lookupDepartment.ItemsSource = _company.departments;

        }
        private void lookupCompany_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var company = lookupCompany.SelectedItem as Company;
            LoadDepartments(company);
            LoadBanks(company);
        }
       public void LoadBanks(Company _company)
        {
            if (_company != null)
            {
                    if (_company.departments != null)
                    {
                        List<Department> departments = new List<Department>();
                        foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsInterBankTransferType == true))
                        {
                          if (_dept.companies.FirstOrDefault(x => x.Id == _company.Id) != null)
                              departments.Add(_dept);
                        }
                    }
                var bankList = receiptRepo.GetAllBanksByCompany(_company.Id);
                lookupSTLBank.ItemsSource = bankList;
                lookupPaymentBank.ItemsSource = bankList;
                lookupCashMarginBank.ItemsSource = bankList;
            }
        }

        private void lookupPaymentBank_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var bank = lookupPaymentBank.SelectedItem as Bank;

            LoadPaymentBankAccounts(bank);
        }
        public void LoadPaymentBankAccounts(Bank _bank)
        {
            var department = lookupDepartment.SelectedItem as Department;
            if (_bank != null)
            {
                var accntList = receiptRepo.GetAllAccountsByBankId(_bank.Id).Where(x => x.isActive == true).ToList();
                List<Account> allowedAccounts = new List<Account>();

                foreach (var _account in accntList)
                {
                    List<int> dept_ids = new List<int>();

                    foreach (var _dept in _account.departments)
                    {
                        dept_ids.Add(_dept.Id);
                    }
                    if (dept_ids.Contains(department.Id) && _account.bank.Id == _bank.Id && _account.company.Id == (lookupCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal))
                        allowedAccounts.Add(_account);
                }
                lookupPaymnetAccount.ItemsSource = allowedAccounts;
            }
        }
        public void LoadSTLBankAccounts(Bank _bank)
        {
            var department = lookupDepartment.SelectedItem as Department;
            if (_bank != null)
            {
                var accntList = receiptRepo.GetAllAccountsByBankId(_bank.Id).Where(x => x.isActive == true).ToList();
                List<Account> allowedAccounts = new List<Account>();

                foreach (var _account in accntList)
                {
                    List<int> dept_ids = new List<int>();

                    foreach (var _dept in _account.departments)
                    {
                        dept_ids.Add(_dept.Id);
                    }
                    if (dept_ids.Contains(department.Id) && _account.bank.Id == _bank.Id && _account.company.Id == (lookupCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal))
                        allowedAccounts.Add(_account);
                }
                lookupSTLAccount.ItemsSource = allowedAccounts;
            }
        }

        public void LoadCashMarginBankAccounts(Bank _bank)
        {
            var department = lookupDepartment.SelectedItem as Department;
            if (_bank != null)
            {
                var accntList = receiptRepo.GetAllAccountsByBankId(_bank.Id).Where(x => x.isActive == true).ToList();
                List<Account> allowedAccounts = new List<Account>();

                foreach (var _account in accntList)
                {
                    List<int> dept_ids = new List<int>();

                    foreach (var _dept in _account.departments)
                    {
                        dept_ids.Add(_dept.Id);
                    }
                    if (dept_ids.Contains(department.Id) && _account.bank.Id == _bank.Id && _account.company.Id == (lookupCompany.SelectedItem as Company).Id && (_account.accountsCategory == AccountsCategory.Company || _account.accountsCategory == AccountsCategory.Personal))
                        allowedAccounts.Add(_account);
                }
                lookupCashMarginDrAccount.ItemsSource = allowedAccounts;
                lookupCashMarginCrAccount.ItemsSource = allowedAccounts;
            }
        }
        private void lookupSTLBank_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var bank = lookupPaymentBank.SelectedItem as Bank;

            LoadSTLBankAccounts(bank);
        }

        private void lookupCashMarginBank_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var bank = lookupPaymentBank.SelectedItem as Bank;

            LoadCashMarginBankAccounts(bank);
        }

        public List<JournalTransaction> getJournalTransactions()
        {
            journalTransactions = new List<JournalTransaction>();
            Department department = lookupDepartment.SelectedItem as  Department;
                if (btnPushDebits.IsChecked == true)
                {
                    if (lookupCashMarginDrAccount.SelectedIndex > -1)
                    {
                        var debitAccount = lookupCashMarginDrAccount.SelectedItem as Account;
                        if (debitAccount.COA_accountId != null)
                        {
                            JournalTransaction cashMarginDrTransaction = new JournalTransaction();
                            double total = 0;
                            if (
                                   debitAccount.COAaccount.accountType == COA_AccountType.Loan ||
                                   debitAccount.COAaccount.accountType == COA_AccountType.Credit_Card ||
                                   debitAccount.COAaccount.accountType == COA_AccountType.Equity ||
                                   debitAccount.COAaccount.accountType == COA_AccountType.Accounts_Payable ||
                                   debitAccount.COAaccount.accountType == COA_AccountType.Longterm_Liability ||
                                   debitAccount.COAaccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   debitAccount.COAaccount.accountType == COA_AccountType.Other_Income ||
                                   debitAccount.COAaccount.accountType == COA_AccountType.Income
                                   )
                            {
                                total = 0 - Convert.ToDouble(txtCashMarginAmount.Text);
                            }
                            else
                            {
                                total = Convert.ToDouble(txtCashMarginAmount.Text);
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                accountId = debitAccount.COA_accountId,
                                coaTransactionsType = coaTransactionsType.STL,
                                STLId = stl.Id,
                                creationDate = (DateTime)datGLPostingDate.EditValue,
                                //memo = procurementProduct.inquiryProduct.ownDiscription,
                                transactionRefno = txtstlRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = total,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                credit = 0,
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (lookupCashMarginCurrency.SelectedItem as Currency).Id

                            });
                        }

                    }

                    if (lookupPaymnetAccount.SelectedIndex > -1)
                    {
                        var debitAccount = lookupPaymnetAccount.SelectedItem as Account;
                        if (debitAccount.COA_accountId != null)
                        {
                            double total = 0;
                        if (
                               debitAccount.COAaccount.accountType == COA_AccountType.Loan ||
                               debitAccount.COAaccount.accountType == COA_AccountType.Credit_Card ||
                               debitAccount.COAaccount.accountType == COA_AccountType.Equity ||
                               debitAccount.COAaccount.accountType == COA_AccountType.Accounts_Payable ||
                               debitAccount.COAaccount.accountType == COA_AccountType.Longterm_Liability ||
                               debitAccount.COAaccount.accountType == COA_AccountType.Other_Current_Liability ||
                               debitAccount.COAaccount.accountType == COA_AccountType.Other_Income ||
                               debitAccount.COAaccount.accountType == COA_AccountType.Income
                               )
                        {
                            total = 0 - Convert.ToDouble(txtPaymentAmountOC.Text);

                        }
                        else
                        {
                            total = Convert.ToDouble(txtPaymentAmountOC.Text);

                        }
                        journalTransactions.Add(new JournalTransaction()
                            {
                                accountId = debitAccount.COA_accountId,
                                coaTransactionsType = coaTransactionsType.STL,
                                STLId = stl.Id,
                                creationDate = (DateTime)datGLPostingDate.EditValue,
                                //memo = procurementProduct.inquiryProduct.ownDiscription,
                                transactionRefno = txtstlRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = total,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                credit = 0,
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (lookupPaymentCurrency.SelectedItem as Currency).Id

                            });
                        }
                    }
                }
                if (btnPushCredits.IsChecked == true)
                {
                    if (lookupCashMarginDrAccount.SelectedIndex > -1)
                    {
                        var creditAccount = lookupCashMarginDrAccount.SelectedItem as Account;
                        if (creditAccount.COA_accountId != null)
                        {
                            double total = 0;
                            if (
                                  creditAccount.COAaccount.accountType == COA_AccountType.Loan ||
                                  creditAccount.COAaccount.accountType == COA_AccountType.Credit_Card ||
                                  creditAccount.COAaccount.accountType == COA_AccountType.Equity ||
                                  creditAccount.COAaccount.accountType == COA_AccountType.Accounts_Payable ||
                                  creditAccount.COAaccount.accountType == COA_AccountType.Longterm_Liability ||
                                  creditAccount.COAaccount.accountType == COA_AccountType.Other_Current_Liability ||
                                  creditAccount.COAaccount.accountType == COA_AccountType.Other_Income ||
                                  creditAccount.COAaccount.accountType == COA_AccountType.Income
                                  )
                            {
                                total = Convert.ToDouble(txtCashMarginAmount.Text);
                            }
                            else
                            {
                                total = 0 - Convert.ToDouble(txtCashMarginAmount.Text);
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                accountId = creditAccount.COA_accountId,
                                coaTransactionsType = coaTransactionsType.STL,
                                STLId = stl.Id,
                                creationDate = (DateTime)datGLPostingDate.EditValue,
                                //memo = procurementProduct.inquiryProduct.ownDiscription,
                                transactionRefno = txtstlRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                credit = total,
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (lookupCashMarginCurrency.SelectedItem as Currency).Id

                            });
                        }
                    }
                    if (lookupSTLAccount.SelectedIndex > -1)
                    {
                        var creditSTLAccount = lookupSTLAccount.SelectedItem as Account;

                        if (creditSTLAccount.COA_accountId != null)
                        {

                            double total = 0;
                            if (
                                   creditSTLAccount.COAaccount.accountType == COA_AccountType.Loan ||
                                   creditSTLAccount.COAaccount.accountType == COA_AccountType.Credit_Card ||
                                   creditSTLAccount.COAaccount.accountType == COA_AccountType.Equity ||
                                   creditSTLAccount.COAaccount.accountType == COA_AccountType.Accounts_Payable ||
                                   creditSTLAccount.COAaccount.accountType == COA_AccountType.Longterm_Liability ||
                                   creditSTLAccount.COAaccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   creditSTLAccount.COAaccount.accountType == COA_AccountType.Other_Income ||
                                   creditSTLAccount.COAaccount.accountType == COA_AccountType.Income
                                   )
                            {
                                total = Convert.ToDouble(txtPaymentAmountSTL.Text);
                            }
                            else
                            {
                                total = 0 - Convert.ToDouble(txtPaymentAmountSTL.Text);
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                accountId = creditSTLAccount.COA_accountId,
                                coaTransactionsType = coaTransactionsType.STL,
                                STLId = stl.Id,
                                creationDate = (DateTime)datGLPostingDate.EditValue,
                                //memo = procurementProduct.inquiryProduct.ownDiscription,
                                transactionRefno = txtstlRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = 0,
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                credit = total,
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (lookupSTLCurrency.SelectedItem as Currency).Id

                            });
                        }
                    }
                }
            return journalTransactions;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (lookupCompany.SelectedIndex == -1 )
                {
                    lookupCompany.Focus();
                    MessageBox.Show("Please Select a company against STL", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                if (lookupDepartment.SelectedIndex == -1 )
                {
                    lookupDepartment.Focus();
                    MessageBox.Show("Please Select a Department against STL", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                if (lookupPaymentBank.SelectedIndex == -1 )
                {
                    MessageBox.Show("Please Select a payment Bank", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupPaymentBank.Focus();
                    return;
                }
                if (lookupPaymnetAccount.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select a Payment Account", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupPaymnetAccount.Focus();
                    return;
                }
                if (lookupPaymentCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select a Payment Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupPaymentCurrency.Focus();
                    return;
                }


                if (lookupSTLBank.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select a STL Bank", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupSTLBank.Focus();
                    return;
                }
                if (lookupSTLAccount.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select a STL Account", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupSTLAccount.Focus();
                    return;
                }
                if (lookupSTLCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select a STL Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupSTLCurrency.Focus();
                    return;
                }
                if (datPaymentMaturityDate.EditValue == null)
                {
                    MessageBox.Show("Please Select a Payment Maturity Date", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    datPaymentMaturityDate.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtstlRef.Text))
                {
                    MessageBox.Show("Please input STL reference nunmber", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    datPaymentMaturityDate.Focus();
                    return;
                }
                if (lookupCashMarginCrAccount.SelectedIndex==-1)
                {
                    MessageBox.Show("Please select cash margin credit account", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCashMarginCrAccount.Focus();
                    return;
                }
                if (lookupCashMarginDrAccount.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select cash margin debit account", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCashMarginDrAccount.Focus();
                    return;
                }

                CompanyRepo companyRepo = new CompanyRepo();
                var company=companyRepo.GetCompany((lookupCompany.SelectedItem as Company).Id);
                DepartmentRepo departmentRepo = new DepartmentRepo();
                var department = departmentRepo.get((lookupDepartment.SelectedItem as Department).Id);

//Basic Information

                stl.company_Id = company.Id;
                stl.dept_Id = department.Id;
                stl.CreationDate = (DateTime)datCreationDate.EditValue;
                stl.paymentBank_Id = (lookupPaymentBank.SelectedItem as Bank).Id;
                stl.paymentAccount_Id = (lookupPaymnetAccount.SelectedItem as Account).Id;
                stl.paymentCurrency_Id = (lookupPaymentCurrency.SelectedItem as Currency).Id;
                stl.paymentAmountOC = Convert.ToDouble(txtPaymentAmountOC.Text);
                stl.statusId = (cmbSTLStatus.SelectedItem as cmbitem).id;
                stl.stlRef = txtstlRef.Text;
                if(!string.IsNullOrEmpty(txtSalesRef.Text))
                {
                    stl.salesReferenceNo = txtSalesRef.Text;
                }
                if(lookupVendor.SelectedIndex>-1)
                {
                    stl.vendor_Id = (lookupVendor.SelectedItem as Vendor).Id;
                }
                if (lookupCustomer.SelectedIndex > -1)
                {
                    stl.customer_Id = (lookupCustomer.SelectedItem as CustomerCompany).Id;
                }


                //Payment Details
                stl.stlPaymentDate = (DateTime)datSTLPaymentDate.EditValue;
                stl.paymentMaturityDate = (DateTime)datPaymentMaturityDate.EditValue;
                stl.creditTenureNo =Convert.ToDouble(txtCashCT.Text);
                stl.extendedCreditTenureNo= Convert.ToDouble(txtCashECT.Text);
                stl.paymentDueDays = Convert.ToDouble(txtCashPDD.Text);



                stl.stlUtilizedDays = Convert.ToDouble(txtSTLUntilizedDays.Text);

                if(lookupSTLInterest.SelectedIndex>-1)
                stl.interestPercent = Convert.ToDouble((lookupSTLInterest.SelectedItem as STLInterest).percentage);
                stl.interestAmountCurrency_Id = (lookupInterestCurrency.SelectedItem as Currency).Id;
                if(datExtendedDate.EditValue!=null)
                stl.extendedCreditTenure = (DateTime)datExtendedDate.EditValue ;
             
                if(!string.IsNullOrEmpty(txInterstAmountTDSTL.Text))
                {
                    stl.InterestAmount = Math.Round(Convert.ToDouble(txInterstAmountTDSTL.Text), 2);
                }
                if (!string.IsNullOrEmpty(txInterstAmountCDSTL.Text))
                {
                    stl.InterestAmountCD = Math.Round(Convert.ToDouble(txInterstAmountCDSTL.Text), 2);
                }
                if (datGLPostingDate.EditValue!=null)
                {
                    stl.GLPostingDate = (DateTime)datGLPostingDate.EditValue;
                }
//STL Settlement 

                stl.settlmentAmount = Convert.ToDouble(txtSettAmountSTL.Text);
                stl.settlmentBalance = Convert.ToDouble(txtBalSettAmountSTL.Text);
//STL Payment
                stl.stlBank_Id = (lookupSTLBank.SelectedItem as Bank).Id;
                stl.stlCurrency_Id = (lookupSTLCurrency.SelectedItem as Currency).Id;
                stl.stlAccount_Id = (lookupSTLAccount.SelectedItem as Account).Id;
                stl.stlPaymentAmountOC = Convert.ToDouble(txtPaymentAmountSTL.Text);
                if(lookupSTLInterest.SelectedIndex>-1)
                {
                    stl.interest_Id = (lookupSTLInterest.SelectedItem as STLInterest).Id;
                }
//Cash Margin
                stl.cashMarginBank_Id = (lookupCashMarginBank.SelectedItem as Bank).Id;
                stl.cashMarginDrAccount_Id = (lookupCashMarginDrAccount.SelectedItem as Account).Id;
                stl.cashMarginCrAccount_Id = (lookupCashMarginCrAccount.SelectedItem as Account).Id;
                stl.cashMarginPercent = 0;
                stl.cashMarginAmount = Convert.ToDouble(txtCashMarginAmount.Text);

                //JournalTransactions
                stl.journalTransactions = getJournalTransactions();
                if(!string.IsNullOrEmpty(txtSER.Text))
                {
                    stl.SER = Convert.ToDouble(txtSER.Text);
                }
                if (!string.IsNullOrEmpty(txtMER.Text))
                {
                    stl.MER = Convert.ToDouble(txtMER.Text);
                }
                if (!string.IsNullOrEmpty(txtAmountSER.Text))
                {
                    stl.STLAmountSER = Convert.ToDouble(txtAmountSER.Text);
                }
                if (!string.IsNullOrEmpty(txtAmountMER.Text))
                {
                    stl.STLAmountMER = Convert.ToDouble(txtAmountMER.Text);
                }
                if(lookupCashMarginCurrency.SelectedIndex>-1)
                {
                    CurrencyRepo currencyRepo = new CurrencyRepo();
                    var currency = currencyRepo.get((lookupCashMarginCurrency.SelectedItem as Currency).Id);
                    stl.cashMarginCurrency_Id = currency.Id;
                }
                if (lookupCashMarginPerc.SelectedIndex > -1)
                {
                    stl.cashMarginPerc_Id = (lookupCashMarginPerc.SelectedItem as MarginPercentage).Id;
                }
//Margin Reversal
                stl.marginReversal = Convert.ToDouble(txtCashReversal.Text);


                if(grdCntrlSettlementList.VisibleItems.Count>0)
                {
                   foreach(var settlement in grdCntrlSettlementList.ItemsSource as List<STLSettlement>)
                    {
                        if(settlement.Id==0)
                        {
                            //STLSettlement stlSettlement = new STLSettlement();
                            //stlSettlement.Id = settlement.Id;
                            //stlSettlement.refNO = settlement.refNO;
                            //stlSettlement.settlementAmount = settlement.settlementAmount;
                            //stl.Settlements.Add(stlSettlement);
                        }
                        else
                        {
                            var stlSettlement= stl.Settlements.FirstOrDefault(x => x.Id == settlement.Id);
                            stlSettlement.Id = settlement.Id;
                            stlSettlement.refNO = settlement.refNO;
                            stlSettlement.settlementAmount = settlement.settlementAmount;
                        }
                    }
                }
                if (grdCntrlMarginReversalList.VisibleItems.Count > 0)
                {
                    foreach (var cashMargin in grdCntrlMarginReversalList.ItemsSource as List<ReversalSettlement>)
                    {
                        if (cashMargin.Id == 0)
                        {
                            //STLSettlement stlSettlement = new STLSettlement();
                            //stlSettlement.Id = settlement.Id;
                            //stlSettlement.refNO = settlement.refNO;
                            //stlSettlement.settlementAmount = settlement.settlementAmount;
                            //stl.Settlements.Add(stlSettlement);
                        }
                        else
                        {
                            var stlSettlement = stl.ReversalSettlements.FirstOrDefault(x => x.Id == cashMargin.Id);
                            stlSettlement.Id = cashMargin.Id;
                            stlSettlement.refNO = cashMargin.refNO;
                            stlSettlement.reversalSettlementAmount = cashMargin.reversalSettlementAmount;
                        }
                    }
                }




                var myWindow = Window.GetWindow(this);
                if (editFlag == true && stl.Id != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit STL") != null)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null && stl.isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This STL is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            stl.stage = TransactionStage.Approved.ToString();

                            stl.isApproved = true;
                            stl.ApprovedDate = System.DateTime.Now;
                        }
                    }
                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        if (checkStatus.Id != stl.stlStatus.Id)
                        {
                            stl.LastStatusChangeDate = System.DateTime.Now;
                            if (stl.stlStatus.isActive != true)
                            {
                                stl.ClosingDate = System.DateTime.Now;
                            }
                        }
                    }

                    stlRepo.update(stl);
                    if (checkStatus.Id != stl.stlStatus.Id)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of STL has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (lookupDepartment.SelectedItem as Department != null && (lookupDepartment.SelectedItem as Department).Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), stl.Id, TransactionItemType.STL);
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

                        string oldStat = checkStatus.Status;
                        string newStat = stl.stlStatus.Status;
                        string symbolCurr = "";

                        if (stl.stlCurrency != null)
                        {
                            symbolCurr = stl.stlCurrency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of STL having amount: " + stl.stlPaymentAmountOC.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(stl.Id, TransactionItemType.STL, comment, SYSTEM_STATIC.currentUser.employeeId);
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in STL #" + stl.salesReferenceNo, stl.Id, TransactionItemType.STL, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in STL #" + stl.salesReferenceNo, stl.Id, TransactionItemType.STL, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }

                    }

                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        UsersRepo.Add(TransactionInfo.Status_Changed, stl.Id, 2, "Status Changed from (" + checkStatus.Status + ") to (" + stl.stlStatus.Status + ")");
                    }
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Edited, stl.Id, 2, frmInputBox.comment);
                    DXMessageBox.Show("STL Updated Succesfully", "Congratulations");
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL") != null)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        MessageBox.Show("Please Create another Account to Create STL, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                        return;
                    }
                    stl.user_Id = MainWindow.currentUserid;
                  
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null /*&& offer.isApproved == false*/)
                    {
                        {

                            stl.stage = TransactionStage.Approved.ToString();

                            stl.isApproved = true;
                            stl.ApprovedDate = System.DateTime.Now;
                        }
                    }
                    else
                    {
                        stl.stage = TransactionStage.AwaitingFirstReview.ToString();
                        stl.isApproved = false;
                    }
                    stl.paymentGroupId = paymentGroupId;
                    stlRepo.Add(stl);
                    DXMessageBox.Show("STL Added Succesfully", "Congratulations");
                }
                else
                {
                    return;
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                    myWindow.Close();
                    return;
                }

                myWindow.Close();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());

                MessageBox.Show(ex.ToString());
            }
        }

        private void cmbSaleOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
        public void loadSTLStatus()
        {


            if (/*SystemLogic.AllowedPermissions.Find(x => x.Name == "View InActive Sale Order Statuses") != null ||*/ SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed STL") != null)
            {
                STLStatuses = stlRepo.getAllSTLStatus();
            }
            else
                STLStatuses = stlRepo.getAllActiveSTLStatus();
       
            STLStatuses= STLStatuses.Where(x=>x.isDisable!=true).ToList();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (STLStatus status in STLStatuses)
            {

                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
                //});
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbSTLStatus.ItemsSource = cmbitems;
        } 
        public void loadSTLStatus(STLStatus sTLStatus)
        {
            STLStatuses.Add(sTLStatus);
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (STLStatus status in STLStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbSTLStatus.ItemsSource = cmbitems;
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editFlag == true && stl != null)
                {

                    stlRepo = new STLRepo();
                    //stl = new ERP_BL.Procurements.InterBankTransfers.STL();
                    stl = stlRepo.Get(stl.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (stl != null)
                    {
                        if (stl.isApproved == true)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added STL") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("STL is Approved, Do you want to UnApprove this STL?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    stl.isApproved = false;
                                    stl.stage = TransactionStage.AwaitingApproval.ToString();
                                    stlRepo.Approve(stl);
                                    var res1 = MessageBox.Show("STL has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(usersRepo.getusersByDepartmentIdsList(new List<int> { stl.department.Id }, new List<int> { stl.company.Id }), stl.Id, TransactionItemType.STL);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(stl.department.Id, stl.company.Id), stl.Id, TransactionItemType.STL);
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

                                    string paymentsymbolCurr = "", STLSymbol = "", cashMarginSymbol = "";
                                    if (stl.paymentCurrency != null)
                                    {
                                        paymentsymbolCurr = stl.paymentCurrency.Abbrivation.ToString();
                                        STLSymbol = stl.stlCurrency.Abbrivation.ToString();
                                        //cashMarginSymbol = stl.cashMarginCurrency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog();
                                    {

                                        comment.Comment = "STL having Payment Amount (OC): " + stl.paymentAmountOC.ToString() + " (" + paymentsymbolCurr + ")\n "
                                                + "STL Payment(OC): " + stl.stlPaymentAmountOC.ToString() + " (" + STLSymbol + ")"
                                                + "\nCash Margin(OC): " + stl.cashMarginAmount.ToString() + " (" + cashMarginSymbol + ")" +
                                                "\n has been UnApproved";
                                        comment.Timestamp = DateTime.Now;
                                        comment.Subject = "STL UnApproved";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;

                                    }
                                    procurementRepo.Add(stl.Id, TransactionItemType.STL, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" /*+ stl.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" /*+ saleOrder.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("STL are UnApproved");
                                    SystemLog.LogInfo(this.GetType(), "STL is UnApproved (" + stl.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve STL Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve STL Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (stl.isApproved == false)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added STL") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("STL are Pending for Approval, Do you want to Approve this STL?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    stl.isApproved = true;
                                    stl.stage = TransactionStage.Approved.ToString();
                                    stlRepo.Approve(stl);

                                    var res1 = MessageBox.Show("STL has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { stl.department.Id }, new List<int> { stl.company.Id}), stl.Id, TransactionItemType.STL);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0 )
                                        {
                                            winTagUsers win = new winTagUsers(usersRepo.getusersByCompanyDepartment(stl.department.Id, stl.company.Id), stl.Id, TransactionItemType.STL);
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

                                    string paymentsymbolCurr = "", STLSymbol="", cashMarginSymbol="";
                                    if (stl.paymentCurrency != null )
                                    {
                                        paymentsymbolCurr = stl.paymentCurrency.Abbrivation.ToString();
                                        STLSymbol = stl.stlCurrency.Abbrivation.ToString();
                                        //cashMarginSymbol = stl.cashMarginCurrency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "STL having Payment Amount (OC): " + stl.paymentAmountOC.ToString() + " (" + paymentsymbolCurr + ")\n "
                                                + "STL Payment(OC): " + stl.stlPaymentAmountOC.ToString() + " (" + STLSymbol + ")"
                                                + "\nCash Margin(OC): " + stl.cashMarginAmount.ToString() + " (" + cashMarginSymbol + ")"+
                                                "\n has been Approved",
                                                Timestamp = DateTime.Now,
                                                Subject = "STL Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(stl.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" /*+ stl.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" /*+ stl.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("STL is Approved");
                                    SystemLog.LogInfo(this.GetType(), "STL is Approved (" + stl.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve STL Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve STL Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }


                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        UsersRepo UsersRepo = new UsersRepo();

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (stl.Id != 0)
                {
                    if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(stl.department.Id, stl.company.Id), comment, TransactionItemType.STL);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (stl != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && stl.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(stl.Id, TransactionItemType.STL, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in STL #" + stl.stlRef, stl.Id, TransactionItemType.STL, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in STL #" + stl.stlRef, stl.Id, TransactionItemType.STL, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in STL #" + stl.stlRef, stl.Id, TransactionItemType.STL, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in STL #" + stl.stlRef, stl.Id, TransactionItemType.STL, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            //{
                            //    foreach (var user in frmInputBox.Comment.TaggedList)
                            //    {
                            //        if (frmInputBox.FlagForTag == true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", null);

                            //    }
                            //    foreach (var user in frmInputBox.Comment.CCUsersList)
                            //    {

                            //        if(frmInputBox.FlagForCC==true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0,user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //    }
                            //}

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (stl.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save STL first to add a comment!");
                        }
                    }
                    loadcomments();
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
        }

        private void Resend_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Resend_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            var comment = grdCommentss.SelectedItem as CommentLog;


            if (comment != null)
            {
                comment = procurementRepo.GetComment(comment.Id);
                if (comment.employee.EmployeeUsers.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) == null)
                {
                    DXMessageBox.Show("Only sender of this comment can change Flag!");
                    return;
                }
                if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(stl.department.Id, stl.company.Id), TransactionItemType.STL);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }

                if (frmInputBox.commentAdded == true && stl.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (stl.Id == 0)
                {
                    DXMessageBox.Show("Kindly save STL first to add a comment!");
                }
            }
            loadcomments();
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            UsersRepo usersRepo = new UsersRepo();
            if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0 )
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartmentIdsList(new List<int> {stl.department.Id }, new List<int> { stl.company.Id }), TransactionItemType.STL);
                inputBox.ShowDialog();
            }
            else if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0 )
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByCompanyDepartment(stl.department.Id, stl.company.Id), TransactionItemType.STL);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (stl != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.commentAdded == true && stl.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(stl.Id, TransactionItemType.STL, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in STL " /*+ stl.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in STL " /*+ saleOrder.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in STL " /*+ stl.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in STL " /*+ stl.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }

                    }

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (stl.Id == 0)
                {
                    DXMessageBox.Show("Kindly save STL first to add a comment!");
                }

            }
            loadcomments();
        }
        public void loadcomments()
        {
            try
            {
                if (stl != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(stl.Id, TransactionItemType.STL);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }
        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL") != null)
            {
                loadcomments();
            }
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (stl.Id != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(stl.Id, TransactionItemType.STL);
                trackingWindow.ShowDialog();
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (stl.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void STL") != null))
            {

                if (DXMessageBox.Show("This STL is currently in the list of Void STL's! Do you want to remove it from Void?", "Remove Void STL", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    stl.isVoid = false;
                    //stlRepo.setSotoVoid(stl.Id, false);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("STL has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { stl.department.Id }, new List<int> { stl.company.Id }), stl.Id, TransactionItemType.STL);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0 )
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(stl.department.Id, stl.company.Id), stl.Id, TransactionItemType.STL);
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

                    string symbolCurr = "";
                    if (stl.paymentCurrency != null)
                    {
                        symbolCurr = stl.paymentCurrency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "STL (Amount OC) having value: " + stl.paymentAmountSTL.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "STL UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(stl.Id, TransactionItemType.STL, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" + stl.paymentAmountSTL, stl.Id, TransactionItemType.STL, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" + stl.paymentAmountSTL, stl.Id, TransactionItemType.STL, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void STL") != null)
            {
                if (DXMessageBox.Show("This STL is not currently in the list of Void STL's! Do you want to move it to Void STL's?", "Add to Void STL's", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    stl.isVoid = true;
                    //stlRepo.setSotoVoid(stl.Id, true);

                    grdVoid.Visibility = Visibility.Visible;


                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("STL has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0 )
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { stl.department.Id }, new List<int> { stl.company.Id}), stl.Id, TransactionItemType.STL);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0 )
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(stl.department.Id, stl.company.Id), stl.Id, TransactionItemType.STL);
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

                    string symbolCurr = "";
                    if (stl.paymentCurrency != null)
                    {
                        symbolCurr = stl.paymentCurrency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "STL (Payment Amount(OC)) having value: " + stl.paymentAmountSTL.ToString() + "(" + symbolCurr + ") " + " has been marked as void \nFrom: ",
                        Timestamp = DateTime.Now,
                        Subject = "STL Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(stl.Id, TransactionItemType.STL, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" + stl.paymentAmountSTL, stl.Id, TransactionItemType.STL, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" + stl.paymentAmountSTL, stl.Id, TransactionItemType.STL, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (stl.stlStatus.isActive == false && stl.PendingForClosing != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can attach document when STL Closed") != null)
                {

                    if (grdAttach1.Visibility == Visibility.Visible)
                        grdAttach1.Visibility = Visibility.Collapsed;
                    else
                    {
                        cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSTLAttachmentCategories();
                        grdAttach1.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required" + " Can attach document when STL Closed!");
                }
            }
            else
            {
                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSTLAttachmentCategories();
                    grdAttach1.Visibility = Visibility.Visible;
                }
            }
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {
                if (stl.Id != 0)
                {
                    treeViewAttachments1.ItemsSource = SYSTEM_STATIC.GetSTLAttachmentsListByCategory(stl.Id, TransactionItemType.STL);
                }
                grdAttachments1.Visibility = Visibility.Visible;
            }
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                ProcurementRepo procurementRepo = new ProcurementRepo();

                stlId = stl.Id;
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL without Approval") != null) ? true : false)
                {
                    double total = 0;
                    total = Math.Round(stl.paymentAmountOC, 2);
                    UsersRepo usersRepo = new UsersRepo();
                    stl = stlRepo.Get(stl.Id);
                    var row = stl;
                    if (row.stlStatus != null)
                    {
                        oldStatus = row.stlStatus;
                    }
                    ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.inActiveStatuses = 1;

                    ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stlid = (int)row.Id;
                    ZAS_ERP.Bankings.STL.Windows.frmSTLStatusChange statusChange = new ZAS_ERP.Bankings.STL.Windows.frmSTLStatusChange(stlRepo);
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id != 0)

                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing STL") != null) ? true : false)
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing = false;
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.stlStatus.Status + ") to (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id, 32, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stage = TransactionStage.AwaitingApproval.ToString();
                            if (ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing == null)
                            {
                                ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.stlStatus.Status + ") to (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id, 32, frmInputBox.comment);
                        }

                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null)
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing != true)
                            {
                                ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.stlStatus.Status + ") to (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id, 32, frmInputBox.comment);
                        }
                        else
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stage = TransactionStage.AwaitingFirstReview.ToString();

                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing = true;
                            usersRepo.Add(TransactionInfo.Closed, ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id, 32, frmInputBox.comment);
                        }
                    ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.LastStatusChangeDate = System.DateTime.Now;
                    ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.ClosingDate = System.DateTime.Now;
                    if (row.stlStatus != ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus)
                        usersRepo.Add(TransactionInfo.Status_Changed, stl.Id, (int)TransactionItemType.STL, "While direct closing Status Changed from (" + row.stlStatus.Status + ") to (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus + ")");
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("STL has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), stl.Id, TransactionItemType.STL);
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
                    string oldStat = "";
                    if (oldStatus != null)
                    {
                        oldStat = oldStatus.Status;
                    }
                    string newStat = ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status;
                    string paymentsymbolCurr = "", cashMarginSymbol = "", STLSymbol = "";
                    if (row.paymentCurrency != null)
                    {
                        paymentsymbolCurr = row.paymentCurrency.Abbrivation.ToString();
                        STLSymbol = row.stlCurrency.Abbrivation.ToString();
                        //cashMarginSymbol = row.cashMarginCurrency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of STL having Payment Amount (OC): " + row.paymentAmountOC.ToString() + " (" + paymentsymbolCurr + ")\n "
                            + "STL Payment(OC): " + row.stlPaymentAmountOC.ToString() + " (" + STLSymbol + ")"
                            + "\nCash Margin(OC): " + row.cashMarginAmount.ToString() + " (" + cashMarginSymbol + ")"
                            + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };

                    procurementRepo.Add(row.Id, TransactionItemType.STL, comment, SYSTEM_STATIC.currentUser.employeeId);

                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in STL #" /*+ row.SalesReferenceNo*/, row.Id, TransactionItemType.STL, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {


                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in STL " /*+ row.SalesReferenceNo*/, row.Id, TransactionItemType.STL, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                    try
                    {
                        row = ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl;

                        stlRepo.updateStatusById(row.Id, row.stlStatus);
                    }
                    catch { }
                    MessageBox.Show("STL status changed to InActive (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status + ")");


                    var thisWindow = Window.GetWindow(this);
                    thisWindow.Close();
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Close STL Directly.");
                }
            }
            catch (Exception)
            {


            }
        }

        private void datSTLPaymentDate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
           LoadTenureData();

        }
        public void LoadCreditTenure()
        {
            if(datPaymentMaturityDate.EditValue!=null && datSTLPaymentDate.EditValue!=null)
            {
                var value = (DateTime)datPaymentMaturityDate.EditValue - (DateTime)datSTLPaymentDate.EditValue;
                txtCashCT.Text = value.Days.ToString();
            }
        }
        public void LoadExtendedCreditTenure()
        {
            if (datPaymentMaturityDate.EditValue != null && datExtendedDate.EditValue != null)
            {
                var value =   (DateTime)datExtendedDate.EditValue- (DateTime)datPaymentMaturityDate.EditValue;
                txtCashECT.Text = value.Days.ToString();
            }
        }
        private void datPaymentMaturityDate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();
        }
        private void txtPaymentAmountSTL_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();

        }
        private void txtCashCT_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();
        }

        private void txtCashECT_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();
        }

        private void txtSTLUntilizedDays_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();
        }
        private void txInterstPerc_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();

        }

        private void txtCashPDD_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();
        }
        public void LoadTenureData()
        {
            LoadCreditTenure();
            LoadExtendedCreditTenure();
            LoadUtilizeDays();
            LoadPaymentDueDays();
            LoadInterestAmount();
            LoadExchangeRates();
            LoadExchangeRateValues();
            LoadCashMarginPerc();
            LoadSettlementBalance();
        }
        public void LoadSettlementBalance()
        {
            if(!string.IsNullOrEmpty(txtSettAmountSTL.Text) && grdCntrlSettlementList.VisibleItems.Count>0 && !string.IsNullOrEmpty(txtPaymentAmountSTL.Text) )
            {
                double settAmount = Convert.ToDouble(txtSettAmountSTL.Text);
                double stlAmount = Convert.ToDouble(txtPaymentAmountSTL.Text);
                double amountSTL = stlAmount - settAmount;
                txtBalSettAmountSTL.Text = Math.Round(amountSTL,2).ToString();
            }
        }
        public void LoadCashMarginPerc()
        {
            if (lookupCashMarginPerc.SelectedIndex > -1)
            {
                double perc = 0;
                perc = (lookupCashMarginPerc.SelectedItem as MarginPercentage).percentage;
                var value = Math.Round(Convert.ToDouble(txtPaymentAmountSTL.Text) * perc, 2);
                txtCashMarginAmount.Text = (value / 100).ToString();
            }
        }
        public void LoadExchangeRateValues()
        {

            if(!string.IsNullOrEmpty(txtMER.Text) && !string.IsNullOrEmpty(txtPaymentAmountSTL.Text))
            {
                txtAmountMER.Text = (Convert.ToDouble(txtMER.Text) * Convert.ToDouble(txtPaymentAmountSTL.Text)).ToString();

            }
            if (!string.IsNullOrEmpty(txtSER.Text) && !string.IsNullOrEmpty(txtPaymentAmountSTL.Text))
            {
                txtAmountSER.Text = (Convert.ToDouble(txtSER.Text) * Convert.ToDouble(txtPaymentAmountSTL.Text)).ToString();

            }
        }
        public void LoadExchangeRates()
        {
           if(!string.IsNullOrEmpty(txtPaymentAmountOC.Text) && !string.IsNullOrEmpty(txtPaymentAmountSTL.Text))
            {
                double paymentAmount = Convert.ToDouble(txtPaymentAmountOC.Text);
                double paymentAmountSTL = Convert.ToDouble(txtPaymentAmountSTL.Text);
                txtSER.Text = (paymentAmount / paymentAmountSTL).ToString();
                txtMER.Text = (paymentAmountSTL / paymentAmount).ToString();
            }
        }
        public void LoadPaymentDueDays()
        {
            if (datSTLPaymentDate.EditValue != null && datPaymentMaturityDate.EditValue != null)
            {
                if (!string.IsNullOrEmpty(txtCashCT.Text) && !string.IsNullOrEmpty(txtCashECT.Text))
                {
                    var value = Convert.ToDouble(txtCashCT.Text) + Convert.ToDouble(txtCashECT.Text);
                    txtCashPDD.Text = (Convert.ToDouble(txtSTLUntilizedDays.Text)- value).ToString();
                }
            }
        }
        public void LoadUtilizeDays()
        {
            if (datSTLPaymentDate.EditValue != null && datPaymentMaturityDate.EditValue != null)
            {
                if (!string.IsNullOrEmpty(txtCashCT.Text) && !string.IsNullOrEmpty(txtCashECT.Text))
                {

                    DateTime start = (DateTime)datSTLPaymentDate.EditValue;
                    DateTime end = (DateTime)datPaymentMaturityDate.EditValue;
                    DateTime calUtilizedDate = DateTime.Now;
                    var stamp = calUtilizedDate - start;
                    txtSTLUntilizedDays.Text = (stamp.Days).ToString();
                }
            }
        }
        public void LoadInterestAmount()
        {
            if(!string.IsNullOrEmpty(txtPaymentAmountSTL.Text)&& lookupSTLInterest.SelectedIndex>-1 && !string.IsNullOrEmpty(txtSTLUntilizedDays.Text) )
            {
                double paymentAmount = 0, interestPerc = 0, utilizeDays=0,creditTenure=0;

                creditTenure= Math.Round(Convert.ToDouble(txtCashCT.Text), 2)+ Math.Round(Convert.ToDouble(txtCashECT.Text), 2);
                paymentAmount =  Math.Round( Convert.ToDouble(txtPaymentAmountSTL.Text),2);
                interestPerc = Math.Round(Convert.ToDouble((lookupSTLInterest.SelectedItem as STLInterest).percentage), 2);
                utilizeDays = Math.Round(Convert.ToDouble(txtSTLUntilizedDays.Text), 2);
                var value1 = paymentAmount * interestPerc;
                var value2 = value1 / 100;
                var value3 = value2 / 365;
                var value4 = value3 * utilizeDays;
                txInterstAmountCDSTL.Text =(Math.Round(value4,2)).ToString();
                var value12 = paymentAmount * interestPerc;
                var value22 = value1 / 100;
                var value32 = value2 / 365;
                var value42 = value3 * creditTenure;
                txInterstAmountTDSTL.Text = (Math.Round(value42, 2)).ToString();
            }
        }
        



        private void txtSER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtAmountSER.Text = (Convert.ToDouble(txtSER.Text) * Convert.ToDouble( txtPaymentAmountSTL.Text)).ToString();

        }

        private void txtMER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtAmountMER.Text = (Convert.ToDouble(txtMER.Text) * Convert.ToDouble(txtPaymentAmountSTL.Text)).ToString();

        }
        private void lookupSTLInterest_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();
        }

        private void lookupSTLCurrency_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();

        }

        private void lookupPaymentCurrency_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();
        }

        private void view_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            //grdCntrlSettlementList.CurrentItem as STLSettlement = new STLSettlement();
        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            List<STLSettlement> source = grdCntrlSettlementList.ItemsSource as List<STLSettlement>;
            double settlementAmount = 0;
            if (source != null)
            {
                foreach (var item in source)
                {
                    settlementAmount += item.settlementAmount;
                }
            }
            txtSettAmountSTL.Text = settlementAmount.ToString();

        }

        private void viewReversal_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            List<ReversalSettlement> source = grdCntrlMarginReversalList.ItemsSource as List<ReversalSettlement>;
            double reversalSettlementAmount = 0;
            if (source != null)
            {
                foreach (var item in source)
                {
                    reversalSettlementAmount += item.reversalSettlementAmount;
                }
            }
            txtCashReversal.Text = reversalSettlementAmount.ToString();
        }

        private void txtSettAmountSTL_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            LoadTenureData();
        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                        if (str.Contains("STL"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.STL);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                    });
                    thread.Start();
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (stl.Id != 0)
                {
                    try
                    {
                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\STL\\ToUpload\\";

                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment1.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += stl.Id + "_" + TransactionItemType.STL.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.STL);
                                    if (result.Item1)
                                    {
                                        UsersRepo repo = new UsersRepo();
                                        AttachmentsRepo attachRepo = new AttachmentsRepo();
                                        attachRepo.Add(System.IO.Path.GetFileName(result.Item2), stl.Id, TransactionItemType.STL, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        repo.Add(TransactionInfo.Attachment_Uploaded, Convert.ToInt32(stl.Id), 3, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {

                                            treeViewAttachments1.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(stl.Id, TransactionItemType.STL);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment1.Content = "Select";
                                        });
                                    }
                                });
                                thread.Start();
                            }
                            else
                            {
                                DXMessageBox.Show("Invalid File name size");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {



                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void lookupDepartment_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var department=lookupDepartment.SelectedItem as Department;
            if(department.customers!=null && department.customers.Count>0)
            {
                lookupCustomer.ItemsSource = department.customers;
                if(payment!=null)
                {
                    if(payment.purchaseInvoice!=null)
                    {
                        if(payment.purchaseInvoice.customerCompany!=null)
                        {
                            lookupCustomer.Text = payment.purchaseInvoice.customerCompany.company.CompanyName;
                        }
                    }
                }
            }
            if (department.Vendors != null && department.Vendors.Count > 0)
            {
                lookupVendor.ItemsSource = department.Vendors;
                if (payment.purchaseInvoice != null)
                {
                    if (payment.purchaseInvoice.vendors != null)
                    {
                        lookupVendor.Text = payment.purchaseInvoice.vendors[0].company.CompanyName;
                    }
                }
            }
        }

        private void BtnGJournal_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View General Journal") != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(getJournalTransactions());
                generalJournal.ShowDialog();
            }
            else
            {
                MessageBox.Show("You are not Allowed to View General Journal.");
            }
        }

        private void btnCreateIBT_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers(TransactionItemType.STL,stl.Id);
                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;

                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;

                ucFrmBankTransfer.frmBankTranfer.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfer!");
                return;
            }
        }

        private void btnCreateICBT_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                ucBankTransferInterCompany ucFrmBankTransfer = new ucBankTransferInterCompany(TransactionItemType.STL, stl.Id);
                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;

                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;

                ucFrmBankTransfer.frmBankTranfer.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfer!");
                return;
            }
        }
        private void btnStageTracking_Click(object sender, RoutedEventArgs e)
        {
            if (gridOrderStageTrack.Visibility == Visibility.Collapsed)
            {
                gridOrderStageTrack.Visibility = Visibility.Visible;
            }
            else
            {
                gridOrderStageTrack.Visibility = Visibility.Collapsed;
            }
        }
        public void GellAllOrdersTracking()
        {

            if (stl.Id != 0)
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(stl.Id, TransactionItemType.STL);
            }
        }

        private void TreeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = grdOrdersTracking;
            if (grid.SelectedItem != null)
            {

                var item = grid.SelectedItem as AllOrdersView;

                if (item.transactionType == TransactionItemType.STL)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL") != null)
                    {
                        STLRepo sTLRepo = new STLRepo();

                        var selectedStl = sTLRepo.Get(Convert.ToInt32(item.Id));
                        winSTLAdd stl = new winSTLAdd(true, Convert.ToInt32(selectedStl.paymentGroupId));
                        stl.stl = selectedStl;
                        stl.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View STL");
                    }
                }
                else

                if (item.transactionType == TransactionItemType.InterBank_Transfer)
                {
                    try
                    {
                        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                        {

                            var selectedBankTransfer = bankTransRepo.GetInterBankTransfer(item.Id);

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
                else
                {
                    if (item.transactionType == TransactionItemType.Sale_Receipt)
                    {
                        GrdSaleReceiptListLoad(item.Id);
                        return;
                    }

                    if (item.transactionType == TransactionItemType.Tasks)
                    {
                        ucTaskAdd taskAdd = new ucTaskAdd();
                        taskAdd.taskId = Convert.ToInt32(item.Id);
                        taskAdd.editFlag = true;
                        Window win = new Window();
                        win.Content = taskAdd;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }

                    if (item.transactionType == TransactionItemType.LoansAdvances)
                    {
                        ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                        frmLoansAdvances.loansAdvanceId = Convert.ToInt32(item.Id);
                        frmLoansAdvances.editFlag = true;
                        Window win = new Window();
                        win.Content = frmLoansAdvances;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }

                    if (item.transactionType == TransactionItemType.TargetReward)
                    {
                        ucFrmBasicTargetRewards frmTargetRewards = new ucFrmBasicTargetRewards();
                        frmTargetRewards.rewardId = Convert.ToInt32(item.Id);
                        //frmTargetRewards.editFlag = true;
                        Window win = new Window();
                        win.Content = frmTargetRewards;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }


                    if (item.transactionType == TransactionItemType.Admin_Bill)
                    {
                        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                        AdminBillsRepo billsRepo = new AdminBillsRepo();
                        DXWindow frmBill = new DXWindow();

                        var bill = billsRepo.GetBill(item.Id);

                        frmBillAdd = new ucFrmBillAdd();

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId);

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBill.Content = frmBillAdd;

                                    frmBill.WindowState = WindowState.Maximized;

                                    frmBill.Title = "Admin Bill";
                                    frmBill.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                            else
                            {
                                frmBillAdd.editFlag = true;
                                frmBillAdd.groupId = bill.transactionGroupId;
                                frmBill.Content = frmBillAdd;

                                frmBill.WindowState = WindowState.Maximized;
                                frmBill.Title = "Admin Bill";
                                frmBill.Show();
                            }
                        }
                        return;
                    }
                    if (item.transactionType == TransactionItemType.Payments)
                    {
                        ucFrmPayments frmPayments = new ucFrmPayments();
                        ucFrmBillPaymentAdd ucFrmBillPayment = new ucFrmBillPaymentAdd();
                        ucFrmPInvoicePaymentAdd frmPInvoicePaymentAdd = new ucFrmPInvoicePaymentAdd();
                        ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                        ucFrmTargetRewardPayment frmTRpayment = new ucFrmTargetRewardPayment();
                        CompanyRepo compRepo = new CompanyRepo();
                        PaymentRepo paymentRepo = new PaymentRepo();


                        var payment = paymentRepo.GetPayment(item.Id);



                        switch (payment.transactionType)
                        {
                            case PaymentTransactionType.Loans_Advances:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmLApayment.editFlag = true;
                                        frmLApayment.groupId = payment.transactionGroupId;
                                        frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                        frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                        frmLApayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLApayment.editFlag = true;
                                    frmLApayment.groupId = payment.transactionGroupId;
                                    frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                    frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                    frmLApayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Target_Reward:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRpayment.editFlag = true;
                                        frmTRpayment.groupId = payment.transactionGroupId;
                                        frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                        frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRpayment.editFlag = true;
                                    frmTRpayment.groupId = payment.transactionGroupId;
                                    frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                    frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRpayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Admin_Bills:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = payment.transactionGroupId;
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
                                    frmPayments.groupId = payment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Vendor_Bills:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        ucFrmBillPayment.editFlag = true;
                                        ucFrmBillPayment.groupId = payment.transactionGroupId;
                                        ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                        ucFrmBillPayment.frmBillPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBillPayment.editFlag = true;
                                    ucFrmBillPayment.groupId = payment.transactionGroupId;
                                    ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                    ucFrmBillPayment.frmBillPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Purchase_Invoice:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPInvoicePaymentAdd.editFlag = true;
                                        frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPInvoicePaymentAdd.editFlag = true;
                                    frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                }
                                break;
                        }
                        return;
                    }
                    Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel((TransactionItemType)Enum.Parse(typeof(TransactionItemType), item.transactionType.ToString()), item.Id);
                    procurmentPanele.Show();
                }
            }
        }
        private void GrdSaleReceiptListLoad(int id)
        {
            try
            {
                var repo = new SalesReceiptRepo();
                var saleReceipt = repo.GetSalesReceipt(id);

                if (saleReceipt.receiptType == ReceiptType.Loans_Advances)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                    {
                        ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
                        //paymentRepo = new PaymentRepo();
                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                        Window frmPiPaymentWindow = new Window();
                        if (saleReceipt.saleReceiptStatus.isActive == false)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                            {
                                frmLAreceipt.editFlag = true;
                                frmLAreceipt.groupId = saleReceipt.transactionGroupId;
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
                            frmLAreceipt.groupId = saleReceipt.transactionGroupId;
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
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                    {
                        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                        updateSaleReceiptObj.saveEditFlag = 1;


                        if (saleReceipt == null)
                        {
                            return;
                        }

                        if (saleReceipt.saleReceiptStatus != null)
                        {
                            var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                            updateSaleReceiptObj.selectedStatus = status;
                        }

                        updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
                        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                        updateSaleReceiptObj.receiptId = saleReceipt.Id;
                        updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;
                        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanMinimize;
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

                        updateSaleReceiptObj.enter_receipt_win.ShowDialog();
                        //Load_Receipts();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                        return;
                    }
                }



            }
            catch
            {

            }
        }

        private void btnExpand_Click_1(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;
            grdTrackingTree.ExpandAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;
        }

        private void btnCollapsed_Click(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;
            grdTrackingTree.CollapseAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;

        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
        }
    }
}
