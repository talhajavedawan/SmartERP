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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using ERP_BL.Databases;

namespace ZAS_ERP.Procurementss.SharedReports
{
    /// <summary>
    /// Interaction logic for ucSelectReportCompany.xaml
    /// </summary>
    public partial class ucSelectReportCompany : UserControl
    {
        public ucSelectReportCompany()
        {
            InitializeComponent();
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdCompany.SelectionChanged += OnGridSelectionChanged;

        }

        private void OnGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)grdCompany.View;
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
                    var selectedRows = grdCompany.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }

        private void OnNodeCheckStateChanged(object sender, TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdCompany.SelectItem(e.Node.RowHandle);
            else
                grdCompany.UnselectItem(e.Node.RowHandle);
        }

        private void GrdCompany_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            SharedReportsStatic.companies = grdCompany.SelectedItems as List<Company>;
            SharedReportsStatic.selectedCompanies = grdCompany.SelectedItems as List<Company>;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCompany.ItemsSource = SharedReportsStatic.companies;
        }
    }
}
