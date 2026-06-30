using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.ExchangeRates;
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

namespace ZAS_ERP.Procurementss.Offerss
{
    /// <summary>
    /// Interaction logic for winComparativeStatements.xaml
    /// </summary>
    public partial class winComparativeStatements : DXWindow
    {

        public static List<ComparativeStatement> comparativeStatements = new List<ComparativeStatement>();
        OfferRepo offerRepo = new OfferRepo();
        Offer offer = new Offer();
        VendorRepo vendorRepo = new VendorRepo();
        ProcurementRepo procurementRepo = new ProcurementRepo();
        IncotermRepo incotermRepo = new IncotermRepo();
        public static int offerId;
        CurrencyRepo currencyRepo = new CurrencyRepo();
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        static List<ExchangeRateGroup> exchangeRateGroupsMER = new List<ExchangeRateGroup>();
        ExchangeRate exchangeRateMER = null;
        public winComparativeStatements()
        {
            InitializeComponent();
        }
        public winComparativeStatements(int _offerId)
        {
            InitializeComponent();
            offerId = _offerId;
        }
        private void grdComparativeStatement_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var statement= grdComparativeStatement.SelectedItem as ComparativeStatement;
            if (statement != null)
            {
                frmComparativeStatement frmComparativeStatement = new frmComparativeStatement();
                frmComparativeStatement.compId = statement.Id;
                frmComparativeStatement.offerId = (int)statement.offerId;
                frmComparativeStatement.Show();
            }
        }
      

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (offerId!=0)
            {
                offer = offerRepo.GetForComparativeStatement(offerId);
                grdPivotComparativeStatement.DataSource = offer.comparativeStatements;
                grdComparativeStatement.ItemsSource = offer.comparativeStatements;
                grdPivotComparativeStatement.BestFitArea = DevExpress.Xpf.PivotGrid.FieldBestFitArea.FieldHeader;
                grdPivotComparativeStatement.BestFit();
            }
        }

        private void grdComparativeStatement_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "totalAmountOC")
            {

                var comparativeStatement = grdComparativeStatement.GetRowByListIndex(e.ListSourceRowIndex) as ComparativeStatement;
                if (comparativeStatement != null)
                {
                    if (comparativeStatement.comparativeStatementItems != null && comparativeStatement.comparativeStatementItems.Count != 0)
                    {
                        var value = comparativeStatement.comparativeStatementItems.Sum(x => x.amountOC);
                        e.Value = value;
                    }
                }
            }
            if (e.Column.FieldName == "totalAmountMER")
            {

                var comparativeStatement = grdComparativeStatement.GetRowByListIndex(e.ListSourceRowIndex) as ComparativeStatement;
                if (comparativeStatement != null)
                {
                    if (comparativeStatement.comparativeStatementItems != null && comparativeStatement.comparativeStatementItems.Count != 0)
                    {
                        var value = comparativeStatement.comparativeStatementItems.Sum(x => x.amountMER);
                        e.Value = value;
                    }
                }
            }
        }

        private void CreateSO_Click(object sender, RoutedEventArgs e)
        {

        }
        private void grdPivotComparativeStatement_CustomUnboundFieldData(object sender, DevExpress.Xpf.PivotGrid.PivotCustomFieldDataEventArgs e)
        {
            if (e.Field.FieldName == "totalAmountOC")
            {
                var Id = Convert.ToInt32(e.GetListSourceColumnValue("Id"));
                var statement=offer.comparativeStatements.FirstOrDefault(x => x.Id == Id);
                var totalValue = Convert.ToDouble(statement.comparativeStatementItems.Sum(x => x.amountOC));
                e.Value = Math.Round(totalValue, 2);
            }
            if (e.Field.FieldName == "totalAmountMER")
            {

                var Id = Convert.ToInt32(e.GetListSourceColumnValue("Id"));
                var statement = offer.comparativeStatements.FirstOrDefault(x => x.Id == Id);
                var totalValue = Convert.ToDouble(statement.comparativeStatementItems.Sum(x => x.amountMER));
                e.Value = Math.Round(totalValue,2);
            }
        }

        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            grdComparativeStatement.Visibility = Visibility.Collapsed;
            grdPivotComparativeStatement.Visibility = Visibility.Visible;
            btnPrint.Visibility = Visibility.Collapsed;
        }

        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            grdComparativeStatement.Visibility = Visibility.Visible;
            grdPivotComparativeStatement.Visibility = Visibility.Collapsed;
            btnPrint.Visibility = Visibility.Visible;
        }

        private void grdPivotComparativeStatement_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //var statement= grdPivotComparativeStatement.GetCellValue( as ComparativeStatement;
            //if (statement != null)
            //{

            //    frmComparativeStatement frmComparativeStatement = new frmComparativeStatement();
            //    frmComparativeStatement.compId = statement.Id;
            //    frmComparativeStatement.offerId = (int)statement.offerId;
            //    frmComparativeStatement.Show();
            //}

        }



        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

            if (grdComparativeStatement.Visibility == Visibility.Collapsed)
            {
                //string DocumentName = "Preview Document";
                //string Title = "Print Preview";

                //grdPivotComparativeStatement.ShowPrintPreview(this, DocumentName, Title);

            }
            else
            {
                DataTemplate dataTemplate = new DataTemplate("Preview Document");
                //dataTemplate.Template = new TextBlock();
                PrintableControlLink link = new PrintableControlLink((TableView)grdComparativeStatement.View);
                link.PaperKind = System.Drawing.Printing.PaperKind.A2;
                link.PageHeaderData = "Preview Document";
                link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
                link.DocumentName = "Preview Document";
                link.ReportHeaderData = "Preview Document";
                link.ReportHeaderTemplate = dataTemplate;
                link.Landscape = true;
                // Show a preview. 
                DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            }

        }
    }
}
