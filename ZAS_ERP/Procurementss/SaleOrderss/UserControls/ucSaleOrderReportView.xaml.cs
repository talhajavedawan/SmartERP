using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
using ERP_BL.Reports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZAS_ERP.Procurementss.SharedReports;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.SaleOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucSaleOrderReportView.xaml
    /// </summary>
    public partial class ucSaleOrderReportView : DXWindow
    {
        GridReport report = new GridReport();
        string reportTitle;
        public MainWindow myParent = null;
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        static List<ExchangeRateGroup> exchangeRateGroupsSER = new List<ExchangeRateGroup>();
        static List<ExchangeRateGroup> exchangeRateGroupsMER = new List<ExchangeRateGroup>();
        ExchangeRate exchangeRateSER = null;
        ExchangeRate exchangeRateMER = null;
        ExchangeRate exchangeRateCMER = null;
        public ucSaleOrderReportView()
        {


            InitializeComponent();
        }
        public ucSaleOrderReportView(GridReport reportToEdit, string title)
        {

            InitializeComponent();
           

            report = reportToEdit;
            reportTitle = title;
        }

        public string ReportHeader { get; private set; }
        private void GrdsaleOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSaleOrder();
        }

        private void EditSaleOrder()
        {
            var selectedItem = grdsaleOrderReport.SelectedItem as SaleOrder;
            if (selectedItem != null)
            {

                Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, selectedItem.Id);
                procurmentPanel.Show();

            }
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

            GridReport report = repo.GetReportByName(reportTitlePath);
            SaleOrderRepo soRepo = new SaleOrderRepo();
            if (report.settingkey == "Sale Register")
            { grdsaleOrderReport.ItemsSource = soRepo.getSaleRegister(SYSTEM_STATIC.currentUser.id); }
            else
            { grdsaleOrderReport.ItemsSource = soRepo.getAll(SYSTEM_STATIC.currentUser.id); }
            loadingGif.Visibility = Visibility.Hidden;

        }
        public void LoadSERGroups()
        {
            exchangeRateGroupsSER = exchangeRateGroupRepo.GetAllSER();
        }
        public void LoadMERGroups()
        {
            exchangeRateGroupsMER = exchangeRateGroupRepo.GetAllMER();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbxDepartmentFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));
            cmbxDepartmentTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.DepartmentLevels)));

            cmbxCustomerFrom.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.CustomerLevels)));
            cmbxCustomerTo.Items.AddRange(Enum.GetNames(typeof(ERP_BL.Enums.CustomerLevels)));

            Loader.DeferedVisibility = true;
            SaleOrderRepo soRepo = new SaleOrderRepo();
            GridReportRepo repo = new GridReportRepo();
            var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));

            LoadSERGroups();
            LoadMERGroups();

           // grdDepartments.ItemsSource = SYSTEM_STATIC.currentUser.employee.departments;

            switch (report.gridReportType)
            {
                case GridReportType.StandardReport:
                    {
                        if (report != null)
                        {
                            //Permissions Should be here 
                            mbtnExportToStandardReport1.IsVisible = false;
                            mbtnShareReport.IsVisible = false;
                            grdsaleOrderReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "SaleOrders" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "SaleOrders" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Sale Register")
                            {
                                grdsaleOrderReport.ItemsSource = soRepo.getSaleRegister(SYSTEM_STATIC.currentUser.id);
                            }
                            else
                                grdsaleOrderReport.ItemsSource = soRepo.getAll(SYSTEM_STATIC.currentUser.id);
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
                            grdsaleOrderReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
                            Title = "SaleOrders" + "/" + group.groupName + "/" + reportTitle;
                            lblHeading.Caption = "SaleOrders" + "/" + group.groupName + "/" + reportTitle;
                            if (report.settingkey == "Sale Register")
                            {
                                grdsaleOrderReport.ItemsSource = soRepo.getSaleRegister(SYSTEM_STATIC.currentUser.id);
                            }
                            else
                                grdsaleOrderReport.ItemsSource = soRepo.getAll(SYSTEM_STATIC.currentUser.id);
                        }

                        break;
                    }
            }
            Loader.DeferedVisibility = false;

        }
        private void BtnPrintPreview_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            DataTemplate dataTemplate = new DataTemplate(report.reportName);
            //dataTemplate.Template = new TextBlock();
            PrintableControlLink link = new PrintableControlLink((TableView)grdsaleOrderReport.View);
            link.PaperKind = System.Drawing.Printing.PaperKind.A2;
            link.PageHeaderData = report.reportName;
            link.PageHeaderTemplate = Resources["reportHeaderTemplate"] as DataTemplate;
            link.DocumentName = report.reportName;
            link.ReportHeaderData = report.reportName;
            link.ReportHeaderTemplate = dataTemplate;
            link.Landscape = true;
            // Show a preview. 
            DevExpress.Xpf.Printing.PrintHelper.ShowRibbonPrintPreview(this, link);
            //grdsaleOrderReport.View.ShowPrintPreview(this);
        }


        private void grid_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            try
            {


                if (e.IsGetData)

                    if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                    {
                        var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        if (row != null)
                        {
                            var deptList = new List<DepartmentLevel>();
                            var node = row.department.departmentLevel;
                            while (node != null)
                            {
                                if (node.ParentID != null)
                                {
                                    if (node.ParentID != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        deptList.Add(node);
                                        node = node.parentLevel;
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
                            e.Value = deptList[0].Title;
                        }
                    }
                if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var deptList = new List<DepartmentLevel>();
                        var node = row.department.departmentLevel;
                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and transverse to its parent
                                    deptList.Add(node);
                                    node = node.parentLevel;
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
                                e.Value = deptList[0].Title;
                                break;
                            case 2:
                                e.Value = deptList[1].Title;
                                break;
                            case 3:
                                e.Value = deptList[1].Title;
                                break;
                            case 4:
                                e.Value = deptList[1].Title;
                                break;
                            case 5:
                                e.Value = deptList[1].Title;
                                break;
                            case 6:
                                e.Value = deptList[1].Title;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var deptList = new List<DepartmentLevel>();
                        var node = row.department.departmentLevel;
                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and transverse to its parent
                                    deptList.Add(node);
                                    node = node.parentLevel;
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
                                e.Value = deptList[0].Title;
                                break;
                            case 2:
                                e.Value = deptList[1].Title;
                                break;
                            case 3:
                                e.Value = deptList[2].Title;
                                break;
                            case 4:
                                e.Value = deptList[2].Title;
                                break;
                            case 5:
                                e.Value = deptList[2].Title;
                                break;
                            case 6:
                                e.Value = deptList[2].Title;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var deptList = new List<DepartmentLevel>();
                        var node = row.department.departmentLevel;

                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and tranverse to its parent
                                    deptList.Add(node);
                                    node = node.parentLevel;
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
                                e.Value = deptList[0].Title;
                                break;
                            case 2:
                                e.Value = deptList[1].Title;
                                break;
                            case 3:
                                e.Value = deptList[2].Title;
                                break;
                            case 4:
                                e.Value = deptList[3].Title;
                                break;
                            case 5:
                                e.Value = deptList[3].Title;
                                break;
                            case 6:
                                e.Value = deptList[3].Title;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var deptList = new List<DepartmentLevel>();
                        var node = row.department.departmentLevel;

                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and tranverse to its parent
                                    deptList.Add(node);
                                    node = node.parentLevel;
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
                                e.Value = deptList[0].Title;
                                break;
                            case 2:
                                e.Value = deptList[1].Title;
                                break;
                            case 3:
                                e.Value = deptList[2].Title;
                                break;
                            case 4:
                                e.Value = deptList[3].Title;
                                break;
                            case 5:
                                e.Value = deptList[4].Title;
                                break;
                            case 6:
                                e.Value = deptList[4].Title;
                                break;
                        }
                    }
                }

                if (e.Column.FieldName == "departmentLevel6" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var deptList = new List<DepartmentLevel>();
                        var node = row.department.departmentLevel;

                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and tranverse to its parent
                                    deptList.Add(node);
                                    node = node.parentLevel;
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
                                e.Value = deptList[0].Title;
                                break;
                            case 2:
                                e.Value = deptList[1].Title;
                                break;
                            case 3:
                                e.Value = deptList[2].Title;
                                break;
                            case 4:
                                e.Value = deptList[3].Title;
                                break;
                            case 5:
                                e.Value = deptList[4].Title;
                                break;
                            case 6:
                                e.Value = deptList[5].Title;
                                break;
                        }
                    }
                }


                if (e.Column.FieldName == "customerLevel1" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.customerCompany;
                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and transverse to its parent
                                    customerList.Add(node);
                                    node = node.parentCompany;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    customerList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
                                customerList.Add(node);
                                break;
                            }

                        }
                        customerList.Reverse();
                        e.Value = customerList[0].company.CompanyName;
                    }
                }
                if (e.Column.FieldName == "customerLevel2" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.customerCompany;
                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and transverse to its parent
                                    customerList.Add(node);
                                    node = node.parentCompany;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    customerList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
                                customerList.Add(node);
                                break;
                            }
                        }
                        customerList.Reverse();
                        switch (customerList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = customerList[0].company.CompanyName;
                                break;
                            case 2:
                                e.Value = customerList[1].company.CompanyName;
                                break;
                            case 3:
                                e.Value = customerList[1].company.CompanyName;
                                break;
                            case 4:
                                e.Value = customerList[1].company.CompanyName;
                                break;
                            case 5:
                                e.Value = customerList[1].company.CompanyName;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "customerLevel3" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.customerCompany;
                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and transverse to its parent
                                    customerList.Add(node);
                                    node = node.parentCompany;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    customerList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
                                customerList.Add(node);
                                break;
                            }
                        }
                        customerList.Reverse();
                        switch (customerList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = customerList[0].company.CompanyName;
                                break;
                            case 2:
                                e.Value = customerList[1].company.CompanyName;
                                break;
                            case 3:
                                e.Value = customerList[2].company.CompanyName;
                                break;
                            case 4:
                                e.Value = customerList[2].company.CompanyName;
                                break;
                            case 5:
                                e.Value = customerList[2].company.CompanyName;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "customerLevel4" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.customerCompany;

                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and tranverse to its parent
                                    customerList.Add(node);
                                    node = node.parentCompany;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    customerList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
                                customerList.Add(node);
                                break;
                            }

                        }
                        customerList.Reverse();
                        switch (customerList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = customerList[0].company.CompanyName;
                                break;
                            case 2:
                                e.Value = customerList[1].company.CompanyName;
                                break;
                            case 3:
                                e.Value = customerList[2].company.CompanyName;
                                break;
                            case 4:
                                e.Value = customerList[3].company.CompanyName;
                                break;
                            case 5:
                                e.Value = customerList[3].company.CompanyName;
                                break;
                        }
                    }
                }
                if (e.Column.FieldName == "customerLevel5" && e.IsGetData)
                {
                    var row = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.customerCompany;

                        while (node != null)
                        {
                            if (node.ParentID != null)
                            {
                                if (node.ParentID != node.Id)
                                {
                                    //this will add current node to department list and tranverse to its parent
                                    customerList.Add(node);
                                    node = node.parentCompany;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    customerList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
                                customerList.Add(node);
                                break;
                            }
                        }
                        customerList.Reverse();

                        switch (customerList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = customerList[0].company.CompanyName;
                                break;
                            case 2:
                                e.Value = customerList[1].company.CompanyName;
                                break;
                            case 3:
                                e.Value = customerList[2].company.CompanyName;
                                break;
                            case 4:
                                e.Value = customerList[3].company.CompanyName;
                                break;
                            case 5:
                                e.Value = customerList[4].company.CompanyName;
                                break;
                        }
                    }
                }









                if (e.Column.FieldName == "Stage" && e.IsGetData)
                {
                    var saleOrder = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                    if (saleOrder.isVoid == true)
                    {
                        e.Value = "Void";
                    }
                    else if (saleOrder.isReApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (saleOrder.isApproved == true && saleOrder.stage == "Closed")
                    {
                        e.Value = "Closed";
                    }
                    else if (saleOrder.isApproved == true && saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing != true)
                    {
                        e.Value = "Closed";
                    }
                    else if (saleOrder.isApproved == true && saleOrder.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                    else if (saleOrder.isApproved == true)
                    {
                        e.Value = "Approved";
                    }
                    else if (saleOrder.isApproved == false)
                    {
                        e.Value = "Under Approval";
                    }
                    else if (saleOrder.PendingForClosing == true)
                    {
                        e.Value = "Under Closing";
                    }
                }

                switch (e.Column.FieldName)
                {
                    case "Vendorss":
                        if (e.Column.FieldName == "Vendorss" && e.IsGetData)
                        {
                            var _SO = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                            var selectedRow66 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

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
                        var selectedRow = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                        var selectedRow1 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var budgetCostMER = Convert.ToDouble(e.GetListSourceFieldValue("BudgetMarginOC"));

                        if (selectedRow1.saleOrdertype == InquiryType.SupplyCCC)
                        {

                            e.Value = budgetCostMER * selectedRow1.ccMER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(budgetCostMER) * Convert.ToDouble(selectedRow1.exchangeRate);
                        }

                        break;
                    case "RevisedMarginSER":
                        var selectedRow2 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                        var selectedRow3 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var revisedCostMER = Convert.ToDouble(e.GetListSourceFieldValue("RevisedMarginOC"));

                        if (selectedRow3.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = revisedCostMER * selectedRow3.ccMER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(revisedCostMER) * Convert.ToDouble(selectedRow3.exchangeRate);
                        }

                        break;
                    case "actualMarginSER":
                        var selectedRow4 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                        var selectedRow5 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var actualMarginMER = Convert.ToDouble(e.GetListSourceFieldValue("ActualMarginOC"));

                        if (selectedRow5.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = actualMarginMER * selectedRow5.ccMER;
                        }
                        else
                        {
                            e.Value = Convert.ToDouble(actualMarginMER) * Convert.ToDouble(selectedRow5.exchangeRate);
                        }

                        break;
                    case "SalesSystemMargin1":
                        var selectedRow6 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                        var selectedRow7 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var systemMarginMER = Convert.ToDouble(e.GetListSourceFieldValue("SystemMargin"));

                        if (selectedRow7.saleOrdertype == InquiryType.SupplyCCC)
                        {
                            e.Value = systemMarginMER * selectedRow7.ccMER;
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
                            var selectedRow66 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

                            if (total != 0)
                            {
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
                            var selectedRow66 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

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
                            var selectedRow66 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

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
                            var selectedRow66 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

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
                            var selectedRow66 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;

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
                        var saleOrder = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        e.Value = saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount);
                        break;
                    case "recieptTotalOC":
                        var saleOrder1 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        e.Value = saleOrder1.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                        break;
                    case "totalDeductionsOC":
                        var saleOrder2 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        e.Value = saleOrder2.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.receiptDeductions.Sum(y => y.Amount)));
                        break;
                    case "totalCreditedOC":
                        var saleOrder3 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        var result = saleOrder3.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.receiptDeductions.Sum(y => y.Amount)));
                        var collection = saleOrder3.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                        e.Value = collection - result;
                        break;
                    case "remainingCollectionOC":
                        var saleOrder4 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                        var saleOrder5 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                            if (Cost != null && Cost.FieldValues != null)
                            {
                                var fields = Cost.FieldValues.Where(x => x.Type == 1).ToList();
                                decimal totalFieldValue = fields.Sum(x => x.Value);
                                var result3 = totalFieldValue /** exchangerate*/;
                                e.Value = Math.Round(result3, 2);
                            }
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
                            var saleOrderSER = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                            var saleOrderMER = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                            var SOCMER = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                        var order = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                        var order1 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                        var order2 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
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
                        var order3 = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        if (order3.PerformanceSheet_Id != null)
                        {
                            e.Value = order3.PerformanceSheet.performanceSheetFields.Sum(x => x.PerformanceSheetHead.totalPoints);
                        }
                        break;
                    case "remainingInvoiced":
                        var so = grdsaleOrderReport.GetRowByListIndex(e.ListSourceRowIndex) as SaleOrder;
                        if (so.SaleInvoices != null)
                        {
                            if (so.SaleInvoices.Count != 0)
                            {
                                double totalSOAmount = 0;
                                var totalInvoiced = so.SaleInvoices.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount);

                                if (so.saleOrdertype == InquiryType.Principal)
                                {
                                    totalSOAmount = Convert.ToDouble(e.GetListSourceFieldValue("commision"));

                                }
                                else
                                {
                                    totalSOAmount = so.totalCFRValue;
                                }
                                var value = Math.Round(totalSOAmount - totalInvoiced, 2);
                                e.Value = value;
                            }
                            else
                            if (so.saleOrdertype == InquiryType.Principal)
                            {
                                e.Value = Convert.ToDouble(e.GetListSourceFieldValue("commision"));

                            }
                            else
                            {
                                e.Value = so.totalCFRValue;
                            }
                        }
                        else
                        if (so.saleOrdertype == InquiryType.Principal)
                        {
                            e.Value = Convert.ToDouble(e.GetListSourceFieldValue("commision"));

                        }
                        else
                        {
                            e.Value = so.totalCFRValue;
                        }
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void GrdsaleOrder_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (tableView.CompactPanelShowMode == CompactPanelShowMode.CompactMode)
            //{
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.CompactMode;
            //    tableView.CompactPanelShowMode = CompactPanelShowMode.Always;
            //}
        }

        private void mbtnShareReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Share Sale Orders Report") != null)
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
                DXMessageBox.Show("You don't have permission Share Sale Orders Report!!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Information);
                return;

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

        private void mbtnExportToStandardReport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
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
                                    ReportLogic.ExportToStandard(grdsaleOrderReport, reportName, reportType, reportGroup, report.settingkey);
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

        private void MbtnExportToMemorizedReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
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
                                    ReportLogic.SaveGridReport(grdsaleOrderReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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

        private void MbtnRefreshReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Do you want to reload the layout?" + this.Title + "report" + " ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (inputfromUser == System.Windows.MessageBoxResult.Yes)
            {
                GridReportRepo repo = new GridReportRepo();
                report = repo.GetReportById(report.Id);

                grdsaleOrderReport.RestoreLayoutFromStream(ReportLogic.ConvertToMemoryStream(report.settingValue));
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

        private void MbtnDeleteReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
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
                                var group = repo.GetReportGroupByGroupId(Convert.ToInt32(report.group_Id));
                                ReportLogic.DeleteReport(grdsaleOrderReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

                                ReportLogic.DeleteReport(grdsaleOrderReport, report.reportName, report.gridReportType, group, report.Id, report.settingkey);
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

        private void MbtnUpdateReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
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
                                    ReportLogic.UpdateGridReport(grdsaleOrderReport, str, reportType, groupDetails, report.Id, report.settingkey);

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
                                    ReportLogic.UpdateGridReport(grdsaleOrderReport, str, reportType, groupDetails, report.Id, report.settingkey);

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

        private void MbtnRenameReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
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
                                        ReportLogic.RenameGridReport(grdsaleOrderReport, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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
                                        ReportLogic.RenameGridReport(grdsaleOrderReport, updatedReport.reportName, updatedReport.gridReportType, updatedReport.gridReportGroup, updatedReport.Id, report.settingkey, report.titleId);
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

        private void MbtnSaveAsNew_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
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
                                    ReportLogic.SaveGridReport(grdsaleOrderReport, reportName, reportType, reportGroup, report.settingkey, report.titleId);
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
                                    ReportLogic.SaveGridReport(grdsaleOrderReport, reportName, reportType, reportGroup, report.settingkey,report.titleId);
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

        private void mbtnApprove_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {


                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null) ? true : false)
                {
                    if (grdsaleOrderReport.GetFocusedRow() != null)
                    {
                        SaleOrder saleOrder = new SaleOrder();
                        //Inquiriess.ucStatuschange.saleOrderid = (int)grdsaleOrderReport.GetFocusedRowCellValue(grdsaleOrderReport.Columns.GetColumnByFieldName("Id"));
                        saleOrder = grdsaleOrderReport.SelectedItem as SaleOrder;

                        UsersRepo usersRepo = new UsersRepo();

                        if (saleOrder.isApproved != true)
                            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added SaleOrder") != null) ? true : false)
                            {
                                saleOrder.isApproved = true;
                                saleOrder.stage = TransactionStage.Approved.ToString();
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, saleOrder.Id, 3, frmInputBox.comment);
                                //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                            {
                                saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                if (saleOrder.isApproved == null)
                                {
                                    saleOrder.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, saleOrder.Id, 3, frmInputBox.comment);
                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                            {
                                saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                if (saleOrder.isApproved == null)
                                {
                                    saleOrder.isApproved = false;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, saleOrder.Id, 3, frmInputBox.comment);
                            }
                            else
                            {
                                saleOrder.isApproved = false;
                            }
                        else //reapprove when it is approved already
                        {
                            if (saleOrder.isReApproved == false)
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order without ReApproval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for new added SaleOrder") != null) ? true : false)
                                {
                                    saleOrder.isReApproved = true;
                                    saleOrder.stage = TransactionStage.Approved.ToString();
                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Adding, saleOrder.Id, 3, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.offerRepo.update(Inquiriess.ucStatuschange.offer);

                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                {
                                    saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                    if (saleOrder.isReApproved == null)
                                    {
                                        saleOrder.isReApproved = false;

                                    }

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, saleOrder.Id, 3, frmInputBox.comment);
                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                                {
                                    saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (saleOrder.isReApproved == null)
                                    {
                                        saleOrder.isReApproved = false;

                                    }

                                    frmInputBox inputBox = new frmInputBox();
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, saleOrder.Id, 3, frmInputBox.comment);
                                }
                                else
                                {
                                    saleOrder.isReApproved = false;
                                }

                        }


                        saleOrderRepo.update(saleOrder);
                        MessageBox.Show("SaleOrder is Approved (" + saleOrder.Id + ")");
                        SystemLog.LogInfo(this.GetType(), "SaleOrder is Approved (" + saleOrder.Id + ")");
                    }
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Approve SaleOrder Directly");
                    SystemLog.LogInfo(this.GetType(), "You are not Allowed to Approve SaleOrder Directly user id=(" + MainWindow.currentUserid + ")");

                }
            }
            catch (Exception)
            {

                throw;
            }
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
        private void mbtnDriectClose_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {


                SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
                ProcurementRepo procurementRepo = new ProcurementRepo();
                NotificationsRepo notificationsRepo = new NotificationsRepo();
                SaleOrderStatus oldStatus = new SaleOrderStatus();

                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null) ? true : false)
                {

                    if (grdsaleOrderReport.GetFocusedRow() != null)
                    {

                        UsersRepo usersRepo = new UsersRepo();
                        var row = grdsaleOrderReport.GetFocusedRow() as SaleOrder;

                        var total = row.totalCFRValue; //Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                        var result = Convert.ToDouble(row.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                        var remainingCollection = total - result;
                        remainingCollection = Math.Round(remainingCollection, 2);
                        if (row.saleOrdertype == InquiryType.Principal)
                        {
                            var invoicedAmount = row.SaleInvoices?.Where(x => x.isVoid != true).Sum(z => z.totalInvoiceAmount);
                            var collected = row.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                            remainingCollection = Convert.ToDouble(invoicedAmount) - Convert.ToDouble(collected);
                            remainingCollection = Math.Round(remainingCollection, 2);
                        }
                        if (remainingCollection != 0)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without receiving fully Collection") != null)
                            {
                                var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to close sale order without complete collection?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                                {
                                    decimal budgetMarginOC = 0;
                                    decimal actualMarginOC = 0;

                                    decimal totalCFRValue = 0;
                                    decimal TotalBudgetedMargin = 0;
                                    decimal margin = 0;

                                    decimal TotalActualMargin = 0;
                                    decimal ActualMargin = 0;

                                    if (row.totalCFRValue != 0)
                                    {
                                        totalCFRValue = Convert.ToDecimal(row.totalCFRValue);
                                    }
                                    if (row.margin != 0)
                                    {
                                        margin = Convert.ToDecimal(row.margin);
                                    }
                                    if (row.ActualMargin != 0)
                                    {
                                        ActualMargin = Convert.ToDecimal(row.ActualMargin);
                                    }
                                    if (row.CostSheet != null)
                                    {

                                        TotalBudgetedMargin = row.CostSheet.TotalBudgetedMargin;
                                        TotalActualMargin = row.CostSheet.TotalActualMargin;
                                    }

                                    if (row.CostSheet != null)
                                    {

                                        budgetMarginOC = totalCFRValue - TotalBudgetedMargin;

                                    }
                                    else
                                    {
                                        budgetMarginOC = margin;
                                    }



                                    if (row.CostSheet != null)
                                    {

                                        actualMarginOC = totalCFRValue - TotalActualMargin;

                                    }
                                    else
                                    {
                                        actualMarginOC = ActualMargin;
                                    }



                                    if (row.saleOrderStatus != null)
                                    {
                                        oldStatus = row.saleOrderStatus;
                                    }
                                    SaleOrderss.ucStatuschange.inActiveStatuses = 1;

                                    SaleOrderss.ucStatuschange.saleOrderid = (int)grdsaleOrderReport.GetFocusedRowCellValue(grdsaleOrderReport.Columns.GetColumnByFieldName("Id"));
                                    SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange(saleOrderRepo);
                                    var myWindow = Window.GetWindow(this);
                                    statusChange.Owner = myWindow;
                                    statusChange.ShowDialog();
                                    //if (SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Id == row.saleOrderStatus.Id)
                                    //    return;
                                    if (SaleOrderss.ucStatuschange.saleOrder.Id != 0)
                                        if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null) ? true : false)
                                        {
                                            SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = false;
                                            SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.Approved.ToString();

                                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                            inputBox.ShowDialog();
                                            usersRepo.Add(TransactionInfo.Approved_Closing, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                            //Inquiriess.ucStatuschange.saleOrderRepo.update(Inquiriess.ucStatuschange.saleOrder);

                                        }
                                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                        {
                                            SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                            if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing == null)
                                            {
                                                SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                            }
                                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                            inputBox.ShowDialog();
                                            usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                        }
                                        else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                                        {
                                            SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                            if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing != true)
                                            {
                                                SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                            }
                                            //SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                            inputBox.ShowDialog();
                                            usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                        }
                                        else
                                        {
                                            SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                            SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;
                                            usersRepo.Add(TransactionInfo.Closed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                        }
                                    //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                                    SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                                    SaleOrderss.ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                                    if (row.saleOrderStatus != SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus)
                                        usersRepo.Add(TransactionInfo.Status_Changed, row.Id, (int)TransactionItemType.Sale_Order, "While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                    //Adding auto Signature
                                    //if (row.saleOrderStatus.Status != saleOrder.saleOrderStatus.Id)
                                    //{
                                    UsersRepo userRepo = new UsersRepo();
                                    //Asking for Tag
                                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                                    var res = MessageBox.Show("SO has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                                    if (res == MessageBoxResult.Yes)
                                    {
                                        if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                        {
                                            winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Sale_Order);
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
                                    string newStat = SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status;
                                    string symbolCurr = "";
                                    if (row.currency != null)
                                    {
                                        symbolCurr = row.currency.Abbrivation.ToString();
                                    }
                                    //CommentLog comment = new CommentLog()
                                    //{
                                    //    Comment = "Status of SO having value: " + row.totalFOBValue.ToString()  +"(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                    //    Timestamp = DateTime.Now,
                                    //    Subject = "Status Changed from Direct Close"
                                    //};

                                    CommentLog comment = new CommentLog()
                                    {
                                        Comment = "Status of SO having SO Amount (OC): " + row.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                            + "Budget Margin(OC): " + budgetMarginOC.ToString() + " (" + symbolCurr + ")"
                                            + "\nActal Margin(OC): " + actualMarginOC.ToString() + " (" + symbolCurr + ")"
                                            + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                        Timestamp = DateTime.Now,
                                        Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                                    };
                                    procurementRepo.Add(row.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);

                                    //Creating notification
                                    if (tagUsers.Count != 0)
                                    {
                                        foreach (var user in tagUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                        }
                                    }

                                    if (ccUsers.Count != 0)
                                    {
                                        foreach (var user in ccUsers)
                                        {
                                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);
                                        }
                                    }


                                    //}

                                    //SaleOrderss.ucStatuschange.Updatestatus();
                                    try
                                    {
                                        row = SaleOrderss.ucStatuschange.saleOrder;
                                        grdsaleOrderReport.RefreshData();
                                        saleOrderRepo.updateStatusById(row.Id, row.saleOrderStatus);
                                    }
                                    catch { }

                                    //SaleOrderss.ucStatuschange.UpdateSaleOrder();//saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                                    MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                }
                                else
                                    return;
                            }
                            else
                                DXMessageBox.Show("Collection is not fully received yet, or you need permission to Close Sale Order without receiving fully Collection", "Permission Denied", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        else
                        {
                            decimal budgetMarginOC = 0;
                            decimal actualMarginOC = 0;

                            decimal totalCFRValue = 0;
                            decimal TotalBudgetedMargin = 0;
                            decimal margin = 0;

                            decimal TotalActualMargin = 0;
                            decimal ActualMargin = 0;

                            if (row.totalCFRValue != 0)
                            {
                                totalCFRValue = Convert.ToDecimal(row.totalCFRValue);
                            }
                            if (row.margin != 0)
                            {
                                margin = Convert.ToDecimal(row.margin);
                            }
                            if (row.ActualMargin != 0)
                            {
                                ActualMargin = Convert.ToDecimal(row.ActualMargin);
                            }
                            if (row.CostSheet != null)
                            {

                                TotalBudgetedMargin = row.CostSheet.TotalBudgetedMargin;
                                TotalActualMargin = row.CostSheet.TotalActualMargin;
                            }

                            if (row.CostSheet != null)
                            {

                                budgetMarginOC = totalCFRValue - TotalBudgetedMargin;

                            }
                            else
                            {
                                budgetMarginOC = margin;
                            }



                            if (row.CostSheet != null)
                            {

                                actualMarginOC = totalCFRValue - TotalActualMargin;

                            }
                            else
                            {
                                actualMarginOC = ActualMargin;
                            }



                            if (row.saleOrderStatus != null)
                            {
                                oldStatus = row.saleOrderStatus;
                            }
                            SaleOrderss.ucStatuschange.inActiveStatuses = 1;

                            SaleOrderss.ucStatuschange.saleOrderid = (int)grdsaleOrderReport.GetFocusedRowCellValue(grdsaleOrderReport.Columns.GetColumnByFieldName("Id"));
                            SaleOrderss.frmSaleOrderStatusChange statusChange = new SaleOrderss.frmSaleOrderStatusChange(saleOrderRepo);
                            var myWindow = Window.GetWindow(this);
                            statusChange.Owner = myWindow;
                            statusChange.ShowDialog();
                            //if (SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Id == row.saleOrderStatus.Id)
                            //    return;
                            if (SaleOrderss.ucStatuschange.saleOrder.Id != 0)
                                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close Sale Order without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing SaleOrder") != null) ? true : false)
                                {
                                    SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = false;
                                    SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.Approved.ToString();

                                    frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Approved_Closing, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                    //Inquiriess.ucStatuschange.saleOrderRepo.update(Inquiriess.ucStatuschange.saleOrder);

                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 SaleOrder") != null)
                                {
                                    SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingApproval.ToString();
                                    if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing == null)
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                    }
                                    frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                }
                                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 SaleOrder") != null)
                                {
                                    SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingSecondReview.ToString();
                                    if (SaleOrderss.ucStatuschange.saleOrder.PendingForClosing != true)
                                    {
                                        SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                    }
                                    //SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;

                                    frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                                    inputBox.ShowDialog();
                                    usersRepo.Add(TransactionInfo.Reviewed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                }
                                else
                                {
                                    SaleOrderss.ucStatuschange.saleOrder.stage = TransactionStage.AwaitingFirstReview.ToString();

                                    SaleOrderss.ucStatuschange.saleOrder.PendingForClosing = true;
                                    usersRepo.Add(TransactionInfo.Closed, SaleOrderss.ucStatuschange.saleOrder.Id, 3, frmInputBox.comment);
                                }
                            //SaleOrderss.ucStatuschange.saleOrder.user_Id = MainWindow.currentUserid;
                            SaleOrderss.ucStatuschange.saleOrder.LastStatusChangeDate = System.DateTime.Now;
                            SaleOrderss.ucStatuschange.saleOrder.ClosingDate = System.DateTime.Now;
                            if (row.saleOrderStatus != SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus)
                                usersRepo.Add(TransactionInfo.Status_Changed, row.Id, (int)TransactionItemType.Sale_Order, "While direct closing Status Changed from (" + row.saleOrderStatus.Status + ") to (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                            //Adding auto Signature
                            //if (row.saleOrderStatus.Status != saleOrder.saleOrderStatus.Id)
                            //{
                            UsersRepo userRepo = new UsersRepo();
                            //Asking for Tag
                            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                            var res = MessageBox.Show("SO has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                if (row.department != null && row.department.Id != 0 && row.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                                {
                                    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(row.department.Id, row.company.Id), row.Id, TransactionItemType.Sale_Order);
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
                            string newStat = SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status;
                            string symbolCurr = "";
                            if (row.currency != null)
                            {
                                symbolCurr = row.currency.Abbrivation.ToString();
                            }
                            //CommentLog comment = new CommentLog()
                            //{
                            //    Comment = "Status of SO having value: " + row.totalFOBValue.ToString()  +"(" + symbolCurr + ") " + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                            //    Timestamp = DateTime.Now,
                            //    Subject = "Status Changed from Direct Close"
                            //};

                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of SO having SO Amount (OC): " + row.totalCFRValue.ToString() + " (" + symbolCurr + ")\n "
                                    + "Budget Margin(OC): " + budgetMarginOC.ToString() + " (" + symbolCurr + ")"
                                    + "\nActal Margin(OC): " + actualMarginOC.ToString() + " (" + symbolCurr + ")"
                                    + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed using Direct Close",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };
                            procurementRepo.Add(row.Id, TransactionItemType.Sale_Order, comment, SYSTEM_STATIC.currentUser.employeeId);

                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in SO #" + row.SalesReferenceNo, row.Id, TransactionItemType.Sale_Order, comment.Comment, user.id, "New Comment ", null);
                                }
                            }


                            //}

                            //SaleOrderss.ucStatuschange.Updatestatus();
                            try
                            {
                                row = SaleOrderss.ucStatuschange.saleOrder;
                                grdsaleOrderReport.RefreshData();
                                saleOrderRepo.updateStatusById(row.Id, row.saleOrderStatus);
                            }
                            catch { }

                            //SaleOrderss.ucStatuschange.UpdateSaleOrder();//saleOrderRepo.update(SaleOrderss.ucStatuschange.saleOrder);
                            MessageBox.Show("SaleOrder status changed to InActive (" + SaleOrderss.ucStatuschange.saleOrder.saleOrderStatus.Status + ")");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You are not Allowed to Close SaleOrder Directly.");
                }
            }
            catch (Exception)
            {


            }
        }

        List<Department> deptList = new List<Department>();
        private void PrintRecursive(TreeListNode treeNode)
        {
            // Print the node.  
            //if (treeNode.IsChecked == true)
            //{
            //    var row = grdDepartments.GetRow(treeNode.RowHandle) as Department;
            //    deptList.Add(row);
            //}


            //// Visit each node recursively.  
            //foreach (TreeListNode tn in treeNode.Nodes)
            //{
            //    PrintRecursive(tn);
            //}
        }

        // Call the procedure using the TreeView.  
        private void CallRecursive(TreeListView treeView)
        {
            // Print each node recursively.  
            foreach (TreeListNode n in treeView.Nodes)
            {
                //recursiveTotalNodes++;
                PrintRecursive(n);
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            //var treeView = grdDepartments.View as TreeListView;
            //CallRecursive(treeView);
            //var deptIds = deptList.Select(x=>x.Id).ToList();
            //if (report.settingkey == "Sale Register")
            //{ 
            //    grdsaleOrderReport.ItemsSource = saleOrderRepo.getSaleRegisterForDepartments(SYSTEM_STATIC.currentUser.id, deptIds);
            //}
            //else
            //{ 
            //    grdsaleOrderReport.ItemsSource = saleOrderRepo.getAllForDepartments(SYSTEM_STATIC.currentUser.id, deptIds);
            //}
        }

        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
           // treeListDepartments.CheckAllNodes();
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            //treeListDepartments.UncheckAllNodes();
        }

        double soAmountSE = 0, soAmountME = 0;
        double bmGrossProfitSE = 0, bmGrossProfitME = 0; // Example for an additional column
        double smGrossProfitSE = 0, smGrossProfitME = 0;

        private void cmbxCustomerFrom_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbxCustomerTo.SelectedIndex = -1;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cmbxCustomerTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxCustomerFrom.SelectedIndex > cmbxCustomerTo.SelectedIndex)
                {
                    cmbxCustomerTo.SelectedIndex = -1;
                    DXMessageBox.Show("Please select greater Customer!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnCustomerFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbxCustomerFrom.SelectedIndex > -1 && cmbxCustomerTo.SelectedIndex > -1)
                {
                    switch (cmbxCustomerFrom.SelectedIndex)
                    {
                        case 0:
                            switch (cmbxCustomerTo.SelectedIndex)
                            {
                                case 0:
                                    colCustLevel1.Visible = true;
                                    colCustLevel2.Visible = false;
                                    colCustLevel3.Visible = false;
                                    colCustLevel4.Visible = false;
                                    colCustLevel5.Visible = false;
                                    break;
                                case 1:
                                    colCustLevel1.Visible = true;
                                    colCustLevel2.Visible = true;
                                    colCustLevel3.Visible = false;
                                    colCustLevel4.Visible = false;
                                    colCustLevel5.Visible = false;
                                    break;
                                case 2:
                                    colCustLevel1.Visible = true;
                                    colCustLevel2.Visible = true;
                                    colCustLevel3.Visible = true;

                                    colCustLevel4.Visible = false;
                                    colCustLevel5.Visible = false;
                                    break;
                                case 3:
                                    colCustLevel1.Visible = true;
                                    colCustLevel2.Visible = true;
                                    colCustLevel3.Visible = true;
                                    colCustLevel4.Visible = true;

                                    colCustLevel5.Visible = false;
                                    break;
                                case 4:
                                    colCustLevel1.Visible = true;
                                    colCustLevel2.Visible = true;
                                    colCustLevel3.Visible = true;
                                    colCustLevel4.Visible = true;
                                    colCustLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 1:
                            switch (cmbxCustomerTo.SelectedIndex)
                            {

                                case 1:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = true;
                                    colCustLevel3.Visible = false;
                                    colCustLevel4.Visible = false;
                                    colCustLevel5.Visible = false;
                                    break;
                                case 2:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = true;
                                    colCustLevel3.Visible = true;

                                    colCustLevel4.Visible = false;
                                    colCustLevel5.Visible = false;
                                    break;
                                case 3:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = true;
                                    colCustLevel3.Visible = true;
                                    colCustLevel4.Visible = true;

                                    colCustLevel5.Visible = false;
                                    break;
                                case 4:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = true;
                                    colCustLevel3.Visible = true;
                                    colCustLevel4.Visible = true;
                                    colCustLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 2:
                            switch (cmbxCustomerTo.SelectedIndex)
                            {
                                case 2:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = false;
                                    colCustLevel3.Visible = true;

                                    colCustLevel4.Visible = false;
                                    colCustLevel5.Visible = false;
                                    break;
                                case 3:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = false;
                                    colCustLevel3.Visible = true;
                                    colCustLevel4.Visible = true;

                                    colCustLevel5.Visible = false;
                                    break;
                                case 4:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = false;
                                    colCustLevel3.Visible = true;
                                    colCustLevel4.Visible = true;
                                    colCustLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 3:
                            switch (cmbxCustomerTo.SelectedIndex)
                            {
                                case 3:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = false;
                                    colCustLevel3.Visible = false;
                                    colCustLevel4.Visible = true;

                                    colCustLevel5.Visible = false;
                                    break;
                                case 4:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = false;
                                    colCustLevel3.Visible = false;
                                    colCustLevel4.Visible = true;
                                    colCustLevel5.Visible = true;
                                    break;
                            }
                            break;
                        case 4:
                            switch (cmbxCustomerTo.SelectedIndex)
                            {
                                case 4:
                                    colCustLevel1.Visible = false;
                                    colCustLevel2.Visible = false;
                                    colCustLevel3.Visible = false;
                                    colCustLevel4.Visible = false;
                                    colCustLevel5.Visible = true;
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    DXMessageBox.Show("Please select Customer Levels to Apply filter!");
                }

            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        double amGrossProfitSE = 0, amGrossProfitME = 0;
        double percentageTotal = 0;
        private void grdsaleOrderReport_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            //if (e.IsTotalSummary)
            //{
            //    GridControl gridControl = sender as GridControl;

            //    switch (e.SummaryProcess)
            //    {
            //        case CustomSummaryProcess.Start:
            //            soAmountSE = 0;
            //            bmGrossProfitSE = 0;
            //            break;
            //        case CustomSummaryProcess.Calculate:
            //            soAmountSE += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["SOAmountSER"]));
            //            bmGrossProfitSE += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["BMgrossProfitSE"]));

            //            //Total = debitTotal - creditTotal;
            //            break;
            //        case CustomSummaryProcess.Finalize:

            //            e.TotalValue = (bmGrossProfitSE / soAmountSE) * 100;
            //            break;
            //    }
            //}


            if (e.IsTotalSummary)
            {
                // Reset totals at the start of the summary process
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    soAmountSE = 0;
                    soAmountME = 0;
                    bmGrossProfitSE = 0;
                    bmGrossProfitME = 0;
                    smGrossProfitSE = 0;
                    smGrossProfitME = 0;
                    amGrossProfitSE = 0;
                    amGrossProfitME = 0;
                }

                // Perform calculations for each row during the summary process
                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    // Access cell values for specific columns
                    if (e.RowHandle != GridControl.InvalidRowHandle)
                    {
                        soAmountSE += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["SOAmountSER"]));
                        soAmountME += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["SOAmountMER"]));

                        bmGrossProfitSE += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["BMgrossProfitSE"]));
                        bmGrossProfitME += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["BMgrossProfitME"]));

                        smGrossProfitSE += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["SalesSystemMargin1"]));
                        smGrossProfitME += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["SalesMarketMargin1"]));

                        amGrossProfitSE += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["actualMarginSER"]));
                        amGrossProfitME += Convert.ToDouble(grdsaleOrderReport.GetCellValue(e.RowHandle, grdsaleOrderReport.Columns["actualMarginMER"]));
                    }
                }

                // Finalize the calculation based on the summary item's FieldName
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    var summaryItem = e.Item as GridSummaryItem; // Cast the item to GridSummaryItem
                    if (summaryItem != null)
                    {
                        if (summaryItem.FieldName == "BMgrossProfitSEPercent")
                        {
                            e.TotalValue = soAmountSE > 0 ? (bmGrossProfitSE / soAmountSE) * 100 : 0;
                        }
                        else if (summaryItem.FieldName == "BMgrossProfitMEPercent")
                        {
                            e.TotalValue = soAmountME > 0 ? (bmGrossProfitME / soAmountME) * 100 : 0;
                        }
                        if (summaryItem.FieldName == "SMgrossProfitSEPercent")
                        {
                            e.TotalValue = soAmountSE > 0 ? (smGrossProfitSE / soAmountSE) * 100 : 0;
                        }
                        else if (summaryItem.FieldName == "SMgrossProfitMEPercent")
                        {
                            e.TotalValue = soAmountME > 0 ? (smGrossProfitME / soAmountME) * 100 : 0;
                        }
                        if (summaryItem.FieldName == "AMgrossProfitSEPercent")
                        {
                            e.TotalValue = soAmountSE > 0 ? (amGrossProfitSE / soAmountSE) * 100 : 0;
                        }
                        else if (summaryItem.FieldName == "AMgrossProfitMEPercent")
                        {
                            e.TotalValue = soAmountME > 0 ? (amGrossProfitME / soAmountME) * 100 : 0;
                        }
                    }
                }
            }



        }

    }


}
