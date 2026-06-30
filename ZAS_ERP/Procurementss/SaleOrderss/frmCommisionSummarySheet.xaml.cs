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
using ERP_BL.Config;
using System.Printing;


using ERP_BL.Enums;
using System.IO;
using System.Windows.Xps.Packaging;
using System.IO.Packaging;
using System.Windows.Xps.Serialization;

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for frmCommissionSummarySheet.xaml
    /// </summary>
    public partial class frmCommissionSummarySheet : Window
    {
        public InquiryType commisionSummarySheetType;
        public static int commissionSummarySheetId = 0;
        public static CommissionSummarySheet summarySheet = new CommissionSummarySheet();
        public List<CostFieldValues> costFieldValue = new List<CostFieldValues>();
        public List<SummaryFieldValue> fieldValues = new List<SummaryFieldValue>();

        public SaleOrderRepo repo = new SaleOrderRepo();
        public frmCommissionSummarySheet()
        {
            InitializeComponent();
        }
        //public frmCommissionSummarySheet(Offer offer)
        //{
        //    InitializeComponent();
        //    if (offer != null)
        //    {
        //        if (offer.offertype == InquiryType.Principal)
        //        {
        //            commisionSummarySheetType = offer.offertype;
        //            grdprincipleCostItems.Visibility = Visibility.Visible;
        //            if (offer.currency != null)
        //            {
        //                grdprincipleCostItems.Columns.GetColumnByFieldName("budgetedValue").Header += "(" + offer.currency.Abbrivation + " " + offer.currency.Symbol + ")";
        //                //txtCurrency.Text = saleOrder.currency.CurrencyName + "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";
        //                //grdCostItems.Columns.GetColumnByFieldName("actualValue").Header += "(" + saleOrder.currency.Abbrivation + " " + saleOrder.currency.Symbol + ")";

        //            }

        //            lbtotal.Text = "Total Costs";
        //            lblgrosstotal.Text = "Total Commission";
        //            lblTotalActualcosts.Visibility = Visibility.Collapsed;
        //            lblTotalActualmargin.Visibility = Visibility.Collapsed;
        //            txtActualTotal.Visibility = Visibility.Collapsed;
        //            txtActualmarginTotal.Visibility = Visibility.Collapsed;
        //            grdCostItems.Visibility = Visibility.Collapsed;
        //        }
        //        lblCustomer.Text = offer.customerCompany.company.CompanyName;
        //        lblDepartment.Text = offer.department.DeptName;
        //        lblSORefrence.Text = offer.SalesReferenceNo;
        //        txtSOTotal.Text = offer.totalCFRValue.ToString();
        //    }
        //    grpDocuments.Visibility = Visibility.Visible;
        //    grpReviewDetails.Visibility = Visibility.Visible;
        //    grpTerms.Visibility = Visibility.Visible;
        //}
        public frmCommissionSummarySheet(SaleOrder saleOrder, string FinanceRefrenceNo, string SOReferenceNo, string OfferrefrenceNo, string customer, int currencyId, string Commisionamount, string SoDate, string paymenttermcustomer, InquiryType type, string principle, string SOCFRAmount, string SOFOBAmount, string SOpakcing, string Sodeliveryterm, DateTime SodeliveryDate, string Sotranshipment, string Creationdate, string SalesReferenceNo, List<ViewInfo> viewinfos)
        {
            InitializeComponent();
            loadCurrencies();
            txtSOCommision.Text = Commisionamount;
            if (saleOrder != null)
            {
                //if (saleOrder.customerCompany != null)
                {
                    lblCustomer.Text = customer;
                    lblPrincipal.Text = principle;
                    lblOfferRefrence.Text = OfferrefrenceNo;
                    lblSORefrence.Text = SOReferenceNo;
                    lblFinRef.Text = FinanceRefrenceNo;
                    lblSalesRefno.Text = SalesReferenceNo;
                    if (Creationdate != "")
                        lblCreationDate.Text = Creationdate;
                    else
                        lblCreationDate.Text = (System.DateTime.Now).ToShortDateString();
                    if (SoDate != "")
                        lblSODate.Text = SoDate;
                    else
                        lblSODate.Text = (System.DateTime.Now).ToShortDateString();
                    if (type == InquiryType.Principal)
                    {
                        commisionSummarySheetType = type;

                        if (currencyId != 0)
                        {
                            var currencySource = (List<cmbitem>)cmbSoCurrency.Items.SourceCollection;
                            cmbSoCurrency.SelectedItem = cmbSoCurrency.Items[cmbSoCurrency.Items.IndexOf(currencySource.Find(x => x.id == currencyId))];

                        }
                        else if (saleOrder.currency != null)
                        {
                            var currencySource = (List<cmbitem>)cmbSoCurrency.Items.SourceCollection;
                            cmbSoCurrency.SelectedItem = cmbSoCurrency.Items[cmbSoCurrency.Items.IndexOf(currencySource.Find(x => x.id == saleOrder.currency_Id))];

                        }
                        //lblTotalActualcosts.Visibility = Visibility.Collapsed;
                    }

                    txtSOCommision.Text = Commisionamount;
                    txtSOCFRAmount.Text = SOCFRAmount;
                    txtSOFOBAmount.Text = SOFOBAmount;
                    txtSOPacking.Text = SOpakcing;
                    txtSOPaymentTerm.Text = paymenttermcustomer;
                    txtSODeliveryTerm.Text = Sodeliveryterm;
                    datSODeliverydate.EditValue = SodeliveryDate;
                    cmbSOTransshipment.Text = Sotranshipment;


                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Offer Fields") != null)
                {
                    //grdCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;
                    datOfferDeliverydate.IsEnabled = true;
                    cmbOfferTransshipment.IsEnabled = true;
                    cmbOfferCurrency.IsEnabled = true;
                    txtOfferCFRAmount.IsReadOnly = false;
                    txtOfferCommision.IsReadOnly = false;
                    txtOfferDeliveryTerm.IsReadOnly = false;
                    txtOfferFOBAmount.IsReadOnly = false;
                    txtOfferPacking.IsReadOnly = false;
                    txtOfferPaymentTerm.IsReadOnly = false;

                }
                else
                {
                    datOfferDeliverydate.IsEnabled = false;
                    cmbOfferTransshipment.IsEnabled = false;
                    cmbOfferCurrency.IsEnabled = false;
                    txtOfferCFRAmount.IsReadOnly = true;
                    txtOfferCommision.IsReadOnly = true;
                    txtOfferDeliveryTerm.IsReadOnly = true;
                    txtOfferFOBAmount.IsReadOnly = true;
                    txtOfferPacking.IsReadOnly = true;
                    txtOfferPaymentTerm.IsReadOnly = true;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit SaleOrder Fields") != null)
                {

                    datSODeliverydate.IsEnabled = true;
                    cmbSOTransshipment.IsEnabled = true;
                    cmbSoCurrency.IsEnabled = true;
                    txtSOCFRAmount.IsReadOnly = false;
                    txtSOCommision.IsReadOnly = false;
                    txtSODeliveryTerm.IsReadOnly = false;
                    txtSOFOBAmount.IsReadOnly = false;
                    txtSOPacking.IsReadOnly = false;
                    txtSOPaymentTerm.IsReadOnly = false;
                }
                else
                {
                    datSODeliverydate.IsEnabled = false;
                    cmbSOTransshipment.IsEnabled = false;
                    cmbSoCurrency.IsEnabled = false;
                    txtSOCFRAmount.IsReadOnly = true;
                    txtSOCommision.IsReadOnly = true;
                    txtSODeliveryTerm.IsReadOnly = true;
                    txtSOFOBAmount.IsReadOnly = true;
                    txtSOPacking.IsReadOnly = true;
                    txtSOPaymentTerm.IsReadOnly = true;
                }
                if (saleOrder.CommissionSummarySheet != null)
                    summarySheet = saleOrder.CommissionSummarySheet;
                else
                {
                    summarySheet = new CommissionSummarySheet();
                }
            }

            loadComments(saleOrder, viewinfos);

        }

        public void loadComments(SaleOrder saleOrder, List<ViewInfo> viewinfos)
        {
            if (saleOrder != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                List<CommentLog> comments = new List<CommentLog>();
                if (saleOrder.isApproved == true)
                    comments = procurementRepo.getcommentslogBeforeApproval(saleOrder.Id, TransactionItemType.Sale_Order, saleOrder.ApprovedDate);
                else if (saleOrder.isApproved == true && saleOrder.PendingForClosing == true)
                {
                    comments = procurementRepo.getcommentslogAfterApproval(saleOrder.Id, TransactionItemType.Sale_Order, saleOrder.ApprovedDate);

                }
                else if (saleOrder.isApproved != true)
                {
                    comments = procurementRepo.getcommentslogAsc(saleOrder.Id, TransactionItemType.Sale_Order);

                }
                else if (saleOrder.PendingForClosing == false)
                {
                    var info = viewinfos.FirstOrDefault(z => z.Info == "Approved_Closing");
                    comments = procurementRepo.getcommentslogBetweenDates(saleOrder.Id, TransactionItemType.Sale_Order, saleOrder.ApprovedDate, info.Timestamp);

                }
                List<cmbitem> items = new List<cmbitem>();
                foreach (CommentLog comment in comments)
                {
                    string Tagged = "";/*System.Environment.NewLine + */
                    if (comment.TaggedList != null && comment.TaggedList.Count != 0)
                    {
                        Tagged = " Tagged : ";
                        foreach (var user in comment.TaggedList)
                        {
                            Tagged += user.userName + ", ";
                        }
                    }
                    //if (comment.UserId == MainWindow.currentUserid)
                    //{
                    //    items.Add(new cmbitem()
                    //    {
                    //        id = comment.Id,
                    //        name = comment.User.employee.person.FName + " " + comment.User.employee.person.LName + "( Me)",
                    //        description = comment.Comment/* + Tagged*/,
                    //        bcolor = "#B1FB17",
                    //    });

                    //}
                    //else
                    //{
                    items.Add(new cmbitem()
                    {
                        id = comment.Id,
                        name = comment.employee.person.FName + " " + comment.employee.person.LName,
                        description = comment.Comment + Tagged,
                        fcolor = comment.Timestamp.ToShortDateString() + ", " + comment.Timestamp.ToLongTimeString(),
                        //bcolor = "#7DFDFE"
                    });
                    //}
                }
                lstComments.ItemsSource = items;
            }
        }
        private void loadCurrencies()
        {

            CurrencyRepo currencyRepo = new CurrencyRepo();
            List<Currency> currencies = currencyRepo.getAll();
            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            foreach (Currency cur in currencies)
            {

                //cmbCurrency.Items.Add(new cmbitem() { name = cur.CurrencyName, id = cur.Id });
                cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
            }

            cmbOfferCurrency.ItemsSource = cmbitems;
            //if (cmbbaseCurrency.ItemsSource == null)
            cmbSoCurrency.ItemsSource = cmbitems;

        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (commisionSummarySheetType == InquiryType.Principal)
            {
                GetCostFieldValues();
                summarySheet.FieldValues = fieldValues;
                if (!string.IsNullOrEmpty(txtOfferCFRAmount.Text))
                    summarySheet.totalOfferCFR = Convert.ToDouble(txtOfferCFRAmount.Text);
                if (!string.IsNullOrEmpty(txtOfferFOBAmount.Text))
                    summarySheet.totalOfferFOB = Convert.ToDouble(txtOfferFOBAmount.Text);
                if (!string.IsNullOrEmpty(txtOfferCommision.Text))
                    summarySheet.OfferCommission = Convert.ToDouble(txtOfferCommision.Text);
                if (!string.IsNullOrEmpty(txtOfferPacking.Text))
                    summarySheet.OfferPacking = txtOfferPacking.Text;
                if (!string.IsNullOrEmpty(txtOfferPaymentTerm.Text))
                    summarySheet.Offerpaymentterm = txtOfferPaymentTerm.Text;
                if (!string.IsNullOrEmpty(txtOfferDeliveryTerm.Text))
                    summarySheet.OfferDeliveryTerm = txtOfferDeliveryTerm.Text;
                if (cmbOfferTransshipment.SelectedIndex == 0)
                    summarySheet.Offertranshipment = true;
                if (cmbOfferTransshipment.SelectedIndex == 1)
                    summarySheet.Offertranshipment = false;
                summarySheet.OfferDeliveryDate = datOfferDeliverydate.DateTime;
                summarySheet.SOCommission = (string.IsNullOrEmpty(txtSOCommision.Text.Trim())) ? 0 : Convert.ToDouble(txtSOCommision.Text);
                summarySheet.netSOCommission = (string.IsNullOrEmpty(txtNetSOCommision.Text.Trim())) ? 0 : Convert.ToDouble(txtNetSOCommision.Text);
                summarySheet.netOfferCommission = (string.IsNullOrEmpty(txtNetOfferCommision.Text.Trim())) ? 0 : Convert.ToDouble(txtNetOfferCommision.Text);

                summarySheet.Timestamp = System.DateTime.Now;
                if (cmbOfferCurrency.SelectedIndex != -1)
                {
                    summarySheet.OfferCurrencyId = (cmbOfferCurrency.SelectedItem as cmbitem).id;
                }
            }


            this.Close();
        }


        public void loadValues()
        {
            txtOfferCFRAmount.Text = summarySheet.totalOfferCFR.ToString();
            txtOfferFOBAmount.Text = summarySheet.totalOfferFOB.ToString();
            txtOfferCommision.Text = summarySheet.OfferCommission.ToString();
            txtOfferPacking.Text = (string.IsNullOrEmpty(summarySheet.OfferPacking)) ? "" : summarySheet.OfferPacking.ToString();
            txtOfferPaymentTerm.Text = (string.IsNullOrEmpty(summarySheet.Offerpaymentterm)) ? "" : summarySheet.Offerpaymentterm.ToString();
            txtOfferDeliveryTerm.Text = (string.IsNullOrEmpty(summarySheet.OfferDeliveryTerm)) ? "" : summarySheet.OfferDeliveryTerm.ToString();
            datOfferDeliverydate.EditValue = summarySheet.OfferDeliveryDate;
            if (summarySheet.Offertranshipment == true)
                cmbOfferTransshipment.SelectedIndex = 0;
            if (summarySheet.Offertranshipment == false)
                cmbOfferTransshipment.SelectedIndex = 1;

            if (summarySheet.OfferCurrencyId != null && summarySheet.OfferCurrencyId != 0)
            {
                var currencySource = (List<cmbitem>)cmbSoCurrency.Items.SourceCollection;
                cmbOfferCurrency.SelectedItem = cmbSoCurrency.Items[cmbSoCurrency.Items.IndexOf(currencySource.Find(x => x.id == summarySheet.OfferCurrencyId))];

            }

        }


        void loadMarketingFields()
        {
            foreach (var item in repo.getActiveSummarySheetFields())
            {
                costFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title });
            }
            grdSummaryItems.ItemsSource = costFieldValue;

        }
        void loadMarketingFieldValues()
        {
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costfieldValues = grdSummaryItems.ItemsSource as List<CostFieldValues>;
            foreach (var item in costfieldValues)
            {
                if(summarySheet.FieldValues!=null)
                foreach (var field in summarySheet.FieldValues)
                    if (item.Id == field.FieldId)
                    {
                        //if (field.Type == 2)
                        //    item.actualValue = field.Value;
                        //else if (field.Type == 1)
                            item.budgetedValue = field.Value;
                    }
                costfields.Add(item);

            }
            grdSummaryItems.ItemsSource = costfields;

        }
        public/* List<FieldValue>*/ void GetCostFieldValues()
        {
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            fieldValues = new List<SummaryFieldValue>();
            
                costfieldValues = grdSummaryItems.ItemsSource as List<CostFieldValues>;

                foreach (var item in costfieldValues)
                {
                    fieldValues.Add(new SummaryFieldValue { FieldId = item.Id, Type = 1, Value = item.budgetedValue });

                }
            

            // return fieldValues;
        }
        private void frmCommissionSummarySheet_Loaded(object sender, RoutedEventArgs e)
        {
            loadValues();
            loadMarketingFields();
            loadMarketingFieldValues();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Marketing Cost in Summary Sheet") != null)
            {
                
                grdSummaryItems.Visibility = Visibility.Visible;
                txtNetOfferCommision.Visibility = Visibility.Visible;
                txtNetSOCommision.Visibility = Visibility.Visible;
                lblNetCommision.Visibility = Visibility.Visible;
                CalculateTotal();
            }
            else
            {
                grdSummaryItems.Visibility = Visibility.Collapsed;
                txtNetOfferCommision.Visibility = Visibility.Collapsed;
                txtNetSOCommision.Visibility = Visibility.Collapsed;
                lblNetCommision.Visibility = Visibility.Collapsed;
            }



        }

        private void frmCommissionSummarySheet_Unloaded(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)

        {
            btnPrint.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Collapsed;


            grdComments.Visibility = Visibility.Visible;
            PrintDialog dlg = new PrintDialog();
            var pd = new PrintDialog();
            var pageSize = new Size(8.26 * 96, 11.69 * 96);
            FrameworkElement fe = (grdCostPrint as FrameworkElement);
            fe.Measure(new Size(Int32.MaxValue, Int32.MaxValue));
            Size visualSize = fe.DesiredSize;
            fe.Arrange(new Rect(new Point(0, 0), visualSize));
            MemoryStream stream = new MemoryStream();
            string pack = "pack://temp.xps";
            Uri uri = new Uri(pack);
            DocumentPaginator paginator;
            XpsDocument xpsDoc;

            //using (Package container = Package.Open(stream, FileMode.Create))
            //{
            //    using (xpsDoc = new XpsDocument(container, CompressionOption.Fast, pack))
            //    {
            //        paginator = new VisualDocumentPaginator(paginator,
            //                        new Size(pageSize.Width, pageSize.Height),
            //                                 new Size(48, 48));
            //        XpsSerializationManager rsm = new XpsSerializationManager(
            //                                 new XpsPackagingPolicy(xpsDoc), false);
            //        rsm.SaveAsXaml(paginator);
            //    }
            //    PackageStore.RemovePackage(uri);
            //}
            using (Package container = Package.Open(stream, FileMode.Create))
            {
                PackageStore.AddPackage(uri, container);
                using (xpsDoc = new XpsDocument(container, CompressionOption.Fast, pack))
                {
                    XpsSerializationManager rsm =
                      new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
                    rsm.SaveAsXaml(grdCostPrint);
                    paginator = ((IDocumentPaginatorSource)
                      xpsDoc.GetFixedDocumentSequence()).DocumentPaginator;
                    paginator.PageSize = visualSize;
                }
                PackageStore.RemovePackage(uri);
            }
            if ((bool)dlg.ShowDialog().GetValueOrDefault())
            {
                //Application.Current.MainWindow = currentMainWindow; // do it early enough if the 'if' is entered
                dlg.PrintDocument(paginator, "");
            }

            grdComments.Visibility = Visibility.Collapsed;

            btnPrint.Visibility = Visibility.Visible;
            btnSave.Visibility = Visibility.Visible;
        }

        private void ViewSummaryItems_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            CalculateTotal();
        }
        private void CalculateTotal()
        {
            decimal SumMarketingCost = 0;
            if (grdSummaryItems.ItemsSource != null)
                foreach (var item in grdSummaryItems.ItemsSource as List<CostFieldValues>)
                {
                    {
                        SumMarketingCost += item.budgetedValue;
                    }
                }
            txtNetOfferCommision.Text = (!string.IsNullOrEmpty(txtOfferCommision.Text)) ? (Convert.ToDecimal(txtOfferCommision.Text.Trim()) - SumMarketingCost).ToString() : "0";
            txtNetSOCommision.Text = (!string.IsNullOrEmpty(txtSOCommision.Text)) ? (Convert.ToDecimal(txtSOCommision.Text.Trim()) - SumMarketingCost).ToString() : "0";
            //txtActualTotal.Text = sumactu.ToString();

        }
    }

}
