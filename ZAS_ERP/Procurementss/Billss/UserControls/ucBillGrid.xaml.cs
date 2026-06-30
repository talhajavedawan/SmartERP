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

namespace ZAS_ERP.Procurementss.Billss
{
    /// <summary>
    /// Interaction logic for ucBillGrid.xaml
    /// </summary>
    public partial class ucBillGrid : UserControl
    {
        public static int statusId;
        public static int AllActive;
        //public static int AllActive;
        ProcurementRepo procurementRepo = new ProcurementRepo();
        BillStatus oldStatus = new BillStatus();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        public ucBillGrid()
        {

            InitializeComponent();
            if (MainWindow.currentUserid != 0)
            {
               

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Bill List") != null)
                {
                    ApprovalCount = billrepo.getAllPendingForApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                    {
                        ApprovalCount = billrepo.getAllPendingForApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApprovalCount = billrepo.getAllPendingForApprovalCountOwn(MainWindow.currentUserid);
                    }
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Bill List") != null)
                {
                    ReApprovalCount = billrepo.getAllPendingForReApprovalDepartmentalCount(MainWindow.currentUserid);

                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                    {
                        ReApprovalCount = billrepo.getAllPendingForReApprovalCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ReApprovalCount = billrepo.getAllPendingForReApprovalCountOwn(MainWindow.currentUserid);
                    }
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Bills") != null)
                {
                    mbtnVoid.Visibility = Visibility.Visible;

                    if (MainWindow.currentUserid!=0)
                    {
                        VoidCount = billrepo.getVoidRegisterCount(MainWindow.currentUserid);

                    }
                    else
                    {
                        VoidCount= billrepo.getVoidRegisterAdministratorCount();

                    }


                }
                else
                {

                    mbtnVoid.Visibility = Visibility.Collapsed;

                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Bill Register") != null)
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Visible;
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null ||
                        SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                    {
                        ApproveunapprovedCount = billrepo.getPurchaseRegisterCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ApproveunapprovedCount = billrepo.getPurchaseRegisterCountOWn(MainWindow.currentUserid);
                    }
                }
                else
                {
                    mbtnApprovedUnapproved.Visibility = Visibility.Collapsed;
                }
            }

            else
            {
                ApprovalCount = billrepo.getAllPendingForAdministratorCount();
                ApproveunapprovedCount = billrepo.getPurchaseRegisterAdministratorCount();
            }
            if (MainWindow.currentUserid != 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Bill List") != null)
                {
                    ClosingCount = billrepo.getAllPendingForClosingDepartmentalCount(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                    {
                        ClosingCount = billrepo.getAllPendingForClosingCount(MainWindow.currentUserid);
                    }
                    else
                    {
                        ClosingCount = billrepo.getAllPendingForClosingCountOwn(MainWindow.currentUserid);

                    }

                }
            }


            else
            {
                ClosingCount = billrepo.getAllPendingForClosingAdministratorCount();
            }
            //this.DataContext = this;

        }

        BillRepo billrepo = new BillRepo();
        Bill bill = new Bill();
        public IList<ERP_BL.Databases.Bill> bills { get; set; }
        //public ObservableCollection<Bill> orders { get; set; }
        public int ApprovalCount { get; set; }
        public int ReApprovalCount { get; set; }
        public int VoidCount { get; set; }
        public int ClosingCount { get; set; }
        public int ApproveunapprovedCount { get; set; }
        /// <summary>
        /// runs when window is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucbillGrid_Loaded(object sender, RoutedEventArgs e)
        {

            //SystemLogic.SetUserSettingOfCurrentWindow(grdbill);
            loadBillgrid();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });

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
                    if(_bill != null && _bill.tax != null)
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
                        if(total > 0)
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
                        var paidAmount = Convert.ToDecimal(_bill.Payments.Where(x=>x.isVoid != true).Sum(x => x.DebitedAmount)) /*+ Convert.ToDecimal(_bill.Payments.Where(x => x.isVoid != true).Sum(x => x.Deductions))*/;
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
                                amountWithTax = Math.Round(Convert.ToDecimal(_bill.totalCFRValue) - paidAmount, 2);
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
                                amountWithTax = Convert.ToDecimal(_bill.totalCFRValue) ;
                            }
                        }
                        e.Value = amountWithTax;
                    }
                }
                //CreationDate


                //int unitsOnOrder = Convert.ToInt32(e.GetListSourceFieldValue("UnitsOnOrder"));
                //e.Value = price * unitsOnOrder;
            }
        }


        private void SetColumnNames()
        {
            grdbill.Columns["maker"].Header = "Bill Reference # 1";
            grdbill.Columns["origin"].Header = "Bill Reference # 2";
            grdbill.Columns["packing"].Header = "Bill Reference # 3";
            grdbill.Columns["DeliveryDate"].Header = "Bill Due Date";
        }



        /// <summary>
        /// Loads data into bill Grid and create columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadBillgrid()

        {
            //this.grdbill.ItemsSource = new List<Bill>();
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Bills(Open)" || lblHeading.Text == "Bills(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            //grdbill.Columns.Clear();
            //orders = new ObservableCollection<Bill>();
            //grdbill.ItemsSource = null;
            //grdbill.ItemsSource = orders;
            //grdbill.VisibleItems.Clear();

            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        bills = billrepo.getAll();
                        //this.grdbill.ItemsSource = bills;
                        //MessageBox.Show("You are not Authorized");

                        //return;
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Bills") != null)
                    {
                        bills = billrepo.getAllFirst(MainWindow.currentUserid);
                    }
                    else
                    {
                        bills = billrepo.getAllActiveFirst(MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    bills = billrepo.getAllActiveFirst(MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    bills = billrepo.getAllInActiveFirst(MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    lblHeading.Text = "Pending For Approvals (Bills)";                    if (MainWindow.currentUserid != 0)                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Bill List") != null)                        {                            bills = billrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);                        }                        else                        {                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)                            {                                bills = billrepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);                            }                            else                            {                                bills = billrepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);                            }                        }                    else                        bills = billrepo.getAllPendingForAdministratorFirst();
                }
                else if (AllActive == 4)                {                    lblHeading.Text = "(Pending for Closing) Bills";                    if (MainWindow.currentUserid != 0)                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Bill List") != null)                        {                            bills = billrepo.getAllPendingForClosingDepartmentalFirst(MainWindow.currentUserid);                        }                        else                        {                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)                            {                                bills = billrepo.getAllPendingForClosingFirst(MainWindow.currentUserid);                            }                            else                            {                                bills = billrepo.getAllPendingForClosingOwnFirst(MainWindow.currentUserid);                            }                        }
                    else                        bills = billrepo.getAllPendingForClosingAdministratorFirst();                    //grdbill.ItemsSource = bills;                    //SetColumnNames();
                    grdbill.Columns["CreationDate"].VisibleIndex = 0;

                    //grdbill.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);

                    //grdbill.View =new CardView();
                }
            }
            else
            {
                bills = billrepo.getAllPobyStatusIdFirst(MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdbill);
            grdbill.ItemsSource = bills;
            SetColumnNames();
            RemoveSourceObjects();
            SetColumnsVisibility();
        }

        private void SetColumnsVisibility()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Summary Memo for Vendor Bills") == null)
            {
                grdbill.Columns["SummaryMemo"].Visible = false;
                grdbill.Columns["SummaryMemo"].ShowInColumnChooser = false;
            }
        }

        private void grdbill_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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

        private void btnNewBill_Click(object sender, RoutedEventArgs e)
        {  
            Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Bill,0);
            procurmentPanel.Show();
        }

        private void btnEditBill_Click(object sender, RoutedEventArgs e)
        {
            EditBill();
        }

        private void Ucbillgrid_Unloaded(object sender, RoutedEventArgs e)
        {
            //AllActive = 0;
            //SystemLogic.SaveUserSettingForCurrentWindow(grdbill);
            //statusId = 0;
        }

        private void RbtnCopy_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            grdbill.View.ShowPrintPreview(this);
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
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Bills Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bills") != null)
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
                        ReportLogic.SaveGridReport(grdbill, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Bills Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //this.RemoveFromVisualTree();
            Billss.ucBillGrid ucBillGrid = new ucBillGrid();
            this.Content = ucBillGrid;
            //InitializeComponent();
            //loadBillgrid();
            SetColumnsVisibility();
        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate((sender as MenuItem).Header.ToString());
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdbill.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = (sender as MenuItem).Header.ToString();
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = (sender as MenuItem).Header.ToString();
            link.ReportHeaderData = (sender as MenuItem).Header.ToString();
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdbill.View.ShowPrintPreview(this);
        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null) ? true : false)
            {

                if (grdbill.GetFocusedRow() != null)
                {
                    UsersRepo usersRepo = new UsersRepo();
                    var row = grdbill.GetFocusedRow() as Bill;
                    if (row.isApproved == false)
                    {
                        DXMessageBox.Show("Bill is pending for approval!");
                        return;
                    }

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
                            frmInputBox inputBox = new frmInputBox( "While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Reviewed, Billss.ucStatuschange.bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                        }
                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null)
                        {
                            Billss.ucStatuschange.bill.stage = TransactionStage.AwaitingSecondReview.ToString();
                            if (Billss.ucStatuschange.bill.PendingForClosing !=true)
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
                    if(row.BillStatus!= Billss.ucStatuschange.bill.BillStatus)
                    usersRepo.Add(TransactionInfo.Status_Changed, bill.Id, (int)TransactionItemType.Bill, "While direct closing Status Changed from (" + row.BillStatus.Status + ") to (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
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
                                winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), bill.Id, TransactionItemType.Bill);
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
                        Comment = "Status of Bill (Amount OC) having value: " + row.totalCFRValue.ToString() + " and Bill (Amount SOC) " + bill.SoAmountSOC_ER?.ToString() + " (" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
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
                    billrepo.updateStatus(row.Id,row.BillStatus);
                    //Billss.ucStatuschange.UpdateBill();//billRepo.update(Billss.ucStatuschange.bill);
                    MessageBox.Show("Bill status changed to InActive (" + Billss.ucStatuschange.bill.BillStatus.Status + ")");
                    
                }

            }
            else
            {
                MessageBox.Show("You are not Allowed to Close Bill Directly.");
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
            LoadPendingForApproval();
            grdbill.ItemsSource = bills;
            SetColumnNames();
            grdbill.Columns["CreationDate"].VisibleIndex = 0;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void LoadPendingForApproval()
        {
            lblHeading.Text = "Pending Bills";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Bill List") != null)
                {
                    bills = billrepo.getAllPendingForApprovalDepartmentalFirst(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                    {
                        bills = billrepo.getAllPendingForApprovalFirst(MainWindow.currentUserid);
                    }

                    else
                    {
                        bills = billrepo.getAllPendingForApprovalOwnFirst(MainWindow.currentUserid);

                    }
                }
            else
                bills = billrepo.getAllPendingForAdministratorFirst();

        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Pending for ReApprovals Bills";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Bill List") != null)
                {
                    bills = billrepo.getAllPendingForReApprovalDepartmentalFirst(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                    {
                        bills = billrepo.getAllPendingForReApprovalFirst(MainWindow.currentUserid);
                    }

                    else
                    {
                        bills = billrepo.getAllPendingForReApprovalOwnFirst(MainWindow.currentUserid);

                    }
                }
            else
                bills = billrepo.getAllPendingForAdministratorFirst();

            grdbill.ItemsSource = bills;
            SetColumnNames();
            //grdbill.ClearGrouping();
            //grdbill.FilterString = "";
            //grdbill.GroupBy("stage");
            //grdbill.Columns["stage"].GroupIndex = 0;
            grdbill.Columns["CreationDate"].VisibleIndex = 0;
            //grdbill.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);
            //grdbill.View = new CardView();
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
            lblHeading.Text = "(Pending for Closing) Bills";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Bill List") != null)
                {
                    bills = billrepo.getAllPendingForClosingDepartmental(MainWindow.currentUserid);


                }
                else
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                    {
                        bills = billrepo.getAllPendingForClosing(MainWindow.currentUserid);
                    }

                    else
                    {
                        bills = billrepo.getAllPendingForClosingOwn(MainWindow.currentUserid);

                    }
                }
            //bills = billrepo.getAllPendingForApproval(MainWindow.currentUserid);
            else
                bills = billrepo.getAllPendingForClosingAdministrator();
            grdbill.ItemsSource = bills;
            SetColumnNames();
            //grdbill.ClearGrouping();
            //grdbill.FilterString = "";
            //grdbill.ItemsSource = bills;
            //grdbill.Columns["stage"].GroupIndex = 0;
            //grdbill.GroupBy("stage");
            grdbill.Columns["CreationDate"].VisibleIndex = 0;

            //grdbill.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, string.Empty);

            //grdbill.View =new CardView();
            statusId = 0;
            AllActive = 4;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null) ? true : false)
            {
                if (grdbill.GetFocusedRow() != null)
                {
                    Bill bill = new Bill();
                    //Inquiriess.ucStatuschange.billid = (int)grdbill.GetFocusedRowCellValue(grdbill.Columns.GetColumnByFieldName("Id"));
                    bill = grdbill.SelectedItem as Bill;
                    //Inquiriess.frmInqyuiryStatusChange statusChange = new Inquiriess.frmInqyuiryStatusChange();
                    //statusChange.Owner = this;
                    //var myWindow = Window.GetWindow(this);
                    //statusChange.Owner = myWindow;
                    //statusChange.ShowDialog();
                    UsersRepo usersRepo = new UsersRepo();

                    if (bill.isApproved != true)
                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null) ? true : false)
                        {
                            bill.isApproved = true;
                            bill.stage = TransactionStage.Approved.ToString();
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                            usersRepo.Add(TransactionInfo.Approved_Adding, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                            //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

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
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

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
        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdbill);
        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Bill Register";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                {
                    bills = billrepo.getPurchaseRegisterFirst(MainWindow.currentUserid);
                }

                else
                {
                    //Displaying all bills instead of OWN transcations
                    //bills = billrepo.getPurchaseRegisterOwn(MainWindow.currentUserid);
                    bills = billrepo.getPurchaseRegisterFirst(MainWindow.currentUserid);

                }

            else
                bills = billrepo.getPurchaseRegisterAdministratorFirst();
            grdbill.ItemsSource = bills;
            SetColumnNames();
            grdbill.Columns["CreationDate"].VisibleIndex = 0;
            statusId = 0;
            AllActive = 6;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void RemoveSourceObjects()
        {
            grdbill.Columns.GetColumnByFieldName("Id").Visible = false;
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("company"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("incoterm"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("paymentTerm"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("TitleValue1"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("TitleValue2"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("currency"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("bid"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("BillStatus"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("vendor"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("employee"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("department"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("customerCompany"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("principal"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("company_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("incoterm_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("paymentterm_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("TitleValue1Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("TitleValue2Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("currency_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("bid_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("offer_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("offer"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("allocation_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("dept_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("customerCompany_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("principal_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("user_Id"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("user"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("vendorPaymentId"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("vendorPaymentStatus"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("CostSheetId"));
            grdbill.Columns.Remove(grdbill.Columns.GetColumnByFieldName("CostSheet"));
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Wait;
            });
            UsersRepo usersRepo = new UsersRepo();

            lblHeading.Text = "Void Bills";
            if (MainWindow.currentUserid != 0)
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Bills") != null )
                {
                    bills = billrepo.getVoidRegister(MainWindow.currentUserid);
                }

                else
                {
                    bills = billrepo.getVoidRegisterOwn(MainWindow.currentUserid);

                }

            else
                bills = billrepo.getVoidRegisterAdministrator();
            grdbill.ItemsSource = bills;
            SetColumnNames();
            grdbill.Columns["CreationDate"].VisibleIndex = 0;
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
        private void LoadOrdersByDate()
        {
            lblHeading.Text = SYSTEM_STATIC.gridTitle;
            if (lblHeading.Text == "Bills(Open)" || lblHeading.Text == "Bills(Closed)")
            { mbtnExportToReport.IsEnabled = false; }
            //grdbill.Columns.Clear();
            //orders = new ObservableCollection<Bill>();
            //grdbill.ItemsSource = null;
            //grdbill.ItemsSource = orders;
            //grdbill.VisibleItems.Clear();

            if (statusId == 0)
            {
                if (AllActive == 0)
                {
                    if (MainWindow.currentUserid == 0)
                    {
                        bills = billrepo.getAll();
                        //this.grdbill.ItemsSource = bills;
                        //MessageBox.Show("You are not Authorized");

                        //return;
                    }
                    else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Bills") != null)
                    {
                        bills = billrepo.getAllByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                    else
                    {
                        bills = billrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                    }
                }
                else if (AllActive == 1)
                {
                    bills = billrepo.getAllActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);

                }
                else if (AllActive == 2)
                {
                    bills = billrepo.getAllInActiveByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);
                }
                else if (AllActive == 3)
                {
                    lblHeading.Text = "Pending For Approvals (Bills)";                    if (MainWindow.currentUserid != 0)                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for Approval) Bill List") != null)                        {                            bills = billrepo.getAllPendingForApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);                        }                        else                        {                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)                            {                                bills = billrepo.getAllPendingForApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);                            }                            else                            {                                bills = billrepo.getAllPendingForApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);                            }                        }                    else                        bills = billrepo.getAllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                }
                else if (AllActive == 4)                {                    lblHeading.Text = "(Pending for Closing) Bills";                    if (MainWindow.currentUserid != 0)                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for closing) Bill List") != null)                        {                            bills = billrepo.getAllPendingForClosingDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);                        }                        else                        {                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)                            {                                bills = billrepo.getAllPendingForClosingByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);                            }                            else                            {                                bills = billrepo.getAllPendingForClosingOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid);                            }                        }
                    else                        bills = billrepo.getAllPendingForClosingAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                }
                else
                    if(AllActive==5)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Pending for ReApprovals Bills";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View(Pending for ReApproval) Bill List") != null)
                        {
                            bills = billrepo.getAllPendingForReApprovalDepartmentalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue,MainWindow.currentUserid);
                        }
                        else
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                            {
                                bills = billrepo.getAllPendingForReApprovalByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue,MainWindow.currentUserid);
                            }
                            else
                            {
                                bills = billrepo.getAllPendingForReApprovalOwnByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue,MainWindow.currentUserid);
                            }
                        }
                    else
                        bills = billrepo.getAllPendingForAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                }
                else
                    if(AllActive==6)
                {
                    UsersRepo usersRepo = new UsersRepo();

                    lblHeading.Text = "Bill Register";
                    if (MainWindow.currentUserid != 0)
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null)
                        {
                            bills = billrepo.getPurchaseRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue,MainWindow.currentUserid);
                        }

                        else
                        {
                            bills = billrepo.getPurchaseRegisterByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue,MainWindow.currentUserid);
                        }

                    else
                        bills = billrepo.getPurchaseRegisterAdministratorByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue);
                    statusId = 0;
                    AllActive = 6;
                }
            }
            else
            {
                bills = billrepo.getAllPobyStatusIdByDateRange((DateTime)dateFrom.EditValue, (DateTime)dateTo.EditValue, MainWindow.currentUserid, statusId);
            }
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdbill);
            grdbill.ItemsSource = bills;
            SetColumnNames();
            RemoveSourceObjects();
        }
    }
}
