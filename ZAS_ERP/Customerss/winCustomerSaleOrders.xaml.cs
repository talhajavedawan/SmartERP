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

namespace ZAS_ERP.Customerss
{
    /// <summary>
    /// Interaction logic for winCustomerSaleOrders.xaml
    /// </summary>
    public partial class winCustomerSaleOrders : DXWindow
    {
        public winCustomerSaleOrders()
        {
            InitializeComponent();
        }
        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loadgrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCustomerSaleOrders);
        }
        private void loadgrid()
        {
            CustomerCompRepo compRepo = new CustomerCompRepo();
            EmployeeRepo employeeRepo = new EmployeeRepo();
            List<CustomerCompany> customerCompanies = new List<CustomerCompany>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Customers") != null)
            {

                customerCompanies = employeeRepo.GetAllCustomersByUserId(MainWindow.currentUserid);
                grdcutomercompanies.ItemsSource = customerCompanies;
            }
            else
            {
                customerCompanies = employeeRepo.GetActiveCustomersByUserId(MainWindow.currentUserid);
                grdcutomercompanies.ItemsSource = customerCompanies;
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCustomerSaleOrders);
        }
        private void grdcutomercompanies_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            var customer = grdcutomercompanies.SelectedItem as CustomerCompany;
            if (customer != null)
                grdCustomerSaleOrders.ItemsSource = saleOrderRepo.getAllByCustomer(SYSTEM_STATIC.currentUser.id, customer.Id);
        }
        private void GrdsaleOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            EditSaleOrder();
        }

        private void EditSaleOrder()
        {
            var selectedItem = grdCustomerSaleOrders.SelectedItem as SaleOrder;
            if (selectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, selectedItem.Id);
                procurmentPanel.Show();
            }
        }
        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Stage" && e.IsGetData)
            {
                var saleOrder = grdCustomerSaleOrders.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                if (saleOrder.isVoid == true)
                {
                    e.Value = "Void";
                }
                else if (saleOrder.isReApproved == false)
                {
                    e.Value = "Under Approval";
                }
                else if (saleOrder.isApproved == true && saleOrder.stage == "Closed")
                {
                    e.Value = "Closed";
                }
                else if (saleOrder.isApproved == true && saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing != true)
                {
                    e.Value = "Closed";
                }
                else if (saleOrder.isApproved == true && saleOrder.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
                else if (saleOrder.isApproved == true)
                {
                    e.Value = "Approved";
                }
                else if (saleOrder.isApproved == false)
                {
                    e.Value = "Under Approval";
                }
                else if (saleOrder.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
            }
        }

        private void btnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCustomerSaleOrders);
        }
    }
}
