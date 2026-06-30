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
    /// Interaction logic for ucFrmAssetNature.xaml
    /// </summary>
    public partial class ucFrmAssetNature : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        RentalAssetNature assetNature = new RentalAssetNature();
        public bool editFlag = false;
        public int assetNatureId = 0;

        public ucFrmAssetNature()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                assetNature = rentalRepo.GetAssetNature(assetNatureId);

                txtAssetNature.Text = assetNature.NatureName;

                chkIsActive.IsChecked = assetNature.isActive;

                
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtAssetNature.Text))
            {
                DXMessageBox.Show("Plase enter Asset Nature!");
                txtAssetNature.Focus();
                return;
            }
            

            assetNature.NatureName = txtAssetNature.Text;
            assetNature.isActive = chkIsActive.IsChecked.Value;

           

            if(editFlag == true)
            {
                rentalRepo.UpdateAssetNature(assetNature);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                rentalRepo.AddAssetNature(assetNature);
                DXMessageBox.Show("Added Successfully!");
            }

            var myWindows = Window.GetWindow(this);
            myWindows.Close();

        }
    }
}
