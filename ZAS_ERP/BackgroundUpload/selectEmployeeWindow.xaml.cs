using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using ERP_BL.BackgroundImages;
using ERP_BL.Databases;
using ERP_BL.Enums;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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


namespace ZAS_ERP.BackgroundUpload
{
    /// <summary>
    /// Interaction logic for selectEmployeeWindow.xaml
    /// </summary>
   
    public partial class selectEmployeeWindow : DXWindow 
    {
       
      
        List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        DepartmentRepo deptRepo = new DepartmentRepo();
        public bool isCancle;
        public string empId;
       
        public selectEmployeeWindow()
        {

            InitializeComponent();
            grdUserTask.SelectionChanged += OnGridSelectionChanged;
            treeListEmployeeView.NodeCheckStateChanged += OnNodeCheckStateChanged;
        }

        private void OnNodeCheckStateChanged(object sender, TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdUserTask.SelectItem(e.Node.RowHandle);
            else
                grdUserTask.UnselectItem(e.Node.RowHandle);
        }

        private void OnGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)grdUserTask.View;
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
                    var selectedRows = grdUserTask.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loademployees();
        }
        public void loademployees()
        {
            employees = employeeRepo.GetAllActiveEmployeesForBackgroundImage();
            grdUserTask.ItemsSource = employees;
        }

      
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            empId = "";
            if (grdUserTask.SelectedItems.Count != 0)
            {
                isCancle = true;
                foreach (ERP_BL.Databases.Employee Employ in grdUserTask.SelectedItems)
                {
                    if (String.IsNullOrEmpty(empId))
                    {
                        empId =  Employ.EmpId.ToString();
                    }
                    else
                    {
                        empId = empId + "," + Employ.EmpId.ToString();
                    }               
                }

            }
            else
            {
                MessageBox.Show("Please select Employee First!");
                return;
            }
            this.Close();
               
        }

        private void treeListEmployeeView_CustomUnboundColumnData(object sender, TreeListUnboundColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CoreDept" && e.IsGetData)
            {
                var emp = grdUserTask.GetRow(e.Node.RowHandle) as ERP_BL.Databases.Employee;
                string deptNames = "";
                if (emp.CoreDepartment != null)
                {
                    var coreDept = emp.CoreDepartment;
                    deptNames = coreDept.DeptName;
                    while(coreDept.parentDepartment != null)
                    {
                        coreDept = coreDept.parentDepartment;
                        deptNames = coreDept.DeptName+" | " + deptNames;
                    }

                }
                e.Value = deptNames;
            }

            //if (e.Column.FieldName == "companiess" && e.IsGetData)
            //{
            //    var cmpy = grdUserTask.GetRow(e.Node.RowHandle) as ERP_BL.Databases.Employee;
            //    string cmpyNames = "";

            //    if (cmpy.Companies != null && cmpy.Companies.Count > 0)
            //    {
            //        cmpyNames = String.Join(" | ", cmpy.Companies.Select(x => x.CompanyName));
            //    }
            //    e.Value = cmpyNames;
            //}
        }

        private void GrdUserTask_FilterChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
