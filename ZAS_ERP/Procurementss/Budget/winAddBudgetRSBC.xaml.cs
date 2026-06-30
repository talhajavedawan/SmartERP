using DevExpress.Xpf.Core;
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
    /// Interaction logic for winAddBudgetRSBC.xaml
    /// </summary>
    public partial class winAddBudgetRSBC : DXWindow
    {
        int budgetId;
        double amountSOC;
        BudgetCostSheet budget = new BudgetCostSheet();
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
        public winAddBudgetRSBC()
        {
            InitializeComponent();
        }
        public winAddBudgetRSBC(int _budgetId , double _amountSOC)
        {
            InitializeComponent();
            budgetId= _budgetId;
            amountSOC = _amountSOC;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ( txtDifference.Text != 0.ToString())
                {
                    DXMessageBox.Show("There is a remaining difference", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    var source = grdBudgetCostItems.ItemsSource as List<BudgetCostField>;
                    budget.budgetCostFields = source;
                    budget.TotalRSBCIncome = Convert.ToDouble(txtRSBCTotalIncome.Text);
                    budget.TotalRSBCExpense = Convert.ToDouble(txtRSBCTotalExpense.Text);
                    budget.TotalRSBCCGS = Convert.ToDouble(txtRSBCTotalCGS.Text);
                    budget.TotalRSBCGrossProfit = Convert.ToDouble(txtRSBCTotalGrossProfit.Text);
                    budget.TotalRSBCNetProfit = Convert.ToDouble(txtRSBCTotalNetProfit.Text);
                    repo.update(budget);
                    DXMessageBox.Show("Budget has been revised sucessfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                this.Close();
            }
            catch (Exception)
            {
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var row = grdBudgetCostItems.GetFocusedRow() as BudgetCostField;
            row.RSBC = row.addedRSBC;

            if (e.Column.FieldName == "addedRSBC")
            {
                getDifference();
            }
        }
        public void getDifference()
        {
            double total = 0;
            var selectedItems = grdBudgetCostItems.ItemsSource as List<BudgetCostField>;

            if (selectedItems != null)
            {
                foreach (var item in selectedItems)
                {
                    total = total + item.addedRSBC;
                }
                txtTotalRSBC.Text = total.ToString();
                txtDifference.Text = (amountSOC - total).ToString();
            }
            var totalRSBCCgs = selectedItems.Where(x => x.BudgetSheettHead.isCGS == true).Sum(x => x.RSBC);
            var totalRSBCIncome = selectedItems.Where(x => x.BudgetSheettHead.isIncome == true).Sum(x => x.RSBC);
            var totalRSBCExpense = selectedItems.Where(x => x.BudgetSheettHead.isExpense == true).Sum(x => x.RSBC);
            var grossRSBCProfit = totalRSBCIncome - totalRSBCCgs;
            var netRSBCProfit = grossRSBCProfit - totalRSBCExpense;
            txtRSBCTotalIncome.Text = totalRSBCIncome.ToString();
            txtRSBCTotalCGS.Text = totalRSBCCgs.ToString();
            txtRSBCTotalExpense.Text = totalRSBCExpense.ToString();
            txtRSBCTotalGrossProfit.Text = grossRSBCProfit.ToString();
            txtRSBCTotalNetProfit.Text = netRSBCProfit.ToString();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            budget=repo.get(budgetId);
            grdBudgetCostItems.ItemsSource = budget.budgetCostFields;
            txtTotalSOAmount.Text = amountSOC.ToString();

        }
        private void BtnViewHistory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtTotalSOAmount_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            getDifference();
        }
    }
}
