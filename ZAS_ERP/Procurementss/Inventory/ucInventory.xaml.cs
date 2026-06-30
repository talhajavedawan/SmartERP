using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.Inventories;
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

namespace ZAS_ERP.Procurementss.Inventory
{
    /// <summary>
    /// Interaction logic for ucInventory.xaml
    /// </summary>
    public partial class ucInventory : UserControl
    {
        ProductRepo repo = new ProductRepo();
        List<Product> products = new List<Product>();
        public ucInventory()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadItemgrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void loadItemgrid()
        {
            if (MainWindow.currentUserid == 0)
                products = repo.getAll();
            else
            {
                var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();
                var userDepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
                products = repo.getActiveInventoryProducts(userCompanies.Select(x => x.Id).ToList(), userDepartments.Select(x => x.Id).ToList());
            }
            this.grdItems.ItemsSource = products;
        }

        private void view_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "OnHand")
            {
                var product = e.Node.Content as Product;
                if (product.Inventories != null && product.Inventories.Count != 0)
                {
                    var purchaseQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice).Sum(x => x.Quantity);
                    var saleQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice).Sum(x => x.Quantity);
                    var adjustmentQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id!=null).Sum(x => x.Quantity);
                    purchaseQuantity = purchaseQuantity + adjustmentQuantity;

                    e.Value = purchaseQuantity + saleQuantity;
                }
            }
            else
            if(e.Column.FieldName == "AmountOC")
            {
                var product = e.Node.Content as Product;
                var totalPurchaseAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice).Sum(x => x.AmountOC);
                var totalSaleAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice).Sum(x => x.AmountOC);
                var totalAdjustmentAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id!=null).Sum(x => x.AmountOC);
                totalPurchaseAmount = totalPurchaseAmount - totalAdjustmentAmount;

                e.Value = totalPurchaseAmount+totalSaleAmount;
            }
            else
            if (e.Column.FieldName == "AmountMER")
            {
                var product = e.Node.Content as Product;
                var totalPAmountMER = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice).Sum(x => x.AmountMER * x.MER);
                var totalSAmountMER = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice).Sum(x => x.AmountMER * x.MER);
                var totalAdjustmentMER = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null).Sum(x => x.AmountMER * x.MER);
                totalPAmountMER = totalPAmountMER + totalAdjustmentMER;

                e.Value = totalPAmountMER+ totalSAmountMER;
            }
            else
           if (e.Column.FieldName == "AvgCost")
            {
                var product = e.Node.Content as Product;
                var sumPurchase = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice).Sum(x => x.AmountOC);
                var sumAdjustmentsAmount = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment &&  x.adjustment_Id != null).Sum(x => x.AmountOC);
                sumPurchase = sumPurchase + sumAdjustmentsAmount;


                var sumSales = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice).Sum(x => x.AmountOC);
                var qSumPurchases = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.PurchaseInvoice).Sum(x => x.Quantity);
                var sumAdjustmentQuantity = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.Adjustment && x.adjustment_Id != null).Sum(x => x.Quantity);
                qSumPurchases = qSumPurchases + sumAdjustmentQuantity;

                var qSumSales = product.Inventories.Where(x => x.TransactionsType == InventoryTransactionsType.SaleInvoice).Sum(x => x.Quantity);
                var remainingAmountInven = sumPurchase - Math.Abs(sumSales);
                var remaiInvenQuantity = qSumPurchases - Math.Abs(qSumSales);
                var averageCost = Math.Round(remainingAmountInven / remaiInvenQuantity, 2);
                if (averageCost == 0)
                {
                    e.Value = 0;
                }
                else
                    e.Value = averageCost;
            }
        }

        private void GrdItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var product = (grdItems.SelectedItem as Product);
            //if(product != null)
            //{
            //    //ucIVDetailReport report = new ucIVDetailReport();
            //    //report.prodId = product.Id;
            //    //report.reportLabel = product.item+""+" Inventory Valuation Detail";

            //    //report.Show();

            //}
            
        }
        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            repo = new ProductRepo();
            loadItemgrid();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(adornerCont.Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TreeListView)grdItems.View);
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
