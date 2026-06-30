using DevExpress.Xpf.Core;
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
    /// Interaction logic for ucFrmAssetNumberAdd.xaml
    /// </summary>
    public partial class ucFrmAssetNumberAdd : UserControl
    {
        public bool editFlag = false;
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        AssetNumber assetNumber = new AssetNumber();
        public int assetNumberId = 0;
        public ucFrmAssetNumberAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                assetNumber = rentalRepo.GetAssetNumber(assetNumberId);

                txtAssetNumber.Text = assetNumber.Number;
                chkIsActive.IsChecked = assetNumber.isActive;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtAssetNumber.Text))
            {
                DXMessageBox.Show("Please Enter Asset Type!");
                txtAssetNumber.Focus();
                return;
            }

            assetNumber.Number = txtAssetNumber.Text;

            if (chkIsActive.IsChecked == true)
                assetNumber.isActive = true;
            else
                assetNumber.isActive = false;

            if (editFlag == false)
            {
                rentalRepo.AddAssetNumber(assetNumber);
                DXMessageBox.Show("Added Succesfully!");
            }
            else
            {
                rentalRepo.UpdateAssetNumber(assetNumber);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

    }
}
