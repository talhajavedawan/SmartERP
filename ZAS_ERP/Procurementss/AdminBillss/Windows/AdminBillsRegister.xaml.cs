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
using System.Windows.Shapes;
using ZAS_ERP.Procurementss.AdminBillss.UserControls;

namespace ZAS_ERP.Procurementss.AdminBillss.Windows
{
    /// <summary>
    /// Interaction logic for AdminBillsRegister.xaml
    /// </summary>
    public partial class AdminBillsRegister : Window
    {
        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();

        List<AdminBill> adminBillList = new List<AdminBill>();
        List<cmbitem> TreeItems = new List<cmbitem>();
        AdminBillsRepo billsRepo = new AdminBillsRepo();

        ucBillList billRegister = new ucBillList();
        public string transctions;

        public AdminBillsRegister()
        {
            InitializeComponent();

            //try
            //{
                
            //    adminBillList = new List<AdminBill>();
            //    adminBillList = billsRepo.GetAllOpenBills(SystemLogic.currentUser.id);
            //    faRightGrid.Children.Add(billRegister);

            //    if (SystemLogic.AllowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
            //    {
            //        billRegister.grdBillRegister.ItemsSource = adminBillList;
            //    }                

            //    cmbitem treeItem1 = new cmbitem() { name = "Admin Bills" };
            //    cmbitem treeItema = new cmbitem() { name = "Admin Bills(Open)" };
            //    ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
            //    foreach (AdminBillStatus status in billsRepo.GetAllBillStatuses().Where(x=>x.isActive ==true).ToList())
            //    {
            //        cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Admin Bills" });
            //    }
            //    treeItema.Items = cmbItemsa;
            //    treeItem1.Items.Add(treeItema);
            //    cmbitem treeItemb = new cmbitem() { name = "Admin Bills(Closed)" };

            //    ICollection<cmbitem> cmbItems = new List<cmbitem>();
            //    foreach (AdminBillStatus status in billsRepo.GetAllBillStatuses().Where(x => x.isActive == false).ToList())
            //    {
            //        cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Admin Bills" });
            //    }
            //    treeItemb.Items = cmbItems;
            //    treeItem1.Items.Add(treeItemb);

            //    //}


            //    TreeItems.Add(treeItem1);
            //    treeViewAdminBillStatus.ItemsSource = TreeItems;
            //}
            //catch
            //{

            //}
        }

        private void AddAdminBillBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill") != null)
            {
                frmBillAdd = new ucFrmBillAdd();
                DXWindow frmBill = new DXWindow();
                frmBill.WindowState = WindowState.Maximized;
                frmBill.Title = "Enter Bills";
                frmBillAdd.editFlag = false;
                frmBill.Content = frmBillAdd;
                frmBill.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Bill!");
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

        private void TreeViewAdminBillStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            adminBillList = new List<AdminBill>();
            billsRepo = new AdminBillsRepo();
            var item = (cmbitem)treeViewAdminBillStatus.SelectedItem;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
            {
                if (item.name == "Admin Bills")
                {
                    adminBillList = billsRepo.GetAllOpenAndClosed(MainWindow.currentUserid);
                }
                else if (item.name == "Admin Bills(Open)")
                {
                    adminBillList = billsRepo.getAllActiveandUnapprovedTransactions(MainWindow.currentUserid);
                }
                else if (item.name == "Admin Bills(Closed)")
                {
                    adminBillList = billsRepo.getAllInActiveandUnapprovedReceipts(MainWindow.currentUserid);
                }
                else
                {
                    adminBillList = billsRepo.getAllSaleReceiptsbyStatusId(MainWindow.currentUserid, item.id);
                }


                billRegister.grdBillRegister.ItemsSource = adminBillList;
                //billRegister.GridControlSetUserSettings();
                //bankTransferRegister.SetColumnsVisibility();
                billRegister.lblHeading.Text = item.name;
            }
            else
            {
                billRegister.grdBillRegister.ItemsSource = null;
                billRegister.lblHeading.Text = item.name;
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                billRegister = new ucBillList();
                switch (transctions)
                {
                    case "PendingForApproval":

                        adminBillList = billsRepo.GetAllPendingForApprovalBills(MainWindow.currentUserid);
                        // interBankTransfer = bankTransRepo.getAllPendingForClosingDepartmental(SystemLogic.currentUser.id);
                        billRegister.lblHeading.Text = "(Pending for Approval) Admin Bills";

                        billRegister.grdBillRegister.ItemsSource = adminBillList;


                        break;

                    case "PendingForClosing":
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Closing) Inter-Bank Transfer List") != null)
                        {

                            adminBillList = billsRepo.GetAllPendingForClosingBills();
                            // interBankTransfer = bankTransRepo.getAllPendingForClosingDepartmental(SystemLogic.currentUser.id);
                            billRegister.lblHeading.Text = "(Pending for Approval) Admin Bills";

                            billRegister.grdBillRegister.ItemsSource = adminBillList;
                        }
                        else
                        {
                            MessageBox.Show("Permission Required to View Admin bills!!");
                        }
                        break;
                    case "PendingForReapproval":
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Inter-Bank Transfer List") != null)
                        {
                            adminBillList = billsRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);
                            // interBankTransfer = bankTransRepo.getAllPendingForClosingDepartmental(SystemLogic.currentUser.id);
                            billRegister.lblHeading.Text = "(Pending for Re-Approval) Admin Bills";

                            billRegister.grdBillRegister.ItemsSource = adminBillList;
                        }
                        else
                        {
                            MessageBox.Show("Permission Required to View Admin bills!!");
                        }
                        break;
                    case "Open":
                        adminBillList = billsRepo.GetAllOpenBills(SYSTEM_STATIC.currentUser.id);
                        billRegister.lblHeading.Text = "Admin Bills(Open)";
                        billRegister.grdBillRegister.ItemsSource = adminBillList;
                        break;
                    case "Close":
                        adminBillList = billsRepo.getAllInActiveandUnapprovedReceipts(MainWindow.currentUserid);
                        billRegister.lblHeading.Text = "Admin Bills (Close)";
                        billRegister.grdBillRegister.ItemsSource = adminBillList;
                        break;
                    default:
                        //adminBillList = billsRepo.GetAllOpenBills(SYSTEM_STATIC.currentUser.id);
                        //billRegister.lblHeading.Text = "Admin Bills (Open)";
                        //billRegister.grdBillRegister.ItemsSource = adminBillList;
                        break;
                }
                //adminBillList = new List<AdminBill>();
                //adminBillList = billsRepo.GetAllOpenBills(SystemLogic.currentUser.id);
                faRightGrid.Children.Add(billRegister);

                //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
                //{
                //    billRegister.grdBillRegister.ItemsSource = adminBillList;
                //}

                cmbitem treeItem1 = new cmbitem() { name = "Admin Bills" };
                cmbitem treeItema = new cmbitem() { name = "Admin Bills(Open)" };
                var statusList = billsRepo.GetAllBillStatuses();
                ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                foreach (AdminBillStatus status in statusList.Where(x => x.isActive == true).ToList())
                {
                    cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Admin Bills" });
                }
                treeItema.Items = cmbItemsa;
                treeItem1.Items.Add(treeItema);
                cmbitem treeItemb = new cmbitem() { name = "Admin Bills(Closed)" };

                ICollection<cmbitem> cmbItems = new List<cmbitem>();
                foreach (AdminBillStatus status in statusList.Where(x => x.isActive == false).ToList())
                {
                    cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Admin Bills" });
                }
                treeItemb.Items = cmbItems;
                treeItem1.Items.Add(treeItemb);

                //}


                TreeItems.Add(treeItem1);
                treeViewAdminBillStatus.ItemsSource = TreeItems;
            }
            catch
            {

            }
        }
    }
}
