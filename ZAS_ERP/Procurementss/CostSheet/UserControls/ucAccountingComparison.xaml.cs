using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL;
using ERP_BL.Databases;
using ERP_BL.Enums;
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
using System.Windows.Shapes;
using ZAS_ERP.Procurementss.SaleOrderss;


namespace ZAS_ERP.Procurementss.CostSheet.UserControls
{
    /// <summary>
    /// Interaction logic for ucAccountingComparison.xaml
    /// </summary>
    public partial class ucAccountingComparison : UserControl
    {
        ProcurementRepo repo = new ProcurementRepo();
        public bool isParent = false;
        public int Id = 0;
        public ERP_BL.Enums.TransactionItemType Type = TransactionItemType.Inquiry;
        decimal sum = 0;
        decimal sumChild = 0;
        decimal totalChildBudget = 0;
        decimal totalParentBudget = 0;
        decimal totalChildActual = 0;
        decimal totalParentActual = 0;

        decimal totalChildSystem = 0;
        decimal totalParentSystem = 0;
        //decimal totalChildRSBC = 0;
        //decimal totalChildSystemCost = 0;
        public List<CostSheetBillField> costSheetBillFieldsParent = new List<CostSheetBillField>();
        public List<CostSheetPOField> costSheetPOFieldsParent = new List<CostSheetPOField>();
        public List<CostSheetSIField> costSheetSIFieldsParent = new List<CostSheetSIField>();
        public List<CostSheetSOField> costSheetSOFieldsParent = new List<CostSheetSOField>();
        public List<CostSheetSaleReceiptField> costSheetSaleReceiptFieldsParent = new List<CostSheetSaleReceiptField>();
        public List<CostSheetPaymentField> costSheetPaymentFieldsParent = new List<CostSheetPaymentField>();
        public List<CostSheetBillField> costSheetBillFieldsChild = new List<CostSheetBillField>();
        public List<CostSheetPOField> costSheetPOFieldsChild = new List<CostSheetPOField>();
        public List<CostSheetSIField> costSheetSIFieldsChild = new List<CostSheetSIField>();
        public List<CostSheetSOField> costSheetSOFieldsChild = new List<CostSheetSOField>();
        public List<CostSheetSaleReceiptField> costSheetSaleReceiptFieldsChild = new List<CostSheetSaleReceiptField>();
        public List<CostSheetPaymentField> costSheetPaymentFieldsChild = new List<CostSheetPaymentField>();
        public List<CostFieldValues> costParentFieldValue = new List<CostFieldValues>();
        public List<CostFieldValues> costChildFieldValue = new List<CostFieldValues>();
        public List<int> companyIds = new List<int>();
        public List<int> deptIds = new List<int>();
        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        List<SaleOrderStatus> SaleOrderStatuses = new List<SaleOrderStatus>();

        public ucAccountingComparison()
        {
            InitializeComponent();
        }
        public ucAccountingComparison(int id, TransactionItemType type, bool _isParent)
        {
            InitializeComponent();
            Id = id;
            Type = type;
            isParent = _isParent;
        }
        private void grdParentCostItems_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:

                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                sum = sum + fieldValue;
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    sum = Convert.ToDecimal(txtTotalParentAmount.Text) - sum;
                                    totalParentBudget = sum;

                                }
                                e.TotalValue = sum;


                                break;
                        }
                    }
                    txtTotalParentBudgetedCost.Text = totalParentBudget.ToString();


                }
                else
                 if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "actualValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "actualValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:

                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValue"));
                                sum = sum + fieldValue;
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    sum = Convert.ToDecimal(txtTotalParentAmount.Text) - sum;
                                    totalParentActual = sum;

                                }
                                e.TotalValue = sum;


                                break;
                        }
                    }
                    txtTotalParentActualCost.Text = totalParentActual.ToString();
                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "revisedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "revisedValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("revisedValue"));
                                sum = sum + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    sum = Convert.ToDecimal(txtTotalParentAmount.Text) - sum;
                                }
                                e.TotalValue = sum;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBC")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBC")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));
                                var budgetFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                if (fieldValue != 0)
                                {
                                    sum = sum + fieldValue;
                                }
                                else
                                {
                                    sum = sum + budgetFieldValue;
                                }
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {
                                    sum = Convert.ToDecimal(txtTotalParentAmount.Text) - sum;
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = sum;



                                break;
                        }
                    }

                }
               
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));
                                var adjSCOst = Convert.ToDecimal(e.GetValue("adjSCost"));
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {
                                    sum = Convert.ToDecimal(txtTotalParentAmount.Text) - sum;
                                    totalParentSystem = sum;
                                }
                                e.TotalValue = sum;
                                break;
                        }
                        txtTotalParentSystemCost.Text = totalParentSystem.ToString();
                    }
                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalParentAmount.Text) - sum) / Convert.ToDecimal(txtTotalParentAmount.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "actualValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "actualValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValuePerc"));
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (Convert.ToDecimal(txtTotalParentAmount.Text) - sum) / Convert.ToDecimal(txtTotalParentAmount.Text) * 100;
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;
                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBCValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBCValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));

                                var revisedFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                if (fieldValue != 0)
                                {
                                    sum = sum + fieldValue;
                                }
                                else
                                if (revisedFieldValue != 0)
                                {
                                    sum = sum + revisedFieldValue;
                                }
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalParentAmount.Text) - sum) / Convert.ToDecimal(txtTotalParentAmount.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;



                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));

                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalParentAmount.Text) - sum) / Convert.ToDecimal(txtTotalParentAmount.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        private void grdParentCostItems_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTotalParentAmount.Text))
                {
                    var totalSO = Convert.ToDecimal(txtTotalParentAmount.Text);

                    if (e.Column.FieldName == "budgetedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("budgetedValue") != null)
                        {
                            decimal budgetValue = Convert.ToDecimal(e.GetListSourceFieldValue("budgetedValue"));
                            var percent = (((/*totalso - */budgetValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);

                        }
                    }
                    else
                     if (e.Column.FieldName == "actualValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("actualValue") != null)
                        {
                            decimal budgetValue = Convert.ToDecimal(e.GetListSourceFieldValue("actualValue"));
                            var percent = /*totalso - */budgetValue / totalSO * 100;
                            e.Value = decimal.Round(percent, 2);

                        }
                    }
                    else
                if (e.Column.FieldName == "revisedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("revisedValue") != null)
                        {
                            decimal revisedValue = Convert.ToDecimal(e.GetListSourceFieldValue("revisedValue"));


                            var percent = (((/*totalSO -*/revisedValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);


                        }
                    }
                    else
                if (e.Column.FieldName == "SRBCValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("SRBC") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("SRBC"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);


                        }
                    }
                    else
                if (e.Column.FieldName == "systemValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("systemValue") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("systemValue"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);


                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grdChildCostItems_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                sumChild = sumChild + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                    totalChildBudget = sumChild;

                                }
                                e.TotalValue = sumChild;


                                break;
                        }
                    }
                    txtTotalChildBudgetedCost.Text = totalChildBudget.ToString();
                }
                else
                  if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "actualValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "actualValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                            
                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValue"));
                                sumChild = sumChild + fieldValue;
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                    totalChildActual = sumChild;
                                }
                                e.TotalValue = sumChild;


                                break;
                        }
                    }
                    txtTotalChildActualCost.Text = totalChildActual.ToString();
                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "revisedValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "revisedValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("revisedValue"));
                                sumChild = sumChild + fieldValue;
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {

                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                }
                                e.TotalValue = sumChild;


                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBC")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBC")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));
                                var budgetFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                if (fieldValue != 0)
                                {
                                    sumChild = sumChild + fieldValue;
                                }
                                else
                                {
                                    sumChild = sumChild + budgetFieldValue;
                                }
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    //totalChildRSBC = sumChild;
                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                    e.TotalValue = sumChild;
                                }
                                else
                                    e.TotalValue = sumChild;
                                break;
                        }

                    }
                    //txtTotalRSBC.Text = totalChildRSBC.ToString();
                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValue")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValue")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));
                                var adjSCOst = Convert.ToDecimal(e.GetValue("adjSCost"));
                                sumChild = sumChild + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    //totalChildSystemCost = sumChild;
                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                    totalChildSystem = sumChild;

                                }
                                e.TotalValue = sumChild;


                                break;
                        }
                        txtTotalChildSystemCost.Text = totalChildSystem.ToString();

                    }
                    //txtTotalSystemCost.Text = totalChildSystemCost.ToString();

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValuePercChild")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValuePercChild")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));

                                sumChild = sumChild + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild) / Convert.ToDecimal(txtTotalAmountChild.Text)) * 100);
                                    sumChild = decimal.Round(percent, 2);
                                    e.TotalValue = sumChild;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
                else
                   if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "actualValuePercChild")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "actualValuePercChild")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValue"));
                                sumChild = sumChild + fieldValue;
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    var percent = (Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild) / Convert.ToDecimal(txtTotalAmountChild.Text) * 100;
                                    sumChild = decimal.Round(percent, 2);
                                    e.TotalValue = sumChild;
                                }
                                else
                                    e.TotalValue = 100;
                                break;
                        }
                    }
                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "SRBCValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "SRBCValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("SRBC"));

                                var revisedFieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                if (fieldValue != 0)
                                {
                                    sumChild = sumChild + fieldValue;
                                }
                                else
                                if (revisedFieldValue != 0)
                                {
                                    sumChild = sumChild + revisedFieldValue;
                                }
                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild) / Convert.ToDecimal(txtTotalAmountChild.Text)) * 100);
                                    sumChild = decimal.Round(percent, 2);
                                    e.TotalValue = sumChild;
                                }
                                else
                                    e.TotalValue = 100;



                                break;
                        }
                    }

                }
                else
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "systemValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "systemValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sumChild = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));

                                sumChild = sumChild + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild) / Convert.ToDecimal(txtTotalAmountChild.Text)) * 100);
                                    sumChild = decimal.Round(percent, 2);
                                    e.TotalValue = sumChild;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "budgetedValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "budgetedValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValueChild"));
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (((Convert.ToDecimal(txtTotalAmountChild.Text) - sum) / Convert.ToDecimal(txtTotalAmountChild.Text)) * 100);
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
                if (e.IsTotalSummary && (e.Item as GridSummaryItem).FieldName == "actualValuePerc")
                {
                    GridSummaryItem item = e.Item as GridSummaryItem;
                    if (item.FieldName == "actualValuePerc")
                    {
                        switch (e.SummaryProcess)
                        {
                            case CustomSummaryProcess.Start:
                                sum = 0;
                                break;
                            case CustomSummaryProcess.Calculate:
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValueChild"));
                                sum = sum + fieldValue;

                                //}
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sum != 0)
                                {

                                    var percent = (Convert.ToDecimal(txtTotalAmountChild.Text) - sum) / Convert.ToDecimal(txtTotalAmountChild.Text) * 100;
                                    sum = decimal.Round(percent, 2);
                                    e.TotalValue = sum;
                                }
                                else
                                    e.TotalValue = 100;


                                break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grdChildCostItems_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtTotalAmountChild.Text))
                {
                    var totalSO = Convert.ToDecimal(txtTotalAmountChild.Text);

                    if (e.Column.FieldName == "budgetedValuePercChild" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("budgetedValue") != null)
                        {
                            decimal budgetValue = Convert.ToDecimal(e.GetListSourceFieldValue("budgetedValue"));
                            var percent = (((/*totalSO - */budgetValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";
                        }
                    }
                    else
                      if (e.Column.FieldName == "actualValuePercChild" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("actualValue") != null)
                        {
                            decimal actualtValue = Convert.ToDecimal(e.GetListSourceFieldValue("actualValue"));
                            var percent = (/*totalSO - */actualtValue) / totalSO * 100;
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";
                        }
                    }
                    else
                if (e.Column.FieldName == "revisedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("revisedValue") != null)
                        {
                            decimal revisedValue = Convert.ToDecimal(e.GetListSourceFieldValue("revisedValue"));


                            var percent = (((/*totalSO -*/revisedValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }
                    else
                if (e.Column.FieldName == "SRBCValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("SRBC") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("SRBC"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }
                    else
                if (e.Column.FieldName == "systemValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("systemValue") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("systemValue"));


                            var percent = (((/*totalSO -*/ srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                            //e.Value += "%";

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void parentView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void childView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void GrdTransactionListing_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void GrdTransactionListing_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var grid = (DevExpress.Xpf.Grid.GridControl)sender;
            if (grid.SelectedItem != null)
            {

                var item = (AllTransactionsView)grid.SelectedItem;


                Procurementss.frmProcurmentPanel procurmentPanele = new Procurementss.frmProcurmentPanel((TransactionItemType)Enum.Parse(typeof(TransactionItemType), item.TransactionType), Convert.ToInt32(item.Id.Split('_').First()));
                procurmentPanele.Show();

            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCreatePO_Click(object sender, RoutedEventArgs e)
        {

        }
        public void loadSaleOrderStatus()
        {


            if (/*SystemLogic.AllowedPermissions.Find(x => x.Name == "View InActive Sale Order Statuses") != null ||*/ SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed SaleOrder") != null)
            {
                SaleOrderStatuses = saleOrderRepo.getAllSaleOrderStatus();
            }
            else
                SaleOrderStatuses = saleOrderRepo.getAllActiveSaleOrderStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            /*Parallel.ForEach(SaleOrderStatuses, delegate (SaleOrderStatus status)*/
            foreach (SaleOrderStatus status in SaleOrderStatuses)
            {

                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
                //});
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbStatus.ItemsSource = cmbitems;
            cmbStatusChild.ItemsSource = cmbitems;
        }
        public void loadSaleOrderStatus(SaleOrderStatus _saleOrderStatus)
        {
            SaleOrderStatuses.Add(_saleOrderStatus);
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (SaleOrderStatus status in SaleOrderStatuses)
            {

                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
                //});
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            cmbStatus.ItemsSource = cmbitems;
            cmbStatusChild.ItemsSource = cmbitems;
        }

        public void loadVendors(SaleOrder saleOrder)
        {
            if (saleOrder.Id != 0 && saleOrder.department != null && saleOrder.vendors.Count != 0)
                lookupVendors.ItemsSource = saleOrder.department.Vendors;
        }
        public void loadPaymentTerms()
        {
            List<PaymentTerm> paymentTerms = new List<PaymentTerm>();
            PaymentTermRepo TermRepo = new PaymentTermRepo();
            paymentTerms = TermRepo.getAll();

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (PaymentTerm paymentTerm in paymentTerms)
            {
                cmbitems.Add(new cmbitem() { name = paymentTerm.term, id = paymentTerm.Id });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            lookupPaymentTerms.ItemsSource = paymentTerms;
            lookupPaymentTermsChild.ItemsSource = paymentTerms;


        }
        public void loadIncoterms()
        {
            IncotermRepo termRepo = new IncotermRepo();
            List<Incoterm> incoterms = new List<Incoterm>();
            List<IncoTermName> incotermNames = new List<IncoTermName>();

            incoterms = termRepo.getAll();

            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (var term in incoterms)
            {
                IncoTermName termName = new IncoTermName()
                {
                    Id = term.Id,
                    termName = term.term,
                    discription = term.discription,
                    isActive = term.isActive

                };
                incotermNames.Add(termName);
            }
            lookupInco.ItemsSource = incotermNames.Distinct();
            lookupIncoChild.ItemsSource = incotermNames.Distinct();

        }
        public void loadWarrantys()
        {
            List<Warranty> warrantys = new List<Warranty>();
            ProcurementRepo repo = new ProcurementRepo();
            warrantys = repo.GetActiveWarranties();
            lookupWarrenty.ItemsSource = warrantys;
            lookupWarrentyChild.ItemsSource = warrantys;

        }
        /// <summary>
        /// Load Cost Fields Value
        /// </summary>
        public List<CostFieldValues> GetCostSheetParentValues(SaleOrder saleOrder)
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costSheetBillFieldsParent = new List<CostSheetBillField>();
            costSheetPOFieldsParent = new List<CostSheetPOField>();
            costSheetSIFieldsParent = new List<CostSheetSIField>();
            costSheetSOFieldsParent = new List<CostSheetSOField>();
            costSheetSaleReceiptFieldsParent = new List<CostSheetSaleReceiptField>();
            costSheetPaymentFieldsParent = new List<CostSheetPaymentField>();
            costfieldValues = grdParentCostItems.ItemsSource as List<CostFieldValues>;
            if (saleOrder.Id != 0 && saleOrder.CostSheet_Id != 0)
            {
                costSheetSOFieldsParent = saleOrderRepo.GetCostSheetSOFields(saleOrder.Id, (int)saleOrder.CostSheet_Id);
            }

            foreach (var item in costfieldValues)
            {

                foreach (var field in saleOrder.CostSheet.FieldValues)
                    if (item.Id == field.FieldId)
                    {
                        if (field.Type == 2)
                            item.actualValue = field.Value;

                        else if (field.Type == 0)
                        {
                            item.revisedValue = field.Value;
                            if (item.revisedValue == 0)
                            {
                                item.revisedValue = item.budgetedValue;
                            }
                        }
                        else if (field.Type == 1)
                        {
                            item.budgetedValue = field.Value;
                            if (item.revisedValue == 0 || saleOrder.isApproved != true)
                            {
                                item.revisedValue = field.Value;
                            }
                        }
                        else if (field.Type == 3)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                var value = saleOrderRepo.GetSystemCost((int)saleOrder.CostSheet_Id, item.Id);
                                if (item.systemValue == 0 || saleOrder.isApproved != true)
                                {
                                    if (item.systemValue == 0)
                                    {
                                        item.systemValue = value + field.adjSCost;
                                        item.adjSCost = field.adjSCost;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 4)
                        {
                            VendorRepo vendorRepo = new VendorRepo();
                            var vendor = vendorRepo.Get((int)field.Value);
                            item.vendor = vendor;
                        }
                        else if (field.Type == 10)
                        {
                            if (costSheetSOFieldsParent.Count != 0)
                            {
                                var dbField = costSheetSOFieldsParent.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.soAmountSRBC && x.CostSheetId == saleOrder.CostSheet_Id && x.SO_Id == saleOrder.Id);
                                if (dbField != null)
                                {
                                    var soFieldAmount = dbField.Value;
                                    if (soFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.soAmountSRBC = soFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 11)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                var value = saleOrderRepo.GetSBRC((int)saleOrder.CostSheet_Id, item.Id);
                                if (item.SRBC == 0 || saleOrder.isApproved != true)
                                {
                                    item.SRBC = value;
                                }
                            }
                        }
                        else if (field.Type == 12)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.Maker = field.stringValue;

                            }
                        }
                        else if (field.Type == 13)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.Origin = field.stringValue;
                            }
                        }
                        else if (field.Type == 14)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.Packing = field.isPacking;
                            }
                        }
                        else if (field.Type == 15)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.deliveryDate = field.dateValue;
                            }
                        }
                        else if (field.Type == 16)
                        {
                            PaymentTermRepo paymentTermRepo = new PaymentTermRepo();
                            var paymentTerm = paymentTermRepo.get((int)field.Value);
                            item.PaymentTerm = paymentTerm;
                        }
                        else if (field.Type == 17)
                        {
                            IncotermRepo incotermRepo = new IncotermRepo();
                            var incoterm = incotermRepo.get((int)field.Value);
                            IncoTermName incoTermName = new IncoTermName()
                            {
                                Id = incoterm.Id,
                                termName = incoterm.term,
                                discription = incoterm.discription,
                                isActive = incoterm.isActive
                            };

                            item.IncotermName = incoTermName;
                        }
                        else if (field.Type == 18)
                        {
                            ProcurementRepo repo = new ProcurementRepo();
                            var warranty = repo.GetWarranty((int)field.Value);

                            item.Warranty = warranty;
                        }
                        else if (field.Type == 19)
                        {
                            item.Dg_Goods = field.isdgGood;
                        }
                        else if (field.Type == 20)
                        {
                            CurrencyRepo currencyRepo = new CurrencyRepo();
                            var currency = currencyRepo.get((int)field.Value);
                            item.OC = currency;
                        }
                        else
                        if (field.Type == 21)
                            item.OCamount = field.Value;
                        else if (field.Type == 22)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.LoadingPort = field.stringValue;
                            }

                        }
                        else if (field.Type == 23)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.DestinationPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 24)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var document = repo.GetPQ((int)field.Value);
                                item.PQ = document;
                            }
                        }
                        else if (field.Type == 25)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var term = repo.GetST((int)field.Value);
                                item.ST = term;
                            }
                        }
                        else if (field.Type == 26)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.HScode = field.stringValue;
                            }
                        }
                        else if (field.Type == 27)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.DrawaingRequired = field.drawingRequired;
                            }
                        }
                        else if (field.Type == 28)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.AttestedCOO = field.stringValue;
                            }
                        }
                        else if (field.Type == 29)
                        {
                            item.exchangeRate = field.Value;
                            if (item.exchangeRate == 0 || saleOrder.isApproved != true)
                            {
                                item.exchangeRate = field.Value;
                            }
                        }
                        else if (field.Type == 30)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.isExportLicense = field.isExportLicense;
                            }
                        }
                        //else if (field.Type == 31)
                        //{
                        //    item.billAmount = field.Value;
                        //    if (costSheetSaleReceiptFields.Count != 0)
                        //    {
                        //        var dbField = costSheetSaleReceiptFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.Receipt_Cost && x.CostSheetId == costSheet.Id && x.Receipt_Id == salesReceipt.Id);
                        //        if (dbField != null)
                        //        {
                        //            var ReceiptFieldAmount = dbField.Value;
                        //            if (ReceiptFieldAmount != 0 || saleOrder.isApproved != true)
                        //            {
                        //                item.receiptAmount = ReceiptFieldAmount;
                        //            }
                        //        }
                        //    }
                        //}
                        //else if (field.Type == 32)
                        //{
                        //    //if (field.Value != 0)
                        //    //{
                        //    //    //item.addedSystemValue = field.Value;
                        //    //    item.paymentAmount = field.Value;
                        //    //}
                        //    if (costSheetPaymentFields.Count != 0)
                        //    {
                        //        var dbField = costSheetPaymentFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.System_Payment && x.CostSheetId == costSheet.Id && x.Payment_Id == payment.Id);
                        //        if (dbField != null)
                        //        {
                        //            var paymentFieldAmount = dbField.Value;
                        //            if (paymentFieldAmount != 0 || saleOrder.isApproved != true)
                        //            {
                        //                item.paymentAmount = paymentFieldAmount;
                        //                item.addedSystemValue = paymentFieldAmount;
                        //            }
                        //        }
                        //    }
                        //}
                        else if (field.Type == 33)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.IntermediaryPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 34)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.deliveryDays = field.stringValue;
                            }
                        }

                        //else if (field.Type == 35)
                        //{
                        //    if (costSheet != null && costSheet.Id != 0)
                        //    {
                        //        if(item.adjSCost==0)
                        //        item.adjSCost = field.adjSCost;
                        //    }
                        //}

                        //else if (field.Type == 36)
                        //{
                        //    //item.billAmount = field.Value;
                        //    if (costSheetSIFields.Count != 0)
                        //    {
                        //        var dbField = costSheetSIFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.SI_Cost && x.CostSheetId == costSheet.Id && x.SI_Id == saleInvoice.Id);
                        //        if (dbField != null)
                        //        {
                        //            var SIFieldAmount = dbField.Value;
                        //            if (SIFieldAmount != 0 || saleOrder.isApproved != true)
                        //            {
                        //                item.poAmount = SIFieldAmount;
                        //            }
                        //        }
                        //    }
                        //}

                    }
                costfields.Add(item);

            }
            return costfields;
        }
        public List<CostFieldValues> GetCostSheetChildValues(SaleOrder saleOrder)
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costSheetBillFieldsParent = new List<CostSheetBillField>();
            costSheetPOFieldsParent = new List<CostSheetPOField>();
            costSheetSIFieldsParent = new List<CostSheetSIField>();
            costSheetSOFieldsParent = new List<CostSheetSOField>();
            costSheetSaleReceiptFieldsParent = new List<CostSheetSaleReceiptField>();
            costSheetPaymentFieldsParent = new List<CostSheetPaymentField>();
            costfieldValues = grdChildCostItems.ItemsSource as List<CostFieldValues>;
            if (saleOrder.Id != 0 && saleOrder.CostSheet_Id != 0)
            {
                costSheetSOFieldsParent = saleOrderRepo.GetCostSheetSOFields(saleOrder.Id, (int)saleOrder.CostSheet_Id);
            }

            foreach (var item in costfieldValues)
            {

                foreach (var field in saleOrder.CostSheet.FieldValues)
                    if (item.Id == field.FieldId)
                    {
                        if (field.Type == 2)
                            item.actualValue = field.Value;

                        else if (field.Type == 0)
                        {
                            item.revisedValue = field.Value;
                            if (item.revisedValue == 0)
                            {
                                item.revisedValue = item.budgetedValue;
                            }
                        }
                        else if (field.Type == 1)
                        {
                            item.budgetedValue = field.Value;
                            if (item.revisedValue == 0 || saleOrder.isApproved != true)
                            {
                                item.revisedValue = field.Value;
                            }
                        }
                        else if (field.Type == 3)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                var value = saleOrderRepo.GetSystemCost((int)saleOrder.CostSheet_Id, item.Id);
                                if (item.systemValue == 0 || saleOrder.isApproved != true)
                                {
                                    if (item.systemValue == 0)
                                    {
                                        item.systemValue = value + field.adjSCost;
                                        item.adjSCost = field.adjSCost;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 4)
                        {
                            VendorRepo vendorRepo = new VendorRepo();
                            var vendor = vendorRepo.Get((int)field.Value);
                            item.vendor = vendor;
                        }
                        else if (field.Type == 10)
                        {
                            if (costSheetSOFieldsParent.Count != 0)
                            {
                                var dbField = costSheetSOFieldsParent.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.soAmountSRBC && x.CostSheetId == saleOrder.CostSheet_Id && x.SO_Id == saleOrder.Id);
                                if (dbField != null)
                                {
                                    var soFieldAmount = dbField.Value;
                                    if (soFieldAmount != 0 || saleOrder.isApproved != true)
                                    {
                                        item.soAmountSRBC = soFieldAmount;
                                    }
                                }
                            }
                        }
                        else if (field.Type == 11)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                var value = saleOrderRepo.GetSBRC((int)saleOrder.CostSheet_Id, item.Id);
                                if (item.SRBC == 0 || saleOrder.isApproved != true)
                                {
                                    item.SRBC = value;
                                }
                            }
                        }
                        else if (field.Type == 12)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.Maker = field.stringValue;

                            }
                        }
                        else if (field.Type == 13)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.Origin = field.stringValue;
                            }
                        }
                        else if (field.Type == 14)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.Packing = field.isPacking;
                            }
                        }
                        else if (field.Type == 15)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.deliveryDate = field.dateValue;
                            }
                        }
                        else if (field.Type == 16)
                        {
                            PaymentTermRepo paymentTermRepo = new PaymentTermRepo();
                            var paymentTerm = paymentTermRepo.get((int)field.Value);
                            item.PaymentTerm = paymentTerm;
                        }
                        else if (field.Type == 17)
                        {
                            IncotermRepo incotermRepo = new IncotermRepo();
                            var incoterm = incotermRepo.get((int)field.Value);
                            IncoTermName incoTermName = new IncoTermName()
                            {
                                Id = incoterm.Id,
                                termName = incoterm.term,
                                discription = incoterm.discription,
                                isActive = incoterm.isActive
                            };

                            item.IncotermName = incoTermName;
                        }
                        else if (field.Type == 18)
                        {
                            ProcurementRepo repo = new ProcurementRepo();
                            var warranty = repo.GetWarranty((int)field.Value);

                            item.Warranty = warranty;
                        }
                        else if (field.Type == 19)
                        {
                            item.Dg_Goods = field.isdgGood;
                        }
                        else if (field.Type == 20)
                        {
                            CurrencyRepo currencyRepo = new CurrencyRepo();
                            var currency = currencyRepo.get((int)field.Value);
                            item.OC = currency;
                        }
                        else
                        if (field.Type == 21)
                            item.OCamount = field.Value;
                        else if (field.Type == 22)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.LoadingPort = field.stringValue;
                            }

                        }
                        else if (field.Type == 23)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.DestinationPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 24)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var document = repo.GetPQ((int)field.Value);
                                item.PQ = document;
                            }
                        }
                        else if (field.Type == 25)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                SaleOrderRepo repo = new SaleOrderRepo();
                                var term = repo.GetST((int)field.Value);
                                item.ST = term;
                            }
                        }
                        else if (field.Type == 26)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.HScode = field.stringValue;
                            }
                        }
                        else if (field.Type == 27)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.DrawaingRequired = field.drawingRequired;
                            }
                        }
                        else if (field.Type == 28)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.AttestedCOO = field.stringValue;
                            }
                        }
                        else if (field.Type == 29)
                        {
                            item.exchangeRate = field.Value;
                            if (item.exchangeRate == 0 || saleOrder.isApproved != true)
                            {
                                item.exchangeRate = field.Value;
                            }
                        }
                        else if (field.Type == 30)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {

                                item.isExportLicense = field.isExportLicense;
                            }
                        }
                        //else if (field.Type == 31)
                        //{
                        //    item.billAmount = field.Value;
                        //    if (costSheetSaleReceiptFields.Count != 0)
                        //    {
                        //        var dbField = costSheetSaleReceiptFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.Receipt_Cost && x.CostSheetId == costSheet.Id && x.Receipt_Id == salesReceipt.Id);
                        //        if (dbField != null)
                        //        {
                        //            var ReceiptFieldAmount = dbField.Value;
                        //            if (ReceiptFieldAmount != 0 || saleOrder.isApproved != true)
                        //            {
                        //                item.receiptAmount = ReceiptFieldAmount;
                        //            }
                        //        }
                        //    }
                        //}
                        //else if (field.Type == 32)
                        //{
                        //    //if (field.Value != 0)
                        //    //{
                        //    //    //item.addedSystemValue = field.Value;
                        //    //    item.paymentAmount = field.Value;
                        //    //}
                        //    if (costSheetPaymentFields.Count != 0)
                        //    {
                        //        var dbField = costSheetPaymentFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.System_Payment && x.CostSheetId == costSheet.Id && x.Payment_Id == payment.Id);
                        //        if (dbField != null)
                        //        {
                        //            var paymentFieldAmount = dbField.Value;
                        //            if (paymentFieldAmount != 0 || saleOrder.isApproved != true)
                        //            {
                        //                item.paymentAmount = paymentFieldAmount;
                        //                item.addedSystemValue = paymentFieldAmount;
                        //            }
                        //        }
                        //    }
                        //}
                        else if (field.Type == 33)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.IntermediaryPort = field.stringValue;
                            }
                        }
                        else if (field.Type == 34)
                        {
                            if (saleOrder.CostSheet != null && saleOrder.CostSheet_Id != 0)
                            {
                                item.deliveryDays = field.stringValue;
                            }
                        }

                        //else if (field.Type == 35)
                        //{
                        //    if (costSheet != null && costSheet.Id != 0)
                        //    {
                        //        if(item.adjSCost==0)
                        //        item.adjSCost = field.adjSCost;
                        //    }
                        //}

                        //else if (field.Type == 36)
                        //{
                        //    //item.billAmount = field.Value;
                        //    if (costSheetSIFields.Count != 0)
                        //    {
                        //        var dbField = costSheetSIFields.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.SI_Cost && x.CostSheetId == costSheet.Id && x.SI_Id == saleInvoice.Id);
                        //        if (dbField != null)
                        //        {
                        //            var SIFieldAmount = dbField.Value;
                        //            if (SIFieldAmount != 0 || saleOrder.isApproved != true)
                        //            {
                        //                item.poAmount = SIFieldAmount;
                        //            }
                        //        }
                        //    }
                        //}

                    }
                costfields.Add(item);

            }
            return costfields;
        }

        private void txtTotalBudgetedCost_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {


        }

        private void txtTotalRSBC_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }

        private void txtTotalSystemCost_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }

        private void txtTotalBudgetMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTotalBudgetMargin.Text) && !string.IsNullOrEmpty(txtTotalParentAmount.Text))
            {
                decimal totalMargin = Convert.ToDecimal(txtTotalBudgetMargin.Text);
                decimal totalSOAmount = Convert.ToDecimal(txtTotalParentAmount.Text);
                decimal totalValue = totalMargin / totalSOAmount * 100;
                txtTotalBudgetMarginPerc.Text = totalValue.ToString();
            }
        }
        private void txtTotalActualMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTotalActualMargin.Text) && !string.IsNullOrEmpty(txtTotalParentAmount.Text))
            {
                decimal totalMargin = Convert.ToDecimal(txtTotalActualMargin.Text);
                decimal totalSOAmount = Convert.ToDecimal(txtTotalParentAmount.Text);
                decimal totalValue = totalMargin / totalSOAmount * 100;
                txtTotalActualMarginPerc.Text = totalValue.ToString();
            }
        }
        private void txtTotalSystemMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTotalSystemMargin.Text) && !string.IsNullOrEmpty(txtTotalParentAmount.Text))
            {
                decimal totalMargin = Convert.ToDecimal(txtTotalSystemMargin.Text);
                decimal totalSOAmount = Convert.ToDecimal(txtTotalParentAmount.Text);
                decimal totalValue = totalMargin / totalSOAmount * 100;
                txtTotalSystemMarginPerc.Text = totalValue.ToString();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            costChildFieldValue = new List<CostFieldValues>();
            costParentFieldValue = new List<CostFieldValues>();
            loadSaleOrderStatus();
            List<AllTransactionsView> allTransactions = new List<AllTransactionsView>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View CostSheet in Tracking 360 Window") != null)
            {
                try
                {
                    if (isParent == true)
                    {
                        var saleOrders = repo.getLinkedSOS(Id, Type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.employeeId);

                        saleOrders= saleOrders.GroupBy(x => x.Id).Select(x => x.First()).ToList();


                        foreach (var SO in saleOrders)
                        {
                            if (SO.ParentSO_Id == null)
                            {
                                txtCompany.Text = SO.company.CompanyName;
                                txtCustomer.Text = SO.customerCompany.company.CompanyName;
                                txtDepartment.Text = SO.department.DeptName;
                                txtPrincipal.Text = SO.principal.company.CompanyName;
                                txtSaleOrderDate.Text = SO.saleOrderDate.ToString();
                                txtSORef.Text = SO.referenceNo;
                                txtFinanceRef.Text = SO.FinanceRefrenceNo;
                                txtSalesRef.Text = SO.SalesReferenceNo;
                                txtCreationDate.Text = SO.CreationDate.ToString();
                                txtTotalParentSOAmount.Text = SO.totalCFRValue.ToString();

                                if (SO.saleOrdertype == InquiryType.SupplyCCC)
                                {
                                    txtTotalParentAmount.Text = SO.costCenterAmount.ToString();
                                    txtParentSupplyCCTAmount.Text = SO.costCenterAmount.ToString();
                                    txtTotalParentSupplyCCER.Text = SO.costCenterExchangeRate.ToString();
                                    if (SO.costcenterCurrency != null)
                                        txtParentCurrencyCC.Text = SO.costcenterCurrency.CurrencyName;
                                    txtParentCurrencyOC.Text = SO.currency.CurrencyName;
                                }
                                else
                                {
                                    txtParentCurrencyCC.Text = SO.currency.CurrencyName;
                                    txtParentCurrencyOC.Text = SO.currency.CurrencyName;
                                    txtParentSupplyCCTAmount.Text = 0.ToString();

                                    txtTotalParentAmount.Text = SO.totalCFRValue.ToString();

                                }
                                if (SO.saleOrderStatus != null)
                                {
                                    var disAbleStatus = SaleOrderStatuses.FirstOrDefault(x => x.Id == SO.saleOrderStatus.Id);
                                    if (disAbleStatus == null)
                                    {
                                        loadSaleOrderStatus(SO.saleOrderStatus);
                                    }
                                }
                                var SOSource = (List<cmbitem>)cmbStatus.Items.SourceCollection;
                                cmbStatus.SelectedItem = cmbStatus.Items[cmbStatus.Items.IndexOf(SOSource.Find(x => x.name == SO.saleOrderStatus.Status))];

                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
                                {
                                    grdParentCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;

                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
                                {
                                    grdParentCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
                                {
                                    grdParentCostItems.Columns.GetColumnByFieldName("actualValue").ReadOnly = true;

                                }
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
                                {
                                    grdParentCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = false;

                                }
                                else
                                {
                                    grdParentCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = true;

                                }
                                foreach (var item in saleOrderRepo.getActiveCostSheetFields())
                                {
                                    costParentFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title });

                                }

                                grdParentCostItems.ItemsSource = costParentFieldValue;
                                var costfieldValues = GetCostSheetParentValues(SO);
                                var costfields = costfieldValues.Where(x => x.actualValue != 0 || x.SRBC != 0 || x.budgetedValue != 0 || x.revisedValue != 0 /*&& x.systemPayment != 0*/ && x.systemValue <= 0 && x.vendor == null).ToList();
                                grdParentCostItems.ItemsSource = costfields;

                            }
                            else
                            {
                                if (SO.isGeneratedBySOLink == false)
                                {
                                    txtCompanyChild.Text = SO.company.CompanyName;
                                    txtCustomerChild.Text = SO.customerCompany.company.CompanyName;
                                    txtDepartmentChild.Text = SO.department.DeptName;
                                    txtPrincipalChild.Text = SO.principal.company.CompanyName;
                                    txtSaleOrderDateChild.Text = SO.saleOrderDate.ToString();
                                    txtSORefChild.Text = SO.referenceNo;
                                    txtFinanceRefChild.Text = SO.FinanceRefrenceNo;
                                    txtSalesRefChild.Text = SO.SalesReferenceNo;
                                    txtCreationDateChild.Text = SO.CreationDate.ToString();
                                    txtTotalSOAmountChild.Text = SO.totalCFRValue.ToString();

                                    if (SO.saleOrdertype == InquiryType.SupplyCCC)
                                    {
                                        if (SO.costcenterCurrency != null)
                                            txtChildCurrencyCC.Text = SO.costcenterCurrency.CurrencyName;
                                        txtChildCurrencyOC.Text = SO.currency.CurrencyName;
                                        txtTotalAmountChild.Text = SO.costCenterAmount.ToString();
                                        txtSupplyCCTotalAmountChild.Text = SO.costCenterAmount.ToString();
                                        txtSupplyCCERChild.Text = SO.costCenterExchangeRate.ToString();

                                    }
                                    else
                                    {
                                        txtTotalAmountChild.Text = SO.totalCFRValue.ToString();
                                        txtChildCurrencyOC.Text = SO.currency.CurrencyName;
                                        txtSupplyCCTotalAmountChild.Text = 0.ToString();
                                        txtSupplyCCERChild.Text = 0.ToString();

                                    }
                                    if (SO.saleOrderStatus != null)
                                    {
                                        var disAbleStatus = SaleOrderStatuses.FirstOrDefault(x => x.Id == SO.saleOrderStatus.Id);
                                        if (disAbleStatus == null)
                                        {
                                            loadSaleOrderStatus(SO.saleOrderStatus);
                                        }
                                    }
                                    var SOSource = (List<cmbitem>)cmbStatusChild.Items.SourceCollection;
                                    cmbStatusChild.SelectedItem = cmbStatusChild.Items[cmbStatusChild.Items.IndexOf(SOSource.Find(x => x.name == SO.saleOrderStatus.Status))];
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budgeted Costs") == null)
                                    {
                                        grdChildCostItems.Columns.GetColumnByFieldName("budgetedValue").ReadOnly = true;

                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Adjustment System Costs") == null)
                                    {
                                        grdChildCostItems.Columns.GetColumnByFieldName("adjSCost").ReadOnly = true;

                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Actual Costs") == null)
                                    {
                                        grdChildCostItems.Columns.GetColumnByFieldName("actualValue").ReadOnly = true;

                                    }
                                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Revised Costs with out Approval") != null)
                                    {
                                        grdChildCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = false;

                                    }
                                    else
                                    {
                                        grdChildCostItems.Columns.GetColumnByFieldName("revisedValue").ReadOnly = true;

                                    }
                                    foreach (var item in saleOrderRepo.getActiveCostSheetFields())
                                    {
                                        costChildFieldValue.Add(new CostFieldValues { Id = item.Id, Title = item.Title });
                                    }
                                    grdChildCostItems.ItemsSource = costChildFieldValue;
                                    var costfieldValues = GetCostSheetChildValues(SO);
                                    costfieldValues = costfieldValues.GroupBy(x => x.Id).Select(x => x.First()).ToList();
                                    grdChildCostItems.ItemsSource = null;
                                    var costfields = costfieldValues.Where(x => x.actualValue != 0 || x.SRBC != 0 || x.budgetedValue != 0 || x.revisedValue != 0 /*&& x.systemPayment != 0*/ && x.systemValue <= 0 && x.vendor == null).ToList();
                                    grdChildCostItems.ItemsSource = costfields;
                                }
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

            if (!string.IsNullOrEmpty(txtTotalParentAmount.Text) && !string.IsNullOrEmpty(txtTotalChildBudgetedCost.Text) && !string.IsNullOrEmpty(txtTotalParentBudgetedCost.Text))
            {

                var totalBudget = Convert.ToDecimal(txtTotalParentBudgetedCost.Text) + Convert.ToDecimal(txtTotalChildBudgetedCost.Text);
                txtTotalBudgetMargin.Text = totalBudget.ToString();
            }
            if (!string.IsNullOrEmpty(txtTotalParentAmount.Text) && !string.IsNullOrEmpty(txtTotalChildActualCost.Text) && !string.IsNullOrEmpty(txtTotalParentActualCost.Text))
            {

                var totalActual = Convert.ToDecimal(txtTotalParentActualCost.Text) + Convert.ToDecimal(txtTotalChildActualCost.Text);
                
                txtTotalActualMargin.Text = totalActual==0?txtTotalParentAmount.Text:totalActual.ToString();
            }
            if (!string.IsNullOrEmpty(txtTotalParentAmount.Text) && !string.IsNullOrEmpty(txtTotalChildSystemCost.Text) && !string.IsNullOrEmpty(txtTotalParentSystemCost.Text))
            {

                var totalBudget = Convert.ToDecimal(txtTotalParentSystemCost.Text) + Convert.ToDecimal(txtTotalChildSystemCost.Text);
                txtTotalSystemMargin.Text = totalBudget.ToString();

            }
        }
    }
}

