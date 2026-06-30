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
    /// Interaction logic for ucSharedCompDepUser.xaml
    /// </summary>
    public partial class ucSharedCompDepUser : UserControl
    {
        List<int> deptIds = new List<int>();
        public ucSharedCompDepUser()
        {
            InitializeComponent();
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdCompany.SelectionChanged += OnGridSelectionChanged;
            viewTableDepartment.NodeCheckStateChanged += OnNodeCheckStateChangedDepartment;
            grdDepartment.SelectionChanged += OnGridSelectionChangedDepartment;
            viewTableEmployee.NodeCheckStateChanged += OnNodeCheckStateChangedEmployee;
            grdEmployee.SelectionChanged += OnGridSelectionChangedEmployee;
        }
        private void OnGridSelectionChangedEmployee(object sender, GridSelectionChangedEventArgs e)
        {
            var viewEmployee = (TreeListView)grdEmployee.View;
            var nodeEmployee = viewEmployee.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (nodeEmployee != null)
                        nodeEmployee.IsChecked = true;
                    break;
                case CollectionChangeAction.Remove:
                    if (nodeEmployee != null)
                        nodeEmployee.IsChecked = false;
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = grdEmployee.GetSelectedRowHandles();
                    viewEmployee.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        nodeEmployee = viewEmployee.GetNodeByRowHandle(rowHandle);
                        nodeEmployee.IsChecked = true;
                    }
                    break;
            }
        }

        private void OnNodeCheckStateChangedEmployee(object sender, TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdEmployee.SelectItem(e.Node.RowHandle);
            else
                grdEmployee.UnselectItem(e.Node.RowHandle);
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

        private void OnNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdCompany.SelectItem(e.Node.RowHandle);
            else
                grdCompany.UnselectItem(e.Node.RowHandle);

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
        private void GrdCompany_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            List<Department> departments = new List<Department>();
            List<Company> companies = new List<Company>();
            if (grdCompany.SelectedItems != null)
            {
                foreach (Company company in grdCompany.SelectedItems)
                {
                    foreach (Department department in company.departments)
                    {
                        if (deptIds.Contains(department.Id))
                        {
                            departments.Add(department);
                        }
                    }
                    //departments.AddRange(company.departments.Distinct());
                }
            }
            grdDepartment.ItemsSource = departments.Distinct();
            SharedReportsStatic.departments = departments.Distinct().ToList();
            if (grdCompany.SelectedItems != null)
            {
                foreach (Company company in grdCompany.SelectedItems)
                {
                    companies.Add(company);
                }
            }
            SharedReportsStatic.selectedCompanies=(companies.Distinct().ToList());

        }
        private void GrdDepartment_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            List<Department> departments = new List<Department>();

            List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();

            if (grdDepartment.SelectedItems != null)
            {
                foreach (Department department in grdDepartment.SelectedItems)
                {
                    employees.AddRange(department.employees.Distinct());
                    departments.Add(department);
                }
                grdEmployee.ItemsSource = employees.Distinct();
              
            }
            else
            {
                grdEmployee.ItemsSource = null;
             
                employees.Clear();
            }
            SharedReportsStatic.employees = employees.Distinct().ToList();
 
            SharedReportsStatic.selectedDepartments = departments.Distinct().ToList();
        }

        private void GrdEmployee_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            foreach (ERP_BL.Databases.Employee employee in grdEmployee.SelectedItems)
            {
                employees.Add(employee);
            }

            SharedReportsStatic.selectedEmployees = employees.Distinct().ToList();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
            grdCompany.ItemsSource = SYSTEM_STATIC.LoadCurrentUserCompanies();
            deptIds = SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id).ToList();
        }
    }
}
