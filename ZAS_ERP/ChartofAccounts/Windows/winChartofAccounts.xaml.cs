using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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
    /// Interaction logic for winChartofAccounts.xaml
    /// </summary>
    public partial class winChartofAccounts : DXWindow
    {
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        public winChartofAccounts()
        {
            InitializeComponent();
  
        }

        private void grdAccountListing_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedAccount = grdAccounts.SelectedItem as ChartofAccount;

            if (selectedAccount != null)
            {
                var chartofAccount = repo.get(selectedAccount.Id);
                winLedger ledger = new winLedger(chartofAccount);
                ledger.deptIds = deptIds;
                ledger.companyIds = companyIds;
                ledger.Title = "Transactions";
                ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.accountName;
                ledger.Show();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
            var userCompanies = SYSTEM_STATIC.currentUser.employee.Companies;
            deptIds = userDepartments.Select(x => x.Id).ToList();
            companyIds = userCompanies.Select(x => x.Id).ToList();
            grdChartofAccountsGroups.ItemsSource = repo.GetAllChartofAccountGroups(SYSTEM_STATIC.currentUser.id);
        }

        private void tblChartofAccountListView_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                var group = e.Node.Content as ChartofAccountGroup;
                if (e.Column.FieldName == "totalAccounts")
                {
                    if (group.ChartofAccounts != null && group.ChartofAccounts.Count != 0)
                    {
                        e.Value = group.ChartofAccounts.Count;
                    }
                }
            }
        }

        private void grdChartofAccountsGroups_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (grdChartofAccountsGroups.SelectedItem != null)
            {
                var selectedGroup = grdChartofAccountsGroups.SelectedItem as ChartofAccountGroup;
                //if (selectedGroup != null)
                //{
                //    foreach (var account in selectedGroup.ChartofAccounts)
                //    {
                //        //account.CompanyNames = account.Companies != null
                //        //    ? string.Join(", ", account.Companies.Select(x => x.CompanyName))
                //        //    : string.Empty;

                //        //account.DepartmentNames = account.Departments != null
                //        //    ? string.Join(", ", account.Departments.Select(x => x.DeptName))
                //        //    : string.Empty;

                //        //var validTransactions = account.JournalTransactions?.Where(x =>
                //        //    x.AdminBill?.isVoid != true &&
                //        //    x.Bill?.isVoid != true &&
                //        //    x.PurchaseInvoice?.isVoid != true &&
                //        //    x.SaleInvoice?.isVoid != true &&
                //        //    x.SalesReceipt?.isVoid != true &&
                //        //    x.journalVoucher?.isVoid != true &&
                //        //    x.Payment?.isVoid != true &&
                //        //    x.InterBank?.isVoid != true &&
                //        //    x.InterCompanyTransfer?.isVoid != true &&
                //        //    x.deptId != null && deptIds.Contains((int)x.deptId) &&
                //        //    x.companyId != null && companyIds.Contains((int)x.companyId)
                //        //).ToList() ?? new List<JournalTransaction>();

                //        account.BalanceOC = Math.Round(account.JournalTransactions.Sum(x => x.total), 2);
                //        account.BalancePKR = account.JournalTransactions.Sum(x => x.total * x.MER);
                //    }

                    grdAccounts.ItemsSource = selectedGroup.ChartofAccounts;
                //}
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void GrdAccountsTree_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData)
                {

                    var chartofAccount = e.Node.Content as ChartofAccount;
                    if (e.Column.FieldName == "Company")
                    {
                        var chartofAccount1 = e.Node.Content as ChartofAccount;
                        if (chartofAccount.Companies != null && chartofAccount.Companies.Count != 0)
                        {
                            var res = String.Join(", ", chartofAccount.Companies.Select(x => x.CompanyName));
                            e.Value = res;
                        }
                    }
                    else
                   if (e.Column.FieldName == "Department")
                    {
                        var chartofAccount2 = e.Node.Content as ChartofAccount;
                        var res = String.Join(", ", chartofAccount.Departments.Select(x => x.DeptName));
                        e.Value = res;
                    }
                   // else
                   //if (e.Column.FieldName == "balanceOC")
                   // {

                   //     var value = chartofAccount.JournalTransactions.Where(
                   // x => x.AdminBill?.isVoid != true
                   //  && x.Bill?.isVoid != true
                   //  && x.PurchaseInvoice?.isVoid != true
                   //  && x.SaleInvoice?.isVoid != true
                   //  && x.SalesReceipt?.isVoid != true
                   //  && x.journalVoucher?.isVoid != true
                   //  && x.Payment?.isVoid != true
                   //  && x.InterBank?.isVoid != true
                   //  && x.InterCompanyTransfer?.isVoid != true
                   //  &&
                   // x.deptId != null && deptIds.Contains((int)x.deptId)
                   // &&
                   // x.companyId != null && companyIds.Contains((int)x.companyId)
                   // ).Sum(x => x.total);
                   //     e.Value = e.Value = Math.Round(value, 2);

                   // }
                   // if (e.Column.FieldName == "balancePKR")
                   // {
                   //     //var chartofAccount4 = e.Node.Content as ChartofAccount;
                   //     var value = chartofAccount.JournalTransactions.Where(
                   //     x => x.AdminBill?.isVoid != true
                   //      && x.Bill?.isVoid != true
                   //      && x.PurchaseInvoice?.isVoid != true
                   //      && x.SaleInvoice?.isVoid != true
                   //      && x.SalesReceipt?.isVoid != true
                   //      && x.journalVoucher?.isVoid != true
                   //      && x.Payment?.isVoid != true
                   //      && x.InterBank?.isVoid != true
                   //      && x.InterCompanyTransfer?.isVoid != true
                   //      &&
                   //     x.deptId != null && deptIds.Contains((int)x.deptId)
                   //     &&
                   //     x.companyId != null && companyIds.Contains((int)x.companyId)
                   //     ).Sum(x => x.total * x.MER);

                   //     e.Value = value;
                   //     //e.Value = Math.Round(totalDebitsPKR, 2);
                   // }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnLoanBalances_Click(object sender, RoutedEventArgs e)
        {

            if (grdAccounts.SelectedItem != null)
            {
                var chartofAccount = grdAccounts.SelectedItem as ChartofAccount;
                LoadBalanceOC(chartofAccount);
                LoadBalanceCMER(chartofAccount);
                grdAccounts.RefreshData();
            }
        }
        public void LoadBalanceOC(ChartofAccount chartofAccount)
        {
           
                var value = chartofAccount.JournalTransactions.Where(
                  x => x.AdminBill?.isVoid != true
                  && x.Bill?.isVoid != true
                  && x.PurchaseInvoice?.isVoid != true
                  && x.SaleInvoice?.isVoid != true
                  && x.SalesReceipt?.isVoid != true
                  && x.journalVoucher?.isVoid != true
                  && x.Payment?.isVoid != true
                  && x.InterBank?.isVoid != true
                  && x.InterCompanyTransfer?.isVoid != true &&
                  x.creationDate != null &&
                  x.deptId != null &&
                  deptIds.Contains((int)x.deptId) &&
                  x.companyId != null &&
                  companyIds.Contains((int)x.companyId)
                ).Sum(x => x.total);
                chartofAccount.BalanceOC = Math.Round(value, 2);
            
        }
        public void LoadBalanceCMER(ChartofAccount chartofAccount)
        {
           
                var value = chartofAccount.JournalTransactions.Where(
                x => x.AdminBill?.isVoid != true
                 && x.Bill?.isVoid != true
                 && x.PurchaseInvoice?.isVoid != true
                 && x.SaleInvoice?.isVoid != true
                 && x.SalesReceipt?.isVoid != true
                 && x.journalVoucher?.isVoid != true
                 && x.Payment?.isVoid != true
                 && x.InterBank?.isVoid != true
                 &&
               x.creationDate != null

                 && x.InterCompanyTransfer?.isVoid != true
                 &&
                x.deptId != null && deptIds.Contains((int)x.deptId)
                &&
                x.companyId != null && companyIds.Contains((int)x.companyId)
                ).Sum(x => x.total * x.MER);

               chartofAccount.BalancePKR = Math.Round(value, 2);
            
        }

    }
}
