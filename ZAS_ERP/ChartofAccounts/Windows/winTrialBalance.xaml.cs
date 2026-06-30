using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winTrialBalance.xaml
    /// </summary>
    public partial class winTrialBalance : DXWindow
    {
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        CoaLogicClass ModuleLogic = new CoaLogicClass();
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        CompanyRepo companyRepo = new CompanyRepo();
        bool isFirstLoad = true;
        CurrencyRepo currencyRepo = new CurrencyRepo();
        EmployeeRepo employeeRepo = new EmployeeRepo();

        public winTrialBalance()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datAsof.EditValue = DateTime.Now;
            lblAsOf.Text = datAsof.EditValue.ToString();

            var companies = SYSTEM_STATIC.currentUser.employee.Companies;
            companyIds.AddRange(companies.Select(x => x.Id));
            deptIds.AddRange(SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id));

            LoadCompanies();
            LoadAllTransactions();
            LoadAllCurrency();
        }

        public void LoadCompanies()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mbtnAllCompanies.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        public void LoadAllCurrency()
        {
            var currencies = currencyRepo.getAll()
                .Where(x => !x.isVoid)
                .ToList();
            mbtnAllCurrencies.ItemsSource = currencies;
        }

        private void GrdTrialBalanceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdTrialBalanceList.SelectedItem is ChartofAccountViewModel selectedAccount)
            {
                var company = mbtnAllCompanies.SelectedItem as Company;
                var chartofAccount = repo.get(selectedAccount.Id);

                var ledger = company != null
                    ? new winLedger(chartofAccount, company)
                    : new winLedger(chartofAccount);

                ledger.Title = "Transactions";
                ledger.lblHeading.Text = $"All Transactions by Account: {selectedAccount.accountName}";
                ledger.Show();
            }
        }

        public double GetMER(DateTime creationDate, int transactionCurrencyId, int reportingCurrencyId, int companyId)
        {

            if (transactionCurrencyId == reportingCurrencyId)
            {
                return 1;

            }
            else
            {
                ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
                var exchangeRateGroupMER = SYSTEM_STATIC.exchangeRateGroups.FirstOrDefault(x => x.transaction_currency_Id == transactionCurrencyId && x.base_currency_Id == reportingCurrencyId && x.TargetYear == creationDate.Year);
                double rate = 0;
                ExchangeRate exchangeRate = null;
                if (exchangeRateGroupMER != null)
                {
                    exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == companyId);
                    switch (creationDate.Month)
                    {
                        case 1:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateJan;
                            break;
                        case 2:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateFeb;
                            break;
                        case 3:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateMar;
                            break;
                        case 4:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateApr;
                            break;
                        case 5:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateMay;
                            break;
                        case 6:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateJun;
                            break;
                        case 7:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateJul;
                            break;
                        case 8:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateAug;
                            break;
                        case 9:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateSep;
                            break;
                        case 10:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateOct;
                            break;
                        case 11:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateNov;
                            break;
                        case 12:
                            if (exchangeRate != null)
                                rate = exchangeRate.rateDec;
                            break;
                        default:
                            if (exchangeRate != null)
                                rate = 0;
                            break;
                    }
                }
                if (exchangeRate == null)
                {
                    rate = 0;
                }
                return rate;
            }


        }

        public void LoadAllTransactions()
        {

            try
            {

                var company = mbtnAllCompanies.SelectedItem as Company;
                var isCompanySelected = company != null;
                var dateAsOf = datAsof.EditValue as DateTime?;
                var dateFrom = datFrom.EditValue as DateTime?;
                var dateTo = datTo.EditValue as DateTime?;

                var chartofAccounts = repo.getAllTrialBalanceAccounts(SYSTEM_STATIC.currentUser.id)
                    .Where(account => account.JournalTransactions != null)
                    .ToList();

                var modelList = new List<ChartofAccountViewModel>();

                Parallel.ForEach(chartofAccounts, currentElement =>
                {
                    var journalTransactions = currentElement.JournalTransactions
                        ?.GroupBy(x => x.Id).Select(g => g.First()).ToList();

                    if (journalTransactions != null)
                    {
                        journalTransactions = journalTransactions
                            .Where(x => x.deptId.HasValue && x.companyId.HasValue &&
                                        deptIds.Contains(x.deptId.Value) &&
                                        companyIds.Contains(x.companyId.Value) &&
                                        (!isCompanySelected || x.companyId == company?.Id) &&
                                        (!dateAsOf.HasValue || x.creationDate <= dateAsOf) &&
                                        IsTransactionValid(x))
                            .ToList();

                        if (dateFrom.HasValue && dateTo.HasValue)
                        {
                            journalTransactions = journalTransactions
                                .Where(x => x.creationDate >= dateFrom.Value.Date.AddHours(12) &&
                                            x.creationDate <= dateTo.Value.Date.AddHours(35.9999))
                                .ToList();
                        }

                        if (journalTransactions.Any())
                        {
                            var model = new ChartofAccountViewModel
                            {
                                Id = currentElement.Id,
                                accountName = currentElement.accountName,
                                accountType = currentElement.accountType,
                                Currency = currentElement.Currency
                            };

                            double totalValue = journalTransactions.Sum(x => x.total);
                            double totalValuePKR = journalTransactions.Sum(x => x.total * x.MER);

                            UpdateModelCreditDebit(model, totalValue, totalValuePKR, currentElement.accountType);
                            lock (modelList)
                            {
                                modelList.Add(model);
                            }
                        }
                    }
                });

                grdTrialBalanceList.ItemsSource = isCompanySelected
                    ? modelList.Where(x => x.debit != 0 || x.credit != 0).ToList()
                    : modelList;

            }
            catch (Exception)
            {

            }
        }

        private bool IsTransactionValid(JournalTransaction transaction)
        {
            return transaction.AdminBill?.isVoid != true &&
                   transaction.Bill?.isVoid != true &&
                   transaction.journalVoucher?.isVoid != true &&
                   transaction.InterBank?.isVoid != true &&
                   transaction.Payment?.isVoid != true &&
                   transaction.SaleInvoice?.isVoid != true &&
                   transaction.PurchaseInvoice?.isVoid != true &&
                   transaction.SalesReceipt?.isVoid != true &&
                   transaction.InterCompanyTransfer?.isVoid != true;
        }

        private void UpdateModelCreditDebit(ChartofAccountViewModel model, double value, double valuePKR, COA_AccountType accountType)
        {
            if (IsCreditType(accountType))
            {
                model.credit = Math.Abs(value > 0 ? value : 0);
                model.debit = Math.Abs(value < 0 ? value : 0);
                model.creditPKR = Math.Abs(valuePKR > 0 ? valuePKR : 0);
                model.debitPKR = Math.Abs(valuePKR < 0 ? valuePKR : 0);
            }
            else if (IsDebitType(accountType))
            {
                model.debit = Math.Abs(value > 0 ? value : 0);
                model.credit = Math.Abs(value < 0 ? value : 0);
                model.debitPKR = Math.Abs(valuePKR > 0 ? valuePKR : 0);
                model.creditPKR = Math.Abs(valuePKR < 0 ? valuePKR : 0);
            }
        }

        private bool IsCreditType(COA_AccountType accountType)
        {
            return accountType == COA_AccountType.Income ||
                   accountType == COA_AccountType.Loan ||
                   accountType == COA_AccountType.Credit_Card ||
                   accountType == COA_AccountType.Equity ||
                   accountType == COA_AccountType.Other_Income ||
                   accountType == COA_AccountType.Accounts_Payable ||
                   accountType == COA_AccountType.Longterm_Liability ||
                   accountType == COA_AccountType.Other_Current_Liability;
        }

        private bool IsDebitType(COA_AccountType accountType)
        {
            return accountType == COA_AccountType.Expense ||
                   accountType == COA_AccountType.Other_Expense ||
                   accountType == COA_AccountType.Fixed_Asset ||
                   accountType == COA_AccountType.Other_Current_Asset ||
                   accountType == COA_AccountType.Bank ||
                   accountType == COA_AccountType.Cost_of_Goods_Sold ||
                   accountType == COA_AccountType.Account_Receivable ||
                   accountType == COA_AccountType.Other_Asset;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAllTransactions();
        }

        private void btnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Trial Balance Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    var company = mbtnAllCompanies.SelectedItem as Company;

                    if (company == null)
                    {

                        Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblHeading.Text);
                        setReportName.ShowDialog();
                        var report = setReportName.report;
                        if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                        {
                            var reportGroup = report.gridReportGroup;
                            var reportType = report.gridReportType;
                            var reportName = report.reportName;
                            ReportLogic.SaveGridReport(grdTrialBalanceList, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                            DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                    }
                    else
                    {
                        if (datFrom.EditValue == null)
                        {
                            DXMessageBox.Show("Please select from date");
                            return;
                        }
                        else
                        if (datTo.EditValue == null)
                        {
                            DXMessageBox.Show("Please select to date");
                            return;
                        }
                        else
                        {
                            Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblHeading.Text);
                            setReportName.ShowDialog();
                            var report = setReportName.report;
                            if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                            {
                                var reportGroup = report.gridReportGroup;
                                var reportType = report.gridReportType;
                                var reportName = report.reportName;
                                ReportLogic.SaveGridReport(grdTrialBalanceList, reportName, reportType, reportGroup, lblHeading.Text, company, (DateTime)datFrom.EditValue, (DateTime)datTo.EditValue);
                                DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Sale Orders Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void grdTrialBalanceList_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
           
        }
    }

    public class ChartofAccountViewModel
    {
        public int Id { get; set; }
        public string accountName { get; set; }
        public COA_AccountType accountType { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public Currency Currency { get; set; }
        public double debit { get; set; }
        public double credit { get; set; }  
        public double debitPKR { get; set; }
        public double creditPKR { get; set; }
        public double debitRC { get; set; }
        public double creditRC { get; set; }

    }
    
}
