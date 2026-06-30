using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Reports;
using ERP_BL.ToDoTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;
using ZAS_ERP.ToDoTasks.TargetRewardss;
using ZAS_ERP.ToDoTasks.UserControls;

namespace ZAS_ERP.ToDoTasks.Windows
{
    /// <summary>
    /// Interaction logic for winTargetRegisterReport.xaml
    /// </summary>
    public partial class winTargetRegisterReport : DXWindow
    {
        GridReport report = new GridReport();

        public MainWindow myParent = null;
        string reportTitle;
        ToDoTaskRepo toDoTaskRepo = new ToDoTaskRepo();
      
 
        public winTargetRegisterReport()
        {
            InitializeComponent();
        }
        public winTargetRegisterReport(GridReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGroupData();
        }

        private void MbtnRefreshReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {

                grdRewards.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
            }
            else
            {
            }
            grdRewards.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }


        private void MbtnAll_Click(object sender, RoutedEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllRewards(SYSTEM_STATIC.currentUser.id);
            lblHeading.Content = "All Rewards";
        }

        private void MbtnAchieved_Click(object sender, RoutedEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllAchievedRewards(SYSTEM_STATIC.currentUser.id);
            lblHeading.Content = "Achieved Rewards";
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
                    case "CalculationTypee":
                        if (step != null && step.calculationType != null)
                        {
                            e.Value = step.calculationType.TypeName;
                        }
                        else if (step != null && step.taskGroup != null && step.taskGroup.calculationType != null)
                        {
                            e.Value = step.taskGroup.calculationType.TypeName;
                        }
                        break;
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
                            if (step.targetGroup == null)
                            {
                                if (step.taskGroup != null && step.taskGroup.targetGroup != null)
                                {
                                    var groupList = new List<TargetGroup>();
                                    var node = step.taskGroup.targetGroup;

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

                            }
                            else
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

                        }
                        break;
                    case "StepLevel2":

                        if (step != null)
                        {
                            if (step.targetGroup == null)
                            {
                                if (step.taskGroup != null && step.taskGroup.targetGroup != null)
                                {
                                    var groupList = new List<TargetGroup>();
                                    var node = step.taskGroup.targetGroup;

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
                            }
                            else
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

                        }
                        break;

                    case "StepLevel3":

                        if (step != null)
                        {
                            if (step.targetGroup == null)
                            {
                                if (step.taskGroup != null && step.taskGroup.targetGroup != null)
                                {
                                    var groupList = new List<TargetGroup>();
                                    var node = step.taskGroup.targetGroup;

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
                            }
                            else
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

                        }
                        break;
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


        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {

        }

      
        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdRewards.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = report.reportName;
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = report.reportName;
            link.ReportHeaderData = report.reportName;
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }
        private void MbtnSaveAsNew1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Save as new Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {

                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var saveAsNewReport = setReportName.report;
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.gridReportType != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    GridReportRepo repo = new GridReportRepo();
                                    bool isExist = repo.GetReportByNameAndUserId(report);
                                    if (isExist)
                                    {
                                        DXMessageBox.Show("( " + reportName + " ) is already exist you cannot duplicate name", "Invalid Name !", MessageBoxButton.OK, MessageBoxImage.Information);
                                        return;

                                    }
                                    else
                                    {
                                        ReportLogic.SaveGridReport(grdRewards, reportName, reportType, reportGroup, report.settingkey, report.titleId);
                                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to save as new Standard Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
                case GridReportType.MemorizedReport:
                    {

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Save as new Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var saveAsNewReport = setReportName.report;
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.gridReportType != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    ReportLogic.SaveGridReport(grdRewards, reportName, reportType, reportGroup, report.settingkey, report.titleId);
                                    DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to save as new Memorized Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
            }

        }

        private void MbtnRenameReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to rename " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.report = report;
                                setReportName.editableGroup = group;
                                //_setting input Fields
                                setReportName.txtName.Text = report.reportName;
                                //setReportName.LoadReportTypes();
                                setReportName.GetEnum();
                                setReportName.reportTypes.EditValue = report.gridReportType;
                                setReportName.lookupGroup.EditValue = group.groupName;
                                setReportName.LoadGroups();
                                setReportName.ShowDialog();
                                var updatedReport = setReportName.report;
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdRewards, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        this.Close();
                                    }
                                    else
                                        DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Rename Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.report = report;
                                setReportName.editableGroup = group;
                                //setting input Fields
                                setReportName.txtName.Text = report.reportName;
                                //setReportName.LoadReportTypes();
                                setReportName.GetEnum();
                                setReportName.reportTypes.EditValue = report.gridReportType;
                                setReportName.lookupGroup.EditValue = group.groupName;
                                setReportName.LoadGroups();
                                setReportName.ShowDialog();
                                var updatedReport = setReportName.report;
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdRewards, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        this.Close();
                                    }
                                    else
                                        DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                            }
                            else
                                return;
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }

            }
        }

        private void MbtnUpdateReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.Title + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                string str = report.reportName;
                                GridReportType reportType = report.gridReportType;
                                GridReportGroup groupDetails = group;
                                if (str != "")
                                {
                                    ReportLogic.UpdateGridReport(grdRewards, str, reportType, groupDetails, report.Id, report.settingkey);

                                    DXMessageBox.Show(" ( " + str + " ) is Updated Successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                return;
                            }
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Update Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                string str = report.reportName;
                                GridReportType reportType = report.gridReportType;
                                GridReportGroup groupDetails = group;
                                if (str != "")
                                {
                                    ReportLogic.UpdateGridReport(grdRewards, str, reportType, groupDetails, report.Id, report.settingkey);

                                    DXMessageBox.Show(" ( " + str + " ) is Updated Successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                return;
                            }
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    break;
            }

        }

        private void MbtnDeleteReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to delete " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                /*GridReport report = repo.GetReportByName(this.Title)*/
                                ;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                ReportLogic.DeleteReport(grdRewards, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Delete Standard report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        break;
                    }

                case GridReportType.MemorizedReport:
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                /*GridReport report = repo.GetReportByName(this.Title)*/
                                ;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                ReportLogic.DeleteReport(grdRewards, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Delete Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        break;
                    }

            }
        }

      
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }
        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {

            toDoTaskRepo = new ToDoTaskRepo();
            //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdRewards);

            grdRewards.ItemsSource = toDoTaskRepo.GetAllRewards(SYSTEM_STATIC.currentUser.id);
            grdRewards.ShowLoadingPanel = false;
        }

        private void MbtnExportToMemorizedReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Memorized Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Export to Memorized Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var type = report.gridReportType;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var exportToMemorizedReport = setReportName.report;
                                if (exportToMemorizedReport != null && exportToMemorizedReport.gridReportGroup != null && exportToMemorizedReport.gridReportType != null && exportToMemorizedReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToMemorizedReport.gridReportGroup;
                                    var reportType = exportToMemorizedReport.gridReportType;
                                    var reportName = exportToMemorizedReport.reportName;
                                    ReportLogic.SaveGridReport(grdRewards, reportName, reportType, reportGroup, report.settingkey, report.titleId);
                                    DXMessageBox.Show("( " + reportName + " ) is exported to Memorized reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }

                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }

            }
        }

        private void MbtnExportToStandardReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Standard Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


            switch (report.gridReportType)
            {
                case GridReportType.MemorizedReport:
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Export to Standard Report") != null)
                        {
                            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                            {
                                var repo = new GridReportRepo();
                                //var report = repo.GetReportByName(this.Title);
                                var type = report.gridReportType;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var exportToStandardReport = setReportName.report;
                                if (exportToStandardReport != null && exportToStandardReport.gridReportGroup != null && exportToStandardReport.gridReportType != null && exportToStandardReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToStandardReport.gridReportGroup;
                                    var reportType = exportToStandardReport.gridReportType;
                                    var reportName = exportToStandardReport.reportName;
                                    ReportLogic.ExportToStandard(grdRewards, reportName, reportType, reportGroup, report.settingkey);
                                    DXMessageBox.Show("( " + reportName + " ) is exported to Standard reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                                }

                            }
                            else
                                return;

                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        break;
                    }
            }
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Target Reward Report") != null)
            {

                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to share " + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    SelectSharedGroup selectSharedGroup = new SelectSharedGroup(report);
                    selectSharedGroup.ShowDialog();
                }
                else
                {
                    return;
                }
            }
            else
            {
                DXMessageBox.Show("You don't have permission Share Target Reward Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }
        private void LoadGroupData()
        {
            try
            {
                GridReportRepo repo = new GridReportRepo();
                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                toDoTaskRepo = new ToDoTaskRepo();
                var tasks = toDoTaskRepo.GetAllRewards(SYSTEM_STATIC.currentUser.id);
                grdRewards.ItemsSource = tasks;
                switch (report.gridReportType)
                {
                    case GridReportType.StandardReport:
                        {
                            if (report != null)
                            {
                                //mbtnExportToStandardReport1.IsVisible = false;
                                mbtnExportToStandardReport1.IsVisible = false;
                                mbtnShareReport.IsVisible = false;
                                grdRewards.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                                Title = "Reward Register" + "/" + reportTitle;
                                lblHeading.Content = "Reward Register" + "/" + reportTitle;
                                lblHeader.Caption = "Reward Register" + "/" + reportTitle;
                            }
                            break;
                        }
                    case GridReportType.MemorizedReport:
                        {
                            if (report != null)
                            {
                                mbtnExportToMemorizedReport1.IsVisible = false;
                                mbtnShareReport.IsVisible = true;
                                grdRewards.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                                Title = "Reward Register" + "/" + reportTitle;
                                lblHeading.Content = "Reward Register" + "/" + reportTitle;
                                lblHeader.Caption = "Reward Register" + "/" + reportTitle;


                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void MbtnClosed_Click(object sender, RoutedEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllClosedRewards(SYSTEM_STATIC.currentUser.id);
            lblHeading.Content = "Closed Rewards";
        }

        private void MbtnOpen_Click(object sender, RoutedEventArgs e)
        {
            grdRewards.ItemsSource = toDoTaskRepo.GetAllOpenRewards(SYSTEM_STATIC.currentUser.id);
            lblHeading.Content = "Open Rewards";
        }

        private void MenuItemLinkedSaleorders_Click(object sender, RoutedEventArgs e)
        {
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            TimeSpan ts = new TimeSpan(23, 59, 59);
            var targetReward = grdRewards.SelectedItem as TargetRewards;

            var selectedItem = targetReward.toDoTask;

            ToDoTaskRepo repo = new ToDoTaskRepo();
            if (selectedItem != null && selectedItem.Id > 0)
            {
                if (selectedItem.searchedFromDate != null && selectedItem.searchedToDate != null)
                {
                    if (selectedItem.taskGroup.CompaniesBulk != null && selectedItem.taskGroup.CompaniesBulk.Count() > 0 && selectedItem.taskGroup.DepartmentsBulk != null && selectedItem.taskGroup.DepartmentsBulk.Count() > 0)
                    {
                        var toDate = selectedItem.searchedToDate.Value;

                        toDate = toDate.Date + ts;

                        saleOrders = repo.getDepartmentalSOByDates(selectedItem.taskGroup.CompaniesBulk, selectedItem.taskGroup.DepartmentsBulk, (DateTime)selectedItem.searchedFromDate, toDate);
                    }
                    else
                    {
                        var toDate = selectedItem.searchedToDate.Value;

                        toDate = toDate.Date + ts;
                        saleOrders = repo.getDepartmentalSOByDates(selectedItem.taskGroup.Companies, selectedItem.taskGroup.Departments, (DateTime)selectedItem.searchedFromDate, toDate);
                    }
                }
            }

            ucTargetsSaleOrderList targetsSaleOrderList = new ucTargetsSaleOrderList();
            targetsSaleOrderList.saleOrders = saleOrders;
            Window window = new Window();
            window.Content = targetsSaleOrderList;
            window.WindowState = WindowState.Maximized;
            window.Show();
        }

        private void MenuItemVisibleTargetsSaleorders_Click(object sender, RoutedEventArgs e)
        {
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            TimeSpan ts = new TimeSpan(23, 59, 59);
            for (int i = 0; i < grdRewards.VisibleRowCount; i++)
            {
                //var _row = grdCntrlStepsForRegister.GetRow(i);
                int rowHandle = grdRewards.GetRowHandleByVisibleIndex(i);
                var _row = grdRewards.GetRow(rowHandle);
                var targetReward = _row as TargetRewards;
                var _step = targetReward.toDoTask;

                ToDoTaskRepo repo = new ToDoTaskRepo();
                if (rowHandle >= 0 && _step != null && _step.Id > 0)
                {
                    if (_step.searchedFromDate != null && _step.searchedToDate != null /*&&  !String.IsNullOrEmpty(_step.SOField)*/)
                    {
                        if (_step.taskGroup.CompaniesBulk != null && _step.taskGroup.CompaniesBulk.Count() > 0 && _step.taskGroup.DepartmentsBulk != null && _step.taskGroup.DepartmentsBulk.Count() > 0)
                        {
                            var toDate = _step.searchedToDate.Value;

                            toDate = toDate.Date + ts;

                            saleOrders = repo.getDepartmentalSOByDates(_step.taskGroup.CompaniesBulk, _step.taskGroup.DepartmentsBulk, (DateTime)_step.searchedFromDate, toDate);
                        }
                        else
                        {
                            var toDate = _step.searchedToDate.Value;

                            toDate = toDate.Date + ts;
                            saleOrders.AddRange(repo.getDepartmentalSOByDates(_step.taskGroup.Companies, _step.taskGroup.Departments, (DateTime)_step.searchedFromDate, toDate));
                        }
                    }
                }
            }

            saleOrders = saleOrders.Distinct().ToList();

            ucTargetsSaleOrderList targetsSaleOrderList = new ucTargetsSaleOrderList();
            targetsSaleOrderList.saleOrders = saleOrders;
            Window window = new Window();
            window.Content = targetsSaleOrderList;
            window.WindowState = WindowState.Maximized;
            window.Show();
        }

    }
}
