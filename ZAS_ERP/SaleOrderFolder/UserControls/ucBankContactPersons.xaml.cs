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

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucBankContactPersons.xaml
    /// </summary>
    public partial class ucBankContactPersons : UserControl
    {
        SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
        public Bank bank = new Bank();
        ucFrmAddContactPerson updateCntctPrsnObj = new ucFrmAddContactPerson();

        public ucBankContactPersons()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblHeading.Text = bank.BankName+" Contact Person List";
            grdContactPersonsGrid.ItemsSource = bank.contactPersons;
            grdContactPersonsList.ItemsSource = bank.contactPersons;

            //Add New Contact Person menu Item permission
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Contact Person") != null)
            {
                mbtnAddContPerson.IsEnabled = true;
                btnAddContactPerson.IsEnabled = true;
            }
            else
            {
                mbtnAddContPerson.IsEnabled = false;
                btnAddContactPerson.IsEnabled = false;
            }

            //Update Existing Contact Person menu Item permission
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update Existing Contact Person") != null)
            {
                mbtnupdatecontperson.IsEnabled = true;
                btnEditContactPerson.IsEnabled = true;
            }
            else
            {
                mbtnupdatecontperson.IsEnabled = false;
                btnEditContactPerson.IsEnabled = false;
            }
        }

        private void GrdContactPerson_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void Add_New_ContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Contact Person") != null)
                {
                    
                        ucFrmAddContactPerson addCntctPrsnObj = new ucFrmAddContactPerson();

                        //addCntctPrsnObj.cmbxBanks.SelectedIndex = selectedRow.SerialNo - 1;
                        addCntctPrsnObj.selectedBank = bank;


                        addCntctPrsnObj.addEditFlag = 0;
                        addCntctPrsnObj.addCntctPrsnWin.Title = "Add Contact Person";
                        addCntctPrsnObj.addCntctPrsnWin.Content = addCntctPrsnObj;
                        addCntctPrsnObj.addCntctPrsnWin.Height = 400;
                        addCntctPrsnObj.addCntctPrsnWin.Width = 700;
                        addCntctPrsnObj.addCntctPrsnWin.ResizeMode = ResizeMode.CanMinimize;
                        addCntctPrsnObj.addCntctPrsnWin.ShowDialog();                    
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Contact Person!");
                    return;
                }
            }
            catch
            {

            }
        }

        private void Edit_ContactClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update Existing Contact Person") != null)
                {
                    ContactPerson contactPerson = new ContactPerson();

                    if(grdContactPersonsList.IsVisible == true)
                        contactPerson = (ContactPerson)grdContactPersonsList.SelectedItem;
                    else if(grdContactPersonsGrid.IsVisible == true)
                        contactPerson = (ContactPerson)grdContactPersonsGrid.SelectedItem;


                    if (contactPerson != null)
                    {
                        SalesReceiptRepo repo = new SalesReceiptRepo();
                        var cntctPrsn = repo.GetContactPerson(contactPerson.Id);
                        updateCntctPrsnObj = new ucFrmAddContactPerson();

                        updateCntctPrsnObj.contactPerson = cntctPrsn;

                        //updateCntctPrsnObj.txtPersonName.Text = contactPerson.person.FName;
                        //updateCntctPrsnObj.txtCnic.Text = contactPerson.person.CNIC;
                        //updateCntctPrsnObj.txtDOB.DateTime = contactPerson.person.DOB;
                        //updateCntctPrsnObj.txtDesignation.Text = contactPerson.designation;
                        //updateCntctPrsnObj.txtPhone.Text = contactPerson.contact.ContactNo;
                        //updateCntctPrsnObj.txtMobile.Text = contactPerson.contact.SecondaryContact;
                        //updateCntctPrsnObj.txtEmail.Text = contactPerson.contact.Email;
                        //updateCntctPrsnObj.chkActive.IsChecked = contactPerson.isActive;

                        updateCntctPrsnObj.addEditFlag = 1;
                        updateCntctPrsnObj.updateCntctPrsnWin.Title = "Update Contact Person";
                        updateCntctPrsnObj.updateCntctPrsnWin.Content = updateCntctPrsnObj;
                        updateCntctPrsnObj.updateCntctPrsnWin.Height = 400;
                        updateCntctPrsnObj.updateCntctPrsnWin.Width = 700;
                        updateCntctPrsnObj.updateCntctPrsnWin.ResizeMode = ResizeMode.CanMinimize;
                        updateCntctPrsnObj.updateCntctPrsnWin.ShowDialog();


                    }
                }
            }
            catch
            {

            }
        }

        private void MbtnAllContacts_Click(object sender, RoutedEventArgs e)
        {
            if (bank != null)
            {
                grdContactPersonsGrid.ItemsSource = bank.contactPersons;
                grdContactPersonsList.ItemsSource = bank.contactPersons;
            }
        }

        private void MbtnActiveContacts_Click(object sender, RoutedEventArgs e)
        {
            if(bank != null)
            {
                grdContactPersonsGrid.ItemsSource = bank.contactPersons.Where(x => x.isActive == true);
                grdContactPersonsList.ItemsSource = bank.contactPersons.Where(x => x.isActive == true);
            }
            //grdContactPersons.ItemsSource = receiptRepo.GetActiveContactPersonByBankId(bank.Id);

        }

        private void MbtnInActiveContacts_Click(object sender, RoutedEventArgs e)
        {
            if (bank != null)
            {
                grdContactPersonsGrid.ItemsSource = bank.contactPersons.Where(x => x.isActive == false);
                grdContactPersonsList.ItemsSource = bank.contactPersons.Where(x => x.isActive == false);
            }
            //grdContactPersons.ItemsSource = receiptRepo.GetInActiveContactPersonByBankId(bank.Id);
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (bank != null && bank.Id > 0)
            {
                receiptRepo = new SalesReceiptRepo();
                bank = receiptRepo.GetBank(bank.Id);
                grdContactPersonsGrid.ItemsSource = bank.contactPersons;
                grdContactPersonsList.ItemsSource = bank.contactPersons;
            }
        }

        private void MbtnListView_Click(object sender, RoutedEventArgs e)
        {
            grdContactPersonsList.Visibility = Visibility.Visible;
            grdContactPersonsGrid.Visibility = Visibility.Hidden;
        }

        private void MbtnGridView_Click(object sender, RoutedEventArgs e)
        {
            grdContactPersonsList.Visibility = Visibility.Hidden;
            grdContactPersonsGrid.Visibility = Visibility.Visible;
        }
    }
}
