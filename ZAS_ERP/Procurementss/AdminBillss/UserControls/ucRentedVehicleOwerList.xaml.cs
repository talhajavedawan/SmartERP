using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucRentedVehicleOwerList.xaml
    /// </summary>
    public partial class ucRentedVehicleOwerList : UserControl
    {
        AdminBillsRepo billRepo = new AdminBillsRepo();
        public ucRentedVehicleOwerList()
        {
            InitializeComponent();
        }

        private void MbtnAddRentedVehicleOwner_Click(object sender, RoutedEventArgs e)
        {
            ucAddRentedVehicleOwner ucAddRentedVehicle = new ucAddRentedVehicleOwner();
            Window win = new Window();
            ucAddRentedVehicle.editFlag = false;
            win.Content = ucAddRentedVehicle;
            win.Width = 400;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditRentedVehicleOwner_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlRentedVehicleOwnerList.SelectedItem != null)
            {
                var selectedRow = grdCntrlRentedVehicleOwnerList.SelectedItem as RentedVehicleOwner;
                ucAddRentedVehicleOwner ucAddRentedVehicle = new ucAddRentedVehicleOwner();
                Window win = new Window();
                ucAddRentedVehicle.vehicleOwnerId = selectedRow.Id;
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
            billRepo = new AdminBillsRepo();
            var listt = billRepo.GetAllRentedVehicleOwner();
            grdCntrlRentedVehicleOwnerList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billRepo = new AdminBillsRepo();
            var listt = billRepo.GetAllRentedVehicleOwner();
            grdCntrlRentedVehicleOwnerList.ItemsSource = listt;
        }
    }
}
