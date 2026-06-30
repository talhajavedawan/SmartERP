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
    /// Interaction logic for ucManagementSummaryList.xaml
    /// </summary>
    public partial class ucManagementSummaryList : UserControl
    {
        AdminBillsRepo billRepo = new AdminBillsRepo();
        public ucManagementSummaryList()
        {
            InitializeComponent();
        }

        private void MbtnAddManagementSummary_Click(object sender, RoutedEventArgs e)
        {
            ucManagementSummaryAdd frmManagementSummary = new ucManagementSummaryAdd();
            Window win = new Window();
            frmManagementSummary.editFlag = false;
            win.Content = frmManagementSummary;
            win.Width = 400;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditManagementSummary_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlManagementSummaryList.SelectedItem != null)
            {
                var selectedRow = grdCntrlManagementSummaryList.SelectedItem as ManagementSummary;
                ucManagementSummaryAdd frmManagementSummary = new ucManagementSummaryAdd();
                Window win = new Window();
                frmManagementSummary.managementSummary = selectedRow;
                frmManagementSummary.editFlag = true;
                win.Content = frmManagementSummary;
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
            grdCntrlManagementSummaryList.ItemsSource = billRepo.GetAllManagementSummary();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billRepo = new AdminBillsRepo();
            grdCntrlManagementSummaryList.ItemsSource = billRepo.GetAllManagementSummary();
        }
    }
}
