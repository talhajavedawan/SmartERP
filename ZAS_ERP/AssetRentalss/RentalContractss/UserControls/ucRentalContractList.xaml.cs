using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals;
using ERP_BL.AssetsRentals.RentalContracts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ZAS_ERP.AssetRentalss.UserControls
{
    /// <summary>
    /// Interaction logic for ucRentalContractList.xaml
    /// </summary>
    public partial class ucRentalContractList : UserControl
    {
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        RentalContractRepo rentalRepo = new RentalContractRepo();
        public ucRentalContractList()
        {
            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Rental Contracts List") != null)
                {
                    ApprovalCount = rentalRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Contracts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Rental Contracts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Rental Contracts") != null)
                    {
                        ApprovalCount = rentalRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = rentalRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Rental Contracts List") != null)
                {
                    ReApprovalCount = rentalRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Rental Contracts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Rental Contracts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Rental Contracts") != null)
                    {
                        ReApprovalCount = rentalRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = rentalRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Rental Contracts") != null)
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
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Rental Contracts") != null)
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
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Rental Contracts List") != null)
                {
                    ClosingCount = rentalRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Rental Contracts without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Rental Contracts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Rental Contracts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Rental Contracts") != null)
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
            grdRentalContract.ItemsSource = rentalRepo.GetAllActiveRentalContracts(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdRentalContract);
        }

        private void grdRentalContract_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void grdRentalContract_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Contracts") != null)
            {
                var selectedItem = grdRentalContract.SelectedItem as RentalContract;

                if (selectedItem != null)
                {
                    ucFrmRentalContractAdd frmAddAssetRental = new ucFrmRentalContractAdd();
                    frmAddAssetRental.editFlag = true;
                    frmAddAssetRental.contractId = selectedItem.Id;

                    Window win = new Window();
                    win.Content = frmAddAssetRental;
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to view Rental Contract!");
            }
        }

        private void mbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmRentalContractAdd frmAddAssetRental = new ucFrmRentalContractAdd();
            frmAddAssetRental.editFlag = false;

            Window win = new Window();
            win.Content = frmAddAssetRental;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void mbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "View Rental Contracts") != null)
            {
                var selectedItem = grdRentalContract.SelectedItem as RentalContract;

                if (selectedItem != null)
                {
                    ucFrmRentalContractAdd frmAddAssetRental = new ucFrmRentalContractAdd();
                    frmAddAssetRental.editFlag = true;
                    frmAddAssetRental.contractId = selectedItem.Id;

                    Window win = new Window();
                    win.Content = frmAddAssetRental;
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to view Rental Contract!");
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
            ucRentalContractList rentalContractList = new ucRentalContractList();
            this.DataContext = rentalContractList;
            rentalRepo = new RentalContractRepo();
            grdRentalContract.ItemsSource = rentalRepo.GetAllActiveRentalContracts(SYSTEM_STATIC.currentUser.id);
        }

        private void mbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdRentalContract);
        }

        static RentalContractStatus statusChanged = new RentalContractStatus();
        RentalContractStatus oldStatus = new RentalContractStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucRentalContractList(RentalContractStatus rentalContractStatus)
        {
            statusChanged = rentalContractStatus;
        }
        private void mbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            rentalRepo = new RentalContractRepo();
            var selectedRow = (RentalContract)grdRentalContract.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var rentalContract = rentalRepo.GetRentalContract(selectedRow.Id);
                if (rentalContract != null)
                {
                    if (rentalContract.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    ucRentalContractStatusChange ucFrmDirectClose = new ucRentalContractStatusChange();
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Rental Contracts") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Rental Contracts without Approval") != null)
                        {

                            rentalContract.PendingForClosing = false;
                            rentalContract.stage = TransactionStage.Closed.ToString();
                            rentalContract.statusId = statusChanged.Id;
                            rentalContract.LastStatusChangeDate = System.DateTime.Now;
                            rentalContract.ClosingDate = System.DateTime.Now;



                            rentalRepo.UpdateRentalContract(rentalContract);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.RentalContract, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Contract having system ref #: " + rentalContract.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalContract != null)
                                {
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);
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



                            rentalRepo.UpdateRentalContract(rentalContract);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.RentalContract, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Contract having system ref #: " + rentalContract.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalContract != null)
                                {
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalContract, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Contract #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalContract, comment.Comment, user.id, "New Comment ", null);
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
                if (grdRentalContract.GetFocusedRow() != null)
                {
                    rentalRepo = new RentalContractRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (RentalContract)grdRentalContract.GetFocusedRow();
                    var rentalContract = rentalRepo.GetRentalContract(selectedRow.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (rentalContract != null)
                    {
                        if (rentalContract.isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Contracts") != null) ? true : false)
                            {

                                rentalContract.isApproved = true;
                                rentalContract.stage = TransactionStage.Approved.ToString();

                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, 25, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                rentalRepo.UpdateRentalContract(rentalContract);

                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Rental Contract is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Rental Contract is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Rental Contract Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Contract Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (rentalContract.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Contracts without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Rental Contracts") != null) ? true : false)
                            {
                                rentalContract.isReApproved = true;
                                rentalContract.stage = TransactionStage.Approved.ToString();


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.RentalContract, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                rentalRepo.UpdateRentalContract(rentalContract);
                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Rental Contract is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Rental Contract is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Rental Contracts Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Contracts Directly user id=(" + MainWindow.currentUserid + ")");
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Rental Contracts List") != null)
            {
                rentalRepo = new RentalContractRepo();
                lblHeading.Text = "Pending for Approval Rental Contracts";
                var rentalContracts = rentalRepo.GetAllPendingForApproval(SYSTEM_STATIC.currentUser.id);
                grdRentalContract.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Rental Contracts!!");
            }
        }

        private void mbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Rental Contracts List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Rental Contracts";
                var rentalContracts = rentalRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                grdRentalContract.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Rental Contracts!!");
            }
        }

        private void mbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Rental Contracts List") != null)
            {

                var loansAdvanves = rentalRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Rental Contracts";
                grdRentalContract.ItemsSource = loansAdvanves;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Rental Contracts!");
            }
        }

        private void mbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Rental Contracts") != null)
            {
                rentalRepo = new RentalContractRepo();
                var rentalContracts = rentalRepo.GetAllRentalContracts(SYSTEM_STATIC.currentUser.id);
                grdRentalContract.ItemsSource = rentalContracts;
                lblHeading.Text = "Rental Contract Register";
            }
            else
            {
                MessageBox.Show("Permission required to View list of Rental Contracts!");
            }
        }

        private void mbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Rental Contracts") != null)
            {
                lblHeading.Text = "Void Rental Contracts";

                var rentalContracts = rentalRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                grdRentalContract.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Rental Contracts!!");
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
