using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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
using ZAS_ERP.ChartofAccounts.UserControls;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winReconcilation.xaml
    /// </summary>
    public partial class winReconcilation : DXWindow
    {
        //reconcile
        ChartofAccount reconcileAccount = new ChartofAccount();
        DateTime datePeriod = new DateTime();
        double endingBalance = 0;
        double biginningBalance = 0;


        //Service
        double serviceCharge = 0;
        double serviceExchange = 0;
        DateTime serviceDate = new DateTime();
        ChartofAccount serviceAccount = new ChartofAccount();
        Department serviceDepartment = new Department();
        //Interest
        double interestEarned = 0;
        double interestExchange = 0;
        DateTime interestDate = new DateTime();
        ChartofAccount interestAccount = new ChartofAccount();
        Department interestDepartment = new Department();


        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        DepartmentRepo deptRepo = new DepartmentRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        List<int> deptIds = new List<int>();
        List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
        List<JournalTransaction> clearedTransactions = new List<JournalTransaction>();

        List<JournalTransaction> unClearedTransactions = new List<JournalTransaction>();

        JournalEntryRepo journalEntryRepo = new JournalEntryRepo();
        Double clearedBalance = 0;
        Double debit = 0;
        Double credit = 0;
        List<JournalTransaction> totalJournalTransactions = new List<JournalTransaction>();
        Reconcilation reconcilation = new Reconcilation();
        CoaLogicClass ModuleLogic = new CoaLogicClass();


        public winReconcilation()
        {
            InitializeComponent();
        }
        public winReconcilation(ChartofAccount _reconcileAccount, DateTime _datePeriod, double _endingBalance)
        {
            InitializeComponent();
            reconcileAccount = _reconcileAccount;
            datePeriod = _datePeriod;
            endingBalance =_endingBalance;
        }
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var user = SYSTEM_STATIC.currentUser;
            //var departments = companyRepo.GetUserDepartments(user.id);
            //foreach (var dpt in departments)
            //    deptIds.Add(dpt.Id);


            //companyIds = SYSTEM_STATIC.currentUser.employee.Companies.Select(x => x.Id).ToList();
            deptIds = SYSTEM_STATIC.currentUser.employee.departments.Select(x => x.Id).ToList();

            journalTransactions = journalEntryRepo.GetAllJournalTransactionByaccountId(reconcileAccount.Id, deptIds,datePeriod);

            unClearedTransactions= journalTransactions.Where(x=>x.isReconciled==false ).ToList();
            clearedTransactions= journalTransactions.Where(x => x.isReconciled == true).ToList();
            var debit = clearedTransactions.Select(x => x.debit).Sum();
            var credit = clearedTransactions.Select(x => x.credit).Sum();
            clearedBalance = debit - credit;


            LoadDebits();
            LoadCredits();
            txtForPeriod.Text = datePeriod.ToShortDateString();
            PopulateEndingBalance();
            LoadClearedBalance();
            LoadBeginningBalance();
            
       }
        public void LoadClearedBalance()
        {
            if(reconcileAccount.Id!=0)
            {
                    var debit = clearedTransactions.Select(x => x.debit).Sum();
                    var credit = clearedTransactions.Select(x => x.credit).Sum();
                    double value = debit - credit;
                   txtClearedBalance.Text = value.ToString();
            }
        }
        public void LoadBeginningBalance()
        {
            var debit = clearedTransactions.Select(x => x.debit).Sum();
            var credit = clearedTransactions.Select(x => x.credit).Sum();
            double value = debit - credit;
            txtBeginningBalance.Text = Math.Round(value,2).ToString();
        }
        public void PopulateEndingBalance()
        {
            txtEndingBalance.Text = endingBalance.ToString();
        }
        public void LoadDebits()
        {
            var debits = unClearedTransactions.Where(x => x.credit == 0 && x.debit != 0).ToList();
            grdDebits.ItemsSource = debits;
        }
        public void LoadCredits()
        {
            var credits = unClearedTransactions.Where(x => x.debit == 0 && x.credit != 0).ToList();
            grdCredits.ItemsSource = credits;
        }  
        private void GrdDebits_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
           var checkedList = grdDebits.SelectedItems;
           foreach(JournalTransaction transaction in checkedList)
            {
                journalTransactions.Add(transaction);
            }
            debit = journalTransactions.Select(x => x.debit).Sum();
            txtCheckedDebitsCount.Text = checkedList.Count.ToString();
            txtCheckedDebits.Text = debit.ToString();
            var result = debit - credit;
            var result1 = result + clearedBalance;
            txtClearedBalance.Text = Math.Round(result1,2).ToString();
        }
        private void GrdCredits_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
            var checkedList = grdCredits.SelectedItems;
            foreach (JournalTransaction transaction in checkedList)
            {
                journalTransactions.Add(transaction);
            }
             credit = journalTransactions.Select(x => x.credit).Sum();
            txtCheckedCredits.Text = credit.ToString();
            txtCheckedCreditsCount.Text = checkedList.Count.ToString();
            var result = debit - credit;
            var result1 = result + clearedBalance;
            txtClearedBalance.Text = Math.Round(result1, 2).ToString();
        }
        private void TextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEndingBalance.Text) && !string.IsNullOrEmpty(txtClearedBalance.Text))
            {
                txtDifference.Text = Math.Round( (Convert.ToDouble(txtEndingBalance.Text) - Convert.ToDouble(txtClearedBalance.Text)), 2).ToString();
            }
        }

        private void BtnReconcile_Click(object sender, RoutedEventArgs e)
        {
            Reconcilation reconcilation = new Reconcilation();
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
            ReconcilationRepo reconcilationRepo = new ReconcilationRepo();
            Reconcilation accountReconcilation = new Reconcilation();
            if (Convert.ToDouble(txtDifference.Text) == 0)
            {
                var chartofAccount = repo.GetAccountById(reconcileAccount.Id);
                if (reconcileAccount != null)
                {
                    accountReconcilation.reconcilationAmount = endingBalance;
                    accountReconcilation.reconcilationDate = datePeriod;
                    journalTransactions=GetJournalTransactions();
                    chartofAccount.Reconcilations.Add(accountReconcilation);
                    chartofAccount.reconcilationDate = datePeriod;
                    chartofAccount.openingBalance = Math.Round( Convert.ToDouble( txtEndingBalance.Text),2);
                    repo.Reconcile(journalTransactions, chartofAccount);
                    DXMessageBox.Show("Account has been reconciled till" + "" + datePeriod.ToString(), "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);
                        var inputfromUser1 = DXMessageBox.Show("Do you want to see Reconcilation Detail? ", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (inputfromUser1 == System.Windows.MessageBoxResult.Yes)
                        {
                            ucReconcilationDetailReport report = new ucReconcilationDetailReport(reconcileAccount, datePeriod);
                            winReconcilationReport winReconcileReport = new winReconcilationReport();
                            winReconcileReport.grdReport.Children.Add(report);
                            winReconcileReport.Show();
                        }
                        else
                            this.Close();
                    this.Close();
                }
            }
            else
            {
                DXMessageBox.Show("Please select correct transactions to reconcile"  , "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }
        private List<JournalTransaction> GetJournalTransactions()
        {
            List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
            var creditedTransactions = new List<JournalTransaction>();
            var debitedTransactions = new List<JournalTransaction>();
            foreach (JournalTransaction transaction in grdCredits.SelectedItems)
            {
                transaction.isReconciled = true;
                transaction.reconcilationDate = datePeriod;
                transaction.reconcilationType = ReconcilationType.Cleared_Transactions;
                creditedTransactions.Add(transaction);
            }
            foreach (JournalTransaction transaction in grdDebits.SelectedItems)
            {
                transaction.isReconciled = true;
                transaction.reconcilationDate = datePeriod;
                transaction.ReconcilationId = reconcilation.Id;

                transaction.reconcilationType = ReconcilationType.Cleared_Transactions;
                debitedTransactions.Add(transaction);
            }
            journalTransactions.AddRange(creditedTransactions.Distinct());
            journalTransactions.AddRange(debitedTransactions.Distinct());
            return journalTransactions;
        }
        private void BtnModify_Click(object sender, RoutedEventArgs e)
        {
            winSelectReconcileAccount reconcileAccountForm = new winSelectReconcileAccount();
            var va=reconcileAccountForm.lookupReconcileAccounts.SelectedIndex;
            reconcileAccountForm.lookupReconcileAccounts.Text = reconcileAccount.accountName;
            var reconciledtransactions = journalEntryRepo.getAllReconciledTransactions(reconcileAccount.Id, deptIds);
            if (reconciledtransactions.Count != 0)
            {
                var endingBalance = reconciledtransactions.Sum(x => x.debit - x.credit);
                reconcileAccountForm.txtbiginningBalance.Text = endingBalance.ToString();
            }
            else
                reconcileAccountForm.txtbiginningBalance.Text = 0.ToString();
            if (datePeriod != null)
                reconcileAccountForm.dateDatePeriod.EditValue = datePeriod;
            reconcileAccountForm.reconcileAccount = reconcileAccount;
            reconcileAccountForm.txtEndingBalance.Text = endingBalance.ToString();
            reconcileAccountForm.Show();
            this.Close();
        }

        private void BtnLeave_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            var creditedtransactions= grdCredits.ItemsSource as List<JournalTransaction>;
            var debitedtransactions = grdDebits.ItemsSource as List<JournalTransaction>;
            var filteredCreditTransactions = creditedtransactions.Where(x => x.creationDate <= datePeriod.Date && x.credit != 0 && x.debit == 0).ToList();
            grdCredits.ItemsSource = filteredCreditTransactions;
            var filteredDebitTransactions = debitedtransactions.Where(x => x.creationDate <= datePeriod && x.credit==0 &&x.debit!=0).ToList();
            grdDebits.ItemsSource = filteredDebitTransactions;
        }

        private void CheckEdit_Unchecked(object sender, RoutedEventArgs e)
        {
            LoadDebits();
            LoadCredits();
        }

        private void GrdCredits_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var transaction = grdCredits.GetFocusedRow() as JournalTransaction;
                var dbTransaction = journalEntryRepo.GetAllJournalTransactionById(transaction.Id);
                switch (transaction.coaTransactionsType)
                {
                    case coaTransactionsType.Bill:
                        {

                            ModuleLogic.PopulateBill(TransactionItemType.Bill,(int)dbTransaction.Bill_Id);
                            break;
                        }
                    case coaTransactionsType.Inquiry:
                        {
                            break;
                        }

                    case coaTransactionsType.InterBankTransfer:
                        {
                            ModuleLogic.PopulateInterBankTransfer(dbTransaction);
                            break;
                        }
                    case coaTransactionsType.JV:
                        {
                            var trans = journalEntryRepo.GetAllJournalTransactionById(dbTransaction.Id);

                            ModuleLogic.PopulateJV(dbTransaction);
                            break;
                        }

                    case coaTransactionsType.Offer:
                        {
                            break;
                        }

                    case coaTransactionsType.PurchaseOrder:
                        {
                            break;
                        }

                    case coaTransactionsType.SaleInvoice:
                        {
                            ModuleLogic.PopulateSaleInvoice(TransactionItemType.Sale_Invoice, (int)dbTransaction.SaleInvoiceId);
                            break;
                        }
                    case coaTransactionsType.SaleOrder:
                        {
                            break;
                        }
                    case coaTransactionsType.SaleReceipt:
                        {
                            ModuleLogic.PopulateSaleSaleReceipt((int)dbTransaction.SaleReceiptId);
                            break;
                        }
                    case coaTransactionsType.AdminBill:
                        {
                            ModuleLogic.PopulateAdminBills((int)dbTransaction.AdminBillId);
                            break;
                        }
                    case coaTransactionsType.InterCompanyTransfer:
                        {
                            ModuleLogic.PopulateInterCompanyTransfer(dbTransaction);
                            break;
                        }
                    case coaTransactionsType.Payment:
                        {
                            ModuleLogic.PopulatePayments(dbTransaction.Payment);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GrdDebits_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var transaction = grdDebits.GetFocusedRow() as JournalTransaction;
                var dbTransaction = journalEntryRepo.GetAllJournalTransactionById(transaction.Id);
                switch (transaction.coaTransactionsType)
                {
                    case coaTransactionsType.Bill:
                        {
                            break;
                        }
                    case coaTransactionsType.Inquiry:
                        {
                            break;
                        }
                    case coaTransactionsType.InterBankTransfer:
                        {
                            ModuleLogic.PopulateInterBankTransfer(dbTransaction);
                            break;
                        }
                    case coaTransactionsType.JV:
                        {
                            var trans = journalEntryRepo.GetAllJournalTransactionById(dbTransaction.Id);
                            ModuleLogic.PopulateJV(dbTransaction);
                            break;
                        }

                    case coaTransactionsType.Offer:
                        {
                            break;
                        }
                    case coaTransactionsType.PurchaseOrder:
                        {
                            break;
                        }

                    case coaTransactionsType.SaleInvoice:
                        {
                            ModuleLogic.PopulateSaleInvoice(TransactionItemType.Sale_Invoice, (int)dbTransaction.SaleInvoiceId);
                            break;
                        }
                    case coaTransactionsType.SaleOrder:
                        {
                            break;
                        }
                    case coaTransactionsType.SaleReceipt:
                        {
                            ModuleLogic.PopulateSaleSaleReceipt((int)dbTransaction.SaleReceiptId);
                            break;
                        }
                    case coaTransactionsType.AdminBill:
                        {
                            ModuleLogic.PopulateAdminBills((int)dbTransaction.AdminBillId);
                            break;
                        }
                    case coaTransactionsType.InterCompanyTransfer:
                        {
                            ModuleLogic.PopulateInterCompanyTransfer(dbTransaction);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
