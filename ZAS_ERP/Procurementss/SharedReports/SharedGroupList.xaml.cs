using DevExpress.Xpf.Core;
using ERP_BL.Reports;
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

namespace ZAS_ERP.Procurementss.SharedReports
{
    /// <summary>
    /// Interaction logic for SharedGroupList.xaml
    /// </summary>
    public partial class SharedGroupList : ThemedWindow
    {
        List<int> deptIds = new List<int>();
        List<int> compIds = new List<int>();
        List<int> empIds = new List<int>();
        List<SharedGridGroup> groupsWithCompany = new List<SharedGridGroup>();
        List<SharedGridGroup> groupsWithDepartment = new List<SharedGridGroup>();
        List<SharedGridGroup> groupsWithEmployee = new List<SharedGridGroup>();
        public SharedGroupList()
        {
            InitializeComponent();
        }
        private void ThemedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bypass shared group permissions") == null)
            {
                barByPassGroups.IsVisible = false;
            }
            compIds = SYSTEM_STATIC.LoadCurrentUserCompanies().Select(x => x.Id).ToList();
            deptIds = SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id).ToList();
            GridReportRepo repo = new GridReportRepo();
            var allsahredGroups = repo.GetAllSharedGroups();
            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true).ToList();
            foreach (var group in allsahredGroups)
            {
                if (group.Companies.Count != 0)
                {
                    foreach (var company in group.Companies)
                    {
                        if (compIds.Contains(company.Id))
                        {
                            groupsWithCompany.Add(group);
                            break;
                        }
                    }
                }
                else
                    groupsWithCompany.Add(group);
            }
            foreach (var group in groupsWithCompany)
            {
                if (group.Departments.Count != 0)
                {
                    foreach (var department in group.Departments)
                    {
                        if (deptIds.Contains(department.Id))
                        {
                            groupsWithDepartment.Add(group);
                            break;
                        }
                    }
                }
                else
                    groupsWithDepartment.Add(group);
            }
            foreach (var group in groupsWithDepartment)
            {
                if (group.Employees.Count != 0)
                {
                    empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                    foreach (var department in group.Departments)
                    {
                        if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                        {
                            groupsWithEmployee.Add(group);
                            break;
                        }
                    }
                }
                else
                    groupsWithEmployee.Add(group);
            }
            grdSahredGroups.ItemsSource = groupsWithEmployee;
        }

        private void BarRefreshRates_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            groupsWithCompany.Clear();
            groupsWithDepartment.Clear();
            groupsWithEmployee.Clear();
            GridReportRepo repo = new GridReportRepo();
            var allsahredGroups = repo.GetAllSharedGroups();
            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true).ToList();
            foreach (var group in allsahredGroups)
            {
                if (group.Companies.Count != 0)
                {
                    foreach (var company in group.Companies)
                    {
                        if (compIds.Contains(company.Id))
                        {
                            groupsWithCompany.Add(group);
                            break;
                        }
                    }
                }
                else
                    groupsWithCompany.Add(group);
            }
            foreach (var group in groupsWithCompany)
            {
                if (group.Departments.Count != 0)
                {
                    foreach (var department in group.Departments)
                    {
                        if (deptIds.Contains(department.Id))
                        {
                            groupsWithDepartment.Add(group);
                            break;
                        }
                    }
                }
                else
                    groupsWithDepartment.Add(group);
            }
            foreach (var group in groupsWithDepartment)
            {
                if (group.Employees.Count != 0)
                {
                    empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                    foreach (var department in group.Departments)
                    {
                        if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                        {
                            groupsWithEmployee.Add(group);
                            break;
                        }
                    }
                }
                else
                    groupsWithEmployee.Add(group);
            }
            grdSahredGroups.ItemsSource = groupsWithEmployee;
            grdSahredGroups.RefreshData();

        }
        private void BarEditGroup_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Shared Report Groups") != null)
            {
                var group = grdSahredGroups.SelectedItem as SharedGridGroup;
                if (group.parentId != null)
                {
                    SharedGroupEdit view = new SharedGroupEdit(group);
                    view.Show();
                }
                else
                {
                    DXMessageBox.Show("You can not edit parent group");
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Edit Shared Report Groups", "Permission Denied!");
            }
        }
        private void BarNewGroup_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Shared Report Groups") != null)
            {
                var parentGroup = grdSahredGroups.SelectedItem as SharedGridGroup;
                if (parentGroup.parentId == null)
                {
                    SharedGridReport view = new SharedGridReport(parentGroup);
                    view.Show();
                }
                else
                {
                    DXMessageBox.Show("You can not add group into child group", "Indformation");
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add Shared Report Groups", "Permission Denied!");
            }
        }
        private void GrdSahredGroups_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Shared Report Groups") != null)
            {
                var group = grdSahredGroups.SelectedItem as SharedGridGroup;
                if (group.parentId != null)
                {
                    SharedGroupEdit view = new SharedGroupEdit(group);
                    view.Show();
                }
                else
                {
                    DXMessageBox.Show("You are not allowed to edit parent group");
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Edit Shared Report Groups", "Permission Denied!");
            }
        }
        private void GrdSahredGroups_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            var sharedGroup=grdSahredGroups.SelectedItem as SharedGridGroup;
            if (sharedGroup != null)
            {
                grdCompany.ItemsSource = sharedGroup.Companies;
                grdDepartment.ItemsSource = sharedGroup.Departments;
                grdEmployee.ItemsSource = sharedGroup.Employees;
            }
        }

        private void BarByPassGroups_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            grdSahredGroups.ItemsSource = repo.GetAllSharedGroups();
            grdSahredGroups.RefreshData();
        }
    }
}
