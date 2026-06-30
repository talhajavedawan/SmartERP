using DevExpress.Xpf.Core;
using ERP_BL.ToDoTasks.Taskss;
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

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucAddWarehouse.xaml
    /// </summary>
    public partial class ucAddWarehouse : UserControl
    {
        public bool editFlag = false;
        TaskRepo taskRepo = new TaskRepo();
        Warehouse warehouse = new Warehouse();
        public int warehouseId = 0;
        public ucAddWarehouse()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
           
            if (String.IsNullOrEmpty(txtWarehouse.Text))
            {
                DXMessageBox.Show("Please Enter Warehouse Name!");
                txtWarehouse.Focus();
                return;
            }

            warehouse.WarehouseName = txtWarehouse.Text;
            if (chkIsActive.IsChecked == true)
                warehouse.isActive = true;
            else
                warehouse.isActive = false;

            if (editFlag == false)
            {
                taskRepo.AddWarehouse(warehouse);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                taskRepo.UpdateWarehouse(warehouse);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                warehouse = taskRepo.GetWarehouse(warehouseId);

                txtWarehouse.Text = warehouse.WarehouseName;
                chkIsActive.IsChecked = warehouse.isActive;
            }
        }
    }
}
