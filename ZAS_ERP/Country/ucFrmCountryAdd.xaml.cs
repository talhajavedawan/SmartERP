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
    /// Interaction logic for ucFrmCountryAdd.xaml
    /// </summary>
    public partial class ucFrmCountryAdd : UserControl
    {
        public bool editFlag = false;
        public int countryId = 0;
        ERP_BL.Countryy.Country country = new ERP_BL.Countryy.Country();
        CountryRepo countryRepo = new CountryRepo();
        public ucFrmCountryAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(editFlag == true)
            {
                country = countryRepo.GetCountry(countryId);

                txtCountryName.Text = country.CountryName;
                txtAbbriviation.Text = country.Abbriviation;
                txtCountryCode.Text = country.CountryCode;
                txtResidentDays.Text = country.ResidentDays.ToString();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if(String.IsNullOrEmpty(txtCountryName.Text))
            {
                DXMessageBox.Show("Please enter Country Name!");
                txtCountryName.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtAbbriviation.Text))
            {
                DXMessageBox.Show("Please enter Abbriviation!");
                txtAbbriviation.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtCountryCode.Text))
            {
                DXMessageBox.Show("Please enter Country Code!");
                txtCountryCode.Focus();
                return;
            }

            country.CountryName = txtCountryName.Text;
            country.Abbriviation = txtAbbriviation.Text;
            country.CountryCode = txtCountryCode.Text;
            country.ResidentDays = Convert.ToInt32(txtResidentDays.Text);

            if (countryRepo.CheckCountryName(countryId, country.CountryName) != null)
            {
                DXMessageBox.Show("Country Name already exist!");
                txtCountryName.Focus();
                return;
            }
            if (countryRepo.CheckAbbriviation(countryId, country.Abbriviation) != null)
            {
                DXMessageBox.Show("Abbriviation already exist!");
                txtAbbriviation.Focus();
                return;
            }
            if (countryRepo.CheckCountryCode(countryId, country.CountryCode) != null)
            {
                DXMessageBox.Show("Country Code already exist!");
                txtCountryCode.Focus();
                return;
            }

            if(editFlag == true)
            {
                countryRepo.UpdateCountry(country);
                DXMessageBox.Show("Updated Successfully!");

            }
            else
            {
                countryRepo.AddCountry(country);
                DXMessageBox.Show("Added Successfully!");
            }

            Window myWin = Window.GetWindow(this);
            myWin.Close();
        }
    }
}
