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
    /// Interaction logic for ucAssetSubNatureList.xaml
    /// </summary>
    public partial class ucAssetSubNatureList : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        public ucAssetSubNatureList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAssetSubNatureAdd frmAssetBrand = new ucFrmAssetSubNatureAdd();
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
            if (grdCntrlAssetSubNatureList.SelectedItem != null)
            {
                var selectedRow = grdCntrlAssetSubNatureList.SelectedItem as RentalAssetSubNature;
                ucFrmAssetSubNatureAdd frmAssetBrand = new ucFrmAssetSubNatureAdd();
                Window win = new Window();
                frmAssetBrand.assetNatureId = selectedRow.Id;
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
            var listt = rentalRepo.GetAllRentalAssetSubNature();
            grdCntrlAssetSubNatureList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rentalRepo = new AssetRentalRepo();
            var listt = rentalRepo.GetAllRentalAssetSubNature();
            grdCntrlAssetSubNatureList.ItemsSource = listt;
        }

    }
}
