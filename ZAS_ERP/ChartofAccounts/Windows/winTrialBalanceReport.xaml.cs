using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Reports;
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
using ZAS_ERP.Reportss;


namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winTrialBalanceReport.xaml
    /// </summary>
    public partial class winTrialBalanceReport : DXWindow
    {
        ChartofAccountsRepo repo = new ChartofAccountsRepo();
        CoaLogicClass ModuleLogic = new CoaLogicClass();
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        CompanyRepo companyRepo = new CompanyRepo();
        bool isFirstLoad = true;
        CurrencyRepo currencyRepo = new CurrencyRepo();
        Company reportCompany = new Company();
        DateTime from;
        DateTime To;
        EmployeeRepo employeeRepo = new EmployeeRepo();
        GridReport report = new GridReport();
        public winTrialBalanceReport()
        {
            InitializeComponent();
        }
        public winTrialBalanceReport(GridReport _report,  string _reportName)
        {
            InitializeComponent();
            report = _report;
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var companies = employeeRepo.GetUserCOACompanies(SYSTEM_STATIC.currentUser.employeeId);
            var companies = SYSTEM_STATIC.currentUser.employee.Companies;
            companyIds.AddRange(companies.Select(x => x.Id).ToList());
            deptIds.AddRange(SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id));
            LoadCompanies();
            datAsof.EditValue = DateTime.Now;
            if(report.company!=null)
            {
                mbtnAllCompanies.Text = report.company.CompanyName;
                datFrom.EditValue = report.from;
                datTo.EditValue = report.to;
            }
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here       
                            mbtnExportToStandardReport1.IsVisible = false;
                            mbtnShareReport.IsVisible = false;
                            grdTrialBalanceList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = lblHeading.Text + "/" +  report.gridReportGroup.groupName + "/" + report.reportName;
                            lblHeading1.Caption = lblHeading.Text + "/" + report.gridReportGroup.groupName + "/" + report.reportName;
                            LoadAllTransactions();
                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (report != null)
                        {
                            mbtnShareReport.IsVisible = false;
                            mbtnExportToMemorizedReport1.IsVisible = false;
                            grdTrialBalanceList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = lblHeading.Text + "/" + report.gridReportGroup.groupName + "/" + report.reportName;
                            lblHeading1.Caption = lblHeading.Text + "/" + report.gridReportGroup.groupName + "/" + report.reportName;
                            LoadAllTransactions();
                        }
                    }
                    break;
            }

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
            var selectedAccount = grdTrialBalanceList.SelectedItem as ChartofAccountViewModel;
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
        private void grdTrialBalanceList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
           
            
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAllTransactions();
        }

        private void MbtnRenameReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to rename " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.report = report;
                                setReportName.editableGroup = group;
                                //setting input Fields
                                setReportName.txtName.Text = report.reportName;
                                //setReportName.LoadReportTypes();
                                setReportName.GetEnum();
                                setReportName.reportTypes.EditValue = report.gridReportType;
                                setReportName.lookupGroup.EditValue = group.groupName;
                                setReportName.LoadGroups();
                                setReportName.ShowDialog();
                                var updatedReport = setReportName.report;
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {

                                        if (mbtnAllCompanies.SelectedIndex > -1)
                                        {

                                            ReportLogic.RenameGridReport(grdTrialBalanceList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup,

                                                updatedReport.Id, report.settingkey, mbtnAllCompanies.SelectedItem as Company, (DateTime)datFrom.EditValue, (DateTime)datTo.EditValue);
                                        }
                                        else
                                        {
                                            ReportLogic.RenameGridReport(grdTrialBalanceList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        }










                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        this.Close();
                                    }
                                    else
                                        DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.report = report;
                                setReportName.editableGroup = group;
                                //setting input Fields
                                setReportName.txtName.Text = report.reportName;
                                //setReportName.LoadReportTypes();
                                setReportName.GetEnum();
                                setReportName.reportTypes.EditValue = report.gridReportType;
                                setReportName.lookupGroup.EditValue = group.groupName;
                                setReportName.LoadGroups();
                                setReportName.ShowDialog();
                                var updatedReport = setReportName.report;
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        if (mbtnAllCompanies.SelectedIndex > -1)
                                        {
                                            ReportLogic.RenameGridReport(grdTrialBalanceList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup,
                                            updatedReport.Id, report.settingkey, mbtnAllCompanies.SelectedItem as Company, (DateTime)datFrom.EditValue, (DateTime)datTo.EditValue);
                                        }
                                        else
                                        {
                                            ReportLogic.RenameGridReport(grdTrialBalanceList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        }
                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        this.Close();
                                    }
                                    else
                                        DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }

            }
        }

        private void MbtnSaveAsNew1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Save as new Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var saveAsNewReport = setReportName.report;
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.gridReportType != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    if (mbtnAllCompanies.SelectedIndex > -1)
                                    {

                                        ReportLogic.SaveGridReport(grdTrialBalanceList, reportName, reportType, reportGroup,

                                            report.settingkey, mbtnAllCompanies.SelectedItem as Company, (DateTime)datFrom.EditValue, (DateTime)datTo.EditValue);
                                    }
                                    else
                                    {
                                        ReportLogic.SaveGridReport(grdTrialBalanceList, reportName, reportType, reportGroup,report.settingkey, report.titleId);
                                    }
                                    DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to save as new Standard Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
                case GridReportType.MemorizedReport:
                    {

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Save as new Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var saveAsNewReport = setReportName.report;
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.gridReportType != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    if (mbtnAllCompanies.SelectedIndex > -1)
                                    {

                                        ReportLogic.SaveGridReport(grdTrialBalanceList, reportName, reportType, reportGroup,

                                            report.settingkey, mbtnAllCompanies.SelectedItem as Company, (DateTime)datFrom.EditValue, (DateTime)datTo.EditValue);
                                    }
                                    else
                                    {
                                        ReportLogic.SaveGridReport(grdTrialBalanceList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
                                    }
                                    DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to save as new Memorized Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
            }
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdTrialBalanceList.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = report.reportName;
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = report.reportName;
            link.ReportHeaderData = report.reportName;
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdsaleOrder.View.ShowPrintPreview(this);
        }

        private void MbtnUpdateReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.Title + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                string str = report.reportName;
                                GridReportType reportType = report.gridReportType;
                                GridReportGroup groupDetails = group;
                                if (str != "")
                                {



                                    if (mbtnAllCompanies.SelectedIndex > -1)
                                    {
                                        ReportLogic.UpdateGridReport(grdTrialBalanceList, str, reportType, groupDetails, report.Id, report.settingkey, mbtnAllCompanies.SelectedItem as Company, (DateTime)datFrom.EditValue, (DateTime)datTo.EditValue);
                                    }
                                    else
                                    {
                                        ReportLogic.UpdateGridReport(grdTrialBalanceList, str, reportType, groupDetails, report.Id, report.settingkey);
                                    }

                                    DXMessageBox.Show(" ( " + str + " ) is Updated Successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                return;
                            }
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                string str = report.reportName;
                                GridReportType reportType = report.gridReportType;
                                GridReportGroup groupDetails = group;
                                if (str != "")
                                {
                                    if (mbtnAllCompanies.SelectedIndex > -1)
                                    {
                                        ReportLogic.UpdateGridReport(grdTrialBalanceList, str, reportType, groupDetails, report.Id, report.settingkey, mbtnAllCompanies.SelectedItem as Company, (DateTime)datFrom.EditValue, (DateTime)datTo.EditValue);
                                    }
                                    else
                                    {
                                        ReportLogic.UpdateGridReport(grdTrialBalanceList, str, reportType, groupDetails, report.Id, report.settingkey);
                                    }

                                    DXMessageBox.Show(" ( " + str + " ) is Updated Successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                return;
                            }
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
            }
        }

        private void MbtnDeleteReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to delete " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                             
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                ReportLogic.DeleteReport(grdTrialBalanceList, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Delete Standard report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        break;
                    }

                case GridReportType.MemorizedReport:
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                /*GridReport report = repo.GetReportByName(this.Title)*/
                                ;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                ReportLogic.DeleteReport(grdTrialBalanceList, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Delete Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        break;
                    }

            }
        }

        private void btnRefresh_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdTrialBalanceList.ShowLoadingPanel = true;
            LoadAllTransactions();
            grdTrialBalanceList.ShowLoadingPanel = false;
        }

        private void MbtnExportToMemorizedReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Memorized Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Export to Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var type = report.gridReportType;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var exportToMemorizedReport = setReportName.report;
                                if (exportToMemorizedReport != null && exportToMemorizedReport.gridReportGroup != null && exportToMemorizedReport.gridReportType != null && exportToMemorizedReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToMemorizedReport.gridReportGroup;
                                    var reportType = exportToMemorizedReport.gridReportType;
                                    var reportName = exportToMemorizedReport.reportName;
                                    ReportLogic.SaveGridReport(grdTrialBalanceList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
                                    DXMessageBox.Show("( " + reportName + " ) is exported to Memorized reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }

                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }

            }
        }

        private void MbtnExportToStandardReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Standard Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


            switch (report.gridReportType)
            {
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Export to Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var type = report.gridReportType;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var exportToStandardReport = setReportName.report;
                                if (exportToStandardReport != null && exportToStandardReport.gridReportGroup != null && exportToStandardReport.gridReportType != null && exportToStandardReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToStandardReport.gridReportGroup;
                                    var reportType = exportToStandardReport.gridReportType;
                                    var reportName = exportToStandardReport.reportName;
                                    ReportLogic.ExportToStandard(grdTrialBalanceList, reportName, reportType, reportGroup, report.settingkey);
                                    DXMessageBox.Show("( " + reportName + " ) is exported to Standard reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }

                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }

            }
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }
    }
}
