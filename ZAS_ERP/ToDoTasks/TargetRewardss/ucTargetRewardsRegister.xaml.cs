using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
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

namespace ZAS_ERP.ToDoTasks.TargetRewardss
{
    /// <summary>
    /// Interaction logic for ucTargetRewardsRegister.xaml
    /// </summary>
    public partial class ucTargetRewardsRegister : UserControl
    {
        ToDoTaskRepo toDoTaskRepo = new ToDoTaskRepo();
        public int ApprovalCount { get; set; } = 0;
        public int ReApprovalCount { get; set; } = 0;
        public int VoidCount { get; set; } = 0;
        public int ClosingCount { get; set; } = 0;
        public int ApproveunapprovedCount { get; set; } = 0;

        public ucTargetRewardsRegister()
        {
            this.DataContext = this;
            InitializeComponent();

            ApprovalCount = toDoTaskRepo.GetPendingForApprovalRewardsCount(SYSTEM_STATIC.currentUser.id);
            ReApprovalCount = toDoTaskRepo.GetPendingForReApprovalRewardsCount(SYSTEM_STATIC.currentUser.id);
            VoidCount = toDoTaskRepo.GetVoidRewardsCount(SYSTEM_STATIC.currentUser.id);
            ClosingCount = toDoTaskRepo.GetPendingForClosingCount(SYSTEM_STATIC.currentUser.id);
            ApproveunapprovedCount = toDoTaskRepo.GetRewardsRegisterCount(SYSTEM_STATIC.currentUser.id);
        }

        private void GrdRewards_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var reward = grdRewards.GetRowByListIndex(e.ListSourceRowIndex) as TargetRewards;
            var step = reward.toDoTask;
            if (e.IsGetData)
                switch (e.Column.FieldName)
                {
                    case "Stage":


                        if (reward.isVoid == true)
                        {
                            e.Value = "Void";
                            //lblStage.Text = "Void";
                        }
                        else if (reward.isReApproved == false)
                        {
                            e.Value = "Under Re-Approval";
                        }
                        else if (reward.isApproved == true && reward.stage == "Closed")
                        {
                            e.Value = "Approved and Closed";
                         
                        }
                        else if (reward.isApproved == true && reward.Status.isActive == false && reward.PendingForClosing != true)
                        {
                            e.Value = "Approved and Closed";
                        }
                        else if (reward.isApproved == true && reward.PendingForClosing == true)
                        {
                            e.Value = "Under Closing Approval";
                        }
                        else if (reward.isApproved == true)
                        {
                            e.Value = "Approved";
                        }
                        else if (reward.isApproved == false)
                        {
                            e.Value = "Under Approval";
                        }
                        else if (reward.PendingForClosing == true)
                        {
                            e.Value = "Under Closing Approval";
                        }
                        break;
                    //case "AchievedPercentage":

                    //    if (step.TaskPoints > 0)
                    //    {
                    //        decimal percentage = 0;
                    //        if (step.taskGroup.Companies != null && step.taskGroup.Companies.Count > 0 && step.taskGroup.Departments != null && step.taskGroup.Departments.Count > 0)
                    //            percentage = Math.Round((Convert.ToDecimal(step.AchievedPoints) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);
                    //        else
                    //            percentage = Math.Round((Convert.ToDecimal(step.AchievedPoints) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);
                    //        e.Value = percentage;
                    //    }
                    //    break;

                    case "StepYear":
                        if (reward != null && reward.toDoTask != null && reward.toDoTask.TargetYear != null)
                        {
                            e.Value = reward.toDoTask.TargetYear.Value.Year;
                        }
                        break;

                    case "StepMonth":
                        if (reward != null && reward.toDoTask != null && reward.toDoTask.TargetMonth != null)
                        {
                            string fullMonthName = reward.toDoTask.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("en"));
                            e.Value = fullMonthName;
                        }
                        break;
                    case "TargettYear":
                        if (reward != null && reward.toDoTask != null && reward.toDoTask.ParentTask != null && reward.toDoTask.ParentTask.TargetYear != null)
                        {
                            e.Value = reward.toDoTask.ParentTask.TargetYear.Value.Year;
                        }
                        break;

                    case "TargettMonth":
                        if (reward != null && reward.toDoTask != null && reward.toDoTask.ParentTask != null && reward.toDoTask.ParentTask.TargetMonth != null)
                        {
                            string fullMonthName = reward.toDoTask.ParentTask.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("en"));
                            e.Value = fullMonthName;
                        }
                        break;
                    case "Closed":
                        if (reward != null)
                        {
                            if (reward.Payments.Count > 0 && reward.Payments.Where(x => x.Status != null && x.Status.isActive == true).Count() == 0)
                            {
                                e.Value = "Closed";
                            }
                            else
                            {
                                if (reward.RewardAmount - reward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount) == 0)
                                    e.Value = "Paid";
                            }
                        }
                        break;

                    case "PaidAmount":
                        if (reward != null)
                        {
                            if (reward.Payments == null || reward.Payments.Where(x => x.isVoid != true) == null || reward.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                            {
                                e.Value = Convert.ToDecimal(0);
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(reward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount));
                            }
                        }
                        break;
                    case "UnPaidAmount":
                        if (reward != null)
                        {
                            if (reward.Payments == null || reward.Payments.Where(x => x.isVoid != true) == null || reward.Payments.Where(x => x.isVoid != true).ToList().Count == 0)
                            {
                                e.Value = Convert.ToDecimal(reward.RewardAmount);
                            }
                            else
                            {
                                e.Value = Convert.ToDecimal(reward.RewardAmount - reward.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount));
                            }
                        }
                        break;

                    case "SalesBudgetedMarginPercent":
                        if (step.TaskPoints > 0)
                        {
                            decimal percentage = 0;
                            if (step.TaskPoints != 0)
                                percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);

                            e.Value = percentage;
                        }
                        break;
                    case "SoAmountSERpercent":
                        if (step.TaskPoints > 0)
                        {
                            decimal percentage = 0;
                            if (step.SoAmountSER != 0)
                                percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.SoAmountSER)), 2);

                            e.Value = percentage;
                        }
                        break;

                    case "Companies":

                        if (step.taskGroup != null)
                        {
                            var companies = String.Join(" | ", step.taskGroup.Companies.Select(x => x.CompanyName));
                            e.Value = companies;
                        }
                        break;

                    case "Departments":

                        if (step.taskGroup != null)
                        {
                            var depts = String.Join(" | ", step.taskGroup.Departments.Select(x => x.DeptName));
                            e.Value = depts;
                        }
                        break;


                    case "StepLevel1":

                        if (step != null)
                        {
                            var groupList = new List<TargetGroup>();
                            var node = step.targetGroup;

                            while (node != null)
                            {
                                if (node.parentId != null)
                                {
                                    if (node.parentId != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        groupList.Add(node);
                                        node = node.targetGroup;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        groupList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    groupList.Add(node);
                                    break;
                                }

                            }
                            groupList.Reverse();
                            if (groupList.Count > 0)
                                e.Value = groupList[0].GroupName;
                        }

                        break;
                    case "StepLevel2":

                        if (step != null)
                        {
                            var groupList = new List<TargetGroup>();
                            var node = step.targetGroup;

                            while (node != null)
                            {
                                if (node.parentId != null)
                                {
                                    if (node.parentId != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        groupList.Add(node);
                                        node = node.targetGroup;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        groupList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    groupList.Add(node);
                                    break;
                                }

                            }

                            groupList.Reverse();

                            switch (groupList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = groupList[0].GroupName;
                                    break;
                                case 2:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 3:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 4:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 5:
                                    e.Value = groupList[1].GroupName;
                                    break;
                            }
                        }
                        break;

                    case "StepLevel3":

                        if (step != null)
                        {
                            var groupList = new List<TargetGroup>();
                            var node = step.targetGroup;

                            while (node != null)
                            {
                                if (node.parentId != null)
                                {
                                    if (node.parentId != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        groupList.Add(node);
                                        node = node.targetGroup;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        groupList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    groupList.Add(node);
                                    break;
                                }

                            }
                            groupList.Reverse();

                            switch (groupList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = groupList[0].GroupName;
                                    break;
                                case 2:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 3:
                                    e.Value = groupList[2].GroupName;
                                    break;
                                case 4:
                                    e.Value = groupList[2].GroupName;
                                    break;
                                case 5:
                                    e.Value = groupList[2].GroupName;
                                    break;
                            }
                        }

                        break;
                }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllUnAppliedRewards(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdRewards);
        }

        

        private void MbtnSaveLayout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdRewards);
        }

        private void MbtnRefresh_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            toDoTaskRepo = new ToDoTaskRepo();
            grdRewards.ItemsSource = toDoTaskRepo.GetAllUnAppliedRewards(SYSTEM_STATIC.currentUser.id);
        }

        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            if (grdRewards.SelectedItem != null)
            {
                var reward = grdRewards.SelectedItem as TargetRewards;

                if(reward.isApplied == false)
                {
                    ucFrmBasicTargetRewards ucFrmTarget = new ucFrmBasicTargetRewards();
                    ucFrmTarget.rewardId = reward.Id;

                    DXWindow win = new DXWindow();
                    win.WindowState = WindowState.Maximized;
                    win.Content = ucFrmTarget;
                    win.Title = "Target Reward";
                    win.Show();
                }
                else if(reward.isApplied == true)
                {
                    ucFrmTargetRewardAdd ucFrmTarget = new ucFrmTargetRewardAdd();
                    ucFrmTarget.rewardId = reward.Id;

                    DXWindow win = new DXWindow();
                    win.WindowState = WindowState.Maximized;
                    win.Content = ucFrmTarget;
                    win.Title = "Target Reward";
                    win.Show();
                }
                
            }

        }

        private void GrdRewards_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdRewards.SelectedItem != null)
            {
                var reward = grdRewards.SelectedItem as TargetRewards;
                if (reward.isApplied == false)
                {
                    ucFrmBasicTargetRewards ucFrmTarget = new ucFrmBasicTargetRewards();
                    ucFrmTarget.rewardId = reward.Id;

                    DXWindow win = new DXWindow();
                    win.WindowState = WindowState.Maximized;
                    win.Content = ucFrmTarget;
                    win.Title = "Target Reward";
                    win.Show();
                }
                else if (reward.isApplied == true)
                {
                    ucFrmTargetRewardAdd ucFrmTarget = new ucFrmTargetRewardAdd();
                    ucFrmTarget.rewardId = reward.Id;

                    DXWindow win = new DXWindow();
                    win.WindowState = WindowState.Maximized;
                    win.Content = ucFrmTarget;
                    win.Title = "Target Reward";
                    win.Show();
                }
            }
        }

        private void ChkClosedSO_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tableView.FormatConditions)
            {
                if (c.FieldName == "toDoTask.ClosedOrdersCount")
                    c.IsEnabled = true;
            }
        }

        private void ChkClosedSO_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tableView.FormatConditions)
            {
                if (c.FieldName == "toDoTask.ClosedOrdersCount")
                    c.IsEnabled = false;
            }
        }

        private void ChkApprovedSO_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tableView.FormatConditions)
            {
                if (c.FieldName == "toDoTask.OpenOrdersCount")
                    c.IsEnabled = false;
            }
        }

        private void ChkApprovedSO_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tableView.FormatConditions)
            {
                if (c.FieldName == "toDoTask.OpenOrdersCount")
                    c.IsEnabled = true;
            }

        }


        private void mbtnExportoReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Sale Orders Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblHeading.Content.ToString());
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdRewards, reportName, reportType, reportGroup, lblHeading.Content.ToString(), report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Sale Orders Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        public void loadcomments()
        {
            try
            {
                var targetReward = grdRewards.SelectedItem as TargetRewards;
                if (targetReward != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(targetReward.Id, TransactionItemType.TargetReward);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            var targetReward = grdRewards.SelectedItem as TargetRewards;
            if(targetReward != null)
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                if (targetReward != null)
                {
                    var taskGroup = targetReward.toDoTask.taskGroup;
                    if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.users, TransactionItemType.TargetReward);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }


                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && targetReward.Id != 0)
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                else
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Target Type: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, user.id, "New Comment ", null);
                            }
                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Target Reward with Target Type:" + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                                else
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Target Reward with Target Type:" + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            }
                        }

                        procurementRepo.Add(targetReward.Id, TransactionItemType.TargetReward, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                        //frmInputBox.taggedUsers = new List<User>();
                        loadcomments();
                        MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        if (window1 != null) { window1.UrgentNotificationGlow(); }
                    }
                    else if (targetReward.Id == 0)
                    {
                        DXMessageBox.Show("Kindly save Target Reward first to add a comment!");
                    }

                }
                loadcomments();
            }
            else
            {
                DXMessageBox.Show("Please select any row to Add Comment!");
            }
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var targetReward = grdRewards.SelectedItem as TargetRewards;
            if (targetReward != null)
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();

                if (grdCommentss.SelectedItem != null)
                {
                    var comment = grdCommentss.SelectedItem as CommentLog;
                    if (targetReward != null)
                    {
                        var taskGroup = targetReward.toDoTask.taskGroup;
                        if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                        {
                            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.users, comment, TransactionItemType.TargetReward);
                            inputBox.ShowDialog();
                        }
                        else
                        {
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                        }

                        if (targetReward != null)
                        {
                            ProcurementRepo procurementRepo = new ProcurementRepo();

                            if (frmInputBox.commentAdded == true && targetReward.Id != 0)
                            {

                                var commentId = procurementRepo.AddCommentLinkNotification(targetReward.Id, TransactionItemType.TargetReward, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                                if (commentId != null)
                                {
                                    foreach (var user in frmInputBox.Comment.TaggedList)
                                    {
                                        if (frmInputBox.FlagForTag == true)
                                            notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                        else
                                            notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                    }

                                    foreach (var user in frmInputBox.Comment.CCUsersList)
                                    {
                                        if (frmInputBox.FlagForCC == true)
                                            notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                        else
                                            notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target Reward with Reward Amount: " + targetReward.RewardAmount, targetReward.Id, TransactionItemType.TargetReward, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                    }
                                }


                                MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                                if (window1 != null) { window1.UrgentNotificationGlow(); }
                            }
                            else if (targetReward.Id == 0)
                            {
                                DXMessageBox.Show("Kindly save Target Reward first to add a comment!");
                            }
                        }
                        loadcomments();
                    }
                    else
                    {
                        DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                    }
                }
            }
        }

        private void MbtnVoid_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllVoidRewards(SYSTEM_STATIC.currentUser.id);
            lblHeading.Content = "Void Rewards";
        }

        private void MbtnRewardRegister_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllUnAppliedRewards(SYSTEM_STATIC.currentUser.id);
            lblHeading.Content = "All Unapplied Rewards";
        }

        private void MbtnAchieved_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllAchievedRewards(SYSTEM_STATIC.currentUser.id);
            lblHeading.Content = "Achieved Rewards";
        }

        private void MbtnSalesRewards_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Sales Target Rewards") != null )
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllSalesRewards(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Sales Rewards";
            }
            else
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllSalesRewards(SYSTEM_STATIC.currentUser.id).Where(x=>x.user_Id == SYSTEM_STATIC.currentUser.id).ToList();
                lblHeading.Content = "Sales Rewards";
            }
        }

        private void MbtnFinanceRewards_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Finance Target Rewards") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllFinanceRewards(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Finance Rewards";
            }
            else
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllFinanceRewards(SYSTEM_STATIC.currentUser.id).Where(x => x.user_Id == SYSTEM_STATIC.currentUser.id).ToList();
                lblHeading.Content = "Finance Rewards";
            }
            
        }

        private void MbtnOtherRewards_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Other Target Rewards") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllOtherRewards(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Other Rewards";
            }
            else
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllOtherRewards(SYSTEM_STATIC.currentUser.id).Where(x => x.user_Id == SYSTEM_STATIC.currentUser.id).ToList();
                lblHeading.Content = "Other Rewards";
            }
        }

        private void MbtnAppliedRewardRegister_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllAppliedRewards(SYSTEM_STATIC.currentUser.id);
            lblAppliedHeading.Content = "All Applied Rewards";
        }

        private void MbtnAppliedAchieved_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllAppliedAchievedRewards(SYSTEM_STATIC.currentUser.id);
            lblAppliedHeading.Content = "Achieved Rewards";
        }

        private void MbtnAppliedSalesRewards_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Sales Target Rewards") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllAppliedSalesRewards(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Sales Rewards";
            }
            else
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllAppliedSalesRewards(SYSTEM_STATIC.currentUser.id).Where(x => x.user_Id == SYSTEM_STATIC.currentUser.id).ToList();
                lblHeading.Content = "Sales Rewards";
            }
        }

        private void MbtnAppliedFinanceRewards_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Finance Target Rewards") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllAppliedFinanceRewards(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Finance Rewards";
            }
            else
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllAppliedFinanceRewards(SYSTEM_STATIC.currentUser.id).Where(x => x.user_Id == SYSTEM_STATIC.currentUser.id).ToList();
                lblHeading.Content = "Finance Rewards";
            }
        }

        private void MbtnAppliedOtherRewards_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View All Other Target Rewards") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllAppliedOtherRewards(SYSTEM_STATIC.currentUser.id);
                lblHeading.Content = "Other Rewards";
            }
            else
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllAppliedOtherRewards(SYSTEM_STATIC.currentUser.id).Where(x => x.user_Id == SYSTEM_STATIC.currentUser.id).ToList();
                lblHeading.Content = "Other Rewards";
            }
        }

        private void DXTabControl_SelectionChanged(object sender, TabControlSelectionChangedEventArgs e)
        {
            if(tabUnApplied.IsSelected == true)
            {
                if(grdRewards != null)
                {
                    grdRewards.ItemsSource = toDoTaskRepo.GetAllUnAppliedRewards(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Content = "All Unapplied Rewards";
                }
                
            }
            else if (tabApplied.IsSelected == true)
            {
                if (grdRewards != null)
                {
                    grdRewards.ItemsSource = toDoTaskRepo.GetAllAppliedRewards(SYSTEM_STATIC.currentUser.id);
                    lblAppliedHeading.Content = "All Applied Rewards";
                }
                
            }
        }

        static TargetRewardStatus statusChanged = new TargetRewardStatus();
        TargetRewardStatus oldStatus = new TargetRewardStatus();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucTargetRewardsRegister(TargetRewardStatus targetRewardStatus)
        {
            statusChanged = targetRewardStatus;
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Target Reward List") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetPendingForApprovalRewards(SYSTEM_STATIC.currentUser.id);
                lblAppliedHeading.Content = "Pending for Approval Rewards";
            }
            else
            {
                DXMessageBox.Show("Permission Required to View Pending for Approval Target Rewards!");
            }
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
           
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Target Reward List") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetPendingForReApprovalRewards(SYSTEM_STATIC.currentUser.id);
                lblAppliedHeading.Content = "Pending for ReApproval Rewards";
            }
            else
            {
                DXMessageBox.Show("Permission Required to View Pending for ReApproval Target Rewards!");
            }
        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Target Reward List") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetPendingForClosing(SYSTEM_STATIC.currentUser.id);
                lblAppliedHeading.Content = "Pending for Closing Rewards";
            }
            else
            {
                DXMessageBox.Show("Permission Required to View Pending for Closing Target Rewards!");
            }
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Reward Register") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetAllRewardsForRegister(SYSTEM_STATIC.currentUser.id);
                lblAppliedHeading.Content = "Reward Register";
            }
            else
            {
                DXMessageBox.Show("Permission Required to View Target Reward Register!");
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Target Rewards") != null)
            {
                grdRewards.ItemsSource = toDoTaskRepo.GetVoidRewards(SYSTEM_STATIC.currentUser.id);
                lblAppliedHeading.Content = "Void Rewards";
            }
            else
            {
                DXMessageBox.Show("Permission Required to Void Target Rewards!");
            }
        }
    }
}
