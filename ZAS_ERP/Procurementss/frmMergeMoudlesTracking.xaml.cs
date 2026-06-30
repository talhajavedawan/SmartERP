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
using ZAS_ERP.Procurementss.CostSheet.UserControls;
using ZAS_ERP.Procurementss.SaleOrderss;

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for frmMergeMoudlesTracking.xaml
    /// </summary>
    public partial class frmMergeMoudlesTracking : DXWindow
    {
        ProcurementRepo repo = new ProcurementRepo();
        public bool isParent = false;
        public int Id = 0;
        public ERP_BL.Enums.TransactionItemType Type = TransactionItemType.Inquiry;
        decimal sum = 0;
        decimal sumChild = 0;
        decimal totalChildBudget = 0;
        decimal totalParentBudget = 0;

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
        public List<CostFieldValues> costParentFieldValue = new List<CostFieldValues>();
        List<SaleOrderStatus> SaleOrderStatuses = new List<SaleOrderStatus>();

        SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        //SaleOrderRepo saleOrderRepo = new SaleOrderRepo();
        public frmMergeMoudlesTracking()
        {
            InitializeComponent();
        }
        public frmMergeMoudlesTracking(int id, TransactionItemType type, bool _isParent)
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
                                //bool shouldSum = (bool)view.GetRowCellValue(e.RowHandle, "budgetedValue");
                                //if (shouldSum)
                                //{
                                var fieldValue = Convert.ToDecimal(e.GetValue("actualValue"));
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
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadSaleOrderStatus();
            List<AllTransactionsView> allTransactions = new List<AllTransactionsView>();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Transactions in Tracking 360 Window") != null)
            {
                if (isParent != true)
                {
                    tabTransactions.Visibility = Visibility.Visible;

                   
                    allTransactions = repo.getAllTransactions(Id, Type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.id);
            
                    grdTransactionListing.ItemsSource = allTransactions;
                }
                else
                {

                    allTransactions = repo.getLinkedSOs(Id, Type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.employeeId);


                    grdTransactionListing.ItemsSource = allTransactions;

                }
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View CostSheet in Tracking 360 Window") != null)
            {
                try
                {
                    List<ucMergeChildCostSheets> childModels = new List<ucMergeChildCostSheets>();

                    if (isParent == true)
                    {
                        var saleOrders = repo.getLinkedSOSForCostSheets(Id, Type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.employeeId);

                        if(saleOrders.Count>2)
                        {
                            tabMergeCostSheet.IsEnabled = false;
                        }
                        else
                            if(saleOrders.Count==2)
                        {
                            tabCostSheet.IsEnabled = false;
                        }
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
                                ucMergeChildCostSheets model = new ucMergeChildCostSheets(SO);

                                DXTabItem tabItem = new DXTabItem();
                                Grid grid = new Grid();
                                tabItem.Header = "Child SO ("+SO.SalesReferenceNo+ ")";
                                grid.Children.Add(model);
                                tabItem.Content = grid;
                                grdChildSO.Items.Add(tabItem);


                                //childModels.Add(model);
                            }
                        }
                        //grdChildSO.ItemsSource = childModels;


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ucAccountingComparison comparison = new ucAccountingComparison(Id, TransactionItemType.Sale_Order, true);
                gridMergeCostSheet.Children.Add(comparison);
            }
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

        }
        public void loadWarrantys()
        {
            List<Warranty> warrantys = new List<Warranty>();
            ProcurementRepo repo = new ProcurementRepo();
            warrantys = repo.GetActiveWarranties();
            lookupWarrenty.ItemsSource = warrantys;

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
            if(!string.IsNullOrEmpty(txtTotalBudgetMargin.Text) && !string.IsNullOrEmpty(txtTotalParentAmount.Text))
            {
                decimal totalMargin = Convert.ToDecimal(txtTotalBudgetMargin.Text);
                decimal totalSOAmount = Convert.ToDecimal(txtTotalParentAmount.Text);
                if (totalSOAmount != 0)
                {
                    decimal totalValue = totalMargin / totalSOAmount * 100;
                    txtTotalBudgetMarginPerc.Text = totalValue.ToString();
                }
            }
        }

        private void txtTotalSystemMargin_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTotalSystemMargin.Text) && !string.IsNullOrEmpty(txtTotalParentAmount.Text))
            {
                decimal totalMargin = Convert.ToDecimal(txtTotalSystemMargin.Text);
                decimal totalSOAmount = Convert.ToDecimal(txtTotalParentAmount.Text);
                if (totalSOAmount != 0)
                {
                    decimal totalValue = totalMargin / totalSOAmount * 100;
                    txtTotalSystemMarginPerc.Text = totalValue.ToString();
                }
            }
        }

        private void tabCostSheet_MouseUp(object sender, MouseButtonEventArgs e)
        {
            decimal totalBudget = 0;
            decimal totalSystemBudget = 0;

            foreach (var item in grdChildSO.Items)
            {
                // Ensure item is a DXTabItem
                if (item is DXTabItem tabItem && tabItem.Content is Grid grid)
                {
                    // Find the user control in the Grid
                    var userControl = grid.Children.OfType<ucMergeChildCostSheets>().FirstOrDefault();
                    if (userControl != null)
                    {
                        var childB = userControl.grdChildCostItems.Columns["budgetedValue"].TotalSummaries[0].Value;
                        var childSystemBudget = userControl.grdChildCostItems.Columns["systemValue"].TotalSummaries[0].Value;

                        // Ensure text fields are not empty before adding
                        if (!string.IsNullOrEmpty(txtTotalParentAmount.Text) &&
                            !string.IsNullOrEmpty(userControl.txtTotalChildBudgetedCost.Text) &&
                            !string.IsNullOrEmpty(txtTotalParentBudgetedCost.Text))
                        {
                            totalBudget += Convert.ToDecimal(childB);
                        }

                        if (!string.IsNullOrEmpty(txtTotalParentAmount.Text) &&
                            !string.IsNullOrEmpty(userControl.txtTotalChildSystemCost.Text) &&
                            !string.IsNullOrEmpty(txtTotalParentSystemCost.Text))
                        {
                            totalSystemBudget += Convert.ToDecimal(childSystemBudget);
                        }
                    }
                }
            }

            // Add parent cost values
            var parentBudget = grdParentCostItems.Columns["budgetedValue"].TotalSummaries[0].Value;
            var parentSystemBudget = grdParentCostItems.Columns["systemValue"].TotalSummaries[0].Value;

            totalBudget += Convert.ToDecimal(parentBudget);
            totalSystemBudget += Convert.ToDecimal(parentSystemBudget);

            // Update the text fields with calculated values
            txtTotalBudgetMargin.Text = totalBudget.ToString();
            txtTotalSystemMargin.Text = totalSystemBudget.ToString();
        }
    }
}
