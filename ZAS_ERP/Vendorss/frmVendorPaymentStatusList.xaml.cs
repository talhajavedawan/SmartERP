using DevExpress.Xpf.Grid;
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
using ERP_BL;

using ERP_BL.Config;
using ERP_BL.Enums;

namespace ZAS_ERP.Vendorss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmVendorPaymentStatusList : Window
    {


        List<VendorPaymentStatus> VendorPaymentStatuss= new List<VendorPaymentStatus>();
        VendorRepo repo = new VendorRepo();

        //VendorPaymentStatus VendorPaymentStatus = new VendorPaymentStatus();
        public frmVendorPaymentStatusList()
        {
            InitializeComponent();
            
            
        }

        private void winVendorPaymentStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadVendorPaymentStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdVendorPaymentStatus);


        }



        private void loadVendorPaymentStatus()
        {
           
            if (MainWindow.currentUserid == 0)
                VendorPaymentStatuss = repo.getAllVendorPaymentStatus();
            
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Vendor Payment Statuses") != null)
            {
                VendorPaymentStatuss = repo.getAllVendorPaymentStatus();
            }
            else
            {
                VendorPaymentStatuss = repo.getAllActiveVendorPaymentStatus();
            }
            //VendorPaymentStatuss = repo.getAllActiveVendorPaymentStatus();

            
            this.grdVendorPaymentStatus.ItemsSource = VendorPaymentStatuss;
            grdVendorPaymentStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdVendorPaymentStatus.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdVendorPaymentStatus.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newVendorPaymentStatus()
        {
            frmVendorPaymentStatusAdd frmVendorPaymentStatusadd = new frmVendorPaymentStatusAdd();
            frmVendorPaymentStatusadd.ShowDialog();
            
            loadVendorPaymentStatus();
        }
        

        private void mbtnNewVendorPaymentStatus_Click(object sender, RoutedEventArgs e)
        {
            newVendorPaymentStatus();

        }

        private void mbtnEditVendorPaymentStatus_Click(object sender, RoutedEventArgs e)
        {
            if (grdVendorPaymentStatus.SelectedItem != null)
            {

                Vendorss.frmVendorPaymentStatusAdd.VendorPaymentStatusId = (grdVendorPaymentStatus.SelectedItem as VendorPaymentStatus).Id;
                newVendorPaymentStatus();
            }
            else
            {
                MessageBox.Show("Please select a Vendor Payment Status to Edit");
            }
        }



        private void grdVendorPaymentStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Vendorss.frmVendorPaymentStatusAdd.VendorPaymentStatusId = (grdVendorPaymentStatus.SelectedItem as VendorPaymentStatus).Id;
            newVendorPaymentStatus();
        }

        private void WinVendorPaymentStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdVendorPaymentStatus);
        }
        //int empid;



    }
}
