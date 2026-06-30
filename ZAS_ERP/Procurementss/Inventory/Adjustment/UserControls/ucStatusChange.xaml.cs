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
    /// Interaction logic for ucStatusChange.xaml
    /// </summary>
    public partial class ucStatusChange : UserControl
    {
        public static int adjustmentId;
        public static int inActiveStatuses;
        public static InventoryAdjustment adjustment = new InventoryAdjustment();
        public static AdjustmentRepo adjustmentRepo = new AdjustmentRepo();
        public ucStatusChange()
        {
            InitializeComponent();
        }
        public class cmbitem
        {
            public string name { get; set; }
            public int id { get; set; }
            public string bcolor { get; set; }
            public string fcolor { get; set; }
        }
        private void btnStatusSave_Click(object sender, RoutedEventArgs e)
        {
            if ((cmbInventoryAdjustmentStatus.SelectedItem as cmbitem) != null)
            {
                InventoryAdjustmentStatus status = adjustmentRepo.getstatus((cmbInventoryAdjustmentStatus.SelectedItem as cmbitem).id);
                adjustment.AdjustmentStatus = status;
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else
            {
                MessageBox.Show("Please select New Status First");
            }
        }
        public void loadInventoryAdjustmentStatus()
        {
            List<InventoryAdjustmentStatus> adjustmentStatuses = new List<InventoryAdjustmentStatus>();
            if (inActiveStatuses == 0)
                adjustmentStatuses = adjustmentRepo.getAllActiveAdjustmentStatus();
            else
                adjustmentStatuses = adjustmentRepo.getAllInActiveAdjustmentStatus();



            adjustmentStatuses = adjustmentRepo.getAllInActiveInventoryAdjustmentStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (InventoryAdjustmentStatus status in adjustmentStatuses)
            {
                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
            }
            cmbInventoryAdjustmentStatus.ItemsSource = cmbitems;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            adjustment = adjustmentRepo.get(adjustmentId);
            loadInventoryAdjustmentStatus();
            if (adjustment != null && adjustment.Id != 0)
            {
                txtStatus.Text = adjustment.AdjustmentStatus.Status.ToString();

                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(adjustment.AdjustmentStatus.backcolor);
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                lblstatuscolor.Background = new SolidColorBrush(newColor);

                foreach (cmbitem cmbitem in cmbInventoryAdjustmentStatus.Items)
                {
                    if (cmbitem.name == adjustment.AdjustmentStatus.Status)
                    {
                        cmbInventoryAdjustmentStatus.SelectedItem = cmbitem;
                    }
                }
            }
            else
            {
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
                MessageBox.Show("Couldn't load Inventory Adjustment Data! Try again...!");
            }
        }

        private void cmbInventoryAdjustmentStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cmbInventoryAdjustmentStatus.SelectedItem as cmbitem) != null)
            {
                int idd = (cmbInventoryAdjustmentStatus.SelectedItem as cmbitem).id;
                if (idd == 0)
                {
                    frmInventoryAdjustmentStatusAdd statusAdd = new frmInventoryAdjustmentStatusAdd();
                    statusAdd.ShowDialog();
                    loadInventoryAdjustmentStatus();
                }
            }
        }
    }
}
