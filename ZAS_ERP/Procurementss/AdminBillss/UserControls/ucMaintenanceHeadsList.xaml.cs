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
    /// Interaction logic for ucMaintenanceHeadsList.xaml
    /// </summary>
    public partial class ucMaintenanceHeadsList : UserControl
    {
       

        AdminBillsRepo billRepo = new AdminBillsRepo();
        public ucMaintenanceHeadsList()
        {
            InitializeComponent();
        }

        private void MbtnAddMaintenanceHead_Click(object sender, RoutedEventArgs e)
        {
            ucFrmMaintenanceHead ucFrmMaintenance = new ucFrmMaintenanceHead();
            Window win = new Window();
            ucFrmMaintenance.editFlag = false;
            win.Content = ucFrmMaintenance;
            win.Width = 400;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditMaintenanceHead_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlMaintenanceHeadList.SelectedItem != null)
            {
                var selectedRow = grdCntrlMaintenanceHeadList.SelectedItem as MaintenanceHead;
                ucFrmMaintenanceHead ucFrmMaintenance = new ucFrmMaintenanceHead();
                Window win = new Window();
                ucFrmMaintenance.maintenanceHeadId = selectedRow.Id;
                ucFrmMaintenance.editFlag = true;
                win.Content = ucFrmMaintenance;
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
            var listt = billRepo.GetAllMaintenanceHead();
            grdCntrlMaintenanceHeadList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billRepo = new AdminBillsRepo();
            var listt = billRepo.GetAllMaintenanceHead();
            grdCntrlMaintenanceHeadList.ItemsSource = listt;
        }
    }
}
