using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.Procurementss.Billss.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmVendorBillRef.xaml
    /// </summary>
    public partial class ucFrmVendorBillRef : UserControl
    {
        public bool editFlag = false;
        public VendorBillReference billRef = new VendorBillReference();
        public BillRepo billsRepo = new BillRepo();

        public ucFrmVendorBillRef()
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
            if (String.IsNullOrEmpty(txtBillRefNo.Text))
            {
                DXMessageBox.Show("Please enter Bill Ref Number!");
                txtBillRefNo.Focus();
                return;
            }
            billRef.Reference = txtBillRefNo.Text;
            billRef.companyId = (cmbxCompany.SelectedItem as Company).Id;
            billRef.isActive = chkIsActive.IsChecked.Value;

            if (editFlag == false)
            {
                billsRepo.AddBillReferenceNo(billRef);
                DXMessageBox.Show("Added Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (editFlag == true)
            {
                billsRepo.UpdateBillReferenceNo(billRef);
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
                if (billRef.company != null)
                {
                    int index = 0;
                    foreach (var _company in companyList)
                    {
                        if (_company.Id == billRef.companyId)
                        {
                            cmbxCompany.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (billRef.Reference != null)
                {
                    txtBillRefNo.Text = billRef.Reference;
                }
                chkIsActive.IsChecked = billRef.isActive;
            }
        }

        private void loadCompanies()
        {
            CompanyRepo companyRepo = new CompanyRepo();
            cmbxCompany.ItemsSource = companyRepo.GetActiveCompanies();
        }
    }
}
