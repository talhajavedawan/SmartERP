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
using ZAS_ERP.Reportss;
using ERP_BL.ExchangeRates;

namespace ZAS_ERP.Procurementss.SaleInvoicess
{
    /// <summary>
    /// Interaction logic for ucSaleInvoiceGrid.xaml
    /// </summary>
    public partial class ucSaleInvoiceGrid : UserControl
    {
        public static int statusId;
        public static int AllActive;
        public int VoidCount { get; set; }
        ProcurementRepo procurementRepo = new ProcurementRepo();
        SaleInvoiceStatus oldStatus = new SaleInvoiceStatus();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        static List<ExchangeRateGroup> exchangeRateGroups = new List<ExchangeRateGroup>();
        ExchangeRate exchangeRate = null;
        public ucSaleInvoiceGrid()
        {

            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleInvoice List") != null)
                {

                    ApprovalCount = saleInvoicerepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);
                }
                else
                {

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleInvoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                    {
                        ApprovalCount = saleInvoicerepo.getAllPendingForApprovalCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        ApprovalCount = saleInvoicerepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);


                    }

                }
            }

            else
            {
                ApprovalCount = saleInvoicerepo.getAllPendingForAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleInvoice List") != null)
                {
                    ClosingCount = saleInvoicerepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                    {
                        ClosingCount = saleInvoicerepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = saleInvoicerepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                    }

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Sales Invoice Register") != null)
                {
                    mbtnRegister.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                    {
                        ApproveunapprovedCount = saleInvoicerepo.getSaleInvoiceRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApproveunapprovedCount = saleInvoicerepo.getInvoiceRegisterCountOWn();
                    }
                }
                else
                {
                    mbtnRegister.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                ClosingCount = saleInvoicerepo.getAllPendingForClosingAdministratorCount();
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleInvoices") != null)
            {
                mbtnVoid.Visibility = Visibility.Visible;

                if (MainWindow.currentUserid != 0)
                {
                    VoidCount = saleInvoicerepo.getVoidRegisterCount(MainWindow.currentUserid);

                }
                else
                {
                    VoidCount = saleInvoicerepo.getVoidRegisterAdministratorCount();

                }


            }
            else
            {
                mbtnVoid.Visibility = Visibility.Collapsed;
            }
            //this.DataContext = this;
        }
        SaleInvoiceRepo saleInvoicerepo = new SaleInvoiceRepo();
        SaleInvoice saleInvoice = new SaleInvoice();
        public IList<ERP_BL.Databases.SaleInvoice> saleInvoices { get; set; }
        //public ObservableCollection<SaleInvoice> orders { get; set; }
        public int ApprovalCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }

        /// <summary>
        /// runs when window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucsaleInvoiceGrid_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            //SystemLogic.SetUserSettingOfCurrentWindow(grdsaleInvoice);
            loadSaleInvoicegrid();
            loadMarketExchangeRates();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void loadMarketExchangeRates()
        {
            //CurrencyRepo currencyRepo = new CurrencyRepo();
            //marketexchangeRates = currencyRepo.getAllMarketExchangeRatesForToday();
            exchangeRateGroups = exchangeRateGroupRepo.GetAllMER();
        }

        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            //if (e.IsGetData)
            {
                var invoice = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;

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
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
                    if (row.ExpectedPayment != null)
                    {


                        DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime date = row.ExpectedPayment.Value;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = currDate.Subtract(targetDate);
                        e.Value = timeSpan.Days.ToString();
                    }
                     
                } 
                if (e.Column.FieldName == "EdDaysLeft")
                {
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
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
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;

                    if (row.saleInvoiceStatus.isActive != false)
                    {
                        if (row.LastStatusChangeDate != null)
                        {
                            DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                            DateTime date = row.LastStatusChangeDate.Value;
                            DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            var timeSpan = targetDate.Subtract(currDate);
                            e.Value = timeSpan.Days.ToString();
                        }
                    }
                    else
                    {
                        e.Value = "Transaction Closed";
                    }
                     
                }
                if (e.Column.FieldName == "statusClassAgeing")
                {
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
                    if (row.saleInvoiceStatus.isActive != false)
                    {
                        if (row.LastStatusClassChangeDate != null)
                        {
                            DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                            DateTime date = row.LastStatusClassChangeDate.Value;
                            DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                            var timeSpan = targetDate.Subtract(currDate);
                            e.Value = timeSpan.Days.ToString();
                        }
                    }
                    else
                    {
                        e.Value = "Transaction Closed";
                    }

                }
                if (e.Column.FieldName == "PDLeft")
                {
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.SaleInvoice;
                    if (row.paymentOnDate != null)
                    {
                        DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        DateTime date = row.paymentOnDate.Value;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = targetDate.Subtract(currDate);
                        e.Value = timeSpan.Days.ToString();
                    }

                }
                if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                {
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
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
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
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
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
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
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
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
                    var row = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
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
                if (e.Column.FieldName == "Vendorss" && e.IsGetData)
                {
                    var _SI = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
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
                    var reciepts = invoice.salesReceipts;
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
                    if(invoice != null)
                    {
                        if (invoice.saleInvoicetype == InquiryType.DistributionBiz)
                        {
                            if(saleInvoice.BookerStatementItems != null)
                            {
                                var invoiceAmount = Math.Round(saleInvoice.BookerStatementItems.Sum(x => x.siNetAmount), 2);
                                var collected = invoice.salesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                                e.Value = Math.Round((invoiceAmount - collected), 2);
                            }
                            
                        }
                        else
                        {
                            var reciepts = invoice.salesReceipts;
                            if (reciepts != null && e.GetListSourceFieldValue("totalInvoiceAmount") != null)
                            {
                                var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalInvoiceAmount"));
                                var result = Convert.ToDecimal(reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount));
                                e.Value = total - result;
                            }
                        }
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
            }
        }
        /// <summary>
        /// Loads data into saleInvoice Grid and create columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadSaleInvoicegrid()

        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Sale Invoices(Open)" || lblHeading.Text == "Sale Invoices(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        saleInvoices = saleInvoicerepo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Invoices") != null)
                    {
                        saleInvoices = saleInvoicerepo.getAllFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        saleInvoices = saleInvoicerepo.getAllActiveFirst(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    saleInvoices = saleInvoicerepo.getAllActiveFirst(MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    saleInvoices = saleInvoicerepo.getAllInActiveFirst(MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    lblHeading.Text = "Pending For Approvals (Sale Invoices)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleInvoice List") != null)
                        {

                            saleInvoices = saleInvoicerepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                            {
                                saleInvoices = saleInvoicerepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                            }
                            else
                            {
                                saleInvoices = saleInvoicerepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);
                            }
                        }
                    else
                        saleInvoices = saleInvoicerepo.getAllPendingForAdministratorFirst();
                    

                }

                else if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Sale Invoices";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleInvoice List") != null)
                        {
                            saleInvoices = saleInvoicerepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                            {
                                saleInvoices = saleInvoicerepo.getAllPendingForClosingFirst(MainWindow.currentUserid);
                            }
                            else
                            {
                                saleInvoices = saleInvoicerepo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);

                            }
                        }
                    else
                        saleInvoices = saleInvoicerepo.getAllPendingForClosingAdministratorFirst();
                    grdsaleInvoice.ItemsSource = saleInvoices;
                    grdsaleInvoice.Columns["CreationDate"].VisibleIndex = 0;
                }
            }
            else
            {
                saleInvoices = saleInvoicerepo.getAllPobyStatusIdFirst(MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdsaleInvoice);
            grdsaleInvoice.ItemsSource = saleInvoices;
            RemoveSourceObjects();
        }
        private void grdsaleInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSaleInvoice();
        }

        private void EditSaleInvoice()
        {
            if(grdsaleInvoice.SelectedItem!=null)
            {
                var SI = grdsaleInvoice.SelectedItem as SaleInvoice;
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, (int)(SI).Id);

                if (SI.saleInvoicetype == InquiryType.DistributionBiz_CustomerCredit)
                {
                    procurmentPanel.btnCreateReciept.Visibility = Visibility.Collapsed;
                }

                procurmentPanel.Show();
            }
        }

        private void btnNewSaleInvoice_Click(object sender, RoutedEventArgs e)
        {


            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, 0);
            procurmentPanel.isCommission = true;
            procurmentPanel.Show();
        }

        private void btnEditSaleInvoice_Click(object sender, RoutedEventArgs e)
        {
            EditSaleInvoice();
        }

        private void UcsaleInvoicegrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //AllActive = 0;
            //SystemLogic.SaveUserSettingForCurrentWindow(grdsaleInvoice);
            //statusId = 0;
        }

        private void RbtnCopy_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdsaleInvoice.View.ShowPrintPreview(this);
            //ShowDesigner(tableView);
        }
        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            ShowDesigner(tableView);
        }
        // Initializes and runs a Report Designer. 
        public static void ShowDesigner(IGridViewFactory<ColumnWrapper, RowBaseWrapper> factory)
        {
            var report = new XtraReport();
            ReportGenerationExtensions<ColumnWrapper, RowBaseWrapper>.Generate(report, factory);
            Reportss.frmReportPanel frmReport = new Reportss.frmReportPanel(report);
            frmReport.Show();
            //var reportDesigner = new ReportDesigner();
            //reportDesigner.Loaded += (s, e) => {
            //    reportDesigner.OpenDocument(report);
            //};
            //reportDesigner.ShowWindow(factory as FrameworkElement);
        }

        private void MbtnReportCreate_Click(object sender, RoutedEventArgs e)
        {
            ShowDesigner(tableView);
        }
        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Sale Invoices Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Invoices") != null)
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblHeading.Text);
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdsaleInvoice, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Sale Invoices Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //this.RemoveFromVisualTree();
            SaleInvoicess.ucSaleInvoiceGrid ucSaleInvoiceGrid = new ucSaleInvoiceGrid();
            this.Content = ucSaleInvoiceGrid;
            //InitializeComponent();
            //loadSaleInvoicegrid();
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdsaleInvoice.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdsaleInvoice.View.ShowPrintPreview(this);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            saleInvoicerepo = new SaleInvoiceRepo();
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null) ? true : false)
            {

                if (grdsaleInvoice.GetFocusedRow() != null)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    var row = grdsaleInvoice.GetFocusedRow() as SaleInvoice;
                    saleInvoice = new SaleInvoice();
                    saleInvoice = saleInvoicerepo.get(row.Id);
                    var receiptsAmount = Math.Round( row.salesReceipts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount), 2);
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
                                SaleInvoicess.ucStatuschange.saleInvoiceid = (int)grdsaleInvoice.GetFocusedRowCellValue(grdsaleInvoice.Columns.GetColumnByFieldName("Id"));
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
                                SaleOrderss.ucStatuschange.saleOrderid = (int)grdsaleInvoice.GetFocusedRowCellValue(grdsaleInvoice.Columns.GetColumnByFieldName("SaleOrderId"));
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
                        SaleInvoicess.ucStatuschange.saleInvoiceid = (int)grdsaleInvoice.GetFocusedRowCellValue(grdsaleInvoice.Columns.GetColumnByFieldName("Id"));
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
                        SaleOrderss.ucStatuschange.saleOrderid = (int)grdsaleInvoice.GetFocusedRowCellValue(grdsaleInvoice.Columns.GetColumnByFieldName("SaleOrderId"));
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
        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            AllActive = 3;
            statusId = 0;
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Pending Sale Invoices";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleInvoice List") != null)
                {

                    saleInvoices = saleInvoicerepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);
                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                    {
                        saleInvoices = saleInvoicerepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                    }

                    else
                    {
                        saleInvoices = saleInvoicerepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);

                    }
                }
            else
                saleInvoices = saleInvoicerepo.getAllPendingForAdministrator();

            grdsaleInvoice.ItemsSource = saleInvoices;
            //grdsaleInvoice.ClearGrouping();
            //grdsaleInvoice.FilterString = "";
            //grdsaleInvoice.GroupBy("stage");
            //grdsaleInvoice.Columns["stage"].GroupIndex = 0;
            //grdsaleInvoice.Columns["CreationDate"].VisibleIndex = 1;
            //grdsaleInvoice.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);
            //grdsaleInvoice.View = new CardView();
            grdsaleInvoice.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

        }
        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            statusId = 0;
            AllActive = 4;
            lblHeading.Text = "(Pending for Closing) Sale Invoices";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleInvoice List") != null)
                {
                    saleInvoices = saleInvoicerepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                    {
                        saleInvoices = saleInvoicerepo.getAllPendingForClosingFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        saleInvoices = saleInvoicerepo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);

                    }
                }
            else
                saleInvoices = saleInvoicerepo.getAllPendingForClosingAdministratorFirst();
            grdsaleInvoice.ItemsSource = saleInvoices;

            grdsaleInvoice.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null) ? true : false)
            {
                if (grdsaleInvoice.GetFocusedRow() != null)
                {
                    SaleInvoice saleInvoice = new SaleInvoice();
                    //Inquiriess.ucStatuschange.saleInvoiceid = (int)grdsaleInvoice.GetFocusedRowCellValue(grdsaleInvoice.Columns.GetColumnByFieldName("Id"));
                    saleInvoice = grdsaleInvoice.SelectedItem as SaleInvoice;
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
        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdsaleInvoice);
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Pending Sale Invoices";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                {
                    saleInvoices = saleInvoicerepo.getAllandUnapporved(MainWindow.currentUserid);
                }

                else
                {
                    saleInvoices = saleInvoicerepo.getAllandUnapporvedOwn(MainWindow.currentUserid);

                }

            else
                saleInvoices = saleInvoicerepo.getAllandUnapporvedAdministrator();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Void Sale Invoices";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void SaleInvoices") != null)
                {
                    saleInvoices = saleInvoicerepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    saleInvoices = saleInvoicerepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                saleInvoices = saleInvoicerepo.getVoidRegisterAdministrator();
            grdsaleInvoice.ItemsSource = saleInvoices;
            grdsaleInvoice.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void mbtnPartially_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            statusId = 0;
            AllActive = 8;
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Partially Invoiced";
            if (MainWindow.currentUserid != 0)
                saleInvoices = saleInvoicerepo.getPartiallyInvoicedFirst(MainWindow.currentUserid);
            else
                saleInvoices = saleInvoicerepo.getVoidRegisterAdministratorFirst();
            grdsaleInvoice.ItemsSource = saleInvoices;
            grdsaleInvoice.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void mbtnFullyInvoiced_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            statusId = 0;
            AllActive = 7;
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Fully Invoiced";
            if (MainWindow.currentUserid != 0)
                saleInvoices = saleInvoicerepo.getFullyInvoicedFirst(MainWindow.currentUserid);
            else
                saleInvoices = saleInvoicerepo.getVoidRegisterAdministrator();
            grdsaleInvoice.ItemsSource = saleInvoices;
            grdsaleInvoice.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void MbtnRegister_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            AllActive = 6;
            statusId = 0;
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Sales Invoice Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleInvoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)                {
                    saleInvoices = saleInvoicerepo.getSaleInvoiceRegisterFirst(MainWindow.currentUserid);
                }

                else
                {
                    saleInvoices = saleInvoicerepo.getSaleInvoiceRegisterFirst(MainWindow.currentUserid);
                }

            else
                saleInvoices = saleInvoicerepo.getInvoiceRegisterAdministrator();
            grdsaleInvoice.ItemsSource = saleInvoices;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void BtnDateFilter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            if (dateFrom.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateFrom.Focus();

                return;
            }
            else
                if (dateTo.EditValue == null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });
                DXMessageBox.Show("Please select valid date range", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                dateTo.Focus();

                return;

            }
            else
            {

                LoadOrdersByDate();
                RemoveSourceObjects();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                });

            }
        }
        private void RemoveSourceObjects()
        {
            
        }
        private void LoadOrdersByDate()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Sale Invoices(Open)" || lblHeading.Text == "Sale Invoices(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        saleInvoices = saleInvoicerepo.getAll();
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Invoices") != null)
                    {
                        saleInvoices = saleInvoicerepo.getAllByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                    else
                    {
                        saleInvoices = saleInvoicerepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    saleInvoices = saleInvoicerepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    saleInvoices = saleInvoicerepo.getAllInActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    lblHeading.Text = "Pending For Approvals (Sale Invoices)";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) SaleInvoice List") != null)
                        {

                            saleInvoices = saleInvoicerepo.getAllPendingForApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                            {
                                saleInvoices = saleInvoicerepo.getAllPendingForApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                saleInvoices = saleInvoicerepo.getAllPendingForApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                        }
                    else
                        saleInvoices = saleInvoicerepo.getAllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);


                }

                else if (AllActive == 4)
                {
                    lblHeading.Text = "(Pending for Closing) Sale Invoices";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) SaleInvoice List") != null)
                        {
                            saleInvoices = saleInvoicerepo.getAllPendingForClosingDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Invoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                            {
                                saleInvoices = saleInvoicerepo.getAllPendingForClosingByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                            }
                            else
                            {
                                saleInvoices = saleInvoicerepo.getAllPendingForClosingOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                            }
                        }
                    else
                        saleInvoices = saleInvoicerepo.getAllPendingForClosingAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                    grdsaleInvoice.ItemsSource = saleInvoices;
                    grdsaleInvoice.Columns["CreationDate"].VisibleIndex = 0;
                }
                else
                    if (AllActive == 6)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Sales Invoice Register";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add SaleInvoice without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleInvoice") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleInvoice") != null)
                        {
                            saleInvoices = saleInvoicerepo.getSaleInvoiceRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }

                        else
                        {
                            saleInvoices = saleInvoicerepo.getSaleInvoiceRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                        }

                    else
                        saleInvoices = saleInvoicerepo.getInvoiceRegisterAdministrator();
                }
                else
                    if (AllActive == 7)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Fully Invoiced";
                    if (MainWindow.currentUserid != 0)
                        saleInvoices = saleInvoicerepo.getFullyInvoicedByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    else
                        saleInvoices = saleInvoicerepo.getVoidRegisterAdministrator();

                }
                else
                    if (AllActive == 8)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Partially Invoiced";
                    if (MainWindow.currentUserid != 0)
                        saleInvoices = saleInvoicerepo.getPartiallyInvoicedByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    else
                        saleInvoices = saleInvoicerepo.getVoidRegisterAdministrator();
                }
            }
            else
            {
                saleInvoices = saleInvoicerepo.getAllPobyStatusIdByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdsaleInvoice);
            grdsaleInvoice.ItemsSource = saleInvoices;
            RemoveSourceObjects();
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

    }
}
