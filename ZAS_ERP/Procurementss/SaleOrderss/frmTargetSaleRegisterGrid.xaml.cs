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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERP_BL.Databases;
using ERP_BL;
using DevExpress.Xpf.Grid;
using DevExpress.XtraReports.UI;
using DevExpress.XtraExport.Helpers;
using DevExpress.Xpf.Grid.Printing;
using DevExpress.Xpf.Reports.UserDesigner;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using System.Collections.ObjectModel;
using ERP_BL.Enums;
using System.Windows.Threading;

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for frmTargetSaleRegisterGrid.xaml
    /// </summary>
    public partial class frmTargetSaleRegisterGrid : Window
    {
        public static int statusId;
        public static int AllActive;
        public frmTargetSaleRegisterGrid(int companyId, int departmentId, int Year, int month, TargetFrequency type)
        {

            InitializeComponent();
            UsersRepo usersRepo = new UsersRepo();

            if (MainWindow.currentUserid != 0)
                if (type == TargetFrequency.Yearly)
                    saleOrders = saleOrderrepo.getDepartmentalSOForAllPrecedingMonths(departmentId, companyId, Year,month);
                else if (type == TargetFrequency.Monthly)
                    if (month <= 12)
                    saleOrders = saleOrderrepo.getDepartmentalSOByMonth(departmentId, companyId, Year, month);
                else
                    saleOrders = saleOrderrepo.getDepartmentalSOByYear(departmentId, companyId, Year);


            else
                saleOrders = saleOrderrepo.getSaleRegisterAdministrator();
            grdsaleOrder.ItemsSource = saleOrders;
            grdsaleOrder.Columns["CreationDate"].VisibleIndex = 0;
            try
            {
                grdsaleOrder.Columns.GetColumnByFieldName("Id").Visible = false;
                //grdsaleOrder.Columns.GetColumnByFieldName("Id").Visible = false;
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("company"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("incoterm"));
                //grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offerStatus"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("paymentTerm"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue1"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue2"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("currency"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("bid"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("saleOrderStatus"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendor"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("employee"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("department"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("customerCompany"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("principal"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("company_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("incoterm_Id"));
                //grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offerStatus"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("paymentterm_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue1Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("TitleValue2Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("currency_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("bid_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offer_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("offer"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("allocation_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("dept_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("customerCompany_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("principal_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("user_Id"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("user"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendorPaymentId"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("vendorPaymentStatus"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("CostSheetId"));
                grdsaleOrder.Columns.Remove(grdsaleOrder.Columns.GetColumnByFieldName("CostSheet"));
                grdsaleOrder.Columns.GetColumnByFieldName("referenceNo").Header = "SO Refrence";
            }
            catch
            {

            }


        }

        SaleOrderRepo saleOrderrepo = new SaleOrderRepo();
        SaleOrder saleOrder = new SaleOrder();
        public IList<ERP_BL.Databases.SaleOrder> saleOrders { get; set; }



        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            //if (e.IsGetData)
            {
                if (e.Column.FieldName == "PaymentDueAgeingDays" && e.IsGetData)

                {
                    if (e.GetListSourceFieldValue("PaymentDueAgeing") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("PaymentDueAgeing"));

                        //DateTime date;
                        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                        e.Value = NoDueAgeingDays;
                    }
                }

                if (e.Column.FieldName == "Department")

                {

                    // string s = "Test: FieldTwo";

                }
                if (e.Column.FieldName == "CreationAgeing")

                {
                    if (e.GetListSourceFieldValue("CreationDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("CreationDate"));

                        Double CreateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = CreateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "SODateAgeing")

                {
                    if (e.GetListSourceFieldValue("saleOrderDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("saleOrderDate"));

                        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                        e.Value = SoDateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "SODeliveryAgeing")

                {
                    if (e.GetListSourceFieldValue("deliveryDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("deliveryDate"));

                        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                        e.Value = SoDateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "totalWeight")

                {
                    if (e.GetListSourceFieldValue("products") != null)
                    {
                        List<ProcurementProduct> products = (e.GetListSourceFieldValue("products")) as List<ProcurementProduct>;

                        decimal? totalweight = 0;
                        foreach (var pro in products)
                        {
                            if (pro.inquiryProduct.Weight != null || pro.inquiryProduct.Weight != 0)
                            {
                                totalweight = pro.inquiryProduct.Weight;
                            }
                        }
                        //CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = totalweight;
                        // string s = "Test: FieldTwo";
                    }
                }
                if (e.Column.FieldName == "totalQuantity")

                {
                    if (e.GetListSourceFieldValue("products") != null)
                    {
                        List<ProcurementProduct> products = (e.GetListSourceFieldValue("products")) as List<ProcurementProduct>;

                        double totalquantity = 0;
                        foreach (var pro in products)
                        {
                            if (pro.inquiryProduct.quantity != 0)
                            {
                                totalquantity = pro.inquiryProduct.quantity;
                            }
                        }
                        //CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = totalquantity;
                        // string s = "Test: FieldTwo";
                    }
                }
                //CreationDate


                //int unitsOnOrder = Convert.ToInt32(e.GetListSourceFieldValue("UnitsOnOrder"));
                //e.Value = price * unitsOnOrder;
            }
        }
        /// <summary>
        /// Loads data into saleOrder Grid and create columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void grdsaleOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSaleOrder();

        }

        private void btnNewSaleOrder_Click(object sender, RoutedEventArgs e)
        {
           

            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order,0);
            procurmentPanel.Show();
        }
        private void EditSaleOrder()
        {
            if (grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, (int)grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();

            }
        }
        private void btnEditSaleOrder_Click(object sender, RoutedEventArgs e)
        {
            EditSaleOrder();

        }

        private void UcsaleOrdergrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //AllActive = 0;
            //SystemLogic.SaveUserSettingForCurrentWindow(grdsaleOrder);
            //statusId = 0;
        }

        private void RbtnCopy_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdsaleOrder.View.ShowPrintPreview(this);
            //ShowDesigner(tableView);
        }




        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdsaleOrder.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdsaleOrder.View.ShowPrintPreview(this);
        }



        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdsaleOrder);
        }

        private void MbtnLoadLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdsaleOrder);
        }

        private void frmTargetSaleRegisterGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
