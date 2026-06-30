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
using ZAS_ERP.Bankings.InterCompanyBankTransfer;
using ZAS_ERP.Bankings.UserControls;

namespace ZAS_ERP.Bankings.InterCompanyWindows
{
    /// <summary>
    /// Interaction logic for interCompanyBanktransfer.xaml
    /// </summary>
    public partial class interCompanyBanktransfer : Window       
    {
        List<cmbitem> treeItems = new List<cmbitem>();

        InterBankTransRepo statusRepo = new InterBankTransRepo();

        InterCompanyBankTransferRepo transferRepo = new InterCompanyBankTransferRepo();        
        ucInterCompBankTransRegister interCompanyBankTransfer = new ucInterCompBankTransRegister();
        List<ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer> interCompanyBanks = new List<ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer>();
        public string transctions;


        public interCompanyBanktransfer()
        {
            InitializeComponent();

           
            //start

            //try
            //{
            //    interCompanyBankTransfer = new ucInterCompBankTransRegister();
            //    interCompanyBanks = transferRepo.getAllActiveandTransactions(SystemLogic.currentUser.id);

            //    faRightGrid.Children.Add(interCompanyBankTransfer);

            //    interCompanyBankTransfer.grdCntrlInterCompanyBankTransfers.ItemsSource = interCompanyBanks;

            //    cmbitem treeItem1 = new cmbitem() { name = "Inter-Bank Transfer" };
            //    cmbitem treeItema = new cmbitem() { name = "Inter-Bank Transfer(Open)" };
            //    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
            //    foreach (InterBankTransferStatus status in statusRepo.GetAllOpenBankTransferStatus().OrderBy(x => x.Status).ToList())
            //    {
            //        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Inter-Bank Transfers" });
            //    }
            //    treeItema.Items = cmbItemsa;
            //    treeItem1.Items.Add(treeItema);
            //    cmbitem treeItemb = new cmbitem() { name = "Inter-Bank Transfers(Closed)" };

            //    ICollection<cmbitem> cmbItems = new List<cmbitem>();
            //    foreach (InterBankTransferStatus status in statusRepo.GetAllCloseBankTransferStatus().OrderBy(x => x.Status).ToList())
            //    {
            //        cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Inter-Bank Transfers" });
            //    }
            //    treeItemb.Items = cmbItems;
            //    treeItem1.Items.Add(treeItemb);

            //    //}


            //    treeItems.Add(treeItem1);
            //    treeViewInterCompanyBankTransStatus.ItemsSource = treeItems;
            //}
            //catch
            //{

            //}


            //end



        }

        private void AddInterCompanyBankTransBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {


            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer") != null)
            {
               // ucFrmBankTransfers ucFrmBankTransfer = new ucFrmBankTransfers();
                ucBankTransferInterCompany ucFrmBankTransfer = new ucBankTransferInterCompany();


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

        private void TreeViewInterCompanyBankTransStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //start
            // interCompanyBanks = new List<ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer>();
            interCompanyBanks = new List<ERP_BL.Procurements.InterBankTransfers.InterCompanyBankTransfer>();
            statusRepo = new InterBankTransRepo();
            var item = (cmbitem)treeViewInterCompanyBankTransStatus.SelectedItem;

            if (item.name == "Inter-Bank Transfer")
            {
                interCompanyBanks = transferRepo.getAllInterCompanyBankTransfer(SYSTEM_STATIC.currentUser.id);
            }
            else if (item.name == "Inter-Bank Transfer(Open)")
            {
                interCompanyBanks = transferRepo.getAllActiveandTransactions(SYSTEM_STATIC.currentUser.id);
            }
            else if (item.name == "Inter-Bank Transfers(Closed)")
            {
                interCompanyBanks = transferRepo.getAllInActiveTransactions(SYSTEM_STATIC.currentUser.id);
            }
            else
            {
                interCompanyBanks = transferRepo.getAllTransactionsbyStatusId(SYSTEM_STATIC.currentUser.id, item.id);
            }


            interCompanyBankTransfer.grdCntrlInterCompanyBankTransfers.ItemsSource = interCompanyBanks;
            interCompanyBankTransfer.SetColumnsVisibility();
            interCompanyBankTransfer.lblHeading.Text = item.name;


            //end


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                interCompanyBankTransfer = new ucInterCompBankTransRegister();





                switch (transctions)
                {
                    case "PendingForClosing":
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Inter-Bank Transfer List") != null)
                        {

                            interCompanyBanks = transferRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                            interCompanyBankTransfer.lblHeading.Text = "(Pending for Closing) Inter-Bank Tranfers";

                            interCompanyBankTransfer.grdCntrlInterCompanyBankTransfers.ItemsSource = interCompanyBanks;
                        }
                        else
                        {
                            MessageBox.Show("Permission required to View Pending for closing Inter-Bank Transfer!");
                        }
                        break;
                    case "PendingForApproval":
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Inter-Bank Transfer List") != null)
                        {

                            interCompanyBanks = transferRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                            interCompanyBankTransfer.lblHeading.Text = "(Pending For Approval) Inter-Bank Transfers";
                            interCompanyBankTransfer.grdCntrlInterCompanyBankTransfers.ItemsSource = interCompanyBanks;
                        }
                        else
                        {
                            MessageBox.Show("Permission Required to view Pending for Approval Sale Receipts!!");
                        }
                        break;
                    case "PendingForReapproval":
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                        {
                            interCompanyBanks = transferRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                            interCompanyBankTransfer.lblHeading.Text = "Pending for ReApprovals Inter-Bank Transfers";
                            interCompanyBankTransfer.grdCntrlInterCompanyBankTransfers.ItemsSource = interCompanyBanks;
                        }
                        else
                        {
                            MessageBox.Show("Permission Required to View Pending For Re-Approval Inter-Bank Transfers!!");
                        }
                        break;
                    case "Open":
                        interCompanyBanks = transferRepo.getAllActiveandTransactions(SYSTEM_STATIC.currentUser.id);
                        interCompanyBankTransfer.lblHeading.Text = "InterCompany-Bank Transfers(Open)";
                        interCompanyBankTransfer.grdCntrlInterCompanyBankTransfers.ItemsSource = interCompanyBanks;
                        break;
                    case "Close":
                        interCompanyBanks = transferRepo.getAllInActiveTransactions(SYSTEM_STATIC.currentUser.id);
                        interCompanyBankTransfer.lblHeading.Text = "InterCompany-Bank Transfers(Close)";
                        interCompanyBankTransfer.grdCntrlInterCompanyBankTransfers.ItemsSource = interCompanyBanks;
                        break;
                    default:
                        interCompanyBanks = transferRepo.getAllActiveandTransactions(SYSTEM_STATIC.currentUser.id);
                        interCompanyBankTransfer.lblHeading.Text = "InterCompany-Bank Transfers(Open)";
                        interCompanyBankTransfer.grdCntrlInterCompanyBankTransfers.ItemsSource = interCompanyBanks;
                        break;
                }
                faRightGrid.Children.Add(interCompanyBankTransfer);

                cmbitem treeItem1 = new cmbitem() { name = "Inter-Bank Transfer" };
                cmbitem treeItema = new cmbitem() { name = "Inter-Bank Transfer(Open)" };
                ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                foreach (InterBankTransferStatus status in statusRepo.GetAllOpenBankTransferStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Inter-Bank Transfers" });
                }
                treeItema.Items = cmbItemsa;
                treeItem1.Items.Add(treeItema);
                cmbitem treeItemb = new cmbitem() { name = "Inter-Bank Transfers(Closed)" };

                ICollection<cmbitem> cmbItems = new List<cmbitem>();
                foreach (InterBankTransferStatus status in statusRepo.GetAllCloseBankTransferStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Inter-Bank Transfers" });
                }
                treeItemb.Items = cmbItems;
                treeItem1.Items.Add(treeItemb);

                //}


                treeItems.Add(treeItem1);
                treeViewInterCompanyBankTransStatus.ItemsSource = treeItems;
            }
            catch(Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
