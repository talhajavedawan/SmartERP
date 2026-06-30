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
using ZAS_ERP.Procurementss.AdminBillss.UserControls;

namespace ZAS_ERP.AssetRentalss.UserControls
{
    /// <summary>
    /// Interaction logic for ucAssetBook.xaml
    /// </summary>
    public partial class ucAssetBookList : UserControl
    {
        public ucAssetBookList()
        {
            InitializeComponent();
        }

        private void grdAssetBook_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdAssetBook.GetRowByListIndex(e.ListSourceRowIndex) as AdminBill;

            if ( e.Column.FieldName == "billTypeee" && e.IsGetData)
            {
                if (row.isProgressiveCost == false)
                    e.Value = "Purchasing";
                else if (row.isProgressiveCost == true)
                    e.Value = "Progressive";
            }
            if (row.isProgressiveCost == false && e.Column.FieldName == "PurchasingCost" && e.IsGetData)
            {
                e.Value = Convert.ToDecimal(row.AmountOC);
            }
            if (row.isProgressiveCost == true && e.Column.FieldName == "ProgressiveCost" && e.IsGetData)
            {
                e.Value = Convert.ToDecimal(row.AmountOC);
            }
        }

        private void grdAssetBook_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill") != null)
            {
                ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
                Window frmBill = new Window();
                frmBill.WindowState = WindowState.Maximized;
                frmBill.Title = "Update Bills";

                var selectedRow = grdAssetBook.SelectedItem as AdminBill;

                if (selectedRow != null)
                {
                    //billsRepo = new AdminBillsRepo();
                    //frmBillAdd.bills = new List<AdminBill>();
                    //frmBillAdd.bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                    frmBillAdd.editFlag = true;
                    frmBillAdd.groupId = selectedRow.transactionGroupId;
                    frmBillAdd.isProgressiveCost = selectedRow.isProgressiveCost;
                    frmBillAdd.billTypes = selectedRow.billTypes;
                    frmBill.Content = frmBillAdd;
                    frmBill.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Existing Bill!");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AdminBillsRepo billsRepo = new AdminBillsRepo();
            grdAssetBook.ItemsSource = billsRepo.getAllAssetTypeBills(SYSTEM_STATIC.currentUser.id);
        }
    }
}
