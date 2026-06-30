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
    /// Interaction logic for ucAssetNumberList.xaml
    /// </summary>
    public partial class ucAssetNumberList : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();

        public ucAssetNumberList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAssetNumberAdd ucFrmAssetNumber = new ucFrmAssetNumberAdd();
            Window win = new Window();
            ucFrmAssetNumber.editFlag = false;
            win.Content = ucFrmAssetNumber;
            win.Width = 400;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlAssetNumberList.SelectedItem != null)
            {
                var selectedRow = grdCntrlAssetNumberList.SelectedItem as AssetNumber;
                ucFrmAssetNumberAdd ucFrmAssetNumber = new ucFrmAssetNumberAdd();
                Window win = new Window();
                ucFrmAssetNumber.assetNumberId = selectedRow.Id;
                ucFrmAssetNumber.editFlag = true;
                win.Content = ucFrmAssetNumber;
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
            var listt = rentalRepo.GetAllAssetNumber();
            grdCntrlAssetNumberList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rentalRepo = new AssetRentalRepo();
            var listt = rentalRepo.GetAllAssetNumber();
            grdCntrlAssetNumberList.ItemsSource = listt;
        }

    }
}
