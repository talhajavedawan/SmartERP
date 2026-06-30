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
using ZAS_ERP.Procurementss.SaleOrderss;
using System.Text.RegularExpressions;
using DevExpress.Xpf.LayoutControl;
using System.IO;
using DevExpress.Xpf.Core;
using System.Windows.Media.Animation;
using DevExpress.Xpf.Grid;
using System.Linq;
using Microsoft.Win32;
using System.Diagnostics;
using ZAS_ERP.Utils;
using ERP_BL.Tax;
using ERP_BL.ExchangeRates;
using ERP_BL.ToDoTasks.Taskss;
using ZAS_ERP.Procurementss.Budget;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements;
using ERP_BL.Payments;
using ZAS_ERP.Bankings;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.Reportss;
using ERP_BL.Procurements.AdminBills;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ERP_BL.Migrations;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ERP_BL.Procurements.LoansAdvances;

namespace ZAS_ERP.Procurementss.PurchaseOrderss
{


    /// <summary>
    /// Interaction logic for ucInquiryAdd.xaml
    /// </summary>
    public partial class ucPurchaseOrderAdd : UserControl
    {
        UsersRepo UsersRepo = new UsersRepo();
        public int editpurchaseOrder;
        public int purchaseOrderid;
        public static int saleOrderidd;
        public int OrderId;
        public int editOrder;
        public int saleOrderid;
        public int customerParent = 0;
        public int departmentParent = 0; 
        public string customerParentName = "";
        SaleOrder saleOrder = new SaleOrder();
        PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
        PurchaseOrder purchaseOrder = new PurchaseOrder();
        Vendor vendor = new Vendor();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        CustomerCompany customer = new CustomerCompany();
       
        public bool isloading = false;
        public virtual List<ProcurementProduct> procurementProducts { get; set; }
        public virtual List<Product> products { get; set; }
        Currency currency = new Currency();
        bool addinfo = true;
        List<ViewInfo> views = new List<ViewInfo>();
        public PurchaseOrderStatus checkStatus = new PurchaseOrderStatus();
        public double POCFRRemaining = 0;
        PurchaseOrderStatus oldStatus = new PurchaseOrderStatus();

        ERP_BL.Databases.Company InterCompany = new ERP_BL.Databases.Company();
        Department InterDepartment = new Department();
        public bool iscopyTemplate = false;

        int dateShipmentResult = 0;
        int dateRevisedShipmentResult = 0;
        int dateOrderConfirmationResult = 0;
        decimal costSheetPOFieldsSum = 0;



       
        public  Vendor poVendor;
        public Currency poCurrency;
        public decimal poOCamount;
        public string poMaker;
        public string poOrigin;
        public DateTime? poDeliveryDate;
        public PaymentTerm poPaymentTerm;
        public Warranty poWarranty;
        public Incoterm poIncoTerm;
        public int? costSheetFieldId;
        public int? costSheetId;
        List<CostSheetPOField> costSheetPOFields = new List<CostSheetPOField>();
        List<CostSheetSOField> costSheetSOFields = new List<CostSheetSOField>();
        List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
        public List<CostFieldValues> costFieldValue = new List<CostFieldValues>();
        List<CostFieldValues> costfieldCheckedValues = new List<CostFieldValues>();
        public virtual List<BookerStatementItem> BookerStatementItems { get; set; }
        OfferRepo offerRepo = new OfferRepo();
        ProductRepo productRepo = new ProductRepo();
        TaxRepo taxRepo = new TaxRepo();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        PurchaseOrder trackingOrder = new PurchaseOrder();
        List<PurchaseOrderStatus> PurchaseOrderStatuses = new List<PurchaseOrderStatus>();
        List<ERP_BL.Procurements.StatusClass.StatusClass> StatusClasses = new List<ERP_BL.Procurements.StatusClass.StatusClass>();
        public ERP_BL.Procurements.StatusClass.StatusClass checkStatusClass = new ERP_BL.Procurements.StatusClass.StatusClass();

        public ucPurchaseOrderAdd()
        {
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
            InitializeComponent();
            symbol = "";
        }
        public ucPurchaseOrderAdd(bool isCopy)
        {
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
            InitializeComponent();
            symbol = "";
            this.iscopyTemplate = isCopy;
        }

        public void loadBookerItemsSources()
        {
            BookerStatementItems = new List<BookerStatementItem>();
            grdBokkerItems.ItemsSource = BookerStatementItems;
            //lookupBookerFOCSampling.ItemsSource = offerRepo.GetAllActiveFOCSamplings();
            //lookupBookerClaimDiscount.ItemsSource = offerRepo.GetAllActiveClaimDiscounts();
            lookupBookerProductsinGrid.ItemsSource = productRepo.getAllUserProducts(SYSTEM_STATIC.currentUser.id);
            lookupBookerGST.ItemsSource = taxRepo.getAllTaxes();
            //lookupPassOn.ItemsSource = offerRepo.GetAllPassOns();
        }
        private void winPurchaseOrderadd_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loadBookerItemsSources();
                OrderId = purchaseOrderid;
                //saleOrderid = saleOrderidd;
                if (OrderId == 0)
                {
                    string today = DateTime.Today.ToString("ddMMyyyy");
                    int seq = purchaseOrderRepo.getTodayPOCount();
                    seq++;
                    txtPOSystemGeneratedReferenceNumberChange.Text = string.Format("PO-{0}-{1}", today, seq.ToString("D4"));
                }
                editOrder = editpurchaseOrder;
                txttax.Text = "0";
                grdPOItems.ItemsSource = procurementProducts;
                //products = SYSTEM_STATIC.GetItemsForCurrentUser();
                //lookupProductsinGrid.ItemsSource = products;
                loadcompanies();
                cmbPurchaseOrderType.ItemsSource = SYSTEM_STATIC.loadPurchaseOrdertypes();
                loadWarrantys();
                loadPaymentTerms();
                loadIncoterms();
                loadCaptions();
                loadCurrencies();
                loadPurchaseOrderStatus();
                loadVendorPaymentStatus();

                TaxRepo taxRepo = new TaxRepo();
                var taxes = taxRepo.getAllTaxes();
                lookUpTax.ItemsSource = taxes;
                lookUpWHT.ItemsSource = taxes;

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of PurchaseOrder") != null)
                {
                    datpoCreationdate.IsEnabled = true;

                }
                else
                {
                    datpoCreationdate.IsEnabled = false;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with PurchaseOrder") != null)
                {
                    //btnAttachNew.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAttachNew.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with PurchaseOrder") != null)
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
        

                if (editpurchaseOrder == 1 && purchaseOrderid != 0)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Order") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Order") == null)
                    {
                        isloading = true;
                        purchaseOrder = purchaseOrderRepo.get(purchaseOrderid);

                        if (purchaseOrder.PurchaseOrdertype == InquiryType.Standard)
                        {
                            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Select Standard PO") != null)
                            //{
                                grppurchaseOrderinfo.Visibility = Visibility.Collapsed;
                                grppurchaseOrderChangeinfo.Visibility = Visibility.Visible;
                            //}
                            //else
                            //{
                            //    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Standard Purchase Order!");
                            //    grppurchaseOrderinfo.Visibility = Visibility.Visible;
                            //    grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                            //    return;
                            //}

                        }
                        loadonPurchaseOrderdata();
                        GellAllOrdersTracking();
                        //views = _usersRepo.getViwerInfo(purchaseOrder.Id, (int)TransactionItemType.Purchase_Order);
                        //grdUsers.ItemsSource = views;
                        //loadcomments();
                        btnSave.IsEnabled = false;
                        if (purchaseOrder.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Purchase Order") != null)
                        {
                            btnSave.IsEnabled = true;
                        }


                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Order") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Order") != null)
                    {
                        isloading = true;
                        purchaseOrder = purchaseOrderRepo.get(purchaseOrderid);

                        if (purchaseOrder.PurchaseOrdertype == InquiryType.Standard)
                        {
                            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Select Standard PO") != null)
                            //{
                                grppurchaseOrderinfo.Visibility = Visibility.Collapsed;
                                grppurchaseOrderChangeinfo.Visibility = Visibility.Visible;
                            //}
                            //else
                            //{
                            //    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Standard Purchase Order!");
                            //    grppurchaseOrderinfo.Visibility = Visibility.Visible;
                            //    grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                            //    return;
                            //}

                        }
                        loadonPurchaseOrderdata();
                        GellAllOrdersTracking();
                        //views = _usersRepo.getViwerInfo(purchaseOrder.Id, (int)TransactionItemType.Purchase_Order);
                        //grdUsers.ItemsSource = views;
                        //loadcomments();
                        btnSave.IsEnabled = true;

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Order") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Order") != null)
                    {
                        isloading = true;
                        purchaseOrder = purchaseOrderRepo.get(purchaseOrderid);

                        if (purchaseOrder.PurchaseOrdertype == InquiryType.Standard )
                        {
                            //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Select Standard PO") != null)
                            //{
                                grppurchaseOrderinfo.Visibility = Visibility.Collapsed;
                                grppurchaseOrderChangeinfo.Visibility = Visibility.Visible;
                            //}
                            //else
                            //{
                            //    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Standard Purchase Order!");
                            //    grppurchaseOrderinfo.Visibility = Visibility.Visible;
                            //    grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                            //    return;
                            //}

                        }
                        loadonPurchaseOrderdata();
                        GellAllOrdersTracking();
                        //views = _usersRepo.getViwerInfo(purchaseOrder.Id, (int)TransactionItemType.Purchase_Order);
                        //grdUsers.ItemsSource = views;
                        //loadcomments();
                        btnSave.IsEnabled = true;
                    }

                    //Setting void stamp
                    if (purchaseOrder != null)
                    {
                        if (purchaseOrder.isVoid == true)
                        {
                            grdVoid.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Purchase Order!");
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;

                    }

                }
                else if (editpurchaseOrder == 0 && saleOrderid != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order") != null)
                    {
                        SaleOrderRepo offerRepo = new SaleOrderRepo();
                        isloading = true;
                        saleOrder = offerRepo.get(saleOrderid);
                       
                        loadonSaleOrderdata();
       
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to Add Purchase Order!");
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
                }
                calculatetotal();
                isloading = false;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Market Exchange Rate in Purchase Order") != null)
                {
                    lblexchangerate.Visibility = Visibility.Visible;
                    txtexchangerate.Visibility = Visibility.Visible;
                }
                else
                {
                    lblexchangerate.Visibility = Visibility.Collapsed;
                    txtexchangerate.Visibility = Visibility.Collapsed;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void PurchaseOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void PurchaseOrder") != null)
                {
                    btnSetVoid.Visibility = Visibility.Visible;
                }
                if (purchaseOrder.saleOrder_Id != null)
                {
                    saleOrder = purchaseOrder.SaleOrder;
                    saleOrderid = saleOrder.Id;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Move PurchaseOrder to Inter Company") != null)
                {
                    gridInterCompanyDetails.IsEnabled = true;
                    chkInterCompany.IsEnabled = true;
                }
                else
                {
                    gridInterCompanyDetails.IsEnabled = false;
                    chkInterCompany.IsEnabled = false;
                }
                LoadPendingInvoices();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                DXMessageBox.Show(ex.ToString(), "Data Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            grdPOItems.Columns["value1"].AllowMoving = DevExpress.Utils.DefaultBoolean.False;
            grdPOItems.Columns["value2"].AllowMoving = DevExpress.Utils.DefaultBoolean.False;
            grdTrackingTree.ExpandAllNodes();
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
        public void loadcompanies()
        {
            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();
                this.lookupCompany.ItemsSource = cont.GetCompanies();
                return;
            }
           
            lookupCompany.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
            lookupInterCompany.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
        }
        public void loadInterCompanyDepartments()
        {
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
                    if (purchaseOrder != null && purchaseOrder.Id > 0 && editOrder == 1)
                        if (purchaseOrder.department != null && departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                            departments.Add(purchaseOrder.InterDepartment);

                    lookupInterDepartment.ItemsSource = departments;
                    //if (lookupInterDepartment.ItemsSource == null)
                    //    lookupInterDepartment.ItemsSource = departments;

                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
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

        private void lookupInterCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            InterCompany = lookupInterCompany.SelectedItem as Company;
            if (InterCompany != null)
            {
                loadInterCompanyDepartments();
            }
        }

        private void lookupInterDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            InterDepartment = lookupInterDepartment.SelectedItem as Department;

        }
        public void loadvendors()
        {
            lookupVendor.ItemsSource = department.Vendors;
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
                            if (purchaseOrder.Id != 0)
                            {
                                var customer = department.disableCustomers.FirstOrDefault(x => x.Id == purchaseOrder.customerCompany_Id);
                                if (customer != null)
                                {
                                    customers.AddRange(department.disableCustomers);
                                }
                            }
                        }
                        if (customers == null || customers.Count == 0)
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Please select a different Department and Company! No customer is mapped to this department or Company.", "Select another Department or Company", MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
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
            List<string> InquiryType = config.getPurchaseOrderTypes();
            if (InquiryType.Count > 0)
                foreach (string IND in InquiryType)
                {
                    cmbPurchaseOrderType.Items.Add(IND);
                }
        }
        public void loademployees()
        {
            ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
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
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsPurchaseOrderType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (purchaseOrder != null && purchaseOrder.Id > 0 && editOrder == 1)
                        if (purchaseOrder.department != null && departments.FirstOrDefault(x => x.Id == purchaseOrder.dept_Id) == null)
                            departments.Add(purchaseOrder.department);

                    lookupDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }
        public void loadonSaleOrderdata()
        {
            //cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
           
            cmbPurchaseOrderType.SelectedItem = saleOrder.saleOrdertype.ToString();
            //PoType = saleOrder.saleOrdertype;
            PoType = InquiryType.Standard;
            datpoCreationdate.EditValue = System.DateTime.Now;
            datsaleOrderdate.EditValue = saleOrder.saleOrderDate;
            datDeliverydate.EditValue = saleOrder.deliveryDate;
            txtSalesref.Text = saleOrder.SalesReferenceNo;
            txtOfferRefNo.Text = saleOrder.offerReferenceNo;
            //txtCommissionNumber.Text = saleOrder.commisionRefrenceNo;
            txtFinanaceRef.Text = saleOrder.FinanceRefrenceNo;
            txtsaleOrderref.Text = saleOrder.referenceNo;


            //LoadingPOChangeData
            datsaleOrderdateChange.EditValue = saleOrder.saleOrderDate;
            datDeliverydateChange.EditValue = saleOrder.deliveryDate;
            txtSalesrefChange.Text = saleOrder.SalesReferenceNo;
            txtOfferRefNoChange.Text = saleOrder.offerReferenceNo;
            //txtCommissionNumber.Text = saleOrder.commisionRefrenceNo;
            txtFinanaceRefChange.Text = saleOrder.FinanceRefrenceNo;
            txtsaleOrderrefChange.Text = saleOrder.referenceNo;
            txtSOCountryOriginChange.Text = saleOrder.origin;
            txtSOMakerChange.Text = saleOrder.maker;
            txtSOPackingChange.Text = saleOrder.packing;


            if (saleOrder.PurchaseOrders != null)
                lblPOrefrence.Text = saleOrder.referenceNo + " - " + (saleOrder.PurchaseOrders?.Count() + 1);
            else
                lblPOrefrence.Text = saleOrder.referenceNo + " - " + 1;

            txtOwnDescription.Text = saleOrder.OwnDescription;
            txtOwnDescriptionChange.Text = saleOrder.OwnDescription;



            //Warranty
            if (saleOrder.WarrantyId != 0 && saleOrder.Warranty != null)
            {
                var incoSource = (List<cmbitem>)cmbSOWarranty.Items.SourceCollection;
                var incoSourceChange = (List<cmbitem>)cmbSOWarrantyChange.Items.SourceCollection;

                cmbSOWarranty.SelectedItem = cmbSOWarranty.Items[cmbSOWarranty.Items.IndexOf(incoSource.Find(x => x.id == saleOrder.WarrantyId))];
                cmbSOWarrantyChange.SelectedItem = cmbSOWarrantyChange.Items[cmbSOWarrantyChange.Items.IndexOf(incoSourceChange.Find(x => x.id == saleOrder.WarrantyId))];
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
                var paymentTermSource = (List<cmbitem>)cmbSOPaymentTerm.Items.SourceCollection;
               
                //cmbSOPaymentTerm.SelectedItem = cmbSOPaymentTerm.Items[cmbSOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == saleOrder.paymentterm_Id))];

                var term = paymentTermSource.Find(x => x.id == saleOrder.paymentterm_Id);
                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = saleOrder.paymentTerm.term, id = saleOrder.paymentTerm.Id });
                    cmbSOPaymentTerm.ItemsSource = null;
                    cmbSOPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbSOPaymentTerm.SelectedItem = cmbSOPaymentTerm.Items[cmbSOPaymentTerm.Items.IndexOf(term)];



                var paymentTermSourceChange = (List<cmbitem>)cmbSOPaymentTermChange.Items.SourceCollection;

                //cmbSOPaymentTermChange.SelectedItem = cmbSOPaymentTermChange.Items[cmbSOPaymentTermChange.Items.IndexOf(paymentTermSourceChange.Find(x => x.id == saleOrder.paymentterm_Id))];

                term = paymentTermSource.Find(x => x.id == saleOrder.paymentterm_Id);
                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = saleOrder.paymentTerm.term, id = saleOrder.paymentTerm.Id });
                    cmbSOPaymentTermChange.ItemsSource = null;
                    cmbSOPaymentTermChange.ItemsSource = paymentTermSourceChange;
                }
                cmbSOPaymentTermChange.SelectedItem = cmbSOPaymentTermChange.Items[cmbSOPaymentTermChange.Items.IndexOf(term)];

            }
            //Inco Term Load for sale Order
            if (saleOrder.incoterm_Id != 0 && saleOrder.incoterm != null)
            {
                var incotermSourceChange = (List<cmbitem>)cmbSOIncotermChange.Items.SourceCollection;

                cmbSOIncotermChange.SelectedItem = cmbSOIncotermChange.Items[cmbSOIncotermChange.Items.IndexOf(incotermSourceChange.Find(x => x.id == saleOrder.incoterm_Id))];

            }

            if (saleOrder.currency_Id != 0 && saleOrder.currency != null)
            {
                var currencySource = (List<cmbitem>)cmbSOCurrency.Items.SourceCollection;
                cmbSOCurrency.SelectedItem = cmbSOCurrency.Items[cmbSOCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleOrder.currency_Id))];
            }


            // Select Company
            if (saleOrder.company_Id != null || saleOrder.company != null)
            {
                company = saleOrder.company;
                lookupCompany.Text = saleOrder.company.CompanyName;
                //lookupCompany.SelectedItem = lookupCompany.GetItemByKeyValue(saleOrder.company);
                //loaddepartments();
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }
            // Select Department
            if (saleOrder.dept_Id != 0 || saleOrder.department != null)
            {
                lookupDepartment.Text = saleOrder.department.DeptName;
                //lookupDepartment.SelectedItem = lookupDepartment.GetItemByKeyValue(saleOrder.department);
                department = saleOrder.department;

                //lookupCustomer.ItemsSource = department.customers;
                loademployees();

            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }
            //Select Customer
            if (saleOrder.customerCompany.Id != 0 || saleOrder.customerCompany != null)
            {
                lookupCustomer.Text = saleOrder.customerCompany.company.CompanyName;
                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(saleOrder.customerCompany);

                customer = saleOrder.customerCompany;
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
                    vendor = vendr;
                }
            }

            if (saleOrder.allocation_Id != 0 || saleOrder.employee != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == saleOrder.allocation_Id))];

                }
                catch (Exception ex)
                {

                }

            }

            // load Products in Inquiry to grid
            List<DataGridItem> datagriditems = new List<DataGridItem>();


            //if (saleOrder.products != null)
            //{
            //    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
            //    //grdPOItems.ItemsSource = offer.products;
            //    foreach (var procurementProduct in saleOrder.products)
            //    {
            //        procurementProducts.Add(new ProcurementProduct()
            //        {
            //            Id = procurementProduct.Id,

            //            inquiryProduct = new InquiryProduct()
            //            {
            //                Id = procurementProduct.inquiryProduct.Id,
            //                ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
            //                UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
            //                quantity = procurementProduct.inquiryProduct.quantity,
            //                Weight = procurementProduct.inquiryProduct.Weight,
            //                product = new Product()
            //                {
            //                    Id = procurementProduct.inquiryProduct.product.Id,
            //                    categoryId = procurementProduct.inquiryProduct.product.categoryId,
            //                    item = procurementProduct.inquiryProduct.product.item,
            //                    code = procurementProduct.inquiryProduct.product.code,
            //                    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
            //                    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
            //                    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
            //                    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
            //                    unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
            //                    nature = procurementProduct.inquiryProduct.product.nature,
            //                    category = procurementProduct.inquiryProduct.product.category,
            //                    isActive = procurementProduct.inquiryProduct.product.isActive

            //                }
            //                    ,
            //                product_Id = procurementProduct.inquiryProduct.product.Id

            //            },
            //            product_Id = procurementProduct.inquiryProduct.Id,
            //            unitPrice = procurementProduct.unitPrice,
            //            UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity,
            //            InvoicedQuantity = (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,
            //            UnInvoicedWeight = (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,
            //            InvoicedWeight = (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,
            //            value1 = procurementProduct.value1,
            //            value2 = procurementProduct.value2,
            //            caption1 = cmbcaption1.Text.Trim(),
            //            caption2 = cmbcaption2.Text.Trim(),
            //            UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount
            //            //UnInvoicedQuantity= procurementProduct.UnInvoicedQuantity,


            //        });


            //    }
            //    //dGitems.ItemsSource = datagriditems;
            //    grdPOItems.ItemsSource = procurementProducts;
            //    int x = 0;
            //    foreach (var pro in grdPOItems.ItemsSource as List<ProcurementProduct>)
            //    {
            //        grdPOItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
            //        x++;

            //    }
            //    lookupProductsinGrid.DisplayMember = "code";

            //}
            //else
            // {
            //    grdPOItems.ItemsSource = procurementProducts;
            //}

            // selected currency of customer company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {

                if (cmbitem.id == saleOrder.company.CurrencyId)
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
                grdPOItems.Columns.GetColumnByFieldName("value1").ReadOnly = false;
                grdPOItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;

            }
            if (purchaseOrder.products == null)
            {
                List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                //grdPOItems.ItemsSource = offer.products;
                if (saleOrder.products != null)
                {
                    foreach (var procurementProduct in saleOrder.products)
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
                                },
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
                    }
                    //dGitems.ItemsSource = datagriditems;
                    grdPOItems.ItemsSource = procurementProducts;
                    int x = 0;
                    foreach (var pro in grdPOItems.ItemsSource as List<ProcurementProduct>)
                    {
                        grdPOItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                        x++;
                    }
                    lookupProductsinGrid.DisplayMember = "code";
                }
                else
                {
                    grdPOItems.ItemsSource = procurementProducts;
                }
            }
            if (saleOrder.deliveryDateFinal != null)
            {
                datDeliverydateFinalChange.EditValue = saleOrder.deliveryDateFinal;
            }

            
            if ( poVendor!=null)
            {
                lookupVendor.Text = poVendor.company.CompanyName;
                vendor = poVendor;
            }
            //Currency poCurrency;
            //if (poVendor.Id != 0)
            //{

            //}
            if (poCurrency != null)
            {
                var currencySource = (List<cmbitem>)cmbPOCurrency.Items.SourceCollection;
                cmbPOCurrency.SelectedItem = cmbPOCurrency.Items[cmbPOCurrency.Items.IndexOf(currencySource.Find(x => x.id == poCurrency.Id))];
            }
            //double poOCamount;
            //if (poVendor.Id != 0)
            //{
           
            //}


            if (poMaker != null)
            {
                txtMakerChange.Text = poMaker;
            }
            if (poOrigin != null)
            {
                txtOriginChange.Text = poOrigin;
            }
            if (poDeliveryDate != null)
            {
                datPODeliverydateChange.EditValue = poDeliveryDate;
            }
            if (poPaymentTerm != null)
            {
                var paymentTermSourceChange = (List<cmbitem>)cmbPOPaymentTermChange.Items.SourceCollection;
                cmbPOPaymentTermChange.SelectedItem = cmbPOPaymentTermChange.Items[cmbPOPaymentTermChange.Items.IndexOf(paymentTermSourceChange.Find(x => x.id == poPaymentTerm.Id))];

            }
            if (poWarranty != null)
            {
                var warrentySourceChange = (List<cmbitem>)cmbPOWarrantyChange.Items.SourceCollection;

                cmbPOWarrantyChange.SelectedItem = cmbPOWarrantyChange.Items[cmbPOWarrantyChange.Items.IndexOf(warrentySourceChange.Find(x => x.id == poWarranty.Id))];

            }
            if (poIncoTerm != null)
            {
                var incoSourceChange = (List<cmbitem>)cmbIncotermChange.Items.SourceCollection;
                cmbIncotermChange.SelectedItem = cmbIncotermChange.Items[cmbIncotermChange.Items.IndexOf(incoSourceChange.Find(x => x.id == poIncoTerm.Id))];
            }
            if(costSheetId!=0)
            {
                purchaseOrder.CostSheet_Id = costSheetId;
            }



        }
        public void loadonPurchaseOrderdata()
        {
            TaskRepo taskRepo = new TaskRepo();
            var task = taskRepo.GetPOTask(purchaseOrderid);
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

            if (purchaseOrder.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            else if (purchaseOrder.isReApproved == false)
            {
                //lblStage.Text = "Under Re-Approval";
                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (purchaseOrder.isApproved == true && purchaseOrder.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (purchaseOrder.isApproved == true && purchaseOrder.PurchaseOrderStatus.isActive == false && purchaseOrder.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (purchaseOrder.isApproved == true && purchaseOrder.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (purchaseOrder.isApproved == true)
            {
                //lblStage.Text = "Approved";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (purchaseOrder.isApproved == false)
            {
                //lblStage.Text = "Under Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (purchaseOrder.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            if(purchaseOrder.COO!=0)
            {
                chkCOO.IsChecked = true;
                txtCOO.Text = purchaseOrder.COO.ToString();
            }
            if (purchaseOrder.Discount != 0)
            {
                chkDiscount.IsChecked = true;
                txtDiscount.Text = purchaseOrder.Discount.ToString();
            }
            if (purchaseOrder.Freight != 0)
            {
                chkFreight.IsChecked = true;
                txtFreight.Text = purchaseOrder.Freight.ToString();
            }
            txtPaidAdvance.Text = purchaseOrder.totalPOAdvance.ToString();
            txtExpectedPaymentAmount.Text = purchaseOrder.expectedPaymentAmount.ToString();
            txtPOSattled.Text = purchaseOrder.totalPOSattled.ToString();
            if(purchaseOrder.SaleOrder!=null)
            {
                txtSOAmount.Text = purchaseOrder.SaleOrder.totalCFRValue.ToString();
                if(purchaseOrder.SaleOrder.SaleInvoices.Count!=0)
                {
                    txtsoInvoice.Text = purchaseOrder.SaleOrder.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount).ToString();
                }
            }

            if (purchaseOrder.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Basic Information After Approval in PO") != null)
                {
                    grpbasicinfo.IsEnabled = true;
                }
                else if (purchaseOrder.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Edit Basic Information After Approval in PO") == null)
                {
                    grpbasicinfo.IsEnabled = false;
                }

            if (purchaseOrder.saleOrder_Id != null && SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "Can Edit Vendor in Purchase Order") == null)
            {
                lookupVendor.IsEnabled = false;
            }

            var paidAmount = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.Payments.Where(y => y.isVoid != true).Sum(z => z.DebitedAmount));

            if(purchaseOrder.tax != null)
            {
                double amountWithTax = 0;
                var purchaseInvoices = purchaseOrder.PurchaseInvoices;
                var invoicedAmount1 = purchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount);
                if(purchaseOrder.tax.isManual == true)
                {
                    amountWithTax = Math.Round( invoicedAmount1 + purchaseInvoices.Sum(x=>x.totaltaxAmount),2);
                }
                else
                    amountWithTax = Math.Round( invoicedAmount1 + ((invoicedAmount1 * purchaseOrder.tax.percentage) / 100),2);
                txtInvoiceAmount.Text = Math.Round( amountWithTax, 2).ToString();
                txtUnInvoiceAmount.Text = Math.Round(  purchaseOrder.billWithTax.Value - amountWithTax, 2).ToString();

               
                txtPaid.Text = paidAmount.ToString();
                txtUnpaid.Text = Math.Round(amountWithTax - paidAmount, 2).ToString();
            }
            else
            {
                var invoicedAmount = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount + x.totaltaxAmount);

                txtInvoiceAmount.Text = invoicedAmount.ToString() ;
                txtUnInvoiceAmount.Text = Math.Round(purchaseOrder.totalCFRValue+purchaseOrder.totaltaxAmount - invoicedAmount, 2).ToString();
                txtPaid.Text = paidAmount.ToString();
                txtUnpaid.Text = Math.Round(invoicedAmount  - paidAmount,2).ToString();
            }
            txtInvoicedCount.Text= purchaseOrder.PurchaseInvoices.Count().ToString();

            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(purchaseOrder.Id, TransactionItemType.Purchase_Order);
            //cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
            
            cmbPurchaseOrderType.SelectedItem = purchaseOrder.PurchaseOrdertype.ToString();
            //PoType = purchaseOrder.PurchaseOrdertype;
            PoType = purchaseOrder.PurchaseOrdertype;
            lblLastStatusChangeDate.EditValue = purchaseOrder.LastStatusChangeDate;
            datsaleOrderdate.EditValue = purchaseOrder.PurchaseOrderDate;
            datpoCreationdate.EditValue = purchaseOrder.CreationDate;
            datShipmentdate.EditValue = purchaseOrder.ShipmentDate;
            datRevisedShipmentDate.EditValue = purchaseOrder.RevisedShipmentDate;
            datExcpectedPaymentDate.EditValue = purchaseOrder.ExpectedPayment;
            datPaymentDueTill.EditValue = purchaseOrder.PaymentDueAgeing;
            datDeliverydate.EditValue = purchaseOrder.DeliveryDate;
            datOrderConfirmationdate.EditValue = purchaseOrder.OrderConfirmationDate;
            datBillOfLaddingdate.EditValue = purchaseOrder.BillOfLaddingDate;
            datLCDatedate.EditValue = purchaseOrder.lCDate;
            datMaterialReciptdate.EditValue = purchaseOrder.MaterialReciptDate;
            datPaymentDueFrom.EditValue = purchaseOrder.PaymentDueStartDate;
            txtSalesref.Text = purchaseOrder.SalesReferenceNo;
            txtOfferRefNo.Text = purchaseOrder.OfferReferenceNo;
            txtsaleOrderref.Text = purchaseOrder.SOReferenceNo;
            lblPOrefrence.Text = purchaseOrder.POReferenceNo;

            txtLOTNo.Text = purchaseOrder.lotNo;

            if (purchaseOrder.lotNumber != null)
            {
                lookupLotNumbers.Text = purchaseOrder.lotNumber.LotNo;
            }

            if (!string.IsNullOrEmpty(purchaseOrder.SyetmReferenceNo))
                txtPOSystemGeneratedReferenceNumberChange.Text = purchaseOrder.SyetmReferenceNo;
            if (purchaseOrder.proforma!=null)
            {
                txtProformaChange.Text = purchaseOrder.proforma;
            }
            //LoadingPOChangeInformation
            
                lblLastStatusChangeDateChange.EditValue = purchaseOrder.LastStatusChangeDate; 
                datShipmentdateChange.EditValue = purchaseOrder.ShipmentDate;
                datRevisedShipmentDateChange.EditValue = purchaseOrder.RevisedShipmentDate;
                datExcpectedPaymentDateChange.EditValue = purchaseOrder.ExpectedPayment;
                datPaymentDueTillChange.EditValue = purchaseOrder.PaymentDueAgeing;
                datOrderConfirmationdateChange.EditValue = purchaseOrder.OrderConfirmationDate;
                datBillOfLaddingdateChange.EditValue = purchaseOrder.BillOfLaddingDate;
                datMaterialReciptdateChange.EditValue = purchaseOrder.MaterialReciptDate;
                datPaymentDueFromChange.EditValue = purchaseOrder.PaymentDueStartDate;
                txtSalesrefChange.Text = purchaseOrder.SalesReferenceNo;
                txtPOReferenceNumberChange.Text= purchaseOrder.POReferenceNo;
                datPoDateChange.EditValue = purchaseOrder.CreationDate;
                txtOwnDescriptionChange.Text = purchaseOrder.OwnDescription;
                txtOfferRefNoChange.Text = purchaseOrder.OfferReferenceNo;
                datPODeliverydateChange.EditValue = purchaseOrder.DeliveryDate;
                //loadingPOChangeInformation      
                txtPackingChange.Text = (string.IsNullOrEmpty(purchaseOrder.packing)) ? "" : purchaseOrder.packing;
                txtOriginChange.Text = purchaseOrder.origin;
                txtMakerChange.Text = purchaseOrder.maker;
                //LoadingPOChangeInfo
                txtOwnDescriptionChange.Text = purchaseOrder.OwnDescription;
                txtCommissionNumberChange.Text = purchaseOrder.VendorName;
                txtFinanaceRefChange.Text = purchaseOrder.FinanceRefrenceNo;
                //LoadingPOChangeInfo
                txtPaymentDueDaysChange.Text = purchaseOrder.CreditDays.ToString();

            if (purchaseOrder.StatusClass != null)
                checkStatusClass = purchaseOrder.StatusClass;
            if (purchaseOrder.incoterm_Id != 0 || purchaseOrder.incoterm != null)
                {
                    var incoSourceChange = (List<cmbitem>)cmbIncotermChange.Items.SourceCollection;
                    cmbIncotermChange.SelectedItem = cmbIncotermChange.Items[cmbIncotermChange.Items.IndexOf(incoSourceChange.Find(x => x.id == purchaseOrder.incoterm_Id))];
                }
                
                if (purchaseOrder.POWarrantyId != 0 || purchaseOrder.POWarranty != null)
                {
                    var incoSourceChange = (List<cmbitem>)cmbPOWarrantyChange.Items.SourceCollection;

                    cmbPOWarrantyChange.SelectedItem = cmbPOWarrantyChange.Items[cmbPOWarrantyChange.Items.IndexOf(incoSourceChange.Find(x => x.id == purchaseOrder.POWarrantyId))];

                }
               
                //Payment term Load for sale Order
                if (purchaseOrder.POPaymentterm_Id != 0 || purchaseOrder.POPaymentTerm != null)
                {
                    var paymentTermSourceChange = (List<cmbitem>)cmbPOPaymentTermChange.Items.SourceCollection;
                    //cmbPOPaymentTermChange.SelectedItem = cmbPOPaymentTermChange.Items[cmbPOPaymentTermChange.Items.IndexOf(paymentTermSourceChange.Find(x => x.id == purchaseOrder.POPaymentterm_Id))];

                var term = paymentTermSourceChange.Find(x => x.id == purchaseOrder.POPaymentterm_Id);
                if (term == null)
                {
                    paymentTermSourceChange.Add(new cmbitem() { name = purchaseOrder.POPaymentTerm.term, id = purchaseOrder.POPaymentTerm.Id });
                    cmbPOPaymentTermChange.ItemsSource = null;
                    cmbPOPaymentTermChange.ItemsSource = paymentTermSourceChange;
                }
                cmbPOPaymentTermChange.SelectedItem = cmbPOPaymentTermChange.Items[cmbPOPaymentTermChange.Items.IndexOf(paymentTermSourceChange.Find(x => x.id == purchaseOrder.POPaymentterm_Id))];

            }
            if (purchaseOrder.PurchaseOrderStatus != null)
            {
                var disAbleStatus = PurchaseOrderStatuses.FirstOrDefault(x => x.Id == purchaseOrder.PurchaseOrderStatus.Id);
                if (disAbleStatus == null)
                {
                    loadPurchaseOrderStatus(purchaseOrder.PurchaseOrderStatus);
                }
            }
            // loadingPoChangeData
            var POSourceChange = (List<cmbitem>)cmbPurchaseOrderStatusChange.Items.SourceCollection;
                // Select PurchaseOrder Status 
                if (purchaseOrder.PurchaseOrderStatus.isActive == false)
                {
                    try
                    {
                        //loadingPOChangeData
                        cmbPurchaseOrderStatusChange.SelectedItem = cmbPurchaseOrderStatusChange.Items[cmbPurchaseOrderStatusChange.Items.IndexOf(POSourceChange.Find(x => x.name == purchaseOrder.PurchaseOrderStatus.Status))];


                    }

                    catch (Exception ex)
                    {
                        //DXMessageBox.Show("This User cannot see closed Purchase Order status!");
                        SystemLog.LogError(this.GetType(), "This User cannot see closed Purchase Order status! " + ex.ToString());
                        //SystemLog.LogInfo(this.GetType(), ex.ToString());

                    }

                }
                else
                {
                    cmbPurchaseOrderStatusChange.SelectedItem = cmbPurchaseOrderStatusChange.Items[cmbPurchaseOrderStatusChange.Items.IndexOf(POSourceChange.Find(x => x.name == purchaseOrder.PurchaseOrderStatus.Status))];

                }
            if (purchaseOrder.StatusClass != null)
            {
                var disAbleStatus = StatusClasses.FirstOrDefault(x => x.Id == purchaseOrder.statusClass_Id);
                if (disAbleStatus == null)
                {
                    loadPurchaseOrderStatusClass(purchaseOrder.StatusClass);
                }
                var StatusClassSource = (List<cmbitem>)cmbPurchaseOrderStatusClass.Items.SourceCollection;

                // Select SaleInvoice Status 
                if (purchaseOrder.StatusClass.isActive == false)
                {
                    try
                    {
                        cmbPurchaseOrderStatusClass.SelectedItem = cmbPurchaseOrderStatusClass.Items[cmbPurchaseOrderStatusClass.Items.IndexOf(StatusClassSource.Find(x => x.name == purchaseOrder.StatusClass.ClassName))];
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    cmbPurchaseOrderStatusClass.SelectedItem = cmbPurchaseOrderStatusClass.Items[cmbPurchaseOrderStatusClass.Items.IndexOf(StatusClassSource.Find(x => x.name == purchaseOrder.StatusClass.ClassName))];
                }
            }
            if (purchaseOrder.vendorPaymentId != null || purchaseOrder.vendorPaymentStatus != null)
                {
                    var currencySourceChange = (List<cmbitem>)cmbVendorPaymentStatusChange.Items.SourceCollection;

                    cmbVendorPaymentStatusChange.SelectedItem = cmbVendorPaymentStatusChange.Items[cmbVendorPaymentStatusChange.Items.IndexOf(currencySourceChange.Find(x => x.id == purchaseOrder.vendorPaymentId))];

                }
                txtexchangerate.Text = purchaseOrder.ExchangeRate.ToString();
                txtMarginexchangerate.Text = purchaseOrder.marginExchangeRate.ToString();

            SaleOrder saleOrder = new SaleOrder();
            if (purchaseOrder.saleOrder_Id != null && purchaseOrder.saleOrder_Id > 0)
                saleOrder = purchaseOrder.SaleOrder;
                //saleOrder = saleOrderRepo.get(Convert.ToInt32(purchaseOrder.saleOrder_Id));

            if (saleOrder!=null && saleOrder.Id > 0)
                {
                    txtsaleOrderrefChange.Text = saleOrder.referenceNo;
                    datsaleOrderdateChange.EditValue = saleOrder.saleOrderDate;
                    datDeliverydateChange.EditValue = saleOrder.deliveryDate;
                    
                //Payment term Load for sale Order
                if (purchaseOrder.SoPaymentterm_Id != 0 || purchaseOrder.SoPaymentTerm != null)
                {
                    var paymentTermSourceChange = (List<cmbitem>)cmbSOPaymentTermChange.Items.SourceCollection;
                    //cmbSOPaymentTermChange.SelectedItem = cmbSOPaymentTermChange.Items[cmbSOPaymentTermChange.Items.IndexOf(paymentTermSourceChange.Find(x => x.id == purchaseOrder.SoPaymentterm_Id))];

                    var term = paymentTermSourceChange.Find(x => x.id == purchaseOrder.SoPaymentterm_Id);

                    if (term == null)
                    {
                        paymentTermSourceChange.Add(new cmbitem() { name = purchaseOrder.SoPaymentTerm.term, id = purchaseOrder.SoPaymentTerm.Id });
                        cmbSOPaymentTermChange.ItemsSource = null;
                        cmbSOPaymentTermChange.ItemsSource = paymentTermSourceChange;
                    }
                    cmbSOPaymentTermChange.SelectedItem = cmbSOPaymentTermChange.Items[cmbSOPaymentTermChange.Items.IndexOf(paymentTermSourceChange.Find(x => x.id == purchaseOrder.SoPaymentterm_Id))];

                }

                if (saleOrder.incoterm_Id != 0 || saleOrder.incoterm != null)
                {
                    var incoSOSourceChange = (List<cmbitem>)cmbSOIncotermChange.Items.SourceCollection;
                    cmbSOIncotermChange.SelectedItem = cmbSOIncotermChange.Items[cmbSOIncotermChange.Items.IndexOf(incoSOSourceChange.Find(x => x.id == saleOrder.incoterm_Id))];

                }
                //Warranty
                if (purchaseOrder.SOWarrantyId != 0 && purchaseOrder.SOWarranty != null)
                    {
                        var incoSourceChange = (List<cmbitem>)cmbSOWarrantyChange.Items.SourceCollection;

                        cmbSOWarrantyChange.SelectedItem = cmbSOWarrantyChange.Items[cmbSOWarrantyChange.Items.IndexOf(incoSourceChange.Find(x => x.id == purchaseOrder.SOWarrantyId))];

                    }
                    txtSOCountryOriginChange.Text = saleOrder.origin;
                    txtSOMakerChange.Text = saleOrder.maker;
                    txtSOPackingChange.Text = saleOrder.packing;
 
                }          
            {
                datLCShipmentDate.EditValue = purchaseOrder.LCShipmentDate;
                datLCRevisedShipmentDate.EditValue = purchaseOrder.LCShipmentAmendmentDate;
                datLCExpiryDate.EditValue = purchaseOrder.LCExpiryDate;
                datLCExpiryDate.EditValue = purchaseOrder.LCExpiryAmedmentDate;
                txtLCAmedmentno.Text = (string.IsNullOrEmpty(purchaseOrder.LCAmedmentNo)) ? "" : purchaseOrder.LCAmedmentNo;
                txtPacking.Text = (string.IsNullOrEmpty(purchaseOrder.packing)) ? "" : purchaseOrder.packing;
                if (purchaseOrder.transshipment == true)
                { cmbTransshipment.SelectedIndex = 0;
                 
                }
                if (purchaseOrder.transshipment == false)
                { cmbTransshipment.SelectedIndex = 1;
                 
                }
            }
            chkInterCompany.IsChecked = purchaseOrder.isInterCompany;

            double invoiced = 0;

            if (purchaseOrder.PurchaseOrdertype == InquiryType.Principal)
            {
                pbarTarget.Maximum = Convert.ToDouble(purchaseOrder.Commision);


                POCFRRemaining = Convert.ToDouble(purchaseOrder.Commision);

                if (purchaseOrder.InvoiceStage == null)
                {
                    purchaseOrder.RemainingCFRValue = Convert.ToDouble(purchaseOrder.Commision);
                    invoiced = Convert.ToDouble(purchaseOrder.Commision) - purchaseOrder.RemainingCFRValue;

                }
                else if (purchaseOrder.InvoiceStage == InvoiceStage.None.ToString() && purchaseOrder.RemainingCFRValue == 0)
                {
                    purchaseOrder.RemainingCFRValue = Convert.ToDouble(purchaseOrder.Commision);

                    invoiced = Convert.ToDouble(purchaseOrder.Commision) - purchaseOrder.RemainingCFRValue;

                }
                else
                    invoiced = Convert.ToDouble(purchaseOrder.Commision) - purchaseOrder.RemainingCFRValue;

            }
            else
            {
                pbarTarget.Maximum = purchaseOrder.totalCFRValue;
                POCFRRemaining = purchaseOrder.totalCFRValue;

                if (purchaseOrder.InvoiceStage == null)
                {
                    purchaseOrder.RemainingCFRValue = purchaseOrder.totalCFRValue;
                    invoiced = purchaseOrder.totalCFRValue - purchaseOrder.RemainingCFRValue;

                }
                else if (purchaseOrder.InvoiceStage == InvoiceStage.None.ToString() && purchaseOrder.RemainingCFRValue == 0)
                {
                    purchaseOrder.RemainingCFRValue = purchaseOrder.totalCFRValue;

                    invoiced = purchaseOrder.totalCFRValue - purchaseOrder.RemainingCFRValue;
                }
                else
                    invoiced = purchaseOrder.totalCFRValue - purchaseOrder.RemainingCFRValue;
            }

            pbarTarget.Value = invoiced;
            var percent = Math.Round((pbarTarget.Value / pbarTarget.Maximum) * 100);
            txtPbarValue.Text = "(" + percent.ToString() + "%) ";

            txttotalfob.Text = purchaseOrder.totalFOBValue.ToString();
            txtPOremainingcfr.Text = Math.Round(purchaseOrder.RemainingCFRValue, 2).ToString();
            txttotalcfr.Text = purchaseOrder.totalCFRValue.ToString();
            txtBasetotalfob.Text = purchaseOrder.totalBaseFOBValue.ToString();
            txtBasetotalcfr.Text = purchaseOrder.totalBaseCFRValue.ToString();
            txtSalestotalCfr.Text = purchaseOrder.POAmountSER.ToString();
            txtMaker.Text = purchaseOrder.maker;
            txtOrigin.Text = purchaseOrder.origin;
            
            txtSOCER.Text = Convert.ToString(purchaseOrder.SOC_ER);
            txtSoAmountSOC.Text = Convert.ToString(purchaseOrder.SoAmountSOC_ER);
            txtOwnDescription.Text = purchaseOrder.OwnDescription;
            txtCommissionNumber.Text = purchaseOrder.VendorName;
            txtFinanaceRef.Text = purchaseOrder.FinanceRefrenceNo;

           
            //txtSalesTargetYear.Text = purchaseOrder.TargetYear.ToString();
            //txtSalesTargetMonth.Text = purchaseOrder.TargetMonth.ToString();

            txtComments.Text = purchaseOrder.comments;


            txtPaymentDueDays.Text = purchaseOrder.CreditDays.ToString();
           


            lblStage.Text = (purchaseOrder.stage != null) ? purchaseOrder.stage : "";
            txtLCNumber.Text = purchaseOrder.LCnumber;
 
            if (purchaseOrder.isPercentTax == true)
                txttax.Text = purchaseOrder.salesTax.ToString() + "%";
            else
                txttax.Text = purchaseOrder.salesTax.ToString();
            //inco term for po

            if (purchaseOrder.incoterm_Id != 0 && purchaseOrder.incoterm_Id != null)
            {
                var incoSource = (List<cmbitem>)cmbIncoterm.Items.SourceCollection;
                //var incoSourceChange = (List<cmbitem>)cmbIncotermChange.Items.SourceCollection;
                cmbIncoterm.SelectedItem = cmbIncoterm.Items[cmbIncoterm.Items.IndexOf(incoSource.Find(x => x.id == purchaseOrder.incoterm_Id))];
                //cmbIncotermChange.SelectedItem = cmbIncotermChange.Items[cmbIncotermChange.Items.IndexOf(incoSourceChange.Find(x => x.id == purchaseOrder.incoterm_Id))];
                //cmbSOIncotermChange.SelectedItem = cmbIncotermChange.Items[cmbIncotermChange.Items.IndexOf(incoSourceChange.Find(x => x.id == purchaseOrder.incoterm_Id))];

            }
            //Warranty
            if (purchaseOrder.SOWarrantyId != 0 /*|| purchaseOrder.SOWarranty != null*/)
            {
                var incoSource = (List<cmbitem>)cmbSOWarranty.Items.SourceCollection;
                //var incoSourceChange = (List<cmbitem>)cmbSOWarrantyChange.Items.SourceCollection;
                if(purchaseOrder.SOWarrantyId!=null)
                cmbSOWarranty.SelectedItem = cmbSOWarranty.Items[cmbSOWarranty.Items.IndexOf(incoSource.Find(x => x.id == purchaseOrder.SOWarrantyId))];
                //cmbSOWarrantyChange.SelectedItem = cmbSOWarrantyChange.Items[cmbSOWarrantyChange.Items.IndexOf(incoSourceChange.Find(x => x.id == purchaseOrder.SOWarrantyId))];

            }
            if (purchaseOrder.POWarrantyId != 0 && purchaseOrder.POWarrantyId != null)
            {
                var incoSource = (List<cmbitem>)cmbPOWarranty.Items.SourceCollection;
                //var incoSourceChange = (List<cmbitem>)cmbPOWarrantyChange.Items.SourceCollection;

                cmbPOWarranty.SelectedItem = cmbPOWarranty.Items[cmbPOWarranty.Items.IndexOf(incoSource.Find(x => x.id == purchaseOrder.POWarrantyId))];
                //cmbPOWarrantyChange.SelectedItem = cmbPOWarrantyChange.Items[cmbPOWarrantyChange.Items.IndexOf(incoSourceChange.Find(x => x.id == purchaseOrder.POWarrantyId))];

            }
            // Select Creator
            if (purchaseOrder.user != null)
                txtCreator.Text = purchaseOrder.user.employee.person.FName + " " + purchaseOrder.user.employee.person.LName;
            if (purchaseOrder.company_Id != null || purchaseOrder.company != null)
            {
                company = purchaseOrder.company;
                lookupCompany.Text = purchaseOrder.company.CompanyName;
                //lookupCompany.SelectedItem = lookupCompany.GetItemByKeyValue(purchaseOrder.company);



                //loaddepartments();
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }
            // Select Department
            if (purchaseOrder.dept_Id != 0 || purchaseOrder.department != null)
            {
                departmentParent = 1;
                lookupDepartment.Text = purchaseOrder.department.DeptName;
                //lookupDepartment.SelectedItem = lookupDepartment.GetItemByKeyValue(purchaseOrder.department);
                department = purchaseOrder.department;

                //lookupCustomer.ItemsSource = department.customers;
                loademployees();

            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }

            
            
            //Payment term Load for sale Order
            if (purchaseOrder.SoPaymentterm_Id != 0 && purchaseOrder.SoPaymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbSOPaymentTerm.Items.SourceCollection;
                //var paymentTermSourceChange = (List<cmbitem>)cmbSOPaymentTermChange.Items.SourceCollection;


                //cmbSOPaymentTerm.SelectedItem = cmbSOPaymentTerm.Items[cmbSOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == purchaseOrder.SoPaymentterm_Id))];
                //cmbSOPaymentTermChange.SelectedItem = cmbSOPaymentTermChange.Items[cmbSOPaymentTermChange.Items.IndexOf(paymentTermSourceChange.Find(x => x.id == purchaseOrder.SoPaymentterm_Id))];

                var term = paymentTermSource.Find(x => x.id == purchaseOrder.SoPaymentterm_Id);
                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = purchaseOrder.SoPaymentTerm.term, id = purchaseOrder.SoPaymentTerm.Id });
                    cmbSOPaymentTerm.ItemsSource = null;
                    cmbSOPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbSOPaymentTerm.SelectedItem = cmbSOPaymentTerm.Items[cmbSOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == purchaseOrder.SoPaymentterm_Id))];

            }
            //Payment term Load for sale Order
            if (purchaseOrder.POPaymentterm_Id != 0 && purchaseOrder.POPaymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbPOPaymentTerm.Items.SourceCollection;
                //var paymentTermSourceChange = (List<cmbitem>)cmbPOPaymentTermChange.Items.SourceCollection;


                //cmbPOPaymentTerm.SelectedItem = cmbPOPaymentTerm.Items[cmbPOPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == purchaseOrder.POPaymentterm_Id))];
                //cmbPOPaymentTermChange.SelectedItem = cmbPOPaymentTermChange.Items[cmbPOPaymentTermChange.Items.IndexOf(paymentTermSourceChange.Find(x => x.id == purchaseOrder.POPaymentterm_Id))];

                var term = paymentTermSource.Find(x => x.id == purchaseOrder.POPaymentterm_Id);
                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = purchaseOrder.POPaymentTerm.term, id = purchaseOrder.POPaymentTerm.Id });
                    cmbPOPaymentTerm.ItemsSource = null;
                    cmbPOPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbPOPaymentTerm.SelectedItem = cmbPOPaymentTerm.Items[cmbPOPaymentTerm.Items.IndexOf(term)];
            }
            if (purchaseOrder.SOCurrency_Id != 0 && purchaseOrder.SOCurrency_Id != null)
            {
                var currencySource = (List<cmbitem>)cmbSOCurrency.Items.SourceCollection;
                cmbSOCurrency.SelectedItem = cmbSOCurrency.Items[cmbSOCurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.SOCurrency_Id))];


            }
            
            if (purchaseOrder.saleOrder_Id != null)
            {
                saleOrder = purchaseOrder.SaleOrder;

                if (saleOrder.deliveryDateFinal != null)
                {
                    datDeliverydateFinalChange.EditValue = saleOrder.deliveryDateFinal;
                }
            }
            // Select Company
            
            if (purchaseOrder.InterCompany_Id != null || purchaseOrder.InterCompany != null)
            {
                var companylist = (lookupInterCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupInterCompany.ItemsSource as List<Company>;
                if (purchaseOrder.InterCompany != null && companylist.Find(x => x.Id == purchaseOrder.InterCompany_Id) == null)
                {
                    companylist.Add(purchaseOrder.InterCompany);
                    lookupInterCompany.ItemsSource = companylist;
                }
                InterCompany = purchaseOrder.InterCompany;
                lookupInterCompany.Text = purchaseOrder.InterCompany.CompanyName;
            }
            if (purchaseOrder.InterDepartment_Id != null || purchaseOrder.InterDepartment != null)
            {
                var deptlist = (lookupInterDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupInterDepartment.ItemsSource as List<Department>;

                if (purchaseOrder.InterDepartment != null && deptlist.Find(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                {
                    deptlist.Add(purchaseOrder.InterDepartment);
                    lookupInterDepartment.ItemsSource = deptlist;
                }
                lookupInterDepartment.Text = purchaseOrder.InterDepartment.DeptName;
                InterDepartment = purchaseOrder.InterDepartment;
            }
     
            //Select Customer
            if (purchaseOrder.customerCompany.Id != 0 || purchaseOrder.customerCompany != null)
            {
                customerParent = 1;
               
                lookupCustomer.Text = purchaseOrder.customerCompany.company.CompanyName;
                customerParentName = lookupCustomer.Text;
                //lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(purchaseOrder.customerCompany);

                customer = purchaseOrder.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            //Select vendor
            if (purchaseOrder.vendors != null)
            {

                foreach (Vendor vendr in purchaseOrder.vendors)
                {
                    lookupVendor.Text = vendr.company.CompanyName;
                    vendor = vendr;
                }
            }
            //if (purchaseOrder.vendor_Id != 0 || purchaseOrder.vendor != null)
            //{
            //    lookupVendor.Text = purchaseOrder.vendor.company.CompanyName;
            //    vendor = purchaseOrder.vendor;
            //}
            //else
            //{
            //    lookupVendor.Text = "Select Vendor";

            //}
            //Select Principal
            //if (purchaseOrder.principal_Id != 0 || purchaseOrder.principal != null)
            //{
            //    lookupPrincipal.Text = purchaseOrder.principal.company.CompanyName;
            //    principal = purchaseOrder.principal;
            //}
            //else
            //{
            //    lookupPrincipal.Text = "Select Vendor";

            //}
            // Select Employee 
            if (purchaseOrder.allocation_Id != 0 || purchaseOrder.AllocateTo != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == purchaseOrder.allocation_Id))];

                }
                catch (Exception ex)
                {

                }
                //foreach (cmbitem cmbitem in cmbEmployee.Items)
                //{
                //    if (cmbitem.id == purchaseOrder.allocation_Id)
                //    {
                //        cmbEmployee.SelectedItem = cmbitem;
                //        break;
                //    }
                //}
            }
            if (purchaseOrder.bid_Id != null && purchaseOrder.bid_Id != 0 && purchaseOrder.bid != null)
            { //load form data for bid
                grpbondinfo.State = GroupBoxState.Normal;
                datBidBondIssuedate.DateTime = purchaseOrder.bid.issueDate;
                txtBidBondRefno.Text = purchaseOrder.bid.refNo;
                txtBidBonValue.Text = purchaseOrder.bid.value;
                datBidBondSubmitdate.DateTime = purchaseOrder.bid.submitDate;
                datBidBondExpirydate.DateTime = purchaseOrder.bid.expireDate;
                txtIssuingbank.Text = purchaseOrder.bid.bankName;
            }
            //datBidOpendate.DateTime = purchaseOrder.bidOpenDate;
            // Delete Inquiry Type Same for PurchaseOrder Type

            {

            }
            if (purchaseOrder.PurchaseOrderStatus != null)
            {
                var disAbleStatus = PurchaseOrderStatuses.FirstOrDefault(x => x.Id == purchaseOrder.PurchaseOrderStatus.Id);
                if (disAbleStatus == null)
                {
                    loadPurchaseOrderStatus(purchaseOrder.PurchaseOrderStatus);
                }
            }
            var POSource = (List<cmbitem>)cmbPurchaseOrderStatus.Items.SourceCollection;
            ////loadingPoChangeData
            //var POSourceChange = (List<cmbitem>)cmbPurchaseOrderStatusChange.Items.SourceCollection;
           

            // Select PurchaseOrder Status 
            if (purchaseOrder.PurchaseOrderStatus.isActive == false)
            {
                try
                {
                    cmbPurchaseOrderStatus.SelectedItem = cmbPurchaseOrderStatus.Items[cmbPurchaseOrderStatus.Items.IndexOf(POSource.Find(x => x.name == purchaseOrder.PurchaseOrderStatus.Status))];
                    //loadingPOChangeData
                    //cmbPurchaseOrderStatusChange.SelectedItem = cmbPurchaseOrderStatusChange.Items[cmbPurchaseOrderStatusChange.Items.IndexOf(POSourceChange.Find(x => x.name == purchaseOrder.PurchaseOrderStatus.Status))];


                }

                catch (Exception ex)
                {
                    //DXMessageBox.Show("This User cannot see closed Purchase Order status!");
                    SystemLog.LogError(this.GetType(), "This User cannot see closed Purchase Order status! " + ex.ToString());
                    //SystemLog.LogInfo(this.GetType(), ex.ToString());

                }

            }
            else
            {
                var POstatus = POSource.Find(x => x.name == purchaseOrder.PurchaseOrderStatus.Status);
                cmbPurchaseOrderStatus.SelectedItem = cmbPurchaseOrderStatus.Items[cmbPurchaseOrderStatus.Items.IndexOf(POstatus)];
                //cmbPurchaseOrderStatusChange.SelectedItem = cmbPurchaseOrderStatusChange.Items[cmbPurchaseOrderStatusChange.Items.IndexOf(POSourceChange.Find(x => x.name == purchaseOrder.PurchaseOrderStatus.Status))];

            }


            //foreach (cmbitem cmbitem in cmbPurchaseOrderStatus.Items)
            //{
            //    if (cmbitem.name == purchaseOrder.PurchaseOrderStatus.Status)
            //    {
            //        cmbPurchaseOrderStatus.SelectedItem = cmbitem;
            //        break;
            //    }
            //}
            checkStatus = purchaseOrder.PurchaseOrderStatus;

            //var VendorPaymentSource = (List<cmbitem>)cmbVendorPaymentStatus.Items.SourceCollection;
            //cmbIncoterm.SelectedItem = VendorPaymentSource.Find(x => x.name == purchaseOrder.vendorPaymentStatus.Status);

            //foreach (cmbitem cmbitem in cmbVendorPaymentStatus.Items)
            //{
            //    if (purchaseOrder.vendorPaymentStatus != null)
            //        if (cmbitem.name == purchaseOrder.vendorPaymentStatus.Status)
            //        {
            //            cmbVendorPaymentStatus.SelectedItem = cmbitem;
            //            break;
            //        }
            //}
            if (purchaseOrder.vendorPaymentId != null || purchaseOrder.vendorPaymentStatus != null)
            {
                var currencySource = (List<cmbitem>)cmbVendorPaymentStatus.Items.SourceCollection;
                //var currencySourceChange = (List<cmbitem>)cmbVendorPaymentStatusChange.Items.SourceCollection;

                cmbVendorPaymentStatus.SelectedItem = cmbVendorPaymentStatus.Items[cmbVendorPaymentStatus.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.vendorPaymentId))];
                //cmbVendorPaymentStatusChange.SelectedItem = cmbVendorPaymentStatusChange.Items[cmbVendorPaymentStatusChange.Items.IndexOf(currencySourceChange.Find(x => x.id == purchaseOrder.vendorPaymentId))];

            }
            //int a = -1, b = -1;
            // load Products in Inquiry to grid
            List<DataGridItem> datagriditems = new List<DataGridItem>();


            if (purchaseOrder.PurchaseOrdertype == InquiryType.DistributionBiz)
            {

                if (purchaseOrder.BookerStatementItems.Count > 0)
                {
                    grdBokkerItems.ItemsSource = purchaseOrder.BookerStatementItems;
                }

            }
            else
            {
                if (purchaseOrder.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                    //grdPOItems.ItemsSource = offer.products;
                    foreach (var procurementProduct in purchaseOrder.products)
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
                            priority = procurementProduct.priority,

                            //UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity,
                            //InvoicedQuantity = (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,
                            InvoicedQuantity = CalculateUninvoicedQty(procurementProduct) != 0 ? CalculateUninvoicedQty(procurementProduct) :
                            (procurementProduct.InvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.InvoicedQuantity,


                            UnInvoicedQuantity = (CalculateUninvoicedQty(procurementProduct)) != 0 ?
                            (procurementProduct.inquiryProduct.quantity - CalculateUninvoicedQty(procurementProduct)) :
                            ((procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.InvoicedQuantity : procurementProduct.UnInvoicedQuantity),


                            // UnInvoicedWeight = (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,
                            //InvoicedWeight = (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,
                            InvoicedWeight = CalculateUninvoicedWeight(procurementProduct) != 0 ? CalculateUninvoicedWeight(procurementProduct) : (procurementProduct.InvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight : procurementProduct.InvoicedWeight,

                            UnInvoicedWeight = CalculateUninvoicedWeight(procurementProduct) != 0 ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - CalculateUninvoicedWeight(procurementProduct) :
                            (procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.InvoicedWeight : procurementProduct.UnInvoicedWeight,

                            value1 = procurementProduct.value1,
                            value2 = procurementProduct.value2,
                            caption1 = cmbcaption1.Text.Trim(),
                            caption2 = cmbcaption2.Text.Trim(),
                            //UnInvoicedSoAmount =  procurementProduct.UnInvoicedSoAmount
                            UnInvoicedSoAmount = (CalculateInvoicedSoAmount(procurementProduct) != 0) ? (procurementProduct.value2 != 0 ? procurementProduct.value2 : procurementProduct.value1) - (CalculateInvoicedSoAmount(procurementProduct)) : procurementProduct.UnInvoicedSoAmount


                        });



                    }
                    //dGitems.ItemsSource = datagriditems;
                    grdPOItems.ItemsSource = procurementProducts;
                    int x = 0;
                    foreach (var pro in grdPOItems.ItemsSource as List<ProcurementProduct>)
                    {
                        grdPOItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                        x++;

                    }
                    lookupProductsinGrid.DisplayMember = "code";

                }
                else
                {
                    grdPOItems.ItemsSource = procurementProducts;
                }
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPOItems);

            if (purchaseOrder.currency_Id != 0 && purchaseOrder.currency_Id != null)
            {
                var currencySource = (List<cmbitem>)cmbPOCurrency.Items.SourceCollection;

                var currList = cmbPOCurrency.ItemsSource as List<cmbitem>;
                cmbPOCurrency.SelectedItem = currList.FirstOrDefault(x => x.id == purchaseOrder.currency_Id);

                //cmbPOCurrency.SelectedItem = cmbPOCurrency.Items[cmbPOCurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.currency_Id))];


            }
            else
            {
                var currencySource = (List<cmbitem>)cmbPOCurrency.Items.SourceCollection;
                cmbPOCurrency.SelectedItem = cmbPOCurrency.Items[cmbPOCurrency.Items.IndexOf(currencySource.Find(x => x.id == purchaseOrder.company.CurrencyId))];


            }



            //select Captions for Item Value 1
            if (purchaseOrder.TitleValue1Id != 0 && purchaseOrder.TitleValue1Id != null)
            {
                var captionSource = (List<cmbitem>)cmbcaption1.Items.SourceCollection;

                cmbcaption1.SelectedItem = cmbcaption1.Items[cmbcaption1.Items.IndexOf(captionSource.Find(x => x.id == purchaseOrder.TitleValue1Id))];

            }
            //select Captions for Item Value 2
            if (purchaseOrder.TitleValue2Id != 0 && purchaseOrder.TitleValue2Id != null)
            {
                var caption2Source = (List<cmbitem>)cmbcaption2.Items.SourceCollection;
                cmbcaption2.SelectedItem = cmbcaption2.Items[cmbcaption2.Items.IndexOf(caption2Source.Find(x => x.id == purchaseOrder.TitleValue2Id))];
            }
            //cmbcaption1.SelectedIndex = a;
            //cmbcaption2.SelectedIndex = b;

            // selected currency of customer company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                //if (inquiry.customerCompany.company!=null)
                //    if (cmbitem.id == inquiry.customerCompany.company.CurrencyId)
                if (cmbitem.id == purchaseOrder.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            //txtexchangerate.Text = purchaseOrder.ExchangeRate.ToString();
            //txtMarginexchangerate.Text = purchaseOrder.marginExchangeRate.ToString();
            txtCommision.Text = purchaseOrder.Commision.ToString();
            txtBaseCommission.Text = purchaseOrder.commisioninBase.ToString();
            //Margin
            if (purchaseOrder.SaleOrder != null && purchaseOrder.CostSheet != null)
            {
                if (purchaseOrder.SaleOrder.saleOrdertype == InquiryType.SupplyCCC)
                {
                    txtBudgetMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.costCenterAmount) - purchaseOrder.CostSheet.TotalBudgetedMargin).ToString();
                    txtRevisedMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.costCenterAmount) - purchaseOrder.CostSheet.TotalRevisedMargin).ToString();
                    txtActualMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.costCenterAmount) - purchaseOrder.CostSheet.TotalActualMargin).ToString();
                }
                else
                {
                    txtBudgetMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalBudgetedMargin).ToString();
                    txtRevisedMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalRevisedMargin).ToString();
                    txtActualMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalActualMargin).ToString();
                }
            }
            else
            {
                txtBudgetMargin.Text = purchaseOrder.margin.ToString();
                txtRevisedMargin.Text = purchaseOrder.RevisedMargin.ToString();
                txtBaseRevisedMargin.Text = purchaseOrder.RevisedMargininBase.ToString();
                txtSaleRevisedMargin.Text = purchaseOrder.SalesRevisedMargin.ToString();

                txtActualMargin.Text = purchaseOrder.ActualMargin.ToString();
                txtBaseBudgetMargin.Text = purchaseOrder.BudgetedMargininBase.ToString();
                txtBaseActualMargin.Text = purchaseOrder.ActualMargininBase.ToString();
                txtSaleBudgetMargin.Text = purchaseOrder.SalesBudgetedMargin.ToString();
                txtSaleAMargin.Text = purchaseOrder.SalesActualMargin.ToString();
            }
                

            txtNetCommision.Text = (purchaseOrder.NetCommision).ToString();

            calculatetotal();


            int index = 0;
            if (purchaseOrder.hasTax == true)
            {
                if (purchaseOrder.isAdjustedTax == true)
                {
                    chkAdjustedTax.IsChecked = true;
                }
                else
                {
                    chkTax.IsChecked = true;
                }

                //Select Company
                var taxList = (lookUpTax.ItemsSource as List<TaxName>) == null ? new List<TaxName>() : lookUpTax.ItemsSource as List<TaxName>;
                if (purchaseOrder.tax_Id != null)
                {
                    index = 0;
                    foreach (var _tax in taxList)
                    {
                        if (_tax.Id == purchaseOrder.tax_Id)
                        {
                            lookUpTax.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    lookUpTax.Text = "Select Tax";
                }

                txtBillWithTax.Text = purchaseOrder.billWithTax.ToString();
            }

            if (purchaseOrder.hasWHT == true)
            {
                chkWHT.IsChecked = true;

                //Select WHT
                var whtList = (lookUpWHT.ItemsSource as List<TaxName>) == null ? new List<TaxName>() : lookUpWHT.ItemsSource as List<TaxName>;
                if (purchaseOrder.WHT_Id != null)
                {
                    index = 0;
                    foreach (var _WHT in whtList)
                    {
                        if (_WHT.Id == purchaseOrder.WHT_Id)
                        {
                            lookUpWHT.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    lookUpWHT.Text = "Select WHT";
                }

                txtBillAfterTax.Text = purchaseOrder.billAfterTax.ToString();
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Commision Field without Approval") == null)
            {
                txtCommision.IsReadOnly = true;
            }
            if (purchaseOrder.isApproved == false)
            {
                grdPOItems.Columns.GetColumnByFieldName("value1").ReadOnly = false;
                grdPOItems.Columns.GetColumnByFieldName("value2").ReadOnly = false;
            }
            else  if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit FOB and CFR value for products in PurchaseOrder") == null)
            {
                grdPOItems.Columns.GetColumnByFieldName("value1").ReadOnly = true;
                grdPOItems.Columns.GetColumnByFieldName("value2").ReadOnly = true;
                cmbPOCurrency.IsEnabled = false;
            }

            if (purchaseOrder.isApproved != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Purchase Order") != null)
                {
                    grdpurchaseOrderdata.IsEnabled = true;
                }
                else
                    grdpurchaseOrderdata.IsEnabled = false;
            }
            if (purchaseOrder.PurchaseOrderStatus.isActive == false && MainWindow.currentUserid != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed PurchaseOrder") == null || (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit (Pending for closing) PurchaseOrder") == null && purchaseOrder.PendingForClosing == false)))
            {
                grdpurchaseOrderdata.IsEnabled = false;
                btnAttachment.IsEnabled = false;

                labeltopStatus.Visibility = Visibility.Visible;
                labeltopStatus.Text = purchaseOrder.PurchaseOrderStatus.Status;
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(purchaseOrder.PurchaseOrderStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                labeltopStatus.Foreground = new SolidColorBrush(newColor);
                var rotateAnimation = new DoubleAnimation(-30, -32, TimeSpan.FromSeconds(60));
                var rt = (RotateTransform)labeltopStatus.RenderTransform;
                rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View CostSheet when Purchase Order closed") != null && purchaseOrder.PendingForClosing != true && purchaseOrder.Id != 0)
                {


                    grdpurchaseOrderdata.IsEnabled = true;
                    grpbasicinfo.IsEnabled = false;
                    grdSaleOrderData.IsEnabled = false;
                    grdPODetails.IsEnabled = false;
                    grdShpmentDetails.IsEnabled = false;
                    grdLCDetail.IsEnabled = false;
                    //grdGridControl.IsEnabled = false;
                    grdPOAmountmer.IsEnabled = false;
                    grpsummries.IsEnabled = false;
                    grpMargindetails.IsEnabled = false;
                    txtSoAmount1.IsEnabled = false;
                    cmbSOCurrency.IsEnabled = false;
                    txtSOCER.IsEnabled = false;
                    btnCostSheet.IsEnabled = true;
                    btnCostSheetPunching.IsEnabled = true;
                }

            }
            if (!string.IsNullOrEmpty(purchaseOrder.stlRefNo))
            {
                txtSTLrefChange.Text = purchaseOrder.stlRefNo;
            }
            if(iscopyTemplate==true)
            {
                purchaseOrder = new PurchaseOrder();
                datpoCreationdate.EditValue = DateTime.Now;
                editOrder = 0;
            }
            if (purchaseOrder.totaltaxAmount != 0)
            {
                if (purchaseOrder.isAdjustedTax == true)
                {
                    chkAdjustedTax.IsChecked = true;
                    var taxList = (lookUpTax.ItemsSource as List<TaxName>) == null ? new List<TaxName>() : lookUpTax.ItemsSource as List<TaxName>;
                    if (purchaseOrder.tax_Id != null)
                    {
                        index = 0;
                        foreach (var _tax in taxList)
                        {
                            if (_tax.Id == purchaseOrder.tax_Id)
                            {
                                lookUpTax.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }
                    else
                    {
                        lookUpTax.Text = "Select Tax";
                    }

                    txtBillWithTax.Text = purchaseOrder.billWithTax.ToString();
                }
                else
                {
                    chkTax.IsChecked = true;
                }
               
                txtTaxAmount.Text = purchaseOrder.totaltaxAmount.ToString();

                if (purchaseOrder.WHT == null)
                    txtBillAfterTax.Text = purchaseOrder.billWithTax.ToString();
            }
                                    
            if (purchaseOrder.SaleOrder != null && purchaseOrder.SaleOrder.CostSheet!=null && purchaseOrder.SaleOrder.CostSheet.FieldValues != null)
            {
                foreach (var item in saleOrderRepo.getActiveCostSheetFields())
                {
                    costFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title });
                }
                grdCostItems.ItemsSource = costFieldValue;
                loadValues();
            }
            if (purchaseOrder.transactionHolderId != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                    cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == purchaseOrder.transactionHolderId))];

                }
                catch (Exception ex)
                {

                }
            }
            if (purchaseOrder.holderChangeDate != null)
            {
                var time = DateTime.Now - purchaseOrder.holderChangeDate;
                txtHolderDays.Text = time.Days.ToString();
            }

            if (purchaseOrder.SaleOrder!= null)
            {
                bool isLinked = saleOrderRepo.GetParentOrders(purchaseOrder.SaleOrder.Id);

                if (purchaseOrder.SaleOrder.ParentSO_Id != null || isLinked == true)
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


        public void GellAllOrdersTracking()
        {
            //saleOrderRepo = new SaleOrderRepo();
            trackingOrder = purchaseOrderRepo.get(purchaseOrder.Id);
            if (trackingOrder != null)
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.Purchase_Order);
            }


            //if (trackingOrder != null)
            //{
            //    var companyIds = SYSTEM_STATIC.LoadCurrentUserCompanies().Select(x => x.Id).ToList();
            //    var deptIds = SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id).ToList();
            //    List<Bill> vendorBills = new List<Bill>();
            //    List<ERP_BL.Databases.PurchaseInvoice> purchaseInvoices = new List<ERP_BL.Databases.PurchaseInvoice>();
            //    List<ERP_BL.Payments.Payment> payments = new List<ERP_BL.Payments.Payment>();
            //    List<InterBankTransfer> ibts = new List<InterBankTransfer>();
            //    List<AllOrdersView> views = new List<AllOrdersView>();

            //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Bill") != null)
            //    {
            //        vendorBills = trackingOrder.Bills;
            //        vendorBills = vendorBills.Where(x => companyIds.Contains(x.company.Id) && deptIds.Contains(x.dept_Id)).ToList();

            //    }
            //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice") != null)
            //    {

            //            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices);

            //    }
            //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Payment") != null)
            //    {
            //        foreach (var purchaseInvoice in purchaseInvoices)
            //        {
            //            payments.AddRange(purchaseInvoice.Payments);
            //        }
            //    }
            //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") != null)
            //    {
            //        foreach (var vendorBill in vendorBills)
            //        {
            //            payments.AddRange(vendorBill.Payments);
            //        }
            //    }
            //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inter-Bank Transfer") != null)
            //    {
            //        foreach (var payment in payments)
            //        {
            //            InterBankTransRepo transRepo = new InterBankTransRepo();

            //            ibts.AddRange(transRepo.GetInterBankTransfersByPaymentGroupId(payment.transactionGroupId).Where(x => companyIds.Contains(x.company.Id) && deptIds.Contains((int)x.dept_Id)).ToList());
            //        }

            //    }
            //    if (vendorBills.Count > 0)
            //    {
            //        foreach (var bill in vendorBills)
            //        {
            //            int parentId = 0;
            //            if (bill.saleOrder_Id != null)
            //            {
            //                parentId = Convert.ToInt32(bill.saleOrder_Id);
            //            }
            //            if (bill.purchaseOrder_Id != null)
            //            {
            //                parentId = Convert.ToInt32(bill.purchaseOrder_Id);
            //            }
            //            string stage = "";
            //            if (bill.isVoid == true)
            //                stage = "Void";
            //            else if (bill.isReApproved == false)
            //                stage = "Under Approval";
            //            else if (bill.isApproved == true && bill.stage == "Closed")
            //                stage = "Closed";
            //            else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
            //                stage = "Closed";
            //            else if (bill.isApproved == true && bill.PendingForClosing == true)
            //                stage = "Under Closing";
            //            else if (bill.isApproved == true)
            //                stage = "Approved";
            //            else if (bill.isApproved == false)
            //                stage = "Under Approval";
            //            else if (bill.PendingForClosing == true)
            //                stage = "Under Closing";
            //            string vendor = "";
            //            if (bill.vendor != null)
            //            {
            //                vendor = bill.vendor.company.CompanyName;
            //            }
            //            AllOrdersView view = new AllOrdersView()
            //            {
            //                Id = bill.Id,
            //                transactionType = TransactionItemType.Bill,
            //                Company = bill.company.CompanyName,
            //                Department = bill.department.DeptName,
            //                CreationDate = (DateTime)bill.CreationDate,
            //                Currency = bill.currency.CurrencyName,
            //                SalesReference = bill.SalesReferenceNo,
            //                Stage = stage,
            //                Status = bill.BillStatus.Status,
            //                AmountOC = bill.totalCFRValue,
            //                GroupId = null,
            //                BackColor = bill.BillStatus.backcolor,
            //                ParentId = parentId,
            //                Vendor = vendor
            //            };
            //            views.Add(view);
            //        }
            //    }
            //    if (purchaseInvoices.Count > 0)
            //    {
            //        foreach (var invoice in purchaseInvoices)
            //        {
            //            int parentId = 0;
            //            if (invoice.purchaseOrder_Id != null)
            //            {
            //                parentId = Convert.ToInt32(invoice.purchaseOrder_Id);
            //            }
            //            string stage = "";
            //            if (invoice.isVoid == true)
            //                stage = "Void";
            //            else if (invoice.isReApproved == false)
            //                stage = "Under Approval";
            //            else if (invoice.isApproved == true && invoice.stage == "Closed")
            //                stage = "Closed";
            //            else if (invoice.isApproved == true && invoice.PurchaseInvoiceStatus.isActive == false && invoice.PendingForClosing != true)
            //                stage = "Closed";
            //            else if (invoice.isApproved == true && invoice.PendingForClosing == true)
            //                stage = "Under Closing";
            //            else if (invoice.isApproved == true)
            //                stage = "Approved";
            //            else if (invoice.isApproved == false)
            //                stage = "Under Approval";
            //            else if (invoice.PendingForClosing == true)
            //                stage = "Under Closing";
            //            string vendor = "";
            //            if (invoice.Vendor != null)
            //            {
            //                vendor = invoice.Vendor.company.CompanyName;
            //            }
            //            AllOrdersView view = new AllOrdersView()
            //            {
            //                Id = invoice.Id,
            //                transactionType = TransactionItemType.Purchase_Invoice,
            //                Company = invoice.company.CompanyName,
            //                Department = invoice.department.DeptName,
            //                CreationDate = (DateTime)invoice.CreationDate,
            //                Currency = invoice.currency.CurrencyName,
            //                SalesReference = invoice.SalesReferenceNo,
            //                Stage = stage,
            //                Status = invoice.PurchaseInvoiceStatus.Status,
            //                AmountOC = invoice.totalInvoiceAmount,
            //                GroupId = null,
            //                BackColor = invoice.PurchaseInvoiceStatus.backcolor,
            //                ParentId = parentId,
            //                Vendor = vendor
            //            };
            //            views.Add(view);
            //        }
            //    }
            //    if (payments.Count > 0)
            //    {
            //        foreach (var payment in payments)
            //        {
            //            int parentId = 0;
            //            if (payment.PInvoice_Id != null)
            //            {
            //                parentId = Convert.ToInt32(payment.PInvoice_Id);
            //            }
            //            if (payment.Bill_Id != null)
            //            {
            //                parentId = Convert.ToInt32(payment.Bill_Id);
            //            }
            //            string stage = "";
            //            if (payment.isVoid == true)
            //                stage = "Void";
            //            else if (payment.isReApproved == false)
            //                stage = "Under Approval";
            //            else if (payment.isApproved == true && payment.stage == "Closed")
            //                stage = "Closed";
            //            else if (payment.isApproved == true && payment.Status.isActive == false && payment.PendingForClosing != true)
            //                stage = "Closed";
            //            else if (payment.isApproved == true && payment.PendingForClosing == true)
            //                stage = "Under Closing";
            //            else if (payment.isApproved == true)
            //                stage = "Approved";
            //            else if (payment.isApproved == false)
            //                stage = "Under Approval";
            //            else if (payment.PendingForClosing == true)
            //                stage = "Under Closing";
            //            string vendor = "";
            //            if (payment.vendor != null)
            //            {
            //                vendor = payment.vendor.company.CompanyName;
            //            }
            //            AllOrdersView view = new AllOrdersView()
            //            {
            //                Id = payment.Id,
            //                transactionType = TransactionItemType.Payments,
            //                Company = payment.company.CompanyName,
            //                //Department = payment.departments.DeptName,
            //                CreationDate = (DateTime)payment.CreationDate,
            //                Currency = payment.currency.CurrencyName,
            //                SalesReference = payment.BillFinanceRefNo,
            //                Stage = stage,
            //                Status = payment.Status.Status,
            //                AmountOC = payment.DebitedAmount,
            //                GroupId = payment.transactionGroupId,
            //                BackColor = payment.Status.backcolor,
            //                ParentId = parentId,
            //                Vendor = vendor
            //            };
            //            views.Add(view);
            //        }
            //    }
            //    if (ibts.Count > 0)
            //    {
            //        foreach (var ibt in ibts)
            //        {
            //            int parentId = 0;
            //            if (ibt.paymentGroupId != 0)
            //            {
            //                parentId = Convert.ToInt32(ibt.paymentGroupId);
            //            }
            //            if (ibt.receiptGroupId != 0)
            //            {
            //                parentId = Convert.ToInt32(ibt.receiptGroupId);
            //            }
            //            string stage = "";
            //            if (ibt.isVoid == true)
            //                stage = "Void";
            //            else if (ibt.isReApproved == false)
            //                stage = "Under Approval";
            //            else if (ibt.isApproved == true && ibt.stage == "Closed")
            //                stage = "Closed";
            //            else if (ibt.isApproved == true && ibt.interBankTransStatus.isActive == false && ibt.PendingForClosing != true)
            //                stage = "Closed";
            //            else if (ibt.isApproved == true && ibt.PendingForClosing == true)
            //                stage = "Under Closing";
            //            else if (ibt.isApproved == true)
            //                stage = "Approved";
            //            else if (ibt.isApproved == false)
            //                stage = "Under Approval";
            //            else if (ibt.PendingForClosing == true)
            //                stage = "Under Closing";
            //            string currency = "";
            //            if (ibt.transferType == TransferType.IBT_Single_Currency)
            //            {
            //                currency = ibt.currency.CurrencyName;
            //            }
            //            else
            //            {
            //                currency = ibt.currencyFrom.CurrencyName;
            //            }
            //            string vendor = "";
            //            if (ibt.vendor != null)
            //            {
            //                vendor = ibt.vendor.company.CompanyName;
            //            }

            //            AllOrdersView view = new AllOrdersView()
            //            {
            //                Id = ibt.Id,
            //                transactionType = TransactionItemType.InterBank_Transfer,
            //                Company = ibt.company.CompanyName,
            //                Department = ibt.department.DeptName,
            //                CreationDate = (DateTime)ibt.CreationDate,
            //                Currency = currency,
            //                SalesReference = ibt.FinanceRefNo,
            //                Stage = stage,
            //                Status = ibt.interBankTransStatus.Status,
            //                AmountOC = ibt.AmountFrom,
            //                GroupId = null,
            //                BackColor = ibt.interBankTransStatus.backcolor,
            //                ParentId = parentId,
            //                Vendor = vendor
            //            };
            //            views.Add(view);
            //        }
            //    }




            //    views = views.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();

            //    grdOrdersTracking.ItemsSource = views;
            //}
        }
        public void loadValues()
        {

            if (purchaseOrder.SaleOrder != null)
            {
                if (purchaseOrder.SaleOrder.CostSheet != null)
                {

                    SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                    List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
                    costSheetPOFields = new List<CostSheetPOField>();
                    costSheetSOFields = new List<CostSheetSOField>();

                    List<CostFieldValues> costfields = new List<CostFieldValues>();
                    costfieldValues = grdCostItems.ItemsSource as List<CostFieldValues>;
                    if (purchaseOrderid != 0 && purchaseOrder.SaleOrder.CostSheet_Id != 0)
                    {
                        costSheetPOFields = saleOrderRepo.GetCostSheetPOFields(purchaseOrderid, (int)purchaseOrder.SaleOrder.CostSheet_Id);
                    }
                    if (saleOrder.Id != 0 && (int)purchaseOrder.SaleOrder.CostSheet_Id != 0)
                    {
                        costSheetSOFields = saleOrderRepo.GetCostSheetSOFields((int)purchaseOrder.saleOrder_Id, (int)purchaseOrder.SaleOrder.CostSheet_Id);
                    }

                    foreach (var item in costfieldValues)
                    {
                        foreach (var field in purchaseOrder.SaleOrder.CostSheet.FieldValues)
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
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {
                                        var value = saleOrderRepo.GetSystemCost((int)purchaseOrder.SaleOrder.CostSheet_Id, item.Id);
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
                                else if (field.Type == 8)
                                {
                                    if (costSheetPOFields.Count != 0)
                                    {
                                        var dbField = costSheetPOFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.PO_Cost && x.CostSheetId == (int)purchaseOrder.SaleOrder.CostSheet_Id && x.PO_Id == purchaseOrderid);
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
                                        var dbField = costSheetSOFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.soAmountSRBC && x.CostSheetId == (int)purchaseOrder.SaleOrder.CostSheet_Id && x.SO_Id == (int)purchaseOrder.SaleOrder.Id);
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
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {
                                        var value = saleOrderRepo.GetSBRC((int)purchaseOrder.SaleOrder.CostSheet_Id, item.Id);
                                        if (item.SRBC == 0 || saleOrder.isApproved != true)
                                        {
                                            item.SRBC = value;
                                        }
                                    }
                                }
                                else if (field.Type == 12)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {

                                        item.Maker = field.stringValue;

                                    }
                                }
                                else if (field.Type == 13)
                                {
                                    if (saleOrder.CostSheet != null && (int)purchaseOrder.SaleOrder.CostSheet_Id != 0)
                                    {
                                        item.Origin = field.stringValue;
                                    }
                                }
                                else if (field.Type == 14)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {

                                        item.Packing = field.isPacking;
                                    }
                                }
                                else if (field.Type == 15)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
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
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {
                                        item.LoadingPort = field.stringValue;
                                    }

                                }
                                else if (field.Type == 23)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {
                                        item.DestinationPort = field.stringValue;
                                    }
                                }
                                else if (field.Type == 24)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {
                                        SaleOrderRepo repo = new SaleOrderRepo();
                                        var document = repo.GetPQ((int)field.Value);
                                        item.PQ = document;
                                    }
                                }
                                else if (field.Type == 25)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {
                                        SaleOrderRepo repo = new SaleOrderRepo();
                                        var term = repo.GetST((int)field.Value);
                                        item.ST = term;
                                    }
                                }
                                else if (field.Type == 26)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {
                                        item.HScode = field.stringValue;
                                    }
                                }
                                else if (field.Type == 27)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {

                                        item.DrawaingRequired = field.drawingRequired;
                                    }
                                }
                                else if (field.Type == 28)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
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
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {

                                        item.isExportLicense = field.isExportLicense;
                                    }
                                }
                                else if (field.Type == 33)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {
                                        item.IntermediaryPort = field.stringValue;
                                    }
                                }
                                else if (field.Type == 34)
                                {
                                    if (purchaseOrder.SaleOrder.CostSheet_Id != null)
                                    {
                                        item.deliveryDays = field.stringValue;
                                    }
                                }

                            }
                        }
                        costfields.Add(item);
                    }
                    grdCostItems.ItemsSource = costfields;
                    List<CostFieldValues> finalCostfields = new List<CostFieldValues>();
                    costfieldCheckedValues = grdCostItems.ItemsSource as List<CostFieldValues>;
                    //costfields = costfieldCheckedValues.Where(x => x.actualValue > 0||x.SRBC>0  || x.budgetedValue > 0 || x.revisedValue > 0 || /*x.systemPayment > 0 ||*/ x.systemValue != 0 || x.vendor != null || x.billAmount > 0).ToList();
                    finalCostfields = costfieldCheckedValues.Where(x => x.actualValue != 0 || x.SRBC != 0 || x.budgetedValue != 0 || x.revisedValue != 0 /*&& x.systemPayment != 0*/ && x.systemValue <= 0 && x.vendor == null).ToList();

                    grdCostItems.ItemsSource = finalCostfields;
                }
            }
        }


        private double CalculateUninvoicedQty(ProcurementProduct procurementProduct)
        {
            var purchaseInvoices = purchaseOrder.PurchaseInvoices.Where(x=>x.isVoid != true).ToList();
            double result = 0;
            if (purchaseInvoices != null && purchaseInvoices.Count != 0)
            {
                foreach (var _PI in purchaseInvoices)
                {
                    var product = _PI.products.FirstOrDefault(x => x.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id);
                    if (product != null)
                    {
                        var invoiceQty = product.InvoicedQuantity;
                        result = result + invoiceQty;
                    }
                }
                
            }
            return result;

        }
        private double CalculateUninvoicedWeight(ProcurementProduct procurementProduct)
        {
            var purchaseInvoices = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).ToList();
            double result = 0;
            if (purchaseInvoices != null && purchaseInvoices.Count != 0)
            {
                foreach (var _PI in purchaseInvoices)
                {
                    var product = _PI.products.FirstOrDefault(x => x.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id);
                    if (product != null)
                    {
                        var invoiceWeight = product.InvoicedWeight;
                        result = result + invoiceWeight;
                    }
                }
            }
            return result;
        }

        private double CalculateInvoicedSoAmount(ProcurementProduct procurementProduct)
        {
            var purchaseInvoices = purchaseOrder.PurchaseInvoices.Where(x => x.isVoid != true).ToList();
            double result = 0;
            if (purchaseInvoices != null && purchaseInvoices.Count != 0)
            {
                foreach (var _PI in purchaseInvoices)
                {
                    var product = _PI.products.FirstOrDefault(x => x.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id);
                    if (product != null)
                    {
                        var invoiceAmount = product.NowAmount;
                        result = result + invoiceAmount;
                    }
                }
            }
            return result;
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
                        item.purchaseOrderId = purchaseOrderid;
                        item.unit = item.unit;

                        item.amount = item.amount;
                        item.amountGST = item.amountGST;
                        item.focValue = item.focValue;
                        item.netAmount = item.netAmount;
                        item.passOnValue = item.passOnValue;
                        item.quantity = item.quantity;
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
                        item.purchaseOrderId = purchaseOrderid;
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
                    item.purchaseOrderId = purchaseOrderid;
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

                    if (item.passOnId != null)
                    {
                        bItem.passOnId = offerRepo.GetPassOn((int)item.passOnId).Id;
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
            List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
            List<ProcurementProduct> purchaseOrderItems = new List<ProcurementProduct>();
            ProcurementProduct product = new ProcurementProduct();
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            procurementProducts = grdPOItems.ItemsSource as List<ProcurementProduct>;
            if (iscopyTemplate != false)
            {
                foreach (var procurementProduct in procurementProducts)
                {

                    ProcurementProduct product1 = new ProcurementProduct()
                    {
                        inquiryProduct = new InquiryProduct()
                        {
                            ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                            UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                            quantity = procurementProduct.inquiryProduct.quantity,
                            Weight = procurementProduct.inquiryProduct.Weight,
                            product_Id = procurementProduct.inquiryProduct.product.Id,
                        },
                        unitPrice = procurementProduct.unitPrice,
                        priority=procurementProduct.priority,
                        value1 = procurementProduct.value1,
                        value2 = procurementProduct.value2,
                        caption1 = cmbcaption1.Text.Trim(),
                        caption2 = cmbcaption2.Text.Trim(),

                    };
                    purchaseOrderItems.Add(product1);
                }
                return purchaseOrderItems;
            }
            else
            {
                
                    
                foreach (var procurementProduct in procurementProducts)
                {
                    if (procurementProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.Id == 0)
                        {
                            if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                            {
                                //product = inquiryProduct;
                                //product.product_Id = inquiryProduct.product.Id;
                                //product.UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                                //offerItems.Add(procurementProduct);
                                purchaseOrderItems.Add(new ProcurementProduct()
                                {
                                    inquiryProduct = new InquiryProduct()
                                    {
                                        Id = procurementProduct.inquiryProduct.Id,
                                        ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                        UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                        quantity = procurementProduct.inquiryProduct.quantity,
                                        Weight = procurementProduct.inquiryProduct.Weight,
                                        //product = new Product()
                                        //{
                                        //    Id = procurementProduct.inquiryProduct.product.Id,
                                        //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                        //    item = procurementProduct.inquiryProduct.product.item,
                                        //    code = procurementProduct.inquiryProduct.product.code,
                                        //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                        //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                        //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                        //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                        //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                        //    //nature = procurementProduct.inquiryProduct.product.nature,
                                        //    //category = procurementProduct.inquiryProduct.product.category,
                                        //    isActive = procurementProduct.inquiryProduct.product.isActive
                                        //}
                                        //,
                                        product_Id = procurementProduct.inquiryProduct.product.Id

                                    },
                                    product_Id = procurementProduct.inquiryProduct.Id,
                                    unitPrice = procurementProduct.unitPrice,
                                    priority = procurementProduct.priority,
                                    value1 = procurementProduct.value1,
                                    value2 = procurementProduct.value2,
                                    caption1 = cmbcaption1.Text.Trim(),
                                    caption2 = cmbcaption2.Text.Trim()


                                });
                            }
                        }
                        else
                        {
                            if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)

                                purchaseOrderItems.Add(new ProcurementProduct()
                                {
                                    inquiryProduct = new InquiryProduct()
                                    {
                                        Id = procurementProduct.inquiryProduct.Id,
                                        ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                        UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                        quantity = procurementProduct.inquiryProduct.quantity,
                                        Weight = procurementProduct.inquiryProduct.Weight,
                                        //product = new Product()
                                        //{
                                        //    Id = procurementProduct.inquiryProduct.product.Id,
                                        //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                        //    item = procurementProduct.inquiryProduct.product.item,
                                        //    code = procurementProduct.inquiryProduct.product.code,
                                        //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                        //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                        //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                        //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                        //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                        //    //nature = procurementProduct.inquiryProduct.product.nature,
                                        //    //category = procurementProduct.inquiryProduct.product.category,
                                        //    isActive = procurementProduct.inquiryProduct.product.isActive
                                        //}
                                        // ,
                                        product_Id = procurementProduct.inquiryProduct.product.Id

                                    },
                                    product_Id = procurementProduct.inquiryProduct.Id,
                                    unitPrice = procurementProduct.unitPrice,
                                    priority = procurementProduct.priority,
                                    value1 = procurementProduct.value1,
                                    value2 = procurementProduct.value2,
                                    caption1 = cmbcaption1.Text.Trim(),
                                    caption2 = cmbcaption2.Text.Trim()


                                });
                        }
                        //product = procurementProduct;
                        //product.product_Id = procurementProduct.inquiryProduct.product.Id;
                        //product.inquiryProduct.UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                        //offerItems.Add(product);
                    }
                    else
                    {
                        // if user is adding completely new prodcut first time.
                        if (procurementProduct.inquiryProduct.Id == 0)
                        {
                            if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                            {
                                //product = inquiryProduct;
                                //product.product_Id = inquiryProduct.product.Id;
                                //product.UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                                purchaseOrderItems.Add(new ProcurementProduct()
                                {
                                    Id = procurementProduct.Id,
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
                                            //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                            //nature = procurementProduct.inquiryProduct.product.nature,
                                            //category = procurementProduct.inquiryProduct.product.category,
                                            isActive = procurementProduct.inquiryProduct.product.isActive
                                        }
                                    },
                                    product_Id = procurementProduct.inquiryProduct.Id,
                                    unitPrice = procurementProduct.unitPrice,
                                    priority = procurementProduct.priority,
                                    UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                    UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                    UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                    value1 = procurementProduct.value1,
                                    value2 = procurementProduct.value2,
                                    caption1 = cmbcaption1.Text.Trim(),
                                    caption2 = cmbcaption2.Text.Trim()


                                });
                            }
                        }
                        else
                        {
                            // if user reloaded he offer and its product item is already there.
                            if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                            {
                                purchaseOrderItems.Add(new ProcurementProduct()
                                {
                                    Id= procurementProduct.Id,
                                    inquiryProduct = new InquiryProduct()
                                    {
                                        Id = procurementProduct.inquiryProduct.Id,
                                        ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                        UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                        quantity = procurementProduct.inquiryProduct.quantity,
                                        Weight = procurementProduct.inquiryProduct.Weight,
                                        //product = new Product()
                                        //{
                                        //    Id = procurementProduct.inquiryProduct.product.Id,
                                        //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                                        //    item = procurementProduct.inquiryProduct.product.item,
                                        //    code = procurementProduct.inquiryProduct.product.code,
                                        //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                                        //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                                        //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                                        //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                                        //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                                        //    //nature = procurementProduct.inquiryProduct.product.nature,
                                        //    //category = procurementProduct.inquiryProduct.product.category,
                                        //    isActive = procurementProduct.inquiryProduct.product.isActive
                                        //}
                                        // ,
                                        product_Id = procurementProduct.inquiryProduct.product.Id

                                    },
                                    product_Id = procurementProduct.inquiryProduct.Id,
                                    unitPrice = procurementProduct.unitPrice,
                                    priority = procurementProduct.priority,
                                    UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                    UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                    UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                    value1 = procurementProduct.value1,
                                    value2 = procurementProduct.value2,
                                    caption1 = cmbcaption1.Text.Trim(),
                                    caption2 = cmbcaption2.Text.Trim()


                                });
                            }
                        }
                    }
                }
                return purchaseOrderItems;
            }
        }
        private void btnPurchaseOrderSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (Convert.ToDouble(txtPaidAdvance.Text) + Convert.ToDouble(Convert.ToDouble(txtPOSattled.Text)) != Convert.ToDouble(txtPaid.Text))
                {
                    var inputfromUser = DXMessageBox.Show("PO Advance and PO Settled summery is not  equal to total Paid amount.\n Do you want to Edit?", "Required Fields", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                    {
                        return;
                    }
                }
                else
                if (Convert.ToDouble(txtSOAmount.Text) != Convert.ToDouble(txtsoInvoice.Text))
                {
                    if (Convert.ToDouble(txtsoInvoice.Text) != Convert.ToDouble(txtPOSattled.Text))
                    {
                        var inputfromUser1 = DXMessageBox.Show("SO Invoiced and PO Settled is are not equal" + "\n" + " Do you want to Edit?", "Required Fields", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (inputfromUser1 == System.Windows.MessageBoxResult.Yes)
                        {
                            return;
                        }
                        
                    }
                }
                if (cmbTransactionHolder.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }





                UsersRepo UsersRepo = new UsersRepo();
                ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                if (PoType == InquiryType.Standard || PoType==InquiryType.Supply ||PoType==InquiryType.Inventory)
                {
                    if (lookupVendor.SelectedIndex == -1 && vendor.Id == 0)
                    {
                        lookupVendor.Focus();
                        DXMessageBox.Show("Please Select a vendor against PO", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                        return;

                    }
                    else if (lookupCustomer.SelectedIndex == -1 && customer.Id == 0)
                    {
                        DXMessageBox.Show("Please Select a Customer for whom PO is being genrated", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupCustomer.Focus();
                        return;
                    }
                    else if (lookupCompany.SelectedIndex == -1 && company.Id == 0)
                    {
                        DXMessageBox.Show("Please select a Refrence Company", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupCompany.Focus();
                        return;
                    }
                    else if (lookupDepartment.SelectedIndex == -1 && department.Id == 0)
                    {
                        DXMessageBox.Show("Please Select a Refrence Department", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupDepartment.Focus();
                        return;
                    }
                    else if (cmbEmployee.SelectedIndex == -1 && employee.EmpId == 0)
                    {
                        DXMessageBox.Show("Please Select an Employee to whom this Purchase Order will be Allocated ", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbEmployee.Focus();
                        return;
                    }
                    else if (cmbPurchaseOrderStatusChange.SelectedIndex == -1 && purchaseOrder.PendingForClosing != true)
                    {
                        DXMessageBox.Show("Please Select Current Status of Purchase Order to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbPurchaseOrderStatusChange.Focus();
                        return;
                    }
                    else if (cmbPOCurrency.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select Purchase Orders Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbPOCurrency.Focus();
                        return;
                    }
                    else if (cmbSOCurrency.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select SaleOrder's Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbSOCurrency.Focus();
                        return;
                    }
                    //else if (Convert.ToDouble(txttotalcfr.Text) == 0 || 0 == Convert.ToDouble(txttotalfob.Text))
                    //{
                    //    DXMessageBox.Show("Please Enter Amount for the Items", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    //    grdPOItems.Focus();
                    //    return;
                    //}
                  
                    else if (cmbPOWarrantyChange.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select PO Warranty", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbPOPaymentTermChange.Focus();
                        return;
                    }
                  
                
                    else if (cmbIncotermChange.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select Incoterm", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbIncotermChange.Focus();
                        return;
                    }
                    else if (cmbcaption1.SelectedIndex == -1 || cmbcaption2.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please select a caption for Items Values", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbIncoterm.Focus();
                        return;
                    }
                    if (!string.IsNullOrEmpty(txtPOSystemGeneratedReferenceNumberChange.Text))
                        purchaseOrder.SyetmReferenceNo = txtPOSystemGeneratedReferenceNumberChange.Text;
                    if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.DistributionBiz)
                    {

                        purchaseOrder.BookerStatementItems = getBookerStatementItems();
                        purchaseOrder.totalAmount = Convert.ToDouble(txttotalcfr.Text);
                        purchaseOrder.totalAmountGST = Convert.ToDouble(txtTaxAmount.Text);
                        purchaseOrder.totalPassOn = Convert.ToDouble(txtPassOn.Text);
                        purchaseOrder.totalClaimDisount = Convert.ToDouble(txtDiscount.Text);
                        purchaseOrder.totalFocSampling = Convert.ToDouble(txtFocSampling.Text);
                    }
                    else
                    {
                        purchaseOrder.products = getProductsdata();
                    }

                    if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.DistributionBiz)
                    {
                        if (grdBokkerItems.VisibleRowCount== 0)
                        {
                            DXMessageBox.Show("Please Select items against which you want to create a Purchase Order", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                            //lookupDepartment.Focus();
                            return;
                        }
                    }
                    else
                    {
                        if (purchaseOrder.products.Count == 0)
                        {
                            DXMessageBox.Show("Please Select items against which you want to create a Purchase Order", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                            //lookupDepartment.Focus();
                            return;
                        }
                    }


                    if (!string.IsNullOrEmpty(txtLOTNo.Text))
                        purchaseOrder.lotNo = txtLOTNo.Text;
                    else
                        purchaseOrder.lotNo = null;

                    if (lookupLotNumbers.SelectedIndex > -1)
                        purchaseOrder.lotNumberId = (lookupLotNumbers.SelectedItem as LotNumber).Id;
                    else
                        purchaseOrder.lotNumberId = null;


                    if (cmbTransactionHolder.SelectedIndex != -1)
                    {
                        purchaseOrder.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                        purchaseOrder.holderChangeDate = (DateTime)datHolderDate.EditValue;
                    }
                    purchaseOrder.totalPOAdvance = Convert.ToDouble(txtPaidAdvance.Text);
                    purchaseOrder.expectedPaymentAmount = Convert.ToDouble(txtExpectedPaymentAmount.Text);

                    purchaseOrder.totalPOSattled = Convert.ToDouble(txtPOSattled.Text);
                    //SaleOrder-Details
                    {
                        if(costSheetFieldId!=null)
                        {
                            
                            purchaseOrder.CostSheetFieldId = (int)costSheetFieldId;
                        }
                        if (saleOrder.Id != 0 && saleOrder != null)
                        {
                            purchaseOrder.saleOrder_Id = saleOrder.Id;
                        }
                        else
                        {
                            purchaseOrder.saleOrder_Id = null;
                        }
                        purchaseOrder.SOReferenceNo = txtsaleOrderrefChange.Text.Trim();
                        if (cmbSOWarrantyChange.SelectedItem != null)
                            purchaseOrder.SOWarrantyId = (cmbSOWarrantyChange.SelectedItem as cmbitem).id;
                        if (cmbSOPaymentTermChange.SelectedItem != null)
                            purchaseOrder.SoPaymentterm_Id = (cmbSOPaymentTermChange.SelectedItem as cmbitem).id;

                    }

                    //Basic-Information
                    {
                        DateTime? dateTime = null;

                        purchaseOrder.PurchaseOrdertype = (InquiryType)cmbPurchaseOrderType.SelectedIndex;
                        purchaseOrder.CreationDate = (datpoCreationdate.Text == "") ? dateTime : datpoCreationdate.DateTime;
                        // selected company
                        if (company != null)
                        {
                            purchaseOrder.company_Id = company.Id;
                        }
                        // selected Department
                        if (department != null)
                        {

                            purchaseOrder.dept_Id = department.Id;
                        }
                        if (chkInterCompany.IsChecked == true)
                        {
                            if (InterCompany != null && InterCompany.Id != 0)
                            {

                                purchaseOrder.InterCompany_Id = InterCompany.Id;
                            }
                            if (InterDepartment != null && InterDepartment.Id != 0)
                            {

                                purchaseOrder.InterDepartment_Id = InterDepartment.Id;
                            }
                            purchaseOrder.isInterCompany = true;

                        }
                        else
                        {
                            purchaseOrder.isInterCompany = false;
                            purchaseOrder.InterCompany_Id = null;
                            purchaseOrder.InterDepartment_Id = null;
                        }
                        //selected customer
                        if (customer != null)
                        {

                            purchaseOrder.customerCompany_Id = customer.Id;
                        }
                        purchaseOrder.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;

                        // Selected Vendor 
                        if (vendor != null)
                        {
                            purchaseOrder.vendors = new List<Vendor>();
                            purchaseOrder.vendors.Add(purchaseOrderRepo.getVendor(vendor.Id));
                        }
                        if(!string.IsNullOrEmpty(txtSTLrefChange.Text))
                        {
                            purchaseOrder.stlRefNo = txtSTLrefChange.Text;
                        }
                    }
                    //PurchaseOrder-Details
                    {
                        DateTime? dateTime = null;
                        PurchaseOrder order; // = new PurchaseOrder();
                        order = purchaseOrderRepo.get(txtSalesrefChange.Text.Trim());   
                        if (order != null && order.Id != purchaseOrder.Id)
                        {
                            string message = "Purchase Order with Sales reference # (" + order.SalesReferenceNo + ") already exists!";
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
                    



                        purchaseOrder.FinanceRefrenceNo = txtFinanaceRefChange.Text;
                        if ((cmbPurchaseOrderStatusChange.SelectedItem as cmbitem) != null)
                        {
                            PurchaseOrderStatus status = purchaseOrderRepo.getstatus((cmbPurchaseOrderStatusChange.SelectedItem as cmbitem).id);
                            purchaseOrder.PurchaseOrderStatus = status;
                        }
                        purchaseOrder.SalesReferenceNo = txtSalesrefChange.Text.Trim();
                        purchaseOrder.VendorName = txtCommissionNumberChange.Text;
                        purchaseOrder.OfferReferenceNo = txtOfferRefNoChange.Text.Trim();
                        purchaseOrder.POReferenceNo = txtPOReferenceNumberChange.Text.ToString();
                        purchaseOrder.PurchaseOrderDate = (datPoDateChange.Text == "") ? dateTime : datPoDateChange.DateTime;
                        purchaseOrder.DeliveryDate = (datPODeliverydateChange.Text == "") ? dateTime : datPODeliverydateChange.DateTime;
                        purchaseOrder.proforma = txtProformaChange.Text;
                        if (cmbPOPaymentTermChange.SelectedItem != null)
                            purchaseOrder.POPaymentterm_Id = (cmbPOPaymentTermChange.SelectedItem as cmbitem).id;
                        if (cmbIncotermChange.SelectedItem != null)
                            purchaseOrder.incoterm_Id = (cmbIncotermChange.SelectedItem as cmbitem).id;
                        if (cmbcaption1.SelectedItem != null)
                            purchaseOrder.origin = txtOriginChange.Text.Trim();
                        purchaseOrder.maker = txtMakerChange.Text.Trim();
                        purchaseOrder.packing = txtPackingChange.Text;
                        if (cmbPOWarrantyChange.SelectedItem != null)
                            purchaseOrder.POWarrantyId = (cmbPOWarrantyChange.SelectedItem as cmbitem).id;
                        if (!string.IsNullOrEmpty(txtWeightChange.Text))
                        {
                            purchaseOrder.TotalWeight = Convert.ToDecimal(txtWeightChange.Text);
                        }
                        else
                        {
                            purchaseOrder.TotalWeight = null;
                        }
                        if (!string.IsNullOrEmpty(txtQuantityChange.Text))
                        {
                            purchaseOrder.TotalQuantity = Convert.ToDecimal(txtQuantityChange.Text);
                        }
                        else
                        {
                            purchaseOrder.TotalQuantity = null;
                        }
                    }
                    //Shipment-Details
                    if (!string.IsNullOrEmpty(txtTaxAmount.Text))
                    {
                        purchaseOrder.totaltaxAmount = Convert.ToDouble(txtTaxAmount.Text);
                    }
                    {
                        DateTime? dateTime = null;
                        purchaseOrder.ShipmentDate = (datShipmentdateChange.Text == "") ? dateTime : datShipmentdateChange.DateTime;
                        purchaseOrder.OrderConfirmationDate = (datOrderConfirmationdateChange.Text == "") ? dateTime : datOrderConfirmationdateChange.DateTime;
                        purchaseOrder.RevisedShipmentDate = (datRevisedShipmentDateChange.Text == "") ? dateTime : datRevisedShipmentDateChange.DateTime;
                        purchaseOrder.OwnDescription = txtOwnDescriptionChange.Text;
                        //Selected PurchaseOrder Status
                        if ((cmbVendorPaymentStatusChange.SelectedItem as cmbitem) != null)
                        {
                            purchaseOrder.vendorPaymentId = (cmbVendorPaymentStatusChange.SelectedItem as cmbitem).id;
                        }
                        purchaseOrder.MaterialReciptDate = (datMaterialReciptdateChange.Text == "") ? dateTime : datMaterialReciptdateChange.DateTime;
                        purchaseOrder.BillOfLaddingDate = (datBillOfLaddingdateChange.Text == "") ? dateTime : datBillOfLaddingdateChange.DateTime;
                        purchaseOrder.PaymentDueStartDate = (datPaymentDueFromChange.Text == "") ? dateTime : datPaymentDueFromChange.DateTime;
                        purchaseOrder.CreditDays = Convert.ToInt32(txtPaymentDueDaysChange.Text.Trim());
                        purchaseOrder.PaymentDueAgeing = (datPaymentDueTillChange.Text == "") ? dateTime : datPaymentDueTillChange.DateTime;
                        purchaseOrder.ExpectedPayment = (datExcpectedPaymentDateChange.Text == "") ? dateTime : datExcpectedPaymentDateChange.DateTime;
                    }
                    //LC-Information
                    {
                        DateTime? dateTime = null;
                        purchaseOrder.LCnumber = txtLCNumber.Text.Trim();
                        purchaseOrder.lCDate = (datLCDatedate.Text == "") ? dateTime : datLCDatedate.DateTime;
                        purchaseOrder.LCShipmentDate = (datLCShipmentDate.Text == "") ? dateTime : datLCShipmentDate.DateTime;
                        purchaseOrder.LCExpiryDate = (datLCExpiryDate.Text == "") ? dateTime : datLCExpiryDate.DateTime;
                        if (cmbTransshipment.SelectedIndex == 0)
                            purchaseOrder.transshipment = true;
                        if (cmbTransshipment.SelectedIndex == 1)
                            purchaseOrder.transshipment = false;
                        purchaseOrder.LCAmedmentNo = txtLCAmedmentno.Text;
                        purchaseOrder.LCShipmentAmendmentDate = (datLCRevisedShipmentDate.Text == "") ? dateTime : datLCRevisedShipmentDate.DateTime;
                        purchaseOrder.LCExpiryAmedmentDate = (datLCRevisedExpiryDate.Text == "") ? dateTime : datLCRevisedExpiryDate.DateTime;

                    }
                    //Captions
                    {
                        purchaseOrder.TitleValue1Id = (cmbcaption1.SelectedItem as cmbitem).id;
                        if (cmbcaption2.SelectedItem != null)
                            purchaseOrder.TitleValue2Id = (cmbcaption2.SelectedItem as cmbitem).id;
                    }

                    //Currency-Details
                    {
                        if(cmbPOCurrency.SelectedIndex!=-1)
                        {
                            purchaseOrder.currency_Id = (cmbPOCurrency.SelectedItem as cmbitem).id;
                        }
                        //// selected currency
                        //if (currency != null)
                        //{

                        //    purchaseOrder.currency_Id = currency.Id;
                        //}
                        purchaseOrder.SOCurrency_Id = (cmbSOCurrency.SelectedItem as cmbitem).id;
                        purchaseOrder.SOC_ER = (string.IsNullOrEmpty(txtSOCER.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCER.Text.Trim());
                        //purchaseOrder.PERValue = (string.IsNullOrEmpty(txtSOCERAmount.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCERAmount.Text.Trim());
                        purchaseOrder.SoAmountSOC_ER = (string.IsNullOrEmpty(txtSoAmountSOC.Text.Trim())) ? 0 : Convert.ToDouble(txtSoAmountSOC.Text.Trim());
                    }
                    //Commision-and-Margins
                    {
                        //purchaseOrder.SOC_ER = (string.IsNullOrEmpty(txtSOCER.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCER.Text.Trim());
                        if (txtCommision.Text != "" && txtBaseCommission.Text != "")
                        {
                            purchaseOrder.Commision = Convert.ToDouble(txtCommision.Text.Trim());
                            purchaseOrder.commisioninBase = Convert.ToDouble(txtBaseCommission.Text.Trim());
                        }
                        purchaseOrder.NetCommision = (string.IsNullOrEmpty(txtNetCommision.Text.Trim())) ? 0 : Convert.ToDouble(txtNetCommision.Text.Trim());
                        if (txtBudgetMargin.Text != "" /*&& txtBudgetMargin.Text != "0" && txtBudgetMargin.Text != "0.00"*/)
                        {
                            purchaseOrder.margin = Convert.ToDouble(txtBudgetMargin.Text.Trim());
                            purchaseOrder.BudgetedMargininBase = Convert.ToDouble(txtBaseBudgetMargin.Text.Trim());
                            purchaseOrder.SalesBudgetedMargin = Convert.ToDouble(txtSaleBudgetMargin.Text.Trim());
                        }
                        purchaseOrder.BudgetedMarginPercent = (string.IsNullOrEmpty(txtBudgetedPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtBudgetedPercent.Text, "[^0-9.]", ""));

                        if (txtRevisedMargin.Text != "")
                        {
                            purchaseOrder.RevisedMargin = (string.IsNullOrEmpty(txtRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtRevisedMargin.Text.Trim());
                            purchaseOrder.RevisedMargininBase = (string.IsNullOrEmpty(txtBaseRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtBaseRevisedMargin.Text.Trim());
                            purchaseOrder.SalesRevisedMargin = (string.IsNullOrEmpty(txtSaleRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtSaleRevisedMargin.Text.Trim());

                        }
                        purchaseOrder.RevisedMarginPercent = (string.IsNullOrEmpty(txtRevisedPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtRevisedPercent.Text, "[^0-9.]", ""));


                        if (txtActualMargin.Text != "" /*&& txtActualMargin.Text != "0" && txtActualMargin.Text != "0.00"*/)
                        {
                            purchaseOrder.ActualMargin = Convert.ToDouble(txtActualMargin.Text.Trim());
                            purchaseOrder.ActualMargininBase = Convert.ToDouble(txtBaseActualMargin.Text.Trim());
                            purchaseOrder.SalesActualMargin = Convert.ToDouble(txtSaleAMargin.Text.Trim());

                        }
                        purchaseOrder.ActualMarginPercent = (string.IsNullOrEmpty(txtActualPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtActualPercent.Text, "[^0-9.]", ""));

                    }

                    //Amounts-DragAndDropController-Tax
                    {
                        purchaseOrder.totalCFRValue = Convert.ToDouble(txttotalcfr.Text);


                        purchaseOrder.totalBaseCFRValue = Convert.ToDouble(txtBasetotalcfr.Text);
                        purchaseOrder.totalBaseFOBValue = Convert.ToDouble(txtBasetotalfob.Text);

                        purchaseOrder.totalFOBValue = Convert.ToDouble(txttotalfob.Text);

                        purchaseOrder.POAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);

                        purchaseOrder.RemainingCFRValue = Convert.ToDouble(txtPOremainingcfr.Text);

                        purchaseOrder.POAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);




                        purchaseOrder.ExchangeRate = (float)Convert.ToDecimal(txtexchangerate.Text.Trim());

                        purchaseOrder.marginExchangeRate = Convert.ToDouble(txtMarginexchangerate.Text.Trim());
                        string str = txttax.Text.Trim();

                        if (str.IndexOf("%") != -1)
                        {
                            purchaseOrder.isPercentTax = true;
                            purchaseOrder.salesTax = Convert.ToDouble(str.Substring(0, str.IndexOf("%")));
                        }
                        else
                        {
                            purchaseOrder.salesTax = Convert.ToDouble(str);
                            purchaseOrder.isPercentTax = false;
                        }


                    }
                    purchaseOrder.comments = txtComments.Text.Trim();

                    var myWindow = Window.GetWindow(this);

                   
                    if (chkTax.IsChecked == true )
                    {
                        purchaseOrder.hasTax = true;
                      
                        if(lookUpTax.SelectedIndex!=-1)
                        purchaseOrder.tax_Id = (lookUpTax.SelectedItem as TaxName).Id;
                        purchaseOrder.billWithTax = Convert.ToDouble(txtBillWithTax.Text);
                    }
                    else
                    {
                        purchaseOrder.hasTax = false;
                        purchaseOrder.tax_Id = null;
                        purchaseOrder.billWithTax = Convert.ToDouble(txtcfr.Text);
                    }

                    if (chkWHT.IsChecked == true)
                    {
                        purchaseOrder.hasWHT = true;
                        if (lookUpWHT.SelectedIndex < 0)
                        {
                            DXMessageBox.Show("Please Select WHT!");
                            lookUpWHT.Focus();
                            return;
                        }
                        purchaseOrder.WHT_Id = (lookUpWHT.SelectedItem as TaxName).Id;
                        purchaseOrder.billAfterTax = Convert.ToDouble(txtBillAfterTax.Text);
                    }
                    else
                    {
                        purchaseOrder.hasWHT = false;
                        purchaseOrder.WHT_Id = null;
                        purchaseOrder.billAfterTax = Convert.ToDouble(txtBillWithTax.Text);
                    }
                    if (chkAdjustedTax.IsChecked == true)
                    {
                        purchaseOrder.isAdjustedTax = true;
                        if (lookUpTax.SelectedIndex != -1)
                            purchaseOrder.tax_Id = (lookUpTax.SelectedItem as TaxName).Id;
                        purchaseOrder.billWithTax = Convert.ToDouble(txtBillWithTax.Text);
                    }
                    else
                    {
                        purchaseOrder.isAdjustedTax = false;
                    }
                    if (editOrder == 1 && OrderId != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Purchase Order") != null))
                    {
                        if (txtBidBondRefno.Text != "" && txtBidBonValue.Text != "" && txtIssuingbank.Text != "")
                        {
                            if (purchaseOrder.bid_Id == 0 && purchaseOrder.bid == null)
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
                                purchaseOrder.bid = bid;
                            }
                            else
                            { //get form data for bid
                                purchaseOrder.bid.issueDate = datBidBondIssuedate.DateTime;
                                purchaseOrder.bid.refNo = txtBidBondRefno.Text.Trim();
                                purchaseOrder.bid.value = txtBidBonValue.Text.Trim();
                                purchaseOrder.bid.submitDate = datBidBondSubmitdate.DateTime;
                                purchaseOrder.bid.expireDate = datBidBondExpirydate.DateTime;
                                purchaseOrder.bid.bankName = txtIssuingbank.Text.Trim();
                            }
                        }
                        if (MainWindow.currentUserid == 0)
                        {

                        }
                        else if (purchaseOrder.user_Id == null)
                            purchaseOrder.user_Id = MainWindow.currentUserid;

                        if (purchaseOrder.CostSheet != null && purchaseOrder.CostSheet.Timestamp != null)
                        {
                            purchaseOrder.CostSheet.Timestamp = System.DateTime.Now;
                        }


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null && purchaseOrder.isApproved != true)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Order is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {

                                purchaseOrder.stage = TransactionStage.Approved.ToString();

                                purchaseOrder.isApproved = true;
                                purchaseOrder.ApprovedDate = System.DateTime.Now;
                            }
                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without ReApproval") != null && purchaseOrder.isReApproved != true)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Order is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {

                                purchaseOrder.stage = TransactionStage.Approved.ToString();

                                purchaseOrder.isReApproved = true;
                                purchaseOrder.ReApprovalDate = System.DateTime.Now;
                            }
                        }
                        if (checkStatus != null && checkStatus.Id != 0)
                        {
                            if (checkStatus.Id != purchaseOrder.PurchaseOrderStatus.Id)
                            {
                                purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                                if (purchaseOrder.PurchaseOrderStatus.isActive != true)
                                {
                                    purchaseOrder.ClosingDate = System.DateTime.Now;
                                }
                            }
                        }
                        if(chkCOO.IsChecked==true)
                        {
                            purchaseOrder.COO =Convert.ToDouble(txtCOO.Text);
                        }
                        else
                        {
                            purchaseOrder.COO = 0;
                        }
                        if (chkDiscount.IsChecked == true)
                        {
                            purchaseOrder.Discount = Convert.ToDouble(txtDiscount.Text);
                        }
                        else
                        {
                            purchaseOrder.Discount = 0;
                        }
                        if (chkFreight.IsChecked == true)
                        {
                            purchaseOrder.Freight = Convert.ToDouble(txtFreight.Text);
                        }
                        else
                        {
                            purchaseOrder.Freight = 0;
                        }
                        if (purchaseOrder.SaleOrder != null)
                        {
                            if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                            {
                                dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                            }
                            if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                            {
                                dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                            }
                            if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                            {
                                dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                            }
                        }
                        else
                         if(purchaseOrder.saleOrder_Id!=null)
                        {
                            SaleOrderRepo repo = new SaleOrderRepo();
                           var saleOrder=repo.get((int)purchaseOrder.saleOrder_Id);


                            if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                            {
                                dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                            }
                            if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                            {
                                dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                            }
                            if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                            {
                                dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                            }


                        }
                        if (dateShipmentResult > 0 || dateRevisedShipmentResult > 0 || dateOrderConfirmationResult > 0)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the shipment dates at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                purchaseOrder.SaleOrder.shipmentDate = purchaseOrder.ShipmentDate;
                            }
                        }
                        if (dateRevisedShipmentResult > 0)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the revised shipment date at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                purchaseOrder.SaleOrder.revisedShipmentDate = purchaseOrder.RevisedShipmentDate;
                            }
                        }
                        if (dateOrderConfirmationResult > 0)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the OC revised shipment date at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                purchaseOrder.SaleOrder.orderConfirmationDate = purchaseOrder.OrderConfirmationDate;
                            }
                        }
                        if ((cmbPurchaseOrderStatusClass.SelectedItem as cmbitem) != null)
                        {

                            ERP_BL.Procurements.StatusClass.StatusClass statusClass = purchaseOrderRepo.GetStatusClass((cmbPurchaseOrderStatusClass.SelectedItem as cmbitem).id);
                            purchaseOrder.statusClass_Id = statusClass.Id;
                        }



                        if (checkStatus.Id != purchaseOrder.PurchaseOrderStatus.Id)
                        {
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Status of PO has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count > 0)
                                    {
                                        if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                        {
                                            purchaseOrder.holderChangeDate = DateTime.Now;
                                        }
                                        purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                    }
                                }
                                else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                                {

                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);                                   
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count > 0)
                                    {
                                        if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                        {
                                            purchaseOrder.holderChangeDate = DateTime.Now;
                                        }
                                        purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                    }
                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();
                                }
                            }
                            
                            string oldStat = checkStatus.Status;
                            string newStat = purchaseOrder.PurchaseOrderStatus.Status;
                            string symbolCurr = "";
                           
                            if (purchaseOrder.currency != null)
                            {
                                symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of PO (Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                
                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);

                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                               
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0,user.id, "New Comment ", null);
                                }
                            }

                        }
                        else
                          if (purchaseOrder.StatusClass != null)
                        {
                            if (checkStatusClass.Id != purchaseOrder.StatusClass.Id && purchaseOrder.StatusClass != null)
                            {
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Status class of Sale Invoice has been changed, Do you want to notify other users by tagging?)", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res == MessageBoxResult.Yes)
                                {
                                    if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                        // frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        if (win.tagUsers.Count > 0)
                                        {
                                            if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                            {
                                                purchaseOrder.holderChangeDate = DateTime.Now;
                                            }
                                            purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                        }
                                    }
                                    else
                                    {
                                        winTagUsers win = new winTagUsers();
                                        win.ShowDialog();

                                    }

                                }

                                string oldStat = purchaseOrder.StatusClass.ClassName;
                                string newStat = checkStatusClass.ClassName;
                                string symbolCurr = "";

                                if (purchaseOrder.currency != null)
                                {
                                    symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                                }
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status Class of Purchase Order(Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Class Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation


                                };
                                procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                    }

                                }

                                //SaleOrderss.ucStatuschange.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);

                                if (checkStatusClass != null && checkStatusClass.Id != 0)
                                {

                                    UsersRepo.Add(TransactionInfo.Status_Class_Changed, purchaseOrder.Id, 5, "Status Class Changed from (" + purchaseOrder.StatusClass.ClassName + ") to (" + checkStatusClass.ClassName + ")");
                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                UsersRepo.Add(TransactionInfo.Edited, purchaseOrder.Id, 5, frmInputBox.comment);



                            }
                        }
                        if (checkStatus != null && checkStatus.Id != 0)
                        {

                            //purchaseOrderRepo.Add(purchaseOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + purchaseOrder.PurchaseOrderStatus.Status + ")");
                            UsersRepo.Add(TransactionInfo.Status_Changed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "Status Changed from (" + checkStatus.Status + ") to (" + purchaseOrder.PurchaseOrderStatus.Status + ")");
                        }
                      




                        purchaseOrderRepo.update(purchaseOrder);

                        //frmInputBox inputBox = new frmInputBox();
                        //inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Edited, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "Status Changed");
                       
                        DXMessageBox.Show("PurchaseOrder Updated Succesfully");
                        SystemLog.LogInfo(this.GetType(), "PurchaseOrder Updated Succesfully refrence No= " + purchaseOrder.SOReferenceNo + " Id=" + purchaseOrder.Id);
                        //var myWindow = Window.GetWindow(this);
                        //myWindow.Close();
                        //return;
                    }
                    else if (editOrder != 1)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order") != null)
                        {
                            if (MainWindow.currentUserid == 0)
                            {
                                DXMessageBox.Show("Please Create another Account to Create PO, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                                return;
                            }
                            else
                                purchaseOrder.user_Id = MainWindow.currentUserid;
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
                                purchaseOrder.bid = bid;

                            }
                            else
                                purchaseOrder.bid_Id = null;
                            if (purchaseOrder.CostSheet != null)
                            {
                                purchaseOrder.CostSheet.Timestamp = System.DateTime.Now;
                                purchaseOrder.CostSheet.TransactionId = purchaseOrder.Id;
                                purchaseOrder.CostSheet.TransactionType = 2;

                            }
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null /*&& purchaseOrder.isApproved == false*/)
                            {
                                //if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Order is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    purchaseOrder.stage = TransactionStage.Approved.ToString();
                                    purchaseOrder.isApproved = true;
                                    purchaseOrder.ApprovedDate = System.DateTime.Now;
                                }
                            }
                            else
                            {
                                purchaseOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                purchaseOrder.isApproved = false;

                            }
                            if (purchaseOrder.SaleOrder != null)
                            {
                                if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                                {
                                    dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                                }
                                if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                                {
                                    dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                                }
                                if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                                {
                                    dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                                }
                            }
                            else
                         if (purchaseOrder.saleOrder_Id != null)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var saleOrder = repo.get((int)purchaseOrder.saleOrder_Id);


                                if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                                {
                                    dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                                }
                                if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                                {
                                    dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                                }
                                if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                                {
                                    dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                                }


                            }
                            if (dateShipmentResult > 0 || dateRevisedShipmentResult > 0 || dateOrderConfirmationResult > 0)
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the shipment dates at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    purchaseOrder.SaleOrder.shipmentDate = purchaseOrder.ShipmentDate;
                                }
                            }
                            if ( dateRevisedShipmentResult > 0)
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the revised shipment date at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    purchaseOrder.SaleOrder.revisedShipmentDate = purchaseOrder.RevisedShipmentDate;
                                }
                            }
                            if (dateOrderConfirmationResult > 0)
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the OC revised shipment date at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    purchaseOrder.SaleOrder.orderConfirmationDate = purchaseOrder.OrderConfirmationDate;
                                }
                            }


                            //purchaseOrder.creatorId = SYSTEM_STATIC.currentUser.id;
                            purchaseOrderRepo.Add(purchaseOrder);
                          
                            if (purchaseOrder.saleOrder_Id != null)
                            {

                                SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                                SaleOrderss.ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                                SaleOrderss.ucStatuschange.UpdateSaleOrder(/*SaleOrderss.ucStatuschange.saleOrder*/);

                          
                                UsersRepo.Add(TransactionInfo.Initialized, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "");
                                UsersRepo.Add(TransactionInfo.Closed, Offerss.ucStatuschange.offer.Id, 2, "Closed, Purchase Order genrated on this Offer");
                            }
                            SystemLog.LogInfo(this.GetType(), "PurchaseOrder Added Succesfully refrence No= " + purchaseOrder.SOReferenceNo + " Id=" + purchaseOrder.Id);

                            //grdPOItems.SaveLayoutToStream(memoryStream);

                            //Properties.Settings.Default["PoItemgridLO"] = streamConverter.converttostring(memoryStream);
                            //Properties.Settings.Default.Save();
                           
                            DXMessageBox.Show("PurchaseOrder Added Succesfully");
                            //myWindow.Close();
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                            //var myWindow = Window.GetWindow(this);
                            myWindow.Close();
                            return;
                        }
                    //DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreviewDialog(myWindow, new Reportss.reportPurchaseOrderSingle(purchaseOrder));

                    //Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(new Reportss.reportPurchaseOrderSingle(purchaseOrder));
                    //reportPanel.Show();

                    //ReportPrintToolWpf window = new ReportPrintToolWpf(new reportOfferSingle(offer));

                    //window.ShowPreviewDialog(myWindow);
                    myWindow.Close();
                }
                else
                 if (PoType == InquiryType.DistributionBiz)
                {
                    if (lookupVendor.SelectedIndex == -1 && vendor.Id == 0)
                    {
                        lookupVendor.Focus();
                        DXMessageBox.Show("Please Select a vendor against PO", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                        return;

                    }
                    else if (lookupCustomer.SelectedIndex == -1 && customer.Id == 0)
                    {
                        DXMessageBox.Show("Please Select a Customer for whom PO is being genrated", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupCustomer.Focus();
                        return;
                    }
                    else if (lookupCompany.SelectedIndex == -1 && company.Id == 0)
                    {
                        DXMessageBox.Show("Please select a Refrence Company", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupCompany.Focus();
                        return;
                    }
                    else if (lookupDepartment.SelectedIndex == -1 && department.Id == 0)
                    {
                        DXMessageBox.Show("Please Select a Refrence Department", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupDepartment.Focus();
                        return;
                    }
                    else if (cmbEmployee.SelectedIndex == -1 && employee.EmpId == 0)
                    {
                        DXMessageBox.Show("Please Select an Employee to whom this Purchase Order will be Allocated ", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbEmployee.Focus();
                        return;
                    }
                    else if (cmbPurchaseOrderStatusChange.SelectedIndex == -1 && purchaseOrder.PendingForClosing != true)
                    {
                        DXMessageBox.Show("Please Select Current Status of Purchase Order to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbPurchaseOrderStatusChange.Focus();
                        return;
                    }
                    else if (cmbPOCurrency.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select Purchase Orders Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbPOCurrency.Focus();
                        return;
                    }
                    else if (cmbSOCurrency.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select SaleOrder's Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbSOCurrency.Focus();
                        return;
                    }
                    //else if (Convert.ToDouble(txttotalcfr.Text) == 0 || 0 == Convert.ToDouble(txttotalfob.Text))
                    //{
                    //    DXMessageBox.Show("Please Enter Amount for the Items", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    //    grdPOItems.Focus();
                    //    return;
                    //}

                    else if (cmbPOWarrantyChange.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select PO Warranty", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbPOPaymentTermChange.Focus();
                        return;
                    }


                    else if (cmbIncotermChange.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select Incoterm", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbIncotermChange.Focus();
                        return;
                    }
                    else if (cmbcaption1.SelectedIndex == -1 || cmbcaption2.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please select a caption for Items Values", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbIncoterm.Focus();
                        return;
                    }
                    if (cmbTransactionHolder.SelectedIndex != -1)
                    {
                        purchaseOrder.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                        purchaseOrder.holderChangeDate = (DateTime)datHolderDate.EditValue;
                    }

                    if (!string.IsNullOrEmpty(txtPOSystemGeneratedReferenceNumberChange.Text))
                        purchaseOrder.SyetmReferenceNo = txtPOSystemGeneratedReferenceNumberChange.Text;
                    if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.DistributionBiz)
                    {

                        purchaseOrder.BookerStatementItems = getBookerStatementItems();
                        purchaseOrder.totalAmount = Convert.ToDouble(txttotalcfr.Text);
                        if(!string.IsNullOrEmpty(txtTaxAmount.Text))
                        purchaseOrder.totalAmountGST = Convert.ToDouble(txtTaxAmount.Text);
                        //purchaseOrder.totalPassOn = Convert.ToDouble(txtPassOn.Text);
                        //purchaseOrder.totalClaimDisount = Convert.ToDouble(txtDiscount.Text);
                        //purchaseOrder.totalFocSampling = Convert.ToDouble(txtFocSampling.Text);
                    }

                    if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.DistributionBiz)
                    {
                        if (grdBokkerItems.VisibleRowCount == 0)
                        {
                            DXMessageBox.Show("Please Select items against which you want to create a Purchase Order", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                            //lookupDepartment.Focus();
                            return;
                        }
                    }
                    else
                    {
                        if (purchaseOrder.products.Count == 0)
                        {
                            DXMessageBox.Show("Please Select items against which you want to create a Purchase Order", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                            //lookupDepartment.Focus();
                            return;
                        }
                    }

                    purchaseOrder.totalPOAdvance = Convert.ToDouble(txtPaidAdvance.Text);
                    purchaseOrder.expectedPaymentAmount = Convert.ToDouble(txtExpectedPaymentAmount.Text);

                    purchaseOrder.totalPOSattled = Convert.ToDouble(txtPOSattled.Text);

                    //SaleOrder-Details
                    {
                        if (costSheetFieldId != null)
                        {

                            purchaseOrder.CostSheetFieldId = (int)costSheetFieldId;
                        }
                        if (saleOrder.Id != 0 && saleOrder != null)
                        {
                            purchaseOrder.saleOrder_Id = saleOrder.Id;
                        }
                        else
                        {
                            purchaseOrder.saleOrder_Id = null;
                        }
                        purchaseOrder.SOReferenceNo = txtsaleOrderrefChange.Text.Trim();
                        if (cmbSOWarrantyChange.SelectedItem != null)
                            purchaseOrder.SOWarrantyId = (cmbSOWarrantyChange.SelectedItem as cmbitem).id;
                        if (cmbSOPaymentTermChange.SelectedItem != null)
                            purchaseOrder.SoPaymentterm_Id = (cmbSOPaymentTermChange.SelectedItem as cmbitem).id;

                    }

                    //Basic-Information
                    {
                        DateTime? dateTime = null;

                        purchaseOrder.PurchaseOrdertype = (InquiryType)cmbPurchaseOrderType.SelectedIndex;
                        purchaseOrder.CreationDate = (datpoCreationdate.Text == "") ? dateTime : datpoCreationdate.DateTime;
                        // selected company
                        if (company != null)
                        {
                            purchaseOrder.company_Id = company.Id;
                        }
                        // selected Department
                        if (department != null)
                        {

                            purchaseOrder.dept_Id = department.Id;
                        }
                        if (chkInterCompany.IsChecked == true)
                        {
                            if (InterCompany != null && InterCompany.Id != 0)
                            {

                                purchaseOrder.InterCompany_Id = InterCompany.Id;
                            }
                            if (InterDepartment != null && InterDepartment.Id != 0)
                            {

                                purchaseOrder.InterDepartment_Id = InterDepartment.Id;
                            }
                            purchaseOrder.isInterCompany = true;

                        }
                        else
                        {
                            purchaseOrder.isInterCompany = false;
                            purchaseOrder.InterCompany_Id = null;
                            purchaseOrder.InterDepartment_Id = null;
                        }
                        //selected customer
                        if (customer != null)
                        {

                            purchaseOrder.customerCompany_Id = customer.Id;
                        }
                        purchaseOrder.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;

                        // Selected Vendor 
                        if (vendor != null)
                        {
                            purchaseOrder.vendors = new List<Vendor>();
                            purchaseOrder.vendors.Add(purchaseOrderRepo.getVendor(vendor.Id));
                        }
                        if (!string.IsNullOrEmpty(txtSTLrefChange.Text))
                        {
                            purchaseOrder.stlRefNo = txtSTLrefChange.Text;
                        }
                    }
                    //PurchaseOrder-Details
                    {
                        DateTime? dateTime = null;
                        PurchaseOrder order; // = new PurchaseOrder();
                        order = purchaseOrderRepo.get(txtSalesrefChange.Text.Trim());
                        if (order != null && order.Id != purchaseOrder.Id)
                        {
                            string message = "Purchase Order with Sales reference # (" + order.SalesReferenceNo + ") already exists!";
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




                        purchaseOrder.FinanceRefrenceNo = txtFinanaceRefChange.Text;
                        if ((cmbPurchaseOrderStatusChange.SelectedItem as cmbitem) != null)
                        {
                            PurchaseOrderStatus status = purchaseOrderRepo.getstatus((cmbPurchaseOrderStatusChange.SelectedItem as cmbitem).id);
                            purchaseOrder.PurchaseOrderStatus = status;
                        }
                        purchaseOrder.SalesReferenceNo = txtSalesrefChange.Text.Trim();
                        purchaseOrder.VendorName = txtCommissionNumberChange.Text;
                        purchaseOrder.OfferReferenceNo = txtOfferRefNoChange.Text.Trim();
                        purchaseOrder.POReferenceNo = txtPOReferenceNumberChange.Text.ToString();
                        purchaseOrder.PurchaseOrderDate = (datPoDateChange.Text == "") ? dateTime : datPoDateChange.DateTime;
                        purchaseOrder.DeliveryDate = (datPODeliverydateChange.Text == "") ? dateTime : datPODeliverydateChange.DateTime;
                        purchaseOrder.proforma = txtProformaChange.Text;
                        if (cmbPOPaymentTermChange.SelectedItem != null)
                            purchaseOrder.POPaymentterm_Id = (cmbPOPaymentTermChange.SelectedItem as cmbitem).id;
                        if (cmbIncotermChange.SelectedItem != null)
                            purchaseOrder.incoterm_Id = (cmbIncotermChange.SelectedItem as cmbitem).id;
                        if (cmbcaption1.SelectedItem != null)
                            purchaseOrder.origin = txtOriginChange.Text.Trim();
                        purchaseOrder.maker = txtMakerChange.Text.Trim();
                        purchaseOrder.packing = txtPackingChange.Text;
                        if (cmbPOWarrantyChange.SelectedItem != null)
                            purchaseOrder.POWarrantyId = (cmbPOWarrantyChange.SelectedItem as cmbitem).id;
                        if (!string.IsNullOrEmpty(txtWeightChange.Text))
                        {
                            purchaseOrder.TotalWeight = Convert.ToDecimal(txtWeightChange.Text);
                        }
                        else
                        {
                            purchaseOrder.TotalWeight = null;
                        }
                        if (!string.IsNullOrEmpty(txtQuantityChange.Text))
                        {
                            purchaseOrder.TotalQuantity = Convert.ToDecimal(txtQuantityChange.Text);
                        }
                        else
                        {
                            purchaseOrder.TotalQuantity = null;
                        }
                    }
                    //Shipment-Details
                    if (!string.IsNullOrEmpty(txtTaxAmount.Text))
                    {
                        purchaseOrder.totaltaxAmount = Convert.ToDouble(txtTaxAmount.Text);
                    }
                    {
                        DateTime? dateTime = null;
                        purchaseOrder.ShipmentDate = (datShipmentdateChange.Text == "") ? dateTime : datShipmentdateChange.DateTime;
                        purchaseOrder.OrderConfirmationDate = (datOrderConfirmationdateChange.Text == "") ? dateTime : datOrderConfirmationdateChange.DateTime;
                        purchaseOrder.RevisedShipmentDate = (datRevisedShipmentDateChange.Text == "") ? dateTime : datRevisedShipmentDateChange.DateTime;
                        purchaseOrder.OwnDescription = txtOwnDescriptionChange.Text;
                        //Selected PurchaseOrder Status
                        if ((cmbVendorPaymentStatusChange.SelectedItem as cmbitem) != null)
                        {
                            purchaseOrder.vendorPaymentId = (cmbVendorPaymentStatusChange.SelectedItem as cmbitem).id;
                        }
                        purchaseOrder.MaterialReciptDate = (datMaterialReciptdateChange.Text == "") ? dateTime : datMaterialReciptdateChange.DateTime;
                        purchaseOrder.BillOfLaddingDate = (datBillOfLaddingdateChange.Text == "") ? dateTime : datBillOfLaddingdateChange.DateTime;
                        purchaseOrder.PaymentDueStartDate = (datPaymentDueFromChange.Text == "") ? dateTime : datPaymentDueFromChange.DateTime;
                        purchaseOrder.CreditDays = Convert.ToInt32(txtPaymentDueDaysChange.Text.Trim());
                        purchaseOrder.PaymentDueAgeing = (datPaymentDueTillChange.Text == "") ? dateTime : datPaymentDueTillChange.DateTime;
                        purchaseOrder.ExpectedPayment = (datExcpectedPaymentDateChange.Text == "") ? dateTime : datExcpectedPaymentDateChange.DateTime;
                    }
                    //LC-Information
                    {
                        DateTime? dateTime = null;
                        purchaseOrder.LCnumber = txtLCNumber.Text.Trim();
                        purchaseOrder.lCDate = (datLCDatedate.Text == "") ? dateTime : datLCDatedate.DateTime;
                        purchaseOrder.LCShipmentDate = (datLCShipmentDate.Text == "") ? dateTime : datLCShipmentDate.DateTime;
                        purchaseOrder.LCExpiryDate = (datLCExpiryDate.Text == "") ? dateTime : datLCExpiryDate.DateTime;
                        if (cmbTransshipment.SelectedIndex == 0)
                            purchaseOrder.transshipment = true;
                        if (cmbTransshipment.SelectedIndex == 1)
                            purchaseOrder.transshipment = false;
                        purchaseOrder.LCAmedmentNo = txtLCAmedmentno.Text;
                        purchaseOrder.LCShipmentAmendmentDate = (datLCRevisedShipmentDate.Text == "") ? dateTime : datLCRevisedShipmentDate.DateTime;
                        purchaseOrder.LCExpiryAmedmentDate = (datLCRevisedExpiryDate.Text == "") ? dateTime : datLCRevisedExpiryDate.DateTime;

                    }
                    //Captions
                    {
                        purchaseOrder.TitleValue1Id = (cmbcaption1.SelectedItem as cmbitem).id;
                        if (cmbcaption2.SelectedItem != null)
                            purchaseOrder.TitleValue2Id = (cmbcaption2.SelectedItem as cmbitem).id;
                    }

                    //Currency-Details
                    {
                        if (cmbPOCurrency.SelectedIndex != -1)
                        {
                            purchaseOrder.currency_Id = (cmbPOCurrency.SelectedItem as cmbitem).id;
                        }
                        //// selected currency
                        //if (currency != null)
                        //{

                        //    purchaseOrder.currency_Id = currency.Id;
                        //}
                        purchaseOrder.SOCurrency_Id = (cmbSOCurrency.SelectedItem as cmbitem).id;
                        purchaseOrder.SOC_ER = (string.IsNullOrEmpty(txtSOCER.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCER.Text.Trim());
                        //purchaseOrder.PERValue = (string.IsNullOrEmpty(txtSOCERAmount.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCERAmount.Text.Trim());
                        purchaseOrder.SoAmountSOC_ER = (string.IsNullOrEmpty(txtSoAmountSOC.Text.Trim())) ? 0 : Convert.ToDouble(txtSoAmountSOC.Text.Trim());
                    }
                    //Commision-and-Margins
                    {
                        //purchaseOrder.SOC_ER = (string.IsNullOrEmpty(txtSOCER.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCER.Text.Trim());
                        if (txtCommision.Text != "" && txtBaseCommission.Text != "")
                        {
                            purchaseOrder.Commision = Convert.ToDouble(txtCommision.Text.Trim());
                            purchaseOrder.commisioninBase = Convert.ToDouble(txtBaseCommission.Text.Trim());
                        }
                        purchaseOrder.NetCommision = (string.IsNullOrEmpty(txtNetCommision.Text.Trim())) ? 0 : Convert.ToDouble(txtNetCommision.Text.Trim());
                        if (txtBudgetMargin.Text != "" /*&& txtBudgetMargin.Text != "0" && txtBudgetMargin.Text != "0.00"*/)
                        {
                            purchaseOrder.margin = Convert.ToDouble(txtBudgetMargin.Text.Trim());
                            purchaseOrder.BudgetedMargininBase = Convert.ToDouble(txtBaseBudgetMargin.Text.Trim());
                            purchaseOrder.SalesBudgetedMargin = Convert.ToDouble(txtSaleBudgetMargin.Text.Trim());
                        }
                        purchaseOrder.BudgetedMarginPercent = (string.IsNullOrEmpty(txtBudgetedPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtBudgetedPercent.Text, "[^0-9.]", ""));

                        if (txtRevisedMargin.Text != "")
                        {
                            purchaseOrder.RevisedMargin = (string.IsNullOrEmpty(txtRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtRevisedMargin.Text.Trim());
                            purchaseOrder.RevisedMargininBase = (string.IsNullOrEmpty(txtBaseRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtBaseRevisedMargin.Text.Trim());
                            purchaseOrder.SalesRevisedMargin = (string.IsNullOrEmpty(txtSaleRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtSaleRevisedMargin.Text.Trim());

                        }
                        purchaseOrder.RevisedMarginPercent = (string.IsNullOrEmpty(txtRevisedPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtRevisedPercent.Text, "[^0-9.]", ""));


                        if (txtActualMargin.Text != "" /*&& txtActualMargin.Text != "0" && txtActualMargin.Text != "0.00"*/)
                        {
                            purchaseOrder.ActualMargin = Convert.ToDouble(txtActualMargin.Text.Trim());
                            purchaseOrder.ActualMargininBase = Convert.ToDouble(txtBaseActualMargin.Text.Trim());
                            purchaseOrder.SalesActualMargin = Convert.ToDouble(txtSaleAMargin.Text.Trim());

                        }
                        purchaseOrder.ActualMarginPercent = (string.IsNullOrEmpty(txtActualPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtActualPercent.Text, "[^0-9.]", ""));

                    }

                    //Amounts-DragAndDropController-Tax
                    {
                        purchaseOrder.totalCFRValue = Convert.ToDouble(txttotalcfr.Text);


                        purchaseOrder.totalBaseCFRValue = Convert.ToDouble(txtBasetotalcfr.Text);
                        purchaseOrder.totalBaseFOBValue = Convert.ToDouble(txtBasetotalfob.Text);

                        purchaseOrder.totalFOBValue = Convert.ToDouble(txttotalfob.Text);

                        purchaseOrder.POAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);

                        purchaseOrder.RemainingCFRValue = Convert.ToDouble(txtPOremainingcfr.Text);

                        purchaseOrder.POAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);




                        purchaseOrder.ExchangeRate = (float)Convert.ToDecimal(txtexchangerate.Text.Trim());

                        purchaseOrder.marginExchangeRate = Convert.ToDouble(txtMarginexchangerate.Text.Trim());
                        string str = txttax.Text.Trim();

                        if (str.IndexOf("%") != -1)
                        {
                            purchaseOrder.isPercentTax = true;
                            purchaseOrder.salesTax = Convert.ToDouble(str.Substring(0, str.IndexOf("%")));
                        }
                        else
                        {
                            purchaseOrder.salesTax = Convert.ToDouble(str);
                            purchaseOrder.isPercentTax = false;
                        }


                    }
                    purchaseOrder.comments = txtComments.Text.Trim();

                    var myWindow = Window.GetWindow(this);


                    if (chkTax.IsChecked == true)
                    {
                        purchaseOrder.hasTax = true;

                        if (lookUpTax.SelectedIndex != -1)
                            purchaseOrder.tax_Id = (lookUpTax.SelectedItem as TaxName).Id;
                        purchaseOrder.billWithTax = Convert.ToDouble(txtBillWithTax.Text);
                    }
                    else
                    {
                        purchaseOrder.hasTax = false;
                        purchaseOrder.tax_Id = null;
                        purchaseOrder.billWithTax = Convert.ToDouble(txtcfr.Text);
                    }

                    if (chkWHT.IsChecked == true)
                    {
                        purchaseOrder.hasWHT = true;
                        if (lookUpWHT.SelectedIndex < 0)
                        {
                            DXMessageBox.Show("Please Select WHT!");
                            lookUpWHT.Focus();
                            return;
                        }
                        purchaseOrder.WHT_Id = (lookUpWHT.SelectedItem as TaxName).Id;
                        purchaseOrder.billAfterTax = Convert.ToDouble(txtBillAfterTax.Text);
                    }
                    else
                    {
                        purchaseOrder.hasWHT = false;
                        purchaseOrder.WHT_Id = null;
                        purchaseOrder.billAfterTax = Convert.ToDouble(txtBillWithTax.Text);
                    }
                    if (chkAdjustedTax.IsChecked == true)
                    {
                        purchaseOrder.isAdjustedTax = true;
                        if (lookUpTax.SelectedIndex != -1)
                            purchaseOrder.tax_Id = (lookUpTax.SelectedItem as TaxName).Id;
                        purchaseOrder.billWithTax = Convert.ToDouble(txtBillWithTax.Text);
                    }
                    else
                    {
                        purchaseOrder.isAdjustedTax = false;
                    }
                    if (editOrder == 1 && OrderId != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Purchase Order") != null))
                    {
                        if (txtBidBondRefno.Text != "" && txtBidBonValue.Text != "" && txtIssuingbank.Text != "")
                        {
                            if (purchaseOrder.bid_Id == 0 && purchaseOrder.bid == null)
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
                                purchaseOrder.bid = bid;
                            }
                            else
                            { //get form data for bid
                                purchaseOrder.bid.issueDate = datBidBondIssuedate.DateTime;
                                purchaseOrder.bid.refNo = txtBidBondRefno.Text.Trim();
                                purchaseOrder.bid.value = txtBidBonValue.Text.Trim();
                                purchaseOrder.bid.submitDate = datBidBondSubmitdate.DateTime;
                                purchaseOrder.bid.expireDate = datBidBondExpirydate.DateTime;
                                purchaseOrder.bid.bankName = txtIssuingbank.Text.Trim();
                            }
                        }
                        if (MainWindow.currentUserid == 0)
                        {

                        }
                        else if (purchaseOrder.user_Id == null)
                            purchaseOrder.user_Id = MainWindow.currentUserid;

                        if (purchaseOrder.CostSheet != null && purchaseOrder.CostSheet.Timestamp != null)
                        {
                            purchaseOrder.CostSheet.Timestamp = System.DateTime.Now;
                        }


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null && purchaseOrder.isApproved != true)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Order is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {

                                purchaseOrder.stage = TransactionStage.Approved.ToString();

                                purchaseOrder.isApproved = true;
                                purchaseOrder.ApprovedDate = System.DateTime.Now;
                            }
                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without ReApproval") != null && purchaseOrder.isReApproved != true)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Order is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {

                                purchaseOrder.stage = TransactionStage.Approved.ToString();

                                purchaseOrder.isReApproved = true;
                                purchaseOrder.ReApprovalDate = System.DateTime.Now;
                            }
                        }
                        if (checkStatus != null && checkStatus.Id != 0)
                        {
                            if (checkStatus.Id != purchaseOrder.PurchaseOrderStatus.Id)
                            {
                                purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                                if (purchaseOrder.PurchaseOrderStatus.isActive != true)
                                {
                                    purchaseOrder.ClosingDate = System.DateTime.Now;
                                }
                            }
                        }
                        if (chkCOO.IsChecked == true)
                        {
                            purchaseOrder.COO = Convert.ToDouble(txtCOO.Text);
                        }
                        else
                        {
                            purchaseOrder.COO = 0;
                        }
                        if (chkDiscount.IsChecked == true)
                        {
                            purchaseOrder.Discount = Convert.ToDouble(txtDiscount.Text);
                        }
                        else
                        {
                            purchaseOrder.Discount = 0;
                        }
                        if (chkFreight.IsChecked == true)
                        {
                            purchaseOrder.Freight = Convert.ToDouble(txtFreight.Text);
                        }
                        else
                        {
                            purchaseOrder.Freight = 0;
                        }
                        if (purchaseOrder.SaleOrder != null)
                        {
                            if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                            {
                                dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                            }
                            if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                            {
                                dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                            }
                            if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                            {
                                dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                            }
                        }
                        else
                         if (purchaseOrder.saleOrder_Id != null)
                        {
                            SaleOrderRepo repo = new SaleOrderRepo();
                            var saleOrder = repo.get((int)purchaseOrder.saleOrder_Id);


                            if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                            {
                                dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                            }
                            if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                            {
                                dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                            }
                            if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                            {
                                dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                            }


                        }
                        if (dateShipmentResult > 0 || dateRevisedShipmentResult > 0 || dateOrderConfirmationResult > 0)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the shipment dates at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                purchaseOrder.SaleOrder.shipmentDate = purchaseOrder.ShipmentDate;
                            }
                        }
                        if (dateRevisedShipmentResult > 0)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the revised shipment date at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                purchaseOrder.SaleOrder.revisedShipmentDate = purchaseOrder.RevisedShipmentDate;
                            }
                        }
                        if (dateOrderConfirmationResult > 0)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the OC revised shipment date at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                purchaseOrder.SaleOrder.orderConfirmationDate = purchaseOrder.OrderConfirmationDate;
                            }
                        }



                        if (checkStatus.Id != purchaseOrder.PurchaseOrderStatus.Id)
                        {
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Status of PO has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count > 0)
                                    {
                                        if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                        {
                                            purchaseOrder.holderChangeDate = DateTime.Now;
                                        }
                                        purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                    }
                                }
                                else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                                {

                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count > 0)
                                    {
                                        if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                        {
                                            purchaseOrder.holderChangeDate = DateTime.Now;
                                        }
                                        purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                    }
                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();
                                }
                            }

                            string oldStat = checkStatus.Status;
                            string newStat = purchaseOrder.PurchaseOrderStatus.Status;
                            string symbolCurr = "";

                            if (purchaseOrder.currency != null)
                            {
                                symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of PO (Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,

                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);

                            //Creating notification
                            if (tagUsers.Count != 0)
                            {

                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }
                            if (checkStatus != null && checkStatus.Id != 0)
                            {

                                //purchaseOrderRepo.Add(purchaseOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + purchaseOrder.PurchaseOrderStatus.Status + ")");
                                UsersRepo.Add(TransactionInfo.Status_Changed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "Status Changed from (" + checkStatus.Status + ") to (" + purchaseOrder.PurchaseOrderStatus.Status + ")");
                            }
                            //frmInputBox inputBox = new frmInputBox();
                            //inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Edited, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "Status Changed");
                        }
                        else
                        if (purchaseOrder.StatusClass != null)
                        {
                            if (checkStatusClass.Id != purchaseOrder.StatusClass.Id && purchaseOrder.StatusClass != null)
                            {
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Status class of Sale Invoice has been changed, Do you want to notify other users by tagging?)", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res == MessageBoxResult.Yes)
                                {
                                    if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                        // frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        if (win.tagUsers.Count > 0)
                                        {
                                            if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                            {
                                                purchaseOrder.holderChangeDate = DateTime.Now;
                                            }
                                            purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                        }
                                    }
                                    else
                                    {
                                        winTagUsers win = new winTagUsers();
                                        win.ShowDialog();

                                    }

                                }

                                string oldStat = purchaseOrder.StatusClass.ClassName;
                                string newStat = checkStatusClass.ClassName;
                                string symbolCurr = "";

                                if (purchaseOrder.currency != null)
                                {
                                    symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                                }
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status Class of Purchase Order(Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Class Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation


                                };
                                procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                    }

                                }

                                //SaleOrderss.ucStatuschange.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);

                                if (checkStatusClass != null && checkStatusClass.Id != 0)
                                {

                                    UsersRepo.Add(TransactionInfo.Status_Class_Changed, purchaseOrder.Id, 5, "Status Class Changed from (" + purchaseOrder.StatusClass.ClassName   + ") to (" + checkStatusClass.ClassName + ")");
                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                UsersRepo.Add(TransactionInfo.Edited, purchaseOrder.Id, 5, frmInputBox.comment);



                            }
                        }


                        purchaseOrderRepo.update(purchaseOrder);
                        DXMessageBox.Show("PurchaseOrder Updated Succesfully");
                        SystemLog.LogInfo(this.GetType(), "PurchaseOrder Updated Succesfully refrence No= " + purchaseOrder.SOReferenceNo + " Id=" + purchaseOrder.Id);
                        //var myWindow = Window.GetWindow(this);
                        //myWindow.Close();
                        //return;
                    }
                    else if (editOrder != 1)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order") != null)
                        {
                            if (MainWindow.currentUserid == 0)
                            {
                                DXMessageBox.Show("Please Create another Account to Create PO, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                                return;
                            }
                            else
                                purchaseOrder.user_Id = MainWindow.currentUserid;
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
                                purchaseOrder.bid = bid;

                            }
                            else
                                purchaseOrder.bid_Id = null;
                            if (purchaseOrder.CostSheet != null)
                            {
                                purchaseOrder.CostSheet.Timestamp = System.DateTime.Now;
                                purchaseOrder.CostSheet.TransactionId = purchaseOrder.Id;
                                purchaseOrder.CostSheet.TransactionType = 2;

                            }
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null /*&& purchaseOrder.isApproved == false*/)
                            {
                                //if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Order is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    purchaseOrder.stage = TransactionStage.Approved.ToString();
                                    purchaseOrder.isApproved = true;
                                    purchaseOrder.ApprovedDate = System.DateTime.Now;
                                }
                            }
                            else
                            {
                                purchaseOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                purchaseOrder.isApproved = false;

                            }
                            if (purchaseOrder.SaleOrder != null)
                            {
                                if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                                {
                                    dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                                }
                                if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                                {
                                    dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                                }
                                if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                                {
                                    dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                                }
                            }
                            else
                         if (purchaseOrder.saleOrder_Id != null)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var saleOrder = repo.get((int)purchaseOrder.saleOrder_Id);


                                if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                                {
                                    dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                                }
                                if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                                {
                                    dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                                }
                                if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                                {
                                    dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                                }


                            }
                            if (dateShipmentResult > 0 || dateRevisedShipmentResult > 0 || dateOrderConfirmationResult > 0)
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the shipment dates at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    purchaseOrder.SaleOrder.shipmentDate = purchaseOrder.ShipmentDate;
                                }
                            }
                            if (dateRevisedShipmentResult > 0)
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the revised shipment date at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    purchaseOrder.SaleOrder.revisedShipmentDate = purchaseOrder.RevisedShipmentDate;
                                }
                            }
                            if (dateOrderConfirmationResult > 0)
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the OC revised shipment date at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    purchaseOrder.SaleOrder.orderConfirmationDate = purchaseOrder.OrderConfirmationDate;
                                }
                            }



                            purchaseOrderRepo.Add(purchaseOrder);

                            if (purchaseOrder.saleOrder_Id != null)
                            {

                                SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                                SaleOrderss.ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                                SaleOrderss.ucStatuschange.UpdateSaleOrder(/*SaleOrderss.ucStatuschange.saleOrder*/);


                                UsersRepo.Add(TransactionInfo.Initialized, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "");
                                UsersRepo.Add(TransactionInfo.Closed, Offerss.ucStatuschange.offer.Id, 2, "Closed, Purchase Order genrated on this Offer");
                            }
                            SystemLog.LogInfo(this.GetType(), "PurchaseOrder Added Succesfully refrence No= " + purchaseOrder.SOReferenceNo + " Id=" + purchaseOrder.Id);

                            //grdPOItems.SaveLayoutToStream(memoryStream);

                            //Properties.Settings.Default["PoItemgridLO"] = streamConverter.converttostring(memoryStream);
                            //Properties.Settings.Default.Save();

                            DXMessageBox.Show("PurchaseOrder Added Succesfully");
                            //myWindow.Close();
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                            //var myWindow = Window.GetWindow(this);
                            myWindow.Close();
                            return;
                        }
                    //DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreviewDialog(myWindow, new Reportss.reportPurchaseOrderSingle(purchaseOrder));

                    //Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(new Reportss.reportPurchaseOrderSingle(purchaseOrder));
                    //reportPanel.Show();

                    //ReportPrintToolWpf window = new ReportPrintToolWpf(new reportOfferSingle(offer));

                    //window.ShowPreviewDialog(myWindow);
                    myWindow.Close();
                }
                else
                {

                    //if (MainWindow.currentUserid == 0)
                    //{
                    //    DXMessageBox.Show("Please Create another Account to Create PO, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    //    lookupDepartment.Focus();
                    //    return;
                    //}
                    //else
                    if (lookupVendor.SelectedIndex == -1 && vendor.Id == 0)
                    {
                        lookupVendor.Focus();
                        DXMessageBox.Show("Please Select a vendor against PO", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                        return;

                    }
                    else if (lookupCustomer.SelectedIndex == -1 && customer.Id == 0)
                    {
                        DXMessageBox.Show("Please Select a Customer for whom PO is being genrated", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupCustomer.Focus();
                        return;
                    }
                    else if (lookupCompany.SelectedIndex == -1 && company.Id == 0)
                    {
                        DXMessageBox.Show("Please select a Refrence Company", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupCompany.Focus();
                        return;
                    }
                    else if (lookupDepartment.SelectedIndex == -1 && department.Id == 0)
                    {
                        DXMessageBox.Show("Please Select a Refrence Department", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        lookupDepartment.Focus();
                        return;
                    }
                    else if (cmbEmployee.SelectedIndex == -1 && employee.EmpId == 0)
                    {
                        DXMessageBox.Show("Please Select an Employee to whom this Purchase Order will be Allocated ", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbEmployee.Focus();
                        return;
                    }
                    else if (cmbPurchaseOrderStatus.SelectedIndex == -1 && purchaseOrder.PendingForClosing != true)
                    {
                        DXMessageBox.Show("Please Select Current Status of Purchase Order to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbPurchaseOrderStatus.Focus();
                        return;
                    }
                    else if (cmbPOCurrency.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select Purchase Orders Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbPOCurrency.Focus();
                        return;
                    }
                    else if (cmbSOCurrency.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select SaleOrder's Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbSOCurrency.Focus();
                        return;
                    }
                    //else if (Convert.ToDouble(txttotalcfr.Text) == 0 || 0 == Convert.ToDouble(txttotalfob.Text))
                    //{
                    //    DXMessageBox.Show("Please Enter Amount for the Items", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    //    grdPOItems.Focus();
                    //    return;
                    //}
                    //else if (cmbSOWarranty.SelectedIndex == -1)
                    //{
                    //    DXMessageBox.Show("Please Select SO Warranty", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    //    cmbPOPaymentTerm.Focus();
                    //    return;
                    //}
                    else if (cmbPOWarranty.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select PO Warranty", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbPOPaymentTerm.Focus();
                        return;
                    }
                    //else if (cmbSOPaymentTerm.SelectedIndex == -1)
                    //{
                    //    DXMessageBox.Show("Please Select SO PaymentTerm", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    //    cmbSOPaymentTerm.Focus();
                    //    return;
                    //}
                    else if (cmbIncoterm.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please Select Incoterm", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbIncoterm.Focus();
                        return;
                    }
                    else if (cmbcaption1.SelectedIndex == -1 || cmbcaption2.SelectedIndex == -1)
                    {
                        DXMessageBox.Show("Please select a caption for Items Values", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        cmbIncoterm.Focus();
                        return;
                    }
                    if (!string.IsNullOrEmpty(txtPOSystemGeneratedReferenceNumberChange.Text))
                        purchaseOrder.SyetmReferenceNo = txtPOSystemGeneratedReferenceNumberChange.Text;
                    if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.DistributionBiz)
                    {

                        purchaseOrder.BookerStatementItems = getBookerStatementItems();
                        purchaseOrder.totalAmount = Convert.ToDouble(txttotalcfr.Text);
                        purchaseOrder.totalAmountGST = Convert.ToDouble(txtTaxAmount.Text);
                        purchaseOrder.totalPassOn = Convert.ToDouble(txtPassOn.Text);
                        purchaseOrder.totalClaimDisount = Convert.ToDouble(txtDiscount.Text);
                        purchaseOrder.totalFocSampling = Convert.ToDouble(txtFocSampling.Text);
                    }
                    if (cmbTransactionHolder.SelectedIndex != -1)
                    {
                        purchaseOrder.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                        purchaseOrder.holderChangeDate = (DateTime)datHolderDate.EditValue;
                    }
                    if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.DistributionBiz)
                    {
                        if (grdBokkerItems.VisibleRowCount == 0)
                        {
                            DXMessageBox.Show("Please Select items against which you want to create a Purchase Order", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                            //lookupDepartment.Focus();
                            return;
                        }
                    }
                    else
                    {
                        if (purchaseOrder.products.Count == 0)
                        {
                            DXMessageBox.Show("Please Select items against which you want to create a Purchase Order", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                            //lookupDepartment.Focus();
                            return;
                        }
                    }
                    PurchaseOrder order; // = new PurchaseOrder();
                    order = purchaseOrderRepo.get(txtSalesref.Text.Trim());
                    //if (order == null)
                    //    order = new PurchaseOrder();

                    if (order != null && order.Id != purchaseOrder.Id)
                    {
                        string message = "Purchase Order with Sales reference # (" + order.SalesReferenceNo + ") already exists!";
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
                    //PurchaseOrder inquiry = new Inquiry();
                    DateTime? dateTime = null;
                    ////inquiry.alertDate = datalertDate.DateTime;
                    //purchaseOrder.alertDate = datduedate.DateTime;
                    purchaseOrder.PurchaseOrderDate = (datsaleOrderdate.Text == "") ? dateTime : datsaleOrderdate.DateTime;
                    purchaseOrder.CreationDate = (datpoCreationdate.Text == "") ? dateTime : datpoCreationdate.DateTime;
                    //purchaseOrder.incoterm_Id = txtIncoTerm.Text.Trim();
                    if (cmbSOPaymentTerm.SelectedItem != null)
                        purchaseOrder.SoPaymentterm_Id = (cmbSOPaymentTerm.SelectedItem as cmbitem).id;
                    if (cmbPOPaymentTerm.SelectedItem != null)
                        purchaseOrder.POPaymentterm_Id = (cmbPOPaymentTerm.SelectedItem as cmbitem).id;
                    if (cmbIncoterm.SelectedItem != null)
                        purchaseOrder.incoterm_Id = (cmbIncoterm.SelectedItem as cmbitem).id;

                    if (cmbcaption1.SelectedItem != null)
                        purchaseOrder.TitleValue1Id = (cmbcaption1.SelectedItem as cmbitem).id;
                    if (cmbcaption2.SelectedItem != null)
                        purchaseOrder.TitleValue2Id = (cmbcaption2.SelectedItem as cmbitem).id;
                    purchaseOrder.maker = txtMaker.Text.Trim();
                    if (cmbSOWarranty.SelectedItem != null)
                        purchaseOrder.SOWarrantyId = (cmbSOWarranty.SelectedItem as cmbitem).id;
                    if (cmbPOWarranty.SelectedItem != null)
                        purchaseOrder.POWarrantyId = (cmbPOWarranty.SelectedItem as cmbitem).id;

                    purchaseOrder.origin = txtOrigin.Text.Trim();
                    purchaseOrder.ShipmentDate = (datShipmentdate.Text == "") ? dateTime : datShipmentdate.DateTime;
                    //if (txtSalesTargetYear.Text != "")
                    //    purchaseOrder.TargetYear = Convert.ToInt32(txtSalesTargetYear.Text.Trim());
                    //if (txtSalesTargetMonth.Text != "")
                    //    purchaseOrder.TargetMonth = Convert.ToInt32(txtSalesTargetMonth.Text.Trim());
                    purchaseOrder.POReferenceNo = lblPOrefrence.Text.ToString();

                    purchaseOrder.DeliveryDate = (datDeliverydate.Text == "") ? dateTime : datDeliverydate.DateTime;
                    purchaseOrder.OrderConfirmationDate = (datOrderConfirmationdate.Text == "") ? dateTime : datOrderConfirmationdate.DateTime;
                    purchaseOrder.RevisedShipmentDate = (datRevisedShipmentDate.Text == "") ? dateTime : datRevisedShipmentDate.DateTime;
                    purchaseOrder.BillOfLaddingDate = (datBillOfLaddingdate.Text == "") ? dateTime : datBillOfLaddingdate.DateTime;
                    purchaseOrder.MaterialReciptDate = (datMaterialReciptdate.Text == "") ? dateTime : datMaterialReciptdate.DateTime;
                    purchaseOrder.ExpectedPayment = (datExcpectedPaymentDate.Text == "") ? dateTime : datExcpectedPaymentDate.DateTime;
                    purchaseOrder.OwnDescription = txtOwnDescription.Text;
                    purchaseOrder.FinanceRefrenceNo = txtFinanaceRef.Text;
                    purchaseOrder.VendorName = txtCommissionNumber.Text;
                    purchaseOrder.comments = txtComments.Text.Trim();
                    purchaseOrder.lCDate = (datLCDatedate.Text == "") ? dateTime : datLCDatedate.DateTime;
                    purchaseOrder.LCnumber = txtLCNumber.Text.Trim();
                    purchaseOrder.SalesReferenceNo = txtSalesref.Text.Trim();
                    purchaseOrder.SOReferenceNo = txtsaleOrderref.Text.Trim();
                    purchaseOrder.OfferReferenceNo = txtOfferRefNo.Text.Trim();
                    purchaseOrder.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
                    purchaseOrder.PurchaseOrdertype = (InquiryType)cmbPurchaseOrderType.SelectedIndex;
                    purchaseOrder.PaymentDueStartDate = (datPaymentDueFrom.Text == "") ? dateTime : datPaymentDueFrom.DateTime;
                    purchaseOrder.PaymentDueAgeing = (datPaymentDueTill.Text == "") ? dateTime : datPaymentDueTill.DateTime;
                    purchaseOrder.CreditDays = Convert.ToInt32(txtPaymentDueDays.Text.Trim());
                    purchaseOrder.packing = txtPacking.Text;
                    if (purchaseOrder.PurchaseOrdertype == InquiryType.Principal)
                    {
                        purchaseOrder.LCShipmentDate = (datLCShipmentDate.Text == "") ? dateTime : datLCShipmentDate.DateTime;
                        purchaseOrder.LCShipmentAmendmentDate = (datLCRevisedShipmentDate.Text == "") ? dateTime : datLCRevisedShipmentDate.DateTime;
                        purchaseOrder.LCExpiryDate = (datLCExpiryDate.Text == "") ? dateTime : datLCExpiryDate.DateTime;
                        purchaseOrder.LCExpiryAmedmentDate = (datLCRevisedExpiryDate.Text == "") ? dateTime : datLCRevisedExpiryDate.DateTime;
                        purchaseOrder.LCAmedmentNo = txtLCAmedmentno.Text;
                        if (cmbTransshipment.SelectedIndex == 0)
                            purchaseOrder.transshipment = true;
                        if (cmbTransshipment.SelectedIndex == 1)
                            purchaseOrder.transshipment = false;
                    }
                    purchaseOrder.SOC_ER = (string.IsNullOrEmpty(txtSOCER.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCER.Text.Trim());
                    //purchaseOrder.PERValue = (string.IsNullOrEmpty(txtSOCERAmount.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCERAmount.Text.Trim());
                    purchaseOrder.SoAmountSOC_ER = (string.IsNullOrEmpty(txtSoAmountSOC.Text.Trim())) ? 0 : Convert.ToDouble(txtSoAmountSOC.Text.Trim());

                    //purchaseOrder.SOC_ER = (string.IsNullOrEmpty(txtSOCER.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCER.Text.Trim());


                    if (txtCommision.Text != "" && txtBaseCommission.Text != "")
                    {
                        purchaseOrder.Commision = Convert.ToDouble(txtCommision.Text.Trim());
                        purchaseOrder.commisioninBase = Convert.ToDouble(txtBaseCommission.Text.Trim());
                    }
                    purchaseOrder.NetCommision = (string.IsNullOrEmpty(txtNetCommision.Text.Trim())) ? 0 : Convert.ToDouble(txtNetCommision.Text.Trim());

                    if (txtBudgetMargin.Text != "" /*&& txtBudgetMargin.Text != "0" && txtBudgetMargin.Text != "0.00"*/)
                    {
                        purchaseOrder.margin = Convert.ToDouble(txtBudgetMargin.Text.Trim());
                        purchaseOrder.BudgetedMargininBase = Convert.ToDouble(txtBaseBudgetMargin.Text.Trim());
                        purchaseOrder.SalesBudgetedMargin = Convert.ToDouble(txtSaleBudgetMargin.Text.Trim());
                    }
                    if (txtActualMargin.Text != "" /*&& txtActualMargin.Text != "0" && txtActualMargin.Text != "0.00"*/)
                    {
                        purchaseOrder.ActualMargin = Convert.ToDouble(txtActualMargin.Text.Trim());
                        purchaseOrder.ActualMargininBase = Convert.ToDouble(txtBaseActualMargin.Text.Trim());
                        purchaseOrder.SalesActualMargin = Convert.ToDouble(txtSaleAMargin.Text.Trim());

                    }
                    if (txtRevisedMargin.Text != "")
                    {
                        purchaseOrder.RevisedMargin = (string.IsNullOrEmpty(txtRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtRevisedMargin.Text.Trim());
                        purchaseOrder.RevisedMargininBase = (string.IsNullOrEmpty(txtBaseRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtBaseRevisedMargin.Text.Trim());
                        purchaseOrder.SalesRevisedMargin = (string.IsNullOrEmpty(txtSaleRevisedMargin.Text.Trim())) ? 0 : Convert.ToDouble(txtSaleRevisedMargin.Text.Trim());

                    }

                    purchaseOrder.RevisedMarginPercent = (string.IsNullOrEmpty(txtRevisedPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtRevisedPercent.Text, "[^0-9.]", ""));

                    purchaseOrder.ActualMarginPercent = (string.IsNullOrEmpty(txtActualPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtActualPercent.Text, "[^0-9.]", ""));
                    purchaseOrder.BudgetedMarginPercent = (string.IsNullOrEmpty(txtBudgetedPercent.Text)) ? 0 : Convert.ToDouble(Regex.Replace(txtBudgetedPercent.Text, "[^0-9.]", ""));
                    purchaseOrder.totalCFRValue = Convert.ToDouble(txttotalcfr.Text);
                    purchaseOrder.RemainingCFRValue = Convert.ToDouble(txtPOremainingcfr.Text);

                    purchaseOrder.POAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);

                    purchaseOrder.totalFOBValue = Convert.ToDouble(txttotalfob.Text);
                    purchaseOrder.totalBaseCFRValue = Convert.ToDouble(txtBasetotalcfr.Text);
                    purchaseOrder.totalBaseFOBValue = Convert.ToDouble(txtBasetotalfob.Text);
                    purchaseOrder.POAmountSER = Convert.ToDouble(txtSalestotalCfr.Text);
                    purchaseOrder.ExchangeRate = (float)Convert.ToDecimal(txtexchangerate.Text.Trim());
                    purchaseOrder.marginExchangeRate = Convert.ToDouble(txtMarginexchangerate.Text.Trim());

                    purchaseOrder.totalPOAdvance = Convert.ToDouble(txtPaidAdvance.Text);
                    purchaseOrder.expectedPaymentAmount = Convert.ToDouble(txtExpectedPaymentAmount.Text);

                    purchaseOrder.totalPOSattled = Convert.ToDouble(txtPOSattled.Text);
                    string str = txttax.Text.Trim();
                    if (!string.IsNullOrEmpty(txtWeight.Text))
                    {
                        purchaseOrder.TotalWeight = Convert.ToDecimal(txtWeight.Text);
                    }
                    else
                    {
                        purchaseOrder.TotalWeight = null;
                    }
                    if (!string.IsNullOrEmpty(txtQuantity.Text))
                    {
                        purchaseOrder.TotalQuantity = Convert.ToDecimal(txtQuantity.Text);
                    }
                    else
                    {
                        purchaseOrder.TotalQuantity = null;
                    }
                    if (str.IndexOf("%") != -1)
                    {
                        purchaseOrder.isPercentTax = true;
                        purchaseOrder.salesTax = Convert.ToDouble(str.Substring(0, str.IndexOf("%")));
                    }
                    else
                    {
                        purchaseOrder.salesTax = Convert.ToDouble(str);
                        purchaseOrder.isPercentTax = false;
                    }
                   

                    if (saleOrder.Id != 0 && saleOrder != null)
                    {
                        purchaseOrder.saleOrder_Id = saleOrder.Id;
                    }
                    else
                    {
                        purchaseOrder.saleOrder_Id = null;
                    }
                    if ((cmbPurchaseOrderStatus.SelectedItem as cmbitem) != null)
                    {

                        PurchaseOrderStatus status = purchaseOrderRepo.getstatus((cmbPurchaseOrderStatus.SelectedItem as cmbitem).id);
                        purchaseOrder.PurchaseOrderStatus = status;
                    }
                    //Selected PurchaseOrder Status
                    if ((cmbVendorPaymentStatus.SelectedItem as cmbitem) != null)
                    {

                        purchaseOrder.vendorPaymentId = (cmbVendorPaymentStatus.SelectedItem as cmbitem).id;

                    }
                    
                    // Selected Vendor 
                    if (vendor != null)
                    {

                        purchaseOrder.vendors = new List<Vendor>();
                        purchaseOrder.vendors.Add(purchaseOrderRepo.getVendor(vendor.Id));
                    }
                    // selected Department
                    if (department != null)
                    {

                        purchaseOrder.dept_Id = department.Id;
                    }
                    //selected customer
                    if (customer != null)
                    {

                        purchaseOrder.customerCompany_Id = customer.Id;
                    }
                    // selected company
                    if (company != null)
                    {

                        purchaseOrder.company_Id = company.Id;
                    }
                    if (chkInterCompany.IsChecked == true)
                    {
                        if (InterCompany != null && InterCompany.Id != 0)
                        {

                            purchaseOrder.InterCompany_Id = InterCompany.Id;
                        }
                        if (InterDepartment != null && InterDepartment.Id != 0)
                        {

                            purchaseOrder.InterDepartment_Id = InterDepartment.Id;
                        }
                        purchaseOrder.isInterCompany = true;

                    }
                    else
                    {
                        purchaseOrder.isInterCompany = false;
                        purchaseOrder.InterCompany_Id = null;
                        purchaseOrder.InterDepartment_Id = null;
                    }
                    // selected currency
                    if (currency != null)
                    {

                        purchaseOrder.currency_Id = currency.Id;
                    }
                    purchaseOrder.SOCurrency_Id = (cmbSOCurrency.SelectedItem as cmbitem).id;

                    var myWindow = Window.GetWindow(this);

                    if (editOrder == 1 && OrderId != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Purchase Order") != null))
                    {
                        if (txtBidBondRefno.Text != "" && txtBidBonValue.Text != "" && txtIssuingbank.Text != "")
                        {
                            if (purchaseOrder.bid_Id == 0 && purchaseOrder.bid == null)
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
                                purchaseOrder.bid = bid;
                            }
                            else
                            { //get form data for bid
                                purchaseOrder.bid.issueDate = datBidBondIssuedate.DateTime;
                                purchaseOrder.bid.refNo = txtBidBondRefno.Text.Trim();
                                purchaseOrder.bid.value = txtBidBonValue.Text.Trim();
                                purchaseOrder.bid.submitDate = datBidBondSubmitdate.DateTime;
                                purchaseOrder.bid.expireDate = datBidBondExpirydate.DateTime;
                                purchaseOrder.bid.bankName = txtIssuingbank.Text.Trim();
                            }
                        }
                        if (MainWindow.currentUserid == 0)
                        {

                        }
                        else if (purchaseOrder.user_Id == null)
                            purchaseOrder.user_Id = MainWindow.currentUserid;

                        if (purchaseOrder.CostSheet != null && purchaseOrder.CostSheet.Timestamp != null)
                        {
                            purchaseOrder.CostSheet.Timestamp = System.DateTime.Now;
                        }


                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null && purchaseOrder.isApproved != true)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Order is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {

                                
                                purchaseOrder.stage = TransactionStage.Approved.ToString();

                                purchaseOrder.isApproved = true;
                                purchaseOrder.ApprovedDate = System.DateTime.Now;
                            }
                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without ReApproval") != null && purchaseOrder.isReApproved != true)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Order is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {

                                purchaseOrder.stage = TransactionStage.Approved.ToString();

                                purchaseOrder.isReApproved = true;
                                purchaseOrder.ReApprovalDate = System.DateTime.Now;
                            }
                        }
                        if (checkStatus != null && checkStatus.Id != 0)
                        {
                            if (checkStatus.Id != purchaseOrder.PurchaseOrderStatus.Id)
                            {
                                purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                                if (purchaseOrder.PurchaseOrderStatus.isActive != true)
                                {
                                    purchaseOrder.ClosingDate = System.DateTime.Now;
                                }
                            }
                        }
                        if (purchaseOrder.SaleOrder != null)
                        {
                            if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                            {
                                dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                            }
                            if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                            {
                                dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                            }
                            if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                            {
                                dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                            }
                        }
                        else
                         if (purchaseOrder.saleOrder_Id != null)
                        {
                            SaleOrderRepo repo = new SaleOrderRepo();
                            var saleOrder = repo.get((int)purchaseOrder.saleOrder_Id);


                            if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                            {
                                dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                            }
                            if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                            {
                                dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                            }
                            if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                            {
                                dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                            }


                        }
                        if (dateShipmentResult > 0 || dateRevisedShipmentResult > 0 || dateOrderConfirmationResult > 0)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the shipment dates at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                purchaseOrder.SaleOrder.shipmentDate = purchaseOrder.ShipmentDate;
                                purchaseOrder.SaleOrder.revisedShipmentDate = purchaseOrder.RevisedShipmentDate;
                                purchaseOrder.SaleOrder.orderConfirmationDate = purchaseOrder.OrderConfirmationDate;
                            }
                        }
                        if (checkStatus.Id != purchaseOrder.PurchaseOrderStatus.Id)
                        {
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Status of PO has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                {
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count > 0)
                                    {
                                        if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                        {
                                            purchaseOrder.holderChangeDate = DateTime.Now;
                                        }
                                        purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                    }
                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();
                                }
                            }
                            string oldStat = checkStatus.Status;
                            string newStat = purchaseOrder.PurchaseOrderStatus.Status;
                            string symbolCurr = "";

                            if (purchaseOrder.currency != null)
                            {
                                symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {

                                Comment = "Status of PO (Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                               
                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0,user.id, "New Comment ", null);
                                }
                            }
                            if (checkStatus != null && checkStatus.Id != 0)
                            {
                                //purchaseOrderRepo.Add(purchaseOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + purchaseOrder.PurchaseOrderStatus.Status + ")");
                                UsersRepo.Add(TransactionInfo.Status_Changed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "Status Changed from (" + checkStatus.Status + ") to (" + purchaseOrder.PurchaseOrderStatus.Status + ")");
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Edited, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                        }


                        if (purchaseOrder.StatusClass != null)
                        {
                            if (checkStatusClass.Id != purchaseOrder.StatusClass.Id && purchaseOrder.StatusClass != null)
                            {
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Status class of Sale Invoice has been changed, Do you want to notify other users by tagging?)", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                if (res == MessageBoxResult.Yes)
                                {
                                    if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                    {
                                        winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                        // frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        if (win.tagUsers.Count > 0)
                                        {
                                            if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                            {
                                                purchaseOrder.holderChangeDate = DateTime.Now;
                                            }
                                            purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;

                                        }
                                    }
                                    else
                                    {
                                        winTagUsers win = new winTagUsers();
                                        win.ShowDialog();

                                    }

                                }
                                string oldStat = purchaseOrder.StatusClass.ClassName;
                                string newStat = checkStatusClass.ClassName;
                                string symbolCurr = "";

                                if (purchaseOrder.currency != null)
                                {
                                    symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                                }
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status Class of Purchase Order(Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Class Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation


                                };
                                procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                //Creating notification
                                if (tagUsers.Count != 0)
                                {
                                    foreach (var user in tagUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                                    }
                                }

                                if (ccUsers.Count != 0)
                                {
                                    foreach (var user in ccUsers)
                                    {
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0, user.id, "New Comment ", null);
                                    }

                                }

                                //SaleOrderss.ucStatuschange.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);

                                if (checkStatusClass != null && checkStatusClass.Id != 0)
                                {

                                    UsersRepo.Add(TransactionInfo.Status_Class_Changed, purchaseOrder.Id, 5, "Status Class Changed from (" + purchaseOrder.StatusClass.ClassName + ") to (" + checkStatusClass.ClassName + ")");
                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                UsersRepo.Add(TransactionInfo.Edited, purchaseOrder.Id, 5, frmInputBox.comment);



                            }
                        }
                        purchaseOrderRepo.update(purchaseOrder);
                        DXMessageBox.Show("PurchaseOrder Updated Succesfully");
                        SystemLog.LogInfo(this.GetType(), "PurchaseOrder Updated Succesfully refrence No= " + purchaseOrder.SOReferenceNo + " Id=" + purchaseOrder.Id);
                        //var myWindow = Window.GetWindow(this);
                        //myWindow.Close();
                        //return;
                    }
                    else if (editOrder != 1)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order") != null)
                        {
                            if (MainWindow.currentUserid == 0)
                            {
                                DXMessageBox.Show("Please Create another Account to Create PO, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                                return;
                            }
                            else
                                purchaseOrder.user_Id = MainWindow.currentUserid;
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
                                purchaseOrder.bid = bid;

                            }
                            else
                                purchaseOrder.bid_Id = null;
                            if (purchaseOrder.CostSheet != null)
                            {
                                purchaseOrder.CostSheet.Timestamp = System.DateTime.Now;
                                purchaseOrder.CostSheet.TransactionId = purchaseOrder.Id;
                                purchaseOrder.CostSheet.TransactionType = 2;

                            }
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null /*&& purchaseOrder.isApproved == false*/)
                            {
                                //if (DevExpress.Xpf.Core.DXMessageBox.Show("This Purchase Order is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {

                                    purchaseOrder.stage = TransactionStage.Approved.ToString();

                                    purchaseOrder.isApproved = true;
                                    purchaseOrder.ApprovedDate = System.DateTime.Now;
                                }
                            }
                            else
                            {
                                purchaseOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                purchaseOrder.isApproved = false;
                            }
                            if (purchaseOrder.SaleOrder != null)
                            {
                                if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                                {
                                    dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                                }
                                if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                                {
                                    dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                                }
                                if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                                {
                                    dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                                }
                            }
                            else
                         if (purchaseOrder.saleOrder_Id != null)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var saleOrder = repo.get((int)purchaseOrder.saleOrder_Id);


                                if (datShipmentdateChange.EditValue != null && saleOrder.shipmentDate != null)
                                {
                                    dateShipmentResult = DateTime.Compare(purchaseOrder.ShipmentDate.Value, saleOrder.shipmentDate.Value);
                                }
                                if (datRevisedShipmentDateChange.EditValue != null && saleOrder.revisedShipmentDate != null)
                                {
                                    dateRevisedShipmentResult = DateTime.Compare(purchaseOrder.RevisedShipmentDate.Value, saleOrder.revisedShipmentDate.Value);
                                }
                                if (datOrderConfirmationdateChange.EditValue != null && saleOrder.orderConfirmationDate != null)
                                {
                                    dateOrderConfirmationResult = DateTime.Compare(purchaseOrder.OrderConfirmationDate.Value, saleOrder.orderConfirmationDate.Value);
                                }


                            }
                            if (dateShipmentResult > 0 || dateRevisedShipmentResult > 0 || dateOrderConfirmationResult > 0)
                            {
                                if (DevExpress.Xpf.Core.DXMessageBox.Show("Do you want to change the shipment dates at SO?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                {
                                    purchaseOrder.SaleOrder.shipmentDate = purchaseOrder.ShipmentDate;
                                    purchaseOrder.SaleOrder.revisedShipmentDate = purchaseOrder.RevisedShipmentDate;
                                    purchaseOrder.SaleOrder.orderConfirmationDate = purchaseOrder.OrderConfirmationDate;
                                }
                            }
                            
                            purchaseOrderRepo.Add(purchaseOrder);
                            SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                            SaleOrderss.ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                            
                             SaleOrderss.ucStatuschange.UpdateSaleOrder(/*SaleOrderss.ucStatuschange.saleOrder*/); 

                            UsersRepo.Add(TransactionInfo.Initialized, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "");
                            UsersRepo.Add(TransactionInfo.Closed, Offerss.ucStatuschange.offer.Id, 2, "Closed, Purchase Order genrated on this Offer");

                            SystemLog.LogInfo(this.GetType(), "PurchaseOrder Added Succesfully refrence No= " + purchaseOrder.SOReferenceNo + " Id=" + purchaseOrder.Id);

                            //grdPOItems.SaveLayoutToStream(memoryStream);

                            //Properties.Settings.Default["PoItemgridLO"] = streamConverter.converttostring(memoryStream);
                            //Properties.Settings.Default.Save();
                            DXMessageBox.Show("PurchaseOrder Added Succesfully");
                            //myWindow.Close();
                        }
                        else
                        {
                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                            //var myWindow = Window.GetWindow(this);
                            myWindow.Close();
                            return;
                        }
                    //DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreviewDialog(myWindow, new Reportss.reportPurchaseOrderSingle(purchaseOrder));

                    //Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(new Reportss.reportPurchaseOrderSingle(purchaseOrder));
                    //reportPanel.Show();

                    //ReportPrintToolWpf window = new ReportPrintToolWpf(new reportOfferSingle(offer));

                    //window.ShowPreviewDialog(myWindow);
                    myWindow.Close();
                }
                
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString(), "Data Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SystemLog.LogError(this.GetType(), "PurchaseOrder Error refrence No= " + purchaseOrder.SOReferenceNo + " Id=" + purchaseOrder.Id + ex.ToString());
               

            }

        }


        private void LookupLotNumbers_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void loadCurrencies()
        {
            cmbPOCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbSOCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbbaseCurrency.ItemsSource= SYSTEM_STATIC.currencySources;
        }
        public void loadVendorPaymentStatus()
        {
             cmbVendorPaymentStatus.ItemsSource = SYSTEM_STATIC.vendorPaymentStatusSource;
            cmbVendorPaymentStatusChange.ItemsSource = SYSTEM_STATIC.vendorPaymentStatusSource;

        }
        public void loadPurchaseOrderStatus()
        {
            //List<cmbitem> cmbitems = new List<cmbitem>();
            //var Items = SYSTEM_STATIC.statusSources.FirstOrDefault(x => x.name == "Purchase Orders").Items.ToList();
            //cmbitems.AddRange(Items.FirstOrDefault(x => x.name == "Purchase Orders(Open)").Items.Distinct().ToList());
            //if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Orders") != null))
            //    cmbitems.AddRange(Items.FirstOrDefault(x => x.name == "Purchase Orders(Closed)").Items.Distinct().ToList());
            //cmbPurchaseOrderStatus.ItemsSource = cmbitems;
            //cmbPurchaseOrderStatusChange.ItemsSource = cmbitems;

            if (/*SystemLogic.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Order Statuses") != null ||*/ SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed PurchaseOrder") != null)
            {
                PurchaseOrderStatuses = purchaseOrderRepo.getAllPurchaseOrderStatus();
            }
            else

                PurchaseOrderStatuses = purchaseOrderRepo.getAllActivePurchaseOrderStatus();
            PurchaseOrderStatuses = PurchaseOrderStatuses.Where(x => x.isDisable != true).ToList();


            List<cmbitem> cmbitems = new List<cmbitem>();
            List<cmbitem> cmbitemsChange = new List<cmbitem>();

           /* Parallel.ForEach(PurchaseOrderStatuses, delegate (PurchaseOrderStatus status)*/  foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
            {
                //string color = status.forecolor;
                //status.forecolor=(color!=null||color!="")?status.forecolor:"#FF000000";
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
                cmbitemsChange.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });


            }/*)*/;
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbPurchaseOrderStatus.ItemsSource = cmbitems;
            cmbPurchaseOrderStatusChange.ItemsSource = cmbitemsChange;

        }
        public void loadPurchaseOrderStatus(PurchaseOrderStatus _status)
        {

            List<cmbitem> cmbitems = new List<cmbitem>();
            List<cmbitem> cmbitemsChange = new List<cmbitem>();

            PurchaseOrderStatuses.Add(_status);
            foreach (PurchaseOrderStatus status in PurchaseOrderStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
                cmbitemsChange.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            };
            cmbPurchaseOrderStatus.ItemsSource = cmbitems;
            cmbPurchaseOrderStatusChange.ItemsSource = cmbitemsChange;

        }

        private void cmbPurchaseOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbPurchaseOrderStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbPurchaseOrderStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Procurementss.PurchaseOrderss.frmPurchaseOrderStatusAdd statusAdd = new Procurementss.PurchaseOrderss.frmPurchaseOrderStatusAdd();
                    statusAdd.ShowDialog();
                    loadPurchaseOrderStatus();
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

            if (customerParent == 1)
            {
                customerParent = 0;
                
                if (customer != null)
                {
                    var customerCountry = customer.billingAddres.Country;
                    txtCustomerCountry.Text = customerCountry;
                    //string selectedcust = customer.company.CompanyName + " (" + customer.contactPerson.FName + ")";
                    //lookupCustomer.EditValue = selectedcust;
                }
            }
            else
            {
                if(customer != null)
                {
                    if(customerParentName != lookupCustomer.Text)
                    {
                        //var customerlist = (lookupCustomer.ItemsSource as List<CustomerCompany>) == null ? new List<CustomerCompany>() : lookupCustomer.ItemsSource as List<CustomerCompany>;
                        var customerCountry = customer.billingAddres.Country;
                        txtCustomerCountry.Text = customerCountry;
                        var child = purchaseOrderRepo.getParent(customer.Id);
                        if (child != null)
                        {
                            lookupCustomer.SelectedItem = null;
                            txtCustomerCountry.Text = "";
                            DXMessageBox.Show("Selected Customer is parent please select child to add selected customer register");
                            return;
                        }
                    } 
                   
                }
              
            }
           
        }

        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company != null)
            {
                //string selecteddept = company.CompanyName;

                foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
                {
                    if (cmbitem.id == company.CurrencyId)
                    {
                        cmbbaseCurrency.SelectedItem = cmbitem;
                        break;
                    }
                }

                loaddepartments();
            }

        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                //if (department.ParentID == null && department.subDepartments.Count != 0)
                //{
                //    DXMessageBox.Show("Cannot map to a parent Department directly. Please select a diffrent department!");
                //    lookupDepartment.Focus();
                //    return;
                //}
                if(purchaseOrder.dept_Id != department.Id)
                {
                    //var child = purchaseOrderRepo.getParentDepart(department.Id);
                    //var child = customerlist.Find(x => x.ParentID == customer.Id);
                    if (department.IsParent == true)
                    {
                        lookupDepartment.SelectedItem = null;
                        DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                        lookupDepartment.Focus();
                        return;
                    }
                }
                //string selecteddept = department.DeptName + " (" + department.Code + ")";
                //lookupDepartment.EditValue = selecteddept;


                loadcustomers();
                ProductRepo productRepo = new ProductRepo();
                var products = productRepo.getAllDepartmentProducts(department.Id);
                lookupProductsinGrid.ItemsSource = products;
                lookupVendor.ItemsSource = department.Vendors;
                loademployees();
                if (department.customers.Count == 0)
                {
                    DXMessageBox.Show("No customer is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.Vendors.Count == 0)
                {
                    DXMessageBox.Show("No Vendor is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                if (department.employees.Count == 0)
                {
                    DXMessageBox.Show("This department do not have Employees. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }

                if (company != null && department != null)
                {
                    TaskRepo taskRepo = new TaskRepo();
                    var lotNumbers = taskRepo.GetAllLotNumberForCompDept(company.Id, department.Id);
                    lookupLotNumbers.ItemsSource = lotNumbers;
                }
                //if (department.Principals.Count == 0)
                //{
                //    DXMessageBox.Show("No Principal is mapped to this department. Please select a diffrent department!");
                //    lookupDepartment.Focus();
                //    return;
                //}
            }
            //department = lookupDepartment.SelectedItem as Department;
            //if (department != null)
            //{
            //    string selecteddept = department.DeptName + " (" + department.Code + ")";
            //    lookupDepartment.EditValue = selecteddept;


            //}
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
                if (datpoCreationdate.EditValue != null && (DateTime)datpoCreationdate.EditValue > new DateTime(2025, 2, 17) && vendor.isBlackList == true)
                {
                    lookupVendor.SelectedItem = null;
                    return; // or whatever action you need to take
                }
                else
                if (vendor.isBlackList == true)
                {

                    lookupVendor.Background = Brushes.DarkRed;
                    lookupVendor.Foreground = Brushes.White;
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

            // cmbCurrency.DisplayMemberPath = (cmbCurrency.SelectedItem as cmbitem).name;
            if (cmbPOCurrency.SelectedItem as cmbitem != null)
            {
                int idd = (cmbPOCurrency.SelectedItem as cmbitem).id;
                currency = currencyRepo.get(idd);
                if (idd == 0)
                {
                    BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                    frmCurrency.ShowDialog();
                    loadCurrencies();
                }
                else
                {
                    string name = (cmbPOCurrency.SelectedItem as cmbitem).name;
                    string caption1 = "";
                    string caption2 = "";
                    if (cmbcaption1.SelectedItem != null)
                        caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                    if (cmbcaption2.SelectedItem != null)
                        caption2 = (cmbcaption2.SelectedItem as cmbitem).name;

                    symbol = name.Substring(name.IndexOf("("));
                    if (grdPOItems.Columns.Count != 0)
                    {
                        grdPOItems.Columns[6].Header = caption1 + symbol;
                        grdPOItems.Columns[7].Header = caption2 + symbol;
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
            
            if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.Supply)
            {
                grdDistributionItemGroup.Visibility = Visibility.Collapsed;
                grdGridControl.Visibility = Visibility.Visible;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                        grppurchaseOrderinfo.Visibility = Visibility.Visible;
                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Visible;

                        //lblCommision.Text = "Margin Amount " + symbol;
                        grpbondinfo.IsEnabled = false;
                        grpbondinfo.State = GroupBoxState.Minimized;
                        //lblBidOpendate.Text = "Quotation Opening Date";
                        //lblMargin.Visibility = Visibility.Visible;
                        grpCommisiondetails.Visibility = Visibility.Collapsed;
                        grpMargindetails.Visibility = Visibility.Visible;
                        //txtMargin.Visibility = Visibility.Visible;
                        PoType = InquiryType.Supply;
                        //txtCommision.Visibility = Visibility.Collapsed;
                        txtCommision.Text = "0";
                        txtBaseCommission.Text = "0";
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbPurchaseOrderType.SelectedItem = PoType;
                    grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                    grppurchaseOrderinfo.Visibility = Visibility.Visible;
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
                GridColumn column1 = grdPOItems.Columns.FirstOrDefault(x => x.Name == "value1");
                column1.ReadOnly = false;
                GridColumn column2 = grdPOItems.Columns.FirstOrDefault(x => x.Name == "value2");
                column2.ReadOnly = false;
                grdPOItems.Columns.GetColumnByFieldName("value2").Visible = true;


            }
            else if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.Tender)
            {
                grdDistributionItemGroup.Visibility = Visibility.Collapsed;
                grdGridControl.Visibility = Visibility.Visible;
                if ((txtBudgetMargin.Text != "" || txtActualMargin.Text != "" || txtBaseBudgetMargin.Text != "" || txtBaseActualMargin.Text != "" || txtSaleBudgetMargin.Text != "" || txtSaleAMargin.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                        grppurchaseOrderinfo.Visibility = Visibility.Visible;
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
                        cmbPurchaseOrderType.SelectedItem = PoType;
                    grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                    grppurchaseOrderinfo.Visibility = Visibility.Visible;
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
                GridColumn column1 = grdPOItems.Columns.FirstOrDefault(x => x.Name == "value1");
                column1.ReadOnly = false;
                GridColumn column2 = grdPOItems.Columns.FirstOrDefault(x => x.Name == "value2");
                column2.ReadOnly = false;
                grdPOItems.Columns.GetColumnByFieldName("value2").Visible = true;


            }

            else if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.Principal)
            {
                grdDistributionItemGroup.Visibility = Visibility.Collapsed;
                grdGridControl.Visibility = Visibility.Visible;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                        grppurchaseOrderinfo.Visibility = Visibility.Visible;
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
                        cmbPurchaseOrderType.SelectedItem = PoType;
                    grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                    grppurchaseOrderinfo.Visibility = Visibility.Visible;

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
                GridColumn column1 = grdPOItems.Columns.FirstOrDefault(x => x.Name == "value1");
                column1.ReadOnly = false;
                GridColumn column2 = grdPOItems.Columns.FirstOrDefault(x => x.Name == "value2");
                column2.ReadOnly = false;
                grdPOItems.Columns.GetColumnByFieldName("value2").Visible = true;

            }
            else if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.Standard)
            {
                grdDistributionItemGroup.Visibility = Visibility.Collapsed;
                grdGridControl.Visibility = Visibility.Visible;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes /*&& SystemLogic.AllowedPermissions.Find(x => x.Name == "Select Standard PO") != null*/)
                    {
                        grppurchaseOrderinfo.Visibility = Visibility.Collapsed;
                        grppurchaseOrderChangeinfo.Visibility = Visibility.Visible;
                        grpShipmentInfo.Visibility = Visibility.Collapsed;

                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Visible;

                        //lblCommision.Text = "Margin Amount " + symbol;
                        grpbondinfo.IsEnabled = false;
                        grpbondinfo.State = GroupBoxState.Minimized;
                        //lblBidOpendate.Text = "Quotation Opening Date";
                        //lblMargin.Visibility = Visibility.Visible;
                        grpCommisiondetails.Visibility = Visibility.Collapsed;
                        grpMargindetails.Visibility = Visibility.Visible;

                        //txtMargin.Visibility = Visibility.Visible;
                        PoType = InquiryType.Standard;
                        //txtCommision.Visibility = Visibility.Collapsed;
                        txtCommision.Text = "0";
                        txtBaseCommission.Text = "0";
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbPurchaseOrderType.SelectedItem = PoType;
                    //grppurchaseOrderinfo.Visibility = Visibility.Collapsed;
                    //grpShipmentInfo.Visibility = Visibility.Collapsed;
                    //grppurchaseOrderChangeinfo.Visibility = Visibility.Visible;
                }
                else
                {
                    grpbondinfo.IsEnabled = false;
                    grpbondinfo.State = GroupBoxState.Minimized;
                    grpCommisiondetails.Visibility = Visibility.Collapsed;
                    grpMargindetails.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Visible;
                    PoType = InquiryType.Standard;
                    txtCommision.Text = "0";
                    txtBaseCommission.Text = "0";
                    txtIssuingbank.Text = "";
                    txtBidBondRefno.Text = "";
                    txtBidBonValue.Text = "";
                }

                grdPOItems.Columns.GetColumnByFieldName("value2").Visible = true;

            }
            else if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.Bill)
            {
                grdDistributionItemGroup.Visibility = Visibility.Collapsed;
                grdGridControl.Visibility = Visibility.Visible;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                        grppurchaseOrderinfo.Visibility = Visibility.Visible;
                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Visible;

                        //lblCommision.Text = "Margin Amount " + symbol;
                        grpbondinfo.IsEnabled = false;
                        grpbondinfo.State = GroupBoxState.Minimized;
                        //lblBidOpendate.Text = "Quotation Opening Date";
                        //lblMargin.Visibility = Visibility.Visible;
                        grpCommisiondetails.Visibility = Visibility.Collapsed;
                        grpMargindetails.Visibility = Visibility.Visible;
                        //txtMargin.Visibility = Visibility.Visible;
                        PoType = InquiryType.Bill;
                        //txtCommision.Visibility = Visibility.Collapsed;
                        txtCommision.Text = "0";
                        txtBaseCommission.Text = "0";
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbPurchaseOrderType.SelectedItem = PoType;
                    grppurchaseOrderChangeinfo.Visibility = Visibility.Collapsed;
                    grppurchaseOrderinfo.Visibility = Visibility.Visible;
                }
                else
                {
                    grpbondinfo.IsEnabled = false;
                    grpbondinfo.State = GroupBoxState.Minimized;
                    grpCommisiondetails.Visibility = Visibility.Collapsed;
                    grpMargindetails.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Visible;
                    PoType = InquiryType.Bill;
                    txtCommision.Text = "0";
                    txtBaseCommission.Text = "0";
                    txtIssuingbank.Text = "";
                    txtBidBondRefno.Text = "";
                    txtBidBonValue.Text = "";
                }
                grdPOItems.Columns.GetColumnByFieldName("value2").Visible = true;


            }

            else if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.Inventory)
            {
                grdBokkerItems.Visibility = Visibility.Collapsed;
                grdGridControl.Visibility = Visibility.Visible;

                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes /*&& SystemLogic.AllowedPermissions.Find(x => x.Name == "Select Standard PO") != null*/)
                    {
                        grppurchaseOrderinfo.Visibility = Visibility.Collapsed;
                        grppurchaseOrderChangeinfo.Visibility = Visibility.Visible;
                        grpShipmentInfo.Visibility = Visibility.Collapsed;

                        btnSummarySheet.Visibility = Visibility.Collapsed;
                        btnCostSheet.Visibility = Visibility.Visible;

                        //lblCommision.Text = "Margin Amount " + symbol;
                        grpbondinfo.IsEnabled = false;
                        grpbondinfo.State = GroupBoxState.Minimized;
                        //lblBidOpendate.Text = "Quotation Opening Date";
                        //lblMargin.Visibility = Visibility.Visible;
                        grpCommisiondetails.Visibility = Visibility.Collapsed;
                        grpMargindetails.Visibility = Visibility.Visible;

                        //txtMargin.Visibility = Visibility.Visible;
                        PoType = InquiryType.Standard;
                        //txtCommision.Visibility = Visibility.Collapsed;
                        txtCommision.Text = "0";
                        txtBaseCommission.Text = "0";
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbPurchaseOrderType.SelectedItem = PoType;
                    //grppurchaseOrderinfo.Visibility = Visibility.Collapsed;
                    //grpShipmentInfo.Visibility = Visibility.Collapsed;
                    //grppurchaseOrderChangeinfo.Visibility = Visibility.Visible;
                }
                else
                {
                    grpbondinfo.IsEnabled = false;
                    grpbondinfo.State = GroupBoxState.Minimized;
                    grpCommisiondetails.Visibility = Visibility.Collapsed;
                    grpMargindetails.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Visible;
                    PoType = InquiryType.Standard;
                    txtCommision.Text = "0";
                    txtBaseCommission.Text = "0";
                    txtIssuingbank.Text = "";
                    txtBidBondRefno.Text = "";
                    txtBidBonValue.Text = "";
                }

                //GridColumn column1= grdPOItems.Columns.FirstOrDefault(x => x.Name == "value1");
                //column1.ReadOnly = true;
                //GridColumn column2 = grdPOItems.Columns.FirstOrDefault(x => x.Name == "value2");
                //column2.ReadOnly = true;
                grdPOItems.Columns.GetColumnByFieldName("value2").Visible = false;
            }
            else if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.DistributionBiz)
            {
                PoType = InquiryType.DistributionBiz;
                grdDistributionItemGroup.Visibility = Visibility.Visible;
                grdGridControl.Visibility = Visibility.Collapsed;

                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes /*&& SystemLogic.AllowedPermissions.Find(x => x.Name == "Select Standard PO") != null*/)
                    {
                        grppurchaseOrderinfo.Visibility = Visibility.Collapsed;
                        grppurchaseOrderChangeinfo.Visibility = Visibility.Visible;
                        grpShipmentInfo.Visibility = Visibility.Collapsed;

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
                        cmbPurchaseOrderType.SelectedItem = PoType;
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

                //GridColumn column1= grdPOItems.Columns.FirstOrDefault(x => x.Name == "value1");
                //column1.ReadOnly = true;
                //GridColumn column2 = grdPOItems.Columns.FirstOrDefault(x => x.Name == "value2");
                //column2.ReadOnly = true;
                grdPOItems.Columns.GetColumnByFieldName("value2").Visible = false;
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

            if((InquiryType)cmbPurchaseOrderType.SelectedIndex==InquiryType.DistributionBiz)
            {
                double sumfob = 0;
                double sumcfr = 0;
                double weight = 0;
                double Quantity = 0;
                double taxAmount = 0;
                double discountAmount = 0;
                double passOnAmount = 0;
                double focSamplingAmount = 0;
                double netAmount = 0;

                if (grdPOItems.ItemsSource != null)
                    foreach (var item in grdBokkerItems.ItemsSource as List<BookerStatementItem>)
                    {
                           
                        weight += weight;
                        Quantity += item.quantity;
                        sumfob += item.amount;
                        sumcfr += item.amount;
                        taxAmount += item.amountGST;
                        discountAmount += item.claimDiscountValue;
                        passOnAmount += item.passOnValue;
                        focSamplingAmount += item.focValue;
                        netAmount += item.netAmount;
                    }
                txtWeight.Text = weight.ToString();
                txtWeightChange.Text = weight.ToString();
                if(passOnAmount!=0)
                {
                    chkPassOn.IsChecked = true;
                    txtPassOn.Text = passOnAmount.ToString();
                }
                if (discountAmount != 0)
                {
                    chkDiscount.IsChecked = true;
                    txtDiscount.Text = discountAmount.ToString();
                }
                if (focSamplingAmount != 0)
                {
                    chkFocSamp.IsChecked = true;
                    txtFocSampling.Text = focSamplingAmount.ToString();
                }
                if (netAmount != 0)
                {
                    chkNetAmount.IsChecked = true;
                    txtNetAmount.Text = netAmount.ToString();
                }
                txtQuantity.Text = Quantity.ToString();
                txtQuantityChange.Text = Quantity.ToString();
                txtfob.Text = sumfob.ToString();
                txtcfr.Text = sumcfr.ToString();

                

                double fob, cfr, tax = 0;
                fob = Convert.ToDouble(txtfob.Text);
                cfr = Convert.ToDouble(txtcfr.Text);
                string str = txttax.Text.ToString();
                tax = taxAmount;
                txtTaxAmount.Text = taxAmount.ToString();
                
                double ExchangeRate = 1;
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDouble(txtexchangerate.Text);
                
                if (str.IndexOf("%") != -1)
                {
                    txttotalfob.Text = (fob + (fob * tax / 100)).ToString();
                    txttotalcfr.Text = (cfr + (cfr * tax / 100)).ToString();
                    txtBasetotalcfr.Text = ((cfr * ExchangeRate) + (cfr * tax / 100)).ToString();
                    txtBasetotalfob.Text = ((fob * ExchangeRate) + (fob * tax / 100)).ToString();
                }
                else
                {
                    txttotalfob.Text = (fob).ToString();
                    txttotalcfr.Text = (cfr).ToString();
                    txtSoAmount.Text = (fob+tax).ToString();
                    txtSoAmount1.Text = (fob+tax).ToString();
                    txtBasetotalfob.Text = ((fob * ExchangeRate) + tax).ToString();
                    txtBasetotalcfr.Text = ((cfr * ExchangeRate) + tax).ToString();

                }
                var remain = Math.Round(Convert.ToDouble(txtPOremainingcfr.Text), 2);
                if (cmbPurchaseOrderType.SelectedItem != null)
                    if (cmbPurchaseOrderType.SelectedItem.ToString() == InquiryType.Principal.ToString())
                    {
                        if (!string.IsNullOrEmpty(txtCommision.Text))
                        {
                            var comm = Math.Round(Convert.ToDouble(txtCommision.Text) - Convert.ToDouble(/*Math.Round(*/POCFRRemaining/*, 2)*/) +/* Math.Round(*/remain/*)*/, 2);
                            txtPOremainingcfr.Text = comm.ToString();
                            POCFRRemaining = Convert.ToDouble(txtCommision.Text);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txttotalcfr.Text))
                        {
                            var remaining = Math.Round(Convert.ToDouble(txttotalcfr.Text) - Convert.ToDouble(/*Math.Round(*/POCFRRemaining/*, 2)*/) + remain, 2).ToString();
                            txtPOremainingcfr.Text = remaining;
                            POCFRRemaining = Convert.ToDouble(txttotalcfr.Text);
                        }
                    }
                decimal marginExchangeRate = 1;
                decimal totalcfr = Convert.ToDecimal(txttotalcfr.Text);
                if (txtMarginexchangerate.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtSalestotalCfr.Text = (marginExchangeRate * totalcfr).ToString();
                }
                txtBillAfterTax.Text = txtBillWithTax.Text;
            }
            else
            {
                double sumfob = 0;
                double sumcfr = 0;
                decimal? weight = 0;
                double Quantity = 0;

                if (grdPOItems.ItemsSource != null)
                    foreach (var item in grdPOItems.ItemsSource as List<ProcurementProduct>)
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
                txtWeightChange.Text = weight.ToString();

                txtQuantity.Text = Quantity.ToString();
                txtQuantityChange.Text = Quantity.ToString();

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
                decimal ExchangeRate = 1;//Convert.ToInt32(txtexchangerate.Text)!=0&& 
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                //string str = txttax.Text.ToString();
                if (str.IndexOf("%") != -1)
                {
                    txttotalfob.Text = (fob + (fob * tax / 100)).ToString();
                    txttotalcfr.Text = (cfr + (cfr * tax / 100)).ToString();
                    txtBasetotalcfr.Text = ((cfr * ExchangeRate) + (cfr * tax / 100)).ToString();
                    txtBasetotalfob.Text = ((fob * ExchangeRate) + (fob * tax / 100)).ToString();

                }
                else
                {
                    txttotalfob.Text = (fob + tax).ToString();
                    txttotalcfr.Text = (cfr + tax).ToString();

                    txtBasetotalfob.Text = ((fob * ExchangeRate) + tax).ToString();
                    txtBasetotalcfr.Text = ((cfr * ExchangeRate) + tax).ToString();

                }
                var remain = Math.Round(Convert.ToDouble(txtPOremainingcfr.Text), 2);
                if (cmbPurchaseOrderType.SelectedItem != null)
                    if (cmbPurchaseOrderType.SelectedItem.ToString() == InquiryType.Principal.ToString())
                    {
                        if (!string.IsNullOrEmpty(txtCommision.Text))
                        {
                            var comm = Math.Round(Convert.ToDouble(txtCommision.Text) - Convert.ToDouble(/*Math.Round(*/POCFRRemaining/*, 2)*/) +/* Math.Round(*/remain/*)*/, 2);
                            txtPOremainingcfr.Text = comm.ToString();
                            POCFRRemaining = Convert.ToDouble(txtCommision.Text);

                        }

                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(txttotalcfr.Text))
                        {
                            var remaining = Math.Round(Convert.ToDouble(txttotalcfr.Text) - Convert.ToDouble(/*Math.Round(*/POCFRRemaining/*, 2)*/) + remain, 2).ToString();
                            txtPOremainingcfr.Text = remaining;
                            POCFRRemaining = Convert.ToDouble(txttotalcfr.Text);
                        }
                    }
                decimal marginExchangeRate = 1;
                decimal totalcfr = Convert.ToDecimal(txttotalcfr.Text);
                if (txtMarginexchangerate.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtSalestotalCfr.Text = (marginExchangeRate * totalcfr).ToString();
                }
            }


            
            //if (!string.IsNullOrEmpty(txtPOremainingcfr.Text) && !string.IsNullOrEmpty(txttotalcfr.Text))
            //    if (Convert.ToDouble(txtPOremainingcfr.Text) < 0 )
            //    {
            //        DXMessageBox.Show("Purchase Order can not be over invoiced, Please check again", "Remaining amount cant be less than 0.00 or greater than So amount");
            //    }
        }

        private void dGitems_CurrentCellChanged(object sender, EventArgs e)
        {
            calculatetotal();
        }

        private void cmbPaymentTerm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as cmbitem) != null)
            {
                int idd = (sender as cmbitem).id;
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
            cmbIncotermChange.ItemsSource = SYSTEM_STATIC.incoTermSource;
            cmbSOIncotermChange.ItemsSource = SYSTEM_STATIC.incoTermSource;  
        }
        public void loadCaptions()
        {

            cmbcaption1.ItemsSource = SYSTEM_STATIC.incoTermSource;
            cmbcaption2.ItemsSource = SYSTEM_STATIC.incoTermSource;
        }
        public void loadWarrantys()
        {
            cmbPOWarranty.ItemsSource = SYSTEM_STATIC.warrantySource;
            cmbSOWarranty.ItemsSource = SYSTEM_STATIC.warrantySource;
            cmbPOWarrantyChange.ItemsSource = SYSTEM_STATIC.warrantySource;
            cmbSOWarrantyChange.ItemsSource = SYSTEM_STATIC.warrantySource;
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

                cmbSOPaymentTerm.ItemsSource = cmbitems;
                cmbSOPaymentTermChange.ItemsSource = cmbitems;
            }
            catch (Exception ex) { }

            cmbitems = new List<cmbitem>();
            try
            {
                PaymentTermRepo TermRepo = new PaymentTermRepo();
                List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
                paymentTerms = TermRepo.getAllForPO();


                foreach (var paymentTerm in paymentTerms)
                {
                    cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
                }
                cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

                cmbPOPaymentTerm.ItemsSource = cmbitems;
                cmbPOPaymentTermChange.ItemsSource = cmbitems;
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
            if(cmbSOCurrency.SelectedItem != null)
            {
                string name = (cmbSOCurrency.SelectedItem as cmbitem).name;
                string caption1 = "";
                if (cmbcaption1.SelectedItem != null)
                {
                    //caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                    caption1 = "SO Value";
                }
                symbol = name.Substring(name.IndexOf("("));
                if (grdPOItems != null && grdPOItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                    grdPOItems.Columns.GetColumnByFieldName("value1").Header = caption1 + symbol +")";
                
            }
            
            

        }

        private void cmbcaption2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPOCurrency.SelectedItem != null)
            {
                string name = (cmbPOCurrency.SelectedItem as cmbitem).name;
                string caption2 = "";
                if (cmbcaption2.SelectedItem != null)
                    //caption2 = (cmbcaption2.SelectedItem as cmbitem).name;
                    caption2 = "PO Value";
                symbol = name.Substring(name.IndexOf("("));
                if (grdPOItems != null && grdPOItems.Columns.Count != 0 && cmbcaption2.SelectedItem != null)
                    grdPOItems.Columns.GetColumnByFieldName("value2").Header = caption2 + symbol + ")";
            }
           
        }
        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (company.Id == 0)
            {
                DXMessageBox.Show("Select Company Frist");
                lookupCompany.Focus();
                return;
            }
        }

        private void cmbEmployee_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                DXMessageBox.Show("Select Department Frist");
                lookupDepartment.Focus();
                return;
            }
        }

        private void lookupCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                DXMessageBox.Show("Select Department Frist");
                //lookupDepartment.Focus();
                return;
            }
        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (cmbPurchaseOrderType.SelectedItem != null)
                if (cmbPurchaseOrderType.SelectedItem.ToString() == InquiryType.Inventory.ToString())
                {
                    var colum = e.Column;
                    var row = e.Row as ProcurementProduct;
                    if (row != null && colum.FieldName != "value2" && colum.FieldName != "value2" && colum.FieldName != "value3")
                    {

                        row.value1 = row.inquiryProduct.quantity * row.unitPrice;
                        row.value2 = row.inquiryProduct.quantity * row.unitPrice;
                        row.value3 = row.inquiryProduct.quantity * row.unitPrice;
                    }
                    else
                    if (colum.FieldName == "value2")
                    {
                        if (row.value2 != 0)
                        {
                            if (row.inquiryProduct.quantity != 0)
                                row.unitPrice = row.value2 / row.inquiryProduct.quantity;
                        }
                    }
                }
                else
                if (cmbPurchaseOrderType.SelectedItem.ToString() == InquiryType.DistributionBiz.ToString())
                {
                    var row = e.Row as BookerStatementItem;
                    row.amount = row.unit * row.quantity;
                    row.netAmount = row.amount + row.amountGST - row.passOnValue - row.claimDiscountValue - row.focValue;
                }
            calculatetotal();
        }
        private void datBillOfLaddingdate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            //if (txtSalesTargetYear.Text != "")

            //    txtSalesTargetYear.Text = datBillOfLaddingdate.DateTime.Year.ToString();
            //if (txtSalesTargetMonth.Text != "")

            //    txtSalesTargetMonth.Text = datBillOfLaddingdate.DateTime.Month.ToString();
        }
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Items") != null)
            {
                Productss.frmItemadd frmItemadd = new Productss.frmItemadd();
                frmItemadd.ShowDialog();
                //products = productrepo.getAll();
                //products = SYSTEM_STATIC.GetItemsForCurrentUser();
                ////lookupProductinGrid.ItemsSource = products;
                //lookupProductsinGrid.ItemsSource = products;
            }
            else
            {
               DXMessageBox.Show("Permission required (Add New Items) to add new item!");
            }


        }
        private void view_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            if ((InquiryType)cmbPurchaseOrderType.SelectedIndex != InquiryType.DistributionBiz)
            {
                
                (grdPOItems.CurrentItem as ProcurementProduct).inquiryProduct = new InquiryProduct();
                
            }
        }
        private void winPurchaseOrderadd_Unloaded(object sender, RoutedEventArgs e)
        {
            UsersRepo UsersRepo = new UsersRepo();

            editpurchaseOrder = 0;
            saleOrderid = 0;
            purchaseOrderid = 0;
            if (purchaseOrder != null && purchaseOrder.Id != 0)
                UsersRepo.Add(TransactionInfo.viewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "Viewed details of Purchase Order");

            frmCostSheet.costSheet = new ERP_BL.Databases.CostSheet();
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdPOItems);


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
            DateTime creationDate = (DateTime)datpoCreationdate.EditValue;
            DateTime d1 = new DateTime(2016, 01, 01);
            ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;
            if (creationDate > d1)
            {
                if (cmbPOCurrency.SelectedIndex != -1 && cmbSOCurrency.SelectedIndex != -1)
                {
                    var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbPOCurrency.SelectedItem as cmbitem).id, (cmbSOCurrency.SelectedItem as cmbitem).id, creationDate.Year);
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
                            default:
                                if (exchangeRate != null)
                                    txtexchangerate.Text = 0.ToString();
                                break;
                        }
                    }

                }
                //calculatetotal();
            }
            else
            {
                CurrencyRepo currencyRepo = new CurrencyRepo();
                ///Working One
                if (cmbPOCurrency.SelectedItem != null)
                {
                    MarketExchangeRate ExchangeRate = new MarketExchangeRate();
                    SalesExchangeRate salesExchangeRate = new SalesExchangeRate();
                    ExchangeRate = currencyRepo.getMarketexchangerate(company.Id, (cmbPOCurrency.SelectedItem as cmbitem).id);
                    salesExchangeRate = currencyRepo.getsalesexchangerate(company.Id, (cmbPOCurrency.SelectedItem as cmbitem).id);
                    if (ExchangeRate != null)
                    {
                        txtexchangerate.Text = ExchangeRate.exchangerate.ToString();
                    }
                    else if (cmbbaseCurrency.SelectedItem != null)
                        if ((cmbbaseCurrency.SelectedItem as cmbitem).id == (cmbPOCurrency.SelectedItem as cmbitem).id)
                        {
                            txtexchangerate.Text = "1";
                            txtMarginexchangerate.Text = "1";
                        }
                    if (salesExchangeRate != null)
                    {
                        txtMarginexchangerate.Text = salesExchangeRate.exchangerate.ToString();
                    }
                }
                if (cmbPOCurrency.SelectedItem as cmbitem != null)
                {
                    int idd = (cmbPOCurrency.SelectedItem as cmbitem).id;
                    currency = currencyRepo.get(idd);
                    if (idd == 0)
                    {
                        BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                        frmCurrency.ShowDialog();
                        loadCurrencies();
                    }
                    else
                    {
                        string name = (cmbPOCurrency.SelectedItem as cmbitem).name;
                        string caption1 = "";
                        string caption2 = "";
                        if (cmbcaption1.SelectedItem != null)
                            caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                        if (cmbcaption2.SelectedItem != null)
                            caption2 = (cmbcaption2.SelectedItem as cmbitem).name;

                        symbol = name.Substring(name.IndexOf("("));
                        txtBaseCurrency.Text = name;
                        if (grdPOItems.Columns.Count != 0)
                        {
                            //grdPOItems.Columns.GetColumnByFieldName("value1").Header = caption1 + symbol;
                            //grdPOItems.Columns.GetColumnByFieldName("value2").Header = caption2 + symbol;
                            lblTotal.Text = "P.O Amount" + symbol;
                            lblCFRTotal.Text = "P.O Amount" + symbol;


                            string str = lblCommision.Text;
                            if (-1 != str.IndexOf("("))
                                lblCommision.Text = (str.Substring(0, str.IndexOf("("))) + symbol;
                            else
                                lblCommision.Text = lblCommision.Text + " " + symbol;
                            //string straa = lblBaseCommision.Text;
                            //if (-1 != straa.IndexOf("("))
                            //    lblBaseCommision.Text = (straa.Substring(0, straa.IndexOf("("))) + symbol;
                            //else
                            //    lblBaseCommision.Text = lblBaseCommision.Text + " " + symbol;


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
                            //string strig = lblSaleAMargin.Text;
                            //if (-1 != strig.IndexOf("("))
                            //    lblSaleAMargin.Text = (strig.Substring(0, strig.IndexOf("("))) + symbol;
                            //else
                            //    lblSaleAMargin.Text = lblSaleAMargin.Text + " " + symbol;
                        }
                        calculateBaseCommision();
                        calculateBaseBudgetMargin();
                        calculateBaseActualMargin();
                        calculatetotal();

                    }
                }
            }

            if (cmbPOCurrency.SelectedItem as cmbitem != null)
            {
                CurrencyRepo currencyRepo = new CurrencyRepo();
                int idd = (cmbPOCurrency.SelectedItem as cmbitem).id;
                currency = currencyRepo.get(idd);
                if (idd == 0)
                {
                    BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                    frmCurrency.ShowDialog();
                    loadCurrencies();
                }
                else
                {
                    string name = (cmbPOCurrency.SelectedItem as cmbitem).name;
                    string caption1 = "";
                    string caption2 = "";
                    if (cmbcaption1.SelectedItem != null)
                    {
                        caption1 = "SO Value";
                        //caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                    }
                    if (cmbcaption2.SelectedItem != null)
                    {
                        caption2 = "PO Value";
                        //caption2 = (cmbcaption2.SelectedItem as cmbitem).name;
                    }

                    symbol = name.Substring(name.IndexOf("("));
                    if (grdPOItems.Columns.Count != 0)
                    {

                        
                            grdPOItems.Columns.GetColumnByFieldName("value1").Header = caption1  + symbol;
                        grdPOItems.Columns.GetColumnByFieldName("value2").Header = caption2  + symbol ;
                        //grdPOItems.Columns[6].Header = caption1 + symbol;
                        //grdPOItems.Columns[7].Header = caption2 + symbol;
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

        private void CmbbaseCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPOCurrency.SelectedItem as cmbitem != null)
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
            if (txtActualMargin.Text != "")
            {
                decimal sumbudg = 0;

                sumbudg = Convert.ToDecimal(txtActualMargin.Text);
                var Sototal = Convert.ToDecimal(txtSoAmount.Text);
                if (purchaseOrder.SaleOrder != null)
                {
                    if (purchaseOrder.SaleOrder.saleOrdertype == InquiryType.SupplyCCC)
                    {
                        Sototal = Convert.ToDecimal(purchaseOrder.SaleOrder.costCenterAmount);
                    }
                }
                if (Sototal != 0)
                {
                    var percent = (((sumbudg) / Sototal) * 100);
                    txtActualPercent.Text = decimal.Round(percent, 2).ToString();
                    txtActualPercent.Text += "%";
                }

                decimal ExchangeRate = 1;
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                decimal marginExchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                decimal margin = Convert.ToDecimal(txtActualMargin.Text);
                //decimal basemargin = Convert.ToDecimal(txtcfr.Text);
                txtBaseActualMargin.Text = (margin * ExchangeRate).ToString();
                txtSaleAMargin.Text = (marginExchangeRate * margin).ToString();
            }
        }
        public void calculateBaseRevisedMargin()
        {
            if (txtRevisedMargin.Text != "")
            {
                decimal sumbudg = 0;

                sumbudg = Convert.ToDecimal(txtRevisedMargin.Text);
                var Sototal = Convert.ToDecimal(txtSoAmount.Text);
                if (purchaseOrder.SaleOrder != null)
                {
                    if (purchaseOrder.SaleOrder.saleOrdertype == InquiryType.SupplyCCC)
                    {
                        Sototal = Convert.ToDecimal(purchaseOrder.SaleOrder.costCenterAmount);
                    }
                }
                if (Sototal != 0)
                {
                    var percent = (((sumbudg) / Sototal) * 100);
                    txtRevisedPercent.Text = decimal.Round(percent, 2).ToString();
                    txtRevisedPercent.Text += "%";
                }

                decimal ExchangeRate = 1;
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                decimal marginExchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                decimal margin = (string.IsNullOrEmpty(txtActualMargin.Text)) ? 0 : Convert.ToDecimal(txtActualMargin.Text);
                //decimal basemargin = Convert.ToDecimal(txtcfr.Text);
                txtBaseRevisedMargin.Text = (margin * ExchangeRate).ToString();
                txtSaleRevisedMargin.Text = (marginExchangeRate * margin).ToString();
            }
        }
        public void calculateBaseBudgetMargin()
        {
            if (txtBudgetMargin.Text != "" /*&& txtActualMargin.Text != "" && txtActualMargin.Text != ""*/)
            {
                decimal sumbudg = 0;

                sumbudg = Convert.ToDecimal(txtBudgetMargin.Text);
                var Sototal = Convert.ToDecimal(txtSoAmount.Text);
                if(purchaseOrder.SaleOrder!=null)
                {
                    if(purchaseOrder.SaleOrder.saleOrdertype==InquiryType.SupplyCCC)
                    {
                        Sototal = Convert.ToDecimal(purchaseOrder.SaleOrder.costCenterAmount);
                    }
                }

                //if(sumbudg != 0)
                if (Sototal != 0)
                {
                    var percent = (((sumbudg) / Sototal) * 100);
                    txtBudgetedPercent.Text = decimal.Round(percent, 2).ToString();
                    txtBudgetedPercent.Text += "%";
                }



                decimal ExchangeRate = 1;
                decimal marginExchangeRate = 1;
                decimal margin = Convert.ToDecimal(txtBudgetMargin.Text);//Convert.ToInt32(txtexchangerate.Text)!=0&& 
                if (txtexchangerate.Text != "")
                {
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);
                    txtBaseBudgetMargin.Text = (margin * ExchangeRate).ToString();

                }
                //Convert.ToInt32(txtexchangerate.Text)!=0&& 
                if (txtMarginexchangerate.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtSaleBudgetMargin.Text = (marginExchangeRate * margin).ToString();
                }//decimal basemargin = Convert.ToDecimal(txtcfr.Text);
            }
        }
        public void calculateBaseCommision()
        {
            if (txtCommision.Text != "" && txtexchangerate.Text != "")
            {
                decimal ExchangeRate = 1;//Convert.ToInt32(txtexchangerate.Text)!=0&& 
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);

                decimal margin = Convert.ToDecimal(txtCommision.Text);
                //decimal basemargin = Convert.ToDecimal(txtcfr.Text);
                txtBaseCommission.Text = (margin * ExchangeRate).ToString();
                decimal marginExchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtSalesCommision.Text = (marginExchangeRate * margin).ToString();
                }
            }
        }
        public void calculateNetBaseCommision()
        {
            if (txtNetCommision.Text != "" && txtexchangerate.Text != "")
            {
                decimal ExchangeRate = 1;//Convert.ToInt32(txtexchangerate.Text)!=0&& 
                if (txtexchangerate.Text != "")
                    ExchangeRate = Convert.ToDecimal(txtexchangerate.Text);

                decimal margin = Convert.ToDecimal(txtNetCommision.Text);
                //decimal basemargin = Convert.ToDecimal(txtcfr.Text);
                txtNetBaseCommission.Text = (margin * ExchangeRate).ToString();
                decimal marginExchangeRate = 1;
                if (txtMarginexchangerate.Text != "")
                {
                    marginExchangeRate = Convert.ToDecimal(txtMarginexchangerate.Text);
                    txtNetSalesCommision.Text = (marginExchangeRate * margin).ToString();
                }
            }
        }
        private void TxtCommision_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txtCommision.Text != "" && purchaseOrder.Commision != null)
            {
                if (Convert.ToDouble(txtCommision.Text) != purchaseOrder.Commision)
                {
                    foreach (var item in grdPOItems.ItemsSource as List<ProcurementProduct>)
                    {
                        item.totalCommission = Convert.ToDouble(txtCommision.Text);
                        item.UnInvoicedSoAmount = Convert.ToDouble(Convert.ToDecimal(txtCommision.Text) - Convert.ToDecimal(purchaseOrder.Commision));
                    }
                    grdPOItems.RefreshData();
                }
                calculateBaseCommision();
                calculatetotal();
            }
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
            txtSoAmount1.Text = txttotalcfr.Text;
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
            btnCostSheetPunching.IsEnabled = false;
            try 
            {
                if (PoType == InquiryType.Standard)
                {
                    string inco = "";
                    string paymentterm = "";
                    if (cmbPOPaymentTermChange.SelectedItem != null)
                    {
                        paymentterm = (cmbPOPaymentTermChange.SelectedItem as cmbitem).name;
                    }
                    if (cmbIncotermChange.SelectedItem != null)
                    {
                        inco = (cmbIncotermChange.SelectedItem as cmbitem).name;
                    }

                    //public frmCostSheet(SaleOrder saleOrder, string currency, string incoterm, string Creationdate, string paymenttermcustomer, string maker, string origin, List<ViewInfo> viewinfos, InquiryType type, string Packing, string Warranty, bool? approved, bool? reApproved)

                    SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                    var dbsSaleOrder = saleOrderRepo.get(purchaseOrder.SaleOrder.Id);
                    frmCostSheet frmCostSheet = new frmCostSheet(dbsSaleOrder, symbol, inco, datpoCreationdate.Text, paymentterm, txtMakerChange.Text, txtOriginChange.Text, views, (InquiryType)cmbPurchaseOrderType.SelectedIndex, txtPackingChange.Text, cmbPOWarranty.Text, purchaseOrder.isApproved, purchaseOrder.isReApproved );
                    frmCostSheet.grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    //frmCostSheet.lookupVendors.IsEnabled = false;
                    //frmCostSheet.lookupOC.IsEnabled = false;
                    //frmCostSheet.isPurchaseOrder = true;
                    frmCostSheet.ShowDialog();
                    //if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.Principal)
                    //{
                    //    if (frmCostSheet.costSheet != null)
                    //    {
                    //        purchaseOrder.CostSheet = frmCostSheet.costSheet;
                    //        txtCommision.Text = (Convert.ToDecimal(txttotalcfr.Text) - purchaseOrder.CostSheet.TotalBudgetedMargin).ToString();

                    //    }
                    //}
                    //else
                    {
                        if (frmCostSheet.costSheet != null)
                        {
                            purchaseOrder.CostSheet = frmCostSheet.costSheet;
                            txtBudgetMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalBudgetedMargin).ToString();
                            txtRevisedMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalRevisedMargin).ToString();

                            txtActualMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) != (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalActualMargin)) ? (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalActualMargin).ToString() : "0";
                            if ((purchaseOrder.isReApproved != false) && frmCostSheet.isReApproved == false)
                                purchaseOrder.stage = TransactionStage.AwaitingFirstReview.ToString();
                            purchaseOrder.isReApproved = frmCostSheet.isReApproved;
                            purchaseOrder.CostSheet_Id = purchaseOrder.CostSheet.Id;
                            purchaseOrder.CostSheet = null;
                        }
                    }
                }
                else
                {
                    string inco = "";
                    string paymentterm = "";
                    if (cmbPOPaymentTerm.SelectedItem != null)
                    {
                        paymentterm = (cmbPOPaymentTerm.SelectedItem as cmbitem).name;
                    }
                    if (cmbIncoterm.SelectedItem != null)
                    {
                        inco = (cmbIncoterm.SelectedItem as cmbitem).name;
                    }
                    frmCostSheet frmCostSheet = new frmCostSheet(purchaseOrder.SaleOrder, symbol, inco, datpoCreationdate.Text, paymentterm, txtMakerChange.Text, txtOriginChange.Text, views, (InquiryType)cmbPurchaseOrderType.SelectedIndex, txtPackingChange.Text, cmbPOWarranty.Text, purchaseOrder.isApproved, purchaseOrder.isReApproved);
                    frmCostSheet.ShowDialog();
                    //if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.Principal)
                    //{
                    //    if (frmCostSheet.costSheet != null)
                    //    {
                    //        purchaseOrder.CostSheet = frmCostSheet.costSheet;
                    //        txtCommision.Text = (Convert.ToDecimal(txttotalcfr.Text) - purchaseOrder.CostSheet.TotalBudgetedMargin).ToString();

                    //    }
                    //}
                    //else
                    {
                        if (frmCostSheet.costSheet != null)
                        {
                            purchaseOrder.CostSheet = frmCostSheet.costSheet;
                            txtBudgetMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalBudgetedMargin).ToString();
                            txtRevisedMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalRevisedMargin).ToString();

                            txtActualMargin.Text = (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) != (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalActualMargin)) ? (Convert.ToDecimal(purchaseOrder.SaleOrder.totalCFRValue) - purchaseOrder.CostSheet.TotalActualMargin).ToString() : "0";
                            if ((purchaseOrder.isReApproved != false) && frmCostSheet.isReApproved == false)
                                purchaseOrder.stage = TransactionStage.AwaitingFirstReview.ToString();
                            purchaseOrder.isReApproved = frmCostSheet.isReApproved;

                        }
                    }
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
            if (gridTracker.Visibility == Visibility.Collapsed && purchaseOrder.Id != 0)
            {
                UsersRepo UsersRepo = new UsersRepo();

                views = UsersRepo.getViwerInfo(purchaseOrder.Id, (int)TransactionItemType.Purchase_Order);
                grdUsers.ItemsSource = views;
                gridTracker.Visibility = Visibility.Visible;
            }
        }
        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrderRepo purchaseOrderrepo = new PurchaseOrderRepo();
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (editOrder == 1 && purchaseOrder != null)
                {

                    purchaseOrderrepo = new PurchaseOrderRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    purchaseOrder = new PurchaseOrder();
                    purchaseOrder = purchaseOrderrepo.get(purchaseOrderid);
                    UsersRepo usersRepo = new UsersRepo();

                    if (purchaseOrder != null)
                    {
                        if (purchaseOrder.isApproved == true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Purchase Order is Approved, Do you want to UnApprove this Purchase Order?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    purchaseOrder.isApproved = false;
                                    purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();

                                    //receipt.isApproved = true;

                                    //receipt.stage = TransactionStage.Approved.ToString();
                                    //frmInputBox inputBox = new frmInputBox();
                                    //inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, purchaseOrder.Id, 6, "PO has been UnApproved");
                                    //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                    //saleOrderrepo = new SaleOrderRepo();
                                    purchaseOrderRepo.Approve(purchaseOrder);

                                    var res1 = MessageBox.Show("PO has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {

                                        if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0 && purchaseOrder.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && purchaseOrder.InterDepartment != null && purchaseOrder.InterDepartment.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count > 0)
                                            {
                                                if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                                {
                                                    purchaseOrder.holderChangeDate = DateTime.Now;
                                                }
                                                purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                                purchaseOrderRepo.updatePO(purchaseOrder);
                                            }
                                        }
                                        else if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count > 0)
                                            {
                                                if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                                {
                                                    purchaseOrder.holderChangeDate = DateTime.Now;
                                                }
                                                purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                                purchaseOrderRepo.updatePO(purchaseOrder);
                                            }
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }


                                        //if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                        //{
                                        //    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(purchaseOrder.department.Id, purchaseOrder.company.Id));
                                        //    win.ShowDialog();
                                        //    tagUsers = win.tagUsers;
                                        //    ccUsers = win.ccUsers;
                                        //}
                                        //else
                                        //{
                                        //    winTagUsers win = new winTagUsers();
                                        //    win.ShowDialog();
                                        //}

                                    }

                                    string symbolCurr = "";
                                    if (purchaseOrder.currency != null)
                                    {
                                        symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "PO (Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been UnApproved \nFrom: ",
                                        Timestamp = DateTime.Now,
                                        Subject = "PO UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0,user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Purchase Order is UnApproved (" + purchaseOrder.SyetmReferenceNo + ")");
                                    SystemLog.LogInfo(this.GetType(), "Purchase Order is UnApproved (" + purchaseOrder.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Purchase Oder Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Purchase Oder Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (purchaseOrder.isApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("Purchase Order are Pending for Approval, Do you want to Approve this Purchase Order?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    purchaseOrder.isApproved = true;
                                    purchaseOrder.stage = TransactionStage.Approved.ToString();
                                    //frmInputBox inputBox = new frmInputBox();
                                    //inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, purchaseOrder.Id, 6, "PO has been Approved");

                                    purchaseOrderRepo.Approve(purchaseOrder);

                                    var res1 = MessageBox.Show("PO has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0 && purchaseOrder.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && purchaseOrder.InterDepartment != null && purchaseOrder.InterDepartment.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                                            //inputBox.ShowDialog();
                                            if (win.tagUsers.Count > 0)
                                            {
                                                if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                                {
                                                    purchaseOrder.holderChangeDate = DateTime.Now;
                                                }
                                                purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                                purchaseOrderRepo.updatePO(purchaseOrder);
                                            }
                                        }
                                        else if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            if (win.tagUsers.Count > 0)
                                            {
                                                if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                                {
                                                    purchaseOrder.holderChangeDate = DateTime.Now;
                                                }
                                                purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                                purchaseOrderRepo.updatePO(purchaseOrder);
                                            }
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }

                                    }
                                    
                                    string symbolCurr = "";
                                    if (purchaseOrder.currency != null)
                                    {
                                        symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "PO (Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been Approved \nFrom: " ,
                                        Timestamp = DateTime.Now,
                                        Subject = "PO Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0,user.id, "New Comment ", null);
                                        }
                                    }

                                    MessageBox.Show("Purchase Oder is Approved (" + purchaseOrder.SyetmReferenceNo + ")");
                                    SystemLog.LogInfo(this.GetType(), "Purchase Order is Approved (" + purchaseOrder.Id + ")");
                                }
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Purchase Order Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Purchase Order Directly user id=(" + MainWindow.currentUserid + ")");
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



            //if (purchaseOrder.isApproved != true)
            //{
            //    if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null)
            //    {
            //        purchaseOrder.isReviewed = true;
            //        purchaseOrder.isApproved = true;
            //        purchaseOrder.stage = TransactionStage.Approved.ToString();
            //        purchaseOrderRepo.update(purchaseOrder);
            //        if (addinfo)
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //            _usersRepo.Add(TransactionInfo.Approved_Adding, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
            //            addinfo = false;
            //        }
            //        return;

            //    }
            //    else if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null))
            //    {
            //        purchaseOrder.isReviewed = true;
            //        purchaseOrder.needReview = false;
            //        purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();

            //        purchaseOrderRepo.update(purchaseOrder);
            //        if (addinfo)
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //            _usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
            //            addinfo = false;
            //        }
            //    }
            //    if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null))
            //    {
            //        purchaseOrder.isReviewed = true;
            //        purchaseOrder.needReview = true;
            //        purchaseOrder.stage = TransactionStage.AwaitingSecondReview.ToString();

            //        purchaseOrderRepo.update(purchaseOrder);
            //        if (addinfo)
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //            _usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
            //            addinfo = false;
            //        }
            //    }
            //}
            //else if (purchaseOrder.PendingForClosing == true)
            //{
            //    if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null)
            //    {
            //        purchaseOrder.isReviewed = true;
            //        purchaseOrder.PendingForClosing = false;
            //        purchaseOrder.stage = TransactionStage.Closed.ToString();
            //        purchaseOrderRepo.update(purchaseOrder);
            //        if (addinfo)
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //            _usersRepo.Add(TransactionInfo.Approved_Closing, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
            //            addinfo = false;
            //        }
            //        return;

            //    }
            //    else if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null))
            //    {
            //        purchaseOrder.isReviewed = true;
            //        purchaseOrder.needReview = false;
            //        purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();

            //        purchaseOrderRepo.update(purchaseOrder);
            //        if (addinfo)
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //            _usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
            //            addinfo = false;
            //        }
            //    }
            //    if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null))
            //    {
            //        purchaseOrder.isReviewed = true;
            //        purchaseOrder.needReview = true;
            //        purchaseOrder.stage = TransactionStage.AwaitingSecondReview.ToString();

            //        purchaseOrderRepo.update(purchaseOrder);
            //        if (addinfo)
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //            _usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
            //            addinfo = false;
            //        }
            //    }
            //}


        }
        private void GrdUsers_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {
                // if (e.GetListSourceFieldValue("employee.person.FName") != null|| e.GetListSourceFieldValue("employee.person.LName") != null )
                {


                    string fname = viewInfo.User.employee.person.FName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.FName"));
                    string lname = viewInfo.User.employee.person.LName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.LName"));

                    //DateTime date;

                    e.Value = fname + " " + lname;
                }
            }
        }
        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            UsersRepo UsersRepo = new UsersRepo();

            if (purchaseOrder.isApproved != true)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null)
                {
                    purchaseOrder.isReviewed = true;
                    purchaseOrder.isApproved = false;
                    purchaseOrder.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, purchaseOrder.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseOrderRepo.update(purchaseOrder);
                    notificationsRepo.Add("PurchaseOrder Rejected", purchaseOrder.Id, TransactionItemType.Purchase_Order, "PurchaseOrder with refrence # " + purchaseOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseOrder" + frmInputBox.comment, null);

                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null))
                {
                    purchaseOrder.isReviewed = false;
                    purchaseOrder.stage = TransactionStage.Rejected.ToString();
                    //if(purchaseOrder.isApproved == null)
                    //{
                    //    purchaseOrder.isApproved = false;

                    //}
                    //purchaseOrderRepo.update(purchaseOrder);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, purchaseOrder.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseOrderRepo.update(purchaseOrder);
                    notificationsRepo.Add("PurchaseOrder Rejected", purchaseOrder.Id, TransactionItemType.Purchase_Order, "PurchaseOrder with refrence # " + purchaseOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseOrder" + frmInputBox.comment, null);

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null))
                {
                    purchaseOrder.isReviewed = false;
                    //if (purchaseOrder.isApproved == null)
                    //{
                    //    purchaseOrder.isApproved = false;

                    //}
                    purchaseOrder.stage = TransactionStage.Rejected.ToString();
                    //purchaseOrderRepo.update(purchaseOrder);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, purchaseOrder.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseOrderRepo.update(purchaseOrder);
                    notificationsRepo.Add("PurchaseOrder Rejected", purchaseOrder.Id, TransactionItemType.Purchase_Order, "PurchaseOrder with refrence # " + purchaseOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseOrder" + frmInputBox.comment, null);

                }
            }
            else if (purchaseOrder.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null)
                {
                    purchaseOrder.isReviewed = true;
                    //if (purchaseOrder.PendingForClosing == null)
                    {
                        purchaseOrder.PendingForClosing = true;

                    }
                    //purchaseOrder.PendingForClosing = false;
                    purchaseOrder.stage = TransactionStage.Rejected.ToString();
                    //purchaseOrderRepo.update(purchaseOrder);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, purchaseOrder.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseOrderRepo.update(purchaseOrder);
                    notificationsRepo.Add("PurchaseOrder Rejected", purchaseOrder.Id, TransactionItemType.Purchase_Order, "PurchaseOrder with refrence # " + purchaseOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseOrder" + frmInputBox.comment, null);

                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null))
                {
                    purchaseOrder.isReviewed = true;
                    purchaseOrder.needReview = false;
                    purchaseOrder.stage = TransactionStage.Rejected.ToString();
                    //if (purchaseOrder.PendingForClosing == null)
                    //{
                    //    purchaseOrder.PendingForClosing = true;

                    //}
                    //purchaseOrderRepo.update(purchaseOrder);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, purchaseOrder.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseOrderRepo.update(purchaseOrder);
                    notificationsRepo.Add("PurchaseOrder Rejected", purchaseOrder.Id, TransactionItemType.Purchase_Order, "PurchaseOrder with refrence # " + purchaseOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseOrder" + frmInputBox.comment, null);

                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null))
                {
                    //if (purchaseOrder.PendingForClosing == null)
                    //{
                    //    purchaseOrder.PendingForClosing = true;

                    //}
                    purchaseOrder.isReviewed = true;
                    purchaseOrder.needReview = true;
                    purchaseOrder.stage = TransactionStage.Rejected.ToString();

                    //purchaseOrderRepo.update(purchaseOrder);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, purchaseOrder.Id, 6, frmInputBox.comment);
                        addinfo = false;
                    }
                    purchaseOrderRepo.update(purchaseOrder);
                    notificationsRepo.Add("PurchaseOrder Rejected", purchaseOrder.Id, TransactionItemType.Purchase_Order, "PurchaseOrder with refrence # " + purchaseOrder.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", purchaseOrder.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this PurchaseOrder" + frmInputBox.comment, null);

                }
            }
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Order") != null)
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

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo UsersRepo = new UsersRepo();

            NotificationsRepo notificationsRepo = new NotificationsRepo();
            if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
            {

                
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                inputBox.ShowDialog();
            }
            else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Purchase_Order);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (purchaseOrder != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && purchaseOrder.Id != 0)
                {
                    var commentId = procurementRepo.AddCommentLinkNotification(purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                    if (commentId != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in PO #" + purchaseOrder.POReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                            else
                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in PO #" + purchaseOrder.POReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                        }

                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in PO #" + purchaseOrder.POReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                            else
                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in PO #" + purchaseOrder.POReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                        }
                    }

                    if (frmInputBox.Comment.TaggedList.Count > 0)
                    {

                        var transactionHolder = frmInputBox.Comment.TaggedList[0].employeeId;
                        var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                        cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == transactionHolder))];
                    }
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (purchaseOrder.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Purchase Order first to add a comment!");
                }

            }
        }
        public void ConvertByteToBmp(byte[] bytesArr)
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

                //UserImage.Source = returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void loadcomments()
        {
            try
            {
                if (purchaseOrder != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(purchaseOrder.Id, TransactionItemType.Purchase_Order);
                    
                    
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
                if (cmbPOPaymentTerm.SelectedItem != null)
                {
                    paymentterm = (cmbPOPaymentTerm.SelectedItem as cmbitem).name;
                }
                if (cmbIncoterm.SelectedItem != null)
                {
                    inco = (cmbIncoterm.SelectedItem as cmbitem).name;
                }
                frmCommissionSummarySheet frmSummarySheet = new frmCommissionSummarySheet(purchaseOrder.SaleOrder, txtFinanaceRef.Text, txtsaleOrderref.Text, txtOfferRefNo.Text, lookupCustomer.Text, (cmbPOCurrency.SelectedItem as cmbitem).id, txtCommision.Text, datsaleOrderdate.Text, paymentterm, (InquiryType)cmbPurchaseOrderType.SelectedIndex, /*lookupPOPrincipal.Text*/"", txttotalcfr.Text, txttotalfob.Text, txtPacking.Text, inco, datDeliverydate.DateTime, cmbTransshipment.Text, datpoCreationdate.Text, txtSalesref.Text, views);
                frmSummarySheet.ShowDialog();
                if ((InquiryType)cmbPurchaseOrderType.SelectedIndex == InquiryType.Principal)
                {
                    if (frmCommissionSummarySheet.summarySheet != null)
                    {
                        purchaseOrder.CostSheet = null;
                        purchaseOrder.CommissionSummarySheet = frmCommissionSummarySheet.summarySheet;
                        txtCommision.Text = (purchaseOrder.CommissionSummarySheet.SOCommission).ToString();
                        txtNetCommision.Text = (purchaseOrder.CommissionSummarySheet.netSOCommission).ToString();
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
            //int index = lstComments.IndexFromPoint(e.Location);
            //lstComments.SelectedIndex = index;
        }

        private void TxtNetCommision_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            calculateNetBaseCommision();

        }
        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach.Visibility == Visibility.Visible)
                grdAttach.Visibility = Visibility.Collapsed;
            else
            {
                grdAttach.Visibility = Visibility.Visible;
                if (purchaseOrder.Id != 0)
                {
                    cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                }
            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                if (purchaseOrder.Id != 0)
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(purchaseOrder.Id, TransactionItemType.Purchase_Order);
                  
                    //cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
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
                        //Button thisButton = (Button)sender;

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
                    //grdProgressBar.Visibility = Visibility.Collapsed;
                }
                else
                    return;



            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());


            }
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            UsersRepo UsersRepo = new UsersRepo();

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
                        destination += "Attachments\\PurchaseOrder\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += OrderId + "_" + TransactionItemType.Purchase_Order.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            System.IO.File.Move(sourceFile, destination);
                            if (sourceFile.Length < 74)
                            {
                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                            {
                                ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                var result = attachment.startUploading(TransactionItemType.Purchase_Order);
                                if (result.Item1)
                                {
                                    AttachmentsRepo repo = new AttachmentsRepo();
                                    //Attachment attachmen= new Attachment();
                                    repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Purchase_Order, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                    UsersRepo.Add(TransactionInfo.Attachment_Uploaded, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "Added a new attachment");

                                    this.Dispatcher.Invoke(() =>
                                    {
                                        treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Purchase_Order);
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


                            //DXMessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        DXMessageBox.Show(ex.ToString());
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
                frmTrackingWindow trackingWindow = new frmTrackingWindow(OrderId, TransactionItemType.Purchase_Order);
                trackingWindow.ShowDialog();
            }
        }

        
        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (purchaseOrder.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void PurchaseOrder") != null))
            {
                if (DXMessageBox.Show("This So is currently in the list of Void Purchase Orders! Do you want to remove it from Void?", "Remove Void Purchase Order", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    purchaseOrder.isVoid = false;
                    purchaseOrderRepo.setSotoVoid(purchaseOrder.Id, false);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("PO has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0 && purchaseOrder.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && purchaseOrder.InterDepartment != null && purchaseOrder.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseOrder.holderChangeDate = DateTime.Now;
                                }
                                purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseOrderRepo.update(purchaseOrder);
                            }
                        }
                        else if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (purchaseOrder.currency != null)
                    {
                        symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "PO (Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid \nFrom: ",
                        Timestamp = DateTime.Now,
                        Subject = "PO UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0,user.id, "New Comment ",null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void PurchaseOrder") != null)
            {
                if (DXMessageBox.Show("This So is not currently in the list of Void Purchase Orders! Do you want to move it to Void Saleorders?", "Add to Void Saleorders", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    purchaseOrder.isVoid = true;
                    purchaseOrderRepo.setSotoVoid(purchaseOrder.Id, true);
                    grdVoid.Visibility = Visibility.Visible;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("PO has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0 && purchaseOrder.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && purchaseOrder.InterDepartment != null && purchaseOrder.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                            //inputBox.ShowDialog();
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseOrder.holderChangeDate = DateTime.Now;
                                }
                                purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseOrderRepo.update(purchaseOrder);
                            }
                        }
                        else if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    purchaseOrder.holderChangeDate = DateTime.Now;
                                }
                                purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;
                                purchaseOrderRepo.update(purchaseOrder);
                            }
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (purchaseOrder.currency != null)
                    {
                        symbolCurr = purchaseOrder.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "PO (Amount OC) having value: " + purchaseOrder.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been marked as void \nFrom: ",
                        Timestamp = DateTime.Now,
                        Subject = "PO Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(purchaseOrder.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + purchaseOrder.SalesReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, comment.Comment, 0,user.id, "New Comment ",null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }
        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            UsersRepo UsersRepo = new UsersRepo();

            NotificationsRepo notificationsRepo = new NotificationsRepo();
            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (purchaseOrder.Id != 0)
                {

                    
                    if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    {


                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), comment, TransactionItemType.Purchase_Order);
                        inputBox.ShowDialog();
                    }
                    else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.Purchase_Order);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (purchaseOrder != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && purchaseOrder.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in PO #" + purchaseOrder.POReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in PO #" + purchaseOrder.POReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in PO #" + purchaseOrder.POReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in PO #" + purchaseOrder.POReferenceNo, purchaseOrder.Id, TransactionItemType.Purchase_Order, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            //{
                            //    foreach (var user in frmInputBox.Comment.TaggedList)
                            //    {
                            //        if (frmInputBox.FlagForTag == true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, user.id, "New Comment ", null);

                            //    }
                            //    foreach (var user in frmInputBox.Comment.CCUsersList)
                            //    {

                            //        if(frmInputBox.FlagForCC==true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0,user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //    }
                            //}

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.Purchase_Invoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (purchaseOrder.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Purchase Order first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
        }

        

        private void CmbPurchaseOrderStatusChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbPurchaseOrderStatusChange.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbPurchaseOrderStatusChange.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Procurementss.PurchaseOrderss.frmPurchaseOrderStatusAdd statusAdd = new Procurementss.PurchaseOrderss.frmPurchaseOrderStatusAdd();
                    statusAdd.ShowDialog();
                    loadPurchaseOrderStatus();
                }
                else
                {
                    loadPurchaseOrderStatusClass(idd);

                }
            }
        }

        private void CmbSOPaymentTermChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as cmbitem) != null)
            {
                int idd = (sender as cmbitem).id;
                if (idd == 0)
                {
                    Termss.frmPaymentTermAdd paymentTerms = new Termss.frmPaymentTermAdd();
                    paymentTerms.ShowDialog();
                    loadPaymentTerms();

                }
            }
        }

        private void CmbSOIncotermChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbIncotermChange.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbIncotermChange.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Termss.frmIncotermAdd incoterms = new Termss.frmIncotermAdd();
                    incoterms.ShowDialog();
                    loadIncoterms();
                }
            }
        }

        private void CmbPOPaymentTermChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as cmbitem) != null)
            {
                int idd = (sender as cmbitem).id;
                if (idd == 0)
                {
                    Termss.frmPaymentTermAdd paymentTerms = new Termss.frmPaymentTermAdd();
                    paymentTerms.ShowDialog();
                    loadPaymentTerms();

                }

            }
        }

        private void CmbIncotermChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CmbIncoandWarrentyChanged();
        }

        private void CmbPOWarrantyChange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CmbIncoandWarrentyChanged();
        }
       public void CmbIncoandWarrentyChanged()
        {
            if ((cmbIncotermChange.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbIncotermChange.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Termss.frmIncotermAdd incoterms = new Termss.frmIncotermAdd();
                    incoterms.ShowDialog();
                    loadIncoterms();
                }
            }
        }

        private void DatBillOfLaddingdateChange_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            //Not set at TJ code
        }

        private void DatPaymentDueFromChange_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            setPaymentTillDateChange();
        }

        private void TxtPaymentDueDaysChange_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txtPaymentDueDaysChange.Text != "")
            {
                setPaymentTillDateChange();
            }
        }
        public void setPaymentTillDateChange()
        {
            if (datPaymentDueFromChange.Text != "" && txtPaymentDueDaysChange.Text != "")
                datPaymentDueTillChange.DateTime = datPaymentDueTillChange.DateTime.AddDays(Convert.ToInt32(txtPaymentDueDaysChange.Text));//new DateTime(datPaymentDueFrom.DateTime.Year, datPaymentDueFrom.DateTime.Month, datPaymentDueFrom.DateTime.Day+Convert.ToInt32(txtPaymentDueDays.Text));
            else
                datPaymentDueTillChange.Text = "";
        }

        private void BtnDirectClose_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();
            PurchaseOrderRepo purchaseOrderrepo = new PurchaseOrderRepo();
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null) ? true : false)
            {

               
                    UsersRepo usersRepo = new UsersRepo();
                if (purchaseOrderid == 0)
                {
                    return;
                }
                   purchaseOrder = purchaseOrderRepo.get(purchaseOrderid);

                   var row = purchaseOrder;
                    if (row.PurchaseOrderStatus != null)
                    {
                        oldStatus = row.PurchaseOrderStatus;
                    }
                    PurchaseOrderss.ucStatuschange.inActiveStatuses = 1;

                    PurchaseOrderss.ucStatuschange.purchaseOrderid = purchaseOrderid;
                    PurchaseOrderss.frmPurchaseOrderStatusChange statusChange = new PurchaseOrderss.frmPurchaseOrderStatusChange(purchaseOrderrepo);
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    //if (PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Id == row.PurchaseOrderStatus.Id)
                    //    return;
                    if (PurchaseOrderss.ucStatuschange.purchaseOrder.Id != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null) ? true : false)
                        {
                            PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = false;
                            PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.purchaseOrderRepo.update(Inquiriess.ucStatuschange.purchaseOrder);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                        {
                            PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();
                            if (PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing == null)
                            {
                                PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null)
                        {
                            PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing != true)
                            {
                                PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;

                            }
                            //PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                        }
                        else
                        {
                            PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                            PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;
                            usersRepo.Add(TransactionInfo.Closed, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                        }
                    //PurchaseOrderss.ucStatuschange.purchaseOrder.user_Id = MainWindow.currentUserid;
                    PurchaseOrderss.ucStatuschange.purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                    PurchaseOrderss.ucStatuschange.purchaseOrder.ClosingDate = System.DateTime.Now;
                    if (row.PurchaseOrderStatus != PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus)
                        usersRepo.Add(TransactionInfo.Status_Changed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                    //Adding signature (comment)

                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("PO has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                    if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                    {
                        winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                        win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        if (win.tagUsers.Count > 0)
                        {
                            if (purchaseOrder.transactionHolderId != win.tagUsers[0].employeeId)
                            {
                                purchaseOrder.holderChangeDate = DateTime.Now;
                            }
                            purchaseOrder.transactionHolderId = win.tagUsers[0].employeeId;
                            purchaseOrderRepo.updatePO(purchaseOrder);
                        }
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
                    string newStat = PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status;
                    string symbolCurr = "";
                    if (row.currency != null)
                    {
                        symbolCurr = row.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of PO (Amount OC) having value: " + row.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Order, comment.Comment, 0,user.id, "New Comment ", null);
                        }
                    }

                    row = PurchaseOrderss.ucStatuschange.purchaseOrder;
                   
                    purchaseOrderrepo.updateStatus(row.Id, row.PurchaseOrderStatus);
                    //PurchaseOrderss.ucStatuschange.UpdatePurchaseOrder();//purchaseOrderRepo.update(PurchaseOrderss.ucStatuschange.purchaseOrder);
                    MessageBox.Show("PurchaseOrder status changed to InActive (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                myWindow = Window.GetWindow(this);
                myWindow.Close();
                

            }
            else
            {
                MessageBox.Show("You are not Allowed to Close PurchaseOrder Directly.");
            }
        }

        private void txttax_TextChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txttax.Text != "")
            {
                calculatetotal();
            }
        }

        private void LookUpTax_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

            if (lookUpTax.SelectedIndex > -1)
            {
                if ((lookUpTax.SelectedItem as TaxName).isManual != false)
                {
                    txttax.IsReadOnly = false;

                }
                else
                {
                    txttax.IsReadOnly = true;

                }
                var item = lookUpTax.SelectedItem as TaxName;
                var percent = item.percentage;
                var amount = Convert.ToDouble(txtcfr.Text);
                var percentAmount = (percent / 100) * amount;
                txtTaxAmount.Text = percentAmount.ToString();
                amount = amount + percentAmount;
                txtBillWithTax.Text = amount.ToString();

                if (chkWHT.IsChecked == true && lookUpWHT.SelectedIndex > -1)
                {
                    var amount1 = Convert.ToDouble(txtBillWithTax.Text);
                    var item1 = lookUpWHT.SelectedItem as TaxName;
                    var percent1 = item1.percentage;
                    var percentAmount1 = (percent1 / 100) * amount1;
                    txtWhtAmount.Text = percentAmount1.ToString();
                    amount1 = amount1 - percentAmount1;
                    txtBillAfterTax.Text = amount1.ToString();
                }
                else
                {
                    txtBillAfterTax.Text = amount.ToString();
                }
            }
        }

        private void ChkTax_Checked(object sender, RoutedEventArgs e)
        {
            lookUpTax.IsEnabled = true;
            TaxRepo taxRepo = new TaxRepo();
            chkAdjustedTax.IsChecked = false;
            lookUpTax.ItemsSource= taxRepo.getAllUnAdjustedTaxes();
                



            //if (lookUpTax.SelectedIndex > -1)
            //{
            //    var amount = Convert.ToDouble(txtcfr.Text);
            //    var item = lookUpTax.SelectedItem as TaxName;
            //    var percent = item.percentage;
            //    var percentAmount = (percent / 100) * amount;
            //    txtTaxAmount.Text = percentAmount.ToString();
            //    amount = amount + percentAmount;
            //    txtBillWithTax.Text = amount.ToString();
            //}

            //if (chkWHT.IsChecked == true)
            //{
            //    var item = lookUpWHT.SelectedItem as TaxName;
            //    var percent = item.percentage;
            //    var amount = Convert.ToDouble(txtBillWithTax.Text);

            //    var percentAmount = (percent / 100) * amount;
            //    txtWhtAmount.Text = percentAmount.ToString();
            //    amount = amount - percentAmount;
            //    txtBillAfterTax.Text = amount.ToString();
            //}
            //else
            //{
            //    txtBillAfterTax.Text = txtBillWithTax.Text;
            //}
        }

        private void ChkTax_Unchecked(object sender, RoutedEventArgs e)
        {
            lookUpTax.IsEnabled = false;
            txtBillWithTax.Text = txtcfr.Text;
            lookUpTax.SelectedIndex = -1;
            txtTaxAmount.Text = 0.ToString();

            //if (chkWHT.IsChecked == true && lookUpWHT.SelectedIndex > -1)
            //{
            //    var item = lookUpWHT.SelectedItem as TaxName;
            //    var percent = item.percentage;
            //    var amount = Convert.ToDouble(txtBillWithTax.Text);

            //    var percentAmount = (percent / 100) * amount;
            //    txtWhtAmount.Text = percentAmount.ToString();
            //    amount = amount - percentAmount;
            //    txtBillAfterTax.Text = amount.ToString();
            //}
            //else
            //{
            //    txtBillAfterTax.Text = txtcfr.Text;
            //}
        }

        private void ChkWHT_Checked(object sender, RoutedEventArgs e)
        {
            lookUpWHT.IsEnabled = true;

            if (lookUpWHT.SelectedIndex > -1)
            {
                var amount = Convert.ToDouble(txtBillWithTax.Text);
                var item = lookUpWHT.SelectedItem as TaxName;
                var percent = item.percentage;
                var percentAmount = (percent / 100) * amount;
                txtWhtAmount.Text = percentAmount.ToString();
                amount = amount - percentAmount;
                txtBillAfterTax.Text = amount.ToString();
            }
        }

        private void ChkWHT_Unchecked(object sender, RoutedEventArgs e)
        {
            lookUpWHT.IsEnabled = false;
            txtBillAfterTax.Text = txtBillWithTax.Text;
        }

        private void LookUpWHT_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var item = lookUpWHT.SelectedItem as TaxName;
            var percent = item.percentage;
            var amount = Convert.ToDouble(txtBillWithTax.Text);

            var percentAmount = (percent / 100) * amount;
            txtWhtAmount.Text = percentAmount.ToString();
            amount = amount - percentAmount;
            txtBillAfterTax.Text = amount.ToString();
        }

        private void Txtfob_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            
        }

        private void Txtcfr_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

            if (!String.IsNullOrEmpty(txtcfr.Text))
            {
                var amount = Convert.ToDouble(txtcfr.Text);

                if (chkTax.IsChecked == true && lookUpTax.SelectedIndex > -1)
                {
                    var item = lookUpTax.SelectedItem as TaxName;
                    var percent = item.percentage;
                    var percentAmount = (percent / 100) * amount;
                    txtTaxAmount.Text = percentAmount.ToString();
                    amount = amount + percentAmount;
                    txtBillWithTax.Text = amount.ToString();
                }
                else
                {
                    txtBillWithTax.Text = amount.ToString();
                }

                if (chkWHT.IsChecked == true && lookUpWHT.SelectedIndex > -1)
                {
                    var item = lookUpWHT.SelectedItem as TaxName;
                    var percent = item.percentage;
                    var percentAmount = (percent / 100) * amount;
                    txtWhtAmount.Text = percentAmount.ToString();
                    amount = amount - percentAmount;
                    txtBillAfterTax.Text = amount.ToString();
                }
                else
                {
                    txtBillAfterTax.Text = txtBillWithTax.Text;
                }
            }
        }

        private void BtnCostSheetPunching_Click(object sender, RoutedEventArgs e)
        {
            btnCostSheet.IsEnabled = false;
            try
            {
                var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Purchase Order?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update CostSheet from Purchase Order") != null)
                    {
                        if (saleOrder.Id != 0 && !string.IsNullOrEmpty(txtSoAmountSOC.Text) && purchaseOrder.Id != 0)
                        {
                            winSelectCostSheetFields winSelectCostSheetFields = new winSelectCostSheetFields(saleOrder, Convert.ToDecimal(txtSoAmountSOC.Text), purchaseOrder.Id, TransactionItemType.Purchase_Order);
                            winSelectCostSheetFields.ShowDialog();
                        }
                    }
                    else
                        DXMessageBox.Show("You do not have permisssion Update CostSheet from Purchase Order", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtBillWithTax_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtSoAmount1.Text = txtBillWithTax.Text;
            txtSoAmount.Text = txtBillWithTax.Text;
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
        } private void TxtSOCER_KeyUp(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSOCER.Text))
            {
                var PER = Convert.ToDecimal(txtSOCER.Text);
                var PERAmount = Convert.ToDecimal(txtSoAmount1.Text);
                txtSoAmountSOC.Text = (Math.Round(PER * PERAmount, 4)).ToString();
            }
            else
                txtSoAmountSOC.Text = txttotalcfr.Text;
        }

        private void TxtSoAmountSOC_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private void TxtSoAmountSOC_EditValueChanged_1(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSoAmountSOC.Text))
            {
                var PER = Convert.ToDouble(txtSoAmountSOC.Text);
                var PERAmount = Convert.ToDouble(txtSoAmount1.Text);
                var SocER = Math.Round(PER / PERAmount, 4);
                txtSOCER.Text = SocER.ToString();

                var SocER1 = Math.Round(PERAmount / PER, 4);
                txtSOCER1.Text = SocER1.ToString();
            }
        }
        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                grdAttach1.Visibility = Visibility.Visible;
                if (purchaseOrder.Id != 0)
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActivePOAttachmentCategories();

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
                if (purchaseOrder.Id != 0)
                {
                    List<TreeItem> otherAttachments = new List<TreeItem>();

                    if (purchaseOrder.saleOrder_Id != null)
                    {
                        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                        saleOrder = purchaseOrder.SaleOrder;
                        List<TreeItem> atachments = SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)saleOrder.Id, TransactionItemType.Sale_Order);
                        if (saleOrder.offer != null)
                        {
                            if (saleOrder.offer_Id != null)
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)saleOrder.offer_Id, TransactionItemType.Offer));
                            if (saleOrder.offer.inquiry_Id != null)
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)saleOrder.offer.inquiry_Id, TransactionItemType.Inquiry));
                        }

                        if (saleOrder.SaleInvoices.Count != 0)
                        {
                            foreach (var invoice in saleOrder.SaleInvoices)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)invoice.Id, TransactionItemType.Sale_Invoice));
                                if (invoice.salesReceipts.Count != 0)
                                    foreach (var receipt in invoice.salesReceipts)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)receipt.transactionGroupId, TransactionItemType.Sale_Receipt));
                                    }
                            }
                        }
                        if (saleOrder.PurchaseOrders.Count != 0)
                        {
                            foreach (var pO in saleOrder.PurchaseOrders)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)pO.Id, TransactionItemType.Purchase_Order));
                                if ( pO.PurchaseInvoices != null)
                                {
                                    if (pO.PurchaseInvoices.Count != 0 && pO.PurchaseInvoices != null)
                                        foreach (var pI in pO.PurchaseInvoices)
                                        {
                                            otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)pI.Id, TransactionItemType.Purchase_Invoice));
                                            if (pI.Payments.Count != 0)
                                                foreach (var payment in pI.Payments)
                                                {
                                                    otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                                }
                                        }
                                }
                                if (pO.Bills != null)
                                {
                                    if (pO.Bills.Count != 0)
                                        foreach (var bill in pO.Bills)
                                        {
                                            otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                                            if (bill.Payments.Count != 0)
                                                foreach (var payment in bill.Payments)
                                                {
                                                    otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                                }
                                        }
                                }

                            }
                        }
                        if (saleOrder.Bills.Count != 0)
                        {
                            foreach (var bill in saleOrder.Bills)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                                if (bill.Payments.Count != 0)
                                    foreach (var payment in bill.Payments)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
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
                    else
                    {
                        List<TreeItem> atachments = SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)purchaseOrder.Id, TransactionItemType.Purchase_Order);
                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                            foreach (var pI in purchaseOrder.PurchaseInvoices)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)pI.Id, TransactionItemType.Purchase_Invoice));
                                if (pI.Payments.Count != 0)
                                    foreach (var payment in pI.Payments)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                    }
                            }
                        if (purchaseOrder.Bills.Count != 0)
                            foreach (var bill in purchaseOrder.Bills)
                            {
                                otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                                if (bill.Payments.Count != 0)
                                    foreach (var payment in bill.Payments)
                                    {
                                        otherAttachments.AddRange(SYSTEM_STATIC.GetPOAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
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
                }
                grdAttachments1.Visibility = Visibility.Visible;

            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            UsersRepo UsersRepo = new UsersRepo();

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
                        destination += "Attachments\\PurchaseOrder\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += OrderId + "_" + TransactionItemType.Purchase_Order.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Purchase_Order);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Purchase_Order, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Purchase_Order);
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


                            //DXMessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        DXMessageBox.Show(ex.ToString());
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

        private void TxtSoAmount1_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSoAmountSOC.Text))
            {
                var PER = Convert.ToDouble(txtSoAmountSOC.Text);
                var PERAmount = Convert.ToDouble(txtSoAmount1.Text);
                var SocER = Math.Round(PER / PERAmount, 4);
                txtSOCER.Text = SocER.ToString();

                var SocER1 = Math.Round(PERAmount / PER, 4);
                txtSOCER1.Text = SocER1.ToString();
            }
        }

        private void TxtTaxAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            var amount = Convert.ToDouble(txtcfr.Text);
            var percentAmount =Convert.ToDouble(txtTaxAmount.Text);
             amount = amount + percentAmount;
            txtBillWithTax.Text = amount.ToString();
        }

        private void chkAdjustedTax_Checked(object sender, RoutedEventArgs e)
        {
            lookUpTax.IsEnabled = true;
            TaxRepo taxRepo = new TaxRepo();
            chkTax.IsChecked = false;
            lookUpTax.ItemsSource = taxRepo.getAllAdjustedTaxes();
        }

        private void chkAdjustedTax_Unchecked(object sender, RoutedEventArgs e)
        {
            lookUpTax.IsEnabled = false;
            txtBillWithTax.Text = txtcfr.Text;
            lookUpTax.SelectedIndex = -1;
            txtTaxAmount.Text = 0.ToString();
        }

        private void btnCostSheetPunching_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                ProcurementRepo repo = new ProcurementRepo();
                string systemCost = "";
                List<CostSheetPOField> costSheetPOValues = repo.GetSystemPOCosts(OrderId);
                foreach (CostSheetPOField field in costSheetPOValues)
                {
                    if (field.Value != 0)
                    {
                        systemCost += field.Field.Title + " " + " " + field.Value + "\n";
                    }

                }
                btnCostSheetPunching.ToolTip = systemCost;
            }
            catch (Exception)
            {
            }
        }
   
        private void lookupSelectVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LoadVendorProfile("Open");
            lblVendorProfileByPO.Text = "Vendor Profile By SO (Open)";
            btnVendorProfileByPO.Content = "Vendor Profile By SO (Open)";
        }
        private void btnSaveSOLayout_Click(object sender, EventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCustomerProfileBySO);

        }

        private void btnSavePOLayout_Click(object sender, EventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdVendorProfileByPO);

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
        private void btnOpenVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Open");
            lblVendorProfileByPO.Text = "Vendor Profile By PO (Open)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Open)";
        }

        private void btnCloseVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Close");
            lblVendorProfileByPO.Text = "Vendor Profile By PO (Close)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Close)";
        }

        private void btnPendingForApprovalVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("Pending for Approval");
            lblVendorProfileByPO.Text = "Vendor Profile By PO (Pending for Approval)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (Pending for Approval)";
        }

        private void btnAllVendorProfile_Click(object sender, EventArgs e)
        {
            LoadVendorProfile("All");
            lblVendorProfileByPO.Text = "Vendor Profile By PO (All)";
            btnVendorProfileByPO.Content = "Vendor Profile By PO (All)";
        }
        private void btnCustomerProfileBySo_Click(object sender, EventArgs e)
        {

        }
        private void btnVendorProfileByPO_Click(object sender, EventArgs e)
        {

        }

        private void grdCustomerProfileBySO_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdCustomerProfileBySO.SelectedItem != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, (grdCustomerProfileBySO.SelectedItem as PendingInvoice).Id);

                procurmentPanel.Show();

            }
        }

        private void grdVendorProfileByPO_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdVendorProfileByPO.SelectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (grdVendorProfileByPO.SelectedItem as PendingInvoice).Id);
                procurmentPanel.Show();
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
            if (!string.IsNullOrEmpty(type))
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
                                                                 .ToList(); allPendingInvoices = allPendingInvoices
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

        private void btnLinkBudget_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Link PO with Budget") != null)
            {
                if (purchaseOrder.Budget_Id != null)
                {
                    winSelectBudget budget = new winSelectBudget(company.Id, department.Id);
                    budget.budget = purchaseOrder.BudgetCostSheet;
                    budget.ShowDialog();
                    purchaseOrder.Budget_Id = budget.budgetId;
                }
                else
                {
                    winSelectBudget budget = new winSelectBudget(company.Id, department.Id);
                    budget.ShowDialog();
                    purchaseOrder.Budget_Id = budget.budgetId;
                }

            }
            else
            {
                DXMessageBox.Show("You don't have permission to Link PO with Budget");

            }
        }

        private void DatExcpectedPaymentDateChange_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var date = datExcpectedPaymentDateChange.DateTime;
            DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var timeSpan = currDate.Subtract(targetDate);

            
            txtPaymentDaysLeft.Text = timeSpan.Days.ToString();
        }
        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (purchaseOrder.Id != 0)
            {
                if (cmbTransactionHolder.SelectedItem != null)
                {

                    if (purchaseOrder.transactionHolderId != (cmbTransactionHolder.SelectedItem as cmbitem).id)
                    {
                        datHolderDate.EditValue = DateTime.Now;
                    }
                    else
                    {
                        datHolderDate.EditValue = purchaseOrder.holderChangeDate;
                    }
                }
            }
            else
            {
                datHolderDate.EditValue = DateTime.Now;
            }
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
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Purchase_Order);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Purchase_Order);
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

                if (frmInputBox.commentAdded == true && purchaseOrder.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    
                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (purchaseOrder.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                }
            }
            loadcomments();
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
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

                        var selectedStl = sTLRepo.Get(item.Id);
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
                        frmTargetRewards.rewardId = item.Id;
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
                //if (item.TransactionType == TransactionItemType.Inquiry.ToString())
                //{

                //    Procurementss.frmProcurmentPanel.childid = 0;
                //    Procurementss.Inquiriess.ucInquiryAdd.editinquiry = 1;
                //    Procurementss.frmProcurmentPanel.inquiryid = item.Id;

                //    Procurementss.Inquiriess.ucInquiryAdd.inquiryid = Procurementss.frmProcurmentPanel.inquiryid;
                //    Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel();
                //    procurmentPanele.Show();
                //}
                //else if (item.TransactionType == TransactionItemType.Offer.ToString())
                //{
                //    Procurementss.frmProcurmentPanel.childid = 1;

                //    Procurementss.Offerss.ucOfferAdd.editoffer = 1;
                //    Procurementss.frmProcurmentPanel.offerid = item.Id;

                //    Procurementss.Offerss.ucOfferAdd.offerid = Procurementss.frmProcurmentPanel.offerid;
                //    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel();
                //    procurmentPanel.Show();


                //}
                //else if (item.TransactionType == TransactionItemType.Sale_Order.ToString())
                //{
                //    Procurementss.frmProcurmentPanel.childid = 2;
                //    Procurementss.SaleOrderss.ucSaleOrderAdd.editsaleOrder = 1;
                //    Procurementss.frmProcurmentPanel.saleOrderid = item.Id;

                //    Procurementss.SaleOrderss.ucSaleOrderAdd.saleOrderid = Procurementss.frmProcurmentPanel.saleOrderid;
                //    Procurementss.frmProcurmentPanel procurmentPane = new Procurementss.frmProcurmentPanel();
                //    procurmentPane.Show();
                //}
                ////else if (item.TransactionType == TransactionItemType.Inquiry.ToString())
                ////{
                ////    Procurementss.frmProcurmentPanel.childid = 3;
                ////    Procurementss.MemorandumSaless.ucMemorandumSaleAdd.editmemorandumSale = 1;
                ////    Procurementss.frmProcurmentPanel.memorandumSaleid = item.Id;

                ////    Procurementss.MemorandumSaless.ucMemorandumSaleAdd.memorandumSaleid = Procurementss.frmProcurmentPanel.memorandumSaleid;
                ////    Procurementss.frmProcurmentPanel procurmentPan = new Procurementss.frmProcurmentPanel();
                ////    procurmentPan.Show();
                ////}
                //else if (item.TransactionType == TransactionItemType.Sale_Invoice.ToString())
                //{
                //    Procurementss.frmProcurmentPanel.childid = 4;
                //    Procurementss.SaleInvoicess.ucSaleInvoiceAdd.editsaleInvoice = 1;
                //    Procurementss.frmProcurmentPanel.saleInvoiceid = item.Id;
                //    Procurementss.frmProcurmentPanel.saleOrderid = 0;

                //    Procurementss.SaleInvoicess.ucSaleInvoiceAdd.saleInvoiceid = Procurementss.frmProcurmentPanel.saleInvoiceid;
                //    Procurementss.frmProcurmentPanel procurmentPane = new Procurementss.frmProcurmentPanel();
                //    procurmentPane.Show();
                //}

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
            }
            catch
            {

            }

            //MessageBox.Show("Mission Successful!");
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

        private void TxtBillAfterTax_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }
        public void loadPurchaseOrderStatusClass(int _statusId)
        {
            cmbPurchaseOrderStatusClass.ItemsSource = null;
            var status = purchaseOrderRepo.getstatus(_statusId);
            if (status.poStatusSubClasses.Count > 0)
            {
                StatusClasses.Clear();
                StatusClasses.AddRange(status.poStatusSubClasses.Where(x => x.isDisable != true).ToList());
                List<cmbitem> cmbitems = new List<cmbitem>();
                Parallel.ForEach(StatusClasses, delegate (ERP_BL.Procurements.StatusClass.StatusClass subClass)
                {
                    cmbitems.Add(new cmbitem() { name = subClass.ClassName, id = subClass.Id, bcolor = subClass.backcolor, fcolor = "#FF000000" });
                });
                cmbPurchaseOrderStatusClass.ItemsSource = cmbitems;
            }

        }
        public void loadPurchaseOrderStatusClass(ERP_BL.Procurements.StatusClass.StatusClass statusClass)
        {
            StatusClasses.Add(statusClass);
            if (StatusClasses.Count > 0)
            {
                List<cmbitem> cmbitems = new List<cmbitem>();
                Parallel.ForEach(StatusClasses, delegate (ERP_BL.Procurements.StatusClass.StatusClass subClass)
                {
                    cmbitems.Add(new cmbitem() { name = subClass.ClassName, id = subClass.Id, bcolor = subClass.backcolor, fcolor = "#FF000000" });
                });
                cmbPurchaseOrderStatusClass.ItemsSource = cmbitems;
            }
        }

        private void cmbPurchaseOrderStatusClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (purchaseOrder.Id != 0 && purchaseOrder.statusClass_Id != null && cmbPurchaseOrderStatusClass.SelectedIndex > -1)
            {
                checkStatusClass = purchaseOrderRepo.GetStatusClass((cmbPurchaseOrderStatusClass.SelectedItem as cmbitem).id);
            }
        }

        private void btnLinkedSO_Click(object sender, RoutedEventArgs e)
        {
            if (purchaseOrder.SaleOrder.Id != 0)
            {
                frmMergeMoudlesTracking trackingWindow = new frmMergeMoudlesTracking(purchaseOrder.SaleOrder.Id, TransactionItemType.Sale_Order, true);
                trackingWindow.ShowDialog();
            }
        }
    }
}
