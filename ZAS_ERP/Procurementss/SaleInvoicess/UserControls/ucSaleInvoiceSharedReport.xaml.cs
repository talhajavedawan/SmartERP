using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.SaleInvoicess.UserControls
{
    /// <summary>
    /// Interaction logic for ucSaleInvoiceSharedReport.xaml
    /// </summary>
    public partial class ucSaleInvoiceSharedReport : ThemedWindow
    {
        string reportTitle;
        public MainWindow myParent = null;

        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        static List<ExchangeRateGroup> exchangeRateGroups = new List<ExchangeRateGroup>();
        ExchangeRate exchangeRate = null;
        SharedReport report = new SharedReport();
        public ucSaleInvoiceSharedReport()
        {
            InitializeComponent();
        }
        public ucSaleInvoiceSharedReport(SharedReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            SaleInvoiceRepo repo = new SaleInvoiceRepo();
            GridReportRepo repoReport = new GridReportRepo();
            var group = repoReport.GetSharedGroup(Convert.ToInt32(report.sharedGroupId));
            if (report != null)
            {
                //Permissions Should be here                        
                grdsaleInvoice.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                txtUserName.Text = report.Creater.userName;
                txtDesignation.Text = report.Creater.employee.DesignationTitle;
                List<int> empIds = new List<int>();
                List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                List<Department> userdepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
                foreach (var userDept in userdepartments)
                {
                    empIds.AddRange(userDept.employees.Select(x => x.EmpId).Distinct().ToList());
                }
                foreach (var emp in report.SharedGridGroup.Employees)
                {
                    if (empIds.Contains(emp.EmpId))
                    {
                        employees.Add(emp);
                    }
                    employees.Distinct();

                }
                grdEmployee.ItemsSource = employees;
                var image = GetBitmapImageFromByteArray(report.Creater.employee.person.Photo);
                UserImage.Source = image;
                Title = "SaleInvoices" + "/" + group.groupName + "/" + reportTitle;
                lblHeading.Caption = "SaleInvoices" + "/" + group.groupName + "/" + reportTitle;
                grdsaleInvoice.ItemsSource = repo.getAll(SYSTEM_STATIC.currentUser.id);
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdsaleInvoice.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = report.reportName;
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = report.reportName;
            link.ReportHeaderData = report.reportName;
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
        }

        private void MbtnUpdateReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Shared Report") != null)
            {
                var inputfromUser = DXMessageBox.Show("Are you sure? \n do you want to update " + this.Title + "?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    var repo = new GridReportRepo();
                    //var report = repo.GetReportByName(this.Title);
                    var group = repo.GetSharedGroup(Convert.ToInt32(report.sharedGroupId));
                    string str = report.reportName;
                    SharedGridGroup groupDetails = group;
                    if (str != "")
                    {
                        ReportLogic.UpdateGridReport(grdsaleInvoice, str, groupDetails, report.Id, report.settingkey);

                        DXMessageBox.Show(" ( " + str + " ) is Updated Successfully!", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    return;
                }
            }
            else
            {
                DXMessageBox.Show("You don't have permission Edit Shared Report ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void MbtnDeleteReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Shared Report") != null)
            {
                GridReportRepo repo = new GridReportRepo();
                repo.DeleteSharedReport(report.Id);
                DXMessageBox.Show("Report has been deleted successfully ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                DXMessageBox.Show("You don't have permission Edit Shared Report ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void MbtnRefreshReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            grdsaleInvoice.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();

        }

        private void grdsaleInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            EditSaleInvoice();
        }

        private void EditSaleInvoice()
        {
            if (grdsaleInvoice.GetFocusedRowCellValue(grdsaleInvoice.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Invoice, (int)grdsaleInvoice.GetFocusedRowCellValue(grdsaleInvoice.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();

            }
        }

        private void GrdsaleInvoice_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            {
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
                                    //this will add current node to department list and transverse to its parent
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
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
                                    //this will add current node to department list and transverse to its parent
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
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
                                    //this will add current node to department list and transverse to its parent
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
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
                                    //this will add current node to department list and tranverse to its parent
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
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
                                    //this will add current node to department list and tranverse to its parent
                                    deptList.Add(node);
                                    node = node.parentDepartment;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    deptList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
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

                        //DateTime date; 
                        Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                        NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                        e.Value = NoDueAgeingDays;
                    }
                }

                //if (e.Column.FieldName == "Department")

                //{

                //   // string s = "Test: FieldTwo";

                //}
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
                    if (e.GetListSourceFieldValue("saleInvoiceDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("saleInvoiceDate"));

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
                if (e.Column.FieldName == "recieptTotalOC")
                {
                    var invoice = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
                    var reciepts = invoice.salesReceipts;
                    if (reciepts != null)
                    {
                        var result = reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount);
                        e.Value = result;
                    }
                }
                if (e.Column.FieldName == "totalDeductionsOC")
                {
                    var invoice = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
                    var reciepts = invoice.salesReceipts;
                    if (reciepts != null)
                    {
                        var result = reciepts.Where(x => x.isVoid != true).Sum(x => x.receiptDeductions.Sum(y => y.Amount));
                        e.Value = result;
                    }
                }
                if (e.Column.FieldName == "totalCreditedOC")
                {
                    var invoice = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
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
                    var invoice = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
                    var reciepts = invoice.salesReceipts;
                    if (reciepts != null && e.GetListSourceFieldValue("totalInvoiceAmount") != null)
                    {
                        var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalInvoiceAmount"));
                        var result = Convert.ToDecimal(reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount));
                        e.Value = total - result;
                    }
                }
                if (e.Column.FieldName == "remainingCollectionCMER")
                {
                    var invoice = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
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

                    var reciepts = invoice.salesReceipts;
                    if (reciepts != null && e.GetListSourceFieldValue("totalInvoiceAmount") != null && todayRate != 0)
                    {
                        var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalInvoiceAmount"));

                        var totalCollectionCMER = invoice.marginExchangeRate != 0 ? total * invoice.marginExchangeRate : total;

                        var result = todayRate != 0 ? Convert.ToDecimal(reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount)) * Convert.ToDecimal(todayRate) : Convert.ToDecimal(reciepts.Where(x => x.isVoid != true).Sum(x => x.CollectionAmount));
                        e.Value = totalCollectionCMER - result;
                    }
                }
                if (e.Column.FieldName == "invoiceAmountCMER")
                {
                    var invoice = grdsaleInvoice.GetRowByListIndex(e.ListSourceRowIndex) as SaleInvoice;
                    //var marketrate = marketexchangeRates.FirstOrDefault(x => x.company_Id == invoice.company_Id && x.target_currency_Id == invoice.currency_Id && (x.effectiveFrom <= System.DateTime.Now && System.DateTime.Now <= x.effectiveTo));
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
                    //if (marketrate != null)
                    //{
                    var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalInvoiceAmount"));

                    var result = todayRate != 0 ? total * Convert.ToDecimal(todayRate) : total;
                    e.Value = result;
                    //}
                }

            }
        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            SaleInvoiceRepo repo = new SaleInvoiceRepo();
            grdsaleInvoice.ItemsSource = repo.getAll(SYSTEM_STATIC.currentUser.id);
            grdsaleInvoice.ShowLoadingPanel = false;
        }

        private void MbtnRenameReport1_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Shared Report") != null)
            {

                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to rename " + this.Title + " report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
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
                DXMessageBox.Show("You don't have permission Edit Shared Report ", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }
        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
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
    }
}
