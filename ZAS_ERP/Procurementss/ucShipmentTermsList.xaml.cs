using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for ucShipmentTermsList.xaml
    /// </summary>
    public partial class ucShipmentTermsList : UserControl
    {
        public SaleOrderRepo repo = new SaleOrderRepo();

        public ucShipmentTermsList()
        {
            InitializeComponent();
        }

        private void MbtnEditST_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Shipping Terms") != null)
            {
                var selectedRow = grdST.SelectedItem as ShippingTerm;

                if (selectedRow != null)
                {
                    ucSTAdd frmST = new ucSTAdd();
                    frmST.editFlag = true;
                    frmST.term = repo.GetST(selectedRow.Id);
                    Window win = new Window();
                    win.Content = frmST;
                    win.Height = 350;
                    win.Width = 400;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ResizeMode = ResizeMode.CanMinimize;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Edit Shipping Terms!");
            }
        }

        private void MbtnAddST_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Shipping Terms") != null)
            {
                ucSTAdd term = new ucSTAdd();
                term.editFlag = false;
                Window win = new Window();
                win.Content = term;
                win.Height = 350;
                win.Width = 400;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add Shipping Terms!");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                repo = new SaleOrderRepo();
                grdST.ItemsSource = repo.GetShippingTerms();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try { 
            repo = new SaleOrderRepo();
            grdST.ItemsSource = repo.GetShippingTerms();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
