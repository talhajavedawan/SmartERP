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
using System.Collections.ObjectModel;
using System.Data;
using DevExpress.Xpf.Grid;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using DevExpress.XtraReports.UI;
using ZAS_ERP.Reportss;
using DevExpress.Xpf.Core;
using Microsoft.Win32;
using System.Diagnostics;
using ZAS_ERP;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Printing;
using ERP_BL.Payments;
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

namespace ZAS_ERP.Procurementss.Inquiriess
{
    /// <summary>
    /// Interaction logic for ucInquiryAdd.xaml
    /// </summary>
    public partial class ucInquiryAdd : UserControl
    {
        public static Inquiry inquiryy { get; set; }
        public ucInquiryAdd()
        {
            inquiryProducts = new List<InquiryProduct>();
            products = new List<Product>();
            //DataContext = this;
            InitializeComponent();
            SystemLog.LogInfo(this.GetType(), "Form Initialized ");

            // this.DataContext = new MyViewModel();
        }
        public static int editinquiry;
        public static int inquiryid;
        public int OrderId;
        public int editOrder;
        ProductRepo productRepo = new ProductRepo();
        EmployeeRepo cont1 = new EmployeeRepo();
        public ObservableCollection<cmbitem> CompanyItems { get; set; }
        ProductRepo productrepo = new ProductRepo();
        Product product = new Product();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public InquiryStatus checkStatus = new InquiryStatus();

        public virtual List<Product> products { get; set; }
        InquiryRepo inquiryRepo = new InquiryRepo();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        //Section section = new Section();
        ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
        CustomerCompany customer = new CustomerCompany();
        User user = new User();
        Inquiry inquiry = new Inquiry();
        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();
        bool addinfo = true;
        public virtual List<InquiryProduct> inquiryProducts { get; set; }
        ProcurementRepo procurementRepo = new ProcurementRepo();
        public InquiryStatus oldStatus = new InquiryStatus();

        Inquiry trackingOrder = new Inquiry();
        List<InquiryStatus> inquiryStatuses = new List<InquiryStatus>();


        private void wininquiryadd_Loaded(object sender, RoutedEventArgs e)
        {
            SystemLog.LogInfo(this.GetType(), "Form Loaded ");

            grdTrackingTree.ExpandAllNodes();

            OrderId = inquiryid;
            editOrder = editinquiry;
            loadcompanies();
            //loadcustomers();
            //loademployees();
            //loaddepartments();

            loadinquiryTypes();
            //loadproducts();
            //List<datagriditem> datagriditems = new List<datagriditem>();
            products = SYSTEM_STATIC.GetItemsForCurrentUser();//productrepo.getAll();
            //{  
            //    products.Add(new Product()
            //    {
            //        Id = product.Id,
            //        categoryId = product.categoryId,
            //        item = product.item,
            //        code = product.code,
            //        itemDescription = product.itemDescription,
            //        ownDescription = product.ownDescription,
            //        nature_Id = product.nature_Id,
            //        unitOfMeasureId = product.unitOfMeasureId,
            //        unitOfMeasure = product.unitOfMeasure,
            //        nature = product.nature,
            //        category = product.category,
            //        isActive = product.isActive
            //    });
            //}
            //lookupProductinGrid.ItemsSource = products;
     
            loadInquiryStatus();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Creation Date of Inquiry") != null)
            {
                datinquiryCreationdate.IsEnabled = true;
            }
            else
            {
                datinquiryCreationdate.IsEnabled = false;
                datinquiryCreationdate.EditValue = System.DateTime.Now;

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Attach a file with Inquiry") != null)
            {
                //btnAttachNew.Visibility = Visibility.Visible;
            }
            else
            {
                btnAttachNew.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of attached files with Inquiry") != null)
            {
                btnAttachmentList.Visibility = Visibility.Visible;
            }
            else
            {
                btnAttachmentList.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Inquiry") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Inquiry") != null)
            {
                btnSetVoid.Visibility = Visibility.Visible;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inquiry") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null)
            {
                btnMarkReviewed.Visibility = Visibility.Visible;
                btnReject.Visibility = Visibility.Visible;
            }
            else
            {
                btnReject.Visibility = Visibility.Collapsed;

                btnMarkReviewed.Visibility = Visibility.Collapsed;

            }
            if (editinquiry != 0 && inquiryid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inquiry") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Inquiry Details") != null)
                { //products = productrepo.getAll();

                    //lookupProductinGrid.ItemsSource = products;
                    inquiry = inquiryRepo.get(inquiryid);
                    loadonInquirydata();
                    GellAllOrdersTracking();
                    views = UsersRepo.getViwerInfo(inquiry.Id, 1);
                    grdUsers.ItemsSource = views;
                    loadcomments();
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(inquiry.Id, TransactionItemType.Inquiry);
                    cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                }
                //Setting void stamp
                if (inquiry != null)
                {
                    if (inquiry.isVoid == true)
                    {
                        grdVoid.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Inquiry!");
                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;

                }

            }
            else
            {
                //products = productrepo.getAll();

                //lookupProductinGrid.ItemsSource = products;
                grdInquiryItems.ItemsSource = inquiryProducts;
            }
            LoadCreator();
            cmbInquiryType.Focus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdInquiryItems);
            grdTrackingTree.ExpandAllNodes();
        }
        public void GellAllOrdersTracking()
        {
            trackingOrder = inquiryRepo.get(inquiry.Id);
            OrderTracking tracking = new OrderTracking();
            grdOrdersTracking.ItemsSource = tracking.getTransactions(trackingOrder.Id, TransactionItemType.Inquiry);

        }
        private void LoadCreator()
        {
            if (editinquiry == 0)
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
            else if (inquiry.user != null)
                txtCreator.Text = inquiry.user.employee.person.FName + " " + inquiry.user.employee.person.LName;
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
            //public List<Product> products { get; set; }
            public int Id { get; set; }
            public int inquiryitemId { get; set; }
            public string Item_Name { get; set; }
            public string Item_Discription { get; set; }
            public string Own_Description { get; set; }
            public string UOM { get; set; }
            public double Quantity { get; set; }
        }

        public List<cmbitem> loadproducts()
        {
            products = productrepo.getAll();
            //lookupProductsinGrid.ItemsSource = products;
            //dGitems.ItemsSource = products;
            //dGitems.DataContext = products;
            //ObservableCollection<cmbitem> comaaaa = new ObservableCollection<cmbitem>();
            //List<cmbitem> cmbitems = new List<cmbitem>();
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            //foreach (Product prod in products)
            //{
            //    comaaaa.Add(new cmbitem() { name = prod.item , id = prod.Id });
            //    product = prod;
            //}
            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            foreach (Product prod in products)
            {
                cmbitems.Add(new cmbitem() { name = prod.item, id = prod.Id });
            }
            return cmbitems;
        }
        public void loadcompanies()
        {
            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();

                this.lookupCompany.ItemsSource = cont.GetCompanies();
                return;
                //List<ERP_BL.Databases.Company> companies = new List<ERP_BL.Databases.Company>();
                //foreach (ERP_BL.Databases.Company comp in companylist)
                //    if (comp.compnayType == ERP_BL.Enums.CompnayTypes.Company)
                //        foreach (ERP_BL.Databases.Company empcomp in empUser.Companies)
                //            if (comp.Id == empcomp.Id)
                //                companies.Add(comp);
                //this.lookupCompany.ItemsSource = companies;
            }

            var usernew = cont1.getuser(MainWindow.currentUserid);
            empUser = cont1.GetEmployeeCompanies(usernew.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;

        }
        public void loadcustomers()
        {
            if(company != null && department != null)
            if (company.Id != 0)
            {
                    if (department.Id != 0)
                    {
                        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
                         List<CustomerCompany> customers = /*customerCompRepo.getCustomersForCompanyAndDepartment(company.Id, department.Id)*/ department.customers.ToList();

                        if (editOrder == 1)
                        {
                            if(inquiry.Id!=0)
                            {
                                var customer = department.disableCustomers.FirstOrDefault(x => x.Id == inquiry.customerCompany_Id);
                                if(customer!=null)
                                {
                                    customers.AddRange(department.disableCustomers);
                                }
                            }
                        }



                    if (customers == null|| customers.Count==0)
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

        public void loadinquiryTypes()
        {
            Config config = new Config();
            List<string> InquiryType = config.getInquiryType();
            if (InquiryType.Count > 0)
                foreach (string IND in InquiryType)
                {
                    cmbInquiryType.Items.Add(IND);
                }
        }

        public void loadonInquirydata()
        {

            if (inquiry.isVoid == true)
            {
                grdVoid.Visibility = Visibility.Visible;
                txtVoid.RenderTransform = new RotateTransform(-45);
                //lblStage.Text = "Void";
            }
            //else if (saleInvoice.isReApproved == false)
            //{
            //    //lblStage.Text = "Under Re-Approval";
            //}
            else if (inquiry.isApproved == true && inquiry.stage == "Closed")
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (inquiry.isApproved == true && inquiry.inquiryStatus.isActive == false && inquiry.PendingForClosing != true)
            {
                //lblStage.Text = "Approved and Closed";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.DeepSkyBlue;
            }
            else if (inquiry.isApproved == true && inquiry.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (inquiry.isApproved == true)
            {
                //lblStage.Text = "Approved";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (inquiry.isApproved == false)
            {
                //lblStage.Text = "Under Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.LightGray;
                grdUnderClosing.Background = Brushes.LightGray;
                grdClosed.Background = Brushes.LightGray;
            }
            else if (inquiry.PendingForClosing == true)
            {
                //lblStage.Text = "Under Closing Approval";

                grdUnderApproval.Background = Brushes.DeepSkyBlue;
                grdApproved.Background = Brushes.DeepSkyBlue;
                grdUnderClosing.Background = Brushes.DeepSkyBlue;
                grdClosed.Background = Brushes.LightGray;
            }

            datLastStatusChangeDate.EditValue = inquiry.LastStatusChangeDate;
            datinquirydate.DateTime = inquiry.inquiryDate;
            datduedate.DateTime = inquiry.DeliveryDueDate;
            datalertDate.DateTime = inquiry.alertDate;
            datinquiryCreationdate.DateTime = (DateTime)((inquiry.CreationDate != null) ? inquiry.CreationDate : datinquiryCreationdate.DateTime);
            datclosedate.DateTime = inquiry.lastSubmissionDate;
            txtfileref.Text = inquiry.SalesReferenceNo;
            txtinquiryref.Text = inquiry.referenceNo;
            txtComments.Text = inquiry.Comments;
            txtOwnDescription.Text = inquiry.OwnDescription;
            lblStage.Text = (inquiry.stage != null) ? inquiry.stage : "";
            checkStatus = inquiry.inquiryStatus;
            // Select Company
            if (inquiry.company_Id != null || inquiry.company != null)
            {
                company = inquiry.company;
                lookupCompany.Text = inquiry.company.CompanyName;

                //lookupCompany.SelectedItem = lookupCompany.GetItemByKeyValue(inquiry.company);
                //lookupCompany.EditValue = lookupCompany.GetItemByKeyValue(inquiry.company.Id);
                //lookupCompany.SelectedIndex = lookupCompany.GetItemByKeyValue(inquiry.company.Id);



                //foreach (ItemCollection item in lookupCompany.ItemsSource)
                //{
                //    if (inquiry.inquirytype.ToString() == item.ToString())
                //        cmbInquiryType.SelectedItem = item;

                //}
                //lookupDepartment.ItemsSource = company.departments;
                //lookupCompany.SelectedItem = company;
                //loaddepartments();

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
                //loadcustomers();
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
            if (inquiry.inquiryStatus != null)
            {
                var disAbleStatus = inquiryStatuses.FirstOrDefault(x => x.Id == inquiry.inquiryStatus.Id);
                if (disAbleStatus == null)
                {
                    loadInquiryStatus(inquiry.inquiryStatus);
                }
            } 
            // Delect Inquiry Type Same for Offer Type
            foreach (String item in cmbInquiryType.Items)
            {
                if (inquiry.inquirytype.ToString() == item.ToString())
                    cmbInquiryType.SelectedItem = item;

            }
            // Select Inquiry Status as of Inquiry Status     /////not valid 
            foreach (cmbitem cmbitem in cmbInquiryStatus.Items)
            {
                if (cmbitem.name == inquiry.inquiryStatus.Status)
                {

                    cmbInquiryStatus.SelectedItem = cmbitem;
                }
            }
            if (inquiry.inquiryStatus.isActive == false && MainWindow.currentUserid != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Closed Inquiry") == null)
            {
                grdinquirydata.IsEnabled = false;
                btnAttachment.IsEnabled = false;
                labeltopStatus.Visibility = Visibility.Visible;
                labeltopStatus.Text = inquiry.inquiryStatus.Status;
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(inquiry.inquiryStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                labeltopStatus.Foreground = new SolidColorBrush(newColor);
                var rotateAnimation = new DoubleAnimation(-30, -32, TimeSpan.FromSeconds(60));
                var rt = (RotateTransform)labeltopStatus.RenderTransform;
                rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
                //labeltopStatus.Visibility = Visibility.Visible;
                //labeltopStatus.Text = inquiry.inquiryStatus.Status;
                //System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(inquiry.inquiryStatus.backcolor);
                //System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                //labeltopStatus.Foreground = new SolidColorBrush(newColor);
            }
            // load Products in Inquiry to grid
            //List<datagriditem> datagriditems = new List<datagriditem>();
            if (inquiry.products != null)
            {
                List<InquiryProduct> inqueryItems = new List<InquiryProduct>();


                foreach (InquiryProduct inquiryProduct in inquiry.products)
                {

                    inquiryProducts.Add(new InquiryProduct()
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
                    });
                }
                grdInquiryItems.ItemsSource = inquiryProducts;
                //lookupProductinGrid.ShowText = true;
                //lookupProductinGrid.DisplayMember = null;

            }
            else
                grdInquiryItems.ItemsSource = inquiryProducts;


            //        datagriditems.Add(new datagriditem {inquiryitemId = inqueryItem.Id,Id=inqueryItem.product_Id , Item_Name = inqueryItem.product.item, Item_Discription = inqueryItem.product.itemDescription, Own_Description = inqueryItem.ownDiscription, UOM = inqueryItem.UOM, Quantity = inqueryItem.quantity });
            //    }
            //dGitems.ItemsSource = datagriditems;
            if (inquiry.transactionHolderId != null)
            {
                try
                {
                    var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                    cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == inquiry.transactionHolderId))];

                }
                catch (Exception ex)
                {

                }
            }
            if (inquiry.holderChangeDate != null)
            {
                var time = DateTime.Now-inquiry.holderChangeDate;
                txtHolderDays.Text = time.Days.ToString();
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
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbEmployee.ItemsSource = cmbitems;
            if (department.employees.Count == 0)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("This department dose not have Employees. Please select a diffrent department!");
                lookupDepartment.Focus();
                return;
            }
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
            {
                if (company.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    foreach (Department dep in company.departments.Where(x=>x.IsInquiryType == true))
                    {
                        foreach (Department empdep in empUser.departments)
                            if (dep.Id == empdep.Id)
                            {
                                departments.Add(dep);
                            }
                    }
                    if(editOrder != 0 && OrderId != 0)
                        if (inquiry.department != null && departments.FirstOrDefault(x => x.Id == inquiry.dept_Id) == null)
                            departments.Add(inquiry.department);
                    lookupDepartment.ItemsSource = departments;
                   

                    if (departments.Count == 0)
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
            }
        }





        public List<InquiryProduct> getformdata()
        {
            List<InquiryProduct> inqueryItems = new List<InquiryProduct>();
            List<InquiryProduct> inqItems = new List<InquiryProduct>();
            InquiryProduct product = new InquiryProduct();
            inqueryItems = grdInquiryItems.ItemsSource as List<InquiryProduct>;
            foreach (var inquiryProduct in inqueryItems)
            {
                if (inquiryProduct.Id == 0)
                {
                    if (inquiryProduct.product.Id != 0)
                    {
                        //product = inquiryProduct;
                        //product.product_Id = inquiryProduct.product.Id;
                        //product.UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure;
                        inqItems.Add(new InquiryProduct() { ownDiscription = inquiryProduct.ownDiscription, product_Id = inquiryProduct.product.Id,/*product=inquiryProduct.product,*/UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure, quantity = inquiryProduct.quantity, Weight = inquiryProduct.Weight });
                    }
                }
                else
                {

                    inqItems.Add(new InquiryProduct() { Id = inquiryProduct.Id, ownDiscription = inquiryProduct.ownDiscription, product_Id = inquiryProduct.product.Id,/*product=inquiryProduct.product,*/UOM = inquiryProduct.product.unitOfMeasure.unitOfMeasure, quantity = inquiryProduct.quantity, Weight = inquiryProduct.Weight });
                }

            }
            //    datagriditem item = dGitems.Items[i - 1] as datagriditem;
            //    {
            //        //if (item.inquiryitemId == 0) 
            //        //{ 
            //        //InquiryProduct inquiryit = new InquiryProduct();
            //        //    //Product product = new Product();
            //        //    inquiryit.Id = item.inquiryitemId;
            //        ////inquiryit.product = product;
            //        //inquiryit.product_Id = item.Id;
            //        ////inquiryit.product.Id = item.Id;
            //        ////inquiryit.product.item = item.Item_Name;
            //        ////inquiryit.product.itemDescription = item.Item_Discription;
            //        //inquiryit.UOM = item.UOM;
            //        //inquiryit.ownDiscription = item.Own_Description;
            //        //inquiryit.quantity = item.Quantity;
            //        //inqueryItems.Add(inquiryit);
            //        //}
            //        //else                     {
            //        //    InquiryProduct inquiryit = new InquiryProduct();
            //        //    //Product product = new Product();
            //        //    inquiryit.Id = item.inquiryitemId;
            //        //    //inquiryit.product = product;
            //        //    inquiryit.product_Id = item.Id;
            //        //    //inquiryit.product.Id = item.Id;
            //        //    //inquiryit.product.item = item.Item_Name;
            //        //    //inquiryit.product.itemDescription = item.Item_Discription;
            //        //    inquiryit.UOM = item.UOM;
            //        //    inquiryit.ownDiscription = item.Own_Description;
            //        //    inquiryit.quantity = item.Quantity;
            //        //    inqueryItems.Add(inquiryit);
            //        //}

            //        if (item.Id != 0)
            //        {
            //            InquiryProduct inquiryit = new InquiryProduct()
            //            {
            //                Id = item.inquiryitemId,

            //                product_Id = item.Id,
            //                ownDiscription = item.Own_Description,
            //                UOM = item.UOM,
            //                quantity = item.Quantity
            //            };
            //            inqueryItems.Add(inquiryit);
            //        }
            //        //else
            //        //{
            //        //    ProcurementProduct offerit = new ProcurementProduct()
            //        //    {
            //        //        Id = item.offeritemId,
            //        //        product_Id = item.inquiryitemId,
            //        //        value1 = item.value1,
            //        //        value2 = item.value2,
            //        //        caption1 = cmbcaption1.Text,
            //        //        caption2 = cmbcaption2.Text
            //        //    };
            //        //    offerItems.Add(inquiryit);
            //        //}

            //        //productrepo.Add(inqueryit);

            //    }

            //}

            return inqItems;
        }
        //public List<InquiryProduct> getformdata()
        //{
        //List<InquiryProduct> inqueryItems = new List<InquiryProduct>();

        //    for (int i = 1; i < dGitems.Items.Count; i++)
        //    {
        //        datagriditem item = dGitems.Items[i - 1] as datagriditem;
        //        {
        //            //if (item.inquiryitemId == 0) 
        //            //{ 
        //            //InquiryProduct inquiryit = new InquiryProduct();
        //            //    //Product product = new Product();
        //            //    inquiryit.Id = item.inquiryitemId;
        //            ////inquiryit.product = product;
        //            //inquiryit.product_Id = item.Id;
        //            ////inquiryit.product.Id = item.Id;
        //            ////inquiryit.product.item = item.Item_Name;
        //            ////inquiryit.product.itemDescription = item.Item_Discription;
        //            //inquiryit.UOM = item.UOM;
        //            //inquiryit.ownDiscription = item.Own_Description;
        //            //inquiryit.quantity = item.Quantity;
        //            //inqueryItems.Add(inquiryit);
        //            //}
        //            //else                     {
        //            //    InquiryProduct inquiryit = new InquiryProduct();
        //            //    //Product product = new Product();
        //            //    inquiryit.Id = item.inquiryitemId;
        //            //    //inquiryit.product = product;
        //            //    inquiryit.product_Id = item.Id;
        //            //    //inquiryit.product.Id = item.Id;
        //            //    //inquiryit.product.item = item.Item_Name;
        //            //    //inquiryit.product.itemDescription = item.Item_Discription;
        //            //    inquiryit.UOM = item.UOM;
        //            //    inquiryit.ownDiscription = item.Own_Description;
        //            //    inquiryit.quantity = item.Quantity;
        //            //    inqueryItems.Add(inquiryit);
        //            //}

        //            if (item.Id != 0)
        //            {
        //                InquiryProduct inquiryit = new InquiryProduct()
        //                {
        //                    Id = item.inquiryitemId,

        //                    product_Id = item.Id,
        //                    ownDiscription = item.Own_Description,
        //                    UOM = item.UOM,
        //                quantity = item.Quantity
        //            };
        //                inqueryItems.Add(inquiryit);
        //            }
        //            //else
        //            //{
        //            //    ProcurementProduct inquiryit = new ProcurementProduct()
        //            //    {
        //            //        Id = item.offeritemId,
        //            //        product_Id = item.inquiryitemId,
        //            //        value1 = item.value1,
        //            //        value2 = item.value2,
        //            //        caption1 = cmbcaption1.Text,
        //            //        caption2 = cmbcaption2.Text
        //            //    };
        //            //    offerItems.Add(offerit);
        //            //}

        //            //productrepo.Add(inqueryit);

        //        }

        //    }

        //    return inqueryItems;
        //}

        private void btniquiryadd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbInquiryType.SelectedIndex == -1)
                {
                    cmbInquiryType.Focus();
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please Select a Template for Inquiry", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Question);
                    return;

                }
                else if (lookupCustomer.SelectedIndex == -1 && customer.Id == 0)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please Select a Customer for whom Inquiry is being genrated", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCustomer.Focus();
                    return;
                }
                else if (lookupCompany.SelectedIndex == -1 && company.Id == 0)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please select a Refrence Company", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupCompany.Focus();
                    return;
                }
                else if (lookupDepartment.SelectedIndex == -1 && department.Id == 0)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please Select a Refrence Department", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    lookupDepartment.Focus();
                    return;
                }
                else if (cmbEmployee.SelectedIndex == -1 && employee.EmpId == 0)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please Select an Employee to whom this Inquiry will be Allocated ", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbEmployee.Focus();
                    return;
                }
                else if (cmbInquiryStatus.SelectedIndex == -1)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please Select Current Status of Inquiry to Continue.", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbInquiryStatus.Focus();
                    return;
                }
                else if (txtinquiryref.Text == "")
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please Enter Inquiry Reference Number", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtinquiryref.Focus();
                    return;
                }
                else if (datinquirydate.DateTime == DateTime.MinValue)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please Select Inquiry Opening Date!", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    datinquirydate.Focus();
                    return;
                }
                else if (datclosedate.DateTime == DateTime.MinValue)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please Select Inquiry Closing Date!", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    datinquirydate.Focus();
                    return;
                }
                //else if (datclosedate.DateTime <= datinquirydate.DateTime)
                //{
                //    DevExpress.Xpf.Core.DXMessageBox.Show("Closing date can't be equal or less than Opening date", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    datinquirydate.Focus();
                //    return;
                //}
                else if (datclosedate.DateTime == DateTime.MinValue)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please Select Inquiry Closing Date!", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    datinquirydate.Focus();
                    return;
                }
                else if (cmbTransactionHolder.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select transaction holder", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbTransactionHolder.Focus();
                    return;
                }
                List<InquiryProduct> inqueryItems = getformdata();
                if (inqueryItems.Count == 0 || inqueryItems == null)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Please select items for Inquiry!", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    datinquirydate.Focus();
                    return;
                }
                if (cmbTransactionHolder.SelectedIndex != -1)
                {
                    inquiry.transactionHolderId = (cmbTransactionHolder.SelectedItem as cmbitem).id;
                    inquiry.holderChangeDate = (DateTime)datHolderDate.EditValue;
                }
                DateTime? dateTime = null;
                inquiry.alertDate = datalertDate.DateTime;
                inquiry.inquiryDate = datinquirydate.DateTime;
                inquiry.CreationDate = (datinquiryCreationdate.Text == "") ? dateTime : datinquiryCreationdate.DateTime;
                inquiry.DeliveryDueDate = datduedate.DateTime;
                inquiry.lastSubmissionDate = datclosedate.DateTime;
                inquiry.OwnDescription = txtOwnDescription.Text.Trim();
                inquiry.Comments = txtComments.Text.Trim();
                inquiry.SalesReferenceNo = txtfileref.Text.Trim();
                inquiry.referenceNo = txtinquiryref.Text.Trim();
                inquiry.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
                inquiry.inquirytype = (InquiryType)cmbInquiryType.SelectedIndex;
                if ((cmbInquiryStatus.SelectedItem as cmbitem) != null)
                {

                    InquiryStatus status = inquiryRepo.getstatus((cmbInquiryStatus.SelectedItem as cmbitem).id);
                    inquiry.inquiryStatus = status;
                }
                if (!string.IsNullOrEmpty(txtWeight.Text))
                {
                    inquiry.TotalWeight = Convert.ToDecimal(txtWeight.Text);
                }
                else
                {
                    inquiry.TotalWeight = null;
                }
                if (!string.IsNullOrEmpty(txtQuantity.Text))
                {
                    inquiry.TotalQuantity = Convert.ToDecimal(txtQuantity.Text);
                }
                else
                {
                    inquiry.TotalQuantity = null;
                }

                if (department != null)
                {

                    inquiry.dept_Id = department.Id;
                }
                if (customer != null)
                {

                    inquiry.customerCompany_Id = customer.Id;
                }
                if (company != null)
                {

                    inquiry.company_Id = company.Id;
                }

                //if ((lookupCustomer.SelectedItem as cmbitem) != null)
                //{
                //    if ((cmbsubcustomer.SelectedItem as cmbitem) != null)
                //    {

                //        inquiry.customerCompany_Id = (cmbsubcustomer.SelectedItem as cmbitem).id;
                //    }
                //    else
                //    {
                //        inquiry.customerCompany_Id = (cmbCustomerName.SelectedItem as cmbitem).id;
                //    }
                //}



                //inquiry.user = user;
                //inquiry.user_Id = user.id;
                //inquiry.allocation_Id = employee.EmpId; (cmbSection.SelectedItem as cmbitem).id
                //inquiry.employee = employee;
                //inquiry.dept_Id = department.Id;
                //inquiry.department = department;
                //inquiry.sec_Id = section.Id;
                //inquiry.section = section;
                //inquiry.customerCompany_Id = customer.Id;
                //inquiry.customerCompany = customer;
                //inquiry.user = user;
                inquiry.products = inqueryItems;
                var myWindow = Window.GetWindow(this);

                if (editOrder != 0 && OrderId != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inquiry") != null)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null && inquiry.isApproved == false)
                    {
                        if (DevExpress.Xpf.Core.DXMessageBox.Show("This Inquiry is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            inquiry.stage = TransactionStage.Approved.ToString();
                            inquiry.isApproved = true;
                            inquiry.ApprovedDate = System.DateTime.Now;
                        }
                    }
                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        if (checkStatus.Id != inquiry.inquiryStatus.Id)
                        {
                            inquiry.LastStatusChangeDate = System.DateTime.Now;
                            if (inquiry.inquiryStatus.isActive != true)
                            {
                                inquiry.ClosingDate = System.DateTime.Now;
                            }
                        }
                    }

                    //Adding comment signature
                    if (checkStatus.Id != inquiry.inquiryStatus.Id)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Status of Inquiry has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (department != null && department.Id != 0 && company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), inquiry.Id, TransactionItemType.Inquiry);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                if (win.tagUsers.Count > 0)
                                {
                                    if (inquiry.transactionHolderId != win.tagUsers[0].employeeId)
                                    {
                                        inquiry.holderChangeDate = DateTime.Now;
                                    }
                                    inquiry.transactionHolderId = win.tagUsers[0].employeeId;

                                }
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();
                            }
                        }

                        string oldStat = checkStatus.Status;
                        string newStat = inquiry.inquiryStatus.Status;
                        string symbolCurr = "";

                        if (inquiry != null)
                        {
                            // symbolCurr = inquiry.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            //Comment = "Status of SO having value: " + txttotalfob.Text + "(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Comment = "Status of Inquiry has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(inquiry.Id, TransactionItemType.Inquiry, comment, SYSTEM_STATIC.currentUser.employeeId);

                        //Creating notification
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Inquiry No " + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, user.id, "New Comment",null);
                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Inquiry No " + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, 0,user.id, "New Comment", null);
                            }
                        }

                    }
                    inquiryRepo.update(inquiry);

                    if (checkStatus != null && checkStatus.Id != 0)
                    {
                        //inquiryRepo.Add(inquiryid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + inquiry.inquiryStatus.Status + ")");
                        UsersRepo.Add(TransactionInfo.Status_Changed, inquiry.Id, 1, "Status Changed from (" + checkStatus.Status + ") to (" + inquiry.inquiryStatus.Status + ")");
                    }
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Edited, inquiry.Id, 1, frmInputBox.comment);
                    SystemLog.LogInfo(this.GetType(), "Inquiry Updated Succesfully refrence No= " + inquiry.referenceNo + "inquiry Id=" + inquiry.Id);

                    DevExpress.Xpf.Core.DXMessageBox.Show("Inquiry Updated Succesfully");


                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry") != null)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        DevExpress.Xpf.Core.DXMessageBox.Show("Please Create another Account to Create Inquiry, You are not Authorized", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

                        return;
                    }
                    else
                        inquiry.user_Id = MainWindow.currentUserid;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry without Approval") != null)
                    {
                        inquiry.isApproved = true;
                        inquiry.stage = TransactionStage.Approved.ToString();
                        inquiry.ApprovedDate = System.DateTime.Now;
                    }
                    else
                    {
                        inquiry.stage = TransactionStage.AwaitingFirstReview.ToString();

                        inquiry.isApproved = false;
                    }

                    inquiryRepo.Add(inquiry);
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.ShowDialog();
                    UsersRepo.Add(TransactionInfo.Initialized, inquiry.Id, 1, frmInputBox.comment);

                    SystemLog.LogInfo(this.GetType(), "Inquiry Added Succesfully refrence No= " + inquiry.referenceNo/*+"inquiry Id="+inquiry.Id*/);
                    DevExpress.Xpf.Core.DXMessageBox.Show("Inquiry Added Succesfully");
                    //var myWindow = Window.GetWindow(this);
                    //myWindow.Close();

                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Access this form!");
                    // myWindows = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
                //reportInquirySingle report = new reportInquirySingle();
                //IList<Inquiry> inquiriesList = new List<Inquiry>();
                //inquiriesList.Add(inquiry);
                //report.DataSource = inquiriesList;

                ////ReportPrintTool tool = new ReportPrintTool(report);
                ////tool.ShowPreview();
                ////InquiryReport report = new InquiryReport();

                ////report.Parameters["InquiryId"].Value = inquiry.Id;
                ////report.Parameters["InquiryId"].Visible = false;
                //ReportPrintToolWpf window = new ReportPrintToolWpf(report);
                addinfo = false;
                //window.ShowPreviewDialog(myWindow);
                //DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreviewDialog(myWindow, new reportInquirySingle(inquiry));
                //frmReportPanel reportPanel = new frmReportPanel(new reportInquirySingle(inquiry));
                //reportPanel.Show();
                myWindow.Close();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), "Inquiry Adding/Editing Error refrence No= " + inquiry.referenceNo + "inquiry Id=" + inquiry.Id + ex.ToString());

                DevExpress.Xpf.Core.DXMessageBox.Show(ex.ToString(), "Data Saving Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Procurementss.frmProcurmentPanel.inquiryid = inquiry.Id;
        }
        public void loadInquiryStatus()
        {
            //List<cmbitem> cmbitems = new List<cmbitem>();
            //var Items = SYSTEM_STATIC.statusSources.FirstOrDefault(x => x.name == "Inquiries").Items.ToList();
            //cmbitems.AddRange(Items.FirstOrDefault(x => x.name == "Inquiries(Open)").Items.Distinct().ToList());
            //if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inquiries") != null))
            //    cmbitems.AddRange(Items.FirstOrDefault(x => x.name == "Inquiries(Closed)").Items.Distinct().ToList());
            //cmbInquiryStatus.ItemsSource = cmbitems;


            inquiryStatuses = inquiryRepo.getAllActiveStatus();
            inquiryStatuses = inquiryStatuses.Where(x => x.isDisable != true).ToList();

            List<cmbitem> cmbitems = new List<cmbitem>();


            foreach (InquiryStatus status in inquiryStatuses)
            {

                //string color = status.forecolor;
                //status.forecolor=(color!=null||color!="")?status.forecolor:"#FF000000";
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            cmbInquiryStatus.ItemsSource = cmbitems;

        }
        public void loadInquiryStatus(InquiryStatus _inquiryStatus)
        {
            inquiryStatuses.Add(_inquiryStatus);
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (InquiryStatus status in inquiryStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0, fcolor = "#0000FF" });

            cmbInquiryStatus.ItemsSource = cmbitems;

        }
        private void cmbInquiryStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbInquiryStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbInquiryStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmInquiryStatusAdd statusAdd = new frmInquiryStatusAdd();
                    statusAdd.ShowDialog();
                    loadInquiryStatus();
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
                //string selectedcust = customer.company.CompanyName + " (" + customer.contactPerson.FName + ")";
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

                loaddepartments();
            }

        }
        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (company.Id == 0)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Select Company Frist");
                lookupCompany.Focus();
                return;
            }
        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                if (department.ParentID == null && department.subDepartments.Count != 0)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Cannot map to a parent Department directly. Please select a diffrent department!");
                    lookupDepartment.Focus();
                    return;
                }
                string selecteddept = department.DeptName + " (" + department.Code + ")";
                //lookupDepartment.EditValue = selecteddept;
                var products = productRepo.getAllDepartmentProducts(department.Id);
                lookupProductsinGrid.ItemsSource = products;

                //lookupCustomer.ItemsSource = department.customers;
                loadcustomers();
                loademployees();

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
            lookupDepartment.ItemsSource = company.departments;

        }

        private void btnAddCompany_Click(object sender, RoutedEventArgs e)
        {
            Companiess.frmcompanyadd frmcompanyadd = new Companiess.frmcompanyadd();
            frmcompanyadd.ShowDialog();
            loadcompanies();

        }

        //private void dGitems_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    if (e.PropertyName == "Id"|| e.PropertyName == "inquiryitemId")
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
        //        string displaymember="name" ;
        //        cb.ItemsSource = loadproducts();
        //        cb.TextBinding = new Binding("Item_Name");
        //        cb.DisplayMemberPath = "name";
        //        cb.SelectedValuePath = "id";/*new List<string> { "C50", "C40", "C30" };*/
        //        cb.SelectedValueBinding = new Binding(displaymember);
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
        //            item.Id = product.Id;
        //            item.Item_Name = product.item;
        //            item.Item_Discription = product.itemDescription;
        //            item.UOM = product.unitOfMeasure.unitOfMeasure;
        //            dGitems.CurrentCell = new DataGridCellInfo(
        //            dGitems.CurrentItem, dGitems.Columns[5]);
        //            dGitems.BeginEdit();
        //            dGitems.CurrentCell = new DataGridCellInfo(
        //            dGitems.CurrentItem, dGitems.Columns[3]);
        //            dGitems.BeginEdit();
        //            dGitems.CurrentCell = new DataGridCellInfo(
        //            dGitems.CurrentItem, dGitems.Columns[1]);
        //            dGitems.BeginEdit();
        //            dGitems.CurrentCell = new DataGridCellInfo(
        //            dGitems.CurrentItem, dGitems.Columns[4]);
        //            dGitems.BeginEdit();
        //            //row.Item = item;
        //            //row.UpdateDefaultStyle();
        //        }



        //    }



        //        }

        //private void dGitems_AutoGeneratingColumn_1(object sender, DevExpress.Xpf.Grid.AutoGeneratingColumnEventArgs e)
        //{

        //    if (e.Column.FieldName == "Item_Name")
        //    {
        //        var cb = new GridColumn();
        //        cb.Header = "Item";
        //        //products = productrepo.getAll();
        //        //List<cmbitem> cmbitems = new List<cmbitem>();
        //        //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
        //        //foreach (Product prod in products)
        //        //{
        //        //    cmbitems.Add(new cmbitem() { name = prod.item, id = prod.Id });
        //        //}           
        //        //cb.ItemsSource = loadproducts();
        //        //cb.DisplayMemberPath = "name";
        //        //cb.SelectedValuePath = "id";/*new List<string> { "C50", "C40", "C30" };*/
        //        //cb.SelectedValueBinding = new Binding("name");
        //        //var style = new Style(typeof(ComboBox));
        //        //style.Setters.Add(new EventSetter(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(ComboBox_SelectionChanged)));
        //        //cb.EditingElementStyle = style;
        //        //dGitems.Columns.Add( cb);


        //    }

        //}

        private void datclosedate_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

            //if (datinquirydate.DateTime != DateTime.MinValue && datclosedate.DateTime <= datinquirydate.DateTime)
            //{
            //    DevExpress.Xpf.Core.DXMessageBox.Show("Closing date can't be equal or less than Opening date", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);

            //    return;
            //}
            try
            {
                if (datclosedate.DateTime.Year >= 2005)
                {
                    if (datclosedate.DateTime.Day == 2)
                    {
                        if(datclosedate.DateTime.Month == 1)
                        {
                            int days = DateTime.DaysInMonth(datclosedate.DateTime.Year - 1, 12);
                            datalertDate.DateTime = new DateTime(datclosedate.DateTime.Year - 1, 12, days);
                        }
                        else
                        {
                            int days = DateTime.DaysInMonth(datclosedate.DateTime.Year, datclosedate.DateTime.Month - 1);
                            datalertDate.DateTime = new DateTime(datclosedate.DateTime.Year, datclosedate.DateTime.Month - 1, days);
                        }
                        
                    }
                    else if(datclosedate.DateTime.Day == 1)
                    {
                        if (datclosedate.DateTime.Month == 1)
                        {
                            int days = DateTime.DaysInMonth(datclosedate.DateTime.Year - 1, 12);
                            datalertDate.DateTime = new DateTime(datclosedate.DateTime.Year - 1, 12, days-1);
                        }
                        else
                        {
                            int days = DateTime.DaysInMonth(datclosedate.DateTime.Year, datclosedate.DateTime.Month - 1);

                            datalertDate.DateTime = new DateTime(datclosedate.DateTime.Year, datclosedate.DateTime.Month - 1, days - 1);
                        }
                        
                    }
                    else
                        datalertDate.DateTime = new DateTime(datclosedate.DateTime.Year, datclosedate.DateTime.Month, datclosedate.DateTime.Day - 2);

                }
                    
            }
            catch (Exception ex)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show(ex.ToString());
            }
        }

        private void cmbEmployee_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Select Department Frist");
                lookupDepartment.Focus();
                return;
            }
        }

        private void lookupCustomer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (department.Id == 0)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Select Department Frist");
                //lookupDepartment.Focus();
                return;
            }
        }
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Items") != null)
            {

                Productss.frmItemadd frmItemadd = new Productss.frmItemadd();
                frmItemadd.ShowDialog();
                products = SYSTEM_STATIC.GetItemsForCurrentUser();
                //lookupProductinGrid.ItemsSource = products;
            }
            else
            {
                DXMessageBox.Show("Permission required (Add New Items) to add new item!");
            }



        }
        private void PART_GridControl_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {

        }

        private void wininquiryadd_Unloaded(object sender, RoutedEventArgs e)
        {
            inquiryid = 0;
            editinquiry = 0;
            if (addinfo)
                UsersRepo.Add(TransactionInfo.viewed, inquiry.Id, 1, "");

            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdInquiryItems);


        }

        private void grdInquiryItems_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {

        }

        private void grdInquiryItems_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "product.code")
            {
                if (inquiry != null)
                {
                    foreach (InquiryProduct inquiryProduct in inquiry.products)
                    {


                        e.DisplayText = inquiryProduct.product.code;

                    }
                }
                //e.DisplayText = (grdInquiryItems.CurrentItem as InquiryProduct).product.code;
            }
        }

        private void TableView_InitNewRow(object sender, InitNewRowEventArgs e)
        {

            //(grdInquiryItems.CurrentItem as InquiryProduct).product = productrepo.get(grdInquiryItems.GetFocusedRowCellDisplayText("product.code")); /*CurrentItem as InquiryProduct).product.code*/
        }

        private void grdInquiryItems_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {

        }

        private void PART_GridControl_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {

        }



        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            if (inquiry.isApproved == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inquiry") != null)
                {
                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    inquiry.isReviewed = true;
                    inquiry.isApproved = true;
                    inquiry.stage = TransactionStage.Approved.ToString();
                    inquiryRepo.update(inquiry);



                    var res1 = MessageBox.Show("Inquiry has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (inquiry.department != null && inquiry.department.Id != 0 && inquiry.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(inquiry.department.Id, inquiry.company.Id), inquiry.Id, TransactionItemType.Inquiry);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count != 0)
                            {
                                if (inquiry.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    inquiry.holderChangeDate = DateTime.Now;
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

                    
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Inquiry having Sales Ref No. : " + inquiry.SalesReferenceNo.ToString() + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "Inquiry Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(inquiry.Id, TransactionItemType.Inquiry, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inquiry #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inquiry #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    MessageBox.Show("Inquiry is Approved (" + inquiry.SalesReferenceNo + ")");
                    SystemLog.LogInfo(this.GetType(), "Inquiry is Approved (" + inquiry.Id + ")");




                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }

          
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null))
                {
                    inquiry.isReviewed = true;
                    inquiry.needReview = false;
                    inquiry.stage = TransactionStage.AwaitingApproval.ToString();

                    inquiryRepo.update(inquiry);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null))
                {
                    inquiry.isReviewed = true;
                    inquiry.needReview = true;
                    inquiry.stage = TransactionStage.AwaitingSecondReview.ToString();

                    inquiryRepo.update(inquiry);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                }
            }
            else if (inquiry.isApproved == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inquiry") != null)
                {
                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Inquiry is Approved, Do you want to UnApprove this Inquiry?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                    if (res == MessageBoxResult.Yes)
                    {
                        inquiry.isApproved = false;
                        inquiry.stage = TransactionStage.AwaitingApproval.ToString();

                        //receipt.isApproved = true;

                        //receipt.stage = TransactionStage.Approved.ToString();
                        //frmInputBox inputBox = new frmInputBox();
                        //inputBox.ShowDialog();
                        userRepo.Add(TransactionInfo.Approved_Adding, inquiry.Id, 6, "PO has been UnApproved");
                        //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                        //saleOrderrepo = new SaleOrderRepo();
                        inquiryRepo.update(inquiry);

                        var res1 = MessageBox.Show("Inquiry has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res1 == MessageBoxResult.Yes)
                        {

                            if (inquiry.department != null && inquiry.department.Id != 0 && inquiry.company?.Id != 0 )
                            {
                                winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), inquiry.Id, TransactionItemType.Inquiry);
                                win.ShowDialog();
                                tagUsers = win.tagUsers;
                                if (win.tagUsers.Count != 0)
                                {
                                    if (inquiry.transactionHolderId != win.tagUsers[0].employeeId)
                                    {
                                        inquiry.holderChangeDate = DateTime.Now;
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
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Inquiry having Sales Ref No. : " + inquiry.SalesReferenceNo.ToString() + " has been UnApproved \n ",
                            Timestamp = DateTime.Now,
                            Subject = "Inquiry UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(inquiry.Id, TransactionItemType.Inquiry, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inquiry #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inquiry #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, 0, user.id, "New Comment ", null);
                            }
                        }

                        MessageBox.Show("Inquiry is UnApproved (" + inquiry.SalesReferenceNo + ")");
                        SystemLog.LogInfo(this.GetType(), "Purchase Order is UnApproved (" + inquiry.Id + ")");
                    }



                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }
            else if (inquiry.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null)
                {
                    inquiry.isReviewed = true;
                    inquiry.PendingForClosing = false;
                    inquiry.stage = TransactionStage.Closed.ToString();
                    inquiryRepo.update(inquiry);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Closing, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null))
                {
                    inquiry.isReviewed = true;
                    inquiry.needReview = false;
                    inquiry.stage = TransactionStage.AwaitingApproval.ToString();

                    inquiryRepo.update(inquiry);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null))
                {
                    inquiry.isReviewed = true;
                    inquiry.needReview = true;
                    inquiry.stage = TransactionStage.AwaitingSecondReview.ToString();

                    inquiryRepo.update(inquiry);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewed, inquiry.Id, 1, frmInputBox.comment);
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
            if (inquiry.isApproved != true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Inquiry") != null)
                {
                    inquiry.isReviewed = true;
                    inquiry.isApproved = false;
                    inquiry.stage = TransactionStage.Rejected.ToString();

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                    inquiryRepo.update(inquiry);
                    notificationsRepo.Add("Inquiry Rejected", inquiry.Id, TransactionItemType.Inquiry, "Inquiry with refrence # " + inquiry.referenceNo + " was rejected by " + SYSTEM_STATIC.currentUser.employee.person.FName, inquiry.user_Id, SYSTEM_STATIC.currentUser.userName + " rejected this Inquiry " + frmInputBox.comment, null);
                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null))
                {
                    inquiry.isReviewed = false;
                    inquiry.stage = TransactionStage.Rejected.ToString();
                    //inquiryRepo.update(inquiry);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                    inquiryRepo.update(inquiry);
                    notificationsRepo.Add("Inquiry Rejected", inquiry.Id, TransactionItemType.Inquiry, "Inquiry under review with refrence # " + inquiry.referenceNo + " was rejected by " + SYSTEM_STATIC.currentUser.employee.person.FName, inquiry.user_Id, SYSTEM_STATIC.currentUser.userName + " rejected this Inquiry " + frmInputBox.comment, null);

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null))
                {
                    inquiry.isReviewed = false;
                    inquiry.stage = TransactionStage.Rejected.ToString();
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();

                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                    inquiryRepo.update(inquiry);

                    notificationsRepo.Add("Inquiry Rejected", inquiry.Id, TransactionItemType.Inquiry, "Inquiry under review with refrence # " + inquiry.referenceNo + " was rejected by " + SYSTEM_STATIC.currentUser.employee.person.FName, inquiry.user_Id, SYSTEM_STATIC.currentUser.userName + " rejected this Inquiry " + frmInputBox.comment, null);

                }
            }
            else if (inquiry.PendingForClosing == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null)
                {
                    inquiry.isReviewed = true;
                    //inquiry.PendingForClosing = false;
                    inquiry.stage = TransactionStage.Rejected.ToString();
                    //inquiryRepo.update(inquiry);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approver_Rejected, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                    inquiryRepo.update(inquiry);

                    notificationsRepo.Add("Inquiry Rejected", inquiry.Id, TransactionItemType.Inquiry, "Inquiry pending for Closing with refrence # " + inquiry.referenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", inquiry.user_Id, SYSTEM_STATIC.currentUser.userName + " rejected this Inquiry " + frmInputBox.comment, null);

                    return;

                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null))
                {
                    inquiry.isReviewed = true;
                    inquiry.needReview = false;
                    inquiry.stage = TransactionStage.Rejected.ToString();

                    //inquiryRepo.update(inquiry);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                    inquiryRepo.update(inquiry);

                    notificationsRepo.Add("Inquiry Rejected", inquiry.Id, TransactionItemType.Inquiry, "Inquiry pending for Closing Under Review with refrence # " + inquiry.referenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", inquiry.user_Id, SYSTEM_STATIC.currentUser.userName + " rejected this Inquiry " + frmInputBox.comment, null);

                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null))
                {
                    inquiry.isReviewed = true;
                    inquiry.needReview = true;
                    inquiry.stage = TransactionStage.Rejected.ToString();

                    //inquiryRepo.update(inquiry);
                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Reviewer_Rejected, inquiry.Id, 1, frmInputBox.comment);
                        addinfo = false;
                    }
                    inquiryRepo.update(inquiry);

                    notificationsRepo.Add("Inquiry Rejected", inquiry.Id, TransactionItemType.Inquiry, "Inquiry pending for Closing Under Review with refrence # " + inquiry.referenceNo + " was rejected by (" + SYSTEM_STATIC.currentUser.employee.person.FName + ")", inquiry.user_Id, SYSTEM_STATIC.currentUser.userName + " rejected this Inquiry " + frmInputBox.comment, null);

                }
            }
        }

        private void view_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            decimal? weight = 0;
            double Quantity = 0;
            if (grdInquiryItems.ItemsSource != null)
                foreach (var item in grdInquiryItems.ItemsSource as List<InquiryProduct>)
                {
                    //datagriditem item = dGitems.Items[i - 1] as datagriditem;
                    {
                        if (item.quantity != 0)
                        {
                            weight += (item.Weight != null ? item.Weight : 0) * Convert.ToDecimal(item.quantity);
                        }
                        else
                        {
                            weight += item.Weight != null ? item.Weight : 0;
                        }
                        Quantity += item.quantity;



                    }
                }
            txtWeight.Text = weight.ToString();
            txtQuantity.Text = Quantity.ToString();
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
            if (department != null && department.Id != 0 && company?.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
            {

                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Inquiry);
                inputBox.ShowDialog();

            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }


            if (inquiry != null)
                if (frmInputBox.comment != "" && inquiry.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(inquiry.Id, TransactionItemType.Inquiry, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inquiry with Ref #" + inquiry.referenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inquiry with Ref #" + inquiry.referenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inquiry with Ref #" + inquiry.referenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inquiry with Ref #" + inquiry.referenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                            if (frmInputBox.Comment.TaggedList.Count > 0)
                            {
                                var transactionHolder = frmInputBox.Comment.TaggedList[0].employeeId;
                                var empSource = (List<cmbitem>)cmbTransactionHolder.Items.SourceCollection;
                                cmbTransactionHolder.SelectedItem = cmbTransactionHolder.Items[cmbTransactionHolder.Items.IndexOf(empSource.Find(x => x.id == transactionHolder))];
                            }
                        }


                        //foreach (var user in frmInputBox.Comment.TaggedList)
                        //{
                        //    if (frmInputBox.FlagForTag == true)
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                        //    else
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, user.id, "New Comment ", null);

                        //}
                        //foreach (var user in frmInputBox.Comment.CCUsersList)
                        //{
                        //    if (frmInputBox.FlagForCC == true)
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                        //    else
                        //        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Purchase Invoice #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        //}
                    }

                    //procurementRepo.Add(inquiry.Id, TransactionItemType.Inquiry, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (inquiry.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Inquiry first to add a comment!");
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
                if (inquiry.Id != 0)
                {
                    if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), comment, TransactionItemType.Inquiry);
                        inputBox.ShowDialog();

                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (inquiry != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && inquiry.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(inquiry.Id, TransactionItemType.Inquiry, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inquiry with Ref #" + inquiry.referenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Inquiry with Ref #" + inquiry.referenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inquiry with Ref #" + inquiry.referenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " mentioned you in Inquiry with Ref #" + inquiry.referenceNo, inquiry.Id, TransactionItemType.Inquiry, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            //if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            //{
                            //    foreach (var user in frmInputBox.Comment.TaggedList)
                            //    {
                            //        if (frmInputBox.FlagForTag == true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Inquiry, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Inquiry, frmInputBox.comment, user.id, "New Comment ", null);

                            //    }
                            //    foreach (var user in frmInputBox.Comment.CCUsersList)
                            //    {

                            //        if(frmInputBox.FlagForCC==true)
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Inquiry, frmInputBox.comment, 0,user.id, "New Comment ", frmInputBox.FlagId);
                            //        else
                            //            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in SO #" + saleOrder.SalesReferenceNo, saleOrder.Id, TransactionItemType.Inquiry, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            //    }
                            //}

                            //procurementRepo.Add(saleOrder.Id, TransactionItemType.Inquiry, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (inquiry.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Inquiry first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"You can not mark the Notificaton which you Sent as Read/Unread, Only User That Recieved this notification Can Change That!", "Not Allowed", MessageBoxButton.OK, MessageBoxImage.Hand);
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
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }
        public void loadcomments()
        {
            if (inquiry != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();


                List<CommentLog> comments = new List<CommentLog>();
                comments = procurementRepo.getcommentslogAsc(inquiry.Id, TransactionItemType.Inquiry);
                
                grdCommentss.ItemsSource = comments;
            }
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
                        destination += "Attachments\\Inquiry\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            if (fileDialog.FileName.Length > 50)
                            {
                                MessageBox.Show("File name too long", "long file name", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                           
                            destination += OrderId + "_" + TransactionItemType.Inquiry.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Inquiry);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Inquiry, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, inquiry.Id, 1, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Inquiry);
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
            if (inquiryid != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(inquiryid, TransactionItemType.Inquiry);
                trackingWindow.ShowDialog();
            }
            

        }
        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (inquiry.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Inquiry") != null))
            {
                if (DXMessageBox.Show("This Inquiry is currently in the list of Void Sale Orders! Do you want to remove it from Void?", "Remove Void Sale Order", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    inquiry.isVoid = false;
                    inquiryRepo.setInquirytoVoid(inquiry.Id, false);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Inquiry has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (inquiry.department != null && inquiry.department.Id != 0 && inquiry.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), inquiry.Id, TransactionItemType.Inquiry);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (inquiry.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    inquiry.holderChangeDate = DateTime.Now;
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

                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Inquiry having Sale Ref No.: " + inquiry.SalesReferenceNo.ToString() + " has been marked as Unvoid ",
                        Timestamp = DateTime.Now,
                        Subject = "Inquiry UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(inquiry.Id, TransactionItemType.Inquiry, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inquiry #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inquiry #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Inquiry") != null)
            {
                if (DXMessageBox.Show("This Inquiry is not currently in the list of Void Sale Orders! Do you want to move it to Void Saleorders?", "Add to Void Saleorders", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    inquiry.isVoid = true;
                    inquiryRepo.setInquirytoVoid(inquiry.Id, true);


                    grdVoid.Visibility = Visibility.Visible;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Inquiry has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (inquiry.department != null && inquiry.department.Id != 0 && inquiry.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), inquiry.Id, TransactionItemType.Inquiry);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                            if (win.tagUsers.Count > 0)
                            {
                                if (inquiry.transactionHolderId != win.tagUsers[0].employeeId)
                                {
                                    inquiry.holderChangeDate = DateTime.Now;
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
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Inquiry having Sales Ref No. : " + inquiry.SalesReferenceNo.ToString() + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Inquiry Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(inquiry.Id, TransactionItemType.Inquiry, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inquiry #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Inquiry #" + inquiry.SalesReferenceNo, inquiry.Id, TransactionItemType.Inquiry, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();

        }

        private void BtnDirectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry") != null) ? true : false)
            {
                if (inquiryid == 0)
                { return; }
                inquiry = inquiryRepo.get(inquiryid);

                Inquiriess.ucStatuschange.inquiryid = (int)inquiryid;

                    var row = inquiry;
                    if (row.inquiryStatus != null)
                    {
                        oldStatus = row.inquiryStatus;
                    }
                    Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                    //statusChange.Owner = this;
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (Inquiriess.ucStatuschange.inquiry.Id != 0)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null) ? true : false)
                        {
                            Inquiriess.ucStatuschange.inquiry.PendingForClosing = false;
                            Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.Closed.ToString();
                            //Inquiriess.ucStatuschange.inquiryRepo.update(Inquiriess.ucStatuschange.inquiry);

                        }
                        else
                        {
                            Inquiriess.ucStatuschange.inquiry.PendingForClosing = true;
                            Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.AwaitingFirstReview.ToString();

                        }
                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null) ? true : false)
                    {
                        Inquiriess.ucStatuschange.inquiry.PendingForClosing = false;

                        //Inquiriess.ucStatuschange.inquiryRepo.update(Inquiriess.ucStatuschange.inquiry);

                    }
                    UsersRepo usersRepo = new UsersRepo();

                    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Inquiry without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing") != null) ? true : false)
                    {
                        Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.Closed.ToString();

                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Approved_Closing, Inquiriess.ucStatuschange.inquiry.Id, 1, frmInputBox.comment);
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2") != null)
                    {
                        Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.AwaitingApproval.ToString();
                        if (Inquiriess.ucStatuschange.inquiry.PendingForClosing == null)
                        {
                            Inquiriess.ucStatuschange.inquiry.PendingForClosing = true;
                        }
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Inquiriess.ucStatuschange.inquiry.Id, 1, frmInputBox.comment);
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1") != null)
                    {
                        Inquiriess.ucStatuschange.inquiry.stage = TransactionStage.AwaitingSecondReview.ToString();
                        if (Inquiriess.ucStatuschange.inquiry.PendingForClosing == null)
                        {
                            Inquiriess.ucStatuschange.inquiry.PendingForClosing = true;
                        }
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        usersRepo.Add(TransactionInfo.Reviewed, Inquiriess.ucStatuschange.inquiry.Id, 1, frmInputBox.comment);
                    }
                    //Adding Signaure
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Inquiry has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), inquiry.Id, TransactionItemType.Inquiry);
                            //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                            win.ShowDialog();
                            tagUsers = win.tagUsers;
                        if (win.tagUsers.Count != 0)
                        {
                            if (inquiry.transactionHolderId != win.tagUsers[0].employeeId)
                            {
                                inquiry.holderChangeDate = DateTime.Now;
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
                    string oldStat = "";
                    if (oldStatus != null)
                    {
                        oldStat = oldStatus.Status;
                    }
                    string newStat = Inquiriess.ucStatuschange.inquiry.inquiryStatus.Status;
                    string symbolCurr = "";

                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of Inquiry has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(row.Id, TransactionItemType.Inquiry, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Inquiry No " + row.SalesReferenceNo, row.Id, TransactionItemType.Inquiry, comment.Comment, user.id, "New Comment", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you at Inquiry No " + row.SalesReferenceNo, row.Id, TransactionItemType.Inquiry, comment.Comment, 0,user.id, "New Comment", null);
                        }
                    }

                    //}

                    Inquiriess.ucStatuschange.inquiry.user_Id = MainWindow.currentUserid;
                    Inquiriess.ucStatuschange.inquiry.ClosingDate = System.DateTime.Now;
                    Inquiriess.ucStatuschange.inquiry.LastStatusChangeDate = System.DateTime.Now;
                    Inquiriess.ucStatuschange.inquiryRepo.update(Inquiriess.ucStatuschange.inquiry);


                    MessageBox.Show("Inquiry status changed to InActive (" + Inquiriess.ucStatuschange.inquiry.inquiryStatus.Status + ")");
                var thisWindow = Window.GetWindow(this);
                thisWindow.Close();
            }
            else
            {
                MessageBox.Show("You are not Allowed to close Inquiry Directly");
            }
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments1.Visibility == Visibility.Visible)
                grdAttachments1.Visibility = Visibility.Collapsed;
            else
            {
                List<TreeItem> atachments = SYSTEM_STATIC.GetInquiryAttachmentsListByCategory((int)inquiry.Id, TransactionItemType.Inquiry);
                if (inquiry.Id != 0)
                {
                    List<TreeItem> otherAttachments = new List<TreeItem>();
                    OfferRepo offerRepo = new OfferRepo();
                    SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                    List<Offer> offers = offerRepo.getAllByInquiryId(inquiry.Id);
                    if(offers.Count!=0)
                    {
                        foreach (var offer in offers)
                        {
                            otherAttachments.AddRange(SYSTEM_STATIC.GetInquiryAttachmentsListByCategory(offer.Id, TransactionItemType.Offer));
                            List<SaleOrder> saleOrders = saleOrderRepo.GellAllbyOfferId(offer.Id);
                            if (saleOrders.Count != 0)
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
                }
                treeViewAttachments1.ItemsSource = atachments;
                grdAttachments1.Visibility = Visibility.Visible;
            }

        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                if(inquiry.Id!=0)
                {
                    cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveInquiryAttachmentCategories();

                }
                grdAttach1.Visibility = Visibility.Visible;
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
                        destination += "Attachments\\Inquiry\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            if (fileDialog.FileName.Length > 50)
                            {
                                MessageBox.Show("File name too long", "long file name", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;

                            destination += OrderId + "_" + TransactionItemType.Inquiry.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Inquiry);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), OrderId, TransactionItemType.Inquiry, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, inquiry.Id, 1, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Inquiry);
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

        private void BtnEmail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editOrder != 0 && OrderId != 0 && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inquiry") != null)
                {

                    EmployeeRepo empRepo = new EmployeeRepo();
                    var employee = empRepo.GetEmployee(SYSTEM_STATIC.currentUser.employee.EmpId);
                    List<string> lstAllRecipients = new List<string>();
                    //Below is hardcoded - can be replaced with db data
                    lstAllRecipients.Add("test@testmail.com");
                    lstAllRecipients.Add("test1a@testmail.com");

                    Outlook.Application outlookApp = new Outlook.Application();
                    Outlook._MailItem oMailItem = (Outlook._MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
                    Outlook.Inspector oInspector = oMailItem.GetInspector;
                    // Thread.Sleep(10000);

                    // Recipient
                    Outlook.Recipients oRecips = (Outlook.Recipients)oMailItem.Recipients;
                    foreach (String recipient in lstAllRecipients)
                    {
                        Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(recipient);
                        oRecip.Resolve();
                    }

                    //Add CC
                    Outlook.Recipient oCCRecip = oRecips.Add("testN@testmail.com");
                    oCCRecip.Type = (int)Outlook.OlMailRecipientType.olCC;
                    oCCRecip.Resolve();

                    //Add Subject
                    //oMailItem.Subject = "Test Mail";
                    oMailItem.Body = txtBody.Text;
                    // body, bcc etc...

                    //Display the mailbox
                    oMailItem.Display(true);
                }
            }
            catch (Exception objEx)
            {
                DXMessageBox.Show(objEx.Message);
            }
            
            
        }
        private void cmbTransactionHolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inquiry.Id != 0)
            {
                if (cmbTransactionHolder.SelectedItem != null)
                {

                    if (inquiry.transactionHolderId != (cmbTransactionHolder.SelectedItem as cmbitem).id)
                    {
                        datHolderDate.EditValue = DateTime.Now;
                    }
                    else
                    {
                        datHolderDate.EditValue = inquiry.holderChangeDate;
                    }
                  
                }    
            }
            else
            {
                datHolderDate.EditValue = DateTime.Now;
            }
        }

        private void cmbTransactionHolder_SelectionChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
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

                if (department != null && department.Id != 0 && company?.Id != 0 )
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Inquiry);
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

                if (frmInputBox.commentAdded == true && inquiry.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);

                    //procurementRepo.Add(inquiry.Id, TransactionItemType.Inquiry, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (inquiry.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Purchase Invoice first to add a comment!");
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
    }
}
