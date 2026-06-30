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
    /// Interaction logic for ucCityList.xaml
    /// </summary>
    public partial class ucCityList : UserControl
    {
        CountryRepo countryRepo = new CountryRepo();
        public ucCityList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();

            ucFrmCityAdd frmCountryAdd = new ucFrmCityAdd();
            win.Content = frmCountryAdd;
            win.Height = 350;
            win.Width = 500;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Title = "Add City";
            win.Show();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = grdCntrlCityList.SelectedItem as ERP_BL.Countryy.Country;
            if (selectedRow != null)
            {
                Window win = new Window();

                ucFrmCityAdd frmCountryAdd = new ucFrmCityAdd();
                frmCountryAdd.cityId = selectedRow.Id;
                frmCountryAdd.editFlag = true;
                win.Content = frmCountryAdd;
                win.Height = 350;
                win.Width = 500;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Title = "Update City";
                win.Show();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlCityList.ItemsSource = countryRepo.GetAllCities();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            countryRepo = new CountryRepo();
            grdCntrlCityList.ItemsSource = countryRepo.GetAllCities();
        }

    }
}
