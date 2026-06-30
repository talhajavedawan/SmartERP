using ERP_BL.ToDoTasks.Taskss;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucTaskTypeList.xaml
    /// </summary>
    public partial class ucTaskTypeList : UserControl
    {
        TaskRepo repo = new TaskRepo();
        ucFrmTaskTypeAdd frmTaskTypeAdd = new ucFrmTaskTypeAdd();
        public ucTaskTypeList()
        {
            InitializeComponent();
        }

        private void MbtnAddTaskType_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            ucSelectTaskType frmSelectType = new ucSelectTaskType();

            win.Height = 250;
            win.Width = 500;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Content = frmSelectType;
            win.ShowDialog();
        }

        private void MbtnEditTaskType_Click(object sender, RoutedEventArgs e)
        {
            Window win = new Window();
            repo = new TaskRepo();
            var selectedRow = grdCntrlTaskTypeList.SelectedItem as TaskType;
            if (selectedRow != null)
            {
                frmTaskTypeAdd = new ucFrmTaskTypeAdd();
                frmTaskTypeAdd.taskTypeId = selectedRow.Id;

                if(selectedRow.isProcurementType == true)
                {
                    frmTaskTypeAdd.isProcurementType = true;
                    win.WindowState = WindowState.Maximized;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
                else
                {
                    frmTaskTypeAdd.isProcurementType = false;
                    win.WindowState = WindowState.Maximized;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
                
                frmTaskTypeAdd.editFlag = true;
                win.Content = frmTaskTypeAdd;
                win.ShowDialog();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            repo = new TaskRepo();
            grdCntrlTaskTypeList.ItemsSource = repo.GetAllTaskTypes();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            repo = new TaskRepo();
            var allApplicants = repo.GetAllTaskTypes();

            grdCntrlTaskTypeList.ItemsSource = allApplicants;
        }

        private void GrdCntrlTaskTypeList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                var row = grdCntrlTaskTypeList.GetRowByListIndex(e.ListSourceRowIndex) as TaskType;
                switch (e.Column.FieldName)
                {
                    case "Companies":
                        if (row != null && row.companies != null)
                        {
                            var companies = String.Join(" | ", row.companies.Select(x => x.CompanyName));
                            e.Value = companies;
                        }
                        break;
                    case "Departments":
                        if (row != null && row.departments != null)
                        {
                            var depts = row.departments.Where(x => row.departments.Select(y => y.ParentID).Contains(x.Id) != true);
                            var deptNames = String.Join(" | ", depts.Select(x => x.DeptName));
                            e.Value = deptNames;
                        }
                        break;
                }
            }
        }
    }
}
