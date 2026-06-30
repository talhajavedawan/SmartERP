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
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP.Termss
{
    /// <summary>
    /// Interaction logic for frmPaymentTermAdd.xaml
    /// </summary>
    public partial class frmPaymentTermAdd : Window
    {
        public frmPaymentTermAdd()
        {
            InitializeComponent();
        }
        public static int paymentTermId;
        PaymentTermRepo repo = new PaymentTermRepo();
        PaymentTerm paymentTerm = new PaymentTerm();
        private void btnPaymentTermSave_Click(object sender, RoutedEventArgs e)
        {
            //comment
            try
            {
                if (txtterm.Text == "")
                {
                    MessageBox.Show("Enter Terms to Continue ");
                    txtterm.Focus();
                    return;
                }

                if (chkSaleOrder.IsChecked == true)
                    paymentTerm.isSaleOrderType = true;
                else
                    paymentTerm.isSaleOrderType = false;

                if (chkPurchaseOrder.IsChecked == true)
                    paymentTerm.isPurchaseOrderType = true;
                else
                    paymentTerm.isPurchaseOrderType = false;

                if (chkPurchaseInvoice.IsChecked == true)
                    paymentTerm.isPurchaseInvoiceType = true;
                else
                    paymentTerm.isPurchaseInvoiceType = false;

                if (chkSaleInvoice.IsChecked == true)
                    paymentTerm.isSaleInvoiceType = true;
                else
                    paymentTerm.isSaleInvoiceType = false;

                if (chkPayments.IsChecked == true)
                    paymentTerm.isPaymentType = true;
                else
                    paymentTerm.isPaymentType = false;

                if (chkVendorBills.IsChecked == true)
                    paymentTerm.isVnedorBillType = true;
                else
                    paymentTerm.isVnedorBillType = false;

                if (chkOffers.IsChecked == true)
                    paymentTerm.isOfferType = true;
                else
                    paymentTerm.isOfferType = false;

                if (chkCostSheet.IsChecked == true)
                    paymentTerm.isCostSheetType = true;
                else
                    paymentTerm.isCostSheetType = false;

                paymentTerm.term = txtterm.Text.Trim();
                if (chkIsActive.IsChecked == true)
                    paymentTerm.isActive = true;
                else
                    paymentTerm.isActive = false;

                paymentTerm.discountPercent = string.IsNullOrEmpty(txtdiscountPercent.Text.Trim()) ? 0 : (float)Convert.ToDecimal(txtdiscountPercent.Text.Trim());

                if (lookupParentTerm.SelectedIndex > -1)
                    paymentTerm.ParentId = (lookupParentTerm.SelectedItem as PaymentTerm).Id;
                else
                    paymentTerm.ParentId = null;

                if (rbtnStandard.IsChecked == true)
                {
                    paymentTerm.netDueDays = string.IsNullOrEmpty(txtnetdue.Text.Trim()) ? 0 : Convert.ToInt32(txtnetdue.Text.Trim());
                    paymentTerm.type = "Standard";
                    paymentTerm.discountDays = string.IsNullOrEmpty(txtDiscDays.Text.Trim()) ? 0 : Convert.ToInt32(txtDiscDays.Text.Trim());
                    paymentTerm.discountonDayofMonth = 1;
                }
                if (rbtnDatebased.IsChecked == true)
                {
                    paymentTerm.type = "DateBased";
                    paymentTerm.daysofMonthDue = string.IsNullOrEmpty(txtnetdue.Text.Trim()) ? 0 : Convert.ToInt32(txtnetdue.Text.Trim());
                    paymentTerm.discountDays = 0;
                    paymentTerm.discountonDayofMonth = string.IsNullOrEmpty(txtMinDueDays.Text.Trim()) ? 0 : Convert.ToInt32(txtMinDueDays.Text.Trim());
                    paymentTerm.daysofMonthDue = string.IsNullOrEmpty(txtMinDueDays.Text.Trim()) ? 0 : Convert.ToInt32(txtMinDueDays.Text.Trim());

                }
                if (paymentTerm.Id != 0)
                {
                    
                    repo.Update(paymentTerm);
                    MessageBox.Show("PaymentTerm (" + txtterm.Text + ") updated", "Congratulations");
                }

                else
                {
                    paymentTerm.user_Id = MainWindow.currentUserid;
                    paymentTerm.isApproved = false;
                    paymentTerm.addedDate = DateTime.Now;
                    repo.Add(paymentTerm);
                    MessageBox.Show("New PaymentTerm (" + txtterm.Text + ") Added", "Congratulations");

                }
                MessageBox.Show(txtterm.Text + " Saved Succesfully!");

                this.Close();
               
            
           
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void rbtnStandard_Checked(object sender, RoutedEventArgs e)
        {
            lblnetdue1.Text = "Net Due in";
            lblnetdue2.Text = "Days";
            lbldiscdays1.Text = "Discount if paid within";
            lbldiscdays2.Text = "days;";
            stpanelduemindays.Visibility = Visibility.Collapsed;
        }

        private void rbtnDatebased_Checked(object sender, RoutedEventArgs e)
        {
            lblnetdue1.Text = "Net Due before the";
            lblnetdue2.Text = "th day of the month.";
            lbldiscdays1.Text = "Discount if paid before";
            lbldiscdays2.Text = "th day of the month.";
            stpanelduemindays.Visibility = Visibility.Visible;

        }
        private void winPaymentTermAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            paymentTermId = 0;
        }

        private void winPaymentTermAdd_Loaded(object sender, RoutedEventArgs e)
        {
            var terms = repo.getAll();
            this.lookupParentTerm.ItemsSource = terms;
            if (paymentTermId != 0)
            {
                paymentTerm = repo.get(paymentTermId);
                txtterm.Text = paymentTerm.term;
                chkIsActive.IsChecked = paymentTerm.isActive;
                txtdiscountPercent.Text=paymentTerm.discountPercent.ToString() ;

                if (paymentTerm.isSaleOrderType == true)
                    chkSaleOrder.IsChecked = true;
                if (paymentTerm.isSaleInvoiceType == true)
                    chkSaleInvoice.IsChecked = true;
                if (paymentTerm.isPurchaseOrderType == true)
                    chkPurchaseOrder.IsChecked = true;
                if (paymentTerm.isPurchaseInvoiceType == true)
                    chkPurchaseInvoice.IsChecked = true;
                if (paymentTerm.isPaymentType == true)
                    chkPayments.IsChecked = true;
                if (paymentTerm.isVnedorBillType == true)
                    chkVendorBills.IsChecked = true;
                if (paymentTerm.isOfferType == true)
                    chkOffers.IsChecked = true;
                if (paymentTerm.isCostSheetType == true)
                    chkCostSheet.IsChecked = true;

                if (paymentTerm.parentTerm != null)
                    lookupParentTerm.Text = paymentTerm.parentTerm.term;

                if (paymentTerm.type == "Standard")
                {
                    rbtnStandard.IsChecked = true;
                    txtnetdue.Text= paymentTerm.netDueDays.ToString();

                    txtDiscDays.Text=paymentTerm.discountDays.ToString();

                }
                else
                {
                    rbtnDatebased.IsChecked = true;
                    txtnetdue.Text=paymentTerm.daysofMonthDue.ToString();
                    
                    txtnetdue.Text=paymentTerm.discountonDayofMonth.ToString();
                    txtMinDueDays.Text=paymentTerm.daysofMonthDue.ToString();
                }
            }
        }
    }
}
