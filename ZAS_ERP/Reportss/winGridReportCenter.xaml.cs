using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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
using ZAS_ERP.AssetRentalss.Windows;
using ZAS_ERP.Bankings.STL.Windows;
using ZAS_ERP.Bankings.Windows;
using ZAS_ERP.CashFlow.Windows;
using ZAS_ERP.ChartofAccounts.UserControls;
using ZAS_ERP.ChartofAccounts.Windows;
using ZAS_ERP.Payments.UserControls;
using ZAS_ERP.Procurementss.AdminBillss.Windows;
using ZAS_ERP.Procurementss.Billss.UserControls;
using ZAS_ERP.Procurementss.Inquiriess.UserControls;
using ZAS_ERP.Procurementss.LoanAdvance.Windows;
using ZAS_ERP.Procurementss.Offerss.UserControls;
using ZAS_ERP.Procurementss.PurchaseInvoice.UserControls;
using ZAS_ERP.Procurementss.PurchaseOrderss.UserControls;
using ZAS_ERP.Procurementss.SaleInvoicess.UserControls;
using ZAS_ERP.Procurementss.SaleOrderss.UserControls;
using ZAS_ERP.SaleOrderFolder.Windows;
using ZAS_ERP.ToDoTasks.Taskks.UserControls;
using ZAS_ERP.ToDoTasks.Windows;

namespace ZAS_ERP.Reportss
{
    /// <summary>
    /// Interaction logic for winGridReportCenter.xaml
    /// </summary>
    public partial class winGridReportCenter : DXWindow
    {
        public winGridReportCenter()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadGroups();
            //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdStandardReportGroups);

            //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdMemorizedReportGroups);

        }
        public void loadGroups()
        {
            loadMemorizedGroups();
            loadStandardGroups();
        }
        private void loadMemorizedGroups()
        {
            List<GridReportGroup> memorizedGroups = new List<GridReportGroup>();
            GridReportRepo gridReportRepo = new GridReportRepo();
            var groupType = GridReportType.MemorizedReport;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Inquiry, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Order, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Offer, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Bill, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Order, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Invoice, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Receipt, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterBank_Transfer, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Admin_Bill, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterCompanyBank_Transfer, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Payments, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Invoice, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Chart_of_Account, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Targets, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.TargetReward, groupType, SYSTEM_STATIC.currentUser.id));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Trial_Balance, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.STL, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Tasks, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Advances, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Company Loans") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Loans, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Assets, groupType, SYSTEM_STATIC.currentUser.id));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null)
            {
                memorizedGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.CashFlow, groupType, SYSTEM_STATIC.currentUser.id));
            }

            this.grdMemorizedReportGroups.ItemsSource = memorizedGroups;
       
        }
        private void loadStandardGroups()
        {
            GridReportRepo gridReportRepo = new GridReportRepo();
            List<GridReportGroup> standardGroups = new List<GridReportGroup>();
            var groupType = GridReportType.StandardReport;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inquiries") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Inquiry, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Order, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Offers") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Offer, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Bill, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Orders") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Order, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Invoice, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Receipts") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Sale_Receipt, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterBank_Transfer, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Admin Bills") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Admin_Bill, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Inter-Bank Transfer") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.InterCompanyBank_Transfer, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Payments") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Payments, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Purchase Invoices") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Purchase_Invoice, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Accountant Center") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Chart_of_Account, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Targets, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Target Rewards") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.TargetReward, groupType));
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Trial Balance") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Trial_Balance, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "STL") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.STL, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Tasks") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Tasks, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Advances") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Advances, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Company Loans") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Loans, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Assets") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.Assets, groupType));
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "CashFlow") != null)
            {
                standardGroups.AddRange(gridReportRepo.GetGroupsByTransactionType(ReportTransactionType.CashFlow, groupType));
            }
            this.grdStandardReportGroups.ItemsSource = standardGroups;
           
        }

        private void DXWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdMemorizedReportGroups);
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdStandardReportGroups);

        }

        private void grdReportsListing_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var report=grdReportsListing.SelectedItem as GridReport;
            if(report!=null)
                switch (report.gridReportGroup.transactionType)
                {
                    case ReportTransactionType.Inquiry:
                        if (report != null)
                        {
                            ucInquiryView reportView = new ucInquiryView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Offer:
                        if (report != null)
                        {
                            ucOffersReportView reportView = new ucOffersReportView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Sale_Order:
                        if (report != null)
                        {
                            if (report.settingkey == "PRIT Register")
                            {
                                ucPRITregisterView rep = new ucPRITregisterView(report, report.reportName);
                                rep.Show();
                            }
                            else
                            {
                                ucSaleOrderReportView reportView = new ucSaleOrderReportView(report, report.reportName);
                                reportView.Show();
                            }
                        }
                        break;
                    case ReportTransactionType.Sale_Invoice:
                        {
                            if (report != null)
                            {
                                ucSaleInvoiceView reportView = new ucSaleInvoiceView(report, report.reportName);
                                reportView.Show();
                            }
                        }
                        break;
                    case ReportTransactionType.Purchase_Order:
                        if (report != null)
                        {
                            ucPurchaseOrderView reportView = new ucPurchaseOrderView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Purchase_Invoice:
                        if (report != null)
                        {
                            ucPIView reportView = new ucPIView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Bill:
                        if (report != null)
                        {
                            ucBillsView reportView = new ucBillsView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Sale_Receipt:
                        if (report != null)
                        {
                            ucSaleReceiptView reportView = new ucSaleReceiptView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.InterBank_Transfer:
                        if (report != null)
                        {
                            ucInterBankTransferView reportView = new ucInterBankTransferView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Admin_Bill:
                        if (report != null)
                        {
                            ucAdminBillsView reportView = new ucAdminBillsView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.InterCompanyBank_Transfer:
                        break;
                    case ReportTransactionType.Payments:
                        if (report != null)
                        {
                            ucPaymentView reportView = new ucPaymentView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Targets:
                        if (report != null)
                        {
                            ucTodoTaskReport reportView = new ucTodoTaskReport(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Trial_Balance:
                        if (report != null)
                        {
                            winTrialBalanceReport reportView = new winTrialBalanceReport(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.TargetReward:
                        if (report != null)
                        {
                            winTargetRegisterReport reportView = new winTargetRegisterReport(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Chart_of_Account:
                        if (report != null)
                        {
                            ucChartofAccountsView reportView = new ucChartofAccountsView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.STL:
                        if (report != null)
                        {
                            winSTLGridView reportView = new winSTLGridView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Tasks:
                        if (report != null)
                        {
                            ucTaskGridView reportView = new ucTaskGridView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Advances:
                        if (report != null)
                        {
                            ucLoansAdvancesView reportView = new ucLoansAdvancesView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Loans:
                        if (report != null)
                        {
                            ucLoansView reportView = new ucLoansView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                    case ReportTransactionType.Assets:
                        if (report != null)
                        {
                            winAssetRentalsView reportView = new winAssetRentalsView(report, report.reportName);
                            reportView.Show();
                        }
                        break;
                }
        }
        private void grdStandardReportGroups_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            var group= grdStandardReportGroups.SelectedItem as GridReportGroup;
            grdReportsListing.ItemsSource = group.reports;
        }

        private void grdMemorizedReportGroups_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            var group = grdMemorizedReportGroups.SelectedItem as GridReportGroup;
            grdReportsListing.ItemsSource = group.reports;
        }

    
        private void tblStandardGroupListView_FocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            var group = grdStandardReportGroups.GetFocusedRow() as GridReportGroup;
            if (group != null)
            {
                if (group.reports.Count > 0)
                {
                    grdReportsListing.ItemsSource = group.reports;
                    grdCashFlowListing.Visibility = Visibility.Collapsed;
                    grdReportsListing.Visibility = Visibility.Visible;
                }
                else
                {
                    if (group.cashFlowReports.Count > 0)
                    {
                        grdCashFlowListing.ItemsSource = group.cashFlowReports;
                        grdCashFlowListing.Visibility = Visibility.Visible;
                        grdReportsListing.Visibility = Visibility.Collapsed;
                    }

                }
                txtGridGroupName.Text = group.groupName;
            }
        }

        private void tblMemorizedGroupListView_FocusedRowChanged(object sender, DevExpress.Xpf.Grid.FocusedRowChangedEventArgs e)
        {
            var group = grdMemorizedReportGroups.GetFocusedRow() as GridReportGroup;
            if (group != null)
            {
                if (group.reports.Count > 0)
                {
                    grdReportsListing.ItemsSource = group.reports;
                    grdCashFlowListing.Visibility = Visibility.Collapsed;
                    grdReportsListing.Visibility = Visibility.Visible;
                }
                else
                {
                    if (group.cashFlowReports.Count > 0)
                    {
                        grdCashFlowListing.ItemsSource = group.cashFlowReports;
                        grdCashFlowListing.Visibility = Visibility.Visible;
                        grdReportsListing.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
        
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loadMemorizedGroups();
            loadStandardGroups();
        }

       

        private void tblStandardGroupListView_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                var group = e.Node.Content as GridReportGroup;
                if (e.Column.FieldName == "totalMReports")
                {
                    if (group.reports != null && group.reports.Count != 0)
                    {
                        e.Value = group.reports.Count;
                    }
                }
                if (e.Column.FieldName == "totalSReports")
                {
                    if (group.reports != null && group.reports.Count != 0)
                    {
                        e.Value = group.reports.Count;
                    }
                }

            }
        }

        private void tblMemorizedGroupListView_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                var group = e.Node.Content as GridReportGroup;
                if (e.Column.FieldName == "totalMReports")
                {
                    if (group.reports != null && group.reports.Count != 0)
                    {
                        e.Value = group.reports.Count;
                    }
                }
                if (e.Column.FieldName == "totalSReports")
                {
                    if (group.reports != null && group.reports.Count != 0)
                    {
                        e.Value = group.reports.Count;
                    }
                }

            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridReportRepo repo = new GridReportRepo();

                if(grdReportsListing.SelectedItem!=null)
                {
                    var report = grdReportsListing.SelectedItem as GridReport;

                    if (report != null)
                    {
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
                        grdReportsListing.RefreshData();
                    }
                }
                else
                {
                    DXMessageBox.Show("Please select report to delete", "Information", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void grdCashFlowListing_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var report = grdCashFlowListing.SelectedItem as ERP_BL.CashFlow.CashFlow;
            if (report != null)
                switch (report.gridReportGroup.transactionType)
                {
                    case ReportTransactionType.CashFlow:
                        if (report != null)
                        {
                            winCashFlowCenter reportView = new winCashFlowCenter(report);
                            reportView.Show();
                        }
                        break;
                }
        }
    }
}
