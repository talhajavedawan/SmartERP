using DevExpress.Xpf.Core;
using ERP_BL.Bankings;
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
using ZAS_ERP.Bankings.Loans.UserControls;

namespace ZAS_ERP.Bankings.Loans.Windows
{
    /// <summary>
    /// Interaction logic for LoansRegister.xaml
    /// </summary>
    public partial class LoansRegister : DevExpress.Xpf.Core.ThemedWindow
    {
        LoansRepo loansRepo = new LoansRepo();
        List<cmbitem> TreeItems = new List<cmbitem>();
        ucLoansList loansRegister = new ucLoansList();
        List<ERP_BL.Bankings.Loans> loansList = new List<ERP_BL.Bankings.Loans>();
        public string transctions;
        public LoansRegister()
        {
            InitializeComponent();
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


        private void AddLoansBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans") != null)
            {
                ucFrmLoans ucFrmLoans = new ucFrmLoans();
                DXWindow win = new DXWindow();

                ucFrmLoans.editFlag = false;
                win.Content = ucFrmLoans;
                win.WindowState = WindowState.Maximized;
                win.MinHeight = 800;
                win.MinWidth = 800;
                win.Show();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required for new Loans!");
                return;
            }
        }

        private void TreeViewLoansStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            loansList = new List<ERP_BL.Bankings.Loans>();
            loansRepo = new LoansRepo();
            var item = (cmbitem)treeViewLoansStatus.SelectedItem;

            if (item.name == "Loans")
            {
                loansList = loansRepo.GetAllLoansOpenAndClosed(MainWindow.currentUserid);
            }
            else if (item.name == "Loans(Open)")
            {
                loansList = loansRepo.getAllActiveandUnapprovedLoans(MainWindow.currentUserid);
            }
            else if (item.name == "Loans(Closed)")
            {
                loansList = loansRepo.getAllInActiveandUnapprovedLoans(MainWindow.currentUserid);
            }
            else
            {
                loansList = loansRepo.getAllLoansbyStatusId(MainWindow.currentUserid, item.id);
            }


            loansRegister.grdCntrlLoans.ItemsSource = loansList;
            //bankTransferRegister.SetColumnsVisibility();
            loansRegister.lblHeading.Text = item.name;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                loansRegister = new ucLoansList();

                switch (transctions)
                {
                    case "PendingForClosing":

                        loansList = loansRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                        // interBankTransfer = bankTransRepo.getAllPendingForClosingDepartmental(SystemLogic.currentUser.id);
                        loansRegister.lblHeading.Text = "(Pending for Closing) Loans";

                        loansRegister.grdCntrlLoans.ItemsSource = loansList;


                        break;
                    case "PendingForApproval":
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Loans List") != null)
                        {

                            loansList = loansRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                            loansRegister.lblHeading.Text = "(Pending For Approval) Loans ";
                            loansRegister.grdCntrlLoans.ItemsSource = loansList;
                        }
                        else
                        {
                            MessageBox.Show("Permission Required to view Pending for Approval Loans!!");
                        }
                        break;
                    case "PendingForReapproval":
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Loans List") != null)
                        {
                            loansList = loansRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                            loansRegister.lblHeading.Text = "Pending for ReApprovals Loans";
                            loansRegister.grdCntrlLoans.ItemsSource = loansList;
                        }
                        else
                        {
                            MessageBox.Show("Permission Required to View Pending For Re-Approval Loans!");
                        }
                        break;
                    case "Open":
                        loansList = loansRepo.getAllActiveandUnapprovedTransactions(MainWindow.currentUserid);
                        loansRegister.lblHeading.Text = "Loans(Open)";
                        loansRegister.grdCntrlLoans.ItemsSource = loansList;
                        break;
                    case "Close":
                        loansList = loansRepo.getAllInActiveandUnapprovedLoans(MainWindow.currentUserid);
                        loansRegister.lblHeading.Text = "Loans(Close)";
                        loansRegister.grdCntrlLoans.ItemsSource = loansList;
                        break;
                    default:
                        loansList = loansRepo.GetAllLoans(MainWindow.currentUserid);
                        loansRegister.lblHeading.Text = "Loans(Open)";
                        loansRegister.grdCntrlLoans.ItemsSource = loansList;
                        break;
                }
                // interBankTransferList = new List<InterBankTransfer>();
                // interBankTransferList = bankTransRepo.GetAllInterBankTransfers(SystemLogic.currentUser.id);
                faRightGrid.Children.Add(loansRegister);

                //  bankTransferRegister.grdCntrlInterBankTransfer.ItemsSource = interBankTransferList;

                cmbitem treeItem1 = new cmbitem() { name = "Loans" };
                cmbitem treeItema = new cmbitem() { name = "Loans(Open)" };
                ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                foreach (LoansStatus status in loansRepo.GetAllOpenLoansStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Loans" });
                }
                treeItema.Items = cmbItemsa;
                treeItem1.Items.Add(treeItema);
                cmbitem treeItemb = new cmbitem() { name = "Loans(Closed)" };

                ICollection<cmbitem> cmbItems = new List<cmbitem>();
                foreach (LoansStatus status in loansRepo.GetAllCloseLoansStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Loans" });
                }
                treeItemb.Items = cmbItems;
                treeItem1.Items.Add(treeItemb);

                //}


                TreeItems.Add(treeItem1);
                treeViewLoansStatus.ItemsSource = TreeItems;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }
    }
}
