using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
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
using ZAS_ERP.Procurementss.SaleOrderss;

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for winSelectCostSheetFields.xaml
    /// </summary>
    public partial class winSelectCostSheetFields : DXWindow
    {
        SaleOrder saleOrder = new SaleOrder();
        public List<CostFieldValues> costFieldValue = new List<CostFieldValues>();
        List<CostFieldValues> changedFields = new List<CostFieldValues>();
        public SaleOrderRepo repo = new SaleOrderRepo();
        public static ERP_BL.Databases.CostSheet  costSheet = new ERP_BL.Databases.CostSheet();
        public List<FieldValue> fieldValues = new List<FieldValue>();
        public List<CostFieldHistory> fieldHistoryValues = new List<CostFieldHistory>();
        public static List<CostSheetBillField> costSheetBillFields = new List<CostSheetBillField>();
        public static List<CostSheetPOField> costSheetPOFields = new List<CostSheetPOField>();
        public List<CostSheetSOField> costSheetSOFields = new List<CostSheetSOField>();
        public static List<CostSheetSaleReceiptField> costSheetSaleReceiptFields = new List<CostSheetSaleReceiptField>();
        public static List<CostSheetPaymentField> costSheetPaymentFields = new List<CostSheetPaymentField>();
        public static List<CostSheetSIField> costSheetSIFields = new List<CostSheetSIField>();




        public List<int> selectedFieldsIds = new List<int>();
        int billId = 0;
        int purchaseOrderId = 0;
        int saleInvoiceId = 0;
        SalesReceipt receipt = new SalesReceipt();
        Payment payment = new Payment();
        bool checkFlag;
        decimal billAmount = 0;
        decimal purchaseOrderAmount = 0;
        decimal saleInvoiceAmount = 0;
        decimal receiptAmount = 0;
        decimal paymentAmount = 0;
        List<CostFieldValues> costfieldCheckedValues = new List<CostFieldValues>();
        TransactionItemType type=0;
        bool isBudgeted = false;
        IncotermRepo IncotermRepo = new IncotermRepo();

        public winSelectCostSheetFields()
        {
            InitializeComponent();
        }
        public winSelectCostSheetFields(SaleOrder _saleOrder, bool _isBudgetd)
        {
            InitializeComponent();
            saleOrder = _saleOrder;
            costSheet = saleOrder.CostSheet;
            isBudgeted = _isBudgetd;

            //textBoxes hiding 
            txtPOAmountSOC.Visibility = Visibility.Collapsed;
            lblAmountSOC.Visibility = Visibility.Collapsed;
            lblDifference.Visibility = Visibility.Collapsed;
            txtDifference.Visibility = Visibility.Collapsed;

            //gridColumns Hiding 
            grdCostItems.Columns.GetColumnByFieldName("poAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("billAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("addedSystemValue").Visible = false;
         
            //grdCostItems.Columns.GetColumnByFieldName("systemValue").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("receiptAmount").Visible = false;

            //grdCostItems.Columns.GetColumnByFieldName("systemValue").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("receiptAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("siAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("paymentAmount").Visible = false;



            if (saleOrder.currency != null)
            {
                if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                {
                    if (saleOrder.costcenterCurrency != null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                    }
                }
                else
                {
                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                }
            }
            else
            {
                if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                {
                    if (saleOrder.costcenterCurrency != null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                    }
                }
                else
                {

                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.currency.CurrencyName;
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.currency.CurrencyName;
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.currency.CurrencyName;
                }
            }
        }
        public winSelectCostSheetFields(SaleOrder _saleOrder, decimal _billAmount, int _billId)
        {
            InitializeComponent();
            saleOrder = _saleOrder;
            costSheet = saleOrder.CostSheet;
            billAmount = _billAmount;
            billId = _billId;
            txtTotalSystemCostSOC.Visibility = Visibility.Visible;
            lblTotalAddedSCost.Visibility = Visibility.Visible;
            lblRBSC.Visibility = Visibility.Collapsed;
            txtTotalSRBC.Visibility = Visibility.Collapsed;
            grdCostItems.Columns.GetColumnByFieldName("receiptAmount").Visible = false;



            grdCostItems.Columns.GetColumnByFieldName("poAmount").Visible = false;

            //grdCostItems.Columns.GetColumnByFieldName("SRBC").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("addedSRBCValue").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("siAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("paymentAmount").Visible = false;
            if (saleOrder.currency != null)
            {
                if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                {
                    if (saleOrder.costcenterCurrency != null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                    }
                }
                else
                {
                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                }
            }
            else
            {
                if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                {
                    if (saleOrder.costcenterCurrency != null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                    }
                }
                else
                {

                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.currency.CurrencyName;
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.currency.CurrencyName;
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.currency.CurrencyName;
                }
            }
        } 

        public winSelectCostSheetFields(SaleOrder _saleOrder, decimal _deductionAmount, SalesReceipt _salesReceipt)
        {
            InitializeComponent();
            saleOrder = _saleOrder;
            costSheet = saleOrder.CostSheet;
            receiptAmount = _deductionAmount;
            receipt = _salesReceipt;
            txtTotalSystemCostSOC.Visibility = Visibility.Visible;
            lblTotalAddedSCost.Visibility = Visibility.Visible;
            lblRBSC.Visibility = Visibility.Collapsed;
            txtTotalSRBC.Visibility = Visibility.Collapsed;
            this.type = TransactionItemType.Sale_Receipt;


            grdCostItems.Columns.GetColumnByFieldName("poAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("billAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("siAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("paymentAmount").Visible = false;

            //grdCostItems.Columns.GetColumnByFieldName("SRBC").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("addedSRBCValue").Visible = false;
            if (saleOrder.currency != null)
            {
                if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                {
                    if (saleOrder.costcenterCurrency != null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                    }
                }
                else
                {
                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                }
            }
            else
            {
                if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                {
                    if (saleOrder.costcenterCurrency != null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                    }
                }
                else
                {

                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.currency.CurrencyName;
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.currency.CurrencyName;
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.currency.CurrencyName;
                }
            }
        }
        public winSelectCostSheetFields(SaleOrder _saleOrder, decimal _purchaseOrderAmount, int _purchaseOrderId, TransactionItemType type)
        {
            InitializeComponent();

            if (type == TransactionItemType.Purchase_Order)
            {
                saleOrder = _saleOrder;
                costSheet = saleOrder.CostSheet;
                purchaseOrderAmount = _purchaseOrderAmount;
                purchaseOrderId = _purchaseOrderId;
                this.type = type;

                grdCostItems.Columns.GetColumnByFieldName("billAmount").Visible = false;

                //grdCostItems.Columns.GetColumnByFieldName("SRBC").Visible = false;
                grdCostItems.Columns.GetColumnByFieldName("addedSRBCValue").Visible = false;
                txtTotalSystemCostSOC.Visibility = Visibility.Visible;
                lblTotalAddedSCost.Visibility = Visibility.Visible;
                lblRBSC.Visibility = Visibility.Collapsed;
                txtTotalSRBC.Visibility = Visibility.Collapsed;
                grdCostItems.Columns.GetColumnByFieldName("receiptAmount").Visible = false;
                grdCostItems.Columns.GetColumnByFieldName("siAmount").Visible = false;
                grdCostItems.Columns.GetColumnByFieldName("paymentAmount").Visible = false;


                if (saleOrder.currency != null)
                {
                    if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                    {
                        if (saleOrder.costcenterCurrency != null)
                        {
                            grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                            grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        }
                    }
                    else
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    }
                }
                else
                {
                    if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                    {
                        if (saleOrder.costcenterCurrency != null)
                        {
                            grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                            grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        }
                    }
                    else
                    {

                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.currency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.currency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.currency.CurrencyName;
                    }
                }
            }
            else
            if(type==TransactionItemType.Sale_Invoice)
            {
                saleOrder = _saleOrder;
                costSheet = saleOrder.CostSheet;
                saleInvoiceAmount = _purchaseOrderAmount;
                saleInvoiceId = _purchaseOrderId;
                this.type = type;

                grdCostItems.Columns.GetColumnByFieldName("billAmount").Visible = false;

                //grdCostItems.Columns.GetColumnByFieldName("SRBC").Visible = false;
                grdCostItems.Columns.GetColumnByFieldName("addedSRBCValue").Visible = false;
                txtTotalSystemCostSOC.Visibility = Visibility.Visible;
                lblTotalAddedSCost.Visibility = Visibility.Visible;
                lblRBSC.Visibility = Visibility.Collapsed;
                txtTotalSRBC.Visibility = Visibility.Collapsed;
                grdCostItems.Columns.GetColumnByFieldName("receiptAmount").Visible = false;
                grdCostItems.Columns.GetColumnByFieldName("paymentAmount").Visible = false;
                grdCostItems.Columns.GetColumnByFieldName("poAmount").Visible = false;



                if (saleOrder.currency != null)
                {
                    if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                    {
                        if (saleOrder.costcenterCurrency != null)
                        {
                            grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                            grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        }
                    }
                    else
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    }
                }
                else
                {
                    if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                    {
                        if (saleOrder.costcenterCurrency != null)
                        {
                            grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                            grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        }
                    }
                    else
                    {

                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.currency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.currency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.currency.CurrencyName;
                    }
                }
            }
        }

        public winSelectCostSheetFields(SaleOrder _saleOrder, decimal _deductionAmount, Payment _Payment)
        {
            InitializeComponent();
            saleOrder = _saleOrder;
            costSheet = saleOrder.CostSheet;
            paymentAmount  = _deductionAmount;
            payment = _Payment;
            txtTotalSystemCostSOC.Visibility = Visibility.Visible;
            lblTotalAddedSCost.Visibility = Visibility.Visible;
            lblRBSC.Visibility = Visibility.Collapsed;
            txtTotalSRBC.Visibility = Visibility.Collapsed;
            this.type = TransactionItemType.Payments;
            grdCostItems.Columns.GetColumnByFieldName("poAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("billAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("receiptAmount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("siAmount").Visible = false;
            //grdCostItems.Columns.GetColumnByFieldName("SRBC").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("addedSRBCValue").Visible = false;
            if (saleOrder.currency != null)
            {
                if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                {
                    if (saleOrder.costcenterCurrency != null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.costcenterCurrency.Abbrivation + " " + saleOrder.costcenterCurrency.Symbol + ")";
                    }
                }
                else
                {
                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
                }
            }
            else
            {
                if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                {
                    if (saleOrder.costcenterCurrency != null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.costcenterCurrency.CurrencyName;
                    }
                }
                else
                {

                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += saleOrder.currency.CurrencyName;
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += saleOrder.currency.CurrencyName;
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").Header += saleOrder.currency.CurrencyName;
                }
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            checkHideActualCost.IsChecked = true;
            checkHideRevisedCost.IsChecked = true;
           
            //checkHideSystemCost.IsChecked = true;
            foreach (var item in repo.getActiveCostSheetFields())
            {
                costFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title });
            }
            grdCostItems.ItemsSource = costFieldValue;
            if (billAmount != 0)
            {
               billAmount= Math.Round(billAmount, 2);
                txtPOAmountSOC.Text = billAmount.ToString();
            }
            else
             if (purchaseOrderAmount != 0)
            {
                purchaseOrderAmount=Math.Round(purchaseOrderAmount, 2);

                txtPOAmountSOC.Text = purchaseOrderAmount.ToString();
            }
            else
             if (saleInvoiceAmount != 0)
            {
                saleInvoiceAmount = Math.Round(saleInvoiceAmount, 2);

                txtPOAmountSOC.Text = saleInvoiceAmount.ToString();
            }
            else
             if (receiptAmount != 0)
            {
                receiptAmount = Math.Round(receiptAmount, 2);

                txtPOAmountSOC.Text = receiptAmount.ToString();
            }
            else
             if (paymentAmount != 0)
            {
                paymentAmount = Math.Round(paymentAmount, 2);

                txtPOAmountSOC.Text = paymentAmount.ToString();
            }
            if (costSheet != null && costSheet.FieldValues != null)
            {
                loadValues();
            }
            checkHideUnHideTransactions.IsChecked = true;
            LoadAddedCosts();
        }
        private void TxtTotalSystemCostSOC_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPOAmountSOC.Text))
            {
                //if (Convert.ToDecimal(txtPOAmountSOC.Text) != 0 /*&& Convert.ToDecimal(txtTotalSystemCostSOC.Text) != 0*/)
                //{
                    var difference = Convert.ToDecimal(txtPOAmountSOC.Text) - Convert.ToDecimal(txtTotalSystemCostSOC.Text);
                    txtDifference.Text = difference.ToString();
                //}
            }
        }
        public void loadValues()
        {
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            costSheetBillFields = new List<CostSheetBillField>();
            costSheetPOFields = new List<CostSheetPOField>();
            costSheetSIFields = new List<CostSheetSIField>();
            costSheetSOFields = new List<CostSheetSOField>();
            costSheetSaleReceiptFields = new List<CostSheetSaleReceiptField>();
            costSheetPaymentFields = new List<CostSheetPaymentField>();
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costfieldValues = grdCostItems.ItemsSource as List<CostFieldValues>;
            if (billId != 0 && costSheet.Id != 0)
            {
                costSheetBillFields = repo.GetCostSheetBillFields(billId, costSheet.Id);
            }
            
            if (purchaseOrderId != 0 && costSheet.Id != 0)
            {
                costSheetPOFields = repo.GetCostSheetPOFields(purchaseOrderId, costSheet.Id);
            }
            if (saleOrder.Id != 0 && costSheet.Id != 0)
            {
                costSheetSOFields = repo.GetCostSheetSOFields(saleOrder.Id, costSheet.Id);
            }
            if (receipt.Id != 0 && costSheet.Id != 0)
            {
                costSheetSaleReceiptFields = repo.GetCostSheetReceiptFields(receipt.Id, costSheet.Id);
            }
            if (payment.Id != 0 && costSheet.Id != 0)
            {
                costSheetPaymentFields = repo.GetCostSheetPaymentFields(payment.Id, costSheet.Id);
            }
            if (saleInvoiceId != 0 && costSheet.Id != 0)
            {
                costSheetSIFields = repo.GetCostSheetSIFields(saleInvoiceId, costSheet.Id);
            }
            foreach (var item in costfieldValues)
            {
                foreach (var field in costSheet.FieldValues)
                {
                    if (item.Id == field.FieldId)
                    {
                        if (field.Type == 2)
                            item.actualValue = field.Value;

                        else if (field.Type == 0)
                        {
                            item.revisedValue = field.Value;
                            if (item.revisedValue == 0)
                            {
                                item.revisedValue = item.budgetedValue;
                            }
                        }
                        else if (field.Type == 1)
                        {
                            item.budgetedValue = field.Value;
                            if (item.revisedValue == 0 || saleOrder.isApproved != true)
                            {
                                item.revisedValue = field.Value;
                            }
                        }
                       else if (field.Type == 3)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                var value = repo.GetSystemCost(costSheet.Id, item.Id);
                                // item.systemValue = field.Value;
                                if (item.systemValue == 0 || saleOrder.isApproved != true)
                                {
                                    if (item.systemValue == 0)
                                    {
                                        item.systemValue = value + field.adjSCost;
                                        item.adjSCost = field.adjSCost;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 4)
                        {
                            VendorRepo vendorRepo = new VendorRepo();
                            var vendor = vendorRepo.Get((int)field.Value);
                            item.vendor = vendor;
                        }
                        //else if (field.Type == 6)
                        //{
                        //    item.addedSystemValue = field.Value;
                        //    if (item.addedSystemValue == 0 || saleOrder.isApproved != true)
                        //    {
                        //        item.addedSystemValue = field.Value;
                        //    }
                        //}
                        else if (field.Type == 7)
                        {
                            if (costSheetBillFields.Count != 0)
                            {
                                var dbField = costSheetBillFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.Bill_Cost && x.CostSheetId == costSheet.Id && x.Bill_Id == billId);
                                if (dbField != null)
                                {
                                    var billFieldAmount = dbField.Value;
                                    if (billFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.billAmount=billFieldAmount;
                                        item.addedSystemValue = billFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 8)
                        {
                            if (costSheetPOFields.Count != 0)
                            {
                                var dbField = costSheetPOFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.PO_Cost && x.CostSheetId == costSheet.Id && x.PO_Id == purchaseOrderId);
                                if (dbField != null)
                                {
                                    var POFieldAmount = dbField.Value;
                                    if (POFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.poAmount = POFieldAmount;
                                        item.addedSystemValue = POFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 10)
                        {
                            if (costSheetSOFields.Count != 0)
                            {
                                var dbField = costSheetSOFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.soAmountSRBC && x.CostSheetId == costSheet.Id && x.SO_Id == saleOrder.Id);
                                if (dbField != null)
                                {
                                    var soFieldAmount = dbField.Value;
                                    if (soFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.soAmountSRBC = soFieldAmount;
                                        item.addedSRBCValue = soFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 11)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                var value = repo.GetSBRC(costSheet.Id, item.Id);
                                if (item.SRBC == 0 || saleOrder.isApproved != true)
                                {
                                    item.SRBC = value;
                                }
                            }
                        }
                        else if (field.Type == 12)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {

                                item.Maker = field.stringValue;

                            }
                        }
                        else if (field.Type == 13)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                item.Origin = field.stringValue;
                            }
                        }
                        else if (field.Type == 14)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {

                                item.Packing = field.isPacking;
                            }
                        }
                        else if (field.Type == 15)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                item.deliveryDate = field.dateValue;
                            }
                        }
                        else if (field.Type == 16)
                        {
                            PaymentTermRepo paymentTermRepo = new PaymentTermRepo();
                            var paymentTerm = paymentTermRepo.get((int)field.Value);
                            item.PaymentTerm = paymentTerm;
                        }
                        else if (field.Type == 17)
                        {
                            IncotermRepo incotermRepo = new IncotermRepo();
                            var incoterm = incotermRepo.get((int)field.Value);
                            IncoTermName incoTermName = new IncoTermName()
                            {
                                Id = incoterm.Id,
                                termName = incoterm.term,
                                discription = incoterm.discription,
                                isActive = incoterm.isActive
                            };

                            item.IncotermName = incoTermName;

                        }
                        else if (field.Type == 18)
                        {
                            ProcurementRepo repo = new ProcurementRepo();
                            var warranty = repo.GetWarranty((int)field.Value);

                            item.Warranty = warranty;
                        }
                        else if (field.Type == 19)
                        {
                            item.Dg_Goods = field.isdgGood;
                        }
                        else if (field.Type == 20)
                        {
                            CurrencyRepo currencyRepo = new CurrencyRepo();
                            var currency = currencyRepo.get((int)field.Value);
                            item.OC = currency;
                        }
                        else
                        if (field.Type == 21)
                            item.OCamount = field.Value;
                        else if (field.Type == 22)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                item.LoadingPort = field.stringValue;
                            }

                        }
                        else if (field.Type == 23)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                item.DestinationPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 24)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var document = repo.GetPQ((int)field.Value);
                                item.PQ = document;
                            }
                        }
                        else if (field.Type == 25)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var term = repo.GetST((int)field.Value);
                                item.ST = term;
                            }
                        }
                        else if (field.Type == 26)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                item.HScode = field.stringValue;
                            }
                        }
                        else if (field.Type == 27)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {

                                item.DrawaingRequired = field.drawingRequired;
                            }
                        }
                        else if (field.Type == 28)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                item.AttestedCOO = field.stringValue;
                            }
                        }
                        else if (field.Type == 29)
                        {
                            item.exchangeRate = field.Value;
                            if (item.exchangeRate == 0 || saleOrder.isApproved != true)
                            {
                                item.exchangeRate = field.Value;
                            }
                        }
                        else if (field.Type == 30)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {

                                item.isExportLicense = field.isExportLicense;
                            }
                        }
                        else if (field.Type == 31)
                        {
                            if (costSheetSaleReceiptFields.Count != 0)
                            {
                                var dbField = costSheetSaleReceiptFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.Receipt_Cost && x.CostSheetId == costSheet.Id && x.Receipt_Id == receipt.Id);
                                if (dbField != null)
                                {
                                    var ReceiptFieldAmount = dbField.Value;
                                    if (ReceiptFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.receiptAmount = ReceiptFieldAmount;
                                        item.addedSystemValue = ReceiptFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 32)
                        {
                            //if(field.Value!=0)
                            //{
                            //    item.addedSystemValue = field.Value;
                            //    item.paymentAmount = field.Value;
                            //}
                            if (costSheetPaymentFields.Count != 0)
                            {
                                var dbField = costSheetPaymentFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.System_Payment && x.CostSheetId == costSheet.Id && x.Payment_Id == payment.Id);
                                if (dbField != null)
                                {
                                    var paymentFieldAmount = dbField.Value;
                                    if (paymentFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.paymentAmount = paymentFieldAmount;
                                        item.addedSystemValue = paymentFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 33)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                item.IntermediaryPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 34)
                        {
                            if (costSheet != null && costSheet.Id != 0)
                            {
                                item.deliveryDays = field.stringValue;
                            }
                        }
                        //else if (field.Type == 35)
                        //{
                        //    if (costSheet != null && costSheet.Id != 0)
                        //    {
                        //        item.adjSCost = field.adjSCost;
                        //    }
                        //}
                        else if (field.Type == 36)
                        {
                            if (costSheetSIFields.Count != 0)
                            {
                                var dbField = costSheetSIFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.SI_Cost && x.CostSheetId == costSheet.Id && x.SI_Id == saleInvoiceId);
                                if (dbField != null)
                                {
                                    var SIFieldAmount = dbField.Value;
                                    if (SIFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.siAmount = SIFieldAmount;
                                        item.addedSystemValue = SIFieldAmount;
                                    }
                                }
                            }
                        }
                        //else if (field.Type == 37)
                        //{
                        //    if (costSheet != null && costSheet.Id != 0)
                        //    {
                        //        CompanyRepo companyRepo = new CompanyRepo();
                        //        item.vendorNature = companyRepo.GetVendorNature( (int)field.Value);
                        //    }
                        //}
                    }
                }
                costfields.Add(item);
            }
            grdCostItems.ItemsSource = costfields;
        }
        public void GetCostFieldValues()
        {
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            fieldValues = new List<FieldValue>();
            costfieldValues = grdCostItems.ItemsSource as List<CostFieldValues>;
            costSheetBillFields = new List<CostSheetBillField>();
            costSheetPOFields = new List<CostSheetPOField>();
            costSheetSOFields = new List<CostSheetSOField>();
            costSheetSIFields = new List<CostSheetSIField>();
            //if (costSheetPaymentFields.Count != grdCostItems.VisibleItems.Count)
            //{
                costSheetPaymentFields = new List<CostSheetPaymentField>();
            //}
            //if (costSheetSaleReceiptFields.Count != grdCostItems.VisibleItems.Count )
            //{
                costSheetSaleReceiptFields = new List<CostSheetSaleReceiptField>();
            //}
            //else
            //costSheetPaymentFields = new List<CostSheetPaymentField>();

            foreach (CostFieldValues item in costfieldValues)
            {
                //Field Values
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 1, Value = item.budgetedValue });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 2, Value = item.actualValue });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 0, Value = item.revisedValue });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 3, Value = item.systemValue, adjSCost = item.adjSCost });

                if (item.vendor != null)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 4, Value = item.vendor.Id });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 8, Value = item.poAmount });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 7, Value = item.billAmount });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 10, Value = item.soAmountSRBC });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 12, stringValue = item.Maker, Value = 0 });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 13, stringValue = item.Origin, Value = 0 });
                if (item.Packing != false)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 14, isPacking = item.Packing, Value = 0 });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 15, dateValue = item.deliveryDate, Value = 0, stringValue=null });
                if (item.PaymentTerm != null)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 16, Value = item.PaymentTerm.Id });
                if (item.IncotermName != null)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 17, Value = item.IncotermName.Id });
                if (item.Warranty != null)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 18, Value = item.Warranty.Id });
                if (item.Dg_Goods != false)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 19, isdgGood = item.Dg_Goods });
                if (item.OC != null)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 20, Value = item.OC.Id });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 21, Value = item.OCamount });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 22, stringValue = item.LoadingPort, Value = 0 });

                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 23, stringValue = item.DestinationPort, Value = 0 });
                if (item.PQ != null)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 24, Value = item.PQ.Id });
                if (item.ST != null)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 25, Value = item.ST.Id });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 26, stringValue = item.HScode, Value = 0 });
                if (item.DrawaingRequired != false)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 27, drawingRequired = item.DrawaingRequired, Value = 0 });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 28, stringValue = item.AttestedCOO, Value = 0 });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 29, Value = item.exchangeRate });
                if (item.isExportLicense != false)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 30, isExportLicense = item.isExportLicense, Value = 0 });
                //if (item.addedSystemValue != 0)
                //{
                //    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 32, Value = item.addedSystemValue });
                //}
               
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 31, Value = item.receiptAmount });
                
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 32, Value = item.paymentAmount });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 33, stringValue = item.IntermediaryPort, Value = 0 });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 34, stringValue = item.deliveryDays, Value = 0 });

                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 35, adjSCost = item.adjSCost });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 36, Value = item.siAmount });
                //if (item.vendorNature!=null)
                //    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 37, Value = item.vendorNature.Id });
                //Historyvalues
                fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Budgeted_Margin, Value = item.budgetedValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Actual_Margin, Value = item.actualValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Revised_Margin, Value = item.revisedValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Added_System_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                var rsbcValue = repo.GetSBRC(costSheet.Id, item.Id);
                if(rsbcValue!=item.addedSRBCValue)
                {
                    changedFields.Add(item);
                }


                // System Costing
                if (type == TransactionItemType.Purchase_Order && isBudgeted != true)
                {

                    if (rsbcValue > item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);

                        value = value + item.addedSystemValue;
                        var dbField = repo.GetPOField(purchaseOrderId, costSheet.Id, item.Id, CostFieldType.PO_Cost);
                        if (dbField != null)
                            value = value - dbField.Value;

                        if (value <= rsbcValue)
                        {
                            var poField = repo.GetPOField(purchaseOrderId, costSheet.Id, item.Id, CostFieldType.PO_Cost, item.addedSystemValue);
                            if (poField != null)
                            {
                                repo.UpdatePOField(purchaseOrderId, costSheet.Id, item.Id, CostFieldType.PO_Cost, item.addedSystemValue);
                            }
                            else
                                costSheetPOFields.Add(new CostSheetPOField { FieldId = item.Id, FieldType = CostFieldType.PO_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, PO_Id = purchaseOrderId });
                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.PO_Cost, Value = item.poAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                        }
                        else
                        {
                            DXMessageBox.Show("System cost is greater than R.S.B.C", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            checkFlag = false;
                            break;
                        }
                    }
                    else
                    if (rsbcValue < item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);
                        value = value + item.addedSystemValue;

                        var dbField = repo.GetPOField(purchaseOrderId, costSheet.Id, item.Id, CostFieldType.PO_Cost);


                        if (dbField != null)
                            value = value - dbField.Value;
                        if (value <= item.budgetedValue)
                        {
                            var poField = repo.GetPOField(purchaseOrderId, costSheet.Id, item.Id, CostFieldType.PO_Cost, item.addedSystemValue);
                            if (poField != null)
                            {
                                repo.UpdatePOField(purchaseOrderId, costSheet.Id, item.Id, CostFieldType.PO_Cost, item.addedSystemValue);
                            }
                            else
                                costSheetPOFields.Add(new CostSheetPOField { FieldId = item.Id, FieldType = CostFieldType.PO_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, PO_Id = purchaseOrderId });
                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.PO_Cost, Value = item.poAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                        }
                        else
                        {
                            DXMessageBox.Show("System cost is greater than budgeted cost", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            checkFlag = false;
                            break;
                        }
                    }

                }
                else 
                if (type == TransactionItemType.Sale_Invoice && isBudgeted != true)
                {

                    if (rsbcValue > item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);

                        value = value + item.addedSystemValue;
                        var dbField = repo.GetSIField(saleInvoiceId, costSheet.Id, item.Id, CostFieldType.SI_Cost);
                        if (dbField != null)
                            value = value - dbField.Value;

                        if (value <= rsbcValue)
                        {
                            var siField = repo.GetSIField(saleInvoiceId, costSheet.Id, item.Id, CostFieldType.SI_Cost, item.addedSystemValue);
                            if (siField != null)
                            {
                                repo.UpdateSIField(saleInvoiceId, costSheet.Id, item.Id, CostFieldType.SI_Cost, item.addedSystemValue);
                            }
                            else
                                costSheetSIFields.Add(new CostSheetSIField { FieldId = item.Id, FieldType = CostFieldType.SI_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, SI_Id = saleInvoiceId });
                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.SI_Cost, Value = item.siAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                        }
                        else
                        {
                            DXMessageBox.Show("System cost is greater than R.S.B.C", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            checkFlag = false;
                            break;
                        }
                    }
                    else
                    if (rsbcValue < item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);
                        value = value + item.addedSystemValue;

                        var dbField = repo.GetSIField(saleInvoiceId, costSheet.Id, item.Id, CostFieldType.SI_Cost);


                        if (dbField != null)
                            value = value - dbField.Value;
                        if (value <= item.budgetedValue)
                        {
                            var siField = repo.GetSIField(saleInvoiceId, costSheet.Id, item.Id, CostFieldType.SI_Cost, item.addedSystemValue);
                            if (siField != null)
                            {
                                repo.UpdateSIField(saleInvoiceId, costSheet.Id, item.Id, CostFieldType.SI_Cost, item.addedSystemValue);
                            }
                            else
                                costSheetSIFields.Add(new CostSheetSIField { FieldId = item.Id, FieldType = CostFieldType.SI_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, SI_Id = saleInvoiceId });
                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.SI_Cost, Value = item.siAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                        }
                        else
                        {
                            DXMessageBox.Show("System cost is greater than budgeted cost", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            checkFlag = false;
                            break;
                        }
                    }

                }
                else
                if (type == TransactionItemType.Sale_Receipt && isBudgeted != true)
                {
                    if (rsbcValue > item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);
                        value = value + item.addedSystemValue;
                        var dbField = repo.GetReceiptField(receipt, costSheet.Id, item.Id, CostFieldType.Receipt_Cost);
                        if (dbField != null)
                            value = value - dbField.Value;
                        if (value <= rsbcValue)
                        {
                            var receiptField = repo.GetReceiptField(receipt.Id, costSheet.Id, item.Id, CostFieldType.Receipt_Cost, item.addedSystemValue);
                            if (receiptField != null)
                            {
                                repo.UpdateReceiptField(receipt.Id, costSheet.Id, item.Id, CostFieldType.Receipt_Cost, item.addedSystemValue);
                            }
                            else
                                costSheetSaleReceiptFields.Add(new CostSheetSaleReceiptField { FieldId = item.Id, FieldType = CostFieldType.Receipt_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, Receipt_Id = receipt.Id });
                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Receipt_Cost, Value = item.receiptAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                        }
                        else
                        {
                            DXMessageBox.Show("System cost is greater than R.S.B.C", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            checkFlag = false;
                            break;
                        }
                    }
                    else
                    if (rsbcValue < item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);
                        value = value + item.addedSystemValue;

                        var dbField = repo.GetReceiptField(receipt, costSheet.Id, item.Id, CostFieldType.Receipt_Cost);

                        if (dbField != null)
                            value = value - dbField.Value;
                        if (value <= item.budgetedValue)
                        {
                            var receiptField = repo.GetReceiptField(receipt.Id, costSheet.Id, item.Id, CostFieldType.Receipt_Cost, item.addedSystemValue);
                            if (receiptField != null)
                            {
                                repo.UpdateReceiptField(receipt.Id, costSheet.Id, item.Id, CostFieldType.Receipt_Cost, item.addedSystemValue);
                            }
                            else
                                costSheetSaleReceiptFields.Add(new CostSheetSaleReceiptField { FieldId = item.Id, FieldType = CostFieldType.Receipt_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, Receipt_Id = receipt.Id });
                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Receipt_Cost, Value = item.poAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                        }
                        else
                        {
                            DXMessageBox.Show("System cost is greater than budgeted cost", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            checkFlag = false;
                            break;
                        }
                    }

                }
                else
                if (type == TransactionItemType.Payments && isBudgeted != true)
                {
                    if (rsbcValue > item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);
                        value = value + item.addedSystemValue;
                        var dbField = repo.GetPaymentField(payment, costSheet.Id, item.Id, CostFieldType.System_Payment);
                        if (dbField != null)
                            value = value - dbField.Value;
                        if (value <= rsbcValue)
                        {
                            var receiptField = repo.GetPaymentField(payment.Id, costSheet.Id, item.Id, CostFieldType.System_Payment, item.addedSystemValue);
                            if (receiptField != null)
                            {
                                repo.UpdatePaymentField(payment.Id, costSheet.Id, item.Id, CostFieldType.System_Payment, item.addedSystemValue);
                            }
                            else
                            {
                                costSheetPaymentFields.Add(new CostSheetPaymentField { FieldId = item.Id, FieldType = CostFieldType.System_Payment, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, Payment_Id = payment.Id });
                            }
                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.System_Payment, Value = item.paymentAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                        }
                        else
                        {
                            DXMessageBox.Show("System cost is greater than R.S.B.C", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            checkFlag = false;
                            break;
                        }
                    }
                    else
                    if (rsbcValue < item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);
                        value = value + item.addedSystemValue;

                        var dbField = repo.GetPaymentField(payment, costSheet.Id, item.Id, CostFieldType.System_Payment);

                        if (dbField != null)
                            value = value - dbField.Value;
                        if (value <= item.budgetedValue)
                        {
                            var paymentField = repo.GetPaymentField(payment.Id, costSheet.Id, item.Id, CostFieldType.System_Payment, item.addedSystemValue);
                            if (paymentField != null)
                            {
                                repo.UpdatePaymentField(payment.Id, costSheet.Id, item.Id, CostFieldType.System_Payment, item.addedSystemValue);
                            }
                            else
                            {
                                costSheetPaymentFields.Add(new CostSheetPaymentField { FieldId = item.Id, FieldType = CostFieldType.System_Payment, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, Payment_Id = payment.Id });
                            }
                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.System_Payment, Value = item.paymentAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });

                        }
                        else
                        {
                            DXMessageBox.Show("System cost is greater than budgeted cost", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            checkFlag = false;
                            break;
                        }
                    }

                }
                else
                if (isBudgeted != true)
                {
                    if (rsbcValue > item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);
                        value = value + item.addedSystemValue;
                        var dbField = repo.GetBillField(billId, costSheet.Id, item.Id, CostFieldType.Bill_Cost);
                        if (dbField != null)
                            value = value - dbField.Value;

                        if (value <= rsbcValue)
                        {
                            var billField = repo.GetBillField(billId, costSheet.Id, item.Id, CostFieldType.Bill_Cost, item.addedSystemValue);

                            if (billField != null)
                            {
                                //costSheetBillFields.Add(billField);
                                repo.UpdateBillField(billId, costSheet.Id, item.Id, CostFieldType.Bill_Cost, item.addedSystemValue);
                            }
                            else
                                costSheetBillFields.Add(new CostSheetBillField { FieldId = item.Id, FieldType = CostFieldType.Bill_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, Bill_Id = billId });

                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Bill_Cost, Value = item.billAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                        }
                        else
                        {
                            DXMessageBox.Show("Added System value is greater than RSBC Value", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            checkFlag = false;
                            break;
                        }
                    }
                    else
                    if (rsbcValue < item.budgetedValue)
                    {
                        var value = repo.GetSystemCost(costSheet.Id, item.Id);
                        value = value + item.addedSystemValue;

                        var dbField = repo.GetBillField(billId, costSheet.Id, item.Id, CostFieldType.Bill_Cost);
                        if (dbField != null)
                            value = value - dbField.Value;


                        if (value <= item.budgetedValue)
                        {
                            var billField = repo.GetBillField(billId, costSheet.Id, item.Id, CostFieldType.Bill_Cost, item.addedSystemValue);
                            if (billField != null)
                            {
                                //costSheetBillFields.Add(billField);
                                repo.UpdateBillField(billId, costSheet.Id, item.Id, CostFieldType.Bill_Cost, item.addedSystemValue);
                            }
                            else
                                costSheetBillFields.Add(new CostSheetBillField { FieldId = item.Id, FieldType = CostFieldType.Bill_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, Bill_Id = billId });

                            fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Bill_Cost, Value = item.billAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                        }
                        else
                        {


                            if (rsbcValue < 0)
                            {
                                var value1 = repo.GetSystemCost(costSheet.Id, item.Id);
                                value1 = value1 + item.addedSystemValue;

                                var dbField1 = repo.GetBillField(billId, costSheet.Id, item.Id, CostFieldType.Bill_Cost);
                                if (dbField1 != null)
                                    value1 = value1 - dbField1.Value;


                                if (value >= item.addedSRBCValue)
                                {
                                    var billField = repo.GetBillField(billId, costSheet.Id, item.Id, CostFieldType.Bill_Cost, item.addedSystemValue);
                                    if (billField != null)
                                    {
                                        //costSheetBillFields.Add(billField);
                                        repo.UpdateBillField(billId, costSheet.Id, item.Id, CostFieldType.Bill_Cost, item.addedSystemValue);
                                    }
                                    else
                                        costSheetBillFields.Add(new CostSheetBillField { FieldId = item.Id, FieldType = CostFieldType.Bill_Cost, Value = item.addedSystemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, Bill_Id = billId });

                                    fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Bill_Cost, Value = item.billAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                                }
                            }
                            else
                            {


                                DXMessageBox.Show("Added System value" + " " + item.addedSystemValue.ToString() + " is greater than budgeted Value", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                checkFlag = false;

                                break;
                            }
                        }
                    }
                }
                //Budget Costing
                else
                if (isBudgeted == true)
                {
                    var soField = repo.GetSOField(saleOrder.Id, costSheet.Id, item.Id, CostFieldType.soAmountSRBC, item.addedSRBCValue);
                    if (soField != null)
                    {
                        repo.UpdateSOField(saleOrder.Id, costSheet.Id, item.Id, CostFieldType.soAmountSRBC, item.addedSRBCValue);
                    }
                    else
                    {
                        costSheetSOFields.Add(new CostSheetSOField { FieldId = item.Id, FieldType = CostFieldType.soAmountSRBC, Value = item.addedSRBCValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id, SO_Id = saleOrder.Id });

                    }
                    fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.soAmountSRBC, Value = item.soAmountSRBC, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });


                }
                FieldValue systemCostfield = new FieldValue();
                systemCostfield.FieldId = item.Id;
                systemCostfield.Type = 3;
                systemCostfield.Value = item.systemValue;
                fieldValues.Add(systemCostfield);
                FieldValue SRBC = new FieldValue();
                SRBC.FieldId = item.Id;
                SRBC.Type = 11;
                SRBC.Value = item.SRBC;
                fieldValues.Add(SRBC);
                checkFlag = true;
                PurchaseOrderRepo poRepo = new PurchaseOrderRepo();
                Incoterm incoterm = new Incoterm();
                if (item.IncotermName != null)
                {
                    incoterm = IncotermRepo.get(item.IncotermName.Id);

                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public void PushComment(CommentLog Comment)
        {
            if (costSheet != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                if (costSheet.Id != 0)
                {
                    if (Comment!=null)
                    {
                        if (Comment.TaggedList != null || Comment.CCUsersList != null)
                        {
                            var commentId = procurementRepo.AddCommentLinkNotification(costSheet.Id, TransactionItemType.CostCenter, Comment, SYSTEM_STATIC.currentUser.employeeId, Comment.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in CostSheet Having SO Reference #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, Comment.Comment, user.id, "New Comment ", Comment.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in CostSheet Having SO Reference #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, Comment.Comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in CostSheet Having SO Reference #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, Comment.Comment, 0, user.id, "New Comment ", Comment.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in CostSheet Having SO Reference #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, Comment.Comment, 0, user.id, "New Comment ", null, commentId.Value);
                                }
                            }
                        }
                        MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        if (window1 != null) { window1.UrgentNotificationGlow(); }
                    }
                }

            }
        }
        private void CheckHideUnHideTransactions_Checked(object sender, RoutedEventArgs e)
        {
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costfieldCheckedValues = grdCostItems.ItemsSource as List<CostFieldValues>;
            //costfields = costfieldCheckedValues.Where(x => x.actualValue > 0||x.SRBC>0  || x.budgetedValue > 0 || x.revisedValue > 0 || /*x.systemPayment > 0 ||*/ x.systemValue != 0 || x.vendor != null || x.billAmount > 0).ToList();
            costfields = costfieldCheckedValues.Where(x => x.actualValue != 0 || x.SRBC != 0 || x.budgetedValue != 0 || x.revisedValue != 0 /*&& x.systemPayment != 0*/ && x.systemValue <= 0 && x.vendor == null).ToList();

            grdCostItems.ItemsSource = costfields;
        }

        private void CheckHideUnHideTransactions_Unchecked(object sender, RoutedEventArgs e)
        {
            if(costfieldCheckedValues.Count!=0)
            {
                grdCostItems.ItemsSource = costfieldCheckedValues;
            }

        }

        private void CheckHideActualCost_Checked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("actualValue").Visible = false;

        }

        private void CheckHideActualCost_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("actualValue").Visible = true;

        }

        private void TxtTotalSRBC_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if(isBudgeted!=true)
            {
                if (Convert.ToDecimal(txtPOAmountSOC.Text) != 0 /*&& Convert.ToDecimal(txtTotalSystemCostSOC.Text) != 0*/)
                {
                    var difference = Convert.ToDecimal(txtPOAmountSOC.Text) - Convert.ToDecimal(txtTotalSRBC.Text);
                    txtDifference.Text = difference.ToString();
                }
            }
            
        }

        private void View_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "addedSystemValue" && saleOrder.Id != 0)
            {
                if (isBudgeted != true)
                {
                    decimal total = 0;
                    var selectedItems = grdCostItems.ItemsSource as List<CostFieldValues>;
                    foreach (CostFieldValues item in selectedItems)
                    {
                        total = total + item.addedSystemValue;
                    }
                    txtTotalSystemCostSOC.Text = total.ToString();
                }
               
                return;
            }
            else
            if (e.Column.FieldName == "addedSRBCValue" && saleOrder.Id != 0)
            {
                decimal total = 0;
                var selectedItems = grdCostItems.ItemsSource as List<CostFieldValues>;
                foreach (CostFieldValues item in selectedItems)
                {
                    total = total + item.addedSRBCValue;
                }
                txtTotalSRBC.Text = total.ToString();
                return;
            }
        }

        private void CheckHideRevisedCost_Checked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Visible = false;

        }

        private void CheckHideRevisedCost_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Visible = true;

        }

        private void CheckHideSystemCost_Checked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("systemValue").Visible = false;
        }

        private void CheckHideSystemCost_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("systemValue").Visible = true;
        }
       public void LoadAddedCosts()
        {
            decimal total = 0;

            var selectedItems = grdCostItems.ItemsSource as List<CostFieldValues>;
            foreach (CostFieldValues item in selectedItems)
            {
                total = total + item.addedSystemValue;
            }
            txtTotalSystemCostSOC.Text = total.ToString();
        }

        private void DXWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (isBudgeted != true)
            {
                if (payment.Id != 0)
                {
                    if (string.IsNullOrEmpty(txtDifference.Text) || Convert.ToDecimal(txtDifference.Text) == 0)
                    {
                        GetCostFieldValues();
                        if (checkFlag == true)
                        {
                            costSheet.FieldValues = fieldValues;
                            if (fieldHistoryValues != null)
                            {
                                costSheet.CostFieldHistories = fieldHistoryValues;
                            }
                            if (costSheetBillFields != null && costSheetBillFields.Count != 0)
                            {
                                costSheet.CostSheetBillFields = costSheetBillFields;
                            }
                            if (costSheetPOFields != null && costSheetPOFields.Count != 0)
                            {
                                costSheet.CostSheetPOFields = costSheetPOFields;
                            }
                            if (costSheetSaleReceiptFields != null && costSheetSaleReceiptFields.Count != 0)
                            {
                                costSheet.CostSheetSaleReceiptFields = costSheetSaleReceiptFields;
                            }
                            if (costSheetPaymentFields != null && costSheetPaymentFields.Count != 0)
                            {
                                costSheet.CostSheetPaymentFields = costSheetPaymentFields;
                            }
                        
                        }
                        else
                            return;
                    }
                    else
                    {
                        DXMessageBox.Show("There is a difference between " + "Added system cost total" + " " + "and" + " " + "PO amount (SOC)", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                }
                else
                if (receipt.Id != 0)
                {
                    if (string.IsNullOrEmpty(txtDifference.Text) || Convert.ToDecimal(txtDifference.Text) == 0)
                    {
                        GetCostFieldValues();
                        if (checkFlag == true)
                        {
                            costSheet.FieldValues = fieldValues;
                            if (fieldHistoryValues != null)
                            {
                                costSheet.CostFieldHistories = fieldHistoryValues;
                            }
                            if (costSheetBillFields != null && costSheetBillFields.Count != 0)
                            {
                                costSheet.CostSheetBillFields = costSheetBillFields;
                            }
                            if (costSheetPOFields != null && costSheetPOFields.Count != 0)
                            {
                                costSheet.CostSheetPOFields = costSheetPOFields;
                            }
                            if (costSheetSaleReceiptFields != null && costSheetSaleReceiptFields.Count != 0)
                            {
                                costSheet.CostSheetSaleReceiptFields = costSheetSaleReceiptFields;
                            }
                            if (costSheetPaymentFields != null && costSheetPaymentFields.Count != 0)
                            {
                                costSheet.CostSheetPaymentFields = costSheetPaymentFields;
                            }
                           
                        }
                        else
                            return;
                    }
                    else
                    {
                        DXMessageBox.Show("There is a difference between " + "Added system cost total" + " " + "and" + " " + "PO amount (SOC)", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                }
                else
                if (string.IsNullOrEmpty(txtDifference.Text) || Convert.ToDecimal(txtDifference.Text) == 0  )
                {
                    GetCostFieldValues();


                    if (checkFlag == true)
                    {

                        costSheet.FieldValues = fieldValues;
                        if (fieldHistoryValues != null)
                        {
                            costSheet.CostFieldHistories = fieldHistoryValues;
                        }
                        if (costSheetBillFields != null && costSheetBillFields.Count != 0)
                        {
                            costSheet.CostSheetBillFields = costSheetBillFields;
                        }
                        if (costSheetPOFields != null && costSheetPOFields.Count != 0)
                        {
                            costSheet.CostSheetPOFields = costSheetPOFields;
                        }
                        if (costSheetSIFields != null && costSheetSIFields.Count != 0)
                        {
                            costSheet.CostSheetSIFields = costSheetSIFields;
                        }

                    }

                    else
                        return;
                }

                else
                {
                    DXMessageBox.Show("There is a difference between " + "Added system cost total" + " " + "and" + " " + "PO amount (SOC)", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }


            }
            else
            if (isBudgeted == true)
            {
                GetCostFieldValues();

                if (checkFlag == true)
                {
                    costSheet.FieldValues = fieldValues;
                    if (fieldHistoryValues != null)
                    {
                        costSheet.CostFieldHistories = fieldHistoryValues;
                    }
                    if (costSheetBillFields != null && costSheetBillFields.Count != 0)
                    {
                        costSheet.CostSheetBillFields = costSheetBillFields;
                    }
                    if (costSheetPOFields != null && costSheetPOFields.Count != 0)
                    {
                        costSheet.CostSheetPOFields = costSheetPOFields;
                    }
                    if (costSheetSOFields != null && costSheetSOFields.Count != 0)
                    {
                        costSheet.CostSheetSOFields = costSheetSOFields;
                    }
                    if (costSheetSaleReceiptFields != null && costSheetSaleReceiptFields.Count != 0)
                    {
                        costSheet.CostSheetSaleReceiptFields = costSheetSaleReceiptFields;
                    }
                 
                }
                if (isBudgeted == true)
                {
                    List<User> tagUsers = new List<User>();
                    List<User> ccUsers = new List<User>();
                    List<User> tagUsersRecommendation = new List<User>();
                    List<User> ccUsersRecommendation = new List<User>();
                    UsersRepo usersRepo = new UsersRepo();
                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    var res = MessageBox.Show("RSBC value has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(usersRepo.getusersByCompanyDepartment((int)saleOrder.department.Id, (int)saleOrder.company.Id), costSheet.Id, TransactionItemType.CostCenter);

                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            ccUsers = win.ccUsers;
                            tagUsersRecommendation = win.tagRecommendationUsers;
                            ccUsersRecommendation = win.ccRecommendationUsers;
                            Currency commentCurrency = new Currency();
                            if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                commentCurrency = saleOrder.costcenterCurrency;
                            }
                            else
                            {
                                commentCurrency = saleOrder.currency;
                            }

                            CommentLog comment = new CommentLog();


                            foreach (var changedField in changedFields)
                            {
                                if (!string.IsNullOrEmpty(comment.Comment))
                                {
                                    comment.Comment += "+\n"+"Under the head " + "RSBC " + "(" + changedField.Title + ")" + "\n"
                                                + "(" + "Revised System Budget Cost has been changed" + ")" + "\n"
                                                + "From: " + changedField.SRBC.ToString() + " (" + commentCurrency.Symbol + ")"
                                                + "\nTo: " + changedField.addedSRBCValue.ToString() + " (" + commentCurrency.Symbol + ")";
                                }
                                else
                                {
                                    comment.Comment = "Under the head " + "RSBC " + "(" + changedField.Title + ")" + "\n"
                                                + "(" + "Revised System Budget Cost has been changed" + ")" + "\n"
                                                + "From: " + changedField.SRBC.ToString() + " (" + commentCurrency.Symbol + ")"
                                                + "\nTo: " + changedField.addedSRBCValue.ToString() + " (" + commentCurrency.Symbol + ")";
                                }
                            }
                            comment.Timestamp = DateTime.Now;
                            comment.Subject = "RSBC Value Changed";
                            comment.TaggedList = tagUsers;
                            comment.CCUsersList = ccUsers;
                            comment.TaggedRecomenndedList = tagUsersRecommendation;
                            comment.CCRecomenndedList = ccUsersRecommendation;
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in CostSheet having SO Reference #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, comment.Comment, user.id, "New Comment ", null);
                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {


                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in CostSheet having SO Reference #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                            //if (saleOrder.InterBankTransfer != null)
                            //{
                            //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartmentIdsList(new List<int> { saleOrder.department.Id, saleOrder.InterDepartment.Id }, new List<int> { saleOrder.company.Id, saleOrder.InterCompany.Id }), TransactionItemType.CostCenter);
                            //    inputBox.ShowDialog();
                            //}
                            //else
                            //{
                            //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartmentIdsList(new List<int> { saleOrder.department.Id }, new List<int> { saleOrder.company.Id }), TransactionItemType.CostCenter);
                            //    inputBox.ShowDialog();
                            //}
                            PushComment(comment);
                            usersRepo.Add(TransactionInfo.Edited, costSheet.Id, 8, comment.Comment);
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }
                }
            }
            else
                return;
        }
    }
}