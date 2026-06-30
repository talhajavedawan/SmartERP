using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
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
    /// Interaction logic for ucReconcilationSummary.xaml
    /// </summary>
    public partial class ucReconcilationSummary : UserControl
    {
        public ChartofAccount chartofAccount = new ChartofAccount();
        DateTime datePeriod = new DateTime();
        ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
        JournalEntryRepo entryRepo = new JournalEntryRepo();
        List<int> deptIds = new List<int>();
        DepartmentRepo deptRepo = new DepartmentRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        double parentBalance = 0;
       
        List<JournalTransaction> dbTransactions = new List<JournalTransaction>();
        List<JournalTransaction> creditedTransactions = new List<JournalTransaction>();
        ReconcilationSummaryViewModel viewModel = new ReconcilationSummaryViewModel();
        List<ReconcilationSummaryViewModel> viewModelTransactions = new List<ReconcilationSummaryViewModel>();

        //List<ReconcilationViewModel> reconcilationTransactions = new List<ReconcilationViewModel>();
        ReconcilationRepo reconcilationRepo = new ReconcilationRepo();
        public ucReconcilationSummary()
        {
            InitializeComponent();
        }
        public ucReconcilationSummary(ChartofAccount _chartofAccount, DateTime _datePeriod)
        {
            InitializeComponent();
            chartofAccount = _chartofAccount;
            datePeriod = _datePeriod;
        }

       
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetUserDepartments();
            LoadAccountCompanies();
            LoadAccountDetail();
            grdReconciledSummary.AutoExpandAllGroups = true;
            GetAllTransactions();
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
        private void LoadAccountDetail()
        {

            lblAccount.Text = chartofAccount.accountName + "," + " " + "Period Ending" + " " + datePeriod.ToShortDateString();
        }
        private void GetAllTransactions()
        {
            dbTransactions = entryRepo.GetAllJournalTransactionByaccountId(chartofAccount.Id, deptIds);
            var creditedTransactions = dbTransactions.Where(x => x.reconcilationDate == datePeriod && x.creationDate <= datePeriod && x.isReconciled == true && x.reconcilationDate != null && x.debit == 0 && x.credit != 0 ).ToList();
            if (creditedTransactions.Count != 0)
            {
                viewModel = new ReconcilationSummaryViewModel();
                var creditSum = creditedTransactions.Sum(x => x.credit);
                viewModel.reconcilationTransaction = ERP_BL.Enums.ReconcilationTransaction.Checks_and_Payments;
                viewModel.transactionType = ERP_BL.Enums.ReconcilationType.Cleared_Transactions;
                viewModel.amount = -creditSum;
                viewModelTransactions.Add(viewModel);
            }
            var debitedTransactions = dbTransactions.Where(x => x.reconcilationDate == datePeriod && x.creationDate <= datePeriod && x.isReconciled == true && x.reconcilationDate != null && x.debit != 0 && x.credit == 0).ToList();
            if (debitedTransactions.Count != 0)
            {
                viewModel = new ReconcilationSummaryViewModel();
                var debitSum = debitedTransactions.Sum(x => x.debit);
                viewModel.reconcilationTransaction = ERP_BL.Enums.ReconcilationTransaction.Deposit_and_other_Credits;
                viewModel.transactionType = ERP_BL.Enums.ReconcilationType.Cleared_Transactions;
                viewModel.amount = debitSum;
                viewModelTransactions.Add(viewModel);
            }


           
            var unClearedCreditedTransactions = dbTransactions.Where(x => x.isReconciled == false && x.reconcilationDate == null && x.debit == 0 && x.credit != 0 && x.creationDate<=datePeriod).ToList();
            if (unClearedCreditedTransactions.Count != 0)
            {
                viewModel = new ReconcilationSummaryViewModel();
                var unClearedCreditSum = unClearedCreditedTransactions.Sum(x => x.credit);
                viewModel.reconcilationTransaction = ERP_BL.Enums.ReconcilationTransaction.Checks_and_Payments;
                viewModel.transactionType = ERP_BL.Enums.ReconcilationType.Uncleared_Transactions;
                viewModel.amount = -unClearedCreditSum;
                viewModelTransactions.Add(viewModel);
            }
            
            var unClearedDebitedTransactions = dbTransactions.Where(x => x.creationDate <= datePeriod && x.isReconciled == false && x.reconcilationDate != null && x.debit != 0 && x.credit == 0 ).ToList();
            if (unClearedDebitedTransactions.Count != 0)
            {
                viewModel = new ReconcilationSummaryViewModel();
                var unClearedDebitSum = unClearedDebitedTransactions.Sum(x => x.debit);
                viewModel.reconcilationTransaction = ERP_BL.Enums.ReconcilationTransaction.Deposit_and_other_Credits;
                viewModel.transactionType = ERP_BL.Enums.ReconcilationType.Uncleared_Transactions;
                viewModel.amount = unClearedDebitSum;
                viewModelTransactions.Add(viewModel);
            }
            var newCreditedTransactions = dbTransactions.Where(x => x.creationDate >= datePeriod && x.reconcilationDate == null && x.isReconciled == false && x.credit != 0 && x.debit == 0).ToList();
            if (newCreditedTransactions.Count != 0)
            {
                viewModel = new ReconcilationSummaryViewModel();
                var newCreditSum = newCreditedTransactions.Sum(x => x.credit);
                viewModel.reconcilationTransaction = ERP_BL.Enums.ReconcilationTransaction.Checks_and_Payments;
                viewModel.transactionType = ERP_BL.Enums.ReconcilationType.New_Transactions;
                viewModel.amount = -newCreditSum;
                viewModelTransactions.Add(viewModel);
            }
            var newDebitedTransactions = dbTransactions.Where(x => x.creationDate >= datePeriod && x.reconcilationDate == null && x.isReconciled == false && x.credit==0 && x.debit!=0).ToList();
            if (newDebitedTransactions.Count != 0)
            {
                viewModel = new ReconcilationSummaryViewModel();
                var newDebitSum = newDebitedTransactions.Sum(x => x.debit);
                viewModel.reconcilationTransaction = ERP_BL.Enums.ReconcilationTransaction.Deposit_and_other_Credits;
                viewModel.transactionType = ERP_BL.Enums.ReconcilationType.New_Transactions;
                viewModel.amount = newDebitSum;
                viewModelTransactions.Add(viewModel);
            }
            grdReconciledSummary.ItemsSource = viewModelTransactions;
        }
        private void GetUserDepartments()
        {
            var user = SYSTEM_STATIC.currentUser;
            var departments = companyRepo.GetUserDepartments(user.id);
            foreach (var dpt in departments)
                deptIds.Add(dpt.Id);

        }
        private void GrdReconciledSummary_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());

            PrintableControlLink link = new PrintableControlLink((TableView)grdReconciledSummary.View);

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
    }
}
