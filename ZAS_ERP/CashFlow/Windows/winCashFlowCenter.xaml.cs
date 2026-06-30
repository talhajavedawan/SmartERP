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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERP_BL.Databases;
using ERP_BL;
using ERP_BL.ExchangeRates;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZAS_ERP.Bankings.STL;
using ERP_BL.Procurements;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ERP_BL.Payments;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.ChartofAccounts.Windows;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.Payments.UserControls.CompanyLoanPayments;
using ERP_BL.VATBook;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.AdminBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ERP_BL.Procurements.LoansAdvances;
using ZAS_ERP.Reportss;
using ERP_BL.Reports;
using DevExpress.Xpf.Grid;

using System.IO;


namespace ZAS_ERP.CashFlow.Windows
{
    /// <summary>
    /// Interaction logic for CashFlowCenter.xaml
    /// </summary>
    public partial class winCashFlowCenter : DXWindow
    {
        PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
        SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        BillRepo billRepo = new BillRepo();
        AdminBillsRepo adminbillRepo = new AdminBillsRepo();
        STLRepo stlRepo = new STLRepo();
        AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
        CompanyLoansRepo loansRepo = new CompanyLoansRepo();
        CashFlowReportRepo cashFlowRepo = new CashFlowReportRepo();
        ChartofAccountsRepo chartofAccountRepo = new ChartofAccountsRepo();
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        JournalEntryRepo journalEntryRepo = new JournalEntryRepo();
        List<SaleOrder> saleOrders = new List<SaleOrder>();
        ERP_BL.CashFlow.CashFlow cashflow = new ERP_BL.CashFlow.CashFlow();
        List<ERP_BL.Databases.SaleOrder> saleOrdersGBP = new List<ERP_BL.Databases.SaleOrder>();
        List<ERP_BL.Databases.SaleOrder> saleOrdersUSD = new List<ERP_BL.Databases.SaleOrder>();
        List<ERP_BL.Databases.SaleOrder> saleOrdersEUR = new List<ERP_BL.Databases.SaleOrder>();
        List<ERP_BL.Databases.SaleOrder> saleOrdersAED = new List<ERP_BL.Databases.SaleOrder>();
        List<ERP_BL.Databases.SaleOrder> saleOrdersPKR = new List<ERP_BL.Databases.SaleOrder>();
        List<ERP_BL.Databases.SaleOrder> saleOrdersOMR = new List<ERP_BL.Databases.SaleOrder>();
        List<ERP_BL.Databases.SaleOrder> saleOrdersOther = new List<ERP_BL.Databases.SaleOrder>();

        List<Product> inventory= new List<Product>();
        List<Product> inventoryGBP = new List<Product>();
        List<Product> inventoryUSD = new List<Product>();
        List<Product> inventoryEUR = new List<Product>();
        List<Product> inventoryAED = new List<Product>();
        List<Product> inventoryPKR = new List<Product>();
        List<Product> inventoryOMR = new List<Product>();
        List<Product> inventoryOther = new List<Product>();

        List<ERP_BL.Databases.SaleInvoice> saleInvoices = new List<ERP_BL.Databases.SaleInvoice>();
        List<ERP_BL.Databases.SaleInvoice> saleInvoicesGBP = new List<ERP_BL.Databases.SaleInvoice>();
        List<ERP_BL.Databases.SaleInvoice> saleInvoicesUSD = new List<ERP_BL.Databases.SaleInvoice>();
        List<ERP_BL.Databases.SaleInvoice> saleInvoicesEUR = new List<ERP_BL.Databases.SaleInvoice>();
        List<ERP_BL.Databases.SaleInvoice> saleInvoicesAED = new List<ERP_BL.Databases.SaleInvoice>();
        List<ERP_BL.Databases.SaleInvoice> saleInvoicesPKR = new List<ERP_BL.Databases.SaleInvoice>();
        List<ERP_BL.Databases.SaleInvoice> saleInvoicesOMR = new List<ERP_BL.Databases.SaleInvoice>();
        List<ERP_BL.Databases.SaleInvoice> saleInvoicesOther = new List<ERP_BL.Databases.SaleInvoice>();

        List<STL> stls = new List<STL>();
        List<STL> stlsGBP = new List<STL>();
        List<STL> stlsUSD = new List<STL>();
        List<STL> stlsEUR = new List<STL>();
        List<STL> stlsAED = new List<STL>();
        List<STL> stlsPKR = new List<STL>();
        List<STL> stlsOMR = new List<STL>();
        List<STL> stlsOther = new List<STL>();


        List<Payment> payments = new List<Payment>();
        List<Payment> paymentsGBP = new List<Payment>();
        List<Payment> paymentsUSD = new List<Payment>();
        List<Payment> paymentsEUR = new List<Payment>();
        List<Payment> paymentsAED = new List<Payment>();
        List<Payment> paymentsOMR = new List<Payment>();
        List<Payment> paymentsPKR = new List<Payment>();
        List<Payment> paymentsOther = new List<Payment>();


        List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
        List<PurchaseOrder> purchaseOrdersGBP = new List<PurchaseOrder>();
        List<PurchaseOrder> purchaseOrdersUSD = new List<PurchaseOrder>();
        List<PurchaseOrder> purchaseOrdersEUR = new List<PurchaseOrder>();
        List<PurchaseOrder> purchaseOrdersAED = new List<PurchaseOrder>();
        List<PurchaseOrder> purchaseOrdersOMR = new List<PurchaseOrder>();
        List<PurchaseOrder> purchaseOrdersPKR = new List<PurchaseOrder>();
        List<PurchaseOrder> purchaseOrdersOther = new List<PurchaseOrder>();
        List<Bill> bills = new List<Bill>();
        List<Bill> billsGBP = new List<Bill>();
        List<Bill> billsUSD = new List<Bill>();
        List<Bill> billsEUR = new List<Bill>();
        List<Bill> billsAED = new List<Bill>();
        List<Bill> billsPKR = new List<Bill>();
        List<Bill> billsOMR = new List<Bill>();
        List<Bill> billsOther = new List<Bill>();
        List<AdminBill> adminBills = new List<AdminBill>();
        List<AdminBill> adminBillsGBP = new List<AdminBill>();
        List<AdminBill> adminBillsUSD = new List<AdminBill>();
        List<AdminBill> adminBillsEUR = new List<AdminBill>();
        List<AdminBill> adminBillsAED = new List<AdminBill>();
        List<AdminBill> adminBillsPKR = new List<AdminBill>();
        List<AdminBill> adminBillsOMR = new List<AdminBill>();
        List<AdminBill> adminBillsOther = new List<AdminBill>();

        List<LoansAdvance> loans = new List<LoansAdvance>();
        List<LoansAdvance> loansGBP = new List<LoansAdvance>();
        List<LoansAdvance> loansUSD = new List<LoansAdvance>();
        List<LoansAdvance> loansEUR = new List<LoansAdvance>();
        List<LoansAdvance> loansAED = new List<LoansAdvance>();
        List<LoansAdvance> loansPKR = new List<LoansAdvance>();
        List<LoansAdvance> loansOMR = new List<LoansAdvance>();
        List<LoansAdvance> loansOther = new List<LoansAdvance>();

        List<LoansAdvance> advances = new List<LoansAdvance>();
        List<LoansAdvance> advancesGBP = new List<LoansAdvance>();
        List<LoansAdvance> advancesUSD = new List<LoansAdvance>();
        List<LoansAdvance> advancesEUR = new List<LoansAdvance>();
        List<LoansAdvance> advancesAED = new List<LoansAdvance>();
        List<LoansAdvance> advancesPKR = new List<LoansAdvance>();
        List<LoansAdvance> advancesOMR = new List<LoansAdvance>();
        List<LoansAdvance> advancesOther = new List<LoansAdvance>();

        List<ChartofAccount> chartofAccounts = new List<ChartofAccount>();
        List<ChartofAccount> chartofAccountsGBP = new List<ChartofAccount>();
        List<ChartofAccount> chartofAccountsUSD = new List<ChartofAccount>();
        List<ChartofAccount> chartofAccountsEUR = new List<ChartofAccount>();
        List<ChartofAccount> chartofAccountsAED = new List<ChartofAccount>();
        List<ChartofAccount> chartofAccountsPKR = new List<ChartofAccount>();
        List<ChartofAccount> chartofAccountsOMR = new List<ChartofAccount>();
        List<ChartofAccount> chartofAccountsOther = new List<ChartofAccount>();

        static List<ExchangeRateGroup> exchangeRateGroups = new List<ExchangeRateGroup>();
        List<Company> allCompanies = new List<Company>();
        List<Company> selectedCompanies = new List<Company>();
        List<Department> allDepartments = new List<Department>();
        List<Department> selectedDepartments = new List<Department>();
        VATBookRepo vatBookRepo = new VATBookRepo();
        List<ERP_BL.VATBook.VATBook> vatSIBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatPIBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatBillBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatAdminBillBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatIBTBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatIBCTBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatPaymentBooks = new List<ERP_BL.VATBook.VATBook>();
        List<ERP_BL.VATBook.VATBook> vatSaleReceiptBooks = new List<ERP_BL.VATBook.VATBook>();
        ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;
        List<int> deptIds = new List<int>();
        List<int> companyIds = new List<int>();
        UsersRepo usersRepo = new UsersRepo();
        PaymentRepo paymentRepo = new PaymentRepo();
        static List<ExchangeRateGroup> exchangeRateGroupsMER = new List<ExchangeRateGroup>();
        ERP_BL.ExchangeRates.ExchangeRate exchangeRateMER = null;
        double
            totalCollectionAmountGBP = 0,
            totalInvoiceAmountGBP,
            totalCollectionAmountGBPCMER,
            totalInvoiceAmountGBPCMER,
            totalInvoiceAmountEUR,
            totalCollectionAmountEURCMER,
            totalInvoiceAmountEURCMER,
            totalCollectionAmountAED,
            totalInvoiceAmountAED,
            totalCollectionAmountAEDCMER,
            totalInvoiceAmountAEDCMER,
            totalCollectionAmountOMR,
            totalInvoiceAmountOMR,
            totalCollectionAmountOMRCMER,
            totalInvoiceAmountOMRCMER,
            totalCollectionAmountPKR,
            totalInvoiceAmountPKR,
            totalCollectionAmountPKRCMER,
            totalInvoiceAmountPKRCMER,
            totalCollectionAmountOther,
            totalInvoiceAmountOther,
            totalCollectionAmountOtherCMER,
            totalInvoiceAmountUSD,
            totalCollectionAmountUSD,
            totalInvoiceAmountUSDCMER,
            totalCollectionAmountUSDCMER,
            totalCollectionAmountEUR,
            totalInvoiceAmountOtherCMER,
            advancePaidAmountGBP,
            advancePaidAmountSystemGBP,
            advancePaidAmountGBPCMER,
            advancePaidAmountSystemGBPCMER,
            advancePaidAmountUSD,
            advancePaidAmountSystemUSD,
            advancePaidAmountUSDCMER,
            advancePaidAmountSystemUSDCMER,
            advancePaidAmountEUR,
            advancePaidAmountSystemEUR,
            advancePaidAmountEURCMER,
            advancePaidAmountSystemEURCMER,
            advancePaidAmountAED,
            advancePaidAmountSystemAED,
            advancePaidAmountAEDCMER,
            advancePaidAmountSystemAEDCMER,
            advancePaidAmountOMR,
            advancePaidAmountSystemOMR,
            advancePaidAmountOMRCMER,
            advancePaidAmountSystemOMRCMER,
            advancePaidAmountPKR,
            advancePaidAmountSystemPKR,
            advancePaidAmountPKRCMER,
            advancePaidAmountSystemPKRCMER,
            advancePaidAmountOther,
            advancePaidAmountSystemOther,
            advancePaidAmountOtherCMER,
            totalRemainigCollectionOCGBP,
            totalRemainigCollectionOCGBPCMER,
            totalRemainigCollectionOCUSD,
            totalRemainigCollectionOCUSDCMER,
            totalRemainigCollectionOCEUR,
            totalRemainigCollectionOCEURCMER,
            totalRemainigCollectionOCAED = 0,
            totalRemainigCollectionOCOMR,
            totalRemainigCollectionOCOMRCMER,
            totalRemainigCollectionOCPKR,
            totalRemainigCollectionOCPKRCMER,
            totalRemainigCollectionOCOther,
            totalRemainigCollectionOCOtherCMER,
            advancePaidAmountSystemOtherCMER,
             totalBankBalanceSGBP,
            totalBankBalanceSGBPCMER,
            totalBankBalanceSEUR,
            totalBankBalanceSEURCMER,
            totalBankBalanceSUSD,
            totalBankBalanceSUSDCMER,
            totalBankBalanceSAED,
            totalBankBalanceSAEDCMER,
            totalBankBalanceSOMR,
            totalBankBalanceSOMRCMER,
            totalBankBalanceSPKR,
            totalBankBalanceSPKRCMER,
            totalBankBalanceSOther,
            totalBankBalanceSOtherCMER,
            totalAdminBillGBP,
            totalAdminBillGBPCMER,
            totalAdminBillEUR,
            totalAdminBillEURCMER,
            totalAdminBillUSD,
            totalAdminBillUSDCMER,
            totalAdminBillAED,
            totalAdminBillAEDCMER,
            totalAdminBillOMR,
            totalAdminBillOMRCMER,
            totalAdminBillPKR,
            totalAdminBillPKRCMER,
            totalAdminBillOther,
            totalAdminBillOtherCMER,
            totalLoanGBP,
            totalLoanGBPCMER,
            totalLoanEUR,
            totalLoanEURCMER,
            totalLoanUSD,
            totalLoanUSDCMER,
            totalLoanAED,
            totalLoanAEDCMER,
            totalLoanOMR,
            totalLoanOMRCMER,
            totalLoanPKR,
            totalLoanPKRCMER,
            totalLoanOther,
            totalLoanOtherCMER,
            totalAdvanceGBP,
            totalAdvanceGBPCMER,
            totalAdvanceEUR,
            totalAdvanceEURCMER,
            totalAdvanceUSD,
            totalAdvanceUSDCMER,
            totalAdvanceAED,
            totalAdvanceAEDCMER,
            totalAdvanceOMR,
            totalAdvanceOMRCMER,
            totalAdvancePKR,
            totalAdvancePKRCMER,
            totalAdvanceOther,
            totalAdvanceOtherCMER,
            totalBankBalanceMGBP,
            totalBankBalanceMGBPCMER,
            totalBankBalanceMEUR,
            totalBankBalanceMEURCMER,
            totalBankBalanceMUSD,
            totalBankBalanceMUSDCMER,
            totalBankBalanceMAED,
            totalBankBalanceMAEDCMER,
            totalBankBalanceMOMR,
            totalBankBalanceMOMRCMER,
            totalBankBalanceMPKR,
            totalBankBalanceMPKRCMER,
            totalBankBalanceMOther,
            totalBankBalanceMOtherCMER,
            totalSTLAmountGBP,
            totalSTLAmountEUR,
            totalSTLAmountUSD,
            totalSTLAmountOMR,
            totalSTLAmountAED,
            totalSTLAmountPKR,
            totalSTLAmountOther,
            totalSTLAmountGBPCMER,
            totalSTLAmountEURCMER,
            totalSTLAmountUSDCMER,
            totalSTLAmountOMRCMER,
            totalSTLAmountAEDCMER,
            totalSTLAmountPKRCMER,
            totalSTLAmountOtherCMER,
            totalSaleOrdersGBP,
            totalSaleOrdersEUR,
            totalSaleOrdersUSD,
            totalSaleOrdersOMR,
            totalSaleOrdersAED,
            totalSaleOrdersPKR,
            totalSaleOrdersOther,
            totalSaleOrdersGBPCMER,
            totalSaleOrdersEURCMER,
            totalSaleOrdersUSDCMER,
            totalSaleOrdersOMRCMER,
            totalSaleOrdersAEDCMER,
            totalSaleOrdersPKRCMER,
            totalSaleOrdersOtherCMER,

             totalSaleOrdersAPGBP,
            totalSaleOrdersAPEUR,
            totalSaleOrdersAPUSD,
            totalSaleOrdersAPOMR,
            totalSaleOrdersAPAED,
            totalSaleOrdersAPPKR,
            totalSaleOrdersAPOther,
            totalSaleOrdersAPGBPCMER,
            totalSaleOrdersAPEURCMER,
            totalSaleOrdersAPUSDCMER,
            totalSaleOrdersAPOMRCMER,
            totalSaleOrdersAPAEDCMER,
            totalSaleOrdersAPPKRCMER,
            totalSaleOrdersAPOtherCMER,

            totalPaymentGBP,
            totalPaymentEUR,
            totalPaymentUSD,
            totalPaymentOMR,
            totalPaymentAED,
            totalPaymentPKR,
            totalPaymentOther,
            totalPaymentGBPCMER,
            totalPaymentEURCMER,
            totalPaymentUSDCMER,
            totalPaymentOMRCMER,
            totalPaymentAEDCMER,
            totalPaymentPKRCMER,
            totalPaymentOtherCMER,
            totalUnPaidPOAmountGBP,
            totalUnPaidPOAmountEUR,
            totalUnPaidPOAmountUSD,
            totalUnPaidPOAmountOMR,
            totalUnPaidPOAmountAED,
            totalUnPaidPOAmountPKR,
            totalUnPaidPOAmountOther,
            totalUnPaidPOAmountGBPCMER,
            totalUnPaidPOAmountEURCMER,
            totalUnPaidPOAmountUSDCMER,
            totalUnPaidPOAmountOMRCMER,
            totalUnPaidPOAmountAEDCMER,
            totalUnPaidPOAmountPKRCMER,
            totalUnPaidPOAmountOtherCMER,
            totalUnPaidBillAmountGBP,
            totalUnPaidBillAmountEUR,
            totalUnPaidBillAmountUSD,
            totalUnPaidBillAmountOMR,
            totalUnPaidBillAmountAED,
            totalUnPaidBillAmountPKR,
            totalUnPaidBillAmountOther,
            totalUnPaidBillAmountGBPCMER,
            totalUnPaidBillAmountEURCMER,
            totalUnPaidBillAmountUSDCMER,
            totalUnPaidBillAmountOMRCMER,
            totalUnPaidBillAmountAEDCMER,
            totalUnPaidBillAmountPKRCMER,
            totalUnPaidBillAmountOtherCMER,
            totalRemainigCollectionOCAEDCMER,
            totalAssetsGBP,
            totalAssetsGBPCMER,
            totalAssetsEUR,
            totalAssetsEURCMER,
            totalAssetsUSD,
            totalAssetsUSDCMER,
            totalAssetsAED,
            totalAssetsAEDCMER,
            totalAssetsOMR,
            totalAssetsOMRCMER,
            totalAssetsPKR,
            totalAssetsPKRCMER,
            totalAssetsOther,
            totalAssetsOtherCMER,
            totalAssetsCMER,
            totalLiabilitiesGBP,
            totalLiabilitiesGBPCMER,
            totalLiabilitiesEUR,
            totalLiabilitiesEURCMER,
            totalLiabilitiesUSD,
            totalLiabilitiesUSDCMER,
            totalLiabilitiesAED,
            totalLiabilitiesAEDCMER,
            totalLiabilitiesOMR,
            totalLiabilitiesOMRCMER,
            totalLiabilitiesPKR,
            totalLiabilitiesPKRCMER,
            totalLiabilitiesOther,
            totalLiabilitiesOtherCMER,
            totalLiabilitiesCMER,
            totalInventoryGBP,
            totalInventoryGBPCMER,
            totalInventoryEUR,
            totalInventoryEURCMER,
            totalInventoryUSD,
            totalInventoryUSDCMER,
            totalInventoryAED,
            totalInventoryAEDCMER,
            totalInventoryOMR,
            totalInventoryOMRCMER,
            totalInventoryPKR,
            totalInventoryPKRCMER,
            totalInventoryOther,
            totalInventoryOtherCMER,
            totalInventoryCMER;

 

        private void btnConvertCurrency_Click(object sender, RoutedEventArgs e)
        {
           
        }

        //private void loadCurrencies()
        //{
        //    cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        //}

        private void cmbCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedCompanies.Count() > 1)
            {
                DXMessageBox.Show("Please select only 1 company to continue ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                var MER = GetCMER((int)selectedCompanies[0].CurrencyId, (int)(cmbCurrency.SelectedItem as cmbitem).id, (int)selectedCompanies[0].Id);

                txtTotalReceivableCCCMER.Text = (Convert.ToDouble(txtTotalReceivableCMER.Text) / MER).ToString();
                txtTotalAdvancePaidSystemCCCMER.Text = (Convert.ToDouble(txtTotalAdvancePaidSystemCMER.Text) / MER).ToString();


                txtTotalAdvancePaidCCMER.Text = (Convert.ToDouble(txtTotalAdvancePaidCMER.Text) / MER).ToString();

                txtTotalBankBalanceSCCCMER.Text = (Convert.ToDouble(txtTotalBankBalanceSCMER.Text) / MER).ToString();

                txtTotalBankBalanceMCCCMER.Text = (Convert.ToDouble(txtTotalBankBalanceMCMER.Text) / MER).ToString();

                txtTotalAdvancePaidSystemCCCMER.Text = (Convert.ToDouble(txtTotalAdvancePaidSystemCMER.Text) / MER).ToString();

                txtAdvancesCCCMER.Text = (Convert.ToDouble(txtAdvancesCMER.Text) / MER).ToString();

                txtInventoryCCCMER.Text = (Convert.ToDouble(txtInventoryCMER.Text) / MER).ToString();

                txtTotalAssetsCCCMER.Text = (Convert.ToDouble(txtTotalAssetsCMER.Text) / MER).ToString();

                txtTotalAPSuppliersCCCMER.Text = (Convert.ToDouble(txtTotalAPSuppliersCMER.Text) / MER).ToString();

                txtTotalSTLCCCMER.Text = (Convert.ToDouble(txtTotalSTLCMER.Text) / MER).ToString();

                txtTotalAPPaymentsCCCMER.Text = (Convert.ToDouble(txtTotalAPPaymentsCMER.Text) / MER).ToString();

                txtTotalAPbillsCCCMER.Text = (Convert.ToDouble(txtTotalAPbillsCMER.Text) / MER).ToString();

                txtTotalAPSOCCCMER.Text = (Convert.ToDouble(txtTotalAPSOCMER.Text) / MER).ToString();

                txtAdminBillsCCCMER.Text = (Convert.ToDouble(txtAdminBillsCMER.Text) / MER).ToString();

                txtLoansCCCMER.Text = (Convert.ToDouble(txtLoansCMER.Text) / MER).ToString();

                txtTotalLiabilitiesCCCMER.Text = (Convert.ToDouble(txtTotalLiabilitiesCMER.Text) / MER).ToString();

                txtTotalNetWorthCCCMER.Text = (Convert.ToDouble(txtTotalNetWorthCMER.Text) / MER).ToString();


            }
        }

        private void chkInventory_Unchecked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP -= totalInventoryGBP;
            totalAssetsGBPCMER -= totalInventoryGBPCMER;
            totalAssetsUSD -= totalInventoryUSD;
            totalAssetsUSDCMER -= totalInventoryUSDCMER;
            totalAssetsEUR -= totalInventoryEUR;
            totalAssetsEURCMER -= totalInventoryEURCMER;
            totalAssetsAED -= totalInventoryAED;
            totalAssetsAEDCMER -= totalInventoryAEDCMER;
            totalAssetsOMR -= totalInventoryOMR;
            totalAssetsOMRCMER -= totalInventoryOMRCMER;
            totalAssetsPKR -= totalInventoryPKR;
            totalAssetsPKRCMER -= totalInventoryPKRCMER;
            totalAssetsOther -= totalInventoryOther;
            totalAssetsOtherCMER -= totalInventoryOtherCMER;
            totalAssetsCMER -=
                +totalInventoryOtherCMER
               + totalInventoryPKRCMER
                + totalInventoryOMRCMER
                + totalInventoryAEDCMER
                + totalInventoryEURCMER
                + totalInventoryUSDCMER
                + totalInventoryGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        private void chkInventory_Checked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP += totalInventoryGBP;
            totalAssetsGBPCMER += totalInventoryGBPCMER;
            totalAssetsUSD += totalInventoryUSD;
            totalAssetsUSDCMER += totalInventoryUSDCMER;
            totalAssetsEUR += totalInventoryEUR;
            totalAssetsEURCMER += totalInventoryEURCMER;
            totalAssetsAED += totalInventoryAED;
            totalAssetsAEDCMER += totalInventoryAEDCMER;
            totalAssetsOMR += totalInventoryOMR;
            totalAssetsOMRCMER += totalInventoryOMRCMER;
            totalAssetsPKR += totalInventoryPKR;
            totalAssetsPKRCMER += totalInventoryPKRCMER;
            totalAssetsOther += totalInventoryOther;
            totalAssetsOtherCMER += totalInventoryOtherCMER;
            totalAssetsCMER +=
                +totalInventoryOtherCMER
                + totalInventoryPKRCMER
                + totalInventoryOMRCMER
                + totalInventoryAEDCMER
                + totalInventoryEURCMER
                + totalInventoryUSDCMER
                + totalInventoryGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        private void btnLoadRefreshInventory_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplyInventoryFilters();
            CalculateInventorySummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });


          
            
        }

        private void MbtnExportToReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ERP_BL.Reports.GridReportRepo gridReportRepo = new ERP_BL.Reports.GridReportRepo();
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export CashFlow Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("CashFlow");
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        cashflow.CashFlowCompanies = selectedCompanies;
                        cashflow.CashflowDepartments = selectedDepartments;
                        cashflow.groupId = report.gridReportGroup.Id;
                        cashflow.userId = report.userId;
                        cashflow.reportName = report.reportName;
                        cashflow.gridReportType = report.gridReportType;
                        cashflow.gridReportType = report.gridReportType;
                        cashflow.titleId = report.titleId;
                        cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
                        cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
                        cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
                        cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
                        cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
                        cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
                        cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
                        cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
                        cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
                        cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);

                        cashFlowRepo.AddCashFlow(cashflow);
                        DXMessageBox.Show("( " + report.reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export CashFlow Reports!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnExportToStandardReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Standard Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (cashflow.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Export to Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("CashFlow");
                                setReportName.ShowDialog();
                                var report = setReportName.report;
                                if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                                {
                                    cashflow = new ERP_BL.CashFlow.CashFlow();
                                    cashflow.CashFlowCompanies = selectedCompanies;
                                    cashflow.CashflowDepartments = selectedDepartments;
                                    cashflow.groupId = report.gridReportGroup.Id;
                                    cashflow.userId = report.userId;
                                    cashflow.reportName = report.reportName;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.titleId = report.titleId;
                                    cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
                                    cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
                                    cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
                                    cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
                                    cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
                                    cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
                                    cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
                                    cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
                                    cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
                                    cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
                                    cashFlowRepo.AddCashFlow(cashflow);
                                    DXMessageBox.Show("( " + report.reportName + " ) is exporteded to standard Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void MbtnExportToMemorizedReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Memorized Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (cashflow.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Export to Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("CashFlow");
                                setReportName.ShowDialog();
                                var report = setReportName.report;
                                if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                                {
                                    cashflow = new ERP_BL.CashFlow.CashFlow();
                                    cashflow.CashFlowCompanies = selectedCompanies;
                                    cashflow.CashflowDepartments = selectedDepartments;
                                    cashflow.groupId = report.gridReportGroup.Id;
                                    cashflow.userId = report.userId;
                                    cashflow.reportName = report.reportName;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.titleId = report.titleId;
                                    cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
                                    cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
                                    cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
                                    cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
                                    cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
                                    cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
                                    cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
                                    cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
                                    cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
                                    cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
                                    cashFlowRepo.AddCashFlow(cashflow);
                                    DXMessageBox.Show("( " + report.reportName + " ) is exporteded to memorized Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                    return;

                            }
                            else
                            {
                                DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        break;
                    }
                  
            }
        }

        private void MbtnRefreshReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            //if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            //{
            //    GridReportRepo repo = new GridReportRepo();
            //    report = repo.GetReportById(report.Id);

            //    grdsaleOrderReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
            //}
            //else
            //{
            //}
            //loadingGif.Visibility = Visibility.Visible;
            //BackgroundWorker worker = new BackgroundWorker();
            //worker.DoWork += OnDoWork;
            //worker.RunWorkerCompleted += OnRunWorkerCompleted;
            //worker.RunWorkerAsync();
        }

        private void MbtnDeleteReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //var inputfromUser = DXMessageBox.Show("Do you want to delete " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            //switch (report.gridReportType)
            //{
            //    case GridReportType.StandardReport:
            //        {
            //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Standard Report") != null)
            //            {
            //                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            //                {
            //                    var repo = new GridReportRepo();
            //                    var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
            //                    ReportLogic.DeleteReport(grdsaleOrderReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
            //                    DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
            //                    this.Close();
            //                }
            //                else
            //                    return;
            //            }
            //            else
            //            {
            //                DXMessageBox.Show("You don't have permission to Delete Standard report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }
            //            break;
            //        }
            //    case GridReportType.MemorizedReport:
            //        {
            //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Memorized Report") != null)
            //            {
            //                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            //                {
            //                    var repo = new GridReportRepo();
            //                    /*GridReport report = repo.GetReportByName(this.Title)*/
            //                    ;
            //                    var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

            //                    ReportLogic.DeleteReport(grdsaleOrderReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
            //                    DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
            //                    this.Close();
            //                }
            //                else
            //                    return;

            //            }
            //            else
            //            {
            //                DXMessageBox.Show("You don't have permission to Delete Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }

            //            break;
            //        }

            //}
        }

        private void MbtnUpdateReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to Update " + this.Title +"?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (cashflow.gridReportType)
            {
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                if (cashflow.Id != 0 && cashflow.gridReportGroup != null  && cashflow.reportName != null && cashflow.userId != null)
                                {
                                    cashflow.CashFlowCompanies = selectedCompanies;
                                    cashflow.CashflowDepartments = selectedDepartments;
                                    cashflow.groupId = cashflow.gridReportGroup.Id;
                                    cashflow.userId = cashflow.userId;
                                    cashflow.reportName = cashflow.reportName;
                                    cashflow.gridReportType = cashflow.gridReportType;
                                    cashflow.gridReportType = cashflow.gridReportType;
                                    cashflow.titleId = cashflow.titleId;
                                    cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
                                    cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
                                    cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
                                    cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
                                    cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
                                    cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
                                    cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
                                    cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
                                    cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
                                    cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
                                    cashFlowRepo.Update(cashflow);
                                    DXMessageBox.Show("( " + cashflow.reportName + " ) is updated Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("CashFlow");
                                setReportName.ShowDialog();
                                var report = setReportName.report;
                                if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                                {
                                    cashflow.CashFlowCompanies = selectedCompanies;
                                    cashflow.CashflowDepartments = selectedDepartments;
                                    cashflow.groupId = report.gridReportGroup.Id;
                                    cashflow.userId = report.userId;
                                    cashflow.reportName = report.reportName;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.titleId = report.titleId;
                                    cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
                                    cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
                                    cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
                                    cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
                                    cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
                                    cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
                                    cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
                                    cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
                                    cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
                                    cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
                                    cashFlowRepo.Update(cashflow);
                                    DXMessageBox.Show("( " + report.reportName + " ) is updated Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }
            }
            //var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.Title + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            //switch (report.gridReportType)
            //{
            //    case GridReportType.StandardReport:
            //        {
            //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Standard Report") != null)
            //            {
            //                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            //                {
            //                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(cashflow);
            //                    setReportName.ShowDialog();
            //                    if (cashflow.Id != 0)
            //                    {
            //                        var exportToMemorizedReport = setReportName.cashFlow;
            //                        cashflow.CashFlowCompanies = selectedCompanies;
            //                        cashflow.CashflowDepartments = selectedDepartments;
            //                        cashflow.groupId = exportToMemorizedReport.gridReportGroup.Id;
            //                        cashflow.userId = exportToMemorizedReport.userId;
            //                        cashflow.reportName = exportToMemorizedReport.reportName;
            //                        cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
            //                        cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
            //                        cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
            //                        cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
            //                        cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
            //                        cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
            //                        cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
            //                        cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
            //                        cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
            //                        cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
            //                    }
            //                    return;
            //                }
            //            }
            //            else
            //            {
            //                DXMessageBox.Show("You don't have permission to Update report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }
            //        }
            //        break;
            //    case GridReportType.MemorizedReport:
            //        {
            //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Memorized Report") != null)
            //            {
            //                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            //                {
            //                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(cashflow);
            //                    setReportName.ShowDialog();
            //                    if (cashflow.Id != 0)
            //                    {
            //                        var exportToMemorizedReport = setReportName.cashFlow;
            //                        cashflow.CashFlowCompanies = selectedCompanies;
            //                        cashflow.CashflowDepartments = selectedDepartments;
            //                        cashflow.groupId = exportToMemorizedReport.gridReportGroup.Id;
            //                        cashflow.userId = exportToMemorizedReport.userId;
            //                        cashflow.reportName = exportToMemorizedReport.reportName;
            //                        cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
            //                        cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
            //                        cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
            //                        cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
            //                        cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
            //                        cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
            //                        cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
            //                        cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
            //                        cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
            //                        cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
            //                    }
            //                    return;
            //                }
            //            }
            //            else
            //            {
            //                DXMessageBox.Show("You don't have permission to Update report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }
            //        }
            //        break;
            //}
        }

        private void MbtnRenameReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to Update " + this.Title + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (cashflow.gridReportType)
            {
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("CashFlow");
                                setReportName.ShowDialog();
                                var report = setReportName.report;
                                if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                                {
                                    cashflow.CashFlowCompanies = selectedCompanies;
                                    cashflow.CashflowDepartments = selectedDepartments;
                                    cashflow.groupId = report.gridReportGroup.Id;
                                    cashflow.userId = report.userId;
                                    cashflow.reportName = report.reportName;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.titleId = report.titleId;
                                    cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
                                    cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
                                    cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
                                    cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
                                    cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
                                    cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
                                    cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
                                    cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
                                    cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
                                    cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
                                    cashFlowRepo.Update(cashflow);
                                    DXMessageBox.Show("( " + report.reportName + " ) is updated Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("CashFlow");
                                setReportName.ShowDialog();
                                var report = setReportName.report;
                                if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                                {
                                    cashflow.CashFlowCompanies = selectedCompanies;
                                    cashflow.CashflowDepartments = selectedDepartments;
                                    cashflow.groupId = report.gridReportGroup.Id;
                                    cashflow.userId = report.userId;
                                    cashflow.reportName = report.reportName;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.gridReportType = report.gridReportType;
                                    cashflow.titleId = report.titleId;
                                    cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
                                    cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
                                    cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
                                    cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
                                    cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
                                    cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
                                    cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
                                    cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
                                    cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
                                    cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
                                    cashFlowRepo.Update(cashflow);
                                    DXMessageBox.Show("( " + report.reportName + " ) is updated Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Rename Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }
            }
            //var inputfromUser = DXMessageBox.Show("Do you want to rename " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            //switch (report.gridReportType)
            //{
            //    case GridReportType.StandardReport:
            //        {
            //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Standard Report") != null)
            //            {
            //                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            //                {
            //                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(cashflow);
            //                    setReportName.ShowDialog();
            //                    if (cashflow.Id != 0)
            //                    {
            //                        var exportToMemorizedReport = setReportName.cashFlow;
            //                        cashflow.CashFlowCompanies = selectedCompanies;
            //                        cashflow.CashflowDepartments = selectedDepartments;
            //                        cashflow.groupId = exportToMemorizedReport.gridReportGroup.Id;
            //                        cashflow.userId = exportToMemorizedReport.userId;
            //                        cashflow.reportName = exportToMemorizedReport.reportName;
            //                        cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
            //                        cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
            //                        cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
            //                        cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
            //                        cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
            //                        cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
            //                        cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
            //                        cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
            //                        cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
            //                        cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
            //                    }

            //                    if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
            //                    {

            //                        if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
            //                        {
            //                            ReportLogic.RenameGridReport(grdsaleOrderReport, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
            //                            DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            //                            this.Close();
            //                        }
            //                        else
            //                            DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //                        return;

            //                    }

            //                }
            //                else
            //                    return;
            //            }
            //            else
            //            {
            //                DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }
            //            break;
            //        }
            //    case GridReportType.MemorizedReport:
            //        {
            //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Memorized Report") != null)
            //            {
            //                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            //                {
            //                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(cashflow);
            //                    setReportName.ShowDialog();
            //                    if (cashflow.Id != 0)
            //                    {
            //                        var exportToMemorizedReport = setReportName.cashFlow;
            //                        cashflow.CashFlowCompanies = selectedCompanies;
            //                        cashflow.CashflowDepartments = selectedDepartments;
            //                        cashflow.groupId = exportToMemorizedReport.gridReportGroup.Id;
            //                        cashflow.userId = exportToMemorizedReport.userId;
            //                        cashflow.reportName = exportToMemorizedReport.reportName;
            //                        cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
            //                        cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
            //                        cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
            //                        cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
            //                        cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
            //                        cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
            //                        cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
            //                        cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
            //                        cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
            //                        cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
            //                    }

            //                    DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            //                    this.Close();

            //                    DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }

            //            }
            //            else
            //            {
            //                DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }
            //            break;
            //        }

            //}
        }

        private void MbtnSaveAsNew_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //switch (report.gridReportType)
            //{
            //    case GridReportType.StandardReport:
            //        {

            //            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Save as new Standard Report") != null)
            //            {
            //                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            //                {
            //                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(cashflow);
            //                    setReportName.ShowDialog();
            //                    if (cashflow.Id != 0)
            //                    {
            //                        var exportToMemorizedReport = setReportName.cashFlow;
            //                        cashflow.CashFlowCompanies = selectedCompanies;
            //                        cashflow.CashflowDepartments = selectedDepartments;
            //                        cashflow.groupId = exportToMemorizedReport.gridReportGroup.Id;
            //                        cashflow.userId = exportToMemorizedReport.userId;
            //                        cashflow.reportName = exportToMemorizedReport.reportName;
            //                        cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
            //                        cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
            //                        cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
            //                        cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
            //                        cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
            //                        cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
            //                        cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
            //                        cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
            //                        cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
            //                        cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
            //                    }
            //                }
            //                else
            //                    return;
            //            }
            //            else
            //            {
            //                DXMessageBox.Show("You don't have permission to save as new Standard Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }
            //        }
            //        break;
            //    case GridReportType.MemorizedReport:
            //        {

            //            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Save as new Memorized Report") != null)
            //            {
            //                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            //                {
            //                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(cashflow);
            //                    setReportName.ShowDialog();
            //                    if (cashflow.Id != 0)
            //                    {
            //                        var exportToMemorizedReport = setReportName.cashFlow;
            //                        cashflow.CashFlowCompanies = selectedCompanies;
            //                        cashflow.CashflowDepartments = selectedDepartments;
            //                        cashflow.groupId = exportToMemorizedReport.gridReportGroup.Id;
            //                        cashflow.userId = exportToMemorizedReport.userId;
            //                        cashflow.reportName = exportToMemorizedReport.reportName;
            //                        cashflow.saleOrderSettingKey = GetLayoutString(grdsaleOrderCashFlow);
            //                        cashflow.saleinvoiceSettingKey = GetLayoutString(grdsaleInvoiceCashFlow);
            //                        cashflow.vendorBillSettingKey = GetLayoutString(grdCashFlowbill);
            //                        cashflow.adminBillSettingKey = GetLayoutString(grdAdminBillCashFlow);
            //                        cashflow.purchaseOrderSettingKey = GetLayoutString(grdCashFlowPurchaseOrder);
            //                        cashflow.stlSettingKey = GetLayoutString(grdSTLCashFlow);
            //                        cashflow.bankSettingKey = GetLayoutString(grdCashFlowBankAccounts);
            //                        cashflow.paymentSettingKey = GetLayoutString(grdCashFlowPayment);
            //                        cashflow.loanSettingKey = GetLayoutString(grdCashFlowLoans);
            //                        cashflow.advanceSettingKey = GetLayoutString(grdCashFlowAdvances);
            //                    }
            //                }
            //                else
            //                    return;
            //            }
            //            else
            //            {
            //                DXMessageBox.Show("You don't have permission to save as new Memorized Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
            //                return;
            //            }
            //        }
            //        break;
            //}
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loadCompanies();
            loadCashFlowData();
            LoadMERGroups();
        }
        public string GetLayoutString(GridControl gridControl)
        {
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            gridControl.SaveLayoutToStream(memoryStream);
            string Layoutstream = "";
            memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(memoryStream);
            return Layoutstream = reader.ReadToEnd();
        }
     
     

        private void chkCMER_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkCMER_Checked(object sender, RoutedEventArgs e)
        {

        }

        public winCashFlowCenter()
        {
            InitializeComponent();
            companyIds = SYSTEM_STATIC.currentUser.employee.Companies.Select(x => x.Id).ToList();
            deptIds = SYSTEM_STATIC.currentUser.employee.departments.Select(x => x.Id).ToList();
        }
        public winCashFlowCenter(ERP_BL.CashFlow.CashFlow _report)
        {
            InitializeComponent();
            companyIds = SYSTEM_STATIC.currentUser.employee.Companies.Select(x => x.Id).ToList();
            deptIds = SYSTEM_STATIC.currentUser.employee.departments.Select(x => x.Id).ToList();
            cashflow = _report;
            selectedCompanies = cashflow.CashFlowCompanies.ToList();
            selectedDepartments = cashflow.CashflowDepartments.ToList();
            

        }
        private void chkLAdvanceAssets_Checked(object sender, RoutedEventArgs e)
        {

            totalAssetsGBP += totalAdvanceGBP;
            totalAssetsGBPCMER += totalAdvanceGBPCMER;
            totalAssetsUSD += totalAdvanceUSD;
            totalAssetsUSDCMER += totalAdvanceUSDCMER;
            totalAssetsEUR += totalAdvanceEUR;
            totalAssetsEURCMER += totalAdvanceEURCMER;
            totalAssetsAED += totalAdvanceAED;
            totalAssetsAEDCMER += totalAdvanceAEDCMER;
            totalAssetsOMR += totalAdvanceOMR;
            totalAssetsOMRCMER += totalAdvanceOMRCMER;
            totalAssetsPKR += totalAdvancePKR;
            totalAssetsPKRCMER += totalAdvancePKRCMER;
            totalAssetsOther += totalAdvanceOther;
            totalAssetsOtherCMER += totalAdvanceOtherCMER;
            totalAssetsCMER +=
                +totalAdvanceOtherCMER
                + totalAdvancePKRCMER
                + totalAdvanceOMRCMER
                + totalAdvanceAEDCMER
                + totalAdvanceEURCMER
                + totalAdvanceUSDCMER
                + totalAdvanceGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        private void chkLAdvanceAssets_Unchecked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP -= totalAdvanceGBP;
            totalAssetsGBPCMER -= totalAdvanceGBPCMER;
            totalAssetsUSD -= totalAdvanceUSD;
            totalAssetsUSDCMER -= totalAdvanceUSDCMER;
            totalAssetsEUR -= totalAdvanceEUR;
            totalAssetsEURCMER -= totalAdvanceEURCMER;
            totalAssetsAED -= totalAdvanceAED;
            totalAssetsAEDCMER -= totalAdvanceAEDCMER;
            totalAssetsOMR -= totalAdvanceOMR;
            totalAssetsOMRCMER -= totalAdvanceOMRCMER;
            totalAssetsPKR -= totalAdvancePKR;
            totalAssetsPKRCMER -= totalAdvancePKRCMER;
            totalAssetsOther -= totalAdvanceOther;
            totalAssetsOtherCMER -= totalAdvanceOtherCMER;
            totalAssetsCMER -=
                +totalAdvanceOtherCMER
               + totalAdvancePKRCMER
                + totalAdvanceOMRCMER
                + totalAdvanceAEDCMER
                + totalAdvanceEURCMER
                + totalAdvanceUSDCMER
                + totalAdvanceGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }
        private void chkLoanLiabilities_Checked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP += totalLoanGBP;
            totalLiabilitiesGBPCMER += totalLoanGBPCMER;
            totalLiabilitiesUSD += totalLoanUSD;
            totalLiabilitiesUSDCMER += totalLoanUSDCMER;
            totalLiabilitiesEUR += totalLoanEUR;
            totalLiabilitiesEURCMER += totalLoanEURCMER;
            totalLiabilitiesAED += totalLoanAED;
            totalLiabilitiesAEDCMER += totalLoanAEDCMER;
            totalLiabilitiesOMR += totalLoanOMR;
            totalLiabilitiesOMRCMER += totalLoanOMRCMER;
            totalLiabilitiesPKR += totalLoanPKR;
            totalLiabilitiesPKRCMER += totalLoanPKRCMER;
            totalLiabilitiesOther += totalLoanOther;
            totalLiabilitiesOtherCMER += totalLoanOtherCMER;
            totalLiabilitiesCMER +=
                +totalLoanOtherCMER
                + totalLoanPKRCMER
                + totalLoanOMRCMER
                + totalLoanAEDCMER
                + totalLoanEURCMER
                + totalLoanUSDCMER
                + totalLoanGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }
        private void chkLoanLiabilities_Unchecked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP -= totalLoanGBP;
            totalLiabilitiesGBPCMER -= totalLoanGBPCMER;
            totalLiabilitiesUSD -= totalLoanUSD;
            totalLiabilitiesUSDCMER -= totalLoanUSDCMER;
            totalLiabilitiesEUR -= totalLoanEUR;
            totalLiabilitiesEURCMER -= totalLoanEURCMER;
            totalLiabilitiesAED -= totalLoanAED;
            totalLiabilitiesAEDCMER -= totalLoanAEDCMER;
            totalLiabilitiesOMR -= totalLoanOMR;
            totalLiabilitiesOMRCMER -= totalLoanOMRCMER;
            totalLiabilitiesPKR -= totalLoanPKR;
            totalLiabilitiesPKRCMER -= totalLoanPKRCMER;
            totalLiabilitiesOther -= totalLoanOther;
            totalLiabilitiesOtherCMER -= totalLoanOtherCMER;
            totalLiabilitiesCMER -=
                +totalLoanOtherCMER
               + totalLoanPKRCMER
                + totalLoanOMRCMER
                + totalLoanAEDCMER
                + totalLoanEURCMER
                + totalLoanUSDCMER
                + totalLoanGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }


        private void isAdvanceCurrency_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbAdvanceCurrency.IsEnabled = false;
            cmbAdvanceCurrency.SelectedItem = null;
        }

        private void isAdvanceCurrency_Checked(object sender, RoutedEventArgs e)
        {
            cmbAdvanceCurrency.IsEnabled = true;
        }

        private void isLoanCurrency_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbLoanCurrency.IsEnabled = false;
            cmbLoanCurrency.SelectedItem = null;
        }

        private void isLoanCurrency_Checked(object sender, RoutedEventArgs e)
        {
            cmbLoanCurrency.IsEnabled = true;

        }



      

        private void btnLoadRefreshAdvance_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplyAdvanceFilters();
            CalculateAdvanceSummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void ApplyAdvanceFilters()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            advances = loansAdvanceRepo.getCashFlowAllActive(currentUserId);
            if (selectedCompanies.Any())
            {
                var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToList();
                advances = advances.Where(x => selectedCompanyIds.Contains((int)x.companyId)).ToList();
            }

            // Check if there are selected departments and apply filter
            if (selectedDepartments.Any())
            {
                var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToList();
                advances = advances.Where(x => selectedDepartmentIds.Contains((int)x.deptId)).ToList();
            }
            if (cmbAdminBillCurrency.SelectedIndex > -1)
            {
                advances = advances.Where(x => x.currencyId == (cmbAdminBillCurrency.SelectedItem as cmbitem).id).ToList();
            }
            advances = advances
                .Where(loansAdvance =>
                    Math.Round(
                        loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) -
                        loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) -
                        (loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0
                            ? loansAdvance.AdminBills.Where(bill => !bill.isVoid)
                                .SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true))
                                .Sum(adj => adj.AdjustmentAmount)
                            : 0)
                    ) != 0
                )
                .ToList();
            advancesOther = advances.Where(x => x.currencyId != 1 && x.currencyId != 2 && x.currencyId != 3 && x.currencyId != 4 && x.currencyId != 5 && x.currencyId != 7).ToList();
            advancesOMR = advances.Where(x => x.currencyId == 7).ToList();
            advancesAED = advances.Where(x => x.currencyId == 5).ToList();
            advancesPKR = advances.Where(x => x.currencyId == 4).ToList();
            advancesGBP = advances.Where(x => x.currencyId == 3).ToList();
            advancesUSD = advances.Where(x => x.currencyId == 2).ToList();
            advancesEUR = advances.Where(x => x.currencyId == 1).ToList();

            advances = advances.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            grdCashFlowAdvances.ItemsSource = advances;
        }
        public void CalculateAdvanceSummary()
        {
            totalAdvanceGBP = Math.Round(advancesGBP.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0)));
            totalAdvanceUSD = Math.Round(advancesUSD.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0)));
            totalAdvanceEUR = Math.Round(advancesEUR.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0)));
            totalAdvanceAED = Math.Round(advancesAED.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0)));
            totalAdvanceOMR = Math.Round(advancesOMR.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0)));
            totalAdvancePKR = Math.Round(advancesPKR.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0)));
            totalAdvanceOther = Math.Round(advancesOther.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0)));
            totalAdvanceGBPCMER = Math.Round(advancesGBP.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalAdvanceUSDCMER = Math.Round(advancesUSD.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalAdvanceEURCMER = Math.Round(advancesEUR.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalAdvanceAEDCMER = Math.Round(advancesAED.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalAdvanceOMRCMER = Math.Round(advancesOMR.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalAdvancePKRCMER = Math.Round(advancesPKR.Sum(loansAdvance => loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalAdvanceOtherCMER = Math.Round(advancesOther.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            txtAdvancesGBP.Text = totalAdvanceGBP.ToString();
            txtAdvancesUSD.Text = totalAdvanceUSD.ToString();
            txtAdvancesEUR.Text = totalAdvanceEUR.ToString();
            txtAdvancesAED.Text = totalAdvanceAED.ToString();
            txtAdvancesOMR.Text = totalAdvanceOMR.ToString();
            txtAdvancesPKR.Text = totalAdvancePKR.ToString();
            txtAdvancesOther.Text = totalAdvanceOther.ToString();
            txtAdvancesGBPCMER.Text = totalAdvanceGBPCMER.ToString();
            txtAdvancesUSDCMER.Text = totalAdvanceUSDCMER.ToString();
            txtAdvancesEURCMER.Text = totalAdvanceEURCMER.ToString();
            txtAdvancesAEDCMER.Text = totalAdvanceAEDCMER.ToString();
            txtAdvancesOMRCMER.Text = totalAdvanceOMRCMER.ToString();
            txtAdvancesPKRCMER.Text = totalAdvancePKRCMER.ToString();
            txtAdvancesOtherCMER.Text = totalAdvanceOtherCMER.ToString();
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

      

        


    

        private void grdAdvancesRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = grdCashFlowAdvances.SelectedItem as LoansAdvance;
            if (selectedItem != null)
            {
                switch (selectedItem.advanceTemplate)
                {
                    case LoansAdvanceTemplate.Advance:
                        switch (selectedItem.loansAdvanceType)
                        {

                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") != null)
                                {
                                    ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                                    frmLoansAdvances.loansAdvanceId = selectedItem.Id;
                                    frmLoansAdvances.editFlag = true;
                                    Window win = new Window();
                                    win.Content = frmLoansAdvances;
                                    win.WindowState = WindowState.Maximized;
                                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    win.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;

                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Loans Advances") != null)
                                {
                                    ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();
                                    frmBillLoansAdvance.loansAdvanceId = selectedItem.Id;
                                    frmBillLoansAdvance.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmBillLoansAdvance;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;

                    case LoansAdvanceTemplate.Loan:
                        switch (selectedItem.loansAdvanceType)
                        {
                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                {
                                    ucFrmAdminBillLoan frmCompanyLoan = new ucFrmAdminBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                {
                                    ucFrmVendorBillLoan frmCompanyLoan = new ucFrmVendorBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;
                }
            }
        }

        private void grdAdvancesRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var loansAdvance = grdCashFlowAdvances.GetRowByListIndex(e.ListSourceRowIndex) as LoansAdvance;

            switch (e.Column.FieldName)
            {
                case "Employee":
                    if (loansAdvance.ApplicantEmployee != null)
                        e.Value = loansAdvance.ApplicantEmployee.person.FName + " " + loansAdvance.ApplicantEmployee.person.LName;
                    break;
                case "BalanceAmount":
                    e.Value = loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) - loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid)
                                  .SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true))
                                  .Sum(adj => adj.AdjustmentAmount) : 0);
                    break;
            }
        }


      

        private void btnLoadRefreshLoan_Click(object sender, RoutedEventArgs e)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplyLoanFilters();
            CalculateLoanSummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void ApplyLoanFilters()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            loans = loansRepo.getCashFlowAllActive(currentUserId);
            if (selectedCompanies.Any())
            {
                var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToList();
                loans = loans.Where(x => selectedCompanyIds.Contains((int)x.companyId)).ToList();
            }

            // Check if there are selected departments and apply filter
            if (selectedDepartments.Any())
            {
                var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToList();
                loans = loans.Where(x => selectedDepartmentIds.Contains((int)x.deptId)).ToList();
            }
            if (cmbAdminBillCurrency.SelectedIndex > -1)
            {
                loans = loans.Where(x => x.currencyId == (cmbAdminBillCurrency.SelectedItem as cmbitem).id).ToList();
            }
            loans = loans
                .Where(loansAdvance =>
                    Math.Round(
                        loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) -
                        loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) -
                        (loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0
                            ? loansAdvance.AdminBills.Where(bill => !bill.isVoid)
                                .SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true))
                                .Sum(adj => adj.AdjustmentAmount)
                            : 0)
                    ) != 0
                )
                .ToList();
            loansOther = loans.Where(x => x.currencyId != 1 && x.currencyId != 2 && x.currencyId != 3 && x.currencyId != 4 && x.currencyId != 5 && x.currencyId != 7).ToList();
            loansOMR = loans.Where(x => x.currencyId == 7).ToList();
            loansAED = loans.Where(x => x.currencyId == 5).ToList();
            loansPKR = loans.Where(x => x.currencyId == 4).ToList();
            loansGBP = loans.Where(x => x.currencyId == 3).ToList();
            loansUSD = loans.Where(x => x.currencyId == 2).ToList();
            loansEUR = loans.Where(x => x.currencyId == 1).ToList();

            loans = loans.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            grdCashFlowLoans.ItemsSource = loans;
        }
        public void CalculateLoanSummary()
        {
            totalLoanGBP = Math.Round(loansGBP.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)));
            totalLoanUSD = Math.Round(loansUSD.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)));
            totalLoanEUR = Math.Round(loansEUR.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)));
            totalLoanAED = Math.Round(loansAED.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)));
            totalLoanOMR = Math.Round(loansOMR.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)));
            totalLoanPKR = Math.Round(loansPKR.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)));
            totalLoanOther = Math.Round(loansOther.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - (loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount))));
            totalLoanGBPCMER = Math.Round(loansGBP.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalLoanUSDCMER = Math.Round(loansUSD.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalLoanEURCMER = Math.Round(loansEUR.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalLoanAEDCMER = Math.Round(loansAED.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalLoanOMRCMER = Math.Round(loansOMR.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalLoanPKRCMER = Math.Round(loansPKR.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            totalLoanOtherCMER = Math.Round(loansOther.Sum(loansAdvance => loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid).SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true)).Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount) * GetCMER((int)loansAdvance?.company.CurrencyId, (int)loansAdvance?.currencyId, (int)loansAdvance?.companyId)));
            txtLoansGBP.Text = totalLoanGBP.ToString();
            txtLoansUSD.Text = totalLoanUSD.ToString();
            txtLoansEUR.Text = totalLoanEUR.ToString();
            txtLoansAED.Text = totalLoanAED.ToString();
            txtLoansOMR.Text = totalLoanOMR.ToString();
            txtLoansPKR.Text = totalLoanPKR.ToString();
            txtLoansOther.Text = totalLoanOther.ToString();
            txtLoansGBPCMER.Text = totalLoanGBPCMER.ToString();
            txtLoansUSDCMER.Text = totalLoanUSDCMER.ToString();
            txtLoansEURCMER.Text = totalLoanEURCMER.ToString();
            txtLoansAEDCMER.Text = totalLoanAEDCMER.ToString();
            txtLoansOMRCMER.Text = totalLoanOMRCMER.ToString();
            txtLoansPKRCMER.Text = totalLoanPKRCMER.ToString();
            txtLoansOtherCMER.Text = totalLoanOtherCMER.ToString();
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

     

        private void grdLoansAdvancesRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = grdCashFlowLoans.SelectedItem as LoansAdvance;
            if (selectedItem != null)
            {
                switch (selectedItem.advanceTemplate)
                {
                    case LoansAdvanceTemplate.Advance:
                        switch (selectedItem.loansAdvanceType)
                        {

                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") != null)
                                {
                                    ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                                    frmLoansAdvances.loansAdvanceId = selectedItem.Id;
                                    frmLoansAdvances.editFlag = true;
                                    Window win = new Window();
                                    win.Content = frmLoansAdvances;
                                    win.WindowState = WindowState.Maximized;
                                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    win.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;

                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Loans Advances") != null)
                                {
                                    ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();
                                    frmBillLoansAdvance.loansAdvanceId = selectedItem.Id;
                                    frmBillLoansAdvance.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmBillLoansAdvance;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;

                    case LoansAdvanceTemplate.Loan:
                        switch (selectedItem.loansAdvanceType)
                        {
                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                {
                                    ucFrmAdminBillLoan frmCompanyLoan = new ucFrmAdminBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                {
                                    ucFrmVendorBillLoan frmCompanyLoan = new ucFrmVendorBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;
                }
            }
        }

        private void grdLoansAdvancesRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var loansAdvance = grdCashFlowLoans.GetRowByListIndex(e.ListSourceRowIndex) as LoansAdvance;

            switch (e.Column.FieldName)
            {

                case "Employee":
                    if (loansAdvance.ApplicantEmployee != null)
                        e.Value = loansAdvance.ApplicantEmployee.person.FName + " " + loansAdvance.ApplicantEmployee.person.LName;
                    break;
                case "BalanceAmount":
                    e.Value = loansAdvance.SalesReceipts.Where(receipt => !receipt.isVoid).Sum(receipt => receipt.CollectionAmount) - ((loansAdvance.AdminBills != null && loansAdvance.AdminBills.Count > 0) ? loansAdvance.AdminBills.Where(bill => !bill.isVoid)
                                  .SelectMany(bill => bill.Adjustments.Where(adj => adj.isApproved == true))
                                  .Sum(adj => adj.AdjustmentAmount) : 0) - loansAdvance.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount);
                    break;
            }
        }

        private void chkAPPaymentsLiabilities_Unchecked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP -= totalPaymentGBP;
            totalLiabilitiesGBPCMER -= totalPaymentGBPCMER;
            totalLiabilitiesUSD -= totalPaymentUSD;
            totalLiabilitiesUSDCMER -= totalPaymentUSDCMER;
            totalLiabilitiesEUR -= totalPaymentEUR;
            totalLiabilitiesEURCMER -= totalPaymentEURCMER;
            totalLiabilitiesAED -= totalPaymentAED;
            totalLiabilitiesAEDCMER -= totalPaymentAEDCMER;
            totalLiabilitiesOMR -= totalPaymentOMR;
            totalLiabilitiesOMRCMER -= totalPaymentOMRCMER;
            totalLiabilitiesPKR -= totalPaymentPKR;
            totalLiabilitiesPKRCMER -= totalPaymentPKRCMER;
            totalLiabilitiesOther -= totalPaymentOther;
            totalLiabilitiesOtherCMER -= totalPaymentOtherCMER;
            totalLiabilitiesCMER -=
                +totalPaymentOtherCMER
               + totalPaymentPKRCMER
                + totalPaymentOMRCMER
                + totalPaymentAEDCMER
                + totalPaymentEURCMER
                + totalPaymentUSDCMER
                + totalPaymentGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkAPPaymentsLiabilities_Checked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP += totalPaymentGBP;
            totalLiabilitiesGBPCMER += totalPaymentGBPCMER;
            totalLiabilitiesUSD += totalPaymentUSD;
            totalLiabilitiesUSDCMER += totalPaymentUSDCMER;
            totalLiabilitiesEUR += totalPaymentEUR;
            totalLiabilitiesEURCMER += totalPaymentEURCMER;
            totalLiabilitiesAED += totalPaymentAED;
            totalLiabilitiesAEDCMER += totalPaymentAEDCMER;
            totalLiabilitiesOMR += totalPaymentOMR;
            totalLiabilitiesOMRCMER += totalPaymentOMRCMER;
            totalLiabilitiesPKR += totalPaymentPKR;
            totalLiabilitiesPKRCMER += totalPaymentPKRCMER;
            totalLiabilitiesOther += totalPaymentOther;
            totalLiabilitiesOtherCMER += totalPaymentOtherCMER;
            totalLiabilitiesCMER +=
                +totalPaymentOtherCMER
                + totalPaymentPKRCMER
                + totalPaymentOMRCMER
                + totalPaymentAEDCMER
                + totalPaymentEURCMER
                + totalPaymentUSDCMER
                + totalPaymentGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkAdminBillLiabilities_Unchecked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP -= totalAdminBillGBP;
            totalLiabilitiesGBPCMER -= totalAdminBillGBPCMER;
            totalLiabilitiesUSD -= totalAdminBillUSD;
            totalLiabilitiesUSDCMER -= totalAdminBillUSDCMER;
            totalLiabilitiesEUR -= totalAdminBillEUR;
            totalLiabilitiesEURCMER -= totalAdminBillEURCMER;
            totalLiabilitiesAED -= totalAdminBillAED;
            totalLiabilitiesAEDCMER -= totalAdminBillAEDCMER;
            totalLiabilitiesOMR -= totalAdminBillOMR;
            totalLiabilitiesOMRCMER -= totalAdminBillOMRCMER;
            totalLiabilitiesPKR -= totalAdminBillPKR;
            totalLiabilitiesPKRCMER -= totalAdminBillPKRCMER;
            totalLiabilitiesOther -= totalAdminBillOther;
            totalLiabilitiesOtherCMER -= totalAdminBillOtherCMER;
            totalLiabilitiesCMER -=
                +totalAdminBillOtherCMER
               + totalAdminBillPKRCMER
                + totalAdminBillOMRCMER
                + totalAdminBillAEDCMER
                + totalAdminBillEURCMER
                + totalAdminBillUSDCMER
                + totalAdminBillGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkAdminBillLiabilities_Checked(object sender, RoutedEventArgs e)
        {

            totalLiabilitiesGBP += totalAdminBillGBP;
            totalLiabilitiesGBPCMER += totalAdminBillGBPCMER;
            totalLiabilitiesUSD += totalAdminBillUSD;
            totalLiabilitiesUSDCMER += totalAdminBillUSDCMER;
            totalLiabilitiesEUR += totalAdminBillEUR;
            totalLiabilitiesEURCMER += totalAdminBillEURCMER;
            totalLiabilitiesAED += totalAdminBillAED;
            totalLiabilitiesAEDCMER += totalAdminBillAEDCMER;
            totalLiabilitiesOMR += totalAdminBillOMR;
            totalLiabilitiesOMRCMER += totalAdminBillOMRCMER;
            totalLiabilitiesPKR += totalAdminBillPKR;
            totalLiabilitiesPKRCMER += totalAdminBillPKRCMER;
            totalLiabilitiesOther += totalAdminBillOther;
            totalLiabilitiesOtherCMER += totalAdminBillOtherCMER;
            totalLiabilitiesCMER +=
                +totalAdminBillOtherCMER
                + totalAdminBillPKRCMER
                + totalAdminBillOMRCMER
                + totalAdminBillAEDCMER
                + totalAdminBillEURCMER
                + totalAdminBillUSDCMER
                + totalAdminBillGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void grdAdminBillCashFlow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateBillWindow();
        }
        private void UpdateBillWindow()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill") != null)
            {
                ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                DXWindow frmBill = new DXWindow();
                frmBill.WindowState = WindowState.Maximized;
                frmBill.Title = "Update Bills";

                var selectedRow = grdAdminBillCashFlow.SelectedItem as AdminBill;

                if (selectedRow != null)
                {
                    AdminBillsRepo billsRepo = new AdminBillsRepo();
                    frmBillAdd.bills = new List<AdminBill>();
                    frmBillAdd.bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                    frmBillAdd.editFlag = true;
                    frmBillAdd.groupId = selectedRow.transactionGroupId;
                    frmBillAdd.isProgressiveCost = selectedRow.isProgressiveCost;
                    frmBillAdd.groupId = selectedRow.transactionGroupId;
                    frmBill.Content = frmBillAdd;
                    frmBill.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Existing Bill!");
            }

        }

        private void grdAdminBillCashFlow_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            //var bill=e.Source.CurrentItem as AdminBill ;
            var bill = grdAdminBillCashFlow.GetRowByListIndex(e.ListSourceRowIndex) as AdminBill;

            if (e.IsGetData)
            {

                switch (e.Column.FieldName)
                {
                    case "Stage":
                        if (bill != null)
                        {
                            if (bill.isVoid == true)
                            {
                                e.Value = "Void";
                            }
                            else if (bill.isReApproved == false)
                            {
                                e.Value = "Under Re-Approval";
                            }
                            else if (bill.isApproved == true && bill.stage == "Closed")
                            {
                                e.Value = "Closed";
                            }
                            else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
                            {
                                e.Value = "Closed";
                            }
                            else if (bill.isApproved == true && bill.PendingForClosing == true)
                            {
                                e.Value = "Under Closing";
                            }
                            else if (bill.isApproved == true)
                            {
                                e.Value = "Approved";
                            }
                            else if (bill.isApproved == false)
                            {
                                e.Value = "Under Approval";
                            }
                            else if (bill.PendingForClosing == true)
                            {
                                e.Value = "Under Closing";
                            }
                        }
                        break;
                    case "paidAmount":
                        if (bill.Payments.Count != 0)
                        {
                            var pymnts = bill.Payments.Where(x => x.isVoid != true).ToList();
                            e.Value = pymnts.Sum(x => x.DebitedAmount);
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(0);
                        }
                        break;

                    case "balanceAmount":
                        if (bill.Payments.Count != 0)
                        {
                            var pymntss = bill.Payments.Where(x => x.isVoid != true).ToList();
                            var amountPaid = pymntss.Sum(x => x.DebitedAmount);
                            e.Value = bill.AmountOC - amountPaid;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(bill.AmountOC);
                        }
                        break;
                }
            }
        }
       

        private void btnAdminBillLoadRefresh_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplyAdminBillFilters();
            CalculateAdminBillSummary();
            calculateTotalCMERLiabilities();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        public void ApplyAdminBillFilters()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            adminBills = adminbillRepo.getCashFlowAllActive(currentUserId);
            if (selectedCompanies.Any())
            {
                var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToList();
                adminBills = adminBills.Where(x => selectedCompanyIds.Contains((int)x.company_Id)).ToList();
            }

            // Check if there are selected departments and apply filter
            if (selectedDepartments.Any())
            {
                var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToList();
                adminBills = adminBills.Where(x => selectedDepartmentIds.Contains((int)x.dept_Id)).ToList();
            }
            if (cmbAdminBillCurrency.SelectedIndex > -1)
            {
                adminBills = adminBills.Where(x => x.currency_Id == (cmbAdminBillCurrency.SelectedItem as cmbitem).id).ToList();
            }
            adminBills = adminBills.Where(bill => bill.AmountOC != bill.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)).ToList();
            adminBillsOther = adminBills.Where(x => x.currency_Id != 1 && x.currency_Id != 2 && x.currency_Id != 3 && x.currency_Id != 4 && x.currency_Id != 5 && x.currency_Id != 7).ToList();
            adminBillsOMR = adminBills.Where(x => x.currency_Id == 7).ToList();
            adminBillsAED = adminBills.Where(x => x.currency_Id == 5).ToList();
            adminBillsPKR = adminBills.Where(x => x.currency_Id == 4).ToList();
            adminBillsGBP = adminBills.Where(x => x.currency_Id == 3).ToList();
            adminBillsUSD = adminBills.Where(x => x.currency_Id == 2).ToList();
            adminBillsEUR = adminBills.Where(x => x.currency_Id == 1).ToList();

            adminBills = adminBills.GroupBy(x => x.Id).Select(y => y.First()).ToList();
            grdAdminBillCashFlow.ItemsSource = adminBills;
        }
        public void CalculateAdminBillSummary()
        {
            totalAdminBillGBP = Math.Round(adminBillsGBP.Sum(bill => bill.AmountOC - bill.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)), 2);
            txtAdminBillsGBP.Text = totalAdminBillGBP.ToString();
            totalAdminBillUSD = Math.Round(adminBillsUSD.Sum(bill => bill.AmountOC - bill.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)), 2);
            txtAdminBillsUSD.Text = totalAdminBillUSD.ToString();
            totalAdminBillEUR = Math.Round(adminBillsEUR.Sum(bill => bill.AmountOC - bill.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)), 2);
            txtAdminBillsEUR.Text = totalAdminBillEUR.ToString();
            totalAdminBillAED = Math.Round(adminBillsAED.Sum(bill => bill.AmountOC - bill.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)), 2);
            txtAdminBillsAED.Text = totalAdminBillAED.ToString();
            totalAdminBillOMR = Math.Round(adminBillsOMR.Sum(bill => bill.AmountOC - bill.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)), 2);
            txtAdminBillsOMR.Text = totalAdminBillOMR.ToString();
            totalAdminBillPKR = Math.Round(adminBillsPKR.Sum(bill => bill.AmountOC - bill.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)), 2);
            txtAdminBillsPKR.Text = totalAdminBillPKR.ToString();
            totalAdminBillOther = Math.Round(adminBillsOther.Sum(bill => bill.AmountOC - bill.Payments.Where(payment => !payment.isVoid).Sum(payment => payment.DebitedAmount)), 2);
            txtAdminBillsOther.Text = totalAdminBillOther.ToString();


            totalAdminBillGBPCMER = adminBillsGBP.Count == 0 ? 0 : Math.Round(totalAdminBillGBP * GetCMER((int)adminBillsGBP[0]?.company.CurrencyId, (int)adminBillsGBP[0]?.currency_Id, (int)adminBillsGBP[0]?.company_Id), 2);
            txtAdminBillsGBPCMER.Text = totalAdminBillGBPCMER.ToString();


            totalAdminBillUSDCMER = adminBillsUSD.Count == 0 ? 0 : Math.Round(totalAdminBillUSD * GetCMER((int)adminBillsUSD[0]?.company.CurrencyId, (int)adminBillsUSD[0]?.currency_Id, (int)adminBillsUSD[0]?.company_Id), 2);
            txtAdminBillsUSDCMER.Text = totalAdminBillUSDCMER.ToString();


            totalAdminBillEURCMER = adminBillsEUR.Count == 0 ? 0 : Math.Round(totalAdminBillEUR * GetCMER((int)adminBillsEUR[0]?.company.CurrencyId, (int)adminBillsEUR[0]?.currency_Id, (int)adminBillsEUR[0]?.company_Id), 2);
            txtAdminBillsEURCMER.Text = totalAdminBillEURCMER.ToString();



            totalAdminBillAEDCMER = adminBillsAED.Count == 0 ? 0 : Math.Round(totalAdminBillAED * GetCMER((int)adminBillsAED[0]?.company.CurrencyId, (int)adminBillsAED[0]?.currency_Id, (int)adminBillsAED[0]?.company_Id), 2);
            txtAdminBillsAEDCMER.Text = totalAdminBillAEDCMER.ToString();


            totalAdminBillOMRCMER = adminBillsOMR.Count == 0 ? 0 : Math.Round(totalAdminBillOMR * GetCMER((int)adminBillsOMR[0]?.company.CurrencyId, (int)adminBillsOMR[0]?.currency_Id, (int)adminBillsOMR[0]?.company_Id), 2);
            txtAdminBillsOMRCMER.Text = totalAdminBillOMRCMER.ToString();

            totalAdminBillPKRCMER = adminBillsPKR.Count == 0 ? 0 : Math.Round(totalAdminBillPKR * GetCMER((int)adminBillsPKR[0]?.company.CurrencyId, (int)adminBillsPKR[0]?.currency_Id, (int)adminBillsPKR[0]?.company_Id), 2);
            txtAdminBillsPKRCMER.Text = totalAdminBillPKRCMER.ToString();


            totalAdminBillOtherCMER = adminBillsOther.Count == 0 ? 0 : Math.Round(totalAdminBillOther * GetCMER((int)adminBillsOther[0]?.company.CurrencyId, (int)adminBillsOther[0]?.currency_Id, (int)adminBillsOther[0]?.company_Id), 2);
            txtAdminBillsOtherCMER.Text = totalAdminBillOtherCMER.ToString();

        }
        private void isAdminBillCurrency_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbAdminBillCurrency.IsEnabled = false;
            cmbAdminBillCurrency.SelectedItem = null;
        }

        private void isAdminBillCurrency_Checked(object sender, RoutedEventArgs e)
        {
            cmbAdminBillCurrency.IsEnabled = true;

        }

        private void chkAPSaleOrdersLiabilities_Unchecked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP -= totalSaleOrdersAPGBP;
            totalLiabilitiesGBPCMER -= totalSaleOrdersAPGBPCMER;
            totalLiabilitiesUSD -= totalSaleOrdersAPUSD;
            totalLiabilitiesUSDCMER -= totalSaleOrdersAPUSDCMER;
            totalLiabilitiesEUR -= totalSaleOrdersAPEUR;
            totalLiabilitiesEURCMER -= totalSaleOrdersAPEURCMER;
            totalLiabilitiesAED -= totalSaleOrdersAPAED;
            totalLiabilitiesAEDCMER -= totalSaleOrdersAPAEDCMER;
            totalLiabilitiesOMR -= totalSaleOrdersAPOMR;
            totalLiabilitiesOMRCMER -= totalSaleOrdersAPOMRCMER;
            totalLiabilitiesPKR -= totalSaleOrdersAPPKR;
            totalLiabilitiesPKRCMER -= totalSaleOrdersAPPKRCMER;
            totalLiabilitiesOther -= totalSaleOrdersAPOther;
            totalLiabilitiesOtherCMER -= totalSaleOrdersAPOtherCMER;
            totalLiabilitiesCMER -=
                +totalSaleOrdersAPOtherCMER
               + totalSaleOrdersAPPKRCMER
                + totalSaleOrdersAPOMRCMER
                + totalSaleOrdersAPAEDCMER
                + totalSaleOrdersAPEURCMER
                + totalSaleOrdersAPUSDCMER
                + totalSaleOrdersAPGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkAPSaleOrdersLiabilities_Checked(object sender, RoutedEventArgs e)
        {

            totalLiabilitiesGBP += totalSaleOrdersAPGBP;
            totalLiabilitiesGBPCMER += totalSaleOrdersAPGBPCMER;
            totalLiabilitiesUSD += totalSaleOrdersAPUSD;
            totalLiabilitiesUSDCMER += totalSaleOrdersAPUSDCMER;
            totalLiabilitiesEUR += totalSaleOrdersAPEUR;
            totalLiabilitiesEURCMER += totalSaleOrdersAPEURCMER;
            totalLiabilitiesAED += totalSaleOrdersAPAED;
            totalLiabilitiesAEDCMER += totalSaleOrdersAPAEDCMER;
            totalLiabilitiesOMR += totalSaleOrdersAPOMR;
            totalLiabilitiesOMRCMER += totalSaleOrdersAPOMRCMER;
            totalLiabilitiesPKR += totalSaleOrdersAPPKR;
            totalLiabilitiesPKRCMER += totalSaleOrdersAPPKRCMER;
            totalLiabilitiesOther += totalSaleOrdersAPOther;
            totalLiabilitiesOtherCMER += totalSaleOrdersAPOtherCMER;
            totalLiabilitiesCMER +=
                +totalSaleOrdersAPOtherCMER
                + totalSaleOrdersAPPKRCMER
                + totalSaleOrdersAPOMRCMER
                + totalSaleOrdersAPAEDCMER
                + totalSaleOrdersAPEURCMER
                + totalSaleOrdersAPUSDCMER
                + totalSaleOrdersAPGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void grdsaleOrderCashFlow_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {


                if (e.IsGetData)
                    if (e.Column.FieldName == "Stage" && e.IsGetData)
                    {
                        var saleOrder = grdsaleOrderCashFlow.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                        if (saleOrder.isVoid == true)
                        {
                            e.Value = "Void";
                        }
                        else if (saleOrder.isReApproved == false)
                        {
                            e.Value = "Under Approval";
                        }
                        else if (saleOrder.isApproved == true && saleOrder.stage == "Closed")
                        {
                            e.Value = "Closed";
                        }
                        else if (saleOrder.isApproved == true && saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing != true)
                        {
                            e.Value = "Closed";
                        }
                        else if (saleOrder.isApproved == true && saleOrder.PendingForClosing == true)
                        {
                            e.Value = "Under Closing";
                        }
                        else if (saleOrder.isApproved == true)
                        {
                            e.Value = "Approved";
                        }
                        else if (saleOrder.isApproved == false)
                        {
                            e.Value = "Under Approval";
                        }
                        else if (saleOrder.PendingForClosing == true)
                        {
                            e.Value = "Under Closing";
                        }
                    }

                switch (e.Column.FieldName)
                {

                    case "BudgetMarginOC":
                        if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                        {
                            decimal total = 0;
                            var selectedRow66 = grdsaleOrderCashFlow.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                            if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                            }
                            else
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                            }
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result1 = total - Cost.TotalBudgetedMargin;
                            e.Value = result1;
                        }
                        else
                        {
                            e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("margin"));
                        }
                        break;


                    case "BudgetMarginMER":
                        var selectedRow1 = grdsaleOrderCashFlow.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var budgetCostMER = Convert.ToDouble(e.GetListSourceFieldValue("BudgetMarginOC"));

                        if (selectedRow1.saleOrdertype == InquiryType.SupplyCCC)
                        {

                            e.Value = budgetCostMER * selectedRow1.ccMER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(budgetCostMER) * Convert.ToDouble(selectedRow1.exchangeRate);
                        }
                        break;
                    case "budgetCost":
                        if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                        {
                            var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("exchangeRate"));
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            if (Cost != null && Cost.FieldValues != null)
                            {
                                var fields = Cost.FieldValues.Where(x => x.Type == 1).ToList();
                                decimal totalFieldValue = fields.Sum(x => x.Value);
                                var result3 = totalFieldValue /** exchangerate*/;
                                e.Value = Math.Round(result3, 2);
                            }
                        }
                        else
                        {
                            e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesBudgetedMargin"));
                        }
                        break;
                    case "MER":
                        {
                            var saleOrderMER = grdsaleOrderCashFlow.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            double todayRate = 0;
                            var exchangeRateGroupMER = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == saleOrderMER.currency_Id && x.base_currency_Id == saleOrderMER.company.CurrencyId && x.TargetYear == saleOrderMER.CreationDate.Value.Year);
                            if (exchangeRateGroupMER != null)
                            {
                                exchangeRateMER = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == saleOrderMER.company_Id);
                                switch (saleOrderMER.CreationDate.Value.Month)
                                {
                                    case 1:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJan;
                                        break;
                                    case 2:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateFeb;
                                        break;
                                    case 3:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateMar;
                                        break;
                                    case 4:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateApr;
                                        break;
                                    case 5:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateMay;
                                        break;
                                    case 6:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJun;
                                        break;
                                    case 7:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJul;
                                        break;
                                    case 8:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateAug;
                                        break;
                                    case 9:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateSep;
                                        break;
                                    case 10:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateOct;
                                        break;
                                    case 11:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateNov;
                                        break;
                                    case 12:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateDec;
                                        break;
                                    default:
                                        if (exchangeRateMER != null)
                                            todayRate = 0;
                                        break;
                                }
                            }
                            var total = todayRate;
                            e.Value = total;
                        }
                        break;
                }
            }
            catch (Exception)
            {
            }
        }
        private void EditSaleOrder()
        {
            var selectedItem = grdsaleOrderCashFlow.SelectedItem as SaleOrder;
            if (selectedItem != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, selectedItem.Id);
                procurmentPanel.Show();

            }
        }
        private void grdsaleOrderCashFlow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSaleOrder();
        }

        private void imgRightToLeftSaleOrderDept_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftSaleOrderDept.Width = 28;

            if (gridSelectedDepartments.SelectedItem != null)
            {
                var department = gridSelectedDepartments.SelectedItem as Department;

                if (selectedDepartments.Find(x => x.ParentID == department.Id) == null)
                {
                    selectedDepartments.Remove(department);
                    if (!allDepartments.Contains(department))
                        allDepartments.Add(department);

                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (selectedDepartments.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!allDepartments.Contains(parent))
                                allDepartments.Add(parent);
                            var findParet = selectedDepartments.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                selectedDepartments.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    gridAllDepartment.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;

                    gridAllDepartment.ItemsSource = allDepartments;
                    gridSelectedDepartments.ItemsSource = selectedDepartments;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Department which you want to Remove from Selected Departments!");
            }
        }

        private void imgRightToLeftSaleOrderDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftSaleOrderDept.Width = 28;

        }

        private void imgLeftToRightSaleOrderDept_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightSaleOrderDept.Width = 30;
            if (gridAllDepartment.SelectedItem != null)
            {
                var department = gridAllDepartment.SelectedItem as Department;
                if (allDepartments.Find(x => x.ParentID == department.Id) == null)
                {
                    allDepartments.Remove(department);
                    if (!selectedDepartments.Contains(department))
                        selectedDepartments.Add(department);
                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (allDepartments.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedDepartments.Contains(parent))
                            {
                                selectedDepartments.Add(parent);
                            }
                            var findParet = allDepartments.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                allDepartments.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    allDepartments = allDepartments.Where(x => deptIds.Contains(x.Id)).ToList();
                    gridAllDepartment.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;
                    gridAllDepartment.ItemsSource = allDepartments;
                    gridSelectedDepartments.ItemsSource = selectedDepartments;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Department which you want to Insert to Selected Departments!");
            }
        }

        private void imgLeftToRightSaleOrderDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightSaleOrderDept.Width = 28;

        }

        private void imgRightToLeftSaleOrderComp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftSaleOrderComp.Width = 30;
            if (gridSelectedCompanies.SelectedItem != null)
            {
                var company = gridSelectedCompanies.SelectedItem as Company;

                if (company.departments.Intersect(selectedDepartments).Count() > 0)
                {
                    DXMessageBox.Show("Kindly remove the Departments of this Company from Selected Departments!");
                    return;
                }
                selectedCompanies.Remove(company);
                allCompanies.Add(company);

                gridAllCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;

                var depts = selectedDepartments.Except(allDepartments.Intersect(selectedDepartments));
                allDepartments = new List<Department>();
                foreach (var _cmpny in selectedCompanies)
                {
                    allDepartments.AddRange(_cmpny.departments);
                }


                allDepartments = allDepartments.Except(depts).ToList();

                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                allDepartments = allDepartments.Where(x => deptIds.Contains(x.Id)).ToList();

                gridAllDepartment.ItemsSource = null;
                gridAllDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = null;
                gridSelectedDepartments.ItemsSource = selectedDepartments;
            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }

        private void imgRightToLeftSaleOrderComp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftSaleOrderComp.Width = 28;

        }

        private void imgLeftToRightCompSaleOrder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCompSaleOrder.Width = 30;
            if (gridAllCompany.SelectedItem != null)
            {
                var company = gridAllCompany.SelectedItem as Company;
                selectedCompanies.Add(company);
                allCompanies.Remove(company);
                gridAllCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;

                allDepartments.AddRange(company.departments.Except(selectedDepartments));

                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                allDepartments = allDepartments.Where(x => deptIds.Contains(x.Id)).ToList();
                gridAllDepartment.ItemsSource = null;
                gridAllDepartment.ItemsSource = allDepartments;
            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }

        private void imgLeftToRightCompSaleOrder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCompSaleOrder.Width = 28;

        }

        private void btnSaleOrderLoadRefresh_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplySOFilters();
            CalculateSaleOrderSummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void ApplyInventoryFilters()
        {
            ProductRepo repo = new ProductRepo();
            if (selectedCompanies.Count == 0)
            {
                DXMessageBox.Show("Please select companies ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (selectedDepartments.Count == 0)
            {
                DXMessageBox.Show("Please select departments ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var userCompIds = selectedCompanies.Select(x => x.Id).ToList();
            var userDeptIds = selectedDepartments.Select(x => x.Id).ToList();

            if (userDeptIds.Count != 0)
            {
                inventory = repo.getActiveProductsForInventory(userCompIds, userDeptIds);

            }
            else
                inventory = repo.getActiveProductsForInventory(userCompIds);

            foreach (var product in inventory)
            {

                product.Inventories = product.Inventories.Where(x => x.SaleInvoice?.isVoid != true).ToList();
                product.Inventories = product.Inventories.Where(x => x.PurchaseInvoice?.isVoid != true).ToList();
                product.Inventories = product.Inventories.Where(x => x.InventoryAdjustment?.isVoid != true).ToList();
            }
            inventoryOMR = inventory.Where(x => x.company?.CurrencyId == 7 && x.company?.CurrencyId!=null).ToList();
            inventoryAED = inventory.Where(x => x.company?.CurrencyId == 5 && x.company?.CurrencyId != null).ToList();
            inventoryPKR = inventory.Where(x => x.company?.CurrencyId == 4 && x.company?.CurrencyId != null).ToList();
            inventoryGBP = inventory.Where(x => x.company?.CurrencyId == 3 && x.company?.CurrencyId != null).ToList();
            inventoryUSD = inventory.Where(x => x.company?.CurrencyId == 2 && x.company?.CurrencyId != null).ToList();
            inventoryEUR = inventory.Where(x => x.company?.CurrencyId == 1 && x.company?.CurrencyId != null).ToList();
            grdItems.ItemsSource = inventory;
        }



        public void ApplySOFilters()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            saleOrders = saleOrderRepo.getCashFlowAllActive(currentUserId);
            if (selectedCompanies.Any())
            {
                var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToList();
                saleOrders = saleOrders.Where(x => selectedCompanyIds.Contains((int)x.company_Id)).ToList();
            }
            // Check if there are selected departments and apply filter
            if (selectedDepartments.Any())
            {
                var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToList();
                saleOrders = saleOrders.Where(x => selectedDepartmentIds.Contains(x.dept_Id)).ToList();
            }
            if (cmbSaleOrderCurrency.SelectedIndex > -1)
            {
                saleOrders = saleOrders.Where(x => x.currency_Id == (cmbSaleOrderCurrency.SelectedItem as cmbitem).id).ToList();
            }
            if (!string.IsNullOrEmpty(txtSaleOrderDaysLeft.Text) && Convert.ToInt32(txtSaleOrderDaysLeft.Text) != 0)
            {

                int value = 0;
                if (Convert.ToInt32(txtSIDaysLeft.Text) < 0)
                {
                    value = Convert.ToInt32(txtSaleOrderDaysLeft.Text);
                    value = Math.Abs(value);
                }
                else
                {
                    value = Convert.ToInt32(txtSaleOrderDaysLeft.Text);
                    value = -value;
                }
                DateTime dateTime = DateTime.Now.AddDays(value);
                if (dateTime.Date < DateTime.Now)
                {
                    saleOrders = saleOrders.Where(x => x.ExpectedPayment >= dateTime).ToList();
                    saleOrders = saleOrders.Where(x => x.ExpectedPayment <= DateTime.Now).ToList();
                }
                else
                {
                    saleOrders = saleOrders.Where(x => x.ExpectedPayment <= dateTime && x.ExpectedPayment >= DateTime.Now).ToList();
                }
            }
            saleOrdersOther = saleOrders.Where(x => x.currency_Id != 1 && x.currency_Id != 2 && x.currency_Id != 3 && x.currency_Id != 4 && x.currency_Id != 5 && x.currency_Id != 7).ToList();
            saleOrdersOMR = saleOrders.Where(x => x.currency_Id == 7).ToList();
            saleOrdersAED = saleOrders.Where(x => x.currency_Id == 5).ToList();
            saleOrdersPKR = saleOrders.Where(x => x.currency_Id == 4).ToList();
            saleOrdersGBP = saleOrders.Where(x => x.currency_Id == 3).ToList();
            saleOrdersUSD = saleOrders.Where(x => x.currency_Id == 2).ToList();
            saleOrdersEUR = saleOrders.Where(x => x.currency_Id == 1).ToList();
            grdsaleOrderCashFlow.ItemsSource = saleOrders;
        }

        private void isSaleOrderCurrency_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbSaleOrderCurrency.IsEnabled = false;
            cmbSaleOrderCurrency.SelectedItem = null;
        }

        private void isSaleOrderCurrency_Checked(object sender, RoutedEventArgs e)
        {
            cmbSaleOrderCurrency.IsEnabled = true;
        }

      
       
        public void loadCompanies()
        {
            cashFlowRepo = new CashFlowReportRepo();
            var employee = cashFlowRepo.GetEmployeeForCashFlow(SYSTEM_STATIC.currentUser.employeeId);
            allCompanies = employee;
            gridAllCompany.ItemsSource = allCompanies;
        }
       
        public void LoadMERGroups()
        {
            exchangeRateGroupsMER = exchangeRateGroupRepo.GetAllMER();
            loadCompanies();
            loadCurrencies();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view sales invoice from Cashflow statement") != null)
            {
                tabSalesInvoice.IsEnabled = true;
            }
            else
            {
                tabSalesInvoice.IsEnabled = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view purchase orders from Cashflow statement") != null)
            {
                tabPurchaseOrders.IsEnabled = true;
            }
            else
            {
                tabPurchaseOrders.IsEnabled = false;
            }
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view bills from Cashflow statement") != null)
            //{
            //    tabBills.IsEnabled = true;
            //}
            //else
            //{
            //    tabBills.IsEnabled = false;
            //}
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view adminBills from Cashflow statement") != null)
            {
                tabAdminBills.IsEnabled = true;
            }
            else
            {
                tabAdminBills.IsEnabled = false;
            }
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view sale orders from Cashflow statement") != null)
            //{
            //    tabSaleOrders.IsEnabled = true;
            //}
            //else
            //{
            //    tabSaleOrders.IsEnabled = false;
            //}
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view banks from Cashflow statement") != null)
            {
                tabBanks.IsEnabled = true;
            }
            else
            {
                tabBanks.IsEnabled = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view Cashflow Summaries") != null)
            {
                tabCashFlowSummary.IsEnabled = true;
            }
            else
            {
                tabCashFlowSummary.IsEnabled = false;
            }
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can view sales invoice from Cashflow statement") != null)
            //{
            loadMarketExchangeRates();
            loadCashFlowData();
        }
      
        public void loadCashFlowData()
        {
            try
            {
                datSICashInflowDate.EditValue = DateTime.Now;
                foreach (Company _Company in selectedCompanies)
                {
                    allCompanies = allCompanies.Where(c => c.Id != _Company.Id).ToList();

                    // Add departments from the selected company and remove duplicates
                    allDepartments.AddRange(_Company.departments);
                    allDepartments = allDepartments.GroupBy(d => d.Id).Select(d => d.First()).ToList();
                }
                // Remove selectedDepartments from allDepartments and update allEmployees
                foreach (Department _department in selectedDepartments)
                {
                    allDepartments = allDepartments.Where(d => d.Id != _department.Id).ToList();
                }
                // Update grids
                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;
                gridAllDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = selectedDepartments;
                if (cashflow.Id != 0)
                {
                    grdsaleOrderCashFlow.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.saleOrderSettingKey));
                    grdsaleInvoiceCashFlow.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.saleinvoiceSettingKey));
                    grdCashFlowbill.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.vendorBillSettingKey));
                    grdCashFlowPurchaseOrder.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.purchaseOrderSettingKey));
                    grdAdminBillCashFlow.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.adminBillSettingKey));
                    grdSTLCashFlow.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.stlSettingKey));
                    grdCashFlowBankAccounts.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.bankSettingKey));
                    grdCashFlowLoans.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.loanSettingKey));
                    grdCashFlowAdvances.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.advanceSettingKey));
                    grdCashFlowPayment.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(cashflow.paymentSettingKey));
                    ApplySOFilters();
                    
                    ApplySIFilters();
                    ApplyPOFilters();
                    ApplyBillFilters();
                    ApplyAdminBillFilters();
                    ApplyPaymentFilters();
                    ApplyAdvanceFilters();
                    ApplyLoanFilters();

                    CalculateSaleOrderSummary();
                    CalculateSalesInvoiceSummary();
                    CalculatePOAssetSummary();
                    CalculatePOLiabilitySummary();
                    CalculateAdminBillSummary();
                    CalculateBankSummary();
                    CalculateBillLiabilitySummary();
                    CalculateAdminBillSummary();
                    CalculateLoanSummary();
                    CalculatePaymentSummary();
                    CalculateSTLSummary();
                    calculateTotalAssets();
                    calculateTotalCMERAssets();
                    calculateTotalLiabilities();
                    calculateTotalCMERLiabilities();
                    CalculateTotalNetWorth();


                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                });
            }
            catch (Exception)
            {
            }
        }
        private void btnLoadRefresh_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplySIFilters();
            CalculateSalesInvoiceSummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void grdsaleInvoiceCashFlow_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var saleInvoice = grdsaleInvoiceCashFlow.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;

            if (saleInvoice != null)
            {

                if (e.Column.FieldName == "daysLeft")
                {
                    if (saleInvoice.ExpectedPayment != null)
                    {
                        DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime date = saleInvoice.ExpectedPayment.Value;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = currDate.Subtract(targetDate);
                        e.Value = timeSpan.Days;
                    }
                }
                if (e.Column.FieldName == "Stage" && e.IsGetData)
                {
                    if (saleInvoice.isVoid == true)
                    {
                        e.Value = "Void";
                    }
                    //else if (saleinvoice.isreapproved == false)
                    //{
                    //    e.value = "under approval";
                    //}
                    else if (saleInvoice.isApproved == true && saleInvoice.stage == "Closed")
                    {
                        e.Value = "Closed";
                    }
                    else if (saleInvoice.isApproved == true && saleInvoice.saleInvoiceStatus?.isActive == false && saleInvoice.PendingForClosing != true)
                    {
                        e.Value = "Closed";
                    }
                    else if (saleInvoice.isApproved == true && saleInvoice.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                    else if (saleInvoice.isApproved == true)
                    {
                        e.Value = "Approved";
                    }
                    else if (saleInvoice.isApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (saleInvoice.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                }
                if (e.Column.FieldName == "Vendorss" && e.IsGetData)
                {
                    string vendorNames = "";

                    if (saleInvoice.vendors != null && saleInvoice.vendors.Count > 0)
                    {
                        vendorNames = String.Join(" | ", saleInvoice.vendors.Select(x => x.company.CompanyName));
                    }
                    e.Value = vendorNames;
                }
                if (e.Column.FieldName == "recieptTotalOC" && datSICashInflowDate.EditValue != null)
                {
                    if (saleInvoice.salesReceipts != null && datSICashInflowDate.EditValue != null)
                    {
                        var reciepts = saleInvoice.salesReceipts.Where(x => x.GLPostingDate <= (DateTime)datSICashInflowDate.EditValue);
                        if (reciepts != null)
                        {
                            var result = reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                            e.Value = result;
                        }
                    }
                }
                if (e.Column.FieldName == "remainingCollectionOC")
                {
                    if (saleInvoice.salesReceipts != null && datSICashInflowDate.EditValue != null)
                    {
                        var reciepts = saleInvoice.salesReceipts.Where(x => x.GLPostingDate <= (DateTime)datSICashInflowDate.EditValue);
                        if (reciepts != null && e.GetListSourceFieldValue("totalInvoiceAmount") != null)
                        {
                            var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalInvoiceAmount"));
                            var result = Convert.ToDecimal(reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount));
                            e.Value = total - result;
                        }
                    }
                }
                if (e.Column.FieldName == "cMER")
                {
                    double todayRate = 0;
                    var exchangeRateGroupMER = exchangeRateGroups.FirstOrDefault(x => x.transaction_currency_Id == saleInvoice.currency_Id && x.base_currency_Id == saleInvoice.company.CurrencyId && x.TargetYear == DateTime.Now.Year);

                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == saleInvoice.company_Id);
                        switch (DateTime.Now.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJan;
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateFeb;
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMar;
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateApr;
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMay;
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJun;
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJul;
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateAug;
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateSep;
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateOct;
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateNov;
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateDec;
                                break;
                            default:
                                if (exchangeRate != null)
                                    todayRate = 0;
                                break;
                        }
                    }

                    var total = todayRate;
                    e.Value = total;
                }
            }
        }
        private void grdsaleInvoiceCashFlow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdsaleInvoiceCashFlow.SelectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(ERP_BL.Enums.TransactionItemType.Sale_Invoice, (int)(grdsaleInvoiceCashFlow.SelectedItem as ERP_BL.Databases.SaleInvoice).Id);
                procurmentPanel.Show();
            }
        }
        private void loadMarketExchangeRates()
        {
            exchangeRateGroups = exchangeRateGroupRepo.GetAllMER();
        }
        private void grdCashFlowPurchaseOrder_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var purchaseOrder = grdCashFlowPurchaseOrder.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseOrder;
            if (purchaseOrder != null)
            {
                if (e.Column.FieldName == "Stage" && e.IsGetData)
                {
                    if (purchaseOrder.isVoid == true)
                    {
                        e.Value = "Void";
                    }
                    else if (purchaseOrder.isReApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (purchaseOrder.isApproved == true && purchaseOrder.stage == "Closed")
                    {
                        e.Value = "Closed";
                    }
                    else if (purchaseOrder.isApproved == true && purchaseOrder.PurchaseOrderStatus.isActive == false && purchaseOrder.PendingForClosing != true)
                    {
                        e.Value = "Closed";
                    }
                    else if (purchaseOrder.isApproved == true && purchaseOrder.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                    else if (purchaseOrder.isApproved == true)
                    {
                        e.Value = "Approved";
                    }
                    else if (purchaseOrder.isApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (purchaseOrder.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                }
                if (e.Column.FieldName == "InvoicedAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        decimal amountWithTax = 0;
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                        if (purchaseOrder.PurchaseInvoices != null)
                        {
                            var invoicedAmount = Convert.ToDecimal(purchaseOrder.PurchaseInvoices.Sum(x => x.totalInvoiceAmount));
                            if (purchaseOrder.PurchaseInvoices.Count > 0)
                                if (purchaseOrder.tax != null)
                                {
                                    amountWithTax = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(purchaseOrder.tax.percentage)) / 100);
                                }
                                else
                                {
                                    amountWithTax = invoicedAmount + Convert.ToDecimal(purchaseOrder.totaltaxAmount);
                                }
                        }
                        e.Value = amountWithTax;
                    }
                }
                if (e.Column.FieldName == "UninvoicedAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        decimal amountWithTax = 0;

                        if (purchaseOrder.PurchaseInvoices != null)
                        {
                            var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                            PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();

                            var invoicedAmount = Convert.ToDecimal(purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount));
                            if (purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).ToList().Count > 0)
                            {
                                if (purchaseOrder.tax != null && purchaseOrder.billWithTax != null)
                                {
                                    invoicedAmount = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(purchaseOrder.tax.percentage)) / 100);
                                    amountWithTax = Convert.ToDecimal(purchaseOrder.billWithTax.Value) - invoicedAmount;
                                }
                                if (purchaseOrder.totaltaxAmount != 0)
                                {
                                    invoicedAmount = invoicedAmount + Convert.ToDecimal(purchaseOrder.totaltaxAmount);
                                    amountWithTax = Convert.ToDecimal(purchaseOrder.billWithTax.Value) - invoicedAmount;
                                }
                                else
                                {
                                    amountWithTax = Convert.ToDecimal(purchaseOrder.totalCFRValue) - invoicedAmount;
                                }

                            }
                            else
                            {
                                if (purchaseOrder.tax != null && purchaseOrder.billWithTax != null)
                                {
                                    amountWithTax = Convert.ToDecimal(purchaseOrder.billWithTax.Value);
                                }
                                else
                                {
                                    amountWithTax = Convert.ToDecimal(purchaseOrder.totalCFRValue);
                                }
                            }
                        }
                        e.Value = amountWithTax;
                    }
                }
                if (e.Column.FieldName == "PaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        decimal amountWithTax = 0;

                        if (purchaseOrder.PurchaseInvoices != null)
                        {
                            var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                            PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                            var PO = purchaseOrderRepo.getForGrid(id);
                            //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                            if (PO.PurchaseInvoices.Count > 0)
                                foreach (var _PI in PO.PurchaseInvoices)
                                {
                                    amountWithTax = amountWithTax + Convert.ToDecimal(_PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount));
                                }
                            //if (PO.tax != null)
                            //{
                            //    amountWithTax = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(PO.tax.percentage)) / 100);
                            //}
                            //else
                            //{
                            //    amountWithTax = invoicedAmount;
                            //}
                        }
                        e.Value = amountWithTax;
                    }
                }
                if (e.Column.FieldName == "UnpaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        decimal unPaidAmount = 0;
                        decimal amountWithTax = 0;
                        if (purchaseOrder.PurchaseInvoices != null)
                        {
                            var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                            PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                            var PO = purchaseOrderRepo.getForGrid(id);

                            if (PO.PurchaseInvoices.Count > 0)
                                foreach (var _PI in PO.PurchaseInvoices)
                                {
                                    amountWithTax = Convert.ToDecimal(amountWithTax + Convert.ToDecimal(_PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)));
                                }
                            if (PO.billWithTax != null)
                            {
                                unPaidAmount = Convert.ToDecimal(PO.billWithTax) - amountWithTax;
                            }
                            else
                            {
                                unPaidAmount = Convert.ToDecimal(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                            }
                        }

                        e.Value = unPaidAmount;
                    }
                }
                if (e.Column.FieldName == "CMER")
                {
                    double todayRate = 0;
                    var exchangeRateGroupMER = exchangeRateGroups.FirstOrDefault(x => x.transaction_currency_Id == purchaseOrder.currency_Id && x.base_currency_Id == purchaseOrder.company.CurrencyId && x.TargetYear == DateTime.Now.Year);
                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == purchaseOrder.company_Id);
                        switch (DateTime.Now.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJan;
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateFeb;
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMar;
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateApr;
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMay;
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJun;
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJul;
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateAug;
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateSep;
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateOct;
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateNov;
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateDec;
                                break;
                            default:
                                if (exchangeRate != null)
                                    todayRate = 0;
                                break;
                        }
                    }
                    var total = todayRate;
                    e.Value = total;
                }
                if (e.Column.FieldName == "daysLeft")
                {
                    DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    if (purchaseOrder.ExpectedPayment != null)
                    {
                        var date = (DateTime)purchaseOrder.ExpectedPayment;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = currDate.Subtract(targetDate);
                        e.Value = timeSpan.Days;
                    }
                }
            }

        }
        private void grdCashFlowPurchaseOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdCashFlowPurchaseOrder.SelectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (grdCashFlowPurchaseOrder.SelectedItem as PurchaseOrder).Id);
                procurmentPanel.Show();
            }
        }
        private void grdCashFlowAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedAccount = grdCashFlowBankAccounts.SelectedItem as ChartofAccount;
            if (selectedAccount != null)
            {
                var chartofAccount = chartofAccountRepo.get(selectedAccount.Id);
                winLedger ledger = new winLedger(chartofAccount);
                ledger.Title = "Transactions";
                ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.accountName;
                ledger.Show();
            }
        }
        private void GrdSaleReceiptListLoad(int id)
        {
            try
            {
                var repo = new SalesReceiptRepo();
                var saleReceipt = repo.GetSalesReceipt(id);

                if (saleReceipt != null)
                {
                    if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Customer_Credits)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Customer Credit Receipts") != null)
                        {
                            ucFrmCustomerCreditReceipt frmReceipt = new ucFrmCustomerCreditReceipt();
                            Window frmPiPaymentWindow = new Window();
                            if (saleReceipt.saleReceiptStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                {
                                    frmReceipt.editFlag = true;
                                    frmReceipt.groupId = saleReceipt.transactionGroupId;
                                    frmReceipt.receiptId = saleReceipt.Id;
                                    frmPiPaymentWindow.Content = frmReceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Direct Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                    return;
                                }
                            }
                            else
                            {
                                frmReceipt.editFlag = true;
                                frmReceipt.groupId = saleReceipt.transactionGroupId;
                                frmReceipt.receiptId = saleReceipt.Id;
                                frmPiPaymentWindow.Content = frmReceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Direct Receipts";
                                frmPiPaymentWindow.Show();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Direct Receipts!");
                        }
                    }
                    else if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Direct_Receipt)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Direct Receipts") != null)
                        {
                            ucFrmDirectSaleReceipt frmReceipt = new ucFrmDirectSaleReceipt();
                            //paymentRepo = new PaymentRepo();
                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                            Window frmPiPaymentWindow = new Window();
                            if (saleReceipt.saleReceiptStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                {
                                    frmReceipt.editFlag = true;
                                    frmReceipt.groupId = saleReceipt.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmReceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Direct Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                    return;
                                }
                            }
                            else
                            {
                                frmReceipt.editFlag = true;
                                frmReceipt.groupId = saleReceipt.transactionGroupId;
                                frmPiPaymentWindow.Content = frmReceipt;
                                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                frmPiPaymentWindow.Title = "Direct Receipts";
                                frmPiPaymentWindow.Show();
                            }

                        }
                        else
                        {
                            DXMessageBox.Show("Permission required to View Direct Receipts!");
                        }
                    }
                    else if (saleReceipt.receiptType == ERP_BL.Enums.ReceiptType.Loans_Advances)
                    {

                        switch (saleReceipt.loansAdvance.advanceTemplate)
                        {
                            case LoansAdvanceTemplate.Loan:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                                {
                                    ucFrmCompanyLoanSaleReceipt frmLAreceipt = new ucFrmCompanyLoanSaleReceipt();
                                    //paymentRepo = new PaymentRepo();
                                    //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                                    Window frmPiPaymentWindow = new Window();
                                    if (saleReceipt.saleReceiptStatus.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                        {
                                            frmLAreceipt.editFlag = true;
                                            frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                            frmPiPaymentWindow.Content = frmLAreceipt;
                                            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmPiPaymentWindow.Title = "Sale Receipts";
                                            frmPiPaymentWindow.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        frmLAreceipt.editFlag = true;
                                        frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                        frmPiPaymentWindow.Content = frmLAreceipt;
                                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPiPaymentWindow.Title = "Sale Receipts";
                                        frmPiPaymentWindow.Show();
                                    }

                                }
                                else
                                {
                                    DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                                }
                                break;
                            default:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                                {
                                    ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
                                    Window frmPiPaymentWindow = new Window();
                                    if (saleReceipt.saleReceiptStatus.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                                        {
                                            frmLAreceipt.editFlag = true;
                                            frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                            frmPiPaymentWindow.Content = frmLAreceipt;
                                            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmPiPaymentWindow.Title = "Sale Receipts";
                                            frmPiPaymentWindow.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        frmLAreceipt.editFlag = true;
                                        frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                        frmPiPaymentWindow.Content = frmLAreceipt;
                                        frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPiPaymentWindow.Title = "Sale Receipts";
                                        frmPiPaymentWindow.Show();
                                    }

                                }
                                else
                                {
                                    DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                                }
                                break;
                        }

                    }
                    else
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Open Transactions without Authority from Petty Cash") == null)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") == null)
                            {
                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                                return;
                            }
                            if (saleReceipt.saleReceiptStatus.isActive == false)
                            {
                                if (saleReceipt.saleReceiptStatus.isActive == false && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") == null))
                                {
                                    DXMessageBox.Show("Permission required to View Closed Receipts!");
                                    return;
                                }
                            }
                        }

                        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                        updateSaleReceiptObj.saveEditFlag = 1;

                        updateSaleReceiptObj.dateEditcreationDate.EditValue = saleReceipt.CreationDate;
                        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                        updateSaleReceiptObj.receiptId = saleReceipt.Id;
                        updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;

                        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
                        updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                        updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                        updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                        updateSaleReceiptObj.enterReceiptWindowFlag = true;


                        if (saleReceipt.CreditedDate != null)
                            updateSaleReceiptObj.dateEditcreditedDate.EditValue = (DateTime)saleReceipt.CreditedDate;

                        if (saleReceipt.DepositedDate != null)
                            updateSaleReceiptObj.datDepositedDate.EditValue = (DateTime)saleReceipt.DepositedDate;

                        if (saleReceipt.InstrumentDate != null)
                            updateSaleReceiptObj.datInstrumentDate.EditValue = (DateTime)saleReceipt.InstrumentDate;

                        if (saleReceipt.InstrumentNo != null)
                            updateSaleReceiptObj.txtInstrumentNo.Text = saleReceipt.InstrumentNo;

                        if (saleReceipt.principal != null)
                        {
                            PrincipalRepo prinRepo = new PrincipalRepo();
                            var principal = prinRepo.get(saleReceipt.principal.Id);
                        }

                        updateSaleReceiptObj.enter_receipt_win.Show();
                    }
                }

            }
            catch
            {

            }
        }

        private void grdCashFlowbill_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdCashFlowbill.SelectedItem as Bill != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Bill, (int)(grdCashFlowbill.SelectedItem as Bill).Id);
                procurmentPanel.Show();
            }
        }

        private void grdCashFlowbill_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var bill = grdCashFlowbill.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.Bill;
            if (bill != null)
            {
                if (e.Column.FieldName == "PaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));

                        var paidAmount = Convert.ToDecimal(bill.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
                        e.Value = paidAmount;
                    }
                }
                if (e.Column.FieldName == "UnpaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        var paidAmount = Convert.ToDecimal(bill.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
                        decimal amountWithTax = 0;
                        if (bill.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            if (bill.billWithTax != null)
                            {
                                amountWithTax = Math.Round(Convert.ToDecimal(bill.billWithTax.Value) - paidAmount, 2);
                            }
                            else
                            {
                                amountWithTax = Convert.ToDecimal(bill.totalCFRValue) - paidAmount;
                            }
                        }
                        else
                        {
                            if (bill.billWithTax != null)
                            {
                                amountWithTax = Convert.ToDecimal(bill.billWithTax.Value);
                            }
                            else
                            {
                                amountWithTax = Convert.ToDecimal(bill.totalCFRValue);
                            }
                        }
                        e.Value = amountWithTax;
                    }
                }
                if (e.Column.FieldName == "Stage" && e.IsGetData)
                {
                    if (bill.isVoid == true)
                    {
                        e.Value = "Void";
                    }
                    //else if (saleinvoice.isreapproved == false)
                    //{
                    //    e.value = "under approval";
                    //}
                    else if (bill.isApproved == true && bill.stage == "Closed")
                    {
                        e.Value = "Closed";
                    }
                    else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
                    {
                        e.Value = "Closed";
                    }
                    else if (bill.isApproved == true && bill.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                    else if (bill.isApproved == true)
                    {
                        e.Value = "Approved";
                    }
                    else if (bill.isApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (bill.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                }
            }

        }
        private void loadCurrencies()
        {
            cmbSICurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbPOCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbPaymentCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbSTLCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbBankCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }

        public void ApplySIFilters()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            saleInvoices = saleInvoiceRepo.getCashFlowAllActive(currentUserId);
            if (selectedCompanies.Any())
            {
                var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToList();
                saleInvoices = saleInvoices.Where(x => selectedCompanyIds.Contains((int)x.company_Id)).ToList();
            }
            // Check if there are selected departments and apply filter
            if (selectedDepartments.Any())
            {
                var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToList();
                saleInvoices = saleInvoices.Where(x => selectedDepartmentIds.Contains(x.dept_Id)).ToList();
            }
            if (cmbSICurrency.SelectedIndex > -1)
            {
                saleInvoices = saleInvoices.Where(x => x.currency_Id == (cmbSICurrency.SelectedItem as cmbitem).id).ToList();
            }
            if (!string.IsNullOrEmpty(txtSIDaysLeft.Text) && Convert.ToInt32(txtSIDaysLeft.Text) != 0)
            {

                int value = 0;
                if (Convert.ToInt32(txtSIDaysLeft.Text) < 0)
                {
                    value = Convert.ToInt32(txtSIDaysLeft.Text);
                    value = Math.Abs(value);
                }
                else
                {
                    value = Convert.ToInt32(txtSIDaysLeft.Text);
                    value = -value;
                }
                DateTime dateTime = DateTime.Now.AddDays(value);
                if (dateTime.Date < DateTime.Now)
                {
                    saleInvoices = saleInvoices.Where(x => x.ExpectedPayment >= dateTime).ToList();
                    saleInvoices = saleInvoices.Where(x => x.ExpectedPayment <= DateTime.Now).ToList();
                }
                else
                {

                    saleInvoices = saleInvoices.Where(x => x.ExpectedPayment <= dateTime && x.ExpectedPayment >= DateTime.Now).ToList();
                }

            }
            saleInvoicesOther = saleInvoices.Where(x => x.currency_Id != 1 && x.currency_Id != 2 && x.currency_Id != 3 && x.currency_Id != 4 && x.currency_Id != 5 && x.currency_Id != 7).ToList();
            saleInvoicesOMR = saleInvoices.Where(x => x.currency_Id == 7).ToList();
            saleInvoicesAED = saleInvoices.Where(x => x.currency_Id == 5).ToList();
            saleInvoicesPKR = saleInvoices.Where(x => x.currency_Id == 4).ToList();
            saleInvoicesGBP = saleInvoices.Where(x => x.currency_Id == 3).ToList();
            saleInvoicesUSD = saleInvoices.Where(x => x.currency_Id == 2).ToList();
            saleInvoicesEUR = saleInvoices.Where(x => x.currency_Id == 1).ToList();
            grdsaleInvoiceCashFlow.ItemsSource = saleInvoices;
        }



        public void CalculateSalesInvoiceSummary()
        {
            //GBP
            totalInvoiceAmountGBP = Math.Round(saleInvoicesGBP.Sum(x => x.totalInvoiceAmount), 2);
            totalCollectionAmountGBP = Math.Round(saleInvoicesGBP.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount)), 2);
            totalRemainigCollectionOCGBP = (Convert.ToDouble(totalInvoiceAmountGBP) - Convert.ToDouble(totalCollectionAmountGBP));
            txtTotalReceivableGBP.Text = totalRemainigCollectionOCGBP.ToString();
            //GBPCMER
            totalInvoiceAmountGBPCMER = Math.Round(saleInvoicesGBP.Sum(x => x.totalInvoiceAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            totalCollectionAmountGBPCMER = Math.Round(saleInvoicesGBP.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id))), 2);
            totalRemainigCollectionOCGBPCMER = (Convert.ToDouble(totalInvoiceAmountGBPCMER) - Convert.ToDouble(totalCollectionAmountGBPCMER));
            txtTotalReceivableGBPCMER.Text = totalRemainigCollectionOCGBPCMER.ToString();
            //USD
            totalInvoiceAmountUSD = Math.Round(saleInvoicesUSD.Sum(x => x.totalInvoiceAmount), 2);
            totalCollectionAmountUSD = Math.Round(saleInvoicesUSD.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount)), 2);
            totalRemainigCollectionOCUSD = (Convert.ToDouble(totalInvoiceAmountUSD) - Convert.ToDouble(totalCollectionAmountUSD));
            txtTotalReceivableUSD.Text = totalRemainigCollectionOCUSD.ToString();
            //USDCMER
            totalInvoiceAmountUSDCMER = Math.Round(saleInvoicesUSD.Sum(x => x.totalInvoiceAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            totalCollectionAmountUSDCMER = Math.Round(saleInvoicesUSD.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id))), 2);
            totalRemainigCollectionOCUSDCMER = (Convert.ToDouble(totalInvoiceAmountUSDCMER) - Convert.ToDouble(totalCollectionAmountUSDCMER));
            txtTotalReceivableUSDCMER.Text = totalRemainigCollectionOCUSDCMER.ToString();
            //EURO
            totalInvoiceAmountEUR = Math.Round(saleInvoicesEUR.Sum(x => x.totalInvoiceAmount), 2);
            totalCollectionAmountEUR = Math.Round(saleInvoicesEUR.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount)), 2);
            totalRemainigCollectionOCEUR = (Convert.ToDouble(totalInvoiceAmountEUR) - Convert.ToDouble(totalCollectionAmountEUR));
            txtTotalReceivableEUR.Text = totalRemainigCollectionOCEUR.ToString();
            //EUROCMER
            totalInvoiceAmountEURCMER = Math.Round(saleInvoicesEUR.Sum(x => x.totalInvoiceAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            totalCollectionAmountEURCMER = Math.Round(saleInvoicesEUR.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id))), 2);
            totalRemainigCollectionOCEURCMER = (Convert.ToDouble(totalInvoiceAmountEURCMER) - Convert.ToDouble(totalCollectionAmountEURCMER));
            txtTotalReceivableEURCMER.Text = totalRemainigCollectionOCEURCMER.ToString();
            //AED
            totalInvoiceAmountAED = Math.Round(saleInvoicesAED.Sum(x => x.totalInvoiceAmount), 2);
            totalCollectionAmountAED = Math.Round(saleInvoicesAED.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount)), 2);
            totalRemainigCollectionOCAED = (Convert.ToDouble(totalInvoiceAmountAED) - Convert.ToDouble(totalCollectionAmountAED));
            txtTotalReceivableAED.Text = totalRemainigCollectionOCAED.ToString();
            //AEDCMER
            totalInvoiceAmountAEDCMER = Math.Round(saleInvoicesAED.Sum(x => x.totalInvoiceAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            totalCollectionAmountAEDCMER = Math.Round(saleInvoicesAED.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id))), 2);
            totalRemainigCollectionOCAEDCMER = (Convert.ToDouble(totalInvoiceAmountAEDCMER) - Convert.ToDouble(totalCollectionAmountAEDCMER));
            txtTotalReceivableAEDCMER.Text = totalRemainigCollectionOCAEDCMER.ToString();
            //OMR
            totalInvoiceAmountOMR = Math.Round(saleInvoicesOMR.Sum(x => x.totalInvoiceAmount), 2);
            totalCollectionAmountOMR = Math.Round(saleInvoicesOMR.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount)), 2);
            totalRemainigCollectionOCOMR = (Convert.ToDouble(totalInvoiceAmountOMR) - Convert.ToDouble(totalCollectionAmountOMR));
            txtTotalReceivableOMR.Text = totalRemainigCollectionOCOMR.ToString();
            //OMERCMER
            totalInvoiceAmountOMRCMER = Math.Round(saleInvoicesOMR.Sum(x => x.totalInvoiceAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            totalCollectionAmountOMRCMER = Math.Round(saleInvoicesOMR.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id))), 2);
            totalRemainigCollectionOCOMRCMER = (Convert.ToDouble(totalInvoiceAmountOMRCMER) - Convert.ToDouble(totalCollectionAmountOMRCMER));
            txtTotalReceivableOMRCMER.Text = totalRemainigCollectionOCOMRCMER.ToString();
            //PKR
            totalInvoiceAmountPKR = Math.Round(saleInvoicesPKR.Sum(x => x.totalInvoiceAmount), 2);
            totalCollectionAmountPKR = Math.Round(saleInvoicesPKR.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount)), 2);
            totalRemainigCollectionOCPKR = (Convert.ToDouble(totalInvoiceAmountPKR) - Convert.ToDouble(totalCollectionAmountPKR));
            txtTotalReceivablePKR.Text = totalRemainigCollectionOCPKR.ToString();
            //PKRCMER
            totalInvoiceAmountPKRCMER = Math.Round(saleInvoicesPKR.Sum(x => x.totalInvoiceAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            totalCollectionAmountPKRCMER = Math.Round(saleInvoicesPKR.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id))), 2);
            totalRemainigCollectionOCPKRCMER = (Convert.ToDouble(totalInvoiceAmountPKRCMER) - Convert.ToDouble(totalCollectionAmountPKRCMER));
            txtTotalReceivablePKRCMER.Text = totalRemainigCollectionOCPKRCMER.ToString();
            //Other
            totalInvoiceAmountOther = Math.Round(saleInvoicesOther.Sum(x => x.totalInvoiceAmount), 2);
            totalCollectionAmountOther = Math.Round(saleInvoicesOther.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount)), 2);
            totalRemainigCollectionOCOther = (Convert.ToDouble(totalInvoiceAmountOther) - Convert.ToDouble(totalCollectionAmountOther));
            txtTotalReceivableOther.Text = totalRemainigCollectionOCOther.ToString();

            //OtherCMER
            totalInvoiceAmountOtherCMER = Math.Round(saleInvoicesOther.Sum(x => x.totalInvoiceAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            totalCollectionAmountOtherCMER = Math.Round(saleInvoicesOther.Sum(x => x.salesReceipts.Where(z => z.isVoid != true).Sum(y => y.CollectionAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id))), 2);
            totalRemainigCollectionOCOtherCMER = (Convert.ToDouble(totalInvoiceAmountOtherCMER) - Convert.ToDouble(totalCollectionAmountOtherCMER));
            txtTotalReceivableOtherCMER.Text = totalRemainigCollectionOCOtherCMER.ToString();
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }
        double CalculateInventoryTotal(List<Product> inventory)
        {
            return inventory.SelectMany(p => p.Inventories)
                .Where(x =>
                    (x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true) ||
                    (x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true) ||
                    (x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true)
                )
                .Sum(x => x.AmountOC); // This returns double directly




        }
        double CalculateInventoryTotalCMER(List<Product> inventory)
        {
            return inventory.SelectMany(p => p.Inventories)
                .Where(x =>
                    (x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true) ||
                    (x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true) ||
                    (x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true))
                .Sum(x =>
                {
                    int fromCurrency = 0;
                    int toCurrency = 0;
                    int companyId = 0;

                    if (x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.company != null)
                    {
                        toCurrency = x.PurchaseInvoice.currency_Id;
                        companyId = x.PurchaseInvoice.company.Id;
                    }
                    else if (x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.company != null)
                    {
                        toCurrency = x.SaleInvoice.currency_Id;
                        companyId = x.SaleInvoice.company.Id;
                    }
                    else if (x.TransactionsType == InventoryTransactionsType.Adjustment && x.InventoryAdjustment?.Company != null)
                    {
                        toCurrency = x.InventoryAdjustment.currency_Id;
                        companyId = (int)x.InventoryAdjustment.company_Id;
                    }

                    return x.AmountOC * GetCMER(fromCurrency, toCurrency, companyId);
                });
        }
        public void CalculateInventorySummary()
        {
            //var product = e.Node.Content as Product;
            //var totalPurchaseAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice).Sum(x => x.AmountOC);
            //var totalSaleAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice).Sum(x => x.AmountOC);
            //var totalAdjustmentAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null).Sum(x => x.AmountOC);
            //totalPurchaseAmount = totalPurchaseAmount - totalAdjustmentAmount;

            // GBP
            totalInventoryGBP = CalculateInventoryTotal(inventoryGBP);
            txtInventoryGBP.Text = totalInventoryGBP.ToString("N2");

            // USD
            totalInventoryUSD = CalculateInventoryTotal(inventoryUSD);
            txtInventoryUSD.Text = totalInventoryUSD.ToString("N2");

            // EUR
            totalInventoryEUR = CalculateInventoryTotal(inventoryEUR);
            txtInventoryEUR.Text = totalInventoryEUR.ToString("N2");

            // AED
            totalInventoryAED = CalculateInventoryTotal(inventoryAED);
            txtInventoryAED.Text = totalInventoryAED.ToString("N2");

            // OMR
            totalInventoryOMR = CalculateInventoryTotal(inventoryOMR);
            txtInventoryOMR.Text = totalInventoryOMR.ToString("N2");

            // PKR
            totalInventoryPKR = CalculateInventoryTotal(inventoryPKR);
            txtInventoryPKR.Text = totalInventoryPKR.ToString("N2");

            // Other
            totalInventoryOther = CalculateInventoryTotal(inventoryOther);
            txtInventoryOther.Text = totalInventoryOther.ToString("N2");



            // GBP
            totalInventoryGBPCMER = CalculateInventoryTotalCMER(inventoryGBP);
            txtInventoryGBPCMER.Text = totalInventoryGBPCMER.ToString("N2");

            // USD
            totalInventoryUSDCMER = CalculateInventoryTotalCMER(inventoryUSD);
            txtInventoryUSDCMER.Text = totalInventoryUSDCMER.ToString("N2");

            // EUR
            totalInventoryEURCMER = CalculateInventoryTotalCMER(inventoryEUR);
            txtInventoryEURCMER.Text = totalInventoryEURCMER.ToString("N2");

            // AED
            totalInventoryAEDCMER = CalculateInventoryTotalCMER(inventoryAED);
            txtInventoryAEDCMER.Text = totalInventoryAED.ToString("N2");

            // OMR
            totalInventoryOMRCMER = CalculateInventoryTotalCMER(inventoryOMR);
            txtInventoryOMR.Text = totalInventoryOMRCMER.ToString("N2");

            // PKR
            totalInventoryPKRCMER = CalculateInventoryTotalCMER(inventoryPKR);
            txtInventoryPKRCMER.Text = totalInventoryPKRCMER.ToString("N2");

            // Other
            totalInventoryOtherCMER = CalculateInventoryTotalCMER(inventoryOther);
            txtInventoryOtherCMER.Text = totalInventoryOtherCMER.ToString("N2");




            calculateTotalInventories();
            calculateTotalCMERAssets();
        }

        public void CalculateSaleOrderSummary()
        {
            //GBP
            totalSaleOrdersGBP = Math.Round(saleOrdersGBP.Sum(x => x.totalCFRValue), 2);
            txtTotalSOReceivableGBP.Text = totalSaleOrdersGBP.ToString();
            //GBPCMER
            totalSaleOrdersGBPCMER = Math.Round(saleOrdersGBP.Sum(x => x.totalCFRValue * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalSOReceivableGBPCMER.Text = totalSaleOrdersGBPCMER.ToString();
            //USD
            totalSaleOrdersUSD = Math.Round(saleOrdersUSD.Sum(x => x.totalCFRValue), 2);
            txtTotalSOReceivableUSD.Text = totalSaleOrdersUSD.ToString();
            //USDCMER
            totalSaleOrdersUSDCMER = Math.Round(saleOrdersUSD.Sum(x => x.totalCFRValue * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalSOReceivableUSDCMER.Text = totalSaleOrdersUSDCMER.ToString();
            //EURO
            totalSaleOrdersEUR = Math.Round(saleOrdersEUR.Sum(x => x.totalCFRValue), 2);
            txtTotalSOReceivableEUR.Text = totalSaleOrdersEUR.ToString();
            //EUROCMER
            totalSaleOrdersEURCMER = Math.Round(saleOrdersEUR.Sum(x => x.totalCFRValue * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalSOReceivableEURCMER.Text = totalSaleOrdersEURCMER.ToString();
            //AED
            totalSaleOrdersAED = Math.Round(saleOrdersAED.Sum(x => x.totalCFRValue), 2);
            txtTotalSOReceivableAED.Text = totalSaleOrdersAED.ToString();
            //AEDCMER
            totalSaleOrdersAEDCMER = Math.Round(saleOrdersAED.Sum(x => x.totalCFRValue * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalSOReceivableAEDCMER.Text = totalSaleOrdersAEDCMER.ToString();
            //OMR
            totalSaleOrdersOMR = Math.Round(saleOrdersOMR.Sum(x => x.totalCFRValue), 2);
            txtTotalSOReceivableOMR.Text = totalSaleOrdersOMR.ToString();
            //OMERCMER
            totalSaleOrdersOMRCMER = Math.Round(saleOrdersOMR.Sum(x => x.totalCFRValue * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalSOReceivableOMRCMER.Text = totalSaleOrdersOMRCMER.ToString();
            //PKR
            totalSaleOrdersPKR = Math.Round(saleOrdersPKR.Sum(x => x.totalCFRValue), 2);
            txtTotalSOReceivablePKR.Text = totalSaleOrdersPKR.ToString();
            //PKRCMER
            totalSaleOrdersPKRCMER = Math.Round(saleOrdersPKR.Sum(x => x.totalCFRValue * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalSOReceivablePKRCMER.Text = totalSaleOrdersPKRCMER.ToString();
            //Other
            totalSaleOrdersOther = Math.Round(saleOrdersOther.Sum(x => x.totalCFRValue), 2);
            txtTotalSOReceivableOther.Text = totalSaleOrdersOther.ToString();

            //OtherCMER
            totalSaleOrdersOtherCMER = Math.Round(saleOrdersOther.Sum(x => x.totalCFRValue * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalSOReceivableOtherCMER.Text = totalRemainigCollectionOCOtherCMER.ToString();
            UpdateUI();




            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }
        private double CalculateTotal(IEnumerable<SaleOrder> saleOrders)
        {
            return Math.Round(
                saleOrders
                    .Where(x => x.CostSheet_Id != null && x.CostSheet?.FieldValues != null)
                    .Sum(x => x.CostSheet.FieldValues
                        .Where(y => y.Type == 1)
                        .Sum(z => Convert.ToDouble(z?.Value ?? 0))), 2);
        }

        private double CalculateTotalWithCMER(IEnumerable<SaleOrder> saleOrders)
        {
            return Math.Round(
                saleOrders
                    .Where(x => x.CostSheet_Id != null && x.CostSheet?.FieldValues != null)
                    .Sum(x => x.CostSheet.FieldValues
                        .Where(y => y.Type == 1)
                        .Sum(z => Convert.ToDouble(z?.Value ?? 0)) *
                        GetCMER((int)x.company?.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
        }
        private void UpdateUI()
        {
            txtTotalAPSOGBP.Text = CalculateTotal(saleOrdersGBP).ToString();
            txtTotalAPSOGBPCMER.Text = CalculateTotalWithCMER(saleOrdersGBP).ToString();
            txtTotalAPSOUSD.Text = CalculateTotal(saleOrdersUSD).ToString();
            txtTotalAPSOUSDCMER.Text = CalculateTotalWithCMER(saleOrdersUSD).ToString();
            txtTotalAPSOEUR.Text = CalculateTotal(saleOrdersEUR).ToString();
            txtTotalAPSOEURCMER.Text = CalculateTotalWithCMER(saleOrdersEUR).ToString();
            txtTotalAPSOAED.Text = CalculateTotal(saleOrdersAED).ToString();
            txtTotalAPSOAEDCMER.Text = CalculateTotalWithCMER(saleOrdersAED).ToString();
            txtTotalAPSOOMR.Text = CalculateTotal(saleOrdersOMR).ToString();
            txtTotalAPSOOMRCMER.Text = CalculateTotalWithCMER(saleOrdersOMR).ToString();
            txtTotalAPSOPKR.Text = CalculateTotal(saleOrdersPKR).ToString();
            txtTotalAPSOPKRCMER.Text = CalculateTotalWithCMER(saleOrdersPKR).ToString();
            txtTotalAPSOOther.Text = CalculateTotal(saleOrdersOther).ToString();
            txtTotalAPSOOtherCMER.Text = CalculateTotalWithCMER(saleOrdersOther).ToString();






            totalSaleOrdersAPGBP = CalculateTotal(saleOrdersGBP);
            totalSaleOrdersAPGBPCMER = CalculateTotalWithCMER(saleOrdersGBP);
            totalSaleOrdersAPUSD = CalculateTotal(saleOrdersUSD);
            totalSaleOrdersAPUSDCMER = CalculateTotalWithCMER(saleOrdersUSD);
            totalSaleOrdersAPEUR = CalculateTotal(saleOrdersEUR);
            totalSaleOrdersAPEURCMER = CalculateTotalWithCMER(saleOrdersEUR);
            totalSaleOrdersAPAED = CalculateTotal(saleOrdersAED);
            totalSaleOrdersAPAEDCMER = CalculateTotalWithCMER(saleOrdersAED);
            totalSaleOrdersAPOMR = CalculateTotal(saleOrdersOMR);
            totalSaleOrdersAPOMRCMER = CalculateTotalWithCMER(saleOrdersOMR);
            totalSaleOrdersAPPKR = CalculateTotal(saleOrdersPKR);
            totalSaleOrdersAPPKRCMER = CalculateTotalWithCMER(saleOrdersPKR);
            totalSaleOrdersAPOther = CalculateTotal(saleOrdersOther);
            totalSaleOrdersAPOtherCMER = CalculateTotalWithCMER(saleOrdersOther);
        }
        public void CalculateSTLSummary()
        {
            //GBP
            totalSTLAmountGBP = Math.Round(stlsGBP.Sum(x => x.settlmentBalance), 2);
            txtTotalSTLGBP.Text = totalSTLAmountGBP.ToString();

            //GBPCMER
            totalSTLAmountGBPCMER = Math.Round(stlsGBP.Sum(x => x.settlmentBalance * GetCMER((int)x.company.CurrencyId, (int)x.stlCurrency_Id, (int)x.company_Id)), 2);
            txtTotalSTLGBPCMER.Text = totalSTLAmountGBP.ToString();
            //USD
            totalSTLAmountUSD = Math.Round(stlsUSD.Sum(x => x.settlmentBalance), 2);
            txtTotalSTLUSD.Text = totalSTLAmountUSD.ToString();
            //USDCMER
            totalSTLAmountUSDCMER = Math.Round(stlsUSD.Sum(x => x.settlmentBalance * GetCMER((int)x.company.CurrencyId, (int)x.stlCurrency_Id, (int)x.company_Id)), 2);
            txtTotalSTLUSDCMER.Text = totalSTLAmountUSDCMER.ToString();
            //EURO
            totalSTLAmountEUR = Math.Round(stlsEUR.Sum(x => x.settlmentBalance), 2);
            txtTotalSTLEUR.Text = totalSTLAmountEUR.ToString();
            //EUROCMER
            totalSTLAmountEURCMER = Math.Round(stlsEUR.Sum(x => x.settlmentBalance * GetCMER((int)x.company.CurrencyId, (int)x.stlCurrency_Id, (int)x.company_Id)), 2);
            txtTotalSTLEURCMER.Text = totalSTLAmountEURCMER.ToString();
            //AED
            totalSTLAmountAED = Math.Round(stlsAED.Sum(x => x.settlmentBalance), 2);
            txtTotalSTLAED.Text = totalSTLAmountAED.ToString();
            //AEDCMER
            totalSTLAmountAEDCMER = Math.Round(stlsAED.Sum(x => x.settlmentBalance * GetCMER((int)x.company.CurrencyId, (int)x.stlCurrency_Id, (int)x.company_Id)), 2);
            txtTotalSTLAEDCMER.Text = totalSTLAmountAEDCMER.ToString();
            //OMR
            totalSTLAmountOMR = Math.Round(stlsOMR.Sum(x => x.settlmentBalance), 2);
            txtTotalSTLOMR.Text = totalSTLAmountOMR.ToString();
            //OMERCMER
            totalSTLAmountOMRCMER = Math.Round(stlsOMR.Sum(x => x.settlmentBalance * GetCMER((int)x.company.CurrencyId, (int)x.stlCurrency_Id, (int)x.company_Id)), 2);
            txtTotalSTLOMRCMER.Text = totalSTLAmountOMRCMER.ToString();
            //PKR
            totalSTLAmountPKR = Math.Round(stlsPKR.Sum(x => x.settlmentBalance), 2);
            txtTotalSTLPKR.Text = totalSTLAmountPKR.ToString();
            totalSTLAmountPKRCMER = Math.Round(stlsPKR.Sum(x => x.settlmentBalance * GetCMER((int)x.company.CurrencyId, (int)x.stlCurrency_Id, (int)x.company_Id)), 2);
            txtTotalSTLPKRCMER.Text = totalSTLAmountPKRCMER.ToString();
            //Other
            totalSTLAmountOther = Math.Round(stlsOther.Sum(x => x.settlmentBalance), 2);
            txtTotalSTLOther.Text = totalSTLAmountOther.ToString();

            //OtherCMER
            totalSTLAmountOtherCMER = Math.Round(stlsOther.Sum(x => x.settlmentBalance * GetCMER((int)x.company.CurrencyId, (int)x.stlCurrency_Id, (int)x.company_Id)), 2);
            txtTotalSTLOtherCMER.Text = totalSTLAmountOtherCMER.ToString();
            //calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        public void CalculatePaymentSummary()
        {
            totalPaymentGBP = Math.Round(paymentsGBP.Sum(x => x.DebitedAmount), 2);
            txtTotalAPPaymentsGBP.Text = totalPaymentGBP.ToString();
            totalPaymentUSD = Math.Round(paymentsUSD.Sum(x => x.DebitedAmount), 2);
            txtTotalAPPaymentsUSD.Text = totalPaymentUSD.ToString();
            totalPaymentEUR = Math.Round(paymentsEUR.Sum(x => x.DebitedAmount), 2);
            txtTotalAPPaymentsEUR.Text = totalPaymentEUR.ToString();
            totalPaymentAED = Math.Round(paymentsAED.Sum(x => x.DebitedAmount), 2);
            txtTotalAPPaymentsAED.Text = totalPaymentAED.ToString();
            totalPaymentOMR = Math.Round(paymentsOMR.Sum(x => x.DebitedAmount), 2);
            txtTotalAPPaymentsOMR.Text = totalPaymentOMR.ToString();
            totalPaymentPKR = Math.Round(paymentsPKR.Sum(x => x.DebitedAmount), 2);
            txtTotalAPPaymentsPKR.Text = totalPaymentPKR.ToString();
            totalPaymentOther = Math.Round(paymentsOther.Sum(x => x.DebitedAmount), 2);
            txtTotalAPPaymentsOther.Text = totalPaymentOther.ToString();


            totalPaymentGBPCMER = Math.Round(paymentsGBP.Sum(x => x.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalAPPaymentsGBPCMER.Text = totalPaymentGBPCMER.ToString();
            totalPaymentUSDCMER = Math.Round(paymentsUSD.Sum(x => x.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalAPPaymentsUSDCMER.Text = totalPaymentUSDCMER.ToString();
            totalPaymentEURCMER = Math.Round(paymentsEUR.Sum(x => x.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalAPPaymentsEURCMER.Text = totalPaymentEURCMER.ToString();
            totalPaymentAEDCMER = Math.Round(paymentsAED.Sum(x => x.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalAPPaymentsAEDCMER.Text = totalPaymentAEDCMER.ToString();
            totalPaymentOMRCMER = Math.Round(paymentsOMR.Sum(x => x.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalAPPaymentsOMRCMER.Text = totalPaymentOMRCMER.ToString();
            totalPaymentPKRCMER = Math.Round(paymentsPKR.Sum(x => x.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalAPPaymentsPKRCMER.Text = totalPaymentPKRCMER.ToString();
            totalPaymentOtherCMER = Math.Round(paymentsOther.Sum(x => x.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)), 2);
            txtTotalAPPaymentsOtherCMER.Text = totalPaymentOtherCMER.ToString();

        }

        public void CalculateBillLiabilitySummary()
        {
            //GBP
            totalUnPaidPOAmountGBP = purchaseOrdersGBP.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                return unPaidAmount;
            }).Sum();

            totalUnPaidBillAmountGBP = billsGBP.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax;
            });


            //USD
            totalUnPaidBillAmountUSD = billsUSD.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax;
            });
            //EURO
            totalUnPaidBillAmountEUR = billsEUR.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax;
            });
            //AED
            totalUnPaidBillAmountAED = billsAED.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax;
            });
            //OMR
            totalUnPaidBillAmountOMR = billsOMR.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax;
            });
            //PKR
            totalUnPaidBillAmountPKR = billsPKR.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax;
            });
            //Other
            totalUnPaidBillAmountOther = billsOther.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax;
            });


            //GBPCMER
            totalUnPaidBillAmountGBPCMER = billsGBP.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax * GetCMER((int)bill.company.CurrencyId, (int)bill.currency_Id, (int)bill.company_Id);
            });
            //USD

            totalUnPaidBillAmountUSDCMER = billsUSD.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax * GetCMER((int)bill.company.CurrencyId, (int)bill.currency_Id, (int)bill.company_Id);
            });
            //EURO
            totalUnPaidBillAmountEURCMER = billsEUR.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax * GetCMER((int)bill.company.CurrencyId, (int)bill.currency_Id, (int)bill.company_Id);
            });
            //AED
            totalUnPaidBillAmountAEDCMER = billsAED.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax * GetCMER((int)bill.company.CurrencyId, (int)bill.currency_Id, (int)bill.company_Id);
            });
            //OMR
            totalUnPaidBillAmountOMRCMER = billsOMR.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax * GetCMER((int)bill.company.CurrencyId, (int)bill.currency_Id, (int)bill.company_Id);
            });
            //PKR
            totalUnPaidBillAmountPKRCMER = billsPKR.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax * GetCMER((int)bill.company.CurrencyId, (int)bill.currency_Id, (int)bill.company_Id);
            });
            //Other
            totalUnPaidBillAmountOtherCMER = billsOther.Sum(bill =>
            {
                // Calculate the paid amount for valid payments
                var paidAmount = bill.Payments
                                     .Where(payment => !payment.isVoid)
                                     .Sum(payment => payment.DebitedAmount);

                // Calculate the amount with tax based on the logic
                double amountWithTax = 0;
                if (bill.Payments.Any(payment => !payment.isVoid))
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = Math.Round(bill.billWithTax.Value - paidAmount, 2);
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue - paidAmount;
                    }
                }
                else
                {
                    if (bill.billWithTax.HasValue)
                    {
                        amountWithTax = bill.billWithTax.Value;
                    }
                    else
                    {
                        amountWithTax = bill.totalCFRValue;
                    }
                }

                return amountWithTax * GetCMER((int)bill.company.CurrencyId, (int)bill.currency_Id, (int)bill.company_Id);
            });
            txtTotalAPbillsGBP.Text = totalUnPaidBillAmountGBP.ToString();
            txtTotalAPbillsUSD.Text = totalUnPaidBillAmountUSD.ToString();
            txtTotalAPbillsEUR.Text = totalUnPaidBillAmountEUR.ToString();
            txtTotalAPbillsAED.Text = totalUnPaidBillAmountAED.ToString();
            txtTotalAPbillsOMR.Text = totalUnPaidBillAmountOMR.ToString();
            txtTotalAPbillsPKR.Text = totalUnPaidBillAmountPKR.ToString();
            txtTotalAPbillsOther.Text = totalUnPaidBillAmountOther.ToString();
            txtTotalAPbillsGBPCMER.Text = totalUnPaidBillAmountGBPCMER.ToString();
            txtTotalAPbillsUSDCMER.Text = totalUnPaidBillAmountUSDCMER.ToString();
            txtTotalAPbillsEURCMER.Text = totalUnPaidBillAmountEURCMER.ToString();
            txtTotalAPbillsAEDCMER.Text = totalUnPaidBillAmountAEDCMER.ToString();
            txtTotalAPbillsOMRCMER.Text = totalUnPaidBillAmountOMRCMER.ToString();
            txtTotalAPbillsPKRCMER.Text = totalUnPaidBillAmountPKRCMER.ToString();
            txtTotalAPbillsOtherCMER.Text = totalUnPaidBillAmountOtherCMER.ToString();
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }
        public void CalculatePOLiabilitySummary()
        {
            //GBP
            totalUnPaidPOAmountGBP = purchaseOrdersGBP.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                return unPaidAmount;
            }).Sum();
            //USD
            totalUnPaidPOAmountUSD = purchaseOrdersUSD.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                return unPaidAmount;
            }).Sum();
            //EURO
            totalUnPaidPOAmountEUR = purchaseOrdersEUR.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                return unPaidAmount;
            }).Sum();
            //AED
            totalUnPaidPOAmountAED = purchaseOrdersAED.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                return unPaidAmount;
            }).Sum();
            //OMR
            totalUnPaidPOAmountOMR = purchaseOrdersOMR.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                return unPaidAmount;
            }).Sum();
            //PKR
            totalUnPaidPOAmountPKR = purchaseOrdersPKR.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                return unPaidAmount;
            }).Sum();
            //Other
            totalUnPaidPOAmountOther = purchaseOrdersOther.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                return unPaidAmount;
            }).Sum();


            //GBPCMER
            totalUnPaidPOAmountGBPCMER = purchaseOrdersGBP.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                double unPaidAmountCMER = unPaidAmount * GetCMER((int)PO.company.CurrencyId, (int)PO.currency_Id, (int)PO.company_Id);
                return unPaidAmountCMER;
            }).Sum();
            //USD
            totalUnPaidPOAmountUSDCMER = purchaseOrdersUSD.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                double unPaidAmountCMER = unPaidAmount * GetCMER((int)PO.company.CurrencyId, (int)PO.currency_Id, (int)PO.company_Id);
                return unPaidAmountCMER;
            }).Sum();
            //EURO
            totalUnPaidPOAmountEURCMER = purchaseOrdersEUR.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                double unPaidAmountCMER = unPaidAmount * GetCMER((int)PO.company.CurrencyId, (int)PO.currency_Id, (int)PO.company_Id);
                return unPaidAmountCMER;
            }).Sum();
            //AED
            totalUnPaidPOAmountAEDCMER = purchaseOrdersAED.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                double unPaidAmountCMER = unPaidAmount * GetCMER((int)PO.company.CurrencyId, (int)PO.currency_Id, (int)PO.company_Id);
                return unPaidAmountCMER;
            }).Sum();
            //OMR
            totalUnPaidPOAmountOMRCMER = purchaseOrdersOMR.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                double unPaidAmountCMER = unPaidAmount * GetCMER((int)PO.company.CurrencyId, (int)PO.currency_Id, (int)PO.company_Id);
                return unPaidAmountCMER;
            }).Sum();
            //PKR
            totalUnPaidPOAmountPKRCMER = purchaseOrdersPKR.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                double unPaidAmountCMER = unPaidAmount * GetCMER((int)PO.company.CurrencyId, (int)PO.currency_Id, (int)PO.company_Id);
                return unPaidAmountCMER;
            }).Sum();
            //Other
            totalUnPaidPOAmountOtherCMER = purchaseOrdersOther.Where(x => x.PurchaseInvoices != null).Select(PO =>
            {
                double amountWithTax = PO.PurchaseInvoices.Where(pi => pi.Payments != null).SelectMany(pi => pi.Payments).Where(payment => payment.isVoid != true).Sum(payment => Convert.ToDouble(payment.DebitedAmount));
                double unPaidAmount = PO.billWithTax != null ? Convert.ToDouble(PO.billWithTax) - amountWithTax : Convert.ToDouble(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                double unPaidAmountCMER = unPaidAmount * GetCMER((int)PO.company.CurrencyId, (int)PO.currency_Id, (int)PO.company_Id);
                return unPaidAmountCMER;
            }).Sum();
            txtTotalAPSuppliersGBP.Text = totalUnPaidPOAmountGBP.ToString();
            txtTotalAPSuppliersUSD.Text = totalUnPaidPOAmountUSD.ToString();
            txtTotalAPSuppliersEUR.Text = totalUnPaidPOAmountEUR.ToString();
            txtTotalAPSuppliersAED.Text = totalUnPaidPOAmountAED.ToString();
            txtTotalAPSuppliersOMR.Text = totalUnPaidPOAmountOMR.ToString();
            txtTotalAPSuppliersPKR.Text = totalUnPaidPOAmountPKR.ToString();
            txtTotalAPSuppliersOther.Text = totalUnPaidPOAmountOther.ToString();
            txtTotalAPSuppliersGBPCMER.Text = totalUnPaidPOAmountGBPCMER.ToString();
            txtTotalAPSuppliersUSDCMER.Text = totalUnPaidPOAmountUSDCMER.ToString();
            txtTotalAPSuppliersEURCMER.Text = totalUnPaidPOAmountEURCMER.ToString();
            txtTotalAPSuppliersAEDCMER.Text = totalUnPaidPOAmountAEDCMER.ToString();
            txtTotalAPSuppliersOMRCMER.Text = totalUnPaidPOAmountOMRCMER.ToString();
            txtTotalAPSuppliersPKRCMER.Text = totalUnPaidPOAmountPKRCMER.ToString();
            txtTotalAPSuppliersOtherCMER.Text = totalUnPaidPOAmountOtherCMER.ToString();
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }



        public void CalculatePOAssetSummary()
        {
            //GBP
            advancePaidAmountGBP = Math.Round(purchaseOrdersGBP.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount))), 2);
            txtTotalAdvancePaidGBP.Text = advancePaidAmountGBP.ToString();
            advancePaidAmountGBPCMER = Math.Round(purchaseOrdersGBP.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)))), 2);
            txtTotalAdvancePaidGBPCMER.Text = advancePaidAmountGBPCMER.ToString();
            advancePaidAmountSystemGBP = Math.Round(purchaseOrdersGBP.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance));
            txtTotalAdvancePaidSystemGBP.Text = advancePaidAmountSystemGBP.ToString();
            advancePaidAmountSystemGBPCMER = Math.Round(purchaseOrdersGBP.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance * GetCMER((int)y.company.CurrencyId, (int)y.currency_Id, (int)y.company_Id)), 2);
            txtTotalAdvancePaidSystemGBPCMER.Text = advancePaidAmountSystemGBPCMER.ToString();
            //USD
            advancePaidAmountUSD = Math.Round(purchaseOrdersUSD.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount))), 2);
            txtTotalAdvancePaidUSD.Text = advancePaidAmountUSD.ToString();
            advancePaidAmountUSDCMER = Math.Round(purchaseOrdersUSD.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)))), 2);
            txtTotalAdvancePaidUSDCMER.Text = advancePaidAmountUSDCMER.ToString();
            advancePaidAmountSystemUSD = Math.Round(purchaseOrdersUSD.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance));
            txtTotalAdvancePaidSystemUSD.Text = advancePaidAmountSystemUSD.ToString();
            advancePaidAmountSystemUSDCMER = Math.Round(purchaseOrdersUSD.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance * GetCMER((int)y.company.CurrencyId, (int)y.currency_Id, (int)y.company_Id)), 2);
            txtTotalAdvancePaidSystemUSDCMER.Text = advancePaidAmountSystemUSDCMER.ToString();
            //EURO
            advancePaidAmountEUR = Math.Round(purchaseOrdersEUR.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount))), 2);
            txtTotalAdvancePaidEUR.Text = advancePaidAmountEUR.ToString();
            advancePaidAmountEURCMER = Math.Round(purchaseOrdersEUR.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)))), 2);
            txtTotalAdvancePaidEURCMER.Text = advancePaidAmountEURCMER.ToString();
            advancePaidAmountSystemEUR = Math.Round(purchaseOrdersEUR.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance), 2);
            txtTotalAdvancePaidSystemEUR.Text = advancePaidAmountSystemEUR.ToString();
            advancePaidAmountSystemEURCMER = Math.Round(purchaseOrdersEUR.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance * GetCMER((int)y.company.CurrencyId, (int)y.currency_Id, (int)y.company_Id)), 2);
            txtTotalAdvancePaidSystemEURCMER.Text = advancePaidAmountSystemEURCMER.ToString();
            //AED
            advancePaidAmountAED = Math.Round(purchaseOrdersAED.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount))), 2);
            txtTotalAdvancePaidAED.Text = advancePaidAmountAED.ToString();
            advancePaidAmountAEDCMER = Math.Round(purchaseOrdersAED.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)))), 2);
            txtTotalAdvancePaidAEDCMER.Text = advancePaidAmountAEDCMER.ToString();
            advancePaidAmountSystemAED = Math.Round(purchaseOrdersAED.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance), 2);
            txtTotalAdvancePaidSystemAED.Text = advancePaidAmountSystemAED.ToString();
            advancePaidAmountSystemAEDCMER = Math.Round(purchaseOrdersAED.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance * GetCMER((int)y.company.CurrencyId, (int)y.currency_Id, (int)y.company_Id)), 2);
            txtTotalAdvancePaidSystemAEDCMER.Text = advancePaidAmountSystemAEDCMER.ToString();
            //OMR
            advancePaidAmountOMR = Math.Round(purchaseOrdersOMR.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount))), 2);
            txtTotalAdvancePaidOMR.Text = advancePaidAmountOMR.ToString();
            advancePaidAmountOMRCMER = Math.Round(purchaseOrdersOMR.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)))), 2);
            txtTotalAdvancePaidOMRCMER.Text = advancePaidAmountOMRCMER.ToString();
            advancePaidAmountSystemOMR = Math.Round(purchaseOrdersOMR.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance), 2);
            txtTotalAdvancePaidSystemOMR.Text = advancePaidAmountSystemOMR.ToString();
            advancePaidAmountSystemOMRCMER = Math.Round(purchaseOrdersOMR.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance * GetCMER((int)y.company.CurrencyId, (int)y.currency_Id, (int)y.company_Id)), 2);
            txtTotalAdvancePaidSystemOMRCMER.Text = advancePaidAmountSystemOMRCMER.ToString();
            //PKR
            advancePaidAmountPKR = Math.Round(purchaseOrdersPKR.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount))), 2);
            txtTotalAdvancePaidPKR.Text = advancePaidAmountPKR.ToString();
            advancePaidAmountPKRCMER = Math.Round(purchaseOrdersPKR.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)))), 2);
            txtTotalAdvancePaidPKRCMER.Text = advancePaidAmountPKRCMER.ToString();
            advancePaidAmountSystemPKR = Math.Round(purchaseOrdersPKR.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance), 2);
            txtTotalAdvancePaidSystemPKR.Text = advancePaidAmountSystemPKR.ToString();
            advancePaidAmountSystemPKRCMER = Math.Round(purchaseOrdersPKR.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance * GetCMER((int)y.company.CurrencyId, (int)y.currency_Id, (int)y.company_Id)), 2);
            txtTotalAdvancePaidSystemPKRCMER.Text = advancePaidAmountSystemPKRCMER.ToString();
            //Other
            advancePaidAmountOther = Math.Round(purchaseOrdersOther.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount))), 2);
            txtTotalAdvancePaidOther.Text = advancePaidAmountOther.ToString();
            advancePaidAmountOtherCMER = Math.Round(purchaseOrdersOther.Sum(x => x.PurchaseInvoices.Sum(y => y.Payments.Where(z => z.isVoid != true).Sum(z => z.DebitedAmount * GetCMER((int)x.company.CurrencyId, (int)x.currency_Id, (int)x.company_Id)))), 2);
            txtTotalAdvancePaidOtherCMER.Text = advancePaidAmountOtherCMER.ToString();
            advancePaidAmountSystemOther = Math.Round(purchaseOrdersOther.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance), 2);
            txtTotalAdvancePaidSystemOther.Text = advancePaidAmountSystemOther.ToString();
            advancePaidAmountSystemOtherCMER = Math.Round(purchaseOrdersOther.Where(x => x.isVoid != true).Sum(y => y.totalPOAdvance * GetCMER((int)y.company.CurrencyId, (int)y.currency_Id, (int)y.company_Id)), 2);
            txtTotalAdvancePaidSystemOtherCMER.Text = advancePaidAmountSystemOtherCMER.ToString();
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }
        public void calculateTotalAssets()
        {
            txtTotalAssetsGBP.Text = Math.Round(totalAssetsGBP, 2).ToString();
            txtTotalAssetsEUR.Text = Math.Round(totalAssetsEUR, 2).ToString();
            txtTotalAssetsUSD.Text = Math.Round(totalAssetsUSD, 2).ToString();
            txtTotalAssetsAED.Text = Math.Round(totalAssetsAED, 2).ToString();
            txtTotalAssetsOMR.Text = Math.Round(totalAssetsOMR, 2).ToString();
            txtTotalAssetsPKR.Text = Math.Round(totalAssetsPKR, 2).ToString();
            txtTotalAssetsOther.Text = Math.Round(totalAssetsOther, 2).ToString();
            txtTotalAssetsGBPCMER.Text = Math.Round(totalAssetsGBPCMER, 2).ToString();
            txtTotalAssetsEURCMER.Text = Math.Round(totalAssetsEURCMER, 2).ToString();
            txtTotalAssetsUSDCMER.Text = Math.Round(totalAssetsUSDCMER, 2).ToString();
            txtTotalAssetsAEDCMER.Text = Math.Round(totalAssetsAEDCMER, 2).ToString();
            txtTotalAssetsOMRCMER.Text = Math.Round(totalAssetsOMRCMER, 2).ToString();
            txtTotalAssetsPKRCMER.Text = Math.Round(totalAssetsPKRCMER, 2).ToString();
            txtTotalAssetsOtherCMER.Text = Math.Round(totalAssetsOtherCMER, 2).ToString();
            txtTotalAssetsCMER.Text = Math.Round(totalAssetsCMER, 2).ToString();
            CalculateTotalNetWorth();
        }
        public void calculateTotalLiabilities()
        {
            txtTotalLiabilitiesGBP.Text = Math.Round(totalLiabilitiesGBP, 2).ToString();
            txtTotalLiabilitiesEUR.Text = Math.Round(totalLiabilitiesEUR, 2).ToString();
            txtTotalLiabilitiesUSD.Text = Math.Round(totalLiabilitiesUSD, 2).ToString();
            txtTotalLiabilitiesAED.Text = Math.Round(totalLiabilitiesAED, 2).ToString();
            txtTotalLiabilitiesOMR.Text = Math.Round(totalLiabilitiesOMR, 2).ToString();
            txtTotalLiabilitiesPKR.Text = Math.Round(totalLiabilitiesPKR, 2).ToString();
            txtTotalLiabilitiesOther.Text = Math.Round(totalLiabilitiesOther, 2).ToString();
            txtTotalLiabilitiesGBPCMER.Text = Math.Round(totalLiabilitiesGBPCMER, 2).ToString();
            txtTotalLiabilitiesEURCMER.Text = Math.Round(totalLiabilitiesEURCMER, 2).ToString();
            txtTotalLiabilitiesUSDCMER.Text = Math.Round(totalLiabilitiesUSDCMER, 2).ToString();
            txtTotalLiabilitiesAEDCMER.Text = Math.Round(totalLiabilitiesAEDCMER, 2).ToString();
            txtTotalLiabilitiesOMRCMER.Text = Math.Round(totalLiabilitiesOMRCMER, 2).ToString();
            txtTotalLiabilitiesPKRCMER.Text = Math.Round(totalLiabilitiesPKRCMER, 2).ToString();
            txtTotalLiabilitiesOtherCMER.Text = Math.Round(totalLiabilitiesOtherCMER, 2).ToString();
            txtTotalLiabilitiesCMER.Text = Math.Round(totalLiabilitiesCMER, 2).ToString();
            CalculateTotalNetWorth();

        }
        public void calculateTotalInventories()
        {
            //txtInventoryGBP.Text = Math.Round(totalInventoryGBP, 2).ToString();
            //txtInventoryEUR.Text = Math.Round(totalInventoryEUR, 2).ToString();
            //txtInventoryUSD.Text = Math.Round(totalInventoryUSD, 2).ToString();
            //txtInventoryAED.Text = Math.Round(totalInventoryAED, 2).ToString();
            //txtInventoryOMR.Text = Math.Round(totalInventoryOMR, 2).ToString();
            //txtInventoryPKR.Text = Math.Round(totalInventoryPKR, 2).ToString();
            //txtInventoryOther.Text = Math.Round(totalInventoryOther, 2).ToString();
            txtInventoryGBPCMER.Text = Math.Round(totalInventoryGBPCMER, 2).ToString();
            txtInventoryEURCMER.Text = Math.Round(totalInventoryEURCMER, 2).ToString();
            txtInventoryUSDCMER.Text = Math.Round(totalInventoryUSDCMER, 2).ToString();
            txtInventoryAEDCMER.Text = Math.Round(totalInventoryAEDCMER, 2).ToString();
            txtInventoryOMRCMER.Text = Math.Round(totalInventoryOMRCMER, 2).ToString();
            txtInventoryPKRCMER.Text = Math.Round(totalInventoryPKRCMER, 2).ToString();
            txtInventoryOtherCMER.Text = Math.Round(totalInventoryOtherCMER, 2).ToString();
            txtInventoryCMER.Text = Math.Round(totalInventoryCMER, 2).ToString();
            CalculateTotalNetWorth();

        }
        public void CalculateTotalNetWorth()
        {
            txtTotalNetWorthGBP.Text = (totalAssetsGBP - totalLiabilitiesGBP).ToString();
            txtTotalNetWorthGBPCMER.Text = (totalAssetsGBPCMER - totalLiabilitiesGBPCMER).ToString();
            txtTotalNetWorthEUR.Text = (totalAssetsEUR - totalLiabilitiesEUR).ToString();
            txtTotalNetWorthEURCMER.Text = (totalAssetsEURCMER - totalLiabilitiesEURCMER).ToString();
            txtTotalNetWorthUSD.Text = (totalAssetsUSD - totalLiabilitiesUSD).ToString();
            txtTotalNetWorthUSDCMER.Text = (totalAssetsUSDCMER - totalLiabilitiesUSDCMER).ToString();
            txtTotalNetWorthAED.Text = (totalAssetsAED - totalLiabilitiesAED).ToString();
            txtTotalNetWorthAEDCMER.Text = (totalAssetsAEDCMER - totalLiabilitiesAEDCMER).ToString();
            txtTotalNetWorthOMR.Text = (totalAssetsOMR - totalLiabilitiesOMR).ToString();
            txtTotalNetWorthOMRCMER.Text = (totalAssetsOMRCMER - totalLiabilitiesOMRCMER).ToString();
            txtTotalNetWorthPKR.Text = (totalAssetsPKR - totalLiabilitiesPKR).ToString();
            txtTotalNetWorthPKRCMER.Text = (totalAssetsPKRCMER - totalLiabilitiesPKRCMER).ToString();
            txtTotalNetWorthOther.Text = (totalAssetsOther - totalLiabilitiesOther).ToString();
            txtTotalNetWorthOtherCMER.Text = (totalAssetsOtherCMER - totalLiabilitiesOtherCMER).ToString();
            txtTotalNetWorthCMER.Text = (totalAssetsCMER - totalLiabilitiesCMER).ToString();

        }
        private void btnSTLRemarks_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL Remarks") != null)
            {
                if (grdSTLCashFlow.SelectedItem != null && !string.IsNullOrEmpty(txtSTLRemarks.Text) && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") != null)
                {
                    var stl = grdSTLCashFlow.SelectedItem as STL;
                    stl.stlRemarks = txtSTLRemarks.Text;
                    stlRepo.update(stl);
                    DevExpress.Xpf.Core.DXMessageBox.Show("STL Remarks added successfully!");
                    txtSTLRemarks.Text = "";
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(" You don't have permission to Add STL Remarks");

            }
        }

        private void btnLoadRefreshBank_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplyBankFilters();
            CalculateBankSummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void CalculateBankSummary()
        {
            //GBP
            totalBankBalanceSGBP = chartofAccountsGBP.Sum(x => x.JournalTransactions.Where(
                   y => y.AdminBill?.isVoid != true
                   && y.Bill?.isVoid != true
                   && y.PurchaseInvoice?.isVoid != true
                   && y.SaleInvoice?.isVoid != true
                   && y.SalesReceipt?.isVoid != true
                   && y.journalVoucher?.isVoid != true
                   && y.Payment?.isVoid != true
                   && y.InterBank?.isVoid != true
                   && y.InterCompanyTransfer?.isVoid != true &&
                   y.creationDate != null &&
                   y.deptId != null &&
                   deptIds.Contains((int)y.deptId) &&
                   y.companyId != null &&
                   companyIds.Contains((int)y.companyId)
                 ).Sum(y => y.total));

            //GBPCMER
            totalBankBalanceSGBPCMER = chartofAccountsGBP.Sum(x => x.JournalTransactions.Where(
                   y => y.AdminBill?.isVoid != true
                   && y.Bill?.isVoid != true
                   && y.PurchaseInvoice?.isVoid != true
                   && y.SaleInvoice?.isVoid != true
                   && y.SalesReceipt?.isVoid != true
                   && y.journalVoucher?.isVoid != true
                   && y.Payment?.isVoid != true
                   && y.InterBank?.isVoid != true
                   && y.InterCompanyTransfer?.isVoid != true &&
                   y.creationDate != null &&
                   y.deptId != null &&
                   deptIds.Contains((int)y.deptId) &&
                   y.companyId != null &&
                   companyIds.Contains((int)y.companyId)
                 ).Sum(y => y.total * y.MER));


            //USD
            totalBankBalanceSUSD = chartofAccountsUSD.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total));

            //USDCMER
            totalBankBalanceSUSDCMER = chartofAccountsUSD.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total * y.MER));
            //EURO
            totalBankBalanceSEUR = chartofAccountsEUR.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total));

            //EUROCMER
            totalBankBalanceSEURCMER = chartofAccountsEUR.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total * y.MER));
            //AED
            totalBankBalanceSAED = chartofAccountsAED.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total));
            //AEDCMER
            totalBankBalanceSAEDCMER = chartofAccountsAED.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total * y.MER));
            //OMR
            totalBankBalanceSOMR = chartofAccountsOMR.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total));
            //OMERCMER
            totalBankBalanceSOMRCMER = chartofAccountsOMR.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total * y.MER));
            //PKR
            totalBankBalanceSPKR = chartofAccountsPKR.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total));
            //PKRCMER
            totalBankBalanceSPKRCMER = chartofAccountsPKR.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total * y.MER));
            //Other
            totalBankBalanceSOther = chartofAccountsOther.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total));

            //OtherCMER
            totalBankBalanceSOtherCMER = chartofAccountsOther.Sum(x => x.JournalTransactions.Where(
                      y => y.AdminBill?.isVoid != true
                      && y.Bill?.isVoid != true
                      && y.PurchaseInvoice?.isVoid != true
                      && y.SaleInvoice?.isVoid != true
                      && y.SalesReceipt?.isVoid != true
                      && y.journalVoucher?.isVoid != true
                      && y.Payment?.isVoid != true
                      && y.InterBank?.isVoid != true
                      && y.InterCompanyTransfer?.isVoid != true &&
                      y.creationDate != null &&
                      y.deptId != null &&
                      deptIds.Contains((int)y.deptId) &&
                      y.companyId != null &&
                      companyIds.Contains((int)y.companyId)
                    ).Sum(y => y.total * y.MER));


            txtTotalBankBalanceSGBP.Text = totalBankBalanceSGBP.ToString();
            txtTotalBankBalanceSEUR.Text = totalBankBalanceSEUR.ToString();
            txtTotalBankBalanceSAED.Text = totalBankBalanceSAED.ToString();
            txtTotalBankBalanceSOMR.Text = totalBankBalanceSOMR.ToString();
            txtTotalBankBalanceSUSD.Text = totalBankBalanceSUSD.ToString();
            txtTotalBankBalanceSPKR.Text = totalBankBalanceSPKR.ToString();
            txtTotalBankBalanceSOther.Text = totalBankBalanceSOther.ToString();
            txtTotalBankBalanceSGBPCMER.Text = totalBankBalanceSGBPCMER.ToString();
            txtTotalBankBalanceSEURCMER.Text = totalBankBalanceSEURCMER.ToString();
            txtTotalBankBalanceSAEDCMER.Text = totalBankBalanceSAEDCMER.ToString();
            txtTotalBankBalanceSOMRCMER.Text = totalBankBalanceSOMRCMER.ToString();
            txtTotalBankBalanceSUSDCMER.Text = totalBankBalanceSUSDCMER.ToString();
            txtTotalBankBalanceSPKRCMER.Text = totalBankBalanceSPKRCMER.ToString();
            txtTotalBankBalanceSOtherCMER.Text = totalBankBalanceSOtherCMER.ToString();

            totalBankBalanceMGBP = chartofAccountsGBP.Sum(x => x.manualBalanceOC);

            //GBPCMER
            totalBankBalanceMGBPCMER = chartofAccountsGBP.Sum(x => x.manualBalancePKR);


            //USD
            totalBankBalanceMUSD = chartofAccountsUSD.Sum(x => x.manualBalanceOC);

            //USDCMER
            totalBankBalanceMUSDCMER = chartofAccountsUSD.Sum(x => x.manualBalancePKR);
            //EURO
            totalBankBalanceMEUR = chartofAccountsEUR.Sum(x => x.manualBalanceOC);

            //EUROCMER
            totalBankBalanceMEURCMER = chartofAccountsEUR.Sum(x => x.manualBalancePKR);
            //AED
            totalBankBalanceMAED = chartofAccountsAED.Sum(x => x.manualBalanceOC);
            //AEDCMER
            totalBankBalanceMAEDCMER = chartofAccountsAED.Sum(x => x.manualBalancePKR);
            //OMR
            totalBankBalanceMOMR = chartofAccountsOMR.Sum(x => x.manualBalanceOC);
            //OMERCMER
            totalBankBalanceMOMRCMER = chartofAccountsOMR.Sum(x => x.manualBalancePKR);
            //PKR
            totalBankBalanceMPKR = chartofAccountsPKR.Sum(x => x.manualBalanceOC);
            //PKRCMER
            totalBankBalanceMPKRCMER = chartofAccountsPKR.Sum(x => x.manualBalancePKR);
            //Other
            totalBankBalanceMOther = chartofAccountsOther.Sum(x => x.manualBalanceOC);

            //OtherCMER
            totalBankBalanceMOtherCMER = chartofAccountsOther.Sum(x => x.manualBalancePKR);


            txtTotalBankBalanceMGBP.Text = totalBankBalanceMGBP.ToString();
            txtTotalBankBalanceMEUR.Text = totalBankBalanceMEUR.ToString();
            txtTotalBankBalanceMAED.Text = totalBankBalanceMAED.ToString();
            txtTotalBankBalanceMOMR.Text = totalBankBalanceMOMR.ToString();
            txtTotalBankBalanceMUSD.Text = totalBankBalanceMUSD.ToString();
            txtTotalBankBalanceMPKR.Text = totalBankBalanceMPKR.ToString();
            txtTotalBankBalanceMOther.Text = totalBankBalanceMOther.ToString();



            txtTotalBankBalanceMGBPCMER.Text = totalBankBalanceMGBPCMER.ToString();
            txtTotalBankBalanceMEURCMER.Text = totalBankBalanceMEURCMER.ToString();
            txtTotalBankBalanceMAEDCMER.Text = totalBankBalanceMAEDCMER.ToString();
            txtTotalBankBalanceMOMRCMER.Text = totalBankBalanceMOMRCMER.ToString();
            txtTotalBankBalanceMUSDCMER.Text = totalBankBalanceMUSDCMER.ToString();
            txtTotalBankBalanceMPKRCMER.Text = totalBankBalanceMPKRCMER.ToString();
            txtTotalBankBalanceMOtherCMER.Text = totalBankBalanceMOtherCMER.ToString();
            calculateTotalBankBalancesCMER();
        }
        public void calculateTotalBankBalancesCMER()
        {
            txtTotalBankBalanceSCMER.Text = (
                  totalBankBalanceSGBPCMER
                + totalBankBalanceSEURCMER
                + totalBankBalanceSAEDCMER
                + totalBankBalanceSOMRCMER
                + totalBankBalanceSUSDCMER
                + totalBankBalanceSPKRCMER
                + totalBankBalanceSOtherCMER).ToString();
            txtTotalBankBalanceMCMER.Text = (
                  totalBankBalanceMGBPCMER
                + totalBankBalanceMEURCMER
                + totalBankBalanceMAEDCMER
                + totalBankBalanceMOMRCMER
                + totalBankBalanceMUSDCMER
                + totalBankBalanceMPKRCMER
                + totalBankBalanceMOtherCMER).ToString();
        }
      

        private void isBankCurrency_Checked(object sender, RoutedEventArgs e)
        {
            cmbBankCurrency.IsEnabled = true;

        }

        private void isBankCurrency_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbBankCurrency.IsEnabled = false;
            cmbBankCurrency.SelectedItem = null;
        }

        private void btnSaveBankBalances_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Manual Bank Balances") != null)
            {
                if (grdCashFlowBankAccounts.SelectedItem != null)
                {
                    var chartofAccount = grdCashFlowBankAccounts.SelectedItem as ChartofAccount;
                    if (!string.IsNullOrEmpty(txtBankBalanceOC.Text))
                        chartofAccount.manualBalanceOC = Convert.ToDouble(txtBankBalanceOC.Text);
                    if (!string.IsNullOrEmpty(txtBankBalancePKR.Text))
                        chartofAccount.manualBalancePKR = Convert.ToDouble(txtBankBalancePKR.Text);
                    chartofAccountRepo.UpdateAccountForCashFlow(chartofAccount);
                    DevExpress.Xpf.Core.DXMessageBox.Show("Chart of Account updated successfully!");
                    txtBankBalanceOC.Text = "";
                    txtBankBalancePKR.Text = "";
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(" You don't have permission to Add Manual Bank Balances");

            }
        }

        public void calculateTotalCMERAssets()
        {
            txtTotalReceivableCMER.Text = (totalRemainigCollectionOCGBPCMER + totalRemainigCollectionOCEURCMER + totalRemainigCollectionOCUSDCMER + totalRemainigCollectionOCOMRCMER + totalRemainigCollectionOCAEDCMER + totalRemainigCollectionOCPKRCMER + totalRemainigCollectionOCOtherCMER).ToString();
            txtTotalAdvancePaidCMER.Text = (advancePaidAmountGBPCMER + advancePaidAmountEURCMER + advancePaidAmountUSDCMER + advancePaidAmountAEDCMER + advancePaidAmountOMRCMER + advancePaidAmountPKRCMER + advancePaidAmountOtherCMER).ToString();
            txtTotalAdvancePaidSystemCMER.Text = (advancePaidAmountSystemGBPCMER + advancePaidAmountSystemEURCMER + advancePaidAmountSystemUSDCMER + advancePaidAmountSystemAEDCMER + advancePaidAmountSystemOMRCMER + advancePaidAmountSystemPKRCMER + advancePaidAmountSystemOtherCMER).ToString();
            txtTotalSTLCMER.Text = (totalSTLAmountGBPCMER + totalSTLAmountEURCMER + totalSTLAmountUSDCMER + totalSTLAmountAEDCMER + totalSTLAmountOMRCMER + totalSTLAmountPKRCMER + totalSTLAmountOtherCMER).ToString();
            txtTotalSOReceivableCMER.Text = (totalSaleOrdersGBPCMER + totalSaleOrdersEURCMER + totalSaleOrdersUSDCMER + totalSaleOrdersAEDCMER + totalSaleOrdersOMRCMER + totalSaleOrdersPKRCMER + totalSaleOrdersOtherCMER).ToString();
            txtAdvancesCMER.Text = (totalAdvanceGBPCMER + totalAdvanceEURCMER + totalAdvanceUSDCMER + totalAdvanceAEDCMER + totalAdvanceOMRCMER + totalAdvancePKRCMER + totalAdvanceOtherCMER + totalAdvanceOtherCMER).ToString();
            txtInventoryCMER.Text = (totalInventoryGBPCMER + totalInventoryEURCMER + totalInventoryUSDCMER + totalInventoryAEDCMER + totalInventoryOMRCMER + totalInventoryPKRCMER + totalInventoryOtherCMER + totalInventoryOtherCMER).ToString();
        }
        public void calculateTotalCMERLiabilities()
        {
            txtTotalAPSuppliersCMER.Text = (totalUnPaidPOAmountGBPCMER + totalUnPaidPOAmountEURCMER + totalUnPaidPOAmountUSDCMER + totalUnPaidPOAmountAEDCMER + totalUnPaidPOAmountOMRCMER + totalUnPaidPOAmountPKRCMER + totalUnPaidPOAmountOtherCMER).ToString();
            txtTotalSTLCMER.Text = (totalSTLAmountGBPCMER + totalSTLAmountEURCMER + totalSTLAmountUSDCMER + totalSTLAmountAEDCMER + totalSTLAmountOMRCMER + totalSTLAmountPKRCMER + totalSTLAmountOtherCMER).ToString();
            txtTotalAPbillsCMER.Text = (totalUnPaidBillAmountGBPCMER + totalUnPaidBillAmountEURCMER + totalUnPaidBillAmountUSDCMER + totalUnPaidBillAmountAEDCMER + totalUnPaidBillAmountOMRCMER + totalUnPaidBillAmountPKRCMER + totalUnPaidBillAmountOtherCMER).ToString();
            txtTotalAPPaymentsCMER.Text = (totalPaymentGBPCMER + totalPaymentEURCMER + totalPaymentUSDCMER + totalPaymentAEDCMER + totalPaymentOMRCMER + totalPaymentPKRCMER + totalPaymentOtherCMER).ToString();
            txtTotalAPSOCMER.Text = (totalSaleOrdersAPGBPCMER + totalSaleOrdersAPEURCMER + totalSaleOrdersAPUSDCMER + totalSaleOrdersAPAEDCMER + totalSaleOrdersAPOMRCMER + totalSaleOrdersAPPKRCMER + totalSaleOrdersAPOtherCMER).ToString();
            txtAdminBillsCMER.Text = (totalAdminBillGBPCMER + totalAdminBillEURCMER + totalAdminBillUSDCMER + totalAdminBillAEDCMER + totalAdminBillOMRCMER + totalAdminBillPKRCMER + totalAdminBillOtherCMER).ToString();
            txtLoansCMER.Text = (totalLoanGBPCMER + totalLoanEURCMER + totalLoanUSDCMER + totalLoanAEDCMER + totalLoanOMRCMER + totalLoanPKRCMER + totalLoanOtherCMER).ToString();
        }
     
        private void grdCashFlowAdminBillsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateBillWindow();
        }
        private void grdCashFlowAdminBillsList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "PaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    var _bill = billRepo.getForGrid(id);
                    var paidAmount = Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
                    e.Value = paidAmount;
                }
            }
            if (e.Column.FieldName == "UnpaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    var _bill = billRepo.getForGrid(id);
                    var paidAmount = Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
                    decimal amountWithTax = 0;
                    if (_bill.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                    {
                        if (_bill.billWithTax != null)
                        {
                            amountWithTax = Math.Round(Convert.ToDecimal(_bill.billWithTax.Value) - paidAmount, 2);
                        }
                        else
                        {
                            amountWithTax = Convert.ToDecimal(_bill.totalCFRValue) - paidAmount;
                        }
                    }
                    else
                    {
                        if (_bill.billWithTax != null)
                        {
                            amountWithTax = Convert.ToDecimal(_bill.billWithTax.Value);
                        }
                        else
                        {
                            amountWithTax = Convert.ToDecimal(_bill.totalCFRValue);
                        }
                    }
                    e.Value = amountWithTax;
                }
            }
            if (e.Column.FieldName == "Stage" && e.IsGetData)
            {
                var bill = grdCashFlowbill.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.Bill;
                if (bill.isVoid == true)
                {
                    e.Value = "Void";
                }
                else if (bill.isApproved == true && bill.stage == "Closed")
                {
                    e.Value = "Closed";
                }
                else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
                {
                    e.Value = "Closed";
                }
                else if (bill.isApproved == true && bill.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
                else if (bill.isApproved == true)
                {
                    e.Value = "Approved";
                }
                else if (bill.isApproved == false)
                {
                    e.Value = "Under Approval";
                }
                else if (bill.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
            }
        }
        private void btnPOLoadRefresh_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplyPOFilters();
            CalculatePOAssetSummary();
            CalculatePOLiabilitySummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void ApplyPOFilters()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            purchaseOrders = purchaseOrderRepo.getCashFlowAllActive(currentUserId);
            if (selectedCompanies.Any())
            {
                var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToList();
                purchaseOrders = purchaseOrders.Where(x => selectedCompanyIds.Contains((int)x.company_Id)).ToList();
            }

            // Check if there are selected departments and apply filter
            if (selectedDepartments.Any())
            {
                var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToList();
                purchaseOrders = purchaseOrders.Where(x => selectedDepartmentIds.Contains(x.dept_Id)).ToList();
            }
            if (cmbPOCurrency.SelectedIndex > -1)
            {
                purchaseOrders = purchaseOrders.Where(x => x.currency_Id == (cmbPOCurrency.SelectedItem as cmbitem).id).ToList();
            }
            if (!string.IsNullOrEmpty(txtPODaysLeft.Text) && Convert.ToInt32(txtPODaysLeft.Text) != 0)
            {

                int value = 0;
                value = Convert.ToInt32(txtPODaysLeft.Text);

                if (Convert.ToInt32(txtPODaysLeft.Text) < 0)
                {
                    value = Convert.ToInt32(txtPODaysLeft.Text);
                    value = Math.Abs(value);
                }
                else
                {
                    value = Convert.ToInt32(txtPODaysLeft.Text);
                    value = -value;
                }
                DateTime dateTime = DateTime.Now.AddDays(value);
                if (dateTime.Date < DateTime.Now)
                {
                    purchaseOrders = purchaseOrders.Where(x => x.ExpectedPayment >= dateTime).ToList();
                    purchaseOrders = purchaseOrders.Where(x => x.ExpectedPayment <= DateTime.Now).ToList();
                }
                else
                {
                    purchaseOrders = purchaseOrders.Where(x => x.ExpectedPayment <= dateTime && x.ExpectedPayment >= DateTime.Now).ToList();
                }

            }
            purchaseOrdersOther = purchaseOrders.Where(x => x.currency_Id != 1 && x.currency_Id != 2 && x.currency_Id != 3 && x.currency_Id != 4 && x.currency_Id != 5 && x.currency_Id != 7).ToList();
            purchaseOrdersOMR = purchaseOrders.Where(x => x.currency_Id == 7).ToList();
            purchaseOrdersAED = purchaseOrders.Where(x => x.currency_Id == 5).ToList();
            purchaseOrdersPKR = purchaseOrders.Where(x => x.currency_Id == 4).ToList();
            purchaseOrdersGBP = purchaseOrders.Where(x => x.currency_Id == 3).ToList();
            purchaseOrdersUSD = purchaseOrders.Where(x => x.currency_Id == 2).ToList();
            purchaseOrdersEUR = purchaseOrders.Where(x => x.currency_Id == 1).ToList();
            grdCashFlowPurchaseOrder.ItemsSource = purchaseOrders;
        }
       

      






        private void isSICurrency_Checked(object sender, RoutedEventArgs e)
        {
            cmbSICurrency.IsEnabled = true;


        }

        private void isSICurrency_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbSICurrency.IsEnabled = false;
            cmbSICurrency.SelectedItem = null;
        }

        private void isPOCurrency_Checked(object sender, RoutedEventArgs e)
        {
            cmbPOCurrency.IsEnabled = true;

        }

        private void isPOCurrency_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbPOCurrency.IsEnabled = false;
            cmbPOCurrency.SelectedItem = null;
        }
        private void grdPaymentCashFlow_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Depts" && e.IsGetData)
            {
                var pymnt = grdCashFlowPayment.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string deptNames = "";

                if (pymnt.departments != null && pymnt.departments.Count > 0)
                {
                    deptNames = String.Join(" | ", pymnt.departments.Select(x => x.DeptName));
                }
                e.Value = deptNames;
            }
            if (e.Column.FieldName == "Vendor1" && e.IsGetData)
            {
                var pymnt = grdCashFlowPayment.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string vendor = "";

                if (pymnt.vendor != null && pymnt.vendor.company != null)
                    vendor = pymnt.vendor.company.CompanyName;
                else
                {
                    if (pymnt.transactionType == PaymentTransactionType.Admin_Bills && pymnt.adminBill != null && pymnt.adminBill.vendor != null)
                        vendor = pymnt.adminBill.vendor.company.CompanyName;
                    else if (pymnt.transactionType == PaymentTransactionType.Vendor_Bills && pymnt.Bill.vendor != null)
                        vendor = pymnt.Bill.vendor.company.CompanyName;
                    else if (pymnt.transactionType == PaymentTransactionType.Purchase_Invoice && pymnt.purchaseInvoice.PurchaseOrder != null && pymnt.purchaseInvoice.PurchaseOrder.vendors != null && pymnt.purchaseInvoice.PurchaseOrder.vendors.Count > 0)
                        vendor = pymnt.purchaseInvoice.PurchaseOrder.vendors[0].company.CompanyName;
                }
                e.Value = vendor;
            }
            if (e.Column.FieldName == "Customer" && e.IsGetData)
            {
                var pymnt = grdCashFlowPayment.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string customer = "";
                if (pymnt.transactionType == PaymentTransactionType.Vendor_Bills && pymnt.Bill.customerCompany != null)
                    customer = pymnt.Bill.customerCompany.company.CompanyName;
                else if (pymnt.transactionType == PaymentTransactionType.Purchase_Invoice && pymnt.purchaseInvoice.PurchaseOrder != null && pymnt.purchaseInvoice.PurchaseOrder.customerCompany != null)
                    customer = pymnt.purchaseInvoice.PurchaseOrder.customerCompany.company.CompanyName;


                e.Value = customer;
            }
        }

        private void grdPaymentCashFlow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                PaymentRepo paymentRepo = new PaymentRepo();

                var selectedPayment = grdCashFlowPayment.SelectedItem as Payment;

                if (selectedPayment != null)
                {
                    CompanyRepo compRepo = new CompanyRepo();
                    switch (selectedPayment.transactionType)
                    {
                        case PaymentTransactionType.Admin_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") != null)
                            {
                                ucFrmPayments frmPayments = new ucFrmPayments();


                                frmPayments = new ucFrmPayments();
                                var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;


                                if (pymnt.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = selectedPayment.transactionGroupId;
                                        frmPayments.frmPaymentWindow.Content = frmPayments;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPayments.frmPaymentWindow.Title = "Payments";
                                        frmPayments.frmPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPayments.editFlag = true;
                                    frmPayments.groupId = selectedPayment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Admin Bill Payments!");
                            }

                            break;

                        case PaymentTransactionType.Vendor_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") != null)
                            {
                                ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd();

                                //paymentRepo = new PaymentRepo();
                                //frmBillPayments.payments = paymentRepo.GetVendorBillPaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment != null)
                                {
                                    if (selectedPayment.Status.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                        {
                                            frmBillPayments.editFlag = true;
                                            frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                            frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;

                                            frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                            frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                            frmBillPayments.frmBillPaymentWindow.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        frmBillPayments.editFlag = true;
                                        frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                        frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;
                                        frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                        frmBillPayments.frmBillPaymentWindow.Show();
                                    }
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Vendor Bill Payment!");
                            }

                            break;

                        case PaymentTransactionType.Purchase_Invoice:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Payment") != null)
                            {
                                ucFrmPInvoicePaymentAdd frmPIpayment = new ucFrmPInvoicePaymentAdd();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPIpayment.editFlag = true;
                                        frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                        frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                        frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmPIpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPIpayment.editFlag = true;
                                    frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                    frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                    frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmPIpayment.frmPiPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                            }


                            break;
                        case PaymentTransactionType.Loans_Advances:

                            switch (selectedPayment.loansAdvance.advanceTemplate)
                            {
                                case LoansAdvanceTemplate.Loan:
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                    {
                                        ucFrmCompanyLoanPayment frmLApayment = new ucFrmCompanyLoanPayment();
                                        //paymentRepo = new PaymentRepo();
                                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                        if (selectedPayment.Status.isActive == false)
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                            {
                                                frmLApayment.editFlag = true;
                                                frmLApayment.groupId = selectedPayment.transactionGroupId;
                                                frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                                frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                                frmLApayment.frmPiPaymentWindow.Show();
                                            }
                                            else
                                            {
                                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            frmLApayment.editFlag = true;
                                            frmLApayment.groupId = selectedPayment.transactionGroupId;
                                            frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                            frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                            frmLApayment.frmPiPaymentWindow.Show();
                                        }

                                    }
                                    else
                                    {
                                        DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                                    }
                                    break;
                                default:
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                    {
                                        ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                                        //paymentRepo = new PaymentRepo();
                                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                        if (selectedPayment.Status.isActive == false)
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                            {
                                                frmLApayment.editFlag = true;
                                                frmLApayment.groupId = selectedPayment.transactionGroupId;
                                                frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                                frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                                frmLApayment.frmPiPaymentWindow.Show();
                                            }
                                            else
                                            {
                                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            frmLApayment.editFlag = true;
                                            frmLApayment.groupId = selectedPayment.transactionGroupId;
                                            frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                            frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                            frmLApayment.frmPiPaymentWindow.Show();
                                        }

                                    }
                                    else
                                    {
                                        DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                                    }
                                    break;
                            }



                            break;
                        case PaymentTransactionType.Target_Reward:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Rewards Payment") != null)
                            {
                                ucFrmTargetRewardPayment frmTRPayment = new ucFrmTargetRewardPayment();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRPayment.editFlag = true;
                                        frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                        frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                        frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRPayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRPayment.editFlag = true;
                                    frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                    frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                    frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRPayment.frmPiPaymentWindow.Show();
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

       

    

        private void isPaymentCurrency_Checked(object sender, RoutedEventArgs e)
        {
            cmbPaymentCurrency.IsEnabled = true;
        }

        private void isPaymentCurrency_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbPaymentCurrency.IsEnabled = false;
        }

       

        private void grdCashFlowPayment_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                PaymentRepo paymentRepo = new PaymentRepo();

                var selectedPayment = grdCashFlowPayment.SelectedItem as Payment;

                if (selectedPayment != null)
                {
                    CompanyRepo compRepo = new CompanyRepo();
                    switch (selectedPayment.transactionType)
                    {
                        case PaymentTransactionType.Admin_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") != null)
                            {
                                ucFrmPayments frmPayments = new ucFrmPayments();


                                frmPayments = new ucFrmPayments();
                                var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;


                                if (pymnt.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = selectedPayment.transactionGroupId;
                                        frmPayments.frmPaymentWindow.Content = frmPayments;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPayments.frmPaymentWindow.Title = "Payments";
                                        frmPayments.frmPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPayments.editFlag = true;
                                    frmPayments.groupId = selectedPayment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Admin Bill Payments!");
                            }

                            break;

                        case PaymentTransactionType.Vendor_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") != null)
                            {
                                ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd();

                                //paymentRepo = new PaymentRepo();
                                //frmBillPayments.payments = paymentRepo.GetVendorBillPaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment != null)
                                {
                                    if (selectedPayment.Status.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                        {
                                            frmBillPayments.editFlag = true;
                                            frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                            frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;

                                            frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                            frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                            frmBillPayments.frmBillPaymentWindow.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        frmBillPayments.editFlag = true;
                                        frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                        frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;
                                        frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                        frmBillPayments.frmBillPaymentWindow.Show();
                                    }
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Vendor Bill Payment!");
                            }

                            break;

                        case PaymentTransactionType.Purchase_Invoice:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Payment") != null)
                            {
                                ucFrmPInvoicePaymentAdd frmPIpayment = new ucFrmPInvoicePaymentAdd();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPIpayment.editFlag = true;
                                        frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                        frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                        frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmPIpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPIpayment.editFlag = true;
                                    frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                    frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                    frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmPIpayment.frmPiPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                            }


                            break;
                        case PaymentTransactionType.Loans_Advances:

                            switch (selectedPayment.loansAdvance.advanceTemplate)
                            {
                                case LoansAdvanceTemplate.Loan:
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                    {
                                        ucFrmCompanyLoanPayment frmLApayment = new ucFrmCompanyLoanPayment();
                                        //paymentRepo = new PaymentRepo();
                                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                        if (selectedPayment.Status.isActive == false)
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                            {
                                                frmLApayment.editFlag = true;
                                                frmLApayment.groupId = selectedPayment.transactionGroupId;
                                                frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                                frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                                frmLApayment.frmPiPaymentWindow.Show();
                                            }
                                            else
                                            {
                                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            frmLApayment.editFlag = true;
                                            frmLApayment.groupId = selectedPayment.transactionGroupId;
                                            frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                            frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                            frmLApayment.frmPiPaymentWindow.Show();
                                        }

                                    }
                                    else
                                    {
                                        DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                                    }
                                    break;
                                default:
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                    {
                                        ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                                        //paymentRepo = new PaymentRepo();
                                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                        if (selectedPayment.Status.isActive == false)
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                            {
                                                frmLApayment.editFlag = true;
                                                frmLApayment.groupId = selectedPayment.transactionGroupId;
                                                frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                                frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                                frmLApayment.frmPiPaymentWindow.Show();
                                            }
                                            else
                                            {
                                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            frmLApayment.editFlag = true;
                                            frmLApayment.groupId = selectedPayment.transactionGroupId;
                                            frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                            frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                            frmLApayment.frmPiPaymentWindow.Show();
                                        }

                                    }
                                    else
                                    {
                                        DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                                    }
                                    break;
                            }



                            break;
                        case PaymentTransactionType.Target_Reward:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Rewards Payment") != null)
                            {
                                ucFrmTargetRewardPayment frmTRPayment = new ucFrmTargetRewardPayment();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRPayment.editFlag = true;
                                        frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                        frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                        frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRPayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRPayment.editFlag = true;
                                    frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                    frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                    frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRPayment.frmPiPaymentWindow.Show();
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

      

        private void btnPaymentLoadRefresh_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplyPaymentFilters();
            CalculatePaymentSummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void ApplyPaymentFilters()
        {
            try
            {
                var currentUserId = SYSTEM_STATIC.currentUser.id;
                payments = paymentRepo.getCashFlowAllActive(currentUserId);
                if (selectedCompanies.Any())
                {
                    var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToList();
                    payments = payments.Where(x => selectedCompanyIds.Contains((int)x.company_Id)).ToList();
                }
                // Check if there are selected departments and apply filter
                if (selectedDepartments.Any())
                {
                    var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToList();
                    payments = payments.Where(x =>
                             (x.loansAdvance?.deptId != null && selectedDepartmentIds.Contains((int)x.loansAdvance.deptId))
                             || (x.adminBill?.dept_Id != null && selectedDepartmentIds.Contains((int)x.adminBill.dept_Id))
                             || (x.targetRewards?.toDoTask?.taskGroup?.Departments != null
                                 && selectedDepartmentIds.Intersect(x.targetRewards.toDoTask.taskGroup.Departments.Select(y => y.Id)).Any())
                             || (x.Bill?.dept_Id != null && selectedDepartmentIds.Contains(x.Bill.dept_Id))
                             || (x.Bill?.InterDepartment_Id != null && selectedDepartmentIds.Contains(x.Bill.InterDepartment_Id.Value))
                             || (x.purchaseInvoice?.dept_Id != null && selectedDepartmentIds.Contains(x.purchaseInvoice.dept_Id))
                             || (x.purchaseInvoice?.InterDepartment_Id != null && selectedDepartmentIds.Contains(x.purchaseInvoice.InterDepartment_Id.Value)))
                        .ToList();
                }
                if (cmbPaymentCurrency.SelectedIndex > -1)
                {
                    payments = payments.Where(x => x.currency_Id == (cmbPaymentCurrency.SelectedItem as cmbitem).id).ToList();
                }
                paymentsOther = payments.Where(x => x.currency_Id != 1 && x.currency_Id != 2 && x.currency_Id != 3 && x.currency_Id != 4 && x.currency_Id != 5 && x.currency_Id != 7).ToList();
                paymentsOMR = payments.Where(x => x.currency_Id == 7).ToList();
                paymentsAED = payments.Where(x => x.currency_Id == 5).ToList();
                paymentsPKR = payments.Where(x => x.currency_Id == 4).ToList();
                paymentsGBP = payments.Where(x => x.currency_Id == 3).ToList();
                paymentsUSD = payments.Where(x => x.currency_Id == 2).ToList();
                paymentsEUR = payments.Where(x => x.currency_Id == 1).ToList();
                grdCashFlowPayment.ItemsSource = payments;
            }
            catch (Exception ex)
            {

            }
        }

        private void grdCashFlowGLAccounts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedAccount = grdCashFlowBankAccounts.SelectedItem as ChartofAccount;
            if (selectedAccount != null)
            {
                var chartofAccount = chartofAccountRepo.get(selectedAccount.Id);
                winLedger ledger = new winLedger(chartofAccount);
                ledger.Title = "Transactions";
                ledger.lblHeading.Text = "All Transactions by Account:" + " " + selectedAccount.accountName;
                ledger.Show();
            }
        }

        private void grdCashFlowAccountsTree_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            var chartofAccount = e.Node.Content as ChartofAccount;
            if (e.Column.FieldName == "balanceOC")
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
                e.Value = e.Value = Math.Round(value, 2);



            }
            if (e.Column.FieldName == "balancePKR")
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

                e.Value = value;

            }
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
        }

        private void TreeListView_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            var chartofAccount = e.Node.Content as ChartofAccount;
            if (e.Column.FieldName == "balanceOC")
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
                e.Value = e.Value = Math.Round(value, 2);
            }
            if (e.Column.FieldName == "balancePKR")
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

                e.Value = value;

            }
        }

        private void btnBillLoadRefresh_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplyBillFilters();
            CalculateBillLiabilitySummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void ApplyBillFilters()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            bills = billRepo.getCashFlowAllActive(currentUserId);
            if (selectedCompanies.Any())
            {
                var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToList();
                bills = bills.Where(x => selectedCompanyIds.Contains((int)x.company_Id)).ToList();
            }

            // Check if there are selected departments and apply filter
            if (selectedDepartments.Any())
            {
                var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToList();
                bills = bills.Where(x => selectedDepartmentIds.Contains(x.dept_Id)).ToList();
            }
            if (cmbBillCurrency.SelectedIndex > -1)
            {
                bills = bills.Where(x => x.currency_Id == (cmbBillCurrency.SelectedItem as cmbitem).id).ToList();
                //lblbills.Text = "Bills" + " ( " + (cmbCurrency.SelectedItem as cmbitem).name + " )";
            }
            if (!string.IsNullOrEmpty(txtBillDaysLeft.Text) && Convert.ToInt32(txtBillDaysLeft.Text) != 0)
            {

                int value = 0;
                value = Convert.ToInt32(txtBillDaysLeft.Text);

                if (Convert.ToInt32(txtBillDaysLeft.Text) < 0)
                {
                    value = Convert.ToInt32(txtBillDaysLeft.Text);
                    value = Math.Abs(value);
                }
                else
                {
                    value = Convert.ToInt32(txtBillDaysLeft.Text);
                    value = -value;
                }
                DateTime dateTime = DateTime.Now.AddDays(value);
                if (dateTime.Date < DateTime.Now)
                {
                    bills = bills.Where(x => x.ExpectedPayment >= dateTime).ToList();
                    bills = bills.Where(x => x.ExpectedPayment <= DateTime.Now).ToList();
                }
                else
                {
                    bills = bills.Where(x => x.ExpectedPayment <= dateTime && x.ExpectedPayment >= DateTime.Now).ToList();
                }

            }
            billsOther = bills.Where(x => x.currency_Id != 1 && x.currency_Id != 2 && x.currency_Id != 3 && x.currency_Id != 4 && x.currency_Id != 5 && x.currency_Id != 7).ToList();
            billsOMR = bills.Where(x => x.currency_Id == 7).ToList();
            billsAED = bills.Where(x => x.currency_Id == 5).ToList();
            billsPKR = bills.Where(x => x.currency_Id == 4).ToList();
            billsGBP = bills.Where(x => x.currency_Id == 3).ToList();
            billsUSD = bills.Where(x => x.currency_Id == 2).ToList();
            billsEUR = bills.Where(x => x.currency_Id == 1).ToList();
            grdCashFlowbill.ItemsSource = bills;
            //Calculatebillsummary();

        }

     


        private void isSTLCurrency_Checked(object sender, RoutedEventArgs e)
        {
            cmbSTLCurrency.IsEnabled = true;

        }

        private void isSTLCurrency_Unchecked(object sender, RoutedEventArgs e)
        {
            //lblSaleInvoices.Text = "Sale Invoices";
            cmbSTLCurrency.IsEnabled = false;
            cmbSTLCurrency.SelectedItem = null;
        }

        private void btnLoadRefreshSTL_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            ApplySTLFilters();
            CalculateSTLSummary();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void ApplySTLFilters()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            stls = stlRepo.getCashFlowAllActive(currentUserId);
            if (selectedCompanies.Any())
            {
                var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToList();
                stls = stls.Where(x => selectedCompanyIds.Contains((int)x.company_Id)).ToList();
            }
            // Check if there are selected departments and apply filter
            if (selectedDepartments.Any())
            {
                var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToList();
                stls = stls.Where(x => selectedDepartmentIds.Contains((int)x.dept_Id)).ToList();
            }
            if (cmbSTLCurrency.SelectedIndex > -1)
            {
                stls = stls.Where(x => x.stlCurrency_Id == (cmbSTLCurrency.SelectedItem as cmbitem).id).ToList();
                //lblSTLS.Text = "Sale Invoices" + " ( " + (cmbSICurrency.SelectedItem as cmbitem).name + " )";
            }
            if (!string.IsNullOrEmpty(txtSTLDaysLeft.Text) && Convert.ToInt32(txtSTLDaysLeft.Text) != 0)
            {

                int value = 0;
                if (Convert.ToInt32(txtSTLDaysLeft.Text) < 0)
                {
                    value = Convert.ToInt32(txtSTLDaysLeft.Text);
                    value = Math.Abs(value);
                }
                else
                {
                    value = Convert.ToInt32(txtSTLDaysLeft.Text);
                    value = -value;
                }
                DateTime dateTime = DateTime.Now.AddDays(value);
                if (dateTime.Date < DateTime.Now)
                {
                    stls = stls.Where(x => x.paymentMaturityDate >= dateTime).ToList();
                    stls = stls.Where(x => x.paymentMaturityDate <= DateTime.Now).ToList();
                }
                else
                {

                    stls = stls.Where(x => x.paymentMaturityDate <= dateTime && x.paymentMaturityDate >= DateTime.Now).ToList();
                }

            }
            stlsOther = stls.Where(x => x.stlCurrency_Id != 1 && x.stlCurrency_Id != 2 && x.stlCurrency_Id != 3 && x.stlCurrency_Id != 4 && x.stlCurrency_Id != 5 && x.stlCurrency_Id != 7).ToList();
            stlsOMR = stls.Where(x => x.stlCurrency_Id == 7).ToList();
            stlsAED = stls.Where(x => x.stlCurrency_Id == 5).ToList();
            stlsPKR = stls.Where(x => x.stlCurrency_Id == 4).ToList();
            stlsGBP = stls.Where(x => x.stlCurrency_Id == 3).ToList();
            stlsUSD = stls.Where(x => x.stlCurrency_Id == 2).ToList();
            stlsEUR = stls.Where(x => x.stlCurrency_Id == 1).ToList();
            grdSTLCashFlow.ItemsSource = stls;
        }


        private void grdSTLRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                var stl = grdSTLCashFlow.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Procurements.InterBankTransfers.STL;

                STLRepo repo = new STLRepo();
                stl = repo.Get(stl.Id);
                if (e.Column.FieldName == "InterestAmountIAC" && e.IsGetData)
                {
                    double paymentAmount = 0, interestPerc = 0, utilizeDays = 0, creditTenure = 0;
                    DateTime start = (DateTime)stl.stlPaymentDate;
                    DateTime end = (DateTime)stl.paymentMaturityDate;
                    DateTime calUtilizedDate = DateTime.Now;
                    var stamp = calUtilizedDate - start;
                    utilizeDays = stamp.Days;
                    if (stl.creditTenureNo != 0 && stl.interest != null && stl.stlPaymentDate != null && stl.paymentMaturityDate != null)
                    {
                        creditTenure = Math.Round(stl.creditTenureNo + stl.extendedCreditTenureNo);
                        paymentAmount = Math.Round(stl.stlPaymentAmountOC);
                        interestPerc = Math.Round(Convert.ToDouble(stl.interest.percentage), 2);
                        var value1 = paymentAmount * interestPerc;
                        var value2 = value1 / 100;
                        var value3 = value2 / 365;
                        var value4 = value3 * utilizeDays;
                        e.Value = (Math.Round(value4, 2)).ToString();
                    }
                }

                if (e.Column.FieldName == "stlUtilizedDays1" && e.IsGetData)
                {
                    double utilizeDays = 0;

                    DateTime start = (DateTime)stl.stlPaymentDate;
                    DateTime end = (DateTime)stl.paymentMaturityDate;
                    DateTime calUtilizedDate = DateTime.Now;
                    var stamp = calUtilizedDate - start;
                    utilizeDays = stamp.Days;
                    e.Value = utilizeDays;

                }
                if (e.Column.FieldName == "paymentDueDays1" && e.IsGetData)
                {
                    double utilizeDays = 0;
                    DateTime start = (DateTime)stl.stlPaymentDate;
                    DateTime end = (DateTime)stl.paymentMaturityDate;
                    DateTime calUtilizedDate = DateTime.Now;
                    var stamp = calUtilizedDate - start;
                    utilizeDays = stamp.Days;
                    var value = Convert.ToDouble(stl.creditTenureNo) + Convert.ToDouble(stl.extendedCreditTenureNo);
                    e.Value = Convert.ToDouble(utilizeDays) - value;


                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grdSTLRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL") != null)
                {

                    var selectedStl = grdSTLCashFlow.SelectedItem as ERP_BL.Procurements.InterBankTransfers.STL;
                    winSTLAdd stl = new winSTLAdd(true, Convert.ToInt32(selectedStl.paymentGroupId));
                    stl.stl = selectedStl;
                    stl.Show();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View STL");
                }
            }
            catch (Exception ex)
            {

            }
        }

       
        public void ApplyBankFilters()
        {
            var currentUserId = SYSTEM_STATIC.currentUser.id;
            chartofAccountRepo = new ChartofAccountsRepo();
            chartofAccounts = chartofAccountRepo.getAllActiveForCashFlow(currentUserId);
            if (selectedCompanies?.Any() == true)
            {
                var selectedCompanyIds = selectedCompanies.Select(c => c.Id).ToHashSet();
                chartofAccounts = chartofAccounts
                    .Where(x => x.Companies.Any(company => selectedCompanyIds.Contains(company.Id)))
                    .ToList();
            }

            if (selectedDepartments?.Any() == true)
            {
                var selectedDepartmentIds = selectedDepartments.Select(d => d.Id).ToHashSet();
                chartofAccounts = chartofAccounts
                    .Where(x => x.Departments.Any(department => selectedDepartmentIds.Contains(department.Id)))
                    .ToList();
            }
            if (cmbSTLCurrency.SelectedIndex > -1)
            {
                chartofAccounts = chartofAccounts.Where(x => x.currencyId == (cmbSTLCurrency.SelectedItem as cmbitem).id).ToList();
            }
            chartofAccountsOther = chartofAccounts.Where(x => x.currencyId != 1 && x.currencyId != 2 && x.currencyId != 3 && x.currencyId != 4 && x.currencyId != 5 && x.currencyId != 7).ToList();
            chartofAccountsOMR = chartofAccounts.Where(x => x.currencyId == 7).ToList();
            chartofAccountsAED = chartofAccounts.Where(x => x.currencyId == 5).ToList();
            chartofAccountsPKR = chartofAccounts.Where(x => x.currencyId == 4).ToList();
            chartofAccountsGBP = chartofAccounts.Where(x => x.currencyId == 3).ToList();
            chartofAccountsUSD = chartofAccounts.Where(x => x.currencyId == 2).ToList();
            chartofAccountsEUR = chartofAccounts.Where(x => x.currencyId == 1).ToList();
            grdCashFlowBankAccounts.ItemsSource = chartofAccounts;


        }
        private void chkBankBalanceM_Unchecked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP -= totalBankBalanceMGBP;
            totalAssetsGBPCMER -= totalBankBalanceMGBPCMER;
            totalAssetsEUR -= totalBankBalanceMEUR;
            totalAssetsEURCMER -= totalBankBalanceMEURCMER;
            totalAssetsUSD -= totalBankBalanceMUSD;
            totalAssetsUSDCMER -= totalBankBalanceMUSDCMER;
            totalAssetsAED -= totalBankBalanceMAED;
            totalAssetsAEDCMER -= totalBankBalanceMAEDCMER;
            totalAssetsOMR -= totalBankBalanceMOMR;
            totalAssetsOMRCMER -= totalBankBalanceMOMRCMER;
            totalAssetsPKR -= totalBankBalanceMPKR;
            totalAssetsPKRCMER -= totalBankBalanceMPKRCMER;
            totalAssetsOther -= totalBankBalanceMOther;
            totalAssetsOtherCMER -= totalBankBalanceMOtherCMER;
            totalAssetsCMER -= totalBankBalanceMOtherCMER
                         + totalBankBalanceMPKRCMER
                         + totalBankBalanceMOMRCMER
                         + totalBankBalanceMAEDCMER
                         + totalBankBalanceMEURCMER
                         + totalBankBalanceMUSDCMER
                         + totalBankBalanceMGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        private void chkBankBalanceM_Checked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP += totalBankBalanceMGBP;
            totalAssetsGBPCMER += totalBankBalanceMGBPCMER;
            totalAssetsEUR += totalBankBalanceMEUR;
            totalAssetsEURCMER += totalBankBalanceMEURCMER;
            totalAssetsUSD += totalBankBalanceMUSD;
            totalAssetsUSDCMER += totalBankBalanceMUSDCMER;
            totalAssetsAED += totalBankBalanceMAED;
            totalAssetsAEDCMER += totalBankBalanceMAEDCMER;
            totalAssetsOMR += totalBankBalanceMOMR;
            totalAssetsOMRCMER += totalBankBalanceMOMRCMER;
            totalAssetsPKR += totalBankBalanceMPKR;
            totalAssetsPKRCMER += totalBankBalanceMPKRCMER;
            totalAssetsOther += totalBankBalanceMOther;
            totalAssetsOtherCMER += totalBankBalanceMOtherCMER;
            totalAssetsCMER += totalBankBalanceMOtherCMER
                            + totalBankBalanceMPKRCMER
                            + totalBankBalanceMOMRCMER
                            + totalBankBalanceMAEDCMER
                            + totalBankBalanceMEURCMER
                            + totalBankBalanceMUSDCMER
                            + totalBankBalanceMGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }
        private void chkBankBalanceS_Unchecked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP -= totalBankBalanceSGBP;
            totalAssetsGBPCMER -= totalBankBalanceSGBPCMER;
            totalAssetsEUR -= totalBankBalanceSEUR;
            totalAssetsEURCMER -= totalBankBalanceSEURCMER;
            totalAssetsUSD -= totalBankBalanceSUSD;
            totalAssetsUSDCMER -= totalBankBalanceSUSDCMER;
            totalAssetsAED -= totalBankBalanceSAED;
            totalAssetsAEDCMER -= totalBankBalanceSAEDCMER;
            totalAssetsOMR -= totalBankBalanceSOMR;
            totalAssetsOMRCMER -= totalBankBalanceSOMRCMER;
            totalAssetsPKR -= totalBankBalanceSPKR;
            totalAssetsPKRCMER -= totalBankBalanceSPKRCMER;
            totalAssetsOther -= totalBankBalanceSOther;
            totalAssetsOtherCMER -= totalBankBalanceSOtherCMER;
            totalAssetsCMER -= totalBankBalanceSOtherCMER
    + totalBankBalanceSPKRCMER
    + totalBankBalanceSOMRCMER
    + totalBankBalanceSAEDCMER
    + totalBankBalanceSEURCMER
    + totalBankBalanceSUSDCMER
    + totalBankBalanceSGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        private void chkBankBalanceS_Checked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP += totalBankBalanceSGBP;
            totalAssetsGBPCMER += totalBankBalanceSGBPCMER;
            totalAssetsEUR += totalBankBalanceSEUR;
            totalAssetsEURCMER += totalBankBalanceSEURCMER;
            totalAssetsUSD += totalBankBalanceSUSD;
            totalAssetsUSDCMER += totalBankBalanceSUSDCMER;
            totalAssetsAED += totalBankBalanceSAED;
            totalAssetsAEDCMER += totalBankBalanceSAEDCMER;
            totalAssetsOMR += totalBankBalanceSOMR;
            totalAssetsOMRCMER += totalBankBalanceSOMRCMER;
            totalAssetsPKR += totalBankBalanceSPKR;
            totalAssetsPKRCMER += totalBankBalanceSPKRCMER;
            totalAssetsOther += totalBankBalanceSOther;
            totalAssetsOtherCMER += totalBankBalanceSOtherCMER;
            totalAssetsCMER += totalBankBalanceSOtherCMER
      + totalBankBalanceSPKRCMER
      + totalBankBalanceSOMRCMER
      + totalBankBalanceSAEDCMER
      + totalBankBalanceSEURCMER
      + totalBankBalanceSUSDCMER
      + totalBankBalanceSGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }
        private void chkAdvancePaid_Unchecked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP -= advancePaidAmountGBP;
            totalAssetsGBPCMER -= advancePaidAmountGBPCMER;
            totalAssetsEUR -= advancePaidAmountEUR;
            totalAssetsEURCMER -= advancePaidAmountEURCMER;
            totalAssetsAED -= advancePaidAmountAED;
            totalAssetsAEDCMER -= advancePaidAmountAEDCMER;
            totalAssetsUSD -= advancePaidAmountAED;
            totalAssetsUSDCMER -= advancePaidAmountAEDCMER;
            totalAssetsPKR -= advancePaidAmountPKR;
            totalAssetsPKRCMER -= advancePaidAmountPKRCMER;
            totalAssetsOMR -= advancePaidAmountOMR;
            totalAssetsOMRCMER -= advancePaidAmountOMRCMER;
            totalAssetsOther -= advancePaidAmountOther;
            totalAssetsOtherCMER -= advancePaidAmountOtherCMER;
            totalAssetsCMER -= advancePaidAmountOtherCMER
      + advancePaidAmountPKRCMER
      + advancePaidAmountOMRCMER
      + advancePaidAmountAEDCMER
      + advancePaidAmountEURCMER
      + advancePaidAmountUSDCMER
      + advancePaidAmountGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        private void chkAdvancePaid_Checked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP += advancePaidAmountGBP;
            totalAssetsGBPCMER += advancePaidAmountGBPCMER;
            totalAssetsEUR += advancePaidAmountEUR;
            totalAssetsEURCMER += advancePaidAmountEURCMER;
            totalAssetsAED += advancePaidAmountAED;
            totalAssetsAEDCMER += advancePaidAmountAEDCMER;
            totalAssetsUSD += advancePaidAmountAED;
            totalAssetsUSDCMER += advancePaidAmountAEDCMER;
            totalAssetsPKR += advancePaidAmountPKR;
            totalAssetsPKRCMER += advancePaidAmountPKRCMER;
            totalAssetsOMR += advancePaidAmountOMR;
            totalAssetsOMRCMER += advancePaidAmountOMRCMER;
            totalAssetsOther += advancePaidAmountOther;
            totalAssetsOtherCMER += advancePaidAmountOtherCMER;
            totalAssetsCMER += advancePaidAmountOtherCMER
          + advancePaidAmountPKRCMER
          + advancePaidAmountOMRCMER
          + advancePaidAmountAEDCMER
          + advancePaidAmountEURCMER
          + advancePaidAmountUSDCMER
          + advancePaidAmountGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }
        private void chkAdvancePaidS_Unchecked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP -= advancePaidAmountSystemGBP;
            totalAssetsGBPCMER -= advancePaidAmountSystemGBPCMER;
            totalAssetsEUR -= advancePaidAmountSystemEUR;
            totalAssetsEURCMER -= advancePaidAmountSystemEURCMER;
            totalAssetsAED -= advancePaidAmountSystemAED;
            totalAssetsAEDCMER -= advancePaidAmountSystemAEDCMER;
            totalAssetsUSD -= advancePaidAmountSystemAED;
            totalAssetsUSDCMER -= advancePaidAmountSystemAEDCMER;
            totalAssetsPKR -= advancePaidAmountSystemPKR;
            totalAssetsPKRCMER -= advancePaidAmountSystemPKRCMER;
            totalAssetsOMR -= advancePaidAmountSystemOMR;
            totalAssetsOMRCMER -= advancePaidAmountSystemOMRCMER;
            totalAssetsOther -= advancePaidAmountSystemOther;
            totalAssetsOtherCMER -= advancePaidAmountSystemOtherCMER;
            totalAssetsCMER -= advancePaidAmountSystemOtherCMER
            + advancePaidAmountSystemPKRCMER
            + advancePaidAmountSystemOMRCMER
            + advancePaidAmountSystemAEDCMER
            + advancePaidAmountSystemEURCMER
            + advancePaidAmountSystemUSDCMER
            + advancePaidAmountSystemGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        private void chkAdvancePaidS_Checked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP += advancePaidAmountSystemGBP;
            totalAssetsGBPCMER += advancePaidAmountSystemGBPCMER;
            totalAssetsEUR += advancePaidAmountSystemEUR;
            totalAssetsEURCMER += advancePaidAmountSystemEURCMER;
            totalAssetsAED += advancePaidAmountSystemAED;
            totalAssetsAEDCMER += advancePaidAmountSystemAEDCMER;
            totalAssetsUSD += advancePaidAmountSystemAED;
            totalAssetsUSDCMER += advancePaidAmountSystemAEDCMER;
            totalAssetsPKR += advancePaidAmountSystemPKR;
            totalAssetsPKRCMER += advancePaidAmountSystemPKRCMER;
            totalAssetsOMR += advancePaidAmountSystemOMR;
            totalAssetsOMRCMER += advancePaidAmountSystemOMRCMER;
            totalAssetsOther += advancePaidAmountSystemOther;
            totalAssetsOtherCMER += advancePaidAmountSystemOtherCMER;
            totalAssetsCMER += advancePaidAmountSystemOtherCMER
             + advancePaidAmountSystemPKRCMER
             + advancePaidAmountSystemOMRCMER
             + advancePaidAmountSystemAEDCMER
             + advancePaidAmountSystemEURCMER
             + advancePaidAmountSystemUSDCMER
             + advancePaidAmountSystemGBPCMER;

            calculateTotalAssets();
            calculateTotalCMERAssets();
        }
        private void chkReceivable_Unchecked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP -= totalRemainigCollectionOCGBP;
            totalAssetsGBPCMER -= totalRemainigCollectionOCGBPCMER;
            totalAssetsUSD -= totalRemainigCollectionOCUSD;
            totalAssetsUSDCMER -= totalRemainigCollectionOCUSDCMER;
            totalAssetsEUR -= totalRemainigCollectionOCEUR;
            totalAssetsEURCMER -= totalRemainigCollectionOCEURCMER;
            totalAssetsAED -= totalRemainigCollectionOCAED;
            totalAssetsAEDCMER -= totalRemainigCollectionOCAEDCMER;
            totalAssetsOMR -= totalRemainigCollectionOCOMR;
            totalAssetsOMRCMER -= totalRemainigCollectionOCOMRCMER;
            totalAssetsPKR -= totalRemainigCollectionOCPKR;
            totalAssetsPKRCMER -= totalRemainigCollectionOCPKRCMER;
            totalAssetsOther -= totalRemainigCollectionOCOther;
            totalAssetsOtherCMER -= totalRemainigCollectionOCOtherCMER;
            totalAssetsCMER -=
               +totalRemainigCollectionOCOtherCMER
               + totalRemainigCollectionOCPKRCMER
               + totalRemainigCollectionOCOMRCMER
               + totalRemainigCollectionOCAEDCMER
               + totalRemainigCollectionOCEURCMER
               + totalRemainigCollectionOCUSDCMER
               + totalRemainigCollectionOCGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        private void chkReceivable_Checked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP += totalRemainigCollectionOCGBP;
            totalAssetsGBPCMER += totalRemainigCollectionOCGBPCMER;
            totalAssetsUSD += totalRemainigCollectionOCUSD;
            totalAssetsUSDCMER += totalRemainigCollectionOCUSDCMER;
            totalAssetsEUR += totalRemainigCollectionOCEUR;
            totalAssetsEURCMER += totalRemainigCollectionOCEURCMER;
            totalAssetsAED += totalRemainigCollectionOCAED;
            totalAssetsAEDCMER += totalRemainigCollectionOCAEDCMER;
            totalAssetsOMR += totalRemainigCollectionOCOMR;
            totalAssetsOMRCMER += totalRemainigCollectionOCOMRCMER;
            totalAssetsPKR += totalRemainigCollectionOCPKR;
            totalAssetsPKRCMER += totalRemainigCollectionOCPKRCMER;
            totalAssetsOther += totalRemainigCollectionOCOther;
            totalAssetsOtherCMER += totalRemainigCollectionOCOtherCMER;
            totalAssetsCMER +=
                +totalRemainigCollectionOCOtherCMER
                + totalRemainigCollectionOCPKRCMER
                + totalRemainigCollectionOCOMRCMER
                + totalRemainigCollectionOCAEDCMER
                + totalRemainigCollectionOCEURCMER
                + totalRemainigCollectionOCUSDCMER
                + totalRemainigCollectionOCGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }
        private void chkAPSuppliersLiabilities_Unchecked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP -= totalUnPaidPOAmountGBP;
            totalLiabilitiesGBPCMER -= totalUnPaidPOAmountGBPCMER;
            totalLiabilitiesUSD -= totalUnPaidPOAmountUSD;
            totalLiabilitiesUSDCMER -= totalUnPaidPOAmountUSDCMER;
            totalLiabilitiesEUR -= totalUnPaidPOAmountEUR;
            totalLiabilitiesEURCMER -= totalUnPaidPOAmountEURCMER;
            totalLiabilitiesAED -= totalUnPaidPOAmountAED;
            totalLiabilitiesAEDCMER -= totalUnPaidPOAmountAEDCMER;
            totalLiabilitiesOMR -= totalUnPaidPOAmountOMR;
            totalLiabilitiesOMRCMER -= totalUnPaidPOAmountOMRCMER;
            totalLiabilitiesPKR -= totalUnPaidPOAmountPKR;
            totalLiabilitiesPKRCMER -= totalUnPaidPOAmountPKRCMER;
            totalLiabilitiesOther -= totalUnPaidPOAmountOther;
            totalLiabilitiesOtherCMER -= totalUnPaidPOAmountOtherCMER;
            totalLiabilitiesCMER -=
                +totalUnPaidPOAmountOtherCMER
                + totalUnPaidPOAmountPKRCMER
                + totalUnPaidPOAmountOMRCMER
                + totalUnPaidPOAmountAEDCMER
                + totalUnPaidPOAmountEURCMER
                + totalUnPaidPOAmountUSDCMER
                + totalUnPaidPOAmountGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();

        }

        private void chkAPSuppliersLiabilities_Checked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP += totalUnPaidPOAmountGBP;
            totalLiabilitiesGBPCMER += totalUnPaidPOAmountGBPCMER;
            totalLiabilitiesUSD += totalUnPaidPOAmountUSD;
            totalLiabilitiesUSDCMER += totalUnPaidPOAmountUSDCMER;
            totalLiabilitiesEUR += totalUnPaidPOAmountEUR;
            totalLiabilitiesEURCMER += totalUnPaidPOAmountEURCMER;
            totalLiabilitiesAED += totalUnPaidPOAmountAED;
            totalLiabilitiesAEDCMER += totalUnPaidPOAmountAEDCMER;
            totalLiabilitiesOMR += totalUnPaidPOAmountOMR;
            totalLiabilitiesOMRCMER += totalUnPaidPOAmountOMRCMER;
            totalLiabilitiesPKR += totalUnPaidPOAmountPKR;
            totalLiabilitiesPKRCMER += totalUnPaidPOAmountPKRCMER;
            totalLiabilitiesOther += totalUnPaidPOAmountOther;
            totalLiabilitiesOtherCMER += totalUnPaidPOAmountOtherCMER;
            totalLiabilitiesCMER +=
                +totalUnPaidPOAmountOtherCMER
                + totalUnPaidPOAmountPKRCMER
                + totalUnPaidPOAmountOMRCMER
                + totalUnPaidPOAmountAEDCMER
                + totalUnPaidPOAmountEURCMER
                + totalUnPaidPOAmountUSDCMER
                + totalUnPaidPOAmountGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkSTLLiabilities_Unchecked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP -= totalSTLAmountGBP;
            totalLiabilitiesGBPCMER -= totalSTLAmountGBPCMER;
            totalLiabilitiesUSD -= totalSTLAmountUSD;
            totalLiabilitiesUSDCMER -= totalSTLAmountUSDCMER;
            totalLiabilitiesEUR -= totalSTLAmountEUR;
            totalLiabilitiesEURCMER -= totalSTLAmountEURCMER;
            totalLiabilitiesAED -= totalSTLAmountAED;
            totalLiabilitiesAEDCMER -= totalSTLAmountAEDCMER;
            totalLiabilitiesOMR -= totalSTLAmountOMR;
            totalLiabilitiesOMRCMER -= totalSTLAmountOMRCMER;
            totalLiabilitiesPKR -= totalSTLAmountPKR;
            totalLiabilitiesPKRCMER -= totalSTLAmountPKRCMER;
            totalLiabilitiesOther -= totalSTLAmountOther;
            totalLiabilitiesOtherCMER -= totalSTLAmountOtherCMER;
            totalLiabilitiesCMER -=
                +totalSTLAmountOtherCMER
                + totalSTLAmountPKRCMER
                + totalSTLAmountOMRCMER
                + totalSTLAmountAEDCMER
                + totalSTLAmountEURCMER
               + totalSTLAmountUSDCMER
                + totalSTLAmountGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkSTLLiabilities_Checked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP += totalSTLAmountGBP;
            totalLiabilitiesGBPCMER += totalSTLAmountGBPCMER;
            totalLiabilitiesUSD += totalSTLAmountUSD;
            totalLiabilitiesUSDCMER += totalSTLAmountUSDCMER;
            totalLiabilitiesEUR += totalSTLAmountEUR;
            totalLiabilitiesEURCMER += totalSTLAmountEURCMER;
            totalLiabilitiesAED += totalSTLAmountAED;
            totalLiabilitiesAEDCMER += totalSTLAmountAEDCMER;
            totalLiabilitiesOMR += totalSTLAmountOMR;
            totalLiabilitiesOMRCMER += totalSTLAmountOMRCMER;
            totalLiabilitiesPKR += totalSTLAmountPKR;
            totalLiabilitiesPKRCMER += totalSTLAmountPKRCMER;
            totalLiabilitiesOther += totalSTLAmountOther;
            totalLiabilitiesOtherCMER += totalSTLAmountOtherCMER;
            totalLiabilitiesCMER +=
                +totalSTLAmountOtherCMER
                + totalSTLAmountPKRCMER
                + totalSTLAmountOMRCMER
                + totalSTLAmountAEDCMER
                + totalSTLAmountEURCMER
                + totalSTLAmountUSDCMER
                + totalSTLAmountGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkAdminBillsLiabilities_Unchecked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP -= totalPaymentGBP;
            totalLiabilitiesGBPCMER -= totalPaymentGBPCMER;
            totalLiabilitiesUSD -= totalPaymentUSD;
            totalLiabilitiesUSDCMER -= totalPaymentUSDCMER;
            totalLiabilitiesEUR -= totalPaymentEUR;
            totalLiabilitiesEURCMER -= totalPaymentEURCMER;
            totalLiabilitiesAED -= totalPaymentAED;
            totalLiabilitiesAEDCMER -= totalPaymentAEDCMER;
            totalLiabilitiesOMR -= totalPaymentOMR;
            totalLiabilitiesOMRCMER -= totalPaymentOMRCMER;
            totalLiabilitiesPKR -= totalPaymentPKR;
            totalLiabilitiesPKRCMER -= totalPaymentPKRCMER;
            totalLiabilitiesOther -= totalPaymentOther;
            totalLiabilitiesOtherCMER -= totalPaymentOtherCMER;
            totalLiabilitiesCMER -=
                +totalPaymentOtherCMER
               + totalPaymentPKRCMER
                + totalPaymentOMRCMER
                + totalPaymentAEDCMER
                + totalPaymentEURCMER
                + totalPaymentUSDCMER
                + totalPaymentGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkAdminBillsLiabilities_Checked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP += totalPaymentGBP;
            totalLiabilitiesGBPCMER += totalPaymentGBPCMER;
            totalLiabilitiesUSD += totalPaymentUSD;
            totalLiabilitiesUSDCMER += totalPaymentUSDCMER;
            totalLiabilitiesEUR += totalPaymentEUR;
            totalLiabilitiesEURCMER += totalPaymentEURCMER;
            totalLiabilitiesAED += totalPaymentAED;
            totalLiabilitiesAEDCMER += totalPaymentAEDCMER;
            totalLiabilitiesOMR += totalPaymentOMR;
            totalLiabilitiesOMRCMER += totalPaymentOMRCMER;
            totalLiabilitiesPKR += totalPaymentPKR;
            totalLiabilitiesPKRCMER += totalPaymentPKRCMER;
            totalLiabilitiesOther += totalPaymentOther;
            totalLiabilitiesOtherCMER += totalPaymentOtherCMER;
            totalLiabilitiesCMER +=
                +totalPaymentOtherCMER
                + totalPaymentPKRCMER
                + totalPaymentOMRCMER
                + totalPaymentAEDCMER
                + totalPaymentEURCMER
                + totalPaymentUSDCMER
                + totalPaymentGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkAPVendorBillsLiabilities_Checked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP += totalUnPaidBillAmountGBP;
            totalLiabilitiesGBPCMER += totalUnPaidBillAmountGBPCMER;
            totalLiabilitiesUSD += totalUnPaidBillAmountUSD;
            totalLiabilitiesUSDCMER += totalUnPaidBillAmountUSDCMER;
            totalLiabilitiesEUR += totalUnPaidBillAmountEUR;
            totalLiabilitiesEURCMER += totalUnPaidBillAmountEURCMER;
            totalLiabilitiesAED += totalUnPaidBillAmountAED;
            totalLiabilitiesAEDCMER += totalUnPaidBillAmountAEDCMER;
            totalLiabilitiesOMR += totalUnPaidBillAmountOMR;
            totalLiabilitiesOMRCMER += totalUnPaidBillAmountOMRCMER;
            totalLiabilitiesPKR += totalUnPaidBillAmountPKR;
            totalLiabilitiesPKRCMER += totalUnPaidBillAmountPKRCMER;
            totalLiabilitiesOther += totalUnPaidBillAmountOther;
            totalLiabilitiesOtherCMER += totalUnPaidBillAmountOtherCMER;
            totalLiabilitiesCMER +=
                +totalUnPaidBillAmountOtherCMER
                + totalUnPaidBillAmountPKRCMER
                + totalUnPaidBillAmountOMRCMER
                + totalUnPaidBillAmountAEDCMER
                + totalUnPaidBillAmountEURCMER
                + totalUnPaidBillAmountUSDCMER
                + totalUnPaidBillAmountGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }

        private void chkAPVendorBillsLiabilities_Unchecked(object sender, RoutedEventArgs e)
        {
            totalLiabilitiesGBP -= totalUnPaidBillAmountGBP;
            totalLiabilitiesGBPCMER -= totalUnPaidBillAmountGBPCMER;
            totalLiabilitiesUSD -= totalUnPaidBillAmountUSD;
            totalLiabilitiesUSDCMER -= totalUnPaidBillAmountUSDCMER;
            totalLiabilitiesEUR -= totalUnPaidBillAmountEUR;
            totalLiabilitiesEURCMER -= totalUnPaidBillAmountEURCMER;
            totalLiabilitiesAED -= totalUnPaidBillAmountAED;
            totalLiabilitiesAEDCMER -= totalUnPaidBillAmountAEDCMER;
            totalLiabilitiesOMR -= totalUnPaidBillAmountOMR;
            totalLiabilitiesOMRCMER -= totalUnPaidBillAmountOMRCMER;
            totalLiabilitiesPKR -= totalUnPaidBillAmountPKR;
            totalLiabilitiesPKRCMER -= totalUnPaidBillAmountPKRCMER;
            totalLiabilitiesOther -= totalUnPaidBillAmountOther;
            totalLiabilitiesOtherCMER -= totalUnPaidBillAmountOtherCMER;
            totalLiabilitiesCMER -=
                +totalUnPaidBillAmountOtherCMER
                + totalUnPaidBillAmountPKRCMER
                + totalUnPaidBillAmountOMRCMER
               + totalUnPaidBillAmountAEDCMER
                + totalUnPaidBillAmountEURCMER
                + totalUnPaidBillAmountUSDCMER
                + totalUnPaidBillAmountGBPCMER;
            calculateTotalLiabilities();
            calculateTotalCMERLiabilities();
        }
        private void chkSOReceibable_Unchecked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP -= totalSaleOrdersGBP;
            totalAssetsGBPCMER -= totalSaleOrdersGBPCMER;
            totalAssetsUSD -= totalSaleOrdersUSD;
            totalAssetsUSDCMER -= totalSaleOrdersUSDCMER;
            totalAssetsEUR -= totalSaleOrdersEUR;
            totalAssetsEURCMER -= totalSaleOrdersEURCMER;
            totalAssetsAED -= totalSaleOrdersAED;
            totalAssetsAEDCMER -= totalSaleOrdersAEDCMER;
            totalAssetsOMR -= totalSaleOrdersOMR;
            totalAssetsOMRCMER -= totalSaleOrdersOMRCMER;
            totalAssetsPKR -= totalSaleOrdersPKR;
            totalAssetsPKRCMER -= totalSaleOrdersPKRCMER;
            totalAssetsOther -= totalSaleOrdersOther;
            totalAssetsOtherCMER -= totalSaleOrdersOtherCMER;
            totalAssetsCMER -=
               +totalSaleOrdersOtherCMER
               + totalSaleOrdersPKRCMER
               + totalSaleOrdersOMRCMER
               + totalSaleOrdersAEDCMER
               + totalSaleOrdersEURCMER
               + totalSaleOrdersUSDCMER
               + totalSaleOrdersGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();
        }

        private void chkSOReceibable_Checked(object sender, RoutedEventArgs e)
        {
            totalAssetsGBP += totalSaleOrdersGBP;
            totalAssetsGBPCMER += totalSaleOrdersGBPCMER;
            totalAssetsUSD += totalSaleOrdersUSD;
            totalAssetsUSDCMER += totalSaleOrdersUSDCMER;
            totalAssetsEUR += totalSaleOrdersEUR;
            totalAssetsEURCMER += totalSaleOrdersEURCMER;
            totalAssetsAED += totalSaleOrdersAED;
            totalAssetsAEDCMER += totalSaleOrdersAEDCMER;
            totalAssetsOMR += totalSaleOrdersOMR;
            totalAssetsOMRCMER += totalSaleOrdersOMRCMER;
            totalAssetsPKR += totalSaleOrdersPKR;
            totalAssetsPKRCMER += totalSaleOrdersPKRCMER;
            totalAssetsOther += totalSaleOrdersOther;
            totalAssetsOtherCMER += totalSaleOrdersOtherCMER;
            totalAssetsCMER +=
                +totalSaleOrdersOtherCMER
                + totalSaleOrdersPKRCMER
                + totalSaleOrdersOMRCMER
                + totalSaleOrdersAEDCMER
                + totalSaleOrdersEURCMER
                + totalSaleOrdersUSDCMER
                + totalSaleOrdersGBPCMER;
            calculateTotalAssets();
            calculateTotalCMERAssets();

        }
        private void chkARAdvances_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void chkARAdvances_Checked(object sender, RoutedEventArgs e)
        {

        }

        public double GetCMER(int companyCurrencyId, int tCurrencyId, int companyId)
        {

            double todayRate = 0;
            var exchangeRateGroupMER = exchangeRateGroups.FirstOrDefault(x => x.transaction_currency_Id == tCurrencyId && x.base_currency_Id == companyCurrencyId && x.TargetYear == DateTime.Now.Year);
            if (exchangeRateGroupMER != null)
            {
                exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == companyId);
                switch (DateTime.Now.Month)
                {
                    case 1:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateJan;
                        break;
                    case 2:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateFeb;
                        break;
                    case 3:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateMar;
                        break;
                    case 4:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateApr;
                        break;
                    case 5:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateMay;
                        break;
                    case 6:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateJun;
                        break;
                    case 7:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateJul;
                        break;
                    case 8:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateAug;
                        break;
                    case 9:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateSep;
                        break;
                    case 10:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateOct;
                        break;
                    case 11:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateNov;
                        break;
                    case 12:
                        if (exchangeRate != null)
                            todayRate = exchangeRate.rateDec;
                        break;
                    default:
                        if (exchangeRate != null)
                            todayRate = 0;
                        break;
                }
            }
            return todayRate;

        }
        private void GrdItems_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "OnHand")
            {

                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                if (product.Inventories != null && product.Inventories.Count != 0)
                {
                    var purchaseQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.Quantity);
                    var saleQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.Quantity);

                    var adjustmentQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true).Sum(x => x.Quantity);
                    purchaseQuantity = purchaseQuantity + adjustmentQuantity;

                    e.Value = purchaseQuantity + saleQuantity;

                }
            }
            else
           if (e.Column.FieldName == "AmountOC")
            {
                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                var totalPurchaseAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.AmountOC);
                var totalSaleAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.AmountOC);
                var totalAdjustmentAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true).Sum(x => x.AmountOC);
                totalPurchaseAmount = totalPurchaseAmount + totalAdjustmentAmount;
                e.Value = totalPurchaseAmount + totalSaleAmount;
            }
            else
           if (e.Column.FieldName == "AmountPKR")
            {
                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                var totalPAmountMER = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.AmountMER * x.MER);
                var totalSAmountMER = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.AmountMER * x.MER);
                var totalAdjustmentMER = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true).Sum(x => x.AmountMER * x.MER);
                totalPAmountMER = totalPAmountMER + totalAdjustmentMER;
                e.Value = totalPAmountMER + totalSAmountMER;
            }
            else
           if (e.Column.FieldName == "AvgCost")
            {
                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                var sumPurchase = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.AmountOC);
                var sumAdjusted = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true).Sum(x => x.AmountOC);
                var sumSales = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.AmountOC);
                var qSumPurchases = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.Quantity);
                var qSumAdjustment = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true).Sum(x => x.Quantity);


                sumPurchase = sumPurchase + sumAdjusted;
                qSumPurchases = qSumPurchases + qSumAdjustment;

                var qSumSales = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.Quantity);





                var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                if (averageCost == 0)
                {
                    e.Value = 0;
                }
                else
                    e.Value = averageCost;
            }
            if (e.Column.FieldName == "Departments")
            {
                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                var res = String.Join(", ", product.departments.Select(x => x.DeptName));
                e.Value = res;
            }

        }

        private void GrdItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            var inventory = grdInventory.SelectedItem as ERP_BL.Procurements.Inventories.Inventory;
            if (inventory != null)
            {
                if (inventory.PurchaseInvoiceId != null)
                {

                    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Invoice, (int)inventory.PurchaseInvoiceId);
                    procurmentPanel.Show();

                }
                else
                if (inventory.SaleInviceId != null)
                {
                    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, (int)inventory.SaleInviceId);
                    procurmentPanel.Show();
                }
                else
                if (inventory.adjustment_Id != null)
                {
                    try
                    {
                        ZAS_ERP.Procurementss.Inventory.Adjustment.WIndows.winfrmAdjustInventory adjustment = new ZAS_ERP.Procurementss.Inventory.Adjustment.WIndows.winfrmAdjustInventory();
                        adjustment.OrderId = (int)inventory.adjustment_Id;
                        adjustment.editOrder = 1;
                        adjustment.Show();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

        }
    }
}
