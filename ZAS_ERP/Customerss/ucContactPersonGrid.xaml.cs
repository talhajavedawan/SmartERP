using ERP_BL.Databases;
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
    /// Interaction logic for ucContactPersonGrid.xaml
    /// </summary>
    public partial class ucContactPersonGrid : UserControl
    {
        ucContactPerson ucContactPerson = new ucContactPerson();
        CustomerCompany customerCompany = new CustomerCompany();
        CustomerCompRepo CustomerCompRepo = new CustomerCompRepo();
        public ucContactPersonGrid()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdContactPersonsList);
        }
        private void GrdContactPersonsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Update Contact Person Detail") != null))
                {
                    //var selectedContact = grdContactPersonsGrid.SelectedItem as ContactPerson;
                    var selectedContact = grdContactPersonsList.SelectedItem as ContactPerson;
                    ucContactPerson = new ucContactPerson();
                    if (selectedContact != null)
                    {
                        ucContactPerson.ContactPersonId = selectedContact.Id;
                    }
                    else
                    {
                        return;
                    }

                    ucContactPerson.addCategoryWindow.Height = 450;
                    ucContactPerson.addCategoryWindow.Width = 750;
                    ucContactPerson.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucContactPerson.editFlag = true;
                    ucContactPerson.addCategoryWindow.Content = ucContactPerson;
                    ucContactPerson.addCategoryWindow.ShowDialog();

                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Update Contact Person!");
                    return;
                }
            }
            catch (Exception)
            {


            }
        }

        private void Add_New_ContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Contact Info") != null))
                {
                    //ucContactPersonGrid frmCustomer = new ucContactPersonGrid();
                    // frmCustomerCenter frmCustomer = new frmCustomerCenter();
                    // var selectedItem = grdContactPersonsList.SelectedItem as ContactPerson;
                    var selectedItem = ucCustomerCenterGrid.custId;
                    ucContactPerson = new ucContactPerson();
                    ucContactPerson.ContactPersonId = selectedItem;

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

        private void Edit_ContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Update Contact Person Detail") != null))
                {
                    //var selectedContact = grdContactPersonsGrid.SelectedItem as ContactPerson;
                    var selectedContact = grdContactPersonsList.SelectedItem as ContactPerson;
                    ucContactPerson = new ucContactPerson();
                    if (selectedContact != null)
                    {
                        ucContactPerson.ContactPersonId = selectedContact.Id;
                    }
                    else
                    {
                        return;
                    }

                    ucContactPerson.addCategoryWindow.Height = 450;
                    ucContactPerson.addCategoryWindow.Width = 750;
                    ucContactPerson.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucContactPerson.editFlag = true;
                    ucContactPerson.addCategoryWindow.Content = ucContactPerson;
                    ucContactPerson.addCategoryWindow.ShowDialog();

                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Update Contact Person!");
                    return;
                }
            }
            catch (Exception)
            {


            }
        }

        private void MbtnAddNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Add Customer Contact Info") != null))
                {
                    //ucContactPersonGrid frmCustomer = new ucContactPersonGrid();
                    // frmCustomerCenter frmCustomer = new frmCustomerCenter();
                    // var selectedItem = grdContactPersonsList.SelectedItem as ContactPerson;
                    var selectedItem = ucCustomerCenterGrid.custId;
                    ucContactPerson = new ucContactPerson();
                    ucContactPerson.ContactPersonId = selectedItem;

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

        private void MbtnUpdate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                if ((SYSTEM_STATIC.AllowedPermissions.Find(a => a.Name == "Update Contact Person Detail") != null))
                {
                    //var selectedContact = grdContactPersonsGrid.SelectedItem as ContactPerson;
                    var selectedContact = grdContactPersonsList.SelectedItem as ContactPerson;
                    ucContactPerson = new ucContactPerson();
                    if (selectedContact != null)
                    {
                        ucContactPerson.ContactPersonId = selectedContact.Id;
                    }
                    else
                    {
                        return;
                    }

                    ucContactPerson.addCategoryWindow.Height = 450;
                    ucContactPerson.addCategoryWindow.Width = 750;
                    ucContactPerson.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucContactPerson.editFlag = true;
                    ucContactPerson.addCategoryWindow.Content = ucContactPerson;
                    ucContactPerson.addCategoryWindow.ShowDialog();

                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Update Contact Person!");
                    return;
                }
            }
            catch (Exception)
            {


            }
        }

        private void MbtnSaveLayout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdContactPersonsList);
        }

        private void MbtnRefresh_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            loadActiveContactPerson();
        }
        private void loadContactPerson()
        {
            try
            {

                //var custId = (grdContactPersonsList.SelectedItem as ContactPerson).Id; GetAllContactPersonsByCustomer
               // var contactPerson = CustomerCompRepo.GetAllContactPersons(ucCustomerCenterGrid.custId);
                var contactPerson = CustomerCompRepo.GetAllContactPersonsByCustomer(ucCustomerCenterGrid.custId);
                if (contactPerson != null)
                {
                    //grdContactPersonsGrid.ItemsSource = contactPerson;
                    grdContactPersonsList.ItemsSource = contactPerson;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void loadActiveContactPerson()
        {
            try
            {

                //var custId = (grdContactPersonsList.SelectedItem as ContactPerson).Id; GetAllContactPersonsByCustomer
                // var contactPerson = CustomerCompRepo.GetAllContactPersons(ucCustomerCenterGrid.custId);
                var contactPerson = CustomerCompRepo.GetAllActiveContactPersonsByCustomer(ucCustomerCenterGrid.custId);
                if (contactPerson != null)
                {
                    //grdContactPersonsGrid.ItemsSource = contactPerson;
                    grdContactPersonsList.ItemsSource = contactPerson;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void loadInActiveContactPerson()
        {
            try
            {

                //var custId = (grdContactPersonsList.SelectedItem as ContactPerson).Id; GetAllContactPersonsByCustomer
                // var contactPerson = CustomerCompRepo.GetAllContactPersons(ucCustomerCenterGrid.custId);
                var contactPerson = CustomerCompRepo.GetAllInActiveContactPersonsByCustomer(ucCustomerCenterGrid.custId);
                if (contactPerson != null)
                {
                    //grdContactPersonsGrid.ItemsSource = contactPerson;
                    grdContactPersonsList.ItemsSource = contactPerson;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnAllContact_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            loadContactPerson();
        }

        private void MbtnActiveContact_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            loadActiveContactPerson();
        }

        private void MbtnInactiveContact_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            loadInActiveContactPerson();
        }

     
    }
}
