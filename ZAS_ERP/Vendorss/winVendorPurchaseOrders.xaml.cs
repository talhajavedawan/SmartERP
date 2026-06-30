using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Vendorss
{
    /// <summary>
    /// Interaction logic for winVendorPurchaseOrders.xaml
    /// </summary>
    public partial class winVendorPurchaseOrders : DXWindow
    {
        PurchaseOrderRepo repo = new PurchaseOrderRepo();
        public winVendorPurchaseOrders()
        {
            InitializeComponent();
        }

        private void grdVendorcompanies_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            if (grdVendorcompanies.SelectedItem != null)
            {
                var vendor = grdVendorcompanies.SelectedItem as Vendor;
                grdVendorPurchaseOrders.ItemsSource = vendor.PurchaseOrders;
            }
        }
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loadgrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdVendorPurchaseOrders);
        }
        private void loadgrid()
        {
            Vendor vendor = new Vendor();
            VendorRepo vendorRepo = new VendorRepo();
            if (MainWindow.currentUserid == 0)
            {
                List<ERP_BL.Databases.Vendor> companyRepos = new List<ERP_BL.Databases.Vendor>();
                companyRepos = vendorRepo.getAll();
                this.grdVendorcompanies.ItemsSource = companyRepos;
                return;

            }
            List<Vendor> VendorCompanies = new List<Vendor>();
            EmployeeRepo employeeRepo = new EmployeeRepo();
            //var currentuser =employeeRepo.getuser(MainWindow.currentUserid);
            var currentemployee = employeeRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Customers") != null)
            {
                foreach (var dept in currentemployee.departments)
                {
                    foreach (var cust in dept.Vendors)
                    {
                        if (!VendorCompanies.Contains(cust))
                        {
                            VendorCompanies.Add(cust);
                        }
                    }
                }
            }
            else
            {
                foreach (var dept in currentemployee.departments)
                {
                    foreach (var cust in dept.Vendors)
                    {
                        if (!VendorCompanies.Contains(cust) && cust.isActive == true)
                        {
                            VendorCompanies.Add(cust);
                        }
                    }
                }
            }
            this.grdVendorcompanies.ItemsSource = VendorCompanies;
        }
        private void grdVendorPurchaseOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(grdVendorPurchaseOrders.SelectedItem!=null)
            {
                var purchaseOrder =   grdVendorPurchaseOrders.SelectedItem as PurchaseOrder;
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, purchaseOrder.Id);
                procurmentPanel.Show();
            }
        }
        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Stage" && e.IsGetData)
            {
                var purchaseOrder = grdVendorPurchaseOrders.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;

                if (purchaseOrder != null)
                {
                    if (purchaseOrder.isVoid == true)
                    {
                        e.Value = "Void";
                    }
                    else if (purchaseOrder.isReApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (purchaseOrder.isApproved == true && purchaseOrder.stage == "Closed")
                    {
                        e.Value = "Closed";
                    }
                    else if (purchaseOrder.isApproved == true && purchaseOrder.PurchaseOrderStatus.isActive == false && purchaseOrder.PendingForClosing != true)
                    {
                        e.Value = "Closed";
                    }
                    else if (purchaseOrder.isApproved == true && purchaseOrder.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                    else if (purchaseOrder.isApproved == true)
                    {
                        e.Value = "Approved";
                    }
                    else if (purchaseOrder.isApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (purchaseOrder.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                }
            }
        }
        private void grdVendorcompanies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void btnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdVendorPurchaseOrders);
        }
    }
}
