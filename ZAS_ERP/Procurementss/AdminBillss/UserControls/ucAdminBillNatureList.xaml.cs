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
    /// Interaction logic for ucAdminBillNatureList.xaml
    /// </summary>
    public partial class ucAdminBillNatureList : UserControl
    {
        AdminBillsRepo billRepo = new AdminBillsRepo();
        public ucAdminBillNatureList()
        {
            InitializeComponent();
        }

        private void MbtnAddAdminBillNature_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAdminBillNatureAdd frmBillNature = new ucFrmAdminBillNatureAdd();
            Window win = new Window();
            frmBillNature.editFlag = false;
            win.Content = frmBillNature;
            win.Width = 400;
            win.Height = 300;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditAdminBillNature_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlAdminBillNatureList.SelectedItem != null)
            {
                var selectedRow = grdCntrlAdminBillNatureList.SelectedItem as AdminBillNature;
                ucFrmAdminBillNatureAdd frmBillNature = new ucFrmAdminBillNatureAdd();
                Window win = new Window();
                frmBillNature.billNatureId = selectedRow.Id;
                frmBillNature.editFlag = true;
                win.Content = frmBillNature;
                win.Width = 400;
                win.Height = 300;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            billRepo = new AdminBillsRepo();
            grdCntrlAdminBillNatureList.ItemsSource = billRepo.GetAllAdminBillNature();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billRepo = new AdminBillsRepo();
            grdCntrlAdminBillNatureList.ItemsSource = billRepo.GetAllAdminBillNature();
        }

        private void GrdCntrlAdminBillNatureList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Companiess" && e.IsGetData)
            {
                var _Nature = grdCntrlAdminBillNatureList.GetRowByListIndex(e.ListSourceRowIndex) as AdminBillNature;
                string companies = "";

                if (_Nature.Companies != null && _Nature.Companies.Count > 0)
                {
                    companies = String.Join(" | ", _Nature.Companies.Select(x => x.CompanyName));
                }
                e.Value = companies;
            }
        }
    }
}
