using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ZAS_ERP.Bankings;
using ZAS_ERP.ChartofAccounts.ViewModels;
//using static DevExpress.XtraExport.Helpers.TableRowControl;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winLedger.xaml
    /// </summary>
    public partial class winLedger : DXWindow
    {
        JournalEntryRepo repo = new JournalEntryRepo();
        CoaLogicClass ModuleLogic = new CoaLogicClass();
        List<int> check = new List<int>();
        ChartofAccountsRepo chartofAccountsRepo = new ChartofAccountsRepo();
        public List<int> deptIds = new List<int>();
        public List<int> companyIds = new List<int>();
        ChartofAccount chartofAccount = new ChartofAccount();
        List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
        Company company = new Company();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        double balanceOC = 0;
        double balanceMER = 0;
        public winLedger()
        {
            InitializeComponent();
        }
        public winLedger(ChartofAccount _account)
        {
            InitializeComponent();
            chartofAccount = _account;

            //var companies=  employeeRepo.GetUserCOACompanies(SYSTEM_STATIC.currentUser.employeeId);
            //companyIds.AddRange(companies.Select(x => x.Id).ToList());
            //deptIds.AddRange(SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id));

            companyIds = SYSTEM_STATIC.currentUser.employee.Companies.Select(x => x.Id).ToList();
            deptIds = SYSTEM_STATIC.currentUser.employee.departments.Select(x => x.Id).ToList();

        }
        public winLedger(ChartofAccount _account, Company _company)
        { 
            InitializeComponent();
            chartofAccount = _account;
            //var companies = employeeRepo.GetUserCOACompanies(SYSTEM_STATIC.currentUser.employeeId);
            //companyIds.AddRange(companies.Select(x => x.Id).ToList());
            //deptIds.AddRange(SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id));


            companyIds = SYSTEM_STATIC.currentUser.employee.Companies.Select(x => x.Id).ToList();
            deptIds = SYSTEM_STATIC.currentUser.employee.departments.Select(x => x.Id).ToList();


            company = _company;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //LoadAllAccountEntries();
            chartofAccount = chartofAccountsRepo.get(chartofAccount.Id);
            dateFrom.EditValue = DateTime.Now.AddMonths(-2);
            dateTo.EditValue = DateTime.Now;
            //dateTo.EditValue = DateTime.Now;
            CalculateBalances();

        }
        public void LoadAllAccountEntries()
        {
           chartofAccount= chartofAccountsRepo.get(chartofAccount.Id);
            if (company.Id != 0)
            {
                txtCompany.Text = company.CompanyName;
                journalTransactions = chartofAccount.JournalTransactions.Where(x =>
                            x.AdminBill?.isVoid != true &&
                                 x.Bill?.isVoid != true &&
                                 x.journalVoucher?.isVoid != true &&
                                 x.InterBank?.isVoid != true &&
                                 x.Payment?.isVoid != true &&
                                 x.SaleInvoice?.isVoid != true &&
                                 x.PurchaseInvoice?.isVoid != true &&
                                 x.SalesReceipt?.isVoid != true &&
                                 x.InterCompanyTransfer?.isVoid != true &&
                                 x.TargetReward?.isVoid != true &&
                                 x.creationDate != null &&
                                 (x.creationDate.Value.Year == DateTime.Now.Year &&
                               x.creationDate.Value.Month == DateTime.Now.Month)
                               &&
                            x.deptId != null && deptIds.Contains((int)x.deptId)
                            &&
                            x.companyId != null && companyIds.Contains((int)x.companyId)
                            && x.companyId==company.Id
                           ).ToList();
                journalTransactions.ForEach(x => x.credit = -x.credit);
                balanceOC = 0;
                balanceMER = 0;


                var openingBalanceOC = chartofAccount.JournalTransactions.Where(x =>
                   x.AdminBill?.isVoid != true &&
                    x.Bill?.isVoid != true &&
                    x.journalVoucher?.isVoid != true &&
                    x.InterBank?.isVoid != true &&
                    x.Payment?.isVoid != true &&
                    x.SaleInvoice?.isVoid != true &&
                    x.SalesReceipt?.isVoid != true &&
                     x.PurchaseInvoice?.isVoid != true &&
                    x.InterCompanyTransfer?.isVoid != true &&
                    x.TargetReward?.isVoid != true &&

                    x.creationDate != null &&
                    !(x.creationDate.Value.Year == DateTime.Now.Year &&
                    x.creationDate.Value.Month == DateTime.Now.Month) &&

                    x.deptId != null && deptIds.Contains((int)x.deptId)
               &&
               x.companyId != null && companyIds.Contains((int)x.companyId)
               && x.companyId == company.Id
             ).Sum(x => x.total);
                var openingBalanceMER = chartofAccount.JournalTransactions.Where(x =>
                  x.AdminBill?.isVoid != true &&
                   x.Bill?.isVoid != true &&
                   x.journalVoucher?.isVoid != true &&
                   x.InterBank?.isVoid != true &&
                   x.Payment?.isVoid != true &&
                   x.SaleInvoice?.isVoid != true &&
                   x.SalesReceipt?.isVoid != true &&
                    x.PurchaseInvoice?.isVoid != true &&
                   x.InterCompanyTransfer?.isVoid != true &&
                   x.TargetReward?.isVoid != true &&

                   x.creationDate != null &&
                   !(x.creationDate.Value.Year == DateTime.Now.Year &&
                   x.creationDate.Value.Month == DateTime.Now.Month) &&

                   x.deptId != null && deptIds.Contains((int)x.deptId)
              &&
              x.companyId != null && companyIds.Contains((int)x.companyId)
              && x.companyId == company.Id
            ).Sum(x => x.total * x.MER);

                txtOpeningBalanceOC.Text = Math.Round(openingBalanceOC, 2).ToString();
                var closingBalanceOC = journalTransactions.Sum(x => x.total) + Convert.ToDouble(txtOpeningBalanceOC.Text);
                txtClosingBalanceOC.Text = Math.Round(closingBalanceOC, 2).ToString();
                balanceOC = openingBalanceOC;

                txtOpeningBalanceMER.Text = Math.Round(openingBalanceMER, 2).ToString();
                var closingBalanceMER = journalTransactions.Sum(x => x.total * x.MER) + Convert.ToDouble(txtOpeningBalanceMER.Text);
                txtClosingBalanceMER.Text = Math.Round(closingBalanceMER, 2).ToString();
                balanceMER = openingBalanceMER;

                grdListEntries.ItemsSource = journalTransactions;
            }
        else
            {
                journalTransactions = chartofAccount.JournalTransactions.Where(x =>
            x.AdminBill?.isVoid != true &&
                 x.Bill?.isVoid != true &&
                 x.journalVoucher?.isVoid != true &&
                 x.InterBank?.isVoid != true &&
                 x.Payment?.isVoid != true &&
                 x.SaleInvoice?.isVoid != true &&
                 x.SalesReceipt?.isVoid != true &&
                  x.PurchaseInvoice?.isVoid != true &&
                 x.InterCompanyTransfer?.isVoid != true &&
                                                  x.TargetReward?.isVoid != true &&

                 x.creationDate != null &&
                 (x.creationDate.Value.Year == DateTime.Now.Year &&
               x.creationDate.Value.Month == DateTime.Now.Month)
               &&
            x.deptId != null && deptIds.Contains((int)x.deptId)
            &&
            x.companyId != null && companyIds.Contains((int)x.companyId)
           ).ToList();
                journalTransactions.ForEach(x => x.credit = -x.credit);
                balanceOC = 0;
                balanceMER= 0;


                var openingBalance = chartofAccount.JournalTransactions.Where(x =>
                   x.AdminBill?.isVoid != true &&
                    x.Bill?.isVoid != true &&
                    x.journalVoucher?.isVoid != true &&
                    x.InterBank?.isVoid != true &&
                    x.Payment?.isVoid != true &&
                    x.SaleInvoice?.isVoid != true &&
                    x.SalesReceipt?.isVoid != true &&
                    x.InterCompanyTransfer?.isVoid != true &&
                     x.PurchaseInvoice?.isVoid != true &&
                                                      x.TargetReward?.isVoid != true &&

                    x.creationDate != null &&
                    !(x.creationDate.Value.Year == DateTime.Now.Year &&
                    x.creationDate.Value.Month == DateTime.Now.Month) &&
                    x.deptId != null && deptIds.Contains((int)x.deptId)
               &&
               x.companyId != null && companyIds.Contains((int)x.companyId)
             ).Sum(x => x.total);
                var openingBalanceMER = chartofAccount.JournalTransactions.Where(x =>
                   x.AdminBill?.isVoid != true &&
                    x.Bill?.isVoid != true &&
                    x.journalVoucher?.isVoid != true &&
                    x.InterBank?.isVoid != true &&
                    x.Payment?.isVoid != true &&
                    x.SaleInvoice?.isVoid != true &&
                    x.SalesReceipt?.isVoid != true &&
                    x.InterCompanyTransfer?.isVoid != true &&
                     x.PurchaseInvoice?.isVoid != true &&
                                                      x.TargetReward?.isVoid != true &&

                    x.creationDate != null &&
                    !(x.creationDate.Value.Year == DateTime.Now.Year &&
                    x.creationDate.Value.Month == DateTime.Now.Month) &&
                    x.deptId != null && deptIds.Contains((int)x.deptId)
               &&
               x.companyId != null && companyIds.Contains((int)x.companyId)
             ).Sum(x => x.total * x.MER);

                txtOpeningBalanceOC.Text = Math.Round(openingBalance, 2).ToString();
                var closingBalance = journalTransactions.Sum(x => x.total) + Convert.ToDouble(txtOpeningBalanceOC.Text);
                txtClosingBalanceOC.Text = Math.Round(closingBalance, 2).ToString();
                balanceOC = openingBalance;
                
                
                txtOpeningBalanceMER.Text = Math.Round(openingBalanceMER, 2).ToString();
                var closingBalanceMER = journalTransactions.Sum(x => x.total * x.MER) + Convert.ToDouble(txtOpeningBalanceMER.Text);
                txtClosingBalanceMER.Text = Math.Round(closingBalanceMER, 2).ToString();
                balanceOC = openingBalance;
                grdListEntries.ItemsSource = journalTransactions;
            }
            
        }
        private void GrdListEntries_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                grdListEntries.ShowLoadingPanel = true;
                var selectedRow = grdListEntries.SelectedItem as JournalTransaction;
                var transaction = selectedRow;


                switch (transaction.coaTransactionsType)
                {
                    case coaTransactionsType.Bill:
                        {
                            ModuleLogic.PopulateBill(TransactionItemType.Bill, (int)transaction.Bill_Id);
                            break;
                        }
                    case coaTransactionsType.Inquiry:
                        {
                            break;
                        }

                    case coaTransactionsType.InterBankTransfer:
                        {
                            ModuleLogic.PopulateInterBankTransfer(transaction);
                            break;
                        }
                    case coaTransactionsType.JV:
                        {
                            var trans = repo.GetAllJournalTransactionById(transaction.Id);

                            ModuleLogic.PopulateJV(transaction);
                            break;
                        }

                    case coaTransactionsType.Offer:
                        {
                            break;
                        }

                    case coaTransactionsType.PurchaseOrder:
                        {
                            break;
                        }

                    case coaTransactionsType.SaleInvoice:
                        {
                            ModuleLogic.PopulateSaleInvoice(TransactionItemType.Sale_Invoice, (int)transaction.SaleInvoiceId);
                            break;
                        }

                    case coaTransactionsType.SaleOrder:
                        {
                            break;
                        }

                    case coaTransactionsType.SaleReceipt:
                        {


                            ModuleLogic.PopulateSaleSaleReceipt((int)transaction.SaleReceiptId);
                            
                            break;
                          

                        }
                    case coaTransactionsType.AdminBill:
                        {


                            ModuleLogic.PopulateAdminBills((int)transaction.AdminBillId);
                            

                            break;


                        }
                    case coaTransactionsType.Payment:
                        {

                            ModuleLogic.PopulatePayments(transaction.Payment);
                            break;
                        }
                    case coaTransactionsType.PurchaseInvoice:
                        {

                            ModuleLogic.PopulatePurchaseInvoice(TransactionItemType.Purchase_Invoice, (int)transaction.PurchaseInvoiceId);
                            break;
                        }
                    case coaTransactionsType.InterCompanyTransfer:
                        {


                            ModuleLogic.PopulateInterCompanyTransfer(transaction);

                            break;
                        }    
                    case coaTransactionsType.TragetReward:
                        {


                            ModuleLogic.PopulateInterCompanyTransfer(transaction);

                            break;
                        }
                }
                grdListEntries.ShowLoadingPanel = false;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            CalculateBalances();
        }


        public void CalculateBalances()
        {
            if (dateFrom.EditValue != null && dateTo.EditValue != null)
            {
                TimeSpan ts = new TimeSpan(12, 00, 0);
                DateTime from = (DateTime)dateFrom.EditValue;
                DateTime to = (DateTime)dateTo.EditValue;
                from = from.Date + ts;
                to = to.Date + ts;
                from = from.AddHours(-12.0);
                to = to.AddHours(-12.0);
                to = to.AddHours(24.0);
                to = to.AddSeconds(-1.0);
                List<JournalTransaction> transactions = repo.GetAllJournalTransactionByaccountId(chartofAccount.Id, deptIds);
                grdListEntries.ItemsSource = transactions;
                if (company.Id != 0)
                {
                    List<JournalTransaction> companyTransactions = transactions.Where(x => x.companyId == company.Id).ToList();
                    if (companyTransactions != null)
                    {
                        var filteredTransactions = companyTransactions.Where(x => x.creationDate.Value >= from && x.creationDate.Value <= to).ToList();
                        var openingBalanceTransactions = companyTransactions.Where(x => x.creationDate.Value < from).ToList();
                        filteredTransactions = filteredTransactions.Where(x => companyIds.Contains((int)x.companyId) && deptIds.Contains((int)x.deptId)).ToList();
                        grdListEntries.ItemsSource = filteredTransactions;
                        txtOpeningBalanceOC.Text = Math.Round(openingBalanceTransactions.Sum(x => x.total), 2).ToString();
                        var totalClosing = Convert.ToDouble(txtOpeningBalanceOC.Text) + filteredTransactions.Sum(x => x.total);
                        txtClosingBalanceOC.Text = Math.Round(totalClosing, 2).ToString();
                        txtOpeningBalanceMER.Text = Math.Round(openingBalanceTransactions.Sum(x => x.total * x.MER), 2).ToString();
                        var totalClosingMER = Convert.ToDouble(txtOpeningBalanceMER.Text) + filteredTransactions.Sum(x => x.total * x.MER);
                        txtClosingBalanceMER.Text = Math.Round(totalClosingMER, 2).ToString();
                    }
                }
                else
                {
                    if (transactions != null)
                    {
                        var filteredTransactions = transactions.Where(x => x.creationDate.Value >= from && x.creationDate.Value <= to).ToList();
                        var openingBalanceTransactions = transactions.Where(x => x.creationDate.Value < from).ToList();
                        filteredTransactions = filteredTransactions.Where(x => companyIds.Contains((int)x.companyId) && deptIds.Contains((int)x.deptId)).ToList();
                        grdListEntries.ItemsSource = filteredTransactions;
                        txtOpeningBalanceOC.Text = Math.Round(openingBalanceTransactions.Sum(x => x.total), 2).ToString();
                        var totalClosing = Convert.ToDouble(txtOpeningBalanceOC.Text) + filteredTransactions.Sum(x => x.total);
                        txtClosingBalanceOC.Text = Math.Round(totalClosing, 2).ToString();
                        txtOpeningBalanceMER.Text = Math.Round(openingBalanceTransactions.Sum(x => x.total * x.MER), 2).ToString();
                        var totalClosingMER = Convert.ToDouble(txtOpeningBalanceMER.Text) + filteredTransactions.Sum(x => x.total * x.MER);
                        txtClosingBalanceMER.Text = Math.Round(totalClosingMER, 2).ToString();
                    }
                }
            }
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
          
            PrintableControlLink link = new PrintableControlLink((TableView)grdListEntries.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdListEntries);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            grdListEntries.ShowLoadingPanel = true;
            chartofAccountsRepo = new ChartofAccountsRepo();
            LoadAllAccountEntries();
            grdListEntries.ShowLoadingPanel = false;


        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }



        private void grdListEntries_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            GridControl view = (GridControl)sender;
            if (e.Column.FieldName == "balanceAmount" & e.IsGetData)
            {
                double total = 0;
                int rHandle = view.GetRowHandleByListIndex(e.ListSourceRowIndex);
                for (int i = -1; i <= rHandle - 1; i++)
                {
                    if (i != -1)
                    {
                        total += Convert.ToDouble(view.GetCellValue(i + 1, "total"));
                    }
                    else
                    {
                        total += Convert.ToDouble(view.GetCellValue(i + 1, "total"))+ Convert.ToDouble(txtOpeningBalanceOC.Text);
                        //total += Convert.ToDouble(txtOpeningBalance.Text);
                    }
                }
                e.Value = total;
            }

            if (e.Column.FieldName == "balance" & e.IsGetData)
            {
                //double total = 0;
                double totalMER = 0;
                int rHandle = view.GetRowHandleByListIndex(e.ListSourceRowIndex);
                for (int i = -1; i <= rHandle - 1; i++)
                {
                    if (i != -1)
                    {
                        var totalOC = Convert.ToDouble(view.GetCellValue(i + 1, "total"));
                        //total += Convert.ToDouble(view.GetCellValue(i + 1, "total"));
                        totalMER += totalOC * Convert.ToDouble(view.GetCellValue(i + 1, "MER"));
                    }
                    else
                    {
                        var totalOC = Convert.ToDouble(view.GetCellValue(i + 1, "total"));
                        //total += Convert.ToDouble(view.GetCellValue(i + 1, "total"));
                        totalMER += totalOC * Convert.ToDouble(view.GetCellValue(i + 1, "MER")) + Convert.ToDouble(txtOpeningBalanceMER.Text);
                        //totalMER += Convert.ToDouble(view.GetCellValue(i + 1, "total")) + Convert.ToDouble(txtOpeningBalance.Text);
                        //total += Convert.ToDouble(txtOpeningBalance.Text);
                    }
                }
                e.Value = totalMER;
            }

        }

    }
}

