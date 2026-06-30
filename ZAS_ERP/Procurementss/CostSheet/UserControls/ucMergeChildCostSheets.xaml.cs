using DevExpress.Data;
using DevExpress.Xpf.Grid;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZAS_ERP.Procurementss.SaleOrderss;

namespace ZAS_ERP.Procurementss.CostSheet.UserControls
{
    /// <summary>
    /// Interaction logic for ucMergeChildCostSheets.xaml
    /// </summary>
    public partial class ucMergeChildCostSheets : UserControl
    {
        decimal sum = 0;
        decimal sumChild = 0;
        decimal totalChildBudget = 0;
        decimal totalParentBudget = 0;

        decimal totalChildSystem = 0;
        decimal totalParentSystem = 0;
        public List<CostSheetBillField> costSheetBillFieldsChild = new List<CostSheetBillField>();
        public List<CostSheetPOField> costSheetPOFieldsChild = new List<CostSheetPOField>();
        public List<CostSheetSIField> costSheetSIFieldsChild = new List<CostSheetSIField>();
        public List<CostSheetSOField> costSheetSOFieldsChild = new List<CostSheetSOField>();
        public List<CostSheetSaleReceiptField> costSheetSaleReceiptFieldsChild = new List<CostSheetSaleReceiptField>();
        public List<CostSheetPaymentField> costSheetPaymentFieldsChild = new List<CostSheetPaymentField>();
        public List<CostFieldValues> costChildFieldValue = new List<CostFieldValues>();

        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        SaleOrder SO = new SaleOrder();
        List<SaleOrderStatus> SaleOrderStatuses = new List<SaleOrderStatus>();

        public ucMergeChildCostSheets()
        {
            InitializeComponent();
    
        }
        public ucMergeChildCostSheets(SaleOrder _SO )
        {
            InitializeComponent();
            SO = _SO;


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
            lookupIncoChild.ItemsSource = incotermNames.Distinct();

        }
        public void loadWarrantys()
        {
            List<Warranty> warrantys = new List<Warranty>();
            ProcurementRepo repo = new ProcurementRepo();
            warrantys = repo.GetActiveWarranties();
            lookupWarrentyChild.ItemsSource = warrantys;

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
            lookupPaymentTermsChild.ItemsSource = paymentTerms;


        }
        public void loadSaleOrderStatus()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit closed SaleOrder") != null)
            {
                SaleOrderStatuses = saleOrderRepo.getAllSaleOrderStatus();
            }
            else
                SaleOrderStatuses = saleOrderRepo.getAllActiveSaleOrderStatus();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (SaleOrderStatus status in SaleOrderStatuses)
            {

                cmbitems.Add(new cmbitem() { name = status.Status, id = status.Id, bcolor = status.backcolor, fcolor = "#FF000000" });
                //});
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
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
            cmbStatusChild.ItemsSource = cmbitems;
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
                            
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));
                                sumChild = sumChild + fieldValue;
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
                                var fieldValue = Convert.ToDecimal(e.GetValue("revisedValue"));
                                sumChild = sumChild + fieldValue;
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
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
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
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValue"));
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
                               
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));
                                var adjSCOst = Convert.ToDecimal(e.GetValue("adjSCost"));
                                sumChild = sumChild + fieldValue;
                                break;
                            case CustomSummaryProcess.Finalize:
                                if (sumChild != 0)
                                {
                                    sumChild = Convert.ToDecimal(txtTotalAmountChild.Text) - sumChild;
                                    totalChildSystem = sumChild;

                                }
                                e.TotalValue = sumChild;


                                break;
                        }
                        txtTotalChildSystemCost.Text = totalChildSystem.ToString();
                    }
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
                     
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValue"));

                                sumChild = sumChild + fieldValue;
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
                           
                                var fieldValue = Convert.ToDecimal(e.GetValue("systemValue"));

                                sumChild = sumChild + fieldValue;
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
                                var fieldValue = Convert.ToDecimal(e.GetValue("budgetedValueChild"));
                                sum = sum + fieldValue;
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
                            var percent = (((budgetValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                        }
                    }
                    else
                if (e.Column.FieldName == "revisedValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("revisedValue") != null)
                        {
                            decimal revisedValue = Convert.ToDecimal(e.GetListSourceFieldValue("revisedValue"));
                            var percent = (((revisedValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                        }
                    }
                    else
                if (e.Column.FieldName == "SRBCValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("SRBC") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("SRBC"));
                            var percent = (((srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                        }
                    }
                    else
                if (e.Column.FieldName == "systemValuePerc" && e.IsGetData)
                    {
                        if (e.GetListSourceFieldValue("systemValue") != null)
                        {
                            decimal srbcValue = Convert.ToDecimal(e.GetListSourceFieldValue("systemValue"));


                            var percent = (((srbcValue) / totalSO) * 100);
                            e.Value = decimal.Round(percent, 2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            costChildFieldValue.Clear();
            loadIncoterms();
            loadWarrantys();
            loadPaymentTerms();
            loadSaleOrderStatus();
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
            var costfields = costfieldValues.Where(x => x.actualValue != 0 || x.SRBC != 0 || x.budgetedValue != 0 || x.revisedValue != 0 /*&& x.systemPayment != 0*/ && x.systemValue <= 0 && x.vendor == null).ToList();
            grdChildCostItems.ItemsSource = costfields;
        }
        public List<CostFieldValues> GetCostSheetChildValues(SaleOrder saleOrder)
        {
            SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
            List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
            List<CostFieldValues> costfields = new List<CostFieldValues>();
            costSheetBillFieldsChild = new List<CostSheetBillField>();
            costSheetPOFieldsChild = new List<CostSheetPOField>();
            costSheetSIFieldsChild = new List<CostSheetSIField>();
            costSheetSOFieldsChild = new List<CostSheetSOField>();
            costSheetSaleReceiptFieldsChild = new List<CostSheetSaleReceiptField>();
            costSheetPaymentFieldsChild = new List<CostSheetPaymentField>();
            costfieldValues = grdChildCostItems.ItemsSource as List<CostFieldValues>;
            if (saleOrder.Id != 0 && saleOrder.CostSheet_Id != 0)
            {
                costSheetSOFieldsChild = saleOrderRepo.GetCostSheetSOFields(saleOrder.Id, (int)saleOrder.CostSheet_Id);
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
                            if (costSheetSOFieldsChild.Count != 0)
                            {
                                var dbField = costSheetSOFieldsChild.FirstOrDefault(x => x.FieldId == item.Id && x.FieldType == CostFieldType.soAmountSRBC && x.CostSheetId == saleOrder.CostSheet_Id && x.SO_Id == saleOrder.Id);
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



                    }
                costfields.Add(item);

            }
            return costfields;
        }

        private void childView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {

        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
