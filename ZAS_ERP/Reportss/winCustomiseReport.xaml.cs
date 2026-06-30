using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Reports;
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
using System.Windows.Shapes;

namespace ZAS_ERP.Reportss
{
    /// <summary>
    /// Interaction logic for winCustomiseReport.xaml
    /// </summary>
    public partial class winCustomiseReport : DXWindow
    {

        List<GridReport> gridReports = new List<GridReport>();
        GridReportRepo repo = new GridReportRepo();

        public winCustomiseReport()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            LoadReports();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdReportListing);
        }
        public void LoadReports()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Procurment Panel") != null)
            {
                try
                {
                    var groups = repo.GetALLReportGroups();
                    foreach (var group in groups)
                    {
                        switch (group.gridReportType)
                        {
                            case GridReportType.StandardReport:
                                {
                                    if (group.parentId == null)
                                        continue;

                                    if (group.parent.groupName == "Inquiries" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else    
                                    if (group.parent.groupName == "Offers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else    
                                    if (group.parent.groupName == "Sale Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else    
                                    if (group.parent.groupName == "Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else    
                                    if (group.parent.groupName == "Purchase Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else    
                                    if (group.parent.groupName == "Sale Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else
                                    if (group.parent.groupName == "Sale Receipts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else
                                    if (group.parent.groupName == "Inter-Bank Transfers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else
                                    if (group.parent.groupName == "Admin Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else
                                    if (group.parent.groupName == "Purchase Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else
                                    if (group.parent.groupName == "Payments" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else
                                    if (group.parent.groupName == "Chart of Accounts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                    }
                                    else
                                    if (group.parent.groupName == "Todo Tasks" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "To Do Tasks") != null)
                                    {
                                        var reports = repo.GetALLReportsbyGroupId(group.Id);
                                        gridReports.AddRange(reports);
                                        break;
                                    }
                                    break;
                                }
                            case GridReportType.MemorizedReport:
                                {
                                    if (group.parentId == null)
                                        continue;
                                    if (group.userId == SystemLog.CurrentUserId)
                                    {

                                        if (group.parent.groupName == "Inquiries" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);


                                        }
                                        else
                                            if (group.parent.groupName == "Offers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);


                                        }
                                        else
                                            if (group.parent.groupName == "Sale Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
                                        {

                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);


                                        }
                                        else
                                            if (group.parent.groupName == "Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);


                                        }
                                        else
                                            if (group.parent.groupName == "Purchase Orders" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
                                        {

                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);


                                        }
                                        else
                                            if (group.parent.groupName == "Sale Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
                                        {

                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);


                                        }
                                        else
                                            if (group.parent.groupName == "Sale Receipts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);


                                        }
                                        else
                                            if (group.parent.groupName == "Inter-Bank Transfers" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);
                                        }
                                        else
                                            if (group.parent.groupName == "Admin Bills" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);
                                        }
                                        else
                                            if (group.parent.groupName == "Purchase Invoices" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);
                                        }
                                        else
                                            if (group.parent.groupName == "Payments" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);
                                        }
                                        else
                                    if (group.parent.groupName == "Todo Tasks" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "To Do Tasks") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);
                                        }
                                        else
                                        if (group.parent.groupName == "Chart of Accounts" && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
                                        {
                                            var reports = repo.GetALLReportsbyGroupId(group.Id);
                                            gridReports.AddRange(reports);
                                        }
                                        break;

                                    }
                                    break;
                                }
                        }
                    }
                    grdReportListing.ItemsSource = gridReports;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
            }
        }

        private void mbtnSaveLayout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdReportListing);

        }

        private void btnDelete_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                var report = grdReportListing.SelectedItem as GridReport;
                if (report.gridReportType == GridReportType.StandardReport)
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Standard Report") != null)
                    {
                        repo.DeleteReport(report);
                        DXMessageBox.Show("Report has been deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.OK);
                    }
                    else
                    {
                        DXMessageBox.Show("You don't have permission to Delete Standard report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Access to Delete Memorized Report") != null)
                    {
                        repo.DeleteReport(report);
                        DXMessageBox.Show("Report has been deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.OK);
                    }
                    else
                    {
                        DXMessageBox.Show("You don't have permission to Delete Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                LoadReports();
                grdReportListing.RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
