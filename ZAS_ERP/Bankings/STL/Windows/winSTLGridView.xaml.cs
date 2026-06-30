using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Procurements.InterBankTransfers;
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
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Bankings.STL.Windows
{
    /// <summary>
    /// Interaction logic for winSTLGridView.xaml
    /// </summary>
    public partial class winSTLGridView : DXWindow
    {
        GridReport report = new GridReport();
        string reportTitle;
        ProcurementRepo procurementRepo = new ProcurementRepo();
        public winSTLGridView()
        {
            InitializeComponent();
        }
        public winSTLGridView(GridReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            STLRepo repo = new STLRepo();
            GridReportRepo repoReport = new GridReportRepo();
            var group = repoReport.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here  
                            //mbtnShareReport.IsVisible = false;
                            mbtnExportToStandardReport1.IsVisible = false;
                            grdSTLRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "STL" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "STL" + "/" + group.groupName + "/" + reportTitle;
                            grdSTLRegister.ItemsSource = repo.getSTLRegister(SYSTEM_STATIC.currentUser.id);
                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here   
                            mbtnExportToMemorizedReport1.IsVisible = false;
                            grdSTLRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "STL" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "STL" + "/" + group.groupName + "/" + reportTitle;
                            grdSTLRegister.ItemsSource = repo.getSTLRegister(SYSTEM_STATIC.currentUser.id);
                        }

                        break;
                    }
            }
        }
        private void grdSTLRegister_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                var stl = grdSTLRegister.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Procurements.InterBankTransfers.STL;

                STLRepo repo = new STLRepo();
                stl= repo.Get(stl.Id);
                if (e.Column.FieldName == "InterestAmountIAC" && e.IsGetData)
                {
                    double paymentAmount = 0, interestPerc = 0, utilizeDays = 0, creditTenure = 0;
                    DateTime start = (DateTime)stl.stlPaymentDate;
                    DateTime end = (DateTime)stl.paymentMaturityDate;
                    DateTime calUtilizedDate = DateTime.Now;
                    var stamp = calUtilizedDate - start;
                    utilizeDays = stamp.Days;
                    if (stl.creditTenureNo != 0 && stl.interest != null && stl.stlPaymentDate != null && stl.paymentMaturityDate != null)
                    {
                        creditTenure = Math.Round(stl.creditTenureNo + stl.extendedCreditTenureNo);
                        paymentAmount = Math.Round(stl.stlPaymentAmountOC);
                        interestPerc = Math.Round(Convert.ToDouble(stl.interest.percentage), 2);
                        var value1 = paymentAmount * interestPerc;
                        var value2 = value1 / 100;
                        var value3 = value2 / 365;
                        var value4 = value3 * utilizeDays;
                        e.Value = (Math.Round(value4, 2)).ToString();
                    }
                }
               
                if (e.Column.FieldName == "stlUtilizedDays1" && e.IsGetData)
                {
                    double utilizeDays = 0;

                    DateTime start = (DateTime)stl.stlPaymentDate;
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
            catch (Exception ex)
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
                                        ReportLogic.RenameGridReport(grdSTLRegister, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                        ReportLogic.RenameGridReport(grdSTLRegister, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                    ReportLogic.UpdateGridReport(grdSTLRegister, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdSTLRegister, str, reportType, groupDetails, report.Id, report.settingkey);

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

                                ReportLogic.DeleteReport(grdSTLRegister, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                                ReportLogic.DeleteReport(grdSTLRegister, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                grdSTLRegister.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
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
                                    ReportLogic.SaveGridReport(grdSTLRegister, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.ExportToStandard(grdSTLRegister, reportName, reportType, reportGroup, report.settingkey);
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
            PrintableControlLink link = new PrintableControlLink((TableView)grdSTLRegister.View);
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
                                    ReportLogic.SaveGridReport(grdSTLRegister, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.SaveGridReport(grdSTLRegister, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            STLRepo repo = new STLRepo();
            grdSTLRegister.ItemsSource = repo.getSTLRegister(SYSTEM_STATIC.currentUser.id);
            loadingGif.Visibility = Visibility.Hidden;
        }

    }
}
