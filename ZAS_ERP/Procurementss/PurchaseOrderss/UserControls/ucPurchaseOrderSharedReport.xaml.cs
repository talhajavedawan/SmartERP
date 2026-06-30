using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
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

namespace ZAS_ERP.Procurementss.PurchaseOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucPurchaseOrderSharedReport.xaml
    /// </summary>
    public partial class ucPurchaseOrderSharedReport : ThemedWindow
    {
        SharedReport report = new SharedReport();
        public MainWindow myParent = null;
        string reportTitle;
        PurchaseOrderRepo purchaseOrderRepo = new PurchaseOrderRepo();
        public ucPurchaseOrderSharedReport()
        {
            InitializeComponent();

        }
        public ucPurchaseOrderSharedReport(SharedReport reportToEdit, string title)
        {
            InitializeComponent();
            report = reportToEdit;
            reportTitle = title;
        }
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
            SharedReport report = repo.GetSharedReportByName(reportTitlePath);
            purchaseOrderRepo = new PurchaseOrderRepo();
            if (report.settingkey == "Purchase Register")
            { grdpurchaseOrder.ItemsSource = purchaseOrderRepo.getPurchaseRegister(SYSTEM_STATIC.currentUser.id); }
            else
            { grdpurchaseOrder.ItemsSource = purchaseOrderRepo.getAll(SYSTEM_STATIC.currentUser.id); }
        }
        private void GrdpurchaseOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditPurchaseOrder();
        }

        private void EditPurchaseOrder()
        {
            if (grdpurchaseOrder.GetFocusedRowCellValue(grdpurchaseOrder.Columns.GetColumnByFieldName("Id")) != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Purchase_Order, (int)grdpurchaseOrder.GetFocusedRowCellValue(grdpurchaseOrder.Columns.GetColumnByFieldName("Id")));
                procurmentPanel.Show();

            }
        }

        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            PurchaseOrderRepo poRepo = new PurchaseOrderRepo();
            GridReportRepo repo = new GridReportRepo();
            var group = repo.GetSharedGroup(Convert.ToInt32(report.sharedGroupId));
            if (report != null)
            {
                grdpurchaseOrder.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
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
                Title = "Purchase Orders" + "/" + group.groupName + "/" + reportTitle;
                lblHeading.Caption = "Purchase Orders" + "/" + group.groupName + "/" + reportTitle;
                if (report.settingkey == "Purchase Register")
                { grdpurchaseOrder.ItemsSource = poRepo.getPurchaseRegister(SYSTEM_STATIC.currentUser.id); }
                else
                    grdpurchaseOrder.ItemsSource = poRepo.getAll(SYSTEM_STATIC.currentUser.id);

            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            });
        }
        private void MbtnUpdateReport1_Click(object sender, EventArgs e)
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
                        ReportLogic.UpdateGridReport(grdpurchaseOrder, str, groupDetails, report.Id, report.settingkey);

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

        private void MbtnDeleteReport1_Click(object sender, EventArgs e)
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
        private void MbtnRefreshReport1_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }
        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdpurchaseOrder.View);
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
        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            //if (e.IsGetData)
            {
                if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                {
                    var row = grdpurchaseOrder.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
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
                    var row = grdpurchaseOrder.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
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
                    var row = grdpurchaseOrder.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
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
                    var row = grdpurchaseOrder.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
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
                    var row = grdpurchaseOrder.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
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


                        //if (deptList.Count == 1)
                        //{
                        //    e.Value = deptList[0].DeptName;
                        //}
                        //if (deptList.Count == 2)
                        //{
                        //    e.Value = deptList[1].DeptName;
                        //}
                        //if (deptList.Count == 3)
                        //{
                        //    e.Value = deptList[2].DeptName;
                        //}
                        //if (deptList.Count == 4)
                        //{
                        //    e.Value = deptList[3].DeptName;
                        //}
                        //if (deptList.Count == 5)
                        //{
                        //    e.Value = deptList[4].DeptName;
                        //}

                        //if (deptList.Count <= 4)
                        //{
                        //    e.Value = dep;
                        //}
                        //else
                        //{
                        //    int index = 4;
                        //    if (index >= 0 && index < deptList.Count)
                        //    {
                        //        e.Value = deptList[4].DeptName;
                        //        dep = deptList[4].DeptName;
                        //    }
                        //}

                    }

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
                    if (e.GetListSourceFieldValue("purchaseOrderDate") != null)
                    {
                        DateTime dateTime = Convert.ToDateTime(e.GetListSourceFieldValue("purchaseOrderDate"));

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
                        var result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalBudgetedMargin) / total) * 100;
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
                        var result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalActualMargin) / total) * 100;
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
                        var result = ((Convert.ToDecimal(SO.totalCFRValue) - Cost.TotalRevisedMargin) / total) * 100;
                        e.Value = result;
                    }
                    else
                    {
                        e.Value = Convert.ToDecimal(e.GetListSourceFieldValue("RevisedMarginPercent"));
                    }
                }
                if (e.Column.FieldName == "InvoicedAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                        var PO = purchaseOrderRepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        var invoicedAmount = Convert.ToDecimal(PO.PurchaseInvoices.Sum(x => x.totalInvoiceAmount));
                        decimal amountWithTax = 0;
                        if (PO.PurchaseInvoices.Count > 0)
                            if (PO.tax != null)
                            {
                                amountWithTax = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(PO.tax.percentage)) / 100);
                            }
                            else
                            {
                                amountWithTax = invoicedAmount;
                            }
                        e.Value = amountWithTax;
                    }
                }

                if (e.Column.FieldName == "UninvoicedAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                        var PO = purchaseOrderRepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        var invoicedAmount = Convert.ToDecimal(PO.PurchaseInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount));
                        decimal amountWithTax = 0;
                        if (PO.PurchaseInvoices.Where(x => x.isVoid != true).ToList().Count > 0)
                        {
                            if (PO.tax != null && PO.billWithTax != null)
                            {
                                invoicedAmount = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(PO.tax.percentage)) / 100);
                                amountWithTax = Convert.ToDecimal(PO.billWithTax.Value) - invoicedAmount;
                            }
                            else
                            {
                                amountWithTax = Convert.ToDecimal(PO.totalCFRValue) - invoicedAmount;
                            }

                        }
                        else
                        {
                            if (PO.tax != null && PO.billWithTax != null)
                            {
                                amountWithTax = Convert.ToDecimal(PO.billWithTax.Value);
                            }
                            else
                            {
                                amountWithTax = Convert.ToDecimal(PO.totalCFRValue);
                            }
                        }
                        e.Value = amountWithTax;
                    }
                }

                if (e.Column.FieldName == "PaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                        var PO = purchaseOrderRepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        decimal amountWithTax = 0;
                        if (PO.PurchaseInvoices.Count > 0)
                            foreach (var _PI in PO.PurchaseInvoices)
                            {
                                amountWithTax = amountWithTax + Convert.ToDecimal(_PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount));
                            }
                        //if (PO.tax != null)
                        //{
                        //    amountWithTax = invoicedAmount + ((invoicedAmount * Convert.ToDecimal(PO.tax.percentage)) / 100);
                        //}
                        //else
                        //{
                        //    amountWithTax = invoicedAmount;
                        //}
                        e.Value = amountWithTax;
                    }
                }

                if (e.Column.FieldName == "UnpaidAmount")
                {
                    if (e.GetListSourceFieldValue("Id") != null)
                    {
                        var id = Convert.ToInt32(e.GetListSourceFieldValue("Id"));
                        PurchaseInvoiceRepo invoiceRepo = new PurchaseInvoiceRepo();
                        var PO = purchaseOrderRepo.getForGrid(id);
                        //var purchaseInvoices = invoiceRepo.GetPurchaseInvoicesByPoId(id);
                        decimal unPaidAmount = 0;
                        decimal amountWithTax = 0;
                        if (PO.PurchaseInvoices.Count > 0)
                            foreach (var _PI in PO.PurchaseInvoices)
                            {
                                amountWithTax = Convert.ToDecimal(amountWithTax + Convert.ToDecimal(_PI.Payments.Where(x => x.isVoid != true).Sum(x => x.DebitedAmount)));
                            }
                        if (PO.billWithTax != null)
                        {
                            unPaidAmount = Convert.ToDecimal(PO.billWithTax) - amountWithTax;
                        }
                        else
                        {
                            unPaidAmount = Convert.ToDecimal(PO.totalCFRValue) - amountWithTax;
                        }
                        e.Value = unPaidAmount;
                    }
                }
                if (e.Column.FieldName == "daysLeft")
                {
                    DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    var PO = grdpurchaseOrder.GetRowByListIndex(e.ListSourceRowIndex) as PurchaseOrder;
                    if (PO.ExpectedPayment != null)
                    {
                        var date = (DateTime)PO.ExpectedPayment;
                        DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                        var timeSpan = currDate.Subtract(targetDate);


                        e.Value = System.Math.Abs(timeSpan.Days);
                    }
                }
                //CreationDate


                //int unitsOnOrder = Convert.ToInt32(e.GetListSourceFieldValue("UnitsOnOrder"));
                //e.Value = price * unitsOnOrder;
            }
        }
        private void GrdpurchaseOrder_MouseEnter(object sender, MouseEventArgs e)
        {
            if (tableView.CompactPanelShowMode == CompactPanelShowMode.CompactMode)
            {
                tableView.CompactPanelShowMode = CompactPanelShowMode.CompactMode;
                tableView.CompactPanelShowMode = CompactPanelShowMode.Always;
            }
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
