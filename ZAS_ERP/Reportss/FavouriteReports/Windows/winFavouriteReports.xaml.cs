using DevExpress.Xpf.Core;
using DevExpress.Xpf.Ribbon;
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
using ZAS_ERP.Procurementss.Inquiriess.UserControls;
using ZAS_ERP.Procurementss.Offerss.UserControls;
using ZAS_ERP.Procurementss.PurchaseOrderss.UserControls;
using ZAS_ERP.Procurementss.SaleInvoicess.UserControls;
using ZAS_ERP.Procurementss.SaleOrderss.UserControls;
using ZAS_ERP.ToDoTasks.UserControls;
using ZAS_ERP.ToDoTasks.Windows;

namespace ZAS_ERP.Reportss.FavouriteReports.Windows
{
    /// <summary>
    /// Interaction logic for winFavouriteReports.xaml
    /// </summary>
    public partial class winFavouriteReports : DXWindow
    {
        public winFavouriteReports()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFavouriteReports();
        }
        public void LoadFavouriteReports()
        {
            GridReportRepo reportRepo = new GridReportRepo();
            var reports = reportRepo.GetAllFavouriteReports(SYSTEM_STATIC.currentUser.id);
            backstageViewControl.Items.Clear();

            foreach (var report in reports)
            {
                var imgUriReport = "/ZAS_ERP;component/images/Report_16x16.png";
                BitmapImage bitmapReport = new BitmapImage(new Uri(imgUriReport, UriKind.Relative));


                BackstageTabItem backstageTabItem = new BackstageTabItem()
                {
                    Content = report.reportName,
                    Glyph = bitmapReport
                    // Set other properties as needed
                };

                // Create the content for the BackstageTabItem
                // For example, you can add controls like TextBlock, Grid, etc., as content
                // Replace this with your specific content creation logic
                TextBlock contentTextBlock = new TextBlock()
                {
                    Text = $"{report.reportName}",
                    VerticalAlignment = VerticalAlignment.Center
                };

                backstageTabItem.Content = contentTextBlock;

                // Subscribe to the MouseDoubleClick event for each BackstageTabItem
                //backstageTabItem.MouseDown += (sender, e) =>
                //{
                //    // Handle double-click event here
                //    // For example:
                //    MessageBox.Show($"You double-clicked on {report.reportName}");
                //};

                // Add the BackstageTabItem to your BackstageView control
                if (report != null)
                    switch (report.gridReportGroup.transactionType)
                    {
                        case ReportTransactionType.Inquiry:
                            if (report != null)
                            {
                                ucInquiryViewFav reportView = new ucInquiryViewFav(report, report.reportName);
                                Grid grid = new Grid();
                                grid.Children.Add(reportView);
                                backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Offer:
                            if (report != null)
                            {
                                ucOffersReportViewFav reportView = new ucOffersReportViewFav(report, report.reportName);
                                Grid grid = new Grid();
                                grid.Children.Add(reportView);
                                backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Sale_Order:
                            if (report != null)
                            {
                                ucSaleOrderReportViewFav reportView = new ucSaleOrderReportViewFav(report, report.reportName);
                                Grid grid = new Grid();
                                grid.Children.Add(reportView);
                                backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Sale_Invoice:
                            {
                                if (report != null)
                                {
                                    ucSaleInvoiceViewFav reportView = new ucSaleInvoiceViewFav(report, report.reportName);
                                    Grid grid = new Grid();
                                    grid.Children.Add(reportView);
                                    backstageTabItem.ControlPane = grid;
                                }
                            }
                            break;
                        case ReportTransactionType.Purchase_Order:
                            if (report != null)
                            {
                                ucPurchaseOrderViewFav reportView = new ucPurchaseOrderViewFav(report, report.reportName);
                                Grid grid = new Grid();
                                grid.Children.Add(reportView);
                                backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Purchase_Invoice:
                            if (report != null)
                            {
                                //ucPIView reportView = new ucPIView(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Bill:
                            if (report != null)
                            {
                                //ucBillsView reportView = new ucBillsView(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Sale_Receipt:
                            if (report != null)
                            {
                                //    ucSaleReceiptView reportView = new ucSaleReceiptView(report, report.reportName);
                                //    Grid grid = new Grid();
                                //    grid.Children.Add(reportView);
                                //    backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.InterBank_Transfer:
                            if (report != null)
                            {
                                //ucInterBankTransferView reportView = new ucInterBankTransferView(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Admin_Bill:
                            if (report != null)
                            {
                                //ucAdminBillsView reportView = new ucAdminBillsView(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.InterCompanyBank_Transfer:
                            break;
                        case ReportTransactionType.Payments:
                            if (report != null)
                            {
                                //ucPaymentView reportView = new ucPaymentView(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Targets:
                            if (report != null)
                            {
                                ucTodoTaskFavReport reportView = new ucTodoTaskFavReport(report, report.reportName);
                                Grid grid = new Grid();
                                grid.Children.Add(reportView);
                                backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Trial_Balance:
                            if (report != null)
                            {
                                //winTrialBalanceReport reportView = new winTrialBalanceReport(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.TargetReward:
                            if (report != null)
                            {
                                //winTargetRegisterReport reportView = new winTargetRegisterReport(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Chart_of_Account:
                            if (report != null)
                            {
                                //ucChartofAccountsView reportView = new ucChartofAccountsView(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.STL:
                            if (report != null)
                            {
                                //winSTLGridView reportView = new winSTLGridView(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                        case ReportTransactionType.Tasks:
                            if (report != null)
                            {
                                //ucTaskGridView reportView = new ucTaskGridView(report, report.reportName);
                                //Grid grid = new Grid();
                                //grid.Children.Add(reportView);
                                //backstageTabItem.ControlPane = grid;
                            }
                            break;
                    }

                backstageViewControl.Items.Add(backstageTabItem);
            }

        }
    }
}
