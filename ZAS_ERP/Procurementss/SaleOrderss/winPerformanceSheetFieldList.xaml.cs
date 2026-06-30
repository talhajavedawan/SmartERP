using DevExpress.Xpf.Core;
using ERP_BL.Databases;
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

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for winPerformanceSheetFieldList.xaml
    /// </summary>
    public partial class winPerformanceSheetFieldList : DXWindow
    {
        List<PerformanceSheetHead> performancesheetfields = new List<PerformanceSheetHead>();
        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
        List<PerformanceSheetHead> Fields = new List<PerformanceSheetHead>();
        PerformanceSheetHead Field = new PerformanceSheetHead();
        public winPerformanceSheetFieldList()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadCostSheetField();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPerformanceSheetField);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdPerformanceSheetField);
        }
        private void mbtnEditCostSheetField_Click(object sender, RoutedEventArgs e)
        {
            if (grdPerformanceSheetField.SelectedItem != null)
            {
                winPerformanceSheetHeadAdd.performanceSheetFieldId = (grdPerformanceSheetField.SelectedItem as PerformanceSheetHead).Id;
                newCostSheetField();
            }
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
                performancesheetfields = repo.getallPerformanceSheetFields();
            else
                performancesheetfields = repo.getActivePerformanceSheetHeads();

            this.grdPerformanceSheetField.ItemsSource = performancesheetfields;
        }
        public void newCostSheetField()
        {
            winPerformanceSheetHeadAdd productCostSheetFieldAdd = new winPerformanceSheetHeadAdd();
            productCostSheetFieldAdd.ShowDialog();
            loadCostSheetField();
        }
        public void newItem()
        {
            winPerformanceSheetHeadAdd frmItemadd = new winPerformanceSheetHeadAdd();
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
            winPerformanceSheetHeadAdd.performanceSheetFieldId = (grdPerformanceSheetField.SelectedItem as PerformanceSheetHead).Id;
            newCostSheetField();
        }
    }
}
