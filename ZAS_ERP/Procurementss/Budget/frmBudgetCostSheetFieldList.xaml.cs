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
    /// Interaction logic for frmBudgetCostSheetFieldList.xaml
    /// </summary>
    public partial class frmBudgetCostSheetFieldList : DXWindow
    {
        List<BudgetSheetHead> CostSheetFields = new List<BudgetSheetHead>();
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
        List<BudgetSheetHead> Fields = new List<BudgetSheetHead>();
        BudgetSheetHead Field = new BudgetSheetHead();
        public frmBudgetCostSheetFieldList()
        {
            InitializeComponent();
        }

        private void winBudgetCostSheetFieldsList_Loaded(object sender, RoutedEventArgs e)
        {
            loadCostSheetField();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdBudgetCostSheetField);
        }

        private void WinBudgetCostSheetFieldsList_Loaded_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdBudgetCostSheetField);
        }

        private void mbtnEditCostSheetField_Click(object sender, RoutedEventArgs e)
        {
            frmBudgetCostSheetFieldAdd.costSheetFieldId = (grdBudgetCostSheetField.SelectedItem as BudgetSheetHead).Id;
            newCostSheetField();
        }

        private void mbtnNewCostSheetField_Click(object sender, RoutedEventArgs e)
        {
            newCostSheetField();
        }

        private void grdBudgetCostSheetField_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        public void loadCostSheetField()
        {
            if (MainWindow.currentUserid == 0)
                CostSheetFields = repo.getallCostSheetField();
            else
                CostSheetFields = repo.getActiveCostSheetFields();

            this.grdBudgetCostSheetField.ItemsSource = CostSheetFields;
        }
        public void newCostSheetField()
        {
            frmBudgetCostSheetFieldAdd productCostSheetFieldAdd = new frmBudgetCostSheetFieldAdd();
            productCostSheetFieldAdd.ShowDialog();
            loadCostSheetField();
        }
        public void newItem()
        {
            frmBudgetCostSheetFieldAdd frmItemadd = new frmBudgetCostSheetFieldAdd();
            frmItemadd.ShowDialog();
            loadCostSheetField();
            //loadItemgrid();
        }
        private void mbtnNewItem_Click(object sender, RoutedEventArgs e)
        {
            newItem();

        }

        private void grdCostSheetField_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmBudgetCostSheetFieldAdd.costSheetFieldId = (grdBudgetCostSheetField.SelectedItem as BudgetSheetHead).Id;
            newCostSheetField();
        }

    }
}