using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.LoansAdvances;
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
using ZAS_ERP.Procurementss.LoanAdvance.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.AdminBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.LoanAdvance.Windows
{
    /// <summary>
    /// Interaction logic for ucLoansView.xaml
    /// </summary>
    public partial class ucLoansView : DXWindow
    {//ucFrm frmBillAdd = new ucFrmBillAdd();
        GridReport report = new GridReport();
        string reportTitle;
        public MainWindow myParent = null;
        public ucLoansView()
        {
            InitializeComponent();
        }

        public ucLoansView(GridReport reportToEdit, string title)
        {
            this.DataContext = this;
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }

        private void grdLoansAdvancesRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = grdLoansList.SelectedItem as LoansAdvance;
            if (selectedItem != null)
            {
                switch (selectedItem.advanceTemplate)
                {
                    case LoansAdvanceTemplate.Advance:
                        switch (selectedItem.loansAdvanceType)
                        {

                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Loans and Advances") != null)
                                {
                                    ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();
                                    frmLoansAdvances.loansAdvanceId = selectedItem.Id;
                                    frmLoansAdvances.editFlag = true;
                                    Window win = new Window();
                                    win.Content = frmLoansAdvances;
                                    win.WindowState = WindowState.Maximized;
                                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    win.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;

                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Vendor Bill Loans Advances") != null)
                                {
                                    ucFrmBillLoansAdvance frmBillLoansAdvance = new ucFrmBillLoansAdvance();
                                    frmBillLoansAdvance.loansAdvanceId = selectedItem.Id;
                                    frmBillLoansAdvance.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmBillLoansAdvance;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;

                    case LoansAdvanceTemplate.Loan:
                        switch (selectedItem.loansAdvanceType)
                        {
                            case LoansAdvanceType.Admin_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                {
                                    ucFrmAdminBillLoan frmCompanyLoan = new ucFrmAdminBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                            case LoansAdvanceType.Vendor_Bill:
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Company Loans") != null)
                                {
                                    ucFrmVendorBillLoan frmCompanyLoan = new ucFrmVendorBillLoan();
                                    frmCompanyLoan.loansAdvanceId = selectedItem.Id;
                                    frmCompanyLoan.editFlag = true;
                                    Window wind = new Window();
                                    wind.Content = frmCompanyLoan;
                                    wind.WindowState = WindowState.Maximized;
                                    wind.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                                    wind.Show();
                                }
                                else
                                {
                                    DXMessageBox.Show("Permission denied!");
                                }
                                break;
                        }
                        break;
                }
            }
        }

        private void grdLoansAdvancesRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "Employee":
                    var item = grdLoansList.GetRowByListIndex(e.ListSourceRowIndex) as LoansAdvance;
                    if (item.ApplicantEmployee != null)
                        e.Value = item.ApplicantEmployee.person.FName + " " + item.ApplicantEmployee.person.LName;
                    break;

                case "BalanaceAmountOC":
                    var loansAdvance = grdLoansList.GetRowByListIndex(e.ListSourceRowIndex) as LoansAdvance;

                    if (loansAdvance.loansAdvanceType == LoansAdvanceType.Vendor_Bill)
                    {

                        double paidAmount = 0, receivedAmount = 0, adjustedAmount = 0;
                        if (loansAdvance.Payments != null && loansAdvance.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            paidAmount = loansAdvance.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount);
                        }

                        if (loansAdvance.SalesReceipts != null && loansAdvance.SalesReceipts.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            receivedAmount = loansAdvance.SalesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                        }

                        if (loansAdvance.bills != null && loansAdvance.bills.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            foreach (var bill in loansAdvance.bills.Where(x => x.isVoid != true))
                            {
                                adjustedAmount = adjustedAmount + bill.Adjustmentss.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                            }
                        }
                        e.Value = Math.Round(receivedAmount - paidAmount - adjustedAmount, 2);
                    }
                    else if (loansAdvance.loansAdvanceType == LoansAdvanceType.Admin_Bill)
                    {
                        double paidAmount = 0, receivedAmount = 0, adjustedAmount = 0;
                        if (loansAdvance.Payments != null && loansAdvance.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            paidAmount = loansAdvance.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount);
                        }

                        if (loansAdvance.SalesReceipts != null && loansAdvance.SalesReceipts.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            receivedAmount = loansAdvance.SalesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                        }

                        if (loansAdvance.AdminBills != null && loansAdvance.AdminBills.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            foreach (var adminBill in loansAdvance.AdminBills.Where(x => x.isVoid != true))
                            {
                                adjustedAmount = adjustedAmount + adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount);
                            }
                        }
                        e.Value = Math.Round(receivedAmount - paidAmount - adjustedAmount, 2);

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

                                ReportLogic.DeleteReport(grdLoansList, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                                ReportLogic.DeleteReport(grdLoansList, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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


        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdLoansList.View);
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
                                if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
                                {
                                    var reportGroup = saveAsNewReport.gridReportGroup;
                                    var reportType = saveAsNewReport.gridReportType;
                                    var reportName = saveAsNewReport.reportName;
                                    ReportLogic.SaveGridReport(grdLoansList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.SaveGridReport(grdLoansList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdLoansList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.reportName != null && updatedReport.userId != null)
                                {
                                    if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
                                    {
                                        ReportLogic.RenameGridReport(grdLoansList, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                    ReportLogic.UpdateGridReport(grdLoansList, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdLoansList, str, reportType, groupDetails, report.Id, report.settingkey);

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
                var repo = new GridReportRepo();

                report = repo.GetReportById(report.Id);
                grdLoansList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
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
            CompanyLoansRepo advanceRepo = new CompanyLoansRepo();
            if (report.settingkey == "Loans")
            { grdLoansList.ItemsSource = advanceRepo.GetAllLoansAdvances(SYSTEM_STATIC.currentUser.id); }
            else
            { grdLoansList.ItemsSource = advanceRepo.GetAllLoansAdvances(SYSTEM_STATIC.currentUser.id); }
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
                                if (exportToMemorizedReport != null && exportToMemorizedReport.gridReportGroup != null && exportToMemorizedReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToMemorizedReport.gridReportGroup;
                                    var reportType = exportToMemorizedReport.gridReportType;
                                    var reportName = exportToMemorizedReport.reportName;
                                    ReportLogic.SaveGridReport(grdLoansList, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                if (exportToStandardReport != null && exportToStandardReport.gridReportGroup != null && exportToStandardReport.reportName != null && report.userId != null)
                                {
                                    var reportGroup = exportToStandardReport.gridReportGroup;
                                    var reportType = exportToStandardReport.gridReportType;
                                    var reportName = exportToStandardReport.reportName;
                                    ReportLogic.ExportToStandard(grdLoansList, reportName, reportType, reportGroup, report.settingkey);
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
            CompanyLoansRepo advanceRepo = new CompanyLoansRepo();
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
                            grdLoansList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Company Loans" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Company Loans" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Company Loans Register")
                            {
                                grdLoansList.ItemsSource = advanceRepo.GetAllLoansAdvances(SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                grdLoansList.ItemsSource = advanceRepo.GetAllLoansAdvances(SYSTEM_STATIC.currentUser.id);
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
                            grdLoansList.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Company Loans" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Company Loans" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Company Loans Register")
                            {
                                grdLoansList.ItemsSource = advanceRepo.GetAllLoansAdvances(SYSTEM_STATIC.currentUser.id);
                            }
                            else
                            {
                                grdLoansList.ItemsSource = advanceRepo.GetAllLoansAdvances(SYSTEM_STATIC.currentUser.id);
                            }
                        }
                    }
                    break;
            }
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Loans Advances Report") != null)
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
                DXMessageBox.Show("You don't have permission Share Advances Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
        }


        private void mbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo usersRepo = new UsersRepo();
            CompanyLoansRepo loansAdvanceRepo = new CompanyLoansRepo();

            LoansAdvanceStatus statusChanged = new LoansAdvanceStatus();
            ProcurementRepo procurementRepo = new ProcurementRepo();
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            var selectedRow = (LoansAdvance)grdLoansList.GetFocusedRow();

            if (selectedRow != null)
            {
                var previous_status = selectedRow.Status.Status;
                var loansAdvance = loansAdvanceRepo.GetLoansAdvance(selectedRow.Id);
                if (loansAdvance != null)
                {
                    if (loansAdvance.isApproved == false)
                    {
                        DXMessageBox.Show("Transaction is under approval!");
                        return;
                    }

                    statusChanged = null;
                    frmLoansAdvanceStatusChange ucFrmDirectClose = new frmLoansAdvanceStatusChange();
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

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Loans and Advances") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Loans and Advances without Approval") != null)
                        {

                            loansAdvance.PendingForClosing = false;
                            loansAdvance.stage = TransactionStage.Closed.ToString();
                            loansAdvance.statusId = statusChanged.Id;
                            loansAdvance.LastStatusChangeDate = System.DateTime.Now;
                            loansAdvance.ClosingDate = System.DateTime.Now;



                            loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.LoansAdvances, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Loans and Advances having system ref #: " + loansAdvance.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + loansAdvance.LoanAmountOC,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers,
                                    TaggedRecomenndedList = tagUsersRecommendation,
                                    CCRecomenndedList = ccUsersRecommendation
                                };
                                if (loansAdvance != null)
                                {
                                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans and Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans and Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                            }
                            //Load_Receipts();
                        }
                        else
                        {

                            loansAdvance.PendingForClosing = true;
                            loansAdvance.stage = TransactionStage.AwaitingApproval.ToString();
                            loansAdvance.statusId = statusChanged.Id;
                            loansAdvance.LastStatusChangeDate = System.DateTime.Now;
                            loansAdvance.ClosingDate = System.DateTime.Now;



                            loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.LoansAdvances, frmInputBox.comment);

                            if (previous_status != statusChanged.Status)
                            {
                                string oldStat = previous_status;
                                string newStat = statusChanged.Status;
                                CommentLog comment = new CommentLog()
                                {
                                    Comment = "Status of Loans and Advances having system ref #: " + loansAdvance.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat + "\nAmount OC:" + loansAdvance.LoanAmountOC,
                                    Timestamp = DateTime.Now,
                                    Subject = "Status Changed",
                                    TaggedList = tagUsers,
                                    CCUsersList = ccUsers,
                                    TaggedRecomenndedList = tagUsersRecommendation,
                                    CCRecomenndedList = ccUsersRecommendation
                                };
                                if (loansAdvance != null)
                                {
                                    procurementRepo.Add(loansAdvance.Id, TransactionItemType.LoansAdvances, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans and Advances #" + loansAdvance.SystemRef, loansAdvance.Id, TransactionItemType.LoansAdvances, comment.Comment, user.id, "New Comment ", null);
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
                if (grdLoansList.GetFocusedRow() != null)
                {
                    CompanyLoansRepo loansAdvanceRepo = new CompanyLoansRepo();
                    //SalesReceipt receipt = new SalesReceipt();
                    var selectedRow = (LoansAdvance)grdLoansList.GetFocusedRow();
                    var loansAdvance = loansAdvanceRepo.GetLoansAdvance(selectedRow.Id);
                    UsersRepo usersRepo = new UsersRepo();

                    if (loansAdvance != null)
                    {
                        if (loansAdvance.isApproved != true)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Loans and Advances") != null) ? true : false)
                            {

                                loansAdvance.isApproved = true;
                                loansAdvance.stage = TransactionStage.Approved.ToString();

                                //receipt.isApproved = true;

                                //receipt.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, 25, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);

                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Loans and Advance is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Loans and Advance is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Approve Loans and Advance Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans and Advance Directly user id=(" + MainWindow.currentUserid + ")");
                                return;
                            }

                        }
                        else if (loansAdvance.isReApproved == false)
                        {
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Loans and Advances") != null) ? true : false)
                            {
                                loansAdvance.isReApproved = true;
                                loansAdvance.stage = TransactionStage.Approved.ToString();


                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, selectedRow.Id, (int)TransactionItemType.LoansAdvances, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);=

                                loansAdvanceRepo.UpdateLoansAdvance(loansAdvance);
                                //foreach (var _bill in loansAdvance)
                                //{
                                //    loansAdvanceRepo.updateSalesReceipt(_bill);
                                //}

                                MessageBox.Show("Loans and Advance is Approved (" + selectedRow.Id + ")");
                                SystemLog.LogInfo(this.GetType(), "Loans and Advance is Approved (" + selectedRow.Id + ")");
                            }
                            else
                            {
                                MessageBox.Show("You are not Allowed to Re-Approve Loans and Advances Directly");
                                SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Loans and Advances Directly user id=(" + MainWindow.currentUserid + ")");
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

        private void mbtnDriectClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void mbtnApprove_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void mbtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mbtnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
