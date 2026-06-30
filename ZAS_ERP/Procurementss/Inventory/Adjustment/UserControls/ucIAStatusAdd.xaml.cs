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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.Inventory.Adjustment.UserControls
{
    /// <summary>
    /// Interaction logic for ucIAStatusAdd.xaml
    /// </summary>
    public partial class ucIAStatusAdd : UserControl
    {
        public ucIAStatusAdd()
        {
            InitializeComponent();
        }

        AdjustmentRepo repo = new AdjustmentRepo();
        InventoryAdjustmentStatus status = new InventoryAdjustmentStatus();
        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            status.Status = txtStatus.Text.Trim();
            if (chkisactive.IsChecked == true)
                status.isActive = true;
            else
                status.isActive = false;
            status.backcolor = cpStatus.Text.Trim();
            repo.addStatus(status);
            MessageBox.Show("New Inventory Adjustment Status " + txtStatus.Text + " Added");
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
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
    }
}
