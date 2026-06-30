using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals.RentalOrders;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.AssetRentalss.RentalOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucRentalOrderList.xaml
    /// </summary>
    public partial class ucRentalOrderList : UserControl
    {
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        RentalOrderRepo rentalRepo = new RentalOrderRepo();
        public ucRentalOrderList()
        {

            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Rental Orders List") != null)
                {
                    ApprovalCount = rentalRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Orders without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Orders") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Rental Orders") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Rental Orders") != null)
                    {
                        ApprovalCount = rentalRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = rentalRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Rental Orders List") != null)
                {
                    ReApprovalCount = rentalRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Orders without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Rental Orders") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Rental Orders") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Rental Orders") != null)
                    {
                        ReApprovalCount = rentalRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = rentalRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Rental Orders") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = rentalRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = rentalRepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Rental Orders") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = rentalRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
                    //}
                    //else
                    //{
                    //    ApproveunapprovedCount = bankTransRepo.getBankTransferRegisterCountOWn(MainWindow.currentUserid);
                    //}
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                ApprovalCount = rentalRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = rentalRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Rental Orders List") != null)
                {
                    ClosingCount = rentalRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Rental Orders without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Rental Orders") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Rental Orders") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Rental Orders") != null)
                    {
                        ClosingCount = rentalRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = rentalRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }


            else
            {
                ClosingCount = rentalRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdRentalOrder.ItemsSource = rentalRepo.GetAllActiveRentalOrders(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdRentalOrder);
        }

        private void grdRentalOrder_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void grdRentalOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Orders") != null)
            {
                var selectedItem = grdRentalOrder.SelectedItem as RentalOrder;

                if (selectedItem != null)
                {
                    ucRentalOrderAdd frmAddAssetRental = new ucRentalOrderAdd();
                    frmAddAssetRental.editFlag = true;
                    frmAddAssetRental.orderId = selectedItem.Id;
                    frmAddAssetRental.contractId = selectedItem.rentalContractId.Value;

                    Window win = new Window();
                    win.Content = frmAddAssetRental;
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to view Rental Order!");
            }
        }

        private void mbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucRentalOrderAdd frmAddAssetRental = new ucRentalOrderAdd();
            frmAddAssetRental.editFlag = false;

            Window win = new Window();
            win.Content = frmAddAssetRental;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void mbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Orders") != null)
            {
                var selectedItem = grdRentalOrder.SelectedItem as RentalOrder;

                if (selectedItem != null)
                {
                    ucRentalOrderAdd frmAddAssetRental = new ucRentalOrderAdd();
                    frmAddAssetRental.editFlag = true;
                    frmAddAssetRental.orderId = selectedItem.Id;
                    frmAddAssetRental.contractId = selectedItem.rentalContractId.Value;

                    Window win = new Window();
                    win.Content = frmAddAssetRental;
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to view Rental Order!");
            }
        }

        private void mbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ucRentalOrderList rentalContractList = new ucRentalOrderList();
            this.DataContext = rentalContractList;
            rentalRepo = new RentalOrderRepo();
            grdRentalOrder.ItemsSource = rentalRepo.GetAllActiveRentalOrders(SYSTEM_STATIC.currentUser.id);
        }

        private void mbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdRentalOrder);
        }

        static RentalOrderStatus statusChanged = new RentalOrderStatus();
        RentalOrderStatus oldStatus = new RentalOrderStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucRentalOrderList(RentalOrderStatus rentalContractStatus)
        {
            statusChanged = rentalContractStatus;
        }
        private void mbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            rentalRepo = new RentalOrderRepo();
            var selectedRow = (RentalOrder)grdRentalOrder.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var rentalContract = rentalRepo.GetRentalOrder(selectedRow.Id);
                if (rentalContract != null)
                {
                    if (rentalContract.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    ucRentalOrderStatusChange ucFrmDirectClose = new ucRentalOrderStatusChange();
                    if (selectedRow.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = selectedRow.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(selectedRow.Status.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;

                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Rental Orders") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Rental Orders without Approval") != null)
                        {

                            rentalContract.PendingForClosing = false;
                            rentalContract.stage = TransactionStage.Closed.ToString();
                            rentalContract.statusId = statusChanged.Id;
                            rentalContract.LastStatusChangeDate = System.DateTime.Now;
                            rentalContract.ClosingDate = System.DateTime.Now;



                            rentalRepo.UpdateRentalOrder(rentalContract);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.RentalOrder, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Order having system ref #: " + rentalContract.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalContract != null)
                                {
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalOrder, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Order #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalOrder, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Order #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalOrder, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {

                            rentalContract.PendingForClosing = true;
                            rentalContract.stage = TransactionStage.AwaitingApproval.ToString();
                            rentalContract.statusId = statusChanged.Id;
                            rentalContract.LastStatusChangeDate = System.DateTime.Now;
                            rentalContract.ClosingDate = System.DateTime.Now;



                            rentalRepo.UpdateRentalOrder(rentalContract);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.RentalOrder, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Order having system ref #: " + rentalContract.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalContract != null)
                                {
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalOrder, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Order #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalOrder, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Order #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalOrder, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        private void mbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (grdRentalOrder.GetFocusedRow() != null)
                {
                    rentalRepo = new RentalOrderRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (RentalOrder)grdRentalOrder.GetFocusedRow();
                    var rentalContract = rentalRepo.GetRentalOrder(selectedRow.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (rentalContract != null)
                    {
                        if (rentalContract.isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Orders without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Orders") != null) ? true : false)
                            {

                                rentalContract.isApproved = true;
                                rentalContract.stage = TransactionStage.Approved.ToString();

                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, 25, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                rentalRepo.UpdateRentalOrder(rentalContract);

                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show(" Rental Order is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), " Rental Order is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Rental Order Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Order Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (rentalContract.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Orders without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Rental Orders") != null) ? true : false)
                            {
                                rentalContract.isReApproved = true;
                                rentalContract.stage = TransactionStage.Approved.ToString();


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.RentalOrder, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                rentalRepo.UpdateRentalOrder(rentalContract);
                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show(" Rental Order is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), " Rental Order is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Rental Orders Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Orders Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                    }
                    //Load_Receipts();
                }
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void mbtnPending_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Rental Orders List") != null)
            {
                rentalRepo = new RentalOrderRepo();
                lblHeading.Text = "Pending for Approval Rental Orders";
                var rentalContracts = rentalRepo.GetAllPendingForApproval(SYSTEM_STATIC.currentUser.id);
                grdRentalOrder.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Rental Orders!!");
            }
        }

        private void mbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Rental Orders List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Rental Orders";
                var rentalContracts = rentalRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                grdRentalOrder.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Rental Orders!!");
            }
        }

        private void mbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Rental Orders List") != null)
            {

                var loansAdvanves = rentalRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Rental Orders";
                grdRentalOrder.ItemsSource = loansAdvanves;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Rental Orders!");
            }
        }

        private void mbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Rental Orders") != null)
            {
                rentalRepo = new RentalOrderRepo();
                var rentalContracts = rentalRepo.GetAllRentalOrders(SYSTEM_STATIC.currentUser.id);
                grdRentalOrder.ItemsSource = rentalContracts;
                lblHeading.Text = " Rental Order Register";
            }
            else
            {
                MessageBox.Show("Permission required to View list of Rental Orders!");
            }
        }

        private void mbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Rental Orders") != null)
            {
                lblHeading.Text = "Void Rental Orders";

                var rentalContracts = rentalRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                grdRentalOrder.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Rental Orders!!");
            }
        }

        private void cmbxDepartmentFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void cmbxDepartmentTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void btnDepartmentFilter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
