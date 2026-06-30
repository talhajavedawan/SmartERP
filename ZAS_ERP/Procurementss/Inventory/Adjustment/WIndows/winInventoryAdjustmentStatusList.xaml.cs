using DevExpress.Xpf.Core;
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
    /// Interaction logic for winInventoryAdjustmentStatusList.xaml
    /// </summary>
    public partial class winInventoryAdjustmentStatusList : Window
    {
        List<InventoryAdjustmentStatus> InventoryAdjustmentStatuss = new List<InventoryAdjustmentStatus>();
        AdjustmentRepo repo = new AdjustmentRepo();
        public winInventoryAdjustmentStatusList()
        {
            InitializeComponent();
        }

        private void grdInventoryAdjustmentStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnNewInventoryAdjustmentStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inventory Adjustment Status") != null)
            {
                newInventoryAdjustmentStatus();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Status!");
            }
        }

        private void btnEditInventoryAdjustmentStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inventory Adjustment Status") != null)
            {
                if (grdInventoryAdjustmentStatus.SelectedItem != null)
                {

                    frmInventoryAdjustmentStatussAdd.StatusId = (grdInventoryAdjustmentStatus.SelectedItem as InventoryAdjustmentStatus).Id;

                    newInventoryAdjustmentStatus();
                }
                else
                {
                    MessageBox.Show("Please select a Inventory Adjustment Status to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Status!");
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
           
        }
        private void loadInventoryAdjustmentStatus()
        {

            if (MainWindow.currentUserid == 0)
                InventoryAdjustmentStatuss = repo.getAllInventoryAdjustmentStatus();

            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Inventory Adjustment Statuses") != null)
            {
                InventoryAdjustmentStatuss = repo.getAllInventoryAdjustmentStatus();
            }
            else
            {
                InventoryAdjustmentStatuss = repo.getAllActiveInventoryAdjustmentStatus();
            }


            this.grdInventoryAdjustmentStatus.ItemsSource = InventoryAdjustmentStatuss;
            grdInventoryAdjustmentStatus.Columns.GetColumnByFieldName("Id").Visible = false;
        }
        public void newInventoryAdjustmentStatus()
        {
            frmInventoryAdjustmentStatussAdd frmInventoryAdjustmentStatussadd = new frmInventoryAdjustmentStatussAdd();
            frmInventoryAdjustmentStatussadd.ShowDialog();

            loadInventoryAdjustmentStatus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadInventoryAdjustmentStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdInventoryAdjustmentStatus);
        }
    }
}
