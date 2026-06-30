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
    /// Interaction logic for winSelectBudget.xaml
    /// </summary>
    public partial class winSelectBudget : DXWindow
    {
        public int compId, deptId, budgetId;
        public BudgetCostSheet budget = new BudgetCostSheet();
        public winSelectBudget()
        {
            InitializeComponent();
        }
        public winSelectBudget(int _compId, int _deptId)
        {
            InitializeComponent();
            compId = _compId;
            deptId = _deptId;
        }
        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {

            if (grdbudget.SelectedItems.Count == 1)
            {
                var budget = grdbudget.SelectedItem as BudgetCostSheet;

                if (budget != null)
                {
                    budgetId = budget.Id;
                    this.Close();
                }
                else
                {
                    DXMessageBox.Show("Please select Budget", "Information");
                    return;
                }
            }
            else
            {
                
                DXMessageBox.Show("You can not attach with 2 budgets at a time", "Information");
                return;
            }
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
            var budgets = repo.getByCompanyDepartment(compId, deptId);
            grdbudget.ItemsSource = budgets;
            if (budget.Id!=0)
            {
                grdbudget.SelectedItem = (grdbudget.ItemsSource as List<BudgetCostSheet>).FirstOrDefault(x=>x.Id==budget.Id);
            }
        }
    }
}
