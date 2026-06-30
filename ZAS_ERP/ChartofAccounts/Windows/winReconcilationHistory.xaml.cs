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
using ZAS_ERP.ChartofAccounts.UserControls;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winReconcilationHistory.xaml
    /// </summary>
    public partial class winReconcilationHistory : DXWindow
    {
        List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        public winReconcilationHistory()
        {
            InitializeComponent();
        }

        private void LookupReconcileAccounts_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var chartofAccount = lookupReconcileAccounts.SelectedItem as ChartofAccount;
            List<DateTime> recocilationDates = new List<DateTime>();
            var reconcilations = chartofAccount.Reconcilations;
            foreach (var reconcilation in reconcilations)
            {
                if (!lstboxReconcilations.Items.Contains(reconcilation.reconcilationDate))
                {
                    lstboxReconcilations.Items.Add(reconcilation.reconcilationDate);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAccounts();
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
            //chartofAccounts = chartofAccounts.Where(x => x.reconcilationDate != null).ToList();
            lookupReconcileAccounts.ItemsSource = chartofAccounts;

        }



        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnDisplay_Click(object sender, RoutedEventArgs e)
        {
            ShowReconcilations();
        }
        private void ShowReconcilations()
        {
            var chartofAccount = lookupReconcileAccounts.SelectedItem as ChartofAccount;

            if (chartofAccount != null)
            {
                if (lstboxReconcilations.SelectedItem != null)
                {
                    var datePeriod = (DateTime)lstboxReconcilations.SelectedItem;
                    if (radioDetailReport.IsChecked == true)
                    {

                        ucReconcilationDetailReport detailReport = new ucReconcilationDetailReport(chartofAccount, datePeriod);
                        winReconcilationReport winReconcileReport = new winReconcilationReport();
                        winReconcileReport.grdReport.Children.Add(detailReport);
                        winReconcileReport.Show();
                        this.Close();
                    }
                    else
                    if (radioSummaryReport.IsChecked == true)
                    {
                        ucReconcilationSummary detailReport = new ucReconcilationSummary(chartofAccount, datePeriod);
                        winReconcilationReport winReconcileReport = new winReconcilationReport();
                        winReconcileReport.grdReport.Children.Add(detailReport);
                        winReconcileReport.Show();
                        this.Close();
                    }
                    else
                    if (radioBothReport.IsChecked == true)
                    {
                        ucReconcilationDetailReport detailReport = new ucReconcilationDetailReport(chartofAccount, datePeriod);
                        winReconcilationReport winReconcileReport = new winReconcilationReport();
                        winReconcileReport.grdReport.Children.Add(detailReport);
                        winReconcileReport.Show();
                        ucReconcilationSummary summaryReport = new ucReconcilationSummary(chartofAccount, datePeriod);
                        winReconcilationReport winReconcileSummaryReport = new winReconcilationReport();
                        winReconcileSummaryReport.grdReport.Children.Add(summaryReport);
                        winReconcileSummaryReport.Show();
                        this.Close();
                    }
                    else
                    {
                        DXMessageBox.Show("Please Select Report Type  ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    DXMessageBox.Show("Please Select Reconcilation Date", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

            }
            else
            {
                DXMessageBox.Show("Please Select Chart of Account", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}
