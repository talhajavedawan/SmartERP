using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
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
using ZAS_ERP.Reportss;

namespace ZAS_ERP.Procurementss.SaleOrderss.UserControls
{
    /// <summary>
    /// Interaction logic for ucPRITregister.xaml
    /// </summary>
    public partial class ucPRITregister : UserControl
    {
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        static List<ExchangeRateGroup> exchangeRateGroupsSER = new List<ExchangeRateGroup>();
        static List<ExchangeRateGroup> exchangeRateGroupsMER = new List<ExchangeRateGroup>();
        ExchangeRate exchangeRateSER = null;
        ExchangeRate exchangeRateMER = null;
        ExchangeRate exchangeRateCMER = null;
        public ucPRITregister()
        {
            InitializeComponent();
        }

        private void grid_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {

                if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
                {
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.saleOrder.department;
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
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.saleOrder.department;
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
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.saleOrder.department;
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
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.saleOrder.department;

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
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var deptList = new List<Department>();
                        var node = row.saleOrder.department;

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

                if (e.Column.FieldName == "customerLevel1" && e.IsGetData)
                {
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.saleOrder.customerCompany;
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
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.saleOrder.customerCompany;
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
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.saleOrder.customerCompany;
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
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.saleOrder.customerCompany;

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
                    var row = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                    if (row != null)
                    {
                        var customerList = new List<CustomerCompany>();
                        var node = row.saleOrder.customerCompany;

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

                if (e.IsGetData)
                    switch (e.Column.FieldName)
                    {
                        case "SoAmountPER":
                            var selectedRow31 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;

                            e.Value = Math.Round(Convert.ToDecimal(selectedRow31.Amount) * selectedRow31.saleOrder.PER, 2);

                            break;
                        case "Vendorss":
                            if (e.Column.FieldName == "Vendorss" && e.IsGetData)
                            {
                                var _SO = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                                string vendorNames = "";

                                if (_SO.saleOrder.vendors != null && _SO.saleOrder.vendors.Count > 0)
                                {
                                    vendorNames = String.Join(" | ", _SO.saleOrder.vendors.Select(x => x.company.CompanyName));
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
                                var selectedRow66 =  grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;

                                if (selectedRow66.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
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
                            var selectedRow = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            var budgetCost = Convert.ToDouble(e.GetListSourceFieldValue("BudgetMarginOC"));
                            if (selectedRow.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = budgetCost * selectedRow.saleOrder.ccSER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(budgetCost) * Convert.ToDouble(selectedRow.saleOrder.marginExchangeRate);
                            }

                            break;
                        case "BudgetMarginMER":
                            var selectedRow1 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            var budgetCostMER = Convert.ToDouble(e.GetListSourceFieldValue("BudgetMarginOC"));

                            if (selectedRow1.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {

                                e.Value = budgetCostMER * selectedRow1.saleOrder.ccMER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(budgetCostMER) * Convert.ToDouble(selectedRow1.saleOrder.exchangeRate);
                            }

                            break;
                        case "RevisedMarginSER":
                            var selectedRow2 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            var revisedCost = Convert.ToDouble(e.GetListSourceFieldValue("RevisedMarginOC"));

                            if (selectedRow2.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = revisedCost * selectedRow2.saleOrder.ccSER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(revisedCost) * Convert.ToDouble(selectedRow2.saleOrder.marginExchangeRate);
                            }

                            break;
                        case "RevisedMarginMER":
                            var selectedRow3 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            var revisedCostMER = Convert.ToDouble(e.GetListSourceFieldValue("RevisedMarginOC"));

                            if (selectedRow3.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = revisedCostMER * selectedRow3.saleOrder.ccMER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(revisedCostMER) * Convert.ToDouble(selectedRow3.saleOrder.exchangeRate);
                            }

                            break;
                        case "actualMarginSER":
                            var selectedRow4 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            var actualMargin = Convert.ToDouble(e.GetListSourceFieldValue("ActualMarginOC"));

                            if (selectedRow4.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = actualMargin * selectedRow4.saleOrder.ccSER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(actualMargin) * Convert.ToDouble(selectedRow4.saleOrder.marginExchangeRate);
                            }

                            break;
                        case "actualMarginMER":
                            var selectedRow5 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            var actualMarginMER = Convert.ToDouble(e.GetListSourceFieldValue("ActualMarginOC"));

                            if (selectedRow5.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = actualMarginMER * selectedRow5.saleOrder.ccMER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(actualMarginMER) * Convert.ToDouble(selectedRow5.saleOrder.exchangeRate);
                            }

                            break;
                        case "SalesSystemMargin1":
                            var selectedRow6 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            var systemMargin = Convert.ToDouble(e.GetListSourceFieldValue("SystemMargin"));

                            if (selectedRow6.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = systemMargin * selectedRow6.saleOrder.ccSER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(systemMargin) * Convert.ToDouble(selectedRow6.saleOrder.marginExchangeRate);
                            }

                            break;
                        case "SalesMarketMargin1":
                            var selectedRow7 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            var systemMarginMER = Convert.ToDouble(e.GetListSourceFieldValue("SystemMargin"));

                            if (selectedRow7.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
                            {
                                e.Value = systemMarginMER * selectedRow7.saleOrder.ccMER;
                            }
                            else
                            {
                                e.Value = Convert.ToDouble(systemMarginMER) * Convert.ToDouble(selectedRow7.saleOrder.exchangeRate);
                            }

                            break;



                        case "BudgetedMarginPercentAge":
                            if (e.GetListSourceFieldValue("totalCFRValue") != null && e.GetListSourceFieldValue("CostSheet") != null)
                            {
                                decimal total = 0;
                                var selectedRow66 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;

                                if (selectedRow66.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
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
                                var selectedRow66 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;

                                if (selectedRow66.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
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
                                var selectedRow66 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;

                                if (selectedRow66.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
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
                                var selectedRow66 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;

                                if (selectedRow66.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
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
                                var selectedRow66 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;

                                if (selectedRow66.saleOrder.saleOrdertype == InquiryType.SupplyCCC)
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
                            var saleOrder = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            e.Value = saleOrder.saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.totalInvoiceAmount);
                            break;
                        case "recieptTotalOC":
                            var saleOrder1 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            e.Value = saleOrder1.saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                            break;
                        case "totalDeductionsOC":
                            var saleOrder2 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            e.Value = saleOrder2.saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.receiptDeductions.Sum(y => y.Amount)));
                            break;
                        case "totalCreditedOC":
                            var saleOrder3 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            var result = saleOrder3.saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.receiptDeductions.Sum(y => y.Amount)));
                            var collection = saleOrder3.saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount));
                            e.Value = collection - result;
                            break;
                        case "remainingCollectionOC":
                            var saleOrder4 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            if (saleOrder4.saleOrder.saleOrdertype == InquiryType.DistributionBiz)
                            {
                                var invoicedAmount = Math.Round((saleOrder4.saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(y => y.BookerStatementItems.Sum(z => z.siNetAmount))).Value, 2);
                                var collected = Math.Round(Convert.ToDouble(saleOrder4.saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount))), 2);
                                e.Value = Math.Round((Convert.ToDouble(invoicedAmount) - collected), 2);
                            }
                            else if (saleOrder4.saleOrder.saleOrdertype == InquiryType.Principal)
                            {
                                if (saleOrder4.saleOrder.SaleInvoices != null && e.GetListSourceFieldValue("commision") != null)
                                {
                                    var total = Convert.ToDecimal(e.GetListSourceFieldValue("commision"));
                                    var result13 = Convert.ToDecimal(saleOrder4.saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                                    e.Value = total - result13;
                                }
                            }
                            else
                            {
                                if (saleOrder4.saleOrder.SaleInvoices != null && e.GetListSourceFieldValue("totalCFRValue") != null)
                                {
                                    var total = Convert.ToDecimal(e.GetListSourceFieldValue("totalCFRValue"));
                                    var result13 = Convert.ToDecimal(saleOrder4.saleOrder.SaleInvoices?.Where(x => x.isVoid != true).Sum(x => x.salesReceipts?.Where(y => y.isVoid != true).Sum(z => z.CollectionAmount)));
                                    e.Value = total - result13;
                                }
                            }

                            break;
                        case "SystemCost":
                            var saleOrder5 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            if (saleOrder5.saleOrder.CostSheet_Id != null)
                            {
                                var systemCost = saleOrderRepo.GetSOSystemCost((int)saleOrder5.saleOrder.CostSheet_Id);

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
                                var saleOrderSER = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                                double todayRate = 0;
                                var exchangeRateGroupSER = exchangeRateGroupsSER.FirstOrDefault(x => x.transaction_currency_Id == saleOrderSER.saleOrder.currency_Id && x.base_currency_Id == saleOrderSER.saleOrder.company.CurrencyId && x.TargetYear == saleOrderSER.saleOrder.CreationDate.Value.Year);
                                if (exchangeRateGroupSER != null)
                                {
                                    exchangeRateSER = exchangeRateGroupSER.exchangeRates.FirstOrDefault(x => x.company_Id == saleOrderSER.saleOrder.company_Id);
                                    switch (saleOrderSER.saleOrder.CreationDate.Value.Month)
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
                                var saleOrderMER = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                                double todayRate = 0;
                                var exchangeRateGroupMER = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == saleOrderMER.saleOrder.currency_Id && x.base_currency_Id == saleOrderMER.saleOrder.company.CurrencyId && x.TargetYear == saleOrderMER.saleOrder.CreationDate.Value.Year);
                                if (exchangeRateGroupMER != null)
                                {
                                    exchangeRateMER = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == saleOrderMER.saleOrder.company_Id);
                                    switch (saleOrderMER.saleOrder.CreationDate.Value.Month)
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
                                var SOCMER = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                                double todayRate = 0;
                                var exchangeRateGroupMER = exchangeRateGroupsMER.FirstOrDefault(x => x.transaction_currency_Id == SOCMER.saleOrder.currency_Id && x.base_currency_Id == SOCMER.saleOrder.company.CurrencyId && x.TargetYear == DateTime.Now.Year);
                                if (exchangeRateGroupMER != null)
                                {
                                    exchangeRateCMER = exchangeRateGroupMER.exchangeRates.FirstOrDefault(x => x.company_Id == SOCMER.saleOrder.company_Id);
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
                            var order = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            if (order.saleOrder.PerformanceSheet_Id != null)
                            {
                                e.Value = order.saleOrder.PerformanceSheet.performanceSheetFields.Sum(x => x.point);
                            }
                            //else
                            //{
                            //    e.Value = 0;
                            //}
                            break;
                        case "depHeadPoints":
                            var order1 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            if (order1.saleOrder.PerformanceSheet_Id != null)
                            {
                                e.Value = order1.saleOrder.PerformanceSheet.performanceSheetFields.Sum(x => x.revisedPoint);
                            }
                            //else
                            //{
                            //    e.Value = 0;
                            //}
                            break;
                        case "finalPoints":
                            var order2 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            if (order2.saleOrder.PerformanceSheet_Id != null)
                            {
                                e.Value = order2.saleOrder.PerformanceSheet.totalPoints;
                            }
                            //else
                            //{
                            //    e.Value = 0;
                            //}
                            break;
                        case "totalPoints":
                            var order3 = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            if (order3.saleOrder.PerformanceSheet_Id != null)
                            {
                                e.Value = order3.saleOrder.PerformanceSheet.performanceSheetFields.Sum(x => x.PerformanceSheetHead.totalPoints);
                            }
                            break;
                        case "remainingInvoiced":
                            var so = grdPERgrid.GetRowByListIndex(e.ListSourceRowIndex) as SplitPER;
                            if (so.saleOrder.SaleInvoices != null)
                            {
                                if (so.saleOrder.SaleInvoices.Count != 0)
                                {
                                    var totalInvoiced = so.saleOrder.SaleInvoices.Sum(x => x.totalInvoiceAmount);
                                    var totalSOAmount = so.saleOrder.totalCFRValue;
                                    var value = totalSOAmount - totalInvoiced;
                                    e.Value = value;
                                }
                            }

                            break;
                    }
            }
            catch (Exception)
            {


            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdPERgrid.ItemsSource = saleOrderRepo.GetSplitPER(SYSTEM_STATIC.currentUser.id);
        }

        private void GrdsaleOrder_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void grdsaleOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSaleOrder();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            saleOrderRepo = new SaleOrderRepo();
            grdPERgrid.ItemsSource = saleOrderRepo.GetSplitPER(SYSTEM_STATIC.currentUser.id);
        }

        private void EditSaleOrder()
        {
            if (grdPERgrid.SelectedItem != null)
            {
                var PER = grdPERgrid.SelectedItem as SplitPER;

                if(PER != null && PER.saleOrder != null)
                {
                    Procurementss.frmProcurmentPanel procurmentPanel = new Procurementss.frmProcurmentPanel(TransactionItemType.Sale_Order, PER.saleOrder.Id);
                    procurmentPanel.Show();
                }
            }
        }

        private void btnExporttoReport_Click(object sender, RoutedEventArgs e)
        {
            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Sale Orders Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Sale Orders") != null)
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
                        ReportLogic.SaveGridReport(grdPERgrid, reportName, reportType, reportGroup, lblHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Sale Orders Report!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
