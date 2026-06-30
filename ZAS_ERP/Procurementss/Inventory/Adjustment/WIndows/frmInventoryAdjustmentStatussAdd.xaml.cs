using ERP_BL.Databases;
using ERP_BL.Procurements.Inventories;
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

namespace ZAS_ERP.Procurementss.Inventory.Adjustment.WIndows
{
    /// <summary>
    /// Interaction logic for frmInventoryAdjustmentStatussAdd.xaml
    /// </summary>
    public partial class frmInventoryAdjustmentStatussAdd : Window
    {
        public frmInventoryAdjustmentStatussAdd()
        {
            InitializeComponent();
        }
        public static int StatusId;
        AdjustmentRepo repo = new AdjustmentRepo();
        InventoryAdjustmentStatus status = new InventoryAdjustmentStatus();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (StatusId != 0)
            {
                status = repo.getstatus(StatusId);
                txtStatus.Text = status.Status;
                chkisactive.IsChecked = status.isActive;
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
                status.backcolor = cpStatus.Text.Trim();
                if (status.Id == 0)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice Status") != null)
                    {
                        repo.addStatus(status);
                        SystemLog.LogInfo(this.GetType(), "New Inventory Adjustment Status(" + txtStatus.Text + ") Added");
                        MessageBox.Show("New PurchaseInvoice Payment Status (" + txtStatus.Text + ") Added", "Congratulations");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new Inventory Adjustment status.");
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inventory Adjustment Status") != null)
                    {
                        repo.updateStatus(status);

                        MessageBox.Show("Inventory Adjustment Status (" + txtStatus.Text + ") updated", "Congratulations");
                        SystemLog.LogInfo(this.GetType(), " Inventory Adjustment Status(" + txtStatus.Text + ") updated");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new Inventory Adjustment status.");

                }
                this.Close();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), " Inventory Adjustment Status error" + ex.ToString());

                MessageBox.Show(ex.ToString());
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StatusId = 0;
        }
    }
}
