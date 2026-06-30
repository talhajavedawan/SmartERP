using ERP_BL.AssetsRentals;
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

namespace ZAS_ERP.AssetRentalss.UserControls
{
    /// <summary>
    /// Interaction logic for ucAssetBrandList.xaml
    /// </summary>
    public partial class ucAssetBrandList : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();

        public ucAssetBrandList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAssetBrandAdd frmAssetBrand = new ucFrmAssetBrandAdd();
            Window win = new Window();
            frmAssetBrand.editFlag = false;
            win.Content = frmAssetBrand;
            win.Width = 400;
            win.Height = 350;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }
        //comment
        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlAssetBrandList.SelectedItem != null)
            {
                var selectedRow = grdCntrlAssetBrandList.SelectedItem as AssetBrand;
                ucFrmAssetBrandAdd frmAssetBrand = new ucFrmAssetBrandAdd();
                Window win = new Window();
                frmAssetBrand.assetBrandId = selectedRow.Id;
                frmAssetBrand.editFlag = true;
                win.Content = frmAssetBrand;
                win.Width = 400;
                win.Height = 350;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            rentalRepo = new AssetRentalRepo();
            var listt = rentalRepo.GetAllAssetBrand();
            grdCntrlAssetBrandList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rentalRepo = new AssetRentalRepo();
            var listt = rentalRepo.GetAllAssetBrand();
            grdCntrlAssetBrandList.ItemsSource = listt;
        }

        private void grdCntrlAssetBrandList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdCntrlAssetBrandList.GetRowByListIndex(e.ListSourceRowIndex) as AssetBrand;
            if (e.Column.FieldName == "AssetSubNaturess" && e.IsGetData)
            {
                if (row.AssetSubNatures != null && row.AssetSubNatures.Count > 0)
                    e.Value = String.Join(" | ", row.AssetSubNatures.Select(x => x.NatureName));
            }
        }
    }
}
