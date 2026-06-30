using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.Budget;
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

namespace ZAS_ERP.Procurementss.Budget
{
    /// <summary>
    /// Interaction logic for winAddBudgetSystemCost.xaml
    /// </summary>
    public partial class winAddBudgetSystemCost : DXWindow
    {
        public static List<BudgetSystemCostField> costSheetFields = new List<BudgetSystemCostField>();
        int budgetId;
        public bool editFlag=false;
        double amountSOC = 0;
        TransactionItemType type = TransactionItemType.Sale_Invoice;
        BudgetCostCenterRepo budgetCostRepo = new BudgetCostCenterRepo();
        SaleOrder saleOrder = new SaleOrder();
        PurchaseOrder purchaseOrder = new PurchaseOrder();
        SalesReceipt salesReceipt = new SalesReceipt();
        List<SaleOrder> saleOrders = new List<SaleOrder>();
        List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

        public winAddBudgetSystemCost()
        {
            InitializeComponent();
        }
        public winAddBudgetSystemCost(int _budgetId, bool _editFlag, double _amountSOC,SaleOrder _saleOrder)
        {
            InitializeComponent();
            budgetId = _budgetId;
            editFlag = _editFlag;
            amountSOC = _amountSOC;
            saleOrder = _saleOrder;
        }
        public winAddBudgetSystemCost(int _budgetId, bool _editFlag, double _amountSOC, SalesReceipt _saleReceipt, SaleOrder _saleOrder)
        {
            InitializeComponent();
            budgetId = _budgetId;
            editFlag = _editFlag;
            amountSOC = _amountSOC;
            salesReceipt = _saleReceipt;
            saleOrder = _saleOrder;
        }
        public winAddBudgetSystemCost(int _budgetId, bool _editFlag, double _amountSOC, PurchaseOrder _purchaseOrder)
        {
            InitializeComponent();
            budgetId = _budgetId;
            editFlag = _editFlag;
            amountSOC = _amountSOC;
            purchaseOrder = _purchaseOrder;
        }
        private void BtnViewHistory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {


            if (Convert.ToDouble(txtDifference.Text) != 0)
            {
                DXMessageBox.Show("There is a remaining difference", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            else
            {
                if (editFlag == false)
                {
                    costSheetFields.ForEach(x => x.BudgetSheettHead = null);
                }
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(editFlag==false)
            {
                costSheetFields.Clear();
                var budget= budgetCostRepo.get(budgetId);
                foreach(var field in budget.budgetCostFields)
                {
                    BudgetSystemCostField systemCost = new BudgetSystemCostField();
                    systemCost.Head_Id = field.Head_Id;
                    systemCost.BudgetSheettHead = field.BudgetSheettHead;
                    systemCost.RSBC = field.RSBC;
                    systemCost.BudgetedCost = field.BudgetedCost;
                    costSheetFields.Add(systemCost);
                }
                grdBudgetCostItems.ItemsSource = costSheetFields;
                txtTotalSOAmount.Text = amountSOC.ToString();
            }
            else
            {
                grdBudgetCostItems.ItemsSource = costSheetFields;
                txtTotalSOAmount.Text = amountSOC.ToString();

            }
        }

        private void txtTotalSOAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            getDifference();

        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "addedSystemCost")
            {
                getDifference();
            }

        }
        public void getDifference()
        {
            double total = 0;
            var selectedItems = grdBudgetCostItems.ItemsSource as List<BudgetSystemCostField>;
            foreach (var item in selectedItems)
            {
                total = total + item.addedSystemCost;
            }
            txtTotalSystemCost.Text = total.ToString();
            txtDifference.Text = (amountSOC - total).ToString();
        }

        private void grdBudgetCostItems_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
                switch (e.Column.FieldName)
                {
                    case "systemCost":
                        double totalSystemCost = 0;
                        var SO = grdBudgetCostItems.GetRowByListIndex(e.ListSourceRowIndex) as BudgetSystemCostField;
                        SaleOrderRepo repo = new SaleOrderRepo();
                        saleOrders= repo.GetSOBYBudgetId(budgetId);
                        purchaseOrders = repo.GetPOBYBudgetId(budgetId);
                        foreach (var saleOrder in saleOrders)
                        {
                            if (saleOrder.Id != 0)
                            {
                                foreach (var invoice in saleOrder.SaleInvoices)
                                {
                                    if (invoice.BudgetSystemCostFields.Count != 0)
                                    {
                                        totalSystemCost += invoice.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == SO.Head_Id).addedSystemCost;
                                    }
                                    foreach (var receipt in invoice.salesReceipts)
                                    {
                                        if (receipt.BudgetSystemCostFields.Count != 0)
                                        {
                                            totalSystemCost += receipt.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == SO.Head_Id).addedSystemCost;
                                        }
                                    }
                                }
                                foreach (var bill in saleOrder.Bills)
                                {
                                    if (bill.BudgetSystemCostFields.Count != 0)
                                    {
                                        totalSystemCost += bill.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == SO.Head_Id).addedSystemCost;
                                    }
                                }
                            }
                        }
                        foreach (var purchaseOrder in purchaseOrders)
                        {
                            if (purchaseOrder.Id != 0)
                            {
                                foreach (var invoice in purchaseOrder.PurchaseInvoices)
                                {
                                    if (invoice.BudgetSystemCostFields.Count != 0)
                                    {
                                        totalSystemCost += invoice.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == SO.Head_Id).addedSystemCost;
                                    }
                                    foreach (var payment in invoice.Payments)
                                    {
                                        if (payment.BudgetSystemCostFields.Count != 0)
                                        {
                                            totalSystemCost += payment.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == SO.Head_Id).addedSystemCost;
                                        }
                                    }
                                }
                                foreach (var bill in purchaseOrder.Bills)
                                {
                                    if (bill.BudgetSystemCostFields.Count != 0)
                                    {
                                        totalSystemCost += bill.BudgetSystemCostFields.FirstOrDefault(x => x.Head_Id == SO.Head_Id).addedSystemCost;
                                    }
                                }
                            }
                        }

                        e.Value = totalSystemCost;
                        break;
                }
        }
    }
}
