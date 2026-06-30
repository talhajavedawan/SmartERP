using ERP_BL.Payments;
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

namespace ZAS_ERP.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmPaymentStatus.xaml
    /// </summary>
    public partial class ucFrmPaymentStatus : UserControl
    {
        public PaymentStatus status = new PaymentStatus();
        PaymentRepo paymentsRepo = new PaymentRepo();
        public bool saveEditFlag = false;
        public Window frmPaymentStatusWin = new Window();
        public ucFrmPaymentStatus()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (saveEditFlag == true && status != null)
            {
                if (status.Status != null)
                {
                    txtStatus.Text = status.Status;
                }

                if (status.backcolor != null)
                {
                    object color = ColorConverter.ConvertFromString(status.backcolor);
                    cpStatus.Color = (Color)color;
                }

                if (status.isActive == true)
                    chkisactive.IsChecked = true;
                else
                    chkisactive.IsChecked = false;  
                if (status.isDisable == true)
                    chkDisable.IsChecked = true;
                else
                    chkDisable.IsChecked = false; 
                if (status.isPaid == true)
                    chkPaid.IsChecked = true;
                else
                    chkPaid.IsChecked = false;
            }
        }

        private void ColorEdit_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (cpStatus.Color.R <= 120 || cpStatus.Color.G <= 120 || cpStatus.Color.B <= 120)
            {
                var myColor = "#FFFFFFFF";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                txtStatus.Foreground = new SolidColorBrush(newColor);
                status.forecolor = myColor;
            }
            else
            {
                var myColor = "#FF000000";
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(myColor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                txtStatus.Foreground = new SolidColorBrush(newColor);
                status.forecolor = myColor;
            }
            txtStatus.Background = new SolidColorBrush(cpStatus.Color);
        }

        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            status.Status = txtStatus.Text.Trim();
            if (chkisactive.IsChecked == true)
                status.isActive = true;
            else
                status.isActive = false;  
            if (chkDisable.IsChecked == true)
                status.isDisable = true;
            else
                status.isDisable = false;
            if (chkPaid.IsChecked == true)
                status.isPaid = true;
            else
                status.isPaid = false;
            status.backcolor = cpStatus.Text.Trim();

            paymentsRepo = new PaymentRepo();
            if (saveEditFlag == false)
            {
                paymentsRepo.AddPaymentStatus(status);
                MessageBox.Show("New Payment Status " + txtStatus.Text + " Added");
            }
            else if (saveEditFlag == true)
            {
                paymentsRepo.UpdatePaymentStatus(status);
                MessageBox.Show("Status Updated " + txtStatus.Text + " Added");
            }

            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        
    }
}
