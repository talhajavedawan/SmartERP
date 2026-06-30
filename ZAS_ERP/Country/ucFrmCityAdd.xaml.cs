using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.Country
{
    /// <summary>
    /// Interaction logic for ucFrmCityAdd.xaml
    /// </summary>
    public partial class ucFrmCityAdd : UserControl
    {
        public bool editFlag = false;
        public int cityId = 0;
        ERP_BL.Countryy.City city = new ERP_BL.Countryy.City();
        CountryRepo countryRepo = new CountryRepo();
        public ucFrmCityAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCountries();
            if (editFlag == true)
            {
                city = countryRepo.GetCity(cityId);

                txtCityName.Text = city.CityName;

                if (city.country != null)
                    lookupCountry.Text = city.country.CountryName;

                txtAbbriviation.Text = city.Abbriviation;
                txtPostalCode.Text = city.PostalCode;
                
            }
        }

        private void LoadCountries()
        {
            lookupCountry.ItemsSource = countryRepo.GetAllCountries();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtCityName.Text))
            {
                DXMessageBox.Show("Please enter Country Name!");
                txtCityName.Focus();
                return;
            }
            if (String.IsNullOrEmpty(lookupCountry.Text))
            {
                DXMessageBox.Show("Please select Country!");
                lookupCountry.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtAbbriviation.Text))
            {
                DXMessageBox.Show("Please enter Abbriviation!");
                txtAbbriviation.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtPostalCode.Text))
            {
                DXMessageBox.Show("Please enter Country Code!");
                txtPostalCode.Focus();
                return;
            }

            city.CityName = txtCityName.Text;
            city.Abbriviation = txtAbbriviation.Text;
            city.PostalCode = txtPostalCode.Text;
            city.countryId = (lookupCountry.SelectedItem as ERP_BL.Countryy.Country).Id;

            if (countryRepo.CheckCityName(cityId, city.CityName) != null)
            {
                DXMessageBox.Show("City Name already exist!");
                txtCityName.Focus();
                return;
            }
            if (countryRepo.CheckCityAbbriviation(cityId, city.Abbriviation) != null)
            {
                DXMessageBox.Show("Abbriviation already exist!");
                txtAbbriviation.Focus();
                return;
            }
            if (countryRepo.CheckCityPostalCode(cityId, city.PostalCode) != null)
            {
                DXMessageBox.Show("Country Code already exist!");
                txtPostalCode.Focus();
                return;
            }

            if (editFlag == true)
            {
                countryRepo.UpdateCity(city);
                DXMessageBox.Show("Updated Successfully!");

            }
            else
            {
                countryRepo.AddCity(city);
                DXMessageBox.Show("Added Successfully!");
            }

            Window myWin = Window.GetWindow(this);
            myWin.Close();
        }

    }
}
