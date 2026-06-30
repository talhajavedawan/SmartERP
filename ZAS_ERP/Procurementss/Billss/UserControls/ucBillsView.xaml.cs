using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.Billss.UserControls
{
    /// <summary>
    /// Interaction logic for ucBillsView.xaml
    /// </summary>
    public partial class ucBillsView : DXWindow
    {
        GridReport report = new GridReport();
        string reportTitle;
        public MainWindow myParent = null;
        BillRepo billrepo = new BillRepo();
        public ucBillsView()
        {
            InitializeComponent();
        }
        public ucBillsView(GridReport reportToEdit,string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
            
        }
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
                    if (e.GetListSourceFieldValue("billDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("billDate"));

                        Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                        e.Value = SoDateAgeingDays;
                        // string s = "Test: FieldTwo";
                    }
                }

                if (e.Column.FieldName == "IsAdjusted")
                {
                    var _bill = grdbill.GetRowByListIndex(e.ListSourceRowIndex) as Bill;
                    if (_bill != null && _bill.tax != null)
                    {
                        if (_bill.tax.isAdjusted == true)
                            e.Value = true;
                        else
                            e.Value = false;
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
                if (e.Column.FieldName == "BudgetMarginOC")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("margin"));
                    }
                }
                if (e.Column.FieldName == "BudgetedMarginME")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("ExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("ExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("BudgetedMargininBase"));
                    }
                }
                if (e.Column.FieldName == "BudgetedMarginSE")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("marginExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesBudgetedMargin"));
                    }
                }
                if (e.Column.FieldName == "BudgetedMarginPercentAge")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                        Decimal result = 0;
                        if (total > 0)
                            result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) / total) * 100;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("BudgetedMarginPercent"));
                    }
                }
                if (e.Column.FieldName == "ActualMarginOC")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMargin"));
                    }
                }
                if (e.Column.FieldName == "ActualMarginME")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("ExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("ExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMargininBase"));
                    }
                }
                if (e.Column.FieldName == "ActualMarginSE")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("marginExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesActualMargin"));
                    }
                }
                if (e.Column.FieldName == "ActualMarginPercentAge")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                        Decimal result = 0;
                        if (total > 0)
                            result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) / total) * 100;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMarginPercent"));
                    }
                }
                if (e.Column.FieldName == "RevisedMarginOC")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMargin"));
                    }
                }
                if (e.Column.FieldName == "RevisedMarginME")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("ExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("ExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMargininBase"));
                    }
                }
                if (e.Column.FieldName == "RevisedMarginSE")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("marginExchangeRate"));
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                        var result = (Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) * exchangerate;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesRevisedMargin"));
                    }
                }
                if (e.Column.FieldName == "RevisedMarginPercentAge")
                {
                    if (e.GetListSourceFieldValue("SaleOrder") != null && e.GetListSourceFieldValue("CostSheet") != null)
                    {
                        var SO = e.GetListSourceFieldValue("SaleOrder") as SaleOrder;
                        var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                        var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                        Decimal result = 0;
                        if (total > 0)
                            result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) / total) * 100;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMarginPercent"));
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


                if (e.Column.FieldName == "PaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        billrepo = new BillRepo();
                        var _bill = billrepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        var paidAmount = Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
                        //decimal amountWithTax = 0;
                        //if (_bill.Payments.Count > 0)
                        //    if (_bill.billWithTax != null)
                        //    {
                        //        amountWithTax = Convert.ToDecimal(_bill.billWithTax);
                        //    }
                        //    else
                        //    {
                        //        amountWithTax = Convert.ToDecimal(_bill.totalCFRValue);
                        //    }
                        e.Value = paidAmount;
                    }
                }

                if (e.Column.FieldName == "UnpaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        billrepo = new BillRepo();
                        var _bill = billrepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        var paidAmount = Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
                        decimal amountWithTax = 0;
                        if (_bill.Payments.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            if (_bill.billWithTax != null)
                            {
                                //paidAmount =  Convert.ToDecimal(_bill.billWithTax);
                                amountWithTax = Math.Round(Convert.ToDecimal(_bill.billWithTax.Value) - paidAmount, 2);
                            }
                            else
                            {
                                amountWithTax = Convert.ToDecimal(_bill.totalCFRValue) - paidAmount;
                            }

                        }
                        else
                        {
                            if (_bill.billWithTax != null)
                            {
                                amountWithTax = Convert.ToDecimal(_bill.billWithTax.Value);
                            }
                            else
                            {
                                amountWithTax = Convert.ToDecimal(_bill.totalCFRValue);
                            }
                        }
                        e.Value = amountWithTax;
                    }
                }

                if (e.Column.FieldName == "Stage" && e.IsGetData)
                {
                    var bill = grdbill.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.Bill;
                    if (bill.isVoid == true)
                    {
                        e.Value = "Void";
                    }
                    //else if (saleinvoice.isreapproved == false)
                    //{
                    //    e.value = "under approval";
                    //}
                    else if (bill.isApproved == true && bill.stage == "Closed")
                    {
                        e.Value = "Closed";
                    }
                    else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
                    {
                        e.Value = "Closed";
                    }
                    else if (bill.isApproved == true && bill.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                    else if (bill.isApproved == true)
                    {
                        e.Value = "Approved";
                    }
                    else if (bill.isApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (bill.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                }
            }
        }
        //private void MbtnSaveAsNew_Click(object sender, RoutedEventArgs e)
        //{
        //    switch (report.gridReportType)
        //    {
        //        case GridReportType.StandardReport:
        //            {
        //                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Save as new Standard Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //                        setReportName.ShowDialog();
        //                        var saveAsNewReport = setReportName.report;
        //                        if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.gridReportType != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
        //                        {
        //                            var reportGroup = saveAsNewReport.gridReportGroup;
        //                            var reportType = saveAsNewReport.gridReportType;
        //                            var reportName = saveAsNewReport.reportName;
        //                            ReportLogic.SaveGridReport(grdbill, reportName, reportType, reportGroup, report.settingkey);
        //                            DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        }
        //                    }
        //                    else
        //                        return;
        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to save as new Standard Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //            }
        //            break;
        //        case GridReportType.MemorizedReport:
        //            {

        //                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Save as new Memorized Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //                        setReportName.ShowDialog();
        //                        var saveAsNewReport = setReportName.report;
        //                        if (saveAsNewReport != null && saveAsNewReport.gridReportGroup != null && saveAsNewReport.gridReportType != null && saveAsNewReport.reportName != null && saveAsNewReport.userId != null)
        //                        {
        //                            var reportGroup = saveAsNewReport.gridReportGroup;
        //                            var reportType = saveAsNewReport.gridReportType;
        //                            var reportName = saveAsNewReport.reportName;
        //                            ReportLogic.SaveGridReport(grdbill, reportName, reportType, reportGroup, lblHeading.Text);
        //                            DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        }
        //                    }
        //                    else
        //                        return;
        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to save as new Memorized Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //            }
        //            break;
        //    }
        //}

        //private void MbtnRenameReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var inputfromUser = DXMessageBox.Show("Do you want to rename " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        //    switch (report.gridReportType)
        //    {
        //        case GridReportType.StandardReport:
        //            {
        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Rename Standard Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        //var report = repo.GetReportByName(this.Title);
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
        //                        Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //                        setReportName.report = report;
        //                        setReportName.editableGroup = group;
        //                        //setting input Fields
        //                        setReportName.txtName.Text = report.reportName;
        //                        //setReportName.LoadReportTypes();
        //                        setReportName.GetEnum();
        //                        setReportName.reportTypes.EditValue = report.gridReportType;
        //                        setReportName.lookupGroup.EditValue = group.groupName;
        //                        setReportName.LoadGroups();
        //                        setReportName.ShowDialog();
        //                        var updatedReport = setReportName.report;
        //                        if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
        //                        {
        //                            if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
        //                            {
        //                                ReportLogic.RenameGridReport(grdbill, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey);
        //                                DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        //                                this.Close();
        //                            }
        //                            else
        //                                DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            return;
        //                        }

        //                    }
        //                    else
        //                        return;
        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //                break;
        //            }
        //        case GridReportType.MemorizedReport:
        //            {
        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Rename Memorized Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        //var report = repo.GetReportByName(this.Title);
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
        //                        Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //                        setReportName.report = report;
        //                        setReportName.editableGroup = group;
        //                        //setting input Fields
        //                        setReportName.txtName.Text = report.reportName;
        //                        //setReportName.LoadReportTypes();
        //                        setReportName.GetEnum();
        //                        setReportName.reportTypes.EditValue = report.gridReportType;
        //                        setReportName.lookupGroup.EditValue = group.groupName;
        //                        setReportName.LoadGroups();
        //                        setReportName.ShowDialog();
        //                        var updatedReport = setReportName.report;
        //                        if (updatedReport != null && updatedReport.gridReportGroup != null && updatedReport.gridReportType != null && updatedReport.reportName != null && updatedReport.userId != null)
        //                        {
        //                            if (updatedReport.group_Id == report.gridReportGroup.Id && updatedReport.gridReportType == report.gridReportType)
        //                            {
        //                                ReportLogic.RenameGridReport(grdbill, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey);
        //                                DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        //                                this.Close();
        //                            }
        //                            else
        //                                DXMessageBox.Show("You cannot change (Grid Report Type) or (Grid Report Group)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            return;
        //                        }

        //                    }
        //                    else
        //                        return;
        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Rename report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //                break;
        //            }

        //    }

        //}

        //private void MbtnUpdateReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.Title + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        //    if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Update Report") != null)
        //    {
        //        if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //        {
        //            var repo = new GridReportRepo();
        //            var report = repo.GetReportByName(this.Title);
        //            var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
        //            string str = report.reportName;
        //            GridReportType reportType = report.gridReportType;
        //            GridReportGroup groupDetails = group;
        //            if (str != "")
        //            {
        //                ReportLogic.UpdateGridReport(grdbill, str, reportType, groupDetails, report.Id, report.settingkey);

        //                DXMessageBox.Show(" ( " + str + " ) is Updated Successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            return;
        //        }
        //        return;
        //    }
        //    else
        //    {
        //        DXMessageBox.Show("You don't have permission to Update report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}

        //private void MbtnDeleteReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var inputfromUser = DXMessageBox.Show("Do you want to delete " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

        //    switch (report.gridReportType)
        //    {
        //        case GridReportType.StandardReport:
        //            {
        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Delete Standard Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        /*GridReport report = repo.GetReportByName(this.Title)*/
        //                        ;
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

        //                        ReportLogic.DeleteReport(grdbill, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
        //                        DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        this.Close();
        //                    }
        //                    else
        //                        return;

        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Delete Standard report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }

        //                break;
        //            }

        //        case GridReportType.MemorizedReport:
        //            {

        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Delete Memorized Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        /*GridReport report = repo.GetReportByName(this.Title)*/
        //                        ;
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

        //                        ReportLogic.DeleteReport(grdbill, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
        //                        DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        this.Close();
        //                    }
        //                    else
        //                        return;

        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Delete Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }

        //                break;
        //            }

        //    }




        //}

        //private void MbtnRefreshReport_Click(object sender, RoutedEventArgs e)
        //{
        //    loadingGif.Visibility = Visibility.Visible;
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.DoWork += OnDoWork;
        //    worker.RunWorkerCompleted += OnRunWorkerCompleted;
        //    worker.RunWorkerAsync(); 
        //}
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            var repo = new GridReportRepo();
            string path = this.Title;
            int pos = path.LastIndexOf("/") + 1;
            var reportTitlePath = path.Substring(pos, path.Length - pos);

            GridReport report = repo.GetReportByName(reportTitlePath);
            BillRepo billRepo = new BillRepo();
            if (report.settingkey == "Bill Register")
            { grdbill.ItemsSource = billRepo.getPurchaseRegister(SYSTEM_STATIC.currentUser.id); }
            else
            { grdbill.ItemsSource = billRepo.getAll(SYSTEM_STATIC.currentUser.id); }
            loadingGif.Visibility = Visibility.Hidden;
         
        }

        private void Grdinquiry_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditBill();
        }

        private void EditBill()
        {
            if (grdbill.GetFocusedRowCellValue(grdbill.Columns.GetColumnByFieldName("Id")) != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Bill, (int)grdbill.GetFocusedRowCellValue(grdbill.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();
            }
        }

        //private void MbtnExportToMemorizedReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Memorized Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        //    switch (report.gridReportType)
        //    {
        //        case GridReportType.StandardReport:
        //            {
        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Export to Memorized Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        //var report = repo.GetReportByName(this.Title);
        //                        var type = report.gridReportType;
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

        //                        Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //                        setReportName.ShowDialog();
        //                        var exportToMemorizedReport = setReportName.report;
        //                        if (exportToMemorizedReport != null && exportToMemorizedReport.gridReportGroup != null && exportToMemorizedReport.gridReportType != null && exportToMemorizedReport.reportName != null && report.userId != null)
        //                        {
        //                            var reportGroup = exportToMemorizedReport.gridReportGroup;
        //                            var reportType = exportToMemorizedReport.gridReportType;
        //                            var reportName = exportToMemorizedReport.reportName;
        //                            ReportLogic.SaveGridReport(grdbill, reportName, reportType, reportGroup, report.settingkey);
        //                            DXMessageBox.Show("( " + reportName + " ) is exported to Memorized reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        }

        //                    }
        //                    else
        //                        return;

        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //                break;
        //            }

        //    }
        //}

        //private void MbtnExportToStandardReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Standard Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


        //    switch (report.gridReportType)
        //    {
        //        case GridReportType.MemorizedReport:
        //            {
        //                if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Export to Standard Report") != null)
        //                {
        //                    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //                    {
        //                        var repo = new GridReportRepo();
        //                        //var report = repo.GetReportByName(this.Title);
        //                        var type = report.gridReportType;
        //                        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

        //                        Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //                        setReportName.ShowDialog();
        //                        var exportToStandardReport = setReportName.report;
        //                        if (exportToStandardReport != null && exportToStandardReport.gridReportGroup != null && exportToStandardReport.gridReportType != null && exportToStandardReport.reportName != null && report.userId != null)
        //                        {
        //                            var reportGroup = exportToStandardReport.gridReportGroup;
        //                            var reportType = exportToStandardReport.gridReportType;
        //                            var reportName = exportToStandardReport.reportName;
        //                            ReportLogic.ExportToStandard(grdbill, reportName, reportType, reportGroup, report.settingkey);
        //                            DXMessageBox.Show("( " + reportName + " ) is exported to Standard reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
        //                        }

        //                    }
        //                    else
        //                        return;

        //                }
        //                else
        //                {
        //                    DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //                break;
        //            }

        //    }



        //    //if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Access to Export to Standard Report") != null)
        //    //{
        //    //    if (inputfromUser == System.Windows.MessageBoxResult.Yes)
        //    //    {
        //    //        var repo = new GridReportRepo();
        //    //        //var report = repo.GetReportByName(this.Title);
        //    //        var type = report.gridReportType;
        //    //        var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
        //    //        if (report.gridReportType == GridReportType.MemorizedReport)
        //    //        {
        //    //            Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(report.settingkey);
        //    //            setReportName.ShowDialog();
        //    //            var exportMemorizedReport = setReportName.report;
        //    //            if (exportMemorizedReport != null && exportMemorizedReport.gridReportGroup != null && exportMemorizedReport.gridReportType != null && exportMemorizedReport.reportName != null && report.userId != null)
        //    //            {
        //    //                var reportGroup = exportMemorizedReport.gridReportGroup;
        //    //                var reportType = exportMemorizedReport.gridReportType;
        //    //                var reportName = exportMemorizedReport.reportName;
        //    //                ReportLogic.ExportToStandard(grdinquiry, reportName, reportType, reportGroup, report.settingkey);
        //    //                DXMessageBox.Show("( " + reportName + " ) is exported to Standard reports Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            DXMessageBox.Show("Select Report type "+" Standard"+"!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //            return;
        //    //        }
        //    //        return;
        //    //    }
        //    //    else
        //    //        return;
        //    //}
        //    //else
        //    //{
        //    //    DXMessageBox.Show("You don't have permission to Export to Memorized report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //    return;
        //    //}
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Loader.DeferedVisibility = true;
            BillRepo billRepo = new BillRepo();
            GridReportRepo repo = new GridReportRepo();
            
            var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here          
                            mbtnExportToStandardReport1.IsVisible = false;
                            mbtnShareReport.IsVisible = false;
                            grdbill.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Bills" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Bills" + "/" + group.groupName + "/" + reportTitle;

                            if (report.settingkey == "Bill Register")
                            {
                                grdbill.ItemsSource = billRepo.getPurchaseRegister(SYSTEM_STATIC.currentUser.id);
                            }
                            else
                                grdbill.ItemsSource = billRepo.getAll(SYSTEM_STATIC.currentUser.id);
                        }
                        break;
                    }
                case GridReportType.MemorizedReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here   
                            mbtnExportToMemorizedReport1.IsVisible = false;
                            grdbill.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "Bills" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "Bills" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Bill Register")
                            {
                                grdbill.ItemsSource = billRepo.getPurchaseRegister(SYSTEM_STATIC.currentUser.id);
                            }
                            else
                                grdbill.ItemsSource = billRepo.getAll(SYSTEM_STATIC.currentUser.id);
                        }

                        break;
                    }
            }
            Loader.DeferedVisibility = false;
        }
        private void MbtnSaveAsNew1_Click(object sender, EventArgs e)
        {
            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                    ReportLogic.SaveGridReport(grdbill, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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

                        var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to save " + this.Title + "" + " as new Report ? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                    ReportLogic.SaveGridReport(grdbill, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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

        private void MbtnRenameReport1_Click(object sender, EventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to rename " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                        ReportLogic.RenameGridReport(grdbill, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        this.Close();
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
                                        ReportLogic.RenameGridReport(grdbill, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
                                        DXMessageBox.Show(report.reportName + " is renamed with ( " + updatedReport.reportName + " )", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                        this.Close();
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.Title + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
                                    ReportLogic.UpdateGridReport(grdbill, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdbill, str, reportType, groupDetails, report.Id, report.settingkey);

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
            var inputfromUser = DXMessageBox.Show("Do you want to delete " + this.Title + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

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

                                ReportLogic.DeleteReport(grdbill, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
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

                                ReportLogic.DeleteReport(grdbill, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
                                DXMessageBox.Show(report.reportName + " is Deleted Successfully! ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                                this.Close();
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
            var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {

                grdbill.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Memorized Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
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
                                    ReportLogic.SaveGridReport(grdbill, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to export " + this.Title + " to Standard Reports ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);


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
                                    ReportLogic.ExportToStandard(grdbill, reportName, reportType, reportGroup, report.settingkey);
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
            PrintableControlLink link = new PrintableControlLink((TableView)grdbill.View);
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

     

        private void Grdbill_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (tableView.CompactPanelShowMode == CompactPanelShowMode.CompactMode)
            //{
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.CompactMode;
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.Always;
            //}
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Vendor Bills Report") != null)
            {

                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to share " + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                    //GridControl gridControl = grdsaleOrder;
                    //gridControl.SaveLayoutToStream(memoryStream);
                    //report.settingValue = Convert.ToString(memoryStream).Trim();
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
                DXMessageBox.Show("You don't have permission Share Vendor Bills Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

            }
        }

        private void mbtnDriectClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                BillStatus oldStatus = new BillStatus();

                BillRepo billrepo = new BillRepo();
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null) ? true : false)
                {

                    if (grdbill.GetFocusedRow() != null)
                    {
                        UsersRepo usersRepo = new UsersRepo();
                        var row = grdbill.GetFocusedRow() as Bill;

                        if (row.BillStatus != null)
                        {
                            oldStatus = row.BillStatus;
                        }
                        Billss.ucStatuschange.inActiveStatuses = 1;

                        Billss.ucStatuschange.billid = (int)grdbill.GetFocusedRowCellValue(grdbill.Columns.GetColumnByFieldName("Id"));
                        Billss.frmBillStatusChange statusChange = new Billss.frmBillStatusChange(billrepo);
                        var myWindow = Window.GetWindow(this);
                        statusChange.Owner = myWindow;
                        statusChange.ShowDialog();
                        //if (Billss.ucStatuschange.bill.BillStatus.Id == row.BillStatus.Id)
                        //    return;
                        if (Billss.ucStatuschange.bill.Id != 0)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null) ? true : false)
                            {
                                Billss.ucStatuschange.bill.PendingForClosing = false;
                                Billss.ucStatuschange.bill.stage = TransactionStage.Approved.ToString();

                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Closing, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.billRepo.update(Inquiriess.ucStatuschange.bill);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                            {
                                Billss.ucStatuschange.bill.stage = TransactionStage.AwaitingApproval.ToString();
                                if (Billss.ucStatuschange.bill.PendingForClosing == null)
                                {
                                    Billss.ucStatuschange.bill.PendingForClosing = true;

                                }
                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null)
                            {
                                Billss.ucStatuschange.bill.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (Billss.ucStatuschange.bill.PendingForClosing != true)
                                {
                                    Billss.ucStatuschange.bill.PendingForClosing = true;

                                }
                                //Billss.ucStatuschange.bill.PendingForClosing = true;

                                frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            }
                            else
                            {
                                Billss.ucStatuschange.bill.stage = TransactionStage.AwaitingFirstReview.ToString();

                                Billss.ucStatuschange.bill.PendingForClosing = true;
                                usersRepo.Add(TransactionInfo.Closed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            }
                        //Billss.ucStatuschange.bill.user_Id = MainWindow.currentUserid;
                        Billss.ucStatuschange.bill.LastStatusChangeDate = System.DateTime.Now;
                        Billss.ucStatuschange.bill.ClosingDate = System.DateTime.Now;
                        if (row.BillStatus != Billss.ucStatuschange.bill.BillStatus)
                            usersRepo.Add(TransactionInfo.Status_Changed, row.Id, (int)TransactionItemType.Bill, "While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                        //Adding auto signature
                        //Adding auto Signature
                        if (oldStatus.Status != Billss.ucStatuschange.bill.BillStatus.Status)
                        {
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("Status of Bill has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                {
                                    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Bill);
                                    //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
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
                            string newStat = Billss.ucStatuschange.bill.BillStatus.Status;
                            string symbolCurr = "";
                            if (row.currency != null)
                            {
                                symbolCurr = row.currency.Abbrivation.ToString();
                            }
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Bill (Amount OC) having value: " + row.totalCFRValue.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(row.Id, TransactionItemType.Bill, comment, SYSTEM_STATIC.currentUser.employeeId);

                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + row.SalesReferenceNo, row.Id, TransactionItemType.Bill, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Bill #" + row.SalesReferenceNo, row.Id, TransactionItemType.Bill, comment.Comment, user.id, "New Comment ", null);
                                }
                            }

                        }




                        row = Billss.ucStatuschange.bill;
                        grdbill.RefreshData();
                        billrepo.updateStatus(row.Id, row.BillStatus);
                        //Billss.ucStatuschange.UpdateBill();//billRepo.update(Billss.ucStatuschange.bill);
                        MessageBox.Show("Bill status changed to InActive (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");

                    }

                }
                else
                {
                    MessageBox.Show("You are not Allowed to Close Bill Directly.");
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
                BillRepo billrepo = new BillRepo();

                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null) ? true : false)
                {
                    if (grdbill.GetFocusedRow() != null)
                    {
                        Bill bill = new Bill();
                        bill = grdbill.SelectedItem as Bill;
                        UsersRepo usersRepo = new UsersRepo();

                        if (bill.isApproved != true)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null) ? true : false)
                            {
                                bill.isApproved = true;
                                bill.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                            {
                                bill.stage = TransactionStage.AwaitingApproval.ToString();
                                if (bill.isApproved == null)
                                {
                                    bill.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null)
                            {
                                bill.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (bill.isApproved == null)
                                {
                                    bill.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            }
                            else
                            {
                                bill.isApproved = false;
                            }
                        else //reapprove when it is approved already
                        {
                            if (bill.isReApproved == false)
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Bill") != null) ? true : false)
                                {
                                    bill.isReApproved = true;
                                    bill.stage = TransactionStage.Approved.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                                {
                                    bill.stage = TransactionStage.AwaitingApproval.ToString();
                                    if (bill.isReApproved == null)
                                    {
                                        bill.isReApproved = false;
                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null)
                                {
                                    bill.stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (bill.isReApproved == null)
                                    {
                                        bill.isReApproved = false;
                                    }
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                                }
                                else
                                {
                                    bill.isReApproved = false;
                                }

                        }


                        billrepo.update(bill);
                        MessageBox.Show("Bill is Approved (" + bill.Id + ")");
                        SystemLog.LogInfo(this.GetType(), "Bill is Approved (" + bill.Id + ")");
                    }
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Approve Bill Directly");
                    SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve Bill Directly user id=(" + MainWindow.currentUserid + ")");
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
