using ERP_BL.Databases;
using ERP_BL.Procurements;
using ERP_BL.Procurements.InterBankTransfers;
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
using ZAS_ERP.Bankings.UserControls;

namespace ZAS_ERP.Bankings.Windows
{
    /// <summary>
    /// Interaction logic for InterBankTransferRegister.xaml
    /// </summary>
    public partial class InterBankTransferRegister : Window
    {

        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
        List<cmbitem> TreeItems = new List<cmbitem>();
        ucBankTransferRegister bankTransferRegister = new ucBankTransferRegister();
        List<ERP_BL.Procurements.InterBankTransfers.InterBankTransfer> interBankTransferList = new List<ERP_BL.Procurements.InterBankTransfers.InterBankTransfer>();
        public string transctions;
        public InterBankTransferRegister()
        {
            InitializeComponent();

            //try
            //{
            //    interBankTransferList = new List<InterBankTransfer>();
            //    interBankTransferList = bankTransRepo.GetAllInterBankTransfers(SystemLogic.currentUser.id);
            //    faRightGrid.Children.Add(bankTransferRegister);
                
            //    bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;

            //    cmbitem treeItem1 = new cmbitem() { name = "Inter-Bank Transfer" };
            //    cmbitem treeItema = new cmbitem() { name = "Inter-Bank Transfer(Open)" };
            //    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
            //    foreach (InterBankTransferStatus status in bankTransRepo.GetAllOpenBankTransferStatus().OrderBy(x => x.Status).ToList())
            //    {
            //        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Inter-Bank Transfers" });
            //    }
            //    treeItema.Items = cmbItemsa;
            //    treeItem1.Items.Add(treeItema);
            //    cmbitem treeItemb = new cmbitem() { name = "Inter-Bank Transfers(Closed)" };

            //    ICollection<cmbitem> cmbItems = new List<cmbitem>();
            //    foreach (InterBankTransferStatus status in bankTransRepo.GetAllCloseBankTransferStatus().OrderBy(x => x.Status).ToList())
            //    {
            //        cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Inter-Bank Transfers" });
            //    }
            //    treeItemb.Items = cmbItems;
            //    treeItem1.Items.Add(treeItemb);

            //    //}


            //    TreeItems.Add(treeItem1);
            //    treeViewInterBankTransStatus.ItemsSource = TreeItems;
            //}
            //catch
            //{

            //}
        }

        private void AddInterBankTransBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
                ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();

                ucFrmBankTransfer.editFlag = false;
                ucFrmBankTransfer.frmBankTranfer.Content = ucFrmBankTransfer;
                ucFrmBankTransfer.frmBankTranfer.WindowState = WindowState.Maximized;
                ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                ucFrmBankTransfer.frmBankTranfer.ShowDialog();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Inter-Bank Transfer!");
                return;
            }
        }

        private void GrdCollapse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Collapsed;
            GridSplitter.Visibility = Visibility.Collapsed;
            faRightGrid.SetValue(Grid.ColumnProperty, 0);

            faRightGrid.SetValue(Grid.ColumnSpanProperty, 3);
        }

        private void GrdExpand_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Visible;
            GridSplitter.Visibility = Visibility.Visible;

            faRightGrid.SetValue(Grid.ColumnProperty, 2);
            faRightGrid.SetValue(Grid.ColumnSpanProperty, 1);
        }

        private void TreeViewInterBankTransStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            interBankTransferList = new List<ERP_BL.Procurements.InterBankTransfers.InterBankTransfer>();
            bankTransRepo = new InterBankTransRepo();
            var item = (cmbitem)treeViewInterBankTransStatus.SelectedItem;

            if (item.name == "Inter-Bank Transfer")
            {
                interBankTransferList = bankTransRepo.GetAllTransactionsOpenAndClosed(MainWindow.currentUserid);
            }
            else if (item.name == "Inter-Bank Transfer(Open)")
            {
                interBankTransferList = bankTransRepo.getAllActiveandUnapprovedTransactions(MainWindow.currentUserid);
            }
            else if (item.name == "Inter-Bank Transfers(Closed)")
            {
                interBankTransferList = bankTransRepo.getAllInActiveandUnapprovedReceipts(MainWindow.currentUserid);
            }
            else
            {
                interBankTransferList = bankTransRepo.getAllSaleReceiptsbyStatusId(MainWindow.currentUserid, item.id);
            }


            bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;
            bankTransferRegister.SetColumnsVisibility();
            bankTransferRegister.lblHeading.Text = item.name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bankTransferRegister = new ucBankTransferRegister();

                switch (transctions)
                {
                    case "PendingForClosing":

                        interBankTransferList = bankTransRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                        // interBankTransfer = bankTransRepo.getAllPendingForClosingDepartmental(SystemLogic.currentUser.id);
                        bankTransferRegister.lblHeading.Text = "(Pending for Closing) Inter-Bank Tranfers";

                        bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;


                        break;
                    case "PendingForApproval":
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
                        {

                            interBankTransferList = bankTransRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                            bankTransferRegister.lblHeading.Text = "(Pending For Approval) Inter-Bank Transfers ";
                            bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;
                        }
                        else
                        {
                            MessageBox.Show("Permission Required to view Pending for Approval Sale Receipts!!");
                        }
                        break;
                    case "PendingForReapproval":
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                        {
                            interBankTransferList = bankTransRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                            bankTransferRegister.lblHeading.Text = "Pending for ReApprovals Inter-Bank Transfers";
                            bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;
                        }
                        else
                        {
                            MessageBox.Show("Permission Required to View Pending For Re-Approval Inter-Bank Transfers!!");
                        }
                        break;
                    case "Open":
                        interBankTransferList = bankTransRepo.getAllActiveandUnapprovedTransactions(MainWindow.currentUserid);
                        bankTransferRegister.lblHeading.Text = "Inter-Bank Transfers(Open)";
                        bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;
                        break;
                    case "Close":
                        interBankTransferList = bankTransRepo.getAllInActiveandUnapprovedReceipts(MainWindow.currentUserid);
                        bankTransferRegister.lblHeading.Text = "Inter-Bank Transfers(Close)";
                        bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;
                        break;
                    default:
                        interBankTransferList = bankTransRepo.GetAllInterBankTransfers(MainWindow.currentUserid);
                        bankTransferRegister.lblHeading.Text = "Inter-Bank Transfers(Open)";
                        bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;
                        break;
                }
                // interBankTransferList = new List<InterBankTransfer>();
                // interBankTransferList = bankTransRepo.GetAllInterBankTransfers(SystemLogic.currentUser.id);
                faRightGrid.Children.Add(bankTransferRegister);

                //  bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;

                cmbitem treeItem1 = new cmbitem() { name = "Inter-Bank Transfer" };
                cmbitem treeItema = new cmbitem() { name = "Inter-Bank Transfer(Open)" };
                ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                foreach (InterBankTransferStatus status in bankTransRepo.GetAllOpenBankTransferStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Inter-Bank Transfers" });
                }
                treeItema.Items = cmbItemsa;
                treeItem1.Items.Add(treeItema);
                cmbitem treeItemb = new cmbitem() { name = "Inter-Bank Transfers(Closed)" };

                ICollection<cmbitem> cmbItems = new List<cmbitem>();
                foreach (InterBankTransferStatus status in bankTransRepo.GetAllCloseBankTransferStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Inter-Bank Transfers" });
                }
                treeItemb.Items = cmbItems;
                treeItem1.Items.Add(treeItemb);

                //}


                TreeItems.Add(treeItem1);
                treeViewInterBankTransStatus.ItemsSource = TreeItems;
            }
            catch
            {

            }
        }
    }
}
