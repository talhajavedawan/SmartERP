using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ucContactPerson.xaml
    /// </summary>
    public partial class ucContactPerson : UserControl
    {
        CustomerCompRepo customerCompRepo = new CustomerCompRepo();
        public Window addCategoryWindow = new Window();
        public bool editFlag = false;
        public int ContactPersonId;
        CustomerCompany customerCompanys = new CustomerCompany();
        ContactPerson contactPerson = new ContactPerson();
        public ucContactPerson()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var religionList = customerCompRepo.GetAllReligion();
                cmbxReligion.ItemsSource = religionList;
                var lstcompany = customerCompRepo.GetAllCustomerCompanies();
                cmbxCompanies.ItemsSource = lstcompany;
                
                if (ContactPersonId != 0 && editFlag == false) //add new
                {
                    customerCompanys = customerCompRepo.GetCustomerCompany(ContactPersonId);
                    if (customerCompanys.company != null)
                        cmbxCompanies.Text = customerCompanys.company.CompanyName;

                    //contactPerson = customerCompRepo.GetContactPerson(ContactPersonId);
                    //if (contactPerson.customerCompanyId != null)
                    //{
                    //    cmbxCompanies.Text = contactPerson.customerCompany.company.CompanyName;
                    //}
                }
                if (editFlag == true) //update
                    {
                        contactPerson = customerCompRepo.GetContactPerson(ContactPersonId);

                        //if (customerCompany.contactPerson.FName != null)
                        //    txtPersonName.Text = customerCompany.contactPerson.FName;
                        if (contactPerson.customerCompanyId != null)
                        {
                            cmbxCompanies.Text = contactPerson.customerCompany.company.CompanyName;
                        }
                        txtPersonName.Text = contactPerson.person.FName;
                        txtCnic.Text = contactPerson.person.CNIC;
                        txtDOB.EditValue = (DateTime)contactPerson.person.DOB;
                        txtDesignation.Text = contactPerson.designation;
                        txtPhone.Text = contactPerson.contact.ContactNo;
                        txtMobile.Text = contactPerson.contact.SecondaryContact;
                        txtEmail.Text = contactPerson.contact.Email;

                        if (contactPerson.ReligionId != null)
                            cmbxReligion.Text = contactPerson.religion.ReligionName;
                    if (contactPerson.isActive == true)
                        chkIsActive.IsChecked = true;
                    else
                        chkIsActive.IsChecked = false;
                }
                 


               
              
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
          
            
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxCompanies.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please Select Company First!");
                    cmbxCompanies.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtPersonName.Text))
                {
                    DXMessageBox.Show("Please add person Name First!");
                    txtPersonName.Focus();
                    return;
                }
                if (txtCnic.Text.Length != 13)
                {
                    MessageBox.Show("Please insert Valid length of Cnic!");
                    txtCnic.Focus();
                    return;
                }
                if (txtDOB.EditValue == null)
                {
                    MessageBox.Show("Please select your Date of birth!");
                    txtDOB.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtMobile.Text))
                {
                    DXMessageBox.Show("Please add Mobile Number First!");
                    txtMobile.Focus();
                    return;
                }
                if (cmbxReligion.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please Select Religion First!");
                    cmbxReligion.Focus();
                    return;
                }
                contactPerson.customerCompanyId = (cmbxCompanies.SelectedItem as CustomerCompany).Id;
                contactPerson.person = new Person();
                contactPerson.contact = new Contact();

                contactPerson.person.FName = txtPersonName.Text;
                contactPerson.person.CNIC = txtCnic.Text;
                contactPerson.person.DOB = txtDOB.DateTime;

                contactPerson.contact.ContactNo = txtPhone.Text;
                contactPerson.contact.SecondaryContact = txtMobile.Text;


                contactPerson.designation = txtDesignation.Text;
                contactPerson.contact.Email = txtEmail.Text;

                contactPerson.ReligionId = (cmbxReligion.SelectedItem as Religion).Id;

                if (chkIsActive.IsChecked == true)
                    contactPerson.isActive = true;
                else
                    contactPerson.isActive = false;

                if (editFlag == false && contactPerson.Id == 0)
                {
                    customerCompRepo.AddContactPerson(contactPerson);
                    DXMessageBox.Show("Successfully Added!");
                    addCategoryWindow.Close();
                }
                else if (editFlag == true && contactPerson.Id != 0)
                {
                    customerCompRepo.UpdateContactPerson(contactPerson);
                    DXMessageBox.Show("Updated Successfully!");
                    addCategoryWindow.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }
        }

        private void CmbxCompanies_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
          
           
        }

        private void TxtCnic_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]{5}-[0-9]{7}-[0-9]{1}$"); 
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
