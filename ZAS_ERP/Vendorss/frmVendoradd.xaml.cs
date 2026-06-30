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
using ZAS_ERP.SaleOrderFolder.UserControls;
using DevExpress.Xpf.Core;
using ZAS_ERP.BussinessLogicss.IndustryType;

namespace ZAS_ERP.Vendorss

{

    /// <summary>
    /// Interaction logic for frmvendoradd.xaml
    /// </summary>
    public partial class frmVendoradd : Window
    {
        List<Company> allCompanies = new List<Company>();
        List<Company> selectedCompanies = new List<Company>();
        List<Department> allDepartments= new List<Department>();
        List<Department> selectedDepartments = new List<Department>();

        Vendor vendor = new Vendor();
        VendorRepo vendrepo = new VendorRepo();
        Config config = new Config();
        private Vendor parentVendor;
        DepartmentRepo departmentRepo = new DepartmentRepo();


        public string onlychar { get; set; }
        public string onlynum { get; set; }
        public string required { get; set; }
        public string nospecchar { get; set; }
        public frmVendoradd()
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

        private void loaddepartments()
        {


            //List<ERP_BL.Databases.Department> deptRepo = new List<ERP_BL.Databases.Department>();
            //deptRepo = vendrepo.GetDepartments();
            allDepartments = vendrepo.GetUserDepartments(MainWindow.currentUserid);
            allDepartments = allDepartments.Where(x => x.DeptName != "VDummy").ToList();
            this.gridAllDepartment.ItemsSource = allDepartments;
            gridAllDepartment.SelectedItems.Clear();

            //gridAllDepartment.ItemsSource = vendrepo.GetUserDepartments(MainWindow.currentUserid)
        }

        private void btnaddVendor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
              

                if (txtBussinesName.Text == "")
                {
                    MessageBox.Show("Please Enter Bussines Name", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtBussinesName.Focus();
                }
                else if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Vendor's Currency", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    cmbCurrency.Focus();
                }
                //else if(cmbIndustry.SelectedIndex == -1)
                //{
                //    MessageBox.Show("Please Select Industry", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    cmbIndustry.Focus();
                //}  
                //else if(cmbVendorNature.SelectedIndex == -1)
                //{
                //    MessageBox.Show("Please Select Vendor Nature", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                //    cmbVendorNature.Focus();
                //}
                //else if (selectedCompanies.Count == 0 && selectedCompanies.Count == 0)
                //{
                //    DXMessageBox.Show("Please select at least one Company!");
                //    return;
                //}
                else
                {

                   

                    if (selectedCompanies.Count != 0)
                    {
                        vendor.Companies = new List<Company>();
                        foreach (Company _company in selectedCompanies)
                        {
                            if (!vendor.Companies.Contains(_company))
                            {
                                vendor.Companies.Add(_company);
                            }
                        }
                    }

                    if (txtEIN.Text == "")
                        txtEIN.Text = "0";

                    if (selectedDepartments.Count != 0)
                    {
                        vendor.departments = new List<Department>();
                        foreach (var _dept in selectedDepartments)
                        {
                            if (!vendor.departments.Contains(_dept))
                            {
                                vendor.departments.Add(vendrepo.getDepartment(_dept.Id));
                            }
                        }
                    }
                    else
                    {
                        vendor.departments = new List<Department>();
                    }

                    if (chkissubsidroy.IsChecked == true && parentVendor.Id != 0)
                    {
                        vendor.IsSubsidary = true;
                        vendor.ParentID = parentVendor.Id;
                    }
                    else
                    {
                        vendor.IsSubsidary = false;
                        vendor.ParentID = null;
                        vendor.ParentVendor = null;
                    }
                    vendor.isActive = (chkisActive.IsChecked == true) ? true : false;
                    vendor.isBlackList = (chkisBlackList.IsChecked == true) ? true : false;
                    vendor.Rating = Convert.ToInt32(ratingControl.EditValue);
                    if (ucVendorCenterGrid.Editit == 1)
                    {

                        vendor.contactPerson.FName = txtfirstName.Text.Trim();
                        vendor.contactPerson.LName = txtlastName.Text.Trim();
                        vendor.contactPerson.CNIC = "6110100000000";
                        vendor.contactPerson.FatherName = "";
                        vendor.contactPerson.Gender = Gender.Male;
                        vendor.contactPerson.DOB = Convert.ToDateTime("02-03-2018");
                        vendor.contactPerson.NextKin = "NA";



                        vendor.company.CompanyName = txtBussinesName.Text.Trim();
                        vendor.company.BizType = (BizTypes)cmbBussinesType.SelectedIndex;
                        vendor.company.EmployeerNo = txtEIN.Text.Trim();
                        vendor.company.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                        if(cmbVendorNature.SelectedItem!=null)
                        vendor.vendorNatureId = (cmbVendorNature.SelectedItem as cmbitem).id;
                        vendor.company.compnayType = CompnayTypes.VendorCompany;
                        vendor.company.CurrencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                        vendor.company.openingDate = System.DateTime.Now;
                        vendor.company.closingDate = null;


                        vendor.billingAddres.Country = txtCountry.Text.Trim();
                        vendor.billingAddres.Line1 = txtAdressline1.Text.Trim();
                        vendor.billingAddres.Line2 = txtAdressline2.Text.Trim();
                        vendor.billingAddres.State = txtState.Text.Trim();
                        vendor.billingAddres.Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                        vendor.billingAddres.City = txtCity.Text.Trim();
                        //vendor.billingAddres.region = txtRegionbil.Text.Trim();
                        //vendor.billingAddres.addressType = AddressTypes.billingAddress;



                        vendor.shippingAddress.Country = txtsCountry.Text.Trim();
                        vendor.shippingAddress.Line1 = txtsAdressline1.Text.Trim();
                        vendor.shippingAddress.Line2 = txtsAdressline2.Text.Trim();
                        vendor.shippingAddress.State = txtsState.Text.Trim();
                        vendor.shippingAddress.Zip = string.IsNullOrEmpty(txtsZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                        vendor.shippingAddress.City = txtsCity.Text.Trim();
                        //vendor.shippingAddress.region = txtRegionship.Text.Trim();

                        //vendor.shippingAddress.addressType = AddressTypes.shippingAddress;



                        vendor.contact.ContactNo = txtPhonenum.Text.Trim();
                        vendor.contact.Fax = txtFaxnum.Text.Trim();
                        vendor.contact.Email = txtEmail.Text.Trim();
                        vendor.contact.SMLink1 = txtLink1.Text.Trim();
                        vendor.contact.SMLink2 = txtLink2.Text.Trim();
                        vendor.contact.SMLink3 = txtLink3.Text.Trim();
                        vendor.contact.Website = txtWebsite.Text.Trim();
                        vendor.departments.Add(departmentRepo.GetByName());
                        vendrepo.Update(vendor);
                        MessageBox.Show(vendor.company.CompanyName + " Updated Succesfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                        return;
                    }
                    if (frmVendorCenter.Editit == 1)
                    {

                        vendor.contactPerson.FName = txtfirstName.Text.Trim();
                        vendor.contactPerson.LName = txtlastName.Text.Trim();
                        vendor.contactPerson.CNIC = "6110100000000";
                        vendor.contactPerson.FatherName = "";
                        vendor.contactPerson.Gender = Gender.Male;
                        vendor.contactPerson.DOB = Convert.ToDateTime("02-03-2018");
                        vendor.contactPerson.NextKin = "NA";



                        vendor.company.CompanyName = txtBussinesName.Text.Trim();
                        vendor.company.BizType = (BizTypes)cmbBussinesType.SelectedIndex;
                        vendor.company.EmployeerNo = txtEIN.Text.Trim();
                        vendor.company.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                        vendor.vendorNatureId = (cmbVendorNature.SelectedItem as cmbitem).id;
                        vendor.company.compnayType = CompnayTypes.VendorCompany;
                        vendor.company.CurrencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                        vendor.company.openingDate = System.DateTime.Now;
                        vendor.company.closingDate = null;


                        vendor.billingAddres.Country = txtCountry.Text.Trim();
                        vendor.billingAddres.Line1 = txtAdressline1.Text.Trim();
                        vendor.billingAddres.Line2 = txtAdressline2.Text.Trim();
                        vendor.billingAddres.State = txtState.Text.Trim();
                        vendor.billingAddres.Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                        vendor.billingAddres.City = txtCity.Text.Trim();
                        //vendor.billingAddres.region = txtRegionbil.Text.Trim();
                        //vendor.billingAddres.addressType = AddressTypes.billingAddress;



                        vendor.shippingAddress.Country = txtsCountry.Text.Trim();
                        vendor.shippingAddress.Line1 = txtsAdressline1.Text.Trim();
                        vendor.shippingAddress.Line2 = txtsAdressline2.Text.Trim();
                        vendor.shippingAddress.State = txtsState.Text.Trim();
                        vendor.shippingAddress.Zip = string.IsNullOrEmpty(txtsZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                        vendor.shippingAddress.City = txtsCity.Text.Trim();
                        //vendor.shippingAddress.region = txtRegionship.Text.Trim();

                        //vendor.shippingAddress.addressType = AddressTypes.shippingAddress;



                        vendor.contact.ContactNo = txtPhonenum.Text.Trim();
                        vendor.contact.Fax = txtFaxnum.Text.Trim();
                        vendor.contact.Email = txtEmail.Text.Trim();
                        vendor.contact.SMLink1 = txtLink1.Text.Trim();
                        vendor.contact.SMLink2 = txtLink2.Text.Trim();
                        vendor.contact.SMLink3 = txtLink3.Text.Trim();
                        vendor.contact.Website = txtWebsite.Text.Trim();
                        vendor.departments.Add(departmentRepo.GetByName());
                        vendrepo.Update(vendor);
                        MessageBox.Show(vendor.company.CompanyName + " Updated Succesfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {

                        Person person = new Person()
                        {
                            FName = txtfirstName.Text.Trim(),
                            LName = txtlastName.Text.Trim(),
                            CNIC = "611010000000",
                            FatherName = "",
                            Gender = Gender.Male,
                            DOB = Convert.ToDateTime("02-03-2018"),
                            NextKin = "NA"


                        };
                        vendor.contactPerson = person;
                        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company()
                        {
                            CompanyName = txtBussinesName.Text.Trim(),
                            BizType = (BizTypes)cmbBussinesType.SelectedIndex,
                            EmployeerNo = txtEIN.Text.Trim(),
                            industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id,
                            compnayType = CompnayTypes.VendorCompany,
                            CurrencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                            openingDate = System.DateTime.Now,
                            closingDate = null
                        };
                        vendor.company = company;
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
                        vendor.billingAddres = billadd;
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
                        vendor.shippingAddress = shipadd;
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
                        vendor.contact = contact;

                        var check = vendrepo.CheckVendorName(vendor.company.CompanyName);

                        if ( check != null && check.Count > 0)
                        {
                            DXMessageBox.Show("Please enter a different Company Name!", "Company Name Already Exists");
                            txtBussinesName.Focus();
                            return;
                        }
                        vendor.vendorNatureId = (cmbVendorNature.SelectedItem as cmbitem).id;
                        vendor.departments.Add(departmentRepo.GetByName());
                        vendrepo.Add(vendor);
                        MessageBox.Show(vendor.company.CompanyName + " Added Succesfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
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
                vendor = vendrepo.get(frmVendorCenter.vendorid);

                ratingControl.EditValue = vendor.Rating;
                // company info
                txtBussinesName.Text = vendor.company.CompanyName;
                foreach (cmbitem cmbitem in cmbIndustry.Items)
                {
                    if (cmbitem.id == vendor.company.industryTypeId)
                        cmbIndustry.SelectedItem = cmbitem;
                } 
                foreach (cmbitem cmbitem in cmbVendorNature.Items)
                {
                    if (cmbitem.id == vendor.vendorNatureId)
                        cmbVendorNature.SelectedItem = cmbitem;
                }
                cmbBussinesType.SelectedIndex = Convert.ToInt32(vendor.company.BizType);
                txtEIN.Text = vendor.company.EmployeerNo;
                //currency Removed from vendor
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == vendor.company.CurrencyId)
                        cmbCurrency.SelectedItem = cmbitem;
                }
                // contact person
                txtfirstName.Text = vendor.contactPerson.FName;
                txtlastName.Text = vendor.contactPerson.LName;
                chkisActive.IsChecked = vendor.isActive;
                //Billing adress
                txtCountry.Text = vendor.billingAddres.Country;
                txtCity.Text = vendor.billingAddres.City;
                txtAdressline1.Text = vendor.billingAddres.Line1;
                txtAdressline2.Text = vendor.billingAddres.Line2;
                txtState.Text = vendor.billingAddres.State;
                txtZIP.Text = vendor.billingAddres.Zip.ToString();
                //shiping adress
                txtsCountry.Text = vendor.shippingAddress.Country;
                txtsCity.Text = vendor.shippingAddress.City;
                txtsAdressline1.Text = vendor.shippingAddress.Line1;
                txtsAdressline2.Text = vendor.shippingAddress.Line2;
                txtsState.Text = vendor.shippingAddress.State;
                txtsZIP.Text = vendor.shippingAddress.Zip.ToString();
                //contact details
                txtPhonenum.Text = vendor.contact.ContactNo;
                txtFaxnum.Text = vendor.contact.Fax;
                txtEmail.Text = vendor.contact.Email;
                txtLink1.Text = vendor.contact.SMLink1;
                txtLink2.Text = vendor.contact.SMLink2;
                txtLink3.Text = vendor.contact.SMLink3;
                txtWebsite.Text = vendor.contact.Website;
                selectedCompanies = vendor.Companies;
                selectedDepartments = vendor.departments;

                selectedDepartments = vendor.departments.Where(x => x.DeptName != "VDummy").ToList();
                foreach (Company _Company in selectedCompanies)
                {
                    allCompanies = allCompanies.Where(c => c.Id != _Company.Id).ToList();

                }
                // Remove selectedDepartments from allDepartments and update allEmployees
                foreach (Department _department in selectedDepartments)
                {
                    allDepartments = allDepartments.Where(d => d.Id != _department.Id).ToList();
                }
                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;
                gridAllDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = selectedDepartments;

                //foreach (Department dept in vendor.departments)
                //{
                //    gridAllDepartment.SelectItem(gridAllDepartment.FindRowByValue(gridAllDepartment.Columns.GetColumnByFieldName("Id"), dept.Id));
                //}
                if (vendor.IsSubsidary == true)
                {
                    chkissubsidroy.IsChecked = true;
                    if (vendor.ParentID != null)
                    {
                        parentVendor = vendor.ParentVendor;
                        //lookupParentDept.SelectedItem = lookupParentDept.GetItemByKeyValue(department.parentDepartment);
                        lookupVendor.Text = vendor.ParentVendor.company.CompanyName;
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
        private void loadcompanys()
        {
            try
            {



                //get company by id
                vendor = vendrepo.get(ucVendorCenterGrid.vendorid);

                ratingControl.EditValue = vendor.Rating;
                // company info
                txtBussinesName.Text = vendor.company.CompanyName;
                foreach (cmbitem cmbitem in cmbIndustry.Items)
                {
                    if (cmbitem.id == vendor.company.industryTypeId)
                        cmbIndustry.SelectedItem = cmbitem;
                }  
                foreach (cmbitem cmbitem in cmbVendorNature.Items)
                {
                    if (cmbitem.id == vendor.vendorNatureId)
                        cmbVendorNature.SelectedItem = cmbitem;
                }
                cmbBussinesType.SelectedIndex = Convert.ToInt32(vendor.company.BizType);
                txtEIN.Text = vendor.company.EmployeerNo;
                //currency Removed from vendor
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == vendor.company.CurrencyId)
                        cmbCurrency.SelectedItem = cmbitem;
                }
                // contact person
                txtfirstName.Text = vendor.contactPerson.FName;
                txtlastName.Text = vendor.contactPerson.LName;
                chkisActive.IsChecked = vendor.isActive;
                //Billing adress
                txtCountry.Text = vendor.billingAddres.Country;
                txtCity.Text = vendor.billingAddres.City;
                txtAdressline1.Text = vendor.billingAddres.Line1;
                txtAdressline2.Text = vendor.billingAddres.Line2;
                txtState.Text = vendor.billingAddres.State;
                txtZIP.Text = vendor.billingAddres.Zip.ToString();
                //shiping adress
                txtsCountry.Text = vendor.shippingAddress.Country;
                txtsCity.Text = vendor.shippingAddress.City;
                txtsAdressline1.Text = vendor.shippingAddress.Line1;
                txtsAdressline2.Text = vendor.shippingAddress.Line2;
                txtsState.Text = vendor.shippingAddress.State;
                txtsZIP.Text = vendor.shippingAddress.Zip.ToString();
                //contact details
                txtPhonenum.Text = vendor.contact.ContactNo;
                txtFaxnum.Text = vendor.contact.Fax;
                txtEmail.Text = vendor.contact.Email;
                txtLink1.Text = vendor.contact.SMLink1;
                txtLink2.Text = vendor.contact.SMLink2;
                txtLink3.Text = vendor.contact.SMLink3;
                txtWebsite.Text = vendor.contact.Website;
                selectedCompanies = vendor.Companies;
                selectedDepartments = vendor.departments;

                selectedDepartments = vendor.departments.Where(x => x.DeptName != "VDummy").ToList();
                foreach (Company _Company in selectedCompanies)
                {
                    allCompanies = allCompanies.Where(c => c.Id != _Company.Id).ToList();

                }
                // Remove selectedDepartments from allDepartments and update allEmployees
                foreach (Department _department in selectedDepartments)
                {
                    allDepartments = allDepartments.Where(d => d.Id != _department.Id).ToList();
                }
                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;
                gridAllDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = selectedDepartments;

                if (vendor.IsSubsidary == true)
                {
                    chkissubsidroy.IsChecked = true;
                    if (vendor.ParentID != null)
                    {
                        parentVendor = vendor.ParentVendor;
                        //lookupParentDept.SelectedItem = lookupParentDept.GetItemByKeyValue(department.parentDepartment);
                        lookupVendor.Text = vendor.ParentVendor.company.CompanyName;
                    }

                }
                else
                {
                    chkissubsidroy.IsChecked = false;
                }   
                if (vendor.isBlackList == true)
                {
                    chkisBlackList.IsChecked = true;
                }
                else
                {
                    chkisBlackList.IsChecked = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        private void winVendoradd_Loaded(object sender, RoutedEventArgs e)
        {
            chkisActive.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark Vendor as InActive") != null) ? true : false;
            chkisBlackList.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark Vendor as BlackList") != null) ? true : false;

            loadcompanies();
            loaddepartments();
            loadIndustryTypes();
            loadCurrencies();
            VendorNaturesTypes();
            if (ucVendorCenterGrid.Editit == 1)
            {
                loadcompanys();
                btnaddVendor.Content = "Edit Vendor";
                winVendoradd.Title = "Edit Vendor";
                loadAccounts();
                tabBulkCompanies.IsEnabled = true;
                tabDep.IsEnabled = true;

            }
            if (frmVendorCenter.Editit == 1)
            {
                loadcompany();
                btnaddVendor.Content = "Edit Vendor";
                winVendoradd.Title = "Edit Vendor";
                loadAccounts();
                tabBulkCompanies.IsEnabled = true;
                tabDep.IsEnabled = true;

            }
            txtBussinesName.Focus();
        }

        private void btnRightMove_Click(object sender, RoutedEventArgs e)
        {
            int hasParent = 0;
            try
            {
                var selectedItem = gridAllDepartment.SelectedItem as Department;
                if (selectedItem != null)
                {
                    var child = vendrepo.getParent(selectedItem.Id);

                    if (child != null)
                    {
                        DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                        return;
                    }
                    var node = selectedItem.parentDepartment;
                    while (node != null)
                    {
                        hasParent = 1;
                        allDepartments.Remove(selectedItem);
                        if (!selectedDepartments.Contains(selectedItem))
                            selectedDepartments.Add(selectedItem);
                        if (!selectedDepartments.Contains(node))
                            selectedDepartments.Add(node);
                        var findParet = allDepartments.Find(x => x.parentDepartment == node);
                        if (findParet == null)
                        {
                            allDepartments.Remove(node);
                        }
                        node = node.parentDepartment;
                    }
                    if (hasParent == 0)
                    {
                        allDepartments.Remove(selectedItem);
                        if (!selectedDepartments.Contains(selectedItem))
                            selectedDepartments.Add(selectedItem);
                    }
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Department List Register!");
                }
                gridSelectedDepartments.RefreshData();
                gridAllDepartment.RefreshData();
                gridAllDepartment.SelectedItem = null;
                gridSelectedDepartments.SelectedItem = null;
                gridSelectedDepartments.ItemsSource=selectedDepartments;
                gridAllDepartment.ItemsSource=allDepartments;
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
                var selectedItem = gridSelectedDepartments.SelectedItem as Department;

                if (selectedItem != null)
                {
                    var child = vendrepo.getParent(selectedItem.Id);

                    if (child != null)
                    {
                        DXMessageBox.Show("Selected Department is parent please select child to add selected Department register");
                        return;
                    }
                    var node = selectedItem.parentDepartment;
                    while (node != null)
                    {
                        parent = 1;
                        if (!allDepartments.Contains(selectedItem))
                            allDepartments.Add(selectedItem);
                        selectedDepartments.Remove(selectedItem);
                        if (!allDepartments.Contains(node))
                            allDepartments.Add(node);
                        var findParet = selectedDepartments.Find(x => x.parentDepartment == node);
                        if (findParet == null)
                        {
                            selectedDepartments.Remove(node);
                        }
                        node = node.parentDepartment;
                    }
                    if (parent == 0)
                    {
                        if (!allDepartments.Contains(selectedItem))
                            allDepartments.Add(selectedItem);
                        selectedDepartments.Remove(selectedItem);
                    }
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Department Register!");
                }
                gridSelectedDepartments.RefreshData();
                gridAllDepartment.RefreshData();
                gridAllDepartment.SelectedItem = null;
                gridSelectedDepartments.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);

            }
        }

        private void loadAccounts()
        {
            if(vendor.Id > 0)
            {
                SalesReceiptRepo repo = new SalesReceiptRepo();
                var accntList = repo.GetAllAccounts().Where(x => x.vendor_Id == vendor.Id).ToList();

                //comment for commit
                //GetAllAccounts accnts = new GetAllAccounts(accntList);
                grdCntrlAccountList.ItemsSource = accntList;
                //grdCntrlAccountList.Columns["SerialNo"].Visible = false;
            }
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
        private void chkissubsidroy_Checked(object sender, RoutedEventArgs e)
        {


            lblcustomername.Visibility = Visibility.Visible;
            lookupVendor.Visibility = Visibility.Visible;
            loadVendors();

        }

        private void loadVendors()
        {
            lookupVendor.ItemsSource = vendrepo.getAll();
        }

        private void chkissubsidroy_Unchecked(object sender, RoutedEventArgs e)
        {
            lblcustomername.Visibility = Visibility.Hidden;
            lookupVendor.Visibility = Visibility.Hidden;
        }
        private void cmbCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            // cmbCurrency.DisplayMemberPath = (cmbCurrency.SelectedItem as cmbitem).name;
            if (cmbCurrency.SelectedItem as cmbitem != null)
            {
                int idd = (cmbCurrency.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    BussinessLogicss.frmCurrency frmCurrency = new BussinessLogicss.frmCurrency();
                    frmCurrency.ShowDialog();
                    loadCurrencies();
                }



            }
        }
        public void loadIndustryTypes()
        {


            List<IndustryType> industryTypes = new List<IndustryType>();
            //  employees = cont1.GetEmployees();
            CompanyRepo repo = new CompanyRepo();
            //industryTypes = (List<IndustryType>)repo.GetIndustryTypes().Where(x=>x.isVendorType == true);
            industryTypes = repo.GetVendorIndustryTypes();
            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (IndustryType industry in industryTypes)
            {

                cmbitems.Add(new cmbitem() { name = industry.name, id = industry.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbIndustry.ItemsSource = cmbitems;

        }
        public void VendorNaturesTypes()
        {


            List<VendorNature> industryTypes = new List<VendorNature>();
            CompanyRepo repo = new CompanyRepo();
            industryTypes = repo.GetVendorNatures();
            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (VendorNature industry in industryTypes)
            {

                cmbitems.Add(new cmbitem() { name = industry.name, id = industry.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbVendorNature.ItemsSource = cmbitems;

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
        private void lookupVendor_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            parentVendor = lookupVendor.SelectedItem as Vendor;
        }

        public void loadcompanies()
        {
            allCompanies = vendrepo.GetEmployeeCompanies(SYSTEM_STATIC.currentUser.employeeId);
            gridAllCompany.ItemsSource = allCompanies;
        }

        private void ImgLeftToRightCompBulk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCompBulk.Width = 28;
        }

        private void ImgRightToLeftCompBulk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCompBulk.Width = 28;
        }

        private void ImgLeftToRightCompBulk_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCompBulk.Width = 30;

            if (gridAllCompany.SelectedItem != null)
            {
                var company = gridAllCompany.SelectedItem as Company;
                selectedCompanies.Add(company);
                allCompanies.Remove(company);
                gridAllCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;
                        
                //allDepartmentsBulk.AddRange(company.departments.Where(x=> x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());

                //allDepartmentsBulk = allDepartmentsBulk.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                //gridDepartmentBulk.ItemsSource = null;
                //gridDepartmentBulk.ItemsSource = allDepartmentsBulk;
            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }

        private void ImgRightToLeftCompBulk_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCompBulk.Width = 30;
            if (gridSelectedCompanies.SelectedItem != null)
            {
                var company = gridSelectedCompanies.SelectedItem as Company;

                //if (company.departments.Intersect(selectedDepartmentsBulk).Count() > 0)
                //{
                //    DXMessageBox.Show("Kindly remove the Departments of this Company from Selected Departments!");
                //    return;
                //}
                selectedCompanies.Remove(company);
                allCompanies.Add(company);

                gridAllCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;

                //var depts = selectedDepartmentsBulk.Except(allDepartmentsBulk.Intersect(selectedDepartmentsBulk));
                //allDepartmentsBulk = new List<Department>();
                //foreach (var _cmpny in selectedCompanies)
                //{

                //    switch ((TargetsTransactionType)cmbxModuleType.SelectedIndex)
                //    {
                //        case TargetsTransactionType.Inquiry:
                //            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                //            break;
                //        case TargetsTransactionType.Offer:
                //            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                //            break;
                //        case TargetsTransactionType.PurchaseInvoice:
                //            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                //            break;
                //        case TargetsTransactionType.PurchaseOrder:
                //            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                //            break;
                //        case TargetsTransactionType.SaleInvoice:
                //            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                //            break;
                //        case TargetsTransactionType.SaleOrder:
                //            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                //            break;
                //        case TargetsTransactionType.SaleReceipt:
                //            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                //            break;
                //        case TargetsTransactionType.VendorBill:
                //            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsVendorBillType == true).ToList());
                //            break;
                //    }
                //}


                //allDepartmentsBulk = allDepartmentsBulk.Except(depts).ToList();

                //allDepartmentsBulk = allDepartmentsBulk.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                //gridDepartmentBulk.ItemsSource = null;
                //gridDepartmentBulk.ItemsSource = allDepartmentsBulk;
                //gridSelectedDepartmentsBulk.ItemsSource = null;
                //gridSelectedDepartmentsBulk.ItemsSource = selectedDepartmentsBulk;

            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }

        private void cmbVendorNature_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbVendorNature.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbVendorNature.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmIndustryTypeAddManual industryadd = new frmIndustryTypeAddManual();
                    industryadd.ShowDialog();
                    VendorNaturesTypes();
                }



            }
        }
    }
}
