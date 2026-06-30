using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Migrations;
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
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.PurchaseOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucPurchaseOrderView.xaml
    /// </summary>
    public partial class ucPurchaseOrderView : DXWindow
    {
        GridReport report = new GridReport();
        string reportTitle;
        public MainWindow myParent = null;
        PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
        static List<ExchangeRateGroup> exchangeRateGroupsSER = new List<ExchangeRateGroup>();
        static List<ExchangeRateGroup> exchangeRateGroupsMER = new List<ExchangeRateGroup>();
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;
        public ucPurchaseOrderView()
        {
            InitializeComponent();
        }
        public ucPurchaseOrderView(GridReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
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
            GridReport report = repo.GetReportByName(reportTitlePath);
             purchaseOrderRepo = new PurchaseOrderRepo();
            if (report.settingkey == "Purchase Register")
            { grdPurchaseOrderReport.ItemsSource = purchaseOrderRepo.getPurchaseRegister(SYSTEM_STATIC.currentUser.id); }
            else
            { grdPurchaseOrderReport.ItemsSource = purchaseOrderRepo.getAll(SYSTEM_STATIC.currentUser.id); }
            loadingGif.Visibility = Visibility.Hidden;

        }

        private void GrdpurchaseOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditPurchaseOrder();
        }

        private void EditPurchaseOrder()
        {
            if (grdPurchaseOrderReport.GetFocusedRowCellValue(grdPurchaseOrderReport.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (int)grdPurchaseOrderReport.GetFocusedRowCellValue(grdPurchaseOrderReport.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();

            }
        }
        public void LoadSERGroups()
        {
            exchangeRateGroupsSER = exchangeRateGroupRepo.GetAllSER();
        }
        public void LoadMERGroups()
        {
            exchangeRateGroupsMER = exchangeRateGroupRepo.GetAllMER();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            Loader.DeferedVisibility = true;
            PurchaseOrderRepo poRepo = new PurchaseOrderRepo();
            GridReportRepo repo = new GridReportRepo();
            LoadSERGroups();
            LoadMERGroups();
            var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here       
                            mbtnExportToStandardReport1.IsVisible = false;
                            mbtnShareReport.IsVisible = false;
                            grdPurchaseOrderReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Purchase Orders" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Purchase Orders" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Purchase Register")
                            { grdPurchaseOrderReport.ItemsSource = poRepo.getPurchaseRegister(SYSTEM_STATIC.currentUser.id); }
                            else
                                grdPurchaseOrderReport.ItemsSource = poRepo.getAll(SYSTEM_STATIC.currentUser.id);
                            btnFav.IsEnabled = false;

                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here     
                            mbtnExportToMemorizedReport1.IsVisible = false;
                            grdPurchaseOrderReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Purchase Orders" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Purchase Orders" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey=="Purchase Register")
                            { grdPurchaseOrderReport.ItemsSource = poRepo.getPurchaseRegister(SYSTEM_STATIC.currentUser.id); }
                            else
                                grdPurchaseOrderReport.ItemsSource = poRepo.getAll(SYSTEM_STATIC.currentUser.id);
                        }
                    }
                        break;        
            }
            Loader.DeferedVisibility = false;
        }
        private void MbtnSaveAsNew1_Click(object sender, EventArgs e)
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
                                    ReportLogic.SaveGridReport(grdPurchaseOrderReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.gridReportType != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    ReportLogic.SaveGridReport(grdPurchaseOrderReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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

        private void MbtnRenameReport1_Click(object sender, EventArgs e)
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
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdPurchaseOrderReport, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                        ReportLogic.RenameGridReport(grdPurchaseOrderReport, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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

        private void MbtnUpdateReport1_Click(object sender, EventArgs e)
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
                                    ReportLogic.UpdateGridReport(grdPurchaseOrderReport, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdPurchaseOrderReport, str, reportType, groupDetails, report.Id, report.settingkey);

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

        private void MbtnDeleteReport1_Click(object sender, EventArgs e)
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

                                ReportLogic.DeleteReport(grdPurchaseOrderReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                                ReportLogic.DeleteReport(grdPurchaseOrderReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

        private void MbtnRefreshReport1_Click(object sender, EventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {
                GridReportRepo repo = new GridReportRepo();
                report=repo.GetReportById(report.Id);
                grdPurchaseOrderReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
            }
            else
            {
            }
            loadingGif.Visibility = Visibility.Visible;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void MbtnExportToMemorizedReport1_Click(object sender, EventArgs e)
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
                                    ReportLogic.SaveGridReport(grdPurchaseOrderReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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

        private void MbtnExportToStandardReport1_Click(object sender, EventArgs e)
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
                                    ReportLogic.ExportToStandard(grdPurchaseOrderReport, reportName, reportType, reportGroup, report.settingkey);
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
        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdPurchaseOrderReport.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = report.reportName;
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = report.reportName;
            link.ReportHeaderData = report.reportName;
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdsaleOrder.View.ShowPrintPreview(this);
        }

        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {

            var po = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;

            if (e.Column.FieldName == "LotNumberr")
            {
                string LotNo = "";
                if (po.lotNumber != null)
                    LotNo = po.lotNumber.LotNo;
                else if (!String.IsNullOrEmpty(po.lotNo))
                    LotNo = po.lotNo;
                e.Value = LotNo;
            }

            if (e.Column.FieldName == "Stage" && e.IsGetData)
            {
                var purchaseOrder = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseOrder;
                if (purchaseOrder.isVoid == true)
                {
                    e.Value = "Void";
                }
                else if (purchaseOrder.isReApproved == false)
                {
                    e.Value = "Under Approval";
                }
                else if (purchaseOrder.isApproved == true && purchaseOrder.stage == "Closed")
                {
                    e.Value = "Closed";
                }
                else if (purchaseOrder.isApproved == true && purchaseOrder.PurchaseOrderStatus.isActive == false && purchaseOrder.PendingForClosing != true)
                {
                    e.Value = "Closed";
                }
                else if (purchaseOrder.isApproved == true && purchaseOrder.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
                else if (purchaseOrder.isApproved == true)
                {
                    e.Value = "Approved";
                }
                else if (purchaseOrder.isApproved == false)
                {
                    e.Value = "Under Approval";
                }
                else if (purchaseOrder.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
            }
            if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();
                    e.Value = deptList[0].DeptName;
                }

            }
            if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }

                    deptList.Reverse();

                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[1].DeptName;
                            break;
                    }
          

                }

            }
            if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();

                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[2].DeptName;
                            break;
                    }

                


                }

            }
            if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and tranverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();

                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[3].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[3].DeptName;
                            break;
                    }

              

                }

            }
            if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row.department;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and tranverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();

                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[3].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[4].DeptName;
                            break;
                    }


                    //if (deptList.Count == 1)
                    //{
                    //    e.Value = deptList[0].DeptName;
                    //}
                    //if (deptList.Count == 2)
                    //{
                    //    e.Value = deptList[1].DeptName;
                    //}
                    //if (deptList.Count == 3)
                    //{
                    //    e.Value = deptList[2].DeptName;
                    //}
                    //if (deptList.Count == 4)
                    //{
                    //    e.Value = deptList[3].DeptName;
                    //}
                    //if (deptList.Count == 5)
                    //{
                    //    e.Value = deptList[4].DeptName;
                    //}

                    //if (deptList.Count <= 4)
                    //{
                    //    e.Value = dep;
                    //}
                    //else
                    //{
                    //    int index = 4;
                    //    if (index >= 0 && index < deptList.Count)
                    //    {
                    //        e.Value = deptList[4].DeptName;
                    //        dep = deptList[4].DeptName;
                    //    }
                    //}

                }

            }

            //if (e.Column.FieldName == "PaymentDueAgeingDays" && e.IsGetData)

            //{
            //    if (e.GetListSourceFieldValue("PaymentDueAgeing") != null)
            //    {
            //        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("PaymentDueAgeing"));

            //        //DateTime date;
            //        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
            //        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
            //        e.Value = NoDueAgeingDays;
            //    }
            //}

            if (e.Column.FieldName == "Department")

            {

                // string s = "Test: FieldTwo";

            }
            //if (e.Column.FieldName == "CreationAgeing")

            //{
            //    if (e.GetListSourceFieldValue("CreationDate") != null)
            //    {
            //        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("CreationDate"));

            //        Double CreateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
            //        CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
            //        e.Value = CreateAgeingDays;
            //        // string s = "Test: FieldTwo";
            //    }
            //}
            //if (e.Column.FieldName == "SODateAgeing")

            //{
            //    if (e.GetListSourceFieldValue("purchaseOrderDate") != null)
            //    {
            //        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("purchaseOrderDate"));

            //        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
            //        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
            //        e.Value = SoDateAgeingDays;
            //        // string s = "Test: FieldTwo";
            //    }
            //}
            //if (e.Column.FieldName == "SODeliveryAgeing")

            //{
            //    if (e.GetListSourceFieldValue("deliveryDate") != null)
            //    {
            //        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("deliveryDate"));

            //        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
            //        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
            //        e.Value = SoDateAgeingDays;
            //        // string s = "Test: FieldTwo";
            //    }
            //}
            if (e.Column.FieldName == "totalWeight")

            {
                if (e.GetListSourceFieldValue("products") != null)
                {
                    List<ProcurementProduct> products = (e.GetListSourceFieldValue("products")) as List<ProcurementProduct>;

                    decimal? totalweight = 0;
                    foreach (var pro in products)
                    {
                        if (pro.inquiryProduct.Weight != null || pro.inquiryProduct.Weight != 0)
                        {
                            totalweight = pro.inquiryProduct.Weight;
                        }
                    }
                    //CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                    e.Value = totalweight;
                    // string s = "Test: FieldTwo";
                }
            }
            if (e.Column.FieldName == "totalQuantity")

            {
                if (e.GetListSourceFieldValue("products") != null)
                {
                    List<ProcurementProduct> products = (e.GetListSourceFieldValue("products")) as List<ProcurementProduct>;

                    double totalquantity = 0;
                    foreach (var pro in products)
                    {
                        if (pro.inquiryProduct.quantity != 0)
                        {
                            totalquantity = pro.inquiryProduct.quantity;
                        }
                    }
                    //CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                    e.Value = totalquantity;
                    // string s = "Test: FieldTwo";
                }
            }
            if (e.Column.FieldName == "BudgetMarginOC")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalBudgetedMargin;
                        e.Value = result;
                    }
                    else
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin;
                        e.Value = result;
                    }
                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("margin"));
                }
            }
            if (e.Column.FieldName == "BudgetedMarginME")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("ExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {

                        var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalBudgetedMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("ExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) * exchangerate;
                        e.Value = result;
                    }


                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("BudgetedMargininBase"));
                }
            }
            if (e.Column.FieldName == "BudgetedMarginSE")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {

                        var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalBudgetedMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("marginExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) * exchangerate;
                        e.Value = result;
                    }
                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesBudgetedMargin"));
                }
            }
            if (e.Column.FieldName == "BudgetedMarginPercentAge")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var total = Convert.ToDecimal(SO.costCenterAmount);
                        var result = ((Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalBudgetedMargin) / total) * 100;
                        e.Value = result;
                    }
                    else
                    {

                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                        var result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) / total) * 100;
                        e.Value = result;
                    }
                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("BudgetedMarginPercent"));
                }
            }
            if (e.Column.FieldName == "ActualMarginOC")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var result = Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalActualMargin;
                        e.Value = result;
                    }
                    else
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin;
                        e.Value = result;
                    }

                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMargin"));
                }
            }
            if (e.Column.FieldName == "ActualMarginME")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("ExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalActualMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("ExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) * exchangerate;
                        e.Value = result;
                    }
                }

                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMargininBase"));
                }
            }
            if (e.Column.FieldName == "ActualMarginSE")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalActualMargin) * exchangerate;
                        e.Value = result;

                    }
                    else
                    {
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("marginExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) * exchangerate;
                        e.Value = result;
                    }

                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesActualMargin"));
                }
            }
            if (e.Column.FieldName == "ActualMarginPercentAge")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var total = Convert.ToDecimal(SO.costCenterAmount);
                        var result = ((Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalActualMargin) / total) * 100;
                        e.Value = result;
                    }
                    else
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                        var result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) / total) * 100;
                        e.Value = result;
                    }

                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMarginPercent"));
                }
            }
            if (e.Column.FieldName == "RevisedMarginOC")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalRevisedMargin;
                        e.Value = result;
                    }
                    else
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin;
                        e.Value = result;
                    }

                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMargin"));
                }
            }
            if (e.Column.FieldName == "RevisedMarginME")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("ExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;

                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalRevisedMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("ExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) * exchangerate;
                        e.Value = result;
                    }

                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMargininBase"));
                }
            }
            if (e.Column.FieldName == "RevisedMarginSE")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var exchangerate = Convert.ToDecimal(SO.costCenterExchangeRate);
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalRevisedMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("marginExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) * exchangerate;
                        e.Value = result;
                    }

                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesRevisedMargin"));
                }
            }
            if (e.Column.FieldName == "RevisedMarginPercentAge")
            {
                if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                {
                    var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                    if (SO.saleOrdertype == InquiryType.SupplyCCC && SO.costCenterAmount != 0)
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var total = Convert.ToDecimal(SO.costCenterAmount);
                        var result = ((Convert.ToDecimal(SO.costCenterAmount) - Cost.TotalRevisedMargin) / total) * 100;
                        e.Value = result;
                    }
                    else
                    {
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                        var result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) / total) * 100;
                        e.Value = result;
                    }

                }
                else
                {
                    e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMarginPercent"));
                }

            }
            if (e.Column.FieldName == "InvoicedAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PO = purchaseOrderRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    var invoicedAmount = Convert.ToDecimal(PO.PurchaseInvoices.Sum(x => x.totalInvoiceAmount));
                    decimal amountWithTax = 0;
                    if (PO.PurchaseInvoices.Count > 0)
                        if (PO.tax != null)
                        {
                            amountWithTax = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(PO.tax.percentage)) / 100);
                        }
                        else
                        {
                            amountWithTax = invoicedAmount + Convert.ToDecimal(PO.totaltaxAmount);
                        }
                    e.Value = amountWithTax;
                }
            }
            if (e.Column.FieldName == "UninvoicedAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PO = purchaseOrderRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    var invoicedAmount = Convert.ToDecimal(PO.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount));
                    decimal amountWithTax = 0;
                    if (PO.PurchaseInvoices.Where(x => x.isVoid != true).ToList().Count > 0)
                    {
                        if (PO.tax != null && PO.billWithTax != null)
                        {
                            invoicedAmount = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(PO.tax.percentage)) / 100);
                            amountWithTax = Convert.ToDecimal(PO.billWithTax.Value) - invoicedAmount;
                        }
                        if (PO.totaltaxAmount != 0)
                        {
                            invoicedAmount = invoicedAmount + Convert.ToDecimal(PO.totaltaxAmount);
                            amountWithTax = Convert.ToDecimal(PO.billWithTax.Value) - invoicedAmount;
                        }
                        else
                        {
                            amountWithTax = Convert.ToDecimal(PO.totalCFRValue) - invoicedAmount;
                        }

                    }
                    else
                    {
                        if (PO.tax != null && PO.billWithTax != null)
                        {
                            amountWithTax = Convert.ToDecimal(PO.billWithTax.Value);
                        }
                        else
                        {
                            amountWithTax = Convert.ToDecimal(PO.totalCFRValue);
                        }
                    }
                    e.Value = amountWithTax;
                }
            }
            if (e.Column.FieldName == "PaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PO = purchaseOrderRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    decimal amountWithTax = 0;
                    if (PO.PurchaseInvoices.Count > 0)
                        foreach (var _PI in PO.PurchaseInvoices)
                        {
                            amountWithTax = amountWithTax + Convert.ToDecimal(_PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount));
                        }
                    //if (PO.tax != null)
                    //{
                    //    amountWithTax = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(PO.tax.percentage)) / 100);
                    //}
                    //else
                    //{
                    //    amountWithTax = invoicedAmount;
                    //}
                    e.Value = amountWithTax;
                }
            }
            if (e.Column.FieldName == "UnpaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PO = purchaseOrderRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    decimal unPaidAmount = 0;
                    decimal amountWithTax = 0;
                    if (PO.PurchaseInvoices.Count > 0)
                        foreach (var _PI in PO.PurchaseInvoices)
                        {
                            amountWithTax = Convert.ToDecimal(amountWithTax + Convert.ToDecimal(_PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)));
                        }
                    if (PO.billWithTax != null)
                    {
                        unPaidAmount = Convert.ToDecimal(PO.billWithTax) - amountWithTax;
                    }
                    else
                    {
                        unPaidAmount = Convert.ToDecimal(PO.totalCFRValue + PO.totaltaxAmount) - amountWithTax;
                    }
                    e.Value = unPaidAmount;
                }
            }
            if (e.Column.FieldName == "CMER")
            {
                var purchaseOrder = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                double todayRate = 0;
                var exchangeRateGroupMER = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == purchaseOrder.currency_Id && x.base_currency_Id == purchaseOrder.company.CurrencyId && x.TargetYear == DateTime.Now.Year);
                if (exchangeRateGroupMER != null)
                {
                    exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == purchaseOrder.company_Id);
                    switch (DateTime.Now.Month)
                    {
                        case 1:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateJan;
                            break;
                        case 2:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateFeb;
                            break;
                        case 3:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateMar;
                            break;
                        case 4:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateApr;
                            break;
                        case 5:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateMay;
                            break;
                        case 6:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateJun;
                            break;
                        case 7:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateJul;
                            break;
                        case 8:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateAug;
                            break;
                        case 9:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateSep;
                            break;
                        case 10:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateOct;
                            break;
                        case 11:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateNov;
                            break;
                        case 12:
                            if (exchangeRate != null)
                                todayRate = exchangeRate.rateDec;
                            break;
                        default:
                            if (exchangeRate != null)
                                todayRate = 0;
                            break;
                    }
                }
                var total = todayRate;
                e.Value = total;
            }
            if (e.Column.FieldName == "SOAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var PO = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if (PO.saleOrder_Id != null)
                    {
                        e.Value = PO.SaleOrder.totalCFRValue;
                    }
                }
            }
            if (e.Column.FieldName == "SOInvoiced")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var PO = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;

                    if (PO.saleOrder_Id != null)
                    {
                        e.Value = PO.SaleOrder.SaleInvoices.Sum(x => x.totalInvoiceAmount);
                    }
                }
            }
            if (e.Column.FieldName == "daysLeft")
            {
                DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                var PO = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (PO.ExpectedPayment != null)
                {
                    var date = (DateTime)PO.ExpectedPayment;
                    DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                    var timeSpan = currDate.Subtract(targetDate);
                    e.Value = timeSpan.Days;
                }
            }
            if (e.Column.FieldName == "SODateAgeing" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row.saleOrder_Id != null)
                {
                    DateTime dateTime = (DateTime)row.SaleOrder.saleOrderDate;
                    Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                    NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                    e.Value = NoDueAgeingDays;
                }

            }
            if (e.Column.FieldName == "soDeliveryAging" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row.saleOrder_Id != null)
                {
                    if (row.SaleOrder.deliveryDate != null)
                    {
                        DateTime dateTime = (DateTime)row.SaleOrder.deliveryDate;
                        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                        e.Value = NoDueAgeingDays;
                    }
                }

            }
            if (e.Column.FieldName == "soDeliveryFinalAging" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row.saleOrder_Id != null)
                {
                    if (row.SaleOrder.deliveryDateFinal != null)
                    {
                        DateTime dateTime = (DateTime)row.SaleOrder.deliveryDateFinal;
                        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                        e.Value = NoDueAgeingDays;
                    }
                }

            }
            if (e.Column.FieldName == "revisedShipmentAging" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row.saleOrder_Id != null)
                {
                    if (row.RevisedShipmentDate != null)
                    {
                        DateTime dateTime = (DateTime)row.RevisedShipmentDate;
                        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                        e.Value = NoDueAgeingDays;
                    }
                }

            }
            if (e.Column.FieldName == "ocShipmentAging" && e.IsGetData)
            {
                var row = grdPurchaseOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                if (row.saleOrder_Id != null)
                {
                    if (row.ShipmentDate != null)
                    {
                        DateTime dateTime = (DateTime)row.ShipmentDate;
                        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                        e.Value = NoDueAgeingDays;
                    }
                }

            }
        }

        private void GrdpurchaseOrder_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (tableView.CompactPanelShowMode == CompactPanelShowMode.CompactMode)
            //{
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.CompactMode;
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.Always;
            //}
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Purchase Orders Report") != null)
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
                DXMessageBox.Show("You don't have permission Share Purchase Orders Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
        }

        private void btnDepartmentFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > -1 && cmbxDepartmentTo.SelectedIndex > -1)
                {
                    switch (cmbxDepartmentFrom.SelectedIndex)
                    {
                        case 0:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 0:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 1:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 1:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {

                                case 1:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 2:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 3:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 4:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    DXMessageBox.Show("Please select Department Levels to Apply filter!");
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void cmbxDepartmentTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > cmbxDepartmentTo.SelectedIndex)
                {
                    cmbxDepartmentTo.SelectedIndex = -1;
                    DXMessageBox.Show("Please select greater department!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cmbxDepartmentFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbxDepartmentTo.SelectedIndex = -1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void mbtnDriectClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                PurchaseOrderRepo purchaseOrderrepo = new PurchaseOrderRepo();
                PurchaseOrderStatus oldStatus = new PurchaseOrderStatus();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null) ? true : false)
                {

                    if (grdPurchaseOrderReport.GetFocusedRow() != null)
                    {
                        UsersRepo usersRepo = new UsersRepo();
                        var row = grdPurchaseOrderReport.GetFocusedRow() as PurchaseOrder;
                        if (row.PurchaseOrderStatus != null)
                        {
                            oldStatus = row.PurchaseOrderStatus;
                        }
                        PurchaseOrderss.ucStatuschange.inActiveStatuses = 1;

                        PurchaseOrderss.ucStatuschange.purchaseOrderid = (int)grdPurchaseOrderReport.GetFocusedRowCellValue(grdPurchaseOrderReport.Columns.GetColumnByFieldName("Id"));
                        PurchaseOrderss.frmPurchaseOrderStatusChange statusChange = new PurchaseOrderss.frmPurchaseOrderStatusChange(purchaseOrderrepo);
                        var myWindow = Window.GetWindow(this);
                        statusChange.Owner = myWindow;
                        statusChange.ShowDialog();
                        //if (PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Id == row.PurchaseOrderStatus.Id)
                        //    return;
                        if (PurchaseOrderss.ucStatuschange.purchaseOrder.Id != 0)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseOrder") != null) ? true : false)
                            {
                                PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = false;
                                PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.purchaseOrderRepo.update(Inquiriess.ucStatuschange.purchaseOrder);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                            {
                                PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                if (PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing == null)
                                {
                                    PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;

                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null)
                            {
                                PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing != true)
                                {
                                    PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;

                                }
                                //PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;

                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                            }
                            else
                            {
                                PurchaseOrderss.ucStatuschange.purchaseOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                PurchaseOrderss.ucStatuschange.purchaseOrder.PendingForClosing = true;
                                usersRepo.Add(TransactionInfo.Closed, PurchaseOrderss.ucStatuschange.purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                            }
                        //PurchaseOrderss.ucStatuschange.purchaseOrder.user_Id = MainWindow.currentUserid;
                        PurchaseOrderss.ucStatuschange.purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                        PurchaseOrderss.ucStatuschange.purchaseOrder.ClosingDate = System.DateTime.Now;
                        if (row.PurchaseOrderStatus != PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus)
                            usersRepo.Add(TransactionInfo.Status_Changed, row.Id, (int)TransactionItemType.Purchase_Order, "While direct closing Status Changed from (" + row.PurchaseOrderStatus.Status + ") to (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                        //Adding signature (comment)

                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("PO has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Purchase_Order);
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
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
                        string newStat = PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status;
                        string symbolCurr = "";
                        if (row.currency != null)
                        {
                            symbolCurr = row.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of PO (Amount OC) having value: " + row.totalCFRValue.ToString() + "(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(row.Id, TransactionItemType.Purchase_Order, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);

                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in PO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Order, comment.Comment, user.id, "New Comment ", null);
                            }
                        }

                        row = PurchaseOrderss.ucStatuschange.purchaseOrder;
                        grdPurchaseOrderReport.RefreshData();
                        purchaseOrderrepo.updateStatus(row.Id, row.PurchaseOrderStatus);
                        //PurchaseOrderss.ucStatuschange.UpdatePurchaseOrder();//purchaseOrderRepo.update(PurchaseOrderss.ucStatuschange.purchaseOrder);
                        MessageBox.Show("PurchaseOrder status changed to InActive (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");

                    }

                }
                else
                {
                    MessageBox.Show("You are not Allowed to Close PurchaseOrder Directly.");
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
                PurchaseOrderRepo purchaseOrderrepo = new PurchaseOrderRepo();
                PurchaseOrderStatus oldStatus = new PurchaseOrderStatus();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null) ? true : false)
                {
                    if (grdPurchaseOrderReport.GetFocusedRow() != null)
                    {
                        PurchaseOrder purchaseOrder = new PurchaseOrder();
                        purchaseOrder = grdPurchaseOrderReport.SelectedItem as PurchaseOrder;

                        UsersRepo usersRepo = new UsersRepo();

                        if (purchaseOrder.isApproved != true)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseOrder") != null) ? true : false)
                            {
                                purchaseOrder.isApproved = true;
                                purchaseOrder.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                            {
                                purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                if (purchaseOrder.isApproved == null)
                                {
                                    purchaseOrder.isApproved = false;
                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null)
                            {
                                purchaseOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (purchaseOrder.isApproved == null)
                                {
                                    purchaseOrder.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                            }
                            else
                            {
                                purchaseOrder.isApproved = false;
                            }
                        else //reapprove when it is approved already
                        {
                            if (purchaseOrder.isReApproved == false)
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added PurchaseOrder") != null) ? true : false)
                                {
                                    purchaseOrder.isReApproved = true;
                                    purchaseOrder.stage = TransactionStage.Approved.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseOrder") != null)
                                {
                                    purchaseOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                    if (purchaseOrder.isReApproved == null)
                                    {
                                        purchaseOrder.isReApproved = false;

                                    }

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseOrder") != null)
                                {
                                    purchaseOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (purchaseOrder.isReApproved == null)
                                    {
                                        purchaseOrder.isReApproved = false;

                                    }

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, purchaseOrder.Id, (int)TransactionItemType.Purchase_Order, frmInputBox.comment);
                                }
                                else
                                {
                                    purchaseOrder.isReApproved = false;
                                }

                        }


                        purchaseOrderrepo.update(purchaseOrder);
                        MessageBox.Show("PurchaseOrder is Approved (" + purchaseOrder.Id + ")");
                        SystemLog.LogInfo(this.GetType(), "PurchaseOrder is Approved (" + purchaseOrder.Id + ")");
                    }
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Approve PurchaseOrder Directly");
                    SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve PurchaseOrder Directly user id=(" + MainWindow.currentUserid + ")");

                }
            }
            catch (Exception)
            {

                throw;
            }
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
    }
}
