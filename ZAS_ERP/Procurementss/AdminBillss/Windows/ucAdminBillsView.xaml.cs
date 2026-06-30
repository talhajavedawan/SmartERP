using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Reports;
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
using ZAS_ERP.Procurementss.AdminBillss.UserControls;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.AdminBillss.Windows
{
    /// <summary>
    /// Interaction logic for ucAdminBillsView.xaml
    /// </summary>
    public partial class ucAdminBillsView : DXWindow
    {
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        ucFrmBillAdd frmBillAdd = new ucFrmBillAdd();
        GridReport report = new GridReport();
        string reportTitle;
        public MainWindow myParent = null;

        public ucAdminBillsView(GridReport reportToEdit, string title)
        {
            this.DataContext = this;
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }

        private void GrdBillsList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            //var bill=e.Source.CurrentItem as AdminBill ;
            var bill = grdBillsList.GetRowByListIndex(e.ListSourceRowIndex) as AdminBill;

            if (e.IsGetData)
            {

                switch (e.Column.FieldName)
                {
                    case "Stage":
                        if (bill != null)
                        {
                            if (bill.isVoid == true)
                            {
                                e.Value = "Void";
                            }
                            else if (bill.isReApproved == false)
                            {
                                e.Value = "Under Re-Approval";
                            }
                            else if (bill.isApproved == true && bill.stage == "Closed")
                            {
                                e.Value = "Closed";
                            }
                            else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
                            {
                                e.Value = "Closed";
                            }
                            else if (bill.isApproved == true && bill.PendingForClosing == true)
                            {
                                e.Value = "Under Closing";
                            }
                            else if (bill.isApproved == true)
                            {
                                e.Value = "Approved";
                            }
                            else if (bill.isApproved == false)
                            {
                                e.Value = "Under Approval";
                            }
                            else if (bill.PendingForClosing == true)
                            {
                                e.Value = "Under Closing";
                            }
                        }
                        break;

                    case "Closed":
                        if (bill.Payments.Count > 0 && bill.Payments.Where(x => x.Status != null && x.Status.isActive == true).Count() == 0)
                        {
                            e.Value = "Closed";
                        }
                        else
                        {
                            e.Value = "Open";
                        }
                        break;
                    case "PayeeName":
                        if (e.GetListSourceFieldValue("payee_Id") != null)
                        {
                            var payee_id = Convert.ToInt32(e.GetListSourceFieldValue("payee_Id"));
                            if (payee_id > 0)
                            {
                                var payee = billsRepo.GetPayeeForBillRegister(payee_id);
                                string payeeName;
                                payeeName = payee.PayeeName;
                                while (payee.ParentId != null)
                                {
                                    payee = billsRepo.GetPayeeForBillRegister((int)payee.ParentId);
                                    payeeName = payee.PayeeName + " > " + payeeName;
                                }
                                e.Value = payeeName;
                            }
                        }
                        break;

                    case "paidAmount":
                        if (bill.Payments.Count != 0)
                        {
                            var pymnts = bill.Payments.Where(x => x.isVoid != true).ToList();
                            e.Value = pymnts.Sum(x => x.DebitedAmount);
                        }
                        break;

                    case "balanceAmount":
                        if (bill.Payments.Count != 0)
                        {
                            var pymntss = bill.Payments.Where(x => x.isVoid != true).ToList();
                            var amountPaid = pymntss.Sum(x => x.DebitedAmount);
                            e.Value = bill.AmountOC - amountPaid;
                        }
                        break;

                    case "percBalanceAmount":
                        var finalPayments = bill.Payments.Where(x => x.isVoid != true).ToList();
                        var paidAmount = finalPayments.Sum(x => x.DebitedAmount);
                        var percent = (((paidAmount) / bill.AmountOC) * 100);
                        e.Value = Math.Round(percent, 2);
                        break;

                    case "BillCreator":
                        if (bill.Creator != null && bill.Creator.person != null)
                        {
                            var userName = bill.Creator.person.FName + " " + bill.Creator.person.LName;
                            e.Value = userName;
                        }
                        break;
                }
            }

        }

        private void GrdBillsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateBillWindow();
        }
        private void UpdateBillWindow()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill") != null)
            {
                frmBillAdd = new ucFrmBillAdd();
                DXWindow frmBill = new DXWindow();
                frmBill.WindowState = WindowState.Maximized;
                frmBill.Title = "Update Bills";

                var selectedRow = grdBillsList.SelectedItem as AdminBill;

                if (selectedRow != null)
                {
                    billsRepo = new AdminBillsRepo();
                    frmBillAdd.bills = new List<AdminBill>();
                    frmBillAdd.bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                    frmBillAdd.editFlag = true;
                    frmBillAdd.groupId = selectedRow.transactionGroupId;
                    frmBillAdd.isProgressiveCost = selectedRow.isProgressiveCost;
                    frmBillAdd.groupId = selectedRow.transactionGroupId;
                    frmBill.Content = frmBillAdd;
                    frmBill.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Existing Bill!");
            }

        }
        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdBillsList.View);
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
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null  && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    ReportLogic.SaveGridReport(grdBillsList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
                                    DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
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
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    ReportLogic.SaveGridReport(grdBillsList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                //setting input Fields
                                setReportName.txtName.Text = report.reportName;
                                //setReportName.LoadReportTypes();
                                setReportName.GetEnum();
                                setReportName.reportTypes.EditValue = report.gridReportType;
                                setReportName.lookupGroup.EditValue = group.groupName;
                                setReportName.LoadGroups();
                                setReportName.ShowDialog();
                                var updatedReport = setReportName.report;
                                if (updatedReport != null && updatedReport.gridReportGroup != null  && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdBillsList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                if (updatedReport != null && updatedReport.gridReportGroup != null  && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdBillsList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                    ReportLogic.UpdateGridReport(grdBillsList, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdBillsList, str, reportType, groupDetails, report.Id, report.settingkey);

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

                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                ReportLogic.DeleteReport(grdBillsList, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                                ReportLogic.DeleteReport(grdBillsList, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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
           
            loadingGif.Visibility = Visibility.Visible;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {
               var  repo = new GridReportRepo();

                report = repo.GetReportById(report.Id);
                grdBillsList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
            }
            else
            {
            }
        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            var repo = new GridReportRepo();
            string path = this.Title;
            int pos = path.LastIndexOf("/") + 1;
            var reportTitlePath = path.Substring(pos, path.Length - pos);

             report = repo.GetReportByName(reportTitlePath);
            AdminBillsRepo adminBillsRepo = new AdminBillsRepo();
            if (report.settingkey == "Admin Bill Register")
            { grdBillsList.ItemsSource = adminBillsRepo.GetAllOpenAndClosed(SYSTEM_STATIC.currentUser.id); }
            else
            { grdBillsList.ItemsSource = adminBillsRepo.GetAllOpenBills(SYSTEM_STATIC.currentUser.id); }
            loadingGif.Visibility = Visibility.Hidden;

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
                                
                                var type = report.gridReportType;
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

                                Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
                                setReportName.ShowDialog();
                                var exportToMemorizedReport = setReportName.report;
                                if (exportToMemorizedReport != null && exportToMemorizedReport.gridReportGroup != null  && exportToMemorizedReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToMemorizedReport.gridReportGroup;
                                    var reportType = exportToMemorizedReport.gridReportType;
                                    var reportName = exportToMemorizedReport.reportName;
                                    ReportLogic.SaveGridReport(grdBillsList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                if (exportToStandardReport != null && exportToStandardReport.gridReportGroup != null  && exportToStandardReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToStandardReport.gridReportGroup;
                                    var reportType = exportToStandardReport.gridReportType;
                                    var reportName = exportToStandardReport.reportName;
                                    ReportLogic.ExportToStandard(grdBillsList, reportName, reportType, reportGroup, report.settingkey);
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

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AdminBillsRepo adminBillsRepo = new AdminBillsRepo();
            GridReportRepo repo = new GridReportRepo();
            var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (report != null)
                        {
                            mbtnExportToStandardReport1.IsVisible = false;
                            mbtnShareReport.IsVisible = false;
                            grdBillsList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Admin Bills" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Admin Bills" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Admin Bill Register")
                            {
                                grdBillsList.ItemsSource = adminBillsRepo.GetAllBills(SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                grdBillsList.ItemsSource = adminBillsRepo.GetAllOpenBills(SYSTEM_STATIC.currentUser.id);
                            }
                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here     
                            mbtnExportToMemorizedReport1.IsVisible = false;
                            grdBillsList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Admin Bills" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Admin Bills" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Admin Bill Register")
                            {
                                grdBillsList.ItemsSource = adminBillsRepo.GetAllBills(SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                grdBillsList.ItemsSource = adminBillsRepo.GetAllOpenBills(SYSTEM_STATIC.currentUser.id);
                            }
                        }
                    }
                    break;
            }
            SetColumnsVisibility();
        }

        private void SetColumnsVisibility()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Summary Memo for Admin Bills") == null)
            {
                grdBillsList.Columns["hasSummary"].Visible = false;
                grdBillsList.Columns["hasSummary"].ShowInColumnChooser = false;
                grdBillsList.Columns["managementSummary.SummaryName"].Visible = false;
                grdBillsList.Columns["managementSummary.SummaryName"].ShowInColumnChooser = false;
                grdBillsList.Columns["SummaryMemo"].Visible = false;
                grdBillsList.Columns["SummaryMemo"].ShowInColumnChooser = false;
            }
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Admin Bills Report") != null)
            {

                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to share " + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    //GridControl gridControl = grdsaleOrder;
                    //gridControl.SaveLayoutToStream(memoryStream);
                    //report.settingValue = Convert.ToString(memoryStream).Trim();
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
                DXMessageBox.Show("You don't have permission Share Sale Orders Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
        }

        private void mbtnDriectClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                UsersRepo usersRepo = new UsersRepo();
                billsRepo = new AdminBillsRepo();
                var selectedRow = (AdminBill)grdBillsList.GetFocusedRow();
                bool isFullyPaid = true;
                AdminBillStatus statusChanged = new AdminBillStatus();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                if (selectedRow != null)
                {
                    var previous_status = selectedRow.BillStatus.Status;
                    var bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                    var totalAmountOC = bills.Sum(x => x.AmountOC);
                    if (bills != null && bills.Count > 0)
                    {
                        if (bills[0].isApproved == false)
                        {
                            DXMessageBox.Show("Bills are pending for approval!");
                            return;
                        }

                        foreach (var _billl in bills)
                        {


                            if (_billl.Payments == null || _billl.Payments.Count == 0)
                            {
                                //DXMessageBox.Show("Bills are not Fully Paid yet!");
                                isFullyPaid = false;
                                break;
                            }
                            else if ((_billl.Payments.Sum(x => x.DebitedAmount) - _billl.AmountOC) != 0)
                            {
                                //DXMessageBox.Show("Bills are not Fully Paid yet!");
                                isFullyPaid = false;
                                break;
                            }

                        }

                        if (isFullyPaid == false && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bills without Payments") == null)
                        {
                            DXMessageBox.Show("Permission required to close Unpaid Admin Bills!");
                            return;
                        }

                        if (isFullyPaid == false)
                        {
                            MessageBoxResult result = DevExpress.Xpf.Core.DXMessageBox.Show("This Admin Bill is not Paid Yet, Do you want to Close it?", "Bill is Unpaid", MessageBoxButton.YesNo);
                            if (result == MessageBoxResult.No)
                            {
                                return;
                            }
                        }

                        statusChanged = null;
                        ucFrmBillDirectClose ucFrmDirectClose = new ucFrmBillDirectClose();
                        if (selectedRow.BillStatus != null)
                        {
                            ucFrmDirectClose.statusName.Text = selectedRow.BillStatus.Status;

                            var brush = new BrushConverter();
                            ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(selectedRow.BillStatus.backcolor);
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

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Admin Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Admin Bill without Approval") != null)
                            {
                                for (int i = 0; i < bills.Count; i++)
                                {
                                    bills[i].PendingForClosing = false;
                                    bills[i].stage = TransactionStage.Closed.ToString();
                                    bills[i].statusId = statusChanged.Id;
                                    bills[i].LastStatusChangeDate = System.DateTime.Now;
                                    bills[i].ClosingDate = System.DateTime.Now;


                                }
                                billsRepo.ApproveBill(bills);
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);

                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Admin Bill having system ref #: " + bills[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + totalAmountOC,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (bills.Count != 0)
                                    {
                                        procurementRepo.Add(bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }
                                //Load_Receipts();
                            }
                            else
                            {
                                for (int i = 0; i < bills.Count; i++)
                                {
                                    bills[i].PendingForClosing = true;
                                    bills[i].stage = TransactionStage.AwaitingApproval.ToString();
                                    bills[i].statusId = statusChanged.Id;
                                    bills[i].LastStatusChangeDate = System.DateTime.Now;
                                    bills[i].ClosingDate = System.DateTime.Now;


                                }
                                billsRepo.ApproveBill(bills);
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);

                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Admin Bill having system ref #: " + bills[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + totalAmountOC,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (bills.Count != 0)
                                    {
                                        procurementRepo.Add(bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Admin Bill #" + bills[0].SystemRefNo, bills[0].transactionGroupId, TransactionItemType.Admin_Bill, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception)
            {
            }
        }

        private void mbtnApprove_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                //if ((SystemLogic.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt without Approval") != null) ? true : false)
                //{
                if (grdBillsList.GetFocusedRow() != null)
                {
                    billsRepo = new AdminBillsRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (AdminBill)grdBillsList.GetFocusedRow();
                    var bills = billsRepo.GetBillsByGroupId(selectedRow.transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (bills != null && bills.Count > 0)
                    {
                        if (bills[0].isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Admin Bill") != null) ? true : false)
                            {
                                for (int i = 0; i < bills.Count; i++)
                                {
                                    bills[i].isApproved = true;
                                    bills[i].stage = TransactionStage.Approved.ToString();
                                }
                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, 17, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                billsRepo.ApproveBill(bills);

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
                        else if (bills[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Admin Bill") != null) ? true : false)
                            {
                                for (int i = 0; i < bills.Count; i++)
                                {
                                    bills[i].isReApproved = true;
                                    bills[i].stage = TransactionStage.Approved.ToString();

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, (int)TransactionItemType.Admin_Bill, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                billsRepo.ApproveBill(bills);
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
    }
}
