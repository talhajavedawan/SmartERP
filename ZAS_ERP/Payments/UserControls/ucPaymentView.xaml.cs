using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
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
using ZAS_ERP.Payments.UserControls.CompanyLoanPayments;
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.Payments.UserControls;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucPaymentView.xaml
    /// </summary>
    public partial class ucPaymentView : Window
    {
        private PaymentRepo paymentRepo;
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        GridReport report = new GridReport();
        string reportTitle;
        public MainWindow myParent = null;

        public ucPaymentView()
        {
            InitializeComponent();

        }
        public ucPaymentView(GridReport reportToEdit, string title)
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
            Loader.DeferedVisibility = true;
            PaymentRepo paymentRepo = new PaymentRepo();
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
                            grdPaymentRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Payments" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Payments" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Payment Register")
                            { grdPaymentRegister.ItemsSource = paymentRepo.getPaymentRegister(SYSTEM_STATIC.currentUser.id); }
                            else
                                grdPaymentRegister.ItemsSource = paymentRepo.GetAllOpenPayments(SYSTEM_STATIC.currentUser.id);

                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here     
                            mbtnExportToMemorizedReport1.IsVisible = false;
                            grdPaymentRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Payments" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Payments" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Payment Register")
                            { grdPaymentRegister.ItemsSource = paymentRepo.getPaymentRegister(SYSTEM_STATIC.currentUser.id); }
                            else
                                grdPaymentRegister.ItemsSource = paymentRepo.GetAllOpenPayments(SYSTEM_STATIC.currentUser.id);
                        }
                    }
                    break;
            }
            Loader.DeferedVisibility = false;
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdPaymentRegister.View);
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
                                    ReportLogic.SaveGridReport(grdPaymentRegister, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.SaveGridReport(grdPaymentRegister, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                        ReportLogic.RenameGridReport(grdPaymentRegister, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                        ReportLogic.RenameGridReport(grdPaymentRegister, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                    ReportLogic.UpdateGridReport(grdPaymentRegister, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdPaymentRegister, str, reportType, groupDetails, report.Id, report.settingkey);

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

                                ReportLogic.DeleteReport(grdPaymentRegister, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                                ReportLogic.DeleteReport(grdPaymentRegister, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

            GridReportRepo repo = new GridReportRepo();
            report = repo.GetReportById(report.Id);
            var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {

                grdPaymentRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
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
        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            var repo = new GridReportRepo();
            paymentRepo = new PaymentRepo();
            string path = this.Title;
            int pos = path.LastIndexOf("/") + 1;
            var reportTitlePath = path.Substring(pos, path.Length - pos);
            GridReport report = repo.GetReportByName(reportTitlePath);
            //PurchaseInvoiceRepo purchaseInvoiceRepo = new PurchaseInvoiceRepo();
            if (report.settingkey == "Payment Register")
            { grdPaymentRegister.ItemsSource = paymentRepo.getPaymentRegister(SYSTEM_STATIC.currentUser.id); }
            else
            { grdPaymentRegister.ItemsSource = paymentRepo.GetAllOpenPayments(SYSTEM_STATIC.currentUser.id); }
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
                                    ReportLogic.SaveGridReport(grdPaymentRegister, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.ExportToStandard(grdPaymentRegister, reportName, reportType, reportGroup, report.settingkey);
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

        private void GrdPaymentList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            switch (row.transactionType)
            {
                case PaymentTransactionType.Admin_Bills:
                    if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();

                            if (row.adminBill != null)
                            {
                                var node = row.adminBill.department;

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

                    }
                    if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.adminBill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.adminBill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.adminBill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.adminBill.department;

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
                    break;
                case PaymentTransactionType.Loans_Advances:
                    if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.loansAdvance.department;

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
                    break;
                case PaymentTransactionType.Purchase_Invoice:
                    if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.purchaseInvoice.department;

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
                    break;
                case PaymentTransactionType.Vendor_Bills:
                    if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                    {
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                        if (row != null)
                        {
                            var deptList = new List<Department>();
                            var node = row.Bill.department;

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
                    break;
            }

            //if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {
            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First(); //get first item from department list

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }
            //        }
            //        deptList.Reverse();
            //        e.Value = deptList[0].DeptName;
            //    }

            //}
            //if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {
            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First();

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }

            //        }


            //        deptList.Reverse();

            //        switch (deptList.Count)
            //        {
            //            case 0:

            //                break;
            //            case 1:
            //                e.Value = deptList[0].DeptName;
            //                break;
            //            case 2:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 3:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 4:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 5:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //        }
            //        //if (deptList.Count == 1)
            //        //{
            //        //    e.Value = deptList[0].DeptName;
            //        //}
            //        //if (deptList.Count == 2)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}
            //        //if (deptList.Count == 3)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}
            //        //if (deptList.Count == 4)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}
            //        //if (deptList.Count == 5)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}

            //        //int index = 1;
            //        //if (index >= 0 && index < deptList.Count)
            //        //{

            //        //    e.Value = deptList[1].DeptName;
            //        //}

            //        //if (deptList.Count <= 1)
            //        //{
            //        //    e.Value = dep;
            //        //}
            //        //else
            //        //{
            //        //    int index = 1;
            //        //    if (index >= 0 && index < deptList.Count)
            //        //    {

            //        //        e.Value = deptList[1].DeptName;
            //        //        dep = deptList[1].DeptName;
            //        //    }
            //        //}

            //    }

            //}
            //if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {
            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First();

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }

            //        }
            //        deptList.Reverse();

            //        switch (deptList.Count)
            //        {
            //            case 0:

            //                break;
            //            case 1:
            //                e.Value = deptList[0].DeptName;
            //                break;
            //            case 2:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 3:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //            case 4:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //            case 5:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //        }

            //        //if (deptList.Count == 1)
            //        //{
            //        //    e.Value = deptList[0].DeptName;
            //        //}
            //        //if (deptList.Count == 2)
            //        //{
            //        //    e.Value = deptList[1].DeptName;
            //        //}
            //        //if (deptList.Count == 3)
            //        //{
            //        //    e.Value = deptList[2].DeptName;
            //        //}
            //        //if (deptList.Count == 4)
            //        //{
            //        //    e.Value = deptList[2].DeptName;
            //        //}
            //        //if (deptList.Count == 5)
            //        //{
            //        //    e.Value = deptList[2].DeptName;
            //        //}


            //        //if (deptList.Count <= 2)
            //        //{
            //        //    e.Value = dep;
            //        //}
            //        //else
            //        //{
            //        //    int index = 2;
            //        //    if (index >= 0 && index < deptList.Count)
            //        //    {

            //        //        e.Value = deptList[2].DeptName;
            //        //        dep = deptList[2].DeptName;
            //        //    }
            //        //}


            //    }

            //}
            //if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {
            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First();

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }

            //        }
            //        deptList.Reverse();

            //        switch (deptList.Count)
            //        {
            //            case 0:

            //                break;
            //            case 1:
            //                e.Value = deptList[0].DeptName;
            //                break;
            //            case 2:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 3:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //            case 4:
            //                e.Value = deptList[3].DeptName;
            //                break;
            //            case 5:
            //                e.Value = deptList[3].DeptName;
            //                break;
            //        }

            //    }

            //}
            //if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
            //{
            //    var row = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
            //    if (row != null)
            //    {

            //        var deptList = new List<Department>();
            //        var node = row.departments;
            //        if (node.Count > 0)
            //        {
            //            var fDepartm = node.First();

            //            while (fDepartm != null)
            //            {
            //                if (fDepartm.ParentID != null)
            //                {
            //                    if (fDepartm.ParentID != fDepartm.Id)
            //                    {
            //                        //this will add current node to department list and tranverse to its parent
            //                        deptList.Add(fDepartm);
            //                        fDepartm = fDepartm.parentDepartment;
            //                    }
            //                    else
            //                    {
            //                        //when node is parent to itself
            //                        deptList.Add(fDepartm);
            //                        break;
            //                    }
            //                }
            //                else
            //                {
            //                    //parent with parent id is null
            //                    deptList.Add(fDepartm);
            //                    break;
            //                }

            //            }







            //            //foreach (var _dept in fDepartm) 
            //            //{
            //            //    var ddept = _dept.parentDepartment;

            //            //    while (ddept != null)
            //            //    {
            //            //        if (ddept.ParentID != null)
            //            //        {
            //            //            if (ddept.ParentID != ddept.Id)
            //            //            {
            //            //                //this will add current node to department list and tranverse to its parent
            //            //                deptList.Add(ddept);
            //            //                ddept = ddept.parentDepartment;
            //            //            }
            //            //            else
            //            //            {
            //            //                //when node is parent to itself
            //            //                deptList.Add(ddept);
            //            //                break;
            //            //            }
            //            //        }
            //            //        else
            //            //        {
            //            //            //parent with parent id is null
            //            //            deptList.Add(ddept);
            //            //            break;
            //            //        }
            //            //    }
            //            //}
            //        }



            //        deptList.Reverse();

            //        switch (deptList.Count)
            //        {
            //            case 0:

            //                break;
            //            case 1:
            //                e.Value = deptList[0].DeptName;
            //                break;
            //            case 2:
            //                e.Value = deptList[1].DeptName;
            //                break;
            //            case 3:
            //                e.Value = deptList[2].DeptName;
            //                break;
            //            case 4:
            //                e.Value = deptList[3].DeptName;
            //                break;
            //            case 5:
            //                e.Value = deptList[4].DeptName;
            //                break;
            //        }

            //    }

            //}


            if (e.Column.FieldName == "Depts" && e.IsGetData)
            {
                var pymnt = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string deptNames = "";

                if (pymnt.departments != null && pymnt.departments.Count > 0)
                {
                    deptNames = String.Join(" | ", pymnt.departments.Select(x => x.DeptName));
                }
                e.Value = deptNames;
            }
            if (e.Column.FieldName == "Vendor1" && e.IsGetData)
            {
                var pymnt = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string vendor = "";

                if (pymnt.vendor != null && pymnt.vendor.company != null)
                    vendor = pymnt.vendor.company.CompanyName;
                else
                {
                    if (pymnt.transactionType == PaymentTransactionType.Admin_Bills && pymnt.adminBill != null && pymnt.adminBill.vendor != null)
                        vendor = pymnt.adminBill.vendor.company.CompanyName;
                    else if (pymnt.transactionType == PaymentTransactionType.Vendor_Bills && pymnt.Bill.vendor != null)
                        vendor = pymnt.Bill.vendor.company.CompanyName;
                    else if (pymnt.transactionType == PaymentTransactionType.Purchase_Invoice && pymnt.purchaseInvoice.PurchaseOrder != null && pymnt.purchaseInvoice.PurchaseOrder.vendors != null && pymnt.purchaseInvoice.PurchaseOrder.vendors.Count > 0)
                        vendor = pymnt.purchaseInvoice.PurchaseOrder.vendors[0].company.CompanyName;
                }

                e.Value = vendor;
            }
            if (e.Column.FieldName == "Customer" && e.IsGetData)
            {
                var pymnt = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                string customer = "";


                if (pymnt.transactionType == PaymentTransactionType.Vendor_Bills && pymnt.Bill.customerCompany != null)
                    customer = pymnt.Bill.customerCompany.company.CompanyName;
                else if (pymnt.transactionType == PaymentTransactionType.Purchase_Invoice && pymnt.purchaseInvoice.PurchaseOrder != null && pymnt.purchaseInvoice.PurchaseOrder.customerCompany != null)
                    customer = pymnt.purchaseInvoice.PurchaseOrder.customerCompany.company.CompanyName;


                e.Value = customer;
            }
            if (e.Column.FieldName == "SystemCost" && e.IsGetData)
            {
                var pymnt = grdPaymentRegister.GetRowByListIndex(e.ListSourceRowIndex) as Payment;
                if (pymnt.CostSheetId != null)
                {
                    var systemCost = saleOrderRepo.GetPaymentSystemCost((int)pymnt.CostSheetId, pymnt.Id);
                    e.Value = systemCost;
                }

            }
        }

        private void GrdPaymentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var selectedPayment = grdPaymentRegister.SelectedItem as Payment;

                if (selectedPayment != null)
                {
                    CompanyRepo compRepo = new CompanyRepo();
                    switch (selectedPayment.transactionType)
                    {
                        case PaymentTransactionType.Admin_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Admin Bill Payment") != null)
                            {
                                ucFrmPayments frmPayments = new ucFrmPayments();

                                paymentRepo = new PaymentRepo();

                                frmPayments = new ucFrmPayments();
                                var pymnt = paymentRepo.GetPaymentByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;


                                if (pymnt.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPayments.editFlag = true;
                                        frmPayments.groupId = selectedPayment.transactionGroupId;
                                        frmPayments.frmPaymentWindow.Content = frmPayments;
                                        //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                        //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                        frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                        //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                        //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                        frmPayments.frmPaymentWindow.Title = "Payments";
                                        frmPayments.frmPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPayments.editFlag = true;
                                    frmPayments.groupId = selectedPayment.transactionGroupId;
                                    frmPayments.frmPaymentWindow.Content = frmPayments;
                                    //ucFrmBankTransfer.frmBankTranfer.Height = 700;
                                    //ucFrmBankTransfer.frmBankTranfer.ResizeMode = ResizeMode.CanMinimize;
                                    frmPayments.frmPaymentWindow.WindowState = WindowState.Maximized;
                                    //ucFrmBankTransfer.frmBankTranfer.MinHeight = 800;
                                    //ucFrmBankTransfer.frmBankTranfer.MinWidth = 800;
                                    frmPayments.frmPaymentWindow.Title = "Payments";
                                    frmPayments.frmPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Admin Bill Payments!");
                            }

                            break;

                        case PaymentTransactionType.Vendor_Bills:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Payment") != null)
                            {
                                ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd();

                                //paymentRepo = new PaymentRepo();
                                //frmBillPayments.payments = paymentRepo.GetVendorBillPaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment != null)
                                {
                                    if (selectedPayment.Status.isActive == false)
                                    {
                                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                        {
                                            frmBillPayments.editFlag = true;
                                            frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                            frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;

                                            frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                            frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                            frmBillPayments.frmBillPaymentWindow.Show();
                                        }
                                        else
                                        {
                                            DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        frmBillPayments.editFlag = true;
                                        frmBillPayments.groupId = selectedPayment.transactionGroupId;
                                        frmBillPayments.frmBillPaymentWindow.Content = frmBillPayments;
                                        frmBillPayments.frmBillPaymentWindow.WindowState = WindowState.Maximized;
                                        frmBillPayments.frmBillPaymentWindow.Title = "Payments";
                                        frmBillPayments.frmBillPaymentWindow.Show();
                                    }
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Vendor Bill Payment!");
                            }

                            break;

                        case PaymentTransactionType.Purchase_Invoice:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Purchase Invoice Payment") != null)
                            {
                                ucFrmPInvoicePaymentAdd frmPIpayment = new ucFrmPInvoicePaymentAdd();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmPIpayment.editFlag = true;
                                        frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                        frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                        frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                        frmPIpayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmPIpayment.editFlag = true;
                                    frmPIpayment.groupId = selectedPayment.transactionGroupId;
                                    frmPIpayment.frmPiPaymentWindow.Content = frmPIpayment;
                                    frmPIpayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmPIpayment.frmPiPaymentWindow.Title = "Payments";
                                    frmPIpayment.frmPiPaymentWindow.Show();
                                }

                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                            }


                            break;
                        case PaymentTransactionType.Loans_Advances:

                            switch (selectedPayment.loansAdvance.advanceTemplate)
                            {
                                case LoansAdvanceTemplate.Loan:
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                    {
                                        ucFrmCompanyLoanPayment frmLApayment = new ucFrmCompanyLoanPayment();
                                        //paymentRepo = new PaymentRepo();
                                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                        if (selectedPayment.Status.isActive == false)
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                            {
                                                frmLApayment.editFlag = true;
                                                frmLApayment.groupId = selectedPayment.transactionGroupId;
                                                frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                                frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                                frmLApayment.frmPiPaymentWindow.Show();
                                            }
                                            else
                                            {
                                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            frmLApayment.editFlag = true;
                                            frmLApayment.groupId = selectedPayment.transactionGroupId;
                                            frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                            frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                            frmLApayment.frmPiPaymentWindow.Show();
                                        }

                                    }
                                    else
                                    {
                                        DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                                    }
                                    break;
                                default:
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances Payment") != null)
                                    {
                                        ucFrmLoansAdvancePaymentAdd frmLApayment = new ucFrmLoansAdvancePaymentAdd();
                                        //paymentRepo = new PaymentRepo();
                                        //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                        if (selectedPayment.Status.isActive == false)
                                        {
                                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                            {
                                                frmLApayment.editFlag = true;
                                                frmLApayment.groupId = selectedPayment.transactionGroupId;
                                                frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                                frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                                frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                                frmLApayment.frmPiPaymentWindow.Show();
                                            }
                                            else
                                            {
                                                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            frmLApayment.editFlag = true;
                                            frmLApayment.groupId = selectedPayment.transactionGroupId;
                                            frmLApayment.frmPiPaymentWindow.Content = frmLApayment;
                                            frmLApayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                            frmLApayment.frmPiPaymentWindow.Title = "Payments";
                                            frmLApayment.frmPiPaymentWindow.Show();
                                        }

                                    }
                                    else
                                    {
                                        DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                                    }
                                    break;
                            }



                            break;
                        case PaymentTransactionType.Target_Reward:

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Rewards Payment") != null)
                            {
                                ucFrmTargetRewardPayment frmTRPayment = new ucFrmTargetRewardPayment();
                                //paymentRepo = new PaymentRepo();
                                //frmPIpayment.payments = paymentRepo.GetPIpaymentsByGroupId(selectedPayment.transactionGroupId); //grdCntrlInterBankTransfer.SelectedItem as InterBankTransfer;

                                if (selectedPayment.Status.isActive == false)
                                {
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Payment") != null)
                                    {
                                        frmTRPayment.editFlag = true;
                                        frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                        frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                        frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                        frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                        frmTRPayment.frmPiPaymentWindow.Show();
                                    }
                                    else
                                    {
                                        DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to View or Edit InActive Payments!");
                                        return;
                                    }
                                }
                                else
                                {
                                    frmTRPayment.editFlag = true;
                                    frmTRPayment.groupId = selectedPayment.transactionGroupId;
                                    frmTRPayment.frmPiPaymentWindow.Content = frmTRPayment;
                                    frmTRPayment.frmPiPaymentWindow.WindowState = WindowState.Maximized;
                                    frmTRPayment.frmPiPaymentWindow.Title = "Payments";
                                    frmTRPayment.frmPiPaymentWindow.Show();
                                }
                            }
                            else
                            {
                                DXMessageBox.Show("Permission required to View Purchase Invoice Payment!");
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Pyment Report") != null)
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
                ProcurementRepo procurementRepo = new ProcurementRepo();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                PaymentStatus statusChanged = new PaymentStatus();
                UsersRepo usersRepo = new UsersRepo();
                paymentRepo = new PaymentRepo();
                var selectedRow = (Payment)grdPaymentRegister.GetFocusedRow();


                if (selectedRow != null)
                {
                    var previous_status = selectedRow.Status.Status;
                    var payments = paymentRepo.GetPaymentForApproval(selectedRow.transactionGroupId);
                    if (payments != null && payments.Count > 0)
                    {
                        List<Department> deptss = new List<Department>();
                        if (selectedRow.departments != null)
                            deptss = selectedRow.departments;

                        if (payments[0].isApproved == false)
                        {
                            DXMessageBox.Show("Payments are pending for approval!");
                            return;
                        }
                        statusChanged = null;
                        ucFrmPaymentDirectClose ucFrmDirectClose = new ucFrmPaymentDirectClose();
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

                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Payment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Payment without Approval") != null)
                            {
                                for (int i = 0; i < payments.Count; i++)
                                {
                                    payments[i].PendingForClosing = false;
                                    payments[i].stage = TransactionStage.Closed.ToString();
                                    payments[i].statusId = statusChanged.Id;
                                    payments[i].LastStatusChangeDate = System.DateTime.Now;
                                    payments[i].ClosingDate = System.DateTime.Now;


                                }
                                paymentRepo.ApprovePayment(payments, deptss);
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Payments, frmInputBox.comment);

                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Payments having system ref #: " + payments[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (payments.Count != 0)
                                    {
                                        procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payemnts #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);
                                            }
                                        }
                                    }
                                }
                                //Load_Receipts();
                            }
                            else
                            {
                                for (int i = 0; i < payments.Count; i++)
                                {
                                    payments[i].PendingForClosing = true;
                                    payments[i].stage = TransactionStage.AwaitingApproval.ToString();
                                    payments[i].statusId = statusChanged.Id;
                                    payments[i].LastStatusChangeDate = System.DateTime.Now;
                                    payments[i].ClosingDate = System.DateTime.Now;


                                }
                                paymentRepo.ApprovePayment(payments, deptss);
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Payments, frmInputBox.comment);

                                if (previous_status != statusChanged.Status)
                                {
                                    string oldStat = previous_status;
                                    string newStat = statusChanged.Status;
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Payments having system ref #: " + payments[0].SystemRefNo + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    if (payments.Count != 0)
                                    {
                                        procurementRepo.Add(payments[0].transactionGroupId, TransactionItemType.Payments, comment, SYSTEM_STATIC.currentUser.employeeId);
                                        //Creating notification
                                        if (tagUsers.Count != 0)
                                        {
                                            foreach (var user in tagUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);

                                            }
                                        }

                                        if (ccUsers.Count != 0)
                                        {
                                            foreach (var user in ccUsers)
                                            {
                                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Payments #" + payments[0].SystemRefNo, payments[0].transactionGroupId, TransactionItemType.Payments, comment.Comment, user.id, "New Comment ", null);
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
                if (grdPaymentRegister.GetFocusedRow() != null)
                {
                    paymentRepo = new PaymentRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (Payment)grdPaymentRegister.GetFocusedRow();
                    var payments = paymentRepo.GetPaymentForApproval(selectedRow.transactionGroupId);
                    UsersRepo usersRepo = new UsersRepo();

                    if (payments != null && payments.Count > 0)
                    {
                        List<Department> deptss = new List<Department>();
                        if (selectedRow.departments != null)
                            deptss = selectedRow.departments;

                        if (payments[0].isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Payment") != null) ? true : false)
                            {
                                for (int i = 0; i < payments.Count; i++)
                                {
                                    payments[i].isApproved = true;
                                    payments[i].stage = TransactionStage.Approved.ToString();
                                    payments[i].ApprovedDate = DateTime.Now;
                                }
                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, 17, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                paymentRepo.ApprovePayment(payments, deptss);

                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Payments are Approved (" + selectedRow.transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Payment is Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Payments Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Payments Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (payments[0].isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Payment") != null) ? true : false)
                            {
                                for (int i = 0; i < payments.Count; i++)
                                {
                                    payments[i].isReApproved = true;
                                    payments[i].stage = TransactionStage.Approved.ToString();
                                    payments[i].ReApprovalDate = DateTime.Now;
                                }
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.transactionGroupId, (int)TransactionItemType.Payments, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                paymentRepo.ApprovePayment(payments, deptss);
                                //foreach (var _bill in bills)
                                //{
                                //    billsRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Payments are Approved (" + selectedRow.transactionGroupId + ")");
                                SystemLog.LogInfo(this.GetType(), "Payment is Approved (" + selectedRow.transactionGroupId + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Payments Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Payments Directly user id=(" + MainWindow.currentUserid + ")");
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
