using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.ChartofAccounts;
using ERP_BL.Config;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Tax;
using ERP_BL.ToDoTasks.Taskss;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using ZAS_ERP.Bankings;
using ZAS_ERP.Bankings.STL;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.Budget;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Procurementss.SaleInvoicess.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls;
using ZAS_ERP.SaleOrderFolder.UserControls.CompanyLoansReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt.DirectReceiptPayment;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;

namespace ZAS_ERP.Procurementss.SaleInvoicess
{
    /// <summary>
    /// Interaction logic for ucInquiryAdd.xaml
    /// </summary>
    public partial class ucSaleInvoiceAdd : UserControl
    {
        public static int editsaleInvoice;
        public static int saleInvoiceid;
        public int InvoiceId;
        public static int editInvoice;
        public static int saleOrderid;
        SaleOrder saleOrder = new SaleOrder();
        SaleInvoiceRepo saleInvoiceRepo = new SaleInvoiceRepo();
        SaleInvoice saleInvoice = new SaleInvoice();
        Vendor vendor = new Vendor();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        CustomerCompany customer = new CustomerCompany();
        public bool isloading = false;
        public virtual List<ProcurementProduct> procurementProducts { get; set; }
        public virtual List<CustomerCredit> customerCredits { get; set; }
        public virtual List<Product> products { get; set; }
        Principal principal = new Principal();
        Currency currency = new Currency();
        UsersRepo UsersRepo = new UsersRepo();
        bool addinfo = true;
        List<ViewInfo> views = new List<ViewInfo>();
        public SaleInvoiceStatus checkStatus = new SaleInvoiceStatus();
        public ERP_BL.Procurements.StatusClass.StatusClass checkStatusClass = new ERP_BL.Procurements.StatusClass.StatusClass();

        //public StatusClass checkStatusClass = new StatusClass();
        public double SOFobRemaining = 0;

        public double SOCFRRemaining = 0;
        public double InvoiceFobRemaining = 0;
        public double InvoiceCFRemaining = 0;
        ERP_BL.Databases.Company InterCompany = new ERP_BL.Databases.Company();
        Department InterDepartment = new Department();
        SaleInvoiceStatus oldStatus = new SaleInvoiceStatus();
        List<JournalTransaction> journalTransactions = new List<JournalTransaction>();
        double receivableTotal = 0;
        TaxRepo taxRepo = new TaxRepo();
        TaxName saleOrderTax = new TaxName();
        List<Company> currentUserCompanies;
        ucfrmAddSubject frmAddSubject = new ucfrmAddSubject();
        Window SubjectWindow = new Window();
        private string saleInvoiceTaxSubject = "";
        private string saleInvoiceDnoteSubject = "";
        private string saleInvoiceCISubject = "";
        private string saleInvoiceZasCommission = "";
        List<Notification> notifications = new List<Notification>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        List<CommentLog> comments = new List<CommentLog>();
        public static int count;
        public static int Totalcount;
        bool isCount = false;
        bool isLoadComment = false;
        ProductRepo productRepo = new ProductRepo();
        OfferRepo offerRepo = new OfferRepo();
        public virtual List<BookerStatementItem> BookerStatementItems { get; set; }
        BudgetCostCenterRepo budgetCostRepo = new BudgetCostCenterRepo();
        SaleInvoice trackingOrder = new  ERP_BL.Databases.SaleInvoice();
        AdjustmentRepo adjustmentRepo = new AdjustmentRepo();
        List<SaleInvoiceStatus> SaleInvoiceStatuses = new List<SaleInvoiceStatus>();
        List<ERP_BL.Procurements.StatusClass.StatusClass> StatusClasses = new List<ERP_BL.Procurements.StatusClass.StatusClass>();
        DateTime statusChangeDate;
        DateTime statusClassChangeDate;
        static string systemRefIntitials = "SI-";
        ERP_BL.Databases.Employee insuarenceAppliedEmployee = new ERP_BL.Databases.Employee();
        bool firstInsuranceCheck = false;
        public ucSaleInvoiceAdd()
        {
            procurementProducts = new List<ProcurementProduct>();
            customerCredits = new List<CustomerCredit>();
            products = new List<Product>();
            InitializeComponent();
            symbol = "";
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

        private void winSaleInvoiceadd_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                InvoiceId = saleInvoiceid;
                if(InvoiceId==0)
                {
                    btnInsuranceRequired.IsChecked = true;
                }
                editInvoice = editsaleInvoice;
                txttax.Text = "0";
                grdInvoiceItems.ItemsSource = procurementProducts;
                grdCustomerCredits.ItemsSource = customerCredits;
                loadBookerItemsSources();
                loadcompanies();
                loadinquirytypes();
                loadPaymentTerms();
                loadIncoterms();
                loadCurrencies();
                loadSaleInvoiceStatus();
                SetValuesforPaymentyearandQuarter();
                LoadTaxes();

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Insurance") != null)
                {
                    insuranceGrid.IsEnabled = true;
                }


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Move SaleInvoice to Inter Company") != null)
                {
                    lookupInterCompany.IsEnabled = true;
                    lookupInterDepartment.IsEnabled = true;
                    chkInterCompany.IsEnabled = true;
                }
                else
                {
                    lookupInterCompany.IsEnabled = false;
                    lookupInterDepartment.IsEnabled = false;
                    chkInterCompany.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of SaleInvoice") != null)
                {
                    datpoCreationdate.IsEnabled = true;
                }
                else
                {
                    datpoCreationdate.IsEnabled = false;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit GL Posting Date of SaleInvoice") != null)
                {
                    datglPostingdate.IsEnabled = true;
                }
                else
                {
                    datglPostingdate.IsEnabled = false;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with SaleInvoice") != null)
                {
                    //btnAttachNew.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAttachNew.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with SaleInvoice") != null)
                {
                    btnAttachmentList.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAttachmentList.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void SaleInvoice") != null)
                {
                    btnSetVoid.Visibility = Visibility.Visible;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit STL stamp") != null)
                {
                    btnSTLGenerated.IsEnabled = true;
                }
                else
                {
                    btnSTLGenerated.IsEnabled = false;

                }
                //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit STL discount") != null)
                //{
                //    btnSTLDiscounted.IsEnabled = true;
                //}
                //else
                //{
                //    btnSTLDiscounted.IsEnabled = false;

                //}


                if (editsaleInvoice == 1 && saleInvoiceid != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Invoice") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Invoice") == null)
                    {
                        isloading = true;
                        saleInvoice = saleInvoiceRepo.get(saleInvoiceid);
                        loadonSaleInvoicedata();
                        GellAllOrdersTracking();
                        //views = _usersRepo.getViwerInfo(saleInvoice.Id, 5);
                        //grdUsers.ItemsSource = views;
                        loadcomments();
                        btnSave.IsEnabled = false;
                        if (saleInvoice.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Sale Invoice") != null)
                        {
                            btnSave.IsEnabled = true;
                        }


                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Invoice") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Invoice") != null)
                    {
                        isloading = true;
                        saleInvoice = saleInvoiceRepo.get(saleInvoiceid);
                        loadonSaleInvoicedata();
                        GellAllOrdersTracking();
                        //views = _usersRepo.getViwerInfo(saleInvoice.Id, 5);
                        //grdUsers.ItemsSource = views;
                        loadcomments();
                        btnSave.IsEnabled = true;

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Invoice") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Invoice") != null)
                    {
                        isloading = true;
                        saleInvoice = saleInvoiceRepo.get(saleInvoiceid);

                        loadonSaleInvoicedata();
                        GellAllOrdersTracking();
                        //views = _usersRepo.getViwerInfo(saleInvoice.Id, 5);
                        //grdUsers.ItemsSource = views;
                        loadcomments();

                        btnSave.IsEnabled = true;

                    }
                    //Setting void stamp
                    if (saleInvoice != null)
                    {
                        if (saleInvoice.isVoid == true)
                        {
                            grdVoid.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Sale Invoice!");
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;

                    }

                }
                else if (editsaleInvoice == 0 && saleOrderid != 0)
                {
                        txtSystemRefNo.Text = calculateSystemRefNo();
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice") != null)
                    {
                        isloading = true;
                        saleOrder = saleInvoiceRepo.getsaleOrder(saleOrderid);
                        loadonSaleOrderdata();
                        GellAllOrdersTracking();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Sale Invoice!");
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;

                    }


                }
                else
                {
                    //cmbcaption1.SelectedIndex = 0;
                    //cmbcaption2.SelectedIndex = 1;
                }
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {

                    CalculateBookerTotal();
                }
                else
                {
                    calculatetotal();
                }

                isloading = false;
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Market Exchange Rate in Sale Invoice") != null)
                {
                    lblexchangerate.Visibility = Visibility.Visible;
                    txtMER.Visibility = Visibility.Visible;
                }
                else
                {
                    lblexchangerate.Visibility = Visibility.Collapsed;
                    txtMER.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit SO SER") != null)
                {

                    txtSER.IsEnabled = true;
                }
                else
                {
                    txtSER.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit SO MER") != null)
                {

                    txtMER.IsEnabled = true;
                }
                else
                {
                    txtSER.IsEnabled = false;
                }



                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Post/Un-Post to GL") != null)
                {
                    btnPushDebits.IsEnabled = true;
                }
                else
                {
                    btnPushDebits.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Post/Un-Post to GL") != null)
                {
                    btnPushCredits.IsEnabled = true;
                }
                else
                {
                    btnPushCredits.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit is-InterCompany receivable check") != null)
                {
                    btnInterCompanyReceivable.IsEnabled = true;
                }
                else
                {
                    btnInterCompanyReceivable.IsEnabled = false;
                }
                if(saleInvoice.Id!=0)
                if (saleInvoice.VATBookRefId != 0 && saleInvoice.VATBookRefNumber != null)
                {
                    var vatBookSource = (List<cmbitem>)cmbxVATBookRef.Items.SourceCollection;
                    var term = vatBookSource.Find(x => x.id == saleInvoice.VATBookRefId);

                    if (term == null)
                    {
                        vatBookSource.Add(new cmbitem() { name = saleInvoice.VATBookRefNumber.VATBookReferenceNo, id = saleInvoice.VATBookRefNumber.Id });
                        cmbxVATBookRef.ItemsSource = null;
                        cmbxVATBookRef.ItemsSource = vatBookSource;
                    }
                    cmbxVATBookRef.SelectedItem = cmbxVATBookRef.Items[cmbxVATBookRef.Items.IndexOf(vatBookSource.Find(x => x.id == saleInvoice.VATBookRefId))];
                }

                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdInvoiceItems);
                grdTrackingTree.ExpandAllNodes();

            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                MessageBox.Show(ex.ToString(), "Data Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //lookupCustomer.Focus();

        }

        private void SetValuesforPaymentyearandQuarter()
        {
            if (System.DateTime.Now.Month < 4)
            {
                spnQuarter.Text = "1";

            }
            else if (System.DateTime.Now.Month >= 4 && System.DateTime.Now.Month <= 6)
            {
                spnQuarter.Text = "2";

            }
            else if (System.DateTime.Now.Month >= 7 && System.DateTime.Now.Month <= 9)
            {
                spnQuarter.Text = "3";

            }
            else if (System.DateTime.Now.Month <= 12)
            {
                spnQuarter.Text = "4";
            }
            spnYear.Text = System.DateTime.Now.Year.ToString();

        }

        //public class cmbitem
        //{
        //    public string name { get; set; }
        //    public int id { get; set; }
        //    public string bcolor { get; set; }
        //    public string fcolor { get; set; }

        //}
        public class datagriditem
        {
            public int itemId { get; set; }
            public int inquiryitemId { get; set; }
            public int saleOrderitemId { get; set; }

            public string Item_Name { get; set; }
            public string Item_Discription { get; set; }
            public string Own_Description { get; set; }
            public string UOM { get; set; }
            public double Quantity { get; set; }

            public double value1 { get; set; }
            public double value2 { get; set; }



        }
        public void loadcompanies()
        {
            //try
            //{
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
        public void loadPrincipals()
        {
            
            CompanyRepo companyRepo = new CompanyRepo();
            var dept=lookupDepartment.SelectedItem as Department;
            var deptPrinciple = companyRepo.getPrinciplesByDepartment(dept.Id);
            lookupPrincipal.ItemsSource = deptPrinciple;
            //lookupPrincipal.ItemsSource = department.Principals;
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

                        if (editInvoice == 1)
                        {
                            if (saleInvoice.Id != 0)
                            {
                                var customer = department.disableCustomers.FirstOrDefault(x => x.Id == saleInvoice.customerCompany_Id);
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
                            lookupCustomerinGrid.ItemsSource = customers;
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
                    cmbSaleInvoiceType.Items.Add(IND);
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
            cmbAppliedBy.ItemsSource = cmbitems;
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
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsSaleInvoiceType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }
                    if (saleInvoice != null && saleInvoice.Id > 0 && editsaleInvoice == 1)
                        if (saleInvoice.department != null && departments.FirstOrDefault(x => x.Id == saleInvoice.dept_Id) == null)
                            departments.Add(saleInvoice.department);

                    lookupDepartment.ItemsSource = departments;
                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }

            //DepartmentRepo cont = new DepartmentRepo();
            //List<ERP_BL.Databases.Department> deptRepo = new List<ERP_BL.Databases.Department>();
            //deptRepo = cont.GetDepartments();
            //List<Department> deptlst = new List<Department>();
            //foreach (Department dept in deptRepo)
            //{
            //    //if (dept.IsSubsidary == false && dept.ParentID == null && dept.Id != frmcompanyCenter.departmentId)
            //    deptlst.Add(dept);

            //}
            //if (company != null)
            //{
            //    CompanyRepo conte = new CompanyRepo();
            //    company = conte.GetCompany(company.Id);
            //    lookupDepartment.ItemsSource = company.departments;
            //}


        }
      
        public void loadonSaleOrderdata()
        {


            if (saleOrder.isApproved != true)
            {
                DXMessageBox.Show("This order is not approved, Invoice can't be created at this time.", "Get Approval for SaleOrder", MessageBoxButton.OK, MessageBoxImage.Stop);
                var myWindow = Window.GetWindow(this);

                myWindow.Close();
                return;
            }
            if (saleOrder.saleOrderStatus.isActive != true)
            {
                DXMessageBox.Show("This order is closed, Invoice can't be created at this SO.", "Closed SaleOrder", MessageBoxButton.OK, MessageBoxImage.Stop);
                var myWindow = Window.GetWindow(this);

                myWindow.Close();
                return;
            }
            if (saleOrder.Id > 0)
            {
                var saleInvoicess = saleInvoiceRepo.getAllBySOid(saleOrder.Id);
                var SIamount = saleInvoicess.Sum(x => x.totalInvoiceAmount);
                if (SIamount >= saleOrder.totalCFRValue)
                {
                    DXMessageBox.Show("This Sale Order has been Invoiced Fully!", "Sale Order fully Invoiced", MessageBoxButton.OK, MessageBoxImage.Information);
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }
            cmbSaleInvoiceType.Text = saleOrder.saleOrdertype.ToString();
            PoType = saleOrder.saleOrdertype;


            /////
            datsaleInvoicedate.EditValue = saleOrder.saleOrderDate;


            datExcpectedPaymentDate.EditValue = saleOrder.ExpectedPayment;
            datPaymentDueTill.EditValue = saleOrder.PaymentDueAgeing;
            datDeliverydate.EditValue = saleOrder.deliveryDate;
            datDeliverydateFinal.EditValue = saleOrder.deliveryDateFinal;
            datpoCreationdate.EditValue = System.DateTime.Now;
            datglPostingdate.EditValue = System.DateTime.Now;


            datBillOfLaddingdate.EditValue = saleOrder.billLaddingDate;
            datLCDatedate.EditValue = saleOrder.lCDate;
            datMaterialReciptdate.EditValue = saleOrder.materialReciptDate;
            datPaymentDueFrom.EditValue = saleOrder.PaymentDueStartDate;

            txtSalesref.Text = saleOrder.SalesReferenceNo;
            txtOfferRefNo.Text = saleOrder.offerReferenceNo;
            txtsaleInvoiceref.Text = saleOrder.referenceNo;

            if (saleOrder.saleOrdertype == InquiryType.Principal)
            {
                //txtSOtotalfob.Text = saleOrder.commision.ToString();
                txtSOtotalcfr.Text = saleOrder.commision.ToString();
                //txtSOremainingfob.Text = saleOrder.commision.ToString();
                txtSOremainingcfr.Text = (saleOrder.RemainingCFRValue == 0) ? saleOrder.commision.ToString() : saleOrder.RemainingCFRValue.ToString();


                if (saleOrder.RemainingCFRValue == 0)

                    SOCFRRemaining = Convert.ToDouble(saleOrder.commision) - saleOrder.RemainingCFRValue;
                else
                    SOCFRRemaining = saleOrder.RemainingCFRValue;

            }
            else
            {
                //txtSOtotalfob.Text = saleOrder.totalFOBValue.ToString();
                txtSOtotalcfr.Text = saleOrder.totalCFRValue.ToString();
                //txtSOremainingfob.Text = saleOrder.totalFOBValue.ToString();


                


                txtSOremainingcfr.Text = (saleOrder.RemainingCFRValue == 0) ? saleOrder.totalCFRValue.ToString() : saleOrder.RemainingCFRValue.ToString();
                SOCFRRemaining = (saleOrder.RemainingCFRValue == 0) ? saleOrder.totalCFRValue - saleOrder.RemainingCFRValue : saleOrder.RemainingCFRValue;
                SOFobRemaining = (saleOrder.RemainingFOBValue == 0) ? saleOrder.totalFOBValue - saleOrder.RemainingFOBValue : saleOrder.RemainingFOBValue;
                txtBasetotalfob.Text = saleOrder.totalBaseFOBValue.ToString();
                totalMER.Text = saleOrder.totalBaseCFRValue.ToString();
                txtTotalSER.Text = saleOrder.SoAmountSER.ToString();
            }



            txtMaker.Text = saleOrder.maker;
            txtOrigin.Text = saleOrder.origin;

            txtOwnDescription.Text = saleOrder.OwnDescription;
            txtCommissionNumber.Text = saleOrder.commisionRefrenceNo;
            txtFinanaceRef.Text = saleOrder.FinanceRefrenceNo;


            //txtSalesTargetYear.Text = saleOrder.targetYear.ToString();
            //txtSalesTargetMonth.Text = saleOrder.targetMonth.ToString();

            //txtComments.Text = saleOrder.comments;


            txtPaymentDueDays.Text = saleOrder.CreditDays.ToString();

            lblStage.Text = (saleOrder.stage != null) ? saleOrder.stage : "";
            txtLCNumber.Text = saleOrder.lCnumber;
            if (saleOrder.isPercentTax == true)
                txttax.Text = saleOrder.salesTax.ToString() + "%";
            else
                txttax.Text = saleOrder.salesTax.ToString();
            //inco term for 

            if (saleOrder.incoterm_Id != 0 && saleOrder.incoterm != null)
            {
                var incoSource = (List<cmbitem>)cmbIncoterm.Items.SourceCollection;
                cmbIncoterm.SelectedItem = cmbIncoterm.Items[cmbIncoterm.Items.IndexOf(incoSource.Find(x => x.id == saleOrder.incoterm_Id))];

            }


            ////select Captions for Item Value 1
            //if (saleOrder.TitleValue1Id != 0 || saleOrder.TitleValue1 != null)
            //{
            //    var captionSource = (List<cmbitem>)cmbcaption1.Items.SourceCollection;
            //    cmbcaption1.SelectedItem = cmbcaption1.Items[cmbcaption1.Items.IndexOf(captionSource.Find(x => x.id == saleOrder.TitleValue1Id))];


            //}
            ////select Captions for Item Value 2
            //if (saleOrder.TitleValue2Id != 0 || saleOrder.TitleValue2 != null)
            //{
            //    var caption2Source = (List<cmbitem>)cmbcaption2.Items.SourceCollection;
            //    cmbcaption2.SelectedItem = cmbcaption2.Items[cmbcaption2.Items.IndexOf(caption2Source.Find(x => x.id == saleOrder.TitleValue2Id))];


            //}
            //Payment term Load for sale Order
            if (saleOrder.paymentterm_Id != 0 && saleOrder.paymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbPaymentTerm.Items.SourceCollection;
                //cmbPaymentTerm.SelectedItem = cmbPaymentTerm.Items[cmbPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == saleOrder.paymentterm_Id))];

                var term = paymentTermSource.Find(x => x.id == saleOrder.paymentterm_Id);

                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = saleOrder.paymentTerm.term, id = saleOrder.paymentTerm.Id });
                    cmbPaymentTerm.ItemsSource = null;
                    cmbPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbPaymentTerm.SelectedItem = cmbPaymentTerm.Items[cmbPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == saleOrder.paymentTerm.Id))];
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
                catch (Exception ex)
                {

                }

            }


            // load Products in Inquiry to grid
            List<datagriditem> datagriditems = new List<datagriditem>();
            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
                List<BookerStatementItem> bItems = new List<BookerStatementItem>();

                foreach(var book in saleOrder.BookerStatementItems)
                {
                    BookerStatementItem item = new BookerStatementItem()
                    {
                        amount=book.amount,
                        siAmount=book.amount,
                        ClaimDiscount=book.ClaimDiscount,
                        amountGST=book.amountGST,
                        siAmountGST=book.amountGST,
                        claimDiscountId=book.claimDiscountId,
                        claimDiscountValue=book.claimDiscountValue,
                        siClaimDiscountValue=book.claimDiscountValue,
                        FOCSampling=book.FOCSampling,
                        focSamplingId=book.focSamplingId,
                        focValue=book.focValue,
                        siFocValue=book.focValue,
                        netAmount=book.netAmount,
                        siNetAmount=book.netAmount,
                        PassOn=book.PassOn,
                        passOnId=book.passOnId,
                        product=book.product,
                        product_Id=book.product_Id,
                        passOnValue=book.passOnValue,
                        siPassOnValue=book.passOnValue,
                        quantity=book.quantity,
                        siQuantity=book.quantity,
                        TaxName=book.TaxName,
                        unit=book.unit,
                        weight=book.weight,
                        siWeight=book.weight,
                        taxNameId=book.taxNameId
                    };
                    bItems.Add(item);
                }
                grdBokkerItems.ItemsSource = bItems;
            }
            else
            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {
                List<BookerStatementItem> bItems = new List<BookerStatementItem>();

                foreach (var book in saleOrder.BookerStatementItems)
                {
                    BookerStatementItem item = new BookerStatementItem()
                    {
                        amount = book.amount,
                        siAmount = book.amount,
                        ClaimDiscount = book.ClaimDiscount,
                        amountGST = book.amountGST,
                        siAmountGST = book.amountGST,
                        claimDiscountId = book.claimDiscountId,
                        claimDiscountValue = book.claimDiscountValue,
                        siClaimDiscountValue = book.claimDiscountValue,
                        FOCSampling = book.FOCSampling,
                        focSamplingId = book.focSamplingId,
                        focValue = book.focValue,
                        siFocValue = book.focValue,
                        netAmount = book.netAmount,
                        siNetAmount = book.netAmount,
                        PassOn = book.PassOn,
                        passOnId = book.passOnId,
                        product = book.product,
                        product_Id = book.product_Id,
                        passOnValue = book.passOnValue,
                        siPassOnValue = book.passOnValue,
                        quantity = book.quantity,
                        siQuantity = book.quantity,
                        TaxName = book.TaxName,
                        unit = book.unit,
                        weight = book.weight,
                        siWeight = book.weight,
                        taxNameId = book.taxNameId
                    };
                    bItems.Add(item);
                }
                grdBokkerItems.ItemsSource = bItems;
            }
            else
            {
                if (saleOrder.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();

                    foreach (var procurementProduct in saleOrder.products)
                    {
                        procurementProducts.Add(new ProcurementProduct()
                        {


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
                            UnInvoicedQuantity = (procurementProduct.UnInvoicedQuantity == 0) ? ((procurementProduct.TotalInvoicedQuantity != 0 && procurementProduct.UnInvoicedQuantity == 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.TotalInvoicedQuantity : procurementProduct.inquiryProduct.quantity) : procurementProduct.UnInvoicedQuantity,
                            TotalInvoicedQuantity = (procurementProduct.TotalInvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity == 0) ? 0 : ((procurementProduct.TotalInvoicedQuantity == 0 && procurementProduct.UnInvoicedQuantity != 0) ? procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity : procurementProduct.TotalInvoicedQuantity),
                            UnInvoicedWeight = (procurementProduct.inquiryProduct.Weight == null) ? 0 : ((procurementProduct.UnInvoicedWeight == 0) ? ((procurementProduct.TotalInvoicedWeight != 0 && procurementProduct.UnInvoicedWeight == 0) ? Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.TotalInvoicedWeight : Convert.ToDouble(procurementProduct.inquiryProduct.Weight)) : procurementProduct.UnInvoicedWeight),
                            TotalInvoicedWeight = (procurementProduct.inquiryProduct.Weight == null) ? 0 : ((procurementProduct.TotalInvoicedWeight == 0 && procurementProduct.UnInvoicedWeight == 0) ? 0 : ((procurementProduct.TotalInvoicedWeight == 0 && procurementProduct.UnInvoicedWeight != 0) ? (Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight) : procurementProduct.TotalInvoicedWeight)),
                            totalCommission = (saleOrder.commision == null) ? 0 : (Convert.ToDouble(saleOrder.commision)),
                            UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                            totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                            value2 = procurementProduct.value2,
                            priority = procurementProduct.priority
                        });
                    }
                    grdInvoiceItems.ItemsSource = procurementProducts;
                    int x = 0;
                    foreach (var pro in grdInvoiceItems.ItemsSource as List<ProcurementProduct>)
                    {
                        grdInvoiceItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                        x++;

                    }
                    lookupProductsinGrid.DisplayMember = "code";

                }
                else
                {
                    grdInvoiceItems.ItemsSource = procurementProducts;
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
            txtSER.Text = saleOrder.marginExchangeRate.ToString();
            txtMER.Text = saleOrder.exchangeRate.ToString();

            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {

                CalculateBookerTotal();
            }
            else
             if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {
                CalculateBookerTotal();
            }
            else
            {
                calculatetotal();
            }
            btnPushDebits.IsChecked = true;
            btnPushCredits.IsChecked = true;
         
        }
        private void loadVATBookReferenceNo()
        {
            if (lookupCompany.SelectedItem != null)
            {
                ERP_BL.VATBook.VATBookRepo vatBookRepo = new ERP_BL.VATBook.VATBookRepo();
                var references = vatBookRepo.GetAllActiveVATBookReferenceNo((lookupCompany.SelectedItem as Company).Id);
                List<cmbitem> cmbitems = new List<cmbitem>();
                cmbitems.Add(new cmbitem() { name = "--Select--", id = 0 });
                foreach (ERP_BL.VATBook.VATBookRefNumber _ref in references)
                {
                    cmbitems.Add(new cmbitem() { name = _ref.VATBookReferenceNo, id = _ref.Id });
                }
                cmbxVATBookRef.ItemsSource = cmbitems;
            }
        }
        public void loadonSaleInvoicedata()
        {
            TaskRepo taskRepo = new TaskRepo();
            var task = taskRepo.GetSITask(saleInvoiceid);
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

            

            if (saleInvoice.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            //else if (saleInvoice.isReApproved == false)
            //{
            //    //lblStage.Text = "Under Re-Approval";
            //}
            else if (saleInvoice.isApproved == true && saleInvoice.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (saleInvoice.isApproved == true && saleInvoice.saleInvoiceStatus.isActive == false && saleInvoice.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (saleInvoice.isApproved == true && saleInvoice.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (saleInvoice.isApproved == true)
            {
                //lblStage.Text = "Approved";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (saleInvoice.isApproved == false)
            {
                //lblStage.Text = "Under Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (saleInvoice.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            if(saleInvoice.isRedInvoice!=false)
            {
                btnRedInvoice.IsChecked = true;
            }
            if (saleInvoice.isPaid != false)
            {
                btnPaid.IsChecked = true;
            }
            if (saleInvoice.paymentOnDate != null)
            {
                datPaymentOnDate.EditValue = saleInvoice.paymentOnDate.Value;
            }
            txtDeliveryDays.Text = saleInvoice.deliveryDays.ToString();
            

            ProcurementRepo procurementRepo = new ProcurementRepo();

            // treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(saleInvoice.Id, TransactionItemType.Sale_Invoice);
            //cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();

             var creditJournalTransactions = saleInvoice.journalTransactions.Where(x => x.credit != 0).ToList();
            var debitJournalTransactions = saleInvoice.journalTransactions.Where(x => x.debit != 0).ToList();
            if(creditJournalTransactions.Count>0)
            {
                btnPushCredits.IsChecked = true;
            }
            if (debitJournalTransactions.Count > 0)
            {
                btnPushDebits.IsChecked = true;
            }
            //if (saleInvoice.journalTransactions!=null && saleInvoice.journalTransactions.Count > 0)
            //{
            //    btnPushtoGL.IsChecked = true;
            //}
            cmbSaleInvoiceType.Text = saleInvoice.saleInvoicetype.ToString();
            PoType = saleInvoice.saleInvoicetype;
            if(saleInvoice.GLPostingDate!=null)
            {
                datglPostingdate.EditValue = saleInvoice.GLPostingDate;
            }
            else
            {
                datglPostingdate.EditValue = saleInvoice.CreationDate;
            }
            if (saleInvoice.paymentOnDate != null)
            {
                datPaymentOnDate.EditValue = (DateTime)saleInvoice.paymentOnDate;
            }
            txtDeliveryDays.Text = saleInvoice.deliveryDays.ToString();
            /// Old Logic Working. Only Issue on Current Date 

            /////

            datLastStatusChangeDate.EditValue = saleInvoice.LastStatusChangeDate;
            datLastStatusClassChangeDate.EditValue = saleInvoice.LastStatusClassChangeDate;
            datsaleInvoicedate.EditValue = saleInvoice.saleInvoiceDate;
            datpoCreationdate.EditValue = saleInvoice.CreationDate;
            datETADate.EditValue = saleInvoice.ETADate;
            datETDdate.EditValue = saleInvoice.ETDDate;
            datExcpectedPaymentDate.EditValue = saleInvoice.ExpectedPayment;
            datExcpectedDiscountDate.EditValue = saleInvoice.ExpectedDiscountDate;
            datPaymentDueTill.EditValue = saleInvoice.PaymentDueAgeing;
            datDeliverydate.EditValue = saleInvoice.deliveryDate;
            datDeliverydateFinal.EditValue = saleInvoice.SaleOrder?.deliveryDateFinal;
            //datInvoiceConfirmationdate.EditValue = saleInvoice.orderConfirmationDate;
            datBillOfLaddingdate.EditValue = saleInvoice.BLAWBDate;
            txtBillLandingRef.Text = saleInvoice.BLdeliveryRefNo;
            txtLOTNo.Text = saleInvoice.lotNo;
            datLCDatedate.EditValue = saleInvoice.lCDate;
            datMaterialReciptdate.EditValue = saleInvoice.materialReciptDate;
            datPaymentDueFrom.EditValue = saleInvoice.PaymentDueStartDate;
            if (saleInvoice.amountSOC != 0)
            {
                txtAmountSOC.Text = saleInvoice.amountSOC.ToString();
            }
            txtInvoiceNo.Text = saleInvoice.invoiceNo;
            datInvoice.EditValue = saleInvoice.invoiceDate;
            txtSalesref.Text = saleInvoice.SalesReferenceNo;
            txtOfferRefNo.Text = saleInvoice.offerReferenceNo;
            txtsaleInvoiceref.Text = saleInvoice.referenceNo;
            if (saleInvoice.PaymentYear == null && saleInvoice.PaymentQuarter == null)
                SetValuesforPaymentyearandQuarter();
            else
            {
                spnYear.Text = saleInvoice.PaymentYear.ToString();
                spnQuarter.Text = saleInvoice.PaymentQuarter.ToString();
            }

            if (saleInvoice.salesReceipts != null)
            {

                double invoiceAmount = 0;
                if (saleInvoice.saleInvoicetype == InquiryType.DistributionBiz)
                    invoiceAmount = Math.Round(saleInvoice.BookerStatementItems.Sum(x => x.siNetAmount), 2);
                else
                     if (saleInvoice.saleInvoicetype == InquiryType.DistributionBiz_CustomerCredit)
                    invoiceAmount = Math.Round(saleInvoice.BookerStatementItems.Sum(x => x.siNetAmount), 2);
                else
                    invoiceAmount = saleInvoice.totalInvoiceAmount;

                var collected = saleInvoice.salesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                txtReceivedtotal.Text = collected.ToString();
                txtRemainingReciepts.Text = Math.Round((invoiceAmount - collected), 2).ToString();
            }


            if (saleInvoice.SaleOrder?.SaleInvoices != null) // load info from saleorder
            {
                var soAmount = Math.Round(saleInvoice?.SaleOrder?.commision != 0 ? Convert.ToDouble(saleInvoice?.SaleOrder?.commision) : saleInvoice.SaleOrder.totalCFRValue, 2, MidpointRounding.AwayFromZero);
                var sumInvocies = Math.Round(Convert.ToDouble(saleInvoice.SaleOrder?.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount)), 2, MidpointRounding.AwayFromZero);
                //sumInvocies = Math.Round(sumInvocies, 0, MidpointRounding.AwayFromZero);
                txtSOtotalcfr.Text = soAmount.ToString();
                txtSOremainingcfr.Text = (Math.Round(soAmount - sumInvocies, 2)).ToString();
                SOCFRRemaining =Math.Round( Convert.ToDouble(soAmount - sumInvocies) + saleInvoice.totalInvoiceAmount, 2);

            }
            else
            {
                //txtSOtotalfob.Text = saleInvoice.SOFOBValue.ToString();
                txtSOtotalcfr.Text = saleInvoice.SOCFRValue.ToString();
                //txtSOremainingfob.Text = saleInvoice.SaleOrder.RemainingFOBValue.ToString();
                txtSOremainingcfr.Text = saleInvoice.SaleOrder.RemainingCFRValue.ToString();
                //SOFobRemaining = saleInvoice.SaleOrder.RemainingFOBValue + saleInvoice.totalFOBValue;
                SOCFRRemaining = Math.Round(saleInvoice.SaleOrder.RemainingCFRValue + saleInvoice.totalInvoiceAmount, 2);
            }
           

            //txttotalfob.Text = saleInvoice.totalFOBValue.ToString();
            txttotalcfr.Text = saleInvoice.totalInvoiceAmount.ToString();
            //txtBasetotalfob.Text = saleInvoice.totalBaseFOBValue.ToString();
            totalMER.Text = saleInvoice.totalBaseAmount.ToString();
            txtTotalSER.Text = saleInvoice.SoAmountSER.ToString();
            txtMaker.Text = saleInvoice.maker;
            txtOrigin.Text = saleInvoice.origin;

            txtOwnDescription.Text = saleInvoice.OwnDescription;
            txtCommissionNumber.Text = saleInvoice.commisionRefrenceNo;
            txtFinanaceRef.Text = saleInvoice.FinanceRefrenceNo;

            //txtSalesTargetYear.Text = saleInvoice.targetYear.ToString();
            //txtSalesTargetMonth.Text = saleInvoice.targetMonth.ToString();

            //txtComments.Text = saleInvoice.comments;


            txtPaymentDueDays.Text = saleInvoice.CreditDays.ToString();

            lblStage.Text = (saleInvoice.stage != null) ? saleInvoice.stage : "";
            txtLCNumber.Text = saleInvoice.lCnumber;
            if (saleInvoice.isPercentTax == true)
                txttax.Text = saleInvoice.salesTax.ToString() + "%";
            else
                txttax.Text = saleInvoice.salesTax.ToString();
            //inco term for po

            if (saleInvoice.incoterm_Id != 0 && saleInvoice.incoterm != null)
            {
                var incoSource = (List<cmbitem>)cmbIncoterm.Items.SourceCollection;
                cmbIncoterm.SelectedItem = cmbIncoterm.Items[cmbIncoterm.Items.IndexOf(incoSource.Find(x => x.id == saleInvoice.incoterm_Id))];

            }

           
            //select Captions for Item Value 1
            //if (saleInvoice.TitleValue1Id != 0 || saleInvoice.TitleValue1 != null)
            //{
            //    var captionSource = (List<cmbitem>)cmbcaption1.Items.SourceCollection;
            //    cmbcaption1.SelectedItem = cmbcaption1.Items[cmbcaption1.Items.IndexOf(captionSource.Find(x => x.id == saleInvoice.TitleValue1Id))];

            //    //foreach (cmbitem cmbitem in cmbcaption1.Items)
            //    //{
            //    //    if (cmbitem.id == saleInvoice.TitleValue1Id)
            //    //    {
            //    //        cmbcaption1.SelectedItem = cmbitem;
            //    //        break;
            //    //    }
            //    //}
            //}
            //select Captions for Item Value 2
            //if (saleInvoice.TitleValue2Id != 0 || saleInvoice.TitleValue2 != null)
            //{
            //    //var caption2Source = (List<cmbitem>)cmbcaption2.Items.SourceCollection;
            //    //cmbcaption2.SelectedItem = cmbcaption2.Items[cmbcaption2.Items.IndexOf(caption2Source.Find(x => x.id == saleInvoice.TitleValue2Id))];

            //    //foreach (cmbitem cmbitem in cmbcaption2.Items)
            //    //{
            //    //    if (cmbitem.id == saleInvoice.TitleValue2Id)
            //    //    {
            //    //        cmbcaption2.SelectedItem = cmbitem;
            //    //        break;
            //    //    }
            //    //}
            //}
            //Payment term Load for sale Invoice
            if (saleInvoice.paymentterm_Id != 0 && saleInvoice.paymentTerm != null)
            {
                var paymentTermSource = (List<cmbitem>)cmbPaymentTerm.Items.SourceCollection;
                //cmbPaymentTerm.SelectedItem = cmbPaymentTerm.Items[cmbPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == saleInvoice.paymentterm_Id))];

                var term = paymentTermSource.Find(x => x.id == saleInvoice.paymentterm_Id);

                if (term == null)
                {
                    paymentTermSource.Add(new cmbitem() { name = saleInvoice.paymentTerm.term, id = saleInvoice.paymentTerm.Id });
                    cmbPaymentTerm.ItemsSource = null;
                    cmbPaymentTerm.ItemsSource = paymentTermSource;
                }
                cmbPaymentTerm.SelectedItem = cmbPaymentTerm.Items[cmbPaymentTerm.Items.IndexOf(paymentTermSource.Find(x => x.id == saleInvoice.paymentTerm.Id))];

               

                //foreach (cmbitem cmbitem in cmbPaymentTerm.Items)
                //{
                //    if (cmbitem.id == saleInvoice.paymentterm_Id)
                //    {
                //        cmbPaymentTerm.SelectedItem = cmbitem;
                //        break;
                //    }
                //}
            }
            //saleInvoice.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
            //saleInvoice.saleInvoicetype = (InquiryType)cmbInquiryType.SelectedIndex;
            //saleInvoice.saleOrderReferenceNo = txtOurRefNo.Text.Trim();
            //saleInvoice.deliveryTerm = txtDeliveryTerm.Text.Trim();
            if (saleInvoice.currency_Id != 0 && saleInvoice.currency != null)
            {
                var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleInvoice.currency_Id))];

                //foreach (cmbitem cmbitem in cmbCurrency.Items)
                //{
                //    if (cmbitem.id == saleInvoice.currency_Id)
                //    {
                //        cmbCurrency.SelectedItem = cmbitem;
                //        break;
                //    }

                //}
            }
            else
            {
                var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleInvoice.company.CurrencyId))];

                //foreach (cmbitem cmbitem in cmbCurrency.Items)
                //{
                //    if (cmbitem.id == saleInvoice.company.CurrencyId)

                //    {
                //        cmbCurrency.SelectedItem = cmbitem;
                //        break;
                //    }
                //}
            }
            if (saleInvoice.stlDiscountCurrency_Id != 0 && saleInvoice.stlDiscountCurrency != null)
            {
                var currencySource = (List<cmbitem>)cmbDiscountCurrency.Items.SourceCollection;
                cmbDiscountCurrency.SelectedItem = cmbDiscountCurrency.Items[cmbDiscountCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleInvoice.stlDiscountCurrency_Id))];

                //foreach (cmbitem cmbitem in cmbCurrency.Items)
                //{
                //    if (cmbitem.id == saleInvoice.currency_Id)
                //    {
                //        cmbCurrency.SelectedItem = cmbitem;
                //        break;
                //    }

                //}
            }
            if (saleInvoice.stlSTLCurrency_Id != 0 && saleInvoice.stlCurrency != null)
            {
                var currencySource = (List<cmbitem>)cmbSTLCurrency.Items.SourceCollection;
                cmbSTLCurrency.SelectedItem = cmbSTLCurrency.Items[cmbSTLCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleInvoice.stlSTLCurrency_Id))];

                //foreach (cmbitem cmbitem in cmbCurrency.Items)
                //{
                //    if (cmbitem.id == saleInvoice.currency_Id)
                //    {
                //        cmbCurrency.SelectedItem = cmbitem;
                //        break;
                //    }

                //}
            }
            txtStlDiscount.Text = saleInvoice.stlDiscountAmount.ToString();
            txtStlAmount.Text = saleInvoice.stlAmount.ToString();

            if (saleInvoice.SaleOrderId != null && saleInvoice.SaleOrder != null)
                saleOrder = saleInvoice.SaleOrder;

            // Select Company
            if (saleInvoice.company_Id != null || saleInvoice.company != null)
            {
                company = saleInvoice.company;
                lookupCompany.Text = saleInvoice.company.CompanyName;
                //lookupCompany.SelectedItem = lookupCompany.GetItemByKeyValue(saleInvoice.company);



                //loaddepartments();
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }
            // Select Department
            if (saleInvoice.dept_Id != 0 || saleInvoice.department != null)
            {
                lookupDepartment.Text = saleInvoice.department.DeptName;
                //lookupDepartment.SelectedItem = lookupDepartment.GetItemByKeyValue(saleInvoice.department);
                department = saleInvoice.department;

                //lookupCustomer.ItemsSource = department.customers;
                loademployees();

            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }



            if (saleInvoice.lotNumber != null)
            {
                lookupLotNumbers.Text = saleInvoice.lotNumber.LotNo;
            }

            if (saleInvoice.InterCompany_Id != null || saleInvoice.InterCompany != null)
            {
                var companylist = (lookupInterCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupInterCompany.ItemsSource as List<Company>;
                if (saleInvoice.InterCompany != null && companylist.Find(x => x.Id == saleInvoice.InterCompany_Id) == null)
                {
                    companylist.Add(saleInvoice.InterCompany);
                    lookupInterCompany.ItemsSource = companylist;
                }
                InterCompany = saleInvoice.InterCompany;
                lookupInterCompany.Text = saleInvoice.InterCompany?.CompanyName;
                chkInterCompany.IsChecked = true;
            }
            if (saleInvoice.InterDepartment_Id != null || saleInvoice.InterDepartment != null)
            {
                var deptlist = (lookupInterDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupInterDepartment.ItemsSource as List<Department>;

                if (saleInvoice.InterDepartment != null && deptlist.Find(x => x.Id == saleInvoice.InterDepartment_Id) == null)
                {
                    deptlist.Add(saleInvoice.InterDepartment);
                    lookupInterDepartment.ItemsSource = deptlist;
                }
                lookupInterDepartment.Text = saleInvoice.InterDepartment?.DeptName;
                InterDepartment = saleInvoice.InterDepartment;
            }


            //lookupBank.SelectedIndex = -1;
            if (saleInvoice.bank != null)
            {
                lookupBank.Text = saleInvoice.bank.BankName;
            }
            if (saleInvoice.account != null)
            {
                lookupAccount.Text = saleInvoice.account.AccountNo;
            }

            //Select Customer
            if (saleInvoice.customerCompany.Id != 0 || saleInvoice.customerCompany != null)
            {
                lookupCustomer.Text = saleInvoice.customerCompany.company.CompanyName;
                //lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(saleInvoice.customerCompany);

                customer = saleInvoice.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            //Select vendor
            if (saleInvoice.vendors != null)
            {

                foreach (Vendor vendr in saleInvoice.vendors)
                {
                    lookupVendor.Text = vendr.company.CompanyName;
                    vendor = vendr;
                }
            }
            //if (saleInvoice.vendor_Id != 0 || saleInvoice.vendor != null)
            //{
            //    lookupVendor.Text = saleInvoice.vendor.company.CompanyName;
            //    vendor = saleInvoice.vendor;
            //}
            //else
            //{
            //    lookupVendor.Text = "Select Vendor";

            //}
            //Select Principal
            if (saleInvoice.principal_Id != 0 || saleInvoice.principal != null)
            {
                lookupPrincipal.Text = saleInvoice.principal.company.CompanyName;
                principal = saleInvoice.principal;
            }
            else
            {
                lookupPrincipal.Text = "Select Vendor";

            }
            // Select Employee 
            if (saleInvoice.allocation_Id != 0 || saleInvoice.employee != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbEmployee.Items.SourceCollection;
                    cmbEmployee.SelectedItem = cmbEmployee.Items[cmbEmployee.Items.IndexOf(empSource.Find(x => x.id == saleInvoice.allocation_Id))];

                }
                catch (Exception ex)
                {

                }
            }
            if (saleInvoice.insuranceAppliedBy_Id != 0 || saleInvoice.insuranceAppliedBy != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbAppliedBy.Items.SourceCollection;
                    cmbAppliedBy.SelectedItem = cmbAppliedBy.Items[cmbAppliedBy.Items.IndexOf(empSource.Find(x => x.id == saleInvoice.insuranceAppliedBy_Id))];
                    insuarenceAppliedEmployee = saleInvoice.insuranceAppliedBy;

                }
                catch (Exception ex)
                {

                }
            }


            if (saleInvoice.saleInvoiceStatus != null)
            {
                var disAbleStatus = SaleInvoiceStatuses.FirstOrDefault(x => x.Id == saleInvoice.saleInvoiceStatus.Id);
                if (disAbleStatus == null)
                {
                    loadSaleInvoiceStatus(saleInvoice.saleInvoiceStatus);
                }
            } 
          

            var SOSource = (List<cmbitem>)cmbSaleInvoiceStatus.Items.SourceCollection;

            // Select SaleInvoice Status 
            if (saleInvoice.saleInvoiceStatus != null)
                if (saleInvoice.saleInvoiceStatus.isActive == false)
                {
                    try
                    {
                        cmbSaleInvoiceStatus.SelectedItem = cmbSaleInvoiceStatus.Items[cmbSaleInvoiceStatus.Items.IndexOf(SOSource.Find(x => x.name == saleInvoice.saleInvoiceStatus.Status))];
                    }
                    catch (Exception ex)
                    {
                        SystemLog.LogError(this.GetType(), "This User cannot see closed Sale Invoice status! " + ex.ToString());
                    }

                }
                else
                {
                    cmbSaleInvoiceStatus.SelectedItem = cmbSaleInvoiceStatus.Items[cmbSaleInvoiceStatus.Items.IndexOf(SOSource.Find(x => x.name == saleInvoice.saleInvoiceStatus.Status))];
                }
            checkStatus = saleInvoice.saleInvoiceStatus;
            if(saleInvoice.StatusClass!=null)
            checkStatusClass = saleInvoice.StatusClass;
            if (saleInvoice.StatusClass != null)
            {
                var disAbleStatus = StatusClasses.FirstOrDefault(x => x.Id == saleInvoice.statusClass_Id);
                if (disAbleStatus == null)
                {
                    loadSaleInvoiceStatusClass(saleInvoice.StatusClass);
                }
                var StatusClassSource = (List<cmbitem>)cmbSaleInvoiceStatusClass.Items.SourceCollection;

                // Select SaleInvoice Status 
                if (saleInvoice.StatusClass.isActive == false)
                {
                    try
                    {
                        cmbSaleInvoiceStatusClass.SelectedItem = cmbSaleInvoiceStatusClass.Items[cmbSaleInvoiceStatusClass.Items.IndexOf(StatusClassSource.Find(x => x.name == saleInvoice.StatusClass.ClassName))];
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    cmbSaleInvoiceStatusClass.SelectedItem = cmbSaleInvoiceStatusClass.Items[cmbSaleInvoiceStatusClass.Items.IndexOf(StatusClassSource.Find(x => x.name == saleInvoice.StatusClass.ClassName))];
                }
            }
            if (saleInvoice.CustomerCredits != null)
            {
                grdCustomerCredits.ItemsSource = saleInvoice.CustomerCredits;
            }
            List<datagriditem> datagriditems = new List<datagriditem>();
            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
                if (saleInvoice.BookerStatementItems.Count > 0)
                {
                    grdBokkerItems.ItemsSource = saleInvoice.BookerStatementItems;
                }
              
            }
            else
            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {
                if (saleInvoice.BookerStatementItems.Count > 0)
                {
                    grdBokkerItems.ItemsSource = saleInvoice.BookerStatementItems;
                }
            }
            else
            {
                if (saleInvoice.products != null)
                {
                    List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                    //grdInvoiceItems.ItemsSource = saleOrder.products;
                    foreach (var procurementProduct in saleInvoice.products)
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


                                    isActive = procurementProduct.inquiryProduct.product.isActive,
                                    journalTransactions = procurementProduct.inquiryProduct.product.journalTransactions,
                                    incomeAccount = procurementProduct.inquiryProduct.product.incomeAccount,
                                    //accountReceivable = procurementProduct.inquiryProduct.product.accountReceivable,
                                }
                                    ,
                                product_Id = procurementProduct.inquiryProduct.product.Id

                            },
                            product_Id = procurementProduct.inquiryProduct.Id,
                            unitPrice = procurementProduct.unitPrice,
                            UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                            InvoicedQuantity = procurementProduct.InvoicedQuantity,
                            TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                            UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                            InvoicedWeight = procurementProduct.InvoicedWeight,
                            TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,

                            totalCommission = (saleOrder.commision != null) ? (Convert.ToDouble(saleOrder.commision)) : ((procurementProduct.totalCommission != 0) ? procurementProduct.totalCommission : 0),


                            value2 = procurementProduct.value2,
                            //value2= GetProductSoAmount(procurementProduct),
                            totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                            UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                            NowAmount = procurementProduct.NowAmount,
                            priority = procurementProduct.priority
                        });


                    }
                    grdInvoiceItems.ItemsSource = procurementProducts;
                    int x = 0;
                    foreach (var pro in grdInvoiceItems.ItemsSource as List<ProcurementProduct>)
                    {
                        grdInvoiceItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                        x++;

                    }
                    lookupProductsinGrid.DisplayMember = "code";

                }
                else
                {
                    grdInvoiceItems.ItemsSource = procurementProducts;
                }
            }

            // selected currency of customer company
            foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
            {
                //if (inquiry.customerCompany.company!=null)
                //    if (cmbitem.id == inquiry.customerCompany.company.CurrencyId)
                if (cmbitem.id == saleInvoice.company.CurrencyId)
                {
                    cmbbaseCurrency.SelectedItem = cmbitem;
                    break;
                }
            }
            txtMER.Text = saleInvoice.exchangeRate.ToString();
            txtSER.Text = saleInvoice.marginExchangeRate.ToString();

            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {

                CalculateBookerTotal();
            }
            else
              if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {

                CalculateBookerTotal();
            }
            else
            {
                calculatetotal();
            }
            if (saleInvoice.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sales Invoice Value before approval") != null)
            {
                grdInvoiceItems.Columns.GetColumnByFieldName("NowAmount").AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                grdInvoiceItems.Columns.GetColumnByFieldName("value2").AllowEditing = DevExpress.Utils.DefaultBoolean.True;

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Sale Invoice") != null)
                {
                    grdsaleInvoicedata.IsEnabled = true;
                }
                else
                    grdsaleInvoicedata.IsEnabled = false;
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sales Invoice Value after approval") == null && saleInvoice.isApproved == true)
            {
                grdInvoiceItems.Columns.GetColumnByFieldName("NowAmount").AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                grdInvoiceItems.Columns.GetColumnByFieldName("value2").AllowEditing = DevExpress.Utils.DefaultBoolean.True;
                grdTax.IsEnabled = false;

            }
            if (saleInvoice.saleInvoiceStatus != null)
                if (saleInvoice.saleInvoiceStatus.isActive == false && MainWindow.currentUserid != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed SaleInvoice") == null)
                {
                    //grdsaleInvoicedata.IsEnabled = false;
                    grdInvoiceItems.Columns.GetColumnByFieldName("NowAmount").AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    grdInvoiceItems.Columns.GetColumnByFieldName("value2").AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                    labeltopStatus.Visibility = Visibility.Visible;
                    labeltopStatus.Text = saleInvoice.saleInvoiceStatus.Status;
                    System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(saleInvoice.saleInvoiceStatus.backcolor);
                    System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                    labeltopStatus.Foreground = new SolidColorBrush(newColor);
                    var rotateAnimation = new DoubleAnimation(-30, -32, TimeSpan.FromSeconds(60));
                    var rt = (RotateTransform)labeltopStatus.RenderTransform;
                    rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

                }
            if (saleInvoice.totaltaxAmount != 0)
            {
                isSalesTax.IsChecked = true;
                txttax.Text = saleInvoice.totaltaxAmount.ToString();
            }
            if(saleInvoice.isInterCompanyReceivable==true)
            {
                btnInterCompanyReceivable.IsChecked = true;
            }
            else
            {
                btnInterCompanyReceivable.IsChecked = false;
            }
            if (saleInvoice.insuranceRequired == true)
            {
                btnInsuranceRequired.IsChecked = true;
            }
            else
            {
                btnInsuranceRequired.IsChecked = false;
            }
            if (saleInvoice.insuranceApplied == true)
            {
                btnInsuranceApplied.IsChecked = true;
            }
            else
            {
                btnInsuranceApplied.IsChecked = false;
            }

            if (saleInvoice.insuranceNotApplicable == true)
            {
                btnInsuranceNotApplicable.IsChecked = true;
            }
            else
            {
                btnInsuranceNotApplicable.IsChecked = false;
            }


            if (saleInvoice.isAdvancePayment == true)
            {
                btnAdvancePayment.IsChecked = true;
            }
            else
            {
                btnAdvancePayment.IsChecked = false;
            }

            if (saleInvoice.transactionHolderId != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                    cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == saleInvoice.transactionHolderId))];

                }
                catch (Exception ex)
                {

                }
            }
            if (saleInvoice.holderChangeDate != null)
            {
                var time = DateTime.Now - saleInvoice.holderChangeDate;
                txtHolderDays.Text = time.Days.ToString();
            }
            if(saleInvoice.isSTLGenerated==true)
            {
                btnSTLGenerated.IsChecked = true;
            }
            else
            {
                btnSTLGenerated.IsChecked = false;
            }
            if (saleInvoice.isSTLDiscount == true)
            {
                btnSTLDiscounted.IsChecked = true;
            }
            else
            {
                btnSTLDiscounted.IsChecked = false;
            }
            loadVATBookReferenceNo();
            if (saleInvoice.VATBookRefId != 0 && saleInvoice.VATBookRefNumber != null)
            {
                var vatSource = (List<cmbitem>)cmbxVATBookRef.Items.SourceCollection;
                cmbxVATBookRef.SelectedItem = cmbxVATBookRef.Items[cmbxVATBookRef.Items.IndexOf(vatSource.Find(x => x.id == saleInvoice.VATBookRefId))];

            }

        }


        public List<BookerStatementItem> getBookerStatementItems()
        {
            List<BookerStatementItem> bookeritems = new List<BookerStatementItem>();
            List<BookerStatementItem> bookerProducts = new List<BookerStatementItem>();

            bookerProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;

            foreach (var item in bookerProducts)
            {
                if (editInvoice != 0)
                {
                    if (item.Id != 0)
                    {
                        item.saleInvoiceId = InvoiceId;
                        //if (item.saleOrder != null)
                        //{
                        //    item.saleOrder = null;
                        //}
                        //if (item.saleOrderId != null)
                        //{
                        //    item.saleOrderId = null;
                        //}
                        item.unit = item.unit;

                        item.amount = Math.Round(item.amount, 2);
                        item.amountGST = Math.Round(item.amountGST, 2);
                        item.focValue = Math.Round(item.focValue, 2); 
                        item.netAmount = Math.Round(item.netAmount, 2); 
                        item.passOnValue = Math.Round(item.passOnValue, 2);
                        item.weight = item.weight;
                        item.quantity = item.quantity;
                        item.claimDiscountValue = Math.Round(item.claimDiscountValue, 2);
                        item.focValue = Math.Round(item.focValue, 2);
                        item.passOnValue = Math.Round(item.passOnValue, 2);




                        item.siAmount = Math.Round(item.siAmount, 2);
                        item.siAmountGST = Math.Round(item.siAmountGST, 2);
                        item.siFocValue = Math.Round(item.siFocValue, 2);
                        item.siNetAmount = Math.Round(item.siNetAmount, 2);
                        item.siPassOnValue = Math.Round(item.siPassOnValue, 2);
                        item.siQuantity = item.siQuantity;
                        item.siWeight = item.siWeight;
                        item.siClaimDiscountValue = Math.Round(item.siClaimDiscountValue, 2);
                        item.siFocValue = Math.Round(item.siFocValue, 2);
                        item.siPassOnValue = Math.Round(item.siPassOnValue, 2);
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
                        item.saleInvoiceId = InvoiceId;
                        //if (item.saleOrder != null)
                        //{
                        //    item.saleOrder = null;
                        //}
                        //if (item.saleOrderId != null)
                        //{
                        //    item.saleOrderId = null;
                        //}
                        bItem.unit = item.unit;
                        bItem.amount = Math.Round(item.amount, 2);
                        bItem.amountGST = Math.Round(item.amountGST, 2);
                        bItem.netAmount = Math.Round(item.netAmount, 2);
                        bItem.weight = item.weight;
                        bItem.quantity = item.quantity;
                        bItem.passOnValue = Math.Round(item.passOnValue, 2);
                        bItem.claimDiscountValue = Math.Round(item.claimDiscountValue, 2);
                        bItem.focValue = Math.Round(item.focValue, 2);



                        bItem.siAmount = Math.Round(item.siAmount, 2);
                        bItem.siAmountGST = Math.Round(item.siAmountGST, 2);
                        bItem.siFocValue = Math.Round(item.siFocValue, 2);
                        bItem.siNetAmount = Math.Round(item.siNetAmount, 2);
                        bItem.siPassOnValue = Math.Round(item.siPassOnValue, 2);
                        bItem.siQuantity = item.siQuantity;
                        bItem.siWeight = item.siWeight;
                        bItem.siPassOnValue = Math.Round(item.siPassOnValue, 2);
                        bItem.siClaimDiscountValue = Math.Round(item.siClaimDiscountValue, 2);
                        bItem.siFocValue = Math.Round(item.siFocValue, 2);



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
                    item.saleInvoiceId = InvoiceId;
                    //if (item.saleOrder != null)
                    //{
                    //    item.saleOrder = null;
                    //}
                    //if (item.saleOrderId != null)
                    //{
                    //    item.saleOrderId = null;
                    //}
                    bItem.unit = item.unit;
                    bItem.amount = Math.Round(item.amount, 2);
                    bItem.amountGST = Math.Round(item.amountGST, 2);
                    bItem.netAmount = Math.Round(item.netAmount, 2);
                    bItem.weight = item.weight;
                    bItem.quantity = item.quantity;
                    bItem.passOnValue = Math.Round(item.passOnValue, 2);
                    bItem.claimDiscountValue = Math.Round(item.claimDiscountValue, 2);
                    bItem.focValue = Math.Round(item.focValue, 2);



                    bItem.siAmount = Math.Round(item.siAmount, 2);
                    bItem.siAmountGST = Math.Round(item.siAmountGST, 2);
                    bItem.siFocValue = Math.Round(item.siFocValue, 2);
                    bItem.siNetAmount = Math.Round(item.siNetAmount, 2);
                    bItem.siPassOnValue = Math.Round(item.siPassOnValue, 2);
                    bItem.siQuantity = item.siQuantity;
                    bItem.siWeight = item.siWeight;
                    bItem.siPassOnValue = Math.Round(item.siPassOnValue, 2);
                    bItem.siClaimDiscountValue = Math.Round(item.siClaimDiscountValue, 2);
                    bItem.siFocValue = Math.Round(item.siFocValue, 2);

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
            List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
            List<ProcurementProduct> SOProducts = new List<ProcurementProduct>();

            List<ProcurementProduct> saleInvoiceItems = new List<ProcurementProduct>();
            ProcurementProduct product = new ProcurementProduct();
            procurementProducts = grdInvoiceItems.ItemsSource as List<ProcurementProduct>;
            foreach (var procurementProduct in procurementProducts)
            {
                if (procurementProduct.Id == 0)
                {
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {

                            saleInvoiceItems.Add(new ProcurementProduct()
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
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                InvoicedQuantity = procurementProduct.InvoicedQuantity,
                                TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                InvoicedWeight = procurementProduct.InvoicedWeight,
                                TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,

                                totalCommission = procurementProduct.totalCommission,

                                value2 = procurementProduct.value2,
                                totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                NowAmount = procurementProduct.NowAmount,
                                priority = procurementProduct.priority
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                        }
                    }
                    else
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)

                            saleInvoiceItems.Add(new ProcurementProduct()
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
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                InvoicedQuantity = procurementProduct.InvoicedQuantity,
                                TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                InvoicedWeight = procurementProduct.InvoicedWeight,
                                TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,
                                totalCommission = procurementProduct.totalCommission,


                                value2 = procurementProduct.value2,
                                totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                NowAmount = procurementProduct.NowAmount,
                                priority = procurementProduct.priority
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                    }
                    if (saleOrder.products != null)

                        foreach (var SaleOrderProduct in saleOrder.products)
                        {
                            if (SaleOrderProduct.product_Id == procurementProduct.product_Id || SaleOrderProduct.inquiryProduct == procurementProduct.inquiryProduct)
                            {
                                //if (procurementProduct.inquiryProduct.Id == 0)
                                {
                                    if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                                    {
                                        SaleOrderProduct.UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity;
                                        SaleOrderProduct.TotalInvoicedQuantity = procurementProduct.inquiryProduct.quantity - procurementProduct.UnInvoicedQuantity;
                                        //SaleOrderProduct.InvoicedQuantity = procurementProduct.InvoicedQuantity;
                                        SaleOrderProduct.UnInvoicedWeight = procurementProduct.UnInvoicedWeight;
                                        SaleOrderProduct.TotalInvoicedWeight = Convert.ToDouble(procurementProduct.inquiryProduct.Weight) - procurementProduct.UnInvoicedWeight;

                                        SaleOrderProduct.UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount;
                                        SaleOrderProduct.totalInvoicedSoAmount = (procurementProduct.totalCommission == 0 || cmbSaleInvoiceType.SelectedItem.ToString() != InquiryType.Principal.ToString()) ? procurementProduct.value2 - procurementProduct.UnInvoicedSoAmount : procurementProduct.totalCommission - procurementProduct.UnInvoicedSoAmount;//SaleOrderProduct.InvoicedQuantity
                                                                                                                                                                                                                                                                                                                                                              //SOProducts.Add(SOpro);
                                    }
                                }
                            }
                        }
                }
                else
                {
                    // if user is adding completely new prodcut first time.
                    if (procurementProduct.inquiryProduct.Id == 0)
                    {
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {

                            saleInvoiceItems.Add(new ProcurementProduct()
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

                                        isActive = procurementProduct.inquiryProduct.product.isActive
                                    }
                                },
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                InvoicedQuantity = procurementProduct.InvoicedQuantity,
                                TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                InvoicedWeight = procurementProduct.InvoicedWeight,
                                TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,
                                totalCommission = procurementProduct.totalCommission,


                                value2 = procurementProduct.value2,
                                totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                NowAmount = procurementProduct.NowAmount,
                                priority = procurementProduct.priority
                                //caption2 = cmbcaption2.Text.Trim()


                            });
                        }
                    }
                    else
                    {
                        // if user reloaded he saleOrder and its product item is already there.
                        if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                        {
                            saleInvoiceItems.Add(new ProcurementProduct()
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
                                Id= procurementProduct.Id,
                                product_Id = procurementProduct.inquiryProduct.Id,
                                unitPrice = procurementProduct.unitPrice,
                                UnInvoicedQuantity = procurementProduct.UnInvoicedQuantity,
                                InvoicedQuantity = procurementProduct.InvoicedQuantity,
                                TotalInvoicedQuantity = procurementProduct.TotalInvoicedQuantity,
                                UnInvoicedWeight = procurementProduct.UnInvoicedWeight,
                                InvoicedWeight = procurementProduct.InvoicedWeight,
                                TotalInvoicedWeight = procurementProduct.TotalInvoicedWeight,

                                totalCommission = procurementProduct.totalCommission,

                                value2 = procurementProduct.value2,
                                totalInvoicedSoAmount = procurementProduct.totalInvoicedSoAmount,
                                UnInvoicedSoAmount = procurementProduct.UnInvoicedSoAmount,
                                NowAmount = procurementProduct.NowAmount,
                                priority = procurementProduct.priority
                                //caption2 = cmbcaption2.Text.Trim()


                            });

                        }
                    }

                    if (saleOrder.products != null && saleInvoice.products != null)
                        foreach (var SaleInvoiceProduct in saleInvoice.products)
                        {
                            if (SaleInvoiceProduct.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id)
                                foreach (var SaleOrderProduct in saleOrder.products)
                                {
                                    if (SaleOrderProduct.inquiryProduct.product_Id == procurementProduct.inquiryProduct.product_Id || SaleOrderProduct.inquiryProduct == procurementProduct.inquiryProduct)
                                    {
                                        //if (procurementProduct.inquiryProduct.Id == 0)
                                        {
                                            if (procurementProduct.inquiryProduct.product != null && procurementProduct.inquiryProduct.product.Id != 0)
                                            {
                                                SaleOrderProduct.UnInvoicedQuantity = SaleOrderProduct.UnInvoicedQuantity + SaleInvoiceProduct.InvoicedQuantity - procurementProduct.InvoicedQuantity;
                                                SaleOrderProduct.TotalInvoicedQuantity = SaleOrderProduct.TotalInvoicedQuantity - SaleInvoiceProduct.InvoicedQuantity + procurementProduct.InvoicedQuantity;
                                                //SaleOrderProduct.InvoicedQuantity = procurementProduct.InvoicedQuantity;
                                                SaleOrderProduct.UnInvoicedWeight = SaleOrderProduct.UnInvoicedWeight + SaleInvoiceProduct.InvoicedWeight - procurementProduct.InvoicedWeight;
                                                SaleOrderProduct.TotalInvoicedWeight = SaleOrderProduct.TotalInvoicedWeight - SaleInvoiceProduct.InvoicedWeight + procurementProduct.InvoicedWeight;

                                                SaleOrderProduct.UnInvoicedSoAmount = SaleOrderProduct.totalInvoicedSoAmount + SaleInvoiceProduct.NowAmount - procurementProduct.NowAmount;
                                                SaleOrderProduct.totalInvoicedSoAmount = SaleOrderProduct.totalInvoicedSoAmount - SaleInvoiceProduct.NowAmount + procurementProduct.NowAmount;
                                            }
                                        }
                                    }
                                }
                        }
                }
            }


            return saleInvoiceItems;
        }

        public List<JournalTransaction> getJournalTransactions()
        {
            journalTransactions = new List<JournalTransaction>();
            ProductRepo productrepo = new ProductRepo();
            if (saleInvoice.journalTransactions == null || saleInvoice.journalTransactions.Count==0)
            {
                var procurementProducts = grdInvoiceItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.inquiryProduct.product.Id);
                    if(dbProduct.productType==ProductType.Inventory && (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.Inventory)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbProduct.incomeAccount != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Income
                                       )
                                {
                                    total = procurementProduct.NowAmount;
                                }
                                else
                                {
                                    total = 0 - procurementProduct.NowAmount;
                                }



                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.incomeAccount.Id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = procurementProduct.NowAmount,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }
                        }
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);

                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbProduct.cgsInvenAccount_id != null)
                            {
                                double total = 0;
                                if (
                                        dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                        dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                        dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                        dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                        dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       department.ChartofAccount.accountType == COA_AccountType.Income ||
                                       department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total =0- averageCost * procurementProduct.InvoicedQuantity;
                                }
                                else
                                {
                                    total = averageCost * procurementProduct.InvoicedQuantity - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.cgsInvenAccount_id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    //debit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                    debit = averageCost * procurementProduct.InvoicedQuantity,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2)- 0,
                                    total = total ,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }
                        }
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbProduct.cAssetAccount_id != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                        dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                        dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                        dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                        dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total =  averageCost * procurementProduct.InvoicedQuantity;
                                }
                                else
                                {
                                    total = 0 - averageCost * procurementProduct.InvoicedQuantity;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.cAssetAccount_id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    //credit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                    credit = averageCost * procurementProduct.InvoicedQuantity,
                                    deptId = department.Id,
                                    //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id
                                });
                            }
                        }
                    }
                    else
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbProduct.incomeAccount != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                      dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = procurementProduct.NowAmount;
                                }
                                else
                                {
                                    total = 0 - procurementProduct.NowAmount;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.incomeAccount.Id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = procurementProduct.NowAmount,
                                    deptId = department.Id,
                                    total =total ,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });

                            }
                        }
                    }
                   
                }
                if (department.ChartofAccount != null)
                {
                    if (btnPushDebits.IsChecked == true)
                    {
                        double total = 0;
                        if (
                               department.ChartofAccount.accountType == COA_AccountType.Loan ||
                               department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                               department.ChartofAccount.accountType == COA_AccountType.Equity ||
                               department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                               department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                               department.ChartofAccount.accountType == COA_AccountType.Income ||
                               department.ChartofAccount.accountType == COA_AccountType.Other_Income 
                               )
                        {
                            total =0- Convert.ToDouble(txttotalcfr.Text) ;
                        }
                        else
                        {
                            total = Convert.ToDouble(txttotalcfr.Text) - 0;
                        }
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = department.chartofAccountId,
                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                            SaleInvoiceId = saleInvoice.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                            transactionRefno = txtFinanaceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = Convert.ToDouble(txttotalcfr.Text),
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            credit = 0,
                            deptId = department.Id,
                            total = total,
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                        });
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please contact admin to attach receivable account with department", "Approval Required", MessageBoxButton.OK);
                    //return null;
                }

                if (lookupSalesTax.SelectedIndex != -1)
                {
                    var saleInvoiceTax = taxRepo.getTaxtById((lookupSalesTax.SelectedItem as TaxName).Id);

                    if (saleOrderTax.chartofAccount != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            double total = 0;
                            if (
                                   saleOrderTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                   saleOrderTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                   saleOrderTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                   saleOrderTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                   saleOrderTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   saleOrderTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   saleOrderTax.chartofAccount.accountType == COA_AccountType.Income ||
                                   saleOrderTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total =  Convert.ToDouble(txttax.Text);
                            }
                            else
                            {
                                total = 0 - Convert.ToDouble(txttax.Text);
                            }
                            JournalTransaction taxTransaction = new JournalTransaction();
                            {
                                taxTransaction.accountId = saleOrderTax.COA_Id;
                                taxTransaction.credit = Convert.ToDouble(txttax.Text);
                                taxTransaction.deptId = department.Id;
                                taxTransaction.SaleInvoiceId = saleInvoice.Id;
                                taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                taxTransaction.userId = saleInvoice.user_Id;
                                taxTransaction.coaTransactionsType = coaTransactionsType.SaleInvoice;
                                taxTransaction.MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                                taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                taxTransaction.total = total;
                                taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                taxTransaction.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                            }

                            journalTransactions.Add(taxTransaction);
                        }
                    }

                }
                
            }
            else
            {
                var procurementProducts = grdInvoiceItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.inquiryProduct.product.Id);

                    if (dbProduct.productType == ProductType.Inventory && (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.Inventory)
                    {
                        if (dbProduct.incomeAccount != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x => x.accountId ==
                                dbProduct.incomeAccount.Id &&
                                x.credit == procurementProduct.NowAmount &&
                                x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                x.deptId == department.Id

                                );

                                if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = procurementProduct.NowAmount;
                                    }
                                    else
                                    {
                                        total = 0 - procurementProduct.NowAmount;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.incomeAccount.Id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = procurementProduct.NowAmount,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                        isReconciled=false,
                                        reconcilationDate=null,
                                        ReconcilationId=null,
                                        reconcilationType=ReconcilationType.Uncleared_Transactions
                                    });
                                }
                                else
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = procurementProduct.NowAmount;
                                    }
                                    else
                                    {
                                        total = 0 - procurementProduct.NowAmount;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.incomeAccount.Id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = procurementProduct.NowAmount,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                        reconcilationDate=dbTransaction.reconcilationDate,
                                        reconcilationType=dbTransaction.reconcilationType,
                                        ReconcilationId=dbTransaction.ReconcilationId,
                                        isReconciled=dbTransaction.isReconciled
                                    });
                                }

                               
                            }
                        }
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        if (dbProduct.cgsInvenAccount_id != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x => 
                                x.accountId == dbProduct.cgsInvenAccount_id &&
                               x.debit == averageCost * procurementProduct.InvoicedQuantity &&
                               x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                               x.deptId == department.Id

                               );

                                if(dbTransaction==null)
                                {
                                    double total = 0;
                                    if (
                                            dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                            dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                            dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                            dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                            dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                            dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                            dbProduct.cgsInvenAccount.accountType == COA_AccountType.Income ||
                                            dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = averageCost * procurementProduct.InvoicedQuantity;
                                    }
                                    else
                                    {
                                        total = averageCost * procurementProduct.InvoicedQuantity - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cgsInvenAccount_id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        //debit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                        debit = averageCost * procurementProduct.InvoicedQuantity,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = 0,
                                        deptId = department.Id,
                                        //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) - 0,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Income ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = averageCost * procurementProduct.InvoicedQuantity;
                                        }
                                        else
                                        {
                                            total = averageCost * procurementProduct.InvoicedQuantity - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsInvenAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            //debit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            debit = averageCost * procurementProduct.InvoicedQuantity,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) - 0,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions

                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Income ||
                                                dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = averageCost * procurementProduct.InvoicedQuantity;
                                        }
                                        else
                                        {
                                            total = averageCost * procurementProduct.InvoicedQuantity - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsInvenAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            //debit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            debit = averageCost * procurementProduct.InvoicedQuantity,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) - 0,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled

                                        });
                                    }
                                }


                              

                                  
                            }
                        }
                        if (dbProduct.cAssetAccount_id != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {

                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                    x.accountId == dbProduct.cAssetAccount_id &&
                                    x.credit == averageCost * procurementProduct.InvoicedQuantity &&
                                    x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                    x.deptId == department.Id
                                    );

                                if (dbTransaction == null)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Income||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = averageCost * procurementProduct.InvoicedQuantity;
                                    }
                                    else
                                    {
                                        total = 0 - averageCost * procurementProduct.InvoicedQuantity;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cAssetAccount_id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        //credit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                        credit = averageCost * procurementProduct.InvoicedQuantity,
                                        deptId = department.Id,
                                        //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = averageCost * procurementProduct.InvoicedQuantity;
                                        }
                                        else
                                        {
                                            total = 0 - averageCost * procurementProduct.InvoicedQuantity;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            //credit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            credit = averageCost * procurementProduct.InvoicedQuantity,
                                            deptId = department.Id,
                                            //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions

                                        });

                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = averageCost * procurementProduct.InvoicedQuantity;
                                        }
                                        else
                                        {
                                            total = 0 - averageCost * procurementProduct.InvoicedQuantity;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            //credit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            credit = averageCost * procurementProduct.InvoicedQuantity,
                                            deptId = department.Id,
                                            //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }
                                   
                            }
                        }
                    }
                    else
                   if (dbProduct.incomeAccount != null )
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                   x.accountId == dbProduct.incomeAccount.Id &&
                                   x.credit == procurementProduct.NowAmount &&
                                   x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                   x.deptId == department.Id
                                   );

                            if(dbTransaction==null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = procurementProduct.NowAmount;
                                }
                                else
                                {
                                    total = 0 - procurementProduct.NowAmount;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.incomeAccount.Id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    credit = procurementProduct.NowAmount,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                });
                            }
                            else
                            {
                                if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = procurementProduct.NowAmount;
                                    }
                                    else
                                    {
                                        total = 0 - procurementProduct.NowAmount;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.incomeAccount.Id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        debit = 0,
                                        credit = procurementProduct.NowAmount,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                        isReconciled = false,
                                        reconcilationDate = null,
                                        ReconcilationId = null,
                                        reconcilationType = ReconcilationType.Uncleared_Transactions

                                    });
                                }
                                else
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = procurementProduct.NowAmount;
                                    }
                                    else
                                    {
                                        total = 0 - procurementProduct.NowAmount;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.incomeAccount.Id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        debit = 0,
                                        credit = procurementProduct.NowAmount,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                        reconcilationDate = dbTransaction.reconcilationDate,
                                        reconcilationType = dbTransaction.reconcilationType,
                                        ReconcilationId = dbTransaction.ReconcilationId,
                                        isReconciled = dbTransaction.isReconciled

                                    });
                                }
                            }

                            
                        }

                    }
                    receivableTotal = receivableTotal + procurementProduct.NowAmount;
                }
                if (department.ChartofAccount != null)
                {
                    if (btnPushDebits.IsChecked == true)
                    {
                        var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                    x.accountId == department.chartofAccountId &&
                                    x.debit == Convert.ToDouble(txttotalcfr.Text) &&
                                    x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                    x.deptId == department.Id
                                    );

                        if (dbTransaction == null)
                        {
                            double total = 0;
                            if (
                                   department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                   department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                   department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                   department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                   department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   department.ChartofAccount.accountType == COA_AccountType.Income ||
                                   department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = 0- Convert.ToDouble(txttotalcfr.Text) ;
                            }
                            else
                            {
                                total=Convert.ToDouble(txttotalcfr.Text) - 0;
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                accountId = department.chartofAccountId,
                                coaTransactionsType = coaTransactionsType.SaleInvoice,
                                SaleInvoiceId = saleInvoice.Id,
                                creationDate = (DateTime)datglPostingdate.EditValue,
                                //memo = procurementProduct.inquiryProduct.ownDiscription,
                                transactionRefno = txtFinanaceRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = Convert.ToDouble(txttotalcfr.Text),
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                credit = 0,
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                            });
                        }
                        else
                        {
                            if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                       department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       department.ChartofAccount.accountType == COA_AccountType.Income ||
                                       department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - Convert.ToDouble(txttotalcfr.Text);
                                }
                                else
                                {
                                    total = Convert.ToDouble(txttotalcfr.Text) - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = department.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = Convert.ToDouble(txttotalcfr.Text),
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    isReconciled = false,
                                    reconcilationDate = null,
                                    ReconcilationId = null,
                                    reconcilationType = ReconcilationType.Uncleared_Transactions

                                });
                            }
                            else
                            {
                                double total = 0;
                                if (
                                       department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       department.ChartofAccount.accountType == COA_AccountType.Income ||
                                       department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - Convert.ToDouble(txttotalcfr.Text);
                                }
                                else
                                {
                                    total = Convert.ToDouble(txttotalcfr.Text) - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = department.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = Convert.ToDouble(txttotalcfr.Text),
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    reconcilationDate = dbTransaction.reconcilationDate,
                                    reconcilationType = dbTransaction.reconcilationType,
                                    ReconcilationId = dbTransaction.ReconcilationId,
                                    isReconciled = dbTransaction.isReconciled

                                });
                            }
                        }
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please contact admin to attach receivable account with department", "Approval Required", MessageBoxButton.OK);
                    //return null;
                }
                if (lookupSalesTax.SelectedIndex != -1)
                {
                    var saleInvoiceTax = taxRepo.getTaxtById((lookupSalesTax.SelectedItem as TaxName).Id);
                    if (saleOrderTax.chartofAccount != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                   x.accountId == saleOrderTax.COA_Id &&
                                   x.credit == Convert.ToDouble(txttax.Text) &&
                                   x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                   x.deptId == department.Id
                                   );

                            if (dbTransaction == null)
                            {
                                double total = 0;
                                if (
                                      saleOrderTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                       saleOrderTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       saleOrderTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                       saleOrderTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       saleOrderTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       saleOrderTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       saleOrderTax.chartofAccount.accountType == COA_AccountType.Income ||
                                       saleOrderTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = Convert.ToDouble(txttax.Text);
                                }
                                else
                                {
                                    total = 0 - Convert.ToDouble(txttax.Text);
                                }
                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = saleOrderTax.COA_Id;
                                    taxTransaction.credit = Convert.ToDouble(txttax.Text);
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.SaleInvoiceId = saleInvoice.Id;
                                    taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                    taxTransaction.userId = saleInvoice.user_Id;
                                    taxTransaction.coaTransactionsType = coaTransactionsType.SaleInvoice;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                                    taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    taxTransaction.total = total;
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                               

                                }
                                journalTransactions.Add(taxTransaction);
                            }
                            else
                            {
                                if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                {
                                    double total = 0;
                                    if (
                                          saleOrderTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Income ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = Convert.ToDouble(txttax.Text);
                                    }
                                    else
                                    {
                                        total = 0 - Convert.ToDouble(txttax.Text);
                                    }
                                    JournalTransaction taxTransaction = new JournalTransaction();
                                    {
                                        taxTransaction.accountId = saleOrderTax.COA_Id;
                                        taxTransaction.credit = Convert.ToDouble(txttax.Text);
                                        taxTransaction.deptId = department.Id;
                                        taxTransaction.SaleInvoiceId = saleInvoice.Id;
                                        taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                        taxTransaction.userId = saleInvoice.user_Id;
                                        taxTransaction.coaTransactionsType = coaTransactionsType.SaleInvoice;
                                        taxTransaction.MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                                        taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        taxTransaction.total = total;
                                        taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        taxTransaction.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                                        taxTransaction.isReconciled = false;
                                        taxTransaction.reconcilationDate = null;
                                        taxTransaction.ReconcilationId = null;
                                        taxTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;

                                    }
                                    journalTransactions.Add(taxTransaction);

                                }
                                else
                                {
                                    double total = 0;
                                    if (
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Loan ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Equity ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Income ||
                                           saleOrderTax.chartofAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = Convert.ToDouble(txttax.Text);
                                    }
                                    else
                                    {
                                        total = 0 - Convert.ToDouble(txttax.Text);
                                    }
                                    JournalTransaction taxTransaction = new JournalTransaction();
                                    {
                                        taxTransaction.accountId = saleOrderTax.COA_Id;
                                        taxTransaction.credit = Convert.ToDouble(txttax.Text);
                                        taxTransaction.deptId = department.Id;
                                        taxTransaction.SaleInvoiceId = saleInvoice.Id;
                                        taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                        taxTransaction.userId = saleInvoice.user_Id;
                                        taxTransaction.coaTransactionsType = coaTransactionsType.SaleInvoice;
                                        taxTransaction.MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                                        taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        taxTransaction.total = total;
                                        taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        taxTransaction.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                                        taxTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                        taxTransaction.reconcilationType = dbTransaction.reconcilationType;
                                        taxTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                        taxTransaction.isReconciled = dbTransaction.isReconciled;

                                    }
                                    journalTransactions.Add(taxTransaction);

                                }
                            }



                        }
                    }
                }
            }
            return journalTransactions;
        }

        public List<JournalTransaction> getBookerJournalTransactions()
        {
            journalTransactions = new List<JournalTransaction>();
            ProductRepo productrepo = new ProductRepo();
            if (saleInvoice.journalTransactions == null || saleInvoice.journalTransactions.Count == 0)
            {
                var procurementProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory && (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbProduct.incomeAccount != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                      dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total = procurementProduct.siAmount;
                                }
                                else
                                {
                                    total = 0 - procurementProduct.siAmount;
                                }




                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.incomeAccount.Id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = procurementProduct.siAmount,
                                    deptId = department.Id,
                                    total =total ,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }
                        }
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbProduct.cgsInvenAccount_id != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                      dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                      dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total = 0-averageCost * procurementProduct.siQuantity ;
                                }
                                else
                                {
                                    total = averageCost * procurementProduct.siQuantity - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.cgsInvenAccount_id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = averageCost * procurementProduct.siQuantity,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2)- 0,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }
                        }
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbProduct.cAssetAccount_id != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total = averageCost * procurementProduct.siQuantity;
                                }
                                else
                                {
                                    total = 0 - averageCost * procurementProduct.siQuantity;
                                }


                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.cAssetAccount_id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    //credit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                    credit = averageCost * procurementProduct.siQuantity,
                                    deptId = department.Id,
                                    //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id
                                });
                            }
                        }
                    }
                    else
                    if (dbProduct.productType == ProductType.Inventory && (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbProduct.incomeAccount != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                      dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total = procurementProduct.siAmount;
                                }
                                else
                                {
                                    total = 0 - procurementProduct.siAmount;
                                }




                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.incomeAccount.Id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = procurementProduct.siAmount,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }
                        }
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        if (btnPushDebits.IsChecked == true)
                        {
                            if (dbProduct.cgsInvenAccount_id != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                      dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                      dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total = 0 - averageCost * procurementProduct.siQuantity;
                                }
                                else
                                {
                                    total = averageCost * procurementProduct.siQuantity - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.cgsInvenAccount_id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = averageCost * procurementProduct.siQuantity,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2)- 0,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }
                        }
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbProduct.cAssetAccount_id != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total = averageCost * procurementProduct.siQuantity;
                                }
                                else
                                {
                                    total = 0 - averageCost * procurementProduct.siQuantity;
                                }


                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.cAssetAccount_id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    //credit = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                    credit = averageCost * procurementProduct.siQuantity,
                                    deptId = department.Id,
                                    //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id
                                });
                            }
                        }
                    }
                    else
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            if (dbProduct.incomeAccount != null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total =procurementProduct.siQuantity;
                                }
                                else
                                {
                                    total = 0 - procurementProduct.siAmount;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.incomeAccount.Id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = procurementProduct.siAmount,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });

                            }
                        }
                    }

                }
                if (department.ChartofAccount != null)
                {
                    if (btnPushDebits.IsChecked == true)
                    {
                        if (procurementProducts[0].TaxName != null)
                        {
                            var gstAccount = procurementProducts[0].TaxName.COA_Id;
                        }
                        var totalGST = procurementProducts.Sum(x => x.siAmountGST);
                        if (procurementProducts[0].PassOn != null)
                        {
                            var passOnAccount = procurementProducts[0].PassOn.chartofAccountId;
                            var totalPassOn = procurementProducts.Sum(x => x.siPassOnValue);

                            if (passOnAccount != null)
                            {
                                double total1 = 0;
                                if (
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total1 =0- totalPassOn;
                                }
                                else
                                {
                                    total1 = totalPassOn - 0;
                                }


                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = passOnAccount,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = totalPassOn,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    total =total1,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }
                        }
                        if (procurementProducts[0].ClaimDiscount != null)
                        {
                            var claimDiscountAccount = procurementProducts[0].ClaimDiscount.chartofAccountId;
                            var totalClaimDiscount = procurementProducts.Sum(x => x.siClaimDiscountValue);

                            if (claimDiscountAccount != null)
                            {
                                double total1 = 0;
                                if (
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total1 = 0 - totalClaimDiscount;
                                }
                                else
                                {
                                    total1= totalClaimDiscount - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = claimDiscountAccount,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = totalClaimDiscount,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    total = total1,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }

                        }
                        if (procurementProducts[0].FOCSampling != null)
                        {
                            var focSamplingAccount = procurementProducts[0].FOCSampling.chartofAccountId;
                            var totalFocSampling = procurementProducts.Sum(x => x.siFocValue);

                            if (focSamplingAccount != null)
                            {
                                double total1 = 0;

                                if (
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total1 = 0 - totalFocSampling;
                                }
                                else
                                {
                                    total1 = totalFocSampling - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = focSamplingAccount,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = totalFocSampling,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    total = total1,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }

                        }
                        double total = 0;
                        if (
                               department.ChartofAccount.accountType == COA_AccountType.Loan ||
                               department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                               department.ChartofAccount.accountType == COA_AccountType.Equity ||
                               department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                               department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                               )
                        {
                            total = 0 - Convert.ToDouble(txttotalcfr.Text);
                        }
                        else
                        {
                            total = Convert.ToDouble(txttotalcfr.Text) - 0;
                        }
                        journalTransactions.Add(new JournalTransaction()
                        {
                            accountId = department.chartofAccountId,
                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                            SaleInvoiceId = saleInvoice.Id,
                            creationDate = (DateTime)datglPostingdate.EditValue,
                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                            transactionRefno = txtFinanaceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            debit = Convert.ToDouble(txttotalcfr.Text),
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            credit = 0,
                            deptId = department.Id,
                            total =total,
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                        });
                       

                    }
                    if (btnPushCredits.IsChecked == true)
                    {
                        if (procurementProducts[0].TaxName != null)
                        {
                            var gstAccount = procurementProducts[0].TaxName.COA_Id;
                            var totalGST = procurementProducts.Sum(x => x.siAmountGST);
                            if (gstAccount != null)
                            {
                                double total = 0;
                                if (
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Loan ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Equity ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total = totalGST;
                                }
                                else
                                {
                                    total = 0 - totalGST;
                                }
                                JournalTransaction taxTransaction = new JournalTransaction();
                                {
                                    taxTransaction.accountId = gstAccount;
                                    taxTransaction.credit = totalGST;
                                    taxTransaction.deptId = department.Id;
                                    taxTransaction.SaleInvoiceId = saleInvoice.Id;
                                    taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                    taxTransaction.userId = saleInvoice.user_Id;
                                    taxTransaction.coaTransactionsType = coaTransactionsType.SaleInvoice;
                                    taxTransaction.MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                                    taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                    taxTransaction.total = total;
                                    taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                    taxTransaction.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                                }
                                journalTransactions.Add(taxTransaction);
                            }


                        }
                        if (procurementProducts[0].PassOn != null)
                        {
                            var passOnAccount = procurementProducts[0].PassOn.chartofAccountId;
                            var totalPassOn = procurementProducts.Sum(x => x.siPassOnValue);
                            if (passOnAccount != null)
                            {
                                double total = 0;
                                if (
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                       
                                       )
                                {
                                    total = totalPassOn;
                                }
                                else
                                {
                                    total = 0 - totalPassOn;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = passOnAccount,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = totalPassOn,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }

                        }
                        if (procurementProducts[0].ClaimDiscount != null)
                        {
                            var claimDiscountAccount = procurementProducts[0].ClaimDiscount.chartofAccountId;
                            var totalClaimDiscount = procurementProducts.Sum(x => x.siClaimDiscountValue);
                            if (claimDiscountAccount != null)
                            {

                                double total = 0;
                                if (
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total = totalClaimDiscount;
                                }
                                else
                                {
                                    total = 0 - totalClaimDiscount;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = claimDiscountAccount,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = totalClaimDiscount,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }

                        }
                        if (procurementProducts[0].FOCSampling != null)
                        {
                            var focSamplingAccount = procurementProducts[0].FOCSampling.chartofAccountId;
                            var totalFocSampling = procurementProducts.Sum(x => x.siFocValue);
                            if (focSamplingAccount != null)
                            {
                                double total = 0;
                                if (
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                       )
                                {
                                    total = totalFocSampling;
                                }
                                else
                                {
                                    total = 0 - totalFocSampling;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = focSamplingAccount,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = 0,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = totalFocSampling,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id

                                });
                            }


                        }
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please contact admin to attach receivable account with department", "Approval Required", MessageBoxButton.OK);
                }
            }
            else
            {
                var procurementProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.product.Id);

                    if (dbProduct.productType == ProductType.Inventory && (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
                    {
                        if (dbProduct.incomeAccount != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                x.accountId == dbProduct.incomeAccount.Id &&
                                x.credit == procurementProduct.siAmount &&
                                x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                x.deptId == department.Id
                                );
                                if (dbTransaction != null)
                                {


                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = procurementProduct.siAmount;
                                        }
                                        else
                                        {
                                            total = 0 - procurementProduct.siAmount;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.incomeAccount.Id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = procurementProduct.siAmount,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions
                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = procurementProduct.siAmount;
                                        }
                                        else
                                        {
                                            total = 0 - procurementProduct.siAmount;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.incomeAccount.Id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = procurementProduct.siAmount,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }
                                else
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = procurementProduct.siAmount;
                                    }
                                    else
                                    {
                                        total = 0 - procurementProduct.siAmount;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.incomeAccount.Id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = procurementProduct.siAmount,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    });
                                }
                            }
                        }
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        if (dbProduct.cgsInvenAccount_id != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                x.accountId == dbProduct.cgsInvenAccount_id &&
                                x.debit == averageCost * procurementProduct.siQuantity &&
                                x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                x.deptId == department.Id
                                );
                                if(dbTransaction==null)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = 0 - averageCost * procurementProduct.siQuantity;
                                    }
                                    else
                                    {
                                        total = averageCost * procurementProduct.siQuantity - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cgsInvenAccount_id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,

                                        debit = averageCost * procurementProduct.siQuantity,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = 0,
                                        deptId = department.Id,
                                        //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) - 0,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - averageCost * procurementProduct.siQuantity;
                                        }
                                        else
                                        {
                                            total = averageCost * procurementProduct.siQuantity - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsInvenAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,

                                            debit = averageCost * procurementProduct.siQuantity,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) - 0,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions

                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - averageCost * procurementProduct.siQuantity;
                                        }
                                        else
                                        {
                                            total = averageCost * procurementProduct.siQuantity - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsInvenAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,

                                            debit = averageCost * procurementProduct.siQuantity,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) - 0,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled

                                        });
                                    }
                                }

                                
                            }
                        }
                        if (dbProduct.cAssetAccount_id != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                               x.accountId == dbProduct.cAssetAccount_id &&
                               x.credit == averageCost * procurementProduct.siQuantity &&
                               x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                               x.deptId == department.Id
                               );

                                if(dbTransaction==null)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = averageCost * procurementProduct.siQuantity;
                                    }
                                    else
                                    {
                                        total = 0 - averageCost * procurementProduct.siQuantity;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cAssetAccount_id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = averageCost * procurementProduct.siQuantity,
                                        deptId = department.Id,
                                        //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = averageCost * procurementProduct.siQuantity;
                                        }
                                        else
                                        {
                                            total = 0 - averageCost * procurementProduct.siQuantity;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = averageCost * procurementProduct.siQuantity,
                                            deptId = department.Id,
                                            //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions
                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = averageCost * procurementProduct.siQuantity;
                                        }
                                        else
                                        {
                                            total = 0 - averageCost * procurementProduct.siQuantity;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = averageCost * procurementProduct.siQuantity,
                                            deptId = department.Id,
                                            //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }
                            
                            }
                        }
                    }
                    else
                     if (dbProduct.productType == ProductType.Inventory && (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                    {
                        if (dbProduct.incomeAccount != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                x.accountId == dbProduct.incomeAccount.Id &&
                                x.credit == procurementProduct.siAmount &&
                                x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                x.deptId == department.Id
                                );
                                if(dbTransaction==null)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = procurementProduct.siAmount;
                                    }
                                    else
                                    {
                                        total = 0 - procurementProduct.siAmount;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.incomeAccount.Id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = procurementProduct.siAmount,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                      
                                    });

                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = procurementProduct.siAmount;
                                        }
                                        else
                                        {
                                            total = 0 - procurementProduct.siAmount;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.incomeAccount.Id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = procurementProduct.siAmount,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions
                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = procurementProduct.siAmount;
                                        }
                                        else
                                        {
                                            total = 0 - procurementProduct.siAmount;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.incomeAccount.Id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = procurementProduct.siAmount,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }
                                
                            }
                        }
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        if (dbProduct.cgsInvenAccount_id != null)
                        {
                            if (btnPushDebits.IsChecked == true)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                x.accountId == dbProduct.cgsInvenAccount_id &&
                                x.debit == averageCost * procurementProduct.siQuantity &&
                                x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                x.deptId == department.Id
                                );
                                if(dbTransaction==null )
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = 0 - averageCost * procurementProduct.siQuantity;
                                    }
                                    else
                                    {
                                        total = averageCost * procurementProduct.siQuantity - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cgsInvenAccount_id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,

                                        debit = averageCost * procurementProduct.siQuantity,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = 0,
                                        deptId = department.Id,
                                        //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) - 0,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - averageCost * procurementProduct.siQuantity;
                                        }
                                        else
                                        {
                                            total = averageCost * procurementProduct.siQuantity - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsInvenAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,

                                            debit = averageCost * procurementProduct.siQuantity,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) - 0,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions

                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cgsInvenAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - averageCost * procurementProduct.siQuantity;
                                        }
                                        else
                                        {
                                            total = averageCost * procurementProduct.siQuantity - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cgsInvenAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,

                                            debit = averageCost * procurementProduct.siQuantity,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            //total = Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) - 0,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled

                                        });
                                    }
                                }

                              
                            }
                        }
                        if (dbProduct.cAssetAccount_id != null)
                        {
                            if (btnPushCredits.IsChecked == true)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                               x.accountId == dbProduct.cAssetAccount_id &&
                               x.credit == averageCost * procurementProduct.siQuantity &&
                               x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                               x.deptId == department.Id
                               );
                                if (dbTransaction == null)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = averageCost * procurementProduct.siQuantity;
                                    }
                                    else
                                    {
                                        total = 0 - averageCost * procurementProduct.siQuantity;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.cAssetAccount_id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = averageCost * procurementProduct.siQuantity,
                                        deptId = department.Id,
                                        //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                      
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = averageCost * procurementProduct.siQuantity;
                                        }
                                        else
                                        {
                                            total = 0 - averageCost * procurementProduct.siQuantity;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = averageCost * procurementProduct.siQuantity,
                                            deptId = department.Id,
                                            //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions
                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Loan ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Credit_Card ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Equity ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Income ||
                                               dbProduct.cAssetAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = averageCost * procurementProduct.siQuantity;
                                        }
                                        else
                                        {
                                            total = 0 - averageCost * procurementProduct.siQuantity;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            prodId = dbProduct.Id,
                                            accountId = dbProduct.cAssetAccount_id,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = averageCost * procurementProduct.siQuantity,
                                            deptId = department.Id,
                                            //total = 0 - Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }
                             
                            }
                        }
                    }
                    else
                   if (dbProduct.incomeAccount != null)
                    {
                        if (btnPushCredits.IsChecked == true)
                        {
                            var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                              x.accountId == dbProduct.incomeAccount.Id &&
                              x.credit == procurementProduct.siAmount &&
                              x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                              x.deptId == department.Id
                              );
                            if(dbTransaction==null)
                            {
                                double total = 0;
                                if (
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                       dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = procurementProduct.siAmount;
                                }
                                else
                                {
                                    total = 0 - procurementProduct.siAmount;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    prodId = dbProduct.Id,
                                    accountId = dbProduct.incomeAccount.Id,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    debit = 0,
                                    credit = procurementProduct.siAmount,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                });
                            }
                            else
                            {
                                if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = procurementProduct.siAmount;
                                    }
                                    else
                                    {
                                        total = 0 - procurementProduct.siAmount;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.incomeAccount.Id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        debit = 0,
                                        credit = procurementProduct.siAmount,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                        isReconciled = false,
                                        reconcilationDate = null,
                                        ReconcilationId = null,
                                        reconcilationType = ReconcilationType.Uncleared_Transactions

                                    });
                                }
                                else
                                {
                                    double total = 0;
                                    if (
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Loan ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Credit_Card ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Equity ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Income ||
                                           dbProduct.incomeAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = procurementProduct.siAmount;
                                    }
                                    else
                                    {
                                        total = 0 - procurementProduct.siAmount;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        prodId = dbProduct.Id,
                                        accountId = dbProduct.incomeAccount.Id,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        debit = 0,
                                        credit = procurementProduct.siAmount,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                        reconcilationDate = dbTransaction.reconcilationDate,
                                        reconcilationType = dbTransaction.reconcilationType,
                                        ReconcilationId = dbTransaction.ReconcilationId,
                                        isReconciled = dbTransaction.isReconciled

                                    });
                                }
                            }
                        
                        }

                    }
                    receivableTotal = receivableTotal + procurementProduct.siAmount;
                }
                if (department.ChartofAccount != null)
                {
                    if (btnPushDebits.IsChecked == true)
                    {
                       
                        if(procurementProducts[0].PassOn!=null)
                        {
                            var passOnAccount = procurementProducts[0].PassOn.chartofAccountId;
                            var totalPassOn = procurementProducts.Sum(x => x.siPassOnValue);
                            if (passOnAccount != null)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                     x.accountId == passOnAccount &&
                                     x.debit == totalPassOn &&
                                     x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                     x.deptId == department.Id
                                     );

                                if (dbTransaction == null)
                                {
                                    double total = 0;
                                    if (
                                           procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Loan ||
                                           procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Equity ||
                                           procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Income ||
                                           procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = 0 - totalPassOn;
                                    }
                                    else
                                    {
                                        total = totalPassOn - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        accountId = passOnAccount,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = totalPassOn,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = 0,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                   

                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Income ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - totalPassOn;
                                        }
                                        else
                                        {
                                            total = totalPassOn - 0;
                                        }

                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = passOnAccount,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = totalPassOn,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions

                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Income ||
                                               procurementProducts[0].PassOn.ChartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - totalPassOn;
                                        }
                                        else
                                        {
                                            total = totalPassOn - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = passOnAccount,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = totalPassOn,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled

                                        });
                                    }
                                }
                               
                            }

                        }
                        if (procurementProducts[0].ClaimDiscount != null)
                        {
                            var claimDiscountAccount = procurementProducts[0].ClaimDiscount.chartofAccountId;
                            var totalClaimDiscount = procurementProducts.Sum(x => x.siClaimDiscountValue);
                            if (claimDiscountAccount != null)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                   x.accountId == claimDiscountAccount &&
                                   x.debit == claimDiscountAccount &&
                                   x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                   x.deptId == department.Id
                                   );
                                if (dbTransaction == null)
                                {
                                    double total = 0;
                                    if (
                                           procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Loan ||
                                           procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Equity ||
                                           procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Income ||
                                           procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = 0 - totalClaimDiscount;
                                    }
                                    else
                                    {
                                        total = totalClaimDiscount - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        accountId = claimDiscountAccount,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = totalClaimDiscount,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = 0,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    });
                                }
                                else
                                {

                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Income ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - totalClaimDiscount;
                                        }
                                        else
                                        {
                                            total = totalClaimDiscount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = claimDiscountAccount,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = totalClaimDiscount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions

                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Income ||
                                               procurementProducts[0].ClaimDiscount.ChartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - totalClaimDiscount;
                                        }
                                        else
                                        {
                                            total = totalClaimDiscount - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = claimDiscountAccount,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = totalClaimDiscount,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled

                                        });
                                    }
                                }
                            }

                        }
                        if (procurementProducts[0].FOCSampling != null)
                        {
                            var focSamplingAccount = procurementProducts[0].FOCSampling.chartofAccountId;
                            var totalFocSampling = procurementProducts.Sum(x => x.siFocValue);
                            if (focSamplingAccount != null)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                  x.accountId == focSamplingAccount &&
                                  x.debit == totalFocSampling &&
                                  x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                  x.deptId == department.Id
                                  );
                                if(dbTransaction==null)
                                {
                                    double total = 0;
                                    if (
                                           procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Loan ||
                                           procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Equity ||
                                           procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Income ||
                                           procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = 0 - totalFocSampling;
                                    }
                                    else
                                    {
                                        total = totalFocSampling - 0;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        accountId = focSamplingAccount,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = totalFocSampling,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = 0,
                                        deptId = department.Id,
                                        total = totalFocSampling - 0,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                     

                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Income ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - totalFocSampling;
                                        }
                                        else
                                        {
                                            total = totalFocSampling - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = focSamplingAccount,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = totalFocSampling,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions

                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Income ||
                                               procurementProducts[0].FOCSampling.ChartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = 0 - totalFocSampling;
                                        }
                                        else
                                        {
                                            total = totalFocSampling - 0;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = focSamplingAccount,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = totalFocSampling,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = 0,
                                            deptId = department.Id,
                                            total = totalFocSampling - 0,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled

                                        });
                                    }
                                }

                             
                            }

                        }



                        var dbTransaction1 = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                x.accountId == department.chartofAccountId &&
                                x.debit == Convert.ToDouble(txttotalcfr.Text) &&
                                x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                x.deptId == department.Id
                                );
                        if(dbTransaction1==null)
                        {
                            double total = 0;
                            if (
                                   department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                   department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                   department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                   department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                   department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                   department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                   department.ChartofAccount.accountType == COA_AccountType.Income ||
                                   department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                   )
                            {
                                total = 0 - Convert.ToDouble(txttotalcfr.Text);
                            }
                            else
                            {
                                total = Convert.ToDouble(txttotalcfr.Text) - 0;
                            }
                            journalTransactions.Add(new JournalTransaction()
                            {
                                accountId = department.chartofAccountId,
                                coaTransactionsType = coaTransactionsType.SaleInvoice,
                                SaleInvoiceId = saleInvoice.Id,
                                creationDate = (DateTime)datglPostingdate.EditValue,
                                //memo = procurementProduct.inquiryProduct.ownDiscription,
                                transactionRefno = txtFinanaceRef.Text,
                                userId = SYSTEM_STATIC.currentUser.id,
                                debit = Convert.ToDouble(txttotalcfr.Text),
                                MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                credit = 0,
                                deptId = department.Id,
                                total = total,
                                companyId = (lookupCompany.SelectedItem as Company).Id,
                                currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                             

                            });
                        }
                        else
                        {
                            if (dbTransaction1.creationDate != (DateTime)datglPostingdate.EditValue)
                            {
                                double total = 0;
                                if (
                                       department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       department.ChartofAccount.accountType == COA_AccountType.Income ||
                                       department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - Convert.ToDouble(txttotalcfr.Text);
                                }
                                else
                                {
                                    total = Convert.ToDouble(txttotalcfr.Text) - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = department.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = Convert.ToDouble(txttotalcfr.Text),
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    isReconciled = false,
                                    reconcilationDate = null,
                                    ReconcilationId = null,
                                    reconcilationType = ReconcilationType.Uncleared_Transactions

                                });
                            }
                            else
                            {
                                double total = 0;
                                if (
                                       department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                       department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                       department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                       department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                       department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                       department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                       department.ChartofAccount.accountType == COA_AccountType.Income ||
                                       department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                       )
                                {
                                    total = 0 - Convert.ToDouble(txttotalcfr.Text);
                                }
                                else
                                {
                                    total = Convert.ToDouble(txttotalcfr.Text) - 0;
                                }
                                journalTransactions.Add(new JournalTransaction()
                                {
                                    accountId = department.chartofAccountId,
                                    coaTransactionsType = coaTransactionsType.SaleInvoice,
                                    SaleInvoiceId = saleInvoice.Id,
                                    creationDate = (DateTime)datglPostingdate.EditValue,
                                    //memo = procurementProduct.inquiryProduct.ownDiscription,
                                    transactionRefno = txtFinanaceRef.Text,
                                    userId = SYSTEM_STATIC.currentUser.id,
                                    debit = Convert.ToDouble(txttotalcfr.Text),
                                    MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                    credit = 0,
                                    deptId = department.Id,
                                    total = total,
                                    companyId = (lookupCompany.SelectedItem as Company).Id,
                                    currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    reconcilationDate = dbTransaction1.reconcilationDate,
                                    reconcilationType = dbTransaction1.reconcilationType,
                                    ReconcilationId = dbTransaction1.ReconcilationId,
                                    isReconciled = dbTransaction1.isReconciled

                                });
                            }
                        }

                    }
                    if (btnPushCredits.IsChecked == true)
                    {

                        if (procurementProducts[0].TaxName != null)
                        {
                            var gstAccount = procurementProducts[0].TaxName.COA_Id;
                            var totalGST = procurementProducts.Sum(x => x.siAmountGST);
                            if (gstAccount != null)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                 x.accountId == gstAccount &&
                                 x.credit == totalGST &&
                                 x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                 x.deptId == department.Id
                                 );
                                if(dbTransaction==null)
                                {
                                    double total = 0;
                                    if (
                                           procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Loan ||
                                           procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Equity ||
                                           procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Income ||
                                           procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = totalGST;
                                    }
                                    else
                                    {
                                        total = 0 - totalGST;
                                    }
                                    JournalTransaction taxTransaction = new JournalTransaction();
                                    {
                                        taxTransaction.accountId = gstAccount;
                                        taxTransaction.credit = totalGST;
                                        taxTransaction.deptId = department.Id;
                                        taxTransaction.SaleInvoiceId = saleInvoice.Id;
                                        taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                        taxTransaction.userId = saleInvoice.user_Id;
                                        taxTransaction.coaTransactionsType = coaTransactionsType.SaleInvoice;
                                        taxTransaction.MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                                        taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                        taxTransaction.total = total;
                                        taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                        taxTransaction.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                               
                                    }
                                    journalTransactions.Add(taxTransaction);
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Loan ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Equity ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Income ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = totalGST;
                                        }
                                        else
                                        {
                                            total = 0 - totalGST;
                                        }
                                        JournalTransaction taxTransaction = new JournalTransaction();
                                        {
                                            taxTransaction.accountId = gstAccount;
                                            taxTransaction.credit = totalGST;
                                            taxTransaction.deptId = department.Id;
                                            taxTransaction.SaleInvoiceId = saleInvoice.Id;
                                            taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                            taxTransaction.userId = saleInvoice.user_Id;
                                            taxTransaction.coaTransactionsType = coaTransactionsType.SaleInvoice;
                                            taxTransaction.MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                                            taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                            taxTransaction.total = total;
                                            taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                            taxTransaction.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                                            taxTransaction.isReconciled = false;
                                            taxTransaction.reconcilationDate = null;
                                            taxTransaction.ReconcilationId = null;
                                            taxTransaction.reconcilationType = ReconcilationType.Uncleared_Transactions;
                                        }
                                        journalTransactions.Add(taxTransaction);
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Loan ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Equity ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Income ||
                                               procurementProducts[0].TaxName.chartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = totalGST;
                                        }
                                        else
                                        {
                                            total = 0 - totalGST;
                                        }
                                        JournalTransaction taxTransaction = new JournalTransaction();
                                        {
                                            taxTransaction.accountId = gstAccount;
                                            taxTransaction.credit = totalGST;
                                            taxTransaction.deptId = department.Id;
                                            taxTransaction.SaleInvoiceId = saleInvoice.Id;
                                            taxTransaction.transactionRefno = txtFinanaceRef.Text;
                                            taxTransaction.userId = saleInvoice.user_Id;
                                            taxTransaction.coaTransactionsType = coaTransactionsType.SaleInvoice;
                                            taxTransaction.MER = Math.Round(Convert.ToDouble(txtMER.Text), 2);
                                            taxTransaction.creationDate = (DateTime)datglPostingdate.EditValue;
                                            taxTransaction.total = total;
                                            taxTransaction.companyId = (lookupCompany.SelectedItem as Company).Id;
                                            taxTransaction.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                                            taxTransaction.reconcilationDate = dbTransaction.reconcilationDate;
                                            taxTransaction.reconcilationType = dbTransaction.reconcilationType;
                                            taxTransaction.ReconcilationId = dbTransaction.ReconcilationId;
                                            taxTransaction.isReconciled = dbTransaction.isReconciled;
                                        }
                                        journalTransactions.Add(taxTransaction);
                                    }
                                }
                                

                            }

                        }
                        if (procurementProducts[0].PassOn != null)
                        {
                            var passOnAccount = procurementProducts[0].PassOn.chartofAccountId;
                            var totalPassOn = procurementProducts.Sum(x => x.siPassOnValue);
                            if (passOnAccount != null)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                 x.accountId == department.chartofAccountId &&
                                 x.credit == totalPassOn &&
                                 x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                 x.deptId == department.Id
                                 );

                                if(dbTransaction==null)
                                {
                                    double total = 0;
                                    if (
                                           department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                           department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                           department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           department.ChartofAccount.accountType == COA_AccountType.Income ||
                                           department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                           )
                                    {
                                        total = totalPassOn;
                                    }
                                    else
                                    {
                                        total = 0 - totalPassOn;
                                    }


                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        accountId = department.chartofAccountId,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = totalPassOn,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                   

                                    });

                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Income ||
                                               department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = totalPassOn;
                                        }
                                        else
                                        {
                                            total = 0 - totalPassOn;
                                        }

                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = department.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = totalPassOn,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions

                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Income ||
                                               department.ChartofAccount.accountType == COA_AccountType.Other_Income
                                               )
                                        {
                                            total = totalPassOn;
                                        }
                                        else
                                        {
                                            total = 0 - totalPassOn;
                                        }


                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = department.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = totalPassOn,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled

                                        });
                                    }
                                }
                             
                            }
                        }
                        if (procurementProducts[0].FOCSampling != null)
                        {

                            var focSamplingAccount = procurementProducts[0].FOCSampling.chartofAccountId;
                            var totalFocSampling = procurementProducts.Sum(x => x.siFocValue);
                            if (focSamplingAccount != null)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                                x.accountId == department.chartofAccountId &&
                                x.credit == totalFocSampling &&
                                x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                                x.deptId == department.Id
                                );
                                if (dbTransaction == null)
                                {
                                    double total = 0;
                                    if (
                                           department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                           department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                           department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                           )
                                    {
                                        total = totalFocSampling;
                                    }
                                    else
                                    {
                                        total = 0 - totalFocSampling;
                                    }
                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        accountId = department.chartofAccountId,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = totalFocSampling,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {

                                        double total = 0;
                                        if (
                                               department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                               )
                                        {
                                            total = totalFocSampling;
                                        }
                                        else
                                        {
                                            total = 0 - totalFocSampling;
                                        }


                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = department.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = totalFocSampling,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions


                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                               )
                                        {
                                            total = totalFocSampling;
                                        }
                                        else
                                        {
                                            total = 0 - totalFocSampling;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = department.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = totalFocSampling,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled


                                        });
                                    }
                                }
                            }

                        }
                        if (procurementProducts[0].ClaimDiscount != null)
                        {
                            var claimDiscountAccount = procurementProducts[0].ClaimDiscount.chartofAccountId;
                            var totalClaimDiscount = procurementProducts.Sum(x => x.siClaimDiscountValue);
                            if (claimDiscountAccount != null)
                            {
                                var dbTransaction = saleInvoice.journalTransactions.FirstOrDefault(x =>
                              x.accountId == department.chartofAccountId &&
                              x.credit == totalClaimDiscount &&
                              x.companyId == (lookupCompany.SelectedItem as Company).Id &&
                              x.deptId == department.Id
                              );

                                if (dbTransaction == null)
                                {
                                    double total = 0;
                                    if (
                                           department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                           department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                           department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                           department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                           department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                           department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                           department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                           )
                                    {
                                        total = totalClaimDiscount;
                                    }
                                    else
                                    {
                                        total = 0 - totalClaimDiscount;
                                    }

                                    journalTransactions.Add(new JournalTransaction()
                                    {
                                        accountId = department.chartofAccountId,
                                        coaTransactionsType = coaTransactionsType.SaleInvoice,
                                        SaleInvoiceId = saleInvoice.Id,
                                        creationDate = (DateTime)datglPostingdate.EditValue,
                                        //memo = procurementProduct.inquiryProduct.ownDiscription,
                                        transactionRefno = txtFinanaceRef.Text,
                                        userId = SYSTEM_STATIC.currentUser.id,
                                        debit = 0,
                                        MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                        credit = totalClaimDiscount,
                                        deptId = department.Id,
                                        total = total,
                                        companyId = (lookupCompany.SelectedItem as Company).Id,
                                        currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                    

                                    });
                                }
                                else
                                {
                                    if (dbTransaction.creationDate != (DateTime)datglPostingdate.EditValue)
                                    {
                                        double total = 0;
                                        if (
                                               department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                               )
                                        {
                                            total = totalClaimDiscount;
                                        }
                                        else
                                        {
                                            total = 0 - totalClaimDiscount;
                                        }

                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = department.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = totalClaimDiscount,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            isReconciled = false,
                                            reconcilationDate = null,
                                            ReconcilationId = null,
                                            reconcilationType = ReconcilationType.Uncleared_Transactions
                                        });
                                    }
                                    else
                                    {
                                        double total = 0;
                                        if (
                                               department.ChartofAccount.accountType == COA_AccountType.Loan ||
                                               department.ChartofAccount.accountType == COA_AccountType.Credit_Card ||
                                               department.ChartofAccount.accountType == COA_AccountType.Equity ||
                                               department.ChartofAccount.accountType == COA_AccountType.Accounts_Payable ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Other_Current_Liability ||
                                               department.ChartofAccount.accountType == COA_AccountType.Longterm_Liability
                                               )
                                        {
                                            total = totalClaimDiscount;
                                        }
                                        else
                                        {
                                            total = 0 - totalClaimDiscount;
                                        }
                                        journalTransactions.Add(new JournalTransaction()
                                        {
                                            accountId = department.chartofAccountId,
                                            coaTransactionsType = coaTransactionsType.SaleInvoice,
                                            SaleInvoiceId = saleInvoice.Id,
                                            creationDate = (DateTime)datglPostingdate.EditValue,
                                            //memo = procurementProduct.inquiryProduct.ownDiscription,
                                            transactionRefno = txtFinanaceRef.Text,
                                            userId = SYSTEM_STATIC.currentUser.id,
                                            debit = 0,
                                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                                            credit = totalClaimDiscount,
                                            deptId = department.Id,
                                            total = total,
                                            companyId = (lookupCompany.SelectedItem as Company).Id,
                                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                                            reconcilationDate = dbTransaction.reconcilationDate,
                                            reconcilationType = dbTransaction.reconcilationType,
                                            ReconcilationId = dbTransaction.ReconcilationId,
                                            isReconciled = dbTransaction.isReconciled
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please contact admin to attach receivable account with department", "Approval Required", MessageBoxButton.OK);
                }
            }
            return journalTransactions;
        }
        public ICollection<ERP_BL.Procurements.Inventories.Inventory> getInventories()
        {
            List<ERP_BL.Procurements.Inventories.Inventory> inventories = new List<ERP_BL.Procurements.Inventories.Inventory>();
            ProductRepo productrepo = new ProductRepo();
            if (saleInvoice.Inventories == null && editInvoice == 0)
            {
                var procurementProducts = grdInvoiceItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.inquiryProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory)
                    {
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        if (averageCost.ToString()== "NaN")
                        {
                            DXMessageBox.Show(procurementProduct.inquiryProduct.product.code+ " "+ "has no purchases remaining into the ERP."+"\n" +"Please close the invoice and put Purchases first.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                        if (averageCost == 0 || remainingAmountInven == 0 && remaiInvenQuantity == 0)
                        {
                            averageCost = Math.Round(procurementProduct.unitPrice * procurementProduct.InvoicedQuantity / procurementProduct.InvoicedQuantity, 2);
                        }
                        inventories.Add(new ERP_BL.Procurements.Inventories.Inventory()
                        {
                            prodId = dbProduct.Id,
                            Quantity = -procurementProduct.InvoicedQuantity,
                            Weight = -procurementProduct.InvoicedWeight,
                            UnitRate = procurementProduct.unitPrice,
                            TransactionsType = InventoryTransactionsType.SaleInvoice,
                            SaleInviceId = saleInvoice.Id,
                            creationDate = (DateTime)datpoCreationdate.EditValue,
                            transactionRefno = txtFinanaceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            deptId = department.Id,
                            AmountOC = -Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                            AmountMER = -Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) * Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                            AverageCost = averageCost
                        });
                    }

                }
            }
            else
            {
                var procurementProducts = grdInvoiceItems.ItemsSource as List<ProcurementProduct>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.inquiryProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory)
                    {
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                            var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        if (averageCost.ToString() == "NaN")
                        {
                            DXMessageBox.Show(procurementProduct.inquiryProduct.product.code + " " + "has no purchases remaining into the ERP." + "\n" + "Please close the invoice and put Purchases first.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        }
                        if (averageCost == 0 || remainingAmountInven == 0 && remaiInvenQuantity == 0)
                        {
                            averageCost = Math.Round(procurementProduct.unitPrice * procurementProduct.InvoicedQuantity / procurementProduct.InvoicedQuantity, 2);
                        }
                        inventories.Add(new ERP_BL.Procurements.Inventories.Inventory()
                        {
                            prodId = dbProduct.Id,
                            Quantity = -procurementProduct.InvoicedQuantity,
                            Weight = -procurementProduct.InvoicedWeight,
                            UnitRate = procurementProduct.unitPrice,
                            TransactionsType = InventoryTransactionsType.SaleInvoice,
                            SaleInviceId = saleInvoice.Id,
                            creationDate = (DateTime)datpoCreationdate.EditValue,
                            transactionRefno = txtFinanaceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            deptId = department.Id,
                            AmountOC = -Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2),
                            AmountMER = -Math.Round(averageCost * procurementProduct.InvoicedQuantity, 2) * Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                            AverageCost = averageCost
                        });
                    }
                }
            }
            return inventories;
        }
        public ICollection<ERP_BL.Procurements.Inventories.Inventory> getBookerInventories()
        {
            List<ERP_BL.Procurements.Inventories.Inventory> inventories = new List<ERP_BL.Procurements.Inventories.Inventory>();

            ProductRepo productrepo = new ProductRepo();
            if (saleInvoice.Inventories == null && editInvoice == 0)
            {
                var procurementProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productRepo.get(procurementProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory)
                    {

                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        inventories.Add(new ERP_BL.Procurements.Inventories.Inventory()
                        {
                            prodId = dbProduct.Id,
                            Quantity = -procurementProduct.siQuantity,
                            Weight = -procurementProduct.siWeight,
                            UnitRate = procurementProduct.unit,
                            TransactionsType = InventoryTransactionsType.SaleInvoice,
                            SaleInviceId = saleInvoice.Id,
                            creationDate = (DateTime)datpoCreationdate.EditValue,
                            transactionRefno = txtFinanaceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            deptId = department.Id,
                            AmountOC = -Math.Round(averageCost * procurementProduct.siQuantity, 2),
                            AmountMER = -Math.Round(averageCost * procurementProduct.siQuantity, 2) * Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                            AverageCost = averageCost
                        });
                    }
                }
            }
            else
            {
                var procurementProducts = grdBokkerItems.ItemsSource as List<BookerStatementItem>;
                foreach (var procurementProduct in procurementProducts)
                {
                    var dbProduct = productrepo.get(procurementProduct.product.Id);
                    if (dbProduct.productType == ProductType.Inventory)
                    {
                        var allPurcashes = productrepo.getPurchases(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> purchases = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        purchases = allPurcashes.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                        purchases = purchases.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var allAdjustments = adjustmentRepo.GetbyProductId(dbProduct.Id, (DateTime)datpoCreationdate.EditValue);
                        List<ERP_BL.Procurements.Inventories.Inventory> adjustments = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        adjustments = allAdjustments.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                        var sumAdjustmentsAmount = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.AmountOC);
                        var sumAdjustmentQuantity = adjustments.Where(x => x.InventoryAdjustment.isVoid != true).Sum(x => x.Quantity);
                        var allSales = productrepo.getSales(dbProduct.Id);
                        List<ERP_BL.Procurements.Inventories.Inventory> sales = new List<ERP_BL.Procurements.Inventories.Inventory>();
                        sales = allSales.Where(x => x.SaleInvoice.isVoid != true).ToList();
                        sales = sales.Where(x => x.creationDate < saleInvoice.CreationDate).ToList();
                        var sumPurchase = purchases.Sum(x => x.AmountOC);
                        sumPurchase = sumPurchase + sumAdjustmentsAmount;
                        var sumSales = sales.Sum(x => x.AmountOC);
                        var qSumPurchases = purchases.Sum(x => x.Quantity);
                        qSumPurchases = qSumPurchases + sumAdjustmentQuantity;
                        var qSumSales = sales.Sum(x => x.Quantity);
                        var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                        var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                        var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                        inventories.Add(new ERP_BL.Procurements.Inventories.Inventory()
                        {
                            prodId = dbProduct.Id,
                            Quantity = -procurementProduct.siQuantity,
                            Weight = -procurementProduct.siWeight,
                            UnitRate = procurementProduct.unit,
                            TransactionsType = InventoryTransactionsType.SaleInvoice,
                            SaleInviceId = saleInvoice.Id,
                            creationDate = (DateTime)datpoCreationdate.EditValue,
                            transactionRefno = txtFinanaceRef.Text,
                            userId = SYSTEM_STATIC.currentUser.id,
                            MER = Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            deptId = department.Id,
                            AmountOC = -Math.Round(averageCost * procurementProduct.siQuantity, 2),
                            AmountMER = -Math.Round(averageCost * procurementProduct.siQuantity, 2) * Math.Round(Convert.ToDouble(txtMER.Text), 2),
                            companyId = (lookupCompany.SelectedItem as Company).Id,
                            currencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                            AverageCost = averageCost
                        });
                    }
                }
            }
            return inventories;
        }
        public double updateAmountMER(JournalTransaction transaction, double nowAmount)
        {
            double value = 0;
            if (transaction.debit != 0)
            {
                value = nowAmount * Convert.ToDouble(txtMER.Text);
            }
            else
                if (transaction.credit != 0)
            {
                value = -nowAmount * Convert.ToDouble(txtMER.Text);
            }
            return value;
        }
        public double GetDebitAmount(JournalTransaction transaction, double nowAmount)
        {
            if (transaction.debit != 0)
            {
                return nowAmount;
            }
            else
                return 0;

        }
        public double GetCreditAmount(JournalTransaction transaction, double nowAmount)
        {
            if (transaction.credit != 0)
            {
                return nowAmount;
            }
            else
                return 0;

        }
        public List<ERP_BL.VATBook.VATBook> GetVATBooks()
        {
            List<ERP_BL.VATBook.VATBook> vatBooks = new List<ERP_BL.VATBook.VATBook>();
            ERP_BL.VATBook.VATBook book = new  ERP_BL.VATBook.VATBook();
            book.CreationDate = datpoCreationdate.DateTime;
            book.GLPostingDate = datpoCreationdate.DateTime;
            book.saleInvoiceId = saleInvoice.Id;
            book.TransactionType = TransactionItemType.Sale_Invoice;
            book.debit = 0;
            book.credit = Convert.ToDouble(txttax.Text);
            book.total = 0 - Convert.ToDouble(txttax.Text);
            book.FinanceRefNo = txtFinanaceRef.Text;
            book.SystemRefNo = txtSystemRefNo.Text;
            book.deptId = saleInvoice.dept_Id;
            book.companyId = saleInvoice.company_Id;
            book.currencyId = (cmbCurrency.SelectedItem as cmbitem).id;
            book.VATBookRefNumberRefId = cmbxVATBookRef.SelectedIndex > -1 ? (cmbxVATBookRef.SelectedItem as cmbitem).id : (int?)null;
            vatBooks.Add(book);
            return vatBooks;
        }
            
        private void btnSaleInvoiceSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                NotificationsRepo notificationsRepo = new NotificationsRepo();

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
                    MessageBox.Show("Please Select an Employee to whom this Sale Invoice will be Allocated ", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                else if (cmbSaleInvoiceStatus.SelectedIndex == -1 && saleInvoice.PendingForClosing != true)
                {
                    MessageBox.Show("Please Select Current Status of Sale Invoice to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbSaleInvoiceStatus.Focus();
                    return;
                }
                else if (cmbSaleInvoiceStatusClass.SelectedIndex == -1 /*&& saleInvoice.PendingForClosing != true*/)
                {
                    MessageBox.Show("Please Select Current Status class of Sale Invoice to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbSaleInvoiceStatusClass.Focus();
                    return;
                }
                else if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Sale Invoices Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbCurrency.Focus();
                    return;
                }
                else if (Convert.ToDouble(txttotalcfr.Text) == 0)
                {
                    MessageBox.Show("Please Enter Amount for the Items", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    grdInvoiceItems.Focus();
                    return;
                }
                else if (cmbPaymentTerm.SelectedIndex == -1)
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

                else if (cmbTransactionHolder.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }
                else if (saleOrder.currency_Id!= (cmbCurrency.SelectedItem as cmbitem).id)
                {
                    MessageBox.Show("Sale Invoice Currency don't match with sale order currency", "Information", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbCurrency.Focus();
                    return;
                }
                if (cmbTransactionHolder.SelectedIndex != -1)
                {
                    saleInvoice.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                    saleInvoice.holderChangeDate = (DateTime)datHolderDate.EditValue;
                }
                if (btnSTLGenerated.IsChecked==true)
                {
                    saleInvoice.isSTLGenerated = true;
                }
                else
                    saleInvoice.isSTLGenerated = false;
                if (btnSTLDiscounted.IsChecked == true)
                {
                    saleInvoice.isSTLDiscount = true;
                }
                else
                    saleInvoice.isSTLDiscount = false;

                if (btnPaid.IsChecked == true)
                {
                    saleInvoice.isPaid = true;
                }
                else
                    saleInvoice.isPaid = false;

                if (btnRedInvoice.IsChecked == true)
                {
                    saleInvoice.isRedInvoice = true;
                }
                else
                    saleInvoice.isRedInvoice = false;

                if(datPaymentOnDate.EditValue!=null)
                {
                    saleInvoice.paymentOnDate = (DateTime)datPaymentOnDate.EditValue;
                }
                if(!string.IsNullOrEmpty(txtDeliveryDays.Text))
                {
                    saleInvoice.deliveryDays = Convert.ToDouble(txtDeliveryDays.Text);
                }


                if (!string.IsNullOrEmpty(txtStlDiscount.Text))
                {
                    saleInvoice.stlDiscountAmount = Convert.ToDouble(txtStlDiscount.Text);
                }

                if (!string.IsNullOrEmpty(txtLOTNo.Text))
                    saleInvoice.lotNo =txtLOTNo.Text;
                else
                    saleInvoice.lotNo = null;

                if (lookupLotNumbers.SelectedIndex > -1)
                    saleInvoice.lotNumberId = (lookupLotNumbers.SelectedItem as LotNumber).Id;
                else
                    saleInvoice.lotNumberId = null;

                if (!string.IsNullOrEmpty(txtInvoiceNo.Text))
                    saleInvoice.invoiceNo = txtInvoiceNo.Text;
                

                if (!string.IsNullOrEmpty (datInvoice.Text))
                {
                    saleInvoice.invoiceDate = datsaleInvoicedate.DateTime;
                }
                if (cmbDiscountCurrency.SelectedIndex>-1)
                {
                    saleInvoice.stlDiscountCurrency_Id = (cmbDiscountCurrency.SelectedItem as cmbitem).id;
                }
                if (!string.IsNullOrEmpty(txtStlAmount.Text))
                {
                    saleInvoice.stlAmount = Convert.ToDouble(txtStlAmount.Text);
                }
                if (cmbSTLCurrency.SelectedIndex > -1)
                {
                    saleInvoice.stlSTLCurrency_Id = (cmbSTLCurrency.SelectedItem as cmbitem).id;
                }
                DateTime creationDate = new DateTime(2022, 3, 17);
                if (datpoCreationdate.DateTime > creationDate)
                {
                    if (lookupBank.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please Select bank!");
                        lookupBank.Focus();
                        return;
                    }
                    if (lookupAccount.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please Select Account!");
                        lookupAccount.Focus();
                        return;
                    }
                }
                if (cmbxVATBookRef.SelectedIndex > -1)
                {
                    saleInvoice.VATBookRefId = (cmbxVATBookRef.SelectedItem as cmbitem).id;
                    if (lookupSalesTax.SelectedIndex > -1)
                    {
                        saleInvoice.VATBooks = new List<ERP_BL.VATBook.VATBook>();
                        saleInvoice.VATBooks = GetVATBooks();
                    }
                }

                if (lookupBank.SelectedItem != null)
                    saleInvoice.bank_Id = (lookupBank.SelectedItem as Bank).Id;
                if(lookupAccount.SelectedItem != null)
                    saleInvoice.account_Id = (lookupAccount.SelectedItem as Account).Id;

                DateTime? dateTime = null;
            
                saleInvoice.saleInvoiceDate = (datsaleInvoicedate.Text == "") ? dateTime : datsaleInvoicedate.DateTime;
                saleInvoice.CreationDate = (datpoCreationdate.Text == "") ? dateTime : datpoCreationdate.DateTime;
            
                saleInvoice.paymentterm_Id = (cmbPaymentTerm.SelectedItem as cmbitem).id;
                saleInvoice.incoterm_Id = (cmbIncoterm.SelectedItem as cmbitem).id;
                if(!string.IsNullOrEmpty(txtAmountSOC.Text))
                {
                    saleInvoice.amountSOC = Convert.ToDouble(txtAmountSOC.Text);

                }
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
                {
                    saleInvoice.BookerStatementItems = getBookerStatementItems();
                    saleInvoice.journalTransactions = getBookerJournalTransactions();
                    saleInvoice.Inventories = getBookerInventories();

                }
                else
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    saleInvoice.BookerStatementItems = getBookerStatementItems();
                    saleInvoice.journalTransactions = getBookerJournalTransactions();
                    saleInvoice.Inventories = getBookerInventories();
                }
                else
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.Inventory)
                {
                    saleInvoice.products = getProductsdata();
                    saleInvoice.journalTransactions = getJournalTransactions();
                    saleInvoice.Inventories = getInventories();
                }
                else
                {
                    saleInvoice.products = getProductsdata();
                    saleInvoice.journalTransactions = getJournalTransactions();

                    if (saleInvoice.products.Count == 0)
                    {
                        MessageBox.Show("Please Select items against which you want to create a Purchase Invoice", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                        return;
                    }

                }
                if(btnInterCompanyReceivable.IsChecked==true)
                {
                    saleInvoice.isInterCompanyReceivable = true;
                }
                else
                {
                    saleInvoice.isInterCompanyReceivable = false;
                }
                if (btnInsuranceRequired.IsChecked == true)
                {
                    saleInvoice.insuranceRequired = true;
                }
                else
                {
                    saleInvoice.insuranceRequired = false;
                }
                if (btnInsuranceApplied.IsChecked == true)
                {
                    saleInvoice.insuranceApplied = true;
                }
                else
                {
                    saleInvoice.insuranceApplied = false;
                }
                if (btnInsuranceNotApplicable.IsChecked == true)
                {
                    saleInvoice.insuranceNotApplicable = true;
                }
                else
                {
                    saleInvoice.insuranceNotApplicable = false;
                }





                if (btnAdvancePayment.IsChecked == true)
                {
                    saleInvoice.isAdvancePayment = true;
                }
                else
                {
                    saleInvoice.isAdvancePayment = false;
                }
                saleInvoice.maker = txtMaker.Text.Trim();
                saleInvoice.origin = txtOrigin.Text.Trim();
                saleInvoice.PaymentYear = Convert.ToInt32(spnYear.Text);
                saleInvoice.PaymentQuarter = Convert.ToInt32(spnQuarter.Text);
                saleInvoice.deliveryDate = (datDeliverydate.Text == "") ? dateTime : datDeliverydate.DateTime;
                saleInvoice.ETADate = (datETADate.Text == "") ? dateTime : datETADate.DateTime;
                saleInvoice.ETDDate = (datETDdate.Text == "") ? dateTime : datETDdate.DateTime;
                saleInvoice.BLAWBDate = (datBillOfLaddingdate.Text == "") ? dateTime : datBillOfLaddingdate.DateTime;
                saleInvoice.BLdeliveryRefNo = txtBillLandingRef.Text;
                saleInvoice.materialReciptDate = (datMaterialReciptdate.Text == "") ? dateTime : datMaterialReciptdate.DateTime;
                saleInvoice.ExpectedPayment = (datExcpectedPaymentDate.Text == "") ? dateTime : datExcpectedPaymentDate.DateTime;
                saleInvoice.ExpectedPayment = (datExcpectedPaymentDate.Text == "") ? dateTime : datExcpectedPaymentDate.DateTime;
                saleInvoice.ExpectedDiscountDate = (datExcpectedDiscountDate.Text == "") ? dateTime : datExcpectedDiscountDate.DateTime;
                saleInvoice.OwnDescription = txtOwnDescription.Text;
                saleInvoice.FinanceRefrenceNo = txtFinanaceRef.Text;
                saleInvoice.commisionRefrenceNo = txtCommissionNumber.Text;
                saleInvoice.lCDate = (datLCDatedate.Text == "") ? dateTime : datLCDatedate.DateTime;
                saleInvoice.lCnumber = txtLCNumber.Text.Trim();
                saleInvoice.SalesReferenceNo = txtSalesref.Text.Trim();
                saleInvoice.referenceNo = txtsaleInvoiceref.Text.Trim();
                saleInvoice.offerReferenceNo = txtOfferRefNo.Text.Trim();
                saleInvoice.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
                if (insuarenceAppliedEmployee.EmpId!=0)
                {
                    saleInvoice.insuranceAppliedBy_Id = insuarenceAppliedEmployee.EmpId;
                }
                saleInvoice.saleInvoicetype = (InquiryType)cmbSaleInvoiceType.SelectedIndex;
                saleInvoice.PaymentDueStartDate = (datPaymentDueFrom.Text == "") ? dateTime : datPaymentDueFrom.DateTime;
                saleInvoice.PaymentDueAgeing = (datPaymentDueTill.Text == "") ? dateTime : datPaymentDueTill.DateTime;
                saleInvoice.CreditDays = Convert.ToInt32(txtPaymentDueDays.Text.Trim());
                saleInvoice.CostSheet_Id = saleOrder.CostSheet_Id;
                if (!string.IsNullOrEmpty(saleInvoiceCISubject))
                {
                    saleInvoice.CISubject = saleInvoiceCISubject;
                }
                if (!string.IsNullOrEmpty(saleInvoiceTaxSubject))
                {
                    saleInvoice.SItaxSubject = saleInvoiceTaxSubject;
                }
                if (!string.IsNullOrEmpty(saleInvoiceDnoteSubject))
                {
                    saleInvoice.DNSubject = saleInvoiceDnoteSubject;
                }
                if (!string.IsNullOrEmpty(saleInvoiceZasCommission))
                {
                    saleInvoice.Commission = saleInvoiceZasCommission;
                }
                if (datglPostingdate.EditValue != null)
                {
                    saleInvoice.GLPostingDate = (DateTime)datglPostingdate.EditValue;
                }
                else
                {
                    saleInvoice.GLPostingDate = null;
                }
                if (!string.IsNullOrEmpty(txttax.Text))
                {
                    saleInvoice.totaltaxAmount = Convert.ToDouble(txttax.Text);
                }
                if (saleInvoice.SaleOrder == null && saleOrder != null)
                {
                    saleInvoice.SaleOrderId = saleOrder.Id;

                }
                saleInvoice.totalInvoiceAmount = Math.Round( Convert.ToDouble(txttotalcfr.Text), 2);
                saleInvoice.SOCFRValue = Convert.ToDouble(txtSOtotalcfr.Text);
                if (saleInvoice.SaleOrder != null)
                {
                    saleInvoice.SaleOrder.RemainingCFRValue = Convert.ToDouble(txtSOremainingcfr.Text);
                    if (saleInvoice.SaleOrder.RemainingFOBValue == 0 && saleInvoice.SaleOrder.RemainingCFRValue == 0)
                    {
                        saleInvoice.SaleOrder.InvoiceStage = InvoiceStage.Fully.ToString();
                    }
                    else if (saleInvoice.SaleOrder.RemainingFOBValue != 0 && saleInvoice.SaleOrder.RemainingCFRValue != 0 && saleInvoice.SaleOrder.RemainingCFRValue < saleInvoice.SOCFRValue)
                    {
                        saleInvoice.SaleOrder.InvoiceStage = InvoiceStage.Partialy.ToString();
                    }
                    else
                    {
                        saleInvoice.SaleOrder.InvoiceStage = InvoiceStage.Fully.ToString();
                    }
                }
                else
                {
                    saleOrder.RemainingCFRValue = Convert.ToDouble(txtSOremainingcfr.Text);
                    if (saleOrder.RemainingFOBValue == 0 && saleOrder.RemainingCFRValue == 0)
                    {
                        saleOrder.InvoiceStage = InvoiceStage.Fully.ToString();
                    }
                    else if (saleOrder.RemainingFOBValue != 0 && saleOrder.RemainingCFRValue != 0 && saleOrder.RemainingCFRValue < saleInvoice.SOCFRValue)
                    {
                        saleOrder.InvoiceStage = InvoiceStage.Partialy.ToString();
                    }
                    else
                    {
                        saleOrder.InvoiceStage = InvoiceStage.None.ToString();
                    }
                }
                saleInvoice.totalBaseAmount = Convert.ToDouble(totalMER.Text);
                saleInvoice.SoAmountSER = Convert.ToDouble(txtTotalSER.Text);
                saleInvoice.exchangeRate = (float)Convert.ToDecimal(txtMER.Text.Trim());
                saleInvoice.marginExchangeRate = Convert.ToDecimal(txtSER.Text.Trim());
                string str = txttax.Text.Trim();
                if (!string.IsNullOrEmpty(txtWeight.Text))
                {
                    saleInvoice.TotalWeight = Convert.ToDecimal(txtWeight.Text);
                }
                else
                {
                    saleInvoice.TotalWeight = null;
                }
                if (!string.IsNullOrEmpty(txtQuantity.Text))
                {
                    saleInvoice.TotalQuantity = Convert.ToDecimal(txtQuantity.Text);
                }
                else
                {
                    saleInvoice.TotalQuantity = null;
                }
                if (str.IndexOf("%") != -1)
                {
                    saleInvoice.isPercentTax = true;
                    saleInvoice.salesTax = Convert.ToDouble(str.Substring(0, str.IndexOf("%")));
                }
                else
                {
                    saleInvoice.salesTax = (!string.IsNullOrEmpty(str)) ? Convert.ToDouble(str) : 0;
                    saleInvoice.isPercentTax = false;
                }

                if ((cmbSaleInvoiceStatus.SelectedItem as cmbitem) != null)
                {

                    SaleInvoiceStatus status = saleInvoiceRepo.getstatus((cmbSaleInvoiceStatus.SelectedItem as cmbitem).id);
                    saleInvoice.saleInvoiceStatus = status;
                }

                if ((cmbSaleInvoiceStatusClass.SelectedItem as cmbitem) != null)
                {

                    ERP_BL.Procurements.StatusClass.StatusClass statusClass = saleInvoiceRepo.GetStatusClass((cmbSaleInvoiceStatusClass.SelectedItem as cmbitem).id);
                    saleInvoice.statusClass_Id = statusClass.Id;
                }


                // Selected Principal 
                if (principal != null)
                {

                    saleInvoice.principal_Id = principal.Id;
                }
                // Selected Vendor 
                if (vendor != null)
                {

                    saleInvoice.vendors = new List<Vendor>();
                    saleInvoice.vendors.Add(saleInvoiceRepo.getVendor(vendor.Id));
                }
                // selected Department
                if (department != null)
                {

                    saleInvoice.dept_Id = department.Id;
                }
                //selected customer
                if (customer != null)
                {

                    saleInvoice.customerCompany_Id = customer.Id;
                }
                // selected company
                if (company != null)
                {

                    saleInvoice.company_Id = company.Id;
                }
                if (chkInterCompany.IsChecked == true)
                {
                    if (InterCompany != null && InterCompany.Id != 0)
                    {

                        saleInvoice.InterCompany_Id = InterCompany.Id;
                    }
                    if (InterDepartment != null && InterDepartment.Id != 0)
                    {

                        saleInvoice.InterDepartment_Id = InterDepartment.Id;
                    }
                    saleInvoice.isInterCompany = true;

                }
                else
                {
                    saleInvoice.isInterCompany = false;
                    saleInvoice.InterCompany_Id = null;
                    saleInvoice.InterDepartment_Id = null;
                }
                // selected currency
                if (cmbCurrency.SelectedIndex != -1)
                {

                    saleInvoice.currency_Id = (cmbCurrency.SelectedItem as cmbitem).id;
                }
                saleInvoice.BatchRefrenceNo = txtSystemRefNo.Text;

                var myWindow = Window.GetWindow(this);

                if (editInvoice == 1 && InvoiceId != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Invoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Sale Invoice") != null))
                {

                    if (MainWindow.currentUserid == 0)
                    {

                    }
                    else if (saleInvoice.user_Id == null)
                        saleInvoice.user_Id = MainWindow.currentUserid;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null && saleInvoice.isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Sale Invoice is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            saleInvoice.stage = TransactionStage.Approved.ToString();

                            saleInvoice.isApproved = true;
                            saleInvoice.ApprovedDate = System.DateTime.Now;
                        }
                    }

                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        if (checkStatus.Id != saleInvoice.saleInvoiceStatus.Id)
                        {
                            saleInvoice.LastStatusChangeDate = System.DateTime.Now;
                        }
                    }
                    if (checkStatusClass != null && checkStatusClass.Id != 0)
                    {
                        if (checkStatusClass.Id != saleInvoice.statusClass_Id)
                        {
                            saleInvoice.LastStatusClassChangeDate = System.DateTime.Now;
                        }
                    }

                    if (checkStatus.Id != saleInvoice.saleInvoiceStatus.Id)
                    {
                        List<User> tagUsers = new List<User>();
                        List<User> ccUsers = new List<User>();
                        List<User> tagUsersRecommendation = new List<User>();
                        List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Sale Invoice has been changed, Do you want to notify other users by tagging?)", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                                // frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                win.ShowDialog();
                                tagUsers = win.tagUsers;
                                ccUsers = win.ccUsers;
                                tagUsersRecommendation = win.tagRecommendationUsers;
                                ccUsersRecommendation = win.ccRecommendationUsers;
                                if (win.tagUsers.Count > 0)
                                {
                                    if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                    {
                                        saleInvoice.holderChangeDate = DateTime.Now;
                                    }
                                    saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                }
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }
                        }
                        string oldStat = checkStatus.Status;
                        string newStat = saleInvoice.saleInvoiceStatus.Status;
                        string symbolCurr = "";

                        if (saleInvoice.currency != null)
                        {
                            symbolCurr = saleInvoice.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of Sales Invoice(Amount OC) having value: " + saleInvoice.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",
                            TaggedList = tagUsers,
                            CCUsersList = ccUsers,
                            TaggedRecomenndedList = tagUsersRecommendation,
                            CCRecomenndedList = ccUsersRecommendation


                        };
                        procurementRepo.Add(saleInvoice.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, 0, user.id, "New Comment ", null);
                            }

                        }

                        //SaleOrderss.ucStatuschange.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);

                        if (checkStatus != null && checkStatus.Id != 0)
                        {

                            UsersRepo.Add(TransactionInfo.Status_Changed, saleInvoice.Id, 5, "Status Changed from (" + checkStatus.Status + ") to (" + saleInvoice.saleInvoiceStatus.Status + ")");
                        }
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Edited, saleInvoice.Id, 5, frmInputBox.comment);
                    }
                    else
                    if (saleInvoice.StatusClass != null)
                    {
                        if (checkStatusClass.Id != saleInvoice.StatusClass.Id && saleInvoice.StatusClass != null)
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
                                    winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                                    // frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                    win.ShowDialog();
                                    tagUsers = win.tagUsers;
                                    ccUsers = win.ccUsers;
                                    tagUsersRecommendation = win.tagRecommendationUsers;
                                    ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count > 0)
                                    {
                                        if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                        {
                                            saleInvoice.holderChangeDate = DateTime.Now;
                                        }
                                        saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;

                                    }
                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();

                                }

                            }

                            string oldStat = saleInvoice.StatusClass.ClassName;
                            string newStat = checkStatusClass.ClassName;
                            string symbolCurr = "";

                            if (saleInvoice.currency != null)
                            {
                                symbolCurr = saleInvoice.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status Class of Sales Invoice(Amount OC) having value: " + saleInvoice.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Class Changed",
                                TaggedList = tagUsers,
                                CCUsersList = ccUsers,
                                TaggedRecomenndedList = tagUsersRecommendation,
                                CCRecomenndedList = ccUsersRecommendation


                            };
                            procurementRepo.Add(saleInvoice.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, 0, user.id, "New Comment ", null);
                                }

                            }

                            //SaleOrderss.ucStatuschange.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);

                            if (checkStatusClass != null && checkStatusClass.Id != 0)
                            {

                                UsersRepo.Add(TransactionInfo.Status_Class_Changed, saleInvoice.Id, 5, "Status Class Changed from (" + saleInvoice.StatusClass.ClassName + ") to (" + checkStatusClass.ClassName + ")");
                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Edited, saleInvoice.Id, 5, frmInputBox.comment);



                        }
                    }


                    saleInvoice.CustomerCredits = GetCustomerCredits();

                    saleInvoiceRepo.update(saleInvoice);
                    MessageBox.Show("SaleInvoice Updated Succesfully");
                    SystemLog.LogInfo(this.GetType(), "SaleInvoice Updated Succesfully refrence No= " + saleInvoice.referenceNo + " Id=" + saleInvoice.Id);
                    myWindow.Close();
                }
                else if (editInvoice != 1)
                {
                    txtSystemRefNo.Text = calculateSystemRefNo();
                    saleInvoice.BatchRefrenceNo = txtSystemRefNo.Text;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice") != null)
                    {
                        if (MainWindow.currentUserid == 0)
                        {
                            MessageBox.Show("Please Create another Account to Create Sale Invoice, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                            return;
                        }
                        else
                            saleInvoice.user_Id = MainWindow.currentUserid;
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null /*&& saleInvoice.isApproved == false*/)
                        {
                            saleInvoice.stage = TransactionStage.Approved.ToString();
                            saleInvoice.isApproved = true;
                            saleInvoice.ApprovedDate = System.DateTime.Now;
                        }
                        else
                        {
                            saleInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();
                            saleInvoice.isApproved = false;
                        }
                        if (cmbSaleInvoiceStatusClass.SelectedIndex > -1)
                        {
                            saleInvoice.LastStatusClassChangeDate = DateTime.Now;
                        }
                        saleInvoice.exchangeRate =(float) 1.00;
                        saleInvoiceRepo.Add(saleInvoice);
                        if (saleInvoice.SaleOrder == null && saleOrder != null)
                            saleInvoiceRepo.update(saleOrder);
                        SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                        SaleOrderss.ucStatuschange.inActiveStatuses = 0;
                        SaleOrderss.ucStatuschange.UpdateSaleOrder();//.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                        UsersRepo.Add(TransactionInfo.Initialized, saleInvoice.Id, 5, "");
                        UsersRepo.Add(TransactionInfo.Created_Invoice, SaleOrderss.ucStatuschange.saleOrder.Id, 3, "Sale Invoice genrated on this SaleOrder");
                        SystemLog.LogInfo(this.GetType(), "SaleInvoice Added Succesfully refrence No= " + saleInvoice.referenceNo + " Id=" + saleInvoice.Id);
                        MessageBox.Show("SaleInvoice Added Succesfully");
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                        myWindow.Close();
                        return;
                    }
                    myWindow.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Data Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SystemLog.LogError(this.GetType(), "SaleInvoice Error refrence No= " + saleInvoice.referenceNo + " Id=" + saleInvoice.Id + ex.ToString());

            }

        }
      public List<CustomerCredit> GetCustomerCredits()
        {
            List<CustomerCredit> customerCredits = new List<CustomerCredit>();
          
            if (grdCustomerCredits.ItemsSource!=null)
            {
                foreach (var customerCredit in grdCustomerCredits.ItemsSource as List<CustomerCredit>)
                {
                    CustomerCredit customer = new CustomerCredit();
                    customer.Id = customerCredit.Id;
                    customer.CustomerCompanyId = customerCredit.CustomerCompany.Id;
                    customer.SaleInvoiceId = saleInvoice.Id;
                    customer.creditAmount = customerCredit.creditAmount;
                    customer.SerialNo = customerCredit.SerialNo;
                    customerCredits.Add(customer);
                }
            }
            return customerCredits;
        }
        private void loadCurrencies()
        {
            cmbCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbDiscountCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbSTLCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
            if (cmbbaseCurrency.ItemsSource == null)
                cmbbaseCurrency.ItemsSource = SYSTEM_STATIC.currencySources;
        }

        public void loadSaleInvoiceStatus()
        {

            if (/*SystemLogic.AllowedPermissions.Find(x => x.Name == "View InActive Sale Invoice Statuses") != null ||*/ SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed SaleInvoice") != null)
            {
                SaleInvoiceStatuses = saleInvoiceRepo.getAllSaleInvoiceStatus();
            }
            else
                SaleInvoiceStatuses = saleInvoiceRepo.getAllActiveSaleInvoiceStatus();
            SaleInvoiceStatuses = SaleInvoiceStatuses.Where(x => x.isDisable != true).ToList();

            List<cmbitem> cmbitems = new List<cmbitem>();
            Parallel.ForEach(SaleInvoiceStatuses, delegate (SaleInvoiceStatus status) // foreach (SaleInvoiceStatus status in SaleInvoiceStatuses)
            {
                //string color = status.forecolor;
                //status.forecolor=(color!=null||color!="")?status.forecolor:"#FF000000";
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });


            });
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbSaleInvoiceStatus.ItemsSource = cmbitems;
        }
        public void loadSaleInvoiceStatus(SaleInvoiceStatus _status)
        {
            SaleInvoiceStatuses.Add(_status);
            List<cmbitem> cmbitems = new List<cmbitem>();
            Parallel.ForEach(SaleInvoiceStatuses, delegate (SaleInvoiceStatus status)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            });
            cmbSaleInvoiceStatus.ItemsSource = cmbitems;
        }  
        public void loadSaleInvoiceStatusClass(int _statusId)
        {
            cmbSaleInvoiceStatusClass.ItemsSource=null;
               var status = saleInvoiceRepo.getstatus(_statusId);
            if(status.siStatusSubClasses.Count>0)
            {
                StatusClasses.Clear();
                StatusClasses.AddRange(status.siStatusSubClasses.Where(x => x.isDisable != true).ToList());
                List<cmbitem> cmbitems = new List<cmbitem>();
                Parallel.ForEach(StatusClasses, delegate (ERP_BL.Procurements.StatusClass.StatusClass subClass)
                {
                    cmbitems.Add(new cmbitem() { name = subClass.ClassName, id = subClass.Id, bcolor = subClass.backcolor, fcolor = "#FF000000" });
                });
                cmbSaleInvoiceStatusClass.ItemsSource = cmbitems;
            }

        }   
        public void loadSaleInvoiceStatusClass(ERP_BL.Procurements.StatusClass.StatusClass statusClass)
        {
            StatusClasses.Add(statusClass);
            if(StatusClasses.Count>0)
            {
                List<cmbitem> cmbitems = new List<cmbitem>();
                Parallel.ForEach(StatusClasses, delegate (ERP_BL.Procurements.StatusClass.StatusClass subClass)
                {
                    cmbitems.Add(new cmbitem() { name = subClass.ClassName, id = subClass.Id, bcolor = subClass.backcolor, fcolor = "#FF000000" });
                });
                cmbSaleInvoiceStatusClass.ItemsSource = cmbitems;
            }
        }


        private void cmbSaleInvoiceStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbSaleInvoiceStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbSaleInvoiceStatus.SelectedItem as cmbitem).id;
                loadSaleInvoiceStatusClass(idd);
                if (idd == 0)
                {
                    Procurementss.SaleInvoicess.frmSaleInvoiceStatusAdd statusAdd = new Procurementss.SaleInvoicess.frmSaleInvoiceStatusAdd();
                    statusAdd.ShowDialog();
                    loadSaleInvoiceStatus();
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
                string selectedcust = customer.company.CompanyName + " (" + customer.contactPerson.FName + ")";
            }
        }
        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company != null)
            {
                string selecteddept = company.CompanyName;
                //lookupCompany.EditValue = selecteddept;
                //lookupCompany.DisplayMember = selecteddept;
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == company.CurrencyId)
                    {
                        cmbbaseCurrency.SelectedItem = cmbitem;
                        break;
                    }
                }
                loaddepartments();
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                lookupBank.ItemsSource = receiptRepo.GetAllBanksByCompany(company.Id);
            }
        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                if (department.ParentID == null && department.subDepartments.Count != 0)
                {
                    MessageBox.Show("Cannot map to a parent Department directly. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                loadcustomers();
                lookupVendor.ItemsSource = department.Vendors;
                lookupPrincipal.ItemsSource = department.Principals;
                ProductRepo productRepo = new ProductRepo();
                var products = productRepo.getAllDepartmentProducts(department.Id);
                lookupBookerProductsinGrid.ItemsSource = products;
                lookupProductsinGrid.ItemsSource = products;
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

            if(company != null && department != null)
            {
                TaskRepo taskRepo = new TaskRepo();
                var lotNumbers = taskRepo.GetAllLotNumberForCompDept(company.Id, department.Id);
                lookupLotNumbers.ItemsSource = lotNumbers;
            }
            loadPrincipals();
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
                string selectedvend = vendor.company.CompanyName + " (" + vendor.contactPerson.FName + ")";
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
                    symbol = name.Substring(name.IndexOf("("));
                    if (grdInvoiceItems.Columns.Count != 0)
                    {
                        lblTotal.Text = lblTotal.Text + symbol;
                        lblCFRTotal.Text = lblCFRTotal.Text + symbol;
                    }
                }
            }
        }
        string symbol;
        private static InquiryType PoType;
        private void cmbInquiryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSaleInvoiceType.SelectedItem != null)
            {
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.Supply)
                {
                    GridColumn nouwAmount = grdInvoiceItems.Columns.FirstOrDefault(x => x.Name == "colNowInvoiceAmount");
                    nouwAmount.ReadOnly = false;
                    lblCustomerName.Visibility = Visibility.Visible;
                    lookupCustomer.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    PoType = InquiryType.Supply;
                    grdDistributionItems.Visibility = Visibility.Collapsed;
                    grpProcItems.Visibility = Visibility.Visible;
                    lblTotalClaimDiscount.Visibility = Visibility.Collapsed;
                    txTotalClaimDiscount.Visibility = Visibility.Collapsed;
                    lblTotalPassOn.Visibility = Visibility.Collapsed;
                    txTotalPassOn.Visibility = Visibility.Collapsed;
                    lblTotalFocSampling.Visibility = Visibility.Collapsed;
                    txTotalFocSampling.Visibility = Visibility.Collapsed;
                    lblTotalNetAmount.Visibility = Visibility.Collapsed;
                    txTotalNetAmount.Visibility = Visibility.Collapsed;
                    btnBudgetCost.Visibility = Visibility.Collapsed;
                    btnBudgetCostPunching.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Visible;
                    btnCostSheetPunching.Visibility = Visibility.Visible;
                    btnCustomerCredit.Visibility = Visibility.Collapsed;

                }
                else if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.Tender)
                {
                    GridColumn nouwAmount = grdInvoiceItems.Columns.FirstOrDefault(x => x.Name == "colNowInvoiceAmount");
                    nouwAmount.ReadOnly = false;
                    lblCustomerName.Visibility = Visibility.Visible;
                    lookupCustomer.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    grdDistributionItems.Visibility = Visibility.Collapsed;
                    grpProcItems.Visibility = Visibility.Visible;
                    lblTotalClaimDiscount.Visibility = Visibility.Collapsed;
                    txTotalClaimDiscount.Visibility = Visibility.Collapsed;
                    lblTotalPassOn.Visibility = Visibility.Collapsed;
                    txTotalPassOn.Visibility = Visibility.Collapsed;
                    lblTotalFocSampling.Visibility = Visibility.Collapsed;
                    txTotalFocSampling.Visibility = Visibility.Collapsed;
                    lblTotalNetAmount.Visibility = Visibility.Collapsed;
                    txTotalNetAmount.Visibility = Visibility.Collapsed;
                    btnBudgetCost.Visibility = Visibility.Collapsed;
                    btnBudgetCostPunching.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Collapsed;
                    btnCostSheetPunching.Visibility = Visibility.Collapsed;
                    btnCustomerCredit.Visibility = Visibility.Collapsed;


                }

                else if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.Principal)
                {
                    lblCustomerName.Visibility = Visibility.Visible;
                    lookupCustomer.Visibility = Visibility.Visible;
                    GridColumn nouwAmount = grdInvoiceItems.Columns.FirstOrDefault(x => x.Name == "colNowInvoiceAmount");
                    nouwAmount.ReadOnly = false;
                    btnSummarySheet.Visibility = Visibility.Visible;
                    grdDistributionItems.Visibility = Visibility.Collapsed;
                    grpProcItems.Visibility = Visibility.Visible;
                    lblTotalClaimDiscount.Visibility = Visibility.Collapsed;
                    txTotalClaimDiscount.Visibility = Visibility.Collapsed;
                    lblTotalPassOn.Visibility = Visibility.Collapsed;
                    txTotalPassOn.Visibility = Visibility.Collapsed;
                    lblTotalFocSampling.Visibility = Visibility.Collapsed;
                    txTotalFocSampling.Visibility = Visibility.Collapsed;
                    lblTotalNetAmount.Visibility = Visibility.Collapsed;
                    txTotalNetAmount.Visibility = Visibility.Collapsed;
                    btnBudgetCost.Visibility = Visibility.Collapsed;
                    btnBudgetCostPunching.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Collapsed;
                    btnCostSheetPunching.Visibility = Visibility.Collapsed;
                    btnCustomerCredit.Visibility = Visibility.Collapsed;


                }
                else
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.Inventory)
                {
                    lblCustomerName.Visibility = Visibility.Visible;
                    lookupCustomer.Visibility = Visibility.Visible;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    PoType = InquiryType.Inventory;
                    grdDistributionItems.Visibility = Visibility.Collapsed;
                    grpProcItems.Visibility = Visibility.Visible;
                    lblTotalClaimDiscount.Visibility = Visibility.Collapsed;
                    txTotalClaimDiscount.Visibility = Visibility.Collapsed;
                    lblTotalPassOn.Visibility = Visibility.Collapsed;
                    txTotalPassOn.Visibility = Visibility.Collapsed;
                    lblTotalFocSampling.Visibility = Visibility.Collapsed;
                    txTotalFocSampling.Visibility = Visibility.Collapsed;
                    lblTotalNetAmount.Visibility = Visibility.Collapsed;
                    txTotalNetAmount.Visibility = Visibility.Collapsed;
                    btnBudgetCost.Visibility = Visibility.Collapsed;
                    btnBudgetCostPunching.Visibility = Visibility.Collapsed;
                    btnCustomerCredit.Visibility = Visibility.Collapsed;

                    //btnCostSheet.Visibility = Visibility.Collapsed;
                    //btnCostSheetPunching.Visibility = Visibility.Collapsed;

                }
                else
                 if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
                {
                    lblCustomerName.Visibility = Visibility.Visible;
                    lookupCustomer.Visibility = Visibility.Visible;


                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    PoType = InquiryType.DistributionBiz;
                    grdDistributionItems.Visibility = Visibility.Visible;
                    grpProcItems.Visibility = Visibility.Collapsed;
                    lblTotalClaimDiscount.Visibility = Visibility.Visible;
                    txTotalClaimDiscount.Visibility = Visibility.Visible;
                    lblTotalPassOn.Visibility = Visibility.Visible;
                    txTotalPassOn.Visibility = Visibility.Visible;
                    lblTotalFocSampling.Visibility = Visibility.Visible;
                    txTotalFocSampling.Visibility = Visibility.Visible;
                    lblTotalNetAmount.Visibility = Visibility.Visible;
                    txTotalNetAmount.Visibility = Visibility.Visible;
                    btnBudgetCost.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Collapsed;
                    btnCostSheetPunching.Visibility = Visibility.Collapsed;
                    btnCustomerCredit.Visibility = Visibility.Collapsed;

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Punch Budget System Cost") != null)
                    {
                        btnBudgetCostPunching.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnBudgetCostPunching.Visibility = Visibility.Collapsed;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget") != null)
                    {
                        btnBudgetCost.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnBudgetCost.Visibility = Visibility.Collapsed;
                    }

                }
                else
                 if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    lblCustomerName.Visibility = Visibility.Collapsed;
                    lookupCustomer.Visibility = Visibility.Collapsed;
                    btnSummarySheet.Visibility = Visibility.Collapsed;
                    PoType = InquiryType.DistributionBiz_CustomerCredit;
                    grdDistributionItems.Visibility = Visibility.Visible;
                    grpProcItems.Visibility = Visibility.Collapsed;
                    lblTotalClaimDiscount.Visibility = Visibility.Visible;
                    txTotalClaimDiscount.Visibility = Visibility.Visible;
                    lblTotalPassOn.Visibility = Visibility.Visible;
                    txTotalPassOn.Visibility = Visibility.Visible;
                    lblTotalFocSampling.Visibility = Visibility.Visible;
                    txTotalFocSampling.Visibility = Visibility.Visible;
                    lblTotalNetAmount.Visibility = Visibility.Visible;
                    txTotalNetAmount.Visibility = Visibility.Visible;
                    btnBudgetCost.Visibility = Visibility.Collapsed;
                    btnCostSheet.Visibility = Visibility.Collapsed;
                    btnCostSheetPunching.Visibility = Visibility.Collapsed;
                    btnCustomerCredit.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Punch Budget System Cost") != null)
                    {
                        btnBudgetCostPunching.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnBudgetCostPunching.Visibility = Visibility.Collapsed;
                    }
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget") != null)
                    {
                        btnBudgetCost.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btnBudgetCost.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        private void txttax_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txttax.Text != "")
            {
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
                {

                    CalculateBookerTotal();
                }
                else
                {
                    calculatetotal();
                }
            }
        }
        private void CalculateBookerTotal()
        {
            double sumfob = 0; double sumcfr = 0;
            double weight = 0;
            double Quantity = 0;
            double taxAmount = 0;
            double totalPassOn = 0;
            double totalClaimDiscount = 0;
            double totalFocSampling = 0;
            double totalNetAmount = 0;
            if (grdBokkerItems.ItemsSource != null)
                foreach (var item in grdBokkerItems.ItemsSource as List<BookerStatementItem>)
                {
                        if (item.quantity != 0)
                        {
                            Quantity += item.siQuantity;
                        }
                        else
                        {
                            weight += item.siWeight;
                        }
                        sumcfr += item.siAmount;
                        taxAmount += item.siAmountGST;
                    totalPassOn += item.siPassOnValue;
                    totalClaimDiscount += item.siClaimDiscountValue;
                    totalFocSampling += item.siFocValue;
                    totalNetAmount += item.siNetAmount;
                }
            txTotalClaimDiscount.Text = totalClaimDiscount.ToString();
            txTotalPassOn.Text = totalPassOn.ToString();
            txTotalFocSampling.Text = totalFocSampling.ToString();
            txTotalNetAmount.Text = totalNetAmount.ToString();
            txtWeight.Text = weight.ToString();
            txtQuantity.Text = Quantity.ToString();
            txtcfr.Text = sumcfr.ToString();
            txttax.Text = taxAmount.ToString();

            double cfr, tax = 0;

            cfr = Convert.ToDouble(txtcfr.Text);
            string str = txttax.Text.ToString();
            tax = taxAmount;

            double exchangeRate = 1;
            if (txtMER.Text != "")
                exchangeRate = Convert.ToDouble(txtMER.Text);
            if (str.IndexOf("%") != -1)
            {
                txttotalcfr.Text = (cfr + (cfr * tax / 100)).ToString();
                totalMER.Text = ((cfr * exchangeRate) + (cfr * tax / 100)).ToString();
                totalMER.Text = ((cfr * Convert.ToDouble(txtCMER.Text)) + (cfr * tax / 100)).ToString();
            }
            else
            {
                txttotalcfr.Text = (cfr + tax).ToString();
                totalMER.Text = ((cfr * exchangeRate) + tax).ToString();
                totalCMER.Text = ((cfr * Convert.ToDouble(txtCMER.Text)) + tax).ToString();

            }
            if (!string.IsNullOrEmpty(txttotalcfr.Text))
            {
                var roundedOffValue = SOCFRRemaining;
                var cfrRoundedOff =  Math.Round( Convert.ToDouble(txttotalcfr.Text), 2);
                var result = (roundedOffValue - cfrRoundedOff);
                txtSOremainingcfr.Text =Math.Round(result,2)  .ToString();
            }
            decimal marginexchangeRate = 1;
            decimal totalcfr = Convert.ToDecimal(txttotalcfr.Text);
            if (txtSER.Text != "")
            {
                marginexchangeRate = Convert.ToDecimal(txtSER.Text);
                txtTotalSER.Text = Math.Round(marginexchangeRate * totalcfr, 2).ToString();
            }
        }
        private void calculatetotal()
        {
            double sumfob = 0; double sumcfr = 0;
            decimal? weight = 0;
            double Quantity = 0;
            if (grdInvoiceItems.ItemsSource != null)
                foreach (var item in grdInvoiceItems.ItemsSource as List<ProcurementProduct>)
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

                        sumcfr += item.NowAmount;
                    }
                }
            txtWeight.Text = weight.ToString();
            txtQuantity.Text = Quantity.ToString();
            txtcfr.Text = sumcfr.ToString();
            decimal cfr, tax = 0;
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
            if (txtMER.Text != "")
                exchangeRate = Convert.ToDecimal(txtMER.Text);
            if (str.IndexOf("%") != -1)
            {
                txttotalcfr.Text = (cfr + (cfr * tax / 100)).ToString();
                totalMER.Text = ((cfr * exchangeRate) + (cfr * tax / 100)).ToString();
                totalMER.Text = ((cfr * Convert.ToDecimal(txtCMER.Text)) + (cfr * tax / 100)).ToString();
            }
            else
            {
                txttotalcfr.Text = (cfr + tax).ToString();
                totalMER.Text = ((cfr * exchangeRate) + tax).ToString();
                totalCMER.Text = ((cfr * Convert.ToDecimal(txtCMER.Text)) + tax).ToString();

            }
            if (!string.IsNullOrEmpty(txttotalcfr.Text))
            {
                var roundedOffValue = SOCFRRemaining;
                var cfrRoundedOff =  Math.Round( Convert.ToDouble(txttotalcfr.Text), 2);
                var result = (roundedOffValue - cfrRoundedOff);
                txtSOremainingcfr.Text =Math.Round(result,2)  .ToString();
            }
            decimal marginexchangeRate = 1;
            decimal totalcfr = Convert.ToDecimal(txttotalcfr.Text);
            if (txtSER.Text != "")
            {
                marginexchangeRate = Convert.ToDecimal(txtSER.Text);
                txtTotalSER.Text = Math.Round(marginexchangeRate * totalcfr, 2).ToString();
            }
            if (!string.IsNullOrEmpty(txtSOremainingcfr.Text) && !string.IsNullOrEmpty(txtSOtotalcfr.Text))
                if (Convert.ToDouble(txtSOremainingcfr.Text) < 0 || Math.Round( Convert.ToDouble(txtSOtotalcfr.Text), 2) < Math.Round( Convert.ToDouble(txtSOremainingcfr.Text), 2))
                {
                    DXMessageBox.Show("Sale Order can not be over invoiced, Please check again", "Remaining amount cant be less than 0.00 or greater than So amount");
                }
        }

        private void dGitems_CurrentCellChanged(object sender, EventArgs e)
        {
            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {

                CalculateBookerTotal();
            }
            else
            {
                calculatetotal();
            }
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
        }
        public void loadPaymentTerms()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            try
            {
                PaymentTermRepo TermRepo = new PaymentTermRepo();
                List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
                paymentTerms = TermRepo.getAllForSI();
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
                //lookupDepartment.Focus();
                return;
            }
        }

        private void lookupPrincipal_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            principal = lookupPrincipal.SelectedItem as Principal;
            if (principal != null)
            {
                string selectedvend = principal.company.CompanyName + " (" + principal.contactPerson.FName + ")";
            }
        }
        private void btnAddPrincipal_Click(object sender, RoutedEventArgs e)
        {

            Principalss.frmPrincipaladd Principaladd = new Principalss.frmPrincipaladd();
            Principaladd.ShowDialog();
            loadPrincipals();

        }
        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {
                CalculateBookerTotal();
                var row = e.Row as BookerStatementItem;
                row.siAmount = row.unit * row.siQuantity;
                row.siNetAmount = row.siAmount + row.siAmountGST - row.siPassOnValue - row.siClaimDiscountValue - row.siFocValue;
                row.amount = row.unit * row.quantity;
                row.netAmount = row.amount + row.amountGST - row.passOnValue - row.claimDiscountValue - row.focValue;

            }
            else
            {
                if (cmbSaleInvoiceType.SelectedItem != null)
                {
                    if (cmbSaleInvoiceType.SelectedItem.ToString() == InquiryType.Inventory.ToString())
                    {
                        var colum = e.Column;
                        var row = e.Row as ProcurementProduct;
                        if (row != null && colum.FieldName != "NowAmount")
                            row.NowAmount = row.InvoicedQuantity * row.unitPrice;
                    }
                    calculatetotal();
                }
            }
            CalculateExchangeRates();
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
            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz)
            {
            }
            else
            {
                (grdInvoiceItems.CurrentItem as ProcurementProduct).inquiryProduct = new InquiryProduct();
            }
        }
        private void winSaleInvoiceadd_Unloaded(object sender, RoutedEventArgs e)
        {
            editsaleInvoice = 0;
            saleOrderid = 0;
            saleInvoiceid = 0;
            if (saleInvoice != null && saleInvoice.Id != 0)
                UsersRepo.Add(TransactionInfo.viewed, saleInvoice.Id, 5, "Viewed details of Sale Invoice");
            SaleOrderss.frmCostSheet.costSheet = new ERP_BL.Databases.CostSheet();
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdInvoiceItems);
        }


        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateExchangeRates();
        }
        public void CalculateExchangeRates()
        {
            DateTime creationDate = (DateTime)datpoCreationdate.EditValue;
            DateTime d1 = new DateTime(2016, 01, 01);
            ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
            ExchangeRate exchangeRate = null;
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
                            default:
                                if (exchangeRate != null)
                                    txtMER.Text = 0.ToString();
                                break;
                        }
                    }
                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == (lookupCompany.SelectedItem as Company).Id);
                        switch (DateTime.Today.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateJan.ToString();
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateFeb.ToString();
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateMar.ToString();
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateApr.ToString();
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateMay.ToString();
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateJun.ToString();
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateJul.ToString();
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateAug.ToString();
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateSep.ToString();
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateOct.ToString();
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateNov.ToString();
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    txtCMER.Text = exchangeRate.rateDec.ToString();
                                break;
                            default:
                                if (exchangeRate != null)
                                    txtCMER.Text = 0.ToString();
                                break;
                        }
                    }

                }
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {

                    CalculateBookerTotal();
                }
                else
                {
                    calculatetotal();
                }
            }
            else
            {

                CurrencyRepo currencyRepo = new CurrencyRepo();

                ///Working One
                if (cmbCurrency.SelectedItem != null)
                {
                    MarketExchangeRate exchangeRate1 = new MarketExchangeRate();
                    SalesExchangeRate salesExchangeRate = new SalesExchangeRate();
                    exchangeRate1 = currencyRepo.getMarketexchangerate(company.Id, (cmbCurrency.SelectedItem as cmbitem).id);
                    salesExchangeRate = currencyRepo.getsalesexchangerate(company.Id, (cmbCurrency.SelectedItem as cmbitem).id);
                    if (exchangeRate1 != null)
                    {
                        txtMER.Text = exchangeRate1.exchangerate.ToString();
                    }
                    else if (cmbbaseCurrency.SelectedItem != null)
                        if ((cmbbaseCurrency.SelectedItem as cmbitem).id == (cmbCurrency.SelectedItem as cmbitem).id)
                        {
                            txtMER.Text = "1";
                            txtSER.Text = "1";
                        }
                    if (salesExchangeRate != null)
                    {
                        txtSER.Text = salesExchangeRate.exchangerate.ToString();
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
                        //string caption1 = "";
                        //string caption2 = "";
                        //if (cmbcaption1.SelectedItem != null)
                        //    caption1 = (cmbcaption1.SelectedItem as cmbitem).name;
                        //if (cmbcaption2.SelectedItem != null)
                        //    caption2 = (cmbcaption2.SelectedItem as cmbitem).name;

                        symbol = name.Substring(name.IndexOf("("));
                        if (grdInvoiceItems.Columns.Count != 0)
                        {
                            //grdInvoiceItems.Columns.GetColumnByFieldName("value1").Header = caption1 + symbol;
                            //grdInvoiceItems.Columns.GetColumnByFieldName("value2").Header = caption2 + symbol;
                            lblTotal.Text = "Sub Total" + symbol;
                            lblCFRTotal.Text = "Total" + symbol;
                            string stri1 = lblbaseTotal.Text;
                            if (-1 != stri1.IndexOf("("))
                                lblbaseTotal.Text = (stri1.Substring(0, stri1.IndexOf("("))) + symbol;
                            else
                                lblbaseTotal.Text = lblbaseTotal.Text + " " + symbol;
                        }
                        if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                        {
                            CalculateBookerTotal();
                        }
                        else
                        {
                            calculatetotal();
                        }

                    }
                }
            }
        }

        private void CmbbaseCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            }
        }

        private void TxtBudgetMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

            //calculateBaseBudgetMargin();
        }

        private void TxtActualMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            //calculateBaseActualMargin();
        }
        private void TxtCommision_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            //calculateBaseCommision();
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
        }

        private void TxtMarginexchangerate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {

                CalculateBookerTotal();
            }
            else
            {
                calculatetotal();
            }

        }

        private void Txtexchangerate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
            {

                CalculateBookerTotal();
            }
            else
            {
                calculatetotal();
            }
        }

        private void BtnCostSheet_Click(object sender, RoutedEventArgs e)
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
                SaleOrderss.frmCostSheet frmCostSheet = new SaleOrderss.frmCostSheet(saleOrder, txtFinanaceRef.Text, txtSalesref.Text, lookupDepartment.Text, lookupCustomer.Text, symbol, txttotalcfr.Text, inco, datpoCreationdate.Text, paymentterm, txtMaker.Text, txtOrigin.Text, views, (InquiryType)cmbSaleInvoiceType.SelectedIndex, lookupPrincipal.Text, saleOrder.packing, datDeliverydate.DateTime, (saleOrder.Warranty == null) ? "" : saleOrder.Warranty.name, txtsaleInvoiceref.Text, datsaleInvoicedate.Text, saleInvoice.isApproved, saleOrder.isReApproved);
                frmCostSheet.grdCostItems.Columns["budgetedValuePerc"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["AttestedCOO"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["isExportLicense"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["exchangeRate"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["OCamount"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                frmCostSheet.grdCostItems.Columns["budgetedValue"].AllowEditing = DevExpress.Utils.DefaultBoolean.False;
                //frmCostSheet.lookupVendors.IsEnabled = false;
                //frmCostSheet.lookupOC.IsEnabled = false;
                frmCostSheet.ShowDialog();

                //{
                //    if (SaleOrderss.frmCostSheet.costSheet != null)
                //    {
                //        //saleInvoice.CostSheet = SaleOrderss.frmCostSheet.costSheet;
                //        txtBudgetMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalBudgetedMargin).ToString();
                //        txtActualMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) != (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalActualMargin)) ? (Convert.ToDecimal(txttotalcfr.Text) - saleOrder.CostSheet.TotalActualMargin).ToString() : "0";
                //    }
                //}

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
           if (gridTracker.Visibility == Visibility.Collapsed && saleInvoice.Id != 0)
            {

                views = UsersRepo.getViwerInfo(saleInvoice.Id, (int)TransactionItemType.Sale_Invoice);
                grdUsers.ItemsSource = views;
                gridTracker.Visibility = Visibility.Visible;
            }
        }
        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            if (saleInvoice.isApproved == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null)
                {
                    saleInvoice.isReviewed = true;
                    saleInvoice.isApproved = true;
                    saleInvoice.stage = TransactionStage.Approved.ToString();
                    saleInvoiceRepo.updateForDirectClose(saleInvoice);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("SI has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (saleInvoice.department != null && saleInvoice.department.Id != 0 && saleInvoice.company?.Id != 0 && saleInvoice.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && saleInvoice.InterDepartment != null && saleInvoice.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleInvoice.holderChangeDate = DateTime.Now;
                                }
                                saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                saleInvoiceRepo.updateInvoice(saleInvoice);
                            }
                        }
                        else if (saleInvoice.department != null && saleInvoice.department.Id != 0 && saleInvoice.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleInvoice.holderChangeDate = DateTime.Now;
                                }
                                saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                saleInvoiceRepo.updateInvoice(saleInvoice);
                            }
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }
                    }

                    string symbolCurr = "";
                    if (saleInvoice.currency != null)
                    {
                        symbolCurr = saleInvoice.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "SI (Amount OC) having value: " + saleInvoice.totalInvoiceAmount.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "SI Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(saleInvoice.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SI #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SI #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }


                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, saleInvoice.Id, 5, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null))
                {
                    saleInvoice.isReviewed = true;
                    saleInvoice.needReview = false;
                    saleInvoice.stage = TransactionStage.AwaitingApproval.ToString();

                    saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, saleInvoice.Id, 5, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null))
                {
                    saleInvoice.isReviewed = true;
                    saleInvoice.needReview = true;
                    saleInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();

                    saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, saleInvoice.Id, 5, frmInputBox.comment);
                        addinfo = false;
                    }
                }
            }
            else if(saleInvoice.isApproved == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null)
                {
                    saleInvoice.isReviewed = true;
                    saleInvoice.isApproved = false;
                    saleInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                    saleInvoiceRepo.update(saleInvoice);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("SI has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (saleInvoice.department != null && saleInvoice.department.Id != 0 && saleInvoice.company?.Id != 0 && saleInvoice.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && saleInvoice.InterDepartment != null && saleInvoice.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleInvoice.holderChangeDate = DateTime.Now;
                                }
                                saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                saleInvoiceRepo.updateInvoice(saleInvoice);
                            }
                        }
                        else if (saleInvoice.department != null && saleInvoice.department.Id != 0 && saleInvoice.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleInvoice.holderChangeDate = DateTime.Now;
                                }
                                saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                saleInvoiceRepo.updateInvoice(saleInvoice);
                            }
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }
                    }

                    string symbolCurr = "";
                    if (saleInvoice.currency != null)
                    {
                        symbolCurr = saleInvoice.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "SI (Amount OC) having value: " + saleInvoice.totalInvoiceAmount.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                        Timestamp = DateTime.Now,
                        Subject = "SI UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(saleInvoice.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SI #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SI #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }


                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, saleInvoice.Id, 5, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }
            else if (saleInvoice.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null)
                {
                    saleInvoice.isReviewed = true;
                    saleInvoice.PendingForClosing = false;
                    saleInvoice.stage = TransactionStage.Closed.ToString();
                    saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Closing, saleInvoice.Id, 5, frmInputBox.comment);
                        addinfo = false;
                    }
                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null))
                {
                    saleInvoice.isReviewed = true;
                    saleInvoice.needReview = false;
                    saleInvoice.stage = TransactionStage.AwaitingApproval.ToString();

                    saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, saleInvoice.Id, 5, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null))
                {
                    saleInvoice.isReviewed = true;
                    saleInvoice.needReview = true;
                    saleInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();

                    saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, saleInvoice.Id, 5, frmInputBox.comment);
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

            if (saleInvoice.isApproved != true)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null)
                {
                    saleInvoice.isReviewed = true;
                    saleInvoice.isApproved = false;
                    saleInvoice.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, saleInvoice.Id, (int)TransactionItemType.Sale_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleInvoiceRepo.update(saleInvoice);
                    notificationsRepo.Add("SaleInvoice Rejected", saleInvoice.Id, TransactionItemType.Sale_Invoice, "SaleInvoice with refrence # " + saleInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleInvoice" + frmInputBox.comment, null);

                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null))
                {
                    saleInvoice.isReviewed = false;
                    saleInvoice.stage = TransactionStage.Rejected.ToString();
                    //if(saleInvoice.isApproved == null)
                    //{
                    //    saleInvoice.isApproved = false;

                    //}
                    //saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, saleInvoice.Id, (int)TransactionItemType.Sale_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleInvoiceRepo.update(saleInvoice);
                    notificationsRepo.Add("SaleInvoice Rejected", saleInvoice.Id, TransactionItemType.Sale_Invoice, "SaleInvoice with refrence # " + saleInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleInvoice" + frmInputBox.comment, null);

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null))
                {
                    saleInvoice.isReviewed = false;
                    //if (saleInvoice.isApproved == null)
                    //{
                    //    saleInvoice.isApproved = false;

                    //}
                    saleInvoice.stage = TransactionStage.Rejected.ToString();
                    //saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, saleInvoice.Id, (int)TransactionItemType.Sale_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleInvoiceRepo.update(saleInvoice);
                    notificationsRepo.Add("SaleInvoice Rejected", saleInvoice.Id, TransactionItemType.Sale_Invoice, "SaleInvoice with refrence # " + saleInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleInvoice" + frmInputBox.comment, null);

                }
            }
            else if (saleInvoice.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null)
                {
                    saleInvoice.isReviewed = true;
                    //if (saleInvoice.PendingForClosing == null)
                    {
                        saleInvoice.PendingForClosing = true;

                    }
                    //saleInvoice.PendingForClosing = false;
                    saleInvoice.stage = TransactionStage.Rejected.ToString();
                    //saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, saleInvoice.Id, (int)TransactionItemType.Sale_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleInvoiceRepo.update(saleInvoice);
                    notificationsRepo.Add("SaleInvoice Rejected", saleInvoice.Id, TransactionItemType.Sale_Invoice, "SaleInvoice with refrence # " + saleInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleInvoice" + frmInputBox.comment, null);

                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null))
                {
                    saleInvoice.isReviewed = true;
                    saleInvoice.needReview = false;
                    saleInvoice.stage = TransactionStage.Rejected.ToString();
                    //if (saleInvoice.PendingForClosing == null)
                    //{
                    //    saleInvoice.PendingForClosing = true;

                    //}
                    //saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, saleInvoice.Id, (int)TransactionItemType.Sale_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleInvoiceRepo.update(saleInvoice);
                    notificationsRepo.Add("SaleInvoice Rejected", saleInvoice.Id, TransactionItemType.Sale_Invoice, "SaleInvoice with refrence # " + saleInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleInvoice" + frmInputBox.comment, null);

                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null))
                {
                    //if (saleInvoice.PendingForClosing == null)
                    //{
                    //    saleInvoice.PendingForClosing = true;

                    //}
                    saleInvoice.isReviewed = true;
                    saleInvoice.needReview = true;
                    saleInvoice.stage = TransactionStage.Rejected.ToString();

                    //saleInvoiceRepo.update(saleInvoice);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, saleInvoice.Id, (int)TransactionItemType.Sale_Invoice, frmInputBox.comment);
                        addinfo = false;
                    }
                    saleInvoiceRepo.update(saleInvoice);
                    notificationsRepo.Add("SaleInvoice Rejected", saleInvoice.Id, TransactionItemType.Sale_Invoice, "SaleInvoice with refrence # " + saleInvoice.SalesReferenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", saleInvoice.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this SaleInvoice" + frmInputBox.comment, null);

                }
            }
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sale Invoice") != null)
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
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Sale_Invoice);
                inputBox.ShowDialog();
            }
            else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (saleInvoice != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && saleInvoice.Id != 0)
                {
                    var commentId = procurementRepo.AddCommentLinkNotification(saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                    if (commentId != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SI #" + saleInvoice.referenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                            else
                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SI #" + saleInvoice.referenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                        }

                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SI #" + saleInvoice.referenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                            else
                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SI #" + saleInvoice.referenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

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
                else if (saleInvoice.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Invoice first to add a comment!");
                }

            }
        }
        public void loadcomments()
        {
            try
            {
                if (saleInvoice != null)
                {
                    if (isLoadComment == false)
                    {
                        isLoadComment = true;
                        comments = procurementRepo.getcommentslogAsc(saleInvoice.Id, TransactionItemType.Sale_Invoice);
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
                                    txtComments.Text = comments[count].Comment;
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
                SaleOrderss.frmCommissionSummarySheet frmSummarySheet = new SaleOrderss.frmCommissionSummarySheet(saleOrder, txtFinanaceRef.Text, txtsaleInvoiceref.Text, txtOfferRefNo.Text, lookupCustomer.Text, (cmbCurrency.SelectedItem as cmbitem).id, saleOrder.commision.ToString(), datsaleInvoicedate.Text, paymentterm, (InquiryType)cmbSaleInvoiceType.SelectedIndex, lookupPrincipal.Text, txttotalcfr.Text, saleOrder.totalFOBValue.ToString(), "", inco, datDeliverydate.DateTime, "", datpoCreationdate.Text, txtSalesref.Text, views/*,TransactionItemType.Sale_Invoice*/);
                frmSummarySheet.ShowDialog();
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.Principal)
                {
                    //if (SaleOrderss.frmCommissionSummarySheet.summarySheet != null)
                    //{
                    //    saleInvoice.CostSheet = null;
                    //    //saleInvoice.CommissionSummarySheet = SaleOrderss.frmCommissionSummarySheet.summarySheet;
                    //    //txtCommision.Text = (saleInvoice.CommissionSummarySheet.SOCommission).ToString();

                    //}
                }


            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void GridColumn_Validate(object sender, GridCellValidationEventArgs e)
        {
            var Totalquantity = ((ProcurementProduct)e.Row).inquiryProduct.quantity;
            var TotalInvoicedQTy = ((ProcurementProduct)e.Row).TotalInvoicedQuantity;
            var unInvoicedQTy = ((ProcurementProduct)e.Row).UnInvoicedQuantity;


            var invoicedqty = Convert.ToDouble(e.Value);
            if (Totalquantity < invoicedqty || invoicedqty < 0)
            {

                //if (!(invoicedqty > 0 && discount <= 30))
                {
                    e.IsValid = false;
                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                    if (invoicedqty < 0)
                    {
                        e.ErrorContent = string.Format("The Invoiced Quantity cannot be greater than ${0}",
                                                    Convert.ToDouble(e.CellValue));
                        return;
                    }
                    e.ErrorContent = string.Format(
                       "The Invoiced Quantity cannot be greater than (${0}). Please correct the Invoiced Quantity.",
                       Convert.ToDouble(e.CellValue));
                }
            }
            else
            {
                var newUnInvQty = Totalquantity - (TotalInvoicedQTy + invoicedqty);
                grdInvoiceItems.SetFocusedRowCellValue("UnInvoicedQuantity", newUnInvQty);
            }

        }

        private void GridColumn_Validate_1(object sender, GridCellValidationEventArgs e)
        {
            var Totalweight = Convert.ToDouble(((ProcurementProduct)e.Row).inquiryProduct.Weight);
            var TotalInvoicedWeight = ((ProcurementProduct)e.Row).TotalInvoicedWeight;
            var unInvoicedWeight = ((ProcurementProduct)e.Row).UnInvoicedWeight;
            var invoicedwgt = Convert.ToDouble(e.Value);
            if (Totalweight < invoicedwgt)
            {

                //if (!(invoicedqty > 0 && discount <= 30))
                {
                    e.IsValid = false;
                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                    if (invoicedwgt < 0)
                    {
                        e.ErrorContent = string.Format("The Invoiced Weight can't be negative!",
                                                    Convert.ToDouble(e.CellValue));
                        return;
                    }
                    e.ErrorContent = string.Format(
                       "The Invoiced Weight cannot be greater than (${0}). Please correct the Invoiced Weight.",
                       Convert.ToDouble(e.CellValue));
                }
            }
            else
            {
                var UnInv =/*(unInvoicedWeight==0)? */Totalweight - (TotalInvoicedWeight + invoicedwgt)/*: TotalInvoicedWeight - (unInvoicedWeight + invoicedwgt)*/;
                grdInvoiceItems.SetFocusedRowCellValue("UnInvoicedWeight", UnInv);
            }
        }

        private void GrdInvoiceItems_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {
                // if (e.GetListSourceFieldValue("employee.person.FName") != null|| e.GetListSourceFieldValue("employee.person.LName") != null )
                {
                    decimal commission = 0;
                    if (saleInvoice.SaleOrder != null)
                    {
                        if (saleInvoice.SaleOrder.commision != null)
                        {
                            commission = Convert.ToDecimal(saleInvoice.SaleOrder.commision);
                        }

                    }
                    else if (saleOrder != null)
                    {
                        if (saleOrder.commision != null)
                        {
                            commission = Convert.ToDecimal(saleOrder.commision);

                        }

                    }




                    //DateTime date;

                    e.Value = commission;
                }
            }
        }

        private void ColNowInvoiceAmount_Validate(object sender, GridCellValidationEventArgs e)
        {
            if (cmbSaleInvoiceType.SelectedItem != null)
            {
                if (cmbSaleInvoiceType.SelectedItem.ToString() == InquiryType.Principal.ToString())
                {
                    var Total = Convert.ToDouble(((ProcurementProduct)e.Row).totalCommission);
                    var TotalInvoiced = ((ProcurementProduct)e.Row).totalInvoicedSoAmount;
                    var unInvoiced = ((ProcurementProduct)e.Row).UnInvoicedSoAmount;
                    var invoiced = Convert.ToDouble(e.Value);
                    if (Total < invoiced)
                    {

                        //if (!(invoicedqty > 0 && discount <= 30))
                        {
                            e.IsValid = false;
                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            if (invoiced < 0)
                            {
                                e.ErrorContent = string.Format("The invoiced amount can't be negative!",
                                                            Convert.ToDouble(e.CellValue));
                                return;
                            }
                            e.ErrorContent = string.Format(
                               "The Invoiced amount cannot be greater than (${0}). Please correct the Invoiced amount.",
                               Convert.ToDouble(e.CellValue));
                        }
                    }
                    else
                    {
                        var UnInv = Total - (TotalInvoiced + invoiced);
                        grdInvoiceItems.SetFocusedRowCellValue("UnInvoicedSoAmount", UnInv);
                    }
                }
                else if (cmbSaleInvoiceType.SelectedItem.ToString() != InquiryType.Principal.ToString())
                {
                    var Total = Convert.ToDouble(((ProcurementProduct)e.Row).value2);
                    var TotalInvoiced = ((ProcurementProduct)e.Row).totalInvoicedSoAmount;
                    var unInvoiced = ((ProcurementProduct)e.Row).UnInvoicedSoAmount;
                    var invoiced = Convert.ToDouble(e.Value);
                    if (Total < invoiced)
                    {

                        //if (!(invoicedqty > 0 && discount <= 30))
                        {
                            e.IsValid = false;
                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            if (invoiced < 0)
                            {
                                e.ErrorContent = string.Format("The invoiced amount can't be negative!",
                                                            Convert.ToDouble(e.CellValue));
                                return;
                            }
                            e.ErrorContent = string.Format(
                               "The Invoiced amount cannot be greater than (${0}). Please correct the Invoiced amount.",
                               Convert.ToDouble(e.CellValue));
                        }
                    }
                    else
                    {
                        var UnInv = Total - (TotalInvoiced + invoiced);
                        grdInvoiceItems.SetFocusedRowCellValue("UnInvoicedSoAmount", UnInv);
                    }
                }

            }
           
        }
        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (saleInvoice.saleInvoiceStatus.isActive==false && saleInvoice.PendingForClosing!=true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can attach document when Sale Invoice Closed") != null)
                {
                    if (grdAttach.Visibility == Visibility.Visible)
                        grdAttach.Visibility = Visibility.Collapsed;
                    else
                    {
                        if (saleInvoice.Id != 0)
                        {
                            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                        }
                        grdAttach.Visibility = Visibility.Visible;

                    }
                }
                else
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required "+ "Can attach document when Sale Invoice Closed!");
            }
            else
            {
                if (grdAttach.Visibility == Visibility.Visible)
                    grdAttach.Visibility = Visibility.Collapsed;
                else
                {
                    if (saleInvoice.Id != 0)
                    {
                        cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                    }
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
                if (saleInvoice.Id != 0)
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(saleInvoice.Id, TransactionItemType.Sale_Invoice);
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
                if (saleInvoiceid != 0)
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
                        destination += "Attachments\\Sale_Invoice\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += saleInvoiceid + "_" + TransactionItemType.Sale_Invoice.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Sale_Invoice);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), saleInvoiceid, TransactionItemType.Sale_Invoice, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, saleInvoice.Id, 5, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(saleInvoiceid, TransactionItemType.Sale_Invoice);
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

                            //MessageBox.Show("Attachment Uploaded");


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

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (InvoiceId != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(InvoiceId, TransactionItemType.Sale_Invoice);
                trackingWindow.ShowDialog();
            }
        }


        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            var SRcount = saleInvoice.salesReceipts.Where(x=>x.isVoid != true).Count();
            if (SRcount > 0)
            {
                DXMessageBox.Show("This Sale Invoice cannot be Voided because it has Active Sale Receipts!");
                return;
            }
            if (saleInvoice.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void SaleInvoice") != null))
            {
                if (DXMessageBox.Show("This Invoice is currently in the list of Void Sale Invoices! Do you want to remove it from Void?", "Remove Void Sale Invoice", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    saleInvoice.isVoid = false;
                    saleInvoiceRepo.setSaleInvoicetoVoid(saleInvoice.Id, false);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("SI has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (saleInvoice.department != null && saleInvoice.department.Id != 0 && saleInvoice.company?.Id != 0 && saleInvoice.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && saleInvoice.InterDepartment != null && saleInvoice.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleInvoice.holderChangeDate = DateTime.Now;
                                }
                                saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                saleInvoiceRepo.update(saleInvoice);
                            }
                        }
                        else if (saleInvoice.department != null && saleInvoice.department.Id != 0 && saleInvoice.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleInvoice.holderChangeDate = DateTime.Now;
                                }
                                saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                saleInvoiceRepo.update(saleInvoice);
                            }
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (saleInvoice.currency != null)
                    {
                        symbolCurr = saleInvoice.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "SI (Amount OC) having value: " + saleInvoice.totalInvoiceAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "SI UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(saleInvoice.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SI #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ",null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SI #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void SaleInvoice") != null)
            {
                if (DXMessageBox.Show("This Invoice is not currently in the list of Void Sale Invoices! Do you want to move it to Void Saleorders?", "Add to Void List", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    saleInvoice.isVoid = true;
                    saleInvoiceRepo.setSaleInvoicetoVoid(saleInvoice.Id, true);

                    grdVoid.Visibility = Visibility.Visible;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("SI has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (saleInvoice.department != null && saleInvoice.department.Id != 0 && saleInvoice.company?.Id != 0 && saleInvoice.InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && saleInvoice.InterDepartment != null && saleInvoice.InterDepartment.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleInvoice.holderChangeDate = DateTime.Now;
                                }
                                saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                saleInvoiceRepo.update(saleInvoice);
                            }
                        }
                        else if (saleInvoice.department != null && saleInvoice.department.Id != 0 && saleInvoice.company?.Id != 0 && chkInterCompany.IsChecked != true/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleInvoice.holderChangeDate = DateTime.Now;
                                }
                                saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                saleInvoiceRepo.update(saleInvoice);
                            }
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (saleInvoice.currency != null)
                    {
                        symbolCurr = saleInvoice.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "SI (Amount OC) having value: " + saleInvoice.totalInvoiceAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as void \nFrom: ",
                        Timestamp = DateTime.Now,
                        Subject = "SI Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(saleInvoice.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SI #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in SI #" + saleInvoice.SalesReferenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                }
            }
            //loadcomments();
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
                if (saleInvoice.Id != 0)
                {

                    if (department != null && department.Id != 0 && company?.Id != 0 && InterCompany?.Id != 0 && chkInterCompany.IsChecked == true && InterDepartment != null && InterDepartment.Id != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), comment, TransactionItemType.Sale_Invoice);
                        inputBox.ShowDialog();
                    }
                    else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.Sale_Invoice);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (saleInvoice != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && saleInvoice.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SI #" + saleInvoice.referenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in SI #" + saleInvoice.referenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SI #" + saleInvoice.referenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in SI #" + saleInvoice.referenceNo, saleInvoice.Id, TransactionItemType.Sale_Invoice, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (saleInvoice.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Sale Invoice first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null) ? true : false)
            {
                if (saleInvoiceid == 0)
                {
                    return;
                }
                saleInvoice = saleInvoiceRepo.get(saleInvoiceid);
                var receiptsAmount= Math.Round(saleInvoice.salesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount), 2);

                double result = 0;
                if (saleInvoice.saleInvoicetype == InquiryType.DistributionBiz || saleInvoice.saleInvoicetype == InquiryType.DistributionBiz_CustomerCredit)
                {
                    var total = Math.Round(saleInvoice.BookerStatementItems.Sum(x => x.siNetAmount), 2); ;
                    result = total - receiptsAmount;
                }
                else
                {
                    var total = Math.Round(saleInvoice.totalInvoiceAmount, 2);
                    result = total - receiptsAmount;
                }
                

                if (result != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Invoice without receiving fully Collection") != null)
                    {
                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to close sale Invoice without complete collection?" , "Information", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                        {
                            UsersRepo usersRepo = new UsersRepo();
                            var row = saleInvoice;
                            if (row.saleInvoiceStatus != null)
                            {
                                oldStatus = row.saleInvoiceStatus;
                            }
                            SaleInvoicess.ucStatuschange.saleInvoiceid = saleInvoiceid;
                            SaleInvoicess.frmSaleInvoiceStatusChange statusChange = new SaleInvoicess.frmSaleInvoiceStatusChange();
                            var myWindow = Window.GetWindow(this);
                            statusChange.Owner = myWindow;
                            statusChange.ShowDialog();
                            if (SaleInvoicess.ucStatuschange.saleInvoice.Id != 0)
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null) ? true : false)
                                {
                                    SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = false;
                                    SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Closing, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.saleInvoiceRepo.update(Inquiriess.ucStatuschange.saleInvoice);

                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                                {
                                    SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                                    if (SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing == null)
                                    {
                                        SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null)
                                {
                                    SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing == null)
                                    {
                                        SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                                    }
                                    //SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                                }
                                else
                                {
                                    SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();

                                    SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;
                                }
                            //SaleInvoicess.ucStatuschange.saleInvoice.user_Id = MainWindow.currentUserid;
                            SaleInvoicess.ucStatuschange.saleInvoice.ClosingDate = System.DateTime.Now;
                            SaleInvoicess.ucStatuschange.saleInvoice.LastStatusChangeDate = System.DateTime.Now;
                            SaleInvoicess.ucStatuschange.saleInvoiceRepo.updateForDirectClose(SaleInvoicess.ucStatuschange.saleInvoice);
                            SaleOrderss.ucStatuschange.saleOrderid = (int)saleInvoice.SaleOrderId;
                            SaleOrderss.frmSaleOrderStatusChange statChange = new SaleOrderss.frmSaleOrderStatusChange();
                            SaleOrderss.ucStatuschange.inActiveStatuses = 0;                    //Show Only active SO Statusses

                            statChange.Owner = myWindow;
                            statChange.ShowDialog();
                            //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                            SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                            SaleOrderss.ucStatuschange.UpdateSaleOrderStatusforInvoice();//.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                            MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                            MessageBox.Show("SaleInvoice status changed to InActive (" + SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status + ")");

                            //Adding auto Signature
                            //if (row.saleInvoiceStatus.Status != SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status)
                            //{
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Sales Invocie has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                {
                                    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                    if (win.tagUsers.Count != 0)
                                    {
                                        if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                        {
                                            saleInvoice.holderChangeDate = DateTime.Now;
                                        }
                                        saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                        saleInvoiceRepo.updateInvoice(saleInvoice);
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
                            string newStat = SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status;
                            string symbolCurr = "";
                            if (row.currency != null)
                            {
                                symbolCurr = row.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Sale Invoice(Amount OC) having value: " + row.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            if (saleInvoice.StatusClass != null)
                            {
                                string oldStatClass = saleInvoice.StatusClass.ClassName;

                                string newStatClass = checkStatusClass.ClassName;
                                if (checkStatusClass.Id != saleInvoice.StatusClass.Id && saleOrder.StatusClass != null)
                                {
                                    comment.Comment = "Status and Status Class of Sales Invoice having Invoice Amount (OC): " + row.totalInvoiceAmount.ToString() + " (" + symbolCurr + ")\n "
                                     + "\nhas been changed \n"
                                     + "Status From: " + oldStat + " \nTo: " + newStat
                                     + "Status From: " + oldStatClass + " \nTo: " + newStatClass;                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                    comment.Subject = "Status and Status Class Changed using direct close";
                                }
                            }


                            procurementRepo.Add(row.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);
                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Invoice, comment.Comment, 0,user.id, "New Comment ", null);
                                }
                            }
                            var thiWindow = Window.GetWindow(this);
                            thiWindow.Close();
                        }
                        else
                            return;
                        
                    }
                    else
                        DXMessageBox.Show("Sale Invoice is not fully received yet, or you need permission to Close Invoice without receiving fully Collection");
                }
                else
                {
                    UsersRepo usersRepo = new UsersRepo();

                    var row = saleInvoice;
                    if (row.saleInvoiceStatus != null)
                    {
                        oldStatus = row.saleInvoiceStatus;
                    }
                    SaleInvoicess.ucStatuschange.saleInvoiceid = saleInvoiceid;
                    SaleInvoicess.frmSaleInvoiceStatusChange statusChange = new SaleInvoicess.frmSaleInvoiceStatusChange();
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (SaleInvoicess.ucStatuschange.saleInvoice.Id != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null) ? true : false)
                        {
                            SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = false;
                            SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.saleInvoiceRepo.update(Inquiriess.ucStatuschange.saleInvoice);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                        {
                            SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                            if (SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing == null)
                            {
                                SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null)
                        {
                            SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing == null)
                            {
                                SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                            }
                            //SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                        }
                        else
                        {
                            SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();

                            SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;
                        }
                    //SaleInvoicess.ucStatuschange.saleInvoice.user_Id = MainWindow.currentUserid;
                    SaleInvoicess.ucStatuschange.saleInvoice.ClosingDate = System.DateTime.Now;
                    SaleInvoicess.ucStatuschange.saleInvoice.LastStatusChangeDate = System.DateTime.Now;
                    SaleInvoicess.ucStatuschange.saleInvoiceRepo.updateForDirectClose(SaleInvoicess.ucStatuschange.saleInvoice);
                    SaleOrderss.ucStatuschange.saleOrderid = (int)saleInvoice.SaleOrderId;
                    SaleOrderss.frmSaleOrderStatusChange statChange = new SaleOrderss.frmSaleOrderStatusChange();
                    SaleOrderss.ucStatuschange.inActiveStatuses = 0;                    //Show Only active SO Statusses

                    statChange.Owner = myWindow;
                    statChange.ShowDialog();
                    //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                    SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                    SaleOrderss.ucStatuschange.UpdateSaleOrderStatusforInvoice();//.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                    MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                    MessageBox.Show("SaleInvoice status changed to InActive (" + SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status + ")");

                    //Adding auto Signature
                    //if (row.saleInvoiceStatus.Status != SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status)
                    //{
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Sales Invocie has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (saleInvoice.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    saleInvoice.holderChangeDate = DateTime.Now;
                                }
                                saleInvoice.transactionHolderId = win.tagUsers[0].employeeId;
                                saleInvoiceRepo.updateInvoice(saleInvoice);
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
                    string newStat = SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status;
                    string symbolCurr = "";
                    if (row.currency != null)
                    {
                        symbolCurr = row.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of Sale Invoice(Amount OC) having value: " + row.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    if (saleInvoice.StatusClass != null)
                    {
                        string oldStatClass = saleInvoice.StatusClass.ClassName;

                        string newStatClass = checkStatusClass.ClassName;
                        if (checkStatusClass.Id != saleInvoice.StatusClass.Id && saleOrder.StatusClass != null)
                        {
                            comment.Comment = "Status and Status Class of Sales Invoice having Invoice Amount (OC): " + row.totalInvoiceAmount.ToString() + " (" + symbolCurr + ")\n "
                             + "\nhas been changed \n"
                             + "Status From: " + oldStat + " \nTo: " + newStat
                             + "Status From: " + oldStatClass + " \nTo: " + newStatClass;                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                            comment.Subject = "Status and Status Class Changed using direct close";
                        }
                    }
                    procurementRepo.Add(row.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);
                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Invoice, comment.Comment, 0,user.id, "New Comment ", null);
                        }
                    }
                }
                var thisWindow = Window.GetWindow(this);
                thisWindow.Close();

            }
            else
            {
                MessageBox.Show("You are not Allowed to Close SaleInvoice Directly.");
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

                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsProcurementType == true))
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == InterCompany.Id) != null)
                            departments.Add(_dept);
                    }
                    if (saleInvoice != null && saleInvoice.Id > 0 && editsaleInvoice == 1)
                        if (saleInvoice.InterDepartment != null && departments.FirstOrDefault(x => x.Id == saleInvoice.InterDepartment_Id) == null)
                            departments.Add(saleInvoice.InterDepartment);

                    //foreach (Department dep in InterCompany.departments)
                    //    foreach (Department empdep in SYSTEM_STATIC.currentUser.employee.departments)
                    //        if (dep.Id == empdep.Id)
                    //        {
                    //            departments.Add(dep);
                    //        }
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

        private void IsSalesTax_Unchecked(object sender, RoutedEventArgs e)
        {
            var zeroTax = 0;
            txttax.Text = zeroTax.ToString();
        }
        public void LoadTaxes()
        {
            lookupSalesTax.ItemsSource = taxRepo.getAllTaxes();

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
            CalculateAmounts();
        }
        public void CalculateAmounts()
        {
            var selectedTax = lookupSalesTax.SelectedItem as TaxName;
            if (selectedTax != null)
            {
                if (selectedTax.isManual != true)
                {
                    if (selectedTax != null)
                    {
                        saleOrderTax = selectedTax;
                        if (selectedTax != null)
                        {
                            saleOrderTax = selectedTax;
                            txttax.Text = (selectedTax.percentage / 100 * Convert.ToDouble(txtcfr.Text)).ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(txttax.Text) && !string.IsNullOrEmpty(txtcfr.Text))
                    {
                        var total = Convert.ToDouble(txttax.Text) + Convert.ToDouble(txtcfr.Text);
                        txttotalcfr.Text = total.ToString();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(txttax.Text) && !string.IsNullOrEmpty(txtcfr.Text))
                    {
                        var total = Convert.ToDouble(txttax.Text) + Convert.ToDouble(txtcfr.Text);
                        txttotalcfr.Text = total.ToString();
                    }
                }
            }
        }
        private void Txtcfr_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (saleOrder.taxNameId != null)
            {
                saleOrderTax = taxRepo.getTaxtById((int)saleOrder.taxNameId);
                lookupSalesTax.Text = saleOrderTax.Name;
                isSalesTax.IsChecked = true;
                if (saleOrderTax != null)
                {
                    txttax.Text = (saleOrderTax.percentage / 100 * Convert.ToDouble(txtcfr.Text)).ToString();
                }
            }
            else
            {
                lookupSalesTax.Text = null;
                isSalesTax.IsChecked = false;
                txttax.Text = 0.ToString();
            }
        }

        private void Txttax_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txttax.Text != "")
            {
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {

                    CalculateBookerTotal();
                }
                else
                {
                    calculatetotal();
                }
            }
        }

        private void TxtComments_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (tb.Text.Length > 0)
            {
               // tb.Text = Char.ToUpper(tb.Text[0]).ToString();
                var v = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tb.Text);
                tb.Text = v;
            }
        }

   
        private void BtnSaveSubject_Click(object sender, RoutedEventArgs e)
        {
            switch (SubjectWindow.Title)
            {
                case "Sale Invoice Tax [SUBJECT]":
                    if (!String.IsNullOrEmpty(frmAddSubject.txtSubject.Text))
                    {
                        saleInvoiceTaxSubject = frmAddSubject.txtSubject.Text;
                    }
                    break;
                case "Commission Invoice [SUBJECT]":
                    if (!String.IsNullOrEmpty(frmAddSubject.txtSubject.Text))
                    {
                        saleInvoiceCISubject = frmAddSubject.txtSubject.Text;
                    }
                    break;
                case "Delivery-Note [SUBJECT]":
                    if (!String.IsNullOrEmpty(frmAddSubject.txtSubject.Text))
                    {
                        saleInvoiceDnoteSubject = frmAddSubject.txtSubject.Text;
                    }
                    break;
                case "ZAS Commission":
                    if (!String.IsNullOrEmpty(frmAddSubject.txtSubject.Text))
                    {
                        saleInvoiceZasCommission = frmAddSubject.txtSubject.Text;
                    }
                    break;
            }
            SubjectWindow.Close();
        }

   

        private void MbtnSItaxSubject_Click(object sender, RoutedEventArgs e)
        {
            SubjectWindow = new Window();
            frmAddSubject = new ucfrmAddSubject();

            SubjectWindow.Width = 400;
            SubjectWindow.Height = 300;
            SubjectWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SubjectWindow.ResizeMode = ResizeMode.NoResize;
            SubjectWindow.Title = "Sale Invoice Tax [SUBJECT]";
            SubjectWindow.Padding = new Thickness(5, 5, 5, 5);
            if (!string.IsNullOrEmpty(saleInvoiceTaxSubject))
            {
                frmAddSubject.txtSubject.Text = saleInvoiceTaxSubject;
            }
            else
            {

                if (!string.IsNullOrEmpty(saleInvoice.SItaxSubject))
                {
                    frmAddSubject.txtSubject.Text = saleInvoice.SItaxSubject;
                }
            }


            frmAddSubject.btnSave.Click += BtnSaveSubject_Click;
            SubjectWindow.Content = frmAddSubject;
            SubjectWindow.ShowDialog();
        }

        private void MbtnCISubject_Click(object sender, RoutedEventArgs e)
        {
            SubjectWindow = new Window();
            frmAddSubject = new ucfrmAddSubject();

            SubjectWindow.Width = 400;
            SubjectWindow.Height = 300;
            SubjectWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SubjectWindow.ResizeMode = ResizeMode.NoResize;
            SubjectWindow.Title = "Commission Invoice [SUBJECT]";
            SubjectWindow.Padding = new Thickness(5, 5, 5, 5);

            if (!string.IsNullOrEmpty(saleInvoiceCISubject))
            {
                frmAddSubject.txtSubject.Text = saleInvoiceCISubject;
            }
            else
            {
                if (!string.IsNullOrEmpty(saleInvoice.CISubject))
                {
                    frmAddSubject.txtSubject.Text = saleInvoice.CISubject;
                }
            }


            frmAddSubject.btnSave.Click += BtnSaveSubject_Click;
            SubjectWindow.Content = frmAddSubject;
            SubjectWindow.ShowDialog();
        }

        private void MbtnDNoteSubject_Click(object sender, RoutedEventArgs e)
        {
            SubjectWindow = new Window();
            frmAddSubject = new ucfrmAddSubject();

            SubjectWindow.Width = 400;
            SubjectWindow.Height = 300;
            SubjectWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SubjectWindow.ResizeMode = ResizeMode.NoResize;
            SubjectWindow.Title = "Delivery-Note [SUBJECT]";
            SubjectWindow.Padding = new Thickness(5, 5, 5, 5);
            if (!string.IsNullOrEmpty(saleInvoiceDnoteSubject))
            {
                frmAddSubject.txtSubject.Text = saleInvoiceDnoteSubject;
            }
            else
            {
                if (!string.IsNullOrEmpty(saleInvoice.DNSubject))
                {

                    frmAddSubject.txtSubject.Text = saleInvoice.DNSubject;
                }
            }


            frmAddSubject.btnSave.Click += BtnSaveSubject_Click;
            SubjectWindow.Content = frmAddSubject;
            SubjectWindow.ShowDialog();
        }

        private void BtnPreviousComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (saleInvoice != null)
                {
                    if(comments.Count != 0)
                    {
                        if (count != 0)
                        {
                            count = count - 1;

                            if (count != -1)
                            {

                                var comment = grdCommentss.GetCellValue(0, grdCommentss.Columns["Comment"]);
                                if (comment != null)
                                    txtComments.Text = comments[count].Comment;
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
                                //isPrevious = true;

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
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnNextComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (saleInvoice != null)
                {
                    if(comments.Count!= 0)
                    {

                        //count--;
                        if (count < Totalcount)
                        {
                            count = count + 1;
                            //var PreviousCommentCount = count - 1;
                            if (count != -1)
                            {

                                var comment = grdCommentss.GetCellValue(0, grdCommentss.Columns["Comment"]);
                                if (comment != null)
                                    txtComments.Text = comments[count].Comment;
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
                                //isPrevious = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("This is the Latest Tag, Thanks");
                        }
                    }

                }
               
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnOldestComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (saleInvoice != null)
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
                                    txtComments.Text = comments[min].Comment;
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
                                //isPrevious = true;

                            }
                        }
                        else
                        {
                            MessageBox.Show("This is Oldest Comment, Thanks");
                        }


                    }

                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnLatestComment_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (saleInvoice != null)
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
                                    txtComments.Text = comments[max].Comment;
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
                                //isPrevious = true;

                            }
                        }
                        else
                        {
                            MessageBox.Show("This is Latest Comment, Thanks");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }


         
        }


        private void BtnGJournal_Click(object sender, RoutedEventArgs e)
        {

           
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View General Journal") != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                if ((InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz || (InquiryType)cmbSaleInvoiceType.SelectedIndex == InquiryType.DistributionBiz_CustomerCredit)
                {
                    ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(getBookerJournalTransactions());
                    generalJournal.ShowDialog();
                }
                else
                {

                    ZAS_ERP.ChartofAccounts.Windows.GernalJournal generalJournal = new ZAS_ERP.ChartofAccounts.Windows.GernalJournal(getJournalTransactions());
                    generalJournal.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to View General Journal.");
            }
        
        }
        private void MbtnCommission_Click(object sender, RoutedEventArgs e)
        {
            SubjectWindow = new Window();
            frmAddSubject = new ucfrmAddSubject();

            SubjectWindow.Width = 400;
            SubjectWindow.Height = 300;
            SubjectWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SubjectWindow.ResizeMode = ResizeMode.NoResize;
            SubjectWindow.Title = "ZAS Commission";
            SubjectWindow.Padding = new Thickness(5, 5, 5, 5);
            if (!string.IsNullOrEmpty(saleInvoiceZasCommission))
            {
                frmAddSubject.txtSubject.Text = saleInvoiceZasCommission;
            }
            else
            {
                if (!string.IsNullOrEmpty(saleInvoice.Commission))
                {

                    frmAddSubject.txtSubject.Text = saleInvoice.Commission;
                }
            }


            frmAddSubject.btnSave.Click += BtnSaveSubject_Click;
            SubjectWindow.Content = frmAddSubject;
            SubjectWindow.ShowDialog();
        }

        private void CmbxBankTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupBank.SelectedIndex > -1)
            {
                SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                var dept = lookupDepartment.SelectedItem as Department;
                var bank = lookupBank.SelectedItem as Bank;
                var accntList = receiptRepo.GetAllAccountsByBankId(bank.Id).Where(x=> x.isActive == true && x.departments.FirstOrDefault(y=>y.Id == dept.Id) != null  && x.bank.Id == bank.Id && x.company.Id == (lookupCompany.SelectedItem as Company).Id && (x.accountsCategory == ERP_BL.Enums.AccountsCategory.Company || x.accountsCategory == ERP_BL.Enums.AccountsCategory.Personal));
              
                lookupAccount.ItemsSource = accntList;
            }
        }

        private void CmbxBankTo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                lookupCompany.Focus();
                return;
            }

        }

        private void CmbxAccountTo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                lookupCompany.Focus();
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                lookupDepartment.Focus();
                return;
            }
            if (lookupBank.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Bank first!");
                lookupBank.Focus();
                return;
            }
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (saleInvoice.saleInvoiceStatus.isActive == false && saleInvoice.PendingForClosing != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can attach document when Sale Invoice Closed") != null)
                {
                    if (grdAttach1.Visibility == Visibility.Visible)
                        grdAttach1.Visibility = Visibility.Collapsed;
                    else
                    {
                        if (saleInvoice.Id != 0)
                        {
                            cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSIAttachmentCategories();
                        }
                        grdAttach1.Visibility = Visibility.Visible;

                    }
                }
                else
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required " + "Can attach document when Sale Invoice Closed!");
            }
            else
            {
                if (grdAttach1.Visibility == Visibility.Visible)
                    grdAttach1.Visibility = Visibility.Collapsed;
                else
                {
                    if (saleInvoice.Id != 0)
                    {
                        cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveSIAttachmentCategories();
                    }
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
                if (saleInvoice.Id != 0)
                {
                    //treeViewAttachments1.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(saleInvoice.Id, TransactionItemType.Sale_Invoice);

                    List<TreeItem> otherAttachments = new List<TreeItem>();
                    saleOrder = saleInvoice.SaleOrder;
                    List<TreeItem> atachments = SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)saleOrder.Id, TransactionItemType.Sale_Order);
                    if (saleOrder.offer != null)
                    {
                        if (saleOrder.offer_Id != null)
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)saleOrder.offer_Id, TransactionItemType.Offer));
                        if (saleOrder.offer.inquiry_Id != null)
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)saleOrder.offer.inquiry_Id, TransactionItemType.Inquiry));
                    }
                    if (saleOrder.SaleInvoices.Count != 0)
                    {
                        foreach (var invoice in saleOrder.SaleInvoices)
                        {
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)invoice.Id, TransactionItemType.Sale_Invoice));
                            if (invoice.salesReceipts.Count != 0)
                                foreach (var receipt in invoice.salesReceipts)
                                {
                                    otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)receipt.transactionGroupId, TransactionItemType.Sale_Receipt));
                                }
                        }
                    }
                    if (saleOrder.PurchaseOrders.Count != 0)
                    {
                        foreach (var pO in saleOrder.PurchaseOrders)
                        {
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)pO.Id, TransactionItemType.Purchase_Order));
                            if (pO.PurchaseInvoices.Count != 0)
                                foreach (var pI in pO.PurchaseInvoices)
                                {
                                    otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)pI.Id, TransactionItemType.Purchase_Invoice));
                                    if (pI.Payments.Count != 0)
                                        foreach (var payment in pI.Payments)
                                        {
                                            otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
                                        }
                                }
                        }
                    }
                    if (saleOrder.Bills.Count != 0)
                    {
                        foreach (var bill in saleOrder.Bills)
                        {
                            otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)bill.Id, TransactionItemType.Bill));
                            if (bill.Payments.Count != 0)
                                foreach (var payment in bill.Payments)
                                {
                                    otherAttachments.AddRange(SYSTEM_STATIC.GetSIAttachmentsListByCategory((int)payment.transactionGroupId, TransactionItemType.Payments));
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




                    //cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
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
            if (cmbCategory1.SelectedItem != null)
            {
                if (saleInvoiceid != 0)
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
                        destination += "Attachments\\Sale_Invoice\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += saleInvoiceid + "_" + TransactionItemType.Sale_Invoice.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Sale_Invoice);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), saleInvoiceid, TransactionItemType.Sale_Invoice, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, saleInvoice.Id, 5, "Added a New attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(saleInvoiceid, TransactionItemType.Sale_Invoice);
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

        private void Txttax_EditValueChanged_1(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            CalculateAmounts();
        }

        private void BtnCostSheetPunching_Click(object sender, RoutedEventArgs e)
        {
            btnCostSheet.IsEnabled = false;
            try
            {
                var inputfromUser = DXMessageBox.Show("Do you want to Update/View Cost Sheet from this Sales Invoice?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update CostSheet from Sales Invoice") != null)
                    {
                        if (saleOrder.Id != 0 && !string.IsNullOrEmpty(txtAmountSOC.Text) && saleInvoice.Id != 0)
                        {
                            winSelectCostSheetFields winSelectCostSheetFields = new winSelectCostSheetFields(saleOrder, Convert.ToDecimal(txtAmountSOC.Text), saleInvoice.Id, TransactionItemType.Sale_Invoice);
                            winSelectCostSheetFields.ShowDialog();
                        }
                    }
                    else
                        DXMessageBox.Show("You do not have permisssion Update CostSheet from Sales Invoice", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCostSheetPunching_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                ProcurementRepo repo = new ProcurementRepo();
                string systemCost = "";
                List<CostSheetSIField> costSheetSIValues = repo.GetSystemSICosts(saleInvoiceid);
                foreach (CostSheetSIField field in costSheetSIValues)
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

        private void btnBudgetCost_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget") != null)
            {
                if (saleOrder.Budget_Id != null)
                {
                    frmBudgetAdd budget = new frmBudgetAdd((int)saleOrder.Budget_Id);
                    budget.ShowDialog();
                }
                else
                {
                    DXMessageBox.Show("Please attach budget with sale order first", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            {
                DXMessageBox.Show("View Budget", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnBudgetCostPunching_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Punch Budget System Cost") != null)
            {
                if (saleInvoice.BudgetSystemCostFields.Count != 0)
                {
                    winAddBudgetSystemCost systemCost = new winAddBudgetSystemCost((int)saleOrder.Budget_Id, true, Convert.ToDouble(txtAmountSOC.Text), saleOrder);
                    winAddBudgetSystemCost.costSheetFields = saleInvoice.BudgetSystemCostFields;
                    systemCost.ShowDialog();
                    saleInvoice.BudgetSystemCostFields = winAddBudgetSystemCost.costSheetFields;
                }
                else
                {
                    if (saleOrder.Budget_Id != null)
                    {
                        winAddBudgetSystemCost systemCost = new winAddBudgetSystemCost((int)saleOrder.Budget_Id, false, Convert.ToDouble(txtAmountSOC.Text), saleOrder);
                        systemCost.ShowDialog();
                        saleInvoice.BudgetSystemCostFields = winAddBudgetSystemCost.costSheetFields;

                    }
                }
            }
            else
            {
                DXMessageBox.Show("Punch Budget System Cost", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Information);
            }
           
        }
        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (saleInvoice.Id != 0)
            {
                if (cmbTransactionHolder.SelectedItem != null)
                {

                    if (saleInvoice.transactionHolderId != (cmbTransactionHolder.SelectedItem as cmbitem).id)
                    {
                        datHolderDate.EditValue = DateTime.Now;
                    }
                    else
                    {
                        datHolderDate.EditValue = saleInvoice.holderChangeDate;
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
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByDepartmentIdsList(new List<int> { department.Id, InterDepartment.Id }, new List<int> { company.Id, InterCompany.Id }), TransactionItemType.Sale_Invoice);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else if (department != null && department.Id != 0 && company?.Id != 0 && chkInterCompany.IsChecked != true)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
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

                if (frmInputBox.commentAdded == true && saleInvoice.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    
                    //procurementRepo.Add(saleOrder.Id, TransactionItemType.Sale_Order, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (saleInvoice.Id == 0)
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GellAllOrdersTracking();
        }
        public void GellAllOrdersTracking()
        {
            trackingOrder = saleInvoiceRepo.get(saleInvoice.Id);
            if (trackingOrder != null)
            {
                OrderTracking tracking = new OrderTracking();
                grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.Sale_Invoice);
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

        private void btnSTLGenerated_Checked(object sender, RoutedEventArgs e)
        {
            imgSTL.Visibility = Visibility.Visible;
        }
        private void btnSTLGenerated_Unchecked(object sender, RoutedEventArgs e)
        {
            imgSTL.Visibility = Visibility.Collapsed;
        }
        private void btnSTLDiscounted_Checked(object sender, RoutedEventArgs e)
        {
            imgDiscounted.Visibility = Visibility.Visible;

        }
        private void btnSTLDiscounted_Unchecked(object sender, RoutedEventArgs e)
        {
            imgDiscounted.Visibility = Visibility.Collapsed;
        }
        private void cmbDiscountCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
        private void datExcpectedPaymentDate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var date = datExcpectedPaymentDate.DateTime;
            DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var timeSpan = currDate.Subtract(targetDate);
            txtCollectionDaysLeft.Text = timeSpan.Days.ToString();
        }

        private void datExcpectedDiscountDate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var date = datExcpectedDiscountDate.DateTime;
            DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var timeSpan = targetDate.Subtract(currDate);
            txtDiscountDaysLeft.Text = timeSpan.Days.ToString();
        }
        private void btnCreateLA_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") != null)
            {
                btnSave.IsEnabled = false;
                ucFrmLoansAdvances ucFrmLoansAdvances = new ucFrmLoansAdvances(TransactionItemType.Sale_Invoice, saleInvoiceid);
                Window win = new Window();
                win.Content = ucFrmLoansAdvances;
                win.WindowState = WindowState.Maximized;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to create LA!");
            }
        }
        private void cmbSaleInvoiceStatusClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (saleInvoice.Id != 0 && saleInvoice.statusClass_Id != null && cmbSaleInvoiceStatusClass.SelectedIndex>-1)
            {
             checkStatusClass=saleInvoiceRepo.GetStatusClass((cmbSaleInvoiceStatusClass.SelectedItem as cmbitem).id);
            }
        }
        private void btnCustomerCredit_Click(object sender, RoutedEventArgs e)
        {
            if (gridCustomerCredits.Visibility == Visibility.Collapsed)
            {
                gridCustomerCredits.Visibility = Visibility.Visible;
            }
            else
            {
                gridCustomerCredits.Visibility = Visibility.Collapsed;
            }
        }

        private void view_InitNewCustomer(object sender, InitNewRowEventArgs e)
        {
           
        }
        private string calculateSystemRefNo()
        {
            string sysRefNo;
            string lastSystemRefNo;

            lastSystemRefNo = saleInvoiceRepo.getLastSystemReferenceNo();

            if (lastSystemRefNo == null)
            {
                sysRefNo = systemRefIntitials + "1";
            }
            else
            {
                int refNo = Convert.ToInt32(lastSystemRefNo.Remove(0, 4));
                refNo = refNo + 1;
                sysRefNo = systemRefIntitials + refNo.ToString();
            }

            return sysRefNo;
        }

        private void btnCreateSaleReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Customer Credit Receipts") != null)
            {
                var cusCredit = grdCustomerCredits.SelectedItem as CustomerCredit;

                if (cusCredit != null && cusCredit.Id != 0)
                {
                    SaleInvoiceRepo invoiceRepo = new SaleInvoiceRepo();
                    var SI = invoiceRepo.get(cusCredit.SaleInvoiceId.Value);

                    if (SI.saleInvoiceStatus != null && SI.saleInvoiceStatus.isActive == false)
                    {
                        DXMessageBox.Show("You cannot create Sale Receipt of a closed Sale Invoice!", "Closed Sale Invoice", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (SI.isApproved == false)
                    {
                        DXMessageBox.Show("You cannot create Sale Receipt of an UnApproved Sale Invoice!", "UnApproved Sale Invoice", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (SI.isVoid == true)
                    {
                        DXMessageBox.Show("You cannot create Sale Receipt of a voided Sale Invoice!", "Voided Sale Invoice", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (SI.CustomerCredits != null && SI.CustomerCredits.Count > 0)
                    {
                        var amount = SI.salesReceipts.Where(x => x.CustomerCreditSerialNo == cusCredit.SerialNo).Sum(x => x.CollectionAmount);
                        if (SI.CustomerCredits.FirstOrDefault(x => x.Id == cusCredit.Id).creditAmount <= amount)
                        {
                            DXMessageBox.Show("You cannot create Sale Recaipt of a Fully Received Sale Invoice!", "Fully Received", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }


                    Window win = new Window();
                    ucFrmCustomerCreditReceipt enterSaleReceiptObj = new ucFrmCustomerCreditReceipt();
                    enterSaleReceiptObj.SerialNo = cusCredit.SerialNo;
                    enterSaleReceiptObj.saleInvoiceId = cusCredit.SaleInvoiceId.Value;
                    win.Content = enterSaleReceiptObj;
                    win.Title = "Sale Receipt";
                    win.ResizeMode = ResizeMode.CanResize;
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
                else
                {
                    MessageBox.Show("Create or load Sale Invoice First to create Sale Receipt!", "Invoice not Found");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Customer Credit Receipts!");
            }

        }

        private void grdCustomerCredits_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            var cusCredit = grdCustomerCredits.GetRowByListIndex(e.ListSourceRowIndex) as CustomerCredit;

            if(cusCredit != null)
            {
                if (e.Column.FieldName == "collectedCustomerCredit")
                {
                    if (saleInvoice != null && saleInvoice.salesReceipts != null && saleInvoice.salesReceipts.Count > 0)
                    {
                        var collectedAmount = saleInvoice.salesReceipts.Where(x => x.CustomerCreditSerialNo == cusCredit.SerialNo).Sum(x => x.CollectionAmount);
                        e.Value = collectedAmount;
                    }
                    else
                    {
                        e.Value = 0;
                    }
                }
                if (e.Column.FieldName == "balanceCustomerCredit")
                {
                    if (saleInvoice != null && saleInvoice.salesReceipts != null && saleInvoice.salesReceipts.Count > 0)
                    {
                        var collectedAmount = saleInvoice.salesReceipts.Where(x => x.CustomerCreditSerialNo == cusCredit.SerialNo).Sum(x => x.CollectionAmount);
                        e.Value = cusCredit.creditAmount - collectedAmount;
                    }
                    else
                    {
                        e.Value = 0;
                    }
                }
            }
        }

        private void grdCustomerCreditsView_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            var listIndex = grdCustomerCredits.GetListIndexByRowHandle(e.RowHandle);
            var row = grdCustomerCredits.GetRowByListIndex(listIndex) as CustomerCredit;

            if(grdCustomerCredits.VisibleRowCount == 0)
            {
                row.SerialNo = 1;
            }
            else
            {
                var customerCredits = grdCustomerCredits.ItemsSource as List<CustomerCredit>;

                var maxValue = customerCredits.Max(x=>x.SerialNo);

                row.SerialNo = maxValue + 1;
                //row.SerialNo = maxValue + 1;
            }
            
        }
        private void btnInsuranceRequired_Checked(object sender, RoutedEventArgs e)
        {
                imgInsuarance.Visibility = Visibility.Visible;
                imgInsuarance.Background = Brushes.Gray;
                btnInsuranceApplied.IsEnabled = true;
                btnInsuranceNotApplicable.IsEnabled = true;
        }
        private void btnInsuranceRequired_Unchecked(object sender, RoutedEventArgs e)
        {
            imgInsuarance.Visibility = Visibility.Collapsed;
        }
        private void btnInsuranceApplied_Checked(object sender, RoutedEventArgs e)
        {
            imgInsuarance.Content = "Insurance Applied";
            imgInsuarance.Background=Brushes.Green;
            btnInsuranceNotApplicable.IsChecked = false;
            if (insuarenceAppliedEmployee.EmpId!=SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck==true)
            {
                insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
            }
            firstInsuranceCheck = true;
        }

        private void btnInsuranceApplied_Unchecked(object sender, RoutedEventArgs e)
        {
            if(btnInsuranceNotApplicable.IsChecked!=true)
            {
                imgInsuarance.Content = "Insurance Required";
                imgInsuarance.Background = Brushes.Gray;
                btnInsuranceApplied.IsChecked = false;

                if (insuarenceAppliedEmployee.EmpId != SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck == true)
                {
                    insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
                }
                firstInsuranceCheck = true;
            }
        }
        private void btnInsuranceNotApplicable_Checked(object sender, RoutedEventArgs e)
        {
            imgInsuarance.Visibility = Visibility.Visible;
            imgInsuarance.Content = "Insurance N/A";
            imgInsuarance.Background = Brushes.Orange;
            btnInsuranceApplied.IsChecked = false;
            if (insuarenceAppliedEmployee.EmpId != SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck == true)
            {
                insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
            }
            firstInsuranceCheck = true;
        }
        private void btnInsuranceNotApplicable_Unchecked(object sender, RoutedEventArgs e)
        {
            if (btnInsuranceNotApplicable.IsChecked != true && btnInsuranceApplied.IsChecked != true)
            {
                imgInsuarance.Content = "Insurance Required";
                imgInsuarance.Background = Brushes.Gray;
                if (insuarenceAppliedEmployee.EmpId != SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck == true)
                {
                    insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
                }
                firstInsuranceCheck = true;
            }
            else
            if (btnInsuranceNotApplicable.IsChecked != true && btnInsuranceApplied.IsChecked == true)
            {
                imgInsuarance.Content = "Insurance Applied";
                imgInsuarance.Background = Brushes.Green;
                if (insuarenceAppliedEmployee.EmpId != SYSTEM_STATIC.currentUser.employeeId && firstInsuranceCheck == true)
                {
                    insuarenceAppliedEmployee = SYSTEM_STATIC.currentUser.employee;
                }
                firstInsuranceCheck = true;
            }
        }

        private void cmbAppliedBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void datPaidDate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            var date = datPaymentOnDate.DateTime;
            DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var timeSpan = currDate.Subtract(targetDate);
            txtPaidDaysLeft.Text = timeSpan.Days.ToString();
            txtDeliveryDaysLeft.Text = (Convert.ToDouble(txtPaidDaysLeft.Text)-Convert.ToDouble(txtDeliveryDays.Text)) .ToString();
        }

        private void btnRedInvoice_Checked(object sender, RoutedEventArgs e)
        {
            imgRedInvoice.Visibility = Visibility.Visible;
        }

        private void btnRedInvoice_Unchecked(object sender, RoutedEventArgs e)
        {
            imgRedInvoice.Visibility = Visibility.Collapsed;
        }
    }
    public class PendingInvoice
    {
        public int Id { get; set; }
        public string refNO { get; set; }
        public string currencyName { get; set; }
        public double invoiceAmount { get; set; }
        public int amountOC { get; set; }
        public int receiptAmount { get; set; }
        public int balanceAmount { get; set; }
        public int ageningDays { get; set; }

    }

}
