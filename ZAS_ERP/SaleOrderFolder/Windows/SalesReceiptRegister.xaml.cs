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
using ZAS_ERP.SaleOrderFolder.UserControls;

namespace ZAS_ERP.SaleOrderFolder.Windows
{
    /// <summary>
    /// Interaction logic for SalesReceiptRegister.xaml
    /// </summary>
    public partial class SalesReceiptRegister : Window
    {
        SalesReceiptRepo repo = new SalesReceiptRepo();
        List<cmbitem> TreeItems = new List<cmbitem>();
        ucSaleReceiptList uc = new ucSaleReceiptList();
        List<SalesReceipt> salesReceipts = new List<SalesReceipt>();

        public string transctions;
        public SalesReceiptRegister()
        {
            InitializeComponent();
            //try
            //{
            //    List<SalesReceipt> salesReceipts = new List<SalesReceipt>();
            //    salesReceipts = repo.GetAllSalesReceipt(MainWindow.currentUserid);
            //    faRightGrid.Children.Add(uc);
            //    GetAllSaleReceipts obj = new GetAllSaleReceipts(salesReceipts);
            //    //uc.Load_Receipts();
            //    uc.grdSaleReceiptList.ItemsSource = obj.SaleReceiptList;

            //    cmbitem treeItem1 = new cmbitem() { name = "Sale Receipts" };
            //    cmbitem treeItema = new cmbitem() { name = "Sale Receipts(Open)" };
            //    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
            //    foreach (SalesReceiptStatus status in repo.GetAllOpenSaleReceiptStatus().OrderBy(x => x.Status).ToList())
            //    {
            //        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Sale Receipts" });
            //    }
            //    treeItema.Items = cmbItemsa;
            //    treeItem1.Items.Add(treeItema);
            //    cmbitem treeItemb = new cmbitem() { name = "Sale Receipts(Closed)" };

            //    //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "View InActive Inquiries") != null))
            //    //{
            //    ICollection<cmbitem> cmbItems = new List<cmbitem>();
            //    foreach (SalesReceiptStatus status in repo.GetAllCloseSaleReceiptStatus().OrderBy(x => x.Status).ToList())
            //    {
            //        cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Sale Receipts" });
            //    }
            //    treeItemb.Items = cmbItems;
            //    treeItem1.Items.Add(treeItemb);

            //    //}


            //    TreeItems.Add(treeItem1);
            //    treeViewSaleReceiptStatus.ItemsSource = TreeItems;
            //}
            //catch
            //{

            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
             
                switch (transctions)
                {
                    case "PendingForClosing":

                        //salesReceipts = repo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                        // interBankTransfer = bankTransRepo.getAllPendingForClosingDepartmental(SystemLogic.currentUser.id);
                        uc.labelHeader = "(Pending for Closing) Sale Receipt";
                        //uc.grdSaleReceiptList.ItemsSource = salesReceipts;
                        break;

                    case "PendingForApproval":
                        //salesReceipts = repo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                        uc.labelHeader = "(Pending For Approval) Sale Receipt ";
                        //uc.grdSaleReceiptList.ItemsSource = salesReceipts;
                        break;

                    case "PendingForReapproval":
                        //salesReceipts = repo.getAllPendingForReApproval(MainWindow.currentUserid);
                        uc.labelHeader = "(Pending for ReApprovals) Sale Receipt";
                        //uc.grdSaleReceiptList.ItemsSource = salesReceipts;
                        break;

                    case "Open":
                        //salesReceipts = repo.getAllActiveandUnapprovedReceipts(MainWindow.currentUserid);
                        uc.labelHeader = "Sale Receipts(Open)";
                        //uc.grdSaleReceiptList.ItemsSource = salesReceipts;
                        break;

                    case "Close":
                        //salesReceipts = repo.getAllInActiveandUnapprovedReceipts(MainWindow.currentUserid);
                        uc.labelHeader = "Sale Receipts(Close)";
                        //uc.grdSaleReceiptList.ItemsSource = salesReceipts;
                        break;
                    default:
                        //salesReceipts = repo.GetAllSalesReceipt(MainWindow.currentUserid);
                        uc.labelHeader = "Sale Receipts Register";
                        uc.AllActive = 8;
                        uc.statusId = 0;
                        //uc.grdSaleReceiptList.ItemsSource = salesReceipts;
                        break;
                }

                //List<SalesReceipt> salesReceipts = new List<SalesReceipt>();
                //salesReceipts = repo.GetAllSalesReceipt(MainWindow.currentUserid);
                faRightGrid.Children.Add(uc);
                //GetAllSaleReceipts obj = new GetAllSaleReceipts(salesReceipts);
                ////  uc.Load_Receipts();
                //uc.grdSaleReceiptList.ItemsSource = obj.SaleReceiptList;

                cmbitem treeItem1 = new cmbitem() { name = "Sale Receipts" };
                cmbitem treeItema = new cmbitem() { name = "Sale Receipts(Open)" };
                ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                foreach (SalesReceiptStatus status in repo.GetAllOpenSaleReceiptStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Sale Receipts" });
                }
                treeItema.Items = cmbItemsa;
                treeItem1.Items.Add(treeItema);
                cmbitem treeItemb = new cmbitem() { name = "Sale Receipts(Closed)" };

                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "View InActive Inquiries") != null))
                //{
                ICollection<cmbitem> cmbItems = new List<cmbitem>();
                foreach (SalesReceiptStatus status in repo.GetAllCloseSaleReceiptStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Sale Receipts" });
                }
                treeItemb.Items = cmbItems;
                treeItem1.Items.Add(treeItemb);

                //}


                TreeItems.Add(treeItem1);
                treeViewSaleReceiptStatus.ItemsSource = TreeItems;
            }
            catch(Exception ex)
            {

            }
        }

        private void TreeViewSaleReceiptStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            salesReceipts = new List<SalesReceipt>();
            repo = new SalesReceiptRepo();
            uc = new ucSaleReceiptList();
            var item = (cmbitem)treeViewSaleReceiptStatus.SelectedItem;

            if (item.name == "Sale Receipts")
            {
                //salesReceipts = repo.GetAllSalesReceiptOpenAndClosedFirst(MainWindow.currentUserid);
                uc.statusId = 0;
                uc.AllActive = 1;
            }
            else if (item.name == "Sale Receipts(Open)")
            {
                //salesReceipts = repo.getAllActiveandUnapprovedReceiptsFirst(MainWindow.currentUserid);
                uc.statusId = 0;
                uc.AllActive = 2;
            }
            else if (item.name == "Sale Receipts(Closed)")
            {
                //salesReceipts = repo.getAllInActiveandUnapprovedReceiptsFirst(MainWindow.currentUserid);
                uc.statusId = 0;
                uc.AllActive = 3;
            }
            else
            {
                //salesReceipts = repo.getAllSaleReceiptsbyStatusIdFirst(MainWindow.currentUserid, item.id);
                uc.statusId = 0;
                uc.AllActive = 4;
                
                uc.treeStatusId = item.id;
            }
            faRightGrid.Children.Clear();
            faRightGrid.Children.Add(uc);
            uc.labelHeader = item.name; 

        }

        private void AddReceiptBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                ucFrmSaleReceipt enterSaleReceiptObj = new ucFrmSaleReceipt();
                enterSaleReceiptObj.enter_receipt_win.Content = enterSaleReceiptObj;
                enterSaleReceiptObj.enter_receipt_win.Title = "Sale Receipt";
                enterSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
                enterSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                enterSaleReceiptObj.enter_receipt_win.ShowDialog();
                uc.Load_Receipts();
            }
            catch
            {

            }
            
        }

        private void GrdExpand_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            faLeftGrid.Visibility = Visibility.Visible;
            GridSplitter.Visibility = Visibility.Visible;

            faRightGrid.SetValue(Grid.ColumnProperty, 2);
            faRightGrid.SetValue(Grid.ColumnSpanProperty, 1);
        }

        private void GrdCollapse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Collapsed;
            GridSplitter.Visibility = Visibility.Collapsed;
            faRightGrid.SetValue(Grid.ColumnProperty, 0);

            faRightGrid.SetValue(Grid.ColumnSpanProperty, 3);
        }
    }
}
