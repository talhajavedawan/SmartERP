using DevExpress.Xpf.Core;
using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucBillReferenceList.xaml
    /// </summary>
    public partial class ucBillReferenceList : UserControl
    {
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        public ucBillReferenceList()
        {
            InitializeComponent();
        }

        private void MbtnAddReference_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill Reference") != null)
            {
                ucFrmBillReferenceNo frmBillReferenceNo = new ucFrmBillReferenceNo();
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

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Bill Reference") != null)
            {
                var selectedRow = grdCntrlReferenceList.SelectedItem as BillRefNumber;

                if (selectedRow != null)
                {
                    ucFrmBillReferenceNo frmBillReferenceNo = new ucFrmBillReferenceNo();
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
            billsRepo = new AdminBillsRepo();
            grdCntrlReferenceList.ItemsSource = billsRepo.GetAllBillRefNo();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            billsRepo = new AdminBillsRepo();
            grdCntrlReferenceList.ItemsSource = billsRepo.GetAllBillRefNo();
        }
    }
}
