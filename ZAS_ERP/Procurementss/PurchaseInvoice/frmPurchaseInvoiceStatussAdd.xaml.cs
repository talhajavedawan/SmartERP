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
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.PurchaseInvoice
{
    /// <summary>
    /// Interaction logic for frmPurchaseInvoiceStatussAdd.xaml
    /// </summary>
    public partial class frmPurchaseInvoiceStatussAdd : Window
    {
        public frmPurchaseInvoiceStatussAdd()
        {
            InitializeComponent();
        }
        public static int StatusId;
        PurchaseInvoiceRepo repo = new PurchaseInvoiceRepo();
        PurchaseInvoiceStatus status = new PurchaseInvoiceStatus();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (StatusId != 0)
            {
                status = repo.getstatus(StatusId);
                txtStatus.Text = status.Status;
                chkisactive.IsChecked = status.isActive;
                chkDisable.IsChecked = status.isDisable;
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
            try
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
                status.backcolor = cpStatus.Text.Trim();
                if (status.Id == 0)
                {
                    
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice Status") != null)
                    {
                        repo.addStatus(status);
                        SystemLog.LogInfo(this.GetType(), "New PurchaseInvoice Payment Status(" + txtStatus.Text + ") Added");
                        MessageBox.Show("New PurchaseInvoice Payment Status (" + txtStatus.Text + ") Added", "Congratulations");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new PurchaseInvoice status.");
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Invoice Status") != null)
                    {
                        repo.updateStatus(status);

                        MessageBox.Show("PurchaseInvoice Payment Status (" + txtStatus.Text + ") updated", "Congratulations");
                        SystemLog.LogInfo(this.GetType(), " PurchaseInvoice Payment Status(" + txtStatus.Text + ") updated");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new PurchaseInvoice status.");
                    //    repo.updateStatus(SaleInvoiceStatus);

                    //MessageBox.Show("SaleInvoice Payment Status (" + txtStatus.Text + ") updated", "Congratulations");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), " PurchaseInvoice Status error" + ex.ToString());

                MessageBox.Show(ex.ToString());
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StatusId = 0;
        }
    }
}
