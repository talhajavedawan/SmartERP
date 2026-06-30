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
    /// Interaction logic for ucWarehouseList.xaml
    /// </summary>
    public partial class ucWarehouseList : UserControl
    {
        TaskRepo taskRepo = new TaskRepo();
        public ucWarehouseList()
        {
            InitializeComponent();
        }

        private void MbtnAddWarehouse_Click(object sender, RoutedEventArgs e)
        {
            ucAddWarehouse ucAddWarehouse = new ucAddWarehouse();
            Window win = new Window();
            ucAddWarehouse.editFlag = false;
            win.Content = ucAddWarehouse;
            win.Width = 400;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditWarehouse_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlWarehouseList.SelectedItem != null)
            {
                var selectedRow = grdCntrlWarehouseList.SelectedItem as Warehouse;
                ucAddWarehouse ucAddRentedVehicle = new ucAddWarehouse();
                Window win = new Window();
                ucAddRentedVehicle.warehouseId = selectedRow.Id;
                ucAddRentedVehicle.editFlag = true;
                win.Content = ucAddRentedVehicle;
                win.Width = 400;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taskRepo = new TaskRepo();
            var listt = taskRepo.GetAllWarehouse();
            grdCntrlWarehouseList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            taskRepo = new TaskRepo();
            var listt = taskRepo.GetAllWarehouse();
            grdCntrlWarehouseList.ItemsSource = listt;
        }
    }
}
