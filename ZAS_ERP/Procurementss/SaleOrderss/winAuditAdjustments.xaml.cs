using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements;
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

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for winAuditAdjustments.xaml
    /// </summary>
    public partial class winAuditAdjustments : DXWindow
    {
        List<AuditYearAdjustment> auditYearAdjustments = new List<AuditYearAdjustment>();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        public winAuditAdjustments()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grdAuditAdjustmentgrid.ItemsSource = saleOrderRepo.GetAllAuditYearAdjustments(SYSTEM_STATIC.currentUser.id);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdAuditAdjustmentgrid);

        }
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            grdAuditAdjustmentgrid.ItemsSource = saleOrderRepo.GetAllAuditYearAdjustments(SYSTEM_STATIC.currentUser.id);

        }

        private void btnExporttoReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grdAuditAdjustmentgrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = grdAuditAdjustmentgrid.SelectedItem as SaleOrder;
            if (selectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, selectedItem.Id);
                procurmentPanel.Show();
            }
        }

        private void btnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdAuditAdjustmentgrid);

        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdAuditAdjustmentgrid.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }
    }
}
