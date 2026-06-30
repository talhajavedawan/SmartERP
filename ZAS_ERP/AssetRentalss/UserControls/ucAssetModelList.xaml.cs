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
    /// Interaction logic for ucAssetModelList.xaml
    /// </summary>
    public partial class ucAssetModelList : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        public ucAssetModelList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAssetModelAdd frmAssetModel = new ucFrmAssetModelAdd();
            Window win = new Window();
            frmAssetModel.editFlag = false;
            win.Content = frmAssetModel;
            win.Width = 400;
            win.Height = 350;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }
        //comment
        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlAssetModelList.SelectedItem != null)
            {
                var selectedRow = grdCntrlAssetModelList.SelectedItem as AssetModel;
                ucFrmAssetModelAdd frmAssetModel = new ucFrmAssetModelAdd();
                Window win = new Window();
                frmAssetModel.assetModelId = selectedRow.Id;
                frmAssetModel.editFlag = true;
                win.Content = frmAssetModel;
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
            var listt = rentalRepo.GetAllAssetModel();
            grdCntrlAssetModelList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rentalRepo = new AssetRentalRepo();
            var listt = rentalRepo.GetAllAssetModel();
            grdCntrlAssetModelList.ItemsSource = listt;

        }

        private void grdCntrlAssetModelList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdCntrlAssetModelList.GetRowByListIndex(e.ListSourceRowIndex) as AssetModel;
            if (e.Column.FieldName == "AssetSubNaturess" && e.IsGetData)
            {
                if (row.AssetSubNatures != null && row.AssetSubNatures.Count > 0)
                    e.Value = String.Join(" | ", row.AssetSubNatures.Select(x => x.NatureName));
            }
            if (e.Column.FieldName == "AssetBrandss" && e.IsGetData)
            {
                if (row.assetBrands != null && row.assetBrands.Count > 0)
                    e.Value = String.Join(" | ", row.assetBrands.Select(x => x.BrandName));
            }
        }
    }
}
