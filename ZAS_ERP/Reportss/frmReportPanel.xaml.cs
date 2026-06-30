using DevExpress.XtraReports.UI;
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
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Configuration;

using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using DevExpress.Xpf.Reports.UserDesigner;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Reports.UserDesigner.FieldList;
using DevExpress.Xpf.Docking;
using DevExpress.XtraReports.Wizards;
using System.ComponentModel.Design;
using DevExpress.Xpf.Reports.UserDesigner.ReportWizard;
using DevExpress.Xpf.DataAccess.DataSourceWizard;
using DevExpress.DataAccess.Wizard.Presenters;
using DevExpress.DataAccess.Wizard.Model;
//using DevExpress.Data.XtraReports.Wizard.Presenters;
using DevExpress.Utils.IoC;
using DevExpress.DataAccess.UI.Wizard;
using DevExpress.Xpf.Reports.UserDesigner.ReportWizard.Pages;
using DevExpress.XtraExport.Helpers;
using DevExpress.Xpf.Grid.Printing;


namespace ZAS_ERP.Reportss
{
    /// <summary>
    /// Interaction logic for frmReportPanel.xaml
    /// </summary>
    public partial class frmReportPanel : Window
    {

        //string reportFilePath = System.AppDomain.CurrentDomain.BaseDirectory;
        //string styleSheetFilePath = System.AppDomain.CurrentDomain.BaseDirectory;
        DockLayoutManager Manager { get; set; }
        LayoutPanel FieldListPanel { get; set; }
        public frmReportPanel()
        {
            InitializeComponent();
        }
        XtraReport xtraReport = new XtraReport();

        public frmReportPanel(XtraReport Report)
        {
            try
            {
                //reportFilePath = reportFilePath + /*Report+*/"reportOfferSingle1.repx";
                //styleSheetFilePath = styleSheetFilePath + /*Report +*/ "reportOfferSingle1.repss";
                InitializeComponent();


                //SystemLogic.SetLayoutOfCurrentReport(Report);
                // Load a report's layout from XML. 
                //if (System.IO.File.Exists(reportFilePath))
                //{
                //    xtraReport.LoadLayout(reportFilePath);
                //}
                //else
                //{
                //    MessageBox.Show("The source file does not exist." +reportFilePath);
                //}

                //// Load a report's style sheet from XML. 
                //if (System.IO.File.Exists(styleSheetFilePath))
                //{
                //    xtraReport.StyleSheet.LoadFromXml(styleSheetFilePath);
                //}
                //else
                //{
                //    MessageBox.Show("The source file does not exist.");
                //}
                //Report.LoadLayout(Report.Name + ".repx");
                reportDesign.Loaded += designer_Loaded;
                reportDesign.DocumentOpened += designer_DocumentOpened;

                //reportDesign.Commands= new CustomDesignerCommands();

                //// Un comment this to see the Print preview on load
                ReportDesignerDocument document = reportDesign.OpenDocument(Report);
                document.ViewKind = ReportDesignerDocumentViewKind.Preview;

                //reportDesign.ActiveDocument.Report.LoadLayout(Report.Name + ".repx");
                //xtraReport = Report;

                //reportDesign.OpenDocument(xtraReport);
                FilterFieldList();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message + "error at panel constructor");
            }
           
        }
        public frmReportPanel(XtraReport firstPage, XtraReport secondPage)
        {
            try
            {
                //reportFilePath = reportFilePath + /*Report+*/"reportOfferSingle1.repx";
                //styleSheetFilePath = styleSheetFilePath + /*Report +*/ "reportOfferSingle1.repss";
                InitializeComponent();
            }
            catch { }

            //SystemLogic.SetLayoutOfCurrentReport(Report);
            // Load a report's layout from XML. 
            //if (System.IO.File.Exists(reportFilePath))
            //{
            //    xtraReport.LoadLayout(reportFilePath);
            //}
            //else
            //{
            //    MessageBox.Show("The source file does not exist." +reportFilePath);
            //}

            //// Load a report's style sheet from XML. 
            //if (System.IO.File.Exists(styleSheetFilePath))
            //{
            //    xtraReport.StyleSheet.LoadFromXml(styleSheetFilePath);
            //}
            //else
            //{
            //    MessageBox.Show("The source file does not exist.");
            //}
            //Report.LoadLayout(Report.Name + ".repx");
            reportDesign.Loaded += designer_Loaded;
            reportDesign.DocumentOpened += designer_DocumentOpened;

            //reportDesign.Commands= new CustomDesignerCommands();

            //// Un comment this to see the Print preview on load
            ReportDesignerDocument document = reportDesign.OpenDocument(firstPage);
            secondPage.DisplayName = "Page 2";
            ReportDesignerDocument secondDocument = reportDesign.OpenDocument(secondPage);

            secondDocument.ViewKind = ReportDesignerDocumentViewKind.Preview;
            document.ViewKind = ReportDesignerDocumentViewKind.Preview;

            //reportDesign.ActiveDocument.Report.LoadLayout(Report.Name + ".repx");
            //xtraReport = Report;

            //reportDesign.OpenDocument(xtraReport);
            FilterFieldList();
        }

        //public frmReportPanel(IGridViewFactory<ColumnWrapper, RowBaseWrapper> factory)
        //{

        //    InitializeComponent();

        //    reportDesign.Loaded += designer_Loaded;
        //    reportDesign.DocumentOpened += designer_DocumentOpened;
        //    //reportDesign.Commands= new CustomDesignerCommands();

        //    //// Un comment this to see the Print preview on load
        //    //ReportDesignerDocument document = reportDesign.OpenDocument(Report);
        //    //document.ViewKind = ReportDesignerDocumentViewKind.Preview;

        //    //reportDesign.ActiveDocument.Report.LoadLayout(Report.Name+".repx");
        //    xtraReport = Report;
        //    FilterFieldList();
        //}

        void designer_DocumentOpened(object sender, DevExpress.Xpf.Reports.UserDesigner.ReportDesignerDocumentEventArgs e)
        {

        }

        private void FilterFieldList()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                List<LayoutPanel> lst = Manager.GetItems().OfType<LayoutPanel>().ToList();
                //FieldListPanel = lst.Where(x => x.Caption != null && x.Caption.Equals("Field List")).FirstOrDefault();
                FieldListPanel = lst.FirstOrDefault(x => x.Caption != null && x.Caption.ToString() == "Field List");

                if (FieldListPanel != null)
                    FieldListPanel.Loaded += fieldListPanel_Loaded;

            }), System.Windows.Threading.DispatcherPriority.Background);
        }

        void fieldListPanel_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                TreeListControl fieldList = LayoutHelper.FindElementByType(FieldListPanel, typeof(TreeListControl)) as TreeListControl;
                if (fieldList != null)
                {
                    fieldList.BeginDataUpdate();
                    (fieldList.View as TreeListView).CustomNodeFilter += MainWindow_CustomNodeFilter;
                    fieldList.EndDataUpdate();
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void MainWindow_CustomNodeFilter(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeFilterEventArgs e)
        {
            TreeListNode node = e.Node;
            if (!e.Node.HasChildren)
            {
                object obj = e.Node.Content;
                if (((FieldListNodeBase)obj).DataMember.Contains("user"))
                {
                    e.Visible = false;
                    e.Handled = true;
                }
            }
        }

        void designer_Loaded(object sender, RoutedEventArgs e)
        {
            Manager = LayoutHelper.FindElementByType(reportDesign, typeof(DockLayoutManager)) as DockLayoutManager;
       
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            reportDesign.ReportStorage = new ReportStorage();

            reportDesign.UnregisterHotKey(Key.N, ModifierKeys.Control);
            reportDesign.UnregisterHotKey(Key.S, ModifierKeys.Control);
            reportDesign.UnregisterHotKey(Key.S, ModifierKeys.Shift);
            reportDesign.ActualCommands.NewDocumentCommand.CanExecute(false);
            reportDesign.ActualCommands.SaveDocumentCommand.CanExecute(false);
            reportDesign.ActualCommands.SaveDocumentAsCommand.CanExecute(false);
            reportDesign.ActualCommands.CloseDocumentCommand.CanExecute(false);
            reportDesign.ActualCommands.ExitCommand.CanExecute(false);
            reportDesign.Commands.SaveDocumentCommand.CanExecute(false);
            reportDesign.Commands.NewDocumentCommand.CanExecute(false);
            reportDesign.Commands.OpenDocumentCommand.CanExecute(false);
            reportDesign.Commands.CloseDocumentCommand.CanExecute(false);

            reportDesign.Commands.ExitCommand.CanExecute(false);
            reportDesign.Commands.Designer.Visibility = Visibility.Visible;





            ////reportDesigneTest.NewDocument += reportDesigneTest.NewDocument();
            ////reportDesigneTest.ActualCommands.NewReportWizard.
            //InquiryReport report = new InquiryReport();

            //    report.Parameters["InquiryId"].Value = 7;
            //report.Parameters["InquiryId"].Visible = false;
            ////ReportPrintToolWpf window = new ReportPrintToolWpf(report);
            ////window.ShowPreviewDialog(this);
            ////PrintHelper.Print(report);

            //reportDesigneTest.OpenDocument(report);

        }

        private void BtnOpen_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }
        public void SaveReportLayoutToDB()
        {

            if (reportDesign.ActiveDocument != null)
                SYSTEM_STATIC.SaveLayoutForCurrentReport(reportDesign.ActiveDocument.Report);
        }
        private void BtnSave_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            SaveReportLayoutToDB();
            //if (reportDesign.ActiveDocument!=null)
            //SystemLogic.SaveLayoutForCurrentReport(reportDesign.ActiveDocument.Report);

            //String s=System.AppDomain.CurrentDomain.BaseDirectory;
            //reportDesign.ActiveDocument.Report.SaveLayout(reportFilePath);
            ////reportDesign.ActiveDocument.Report.SaveLayoutToXml(s+reportDesign.ActiveDocument.Report.Name+".Xml");
            //reportDesign.ActiveDocument.Report.StyleSheet.SaveXmlToFile(styleSheetFilePath);
        }

        private void ReportDesign_DocumentOpened(object sender, DevExpress.Xpf.Reports.UserDesigner.ReportDesignerDocumentEventArgs e)
        {
            //e.Document.Report.DataSource = xtraReport.DataSource;
        }

        private void ReportDesign_DocumentSaved(object sender, DevExpress.Xpf.Reports.UserDesigner.ReportDesignerDocumentEventArgs e)
        {
            //MemoryStream stream = new MemoryStream();
            //report.SaveLayout(stream);
            //InquiryReport inquiryReport = new InquiryReport();
            //e.Document.Report.Report.Cast =  inquiryReport;
        }
    }
    public class MyWizardCustomizationService : IWizardCustomizationService
    {
        void IDataSourceWizardCustomizationService.CustomizeDataSourceWizard(DataSourceWizardCustomizationModel customization,
            ViewModelSourceIntegrityContainer container)
        {
            if (customization.StartPage == typeof(ChooseDataSourceTypePage<IDataSourceModel>))
            {
                customization.Model.DataSourceType = DataSourceType.Xpo;
                customization.StartPage = typeof(ConnectionPropertiesPage<IDataSourceModel>);
            }
            CustomizeProviders(container);
        }

        void IWizardCustomizationService.CustomizeReportWizard(ReportWizardCustomizationModel customization,
            ViewModelSourceIntegrityContainer container)
        {
            if (customization.StartPage == typeof(ChooseReportTypePage))
            {
                customization.Model.ReportType = ReportType.Standard;
                customization.Model.DataSourceType = DataSourceType.Xpo;
                customization.StartPage = typeof(ConnectionPropertiesPage<IDataSourceModel>);
            }
            CustomizeProviders(container);
        }

        bool IDataSourceWizardCustomizationService.TryCreateDataSource(IDataSourceModel model,
            out object dataSource, out string dataMember)
        {
            dataSource = null;
            dataMember = null;
            return false;
        }

        bool IWizardCustomizationService.TryCreateReport(XtraReportModel model, out XtraReport report)
        {
            report = null;
            return false;
        }

        static void CustomizeProviders(IntegrityContainer container)
        {
            var providers = container.Resolve<List<ProviderLookupItem>>();
            providers.RemoveAll((ProviderLookupItem x) => x.ProviderKey != "MSSqlServer");
        }
    }

    public class CustomDesignerCommands : ReportDesignerCommands
    {
        protected override void SaveDocumentAs()
        {
            MessageBox.Show("CustomDesignerCommands.SaveDocumentAs");
            //ReportDesignerCommands.GetPropertyName<RibbonCustomization.>
            //     ReportDesignerCommands.ReferenceEquals.clo
        }
        protected override void SaveDocument()
        {
            MessageBox.Show("CustomDesignerCommands.SaveDocumentAs");
            //base.Designer.ActiveDocument.Saved();

            //if (reportDesign.ActiveDocument != null)
            //    SystemLogic.SaveLayoutForCurrentReport(reportDesign.ActiveDocument.Report);
        }
        protected override void CloseDocument()
        {
            MessageBox.Show("CustomDesignerCommands.SaveDocumentAs");
            //if (reportDesign.ActiveDocument != null)
            //    SystemLogic.SaveLayoutForCurrentReport(reportDesign.ActiveDocument.Report);
        }

    }
}
