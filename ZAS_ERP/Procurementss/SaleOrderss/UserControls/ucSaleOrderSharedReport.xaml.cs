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

namespace ZAS_ERP.Procurementss.SaleOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucSaleOrderSharedReport.xaml
    /// </summary>
    public partial class ucSaleOrderSharedReport : ThemedWindow
    {
        SharedReport report = new SharedReport();
        public MainWindow myParent = null;
        string reportTitle;
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        static List<ExchangeRateGroup> exchangeRateGroupsSER = new List<ExchangeRateGroup>();
        static List<ExchangeRateGroup> exchangeRateGroupsMER = new List<ExchangeRateGroup>();
        ExchangeRate exchangeRateSER = null;
        ExchangeRate exchangeRateMER = null;
        ExchangeRate exchangeRateCMER = null;
        public ucSaleOrderSharedReport()
        {
            InitializeComponent();
        }
        public ucSaleOrderSharedReport(SharedReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }

        private void GrdsaleOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSaleOrder();
        }
        private void EditSaleOrder()
        {
            if (grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id")) != null)
            {
                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, (int)grdsaleOrder.GetFocusedRowCellValue(grdsaleOrder.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();
            }
        }
        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }

        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            grdsaleOrder.ShowLoadingPanel = true;
            var repo = new GridReportRepo();
            string path = this.Title;
            int pos = path.LastIndexOf("/") + 1;
            var reportTitlePath = path.Substring(pos, path.Length - pos);

            SharedReport report = repo.GetSharedReportByName(reportTitlePath);
            SaleOrderRepo soRepo = new SaleOrderRepo();
            if (report.settingkey == "Sale Register")
            { grdsaleOrder.ItemsSource = soRepo.getSaleRegister(SYSTEM_STATIC.currentUser.id); }
            else
            { grdsaleOrder.ItemsSource = soRepo.getAll(SYSTEM_STATIC.currentUser.id); }
            grdsaleOrder.ShowLoadingPanel = false;

        }
        public void LoadSERGroups()
        {
            exchangeRateGroupsSER = exchangeRateGroupRepo.GetAllSER();
        }
        public void LoadMERGroups()
        {
            exchangeRateGroupsMER = exchangeRateGroupRepo.GetAllMER();
        }
        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            grdsaleOrder.ShowLoadingPanel = true;
            SaleOrderRepo soRepo = new SaleOrderRepo();
            GridReportRepo repo = new GridReportRepo();
            LoadSERGroups();
            LoadMERGroups();
            var group = repo.GetSharedGroup(Convert.ToInt32(report.sharedGroupId));
            if (report != null)
            {
                grdsaleOrder.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                Title = "SaleOrders" + "/" + group.groupName + "/" + reportTitle;
                lblHeading.Caption = "SaleOrders" + "/" + group.groupName + "/" + reportTitle;
                txtUserName.Text = report.Creater.userName;
                txtDesignation.Text = report.Creater.employee.DesignationTitle;
                List<int> empIds = new List<int>();
                List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                List<Department> userdepartments = SYSTEM_STATIC.LoadCurrentUserDepartments();
                foreach (var userDept in  userdepartments)
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
                if (report.settingkey == "Sale Register")
                {
                    grdsaleOrder.ItemsSource = soRepo.getSaleRegister(SYSTEM_STATIC.currentUser.id);
                }
                else
                    grdsaleOrder.ItemsSource = soRepo.getAll(SYSTEM_STATIC.currentUser.id);


            }
            grdsaleOrder.ShowLoadingPanel = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }

        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdsaleOrder.View);
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
                        ReportLogic.UpdateGridReport(grdsaleOrder, str, groupDetails, report.Id, report.settingkey);

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
            grdsaleOrder.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
           
        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)

                if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                {
                    var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                var row = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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


            if (e.IsGetData)
                switch (e.Column.FieldName)
                {
                    case "Vendorss":
                        if (e.Column.FieldName == "Vendorss" && e.IsGetData)
                        {
                            var _SO = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            string vendorNames = "";

                            if (_SO.vendors != null && _SO.vendors.Count > 0)
                            {
                                vendorNames = String.Join(" | ", _SO.vendors.Select(x => x.company.CompanyName));
                            }
                            e.Value = vendorNames;
                        }
                        break;
                    case "PaymentDueAgeingDays":
                        if (e.GetListSourceFieldValue("PaymentDueAgeing") != null)
                        {
                            DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("PaymentDueAgeing"));
                            Double NoDueAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                            NoDueAgeingDays = Math.Round(NoDueAgeingDays, 0);
                            e.Value = NoDueAgeingDays;
                        }
                        break;
                    case "Department":
                        break;
                    case "CreationAgeing":
                        if (e.GetListSourceFieldValue("CreationDate") != null)
                        {
                            DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("CreationDate"));


                            Double CreateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                            CreateAgeingDays = Math.Round(CreateAgeingDays, 0);
                            e.Value = CreateAgeingDays;
                        }
                        break;
                    case "SODateAgeing":
                        if (e.GetListSourceFieldValue("saleOrderDate") != null)
                        {
                            DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("saleOrderDate"));


                            Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                            SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                            e.Value = SoDateAgeingDays;
                        }
                        break;
                    case "SODeliveryAgeing":
                        if (e.GetListSourceFieldValue("deliveryDate") != null)
                        {
                            DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("deliveryDate"));


                            Double SoDateAgeingDays = (Convert.ToDateTime(System.DateTime.Now) - dateTime).TotalDays;
                            SoDateAgeingDays = Math.Round(SoDateAgeingDays, 0);
                            e.Value = SoDateAgeingDays;
                        }
                        break;
                    case "totalWeight":
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

                        break;
                    case "totalQuantity":
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

                        break;
                    case "BudgetMarginOC":
                        if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                        {
                            decimal total = 0;
                            var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                            if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                            }
                            else
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                            }
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result1 = total - Cost.TotalBudgetedMargin;
                            e.Value = result1;
                        }
                        else
                        {
                            e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("margin"));
                        }
                        break;

                    case "BudgetMarginSER":
                        var selectedRow = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var budgetCost = Convert.ToDouble(e.GetListSourceFieldValue("BudgetMarginOC"));
                        if (selectedRow.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = budgetCost * selectedRow.ccSER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(budgetCost) * Convert.ToDouble(selectedRow.marginExchangeRate);
                        }

                        break;
                    case "BudgetMarginMER":
                        var selectedRow1 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var budgetCostMER = Convert.ToDouble(e.GetListSourceFieldValue("BudgetMarginOC"));

                        if (selectedRow1.saleOrdertype == InquiryType.SupplyCCC)
                        {

                            e.Value = budgetCostMER * selectedRow1.ccSER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(budgetCostMER) * Convert.ToDouble(selectedRow1.exchangeRate);
                        }

                        break;
                    case "RevisedMarginSER":
                        var selectedRow2 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var revisedCost = Convert.ToDouble(e.GetListSourceFieldValue("RevisedMarginOC"));

                        if (selectedRow2.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = revisedCost * selectedRow2.ccSER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(revisedCost) * Convert.ToDouble(selectedRow2.marginExchangeRate);
                        }

                        break;
                    case "RevisedMarginMER":
                        var selectedRow3 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var revisedCostMER = Convert.ToDouble(e.GetListSourceFieldValue("RevisedMarginOC"));

                        if (selectedRow3.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = revisedCostMER * selectedRow3.ccSER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(revisedCostMER) * Convert.ToDouble(selectedRow3.exchangeRate);
                        }

                        break;
                    case "actualMarginSER":
                        var selectedRow4 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var actualMargin = Convert.ToDouble(e.GetListSourceFieldValue("ActualMarginOC"));

                        if (selectedRow4.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = actualMargin * selectedRow4.ccSER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(actualMargin) * Convert.ToDouble(selectedRow4.marginExchangeRate);
                        }

                        break;
                    case "actualMarginMER":
                        var selectedRow5 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var actualMarginMER = Convert.ToDouble(e.GetListSourceFieldValue("ActualMarginOC"));

                        if (selectedRow5.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = actualMarginMER * selectedRow5.ccSER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(actualMarginMER) * Convert.ToDouble(selectedRow5.exchangeRate);
                        }

                        break;
                    case "SalesSystemMargin1":
                        var selectedRow6 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var systemMargin = Convert.ToDouble(e.GetListSourceFieldValue("SystemMargin"));

                        if (selectedRow6.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = systemMargin * selectedRow6.ccSER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(systemMargin) * Convert.ToDouble(selectedRow6.marginExchangeRate);
                        }

                        break;
                    case "SalesMarketMargin1":
                        var selectedRow7 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var systemMarginMER = Convert.ToDouble(e.GetListSourceFieldValue("SystemMargin"));

                        if (selectedRow7.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = systemMarginMER * selectedRow7.ccSER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(systemMarginMER) * Convert.ToDouble(selectedRow7.exchangeRate);
                        }

                        break;



                    case "BudgetedMarginPercentAge":
                        if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                        {
                            decimal total = 0;
                            var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                            if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                            }
                            else
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                            }
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result4 = ((total - Cost.TotalBudgetedMargin) / total) * 100;
                            e.Value = result4;
                        }
                        else
                        {
                            e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("BudgetedMarginPercent"));
                        }
                        break;
                    case "ActualMarginOC":
                        if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                        {
                            decimal total = 0;
                            var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                            if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                            }
                            else
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                            }

                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result5 = total - Cost.TotalActualMargin;
                            e.Value = result5;
                        }
                        else
                        {
                            e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMargin"));
                        }
                        break;

                    case "ActualMarginPercentAge":
                        if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                        {
                            decimal total = 0;
                            var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                            if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                            }
                            else
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                            }
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result8 = ((total - Cost.TotalActualMargin) / total) * 100;
                            e.Value = result8;
                        }
                        else
                        {
                            e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("ActualMarginPercent"));
                        }
                        break;
                    case "RevisedMarginOC":
                        if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                        {
                            decimal total = 0;
                            var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                            if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                            }
                            else
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                            }
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result9 = total - Cost.TotalRevisedMargin;
                            e.Value = result9;


                        }
                        else
                        {
                            e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMargin"));
                        }
                        break;

                    case "RevisedMarginPercentAge":
                        if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                        {
                            decimal total = 0;
                            var selectedRow66 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                            if (selectedRow66.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("costCenterAmount"));

                            }
                            else
                            {
                                total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));

                            }
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;
                            var result12 = ((total - Cost.TotalRevisedMargin) / total) * 100;
                            e.Value = result12;
                        }
                        else
                        {
                            e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMarginPercent"));
                        }
                        break;
                    case "invoicedTotalOC":
                        var saleOrder = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        e.Value = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount);
                        break;
                    case "recieptTotalOC":
                        var saleOrder1 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        e.Value = saleOrder1.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                        break;
                    case "totalDeductionsOC":
                        var saleOrder2 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        e.Value = saleOrder2.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.receiptDeductions.Sum(y => y.Amount)));
                        break;
                    case "totalCreditedOC":
                        var saleOrder3 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var result = saleOrder3.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.receiptDeductions.Sum(y => y.Amount)));
                        var collection = saleOrder3.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                        e.Value = collection - result;
                        break;
                    case "remainingCollectionOC":
                        var saleOrder4 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        if (saleOrder4.saleOrdertype == InquiryType.DistributionBiz)
                        {
                            var invoicedAmount = Math.Round((saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(y => y.BookerStatementItems.Sum(z => z.siNetAmount))).Value, 2);
                            var collected = Math.Round(Convert.ToDouble(saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount))), 2);
                            e.Value = Math.Round((Convert.ToDouble(invoicedAmount) - collected), 2);
                        }
                        else if (saleOrder4.saleOrdertype == InquiryType.Principal)
                        {
                            if (saleOrder4.SaleInvoices != null && e.GetListSourceFieldValue("commision") != null)
                            {
                                var total = Convert.ToDecimal(e.GetListSourceFieldValue("commision"));
                                var result13 = Convert.ToDecimal(saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                                e.Value = total - result13;
                            }
                        }
                        else
                        {
                            if (saleOrder4.SaleInvoices != null && e.GetListSourceFieldValue("totalCFRValue") != null)
                            {
                                var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                                var result13 = Convert.ToDecimal(saleOrder4.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                                e.Value = total - result13;
                            }
                        }

                        break;
                    case "SystemCost":
                        var saleOrder5 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        if (saleOrder5.CostSheet_Id != null)
                        {
                            var systemCost = saleOrderRepo.GetSOSystemCost((int)saleOrder5.CostSheet_Id);

                            e.Value = systemCost;
                        }
                        break;

                    case "budgetCost":
                        if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("marginExchangeRate") != null && e.GetListSourceFieldValue("CostSheet") != null)
                        {
                            var exchangerate = Convert.ToDecimal(e.GetListSourceFieldValue("exchangeRate"));
                            var Cost = e.GetListSourceFieldValue("CostSheet") as ERP_BL.Databases.CostSheet;

                            var fields = Cost.FieldValues.Where(x => x.Type == 1).ToList();
                            decimal totalFieldValue = fields.Sum(x => x.Value);
                            var result3 = totalFieldValue /** exchangerate*/;
                            e.Value = Math.Round(result3, 2);
                        }
                        else
                        {
                            e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("SalesBudgetedMargin"));
                        }
                        break;

                    case "adjCost":
                        {
                            var adjCost = saleOrderRepo.GetAdjustmentCost(Convert.ToInt32(e.GetListSourceFieldValue("Id")), Convert.ToInt32(e.GetListSourceFieldValue("CostSheet_Id")));
                            e.Value = adjCost;
                        }
                        break;
                    case "SER":
                        {
                            var saleOrderSER = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            double todayRate = 0;
                            var exchangeRateGroupSER = exchangeRateGroupsSER.FirstOrDefault(x => x.transaction_currency_Id == saleOrderSER.currency_Id && x.base_currency_Id == saleOrderSER.company.CurrencyId && x.TargetYear == saleOrderSER.CreationDate.Value.Year);
                            if (exchangeRateGroupSER != null)
                            {
                                exchangeRateSER = exchangeRateGroupSER.exchangeRates.FirstOrDefault(x => x.company_Id == saleOrderSER.company_Id);
                                switch (saleOrderSER.CreationDate.Value.Month)
                                {
                                    case 1:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateJan;
                                        break;
                                    case 2:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateFeb;
                                        break;
                                    case 3:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateMar;
                                        break;
                                    case 4:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateApr;
                                        break;
                                    case 5:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateMay;
                                        break;
                                    case 6:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateJun;
                                        break;
                                    case 7:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateJul;
                                        break;
                                    case 8:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateAug;
                                        break;
                                    case 9:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateSep;
                                        break;
                                    case 10:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateOct;
                                        break;
                                    case 11:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateNov;
                                        break;
                                    case 12:
                                        if (exchangeRateSER != null)
                                            todayRate = exchangeRateSER.rateDec;
                                        break;
                                    default:
                                        if (exchangeRateSER != null)
                                            todayRate = 0;
                                        break;
                                }
                            }
                            var total = todayRate;
                            e.Value = total;
                        }
                        break;
                    case "MER":
                        {
                            var saleOrderMER = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            double todayRate = 0;
                            var exchangeRateGroupMER = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == saleOrderMER.currency_Id && x.base_currency_Id == saleOrderMER.company.CurrencyId && x.TargetYear == saleOrderMER.CreationDate.Value.Year);
                            if (exchangeRateGroupMER != null)
                            {
                                exchangeRateMER = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == saleOrderMER.company_Id);
                                switch (saleOrderMER.CreationDate.Value.Month)
                                {
                                    case 1:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJan;
                                        break;
                                    case 2:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateFeb;
                                        break;
                                    case 3:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateMar;
                                        break;
                                    case 4:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateApr;
                                        break;
                                    case 5:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateMay;
                                        break;
                                    case 6:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJun;
                                        break;
                                    case 7:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateJul;
                                        break;
                                    case 8:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateAug;
                                        break;
                                    case 9:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateSep;
                                        break;
                                    case 10:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateOct;
                                        break;
                                    case 11:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateNov;
                                        break;
                                    case 12:
                                        if (exchangeRateMER != null)
                                            todayRate = exchangeRateMER.rateDec;
                                        break;
                                    default:
                                        if (exchangeRateMER != null)
                                            todayRate = 0;
                                        break;
                                }
                            }
                            var total = todayRate;
                            e.Value = total;
                        }
                        break;
                    case "CMER":
                        {
                            var SOCMER = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                            double todayRate = 0;
                            var exchangeRateGroupMER = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == SOCMER.currency_Id && x.base_currency_Id == SOCMER.company.CurrencyId && x.TargetYear == DateTime.Now.Year);
                            if (exchangeRateGroupMER != null)
                            {
                                exchangeRateCMER = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == SOCMER.company_Id);
                                switch (DateTime.Now.Month)
                                {
                                    case 1:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateJan;
                                        break;
                                    case 2:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateFeb;
                                        break;
                                    case 3:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateMar;
                                        break;
                                    case 4:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateApr;
                                        break;
                                    case 5:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateMay;
                                        break;
                                    case 6:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateJun;
                                        break;
                                    case 7:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateJul;
                                        break;
                                    case 8:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateAug;
                                        break;
                                    case 9:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateSep;
                                        break;
                                    case 10:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateOct;
                                        break;
                                    case 11:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateNov;
                                        break;
                                    case 12:
                                        if (exchangeRateCMER != null)
                                            todayRate = exchangeRateCMER.rateDec;
                                        break;
                                    default:
                                        if (exchangeRateCMER != null)
                                            todayRate = 0;
                                        break;
                                }
                            }
                            var total = todayRate;
                            e.Value = total;
                        }
                        break;
                    case "supervisorPoints":
                        var order = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        if (order.PerformanceSheet_Id != null)
                        {
                            e.Value = order.PerformanceSheet.performanceSheetFields.Sum(x => x.point);
                        }
                        //else
                        //{
                        //    e.Value = 0;
                        //}
                        break;
                    case "depHeadPoints":
                        var order1 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        if (order1.PerformanceSheet_Id != null)
                        {
                            e.Value = order1.PerformanceSheet.performanceSheetFields.Sum(x => x.revisedPoint);
                        }
                        //else
                        //{
                        //    e.Value = 0;
                        //}
                        break;
                    case "finalPoints":
                        var order2 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        if (order2.PerformanceSheet_Id != null)
                        {
                            e.Value = order2.PerformanceSheet.totalPoints;
                        }
                        //else
                        //{
                        //    e.Value = 0;
                        //}
                        break;
                    case "totalPoints":
                        var order3 = grdsaleOrder.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        if (order3.PerformanceSheet_Id != null)
                        {
                            e.Value = order3.PerformanceSheet.performanceSheetFields.Sum(x => x.PerformanceSheetHead.totalPoints);
                        }
                        //else
                        //{
                        //    e.Value = 0;
                        //}
                        break;
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
    }
}
