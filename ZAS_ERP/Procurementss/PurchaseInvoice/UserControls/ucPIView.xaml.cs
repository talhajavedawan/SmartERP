using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Procurementss.PurchaseInvoice.UserControls
{
    /// <summary>
    /// Interaction logic for ucPIView.xaml
    /// </summary>
    public partial class ucPIView : Window
    {
        GridReport report = new GridReport();
        string reportTitle;
        public MainWindow myParent = null;
        PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();
        public ucPIView()
        {
            InitializeComponent();
        }
        public ucPIView(GridReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            Loader.DeferedVisibility = true;
            PurchaseInvoiceRepo poRepo = new PurchaseInvoiceRepo();
            GridReportRepo repo = new GridReportRepo();
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
                            grdpurchaseInvoice.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Purchase Invoices" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Purchase Invoices" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Purchase Invoice Register")
                            { grdpurchaseInvoice.ItemsSource = poRepo.getSaleInvoiceRegister(SYSTEM_STATIC.currentUser.id); }
                            else
                                grdpurchaseInvoice.ItemsSource = poRepo.getAll(SYSTEM_STATIC.currentUser.id);

                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here     
                            mbtnExportToMemorizedReport1.IsVisible = false;
                            grdpurchaseInvoice.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Purchase Invoices" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Purchase Invoices" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Purchase Invoice Register")
                            { grdpurchaseInvoice.ItemsSource = poRepo.getSaleInvoiceRegister(SYSTEM_STATIC.currentUser.id); }
                            else
                                grdpurchaseInvoice.ItemsSource = poRepo.getAll(SYSTEM_STATIC.currentUser.id);
                        }
                    }
                    break;
            }
            Loader.DeferedVisibility = false;
        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Stage" && e.IsGetData)
            {
                var purchaseInvoice = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
                if (purchaseInvoice.isVoid == true)
                {
                    e.Value = "Void";
                }
                else if (purchaseInvoice.isReApproved == false)
                {
                    e.Value = "Under Approval";
                }
                else if (purchaseInvoice.isApproved == true && purchaseInvoice.stage == "Closed")
                {
                    e.Value = "Closed";
                }
                else if (purchaseInvoice.isApproved == true && purchaseInvoice.PurchaseInvoiceStatus.isActive == false && purchaseInvoice.PendingForClosing != true)
                {
                    e.Value = "Closed";
                }
                else if (purchaseInvoice.isApproved == true && purchaseInvoice.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
                else if (purchaseInvoice.isApproved == true)
                {
                    e.Value = "Approved";
                }
                else if (purchaseInvoice.isApproved == false)
                {
                    e.Value = "Under Approval";
                }
                else if (purchaseInvoice.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
            }
            if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
            {
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
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
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
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
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
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
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
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
                var row = grdpurchaseInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.PurchaseInvoice;
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
                }
            }


            if (e.Column.FieldName == "PaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PO = purchaseInvoiceRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    var paidAmount = Convert.ToDecimal(PO.Payments.Where(x => x.isVoid != true).ToList().Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(PO.Payments.Where(x => x.isVoid != true).ToList().Sum(x => x.Deductions))*/;
                    decimal amountWithTax = 0;
                    if (PO.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                        if (PO.tax != null)
                        {
                            amountWithTax = paidAmount + (paidAmount * Convert.ToDecimal(PO.tax.percentage) / 100);
                        }
                        else
                        {
                            amountWithTax = paidAmount + Convert.ToDecimal(PO.totaltaxAmount);
                        }
                    e.Value = amountWithTax;
                }
            }

            if (e.Column.FieldName == "UnpaidAmount")
            {
                if (e.GetListSourceFieldValue("Id") != null)
                {
                    var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                    PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                    var PI = purchaseInvoiceRepo.getForGrid(id);
                    //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                    var paidAmount = Convert.ToDecimal(PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(PI.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
                    decimal amountWithTax = 0;
                    if (PI.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                    {
                        if (PI.PurchaseOrder.tax != null)
                        {
                            //paidAmount = paidAmount + ((paidAmount * Convert.ToDecimal(PI.PurchaseOrder.tax.percentage)) / 100);

                            amountWithTax = Math.Round(Convert.ToDecimal(PI.totalInvoiceAmount + (PI.totalInvoiceAmount * PI.PurchaseOrder.tax.percentage) / 100) - paidAmount, 2);
                        }
                        else
                        {
                            amountWithTax = Math.Round(Convert.ToDecimal(PI.totalInvoiceAmount + PI.totaltaxAmount) - paidAmount, 2);
                        }
                    }
                    else
                    {
                        if (PI.PurchaseOrder.tax != null)
                        {
                            amountWithTax = Math.Round(Convert.ToDecimal(PI.totalInvoiceAmount + (PI.totalInvoiceAmount * PI.PurchaseOrder.tax.percentage) / 100), 2);
                        }
                        else
                        {
                            amountWithTax = Convert.ToDecimal(PI.totalInvoiceAmount + PI.totaltaxAmount);
                        }
                    }
                    e.Value = amountWithTax;
                }
            }
        }

        private void GrdpurchaseInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Invoice, (int)grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();

            }
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdpurchaseInvoice.View);
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
                                    ReportLogic.SaveGridReport(grdpurchaseInvoice, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.SaveGridReport(grdpurchaseInvoice, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdpurchaseInvoice, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                        ReportLogic.RenameGridReport(grdpurchaseInvoice, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                    ReportLogic.UpdateGridReport(grdpurchaseInvoice, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdpurchaseInvoice, str, reportType, groupDetails, report.Id, report.settingkey);

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

                                ReportLogic.DeleteReport(grdpurchaseInvoice, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                                ReportLogic.DeleteReport(grdpurchaseInvoice, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                grdpurchaseInvoice.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
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
                                    ReportLogic.SaveGridReport(grdpurchaseInvoice, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.ExportToStandard(grdpurchaseInvoice, reportName, reportType, reportGroup, report.settingkey);
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

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            var repo = new GridReportRepo();
            string path = this.Title;
            int pos = path.LastIndexOf("/") + 1;
            var reportTitlePath = path.Substring(pos, path.Length - pos);
            GridReport report = repo.GetReportByName(reportTitlePath);
            PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();
            if (report.settingkey == "Purchase Invoice Register")
            { grdpurchaseInvoice.ItemsSource = purchaseInvoiceRepo.getSaleInvoiceRegister(SYSTEM_STATIC.currentUser.id); }
            else
            { grdpurchaseInvoice.ItemsSource = purchaseInvoiceRepo.getAll(SYSTEM_STATIC.currentUser.id); }
            loadingGif.Visibility = Visibility.Hidden;

        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Purchase Invoices Report") != null)
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
                DXMessageBox.Show("You don't have permission Share Purchase Invoices Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
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
                PurchaseInvoiceStatus oldStatus = new PurchaseInvoiceStatus();

                NotificationsRepo notificationsRepo = new NotificationsRepo();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null) ? true : false)
                {

                    if (grdpurchaseInvoice.GetFocusedRow() != null)
                    {
                        UsersRepo usersRepo = new UsersRepo();

                        var row = grdpurchaseInvoice.GetFocusedRow() as ERP_BL.Databases.PurchaseInvoice;



                        if (row.PurchaseInvoiceStatus != null)
                        {
                            oldStatus = row.PurchaseInvoiceStatus;
                        }
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoiceid = (int)grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("Id"));
                        ZAS_ERP.Procurementss.PurchaseInvoice.frmPurchaseInvoiceStatusChange statusChange = new ZAS_ERP.Procurementss.PurchaseInvoice.frmPurchaseInvoiceStatusChange();
                        var myWindow = Window.GetWindow(this);
                        statusChange.Owner = myWindow;
                        statusChange.ShowDialog();
                        if (ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoiceid != 0)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing PurchaseInvoice") != null) ? true : false)
                            {
                                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = false;
                                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.Id, 7, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.saleInvoiceRepo.update(Inquiriess.ucStatuschange.saleInvoice);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                            {
                                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                                if (ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing == null)
                                {
                                    ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = true;

                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.Id, 7, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null)
                            {
                                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing == null)
                                {
                                    ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = true;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.Id, 7, frmInputBox.comment);
                            }
                            else
                            {
                                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();

                                ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PendingForClosing = true;
                            }
                        //SaleInvoicess.ucStatuschange.saleInvoice.user_Id = MainWindow.currentUserid;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.ClosingDate = System.DateTime.Now;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.LastStatusChangeDate = System.DateTime.Now;
                        ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoiceRepo.updateForDirectClose(ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice);
                        PurchaseOrderss.ucStatuschange.purchaseOrderid = (int)grdpurchaseInvoice.GetFocusedRowCellValue(grdpurchaseInvoice.Columns.GetColumnByFieldName("purchaseOrder_Id"));
                        PurchaseOrderss.frmPurchaseOrderStatusChange statChange = new PurchaseOrderss.frmPurchaseOrderStatusChange();
                        PurchaseOrderss.ucStatuschange.inActiveStatuses = 0;                    //Show Only active SO Statusses

                        statChange.Owner = myWindow;
                        statChange.ShowDialog();
                        //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                        PurchaseOrderss.ucStatuschange.purchaseOrder.LastStatusChangeDate = System.DateTime.Now;
                        PurchaseOrderss.ucStatuschange.UpdatePurchaseOrderStatusforInvoice();//.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                        MessageBox.Show("PurchaseOrder status changed to InActive (" + PurchaseOrderss.ucStatuschange.purchaseOrder.PurchaseOrderStatus.Status + ")");
                        MessageBox.Show("PurchaseInvoice status changed to InActive (" + ZAS_ERP.Procurementss.PurchaseInvoice.UserControls.ucStatusChange.purchaseInvoice.PurchaseInvoiceStatus.Status + ")");

                        //Adding auto Signature
                        //if (row.saleInvoiceStatus.Status != SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status)
                        //{
                        UsersRepo userRepo = new UsersRepo();
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Purchase Invoice has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                            {
                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Purchase_Invoice);
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
                        string newStat = SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status;
                        string symbolCurr = "";
                        if (row.currency != null)
                        {
                            symbolCurr = row.currency.Abbrivation.ToString();
                        }
                        CommentLog comment = new CommentLog()
                        {
                            Comment = "Status of Purchase Invoice(Amount OC) having value: " + row.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            Timestamp = DateTime.Now,
                            Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                        };
                        procurementRepo.Add(row.Id, TransactionItemType.Purchase_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                        //Creating Comments
                        if (tagUsers.Count != 0)
                        {
                            foreach (var user in tagUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ", null);
                            }
                        }

                        if (ccUsers.Count != 0)
                        {
                            foreach (var user in ccUsers)
                            {
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Purchase Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Purchase_Invoice, comment.Comment, user.id, "New Comment ", null);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Close PurchaseInvoice Directly.");
                }
            }
            catch (Exception)
            {
            }
        }

        private void mbtnApprove_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null) ? true : false)
            {
                if (grdpurchaseInvoice.GetFocusedRow() != null)
                {
                    ERP_BL.Databases.PurchaseInvoice purchaseInvoice = new ERP_BL.Databases.PurchaseInvoice();
                    purchaseInvoice = grdpurchaseInvoice.SelectedItem as ERP_BL.Databases.PurchaseInvoice;

                    UsersRepo usersRepo = new UsersRepo();

                    if (purchaseInvoice.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added PurchaseInvoice") != null) ? true : false)
                        {
                            purchaseInvoice.isApproved = true;
                            purchaseInvoice.stage = TransactionStage.Approved.ToString();
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Adding, purchaseInvoice.Id, 7, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 PurchaseInvoice") != null)
                        {
                            purchaseInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                            if (purchaseInvoice.isApproved == null)
                            {
                                purchaseInvoice.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, purchaseInvoice.Id, 7, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 PurchaseInvoice") != null)
                        {
                            purchaseInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (purchaseInvoice.isApproved == null)
                            {
                                purchaseInvoice.isApproved = false;

                            }

                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, purchaseInvoice.Id, 7, frmInputBox.comment);
                        }
                        else
                        {
                            purchaseInvoice.isApproved = false;
                        }




                    purchaseInvoiceRepo.updateForDirectClose(purchaseInvoice);
                    MessageBox.Show("PurchaseInvoice is Approved (" + purchaseInvoice.Id + ")");
                    SystemLog.LogInfo(this.GetType(), "PurchaseInvoice is Approved (" + purchaseInvoice.Id + ")");
                }
            }
            else
            {
                MessageBox.Show("You are not Allowed to Approve PurchaseInvoice Directly");
                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve PurchaseInvoice Directly user id=(" + MainWindow.currentUserid + ")");

            }
        }
    }
}
