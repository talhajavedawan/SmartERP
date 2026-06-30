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
    /// Interaction logic for ucAssetNatureList.xaml
    /// </summary>
    public partial class ucAssetNatureList : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        public ucAssetNatureList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAssetNature frmAssetNature = new ucFrmAssetNature();
            Window win = new Window();
            frmAssetNature.editFlag = false;
            win.Content = frmAssetNature;
            win.Width = 400;
            win.Height = 350;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }
        //comment
        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlAssetNatureList.SelectedItem != null)
            {
                var selectedRow = grdCntrlAssetNatureList.SelectedItem as RentalAssetNature;
                ucFrmAssetNature frmAssetNature = new ucFrmAssetNature();
                Window win = new Window();
                frmAssetNature.assetNatureId = selectedRow.Id;
                frmAssetNature.editFlag = true;
                win.Content = frmAssetNature;
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
            var listt = rentalRepo.GetAllAssetNature();
            grdCntrlAssetNatureList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rentalRepo = new AssetRentalRepo();
            var listt = rentalRepo.GetAllAssetNature();
            grdCntrlAssetNatureList.ItemsSource = listt;
        }
    }
}
