using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Customerss
{
    /// <summary>
    /// Interaction logic for ucCustomerCenterGrid.xaml
    /// </summary>
    public partial class ucCustomerCenterGrid : UserControl
    {
        public static int custId;
        public static int Editit;
        public static int customerid;
        ucContactPerson ucContactPerson = new ucContactPerson();
        CustomerCompRepo CustomerCompRepo = new CustomerCompRepo();

        public ucCustomerCenterGrid()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadgrid();
        }
     
        private void loadgrid()
        {

            Editit = 0;

            CustomerCompRepo compRepo = new CustomerCompRepo();

            EmployeeRepo employeeRepo = new EmployeeRepo();
            if (MainWindow.currentUserid == 0)
            {

                this.grdCutomerCompanies.ItemsSource = compRepo.getAll();
                return;

            }
            List<CustomerCompany> customerCompanies = new List<CustomerCompany>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Customers") != null)
            {

                customerCompanies = employeeRepo.GetAllCustomersByUserId(MainWindow.currentUserid);
                this.grdCutomerCompanies.ItemsSource = customerCompanies;
            }
            else
            {
                customerCompanies = employeeRepo.GetActiveCustomersByUserId(MainWindow.currentUserid);
                this.grdCutomerCompanies.ItemsSource = customerCompanies;
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCutomerCompanies);
        }

        private void TreeListView1_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "linkedDepartmentsss" && e.IsGetData)
            {
                var dept = grdCutomerCompanies.GetRow(e.Node.RowHandle) as CustomerCompany;
                string deptNames = "";

                if (dept.departments != null && dept.departments.Count > 0)
                {
                    deptNames = String.Join(" | ", dept.departments.Select(x => x.DeptName));
                }
                e.Value = deptNames;
            }
            
            if (e.Column.FieldName == "linkedCompaniesss" && e.IsGetData)
            {
                var cmpy = grdCutomerCompanies.GetRow(e.Node.RowHandle) as CustomerCompany;
                string cmpyNames = "";

                if (cmpy.Companies != null && cmpy.Companies.Count > 0)
                {
                    cmpyNames = String.Join(" | ", cmpy.Companies.Select(x => x.CompanyName));
                }
                e.Value = cmpyNames;
            }
            if (e.Column.FieldName == "multiIndustry" && e.IsGetData)
            {
                var cmpy = grdCutomerCompanies.GetRow(e.Node.RowHandle) as CustomerCompany;
                string cmpyNames = "";

                if (cmpy.IndustryTypes != null && cmpy.IndustryTypes.Count > 0)
                {
                    cmpyNames = String.Join(" | ", cmpy.IndustryTypes.Select(x => x.name));
                }
                e.Value = cmpyNames;
            }
        }

        private void TreeListView1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            EditCustomer();
        }

        private void btnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            AddCompany();
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            EditCustomer();
        }

        private void Add_New_ContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Contact Info") != null))
                {

                    var selectedItem = grdCutomerCompanies.SelectedItem as CustomerCompany;
                    ucContactPerson = new ucContactPerson();
                    ucContactPerson.ContactPersonId = selectedItem.Id;
                    ucContactPerson.addCategoryWindow.Height = 450;
                    ucContactPerson.addCategoryWindow.Width = 750;
                    ucContactPerson.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucContactPerson.editFlag = false;
                    ucContactPerson.addCategoryWindow.Content = ucContactPerson;
                    ucContactPerson.addCategoryWindow.ShowDialog();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Contact Person!");
                    return;
                }
            }
            catch (Exception)
            {


            }
        }
        private void AddCompany()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Customer") != null)
            {
                frmCustomeradd customeradd = new frmCustomeradd();

                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Info") != null))
                {
                    customeradd.tabCustomerInfo.IsEnabled = true;
                    customeradd.txtBussinesName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Company Name") != null) ? true : false;
                    //customeradd.cmbIndustry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Industry") != null) ? true : false;
                    customeradd.lookupIndustry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Industry") != null) ? true : false;
                    customeradd.cmbBussinesType.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Business Type") != null) ? true : false;
                    customeradd.cmbCurrency.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Currency") != null) ? true : false;
                    customeradd.lookupCustomer.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Company Name") != null) ? true : false;
                    //billing
                    customeradd.txtAdressline1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address line1") != null) ? true : false;
                    customeradd.txtAdressline2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address line2") != null) ? true : false;
                    customeradd.txtCity.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address City") != null) ? true : false;
                    customeradd.txtState.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address State") != null) ? true : false;
                    customeradd.txtCountry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address Country") != null) ? true : false;
                    customeradd.txtZIP.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address Zip") != null) ? true : false;
                    customeradd.txtRegionbil.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Billing address Region") != null) ? true : false;
                    //Shipping
                    customeradd.txtsAdressline1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address line1") != null) ? true : false;
                    customeradd.txtsAdressline2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address line2") != null) ? true : false;
                    customeradd.txtsCity.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address City") != null) ? true : false;
                    customeradd.txtsState.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address State") != null) ? true : false;
                    customeradd.txtsCountry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address Country") != null) ? true : false;
                    customeradd.txtsZIP.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address Zip") != null) ? true : false;
                    customeradd.txtRegionship.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Shipping address Region") != null) ? true : false;
                }
                //else
                //{
                //    DXMessageBox.Show("Permission required to Add new Customer", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                //}


                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Tax Info") != null))
                {
                    customeradd.tabTaxInformationInfo.IsEnabled = true;
                    customeradd.txtVAT.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vat Number") != null) ? true : false;
                    customeradd.txtEIN.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add NTN number") != null) ? true : false;
                    customeradd.txtRegistrationTaxNumber.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Customer sale tax registration Number") != null) ? true : false;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Customer tax info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Contact Info") != null))
                {
                    customeradd.tabContactInfo.IsEnabled = true;
                    //customeradd.tabContactInfo.IsEnabled = true;
                    customeradd.txtfirstName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add First Name") != null) ? true : false;
                    customeradd.txtlastName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Last Name") != null) ? true : false;
                    customeradd.txtPhonenum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Default Phone Number") != null) ? true : false;
                    customeradd.txtFaxnum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add FAX Number") != null) ? true : false;
                    customeradd.txtEmail.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Default Email") != null) ? true : false;
                    customeradd.txtWebsite.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Website") != null) ? true : false;
                    customeradd.txtLink1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Link1") != null) ? true : false;
                    customeradd.txtLink2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Link2") != null) ? true : false;
                    customeradd.txtLink3.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Link3") != null) ? true : false;

                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Customer contact info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Department Info") != null))
                {
                    customeradd.tabDepartmentInfo.IsEnabled = true;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Customer department info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Company Info") != null))
                    customeradd.tabCompanynfo.IsEnabled = true;
                else
                    DXMessageBox.Show("Permission required to Add new Customer company info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);

                Editit = 0;
                customeradd.WindowState = WindowState.Maximized;
                customeradd.ShowDialog();
                // loadgrid();
                grdCutomerCompanies.RefreshData();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Customer", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }
        private void EditCustomer()
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Customer") != null))
            {
                frmCustomeradd customeradd = new frmCustomeradd();
                
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Edit Customer Info") != null))
                {
                    customeradd.tabCustomerInfo.IsEnabled = true;
                    customeradd.txtBussinesName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Company Name") != null) ? true : false;
                    //customeradd.cmbIndustry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Industry") != null) ? true : false;
                    customeradd.lookupIndustry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Industry") != null) ? true : false;
                    customeradd.cmbBussinesType.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Business Type") != null) ? true : false;
                    customeradd.cmbCurrency.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Currency") != null) ? true : false;
                    customeradd.lookupCustomer.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Company Name") != null) ? true : false;
                    //billing
                    customeradd.txtAdressline1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address line1") != null) ? true : false;
                    customeradd.txtAdressline2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address line2") != null) ? true : false;
                    customeradd.txtCity.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address City") != null) ? true : false;
                    customeradd.txtState.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address State") != null) ? true : false;
                    customeradd.txtCountry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address Country") != null) ? true : false;
                    customeradd.txtZIP.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address Zip") != null) ? true : false;
                    customeradd.txtRegionbil.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Billing address Region") != null) ? true : false;
                    //Shipping
                    customeradd.txtsAdressline1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address line1") != null) ? true : false;
                    customeradd.txtsAdressline2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address line2") != null) ? true : false;
                    customeradd.txtsCity.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address City") != null) ? true : false;
                    customeradd.txtsState.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address State") != null) ? true : false;
                    customeradd.txtsCountry.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address Country") != null) ? true : false;
                    customeradd.txtsZIP.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address Zip") != null) ? true : false;
                    customeradd.txtRegionship.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit  Shipping address Region") != null) ? true : false;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Edit Customer Tax Info") != null))
                {
                    customeradd.tabTaxInformationInfo.IsEnabled = true;
                    customeradd.txtVAT.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Vat Number") != null) ? true : false;
                    customeradd.txtEIN.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit NTN number") != null) ? true : false;
                    customeradd.txtRegistrationTaxNumber.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Customer sale tax registration Number") != null) ? true : false;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer tax info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }

                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Edit Customer Contact Info") != null))
                {
                    customeradd.tabContactInfo.IsEnabled = true;
                    customeradd.txtfirstName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit First Name") != null) ? true : false;
                    customeradd.txtlastName.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Last Name") != null) ? true : false;
                    customeradd.txtPhonenum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Default Phone Number") != null) ? true : false;
                    customeradd.txtFaxnum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit FAX Number") != null) ? true : false;
                    customeradd.txtEmail.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Default Email") != null) ? true : false;
                    customeradd.txtWebsite.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Website") != null) ? true : false;
                    customeradd.txtLink1.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Link1") != null) ? true : false;
                    customeradd.txtLink2.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Link2") != null) ? true : false;
                    customeradd.txtLink3.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Link3") != null) ? true : false;

                    //customeradd.btnAddmorePhoneNum.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit More Phone Number") != null) ? true : false;
                    //customeradd.btnAddmoreEmails.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit More Email") != null) ? true : false;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer contact info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Edit Customer Department Info") != null))
                {
                    customeradd.tabDepartmentInfo.IsEnabled = true;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer department info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Edit Customer Company Info") != null))
                {
                    customeradd.tabCompanynfo.IsEnabled = true;
                }
                else
                {
                    DXMessageBox.Show("Permission required to Edit Customer company info", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                Editit = 1;
                if (grdCutomerCompanies.GetFocusedRowCellValue(grdCutomerCompanies.Columns.GetColumnByFieldName("Id")) != null)
                {

                    customerid = (int)grdCutomerCompanies.GetFocusedRowCellValue(grdCutomerCompanies.Columns.GetColumnByFieldName("Id"));

                }
                customeradd.WindowState = WindowState.Maximized;
                customeradd.ShowDialog();
                // loadgrid();
                grdCutomerCompanies.RefreshData();            }
            else
            {
                DXMessageBox.Show("Permission required to edit Customer", "Permission Reqired", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

 

        private void View_contact_person_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "View Contact Detail Info") != null))
            {
                loadActiveContactPerson();
            }
            else
            {
                return;
            }
        }

        private void loadContactPerson()
        {
           
            try
            {
                custId = (grdCutomerCompanies.SelectedItem as CustomerCompany).Id;
                var contactPerson = CustomerCompRepo.GetAllContactPersonsByCustomer(custId);
                if (contactPerson != null)
                {
                    ucContactPersonGrid personGrid = new ucContactPersonGrid(); ;
                    Window win = new Window();
                    win.Content = personGrid;                  
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    personGrid.grdContactPersonsList.ItemsSource = contactPerson;
                    win.WindowState = WindowState.Maximized;
                    win.ShowDialog();
                   
                }
            }
            catch (Exception)
            {


            }
        }
        private void loadActiveContactPerson()
        {

            try
            {
                custId = (grdCutomerCompanies.SelectedItem as CustomerCompany).Id;
                var contactPerson = CustomerCompRepo.GetAllActiveContactPersonsByCustomer(custId);
                if (contactPerson != null)
                {
                    ucContactPersonGrid personGrid = new ucContactPersonGrid(); ;
                    Window win = new Window();
                    win.Content = personGrid;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    personGrid.grdContactPersonsList.ItemsSource = contactPerson;
                    win.WindowState = WindowState.Maximized;
                    win.ShowDialog();

                }
            }
            catch (Exception)
            {


            }
        }
        private void loadInActiveContactPerson()
        {

            try
            {
                custId = (grdCutomerCompanies.SelectedItem as CustomerCompany).Id;
                var contactPerson = CustomerCompRepo.GetAllInActiveContactPersonsByCustomer(custId);
                if (contactPerson != null)
                {
                    ucContactPersonGrid personGrid = new ucContactPersonGrid(); ;
                    Window win = new Window();
                    win.Content = personGrid;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    personGrid.grdContactPersonsList.ItemsSource = contactPerson;
                    win.WindowState = WindowState.Maximized;
                    win.ShowDialog();

                }
            }
            catch (Exception)
            {


            }
        }
        private void MbtnAddNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddCompany();
        }

        private void MbtnUpdate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            EditCustomer();
        }

        private void MbtnSaveLayout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCutomerCompanies);
        }

        private void MbtnRefresh_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            loadgrid();
        }

        private void MbtnAddContactPerson_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Contact Info") != null))
                {

                    var selectedItem = grdCutomerCompanies.SelectedItem as CustomerCompany;
                    ucContactPerson = new ucContactPerson();
                    ucContactPerson.ContactPersonId = selectedItem.Id;
                    ucContactPerson.addCategoryWindow.Height = 450;
                    ucContactPerson.addCategoryWindow.Width = 750;
                    ucContactPerson.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucContactPerson.editFlag = false;
                    ucContactPerson.addCategoryWindow.Content = ucContactPerson;
                    ucContactPerson.addCategoryWindow.ShowDialog();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Contact Person!");
                    return;
                }
            }
            catch (Exception)
            {


            }
        }

        private void MbtnViewContact_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "View Contact Detail Info") != null))
            {
                loadActiveContactPerson();
            }
            else
            {
                return;
            }
        }

        private void BtnCollaps_Click(object sender, RoutedEventArgs e)
        {
            grdCutomerCompanies.ShowLoadingPanel = true;
            treeListView1.CollapseAllNodes();
            grdCutomerCompanies.ShowLoadingPanel = false;
        }

        private void BtnExpand_Click(object sender, RoutedEventArgs e)
        {
            grdCutomerCompanies.ShowLoadingPanel = true;
            treeListView1.ExpandAllNodes();
            grdCutomerCompanies.ShowLoadingPanel = false;
        }

       

       
    }
}
