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

namespace ZAS_ERP.Procurementss.Billss.UserControls
{
    /// <summary>
    /// Interaction logic for UcVendorBillRefList.xaml
    /// </summary>
    public partial class ucVendorBillRefList : UserControl
    {
        BillRepo billsRepo = new BillRepo();
        public ucVendorBillRefList()
        {
            InitializeComponent();
        }

        private void MbtnAddReference_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Bill Reference") != null)
            {
                ucFrmVendorBillRef frmBillReferenceNo = new ucFrmVendorBillRef();
                frmBillReferenceNo.editFlag = false;
                Window win = new Window();
                win.Content = frmBillReferenceNo;
                win.Height = 350;
                win.Width = 400;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add Bill Reference!");
            }

        }

        private void MbtnEditReference_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Vendor Bill Reference") != null)
            {
                var selectedRow = grdCntrlReferenceList.SelectedItem as VendorBillReference;

                if (selectedRow != null)
                {
                    ucFrmVendorBillRef frmBillReferenceNo = new ucFrmVendorBillRef();
                    frmBillReferenceNo.editFlag = true;
                    frmBillReferenceNo.billRef = billsRepo.GetBillReferenceNo(selectedRow.Id);
                    Window win = new Window();
                    win.Content = frmBillReferenceNo;
                    win.Height = 350;
                    win.Width = 400;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ResizeMode = ResizeMode.CanMinimize;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Edit Bill Reference!");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billsRepo = new BillRepo();

            var references = billsRepo.GetAllBillReferenceNo();
            grdCntrlReferenceList.ItemsSource = references.Where(x=>x.isActive == true).ToList();
            grdCntrlInActiveReferenceList.ItemsSource = references.Where(x => x.isActive == false).ToList();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            billsRepo = new BillRepo();
            var references = billsRepo.GetAllBillReferenceNo();
            grdCntrlReferenceList.ItemsSource = references.Where(x => x.isActive == true).ToList();
            grdCntrlInActiveReferenceList.ItemsSource = references.Where(x => x.isActive == false).ToList();
        }
    }
}
