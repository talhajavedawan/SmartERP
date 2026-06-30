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

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winSelectReconcileAccount.xaml
    /// </summary>
    public partial class winSelectReconcileAccount : DXWindow
    {

        List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        public ChartofAccount reconcileAccount = new ChartofAccount();
        ChartofAccount serviceAccount = new ChartofAccount();
        ChartofAccount interestAccount = new ChartofAccount();
        Department serviceDepartment = new Department();
        Department interestDepartment = new Department();
        DateTime reconcilationFromDate = new DateTime();
        DateTime reconcilationToDate = new DateTime();
        JournalEntryRepo journalTransactionsRepo = new JournalEntryRepo();
        List<int> deptIds = new List<int>();

        public winSelectReconcileAccount()
        {
            InitializeComponent();
        }
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var user = SYSTEM_STATIC.currentUser;
            var departments = companyRepo.GetUserDepartments(user.id);
            foreach (var dpt in departments)
                deptIds.Add(dpt.Id);
            LoadAccounts();
            LoadDepartments();
        }
        public void LoadAccounts()
        {
            if (MainWindow.currentUserid == 0)
            {
                chartofAccounts = repo.getAll();

            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Chart of Accounts") != null)
            {
                chartofAccounts = repo.getAll(MainWindow.currentUserid);
            }
            else
            {
                chartofAccounts = repo.getAllActive(MainWindow.currentUserid);
            }
            lookupInterestAccounts.ItemsSource = chartofAccounts;
            lookupReconcileAccounts.ItemsSource = chartofAccounts;
            lookupServiceAccounts.ItemsSource = chartofAccounts;

        }
        public void LoadDepartments()
        {
           lookupServiceDepartment.ItemsSource=  companyRepo.GetUserDepartments(SYSTEM_STATIC.currentUser.id);
           lookupInterestDepartment.ItemsSource = companyRepo.GetUserDepartments(SYSTEM_STATIC.currentUser.id);
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            if(lookupReconcileAccounts.SelectedIndex==-1)
            {
                DXMessageBox.Show("Please select reconcile account!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                lookupReconcileAccounts.Focus();
                return;
            }
            //if (Convert.ToDouble( txtEndingBalance.Text)==0)
            //{
            //    DXMessageBox.Show("Please enter ending balance to continue!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            //    txtEndingBalance.Focus();
            //    return;
            //}
            if(dateDatePeriod.EditValue==null)
            {
                DXMessageBox.Show("Please enter date period to continue!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateDatePeriod.Focus();
                return;
            }
            

            winReconcilation reconcilation = new winReconcilation(reconcileAccount,Convert.ToDateTime(dateDatePeriod.EditValue),Convert.ToDouble(txtEndingBalance.Text));
            reconcilation.Show();
            this.Close();
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LookupReconcileAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            reconcileAccount= lookupReconcileAccounts.SelectedItem as ChartofAccount;

            if (reconcileAccount.reconcilationDate != null)
            {
                dateDatePeriod.EditValue = reconcileAccount.reconcilationDate;
                txtbiginningBalance.Text = reconcileAccount.openingBalance.ToString();
                
            }






            //var reconciledtransactions=journalTransactionsRepo.getAllReconciledTransactions(reconcileAccount.Id,deptIds);

            //var transactions = reconciledtransactions.Where(x => deptIds.Contains((int)x.deptId));


            //if (reconciledtransactions.Count != 0)
            //{
            //    var endingBalance = reconciledtransactions.Sum(x => x.debit - x.credit);
            //    txtbiginningBalance.Text = endingBalance.ToString();
            //    if (reconcileAccount.reconcilationDate != null)
            //        dateDatePeriod.EditValue = reconcileAccount.reconcilationDate;
            //}
            //else
            //    txtbiginningBalance.Text = 0.ToString();
            
        }

        private void LookupServiceAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            serviceAccount = lookupServiceAccounts.SelectedItem as ChartofAccount;
        }
        private void LookupInterestAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            interestAccount= lookupInterestAccounts.SelectedItem as ChartofAccount;
        }

        private void LookupServiceDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            serviceDepartment=lookupServiceDepartment.SelectedItem as Department;

        }

        private void LookupInterestDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            interestDepartment = lookupInterestDepartment.SelectedItem as Department;
        }
    }
}
