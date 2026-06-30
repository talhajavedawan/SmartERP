using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.SharedReports
{
    /// <summary>
    /// Interaction logic for ucSelectCompanyandDepartment.xaml
    /// </summary>
    public partial class ucSelectCompanyandDepartment : UserControl
    {
        public ucSelectCompanyandDepartment()
        {
            InitializeComponent();
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdCompany.SelectionChanged += OnGridSelectionChanged;
            viewTableDepartment.NodeCheckStateChanged += OnNodeCheckStateChangedDepartment;
            grdDepartment.SelectionChanged += OnGridSelectionChangedDepartment;
        }

       

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCompany.ItemsSource = SYSTEM_STATIC.LoadCurrentUserCompanies();
        }

        private void GrdDepartment_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            SharedReportsStatic.selectedDepartments = grdDepartment.SelectedItems as List<Department>;
        }

        private void GrdCompany_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            List<Department> departments = new List<Department>();

            if (grdCompany.SelectedItems != null)
            {
                foreach (Company company in grdCompany.SelectedItems)
                {
                    departments.AddRange(company.departments.Distinct());
                }
                grdDepartment.ItemsSource = departments.Distinct();

            }
            SharedReportsStatic.selectedCompanies = grdCompany.SelectedItems as List<Company>; 

        }
        private void OnGridSelectionChangedDepartment(object sender, GridSelectionChangedEventArgs e)
        {
            var viewDepartment = (TreeListView)grdDepartment.View;
            var nodeDepartment = viewDepartment.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (nodeDepartment != null)
                        nodeDepartment.IsChecked = true;
                    break;
                case CollectionChangeAction.Remove:
                    if (nodeDepartment != null)
                        nodeDepartment.IsChecked = false;
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = grdDepartment.GetSelectedRowHandles();
                    viewDepartment.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        nodeDepartment = viewDepartment.GetNodeByRowHandle(rowHandle);
                        nodeDepartment.IsChecked = true;
                    }
                    break;
            }
        }

        private void OnNodeCheckStateChangedDepartment(object sender, TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdDepartment.SelectItem(e.Node.RowHandle);
            else
                grdDepartment.UnselectItem(e.Node.RowHandle);
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
    }
}
