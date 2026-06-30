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
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP.Vendorss
{
    /// <summary>
    /// Interaction logic for frmVendorPaymentStatusAdd.xaml
    /// </summary>
    public partial class frmVendorPaymentStatusAdd : Window
    {
        public frmVendorPaymentStatusAdd()
        {
            InitializeComponent();
        }
        public static int VendorPaymentStatusId;
        VendorRepo repo = new VendorRepo();
        VendorPaymentStatus VendorPaymentStatus = new VendorPaymentStatus();
        private void btnMeasureSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            
            VendorPaymentStatus.Status= txtMeasure.Text.Trim();
            if (chkisactive.IsChecked == true)
                VendorPaymentStatus.isActive = true;
            else
                VendorPaymentStatus.isActive = false;
            if (VendorPaymentStatus.Id == 0)
            {
                    //if (MainWindow.currentUserid != 0)
                    //    VendorPaymentStatus.user_Id = MainWindow.currentUserid;
                    //else
                    //    VendorPaymentStatus.user_Id = null;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Payment Status") != null)
                    {
                        repo.addStatus(VendorPaymentStatus);
                        SystemLog.LogInfo(this.GetType(), "New Vendor Payment Status(" + txtMeasure.Text + ") Added");
                        MessageBox.Show("New Vendor Payment Status (" + txtMeasure.Text + ") Added", "Congratulations");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new Vendor Payment status.");
            }
            else
            {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Vendor Payment Status") != null)
                    {
                        repo.updateStatus(VendorPaymentStatus);

                        MessageBox.Show("Vendor Payment Status (" + txtMeasure.Text + ") updated", "Congratulations");
                        SystemLog.LogInfo(this.GetType(), " Vendor Payment Status(" + txtMeasure.Text + ") updated");
                    }
                    else
                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission Required!!! You are not Allowed to add new Vendor Payment status.");
                //    repo.updateStatus(VendorPaymentStatus);

                //MessageBox.Show("Vendor Payment Status (" + txtMeasure.Text + ") updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                SystemLog.LogError(this.GetType(), " Vendor Payment Status error" + ex.ToString());

                MessageBox.Show(ex.ToString());
            }
        }

        private void winVendorPaymentStatusAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            VendorPaymentStatusId = 0;
        }

        private void winVendorPaymentStatusAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (VendorPaymentStatusId != 0)
            {
                VendorPaymentStatus = repo.getstatus(VendorPaymentStatusId);
                txtMeasure.Text = VendorPaymentStatus.Status;
                chkisactive.IsChecked = VendorPaymentStatus.isActive;
            }
        }
    }
}
