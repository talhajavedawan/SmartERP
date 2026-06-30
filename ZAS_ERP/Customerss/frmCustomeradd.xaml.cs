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
using System.Globalization;

namespace ZAS_ERP.Customerss

{

    /// <summary>
    /// Interaction logic for frmCustomeradd.xaml
    /// </summary>
    public partial class frmCustomeradd : DXWindow
    {
        CurrentUserSetting currentUserSetting = new CurrentUserSetting();
        CustomerCompany customer = new CustomerCompany();
        CustomerCompany parentcustomer = new CustomerCompany();
        CustomerCompRepo compRepo = new CustomerCompRepo();
        List<Company> allCompanies = new List<Company>();
        List<Company> selectedCompanies = new List<Company>();
        List<Department> allDepartments = new List<Department>();
        List<Department> selectedDepartments = new List<Department>();

        List<Department> disableDepartments = new List<Department>();

        //CompanyRepo repo = new CompanyRepo();
        Config config = new Config();
        public string onlychar { get; set; }
        public string onlynum { get; set; }
        public string required { get; set; }
        public string nospecchar { get; set; }
        List<IndustryType> industryTypes = new List<IndustryType>();
        public frmCustomeradd()
        {
            try

            {
                InitializeComponent();
                List<string> INDList = config.getIndustryType();
                List<string> Biztype = config.getBizType();
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
        private void btnaddCustomer_Click(object sender, RoutedEventArgs e)
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


                if (txtEIN.Text == "")
                    txtEIN.Text = "0";
                if (gridDepartment.SelectedItems.Count != 0)
                {
                    //string unitNames = "";
                    customer.departments = new List<Department>();
                    //int a = 0;

                    foreach (Department dept in gridDepartment.SelectedItems)
                    {
                        if (!customer.departments.Contains(dept))
                        {
                            customer.departments.Add(dept);
                            //unitNames = unitNames + dept.DeptName + " | ";
                        }
                        //customer.LinkedDepartment = unitNames;

                        //company.departments.Add(dept);
                    }

                }
                if (gridCompany.SelectedItems.Count != 0)
                {
                    customer.Companies = new List<Company>();
                    foreach (Company comapny in gridCompany.SelectedItems)
                    {

                        if (!customer.Companies.Contains(comapny))
                        {
                            customer.Companies.Add(comapny);
                        }
                        //company.departments.Add(dept);
                    }
                }


                var grd = lookupIndustry.GetGridControl();
                if (grd != null)
                {
                    customer.IndustryTypes = new List<IndustryType>();

                    foreach (IndustryType industry in grd.SelectedItems)
                    {
                        if (!customer.IndustryTypes.Contains(industry))
                        {
                            customer.IndustryTypes.Add(industry);
                        }
                    }
                }


                customer.isActive = (chkisActive.IsChecked == true) ? true : false;

                if (chkisParent.IsChecked == true && lookupCustomer.SelectedIndex>-1)
                {
                    customer.IsSubsidary = true;

                    //customer.ParentID = parentcustomer.Id;
                    customer.ParentID = (lookupCustomer.SelectedItem as CustomerCompany).Id;
                }
                else
                {
                    customer.IsSubsidary = false;
                    customer.ParentID = null;
                    customer.parentCompany = null;
                }
                customer.Companies = GetSelectedCompanies();
                customer.departments = new List<Department>();

                var selDepartments = GetSelectedDepartments();
                if (selDepartments.Count > 0)
                {
                    customer.departments = selDepartments;
                }                
                else
                {
                    DXMessageBox.Show("Please attach departments");
                    return;
                }
                var disDepartments = GetDisableDepartments();
                if (disDepartments != null)
                    customer.disableDepartments = disableDepartments;


                //if (ucCustomerCenterGrid.Editit == 1)
                //{

                    //    customer.contactPerson.FName = txtfirstName.Text.Trim();
                    //    customer.contactPerson.LName = txtlastName.Text.Trim();
                    //    customer.contactPerson.CNIC = "6110100000000";
                    //    customer.contactPerson.FatherName = "";
                    //    customer.contactPerson.Gender = Gender.Male;
                    //    customer.contactPerson.DOB = Convert.ToDateTime("02-03-2018");
                    //    customer.contactPerson.NextKin = "NA";



                    //    customer.company.CompanyName = txtBussinesName.Text.Trim();
                    //    customer.company.BizType = (BizTypes)cmbBussinesType.SelectedIndex;
                    //    customer.company.EmployeerNo = txtEIN.Text.Trim();
                    //    //customer.company.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                    //    customer.company.compnayType = CompnayTypes.CustomerCompany;
                    //    customer.company.CurrencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                    //    customer.company.openingDate = System.DateTime.Now;
                    //    customer.company.closingDate = null;
                    //    customer.company.CustomerVAT = txtVAT.Text;
                    //    customer.company.SaleTaxRegistrationNumber = txtRegistrationTaxNumber.Text;


                    //    customer.billingAddres.Country = txtCountry.Text.Trim();
                    //    customer.billingAddres.Line1 = txtAdressline1.Text.Trim();
                    //    customer.billingAddres.Line2 = txtAdressline2.Text.Trim();
                    //    customer.billingAddres.State = txtState.Text.Trim();
                    //    customer.billingAddres.Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                    //    customer.billingAddres.City = txtCity.Text.Trim();
                    //    customer.billingAddres.region = txtRegionbil.Text.Trim();
                    //    //customer.billingAddres.addressType = AddressTypes.billingAddress;



                    //    customer.shippingAddress.Country = txtsCountry.Text.Trim();
                    //    customer.shippingAddress.Line1 = txtsAdressline1.Text.Trim();
                    //    customer.shippingAddress.Line2 = txtsAdressline2.Text.Trim();
                    //    customer.shippingAddress.State = txtsState.Text.Trim();
                    //    customer.shippingAddress.Zip = string.IsNullOrEmpty(txtsZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                    //    customer.shippingAddress.City = txtsCity.Text.Trim();
                    //    customer.shippingAddress.region = txtRegionship.Text.Trim();

                    //    //customer.shippingAddress.addressType = AddressTypes.shippingAddress;



                    //    customer.contact.ContactNo = txtPhonenum.Text.Trim();
                    //    customer.contact.Fax = txtFaxnum.Text.Trim();
                    //    customer.contact.Email = txtEmail.Text.Trim();
                    //    customer.contact.SMLink1 = txtLink1.Text.Trim();
                    //    customer.contact.SMLink2 = txtLink2.Text.Trim();
                    //    customer.contact.SMLink3 = txtLink3.Text.Trim();
                    //    customer.contact.Website = txtWebsite.Text.Trim();
                    //    customer.contact.ContactNo1 = txtPhonenum1.Text.Trim();
                    //    customer.contact.ContactNo2 = txtPhonenum2.Text.Trim();
                    //    customer.contact.ContactNo3 = txtPhonenum3.Text.Trim();
                    //    customer.contact.Email1 = txtEmail1.Text.Trim();
                    //    customer.contact.Email2 = txtEmail2.Text.Trim();
                    //    customer.contact.Email3 = txtEmail3.Text.Trim();

                    //    compRepo.Update(customer);
                    //    MessageBox.Show("Customer Updated");
                    //    this.Close();
                    //    return;
                    //}

                if (frmCustomerCenter.Editit == 1 || ucCustomerCenterGrid.Editit==1)
                {
                    customer.Companies = new List<Company>();
                    customer.Companies = GetSelectedCompanies();
                    //if (gridSelectedDepartments.ItemsSource != null)
                    //{
                    //    customer.departments = new List<Department>();
                    //    customer.departments = GetSelectedDepartments();
                    //}
                    //else
                    //{
                    //    DXMessageBox.Show("Please attach departments");
                    //    return;
                    //}
                    //if(gridSelectedDepartments.ItemsSource!=null)
                    //customer.disableDepartments = GetDisableDepartments();

                    customer.contactPerson.FName = txtfirstName.Text.Trim();
                    customer.contactPerson.LName = txtlastName.Text.Trim();
                    customer.contactPerson.CNIC = "6110100000000";
                    customer.contactPerson.FatherName = "";
                    customer.contactPerson.Gender = Gender.Male;
                    customer.contactPerson.DOB = Convert.ToDateTime("02-03-2018");
                    customer.contactPerson.NextKin = "NA";



                    customer.company.CompanyName = txtBussinesName.Text.Trim();
                    customer.company.BizType = (BizTypes)cmbBussinesType.SelectedIndex;
                    customer.company.EmployeerNo = txtEIN.Text.Trim();
                    // customer.company.industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id;
                    customer.company.compnayType = CompnayTypes.CustomerCompany;
                    customer.company.CurrencyId = (cmbCurrency.SelectedItem as cmbitem).id;
                    customer.company.openingDate = System.DateTime.Now;
                    customer.company.closingDate = null;
                    customer.company.CustomerVAT = txtVAT.Text;
                    customer.company.SaleTaxRegistrationNumber = txtRegistrationTaxNumber.Text;


                    customer.billingAddres.Country = txtCountry.Text.Trim();
                    customer.billingAddres.Line1 = txtAdressline1.Text.Trim();
                    customer.billingAddres.Line2 = txtAdressline2.Text.Trim();
                    customer.billingAddres.State = txtState.Text.Trim();
                    customer.billingAddres.Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                    customer.billingAddres.City = txtCity.Text.Trim();
                    customer.billingAddres.region = txtRegionbil.Text.Trim();
                    //customer.billingAddres.addressType = AddressTypes.billingAddress;



                    customer.shippingAddress.Country = txtsCountry.Text.Trim();
                    customer.shippingAddress.Line1 = txtsAdressline1.Text.Trim();
                    customer.shippingAddress.Line2 = txtsAdressline2.Text.Trim();
                    customer.shippingAddress.State = txtsState.Text.Trim();
                    customer.shippingAddress.Zip = string.IsNullOrEmpty(txtsZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim());
                    customer.shippingAddress.City = txtsCity.Text.Trim();
                    customer.shippingAddress.region = txtRegionship.Text.Trim();

                    //customer.shippingAddress.addressType = AddressTypes.shippingAddress;



                    customer.contact.ContactNo = txtPhonenum.Text.Trim();
                    customer.contact.Fax = txtFaxnum.Text.Trim();
                    customer.contact.Email = txtEmail.Text.Trim();
                    customer.contact.SMLink1 = txtLink1.Text.Trim();
                    customer.contact.SMLink2 = txtLink2.Text.Trim();
                    customer.contact.SMLink3 = txtLink3.Text.Trim();
                    customer.contact.Website = txtWebsite.Text.Trim();
                    customer.contact.ContactNo1 = txtPhonenum1.Text.Trim();
                    customer.contact.ContactNo2 = txtPhonenum2.Text.Trim();
                    customer.contact.ContactNo3 = txtPhonenum3.Text.Trim();
                    customer.contact.Email1 = txtEmail1.Text.Trim();
                    customer.contact.Email2 = txtEmail2.Text.Trim();
                    customer.contact.Email3 = txtEmail3.Text.Trim();

                    compRepo.Update(customer);
                    MessageBox.Show("Customer Updated");
                    this.Close();
                }
                else
                {
                    

                    Person person = new Person()
                    {
                        FName = txtfirstName.Text.Trim(),
                        LName = txtlastName.Text.Trim(),
                        CNIC = "6110100000000",
                        FatherName = "",
                        Gender = Gender.Male,
                        DOB = Convert.ToDateTime("02-03-2018"),
                        NextKin = "NA"
                    };
                    customer.contactPerson = person;
                    ERP_BL.Databases.Company company = new ERP_BL.Databases.Company()
                    {
                        CompanyName = txtBussinesName.Text.Trim(),

                        BizType = (BizTypes)cmbBussinesType.SelectedIndex,
                        EmployeerNo = txtEIN.Text.Trim(),
                        //industryTypeId = (cmbIndustry.SelectedItem as cmbitem).id,
                        compnayType = CompnayTypes.CustomerCompany,
                        CurrencyId = (cmbCurrency.SelectedItem as cmbitem).id,
                        openingDate = System.DateTime.Now,
                        closingDate = null,
                        CustomerVAT = txtVAT.Text,
                        SaleTaxRegistrationNumber = txtRegistrationTaxNumber.Text
                    };
                    customer.company = company;
                    var match = compRepo.CheckCompanyNameExixtence(customer.company.CompanyName, customer.Id);//check if it exist already or not
                    if (match == true)
                    {
                        DXMessageBox.Show("Country name " + txtBussinesName.Text + " Already exist please add another one!");
                        return;
                    }

                    Address billadd = new Address()

                    {

                        Country = txtCountry.Text.Trim(),
                        Line1 = txtAdressline1.Text.Trim(),
                        Line2 = txtAdressline2.Text.Trim(),
                        State = txtState.Text.Trim(),
                        Zip = string.IsNullOrEmpty(txtZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim()),
                        City = txtCity.Text.Trim(),
                        region = txtRegionbil.Text.Trim(),
                        addressType = AddressTypes.billingAddress,
                    };
                    customer.billingAddres = billadd;
                    Address shipadd = new Address()

                    {
                        Id = billadd.Id + 1,
                        Country = txtsCountry.Text.Trim(),
                        Line1 = txtsAdressline1.Text.Trim(),
                        Line2 = txtsAdressline2.Text.Trim(),
                        State = txtsState.Text.Trim(),
                        Zip = string.IsNullOrEmpty(txtsZIP.Text.Trim()) ? 0 : Convert.ToInt32(txtZIP.Text.Trim()),
                        City = txtsCity.Text.Trim(),
                        region = txtRegionship.Text.Trim(),

                        addressType = AddressTypes.shippingAddress

                    };
                    customer.shippingAddress = shipadd;
                    Contact contact = new Contact()
                    {
                        ContactNo = txtPhonenum.Text.Trim(),
                        Fax = txtFaxnum.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        SMLink1 = txtLink1.Text.Trim(),
                        SMLink2 = txtLink2.Text.Trim(),
                        SMLink3 = txtLink3.Text.Trim(),
                        Website = txtWebsite.Text.Trim(),
                        ContactNo1 = txtPhonenum1.Text.Trim(),
                        ContactNo2 = txtPhonenum2.Text.Trim(),
                        ContactNo3 = txtPhonenum3.Text.Trim(),
                        Email1 = txtEmail1.Text.Trim(),
                        Email2 = txtEmail2.Text.Trim(),
                        Email3 = txtEmail3.Text.Trim()
                    };

                    customer.contact = contact;
                    //customer.company.CustomerVAT = txtVAT.Text;


                    compRepo.Add(customer);
                    MessageBox.Show("Customer Added");
                    this.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        //private void  loadgrid()
        //  {
        //      List<ERP_BL.Databases.CustomerCompany> companyRepos = new List<ERP_BL.Databases.CustomerCompany>();
        //      companyRepos = compRepo.getAll();
        //      List<ERP_BL.Databases.CustomerCompany> complst = new List<ERP_BL.Databases.CustomerCompany>();
        //      foreach (CustomerCompany cust in companyRepos)
        //      {
        //          if (cust.IsSubsidary == false && cust.ParentID == null && cust.Id !=frmCustomerCenter.customerid)
        //              complst.Add(cust);

        //      }
        //      this.grdcutomercompanies.ItemsSource = complst;

        //      // grdcutomercompanies.AutoGenerateColumns = AutoGenerateColumnsMode.AddNew;
        //      grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "Id" });
        //      grdcutomercompanies.Columns.GetColumnByFieldName("Id").Visible = false;
        //      grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "company.CompanyName" });
        //      grdcutomercompanies.Columns.GetColumnByFieldName("company.CompanyName").Header = "Company Name";
        //      grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "contactPerson.FName" });
        //      grdcutomercompanies.Columns.GetColumnByFieldName("contactPerson.FName").Header = "First Name";
        //      grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "contactPerson.LName" });
        //      grdcutomercompanies.Columns.GetColumnByFieldName("contactPerson.LName").Header = "Last Name";
        //      //grdcutomercompanies.Columns.Add(new GridColumn() { FieldName = "IsSubsidary" });
        //      //grdcutomercompanies.Columns.GetColumnByFieldName("contactPerson.LName").Header = "Subsidory";
        //      //foreach ( CustomerCompany customercomp in grdcutomercompanies.VisibleItems)
        //      //{
        //      //    //grdcutomercompanies.FindRowByValue()


        //      //}


        //  }
        private void loadcompanys()
        {
            try
            {
                //get company by id
                customer = compRepo.get(ucCustomerCenterGrid.customerid);
                // company info
                txtBussinesName.Text = customer.company.CompanyName;
                if (customer.company.industryType != null && customer.IndustryTypes.Count == 0)
                {
                    lookupIndustry.EditValue = customer.company.industryType;
                    lookupIndustry.DisplayMember = "name";

                }
                foreach (var BT in Enum.GetNames(typeof(ERP_BL.Enums.BizTypes)))
                {
                    if (customer.company.BizType.ToString() == BT)
                    {
                        cmbBussinesType.SelectedItem = BT;
                        break;
                    }
                }
                txtEIN.Text = customer.company.EmployeerNo;
                txtVAT.Text = customer.company.CustomerVAT;
                txtRegistrationTaxNumber.Text = customer.company.SaleTaxRegistrationNumber;
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == customer.company.CurrencyId)
                        cmbCurrency.SelectedItem = cmbitem;
                }
                //contact person
                txtfirstName.Text = customer.contactPerson.FName;
                txtlastName.Text = customer.contactPerson.LName;
                chkisActive.IsChecked = customer.isActive;
                //Billing adress
                txtCountry.Text = customer.billingAddres.Country;
                txtCity.Text = customer.billingAddres.City;
                txtAdressline1.Text = customer.billingAddres.Line1;
                txtAdressline2.Text = customer.billingAddres.Line2;
                txtState.Text = customer.billingAddres.State;
                txtRegionbil.Text = customer.billingAddres.region;

                txtZIP.Text = customer.billingAddres.Zip.ToString();
                //shiping adress
                txtsCountry.Text = customer.shippingAddress.Country;
                txtsCity.Text = customer.shippingAddress.City;
                txtsAdressline1.Text = customer.shippingAddress.Line1;
                txtsAdressline2.Text = customer.shippingAddress.Line2;
                txtsState.Text = customer.shippingAddress.State;
                txtsZIP.Text = customer.shippingAddress.Zip.ToString();
                txtRegionship.Text = customer.shippingAddress.region;

                //contact details
                txtPhonenum.Text = customer.contact.ContactNo;

                if (customer.contact.ContactNo1 != null)
                    txtPhonenum1.Text = customer.contact.ContactNo1;
                if (customer.contact.ContactNo2 != null)
                    txtPhonenum2.Text = customer.contact.ContactNo2;
                if (customer.contact.ContactNo3 != null)
                    txtPhonenum3.Text = customer.contact.ContactNo3;

                txtFaxnum.Text = customer.contact.Fax;
                txtEmail.Text = customer.contact.Email;
                if (customer.contact.Email1 != null)
                    txtEmail1.Text = customer.contact.Email1;
                if (customer.contact.Email2 != null)
                    txtEmail2.Text = customer.contact.Email2;
                if (customer.contact.Email3 != null)
                    txtEmail3.Text = customer.contact.Email3;

                txtLink1.Text = customer.contact.SMLink1;
                txtLink2.Text = customer.contact.SMLink2;
                txtLink3.Text = customer.contact.SMLink3;
                txtWebsite.Text = customer.contact.Website;
             

                string industryName = "";
                industryTypes = new List<IndustryType>();
                if (customer.company.industryType == null || customer.IndustryTypes.Count > 0)
                {
                    foreach (IndustryType company in customer.IndustryTypes)
                    {
                        industryName = industryName + " | " + company.name;
                        industryTypes.Add(company);
                    }
                    lookupIndustry.EditValue = industryName;
                }
                selectedCompanies = customer.Companies;
                selectedDepartments = customer.departments;
                disableDepartments = customer.disableDepartments;
                foreach (Company _Company in selectedCompanies)
                {
                    allCompanies.Remove(_Company);
                    allDepartments.AddRange(_Company.departments);
                    allDepartments = allDepartments.GroupBy(x => x.Id).Select(x => x.First()).ToList();
                }
                foreach (Department _department in selectedDepartments)
                {
                    allDepartments.Remove(_department);
                }
                foreach (Department _department in disableDepartments)
                {
                    selectedDepartments.Remove(_department);
                }
                gridSelectedCompanies.ItemsSource = selectedCompanies;
                gridDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = selectedDepartments;
                gridDisableDepartments.ItemsSource = disableDepartments;


                if (customer.IsSubsidary == true)
                {// set selected row  in a grid for parent company
                    chkisParent.IsChecked = true;
                    {
                        if (customer.ParentID != null || customer.parentCompany != null)
                        {
                            lookupCustomer.Text = customer.parentCompany.company.CompanyName;
                            parentcustomer = customer.parentCompany;
                        }
                        else
                        {
                            lookupCustomer.Text = "Select Customer";

                        }
                    }

                }
                else
                {
                    chkisParent.IsChecked = false;
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
                customer = compRepo.get(frmCustomerCenter.customerid);

                customer = compRepo.get(ucCustomerCenterGrid.customerid);
                // company info
                txtBussinesName.Text = customer.company.CompanyName;
                cmbBussinesType.SelectedIndex = Convert.ToInt32(customer.company.BizType);
                txtEIN.Text = customer.company.EmployeerNo;
                txtVAT.Text = customer.company.CustomerVAT;
                txtRegistrationTaxNumber.Text = customer.company.SaleTaxRegistrationNumber;
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == customer.company.CurrencyId)
                        cmbCurrency.SelectedItem = cmbitem;
                }
                //contact person
                txtfirstName.Text = customer.contactPerson.FName;
                txtlastName.Text = customer.contactPerson.LName;
                chkisActive.IsChecked = customer.isActive;
                //Billing adress
                txtCountry.Text = customer.billingAddres.Country;
                txtCity.Text = customer.billingAddres.City;
                txtAdressline1.Text = customer.billingAddres.Line1;
                txtAdressline2.Text = customer.billingAddres.Line2;
                txtState.Text = customer.billingAddres.State;
                txtRegionbil.Text = customer.billingAddres.region;

                txtZIP.Text = customer.billingAddres.Zip.ToString();
                //shiping adress
                txtsCountry.Text = customer.shippingAddress.Country;
                txtsCity.Text = customer.shippingAddress.City;
                txtsAdressline1.Text = customer.shippingAddress.Line1;
                txtsAdressline2.Text = customer.shippingAddress.Line2;
                txtsState.Text = customer.shippingAddress.State;
                txtsZIP.Text = customer.shippingAddress.Zip.ToString();
                txtRegionship.Text = customer.shippingAddress.region;

                //contact details
                txtPhonenum.Text = customer.contact.ContactNo;

                if (customer.contact.ContactNo1 != null)
                    txtPhonenum1.Text = customer.contact.ContactNo1;
                if (customer.contact.ContactNo2 != null)
                    txtPhonenum2.Text = customer.contact.ContactNo2;
                if (customer.contact.ContactNo3 != null)
                    txtPhonenum3.Text = customer.contact.ContactNo3;

                txtFaxnum.Text = customer.contact.Fax;
                txtEmail.Text = customer.contact.Email;
                if (customer.contact.Email1 != null)
                    txtEmail1.Text = customer.contact.Email1;
                if (customer.contact.Email2 != null)
                    txtEmail2.Text = customer.contact.Email2;
                if (customer.contact.Email3 != null)
                    txtEmail3.Text = customer.contact.Email3;

                txtLink1.Text = customer.contact.SMLink1;
                txtLink2.Text = customer.contact.SMLink2;
                txtLink3.Text = customer.contact.SMLink3;
                txtWebsite.Text = customer.contact.Website;
                selectedCompanies = customer.Companies;
                selectedDepartments = customer.departments;
                disableDepartments = customer.disableDepartments;
                foreach (Company _Company in selectedCompanies)
                {
                    allCompanies.Remove(_Company);
                    allDepartments.AddRange(_Company.departments);
                    allDepartments = allDepartments.GroupBy(x => x.Id).Select(x => x.First()).ToList();
                }
                foreach (Department _department in selectedDepartments)
                {
                    allDepartments.Remove(_department);
                }
                foreach (Department _department in disableDepartments)
                {
                    disableDepartments.Remove(_department);
                }
                gridSelectedCompanies.ItemsSource = selectedCompanies;
                gridDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = selectedDepartments;
                gridDisableDepartments.ItemsSource = disableDepartments;
                if (customer.IsSubsidary == true)
                {
                    // set selected row  in a grid for parent company
                    chkisParent.IsChecked = true;
                    {
                        if (customer.ParentID != null || customer.parentCompany != null)
                        {
                            lookupCustomer.Text = customer.parentCompany.company.CompanyName;
                            parentcustomer = customer.parentCompany;
                        }
                        else
                        {
                            lookupCustomer.Text = "Select Customer";
                        }
                    }
                }
                else
                {
                    chkisParent.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void wincustomeradd_Loaded(object sender, RoutedEventArgs e)
        {
            chkisActive.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark Customer as InActive") != null) ? true : false;
            allCompanies = compRepo.GetEmployeeCompanies(SYSTEM_STATIC.currentUser.employeeId);
            gridCompany.ItemsSource = allCompanies; 
            loadMultipleIndustryType();
            loadCurrencies();
            if (ucCustomerCenterGrid.Editit == 1)
            {
                loadcompanys();
                wincustomeradd.Title = "Edit Customer";
                btnaddCustomer.Content = "Edit Customer";
            }
            if (frmCustomerCenter.Editit == 1)
            {
                loadcompany();
                wincustomeradd.Title = "Edit Customer";
                btnaddCustomer.Content = "Edit Customer";
            }
        }
        private void loadCurrencies()
        {

            CurrencyRepo currencyRepo = new CurrencyRepo();
            List<Currency> currencies = currencyRepo.getAll();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (Currency cur in currencies)
            {
                cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
            }
            cmbCurrency.ItemsSource = cmbitems;
        }
        private void cmbCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
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
        private void lookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }

        }

        private void chkissubsidroy_Checked(object sender, RoutedEventArgs e)
        {
            lblcustomername.Visibility = Visibility.Visible;
            lookupCustomer.Visibility = Visibility.Visible;
            loadcustomers();

        }
        public void loadcustomers()
        {

            lookupCustomer.ItemsSource = compRepo.getUserCustomers(MainWindow.currentUserid);

        }

        private void chkissubsidroy_Unchecked(object sender, RoutedEventArgs e)
        {
            lblcustomername.Visibility = Visibility.Hidden;
            lookupCustomer.Visibility = Visibility.Hidden;
        }
        public void loadMultipleIndustryType()
        {
            List<IndustryType> industryTypes = new List<IndustryType>();
            industryTypes = compRepo.GetIndustryTypes();
            lookupIndustry.ItemsSource = industryTypes;
        }

        public void loadIndustryTypes()
        {
            List<IndustryType> industryTypes = new List<IndustryType>();
            industryTypes = compRepo.GetIndustryTypes();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (IndustryType industry in industryTypes)
            {
                cmbitems.Add(new cmbitem() { name = industry.name, id = industry.Id });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbIndustry.ItemsSource = cmbitems;
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

        private void Wincustomeradd_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void BtnAddmorePhoneNum_Click(object sender, RoutedEventArgs e)
        {

            if (grdPhone.Visibility == Visibility.Collapsed)
            {
                grdPhone.Visibility = Visibility.Visible;
            }
            else
            {
                grdPhone.Visibility = Visibility.Collapsed;
            }
        }
        private void BtnAddmoreEmails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdEmail.Visibility == Visibility.Collapsed)
                {
                    grdEmail.Visibility = Visibility.Visible;
                }
                else
                {
                    grdEmail.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void LookupIndustry_PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            try
            {
                industryTypes = new List<IndustryType>();
                var grd = lookupIndustry.GetGridControl();
                string industryName = "";
                if (grd.SelectedItems.Count != 0)
                {
                    foreach (IndustryType _item in grd.SelectedItems)
                    {
                        industryName = industryName + " | " + _item.name;
                        industryTypes.Add(_item);
                    }
                }

                lookupIndustry.EditValue = industryName;
                lookupIndustry.DisplayMember = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PART_GridControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //loadMultipleIndustryType();

                var grd = lookupIndustry.GetGridControl();
                if (industryTypes.Count != 0)
                    grd.SelectedItems = industryTypes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frmIndustryTypeAdd industryadd = new frmIndustryTypeAdd();
                industryadd.ShowDialog();
                loadMultipleIndustryType();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //private void LookUpCountry_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if(txtCountry.Text != "")
        //        {
        //            if (txtCountry.SelectedIndex > -1)
        //            {
        //                txtCity.Text = "";
        //                txtCity.ItemsSource = null;
        //                txtState.Text = "";
        //                txtState.ItemsSource = null;
        //                var selectedCountry = txtCountry.SelectedItem as string;
        //                if (selectedCountry != null)
        //                {
        //                    var listofCity = compRepo.GetCityListByCountry(selectedCountry);
        //                    var listOfState = compRepo.GetStateListByCountry(selectedCountry);
        //                    txtCity.ItemsSource = listofCity;
        //                    txtState.ItemsSource = listOfState;
        //                }
        //        }
        //    }


        //    }
        //    catch (Exception ex)
        //    {

        //        DXMessageBox.Show(ex.Message);
        //    }
        //}

        //private void LookUpCountry_Loaded(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var listOfCountry = compRepo.GetCountryList();

        //        txtCountry.ItemsSource = listOfCountry;
        //    }
        //    catch (Exception ex)
        //    {

        //        DXMessageBox.Show(ex.Message);
        //    }
        //}


        private void TxtRegionbil_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var listofRegion = compRepo.GetRegionList();

                txtRegionbil.ItemsSource = listofRegion;
                txtRegionship.ItemsSource = listofRegion;

                //txtCountry.IsEnabled = false;
                //txtCity.IsEnabled = false;
                //txtsState.IsEnabled = false;

                //txtsCountry.IsEnabled = false;
                //txtsCity.IsEnabled = false;
                //txtState.IsEnabled = false;

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtRegionbil_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtRegionbil.Text))
                {
                    txtCountry.Text = "";
                    txtState.Text = "";
                    txtCity.Text = "";
                    txtCountry.ItemsSource = null;
                    txtCity.ItemsSource = null;
                    txtState.ItemsSource = null;
                    //txtCountry.IsEnabled = false;
                    //txtCity.IsEnabled = false;
                    //txtState.IsEnabled = false;

                    string selectedItem = txtRegionbil.Text;
                    if (selectedItem != null)
                    {
                        var listofCountry = compRepo.GetCountryListByRegion(selectedItem);

                        txtCountry.ItemsSource = listofCountry;
                        //txtCountry.IsEnabled = true;
                    }
                    else
                    {
                        txtCountry.ItemsSource = null;
                        //txtCountry.IsEnabled = false;
                    }
                }
                else
                {
                    txtCountry.Text = "";
                    txtState.Text = "";
                    txtCity.Text = "";
                    txtCountry.ItemsSource = null;
                    txtCity.ItemsSource = null;
                    txtState.ItemsSource = null;
                    //txtCountry.IsEnabled = false;
                    //txtCity.IsEnabled = false;
                    //txtState.IsEnabled = false;
                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtCountry_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCountry.Text) && !string.IsNullOrEmpty(txtRegionbil.Text))
                {
                    txtState.Text = "";
                    txtCity.Text = "";
                    txtCity.ItemsSource = null;
                    txtState.ItemsSource = null;
                    //txtCity.IsEnabled = false;
                    //txtState.IsEnabled = false;
                    string selectedItem = txtCountry.Text;
                    string selecteRegion = txtRegionbil.Text;
                    if (selectedItem != null && selecteRegion != null)
                    {
                        var listofState = compRepo.GetStateListByCountryRegion(selectedItem, selecteRegion);

                        txtState.ItemsSource = listofState;
                        txtState.IsEnabled = true;
                    }
                    else
                    {
                        txtState.ItemsSource = null;
                        //txtState.IsEnabled = true;
                    }
                }
                else
                {
                    //txtState.ItemsSource = null;
                    //txtState.IsEnabled = false;

                    txtState.Text = "";
                    txtCity.Text = "";
                    txtCity.ItemsSource = null;
                    txtState.ItemsSource = null;
                    //txtCity.IsEnabled = false;
                    //txtState.IsEnabled = false;

                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtState_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtCountry.Text) && !string.IsNullOrEmpty(txtRegionbil.Text) && !string.IsNullOrEmpty(txtState.Text))
                {
                    txtCity.Text = "";
                    txtCity.ItemsSource = null;
                    //txtCity.IsEnabled = false;
                    string selectedItem = txtCountry.Text;
                    string selecteRegion = txtRegionbil.Text;
                    string selecteState = txtState.Text;
                    if (selectedItem != null && selecteRegion != null && selecteState != null)
                    {
                        var listofCities = compRepo.GetCityListByCountryRegionState(selectedItem, selecteRegion, selecteState);


                        txtCity.ItemsSource = listofCities;
                        txtCity.IsEnabled = true;
                    }
                    else
                    {
                        txtCity.ItemsSource = null;
                        // txtCity.IsEnabled = true;
                    }
                }
                else
                {
                    //txtCity.ItemsSource = null;
                    //txtCity.IsEnabled = false;
                    txtCity.Text = "";
                    txtCity.ItemsSource = null;
                    //  txtCity.IsEnabled = false;

                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        //Shipment Address Start
        private void TxtRegionship_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtRegionship.Text))
                {
                    txtsCountry.Text = "";
                    txtsState.Text = "";
                    txtsCity.Text = "";


                    txtsCountry.ItemsSource = null;
                    txtsCity.ItemsSource = null;
                    txtsState.ItemsSource = null;
                    //txtsCountry.IsEnabled = false;
                    //txtsCity.IsEnabled = false;
                    //txtsState.IsEnabled = false;

                    string selectedItem = txtRegionship.Text;
                    if (selectedItem != null)
                    {
                        var listofCountry = compRepo.GetCountryListByRegion(selectedItem);

                        txtsCountry.ItemsSource = listofCountry;
                        // txtsCountry.IsEnabled = true;
                    }
                    else
                    {
                        txtsCountry.ItemsSource = null;
                        // txtsCountry.IsEnabled = false;
                    }
                }
                else
                {
                    txtsCountry.Text = "";
                    txtsState.Text = "";
                    txtsCity.Text = "";
                    txtsCountry.ItemsSource = null;
                    txtsCity.ItemsSource = null;
                    txtsState.ItemsSource = null;
                    //txtsCountry.IsEnabled = false;
                    //txtsCity.IsEnabled = false;
                    //txtsState.IsEnabled = false;
                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtsCountry_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtsCountry.Text) && !string.IsNullOrEmpty(txtRegionship.Text))
                {
                    txtsState.Text = "";
                    txtsCity.Text = "";


                    txtsCity.ItemsSource = null;
                    txtsState.ItemsSource = null;
                    //txtsCity.IsEnabled = false;
                    //txtsState.IsEnabled = false;

                    string selectedItem = txtsCountry.Text;
                    string selecteRegion = txtRegionship.Text;
                    if (selectedItem != null && selecteRegion != null)
                    {
                        var listofState = compRepo.GetStateListByCountryRegion(selectedItem, selecteRegion);

                        txtsState.ItemsSource = listofState;
                        //  txtsState.IsEnabled = true;
                    }
                    else
                    {
                        txtsState.ItemsSource = null;
                        // txtsState.IsEnabled = true;
                    }
                }
                else
                {
                    txtsState.Text = "";
                    txtsCity.Text = "";
                    txtsCity.ItemsSource = null;
                    txtsState.ItemsSource = null;
                    //txtsCity.IsEnabled = false;
                    //txtsState.IsEnabled = false;

                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtsState_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtsCountry.Text) && !string.IsNullOrEmpty(txtRegionship.Text) && !string.IsNullOrEmpty(txtsState.Text))
                {
                    txtsCity.Text = "";
                    txtsCity.ItemsSource = null;
                    //txtsCity.IsEnabled = false;

                    string selectedItem = txtsCountry.Text;
                    string selecteRegion = txtRegionship.Text;
                    string selecteState = txtsState.Text;
                    if (selectedItem != null && selecteRegion != null && selecteState != null) //Check null text
                    {
                        var listofCities = compRepo.GetCityListByCountryRegionState(selectedItem, selecteRegion, selecteState);


                        txtsCity.ItemsSource = listofCities;
                        // txtsCity.IsEnabled = true;
                    }
                    else
                    {
                        txtsCity.ItemsSource = null;
                        // txtsCity.IsEnabled = true;
                    }
                }
                else
                {
                    txtsCity.Text = "";
                    txtsCity.ItemsSource = null;
                    // txtsCity.IsEnabled = false;

                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        //KeyUp events for billing

        private void TxtRegionbil_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    txtRegionbil.IsPopupOpen = false;
                }
                else
                {
                    txtRegionbil.FilterCondition = DevExpress.Data.Filtering.FilterCondition.Like;
                    txtRegionbil.IsPopupOpen = true;
                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtCountry_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    txtCountry.IsPopupOpen = false;
                }
                else
                {
                    txtCountry.IsPopupOpen = true;
                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtState_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    txtState.IsPopupOpen = false;
                }
                else
                {
                    txtState.IsPopupOpen = true;
                }


            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }


        private void TxtCity_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    txtCity.IsPopupOpen = false;
                }
                else
                {
                    txtsCity.FilterCondition = DevExpress.Data.Filtering.FilterCondition.Contains;
                    txtCity.IsPopupOpen = true;
                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }
        //KeyUp events for Shipping
        private void TxtRegionship_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    txtRegionship.IsPopupOpen = false;
                }
                else
                {
                    txtRegionship.IsPopupOpen = true;
                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtsCountry_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    txtsCountry.IsPopupOpen = false;
                }
                else
                {
                    txtsCountry.IsPopupOpen = true;
                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtsState_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    txtsState.IsPopupOpen = false;
                }
                else
                {

                    txtsState.IsPopupOpen = true;
                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void TxtsCity_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    txtsCity.IsPopupOpen = false;
                }
                else
                {

                    txtsCity.IsPopupOpen = true;
                }

            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
        }

        private void chkisParent_Checked(object sender, RoutedEventArgs e)
        {
            loadcustomers();
        }

        private void ImgLeftToRightComp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightComp.Width = 28;

        }

        private void ImgRightToLeftComp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftComp.Width = 28;
        }
        private void ImgLeftToRightDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightDept.Width = 28;

        }

        private void ImgRightToLeftDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftDept.Width = 28;
        }

        private void ImgRightToLeftComp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftComp.Width = 30;
            if (gridSelectedCompanies.SelectedItem != null)
            {
                var company = gridSelectedCompanies.SelectedItem as Company;

                if (company.departments.Intersect(selectedDepartments).Count() > 0)
                {
                    DXMessageBox.Show("Kindly remove the Departments of this Company from Selected Departments!");
                    return;
                }
                selectedCompanies.Remove(company);
                allCompanies.Add(company);

                gridCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;

                var depts = selectedDepartments.Except(allDepartments.Intersect(selectedDepartments));
                allDepartments = new List<Department>();
                foreach (var _cmpny in selectedCompanies)
                {
                    allDepartments.AddRange(_cmpny.departments);
                }


                allDepartments = allDepartments.Except(depts).ToList();

                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                gridDepartment.ItemsSource = null;
                gridDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = null;
                gridSelectedDepartments.ItemsSource = selectedDepartments;
            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }
        private void ImgLeftToRightDept_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightDept.Width = 30;
            if (gridDepartment.SelectedItem != null)
            {
                var department = gridDepartment.SelectedItem as Department;
                if (allDepartments.Find(x => x.ParentID == department.Id) == null)
                {
                    allDepartments.Remove(department);
                    if (!selectedDepartments.Contains(department))
                        selectedDepartments.Add(department);
                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (allDepartments.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedDepartments.Contains(parent))
                            {
                                selectedDepartments.Add(parent);
                            }
                            var findParet = allDepartments.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                allDepartments.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    gridDepartment.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;
                    gridDepartment.ItemsSource = allDepartments;
                    gridSelectedDepartments.ItemsSource = selectedDepartments;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Department which you want to Insert to Selected Departments!");
            }
        }
        private void ImgRightToLeftDept_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftDept.Width = 28;

            if (gridSelectedDepartments.SelectedItem != null)
            {
                var department = gridSelectedDepartments.SelectedItem as Department;

                if(customer.Id!=0)
                {
                    var check= compRepo.CheckOrders(department.Id, customer.Id);
                    if(check==true)
                    {
                        DXMessageBox.Show("Unable to move this department from selected section", "Department has Orders", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }


                if (selectedDepartments.Find(x => x.ParentID == department.Id) == null)
                {
                    selectedDepartments.Remove(department);
                    if (!allDepartments.Contains(department))
                        allDepartments.Add(department);
                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (selectedDepartments.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!allDepartments.Contains(parent))
                                allDepartments.Add(parent);
                            var findParet = selectedDepartments.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                selectedDepartments.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    gridDepartment.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;
                    gridDepartment.ItemsSource = allDepartments;
                    gridSelectedDepartments.ItemsSource = selectedDepartments;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Department which you want to Remove from Selected Departments!");
            }
        }
        private void ImgLeftToRightComp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightComp.Width = 30;
            if (gridCompany.SelectedItem != null)
            {
                var company = gridCompany.SelectedItem as Company;
                selectedCompanies.Add(company);
                allCompanies.Remove(company);
                gridCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;

                allDepartments.AddRange(company.departments.Except(selectedDepartments));

                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                gridDepartment.ItemsSource = null;
                gridDepartment.ItemsSource = allDepartments;
            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }
        public List<Company> GetSelectedCompanies()
        {
            List<Company> companies = new List<Company>();
            var grdCompanies = gridSelectedCompanies.ItemsSource as List<Company>;
            foreach (Company company in grdCompanies)
            {
                companies.Add(company);
            }
            return companies;
        }
        public List<Department> GetSelectedDepartments()
        {
            List<Department> departments = new List<Department>();
            var grdDepartments = gridSelectedDepartments.ItemsSource as List<Department>;
            foreach (Department department in grdDepartments)
            {
                departments.Add(department);
            }
            return departments;
        }
        public List<Department> GetDisableDepartments()
        {
            List<Department> departments = new List<Department>();
                var grdDepartments = gridDisableDepartments.ItemsSource as List<Department>;
            if (grdDepartments != null)
            {
                foreach (Department department in grdDepartments)
                {
                    departments.Add(department);
                }
                return departments;
            }
            else
            {
                return null;
            }
        }

        private void imgRightToLeftDeptDisable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftDept.Width = 28;
        }

        private void imgRightToLeftDeptDisable_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftDeptDisable.Width = 28;

            if (gridDisableDepartments.SelectedItem != null)
            {
                var department = gridDisableDepartments.SelectedItem as Department;
                //if (customer.disableDepartments.Count > 0)
                //{
                //   var disableDept= customer.disableDepartments.Find(x => x.Id == department.Id);
                //    if(disableDept!=null)
                //    {
                //        DXMessageBox.Show("Cannot move disable department");
                //        return;
                //    }
                //} 
                if (selectedDepartments.Find(x => x.ParentID == department.Id) == null)
                {
                    disableDepartments.Remove(department);
                    if (!selectedDepartments.Contains(department))
                        selectedDepartments.Add(department);
                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (disableDepartments.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedDepartments.Contains(parent))
                                selectedDepartments.Add(parent);
                            var findParet = disableDepartments.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                disableDepartments.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    gridDisableDepartments.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = selectedDepartments;
                    gridDisableDepartments.ItemsSource = disableDepartments;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Department which you want to Remove from Disable Departments!");
            }
        }

        private void imgLeftToRightDeptDisable_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightDeptDisable.Width = 30;
            if (gridSelectedDepartments.SelectedItem != null)
            {
                var department = gridSelectedDepartments.SelectedItem as Department;
                if (selectedDepartments.Find(x => x.ParentID == department.Id) == null)
                {
                    selectedDepartments.Remove(department);
                    if (!disableDepartments.Contains(department))
                        disableDepartments.Add(department);
                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (selectedDepartments.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!disableDepartments.Contains(parent))
                            {
                                disableDepartments.Add(parent);
                            }
                            var findParet = selectedDepartments.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                selectedDepartments.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    gridDisableDepartments.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = selectedDepartments;
                    gridDisableDepartments.ItemsSource = disableDepartments;

                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Department which you want to Insert to Disable Departments!");
            }
        }

        private void imgLeftToRightDeptDisable_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightDeptDisable.Width = 28;
        }
    }
}
