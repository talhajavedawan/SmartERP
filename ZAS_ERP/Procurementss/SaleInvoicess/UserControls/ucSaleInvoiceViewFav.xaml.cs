using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Migrations;
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
using System.Windows.Threading;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;
using static System.Windows.Forms.DataFormats;

namespace ZAS_ERP.Procurementss.SaleInvoicess.UserControls
{
    /// <summary>
    /// Interaction logic for ucSaleInvoiceViewFav.xaml
    /// </summary>
    public partial class ucSaleInvoiceViewFav : UserControl
    {
        public ucSaleInvoiceViewFav()
        {
            InitializeComponent();
        }
        GridReport report = new GridReport();
        string reportTitle;
        public MainWindow myParent = null;

        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        static List<ExchangeRateGroup> exchangeRateGroups = new List<ExchangeRateGroup>();
        ERP_BL.ExchangeRates.ExchangeRate exchangeRate = null;
        ProcurementRepo procurementRepo = new ProcurementRepo();
        public ucSaleInvoiceViewFav(GridReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;

        }

        private void grdsaleInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSaleInvoice();
        }

        private void EditSaleInvoice()
        {
            if (grdsaleInvoiceReport.SelectedItem != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, (int)(grdsaleInvoiceReport.SelectedItem as ERP_BL.Databases.SaleInvoice).Id);
                procurmentPanel.Show();
            }
        }

        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            SaleInvoiceRepo repo = new SaleInvoiceRepo();
            grdsaleInvoiceReport.ItemsSource = repo.getAll(SYSTEM_STATIC.currentUser.id);
            loadingGif.Visibility = Visibility.Hidden;
        }

        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            //if (e.IsGetData)
            {
                var invoice = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;

                if (e.Column.FieldName == "LotNumberr")
                {
                    string LotNo = "";
                    if (invoice.lotNumber != null)
                        LotNo = invoice.lotNumber.LotNo;
                    else if (!String.IsNullOrEmpty(invoice.lotNo))
                        LotNo = invoice.lotNo;
                    e.Value = LotNo;
                }
                if (e.Column.FieldName == "daysLeft")
                {
                    var row = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row.ExpectedPayment != null)
                    {


                        DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime date = row.ExpectedPayment.Value;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = targetDate.Subtract(currDate);
                        e.Value = timeSpan.Days.ToString();
                    }

                }
                if (e.Column.FieldName == "EdDaysLeft")
                {
                    var row = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row.ExpectedDiscountDate != null)
                    {
                        DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime date = row.ExpectedDiscountDate.Value;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = targetDate.Subtract(currDate);
                        e.Value = timeSpan.Days.ToString();
                    }

                }
                if (e.Column.FieldName == "statusChangeAgeing")
                {
                    var row = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row.LastStatusChangeDate != null)
                    {
                        DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime date = row.LastStatusChangeDate.Value;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = targetDate.Subtract(currDate);
                        e.Value = timeSpan.Days.ToString();
                    }

                }
                if (e.Column.FieldName == "statusClassAgeing")
                {
                    var row = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row.LastStatusClassChangeDate != null)
                    {
                        DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime date = row.LastStatusClassChangeDate.Value;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = targetDate.Subtract(currDate);
                        e.Value = timeSpan.Days.ToString();
                    }

                }
                if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                {
                    var row = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.department;
                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                deptList.Add(node);
                                break;
                            }

                        }
                        deptList.Reverse();
                        e.Value = deptList[0].DeptName;
                    }
                }
                if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
                {
                    var row = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.department;
                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                deptList.Add(node);
                                break;
                            }
                        }
                        deptList.Reverse();
                        switch (deptList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = deptList[0].DeptName;
                                break;
                            case 2:
                                e.Value = deptList[1].DeptName;
                                break;
                            case 3:
                                e.Value = deptList[1].DeptName;
                                break;
                            case 4:
                                e.Value = deptList[1].DeptName;
                                break;
                            case 5:
                                e.Value = deptList[1].DeptName;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
                {
                    var row = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.department;
                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                deptList.Add(node);
                                break;
                            }
                        }
                        deptList.Reverse();
                        switch (deptList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = deptList[0].DeptName;
                                break;
                            case 2:
                                e.Value = deptList[1].DeptName;
                                break;
                            case 3:
                                e.Value = deptList[2].DeptName;
                                break;
                            case 4:
                                e.Value = deptList[2].DeptName;
                                break;
                            case 5:
                                e.Value = deptList[2].DeptName;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
                {
                    var row = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.department;

                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                deptList.Add(node);
                                break;
                            }

                        }
                        deptList.Reverse();
                        switch (deptList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = deptList[0].DeptName;
                                break;
                            case 2:
                                e.Value = deptList[1].DeptName;
                                break;
                            case 3:
                                e.Value = deptList[2].DeptName;
                                break;
                            case 4:
                                e.Value = deptList[3].DeptName;
                                break;
                            case 5:
                                e.Value = deptList[3].DeptName;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
                {
                    var row = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.department;

                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                deptList.Add(node);
                                break;
                            }
                        }
                        deptList.Reverse();

                        switch (deptList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = deptList[0].DeptName;
                                break;
                            case 2:
                                e.Value = deptList[1].DeptName;
                                break;
                            case 3:
                                e.Value = deptList[2].DeptName;
                                break;
                            case 4:
                                e.Value = deptList[3].DeptName;
                                break;
                            case 5:
                                e.Value = deptList[4].DeptName;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "Stage" && e.IsGetData)
                {
                    var saleInvoice = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (saleInvoice.isVoid == true)
                    {
                        e.Value = "Void";
                    }
                    //else if (saleinvoice.isreapproved == false)
                    //{
                    //    e.value = "under approval";
                    //}
                    else if (saleInvoice.isApproved == true && saleInvoice.stage == "Closed")
                    {
                        e.Value = "Closed";
                    }
                    else if (saleInvoice.isApproved == true && saleInvoice.saleInvoiceStatus.isActive == false && saleInvoice.PendingForClosing != true)
                    {
                        e.Value = "Closed";
                    }
                    else if (saleInvoice.isApproved == true && saleInvoice.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                    else if (saleInvoice.isApproved == true)
                    {
                        e.Value = "Approved";
                    }
                    else if (saleInvoice.isApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (saleInvoice.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                }

                if (e.Column.FieldName == "Vendorss" && e.IsGetData)
                {
                    var _SI = grdsaleInvoiceReport.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    string vendorNames = "";

                    if (_SI.vendors != null && _SI.vendors.Count > 0)
                    {
                        vendorNames = String.Join(" | ", _SI.vendors.Select(x => x.company.CompanyName));
                    }
                    e.Value = vendorNames;
                }

                if (e.Column.FieldName == "PaymentDueAgeingDays" && e.IsGetData)

                {
                    if (e.GetListSourceFieldValue("PaymentDueAgeing") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("PaymentDueAgeing"));

                        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                        e.Value = NoDueAgeingDays;
                    }
                }
                if (e.Column.FieldName == "CreationAgeing")

                {
                    if (e.GetListSourceFieldValue("CreationDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("CreationDate"));

                        Double CreateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                        e.Value = CreateAgeingDays;
                    }
                }
                if (e.Column.FieldName == "SODateAgeing")

                {
                    if (e.GetListSourceFieldValue("saleInvoiceDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("saleInvoiceDate"));

                        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                        e.Value = SoDateAgeingDays;
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
                        e.Value = totalweight;
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
                        e.Value = totalquantity;
                    }
                }
                if (e.Column.FieldName == "recieptTotalOC")
                {
                    var reciepts = invoice.salesReceipts.Where(x => x.GLPostingDate <= (DateTime)datAsOff.EditValue);
                    if (reciepts != null)
                    {
                        var result = reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                        e.Value = result;
                    }
                }
                if (e.Column.FieldName == "totalDeductionsOC")
                {
                    var reciepts = invoice.salesReceipts;
                    if (reciepts != null)
                    {
                        var result = reciepts.Where(x => x.isVoid != true).Sum(x => x.receiptDeductions.Sum(y => y.Amount));
                        e.Value = result;
                    }
                }
                if (e.Column.FieldName == "totalCreditedOC")
                {
                    var reciepts = invoice.salesReceipts;
                    if (reciepts != null)
                    {
                        var result = reciepts.Where(x => x.isVoid != true).Sum(x => x.receiptDeductions.Sum(y => y.Amount));
                        var collection = reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                        e.Value = collection - result;
                    }
                }
                if (e.Column.FieldName == "remainingCollectionOC")
                {
                    var reciepts = invoice.salesReceipts.Where(x => x.GLPostingDate <= (DateTime)datAsOff.EditValue);
                    if (reciepts != null && e.GetListSourceFieldValue("totalInvoiceAmount") != null)
                    {
                        var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalInvoiceAmount"));
                        var result = Convert.ToDecimal(reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount));
                        e.Value = total - result;
                    }
                }
                if (e.Column.FieldName == "cMER")
                {
                    double todayRate = 0;
                    var exchangeRateGroupMER = exchangeRateGroups.FirstOrDefault(x => x.transaction_currency_Id == invoice.currency_Id && x.base_currency_Id == invoice.company.CurrencyId && x.TargetYear == DateTime.Now.Year);

                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == invoice.company_Id);
                        switch (DateTime.Now.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJan;
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateFeb;
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMar;
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateApr;
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMay;
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJun;
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJul;
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateAug;
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateSep;
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateOct;
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateNov;
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateDec;
                                break;
                            default:
                                if (exchangeRate != null)
                                    todayRate = 0;
                                break;
                        }
                    }

                    var total = todayRate;
                    e.Value = total;
                }
                if (e.Column.FieldName == "exchangeRate1")
                {
                    double todayRate = 0;
                    var exchangeRateGroupMER = exchangeRateGroups.FirstOrDefault(x => x.transaction_currency_Id == invoice.currency_Id && x.base_currency_Id == invoice.company.CurrencyId && x.TargetYear == invoice.CreationDate.Value.Year);

                    if (exchangeRateGroupMER != null)
                    {
                        exchangeRate = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == invoice.company_Id);
                        switch (invoice.CreationDate.Value.Month)
                        {
                            case 1:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJan;
                                break;
                            case 2:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateFeb;
                                break;
                            case 3:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMar;
                                break;
                            case 4:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateApr;
                                break;
                            case 5:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateMay;
                                break;
                            case 6:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJun;
                                break;
                            case 7:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateJul;
                                break;
                            case 8:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateAug;
                                break;
                            case 9:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateSep;
                                break;
                            case 10:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateOct;
                                break;
                            case 11:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateNov;
                                break;
                            case 12:
                                if (exchangeRate != null)
                                    todayRate = exchangeRate.rateDec;
                                break;
                            default:
                                if (exchangeRate != null)
                                    todayRate = 0;
                                break;
                        }
                    }
                    var total = todayRate;
                    e.Value = total;
                }
                if (e.Column.FieldName == "totalCFRValue")
                {
                    var so = invoice.SaleOrder;
                    if (so != null)
                    {
                        e.Value = so.totalCFRValue;
                    }
                }

            }
        }
        private void MbtnSaveAsNew1_Click(object sender, EventArgs e)
        {
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.lblHeading + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                    ReportLogic.SaveGridReport(grdsaleInvoiceReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.lblHeading + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                    ReportLogic.SaveGridReport(grdsaleInvoiceReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
        private void loadMarketExchangeRates()
        {
            exchangeRateGroups = exchangeRateGroupRepo.GetAllMER();
        }
        private void MbtnRenameReport1_Click(object sender, EventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to rename " + this.lblHeading + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                        ReportLogic.RenameGridReport(grdsaleInvoiceReport, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                        ReportLogic.RenameGridReport(grdsaleInvoiceReport, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.lblHeading + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                    ReportLogic.UpdateGridReport(grdsaleInvoiceReport, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdsaleInvoiceReport, str, reportType, groupDetails, report.Id, report.settingkey);

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
            var inputfromUser = DXMessageBox.Show("Do you want to delete " + this.lblHeading + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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

                                ReportLogic.DeleteReport(grdsaleInvoiceReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                                ReportLogic.DeleteReport(grdsaleInvoiceReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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
            var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.lblHeading + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetReportById(report.Id);
                grdsaleInvoiceReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.lblHeading + " to Memorized Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
                                    ReportLogic.SaveGridReport(grdsaleInvoiceReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.lblHeading + " to Standard Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


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
                                    ReportLogic.ExportToStandard(grdsaleInvoiceReport, reportName, reportType, reportGroup, report.settingkey);
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
            PrintableControlLink link = new PrintableControlLink((TableView)grdsaleInvoiceReport.View);
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

        private void GrdsaleInvoice_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (tableView.CompactPanelShowMode == CompactPanelShowMode.CompactMode)
            //{
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.CompactMode;
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.Always;
            //}
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Sale Invoices Report") != null)
            {

                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to share " + this.lblHeading + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
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
                DXMessageBox.Show("You don't have permission Share Sale Invoices Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
        }

        private void btnDepartmentFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > -1 && cmbxDepartmentTo.SelectedIndex > -1)
                {
                    switch (cmbxDepartmentFrom.SelectedIndex)
                    {
                        case 0:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 0:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 1:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = true;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 1:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {

                                case 1:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = true;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 2:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 2:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;

                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = false;
                                    break;
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = true;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 3:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 3:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;

                                    colDeptLevel5.Visible = false;
                                    break;
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = true;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 4:
                            switch (cmbxDepartmentTo.SelectedIndex)
                            {
                                case 4:
                                    colDeptLevel1.Visible = false;
                                    colDeptLevel2.Visible = false;
                                    colDeptLevel3.Visible = false;
                                    colDeptLevel4.Visible = false;
                                    colDeptLevel5.Visible = true;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    DXMessageBox.Show("Please select Department Levels to Apply filter!");
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void cmbxDepartmentTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxDepartmentFrom.SelectedIndex > cmbxDepartmentTo.SelectedIndex)
                {
                    cmbxDepartmentTo.SelectedIndex = -1;
                    DXMessageBox.Show("Please select greater department!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cmbxDepartmentFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbxDepartmentTo.SelectedIndex = -1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void mbtnDriectClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {


                SaleInvoiceRepo saleInvoicerepo = new SaleInvoiceRepo();
                ERP_BL.Databases.SaleInvoice saleInvoice = new ERP_BL.Databases.SaleInvoice();
                SaleInvoiceStatus oldStatus = new SaleInvoiceStatus();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null) ? true : false)
                {

                    if (grdsaleInvoiceReport.GetFocusedRow() != null)
                    {
                        UsersRepo usersRepo = new UsersRepo();

                        var row = grdsaleInvoiceReport.GetFocusedRow() as ERP_BL.Databases.SaleInvoice;
                        saleInvoice = new ERP_BL.Databases.SaleInvoice();
                        saleInvoice = saleInvoicerepo.get(row.Id);
                        var receiptsAmount = Math.Round(row.salesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount), 2);
                        var total = Math.Round(saleInvoice.totalInvoiceAmount, 2);
                        var result = total - receiptsAmount;
                        if (result != 0)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Invoice without receiving fully Collection") != null)
                            {
                                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to close sale Invoice without complete collection?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                                {
                                    if (row.saleInvoiceStatus != null)
                                    {
                                        oldStatus = row.saleInvoiceStatus;
                                    }
                                    SaleInvoicess.ucStatuschange.saleInvoiceid = (int)grdsaleInvoiceReport.GetFocusedRowCellValue(grdsaleInvoiceReport.Columns.GetColumnByFieldName("Id"));
                                    SaleInvoicess.frmSaleInvoiceStatusChange statusChange = new SaleInvoicess.frmSaleInvoiceStatusChange();
                                    var myWindow = Window.GetWindow(this);
                                    statusChange.Owner = myWindow;
                                    statusChange.ShowDialog();
                                    if (SaleInvoicess.ucStatuschange.saleInvoice.Id != 0)
                                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null) ? true : false)
                                        {
                                            SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = false;
                                            SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.Approved.ToString();

                                            frmInputBox inputBox = new frmInputBox();
                                            inputBox.ShowDialog();
                                            usersRepo.Add(TransactionInfo.Approved_Closing, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                                            //Inquiriess.ucStatuschange.saleInvoiceRepo.update(Inquiriess.ucStatuschange.saleInvoice);

                                        }
                                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                                        {
                                            SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                                            if (SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing == null)
                                            {
                                                SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                                            }
                                            frmInputBox inputBox = new frmInputBox();
                                            inputBox.ShowDialog();
                                            usersRepo.Add(TransactionInfo.Reviewed, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                                        }
                                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null)
                                        {
                                            SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                                            if (SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing == null)
                                            {
                                                SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                                            }
                                            //SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                                            frmInputBox inputBox = new frmInputBox();
                                            inputBox.ShowDialog();
                                            usersRepo.Add(TransactionInfo.Reviewed, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                                        }
                                        else
                                        {
                                            SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();

                                            SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;
                                        }
                                    //SaleInvoicess.ucStatuschange.saleInvoice.user_Id = MainWindow.currentUserid;
                                    SaleInvoicess.ucStatuschange.saleInvoice.ClosingDate = System.DateTime.Now;
                                    SaleInvoicess.ucStatuschange.saleInvoice.LastStatusChangeDate = System.DateTime.Now;
                                    SaleInvoicess.ucStatuschange.saleInvoiceRepo.updateForDirectClose(SaleInvoicess.ucStatuschange.saleInvoice);
                                    SaleOrderss.ucStatuschange.saleOrderid = (int)grdsaleInvoiceReport.GetFocusedRowCellValue(grdsaleInvoiceReport.Columns.GetColumnByFieldName("SaleOrderId"));
                                    SaleOrderss.frmSaleOrderStatusChange statChange = new SaleOrderss.frmSaleOrderStatusChange();
                                    SaleOrderss.ucStatuschange.inActiveStatuses = 0;                    //Show Only active SO Statusses

                                    statChange.Owner = myWindow;
                                    statChange.ShowDialog();
                                    //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                                    SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                                    SaleOrderss.ucStatuschange.UpdateSaleOrderStatusforInvoice();//.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                                    MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                    MessageBox.Show("SaleInvoice status changed to InActive (" + SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status + ")");

                                    //Adding auto Signature
                                    //if (row.saleInvoiceStatus.Status != SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status)
                                    //{
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res = MessageBox.Show("Sales Invocie has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Sale_Invoice);
                                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                        }
                                        else
                                        {
                                            winTagUsers win = new winTagUsers();
                                            win.ShowDialog();
                                        }
                                    }
                                    string oldStat = "";
                                    if (oldStatus != null)
                                    {
                                        oldStat = oldStatus.Status;
                                    }
                                    string newStat = SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status;
                                    string symbolCurr = "";
                                    if (row.currency != null)
                                    {
                                        symbolCurr = row.currency.Abbrivation.ToString();
                                    }
                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of Sale Invoice(Amount OC) having value: " + row.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(row.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                                    //Creating Comments
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }
                                }
                                else
                                    return;
                            }
                            else
                                DXMessageBox.Show("Sale Invoice is not fully received yet, or you need permission to Close Invoice without receiving fully Collection");
                        }
                        else
                        {
                            if (row.saleInvoiceStatus != null)
                            {
                                oldStatus = row.saleInvoiceStatus;
                            }
                            SaleInvoicess.ucStatuschange.saleInvoiceid = (int)grdsaleInvoiceReport.GetFocusedRowCellValue(grdsaleInvoiceReport.Columns.GetColumnByFieldName("Id"));
                            SaleInvoicess.frmSaleInvoiceStatusChange statusChange = new SaleInvoicess.frmSaleInvoiceStatusChange();
                            var myWindow = Window.GetWindow(this);
                            statusChange.Owner = myWindow;
                            statusChange.ShowDialog();
                            if (SaleInvoicess.ucStatuschange.saleInvoice.Id != 0)
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null) ? true : false)
                                {
                                    SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = false;
                                    SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Closing, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.saleInvoiceRepo.update(Inquiriess.ucStatuschange.saleInvoice);

                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                                {
                                    SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                                    if (SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing == null)
                                    {
                                        SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null)
                                {
                                    SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing == null)
                                    {
                                        SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                                    }
                                    //SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, SaleInvoicess.ucStatuschange.saleInvoice.Id, 5, frmInputBox.comment);
                                }
                                else
                                {
                                    SaleInvoicess.ucStatuschange.saleInvoice.stage = TransactionStage.AwaitingFirstReview.ToString();

                                    SaleInvoicess.ucStatuschange.saleInvoice.PendingForClosing = true;
                                }
                            //SaleInvoicess.ucStatuschange.saleInvoice.user_Id = MainWindow.currentUserid;
                            SaleInvoicess.ucStatuschange.saleInvoice.ClosingDate = System.DateTime.Now;
                            SaleInvoicess.ucStatuschange.saleInvoice.LastStatusChangeDate = System.DateTime.Now;
                            SaleInvoicess.ucStatuschange.saleInvoiceRepo.updateForDirectClose(SaleInvoicess.ucStatuschange.saleInvoice);
                            SaleOrderss.ucStatuschange.saleOrderid = (int)grdsaleInvoiceReport.GetFocusedRowCellValue(grdsaleInvoiceReport.Columns.GetColumnByFieldName("SaleOrderId"));
                            SaleOrderss.frmSaleOrderStatusChange statChange = new SaleOrderss.frmSaleOrderStatusChange();
                            SaleOrderss.ucStatuschange.inActiveStatuses = 0;                    //Show Only active SO Statusses

                            statChange.Owner = myWindow;
                            statChange.ShowDialog();
                            //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                            SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                            SaleOrderss.ucStatuschange.UpdateSaleOrderStatusforInvoice();//.saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                            MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                            MessageBox.Show("SaleInvoice status changed to InActive (" + SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status + ")");

                            //Adding auto Signature
                            //if (row.saleInvoiceStatus.Status != SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status)
                            //{
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Sales Invocie has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                {
                                    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Sale_Invoice);
                                    win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                                }
                                else
                                {
                                    winTagUsers win = new winTagUsers();
                                    win.ShowDialog();
                                }
                            }
                            string oldStat = "";
                            if (oldStatus != null)
                            {
                                oldStat = oldStatus.Status;
                            }
                            string newStat = SaleInvoicess.ucStatuschange.saleInvoice.saleInvoiceStatus.Status;
                            string symbolCurr = "";
                            if (row.currency != null)
                            {
                                symbolCurr = row.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Sale Invoice(Amount OC) having value: " + row.totalInvoiceAmount.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(row.Id, TransactionItemType.Sale_Invoice, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating Comments
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);
                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Sale Invoice #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Invoice, comment.Comment, user.id, "New Comment ", null);
                                }
                            }

                        }
                    }

                    //}

                }
                else
                {
                    MessageBox.Show("You are not Allowed to Close SaleInvoice Directly.");
                }
            }
            catch (Exception)
            {
            }
        }

        private void mbtnApprove_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                SaleInvoiceRepo saleInvoicerepo = new SaleInvoiceRepo();

                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null) ? true : false)
                {
                    if (grdsaleInvoiceReport.GetFocusedRow() != null)
                    {
                        ERP_BL.Databases.SaleInvoice saleInvoice = new ERP_BL.Databases.SaleInvoice();
                        //Inquiriess.ucStatuschange.saleInvoiceid = (int)grdsaleInvoice.GetFocusedRowCellValue(grdsaleInvoice.Columns.GetColumnByFieldName("Id"));
                        saleInvoice = grdsaleInvoiceReport.SelectedItem as ERP_BL.Databases.SaleInvoice;
                        //Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                        //statusChange.Owner = this;
                        //var myWindow = Window.GetWindow(this);
                        //statusChange.Owner = myWindow;
                        //statusChange.ShowDialog();
                        UsersRepo usersRepo = new UsersRepo();

                        if (saleInvoice.isApproved != true)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null) ? true : false)
                            {
                                saleInvoice.isApproved = true;
                                saleInvoice.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, saleInvoice.Id, 5, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                            {
                                saleInvoice.stage = TransactionStage.AwaitingApproval.ToString();
                                if (saleInvoice.isApproved == null)
                                {
                                    saleInvoice.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, saleInvoice.Id, 5, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null)
                            {
                                saleInvoice.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (saleInvoice.isApproved == null)
                                {
                                    saleInvoice.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, saleInvoice.Id, 5, frmInputBox.comment);
                            }
                            else
                            {
                                saleInvoice.isApproved = false;
                            }




                        saleInvoicerepo.updateForDirectClose(saleInvoice);
                        MessageBox.Show("SaleInvoice is Approved (" + saleInvoice.Id + ")");
                        SystemLog.LogInfo(this.GetType(), "SaleInvoice is Approved (" + saleInvoice.Id + ")");
                    }
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Approve SaleInvoice Directly");
                    SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve SaleInvoice Directly user id=(" + MainWindow.currentUserid + ")");

                }
            }
            catch (Exception)
            {
            }
        }

        private void cmbxAsOff_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            grdsaleInvoiceReport.RefreshData();
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            loadMarketExchangeRates();
            SaleInvoiceRepo repo = new SaleInvoiceRepo();
            GridReportRepo repoReport = new GridReportRepo();
            var group = repoReport.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
            datAsOff.EditValue = DateTime.Now;
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here  
                            mbtnShareReport.IsVisible = false;
                            mbtnExportToStandardReport1.IsVisible = false;
                            //Title = "SaleInvoices" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "SaleInvoices" + "/" + group.groupName + "/" + reportTitle;
                            grdsaleInvoiceReport.ItemsSource = repo.getAll(SYSTEM_STATIC.currentUser.id);

                            var value = report.settingValue.Where(x => x.Equals("Format"));

                            grdsaleInvoiceReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            btnFav.IsEnabled = false;
                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here   
                            mbtnExportToMemorizedReport1.IsVisible = false;
                            //Title = "SaleInvoices" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "SaleInvoices" + "/" + group.groupName + "/" + reportTitle;
                            grdsaleInvoiceReport.ItemsSource = repo.getAllActive(SYSTEM_STATIC.currentUser.id);
                            grdsaleInvoiceReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
              
                        }

                        break;
                    }
            }
        }
    }
}
