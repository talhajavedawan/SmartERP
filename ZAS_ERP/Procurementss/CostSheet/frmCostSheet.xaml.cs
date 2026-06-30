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
using ERP_BL.Config;
using System.Printing;


using ERP_BL.Enums;
using System.IO;
using System.Windows.Xps.Packaging;
using System.IO.Packaging;
using System.Windows.Xps.Serialization;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Data;
using ZAS_ERP.Procurementss.PurchaseOrderss;
using ERP_BL.Payments;
using ZAS_ERP.Procurementss.CostSheet;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Bankings;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.Procurementss.CostSheet.UserControls;
using System.Runtime.CompilerServices;
using DevExpress.Utils.Extensions;

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for frmCostSheet.xaml
    /// </summary>
    public partial class frmCostSheet : DXWindow
    {
        public InquiryType costsheetType;
        SaleOrder saleOrder = new SaleOrder();
        Offer offer = new Offer();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        UsersRepo usersRepo = new UsersRepo();
        Bill bill = new Bill();
        PurchaseOrder purchaseOrder = new PurchaseOrder();
        SaleInvoice saleInvoice = new SaleInvoice();
        public List<CostSheetBillField> costSheetBillFields = new List<CostSheetBillField>();
        public List<CostSheetPOField> costSheetPOFields = new List<CostSheetPOField>();
        public List<CostSheetSIField> costSheetSIFields = new List<CostSheetSIField>();
        public List<CostSheetSOField> costSheetSOFields = new List<CostSheetSOField>();
        public List<CostSheetSaleReceiptField> costSheetSaleReceiptFields = new List<CostSheetSaleReceiptField>();
        public List<CostSheetPaymentField> costSheetPaymentFields = new List<CostSheetPaymentField>();
        List<CostAdjustedFieldValues> changedAdjustedFields = new List<CostAdjustedFieldValues>();
        List<CostBudgetFieldValues> changedBudgetFields = new List<CostBudgetFieldValues>();
        List<CostVendorFieldValues> changedVendorFields = new List<CostVendorFieldValues>();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        List<PendingOrder> allPendingInvoicesOpen = new List<PendingOrder>(); 
        List<PendingOrder> allPendingInvoicesClose = new List<PendingOrder>();
        List<PendingOrder> allPendingInvoicesAll = new List<PendingOrder>();
        List<PendingOrder> allPendingInvoicesPendingforApproval = new List<PendingOrder>();


        SalesReceipt salesReceipt = new SalesReceipt();
        public static Payment payment { get; set; }

        List<ViewInfo> views = new List<ViewInfo>();
        string logValue = "";
        decimal sum = 0;
        IncotermRepo IncotermRepo = new IncotermRepo();
        SaleOrder trackingOrder = new SaleOrder();

        public frmCostSheet()
        {
            InitializeComponent();
        }
        public frmCostSheet(Offer _offer)
        {
            InitializeComponent();

            offer = _offer;
            if (offer != null)
            {
                lblCustomer.Text = offer.customerCompany.company.CompanyName;
                lblDepartment.Text = offer.department.DeptName;
                lblSalesRefrence.Text = "Offer Ref";
                txtSalesRefrence.Text = offer.offerReferenceNo;
                txtSOTotal.Text = offer.totalCFRValue.ToString();
                lblSOAmount.Text = "Offer Amount";
                txtExchangeRate.Text = offer.exchngeRate.ToString();
                txtSOTotal.Text = offer.totalOfferAmount.ToString();

                loadWarrantys();
                loadPaymentTerms();
                loadIncoterms();
                txtSOTotal.Text = saleOrder.totalCFRValue.ToString();
                txtSOAmount.Text = saleOrder.totalCFRValue.ToString();

                if (offer != null)
                {
                    if (offer.totalCFRValue != 0)
                    {
                        txtSOTotal.Text = offer.totalCFRValue.ToString();
                    }
                    else
                    {
                        txtSOTotal.Text = offer.totalCFRValue.ToString();

                    }
                    if (offer.costCenterCurrencyId != null)
                    {
                        txtCostSheetCurrency.Text = offer.CostCenterCurrency.CurrencyName + " " + "(" + offer.CostCenterCurrency.Symbol + ")";
                    }
                    if (offer.SER != 0)
                    {
                        txtExchangeRate.Text = offer.SER.ToString();
                    }
                    else
                    {
                        txtExchangeRate.Text = 1.ToString();

                    }
                    //if (offer.customerCompany != null)
                    {

                        this.offer = offer;
                        lblCustomer.Text = offer.customerCompany.company.CompanyName;
                        if (offer.principal.company != null)
                            lblPrincipal.Text = offer.principal.company.CompanyName;
                        lblDepartment.Text = offer.department.DeptName;
                        txtSalesRefrence.Text = "Cost Sheet( Ref " + offer.SalesReferenceNo + ")";
                        lblFinRef.Text = "Fin Ref# " + offer.offerReferenceNo;
                        lblPrintDate.Text = "Print Date:" + System.DateTime.Now.ToShortDateString();
                        lblSalesRefrence.Text = offer.offerReferenceNo;
                        isApproved = offer.isApproved;
                        //isReApproved = offer.reap;
                        if (offer.CreationDate != null)
                            lblSaleOrderDate.Text = offer.CreationDate.Value.ToShortDateString();
                        else
                            lblSaleOrderDate.Text = (System.DateTime.Now).ToLongDateString();


                        if (offer.CreationDate != null)
                            lblCreationDate.Text = offer.CreationDate.Value.ToLongDateString();
                        else
                            lblCreationDate.Text = (System.DateTime.Now).ToLongDateString();

                        {
                            costsheetType = offer.offertype;

                            if (offer.currency != null)
                            {
                                txtCurrency.Text = offer.currency.CurrencyName + " " + "(" + offer.currency.Symbol + ")";
                            }
                        }

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
                        {
                            grdCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;

                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Advance Costs") == null)
                        {
                            grdCostItems.Columns.GetColumnByFieldName("advanceValue").ReadOnly = true;

                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
                        {
                            grdCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
                        {
                            grdCostItems.Columns.GetColumnByFieldName("actualValue").ReadOnly = true;

                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
                        {
                            grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = false;

                        }
                        else
                        {
                            grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = true;

                        }
                        if (offer.CostSheet != null)
                            costSheet = offer.CostSheet;
                        else
                            costSheet = new ERP_BL.Databases.CostSheet();
                    }
                    //loadViewInfos(offer,  viewinfos);
                    //loadCommentsList(offer, viewinfos);

                }


            }
        }
        public frmCostSheet(SaleOrder saleOrder, Offer offer, string FinanceRefrenceNo, string SalesReferenceNo, string department, string customer, string currency, string totalvalue, string incoterm, string Creationdate, string paymenttermcustomer, string maker, string origin, List<ViewInfo> viewinfos, InquiryType type, string principle, string Packing, DateTime SodeliveryDate, string Warranty, String SOref, string SODate, bool? approved, bool? reApproved)
        {

            InitializeComponent();
            loadWarrantys();
            loadPaymentTerms();
            loadIncoterms();
            txtSOTotal.Text = saleOrder.totalCFRValue.ToString();
            txtSOAmount.Text = saleOrder.totalCFRValue.ToString();

            if (saleOrder != null)
            {
                //if (saleOrder.customerCompany != null)
                {
                    if (saleOrder.costCenterAmount != 0)
                    {
                        txtSOTotal.Text = saleOrder.costCenterAmount.ToString();
                    }
                    else
                    {
                        txtSOTotal.Text = saleOrder.totalCFRValue.ToString();

                    }
                    if (saleOrder.costCentercurrency_Id != null)
                    {
                        txtCostSheetCurrency.Text = saleOrder.costcenterCurrency.CurrencyName + " " + "(" + saleOrder.costcenterCurrency.Symbol + ")";
                    }
                    if (saleOrder.exchangeRate != 0)
                    {
                        txtExchangeRate.Text = saleOrder.costCenterExchangeRate.ToString();
                    }
                    else
                    {
                        txtExchangeRate.Text = 1.ToString();

                    }
                    this.saleOrder = saleOrder;
                    lblCustomer.Text = customer;
                    lblPrincipal.Text = principle;
                    lblDepartment.Text = department;
                    txtSalesRefrence.Text = "Cost Sheet( Ref " + SalesReferenceNo + ")";
                    lblFinRef.Text = "Fin Ref# " + FinanceRefrenceNo;
                    lblPrintDate.Text = "Print Date:" + System.DateTime.Now.ToShortDateString();
                    lblSaleOrder.Text = SOref;
                    isApproved = approved;
                    isReApproved = reApproved;
                    if (SODate != "")
                        lblSaleOrderDate.Text = SODate;
                    else
                        lblSaleOrderDate.Text = (System.DateTime.Now).ToLongDateString();


                    if (Creationdate != "")
                        lblCreationDate.Text = Creationdate;
                    else
                        lblCreationDate.Text = (System.DateTime.Now).ToLongDateString();
                    if (type == InquiryType.Principal)
                    {
                        costsheetType = type;

                        if (saleOrder.currency != null)
                        {
                            txtCurrency.Text = saleOrder.currency.CurrencyName + " " + "(" + saleOrder.currency.Symbol + ")";
                        }
                        else
                        {
                        }
                        //lbtotal.Text = "Total Costs";
                        //lblgrosstotal.Text = "Total Commission";

                        //txtActualTotal.Visibility = Visibility.Collapsed;
                        //txtActualmarginTotal.Visibility = Visibility.Collapsed;
                        grdCostItems.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        costsheetType = type;

                        if (saleOrder.currency != null)
                        {
                            txtCurrency.Text = saleOrder.currency.CurrencyName + " " + "(" + saleOrder.currency.Symbol + ")";
                        }
                        else
                        {
                        }
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = false;

                }
                else
                {
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = true;

                }
                if (saleOrder.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Cost Center after SO Approval") == null)
                {
                    grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    //lookupVendors.IsEnabled = false;
                    //lookupOC.IsEnabled = false;
                }
                if (saleOrder.CostSheet != null)
                    costSheet = saleOrder.CostSheet;
            }
            loadViewInfos(saleOrder, viewinfos);
            loadCommentsList(saleOrder, viewinfos);
            //CalculateTotal();

        }

        public frmCostSheet( ERP_BL.Procurements.ModuleContract moduleContract)
        {
            InitializeComponent();
            if (moduleContract != null)
            {
            
                lblCustomer.Text = moduleContract.customerCompany.company.CompanyName;
                lblDepartment.Text = moduleContract.department.DeptName;
                txtSalesRefrence.Text = moduleContract.SalesReferenceNo;
                txtSOTotal.Text = moduleContract.totalCFRValue.ToString();
                txtSOAmount.Text = moduleContract.totalCFRValue.ToString();
            }
            //grpDocuments.Visibility = Visibility.Visible;
            //grpReviewDetails.Visibility = Visibility.Visible;
            //grpTerms.Visibility = Visibility.Visible;
        }
        public frmCostSheet(SaleOrder saleOrder, string currency, string incoterm, string Creationdate, string paymenttermcustomer, string maker, string origin, List<ViewInfo> viewinfos, InquiryType type, string Packing, string Warranty, bool? approved, bool? reApproved)
        {

            InitializeComponent();
            loadWarrantys();
            loadPaymentTerms();
            loadIncoterms();
            txtSOTotal.Text = saleOrder.totalCFRValue.ToString();
            txtSOAmount.Text = saleOrder.totalCFRValue.ToString();

            if (saleOrder != null)
            {
                if (saleOrder.costCenterAmount != 0)
                {
                    txtSOTotal.Text = saleOrder.costCenterAmount.ToString();
                }
                else
                {
                    txtSOTotal.Text = saleOrder.totalCFRValue.ToString(); 

                }
                if (saleOrder.costCentercurrency_Id != null)
                {
                    txtCostSheetCurrency.Text = saleOrder.costcenterCurrency.CurrencyName + " " + "(" + saleOrder.costcenterCurrency.Symbol + ")";
                }
                if (saleOrder.exchangeRate != 0)
                {
                    txtExchangeRate.Text = saleOrder.costCenterExchangeRate.ToString();
                }
                else
                {
                    txtExchangeRate.Text = 1.ToString();

                }
                //if (saleOrder.customerCompany != null)
                {
                   
                    this.saleOrder = saleOrder;
                    lblCustomer.Text = saleOrder.customerCompany.company.CompanyName;
                    if(saleOrder.principal.company!=null)
                    lblPrincipal.Text = saleOrder.principal.company.CompanyName;
                    lblDepartment.Text = saleOrder.department.DeptName;
                    txtSalesRefrence.Text = "Cost Sheet( Ref " + saleOrder.SalesReferenceNo + ")";
                    lblFinRef.Text = "Fin Ref# " + saleOrder.FinanceRefrenceNo;
                    lblPrintDate.Text = "Print Date:" + System.DateTime.Now.ToShortDateString();
                    lblSaleOrder.Text = saleOrder.referenceNo;
                    isApproved = approved;
                    isReApproved = reApproved;
                    if (saleOrder.saleOrderDate != null)
                        lblSaleOrderDate.Text = saleOrder.saleOrderDate.Value.ToShortDateString();
                    else
                        lblSaleOrderDate.Text = (System.DateTime.Now).ToLongDateString();


                    if (Creationdate != "")
                        lblCreationDate.Text = saleOrder.CreationDate.Value.ToLongDateString();
                    else
                        lblCreationDate.Text = (System.DateTime.Now).ToLongDateString();

                    {
                        costsheetType = type;

                        if (saleOrder.currency != null)
                        {
                            txtCurrency.Text = saleOrder.currency.CurrencyName + " " + "(" + saleOrder.currency.Symbol + ")";
                        }
                    }

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;

                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("actualValue").ReadOnly = true;

                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
                    {
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = false;

                    }
                    else
                    {
                        grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = true;

                    }
                    if (saleOrder.CostSheet != null)
                        costSheet = saleOrder.CostSheet;
                }
                loadViewInfos(saleOrder, viewinfos);
                loadCommentsList(saleOrder, viewinfos);
                //CalculateTotal();

            }
        }
        public frmCostSheet(Bill _bill, string currency, string incoterm, string Creationdate, string paymenttermcustomer, string maker, string origin, List<ViewInfo> viewinfos, InquiryType type, string Packing, string Warranty, bool? approved, bool? reApproved)
        {

            InitializeComponent();
            loadWarrantys();
            loadPaymentTerms();
            loadIncoterms();
            txtSOTotal.Text = saleOrder.totalCFRValue.ToString();
            txtSOAmount.Text = saleOrder.totalCFRValue.ToString();

            if (saleOrder != null)
            {
                if (saleOrder.costCenterAmount != 0)
                {
                    txtSOTotal.Text = saleOrder.costCenterAmount.ToString();
                }
                else
                {
                    txtSOTotal.Text = saleOrder.totalCFRValue.ToString();

                }
                if (saleOrder.costCentercurrency_Id != null)
                {
                    txtCostSheetCurrency.Text = saleOrder.costcenterCurrency.CurrencyName + " " + "(" + saleOrder.costcenterCurrency.Symbol + ")";
                }
                if (saleOrder.exchangeRate != 0)
                {
                    txtExchangeRate.Text = saleOrder.costCenterExchangeRate.ToString();
                }
                else
                {
                    txtExchangeRate.Text = 1.ToString();

                }
                //if (_bill.customerCompany != null)
                {
                    this.bill = _bill;
                    lblCustomer.Text = _bill.customerCompany.company.CompanyName;
                    //lblPrincipal.Text = _bill.principal.company.CompanyName;
                    lblDepartment.Text = _bill.department.DeptName;
                    txtSalesRefrence.Text = "Cost Sheet( Ref " + _bill.SalesReferenceNo + ")";
                    lblFinRef.Text = "Fin Ref# " + _bill.FinanceRefrenceNo;
                    lblPrintDate.Text = "Print Date:" + System.DateTime.Now.ToShortDateString();
                    lblSaleOrder.Text = _bill.POReferenceNo;
                    isApproved = approved;
                    isReApproved = reApproved;
                    if (_bill.saleOrderDate != null)
                        lblSaleOrderDate.Text = _bill.saleOrderDate.Value.ToShortDateString();
                    else
                        lblSaleOrderDate.Text = (System.DateTime.Now).ToLongDateString();


                    if (Creationdate != "")
                        lblCreationDate.Text = _bill.CreationDate.Value.ToLongDateString();
                    else
                        lblCreationDate.Text = (System.DateTime.Now).ToLongDateString();
                    if (type == InquiryType.Principal)
                    {
                        costsheetType = type;
                      
                        if (_bill.currency != null)
                        {
                            txtCurrency.Text = _bill.currency.CurrencyName + " " + "(" + _bill.currency.Symbol + ")";
                        }
                        //lbtotal.Text = "Total Costs";
                        //lblgrosstotal.Text = "Total Commission";
                        //txtActualTotal.Visibility = Visibility.Collapsed;
                        //txtActualmarginTotal.Visibility = Visibility.Collapsed;
                        grdCostItems.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        costsheetType = type;

                        if (_bill.currency != null)
                        {
                            txtCurrency.Text = _bill.currency.CurrencyName + " " + "(" + _bill.currency.Symbol + ")";
                        }
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = false;

                }
                else
                {
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = true;

                }
                if (_bill.CostSheet != null)
                    costSheet = _bill.CostSheet;
            }
            loadViewInfos(_bill, viewinfos);
            loadCommentsList(_bill, viewinfos);
            //CalculateTotal();

        }
        public frmCostSheet(PurchaseOrder purchaseOrder, string currency, string incoterm, string Creationdate, string paymenttermcustomer, string maker, string origin, List<ViewInfo> viewinfos, InquiryType type, string Packing, string Warranty, bool? approved, bool? reApproved)
        {

            InitializeComponent();
            loadWarrantys();
            loadPaymentTerms();
            loadIncoterms();
            txtSOTotal.Text = purchaseOrder.totalCFRValue.ToString();
            txtSOAmount.Text = purchaseOrder.totalCFRValue.ToString();

            if (purchaseOrder != null)
            {
                if (purchaseOrder.SaleOrder != null)
                {
                    if (purchaseOrder.SaleOrder.costCenterAmount != 0)
                    {
                        txtSOTotal.Text = saleOrder.costCenterAmount.ToString();
                    }
                    else
                    {
                        txtSOTotal.Text = purchaseOrder.totalCFRValue.ToString();

                    }
                    if (purchaseOrder.SaleOrder.costCentercurrency_Id != null)
                    {
                        txtCostSheetCurrency.Text = purchaseOrder.SaleOrder.costcenterCurrency.CurrencyName + " " + "(" + purchaseOrder.SaleOrder.costcenterCurrency.Symbol + ")";
                    }
                    if (purchaseOrder.SaleOrder.exchangeRate != 0)
                    {
                        txtExchangeRate.Text = purchaseOrder.SaleOrder.costCenterExchangeRate.ToString();
                    }
                    else
                    {
                        txtExchangeRate.Text = 1.ToString();

                    }
                }
                
                
                //if (purchaseOrder.customerCompany != null)
                {
                    this.purchaseOrder = purchaseOrder;
                    lblCustomer.Text = purchaseOrder.customerCompany.company.CompanyName;
                    lblDepartment.Text = purchaseOrder.department.DeptName;
                    txtSalesRefrence.Text = "Cost Sheet( Ref " + purchaseOrder.SalesReferenceNo + ")";
                    lblFinRef.Text = "Fin Ref# " + purchaseOrder.FinanceRefrenceNo;
                    lblPrintDate.Text = "Print Date:" + System.DateTime.Now.ToShortDateString();
                    lblSaleOrder.Text = purchaseOrder.POReferenceNo;
                    isApproved = approved;
                    isReApproved = reApproved;
                    if (purchaseOrder.saleOrderDate != null)
                        lblSaleOrderDate.Text = purchaseOrder.saleOrderDate.Value.ToShortDateString();
                    else
                        lblSaleOrderDate.Text = (System.DateTime.Now).ToLongDateString();


                    if (Creationdate != "")
                        lblCreationDate.Text = purchaseOrder.CreationDate.Value.ToLongDateString();
                    else
                        lblCreationDate.Text = (System.DateTime.Now).ToLongDateString();
                    if (type == InquiryType.Principal)
                    {
                        costsheetType = type;
                    
                        if (purchaseOrder.currency != null)
                        {
                            txtCurrency.Text = purchaseOrder.currency.CurrencyName + " " + "(" + purchaseOrder.currency.Symbol + ")";
                        }
                        //lbtotal.Text = "Total Costs";
                        //lblgrosstotal.Text = "Total Commission";
                        //txtActualTotal.Visibility = Visibility.Collapsed;
                        //txtActualmarginTotal.Visibility = Visibility.Collapsed;
                        grdCostItems.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        costsheetType = type;

                        if (purchaseOrder.currency != null)
                        {

                            txtCurrency.Text = purchaseOrder.currency.CurrencyName + " " + "(" + purchaseOrder.currency.Symbol + ")";
                        }
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = false;

                }
                else
                {
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = true;

                }
                if (purchaseOrder.CostSheet != null)
                    costSheet = purchaseOrder.CostSheet;
            }
            loadViewInfos(purchaseOrder.SaleOrder, viewinfos);
            loadCommentsList(purchaseOrder.SaleOrder, viewinfos);
            //CalculateTotal();

        }

        public frmCostSheet(SaleOrder saleOrder, string FinanceRefrenceNo, string SalesReferenceNo, string department, string customer, string currency, string totalvalue, string incoterm, string Creationdate, string paymenttermcustomer, string maker, string origin, List<ViewInfo> viewinfos, InquiryType type, string principle, string Packing, DateTime SodeliveryDate, string Warranty, String SOref, string SODate, bool? approved, bool? reApproved)
        {

            InitializeComponent();
            loadWarrantys();
            loadPaymentTerms();
            loadIncoterms();
            txtSOTotal.Text = saleOrder.totalCFRValue.ToString();
            txtSOAmount.Text = saleOrder.totalCFRValue.ToString();

            if (saleOrder != null)
            {
                //if (saleOrder.customerCompany != null)
                {
                    if (saleOrder.costCenterAmount != 0)
                    {
                        txtSOTotal.Text = saleOrder.costCenterAmount.ToString();
                    }
                    else
                    {
                        txtSOTotal.Text = saleOrder.totalCFRValue.ToString();

                    }
                    if(saleOrder.costCentercurrency_Id!=null)
                    {
                        txtCostSheetCurrency.Text = saleOrder.costcenterCurrency.CurrencyName + " " + "(" + saleOrder.costcenterCurrency.Symbol + ")";
                    }
                    if (saleOrder.exchangeRate != 0)
                    {
                        txtExchangeRate.Text = saleOrder.costCenterExchangeRate.ToString();
                    }
                    else
                    {
                        txtExchangeRate.Text = 1.ToString();

                    }
                    this.saleOrder = saleOrder;
                    lblCustomer.Text = customer;
                    lblPrincipal.Text = principle;
                    lblDepartment.Text = department;
                    txtSalesRefrence.Text = "Cost Sheet( Ref " + SalesReferenceNo + ")";
                    lblFinRef.Text = "Fin Ref# " + FinanceRefrenceNo;
                    lblPrintDate.Text = "Print Date:" + System.DateTime.Now.ToShortDateString();
                    lblSaleOrder.Text = SOref;
                    isApproved = approved;
                    isReApproved = reApproved;
                    if (SODate != "")
                        lblSaleOrderDate.Text = SODate;
                    else
                        lblSaleOrderDate.Text = (System.DateTime.Now).ToLongDateString();


                    if (Creationdate != "")
                        lblCreationDate.Text = Creationdate;
                    else
                        lblCreationDate.Text = (System.DateTime.Now).ToLongDateString();
                    if (type == InquiryType.Principal)
                    {
                        costsheetType = type;
                       
                        if (saleOrder.currency != null)
                        {
                            txtCurrency.Text = saleOrder.currency.CurrencyName + " " + "(" + saleOrder.currency.Symbol + ")";
                        }
                        else
                        {
                        }
                        //lbtotal.Text = "Total Costs";
                        //lblgrosstotal.Text = "Total Commission";
             
                        //txtActualTotal.Visibility = Visibility.Collapsed;
                        //txtActualmarginTotal.Visibility = Visibility.Collapsed;
                        grdCostItems.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        costsheetType = type;

                        if (saleOrder.currency != null)
                        {
                            txtCurrency.Text = saleOrder.currency.CurrencyName + " " + "(" + saleOrder.currency.Symbol + ")";
                        }
                        else
                        {
                        }
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = false;

                }
                else
                {
                    grdCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = true;

                }
                if (saleOrder.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Cost Center after SO Approval") == null)
                {
                    grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    //lookupVendors.IsEnabled = false;
                    //lookupOC.IsEnabled = false;
                }
                if (saleOrder.CostSheet != null)
                    costSheet = saleOrder.CostSheet;
            }
            loadViewInfos(saleOrder, viewinfos);
            loadCommentsList(saleOrder, viewinfos);
            //CalculateTotal();

        }

        public frmCostSheet(SaleOrder saleOrder, string FinanceRefrenceNo, string SalesReferenceNo, string department, string customer, string totalvalue, List<ViewInfo> viewinfos)
        {
            InitializeComponent();
            txtSOTotal.Text = totalvalue;
            txtSOAmount.Text = totalvalue;
            if (saleOrder != null)
            {
                if (saleOrder.costCenterAmount != 0)
                {
                    txtSOTotal.Text = saleOrder.costCenterAmount.ToString();
                }
                else
                {
                    txtSOTotal.Text = totalvalue;

                }
                if (saleOrder.costCentercurrency_Id != null)
                {
                    txtCostSheetCurrency.Text = saleOrder.costcenterCurrency.CurrencyName + " " + "(" + saleOrder.costcenterCurrency.Symbol + ")";
                }
                if (saleOrder.exchangeRate != 0)
                {
                    txtExchangeRate.Text = saleOrder.costCenterExchangeRate.ToString();
                }
                else
                {
                    txtExchangeRate.Text = 1.ToString();

                }
                //if (saleOrder.customerCompany != null)
                {
                    lblCustomer.Text = customer;
                    lblDepartment.Text = department;
                    txtSalesRefrence.Text = "Cost Sheet( Ref " + SalesReferenceNo + ")";
                    lblFinRef.Text = "Fin Ref# " + FinanceRefrenceNo;
                    lblPrintDate.Text = "Print Date:" + System.DateTime.Now.ToShortDateString();
                    if (saleOrder.saleOrderDate != null)
                        lblSaleOrderDate.Text = ((DateTime)saleOrder.saleOrderDate).ToLongDateString();
                    else
                        lblSaleOrderDate.Text = (System.DateTime.Now).ToLongDateString();

                    //lbfin.Visibility = Visibility.Visible;
                    if (saleOrder.currency != null)
                    {
                        txtCurrency.Text = saleOrder.currency.CurrencyName + " " + "(" + saleOrder.currency.Symbol + ")";
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
                {
                    grdCostItems.Columns.GetColumnByFieldName("actualValue").ReadOnly = true;

                }
                if (saleOrder.CostSheet != null)
                    costSheet = saleOrder.CostSheet;
            }
            loadViewInfos(saleOrder, viewinfos);

        }

        public void loadCommentsList(SaleOrder saleOrder, List<ViewInfo> viewinfos)
        {
            if (saleOrder != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                List<CommentLog> comments = new List<CommentLog>();
                if (saleOrder.isApproved == true)
                    comments = procurementRepo.getcommentslogBeforeApproval(saleOrder.Id, TransactionItemType.Sale_Order, saleOrder.ApprovedDate);
                else if (saleOrder.isApproved == true && saleOrder.PendingForClosing == true)
                {
                    comments = procurementRepo.getcommentslogAfterApproval(saleOrder.Id, TransactionItemType.Sale_Order, saleOrder.ApprovedDate);

                }
                else if (saleOrder.isApproved != true)
                {
                    comments = procurementRepo.getcommentslogAsc(saleOrder.Id, TransactionItemType.Sale_Order);

                }
                else if (saleOrder.PendingForClosing == false)
                {
                    var info = viewinfos.FirstOrDefault(z => z.Info == "Approved_Closing");
                    comments = procurementRepo.getcommentslogBetweenDates(saleOrder.Id, TransactionItemType.Sale_Order, saleOrder.ApprovedDate, info.Timestamp);

                }
                List<cmbitem> items = new List<cmbitem>();
                foreach (CommentLog comment in comments)
                {
                    string Tagged = "";/*System.Environment.NewLine + */
                    if (comment.TaggedList != null && comment.TaggedList.Count != 0)
                    {
                        Tagged = " Tagged : ";
                        foreach (var user in comment.TaggedList)
                        {
                            Tagged += user.userName + ", ";
                        }
                    }
                    //if (comment.UserId == MainWindow.currentUserid)
                    //{
                    //    items.Add(new cmbitem()
                    //    {
                    //        id = comment.Id,
                    //        name = comment.User.employee.person.FName + " " + comment.User.employee.person.LName + "( Me)",
                    //        description = comment.Comment/* + Tagged*/,
                    //        bcolor = "#B1FB17",
                    //    });

                    //}
                    //else
                    //{
                    items.Add(new cmbitem()
                    {
                        id = comment.Id,
                        name = comment.employee.person.FName + " " + comment.employee.person.LName,
                        description = comment.Comment + Tagged,
                        fcolor = comment.Timestamp.ToShortDateString() + ", " + comment.Timestamp.ToLongTimeString(),
                        //bcolor = "#7DFDFE"
                    });
                    //}
                }
                lstCommentsList.ItemsSource = items;
            }
        }
        public void loadViewInfos(SaleOrder saleOrder, List<ViewInfo> viewinfos)
        {
            //if (saleOrder != null)
            //    if (viewinfos != null && saleOrder.user != null && saleOrder.user.employee != null)
            //    {
            //        if (saleOrder.user.employee.person != null)
            //        {
            //            lblIntializer.Text = saleOrder.user.employee.person.FName + " " + saleOrder.user.employee.person.LName;
            //            lblIntializer.Visibility = Visibility.Visible;
            //            txtIntializer.Text = "Prepared";
            //            txtIntializer.Visibility = Visibility.Visible;

            //        }
            //        if (saleOrder.user.employee.Supervisor != null)
            //        {
            //            lblReviewer1.Text = saleOrder.user.employee.Supervisor.person.FName + " " + saleOrder.user.employee.Supervisor.person.LName;

            //            lblReviewer1.Visibility = Visibility.Visible;
            //            txtReviewer1.Visibility = Visibility.Visible;
            //            if (saleOrder.user.employee.Supervisor.Supervisor != null)
            //            {
            //                lblReviewer2.Text = saleOrder.user.employee.Supervisor.Supervisor.person.FName + " " + saleOrder.user.employee.Supervisor.Supervisor.person.LName;
            //                lblReviewer2.Visibility = Visibility.Visible;
            //                txtReviewer2.Visibility = Visibility.Visible;
            //                if (saleOrder.user.employee.Supervisor.Supervisor.Supervisor != null)
            //                {
            //                    lblApprover.Text = saleOrder.user.employee.Supervisor.Supervisor.Supervisor.person.FName + " " + saleOrder.user.employee.Supervisor.Supervisor.Supervisor.person.LName;
            //                    lblApprover.Visibility = Visibility.Visible;
            //                    txtApprover.Visibility = Visibility.Visible;

            //                }
            //            }
            //        }


            //        foreach (ViewInfo info in viewinfos)
            //        {
            //            if (info.UserId == saleOrder.user_Id)
            //            {
            //                txtIntializer.Text += ", " + info.Info;
            //            }

            //            else if (info.User.employeeId == saleOrder.user.employee.SupervisorId)
            //            {
            //                txtReviewer1.Text += info.Info + ", ";


            //            }
            //            else if (saleOrder.user.employee.Supervisor != null)
            //            {
            //                if (info.User.employeeId == saleOrder.user.employee.Supervisor.SupervisorId)
            //                {
            //                    txtReviewer2.Text += info.Info + ", ";

            //                }
            //                else if (saleOrder.user.employee.Supervisor.Supervisor != null)
            //                {
            //                    if (info.User.employeeId == saleOrder.user.employee.Supervisor.Supervisor.SupervisorId)
            //                    {
            //                        txtApprover.Text += info.Info + ", ";

            //                    }
            //                }
            //            }

            //        }

            //        if (string.IsNullOrEmpty(txtReviewer1.Text))
            //        {
            //            txtReviewer1.Text = "N/A";
            //        }
            //        else if (string.IsNullOrEmpty(txtReviewer2.Text))
            //        {
            //            txtReviewer2.Text = "N/A";
            //        }
            //        else if (string.IsNullOrEmpty(txtApprover.Text))
            //        {
            //            txtApprover.Text = "N/A";
            //        }
            //    }
        }
        public void loadCommentsList(Bill _bill, List<ViewInfo> viewinfos)
        {
            if (_bill != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                List<CommentLog> comments = new List<CommentLog>();
                if (_bill.isApproved == true)
                    comments = procurementRepo.getcommentslogBeforeApproval(_bill.Id, TransactionItemType.Sale_Order, _bill.ApprovedDate);
                else if (_bill.isApproved == true && _bill.PendingForClosing == true)
                {
                    comments = procurementRepo.getcommentslogAfterApproval(_bill.Id, TransactionItemType.Sale_Order, _bill.ApprovedDate);

                }
                else if (_bill.isApproved != true)
                {
                    comments = procurementRepo.getcommentslogAsc(_bill.Id, TransactionItemType.Sale_Order);

                }
                else if (_bill.PendingForClosing == false)
                {
                    var info = viewinfos.FirstOrDefault(z => z.Info == "Approved_Closing");
                    comments = procurementRepo.getcommentslogBetweenDates(_bill.Id, TransactionItemType.Sale_Order, _bill.ApprovedDate, info.Timestamp);

                }
                List<cmbitem> items = new List<cmbitem>();
                foreach (CommentLog comment in comments)
                {
                    string Tagged = "";/*System.Environment.NewLine + */
                    if (comment.TaggedList != null && comment.TaggedList.Count != 0)
                    {
                        Tagged = " Tagged : ";
                        foreach (var user in comment.TaggedList)
                        {
                            Tagged += user.userName + ", ";
                        }
                    }
                    //if (comment.UserId == MainWindow.currentUserid)
                    //{
                    //    items.Add(new cmbitem()
                    //    {
                    //        id = comment.Id,
                    //        name = comment.User.employee.person.FName + " " + comment.User.employee.person.LName + "( Me)",
                    //        description = comment.Comment/* + Tagged*/,
                    //        bcolor = "#B1FB17",
                    //    });

                    //}
                    //else
                    //{
                    items.Add(new cmbitem()
                    {
                        id = comment.Id,
                        name = comment.employee.person.FName + " " + comment.employee.person.LName,
                        description = comment.Comment + Tagged,
                        fcolor = comment.Timestamp.ToShortDateString() + ", " + comment.Timestamp.ToLongTimeString(),
                        //bcolor = "#7DFDFE"
                    });
                    //}
                }
                lstCommentsList.ItemsSource = items;
            }
        }
        public void loadViewInfos(Bill _bill, List<ViewInfo> viewinfos)
        {
            if (_bill != null)
                if (viewinfos != null && _bill.user != null && _bill.user.employee != null)
                {
                    //if (_bill.user.employee.person != null)
                    //{
                    //    lblIntializer.Text = _bill.user.employee.person.FName + " " + _bill.user.employee.person.LName;
                    //    lblIntializer.Visibility = Visibility.Visible;
                    //    txtIntializer.Text = "Prepared";
                    //    txtIntializer.Visibility = Visibility.Visible;

                    //}
                    //if (_bill.user.employee.Supervisor != null)
                    //{
                    //    lblReviewer1.Text = _bill.user.employee.Supervisor.person.FName + " " + _bill.user.employee.Supervisor.person.LName;

                    //    lblReviewer1.Visibility = Visibility.Visible;
                    //    txtReviewer1.Visibility = Visibility.Visible;
                    //    if (_bill.user.employee.Supervisor.Supervisor != null)
                    //    {
                    //        lblReviewer2.Text = _bill.user.employee.Supervisor.Supervisor.person.FName + " " + _bill.user.employee.Supervisor.Supervisor.person.LName;
                    //        lblReviewer2.Visibility = Visibility.Visible;
                    //        txtReviewer2.Visibility = Visibility.Visible;
                    //        if (_bill.user.employee.Supervisor.Supervisor.Supervisor != null)
                    //        {
                    //            lblApprover.Text = _bill.user.employee.Supervisor.Supervisor.Supervisor.person.FName + " " + _bill.user.employee.Supervisor.Supervisor.Supervisor.person.LName;
                    //            lblApprover.Visibility = Visibility.Visible;
                    //            txtApprover.Visibility = Visibility.Visible;

                    //        }
                    //    }
                    //}


                    //foreach (ViewInfo info in viewinfos)
                    //{
                    //    if (info.UserId == _bill.user_Id)
                    //    {
                    //        txtIntializer.Text += ", " + info.Info;
                    //    }

                    //    else if (info.User.employeeId == _bill.user.employee.SupervisorId)
                    //    {
                    //        txtReviewer1.Text += info.Info + ", ";


                    //    }
                    //    else if (_bill.user.employee.Supervisor != null)
                    //    {
                    //        if (info.User.employeeId == _bill.user.employee.Supervisor.SupervisorId)
                    //        {
                    //            txtReviewer2.Text += info.Info + ", ";

                    //        }
                    //        else if (_bill.user.employee.Supervisor.Supervisor != null)
                    //        {
                    //            if (info.User.employeeId == _bill.user.employee.Supervisor.Supervisor.SupervisorId)
                    //            {
                    //                txtApprover.Text += info.Info + ", ";

                    //            }
                    //        }
                    //    }

                    //}

                    //if (string.IsNullOrEmpty(txtReviewer1.Text))
                    //{
                    //    txtReviewer1.Text = "N/A";
                    //}
                    //else if (string.IsNullOrEmpty(txtReviewer2.Text))
                    //{
                    //    txtReviewer2.Text = "N/A";
                    //}
                    //else if (string.IsNullOrEmpty(txtApprover.Text))
                    //{
                    //    txtApprover.Text = "N/A";
                    //}
                }
        }
        //Offer offer = new Offer();
        //SaleOrder saleOrder = new SaleOrder();
        public static int costSheetId = 0;
        public static ERP_BL.Databases.CostSheet costSheet = new ERP_BL.Databases.CostSheet();
        public static bool? isApproved;
        public static bool? isReApproved;

        public static decimal revisedValue;


        public List<FieldValue> fieldValues = new List<FieldValue>();
        public List<CostFieldHistory> fieldHistoryValues = new List<CostFieldHistory>();

        public List<CostFieldValues> costFieldValue = new List<CostFieldValues>();
        //public static List<CostFieldValues> costFieldValues;
        public SaleOrderRepo repo = new SaleOrderRepo();
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            GetCostFieldValues();
            costSheet.FieldValues = fieldValues;
          if(changedVendorFields != null)
            {
                if(changedVendorFields.Count>0)
                {
                    CommentLog comment = new CommentLog();
                    if (saleOrder.isApproved == true)
                        foreach (var vendorValue in changedVendorFields)
                        {
                            Currency commentCurrency = new Currency();
                            if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                commentCurrency = saleOrder.costcenterCurrency;
                            }
                            else
                            {
                                commentCurrency = saleOrder.currency;
                            }
                            if (vendorValue.addedTitle != vendorValue.Title)
                            {
                                if (!string.IsNullOrEmpty(comment.Comment))
                                {
                                    comment.Comment += "Vendor has been changed"  + "\n"
                                                        + "From: " + vendorValue.Title.ToString() 
                                                        + "\nTo: " + vendorValue.addedTitle.ToString();
                                }
                                else
                                {
                                    comment.Comment = 
                                                        "Vendor has been changed" + "\n"
                                                       + "From: " + vendorValue.Title.ToString() 
                                                       + "\nTo: " + vendorValue.addedTitle.ToString();
                                }
                            }
                        }
                    var res = MessageBox.Show("Vendor has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        List<User> tagUsers = new List<User>();
                        List<User> ccUsers = new List<User>();
                        List<User> tagUsersRecommendation = new List<User>();
                        List<User> ccUsersRecommendation = new List<User>();
                        UsersRepo usersRepo = new UsersRepo();
                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(usersRepo.getusersByCompanyDepartment((int)saleOrder.department.Id, (int)saleOrder.company.Id), costSheet.Id, TransactionItemType.CostCenter);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            ccUsers = win.ccUsers;
                            tagUsersRecommendation = win.tagRecommendationUsers;
                            ccUsersRecommendation = win.ccRecommendationUsers;
                            comment.Timestamp = DateTime.Now;
                            comment.Subject = "Vendor Changed";
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
            if(changedBudgetFields!=null)
            {
                if(changedBudgetFields.Count>0)
                {
                    CommentLog comment = new CommentLog();
                    if (saleOrder.isApproved == true)
                        foreach (var budgetValue in changedBudgetFields)
                        {
                            Currency commentCurrency = new Currency();
                            if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                commentCurrency = saleOrder.costcenterCurrency;
                            }
                            else
                            {
                                commentCurrency = saleOrder.currency;
                            }
                            if (budgetValue.addedBudgetValue != budgetValue.budgetCost)
                            {
                                if (!string.IsNullOrEmpty(comment.Comment))
                                {
                                    comment.Comment += "+\n" + "Under the head " + "Budget Cost " + "(" + budgetValue.Title + ")" + "\n"
                                                + "(" + "Budget Cost has been changed" + ")" + "\n"
                                                + "From: " + budgetValue.budgetCost.ToString() + " (" + commentCurrency.Symbol + ")"
                                                + "\nTo: " + budgetValue.addedBudgetValue.ToString() + " (" + commentCurrency.Symbol + ")";
                                }
                                else
                                {
                                    comment.Comment = "Under the head " + "Budget Cost " + "(" + budgetValue.Title + ")" + "\n"
                                                + "(" + "Budget Cost has been changed" + ")" + "\n"
                                                + "From: " + budgetValue.budgetCost.ToString() + " (" + commentCurrency.Symbol + ")"
                                                + "\nTo: " + budgetValue.addedBudgetValue.ToString() + " (" + commentCurrency.Symbol + ")";
                                }
                            }
                        }
                    var res = MessageBox.Show("Budget value has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        List<User> tagUsers = new List<User>();
                        List<User> ccUsers = new List<User>();
                        List<User> tagUsersRecommendation = new List<User>();
                        List<User> ccUsersRecommendation = new List<User>();
                        UsersRepo usersRepo = new UsersRepo();
                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(usersRepo.getusersByCompanyDepartment((int)saleOrder.department.Id, (int)saleOrder.company.Id), costSheet.Id, TransactionItemType.CostCenter);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            ccUsers = win.ccUsers;
                            tagUsersRecommendation = win.tagRecommendationUsers;
                            ccUsersRecommendation = win.ccRecommendationUsers;
                            comment.Timestamp = DateTime.Now;
                            comment.Subject = "Budget Value Changed";
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
            if(changedAdjustedFields!=null)
            {
                if(changedAdjustedFields.Count>0)
                {
                    CommentLog comment = new CommentLog();
                    foreach (var changedAdjustValue in changedAdjustedFields)
                    {
                        if (changedAdjustValue.addedAdjustedValue != changedAdjustValue.adjSCost)
                        {
                            Currency commentCurrency = new Currency();
                            if (saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                commentCurrency = saleOrder.costcenterCurrency;
                            }
                            else
                            {
                                commentCurrency = saleOrder.currency;
                            }
                            if (!string.IsNullOrEmpty(comment.Comment))
                            {
                                comment.Comment += "+\n" + "Under the head " + "Adjusted Cost " + "(" + changedAdjustValue.Title + ")" + "\n"
                                            + "(" + "Adjusted System Cost has been changed" + ")" + "\n"
                                            + "From: " + changedAdjustValue.adjSCost.ToString() + " (" + commentCurrency.Symbol + ")"
                                            + "\nTo: " + changedAdjustValue.addedAdjustedValue.ToString() + " (" + commentCurrency.Symbol + ")";
                            }
                            else
                            {
                                comment.Comment = "Under the head " + "Adjusted Cost " + "(" + changedAdjustValue.Title + ")" + "\n"
                                            + "(" + "Adjusted Cost has been changed" + ")" + "\n"
                                            + "From: " + changedAdjustValue.adjSCost.ToString() + " (" + commentCurrency.Symbol + ")"
                                            + "\nTo: " + changedAdjustValue.addedAdjustedValue.ToString() + " (" + commentCurrency.Symbol + ")";
                            }

                        }
                    }
                    var res = MessageBox.Show("Adjusted value has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        List<User> tagUsers = new List<User>();
                        List<User> ccUsers = new List<User>();
                        List<User> tagUsersRecommendation = new List<User>();
                        List<User> ccUsersRecommendation = new List<User>();
                        UsersRepo usersRepo = new UsersRepo();
                        NotificationsRepo notificationsRepo = new NotificationsRepo();
                        if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(usersRepo.getusersByCompanyDepartment((int)saleOrder.department.Id, (int)saleOrder.company.Id), costSheet.Id, TransactionItemType.CostCenter);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            ccUsers = win.ccUsers;
                            tagUsersRecommendation = win.tagRecommendationUsers;
                            ccUsersRecommendation = win.ccRecommendationUsers;
                            comment.Timestamp = DateTime.Now;
                            comment.Subject = "Adjusted Value Changed";
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



            if (fieldHistoryValues != null)
                {
                    costSheet.CostFieldHistories = fieldHistoryValues;
                }
                if (revisedValue != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
                {

                }
                else
                {
                    //var amount = (string.IsNullOrEmpty(txtRevisedTotal.Text)) ? 0 : Convert.ToDecimal(txtRevisedTotal.Text);
                    //if (revisedValue != amount && isApproved == true)
                    //{
                    //    isReApproved = false;
                    //}
                }
            this.Close();
        }
        public void loadPaymentTerms()
        {
            List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
            PaymentTermRepo TermRepo = new PaymentTermRepo();
            paymentTerms = TermRepo.getAll();

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (PaymentTerm paymentTerm in paymentTerms)
            {
                cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            //lookupPaymentTerms.ItemsSource = paymentTerms;

        }
        public void loadIncoterms()
        {
            IncotermRepo termRepo = new IncotermRepo();
            List<Incoterm> incoterms = new List<Incoterm>();
            List<IncoTermName> incotermNames = new List<IncoTermName>();

            incoterms = termRepo.getAll();

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (Incoterm incoterm in incoterms)
            {
                cmbitems.Add(new cmbitem() { name = incoterm.term, id = incoterm.Id });
            }

          

            foreach(var term in incoterms)
            {
                IncoTermName termName = new IncoTermName()
                {
                    Id = term.Id,
                    termName = term.term,
                    discription = term.discription,
                    isActive = term.isActive

                };
                incotermNames.Add(termName);
            }
            //lookupInco.ItemsSource = incotermNames.Distinct();

        }
        public void loadWarrantys()
        {
            List<Warranty> warrantys = new List<Warranty>();
            ProcurementRepo repo = new ProcurementRepo();
            warrantys = repo.GetActiveWarranties();

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (Warranty warranty in warrantys)
            {
                cmbitems.Add(new cmbitem() { name = warranty.name, id = warranty.Id });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            //lookupWarrenty.ItemsSource = warrantys;
        }
        /// <summary>
        /// Load Cost Fields Value
        /// </summary>
        public void loadValues()
        {
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costSheetBillFields = new List<CostSheetBillField>();
            costSheetPOFields = new List<CostSheetPOField>();    
            costSheetSIFields = new List<CostSheetSIField>();
            costSheetSOFields = new List<CostSheetSOField>();
            costSheetSaleReceiptFields= new List<CostSheetSaleReceiptField>();
            costSheetPaymentFields = new List<CostSheetPaymentField>();

            costfieldValues = grdCostItems.ItemsSource as List<CostFieldValues>;

            if (bill.Id != 0 && costSheet.Id != 0)
            {
                costSheetBillFields = repo.GetCostSheetBillFields(bill.Id, costSheet.Id);
            }
            else
                if (saleInvoice.Id != 0 && costSheet.Id != 0)
            {
                costSheetSIFields = repo.GetCostSheetSIFields(saleInvoice.Id, costSheet.Id);
            }
            else
                if (purchaseOrder.Id != 0 && costSheet.Id != 0)
            {
                costSheetPOFields = repo.GetCostSheetPOFields(purchaseOrder.Id, costSheet.Id);
            }
            if (saleOrder.Id != 0 && costSheet.Id != 0)
            {
                costSheetSOFields = repo.GetCostSheetSOFields(saleOrder.Id, costSheet.Id);
            }
   
            foreach (var item in costfieldValues)
            {
                
                foreach (var field in costSheet.FieldValues)
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
                        else if (field.Type == 37)
                        {
                            item.advanceValue = field.Value;
                        }
                        else if (field.Type == 1)
                        {
                            item.budgetedValue = field.Value;
                            if(field.Value!=0)
                            changedBudgetFields.Add(new CostBudgetFieldValues()
                            {
                                Id = item.Id,
                                addedBudgetValue = item.budgetedValue,
                                budgetCost = item.budgetedValue,
                                Title = item.Title
                            });
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
                                        changedAdjustedFields.Add(new CostAdjustedFieldValues()
                                        {
                                            Id = item.Id,
                                            addedAdjustedValue = item.adjSCost,
                                            adjSCost = item.adjSCost,
                                            Title = item.Title
                                        });
                                    }
                                }
                            }
                        }
                        else if (field.Type == 4)
                        {
                            VendorRepo vendorRepo = new VendorRepo();
                            var vendor = vendorRepo.Get((int)field.Value);
                            item.vendor = vendor;

                            changedVendorFields.Add(new CostVendorFieldValues()
                            {
                                Id = item.Id,
                                addedTitle = item.vendor.company.CompanyName,
                                Title = item.vendor.company.CompanyName
                            });

                        }
                        //else if (field.Type == 5)
                        //{
                        //    item.systemPayment = field.Value;
                        //    if (item.systemPayment == 0 || saleOrder.isApproved != true)
                        //    {
                        //        item.systemPayment = field.Value;
                        //    }
                        //}
                        //else if (field.Type == 6)
                        //{
                        //    item.addedSystemValue = field.Value;
                        //    if (item.addedSystemValue == 0 || saleOrder.isApproved != true)
                        //    {
                        //        item.systemValue = field.Value;
                        //    }
                        //}
                        else if (field.Type == 7)
                        {
                            //item.billAmount = field.Value;
                            if (costSheetBillFields.Count != 0)
                            {
                                var dbField = costSheetBillFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.Bill_Cost && x.CostSheetId == costSheet.Id && x.Bill_Id == bill.Id);
                                if (dbField != null)
                                {
                                    var billFieldAmount = dbField.Value;
                                    if (billFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.billAmount = billFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 8)
                        {
                            //item.billAmount = field.Value;
                            if (costSheetPOFields.Count != 0)
                            {
                                var dbField = costSheetPOFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.PO_Cost && x.CostSheetId == costSheet.Id && x.PO_Id == purchaseOrder.Id);
                                if (dbField != null)
                                {
                                    var POFieldAmount = dbField.Value;
                                    if (POFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.poAmount = POFieldAmount;
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
                            //item.billAmount = field.Value;
                            if (costSheetSaleReceiptFields.Count != 0)
                            {
                                var dbField = costSheetSaleReceiptFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.Receipt_Cost && x.CostSheetId == costSheet.Id && x.Receipt_Id == salesReceipt.Id);
                                if (dbField != null)
                                {
                                    var ReceiptFieldAmount = dbField.Value;
                                    if (ReceiptFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.receiptAmount = ReceiptFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 32)
                        {
                            //if (field.Value != 0)
                            //{
                            //    //item.addedSystemValue = field.Value;
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
                        //else if (field.Type == 37)
                        //{
                        //    if (costSheet != null && costSheet.Id != 0)
                        //    {
                        //        CompanyRepo companyRepo = new CompanyRepo();
                        //        item.vendorNature = companyRepo.GetVendorNature((int)field.Value);
                        //    }
                        //}
                        else if (field.Type == 36)
                        {
                            //item.billAmount = field.Value;
                            if (costSheetSIFields.Count != 0)
                            {
                                var dbField = costSheetSIFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.SI_Cost && x.CostSheetId == costSheet.Id && x.SI_Id == saleInvoice.Id);
                                if (dbField != null)
                                {
                                    var SIFieldAmount = dbField.Value;
                                    if (SIFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.poAmount = SIFieldAmount;
                                    }
                                }
                            }
                        }

                    }
                costfields.Add(item);

            }
            if(changedAdjustedFields!=null)
            {
                if(changedAdjustedFields.Count>0)
                {
                    changedAdjustedFields = changedAdjustedFields.GroupBy(o => o.Id).Select(g => g.First()).ToList();
                }
            }
            grdCostItems.ItemsSource = costfields;
        }
  
        public/* List<FieldValue>*/ void GetCostFieldValues()
        {
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            fieldValues = new List<FieldValue>();
            //fieldHistoryValues = new List<CostFieldHistory>();

            costfieldValues = grdCostItems.ItemsSource as List<CostFieldValues>;

            foreach (var item in costfieldValues)
            {
               
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 1, Value = item.budgetedValue });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 2, Value = item.actualValue });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 0, Value = item.revisedValue });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 3, Value = item.systemValue, adjSCost= item.adjSCost });
                if (saleOrder.isApproved == true)
                {
                    var vendorValue = changedVendorFields.FirstOrDefault(x => x.Id == item.Id);
                    if (vendorValue != null  && item.vendor!=null)
                        if (vendorValue.addedTitle != item.vendor.company.CompanyName)
                        {
                            vendorValue.Id = item.vendor.Id;
                            vendorValue.addedTitle = item.vendor.company.CompanyName;
                            changedVendorFields.Remove(vendorValue);
                            changedVendorFields.Add(vendorValue);

                        }
                        else
                        {
                            changedVendorFields.Remove(vendorValue);
                        }

                }
                if (saleOrder.isApproved == true)
                {
                    var budgetValue = changedBudgetFields.FirstOrDefault(x => x.Id == item.Id);
                    if(budgetValue!=null)
                    if (budgetValue.budgetCost != item.budgetedValue)
                    {
                        budgetValue.addedBudgetValue = item.budgetedValue;
                        changedBudgetFields.Remove(budgetValue);
                        changedBudgetFields.Add(budgetValue);
                       
                    }
                    else
                    {
                        changedBudgetFields.Remove(budgetValue);
                    }

                }
                var changedAdjustValue = changedAdjustedFields.FirstOrDefault(x => x.Id == item.Id);
                if (changedAdjustValue != null)
                if (changedAdjustValue.adjSCost != item.adjSCost)
                {
                    changedAdjustValue.addedAdjustedValue = item.adjSCost;
                    changedAdjustedFields.Remove(changedAdjustValue);
                    changedAdjustedFields.Add(changedAdjustValue);
                   
                }
                else
                {
                    changedAdjustedFields.Remove(changedAdjustValue);
                }
                if (item.vendor != null)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 4, Value = item.vendor.Id });
                //fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 5, Value = item.systemPayment });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 8, Value = item.poAmount });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 7, Value = item.billAmount });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 10, Value = item.soAmountSRBC });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 11, Value = item.SRBC });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 12, stringValue = item.Maker,Value=0 });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 13, stringValue = item.Origin, Value = 0 });
                if (item.Packing != false)
                    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 14, isPacking = item.Packing, Value = 0 });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 15, dateValue = item.deliveryDate, Value = 0,stringValue=null });
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
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 31, Value = item.receiptAmount });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 32, Value = item.paymentAmount });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 33, stringValue = item.IntermediaryPort, Value = 0 });

                //if (item.addedSystemValue != 0)
                //{
                //    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 32, Value = item.addedSystemValue });
                //}
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 34, stringValue = item.deliveryDays, Value = 0 });
                
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 35, adjSCost = item.adjSCost });
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 36, Value = item.siAmount });

                //if (item.vendorNature!=null)
                //    fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 37, Value = item.vendorNature.Id });


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
                PurchaseOrderRepo poRepo = new PurchaseOrderRepo();
                Incoterm incoterm = new Incoterm();
                if(item.IncotermName!=null)
                {
                     incoterm = IncotermRepo.get(item.IncotermName.Id);

                }
                fieldValues.Add(new FieldValue { FieldId = item.Id, Type = 37, Value = item.advanceValue });


                fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Budgeted_Margin, Value = item.budgetedValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Actual_Margin, Value = item.actualValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.Revised_Margin, Value = item.revisedValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.System_Cost, Value = item.systemValue, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
                fieldHistoryValues.Add(new CostFieldHistory { FieldId = item.Id, FieldType = CostFieldType.PO_Cost, Value = item.poAmount, timeStamp = System.DateTime.Now, CostSheetId = costSheet.Id });
            }

            if(offer.Id!=0)
            costSheet.TotalBudgetedMargin = Convert.ToDecimal( txtTotalBudgetedCostMargin.Text);

            // return fieldValues;
        }
        private void loadCurrencies()
        {

            CurrencyRepo currencyRepo = new CurrencyRepo();
            List<Currency> currencies = currencyRepo.getAll();
            currencies = currencies.Where(x => x.isVoid != true).ToList();
            //lookupOC.ItemsSource = currencies;
            
        }
        private void loadPQDocuments()
        {

            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            List<PQDocument> documents = saleOrderRepo.GetPQDocuments();
            
            //lookupPQ.ItemsSource = documents;

        }
        private void loadShippingTerms()
        {

            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            List<ShippingTerm> terms = saleOrderRepo.GetShippingTerms();

            //lookupST.ItemsSource = terms;

        }
        private void FrmCostSheet_Loaded(object sender, RoutedEventArgs e)
        {
            if (saleOrder.Id != 0)
            {
                if (saleOrder.CreationDate.Value.Year >= 2025 && saleOrder.CreationDate.Value.Month >= 1 && saleOrder.CreationDate.Value.Day >= 17)
                {
                    checkHideRevisedCost.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (offer.CreationDate != null && offer.CreationDate.Value.Year >= 2025 && offer.CreationDate.Value.Month >= 1 && offer.CreationDate.Value.Day >= 17)
                {
                    checkHideRevisedCost.Visibility = Visibility.Collapsed;
                }
            }
            PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
            

            loadVendors();
            loadPaymentTerms();
            loadIncoterms();
            loadCurrencies();
            loadPQDocuments();
            loadShippingTerms();
            if(payment!=null && payment.Id!=0)
            {
                SaleOrderRepo repo = new SaleOrderRepo();
                costSheetPaymentFields= repo.GetCostSheetPaymentFields(payment.Id, costSheet.Id);
            }

            foreach (var item in repo.getActiveCostSheetFields())
            {
                bool poClosed = false;
                if (costSheet.Id != 0)
                {
                    var po = purchaseOrderRepo.get(costSheet.Id, item.Id);
                    if (po != null)
                    {
                        if (po.isApproved == true && po.stage == "Closed")
                        {
                            poClosed = true;
                        }
                        else if (po.isApproved == true && po.PurchaseOrderStatus.isActive == false && po.PendingForClosing != true)
                        {
                            poClosed = true;
                        }
                    }
                }
                costFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title, poClosed=poClosed});
            }
            grdCostItems.ItemsSource = costFieldValue;
            if (costSheet != null && costSheet.FieldValues != null)
            {
                loadValues();
                checkHideUnHideTransactions.IsChecked = true;
            }
            CalculateTotal();
            fieldHistoryValues = costSheet.CostFieldHistories != null ? costSheet.CostFieldHistories : new List<CostFieldHistory>();
            loadcomments();
            //checkVendorProfile.IsChecked = true;
            checkHideActualCost.IsChecked = true;
            checkHideRevisedCost.IsChecked = true;
            checkPOTerms.IsChecked = true;
            if (saleOrder.Id != 0)
            {
                bool isLinked = saleOrderRepo.GetParentOrders(saleOrder.Id);

                if (saleOrder.ParentSO_Id != null || isLinked == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View linked sale orders") != null)
                    {
                        btnLinkedSO.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnLinkedSO.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    btnLinkedSO.Visibility = Visibility.Collapsed;
                }
            }
        }

        public void loadVendors()
        {
            //if (saleOrder.Id != 0 && saleOrder.department != null && saleOrder.vendors.Count != 0)
                //lookupVendors.ItemsSource = saleOrder.department.Vendors;
        }
        public void loadcomments()
        {
            try
            {
                if (costSheet != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(costSheet.Id, TransactionItemType.CostCenter);
                    List<cmbitem> items = new List<cmbitem>();

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }
        private void FrmCostSheet_Unloaded(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            if(string.IsNullOrEmpty(logValue))
            {
                logValue = "Viewed Cost Center";
                usersRepo.Add(TransactionInfo.viewed, costSheet.Id, (int)TransactionItemType.CostCenter, logValue);

            }
            else
            usersRepo.Add(TransactionInfo.Edited, costSheet.Id, (int)TransactionItemType.CostCenter, logValue);
        }

        private void LblActualTotal_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            decimal sum = 0;

            foreach (var item in grdCostItems.ItemsSource as List<CostFieldValues>)
            {
                sum += Convert.ToDecimal(item.actualValue);

            }
            //txtActualTotal.Text = sum.ToString();

        }

        private void LblBudgetedTotal_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            decimal sum = 0;

            foreach (var item in grdCostItems.ItemsSource as List<CostFieldValues>)
            {
                sum += Convert.ToDecimal(item.budgetedValue);

            }
            //txtBudgetedTotal.Text = sum.ToString();
        }

        private void LblActualTotal_EditValueChanging(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            decimal sum = 0;

            foreach (var item in grdCostItems.ItemsSource as List<CostFieldValues>)
            {
                sum += Convert.ToDecimal(item.actualValue);

            }
            //txtActualTotal.Text = sum.ToString();

        }

        private void View_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "revisedValue" && saleOrder.Id != 0)
            {
                CalculateTotal();
                
                return;
            }
            if(e.Column.FieldName == "budgetedValue")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited budgeted value"; 
                }
            }
            else
            if(e.Column.FieldName == "revisedValue")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited revised value";
                }
            }
            else
            if (e.Column.FieldName == "OC.CurrencyName")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Currency";
                }
            }
            else
            if (e.Column.FieldName == "exchangeRate")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Exchange Rate";
                }
            }
            else
            if (e.Column.FieldName == "OCamount")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited OCamount";
                }
            }
            else
            if (e.Column.FieldName == "Maker")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Maker";
                }
            }
            else
            if (e.Column.FieldName == "Origin")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Origin";
                }
            }
            else
            if (e.Column.FieldName == "Packing")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Packing";
                }
            }
            else
            if (e.Column.FieldName == "deliveryDate")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Delivery Date";
                }
            }
            else
            if (e.Column.FieldName == "PaymentTerm")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Payment Term";
                }
            }
            else
            if (e.Column.FieldName == "Warranty")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Warranty";
                }
            }
            else
            if (e.Column.FieldName == "IncotermName")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Inco Term";
                }
            }
            else
            if (e.Column.FieldName == "Dg_Goods")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited DG Goods";
                }
            }
            else
            if (e.Column.FieldName == "LoadingPort")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Loading Port";
                }
            }
            else
            if (e.Column.FieldName == "DestinationPort")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Destination Port";
                }
            }
            else
            if (e.Column.FieldName == "IntermediaryPort")
            {
                if (costSheet.Id != 0)
                {
                    logValue = "Edited Destination Port";
                }
            }
            if (grdCostItems.GetFocusedRow() != null)
            {
                var row = grdCostItems.GetFocusedRow() as CostFieldValues;

                if (isApproved != true)
                {
                    row.revisedValue = row.budgetedValue;
                }
                else if (row.revisedValue == 0)
                    row.revisedValue = row.budgetedValue;
            }

            CalculateTotal();
        }

        private void CalculateTotal()
        {
            decimal sumbudg = 0; decimal sumactu = 0;
            decimal sumRevised = 0;
            if (grdCostItems.ItemsSource != null)
                foreach (var item in grdCostItems.ItemsSource as List<CostFieldValues>)
                {
                    {

                        sumbudg += item.budgetedValue;
                        sumactu += item.actualValue;
                        sumRevised += item.revisedValue;


                    }
                }
            //txtBudgetedTotal.Text = sumbudg.ToString();
            //txtActualTotal.Text = sumactu.ToString();
            //txtRevisedTotal.Text = sumRevised.ToString();

            var Sototal = Convert.ToDecimal(txtSOTotal.Text);
            //txtBudgetedMarginTotal.Text = (Sototal - sumbudg).ToString();
            //txtRevisedMarginTotal.Text = (Sototal - sumRevised).ToString();

            if (Sototal != 0)
            {
                var percent = (((Sototal - sumbudg) / Sototal) * 100);
                //txtBudgetedPercent.Text = decimal.Round(percent, 2).ToString();
                //txtBudgetedPercent.Text += "%";
                var percentRevised = (((Sototal - sumRevised) / Sototal) * 100);

                //txtRevisedPercent.Text = decimal.Round(percentRevised, 2).ToString();
                //txtRevisedPercent.Text += "%";
            }




            //if (sumactu != 0)
            //{
            //    //txtActualmarginTotal.Text = (Convert.ToDecimal(txtSOTotal.Text) - sumactu).ToString();
            //    //if (Sototal != 0)
            //    //{
            //    //    var percent = (((Sototal - sumactu) / Sototal) * 100);
            //    //    txtActualPercent.Text = decimal.Round(percent, 2).ToString();
            //    //    txtActualPercent.Text += "%";
            //    //}


            //}

            //else
            //    //txtActualmarginTotal.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)

        {
            btnPrint.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;
            grdCommentsList.Visibility = Visibility.Visible;
            this.Height += grdCommentsList.Height;
            PrintDialog dlg = new PrintDialog();
            var pd = new PrintDialog();
            var pageSize = new Size(8.26 * 96, 11.69 * 96);
            FrameworkElement fe = (grdCostPrint as FrameworkElement);
            fe.Measure(new Size(Int32.MaxValue, Int32.MaxValue));
            Size visualSize = fe.DesiredSize;
            fe.Arrange(new Rect(new Point(0, 0), visualSize));
            MemoryStream stream = new MemoryStream();
            string pack = "pack://temp.xps";
            Uri uri = new Uri(pack);
            DocumentPaginator paginator;
            XpsDocument xpsDoc;
            using (Package container = Package.Open(stream, FileMode.Create))
            {
                PackageStore.AddPackage(uri, container);
                using (xpsDoc = new XpsDocument(container, CompressionOption.Fast, pack))
                {
                    XpsSerializationManager rsm =
                      new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
                    rsm.SaveAsXaml(grdCostPrint);
                    paginator = ((IDocumentPaginatorSource)
                      xpsDoc.GetFixedDocumentSequence()).DocumentPaginator;
                    paginator.PageSize = visualSize;
                }
                PackageStore.RemovePackage(uri);
            }
            if ((bool)dlg.ShowDialog().GetValueOrDefault())
            {
                dlg.PrintDocument(paginator, "");
            }

            grdCostItems.Columns.GetColumnByName("colactualValuePrint").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("actualValue").Visible = true;
            this.Height -= grdCommentsList.Height;
            grdCommentsList.Visibility = Visibility.Collapsed;

            btnPrint.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;
        }

        private void ViewPrinciple_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            CalculatePrinciplTotal();

            
        }
        private void CalculatePrinciplTotal()
        {
        }

        private void CmbSupplierPaymentterm_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void BtnViewHistory_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View edit history of Costs") != null)
            {

                frmCostFieldsHistory fieldsHistory = new frmCostFieldsHistory(costSheet.CostFieldHistories);
                fieldsHistory.ShowDialog();
            }
        }
        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        //private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        //{
        //    if (saleOrder.department != null && saleOrder.department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
        //    {

        //        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartment(saleOrder.department.Id),TransactionItemType.CostCenter);
        //        inputBox.ShowDialog();

        //    }
        //    else
        //    {
        //        frmInputBox inputBox = new frmInputBox();
        //        inputBox.ShowDialog();
        //    }

        //    if (costSheet != null)
        //    {
        //        ProcurementRepo procurementRepo = new ProcurementRepo();

        //        if (frmInputBox.comment != "" && saleOrder.Id != 0)
        //        {
        //            if (frmInputBox.taggedUsers != null)
        //            {
        //                foreach (var user in frmInputBox.taggedUsers)
        //                {
        //                    notificationsRepo.Add(SystemLogic.currentUser.userName + " tagged you in a CostSheet in SO #" + saleOrder.referenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ");

        //                }
        //            }
        //            procurementRepo.Add(costSheet.Id, TransactionItemType.CostCenter, frmInputBox.Comment, SystemLogic.currentUser.employeeId);
        //            frmInputBox.taggedUsers = new List<User>();
        //            loadcomments();
        //        }
        //        else if (saleOrder.Id == 0)
        //        {
        //            DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
        //        }

        //    }
        //}

        private void BtnReplyComment_Click(object sender, RoutedEventArgs e)
        {
            //{
            //    if (saleOrder.department!= null && saleOrder.department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
            //    {
            //        Button thisButton = (Button)sender;
            //        //thisButton.Parent

            //        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartment(saleOrder.department.Id), (thisButton).Tag.ToString());
            //        inputBox.ShowDialog();

            //    }
            //    else
            //    {
            //        frmInputBox inputBox = new frmInputBox();
            //        inputBox.ShowDialog();
            //    }

            //    if (costSheet != null)
            //    {
            //        ProcurementRepo procurementRepo = new ProcurementRepo();

            //        if (frmInputBox.comment != "" && costSheet.Id != 0)
            //        {
            //            if (frmInputBox.taggedUsers != null)
            //            {
            //                foreach (var user in frmInputBox.taggedUsers)
            //                {
            //                    notificationsRepo.Add(SystemLogic.currentUser.userName + " tagged you in SO #" + saleOrder.referenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ");

            //                }
            //            }
            //            procurementRepo.Add(costSheet.Id, TransactionItemType.CostCenter, frmInputBox.Comment,  SystemLogic.currentUser.employeeId);
            //            frmInputBox.taggedUsers = new List<User>();
            //            loadcomments();
            //        }
            //        else if (costSheet.Id == 0)
            //        {
            //            DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
            //        }

            //    }
            //}
        }


        UsersRepo UsersRepo = new UsersRepo();

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (saleOrder.Id != 0)
                {
                    //if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    //{
                    //    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), comment, TransactionItemType.Bill);
                    //    inputBox.ShowDialog();
                    //}
                    //else 
                    if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                    {
                        var userss = UsersRepo.getusersByCompanyDepartment(saleOrder.department.Id, saleOrder.company.Id);

                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, userss, comment, TransactionItemType.CostCenter);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (costSheet != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && costSheet.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(costSheet.Id, TransactionItemType.CostCenter, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SO CostSheet #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SO CostSheet #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in SO CostSheet #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in SO CostSheet #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (saleOrder.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                        }
                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }

            //if (grdCommentss.SelectedItem != null)
            //{
            //    var comment = grdCommentss.SelectedItem as CommentLog;
            //    if (saleOrder.Id != 0)
            //    {
            //        if (saleOrder.department != null && saleOrder.department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
            //        {
            //            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartment(saleOrder.department.Id), comment, TransactionItemType.CostCenter);
            //            inputBox.ShowDialog();

            //        }
            //        else
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //        }

            //        if (saleOrder != null)
            //        {
            //            ProcurementRepo procurementRepo = new ProcurementRepo();

            //            if (frmInputBox.comment != "" && saleOrder.Id != 0)
            //            {
            //                if (frmInputBox.taggedUsers != null)
            //                {
            //                    foreach (var user in frmInputBox.taggedUsers)
            //                    {
            //                        notificationsRepo.Add(SystemLogic.currentUser.userName + " Tagged you in Cost Center #" + saleOrder.referenceNo, saleOrder.Id, TransactionItemType.CostCenter, frmInputBox.comment, user.id, "New Comment ");

            //                    }
            //                }
            //                procurementRepo.Add(saleOrder.Id, TransactionItemType.CostCenter, frmInputBox.Comment, SystemLogic.currentUser.employeeId);
            //                frmInputBox.taggedUsers = new List<User>();
            //                loadcomments();
            //            }
            //            else if (saleOrder.Id == 0)
            //            {
            //                DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
            //            }

            //        }
            //    }
            //    else
            //    {
            //        DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

            //    }
            //}
        }


        private void Resend_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Resend_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            var department = saleOrder.department;
            var company = saleOrder.company;

            var comment = grdCommentss.SelectedItem as CommentLog;


            if (comment != null)
            {
                comment = procurementRepo.GetComment(comment.Id);
                if (comment.employee.EmployeeUsers.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) == null)
                {
                    DXMessageBox.Show("Only sender of this comment can change Flag!");
                    return;
                }
                if (department != null && department.Id != 0 && company?.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.CostCenter);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }

                if (frmInputBox.commentAdded == true && costSheet.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);


                    //procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.PurchaseInvoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (saleOrder.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                }
            }
            loadcomments();
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            Vendorss.frmVendoradd vendoradd = new Vendorss.frmVendoradd();
            vendoradd.ShowDialog();
            loadVendors();
        }

        private void CheckHideUnHideTransactions_Checked(object sender, RoutedEventArgs e)
        {
            HidenullTransactions();

        }
        public void HidenullTransactions()
        {
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costfieldValues = grdCostItems.ItemsSource as List<CostFieldValues>;
            costfields = costfieldValues.Where(x => x.actualValue != 0 || x.SRBC!=0 || x.budgetedValue != 0 || x.revisedValue != 0 || x.adjSCost != 0  /*&& x.systemValue <= 0 && x.vendor == null*/).ToList();
            grdCostItems.ItemsSource = costfields;
            CalculateTotal();
            //revisedValue = (string.IsNullOrEmpty(txtRevisedTotal.Text)) ? 0 : Convert.ToDecimal(txtRevisedTotal.Text);
            fieldHistoryValues = costSheet.CostFieldHistories != null ? costSheet.CostFieldHistories : new List<CostFieldHistory>();
            loadcomments();
        }
        private void CheckHideUnHideTransactions_Unchecked(object sender, RoutedEventArgs e)
        {
            costFieldValue = new List<CostFieldValues>();
            foreach (var item in repo.getActiveCostSheetFields())
            {
                costFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title });
            }

            {

                grdCostItems.ItemsSource = costFieldValue;
                if (costSheet != null && costSheet.FieldValues != null)
                {
                    loadValues();
                    //txtActualTotal.Text = costSheet.TotalActualMargin.ToString();
                    //txtBudgetedTotal.Text = costSheet.TotalBudgetedMargin.ToString();
                    //txtRevisedTotal.Text = costSheet.TotalRevisedMargin.ToString();
                }
                CalculateTotal();
            }
            //revisedValue = (string.IsNullOrEmpty(txtRevisedTotal.Text)) ? 0 : Convert.ToDecimal(txtRevisedTotal.Text);
            fieldHistoryValues = costSheet.CostFieldHistories != null ? costSheet.CostFieldHistories : new List<CostFieldHistory>();
            loadcomments();
        }

        private void CheckHideActualCost_Checked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("actualValue").Visible = false;

        }

        private void CheckHideActualCost_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("actualValue").Visible = true;
        }




        private void GrdCostItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


            GridControl gc = sender as GridControl;
            var row = gc.SelectedItem as CostFieldValues;

            if (gc.CurrentColumn.FieldName == "SRBC")
            {
                try
                {
                    var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet R.S.B.C?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                    {
                        //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Update CostSheet from Bill") != null)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit RSBC from Sale Order") != null)

                        {
                            if (saleOrder.Id != 0)
                            {
                                winSelectCostSheetFields winSelectCostSheetFields = new winSelectCostSheetFields(saleOrder, true);
                                winSelectCostSheetFields.ShowDialog();
                            }
                        }
                        else
                            DXMessageBox.Show("You do not have permisssion Edit RSBC from Sale Order", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            if (gc.CurrentColumn.FieldName == "systemValue")
            {


                decimal budgetedCost = 0;
             
                ProcurementRepo repo = new ProcurementRepo();

                if (row.budgetedValue != 0)
                {
                    budgetedCost = row.budgetedValue;
                }
                else
                {
                    budgetedCost=row.SRBC;
                }
                winSystemCostingLedger ledger = new winSystemCostingLedger(row.Id, costSheet.Id, budgetedCost, Convert.ToDecimal(txtSOTotal.Text), Convert.ToDecimal(row.systemValue));

                ledger.ShowDialog();

                //Need to Show All the PO & SO Costing Windows
            }
            else    
            if(gc.CurrentColumn.FieldName== "vendor.company.CompanyName")
            {
                if (saleOrder.Id == 0 && offer.Id != 0)
                {
                    frmCostSheetFeeding feed = new frmCostSheetFeeding(offer, row);
                    feed.ShowDialog();
                    row.Id = feed.row.Id;
                    row.vendor = feed.row.vendor;
                    row.Title = feed.row.Title;
                    row.budgetedValue = feed.row.budgetedValue;
                    row.advanceValue = feed.row.advanceValue;
                    row.revisedValue = feed.row.revisedValue;
                    row.actualValue = feed.row.actualValue;
                    row.Maker = feed.row.Maker;
                    row.Origin = feed.row.Origin;
                    row.Packing = feed.row.Packing;
                    row.deliveryDate = feed.row.deliveryDate;
                    row.PaymentTerm = feed.row.PaymentTerm;
                    row.Warranty = feed.row.Warranty;
                    row.IncotermName = feed.row.IncotermName;
                    row.Dg_Goods = feed.row.Dg_Goods;
                    row.PQ = feed.row.PQ;
                    row.LoadingPort = feed.row.LoadingPort;
                    row.DestinationPort = feed.row.DestinationPort;
                    row.IntermediaryPort = feed.row.IntermediaryPort;
                    row.HScode = feed.row.HScode;
                    row.DrawaingRequired = feed.row.DrawaingRequired;
                    row.AttestedCOO = feed.row.AttestedCOO;
                    row.ST = feed.row.ST;
                    row.isExportLicense = feed.row.isExportLicense;
                    row.OC = feed.row.OC;
                    row.exchangeRate = feed.row.exchangeRate;
                    row.OCamount = feed.row.OCamount;
                    row.deliveryDays = feed.row.deliveryDays;
                    //row.vendorNature = feed.row.vendorNature;
                    grdCostItems.RefreshData();
                }
                else
                {
                    frmCostSheetFeeding feed = new frmCostSheetFeeding(saleOrder, row);
                    feed.ShowDialog();
                    row.Id = feed.row.Id;
                    row.vendor = feed.row.vendor;
                    row.Title = feed.row.Title;
                    row.budgetedValue = feed.row.budgetedValue;
                    row.advanceValue = feed.row.advanceValue;
                    row.revisedValue = feed.row.revisedValue;
                    row.actualValue = feed.row.actualValue;
                    row.Maker = feed.row.Maker;
                    row.Origin = feed.row.Origin;
                    row.Packing = feed.row.Packing;
                    row.deliveryDate = feed.row.deliveryDate;
                    row.PaymentTerm = feed.row.PaymentTerm;
                    row.Warranty = feed.row.Warranty;
                    row.IncotermName = feed.row.IncotermName;
                    row.Dg_Goods = feed.row.Dg_Goods;
                    row.PQ = feed.row.PQ;
                    row.LoadingPort = feed.row.LoadingPort;
                    row.DestinationPort = feed.row.DestinationPort;
                    row.IntermediaryPort = feed.row.IntermediaryPort;
                    row.HScode = feed.row.HScode;
                    row.DrawaingRequired = feed.row.DrawaingRequired;
                    row.AttestedCOO = feed.row.AttestedCOO;
                    row.ST = feed.row.ST;
                    row.isExportLicense = feed.row.isExportLicense;
                    row.OC = feed.row.OC;
                    row.exchangeRate = feed.row.exchangeRate;
                    row.OCamount = feed.row.OCamount;
                    row.deliveryDays = feed.row.deliveryDays;
                    //row.vendorNature = feed.row.vendorNature;
                    grdCostItems.RefreshData();
                }
            }
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (saleOrder.Id != 0)
            {
                if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.InterCompany?.Id != 0 /*&& saleOrder.chkInterCompany.IsChecked == true*/ && saleOrder.InterDepartment != null && saleOrder.InterDepartment.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByDepartmentIdsList(new List<int> { saleOrder.department.Id, saleOrder.InterDepartment.Id }, new List<int> { saleOrder.company.Id, saleOrder.InterCompany.Id }), TransactionItemType.CostCenter, saleOrder);
                    inputBox.ShowDialog();
                }
                else if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 /*&& chkInterCompany.IsChecked != true*/)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, usersRepo.getusersByCompanyDepartment(saleOrder.department.Id, saleOrder.company.Id), TransactionItemType.CostCenter, saleOrder);
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                }

                if (costSheet.Id != 0)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && costSheet.Id != 0)
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO CostSheet #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, frmInputBox.comment, user.id, null, "New Comment ", frmInputBox.billReference, frmInputBox.poReference);

                            }
                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO CostSheet #" + saleOrder.SalesReferenceNo, costSheet.Id, TransactionItemType.CostCenter, frmInputBox.comment, null,user.id, "New Comment ", frmInputBox.billReference, frmInputBox.poReference);

                            }
                        }
                        procurementRepo.Add(costSheet.Id, TransactionItemType.CostCenter, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.billReference, frmInputBox.poReference);
                        loadcomments();
                    }
                    else if (saleOrder.Id == 0)
                    {
                        DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                    }

                }
            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (saleOrder.Id != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(saleOrder.Id, TransactionItemType.Sale_Order);
                trackingWindow.ShowDialog();
            }
        }

        private void GrdCostItems_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValue")
                    {
                        if (txtTotalBudgetedCost != null && txtTotalBudgetedCost != null)
                        {
                            switch (e.SummaryProcess)
                            {

                                case CustomSummaryProcess.Start:
                                    sum = 0;
                                    break;
                                case CustomSummaryProcess.Calculate:
                                    //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                    //if (shouldSum)
                                    //{
                                    var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                    sum = sum + fieldValue;
                                    //}
                                    break;
                                case CustomSummaryProcess.Finalize:
                                    if (sum != 0)
                                    {

                                        costSheet.TotalBudgetedMargin = sum;
                                        txtTotalBudgetedCost.Text = sum.ToString();

                                        sum = Convert.ToDecimal(txtSOTotal.Text) - sum;
                                    }
                                    e.TotalValue = sum;

                                    txtTotalBudgetedCostMargin.Text = sum.ToString();

                                    break;
                            }
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "revisedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "revisedValue")
                    {
                      

                            switch (e.SummaryProcess)
                            {
                                case CustomSummaryProcess.Start:
                                    sum = 0;
                                    break;
                                case CustomSummaryProcess.Calculate:
                                    //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                    //if (shouldSum)
                                    //{
                                    var fieldValue = Convert.ToDecimal(e.GetValue("revisedValue"));
                                    sum = sum + fieldValue;
                                    //}
                                    break;
                                case CustomSummaryProcess.Finalize:
                                    if (sum != 0)
                                    {
                                        costSheet.TotalRevisedMargin = sum;

                                        sum = Convert.ToDecimal(txtSOTotal.Text) - sum;
                                    }
                                    e.TotalValue = sum;



                                    break;
                            }
                        
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBC")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBC")
                    {
                       if(txtTotalRSBCMargin!=null)
                            switch (e.SummaryProcess)
                            {
                                case CustomSummaryProcess.Start:
                                    sum = 0;
                                    break;
                                case CustomSummaryProcess.Calculate:
                                    //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                    //if (shouldSum)
                                    //{
                                    var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));
                                    var budgetFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                    if (fieldValue != 0)
                                    {
                                        sum = sum + fieldValue;
                                    }
                                    else
                                    {
                                        sum = sum + budgetFieldValue;
                                    }
                                    //}
                                    break;
                                case CustomSummaryProcess.Finalize:
                                    if (sum != 0)
                                    {
                                        txtTotalRSBC.Text = sum.ToString();

                                        sum = Convert.ToDecimal(txtSOTotal.Text) - sum;
                                        e.TotalValue = sum;
                                    }
                                    else
                                        e.TotalValue = sum;
                                    txtTotalRSBCMargin.Text = sum.ToString();



                                    break;
                            }
                        
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "actualValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "actualValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValue"));
                                sum = sum + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {
                                    costSheet.TotalActualMargin = sum;

                                    sum = Convert.ToDecimal(txtSOTotal.Text) - sum;
                                }
                                e.TotalValue = sum;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValue")
                    {
                        if (txtTotalSystemMargin != null)

                            switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));
                                var adjSCOst = Convert.ToDecimal(e.GetValue("adjSCost"));
                                sum = sum + fieldValue ;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {
                                    txtTotalSystemCost.Text =  sum.ToString();

                                    sum = Convert.ToDecimal(txtSOTotal.Text) - sum;
                                }
                                e.TotalValue = sum;
                                txtTotalSystemMargin.Text = sum.ToString();

                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValuePerc")
                    {
                        if(txtTotalBudgetedCostPerc!=null)
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtSOTotal.Text) - sum) / Convert.ToDecimal(txtSOTotal.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;
                                txtTotalBudgetedCostPerc.Text = sum.ToString();

                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBCValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBCValuePerc")
                    {
                        if (txtTotalRSBCPerc != null)

                            switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));

                                var revisedFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                if (fieldValue != 0)
                                {
                                    sum = sum + fieldValue;
                                }
                                else
                                if (revisedFieldValue != 0)
                                {
                                    sum = sum + revisedFieldValue;
                                }
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {
                                    var percent = (((Convert.ToDecimal(txtSOTotal.Text) - sum) / Convert.ToDecimal(txtSOTotal.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;
                                txtTotalRSBCPerc.Text = sum.ToString();
                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValuePerc")
                    {
                        if (txtTotalSystemPerc != null)

                            switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));
                                
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtSOTotal.Text) - sum) / Convert.ToDecimal(txtSOTotal.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;
                                txtTotalSystemPerc.Text = sum.ToString();


                                break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        public class DataStore<T>
        {
            public T Data { get; set; }
        }

        private void GrdCostItems_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            try 
            { 
              
           


                if (!string.IsNullOrEmpty(txtSOTotal.Text))
            {
                var totalSO = Convert.ToDecimal(txtSOTotal.Text);

                    if (e.Column.FieldName == "budgetedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("budgetedValue") != null)
                        {
                            decimal budgetValue = Convert.ToDecimal(e.GetListSourceFieldValue("budgetedValue"));
                            var percent = (((/*totalSO - */budgetValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);

                            //e.Value += "%";
                        }
                    }
                    else
                if (e.Column.FieldName == "revisedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("revisedValue") != null)
                        {
                            decimal revisedValue = Convert.ToDecimal(e.GetListSourceFieldValue("revisedValue"));

                            var percent = (((/*totalSO -*/revisedValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }
                    else
                if (e.Column.FieldName == "SRBCValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("SRBC") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("SRBC"));

                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);

                            //e.Value += "%";

                        }
                    }
                    else
                if (e.Column.FieldName == "systemValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("systemValue") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("systemValue"));

                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);

                            //e.Value += "%";

                        }
                    }
                
            }
            }
            catch (Exception ex)
            {

            }
        }

        private void CheckHideRevisedCost_Checked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("revisedValuePerc").Visible = false;
        }

        private void CheckHideRevisedCost_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("revisedValuePerc").Visible = true;
        }

        private void CheckPOTerms_Checked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("Maker").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("Origin").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("Packing").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("deliveryDate").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("PaymentTerm.term").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("Warranty.name").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("IncotermName.termName").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("Dg_Goods").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("LoadingPort").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("DestinationPort").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("PQ.Name").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("HScode").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("DrawaingRequired").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("AttestedCOO").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("ST.Name").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("isExportLicense").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("IntermediaryPort").Visible = false;
        }

        private void CheckPOTerms_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("Maker").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("Origin").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("Packing").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("deliveryDate").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("PaymentTerm.term").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("Warranty.name").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("IncotermName.termName").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("Dg_Goods").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("LoadingPort").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("DestinationPort").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("PQ.Name").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("ST.Name").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("HScode").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("DrawaingRequired").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("AttestedCOO").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("isExportLicense").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("IntermediaryPort").Visible = true;

        }

        private void CheckCostSheetFields_Checked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("budgetedValuePerc").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("revisedValuePerc").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("SRBCValuePerc").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("systemValuePerc").Visible = false;




            grdCostItems.Columns.GetColumnByFieldName("vendor.company.CompanyName").Visible = false;

            grdCostItems.Columns.GetColumnByFieldName("Title").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("budgetedValuePerc").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("revisedValuePerc").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("actualValue").Visible = false;

            grdCostItems.Columns.GetColumnByFieldName("SRBC").Visible = false;

            grdCostItems.Columns.GetColumnByFieldName("SRBCValuePerc").Visible = false;

            grdCostItems.Columns.GetColumnByFieldName("systemValue").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("systemValuePerc").Visible = false;

            grdCostItems.Columns.GetColumnByFieldName("OC.CurrencyName").Visible = false;

            grdCostItems.Columns.GetColumnByFieldName("OCamount").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("exchangeRate").Visible = false;




        }

        private void CheckCostSheetFields_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("budgetedValuePerc").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("revisedValuePerc").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("SRBCValuePerc").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("systemValuePerc").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("vendor.company.CompanyName").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("Title").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("budgetedValue").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("budgetedValuePerc").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("revisedValue").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("revisedValuePerc").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("actualValue").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("SRBC").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("SRBCValuePerc").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("systemValue").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("systemValuePerc").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("OC.CurrencyName").Visible = true;

            grdCostItems.Columns.GetColumnByFieldName("OCamount").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("exchangeRate").Visible = true;

        }

        private void BtnCreatePO_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                PurchaseOrderRepo repo = new PurchaseOrderRepo();


                var paramtere = grdCostItems.GetFocusedRow() as CostFieldValues;
                var po = repo.get(costSheet.Id, paramtere.Id);
                if (costSheet.Id != 0)
                {
                    if (po == null && costSheet.Id != 0)
                    {
                        DXWindow win = new DXWindow();
                        SaleOrderss.ucStatuschange.saleOrderid = saleOrder.Id;
                        SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange();
                        statusChange.Owner = this;
                        statusChange.ShowDialog();
                        PurchaseOrderss.ucPurchaseOrderAdd ucPOAdd = new PurchaseOrderss.ucPurchaseOrderAdd();
                        ucPOAdd.saleOrderid = saleOrder.Id;
                        ucPOAdd.poVendor = paramtere.vendor;
                        ucPOAdd.poCurrency = paramtere.OC;
                        ucPOAdd.poOCamount = paramtere.OCamount;
                        ucPOAdd.poMaker = paramtere.Maker;
                        ucPOAdd.poOrigin = paramtere.Origin;
                        ucPOAdd.poDeliveryDate = paramtere.deliveryDate;
                        ucPOAdd.poPaymentTerm = paramtere.PaymentTerm;
                        ucPOAdd.poWarranty = paramtere.Warranty;
                        ucPOAdd.costSheetFieldId = paramtere.Id;
                        ucPOAdd.costSheetId = costSheet.Id;
                        
                        //I have to check the values in PO columns
                        //Have to close SO window and Cost sheet
                        if (paramtere.IncotermName != null)
                        {
                            if (paramtere.IncotermName.Id != 0)
                            {
                                IncotermRepo incotermRepo = new IncotermRepo();
                                var incoterm = incotermRepo.get((int)paramtere.IncotermName.Id);
                                ucPOAdd.poIncoTerm = incoterm;
                            }
                        }
                        win.Content = ucPOAdd;
                        win.Show();
                        this.Close();
                    }
                    else
                    {
                        DXMessageBox.Show("Purchase Order already exists, Please select different supplier.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    } 
                }
                else
                {
                    DXMessageBox.Show("Please attach costsheet first.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }


            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Visible)
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
            else
            if (gridTracker.Visibility == Visibility.Collapsed && costSheet.Id != 0)
            {
                UsersRepo UsersRepo = new UsersRepo();
                views = UsersRepo.getViwerInfo(costSheet.Id, (int)TransactionItemType.CostCenter);
                grdUsers.ItemsSource = views;
                gridTracker.Visibility = Visibility.Visible;
            }
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {
                

                    string fname = viewInfo.User.employee.person.FName;
                    string lname = viewInfo.User.employee.person.LName;

                    

                    e.Value = fname + " " + lname;
                
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
        }

        private void btnExpand_Click_1(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;
            grdTrackingTree.ExpandAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;

        }

        private void btnCollapsed_Click(object sender, EventArgs e)
        {
            grdOrdersTracking.ShowLoadingPanel = true;

            grdTrackingTree.CollapseAllNodes();
            grdOrdersTracking.ShowLoadingPanel = false;
        }

        private void TreeListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = grdOrdersTracking;
            if (grid.SelectedItem != null)
            {

                var item = grid.SelectedItem as AllOrdersView;

                if (item.transactionType == TransactionItemType.STL)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL") != null)
                    {
                        STLRepo sTLRepo = new STLRepo();

                        var selectedStl = sTLRepo.Get(Convert.ToInt32(item.Id));
                        winSTLAdd stl = new winSTLAdd(true, Convert.ToInt32(selectedStl.paymentGroupId));
                        stl.stl = selectedStl;
                        stl.Show();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View STL");
                    }
                }
                else

                if (item.transactionType == TransactionItemType.InterBank_Transfer)
                {
                    try
                    {
                        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
                        {

                            var selectedBankTransfer = bankTransRepo.GetInterBankTransfer(item.Id);

                            if (selectedBankTransfer != null)
                            {
                                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                                CompanyRepo compRepo = new CompanyRepo();
                                bankTransRepo = new InterBankTransRepo();

                                //ucFrmBankTransfer.bankTransfer = new InterBankTransfer();
                                ucFrmBankTransfer.bankTransferId = selectedBankTransfer.Id; //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedBankTransfer.interBankTransStatus.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inter-Bank Transfer") != null)
                                    {
                                        ucFrmBankTransfer.editFlag = true;

                                        ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                        ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                                        ucFrmBankTransfer.frmBankTranfer.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Inter-Bank Transfer!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBankTransfer.editFlag = true;

                                    ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                                    ucFrmBankTransfer.frmBankTranfer.Title = "Inter-Bank Transfer";
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;

                                    ucFrmBankTransfer.frmBankTranfer.Show();
                                }


                            }
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Inter-Bank Transfer!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    if (item.transactionType == TransactionItemType.Sale_Receipt)
                    {
                        GrdSaleReceiptListLoad(item.Id);
                        return;
                    }

                    if (item.transactionType == TransactionItemType.Tasks)
                    {
                        ucTaskAdd taskAdd = new ucTaskAdd();
                        taskAdd.taskId = Convert.ToInt32(item.Id);
                        taskAdd.editFlag = true;
                        Window win = new Window();
                        win.Content = taskAdd;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }

                    if (item.transactionType == TransactionItemType.LoansAdvances)
                    {
                        ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                        frmLoansAdvances.loansAdvanceId = Convert.ToInt32(item.Id);
                        frmLoansAdvances.editFlag = true;
                        Window win = new Window();
                        win.Content = frmLoansAdvances;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }

                    if (item.transactionType == TransactionItemType.TargetReward)
                    {
                        ucFrmBasicTargetRewards frmTargetRewards = new ucFrmBasicTargetRewards();
                        frmTargetRewards.rewardId = Convert.ToInt32(item.Id);
                        //frmTargetRewards.editFlag = true;
                        Window win = new Window();
                        win.Content = frmTargetRewards;
                        win.WindowState = WindowState.Maximized;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.Show();
                        return;
                    }


                    if (item.transactionType == TransactionItemType.Admin_Bill)
                    {
                        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                        AdminBillsRepo billsRepo = new AdminBillsRepo();
                        DXWindow frmBill = new DXWindow();

                        var bill = billsRepo.GetBill(item.Id);

                        frmBillAdd = new ucFrmBillAdd();

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBill.Content = frmBillAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmBill.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmBill.Title = "Admin Bill";
                                    frmBill.Show();
                                }
                                else
                                {
                                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                    return;
                                }
                            }
                            else
                            {
                                frmBillAdd.editFlag = true;
                                frmBillAdd.groupId = bill.transactionGroupId;
                                frmBill.Content = frmBillAdd;
                                //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                frmBill.WindowState = WindowState.Maximized;
                                //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                frmBill.Title = "Admin Bill";
                                frmBill.Show();
                            }
                        }
                        return;
                    }
                    if (item.transactionType == TransactionItemType.Payments)
                    {
                        ucFrmPayments frmPayments = new ucFrmPayments();
                        ucFrmBillPaymentAdd ucFrmBillPayment = new ucFrmBillPaymentAdd();
                        ucFrmPInvoicePaymentAdd frmPInvoicePaymentAdd = new ucFrmPInvoicePaymentAdd();
                        ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                        ucFrmTargetRewardPayment frmTRpayment = new ucFrmTargetRewardPayment();
                        CompanyRepo compRepo = new CompanyRepo();
                        PaymentRepo paymentRepo = new PaymentRepo();


                        var payment = paymentRepo.GetPayment(item.Id);
                        //var payments = paymentRepo.GetPaymentsByGroupId(payment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;



                        switch (payment.transactionType)
                        {
                            case PaymentTransactionType.Loans_Advances:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmLApayment.editFlag = true;
                                        frmLApayment.groupId = payment.transactionGroupId;
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
                                    frmLApayment.groupId = payment.transactionGroupId;
                                    frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                    frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                    frmLApayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Target_Reward:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRpayment.editFlag = true;
                                        frmTRpayment.groupId = payment.transactionGroupId;
                                        frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                        frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRpayment.editFlag = true;
                                    frmTRpayment.groupId = payment.transactionGroupId;
                                    frmTRpayment.frmPiPaymentWindow.Content = frmTRpayment;
                                    frmTRpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRpayment.frmPiPaymentWindow.Show();
                                }
                                break;
                            case PaymentTransactionType.Admin_Bills:

                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = payment.transactionGroupId;
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
                                    frmPayments.groupId = payment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Vendor_Bills:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        ucFrmBillPayment.editFlag = true;
                                        ucFrmBillPayment.groupId = payment.transactionGroupId;
                                        ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                        ucFrmBillPayment.frmBillPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    ucFrmBillPayment.editFlag = true;
                                    ucFrmBillPayment.groupId = payment.transactionGroupId;
                                    ucFrmBillPayment.frmBillPaymentWindow.Content = ucFrmBillPayment;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    ucFrmBillPayment.frmBillPaymentWindow.Title = "Payments";
                                    ucFrmBillPayment.frmBillPaymentWindow.Show();
                                }
                                break;

                            case PaymentTransactionType.Purchase_Invoice:
                                if (payment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPInvoicePaymentAdd.editFlag = true;
                                        frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPInvoicePaymentAdd.editFlag = true;
                                    frmPInvoicePaymentAdd.groupId = payment.transactionGroupId;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Content = frmPInvoicePaymentAdd;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Title = "Payments";
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.Show();
                                }
                                break;
                        }
                        return;
                    }
                    Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel((TransactionItemType)Enum.Parse(typeof(TransactionItemType), item.transactionType.ToString()), item.Id);
                    procurmentPanele.Show();
                }
            }
        }
        public void GellAllOrdersTracking()
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            if (saleOrder != null && saleOrder.Id!=0)
            {
                trackingOrder = saleOrderRepo.get(saleOrder.Id);
                if (trackingOrder != null)
                {
                    OrderTracking tracking = new OrderTracking();
                    grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.Sale_Order);
                }
            }
        }
        private void btnStageTracking_Click(object sender, RoutedEventArgs e)
        {
            if (gridOrderStageTrack.Visibility == Visibility.Collapsed)
            {
                GellAllOrdersTracking();
                grdTrackingTree.ExpandAllNodes();

                gridOrderStageTrack.Visibility = Visibility.Visible;

            }
            else
            {
                gridOrderStageTrack.Visibility = Visibility.Collapsed;
            }
        }
        private void GrdSaleReceiptListLoad(int id)
        {
            try
            {
                var repo = new SalesReceiptRepo();
                var saleReceipt = repo.GetSalesReceipt(id);

                if (saleReceipt.receiptType == ReceiptType.Loans_Advances)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                    {
                        ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
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
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                    {
                        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                        updateSaleReceiptObj.saveEditFlag = 1;


                        if (saleReceipt == null)
                        {
                            return;
                        }

                        if (saleReceipt.saleReceiptStatus != null)
                        {
                            var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                            updateSaleReceiptObj.selectedStatus = status;
                        }

                        updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
                        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                        updateSaleReceiptObj.receiptId = saleReceipt.Id;
                        updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;


                        //updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                        ////ucFrmAddAccount obj = new ucFrmAddAccount();
                        //updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                        //updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                        //updateSaleReceiptObj.enter_receipt_win.Show();

                        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanMinimize;
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

                        updateSaleReceiptObj.enter_receipt_win.ShowDialog();
                        //Load_Receipts();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                        return;
                    }
                }



            }
            catch
            {

            }

            //MessageBox.Show("Mission Successful!");
        }
        public void PushComment(CommentLog Comment)
        {
            if (costSheet != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                if (costSheet.Id != 0)
                {
                    if (Comment != null)
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

        private void checkVendorProfile_Checked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("vendor.vendorNature.name").Visible = false;
            grdCostItems.Columns.GetColumnByFieldName("vendor.Rating").Visible = false;
            //grdCostItems.Columns.GetColumnByFieldName("vendor.company.industryType.name").Visible = false;


        }

        private void checkVendorProfile_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCostItems.Columns.GetColumnByFieldName("vendor.vendorNature.name").Visible = true;
            grdCostItems.Columns.GetColumnByFieldName("vendor.Rating").Visible = true;
            //grdCostItems.Columns.GetColumnByFieldName("vendor.company.industryType.name").Visible = true;
        }

        private void btnPOTracking_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Tracking By PO") != null)
            {
                if (gridVendorProfileByPO.Visibility == Visibility.Collapsed)
                {
                    gridVendorProfileByPO.Visibility = Visibility.Visible;
                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdVendorProfileByPO);
                    LoadPendingInvoices();
                }
                else
                {
                    gridVendorProfileByPO.Visibility = Visibility.Collapsed;

                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Vendor Tracking By PO");

            }
        }
        public void LoadVendorProfile(string _type)
        {
            var type = _type;
            if (!string.IsNullOrEmpty(type))
            {
                switch (type)
                {
                    case "Open":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetOpenPurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id);
                                if (poInvoicesByVendor != null)
                                {
                                    allPendingInvoicesOpen = new List<PendingOrder>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingOrder pInvoice = new PendingOrder();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        if (purchaseInvoice.isVoid == true)
                                        {
                                             pInvoice.stage = "Void";
                                        }
                                        else if (purchaseInvoice.isReApproved == false)
                                        {
                                             pInvoice.stage = "Under Approval";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.stage == "Closed")
                                        {
                                             pInvoice.stage = "Closed";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.PurchaseOrderStatus.isActive == false && purchaseInvoice.PendingForClosing != true)
                                        {
                                             pInvoice.stage = "Closed";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.PendingForClosing == true)
                                        {
                                             pInvoice.stage = "Under Closing";
                                        }
                                        else if (purchaseInvoice.isApproved == true)
                                        {
                                             pInvoice.stage = "Approved";
                                        }
                                        else if (purchaseInvoice.isApproved == false)
                                        {
                                             pInvoice.stage = "Under Approval";
                                        }
                                        else if (purchaseInvoice.PendingForClosing == true)
                                        {
                                             pInvoice.stage = "Under Closing";
                                        }
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;
                                        allPendingInvoicesOpen.Add(pInvoice);
                                    }
                                    allPendingInvoicesOpen = allPendingInvoicesOpen
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoicesOpen;
                                }
                            }
                            break;
                        }
                    case "Close":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetClosePurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id);
                                if (poInvoicesByVendor != null)
                                {
                                    allPendingInvoicesClose = new List<PendingOrder>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingOrder pInvoice = new PendingOrder();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        var invoicedAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(purchaseInvoice.totalCFRValue + purchaseInvoice.totaltaxAmount - invoicedAmount, 2);
                                        var paidAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));
                                        var unPaidAmount = Math.Round(invoicedAmount - paidAmount, 2);
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        if (purchaseInvoice.isVoid == true)
                                        {
                                            pInvoice.stage = "Void";
                                        }
                                        else if (purchaseInvoice.isReApproved == false)
                                        {
                                            pInvoice.stage = "Under Approval";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.stage == "Closed")
                                        {
                                            pInvoice.stage = "Closed";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.PurchaseOrderStatus.isActive == false && purchaseInvoice.PendingForClosing != true)
                                        {
                                            pInvoice.stage = "Closed";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.PendingForClosing == true)
                                        {
                                            pInvoice.stage = "Under Closing";
                                        }
                                        else if (purchaseInvoice.isApproved == true)
                                        {
                                            pInvoice.stage = "Approved";
                                        }
                                        else if (purchaseInvoice.isApproved == false)
                                        {
                                            pInvoice.stage = "Under Approval";
                                        }
                                        else if (purchaseInvoice.PendingForClosing == true)
                                        {
                                            pInvoice.stage = "Under Closing";
                                        }
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;
                                        allPendingInvoicesClose.Add(pInvoice);
                                    }
                                    allPendingInvoicesClose = allPendingInvoicesClose
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoicesClose;
                                }
                            }
                            break;
                        }
                    case "All":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetAllPurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id);
                                if (poInvoicesByVendor != null)
                                {
                                    allPendingInvoicesAll = new List<PendingOrder>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingOrder pInvoice = new PendingOrder();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;

                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        if (purchaseInvoice.isVoid == true)
                                        {
                                            pInvoice.stage = "Void";
                                        }
                                        else if (purchaseInvoice.isReApproved == false)
                                        {
                                            pInvoice.stage = "Under Approval";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.stage == "Closed")
                                        {
                                            pInvoice.stage = "Closed";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.PurchaseOrderStatus.isActive == false && purchaseInvoice.PendingForClosing != true)
                                        {
                                            pInvoice.stage = "Closed";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.PendingForClosing == true)
                                        {
                                            pInvoice.stage = "Under Closing";
                                        }
                                        else if (purchaseInvoice.isApproved == true)
                                        {
                                            pInvoice.stage = "Approved";
                                        }
                                        else if (purchaseInvoice.isApproved == false)
                                        {
                                            pInvoice.stage = "Under Approval";
                                        }
                                        else if (purchaseInvoice.PendingForClosing == true)
                                        {
                                            pInvoice.stage = "Under Closing";
                                        }
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;
                                        allPendingInvoicesAll.Add(pInvoice);
                                    }
                                    allPendingInvoicesAll = allPendingInvoicesAll
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoicesAll;
                                }
                            }
                            break;
                        }
                    case "Pending for Approval":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetPendingForApprovalPurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id);
                                if (poInvoicesByVendor != null)
                                {
                                    allPendingInvoicesPendingforApproval = new List<PendingOrder>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingOrder pInvoice = new PendingOrder();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        if (purchaseInvoice.isVoid == true)
                                        {
                                            pInvoice.stage = "Void";
                                        }
                                        else if (purchaseInvoice.isReApproved == false)
                                        {
                                            pInvoice.stage = "Under Approval";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.stage == "Closed")
                                        {
                                            pInvoice.stage = "Closed";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.PurchaseOrderStatus.isActive == false && purchaseInvoice.PendingForClosing != true)
                                        {
                                            pInvoice.stage = "Closed";
                                        }
                                        else if (purchaseInvoice.isApproved == true && purchaseInvoice.PendingForClosing == true)
                                        {
                                            pInvoice.stage = "Under Closing";
                                        }
                                        else if (purchaseInvoice.isApproved == true)
                                        {
                                            pInvoice.stage = "Approved";
                                        }
                                        else if (purchaseInvoice.isApproved == false)
                                        {
                                            pInvoice.stage = "Under Approval";
                                        }
                                        else if (purchaseInvoice.PendingForClosing == true)
                                        {
                                            pInvoice.stage = "Under Closing";
                                        }
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;
                                        allPendingInvoicesPendingforApproval.Add(pInvoice);
                                    }
                                    allPendingInvoicesPendingforApproval = allPendingInvoicesPendingforApproval
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoicesPendingforApproval;
                                }
                            }
                            break;
                        }
                }
            }

            //if(allPendingInvoicesAll!=null)
            //{
            //    int piAllcount = allPendingInvoicesAll.Count();


            //}
            //if (allPendingInvoicesOpen != null)
            //{
            //    int piAllcountOpen = allPendingInvoicesOpen.Count();


            //}
            //if (allPendingInvoicesClose != null)
            //{
            //    int piAllcountClose = allPendingInvoicesClose.Count();


            //}
            //if (allPendingInvoicesAll != null)
            //{
            //    int piAllcountPendingforApproval = allPendingInvoicesAll.Count();


            //}
            int piAllcount= allPendingInvoicesAll.Count();
            int piAllcountOpen= allPendingInvoicesOpen.Count();
            int piAllcountClose= allPendingInvoicesClose.Count();
            int piAllcountPendingforApproval = allPendingInvoicesPendingforApproval.Count();
            btnOpenVendorProfile.Content="Open ("+piAllcountOpen.ToString()+")";
            btnCloseVendorProfile.Content = "Close (" + piAllcountClose.ToString() + ")";
            btnAllVendorProfile.Content = "All (" + piAllcount.ToString() + ")";
            btnPendingForApprovalVendorProfile.Content = "PendingforApproval (" + piAllcountPendingforApproval.ToString() + ")";
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void LoadPendingInvoices()
        {
            try
            {
                if (saleOrder.CostSheet != null)
                {
                    var fields = saleOrder.CostSheet.FieldValues.Where(x => x.Type == 4).ToList();
                    if (fields.Count > 0)
                    {
                        List<Vendor> vendors = new List<Vendor>();
                        VendorRepo vendorRepo = new VendorRepo();
                        foreach (var vendor in fields)
                        {
                            vendors.Add(vendorRepo.Get((int)vendor.Value));

                        }
                        lookupSelectVendor.ItemsSource = vendors;
                        lookupSelectVendor.SelectedIndex = 0;
                    }
                }
                LoadVendorProfile("Close");
                LoadVendorProfile("Pending for Approval");
                LoadVendorProfile("Open");
                LoadVendorProfile("All");
                btnVendorProfileByPO.Content = "Vendor Profile By PO (All)";
            }
            catch (Exception)
            {
            }
        }
        private void btnOpenVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Open");

        }

        private void btnCloseVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Close");
        }

        private void btnPendingForApprovalVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Pending for Approval");

        }

        private void btnAllVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("All");

        }

        private void lookupSelectVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LoadPendingInvoices();
            //if (lookupSelectVendor.SelectedIndex > -1)
            //{
            //    var vendor = lookupSelectVendor.SelectedItem as Vendor;
            //    switch (vendor.Rating)
            //    {
            //        case 0:
            //            txtTotalRatingRed.Text = vendor.Rating.ToString();
            //            break;
            //        case 2:
            //            txtTotalRatingGray.Text = vendor.Rating.ToString();
            //            break;
            //        case 3:
            //            txtTotalRatingBlue.Text = vendor.Rating.ToString();
            //            break;
            //        case 4:
            //            txtTotalRatingGreen.Text = vendor.Rating.ToString();
            //            break;
            //        case 5:
            //            txtTotalRatingGold.Text = vendor.Rating.ToString();
            //            break;
            //    }
            //}
        }
        private void btnSavePOLayout_Click(object sender, EventArgs e)
        {

        }

        private void btnVendorProfileByPO_Click(object sender, EventArgs e)
        {

        }

        private void grdVendorProfileByPO_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Stage" && e.IsGetData)
            {
                var purchaseOrder = grdVendorProfileByPO.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseOrder;
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
        }

        private void btnLinkedSO_Click(object sender, RoutedEventArgs e)
        {
            if (saleOrder.Id != 0)
            {
                frmMergeMoudlesTracking trackingWindow = new frmMergeMoudlesTracking(saleOrder.Id, TransactionItemType.Sale_Order, true);
                trackingWindow.ShowDialog();
            }
        }
    }
    public class PendingOrder
    {
        public int Id { get; set; }
        public string refNo { get; set; }
        public string currencyName { get; set; }
        public double agingDays { get; set; }
        public string customer { get; set; }
        public string vendor { get; set; }
        public string stage { get; set; }
        public SaleOrderStatus saleInvoiceStatus { get; set; }
        public PurchaseOrderStatus PurchaseInvoiceStatus { get; set; }
    }

    public class IncoTermName
    {
        public int Id { get; set; }
        public string termName { get; set; }
        public string discription { get; set; }
        public bool isActive { get; set; } = true;
    };
}

