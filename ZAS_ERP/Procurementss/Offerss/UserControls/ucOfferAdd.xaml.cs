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
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;
using DevExpress.Xpf.LayoutControl;
using DevExpress.Xpf.Core;
using DevExpress.XtraReports.UI;
using ZAS_ERP.Reportss;
using DevExpress.Xpf.Grid.Printing;
using DevExpress.Xpf.Grid;
using Microsoft.Win32;
using System.Diagnostics;
using ERP_BL.ToDoTasks.Taskss;
using ZAS_ERP.Procurementss.Offerss.UserControls;
using ERP_BL.Tax;
using DevExpress.DataAccess.Excel;
using System.Web.Hosting;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using ZAS_ERP.Procurementss.SaleOrderss;
using System.Printing;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ERP_BL.Procurements.AdminBills;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ERP_BL.Payments;
using ZAS_ERP.Bankings;
using ERP_BL.Procurements.InterBankTransfers;
using ZAS_ERP.Bankings.STL;
using ERP_BL.Procurements;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ERP_BL.ExchangeRates;
using DevExpress.XtraRichEdit;


//using System.Diagnostics;

namespace ZAS_ERP.Procurementss.Offerss
{


    /// <summary>
    /// Interaction logic for ucInquiryAdd.xaml
    /// </summary>
    public partial class ucOfferAdd : UserControl
    {
        public static int editoffer;
        public static int offerid;
        public int OrderId;
        public int editOrder;
        public static int inquiryid;
        EmployeeRepo cont1 = new EmployeeRepo();
        CompanyRepo cont = new CompanyRepo();
        VendorRepo vendorRepo = new VendorRepo();
        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
        Inquiry inquiry = new Inquiry();
        InquiryRepo inquiryRepo = new InquiryRepo();
        OfferRepo offerRepo = new OfferRepo();
        Offer offer = new Offer();
        Vendor vendor = new Vendor();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        CustomerCompany customer = new CustomerCompany();
        User user = new User();
        NotificationsRepo notificationsRepo = new NotificationsRepo();

        List<Product> inqueryItems = new List<Product>();
        List<ProcurementProduct> offerItems = new List<ProcurementProduct>();
        ProductRepo productrepo = new ProductRepo();
        Product product = new Product();
        Principal principal = new Principal();
        public bool isloading = false;
        List<ViewInfo> views = new List<ViewInfo>();
        public OfferStatus checkStatus = new OfferStatus();

        public virtual List<Product> products { get; set; }
        Bid bidbond = new Bid();
        Currency currency = new Currency();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        ProcurementProduct procurementPro = new ProcurementProduct();
        UsersRepo UsersRepo = new UsersRepo();
        public virtual List<ProcurementProduct> procurementProducts { get; set; }
        bool addinfo = true;
        ProcurementRepo procurementRepo = new ProcurementRepo();
        OfferStatus oldStatus = new OfferStatus();
        ERP_BL.Databases.Company InterCompany = new ERP_BL.Databases.Company();
        Department InterDepartment = new Department();
        List<Company> currentUserCompanies;
     
        List<CommentLog> comments = new List<CommentLog>();
        //List<BookerStatementItem> bookers = new List<BookerStatement>();
        public static int count;
        public static int Totalcount;
        bool isCount = false;
        bool isLoadComment = false;
        ProductRepo productRepo = new ProductRepo();
        TaxRepo taxRepo = new TaxRepo();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        public virtual List<BookerStatementItem> items { get; set; }
        Offer trackingOrder = new Offer();
        List<OfferStatus> OfferStatuses = new List<OfferStatus>();

        public ucOfferAdd()
        {
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
            InitializeComponent();
            //cmbcaption1.SelectedIndex = 0;
            //cmbcaption2.SelectedIndex = 1;
        }
        private void cmbPaymentTerm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (lookupPaymentTerms.SelectedItem != null)
            //{
            //    int idd = (cmbPaymentTerm.SelectedItem as cmbitem).id;
            //    if (idd == 0)
            //    {
            //        Termss.frmPaymentTermAdd paymentTerms = new Termss.frmPaymentTermAdd();
            //        paymentTerms.ShowDialog();
            //        loadPaymentTerms();

            //    }
            //}
        }
        public void loadBookerItemsSources()
        {
            items = new List<BookerStatementItem>();
            grdBokkerItems.ItemsSource = items;
            lookupBookerFOCSampling.ItemsSource = offerRepo.GetAllActiveFOCSamplings();
            lookupBookerClaimDiscount.ItemsSource = offerRepo.GetAllActiveClaimDiscounts();
            lookupBookerGST.ItemsSource = taxRepo.getAllTaxes();
            lookupPassOn.ItemsSource = offerRepo.GetAllPassOns();
        }
        private void winOfferadd_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //grdBookerItem.ItemsSource = bookers;
                OrderId = offerid;
                editOrder = editoffer;
                SystemLog.LogInfo(this.GetType(), "Form Loaded ");
                txttax.Text = "0";
                //List<datagriditem> datagriditems = new List<datagriditem>();
                //dGitems.ItemsSource = datagriditems;
                grdItems.ItemsSource = procurementProducts;
                //products = SYSTEM_STATIC.GetItemsForCurrentUser();

                //lookupProductsinGrid.ItemsSource = products;
                //LoadCreator();
                loadcompanies();
                //loadcustomers();
                //loademployees();
                loadinquirytypes();
                //loadvendors();
                loadCurrencies();
                loadOfferStatus();
                loadIncoterms();
                loadPaymentTerms();
                loadBookerItemsSources();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Offer") != null)
                {
                    datofferCreationdate.IsEnabled = true;
                }
                else
                {
                    datofferCreationdate.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Offer") != null)
                {
                    //btnAttachNew.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAttachNew.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Offer") != null)
                {
                    btnAttachmentList.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAttachmentList.Visibility = Visibility.Collapsed;
                }
                datofferCreationdate.EditValue = System.DateTime.Now;
                if (editoffer == 1 && offerid != 0)
                {
                    // if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Edit Offer") != null)
                    {
                        isloading = true;
                        offer = offerRepo.get(offerid);
                        loadonOfferdata();
                        GellAllOrdersTracking();
                        loadcomments();

                     
                    }
                }
                else if (editoffer == 0 && inquiryid != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Offer ") != null)
                    {
                        isloading = true;
                        inquiry = inquiryRepo.get(inquiryid);
                        loadonInquirydata();
                        cmbcaption1.SelectedIndex = 0;
                        cmbcaption2.SelectedIndex = 1;
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Add new Offer!");
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;
                    }
                }
                else
                {
                    
                    cmbcaption1.SelectedIndex = 0;
                    cmbcaption2.SelectedIndex = 1;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Offer") != null)
                {
                    btnSetVoid.Visibility = Visibility.Visible;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Move Offer to Inter Company") == null)
                {
                    grpInterCompany.IsEnabled = false;
                }
                if ((InquiryType)cmbOfferType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbOfferType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    CalCulateBooketItems();
                }
                else
                {
                    calculatetotal();
                }
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdItems);
                isloading = false;
                //Setting void stamp
                if (offer != null)
                {
                    if (offer.isVoid == true)
                    {
                        grdVoid.Visibility = Visibility.Visible;
                    }
                }
                LoadCreator();
                LoadCustomerCountry();
                LoadPendingInvoices();
            }
            catch (System.Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                MessageBox.Show(ex.ToString(), "Data Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            grdTrackingTree.ExpandAllNodes();
        }
       public void LoadCustomerCountry()
        {
            //if (editoffer == 0)
            //    txtCustomerCountry.Text = offer.customerCompany.billingAddres.Country + " " + offer.customerCompany.billingAddres.Country;
            //else if (offer.customerCompany.billingAddres != null)
            //    txtCustomerCountry.Text = offer.customerCompany.billingAddres.Country + " " + offer.customerCompany.billingAddres.Country;
        }
        public class datagriditem
        {

            public int itemId { get; set; }
            public int inquiryitemId { get; set; }
            public int offeritemId { get; set; }
            public string Item_Name { get; set; }
            public string Item_Discription { get; set; }
            public string Own_Description { get; set; }
            public string UOM { get; set; }
            public double Quantity { get; set; }
            public double value1 { get; set; }
            public double value2 { get; set; }

        }
        private void LoadCreator()
        {
            if (editoffer == 0)
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
            else if (offer.user_Id != null)
                txtCreator.Text = offer.user.employee.person.FName + " " + offer.user.employee.person.LName;
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
        public void loadvendors()
        {
            lookupVendor.ItemsSource = department.Vendors;
        }
        public void loadPaymentTerms()
        {
            PaymentTermRepo termRepo = new PaymentTermRepo();
            List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
            paymentTerms = termRepo.getAllForOffers();

            //List<cmbitem> cmbitems = new List<cmbitem>();
            //foreach (PaymentTerm paymentTerm in paymentTerms)
            //{
            //    cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
            //}
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            lookupPaymentTerms.ItemsSource = paymentTerms;
        }
        public void loadIncoterms()
        {
            loadCaptions();
            cmbIncoterm.ItemsSource = SYSTEM_STATIC.incoTermSource;
        }
        public void loadCaptions()
        {
            cmbcaption1.ItemsSource = SYSTEM_STATIC.incoTermSource;
            cmbcaption2.ItemsSource = SYSTEM_STATIC.incoTermSource;
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
                            if (offer.Id != 0)
                            {
                                var customer = department.disableCustomers.FirstOrDefault(x => x.Id == offer.customerCompany_Id);
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
        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (company.Id == 0)
            {
                MessageBox.Show("Select Company Frist");
                lookupCompany.Focus();
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
                    cmbOfferType.Items.Add(IND);
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
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x=>x.IsOfferType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (offer != null && offer.Id > 0 && editoffer == 1)
                        if (offer.department != null && departments.FirstOrDefault(x => x.Id == offer.dept_Id) == null)
                            departments.Add(offer.department);

                    lookupDepartment.ItemsSource = departments;
                    
                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }

        public void loadonInquirydata()
        {
            datInquirydate.DateTime = inquiry.inquiryDate;
            datduedate.DateTime = inquiry.DeliveryDueDate;
            datclosedate.DateTime = inquiry.lastSubmissionDate;
            txtSalesref.Text = inquiry.SalesReferenceNo;
            txtInquiryref.Text = inquiry.referenceNo;
            txtOwnDescription.Text = inquiry.OwnDescription;
            datofferCreationdate.EditValue = System.DateTime.Now;
            

            // Select Company
            if (inquiry.company_Id != null || inquiry.company != null)
            {
                company = inquiry.company;
                lookupCompany.Text = inquiry.company.CompanyName;
            }
            else
            {
                lookupCompany.Text = "Select Company";
            }
            // Select Department
            if (inquiry.dept_Id != 0 || inquiry.department != null)
            {
                lookupDepartment.Text = inquiry.department.DeptName;
                //lookupDepartment.SelectedItem = lookupDepartment.GetItemByKeyValue(inquiry.department);

                department = inquiry.department;
                lookupCustomer.ItemsSource = department.customers;
                loademployees();
            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }
            //Select Customer
            if (inquiry.customerCompany.Id != 0 || inquiry.customerCompany != null)
            {
                lookupCustomer.Text = inquiry.customerCompany.company.CompanyName;

                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(inquiry.customerCompany);

                customer = inquiry.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
           
            // Select Employee 
            if (inquiry.allocation_Id != 0 || inquiry.employee != null)
            {
                foreach (cmbitem cmbitem in cmbEmployee.Items)
                {
                    if (cmbitem.id == inquiry.allocation_Id)
                    {

                        cmbEmployee.SelectedItem = cmbitem;
                    }
                }


            }
            // select Inquiry Type Same for Offer Type
            foreach (String item in cmbOfferType.Items)
            {
                if (inquiry.inquirytype.ToString() == item.ToString())
                {
                    cmbOfferType.SelectedItem = item;
                    OfferTypeindex = inquiry.inquirytype;
                    break;
                }
            }
            // load Products in Inquiry to grid
            List<datagriditem> datagriditems = new List<datagriditem>();
            if (inquiry.products != null)
            {
                foreach (InquiryProduct inquiryProduct in inquiry.products)
                {
                    procurementProducts.Add(new ProcurementProduct()
                    {
                        inquiryProduct = new InquiryProduct()
                        {
                            Id = inquiryProduct.Id,
                            ownDiscription = inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                            UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                            quantity = inquiryProduct.quantity,
                            Weight = inquiryProduct.Weight,
                            product = new Product()
                            {
                                Id = inquiryProduct.product.Id,
                                categoryId = inquiryProduct.product.categoryId,
                                item = inquiryProduct.product.item,
                                code = inquiryProduct.product.code,
                                itemDescription = inquiryProduct.product.itemDescription,
                                ownDescription = inquiryProduct.product.ownDescription,
                                nature_Id = inquiryProduct.product.nature_Id,
                                unitOfMeasureId = inquiryProduct.product.unitOfMeasureId,
                                unitOfMeasure = inquiryProduct.product.unitOfMeasure,
                                nature = inquiryProduct.product.nature,
                                category = inquiryProduct.product.category,
                                isActive = inquiryProduct.product.isActive
                            }
                        },
                        product_Id = inquiryProduct.Id
                    });
                }
                grdItems.ItemsSource = procurementProducts;

            }
            


            foreach (cmbitem cmbitem in cmbCurrency.Items)
            {
                if (cmbitem.id == inquiry.customerCompany.company.CurrencyId)
                {
                    cmbCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                if (cmbitem.id == inquiry.company.CurrencyId)
                { cmbbaseCurrency.SelectedItem = cmbitem; break; }
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
        public void loadonOfferdata()
        {
            try
            {
                TaskRepo taskRepo = new TaskRepo();
                var task = taskRepo.GetOfferTask(offerid);

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

                if (offer.isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                //else if (saleInvoice.isReApproved == false)
                //{
                //    //lblStage.Text = "Under Re-Approval";
                //}
                else if (offer.isApproved == true && offer.stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (offer.isApproved == true && offer.offerStatus.isActive == false && offer.PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (offer.isApproved == true && offer.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (offer.isApproved == true)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (offer.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (offer.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                datofferCreationdate.EditValue = offer.CreationDate;
                datofferdate.EditValue = offer.OfferDate;
                datduedate.EditValue = offer.responseDate;
                datclosedate.EditValue = offer.closingDate;
                datInquirydate.EditValue = offer.InquiryDate;
                datValiditydate.EditValue = offer.offerValidityDate;
                datDeliverydate.EditValue = offer.deliveryDate;
                txtSalesref.Text = offer.SalesReferenceNo;
                txtInquiryref.Text = offer.InquiryReferenceNo;
                txtCommision.Text = offer.commision.ToString();
                txtMargin.Text = offer.margin.ToString();
                txtOfferRefNo.Text = offer.offerReferenceNo;
                txtOwnDescription.Text = offer.OwnDescription;
                txtComissionNumber.Text = offer.commisionRefrenceNo;
                //txtMargin.Text = offer.deliveryTime;
                txtexchangerate.Text = offer.exchngeRate.ToString();
                txttotalfob.Text = offer.totalFOBValue.ToString();
                txttotalcfr.Text = offer.totalCFRValue.ToString();
                txtBasetotalfob.Text = offer.totalBaseFOBValue.ToString();
                txtBasetotalcfr.Text = offer.totalBaseCFRValue.ToString();
                if (offer.isPercentTax == true)
                    txttax.Text = offer.salesTax.ToString() + "%";
                else
                    txttax.Text = offer.salesTax.ToString();
                txtMaker.Text = offer.maker;
                txtOrigin.Text = offer.origin;

                txtComments.Text = offer.comments;
                lblStage.Text = (offer.stage != null) ? offer.stage : "";
                // Select IncoTerm
                if (offer.incoterm_Id != 0 || offer.incoterm != null)
                {
                    foreach (cmbitem cmbitem in cmbIncoterm.Items)
                    {
                        if (cmbitem.id == offer.incoterm_Id)
                        {
                            cmbIncoterm.SelectedItem = cmbitem;
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
                if (offer.paymentterm_Id != 0 && offer.paymentTerm != null)
                {

                    var paymentTermSource = (List<PaymentTerm>)lookupPaymentTerms.ItemsSource;
                    var term = paymentTermSource.Find(x => x.Id == offer.paymentterm_Id);
                    if (term == null)
                    {
                        paymentTermSource.Add(offer.paymentTerm);
                        lookupPaymentTerms.ItemsSource = null;
                        lookupPaymentTerms.ItemsSource = paymentTermSource;
                    }
                    lookupPaymentTerms.Text = offer.paymentTerm.term;


                }
                if (offer.currency_Id != 0 && offer.currency != null)
                    foreach (cmbitem cmbitem in cmbCurrency.Items)
                    {
                        if (cmbitem.id == offer.currency_Id)
                            cmbCurrency.SelectedItem = cmbitem;
                    }
                else
                    foreach (cmbitem cmbitem in cmbCurrency.Items)
                    {
                        if (cmbitem.id == offer.company.CurrencyId)
                            cmbCurrency.SelectedItem = cmbitem;
                    }
                if (offer.inquiry_Id != null)
                    inquiry = offer.inquiry;

                // Select Company
                if (offer.company_Id != null || offer.company != null)
                {
                    company = offer.company;
                    lookupCompany.Text = offer.company.CompanyName;
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
                    lookupCustomer.ItemsSource = department.customers;
                    loademployees();

                }
                else
                {
                    lookupDepartment.Text = "Select Department";
                }
                if (offer.InterCompany_Id != null || offer.InterCompany != null)
                {
                    var companylist = (lookupInterCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupInterCompany.ItemsSource as List<Company>;
                    if (offer.InterCompany != null && companylist.Find(x => x.Id == offer.InterCompany_Id) == null)
                    {
                        companylist.Add(offer.InterCompany);
                        lookupInterCompany.ItemsSource = companylist;
                    }
                    InterCompany = offer.InterCompany;
                    lookupInterCompany.Text = offer.InterCompany?.CompanyName;
                }
                if (offer.InterDepartment_Id != null || offer.InterDepartment != null)
                {
                    var deptlist = (lookupInterDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupInterDepartment.ItemsSource as List<Department>;

                    if (offer.InterDepartment != null && deptlist.Find(x => x.Id == offer.InterDepartment_Id) == null)
                    {
                        deptlist.Add(offer.InterDepartment);
                        lookupInterDepartment.ItemsSource = deptlist;
                    }
                    lookupInterDepartment.Text = offer.InterDepartment?.DeptName;
                    InterDepartment = offer.InterDepartment;
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
                //Select Customer
                if (offer.principal_Id != null)
                    if (offer.principal_Id != 0 || offer.principal != null)
                    {
                        var principalList = (lookupPrincipal.ItemsSource as List<Principal>) == null ? new List<Principal>() : lookupPrincipal.ItemsSource as List<Principal>;
                        int index = 0;

                        foreach (var _principal in principalList)
                        {
                            if (_principal.Id == offer.principal_Id)
                            {
                                lookupPrincipal.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                        principal = offer.principal;
                    }
                    else
                    {
                        lookupCustomer.Text = "Select Customer";

                    }
                checkStatus = offer.offerStatus;
                //Select Customer

                // Select Employee 
                if (offer.allocation_Id != 0 || offer.employee != null)
                {
                    foreach (cmbitem cmbitem in cmbEmployee.Items)
                    {
                        if (cmbitem.id == offer.allocation_Id)
                        {

                            cmbEmployee.SelectedItem = cmbitem;
                        }
                    }


                }
                if (offer.bid_Id != null && offer.bid_Id != 0 && offer.bid != null)
                { //load form data for bid
                    //grpbondinfo.State = GroupBoxState.Normal;
                    datBidBondIssuedate.DateTime = offer.bid.issueDate;

                    txtBidBondRefno.Text = offer.bid.refNo;
                    txtBidBonValue.Text = offer.bid.value;
                    datBidBondSubmitdate.DateTime = offer.bid.submitDate;
                    datBidBondExpirydate.DateTime = offer.bid.expireDate;
                    txtIssuingbank.Text = offer.bid.bankName;
                }
                datBidOpendate.DateTime = (DateTime)((offer.bidOpenDate != null) ? offer.bidOpenDate : datBidOpendate.DateTime);
                // Delete Inquiry Type Same for Offer Type

                {
                    cmbOfferType.Text = offer.offertype.ToString();
                    OfferTypeindex = offer.offertype;

                }
                if (offer.offerStatus != null)
                {
                    var disAbleStatus = OfferStatuses.FirstOrDefault(x => x.Id == offer.offerStatus.Id);
                    if (disAbleStatus == null)
                    {
                        loadOfferStatus(offer.offerStatus);
                    }
                }
                // Select Offer Status 
                foreach (cmbitem cmbitem in cmbOfferStatus.Items)
                {
                    if (cmbitem.id == offer.offerStatus.Id)
                    {
                        cmbOfferStatus.SelectedItem = cmbitem;
                    }
                }
                if (offer.offerStatus.isActive == false && MainWindow.currentUserid != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed Offer") == null)
                {
                    grdBokkerItems.IsEnabled = false;
                    grdInquiryItems.IsEnabled = false;
                    grdOfferDetail.IsEnabled = false;
                    grdBasicInfo.IsEnabled = false;
                    cmbCurrency.IsEnabled = false;
                    btnAttachment.IsEnabled = false;

                    labeltopStatus.Visibility = Visibility.Visible;
                    labeltopStatus.Text = offer.offerStatus.Status;
                    System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(offer.offerStatus.backcolor);
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    labeltopStatus.Foreground = new SolidColorBrush(newColor);
                    var rotateAnimation = new DoubleAnimation(-30, -32, TimeSpan.FromSeconds(60));
                    var rt = (RotateTransform)labeltopStatus.RenderTransform;
                    rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

                }
                //int a = -1, b = -1;
                // load Products in Inquiry to grid
                //List<datagriditem> datagriditems = new List<datagriditem>();
                if (offer.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                    //grdItems.ItemsSource = offer.products;
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
                            unitPrice = procurementProduct.unitPrice,
                            product_Id = procurementProduct.inquiryProduct.Id,
                            value1 = procurementProduct.value1,
                            value2 = procurementProduct.value2,
                            caption1 = cmbcaption1.Text.Trim(),
                            caption2 = cmbcaption2.Text.Trim(),
                            priority=procurementProduct.priority


                        });;

                       
                    }
                  
                    grdItems.ItemsSource = procurementProducts;
                    
                }
                else
                {
                    grdItems.ItemsSource = procurementProducts;
                }

                grdItems.RefreshData();

                if(offer.BookerStatementItems!=null)
                {
                    grdBokkerItems.ItemsSource = offer.BookerStatementItems;
                }
                if(offer.CostSheet_Id!=null)
                {
                    txtBudgetMargin.Text = offer.CostSheet.TotalBudgetedMargin.ToString();
                }


                // selected currency of customer company
                foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
                {
                    if (cmbitem.id == offer.company.CurrencyId)
                        cmbbaseCurrency.SelectedItem = cmbitem;
                }
                if ((InquiryType)cmbOfferType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbOfferType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    CalCulateBooketItems();

                }
                else
                {
                    calculatetotal();

                }
                if(offer.uniqueNumber!=null)
                {
                    txtUniqueNumber.Text = offer.uniqueNumber;
                }
                if (offer.transactionHolderId != null)
                {
                    try
                    {
                        var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                        cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == offer.transactionHolderId))];

                    }
                    catch (System.Exception ex)
                    {

                    }
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }



        public List<ProcurementProduct> getProductsdata()
        {
            List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
            List<ProcurementProduct> offerItems = new List<ProcurementProduct>();
            ProcurementProduct product = new ProcurementProduct();
            procurementProducts = grdItems.ItemsSource as List<ProcurementProduct>;
            foreach (var procurementProduct in procurementProducts)
            {
                if (procurementProduct.Id == 0)
                {
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            offerItems.Add(new ProcurementProduct()
                            {
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, 
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

                            offerItems.Add(new ProcurementProduct()
                            {
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription,
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
                    // if user is adding completely new prodcut first time.
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            offerItems.Add(new ProcurementProduct()
                            {
                                Id = procurementProduct.Id,
                                inquiryProduct = new InquiryProduct()
                                {
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
                                        isActive = procurementProduct.inquiryProduct.product.isActive
                                    }
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
                        // if user reloaded he offer and its product item is already there.
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            offerItems.Add(new ProcurementProduct()
                            {
                                Id=procurementProduct.Id,
                                inquiryProduct = new InquiryProduct()
                                {
                                    Id = procurementProduct.inquiryProduct.Id,
                                    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                                    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                                    quantity = procurementProduct.inquiryProduct.quantity,
                                    Weight = procurementProduct.inquiryProduct.Weight,
                                    product_Id = procurementProduct.inquiryProduct.product.Id
                                },
                                unitPrice = procurementProduct.unitPrice,
                                product_Id = procurementProduct.inquiryProduct.Id,
                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                caption1 = cmbcaption1.Text.Trim(),
                                caption2 = cmbcaption2.Text.Trim(),
                                priority = procurementProduct.priority
                            });
                        }
                    }
                }
            }
            return offerItems;
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
                        item.offerId = offerid;
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
                        item.offerId = offerid;
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
                    item.offerId = offerid;
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
        private void btnOfferSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lookupVendor.SelectedIndex == -1 && vendor.Id == 0)
                {
                    lookupVendor.Focus();
                    MessageBox.Show("Please Select a vendor against Offer", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                if (lookupPrincipal.SelectedIndex == -1 && principal.Id == 0)
                {
                    lookupPrincipal.Focus();
                    MessageBox.Show("Please Select a principal against Offer", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                if (lookupCustomer.SelectedIndex == -1 && customer.Id == 0)
                {
                    MessageBox.Show("Please Select a Customer for whom Offer is being genrated", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCustomer.Focus();
                    return;
                }
                if (lookupCompany.SelectedIndex == -1 && company.Id == 0)
                {
                    MessageBox.Show("Please select a Refrence Company", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCompany.Focus();
                    return;
                }
                if (lookupDepartment.SelectedIndex == -1 && department.Id == 0)
                {
                    MessageBox.Show("Please Select a Refrence Department", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupDepartment.Focus();
                    return;
                }
                if (cmbEmployee.SelectedIndex == -1 && employee.EmpId == 0)
                {
                    MessageBox.Show("Please Select an Employee to whom this Offer will be Allocated ", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                if (cmbOfferStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Current Status of Offer to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbOfferStatus.Focus();
                    return;
                }
                if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Offer Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbCurrency.Focus();
                    return;
                }
                if ((InquiryType)cmbOfferType.SelectedIndex != InquiryType.DistributionBiz)
                {

                 if (Convert.ToDouble(txttotalcfr.Text) == 0 || 0 == Convert.ToDouble(txttotalfob.Text))
                    {
                        MessageBox.Show("Please Enter Amount for the Items", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                        grdItems.Focus();
                        return;
                    }
                }
                if (lookupPaymentTerms.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select PaymentTerm", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupPaymentTerms.Focus();
                    return;
                }
                if (cmbIncoterm.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Incoterm", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbIncoterm.Focus();
                    return;
                }
                if (cmbcaption1.SelectedIndex == -1 || cmbcaption2.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a caption for Items Values", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                    return;
                }
                if ((InquiryType)cmbOfferType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbOfferType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    if (!string.IsNullOrEmpty(txtUniqueNumber.Text))
                    {
                        offer.uniqueNumber = txtUniqueNumber.Text;
                    }
                    else
                    {
                        MessageBox.Show("Please Load Unique Number", "Required Unique Number", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                    offer.BookerStatementItems = getBookerStatementItems();
                }
                else
                {
                    offer.products = getProductsdata();
                    if (offer.products.Count == 0)
                    {
                        MessageBox.Show("Please Select items against which you want to create an Offer", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }
                }
                if (cmbTransactionHolder.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }
                DateTime? dateTime = null;
                offer.alertDate = (datduedate.Text == "") ? dateTime : datduedate.DateTime;
                offer.OfferDate = (datofferdate.Text == "") ? dateTime : datofferdate.DateTime;
                offer.CreationDate = (datofferCreationdate.Text == "") ? dateTime : datofferCreationdate.DateTime;
                offer.responseDate = (datduedate.Text == "") ? dateTime : datduedate.DateTime;
                offer.closingDate = (datclosedate.Text == "") ? dateTime : datclosedate.DateTime;
                offer.SalesReferenceNo = txtSalesref.Text.Trim();
                offer.InquiryReferenceNo = txtInquiryref.Text.Trim();
                offer.commisionRefrenceNo = txtComissionNumber.Text.Trim();
                offer.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
                offer.offertype = (InquiryType)cmbOfferType.SelectedIndex;
                offer.offerReferenceNo = txtOfferRefNo.Text.Trim();
                offer.InquiryDate = (datInquirydate.Text == "") ? dateTime : datInquirydate.DateTime;
                if (txtCommision.Text != "")
                    offer.commision = Convert.ToDecimal(txtCommision.Text.Trim());
                if (txtMargin.Text != "")
                    offer.margin = Convert.ToDecimal(txtMargin.Text.Trim());
                offer.OwnDescription = txtOwnDescription.Text.Trim();
                offer.bidOpenDate = (datBidOpendate.Text == "") ? dateTime : datBidOpendate.DateTime;
                offer.totalCFRValue = Convert.ToDouble(txttotalcfr.Text);
                offer.totalFOBValue = Convert.ToDouble(txttotalfob.Text);
                offer.totalBaseCFRValue = Convert.ToDouble(txtBasetotalcfr.Text);
                offer.totalBaseFOBValue = Convert.ToDouble(txtBasetotalfob.Text);
                offer.maker = txtMaker.Text.Trim();
                offer.origin = txtOrigin.Text.Trim();
                offer.offerValidityDate = (datValiditydate.Text == "") ? dateTime : datValiditydate.DateTime;
                offer.deliveryDate = (datDeliverydate.Text == "") ? dateTime : datDeliverydate.DateTime;
                offer.comments = txtComments.Text.Trim();
                offer.paymentterm_Id = (lookupPaymentTerms.SelectedItem as PaymentTerm).Id;
                offer.incoterm_Id = (cmbIncoterm.SelectedItem as cmbitem).id;
                offer.TitleValue1Id = (cmbcaption1.SelectedItem as cmbitem).id;
                offer.TitleValue2Id = (cmbcaption2.SelectedItem as cmbitem).id;
                offer.exchngeRate = (float)Convert.ToDecimal(txtexchangerate.Text.Trim());
                if (cmbTransactionHolder.SelectedIndex != -1)
                {
                    offer.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                    offer.holderChangeDate = (DateTime)datHolderDate.EditValue;
                }
                if (!string.IsNullOrEmpty(txtWeight.Text))
                {
                    offer.TotalWeight = Convert.ToDecimal(txtWeight.Text);
                }
                else
                {
                    offer.TotalWeight = null;
                }
                if (!string.IsNullOrEmpty(txtQuantity.Text))
                {
                    offer.TotalQuantity = Convert.ToDecimal(txtQuantity.Text);
                }
                else
                {
                    offer.TotalQuantity = null;
                }
                string str = txttax.Text.Trim();
                if (str.IndexOf("%") != -1)
                {
                    offer.isPercentTax = true;
                    offer.salesTax = Convert.ToDouble(str.Substring(0, str.IndexOf("%")));
                }
                else
                {
                    offer.salesTax = Convert.ToDouble(str);
                    offer.isPercentTax = false;
                }


                if (inquiry.Id != 0)
                {
                    offer.inquiry_Id = inquiry.Id;
                }
                else
                {
                    offer.inquiry_Id = null;
                }
                if ((cmbOfferStatus.SelectedItem as cmbitem) != null)
                {

                    OfferStatus status = offerRepo.getstatus((cmbOfferStatus.SelectedItem as cmbitem).id);
                    offer.offerStatus = status;
                }
                //Selected Offer Status
                if ((cmbOfferStatus.SelectedItem as cmbitem) != null)
                {

                    OfferStatus status = offerRepo.getstatus((cmbOfferStatus.SelectedItem as cmbitem).id);
                    offer.offerStatus = status;
                }

                // Selected Vendor 
                if (vendor != null)
                {

                    offer.vendors = new List<Vendor>();
                    offer.vendors.Add(offerRepo.getVendor(vendor.Id));
                }
                // selected Department
                if (department != null)
                {

                    offer.dept_Id = department.Id;
                }
                //selected Prinicipal
                if (principal != null)
                {

                    offer.principal_Id = principal.Id;
                }
                //selected customer
                if (customer != null)
                {

                    offer.customerCompany_Id = customer.Id;
                }
                // selected company
                if (company != null)
                {

                    offer.company_Id = company.Id;
                }
                // selected currency
                if (currency != null)
                {

                    offer.currency_Id = currency.Id;
                }
                if (currency != null)
                {

                    offer.costCenterCurrencyId = currency.Id;
                }
                if (!string.IsNullOrEmpty(txtSER.Text))
                {
                    offer.SER = Convert.ToDouble(txtSER.Text);

                }
                if (!string.IsNullOrEmpty(txtMER.Text))
                {
                    offer.MER = Convert.ToDouble(txtMER.Text);

                }
                if (!string.IsNullOrEmpty(txtTotalOfferAmount.Text))
                {
                    offer.totalOfferAmount=Convert.ToDouble(txtTotalOfferAmount.Text);
                }
                if (!string.IsNullOrEmpty(txtOfferAmountSER.Text))
                {
                    offer.totalOfferAmountSER = Convert.ToDouble(txtOfferAmountSER.Text);

                }
                if (!string.IsNullOrEmpty(txtOfferAmountMER.Text))
                {
                    offer.totalOfferAmountMER = Convert.ToDouble(txtOfferAmountMER.Text);

                }
               
                if (!string.IsNullOrEmpty(txtBudgetMargin.Text))
                {
                    offer.totalbudgetCost = Convert.ToDouble(txtBudgetMargin.Text);

                }
                if (!string.IsNullOrEmpty(txtBudgetMarginSER.Text))
                {
                    offer.budgetMarginSER = Convert.ToDouble(txtBudgetMarginSER.Text);

                }
                if (!string.IsNullOrEmpty(txtBudgetMarginMER.Text))
                {
                    offer.budgetMarginMER = Convert.ToDouble(txtBudgetMarginMER.Text);

                }
               

                if (chkInterCompany.IsChecked == true)
                {
                    if (InterCompany != null && InterCompany.Id != 0)
                    {

                        offer.InterCompany_Id = InterCompany.Id;
                    }
                    if (InterDepartment != null && InterDepartment.Id != 0)
                    {

                        offer.InterDepartment_Id = InterDepartment.Id;
                    }
                    offer.isInterCompany = true;

                }
                else
                {
                    offer.isInterCompany = false;
                    offer.InterCompany_Id = null;
                    offer.InterDepartment_Id = null;
                }
                var myWindow = Window.GetWindow(this);
                if (editOrder == 1 && OrderId != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Offer") != null)
                {
                    if (txtBidBondRefno.Text != "" && txtBidBonValue.Text != "" && txtIssuingbank.Text != "")
                    {
                        if (offer.bid_Id == null && offer.bid == null)
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
                            offer.bid = bid;
                            offer.bid_Id = bid.Id;
                        }
                        else
                        { //get form data for bid
                            offer.bid.issueDate = datBidBondIssuedate.DateTime;
                            offer.bid.refNo = txtBidBondRefno.Text.Trim();
                            offer.bid.value = txtBidBonValue.Text.Trim();
                            offer.bid.submitDate = datBidBondSubmitdate.DateTime;
                            offer.bid.expireDate = datBidBondExpirydate.DateTime;
                            offer.bid.bankName = txtIssuingbank.Text.Trim();
                        }
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null && offer.isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Offer is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            offer.stage = TransactionStage.Approved.ToString();

                            offer.isApproved = true;
                            offer.ApprovedDate = System.DateTime.Now;
                        }
                    }
                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        if (checkStatus.Id != offer.offerStatus.Id)
                        {
                            offer.LastStatusChangeDate = System.DateTime.Now;
                            if (offer.offerStatus.isActive != true)
                            {
                                offer.closingDate = System.DateTime.Now;
                            }
                        }
                    }


                    if (checkStatus.Id != offer.offerStatus.Id)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Offer has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), offer.Id, TransactionItemType.Offer);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                if (win.tagUsers.Count > 0)
                                {
                                    if (offer.transactionHolderId != win.tagUsers[0].employeeId)
                                    {
                                        offer.holderChangeDate = DateTime.Now;
                                    }
                                    offer.transactionHolderId = win.tagUsers[0].employeeId;

                                }
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }
                        }

                        string oldStat = checkStatus.Status;
                        string newStat = offer.offerStatus.Status;
                        string symbolCurr = "";
                        
                        if (offer.currency != null)
                        {
                            symbolCurr = offer.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of Offer having CFR: " + offer.totalCFRValue.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(offer.Id, TransactionItemType.Offer, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, 0,user.id, "New Comment ", null);
                            }
                        }

                    }
                    offerRepo.update(offer);

                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        UsersRepo.Add(TransactionInfo.Status_Changed, offer.Id, 2, "Status Changed from (" + checkStatus.Status + ") to (" + offer.offerStatus.Status + ")");
                    }
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Edited, offer.Id, 2, frmInputBox.comment);
                    DXMessageBox.Show("Offer Updated Succesfully", "Congratulations");
                    SystemLog.LogInfo(this.GetType(), "Offer Updated Succesfully refrence No= " + offer.offerReferenceNo + " Id=" + offer.Id);

                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Offer ") != null)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        MessageBox.Show("Please Create another Account to Create Offer, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                        return;
                    }
                    offer.user_Id = MainWindow.currentUserid;
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
                        offer.bid = bid;
                        offer.bid_Id = bid.Id;

                    }
                    else
                    {
                        offer.bid_Id = null;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer without Approval") != null)
                    {
                            offer.stage = TransactionStage.Approved.ToString();
                            offer.isApproved = true;
                            offer.ApprovedDate = System.DateTime.Now;
                    }
                    else
                    {
                        offer.stage = TransactionStage.AwaitingFirstReview.ToString();
                        offer.isApproved = false;
                    }
                    offerRepo.Add(offer);
                    Inquiriess.ucStatuschange.inquiry.ClosingDate = System.DateTime.Now;
                    Inquiriess.ucStatuschange.inquiry.LastStatusChangeDate = System.DateTime.Now;
                    Inquiriess.ucStatuschange.inquiryRepo.update(Inquiriess.ucStatuschange.inquiry);

                    UsersRepo.Add(TransactionInfo.Initialized, offer.Id, 2, "");

                    UsersRepo.Add(TransactionInfo.Closed, Inquiriess.ucStatuschange.inquiry.Id, 1, "Closed, Offer genrated on this inquiry");
                    SystemLog.LogInfo(this.GetType(), "Offer added Succesfully refrence No= " + offer.offerReferenceNo + " Id=" + offer.Id);

                    DXMessageBox.Show("Offer Added Succesfully", "Congratulations");
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                    myWindow.Close();
                    return;
                }
                myWindow.Close();
            }
            catch (System.Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
        private void loadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbbaseCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbCostCenterCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }
        public void loadOfferStatus()
        {
            OfferStatuses = offerRepo.getAllActiveStatus();
            OfferStatuses = OfferStatuses.Where(x => x.isDisable != true).ToList();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (OfferStatus status in OfferStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });
            cmbOfferStatus.ItemsSource = cmbitems;
        }
        public void loadOfferStatus(OfferStatus _status)
        {
            OfferStatuses.Add(_status);
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (OfferStatus status in OfferStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });
            cmbOfferStatus.ItemsSource = cmbitems;
        }

        private void cmbOfferStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbOfferStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbOfferStatus.SelectedItem as cmbitem).id;
                if (idd == 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Offer Status") != null)
                {
                    Procurementss.Offerss.frmOfferStatusAdd statusAdd = new Procurementss.Offerss.frmOfferStatusAdd();
                    statusAdd.ShowDialog();
                    loadOfferStatus();
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
                var customerCountry = customer.billingAddres.Country;
                txtCustomerCountry.Text = customerCountry;
            }
        }

        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company != null)
            {
                string selecteddept = company.CompanyName;
                foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
                {
                    if (cmbitem.id == company.CurrencyId)
                        cmbbaseCurrency.SelectedItem = cmbitem;
                }
                loaddepartments();
            }
        }
        

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupDepartment.SelectedIndex > -1)
            {
                DepartmentRepo deptRepo = new DepartmentRepo();
                department = deptRepo.GetDepartment( (lookupDepartment.SelectedItem as Department).Id);

                if (department != null)
                {
                    if (department.ParentID == null && department.subDepartments.Count != 0)
                    {
                        MessageBox.Show("Cannot map to a parent Department directly. Please select a diffrent department!");
                        lookupDepartment.Focus();
                        return;
                    }
                    string selecteddept = department.DeptName + " (" + department.Code + ")";
                    loadcustomers();
                    lookupVendor.ItemsSource = department.Vendors;
                    ProductRepo productRepo = new ProductRepo();
                    var products = productRepo.getAllDepartmentProducts(department.Id);
                    lookupProductsinGrid.ItemsSource = products;
                    lookupBookerProductsinGrid.ItemsSource = products;
                    loadPrincipals();
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
                var vendorCountry = vendor.billingAddres.Country;
                txtVendorCountry.Text = vendorCountry;
            }
        }
        string symbol;
        private void cmbCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
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
                    if (grdItems.Columns.Count != 0)
                    {
                        grdItems.Columns[6].Header = caption1 + symbol;
                        grdItems.Columns[7].Header = caption2 + symbol;
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

        private static InquiryType OfferTypeindex;
        private void cmbInquiryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if ((InquiryType)cmbOfferType.SelectedIndex == InquiryType.Supply)
            {
                grdInquiryItems.Visibility = Visibility.Visible;
                grdDitributionItems.Visibility = Visibility.Collapsed;
                txttotalClaimDisount.Visibility = Visibility.Collapsed;
                txttotalPassOn.Visibility = Visibility.Collapsed;
                txttotalNetAmount.Visibility = Visibility.Collapsed;
                txttotalFocSampling.Visibility = Visibility.Collapsed;
                lblClaimTotal.Visibility = Visibility.Collapsed;
                lblPassOn.Visibility = Visibility.Collapsed;
                lblFocSampling.Visibility = Visibility.Collapsed;
                lblNetAmount.Visibility = Visibility.Collapsed;
                btnImportData.Visibility = Visibility.Collapsed;
                lblUniqueNumber.Visibility = Visibility.Collapsed;
                txtUniqueNumber.Visibility = Visibility.Collapsed;
                btnCreateUniqueNumber.Visibility = Visibility.Collapsed;
                btnLoadUniqueNumber.Visibility = Visibility.Collapsed;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        lblMargin.Text = "Margin Amount " + symbol;
                        grpbondinfo.IsEnabled = false;
                        //grpbondinfo.State = GroupBoxState.Minimized;
                        lblBidOpendate.Text = "Quotation Opening Date";
                        lblMargin.Visibility = Visibility.Visible;
                        txtMargin.Visibility = Visibility.Visible;
                        OfferTypeindex = InquiryType.Supply;
                        txtCommision.Text = "0";
                        txtCommision.Visibility = Visibility.Collapsed;
                        lblCommision.Visibility = Visibility.Collapsed;
                        txtIssuingbank.Text = "";
                        txtBidBondRefno.Text = "";
                        txtBidBonValue.Text = "";
                    }
                    else
                        cmbOfferType.SelectedItem = OfferTypeindex;
                }
                else
                {
                    lblMargin.Text = "Margin Amount " + symbol;
                    grpbondinfo.IsEnabled = false;
                   // grpbondinfo.State = GroupBoxState.Minimized;
                    lblBidOpendate.Text = "Quotation Opening Date";
                    lblMargin.Visibility = Visibility.Visible;
                    txtMargin.Visibility = Visibility.Visible;
                    OfferTypeindex = InquiryType.Supply;
                    txtCommision.Text = "0";
                    txtCommision.Visibility = Visibility.Collapsed;
                    lblCommision.Visibility = Visibility.Collapsed;
                    txtIssuingbank.Text = "";
                    txtBidBondRefno.Text = "";

                    txtBidBonValue.Text = "";
                }
            }
            else if ((InquiryType)cmbOfferType.SelectedIndex == InquiryType.Tender)
            {
                grdInquiryItems.Visibility = Visibility.Visible;
                grdDitributionItems.Visibility = Visibility.Collapsed;
                txttotalClaimDisount.Visibility = Visibility.Collapsed;
                txttotalPassOn.Visibility = Visibility.Collapsed;
                txttotalNetAmount.Visibility = Visibility.Collapsed;
                txttotalFocSampling.Visibility = Visibility.Collapsed;
                lblClaimTotal.Visibility = Visibility.Collapsed;
                lblPassOn.Visibility = Visibility.Collapsed;
                lblFocSampling.Visibility = Visibility.Collapsed;
                lblNetAmount.Visibility = Visibility.Collapsed;
                btnImportData.Visibility = Visibility.Collapsed;
                lblUniqueNumber.Visibility = Visibility.Collapsed;
                txtUniqueNumber.Visibility = Visibility.Collapsed;
                btnCreateUniqueNumber.Visibility = Visibility.Collapsed;
                btnLoadUniqueNumber.Visibility = Visibility.Collapsed;
                if ((txtMargin.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        lblCommision.Text = "Commission Amount " + symbol;
                        txtCommision.Visibility = Visibility.Visible;
                        lblCommision.Visibility = Visibility.Visible;
                        grpbondinfo.IsEnabled = true;             
                        lblMargin.Visibility = Visibility.Hidden;
                        txtMargin.Visibility = Visibility.Hidden;
                        txtMargin.Text = "0";
                    }
                    else
                        cmbOfferType.SelectedItem = OfferTypeindex;
                }
                else
                {
                    lblCommision.Text = "Commission Amount " + symbol;
                    txtCommision.Visibility = Visibility.Visible;
                    lblCommision.Visibility = Visibility.Visible;
                    grpbondinfo.IsEnabled = true;
                    lblMargin.Visibility = Visibility.Hidden;
                    txtMargin.Visibility = Visibility.Hidden;
                    txtMargin.Text = "0";
                }
            }

            else if ((InquiryType)cmbOfferType.SelectedIndex == InquiryType.Principal)
            {
                grdInquiryItems.Visibility = Visibility.Visible;
                grdDitributionItems.Visibility = Visibility.Collapsed;
                txttotalClaimDisount.Visibility = Visibility.Collapsed;
                txttotalPassOn.Visibility = Visibility.Collapsed;
                txttotalNetAmount.Visibility = Visibility.Collapsed;
                txttotalFocSampling.Visibility = Visibility.Collapsed;
                lblClaimTotal.Visibility = Visibility.Collapsed;
                lblPassOn.Visibility = Visibility.Collapsed;
                lblFocSampling.Visibility = Visibility.Collapsed;
                lblNetAmount.Visibility = Visibility.Collapsed;
                btnImportData.Visibility = Visibility.Collapsed;
                lblUniqueNumber.Visibility = Visibility.Collapsed;
                txtUniqueNumber.Visibility = Visibility.Collapsed;
                btnCreateUniqueNumber.Visibility = Visibility.Collapsed;
                btnLoadUniqueNumber.Visibility = Visibility.Collapsed;
                if ((txtCommision.Text != "" || txtIssuingbank.Text != "" || txtBidBondRefno.Text != "" || txtBidBonValue.Text != "") && !isloading)
                {
                    if (DXMessageBox.Show("You have unsaved data that would be lost if you change the template. Do you wish to continue?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        lblCommision.Text = "Commission Amount " + symbol;
                        txtCommision.Visibility = Visibility.Visible;
                        lblCommision.Visibility = Visibility.Visible;
                        grpbondinfo.IsEnabled = true;
                        lblMargin.Visibility = Visibility.Hidden;
                        txtMargin.Visibility = Visibility.Hidden;
                        txtMargin.Text = "0";
                    }
                    else
                        cmbOfferType.SelectedItem = OfferTypeindex;

                }
                else
                {
                    lblCommision.Text = "Commission Amount " + symbol;

                    grpbondinfo.IsEnabled = true;
                    txtCommision.Visibility = Visibility.Visible;
                    lblCommision.Visibility = Visibility.Visible;
                    lblMargin.Visibility = Visibility.Hidden;
                    txtMargin.Visibility = Visibility.Hidden;
                    txtMargin.Text = "0";
                }

            }
            else
                if ((InquiryType)cmbOfferType.SelectedIndex == InquiryType.Inventory)
            {
                grdInquiryItems.Visibility = Visibility.Visible;
                grdDitributionItems.Visibility = Visibility.Collapsed;
                txttotalClaimDisount.Visibility = Visibility.Collapsed;
                txttotalPassOn.Visibility = Visibility.Collapsed;
                txttotalNetAmount.Visibility = Visibility.Collapsed;
                txttotalFocSampling.Visibility = Visibility.Collapsed;
                lblClaimTotal.Visibility = Visibility.Collapsed;
                lblPassOn.Visibility = Visibility.Collapsed;
                lblFocSampling.Visibility = Visibility.Collapsed;
                lblNetAmount.Visibility = Visibility.Collapsed;
                btnImportData.Visibility = Visibility.Collapsed;
                lblUniqueNumber.Visibility = Visibility.Collapsed;
                txtUniqueNumber.Visibility = Visibility.Collapsed;
                btnCreateUniqueNumber.Visibility = Visibility.Collapsed;
                btnLoadUniqueNumber.Visibility = Visibility.Collapsed;

            }
            else
                if((InquiryType)cmbOfferType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbOfferType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {
                grdDitributionItems.Visibility = Visibility.Visible;
                grdInquiryItems.Visibility = Visibility.Collapsed;
                txttotalClaimDisount.Visibility = Visibility.Visible;
                txttotalPassOn.Visibility = Visibility.Visible;
                txttotalNetAmount.Visibility = Visibility.Visible;
                txttotalFocSampling.Visibility = Visibility.Visible;
                lblClaimTotal.Visibility = Visibility.Visible;
                lblPassOn.Visibility = Visibility.Visible;
                lblFocSampling.Visibility = Visibility.Visible;
                lblNetAmount.Visibility = Visibility.Visible;
                btnImportData.Visibility = Visibility.Visible;
                lblUniqueNumber.Visibility = Visibility.Visible;
                txtUniqueNumber.Visibility = Visibility.Visible;
                btnCreateUniqueNumber.Visibility = Visibility.Visible;
                btnLoadUniqueNumber.Visibility = Visibility.Visible;
            }

        }
        private void txttax_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txttax.Text != "")
            {
                if ((InquiryType)cmbOfferType.SelectedIndex == InquiryType.DistributionBiz)
                {
                    CalCulateBooketItems();
                }
                else
                {
                    calculatetotal();
                }
            }
        }
        private void calculatetotal()
        {
            double sumfob = 0; double sumcfr = 0;
            decimal? weight = 0;
            double Quantity = 0;

            if (grdItems.ItemsSource != null)
                foreach (var item in grdItems.ItemsSource as List<ProcurementProduct>)
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
                txttotalfob.Text = (fob + tax).ToString();
                txttotalcfr.Text = (cfr + tax).ToString();
                txtBasetotalfob.Text = ((fob * exchangeRate) + tax).ToString();
                txtBasetotalcfr.Text = ((cfr * exchangeRate) + tax).ToString();
            }
        }

        private void dGitems_CurrentCellChanged(object sender, EventArgs e)
        {
            if ((InquiryType)cmbOfferType.SelectedIndex != InquiryType.DistributionBiz || (InquiryType)cmbOfferType.SelectedIndex != InquiryType.DistributionBiz_CustomerCredit)
            {
                calculatetotal();
            }
            else
            {

                CalCulateBooketItems();

            }
        }
        private void txttax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9|%]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cmbcaption1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdItems != null && grdItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdItems.Columns.GetColumnByFieldName("value1").Header = ((cmbitem)cmbcaption1.SelectedItem).name + symbol;

        }

        private void cmbcaption2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (grdItems != null && grdItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdItems.Columns.GetColumnByFieldName("value2").Header = ((cmbitem)cmbcaption2.SelectedItem).name + symbol;
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
            if ((InquiryType)cmbOfferType.SelectedIndex != InquiryType.DistributionBiz || (InquiryType)cmbOfferType.SelectedIndex != InquiryType.DistributionBiz_CustomerCredit)
            {
                (grdItems.CurrentItem as ProcurementProduct).inquiryProduct = new InquiryProduct();
            }
            else
            {
            }
        }

        private void PART_GridControl_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            (grdItems.CurrentItem as ProcurementProduct).inquiryProduct.product = sender as Product;
        }

        private void PART_GridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
        }
        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if ((InquiryType)cmbOfferType.SelectedIndex != InquiryType.DistributionBiz || (InquiryType) cmbOfferType.SelectedIndex != InquiryType.DistributionBiz_CustomerCredit)
            {
                calculatetotal();
            }
            else
            {
                CalCulateBooketItems();
            }
        }
        private void view_CustomCellAppearance(object sender, DevExpress.Xpf.Grid.CustomCellAppearanceEventArgs e)
        {

        }

        private void view_CellValueChanging(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void view_RowUpdated(object sender, DevExpress.Xpf.Grid.RowEventArgs e)
        {

        }

        private void WinOfferadd_Unloaded(object sender, RoutedEventArgs e)
        {
            inquiryid = 0;
            offerid = 0;
            editoffer = 0;
            if (offer != null && offer.Id != 0)
                UsersRepo.Add(TransactionInfo.viewed, offer.Id, 2, "Viewed details of offer");
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdItems);
        }

        private void Txtexchangerate_TextInput(object sender, TextCompositionEventArgs e)
        {
            if ((InquiryType)cmbOfferType.SelectedIndex != InquiryType.DistributionBiz || (InquiryType)cmbOfferType.SelectedIndex != InquiryType.DistributionBiz_CustomerCredit)
            {
                calculatetotal();
            }
            else
            {

                CalCulateBooketItems();

            }
        }

        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCurrency.SelectedItem != null)
            {
                MarketExchangeRate exchangeRate = new MarketExchangeRate();
                exchangeRate = currencyRepo.getMarketexchangerate(company.Id, (cmbCurrency.SelectedItem as cmbitem).id);
                if (exchangeRate != null)
                {
                    txtexchangerate.Text = exchangeRate.exchangerate.ToString();
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
                    if (grdItems.Columns.Count != 0)
                    {
                        grdItems.Columns.GetColumnByFieldName("value1").Header = caption1 + symbol;
                        grdItems.Columns.GetColumnByFieldName("value2").Header = caption2 + symbol;
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

            if (cmbCurrency.SelectedItem as cmbitem != null)
            {
                foreach (cmbitem cmbitem in cmbCostCenterCurrency.Items)
                {
                    if (cmbitem.id == (cmbCurrency.SelectedItem as cmbitem).id)
                    {
                        cmbCostCenterCurrency.SelectedItem = cmbitem;
                        break;
                    }
                }
            }
        }
        private void btnAddPrincipal_Click(object sender, RoutedEventArgs e)
        {

            Principalss.frmPrincipaladd Principaladd = new Principalss.frmPrincipaladd();
            Principaladd.ShowDialog();
            loadPrincipals();

        }
        private void lookupPrincipal_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            principal = lookupPrincipal.SelectedItem as Principal;
        }
        public void loadPrincipals()
        {

            PrincipalRepo principalRepo = new PrincipalRepo();
            List<Principal> principals = new List<Principal>();
            principals = principalRepo.getAllByDept(department.Id);
            lookupPrincipal.ItemsSource =principals;
        }



        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            if (offer.isApproved == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null)
                {
                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    offer.isReviewed = true;
                    offer.isApproved = true;
                    offer.stage = TransactionStage.Approved.ToString();
                    offerRepo.update(offer);

                    var res1 = MessageBox.Show("Offer has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (offer.department != null && offer.department.Id != 0 && offer.company?.Id != 0 && offer.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && offer.InterDepartment != null && offer.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), offer.Id, TransactionItemType.Offer);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (offer.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    offer.holderChangeDate = DateTime.Now;
                                }
                                offer.transactionHolderId = win.tagUsers[0].employeeId;
                                offerRepo.update(offer);
                            }
                            ccUsers = win.ccUsers;
                        }
                        else if (offer.department != null && offer.department.Id != 0 && offer.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), offer.Id, TransactionItemType.Offer);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (offer.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    offer.holderChangeDate = DateTime.Now;
                                }
                                inquiry.transactionHolderId = win.tagUsers[0].employeeId;
                                inquiryRepo.update(inquiry);
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
                    if (offer.currency != null)
                    {
                        symbolCurr = offer.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Offer (Amount OC) having value: " + offer.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been Approved ",
                        Timestamp = DateTime.Now,
                        Subject = "Offer Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(offer.Id, TransactionItemType.Offer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null))
                {
                    if (offer.isApproved == null)
                    {
                        offer.isApproved = false;
                    }
                    offer.isReviewed = true;
                    offer.needReview = false;
                    offer.stage = TransactionStage.AwaitingApproval.ToString();

                    offerRepo.update(offer);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null))
                {
                    if (offer.isApproved == null)
                    {
                        offer.isApproved = false;
                    }
                    offer.isReviewed = true;
                    offer.needReview = true;
                    offer.stage = TransactionStage.AwaitingSecondReview.ToString();

                    offerRepo.update(offer);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                }
            }
            else if (offer.isApproved == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null)
                {
                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();
                    offer.isApproved = false;
                    offer.stage = TransactionStage.AwaitingApproval.ToString();
                    offerRepo.update(offer);
                    var res1 = MessageBox.Show("Offer has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (offer.department != null && offer.department.Id != 0 && offer.company?.Id != 0 && offer.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && offer.InterDepartment != null && offer.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), offer.Id, TransactionItemType.Offer);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (offer.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    offer.holderChangeDate = DateTime.Now;
                                }
                                offer.transactionHolderId = win.tagUsers[0].employeeId;
                                offerRepo.update(offer);
                            }
                            ccUsers = win.ccUsers;
                        }
                        else if (offer.department != null && offer.department.Id != 0 && offer.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), offer.Id, TransactionItemType.Offer);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (offer.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    offer.holderChangeDate = DateTime.Now;
                                }
                                inquiry.transactionHolderId = win.tagUsers[0].employeeId;
                                inquiryRepo.update(inquiry);
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
                    if (offer.currency != null)
                    {
                        symbolCurr = offer.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Offer (Amount OC) having value: " + offer.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been UnApproved ",
                        Timestamp = DateTime.Now,
                        Subject = "Offer UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(offer.Id, TransactionItemType.Offer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to UnApprove the Offer!");
                    return;
                }
            }
            else if (offer.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null)
                {
                    offer.isReviewed = true;
                    offer.PendingForClosing = false;
                    offer.stage = TransactionStage.Closed.ToString();
                    offerRepo.update(offer);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Closing, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null))
                {
                    offer.isReviewed = true;
                    offer.needReview = false;
                    offer.stage = TransactionStage.AwaitingApproval.ToString();
                    offerRepo.update(offer);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null))
                {
                    offer.isReviewed = true;
                    offer.needReview = true;
                    offer.stage = TransactionStage.AwaitingSecondReview.ToString();

                    offerRepo.update(offer);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                }
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

            if (offer.isApproved != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Offer") != null)
                {
                    offer.isReviewed = true;
                    offer.isApproved = false;
                    offer.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                    offerRepo.update(offer);

                    notificationsRepo.Add("Offer Rejected", offer.Id, TransactionItemType.Offer, "Offer with refrence # " + offer.offerReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", offer.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Offer " + frmInputBox.comment, null);

                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null))
                {
                    offer.isReviewed = false;
                    offer.stage = TransactionStage.Rejected.ToString();
                    offer.isApproved = false;
                    offerRepo.update(offer);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                    offerRepo.update(offer);
                    notificationsRepo.Add("Offer Rejected", offer.Id, TransactionItemType.Offer, "Offer under review with refrence # " + offer.offerReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", offer.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Offer " + frmInputBox.comment, null);
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null))
                {
                    offer.isReviewed = false;
                    offer.stage = TransactionStage.Rejected.ToString();
                    if (offer.isApproved == null)
                    {
                        offer.isApproved = false;
                    }
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                    offerRepo.update(offer);

                    notificationsRepo.Add("Offer Rejected", offer.Id, TransactionItemType.Offer, "Offer under review with refrence # " + offer.offerReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", offer.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Offer " + frmInputBox.comment, null);

                }
            }
            else if (offer.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null)
                {
                    offer.isReviewed = true;
                    if (offer.PendingForClosing == null)
                    {
                        offer.PendingForClosing = true;
                    }
                    //offer.PendingForClosing = false;
                    offer.stage = TransactionStage.Rejected.ToString();
                    //offerRepo.update(offer);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                    offerRepo.update(offer);
                    notificationsRepo.Add("Offer Rejected", offer.Id, TransactionItemType.Offer, "Offer Pending for closing with refrence # " + offer.offerReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", offer.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Offer " + frmInputBox.comment, null);
                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null))
                {
                    offer.isReviewed = true;
                    offer.needReview = false;
                    offer.stage = TransactionStage.Rejected.ToString();
                    if (offer.PendingForClosing == null)
                    {
                        offer.PendingForClosing = true;
                    }
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                    offerRepo.update(offer);
                    notificationsRepo.Add("Offer Rejected", offer.Id, TransactionItemType.Offer, "Offer Pending for closing with refrence # " + offer.offerReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", offer.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Offer " + frmInputBox.comment, null);

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null))
                {
                    offer.isReviewed = true;
                    offer.needReview = true;
                    offer.stage = TransactionStage.Rejected.ToString();
                    if (offer.PendingForClosing == null)
                    {
                        offer.PendingForClosing = true;
                    }
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, offer.Id, 2, frmInputBox.comment);
                        addinfo = false;
                    }
                    offerRepo.update(offer);
                    notificationsRepo.Add("Offer Rejected", offer.Id, TransactionItemType.Offer, "Offer Pending for closing under review with refrence # " + offer.offerReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", offer.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this Offer " + frmInputBox.comment, null);

                }
            }
        }
        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                if(offer.Id!=0)
                {
                    loadcomments();
                }
                grdComments.Visibility = Visibility.Visible;
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Offer);
                inputBox.ShowDialog();
            }
            else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Offer);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (offer != null)
            {


                if (frmInputBox.comment != "" && offer.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(offer.Id, TransactionItemType.Offer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Offer #" + offer.offerReferenceNo, offer.Id, TransactionItemType.Offer, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Offer #" + offer.offerReferenceNo, offer.Id, TransactionItemType.Offer, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Offer #" + offer.offerReferenceNo, offer.Id, TransactionItemType.Offer, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Offer #" + offer.offerReferenceNo, offer.Id, TransactionItemType.Offer, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                            if (frmInputBox.Comment.TaggedList.Count > 0)
                            {
                                var transactionHolder = frmInputBox.Comment.TaggedList[0].employeeId;
                                var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                                cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == transactionHolder))];
                            }
                        }
                    }
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (offer.Id == 0)
                {
                    DXMessageBox.Show("Kindly save offer first to add a comment!");
                }
            }
            loadcomments();
        }
        public void loadcomments()
        {
            if (offer != null)
            {
                if (isLoadComment == false)
                {
                    isLoadComment = true;
                    comments = procurementRepo.getcommentslogAsc(offer.Id, TransactionItemType.Offer);
                    grdCommentss.ItemsSource = comments;
                    count = comments.Count - 1;

                    Totalcount = comments.Count - 1;

                    if (isCount == false)
                    {
                        if (count != -1)
                        {
                            isCount = true;

                            var comment = grdCommentss.GetCellValue(0, grdCommentss.Columns["Comment"]);
                            if (comment != null)
                                txtCommentss.Text = comments[count].Comment; 
                            var subject = grdCommentss.GetCellValue(0, grdCommentss.Columns["Subject"]);
                            if (subject != null)
                                txtSubject.Text = comments[count].Subject;

                            var Sender = grdCommentss.GetCellValue(0, grdCommentss.Columns["employee.person.FName"]);
                            if (Sender != null)
                                txtSenderName.Text = comments[count].employee.person.FName + "  " + comments[count].employee.person.LName;
                            var tag = string.Join(", ", comments[count].TaggedList.Select(x => x.userName));
                            if (tag != null)
                                txtTagged.Text = tag.ToString();

                            var Cc = string.Join(", ", comments[count].CCUsersList.Select(x => x.userName));
                            if (Cc != null)
                                txtCC.Text = Cc.ToString();

                            var Timestamp = grdCommentss.GetCellValue(0, grdCommentss.Columns["Timestamp"]);
                            if (Timestamp != null)
                                dteCommit.DateTime = (DateTime)Timestamp;
                        }
                    }
                }
                   
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
        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                if (offer.Id != 0)
                {
                    views = UsersRepo.getViwerInfo(offer.Id, 2);
                    grdUsers.ItemsSource = views;
                }
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach.Visibility == Visibility.Visible)
            {
                grdAttach.Visibility = Visibility.Collapsed;
            }
            else
            {
                if(offer.Id!=0)
                {
                    cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();

                }
                grdAttach.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
            {
                grdAttachments.Visibility = Visibility.Collapsed;
            }
            else
            {
                if(offer.Id!=0)
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(offer.Id, TransactionItemType.Offer);
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
            catch (System.Exception ex)
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
                        destination += "Attachments\\Offer\\ToUpload\\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) 
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += OrderId + "_" + TransactionItemType.Offer.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Offer);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                    
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Offer, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, offer.Id, 2, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {

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
                    catch (System.Exception ex)
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
        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (OrderId != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(OrderId, TransactionItemType.Offer);
                trackingWindow.ShowDialog();
            }
        }
        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (offer.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Offer") != null))
            {
                if (DXMessageBox.Show("This offer is currently in the list of Void Offers! Do you want to remove it from Void?", "Remove Void Offer", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    offer.isVoid = false;
                    offerRepo.setOffertoVoid(offer.Id, false);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Offer has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (offer.department != null && offer.department.Id != 0 && offer.company?.Id != 0 && offer.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && offer.InterDepartment != null && offer.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), offer.Id, TransactionItemType.Offer);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (offer.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    offer.holderChangeDate = DateTime.Now;
                                }
                                offer.transactionHolderId = win.tagUsers[0].employeeId;
                                offerRepo.update(offer);
                            }
                            ccUsers = win.ccUsers;
                        }
                        else if (offer.department != null && offer.department.Id != 0 && offer.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), offer.Id, TransactionItemType.Offer);
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
                    if (offer.currency != null)
                    {
                        symbolCurr = offer.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Offer (Amount OC) having value: " + offer.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Offer UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(offer.Id, TransactionItemType.Offer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, user.id, "New Comment ",null);
                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Offer") != null)
            {
                if (DXMessageBox.Show("This offer is not currently in the list of Void Offers! Do you want to move it to Void Offers?", "Mark as Void Offer", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    offer.isVoid = true;
                    offerRepo.setOffertoVoid(offer.Id, true);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Offer has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (offer.department != null && offer.department.Id != 0 && offer.company?.Id != 0 && offer.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && offer.InterDepartment != null && offer.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), offer.Id, TransactionItemType.Offer);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (offer.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    offer.holderChangeDate = DateTime.Now;
                                }
                                offer.transactionHolderId = win.tagUsers[0].employeeId;
                                offerRepo.update(offer);
                            }
                            ccUsers = win.ccUsers;
                        }
                        else if (offer.department != null && offer.department.Id != 0 && offer.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), offer.Id, TransactionItemType.Offer);
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
                    if (offer.currency != null)
                    {
                        symbolCurr = offer.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Offer (Amount OC) having value: " + offer.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Offer Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(offer.Id, TransactionItemType.Offer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, user.id, "New Comment ", null);
                        }
                    }
                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Offer #" + offer.SalesReferenceNo, offer.Id, TransactionItemType.Offer, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                }
            }
            loadcomments();

        }
        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (offer.Id != 0)
                {
                    if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Offer);
                        inputBox.ShowDialog();
                    }
                    else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.Offer);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (offer != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && offer.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(offer.Id, TransactionItemType.Offer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Offer #" + offer.offerReferenceNo, offer.Id, TransactionItemType.Offer, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Offer #" + offer.offerReferenceNo, offer.Id, TransactionItemType.Offer, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Offer #" + offer.offerReferenceNo, offer.Id, TransactionItemType.Offer, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Offer #" + offer.offerReferenceNo, offer.Id, TransactionItemType.Offer, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (offer.Id == 0)
                        {

                            DXMessageBox.Show("Kindly save Offer first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
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
                    foreach (Department dep in InterCompany.departments)
                        foreach (Department empdep in SYSTEM_STATIC.currentUser.employee.departments)
                            if (dep.Id == empdep.Id)
                            {
                                departments.Add(dep);
                            }
                    lookupInterDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        DXMessageBox.Show("This company dosen't contain any department mapped with the current User");
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
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null) ? true : false)
            {
                UsersRepo usersRepo = new UsersRepo();
                offer = offerRepo.get(offerid);
                if (offerid == 0)
                {
                    return;
                }
                Offerss.ucStatuschange.offerid = offerid;

                var row = offer;

                    if (row.offerStatus != null)
                    {
                        oldStatus = row.offerStatus;
                    }
                    Offerss.frmOfferStatusChange statusChange = new Offerss.frmOfferStatusChange();
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (Offerss.ucStatuschange.offer.Id != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Offer without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Offer") != null) ? true : false)
                        {
                            Offerss.ucStatuschange.offer.PendingForClosing = false;
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, Offerss.ucStatuschange.offer.Id, 2, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Offer") != null)
                        {
                            Offerss.ucStatuschange.offer.stage = TransactionStage.AwaitingApproval.ToString();
                            if (Offerss.ucStatuschange.offer.PendingForClosing != true)
                            {
                                Offerss.ucStatuschange.offer.PendingForClosing = true;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, Offerss.ucStatuschange.offer.Id, 2, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Offer") != null)
                        {
                            Offerss.ucStatuschange.offer.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (Offerss.ucStatuschange.offer.PendingForClosing != true)
                            {
                                Offerss.ucStatuschange.offer.PendingForClosing = true;
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, Offerss.ucStatuschange.offer.Id, 2, frmInputBox.comment);
                        }
                        else
                        {
                            Offerss.ucStatuschange.offer.stage = TransactionStage.AwaitingFirstReview.ToString();
                            Offerss.ucStatuschange.offer.PendingForClosing = true;
                        }
                    //Adding Signature
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Offer has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), offer.Id, TransactionItemType.Offer);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                        if (win.tagUsers.Count > 0)
                        {
                            if (offer.transactionHolderId != win.tagUsers[0].employeeId)
                            {
                                offer.holderChangeDate = DateTime.Now;
                            }
                            offer.transactionHolderId = win.tagUsers[0].employeeId;
                            offerRepo.update(offer);
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
                    string newStat = Offerss.ucStatuschange.offer.offerStatus.Status;
                    string symbolCurr = "";
                    if (row.currency != null)
                    {
                        symbolCurr = row.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of Offer(CFR) having value: " + row.totalCFRValue.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.Offer, comment, SYSTEM_STATIC.currentUser.employeeId);
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in Offer #" + row.SalesReferenceNo, row.Id, TransactionItemType.Offer, comment.Comment, user.id, "New Comment ",null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " Tagged you in Offer #" + row.SalesReferenceNo, row.Id, TransactionItemType.Offer, comment.Comment, 0,user.id, "New Comment ", null);
                        }
                    }
                Offerss.ucStatuschange.offer.user_Id = MainWindow.currentUserid;
                Offerss.ucStatuschange.offer.LastStatusChangeDate = System.DateTime.Now;
                Offerss.ucStatuschange.offer.closingDate = System.DateTime.Now;
                Offerss.ucStatuschange.offerRepo.updateFromGrid(Offerss.ucStatuschange.offer);
                    MessageBox.Show("Offer status changed to InActive (" + Offerss.ucStatuschange.offer.offerStatus.Status + ")");
                var thisWindow = Window.GetWindow(this);
                thisWindow.Close();
            }
            else
            {
                MessageBox.Show("You are not Allowed to Close Offer Directly.");
            }
        }
        private void BtnOldestComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (offer != null)
                {
                    if (comments.Count != 0)
                    {
                        int min = 0;
                        int max = Totalcount;
                        min = max - max;

                        if (count != 0)
                        {
                            count = 0;
                            if (min != -1)
                            {

                                var comment = grdCommentss.GetCellValue(0, grdCommentss.Columns["Comment"]);
                                if (comment != null)
                                    txtCommentss.Text = comments[min].Comment;
                                var subject = grdCommentss.GetCellValue(0, grdCommentss.Columns["Subject"]);
                                if (subject != null)
                                    txtSubject.Text = comments[min].Subject;

                                var Sender = grdCommentss.GetCellValue(0, grdCommentss.Columns["employee.person.FName"]);
                                if (Sender != null)
                                    txtSenderName.Text = comments[min].employee.person.FName + "  " + comments[min].employee.person.LName;
                                var tag = string.Join(", ", comments[min].TaggedList.Select(x => x.userName));
                                if (tag != null)
                                    txtTagged.Text = tag.ToString();

                                var Cc = string.Join(", ", comments[min].CCUsersList.Select(x => x.userName));
                                if (Cc != null)
                                    txtCC.Text = Cc.ToString();

                                var Timestamp = grdCommentss.GetCellValue(0, grdCommentss.Columns["Timestamp"]);
                                if (Timestamp != null)
                                    dteCommit.DateTime = (DateTime)Timestamp;
                            }
                        }
                        else
                        {
                            MessageBox.Show("This is Oldest Comment, Thanks");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnPreviousComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (offer != null)
                {
                    if (comments.Count != 0)
                    {
                        if (count != 0)
                        {
                            count = count - 1;

                            if (count != -1)
                            {

                                var comment = grdCommentss.GetCellValue(0, grdCommentss.Columns["Comment"]);
                                if (comment != null)
                                    txtCommentss.Text = comments[count].Comment;
                                var subject = grdCommentss.GetCellValue(0, grdCommentss.Columns["Subject"]);
                                if (subject != null)
                                    txtSubject.Text = comments[count].Subject;

                                var Sender = grdCommentss.GetCellValue(0, grdCommentss.Columns["employee.person.FName"]);
                                if (Sender != null)
                                    txtSenderName.Text = comments[count].employee.person.FName + "  " + comments[count].employee.person.LName;
                                var tag = string.Join(", ", comments[count].TaggedList.Select(x => x.userName));
                                if (tag != null)
                                    txtTagged.Text = tag.ToString();

                                var Cc = string.Join(", ", comments[count].CCUsersList.Select(x => x.userName));
                                if (Cc != null)
                                    txtCC.Text = Cc.ToString();

                                var Timestamp = grdCommentss.GetCellValue(0, grdCommentss.Columns["Timestamp"]);
                                if (Timestamp != null)
                                    dteCommit.DateTime = (DateTime)Timestamp;
                            }
                        }
                        else
                        {
                            MessageBox.Show("This is Oldest Comment, Thanks");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please add Comment first! Thanks");
                    }


                }
            }
            catch (System.Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnNextComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (offer != null)
                {
                    if (comments.Count != 0)
                    {
                        if (count < Totalcount)
                        {
                            count = count + 1;
                            if (count != -1)
                            {
                                var comment = grdCommentss.GetCellValue(0, grdCommentss.Columns["Comment"]);
                                if (comment != null)
                                    txtCommentss.Text = comments[count].Comment;
                                var subject = grdCommentss.GetCellValue(0, grdCommentss.Columns["Subject"]);
                                if (subject != null)
                                    txtSubject.Text = comments[count].Subject;

                                var Sender = grdCommentss.GetCellValue(0, grdCommentss.Columns["employee.person.FName"]);
                                if (Sender != null)
                                    txtSenderName.Text = comments[count].employee.person.FName + "  " + comments[count].employee.person.LName;
                                var tag = string.Join(", ", comments[count].TaggedList.Select(x => x.userName));
                                if (tag != null)
                                    txtTagged.Text = tag.ToString();

                                var Cc = string.Join(", ", comments[count].CCUsersList.Select(x => x.userName));
                                if (Cc != null)
                                    txtCC.Text = Cc.ToString();

                                var Timestamp = grdCommentss.GetCellValue(0, grdCommentss.Columns["Timestamp"]);
                                if (Timestamp != null)
                                    dteCommit.DateTime = (DateTime)Timestamp;
                            }
                        }
                        else
                        {
                            MessageBox.Show("This is the Latest Tag, Thanks");
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }
        private void BtnLatestComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (offer != null)
                {
                    if (comments.Count != 0)
                    {
                        int max = Totalcount;
                        max = Math.Max(max, Totalcount);
                        if (count < Totalcount)
                        {
                            if (max != -1)
                            {
                                count = Totalcount;
                                var comment = grdCommentss.GetCellValue(0, grdCommentss.Columns["Comment"]);
                                if (comment != null)
                                    txtCommentss.Text = comments[max].Comment;
                                var subject = grdCommentss.GetCellValue(0, grdCommentss.Columns["Subject"]);
                                if (subject != null)
                                    txtSubject.Text = comments[count].Subject;

                                var Sender = grdCommentss.GetCellValue(0, grdCommentss.Columns["employee.person.FName"]);
                                if (Sender != null)
                                    txtSenderName.Text = comments[max].employee.person.FName + "  " + comments[max].employee.person.LName;
                                var tag = string.Join(", ", comments[max].TaggedList.Select(x => x.userName));
                                if (tag != null)
                                    txtTagged.Text = tag.ToString();

                                var Cc = string.Join(", ", comments[max].CCUsersList.Select(x => x.userName));
                                if (Cc != null)
                                    txtCC.Text = Cc.ToString();

                                var Timestamp = grdCommentss.GetCellValue(0, grdCommentss.Columns["Timestamp"]);
                                if (Timestamp != null)
                                    dteCommit.DateTime = (DateTime)Timestamp;
                            }
                        }
                        else
                        {
                            MessageBox.Show("This is Latest Comment, Thanks");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
            {
                grdAttach1.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (offer.Id != 0)
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveOfferAttachmentCategories();

                }
                grdAttach1.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments1.Visibility == Visibility.Visible)
            {
                grdAttachments1.Visibility = Visibility.Collapsed;
            }
            else
            {
                List<TreeItem> atachments = SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)offer.Id, TransactionItemType.Offer);
                if (offer.Id != 0)
                {
                    List<TreeItem> otherAttachments = new List<TreeItem>();
                    SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                    if (offer.inquiry_Id != null)
                        otherAttachments.AddRange(SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)offer.inquiry_Id, TransactionItemType.Inquiry));
                    List<SaleOrder> saleOrders =saleOrderRepo.GellAllbyOfferId(offer.Id);
                    if(saleOrders.Count!=0)
                    {
                        foreach (var saleOrder in saleOrders)
                        {
                            otherAttachments.AddRange(SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)saleOrder.Id, TransactionItemType.Sale_Order));
                            if (saleOrder.SaleInvoices.Count != 0)
                            {
                                foreach (var invoice in saleOrder.SaleInvoices)
                                {
                                    otherAttachments.AddRange(SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)invoice.Id, TransactionItemType.Sale_Invoice));
                                    if (invoice.salesReceipts.Count != 0)
                                        foreach (var receipt in invoice.salesReceipts)
                                        {
                                            otherAttachments.AddRange(SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)receipt.transactionGroupId, TransactionItemType.Sale_Receipt));
                                        }
                                }
                            }
                            if (saleOrder.PurchaseOrders.Count != 0)
                            {
                                foreach (var pO in saleOrder.PurchaseOrders)
                                {
                                    otherAttachments.AddRange(SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)pO.Id, TransactionItemType.Purchase_Order));
                                    if (pO.PurchaseInvoices.Count != 0)
                                        foreach (var pI in pO.PurchaseInvoices)
                                        {
                                            otherAttachments.AddRange(SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)pI.Id, TransactionItemType.Purchase_Invoice));
                                            if (pI.Payments.Count != 0)
                                                foreach (var payment in pI.Payments)
                                                {
                                                    otherAttachments.AddRange(SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                                }
                                        }
                                }
                            }
                            if (saleOrder.Bills.Count != 0)
                            {
                                foreach (var bill in saleOrder.Bills)
                                {
                                    otherAttachments.AddRange(SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                                    if (bill.Payments.Count != 0)
                                        foreach (var payment in bill.Payments)
                                        {
                                            otherAttachments.AddRange(SYSTEM_STATIC.GetOfferAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                        }
                                }
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
                }
                treeViewAttachments1.ItemsSource = atachments;
                grdAttachments1.Visibility = Visibility.Visible;
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
                        destination += "Attachments\\Offer\\ToUpload\\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += OrderId + "_" + TransactionItemType.Offer.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Offer);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Offer, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, offer.Id, 2, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
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
                    catch (System.Exception ex)
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
        private void btnComparativeStatement_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Comparative Statement") != null)
            {
                frmComparativeStatement statement = new frmComparativeStatement(offerid);
                statement.ShowDialog();
                if (frmComparativeStatement.comparativeStatement != null && frmComparativeStatement.comparativeStatement.comparativeStatementItems != null)
                {
                    if (frmComparativeStatement.comparativeStatement.comparativeStatementItems.Count != 0)
                    {
                        offer.comparativeStatements.Add(frmComparativeStatement.comparativeStatement);
                    }
                }
            }
            else
            {
                DXMessageBox.Show("You are not allowed to add vendor comparative statement.", "Information", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void btnComparativeStatements_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Comparative Statement") != null)
            {
                winComparativeStatements statement = new winComparativeStatements(offerid);
                statement.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("You are not allowed to view vendor comparative statement.", "Information", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void btnAddBookerStatement_Click(object sender, RoutedEventArgs e)
        {
        }

        private void grdBookerItem_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
        }

        private void view1_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            var row = e.Row as BookerStatementItem;
            row.amount = row.unit * row.quantity;
            row.netAmount = row.amount + row.amountGST - row.passOnValue - row.claimDiscountValue - row.focValue;
            CalCulateBooketItems();

        }
        public void CalCulateBooketItems()
        {

            double sumNetAmountfob = 0;
            double sumNetAmountCFR = 0;
            double sumGst = 0;
            double? weight = 0;
            double Quantity = 0;
            double totalClaimDisount = 0;
            double totalPassOn = 0;
            double totalFocSampling = 0;

            double totalNetAmount = 0;


            if (grdBokkerItems.ItemsSource != null)
                foreach (var item in grdBokkerItems.ItemsSource as List<BookerStatementItem>)
                {

                    if (item.quantity != 0)
                    {
                        weight += (Convert.ToDouble(item.weight) != null ? Convert.ToDouble(item.weight) : 0) * Convert.ToDouble(item.quantity);
                    }
                    else
                    {
                        weight += Convert.ToDouble(item.weight) != null ? Convert.ToDouble(item.weight) : 0;
                    }
                    Quantity += item.quantity;
                    sumNetAmountfob += item.amount;
                    sumNetAmountCFR += item.amount;
                    sumGst += item.amountGST;
                    totalPassOn += item.passOnValue;
                    totalClaimDisount += item.claimDiscountValue;
                    totalFocSampling += item.focValue;
                    totalNetAmount += item.netAmount;
                }
            txttotalClaimDisount.Text = totalClaimDisount.ToString();
            txttotalPassOn.Text = totalPassOn.ToString();
            txttotalNetAmount.Text = totalNetAmount.ToString();
            txttotalFocSampling.Text = totalFocSampling.ToString();
            txtWeight.Text = weight.ToString();
            txtQuantity.Text = Quantity.ToString();
            txtfob.Text = sumNetAmountfob.ToString();
            txtcfr.Text = sumNetAmountCFR.ToString();
            txttax.Text = sumGst.ToString();
            string str = txttax.Text.ToString();
            decimal fob, cfr, tax = 0;
            fob = Convert.ToDecimal(txtfob.Text);
            cfr = Convert.ToDecimal(txtcfr.Text);
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
                txttotalfob.Text = (fob + tax).ToString();
                txttotalcfr.Text = (cfr + tax).ToString();

                txtBasetotalfob.Text = ((fob * exchangeRate) + tax).ToString();
                txtBasetotalcfr.Text = ((cfr * exchangeRate) + tax).ToString();

            }

        }

        private void LookupPaymentTerms_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var paymentTermSource = (List<PaymentTerm>)lookupPaymentTerms.ItemsSource;
                var term = paymentTermSource.Find(x=>x.ParentId == (lookupPaymentTerms.SelectedItem as PaymentTerm).Id);
            if(term != null)
            {
                DXMessageBox.Show("Parent Term Cannot be Selected!");
                lookupPaymentTerms.SelectedItem = null;
            }
            
        }
        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        private void btnImportData_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            OpenFileDialog fileDialog = new OpenFileDialog();
            ProductRepo productRepo  = new ProductRepo();
            TaxRepo taxRepo = new TaxRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Import Offer Items") != null)
            {
                try
                {
                    if (MainWindow.currentUserid != 0)
                    {
                        if (lookupCompany.SelectedIndex == -1)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Mouse.OverrideCursor = Cursors.Arrow;
                            });
                            DevExpress.Xpf.Core.DXMessageBox.Show("Please select a company.", "Select Company", MessageBoxButton.OK, MessageBoxImage.Information);
                            lookupCompany.Focus();
                            return;
                        }
                        else
                        if (lookupDepartment.SelectedIndex == -1)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Mouse.OverrideCursor = Cursors.Arrow;
                            });
                            DevExpress.Xpf.Core.DXMessageBox.Show("Please select a department.", "Select Department", MessageBoxButton.OK, MessageBoxImage.Information);
                            lookupDepartment.Focus();
                            return;
                        }
                        else
                        if (lookupCustomer.SelectedIndex == -1)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Mouse.OverrideCursor = Cursors.Arrow;
                            });
                            DevExpress.Xpf.Core.DXMessageBox.Show("Please select a customer.", "Select Customer", MessageBoxButton.OK, MessageBoxImage.Information);
                            lookupDepartment.Focus();
                            return;
                        }
                        else
                        if (cmbEmployee.SelectedIndex == -1)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Mouse.OverrideCursor = Cursors.Arrow;
                            });
                            DevExpress.Xpf.Core.DXMessageBox.Show("Please select allocated to.", "Select Employee", MessageBoxButton.OK, MessageBoxImage.Information);
                            cmbEmployee.Focus();
                            return;
                        }
                        else
                        {
                            fileDialog.Multiselect = false;
                            Excel.Application xlApp;
                            Excel.Workbook workbook;
                            Excel.Worksheet sheet;
                            Excel.Range range;
                           
                            int rCnt;
                            int cCnt;
                            int rw = 0;
                            int cl = 0;
                            if (fileDialog.ShowDialog() == true)
                            {
                                var fileName = System.IO.Path.GetFileName(fileDialog.FileName);
                                var filePath = System.IO.Path.GetFullPath(fileDialog.FileName);
                                xlApp = new Excel.Application();
                                workbook = xlApp.Workbooks.Open(filePath, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                                sheet = (Excel.Worksheet)workbook.Worksheets.get_Item(1);
                                range = sheet.UsedRange;
                                rw = range.Rows.Count;
                                cl = range.Columns.Count;
                                bool checkBreak = false;
                                List<BookerStatementItem> bookerDetails = new List<BookerStatementItem>();
                                for (rCnt = 2; rCnt <= rw; rCnt++)
                                {
                                    BookerStatementItem data = new BookerStatementItem();
                                    for (cCnt = 1; cCnt <= cl; cCnt++)
                                    {
                                        var ran = (range.Cells[rCnt, cCnt] as Excel.Range);
                                        var str = ran.Value2;
                                        Product product = new Product();
                                        double unitProce = 0,quantity=0,weight=0,amount=0,gstAmount=0,passOnValue=0,discountValue=0,focValue=0,NetAmount=0;
                                        TaxName gst = new TaxName();
                                        PassOn passOn = new PassOn();
                                        ClaimDiscount discount = new ClaimDiscount();
                                        FOCSampling focSampling = new FOCSampling();
                                        if (str != null)
                                        {
                                            switch (cCnt)
                                            {
                                                case 1:
                                                    product = productRepo.getProductByCode(str.ToString());
                                                    if (product != null)
                                                    {
                                                        data.product_Id = product.Id;
                                                        data.product = product;
                                                    }
                                                    if (product == null)
                                                    {
                                                        checkBreak = true;
                                                    }
                                                    break;
                                                case 2:
                                                    unitProce = Convert.ToDouble(str);
                                                    data.unit = unitProce;
                                                    break;
                                                case 3:
                                                    quantity = Convert.ToDouble(str);
                                                    data.quantity = quantity;
                                                    break;
                                                case 4:
                                                    weight = Convert.ToDouble(str);
                                                    data.weight = weight;
                                                    break;
                                                case 5:
                                                    amount = Convert.ToDouble(str);
                                                    data.amount = amount;
                                                    break;
                                                case 6:
                                                    gst = taxRepo.getTaxtByName(str);
                                                    if (gst != null)
                                                    {
                                                        data.taxNameId = gst.Id;
                                                        data.TaxName = gst;
                                                    }
                                                    break;
                                                case 7:
                                                    gstAmount = Convert.ToDouble(str);
                                                    data.amountGST = gstAmount;
                                                    break;
                                                case 8:
                                                    passOn = offerRepo.GetPassOnByName(str);
                                                    if (passOn != null)
                                                    {
                                                        data.passOnId = passOn.Id;
                                                        data.PassOn = passOn;
                                                    }
                                                    break;
                                                case 9:
                                                    passOnValue = Convert.ToDouble(str);
                                                    data.passOnValue = passOnValue;
                                                    break;
                                                case 10:
                                                    discount = offerRepo.GetClaimDiscountByName(str);
                                                    if (discount != null)
                                                    {
                                                        data.claimDiscountId = discount.Id;
                                                        data.ClaimDiscount = discount;
                                                    }
                                                    break;
                                                case 11:
                                                    discountValue = Convert.ToDouble(str);
                                                    data.claimDiscountValue = discountValue;
                                                    break;
                                                case 12:
                                                    focSampling = offerRepo.GetFOCSamplingByName(str);
                                                    if (focSampling != null)
                                                    {
                                                        data.focSamplingId = focSampling.Id;
                                                        data.FOCSampling = focSampling;
                                                    }
                                                    break;
                                                case 13:
                                                    focValue = Convert.ToDouble(str);
                                                    data.focValue = focValue;
                                                    break;
                                                case 14:
                                                    NetAmount = Convert.ToDouble(str);
                                                    data.netAmount = NetAmount;
                                                    break;

                                            }
                                        }
                                        else
                                        {                                                
                                        }
                                        if (checkBreak == true)
                                        {
                                            break;
                                        }
                                    }
                                    if (checkBreak == true)
                                    {
                                        break;
                                    }
                                    bookerDetails.Add(data);
                                }
                                grdBokkerItems.ItemsSource = bookerDetails;
                                CalCulateBooketItems();
                                workbook.Close(true, null, null);
                                xlApp.Quit();
                                Marshal.ReleaseComObject(sheet);
                                Marshal.ReleaseComObject(workbook);
                                Marshal.ReleaseComObject(xlApp);
                            }
                        }
                    }
                }
                catch(System.Exception ex)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Mouse.OverrideCursor = Cursors.Arrow;
                    });
                    DXMessageBox.Show(ex.Message);

                }
            }
            else
            {
                DXMessageBox.Show("You are not allowed to Import Offer Items");
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        public class Student
        {
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public string Address { get; set; }
            public string MobileNo { get; set; }
        }

        public class ExcelData
        {

            public string Code { get; set; }
            public double UnitPrice { get; set; }
            public double Quantity { get; set; }
            public double Weight { get; set; }
            public double Amount { get; set; }
            public double GSTAmount { get; set; }
            public double PassOn { get; set; }

            public double Discount { get; set; }
            public double Sampling { get; set; }
            public double NetAmount { get; set; }


        }

        private void btnLoadUniqueNumber_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Unique Number Lists") != null)
            {

                frmUniqueNumberList frmUniqueNumberList = new frmUniqueNumberList();
                frmUniqueNumberList.ShowDialog();
                var uniqueNumber = frmUniqueNumberList.uniqueNumber;
                if (uniqueNumber != null)
                    txtUniqueNumber.Text = uniqueNumber.UniqueName;
            }
            else
            {
                DXMessageBox.Show("You are not allowed to View Unique Number Lists");

            }

        }

        private void btnCreateUniqueNumber_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Unique Number") != null)
            {
                frmUniqueNumber frmUniqueNumber = new frmUniqueNumber();
                frmUniqueNumber.Show();
            }
            else
            {
                DXMessageBox.Show("You are not allowed to Add Unique Number");

            }
        }
        public void LoadPendingInvoices()
        {
            try
            {
                LoadCustomerProfile("Open");
                SlblCustomerProfileBySO.Text = "Customer Profile By SO (Open)";
                btnCustomerProfileBySo.Content = "Customer Profile By SO (Open)";
            }
            catch (System.Exception)
            {
            }
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
        public void LoadCustomerProfile(string _type)
        {
            var type = _type;
            if (!string.IsNullOrEmpty(type))
            {
                switch (type)
                {
                    case "Open":
                        {
                            if (lookupCustomer.SelectedIndex > -1 )
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
                                    grdCustomerProfileBySO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "Close":
                        {
                            if (lookupCustomer.SelectedIndex > -1 )
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
                                    grdCustomerProfileBySO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "All":
                        {
                            if (lookupCustomer.SelectedIndex > -1 )
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
                                    grdCustomerProfileBySO.ItemsSource = allPendingInvoices;
                                }
                            }
                            break;
                        }
                    case "Pending for Approval":
                        {
                            if (lookupCustomer.SelectedIndex > -1 )
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

        private void grdCustomerProfileBySO_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdCustomerProfileBySO.SelectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, (grdCustomerProfileBySO.SelectedItem as PendingInvoice).Id);
                procurmentPanel.Show();

            }
        }
        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (offer.Id != 0)
            {
                if (cmbTransactionHolder.SelectedItem != null)
                {

                    if (offer.transactionHolderId != (cmbTransactionHolder.SelectedItem as cmbitem).id)
                    {
                        datHolderDate.EditValue = DateTime.Now;
                    }
                    else
                    {
                        datHolderDate.EditValue = offer.holderChangeDate;
                    }
                }
            }
            else
            {
                datHolderDate.EditValue = DateTime.Now;
            }
        }
        private void btnCustomerProfileBySo_Click(object sender, EventArgs e)
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
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Offer);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Offer);
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

                if (frmInputBox.commentAdded == true && offer.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    
                    //procurementRepo.Add(purchaseInvoice.Id, TransactionItemType.Offer, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (offer.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Offer first to add a comment!");
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

                var item = (AllOrdersView)grid.SelectedItem;

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
                    catch (System.Exception ex)
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
                                    
                                        frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                     
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
                                  
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                 
                                 
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
                                 
                                        ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                        
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
                                  
                                    ucFrmBillPayment.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                        
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
                                
                                        frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                  
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
                                  
                                    frmPInvoicePaymentAdd.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                   
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
            trackingOrder = offerRepo.get(offer.Id);
            if (trackingOrder != null)
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.Offer);
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

        private void BtnCostSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                frmCostSheet frmCostSheet = new frmCostSheet(offer);
                if (offer.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Cost Center after Offer Approval") == null)
                {
                    frmCostSheet.checkHideActualCost.IsEnabled = false;
                    frmCostSheet.checkHideRevisedCost.IsEnabled = false;
                    frmCostSheet.checkPOTerms.IsEnabled = false;
                    frmCostSheet.grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    frmCostSheet.grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                }
                frmCostSheet.ShowDialog();
                if (frmCostSheet.costSheet != null)
                {
                    frmCostSheet.costSheet.Timestamp = DateTime.Now;
                    offer.CostSheet = frmCostSheet.costSheet;
                    if (Convert.ToDecimal(txtTotalOfferAmount.Text) != 0)
                    {
                        txtBudgetMargin.Text = ( offer.CostSheet.TotalBudgetedMargin).ToString();;
                        CalculateMargins();                
                    }
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }
        private void TxtBudgetMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CalculatedExchangeRates();
            CalculateMargins();
        }
        public void CalculateMargins()
        {
            if(offer.CostSheet!=null && Convert.ToDouble(txtTotalOfferAmount.Text) != 0)
            {
                var percent = (Convert.ToDecimal(offer.CostSheet.TotalBudgetedMargin) / Convert.ToDecimal(txtTotalOfferAmount.Text) * 100);

                txtBudgetedPercent.Text = decimal.Round(percent, 2).ToString();
                txtBudgetedPercent.Text += "%";
            }
            if (!string.IsNullOrEmpty(txtSER.Text) && !string.IsNullOrEmpty(txtBudgetMargin.Text))
            {
                //txtBudgetMarginSER.Text = (Convert.ToDouble(txtBudgetMargin.Text) * Convert.ToDouble(txtSER.Text)).ToString();
                var marginSER = Convert.ToDouble(txtBudgetMargin.Text) * Convert.ToDouble(txtSER.Text);
                txtBudgetMarginSER.Text = marginSER.ToString();
            }
            if (!string.IsNullOrEmpty(txtMER.Text) && !string.IsNullOrEmpty(txtBudgetMargin.Text))
            {
                var marginMER = Convert.ToDouble(txtBudgetMargin.Text) * Convert.ToDouble(txtMER.Text);
                txtBudgetMarginMER.Text = marginMER.ToString();
            }
        }
        private void cmbCostCenterCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculatedExchangeRates();
        }
        public void CalculatedExchangeRates()
        {
            if (datofferCreationdate.EditValue != null)
            {
                CurrencyRepo currencyRepo = new CurrencyRepo();
                DateTime creationDate = (DateTime)datofferCreationdate.EditValue;
                DateTime d1 = new DateTime(2016, 01, 01);
                ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
                ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;

                if (lookupCompany.SelectedIndex > -1 && cmbCurrency.SelectedItem != null)
                {
                    if ((cmbCurrency.SelectedItem as cmbitem).id == (lookupCompany.SelectedItem as Company).Id)
                    {
                        txtSER.Text = 1.ToString();
                        txtMER.Text = 1.ToString();
                    }
                    else
                    {

                        if (creationDate > d1)
                        {
                            if (cmbCurrency.SelectedIndex != -1 && cmbbaseCurrency.SelectedIndex != -1)
                            {
                                var exchangeRateGroupSER = exchangeRateGroupRepo.GetGroupByCurrenciesSER((cmbCurrency.SelectedItem as cmbitem).id, (lookupCompany.SelectedItem as Company).Id, creationDate.Year);
                                var exchangeRateGroupMER = exchangeRateGroupRepo.GetGroupByCurrenciesMER((cmbCurrency.SelectedItem as cmbitem).id, (lookupCompany.SelectedItem as Company).Id, creationDate.Year);
                                if (exchangeRateGroupSER != null)
                                {
                                    exchangeRate = exchangeRateGroupSER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                                    switch (creationDate.Month)
                                    {
                                        case 1:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateJan.ToString();
                                            break;
                                        case 2:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateFeb.ToString();
                                            break;
                                        case 3:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateMar.ToString();
                                            break;
                                        case 4:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateApr.ToString();
                                            break;
                                        case 5:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateMay.ToString();
                                            break;
                                        case 6:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateJun.ToString();
                                            break;
                                        case 7:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateJul.ToString();
                                            break;
                                        case 8:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateAug.ToString();
                                            break;
                                        case 9:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateSep.ToString();
                                            break;
                                        case 10:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateOct.ToString();
                                            break;
                                        case 11:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateNov.ToString();
                                            break;
                                        case 12:
                                            if (exchangeRate != null)
                                                txtSER.Text = exchangeRate.rateDec.ToString();
                                            break;
                                        default:
                                            if (exchangeRate != null)
                                                txtSER.Text = 0.ToString();
                                            break;
                                    }
                                }
                                else
                                {
                                    txtSER.Text = 1.ToString();

                                }
                                if (exchangeRateGroupMER != null)
                                {
                                    exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                                    switch (creationDate.Month)
                                    {
                                        case 1:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateJan.ToString();
                                            break;
                                        case 2:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateFeb.ToString();
                                            break;
                                        case 3:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateMar.ToString();
                                            break;
                                        case 4:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateApr.ToString();
                                            break;
                                        case 5:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateMay.ToString();
                                            break;
                                        case 6:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateJun.ToString();
                                            break;
                                        case 7:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateJul.ToString();
                                            break;
                                        case 8:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateAug.ToString();
                                            break;
                                        case 9:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateSep.ToString();
                                            break;
                                        case 10:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateOct.ToString();
                                            break;
                                        case 11:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateNov.ToString();
                                            break;
                                        case 12:
                                            if (exchangeRate != null)
                                                txtMER.Text = exchangeRate.rateDec.ToString();
                                            break;

                                    }
                                }
                                else
                                {
                                    txtMER.Text = 1.ToString();
                                }
                            }
                        }
                        else
                        {
                            ///Working One
                            if (cmbCurrency.SelectedItem != null)
                            {
                                MarketExchangeRate marketyExchangeRate = new MarketExchangeRate();
                                SalesExchangeRate salesExchangeRate = new SalesExchangeRate();
                                marketyExchangeRate = currencyRepo.getMarketexchangerate((int)offer.company_Id, (cmbCurrency.SelectedItem as cmbitem).id);
                                salesExchangeRate = currencyRepo.getsalesexchangerate((int)offer.company_Id, (cmbCurrency.SelectedItem as cmbitem).id);
                                if (marketyExchangeRate != null && salesExchangeRate != null)
                                {

                                    if (marketyExchangeRate != null)
                                    {
                                        txtMER.Text = marketyExchangeRate.exchangerate.ToString();
                                    }
                                    if (salesExchangeRate != null)
                                    {
                                        txtSER.Text = salesExchangeRate.exchangerate.ToString();
                                    }
                                    else if (department?.Id != 0 && department?.applyMERasSER != false && salesExchangeRate != null)
                                    {
                                        txtMER.Text = salesExchangeRate.exchangerate.ToString();
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
                if(!string.IsNullOrEmpty(txtSER.Text) && !string.IsNullOrEmpty(txtTotalOfferAmount.Text))
                {
                    txtOfferAmountSER.Text = (Convert.ToDouble(txtTotalOfferAmount.Text) * Convert.ToDouble(txtSER.Text)).ToString();
                }
                if (!string.IsNullOrEmpty(txtMER.Text) && !string.IsNullOrEmpty(txtTotalOfferAmount.Text))
                {
                    txtOfferAmountMER.Text = (Convert.ToDouble(txtTotalOfferAmount.Text) * Convert.ToDouble(txtMER.Text)).ToString();
                }
                CalculateMargins();
            }

        }

        private void txtMER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CalculateMargins();
        }

        private void txtSER_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CalculateMargins();
        }

        private void txtfob_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtfob.Text))
            {
                txtTotalOfferAmount.Text = txtcfr.Text;
            }
            CalculateMargins();
        }

        private void txtcfr_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtcfr.Text))
            {
                txtTotalOfferAmount.Text = txtcfr.Text;
            }
            CalculateMargins();
        }

        private void txtTotalOfferAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CalculatedExchangeRates();
            CalculateMargins();
        }

        private void btnEmailTread_Click(object sender, RoutedEventArgs e)
        {
           if(gridEmailsTrack.Visibility==Visibility.Visible)
            {
                emailGrid.ItemsSource = null;
                gridEmailsTrack.Visibility = Visibility.Collapsed;
            }
           else
            {
                gridEmailsTrack.Visibility = Visibility.Visible;

                LoadOutlookEmails();
            }

        }

        private void emailGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (emailGrid.SelectedItem != null)
            {
                if (emailGrid.SelectedItem is ERP_BL.EmailIntegration.Email email)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(email.BodyHtml);
                    using (MemoryStream stream = new MemoryStream(byteArray))
                    {
                        emailBodyEditor.LoadDocument(stream, DocumentFormat.Html);
                    }
                    txtFrom.Text = email.From;
                    txtTo.Text = email.To;
                    txtEmailSubject.Text = email.Subject;
                    txtEmailDate.Text = email.Date.ToString();
                }
            }
        }
        private void LoadOutlookEmails()
        {
            try
            {
                string filterValue1 = txtOfferRefNo.Text.Trim().ToLower();
                string filterValue2 = txtInquiryref.Text.Trim().ToLower();
                List<ERP_BL.EmailIntegration.Email> emails = SYSTEM_STATIC.userEmails;
                emails = emails.Where(item =>!string.IsNullOrEmpty(item.Subject) &&(item.Subject.ToLower().Contains(filterValue1) || item.Subject.ToLower().Contains(filterValue2))).ToList();
                emailGrid.ItemsSource = emails;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void GetAllMailItemsRecursively(Microsoft.Office.Interop.Outlook.MAPIFolder folder, List<Microsoft.Office.Interop.Outlook.MailItem> mailItems)
        {
            try
            {
                foreach (object item in folder.Items)
                {
                    if (item is Microsoft.Office.Interop.Outlook.MailItem mail)
                    {
                        mailItems.Add(mail);
                    }
                }

                foreach (Microsoft.Office.Interop.Outlook.MAPIFolder subFolder in folder.Folders)
                {
                    GetAllMailItemsRecursively(subFolder, mailItems);
                }
            }
            catch { /* Skip folders that can't be accessed */ }
        }

    }
}