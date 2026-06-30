using DevExpress.Xpf.Core;
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
using System.Windows.Shapes;
using ZAS_ERP.ChartofAccounts.ViewModels;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for GernalJournal.xaml
    /// </summary>
    public partial class GernalJournal : DXWindow
    {
        List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
        ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
       
        CompanyRepo companyRepo = new CompanyRepo();
        DepartmentRepo departmentRepo = new DepartmentRepo();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        public GernalJournal()
        {
            InitializeComponent();
        }
        public GernalJournal(List<JournalTransaction> _journalTransactions)
        {
            InitializeComponent();
            journalTransactions = _journalTransactions;
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();
            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
        }

        private void GrdListTransactions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<GeneralJournal> journals = new List<GeneralJournal>();
                foreach (var trans in journalTransactions)
                {
                    GeneralJournal journal = new GeneralJournal();
                    if (trans.accountId != null)
                    {
                        var chartofAccount = coaRepo.GetAccountById((int)trans.accountId);
                        var transactions = chartofAccount.JournalTransactions.Where(x =>
                          x.AdminBill?.isVoid != true &&
                          x.Bill?.isVoid != true &&
                          x.journalVoucher?.isVoid != true &&
                          x.InterBank?.isVoid != true &&
                          x.Payment?.isVoid != true &&
                          x.SaleInvoice?.isVoid != true &&
                          x.SalesReceipt?.isVoid != true &&
                          x.InterCompanyTransfer?.isVoid != true &&
                          x.deptId != null &&
                          deptIds.Contains((int)x.deptId) &&
                          x.companyId != null &&
                          companyIds.Contains((int)x.companyId)).ToList();
                        var total = transactions.Sum(x => x.total);
                        var totalPKR = transactions.Sum(x => x.total * x.MER);
                        journal.transactionRefno = trans.transactionRefno;
                        journal.PostingDate = trans.creationDate;
                        journal.chartofAccount = coaRepo.GetAccountById((int)trans.accountId);
                        journal.company = companyRepo.GetCompany((int)trans.companyId);
                        journal.department = departmentRepo.get((int)trans.deptId);
                        journal.credit = trans.credit;
                        if (trans.currencyId != null)
                            journal.currency = currencyRepo.get((int)trans.currencyId);
                        journal.debit = trans.debit;
                        journal.MER = trans.MER;
                        journal.debitPKR = trans.debit * trans.MER;
                        journal.creditPKR = trans.credit * trans.MER;
                        journal.memo = trans.memo;
                        journal.balanceOC = total;
                        journal.balancePKR = totalPKR;
                        journals.Add(journal);
                    }
                    else
                    {

                    }
                }
                grdListEntries.ItemsSource = journals;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
         
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void GrdListEntries_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedAccount = grdListEntries.SelectedItem as GeneralJournal;

            if (selectedAccount != null)
            {
                winLedger ledger = new winLedger(selectedAccount.chartofAccount);
                ledger.deptIds = deptIds;
                ledger.companyIds = companyIds;
                ledger.Title = "Transactions";
                ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.chartofAccount.accountName;
                ledger.Show();
            }
        }
    }
}
