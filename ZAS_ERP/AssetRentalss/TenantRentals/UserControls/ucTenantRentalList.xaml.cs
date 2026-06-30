using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals;
using ERP_BL.AssetsRentals.TenantRentals;
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
using ZAS_ERP.AssetRentalss.TenantRentals;

namespace ZAS_ERP.AssetRentalss.UserControls
{
    /// <summary>
    /// Interaction logic for ucTenantRentalList.xaml
    /// </summary>
    public partial class ucTenantRentalList : UserControl
    {
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        TenantRentalRepo tenantRepo = new TenantRentalRepo();
        public ucTenantRentalList()
        {
            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Tenants List") != null)
                {
                    ApprovalCount = tenantRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tenants without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Tenants") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Tenants") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Tenants") != null)
                    {
                        ApprovalCount = tenantRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = tenantRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Tenants List") != null)
                {
                    ReApprovalCount = tenantRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tenants without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Tenants") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Tenants") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Tenants") != null)
                    {
                        ReApprovalCount = tenantRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = tenantRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Tenants") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = tenantRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = tenantRepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Tenants") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = tenantRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
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
                ApprovalCount = tenantRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = tenantRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Tenants List") != null)
                {
                    ClosingCount = tenantRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Tenants without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Tenants") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Tenants") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Tenants") != null)
                    {
                        ClosingCount = tenantRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = tenantRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }


            else
            {
                ClosingCount = tenantRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlTenantRental.ItemsSource = tenantRepo.GetAllActiveTenantRentals(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlTenantRental);
        }

        private void AddNew_Click(object sender, RoutedEventArgs e)
        {
            ucFrmTenantRentalAdd frmAddTenantRental = new ucFrmTenantRentalAdd();
            frmAddTenantRental.editFlag = false;

            Window win = new Window();
            win.Content = frmAddTenantRental;
                        win.Show();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grdCntrlTenantRental.SelectedItem as TenantRental;

            if (selectedItem != null)
            {
                ucFrmTenantRentalAdd frmAddAssetRental = new ucFrmTenantRentalAdd();
                frmAddAssetRental.editFlag = true;
                frmAddAssetRental.tenantId = selectedItem.Id;

                Window win = new Window();
                win.Content = frmAddAssetRental;
                win.Show();
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ucTenantRentalList tenantRentalList = new ucTenantRentalList();
            this.DataContext = tenantRentalList;
            tenantRepo = new TenantRentalRepo();
            grdCntrlTenantRental.ItemsSource = tenantRepo.GetAllActiveTenantRentals(SYSTEM_STATIC.currentUser.id);
        }

        private void mbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlTenantRental);
        }

        static TenantRentalStatus statusChanged = new TenantRentalStatus();
        TenantRentalStatus oldStatus = new TenantRentalStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucTenantRentalList(TenantRentalStatus tenantRentalStatus)
        {
            statusChanged = tenantRentalStatus;
        }
        private void mbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            tenantRepo = new TenantRentalRepo();
            var selectedRow = (TenantRental)grdCntrlTenantRental.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var tenantRental = tenantRepo.GetTenantRentall(selectedRow.Id);
                if (tenantRental != null)
                {
                    if (tenantRental.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    ucTenantRentalStatusChange ucFrmDirectClose = new ucTenantRentalStatusChange();
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Tenants") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Tenants without Approval") != null)
                        {

                            tenantRental.PendingForClosing = false;
                            tenantRental.stage = TransactionStage.Closed.ToString();
                            tenantRental.statusId = statusChanged.Id;
                            tenantRental.LastStatusChangeDate = System.DateTime.Now;
                            tenantRental.ClosingDate = System.DateTime.Now;



                            tenantRepo.UpdateTenantRental(tenantRental);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.TenantRental, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Tenant having system ref #: " + tenantRental.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers
                                };
                                if (tenantRental != null)
                                {
                                    procurementRepo.Add(tenantRental.Id, TransactionItemType.TenantRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant #" + tenantRental.SystemRef, tenantRental.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant #" + tenantRental.SystemRef, tenantRental.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {

                            tenantRental.PendingForClosing = true;
                            tenantRental.stage = TransactionStage.AwaitingApproval.ToString();
                            tenantRental.statusId = statusChanged.Id;
                            tenantRental.LastStatusChangeDate = System.DateTime.Now;
                            tenantRental.ClosingDate = System.DateTime.Now;



                            tenantRepo.UpdateTenantRental(tenantRental);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.TenantRental, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Tenant having system ref #: " + tenantRental.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers
                                };
                                if (tenantRental != null)
                                {
                                    procurementRepo.Add(tenantRental.Id, TransactionItemType.TenantRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant #" + tenantRental.SystemRef, tenantRental.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Tenant #" + tenantRental.SystemRef, tenantRental.Id, TransactionItemType.TenantRental, comment.Comment, user.id, "New Comment ", null);
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
                if (grdCntrlTenantRental.GetFocusedRow() != null)
                {
                    tenantRepo = new TenantRentalRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (TenantRental)grdCntrlTenantRental.GetFocusedRow();
                    var tenantRental = tenantRepo.GetTenantRentall(selectedRow.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (tenantRental != null)
                    {
                        if (tenantRental.isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tenants without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Tenants") != null) ? true : false)
                            {

                                tenantRental.isApproved = true;
                                tenantRental.stage = TransactionStage.Approved.ToString();

                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.TenantRental, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                tenantRepo.UpdateTenantRental(tenantRental);

                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show(" Tenant is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), " Tenant is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Tenant Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Tenant Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (tenantRental.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Tenants without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Tenants") != null) ? true : false)
                            {
                                tenantRental.isReApproved = true;
                                tenantRental.stage = TransactionStage.Approved.ToString();


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.TenantRental, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                tenantRepo.UpdateTenantRental(tenantRental);
                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show(" Tenant is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), " Tenant is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Tenants Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Tenants Directly user id=(" + MainWindow.currentUserid + ")");
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Tenants List") != null)
            {
                tenantRepo = new TenantRentalRepo();
                lblHeading.Text = "Pending for Approval Tenants";
                var tenants = tenantRepo.GetAllPendingForApproval(SYSTEM_STATIC.currentUser.id);
                grdCntrlTenantRental.ItemsSource = tenants;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Tenants!!");
            }
        }

        private void mbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Tenants List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Tenants";
                var tenantRentals = tenantRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                grdCntrlTenantRental.ItemsSource = tenantRentals;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Tenants!!");
            }
        }

        private void mbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Tenants List") != null)
            {
                var tenantRentals = tenantRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Tenants";
                grdCntrlTenantRental.ItemsSource = tenantRentals;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Tenants!");
            }
        }

        private void mbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Tenants") != null)
            {
                tenantRepo = new TenantRentalRepo();
                var tenantRentals = tenantRepo.GetAllTenantRentals(SYSTEM_STATIC.currentUser.id);
                grdCntrlTenantRental.ItemsSource = tenantRentals;
                lblHeading.Text = " Tenant Register";
            }
            else
            {
                MessageBox.Show("Permission required to View list of Tenants!");
            }
        }

        private void mbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Tenants") != null)
            {
                lblHeading.Text = "Void Tenants";

                var tenantRentals = tenantRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                grdCntrlTenantRental.ItemsSource = tenantRentals;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Tenants!!");
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

        private void grdCntrlTenantRental_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdCntrlTenantRental.GetRowByListIndex(e.ListSourceRowIndex) as TenantRental;
            switch (e.Column.FieldName)
            {
                case "Creatorr":
                    if(row.Creator != null && row.Creator.employee != null)
                    {
                        e.Value = row.Creator.employee.person.FName + " " + row.Creator.employee.person.LName;
                    }
                    break;
            }
        }

        private void grdCntrlTenantRental_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = grdCntrlTenantRental.SelectedItem as TenantRental;

            if (selectedItem != null)
            {
                ucFrmTenantRentalAdd frmAddAssetRental = new ucFrmTenantRentalAdd();
                frmAddAssetRental.editFlag = true;
                frmAddAssetRental.tenantId = selectedItem.Id;

                Window win = new Window();
                win.Content = frmAddAssetRental;
                win.Show();
            }
        }
    }
}
