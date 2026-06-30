using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Documents;
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

namespace ZAS_ERP.FilesAndDocss.Documentss
{
    /// <summary>
    /// Interaction logic for ucDocumentList.xaml
    /// </summary>
    public partial class ucDocumentList : UserControl
    {
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        DocumentRepo documentRepo = new DocumentRepo();
        public ucDocumentList()
        {
            this.DataContext = this;
            InitializeComponent();

            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Document List") != null)
                {
                    ApprovalCount = documentRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Document without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Document") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Document") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Document") != null)
                    {
                        ApprovalCount = documentRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = documentRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Document List") != null)
                {
                    ReApprovalCount = documentRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Document without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Document") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Document") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Document") != null)
                    {
                        ReApprovalCount = documentRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = documentRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Document") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = documentRepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount = documentRepo.getVoidRegisterAdministratorCount();

                    }
                }
                else
                {
                    mbtnVoid.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Documents") != null)
                {
                    //mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for new added Inter-Bank Transfer") != null ||
                    //    SystemLogic.AllowedPermissions.Find(x => x.Name == "Close Inter-Bank Transfer without Approval") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Approver for Closing Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Inter-Bank Transfer") != null || SystemLogic.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Inter-Bank Transfer") != null)
                    //{
                    ApproveunapprovedCount = documentRepo.getBankTransferRegisterCount(MainWindow.currentUserid);
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
                ApprovalCount = documentRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = documentRepo.getInterBankTransferAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Document List") != null)
                {
                    ClosingCount = documentRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Document without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Document") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Document") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Document") != null)
                    {
                        ClosingCount = documentRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = documentRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }
            else
            {
                ClosingCount = documentRepo.getAllPendingForClosingAdministratorCount();
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdDocumentsRegister.ItemsSource = documentRepo.GetAllActiveDocuments(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdDocumentsRegister);
        }

        public void GridControlSetUserSettings()
        {
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdDocumentsRegister);
        }

        private void grdDocumentsRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = grdDocumentsRegister.SelectedItem as Document;
            if (selectedItem != null)
            {
                    
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Document") != null)
                                {
                                    ucFrmDocumentAdd frmDocuments = new ucFrmDocumentAdd();
                                    frmDocuments.groupId = selectedItem.transactionGroupId;
                                    frmDocuments.editFlag = true;
                                    Window win = new Window();
                                    win.Content = frmDocuments;
                                    win.WindowState = WindowState.Maximized;
                                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    win.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                               

                            
                      

                   
                
            }
        }

        private void grdDocumentsRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "employeee":
                    var item = grdDocumentsRegister.GetRowByListIndex(e.ListSourceRowIndex) as Document;
                    if (item.employee != null)
                        e.Value = item.employee.person.FullName;
                    else
                        e.Value = item.Employee;
                    break;
            }
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Document") != null)
            {
                ucFrmDocumentAdd frmDocuments = new ucFrmDocumentAdd();
                frmDocuments.editFlag = false;
                Window win = new Window();
                win.Content = frmDocuments;
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Document") != null)
            {
                var selectedItem = grdDocumentsRegister.SelectedItem as Document;
                if (selectedItem != null)
                {
                    ucFrmDocumentAdd frmDocuments = new ucFrmDocumentAdd();
                    frmDocuments.groupId = selectedItem.transactionGroupId;
                    frmDocuments.editFlag = true;
                    Window win = new Window();
                    win.Content = frmDocuments;
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Documents Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Documents") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("Documents");
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdDocumentsRegister, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Documents Reports!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ucDocumentList documentsList = new ucDocumentList();
            this.DataContext = documentsList;
            documentRepo = new DocumentRepo();
            grdDocumentsRegister.ItemsSource = documentRepo.GetAllActiveDocuments(SYSTEM_STATIC.currentUser.id);
            lblHeading.Text = "Documents";
        }

        private void mbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdDocumentsRegister);
        }


        static DocumentStatus statusChanged = new DocumentStatus();
        DocumentStatus oldStatus = new DocumentStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucDocumentList(DocumentStatus documentStatus)
        {
            statusChanged = documentStatus;
        }
        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            documentRepo = new DocumentRepo();
            var selectedRow = (Document)grdDocumentsRegister.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var documents = documentRepo.GetDocumentsByGroupId(selectedRow.transactionGroupId);
                if (documents != null && documents.Count > 0)
                {
                    if (documents[0].isApproved == false)
                    {
                        DXMessageBox.Show("Documents are pending for approval!");
                        return;
                    }

                    statusChanged = null;
                    ucFrmDocumentDirectClose ucFrmDirectClose = new ucFrmDocumentDirectClose();
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Document") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Document without Approval") != null)
                        {
                            for (int i = 0; i < documents.Count; i++)
                            {
                                documents[i].PendingForClosing = false;
                                documents[i].stage = TransactionStage.Closed.ToString();
                                documents[i].status_Id = statusChanged.Id;
                                documents[i].LastStatusChangeDate = System.DateTime.Now;
                                documents[i].ClosingDate = System.DateTime.Now;


                            }
                            documentRepo.UpdateDocument(documents);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Document, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Document having system ref #: " + documents[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers,
                                    TaggedRecomenndedList = tagUsersRecommendation,
                                    CCRecomenndedList = ccUsersRecommendation
                                };
                                if (documents.Count != 0)
                                {
                                    procurementRepo.Add(documents[0].transactionGroupId, TransactionItemType.Document, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {
                            for (int i = 0; i < documents.Count; i++)
                            {
                                documents[i].PendingForClosing = true;
                                documents[i].stage = TransactionStage.AwaitingApproval.ToString();
                                documents[i].status_Id = statusChanged.Id;
                                documents[i].LastStatusChangeDate = System.DateTime.Now;
                                documents[i].ClosingDate = System.DateTime.Now;


                            }
                            documentRepo.UpdateDocument(documents);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Document, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Documents having system ref #: " + documents[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:",
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers,
                                    TaggedRecomenndedList = tagUsersRecommendation,
                                    CCRecomenndedList = ccUsersRecommendation
                                };
                                if (documents.Count != 0)
                                {
                                    procurementRepo.Add(documents[0].transactionGroupId, TransactionItemType.Document, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Document #" + documents[0].SystemRefNo, documents[0].transactionGroupId, TransactionItemType.Document, comment.Comment, user.id, "New Comment ", null);
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
                if (grdDocumentsRegister.GetFocusedRow() != null)
                {
                    documentRepo = new DocumentRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (Document)grdDocumentsRegister.GetFocusedRow();
                    var documents = documentRepo.GetDocumentsByGroupId(selectedRow.transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (documents != null && documents.Count > 0)
                    {
                        if (documents[0].isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null) ? true : false)
                            {
                                for (int i = 0; i < documents.Count; i++)
                                {
                                    documents[i].isApproved = true;
                                    documents[i].stage = TransactionStage.Approved.ToString();
                                }
                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, (int)TransactionItemType.Document, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                documentRepo.UpdateDocument(documents);

                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Admin Bills are Approved (" + selectedRow.transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Admin Bill is Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Admin Bill Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Admin Bill Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (documents[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Admin Bill") != null) ? true : false)
                            {
                                for (int i = 0; i < documents.Count; i++)
                                {
                                    documents[i].isReApproved = true;
                                    documents[i].stage = TransactionStage.Approved.ToString();

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                documentRepo.UpdateDocument(documents);
                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Admin Bills are Approved (" + selectedRow.transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Admin Bill is Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Admin Bill Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Admin Bill Directly user id=(" + MainWindow.currentUserid + ")");
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Document List") != null)
            {
                documentRepo = new DocumentRepo();
                lblHeading.Text = "Pending for Approval Documents";
                var documentList = documentRepo.GetAllPendingForApproval(SYSTEM_STATIC.currentUser.id);
                grdDocumentsRegister.ItemsSource = documentList;
            }
            else
            {
                MessageBox.Show("Permission Required to view Pending for Approval Documents!!");
            }
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Document List") != null)
            {
                lblHeading.Text = "Pending for ReApprovals Documents";
                var documents = documentRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);


                grdDocumentsRegister.ItemsSource = documents;
            }
            else
            {
                MessageBox.Show("Permission Required to View Pending For Re-Approval Documents!!");
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Document List") != null)
            {

                var documents = documentRepo.getAllPendingForClosingDepartmental(SYSTEM_STATIC.currentUser.id);
                lblHeading.Text = "(Pending for Closing) Documents";
                grdDocumentsRegister.ItemsSource = documents;
            }
            else
            {
                MessageBox.Show("Permission required to View Pending for closing Documents!");
            }
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Documents") != null)
            {
                documentRepo = new DocumentRepo();
                var documentList = documentRepo.GetAllDocuments(SYSTEM_STATIC.currentUser.id);
                grdDocumentsRegister.ItemsSource = documentList;
                lblHeading.Text = "Documents Register";
            }
            else
            {
                MessageBox.Show("Permission required to View list of Documents!");
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Documents") != null)
            {
                lblHeading.Text = "Void Documents";

                var documentList = documentRepo.getVoidRegisterOwn(MainWindow.currentUserid);

                grdDocumentsRegister.ItemsSource = documentList;
            }
            else
            {
                MessageBox.Show("Permission Required to view Void Admin Bills!!");
            }
        }

    }
}
