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
using ERP_BL.Enums;
using ERP_BL.Config;
using System.Data;
using System.Text.RegularExpressions;
using DevExpress.Xpf.LayoutControl;
using System.IO;
using DevExpress.Xpf.Core;
using System.Windows.Media.Animation;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;
using System.Diagnostics;
using ZAS_ERP.Utils;
using ERP_BL.Tax;
using ERP_BL.ExchangeRates;
using ERP_BL.Procurements;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.ToDoTasks.Taskss;
using ZAS_ERP.Procurementss.Budget;
using ZAS_ERP.Procurementss.SaleOrderss.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ERP_BL.Payments;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Bankings;
using ERP_BL.Migrations;
using ERP_BL.Procurements.AdminBills;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using System.Runtime.ConstrainedExecution;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ERP_BL.Procurements.LoansAdvances;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    public class CostFieldValues
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal budgetedValue { get; set; }
        public decimal advanceValue { get; set; }
        public decimal revisedValue { get; set; }

        public decimal actualValue { get; set; }
        public decimal systemValue { get; set; }
        public decimal addedSystemValue { get; set; }


        public Vendor vendor { get; set; }
        //public decimal systemPayment { get; set; }
        public decimal billAmount { get; set; }
        public decimal poAmount { get; set; }
        public decimal siAmount { get; set; }
        public decimal addedSRBCValue { get; set; }
        public decimal SRBC { get; set; }
        public decimal soAmountSRBC { get; set; }
        public string Maker { get; set; }
        public string Origin { get; set; }
        public bool Packing { get; set; }
        public DateTime? deliveryDate { get; set; }
        public PaymentTerm PaymentTerm { get; set; }
        public IncoTermName IncotermName { get; set; }
        public Warranty Warranty { get; set; }
        public Currency OC { get; set; }
        public decimal OCamount { get; set; }
        public string LoadingPort { get; set; }
        public string DestinationPort { get; set; }
        public PQDocument PQ { get; set; }
        public ShippingTerm ST { get; set; }
        public string HScode { get; set; }
        public bool DrawaingRequired { get; set; }
        public string AttestedCOO { get; set; }
        public decimal exchangeRate { get; set; }
        public bool isExportLicense { get; set; }
        public bool Dg_Goods { get; set; } = false;

        public decimal receiptAmount { get; set; }
        public decimal paymentAmount { get; set; }
        public string IntermediaryPort { get; set; }
        public string deliveryDays { get; set; }
        public decimal adjSCost { get; set; }
        public bool poClosed { get; set; }
    }



    public class CostAdjustedFieldValues
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal addedAdjustedValue { get; set; }
        public decimal adjSCost { get; set; }

    }
    public class CostBudgetFieldValues
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal addedBudgetValue { get; set; }
        public decimal budgetCost { get; set; }

    }  
    public class CostVendorFieldValues
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string addedTitle { get; set; }

    }
    /// <summary>
    /// Interaction logic for ucInquiryAdd.xaml
    /// </summary>
    public partial class ucSaleOrderAdd : UserControl
    {
        public static int editsaleOrder;
        public static int saleOrderid;
        public static int parentSaleOrderid;
        public int OrderId;
        public int editOrder;
        public static int offerid;
        //CurrentUserSetting streamConverter;
        Offer offer = new Offer();
        public static int moduleContractid;
        ERP_BL.Procurements.ModuleContract ModuleContract = new ERP_BL.Procurements.ModuleContract();
        InterBankTransfer interBankTransfer = new InterBankTransfer();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        SaleOrder saleOrder = new SaleOrder(); 
        SaleOrder trackingOrder = new SaleOrder(); 
        SaleOrder parentSaleOrder = new SaleOrder();

        Vendor vendor = new Vendor();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        CustomerCompany customer = new CustomerCompany();
        public bool isloading = false;
        public virtual List<ProcurementProduct> procurementProducts { get; set; }
        public virtual List<BookerStatementItem> BookerStatementItems { get; set; }

        public virtual List<Product> products { get; set; }
        Principal principal = new Principal();
        Currency currency = new Currency();
        UsersRepo UsersRepo = new UsersRepo();
        bool addinfo = true;
        List<ViewInfo> views = new List<ViewInfo>();
        public SaleOrderStatus checkStatus = new SaleOrderStatus();
        public double SOCFRRemaining = 0;
        public SaleOrderStatus soStatus;
        public ProcurementRepo procurementRepo = new ProcurementRepo();

        ERP_BL.Databases.Company InterCompany = new ERP_BL.Databases.Company();
        SaleOrderStatus oldStatus = new SaleOrderStatus();

        Department InterDepartment = new Department();
        List<Company> currentUserCompanies;
        TaxName saleOrderTax = new TaxName();
        public virtual List< SplitPER> splitPER { get; set; }
        public bool iscopyTemplate=false;
        int interBankTransferId = 0;
        InterBankTransRepo interBankTransferRepo = new InterBankTransRepo();

        OfferRepo offerRepo = new OfferRepo();
        ProductRepo productRepo = new ProductRepo();
        TaxRepo taxRepo = new TaxRepo();

        SaleOrdeRrefKey key = new SaleOrdeRrefKey();
        List<SaleOrderStatus> SaleOrderStatuses = new List<SaleOrderStatus>();
        List<ERP_BL.Procurements.StatusClass.StatusClass> StatusClasses = new List<ERP_BL.Procurements.StatusClass.StatusClass>();
        bool isGeneratedBySOLink = false;
        public ERP_BL.Procurements.StatusClass.StatusClass checkStatusClass = new ERP_BL.Procurements.StatusClass.StatusClass();

        public ucSaleOrderAdd()
        {
            procurementProducts = new List<ProcurementProduct>();
            splitPER = new List<SplitPER>();
        
            products = new List<Product>();
            InitializeComponent();
            symbol = "";
        }
        public ucSaleOrderAdd(bool isParent,int referenceKey, bool _isGeneratedBySOLink=false)
        {
            saleOrderRepo = new SaleOrderRepo();
            procurementProducts = new List<ProcurementProduct>();
            splitPER = new List<SplitPER>();
            isGeneratedBySOLink = _isGeneratedBySOLink;
            products = new List<Product>();
            key=saleOrderRepo.GetSORef(referenceKey);
            parentSaleOrderid = Convert.ToInt32(key.key);
            InitializeComponent();
            symbol = "";
        }
        public ucSaleOrderAdd(bool isCopy)
        {
            procurementProducts = new List<ProcurementProduct>();
            splitPER = new List<SplitPER>();
            products = new List<Product>();
            InitializeComponent();
            symbol = "";
            this.iscopyTemplate = isCopy;
        }
        public ucSaleOrderAdd(int ibt_Id)
        {
            procurementProducts = new List<ProcurementProduct>();
            splitPER = new List<SplitPER>();
            products = new List<Product>();
            InitializeComponent();
            symbol = "";
            interBankTransferId = ibt_Id;
        }
        public void loadBookerItemsSources()
        {
            BookerStatementItems = new List<BookerStatementItem>();
            grdBokkerItems.ItemsSource = BookerStatementItems;
            lookupBookerFOCSampling.ItemsSource = offerRepo.GetAllActiveFOCSamplings();
            lookupBookerClaimDiscount.ItemsSource = offerRepo.GetAllActiveClaimDiscounts();
            //lookupBookerProductsinGrid.ItemsSource = productRepo.getAllUserProducts(SYSTEM_STATIC.currentUser.id);
            lookupBookerGST.ItemsSource = taxRepo.getAllTaxes();
            lookupPassOn.ItemsSource = offerRepo.GetAllPassOns();
        }
        private void winSaleOrderadd_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                OrderId = saleOrderid;
                editOrder = editsaleOrder;
                txttax.Text = "0";
                grdSOItems.ItemsSource = procurementProducts;
                grdSplitPER.ItemsSource = splitPER;
                loadBookerItemsSources();
                loadcompanies();
                loadinquirytypes();
                loadWarrantys();
                loadPaymentTerms();
                loadIncoterms();
                loadCurrencies();
                loadSaleOrderStatus();
                loadVendorPaymentStatus();
                LoadTaxes();
                CheckPermissions();
                LoadSaleOrder();
                LoadPendingInvoices();
                grdTrackingTree.ExpandAllNodes();
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSOItems);
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                MessageBox.Show(ex.ToString(), "Data Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
                var myWindow = Window.GetWindow(this);
                myWindow.Close();

            }
            grdSOItems.Columns["value1"].AllowMoving = DevExpress.Utils.DefaultBoolean.False;
            grdSOItems.Columns["value2"].AllowMoving = DevExpress.Utils.DefaultBoolean.False;
            grdTrackingTree.ExpandAllNodes();
        }
        public void LoadSaleOrder()
        {
            OfferRepo offerRepo = new OfferRepo();
            ModuleContractRepo moduleContractRepo = new ModuleContractRepo();
            InterBankTransRepo bankTransRepo = new InterBankTransRepo();
            if (editsaleOrder == 1 && saleOrderid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Order") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order") == null)
                {
                    isloading = true;

                    saleOrder = saleOrderRepo.get(saleOrderid);
                    soStatus = saleOrder.saleOrderStatus;
                    loadonSaleOrderdata();
                    GellAllOrdersTracking();

                    btnSave.IsEnabled = false;

                    if (saleOrder.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Sale Order") != null)
                    {
                        btnSave.IsEnabled = true;
                    }
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Order") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order") != null)
                {

                    isloading = true;
                    saleOrder = saleOrderRepo.get(saleOrderid);
                    loadonSaleOrderdata();
                    GellAllOrdersTracking();

                    btnSave.IsEnabled = true;
                }

                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Order") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order") != null)
                {
                    isloading = true;

                    saleOrder = saleOrderRepo.get(saleOrderid);
                    loadonSaleOrderdata();
                    GellAllOrdersTracking();

                    //btnSave.IsEnabled = true;
                    //Setting void stamp
                    if (saleOrder != null)
                    {
                        if (saleOrder.isVoid == true)
                        {
                            grdVoid.Visibility = Visibility.Visible;
                        }
                    }
                }

                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Sale Order!");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }

            }
            else if (editsaleOrder == 0 && offerid != 0)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order") != null)
                {
                    isloading = true;
                    offer = offerRepo.get(offerid);
                    loadonOfferdata();
                    datpoCreationdate.EditValue = System.DateTime.Now;
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Add Sale Order!");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }


            }
            else if (editsaleOrder == 0 && moduleContractid != 0)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order") != null)
                {
                    isloading = true;
                    ModuleContract = moduleContractRepo.get(moduleContractid);
                    loadonModuleContractdata();
                    datpoCreationdate.EditValue = System.DateTime.Now;
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Add Sale Order!");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }


            }
            else 
            if (editsaleOrder == 0 && offerid == 0 && interBankTransferId != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order") != null)
                {
                    isloading = true;
                    interBankTransfer = bankTransRepo.GetInterBankTransfer(interBankTransferId);
                    loadonIBTdata();
                    datpoCreationdate.EditValue = System.DateTime.Now;
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Add Sale Order!");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }
            }
            else
            if (editsaleOrder == 0 && moduleContractid == 0 && interBankTransferId != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order") != null)
                {
                    isloading = true;
                    interBankTransfer = bankTransRepo.GetInterBankTransfer(interBankTransferId);
                    loadonIBTdata();
                    datpoCreationdate.EditValue = System.DateTime.Now;
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Add Sale Order!");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }
            }
            else
            {
                datpoCreationdate.EditValue = System.DateTime.Now;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
                cmbcaption1.SelectedIndex = 0;
                cmbcaption2.SelectedIndex = 1;
                if(parentSaleOrderid!=0)
                {
                    loadParentSaleOrder();
                }




            }
        }
        public  void loadParentSaleOrder()
        {
            isloading = true;
            saleOrder = new SaleOrder();
            parentSaleOrder = saleOrderRepo.get(parentSaleOrderid);
            cmbSaleOrderType.SelectedItem = parentSaleOrder.saleOrdertype.ToString();
            datpoCreationdate.EditValue = System.DateTime.Now;
            datsaleOrderdate.EditValue = parentSaleOrder.saleOrderDate;
            datDeliverydate.EditValue = parentSaleOrder.deliveryDate;
            txtSalesref.Text = parentSaleOrder.SalesReferenceNo;
            txtOfferRefNo.Text = parentSaleOrder.offerReferenceNo;
            txtFinanaceRef.Text = parentSaleOrder.FinanceRefrenceNo;
            txtsaleOrderref.Text = parentSaleOrder.referenceNo;


            //LoadingPOChangeData
            datsaleOrderdate.EditValue = parentSaleOrder.saleOrderDate;
            datDeliverydate.EditValue = parentSaleOrder.deliveryDate;
            txtSalesref.Text = parentSaleOrder.SalesReferenceNo;
            txtOfferRefNo.Text = parentSaleOrder.offerReferenceNo;
            txtFinanaceRef.Text = parentSaleOrder.FinanceRefrenceNo;
            txtsaleOrderref.Text = parentSaleOrder.referenceNo;
            txtOrigin.Text = parentSaleOrder.origin;
            txtMaker.Text = parentSaleOrder.maker;
            txtPacking.Text = parentSaleOrder.packing;


            txtOwnDescription.Text = parentSaleOrder.OwnDescription;



            //Warranty
            if (parentSaleOrder.WarrantyId != 0 && parentSaleOrder.Warranty != null)
            {
                var incoSource = (List<cmbitem>)cmbWarranty.Items.SourceCollection;

                cmbWarranty.SelectedItem = cmbWarranty.Items[cmbWarranty.Items.IndexOf(incoSource.Find(x => x.id == parentSaleOrder.WarrantyId))];
            }


            //select Captions for Item Value 1
            if (parentSaleOrder.TitleValue1Id != 0 || parentSaleOrder.TitleValue1 != null)
            {
                var captionSource = (List<cmbitem>)cmbcaption1.Items.SourceCollection;
                cmbcaption1.SelectedItem = cmbcaption1.Items[cmbcaption1.Items.IndexOf(captionSource.Find(x => x.id == parentSaleOrder.TitleValue1Id))];

            }
            //select Captions for Item Value 2
            if (parentSaleOrder.TitleValue2Id != 0 || parentSaleOrder.TitleValue2 != null)
            {
                var caption2Source = (List<cmbitem>)cmbcaption2.Items.SourceCollection;
                cmbcaption2.SelectedItem = cmbcaption2.Items[cmbcaption2.Items.IndexOf(caption2Source.Find(x => x.id == parentSaleOrder.TitleValue2Id))];


            }
            //Payment term Load for sale Order
            if (parentSaleOrder.paymentterm_Id != 0 && parentSaleOrder.paymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbPaymentTerm.Items.SourceCollection;



                var term = paymentTermSource.Find(x => x.id == parentSaleOrder.paymentterm_Id);

                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = parentSaleOrder.paymentTerm.term, id = parentSaleOrder.paymentTerm.Id });
                    cmbPaymentTerm.ItemsSource = null;
                    cmbPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbPaymentTerm.SelectedItem = cmbPaymentTerm.Items[cmbPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == parentSaleOrder.paymentTerm.Id))];
            }
            //Inco Term Load for sale Order
            if (parentSaleOrder.incoterm_Id != 0 && parentSaleOrder.incoterm != null)
            {
                var incotermSourceChange = (List<cmbitem>)cmbIncoterm.Items.SourceCollection;

                cmbIncoterm.SelectedItem = cmbIncoterm.Items[cmbIncoterm.Items.IndexOf(incotermSourceChange.Find(x => x.id == parentSaleOrder.incoterm_Id))];

            }

            if (parentSaleOrder.currency_Id != 0 && parentSaleOrder.currency != null)
            {
                var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == parentSaleOrder.currency_Id))];
            }


            // Select Company
            if (parentSaleOrder.company_Id != null || parentSaleOrder.company != null)
            {
                company = parentSaleOrder.company;
                lookupCompany.Text = parentSaleOrder.company.CompanyName;
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }
            // Select Department
            if (parentSaleOrder.dept_Id != 0 || parentSaleOrder.department != null)
            {
                lookupDepartment.Text = parentSaleOrder.department.DeptName;
                department = parentSaleOrder.department;
                //lookupCustomer.ItemsSource = department.customers;
                loademployees();

            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }
            //Select Customer
            if (parentSaleOrder.customerCompany.Id != 0 || parentSaleOrder.customerCompany != null)
            {
                lookupCustomer.Text = parentSaleOrder.customerCompany.company.CompanyName;
                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(parentSaleOrder.customerCompany);
                customer = parentSaleOrder.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            //Select vendor
            if (saleOrder.vendors != null)
            {

                foreach (Vendor vendr in parentSaleOrder.vendors)
                {
                    lookupVendor.Text = vendr.company.CompanyName;
                    vendor = vendr;
                }
            }

            if (parentSaleOrder.allocation_Id != 0 || parentSaleOrder.employee != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == parentSaleOrder.allocation_Id))];

                }
                catch (Exception ex)
                {

                }

            }

            // load Products in Inquiry to grid
            List<DataGridItem> datagriditems = new List<DataGridItem>();

            // selected currency of customer company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {

                if (cmbitem.id == parentSaleOrder.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }


            //calculatetotal();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Commision Field without Approval") == null)
            {
                txtCommision.IsReadOnly = true;

            }
            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Edit FOB and CFR value for products") == null)
            {
                grdSOItems.Columns.GetColumnByFieldName("value1").ReadOnly = false;
                grdSOItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;

            }

            if (parentSaleOrder.products != null)
            {
                List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                //grdPOItems.ItemsSource = offer.products;
                if (parentSaleOrder.products != null)
                {
                    foreach (var procurementProduct in parentSaleOrder.products)
                    {
                        procurementProducts.Add(new ProcurementProduct()
                        {
                            //Id = procurementProduct.Id,

                            inquiryProduct = new InquiryProduct()
                            {
                                //Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product = new Product()
                                {
                                    Id = procurementProduct.inquiryProduct.product.Id,
                                    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    item = procurementProduct.inquiryProduct.product.item,
                                    code = procurementProduct.inquiryProduct.product.code,
                                    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    nature = procurementProduct.inquiryProduct.product.nature,
                                    category = procurementProduct.inquiryProduct.product.category,
                                    isActive = procurementProduct.inquiryProduct.product.isActive

                                }
                                    ,
                                product_Id = procurementProduct.inquiryProduct.product.Id

                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,
                            UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity,
                            InvoicedQuantity = (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,
                            UnInvoicedWeight = (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,
                            InvoicedWeight = (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,
                            value1 = procurementProduct.value1,
                            value2 = procurementProduct.value2,
                            caption1 = cmbcaption1.Text.Trim(),
                            caption2 = cmbcaption2.Text.Trim(),
                            UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount
                            //UnInvoicedQuantity= procurementProduct.UnInvoicedQuantity,


                        });



                    }
                    //dGitems.ItemsSource = datagriditems;
                    grdSOItems.ItemsSource = procurementProducts;
                    int x = 0;
                    foreach (var pro in grdSOItems.ItemsSource as List<ProcurementProduct>)
                    {
                        grdSOItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                        x++;

                    }
                    lookupProductsinGrid.DisplayMember = "code";

                }
                else
                {
                    grdSOItems.ItemsSource = procurementProducts;
                }
            }


            if (parentSaleOrder.deliveryDateFinal != null)
            {
                datDeliverydate.EditValue = parentSaleOrder.deliveryDateFinal;
            }
        }
        public void CheckPermissions()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Bill Reference Number in Sale Order") != null)
            {
                cmbxBillRef.IsEnabled = true;
            }
            else
            {
                cmbxBillRef.IsEnabled = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of SaleOrder") != null)
            {
                datpoCreationdate.IsEnabled = true;
            }
            else
            {
                datpoCreationdate.IsEnabled = false;

            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with SaleOrder") != null)
            {
                //btnAttachNew.Visibility = Visibility.Visible;
            }
            else
            {
                btnAttachNew.Visibility = Visibility.Collapsed;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Audit Year") != null)
            {
                datAuditYealy.IsEnabled = true;
            }
            else
            {
                datAuditYealy.IsEnabled = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with SaleOrder") != null)
            {
                btnAttachmentList.Visibility = Visibility.Visible;
            }
            else
            {
                btnAttachmentList.Visibility = Visibility.Collapsed;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Marketing Cost in Summary Sheet") != null)
            {
                txtNetBaseCommission.Visibility = Visibility.Visible;
                txtNetCommision.Visibility = Visibility.Visible;
                lblNetCommision.Visibility = Visibility.Visible;
                txtNetSalesCommision.Visibility = Visibility.Visible;
            }
            else
            {
                txtNetBaseCommission.Visibility = Visibility.Collapsed;
                txtNetCommision.Visibility = Visibility.Collapsed;
                lblNetCommision.Visibility = Visibility.Collapsed;
                txtNetSalesCommision.Visibility = Visibility.Collapsed;

            }

            
            //calculatetotal();
            isloading = false;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Market Exchange Rate in Sale Order") != null)
            {
                lblexchangerate.Visibility = Visibility.Visible;
                txtexchangerate.Visibility = Visibility.Visible;
            }
            else
            {
                lblexchangerate.Visibility = Visibility.Collapsed;
                txtexchangerate.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Exchange Rate in Sale Order") != null)
            {
                lblmarginexchangerate.Visibility = Visibility.Visible;
                txtMarginexchangerate.Visibility = Visibility.Visible;
            }
            else
            {
                lblmarginexchangerate.Visibility = Visibility.Collapsed;
                txtMarginexchangerate.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit SO SER") != null)
            {
             
                txtMarginexchangerate.IsEnabled = true;
            }
            else
            {
                txtMarginexchangerate.IsEnabled = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit SO MER") != null)
            {

                txtexchangerate.IsEnabled = true;
            }
            else
            {
                txtexchangerate.IsEnabled = false;
            }


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void SaleOrder") != null)
            {
                btnSetVoid.Visibility = Visibility.Visible;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Move SaleOrder to Inter Company") != null)
            {
                gridInterCompanyDetails.IsEnabled = true;
                chkInterCompany.IsEnabled = true;
            }
            else
            {
                gridInterCompanyDetails.IsEnabled = false;
                chkInterCompany.IsEnabled = false;
            }
            
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order delivery date Final") != null)
            {
                datDeliverydateFinal.IsEnabled = true;
                datDeliverydateFinal.IsReadOnly = false;
            }
            else
            {
                datDeliverydateFinal.IsEnabled = false;
                datDeliverydateFinal.IsReadOnly = true;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order delivery date") != null )
            {
                datDeliverydate.IsEnabled = true;
                datDeliverydate.IsReadOnly = false;
            }
            else
            {
                if(editOrder==0)
                {
                    datDeliverydate.IsEnabled = true;
                    datDeliverydate.IsReadOnly = false;
                }
                else
                {
                    datDeliverydate.IsEnabled = false;
                    datDeliverydate.IsReadOnly = true;
                }
               
            }
            //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Pending invoices") != null)
            //{
            //    datDeliverydate.IsEnabled = true;
            //    datDeliverydate.IsReadOnly = false;
            //}
            //else
            //{
            //    datDeliverydate.IsEnabled = false;
            //    datDeliverydate.IsReadOnly = true;
            //}
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Performance Sheet") != null)
            {
                btnPerformanceSheet.Visibility = Visibility.Visible;
            }
            else
            {
                btnPerformanceSheet.Visibility = Visibility.Collapsed;

            }

        }
        public void LoadTaxes()
        {
            TaxRepo taxRepo = new TaxRepo();
            lookupSalesTax.ItemsSource= taxRepo.getAllTaxes();
        }

        public void loadcompanies()
        {
            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();
                this.lookupCompany.ItemsSource = cont.GetCompanies();
                return;
            }
            currentUserCompanies = SYSTEM_STATIC.currentUser.employee.Companies;
            lookupCompany.ItemsSource = currentUserCompanies;
            lookupInterCompany.ItemsSource = currentUserCompanies;
        }

        public void loadInterCompanyDepartments()
        {
            ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();

            if (MainWindow.currentUserid == 0)
            {
                DepartmentRepo departmentRepo = new DepartmentRepo();
                this.lookupInterDepartment.ItemsSource = departmentRepo.GetDepartments();
                return;
            }
            if (InterCompany != null)
                if (InterCompany.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    //departments = SYSTEM_STATIC.currentUser.employee.departments /*companyRepo.GetUserDepartments(SYSTEM_STATIC.currentUser.id)*/;
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsProcurementType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == InterCompany.Id) != null)
                            departments.Add(_dept);
                    }
                    if (saleOrder != null && saleOrder.Id > 0 && editsaleOrder == 1)
                        if (saleOrder.InterDepartment != null && departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                            departments.Add(saleOrder.InterDepartment);

                    lookupInterDepartment.ItemsSource = departments;
                    //if (lookupInterDepartment.ItemsSource == null)
                    //    lookupInterDepartment.ItemsSource = departments;

                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }
        private void lookupInterCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            InterCompany = lookupInterCompany.SelectedItem as Company;
            if (InterCompany != null)
            {
                loadInterCompanyDepartments();
            }
        }
        public void loadvendors()
        {


            // List<Vendor> vendors = new List<Vendor>();
            // vendors = vendorRepo.getAll();
            //// List<CustomerCompany> customerlist = new List<CustomerCompany>();

            // //foreach (Vendor vendor in vendors)
            // //{
            // //    customerlist.Add(vendor);
            // //}

            // lookupVendor.ItemsSource = vendors;
            lookupVendor.ItemsSource = department.Vendors;
        }
        public void loadPrincipals()
        {


            //List<Principal> principals = new List<Principal>();
            //principals = principalRepo.getAll();
            //// List<CustomerCompany> customerlist = new List<CustomerCompany>();

            ////foreach (Vendor vendor in vendors)
            ////{
            ////    customerlist.Add(vendor);
            ////}
             
                lookupPrincipal.ItemsSource = department.Principals;
            
        }

        public void loadcustomers()
        {
            if (company != null && department != null)
                if (company.Id != 0)
                {
                    if (department.Id != 0)
                    {
                        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
                        List<CustomerCompany> customers = /*customerCompRepo.getCustomersForCompanyAndDepartment(company.Id, department.Id)*/ department.customers.ToList();

                        if (editOrder == 1)
                        {
                            if (saleOrder.Id != 0)
                            {
                                var customer = department.disableCustomers.FirstOrDefault(x => x.Id == saleOrder.customerCompany_Id);
                                if (customer != null)
                                {
                                    customers.AddRange(department.disableCustomers);
                                }
                            }
                        }
                        if (customers == null || customers.Count == 0)
                        {
                            //DevExpress.Xpf.Core.DXMessageBox.Show("Please select a different Department and Company! No customer is mapped to this department or Company.", "Select another Department or Company", MessageBoxButton.OK, MessageBoxImage.Information);
                            //return;
                        }
                        else
                        {
                            lookupCustomer.ItemsSource = customers;
                            return;
                        }
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Please select a Department First!", "Select Department to Load Customer Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please select a Company First!", "Select Company to Load Customer Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;

                }

        }

        public void loadinquirytypes()
        {
            Config config = new Config();
            List<string> InquiryType = config.getInquiryType();
            if (InquiryType.Count > 0)
                foreach (string IND in InquiryType)
                {
                     
                        cmbSaleOrderType.Items.Add(IND);

                    
                }

        }
        public void loademployees()
        {
            ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            //  employees = cont1.GetEmployees();
            employees = department.employees;

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (ERP_BL.Databases.Employee employee in employees)
            {
                cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbEmployee.ItemsSource = cmbitems;
            cmbTransactionHolder.ItemsSource = cmbitems;
        }

        public void loaddepartments()
        {
            if (MainWindow.currentUserid == 0)
            {
                DepartmentRepo departmentRepo = new DepartmentRepo();
                this.lookupDepartment.ItemsSource = departmentRepo.GetDepartments();
                return;
            }
            if (company != null)
                if (company.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    //departments = SYSTEM_STATIC.currentUser.employee.departments /*companyRepo.GetUserDepartments(SYSTEM_STATIC.currentUser.id)*/;
                    foreach(var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsSaleOrderType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (saleOrder != null && saleOrder.Id > 0 && editsaleOrder == 1)
                        if (saleOrder.department != null && departments.FirstOrDefault(x=>x.Id == saleOrder.dept_Id) == null)
                            departments.Add(saleOrder.department);

                    lookupDepartment.ItemsSource = departments;
                    //if (lookupInterDepartment.ItemsSource == null)
                    //    lookupInterDepartment.ItemsSource = departments;

                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }

        public void loadonOfferdata()
        {
            if (offer.offertype == InquiryType.Supply)
            {
                //frmCostSheet frmCostSheet = new frmCostSheet(offer);
                //frmCostSheet.ShowDialog();
                //if (frmCostSheet.costSheet != null)
                //{
                //    saleOrder.CostSheet = frmCostSheet.costSheet;
                //    saleOrder.CostSheet.Timestamp = DateTime.Now;
                //    txtBudgetMargin.Text = (Convert.ToDecimal(offer.totalCFRValue) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                //    txtActualMargin.Text = (Convert.ToDecimal(offer.totalCFRValue) != (Convert.ToDecimal(offer.totalCFRValue) - saleOrder.CostSheet.TotalActualMargin)) ? (Convert.ToDecimal(offer.totalCFRValue) - saleOrder.CostSheet.TotalActualMargin).ToString() : "0";

                //}
            }
            grdInvoiceProgressBar.Visibility = Visibility.Collapsed;
            datsaleOrderdate.DateTime = System.DateTime.Now;
            datpoCreationdate.EditValue = System.DateTime.Now;
            txtCommissionNumber.Text = offer.commisionRefrenceNo;
            txtSalesref.Text = offer.SalesReferenceNo;
            txtOfferRefNo.Text = offer.offerReferenceNo;
            txtsaleOrderref.Text = offer.InquiryReferenceNo;
            txtCommision.Text = offer.commision.ToString();
            txtOwnDescription.Text = offer.OwnDescription;
            txtexchangerate.Text = offer.exchngeRate.ToString();
            txttotalfob.Text = offer.totalFOBValue.ToString();
            txttotalcfr.Text = offer.totalCFRValue.ToString();
            txtBasetotalfob.Text = offer.totalBaseFOBValue.ToString();
            txtBasetotalcfr.Text = offer.totalBaseCFRValue.ToString();
            txtMaker.Text = offer.maker;
            txtOrigin.Text = offer.origin;
            if (offer.isPercentTax == true)
                txttax.Text = offer.salesTax.ToString() + "%";
            else
                txttax.Text = offer.salesTax.ToString();
            // Select IncoTerm
            if (offer.incoterm_Id != 0 || offer.incoterm != null)
            {
                foreach (cmbitem cmbitem in cmbIncoterm.Items)
                {
                    if (cmbitem.id == offer.incoterm_Id)
                    {

                        cmbIncoterm.SelectedItem = cmbitem;
                        break;
                    }
                }
            }
            //select Captions for Item Value 1
            if (offer.TitleValue1Id != 0 || offer.TitleValue1 != null)
            {
                foreach (cmbitem cmbitem in cmbcaption1.Items)
                {
                    if (cmbitem.id == offer.TitleValue1Id)
                    {
                        cmbcaption1.SelectedItem = cmbitem;
                    }
                }
            }
            //select Captions for Item Value 2
            if (offer.TitleValue2Id != 0 || offer.TitleValue2 != null)
            {
                foreach (cmbitem cmbitem in cmbcaption2.Items)
                {
                    if (cmbitem.id == offer.TitleValue2Id)
                    {
                        cmbcaption2.SelectedItem = cmbitem;
                    }
                }
            }
            // Select Paymentterm 
            if (offer.paymentterm_Id != 0 || offer.paymentTerm != null)
            {
                foreach (cmbitem cmbitem in cmbPaymentTerm.Items)
                {
                    if (cmbitem.id == offer.paymentterm_Id)
                    {

                        cmbPaymentTerm.SelectedItem = cmbitem;
                        break;
                    }
                }


            }
            if (offer.currency_Id != 0 && offer.currency != null)
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == offer.currency_Id)
                    {
                        cmbCurrency.SelectedItem = cmbitem;
                        break;
                    }
                }
            else
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == offer.company.CurrencyId)
                    {
                        cmbCurrency.SelectedItem = cmbitem;
                        break;
                    }

                }


            // Select Company
            if (offer.company_Id != null || offer.company != null)
            {
                company = offer.company;

                lookupCompany.Text = offer.company.CompanyName;
                //lookupCompany.SelectedItem = lookupCompany.GetItemByKeyValue(offer.company);

                //loaddepartments();
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }
            // Select Department
            if (offer.dept_Id != 0 || offer.department != null)
            {
                lookupDepartment.Text = offer.department.DeptName;
                department = offer.department;
                //lookupCustomer.ItemsSource = department.customers;
                loademployees();

            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }
            //Select Customer
            if (offer.customerCompany.Id != 0 || offer.customerCompany != null)
            {
                lookupCustomer.Text = offer.customerCompany.company.CompanyName;
                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(offer.customerCompany);

                customer = offer.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            //Select vendor
            if (offer.vendors != null)
                foreach (Vendor vendr in offer.vendors)


                {
                    lookupVendor.Text = vendr.company.CompanyName;
                    vendor = vendr;
                }
            if (offer.principal_Id != null)
                if (offer.principal_Id != 0 || offer.principal != null)
                {
                    lookupPrincipal.Text = offer.principal.company.CompanyName;
                    principal = offer.principal;
                }
                else
                {
                    lookupCustomer.Text = "Select Customer";

                }
            // Select Employee 
            if (offer.allocation_Id != 0 || offer.employee != null)
            {
                foreach (cmbitem cmbitem in cmbEmployee.Items)
                {
                    if (cmbitem.id == offer.allocation_Id)
                    {

                        cmbEmployee.SelectedItem = cmbitem;
                        break;
                    }
                }


            }
            if (offer.bid_Id != null && offer.bid_Id != 0 && offer.bid != null)
            { //load form data for bid
                grpbondinfo.State = GroupBoxState.Normal;
                datBidBondIssuedate.DateTime = offer.bid.issueDate;

                txtBidBondRefno.Text = offer.bid.refNo;
                txtBidBonValue.Text = offer.bid.value;
                datBidBondSubmitdate.DateTime = offer.bid.submitDate;
                datBidBondExpirydate.DateTime = offer.bid.expireDate;
                txtIssuingbank.Text = offer.bid.bankName;
            }
            // Delete Inquiry Type Same for Offer Type
            {
                cmbSaleOrderType.Text = offer.offertype.ToString();
                PoType = offer.offertype;
            }
            // Select Offer Status 
            foreach (cmbitem cmbitem in cmbSaleOrderStatus.Items)
            {
                if (cmbitem.name == offer.offerStatus.Status)
                {
                    cmbSaleOrderStatus.SelectedItem = cmbitem;
                    break;
                }
            }
            //int a = 0, b = 0;
            // load Products in Inquiry to grid
            List<DataGridItem> datagriditems = new List<DataGridItem>();


            if (offer.offertype == InquiryType.DistributionBiz || offer.offertype == InquiryType.DistributionBiz_CustomerCredit)
            {
                grdBokkerItems.ItemsSource = offer.BookerStatementItems;
            }
            else
            {
                if (offer.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                    foreach (var procurementProduct in offer.products)
                    {
                        procurementProducts.Add(new ProcurementProduct()
                        {
                            Id = procurementProduct.Id,

                            inquiryProduct = new InquiryProduct()
                            {
                                Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product = new Product()
                                {
                                    Id = procurementProduct.inquiryProduct.product.Id,
                                    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    item = procurementProduct.inquiryProduct.product.item,
                                    code = procurementProduct.inquiryProduct.product.code,
                                    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    nature = procurementProduct.inquiryProduct.product.nature,
                                    category = procurementProduct.inquiryProduct.product.category,
                                    isActive = procurementProduct.inquiryProduct.product.isActive
                                }
                                    ,
                                product_Id = procurementProduct.inquiryProduct.product.Id

                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,
                            value1 = procurementProduct.value1,
                            value2 = procurementProduct.value2,
                            caption1 = cmbcaption1.Text.Trim(),
                            caption2 = cmbcaption2.Text.Trim(),
                            priority = procurementProduct.priority


                        });
                    }
                    grdSOItems.ItemsSource = procurementProducts;
                }
                else
                {
                    grdSOItems.ItemsSource = procurementProducts;
                }
            }

            // selected currency of company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                if (cmbitem.id == offer.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            if(offer.CostSheet_Id!=null)
            {
                saleOrder.CostSheet_Id = offer.CostSheet_Id;
            }
            calculatetotal();
        }

        public void loadonModuleContractdata()
        {
            if (ModuleContract.ModuleContracttype == InquiryType.Supply)
            {
                frmCostSheet frmCostSheet = new frmCostSheet(ModuleContract);
                frmCostSheet.ShowDialog();
                if (frmCostSheet.costSheet != null)
                {
                    saleOrder.CostSheet = frmCostSheet.costSheet;
                    txtBudgetMargin.Text = (Convert.ToDecimal(ModuleContract.totalCFRValue) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                    txtActualMargin.Text = (Convert.ToDecimal(ModuleContract.totalCFRValue) != (Convert.ToDecimal(ModuleContract.totalCFRValue) - saleOrder.CostSheet.TotalActualMargin)) ? (Convert.ToDecimal(ModuleContract.totalCFRValue) - saleOrder.CostSheet.TotalActualMargin).ToString() : "0";

                }
            }
            grdInvoiceProgressBar.Visibility = Visibility.Collapsed;
            datsaleOrderdate.DateTime = System.DateTime.Now;
            datpoCreationdate.EditValue = System.DateTime.Now;
            txtCommissionNumber.Text = ModuleContract.commisionRefrenceNo;
            txtSalesref.Text = ModuleContract.SalesReferenceNo;
            txtOfferRefNo.Text = ModuleContract.ModuleContractReferenceNo;
            txtsaleOrderref.Text = ModuleContract.OfferReferenceNo;
            txtCommision.Text = ModuleContract.commision.ToString();
            txtOwnDescription.Text = ModuleContract.OwnDescription;
            txtexchangerate.Text = ModuleContract.exchngeRate.ToString();
            txttotalfob.Text = ModuleContract.totalFOBValue.ToString();
            txttotalcfr.Text = ModuleContract.totalCFRValue.ToString();
            txtBasetotalfob.Text = ModuleContract.totalBaseFOBValue.ToString();
            txtBasetotalcfr.Text = ModuleContract.totalBaseCFRValue.ToString();
            txtMaker.Text = ModuleContract.maker;
            txtOrigin.Text = ModuleContract.origin;
            if (ModuleContract.isPercentTax == true)
                txttax.Text = ModuleContract.salesTax.ToString() + "%";
            else
                txttax.Text = ModuleContract.salesTax.ToString();
            // Select IncoTerm
            if (ModuleContract.incoterm_Id != 0 || ModuleContract.incoterm != null)
            {
                foreach (cmbitem cmbitem in cmbIncoterm.Items)
                {
                    if (cmbitem.id == ModuleContract.incoterm_Id)
                    {

                        cmbIncoterm.SelectedItem = cmbitem;
                        break;
                    }
                }
            }
            //select Captions for Item Value 1
            if (ModuleContract.TitleValue1Id != 0 || ModuleContract.TitleValue1 != null)
            {
                foreach (cmbitem cmbitem in cmbcaption1.Items)
                {
                    if (cmbitem.id == ModuleContract.TitleValue1Id)
                    {
                        cmbcaption1.SelectedItem = cmbitem;
                    }
                }
            }
            //select Captions for Item Value 2
            if (ModuleContract.TitleValue2Id != 0 || ModuleContract.TitleValue2 != null)
            {
                foreach (cmbitem cmbitem in cmbcaption2.Items)
                {
                    if (cmbitem.id == ModuleContract.TitleValue2Id)
                    {
                        cmbcaption2.SelectedItem = cmbitem;
                    }
                }
            }
            // Select Paymentterm 
            if (ModuleContract.paymentterm_Id != 0 || ModuleContract.paymentTerm != null)
            {
                foreach (cmbitem cmbitem in cmbPaymentTerm.Items)
                {
                    if (cmbitem.id == ModuleContract.paymentterm_Id)
                    {

                        cmbPaymentTerm.SelectedItem = cmbitem;
                        break;
                    }
                }


            }
            if (ModuleContract.currency_Id != 0 && ModuleContract.currency != null)
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == ModuleContract.currency_Id)
                    {
                        cmbCurrency.SelectedItem = cmbitem;
                        break;
                    }
                }
            else
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == ModuleContract.company.CurrencyId)
                    {
                        cmbCurrency.SelectedItem = cmbitem;
                        break;
                    }

                }


            // Select Company
            if (ModuleContract.company_Id != null || ModuleContract.company != null)
            {
                company = ModuleContract.company;

                lookupCompany.Text = ModuleContract.company.CompanyName;
                //lookupCompany.SelectedItem = lookupCompany.GetItemByKeyValue(ModuleContract.company);

                //loaddepartments();
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }
            // Select Department
            if (ModuleContract.dept_Id != 0 || ModuleContract.department != null)
            {
                lookupDepartment.Text = ModuleContract.department.DeptName;
                department = ModuleContract.department;
                //lookupCustomer.ItemsSource = department.customers;
                loademployees();

            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }
            //Select Customer
            if (ModuleContract.customerCompany.Id != 0 || ModuleContract.customerCompany != null)
            {
                lookupCustomer.Text = ModuleContract.customerCompany.company.CompanyName;
                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(ModuleContract.customerCompany);

                customer = ModuleContract.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            //Select vendor
            if (ModuleContract.vendors != null)
                foreach (Vendor vendr in ModuleContract.vendors)


                {
                    lookupVendor.Text = vendr.company.CompanyName;
                    vendor = vendr;
                }
            if (ModuleContract.principal_Id != null)
                if (ModuleContract.principal_Id != 0 || ModuleContract.principal != null)
                {
                    lookupPrincipal.Text = ModuleContract.principal.company.CompanyName;
                    principal = ModuleContract.principal;
                }
                else
                {
                    lookupCustomer.Text = "Select Customer";

                }
            // Select Employee 
            if (ModuleContract.allocation_Id != 0 || ModuleContract.employee != null)
            {
                foreach (cmbitem cmbitem in cmbEmployee.Items)
                {
                    if (cmbitem.id == ModuleContract.allocation_Id)
                    {

                        cmbEmployee.SelectedItem = cmbitem;
                        break;
                    }
                }


            }
            if (ModuleContract.bid_Id != null && ModuleContract.bid_Id != 0 && ModuleContract.bid != null)
            { //load form data for bid
                grpbondinfo.State = GroupBoxState.Normal;
                datBidBondIssuedate.DateTime = ModuleContract.bid.issueDate;

                txtBidBondRefno.Text = ModuleContract.bid.refNo;
                txtBidBonValue.Text = ModuleContract.bid.value;
                datBidBondSubmitdate.DateTime = ModuleContract.bid.submitDate;
                datBidBondExpirydate.DateTime = ModuleContract.bid.expireDate;
                txtIssuingbank.Text = ModuleContract.bid.bankName;
            }
            // Delete Inquiry Type Same for ModuleContract Type
            {
                cmbSaleOrderType.Text = ModuleContract.ModuleContracttype.ToString();
                PoType = ModuleContract.ModuleContracttype;
            }
            // Select ModuleContract Status 
            foreach (cmbitem cmbitem in cmbSaleOrderStatus.Items)
            {
                if (cmbitem.name == ModuleContract.ModuleContractStatus.Status)
                {
                    cmbSaleOrderStatus.SelectedItem = cmbitem;
                    break;
                }
            }
            //int a = 0, b = 0;
            // load Products in Inquiry to grid
            List<DataGridItem> datagriditems = new List<DataGridItem>();


            if (ModuleContract.ModuleContracttype == InquiryType.DistributionBiz || ModuleContract.ModuleContracttype == InquiryType.DistributionBiz_CustomerCredit)
            {
                grdBokkerItems.ItemsSource = ModuleContract.BookerStatementItems;
            }
            else
            {
                if (ModuleContract.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                    foreach (var procurementProduct in ModuleContract.products)
                    {
                        procurementProducts.Add(new ProcurementProduct()
                        {
                            Id = procurementProduct.Id,

                            inquiryProduct = new InquiryProduct()
                            {
                                Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product = new Product()
                                {
                                    Id = procurementProduct.inquiryProduct.product.Id,
                                    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    item = procurementProduct.inquiryProduct.product.item,
                                    code = procurementProduct.inquiryProduct.product.code,
                                    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    nature = procurementProduct.inquiryProduct.product.nature,
                                    category = procurementProduct.inquiryProduct.product.category,
                                    isActive = procurementProduct.inquiryProduct.product.isActive
                                }
                                    ,
                                product_Id = procurementProduct.inquiryProduct.product.Id

                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,
                            value1 = procurementProduct.value1,
                            value2 = procurementProduct.value2,
                            caption1 = cmbcaption1.Text.Trim(),
                            caption2 = cmbcaption2.Text.Trim(),
                            priority = procurementProduct.priority


                        });
                    }
                    grdSOItems.ItemsSource = procurementProducts;
                }
                else
                {
                    grdSOItems.ItemsSource = procurementProducts;
                }
            }

            // selected currency of company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                if (cmbitem.id == ModuleContract.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            calculatetotal();
        }



        public void loadonSaleOrderdata()
        {
            TaskRepo taskRepo = new TaskRepo();
            var task = taskRepo.GetSOTask(saleOrderid);
            if (task != null)
            {
                grdTaskCreated.Visibility = Visibility.Visible;
                if (task.Status.isActive == false && task.PendingForClosing != true)
                {
                    grdTaskCreated.Background = Brushes.Red;
                    txtTaskCreated.Text = "Created Task has been Closed!";
                }
            }
            else
            {
                grdTaskCreated.Visibility = Visibility.Collapsed;
            }


            if (saleOrder.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Vendor Bill Reference Number After Approval in Sale Order") == null)
            {
                cmbxBillRef.IsEnabled = false;
            }

            if (saleOrder.isGeneratedBySOLink == true)
            {
                isGeneratedBySOLink = true;
            }
            else
            {
                isGeneratedBySOLink = false;
            }

            bool isLinked= saleOrderRepo.GetParentOrders(saleOrder.Id);

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Audit Year Adjustment") != null)
            {
                btnAuditAdjustment.Visibility = Visibility.Visible;
            }

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
            if(saleOrder.ParentSO_Id!=null)
            {
                btnFetchRef.Visibility = Visibility.Collapsed;

            }
            else
            {
                btnFetchRef.Visibility = Visibility.Visible;
            }

            if (saleOrder.ParentSO_Id != null)
            {
                var checkSO = saleOrderRepo.GetSaleOrdeRefKey((int)saleOrder.ParentSO_Id);
                if (checkSO != null)
                {
                    lblRefKey.Visibility = Visibility.Visible;
                    txtRefKey.Visibility = Visibility.Visible;

                    txtRefKey.Text = checkSO.key.ToString();
                }
            }
            else
            {
                var checkSO = saleOrderRepo.GetSaleOrdeRefKey(saleOrder.Id);
                if (checkSO != null)
                {
                    lblRefKey.Visibility = Visibility.Visible;
                    txtRefKey.Visibility = Visibility.Visible;
                    txtRefKey.Text = checkSO.key;
                }
            }


            if (saleOrder.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (saleOrder.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (saleOrder.isApproved == true && saleOrder.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (saleOrder.isApproved == true && saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (saleOrder.isApproved == true && saleOrder.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (saleOrder.isApproved == true)
            {
                //lblStage.Text = "Approved";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (saleOrder.isApproved == false)
            {
                //lblStage.Text = "Under Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (saleOrder.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            if(saleOrder.interBankTransfer_Id!=null)
            {
                interBankTransferId = (int)saleOrder.interBankTransfer_Id;
            }
            if (saleOrder.SplitPERs != null)
            {
                splitPER = new List<SplitPER>();
                foreach (var _splitPER in saleOrder.SplitPERs)
                {
                    splitPER.Add(new SplitPER()
                    {
                        Year = _splitPER.Year,
                        Month = _splitPER.Month,
                        Amount = _splitPER.Amount
                    });
                }
                saleOrder.SplitPERs = splitPER;
                grdSplitPER.ItemsSource = splitPER;
            }

            SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
            var SInvoices = saleOrder.SaleInvoices.Where(x => x.isVoid != true).ToList();
            var paidAmount = SInvoices.Sum(x => x.totalInvoiceAmount);


            var percentPaid = Math.Round((paidAmount / Convert.ToDouble(saleOrder.totalCFRValue)) * 100, 2);

            if(!Double.IsNaN(percentPaid))
                pbarTarget.Value = percentPaid;
            cmbSaleOrderType.Text = saleOrder.saleOrdertype.ToString();
            PoType = saleOrder.saleOrdertype;
            lblLastStatusChangeDate.EditValue = saleOrder.LastStatusChangeDate;
            datsaleOrderdate.EditValue = saleOrder.saleOrderDate;
            datpoCreationdate.EditValue = saleOrder.CreationDate;
            datShipmentdate.EditValue = saleOrder.shipmentDate;
            datRevisedShipmentDate.EditValue = saleOrder.revisedShipmentDate;
            datExcpectedPaymentDate.EditValue = saleOrder.ExpectedPayment;
            datPaymentDueTill.EditValue = saleOrder.PaymentDueAgeing;
            datDeliverydate.EditValue = saleOrder.deliveryDate;
            datOrderConfirmationdate.EditValue = saleOrder.orderConfirmationDate;
            datBillOfLaddingdate.EditValue = saleOrder.billLaddingDate;
            datLCDatedate.EditValue = saleOrder.lCDate;
            datMaterialReciptdate.EditValue = saleOrder.materialReciptDate;
            datPaymentDueFrom.EditValue = saleOrder.PaymentDueStartDate;
            txtSalesref.Text = saleOrder.SalesReferenceNo;
            txtOfferRefNo.Text = saleOrder.offerReferenceNo;
            txtsaleOrderref.Text = saleOrder.referenceNo;
            if (saleOrder.StatusClass != null)
                checkStatusClass = saleOrder.StatusClass;
            double invoiced = 0;
            if (saleOrder?.SaleInvoices != null && saleOrder?.SaleInvoices.Count > 0)
            {
                var collected = Convert.ToDouble(saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                collected = Math.Round(collected, 2);
                txtReceivedtotal.Text = collected.ToString();
                var collectedSER = Convert.ToDouble(saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount) * x.exchangeRate));
                collectedSER = Math.Round(collectedSER, 2);
                txtReceivedtotalSER.Text = collectedSER.ToString();
                txtReceivedtotalMER.Text = collectedSER.ToString();
                if (saleOrder.saleOrdertype == InquiryType.Principal)
                {
                    var invoicedAmount = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(z => z.totalInvoiceAmount);
                    var invoicedAmountSER = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(z => z.totalInvoiceAmount * z.exchangeRate);

                    txtRemainingReciepts.Text = Math.Round((Convert.ToDouble(invoicedAmount) - collected), 2).ToString();
                    txtRemainingRecieptsSER.Text = Math.Round((Convert.ToDouble(invoicedAmountSER) - collectedSER), 2).ToString();
                    txtRemainingRecieptsMER.Text = Math.Round((Convert.ToDouble(invoicedAmountSER) - collectedSER), 2).ToString();
                }
                else if(saleOrder.saleOrdertype == InquiryType.DistributionBiz)
                {
                    var invoiceAmount = Math.Round( (saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(y => y.BookerStatementItems.Sum(z=>z.siNetAmount))).Value, 2);
                    var invoiceAmountSER =Math.Round( (saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(y => y.BookerStatementItems.Sum(z => z.siNetAmount) * y.exchangeRate)).Value, 2);

                    txtRemainingReciepts.Text = Math.Round((Convert.ToDouble(invoiceAmount) - collected), 2).ToString();
                    txtRemainingRecieptsSER.Text = Math.Round((Convert.ToDouble(invoiceAmountSER) - collectedSER), 2).ToString();
                    txtRemainingRecieptsMER.Text = Math.Round((Convert.ToDouble(invoiceAmountSER) - collectedSER), 2).ToString();
                }
                else if (saleOrder.saleOrdertype == InquiryType.DistributionBiz_CustomerCredit)
                {
                    var invoiceAmount = Math.Round((saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(y => y.BookerStatementItems.Sum(z => z.siNetAmount))).Value, 2);
                    var invoiceAmountSER = Math.Round((saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(y => y.BookerStatementItems.Sum(z => z.siNetAmount) * y.exchangeRate)).Value, 2);

                    txtRemainingReciepts.Text = Math.Round((Convert.ToDouble(invoiceAmount) - collected), 2).ToString();
                    txtRemainingRecieptsSER.Text = Math.Round((Convert.ToDouble(invoiceAmountSER) - collectedSER), 2).ToString();
                    txtRemainingRecieptsMER.Text = Math.Round((Convert.ToDouble(invoiceAmountSER) - collectedSER), 2).ToString();
                }
                else
                {
                    txtRemainingReciepts.Text = Math.Round((saleOrder.totalCFRValue - collected), 2).ToString();

                    if (!string.IsNullOrEmpty(txtSalestotalCfr.Text))
                        txtRemainingRecieptsSER.Text = Math.Round((saleOrder.totalCFRValue * Convert.ToDouble(saleOrder.marginExchangeRate) - collectedSER), 2).ToString();
                    if (!string.IsNullOrEmpty(txtSalestotalCfr.Text))
                        txtRemainingRecieptsMER.Text = Math.Round((saleOrder.totalCFRValue * Convert.ToDouble(saleOrder.exchangeRate) - collectedSER), 2).ToString();
                }
                gridCollection.Visibility = Visibility.Visible;
                invoiced = collected;
            }
            else
            {
               // gridCollection.Visibility = Visibility.Collapsed;
            }
            datLCShipmentDate.EditValue = saleOrder.LCShipmentDate;
            datLCRevisedShipmentDate.EditValue = saleOrder.LCShipmentAmendmentDate;
            datLCRevisedExpiryDate.EditValue = saleOrder.LCExpiryAmedmentDate;
            datLCExpiryDate.EditValue = saleOrder.LCExpiryDate;
            datLCRevisedExpiryDate.EditValue = saleOrder.LCExpiryAmedmentDate;
            txtLCAmedmentno.Text = (string.IsNullOrEmpty(saleOrder.LCAmedmentNo)) ? "" : saleOrder.LCAmedmentNo;
            txtPacking.Text = (string.IsNullOrEmpty(saleOrder.packing)) ? "" : saleOrder.packing;
            if (saleOrder.transshipment == true)
                cmbTransshipment.SelectedIndex = 0;
            if (saleOrder.transshipment == false)
                cmbTransshipment.SelectedIndex = 1;
            chkInterCompany.IsChecked = saleOrder.isInterCompany;




            if (saleOrder.saleOrdertype == InquiryType.Principal)
            {
                SOCFRRemaining = Convert.ToDouble(saleOrder.commision);

                if (saleOrder.InvoiceStage == null)
                {
                    saleOrder.RemainingCFRValue = Convert.ToDouble(saleOrder.commision);
                    invoiced = Convert.ToDouble(saleOrder.commision) - saleOrder.RemainingCFRValue;
                }
                else if (saleOrder.InvoiceStage == InvoiceStage.None.ToString() && saleOrder.RemainingCFRValue == 0)
                {
                    saleOrder.RemainingCFRValue = Convert.ToDouble(saleOrder.commision);

                    invoiced = Convert.ToDouble(saleOrder.commision) - saleOrder.RemainingCFRValue;
                }
                else
                    invoiced = Convert.ToDouble(saleOrder.commision) - saleOrder.RemainingCFRValue;

            }
            else
            {
                SOCFRRemaining = saleOrder.totalCFRValue;
            }
            txtPERAmount.Text = saleOrder.PERValue.ToString();
            txtPER.Text = saleOrder.PER.ToString();
            txtSoAmountPer.Text = saleOrder.SoAmountPER.ToString();
            txttotalfob.Text = saleOrder.totalFOBValue.ToString();
            txttotalcfr.Text = saleOrder.totalCFRValue.ToString();
            txtBasetotalfob.Text = saleOrder.totalBaseFOBValue.ToString();
            txtBasetotalcfr.Text = saleOrder.totalBaseCFRValue.ToString();
            txtSalestotalCfr.Text = saleOrder.SoAmountSER.ToString();
            txtMaker.Text = saleOrder.maker;
            txtOrigin.Text = saleOrder.origin;
            txtOwnDescription.Text = saleOrder.OwnDescription;
            txtCommissionNumber.Text = saleOrder.commisionRefrenceNo;
            txtFinanaceRef.Text = saleOrder.FinanceRefrenceNo;
            txtSalesTargetYear.Text = saleOrder.targetYear.ToString();
            txtSalesTargetMonth.Text = saleOrder.targetMonth.ToString();
            txtPaymentDueDays.Text = saleOrder.CreditDays.ToString();
            txtLCNumber.Text = saleOrder.lCnumber;

            if(saleOrder.auditYear!=null)
            {
                datAuditYealy.EditValue = saleOrder.auditYear;
            }


            //inco term for po

            if (saleOrder.incoterm_Id != 0 && saleOrder.incoterm != null)
            {
                var incoSource = (List<cmbitem>)cmbIncoterm.Items.SourceCollection;
                cmbIncoterm.SelectedItem = cmbIncoterm.Items[cmbIncoterm.Items.IndexOf(incoSource.Find(x => x.id == saleOrder.incoterm_Id))];
            }
            //Warranty
            if (saleOrder.WarrantyId != 0 && saleOrder.Warranty != null)
            {
                var incoSource = (List<cmbitem>)cmbWarranty.Items.SourceCollection;
                cmbWarranty.SelectedItem = cmbWarranty.Items[cmbWarranty.Items.IndexOf(incoSource.Find(x => x.id == saleOrder.WarrantyId))];

            }
            //select Captions for Item Value 1
            if (saleOrder.TitleValue1Id != 0 || saleOrder.TitleValue1 != null)
            {
                var captionSource = (List<cmbitem>)cmbcaption1.Items.SourceCollection;
                cmbcaption1.SelectedItem = cmbcaption1.Items[cmbcaption1.Items.IndexOf(captionSource.Find(x => x.id == saleOrder.TitleValue1Id))];
            }
            //select Captions for Item Value 2
            if (saleOrder.TitleValue2Id != 0 || saleOrder.TitleValue2 != null)
            {
                var caption2Source = (List<cmbitem>)cmbcaption2.Items.SourceCollection;
                cmbcaption2.SelectedItem = cmbcaption2.Items[cmbcaption2.Items.IndexOf(caption2Source.Find(x => x.id == saleOrder.TitleValue2Id))];
            }
            //Payment term Load for sale Order
            if (saleOrder.paymentterm_Id != 0 && saleOrder.paymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbPaymentTerm.Items.SourceCollection;
                var term = paymentTermSource.Find(x => x.id == saleOrder.paymentterm_Id);

                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = saleOrder.paymentTerm.term, id = saleOrder.paymentTerm.Id });
                    cmbPaymentTerm.ItemsSource = null;
                    cmbPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbPaymentTerm.SelectedItem = cmbPaymentTerm.Items[cmbPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == saleOrder.paymentTerm.Id))];
            }

            if (saleOrder.offer_Id != null)
                offer = saleOrder.offer;
            // Select Creator
            if(saleOrder.user!=null)
                txtCreator.Text = saleOrder.user.employee.person.FName + " " + saleOrder.user.employee.person.LName;
            // Select Company
            if (saleOrder.company_Id != null || saleOrder.company != null)
            {
                company = saleOrder.company;
                lookupCompany.Text = saleOrder.company.CompanyName;
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }



            // Select Department
            if (saleOrder.dept_Id != 0 || saleOrder.department != null)
            {
                lookupDepartment.Text = saleOrder.department.DeptName;
                department = saleOrder.department;
                //lookupCustomer.ItemsSource = department.customers;
            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }


            //Select Bill Reference No
            var billRefList = (cmbxBillRef.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbxBillRef.ItemsSource as List<cmbitem>;
            if (saleOrder.BillRefNo != null)
            {
                int ind = 0;
                foreach (var _ref in billRefList)
                {
                    if (_ref.id == saleOrder.BillRefNoId)
                    {
                        cmbxBillRef.SelectedIndex = ind;
                        ind = 0;
                        break;
                    }
                    ind++;
                }
            }

            if (saleOrder.InterCompany_Id != null || saleOrder.InterCompany != null)
            {
                var companylist = (lookupInterCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupInterCompany.ItemsSource as List<Company>;
                if (saleOrder.InterCompany != null && companylist.Find(x => x.Id == saleOrder.InterCompany_Id) == null)
                {
                    companylist.Add(saleOrder.InterCompany);
                    lookupInterCompany.ItemsSource = companylist;
                }
                InterCompany = saleOrder.InterCompany;
                lookupInterCompany.Text = saleOrder.InterCompany.CompanyName;
            }
            if (saleOrder.InterDepartment_Id != null || saleOrder.InterDepartment != null)
            {
                var deptlist = (lookupInterDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupInterDepartment.ItemsSource as List<Department>;

                if (saleOrder.InterDepartment != null && deptlist.Find(x => x.Id == saleOrder.InterDepartment_Id) == null)
                {
                    deptlist.Add(saleOrder.InterDepartment);
                    lookupInterDepartment.ItemsSource = deptlist;
                }
                lookupInterDepartment.Text = saleOrder.InterDepartment?.DeptName;
                InterDepartment = saleOrder.InterDepartment;
            }
            //Select Customer
            if (saleOrder.customerCompany.Id != 0 || saleOrder.customerCompany != null)
            {
                lookupCustomer.Text = saleOrder.customerCompany.company.CompanyName;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            //Select vendor
            if (saleOrder.vendors != null)
            {

                foreach (Vendor vendr in saleOrder.vendors)
                {
                    lookupVendor.Text = vendr.company.CompanyName;
                }
            }

            //Select Principal
            if (saleOrder.principal_Id != 0 || saleOrder.principal != null)
            {
                lookupPrincipal.Text = saleOrder.principal.company.CompanyName;
                principal = saleOrder.principal;
            }
            else
            {
                lookupPrincipal.Text = "Select Vendor";

            }
            // Select Employee 
            if (saleOrder.allocation_Id != 0 || saleOrder.employee != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == saleOrder.allocation_Id))];

                }
                catch
                {

                }
            }
            if (saleOrder.bid_Id != null && saleOrder.bid_Id != 0 && saleOrder.bid != null)
            { //load form data for bid
                grpbondinfo.State = GroupBoxState.Normal;
                datBidBondIssuedate.DateTime = saleOrder.bid.issueDate;
                txtBidBondRefno.Text = saleOrder.bid.refNo;
                txtBidBonValue.Text = saleOrder.bid.value;
                datBidBondSubmitdate.DateTime = saleOrder.bid.submitDate;
                datBidBondExpirydate.DateTime = saleOrder.bid.expireDate;
                txtIssuingbank.Text = saleOrder.bid.bankName;
            }

            if (saleOrder.saleOrderStatus != null)
            {
                var disAbleStatus = SaleOrderStatuses.FirstOrDefault(x => x.Id == saleOrder.saleOrderStatus.Id);
                if (disAbleStatus == null)
                {
                    loadSaleOrderStatus(saleOrder.saleOrderStatus);
                }
            }

            var SOSource = (List<cmbitem>)cmbSaleOrderStatus.Items.SourceCollection;

            // Select SaleOrder Status 
            if (saleOrder.saleOrderStatus.isActive == false)
            {
                try
                {
                    cmbSaleOrderStatus.SelectedItem = cmbSaleOrderStatus.Items[cmbSaleOrderStatus.Items.IndexOf(SOSource.Find(x => x.name == saleOrder.saleOrderStatus.Status))];
                }
                catch (Exception ex)
                {
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Sale Order status! " + ex.ToString());
                }
            }
            else
            {
                cmbSaleOrderStatus.SelectedItem = cmbSaleOrderStatus.Items[cmbSaleOrderStatus.Items.IndexOf(SOSource.Find(x => x.name == saleOrder.saleOrderStatus.Status))];
            }

            checkStatus = saleOrder.saleOrderStatus;
            foreach (cmbitem cmbitem in cmbVendorPaymentStatus.Items)
            {
                if (saleOrder.vendorPaymentStatus != null)
                    if (cmbitem.id == saleOrder.vendorPaymentStatus.Id)
                    {
                        cmbVendorPaymentStatus.SelectedItem = cmbitem;
                        break;
                    }
            }
            if (saleOrder.StatusClass != null)
            {
                var disAbleStatus = StatusClasses.FirstOrDefault(x => x.Id == saleOrder.statusClass_Id);
                if (disAbleStatus == null)
                {
                    loadSaleOrderStatusClass(saleOrder.StatusClass);
                }
                var StatusClassSource = (List<cmbitem>)cmbSaleOrderStatusClass.Items.SourceCollection;

                // Select SaleInvoice Status 
                if (saleOrder.StatusClass.isActive == false)
                {
                    try
                    {
                        cmbSaleOrderStatusClass.SelectedItem = cmbSaleOrderStatusClass.Items[cmbSaleOrderStatusClass.Items.IndexOf(StatusClassSource.Find(x => x.name == saleOrder.StatusClass.ClassName))];
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    cmbSaleOrderStatusClass.SelectedItem = cmbSaleOrderStatusClass.Items[cmbSaleOrderStatusClass.Items.IndexOf(StatusClassSource.Find(x => x.name == saleOrder.StatusClass.ClassName))];
                }
            }

            if (saleOrder.saleOrdertype == InquiryType.DistributionBiz || saleOrder.saleOrdertype == InquiryType.DistributionBiz_CustomerCredit)
            { 

                if(saleOrder.BookerStatementItems.Count>0)
                {
                    grdBokkerItems.ItemsSource = saleOrder.BookerStatementItems;
                    txtRevisedAmount.Text = saleOrder.revisedBudgetAmount.ToString();
                }

            }
            else
            {
                grdBudgetPunching.Visibility = Visibility.Collapsed;
                if (saleOrder.products != null)
                {

                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                    foreach (var procurementProduct in saleOrder.products)
                    {
                        procurementProducts.Add(new ProcurementProduct()
                        {
                            Id = procurementProduct.Id,
                            inquiryProduct = new InquiryProduct()
                            {
                                Id = procurementProduct.inquiryProduct.Id,
                                ownDiscription = procurementProduct.inquiryProduct.ownDiscription,
                                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                quantity = procurementProduct.inquiryProduct.quantity,
                                Weight = procurementProduct.inquiryProduct.Weight,
                                product = new Product()
                                {
                                    Id = procurementProduct.inquiryProduct.product.Id,
                                    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                    item = procurementProduct.inquiryProduct.product.item,
                                    code = procurementProduct.inquiryProduct.product.code,
                                    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                    unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                    nature = procurementProduct.inquiryProduct.product.nature,
                                    category = procurementProduct.inquiryProduct.product.category,
                                    isActive = procurementProduct.inquiryProduct.product.isActive
                                }
                                    ,
                                product_Id = procurementProduct.inquiryProduct.product.Id

                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,
                            UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity,
                            InvoicedQuantity = (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,
                            UnInvoicedWeight = (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,
                            InvoicedWeight = (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,
                            value1 = procurementProduct.value1,
                            value2 = procurementProduct.value2,
                            caption1 = cmbcaption1.Text.Trim(),
                            caption2 = cmbcaption2.Text.Trim(),
                            UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                            priority = procurementProduct.priority
                        });



                        grdSOItems.ItemsSource = procurementProducts;
                        int x = 0;
                        foreach (var pro in grdSOItems.ItemsSource as List<ProcurementProduct>)
                        {
                            grdSOItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                            x++;
                        }
                        lookupProductsinGrid.DisplayMember = "code";
                    }


                }
                else
                {
                    grdSOItems.ItemsSource = procurementProducts;
                }
            }

                
            // selected currency of customer company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                if (cmbitem.id == saleOrder.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
           
            if (saleOrder.currency_Id != 0 && saleOrder.currency != null)
            {
                var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleOrder.currency_Id))];
            }
            else
            {
                var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleOrder.company.CurrencyId))];
            }
           
            
            txtCommision.Text = saleOrder.commision.ToString();
            txtBaseCommission.Text = saleOrder.commisioninBase.ToString();
            if(saleOrder.saleOrdertype== InquiryType.Principal  && saleOrder.netCommision==0)
            {
                txtNetCommision.Text= saleOrder.commision.ToString();
            }
            else
            {
                txtNetCommision.Text = (saleOrder.netCommision).ToString();
            }
            //Margin
            if (saleOrder != null && saleOrder.CostSheet != null)
            {
                var systemCost = saleOrderRepo.GetSOSystemCost((int)saleOrder.CostSheet_Id);
                var adjCost = saleOrderRepo.GetAdjustmentCost(saleOrder.Id, (int)saleOrder.CostSheet_Id);
                systemCost = systemCost + adjCost;

                if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    txtBudgetMargin.Text = (Convert.ToDecimal(txtSoAfterGST.Text) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                    //txtRevisedMargin.Text = (Convert.ToDecimal(txtSoAfterGST.Text) - saleOrder.CostSheet.TotalRevisedMargin).ToString();
                    txtActualMargin.Text = (Convert.ToDecimal(txtSoAfterGST.Text) - saleOrder.CostSheet.TotalActualMargin).ToString();
                    txtSystemMargin.Text = (Convert.ToDecimal(txtSoAfterGST.Text) - systemCost).ToString();
                }
                else
                {
                    decimal soAmount = 0;

                    if(saleOrder.costCenterAmount!=0)
                    {
                        soAmount = Convert.ToDecimal(saleOrder.costCenterAmount);
                    }
                    else
                    {
                        soAmount = Convert.ToDecimal(saleOrder.totalCFRValue);

                    }
                    txtBudgetMargin.Text = (soAmount - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                    //txtRevisedMargin.Text= (soAmount- saleOrder.CostSheet.TotalRevisedMargin).ToString();
                    txtActualMargin.Text = (soAmount - saleOrder.CostSheet.TotalActualMargin).ToString();
                    txtSystemMargin.Text = (soAmount - systemCost).ToString();
                }

                    
            }
            else
            {
                txtBudgetMargin.Text = saleOrder.margin.ToString();
                //txtRevisedMargin.Text = saleOrder.RevisedMargin.ToString();
                //txtBaseRevisedMargin.Text = saleOrder.RevisedMargininBase.ToString();
                //txtSaleRevisedMargin.Text = saleOrder.SalesRevisedMargin.ToString();

                txtActualMargin.Text = saleOrder.ActualMargin.ToString();
                txtBaseBudgetMargin.Text = saleOrder.BudgetedMargininBase.ToString();
                txtBaseActualMargin.Text = saleOrder.ActualMargininBase.ToString();
                txtSaleBudgetMargin.Text = saleOrder.SalesBudgetedMargin.ToString();
                txtSaleAMargin.Text = saleOrder.SalesActualMargin.ToString();
                txtSystemMargin.Text = saleOrder.SystemMargin.ToString();
            }
           
            calculatetotal();
            if (saleOrder.Id != 0)
            {
                if (saleOrder.taxNameId != null)
                {
                    TaxRepo taxRepo = new TaxRepo();
                    saleOrderTax = taxRepo.getTaxtById((int)saleOrder.taxNameId);
                    lookupSalesTax.Text = saleOrderTax.Name;
                    isSalesTax.IsChecked = true;
                }
                else
                {
                    isSalesTax.IsChecked = false;
                }
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Commision Field without Approval") == null)
            {
                txtCommision.IsReadOnly = true;
            }
            if (saleOrder.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order value before approval") != null)
            {
                if ((InquiryType)cmbSaleOrderType.SelectedIndex != InquiryType.Inventory)
                {
                    grdSOItems.Columns.GetColumnByFieldName("value1").ReadOnly = false;
                    grdSOItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;
                    isSalesTax.IsReadOnly = false;
                    grdTax.IsEnabled = true;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Sale Order") != null)
                    {
                        grdsaleOrderdata.IsEnabled = true;
                    }
                    else
                        grdsaleOrderdata.IsEnabled = false;
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order value after approval") == null && iscopyTemplate != true)
            {


                grdSOItems.Columns.GetColumnByFieldName("value1").ReadOnly = true;
                grdSOItems.Columns.GetColumnByFieldName("value2").ReadOnly = true;
                isSalesTax.IsReadOnly = true;
                grdTax.IsEnabled = false;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit (Pending for closing) SaleOrder") != null && saleOrder.PendingForClosing == true)
            {
                grdsaleOrderdata.IsEnabled = true;
                btnAttachment.IsEnabled = true;
            }
            else if (saleOrder.saleOrderStatus.isActive == false && MainWindow.currentUserid != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed SaleOrder") == null))
            {
                grdsaleOrderdata.IsEnabled = false;
                //btnAttachment.IsEnabled = false;
                labeltopStatus.Visibility = Visibility.Visible;
                labeltopStatus.Text = saleOrder.saleOrderStatus.Status;
                btnSave.IsEnabled = false;
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(saleOrder.saleOrderStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                labeltopStatus.Foreground = new SolidColorBrush(newColor);
                var rotateAnimation = new DoubleAnimation(-30, -32, TimeSpan.FromSeconds(60));
                var rt = (RotateTransform)labeltopStatus.RenderTransform;
                rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View CostSheet when Sale Order closed") != null && saleOrder.PendingForClosing != true && saleOrder.Id != 0)
                {
                    grdsaleOrderdata.IsEnabled = true;
                    grpbasicinfo.IsEnabled = true;
                    grpsaleOrderinfo.IsEnabled = false;
                    grpInterCompany.IsEnabled = false;
                    grdSOItems.IsEnabled = false;
                    grpbondinfo.IsEnabled = false;
                    cmbbaseCurrency.IsEnabled = false;
                    cmbCurrency.IsEnabled = false;
                    txtPERAmount.IsEnabled = false;
                    txtPER.IsEnabled = false;
                    txtSoAmountPer.IsEnabled = false;
                    txtMarginexchangerate.IsEnabled = false;
                    txtexchangerate.IsEnabled = false;
                    txtSoAmount.IsEnabled = false;
                    txtSalestotalCfr.IsEnabled = false;
                    txtBasetotalcfr.IsEnabled = false;
                    grpCommisiondetails.IsEnabled = false;
                    grpsummries.IsEnabled = false;
                    btnCostSheet.IsEnabled = true;
                    btnBudgertPunching.IsEnabled = true;

                }
            }
            if (saleOrder.deliveryDateFinal != null)
            {
                datDeliverydateFinal.EditValue = saleOrder.deliveryDateFinal;
            }
            if (iscopyTemplate == true)
            {
                saleOrder = new SaleOrder();
                datpoCreationdate.EditValue = DateTime.Now;
                editOrder = 0;
            }
            txtexchangerate.Text = saleOrder.exchangeRate.ToString();
            txtMarginexchangerate.Text = saleOrder.marginExchangeRate.ToString();
            if(saleOrder.totaltaxAmount!=0)
            {
                isSalesTax.IsChecked = true;
                txttax.Text = saleOrder.totaltaxAmount.ToString();
            }
            else
            {
                isSalesTax.IsChecked = false;
            }

            if (saleOrder.interBankTransfer_Id != null)
            {
                gridIBT.Visibility = Visibility.Visible;
                interBankTransfer = interBankTransferRepo.GetInterBankTransfer((int)saleOrder.interBankTransfer_Id);
                txtTotalIBTValue.Text = interBankTransfer.AmountOC.ToString();
                txtAdjustedTBT.Text= Math.Round(interBankTransfer.SaleOrders.Sum(x => x.totalCFRValue),2).ToString();
                txtRemainingIBT.Text = Math.Round( (interBankTransfer.AmountOC- interBankTransfer.SaleOrders.Sum(x => x.totalCFRValue)),2).ToString();
            }
            else
            {
                gridIBT.Visibility = Visibility.Collapsed;
            }
            if(saleOrder.PerformanceSheet_Id!=null)
            {

                EmployeeRepo repo = new EmployeeRepo();
                var staffLevelOne=repo.GetEmployee((int)saleOrder.PerformanceSheet.staffLevelOneId);
                var staffLevelTwo= repo.GetEmployee((int)saleOrder.PerformanceSheet.staffLevelTwoId);
                var supervisedBy= repo.GetEmployee((int)saleOrder.PerformanceSheet.supervoisedId);
                txtAllocatedTo.Text = supervisedBy.person.FName + " " + supervisedBy.person.LName; 
                if(supervisedBy.person.Photo!=null)
                imgAllocatedTo.ImageSource  =  GetBitmapImageFromByteArray(supervisedBy.person.Photo);
                txtSupervisedBy.Text = staffLevelOne.person.FName + " " + staffLevelOne.person.LName;
                if (staffLevelOne.person.Photo != null)
                    imgSupervisedBy.ImageSource = GetBitmapImageFromByteArray(staffLevelOne.person.Photo);
                txtDepartmentHead.Text = staffLevelTwo.person.FName + " " + staffLevelTwo.person.LName;
               if(staffLevelTwo.person.Photo!=null)
                imgDepartmentHead.ImageSource = GetBitmapImageFromByteArray(staffLevelTwo.person.Photo);
                txtTotalReceivedPoints.Text= saleOrder.PerformanceSheet.totalPoints.ToString();
                txtTotalPointsPerc.Text= saleOrder.PerformanceSheet.totalPointsPerc.ToString();
            }
            if(saleOrder.referenceKey!=null)
            {
                btnGenerateRef.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnGenerateRef.Visibility = Visibility.Visible;
            }
            if (saleOrder.transactionHolderId != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                    cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == saleOrder.transactionHolderId))];

                }
                catch (Exception ex)
                {

                }
            }
            if (saleOrder.holderChangeDate != null)
            {
                var time = DateTime.Now - saleOrder.holderChangeDate;
                txtHolderDays.Text = time.Days.ToString();
            }
            foreach (cmbitem cmbitem in cmbCostCenterCurrency.Items)
            {
                if (cmbitem.id == saleOrder.costCentercurrency_Id)
                {
                    cmbCostCenterCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            txtCostCenterExchangeRate.Text = saleOrder.costCenterExchangeRate.ToString();
            txtCostCenterAmount.Text = saleOrder.costCenterAmount.ToString();
         
            if (saleOrder.costCentercurrency_Id != 0 && saleOrder.costcenterCurrency != null)
            {
                var currencySource = (List<cmbitem>)cmbCostCenterCurrency.Items.SourceCollection;
                cmbCostCenterCurrency.SelectedItem = cmbCostCenterCurrency.Items[cmbCostCenterCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleOrder.costCentercurrency_Id))];
            }
            calculateBaseBudgetMargin();
            calculateBaseActualMargin();
            calculateBaseRevisedMargin();
            calculateSystemMargins();
        }
        public List<BookerStatementItem> getBookerStatementItems()
        {
            List<BookerStatementItem> bookeritems = new List<BookerStatementItem>();
            List<BookerStatementItem> bookerProducts = new List<BookerStatementItem>();

            bookerProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;

            foreach (var item in bookerProducts)
            {
                if (editOrder != 0)
                {
                    if (item.Id != 0)
                    {
                        item.saleOrderId = saleOrderid;
                        item.unit = item.unit;

                        item.amount = item.amount;
                        item.amountGST = item.amountGST;
                        item.focValue = item.focValue;
                        item.netAmount = item.netAmount;
                        item.passOnValue = item.passOnValue;
                        item.weight = item.weight;
                        item.quantity = item.quantity;
                        item.claimDiscountValue = item.claimDiscountValue;
                        item.focValue = item.focValue;
                        item.passOnValue = item.passOnValue;




                        item.siAmount = item.siAmount;
                        item.siAmountGST = item.siAmountGST;
                        item.siFocValue = item.siFocValue;
                        item.siNetAmount = item.siNetAmount;
                        item.siPassOnValue = item.siPassOnValue;
                        item.siQuantity = item.siQuantity;
                        item.siWeight = item.siWeight;
                        item.siClaimDiscountValue = item.siClaimDiscountValue;
                        item.siFocValue = item.siFocValue;
                        item.siPassOnValue = item.siPassOnValue;
                        if (item.product.Id != 0)
                        {
                            item.product_Id = productRepo.get((int)item.product.Id).Id;
                        }
                        if (item.TaxName != null)
                        {
                            var tax = taxRepo.getTaxtById((int)item.TaxName.Id);
                            if (item.taxNameId == null)
                                item.TaxName = null;
                            item.taxNameId = tax.Id;


                        }

                        if (item.ClaimDiscount != null)
                        {
                            var claim = offerRepo.GetClaimDiscount((int)item.ClaimDiscount.Id);
                            if (item.claimDiscountId == null)
                                item.ClaimDiscount = null;
                            item.claimDiscountId = claim.Id;

                        }
                        if (item.FOCSampling != null)
                        {
                            var foc = offerRepo.GetFOCSampling((int)item.FOCSampling.Id);
                            if (item.focSamplingId == null)
                                item.FOCSampling = null;
                            item.focSamplingId = foc.Id;

                        }
                        if (item.PassOn != null)
                        {
                            var passOn = offerRepo.GetPassOn((int)item.PassOn.Id);
                            if (item.passOnId == null)
                                item.PassOn = null;
                            item.passOnId = passOn.Id;

                        }
                        bookeritems.Add(item);
                    }
                    else
                    {
                        BookerStatementItem bItem = new BookerStatementItem();
                        item.saleOrderId = saleOrderid;
                        bItem.unit = item.unit;
                        bItem.amount = item.amount;
                        bItem.amountGST = item.amountGST;
                        bItem.netAmount = item.netAmount;
                        bItem.weight = item.weight;
                        bItem.quantity = item.quantity;
                        bItem.passOnValue = item.passOnValue;
                        bItem.claimDiscountValue = item.claimDiscountValue;
                        bItem.focValue = item.focValue;



                        bItem.siAmount = item.siAmount;
                        bItem.siAmountGST = item.siAmountGST;
                        bItem.siFocValue = item.siFocValue;
                        bItem.siNetAmount = item.siNetAmount;
                        bItem.siPassOnValue = item.siPassOnValue;
                        bItem.siQuantity = item.siQuantity;
                        bItem.siWeight = item.siWeight;
                        bItem.siPassOnValue = item.siPassOnValue;
                        bItem.siClaimDiscountValue = item.siClaimDiscountValue;
                        bItem.siFocValue = item.siFocValue;



                        if (item.ClaimDiscount != null)
                        {
                            bItem.claimDiscountId = offerRepo.GetFOCSampling((int)item.ClaimDiscount.Id).Id;
                        }
                        if (item.FOCSampling != null)
                        {
                            bItem.focSamplingId = offerRepo.GetFOCSampling((int)item.FOCSampling.Id).Id;
                        }

                        if (item.PassOn != null)
                        {
                            bItem.passOnId = offerRepo.GetPassOn((int)item.PassOn.Id).Id;
                        }
                        if (item.product != null)
                        {
                            bItem.product_Id = productRepo.get((int)item.product.Id).Id;
                        }
                        if (item.TaxName != null)
                        {
                            bItem.taxNameId = taxRepo.getTaxtById((int)item.TaxName.Id).Id;
                        }

                        bookeritems.Add(bItem);
                    }
                }
                else
                {
                    BookerStatementItem bItem = new BookerStatementItem();
                    item.saleOrderId = saleOrderid;
                    bItem.unit = item.unit;
                    bItem.amount = item.amount;
                    bItem.amountGST = item.amountGST;
                    bItem.netAmount = item.netAmount;
                    bItem.weight = item.weight;
                    bItem.quantity = item.quantity;
                    bItem.passOnValue = item.passOnValue;
                    bItem.claimDiscountValue = item.claimDiscountValue;
                    bItem.focValue = item.focValue;



                    bItem.siAmount = item.siAmount;
                    bItem.siAmountGST = item.siAmountGST;
                    bItem.siFocValue = item.siFocValue;
                    bItem.siNetAmount = item.siNetAmount;
                    bItem.siPassOnValue = item.siPassOnValue;
                    bItem.siQuantity = item.siQuantity;
                    bItem.siWeight = item.siWeight;
                    bItem.siPassOnValue = item.siPassOnValue;
                    bItem.siClaimDiscountValue = item.siClaimDiscountValue;
                    bItem.siFocValue = item.siFocValue;

                    if (item.ClaimDiscount != null)
                    {
                        bItem.claimDiscountId = offerRepo.GetFOCSampling((int)item.ClaimDiscount.Id).Id;
                    }
                    if (item.FOCSampling != null)
                    {
                        bItem.focSamplingId = offerRepo.GetFOCSampling((int)item.FOCSampling.Id).Id;
                    }

                    if (item.PassOn != null)
                    {
                        bItem.passOnId = offerRepo.GetPassOn((int)item.PassOn.Id).Id;
                    }
                    if (item.product != null)
                    {
                        bItem.product_Id = productRepo.get((int)item.product.Id).Id;
                    }
                    if (item.TaxName != null)
                    {
                        bItem.taxNameId = taxRepo.getTaxtById((int)item.TaxName.Id).Id;
                    }
                    bookeritems.Add(bItem);
                }
            }
            return bookeritems;
        }
        public List<ProcurementProduct> getProductsdata()
        {
           
            if (iscopyTemplate != true)
            {
                List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
                List<ProcurementProduct> saleOrderItems = new List<ProcurementProduct>();
                ProcurementProduct product = new ProcurementProduct();
                procurementProducts = grdSOItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {
                    
                    if (procurementProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.Id == 0)
                        {
                            if (procurementProduct.inquiryProduct.product != null /*&& procurementProduct.inquiryProduct.product_Id != 0*/)
                            {
                                saleOrderItems.Add(new ProcurementProduct()
                                {
                                    inquiryProduct = new InquiryProduct()
                                    {
                                        Id = procurementProduct.inquiryProduct.Id,
                                        ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                        UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                        quantity = procurementProduct.inquiryProduct.quantity,
                                        Weight = procurementProduct.inquiryProduct.Weight,
                                        product_Id = procurementProduct.inquiryProduct.product.Id
                                    },
                                    product_Id = procurementProduct.inquiryProduct.Id,
                                    unitPrice = procurementProduct.unitPrice,

                                    value1 = procurementProduct.value1,
                                    value2 = procurementProduct.value2,
                                    caption1 = cmbcaption1.Text.Trim(),
                                    caption2 = cmbcaption2.Text.Trim(),
                                    priority = procurementProduct.priority
                                });
                            }
                        }
                        else
                        {
                            if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)

                                saleOrderItems.Add(new ProcurementProduct()
                                {
                                    inquiryProduct = new InquiryProduct()
                                    {
                                        Id = procurementProduct.inquiryProduct.Id,
                                        ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                        UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                        quantity = procurementProduct.inquiryProduct.quantity,
                                        Weight = procurementProduct.inquiryProduct.Weight,
                                        product_Id = procurementProduct.inquiryProduct.product.Id
                                    },
                                    product_Id = procurementProduct.inquiryProduct.Id,
                                    unitPrice = procurementProduct.unitPrice,

                                    value1 = procurementProduct.value1,
                                    value2 = procurementProduct.value2,
                                    caption1 = cmbcaption1.Text.Trim(),
                                    caption2 = cmbcaption2.Text.Trim(),
                                    priority = procurementProduct.priority


                                });
                        }
                    }
                    else
                    {
                        product = new ProcurementProduct();
                      
                        product = saleOrderRepo.GetProcurementProduct(procurementProduct.Id);
                        if(product.Id!=0)
                        {
                            product.unitPrice = procurementProduct.unitPrice;
                            product.UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount;
                            product.UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity;
                            product.UnInvoicedWeight = procurementProduct.UnInvoicedWeight;
                            product.value1 = procurementProduct.value1;
                            product.value2 = procurementProduct.value2;
                            product.caption1 = cmbcaption1.Text.Trim();
                            product.caption2 = cmbcaption2.Text.Trim();
                            product.inquiryProduct.ownDiscription = procurementProduct.inquiryProduct.ownDiscription;
                            product.inquiryProduct.UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                            product.inquiryProduct.quantity = procurementProduct.inquiryProduct.quantity;
                            product.inquiryProduct.Weight = procurementProduct.inquiryProduct.Weight;
                            product.priority = procurementProduct.priority;
                        }
                        saleOrderItems.Add(product);
                    }
                }
                return saleOrderItems;
            }
            else
            {
                List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
                List<ProcurementProduct> saleOrderItems = new List<ProcurementProduct>();
                procurementProducts = grdSOItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {

                    ProcurementProduct product = new ProcurementProduct()
                    {
                        inquiryProduct = new InquiryProduct()
                        {
                            ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                            UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                            quantity = procurementProduct.inquiryProduct.quantity,
                            Weight = procurementProduct.inquiryProduct.Weight,
                            product_Id = procurementProduct.inquiryProduct.product.Id
                        },
                        unitPrice = procurementProduct.unitPrice,
                        value1 = procurementProduct.value1,
                        value2 = procurementProduct.value2,
                        caption1 = cmbcaption1.Text.Trim(),
                        caption2 = cmbcaption2.Text.Trim(),
                        priority = procurementProduct.priority
                    };
                    saleOrderItems.Add(product);
                }
                return saleOrderItems;
            }
           
        }

        private void loadBillReferenceNo()
        {
            //BillsRepo = new AdminBillsRepo();
            var references = saleOrderRepo.GetAllBillReferenceNo().Where(x => x.companyId == company.Id && x.isActive == true).ToList();

            if (editOrder == 1 && saleOrder != null)
            {
                if (saleOrder.BillRefNo != null && references.FirstOrDefault(x => x.Id == saleOrder.BillRefNoId) == null)
                    references.Add(saleOrder.BillRefNo);
            }

            List<cmbitem> cmbitems = new List<cmbitem>();

            cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
            foreach (VendorBillReference _ref in references)
            {
                cmbitems.Add(new cmbitem() { name = _ref.Reference, id = _ref.Id });
            }

            cmbxBillRef.ItemsSource = cmbitems;
        }

        private void btnSaleOrderSave_Click(object sender, RoutedEventArgs e)
        {
            ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
            NotificationsRepo notificationsRepo = new NotificationsRepo();

          
            try
            {
                if (cmbSaleOrderStatusClass.SelectedIndex == -1 )
                {
                    cmbSaleOrderStatusClass.Focus();
                    MessageBox.Show("Please Select a status class against SO", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;
                }
                if (lookupVendor.SelectedIndex == -1 && vendor.Id == 0)
                {
                    lookupVendor.Focus();
                    MessageBox.Show("Please Select a vendor against PO", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                else if (lookupCustomer.SelectedIndex == -1 && customer.Id == 0)
                {
                    MessageBox.Show("Please Select a Customer for whom PO is being genrated", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCustomer.Focus();
                    return;
                }
                else if (lookupCompany.SelectedIndex == -1 && company.Id == 0)
                {
                    MessageBox.Show("Please select a Refrence Company", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCompany.Focus();
                    return;
                }
                else if (lookupDepartment.SelectedIndex == -1 && department.Id == 0)
                {
                    MessageBox.Show("Please Select a Refrence Department", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupDepartment.Focus();
                    return;
                }
                else if (cmbEmployee.SelectedIndex == -1 && employee.EmpId == 0)
                {
                    MessageBox.Show("Please Select an Employee to whom this Sale Order will be Allocated ", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                else if (cmbSaleOrderStatus.SelectedIndex == -1 && saleOrder.PendingForClosing != true)
                {
                    MessageBox.Show("Please Select Current Status of Sale Order to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbSaleOrderStatus.Focus();
                    return;
                }
                else if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Sale Orders Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbCurrency.Focus();
                    return;
                }
                else if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Cost Center Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbCostCenterCurrency.Focus();
                    return;
                }
                else
                if((InquiryType)cmbSaleOrderType.SelectedIndex != InquiryType.DistributionBiz || (InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    if (Convert.ToDouble(txttotalcfr.Text) == 0 || 0 == Convert.ToDouble(txttotalfob.Text))
                    {
                        MessageBox.Show("Please Enter Amount for the Items", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        grdSOItems.Focus();
                        return;
                    }
                }
                if (cmbPaymentTerm.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select PaymentTerm", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbPaymentTerm.Focus();
                    return;
                }
                else if (cmbIncoterm.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Incoterm", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbIncoterm.Focus();
                    return;
                }
                else if (lookupPrincipal.SelectedIndex == -1 && principal.Id == 0)
                {
                    MessageBox.Show("Please Select Principal", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupPrincipal.Focus();
                    return;
                }
                else if (cmbcaption1.SelectedIndex == -1 || cmbcaption2.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a caption for Items Values", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbIncoterm.Focus();
                    return;
                }
                else if (cmbWarranty.SelectedIndex == -1 /*|| cmbcaption2.SelectedIndex == -1*/)
                {
                    MessageBox.Show("Please select a Warranty/Gurantee", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbWarranty.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtFinanaceRef.Text))
                {
                    MessageBox.Show("Please add finance reference number", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtFinanaceRef.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtSalesref.Text))
                {
                    MessageBox.Show("Please add sales reference number", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtSalesref.Focus();
                    return;
                }
                else if (cmbPaymentTerm.SelectedIndex == -1 /*|| cmbcaption2.SelectedIndex == -1*/)
                {
                    MessageBox.Show("Please select a Payment Term", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbPaymentTerm.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtMaker.Text))
                {
                    MessageBox.Show("Please add maker", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtMaker.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtOrigin.Text))
                {
                    MessageBox.Show("Please add origin", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtOrigin.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPacking.Text))
                {
                    MessageBox.Show("Please add packing", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtPacking.Focus();
                    return;
                }
                else if (cmbTransactionHolder.SelectedIndex==-1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }
                if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
                {
                   if (cmbCostCenterCurrency.SelectedIndex == -1)
                    {
                        MessageBox.Show("Please select cost center currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbTransactionHolder.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty( txtCostCenterExchangeRate.Text))
                    {
                        MessageBox.Show("Please input cost center exchange rate", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbTransactionHolder.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(txtCostCenterAmount.Text))
                    {
                        MessageBox.Show("Please input cost center amount", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbTransactionHolder.Focus();
                        return;
                    }

                }
                if(isGeneratedBySOLink==true)
                {
                    saleOrder.isGeneratedBySOLink = true;
                }
                else
                {
                    saleOrder.isGeneratedBySOLink = false;

                }

                if (cmbxBillRef.SelectedIndex > 0)
                {
                    saleOrder.BillRefNoId = (cmbxBillRef.SelectedItem as cmbitem).id;
                }
                else
                {
                    saleOrder.BillRefNoId = null;
                }

                if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {

                    saleOrder.BookerStatementItems = getBookerStatementItems();
                    saleOrder.totalDistributionAmount = Convert.ToDouble(txtTotalSOAmount.Text);
                    saleOrder.totalGSTAmount = Convert.ToDouble(txtGstAmount.Text);
                    saleOrder.totalAmountAfterGST = Convert.ToDouble(txtSoAfterGST.Text);
                    saleOrder.totalClaimDiscount = Convert.ToDouble(txtTotalClaimDiscount.Text);
                    saleOrder.totalPassOn = Convert.ToDouble(txtTotalPassOnValue.Text);
                    saleOrder.totalFocSampling = Convert.ToDouble(txttotalfob.Text);
                    saleOrder.totalNetAmount = Convert.ToDouble(txtTotalNetAmount.Text);
                    saleOrder.revisedBudgetAmount = Convert.ToDouble(txtRevisedAmount.Text);
                }
                else
                {
                    saleOrder.products = getProductsdata();
                }
                
                if(interBankTransferId!=0)
                {
                    saleOrder.interBankTransfer_Id = interBankTransferId;
                }

                if(datAuditYealy.EditValue!=null)
                {
                    saleOrder.auditYear = (DateTime)datAuditYealy.EditValue;
                }
                if((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.Principal)
                {
                    if(string.IsNullOrEmpty(txtPERAmount.Text))
                    {
                        DXMessageBox.Show("Please input PER amount");
                        return;
                    }
                 
                    if(Convert.ToDouble( grdSplitPER.Columns["Amount"].TotalSummaries[0].Value) != Convert.ToDouble(txtPERAmount.Text))
                    {
                        DXMessageBox.Show("SO Amount (PER) not matching!");
                        txtSoAmountPer.Focus();
                        return;
                    }
                    var SOsplitPER = grdSplitPER.ItemsSource as List<SplitPER>;
                    List<SplitPER> splitPERsSO = new List<SplitPER>();
                    foreach (var _splitPER in SOsplitPER)
                    {
                        DateTime newDt = new DateTime(_splitPER.Year.Value.Year, _splitPER.Month.Value.Month, _splitPER.Month.Value.Day);
                        _splitPER.Month = newDt;

                        splitPERsSO.Add(new SplitPER()
                        {
                            Year = _splitPER.Year,
                            Month = _splitPER.Month,
                            Amount = _splitPER.Amount
                        });
                    }
                    saleOrder.SplitPERs = splitPERsSO;
                }
                SaleOrder order; // = new SaleOrder();
                order = saleOrderRepo.get(txtSalesref.Text.Trim());

                if (order != null && order.Id != saleOrder.Id)
                {
                    string message = "Sale Order with Sales reference # (" + order.SalesReferenceNo + ") already exists!";
                    if (order.isApproved == false)
                    {
                        message += " Which is pending for Approval. You can't to add duplicate data?";
                    }
                    else if (order.PendingForClosing == true)
                    {
                        message += " Which is pending for Closing and needs approval to be closed. You can't add duplicate data?";

                    }
                    else
                    {
                        message += " You can't add duplicate data?";
                    }
                    if (DXMessageBox.Show(message, "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                DateTime? dateTime = null;
                if(lookupSalesTax.SelectedIndex!=-1)
                {
                    saleOrder.taxNameId = saleOrderTax.Id;
                }
               
                    saleOrder.saleOrderDate = (datsaleOrderdate.Text == "") ? dateTime : datsaleOrderdate.DateTime;
                saleOrder.CreationDate = (datpoCreationdate.Text == "") ? dateTime : datpoCreationdate.DateTime;
                //saleOrder.incoterm_Id = txtIncoTerm.Text.Trim();
                if (cmbPaymentTerm.SelectedItem != null)
                    saleOrder.paymentterm_Id = (cmbPaymentTerm.SelectedItem as cmbitem).id;
                if (cmbIncoterm.SelectedItem != null)
                    saleOrder.incoterm_Id = (cmbIncoterm.SelectedItem as cmbitem).id;

                if (cmbcaption1.SelectedItem != null)
                    saleOrder.TitleValue1Id = (cmbcaption1.SelectedItem as cmbitem).id;
                if (cmbcaption2.SelectedItem != null)
                    saleOrder.TitleValue2Id = (cmbcaption2.SelectedItem as cmbitem).id;
                saleOrder.maker = txtMaker.Text.Trim();
                if (cmbWarranty.SelectedItem != null)
                    saleOrder.WarrantyId = (cmbWarranty.SelectedItem as cmbitem).id;
                saleOrder.origin = txtOrigin.Text.Trim();
                saleOrder.shipmentDate = (datShipmentdate.Text == "") ? dateTime : datShipmentdate.DateTime;
                if (txtSalesTargetYear.Text != "")
                    saleOrder.targetYear = Convert.ToInt32(txtSalesTargetYear.Text.Trim());
                if (txtSalesTargetMonth.Text != "")
                    saleOrder.targetMonth = Convert.ToInt32(txtSalesTargetMonth.Text.Trim());
                saleOrder.deliveryDate = (datDeliverydate.Text == "") ? dateTime : datDeliverydate.DateTime;
                saleOrder.deliveryDateFinal = (datDeliverydateFinal.Text == "") ? dateTime : datDeliverydateFinal.DateTime;
                saleOrder.orderConfirmationDate = (datOrderConfirmationdate.Text == "") ? dateTime : datOrderConfirmationdate.DateTime;
                saleOrder.revisedShipmentDate = (datRevisedShipmentDate.Text == "") ? dateTime : datRevisedShipmentDate.DateTime;
                saleOrder.billLaddingDate = (datBillOfLaddingdate.Text == "") ? dateTime : datBillOfLaddingdate.DateTime;
                saleOrder.materialReciptDate = (datMaterialReciptdate.Text == "") ? dateTime : datMaterialReciptdate.DateTime;
                saleOrder.ExpectedPayment = (datExcpectedPaymentDate.Text == "") ? dateTime : datExcpectedPaymentDate.DateTime;
                saleOrder.OwnDescription = txtOwnDescription.Text;
                saleOrder.FinanceRefrenceNo = txtFinanaceRef.Text;
                saleOrder.commisionRefrenceNo = txtCommissionNumber.Text;
                saleOrder.lCDate = (datLCDatedate.Text == "") ? dateTime : datLCDatedate.DateTime;
                saleOrder.lCnumber = txtLCNumber.Text.Trim();
                saleOrder.SalesReferenceNo = txtSalesref.Text.Trim();
                saleOrder.referenceNo = txtsaleOrderref.Text.Trim();
                saleOrder.offerReferenceNo = txtOfferRefNo.Text.Trim();
                saleOrder.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
                saleOrder.saleOrdertype = (InquiryType)cmbSaleOrderType.SelectedIndex;
                saleOrder.PaymentDueStartDate = (datPaymentDueFrom.Text == "") ? dateTime : datPaymentDueFrom.DateTime;
                saleOrder.PaymentDueAgeing = (datPaymentDueTill.Text == "") ? dateTime : datPaymentDueTill.DateTime;
                saleOrder.CreditDays = Convert.ToInt32(txtPaymentDueDays.Text.Trim());
                saleOrder.packing = txtPacking.Text;
                saleOrder.LCShipmentDate = (datLCShipmentDate.Text == "") ? dateTime : datLCShipmentDate.DateTime;
                saleOrder.LCShipmentAmendmentDate = (datLCRevisedShipmentDate.Text == "") ? dateTime : datLCRevisedShipmentDate.DateTime;
                saleOrder.LCExpiryDate = (datLCExpiryDate.Text == "") ? dateTime : datLCExpiryDate.DateTime;
                saleOrder.LCExpiryAmedmentDate = (datLCRevisedExpiryDate.Text == "") ? dateTime : datLCRevisedExpiryDate.DateTime;
                saleOrder.LCAmedmentNo = txtLCAmedmentno.Text;

                if (cmbTransshipment.SelectedIndex == 0)
                    saleOrder.transshipment = true;
                if (cmbTransshipment.SelectedIndex == 1)
                    saleOrder.transshipment = false;
                saleOrder.PER = (string.IsNullOrEmpty(txtPER.Text.Trim())) ? 0 : Convert.ToDecimal(txtPER.Text.Trim());
                saleOrder.PERValue = (string.IsNullOrEmpty(txtPERAmount.Text.Trim())) ? 0 : Convert.ToDouble(txtPERAmount.Text.Trim());
                saleOrder.SoAmountPER = (string.IsNullOrEmpty(txtSoAmountPer.Text.Trim())) ? 0 : Convert.ToDouble(txtSoAmountPer.Text.Trim());

                saleOrder.PER = (string.IsNullOrEmpty(txtPER.Text.Trim())) ? 0 : Convert.ToDecimal(txtPER.Text.Trim());
                if (!string.IsNullOrEmpty(txttax.Text) && lookupSalesTax.SelectedIndex>-1)
                {
                    saleOrder.totaltaxAmount = Convert.ToDouble(txttax.Text);
                }

                if (txtCommision.Text != "" && txtBaseCommission.Text != "")
                {
                    saleOrder.commision = Convert.ToDecimal(txtCommision.Text.Trim());
                    saleOrder.commisioninBase = Convert.ToDecimal(txtBaseCommission.Text.Trim());
                }
                saleOrder.netCommision = (string.IsNullOrEmpty(txtNetCommision.Text.Trim())) ? 0 : Convert.ToDecimal(txtNetCommision.Text.Trim());

                if (txtBudgetMargin.Text != "" /*&& txtBudgetMargin.Text != "0" && txtBudgetMargin.Text != "0.00"*/)
                {
                    saleOrder.margin = Convert.ToDecimal(txtBudgetMargin.Text.Trim());
                    if(!String.IsNullOrEmpty(txtBaseBudgetMargin.Text))
                        saleOrder.BudgetedMargininBase = Convert.ToDecimal(txtBaseBudgetMargin.Text.Trim());
                    if (!String.IsNullOrEmpty(txtSaleBudgetMargin.Text))
                        saleOrder.SalesBudgetedMargin = Convert.ToDecimal(txtSaleBudgetMargin.Text.Trim());
                }
                if (txtActualMargin.Text != "" /*&& txtActualMargin.Text != "0" && txtActualMargin.Text != "0.00"*/)
                {
                    saleOrder.ActualMargin = Convert.ToDecimal(txtActualMargin.Text.Trim());
                    if (!String.IsNullOrEmpty(txtBaseActualMargin.Text))
                        saleOrder.ActualMargininBase = Convert.ToDecimal(txtBaseActualMargin.Text.Trim());
                    if (!String.IsNullOrEmpty(txtSaleAMargin.Text))
                        saleOrder.SalesActualMargin = Convert.ToDecimal(txtSaleAMargin.Text.Trim());

                }

                //if (txtRevisedMargin.Text != "")
                //{
                //    saleOrder.RevisedMargin = (string.IsNullOrEmpty(txtRevisedMargin.Text.Trim())) ? 0 : Convert.ToDecimal(txtRevisedMargin.Text.Trim());
                //    if (!String.IsNullOrEmpty(txtBaseRevisedMargin.Text))
                //        saleOrder.RevisedMargininBase = (string.IsNullOrEmpty(txtBaseRevisedMargin.Text.Trim())) ? 0 : Convert.ToDecimal(txtBaseRevisedMargin.Text.Trim());
                //    if (!String.IsNullOrEmpty(txtSaleRevisedMargin.Text))
                //        saleOrder.SalesRevisedMargin = (string.IsNullOrEmpty(txtSaleRevisedMargin.Text.Trim())) ? 0 : Convert.ToDecimal(txtSaleRevisedMargin.Text.Trim());

                //}
                if (txtSystemMargin.Text != "")
                {
                    saleOrder.SystemMargin = (string.IsNullOrEmpty(txtSystemMargin.Text.Trim())) ? 0 : Convert.ToDecimal(txtSystemMargin.Text.Trim());
                    if (!String.IsNullOrEmpty(txtMarketSystemMargin.Text))
                        saleOrder.SalesMarketMargin = (string.IsNullOrEmpty(txtMarketSystemMargin.Text.Trim())) ? 0 : Convert.ToDecimal(txtMarketSystemMargin.Text.Trim());
                    if (!String.IsNullOrEmpty(txtSaleSystemMargin.Text))
                        saleOrder.SalesSystemMargin = (string.IsNullOrEmpty(txtSaleSystemMargin.Text.Trim())) ? 0 : Convert.ToDecimal(txtSaleSystemMargin.Text.Trim());

                }
                //saleOrder.RevisedMarginPercent = (string.IsNullOrEmpty(txtRevisedPercent.Text)) ? 0 : Convert.ToDecimal(Regex.Replace(txtRevisedPercent.Text, "[^0-9.]", ""));

                saleOrder.ActualMarginPercent = (string.IsNullOrEmpty(txtActualPercent.Text)) ? 0 : Convert.ToDecimal(Regex.Replace(txtActualPercent.Text, "[^0-9.]", ""));
                saleOrder.BudgetedMarginPercent = (string.IsNullOrEmpty(txtBudgetedPercent.Text)) ? 0 : Convert.ToDecimal(Regex.Replace(txtBudgetedPercent.Text, "[^0-9.]", ""));


                if((InquiryType)cmbSaleOrderType.SelectedIndex==InquiryType.DistributionBiz || (InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    saleOrder.totalCFRValue = Convert.ToDouble(txtSoAfterGST.Text);
                }
                else
                {
                    saleOrder.totalCFRValue = Convert.ToDouble(txttotalcfr.Text);
                }

                saleOrder.RemainingCFRValue = Convert.ToDouble(txtSOremainingcfr.Text);

                saleOrder.SoAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);

                saleOrder.totalFOBValue = Convert.ToDouble(txttotalfob.Text);
                saleOrder.totalBaseCFRValue = Convert.ToDouble(txtBasetotalcfr.Text);
                saleOrder.totalBaseFOBValue = Convert.ToDouble(txtBasetotalfob.Text);
               
                saleOrder.SoAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);

                if (!string.IsNullOrEmpty(txtSalesCommision.Text))
                    saleOrder.totalComissionSER= Convert.ToDouble(txtSalesCommision.Text);
                if (!string.IsNullOrEmpty(txtBaseCommission.Text))
                    saleOrder.totalComissionMER = Convert.ToDouble(txtBaseCommission.Text);
                if (!string.IsNullOrEmpty(txtNetSalesCommision.Text))
                    saleOrder.totalNetComissionSER = Convert.ToDouble(txtNetSalesCommision.Text);
                if (!string.IsNullOrEmpty(txtNetBaseCommission.Text))
                    saleOrder.totalNetComissionMER = Convert.ToDouble(txtNetBaseCommission.Text);




                saleOrder.exchangeRate = (float)Convert.ToDecimal(txtexchangerate.Text.Trim());
                saleOrder.marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text.Trim());
                string str = txttax.Text.Trim();
                if (!string.IsNullOrEmpty(txtWeight.Text))
                {
                    saleOrder.TotalWeight = Convert.ToDecimal(txtWeight.Text);
                }
                else
                {
                    saleOrder.TotalWeight = null;
                }
                if (!string.IsNullOrEmpty(txtQuantity.Text))
                {
                    saleOrder.TotalQuantity = Convert.ToDecimal(txtQuantity.Text);
                }
                else
                {
                    saleOrder.TotalQuantity = null;
                }
                if (str.IndexOf("%") != -1)
                {
                    saleOrder.isPercentTax = true;
                    saleOrder.salesTax = Convert.ToDouble(str.Substring(0, str.IndexOf("%")));
                }
                else
                {
                    saleOrder.salesTax = Convert.ToDouble(str);
                    saleOrder.isPercentTax = false;
                }

                if (offer.Id != 0 && offer != null)
                {
                    saleOrder.offer_Id = offer.Id;
                }
                else
                {
                    saleOrder.offer_Id = null;
                }
                if (ModuleContract.Id != 0 && ModuleContract != null)
                {
                    saleOrder.moduleContract_Id = ModuleContract.Id;
                }

                if ((cmbSaleOrderStatus.SelectedItem as cmbitem) != null)
                {

                    SaleOrderStatus status = saleOrderRepo.getstatus((cmbSaleOrderStatus.SelectedItem as cmbitem).id);
                    saleOrder.saleOrderStatus = status;
                }
                //Selected SaleOrder Status
                if ((cmbVendorPaymentStatus.SelectedItem as cmbitem) != null)
                {

                    saleOrder.vendorPaymentId = (cmbVendorPaymentStatus.SelectedItem as cmbitem).id;

                }


                // Selected Principal 
                if (principal != null)
                {

                    saleOrder.principal_Id = principal.Id;
                }
                // Selected Vendor 
                if (vendor != null)
                {

                    saleOrder.vendors = new List<Vendor>();
                    saleOrder.vendors.Add(saleOrderRepo.getVendor(vendor.Id));
                }
                // selected Department
                if (department != null)
                {

                    saleOrder.dept_Id = department.Id;
                }
                //selected customer
                if (customer != null)
                {

                    saleOrder.customerCompany_Id = customer.Id;
                }
                // selected company
                if (company != null)
                {

                    saleOrder.company_Id = company.Id;
                }
                if (chkInterCompany.IsChecked == true)
                {
                    if (InterCompany != null && InterCompany.Id != 0)
                    {

                        saleOrder.InterCompany_Id = InterCompany.Id;
                    }
                    if (InterDepartment != null && InterDepartment.Id != 0)
                    {

                        saleOrder.InterDepartment_Id = InterDepartment.Id;
                    }
                    saleOrder.isInterCompany = true;

                }
                else
                {
                    saleOrder.isInterCompany = false;
                    saleOrder.InterCompany_Id = null;
                    saleOrder.InterDepartment_Id = null;
                }
                // selected currency
                if (cmbCurrency.SelectedIndex != -1)
                {

                    saleOrder.currency_Id = (cmbCurrency.SelectedItem as cmbitem).id;
                }
                if (cmbCostCenterCurrency.SelectedIndex != -1)
                {
                    saleOrder.costCentercurrency_Id = (cmbCostCenterCurrency.SelectedItem as cmbitem).id;
                    saleOrder.costCenterExchangeRate = Math.Round(Convert.ToDouble(txtCostCenterExchangeRate.Text), 2);
                    saleOrder.costCenterAmount = Math.Round(Convert.ToDouble(txtCostCenterAmount.Text), 2);
                }
                
                if (cmbTransactionHolder.SelectedIndex!=-1)
                {
                    saleOrder.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                    saleOrder.holderChangeDate = (DateTime)datHolderDate.EditValue;
                }
             
              if(!string.IsNullOrEmpty(txtCCSER.Text))
                {
                    saleOrder.ccSER = Convert.ToDouble(txtCCSER.Text);
                }
                if (!string.IsNullOrEmpty(txtCCMER.Text))
                {
                    saleOrder.ccMER = Convert.ToDouble(txtCCMER.Text);
                }
                if ((cmbSaleOrderStatusClass.SelectedItem as cmbitem) != null)
                {

                    ERP_BL.Procurements.StatusClass.StatusClass statusClass = saleOrderRepo.GetStatusClass((cmbSaleOrderStatusClass.SelectedItem as cmbitem).id);
                    saleOrder.statusClass_Id = statusClass.Id;
                }
                var myWindow = Window.GetWindow(this);


                if (editOrder == 1 && OrderId != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Sale Order") != null))
                {
                    if (txtBidBondRefno.Text != "" && txtBidBonValue.Text != "" && txtIssuingbank.Text != "")
                    {
                        if (saleOrder.bid_Id == 0 && saleOrder.bid == null)
                        {
                            Bid bid = new Bid()
                            {
                                issueDate = datBidBondIssuedate.DateTime,
                                refNo = txtBidBondRefno.Text.Trim(),
                                value = txtBidBonValue.Text.Trim(),
                                submitDate = datBidBondSubmitdate.DateTime,
                                expireDate = datBidBondExpirydate.DateTime,
                                bankName = txtIssuingbank.Text.Trim()

                            };
                            saleOrder.bid = bid;
                        }
                        else
                        { //get form data for bid
                            saleOrder.bid.issueDate = datBidBondIssuedate.DateTime;
                            saleOrder.bid.refNo = txtBidBondRefno.Text.Trim();
                            saleOrder.bid.value = txtBidBonValue.Text.Trim();
                            saleOrder.bid.submitDate = datBidBondSubmitdate.DateTime;
                            saleOrder.bid.expireDate = datBidBondExpirydate.DateTime;
                            saleOrder.bid.bankName = txtIssuingbank.Text.Trim();
                        }
                    }
                    if (MainWindow.currentUserid == 0)
                    {

                    }
                    else if (saleOrder.user_Id == null)
                        saleOrder.user_Id = MainWindow.currentUserid;
                    if (saleOrder.CostSheet != null && saleOrder.CostSheet.Timestamp != null)
                    {
                        saleOrder.CostSheet.Timestamp = System.DateTime.Now;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null && saleOrder.isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Sale Order is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            saleOrder.stage = TransactionStage.Approved.ToString();

                            saleOrder.isApproved = true;
                            saleOrder.ApprovedDate = System.DateTime.Now;
                        }
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without ReApproval") != null && saleOrder.isReApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Sale Order is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            saleOrder.stage = TransactionStage.Approved.ToString();

                            saleOrder.isReApproved = true;
                            saleOrder.ReApprovalDate = System.DateTime.Now;
                        }
                    }
                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        if (checkStatus.Id != saleOrder.saleOrderStatus.Id)
                        {
                            saleOrder.LastStatusChangeDate = System.DateTime.Now;
                            if (saleOrder.saleOrderStatus.isActive != true)
                            {
                                saleOrder.ClosingDate = System.DateTime.Now;
                            }
                        }
                    }
                    if (isSalesTax.IsChecked == false)
                    {
                        saleOrder.taxName = null;
                        saleOrder.taxNameId = null;

                    }
                    if (iscopyTemplate == true)
                    {
                        saleOrder.SaleInvoices = null;
                        saleOrder.Bills = null;
                        saleOrder.PurchaseOrders = null;
                        saleOrderRepo.Add(saleOrder);
                    }
                    else
                    {
                        if (checkStatus.Id != saleOrder.saleOrderStatus.Id)
                        {


                            if (saleOrder.saleOrderStatus.isActive == false)
                            {

                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null) ? true : false)
                                {
                                    saleOrder.PendingForClosing = false;
                                    saleOrder.stage = TransactionStage.Approved.ToString();
                                }

                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                {
                                    saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                    if (saleOrder.PendingForClosing == null)
                                    {
                                        saleOrder.PendingForClosing = true;
                                    }
                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                                {
                                    saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (saleOrder.PendingForClosing != true)
                                    {
                                        saleOrder.PendingForClosing = true;
                                    }
                                }
                                else
                                {
                                    saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();
                                    saleOrder.PendingForClosing = true;
                                }

                            }

                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Status of Sale Order has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleOrder.Id, TransactionItemType.Sale_Order);

                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count > 0)
                                    {
                                        if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                        {
                                            saleOrder.holderChangeDate = DateTime.Now;
                                        }
                                        saleOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                        saleOrderRepo.update(saleOrder);
                                    }
                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();
                                }

                            }

                            string oldStat = checkStatus.Status;
                            string newStat = saleOrder.saleOrderStatus.Status;
                         
      
                            string symbolCurr = "";

                            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
                            {
                                if (saleOrder.costcenterCurrency != null)
                                {
                                    symbolCurr = saleOrder.costcenterCurrency.Abbrivation.ToString();
                                }
                            }
                            else
                            {
                                if (saleOrder.currency != null)
                                {
                                    symbolCurr = saleOrder.currency.Abbrivation.ToString();
                                }
                            }
                           
                            CommentLog comment = new CommentLog();
                            if (saleOrder.saleOrdertype == InquiryType.Principal)
                            {
                                comment.Comment = "Status of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                                   + "Comission(OC): " + saleOrder.commision.ToString() + " (" + symbolCurr + ")"
                                                   + "\n Net-Comissio(OC): " + saleOrder.netCommision.ToString() + " (" + symbolCurr + ")"
                                                   + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                                comment.Timestamp = DateTime.Now;
                                comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                if (saleOrder.StatusClass != null)
                                {
                                    string oldStatClass = saleOrder.StatusClass.ClassName;

                                    string newStatClass = checkStatusClass.ClassName;
                                    if (checkStatusClass.Id != saleOrder.StatusClass.Id && saleOrder.StatusClass != null)
                                    {
                                        comment.Comment = "Status and Status Class of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                         + "Comission(OC): " + saleOrder.commision.ToString() + " (" + symbolCurr + ")"
                                         + "\n Net-Comissio(OC): " + saleOrder.netCommision.ToString() + " (" + symbolCurr + ")"
                                         + "\nhas been changed \n"
                                         + "Status From: " + oldStat + " \nTo: " + newStat
                                         + "Status From: " + oldStatClass + " \nTo: " + newStatClass;                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                        comment.Subject = "Status and Status Class Changed";
                                    }
                                }
                            }
                            else
                            {
                                comment.Comment = "Status of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                                + "Budget Margin(OC): " + txtBudgetMargin.Text + " (" + symbolCurr + ")"
                                                + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                                + "\nActual Margin(OC): " + txtActualMargin.Text + " (" + symbolCurr + ")"
                                                + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                                + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                                                    comment.Timestamp = DateTime.Now;
                                                    comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                if (saleOrder.StatusClass != null)
                                {
                                    string oldStatClass = saleOrder.StatusClass.ClassName;

                                    string newStatClass = checkStatusClass.ClassName;
                                    if (checkStatusClass.Id != saleOrder.StatusClass.Id && saleOrder.StatusClass != null)
                                    {
                                        comment.Comment = "Status and Status Class of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                         + "Budget Margin(OC): " + txtBudgetMargin.Text + " (" + symbolCurr + ")"
                                         + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                         + "\nActual Margin(OC): " + txtActualMargin.Text + " (" + symbolCurr + ")"
                                         + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                         + "\nhas been changed \n"
                                         + "Status From: " + oldStat +"\n"  
                                         + "To: " + newStat + "\n"
                                         +"Status Class From: " + oldStatClass + " \n"
                                         +"To: " + newStatClass + "\n";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                        comment.Subject = "Status and Status Class Changed";
                                    }
                                }
                            }
                            procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {

                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {


                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }

                            if (checkStatus != null && checkStatus.Id != 0)
                            {
                                //saleOrderRepo.Add(saleOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                                UsersRepo.Add(TransactionInfo.Status_Changed, saleOrder.Id, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                            }
                            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Sale_Order);
                            inputBox.ShowDialog();
                            PushComment();
                            UsersRepo.Add(TransactionInfo.Edited, saleOrder.Id, 3, frmInputBox.comment);

                        }
                        else
                        if (saleOrder.StatusClass != null)
                        {
                            string oldStatClass = saleOrder.StatusClass.ClassName;

                            string newStatClass = checkStatusClass.ClassName;
                            if (checkStatusClass.Id != saleOrder.StatusClass.Id && saleOrder.StatusClass != null)
                            {
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Status class of Sale Order has been changed, Do you want to notify other users by tagging?)", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res == MessageBoxResult.Yes)
                                {
                                    if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        if (win.tagUsers.Count > 0)
                                        {
                                            if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                            {
                                                saleOrder.holderChangeDate = DateTime.Now;
                                            }
                                            saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                        }
                                    }
                                    else
                                    {
                                        winTagUsers win = new winTagUsers();
                                        win.ShowDialog();
                                    }
                                }
                                string oldStat  = saleOrder.StatusClass.ClassName;

                                string newStat = checkStatusClass.ClassName;
                                string symbolCurr = "";

                                if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
                                {
                                    if (saleOrder.costcenterCurrency != null)
                                    {
                                        symbolCurr = saleOrder.costcenterCurrency.Abbrivation.ToString();
                                    }
                                }
                                else
                                {
                                    if (saleOrder.currency != null)
                                    {
                                        symbolCurr = saleOrder.currency.Abbrivation.ToString();
                                    }
                                }
                                CommentLog comment = new CommentLog();
                                if (saleOrder.saleOrdertype == InquiryType.Principal)
                                {
                                    comment.Comment = "Status Class of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                                       + "Comission(OC): " + saleOrder.commision.ToString() + " (" + symbolCurr + ")"
                                                       + "\n Net-Comissio(OC): " + saleOrder.netCommision.ToString() + " (" + symbolCurr + ")"
                                                       + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                                    comment.Timestamp = DateTime.Now;
                                    comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                }
                                else
                                {

                                    comment.Comment = "Status Class of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                                    + "Budget Margin(OC): " + txtBudgetMargin.Text + " (" + symbolCurr + ")"
                                                    + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                                    + "\nActual Margin(OC): " + txtActualMargin.Text + " (" + symbolCurr + ")"
                                                    + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                                    + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                                    comment.Timestamp = DateTime.Now;
                                    comment.Subject = "Status Class Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                }
                                procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Order #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Order #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                    }

                                }
                                if (checkStatusClass != null && checkStatusClass.Id != 0)
                                {

                                    UsersRepo.Add(TransactionInfo.Status_Class_Changed, saleOrder.Id, 5, "Status Class Changed from (" + saleOrder.StatusClass.ClassName + ") to (" + checkStatusClass.ClassName + ")");
                                }
                                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Sale_Order);
                                inputBox.ShowDialog();
                                PushComment();
                                UsersRepo.Add(TransactionInfo.Edited, saleOrder.Id, 5, frmInputBox.comment);



                            }
                        }

                        saleOrderRepo.update(saleOrder);
                        MessageBox.Show("SaleOrder Updated Succesfully");
                        SystemLog.LogInfo(this.GetType(), "SaleOrder Updated Succesfully refrence No= " + saleOrder.referenceNo + " Id=" + saleOrder.Id);
                    }
                }
                else if (editOrder != 1)
                {
                    if(parentSaleOrder.Id!=0)
                    {
                        saleOrder.ParentSO_Id = parentSaleOrder.Id;
                        if(key.Id!=0)
                        {
                            saleOrder.SaleOrderKey_Id = key.Id;
                        }
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order") != null)
                    {
                        if (MainWindow.currentUserid == 0)
                        {
                            MessageBox.Show("Please Create another Account to Create PO, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                            return;
                        }
                        else
                            saleOrder.user_Id = MainWindow.currentUserid;
                        if (txtBidBondRefno.Text != "" && txtBidBonValue.Text != "" && txtIssuingbank.Text != "")
                        {
                            Bid bid = new Bid()
                            {
                                issueDate = datBidBondIssuedate.DateTime,
                                refNo = txtBidBondRefno.Text.Trim(),
                                value = txtBidBonValue.Text.Trim(),
                                submitDate = datBidBondSubmitdate.DateTime,
                                expireDate = datBidBondExpirydate.DateTime,
                                bankName = txtIssuingbank.Text.Trim()

                            };
                            saleOrder.bid = bid;

                        }
                        else
                            saleOrder.bid_Id = null;
                        if (saleOrder.CostSheet != null)
                        {
                            saleOrder.CostSheet.Timestamp = System.DateTime.Now;
                            saleOrder.CostSheet.TransactionId = saleOrder.Id;
                            saleOrder.CostSheet.TransactionType = 2;

                        }

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null /*&& saleOrder.isApproved == false*/)
                        {

                                saleOrder.stage = TransactionStage.Approved.ToString();
                                saleOrder.isApproved = true;
                                saleOrder.ApprovedDate = System.DateTime.Now;
                        }
                        else
                        {
                            saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                            saleOrder.isApproved = false;
                        }
                        if (isSalesTax.IsChecked == false)
                        {
                            saleOrder.taxName = null;
                            saleOrder.taxNameId = null;
                        }
                        saleOrderRepo.Add(saleOrder);
                        if (Offerss.ucStatuschange.offer != null && Offerss.ucStatuschange.offer.Id != 0)
                        {
                            Offerss.ucStatuschange.offer.LastStatusChangeDate = System.DateTime.Now;
                            Offerss.ucStatuschange.offer.closingDate = System.DateTime.Now;
                            Offerss.ucStatuschange.offerRepo.updateFromGrid(Offerss.ucStatuschange.offer);
                            UsersRepo.Add(TransactionInfo.Closed, Offerss.ucStatuschange.offer.Id, 2, "Closed, Sale Order genrated on this Offer");
                        }
                        UsersRepo.Add(TransactionInfo.Initialized, saleOrder.Id, 3, "");
                        SystemLog.LogInfo(this.GetType(), "SaleOrder Added Succesfully refrence No= " + saleOrder.referenceNo + " Id=" + saleOrder.Id);
                        MessageBox.Show("SaleOrder Added Succesfully");
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                        myWindow.Close();
                        return;
                    }
                }
                myWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Data Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SystemLog.LogError(this.GetType(), "SaleOrder Error refrence No= " + saleOrder.referenceNo + " Id=" + saleOrder.Id + ex.ToString());
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }
        private void loadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbbaseCurrency.ItemsSource= SYSTEM_STATIC.currencySources;
            cmbCostCenterCurrency.ItemsSource= SYSTEM_STATIC.currencySources;
        }
        public void loadVendorPaymentStatus()
        {
            cmbVendorPaymentStatus.ItemsSource = SYSTEM_STATIC.vendorPaymentStatusSource;
        }
        public void loadSaleOrderStatus()
        {
            if (/*SystemLogic.AllowedPermissions.Find(x => x.Name == "View InActive Sale Order Statuses") != null ||*/ SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Order Statuses") != null)
            {
                SaleOrderStatuses = saleOrderRepo.getAllSaleOrderStatus();
            }
            else
                SaleOrderStatuses = saleOrderRepo.getAllActiveSaleOrderStatus();
            SaleOrderStatuses = SaleOrderStatuses.Where(x => x.isDisable != true).ToList();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (SaleOrderStatus status in SaleOrderStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbSaleOrderStatus.ItemsSource = cmbitems;
        }

        public void loadSaleOrderStatus(SaleOrderStatus _saleOrderStatus)
        {
            SaleOrderStatuses.Add(_saleOrderStatus);
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (SaleOrderStatus status in SaleOrderStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbSaleOrderStatus.ItemsSource = cmbitems;
        }
        private void cmbSaleOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbSaleOrderStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbSaleOrderStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Procurementss.SaleOrderss.frmSaleOrderStatusAdd statusAdd = new Procurementss.SaleOrderss.frmSaleOrderStatusAdd();
                    statusAdd.ShowDialog();
                    loadSaleOrderStatus();
                }
                else
                {
                    loadSaleOrderStatusClass(idd);
                }
            }
        }
        private void cmbEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbEmployee.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbEmployee.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Employeess.frmEmployeeAdd employeeadd = new Employeess.frmEmployeeAdd();
                    employeeadd.ShowDialog();
                    loademployees();
                }
            }
        }
        private void lookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            customer = lookupCustomer.SelectedItem as CustomerCompany;
            if (customer != null)
            {
                if (saleOrder.customerCompany_Id != customer.Id)
                {
                    var customerCountry = customer.billingAddres.Country;
                    txtCustomerCountry.Text = customerCountry;
                    var child = saleOrderRepo.getParent(customer.Id);
                    if (child != null)
                    {
                        txtCustomerCountry.Text = "";
                        lookupCustomer.SelectedItem = null;
                        DXMessageBox.Show("Selected Customer is parent please select child to add selected Customer register");
                        lookupDepartment.Focus();
                        return;
                    }
                }
            }
        }
        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company != null)
            {
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == company.CurrencyId)
                    {
                        cmbbaseCurrency.SelectedItem = cmbitem;
                        break;
                    }
                }
                loaddepartments();
                loadBillReferenceNo();
            }
        }
        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                if (saleOrder.dept_Id != department.Id)
                {
                    //var child = saleOrderRepo.getParentDepart(department.Id);
                    if (department.IsParent == true)
                    {
                        lookupDepartment.SelectedItem = null;
                        DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                        lookupDepartment.Focus();
                        return;
                    }
                }
                loadcustomers();
                lookupVendor.ItemsSource = department.Vendors;
                PrincipalRepo principalRepo = new PrincipalRepo();
                lookupPrincipal.ItemsSource = principalRepo.getAllByDept(department.Id);
                ProductRepo productRepo = new ProductRepo();
                var products=productRepo.getAllDepartmentProducts(department.Id);
                lookupProductsinGrid.ItemsSource= products;
                lookupBookerProductsinGrid.ItemsSource = products;
                loademployees();
                if (department.customers.Count == 0)
                {
                    MessageBox.Show("No customer is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.Vendors.Count == 0)
                {
                    MessageBox.Show("No Vendor is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.employees.Count == 0)
                {
                    MessageBox.Show("This department do not have Employees. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.Principals.Count == 0)
                {
                    MessageBox.Show("No Principal is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
            }
        }
        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            Customerss.frmCustomeradd customeradd = new Customerss.frmCustomeradd();
            customeradd.ShowDialog();
            loadcustomers();
        }

        private void btnAddDepartment_Click(object sender, RoutedEventArgs e)
        {
            Companiess.frmcompanyadd frmcompanyadd = new Companiess.frmcompanyadd();
            Companiess.frmcompanyCenter.Editit = 1;
            Companiess.frmcompanyCenter.companyId = company.Id;
            frmcompanyadd.ShowDialog();
            loaddepartments();
        }
        private void btnAddCompany_Click(object sender, RoutedEventArgs e)
        {
            Companiess.frmcompanyadd frmcompanyadd = new Companiess.frmcompanyadd();
            frmcompanyadd.ShowDialog();
            loadcompanies();
        }
        private void btnAddVendor_Click(object sender, RoutedEventArgs e)
        {
            Vendorss.frmVendoradd vendoradd = new Vendorss.frmVendoradd();
            vendoradd.ShowDialog();
            loadvendors();
        }
        private void lookupVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            vendor = lookupVendor.SelectedItem as Vendor;
            if (vendor != null)
            {
                if (datpoCreationdate.EditValue!=null && (DateTime)datpoCreationdate.EditValue > new DateTime(2025, 2, 17) && vendor.isBlackList == true)
                {
                    lookupVendor.SelectedItem = null;
                    return; // or whatever action you need to take
                }
                else
                if (vendor.isBlackList==true)
                {
                    
                    lookupVendor.Background= Brushes.DarkRed;
                    lookupVendor.Foreground= Brushes.White;
                }
                else
                {
                    lookupVendor.Background = Brushes.Transparent;
                    lookupVendor.Foreground = Brushes.Black;
                }
                var vendorCountry = vendor.billingAddres.Country;
                txtVendorCountry.Text = vendorCountry;
            }
        }
        private void cmbCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            if (cmbCurrency.SelectedItem as cmbitem != null)
            {
                int idd = (cmbCurrency.SelectedItem as cmbitem).id;
                currency = currencyRepo.get(idd);
                if (idd == 0)
                {
                    BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                    frmCurrency.ShowDialog();
                    loadCurrencies();
                }
                else
                {
                    string name = (cmbCurrency.SelectedItem as cmbitem).name;
                    string caption1 = "";
                    string caption2 = "";
                    if (cmbcaption1.SelectedItem != null)
                        caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                    if (cmbcaption2.SelectedItem != null)
                        caption2 = (cmbcaption2.SelectedItem as cmbitem).name;

                    symbol = name.Substring(name.IndexOf("("));
                    if (grdSOItems.Columns.Count != 0)
                    {
                        grdSOItems.Columns[6].Header = caption1 + symbol;
                        grdSOItems.Columns[7].Header = caption2 + symbol;
                        lblTotal.Text = "Total " + symbol;
                        lblCFRTotal.Text = "CFR Total " + symbol;
                        string str = lblCommision.Text;
                        if (-1 != str.IndexOf("("))
                            lblCommision.Text = (str.Substring(0, str.IndexOf("("))) + symbol;
                        else
                            lblCommision.Text = lblCommision.Text + " " + symbol;
                    }

                }
            }
        }
        string symbol;
        private static InquiryType PoType;
        private void cmbInquiryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.Supply)
            {
                lblCostCurrency.Visibility = Visibility.Collapsed;
                cmbCostCenterCurrency.Visibility = Visibility.Collapsed;
                lblCostExchangeRate.Visibility = Visibility.Collapsed;
                txtCostCenterExchangeRate.Visibility = Visibility.Collapsed;
                lblCostAmount.Visibility = Visibility.Collapsed;
                txtCostCenterAmount.Visibility = Visibility.Collapsed;
                txtCCMER.Visibility = Visibility.Collapsed;
                txtCCSER.Visibility = Visibility.Collapsed;
                lblCCRates.Visibility = Visibility.Collapsed;
                lblCostSER.Visibility = Visibility.Collapsed;
                lblCostMER.Visibility = Visibility.Collapsed;
                grdPERAmounts.Visibility = Visibility.Visible;
                grpbondinfo.Visibility = Visibility.Visible;
                grpLCInfo.Visibility = Visibility.Visible;
                grpShipmentInfo.Visibility = Visibility.Visible;
                grpComments.Visibility = Visibility.Visible;
                cmbcaption1.Visibility = Visibility.Visible;
                cmbcaption2.Visibility = Visibility.Visible;
                grdDistributionItemGroup.Visibility = Visibility.Collapsed;
                grdProcutementItemGroup.Visibility = Visibility.Visible;
                grdSummDistribution.Visibility = Visibility.Collapsed;
                grdSummProcure.Visibility = Visibility.Visible;
                gridCollection.Visibility = Visibility.Visible;
                grdSplitPER.Visibility = Visibility.Collapsed;
                grdBudgetPunching.Visibility = Visibility.Collapsed;
                btnLinkBudget.Visibility = Visibility.Collapsed;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Visible;
                        grpbondinfo.IsEnabled = false;
                        grpbondinfo.State = GroupBoxState.Minimized;
                        grpCommisiondetails.Visibility = Visibility.Collapsed;
                        grpMargindetails.Visibility = Visibility.Visible;
                        PoType = InquiryType.Supply;
                        txtCommision.Text = "0";
                        txtBaseCommission.Text = "0";
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbSaleOrderType.SelectedItem = PoType;
                }
                else
                {
                    grpbondinfo.IsEnabled = false;
                    grpbondinfo.State = GroupBoxState.Minimized;
                    grpCommisiondetails.Visibility = Visibility.Collapsed;
                    grpMargindetails.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Visible;
                    PoType = InquiryType.Supply;
                    txtCommision.Text = "0";
                    txtBaseCommission.Text = "0";
                    txtIssuingbank.Text = "";
                    txtBidBondRefno.Text = "";
                    txtBidBonValue.Text = "";
                }
                grdSOItems.Columns.GetColumnByFieldName("value1").ReadOnly = false;
                grdSOItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;
                grdSOItems.Columns.GetColumnByFieldName("value2").Visible = true;
            }
            else if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.Tender)
            {
                lblCostCurrency.Visibility = Visibility.Collapsed;
                cmbCostCenterCurrency.Visibility = Visibility.Collapsed;
                lblCostExchangeRate.Visibility = Visibility.Collapsed;
                txtCostCenterExchangeRate.Visibility = Visibility.Collapsed;
                lblCostAmount.Visibility = Visibility.Collapsed;
                txtCostCenterAmount.Visibility = Visibility.Collapsed;

                txtCCMER.Visibility = Visibility.Collapsed;
                txtCCSER.Visibility = Visibility.Collapsed;
                lblCCRates.Visibility = Visibility.Collapsed;
                lblCostSER.Visibility = Visibility.Collapsed;
                lblCostMER.Visibility = Visibility.Collapsed;

                txtCCMER.Visibility = Visibility.Collapsed;
                txtCCSER.Visibility = Visibility.Collapsed;
                lblCCRates.Visibility = Visibility.Collapsed;
                lblCostSER.Visibility = Visibility.Collapsed;
                lblCostMER.Visibility = Visibility.Collapsed;
                grdSplitPER.Visibility = Visibility.Collapsed;
                grpbondinfo.Visibility = Visibility.Visible;
                grpLCInfo.Visibility = Visibility.Visible;
                grpComments.Visibility = Visibility.Visible;
                grpShipmentInfo.Visibility = Visibility.Visible;
                grdSummDistribution.Visibility = Visibility.Collapsed;
                grdSummProcure.Visibility = Visibility.Visible;
                gridCollection.Visibility = Visibility.Visible;
                cmbcaption1.Visibility = Visibility.Visible;
                cmbcaption2.Visibility = Visibility.Visible;
                grdBudgetPunching.Visibility = Visibility.Collapsed;
                btnLinkBudget.Visibility = Visibility.Collapsed;
                if ((txtBudgetMargin.Text != "" || txtActualMargin.Text != "" || txtBaseBudgetMargin.Text != "" || txtBaseActualMargin.Text != "" || txtSaleBudgetMargin.Text != "" || txtSaleAMargin.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        grpCommisiondetails.Visibility = Visibility.Visible;
                        grpMargindetails.Visibility = Visibility.Collapsed;

                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Collapsed;
                        grpbondinfo.IsEnabled = true;
                        txtBudgetMargin.Text = "0";
                        txtBaseBudgetMargin.Text = "0";
                        txtSaleBudgetMargin.Text = "0";
                        txtBudgetMargin.Text = "0";
                        txtBaseActualMargin.Text = "0";
                        txtSaleAMargin.Text = "0";
                    }
                    else
                        cmbSaleOrderType.SelectedItem = PoType;
                }
                else
                {
                    grpCommisiondetails.Visibility = Visibility.Visible;
                    grpMargindetails.Visibility = Visibility.Collapsed;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Collapsed;

                    grpbondinfo.IsEnabled = true;
                    txtBudgetMargin.Text = "0";
                    txtBaseBudgetMargin.Text = "0";
                    txtSaleBudgetMargin.Text = "0";
                    txtBudgetMargin.Text = "0";
                    txtBaseActualMargin.Text = "0";
                    txtSaleAMargin.Text = "0";
                }
                grdSOItems.Columns.GetColumnByFieldName("value1").ReadOnly = false;
                grdSOItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;
                grdSOItems.Columns.GetColumnByFieldName("value2").Visible = true;
            }

            else if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.Principal)
            {
                lblCostCurrency.Visibility = Visibility.Collapsed;
                cmbCostCenterCurrency.Visibility = Visibility.Collapsed;
                lblCostExchangeRate.Visibility = Visibility.Collapsed;
                txtCostCenterExchangeRate.Visibility = Visibility.Collapsed;
                lblCostAmount.Visibility = Visibility.Collapsed;
                txtCostCenterAmount.Visibility = Visibility.Collapsed;
                txtCCMER.Visibility = Visibility.Collapsed;
                txtCCSER.Visibility = Visibility.Collapsed;
                lblCCRates.Visibility = Visibility.Collapsed;
                lblCostSER.Visibility = Visibility.Collapsed;
                lblCostMER.Visibility = Visibility.Collapsed;
                grdPERAmounts.Visibility = Visibility.Visible;
                grpbondinfo.Visibility = Visibility.Visible;
                grpLCInfo.Visibility = Visibility.Visible;
                grpShipmentInfo.Visibility = Visibility.Visible;
                grpComments.Visibility = Visibility.Visible;
                grdSplitPER.Visibility = Visibility.Visible;
                grdDistributionItemGroup.Visibility = Visibility.Collapsed;
                grdProcutementItemGroup.Visibility = Visibility.Visible;
                grdSummDistribution.Visibility = Visibility.Collapsed;
                grdSummProcure.Visibility = Visibility.Visible;
                gridCollection.Visibility = Visibility.Visible;
                cmbcaption1.Visibility = Visibility.Visible;
                cmbcaption2.Visibility = Visibility.Visible;
                grdBudgetPunching.Visibility = Visibility.Collapsed;
                btnLinkBudget.Visibility = Visibility.Collapsed;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {

                        grpCommisiondetails.Visibility = Visibility.Visible;
                        grpMargindetails.Visibility = Visibility.Collapsed;
                        btnSummarySheet.Visibility = Visibility.Visible;
                        btnCostSheet.Visibility = Visibility.Collapsed;
                        grpbondinfo.IsEnabled = true;
                        txtBudgetMargin.Text = "0";
                        txtBaseBudgetMargin.Text = "0";
                        txtSaleBudgetMargin.Text = "0";
                        txtBudgetMargin.Text = "0";
                        txtBaseActualMargin.Text = "0";
                        txtSaleAMargin.Text = "0";

                    }

                    else
                        cmbSaleOrderType.SelectedItem = PoType;

                }
                else
                {
                    grpCommisiondetails.Visibility = Visibility.Visible;
                    grpMargindetails.Visibility = Visibility.Collapsed;
                    btnSummarySheet.Visibility = Visibility.Visible;
                    btnCostSheet.Visibility = Visibility.Collapsed;
                    grpbondinfo.IsEnabled = true;
                    txtBudgetMargin.Text = "0";
                    txtBaseBudgetMargin.Text = "0";
                    txtSaleBudgetMargin.Text = "0";
                    txtBudgetMargin.Text = "0";
                    txtBaseActualMargin.Text = "0";
                    txtSaleAMargin.Text = "0";
                }
                grdSOItems.Columns.GetColumnByFieldName("value1").ReadOnly = false;
                grdSOItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;
                grdSOItems.Columns.GetColumnByFieldName("value2").Visible = true;
            }
            else
            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.Inventory)
            {
                lblCostCurrency.Visibility = Visibility.Collapsed;
                cmbCostCenterCurrency.Visibility = Visibility.Collapsed;
                lblCostExchangeRate.Visibility = Visibility.Collapsed;
                txtCostCenterExchangeRate.Visibility = Visibility.Collapsed;
                lblCostAmount.Visibility = Visibility.Collapsed;
                txtCostCenterAmount.Visibility = Visibility.Collapsed;

                txtCCMER.Visibility = Visibility.Collapsed;
                txtCCSER.Visibility = Visibility.Collapsed;
                lblCCRates.Visibility = Visibility.Collapsed;
                lblCostSER.Visibility = Visibility.Collapsed;
                lblCostMER.Visibility = Visibility.Collapsed;
                grdPERAmounts.Visibility = Visibility.Visible;
                grpbondinfo.Visibility = Visibility.Visible;
                grpLCInfo.Visibility = Visibility.Visible;
                grpShipmentInfo.Visibility = Visibility.Visible;
                grpComments.Visibility = Visibility.Visible;
                grdDistributionItemGroup.Visibility = Visibility.Collapsed;
                grdProcutementItemGroup.Visibility = Visibility.Visible;
                grdSummProcure.Visibility = Visibility.Visible;
                grdSummDistribution.Visibility = Visibility.Collapsed;
                grdSplitPER.Visibility = Visibility.Collapsed;
                cmbcaption1.Visibility = Visibility.Visible;
                cmbcaption2.Visibility = Visibility.Visible;
                grdSummDistribution.Visibility = Visibility.Collapsed;
                grdSummProcure.Visibility = Visibility.Visible;
                gridCollection.Visibility = Visibility.Visible;
                grdBudgetPunching.Visibility = Visibility.Collapsed;
                btnLinkBudget.Visibility = Visibility.Collapsed;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Visible;
                        grpbondinfo.IsEnabled = false;
                        grpbondinfo.State = GroupBoxState.Minimized;
                        grpCommisiondetails.Visibility = Visibility.Collapsed;
                        grpMargindetails.Visibility = Visibility.Visible;
                        txtCommision.Text = "0";
                        txtBaseCommission.Text = "0";
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbSaleOrderType.SelectedItem = PoType;

                }
                else
                {
                    grpbondinfo.IsEnabled = false;
                    grpbondinfo.State = GroupBoxState.Minimized;
                    grpCommisiondetails.Visibility = Visibility.Collapsed;
                    grpMargindetails.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Visible;
                    txtCommision.Text = "0";
                    txtBaseCommission.Text = "0";
                    txtIssuingbank.Text = "";
                    txtBidBondRefno.Text = "";
                    txtBidBonValue.Text = "";
                }
                PoType = InquiryType.Inventory;
                grdSOItems.Columns.GetColumnByFieldName("value2").Visible = false;


            }
            else
            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz)
            {
                lblCostCurrency.Visibility = Visibility.Collapsed;
                cmbCostCenterCurrency.Visibility = Visibility.Collapsed;
                lblCostExchangeRate.Visibility = Visibility.Collapsed;
                txtCostCenterExchangeRate.Visibility = Visibility.Collapsed;
                lblCostAmount.Visibility = Visibility.Collapsed;
                txtCostCenterAmount.Visibility = Visibility.Collapsed;
                txtCCMER.Visibility = Visibility.Collapsed;
                txtCCSER.Visibility = Visibility.Collapsed;
                lblCCRates.Visibility = Visibility.Collapsed;
                lblCostSER.Visibility = Visibility.Collapsed;
                lblCostMER.Visibility = Visibility.Collapsed;
                grdPERAmounts.Visibility = Visibility.Collapsed;
                grpLCInfo.Visibility = Visibility.Collapsed;
                grpShipmentInfo.Visibility = Visibility.Collapsed;
                grpbondinfo.Visibility = Visibility.Visible;
                grpComments.Visibility = Visibility.Collapsed;
                grdDistributionItemGroup.Visibility = Visibility.Visible;
                grdProcutementItemGroup.Visibility = Visibility.Collapsed;
                grdSummProcure.Visibility = Visibility.Collapsed;
                grdSummDistribution.Visibility = Visibility.Visible;
                grdSplitPER.Visibility = Visibility.Collapsed;
                cmbcaption1.Visibility = Visibility.Collapsed;
                cmbcaption2.Visibility = Visibility.Collapsed;
                grdSummDistribution.Visibility = Visibility.Visible;
                grdSummProcure.Visibility = Visibility.Collapsed;
                gridCollection.Visibility = Visibility.Visible;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can revised budget from SO") != null)
                {
                    grdBudgetPunching.Visibility = Visibility.Visible;
                }
                else
                {
                    grdBudgetPunching.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Link SO with Budget") != null)
                {
                    btnLinkBudget.Visibility = Visibility.Visible;
                }
                else
                {
                    btnLinkBudget.Visibility = Visibility.Collapsed;
                }

                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Visible;
                        grpbondinfo.IsEnabled = false;
                        grpbondinfo.State = GroupBoxState.Minimized;
                        grpCommisiondetails.Visibility = Visibility.Collapsed;
                        grpMargindetails.Visibility = Visibility.Visible;
                        txtCommision.Text = "0";
                        txtBaseCommission.Text = "0";
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbSaleOrderType.SelectedItem = PoType;

                }
                else
                {
                    grpbondinfo.IsEnabled = false;
                    grpbondinfo.State = GroupBoxState.Minimized;
                    grpCommisiondetails.Visibility = Visibility.Collapsed;
                    grpMargindetails.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Visible;
                    txtCommision.Text = "0";
                    txtBaseCommission.Text = "0";
                    txtIssuingbank.Text = "";
                    txtBidBondRefno.Text = "";
                    txtBidBonValue.Text = "";
                }
                PoType = InquiryType.DistributionBiz;
                grdSOItems.Columns.GetColumnByFieldName("value2").Visible = false;
            }
            else
            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {
                lblCostCurrency.Visibility = Visibility.Collapsed;
                cmbCostCenterCurrency.Visibility = Visibility.Collapsed;
                lblCostExchangeRate.Visibility = Visibility.Collapsed;
                txtCostCenterExchangeRate.Visibility = Visibility.Collapsed;
                lblCostAmount.Visibility = Visibility.Collapsed;
                txtCostCenterAmount.Visibility = Visibility.Collapsed;
                txtCCMER.Visibility = Visibility.Collapsed;
                txtCCSER.Visibility = Visibility.Collapsed;
                lblCCRates.Visibility = Visibility.Collapsed;
                lblCostSER.Visibility = Visibility.Collapsed;
                lblCostMER.Visibility = Visibility.Collapsed;
                grdPERAmounts.Visibility = Visibility.Collapsed;
                grpLCInfo.Visibility = Visibility.Collapsed;
                grpShipmentInfo.Visibility = Visibility.Collapsed;
                grpbondinfo.Visibility = Visibility.Visible;
                grpComments.Visibility = Visibility.Collapsed;
                grdDistributionItemGroup.Visibility = Visibility.Visible;
                grdProcutementItemGroup.Visibility = Visibility.Collapsed;
                grdSummProcure.Visibility = Visibility.Collapsed;
                grdSummDistribution.Visibility = Visibility.Visible;
                grdSplitPER.Visibility = Visibility.Collapsed;
                cmbcaption1.Visibility = Visibility.Collapsed;
                cmbcaption2.Visibility = Visibility.Collapsed;
                grdSummDistribution.Visibility = Visibility.Visible;
                grdSummProcure.Visibility = Visibility.Collapsed;
                gridCollection.Visibility = Visibility.Visible;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can revised budget from SO") != null)
                {
                    grdBudgetPunching.Visibility = Visibility.Visible;
                }
                else
                {
                    grdBudgetPunching.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Link SO with Budget") != null)
                {
                    btnLinkBudget.Visibility = Visibility.Visible;
                }
                else
                {
                    btnLinkBudget.Visibility = Visibility.Collapsed;
                }

                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Visible;
                        grpbondinfo.IsEnabled = false;
                        grpbondinfo.State = GroupBoxState.Minimized;
                        grpCommisiondetails.Visibility = Visibility.Collapsed;
                        grpMargindetails.Visibility = Visibility.Visible;
                        txtCommision.Text = "0";
                        txtBaseCommission.Text = "0";
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbSaleOrderType.SelectedItem = PoType;

                }
                else
                {
                    grpbondinfo.IsEnabled = false;
                    grpbondinfo.State = GroupBoxState.Minimized;
                    grpCommisiondetails.Visibility = Visibility.Collapsed;
                    grpMargindetails.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Visible;
                    txtCommision.Text = "0";
                    txtBaseCommission.Text = "0";
                    txtIssuingbank.Text = "";
                    txtBidBondRefno.Text = "";
                    txtBidBonValue.Text = "";
                }
                PoType = InquiryType.DistributionBiz_CustomerCredit;
                grdSOItems.Columns.GetColumnByFieldName("value2").Visible = false;
            }
            else
            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
            {

                lblCostCurrency.Visibility = Visibility.Visible;
                cmbCostCenterCurrency.Visibility = Visibility.Visible;
                lblCostExchangeRate.Visibility = Visibility.Visible;
                txtCostCenterExchangeRate.Visibility = Visibility.Visible;
                lblCostAmount.Visibility = Visibility.Visible;
                txtCostCenterAmount.Visibility = Visibility.Visible;
                txtCCMER.Visibility = Visibility.Visible;
                txtCCSER.Visibility = Visibility.Visible;
                lblCCRates.Visibility = Visibility.Visible;
                lblCostSER.Visibility = Visibility.Visible;
                lblCostMER.Visibility = Visibility.Visible;
                grdPERAmounts.Visibility = Visibility.Visible;
                grpbondinfo.Visibility = Visibility.Visible;
                grpLCInfo.Visibility = Visibility.Visible;
                grpShipmentInfo.Visibility = Visibility.Visible;
                grpComments.Visibility = Visibility.Visible;
                cmbcaption1.Visibility = Visibility.Visible;
                cmbcaption2.Visibility = Visibility.Visible;
                grdDistributionItemGroup.Visibility = Visibility.Collapsed;
                grdProcutementItemGroup.Visibility = Visibility.Visible;
                grdSummDistribution.Visibility = Visibility.Collapsed;
                grdSummProcure.Visibility = Visibility.Visible;
                gridCollection.Visibility = Visibility.Visible;
                grdSplitPER.Visibility = Visibility.Collapsed;
                grdBudgetPunching.Visibility = Visibility.Collapsed;
                btnLinkBudget.Visibility = Visibility.Collapsed;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Visible;
                        grpbondinfo.IsEnabled = false;
                        grpbondinfo.State = GroupBoxState.Minimized;
                        grpCommisiondetails.Visibility = Visibility.Collapsed;
                        grpMargindetails.Visibility = Visibility.Visible;
                        PoType = InquiryType.Supply;
                        txtCommision.Text = "0";
                        txtBaseCommission.Text = "0";
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbSaleOrderType.SelectedItem = PoType;
                }
                else
                {
                    grpbondinfo.IsEnabled = false;
                    grpbondinfo.State = GroupBoxState.Minimized;
                    grpCommisiondetails.Visibility = Visibility.Collapsed;
                    grpMargindetails.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Visible;
                    PoType = InquiryType.Supply;
                    txtCommision.Text = "0";
                    txtBaseCommission.Text = "0";
                    txtIssuingbank.Text = "";
                    txtBidBondRefno.Text = "";
                    txtBidBonValue.Text = "";
                }
                grdSOItems.Columns.GetColumnByFieldName("value1").ReadOnly = false;
                grdSOItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;
                grdSOItems.Columns.GetColumnByFieldName("value2").Visible = true;
            }
            }
        private void txttax_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txttax.Text != "")
            {
                calculatetotal();
            }
        }
        private void calculatetotal()
        {
            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {
               
                double soWeight = 0;
                double soQuantity = 0;
                double soAmount = 0;
                double soPassOnValue = 0;
                double soClaimDiscount = 0;
                double soFOCValue = 0;
                double totalGSTAmount = 0;
                double soNetAmount = 0;


                if (grdBokkerItems.ItemsSource != null)
                {
                    List<BookerStatementItem> source = grdBokkerItems.ItemsSource as List<BookerStatementItem>;
                    if(source!=null)
                    foreach (var item in source)
                    {
                        soWeight += item.weight;
                        soQuantity += item.quantity;
                        soAmount += item.amount;
                        soPassOnValue += item.passOnValue;
                        soClaimDiscount += item.claimDiscountValue;
                        soFOCValue += item.focValue;
                        totalGSTAmount += item.amountGST;
                        soNetAmount += item.netAmount;

                    }
                    txtTotalSOAmount.Text = soAmount.ToString();
                    txtSoAmount.Text = (totalGSTAmount+soAmount).ToString();
                    txtCostCenterAmount.Text= ((totalGSTAmount + soAmount)* Convert.ToDouble(txtCostCenterExchangeRate.Text)).ToString();
                    txtSalestotalCfr.Text = (Convert.ToDouble(txtMarginexchangerate.Text) * Convert.ToDouble(txtSoAmount.Text)).ToString();
                    txtBasetotalcfr.Text= (Convert.ToDouble(txtexchangerate.Text) * Convert.ToDouble(txtSoAmount.Text)).ToString();
                    txtQuantity.Text = soQuantity.ToString();
                    txtWeight.Text = soWeight.ToString();
                    txtTotalPassOnValue.Text = soPassOnValue.ToString();
                    txtGstAmount.Text = totalGSTAmount.ToString();
                    txtSoAfterGST.Text = (soAmount + totalGSTAmount).ToString();
                    txtGstAmount.Text = totalGSTAmount.ToString();
                    txtTotalClaimDiscount.Text = soClaimDiscount.ToString();
                    txtTotalFOCValue.Text = soFOCValue.ToString();
                    txtTotalNetAmount.Text = soNetAmount.ToString();
                }
            }
            else
            {
                double sumfob = 0;
                double sumcfr = 0;
                decimal? weight = 0;
                double Quantity = 0;

                if (grdSOItems.ItemsSource != null)
                    foreach (var item in grdSOItems.ItemsSource as List<ProcurementProduct>)
                    {

                        {
                            if (item.inquiryProduct.quantity != 0)
                            {
                                weight += (item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0) * Convert.ToDecimal(item.inquiryProduct.quantity);
                            }
                            else
                            {
                                weight += item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0;
                            }

                            Quantity += item.inquiryProduct.quantity;
                            sumfob += item.value1;
                            sumcfr += item.value2;


                        }
                    }
                txtWeight.Text = weight.ToString();
                txtQuantity.Text = Quantity.ToString();
                txtfob.Text = sumfob.ToString();
                txtcfr.Text = sumcfr.ToString();
                decimal fob, cfr, tax = 0;
                fob = Convert.ToDecimal(txtfob.Text);
                cfr = Convert.ToDecimal(txtcfr.Text);
                string str = txttax.Text.ToString();

                if (txttax.Text != null && txttax.Text != "")
                {
                    if (str.IndexOf("%") == -1)
                        tax = Convert.ToDecimal(str);
                    else
                        tax = Convert.ToDecimal(str.Substring(0, str.IndexOf("%")));
                }
                decimal exchangeRate = 1;
                if (txtexchangerate.Text != "")
                    exchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                if (str.IndexOf("%") != -1)
                {
                    txttotalfob.Text = (fob + (fob * tax / 100)).ToString();
                    txttotalcfr.Text = (cfr + (cfr * tax / 100)).ToString();
                    txtBasetotalcfr.Text = ((cfr * exchangeRate) + (cfr * tax / 100)).ToString();
                    txtBasetotalfob.Text = ((fob * exchangeRate) + (fob * tax / 100)).ToString();
                }
                else
                {
                    txttotalfob.Text = Math.Round((fob + tax), 2).ToString();
                    txttotalcfr.Text = Math.Round((cfr + tax), 2).ToString();
                    txtBasetotalfob.Text = Math.Round(((fob * exchangeRate) + tax), 2).ToString();
                    txtBasetotalcfr.Text = Math.Round(((cfr * exchangeRate) + tax), 2).ToString();
                }
                var remain = Math.Round(Convert.ToDouble(txtSOremainingcfr.Text), 2);

                if (cmbSaleOrderType.SelectedItem != null)
                    if (cmbSaleOrderType.SelectedItem.ToString() == InquiryType.Principal.ToString())
                    {
                        if (!string.IsNullOrEmpty(txtCommision.Text))
                        {
                            var comm = Math.Round(Convert.ToDouble(txtCommision.Text) - Convert.ToDouble(/*Math.Round(*/SOCFRRemaining/*, 2)*/) +/* Math.Round(*/remain/*)*/, 2);
       
                            SOCFRRemaining = Convert.ToDouble(txtCommision.Text);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txttotalcfr.Text))
                        {
                            var remaining = Math.Round(Convert.ToDouble(txttotalcfr.Text) - Convert.ToDouble(/*Math.Round(*/SOCFRRemaining/*, 2)*/) + remain, 2).ToString();
                            SOCFRRemaining = Convert.ToDouble(txttotalcfr.Text);
                        }
                    }
                decimal marginexchangeRate = 1;
                decimal totalcfr = Convert.ToDecimal(txttotalcfr.Text);
                if (txtMarginexchangerate.Text != "")
                {
                    marginexchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtSalestotalCfr.Text = (marginexchangeRate * totalcfr).ToString();

                    txtBasetotalcfr.Text = (exchangeRate * totalcfr).ToString();
                }
            }
        }
        private void dGitems_CurrentCellChanged(object sender, EventArgs e)
        {
            calculatetotal();
        }

        private void cmbPaymentTerm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbPaymentTerm.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbPaymentTerm.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Termss.frmPaymentTermAdd paymentTerms = new Termss.frmPaymentTermAdd();
                    paymentTerms.ShowDialog();
                    loadPaymentTerms();
                }
            }
        }
        private void cmbIncoterm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbIncoterm.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbIncoterm.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Termss.frmIncotermAdd incoterms = new Termss.frmIncotermAdd();
                    incoterms.ShowDialog();
                    loadIncoterms();

                }
            }
        }
        public void loadIncoterms()
        {
            cmbIncoterm.ItemsSource = SYSTEM_STATIC.incoTermSource;
            cmbcaption1.ItemsSource = SYSTEM_STATIC.incoTermSource;
            cmbcaption2.ItemsSource = SYSTEM_STATIC.incoTermSource;
        }
        public void loadWarrantys()
        {
            cmbWarranty.ItemsSource = SYSTEM_STATIC.warrantySource;
        }
        public void loadPaymentTerms()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            try
            {
                PaymentTermRepo TermRepo = new PaymentTermRepo();
                List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
                paymentTerms = TermRepo.getAllForSO();


                foreach (var paymentTerm in paymentTerms)
                {
                    cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

                cmbPaymentTerm.ItemsSource = cmbitems;
            }
            catch (Exception ex) { }
        }

        private void txttax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9|%]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void cmbcaption1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdSOItems != null && grdSOItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdSOItems.Columns.GetColumnByFieldName("value1").Header = "SO Value" + symbol;
        }
        private void cmbcaption2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (grdSOItems != null && grdSOItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdSOItems.Columns.GetColumnByFieldName("value2").Header = "PO Value" + symbol;
        }
        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (company.Id == 0)
            {
                MessageBox.Show("Select Company Frist");
                lookupCompany.Focus();
                return;
            }
        }
        private void cmbEmployee_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                MessageBox.Show("Select Department Frist");
                lookupDepartment.Focus();
                return;
            }
        }
        private void lookupCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                MessageBox.Show("Select Department Frist");
                return;
            }
        }
        private void lookupPrincipal_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            principal = lookupPrincipal.SelectedItem as Principal;
        }

        private void btnAddPrincipal_Click(object sender, RoutedEventArgs e)
        {

            Principalss.frmPrincipaladd Principaladd = new Principalss.frmPrincipaladd();
            Principaladd.ShowDialog();
            loadPrincipals();

        }
        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {
                var row = e.Row as BookerStatementItem;
                row.amount = row.unit *row.quantity;
                row.netAmount = row.amount+  row.amountGST - row.passOnValue - row.claimDiscountValue - row.focValue;
                calculatetotal();
            }
            else
            {
                if (cmbSaleOrderType.SelectedItem != null)
                    if (cmbSaleOrderType.SelectedItem.ToString() == InquiryType.Inventory.ToString())
                    {
                        var row = e.Row as ProcurementProduct;
                        var colum = e.Column;

                        if (row != null && colum.FieldName != "value1" && colum.FieldName != "value2" && colum.FieldName != "value3")
                        {

                            row.value1 = row.inquiryProduct.quantity * row.unitPrice;
                            row.value2 = row.inquiryProduct.quantity * row.unitPrice;
                            row.value3 = row.inquiryProduct.quantity * row.unitPrice;
                        }
                        else
                        if (colum.FieldName == "value1")
                        {
                            if (row.value1 != 0)
                            {
                                if (row.inquiryProduct.quantity != 0)
                                    row.unitPrice = row.value1 / row.inquiryProduct.quantity;
                            }
                        }
                    }
                calculatetotal();
            }
        }
        private void datBillOfLaddingdate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
        }
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Items") != null)
            {
                Productss.frmItemadd frmItemadd = new Productss.frmItemadd();
                frmItemadd.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("Permission required (Add New Items) to add new item!");
            }


        }
        private void view_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            if ((InquiryType)cmbSaleOrderType.SelectedIndex != InquiryType.DistributionBiz || (InquiryType)cmbSaleOrderType.SelectedIndex != InquiryType.DistributionBiz_CustomerCredit)
            {
                (grdSOItems.CurrentItem as ProcurementProduct).inquiryProduct = new InquiryProduct();
            }
        }
        private void winSaleOrderadd_Unloaded(object sender, RoutedEventArgs e)
        {
            editsaleOrder = 0;
            offerid = 0;
            saleOrderid = 0;
            if (saleOrder != null && saleOrder.Id != 0)
                UsersRepo.Add(TransactionInfo.viewed, saleOrder.Id, 3, "Viewed details of Sale Order");
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdSOItems);
            frmCostSheet.costSheet = new ERP_BL.Databases.CostSheet();
        }

        private void CmbVendorPaymentStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbVendorPaymentStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbVendorPaymentStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Vendorss.frmVendorPaymentStatusAdd VendorPaymentStatuss = new Vendorss.frmVendorPaymentStatusAdd();
                    VendorPaymentStatuss.ShowDialog();
                    loadVendorPaymentStatus();
                }
            }
        }

        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
         {
            CurrencyRepo currencyRepo = new CurrencyRepo();
            DateTime creationDate = (DateTime)datpoCreationdate.EditValue;
            DateTime d1 = new DateTime(2016, 01, 01);
            ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;

            if (cmbbaseCurrency.SelectedItem != null && cmbCurrency.SelectedItem != null)
            {
                if ((cmbCurrency.SelectedItem as cmbitem).id == (cmbbaseCurrency.SelectedItem as cmbitem).id)
                {
                    txtexchangerate.Text = 1.ToString();
                    txtMarginexchangerate.Text = 1.ToString();

                }
                else
                {

                    if (creationDate > d1)
                    {
                        if (cmbCurrency.SelectedIndex != -1 && cmbbaseCurrency.SelectedIndex != -1)
                        {
                            var exchangeRateGroupSER = exchangeRateGroupRepo.GetGroupByCurrenciesSER((cmbCurrency.SelectedItem as cmbitem).id, (cmbbaseCurrency.SelectedItem as cmbitem).id, creationDate.Year);
                            var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbCurrency.SelectedItem as cmbitem).id, (cmbbaseCurrency.SelectedItem as cmbitem).id, creationDate.Year);
                            if (exchangeRateGroupSER != null)
                            {
                                exchangeRate = exchangeRateGroupSER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                                switch (creationDate.Month)
                                {
                                    case 1:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateJan.ToString();
                                        break;
                                    case 2:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateFeb.ToString();
                                        break;
                                    case 3:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateMar.ToString();
                                        break;
                                    case 4:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateApr.ToString();
                                        break;
                                    case 5:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateMay.ToString();
                                        break;
                                    case 6:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateJun.ToString();
                                        break;
                                    case 7:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateJul.ToString();
                                        break;
                                    case 8:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateAug.ToString();
                                        break;
                                    case 9:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateSep.ToString();
                                        break;
                                    case 10:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateOct.ToString();
                                        break;
                                    case 11:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateNov.ToString();
                                        break;
                                    case 12:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = exchangeRate.rateDec.ToString();
                                        break;
                                    default:
                                        if (exchangeRate != null)
                                            txtMarginexchangerate.Text = 0.ToString();
                                        break;
                                }
                                //exchangeRateGroup.exchangeRates.Where(x=>x.)
                            }
                            else
                            {
                                txtMarginexchangerate.Text = 1.ToString();

                            }
                            if (exchangeRateGroupMER != null)
                            {
                                exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                                switch (creationDate.Month)
                                {
                                    case 1:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateJan.ToString();
                                        break;
                                    case 2:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateFeb.ToString();
                                        break;
                                    case 3:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateMar.ToString();
                                        break;
                                    case 4:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateApr.ToString();
                                        break;
                                    case 5:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateMay.ToString();
                                        break;
                                    case 6:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateJun.ToString();
                                        break;
                                    case 7:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateJul.ToString();
                                        break;
                                    case 8:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateAug.ToString();
                                        break;
                                    case 9:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateSep.ToString();
                                        break;
                                    case 10:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateOct.ToString();
                                        break;
                                    case 11:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateNov.ToString();
                                        break;
                                    case 12:
                                        if (exchangeRate != null)
                                            txtexchangerate.Text = exchangeRate.rateDec.ToString();
                                        break;

                                }
                            }
                            else
                            {
                                txtexchangerate.Text = 1.ToString();
                            }
                        }
                        calculateBaseCommision();
                        calculateBaseBudgetMargin();

                        calculateBaseActualMargin();
                        calculateBaseRevisedMargin();
                        calculatetotal();
                    }
                    else
                    {
                        ///Working One
                        if (cmbCurrency.SelectedItem != null)
                        {
                            MarketExchangeRate marketyExchangeRate = new MarketExchangeRate();
                            SalesExchangeRate salesExchangeRate = new SalesExchangeRate();
                            marketyExchangeRate = currencyRepo.getMarketexchangerate((int)saleOrder.company_Id, (cmbCurrency.SelectedItem as cmbitem).id);
                            salesExchangeRate = currencyRepo.getsalesexchangerate((int)saleOrder.company_Id, (cmbCurrency.SelectedItem as cmbitem).id);
                            if (marketyExchangeRate != null && salesExchangeRate != null)
                            {

                                if (marketyExchangeRate != null)
                                {
                                    txtexchangerate.Text = marketyExchangeRate.exchangerate.ToString();
                                }
                                if (salesExchangeRate != null)
                                {
                                    txtMarginexchangerate.Text = salesExchangeRate.exchangerate.ToString();
                                }
                                else if (department?.Id != 0 && department?.applyMERasSER != false && salesExchangeRate != null)
                                {
                                    txtMarginexchangerate.Text = salesExchangeRate.exchangerate.ToString();
                                }
                                calculateBaseCommision();
                                calculateBaseBudgetMargin();
                                calculateBaseActualMargin();
                                calculateBaseRevisedMargin();
                                calculatetotal();
                            }
                            else
                            {

                            }
                        }
                        if (cmbCurrency.SelectedItem as cmbitem != null)
                        {
                            int idd = (cmbCurrency.SelectedItem as cmbitem).id;
                            currency = currencyRepo.get(idd);
                            if (idd == 0)
                            {
                                BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                                frmCurrency.ShowDialog();
                                loadCurrencies();
                            }
                            else
                            {
                                string name = (cmbCurrency.SelectedItem as cmbitem).name;
                                string caption1 = "";
                                string caption2 = "";
                                if (cmbcaption1.SelectedItem != null)
                                    caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                                if (cmbcaption2.SelectedItem != null)
                                    caption2 = (cmbcaption2.SelectedItem as cmbitem).name;

                                symbol = name.Substring(name.IndexOf("("));
                                if (grdSOItems.Columns.Count != 0)
                                {
                                    grdSOItems.Columns.GetColumnByFieldName("value1").Header = caption1 + symbol;
                                    grdSOItems.Columns.GetColumnByFieldName("value2").Header = caption2 + symbol;
                                    lblTotal.Text = "S.O Amount" + symbol;
                                    lblCFRTotal.Text = "S.O Amount" + symbol;
                                    string str = lblCommision.Text;
                                    if (-1 != str.IndexOf("("))
                                        lblCommision.Text = (str.Substring(0, str.IndexOf("("))) + symbol;
                                    else
                                        lblCommision.Text = lblCommision.Text + " " + symbol;
                                    string stri = lblActualMargin.Text;
                                    if (-1 != stri.IndexOf("("))
                                        lblActualMargin.Text = (stri.Substring(0, stri.IndexOf("("))) + symbol;
                                    else
                                        lblActualMargin.Text = lblActualMargin.Text + " " + symbol;
                                    string stringg = lblBudgetdMargin.Text;
                                    if (-1 != stringg.IndexOf("("))
                                        lblBudgetdMargin.Text = (stringg.Substring(0, stringg.IndexOf("("))) + symbol;
                                    else
                                        lblBudgetdMargin.Text = lblBudgetdMargin.Text + " " + symbol;


                                    string stri1 = lblbaseTotal.Text;
                                    if (-1 != stri1.IndexOf("("))
                                        lblbaseTotal.Text = (stri1.Substring(0, stri1.IndexOf("("))) + symbol;
                                    else
                                        lblbaseTotal.Text = lblbaseTotal.Text + " " + symbol;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CmbbaseCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime creationDate = (DateTime)datpoCreationdate.EditValue;
            DateTime d1 = new DateTime(2016, 01, 01);
            ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;
            if (creationDate > d1)
            {
                if (cmbbaseCurrency.SelectedIndex != -1 && cmbCurrency.SelectedIndex != -1)
                {
                    var exchangeRateGroupSER = exchangeRateGroupRepo.GetGroupByCurrenciesSER((cmbCurrency.SelectedItem as cmbitem).id, (cmbbaseCurrency.SelectedItem as cmbitem).id, creationDate.Year);
                    var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbCurrency.SelectedItem as cmbitem).id, (cmbbaseCurrency.SelectedItem as cmbitem).id, creationDate.Year);

                    if (exchangeRateGroupSER != null)
                    {
                        exchangeRate = exchangeRateGroupSER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                        switch (creationDate.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateJan.ToString();
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateFeb.ToString();
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateMar.ToString();
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateApr.ToString();
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateMay.ToString();
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateJun.ToString();
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateJul.ToString();
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateAug.ToString();
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateSep.ToString();
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateOct.ToString();
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateNov.ToString();
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = exchangeRate.rateDec.ToString();
                                break;
                            default:
                                if (exchangeRate != null)
                                    txtMarginexchangerate.Text = 0.ToString();
                                break;
                        }
                    }
                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                        switch (creationDate.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateJan.ToString();
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateFeb.ToString();
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateMar.ToString();
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateApr.ToString();
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateMay.ToString();
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateJun.ToString();
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateJul.ToString();
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateAug.ToString();
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateSep.ToString();
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateOct.ToString();
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateNov.ToString();
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = exchangeRate.rateDec.ToString();
                                break;
                          
                        }
                    }

                }
            }
            //else
            //{

                if (cmbCurrency.SelectedItem as cmbitem != null)
                {
                    int idd = (cmbbaseCurrency.SelectedItem as cmbitem).id;
                    if (idd == 0)
                    {
                        loadCurrencies();
                    }
                    else
                    {
                        string name = (cmbbaseCurrency.SelectedItem as cmbitem).name;
                    }
                    calculateBaseBudgetMargin();
                    calculateBaseActualMargin();
              
            }
            //}
        }

        private void TxtBudgetMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

            calculateBaseBudgetMargin();
       
        }
      
        private void TxtActualMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculateBaseActualMargin();
    

        }
        public void calculateBaseActualMargin()
        {
            decimal Sototal = 0;
            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
            {
                if(txtCostCenterAmount.Text != "NaN")
                    Sototal = Convert.ToDecimal(txtCostCenterAmount.Text);
                decimal sumbudg = 0;
                if (!string.IsNullOrEmpty(txtActualMargin.Text))
                    sumbudg = Convert.ToDecimal(txtActualMargin.Text);
                if (Sototal != 0)
                {
                    var percent = (((sumbudg) / Sototal) * 100);
                    txtActualPercent.Text = decimal.Round(percent, 2).ToString();
                    txtActualPercent.Text += "%";
                }

                
                
            }
            else
            {
                Sototal = Convert.ToDecimal(txtSoAmount.Text);
                if (txtActualMargin.Text != "")
                {
                    decimal sumbudg = 0;

                    sumbudg = Convert.ToDecimal(txtActualMargin.Text);
                    if (Sototal != 0)
                    {
                        var percent = (((sumbudg) / Sototal) * 100);
                        txtActualPercent.Text = decimal.Round(percent, 2).ToString();
                        txtActualPercent.Text += "%";
                    }

                    decimal exchangeRate = 1;
                    if (txtexchangerate.Text != "")
                        exchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                    decimal marginexchangeRate = 1;
                    if (txtMarginexchangerate.Text != "")
                        marginexchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    decimal margin = Convert.ToDecimal(txtActualMargin.Text);
                    txtBaseActualMargin.Text = (margin * exchangeRate).ToString();
                    txtSaleAMargin.Text = (marginexchangeRate * margin).ToString();
                }
            }
        }
        public void calculateBaseRevisedMargin()
        {
            //decimal Sototal = 0;
            //if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
            //{
            //    if(txtCostCenterAmount.Text != "NaN")
            //    Sototal = Convert.ToDecimal(txtCostCenterAmount.Text);
            //    decimal sumbudg = 0;
            //    if (!string.IsNullOrEmpty(txtRevisedMargin.Text))
            //        sumbudg = Convert.ToDecimal(txtRevisedMargin.Text);
            //    if (Sototal != 0)
            //    {
            //        var percent = (((sumbudg) / Sototal) * 100);
            //        txtRevisedPercent.Text = decimal.Round(percent, 2).ToString();
            //        txtRevisedPercent.Text += "%";
            //    }
            //    calculateCCSER();
            //    calculateCCMER();
            //}
            //else
            //{
            //    //Sototal = Convert.ToDecimal(txtSoAmount.Text);
            //    //if (txtRevisedMargin.Text != "")
            //    //{
            //    //    decimal sumbudg = 0;

            //    //    sumbudg = Convert.ToDecimal(txtRevisedMargin.Text);
            //    //    if (Sototal != 0)
            //    //    {
            //    //        var percent = (((sumbudg) / Sototal) * 100);
            //    //        txtRevisedPercent.Text = decimal.Round(percent, 2).ToString();
            //    //        txtRevisedPercent.Text += "%";
            //    //    }
            //    //    if (!string.IsNullOrEmpty(txtRevisedMargin.Text) && !string.IsNullOrEmpty(txtMarginexchangerate.Text) && !string.IsNullOrEmpty(txtexchangerate.Text))
            //    //    {
            //    //        //var totalRevisedValue = Convert.ToDouble(txtRevisedMargin.Text);
            //    //        var salesExchangeRate = Convert.ToDouble(txtMarginexchangerate.Text);
            //    //        var marketExchangeRate = Convert.ToDouble(txtexchangerate.Text);
            //    //        //txtSaleRevisedMargin.Text = Math.Round(Convert.ToDouble(totalRevisedValue * salesExchangeRate), 2).ToString();
            //    //        //txtBaseRevisedMargin.Text = Math.Round(Convert.ToDouble(totalRevisedValue * marketExchangeRate), 2).ToString();
            //    //    }
            //    //}
            //}
        }
        public void calculateBaseBudgetMargin()
        {
            decimal Sototal = 0;
            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
            {
                if(txtCostCenterAmount.Text != "NaN")
                    Sototal = Convert.ToDecimal(txtCostCenterAmount.Text);

                decimal sumbudg = 0;
                if (txtBudgetMargin.Text != "" /*&& txtActualMargin.Text != "" && txtActualMargin.Text != ""*/)
                {
                    sumbudg = Convert.ToDecimal(txtBudgetMargin.Text);
                    if (Sototal != 0)
                    {
                        var percent = (((sumbudg) / Sototal) * 100);
                        txtBudgetedPercent.Text = decimal.Round(percent, 2).ToString();
                        txtBudgetedPercent.Text += "%";
                    }
                }

                calculateCCMER();
                calculateCCSER();
            }
            else
            {
                Sototal = Convert.ToDecimal(txtSoAmount.Text);
                if (txtBudgetMargin.Text != "" /*&& txtActualMargin.Text != "" && txtActualMargin.Text != ""*/)
                {
                    decimal sumbudg = 0;
                    sumbudg = Convert.ToDecimal(txtBudgetMargin.Text);
                    if (Sototal != 0)
                    {
                        var percent = (((sumbudg) / Sototal) * 100);
                        txtBudgetedPercent.Text = decimal.Round(percent, 2).ToString();
                        txtBudgetedPercent.Text += "%";
                    }
                    decimal exchangeRate = 1;
                    decimal marginexchangeRate = 1;
                    decimal margin = Convert.ToDecimal(txtBudgetMargin.Text);//Convert.ToInt32(txtexchangerate.Text)!=0&& 
                    if (txtexchangerate.Text != "")
                    {
                        exchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                        txtBaseBudgetMargin.Text = (margin * exchangeRate).ToString();

                    }
                    if (txtMarginexchangerate.Text != "")
                    {
                        marginexchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                        txtSaleBudgetMargin.Text = (marginexchangeRate * margin).ToString();
                    }
                }
            }
        }
        public void calculateBaseCommision()
        {
            if (txtCommision.Text != "" && txtexchangerate.Text != "")
            {
                decimal exchangeRate = 1;
                if (txtexchangerate.Text != "")
                    exchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                decimal margin = Convert.ToDecimal(txtCommision.Text);
                txtBaseCommission.Text = (margin * exchangeRate).ToString();
                decimal marginexchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                {
                    marginexchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtSalesCommision.Text = (marginexchangeRate * margin).ToString();
                }
            }
        }
        public void calculateNetBaseCommision()
        {
            if (txtNetCommision.Text != "" && txtexchangerate.Text != "")
            {
                decimal exchangeRate = 1;
                if (txtexchangerate.Text != "")
                    exchangeRate = Convert.ToDecimal(txtexchangerate.Text);

                decimal margin = Convert.ToDecimal(txtNetCommision.Text);
                txtNetBaseCommission.Text = (margin * exchangeRate).ToString();
                decimal marginexchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                {
                    marginexchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtNetSalesCommision.Text = (marginexchangeRate * margin).ToString();
                }
            }
        }
        private void TxtCommision_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (saleOrder.commision != null)
            {
                if (Convert.ToDecimal(txtCommision.Text) != saleOrder.commision)
                {
                    foreach (var item in grdSOItems.ItemsSource as List<ProcurementProduct>)
                    {
                        item.totalCommission = Convert.ToDouble(txtCommision.Text);
                        item.UnInvoicedSoAmount = Convert.ToDouble(Convert.ToDecimal(txtCommision.Text) - Convert.ToDecimal(saleOrder.commision));
                    }
                    grdSOItems.RefreshData();
                }
            }
            calculateBaseCommision();
            calculatetotal();
        }

        private void DatPaymentDueFrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            setPaymentTillDate();
        }

        private void TxtPaymentDueDays_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txtPaymentDueDays.Text != "")
            {
                setPaymentTillDate();
            }
        }
        public void setPaymentTillDate()
        {
            if (datPaymentDueFrom.Text != "" && txtPaymentDueDays.Text != "")
                datPaymentDueTill.DateTime = datPaymentDueFrom.DateTime.AddDays(Convert.ToInt32(txtPaymentDueDays.Text));//new DateTime(datPaymentDueFrom.DateTime.Year, datPaymentDueFrom.DateTime.Month, datPaymentDueFrom.DateTime.Day+Convert.ToInt32(txtPaymentDueDays.Text));
            else
                datPaymentDueTill.Text = "";
        }

        private void Txttotalcfr_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtSoAmount.Text = txttotalcfr.Text;
            txtSoAmount.Text = txttotalcfr.Text;
            txtCostCenterAmount.Text = (Convert.ToDouble(txttotalcfr.Text) * Convert.ToDouble(txtCostCenterExchangeRate.Text)).ToString();
        }

        private void TxtMarginexchangerate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculatetotal();
            calculateBaseCommision();
            calculateBaseBudgetMargin();
            calculateBaseActualMargin();
            calculateBaseRevisedMargin();
        }

        private void Txtexchangerate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculatetotal();
            calculateBaseCommision();
            calculateBaseBudgetMargin();
            calculateBaseRevisedMargin();
            calculateBaseActualMargin();
        }

        private void BtnCostSheet_Click(object sender, RoutedEventArgs e)
        {
            btnBudgertPunching.IsEnabled = false;
            try
            {
                string inco = "";
                string paymentterm = "";
                if (cmbPaymentTerm.SelectedItem != null)
                {
                    paymentterm = (cmbPaymentTerm.SelectedItem as cmbitem).name;
                }
                if (cmbIncoterm.SelectedItem != null)
                {
                    inco = (cmbIncoterm.SelectedItem as cmbitem).name;
                }

                if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    frmCostSheet frmCostSheet = new frmCostSheet(saleOrder, txtFinanaceRef.Text, txtSalesref.Text, lookupDepartment.Text, lookupCustomer.Text, symbol, txtSoAfterGST.Text, inco, datpoCreationdate.Text, paymentterm, txtMaker.Text, txtOrigin.Text, views, (InquiryType)cmbSaleOrderType.SelectedIndex, lookupPrincipal.Text, txtPacking.Text, datDeliverydate.DateTime, cmbWarranty.Text, txtsaleOrderref.Text, datsaleOrderdate.Text, saleOrder.isApproved, saleOrder.isReApproved);
                    if (saleOrder.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Cost Center after SO Approval") == null)
                    {
                        frmCostSheet.grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        //frmCostSheet.lookupVendors.IsEnabled = false;
                        //frmCostSheet.lookupOC.IsEnabled = false;
                    }

                    frmCostSheet.ShowDialog();
                    if (frmCostSheet.costSheet != null)
                    {
                        saleOrder.CostSheet = frmCostSheet.costSheet;
                        txtBudgetMargin.Text = (Convert.ToDecimal(txtSoAfterGST.Text) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                        //txtRevisedMargin.Text = (Convert.ToDecimal(txtSoAfterGST.Text) - saleOrder.CostSheet.TotalRevisedMargin).ToString();
                        if (Convert.ToDecimal(txtSoAfterGST.Text) != Convert.ToDecimal(txtSoAfterGST.Text))
                        {
                            txtActualMargin.Text = saleOrder.CostSheet.TotalActualMargin.ToString();
                        }
                        else
                        {
                            txtActualMargin.Text = (Convert.ToDecimal(txtSoAfterGST.Text) - saleOrder.CostSheet.TotalActualMargin).ToString();
                        }
                        if ((saleOrder.isReApproved != false) && frmCostSheet.isReApproved == false)
                            saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();
                        saleOrder.isReApproved = frmCostSheet.isReApproved;
                    }
                }
                else
                {
                    frmCostSheet frmCostSheet = new frmCostSheet(saleOrder, txtFinanaceRef.Text, txtSalesref.Text, lookupDepartment.Text, lookupCustomer.Text, symbol, txttotalcfr.Text, inco, datpoCreationdate.Text, paymentterm, txtMaker.Text, txtOrigin.Text, views, (InquiryType)cmbSaleOrderType.SelectedIndex, lookupPrincipal.Text, txtPacking.Text, datDeliverydate.DateTime, cmbWarranty.Text, txtsaleOrderref.Text, datsaleOrderdate.Text, saleOrder.isApproved, saleOrder.isReApproved);
                    if (saleOrder.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Cost Center after SO Approval") == null)
                    {
                        frmCostSheet.grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        frmCostSheet.grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                        //frmCostSheet.lookupVendors.IsEnabled = false;
                        //frmCostSheet.lookupOC.IsEnabled = false;
                    }
                    frmCostSheet.ShowDialog();
                    if (frmCostSheet.costSheet != null)
                    {
                        saleOrder.CostSheet = frmCostSheet.costSheet;
                        if(Convert.ToDecimal(txtCostCenterAmount.Text)!=0)
                        {
                            txtBudgetMargin.Text = (Convert.ToDecimal(txtCostCenterAmount.Text) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                            //txtRevisedMargin.Text = (Convert.ToDecimal(txtCostCenterAmount.Text) - saleOrder.CostSheet.TotalRevisedMargin).ToString();
                            if((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
                            {
                                txtActualMargin.Text = (Convert.ToDecimal(txtCostCenterAmount.Text) - saleOrder.CostSheet.TotalActualMargin).ToString();
                            }
                            else
                            {
                                if (Convert.ToDecimal(txtCostCenterAmount.Text) != Convert.ToDecimal(txttotalcfr.Text))
                                {
                                    txtActualMargin.Text = saleOrder.CostSheet.TotalActualMargin.ToString();
                                }
                                else
                                {
                                    txtActualMargin.Text = (Convert.ToDecimal(txtCostCenterAmount.Text) - saleOrder.CostSheet.TotalActualMargin).ToString();
                                }
                            }

                            
                        }
                        else
                        {
                            txtBudgetMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                            //txtRevisedMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalRevisedMargin).ToString();
                            if (Convert.ToDecimal(txttotalcfr.Text) != Convert.ToDecimal(txttotalcfr.Text))
                            {
                                txtActualMargin.Text = saleOrder.CostSheet.TotalActualMargin.ToString();
                            }
                            else
                            {
                                txtActualMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalActualMargin).ToString();
                            }
                        }
                        if ((saleOrder.isReApproved != false) && frmCostSheet.isReApproved == false)
                            saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();
                        saleOrder.isReApproved = frmCostSheet.isReApproved;
                    }
                    calculateSystemMargins();
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }
        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            
            if (gridTracker.Visibility == Visibility.Visible)
            {
               
                gridTracker.Visibility = Visibility.Collapsed;
            }
            else
            if(gridTracker.Visibility == Visibility.Collapsed && saleOrder.Id!=0)
            {
               
                views = UsersRepo.getViwerInfo(saleOrder.Id, (int)TransactionItemType.Sale_Order);
                grdUsers.ItemsSource = views;
                gridTracker.Visibility = Visibility.Visible;
            }
            
        }
        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editOrder == 1 && saleOrder != null)
                {
                    saleOrderRepo = new SaleOrderRepo();
                    saleOrder = new SaleOrder();
                    saleOrder = saleOrderRepo.get(saleOrderid);
                    UsersRepo usersRepo = new UsersRepo();
                    if (saleOrder != null)
                    {
                        if (saleOrder.isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Sale Order is Approved, Do you want to UnApprove this Sale Order?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {
                                    saleOrder.isApproved = false;
                                    saleOrder.PendingForClosing = null;
                                    saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                    saleOrderRepo.Approve(saleOrder);
                                    var res1 = MessageBox.Show("SO has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && saleOrder.InterDepartment != null && saleOrder.InterDepartment.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), saleOrder.Id, TransactionItemType.Sale_Order);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count != 0)
                                            {
                                                if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                                {
                                                    saleOrder.holderChangeDate = DateTime.Now;
                                                }
                                                saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                                saleOrderRepo.update(saleOrder);
                                            }
                                        }
                                        else if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                            win.ShowDialog();
                                            tagUsers = win.tagUsers;
                                            ccUsers = win.ccUsers;
                                            tagUsersRecommendation = win.tagRecommendationUsers;
                                            ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count != 0)
                                            {
                                                if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                                {
                                                    saleOrder.holderChangeDate = DateTime.Now;
                                                }
                                                saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                                saleOrderRepo.update(saleOrder);
                                            }

                                            ccUsers = win.ccUsers;
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    string symbolCurr = "";

                                    if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
                                    {
                                        if (saleOrder.costcenterCurrency != null)
                                        {
                                            symbolCurr = saleOrder.costcenterCurrency.Abbrivation.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (saleOrder.currency != null)
                                        {
                                            symbolCurr = saleOrder.currency.Abbrivation.ToString();
                                        }
                                    }


                                    CommentLog comment = new CommentLog();
                                    
                                    if (saleOrder.saleOrdertype == InquiryType.Principal)
                                    {
                                        comment.Comment = "SO (Comission) having value: " + saleOrder.commision.ToString() + "(" + symbolCurr + ") " + " has been UnApproved"+"\n"
                                            +"SO(Net Comission) having value: " + saleOrder.netCommision.ToString()
                                           ;
                                        comment.Timestamp = DateTime.Now;
                                        comment.Subject = "SO UnApproved";                                       
                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                      
                                    }
                                    else
                                    {

                                        comment.Comment = "SO (Amount OC) having value: " + saleOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been UnApproved";
                                        comment.Timestamp = DateTime.Now;
                                        comment.Subject = "SO UnApproved";                                       
                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                    }
                                    procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("Sale Order are UnApproved (" + saleOrder.referenceNo + ")");
                                    SystemLog.LogInfo(this.GetType(), "Sale Order is UnApproved (" + saleOrder.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Sale Oder Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Sale Oder Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (saleOrder.isApproved == false)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();
                                var res = MessageBox.Show("Sale Order are Pending for Approval, Do you want to Approve this Sale Order?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res == MessageBoxResult.Yes)
                                {
                                    saleOrder.isApproved = true;
                                    saleOrder.stage = TransactionStage.Approved.ToString();
                                    saleOrderRepo.Approve(saleOrder);

                                    var res1 = MessageBox.Show("SO has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && saleOrder.InterDepartment != null && saleOrder.InterDepartment.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), saleOrder.Id, TransactionItemType.Sale_Order);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count != 0)
                                            {
                                                if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                                {
                                                    saleOrder.holderChangeDate = DateTime.Now;
                                                }
                                                saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                                saleOrderRepo.update(saleOrder);
                                            }
                                        }
                                        else if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                                        {
                                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count != 0)
                                            {
                                                if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                                {
                                                    saleOrder.holderChangeDate = DateTime.Now;
                                                }
                                                saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                                saleOrderRepo.update(saleOrder);
                                            }
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }

                                    }

                                    string symbolCurr = "";
                                    if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
                                    {
                                        if (saleOrder.costcenterCurrency != null)
                                        {
                                            symbolCurr = saleOrder.costcenterCurrency.Abbrivation.ToString();
                                        }
                                    }
                                    else
                                    {
                                        if (saleOrder.currency != null)
                                        {
                                            symbolCurr = saleOrder.currency.Abbrivation.ToString();
                                        }
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "SO (Amount OC) having value: " + saleOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "SO Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("Sale Oder is Approved (" + saleOrder.referenceNo + ")");
                                    SystemLog.LogInfo(this.GetType(), "Sale Order is Approved (" + saleOrder.Id + ")");
                                }


                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Sale Order Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Sale Order Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        

                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }
                    //Load_Receipts();

                }

                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (saleOrder.isApproved != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null)
                {
                    saleOrder.isReviewed = true;
                    saleOrder.isApproved = false;
                    saleOrder.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, saleOrder.Id, (int)TransactionItemType.Sale_Order, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleOrderRepo.update(saleOrder);
                    notificationsRepo.Add("SaleOrder Rejected", saleOrder.Id, TransactionItemType.Sale_Order, "SaleOrder with refrence # " + saleOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleOrder" + frmInputBox.comment, null);
                    return;
                }

                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null))
                {
                    saleOrder.isReviewed = false;
                    saleOrder.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, saleOrder.Id, (int)TransactionItemType.Sale_Order, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleOrderRepo.update(saleOrder);
                    notificationsRepo.Add("SaleOrder Rejected", saleOrder.Id, TransactionItemType.Sale_Order, "SaleOrder with refrence # " + saleOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleOrder" + frmInputBox.comment, null);
                }

                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null))
                {
                    saleOrder.isReviewed = false;
                    saleOrder.stage = TransactionStage.Rejected.ToString();
                  
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, saleOrder.Id, (int)TransactionItemType.Sale_Order, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleOrderRepo.update(saleOrder);

                    notificationsRepo.Add("SaleOrder Rejected", saleOrder.Id, TransactionItemType.Sale_Order, "SaleOrder with refrence # " + saleOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleOrder" + frmInputBox.comment, null);

                }
            }
            else if (saleOrder.PendingForClosing == true)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null)
                {
                    saleOrder.isReviewed = true;
                    {
                        saleOrder.PendingForClosing = true;

                    }
                    saleOrder.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, saleOrder.Id, (int)TransactionItemType.Sale_Order, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleOrderRepo.update(saleOrder);

                    notificationsRepo.Add("SaleOrder Rejected", saleOrder.Id, TransactionItemType.Sale_Order, "SaleOrder with refrence # " + saleOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleOrder" + frmInputBox.comment, null);

                    return;

                }

                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null))
                {
                    saleOrder.isReviewed = true;
                    saleOrder.needReview = false;
                    saleOrder.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, saleOrder.Id, (int)TransactionItemType.Sale_Order, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleOrderRepo.update(saleOrder);

                    notificationsRepo.Add("SaleOrder Rejected", saleOrder.Id, TransactionItemType.Sale_Order, "SaleOrder with refrence # " + saleOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleOrder" + frmInputBox.comment, null);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null))
                {
                    saleOrder.isReviewed = true;
                    saleOrder.needReview = true;
                    saleOrder.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, saleOrder.Id, (int)TransactionItemType.Sale_Order, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleOrderRepo.update(saleOrder);

                    notificationsRepo.Add("SaleOrder Rejected", saleOrder.Id, TransactionItemType.Sale_Order, "SaleOrder with refrence # " + saleOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleOrder" + frmInputBox.comment, null);

                }
            }
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Order") != null )
            {
                loadcomments();
            }
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }
        public void PushComment()
        {
            if (saleOrder != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                if (frmInputBox.commentAdded == true && saleOrder.Id != 0)
                {
                    if (!string.IsNullOrEmpty(frmInputBox.Comment.Comment))
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            var commentId = procurementRepo.AddCommentLinkNotification(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);
                                }
                            }
                            if (frmInputBox.Comment.TaggedList.Count > 0)
                            {
                                var transactionHolder = frmInputBox.Comment.TaggedList[0].employeeId;
                                var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                                cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == transactionHolder))];
                            }
                        }
                        MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        if (window1 != null) { window1.UrgentNotificationGlow(); }
                    }
                }

            }
        }
        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Sale_Order);
                inputBox.ShowDialog();
            }
            else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Order);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (saleOrder != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.commentAdded == true && saleOrder.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);
                            }
                        }
                        if (frmInputBox.Comment.TaggedList.Count > 0)
                        {
                            var transactionHolder = frmInputBox.Comment.TaggedList[0].employeeId;
                            var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                            cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == transactionHolder))];
                        }
                    }
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
        public void loadcomments()
        {
            try
            {
                if (saleOrder != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(saleOrder.Id, TransactionItemType.Sale_Order);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void BtnSummarySheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string inco = "";
                string paymentterm = "";
                if (cmbPaymentTerm.SelectedItem != null)
                {
                    paymentterm = (cmbPaymentTerm.SelectedItem as cmbitem).name;
                }
                if (cmbIncoterm.SelectedItem != null)
                {
                    inco = (cmbIncoterm.SelectedItem as cmbitem).name;
                }
                frmCommissionSummarySheet frmSummarySheet = new frmCommissionSummarySheet(saleOrder, txtFinanaceRef.Text, txtsaleOrderref.Text, txtOfferRefNo.Text, lookupCustomer.Text, (cmbCurrency.SelectedItem as cmbitem).id, txtCommision.Text, datsaleOrderdate.Text, paymentterm, (InquiryType)cmbSaleOrderType.SelectedIndex, lookupPrincipal.Text, txttotalcfr.Text, txttotalfob.Text, txtPacking.Text, inco, datDeliverydate.DateTime, cmbTransshipment.Text, datpoCreationdate.Text, txtSalesref.Text, views);
                frmSummarySheet.ShowDialog();
                if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.Principal)
                {
                    if (frmCommissionSummarySheet.summarySheet != null)
                    {
                        saleOrder.CostSheet = null;
                        saleOrder.CommissionSummarySheet = frmCommissionSummarySheet.summarySheet;
                        txtCommision.Text = (saleOrder.CommissionSummarySheet.SOCommission).ToString();
                        txtNetCommision.Text = (saleOrder.CommissionSummarySheet.netSOCommission).ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }



        private void LstComments_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void TxtNetCommision_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculateNetBaseCommision();

        }
        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing!=true )
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can attach document when SO Closed") != null)
                {

                    if (grdAttach.Visibility == Visibility.Visible)
                        grdAttach.Visibility = Visibility.Collapsed;
                    else
                    {
                        cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                        grdAttach.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required" + " Can attach document when SO Closed!");
                }
            }
            else
            {
                if (grdAttach.Visibility == Visibility.Visible)
                    grdAttach.Visibility = Visibility.Collapsed;
                else
                {
                    cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                    grdAttach.Visibility = Visibility.Visible;
                }
            }
        }
        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                if (saleOrder.Id != 0)
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(saleOrder.Id, TransactionItemType.Sale_Order);
                }
                grdAttachments.Visibility = Visibility.Visible;

            }
        }
        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                        if (str.Contains("Sale_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Inquiry"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Inquiry);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Offer"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Offer);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Invoice"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Invoice);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Purchase_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Purchase_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Receipt"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Receipt);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Payments"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Payments);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }


                    });
                    thread.Start();
                }
                else
                    return;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());
            }
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (OrderId != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\SaleOrder\\ToUpload\\";
                        
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += OrderId + "_" + TransactionItemType.Sale_Order.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Sale_Order);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Sale_Order, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, saleOrder.Id, 3, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {

                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Sale_Order);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });
                                    }
                                });
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {



                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

        }

        private void TxtRevisedMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculateBaseRevisedMargin();
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (OrderId != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(OrderId, TransactionItemType.Sale_Order);
                trackingWindow.ShowDialog();
            }
        }

        private void TxtPER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPER.Text) && !string.IsNullOrEmpty(txtPERAmount.Text))
            {
                var PER = Convert.ToDouble(txtPER.Text);
                var PERAmount = Convert.ToDouble(txtPERAmount.Text);

                txtSoAmountPer.Text = Math.Round( (PER * PERAmount), 2).ToString();

                int count = grdSplitPER.VisibleItems.Count;
                for (int i = 0; i < count; i++)
                {
                    grdSplitPER.SetCellValue(i, grdSplitPER.Columns["PER"], Convert.ToDecimal(PER));
                    //_item.ExchangeRate = exchangeRate;
                }
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            var SIcount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).ToList().Count;
            var purchaseOrders = saleOrder.PurchaseOrders;
            var bills = saleOrder.Bills;

            if(SIcount != 0)
            {
                DXMessageBox.Show("This Sale Order cannot be Voided because it has Active Sale Invoices!");
                return;
            }
            if(purchaseOrders != null)
            {
                var POcount = purchaseOrders.Where(x=>x.isVoid != true).ToList();
                if(POcount.Count > 0)
                {
                    DXMessageBox.Show("This Sale Order cannot be Voided because it has Active Purchase Orders!");
                    return;
                }
            }
            if (bills != null)
            {
                var billCount = bills.Where(x => x.isVoid != true).ToList();
                if (billCount.Count > 0)
                {
                    DXMessageBox.Show("This Sale Order cannot be Voided because it has Active Bills!");
                    return;
                }
            }


            if (saleOrder.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void SaleOrder") != null))
            {

                if (DXMessageBox.Show("This SO is currently in the list of Void Sale Orders! Do you want to remove it from Void?", "Remove Void Sale Order", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    saleOrder.isVoid = false;
                    saleOrderRepo.setSotoVoid(saleOrder.Id, false);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("SO has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && saleOrder.InterDepartment != null && saleOrder.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), saleOrder.Id, TransactionItemType.Sale_Order);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleOrder.holderChangeDate = DateTime.Now;
                                }
                                saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                saleOrderRepo.update(saleOrder);
                            }
                            ccUsers = win.ccUsers;
                        }
                        else if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleOrder.holderChangeDate = DateTime.Now;
                                }
                                saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                saleOrderRepo.update(saleOrder);
                            }
                            ccUsers = win.ccUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (saleOrder.currency != null)
                    {
                        symbolCurr = saleOrder.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "SO (Amount OC) having value: " + saleOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "SO UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void SaleOrder") != null)
            {
                if (DXMessageBox.Show("This So is not currently in the list of Void Sale Orders! Do you want to move it to Void Saleorders?", "Add to Void Saleorders", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    saleOrder.isVoid = true;
                    saleOrderRepo.setSotoVoid(saleOrder.Id, true);

                    grdVoid.Visibility = Visibility.Visible;


                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("SO has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && saleOrder.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && saleOrder.InterDepartment != null && saleOrder.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), saleOrder.Id, TransactionItemType.Sale_Order);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleOrder.holderChangeDate = DateTime.Now;
                                }
                                saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                saleOrderRepo.update(saleOrder);
                            }
                            ccUsers = win.ccUsers;
                        }
                        else if (saleOrder.department != null && saleOrder.department.Id != 0 && saleOrder.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleOrder.holderChangeDate = DateTime.Now;
                                }
                                saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                saleOrderRepo.update(saleOrder);
                            }
                            ccUsers = win.ccUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (saleOrder.currency != null)
                    {
                        symbolCurr = saleOrder.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "SO (Amount OC) having value: " + saleOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been marked as void \nFrom: ",
                        Timestamp = DateTime.Now,
                        Subject = "SO Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();


        }

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
                    if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    {

                        // frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartment(department.Id), comment, TransactionItemType.Sale_Order);
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), comment, TransactionItemType.Sale_Order);
                        inputBox.ShowDialog();
                    }
                    else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.Sale_Order);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (saleOrder != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && saleOrder.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

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
                    loadcomments();
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
        }


        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            gridInterCompanyDetails.IsEnabled = true;

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            gridInterCompanyDetails.IsEnabled = false;
        }
        private void lookupInterDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            InterDepartment = lookupInterDepartment.SelectedItem as Department;

        }

        private void BtnDirectClose_Click(object sender, RoutedEventArgs e)
        {


            bool saleInvoiceClosed = true;
            bool saleReceiptClosed = true;
            bool purchaseOrderClosed = true;
            bool purchaseInvoiceClosed = true;
            bool paymentClosed = true;
            bool vendorBillClosed = true;

            if (saleOrder.SaleInvoices.Count > 0)
            {
                foreach (var invoice in saleOrder.SaleInvoices)
                {   

                    if (invoice.saleInvoiceStatus.isActive != false && invoice.isVoid!=true)
                    {
                        saleInvoiceClosed = false;
                        break;
                    }
                }
            }
            if (saleOrder.SaleInvoices.Count > 0)
            {
                foreach (var invoice in saleOrder.SaleInvoices)
                {
                    if (invoice.salesReceipts.Count > 0)
                    {
                        foreach (var receipt in invoice.salesReceipts)
                        {
                            if (receipt.saleReceiptStatus.isActive != false && receipt.isVoid != true)
                            {
                                saleReceiptClosed = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (saleOrder.PurchaseOrders.Count > 0)
            {
                foreach (var po in saleOrder.PurchaseOrders)
                {
                    if (po.PurchaseOrderStatus.isActive != false && po.isVoid != true)
                    {
                        purchaseOrderClosed = false;
                        break;
                    }
                }
            }
            if (saleOrder.PurchaseOrders.Count > 0)
            {
                foreach (var PO in saleOrder.PurchaseOrders)
                {
                    if (PO.PurchaseInvoices.Count > 0)
                    {
                        foreach (var invoice in PO.PurchaseInvoices)
                        {
                            if (invoice.PurchaseInvoiceStatus.isActive != false && invoice.isVoid != true)
                            {
                                purchaseInvoiceClosed = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (saleOrder.PurchaseOrders.Count > 0)
            {
                foreach (var PO in saleOrder.PurchaseOrders)
                {
                    if (PO.Bills.Count > 0)
                    {
                        foreach (var bill in PO.Bills)
                        {
                            if (bill.BillStatus.isActive != false && bill.isVoid != true)
                            {
                                vendorBillClosed = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (saleOrder.Bills.Count > 0)
            {
                foreach (var bill in saleOrder.Bills)
                {
                    if (bill.BillStatus.isActive != false && bill.isVoid != true)
                    {
                        vendorBillClosed = false;
                        break;
                    }
                }
            }
            if (saleOrder.PurchaseOrders.Count > 0)
            {
                foreach (var PO in saleOrder.PurchaseOrders)
                {
                    if (PO.Bills.Count > 0)
                    {
                        foreach (var bill in PO.Bills)
                        {
                            if (bill.Payments.Count > 0)
                            {
                                foreach (var payment in bill.Payments)
                                {

                                    if (payment.Status.isActive != false && payment.isVoid != true)
                                    {
                                        paymentClosed = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (saleOrder.Bills.Count > 0)
            {
                foreach (var bill in saleOrder.Bills)
                {
                    if (bill.Payments.Count > 0)
                    {
                        foreach (var payment in bill.Payments)
                        {

                            if (payment.Status.isActive != false && payment.isVoid != true)
                            {
                                paymentClosed = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (saleOrder.PurchaseOrders.Count > 0)
            {
                foreach (var PO in saleOrder.PurchaseOrders)
                {
                    if (PO.PurchaseInvoices.Count > 0)
                    {
                        foreach (var purchaseInvoice in PO.PurchaseInvoices)
                        {
                            if (purchaseInvoice.Payments.Count > 0)
                            {
                                foreach (var payment in purchaseInvoice.Payments)
                                {
                                    if (payment.Status.isActive != false && payment.isVoid != true)
                                    {
                                        paymentClosed = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (saleInvoiceClosed==false)
            {
                MessageBox.Show("Please close Sale invoices first", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            if (saleReceiptClosed == false)
            {
                MessageBox.Show("Please close Sales receipt first", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            if (purchaseOrderClosed == false)
            {
                MessageBox.Show("Please close Purchase orders first", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            if (purchaseInvoiceClosed == false)
            {
                MessageBox.Show("Please close Purchase Invoices first", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            if (paymentClosed == false)
            {
                MessageBox.Show("Please close payments first", "Information", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            if (vendorBillClosed == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Close Sale Order without Closing Vendor Bill") == null)
            {
                MessageBox.Show("Permission required to close Sale Order without closing Vendor Bills first", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                saleOrderid = saleOrder.Id;
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null) ? true : false)
                {
                    double total = 0;

                    if (saleOrder.saleOrdertype == InquiryType.DistributionBiz)
                    {
                        total = Math.Round(saleOrder.totalNetAmount, 2);
                    }
                    else if (saleOrder.saleOrdertype == InquiryType.DistributionBiz_CustomerCredit)
                    {
                        total = Math.Round(saleOrder.totalNetAmount, 2);
                    }
                    else
                    {
                        total = Math.Round(saleOrder.totalCFRValue, 2);
                    }

                    var result = Convert.ToDouble(saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                    result = Math.Round(result, 2);
                    double remainingCollection = Math.Round(total - result, 2);
                    if (saleOrder.saleOrdertype == InquiryType.Principal)
                    {
                        var invoicedAmount = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(z => z.totalInvoiceAmount);
                        var collected = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                        remainingCollection = Convert.ToDouble(invoicedAmount) - Convert.ToDouble(collected);
                    }
                    if (remainingCollection != 0)
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without receiving fully Collection") != null)
                        {
                            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to close sale order without complete collection?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {

                                UsersRepo usersRepo = new UsersRepo();

                                saleOrder = saleOrderRepo.get(saleOrderid);

                                var row = saleOrder;

                                decimal budgetMarginOC = 0;
                                decimal actualMarginOC = 0;

                                decimal totalCFRValue = 0;
                                decimal TotalBudgetedMargin = 0;
                                decimal margin = 0;

                                decimal TotalActualMargin = 0;
                                decimal ActualMargin = 0;

                                if (row.totalCFRValue != 0)
                                {
                                    totalCFRValue = Convert.ToDecimal(row.totalCFRValue);
                                }
                                if (row.margin != 0)
                                {
                                    margin = Convert.ToDecimal(row.margin);
                                }
                                if (row.ActualMargin != 0)
                                {
                                    ActualMargin = Convert.ToDecimal(row.ActualMargin);
                                }
                                if (row.CostSheet != null)
                                {
                                    TotalBudgetedMargin = row.CostSheet.TotalBudgetedMargin;
                                    TotalActualMargin = row.CostSheet.TotalActualMargin;
                                }
                                if (row.CostSheet != null)
                                {
                                    budgetMarginOC = totalCFRValue - TotalBudgetedMargin;
                                }
                                else
                                {
                                    budgetMarginOC = margin;
                                }
                                if (row.CostSheet != null)
                                {
                                    actualMarginOC = totalCFRValue - TotalActualMargin;
                                }
                                else
                                {
                                    actualMarginOC = ActualMargin;
                                }
                                if (row.saleOrderStatus != null)
                                {
                                    oldStatus = row.saleOrderStatus;
                                }
                                SaleOrderss.ucStatuschange.inActiveStatuses = 1;

                                SaleOrderss.ucStatuschange.saleOrderid = (int)saleOrderid;
                                SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange(saleOrderRepo);
                                var myWindow = Window.GetWindow(this);
                                statusChange.Owner = myWindow;
                                statusChange.ShowDialog();
                                if (SaleOrderss.ucStatuschange.saleOrder.Id != 0)

                                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null) ? true : false)
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = false;
                                        SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.Approved.ToString();
                                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                        inputBox.ShowDialog();
                                        usersRepo.Add(TransactionInfo.Approved_Closing, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                    }

                                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                        if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing == null)
                                        {
                                            SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;
                                        }
                                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                        inputBox.ShowDialog();
                                        usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                    }
                                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                        if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing != true)
                                        {
                                            SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                        }
                                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                        inputBox.ShowDialog();
                                        usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                    }
                                    else
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                        SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;
                                        usersRepo.Add(TransactionInfo.Closed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                    }
                                SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                                SaleOrderss.ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                                if (row.saleOrderStatus != SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus)
                                    usersRepo.Add(TransactionInfo.Status_Changed, saleOrder.Id, (int)TransactionItemType.Sale_Order, "While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("SO has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res == MessageBoxResult.Yes)
                                {
                                    if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                    {
                                        winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                        win.ShowDialog();
                                        tagUsers = win.tagUsers;
                                        if (win.tagUsers.Count > 0)
                                        {
                                            if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                            {
                                                saleOrder.holderChangeDate = DateTime.Now;
                                            }
                                            saleOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                            saleOrderRepo.update(saleOrder);
                                        }
                                        ccUsers = win.ccUsers;
                                    }
                                    else
                                    {
                                        winTagUsers win = new winTagUsers();
                                        win.ShowDialog();
                                    }
                                }
                                string oldStat = "";
                                if (oldStatus != null)
                                {
                                    oldStat = oldStatus.Status;
                                }
                                string newStat = SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status;
                                string symbolCurr = "";

                                if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
                                {
                                    if (row.costcenterCurrency != null)
                                    {
                                        symbolCurr = row.costcenterCurrency.Abbrivation.ToString();
                                    }
                                }
                                else
                                {
                                    if (row.currency != null)
                                    {
                                        symbolCurr = row.currency.Abbrivation.ToString();
                                    }
                                }
                                CommentLog comment = new CommentLog();
                                if (saleOrder.Id != 0 && saleOrder.saleOrdertype == InquiryType.Principal && saleOrder.commision != 0)
                                {
                                    comment.Comment = "Status of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                                   + "Comission(OC): " + saleOrder.commision.ToString() + " (" + symbolCurr + ")"
                                                   + "\n Net-Comissio(OC): " + saleOrder.netCommision.ToString() + " (" + symbolCurr + ")"
                                                   + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                                    comment.Timestamp = DateTime.Now;
                                    comment.Subject = "Status Changed using direct close";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                    if (saleOrder.StatusClass != null)
                                    {
                                        string oldStatClass = saleOrder.StatusClass.ClassName;

                                        string newStatClass = checkStatusClass.ClassName;
                                        if (checkStatusClass.Id != saleOrder.StatusClass.Id && saleOrder.StatusClass != null)
                                        {
                                            comment.Comment = "Status and Status Class of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                             + "Comission(OC): " + saleOrder.commision.ToString() + " (" + symbolCurr + ")"
                                             + "\n Net-Comissio(OC): " + saleOrder.netCommision.ToString() + " (" + symbolCurr + ")"
                                             + "\nhas been changed \n"
                                             + "Status From: " + oldStat + " \nTo: " + newStat
                                             + "Status From: " + oldStatClass + " \nTo: " + newStatClass;                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                            comment.Subject = "Status and Status Class Changed using direct close";
                                        }
                                    }
                                    procurementRepo.Add(row.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {


                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                                else
                                {
                                    comment = new CommentLog()
                                    {
                                        Comment = "Status of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                                + "Budget Margin(OC): " + txtBudgetMargin.Text + " (" + symbolCurr + ")"
                                                + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                                + "\nActual Margin(OC): " + txtActualMargin.Text + " (" + symbolCurr + ")"
                                                + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                                + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat
                                    };
                                    comment.Timestamp = DateTime.Now;
                                    comment.Subject = "Status Changed using Direct Close";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;

                                    if (saleOrder.StatusClass != null)
                                    {
                                        string oldStatClass = saleOrder.StatusClass.ClassName;

                                        string newStatClass = checkStatusClass.ClassName;
                                        if (checkStatusClass.Id != saleOrder.StatusClass.Id && saleOrder.StatusClass != null)
                                        {
                                            comment.Comment = "Status and Status Class of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                             + "Budget Margin(OC): " + txtBudgetMargin.Text + " (" + symbolCurr + ")"
                                             + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                             + "\nActual Margin(OC): " + txtActualMargin.Text + " (" + symbolCurr + ")"
                                             + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                             + "\nhas been changed \n"
                                             + "Status From: " + oldStat + "\n"
                                             + "To: " + newStat + "\n"
                                             + "Status Class From: " + newStatClass + " \n"
                                             + "To: " + oldStatClass + "\n";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                            comment.Subject = "Status and Status Class Changed using direct close";
                                        }

                                        procurementRepo.Add(row.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {


                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                            }
                                        }
                                    }


                                }
                                try
                                {
                                    row = SaleOrderss.ucStatuschange.saleOrder;

                                    saleOrderRepo.updateStatusById(row.Id, row.saleOrderStatus);
                                }
                                catch { }
                                MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("Collection is not fully received yet, or you need permission to Close Sale Order without receiving fully Collection", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                    }
                    else
                    {
                        UsersRepo usersRepo = new UsersRepo();
                        saleOrder = saleOrderRepo.get(saleOrderid);

                        var row = saleOrder;

                        decimal budgetMarginOC = 0;
                        decimal actualMarginOC = 0;

                        decimal totalCFRValue = 0;
                        decimal TotalBudgetedMargin = 0;
                        decimal margin = 0;

                        decimal TotalActualMargin = 0;
                        decimal ActualMargin = 0;

                        if (row.totalCFRValue != 0)
                        {
                            totalCFRValue = Convert.ToDecimal(row.totalCFRValue);
                        }
                        if (row.margin != 0)
                        {
                            margin = Convert.ToDecimal(row.margin);
                        }
                        if (row.ActualMargin != 0)
                        {
                            ActualMargin = Convert.ToDecimal(row.ActualMargin);
                        }
                        if (row.CostSheet != null)
                        {

                            TotalBudgetedMargin = row.CostSheet.TotalBudgetedMargin;
                            TotalActualMargin = row.CostSheet.TotalActualMargin;
                        }

                        if (row.CostSheet != null)
                        {

                            budgetMarginOC = totalCFRValue - TotalBudgetedMargin;

                        }
                        else
                        {
                            budgetMarginOC = margin;
                        }
                        if (row.CostSheet != null)
                        {
                            actualMarginOC = totalCFRValue - TotalActualMargin;
                        }
                        else
                        {
                            actualMarginOC = ActualMargin;
                        }
                        if (row.saleOrderStatus != null)
                        {
                            oldStatus = row.saleOrderStatus;
                        }
                        SaleOrderss.ucStatuschange.inActiveStatuses = 1;

                        SaleOrderss.ucStatuschange.saleOrderid = (int)saleOrderid;
                        SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange(saleOrderRepo);
                        var myWindow = Window.GetWindow(this);
                        statusChange.Owner = myWindow;
                        statusChange.ShowDialog();
                        if (SaleOrderss.ucStatuschange.saleOrder.Id != 0)

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null) ? true : false)
                            {
                                SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = false;
                                SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing == null)
                                {
                                    SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                            }

                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                            {
                                SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing != true)
                                {
                                    SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                            }
                            else
                            {
                                SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;
                                usersRepo.Add(TransactionInfo.Closed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                            }
                        SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                        SaleOrderss.ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                        if (row.saleOrderStatus != SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus)
                            usersRepo.Add(TransactionInfo.Status_Changed, saleOrder.Id, (int)TransactionItemType.Sale_Order, "While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("SO has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), saleOrder.Id, TransactionItemType.Sale_Order);
                                win.ShowDialog();
                                tagUsers = win.tagUsers;
                                if (win.tagUsers.Count > 0)
                                {
                                    if (saleOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                    {
                                        saleOrder.holderChangeDate = DateTime.Now;
                                    }
                                    saleOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                    saleOrderRepo.update(saleOrder);
                                }
                                ccUsers = win.ccUsers;
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();

                            }

                        }
                        string oldStat = "";
                        if (oldStatus != null)
                        {
                            oldStat = oldStatus.Status;
                        }
                        string newStat = SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status;
                        string symbolCurr = "";
                        if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
                        {
                            if (row.costcenterCurrency != null)
                            {
                                symbolCurr = row.costcenterCurrency.Abbrivation.ToString();
                            }
                        }
                        else
                        {
                            if (row.currency != null)
                            {
                                symbolCurr = row.currency.Abbrivation.ToString();
                            }
                        }
                        CommentLog comment = new CommentLog();

                        if (saleOrder.Id != 0 && saleOrder.saleOrdertype == InquiryType.Principal && saleOrder.commision != 0)
                        {
                            comment.Comment = "Status of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                                   + "Comission(OC): " + saleOrder.commision.ToString() + " (" + symbolCurr + ")"
                                                   + "\n Net-Comissio(OC): " + saleOrder.netCommision.ToString() + " (" + symbolCurr + ")"
                                                   + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                            comment.Timestamp = DateTime.Now;
                            comment.Subject = "Status Changed using direct close";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                            if (saleOrder.StatusClass != null)
                            {
                                string oldStatClass = saleOrder.StatusClass.ClassName;

                                string newStatClass = checkStatusClass.ClassName;
                                if (checkStatusClass.Id != saleOrder.StatusClass.Id && saleOrder.StatusClass != null)
                                {
                                    comment.Comment = "Status and Status Class of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                     + "Comission(OC): " + saleOrder.commision.ToString() + " (" + symbolCurr + ")"
                                     + "\n Net-Comissio(OC): " + saleOrder.netCommision.ToString() + " (" + symbolCurr + ")"
                                     + "\nhas been changed \n"
                                     + "Status From: " + oldStat + " \nTo: " + newStat
                                     + "Status From: " + newStatClass + " \nTo: " + oldStatClass;                                   
                                    comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                    comment.Subject = "Status and Status Class Changed using direct close";
                                }
                            }
                            procurementRepo.Add(row.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {

                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {


                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                        }
                        else
                        {
                            comment = new CommentLog()
                            {
                                Comment = "Status of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                                 + "Budget Margin(OC): " + txtBudgetMargin.Text + " (" + symbolCurr + ")"
                                                 + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                                 + "\nActual Margin(OC): " + txtActualMargin.Text + " (" + symbolCurr + ")"
                                                 + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                                 + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat

                            };
                            comment.Timestamp = DateTime.Now;
                            comment.Subject = "Status Changed using Direct Close";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;

                            if (saleOrder.StatusClass != null)
                            {
                                string oldStatClass = saleOrder.StatusClass.ClassName;

                                string newStatClass = checkStatusClass.ClassName;
                                if (checkStatusClass.Id != saleOrder.StatusClass.Id && saleOrder.StatusClass != null)
                                {
                                    comment.Comment = "Status and Status Class of SO having SO Amount (OC): " + saleOrder.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                     + "Budget Margin(OC): " + txtBudgetMargin.Text + " (" + symbolCurr + ")"
                                     + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                     + "\nActual Margin(OC): " + txtActualMargin.Text + " (" + symbolCurr + ")"
                                     + "\nSystem Margin(OC): " + txtSystemMargin.Text + " (" + symbolCurr + ")"
                                     + "\nhas been changed \n"
                                     + "Status From: " + oldStat + "\n"
                                     + "To: " + newStat + "\n"
                                     + "Status Class From: " + newStatClass + " \n"
                                     + "To: " + oldStatClass + "\n";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                    comment.Subject = "Status and Status Class Changed using direct close";
                                }

                                procurementRepo.Add(row.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {

                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {


                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                    }
                                }
                            }
                        }

                       
                        try
                        {
                            row = SaleOrderss.ucStatuschange.saleOrder;

                            saleOrderRepo.updateStatusById(row.Id, row.saleOrderStatus);
                        }
                        catch { }
                        MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                    }

                    var thisWindow = Window.GetWindow(this);
                    thisWindow.Close();
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Close SaleOrder Directly.");
                }
            }
        }
        private void LookupSalesTax_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

            if ((lookupSalesTax.SelectedItem as TaxName).isManual != false)
            {
                txttax.IsReadOnly = false;
               
            }
            else
            {
                txttax.IsReadOnly = true;
               
            }

            var selectedTax = lookupSalesTax.SelectedItem as TaxName;
            if(selectedTax!=null)
            {
                saleOrderTax = selectedTax;
                if (selectedTax != null)
                {
                    saleOrderTax = selectedTax;
                    var fobValue = Convert.ToDouble(txtfob.Text);
                    txttax.Text = ((selectedTax.percentage * fobValue)/100).ToString();
                }
            }  
        }
        private void IsSalesTax_Unchecked(object sender, RoutedEventArgs e)
        {
            var zeroTax = 0;
            txttax.Text = zeroTax.ToString();
            
        }

        private void Txttotalfob_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (saleOrder?.SaleInvoices != null && saleOrder?.SaleInvoices.Count > 0)
            {
                var invoicedAmount = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(z => z.totalInvoiceAmount);
               invoicedAmount=  Math.Round(Convert.ToDouble (invoicedAmount), 2);
                var fobValue = Convert.ToDouble(txttotalfob.Text);
                txtSOremainingcfr.Text = (fobValue - invoicedAmount).ToString();
                if (saleOrder.Id != 0 && saleOrder.saleOrdertype == InquiryType.Principal && saleOrder.commision != 0)
                {
                    txtSOremainingcfr.Text = (Convert.ToDouble(saleOrder.commision) - invoicedAmount).ToString();
                }
                txtInvoicedAmount.Text = invoicedAmount.ToString();
            }
            else
            {
                if (saleOrder.Id != 0 && saleOrder.saleOrdertype == InquiryType.Principal && saleOrder.commision != 0)
                {
                    txtSOremainingcfr.Text = (Convert.ToDouble(saleOrder.commision)).ToString();
                }
                else
                {
                    var fobValue = Convert.ToDouble(txttotalfob.Text);
                    txtSOremainingcfr.Text = (fobValue).ToString();
                }
            }
           
        }

        private void BtnBudgertPunching_Click(object sender, RoutedEventArgs e)
        {
            btnCostSheet.IsEnabled = false;

            try
            {
                var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Sale Order?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
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

        private void view_InitNewRow1(object sender, InitNewRowEventArgs e)
        {
            
        }

        private void GrdSplitPER_KeyDown(object sender, KeyEventArgs e)
        {
        }
        public void calculateSystemMargins()
        {
            decimal Sototal = 0;
            if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC)
            {
                if(txtCostCenterAmount.Text != "NaN")
                Sototal = Convert.ToDecimal(txtCostCenterAmount.Text);
                decimal sumbudg = 0;
                if (!string.IsNullOrEmpty(txtSystemMargin.Text))

                    sumbudg = Convert.ToDecimal(txtSystemMargin.Text);
                if (Sototal != 0)
                {
                    var percent = (((sumbudg) / Sototal) * 100);
                    txtSystemPercent.Text = decimal.Round(percent, 2).ToString();
                    txtSystemPercent.Text += "%";
                }

                calculateCCMER();
                calculateCCSER();
            }
            else
            {
                Sototal = Convert.ToDecimal(txtSoAmount.Text);
                if ((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz|| (InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    if (txtSystemMargin.Text != "")
                    {
                        decimal sumbudg = 0;

                        sumbudg = Convert.ToDecimal(txtSystemMargin.Text);
                        if (Sototal != 0)
                        {
                            var percent = (((sumbudg) / Sototal) * 100);
                            txtSystemPercent.Text = decimal.Round(percent, 2).ToString();
                            txtSystemPercent.Text += "%";
                        }
                        decimal exchangeRate = 1;
                        if (txtexchangerate.Text != "")
                            exchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                        decimal marginexchangeRate = 1;
                        if (txtMarginexchangerate.Text != "")
                            marginexchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                        decimal margin = Convert.ToDecimal(txtSystemMargin.Text);
                        txtMarketSystemMargin.Text = (margin * exchangeRate).ToString();
                        txtSaleSystemMargin.Text = (marginexchangeRate * margin).ToString();
                    }
                }
                else
                {
                    if (txtSystemMargin.Text != "")
                    {
                        decimal sumbudg = 0;

                        sumbudg = Convert.ToDecimal(txtSystemMargin.Text);
                        if (Sototal != 0)
                        {
                            var percent = (((sumbudg) / Sototal) * 100);
                            txtSystemPercent.Text = decimal.Round(percent, 2).ToString();
                            txtSystemPercent.Text += "%";
                        }
                        decimal exchangeRate = 1;
                        if (txtexchangerate.Text != "")
                            exchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                        decimal marginexchangeRate = 1;
                        if (txtMarginexchangerate.Text != "")
                            marginexchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                        decimal margin = Convert.ToDecimal(txtSystemMargin.Text);
                        txtMarketSystemMargin.Text = (margin * exchangeRate).ToString();
                        txtSaleSystemMargin.Text = (marginexchangeRate * margin).ToString();
                    }
                }
            }
        }

        private void TxtSystemMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculateSystemMargins();
        }

        private void grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            if (((GridControl)sender).View.ActiveEditor != null)
            {
                e.Handled = true;
                TextBox tb = (TextBox)e.OriginalSource;
                var i = tb.CaretIndex;
                tb.Text += "\n";
                tb.CaretIndex = i + 1;
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (OrderId != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\SaleOrder\\ToUpload\\";
                        
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew1.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew1.ToolTip = "Uploading";
                            btnAttachNew1.IsEnabled = true;

                            btnAttachment1.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += OrderId + "_" + TransactionItemType.Sale_Order.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Sale_Order);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();

                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Sale_Order, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, saleOrder.Id, 3, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {

                                            treeViewAttachments1.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Sale_Order);
                                            imgAttachNew1.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew1.ToolTip = "Attach";
                                            btnAttachNew1.IsEnabled = true;
                                            btnAttachment1.Content = "Select";
                                        });
                                    }
                                });
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can attach document when SO Closed") != null)
                {

                    if (grdAttach1.Visibility == Visibility.Visible)
                        grdAttach1.Visibility = Visibility.Collapsed;
                    else
                    {
                        cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSOAttachmentCategories();
                        grdAttach1.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required" + " Can attach document when SO Closed!");
                }
            }
            else
            {
                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSOAttachmentCategories();
                    grdAttach1.Visibility = Visibility.Visible;
                }
            }
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {
                if (saleOrder.Id != 0)
                {
                    List<TreeItem> otherAttachments = new List<TreeItem>();
                   
                    List<TreeItem> atachments=SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)saleOrder.Id, TransactionItemType.Sale_Order);
                    if (saleOrder.offer != null)
                    {
                        if (saleOrder.offer_Id != null)
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)saleOrder.offer_Id, TransactionItemType.Offer));
                        if (saleOrder.offer.inquiry_Id != null)
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)saleOrder.offer.inquiry_Id, TransactionItemType.Inquiry));
                    }

                    if (saleOrder.SaleInvoices.Count != 0)
                    {
                        foreach (var invoice in saleOrder.SaleInvoices)
                        {
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)invoice.Id, TransactionItemType.Sale_Invoice));
                            if(invoice.salesReceipts.Count!=0)
                            foreach (var receipt in invoice.salesReceipts)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)receipt.transactionGroupId, TransactionItemType.Sale_Receipt));
                            }
                        }
                    }
                    if (saleOrder.PurchaseOrders.Count != 0)
                    {
                        foreach (var pO in saleOrder.PurchaseOrders)
                        {
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)pO.Id, TransactionItemType.Purchase_Order));
                            if(pO.PurchaseInvoices.Count!=0)
                            foreach (var pI in pO.PurchaseInvoices)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)pI.Id, TransactionItemType.Purchase_Invoice));
                                    if (pI.Payments.Count != 0)
                                        foreach (var payment in pI.Payments)
                                        {
                                            otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                        }
                            }
                        }
                    }
                    if (saleOrder.Bills.Count != 0)
                    {
                        foreach (var bill in saleOrder.Bills)
                        {
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                            if (bill.Payments.Count != 0)
                                foreach (var payment in bill.Payments)
                                {
                                    otherAttachments.AddRange(SYSTEM_STATIC.GetSOAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                }
                        }
                    }

                    foreach (var cat in atachments)
                    {
                        foreach (var otherCat in otherAttachments)
                        {
                            if (otherCat.name == cat.name)
                            {
                                foreach (var file in otherCat.Items)
                                {
                                    cat.Items.Add(file);
                                }
                            }
                        }
                    }
                    treeViewAttachments1.ItemsSource = atachments;
                }
                grdAttachments1.Visibility = Visibility.Visible;

            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void loadonIBTdata()
        {
      
            grdInvoiceProgressBar.Visibility = Visibility.Collapsed;
            datsaleOrderdate.DateTime = System.DateTime.Now;
            datpoCreationdate.EditValue = System.DateTime.Now;
            if (interBankTransfer.currency_Id != 0 && interBankTransfer.currency != null)
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == interBankTransfer.currency_Id)
                    {
                        cmbCurrency.SelectedItem = cmbitem;
                        break;
                    }
                }
            else
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == interBankTransfer.company.CurrencyId)
                    {
                        cmbCurrency.SelectedItem = cmbitem;
                        break;
                    }

                }
            // Select Company
            if (interBankTransfer.company_Id != null || interBankTransfer.company != null)
            {
                company = interBankTransfer.company;

                lookupCompany.Text = interBankTransfer.company.CompanyName;
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }
            // Select Department
            if (interBankTransfer.dept_Id != 0 || interBankTransfer.department != null)
            {
                lookupDepartment.Text = interBankTransfer.department.DeptName;
                department = interBankTransfer.department;
                //lookupCustomer.ItemsSource = department.customers;
                loademployees();
            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }
            // Select Offer Status 
            foreach (cmbitem cmbitem in cmbSaleOrderStatus.Items)
            {
                if (cmbitem.name == interBankTransfer.interBankTransStatus.Status)
                {
                    cmbSaleOrderStatus.SelectedItem = cmbitem;
                    break;
                }
            }
            // load Products in Inquiry to grid
            List<DataGridItem> datagriditems = new List<DataGridItem>();
            if (interBankTransfer.products != null)
            {
                List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                foreach (var procurementProduct in interBankTransfer.products)
                {
                    procurementProducts.Add(new ProcurementProduct()
                    {
                        Id = procurementProduct.Id,

                        inquiryProduct = new InquiryProduct()
                        {
                            Id = procurementProduct.inquiryProduct.Id,
                            ownDiscription = procurementProduct.inquiryProduct.ownDiscription, 
                            UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                            quantity = procurementProduct.inquiryProduct.quantity,
                            Weight = procurementProduct.inquiryProduct.Weight,
                            product = new Product()
                            {
                                Id = procurementProduct.inquiryProduct.product.Id,
                                categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                item = procurementProduct.inquiryProduct.product.item,
                                code = procurementProduct.inquiryProduct.product.code,
                                itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                nature = procurementProduct.inquiryProduct.product.nature,
                                category = procurementProduct.inquiryProduct.product.category,
                                isActive = procurementProduct.inquiryProduct.product.isActive
                            }
                                ,
                            product_Id = procurementProduct.inquiryProduct.product.Id

                        },
                        product_Id = procurementProduct.inquiryProduct.Id,
                        unitPrice = procurementProduct.unitPrice,

                        value1 = procurementProduct.value2,
                        value2 = procurementProduct.value2,
                        caption1 = cmbcaption1.Text.Trim(),
                        caption2 = cmbcaption2.Text.Trim()


                    });
                }

                grdSOItems.ItemsSource = procurementProducts;
            }
            else
            {
                grdSOItems.ItemsSource = procurementProducts;
            }

            // selected currency of company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
           
                if (cmbitem.id == interBankTransfer.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
        }

        private void btnLinkedSO_Click(object sender, RoutedEventArgs e)
        {
            if (OrderId != 0)
            {
                frmMergeMoudlesTracking trackingWindow = new frmMergeMoudlesTracking(OrderId, TransactionItemType.Sale_Order, true);
                trackingWindow.ShowDialog();
            }
        }

        private void txttax_TextChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txttax.Text != "")
            {
                calculatetotal();
            }                                        

        }
        private void view1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
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
                LoadCustomerProfile("All");
                SlblCustomerProfileBySO.Text = "Customer Profile By SO (All)";
                btnCustomerProfileBySo.Content = "Customer Profile By SO (All)";
                LoadVendorProfile("All");
                //lblVendorProfileByPO.Text = "Vendor Profile By PO (Open)";
                btnVendorProfileByPO.Content = "Vendor Profile By PO (All)";

            }
            catch (Exception)
            {
            }
        }

        private void grdCustomerProfileBySO_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdCustomerProfileBySO.SelectedItem != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, (grdCustomerProfileBySO.SelectedItem as PendingInvoice).Id);

                procurmentPanel.Show();

            }
        }

       

        private void btnSOTracking_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Customer Tracking By SO") != null)
            {
                if (gridCustomerProfileByCustomer.Visibility == Visibility.Collapsed)
                {
                    gridCustomerProfileByCustomer.Visibility = Visibility.Visible;
                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCustomerProfileBySO);

                }
                else
                {
                    gridCustomerProfileByCustomer.Visibility = Visibility.Collapsed;

                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Customer Tracking By SO");

            }
        }
        private void btnCustomerProfileBySo_Click(object sender, EventArgs e)
        {

        }
        private void btnVendorProfileByPO_Click(object sender, EventArgs e)
        {

        }

        private void grdVendorProfileByPO_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdVendorProfileByPO.SelectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (grdVendorProfileByPO.SelectedItem as PendingInvoice).Id);
                procurmentPanel.Show();
            }
        }

        private void lookupSelectVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LoadVendorProfile("Open");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (Open)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Open)";
        }

        private void btnPOTracking_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Tracking By PO") != null)
            {
                if (gridVendorProfileByPO.Visibility == Visibility.Collapsed)
                {
                    gridVendorProfileByPO.Visibility = Visibility.Visible;
                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdVendorProfileByPO);

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

        private void btnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnSavePOLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdVendorProfileByPO);

        }

        private void btnSaveSOLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCustomerProfileBySO);

        }

        private void cmbSoProfileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            //LoadCustomerProfile();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

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
                            if ( lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetOpenPurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id, SYSTEM_STATIC.currentUser.id);
                                if (poInvoicesByVendor != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        var invoicedAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(purchaseInvoice.totalCFRValue + purchaseInvoice.totaltaxAmount - invoicedAmount, 2);
                                        var paidAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));
                                        var unPaidAmount = Math.Round(invoicedAmount - paidAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.paidAmount = paidAmount;
                                        pInvoice.unPaidAmount = unPaidAmount;



                                        pInvoice.amountOC = purchaseInvoice.totalCFRValue;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        pInvoice.department = purchaseInvoice.department.DeptName;
                                        pInvoice.stage = purchaseInvoice.stage;
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;

                                        pInvoice.company = purchaseInvoice.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoices;
                                }
                            }

                            break;
                        }
                    case "Close":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetClosePurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id, SYSTEM_STATIC.currentUser.id);
                                if (poInvoicesByVendor != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        var invoicedAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount= Math.Round(purchaseInvoice.totalCFRValue + purchaseInvoice.totaltaxAmount - invoicedAmount, 2);
                                        var paidAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));
                                        var unPaidAmount = Math.Round(invoicedAmount - paidAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.paidAmount = paidAmount;
                                        pInvoice.unPaidAmount = unPaidAmount;



                                        pInvoice.amountOC = purchaseInvoice.totalCFRValue;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        pInvoice.department = purchaseInvoice.department.DeptName;
                                        pInvoice.stage = purchaseInvoice.stage;
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;
                                   
                                        pInvoice.company = purchaseInvoice.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "All":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetAllPurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id, SYSTEM_STATIC.currentUser.id);
                                if (poInvoicesByVendor != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        var invoicedAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(purchaseInvoice.totalCFRValue + purchaseInvoice.totaltaxAmount - invoicedAmount, 2);
                                        var paidAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));
                                        var unPaidAmount = Math.Round(invoicedAmount - paidAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.paidAmount = paidAmount;
                                        pInvoice.unPaidAmount = unPaidAmount;



                                        pInvoice.amountOC = purchaseInvoice.totalCFRValue;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        pInvoice.department = purchaseInvoice.department.DeptName;
                                        pInvoice.stage = purchaseInvoice.stage;
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;

                                        pInvoice.company = purchaseInvoice.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "Pending for Approval":
                        {
                            if (lookupSelectVendor.SelectedIndex > -1)
                            {
                                var poInvoicesByVendor = saleOrderRepo.GetPendingForApprovalPurchaseInvoicesByVendor((lookupSelectVendor.SelectedItem as Vendor).Id, SYSTEM_STATIC.currentUser.id);
                                if (poInvoicesByVendor != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var purchaseInvoice in poInvoicesByVendor)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = purchaseInvoice.Id;
                                        pInvoice.refNo = purchaseInvoice.POReferenceNo;
                                        pInvoice.currencyName = purchaseInvoice.currency.CurrencyName;
                                        var invoicedAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(purchaseInvoice.totalCFRValue + purchaseInvoice.totaltaxAmount - invoicedAmount, 2);
                                        var paidAmount = purchaseInvoice.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));
                                        var unPaidAmount = Math.Round(invoicedAmount - paidAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.paidAmount = paidAmount;
                                        pInvoice.unPaidAmount = unPaidAmount;



                                        pInvoice.amountOC = purchaseInvoice.totalCFRValue;
                                        pInvoice.vendor = purchaseInvoice.vendors[0].company.CompanyName;
                                        pInvoice.department = purchaseInvoice.department.DeptName;
                                        pInvoice.stage = purchaseInvoice.stage;
                                        pInvoice.PurchaseInvoiceStatus = purchaseInvoice.PurchaseOrderStatus;

                                        pInvoice.company = purchaseInvoice.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdVendorProfileByPO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                }

            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void LoadCustomerProfile(string _type)
        {
            var type = _type;
            if (!string.IsNullOrEmpty( type))
            {
                switch (type)
                {
                    case "Open":
                        {
                            if (lookupCustomer.SelectedIndex > -1 && saleOrder.CostSheet_Id != null)
                            {
                                var soInvoicesByCustomer = saleOrderRepo.GetOpenSaleInvoicesByCustomer((lookupCustomer.SelectedItem as CustomerCompany).Id, SYSTEM_STATIC.currentUser.id);
                                if (soInvoicesByCustomer != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var saleOrder in soInvoicesByCustomer)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = saleOrder.Id;
                                        pInvoice.refNo = saleOrder.referenceNo;
                                        pInvoice.currencyName = saleOrder.currency.CurrencyName;
                                        var invoicedAmount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(saleOrder.totalCFRValue + saleOrder.totaltaxAmount - invoicedAmount, 2);
                                        var collectedAmount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.salesReceipts.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                                        var pendingCollectedAmount = Math.Round(invoicedAmount - collectedAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.collectedAmount = collectedAmount;
                                        pInvoice.pendingCollectionAmount = pendingCollectedAmount;


                                        pInvoice.customer = saleOrder.customerCompany.company.CompanyName;
                                        pInvoice.amountOC = saleOrder.totalCFRValue;
                                        pInvoice.vendor = saleOrder.vendors[0].company.CompanyName;
                                        pInvoice.department = saleOrder.department.DeptName;
                                        pInvoice.stage = saleOrder.stage;
                                        pInvoice.saleInvoiceStatus = saleOrder.saleOrderStatus;

                                        pInvoice.company = saleOrder.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);

                                        if (saleOrder.PaymentDueAgeing != null)
                                        {
                                            DateTime dateTime = Convert.ToDateTime(saleOrder.PaymentDueAgeing);
                                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                                            pInvoice.agingDays = NoDueAgeingDays;
                                        }

                                        allPendingInvoices.Add(pInvoice);
                                    }
                                  allPendingInvoices = allPendingInvoices
                                                                  .GroupBy(p => p.Id)
                                                                  .Select(g => g.First())
                                                                  .ToList();
                                    grdCustomerProfileBySO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "Close":
                        {
                            if (lookupCustomer.SelectedIndex > -1 && saleOrder.CostSheet_Id != null)
                            {
                                var soInvoicesByCustomer = saleOrderRepo.GetCloseSaleInvoicesByCustomer((lookupCustomer.SelectedItem as CustomerCompany).Id, SYSTEM_STATIC.currentUser.id);
                                if (soInvoicesByCustomer != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var saleOrder in soInvoicesByCustomer)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = saleOrder.Id;
                                        pInvoice.refNo = saleOrder.referenceNo;
                                        pInvoice.currencyName = saleOrder.currency.CurrencyName;
                                        var invoicedAmount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(saleOrder.totalCFRValue + saleOrder.totaltaxAmount - invoicedAmount, 2);
                                        var collectedAmount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.salesReceipts.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                                        var pendingCollectedAmount = Math.Round(invoicedAmount - collectedAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.collectedAmount = collectedAmount;
                                        pInvoice.pendingCollectionAmount = pendingCollectedAmount;



                                        pInvoice.amountOC = saleOrder.totalCFRValue;
                                        pInvoice.vendor = saleOrder.vendors[0].company.CompanyName;
                                        pInvoice.department = saleOrder.department.DeptName;
                                        pInvoice.stage = saleOrder.stage;
                                        pInvoice.saleInvoiceStatus = saleOrder.saleOrderStatus;
                                        pInvoice.customer = saleOrder.customerCompany.company.CompanyName;

                                        pInvoice.company = saleOrder.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);

                                        if (saleOrder.PaymentDueAgeing != null)
                                        {
                                            DateTime dateTime = Convert.ToDateTime(saleOrder.PaymentDueAgeing);
                                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                                            pInvoice.agingDays = NoDueAgeingDays;
                                        }

                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdCustomerProfileBySO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "All":
                        {
                            if (lookupCustomer.SelectedIndex > -1 && saleOrder.CostSheet_Id != null)
                            {
                                var soInvoicesByCustomer = saleOrderRepo.GetAllSaleInvoicesByCustomer((lookupCustomer.SelectedItem as CustomerCompany).Id, SYSTEM_STATIC.currentUser.id);
                                if (soInvoicesByCustomer != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var saleOrder in soInvoicesByCustomer)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = saleOrder.Id;
                                        pInvoice.refNo = saleOrder.referenceNo;
                                        pInvoice.currencyName = saleOrder.currency.CurrencyName;
                                        var invoicedAmount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(saleOrder.totalCFRValue + saleOrder.totaltaxAmount - invoicedAmount, 2);
                                        var collectedAmount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.salesReceipts.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                                        var pendingCollectedAmount = Math.Round(invoicedAmount - collectedAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.collectedAmount = collectedAmount;
                                        pInvoice.pendingCollectionAmount = pendingCollectedAmount;



                                        pInvoice.amountOC = saleOrder.totalCFRValue;
                                        pInvoice.vendor = saleOrder.vendors[0].company.CompanyName;
                                        pInvoice.department = saleOrder.department.DeptName;
                                        pInvoice.stage = saleOrder.stage;
                                        pInvoice.saleInvoiceStatus = saleOrder.saleOrderStatus;
                                        pInvoice.customer = saleOrder.customerCompany.company.CompanyName;

                                        pInvoice.company = saleOrder.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);

                                        if (saleOrder.PaymentDueAgeing != null)
                                        {
                                            DateTime dateTime = Convert.ToDateTime(saleOrder.PaymentDueAgeing);
                                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                                            pInvoice.agingDays = NoDueAgeingDays;
                                        }

                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdCustomerProfileBySO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "Pending for Approval":
                        {
                            if (lookupCustomer.SelectedIndex > -1 && saleOrder.CostSheet_Id != null)
                            {
                                var soInvoicesByCustomer = saleOrderRepo.GetPendingForApprovalSaleInvoicesByCustomer((lookupCustomer.SelectedItem as CustomerCompany).Id, SYSTEM_STATIC.currentUser.id);
                                if (soInvoicesByCustomer != null)
                                {
                                    List<PendingInvoice> allPendingInvoices = new List<PendingInvoice>();
                                    foreach (var saleOrder in soInvoicesByCustomer)
                                    {
                                        PendingInvoice pInvoice = new PendingInvoice();
                                        pInvoice.Id = saleOrder.Id;
                                        pInvoice.refNo = saleOrder.referenceNo;
                                        pInvoice.currencyName = saleOrder.currency.CurrencyName;
                                        var invoicedAmount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);
                                        var unInvoicedAmount = Math.Round(saleOrder.totalCFRValue + saleOrder.totaltaxAmount - invoicedAmount, 2);
                                        var collectedAmount = saleOrder.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.salesReceipts.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                                        var pendingCollectedAmount = Math.Round(invoicedAmount - collectedAmount, 2);

                                        pInvoice.invoiceAmount = invoicedAmount;
                                        pInvoice.uninvoiceAmount = unInvoicedAmount;
                                        pInvoice.collectedAmount = collectedAmount;
                                        pInvoice.pendingCollectionAmount = pendingCollectedAmount;


                                        pInvoice.customer = saleOrder.customerCompany.company.CompanyName;

                                        pInvoice.amountOC = saleOrder.totalCFRValue;
                                        pInvoice.vendor = saleOrder.vendors[0].company.CompanyName;
                                        pInvoice.department = saleOrder.department.DeptName;
                                        pInvoice.stage = saleOrder.stage;
                                        pInvoice.saleInvoiceStatus = saleOrder.saleOrderStatus;

                                        pInvoice.company = saleOrder.company.CompanyName;
                                        allPendingInvoices.Add(pInvoice);

                                        if (saleOrder.PaymentDueAgeing != null)
                                        {
                                            DateTime dateTime = Convert.ToDateTime(saleOrder.PaymentDueAgeing);
                                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                                            pInvoice.agingDays = NoDueAgeingDays;
                                        }

                                        allPendingInvoices.Add(pInvoice);
                                    }
                                    allPendingInvoices = allPendingInvoices
                                                               .GroupBy(p => p.Id)
                                                               .Select(g => g.First())
                                                               .ToList();
                                    grdCustomerProfileBySO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                }
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }   
      

        private void btnSaveSOLayout_Click(object sender, EventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCustomerProfileBySO);

        }

    

        private void btnOpenCustomerProfile_Click(object sender, EventArgs e)
        {
            LoadCustomerProfile("Open");
            SlblCustomerProfileBySO.Text = "Customer Profile By SO (Open)";
            btnCustomerProfileBySo.Content = "Customer Profile By SO (Open)";
        }

        private void btnCloseCustomerProfile_Click(object sender, EventArgs e)
        {
            LoadCustomerProfile("Close");
            SlblCustomerProfileBySO.Text = "Customer Profile By SO (Close)";
            btnCustomerProfileBySo.Content = "Customer Profile By SO (Close)";

        }

        private void btnPendingForApprovalCustomerProfile_Click(object sender, EventArgs e)
        {
            LoadCustomerProfile("Pending for Approval");
            SlblCustomerProfileBySO.Text = "Customer Profile By SO (Pending for Approval)";
            btnCustomerProfileBySo.Content = "Customer Profile By SO (Pending for Approval)";

        }

        private void btnAllCustomerProfile_Click(object sender, EventArgs e)
        {
            LoadCustomerProfile("All");
            SlblCustomerProfileBySO.Text = "Customer Profile By SO (All)";
            btnCustomerProfileBySo.Content = "Customer Profile By SO (All)";

        }

        private void cmbPoProfileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            //LoadVendorProfile();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void btnSavePOLayout_Click(object sender, EventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdVendorProfileByPO);

        }

        private void btnOpenVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Open");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (Open)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Open)";
        }

        private void btnCloseVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Close");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (Close)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Close)";
        }

        private void btnPendingForApprovalVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Pending for Approval");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (Pending for Approval)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Pending for Approval)";
        }

        private void btnAllVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("All");
            //lblVendorProfileByPO.Text = "Vendor Profile By PO (All)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (All)";
        }

        private void MbtnResendComment_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Resend_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Resend_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            var comment = grdCommentss.SelectedItem as CommentLog;


            if (comment != null)
            {
                comment = procurementRepo.GetComment(comment.Id);
                if (comment.employee.EmployeeUsers.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) == null)
                {
                    DXMessageBox.Show("Only sender of this comment can change Flag!");
                    return;
                }
                if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Sale_Order);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Order);
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

                if (frmInputBox.commentAdded == true && saleOrder.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);

                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
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

        private void btnLinkBudget_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Link SO with Budget") != null)
            {
                if (saleOrder.Budget_Id != null)
                {
                    winSelectBudget budget = new winSelectBudget(company.Id, department.Id);
                    budget.budget = saleOrder.BudgetCostSheet;
                    budget.ShowDialog();
                    saleOrder.Budget_Id = budget.budgetId;
                }
                else
                {
                    winSelectBudget budget = new winSelectBudget(company.Id, department.Id);
                    budget.ShowDialog();
                    saleOrder.Budget_Id = budget.budgetId;
                }

            }
            else
            {
                DXMessageBox.Show("You don't have permission to Link SO with Budget");

            }
        }
        private void btnBudgetRSBC_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can revised budget from SO") != null)
            {
                if (saleOrder.Budget_Id != null)
                {
                    winAddBudgetRSBC rsbc = new winAddBudgetRSBC((int)saleOrder.Budget_Id, Convert.ToDouble(txtRevisedAmount.Text));
                    rsbc.Show();
                }
                else
                {
                    DXMessageBox.Show("Please Link budget with this Sale Order first");
                    return;
                }
            }
            else
            {
                DXMessageBox.Show("You don't have permission to revised budget from SO");
            }
        }



        private void btnPerformanceSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Performance Sheet") != null)
                {
                    winPerformanceSheet sheet = new winPerformanceSheet(saleOrder);
                    winPerformanceSheet.efficencySheet = new PerformanceSheet();
                    sheet.ShowDialog();

                    saleOrder.PerformanceSheet = winPerformanceSheet.efficencySheet;

                    if (saleOrder.PerformanceSheet != null)
                    {

                        EmployeeRepo repo = new EmployeeRepo();
                        var staffLevelOne = repo.GetEmployee((int)saleOrder.PerformanceSheet.staffLevelOneId);
                        var staffLevelTwo = repo.GetEmployee((int)saleOrder.PerformanceSheet.staffLevelTwoId);
                        var supervisedBy = repo.GetEmployee((int)saleOrder.PerformanceSheet.supervoisedId);
                        txtAllocatedTo.Text = supervisedBy.person.FName + " " + supervisedBy.person.LName;
                        if (supervisedBy.person.Photo != null)
                            imgAllocatedTo.ImageSource = GetBitmapImageFromByteArray(supervisedBy.person.Photo);
                        txtSupervisedBy.Text = staffLevelOne.person.FName + " " + staffLevelOne.person.LName;
                        if (staffLevelOne.person.Photo != null)
                            imgSupervisedBy.ImageSource = GetBitmapImageFromByteArray(staffLevelOne.person.Photo);
                        txtDepartmentHead.Text = staffLevelTwo.person.FName + " " + staffLevelTwo.person.LName;
                        if (staffLevelTwo.person.Photo != null)
                            imgDepartmentHead.ImageSource = GetBitmapImageFromByteArray(staffLevelTwo.person.Photo);
                        txtTotalReceivedPoints.Text = saleOrder.PerformanceSheet.totalPoints.ToString();
                        txtTotalPointsPerc.Text = saleOrder.PerformanceSheet.totalPointsPerc.ToString();
                    }

                }
                else
                {
                    DXMessageBox.Show("You don't have permission to View Performance Sheet");
                }
            }
            catch (Exception)
            {

            }
        }
        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        private void btnLinkBudget_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnPerforSheet_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Performance Sheet") != null)
            {
                if (gridPerformanceSheet.Visibility == Visibility.Collapsed)
                {
                    gridPerformanceSheet.Visibility = Visibility.Visible;

                }
                else
                {
                    gridPerformanceSheet.Visibility = Visibility.Collapsed;

                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View Performance Sheet");

            }
        }

        private void btnGenerateRef_Click(object sender, RoutedEventArgs e)
        {
            

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Generate SO Reference key") != null)
            {
                ucGenerateSoKey key = new ucGenerateSoKey(saleOrderid);
                key.ShowDialog();
                saleOrder.referenceKey = key.key;

            }
            else
            {
                DXMessageBox.Show("You don't have permission to Generate SO Reference key");

            }
        }

        private void btnFetchRef_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to create link SO? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Create Sale order from reference key") != null)
                {
                    ucSelectParentSOKey ucSelectParentSOKey = new ucSelectParentSOKey();
                    ucSelectParentSOKey.ShowDialog();
                    if (ucSelectParentSOKey.soReftId != 0)
                    {

                        saleOrder.ParentSO_Id= ucSelectParentSOKey.soReftId;
                        DXMessageBox.Show("Reference key has been fetched successfully!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


                    }
                }
                else
                {
                    DXMessageBox.Show("You do not have permission Create Sale order from reference ke", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, 0);
                procurmentPanel.Show();
            }
        }

        private void btnStageTracking_Click(object sender, RoutedEventArgs e)
        {
            if (gridOrderStageTrack.Visibility == Visibility.Collapsed)
            {
                gridOrderStageTrack.Visibility = Visibility.Visible;
            }
            else
            {
                gridOrderStageTrack.Visibility = Visibility.Collapsed;
            }
        }
        public void GellAllOrdersTracking()
        {
            trackingOrder = saleOrderRepo.get(saleOrder.Id);
            if (trackingOrder != null)
            {
                OrderTracking tracking = new OrderTracking();
                var orders= tracking.getTransactions(trackingOrder.Id, TransactionItemType.Sale_Order);
                grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.Sale_Order);
                grdStatusTracking.ItemsSource = orders;
            }
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
                        AdvanceRepo advanceRepo = new AdvanceRepo();


                        var advance = advanceRepo.GetLoansAdvance(item.Id);


                        switch (advance.loansAdvanceType)
                        {

                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") != null)
                                {
                                    ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                                    frmLoansAdvances.loansAdvanceId = advance.Id;
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
                                    frmBillLoansAdvance.loansAdvanceId = advance.Id;
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

                        frmBillAdd.bills = billsRepo.GetBillsByGroupId(bill.transactionGroupId); 

                        if (frmBillAdd.bills.Count > 0)
                        {
                            if (frmBillAdd.bills[0].BillStatus.isActive == false)
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                {
                                    frmBillAdd.editFlag = true;
                                    frmBillAdd.groupId = bill.transactionGroupId;
                                    frmBill.Content = frmBillAdd;
                                    
                                    frmBill.WindowState = WindowState.Maximized;
                   
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

                                frmBill.WindowState = WindowState.Maximized;
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

        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(saleOrder.Id!=0)
            {
                if (cmbTransactionHolder.SelectedItem != null)
                {

                    if (saleOrder.transactionHolderId != (cmbTransactionHolder.SelectedItem as cmbitem).id)
                    {
                        datHolderDate.EditValue = DateTime.Now;
                    }
                    else
                    {
                        datHolderDate.EditValue = saleOrder.holderChangeDate;
                    }
                }
            }
            else
            {
                datHolderDate.EditValue = DateTime.Now;
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
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
                            //paymentRepo = new PaymentRepo();
                            //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
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
                            if (saleReceipt.payment != null)
                            {
                                ucFrmDirectReceiptPayment frmLAreceipt = new ucFrmDirectReceiptPayment();
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
                                        frmPiPaymentWindow.Title = "Direct Receipts";
                                        frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Direct Receipts!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmLAreceipt.editFlag = true;
                                    frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                                    frmPiPaymentWindow.Content = frmLAreceipt;
                                    frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPiPaymentWindow.Title = "Direct Receipts";
                                    frmPiPaymentWindow.Show();
                                }
                            }
                            else
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



                //if (saleReceipt.receiptType == ReceiptType.Loans_Advances)
                //{
                //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Sale Receipts") != null)
                //    {
                //        ucFrmLoansAdvanceSaleReceiptAdd frmLAreceipt = new ucFrmLoansAdvanceSaleReceiptAdd();
                //        //paymentRepo = new PaymentRepo();
                //        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;
                //        Window frmPiPaymentWindow = new Window();
                //        if (saleReceipt.saleReceiptStatus.isActive == false)
                //        {
                //            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Receipts") != null)
                //            {
                //                frmLAreceipt.editFlag = true;
                //                frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                //                frmPiPaymentWindow.Content = frmLAreceipt;
                //                frmPiPaymentWindow.WindowState = WindowState.Maximized;
                //                frmPiPaymentWindow.Title = "Sale Receipts";
                //                frmPiPaymentWindow.Show();
                //            }
                //            else
                //            {
                //                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Sale Receipts!");
                //                return;
                //            }
                //        }
                //        else
                //        {
                //            frmLAreceipt.editFlag = true;
                //            frmLAreceipt.groupId = saleReceipt.transactionGroupId;
                //            frmPiPaymentWindow.Content = frmLAreceipt;
                //            frmPiPaymentWindow.WindowState = WindowState.Maximized;
                //            frmPiPaymentWindow.Title = "Sale Receipts";
                //            frmPiPaymentWindow.Show();
                //        }

                //    }
                //    else
                //    {
                //        DXMessageBox.Show("Permission required to View Purchase Invoice Receipts!");
                //    }
                //}
                //else
                //{
                //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Receipt") != null)
                //    {
                //        ucFrmSaleReceipt updateSaleReceiptObj = new ucFrmSaleReceipt();
                //        updateSaleReceiptObj.saveEditFlag = 1;


                //        if (saleReceipt == null)
                //        {
                //            return;
                //        }

                //        if (saleReceipt.saleReceiptStatus != null)
                //        {
                //            var status = repo.GetSaleReceiptStatus(saleReceipt.saleReceiptStatus.Id);
                //            updateSaleReceiptObj.selectedStatus = status;
                //        }

                //        updateSaleReceiptObj.dateEditcreationDate.DateTime = saleReceipt.CreationDate;
                //        updateSaleReceiptObj.txtSystemRef.Text = saleReceipt.SystemRefNo;
                //        updateSaleReceiptObj.txtReceiptRef.Text = saleReceipt.ReceiptRefNo;
                //        updateSaleReceiptObj.txtCollectionAmnt.Text = saleReceipt.CollectionAmount.ToString();

                //        updateSaleReceiptObj.invoiceNo = saleReceipt.saleInvoice == null ? 0 : saleReceipt.saleInvoice.Id;
                //        updateSaleReceiptObj.groupId = saleReceipt.transactionGroupId;
                //        updateSaleReceiptObj.receiptId = saleReceipt.Id;
                //        updateSaleReceiptObj.oldStatus = saleReceipt.saleReceiptStatus;
                //        updateSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanMinimize;
                //        updateSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                //        updateSaleReceiptObj.enter_receipt_win.Title = "Update Sale Receipt";
                //        updateSaleReceiptObj.enter_receipt_win.Content = updateSaleReceiptObj;
                //        updateSaleReceiptObj.enterReceiptWindowFlag = true;

                //        if (saleReceipt.CreditedDate != null)
                //            updateSaleReceiptObj.dateEditcreditedDate.EditValue = (DateTime)saleReceipt.CreditedDate;

                //        if (saleReceipt.DepositedDate != null)
                //            updateSaleReceiptObj.datDepositedDate.EditValue = (DateTime)saleReceipt.DepositedDate;

                //        if (saleReceipt.InstrumentDate != null)
                //            updateSaleReceiptObj.datInstrumentDate.EditValue = (DateTime)saleReceipt.InstrumentDate;

                //        if (saleReceipt.InstrumentNo != null)
                //            updateSaleReceiptObj.txtInstrumentNo.Text = saleReceipt.InstrumentNo;

                //        updateSaleReceiptObj.enter_receipt_win.ShowDialog();
                //        //Load_Receipts();
                //    }
                //    else
                //    {
                //        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to view Sale Receipt!");
                //        return;
                //    }
                //}



            }
            catch
            {

            }
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

        private void txtSoAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CalculateCostCenterAmount();
        }
        public void CalculateCostCenterAmount()
        {
            txtCostCenterAmount.Text = (Convert.ToDouble(txtSoAmount.Text) * Convert.ToDouble(txtCostCenterExchangeRate.Text)).ToString();

        }

        private void txtCostCenterExchangeRate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CalculateCostCenterAmount();

        }

        private void cmbCostCenterCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calculateCCSER();
            calculateCCMER();
        }
        public void calculateCCSER()
        {
            if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 && !string.IsNullOrEmpty(txtBudgetMargin.Text))
            {
                CurrencyRepo currencyRepo = new CurrencyRepo();
                DateTime creationDate = (DateTime)datpoCreationdate.EditValue;
                DateTime d1 = new DateTime(2016, 01, 01);
                ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
                ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;
                double SER = 0, MER = 0;
                if (creationDate > d1)
                {

                    var exchangeRateGroupSER = exchangeRateGroupRepo.GetGroupByCurrenciesSER((cmbCostCenterCurrency.SelectedItem as cmbitem).id, (cmbbaseCurrency.SelectedItem as cmbitem).id, creationDate.Year);
                    var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbCostCenterCurrency.SelectedItem as cmbitem).id, (cmbbaseCurrency.SelectedItem as cmbitem).id, creationDate.Year);
                    if (exchangeRateGroupSER != null)
                    {
                        exchangeRate = exchangeRateGroupSER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                        switch (creationDate.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateJan;
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateFeb;
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateMar;
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateApr;
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateMay;
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateJun;
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateJul;
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateAug;
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateSep;
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateOct;
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateNov;
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    SER = exchangeRate.rateDec;
                                break;
                            default:
                                if (exchangeRate != null)
                                    SER = 0;
                                break;
                        }
                    }
                    else
                    {
                        SER = 1;

                    }
                    txtCCSER.Text = SER.ToString();
                }
            }
        }
        public void calculateCCMER()
        {
            if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 )
            {
                CurrencyRepo currencyRepo = new CurrencyRepo();
                DateTime creationDate = (DateTime)datpoCreationdate.EditValue;
                DateTime d1 = new DateTime(2016, 01, 01);
                ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
                ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;
                double  MER = 0;
                if (creationDate > d1)
                {
                    var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbCostCenterCurrency.SelectedItem as cmbitem).id, (cmbbaseCurrency.SelectedItem as cmbitem).id, creationDate.Year);                  
                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                        switch (creationDate.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateJan;
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateFeb;
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateMar;
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateApr;
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateMay;
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateJun;
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateJul;
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateAug;
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateSep;
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateOct;
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateNov;
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    MER = exchangeRate.rateDec;
                                break;
                            default:
                                if (exchangeRate != null)
                                    MER = 0;
                                break;
                        }
                    }
                    else
                    {
                        MER = 1;
                    }
                    txtCCMER.Text = MER.ToString();
                }
            }
        }
        private void txtCCSER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 && !string.IsNullOrEmpty(txtBudgetMargin.Text))
            {
                txtSaleBudgetMargin.Text = (Convert.ToDouble(txtBudgetMargin.Text) * Convert.ToDouble(txtCCSER.Text)).ToString();
            }
            if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 && !string.IsNullOrEmpty(txtActualMargin.Text))
            {
                txtSaleAMargin.Text = (Convert.ToDouble(txtActualMargin.Text) * Convert.ToDouble(txtCCSER.Text)).ToString();
            }
            //if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 && !string.IsNullOrEmpty(txtRevisedMargin.Text))
            //{
            //    txtSaleRevisedMargin.Text = (Convert.ToDouble(txtRevisedMargin.Text) * Convert.ToDouble(txtCCSER.Text)).ToString();
            //}
            if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 && !string.IsNullOrEmpty(txtSystemMargin.Text))
            {
                txtSaleSystemMargin.Text = (Convert.ToDouble(txtSystemMargin.Text) * Convert.ToDouble(txtCCSER.Text)).ToString();
            }
        }
        private void txtCCMER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 && !string.IsNullOrEmpty(txtBudgetMargin.Text))
            {
                txtBaseBudgetMargin.Text = (Convert.ToDouble(txtBudgetMargin.Text) * Convert.ToDouble(txtCCMER.Text)).ToString();
            }
            if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 && !string.IsNullOrEmpty(txtActualMargin.Text))
            {
                txtBaseActualMargin.Text = (Convert.ToDouble(txtActualMargin.Text) * Convert.ToDouble(txtCCMER.Text)).ToString();
            }
            //if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 && !string.IsNullOrEmpty(txtRevisedMargin.Text))
            //{
            //    txtBaseRevisedMargin.Text = (Convert.ToDouble(txtRevisedMargin.Text) * Convert.ToDouble(txtCCMER.Text)).ToString();
            //}
            if (((InquiryType)cmbSaleOrderType.SelectedIndex == InquiryType.SupplyCCC) && cmbCostCenterCurrency.SelectedIndex > -1 && !string.IsNullOrEmpty(txtSystemMargin.Text))
            {
                txtMarketSystemMargin.Text = (Convert.ToDouble(txtSystemMargin.Text) * Convert.ToDouble(txtCCMER.Text)).ToString();
            }
        }

        private void txtCostCenterAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var val = Convert.ToDouble(txtCostCenterAmount.Text) / Convert.ToDouble(txtSoAmount.Text);
            txtCostCenterExchangeRate.Text = val.ToString();

        }
        public void loadSaleOrderStatusClass(int _statusId)
        {
            cmbSaleOrderStatusClass.ItemsSource = null;
            var status = saleOrderRepo.getstatus(_statusId);
            if (status.soStatusSubClasses.Count > 0)
            {
                StatusClasses.Clear();
                StatusClasses.AddRange(status.soStatusSubClasses.Where(x => x.isDisable != true).ToList());
                List<cmbitem> cmbitems = new List<cmbitem>();
                Parallel.ForEach(StatusClasses, delegate (ERP_BL.Procurements.StatusClass.StatusClass subClass)
                {
                    cmbitems.Add(new cmbitem() { name = subClass.ClassName, id = subClass.Id, bcolor = subClass.backcolor, fcolor = "#FF000000" });
                });
                cmbSaleOrderStatusClass.ItemsSource = cmbitems;
            }

        }
        public void loadSaleOrderStatusClass(ERP_BL.Procurements.StatusClass.StatusClass statusClass)
        {
            StatusClasses.Add(statusClass);
            if (StatusClasses.Count > 0)
            {
                List<cmbitem> cmbitems = new List<cmbitem>();
                Parallel.ForEach(StatusClasses, delegate (ERP_BL.Procurements.StatusClass.StatusClass subClass)
                {
                    cmbitems.Add(new cmbitem() { name = subClass.ClassName, id = subClass.Id, bcolor = subClass.backcolor, fcolor = "#FF000000" });
                });
                cmbSaleOrderStatusClass.ItemsSource = cmbitems;
            }
        }

        private void cmbSaleOrderStatusClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (saleOrder.Id != 0 && saleOrder.statusClass_Id != null && cmbSaleOrderStatusClass.SelectedIndex>-1)
            {
                checkStatusClass = saleOrderRepo.GetStatusClass((cmbSaleOrderStatusClass.SelectedItem as cmbitem).id);
            }
        }

        private void grdStatusTrackingTree_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = grdStatusTracking;
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

        private void btnRefreshStatusTracking_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
        }
        private void btnExpandStatusTracking_Click(object sender, EventArgs e)
        {
            grdStatusTracking.ShowLoadingPanel = true;
            grdStatusTrackingTree.ExpandAllNodes();
            grdStatusTracking.ShowLoadingPanel = false;
        }

        private void btnCollapsedStatusTracking_Click(object sender, EventArgs e)
        {
            grdStatusTracking.ShowLoadingPanel = true;
            grdStatusTrackingTree.CollapseAllNodes();
            grdStatusTracking.ShowLoadingPanel = false;
        }

        private void btnAuditAdjustment_Click(object sender, RoutedEventArgs e)
        {
            ucTargetAdjustment ucTargetAdjustment = new ucTargetAdjustment(saleOrder);
            ucTargetAdjustment.ShowDialog();
            saleOrder.AuditYearAdjustment= ucTargetAdjustment.auditYearAdjustment;
        }

        private void grdSplitPER_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "PER":
                    var PER = Convert.ToDecimal(txtPER.Text);
                    e.Value = Math.Round( PER, 2);
                    break;
                case "SoAmountPER":
                    var splitPER = grdSplitPER.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    var PER1 = Convert.ToDecimal(txtPER.Text);
                    e.Value = Math.Round(Convert.ToDecimal(splitPER.Amount) * PER1, 2);
                    
                    //grdSplitPER.RefreshRow(grdSplitPER.GetRowHandleByListIndex(e.ListSourceRowIndex));
                    break;
            }
        }

        private void grdSplitPER_Loaded(object sender, RoutedEventArgs e)
        {
            grdSplitPER.RefreshData();
        }

        private void cmbxBillRef_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ExecuteDeleteRowCommand()
        {
            var focusedRow = grdSOItems.GetFocusedRow() as ProcurementProduct;

            if (focusedRow != null)
            {
                // Check some custom logic before deletion
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit FOB and CFR value for products in SaleOrder") != null || saleOrder.isApproved==false)
                {
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this row?",
                                                              "Confirm Delete",
                                                              MessageBoxButton.YesNo,
                                                              MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        // Remove the item from the data source
                        var dataSource = grdSOItems.ItemsSource as List<ProcurementProduct>;
                        if (dataSource != null)
                        {
                            dataSource.Remove(focusedRow);
                            grdSOItems.RefreshData();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("This row cannot be deleted due to specific conditions.", "Delete Not Allowed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private bool CanExecuteDeleteRowCommand()
        {
            // Add any condition for enabling/disabling the delete command
            return grdSOItems.SelectedItem != null;
        }

        private void deleteRowItema_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ExecuteDeleteRowCommand();
        }

        private void btnUnlinkParent_Click(object sender, RoutedEventArgs e)
        {
            if(saleOrder.Id!=0)
            {
                if(saleOrder.isApproved==true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Allow user to delink Approved SaleOrder") != null)
                    {
                        saleOrder.ParentSO_Id = null;
                        MessageBox.Show("This SO delinked successfully, kindly save this sale order.", "Delink Sale Order", MessageBoxButton.OK, MessageBoxImage.Information);

                    }

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Allow user to delink unApproved SaleOrder") != null && saleOrder.isApproved == false)
                    {
                        saleOrder.ParentSO_Id = null;
                        MessageBox.Show("This SO delinked successfully, kindly save this sale order.", "Delink Sale Order", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }

    public class PendingInvoice
    {
        public int Id { get; set; }
        public string refNo { get; set; }
        public string currencyName { get; set; }
        public double invoiceAmount { get; set; }
        public double uninvoiceAmount { get; set; }
        public double collectedAmount { get; set; }
        public double pendingCollectionAmount { get; set; }
        public double paidAmount { get; set; }
        public double unPaidAmount { get; set; }
        public double amountOC { get; set; }
        public double agingDays { get; set; }
        public string customer { get; set; }
        public string vendor { get; set; }
        public string company { get; set; }
        public string department { get; set; }
        public string stage { get; set; }
        public SaleOrderStatus saleInvoiceStatus { get; set; }
        public PurchaseOrderStatus PurchaseInvoiceStatus { get; set; }
    }
}
