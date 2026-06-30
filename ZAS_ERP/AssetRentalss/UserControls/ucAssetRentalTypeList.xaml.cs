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
    /// Interaction logic for ucAssetRentalTypeList.xaml
    /// </summary>
    public partial class ucAssetRentalTypeList : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        public ucAssetRentalTypeList()
        {
            InitializeComponent();
        }
        //comment
        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAssetRentalType ucFrmAssetRental = new ucFrmAssetRentalType();
            Window win = new Window();
            ucFrmAssetRental.editFlag = false;
            win.Content = ucFrmAssetRental;
            win.Width = 400;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlAssetTypeList.SelectedItem != null)
            {
                var selectedRow = grdCntrlAssetTypeList.SelectedItem as AssetType;
                ucFrmAssetRentalType ucFrmAssetRental = new ucFrmAssetRentalType();
                Window win = new Window();
                ucFrmAssetRental.assetTypeId = selectedRow.Id;
                ucFrmAssetRental.editFlag = true;
                win.Content = ucFrmAssetRental;
                win.Width = 400;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            rentalRepo = new AssetRentalRepo();
            var listt = rentalRepo.GetAllAssetType();
            grdCntrlAssetTypeList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rentalRepo = new AssetRentalRepo();
            var listt = rentalRepo.GetAllAssetType();
            grdCntrlAssetTypeList.ItemsSource = listt;
        }
    }
}
