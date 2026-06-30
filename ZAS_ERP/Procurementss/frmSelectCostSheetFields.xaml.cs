using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for frmSelectCostSheetFields.xaml
    /// </summary>
    public partial class frmSelectCostSheetFields : Window
    {
        public SaleOrderRepo repo = new SaleOrderRepo();
        public List<CostSheetField> fieldsIds = new List<CostSheetField>();
        public frmSelectCostSheetFields()
        {
            InitializeComponent();
            viewgrdCostSheetFields.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdCostSheetField.SelectionChanged += OnGridSelectionChanged;
        }
        private void OnNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdCostSheetField.SelectItem(e.Node.RowHandle);
            else
                grdCostSheetField.UnselectItem(e.Node.RowHandle);
        }
        private void OnGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)grdCostSheetField.View;
            var node = view.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (node != null)
                        node.IsChecked = true;
                    break;
                case CollectionChangeAction.Remove:
                    if (node != null)
                        node.IsChecked = false;
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = grdCostSheetField.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }
        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            var fields= grdCostSheetField.SelectedItems as List<CostSheetField>;
            if (fields != null)
            {
                fieldsIds.AddRange(fields);
            }
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var costSheetFields=repo.getActiveCostSheetFields();
            grdCostSheetField.ItemsSource = repo.getActiveCostSheetFields();
        }
    }
}
