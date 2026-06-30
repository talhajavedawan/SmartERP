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
using ERP_BL.Databases;
using ERP_BL.Config;
using ERP_BL.Enums;
using DevExpress.Xpf.Grid;
using ZAS_ERP.Validations;
using System.ComponentModel;
using DevExpress.Xpf.Core;

namespace ZAS_ERP.Principalss

{
    
    /// <summary>
    /// Interaction logic for frmprincipaladd.xaml
    /// </summary>
    public partial class frmPrincipaladd : Window
    {
        DepartmentRepo deptRepo = new DepartmentRepo();
        Principal principal = new Principal(); 
             Principal parentcustomer = new Principal(); 
        PrincipalRepo principalrepo = new PrincipalRepo();
        CompanyRepo companyRepo = new CompanyRepo();
        Config config = new Config();
        public string onlychar { get; set; }
        public string onlynum { get; set; }
        public string required { get; set; }
        public string nospecchar { get; set; }

        List<Department> AllDepartments = new List<Department>();
        List<Department> selectedDepartments = new List<Department>();

        public frmPrincipaladd()
        {
            try

            {
                InitializeComponent();
                List<string> INDList = config.getIndustryType();
                List<string> Biztype = config.getBizType();
                //if (INDList.Count > 0)
                //    foreach (string IND in INDList)
                //    {
                //        cmbIndustry.Items.Add(IND);
                //    }
                if (Biztype.Count > 0)
                    foreach (string Biz in Biztype)
                    {
                        cmbBussinesType.Items.Add(Biz);
                    }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
 
        public void loadIndustryTypes()
        {



            List<IndustryType> industryTypes = new List<IndustryType>();
            //  employees = cont1.GetEmployees();
            industryTypes = companyRepo.GetIndustryTypes();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (IndustryType industry in industryTypes)
            {

                cmbitems.Add(new cmbitem() { name = industry.name, id = industry.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbIndustry.ItemsSource = cmbitems;

        }

        private void btnaddPrincipal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txtBussinesName.Text == "")
                {
                    MessageBox.Show("Please Enter Bussines Name", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtBussinesName.Focus();
                }
                else if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Companies Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbCurrency.Focus();
                }
                

                else
                {
                    if (txtEIN.Text == "")
                         txtEIN.Text = "0";


                    if (selectedDepartments.Count != 0)
                    {
                        principal.departments = new List<Department>();
                        foreach (var _dept in selectedDepartments)
                        {
                            if (!principal.departments.Contains(_dept))
                            {
                                principal.departments.Add(_dept);
                            }
                        }
                    }
                    else
                    {
                        principal.departments = new List<Department>();
                    }
                  
                    principal.isActive = (chkisActive.IsChecked == true) ? true : false;
                    if (chkissubsidroy.IsChecked == true && parentcustomer.Id != 0)
                    {
                        principal.IsSubsidary = true;
                        principal.ParentID = parentcustomer.Id;
                    }
                    else
                    {
                        principal.IsSubsidary = false;
                        principal.ParentID = null;
                        principal.parentDepartment = null;
                    }
                    if (frmPrincipalCenter.Editit == 1 || ucPrincipalRegister.Editit == 1)
                    {
                        principal.contactPerson.FName = txtfirstName.Text.Trim();
                        principal.contactPerson.LName = txtlastName.Text.Trim();
                        principal.contactPerson.CNIC = "6110100000000";
                        principal.contactPerson.FatherName = "";
                        principal.contactPerson.Gender = Gender.Male;
                        principal.contactPerson.DOB = Convert.ToDateTime("02-03-2018");
                        principal.contactPerson.NextKin = "NA";



                        principal.company.CompanyName = txtBussinesName.Text.Trim();
                        principal.company.BizType = (BizTypes)cmbBussinesType.SelectedIndex;
                        principal.company.EmployeerNo = txtEIN.Text.Trim();
                        principal.company.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                        principal.company.compnayType = CompnayTypes.PrincipalCompany;
                        principal.company.CurrencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                        principal.company.openingDate = System.DateTime.Now;
                        principal.company.closingDate = null;


                        principal.billingAddres.Country = txtCountry.Text.Trim();
                        principal.billingAddres.Line1 = txtAdressline1.Text.Trim();
                        principal.billingAddres.Line2 = txtAdressline2.Text.Trim();
                        principal.billingAddres.State = txtState.Text.Trim();
                        principal.billingAddres.Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                        principal.billingAddres.City = txtCity.Text.Trim();
                        //principal.billingAddres.region = txtRegionbil.Text.Trim();
                        //principal.billingAddres.addressType = AddressTypes.billingAddress;



                        principal.shippingAddress.Country = txtsCountry.Text.Trim();
                        principal.shippingAddress.Line1 = txtsAdressline1.Text.Trim();
                        principal.shippingAddress.Line2 = txtsAdressline2.Text.Trim();
                        principal.shippingAddress.State = txtsState.Text.Trim();
                        principal.shippingAddress.Zip = string.IsNullOrEmpty(txtsZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                        principal.shippingAddress.City = txtsCity.Text.Trim();
                        //principal.shippingAddress.region = txtRegionship.Text.Trim();

                        //principal.shippingAddress.addressType = AddressTypes.shippingAddress;



                        principal.contact.ContactNo = txtPhonenum.Text.Trim();
                        principal.contact.Fax = txtFaxnum.Text.Trim();
                        principal.contact.Email = txtEmail.Text.Trim();
                        principal.contact.SMLink1 = txtLink1.Text.Trim();
                        principal.contact.SMLink2 = txtLink2.Text.Trim();
                        principal.contact.SMLink3 = txtLink3.Text.Trim();
                        principal.contact.Website = txtWebsite.Text.Trim();

                        principalrepo.Update(principal);
                        MessageBox.Show(principal.company.CompanyName + " Updated Succesfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {

                        Person person = new Person()
                        {
                            FName = txtfirstName.Text.Trim(),
                            LName = txtlastName.Text.Trim(),
                            CNIC = "6110167658959",
                            FatherName = "",
                            Gender = Gender.Male,
                            DOB = Convert.ToDateTime("02-03-2018"),
                            NextKin = "NA"


                        };
                        principal.contactPerson = person;
                        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company()
                        {
                            CompanyName = txtBussinesName.Text.Trim(),
                            BizType = (BizTypes)cmbBussinesType.SelectedIndex,
                            EmployeerNo = txtEIN.Text.Trim(),
                            industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id,
                            compnayType = CompnayTypes.PrincipalCompany,
                            CurrencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                            openingDate = System.DateTime.Now,
                            closingDate = null
                        };
                        principal.company = company;
                        Address billadd = new Address()

                        {

                            Country = txtCountry.Text.Trim(),
                            Line1 = txtAdressline1.Text.Trim(),
                            Line2 = txtAdressline2.Text.Trim(),
                            State = txtState.Text.Trim(),
                            Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim()),
                            City = txtCity.Text.Trim(),
                            addressType = AddressTypes.billingAddress


                        };
                        principal.billingAddres = billadd;
                        Address shipadd = new Address()

                        {
                            Id = billadd.Id + 1,
                            Country = txtsCountry.Text.Trim(),
                            Line1 = txtsAdressline1.Text.Trim(),
                            Line2 = txtsAdressline2.Text.Trim(),
                            State = txtsState.Text.Trim(),
                            Zip = string.IsNullOrEmpty(txtsZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtsZIP.Text.Trim()),
                            City = txtsCity.Text.Trim(),
                            addressType = AddressTypes.shippingAddress

                        };
                        principal.shippingAddress = shipadd;
                        Contact contact = new Contact()
                        {
                            ContactNo = txtPhonenum.Text.Trim(),
                            Fax = txtFaxnum.Text.Trim(),
                            Email = txtEmail.Text.Trim(),
                            SMLink1 = txtLink1.Text.Trim(),
                            SMLink2 = txtLink2.Text.Trim(),
                            SMLink3 = txtLink3.Text.Trim(),
                            Website = txtWebsite.Text.Trim()
                        };
                        principal.contact = contact;

                        principalrepo.Add(principal);
                        MessageBox.Show(principal.company.CompanyName + " Added Succesfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }

                }
            }
            catch (Exception ex)
                {
                MessageBox.Show(ex.ToString());
                }

        }
      
        private void loadcompany()
        {

            try
            {

            
            
            //get company by id
            if(frmPrincipalCenter.principalid != 0)
                principal = principalrepo.get(frmPrincipalCenter.principalid);
                else if (ucPrincipalRegister.principalid != 0)
                    principal = principalrepo.get(ucPrincipalRegister.principalid);
                // company info
                txtBussinesName.Text = principal.company.CompanyName;
                foreach (cmbitem cmbitem in cmbIndustry.Items)
                {
                    if (cmbitem.id == principal.company.industryTypeId)
                        cmbIndustry.SelectedItem = cmbitem;
                }
                //cmbIndustry.SelectedIndex = Convert.ToInt32(principal.company.IndustryType);
                cmbBussinesType.SelectedIndex = Convert.ToInt32(principal.company.BizType);
                txtEIN.Text = principal.company.EmployeerNo;
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == principal.company.CurrencyId)
                        cmbCurrency.SelectedItem = cmbitem;
                }
                //contact person
                txtfirstName.Text = principal.contactPerson.FName;
                txtlastName.Text = principal.contactPerson.LName;
                chkisActive.IsChecked = principal.isActive;
                //Billing adress
                txtCountry.Text = principal.billingAddres.Country;
                txtCity.Text = principal.billingAddres.City;
                txtAdressline1.Text = principal.billingAddres.Line1;
                txtAdressline2.Text = principal.billingAddres.Line2;
                txtState.Text = principal.billingAddres.State;
                txtZIP.Text = principal.billingAddres.Zip.ToString();
                //shiping adress
                txtsCountry.Text = principal.shippingAddress.Country;
                txtsCity.Text = principal.shippingAddress.City;
                txtsAdressline1.Text = principal.shippingAddress.Line1;
                txtsAdressline2.Text = principal.shippingAddress.Line2;
                txtsState.Text = principal.shippingAddress.State;
                txtsZIP.Text = principal.shippingAddress.Zip.ToString();
                //contact details
                txtPhonenum.Text = principal.contact.ContactNo;
                txtFaxnum.Text = principal.contact.Fax;
                txtEmail.Text = principal.contact.Email;
                txtLink1.Text = principal.contact.SMLink1;
                txtLink2.Text = principal.contact.SMLink2;
                txtLink3.Text = principal.contact.SMLink3;
                txtWebsite.Text = principal.contact.Website;
                //Start fetching department

                foreach (var _selectedDept in principal.departments)
                {
                    selectedDepartments.Add(_selectedDept);

                }
                gridSelectedDepartment.ItemsSource = null;
                gridSelectedDepartment.ItemsSource = selectedDepartments;

                foreach (var dept in selectedDepartments.ToList())
                {
                    var dep = AllDepartments.Find(x=>x.Id == dept.Id); 

                    if (dep != null)
                    {
                        AllDepartments.Remove(AllDepartments.Find(x=>x.Id == dep.Id));

                        var findParet = AllDepartments.Find(x => x.ParentID == dept.Id);
                        if (findParet != null)
                        {
                            AllDepartments.Add(dept);
                        }

                    }
                }
                gridDepartment.ItemsSource = null;
                gridDepartment.ItemsSource = AllDepartments;

                //End Fetching Deparments
                if (principal.IsSubsidary == true)
                {// set selected row  in a grid for parent company
                    chkissubsidroy.IsChecked = true;
                    {
                        if (principal.ParentID != null || principal.parentDepartment != null)
                        {
                            //Select Customer

                            lookupCustomer.Text = principal.parentDepartment.company.CompanyName;
                            parentcustomer = principal.parentDepartment;
                        }
                        else
                        {
                            lookupCustomer.Text = "Select Principal";
                        }  
                    }

                }
                else
                {
                    chkissubsidroy.IsChecked = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void winPrincipaladd_Loaded(object sender, RoutedEventArgs e)
        {
            chkisActive.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark Prinicpal as InActive") != null) ? true : false;

            loaddepartments();
            loadCurrencies();
            loadIndustryTypes();
            if (frmPrincipalCenter.Editit == 1 || ucPrincipalRegister.Editit == 1)
            {
                loadcompany();
               
                btnaddPrincipal.Content = "Edit Principal";
                winPrincipaladd.Title = "Edit Principal";
            }
            gridSelectedDepartment.ItemsSource = selectedDepartments;
            //SystemLogic.SetUserSettingOfCurrentWindow(gridDepartment);
            SystemLog.LogInfo(this.GetType(), "Form loaded successfully");

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



            }
        }
        private void loaddepartments()
        {
            AllDepartments = deptRepo.GetUserDepartments(MainWindow.currentUserid);
            gridDepartment.ItemsSource = AllDepartments;
            gridDepartment.SelectedItems.Clear();

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

        private void WinPrincipaladd_Unloaded(object sender, RoutedEventArgs e)
        {
           // SystemLogic.SaveUserSettingForCurrentWindow(gridDepartment);
            SystemLog.LogInfo(this.GetType(),"Form closed");
        }

        private void chkissubsidroy_Checked(object sender, RoutedEventArgs e)
        {
            lblcustomername.Visibility = Visibility.Visible;
            lookupCustomer.Visibility = Visibility.Visible;
            loadcustomers();
        }
        public void loadcustomers()
        {

            lookupCustomer.ItemsSource = principalrepo.getUserPrincipal(MainWindow.currentUserid);

        }

        private void chkissubsidroy_Unchecked(object sender, RoutedEventArgs e)
        {
            lblcustomername.Visibility = Visibility.Hidden;
            lookupCustomer.Visibility = Visibility.Hidden;
            lookupCustomer.SelectedItem = null;

        }

        private void lookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            parentcustomer = lookupCustomer.SelectedItem as Principal;
        }

        private void btnRightMove_Click(object sender, RoutedEventArgs e)
        {
            int hasParent = 0;
            try
            {
                var selectedItem = gridDepartment.SelectedItem as Department;
                if (selectedItem != null)
                {
                    var child = principalrepo.getParent(selectedItem.Id);

                    if (child != null)
                    {
                        DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                        return;
                    }
                    var node = selectedItem.parentDepartment;
                    while (node != null)
                    {
                        hasParent = 1;
                        AllDepartments.Remove(AllDepartments.Find(x=>x.Id == selectedItem.Id));
                        if (selectedDepartments.Find(x => x.Id == selectedItem.Id) == null)
                            selectedDepartments.Add(selectedItem);
                        if (selectedDepartments.Find(x => x.Id == node.Id) == null)
                            selectedDepartments.Add(node);
                        var findParet = AllDepartments.Find(x => x.ParentID == node.Id);
                        if (findParet == null)
                        {
                            AllDepartments.Remove(AllDepartments.Find(x => x.Id == node.Id));
                        }
                        node = node.parentDepartment;
                    }
                    if (hasParent == 0)
                    {
                        AllDepartments.Remove(AllDepartments.Find(x => x.Id == selectedItem.Id));
                        if (selectedDepartments.Find(x => x.Id == selectedItem.Id) == null)
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
                    var child = principalrepo.getParent(selectedItem.Id);

                    if (child != null)
                    {
                        DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                        return;
                    }
                    var node = selectedItem.parentDepartment;
                    while (node != null)
                    {
                        parent = 1;
                        if (AllDepartments.Find(x=>x.Id == selectedItem.Id) == null)
                            AllDepartments.Add(selectedItem);
                        selectedDepartments.Remove(selectedDepartments.Find(x => x.Id == selectedItem.Id));

                        if (AllDepartments.Find(x => x.Id == node.Id) == null)
                            AllDepartments.Add(node);
                        var findParet = selectedDepartments.Find(x => x.ParentID == node.Id);
                  
                        if (findParet == null)
                        {
                            selectedDepartments.Remove(selectedDepartments.Find(x => x.Id == node.Id)); 
                        }
                        node = node.parentDepartment;
                    }
                    if (parent == 0)
                    {
                        if (AllDepartments.Find(x=>x.Id == selectedItem.Id) == null)
                            AllDepartments.Add(selectedItem);
                        selectedDepartments.Remove(selectedDepartments.Find(x=>x.Id == selectedItem.Id));

                        //if (!AllDepartments.Contains(selectedItem))
                        //    AllDepartments.Add(selectedItem);
                        //selectedDepartments.Remove(selectedItem);
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
    }
}
