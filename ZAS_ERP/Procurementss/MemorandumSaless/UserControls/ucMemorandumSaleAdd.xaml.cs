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

namespace ZAS_ERP.Procurementss.MemorandumSaless
{
    

    /// <summary>
    /// Interaction logic for ucInquiryAdd.xaml
    /// </summary>
    public partial class ucMemorandumSaleAdd : UserControl
    {
        public static int editmemorandumSale;
        public static int memorandumSaleid;
        public static int offerid;
        public static int saleOrderid;
        public int OrderId;

        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        //CurrentUserSetting streamConverter;
        EmployeeRepo cont1 = new EmployeeRepo();
        CompanyRepo cont = new CompanyRepo();
        VendorRepo vendorRepo = new VendorRepo();
        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
        Offer offer = new Offer();
        OfferRepo offerRepo = new OfferRepo();
        SaleOrder saleOrder = new SaleOrder();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        MemorandumSaleRepo memorandumSaleRepo = new MemorandumSaleRepo();
        MemorandumSaleStatus oldStatus = new MemorandumSaleStatus();
        MemorandumSale memorandumSale = new MemorandumSale();
        Vendor vendor = new Vendor();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        CustomerCompany customer = new CustomerCompany();
        User user = new User();
        NotificationsRepo notificationsRepo = new NotificationsRepo();

        List<Product> inqueryItems = new List<Product>();
        public bool isloading = false;
        public virtual List<ProcurementProduct> procurementProducts { get; set; }
        public virtual List<Product> products { get; set; }
        //MemoryStream memoryStream;
        List<ProcurementProduct> memorandumSaleItems = new List<ProcurementProduct>();
        ProductRepo productrepo = new ProductRepo();
        Product product = new Product();
        //List<Product> products = new List<Product>();
        Bid bidbond = new Bid();
        Principal principal = new Principal();
        Currency currency = new Currency();
        CurrencyRepo currencyRepo = new CurrencyRepo();
        PaymentTerm PaymentTerm = new PaymentTerm();
        PaymentTermRepo TermRepo = new PaymentTermRepo();
        PrincipalRepo principalRepo = new PrincipalRepo();
        UsersRepo UsersRepo = new UsersRepo();
        bool addinfo = true;
        List<ViewInfo> views = new List<ViewInfo>();
        public MemorandumSaleStatus checkStatus = new MemorandumSaleStatus();


        public ucMemorandumSaleAdd()
        {
            procurementProducts = new List<ProcurementProduct>();
            products = new List<Product>();
            InitializeComponent();
            symbol = "";
            //cmbcaption1.SelectedIndex = 0;
            //cmbcaption2.SelectedIndex = 1;
            //    String gridlayout = (string)Properties.Settings.Default["PoItemgridLO"];
            //    if (gridlayout != "" )
            //    {
            //        byte[] byteArray = Encoding.ASCII.GetBytes(gridlayout);
            //        MemoryStream stream = new MemoryStream(byteArray);
            //        //memoryStream = streamConverter.convertToStream(gridlayout);
            //    grdItems.RestoreLayoutFromStream(stream);
            //}
        }


        private void winMemorandumSaleadd_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderId = memorandumSaleid;
                txttax.Text = "0";
                grdItems.ItemsSource = procurementProducts;
                //products = offerRepo.getAllProducts();
                products = SYSTEM_STATIC.GetItemsForCurrentUser();
                lookupProductsinGrid.ItemsSource = products;
                loadcompanies();
               
              
                loadIncoterms();
                
                loadMemorandumSaleStatus();
                
                loadPrincipals();
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of MemorandumSale") != null)
                {
                    datpoCreationdate.IsEnabled = true;
                }
                else
                {
                    datpoCreationdate.IsEnabled = false;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with MemorandumSale") != null)
                {
                    btnAttachNew.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAttachNew.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with MemorandumSale") != null)
                {
                    btnAttachmentList.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAttachmentList.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Memorandum Sale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Memorandum Sale") != null)
                {
                    btnSetVoid.Visibility = Visibility.Visible;
                }
                if (editmemorandumSale == 1 && memorandumSaleid != 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Memorandum Sale") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Memorandum Sale") == null)
                    {
                        isloading = true;
                        memorandumSale = memorandumSaleRepo.get(memorandumSaleid);
                        loadonMemorandumSaledata();
                        views = UsersRepo.getViwerInfo(memorandumSale.Id, 4);
                        grdUsers.ItemsSource = views;
                        loadcomments();
                        btnSave.IsEnabled = false;
                        if (memorandumSale.isApproved == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Memorandum Sale") != null)
                        {
                            btnSave.IsEnabled = true;
                        }

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Memorandum Sale") == null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Memorandum Sale") != null)
                        {
                        isloading = true;
                        memorandumSale = memorandumSaleRepo.get(memorandumSaleid);
                        loadonMemorandumSaledata();
                        views = UsersRepo.getViwerInfo(memorandumSale.Id, 4);
                        grdUsers.ItemsSource = views;
                        loadcomments();
                        btnSave.IsEnabled = true;

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Memorandum Sale") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Memorandum Sale") != null)
                    {
                        isloading = true;
                        memorandumSale = memorandumSaleRepo.get(memorandumSaleid);
                        
                        loadonMemorandumSaledata();
                        views = UsersRepo.getViwerInfo(memorandumSale.Id, 4);
                        grdUsers.ItemsSource = views;
                        loadcomments();
                        btnSave.IsEnabled = true;

                    }

                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Memorandum Sale!");
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;

                    }

                }
                else if (editmemorandumSale == 0 && (offerid != 0||saleOrderid!=0))
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorandum Sale") != null)
                    {
                        isloading = true;
                        if (offerid != 0)
                        {
                            offer = offerRepo.get(offerid);
                            loadonofferdata();
                        }
                        else if(saleOrderid!=0)
                        {
                            saleOrder = saleOrderRepo.get(saleOrderid);
                            loadonSaleOrderdata();
                        }
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Memorandum Sale!");
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;

                    }


                }
                else
                {
                    datpoCreationdate.EditValue = System.DateTime.Now;

                    cmbcaption1.SelectedIndex = 0;
                    cmbcaption2.SelectedIndex = 1;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void MemorandumSale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void MemorandumSale") != null)
                {
                    btnSetVoid.Visibility = Visibility.Visible;
                }
                calculatetotal();
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdItems);
                isloading = false;
                LoadCreator();


            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                MessageBox.Show(ex.ToString(), "Data Loading Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //lookupCustomer.Focus();

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
            if (editmemorandumSale == 0)
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
            else if (memorandumSale.user_Id != null)
                txtCreator.Text = memorandumSale.user.employee.person.FName + " " + memorandumSale.user.employee.person.LName;
        }

        public void loadcompanies()
        {
            //try
            //{
            //if (MainWindow.currentUserid == 0)
            //{
            //    CompanyRepo cont = new CompanyRepo();

            //    this.lookupCompany.ItemsSource = cont.GetCompanies();
            //    return;

            //}
            //var usernew = cont1.getuser(MainWindow.currentUserid);
            //empUser = cont1.GetEmployee(usernew.employeeId);
            //lookupCompany.ItemsSource = empUser.Companies;


            lookupCompany.ItemsSource = SYSTEM_STATIC.LoadCurrentUserCompanies();

            //List<ERP_BL.Databases.Company> companylist = new List<ERP_BL.Databases.Company>();
            //    companylist = cont.GetCompanies();
            //    List<ERP_BL.Databases.Company> companies = new List<ERP_BL.Databases.Company>();
            //    foreach (ERP_BL.Databases.Company comp in companylist)
            //        if (comp.compnayType == ERP_BL.Enums.CompnayTypes.Company)
            //            companies.Add(comp);
            //    this.lookupCompany.ItemsSource = companies;
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
        //public void loadvendors()
        //{


        //    // List<Vendor> vendors = new List<Vendor>();
        //    // vendors = vendorRepo.getAll();
        //    //// List<CustomerCompany> customerlist = new List<CustomerCompany>();

        //    // //foreach (Vendor vendor in vendors)
        //    // //{
        //    // //    customerlist.Add(vendor);
        //    // //}

        //    // lookupVendor.ItemsSource = vendors;
        //    lookupVendor.ItemsSource = department.Vendors;
        //}
        public void loadPrincipals()
        {


            //List<Principal> principals = new List<Principal>();
            //principals = principalRepo.getAll();
            //// List<CustomerCompany> customerlist = new List<CustomerCompany>();

            ////foreach (Vendor vendor in vendors)
            ////{
            ////    customerlist.Add(vendor);
            ////}
            ///

            PrincipalRepo repo = new PrincipalRepo();
            
            lookupPrincipal.ItemsSource = repo.getAllByDept(department.Id);
        }

        public void loadcustomers()
        {
            if (company != null && department != null)
                if (company.Id != 0)
                {
                    if (department.Id != 0)
                    {
                    CustomerCompRepo customerCompRepo = new CustomerCompRepo();
                    var customers = customerCompRepo.getCustomersForCompanyAndDepartment(company.Id, department.Id);
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




        public void loaddepartments()
        {
            //if (MainWindow.currentUserid == 0)
            //{
            //    DepartmentRepo departmentRepo = new DepartmentRepo();
            //    this.lookupDepartment.ItemsSource = departmentRepo.GetDepartments();
            //    return;
            //}
            //if (company != null)
            //    if (company.departments != null)
            //    {
            //        List<Department> departments = new List<Department>();
            //        foreach (Department dep in company.departments)
            //            foreach (Department empdep in empUser.departments)
            //                if (dep.Id == empdep.Id)
            //                {
            //                    departments.Add(dep);
            //                }
            //        lookupDepartment.ItemsSource = departments;

            //        if (departments.Count == 0)
            //        {
            //            MessageBox.Show("This company dosen't contain any department mapped with the current User");
            //        }
            //    }


            lookupDepartment.ItemsSource=SYSTEM_STATIC.LoadCurrentUserDepartments();

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

        public void loadonofferdata()
        {
            
            datmemorandumSaledate.DateTime = System.DateTime.Now;
            //datduedate.DateTime = offer.DeliveryDueDate;
            //datclosedate.DateTime = offer.lastSubmissionDate;
            
            txtmemorandumSaleref.Text = offer.SalesReferenceNo;
           
            //txtBudgetMargin.Text = offer.margin.ToString();
            txtOwnDescription.Text = offer.OwnDescription;
            //txtOurRefNo.Text = offer.offerReferenceNo;
            //txtDeliveryTime.Text = offer.deliveryTime;
            
            txttotalfob.Text = offer.totalFOBValue.ToString();
            txttotalcfr.Text = offer.totalCFRValue.ToString();
                    datpoCreationdate.EditValue = System.DateTime.Now;



            if (offer.isPercentTax == true)
                txttax.Text = offer.salesTax.ToString() + "%";
            else
                txttax.Text = offer.salesTax.ToString();
            // Select IncoTerm
            
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
                //lookupDepartment.SelectedItem = lookupDepartment.GetItemByKeyValue(offer.department);

                department = offer.department;
                lookupCustomer.ItemsSource = department.customers;
                //loademployees();

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

            
            if (offer.principal_Id != null)
                if (offer.principal_Id != 0 || offer.principal != null)
                {
                    lookupPrincipal.Text = offer.principal.company.CompanyName;
                    //lookupPrincipal.SelectedItem = lookupPrincipal.GetItemByKeyValue(offer.principal);

                    principal = offer.principal;
                }
                else
                {
                    lookupCustomer.Text = "Select Customer";

                }
            
            //int a = 0, b = 0;
            // load Products in Inquiry to grid
            List<datagriditem> datagriditems = new List<datagriditem>();
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
                        product_Id = procurementProduct.inquiryProduct.Id,
                        unitPrice = procurementProduct.unitPrice,

                        value1 = procurementProduct.value1,
                        value2 = procurementProduct.value2,
                        caption1 = cmbcaption1.Text.Trim(),
                        caption2 = cmbcaption2.Text.Trim()


                    });
                }
                //dGitems.ItemsSource = datagriditems;
                grdItems.ItemsSource = procurementProducts;
                

            }
            else
            {
                grdItems.ItemsSource = procurementProducts;
            }

            // selected currency of company
            
            calculatetotal();
            //cmbcaption1.SelectedIndex = a;
            //cmbcaption2.SelectedIndex = b;
        }
        public void loadonMemorandumSaledata()
        {


            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(memorandumSale.Id, TransactionItemType.Memorandum_Sale);
            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
            //cmbMemorandumSaleType.Text = memorandumSale.memorandumSaletype.ToString();

            //PoType = memorandumSale.memorandumSaletype;
            datmemorandumSaledate.DateTime = (DateTime)((memorandumSale.memorandumSalesDate != null) ? memorandumSale.memorandumSalesDate : datmemorandumSaledate.DateTime);
            datpoCreationdate.DateTime = (DateTime)((memorandumSale.CreationDate != null) ? memorandumSale.CreationDate : datpoCreationdate.DateTime);
            datExcpectedClosingDate.DateTime = (DateTime)((memorandumSale.ExpectedClosingDate != null) ? memorandumSale.ExpectedClosingDate: datExcpectedClosingDate.DateTime);
            datCustomerRefDate.DateTime = (DateTime)((memorandumSale.CustomerReferenceDate!= null) ? memorandumSale.CustomerReferenceDate: datCustomerRefDate.DateTime);
            datPrinicipalReferencedate.DateTime = (DateTime)((memorandumSale.PrincipleReferenceDate != null) ? memorandumSale.PrincipleReferenceDate : datPrinicipalReferencedate.DateTime);

            //txtIncoTerm.Text = memorandumSale.incoTerm;
            //txtPaymentTerm.Text = memorandumSale.paymentTerm;
            //datduedate.DateTime = memorandumSale.DeliveryDueDate;
            //datclosedate.DateTime = memorandumSale.lastSubmissionDate;
            txtmemorandumSaleref.Text = memorandumSale.referenceNo;
            txtCustomerNumber.Text = memorandumSale.CustomerReferenceNo;
            txtPrinicipalref.Text = memorandumSale.PrincipleReferenceNo;
            

            txttotalfob.Text = memorandumSale.totalFOBValue.ToString();
            txttotalcfr.Text = memorandumSale.totalCFRValue.ToString();
            
            txtQuantity.Text = memorandumSale.TotalQuantity.ToString();
            txtWeight.Text = memorandumSale.TotalWeight.ToString();

            txtOwnDescription.Text = memorandumSale.OwnDescription;
            
           
            txtComments.Text = memorandumSale.comments;
            
            lblStage.Text = (memorandumSale.stage != null) ? memorandumSale.stage : "";


            if (memorandumSale.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            //else if (saleInvoice.isReApproved == false)
            //{
            //    //lblStage.Text = "Under Re-Approval";
            //}
            else if (memorandumSale.isApproved == true && memorandumSale.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (memorandumSale.isApproved == true && memorandumSale.memorandumSaleStatus.isActive == false && memorandumSale.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (memorandumSale.isApproved == true && memorandumSale.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (memorandumSale.isApproved == true)
            {
                //lblStage.Text = "Approved";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (memorandumSale.isApproved == false)
            {
                //lblStage.Text = "Under Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (memorandumSale.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }


            if (memorandumSale.isPercentTax == true)
                txttax.Text = memorandumSale.salesTax.ToString() + "%";
            else
                txttax.Text = memorandumSale.salesTax.ToString();
            //inco term for po
            
            //select Captions for Item Value 1
            if (memorandumSale.TitleValue1Id != 0 || memorandumSale.TitleValue1 != null)
            {
                foreach (cmbitem cmbitem in cmbcaption1.Items)
                {
                    if (cmbitem.id == memorandumSale.TitleValue1Id)
                    {
                        cmbcaption1.SelectedItem = cmbitem;
                        break;
                    }
                }
            }
            //select Captions for Item Value 2
            if (memorandumSale.TitleValue2Id != 0 || memorandumSale.TitleValue2 != null)
            {
                foreach (cmbitem cmbitem in cmbcaption2.Items)
                {
                    if (cmbitem.id == memorandumSale.TitleValue2Id)
                    {
                        cmbcaption2.SelectedItem = cmbitem;
                        break;
                    }
                }
            }
            
            // Select Company
            if (memorandumSale.company_Id != null || memorandumSale.company != null)
            {
                company = memorandumSale.company;
                lookupCompany.Text = memorandumSale.company.CompanyName;
                //lookupCompany.SelectedItem = lookupCompany.GetItemByKeyValue(memorandumSale.company);



                //loaddepartments();
            }
            else
            {
                lookupCompany.Text = "Select Company";

            }
            // Select Department
            if (memorandumSale.dept_Id != 0 || memorandumSale.department != null)
            {
                lookupDepartment.Text = memorandumSale.department.DeptName;
                //lookupDepartment.SelectedItem = lookupDepartment.GetItemByKeyValue(memorandumSale.department);
                department = memorandumSale.department;

                lookupCustomer.ItemsSource = department.customers;
                //loademployees();

            }
            else
            {
                lookupDepartment.Text = "Select Department";
            }



            //Select Customer
            if (memorandumSale.customerCompany.Id != 0 || memorandumSale.customerCompany != null)
            {
                lookupCustomer.Text = memorandumSale.customerCompany.company.CompanyName;
                lookupCustomer.SelectedItem = lookupCustomer.GetItemByKeyValue(memorandumSale.customerCompany);

                customer = memorandumSale.customerCompany;
            }
            else
            {
                lookupCustomer.Text = "Select Customer";

            }
            
            //if (memorandumSale.vendor_Id != 0 || memorandumSale.vendor != null)
            //{
            //    lookupVendor.Text = memorandumSale.vendor.company.CompanyName;
            //    vendor = memorandumSale.vendor;
            //}
            //else
            //{
            //    lookupVendor.Text = "Select Vendor";

            //}
            //Select Principal
            if (memorandumSale.principal_Id != 0 || memorandumSale.principal != null)
            {
                lookupPrincipal.Text = memorandumSale.principal.company.CompanyName;
                principal = memorandumSale.principal;
            }
            else
            {
                lookupPrincipal.Text = "Select Vendor";

            }
            // Select Employee 
            
            //datBidOpendate.DateTime = memorandumSale.bidOpenDate;
            // Delete Inquiry Type Same for MemorandumSale Type

            {

            }
            // Select MemorandumSale Status 
            
            foreach (cmbitem cmbitem in cmbMemorandumSaleStatus.Items)
            {
                if (cmbitem.name == memorandumSale.memorandumSaleStatus.Status)
                {
                    cmbMemorandumSaleStatus.SelectedItem = cmbitem;
                    checkStatus = memorandumSale.memorandumSaleStatus;
                    break;
                }
            }
            checkStatus = memorandumSale.memorandumSaleStatus;
            
            //int a = -1, b = -1;
            // load Products in Inquiry to grid
            List<datagriditem> datagriditems = new List<datagriditem>();


            if (memorandumSale.products != null)
            {
                List<ProcurementProduct> procItems = new List<ProcurementProduct>();
                //grdItems.ItemsSource = offer.products;
                foreach (var procurementProduct in memorandumSale.products)
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
                            Weight =procurementProduct.inquiryProduct.Weight,
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
                        caption2 = cmbcaption2.Text.Trim()


                    });


                }
                //dGitems.ItemsSource = datagriditems;
                grdItems.ItemsSource = procurementProducts;
                int x = 0;
                foreach (var pro in grdItems.ItemsSource as List<ProcurementProduct>)
                {
                    grdItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                    x++;

                }
                lookupProductsinGrid.DisplayMember = "code";

            }
            else
            {
                grdItems.ItemsSource = procurementProducts;
            }
            //cmbcaption1.SelectedIndex = a;
            //cmbcaption2.SelectedIndex = b;

            // selected currency of customer company
           
            calculatetotal();
            if (memorandumSale.isApproved != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Memorandum Sale") != null)
                {
                    grdmemorandumSaledata.IsEnabled = true;
                }
                else
                    grdmemorandumSaledata.IsEnabled = false;
            }
            if (memorandumSale.memorandumSaleStatus.isActive == false && MainWindow.currentUserid != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed MemorandumSale") == null)
            {
                grdmemorandumSaledata.IsEnabled = false;
                btnAttachment.IsEnabled = false;

                labeltopStatus.Visibility = Visibility.Visible;
                labeltopStatus.Text = memorandumSale.memorandumSaleStatus.Status;
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(memorandumSale.memorandumSaleStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                labeltopStatus.Foreground = new SolidColorBrush(newColor);
                var rotateAnimation = new DoubleAnimation(-30, -32, TimeSpan.FromSeconds(60));
                var rt = (RotateTransform)labeltopStatus.RenderTransform;
                rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);

            }
        }

        public void loadonSaleOrderdata()
        {

            cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
            datpoCreationdate.DateTime = System.DateTime.Now;
            txtmemorandumSaleref.Text = saleOrder.referenceNo;
            txttotalfob.Text = saleOrder.totalFOBValue.ToString();
            txttotalcfr.Text = saleOrder.totalCFRValue.ToString();
            txtQuantity.Text = saleOrder.TotalQuantity.ToString();
            txtWeight.Text = saleOrder.TotalWeight.ToString();
            txtOwnDescription.Text = saleOrder.OwnDescription;
            if (saleOrder.isPercentTax == true)
                txttax.Text = saleOrder.salesTax.ToString() + "%";
            else
                txttax.Text = saleOrder.salesTax.ToString();
            //select Captions for Item Value 1
            if (saleOrder.TitleValue1Id != 0 || saleOrder.TitleValue1 != null)
            {
                foreach (cmbitem cmbitem in cmbcaption1.Items)
                {
                    if (cmbitem.id == saleOrder.TitleValue1Id)
                    {
                        cmbcaption1.SelectedItem = cmbitem;
                        break;
                    }
                }
            }
            //select Captions for Item Value 2
            if (saleOrder.TitleValue2Id != 0 || saleOrder.TitleValue2 != null)
            {
                foreach (cmbitem cmbitem in cmbcaption2.Items)
                {
                    if (cmbitem.id == saleOrder.TitleValue2Id)
                    {
                        cmbcaption2.SelectedItem = cmbitem;
                        break;
                    }
                }
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
                lookupCustomer.ItemsSource = department.customers;
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
            // load Products into grid
            List<datagriditem> datagriditems = new List<datagriditem>();
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

                        value1 = procurementProduct.value1,
                        value2 = procurementProduct.value2,
                        caption1 = cmbcaption1.Text.Trim(),
                        caption2 = cmbcaption2.Text.Trim()
                    });
                }
                grdItems.ItemsSource = procurementProducts;
                int x = 0;
                foreach (var pro in grdItems.ItemsSource as List<ProcurementProduct>)
                {
                    grdItems.SetCellValue(x, "inquiryProduct.product", pro.inquiryProduct.product);
                    x++;

                }
                lookupProductsinGrid.DisplayMember = "code";

            }
            else
            {
                grdItems.ItemsSource = procurementProducts;
            }
            calculatetotal();
        }


        public List<ProcurementProduct> getProductsdata()
        {
            List<ProcurementProduct> procurementProducts = new List<ProcurementProduct>();
            List<ProcurementProduct> memorandumSaleItems = new List<ProcurementProduct>();
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
                            //product = inquiryProduct;
                            //product.product_Id = inquiryProduct.product.Id;
                            //product.UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                            //offerItems.Add(procurementProduct);
                            memorandumSaleItems.Add(new ProcurementProduct()
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

                            memorandumSaleItems.Add(new ProcurementProduct()
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
                            memorandumSaleItems.Add(new ProcurementProduct()
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
                            memorandumSaleItems.Add(new ProcurementProduct()
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

                                value1 = procurementProduct.value1,
                                value2 = procurementProduct.value2,
                                caption1 = cmbcaption1.Text.Trim(),
                                caption2 = cmbcaption2.Text.Trim()


                            });
                            //offerItems.Add(new ProcurementProduct()
                            //{
                            //    Id = procurementProduct.Id,

                            //    //inquiryProduct = new InquiryProduct()
                            //    //{
                            //    //    Id = procurementProduct.inquiryProduct.Id,
                            //    //    ownDiscription = procurementProduct.inquiryProduct.ownDiscription, /*product_Id = inquiryProduct.product.Id,*/
                            //    //    UOM = procurementProduct.inquiryProduct.product.unitOfMeasure.unitOfMeasure,
                            //    //    quantity = procurementProduct.inquiryProduct.quantity,

                            //    //    product= procurementProduct.inquiryProduct.product,
                            //    //    //product = new Product()
                            //    //    //{
                            //    //    //    Id = procurementProduct.inquiryProduct.product.Id,
                            //    //    //    categoryId = procurementProduct.inquiryProduct.product.categoryId,
                            //    //    //    item = procurementProduct.inquiryProduct.product.item,
                            //    //    //    code = procurementProduct.inquiryProduct.product.code,
                            //    //    //    itemDescription = procurementProduct.inquiryProduct.product.itemDescription,
                            //    //    //    ownDescription = procurementProduct.inquiryProduct.product.ownDescription,
                            //    //    //    nature_Id = procurementProduct.inquiryProduct.product.nature_Id,
                            //    //    //    unitOfMeasureId = procurementProduct.inquiryProduct.product.unitOfMeasureId,
                            //    //    //    //unitOfMeasure = procurementProduct.inquiryProduct.product.unitOfMeasure,
                            //    //    //    //nature = procurementProduct.inquiryProduct.product.nature,
                            //    //    //    //category = procurementProduct.inquiryProduct.product.category,
                            //    //    //    isActive = procurementProduct.inquiryProduct.product.isActive
                            //    //    //},
                            //    //    product_Id = procurementProduct.inquiryProduct.product.Id


                            //    //},
                            //    product_Id = procurementProduct.inquiryProduct.Id,
                            //    value1 = procurementProduct.value1,
                            //    value2 = procurementProduct.value2,
                            //    caption1 = cmbcaption1.Text.Trim(),
                            //    caption2 = cmbcaption2.Text.Trim()


                            //});
                        }
                    }
                }
            }


            return memorandumSaleItems;
        }
        private void btnMemorandumSaleSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                NotificationsRepo notificationsRepo = new NotificationsRepo();
                //if (MainWindow.currentUserid == 0)
                //{
                //    MessageBox.Show("Please Create another Account to Create PO, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    lookupDepartment.Focus();
                //    return;
                //}
                //else

                if (lookupCustomer.SelectedIndex == -1 && customer.Id == 0)
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
                
                                else if (cmbMemorandumSaleStatus.SelectedIndex == -1&&memorandumSale.PendingForClosing!=true)
                {
                    MessageBox.Show("Please Select Current Status of Memorandum Sale to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbMemorandumSaleStatus.Focus();
                    return;
                }
                
                                else if (Convert.ToDouble(txttotalcfr.Text) == 0 || 0 == Convert.ToDouble(txttotalfob.Text))
                {
                    MessageBox.Show("Please Enter Amount for the Items", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    grdItems.Focus();
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
                    cmbcaption1.Focus();
                    return;
                }

                memorandumSale.products = getProductsdata();
                if (memorandumSale.products.Count == 0)
                {
                    MessageBox.Show("Please Select items against which you want to create a Memorandum Sale", "Required Items", MessageBoxButton.OK, MessageBoxImage.Stop);
                    //lookupDepartment.Focus();
                    return;
                }
                //MemorandumSale order; // = new MemorandumSale();
                //order = memorandumSaleRepo.get(txtSalesref.Text.Trim());
                //if (order == null)
                //    order = new MemorandumSale();

                //if (order != null && order.Id != memorandumSale.Id)
                //{
                //    string message = "Memorandum Sale with Sales reference # (" + order.referenceNo + ") already exists!";
                //    if (order.isApproved == false)
                //    {
                //        message += " Which is pending for Approval. You can't to add duplicate data?";
                //    }
                //    else if (order.PendingForClosing == true)
                //    {
                //        message += " Which is pending for Closing and needs approval to be closed. You can't add duplicate data?";

                //    }
                //    else
                //    {
                //        message += " You can't add duplicate data?";
                //    }
                //    if (DXMessageBox.Show(message, "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                //    {
                //        return;
                //    }
                //    else
                //    {
                //        return;
                //    }
                //}
                //MemorandumSale inquiry = new Inquiry();
                DateTime? dateTime = null;
                ////inquiry.alertDate = datalertDate.DateTime;
                //memorandumSale.alertDate = datduedate.DateTime;
                memorandumSale.memorandumSalesDate = (datmemorandumSaledate.Text == "") ? dateTime : datmemorandumSaledate.DateTime;
                memorandumSale.CreationDate = (datpoCreationdate.Text == "") ? dateTime : datpoCreationdate.DateTime;
                memorandumSale.CustomerReferenceDate = (datCustomerRefDate.Text == "") ? dateTime : datCustomerRefDate.DateTime;
                memorandumSale.PrincipleReferenceDate = (datPrinicipalReferencedate.Text == "") ? dateTime : datPrinicipalReferencedate.DateTime;

                memorandumSale.ExpectedClosingDate = (datExcpectedClosingDate.Text == "") ? dateTime : datExcpectedClosingDate.DateTime;
                //memorandumSale.incoterm_Id = txtIncoTerm.Text.Trim();

                memorandumSale.TitleValue1Id = (cmbcaption1.SelectedItem as cmbitem).id;
                memorandumSale.TitleValue2Id = (cmbcaption2.SelectedItem as cmbitem).id;
                memorandumSale.TotalQuantity =Convert.ToDecimal( txtQuantity.Text.Trim());
                memorandumSale.TotalWeight = Convert.ToDecimal(txtWeight.Text.Trim());
                
                
                memorandumSale.OwnDescription = txtOwnDescription.Text;
                memorandumSale.CustomerReferenceNo = txtCustomerNumber.Text;
                memorandumSale.PrincipleReferenceNo = txtPrinicipalref.Text;
                memorandumSale.comments = txtComments.Text.Trim();
                
                memorandumSale.referenceNo = txtmemorandumSaleref.Text.Trim();
                
                
                //memorandumSale.offerReferenceNo = txtOurRefNo.Text.Trim();
                //memorandumSale.deliveryTime = txtDeliveryTime.Text.Trim();

                
                //memorandumSale.bidOpenDate = datBidOpendate.DateTime;
                memorandumSale.totalCFRValue = Convert.ToDouble(txttotalcfr.Text);
                memorandumSale.totalFOBValue = Convert.ToDouble(txttotalfob.Text);
                
                string str = txttax.Text.Trim();


                if (str.IndexOf("%") != -1)
                {
                    memorandumSale.isPercentTax = true;
                    memorandumSale.salesTax = Convert.ToDouble(str.Substring(0, str.IndexOf("%")));
                }
                else
                {
                    memorandumSale.salesTax = Convert.ToDouble(str);
                    memorandumSale.isPercentTax = false;
                }
                if ((cmbMemorandumSaleStatus.SelectedItem as cmbitem) != null)
                {

                    MemorandumSaleStatus status = memorandumSaleRepo.getstatus((cmbMemorandumSaleStatus.SelectedItem as cmbitem).id);
                    memorandumSale.memorandumSaleStatus = status;
                }
                


                // Selected Principal 
                if (principal != null)
                {

                    memorandumSale.principal_Id = principal.Id;
                }
                
                // selected Department
                if (department != null)
                {

                    memorandumSale.dept_Id = department.Id;
                }
                //selected customer
                if (customer != null)
                {

                    memorandumSale.customerCompany_Id = customer.Id;
                }
                // selected company
                if (company != null)
                {

                    memorandumSale.company_Id = company.Id;
                }
                
                var myWindow = Window.GetWindow(this);

                if (editmemorandumSale == 1 && memorandumSaleid != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Memorandum Sale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Memorandum Sale") != null))
                {
                    
                    if (MainWindow.currentUserid == 0)
                    {

                    }
                    else if (memorandumSale.user_Id == null)
                        memorandumSale.user_Id = MainWindow.currentUserid;
                    
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorandum Sale without Approval") != null && memorandumSale.isApproved != true)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Memorandum Sale is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            memorandumSale.stage = TransactionStage.Approved.ToString();

                            memorandumSale.isApproved = true;
                            memorandumSale.ApprovedDate = System.DateTime.Now;
                        }
                    }

                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        if (checkStatus.Id != memorandumSale.memorandumSaleStatus.Id)
                        {
                            memorandumSale.LastStatusChangeDate = System.DateTime.Now;
                            if (memorandumSale.memorandumSaleStatus.isActive != true)
                            {
                                memorandumSale.ClosingDate = System.DateTime.Now;
                            }
                        }
                    }
                    memorandumSaleRepo.update(memorandumSale);



                    //if (checkStatus.Id != memorandumSale.memorandumSaleStatus.Id)
                    //{
                    //    List<User> tagUsers = new List<User>();
                    //    List<User> ccUsers = new List<User>();

                    //    var res = MessageBox.Show("Status of Memorandum Sale has been changed, Do you want to notify other users by tagging?)", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    //    if (res == MessageBoxResult.Yes)
                    //    {
                    //        if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                    //        {
                    //            winTagUsers win = new winTagUsers(_usersRepo.getusersByCompanyDepartment(department.Id, company.Id));
                    //            // frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                    //            win.ShowDialog();
                    //            tagUsers = win.tagUsers;
                    //            ccUsers = win.ccUsers;
                    //        }
                    //        else
                    //        {
                    //            winTagUsers win = new winTagUsers();
                    //            win.ShowDialog();

                    //        }

                    //    }

                    //    string oldStat = checkStatus.Status;
                    //    string newStat = memorandumSale.memorandumSaleStatus.Status;
                        
                    //    CommentLog comment = new CommentLog()
                    //    {
                    //        Comment = "Status of Sales Invoice(Amount OC) having value: " + memorandumSale.totalCFRValue.ToString()  + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                    //        Timestamp = DateTime.Now,
                    //        Subject = "Status Changed",
                    //        TaggedList = tagUsers,
                    //        CCUsersList = ccUsers


                    //    };
                    //    procurementRepo.Add(memorandumSale.Id, TransactionItemType.Memorandum_Sale, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //    //Creating notification
                    //    if (tagUsers.Count != 0)
                    //    {
                    //        foreach (var user in tagUsers)
                    //        {
                    //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Memorandum Sale #" + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, comment.Comment, user.id, "New Comment ", null);

                    //        }
                    //    }

                    //    if (ccUsers.Count != 0)
                    //    {
                    //        foreach (var user in ccUsers)
                    //        {
                    //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Memorandum Sale #" + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, comment.Comment, 0, user.id, "New Comment ", null);
                    //        }

                    //    }

                    //    //SaleOrderss.ucStatuschange.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);

                    //    if (checkStatus != null && checkStatus.Id != 0)
                    //    {

                    //        _usersRepo.Add(TransactionInfo.Status_Changed, memorandumSale.Id, 5, "Status Changed from (" + checkStatus.Status + ") to (" + memorandumSale.memorandumSaleStatus.Status + ")");
                    //    }
                    //    frmInputBox inputBox1 = new frmInputBox();
                    //    inputBox1.ShowDialog();
                    //    _usersRepo.Add(TransactionInfo.Edited, memorandumSale.Id, 5, frmInputBox.comment);

                    //    MessageBox.Show("SaleInvoice Updated Succesfully");
                    //    SystemLog.LogInfo(this.GetType(), "SaleInvoice Updated Succesfully refrence No= " + memorandumSale.referenceNo + " Id=" + memorandumSale.Id);

                    //}



                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        //memorandumSaleRepo.Add(memorandumSaleid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + memorandumSale.memorandumSaleStatus.Status + ")");
                        UsersRepo.Add(TransactionInfo.Status_Changed, memorandumSale.Id, 4, "Status Changed from (" + checkStatus.Status + ") to (" + memorandumSale.memorandumSaleStatus.Status + ")");
                    }
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Edited, memorandumSale.Id, 4, frmInputBox.comment);

                    MessageBox.Show("MemorandumSale Updated Succesfully");
                    SystemLog.LogInfo(this.GetType(), "MemorandumSale Updated Succesfully refrence No= " + memorandumSale.referenceNo + " Id=" + memorandumSale.Id);
                    //var myWindow = Window.GetWindow(this);
                    //myWindow.Close();
                    //return;
                }
                else if (editmemorandumSale != 1)
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorandum Sale") != null)
                    {
                        if (MainWindow.currentUserid == 0)
                        {
                            MessageBox.Show("Please Create another Account to Create PO, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                            return;
                        }
                        else
                            memorandumSale.user_Id = MainWindow.currentUserid;
                        //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorandum Sale without Approval") != null /*&& memorandumSale.isApproved == false*/)
                        //{
                        //    //if (DevExpress.Xpf.Core.DXMessageBox.Show("This Memorandum Sale is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        //    {

                        //        memorandumSale.stage = TransactionStage.Approved.ToString();

                        //        memorandumSale.isApproved = true;
                        //        memorandumSale.ApprovedDate = System.DateTime.Now;
                        //    }
                        //}
                        //else
                        //{
                            memorandumSale.stage = TransactionStage.AwaitingFirstReview.ToString();

                            memorandumSale.isApproved = false;
                        //}
                        if (offer.Id != 0 && offer != null)
                        {
                            memorandumSale.Offer_Id = offer.Id;
                        }
                        if (saleOrder?.Id != 0 )
                        {
                            memorandumSale.SaleOrder_Id = saleOrder.Id;
                        }
                        memorandumSaleRepo.Add(memorandumSale);
                        if(Offerss.ucStatuschange.offer.Id != 0)
                        {

                            Offerss.ucStatuschange.offer.LastStatusChangeDate = System.DateTime.Now;
                            Offerss.ucStatuschange.offer.closingDate = System.DateTime.Now;
                            Offerss.ucStatuschange.offerRepo.update(Offerss.ucStatuschange.offer);
                            UsersRepo.Add(TransactionInfo.Closed, Offerss.ucStatuschange.offer.Id, 2, "Closed, Memorandum Sale genrated on this offer");
                        }
                        UsersRepo.Add(TransactionInfo.Initialized, memorandumSale.Id, 4, "");
                        

                        SystemLog.LogInfo(this.GetType(), "MemorandumSale Added Succesfully refrence No= " + memorandumSale.referenceNo + " Id=" + memorandumSale.Id);

                        //grdItems.SaveLayoutToStream(memoryStream);

                        //Properties.Settings.Default["PoItemgridLO"] = streamConverter.converttostring(memoryStream);
                        //Properties.Settings.Default.Save();
                        MessageBox.Show("MemorandumSale Added Succesfully");
                        //myWindow.Close();
                    }
                    else
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to access this form!");
                        //var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                        return;
                    }
                //DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreviewDialog(myWindow, new Reportss.reportMemorandumSaleSingle(memorandumSale));

                //Reportss.frmReportPanel reportPanel = new Reportss.frmReportPanel(new Reportss.reportMemorandumSaleSingle(memorandumSale));
                //reportPanel.Show();

                //ReportPrintToolWpf window = new ReportPrintToolWpf(new reportofferSingle(offer));

                //window.ShowPreviewDialog(myWindow);
                myWindow.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Data Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SystemLog.LogError(this.GetType(), "MemorandumSale Error refrence No= " + memorandumSale.referenceNo + " Id=" + memorandumSale.Id + ex.ToString());

            }

        }
        //private void loadCurrencies()
        //{

        //    CurrencyRepo currencyRepo = new CurrencyRepo();
        //    List<Currency> currencies = currencyRepo.getAll();
        //    List<cmbitem> cmbitems = new List<cmbitem>();
        //    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
        //    foreach (Currency cur in currencies)
        //    {

        //        //cmbCurrency.Items.Add(new cmbitem() { name = cur.CurrencyName, id = cur.Id });
        //        cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
        //    }

        //    cmbCurrency.ItemsSource = cmbitems;
        //    if (cmbbaseCurrency.ItemsSource == null)
        //        cmbbaseCurrency.ItemsSource = cmbitems;

        //}
        //public void loadVendorPaymentStatus()
        //{

        //    List<VendorPaymentStatus> VendorPaymentStatuss = new List<VendorPaymentStatus>();

        //    VendorPaymentStatuss = vendorRepo.getAllActiveVendorPaymentStatus();
        //    List<cmbitem> cmbitems = new List<cmbitem>();

        //    foreach (VendorPaymentStatus status in VendorPaymentStatuss)
        //    {
        //        //string color = status.forecolor;
        //        //status.forecolor=(color!=null||color!="")?status.forecolor:"#FF000000";
        //        cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, fcolor = "#FF000000" });


        //    }
        //    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });
        //    //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
        //    cmbVendorPaymentStatus.ItemsSource = cmbitems;
        //}
        public void loadMemorandumSaleStatus()
        {

            List<MemorandumSaleStatus> MemorandumSaleStatuses = new List<MemorandumSaleStatus>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Memorandum Sale Statuses") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed MemorandumSale") != null)
            {
                MemorandumSaleStatuses = memorandumSaleRepo.getAllMemorandumSaleStatus();

            }
            else

                MemorandumSaleStatuses = memorandumSaleRepo.getAllActiveMemorandumSaleStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (MemorandumSaleStatus status in MemorandumSaleStatuses)
            {
                //string color = status.forecolor;
                //status.forecolor=(color!=null||color!="")?status.forecolor:"#FF000000";
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });


            }
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbMemorandumSaleStatus.ItemsSource = cmbitems;
        }

        private void cmbMemorandumSaleStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbMemorandumSaleStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbMemorandumSaleStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Procurementss.MemorandumSaless.frmMemorandumSaleStatusAdd statusAdd = new Procurementss.MemorandumSaless.frmMemorandumSaleStatusAdd();
                    statusAdd.ShowDialog();
                    loadMemorandumSaleStatus();
                }
            }

        }


        

        private void lookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            customer = lookupCustomer.SelectedItem as CustomerCompany;
            if (customer != null)
            {
                //string selectedcust = customer.company.CompanyName + " (" + customer.contactPerson.FName + ")";
                //lookupCustomer.EditValue = selectedcust;
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
                //lookupCompany.EditValue = selecteddept;
                //lookupCompany.DisplayMember = selecteddept;
                //foreach (cmbitem cmbitem in cmbCurrency.Items)
                //{
                //    if (cmbitem.id == company.CurrencyId)
                //    {
                //        cmbbaseCurrency.SelectedItem = cmbitem;
                //        break;
                //    }
                //}

                loaddepartments();
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
                //string selecteddept = department.DeptName + " (" + department.Code + ")";
                //lookupDepartment.EditValue = selecteddept;

                loadcustomers();

                //lookupVendor.ItemsSource = department.Vendors;
                lookupPrincipal.ItemsSource = department.Principals;
                //loademployees();
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
                //if (department.employees.Count == 0)
                //{
                //    MessageBox.Show("This department do not have Employees. Please select a diffrent department!");
                //    lookupDepartment.Focus();
                //    return;
                //}
                if (department.Principals.Count == 0)
                {
                    MessageBox.Show("No Principal is mapped to this department. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
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
            //loadvendors();
        }

        
        
        string symbol;
        private static InquiryType PoType;
        
        //private void cmbInquiryType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if ((InquiryType)cmbMemorandumSaleType.SelectedIndex == InquiryType.Supply)
        //    {
        //        lblCommision.Text = "Margin Amount" + symbol;
        //        txtCommision.Visibility = Visibility.Collapsed;
        //        txtMargin.Visibility = Visibility.Visible;
        //        grpbondinfo.IsEnabled = false;
        //        grpbondinfo.State = GroupBoxState.Minimized;
        //        //lblBidOpendate.Text = "Quotation Opening Date";
        //        lblDeliveryTime.Visibility = Visibility.Visible;
        //        txtDeliveryTime.Visibility = Visibility.Visible;
        //        //lblBidBondExpirydate.Visibility = Visibility.Hidden;
        //        //lblBidBondIssuedate.Visibility = Visibility.Hidden;
        //        //lblBidBondSubmitdate.Visibility= Visibility.Hidden;
        //        //lblBidBondRefno.Visibility = Visibility.Hidden;
        //        //lblBidBonValue.Visibility = Visibility.Hidden;
        //        //lblIssuingbank.Visibility = Visibility.Hidden;
        //        //txtBidBondRefno.Visibility = Visibility.Hidden;
        //        //txtBidBonValue.Visibility = Visibility.Hidden;
        //        //txtIssuingbank.Visibility = Visibility.Hidden;
        //        //datBidBondSubmitdate.Visibility = Visibility.Hidden;
        //        //datBidBondIssuedate.Visibility = Visibility.Hidden;
        //        //datBidBondSubmitdate.Visibility = Visibility.Hidden;
        //        //datBidBondExpirydate.Visibility = Visibility.Hidden;

        //    }
        //    else if ((InquiryType)cmbMemorandumSaleType.SelectedIndex == InquiryType.Tender)
        //    {
        //        lblCommision.Text = "Commission Amount" + symbol;
        //        txtCommision.Visibility = Visibility.Visible;
        //        txtMargin.Visibility = Visibility.Collapsed;
        //        //lblBidOpendate.Text = "Bid Opening Date";
        //        //lblBidBonValue.Text = "Performance Bond Value";
        //        //lblBidBondIssuedate.Text = "P.Bond Issue Date";
        //        //lblIssuingbank.Text = "P.Bond Issuing Bank";
        //        //lblBidBondRefno.Text = "P.Bond Reference #";
        //        //grpbondinfo.Header = "Performance Bond Information";
        //        grpbondinfo.IsEnabled = true;
        //        //grpbondinfo.Visibility = Visibility.Visible;                
        //        lblDeliveryTime.Visibility = Visibility.Hidden;
        //        txtDeliveryTime.Visibility = Visibility.Hidden;
        //        //lblBidBondExpirydate.Visibility = Visibility.Visible;
        //        //lblBidBondIssuedate.Visibility = Visibility.Visible;
        //        //lblBidBondSubmitdate.Visibility = Visibility.Visible;
        //        //lblBidBondRefno.Visibility = Visibility.Visible;
        //        //lblBidBonValue.Visibility = Visibility.Visible;
        //        //lblIssuingbank.Visibility = Visibility.Visible;
        //        //txtBidBondRefno.Visibility = Visibility.Visible;
        //        //txtBidBonValue.Visibility = Visibility.Visible;
        //        //txtIssuingbank.Visibility = Visibility.Visible;
        //        //datBidBondSubmitdate.Visibility = Visibility.Visible;
        //        //datBidBondIssuedate.Visibility = Visibility.Visible;
        //        //datBidBondSubmitdate.Visibility = Visibility.Visible;
        //        //datBidBondExpirydate.Visibility = Visibility.Visible;
        //    }

        //    else if ((InquiryType)cmbMemorandumSaleType.SelectedIndex == InquiryType.Principal)
        //    {
        //        lblCommision.Text = "Commission Amount" + symbol;
        //        txtCommision.Visibility = Visibility.Visible;
        //        txtMargin.Visibility = Visibility.Collapsed;
        //        //grpbondinfo.Header = "Bid Bond Information";
        //        //lblBidOpendate.Text = "Bid Opening Date";
        //        //lblBidBonValue.Text = "Bid Bond Value";
        //        //lblBidBondIssuedate.Text = "Bid Bond Issue Date";
        //        //lblIssuingbank.Text = "Bid Bond Issuing Bank";
        //        //lblBidBondRefno.Text = "Bid Bond Reference #";
        //        grpbondinfo.IsEnabled = true;
        //        //grpbondinfo.Visibility = Visibility.Visible;
        //        lblDeliveryTime.Visibility = Visibility.Hidden;
        //        txtDeliveryTime.Visibility = Visibility.Hidden;
        //        //lblBidBondExpirydate.Visibility = Visibility.Visible;
        //        //lblBidBondIssuedate.Visibility = Visibility.Visible;
        //        //lblBidBondSubmitdate.Visibility = Visibility.Visible;
        //        //lblBidBondRefno.Visibility = Visibility.Visible;
        //        //lblBidBonValue.Visibility = Visibility.Visible;
        //        //lblIssuingbank.Visibility = Visibility.Visible;
        //        //txtBidBondRefno.Visibility = Visibility.Visible;
        //        //txtBidBonValue.Visibility = Visibility.Visible;
        //        //txtIssuingbank.Visibility = Visibility.Visible;
        //        //datBidBondSubmitdate.Visibility = Visibility.Visible;
        //        //datBidBondIssuedate.Visibility = Visibility.Visible;
        //        //datBidBondSubmitdate.Visibility = Visibility.Visible;
        //        //datBidBondExpirydate.Visibility = Visibility.Visible;
        //    }


        //}


        //private void dGitems_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    if (e.PropertyName == "itemId" || e.PropertyName == "inquiryitemId" || e.PropertyName == "offeritemId")
        //    {
        //        e.Column.Visibility = Visibility.Hidden;
        //    }
        //    if (e.PropertyName == "Item_Name")
        //    {

        //        var cb = new DataGridComboBoxColumn();
        //        cb.Header = "Item";
        //        //products = productrepo.getAll();
        //        //List<cmbitem> cmbitems = new List<cmbitem>();
        //        //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
        //        //foreach (Product prod in products)
        //        //{

        //        //    cmbitems.Add(new cmbitem() { name = prod.item, id = prod.Id });


        //        //}

        //        cb.ItemsSource = loadproducts();
        //        cb.TextBinding = new Binding("Item_Name");
        //        cb.DisplayMemberPath = "name";
        //        cb.SelectedValuePath = "id";/*new List<string> { "C50", "C40", "C30" };*/
        //        cb.SelectedValueBinding = new Binding("name");
        //        var style = new Style(typeof(ComboBox));
        //        style.Setters.Add(new EventSetter(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(ComboBox_SelectionChanged)));
        //        cb.EditingElementStyle = style;
        //        e.Column = cb;

        //    }
        //}
        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var cb = sender as ComboBox;
        //    var cell = sender as DataGridCell;
        //    //int col = cell.Column.DisplayIndex;
        //    //cell = VisualTreeHelper.GetParent(cell);
        //    if ((cb.SelectedItem as cmbitem) != null)
        //    {
        //        int idd = (cb.SelectedItem as cmbitem).id;
        //        if (idd == 0)
        //        {
        //            Productss.frmItemadd frmItem = new Productss.frmItemadd();
        //            frmItem.ShowDialog();
        //            cb.ItemsSource = loadproducts();//loademployees();


        //        }
        //        else
        //        {
        //            product = productrepo.get(idd);

        //            datagriditem item = dGitems.CurrentItem as datagriditem;

        //            DataGridRow row = dGitems.CurrentItem as DataGridRow;
        //            item.Item_Name =product.item;
        //            item.itemId = product.Id;
        //            item.Item_Discription = product.itemDescription;
        //            item.UOM = product.unitOfMeasure.unitOfMeasure;
        //            dGitems.CurrentCell = new DataGridCellInfo(
        //            dGitems.CurrentItem, dGitems.Columns[4]);
        //            dGitems.BeginEdit();
        //            dGitems.CurrentCell = new DataGridCellInfo(
        //            dGitems.CurrentItem, dGitems.Columns[0]);
        //            dGitems.BeginEdit();
        //            dGitems.CurrentCell = new DataGridCellInfo(
        //            dGitems.CurrentItem, dGitems.Columns[6]);
        //            dGitems.BeginEdit();
        //            dGitems.CurrentCell = new DataGridCellInfo(
        //            dGitems.CurrentItem, dGitems.Columns[5]);
        //            dGitems.BeginEdit();
        //            //row.Item = item;
        //            //row.UpdateDefaultStyle();
        //        }



        //    }



        //}
        //public List<cmbitem> loadproducts()
        //{



        //    products = productrepo.getAll();
        //    //dGitems.ItemsSource = products;
        //    //dGitems.DataContext = products;
        //    //ObservableCollection<cmbitem> comaaaa = new ObservableCollection<cmbitem>();
        //    //List<cmbitem> cmbitems = new List<cmbitem>();
        //    //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
        //    //foreach (Product prod in products)
        //    //{

        //    //    comaaaa.Add(new cmbitem() { name = prod.item , id = prod.Id });

        //    //    product = prod;
        //    //}

        //    List<cmbitem> cmbitems = new List<cmbitem>();
        //    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
        //    foreach (Product prod in products)
        //    {

        //        cmbitems.Add(new cmbitem() { name = prod.item, id = prod.Id });


        //    }
        //    return cmbitems;
        //}

        //private void dGitems_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        //        {
        //    double sumfob = 0; double sumcfr = 0;



        //    for (int i = 1; i < dGitems.Items.Count; i++)
        //    {
        //        datagriditem item = dGitems.Items[i - 1] as datagriditem;
        //        {

        //            sumfob += item.FOBvalue;
        //            sumcfr += item.CFRvalue;


        //        }
        //    }
        //    txtfob.Text = sumfob.ToString();
        //    txtcfr.Text = sumcfr.ToString();
        //}

        //private void dGitems_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //{
        //    DataGridRow ite = e.Row;
        //    double sumfob = 0; double sumcfr = 0;

        //    //foreach(DataRow item in dGitems.Items)
        //    //{
        //    //    sumfob +=Convert.ToDouble( item["FOBvalue"]);
        //    //    sumcfr += Convert.ToDouble(item["CFRvalue"]);
        //    //}

        //    for (int i = 1; i < dGitems.Items.Count; i++)
        //    {
        //        datagriditem item = dGitems.Items[i - 1] as datagriditem;
        //        {

        //            sumfob += item.FOBvalue;
        //            sumcfr += item.CFRvalue;


        //        }
        //    }
        //    txtfob.Text = sumfob.ToString();
        //    txtcfr.Text = sumcfr.ToString();
        //}

        private void txttax_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txttax.Text != "")
            {
                calculatetotal();
            }
            //    decimal fob, cfr, tax = 0;
            //    fob = Convert.ToDecimal(txtfob.Text);
            //    cfr = Convert.ToDecimal(txtcfr.Text);
            //    if (txttax.Text != null && txttax.Text != "")
            //    { 
            //        tax = Convert.ToDecimal(txttax.Text);

            //    }
            //    txttotalfob.Text = (fob + (fob * tax/100)).ToString();
            //    txttotalcfr.Text = (cfr + (cfr * tax/100)).ToString();
        }



        private void calculatetotal()
        {
            double sumfob = 0; double sumcfr = 0;
            decimal? weight = 0;
            double Quantity = 0;
            //foreach(DataRow item in dGitems.Items)
            //{
            //    sumfob +=Convert.ToDouble( item["FOBvalue"]);
            //    sumcfr += Convert.ToDouble(item["CFRvalue"]);
            //}

            //for (int i = 1; i < dGitems.Items.Count; i++)
            //{
            //    datagriditem item = dGitems.Items[i - 1] as datagriditem;
            //    {

            //        sumfob += item.value1;
            //        sumcfr += item.value2;


            //    }
            //}
            if (grdItems.ItemsSource != null)
                foreach (var item in grdItems.ItemsSource as List<ProcurementProduct>)
                {
                    //datagriditem item = dGitems.Items[i - 1] as datagriditem;
                    {
                        weight += item.inquiryProduct.Weight != null ? item.inquiryProduct.Weight : 0;
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
            
            //string str = txttax.Text.ToString();
            if (str.IndexOf("%") != -1)
            {
                txttotalfob.Text = (fob + (fob * tax / 100)).ToString();
                txttotalcfr.Text = (cfr + (cfr * tax / 100)).ToString();
               
            }
            else
            {
                txttotalfob.Text = (fob + tax).ToString();
                txttotalcfr.Text = (cfr + tax).ToString();


            }
            decimal marginexchangeRate = 1;
            decimal totalcfr = Convert.ToDecimal(txttotalcfr.Text);
            

        }

        private void dGitems_CurrentCellChanged(object sender, EventArgs e)
        {
            calculatetotal();
        }

     
        public void loadIncoterms()
        {
            IncotermRepo termRepo = new IncotermRepo();
            List<Incoterm> incoterms = new List<Incoterm>();
            incoterms = termRepo.getAll();
            loadCaptions(incoterms);
            //List<cmbitem> cmbitems = new List<cmbitem>();
            //foreach (Incoterm incoterm in incoterms)
            //{
            //    cmbitems.Add(new cmbitem() { name = incoterm.term, id = incoterm.Id });
            //}

            ////cmbcaption1.ItemsSource = cmbitems;
            ////cmbcaption2.ItemsSource = cmbitems;
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            ////cmbIncoterm.ItemsSource = cmbitems;
        }
        public void loadCaptions(List<Incoterm> incoterms)
        {

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (Incoterm incoterm in incoterms)
            {
                cmbitems.Add(new cmbitem() { name = incoterm.term, id = incoterm.Id });
            }

            cmbcaption1.ItemsSource = cmbitems;
            cmbcaption2.ItemsSource = cmbitems;

        }
        
        private void txttax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9|%]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void cmbcaption1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdItems != null && grdItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdItems.Columns[6].Header = ((cmbitem)cmbcaption1.SelectedItem).name + symbol;

        }

        private void cmbcaption2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (grdItems != null && grdItems.Columns.Count != 0 && cmbcaption1.SelectedItem != null)
                grdItems.Columns[7].Header = ((cmbitem)cmbcaption2.SelectedItem).name + symbol;
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
                //lookupPrincipal.EditValue = selectedvend;


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
            Productss.frmItemadd frmItemadd = new Productss.frmItemadd();
            frmItemadd.ShowDialog();
            //products = productrepo.getAll();
            products = SYSTEM_STATIC.GetItemsForCurrentUser();
            //lookupProductinGrid.ItemsSource = products;
            lookupProductsinGrid.ItemsSource = products;


        }
        private void view_InitNewRow(object sender, DevExpress.Xpf.Grid.InitNewRowEventArgs e)
        {
            //Product pro = (Product)lookupProductsinGrid.GetItemFromValue(grdItems.GetCellValue(e.RowHandle, "inquiryProduct.product"));
            (grdItems.CurrentItem as ProcurementProduct).inquiryProduct = new InquiryProduct();
            //(grdItems.CurrentItem as ProcurementProduct).inquiryProduct.product =  as Product;
            //grdItems.SetCellValue(e.RowHandle, "inquiryProduct.product.itemDescription", pro.itemDescription);

        }
        private void winMemorandumSaleadd_Unloaded(object sender, RoutedEventArgs e)
        {
            editmemorandumSale = 0;
            offerid = 0;
            memorandumSaleid = 0;
            //frmCostSheet.costSheet = new CostSheet();
            //SystemLogic.SaveUserSettingForCurrentWindow(grdItems);


        }

     

       

        

       

        

      

       

        private void BtnCostSheet_Click(object sender, RoutedEventArgs e)
        {
            //frmCostSheet frmCostSheet = new frmCostSheet(memorandumSale, txtFinanaceRef.Text, txtSalesref.Text, lookupDepartment.Text, lookupCustomer.Text, txttotalcfr.Text, views);
            //frmCostSheet.ShowDialog();
            //if (frmCostSheet.costSheet != null)
            //{
            //    memorandumSale.CostSheet = frmCostSheet.costSheet;
            //    txtBudgetMargin.Text =(Convert.ToDecimal( txttotalcfr.Text )-memorandumSale.CostSheet.TotalBudgetedMargin).ToString();
            //    txtActualMargin.Text = (Convert.ToDecimal(txttotalcfr.Text) - memorandumSale.CostSheet.TotalActualMargin).ToString();
            //}
        }
        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }
        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            if (memorandumSale.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing MemorandumSale") != null)
                {
                    memorandumSale.isReviewed = true;
                    memorandumSale.PendingForClosing = false;
                    memorandumSale.stage = TransactionStage.Closed.ToString();
                    memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Closing, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                   

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null))
                {
                    memorandumSale.isReviewed = true;
                    memorandumSale.needReview = false;
                    memorandumSale.stage = TransactionStage.AwaitingApproval.ToString();

                    memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null))
                {
                    memorandumSale.isReviewed = true;
                    memorandumSale.needReview = true;
                    memorandumSale.stage = TransactionStage.AwaitingSecondReview.ToString();

                    memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                }

                return;
            }

            if (memorandumSale.isApproved == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added MemorandumSale") != null)
                {
                    memorandumSale.isReviewed = true;
                    memorandumSale.isApproved = true;
                    memorandumSale.stage = TransactionStage.Approved.ToString();
                    memorandumSaleRepo.update(memorandumSale);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Memorandum Sale has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (memorandumSale.department != null && memorandumSale.department.Id != 0 && memorandumSale.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(memorandumSale.department.Id, memorandumSale.company.Id), memorandumSale.Id, TransactionItemType.Memorandum_Sale);
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


                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Memorandum Sale having Ref No. : " + memorandumSale.referenceNo.ToString() + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "Memorandum Sale Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(memorandumSale.Id, TransactionItemType.Memorandum_Sale, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Memorandum Sale #" + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Memorandum Sale #" + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }

                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null))
                {
                    memorandumSale.isReviewed = true;
                    memorandumSale.needReview = false;
                    memorandumSale.stage = TransactionStage.AwaitingApproval.ToString();

                    memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, memorandumSale.Id,(int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null))
                {
                    memorandumSale.isReviewed = true;
                    memorandumSale.needReview = true;
                    memorandumSale.stage = TransactionStage.AwaitingSecondReview.ToString();

                    memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                }
            }
            else if (memorandumSale.isApproved == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added MemorandumSale") != null)
                {
                    memorandumSale.isReviewed = true;
                    memorandumSale.isApproved = false;
                    memorandumSale.stage = TransactionStage.AwaitingApproval.ToString();
                    memorandumSaleRepo.update(memorandumSale);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Memorandum Sale has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (memorandumSale.department != null && memorandumSale.department.Id != 0 && memorandumSale.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(memorandumSale.department.Id, memorandumSale.company.Id), memorandumSale.Id, TransactionItemType.Memorandum_Sale);
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


                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Memorandum Sale having Ref No. : " + memorandumSale.referenceNo.ToString() + " has been UnApproved",
                        Timestamp = DateTime.Now,
                        Subject = "Memorandum Sale UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(memorandumSale.Id, TransactionItemType.Memorandum_Sale, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Memorandum Sale #" + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Memorandum Sale #" + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

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

            if (memorandumSale.isApproved != true)
            {

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added MemorandumSale") != null)
                {
                    memorandumSale.isReviewed = true;
                    memorandumSale.isApproved = false;
                    memorandumSale.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                    memorandumSaleRepo.update(memorandumSale);
                    notificationsRepo.Add("MemorandumSale Rejected", memorandumSale.Id, TransactionItemType.Memorandum_Sale, "MemorandumSale with refrence # " + memorandumSale.referenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", memorandumSale.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this MemorandumSale" + frmInputBox.comment, null);

                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null))
                {
                    memorandumSale.isReviewed = false;
                    memorandumSale.stage = TransactionStage.Rejected.ToString();
                    //if(memorandumSale.isApproved == null)
                    //{
                    //    memorandumSale.isApproved = false;

                    //}
                    //memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                    memorandumSaleRepo.update(memorandumSale);
                    notificationsRepo.Add("MemorandumSale Rejected", memorandumSale.Id, TransactionItemType.Memorandum_Sale, "MemorandumSale with refrence # " + memorandumSale.referenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", memorandumSale.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this MemorandumSale" + frmInputBox.comment, null);

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null))
                {
                    memorandumSale.isReviewed = false;
                    //if (memorandumSale.isApproved == null)
                    //{
                    //    memorandumSale.isApproved = false;

                    //}
                    memorandumSale.stage = TransactionStage.Rejected.ToString();
                    //memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                    memorandumSaleRepo.update(memorandumSale);
                    notificationsRepo.Add("MemorandumSale Rejected", memorandumSale.Id, TransactionItemType.Memorandum_Sale, "MemorandumSale with refrence # " + memorandumSale.referenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", memorandumSale.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this MemorandumSale" + frmInputBox.comment, null);

                }
            }
            else if (memorandumSale.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing MemorandumSale") != null)
                {
                    memorandumSale.isReviewed = true;
                    //if (memorandumSale.PendingForClosing == null)
                    {
                        memorandumSale.PendingForClosing = true;

                    }
                    //memorandumSale.PendingForClosing = false;
                    memorandumSale.stage = TransactionStage.Rejected.ToString();
                    //memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                    memorandumSaleRepo.update(memorandumSale);
                    notificationsRepo.Add("MemorandumSale Rejected", memorandumSale.Id, TransactionItemType.Memorandum_Sale, "MemorandumSale with refrence # " + memorandumSale.referenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", memorandumSale.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this MemorandumSale" + frmInputBox.comment, null);

                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null))
                {
                    memorandumSale.isReviewed = true;
                    memorandumSale.needReview = false;
                    memorandumSale.stage = TransactionStage.Rejected.ToString();
                    //if (memorandumSale.PendingForClosing == null)
                    //{
                    //    memorandumSale.PendingForClosing = true;

                    //}
                    //memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                    memorandumSaleRepo.update(memorandumSale);
                    notificationsRepo.Add("MemorandumSale Rejected", memorandumSale.Id, TransactionItemType.Memorandum_Sale, "MemorandumSale with refrence # " + memorandumSale.referenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", memorandumSale.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this MemorandumSale" + frmInputBox.comment, null);

                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null))
                {
                    //if (memorandumSale.PendingForClosing == null)
                    //{
                    //    memorandumSale.PendingForClosing = true;

                    //}
                    memorandumSale.isReviewed = true;
                    memorandumSale.needReview = true;
                    memorandumSale.stage = TransactionStage.Rejected.ToString();

                    //memorandumSaleRepo.update(memorandumSale);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        addinfo = false;
                    }
                    memorandumSaleRepo.update(memorandumSale);
                    notificationsRepo.Add("MemorandumSale Rejected", memorandumSale.Id, TransactionItemType.Memorandum_Sale, "MemorandumSale with refrence # " + memorandumSale.referenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", memorandumSale.user_Id ?? default(int), SYSTEM_STATIC.currentUser.userName + " rejected this MemorandumSale" + frmInputBox.comment, null);

                }
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
        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
            {

                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Memorandum_Sale);
                inputBox.ShowDialog();

            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (memorandumSale != null)
            {
                if (frmInputBox.comment != "" && memorandumSale.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Memo Sales # " + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Memo Sales # " + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you at Memo Sales # " + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.comment, 0, user.id, "New Comment ",frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you at Memo Sales # " + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        }
                    }
                   
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    procurementRepo.Add(memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.Comment,SYSTEM_STATIC.currentUser.employeeId);
                    loadcomments();
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }

                else if (memorandumSale.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                }

            }
        }
        public void loadcomments()
        {
            if (memorandumSale != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();
                List<CommentLog> comments = new List<CommentLog>();
                comments = procurementRepo.getcommentslogAsc(memorandumSale.Id, TransactionItemType.Memorandum_Sale);
                grdCommentss.ItemsSource = comments;
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
        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach.Visibility == Visibility.Visible)
                grdAttach.Visibility = Visibility.Collapsed;
            else
            {
                grdAttach.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
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
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                        var result = attachment.startDownload(str, TransactionItemType.Memorandum_Sale);
                        if (!string.IsNullOrEmpty(result.Item2))
                        {
                            Process.Start(result.Item2);
                        }
                        else

                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                            });
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
                if (memorandumSaleid != 0)
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
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += memorandumSaleid + "_" + TransactionItemType.Memorandum_Sale.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Memorandum_Sale);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), memorandumSaleid, TransactionItemType.Memorandum_Sale, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, memorandumSale.Id, 4, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(memorandumSaleid, TransactionItemType.Memorandum_Sale);
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
        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (memorandumSale.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void MemorandumSale") != null))
            {
                if (DXMessageBox.Show("This Memorandum Sale is currently in the list of Void Sale Orders! Do you want to remove it from Void?", "Remove Void MemorandumSale", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    memorandumSale.isVoid = false;
                    memorandumSaleRepo.setMemorandumSaletoVoid(memorandumSale.Id, false);
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void MemorandumSale") != null)
            {
                if (DXMessageBox.Show("This Memorandum Sale is not currently in the list of Void Sale Orders! Do you want to move it to Void Saleorders?", "Add to Void MemorandumSale", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    memorandumSale.isVoid = true;
                    memorandumSaleRepo.setMemorandumSaletoVoid(memorandumSale.Id, true);
                }
            }

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
                if (memorandumSale.Id != 0)
                {
                    if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.Memorandum_Sale);
                        inputBox.ShowDialog();

                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (memorandumSale != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.comment != "" && memorandumSale.Id != 0)
                        {
                            if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Memo Sales # " + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Memo Sales # " + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.comment, user.id, "New Comment ", null);
                                }
                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you at Memo Sales # " + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you at Memo Sales # " + memorandumSale.referenceNo, memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.comment, 0, user.id, "New Comment ", null);
                                }
                            }

                            procurementRepo.Add(memorandumSale.Id, TransactionItemType.Memorandum_Sale, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            loadcomments();
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (memorandumSale.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"You can not mark the Notificaton which you Sent as Read/Unread, Only User That Recieved this notification Can Change That!", "Not Allowed", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (OrderId != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(OrderId, TransactionItemType.Memorandum_Sale);
                trackingWindow.ShowDialog();
            }
        }

        private void BtnDirectClose_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();
            memorandumSaleRepo = new MemorandumSaleRepo();
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale without Approval") != null) ? true : false)
            {


                UsersRepo usersRepo = new UsersRepo();
                if (memorandumSaleid == 0)
                {
                    return;
                }
                memorandumSale = memorandumSaleRepo.get(memorandumSaleid);

                var row = memorandumSale;
                if (row.memorandumSaleStatus != null)
                {
                    oldStatus = row.memorandumSaleStatus;
                }
                //MemorandumSaless.ucStatuschange.inActiveStatuses = 1;

                MemorandumSaless.ucStatuschange.memorandumSaleid = memorandumSaleid;
                MemorandumSaless.frmMemorandumSaleStatusChange statusChange = new MemorandumSaless.frmMemorandumSaleStatusChange(memorandumSaleRepo);
                var myWindow = Window.GetWindow(this);
                statusChange.Owner = myWindow;
                statusChange.ShowDialog();
                //if (MemorandumSaless.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Id == row.PurchaseOrderStatus.Id)
                //    return;
                if (MemorandumSaless.ucStatuschange.memorandumSale.Id != 0)
                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Memorandum Sale without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing MemorandumSale") != null) ? true : false)
                    {
                        MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing = false;
                        MemorandumSaless.ucStatuschange.memorandumSale.stage = TransactionStage.Approved.ToString();

                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.memorandumSaleStatus.Status + ") to (" + MemorandumSaless.ucStatuschange.memorandumSale.memorandumSaleStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Closing, MemorandumSaless.ucStatuschange.memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                        //Inquiriess.ucStatuschange.purchaseOrderRepo.update(Inquiriess.ucStatuschange.purchaseOrder);

                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 MemorandumSale") != null)
                    {
                        MemorandumSaless.ucStatuschange.memorandumSale.stage = TransactionStage.AwaitingApproval.ToString();
                        if (MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing == null)
                        {
                            MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing = true;

                        }
                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.memorandumSaleStatus.Status + ") to (" + MemorandumSaless.ucStatuschange.memorandumSale.memorandumSaleStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, MemorandumSaless.ucStatuschange.memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 MemorandumSale") != null)
                    {
                        MemorandumSaless.ucStatuschange.memorandumSale.stage = TransactionStage.AwaitingSecondReview.ToString();
                        if (MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing != true)
                        {
                            MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing = true;

                        }
                        //MemorandumSaless.ucStatuschange.purchaseOrder.PendingForClosing = true;

                        frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.memorandumSaleStatus.Status + ") to (" + MemorandumSaless.ucStatuschange.memorandumSale.memorandumSaleStatus.Status + ")");
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, MemorandumSaless.ucStatuschange.memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                    }
                    else
                    {
                        MemorandumSaless.ucStatuschange.memorandumSale.stage = TransactionStage.AwaitingFirstReview.ToString();

                        MemorandumSaless.ucStatuschange.memorandumSale.PendingForClosing = true;
                        usersRepo.Add(TransactionInfo.Closed, MemorandumSaless.ucStatuschange.memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, frmInputBox.comment);
                    }
                //MemorandumSaless.ucStatuschange.purchaseOrder.user_Id = MainWindow.currentUserid;
                MemorandumSaless.ucStatuschange.memorandumSale.LastStatusChangeDate = System.DateTime.Now;
                MemorandumSaless.ucStatuschange.memorandumSale.ClosingDate = System.DateTime.Now;
                if (row.memorandumSaleStatus != MemorandumSaless.ucStatuschange.memorandumSale.memorandumSaleStatus)
                    usersRepo.Add(TransactionInfo.Status_Changed, memorandumSale.Id, (int)TransactionItemType.Memorandum_Sale, "While direct closing Status Changed from (" + row.memorandumSaleStatus.Status + ") to (" + MemorandumSaless.ucStatuschange.memorandumSale.memorandumSaleStatus.Status + ")");
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
                        winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), memorandumSale.Id, TransactionItemType.Memorandum_Sale);
                        //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
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
                string oldStat = "";
                if (oldStatus != null)
                {
                    oldStat = oldStatus.Status;
                }
                string newStat = MemorandumSaless.ucStatuschange.memorandumSale.memorandumSaleStatus.Status;

                CommentLog comment = new CommentLog()
                {
                    Comment = "Status of Memorandum Sale (Amount OC) having value: " + row.totalCFRValue.ToString() + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                    Timestamp = DateTime.Now,
                    Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                };
                procurementRepo.Add(row.Id, TransactionItemType.Memorandum_Sale, comment, SYSTEM_STATIC.currentUser.employeeId);
                //Creating Comments
                if (tagUsers.Count != 0)
                {
                    foreach (var user in tagUsers)
                    {
                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Memorandum Sale #" + row.referenceNo, row.Id, TransactionItemType.Memorandum_Sale, comment.Comment, user.id, "New Comment ", null);

                    }
                }

                if (ccUsers.Count != 0)
                {
                    foreach (var user in ccUsers)
                    {
                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Memorandum Sale #" + row.referenceNo, row.Id, TransactionItemType.Memorandum_Sale, comment.Comment, 0, user.id, "New Comment ", null);
                    }
                }

                row = MemorandumSaless.ucStatuschange.memorandumSale;

                memorandumSaleRepo.updateStatus(row.Id, row.memorandumSaleStatus);
                //PurchaseOrderss.ucStatuschange.UpdatePurchaseOrder();//purchaseOrderRepo.update(PurchaseOrderss.ucStatuschange.purchaseOrder);
                MessageBox.Show("Memorandum Sale status changed to InActive (" + MemorandumSaless.ucStatuschange.memorandumSale.memorandumSaleStatus.Status + ")");
                myWindow = Window.GetWindow(this);
                myWindow.Close();


            }
            else
            {
                MessageBox.Show("You are not Allowed to Close MemorandumSale Directly.");
            }
        }
    }
}
