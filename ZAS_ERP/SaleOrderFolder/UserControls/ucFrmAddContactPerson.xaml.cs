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
using ZAS_ERP.SaleOrderFolder.Windows;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddContactPerson.xaml
    /// </summary>
    public partial class ucFrmAddContactPerson : UserControl
    {
        SalesReceiptRepo repo = new SalesReceiptRepo();
        List<Bank> allBanks = new List<Bank>();
        public UcListWindow addCntctPrsnWin = new UcListWindow();
        public UcListWindow updateCntctPrsnWin = new UcListWindow();
        public Bank selectedBank = new Bank();

        public ContactPerson contactPerson = new ContactPerson();
        public int addEditFlag = 0;
        public int contactPersonId;

        public ucFrmAddContactPerson()
        {
            InitializeComponent();

            try
            {
                addCntctPrsnWin.Closing += AddCntctPrsn_Window_Closing;
                updateCntctPrsnWin.Closing += UpdateCntctPrsn_Window_Closing;

                
            }
            catch
            {

            }

            //cmbxBanks.SelectedIndex = bankSelectedIndex;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //allBanks = repo.GetAllBank();
            //foreach (var _bank in allBanks)
            //{
            //    cmbxBanks.Items.Add(_bank.BankName + " (" + _bank.BranchCode + ")");
            //}



            var allBanks = repo.GetAllBranches();

            if(allBanks != null)
            {
                List<cmbitem> cmbitems = new List<cmbitem>();

                foreach (var _bank in allBanks)
                {

                    cmbitems.Add(new cmbitem() { name = _bank.BankName + " (" + _bank.BranchCode + ")", id = _bank.Id });


                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Banks") != null)
                    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

                cmbxBanks.ItemsSource = cmbitems;
            }

            if(addEditFlag == 0 && contactPerson.Id == 0)
            {
                    var bankSource = (List<cmbitem>)cmbxBanks.Items.SourceCollection;
                    cmbxBanks.SelectedItem = cmbxBanks.Items[cmbxBanks.Items.IndexOf(bankSource.Find(x => x.id == selectedBank.Id))];
            }
            

            if (addEditFlag == 1 && contactPerson.Id > 0)
            {

                if (contactPerson.bank.Id != 0 || contactPerson.bank != null)
                {
                    var bankSource = (List<cmbitem>)cmbxBanks.Items.SourceCollection;
                    cmbxBanks.SelectedItem = cmbxBanks.Items[cmbxBanks.Items.IndexOf(bankSource.Find(x => x.id == contactPerson.bank.Id))];
                }

                txtPersonName.Text = contactPerson.person.FName;
                txtCnic.Text = contactPerson.person.CNIC;
                txtDOB.DateTime = (DateTime)contactPerson.person.DOB;
                txtDesignation.Text = contactPerson.designation;
                txtPhone.Text = contactPerson.contact.ContactNo;
                txtMobile.Text = contactPerson.contact.SecondaryContact;
                txtEmail.Text = contactPerson.contact.Email;
                chkActive.IsChecked = contactPerson.isActive;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(addEditFlag == 0)
                {
                    Bank bank = new Bank();
                    bank = repo.GetBank((cmbxBanks.SelectedItem as cmbitem).id);
                    //bank = allBanks[cmbxBanks.SelectedIndex];

                    Person person = new Person();
                    person.FName = txtPersonName.Text;
                    person.DOB = txtDOB.DateTime;
                    person.CNIC = txtCnic.Text;

                    Contact contact = new Contact();
                    contact.ContactNo = txtPhone.Text;
                    contact.SecondaryContact = txtMobile.Text;
                    contact.Email = txtEmail.Text;



                    ContactPerson contactPerson = new ContactPerson();
                    contactPerson.person = person;
                    contactPerson.contact = contact;
                    contactPerson.designation = txtDesignation.Text;
                    contactPerson.bank = bank;

                    if(chkActive.IsChecked == true)
                        contactPerson.isActive = true;
                    else
                        contactPerson.isActive = false;

                    repo.addBankContactPerson(bank.Id, contactPerson);


                    MessageBox.Show("Successfully Added.");
                    addCntctPrsnWin.Close();
                }
                else
                {
                    Bank bank = new Bank();
                    bank = repo.GetBank((cmbxBanks.SelectedItem as cmbitem).id);
                    //bank = allBanks[cmbxBanks.SelectedIndex];

                    Person person = new Person();
                    person.FName = txtPersonName.Text;
                    person.DOB = txtDOB.DateTime;
                    person.CNIC = txtCnic.Text;

                    Contact contact = new Contact();
                    contact.ContactNo = txtPhone.Text;
                    contact.SecondaryContact = txtMobile.Text;
                    contact.Email = txtEmail.Text;



                    ContactPerson cntctPrsn = new ContactPerson();
                    cntctPrsn.Id = contactPerson.Id;
                    cntctPrsn.bank = bank;
                    cntctPrsn.person = person;
                    cntctPrsn.contact = contact;
                    cntctPrsn.designation = txtDesignation.Text;
                    

                    if (chkActive.IsChecked == true)
                        cntctPrsn.isActive = true;
                    else
                        cntctPrsn.isActive = false;

                    repo.updateContactPerson(cntctPrsn);


                    MessageBox.Show("Successfully Updated.");
                    updateCntctPrsnWin.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem adding contact person.");
            }
            
        }

        private void AddCntctPrsn_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            e.Cancel = false;
        }

        private void UpdateCntctPrsn_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            e.Cancel = false;
        }

        private void CmbxBanks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
