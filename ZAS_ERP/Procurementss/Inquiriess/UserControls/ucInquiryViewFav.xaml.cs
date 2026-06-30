using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.Themes;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TableDependency.SqlClient;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.Inquiriess.UserControls
{
    /// <summary>
    /// Interaction logic for ucInquiryViewFav.xaml
    /// </summary>
    public partial class ucInquiryViewFav : UserControl
    {

        Inquiry inquiry = new Inquiry();
        GridReport report = new GridReport();

        public MainWindow myParent = null;
        string reportTitle;
        public ucInquiryViewFav()
        {
            InitializeComponent();

        }
        public ucInquiryViewFav(GridReport reportToEdit, string title)
        {
            InitializeComponent();

            report = reportToEdit;
            reportTitle = title;


        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loader.DeferedVisibility = true;
            InquiryRepo inquiryRepo = new InquiryRepo();
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
                            grdinquiryReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            lblHeading.Caption = "Inquiries" + "/" + group.groupName + "/" + reportTitle;
                            grdinquiryReport.ItemsSource = inquiryRepo.getAll(SYSTEM_STATIC.currentUser.id);
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

                            grdinquiryReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            lblHeading.Caption = "Inquiries" + "/" + group.groupName + "/" + reportTitle;
                            grdinquiryReport.ItemsSource = inquiryRepo.getAll(SYSTEM_STATIC.currentUser.id);
                        }
                        break;
                    }
            }
            Loader.DeferedVisibility = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void Grdinquiry_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditInquiry();
            inquiry = ((Inquiry)grdinquiryReport.GetFocusedRow());
        }

        private void EditInquiry()
        {


            if (grdinquiryReport.SelectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Inquiry, (int)(grdinquiryReport.SelectedItem as Inquiry).Id);
                procurmentPanel.inq.Add(inquiry);
                procurmentPanel.Show();
                inquiry = null;
            }
        }

        //private void MbtnDeleteReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var inputfromUser = DXMessageBox.Show("Do you want to delete " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        //    switch (report.gridReportType)
        //    {
        //        case GridReportType.StandardReport:
        //            {
        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Delete Standard Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        /*GridReport report = repo.GetReportByName(this.Title)*/
        //                        ;
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

        //                        ReportLogic.DeleteReport(grdinquiry, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
        //                        DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        this.Close();
        //                    }
        //                    else
        //                        return;

        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Delete Standard report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }

        //                break;
        //            }

        //        case GridReportType.MemorizedReport:
        //            {

        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Delete Memorized Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        /*GridReport report = repo.GetReportByName(this.Title)*/
        //                        ;
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

        //                        ReportLogic.DeleteReport(grdinquiry, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
        //                        DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        this.Close();
        //                    }
        //                    else
        //                        return;

        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Delete Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }

        //                break;
        //            }
        //    }
        //}

        //private void MbtnRefreshReport_Click(object sender, RoutedEventArgs e)
        //{
        //    loadingGif.Visibility = Visibility.Visible;
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.DoWork += OnDoWork;
        //    worker.RunWorkerCompleted += OnRunWorkerCompleted;
        //    worker.RunWorkerAsync();
        //}
        //private void MbtnExportToMemorizedReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Memorized Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        //    switch (report.gridReportType)
        //    {
        //        case GridReportType.StandardReport:
        //            {
        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Export to Memorized Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        //var report = repo.GetReportByName(this.Title);
        //                        var type = report.gridReportType;
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

        //                        Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //                        setReportName.ShowDialog();
        //                        var exportToMemorizedReport = setReportName.report;
        //                        if (exportToMemorizedReport != null && exportToMemorizedReport.gridReportGroup != null && exportToMemorizedReport.gridReportType != null && exportToMemorizedReport.reportName != null && report.userId != null)
        //                        {
        //                            var reportGroup = exportToMemorizedReport.gridReportGroup;
        //                            var reportType = exportToMemorizedReport.gridReportType;
        //                            var reportName = exportToMemorizedReport.reportName;
        //                            ReportLogic.SaveGridReport(grdinquiry, reportName, reportType, reportGroup, report.settingkey);
        //                            DXMessageBox.Show("( " + reportName + " ) is exported to Memorized reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        }

        //                    }
        //                    else
        //                        return;

        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //                break;
        //            }

        //    }
        //}

        //private void MbtnExportToStandardReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Standard Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


        //    switch (report.gridReportType)
        //    {
        //        case GridReportType.MemorizedReport:
        //            {
        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Export to Standard Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        //var report = repo.GetReportByName(this.Title);
        //                        var type = report.gridReportType;
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

        //                        Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //                        setReportName.ShowDialog();
        //                        var exportToStandardReport = setReportName.report;
        //                        if (exportToStandardReport != null && exportToStandardReport.gridReportGroup != null && exportToStandardReport.gridReportType != null && exportToStandardReport.reportName != null && report.userId != null)
        //                        {
        //                            var reportGroup = exportToStandardReport.gridReportGroup;
        //                            var reportType = exportToStandardReport.gridReportType;
        //                            var reportName = exportToStandardReport.reportName;
        //                            ReportLogic.ExportToStandard(grdinquiry, reportName, reportType, reportGroup, report.settingkey);
        //                            DXMessageBox.Show("( " + reportName + " ) is exported to Standard reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        }

        //                    }
        //                    else
        //                        return;

        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //                break;
        //            }

        //    }



        //    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Export to Standard Report") != null)
        //    //{
        //    //    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //    //    {
        //    //        var repo = new GridReportRepo();
        //    //        //var report = repo.GetReportByName(this.Title);
        //    //        var type = report.gridReportType;
        //    //        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
        //    //        if (report.gridReportType == GridReportType.MemorizedReport)
        //    //        {
        //    //            Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //    //            setReportName.ShowDialog();
        //    //            var exportMemorizedReport = setReportName.report;
        //    //            if (exportMemorizedReport != null && exportMemorizedReport.gridReportGroup != null && exportMemorizedReport.gridReportType != null && exportMemorizedReport.reportName != null && report.userId != null)
        //    //            {
        //    //                var reportGroup = exportMemorizedReport.gridReportGroup;
        //    //                var reportType = exportMemorizedReport.gridReportType;
        //    //                var reportName = exportMemorizedReport.reportName;
        //    //                ReportLogic.ExportToStandard(grdinquiry, reportName, reportType, reportGroup, report.settingkey);
        //    //                DXMessageBox.Show("( " + reportName + " ) is exported to Standard reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            DXMessageBox.Show("Select Report type "+" Standard"+"!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //            return;
        //    //        }
        //    //        return;
        //    //    }
        //    //    else
        //    //        return;
        //    //}
        //    //else
        //    //{
        //    //    DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //    return;
        //    //}
        //}

        //private void MbtnExportToReportList_Click(object sender, RoutedEventArgs e)
        //{
        //    var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + "(" + this.Title + " )" + " to Report Lists ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        //    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //    {
        //        var repo = new GridReportRepo();
        //        var report = repo.GetReportByName(this.Title);
        //        var type = report.gridReportType;
        //        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
        //        MenuItem mbtnReport = new MenuItem();
        //        mbtnReport.Header = report.reportName;
        //        Image reportImage = new Image();
        //        reportImage.Source = new BitmapImage(new Uri("/ZAS_ERP;component/images/Report_16x16.png", UriKind.RelativeOrAbsolute));
        //        mbtnReport.Icon = reportImage;
        //        ContextMenu menu = new ContextMenu();
        //        MenuItem item = new MenuItem();
        //        item.Header = "Remove";
        //        item.Click += new RoutedEventHandler(Delete_Report);
        //        menu.Items.Add(item);
        //        mbtnReport.ContextMenu = menu;
        //        mbtnReport.Click += new RoutedEventHandler(InquiryReportsClick);
        //        myParent.lstReports.Items.Add(mbtnReport);
        //        DXMessageBox.Show("Report Exported to Report Lists Successfully", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Information);




        //    }
        //}

        //private void Delete_Report(object sender, RoutedEventArgs e)
        //{
        //    myParent.lstReports.Items.Remove(this);
        //}

        private void InquiryReportsClick(object sender, RoutedEventArgs e)
        {

            //GridReport report = new GridReport();
            //GridReportRepo repo = new GridReportRepo();
            //InquiryRepo inquiryRepo = new InquiryRepo();
            //report = repo.GetReportByName(this.Title);
            //ucInquiryView reportView = new ucInquiryView(report);
            //reportView.mbtnExportToStandardReport.Visibility = Visibility.Collapsed;
            //reportView.grdinquiry.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
            //reportView.Title = (sender as MenuItem).Header.ToString();
            //reportView.lblHeading.Text = (sender as MenuItem).Header.ToString();
            //reportView.grdinquiry.ItemsSource = inquiryRepo.getAll(SystemLogic.currentUser.id);

            //reportView.Show();

        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            InquiryRepo repo = new InquiryRepo();
            grdinquiryReport.ItemsSource = repo.getAll(SYSTEM_STATIC.currentUser.id);

            loadingGif.Visibility = Visibility.Hidden;

        }

     


        private void MbtnSaveAsNew1_Click(object sender, EventArgs e)
        {
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.lblHeading.Caption + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                        ReportLogic.SaveGridReport(grdinquiryReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.lblHeading.Caption + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                    ReportLogic.SaveGridReport(grdinquiryReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
            var inputfromUser = DXMessageBox.Show("Do you want to rename " + this.lblHeading.Caption + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                        ReportLogic.RenameGridReport(grdinquiryReport, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
                                        ReportLogic.RenameGridReport(grdinquiryReport, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.lblHeading.Caption + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                    ReportLogic.UpdateGridReport(grdinquiryReport, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdinquiryReport, str, reportType, groupDetails, report.Id, report.settingkey);

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
            var inputfromUser = DXMessageBox.Show("Do you want to delete " + this.lblHeading.Caption + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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

                                ReportLogic.DeleteReport(grdinquiryReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
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

                                ReportLogic.DeleteReport(grdinquiryReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
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
            var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.lblHeading.Caption + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetReportById(report.Id);
                grdinquiryReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.lblHeading.Caption + " to Memorized Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
                                    ReportLogic.SaveGridReport(grdinquiryReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.lblHeading.Caption + " to Standard Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


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
                                    ReportLogic.ExportToStandard(grdinquiryReport, reportName, reportType, reportGroup, report.settingkey);
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
        private void grdinquiry_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {

            Inquiry inq = grdinquiryReport.CurrentItem as Inquiry;
            if (e.Column.FieldName == "inquiryStatus.Status" && inq != null)
            {
                string mycolor = inq.inquiryStatus.backcolor;
                System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(mycolor.ToString());
                System.Windows.Media.Color newColor = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
                Style st = new Style(typeof(LightweightCellEditor));
                object key = new DevExpress.Xpf.Grid.Themes.GridRowThemeKeyExtension() { ResourceKey = DevExpress.Xpf.Grid.Themes.GridRowThemeKeys.CellStyle };
                SolidColorBrush s = new SolidColorBrush(newColor);
                Binding myBinding = new Binding();
                myBinding.ElementName = "grdinquiry";
                myBinding.Path = new PropertyPath("CurrentItem.inquiryStatus.backcolor");
                myBinding.Mode = BindingMode.TwoWay;
                //st.BasedOn = (Style)this.FindResource(key);
                //Style baseStyle = FindResource(new GridRowThemeKeyExtension() { ResourceKey = GridRowThemeKeys.LightweightCellStyle, ThemeName = ThemeManager.ActualApplicationThemeName }) as Style;
                //st.BasedOn = baseStyle;
                Style myStyle = new Style(typeof(DataGridCell));
                myStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, new Binding("SelectedColour[0]")));
                st.Setters.Add(new Setter(LightweightCellEditor.BackgroundProperty, newColor));
                //new Binding("RowData.Row.inquiryStatus.backcolor")
                //st.Setters.Add(new Setter(LightweightCellEditor.BackgroundProperty, new Binding(new SolidColorBrush(newColor))));
                // GridCell gridCell = grdinquiry.GetCellValue((int)grdinquiry.CurrentItem, grdinquiry.Columns.GetColumnByFieldName("inquiryStatus.Status")) as GridCell;

                //next statment sets the cell style to grds desired column 
                // grdinquiry.Columns.GetColumnByFieldName("inquiryStatus.Status").CellStyle = st;

            }
        }

        private void grdinquiry_AutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {
            if (e.Column.FieldName == "inquiryStatus.Status")
            {
                Inquiry inq = grdinquiryReport.CurrentItem as Inquiry;
                var cb = new GridColumn();
                cb.Header = "Status";
            }
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdinquiryReport.View);
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

        private void Grdinquiry_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (tableView.CompactPanelShowMode == CompactPanelShowMode.CompactMode)
            //{
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.CompactMode;
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.Always;
            //}
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Inquiry Report") != null)
            {
                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to share " + this.lblHeading.Caption + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
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
                DXMessageBox.Show("You don't have permission Share Inquiry Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }

        }

        private void grdinquiry_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Stage" && e.IsGetData)
            {
                var inquiry = grdinquiryReport.GetRowByListIndex(e.ListSourceRowIndex) as Inquiry;

                if (inquiry.isVoid == true)
                {
                    e.Value = "Void";
                }
                //else if (inquiry.isReApproved == false)
                //{
                //    e.Value = "Under Approval";
                //}
                else if (inquiry.isApproved == true && inquiry.stage == "Closed")
                {
                    e.Value = "Closed";
                }
                else if (inquiry.isApproved == true && inquiry.inquiryStatus.isActive == false && inquiry.PendingForClosing != true)
                {
                    e.Value = "Closed";
                }
                else if (inquiry.isApproved == true && inquiry.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
                else if (inquiry.isApproved == true)
                {
                    e.Value = "Approved";
                }
                else if (inquiry.isApproved == false)
                {
                    e.Value = "Under Approval";
                }
                else if (inquiry.PendingForClosing == true)
                {
                    e.Value = "Under Closing";
                }
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

