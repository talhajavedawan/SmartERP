using ERP_BL.Databases;
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

namespace ZAS_ERP.Procurementss.SaleOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucSORefKeys.xaml
    /// </summary>
    public partial class ucSORefKeys : UserControl
    {
        public ucSORefKeys()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            grdRefKeys.ItemsSource = saleOrderRepo.GetAllSORefKeys();
        }

        private void grdRefKeys_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update SO Ref Keys") != null)
            {
                ucGenerateSoKey ucGenerateSoKey = new ucGenerateSoKey((grdRefKeys.SelectedItem as SaleOrdeRrefKey).Id, true);
                ucGenerateSoKey.Show();
            }
        }
    }
}
