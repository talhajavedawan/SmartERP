using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.Budget;
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

namespace ZAS_ERP.Procurementss.Budget.UserControls
{
    /// <summary>
    /// Interaction logic for ucBudgetGrid.xaml
    /// </summary>
    public partial class ucBudgetGrid : UserControl
    {
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
        public int ApprovalCount { get; set; }
        public int ReApprovalCount { get; set; }
        public int VoidCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }
        public static int statusId;
        public static int AllActive;
        List<BudgetCostSheet> budgets = new List<BudgetCostSheet>();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        Department department = new Department();
        Company company = new Company();

        public ucBudgetGrid()
        {
            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Budget List") != null)
                {
                    ApprovalCount = repo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                    {
                        ApprovalCount = repo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = repo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Budget List") != null)
                {
                    ReApprovalCount = repo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                    {
                        ReApprovalCount = repo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = repo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Budget") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;
                    if (MainWindow.currentUserid != 0)
                    {
                        VoidCount = repo.getVoidRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        VoidCount = repo.getVoidRegisterAdministratorCount();
                    }
                }
                else
                {
                    mbtnVoid.Visibility = Visibility.Collapsed;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Budget Register") != null)
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                    {
                        ApproveunapprovedCount = repo.getBudgetRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApproveunapprovedCount = repo.getBudgetRegisterCountOWn(MainWindow.currentUserid);
                    }
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ApprovalCount = repo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = repo.getBudgetRegisterAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Budget List") != null)
                {
                    ClosingCount = repo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                    {
                        ClosingCount = repo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = repo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);
                    }
                }
            }
            else
            {
                ClosingCount = repo.getAllPendingForClosingAdministratorCount();
            }
        }

        private void grdbudget_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmBudgetAdd frmBudget = new frmBudgetAdd((grdbudget.SelectedItem as BudgetCostSheet).Id);
            frmBudget.Show();
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdbudget.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            BudgetCostSheetStatus oldStatus = new BudgetCostSheetStatus();
            var row = grdbudget.SelectedItem as BudgetCostSheet;
            if (row != null)
            {
                var budgetid = row.Id;
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget without Approval") != null) ? true : false)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    row = repo.get(budgetid);
                    if (row.budgetCostSheetStatus != null)
                    {
                        oldStatus = row.budgetCostSheetStatus;
                    }
                    Procurementss.Budget.UserControls.ucStatusChange.inActiveStatuses = 1;

                    Procurementss.Budget.UserControls.ucStatusChange.budgetid = (int)budgetid;

                    Procurementss.Budget.frmBudgetSatatusChange statusChange = new frmBudgetSatatusChange(repo);
                    var myWindow = Window.GetWindow(this);
                    statusChange.Owner = myWindow;
                    statusChange.ShowDialog();
                    if (Procurementss.Budget.UserControls.ucStatusChange.sheet.Id != 0)

                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Budget") != null) ? true : false)
                        {
                            Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing = false;
                            Procurementss.Budget.UserControls.ucStatusChange.sheet.stage = TransactionStage.Approved.ToString();

                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.budgetCostSheetStatus.Status + ") to (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, Procurementss.Budget.UserControls.ucStatusChange.sheet.Id, 3, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                        {
                            Procurementss.Budget.UserControls.ucStatusChange.sheet.stage = TransactionStage.AwaitingApproval.ToString();
                            if (Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing == null)
                            {
                                Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.budgetCostSheetStatus.Status + ") to (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, Procurementss.Budget.UserControls.ucStatusChange.sheet.Id, 3, frmInputBox.comment);
                        }

                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null)
                        {
                            Procurementss.Budget.UserControls.ucStatusChange.sheet.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing != true)
                            {
                                Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing = true;

                            }
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.budgetCostSheetStatus.Status + ") to (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, Procurementss.Budget.UserControls.ucStatusChange.sheet.Id, 3, frmInputBox.comment);
                        }
                        else
                        {
                            Procurementss.Budget.UserControls.ucStatusChange.sheet.stage = TransactionStage.AwaitingFirstReview.ToString();

                            Procurementss.Budget.UserControls.ucStatusChange.sheet.PendingForClosing = true;
                            usersRepo.Add(TransactionInfo.Closed, Procurementss.Budget.UserControls.ucStatusChange.sheet.Id, 3, frmInputBox.comment);
                        }
                    Procurementss.Budget.UserControls.ucStatusChange.sheet.LastStatusChangeDate = System.DateTime.Now;
                    Procurementss.Budget.UserControls.ucStatusChange.sheet.ClosingDate = System.DateTime.Now;
                    if (row.budgetCostSheetStatus != Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus)
                        usersRepo.Add(TransactionInfo.Status_Changed, row.Id, (int)TransactionItemType.Budget, "While direct closing Status Changed from (" + row.budgetCostSheetStatus.Status + ") to (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Budget has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (row.Department != null && row.Department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.Department.Id, row.company.Id), row.Id, TransactionItemType.Budget);
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
                    string newStat = Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status;
                    string symbolCurr = "";
                    if (row.Currency != null)
                    {
                        symbolCurr = row.Currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Status of Budget having Budget Net Profit: " + row.TotalNetProfit.ToString() + " (" + symbolCurr + ")\n "
                            + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                        Timestamp = DateTime.Now,
                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };

                    procurementRepo.Add(row.Id, TransactionItemType.Budget, comment, SYSTEM_STATIC.currentUser.employeeId);

                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Budget #" + row.refNo, row.Id, TransactionItemType.Budget, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Budget #" + row.refNo, row.Id, TransactionItemType.Budget, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                    try
                    {
                        row = Procurementss.Budget.UserControls.ucStatusChange.sheet;
                        repo.updateStatusById(row.Id, row.budgetCostSheetStatus.Id);
                    }
                    catch { }
                    MessageBox.Show("Budget status changed to InActive (" + Procurementss.Budget.UserControls.ucStatusChange.sheet.budgetCostSheetStatus.Status + ")");
                    var thisWindow = Window.GetWindow(this);
                    thisWindow.Close();
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Close Budget Directly.");
                }
            }
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var row = grdbudget.SelectedItem as BudgetCostSheet;

               
                    if (row != null)
                    {

                        repo = new BudgetCostCenterRepo();
                        BudgetCostSheet sheet = new BudgetCostSheet();
                        sheet = repo.get(row.Id);
                        var department = sheet.Department;
                        var company = sheet.company;
                        UsersRepo usersRepo = new UsersRepo();

                        if (sheet != null)
                        {
                            if (sheet.isApproved == true)
                            {

                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null) ? true : false)
                                {
                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res = MessageBox.Show("Budget is Approved, Do you want to UnApprove this Budget?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                    if (res == MessageBoxResult.Yes)
                                    {

                                        sheet.isApproved = false;
                                        sheet.stage = TransactionStage.AwaitingApproval.ToString();
                                        repo.Approve(sheet);
                                        var res1 = MessageBox.Show("Budget has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                        if (res1 == MessageBoxResult.Yes)
                                        {
                                            if (sheet.Department != null && sheet.Department.Id != 0 && sheet.company?.Id != 0)
                                            {
                                                winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id }), sheet.Id, TransactionItemType.Budget);
                                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            }
                                            else if (sheet.Department != null && sheet.Department.Id != 0 && sheet.company?.Id != 0)
                                            {
                                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment( department.Id, company.Id), sheet.Id, TransactionItemType.Budget);
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

                                        string symbolCurr = "";
                                        if (sheet.Currency != null)
                                        {
                                            symbolCurr = sheet.Currency.Abbrivation.ToString();
                                        }
                                        CommentLog comment = new CommentLog();



                                        comment.Comment = "Budget (Net Budget Profit) having value: " + sheet.TotalNetProfit.ToString() + "(" + symbolCurr + ") " + " has been UnApproved";
                                        comment.Timestamp = DateTime.Now;
                                        comment.Subject = "Budget UnApproved";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;


                                        procurementRepo.Add(sheet.Id, TransactionItemType.Budget, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating Comments
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, 0, user.id, "New Comment ", null);
                                            }
                                        }
                                        MessageBox.Show("Budget are UnApproved (" + sheet.refNo + ")");
                                        SystemLog.LogInfo(this.GetType(), "Budget is UnApproved (" + sheet.Id + ")");
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("You are not Allowed to Approve Budget Directly");
                                    SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Budget Directly user id=(" + MainWindow.currentUserid + ")");
                                    return;
                                }

                            }
                            else if (sheet.isApproved == false)
                            {

                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null) ? true : false)
                                {
                                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                                    ProcurementRepo procurementRepo = new ProcurementRepo();
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res = MessageBox.Show("Budget are Pending for Approval, Do you want to Approve this Budget?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                    if (res == MessageBoxResult.Yes)
                                    {

                                        sheet.isApproved = true;
                                        sheet.stage = TransactionStage.Approved.ToString();
                                        repo.Approve(sheet);

                                        var res1 = MessageBox.Show("Budget has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                        if (res1 == MessageBoxResult.Yes)
                                        {
                                            if (sheet.Department != null && sheet.Department.Id != 0 && sheet.company?.Id != 0)
                                            {
                                                winTagUsers win = new winTagUsers(userRepo.getusersByDepartmentIdsList(new List<int> { department.Id }, new List<int> { company.Id }), sheet.Id, TransactionItemType.Budget);
                                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                            }
                                            else if (sheet.Department != null && sheet.Department.Id != 0 && sheet.company?.Id != 0)
                                            {
                                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(department.Id, company.Id), sheet.Id, TransactionItemType.Budget);
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
                                        string symbolCurr = "";
                                        if (sheet.Currency != null)
                                        {
                                            symbolCurr = sheet.Currency.Abbrivation.ToString();
                                        }
                                        CommentLog comment = new CommentLog();
                                        comment.Comment = "Budget (Net Budget Profit) having value: " + sheet.TotalNetProfit.ToString() + "(" + symbolCurr + ") " + " has been UnApproved";
                                        comment.Timestamp = DateTime.Now;
                                        comment.Subject = "Budget UnApproved";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;
                                        procurementRepo.Add(sheet.Id, TransactionItemType.Budget, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Budget #" + sheet.refNo, sheet.Id, TransactionItemType.Budget, comment.Comment, 0, user.id, "New Comment ", null);
                                            }
                                        }
                                        MessageBox.Show("Budget is Approved (" + sheet.refNo + ")");
                                        SystemLog.LogInfo(this.GetType(), "Budget is Approved (" + sheet.Id + ")");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You are not Allowed to Approve Budget Directly");
                                    SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Budget Directly user id=(" + MainWindow.currentUserid + ")");
                                    return;
                                }
                            }
                            var myWindow = Window.GetWindow(this);
                            myWindow.Close();
                        }
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
            lblHeading.Text = "(Pending for Approval) Budget";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Budget List") != null)
                {

                    //saleOrders = saleOrderrepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);

                    budgets = repo.getAllFirstPendingForApprovalDepartmental(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                    {
                        budgets = repo.getAllFirstPendingForApproval(MainWindow.currentUserid);
                    }
                    else
                    {
                        budgets = repo.getAllFirstPendingForApprovalOwn(MainWindow.currentUserid);
                    }
                }
            else
                budgets = repo.getAllFirstPendingForAdministrator();

            grdbudget.ItemsSource = budgets;
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
            lblHeading.Text = "Pending for ReApprovals Budget";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Budget List") != null)
                {

                    budgets = repo.getAllFirstPendingForReApprovalDepartmental(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                    {

                        budgets = repo.getAllFirstPendingForReApproval(MainWindow.currentUserid);
                    }

                    else
                    {

                        budgets = repo.getAllFirstPendingForReApprovalOwn(MainWindow.currentUserid);

                    }
                }
            else

                budgets = repo.getAllFirstPendingForAdministrator();
            grdbudget.ItemsSource = budgets;
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
            lblHeading.Text = "(Pending for Closing) Budgets";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Budget List") != null)
                {

                    budgets = repo.getAllFirstPendingForClosingDepartmental(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                    {

                        budgets = repo.getAllFirstPendingForClosing(MainWindow.currentUserid);
                    }

                    else
                    {

                        budgets = repo.getAllFirstPendingForClosingOwn(MainWindow.currentUserid);

                    }
                }
            else

                budgets = repo.getAllFirstPendingForClosingAdministrator();
            grdbudget.ItemsSource = budgets;


            AllActive = 4;
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
            lblHeading.Text = "Budget Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                {
                    budgets = repo.getFirstSaleRegister(MainWindow.currentUserid);
                }
                else
                {
                    //All bills instead of own bills
                    //saleOrders = saleOrderrepo.getSaleRegisterOwn(MainWindow.currentUserid);
                    budgets = repo.getFirstSaleRegister(MainWindow.currentUserid);
                }
            else
            {
                //budgets = repo.getSaleRegisterAdministrator();
            }
            grdbudget.ItemsSource = budgets;
            statusId = 0;
            AllActive = 6;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdbudget);

        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            lblHeading.Text = "Void Budgets";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Budget") != null)
                {
                    budgets = repo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    budgets = repo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                budgets = repo.getVoidRegisterAdministrator();
            grdbudget.ItemsSource = budgets;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void btnNewBudget_Click(object sender, RoutedEventArgs e)
        {
            frmBudgetAdd budget = new frmBudgetAdd();
            budget.Show();
        }

        private void btnEditBudget_Click(object sender, RoutedEventArgs e)
        {
            frmBudgetAdd frmBudget = new frmBudgetAdd((grdbudget.SelectedItem as BudgetCostSheet).Id);
            frmBudget.Show();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dateFrom.EditValue = DateTime.Now.AddMonths(-2);
            dateTo.EditValue = DateTime.Now;
            loadBudgetGrid();
        
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void loadBudgetGrid()
        {

            lblHeading.Text = "Budgets";
            if (lblHeading.Text == "Budgets(Open)" || lblHeading.Text == "Budgets(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        budgets = repo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Budget") != null)
                    {

                        budgets = repo.getFirstAll(MainWindow.currentUserid);
                    }
                    else
                    {
                        budgets = repo.getAllFirstActive(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    budgets = repo.getAllFirstActive(MainWindow.currentUserid);
                }
                else if (AllActive == 2)
                {
                    budgets = repo.getAllFirstInActive(MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending For Approvals (Budgets)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Budget List") != null)
                        {

                            budgets = repo.getAllFirstPendingForApprovalDepartmental(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                            {
                                budgets = repo.getAllFirstPendingForApproval(MainWindow.currentUserid);
                            }
                            else
                            {
                                budgets = repo.getAllFirstPendingForApprovalOwn(MainWindow.currentUserid);
                            }
                        }
                    else

                        budgets = repo.getAllFirstPendingForAdministrator();
                }
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Budgets";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Budget List") != null)
                        {
                            budgets = repo.getAllFirstPendingForClosingDepartmental(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                            {
                                budgets = repo.getAllFirstPendingForClosing(MainWindow.currentUserid);
                            }
                            else
                            {
                                budgets = repo.getAllFirstPendingForClosingOwn(MainWindow.currentUserid);
                            }
                        }
                    else
                        budgets = repo.getAllFirstPendingForClosingAdministrator();
                    grdbudget.ItemsSource = budgets;
                }
            }
            else
            {
                budgets = repo.getAllFirstPobyStatusId(MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdbudget);
            grdbudget.ItemsSource = budgets;
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ucBudgetGrid budgetGrid = new ucBudgetGrid();
            this.Content = budgetGrid;
        }

        private void BtnDateFilter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (dateFrom.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateFrom.Focus();

                return;
            }
            else
                if (dateTo.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateTo.Focus();

                return;

            }
            else
            {

                LoadOrdersByDate();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });

            }
        }
        private void LoadOrdersByDate()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Budgets(Open)" || lblHeading.Text == "Budgets(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        budgets = repo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Budget") != null)
                    {
                        repo = new BudgetCostCenterRepo();
                        budgets = repo.ActiveInActiveSoByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                    }
                    else
                    {
                        budgets = repo.AllActivebyDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);


                    }
                }
                else if (AllActive == 1)
                {
                    budgets = repo.AllActivebyDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);


                }
                else if (AllActive == 2)
                {
                    budgets = repo.AllInActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);


                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending For Approvals (Budgets)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Budget List") != null)
                        {
                            //saleOrders = saleOrderrepo.getAllPendingForApprovalDepartmental(MainWindow.currentUserid);
                            budgets = repo.AllPendingForApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                            {
                                budgets = repo.AllPendingForApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                            else
                            {
                                budgets = repo.AllPendingForApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    else
                    {
                        budgets = repo.AllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                    }
                }
                if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Budgets";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Budget List") != null)
                        {
                            budgets = repo.PendingForClosingDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                            {
                                budgets = repo.AllPendingForClosingByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);


                            }
                            else
                            {
                                budgets = repo.AllPendingForClosingOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    else
                    {
                        budgets = repo.AllPendingForClosingAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);


                    }
                }
                else
                if (AllActive == 5)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending for ReApprovals Budgets";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Budget List") != null)
                        {
                            budgets = repo.PendingForReApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                            {
                                budgets = repo.PendingForReApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }

                            else
                            {
                                budgets = repo.PendingForReApprovalOwnByDateRamge((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    else
                    {
                        budgets = repo.AllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);

                    }

                    AllActive = 5;
                }
                else
                    if (AllActive == 6)
                {

                    UsersRepo usersRepo = new UsersRepo();
                    lblHeading.Text = "Budget Register";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Budget") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Budget") != null)
                        {
                            budgets = repo.BudgetRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            //All bills instead of own bills
                            //saleOrders = saleOrderrepo.getSaleRegisterOwn(MainWindow.currentUserid);
                            budgets = repo.BudgetRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                    else
                        //budgets = repo.getSaleRegisterAdministrator();
                    grdbudget.ItemsSource = budgets;
                    statusId = 0;
                    AllActive = 6;
                }
            }
            else
            {
                budgets = repo.AllPobyStatusIdByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid, statusId);

            }
            grdbudget.ItemsSource = budgets.Distinct();
            grdbudget.RefreshData();
        }
    }
}
