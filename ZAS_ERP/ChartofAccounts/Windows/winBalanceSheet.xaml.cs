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
using System.Windows.Threading;
using ZAS_ERP.ChartofAccounts.ViewModels;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winBalanceSheet.xaml
    /// </summary>
    public partial class winBalanceSheet : DXWindow
    {
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        List<int> companyIds = new List<int>();
        List<int> deptIds = new List<int>();
        CompanyRepo companyRepo = new CompanyRepo();
        List<ChartofAccount> assets = new List<ChartofAccount>();
        List<ChartofAccount> liabilities = new List<ChartofAccount>();
        List<ChartofAccount> balanceSheetChartofAccounts = new List<ChartofAccount>();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        public winBalanceSheet()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
            //mbtnAllCompanies.ItemsSource = employeeRepo.GetUserCOACompanies(SYSTEM_STATIC.currentUser.employeeId);
            mbtnAllCompanies.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
            GetCompanyDepartmentsIds();
            LoadAccounts();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void GetCompanyDepartmentsIds()
        {
            var user = SYSTEM_STATIC.currentUser;
            var departments = companyRepo.GetUserDepartments(user.id);
            deptIds.AddRange(departments.Select(x => x.Id).ToList());

            //var companies = employeeRepo.GetUserCOACompanies(SYSTEM_STATIC.currentUser.employeeId);
            var companies = SYSTEM_STATIC.currentUser.employee.Companies;
            companyIds.AddRange(companies.Select(x => x.Id).ToList());
        }
      
        public void LoadAccounts()
        {
            //dateFrom.EditValue = DateTime.Now.AddMonths(-2);
            dateTo.EditValue = DateTime.Now;
            List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();

            if (MainWindow.currentUserid == 0)
            {
                chartofAccounts = repo.getAll();
                
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Chart of Accounts") != null)
            {
                chartofAccounts = repo.getAllActive(MainWindow.currentUserid);
            }
            else
            {
                chartofAccounts = repo.getAllActive(MainWindow.currentUserid);
            }
            balanceSheetChartofAccounts= chartofAccounts.Where(x => x.isVoid != true && x.accountType != COA_AccountType.Income || x.isVoid != true && x.accountType != COA_AccountType.Expense || x.isVoid != true && x.accountType != COA_AccountType.Other_Expense || x.isVoid != true && x.accountType != COA_AccountType.Other_Income || x.isVoid != true && x.accountType != COA_AccountType.Cost_of_Goods_Sold).ToList();
            assets= balanceSheetChartofAccounts.Where(x => 
                x.accountType == COA_AccountType.Account_Receivable  
             || x.accountType == COA_AccountType.Fixed_Asset
             || x.accountType == COA_AccountType.Other_Asset
             || x.accountType == COA_AccountType.Other_Current_Asset
             || x.accountType == COA_AccountType.Bank
            ).ToList();
            liabilities = balanceSheetChartofAccounts.Where(x => 
               x.accountType == COA_AccountType.Longterm_Liability
            || x.accountType == COA_AccountType.Other_Current_Liability
            || x.accountType == COA_AccountType.Accounts_Payable
            || x.accountType == COA_AccountType.Equity).ToList();
            grdAssets.ItemsSource = assets;
            grdLiability.ItemsSource = liabilities;
            lblHeading.Text = "Balance Sheet Detail";
            lblHeading.TextAlignment = TextAlignment.Center;
            //System.Threading.Thread th2 = new System.Threading.Thread(() =>
            //{
            //    if (grdAssets.Dispatcher.CheckAccess())
            //    {
            //        SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdAssets);
            //    }
            //    else
            //    {
            //        grdAssets.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdAssets); }));
            //    }
            //});
            //th2.Start();
        }

        private void grdAssets_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData)
                {
                    var chartofAccount = grdAssets.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
                    if (e.Column.FieldName == "Company")
                    {
                        var chartofAccount1 = grdAssets.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
                        if (chartofAccount1 != null)
                        {
                            if (chartofAccount.Companies != null && chartofAccount.Companies.Count != 0)
                            {
                                var res = String.Join(", ", chartofAccount.Companies.Select(x => x.CompanyName));
                                e.Value = res;
                            }
                        }
                    }
                    else
                   if (e.Column.FieldName == "Department")
                    {

                        var chartofAccount2 = grdAssets.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
                        if (chartofAccount2 != null)
                        {
                            var res = String.Join(", ", chartofAccount.Departments.Select(x => x.DeptName));
                            e.Value = res;
                        }
                    }
                    else
                   if (e.Column.FieldName == "balanceOC")
                    {
                        if (mbtnAllCompanies.SelectedIndex > -1)
                        {
                            var company = mbtnAllCompanies.SelectedItem as Company;

                            if (chartofAccount != null)
                            {
                                var journaltransactions = chartofAccount.JournalTransactions.Where(
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
                                                            x.companyId == company.Id &&
                                                            x.creationDate != null &&
                                 //x.creationDate >= (DateTime)dateFrom.EditValue &&
                                 x.creationDate <= (DateTime)dateTo.EditValue &&

                                                            deptIds.Contains((int)x.deptId) &&
                                                            x.companyId != null &&
                                                            x.companyId == company.Id &&
                                                            companyIds.Contains((int)x.companyId)).ToList();
                                var value = journaltransactions
                                                    .Sum(x => x.total);
                                e.Value = e.Value = Math.Round(value, 2);
                            }
                        }
                        else
                        {
                            if (chartofAccount != null)
                            {
                                var journalTransactions = chartofAccount.JournalTransactions.Where(
                              x => x.AdminBill?.isVoid != true
                              && x.Bill?.isVoid != true
                              && x.PurchaseInvoice?.isVoid != true
                              && x.SaleInvoice?.isVoid != true
                              && x.SalesReceipt?.isVoid != true
                              && x.journalVoucher?.isVoid != true
                              && x.Payment?.isVoid != true
                              && x.InterBank?.isVoid != true
                              && x.InterCompanyTransfer?.isVoid != true
                              && x.creationDate != null
                              && x.deptId != null
                              //&& x.creationDate >= (DateTime)dateFrom.EditValue
                              && x.creationDate <= (DateTime)dateTo.EditValue
                              && deptIds.Contains((int)x.deptId)
                              && x.companyId != null

                              && companyIds.Contains((int)x.companyId)).ToList();
                                var value = journalTransactions.Sum(x => x.total);
                               e.Value = Math.Round(value, 2);
                            }
                        }
                    }
                    if (e.Column.FieldName == "balancePKR")
                    {
                        if (mbtnAllCompanies.SelectedIndex > -1)
                        {
                            var company = mbtnAllCompanies.SelectedItem as Company;
                            if (chartofAccount != null)
                            {
                                var journalTransactions = chartofAccount.JournalTransactions.Where(
                                x => x.AdminBill?.isVoid != true
                                && x.Bill?.isVoid != true
                                && x.PurchaseInvoice?.isVoid != true
                                && x.SaleInvoice?.isVoid != true
                                && x.SalesReceipt?.isVoid != true
                                && x.journalVoucher?.isVoid != true
                                && x.Payment?.isVoid != true
                                && x.InterBank?.isVoid != true
                                && x.creationDate != null &&
                                x.InterCompanyTransfer?.isVoid != true &&
                                //x.creationDate >= (DateTime)dateFrom.EditValue &&
                                x.creationDate <= (DateTime)dateTo.EditValue &&
                                x.deptId != null && deptIds.Contains((int)x.deptId) &&
                                x.companyId != null && companyIds.Contains((int)x.companyId) &&
                                x.companyId == company.Id).ToList();
                                var value = journalTransactions.Sum(x => x.total * x.MER);
                                e.Value = value;
                            }
                        }
                        else
                        {
                            if (chartofAccount != null)
                            {
                                var journalTransactions = chartofAccount.JournalTransactions.Where(
                                x => x.AdminBill?.isVoid != true
                                 && x.Bill?.isVoid != true
                                 && x.PurchaseInvoice?.isVoid != true
                                 && x.SaleInvoice?.isVoid != true
                                 && x.SalesReceipt?.isVoid != true
                                 && x.journalVoucher?.isVoid != true
                                 && x.Payment?.isVoid != true
                                 && x.InterBank?.isVoid != true
                                 //&& x.creationDate >= (DateTime)dateFrom.EditValue
                                 && x.creationDate <= (DateTime)dateTo.EditValue
                                 && x.creationDate != null
                                 && x.InterCompanyTransfer?.isVoid != true
                                 && x.deptId != null && deptIds.Contains((int)x.deptId)
                                 && x.companyId != null && companyIds.Contains((int)x.companyId)
                                ).ToList();
                                var value = journalTransactions.Sum(x => x.total * x.MER);
                                e.Value = value;

                            }
                        }
                        //e.Value = Math.Round(totalDebitsPKR, 2);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grdLiability_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData)
                {
                    var chartofAccount = grdLiability.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
                    if (e.Column.FieldName == "Company")
                    {
                        var chartofAccount1 = grdLiability.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
                        if (chartofAccount1 != null)
                        {
                            if (chartofAccount.Companies != null && chartofAccount.Companies.Count != 0)
                            {
                                var res = String.Join(", ", chartofAccount.Companies.Select(x => x.CompanyName));
                                e.Value = res;
                            }
                        }
                    }
                    else
                   if (e.Column.FieldName == "Department")
                    {

                        var chartofAccount2 = grdLiability.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
                        if (chartofAccount2 != null)
                        {
                            var res = String.Join(", ", chartofAccount.Departments.Select(x => x.DeptName));
                            e.Value = res;
                        }
                    }
                    else
                   if (e.Column.FieldName == "balanceOC")
                    {
                        if (mbtnAllCompanies.SelectedIndex > -1)
                        {
                            var company = mbtnAllCompanies.SelectedItem as Company;

                            if (chartofAccount != null)
                            {
                                //var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                                //                    x.AdminBill?.isVoid != true &&
                                //                     x.Bill?.isVoid != true &&
                                //                     x.journalVoucher?.isVoid != true &&
                                //                     x.InterBank?.isVoid != true &&
                                //                     x.Payment?.isVoid != true &&
                                //                     x.SaleInvoice?.isVoid != true &&
                                //                     x.SalesReceipt?.isVoid != true &&
                                //                      x.PurchaseInvoice?.isVoid != true &&
                                //                     x.InterCompanyTransfer?.isVoid != true &&
                                //                     x.creationDate != null &&
                                //                     x.creationDate < (DateTime)dateFrom.EditValue &&
                                //                     x.deptId != null && deptIds.Contains((int)x.deptId) &&
                                //                     x.companyId == company.Id &&
                                //                     x.companyId != null && companyIds.Contains((int)x.companyId)
                                //              ).Sum(x => x.total);
                                var journaltransactions = chartofAccount.JournalTransactions.Where(
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
                                                            x.companyId == company.Id &&
                                                            x.creationDate != null &&
                                 //x.creationDate >= (DateTime)dateFrom.EditValue &&
                                 x.creationDate <= (DateTime)dateTo.EditValue &&

                                                            deptIds.Contains((int)x.deptId) &&
                                                            x.companyId != null &&
                                                            x.companyId == company.Id &&
                                                            companyIds.Contains((int)x.companyId)).ToList();
                                var value = journaltransactions
                                                    .Sum(x => x.total);


                                e.Value = e.Value = Math.Round(value, 2);
                            }
                        }
                        else
                        {
                            if (chartofAccount != null)
                            {
                                var journalTransactions = chartofAccount.JournalTransactions.Where(
                              x => x.AdminBill?.isVoid != true
                              && x.Bill?.isVoid != true
                              && x.PurchaseInvoice?.isVoid != true
                              && x.SaleInvoice?.isVoid != true
                              && x.SalesReceipt?.isVoid != true
                              && x.journalVoucher?.isVoid != true
                              && x.Payment?.isVoid != true
                              && x.InterBank?.isVoid != true
                              && x.InterCompanyTransfer?.isVoid != true
                              && x.creationDate != null
                              && x.deptId != null
                              //&& x.creationDate >= (DateTime)dateFrom.EditValue
                              && x.creationDate <= (DateTime)dateTo.EditValue
                              && deptIds.Contains((int)x.deptId)
                              && x.companyId != null
                              && companyIds.Contains((int)x.companyId)).ToList();
                                var value = journalTransactions.Sum(x => x.total);
                               e.Value = Math.Round(value, 2);
                            }
                        }


                    }
                    if (e.Column.FieldName == "balancePKR")
                    {
                        if (mbtnAllCompanies.SelectedIndex > -1)
                        {
                            var company = mbtnAllCompanies.SelectedItem as Company;
                            if (chartofAccount != null)
                            {
                                var journalTransactions = chartofAccount.JournalTransactions.Where(
                                x => x.AdminBill?.isVoid != true
                                && x.Bill?.isVoid != true
                                && x.PurchaseInvoice?.isVoid != true
                                && x.SaleInvoice?.isVoid != true
                                && x.SalesReceipt?.isVoid != true
                                && x.journalVoucher?.isVoid != true
                                && x.Payment?.isVoid != true
                                && x.InterBank?.isVoid != true
                                && x.creationDate != null &&
                                x.InterCompanyTransfer?.isVoid != true &&
                                //x.creationDate >= (DateTime)dateFrom.EditValue &&
                                x.creationDate <= (DateTime)dateTo.EditValue &&
                                x.deptId != null && deptIds.Contains((int)x.deptId) &&
                                x.companyId != null && companyIds.Contains((int)x.companyId) &&
                                x.companyId == company.Id).ToList();
                                var value = journalTransactions.Sum(x => x.total * x.MER);
                                
                                e.Value = value;
                            }
                        }
                        else
                        {
                            if (chartofAccount != null)
                            {
                                var journalTransactions = chartofAccount.JournalTransactions.Where(
                                x => x.AdminBill?.isVoid != true
                                 && x.Bill?.isVoid != true
                                 && x.PurchaseInvoice?.isVoid != true
                                 && x.SaleInvoice?.isVoid != true
                                 && x.SalesReceipt?.isVoid != true
                                 && x.journalVoucher?.isVoid != true
                                 && x.Payment?.isVoid != true
                                 && x.InterBank?.isVoid != true
                                 //&& x.creationDate >= (DateTime)dateFrom.EditValue
                                 && x.creationDate <= (DateTime)dateTo.EditValue
                                 && x.creationDate != null
                                 && x.InterCompanyTransfer?.isVoid != true
                                 && x.deptId != null && deptIds.Contains((int)x.deptId)
                                 && x.companyId != null && companyIds.Contains((int)x.companyId)
                                ).ToList();
                                var value = journalTransactions.Sum(x => x.total * x.MER);
                                e.Value = value;

                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void BtnDateFilter_Click(object sender, RoutedEventArgs e)
        {
            grdLiability.RefreshData();
            grdAssets.RefreshData();
        }

        private void grdAssets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedAccount = grdAssets.SelectedItem as ChartofAccount;
            var company = mbtnAllCompanies.SelectedItem as Company;

            if (selectedAccount != null)
            {
                var chartofAccount = repo.get(selectedAccount.Id);
                if (company != null)
                {
                    winLedger ledger = new winLedger(chartofAccount, company);
                    ledger.Title = "Transactions";
                    ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.accountName;
                    ledger.Show();
                }
                else
                {
                    winLedger ledger = new winLedger(chartofAccount);
                    ledger.Title = "Transactions";
                    ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.accountName;
                    ledger.Show();
                }
            }
        }

        private void grdLiability_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedAccount = grdLiability.SelectedItem as ChartofAccount;
            var company = mbtnAllCompanies.SelectedItem as Company;

            if (selectedAccount != null)
            {
                var chartofAccount = repo.get(selectedAccount.Id);
                if (company != null)
                {
                    winLedger ledger = new winLedger(chartofAccount, company);
                    ledger.Title = "Transactions";
                    ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.accountName;
                    ledger.Show();
                }
                else
                {
                    winLedger ledger = new winLedger(chartofAccount);
                    ledger.Title = "Transactions";
                    ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.accountName;
                    ledger.Show();
                }
            }
        }

        private void mbtnAllCompanies_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            grdAssets.RefreshData();
            grdLiability.RefreshData();
        }
    }
}
