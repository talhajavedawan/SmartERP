using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals;
using ERP_BL.Countryy;
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
    /// Interaction logic for ucFrmAssetRentalLocation.xaml
    /// </summary>
    public partial class ucFrmAssetRentalLocation : UserControl
    {
        public bool editFlag = false;
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        AssetRentalLocation assetRentalLocation = new AssetRentalLocation();
        public int assetRentalLocationId = 0;
        public ucFrmAssetRentalLocation()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCountries();
            if (editFlag == true)
            {
                assetRentalLocation = rentalRepo.GetAssetRentalLocation(assetRentalLocationId);


                txtLocationTitle.Text = assetRentalLocation.LocationTitle;

                if (assetRentalLocation.country != null)
                    lookupCountry.Text = assetRentalLocation.country.CountryName;

                if (assetRentalLocation.city != null)
                    lookupCity.Text = assetRentalLocation.city.CityName;
            }
        }
        private void LoadCountries()
        {
            CountryRepo countryRepo = new CountryRepo();
            lookupCountry.ItemsSource = countryRepo.GetAllCountries();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtLocationTitle.Text))
            {
                DXMessageBox.Show("Please Enter Location Title!");
                txtLocationTitle.Focus();
                return;
            }
            if (lookupCountry.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Country!");
                lookupCountry.Focus();
                return;
            }
            if (lookupCity.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select City!");
                lookupCity.Focus();
                return;
            }

            assetRentalLocation.LocationTitle = txtLocationTitle.Text;
            assetRentalLocation.cityId = (lookupCity.SelectedItem as City).Id;
            assetRentalLocation.countryId = (lookupCountry.SelectedItem as ERP_BL.Countryy.Country).Id;

            if (editFlag == false)
            {
                rentalRepo.AddAssetRentalLocation(assetRentalLocation);
                DXMessageBox.Show("Added Succesfully!");
            }
            else
            {
                rentalRepo.UpdateAssetRentalLocation(assetRentalLocation);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void lookupCountry_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var country = lookupCountry.SelectedItem as ERP_BL.Countryy.Country;
            CountryRepo countryRepo = new CountryRepo();

            if(country != null)
            {
                lookupCity.ItemsSource = countryRepo.GetAllCities().Where(x=>x.countryId == country.Id).ToList();
            }
        }

        private void lookupCity_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
