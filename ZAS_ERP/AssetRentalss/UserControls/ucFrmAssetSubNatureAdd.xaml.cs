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
    /// Interaction logic for ucFrmAssetSubNatureAdd.xaml
    /// </summary>
    public partial class ucFrmAssetSubNatureAdd : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        RentalAssetSubNature assetNature = new RentalAssetSubNature();
        public bool editFlag = false;
        public int assetNatureId = 0;
        public ucFrmAssetSubNatureAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadParentAssetType();

            if (editFlag == true)
            {
                assetNature = rentalRepo.GetRentalAssetSubNature(assetNatureId);

                txtAssetSubNature.Text = assetNature.NatureName;

                chkIsActive.IsChecked = assetNature.isActive;

                if (assetNature.assetNature != null)
                    lookupAssetNature.Text = assetNature.assetNature.NatureName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtAssetSubNature.Text))
            {
                DXMessageBox.Show("Plase enter Asset Asset Sub Nature!");
                txtAssetSubNature.Focus();
                return;
            }
            if (lookupAssetNature.SelectedIndex < 0)
            {
                DXMessageBox.Show("Plase select Asset Nature!");
                lookupAssetNature.Focus();
                return;
            }

            assetNature.NatureName = txtAssetSubNature.Text;
            assetNature.isActive = chkIsActive.IsChecked.Value;

            assetNature.assetNatureId = (lookupAssetNature.SelectedItem as RentalAssetNature).Id;

            if (editFlag == true)
            {
                rentalRepo.UpdateRentalAssetSubNature(assetNature);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                rentalRepo.AddRentalAssetSubNature(assetNature);
                DXMessageBox.Show("Added Successfully!");
            }

            var myWindows = Window.GetWindow(this);
            myWindows.Close();

        }

        private void LoadParentAssetType()
        {
            lookupAssetNature.ItemsSource = rentalRepo.GetAllAssetNature();
        }

    }
}
