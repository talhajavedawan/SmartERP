using ERP_BL.FilesAndDocs;
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

namespace ZAS_ERP.FilesAndDocss.TravellingRecords.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmTravelingStatus.xaml
    /// </summary>
    public partial class ucFrmTravelingStatus : UserControl
    {
        public TravelingStatus status = new TravelingStatus();
        VisitingRecordRepo visitingRecordRepo = new VisitingRecordRepo();
        public bool saveEditFlag = false;
        public Window frmPaymentStatusWin = new Window();
        public ucFrmTravelingStatus()
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

            visitingRecordRepo = new VisitingRecordRepo();
            if (saveEditFlag == false)
            {
                visitingRecordRepo.AddTravelingStatus(status);
                MessageBox.Show("New Traveling Status " + txtStatus.Text + " Added");
            }
            else if (saveEditFlag == true)
            {
                visitingRecordRepo.UpdateTravelingStatus(status);
                MessageBox.Show("Status Updated " + txtStatus.Text + " Added");
            }

            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
