using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.ChartofAccounts.ViewModels;

namespace ZAS_ERP.ChartofAccounts.UserControls
{
    /// <summary>
    /// Interaction logic for ucReconcilationDetailReport.xaml
    /// </summary>
    public partial class ucReconcilationDetailReport : UserControl
    {
        public ChartofAccount chartofAccount = new ChartofAccount();
        DateTime datePeriod = new DateTime();
        ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
        JournalEntryRepo entryRepo = new JournalEntryRepo();
        List<int> deptIds = new List<int>();
        DepartmentRepo deptRepo = new DepartmentRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
        double parentCreditBalance = 0;
        double parentDebitBalance = 0;
        double parentNewTransactionBalance = 0;
        List<ReconcilationViewModel> reconcilationTransactions = new List<ReconcilationViewModel>();
        ReconcilationRepo reconcilationRepo = new ReconcilationRepo();
        List<JournalTransaction> dbTransactions = new List<JournalTransaction>();
        List<JournalTransaction> creditTransactions = new List<JournalTransaction>();
        List<JournalTransaction> debitTransaction = new List<JournalTransaction>();
        List<JournalTransaction> newTransactions = new List<JournalTransaction>();
        public ucReconcilationDetailReport()
        {
            InitializeComponent();
        }
        public ucReconcilationDetailReport(ChartofAccount _chartofAccount,DateTime _datePeriod)
        {
            InitializeComponent();
            chartofAccount = _chartofAccount;
            datePeriod = _datePeriod;
        }
        double value = 0;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetUserDepartments();
            LoadAccountCompanies();
            LoadAccountDetail();
            grdReconciledTransactions.AutoExpandAllGroups = true;
            GetAllTransactions();
          
        }
        private void LoadAccountDetail()
        {
            lblAccount.Text = chartofAccount.accountName+","+" "+"Period Ending"+" "+datePeriod.ToShortDateString();
        }

        private void LoadAccountCompanies()
        {
            string companies = "";
            foreach (var company in chartofAccount.Companies)
            {
                if (companies == null)
                    companies = company.CompanyName;
                companies = companies + company.CompanyName;
            }
            lblCompany.Text = companies;
        }
        private void GetUserDepartments()
        {
            var user = SYSTEM_STATIC.currentUser;
            var departments = companyRepo.GetUserDepartments(user.id);
            foreach (var dpt in departments)
                deptIds.Add(dpt.Id);
            
        }
        private void GetAllTransactions()
        {
            dbTransactions = entryRepo.GetAllJournalTransactionByaccountId(chartofAccount.Id, deptIds);
            //dbTransactions = journalTransactions.Where(x => x.reconcilationDate == datePeriod || x.creationDate >= datePeriod && x.reconcilationDate == null && x.isReconciled == false).ToList();
            creditTransactions = dbTransactions.Where(x => x.creationDate <= datePeriod && x.debit == 0 && x.credit != 0).ToList();
            var creditClearedTransactions = creditTransactions.Where(x =>x.reconcilationDate==datePeriod&& x.reconcilationType==ReconcilationType.Cleared_Transactions).ToList();
            var creditUnClearedTransactions = creditTransactions.Where(x => x.reconcilationType == ReconcilationType.Uncleared_Transactions).ToList();


            debitTransaction = dbTransactions.Where(x => x.reconcilationDate == datePeriod && x.creationDate <= datePeriod && x.debit != 0 && x.credit == 0).ToList();
            var debitClearedTransactions = debitTransaction.Where(x => x.reconcilationDate == datePeriod && x.reconcilationType == ReconcilationType.Cleared_Transactions).ToList();
            var debitUnClearedTransactions = debitTransaction.Where(x => x.reconcilationType == ReconcilationType.Uncleared_Transactions).ToList();

            newTransactions = dbTransactions.Where(x => x.creationDate >= datePeriod && x.reconcilationDate == null && x.isReconciled == false).ToList();

            foreach (var transaction in creditClearedTransactions)
            {
                ReconcilationViewModel transactionModel = new ReconcilationViewModel();
                transactionModel.Id = transaction.Id;
                transactionModel.coaTransactionsType = transaction.coaTransactionsType;
                transactionModel.chartofAccount = transaction.account;
                transactionModel.creationDate = transaction.creationDate;
                transactionModel.memo = transaction.memo;
                transactionModel.reconcilationTransaction = ReconcilationTransaction.Checks_and_Payments;//getReconcileTransaction(transaction.debit, transaction.credit);
                transactionModel.transactionType = ReconcilationType.Cleared_Transactions;//getTransactionType(transaction);
                transactionModel.amount = getAmount(transaction.debit, transaction.credit);
                transactionModel.balance = getCreditBalance(transaction.debit, transaction.credit, transaction);
                reconcilationTransactions.Add(transactionModel);
            }
            foreach (var transaction in creditUnClearedTransactions)
            {
                ReconcilationViewModel transactionModel = new ReconcilationViewModel();
                transactionModel.Id = transaction.Id;
                transactionModel.coaTransactionsType = transaction.coaTransactionsType;
                transactionModel.chartofAccount = transaction.account;
                transactionModel.creationDate = transaction.creationDate;
                transactionModel.memo = transaction.memo;
                transactionModel.reconcilationTransaction = ReconcilationTransaction.Checks_and_Payments;//getReconcileTransaction(transaction.debit, transaction.credit);
                transactionModel.transactionType = ReconcilationType.Uncleared_Transactions;
                transactionModel.amount = getAmount(transaction.debit, transaction.credit);
                transactionModel.balance = getCreditBalance(transaction.debit, transaction.credit, transaction);
                reconcilationTransactions.Add(transactionModel);
            }
            foreach (var transaction in debitClearedTransactions) 
            {
                ReconcilationViewModel transactionModel = new ReconcilationViewModel();
                transactionModel.Id = transaction.Id;
                transactionModel.coaTransactionsType = transaction.coaTransactionsType;
                transactionModel.chartofAccount = transaction.account;
                transactionModel.creationDate = transaction.creationDate;
                transactionModel.memo = transaction.memo;
                transactionModel.reconcilationTransaction = ReconcilationTransaction.Deposit_and_other_Credits;//getReconcileTransaction(transaction.debit, transaction.credit);
                transactionModel.transactionType = ReconcilationType.Cleared_Transactions;//getTransactionType(transaction);
                transactionModel.amount = getAmount(transaction.debit, transaction.credit);
                transactionModel.balance = getDebitBalance(transaction.debit, transaction.credit, transaction);
                reconcilationTransactions.Add(transactionModel);
            }
            foreach (var transaction in debitUnClearedTransactions)
            {
                ReconcilationViewModel transactionModel = new ReconcilationViewModel();
                transactionModel.Id = transaction.Id;
                transactionModel.coaTransactionsType = transaction.coaTransactionsType;
                transactionModel.chartofAccount = transaction.account;
                transactionModel.creationDate = transaction.creationDate;
                transactionModel.memo = transaction.memo;
                transactionModel.reconcilationTransaction = ReconcilationTransaction.Deposit_and_other_Credits;//getReconcileTransaction(transaction.debit, transaction.credit);
                transactionModel.transactionType = ReconcilationType.Uncleared_Transactions;//getTransactionType(transaction);
                transactionModel.amount = getAmount(transaction.debit, transaction.credit);
                transactionModel.balance = getDebitBalance(transaction.debit, transaction.credit, transaction);
                reconcilationTransactions.Add(transactionModel);
            }
            foreach (var transaction in newTransactions)
            {
                ReconcilationViewModel transactionModel = new ReconcilationViewModel();
                transactionModel.Id = transaction.Id;
                transactionModel.coaTransactionsType = transaction.coaTransactionsType;
                transactionModel.chartofAccount = transaction.account;
                transactionModel.creationDate = transaction.creationDate;
                transactionModel.memo = transaction.memo;
                transactionModel.reconcilationTransaction = getReconcileTransaction(transaction.debit, transaction.credit);
                transactionModel.transactionType = getTransactionType(transaction);
                transactionModel.amount = getAmount(transaction.debit, transaction.credit);
                transactionModel.balance = getNewTransactionBalance(transaction.debit, transaction.credit, transaction);
                reconcilationTransactions.Add(transactionModel);
            }
            grdReconciledTransactions.ItemsSource = reconcilationTransactions;
            
            parentCreditBalance = 0;
            parentDebitBalance = 0;
        }

        private ReconcilationType getTransactionType(JournalTransaction journalTransaction)
        {
            ReconcilationType type = new ReconcilationType();
            if (journalTransaction.isReconciled == true && journalTransaction.reconcilationDate == datePeriod)
            {
                type = ReconcilationType.Cleared_Transactions;
            }
            else
                 if (journalTransaction.creationDate <= datePeriod && journalTransaction.isReconciled == false && journalTransaction.reconcilationDate == null)
            {
                type = ReconcilationType.Uncleared_Transactions;
            }
            else
                 if (journalTransaction.creationDate >= datePeriod && journalTransaction.isReconciled == false && journalTransaction.reconcilationDate == null)
            {
                type = ReconcilationType.New_Transactions;
            }
            return type;
        }

        private ReconcilationTransaction getReconcileTransaction(double debit, double credit)
        {
            ReconcilationTransaction type = new ReconcilationTransaction();
            if (debit != 0)
            {
                 type= ReconcilationTransaction.Deposit_and_other_Credits;
            }
            else
            if (credit != 0)
            {
                type= ReconcilationTransaction.Checks_and_Payments;
            }
            return type;
        }
        private double getCreditBalance(double debit, double credit, JournalTransaction transaction)
        {
            double balance = 0;
            var sum =credit;
            balance = parentCreditBalance + sum;
            parentCreditBalance = balance;
            return balance;



            //double amount = 0;
            //var sum = debit - credit;
            //amount = parentAmount + sum;
            //parentAmount = amount;
            //double MER = 0;
            //if (transaction.SaleInvoice != null)
            //{
            //    MER = Convert.ToDouble(transaction.SaleInvoice.marginExchangeRate);
            //}

            //if (transaction.InterBank != null)
            //{
            //    MER = transaction.InterBank.MER;
            //}
            //if (transaction.SalesReceipt != null)
            //{
            //    MER = Convert.ToDouble(transaction.SalesReceipt.saleInvoice.marginExchangeRate);
            //}
            //if (transaction.AdminBill != null)
            //{
            //    MER = transaction.AdminBill.MER;
            //}
            //if (transaction.journalVoucher != null)
            //{
            //    MER = transaction.journalVoucher.MER;
            //}
            //return MER * amount;
        }
        private double getDebitBalance(double debit, double credit, JournalTransaction transaction)
        {
            double balance = 0;
            var sum = debit;
            balance = parentDebitBalance + sum;
            parentDebitBalance = balance;
            return balance;
        }
        private double getNewTransactionBalance(double debit, double credit, JournalTransaction transaction)
        {
            double balance = 0;
            var sum = debit-credit;
            balance = parentNewTransactionBalance + sum;
            parentNewTransactionBalance = balance;
            return balance;
        }

        private double getAmount(double debit, double credit)
        {
            //double amount = 0;
            var sum = debit - credit;
            //amount = parentAmountOC + sum;
            //parentAmountOC = amount;
            return sum;
        }


        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());

            PrintableControlLink link = new PrintableControlLink((TableView)grdReconciledTransactions.View);
            
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            
            link.Landscape = false;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }

        private void GrdReconciledTransactions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
