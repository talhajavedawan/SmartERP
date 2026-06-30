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
    /// Interaction logic for ucFrmAssetRentalUnit.xaml
    /// </summary>
    public partial class ucFrmAssetRentalUnit : UserControl
    {
        public bool editFlag = false;
        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        AssetRentalUnit assetRentalUnit = new AssetRentalUnit();
        public int assetRentalUnitId = 0;
        public ucFrmAssetRentalUnit()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCountries();
            if (editFlag == true)
            {
                assetRentalUnit = rentalRepo.GetAssetRentalUnit(assetRentalUnitId);


                txtUnitNo.Text = assetRentalUnit.UnitNo;

                if (assetRentalUnit.country != null)
                    lookupCountry.Text = assetRentalUnit.country.CountryName;

                if (assetRentalUnit.city != null)
                    lookupCity.Text = assetRentalUnit.city.CityName;

                if (assetRentalUnit.assetRentalLocation != null)
                    lookupLocation.Text = assetRentalUnit.assetRentalLocation.LocationTitle;
            }
        }
        private void LoadCountries()
        {
            CountryRepo countryRepo = new CountryRepo();
            lookupCountry.ItemsSource = countryRepo.GetAllCountries();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtUnitNo.Text))
            {
                DXMessageBox.Show("Please Enter Location Title!");
                txtUnitNo.Focus();
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
            if (lookupLocation.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Location!");
                lookupLocation.Focus();
                return;
            }

            assetRentalUnit.UnitNo = txtUnitNo.Text;
            assetRentalUnit.cityId = (lookupCity.SelectedItem as City).Id;
            assetRentalUnit.countryId = (lookupCountry.SelectedItem as ERP_BL.Countryy.Country).Id;
            assetRentalUnit.assetRentalLocationId = (lookupLocation.SelectedItem as AssetRentalLocation).Id;

            if (editFlag == false)
            {
                rentalRepo.AddAssetRentalUnit(assetRentalUnit);
                DXMessageBox.Show("Added Succesfully!");
            }
            else
            {
                rentalRepo.UpdateAssetRentalUnit(assetRentalUnit);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void lookupCountry_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var country = lookupCountry.SelectedItem as ERP_BL.Countryy.Country;
            CountryRepo countryRepo = new CountryRepo();

            if (country != null)
            {
                lookupCity.ItemsSource = countryRepo.GetAllCities().Where(x => x.countryId == country.Id).ToList();
            }
        }

        private void lookupCity_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var country = lookupCountry.SelectedItem as ERP_BL.Countryy.Country;

            var city = lookupCity.SelectedItem as ERP_BL.Countryy.City;
            

            if (country != null && city != null)
            {
                lookupLocation.ItemsSource = rentalRepo.GetAllAssetRentalLocationByCountryCity(country, city);
            }
        }

        private void lookupLocation_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
