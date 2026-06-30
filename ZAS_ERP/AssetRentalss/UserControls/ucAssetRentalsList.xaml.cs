using DevExpress.Xpf.Core;
using ERP_BL.AssetsRentals;
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
using ZAS_ERP.Reportss;

namespace ZAS_ERP.AssetRentalss.UserControls
{
    /// <summary>
    /// Interaction logic for ucAssetRentalsList.xaml
    /// </summary>
    public partial class ucAssetRentalsList : UserControl
    {
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        AssetRentalRepo rentalRepo = new AssetRentalRepo();
        public ucAssetRentalsList()
        {

            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Assets List") != null)
                {
                    ApprovalCount = rentalRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Assets without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Assets") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Assets") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Assets") != null)
                    {
                        ApprovalCount = rentalRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = rentalRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Assets List") != null)
                {
                    ReApprovalCount = rentalRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Assets without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Assets") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Assets") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Assets") != null)
                    {
                        ReApprovalCount = rentalRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = rentalRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Assets") != null)
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
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Assets") != null)
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
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Assets List") != null)
                {
                    ClosingCount = rentalRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Assets without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Assets") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Assets") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Assets") != null)
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
            grdCntrlAssetRental.ItemsSource = rentalRepo.GetAllActiveAssetRentals(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlAssetRental);
        }

        private void grdCntrlAssetRental_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdCntrlAssetRental.GetRowByListIndex(e.ListSourceRowIndex) as AssetRental;
            if (e.Column.FieldName == "Vendorr" && e.IsGetData)
            {
                if(row.vendor != null)
                    e.Value = row.vendor.company.CompanyName;
                else
                    e.Value = row.Vendor;
            }
            if (e.Column.FieldName == "AssetHolderr" && e.IsGetData)
            {
                if (row.assetHolderEmployee != null)
                    e.Value = row.assetHolderEmployee.person?.FullName;
                else
                    e.Value = row.AssetHolder;
            }
            if (e.Column.FieldName == "AssetNumberr" && e.IsGetData)
            {
                string assetNo = "";
                if (row.assetNumber != null)
                    assetNo = row.assetNumber.Number;
                assetNo = assetNo + row.AssetNumber;
                e.Value = assetNo;
            }
            if (e.Column.FieldName == "PurchasingggCost" && e.IsGetData)
            {
                e.Value = Convert.ToDecimal( row.adminBills.Where(x => x.isProgressiveCost == false).Sum(x => x.AmountOC).ToString());
            }
            if (e.Column.FieldName == "ProgressiveeCost" && e.IsGetData)
            {
                e.Value = Convert.ToDecimal( row.adminBills.Where(x => x.isProgressiveCost == true).Sum(x => x.AmountOC));
            }
        }

        private void grdCntrlAssetRental_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Assets") != null)
            {
                var selectedItem = grdCntrlAssetRental.SelectedItem as AssetRental;

                if (selectedItem != null)
                {
                    ucFrmAddAssetRental frmAddAssetRental = new ucFrmAddAssetRental();
                    frmAddAssetRental.editFlag = true;
                    frmAddAssetRental.assetId = selectedItem.Id;

                    Window win = new Window();
                    win.Content = frmAddAssetRental;
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to view Asset!");
            }
        }

        private void mbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAddAssetRental frmAddAssetRental = new ucFrmAddAssetRental();
            frmAddAssetRental.editFlag = false;

            Window win = new Window();
            win.Content = frmAddAssetRental;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void mbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Assets") != null)
            {
                var selectedItem = grdCntrlAssetRental.SelectedItem as AssetRental;

                if (selectedItem != null)
                {
                    ucFrmAddAssetRental frmAddAssetRental = new ucFrmAddAssetRental();
                    frmAddAssetRental.editFlag = true;
                    frmAddAssetRental.assetId = selectedItem.Id;

                    Window win = new Window();
                    win.Content = frmAddAssetRental;
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to view Asset!");
            }
        }

        private void mbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Assets Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("Assets");
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdCntrlAssetRental, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Loans Advances Reports!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ucAssetRentalsList rentalContractList = new ucAssetRentalsList();
            this.DataContext = rentalContractList;
            rentalRepo = new AssetRentalRepo();
            grdCntrlAssetRental.ItemsSource = rentalRepo.GetAllActiveAssetRentals(SYSTEM_STATIC.currentUser.id);
        }

        private void mbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlAssetRental);
        }

        static AssetRentalStatus statusChanged = new AssetRentalStatus();
        AssetRentalStatus oldStatus = new AssetRentalStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucAssetRentalsList(AssetRentalStatus assetRentalStatus)
        {
            statusChanged = assetRentalStatus;
        }
        private void mbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            rentalRepo = new AssetRentalRepo();
            var selectedRow = (AssetRental)grdCntrlAssetRental.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var assetRental = rentalRepo.GetAssetRental(selectedRow.Id);
                if (assetRental != null)
                {
                    if (assetRental.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    ucAssetRentalStatusChange ucFrmDirectClose = new ucAssetRentalStatusChange();
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Assets") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Assets without Approval") != null)
                        {

                            assetRental.PendingForClosing = false;
                            assetRental.stage = TransactionStage.Closed.ToString();
                            assetRental.statusId = statusChanged.Id;
                            assetRental.LastStatusChangeDate = System.DateTime.Now;
                            assetRental.ClosingDate = System.DateTime.Now;



                            rentalRepo.UpdateAssetRental(assetRental);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Asset having system ref #: " + assetRental.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (assetRental != null)
                                {
                                    procurementRepo.Add(assetRental.Id, TransactionItemType.AssetRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset #" + assetRental.SystemRef, assetRental.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset #" + assetRental.SystemRef, assetRental.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {

                            assetRental.PendingForClosing = true;
                            assetRental.stage = TransactionStage.AwaitingApproval.ToString();
                            assetRental.statusId = statusChanged.Id;
                            assetRental.LastStatusChangeDate = System.DateTime.Now;
                            assetRental.ClosingDate = System.DateTime.Now;



                            rentalRepo.UpdateAssetRental(assetRental);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Asset having system ref #: " + assetRental.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (assetRental != null)
                                {
                                    procurementRepo.Add(assetRental.Id, TransactionItemType.AssetRental, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset #" + assetRental.SystemRef, assetRental.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Asset #" + assetRental.SystemRef, assetRental.Id, TransactionItemType.AssetRental, comment.Comment, user.id, "New Comment ", null);
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
                if (grdCntrlAssetRental.GetFocusedRow() != null)
                {
                    rentalRepo = new AssetRentalRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (AssetRental)grdCntrlAssetRental.GetFocusedRow();
                    var rentalContract = rentalRepo.GetAssetRental(selectedRow.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (rentalContract != null)
                    {
                        if (rentalContract.isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Assets without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Assets") != null) ? true : false)
                            {

                                rentalContract.isApproved = true;
                                rentalContract.stage = TransactionStage.Approved.ToString();

                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                rentalRepo.UpdateAssetRental(rentalContract);

                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show(" Asset is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), " Asset is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Asset Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Asset Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (rentalContract.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Assets without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Assets") != null) ? true : false)
                            {
                                rentalContract.isReApproved = true;
                                rentalContract.stage = TransactionStage.Approved.ToString();


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.AssetRental, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                rentalRepo.UpdateAssetRental(rentalContract);
                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show(" Asset is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), " Asset is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Assets Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Assets Directly user id=(" + MainWindow.currentUserid + ")");
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Assets List") != null)
            {
                rentalRepo = new AssetRentalRepo();
                lblHeading.Text = "Pending for Approval Assets";
                var rentalContracts = rentalRepo.GetAllPendingForApproval(SYSTEM_STATIC.currentUser.id);
                grdCntrlAssetRental.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Assets!!");
            }
        }

        private void mbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Assets List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Assets";
                var rentalContracts = rentalRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                grdCntrlAssetRental.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Assets!!");
            }
        }

        private void mbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Assets List") != null)
            {

                var loansAdvanves = rentalRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Assets";
                grdCntrlAssetRental.ItemsSource = loansAdvanves;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Assets!");
            }
        }

        private void mbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Assets") != null)
            {
                rentalRepo = new AssetRentalRepo();
                var rentalContracts = rentalRepo.GetAllAssetRentals(SYSTEM_STATIC.currentUser.id);
                grdCntrlAssetRental.ItemsSource = rentalContracts;
                lblHeading.Text = " Asset Register";
            }
            else
            {
                MessageBox.Show("Permission required to View list of Assets!");
            }
        }

        private void mbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Assets") != null)
            {
                lblHeading.Text = "Void Assets";

                var rentalContracts = rentalRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                grdCntrlAssetRental.ItemsSource = rentalContracts;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Assets!!");
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
