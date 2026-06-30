using ERP_BL.AssetsRentals.RentalInvoices;
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

namespace ZAS_ERP.AssetRentalss.RentalInvoicess.UserControls
{
    /// <summary>
    /// Interaction logic for ucRentalInvoiceStatusList.xaml
    /// </summary>
    public partial class ucRentalInvoiceStatusList : UserControl
    {
        RentalInvoiceRepo repo = new RentalInvoiceRepo();

        public ucRentalInvoiceStatusList()
        {
            InitializeComponent();
        }

        private void BtnNewStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Rental Invoice Status") != null)
            {
                Window win = new Window();
                ucRentalInvoiceStatusAdd frm = new ucRentalInvoiceStatusAdd();

                frm.saveEditFlag = false;

                win.Content = frm;
                win.Width = 400;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.ShowDialog();
                //RefreshData();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add Rental Invoice status!");
                return;
            }
        }

        private void BtnEditStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Update Rental Invoice Status") != null)
                {
                    Window win = new Window();
                    ucRentalInvoiceStatusAdd frm = new ucRentalInvoiceStatusAdd();

                    var selectedRow = grdStatus.SelectedItem as RentalInvoiceStatus;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.saveEditFlag = true;
                        frm.status = repo.GetRentalInvoiceStatus(selectedRow.Id);

                        //frm.txtStatus.Text = selectedRow.Status;
                        //frm.chkisActive.IsChecked = selectedRow.isActive;
                        //frm.cpStatus.Color = (Color)color;

                        win.Content = frm;
                        win.Width = 400;
                        win.Height = 250;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.ResizeMode = ResizeMode.CanMinimize;
                        win.ShowDialog();
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Rental Invoice status!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = repo.GetAllRentalInvoiceStatuses();
        }

        private void GrdStatus_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = repo.GetAllRentalInvoiceStatuses();
        }
    }
}
