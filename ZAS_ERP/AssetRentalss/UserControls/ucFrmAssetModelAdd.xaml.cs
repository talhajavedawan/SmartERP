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
    /// Interaction logic for ucFrmAssetModelAdd.xaml
    /// </summary>
    public partial class ucFrmAssetModelAdd : UserControl
    {
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        AssetModel assetModel = new AssetModel();
        public bool editFlag = false;
        public int assetModelId = 0;
        public ucFrmAssetModelAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadParentAssetType();

            if (editFlag == true)
            {
                assetModel = rentalRepo.GetAssetModel(assetModelId);

                txtAssetModel.Text = assetModel.ModelNumber;

                chkIsActive.IsChecked = assetModel.isActive;

                if (assetModel.assetNature != null)
                    lookupAssetNature.Text = assetModel.assetNature.NatureName;

                if (assetModel.AssetSubNatures != null && assetModel.AssetSubNatures.Count > 0)
                {
                    foreach (RentalAssetSubNature assetSubNature in assetModel.AssetSubNatures)
                    {
                        grdAssetSubNature.SelectItem(grdAssetSubNature.FindRowByValue(grdAssetSubNature.Columns.GetColumnByFieldName("Id"), assetSubNature.Id));
                    }
                }

                if (assetModel.assetBrands != null && assetModel.assetBrands.Count > 0)
                {
                    foreach (AssetBrand assetBrand in assetModel.assetBrands)
                    {
                        grdAssetBrand.SelectItem(grdAssetBrand.FindRowByValue(grdAssetBrand.Columns.GetColumnByFieldName("Id"), assetBrand.Id));
                    }
                }

                //if (assetModel.assetSubNature != null)
                //    lookupAssetSubNature.Text = assetModel.assetSubNature.NatureName;

                //if (assetModel.assetBrand != null)
                //    lookupAssetBrand.Text = assetModel.assetBrand.BrandName;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtAssetModel.Text))
            {
                DXMessageBox.Show("Plase enter Asset Brand!");
                txtAssetModel.Focus();
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
            if (grdAssetBrand.SelectedItems == null || grdAssetBrand.SelectedItems.Count == 0)
            {
                DXMessageBox.Show("Plase select Asset Brand!");
                grdAssetBrand.Focus();
                return;
            }

            assetModel.ModelNumber = txtAssetModel.Text;
            assetModel.isActive = chkIsActive.IsChecked.Value;

            assetModel.assetNatureId = (lookupAssetNature.SelectedItem as RentalAssetNature).Id;

            assetModel.AssetSubNatures = new List<RentalAssetSubNature>();
            foreach (RentalAssetSubNature assetSubNature in grdAssetSubNature.SelectedItems)
            {
                assetModel.AssetSubNatures.Add(assetSubNature);
            }

            assetModel.assetBrands = new List<AssetBrand>();
            foreach (AssetBrand assetBrand in grdAssetBrand.SelectedItems)
            {
                assetModel.assetBrands.Add(assetBrand);
            }

            //assetModel.assetBrandId = (lookupAssetBrand.SelectedItem as AssetBrand).Id;

            if (editFlag == true)
            {
                rentalRepo.UpdateAssetModel(assetModel);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                rentalRepo.AddAssetModel(assetModel);
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

        private void lookupAssetSubNature_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void grdAssetSubNature_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            var nature = lookupAssetNature.SelectedItem as RentalAssetNature;
            List<RentalAssetSubNature> subNatures = new List<RentalAssetSubNature>();
            
            foreach(RentalAssetSubNature subNature in grdAssetSubNature.SelectedItems)
            {
                subNatures.Add(subNature);
            }

            List<AssetBrand> assetBrands = new List<AssetBrand>();
            foreach (AssetBrand brand in grdAssetBrand.SelectedItems)
            {
                assetBrands.Add(brand);
            }

            var subNatureIds = subNatures.Select(x=>x.Id);

            if (nature != null && (subNatures != null || subNatures.Count > 0))
            {
                grdAssetBrand.ItemsSource = rentalRepo.GetAllAssetBrand().Where(x => x.assetNatureId == nature.Id && subNatureIds.Intersect(x.AssetSubNatures.Select(y => y.Id)).Count() > 0).ToList();

                foreach (AssetBrand assetBrand in assetBrands)
                {
                    grdAssetBrand.SelectItem(grdAssetBrand.FindRowByValue(grdAssetBrand.Columns.GetColumnByFieldName("Id"), assetBrand.Id));
                }
            }
                
        }
    }
}
