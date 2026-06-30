using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.FilesAndDocs;
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

namespace ZAS_ERP.FilesAndDocss.TravellingRecords.UserControls
{
    /// <summary>
    /// Interaction logic for ucTravelingRecordList.xaml
    /// </summary>
    public partial class ucTravelingRecordList : UserControl
    {
       
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        VisitingRecordRepo recordRepo = new VisitingRecordRepo();
        public ucTravelingRecordList()
        {
            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Traveling Record List") != null)
                {
                    ApprovalCount = recordRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Traveling Record") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Traveling Record") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Traveling Record") != null)
                    {
                        ApprovalCount = recordRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = recordRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Traveling Record List") != null)
                {
                    ReApprovalCount = recordRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Traveling Record") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Traveling Record") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Traveling Record") != null)
                    {
                        ReApprovalCount = recordRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = recordRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Traveling Record") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = recordRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = recordRepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Traveling Record") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = recordRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
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
                ApprovalCount = recordRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = recordRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Traveling Record") != null)
                {
                    ClosingCount = recordRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Traveling Record without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Traveling Record") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Traveling Record") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Traveling Record") != null)
                    {
                        ClosingCount = recordRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = recordRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }


            else
            {
                ClosingCount = recordRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdTravelingRecordRegister.ItemsSource = recordRepo.GetAllActiveTravelRecords(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdTravelingRecordRegister);
        }

        public void GridControlSetUserSettings()
        {
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdTravelingRecordRegister);
        }

        private void grdTravelingRecordRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Traveling Record") != null)
            {
                var selectedItem = grdTravelingRecordRegister.SelectedItem as VisitingCountry;
                if (selectedItem != null)
                {
                    ucFrmTravelingRecord ucFrmTraveling = new ucFrmTravelingRecord();
                    ucFrmTraveling.recordId = selectedItem.travelingRecord.Id;
                    ucFrmTraveling.editFlag = true;
                    DXWindow win = new DXWindow();
                    win.Content = ucFrmTraveling;
                    win.WindowState = WindowState.Maximized;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }

        }

        private void grdTravelingRecordRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "NumberOfDays" && e.IsGetData)
            {
                var visit = grdTravelingRecordRegister.GetRowByListIndex(e.ListSourceRowIndex) as VisitingCountry;

                //if (visit.End == true)
                //{
                //    if (visit != null && visit.DepartureDate != null && visit.ArrivalDate != null)
                //        e.Value = (visit.ArrivalDate.Value - visit.DepartureDate.Value).TotalDays;
                //}
                //else
                //{
                if (visit != null && visit.DepartureDate != null && visit.LeavingDate != null)
                    e.Value = (visit.LeavingDate.Value - visit.DepartureDate.Value).TotalDays;
                //}
            }
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record") != null)
            {
                ucFrmTravelingRecord frmTraveler = new ucFrmTravelingRecord();
                Window win = new Window();
                win.Content = frmTraveler;
                win.WindowState = WindowState.Maximized;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
            }
        }

        private void mbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Traveling Record") != null)
            {
                var selectedItem = grdTravelingRecordRegister.SelectedItem as VisitingCountry;
                if (selectedItem != null)
                {
                    ucFrmTravelingRecord frmTraveler = new ucFrmTravelingRecord();
                    frmTraveler.recordId = selectedItem.travelingRecord.Id;
                    frmTraveler.editFlag = true;
                    DXWindow win = new DXWindow();
                    win.Content = frmTraveler;
                    win.WindowState = WindowState.Maximized;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission denied!");
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
            ucTravelingRecordList travelingRecordList = new ucTravelingRecordList();
            this.DataContext = travelingRecordList;
            recordRepo = new VisitingRecordRepo();
            grdTravelingRecordRegister.ItemsSource = recordRepo.GetAllActiveTravelRecords(SYSTEM_STATIC.currentUser.id);
            lblHeading.Text = "Traveling Records";

        }

        private void mbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdTravelingRecordRegister);
        }


        static TravelingStatus statusChanged = new TravelingStatus();
        TravelingStatus oldStatus = new TravelingStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucTravelingRecordList(TravelingStatus travelingStatus)
        {
            statusChanged = travelingStatus;
        }
        private void mbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            recordRepo = new VisitingRecordRepo();
            var selectedRow = (VisitingCountry)grdTravelingRecordRegister.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.travelingRecord.Status.Status;
                var travelingRecord = recordRepo.GetTravelingRecord(selectedRow.TravelingRecordsId.Value);
                if (travelingRecord != null)
                {
                    if (travelingRecord.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    frmTravelingStatusChange ucFrmDirectClose = new frmTravelingStatusChange();
                    if (travelingRecord.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = travelingRecord.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(travelingRecord.Status.backcolor);
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Traveling Record") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Traveling Record without Approval") != null)
                        {

                            travelingRecord.PendingForClosing = false;
                            travelingRecord.stage = TransactionStage.Closed.ToString();
                            travelingRecord.statusId = statusChanged.Id;
                            travelingRecord.LastStatusChangeDate = System.DateTime.Now;
                            travelingRecord.ClosingDate = System.DateTime.Now;



                            recordRepo.UpdateTravelingRecord(travelingRecord);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.TravelingRecord, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Traveling Record having system ref #: " + travelingRecord.SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (travelingRecord != null)
                                {
                                    procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {

                            travelingRecord.PendingForClosing = true;
                            travelingRecord.stage = TransactionStage.AwaitingApproval.ToString();
                            travelingRecord.statusId = statusChanged.Id;
                            travelingRecord.LastStatusChangeDate = System.DateTime.Now;
                            travelingRecord.ClosingDate = System.DateTime.Now;



                            recordRepo.UpdateTravelingRecord(travelingRecord);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.TravelingRecord, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Traveling Record having system ref #: " + travelingRecord.SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                };
                                if (travelingRecord != null)
                                {
                                    procurementRepo.Add(travelingRecord.Id, TransactionItemType.TravelingRecord, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Traveling Record #" + travelingRecord.SystemRefNo, travelingRecord.Id, TransactionItemType.TravelingRecord, comment.Comment, user.id, "New Comment ", null);
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
                if (grdTravelingRecordRegister.GetFocusedRow() != null)
                {
                    recordRepo = new VisitingRecordRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (VisitingCountry)grdTravelingRecordRegister.GetFocusedRow();
                    var travelingRecord = recordRepo.GetTravelingRecord(selectedRow.TravelingRecordsId.Value);
                    UsersRepo usersRepo = new UsersRepo();

                    if (travelingRecord != null)
                    {
                        if (travelingRecord.isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Traveling Record") != null) ? true : false)
                            {

                                travelingRecord.isApproved = true;
                                travelingRecord.stage = TransactionStage.Approved.ToString();

                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, 25, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                recordRepo.UpdateTravelingRecord(travelingRecord);

                                //foreach (var _bill in loansAdvance)
                                //{
                                //    recordRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Traveling Record is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Traveling Record is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Traveling Record Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Traveling Record Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (travelingRecord.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Record without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Traveling Record") != null) ? true : false)
                            {
                                travelingRecord.isReApproved = true;
                                travelingRecord.stage = TransactionStage.Approved.ToString();


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.TravelingRecord, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                recordRepo.UpdateTravelingRecord(travelingRecord);
                                //foreach (var _bill in loansAdvance)
                                //{
                                //    recordRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Traveling Record is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Traveling Record is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Traveling Record Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Traveling Record Directly user id=(" + MainWindow.currentUserid + ")");
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Traveling Record List") != null)
            {
                recordRepo = new VisitingRecordRepo();
                lblHeading.Text = "Pending for Approval Traveling Records";
                var visitingCountries = recordRepo.GetAllPendingForApproval(SYSTEM_STATIC.currentUser.id);
                grdTravelingRecordRegister.ItemsSource = visitingCountries;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Traveling Records!");
            }
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Traveling Record List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Traveling Records";
                var visitingCountries = recordRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);
                grdTravelingRecordRegister.ItemsSource = visitingCountries;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Traveling Records!!");
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Traveling Record List") != null)
            {

                var visitingCountries = recordRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Traveling Records";
                grdTravelingRecordRegister.ItemsSource = visitingCountries;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Traveling Records!");
            }
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Traveling Record") != null)
            {
                recordRepo = new VisitingRecordRepo();
                var visitingCountries = recordRepo.GetAllTravelingRecords(SYSTEM_STATIC.currentUser.id);
                grdTravelingRecordRegister.ItemsSource = visitingCountries;
                lblHeading.Text = "Traveling Record Register";
            }
            else
            {
                MessageBox.Show("Permission required to View list of Traveling Records!");
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Traveling Record") != null)
            {
                lblHeading.Text = "Void Traveling Record";
                var visitingCountries = recordRepo.getVoidRegisterOwn(MainWindow.currentUserid);
                grdTravelingRecordRegister.ItemsSource = visitingCountries;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Traveling Records!!");
            }
        }
    }
}
