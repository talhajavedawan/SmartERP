using DevExpress.Data;
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
using System.Windows.Threading;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;
using ZAS_ERP.ToDoTasks.UserControls;

namespace ZAS_ERP.ToDoTasks.Windows
{
    /// <summary>
    /// Interaction logic for ucTodoTaskReport.xaml
    /// </summary>
    public partial class ucTodoTaskReport : DXWindow
    {
        GridReport report = new GridReport();

        public MainWindow myParent = null;
        string reportTitle;
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        List<TaskGroups> TaskGroups = new List<TaskGroups>();

        public ucTodoTaskReport()
        {
            InitializeComponent();
        }
        public ucTodoTaskReport(GridReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }
        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdCntrlStepsForRegister.View);
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
                                        ReportLogic.SaveGridReport(grdCntrlStepsForRegister, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.SaveGridReport(grdCntrlStepsForRegister, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                        ReportLogic.RenameGridReport(grdCntrlStepsForRegister, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                        ReportLogic.RenameGridReport(grdCntrlStepsForRegister, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                    ReportLogic.UpdateGridReport(grdCntrlStepsForRegister, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdCntrlStepsForRegister, str, reportType, groupDetails, report.Id, report.settingkey);

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

                                ReportLogic.DeleteReport(grdCntrlStepsForRegister, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                                ReportLogic.DeleteReport(grdCntrlStepsForRegister, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

        private void MbtnRefreshReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {

                grdCntrlStepsForRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
            }
            else
            {
            }
            grdCntrlStepsForRegister.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }
        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {

            taskRepo = new ToDoTaskRepo();
            var tasks = taskRepo.GetAllSteps(SYSTEM_STATIC.currentUser.id);
            grdCntrlStepsForRegister.ItemsSource = tasks;
            grdCntrlStepsForRegister.ShowLoadingPanel = false;
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
                                    ReportLogic.SaveGridReport(grdCntrlStepsForRegister, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.ExportToStandard(grdCntrlStepsForRegister, reportName, reportType, reportGroup, report.settingkey);
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Todo Task Report") != null)
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
                DXMessageBox.Show("You don't have permission Share Todo Task Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGroupData();

        }
        private void LoadGroupData()
        {
            try
            {
                GridReportRepo repo = new GridReportRepo();
                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                taskRepo = new ToDoTaskRepo();
                var tasks = taskRepo.GetAllSteps(SYSTEM_STATIC.currentUser.id);
                grdCntrlStepsForRegister.ItemsSource = tasks;
                switch (report.gridReportType)
                    {
                        case GridReportType.StandardReport:
                            {
                                if (report != null)
                                {
                                    //mbtnExportToStandardReport1.IsVisible = false;
                                    mbtnExportToStandardReport1.IsVisible = false;
                                    mbtnShareReport.IsVisible = false;
                                    grdCntrlStepsForRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                                    Title = "Step Register" + "/" + reportTitle;
                                    lblHeading.Caption = "Step Register" +"/" + reportTitle;
                                    btnFav.IsEnabled = false;

                            }
                            break;
                            }
                        case GridReportType.MemorizedReport:
                            {
                                if (report != null)
                                {
                                    mbtnExportToMemorizedReport1.IsVisible = false;
                                mbtnShareReport.IsVisible = true;
                                grdCntrlStepsForRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                                    Title = "Step Register" + "/" + reportTitle;
                                    lblHeading.Caption = "Step Register" + "/" + reportTitle;
                            }
                                break;
                            }
                    }

                RefreshSystemPoints();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void GrdCntrlToDoTaskList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateTask();
        }

      
        private void UpdateTask()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target") != null)
            {
                var task = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
                if (task != null)
                {
                    ucFrmAddTask frmAddTask = new ucFrmAddTask();
                    frmAddTask.taskId = task.Id;
                    frmAddTask.taskGroupId = task.taskGroupId.Value;
                    frmAddTask.editFlag = true;

                    Window win = new Window();
                    win.Content = frmAddTask;
                    win.Title = "Targets";
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("You do not have permission to View Target!");
                return;
            }
        }

        private void GrdCntrlSteps_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            var step = grdCntrlStepsForRegister.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
            if (e.IsGetData)
                switch (e.Column.FieldName)
                {
                    case "Stage":
                    //var todo = grdCntrlStepsForRegister.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
                    //if (todo.isVoid == true)
                    //{
                    //    e.Value = "Void";
                    //}
                    //else if (todo.isReApproved == false)
                    //{
                    //    e.Value = "Under Approval";
                    //}
                    //else if (todo.isApproved == true && todo.stage == "Closed")
                    //{
                    //    e.Value = "Closed";
                    //}
                    //else if (todo.isApproved == true && todo.Status.isActive == false )
                    //{
                    //    e.Value = "Closed";
                    //}

                    //else if (todo.isApproved == true)
                    //{
                    //    e.Value = "Approved";
                    //}
                    //else if (todo.isApproved == false)
                    //{
                    //    e.Value = "Under Approval";
                    //}
                    //break;
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
                        if (step != null && step.TargetYear != null)
                        {
                            e.Value = step.TargetYear.Value.Year;
                        }
                        break;

                    case "StepMonth":
                        if (step != null && step.TargetMonth != null)
                        {
                            e.Value = step.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("en"));
                        }
                        break;
                    case "TargettYear":
                        if (step != null && step.ParentTask != null && step.ParentTask.TargetYear != null)
                        {
                            e.Value = step.ParentTask.TargetYear.Value.Year;
                        }
                        break;

                    case "TargettMonth":
                        if (step != null && step.ParentTask != null && step.ParentTask.TargetMonth != null)
                        {
                            e.Value = step.ParentTask.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("en"));
                        }
                        break;
                    case "SalesBudgetedMarginPercent":
                        if (step.TaskPoints > 0)
                        {
                            decimal percentage = 0;
                            if (step.TaskPoints != 0)
                            {
                                percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);
                            }
                            e.Value = percentage;
                        }
                        break;
                    case "SoAmountSERpercent":
                        if (step.TaskPoints > 0)
                        {
                            decimal percentage = 0;
                            if (step.SoAmountSER != 0)
                            {
                                percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.SoAmountSER)), 2);

                            }

                            e.Value = percentage;
                        }
                        break;

                    case "Companies":

                        if (step.taskGroup != null)
                        {
                            string companies = "";
                            if (step.taskGroup.Companies != null && step.taskGroup.Companies.Count > 0)
                                companies = String.Join(" | ", step.taskGroup.Companies.Select(x => x.CompanyName));
                            else if (step.taskGroup.CompaniesBulk != null && step.taskGroup.CompaniesBulk.Count > 0)
                                companies = String.Join(" | ", step.taskGroup.CompaniesBulk.Select(x => x.CompanyName));
                            e.Value = companies;
                        }
                        break;

                    case "Departments":

                        if (step.taskGroup != null)
                        {
                            string depts = "";
                            if (step.taskGroup.Departments != null && step.taskGroup.Departments.Count > 0)
                                depts = String.Join(" | ", step.taskGroup.Departments.Select(x => x.DeptName));
                            else if (step.taskGroup.DepartmentsBulk != null && step.taskGroup.DepartmentsBulk.Count > 0)
                                depts = String.Join(" | ", step.taskGroup.DepartmentsBulk.Select(x => x.DeptName));
                            e.Value = depts;
                        }
                        break;
                    case "Group":


                        e.Value = step.taskGroup.GroupName;

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

                    case "StepLevel4":

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
                                            e.Value = groupList[3].GroupName;
                                            break;
                                        case 5:
                                            e.Value = groupList[3].GroupName;
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
                                        e.Value = groupList[3].GroupName;
                                        break;
                                    case 5:
                                        e.Value = groupList[3].GroupName;
                                        break;
                                }
                            }

                        }
                        break;

                    case "StepLevel5":

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
                                            e.Value = groupList[3].GroupName;
                                            break;
                                        case 5:
                                            e.Value = groupList[4].GroupName;
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
                                        e.Value = groupList[3].GroupName;
                                        break;
                                    case 5:
                                        e.Value = groupList[4].GroupName;
                                        break;
                                }
                            }

                        }
                        break;
                }
        }

        private void GrdCntrlStepsForRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateStep();
        }

        private void UpdateStep()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target") != null)
            {
                var task = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
                if (task != null)
                {
                    ucFrmAddTask frmAddTask = new ucFrmAddTask();
                    frmAddTask.taskId = task.parentTaskId.Value;
                    frmAddTask.taskGroupId = task.taskGroupId.Value;
                    frmAddTask.editFlag = true;

                    Window win = new Window();
                    win.Content = frmAddTask;
                    win.Title = "Targets";
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("You do not have permission to View Task!");
                return;
            }
        }

        private void RefreshSystemPoints()
        {
            grdCntrlStepsForRegister.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompletedStepPoints;
            worker.RunWorkerAsync();
        }

        private void MbtnRefreshSystemPoints_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            RefreshSystemPoints();

            //TimeSpan ts = new TimeSpan(23, 59, 59);
            //Dispatcher.BeginInvoke(new Action(() =>
            //{

            //    for (int i = 0; i < grdCntrlStepsForRegister.VisibleRowCount; i++)
            //    {
            //        var _row = grdCntrlStepsForRegister.GetRow(i);
            //        int rowHandle = grdCntrlStepsForRegister.GetRowHandleByVisibleIndex(i);
            //        var _step = _row as ToDoTask;

            //        ToDoTaskRepo repo = new ToDoTaskRepo();
            //        if (_step.searchedFromDate != null && _step.searchedToDate != null /*&&  !String.IsNullOrEmpty(_step.SOField)*/)
            //        {
            //            var toDate = _step.searchedToDate.Value;

            //            toDate = toDate.Date + ts;

            //            var saleOrders = repo.getDepartmentalSOByDates(_step.taskGroup.Companies, _step.taskGroup.Departments, (DateTime)_step.searchedFromDate, toDate);

            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["margin"], saleOrders.Sum(x => x.margin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BudgetedMargininBase"], saleOrders.Sum(x => x.BudgetedMargininBase));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"], saleOrders.Sum(x => x.SalesBudgetedMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargin"], saleOrders.Sum(x => x.RevisedMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargininBase"], saleOrders.Sum(x => x.RevisedMargininBase));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesRevisedMargin"], saleOrders.Sum(x => x.SalesRevisedMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargin"], saleOrders.Sum(x => x.ActualMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargininBase"], saleOrders.Sum(x => x.ActualMargininBase));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesActualMargin"], saleOrders.Sum(x => x.SalesActualMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalFOBValue"], saleOrders.Sum(x => x.totalFOBValue));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalCFRValue"], saleOrders.Sum(x => x.totalCFRValue));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalBaseCFRValue"], saleOrders.Sum(x => x.totalBaseCFRValue));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountSER"], saleOrders.Sum(x => x.SoAmountSER));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountPER"], saleOrders.Sum(x => x.SoAmountPER));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninBase"], saleOrders.Sum(x => x.commisioninBase));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commision"], saleOrders.Sum(x => x.commision));

            //            //grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["searchedFromDate"], _step.searchedFromDate);
            //            //grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["searchedToDate"], _step.searchedToDate);

            //        }
            //        taskRepo.UpdateStep(_step);
            //    }

            //}), DispatcherPriority.Render);
        }

        private void OnDoWorkStepPoints(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }
        private void OnRunWorkerCompletedStepPoints(object o, RunWorkerCompletedEventArgs args)
        {

            TimeSpan ts = new TimeSpan(23, 59, 59);
            //Dispatcher.BeginInvoke(new Action(() =>
            //{

            for (int i = 0; i < grdCntrlStepsForRegister.VisibleRowCount; i++)
            {
                //var _row = grdCntrlStepsForRegister.GetRow(i);
                int rowHandle = grdCntrlStepsForRegister.GetRowHandleByVisibleIndex(i);
                var _row = grdCntrlStepsForRegister.GetRow(rowHandle);
                var _step = _row as ToDoTask;

                ToDoTaskRepo repo = new ToDoTaskRepo();
                if (rowHandle >= 0 && _step != null && _step.Id > 0)
                {
                    List<SaleOrder> saleOrders = new List<SaleOrder>();
                    if (_step.searchedFromDate != null && _step.searchedToDate != null /*&&  !String.IsNullOrEmpty(_step.SOField)*/)
                    {
                        if (_step.taskGroup.CompaniesBulk != null && _step.taskGroup.CompaniesBulk.Count() > 0 && _step.taskGroup.DepartmentsBulk != null && _step.taskGroup.DepartmentsBulk.Count() > 0)
                        {
                            var toDate = _step.searchedToDate.Value;

                            toDate = toDate.Date + ts;

                            saleOrders = repo.getDepartmentalSOByDates(_step.taskGroup.CompaniesBulk, _step.taskGroup.DepartmentsBulk, (DateTime)_step.searchedFromDate, toDate);

                            var TotalCount = saleOrders.Count();
                            var UnderApprovalCount = saleOrders.Where(x => x.isApproved == false && x.isVoid != true).Count();
                            var ApprovedCount = saleOrders.Where(x => x.saleOrderStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true).Count();
                            var UnderClosingCount = saleOrders.Where(x => x.PendingForClosing == true && x.isVoid != true).Count();
                            var ClosedCount = saleOrders.Where(x => (x.isApproved == true && x.stage == "Closed") || (x.isApproved == true && x.saleOrderStatus.isActive == false && x.PendingForClosing != true)).Count();

                            var principalSO = saleOrders.Where(x => x.saleOrdertype == InquiryType.Principal).ToList();
                            var supplyCCCSO = saleOrders.Where(x => x.saleOrdertype == InquiryType.SupplyCCC).ToList();
                            var nonPrincipalSO = saleOrders.Where(x => x.saleOrdertype != InquiryType.Principal && x.saleOrdertype != InquiryType.SupplyCCC).ToList();

                            var margin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin))) /*+ (principalSO.Sum(x => x.margin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["margin"], /*saleOrders.Sum(x => x.margin)*/margin);

                            var MarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BudgetedMargininBase"], MarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var MarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"], MarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);

                            var revMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) /*+ (principalSO.Sum(x => x.RevisedMargin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargin"], /*saleOrders.Sum(x => x.RevisedMargin)*/revMargin);

                            var revMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargininBase"], revMarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var revMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesRevisedMargin"], /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/revMarginSE);

                            var actualMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) /*+ (principalSO.Sum(x => x.ActualMargin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargin"], /*saleOrders.Sum(x => x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin)))*/ actualMargin);

                            var actualMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargininBase"], actualMarginME/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var actualMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesActualMargin"], actualMarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);


                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalFOBValue"], saleOrders.Sum(x => x.totalFOBValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalCFRValue"], saleOrders.Sum(x => x.totalCFRValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalBaseCFRValue"], saleOrders.Sum(x => x.totalBaseCFRValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountSER"], saleOrders.Sum(x => x.SoAmountSER));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountPER"], saleOrders.Sum(x => x.SoAmountPER));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninBase"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * x.exchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commision"], saleOrders.Sum(x => x.commision));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninSE"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * Convert.ToDouble(x.marginExchangeRate)));

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginOC"], saleOrders.Sum(x => Convert.ToDouble(x.SystemMargin)));
                            var systemMarginSE = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginSE"], systemMarginSE);
                            var systemMarginME = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginME"], systemMarginME);

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommision"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision)));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommisionSER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommisionMER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)));

                            var BMGrossProfitSE = saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)) + Convert.ToDouble(MarginSE);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitSE"], BMGrossProfitSE);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitME"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)) + Convert.ToDouble(MarginME)));

                            var EstimatedGrossProfitSE = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["EstimatedGrossProfitSE"]));

                            if (BMGrossProfitSE > EstimatedGrossProfitSE)
                                grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["EstimatedGrossProfitSE"], BMGrossProfitSE);

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TotalQuantity"], saleOrders.Sum(x => Convert.ToDouble(x.TotalQuantity)));

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TotalOrdersCount"], TotalCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["UnderApprovalOrdersCount"], UnderApprovalCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ApprovedOrdersCount"], ApprovedCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["UnderClosingOrdersCount"], UnderClosingCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ClosedOrdersCount"], ClosedCount);
                        }
                        else
                        {
                            var toDate = _step.searchedToDate.Value;

                            toDate = toDate.Date + ts;

                            saleOrders = repo.getDepartmentalSOByDates(_step.taskGroup.Companies, _step.taskGroup.Departments, (DateTime)_step.searchedFromDate, toDate);

                            var TotalCount = saleOrders.Count();
                            var UnderApprovalCount = saleOrders.Where(x => x.isApproved == false && x.isVoid != true).Count();
                            var ApprovedCount = saleOrders.Where(x => x.saleOrderStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true).Count();
                            var UnderClosingCount = saleOrders.Where(x => x.PendingForClosing == true && x.isVoid != true).Count();
                            var ClosedCount = saleOrders.Where(x => (x.isApproved == true && x.stage == "Closed") || (x.isApproved == true && x.saleOrderStatus.isActive == false && x.PendingForClosing != true)).Count();

                            var principalSO = saleOrders.Where(x => x.saleOrdertype == InquiryType.Principal).ToList();
                            var supplyCCCSO = saleOrders.Where(x => x.saleOrdertype == InquiryType.SupplyCCC).ToList();
                            var nonPrincipalSO = saleOrders.Where(x => x.saleOrdertype != InquiryType.Principal && x.saleOrdertype != InquiryType.SupplyCCC).ToList();

                            var margin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin))) /*+ (principalSO.Sum(x => x.margin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["margin"], /*saleOrders.Sum(x => x.margin)*/margin);

                            var MarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BudgetedMargininBase"], MarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var MarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"], MarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);

                            var revMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) /*+ (principalSO.Sum(x => x.RevisedMargin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargin"], /*saleOrders.Sum(x => x.RevisedMargin)*/revMargin);

                            var revMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargininBase"], revMarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var revMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesRevisedMargin"], /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/revMarginSE);

                            var actualMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) /*+ (principalSO.Sum(x => x.ActualMargin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargin"], /*saleOrders.Sum(x => x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin)))*/ actualMargin);

                            var actualMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargininBase"], actualMarginME/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var actualMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesActualMargin"], actualMarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);


                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalFOBValue"], saleOrders.Sum(x => x.totalFOBValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalCFRValue"], saleOrders.Sum(x => x.totalCFRValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalBaseCFRValue"], saleOrders.Sum(x => x.totalBaseCFRValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountSER"], saleOrders.Sum(x => x.SoAmountSER));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountPER"], saleOrders.Sum(x => x.SoAmountPER));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninBase"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * x.exchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commision"], saleOrders.Sum(x => x.commision));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninSE"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * Convert.ToDouble(x.marginExchangeRate)));

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginOC"], saleOrders.Sum(x => Convert.ToDouble(x.SystemMargin)));
                            var systemMarginSE = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginSE"], systemMarginSE);
                            var systemMarginME = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginME"], systemMarginME);

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommision"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision)));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommisionSER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommisionMER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)));

                            var BMGrossProfitSE = saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)) + Convert.ToDouble(MarginSE);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitSE"], BMGrossProfitSE);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitME"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)) + Convert.ToDouble(MarginME)));

                            var EstimatedGrossProfitSE = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["EstimatedGrossProfitSE"]));

                            if (BMGrossProfitSE > EstimatedGrossProfitSE)
                                grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["EstimatedGrossProfitSE"], BMGrossProfitSE);

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TotalQuantity"], saleOrders.Sum(x => Convert.ToDouble(x.TotalQuantity)));

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TotalOrdersCount"], TotalCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["UnderApprovalOrdersCount"], UnderApprovalCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ApprovedOrdersCount"], ApprovedCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["UnderClosingOrdersCount"], UnderClosingCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ClosedOrdersCount"], ClosedCount);
                        }
                    }

                    if (_step.calculationType != null)
                    {
                        var achieved = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, _step.calculationType.AchievedField.SOFieldName));
                        var total = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, _step.calculationType.TotalField.SOFieldName));
                        var result = Math.Round((achieved / total) * 100, 0);

                        grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TargetAchievedPercenatage"], result);
                        //var result1 = Math.Round((Convert.ToDouble(_step.GetPropertyValue(_step.calculationType.AchievedField.SOFieldName)) / Convert.ToDouble(_step.GetPropertyValue(_step.calculationType.TotalField.SOFieldName))) * 100, 2);
                        var status = repo.GetTaskStatusByPercentage(result);
                        if (status != null)
                        {
                            _step.statusId = status.Id;
                        }

                    }
                    else if (_step.taskGroup != null && _step.taskGroup.calculationType != null)
                    {
                        var achieved = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, _step.taskGroup.calculationType.AchievedField.SOFieldName));
                        var total = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, _step.taskGroup.calculationType.TotalField.SOFieldName));
                        var result = Math.Round((achieved / total) * 100, 0);

                        grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TargetAchievedPercenatage"], result);
                        //var result1 = Math.Round((Convert.ToDouble(_step.GetPropertyValue(_step.calculationType.AchievedField.SOFieldName)) / Convert.ToDouble(_step.GetPropertyValue(_step.calculationType.TotalField.SOFieldName))) * 100, 2);
                        var status = repo.GetTaskStatusByPercentage(result);
                        if (status != null)
                        {
                            _step.statusId = status.Id;
                        }

                    }
                    taskRepo.UpdateStep(_step);
                }

            }

            //}), DispatcherPriority.Render);
            //grdProgressBar.Visibility = Visibility.Collapsed;
            grdCntrlStepsForRegister.ShowLoadingPanel = false;
        }

        double salesBMTotal1 = 0, salesBMTotal2 = 0, SOamountSERtotal = 0, totalTaskPoints = 0;

        private void ChkClosedSO_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tblStepsListView.FormatConditions)
            {
                if (c.FieldName == "ClosedOrdersCount")
                    c.IsEnabled = true;
            }
        }

        private void ChkClosedSO_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tblStepsListView.FormatConditions)
            {
                if (c.FieldName == "ClosedOrdersCount")
                    c.IsEnabled = false;
            }
        }

        private void ChkApprovedSO_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tblStepsListView.FormatConditions)
            {
                if (c.FieldName == "OpenOrdersCount")
                    c.IsEnabled = false;
            }
        }

        private void ChkApprovedSO_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tblStepsListView.FormatConditions)
            {
                if (c.FieldName == "OpenOrdersCount")
                    c.IsEnabled = true;
            }

        }

        private void GrdCntrlStepsForRegister_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.IsTotalSummary)
            {
                GridControl gridControl = sender as GridControl;

                GridSummaryItem item = e.Item as GridSummaryItem;
                if (item.FieldName == "SoAmountSERpercent")
                {
                    switch (e.SummaryProcess)
                    {
                        case CustomSummaryProcess.Start:
                            salesBMTotal1 = 0;
                            SOamountSERtotal = 0;
                            break;
                        case CustomSummaryProcess.Calculate:
                            salesBMTotal1 += Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(e.RowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"]));
                            SOamountSERtotal += Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(e.RowHandle, grdCntrlStepsForRegister.Columns["SoAmountSER"]));

                            //Total = debitTotal - creditTotal;
                            break;
                        case CustomSummaryProcess.Finalize:

                            e.TotalValue = Math.Round((salesBMTotal1 / SOamountSERtotal) * 100, 2);
                            break;

                    }
                }
                else if (item.FieldName == "SalesBudgetedMarginPercent")
                {
                    switch (e.SummaryProcess)
                    {
                        case CustomSummaryProcess.Start:
                            salesBMTotal2 = 0;
                            totalTaskPoints = 0;
                            break;
                        case CustomSummaryProcess.Calculate:
                            salesBMTotal2 += Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(e.RowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"]));
                            totalTaskPoints += Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(e.RowHandle, grdCntrlStepsForRegister.Columns["TaskPoints"]));

                            //Total = debitTotal - creditTotal;
                            break;
                        case CustomSummaryProcess.Finalize:

                            e.TotalValue = Math.Round((salesBMTotal2 / totalTaskPoints) * 100, 2);
                            break;
                    }
                }
            }
        }


        public void loadcomments()
        {
            try
            {
                ToDoTask target = new ToDoTask();
                var step = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
                if (step != null && step.ParentTask != null)
                    target = step.ParentTask;
              

                if (target != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(step.Id, TransactionItemType.TargetStep);
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
            var target = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
            if (target != null)
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                if (target != null)
                {
                    var taskGroup = target.taskGroup;

                    List<User> groupUsers = new List<User>();

                    if (taskGroup.usersBulk != null && taskGroup.usersBulk.Count > 0)
                        groupUsers = taskGroup.usersBulk;
                    else if (taskGroup.users != null && taskGroup.users.Count > 0)
                        groupUsers = taskGroup.users;

                    if (taskGroup != null && groupUsers != null && groupUsers.Count != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, groupUsers, TransactionItemType.TargetStep);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }


                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    if (frmInputBox.comment != "" && target.Id != 0)
                    {
                        if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                else
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", null);
                            }
                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                                else
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", null);
                            }
                        }

                        procurementRepo.Add(target.Id, TransactionItemType.TargetStep, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                        //frmInputBox.taggedUsers = new List<User>();
                        loadcomments();
                        MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                        if (window1 != null) { window1.UrgentNotificationGlow(); }
                    }
                    else if (target.Id == 0)
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

        private void MenuItemLinkedSaleorders_Click(object sender, RoutedEventArgs e)
        {
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            TimeSpan ts = new TimeSpan(23, 59, 59);
            var selectedItem = grdCntrlStepsForRegister.SelectedItem as ToDoTask;

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
            for (int i = 0; i < grdCntrlStepsForRegister.VisibleRowCount; i++)
            {
                //var _row = grdCntrlStepsForRegister.GetRow(i);
                int rowHandle = grdCntrlStepsForRegister.GetRowHandleByVisibleIndex(i);
                var _row = grdCntrlStepsForRegister.GetRow(rowHandle);
                var _step = _row as ToDoTask;

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

        private void ChkGroupLevel1_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel1"].Visible = true;
        }

        private void ChkGroupLevel2_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel2"].Visible = true;
            chkGroupLevel1.IsChecked = true;
        }

        private void ChkGroupLevel3_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel3"].Visible = true;
            chkGroupLevel1.IsChecked = true;
            chkGroupLevel2.IsChecked = true;
        }

        private void ChkGroupLevel4_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel4"].Visible = true;
            chkGroupLevel1.IsChecked = true;
            chkGroupLevel2.IsChecked = true;
            chkGroupLevel3.IsChecked = true;
        }

        private void ChkGroupLevel5_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel5"].Visible = true;
            chkGroupLevel1.IsChecked = true;
            chkGroupLevel2.IsChecked = true;
            chkGroupLevel3.IsChecked = true;
            chkGroupLevel4.IsChecked = true;
        }

        private void ChkGroupLevel1_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel1"].Visible = false;
            chkGroupLevel2.IsChecked = false;
            chkGroupLevel3.IsChecked = false;
            chkGroupLevel4.IsChecked = false;
            chkGroupLevel5.IsChecked = false;
        }

        private void ChkGroupLevel2_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel2"].Visible = false;
            chkGroupLevel3.IsChecked = false;
            chkGroupLevel4.IsChecked = false;
            chkGroupLevel5.IsChecked = false;
        }

        private void ChkGroupLevel3_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel3"].Visible = false;
            chkGroupLevel4.IsChecked = false;
            chkGroupLevel5.IsChecked = false;
        }

        private void ChkGroupLevel4_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel4"].Visible = false;
            chkGroupLevel5.IsChecked = false;
        }

        private void ChkGroupLevel5_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel5"].Visible = false;
        }

        private void btnFav_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (report.isFavourite != true)
            {
                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to mark " + this.lblHeading + "" + " as favourite Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    GridReportRepo reportRepo = new GridReportRepo();

                    report.isFavourite = true;
                    reportRepo.MarkFavouriteReport(report);
                    DXMessageBox.Show("Report has been marked as favourite report successfully");

                }
            }
            else
            {
                if (report.isFavourite != false)
                {
                    var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to unmark " + this.lblHeading + "" + " as favourite Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                    {
                        GridReportRepo reportRepo = new GridReportRepo();
                        report.isFavourite = false;
                        reportRepo.MarkFavouriteReport(report);
                        DXMessageBox.Show("Report has been unmarked as favourite report successfully");
                    }
                }
            }
        }

        private void grdCntrlStepsForRegister_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var task = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
            task.EstimatedGrossProfitSE = Convert.ToDouble(txtEstimatedGP.Text);

            taskRepo.UpdateStep(task);

            DXMessageBox.Show("Updated Estimated Gross Profit (SE)");
        }

        private void tblStepsListView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            var task = grdCntrlStepsForRegister.GetFocusedRow() as ToDoTask;
            txtEstimatedGP.Text = task.EstimatedGrossProfitSE.ToString();
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
            ToDoTask target = new ToDoTask();
           
            var step = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
            if (step != null && step.ParentTask != null)
                target = step.ParentTask;

            if (target != null)
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();

                if (grdCommentss.SelectedItem != null)
                {
                    var comment = grdCommentss.SelectedItem as CommentLog;
                    if (target != null)
                    {
                        var taskGroup = target.taskGroup;
                        if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                        {
                            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.users, comment, TransactionItemType.TargetStep);
                            inputBox.ShowDialog();
                        }
                        else
                        {
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                        }

                        if (target != null)
                        {
                            ProcurementRepo procurementRepo = new ProcurementRepo();

                            if (frmInputBox.commentAdded == true && target.Id != 0)
                            {

                                var commentId = procurementRepo.AddCommentLinkNotification(target.Id, TransactionItemType.TargetStep, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                                if (commentId != null)
                                {
                                    foreach (var user in frmInputBox.Comment.TaggedList)
                                    {
                                        if (frmInputBox.FlagForTag == true)
                                            notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                        else
                                            notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                    }

                                    foreach (var user in frmInputBox.Comment.CCUsersList)
                                    {
                                        if (frmInputBox.FlagForCC == true)
                                            notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                        else
                                            notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                    }
                                }


                                MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                                if (window1 != null) { window1.UrgentNotificationGlow(); }
                            }
                            else if (target.Id == 0)
                            {
                                DXMessageBox.Show("Kindly save Target first to add a comment!");
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
    }
}
