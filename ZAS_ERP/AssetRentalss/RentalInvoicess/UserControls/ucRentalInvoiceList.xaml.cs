using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals.RentalInvoices;
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

namespace ZAS_ERP.AssetRentalss.RentalInvoicess.UserControls
{
    /// <summary>
    /// Interaction logic for ucRentalInvoiceList.xaml
    /// </summary>
    public partial class ucRentalInvoiceList : UserControl
    {
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        RentalInvoiceRepo invoiceRepo = new RentalInvoiceRepo();
        public ucRentalInvoiceList()
        {

            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Rental Invoices List") != null)
                {
                    ApprovalCount = invoiceRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Invoices without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Invoices") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Rental Invoices") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Rental Invoices") != null)
                    {
                        ApprovalCount = invoiceRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = invoiceRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Rental Invoices List") != null)
                {
                    ReApprovalCount = invoiceRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Invoices without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Rental Invoices") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Rental Invoices") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Rental Invoices") != null)
                    {
                        ReApprovalCount = invoiceRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = invoiceRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Rental Invoices") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = invoiceRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = invoiceRepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Rental Invoices") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = invoiceRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
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
                ApprovalCount = invoiceRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = invoiceRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Rental Invoices List") != null)
                {
                    ClosingCount = invoiceRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Rental Invoices without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Rental Invoices") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Rental Invoices") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Rental Invoices") != null)
                    {
                        ClosingCount = invoiceRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = invoiceRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }


            else
            {
                ClosingCount = invoiceRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdRentalInvoice.ItemsSource = invoiceRepo.GetAllActiveRentalInvoices(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdRentalInvoice);
        }

        private void grdRentalInvoice_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void grdRentalInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Invoices") != null)
            {
                var selectedItem = grdRentalInvoice.SelectedItem as RentalInvoice;

                if (selectedItem != null)
                {
                    ucRentalInvoiceAdd frmAddAssetRental = new ucRentalInvoiceAdd();
                    frmAddAssetRental.editFlag = true;
                    frmAddAssetRental.invoiceId = selectedItem.Id;
                    frmAddAssetRental.orderId = selectedItem.rentalOrderId.Value;

                    Window win = new Window();
                    win.Content = frmAddAssetRental;
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to view Rental Invoice!");
            }
        }

        private void mbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucRentalInvoiceAdd frmAddAssetRental = new ucRentalInvoiceAdd();
            frmAddAssetRental.editFlag = false;

            Window win = new Window();
            win.Content = frmAddAssetRental;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void mbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Rental Invoices") != null)
            {
                var selectedItem = grdRentalInvoice.SelectedItem as RentalInvoice;

                if (selectedItem != null)
                {
                    ucRentalInvoiceAdd frmAddAssetRental = new ucRentalInvoiceAdd();
                    frmAddAssetRental.editFlag = true;
                    frmAddAssetRental.invoiceId = selectedItem.Id;
                    frmAddAssetRental.orderId = selectedItem.rentalOrderId.Value;

                    Window win = new Window();
                    win.Content = frmAddAssetRental;
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to view Rental Invoice!");
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
            ucRentalInvoiceList rentalContractList = new ucRentalInvoiceList();
            this.DataContext = rentalContractList;
            invoiceRepo = new RentalInvoiceRepo();
            grdRentalInvoice.ItemsSource = invoiceRepo.GetAllActiveRentalInvoices(SYSTEM_STATIC.currentUser.id);
        }

        private void mbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdRentalInvoice);
        }

        static RentalInvoiceStatus statusChanged = new RentalInvoiceStatus();
        RentalInvoiceStatus oldStatus = new RentalInvoiceStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucRentalInvoiceList(RentalInvoiceStatus rentalContractStatus)
        {
            statusChanged = rentalContractStatus;
        }
        private void mbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            invoiceRepo = new RentalInvoiceRepo();
            var selectedRow = (RentalInvoice)grdRentalInvoice.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var rentalContract = invoiceRepo.GetRentalInvoice(selectedRow.Id);
                if (rentalContract != null)
                {
                    if (rentalContract.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    ucRentalInvoiceStatusChange ucFrmDirectClose = new ucRentalInvoiceStatusChange();
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Rental Invoices") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Rental Invoices without Approval") != null)
                        {

                            rentalContract.PendingForClosing = false;
                            rentalContract.stage = TransactionStage.Closed.ToString();
                            rentalContract.statusId = statusChanged.Id;
                            rentalContract.LastStatusChangeDate = System.DateTime.Now;
                            rentalContract.ClosingDate = System.DateTime.Now;



                            invoiceRepo.UpdateRentalInvoice(rentalContract);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.RentalInvoice, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Invoice having system ref #: " + rentalContract.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers,
                                    TaggedRecomenndedList = tagUsersRecommendation,
                                    CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalContract != null)
                                {
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalInvoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);
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



                            invoiceRepo.UpdateRentalInvoice(rentalContract);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.RentalInvoice, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Rental Invoice having system ref #: " + rentalContract.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers,
                                    TaggedRecomenndedList = tagUsersRecommendation,
                                    CCRecomenndedList = ccUsersRecommendation
                                };
                                if (rentalContract != null)
                                {
                                    procurementRepo.Add(rentalContract.Id, TransactionItemType.RentalInvoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Rental Invoice #" + rentalContract.SystemRef, rentalContract.Id, TransactionItemType.RentalInvoice, comment.Comment, user.id, "New Comment ", null);
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
                if (grdRentalInvoice.GetFocusedRow() != null)
                {
                    invoiceRepo = new RentalInvoiceRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (RentalInvoice)grdRentalInvoice.GetFocusedRow();
                    var rentalContract = invoiceRepo.GetRentalInvoice(selectedRow.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (rentalContract != null)
                    {
                        if (rentalContract.isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Invoices without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Rental Invoices") != null) ? true : false)
                            {

                                rentalContract.isApproved = true;
                                rentalContract.stage = TransactionStage.Approved.ToString();

                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, 25, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                invoiceRepo.UpdateRentalInvoice(rentalContract);

                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show(" Rental Invoice is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), " Rental Invoice is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Rental Invoice Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Invoice Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (rentalContract.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Rental Invoices without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Rental Invoices") != null) ? true : false)
                            {
                                rentalContract.isReApproved = true;
                                rentalContract.stage = TransactionStage.Approved.ToString();


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.RentalInvoice, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                invoiceRepo.UpdateRentalInvoice(rentalContract);
                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show(" Rental Invoice is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), " Rental Invoice is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Rental Invoices Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Rental Invoices Directly user id=(" + MainWindow.currentUserid + ")");
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Rental Invoices List") != null)
            {
                invoiceRepo = new RentalInvoiceRepo();
                lblHeading.Text = "Pending for Approval Rental Invoices";
                var rentalContracts = invoiceRepo.GetAllPendingForApproval(SYSTEM_STATIC.currentUser.id);
                grdRentalInvoice.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Rental Invoices!!");
            }
        }

        private void mbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Rental Invoices List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Rental Invoices";
                var rentalContracts = invoiceRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                grdRentalInvoice.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Rental Invoices!!");
            }
        }

        private void mbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Rental Invoices List") != null)
            {

                var loansAdvanves = invoiceRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Rental Invoices";
                grdRentalInvoice.ItemsSource = loansAdvanves;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Rental Invoices!");
            }
        }

        private void mbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Rental Invoices") != null)
            {
                invoiceRepo = new RentalInvoiceRepo();
                var rentalContracts = invoiceRepo.GetAllRentalInvoices(SYSTEM_STATIC.currentUser.id);
                grdRentalInvoice.ItemsSource = rentalContracts;
                lblHeading.Text = "Rental Invoice Register";
            }
            else
            {
                MessageBox.Show("Permission required to View list of Rental Invoices!");
            }
        }

        private void mbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Rental Invoices") != null)
            {
                lblHeading.Text = "Void Rental Invoices";

                var rentalContracts = invoiceRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                grdRentalInvoice.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Rental Invoices!!");
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
