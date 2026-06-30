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
    /// Interaction logic for frmBudgetStatusList.xaml
    /// </summary>
    public partial class frmBudgetStatusList : DXWindow
    {
        List<BudgetCostSheetStatus> budgetCostSheetStatuses = new List<BudgetCostSheetStatus>();
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();

        public frmBudgetStatusList()
        {
            InitializeComponent();
        }
        private void winBudgetStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadBudgetStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdBudgetStatus);

        }

        private void WinBudgetStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdBudgetStatus);
        }

        private void mbtnNewBudgetStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget Status") != null)
            {
                newBudgetStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Status!");
            }
        }

        private void mbtnEditBudgetStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budget Status") != null)
            {
                if (grdBudgetStatus.SelectedItem != null)
                {

                    Procurementss.Budget.frmBudgetStatusAdd.StatusId = (grdBudgetStatus.SelectedItem as BudgetCostSheetStatus).Id;

                    newBudgetStatus();
                }
                else
                {
                    MessageBox.Show("Please select a Budget Status to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Update existing Status!");
            }
        }

        private void grdBudgetStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Budget Status") != null)
            {
                Procurementss.Budget.frmBudgetStatusAdd.StatusId = (grdBudgetStatus.SelectedItem as BudgetCostSheetStatus).Id;
                newBudgetStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to Update existing Status!");
            }
        }

        private void btnNewBudgetStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Budget Status") != null)
            {
                newBudgetStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Status!");
            }
        }

        private void btnEditBudgetStatus_Click(object sender, RoutedEventArgs e)
        {

        }
        private void loadBudgetStatus()
        {

            if (MainWindow.currentUserid == 0)
                budgetCostSheetStatuses = repo.getAllBudgetCostStatus();

            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Budget Statuses") != null)
            {
                budgetCostSheetStatuses = repo.getAllBudgetCostStatus();
            }
            else
            {
                budgetCostSheetStatuses = repo.getAllActiveBudgetCostStatus();
            }
            this.grdBudgetStatus.ItemsSource = budgetCostSheetStatuses;
            grdBudgetStatus.Columns.GetColumnByFieldName("Id").Visible = false;
        }

        public void newBudgetStatus()
        {
            frmBudgetStatusAdd frmSaleOrderStatussadd = new frmBudgetStatusAdd();
            frmSaleOrderStatussadd.ShowDialog();

            loadBudgetStatus();
        }
    }
}
