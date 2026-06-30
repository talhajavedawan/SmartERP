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
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.PurchaseInvoice
{
    /// <summary>
    /// Interaction logic for frmPurchaseInvoiceStatusList.xaml
    /// </summary>
    public partial class frmPurchaseInvoiceStatusList : Window
    {
        List<PurchaseInvoiceStatus> PurchaseInvoiceStatuss = new List<PurchaseInvoiceStatus>();
        PurchaseInvoiceRepo repo = new PurchaseInvoiceRepo();
        public frmPurchaseInvoiceStatusList()
        {
            InitializeComponent();
        }

        private void MbtnNewPurchaseInvoiceStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice Status") != null)
            {
                newPurchaseInvoiceStatus();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Status!");
            }
        }


        private void MbtnEditPurchaseInvoiceStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Purchase Invoice Status") != null)
            {
                if (grdPurchaseInvoiceStatus.SelectedItem != null)
                {

                    PurchaseInvoice.frmPurchaseInvoiceStatussAdd.StatusId = (grdPurchaseInvoiceStatus.SelectedItem as PurchaseInvoiceStatus).Id;

                    newPurchaseInvoiceStatus();
                }
                else
                {
                    MessageBox.Show("Please select a PurchaseInvoice Status to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Status!");
            }
        }

        private void GrdPurchaseInvoiceStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void loadPurchaseInvoiceStatus()
        {

            if (MainWindow.currentUserid == 0)
                PurchaseInvoiceStatuss = repo.getAllPurchaseInvoiceStatus();

            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Invoice Statuses") != null)
            {
                PurchaseInvoiceStatuss = repo.getAllPurchaseInvoiceStatus();
            }
            else
            {
                PurchaseInvoiceStatuss = repo.getAllActivePurchaseInvoiceStatus();
            }


            this.grdPurchaseInvoiceStatus.ItemsSource = PurchaseInvoiceStatuss;
            grdPurchaseInvoiceStatus.Columns.GetColumnByFieldName("Id").Visible = false;   
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadPurchaseInvoiceStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPurchaseInvoiceStatus);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdPurchaseInvoiceStatus);

        }
        public void newPurchaseInvoiceStatus()
        {
            frmPurchaseInvoiceStatussAdd frmPuchaseInvoiceStatussadd = new frmPurchaseInvoiceStatussAdd();
            frmPuchaseInvoiceStatussadd.ShowDialog();

            loadPurchaseInvoiceStatus();
        }
    }
}
