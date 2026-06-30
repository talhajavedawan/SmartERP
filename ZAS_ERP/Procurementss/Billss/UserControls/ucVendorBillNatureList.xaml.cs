using ERP_BL.Databases;
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

namespace ZAS_ERP.Procurementss.Billss.UserControls
{
    /// <summary>
    /// Interaction logic for ucVendorBillNatureList.xaml
    /// </summary>
    public partial class ucVendorBillNatureList : UserControl
    {
        BillRepo billRepo = new BillRepo();
        public ucVendorBillNatureList()
        {
            InitializeComponent();
        }

        private void MbtnAddVendorBillNature_Click(object sender, RoutedEventArgs e)
        {
            ucFrmVendorBillNatureAdd frmBillNature = new ucFrmVendorBillNatureAdd();
            Window win = new Window();
            frmBillNature.editFlag = false;
            win.Content = frmBillNature;
            win.Width = 400;
            win.Height = 300;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditVendorBillNature_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlVendorBillNatureList.SelectedItem != null)
            {
                var selectedRow = grdCntrlVendorBillNatureList.SelectedItem as VendorBillNature;
                ucFrmVendorBillNatureAdd frmBillNature = new ucFrmVendorBillNatureAdd();
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
            billRepo = new BillRepo();
            grdCntrlVendorBillNatureList.ItemsSource = billRepo.GetAllVendorBillNature();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billRepo = new BillRepo();
            grdCntrlVendorBillNatureList.ItemsSource = billRepo.GetAllVendorBillNature();
        }

        private void GrdCntrlVendorBillNatureList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Companiess" && e.IsGetData)
            {
                var _Nature = grdCntrlVendorBillNatureList.GetRowByListIndex(e.ListSourceRowIndex) as VendorBillNature;
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
