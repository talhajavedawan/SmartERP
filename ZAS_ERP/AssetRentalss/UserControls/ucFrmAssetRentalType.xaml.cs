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
    /// Interaction logic for ucFrmAssetType.xaml
    /// </summary>
    public partial class ucFrmAssetRentalType : UserControl
    {
        public bool editFlag = false;
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        AssetType assetType = new AssetType();
        public int assetTypeId = 0;
        public ucFrmAssetRentalType()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                assetType = rentalRepo.GetAssetType(assetTypeId);

                txtAssetType.Text = assetType.TypeName;
                chkIsActive.IsChecked = assetType.isActive;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtAssetType.Text))
            {
                DXMessageBox.Show("Please Enter Asset Type!");
                txtAssetType.Focus();
                return;
            }

            assetType.TypeName = txtAssetType.Text;

            if (chkIsActive.IsChecked == true)
                assetType.isActive = true;
            else
                assetType.isActive = false;

            if (editFlag == false)
            {
                rentalRepo.AddAssetType(assetType);
                DXMessageBox.Show("Added Succesfully!");
            }
            else
            {
                rentalRepo.UpdateAssetType(assetType);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
