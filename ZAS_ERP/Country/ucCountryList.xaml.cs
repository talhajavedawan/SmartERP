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
    /// Interaction logic for ucCountryList.xaml
    /// </summary>
    public partial class ucCountryList : UserControl
    {
        CountryRepo countryRepo = new CountryRepo();
        public ucCountryList()
        {
            InitializeComponent();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();

            ucFrmCountryAdd frmCountryAdd = new ucFrmCountryAdd();
            win.Content = frmCountryAdd;
            win.Height = 350;
            win.Width = 500;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Title = "Add Country";
            win.Show();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = grdCntrlCountryList.SelectedItem as ERP_BL.Countryy.Country;
            if (selectedRow != null)
            {
                Window win = new Window();

                ucFrmCountryAdd frmCountryAdd = new ucFrmCountryAdd();
                frmCountryAdd.countryId = selectedRow.Id;
                frmCountryAdd.editFlag = true;
                win.Content = frmCountryAdd;
                win.Height = 350;
                win.Width = 500;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Title = "Update Country";
                win.Show();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlCountryList.ItemsSource = countryRepo.GetAllCountries();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            countryRepo = new CountryRepo();
            grdCntrlCountryList.ItemsSource = countryRepo.GetAllCountries();
        }
    }
}
