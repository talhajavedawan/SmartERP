using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Tax;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucBillData.xaml
    /// </summary>
    public partial class ucBillData : UserControl
    {
        AdminBillsRepo billsRepo = new AdminBillsRepo();

        public List<Adjustment> adjustments = new List<Adjustment>();
        public ucBillData()
        {
            InitializeComponent();
            datBillingMonth.Mask = CultureInfo.CurrentCulture.DateTimeFormat.YearMonthPattern;
        }

        private void TxtMER_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void UcBillDataRow_Loaded(object sender, RoutedEventArgs e)
        {
            //loadTaxes();
        }

        private void loadTaxes()
        {
            TaxRepo taxRepo = new TaxRepo();
            var taxes = taxRepo.getAllTaxes();
            lookupVat.ItemsSource = taxes;
        }

        private void loadCOA()
        {
            ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
            cmbxCOAcredit.ItemsSource = coaRepo.getAll();
        }

        private void CmbxAdminBillType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if ((cmbxAdminBillType.SelectedItem as cmbitem) != null)
            //{
            //    int idd = (cmbxAdminBillType.SelectedItem as cmbitem).id;
                
            //    AdminBillType billType = billsRepo.GetAdminBillType(idd);

            //    List<Vendor> vendors = new List<Vendor>();
            //    cmbxVendor.SelectedItem = null;
            //    vendors = billType.vendors;
            //    cmbxVendor.ItemsSource = vendors;
            //}
        }

        private void CmbxVendor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupAdminBillType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Admin Bill Link first!");
                lookupAdminBillType.Focus();
            }
        }

        private void CmbxPayeeName_MouseEnter(object sender, MouseEventArgs e)
        {
            if(cmbxPayeeName.SelectedIndex > -1)
            {
                String payeeName = "";
                var payee = cmbxPayeeName.SelectedItem as Payee;
                if(payee != null)
                {
                    payeeName = payee.PayeeName;
                    while(payee.ParentId != null)
                    {
                        payee = payee.parentPayee;
                        payeeName = payee.PayeeName + " > " + payeeName;
                    }
                }
                cmbxPayeeName.ToolTip = payeeName;
            }
        }

        private void CmbxCOAcredit_GotFocus(object sender, RoutedEventArgs e)
        {
            if(cmbxCOA.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select COA Debit!");
                cmbxCOA.Focus();
                return;
            }
        }

        private void ChkVat_Checked(object sender, RoutedEventArgs e)
        {
            lookupVat.IsEnabled = true;

            if (lookupVat.SelectedIndex > -1)
            {
                if (Convert.ToDouble(txtAmountOC.Text) != 0)
                {
                    var amountOC = Convert.ToDouble(txtAmountOC.Text);
                    var percent = (lookupVat.SelectedItem as TaxName).percentage;

                    var taxAmount = (percent / 100) * amountOC;
                    txtTaxAmount.Text = taxAmount.ToString();

                    txtAmountWithTax.Text = (amountOC + taxAmount).ToString();
                }
            }
        }

        private void ChkVat_Unchecked(object sender, RoutedEventArgs e)
        {
            lookupVat.IsEnabled = false;
            txtAmountWithTax.Text = txtAmountOC.Text;
        }

        //private void LookupVat_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
           
        //}

        private void TxtAmountOC_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if(chkVat.IsChecked == true && lookupVat.SelectedIndex > -1)
            {
                var amountOC = Math.Round( Convert.ToDouble(txtAmountOC.Text), 2);
                var percent = (lookupVat.SelectedItem as TaxName).percentage;

                var taxAmount = Math.Round( (percent / 100) * amountOC, 2);
                txtTaxAmount.Text = taxAmount.ToString();

                txtAmountWithTax.Text = Math.Round((amountOC + taxAmount), 2).ToString();
            }
            else
            {
                txtAmountWithTax.Text = txtAmountOC.Text;
            }
        }

        

        //private void TxtTaxAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        //{
        //    var tax = lookupVat.SelectedIndex;
        //    //if (tax != null)
        //        if (Convert.ToDouble(txtAmountOC.Text) != 0)
        //        {
        //            var amountOC = Convert.ToDouble(txtAmountOC.Text);
        //            //var percent = tax.percentage;

        //            var taxAmount = Convert.ToDouble( txtTaxAmount.Text);
        //            //txtTaxAmount.Text = taxAmount.ToString();

        //            txtAmountWithTax.Text = (amountOC + taxAmount).ToString();
        //        }
        //}
    }
}
