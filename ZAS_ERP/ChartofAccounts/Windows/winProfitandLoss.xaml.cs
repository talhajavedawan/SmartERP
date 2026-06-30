using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections;
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
    /// Interaction logic for winProfitandLoss.xaml
    /// </summary>
    public partial class winProfitandLoss : DXWindow
    {
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        List<int> companyIds = new List<int>();
        List<int> deptIds = new List<int>();
        CompanyRepo companyRepo = new CompanyRepo();
        List<ChartofAccount> profitandLossChartofAccounts = new List<ChartofAccount>();
        List<ChartofAccount> expensesAccount = new List<ChartofAccount>();
        decimal sumPLOC = 0; 
        decimal sumPLPKR = 0;
        decimal sumExpenseOC = 0; 
        decimal sumExpensePKR = 0;
        EmployeeRepo employeeRepo = new EmployeeRepo();
        public winProfitandLoss()
        {
            InitializeComponent();

        }
   
  

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dateFrom.EditValue = DateTime.Now.AddMonths(-2);
            dateTo.EditValue = DateTime.Now;
            GetCompanyDepartmentsIds();

            //mbtnAllCompanies.ItemsSource = employeeRepo.GetUserCOACompanies(SYSTEM_STATIC.currentUser.employeeId);
            mbtnAllCompanies.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
            
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
            foreach (var dpt in departments)
                deptIds.Add(dpt.Id);
           
            //var companies = employeeRepo.GetUserCOACompanies(SYSTEM_STATIC.currentUser.employeeId);
            var companies = SYSTEM_STATIC.currentUser.employee.Companies;
            companyIds.AddRange(companies.Select(x => x.Id).ToList());
        }


        private void GrdAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
        public void LoadAccounts()
        {
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
            profitandLossChartofAccounts= chartofAccounts.Where(x => x.isVoid != true && x.accountType == COA_AccountType.Income || x.isVoid != true && x.accountType == COA_AccountType.Other_Income || x.isVoid != true && x.accountType == COA_AccountType.Cost_of_Goods_Sold).ToList();
            expensesAccount = chartofAccounts.Where(x =>  x.isVoid != true && x.accountType == COA_AccountType.Expense || x.isVoid != true && x.accountType == COA_AccountType.Other_Expense ).ToList();
            lblHeading.Text = "Profit & Loss";

            grdPLAccounts.ItemsSource = profitandLossChartofAccounts;
            grdexpennseAccounts.ItemsSource = expensesAccount;
            //LoadProfitandLossAccounts();

            lblHeading.TextAlignment = TextAlignment.Center;
            System.Threading.Thread th2 = new System.Threading.Thread(() =>
            {
                if (grdPLAccounts.Dispatcher.CheckAccess())
                {
                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPLAccounts);
                }
                else
                {
                    grdPLAccounts.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPLAccounts); }));
                }
            });
            th2.Start();
        }
        private void LoadProfitandLossAccounts()
        {
            List<ProfitandLossViewModel> chartofAccounts = new List<ProfitandLossViewModel>();
            ProfitandLossViewModel model = new ProfitandLossViewModel();
            foreach (var account in profitandLossChartofAccounts)
            {
                model = new ProfitandLossViewModel()
                {
                    Id = account.Id,
                    Account = account,
                    Companies = GetAccountCompanies(account),
                    Departments = GetAccountDepartments(account),
                    Balance = GetAccountBalance(account),
                    BalanceOC = GetAccountBalanceOC(account)
                };
                chartofAccounts.Add(model);
            }

            grdPLAccounts.ItemsSource = chartofAccounts;

        }
        private string GetAccountCompanies(ChartofAccount account)
        {
            string companies = "";
            if (account.Companies.Count != 0)
                foreach (var company in account.Companies)
                {
                    if (companies == "")
                    {
                        companies = company.CompanyName;
                    }
                    else
                        companies = companies + "," + company.CompanyName;
                }
            return companies;
        }
        private string GetAccountDepartments(ChartofAccount account)
        {
            string departments = "";
            if (account.Departments.Count != 0)
                foreach (var department in account.Departments)
                {
                    if (string.IsNullOrEmpty(departments))
                    {
                        departments = department.DeptName;
                    }
                    else
                        departments = departments + "," + department.DeptName;
                }
            return departments;
        }
        private double GetAccountBalance(ChartofAccount account)
        {
            var transactions = account.JournalTransactions;
            double value = 0;
            double MER = 0;
            foreach (var item in transactions)
            {
                if (item.debit != 0)
                {
                    if (item.InterBankId != 0 && item.InterBankId != null)
                    {
                        var interBank = item.InterBank;

                        if (interBank.isVoid != true)
                        {
                            value = value + item.debit;
                            MER = interBank.MER;
                        }
                    }
                    else
                        if (item.journalVoucher_id != 0 && item.journalVoucher_id != null)
                    {
                        var jv = item.journalVoucher;

                        if (jv.isVoid != true)
                        {
                            value = value + item.debit;
                            MER = jv.MER;
                        }
                    }
                    else
                    if (item.SaleInvoiceId != 0 && item.SaleInvoice != null)
                    {
                        var saleInvoice = item.SaleInvoice;

                        if (saleInvoice.isVoid != true)
                        {
                            value = value + item.debit;
                            MER = Convert.ToDouble(saleInvoice.marginExchangeRate);
                        }
                    }
                    if (item.SaleReceiptId != 0 && item.SalesReceipt != null)
                    {
                        var saleReceipt = item.SalesReceipt;

                        if (saleReceipt.isVoid != true)
                        {
                            value = value + item.debit;
                            MER = Convert.ToDouble(saleReceipt.saleInvoice.marginExchangeRate);
                        }
                    }
                    if (item.AdminBillId != 0 && item.AdminBill != null)
                    {
                        var adminBill = item.AdminBill;

                        if (adminBill.isVoid != true)
                        {
                            value = value + item.debit;
                            MER = Convert.ToDouble(adminBill.MER);
                        }
                    }
                }
                else
                    if (item.credit != 0)
                {
                    if (item.InterBankId != 0 && item.InterBankId != null)
                    {
                        var interBank = item.InterBank;

                        if (interBank.isVoid != true)
                        {
                            value = value - item.credit;
                            MER = interBank.MER;
                        }
                    }
                    else
                        if (item.journalVoucher_id != 0 && item.journalVoucher_id != null)
                    {
                        var jv = item.journalVoucher;

                        if (jv.isVoid != true)
                        {
                            value = value - item.credit;
                            MER = jv.MER;
                        }
                    }
                    else
                        if (item.SaleInvoiceId != 0 && item.SaleInvoice != null)
                    {
                        var saleInvoice = item.SaleInvoice;

                        if (saleInvoice.isVoid != true)
                        {
                            value = value - item.credit;
                            MER = Convert.ToDouble(saleInvoice.marginExchangeRate);
                        }
                    }
                    else
                        if (item.SaleReceiptId != 0 && item.SalesReceipt != null)
                    {
                        var saleReceipt = item.SalesReceipt;

                        if (saleReceipt.isVoid != true)
                        {
                            value = value - item.credit;
                            MER = Convert.ToDouble(saleReceipt.saleInvoice.marginExchangeRate);
                        }
                    }
                    else
                        if (item.AdminBillId != 0 && item.AdminBill != null)
                    {
                        var adminBill = item.AdminBill;

                        if (adminBill.isVoid != true)
                        {
                            value = value - item.credit;
                            MER = Convert.ToDouble(adminBill.MER);
                        }
                    }
                }
            }
            return value * MER;

        }
        private double GetAccountBalanceOC(ChartofAccount account)
        {
            var transactions = account.JournalTransactions;

            double value = 0;
            foreach (var item in transactions)
            {
                if (item.deptId != null)
                {
                    if (deptIds.Contains((int)item.deptId) && deptIds.Contains((int)item.deptId))
                    {
                        if (item.debit != 0)
                        {
                            if (item.InterBankId != 0 && item.InterBankId != null)
                            {
                                var interBank = item.InterBank;

                                if (interBank.isVoid != true)
                                    value = value + item.debit;
                            }
                            else
                                if (item.journalVoucher_id != 0 && item.journalVoucher_id != null)
                            {
                                var jv = item.journalVoucher;

                                if (jv.isVoid != true)
                                    value = value + item.debit;
                            }
                            else
                            if (item.SaleInvoiceId != 0 && item.SaleInvoice != null)
                            {
                                var saleInvoice = item.SaleInvoice;

                                if (saleInvoice.isVoid != true)
                                    value = value + item.debit;
                            }
                            if (item.SaleReceiptId != 0 && item.SalesReceipt != null)
                            {
                                var saleReceipt = item.SalesReceipt;

                                if (saleReceipt.isVoid != true)
                                    value = value + item.debit;
                            }
                            if (item.AdminBillId != 0 && item.AdminBill != null)
                            {
                                var adminBill = item.AdminBill;

                                if (adminBill.isVoid != true)
                                    value = value + item.debit;
                            }
                        }
                        else
                            if (item.credit != 0)
                        {
                            if (item.InterBankId != 0 && item.InterBankId != null)
                            {
                                var interBank = item.InterBank;

                                if (interBank.isVoid != true)
                                    value = value - item.credit;
                            }
                            else
                                if (item.journalVoucher_id != 0 && item.journalVoucher_id != null)
                            {
                                var jv = item.journalVoucher;

                                if (jv.isVoid != true)
                                    value = value - item.credit;
                            }
                            else
                                if (item.SaleInvoiceId != 0 && item.SaleInvoice != null)
                            {
                                var saleInvoice = item.SaleInvoice;

                                if (saleInvoice.isVoid != true)
                                    value = value - item.credit;
                            }
                            else
                                if (item.SaleReceiptId != 0 && item.SalesReceipt != null)
                            {
                                var saleReceipt = item.SalesReceipt;

                                if (saleReceipt.isVoid != true)
                                    value = value - item.credit;
                            }
                            else
                            if (item.AdminBillId != 0 && item.AdminBill != null)
                            {
                                var adminBill = item.AdminBill;

                                if (adminBill.isVoid != true)
                                    value = value - item.credit;
                            }
                        }
                    }
                    else
                        continue;
                }
            }
            return value;
        }

        private void GrdPLAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedAccount = grdPLAccounts.SelectedItem as ChartofAccount;
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

                //ledger.deptIds = deptIds;
                //ledger.companyIds = companyIds;

            }
        }

        private void grdPLAccounts_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData)
                {
                    var chartofAccount  = grdPLAccounts.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;

                    //var transactions = chartofAccount.JournalTransactions.Where(x =>
                    //  x.AdminBill?.isVoid != true &&
                    //  x.Bill?.isVoid != true &&
                    //  x.journalVoucher?.isVoid != true &&
                    //  x.InterBank?.isVoid != true &&
                    //  x.Payment?.isVoid != true &&
                    //  x.SaleInvoice?.isVoid != true &&
                    //  x.SalesReceipt?.isVoid != true &&
                    //  x.InterCompanyTransfer?.isVoid != true &&
                    //  x.deptId != null &&
                    //  deptIds.Contains((int)x.deptId) &&
                    //  x.companyId != null &&
                    //  companyIds.Contains((int)x.companyId)).ToList();
                    //var totalDebits = transactions.Sum(x => x.debit);
                    //var totalDebitsPKR = transactions.Sum(x => x.debit * x.MER);

                    //var totalCredits = transactions.Sum(x => x.credit);
                    //var totalCreditsPKR = transactions.Sum(x => x.credit * x.MER);



                    if (e.Column.FieldName == "Company")
                    {
                        var chartofAccount1 = grdPLAccounts.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
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
                       
                            var chartofAccount2 = grdPLAccounts.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
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
                                var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                                                    x.AdminBill?.isVoid != true &&
                                                     x.Bill?.isVoid != true &&
                                                     x.journalVoucher?.isVoid != true &&
                                                     x.InterBank?.isVoid != true &&
                                                     x.Payment?.isVoid != true &&
                                                     x.SaleInvoice?.isVoid != true &&
                                                     x.SalesReceipt?.isVoid != true &&
                                                      x.PurchaseInvoice?.isVoid != true &&
                                                     x.InterCompanyTransfer?.isVoid != true &&
                                                     x.creationDate != null &&
                                                     x.creationDate < (DateTime)dateFrom.EditValue &&
                                                     x.deptId != null && deptIds.Contains((int)x.deptId)&&
                                                     x.companyId==company.Id
                                                     
                                                &&
                                                x.companyId != null && companyIds.Contains((int)x.companyId)

                                              ).Sum(x => x.total);
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
                                 x.creationDate >= (DateTime)dateFrom.EditValue &&   
                                 x.creationDate <= (DateTime)dateTo.EditValue &&   
                                
                                                            deptIds.Contains((int)x.deptId) &&
                                                            x.companyId != null &&
                                                            x.companyId == company.Id &&
                                                            companyIds.Contains((int)x.companyId)).ToList();
                                var value = journaltransactions
                                                    .Sum(x => x.total);

                                value = value + openingBalance;

                                e.Value = e.Value = Math.Round(value, 2);
                            }
                        }
                        else
                        {
                            if (chartofAccount != null)
                            {
                                var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                           x.AdminBill?.isVoid != true &&
                            x.Bill?.isVoid != true &&
                            x.journalVoucher?.isVoid != true &&
                            x.InterBank?.isVoid != true &&
                            x.Payment?.isVoid != true &&
                            x.SaleInvoice?.isVoid != true &&
                            x.SalesReceipt?.isVoid != true &&
                             x.PurchaseInvoice?.isVoid != true &&
                            x.InterCompanyTransfer?.isVoid != true &&
                            x.creationDate != null &&
                            x.creationDate < (DateTime)dateFrom.EditValue &&
                            x.deptId != null && deptIds.Contains((int)x.deptId)
                       &&
                       x.companyId != null && companyIds.Contains((int)x.companyId)

                     ).Sum(x => x.total);

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
                              && x.creationDate >= (DateTime)dateFrom.EditValue
                              && x.creationDate <= (DateTime)dateTo.EditValue
                              && deptIds.Contains((int)x.deptId) 
                              && x.companyId != null
                             
                              && companyIds.Contains((int)x.companyId)).ToList();
                              var value = journalTransactions.Sum(x => x.total);
                              value = value + openingBalance;
                              e.Value = e.Value = Math.Round(value, 2);
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
                                var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                                               x.AdminBill?.isVoid != true &&
                                                x.Bill?.isVoid != true &&
                                                x.journalVoucher?.isVoid != true &&
                                                x.InterBank?.isVoid != true &&
                                                x.Payment?.isVoid != true &&
                                                x.SaleInvoice?.isVoid != true &&
                                                x.SalesReceipt?.isVoid != true &&
                                                 x.PurchaseInvoice?.isVoid != true &&
                                                x.InterCompanyTransfer?.isVoid != true &&
                                                x.creationDate != null &&
                                                x.creationDate < (DateTime)dateFrom.EditValue &&
                                                x.deptId != null && deptIds.Contains((int)x.deptId)&&
                                                x.companyId != null && companyIds.Contains((int)x.companyId)).Sum(x => x.total * x.MER);
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
                                x.creationDate >= (DateTime)dateFrom.EditValue &&
                                x.creationDate <= (DateTime)dateTo.EditValue &&
                                x.deptId != null && deptIds.Contains((int)x.deptId) &&
                                x.companyId != null && companyIds.Contains((int)x.companyId) &&
                                x.companyId == company.Id).ToList();
                                var value = journalTransactions.Sum(x => x.total * x.MER);
                                value = value + openingBalance;
                                e.Value = value;
                            }
                        }
                        else
                        {
                            if (chartofAccount != null)
                            {
                                var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                                              x.AdminBill?.isVoid != true &&
                                               x.Bill?.isVoid != true &&
                                               x.journalVoucher?.isVoid != true &&
                                               x.InterBank?.isVoid != true &&
                                               x.Payment?.isVoid != true &&
                                               x.SaleInvoice?.isVoid != true &&
                                               x.SalesReceipt?.isVoid != true &&
                                                x.PurchaseInvoice?.isVoid != true &&
                                               x.InterCompanyTransfer?.isVoid != true &&
                                               x.creationDate != null &&
                                               x.creationDate < (DateTime)dateFrom.EditValue &&
                                               x.deptId != null && deptIds.Contains((int)x.deptId) &&
                                               x.companyId != null && companyIds.Contains((int)x.companyId)).Sum(x => x.total * x.MER);
                                var journalTransactions = chartofAccount.JournalTransactions.Where(
                                x => x.AdminBill?.isVoid != true
                                 && x.Bill?.isVoid != true
                                 && x.PurchaseInvoice?.isVoid != true
                                 && x.SaleInvoice?.isVoid != true
                                 && x.SalesReceipt?.isVoid != true
                                 && x.journalVoucher?.isVoid != true
                                 && x.Payment?.isVoid != true
                                 && x.InterBank?.isVoid != true
                                 && x.creationDate >= (DateTime)dateFrom.EditValue
                                 && x.creationDate <= (DateTime)dateTo.EditValue
                                 && x.creationDate != null
                                 && x.InterCompanyTransfer?.isVoid != true
                                 && x.deptId != null && deptIds.Contains((int)x.deptId)
                                 && x.companyId != null && companyIds.Contains((int)x.companyId)
                                ).ToList();
                                var value = journalTransactions.Sum(x => x.total * x.MER);
                                value = value + openingBalance;
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

        private void mbtnAllCompanies_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            grdPLAccounts.RefreshData();
            grdexpennseAccounts.RefreshData();
        }

        private void grdexpennseAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedAccount = grdexpennseAccounts.SelectedItem as ChartofAccount;
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

                //ledger.deptIds = deptIds;
                //ledger.companyIds = companyIds;

            }
        }

        private void grdexpennseAccounts_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            try
            {
                if (e.IsGetData)
                {
                    var chartofAccount = grdexpennseAccounts.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
             
                    if (e.Column.FieldName == "Company")
                    {
                        var chartofAccount1 = grdexpennseAccounts.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
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

                        var chartofAccount2 = grdexpennseAccounts.GetRowByListIndex(e.ListSourceRowIndex) as ChartofAccount;
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

                                var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                                                 x.AdminBill?.isVoid != true &&
                                                  x.Bill?.isVoid != true &&
                                                  x.journalVoucher?.isVoid != true &&
                                                  x.InterBank?.isVoid != true &&
                                                  x.Payment?.isVoid != true &&
                                                  x.SaleInvoice?.isVoid != true &&
                                                  x.SalesReceipt?.isVoid != true &&
                                                   x.PurchaseInvoice?.isVoid != true &&
                                                  x.InterCompanyTransfer?.isVoid != true &&
                                                  x.creationDate != null &&
                                                  x.creationDate < (DateTime)dateFrom.EditValue &&
                                                  x.companyId==company.Id &&
                                                  x.deptId != null && deptIds.Contains((int)x.deptId)
                                             &&
                                             x.companyId != null && companyIds.Contains((int)x.companyId)

                                           ).Sum(x => x.total);

                                var journalTransactions = chartofAccount.JournalTransactions.Where(
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
                                                            x.companyId == company.Id
                                                             &&
                                   x.creationDate >= (DateTime)dateFrom.EditValue &&
                                 x.creationDate <= (DateTime)dateTo.EditValue
                               &&
                                                            companyIds.Contains((int)x.companyId)
                                                    ).ToList();

                                var value = journalTransactions.Sum(x => x.total);
                                value = value + openingBalance;
                                e.Value = e.Value = Math.Round(value, 2);
                            }
                        }
                        else
                        {
                            if (chartofAccount != null)
                            {
                                var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                                                     x.AdminBill?.isVoid != true &&
                                                      x.Bill?.isVoid != true &&
                                                      x.journalVoucher?.isVoid != true &&
                                                      x.InterBank?.isVoid != true &&
                                                      x.Payment?.isVoid != true &&
                                                      x.SaleInvoice?.isVoid != true &&
                                                      x.SalesReceipt?.isVoid != true &&
                                                       x.PurchaseInvoice?.isVoid != true &&
                                                      x.InterCompanyTransfer?.isVoid != true &&
                                                      x.creationDate != null &&
                                                      x.creationDate<(DateTime)dateFrom.EditValue &&
                                                      x.deptId != null && deptIds.Contains((int)x.deptId)
                                                 &&
                                                 x.companyId != null && companyIds.Contains((int)x.companyId)
         
                                               ).Sum(x => x.total);


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
                              && x.creationDate >= (DateTime)dateFrom.EditValue 
                              && x.creationDate <= (DateTime)dateTo.EditValue
                              && x.creationDate != null 
                              && x.deptId != null 
                              && deptIds.Contains((int)x.deptId) 
                              && x.companyId != null 
                              && companyIds.Contains((int)x.companyId)
                            ).ToList();
                                var value = journalTransactions.Sum(x => x.total);



                                value = value + openingBalance;

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


                                var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                                                 x.AdminBill?.isVoid != true &&
                                                  x.Bill?.isVoid != true &&
                                                  x.journalVoucher?.isVoid != true &&
                                                  x.InterBank?.isVoid != true &&
                                                  x.Payment?.isVoid != true &&
                                                  x.SaleInvoice?.isVoid != true &&
                                                  x.SalesReceipt?.isVoid != true &&
                                                   x.PurchaseInvoice?.isVoid != true &&
                                                  x.InterCompanyTransfer?.isVoid != true &&
                                                  x.creationDate != null &&
                                                  x.creationDate < (DateTime)dateFrom.EditValue &&
                                                  x.companyId == company.Id &&
                                                  x.deptId != null && deptIds.Contains((int)x.deptId)
                                             &&
                                             x.companyId != null && companyIds.Contains((int)x.companyId)

                                           ).Sum(x => x.total * x.MER);
                                var journalTransactions = chartofAccount.JournalTransactions.Where(
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

                               &&
                                   x.creationDate >= (DateTime)dateFrom.EditValue &&
                                 x.creationDate <= (DateTime)dateTo.EditValue

                                && x.InterCompanyTransfer?.isVoid != true
                                &&
                               x.deptId != null && deptIds.Contains((int)x.deptId)
                               &&
                               x.companyId != null && companyIds.Contains((int)x.companyId) &&
                                x.companyId == company.Id
                               ).ToList();
                                var value = journalTransactions.Sum(x => x.total * x.MER);
                                value = value + openingBalance;
                                e.Value = value;
                            }
                        }
                        else
                        {
                            if (chartofAccount != null)
                            {


                                var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                                                    x.AdminBill?.isVoid != true &&
                                                     x.Bill?.isVoid != true &&
                                                     x.journalVoucher?.isVoid != true &&
                                                     x.InterBank?.isVoid != true &&
                                                     x.Payment?.isVoid != true &&
                                                     x.SaleInvoice?.isVoid != true &&
                                                     x.SalesReceipt?.isVoid != true &&
                                                      x.PurchaseInvoice?.isVoid != true &&
                                                     x.InterCompanyTransfer?.isVoid != true &&
                                                     x.creationDate != null &&
                                                     x.creationDate < (DateTime)dateFrom.EditValue &&
                                                     x.deptId != null && deptIds.Contains((int)x.deptId)
                                                &&
                                                x.companyId != null && companyIds.Contains((int)x.companyId)
                                                 
                                              ).Sum(x => x.total* x.MER);

                                var journalTransactions = chartofAccount.JournalTransactions.Where(
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
                            &&
                                   x.creationDate >= (DateTime)dateFrom.EditValue &&
                                 x.creationDate <= (DateTime)dateTo.EditValue

                             && x.InterCompanyTransfer?.isVoid != true
                             &&
                            x.deptId != null && deptIds.Contains((int)x.deptId)
                            &&
                            x.companyId != null && companyIds.Contains((int)x.companyId)
                            ).ToList();
                                var value = journalTransactions.Sum(x => x.total * x.MER);

                                value = value + openingBalance;

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

        private void grdPLAccounts_CustomSummary(object sender, CustomSummaryEventArgs e)
        {
            if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "balanceOC")
            {
                GridSummaryItem item = e.Item as GridSummaryItem;
                if (item.FieldName == "balanceOC")
                {
                    switch (e.SummaryProcess)
                    {
                        case CustomSummaryProcess.Start:
                            sumPLOC = 0;
                            break;
                        case CustomSummaryProcess.Calculate:
                           
                            var budgetFieldValue = Convert.ToDecimal(e.GetValue("balanceOC"));
                           
                                sumPLOC = sumPLOC + budgetFieldValue;
                          
                            break;
                        case CustomSummaryProcess.Finalize:
                            if (sumPLOC != 0)
                            {
                                

                                e.TotalValue = sumPLOC;
                                txtGrossProfitOC.Text = sumPLOC.ToString();
                            }
                            break;
                    }
                }

            }
            else
            if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "balancePKR")
            {
                GridSummaryItem item = e.Item as GridSummaryItem;
                if (item.FieldName == "balancePKR")
                {
                    switch (e.SummaryProcess)
                    {
                        case CustomSummaryProcess.Start:
                            sumPLPKR = 0;
                            break;
                        case CustomSummaryProcess.Calculate:

                            var budgetFieldValue = Convert.ToDecimal(e.GetValue("balancePKR"));

                            sumPLPKR = sumPLPKR + budgetFieldValue;

                            break;
                        case CustomSummaryProcess.Finalize:
                            if (sumPLPKR != 0)
                            {

                                e.TotalValue = sumPLPKR;
                                txtGrossProfitPKR.Text = sumPLPKR.ToString();
                            }
                            break;
                    }
                }

            }
        }

        private void grdexpennseAccounts_CustomSummary(object sender, CustomSummaryEventArgs e)
        {
            if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "balanceOC")
            {
                GridSummaryItem item = e.Item as GridSummaryItem;
                if (item.FieldName == "balanceOC")
                {
                    switch (e.SummaryProcess)
                    {
                        case CustomSummaryProcess.Start:
                            sumExpenseOC = 0;
                            break;
                        case CustomSummaryProcess.Calculate:

                            var budgetFieldValue = Convert.ToDecimal(e.GetValue("balanceOC"));

                            sumExpenseOC = sumExpenseOC + budgetFieldValue;

                            break;
                        case CustomSummaryProcess.Finalize:
                            if (sumExpenseOC != 0)
                            {
                                e.TotalValue = sumExpenseOC;
                                var netProfit = sumExpenseOC - Convert.ToDecimal(txtGrossProfitOC.Text);
                                txtNetProfit.Text = netProfit.ToString();
                            }
                            break;
                    }
                }

            }
            else
           if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "balancePKR")
            {
                GridSummaryItem item = e.Item as GridSummaryItem;
                if (item.FieldName == "balancePKR")
                {
                    switch (e.SummaryProcess)
                    {
                        case CustomSummaryProcess.Start:
                            sumExpensePKR = 0;
                            break;
                        case CustomSummaryProcess.Calculate:

                            var budgetFieldValue = Convert.ToDecimal(e.GetValue("balancePKR"));

                            sumExpensePKR = sumExpensePKR + budgetFieldValue;

                            break;
                        case CustomSummaryProcess.Finalize:
                            if (sumExpensePKR != 0)
                            {

                                e.TotalValue = sumExpensePKR;
                                var totalNetProfitPKR = sumExpensePKR - Convert.ToDecimal(txtGrossProfitPKR.Text);
                                txtNetProfitPKR.Text = totalNetProfitPKR.ToString();
                            }
                            break;
                    }
                }
            }
        }

        private void BtnDateFilter_Click(object sender, RoutedEventArgs e)
        {
            grdPLAccounts.RefreshData();
            grdexpennseAccounts.RefreshData();
        }
    }
}
