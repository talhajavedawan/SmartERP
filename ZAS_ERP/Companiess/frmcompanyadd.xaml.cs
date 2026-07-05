using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ERP_BL;
using ERP_BL.Databases;
using ERP_BL.Config;
using ERP_BL.Enums;
using System.ComponentModel;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Core;

namespace ZAS_ERP.Companiess
{
    /// <summary>
    /// Interaction logic for frmcompanyadd.xaml
    /// </summary>
    public partial class frmcompanyadd : System.Windows.Window
    {
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Config config = new Config();
        CompanyRepo repo = new CompanyRepo();    
        public string onlychar { get; set; }
        public string onlynum { get; set; }
        public string required { get; set; }
        public string nospecchar { get; set; }
        List<Department> AllDepartments = new List<Department>();
        List<Department> selectedDepartments = new List<Department>();
        List<ERP_BL.Databases.Employee> AllEmployees = new List<ERP_BL.Databases.Employee>();
        List<ERP_BL.Databases.Employee> SelectedEmployees = new List<ERP_BL.Databases.Employee>(); 
        List<ERP_BL.Databases.Employee> AlladminBillEmployees = new List<ERP_BL.Databases.Employee>();
        List<ERP_BL.Databases.Employee> SelectedadminBillEmployees = new List<ERP_BL.Databases.Employee>();

        List<ERP_BL.Databases.Employee> AllTaskEmployees = new List<ERP_BL.Databases.Employee>();
        List<ERP_BL.Databases.Employee> SelectedTaskEmployees = new List<ERP_BL.Databases.Employee>();

        List<ERP_BL.Databases.Employee> AllPettyCashEmployees = new List<ERP_BL.Databases.Employee>();
        List<ERP_BL.Databases.Employee> SelectedPettyCashEmployees = new List<ERP_BL.Databases.Employee>();

        List<CustomerCompany> AllcustomerCompanies = new List<CustomerCompany>();
        List<CustomerCompany> SelectedcustomerCompanies = new List<CustomerCompany>();
        public frmcompanyadd()
        {
            InitializeComponent();

            List<string> Biztype = config.getBizType();

            if (Biztype.Count > 0)
                foreach (string Biz in Biztype)
                {
                    cmbBussinesType.Items.Add(Biz);
                }

        }
        public void loadIndustryTypes()
        {
            List<IndustryType> industryTypes = new List<IndustryType>();
            //  employees = cont1.GetEmployees();
            industryTypes = repo.GetIndustryTypes();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (IndustryType industry in industryTypes)
            {

                cmbitems.Add(new cmbitem() { name = industry.name, id = industry.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbIndustry.ItemsSource = cmbitems;

        }
        private void loadCurrencies()
        {

            CurrencyRepo currencyRepo = new CurrencyRepo();
            List<Currency> currencies = currencyRepo.getAll();
            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            foreach (Currency cur in currencies)
            {

                //cmbCurrency.Items.Add(new cmbitem() { name = cur.CurrencyName, id = cur.Id });
                cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
            }

            cmbCurrency.ItemsSource = cmbitems;

        }
        private void loaddeptdata()
        {
            AllDepartments = repo.GetDepartments();
            this.gridDepartment.ItemsSource = AllDepartments;
            gridDepartment.SelectedItems.Clear();
        }
        private void LoadCustomers()
        {
            AllcustomerCompanies = (List<CustomerCompany>)repo.GetAllCustomers();
            grdCustomers.ItemsSource = AllcustomerCompanies;
        }
        private void LoadEmployees()
        {
            AllEmployees  = (List<ERP_BL.Databases.Employee>)repo.GetAllEmployees();
            //AlladminBillEmployees = AllEmployees;

            grdEmployee.ItemsSource = AllEmployees;
           // grdAdminBillEmployee.ItemsSource = AlladminBillEmployees;
        }
        private void loadAdminBillEmployee()
        {
            AlladminBillEmployees = repo.getAdminBillEmployee();
            grdAdminBillEmployee.ItemsSource = AlladminBillEmployees;
        }

        private void LoadTaskEmployee()
        {
            AllTaskEmployees = (List<ERP_BL.Databases.Employee>)repo.GetAllEmployees();
            grdTaskEmployee.ItemsSource = AllTaskEmployees;
        }

        private void LoadPettyCashEmployee()
        {
            AllPettyCashEmployees = (List<ERP_BL.Databases.Employee>)repo.GetAllEmployees();
            grdPettyCashEmployee.ItemsSource = AllPettyCashEmployees;
        }


        private void btnSaveCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBussinesName.Text == "")
                {
                    MessageBox.Show("Please Enter Bussines Name", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtBussinesName.Focus();
                    return;
                }
                else if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Companies Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbCurrency.Focus();
                    return;
                }
                else if (txtEIN.Text == "")
                {
                    txtEIN.Text = "0";
                  
                }

                // get an object of Group company for mapping as a parent
                ERP_BL.Databases.Company comp = repo.GetGroupCompany();

    
                if (selectedDepartments.Count != 0)
                {
                    company.departments = new List<Department>(); 
                    foreach (var _dept in selectedDepartments)
                    {
                        if (!company.departments.Contains(_dept))
                        {
                            company.departments.Add(_dept); 
                        }
                    }
                }
                else
                {
                    company.departments = new List<Department>();
                }

                //Employee save

                if (SelectedEmployees.Count != 0)
                {
                    company.employees = new List<ERP_BL.Databases.Employee>();
                    foreach (var _dept in SelectedEmployees)
                    {
                        if (!company.employees.Contains(_dept))
                        {
                            company.employees.Add(_dept);
                        }
                    }
                }



                //if (grdEmployee.SelectedItems.Count != 0)
                //{
                //    company.employees = new List<ERP_BL.Databases.Employee>();
                //    foreach (ERP_BL.Databases.Employee Emp in grdEmployee.SelectedItems)
                //    {

                //        if (!company.employees.Contains(Emp))
                //        {
                //            company.employees.Add(Emp);
                //        }
                //        //company.departments.Add(dept);
                //    }

                //}

                if(chkLinkable.IsChecked==true)
                {
                    company.isLinkable = true;
                }
                else
                {
                    company.isLinkable = false;
                }

                company.isActive = (chkisActive.IsChecked == true) ? true : false;

                // company information common for editing and new company
                company.CompanyName = txtBussinesName.Text.Trim();
                company.BizType = (BizTypes)cmbBussinesType.SelectedIndex;
                company.EmployeerNo = txtEIN.Text.Trim();
                company.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                company.CurrencyId = (cmbCurrency.SelectedItem as cmbitem).id;


                //Customer company save

                if (SelectedcustomerCompanies.Count != 0)
                {
                    company.Customers = new List<CustomerCompany>();
                    foreach (var _dept in SelectedcustomerCompanies)
                    {
                        if (!company.Customers.Contains(_dept))
                        {
                            company.Customers.Add(_dept);
                        }
                    }
                }



                //if (customerCompanies.Count != 0)
                //{
                //    company.Customers= new List<CustomerCompany>();

                //    foreach (CustomerCompany _cust in customerCompanies)
                //    {
                //        if (!company.Customers.Contains(_cust))
                //        {
                //            company.Customers.Add(_cust);
                //        }
                //    }

                //}

                //Admin Employee save

                if (SelectedadminBillEmployees.Count != 0)
                {
                    company.AdminBillEmployees = new List<ERP_BL.Databases.Employee>();
                    foreach (var _dept in SelectedadminBillEmployees)
                    {
                        if (!company.AdminBillEmployees.Contains(_dept))
                        {
                            company.AdminBillEmployees.Add(_dept);
                        }
                    }
                }


                //Task Employees
                if (SelectedTaskEmployees.Count != 0)
                {
                    company.TaskEmployees = new List<ERP_BL.Databases.Employee>();
                    foreach (var _dept in SelectedTaskEmployees)
                    {
                        if (!company.TaskEmployees.Contains(_dept))
                        {
                            company.TaskEmployees.Add(_dept);
                        }
                    }
                }

                //Petty cash Employees
                if (SelectedPettyCashEmployees.Count != 0)
                {
                    company.PettyCashEmployees = new List<ERP_BL.Databases.Employee>();
                    foreach (var _emp in SelectedPettyCashEmployees)
                    {
                        if (!company.PettyCashEmployees.Contains(_emp))
                        {
                            company.PettyCashEmployees.Add(_emp);
                        }
                    }
                }

                //if (adminBillEmployees.Count != 0)
                //{
                //    company.AdminBillEmployees = new List<ERP_BL.Databases.Employee>();

                //    foreach (ERP_BL.Databases.Employee Emp in adminBillEmployees)
                //    {
                //        if (!company.AdminBillEmployees.Contains(Emp))
                //        {
                //            company.AdminBillEmployees.Add(Emp);
                //        }
                //    }

                //}


                // update Company in case a company is being Edited
                if (frmcompanyCenter.Editit == 1)
                {
                    // set company parent
                    if (company.compnayType != CompnayTypes.Group)
                    {
                        company.compnayType = CompnayTypes.Company;
                        company.ParentID = comp.Id;
                    }

                    //Company Adress 
                    company.address.Country = txtCountry.Text.Trim();
                    company.address.Line1 = txtAdressline1.Text.Trim();
                    company.address.Line2 = txtAdressline2.Text.Trim();
                    company.address.State = txtState.Text.Trim();
                    company.address.City = txtCity.Text.Trim();
                    company.address.Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                    //company Contact to be updated
                    company.contact.ContactNo = txtphone.Text.Trim();
                    company.contact.Fax = txtFax.Text.Trim();
                    company.contact.Email = txtEmail.Text.Trim();
                    company.contact.SMLink1 = txtLink1.Text.Trim();
                    company.contact.SMLink2 = txtLink2.Text.Trim();
                    company.contact.SMLink3 = txtLink3.Text.Trim();
                    company.contact.Website = txtWebsite.Text.Trim();

  
                    repo.updateCompany(company);
                }
                else
                {
                    Address add = new Address()

                    {

                        Country = txtCountry.Text.Trim(),
                        Line1 = txtAdressline1.Text.Trim(),
                        Line2 = txtAdressline2.Text.Trim(),
                        State = txtState.Text.Trim(),
                        City = txtCity.Text.Trim(),
                        Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim())
                    };
                    company.address = add;
                    //New Contact object for company's contact
                    Contact contact = new Contact()
                    {
                        ContactNo = txtphone.Text.Trim(),
                        Fax = txtFax.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        SMLink1 = txtLink1.Text.Trim(),
                        SMLink2 = txtLink2.Text.Trim(),
                        SMLink3 = txtLink3.Text.Trim(),
                        Website = txtWebsite.Text.Trim()
                    };
                    company.contact = contact;
                    // other info about comapny 
                    if(company.ParentID!=null)
                    company.ParentID = comp.Id;
                    company.openingDate = System.DateTime.Now;
                    company.closingDate = null;
                    company.compnayType = CompnayTypes.Company;

                    repo.addCompany(company);
                   
                }

                MessageBox.Show("Changes Saved");
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //this.Close();
            }

        }
        private void wincompanyAdd_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //SystemLogic.SetUserSettingOfCurrentWindow(gridDepartment);
                //working
                chkisActive.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark Company as  InActive") != null) ? true : false;

                loadIndustryTypes();
                loadCurrencies();
                loaddeptdata();
                LoadCustomers();
                LoadEmployees();
                loadAdminBillEmployee();
                LoadTaskEmployee();
                LoadPettyCashEmployee();
                if (frmcompanyCenter.Editit == 1)
                {
                    wincompanyAdd.Title = "Edit Company";


                    chkisActive.IsChecked = company.isActive;
                    company = repo.GetCompany(frmcompanyCenter.companyId);

                    //string fullName = "";
                    //var node = company;
                    //while (node != null)
                    //{
                    //    if (fullName.Length != 0)
                    //        fullName = " | " + fullName;
                    //    fullName = node.CompanyName + fullName;

                    //    node = node.parentCompany;
                    //}
                    lblCompanyName.Text = company.CompanyName;
                    if(company.isLinkable==true)
                    {
                        chkLinkable.IsChecked = company.isLinkable;
                    }

                    txtBussinesName.Text = company.CompanyName;
                    company.address = repo.GetAddresses(company.addressId);
                    //company.BizType = (BizTypes)cmbBussinesType.SelectedIndex;
                    cmbBussinesType.SelectedIndex = Convert.ToInt32(company.BizType);
                    txtEIN.Text = company.EmployeerNo;
                    //contactid =(int) company.address.Id;
                    //addid = frmcompanyCenter.addId;
                    txtCountry.Text = company.address.Country;
                    txtAdressline1.Text = company.address.Line1;
                    txtAdressline2.Text = company.address.Line2;
                    txtState.Text = company.address.State;
                    txtZIP.Text = company.address.Zip.ToString();
                    company.contact = repo.GetContacts(company.contactId);
                    txtphone.Text = company.contact.ContactNo;
                    txtFax.Text = company.contact.Fax;
                    txtEmail.Text = company.contact.Email;
                    txtLink1.Text = company.contact.SMLink1;
                    txtLink2.Text = company.contact.SMLink2;
                    txtLink3.Text = company.contact.SMLink3;
                    txtWebsite.Text = company.contact.Website;
                    foreach (cmbitem cmbitem in cmbIndustry.Items)
                    {
                        if (cmbitem.id == company.industryTypeId)
                            cmbIndustry.SelectedItem = cmbitem;
                    }
                    cmbCurrency.IsEnabled = false;
                    foreach (cmbitem cmbitem in cmbCurrency.Items)
                    {
                        if (cmbitem.id == company.CurrencyId)
                            cmbCurrency.SelectedItem = cmbitem;
                    }
                   
                    //Start fetching department

                    foreach (var _selectedDept in company.departments)
                    {
                        selectedDepartments.Add(_selectedDept);
                       
                    }
                    gridSelectedDepartment.ItemsSource = null;
                    gridSelectedDepartment.ItemsSource = selectedDepartments;

                    foreach (var rrr in selectedDepartments.ToList())
                    {
                        if (AllDepartments.Contains(rrr))
                        {
                            AllDepartments.Remove(rrr);

                            var findParet = AllDepartments.Find(x => x.ParentID == rrr.Id);
                            if (findParet != null)
                            {

                                //if(!allCustomers.Contains(findParet))
                                AllDepartments.Add(rrr);
                            }

                        }
                    }
                    gridDepartment.ItemsSource = null;
                    gridDepartment.ItemsSource = AllDepartments;

                    //End Fetching Deparments

                    //employee start
                    foreach (var _selectedEmp in company.employees)
                    {
                        SelectedEmployees.Add(_selectedEmp);

                    }
                    grdEmployeeSelected.ItemsSource = null;
                    grdEmployeeSelected.ItemsSource = SelectedEmployees;

                    foreach (var rrr in SelectedEmployees)
                    {
                        if (AllEmployees.Contains(rrr))
                        {
                            AllEmployees.Remove(rrr);
                        }
                    }
                    grdEmployee.ItemsSource = null;
                    grdEmployee.ItemsSource = AllEmployees;

                    //AdminEmployee start
                    foreach (var _selectedEmp in company.AdminBillEmployees)
                    {
                        SelectedadminBillEmployees.Add(_selectedEmp);

                    }
                    grdAdminBillEmployeeSelected.ItemsSource = null;
                    grdAdminBillEmployeeSelected.ItemsSource = SelectedadminBillEmployees;

                    foreach (var rrr in SelectedadminBillEmployees)
                    {
                        if (AlladminBillEmployees.Contains(rrr))
                        {
                            AlladminBillEmployees.Remove(rrr);
                        }
                    }
                    grdAdminBillEmployee.ItemsSource = null;
                    grdAdminBillEmployee.ItemsSource = AlladminBillEmployees;



                    //Task Employees
                    foreach (var _selectedEmp in company.TaskEmployees)
                    {
                        SelectedTaskEmployees.Add(_selectedEmp);

                    }
                    grdTaskEmployeeSelected.ItemsSource = null;
                    grdTaskEmployeeSelected.ItemsSource = SelectedTaskEmployees;

                    foreach (var rrr in SelectedTaskEmployees)
                    {
                        if (AllTaskEmployees.Contains(rrr))
                        {
                            AllTaskEmployees.Remove(rrr);
                        }
                    }
                    grdTaskEmployee.ItemsSource = null;
                    grdTaskEmployee.ItemsSource = AllTaskEmployees;

                    //Petty cash Employees
                    foreach (var _selectedEmp in company.PettyCashEmployees)
                    {
                        SelectedPettyCashEmployees.Add(_selectedEmp);

                    }
                    grdPettyCashEmployeeSelected.ItemsSource = null;
                    grdPettyCashEmployeeSelected.ItemsSource = SelectedPettyCashEmployees;

                    foreach (var rrr in SelectedPettyCashEmployees)
                    {
                        if (AllPettyCashEmployees.Contains(rrr))
                        {
                            AllPettyCashEmployees.Remove(rrr);
                        }
                    }
                    grdPettyCashEmployee.ItemsSource = null;
                    grdPettyCashEmployee.ItemsSource = AllPettyCashEmployees;


                    
                    //Customer start

                    foreach (var _selectedEmp in company.Customers)
                    {
                        SelectedcustomerCompanies.Add(_selectedEmp);

                    }
                    grdCustomersSelected.ItemsSource = null;
                    grdCustomersSelected.ItemsSource = SelectedcustomerCompanies;

                    foreach (var rrr in SelectedcustomerCompanies)
                    {
                        if (AllcustomerCompanies.Contains(rrr))
                        {
                            AllcustomerCompanies.Remove(rrr);
                        }
                    }
                    grdCustomers.ItemsSource = null;
                    grdCustomers.ItemsSource = AllcustomerCompanies;


                 

                    txtBussinesName.Focus();

                }
                else
                {
                    gridSelectedDepartment.ItemsSource = selectedDepartments;
                    grdEmployeeSelected.ItemsSource = SelectedEmployees;
                    grdAdminBillEmployeeSelected.ItemsSource = SelectedadminBillEmployees;
                    grdCustomersSelected.ItemsSource = SelectedcustomerCompanies;
                    grdTaskEmployeeSelected.ItemsSource = SelectedTaskEmployees;
                    grdPettyCashEmployeeSelected.ItemsSource = SelectedPettyCashEmployees;
                    lblCompanyName.Text = "Add New Company";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void cmbCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            // cmbCurrency.DisplayMemberPath = (cmbCurrency.SelectedItem as cmbitem).name;
            if (cmbCurrency.SelectedItem as cmbitem != null)
            {
                int idd = (cmbCurrency.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                    frmCurrency.ShowDialog();
                    loadCurrencies();
                }

                txtAdressline1.Focus();


            }
        }
        private void cmbIndustry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbIndustry.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbIndustry.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmIndustryTypeAdd industryadd = new frmIndustryTypeAdd();
                    industryadd.ShowDialog();
                    loadIndustryTypes();

                }



            }
        }
        private void WincompanyAdd_Unloaded(object sender, RoutedEventArgs e)
        {
            //SystemLogic.SaveUserSettingForCurrentWindow(gridDepartment);
        }
        private void btnRightMove_Click(object sender, RoutedEventArgs e)
        {
            int hasParent = 0;
            try
            {
                var selectedItem = gridDepartment.SelectedItem as Department;
                if (selectedItem != null)
                {
                    var child = repo.getParent(selectedItem.Id);

                    if (child != null)
                    {
                        DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                        return;
                    }
                    var node = selectedItem.parentDepartment;
                    while (node != null)
                    {
                        hasParent = 1;
                        AllDepartments.Remove(selectedItem);
                        if (!selectedDepartments.Contains(selectedItem))
                            selectedDepartments.Add(selectedItem);
                        if (!selectedDepartments.Contains(node))
                            selectedDepartments.Add(node);
                        var findParet = AllDepartments.Find(x => x.parentDepartment == node);
                        if (findParet == null)
                        {
                            AllDepartments.Remove(node);
                        }
                        node = node.parentDepartment;
                    }
                    if (hasParent == 0)
                    {
                        AllDepartments.Remove(selectedItem);
                        if (!selectedDepartments.Contains(selectedItem))
                            selectedDepartments.Add(selectedItem);
                    }
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Department List Register!");
                }
                gridSelectedDepartment.RefreshData();
                gridDepartment.RefreshData();
                gridDepartment.SelectedItem = null;
                gridSelectedDepartment.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnLeftMove_Click(object sender, RoutedEventArgs e)
        {
            int parent = 0;
            try
            {
                var selectedItem = gridSelectedDepartment.SelectedItem as Department;

                if (selectedItem != null)
                {
                    var child = repo.getParent(selectedItem.Id);

                    if (child != null)
                    {
                        DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                        return;
                    }
                    var node = selectedItem.parentDepartment;
                    while (node != null)
                    {
                        parent = 1;
                        if (!AllDepartments.Contains(selectedItem))
                            AllDepartments.Add(selectedItem);
                        selectedDepartments.Remove(selectedItem);
                        if (!AllDepartments.Contains(node))
                            AllDepartments.Add(node);
                        var findParet = selectedDepartments.Find(x => x.parentDepartment == node);
                        if (findParet == null)
                        {
                            selectedDepartments.Remove(node);
                        }
                        node = node.parentDepartment;
                    }
                    if (parent == 0)
                    {
                        if (!AllDepartments.Contains(selectedItem))
                            AllDepartments.Add(selectedItem);
                        selectedDepartments.Remove(selectedItem);
                    }     
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Department Register!");
                }
                gridSelectedDepartment.RefreshData();
                gridDepartment.RefreshData();
                gridDepartment.SelectedItem = null;
                gridSelectedDepartment.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);

            }
        }
        private void btnRightMoveEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdEmployee.SelectedItem as ERP_BL.Databases.Employee;

                if (selectedItem != null)
                {

                    AllEmployees.Remove(selectedItem);
                    if (!SelectedEmployees.Contains(selectedItem))
                        SelectedEmployees.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                 grdEmployeeSelected.RefreshData();
                grdEmployee.RefreshData();
                grdEmployee.SelectedItem = null;
                grdEmployeeSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }
        private void btnLeftMoveEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdEmployeeSelected.SelectedItem as ERP_BL.Databases.Employee;

                if (selectedItem != null)
                {
                    if (!AllEmployees.Contains(selectedItem))
                        AllEmployees.Add(selectedItem);
                    SelectedEmployees.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdEmployeeSelected.RefreshData();
                grdEmployee.RefreshData();
                grdEmployee.SelectedItem = null;
                grdEmployeeSelected.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }
        private void btnRightMoveAdminBillEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdAdminBillEmployee.SelectedItem as ERP_BL.Databases.Employee;

                if (selectedItem != null)
                {

                    AlladminBillEmployees.Remove(selectedItem);
                    if (!SelectedadminBillEmployees.Contains(selectedItem))
                        SelectedadminBillEmployees.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                grdAdminBillEmployeeSelected.RefreshData();
                grdAdminBillEmployee.RefreshData();
                grdAdminBillEmployee.SelectedItem = null;
                grdAdminBillEmployeeSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }
        private void btnLeftMoveAdminBillEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdAdminBillEmployeeSelected.SelectedItem as ERP_BL.Databases.Employee;

                if (selectedItem != null)
                {
                    if (!AlladminBillEmployees.Contains(selectedItem))
                        AlladminBillEmployees.Add(selectedItem);
                    SelectedadminBillEmployees.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdAdminBillEmployeeSelected.RefreshData();
                grdAdminBillEmployee.RefreshData();
                grdAdminBillEmployee.SelectedItem = null;
                grdAdminBillEmployeeSelected.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }
        private void btnRightMoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdCustomers.SelectedItem as CustomerCompany;

                if (selectedItem != null)
                {

                    AllcustomerCompanies.Remove(selectedItem);
                    if (!SelectedcustomerCompanies.Contains(selectedItem))
                        SelectedcustomerCompanies.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                grdCustomersSelected.RefreshData();
                grdCustomers.RefreshData();
                grdCustomers.SelectedItem = null;
                grdCustomersSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }
        private void btnLeftMoveCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdCustomersSelected.SelectedItem as CustomerCompany;

                if (selectedItem != null)
                {
                    if (!AllcustomerCompanies.Contains(selectedItem))
                        AllcustomerCompanies.Add(selectedItem);
                    SelectedcustomerCompanies.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdCustomersSelected.RefreshData();
                grdCustomers.RefreshData();
                grdCustomers.SelectedItem = null;
                grdCustomersSelected.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }

        private void btnLeftMoveCustomer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 25;
            imgRightToLeft.Height = 25;
        }

        private void btnLeftMoveCustomer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 32;
            imgRightToLeft.Height = 32;

            try
            {
                var selectedItem = grdCustomersSelected.SelectedItem as CustomerCompany;

                if (selectedItem != null)
                {
                    if (!AllcustomerCompanies.Contains(selectedItem))
                        AllcustomerCompanies.Add(selectedItem);
                    SelectedcustomerCompanies.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdCustomersSelected.RefreshData();
                grdCustomers.RefreshData();
                grdCustomers.SelectedItem = null;
                grdCustomersSelected.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }

        private void imgLeftToRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Height = 25;
            imgLeftToRight.Width = 25;
        }

        private void imgLeftToRight_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Height = 32;
            imgLeftToRight.Width = 32;
            try
            {
                var selectedItem = grdCustomers.SelectedItem as CustomerCompany;

                if (selectedItem != null)
                {

                    AllcustomerCompanies.Remove(selectedItem);
                    if (!SelectedcustomerCompanies.Contains(selectedItem))
                        SelectedcustomerCompanies.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                grdCustomersSelected.RefreshData();
                grdCustomers.RefreshData();
                grdCustomers.SelectedItem = null;
                grdCustomersSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void BtnRightMoveTaskEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdTaskEmployee.SelectedItem as ERP_BL.Databases.Employee;

                if (selectedItem != null)
                {

                    AllTaskEmployees.Remove(selectedItem);
                    if (!SelectedTaskEmployees.Contains(selectedItem))
                        SelectedTaskEmployees.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                grdTaskEmployeeSelected.RefreshData();
                grdTaskEmployee.RefreshData();
                grdTaskEmployee.SelectedItem = null;
                grdTaskEmployeeSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void BtnLeftMoveTaskEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdTaskEmployeeSelected.SelectedItem as ERP_BL.Databases.Employee;

                if (selectedItem != null)
                {
                    if (!AllTaskEmployees.Contains(selectedItem))
                        AllTaskEmployees.Add(selectedItem);
                    SelectedTaskEmployees.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdTaskEmployeeSelected.RefreshData();
                grdTaskEmployee.RefreshData();
                grdTaskEmployee.SelectedItem = null;
                grdTaskEmployeeSelected.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }

        private void BtnRightMovePettyCashEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdPettyCashEmployee.SelectedItem as ERP_BL.Databases.Employee;

                if (selectedItem != null)
                {

                    AllPettyCashEmployees.Remove(selectedItem);
                    if (!SelectedPettyCashEmployees.Contains(selectedItem))
                        SelectedPettyCashEmployees.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                grdPettyCashEmployeeSelected.RefreshData();
                grdPettyCashEmployee.RefreshData();
                grdPettyCashEmployee.SelectedItem = null;
                grdPettyCashEmployeeSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void BtnLeftMovePettyCashEmp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdPettyCashEmployeeSelected.SelectedItem as ERP_BL.Databases.Employee;

                if (selectedItem != null)
                {
                    if (!AllPettyCashEmployees.Contains(selectedItem))
                        AllPettyCashEmployees.Add(selectedItem);
                    SelectedPettyCashEmployees.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdPettyCashEmployeeSelected.RefreshData();
                grdPettyCashEmployee.RefreshData();
                grdPettyCashEmployee.SelectedItem = null;
                grdPettyCashEmployeeSelected.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }
    }


}

