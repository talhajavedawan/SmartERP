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
using ZAS_ERP.Procurementss.Budget.UserControls;

namespace ZAS_ERP.Procurementss.Budget
{
    /// <summary>
    /// Interaction logic for winBudgetRegister.xaml
    /// </summary>
    public partial class winBudgetRegister : DXWindow
    {
        frmBudgetAdd frmBudgetAdd = new frmBudgetAdd();

        List<BudgetCostSheet> budgets = new List<BudgetCostSheet>();
        List<cmbitem> TreeItems = new List<cmbitem>();
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();

        ucBudgetGrid budgetRegister = new ucBudgetGrid();
        public string transctions;
        public winBudgetRegister()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ucBudgetGrid.statusId = 0;
                ucBudgetGrid.AllActive = 1;
                faRightGrid.Children.Add(budgetRegister);
                cmbitem treeItem1 = new cmbitem() { name = "Budget" };
                cmbitem treeItema = new cmbitem() { name = "Budget(Open)" };
                var statusList = repo.getAllBudgetCostStatus();
                ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                foreach (BudgetCostSheetStatus status in statusList.Where(x => x.isActive == true).ToList())
                {
                    cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Budget" });
                }
                treeItema.Items = cmbItemsa;
                treeItem1.Items.Add(treeItema);
                cmbitem treeItemb = new cmbitem() { name = "Budget(Closed)" };

                ICollection<cmbitem> cmbItems = new List<cmbitem>();
                foreach (BudgetCostSheetStatus status in statusList.Where(x => x.isActive == false).ToList())
                {
                    cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Budget" });
                }
                treeItemb.Items = cmbItems;
                treeItem1.Items.Add(treeItemb);
                TreeItems.Add(treeItem1);
                
                treeViewBudgetStatus.ItemsSource = TreeItems;
            }
            catch
            {

            }
        }
        private void treeViewBudgetStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            budgets = new List<BudgetCostSheet>();
            repo = new BudgetCostCenterRepo();
            var item = (cmbitem)treeViewBudgetStatus.SelectedItem;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Budget") != null)
            {
                if (item.name == "Budgets")
                {
                    budgets = repo.GetAllOpenAndClosed(MainWindow.currentUserid);
                }
                else if (item.name == "Budget(Open)")
                {
                    budgets = repo.getAllActiveandUnapprovedTransactions(MainWindow.currentUserid);
                }
                else if (item.name == "Budget(Closed)")
                {
                    budgets = repo.getAllInActiveandUnapproved(MainWindow.currentUserid);
                }
                else
                {
                    budgets = repo.getAllFirstPobyStatusId(MainWindow.currentUserid, item.id);
                }
                budgetRegister.grdbudget.ItemsSource = budgets;
                budgetRegister.lblHeading.Text = item.name;
            }
            else
            {
                budgetRegister.grdbudget.ItemsSource = null;
                budgetRegister.lblHeading.Text = item.name;
            }
        }
        private void addBudgetBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget") != null)
            {
                frmBudgetAdd frmBudgetAdd = new frmBudgetAdd();
                frmBudgetAdd.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add Budget!");
            }
        }

        private void GrdCollapse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Collapsed;
            GridSplitter.Visibility = Visibility.Collapsed;
            faRightGrid.SetValue(Grid.ColumnProperty, 0);

            faRightGrid.SetValue(Grid.ColumnSpanProperty, 3);

        }

        private void GrdExpand_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Visible;
            GridSplitter.Visibility = Visibility.Visible;

            faRightGrid.SetValue(Grid.ColumnProperty, 2);
            faRightGrid.SetValue(Grid.ColumnSpanProperty, 1);
        }
    }
}
