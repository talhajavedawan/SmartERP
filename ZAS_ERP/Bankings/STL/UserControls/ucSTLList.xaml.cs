using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.Procurementss;
using ZAS_ERP.Reportss;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ZAS_ERP.Bankings.STL.UserControls
{
    /// <summary>
    /// Interaction logic for ucSTLList.xaml
    /// </summary>
    public partial class ucSTLList : UserControl
    {
        public int ApprovalCount { get; set; }
        public int ReApprovalCount { get; set; }
        public int VoidCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }
        STLRepo stlRepo = new STLRepo();
        public static int statusId;
        public static int AllActive;
        List<ERP_BL.Procurements.InterBankTransfers.STL> stls = new List<ERP_BL.Procurements.InterBankTransfers.STL>();

        public ucSTLList()
        {
            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) STL List") != null)
                {
                    ApprovalCount = stlRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                    //ApprovalCount = stlRepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                    {
                        ApprovalCount = stlRepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = stlRepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) STL List") != null)
                {
                    ReApprovalCount = stlRepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                    {
                        ReApprovalCount = stlRepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = stlRepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void STL") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;
                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = stlRepo.getVoidRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        VoidCount = stlRepo.getVoidRegisterAdministratorCount();
                    }
                }
                else
                {
                    mbtnVoid.Visibility = Visibility.Collapsed;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL Register") != null)
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added STL") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                    {
                        ApproveunapprovedCount = stlRepo.getSaleRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApproveunapprovedCount = stlRepo.getSaleRegisterCountOWn(MainWindow.currentUserid);
                    }
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }

            }

            else
            {
                ApprovalCount = stlRepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = stlRepo.getSaleRegisterAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) STL List") != null)
                {
                    ClosingCount = stlRepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                    {
                        ClosingCount = stlRepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = stlRepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }
            else
            {
                ClosingCount = stlRepo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export STL Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName("STL");
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdSTLRegister, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export STL Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdSTLRegister);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                STLStatus oldStatus = new STLStatus();

                var stl = grdSTLRegister.SelectedItem as ERP_BL.Procurements.InterBankTransfers.STL;

                var stlId = stl.Id;
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL without Approval") != null) ? true : false)
                {
                    double total = 0;
                    total = Math.Round(stl.paymentAmountOC, 2);
                    UsersRepo usersRepo = new UsersRepo();
                    stl = stlRepo.Get(stl.Id);
                    var row = stl;
                    if (row.stlStatus != null)
                    {
                        oldStatus = row.stlStatus;
                    }
                    ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.inActiveStatuses = 1;

                    ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stlid = (int)row.Id;
                    ZAS_ERP.Bankings.STL.Windows.frmSTLStatusChange statusChange = new ZAS_ERP.Bankings.STL.Windows.frmSTLStatusChange(stlRepo);
                    var myWindow = System.Windows.Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id != 0)

                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing STL") != null) ? true : false)
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing = false;
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.stlStatus.Status + ") to (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id, 32, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stage = TransactionStage.AwaitingApproval.ToString();
                            if (ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing == null)
                            {
                                ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.stlStatus.Status + ") to (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id, 32, frmInputBox.comment);
                        }

                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null)
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing != true)
                            {
                                ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.stlStatus.Status + ") to (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id, 32, frmInputBox.comment);
                        }
                        else
                        {
                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stage = TransactionStage.AwaitingFirstReview.ToString();

                            ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.PendingForClosing = true;
                            usersRepo.Add(TransactionInfo.Closed, ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.Id, 32, frmInputBox.comment);
                        }
                    ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.LastStatusChangeDate = System.DateTime.Now;
                    ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.ClosingDate = System.DateTime.Now;
                    if (row.stlStatus != ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus)
                        usersRepo.Add(TransactionInfo.Status_Changed, stl.Id, (int)TransactionItemType.STL, "While direct closing Status Changed from (" + row.stlStatus.Status + ") to (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus + ")");
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("STL has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), stl.Id, TransactionItemType.STL);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();

                        }

                    }
                    string oldStat = "";
                    if (oldStatus != null)
                    {
                        oldStat = oldStatus.Status;
                    }
                    string newStat = ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status;
                    string paymentsymbolCurr = "", cashMarginSymbol = "", STLSymbol = "";
                    if (row.paymentCurrency != null)
                    {
                        paymentsymbolCurr = row.paymentCurrency.Abbrivation.ToString();
                        STLSymbol = row.stlCurrency.Abbrivation.ToString();
                        //cashMarginSymbol = row.cashMarginCurrency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of STL having Payment Amount (OC): " + row.paymentAmountOC.ToString() + " (" + paymentsymbolCurr + ")\n "
                            + "STL Payment(OC): " + row.stlPaymentAmountOC.ToString() + " (" + STLSymbol + ")"
                            + "\nCash Margin(OC): " + row.cashMarginAmount.ToString() + " (" + cashMarginSymbol + ")"
                            + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };

                    procurementRepo.Add(row.Id, TransactionItemType.STL, comment, SYSTEM_STATIC.currentUser.employeeId);

                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in STL #" /*+ row.SalesReferenceNo*/, row.Id, TransactionItemType.STL, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {


                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in STL " /*+ row.SalesReferenceNo*/, row.Id, TransactionItemType.STL, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                    try
                    {
                        row = ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl;

                        stlRepo.updateStatusById(row.Id, row.stlStatus);
                    }
                    catch { }
                    MessageBox.Show("STL status changed to InActive (" + ZAS_ERP.Bankings.STL.UserControls.ucSTLStatusChange.stl.stlStatus.Status + ")");


                    var thisWindow = System.Windows.Window.GetWindow(this);
                    thisWindow.Close();
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Close STL Directly.");
                }
            }
            catch (Exception)
            {


            }
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                   var stl = grdSTLRegister.SelectedItem as ERP_BL.Procurements.InterBankTransfers.STL;

                 
                    UsersRepo usersRepo = new UsersRepo();

                    if (stl != null)
                    {
                        if (stl.isApproved == true)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added STL") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("STL is Approved, Do you want to UnApprove this STL?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    stl.isApproved = false;
                                    stl.stage = TransactionStage.AwaitingApproval.ToString();
                                    stlRepo.Approve(stl);
                                    var res1 = MessageBox.Show("STL has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(usersRepo.getusersByDepartmentIdsList(new List<int> { stl.department.Id }, new List<int> { stl.company.Id }), stl.Id, TransactionItemType.STL);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(stl.department.Id, stl.company.Id), stl.Id, TransactionItemType.STL);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }

                                    string paymentsymbolCurr = "", STLSymbol = "", cashMarginSymbol = "";
                                    if (stl.paymentCurrency != null)
                                    {
                                        paymentsymbolCurr = stl.paymentCurrency.Abbrivation.ToString();
                                        STLSymbol = stl.stlCurrency.Abbrivation.ToString();
                                        //cashMarginSymbol = stl.cashMarginCurrency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog();
                                    {

                                        comment.Comment = "STL having Payment Amount (OC): " + stl.paymentAmountOC.ToString() + " (" + paymentsymbolCurr + ")\n "
                                                + "STL Payment(OC): " + stl.stlPaymentAmountOC.ToString() + " (" + STLSymbol + ")"
                                                + "\nCash Margin(OC): " + stl.cashMarginAmount.ToString() + " (" + cashMarginSymbol + ")" +
                                                "\n has been UnApproved";
                                        comment.Timestamp = DateTime.Now;
                                        comment.Subject = "STL UnApproved";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;

                                    }
                                    procurementRepo.Add(stl.Id, TransactionItemType.STL, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" /*+ stl.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" /*+ saleOrder.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("STL are UnApproved");
                                    SystemLog.LogInfo(this.GetType(), "STL is UnApproved (" + stl.Id + ")");
                                }

                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve STL Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve STL Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (stl.isApproved == false)
                        {

                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added STL") != null) ? true : false)
                            {
                                NotificationsRepo notificationsRepo = new NotificationsRepo();
                                ProcurementRepo procurementRepo = new ProcurementRepo();
                                UsersRepo userRepo = new UsersRepo();
                                //Asking for Tag
                                List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                var res = MessageBox.Show("STL are Pending for Approval, Do you want to Approve this STL?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (res == MessageBoxResult.Yes)
                                {

                                    stl.isApproved = true;
                                    stl.stage = TransactionStage.Approved.ToString();
                                    stlRepo.Approve(stl);

                                    var res1 = MessageBox.Show("STL has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res1 == MessageBoxResult.Yes)
                                    {
                                        if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { stl.department.Id }, new List<int> { stl.company.Id }), stl.Id, TransactionItemType.STL);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else if (stl.department != null && stl.department.Id != 0 && stl.company?.Id != 0)
                                        {
                                            winTagUsers win = new winTagUsers(usersRepo.getusersByCompanyDepartment(stl.department.Id, stl.company.Id), stl.Id, TransactionItemType.STL);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }

                                    }

                                    string paymentsymbolCurr = "", STLSymbol = "", cashMarginSymbol = "";
                                    if (stl.paymentCurrency != null)
                                    {
                                        paymentsymbolCurr = stl.paymentCurrency.Abbrivation.ToString();
                                        STLSymbol = stl.stlCurrency.Abbrivation.ToString();
                                        //cashMarginSymbol = stl.cashMarginCurrency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "STL having Payment Amount (OC): " + stl.paymentAmountOC.ToString() + " (" + paymentsymbolCurr + ")\n "
                                                + "STL Payment(OC): " + stl.stlPaymentAmountOC.ToString() + " (" + STLSymbol + ")"
                                                + "\nCash Margin(OC): " + stl.cashMarginAmount.ToString() + " (" + cashMarginSymbol + ")" +
                                                "\n has been Approved",
                                        Timestamp = DateTime.Now,
                                        Subject = "STL Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(stl.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" /*+ stl.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }
                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in STL #" /*+ stl.SalesReferenceNo*/, stl.Id, TransactionItemType.STL, comment.Comment, 0, user.id, "New Comment ", null);
                                        }
                                    }
                                    MessageBox.Show("STL is Approved");
                                    SystemLog.LogInfo(this.GetType(), "STL is Approved (" + stl.Id + ")");
                                }
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve STL Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve STL Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }
                        }
                        var myWindow = System.Windows.Window.GetWindow(this);
                        myWindow.Close();
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            statusId = 0;
            lblHeading.Text = "Pending STL";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) STL List") != null)
                {
                    stls = stlRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                    {
                        stls = stlRepo.getAllPendingForApproval(MainWindow.currentUserid);
                    }
                    else
                    {
                        stls = stlRepo.getAllPendingForApprovalOwn(MainWindow.currentUserid);
                    }
                }
            else
                stls = stlRepo.getAllPendingForAdministrator();
                grdSTLRegister.ItemsSource = stls;
                AllActive = 3;
                statusId = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();
            statusId = 0;
            AllActive = 5;
            lblHeading.Text = "Pending for ReApprovals STL";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) STL List") != null)
                {
                    stls = stlRepo.getAllPendingForReApprovalDepartmental(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                    {
                        stls = stlRepo.getAllPendingForReApproval(MainWindow.currentUserid);
                    }
                    else
                    {
                        stls = stlRepo.getAllPendingForReApprovalOwn(MainWindow.currentUserid);
                    }
                }
            else
                stls = stlRepo.getAllPendingForAdministrator();
            grdSTLRegister.ItemsSource = stls;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();
            lblHeading.Text = "STL Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                {
                    stls = stlRepo.getSTLRegister(MainWindow.currentUserid);
                }
                else
                {
                    stls = stlRepo.getSTLRegister(MainWindow.currentUserid);
                }
            else
                stls = stlRepo.getSTLAdministrator();
            grdSTLRegister.ItemsSource = stls;
            statusId = 0;
            AllActive = 6;
            mbtnExportToReport.IsEnabled = true;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void grdSTLRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                var stl = grdSTLRegister.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Procurements.InterBankTransfers.STL;
                if (e.Column.FieldName == "InterestAmountIAC" && e.IsGetData)
                {
                    double paymentAmount = 0, interestPerc = 0, utilizeDays = 0, creditTenure = 0;
                    DateTime start = (DateTime)stl.paymentDate;
                    DateTime end = (DateTime)stl.paymentMaturityDate;
                    DateTime calUtilizedDate = DateTime.Now;
                    var stamp = calUtilizedDate - start;
                    utilizeDays = stamp.Days;
                    if (stl.creditTenureNo != 0 && stl.interest != null && stl.stlPaymentDate!=null && stl.paymentMaturityDate!=null)
                    {
                        creditTenure = Math.Round(stl.creditTenureNo + stl.extendedCreditTenureNo);
                        paymentAmount = Math.Round(stl.paymentAmountSTL);
                        interestPerc = Math.Round(Convert.ToDouble(stl.interest.percentage), 2);
                        var value1 = paymentAmount * interestPerc;
                        var value2 = value1 / 100;
                        var value3 = value2 / 365;
                        var value4 = value3 * utilizeDays;
                        e.Value = (Math.Round(value4, 2)).ToString();
                    }
                }
                 if (e.Column.FieldName == "stlUtilizedDays" && e.IsGetData)
                {
                    double utilizeDays = 0;

                    DateTime start = (DateTime)stl.paymentDate;
                    DateTime end = (DateTime)stl.paymentMaturityDate;
                    DateTime calUtilizedDate = DateTime.Now;
                    var stamp = calUtilizedDate - start;
                    utilizeDays = stamp.Days;
                    e.Value = utilizeDays;
                    
                }
                if (e.Column.FieldName == "paymentDueDays1" && e.IsGetData)
                {
                    double utilizeDays = 0;
                    DateTime start = (DateTime)stl.stlPaymentDate;
                    DateTime end = (DateTime)stl.paymentMaturityDate;
                    DateTime calUtilizedDate = DateTime.Now;
                    var stamp = calUtilizedDate - start;
                    utilizeDays = stamp.Days;
                    var value = Convert.ToDouble(stl.creditTenureNo) + Convert.ToDouble(stl.extendedCreditTenureNo);
                    e.Value = Convert.ToDouble(utilizeDays) - value;


                }
            }
            catch(Exception ex)
            {

            }
        }

        private void grdSTLRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL") != null)
                {

                    var selectedStl = grdSTLRegister.SelectedItem as ERP_BL.Procurements.InterBankTransfers.STL;
                    winSTLAdd stl = new winSTLAdd(true, Convert.ToInt32(selectedStl.paymentGroupId));
                    stl.stl = selectedStl;
                    stl.Show();
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View STL");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View STL") != null)
            {

                var selectedStl = grdSTLRegister.SelectedItem as ERP_BL.Procurements.InterBankTransfers.STL;
                if (selectedStl != null)
                {
                    winSTLAdd stl = new winSTLAdd(true, Convert.ToInt32(selectedStl.paymentGroupId));
                    stl.stl = selectedStl;
                    stl.Show();
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View STL");
            }
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            PrintableControlLink link = new PrintableControlLink((TableView)grdSTLRegister.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);

        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadSTLGrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            statusId = 0;
            lblHeading.Text = "(Pending for Closing) STL";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) STL List") != null)
                {
                    stls = stlRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                    {

                        stls = stlRepo.getAllPendingForClosing(MainWindow.currentUserid);
                    }

                    else
                    {

                        stls = stlRepo.getAllPendingForClosingOwn(MainWindow.currentUserid);

                    }
                }
            else
                stls = stlRepo.getAllPendingForClosingAdministrator();
            grdSTLRegister.ItemsSource = stls;
            AllActive = 4;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            lblHeading.Text = "Void STL";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void STL") != null)
                {
                    stls = stlRepo.getVoidRegister(MainWindow.currentUserid);
                }
                else
                {
                    stls = stlRepo.getVoidRegisterOwn(MainWindow.currentUserid);
                }
            else
                stls = stlRepo.getVoidRegisterAdministrator();
            grdSTLRegister.ItemsSource = stls;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSTLGrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        public void LoadSTLGrid()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "STL(Open)" || lblHeading.Text == "STL(Closed)")
            { mbtnExportToReport.IsEnabled = false; }

            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        stls = stlRepo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive STL") != null)
                    {
                        stls = stlRepo.getAll(MainWindow.currentUserid);
                    }
                    else
                    {
                        stls = stlRepo.getAllActive(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    stls = stlRepo.getAllActive(MainWindow.currentUserid);
                }
                else if (AllActive == 2)
                {
                    stls = stlRepo.getAllInActive(MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    lblHeading.Text = "Pending For Approvals (STL)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) STL List") != null)
                        {
                            stls = stlRepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                            {
                                stls = stlRepo.getAllPendingForApproval(MainWindow.currentUserid);
                            }
                            else
                            {
                                stls = stlRepo.getAllPendingForApprovalOwn(MainWindow.currentUserid);
                            }
                        }
                    else
                        stls = stlRepo.getAllPendingForAdministrator();
                }
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) STL";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) STL List") != null)
                        {
                            stls = stlRepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close STL without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 STL") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 STL") != null)
                            {
                                stls = stlRepo.getAllPendingForClosing(MainWindow.currentUserid);
                            }
                            else
                            {
                                stls = stlRepo.getAllPendingForClosingOwn(MainWindow.currentUserid);
                            }
                        }
                    else
                        stls = stlRepo.getAllPendingForClosingAdministrator();
                    grdSTLRegister.ItemsSource = stls;
                    grdSTLRegister.Columns["CreationDate"].VisibleIndex = 0;
                }
            }
            else
            {
                stls = stlRepo.getAllbyStatusId(MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSTLRegister);
            grdSTLRegister.ItemsSource = stls;
        }
    }
}
