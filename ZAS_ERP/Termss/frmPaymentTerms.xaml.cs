using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ERP_BL;


namespace ZAS_ERP.Termss
{
    /// <summary>
    /// Interaction logic for frmPaymentTerms.xaml
    /// </summary>
    public partial class frmPaymentTerms : Window
    {
        public frmPaymentTerms()
        {
            InitializeComponent();
            rbtnStandard.IsChecked = true;
            loadgrid();
        }
        PaymentTerm paymentTerm = new PaymentTerm();
        PaymentTermRepo paymentTermRepo = new PaymentTermRepo();
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

        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            if (txtterm.Text == "")
            {
                MessageBox.Show("Enter Terms to Continue ");
                txtterm.Focus();
                return;
            }

            paymentTerm.term = txtterm.Text.Trim();


            paymentTerm.discountPercent = string.IsNullOrEmpty(txtdiscountPercent.Text.Trim()) ? 0 : (float)Convert.ToDecimal(txtdiscountPercent.Text.Trim());

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
                paymentTermRepo.Update(paymentTerm);
            }

            else
            {
                paymentTerm.user_Id = MainWindow.currentUserid;
                paymentTerm.isApproved = false;
                paymentTerm.addedDate = DateTime.Now;
                paymentTermRepo.Add(paymentTerm);
            }
            MessageBox.Show(txtterm.Text + " Saved Succesfully!");
            loadgrid();
            refreshform();

        }
        private void loadgrid()
        {

            this.grdPaymentTerms.ItemsSource = paymentTermRepo.getAll();

            grdPaymentTerms.Columns.GetColumnByFieldName("Id").Visible = false;
            grdPaymentTerms.Columns.GetColumnByFieldName("user").Visible = false;
            grdPaymentTerms.Columns.GetColumnByFieldName("isApproved").Visible = false;
            grdPaymentTerms.Columns.GetColumnByFieldName("isActive").Visible = false;
            grdPaymentTerms.Columns.GetColumnByFieldName("user_Id").Visible = false;


            //grdemployee.Columns.GetColumnByFieldName("person").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("address").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("contact").Visible = false;
            ////grdemployee.Columns.GetColumnByFieldName("Companies").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Desig").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Disability").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("DisDescription").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("MaritalStatus").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Status").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("JoinDate").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("BasicPay").Visible = false;
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "EmpId" });
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.FName" });
            //grdemployee.Columns.GetColumnByFieldName("person.FName").Header = "First Name";
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.LName" });
            //grdemployee.Columns.GetColumnByFieldName("person.LName").Header = "Last Name";

        }
        public void refreshform()
        {
            txtDiscDays.Text = "";
            txtdiscountPercent.Text = "";
            txtMinDueDays.Text = "";
            txtnetdue.Text = "";
            txtterm.Text = "";
            paymentTerm = new PaymentTerm();
        }
    }
}
