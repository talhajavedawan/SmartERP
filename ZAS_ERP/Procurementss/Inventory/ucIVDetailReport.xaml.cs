using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
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
using ZAS_ERP.Procurementss.Inventory.Adjustment.WIndows;

namespace ZAS_ERP.Procurementss.Inventory
{
    /// <summary>
    /// Interaction logic for ucIVDetailReport.xaml
    /// </summary>
    public partial class ucIVDetailReport : UserControl
    {
        public  int prodId = 0;
        public string reportLabel = "";
        public ucIVDetailReport()
        {
            InitializeComponent();
        }
        private void GrdItems_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "OnHand")
            {

                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                if (product.Inventories != null && product.Inventories.Count != 0)
                {
                    var purchaseQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.Quantity);
                    var saleQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.Quantity);

                    var adjustmentQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true).Sum(x => x.Quantity);
                    purchaseQuantity = purchaseQuantity + adjustmentQuantity;
                    
                    e.Value = purchaseQuantity + saleQuantity;
                   
                }
            }
            else
           if (e.Column.FieldName == "AmountOC")
            {
                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                var totalPurchaseAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.AmountOC);
                var totalSaleAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.AmountOC);
                var totalAdjustmentAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid!=true).Sum(x => x.AmountOC);
                totalPurchaseAmount = totalPurchaseAmount + totalAdjustmentAmount;
                e.Value = totalPurchaseAmount + totalSaleAmount;
            }
            else
           if (e.Column.FieldName == "AmountPKR")
            {
                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                var totalPAmountMER = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.AmountMER * x.MER);
                var totalSAmountMER = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.AmountMER * x.MER);
                var totalAdjustmentMER = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true).Sum(x => x.AmountMER * x.MER);
                totalPAmountMER = totalPAmountMER + totalAdjustmentMER;
                e.Value = totalPAmountMER + totalSAmountMER;
            }
            else
           if (e.Column.FieldName == "AvgCost")
            {
                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                var sumPurchase  = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.AmountOC);
                var sumAdjusted  = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true).Sum(x => x.AmountOC);
                var sumSales = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.AmountOC);
                var qSumPurchases = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice && x.PurchaseInvoice?.isVoid != true).Sum(x => x.Quantity);
                var qSumAdjustment= product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null && x.InventoryAdjustment?.isVoid != true).Sum(x => x.Quantity);

                
                sumPurchase = sumPurchase + sumAdjusted;
                qSumPurchases = qSumPurchases + qSumAdjustment;

                var qSumSales = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice && x.SaleInvoice?.isVoid != true).Sum(x => x.Quantity);





                var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                if (averageCost == 0 )
                {
                    e.Value = 0;
                }
                else
                    e.Value = averageCost;
            }
            if (e.Column.FieldName == "Departments")
            {
                var product = grdItems.GetRowByListIndex(e.ListSourceRowIndex) as Product;
                var res = String.Join(", ", product.departments.Select(x => x.DeptName));
                e.Value = res;
            }

        }

        private void GrdItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            var inventory = grdInventory.SelectedItem as ERP_BL.Procurements.Inventories.Inventory;
            if (inventory != null)
            {
                if (inventory.PurchaseInvoiceId != null)
                {

                    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Invoice, (int)inventory.PurchaseInvoiceId);
                    procurmentPanel.Show();

                }
                else
                if (inventory.SaleInviceId != null)
                {
                    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, (int)inventory.SaleInviceId);
                    procurmentPanel.Show();
                }
                else
                if (inventory.adjustment_Id != null)
                {
                    try
                    {
                        winfrmAdjustInventory adjustment = new winfrmAdjustInventory();
                        adjustment.OrderId = (int)inventory.adjustment_Id;
                        adjustment.editOrder = 1;
                        adjustment.Show();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProductRepo repo = new ProductRepo();
            var userCompIds = SYSTEM_STATIC.LoadCurrentUserCompanies().Select(x => x.Id).ToList();
            var userDeptIds = SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id).ToList();
            if (prodId != 0)
            {
                var products = repo.getActiveProductForInventory(userCompIds, prodId, userDeptIds);
                foreach (var product in products)
                {
                    product.Inventories = product.Inventories.Where(x => x.SaleInvoice.isVoid != true).ToList();
                    product.Inventories = product.Inventories.Where(x => x.PurchaseInvoice.isVoid != true).ToList();
                    product.Inventories = product.Inventories.Where(x => x.InventoryAdjustment.isVoid != true).ToList();
                }
                grdItems.ItemsSource = products;
            }
            else
            {
                var products = repo.getActiveProductsForInventory(userCompIds, userDeptIds);
                foreach (var product in products)
                {
           
                    product.Inventories = product.Inventories.Where(x => x.SaleInvoice?.isVoid != true).ToList();
                    product.Inventories = product.Inventories.Where(x => x.PurchaseInvoice?.isVoid != true).ToList();
                    product.Inventories = product.Inventories.Where(x => x.InventoryAdjustment?.isVoid != true).ToList();
                }
                grdItems.ItemsSource = products;
            }
  
        }
      
        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ProductRepo repo = new ProductRepo();
            var userCompIds = SYSTEM_STATIC.LoadCurrentUserCompanies().Select(x => x.Id).ToList();
            var userDeptIds = SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id).ToList();
            //lblHeading.Header = reportLabel;
            if (prodId != 0)
            {

                grdItems.ItemsSource = repo.getActiveProductForInventory(userCompIds, prodId, userDeptIds);
            }
            else
            {
                grdItems.ItemsSource = repo.getActiveProductsForInventory(userCompIds, userDeptIds);
            }
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(adornerCont.Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdItems.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = adornerCont.Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = adornerCont.Header.ToString();
            link.ReportHeaderData = adornerCont.Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);

        }
    }
}
