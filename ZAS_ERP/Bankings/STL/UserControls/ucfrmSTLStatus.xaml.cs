using ERP_BL.Procurements.InterBankTransfers;
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

namespace ZAS_ERP.Bankings.STL
{
    /// <summary>
    /// Interaction logic for ucfrmSTLStatus.xaml
    /// </summary>
    public partial class ucfrmSTLStatus : UserControl
    {
        STLStatus status = new STLStatus();
        public int statusId = 0;
        public bool editFlag = false;
        STLRepo stlRepo = new STLRepo();
        public ucfrmSTLStatus()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && statusId != 0)
            {
                status = stlRepo.GetLoansStatus(statusId);
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
                    chkIsDisable.IsChecked = true;
                else
                    chkIsDisable.IsChecked = false;
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
            status.backcolor = cpStatus.Text.Trim();   
            if (chkIsDisable.IsChecked == true)
                status.isDisable = true;
            else
                status.isDisable = false;
            status.backcolor = cpStatus.Text.Trim();

            if (editFlag == false)
            {
                stlRepo.AddSTLStatus(status);
                MessageBox.Show("New STL Status " + txtStatus.Text + " Added");
            }
            else if (editFlag == true)
            {
                stlRepo.UpdateLoansStatus(status);
                MessageBox.Show("Status Updated " + txtStatus.Text + " Added");
            }

            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
