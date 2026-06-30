using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.VATBook;
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

namespace ZAS_ERP.VATBook.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmVATReferenceNo.xaml
    /// </summary>
    public partial class ucFrmVATReferenceNo : UserControl
    {
        public bool editFlag = false;
        public VATBookRefNumber vatBookRefNumbers = new VATBookRefNumber();
        public VATBookRepo vatBookRepo = new VATBookRepo();
        public ucFrmVATReferenceNo()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                cmbxCompany.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtVATBookRefNo.Text))
            {
                DXMessageBox.Show("Please enter VATBook Ref Number!");
                txtVATBookRefNo.Focus();
                return;
            }
            vatBookRefNumbers.VATBookReferenceNo = txtVATBookRefNo.Text;
            vatBookRefNumbers.companyId = (cmbxCompany.SelectedItem as Company).Id;
            vatBookRefNumbers.isActive = chkIsActive.IsChecked.Value;

            if (editFlag == false)
            {
                vatBookRepo.AddVATBookReferenceNo(vatBookRefNumbers);
                DXMessageBox.Show("Added Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (editFlag == true)
            {
                vatBookRepo.UpdateVATBookReferenceNo(vatBookRefNumbers);
                DXMessageBox.Show("Updated Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadCompanies();

            if (editFlag == true)
            {

                //Select Company
                var companyList = (cmbxCompany.ItemsSource as List<Company>) == null ? new List<Company>() : cmbxCompany.ItemsSource as List<Company>;
                if (vatBookRefNumbers.company != null)
                {
                    int index = 0;
                    foreach (var _company in companyList)
                    {
                        if (_company.Id == vatBookRefNumbers.companyId)
                        {
                            cmbxCompany.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (vatBookRefNumbers.VATBookReferenceNo != null)
                {
                    txtVATBookRefNo.Text = vatBookRefNumbers.VATBookReferenceNo;
                }

                chkIsActive.IsChecked = vatBookRefNumbers.isActive;
            }
        }

        private void loadCompanies()
        {
            CompanyRepo companyRepo = new CompanyRepo();
            cmbxCompany.ItemsSource = companyRepo.GetActiveCompanies();
        }
    }
}
