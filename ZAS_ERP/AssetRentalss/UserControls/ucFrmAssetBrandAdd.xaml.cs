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
    /// Interaction logic for ucFrmAssetBrandAdd.xaml
    /// </summary>
    public partial class ucFrmAssetBrandAdd : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        AssetBrand assetBrand = new AssetBrand();
        public bool editFlag = false;
        public int assetBrandId = 0;
        public ucFrmAssetBrandAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadParentAssetType();

            if (editFlag == true)
            {
                assetBrand = rentalRepo.GetAssetBrand(assetBrandId);

                txtAssetBrand.Text = assetBrand.BrandName;

                chkIsActive.IsChecked = assetBrand.isActive;

                if (assetBrand.assetNature != null)
                    lookupAssetNature.Text = assetBrand.assetNature.NatureName;

                if (assetBrand.AssetSubNatures != null && assetBrand.AssetSubNatures.Count > 0)
                {
                    foreach (RentalAssetSubNature assetSubNature in assetBrand.AssetSubNatures)
                    {
                        grdAssetSubNature.SelectItem(grdAssetSubNature.FindRowByValue(grdAssetSubNature.Columns.GetColumnByFieldName("Id"), assetSubNature.Id));
                    }
                }
                //lookupAssetSubNature.Text = assetBrand.assetSubNature.NatureName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtAssetBrand.Text))
            {
                DXMessageBox.Show("Plase enter Asset Brand!");
                txtAssetBrand.Focus();
                return;
            }
            if (lookupAssetNature.SelectedIndex < 0)
            {
                DXMessageBox.Show("Plase select Asset Nature!");
                lookupAssetNature.Focus();
                return;
            }
            if (grdAssetSubNature.SelectedItems == null || grdAssetSubNature.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Plase select Asset Sub Nature!");
                grdAssetSubNature.Focus();
                return;
            }

            assetBrand.BrandName = txtAssetBrand.Text;
            assetBrand.isActive = chkIsActive.IsChecked.Value;

            assetBrand.assetNatureId = (lookupAssetNature.SelectedItem as RentalAssetNature).Id;
            //assetBrand.assetSubNatureId = (lookupAssetSubNature.SelectedItem as RentalAssetSubNature).Id;

            assetBrand.AssetSubNatures = new List<RentalAssetSubNature>();
            foreach (RentalAssetSubNature assetSubNature in grdAssetSubNature.SelectedItems)
            {
                assetBrand.AssetSubNatures.Add(assetSubNature);
            }


            if (editFlag == true)
            {
                rentalRepo.UpdateAssetBrand(assetBrand);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                rentalRepo.AddAssetBrand(assetBrand);
                DXMessageBox.Show("Added Successfully!");
            }

            var myWindows = Window.GetWindow(this);
            myWindows.Close();

        }

        private void LoadParentAssetType()
        {
            lookupAssetNature.ItemsSource = rentalRepo.GetAllAssetNature();
        }

        private void lookupAssetNature_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var nature = lookupAssetNature.SelectedItem as RentalAssetNature;

            if (nature != null)
            {
                var natureList = rentalRepo.GetAllRentalAssetSubNature().Where(x => x.assetNatureId == nature.Id);
                grdAssetSubNature.ItemsSource = natureList;
            }
                
        }
    }
}
