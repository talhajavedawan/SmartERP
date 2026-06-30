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

namespace ZAS_ERP.Procurementss.Inquiriess
{
    /// <summary>
    /// Interaction logic for frmInquiryStatusAdd.xaml
    /// </summary>
    public partial class frmInquiryStatussAdd : Window
    {
        bool isVoid = false;
        public frmInquiryStatussAdd()
        {
            InitializeComponent();
        }
        public static int StatusId;
        InquiryRepo repo = new InquiryRepo();
        InquiryStatus status = new InquiryStatus();
        private void BtnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            

                status.Status = txtStatus.Text.Trim();
                if (chkisactive.IsChecked == true)
                    status.isActive = true;
                else
                    status.isActive = false;   
                if (chkisDisable.IsChecked == true)
                    status.isDisable = true;
                else
                    status.isDisable = false;
                status.backcolor = cpStatus.Text.Trim();
                //repo.addStatus(status);
                
                status.isVoid = isVoid;
                
                if (status.Id == 0)
            {
                    //if (MainWindow.currentUserid != 0)
                    //    InquiryStatus.user_Id = MainWindow.currentUserid;
                    //else
                    //    InquiryStatus.user_Id = null;
                   
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inquiry Status") != null)
                    {
                        repo.addStatus(status);
                        SystemLog.LogInfo(this.GetType(), "New Inquiry Payment Status(" + txtStatus.Text + ") Added");
                        MessageBox.Show("New Inquiry Payment Status (" + txtStatus.Text + ") Added", "Congratulations");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new Inquiry status.");
            }
            else
            {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inquiry Status") != null)
                    {
                        repo.updateStatus(status);

                        MessageBox.Show("Inquiry Payment Status (" + txtStatus.Text + ") updated", "Congratulations");
                        SystemLog.LogInfo(this.GetType(), " Inquiry Payment Status(" + txtStatus.Text + ") updated");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new Inquiry status.");
                //    repo.updateStatus(InquiryStatus);

                //MessageBox.Show("Inquiry Payment Status (" + txtStatus.Text + ") updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                SystemLog.LogError(this.GetType(), " Inquiry Status error" + ex.ToString());

                MessageBox.Show(ex.ToString());
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
        private void winInquiryStatusAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StatusId = 0;
        }

        private void winInquiryStatusAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (StatusId != 0)
            {
                status = repo.getstatus(StatusId);
                txtStatus.Text = status.Status;
                if (status.isVoid == true)
                    chkisvoid.IsChecked = true;
                chkisactive.IsChecked = status.isActive;
            }
        }

        private void Chkisvoid_Checked(object sender, RoutedEventArgs e)
        {
            isVoid = true;
        }

        private void Chkisvoid_Unchecked(object sender, RoutedEventArgs e)
        {
            isVoid = false;
        }
    }
}
