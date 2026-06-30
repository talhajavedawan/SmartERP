using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ERP_BL.Enums;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ERP_BL;
using ERP_BL.Config;
using DevExpress.Xpf.Grid;

namespace ZAS_ERP.Inquiriess
{
    /// <summary>
    /// Interaction logic for frmInquiryadd.xaml
    /// </summary>
    public partial class frmInquiryadd : Window
    {
        InquiryRepo inquiryRepo = new InquiryRepo();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        //Section section = new Section();
        ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        UsersRepo UsersRepo = new UsersRepo();
        CustomerCompany customer = new CustomerCompany();
        User user = new User();
        public frmInquiryadd()
        {
            InitializeComponent();
        }

        private void wininquiryadd_Loaded(object sender, RoutedEventArgs e)
        {
            loadcompanies();
            loadcustomers();
            loaddepartments();
            loademployees();
            loadinquiries();
            List<datagriditem> datagriditems = new List<datagriditem>();
            dGitems.ItemsSource = datagriditems;
            loadInquiryStatus();
            
        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }
            
        }
        public class datagriditem
        {
            public string Item_Name { get; set; }
            public string Item_Discription { get; set; }
            public string Own_Description { get; set; }
            public string UOM { get; set; } 
            public double Quantity { get; set; }
            

        }
        public void loadcompanies()
        {
            CompanyRepo cont = new CompanyRepo();
            List<ERP_BL.Databases.Company> companylist = new List<ERP_BL.Databases.Company>();
            companylist = cont.GetCompanies();
            List<ERP_BL.Databases.Company> companies = new List<ERP_BL.Databases.Company>();
            foreach (ERP_BL.Databases.Company comp in companylist)
                if (comp.compnayType == ERP_BL.Enums.CompnayTypes.Company)
                    companies.Add(comp);
            this.lookupCompany.ItemsSource = companies;
        }
        public void loadcustomers()
        {
            DepartmentRepo cont = new DepartmentRepo();
            List<ERP_BL.Databases.Department> deptRepo = new List<ERP_BL.Databases.Department>();
            deptRepo = cont.GetDepartments();
            List<Department> deptlst = new List<Department>();
            foreach (Department dept in deptRepo)
            {
                //if (dept.IsSubsidary == false && dept.ParentID == null && dept.Id != frmcompanyCenter.departmentId)
                deptlst.Add(dept);

            }

            CustomerCompRepo customerCompRepo = new CustomerCompRepo();
            List<CustomerCompany> customers = new List<CustomerCompany>();
            customers = customerCompRepo.getAll();
            List<CustomerCompany> customerlist = new List<CustomerCompany>();
            //List<cmbitem> cmbitems = new List<cmbitem>();
            //customer.Id = 0;
            ////customer.company.CompanyName = "<--Add New-->";
            //customerlist.Add(customer);
            foreach (CustomerCompany cust in customers)
            {
                customerlist.Add(cust);
            }
            
            lookupCustomer.ItemsSource = customerlist;

        }
        //public void loadsubcustomers(int id)
        //{

        //    CustomerCompRepo customerCompRepo = new CustomerCompRepo();
        //    List<CustomerCompany> customers = customerCompRepo.getSubsidaries(id);
        //    List<cmbitem> cmbitems = new List<cmbitem>();
            
        //    foreach (CustomerCompany cust in customers)
        //    {
        //        cmbitems.Add(new cmbitem() { name = cust.company.CompanyName, id = cust.Id });
        //    }
        //    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
        //    cmbsubcustomer.ItemsSource = cmbitems;

        //}
        public void loadinquiries()
        {
            Config config = new Config();
            List<string> InquiryType = config.getInquiryType();
            if (InquiryType.Count > 0)
                foreach (string IND in InquiryType)
                {
                    cmbInquiryType.Items.Add(IND);
                }




            //List<cmbitem> cmbitems = new List<cmbitem>();



            

            //cmbitems.Add(new cmbitem() { name = "Tender", id = 1 });
            //cmbitems.Add(new cmbitem() { name = "Supply", id = 2 });
            //cmbitems.Add(new cmbitem() { name = "Principal", id = 3 });



            //cmbInquiryType.ItemsSource = cmbitems;

        }
        public void loademployees()
        {


           EmployeeRepo cont1 = new EmployeeRepo();
            List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            employees = cont1.GetEmployees();

            List<cmbitem> cmbitems = new List<cmbitem>();
          


            foreach (ERP_BL.Databases.Employee employee in employees)
            {

                cmbitems.Add(new cmbitem() { name = employee.person.FName+" "+employee.person.LName, id = employee.EmpId });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbEmployee.ItemsSource = cmbitems;

        }
        public void loaddepartments()
        {
            
            DepartmentRepo cont = new DepartmentRepo();
            List<ERP_BL.Databases.Department> deptRepo = new List<ERP_BL.Databases.Department>();
            deptRepo = cont.GetDepartments();
            List<Department> deptlst = new List<Department>();
            foreach (Department dept in deptRepo)
            {
                //if (dept.IsSubsidary == false && dept.ParentID == null && dept.Id != frmcompanyCenter.departmentId)
                    deptlst.Add(dept);

            }
            if(company!=null)
            lookupDepartment.ItemsSource = company.departments;

            //DepartmentRepo cont1 = new DepartmentRepo();
            //List<Department> departments = new List<Department>();
            //departments = cont1.GetDepartments();

            //List<cmbitem> cmbitems = new List<cmbitem>();
            


            //foreach (Department dept in departments)
            //{
                
            //        cmbitems.Add(new cmbitem() { name = dept.DeptName, id = dept.Id });
                

            //}
            //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            //cmbDepartment.ItemsSource = cmbitems;

        }

        //public void loadsections( int id)
        //{
        //    //loads all sections
        //    //DepartmentRepo cont1 = new DepartmentRepo();
        //    //List<ERP_BL.Databases.Section> secRepo = new List<ERP_BL.Databases.Section>();
        //    //secRepo = cont1.GetSections();
        //    //List<cmbitem> items = new List<cmbitem>();
        //    //foreach (Section section in secRepo)
        //    //{
        //    //    items.Add(new cmbitem() { name = section.SectionName, id = section.Id });
        //    //}


        //    if((cmbDepartment.SelectedItem as cmbitem)!=null)


        //   {    // load only mapped sections on the base of selected department
        //        DepartmentRepo cont1 = new DepartmentRepo();
        //        Department department = new Department();
        //        department = cont1.GetDepartment(id);

        //        List<cmbitem> cmbitems = new List<cmbitem>();


        //        List<Department> departments = cont1.getSubsidaries(id);


        //        foreach (Department dept in departments)
        //        {
        //            cmbitems.Add(new cmbitem() { name = dept.DeptName, id = dept.Id });
        //        }


        //        cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
        //        cmbDepartment.ItemsSource = cmbitems;
        //    }
        //}

        //private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if ((cmbDepartment.SelectedItem as cmbitem) != null)
        //    {
        //        int idd = (cmbDepartment.SelectedItem as cmbitem).id;
        //        if (idd == 0)
        //        {
        //            Company.frmDepartmentAdd departmentAdd = new Company.frmDepartmentAdd();
        //            departmentAdd.ShowDialog();
        //            loaddepartments();
        //        }
        //        else
        //            loadsections(idd);
        //    }
        //}

        //private void cmbCustomerName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if ((cmbCustomerName.SelectedItem as cmbitem) != null)
        //    {
        //        int idd = (cmbCustomerName.SelectedItem as cmbitem).id;
        //        if (idd == 0)
        //        {
        //            Customerss.frmCustomeradd customeradd = new Customerss.frmCustomeradd();
        //            customeradd.ShowDialog();
        //            loadcustomers();
        //        }
        //        else
        //            loadsubcustomers(idd);
        //    }
        //}
        ProductRepo productRepo = new ProductRepo();
        List<InquiryProduct> inqueryItems = new List<InquiryProduct>();
        InquiryProduct inqueryit = new InquiryProduct();
        public void getformdata()
        {
            //DepartmentRepo cont1 = new DepartmentRepo();

            //EmployeeRepo employeeRepo = new EmployeeRepo();
            //CustomerCompRepo customerCompRepo = new CustomerCompRepo();



            //if ((cmbDepartment.SelectedItem as cmbitem) != null)
            //{

            //    department = cont1.GetDepartment((cmbDepartment.SelectedItem as cmbitem).id);
            //}
            //if ((cmbSection.SelectedItem as cmbitem) != null)
            //{

            //    section = cont1.GetSection((cmbSection.SelectedItem as cmbitem).id);
            //}
            //if ((cmbCustomerName.SelectedItem as cmbitem) != null)
            //{
            //    if ((cmbsubcustomer.SelectedItem as cmbitem) != null)
            //    {

            //        customer = customerCompRepo.get((cmbsubcustomer.SelectedItem as cmbitem).id);
            //    }
            //    else
            //    {
            //        customer = customerCompRepo.get((cmbCustomerName.SelectedItem as cmbitem).id);
            //    }
            //}
            //if ((cmbEmployee.SelectedItem as cmbitem) != null)
            //{

            //    employee = employeeRepo.GetEmployee((cmbEmployee.SelectedItem as cmbitem).id);
            //}
            //if (MainWindow.currentUserid != 0)
            //{
            //    user = employeeRepo.getuser(MainWindow.currentUserid);


            //}


            for (int i = 1; i < dGitems.Items.Count; i++)
            {
                datagriditem item = dGitems.Items[i - 1] as datagriditem;
                {
                    
                
                    
                    
                    inqueryit.product.item = item.Item_Discription;
                    inqueryit.product.itemDescription = item.Own_Description;
                   // inqueryit. = item.Quantity;
                    inqueryit.UOM = item.UOM;
                    inqueryit.product.ownDescription = item.Own_Description;
                    inqueryit.quantity = item.Quantity;
                    productRepo.Add(inqueryit);
                    inqueryItems.Add(inqueryit);
                    
                }
            }


        }
        private void btniquiryadd_Click(object sender, RoutedEventArgs e)
        {
            getformdata();
            

            Inquiry inquiry = new Inquiry();
            
                inquiry.alertDate = datalertDate.DateTime;
                inquiry.inquiryDate = datinquirydate.DateTime;
                inquiry.DeliveryDueDate = datduedate.DateTime;
            inquiry.lastSubmissionDate = datclosedate.DateTime;
                inquiry.SalesReferenceNo = txtfileref.Text.Trim();
                inquiry.referenceNo = txtinquiryref.Text.Trim();
            inquiry.user_Id = MainWindow.currentUserid;
            inquiry.allocation_Id = (cmbEmployee.SelectedItem as cmbitem).id;
            inquiry.inquirytype = (InquiryType)cmbInquiryType.SelectedIndex;
            if ((cmbInquiryStatus.SelectedItem as cmbitem) != null)
            {

                InquiryStatus status = inquiryRepo.getstatus((cmbInquiryStatus.SelectedItem as cmbitem).id);
                inquiry.inquiryStatus = status;
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
            inquiry.products =inqueryItems;



            inquiryRepo.Add(inquiry);
            MessageBox.Show("Inquiry Added Succesfully");
        }
        public void loadInquiryStatus()
        {
            
            List<InquiryStatus> inquiryStatuses = new List<InquiryStatus>();
            inquiryStatuses = inquiryRepo.getAllInquiryStatus();

            List<cmbitem> cmbitems = new List<cmbitem>();
           


            foreach (InquiryStatus status in inquiryStatuses)
            {

                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbInquiryStatus.ItemsSource = cmbitems;
        }

        private void cmbInquiryStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbInquiryStatus.SelectedItem as cmbitem)!=null)
                {
                int idd = (cmbInquiryStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    Procurementss.Inquiriess.frmInquiryStatusAdd statusAdd = new Procurementss.Inquiriess.frmInquiryStatusAdd();
                    statusAdd.ShowDialog();
                    loadInquiryStatus();
                }



            }

        }

        //private void cmbsubcustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if ((cmbsubcustomer.SelectedItem as cmbitem) != null)
        //    {
        //        int idd = (cmbsubcustomer.SelectedItem as cmbitem).id;
        //        if (idd == 0)
        //        {
        //            Customerss.frmCustomeradd customeradd = new Customerss.frmCustomeradd();
        //            customeradd.ShowDialog();
        //            loadcustomers();
                       
        //        }



        //    }
        //}

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
                lookupCustomer.EditValue = selectedcust;


            }
        }

        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company!= null)
            {
                string selecteddept = company.CompanyName ;
                lookupCompany.EditValue = selecteddept;

                loaddepartments();
            }

        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                string selecteddept = department.DeptName + " (" + department.Code + ")";
                lookupDepartment.EditValue = selecteddept;


            }
        }

        private void tableView1_InitNewRow(object sender, InitNewRowEventArgs e)
        {
           
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
        private void dGitems_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Item_Name")
            {
                var cb = new DataGridComboBoxColumn();
                cb.Header = "Item";
                //products = productrepo.getAll();
                //List<cmbitem> cmbitems = new List<cmbitem>();
                //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
                //foreach (Product prod in products)
                //{

                //    cmbitems.Add(new cmbitem() { name = prod.item, id = prod.Id });


                //}
                cb.ItemsSource = loadproducts();
                cb.DisplayMemberPath = "name";
                cb.SelectedValuePath = "id";/*new List<string> { "C50", "C40", "C30" };*/
                cb.SelectedValueBinding = new Binding("name");
                var style = new Style(typeof(ComboBox));
                style.Setters.Add(new EventSetter(ComboBox.SelectionChangedEvent, new SelectionChangedEventHandler(ComboBox_SelectionChanged)));
                cb.EditingElementStyle = style;
                e.Column = cb;

            }
        }
        
        ProductRepo productrepo = new ProductRepo();
        Product product = new Product();
        List<Product> products = new List<Product>();
        public List<cmbitem> loadproducts()
        {



            products = productrepo.getAll();
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


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            var cell = sender as DataGridCell;
            //int col = cell.Column.DisplayIndex;
            //cell = VisualTreeHelper.GetParent(cell);
            if ((cb.SelectedItem as cmbitem) != null)
            {
                int idd = (cb.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmItem frmItem = new frmItem();
                    frmItem.ShowDialog();
                    cb.ItemsSource = loadproducts();//loademployees();


                }
                else
                {
                    product = productrepo.get(idd);

                    datagriditem item = dGitems.CurrentItem as datagriditem;
                    
                    DataGridRow row = dGitems.CurrentItem as DataGridRow;
                    item.Item_Name =product.Id +" "+ product.item;
                    item.Item_Discription = product.itemDescription;
                    item.Own_Description = product.ownDescription;

                    //row.Item = item;
                    //row.UpdateDefaultStyle();
                }



            }
        }
    }
}
