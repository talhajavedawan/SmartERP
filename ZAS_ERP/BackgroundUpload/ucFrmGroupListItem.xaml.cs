using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;
using ERP_BL.BackgroundImages;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
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

namespace ZAS_ERP.BackgroundUpload
{
    /// <summary>
    /// Interaction logic for ucFrmGroupListItem.xaml
    /// </summary>
    public partial class ucFrmGroupListItem : UserControl
    {
        BackgroundImagesRepo repo = new BackgroundImagesRepo();
        List<User> selectedUsers = new List<User>();
        public TaskGroupTemplate template;
        public int groupId = 0;
        public bool editFlag = false;
        public static string Title;
        TaskGroups taskGroup = new TaskGroups();
        TaskGroups parentTaskGroup = new TaskGroups();
        List<User> selectedUser = new List<User>(); 
        List<User> allUsers = new List<User>(); 
        public ucFrmGroupListItem()
        {
            InitializeComponent();

            gridCompanyView.NodeCheckStateChanged += OnCompanyGirdNodeCheckStateChanged;
            grdCntrlCompanies.SelectionChanged += OnCompanyGridSelectionChanged;

            griddeptview.NodeCheckStateChanged += OndeptgirdNodeCheckStateChanged;
            gridDepartment.SelectionChanged += OndeptGridSelectionChanged;

            gridUsersView.NodeCheckStateChanged += OnUsersGirdNodeCheckStateChanged;
            grdCntrlUsers.SelectionChanged += OnUsersGridSelectionChanged;
        }

        private void OnUsersGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = gridUsersView;
            var node = view.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (node != null)
                    {
                        node.IsChecked = true;
                        var user = grdCntrlUsers.GetRow(node.RowHandle) as User;
                        if (!selectedUsers.Contains(user))
                            selectedUsers.Add(user);
                    }

                    break;
                case CollectionChangeAction.Remove:
                    if (node != null)
                    {
                        node.IsChecked = false;
                        var user = grdCntrlUsers.GetRow(node.RowHandle) as User;
                        if (selectedUsers.Contains(user))
                            selectedUsers.Remove(user);
                    }
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = grdCntrlUsers.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }

        }

        private void OnUsersGirdNodeCheckStateChanged(object sender, TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
            {
                grdCntrlUsers.SelectItem(e.Node.RowHandle);

                //selectedUsers = new List<User>();
                //foreach (var _user in grdCntrlUsers.SelectedItems)
                //{
                //    selectedUsers.Add(_user as User);
                //}

                //foreach (User _user in selectedUsers)
                //{
                //    grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                //}

            }
            else
            {
                grdCntrlUsers.UnselectItem(e.Node.RowHandle);

                //selectedUsers = new List<User>();
                //foreach (var _user in grdCntrlUsers.SelectedItems)
                //{
                //    selectedUsers.Add(_user as User);
                //}

                //foreach (User _user in selectedUsers)
                //{
                //    grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                //}

            }
        }

        private void OndeptGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = griddeptview;
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
                    var selectedRows = gridDepartment.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }

        private void OndeptgirdNodeCheckStateChanged(object sender, TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
            {
                gridDepartment.SelectItem(e.Node.RowHandle);
                if (chkHasParent.IsChecked == true)
                {

                    List<ERP_BL.Databases.Employee> employeess = new List<ERP_BL.Databases.Employee>();
                    foreach (var _dept in gridDepartment.SelectedItems)
                    {
                        employeess.AddRange((_dept as Department).employees.Where(x => x.isActive == true).ToList());
                    }
                    employeess = employeess.Distinct().ToList();

                    List<Company> Companiesss = new List<Company>();
                    foreach (var _comp in grdCntrlCompanies.SelectedItems)
                    {
                        Companiesss.Add(_comp as Company);
                    }

                    var empIdss = employeess.Where(x => x.Companies.Intersect(Companiesss) != null).Select(y => y.EmpId).ToList();
                    grdCntrlUsers.ItemsSource = null;
                    
                    List<User> userr = new List<User>();
                    var emppp = repo.getAllusersByEmpIds(empIdss);
                    foreach (var _user in emppp)
                    {
                        if (_user.taskGroups.Contains(parentTaskGroup))
                        {
                            userr.Add(_user);
                        }
                    }
                    grdCntrlUsers.ItemsSource = userr;
                    foreach (User _user in selectedUsers)
                    {
                        if (_user != null)
                            grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                    }
                    return;
                }


                List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                foreach (var _dept in gridDepartment.SelectedItems)
                {
                    employees.AddRange((_dept as Department).employees.Where(x => x.isActive == true).ToList());
                }
                employees = employees.Distinct().ToList();

                List<Company> Companiess = new List<Company>();
                foreach (var _comp in grdCntrlCompanies.SelectedItems)
                {
                    Companiess.Add(_comp as Company);
                }
                var empIds = employees.Where(x => x.Companies.Intersect(Companiess) != null).Select(y => y.EmpId).ToList();
                grdCntrlUsers.ItemsSource = repo.getAllusersByEmpIds(empIds);
 
                foreach (User _user in selectedUsers)
                {
                    if (_user != null)
                        grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                }

            }
            else
            {
                gridDepartment.UnselectItem(e.Node.RowHandle);
                if (chkHasParent.IsChecked == true)
                {
                    List<ERP_BL.Databases.Employee> employeess = new List<ERP_BL.Databases.Employee>();
                    foreach (var _dept in gridDepartment.SelectedItems)
                    {
                        employeess.AddRange((_dept as Department).employees.Where(x => x.isActive == true).ToList());
                    }
                    employeess = employeess.Distinct().ToList();

                    List<Company> Companiesss = new List<Company>();
                    foreach (var _comp in grdCntrlCompanies.SelectedItems)
                    {
                        Companiesss.Add(_comp as Company);
                    }

                    var empIdss = employeess.Where(x => x.Companies.Intersect(Companiesss) != null).Select(y => y.EmpId).ToList();
                    grdCntrlUsers.ItemsSource = null;

                    List<User> userr = new List<User>();
                    var emppp = repo.getAllusersByEmpIds(empIdss);
                    foreach (var _user in emppp)
                    {
                        if (_user.taskGroups.Contains(parentTaskGroup))
                        {
                            userr.Add(_user);
                        }
                    }
                    grdCntrlUsers.ItemsSource = userr;
                    foreach (User _user in selectedUsers)
                    {
                        if (_user != null)
                            grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                    }
                    return;
                }
                List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                foreach (var _dept in gridDepartment.SelectedItems)
                {
                    employees.AddRange((_dept as Department).employees.Where(x => x.isActive == true).ToList());
                }
                employees = employees.Distinct().ToList();

                List<Company> Companiess = new List<Company>();
                foreach (var _comp in grdCntrlCompanies.SelectedItems)
                {
                    Companiess.Add(_comp as Company);
                }
                //selectedUsers = new List<User>();
                //foreach (var _user in grdCntrlUsers.SelectedItems)
                //{
                //    selectedUsers.Add(_user as User);
                //}

                var empIds = employees.Where(x => x.Companies.Intersect(Companiess) != null).Select(y => y.EmpId).ToList();
                grdCntrlUsers.ItemsSource = repo.getAllusersByEmpIds(empIds);

                foreach (User _user in selectedUsers)
                {
                    grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                }
            }



        }

        private void OnCompanyGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = gridCompanyView;
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
                    var selectedRows = grdCntrlCompanies.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }

        private void OnCompanyGirdNodeCheckStateChanged(object sender, TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
            {
                grdCntrlCompanies.SelectItem(e.Node.RowHandle);
                if (chkHasParent.IsChecked == true)
                {
                    
                    List<Department> deept = new List<Department>();
                    List<Department> departmentList = new List<Department>();
                    foreach (var _dep in grdCntrlCompanies.SelectedItems)
                    {
                        var comp = _dep as Company; 
                         var task = comp.departments;

                        var departments = comp.departments;
                        
                        foreach (var dept in departments)
                        {
                            if (dept.isActive == true)
                            {
                                List<int> empyoyeeIds = new List<int>();
                                foreach (var emp in dept.employees)
                                {
                                    empyoyeeIds.Add(emp.EmpId);
                                }
                                if (empyoyeeIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                    departmentList.Add(dept);
                            }
                        }

                        deept.AddRange(comp.departments.Where(x=>x.TaskGroups.Contains(parentTaskGroup)).ToList());
                    }
                    gridDepartment.ItemsSource = null;
                    gridDepartment.ItemsSource = deept.Distinct();

                    List<Department> deptsss = new List<Department>();
                    foreach (var _dept in gridDepartment.SelectedItems)
                    {
                        deptsss.Add(_dept as Department);
                    }
                    foreach (var _dept in deptsss)
                    {
                        gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), _dept.Id));
                    }
                    foreach (User _user in selectedUsers)
                    {
                        grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                    }
                    return;
                }
                List<Department> deptss = new List<Department>();
                foreach (var _comp in grdCntrlCompanies.SelectedItems)
                {
                    var comp = _comp as Company;
                    deptss.AddRange(comp.departments.Where(x => x.isActive == true).ToList());
                }

                List<Department> depts = new List<Department>();
                foreach (var _dept in gridDepartment.SelectedItems)
                {
                    depts.Add(_dept as Department);
                }
                foreach (User _user in selectedUsers)
                {
                    grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                }
                gridDepartment.ItemsSource = null;
                gridDepartment.ItemsSource = deptss.Distinct();

                foreach (var _dept in depts)
                {
                    gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), _dept.Id));
                }


            }
            else
            {
                grdCntrlCompanies.UnselectItem(e.Node.RowHandle);
                if (chkHasParent.IsChecked == true)
                {

                    List<Department> deept = new List<Department>();
                    List<Department> departmentList = new List<Department>();
                    foreach (var _dep in grdCntrlCompanies.SelectedItems)
                    {
                        var comp = _dep as Company;
                        var task = comp.departments;

                        var departments = comp.departments;

                        foreach (var dept in departments)
                        {
                            if (dept.isActive == true)
                            {
                                List<int> empyoyeeIds = new List<int>();
                                foreach (var emp in dept.employees)
                                {
                                    empyoyeeIds.Add(emp.EmpId);
                                }
                                if (empyoyeeIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                    departmentList.Add(dept);
                            }
                        }

                        deept.AddRange(comp.departments.Where(x => x.TaskGroups.Contains(parentTaskGroup)).ToList());
                    }
                    gridDepartment.ItemsSource = null;
                    gridDepartment.ItemsSource = deept.Distinct();

                    List<Department> deptsss = new List<Department>();
                    foreach (var _dept in gridDepartment.SelectedItems)
                    {
                        deptsss.Add(_dept as Department);
                    }
                    foreach (var _dept in deptsss)
                    {
                        gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), _dept.Id));
                    }
                    foreach (User _user in selectedUsers)
                    {
                        grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                    }
                    return;
                }
                List<Department> deptss = new List<Department>();
                foreach (var _comp in grdCntrlCompanies.SelectedItems)
                {
                    var comp = _comp as Company;
                    deptss.AddRange(comp.departments.Where(x => x.isActive == true).ToList());
                }

                List<Department> depts = new List<Department>();
                foreach (var _dept in gridDepartment.SelectedItems)
                {
                    depts.Add(_dept as Department);
                }

                gridDepartment.ItemsSource = null;
                gridDepartment.ItemsSource = deptss.Distinct();
                //foreach (var _user in grdCntrlUsers.SelectedItems)
                //{
                //    selectedUsers.Add(_user as User);
                //}

                foreach (User _user in selectedUsers)
                {
                    grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
                }
                foreach (var _dept in depts)
                {
                    gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), _dept.Id));
                }


            }
        }

        private void Help_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdHelp.Visibility == Visibility.Collapsed)
                {
                    grdHelp.Visibility = Visibility.Visible;
                }
                else
                {
                    grdHelp.Visibility = Visibility.Collapsed;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        public void loadcompanies()
        {
            //var currentUserCompanies = SYSTEM_STATIC.currentUser.employee.Companies;
            var currentUserCompanies = repo.GetEmployeeForBackground(SYSTEM_STATIC.currentUser.employeeId);
            grdCntrlCompanies.ItemsSource = currentUserCompanies.Companies;
        }

        private void ChkHasParent_Checked(object sender, RoutedEventArgs e)
        {
            lookupParentGroup.IsEnabled = true;
        }

        private void ChkHasParent_Unchecked(object sender, RoutedEventArgs e)
        {
            lookupParentGroup.IsEnabled = false;
        }

        private void LookupParentGroup_PopupOpened(object sender, RoutedEventArgs e)
        {
            ColumnsVisibility();
        }
        private void LoadGroups()
        {
            //BackgroundImagesRepo bgRepo = new BackgroundImagesRepo();
            if(template == TaskGroupTemplate.Standard)
            {
                var standardgroups = repo.GetAllParentTaskGroups();
                lookupParentGroup.ItemsSource = standardgroups;
            }
            else
            {
                var groups = repo.GetAllOptionalTaskGroups(); 
                lookupParentGroup.ItemsSource = groups;
            }
            
        }
        private void LoadUsers()
        {
            allUsers = repo.getAllusers();
            grdCntrlUsers.ItemsSource = allUsers;
            grdCntrlUsersList.ItemsSource = allUsers;
            grdCntrlUsersSelectedList.ItemsSource = selectedUser;

            //var user = repo.getAllusers();
            //grdCntrlUsers.ItemsSource = user;
            //grdCntrlUsersList.ItemsSource = user;
            //grdCntrlUsersSelectedList.ItemsSource = selectedUser;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadGroups();
                if (editFlag == false)
                {

                    if (template == TaskGroupTemplate.Standard)
                    {
                        grdCntrlCompanies.Visibility = Visibility.Visible;
                        gridDepartment.Visibility = Visibility.Visible;
                        grdCntrlUsersList.Visibility = Visibility.Collapsed;
                        grdCntrlUsersSelectedList.Visibility = Visibility.Collapsed;
                        btnLeftMove.Visibility = Visibility.Collapsed;
                        btnRightMove.Visibility = Visibility.Collapsed;
                            
                        loadcompanies();
                    }
                    else
                    {


                        //colCompWidth.Width = GridLength.Auto;
                        colDeptWidth.Width = GridLength.Auto;
                        grdCntrlCompanies.Visibility = Visibility.Collapsed;
                        gridDepartment.Visibility = Visibility.Collapsed;
                        grdCntrlUsers.Visibility = Visibility.Collapsed;


                        LoadUsers();
                    }
                }

                if (groupId > 0 && editFlag == true)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Group") == null)
                        btnSave.IsEnabled = false;

                    taskGroup = repo.GetTaskGroup(groupId);

                    if (taskGroup.Companies.Count == 0 && taskGroup.Departments.Count == 0)
                    {

                        template = TaskGroupTemplate.Optional;
                        //colCompWidth.Width = GridLength.Auto;
                        colDeptWidth.Width = GridLength.Auto;
                        grdCntrlCompanies.Visibility = Visibility.Collapsed;
                        gridDepartment.Visibility = Visibility.Collapsed;
                        grdCntrlUsers.Visibility = Visibility.Collapsed;
                        //selectedUser = taskGroup.users;
                        //grdCntrlUsersSelectedList.ItemsSource = selectedUser;
                            
                        if (taskGroup.parentGroup != null)
                        {
                            chkHasParent.IsChecked = true;
                            lookupParentGroup.Text = taskGroup.parentGroup.GroupName;
                            //LoadUsers();
                            foreach (var rrr in taskGroup.users)
                            {
                                selectedUser.Add(rrr);

                            }

                            grdCntrlUsersSelectedList.ItemsSource = null;
                            grdCntrlUsersSelectedList.ItemsSource = selectedUser;
                            foreach (var rrr in selectedUser)
                            {

                                if (allUsers.Contains(rrr))
                                {
                                    allUsers.Remove(rrr);

                                }
                            }
                            grdCntrlUsersList.ItemsSource = null;
                            grdCntrlUsersList.ItemsSource = allUsers;
                        }
                        else
                        {
                            LoadUsers();
                            foreach (var rrr in taskGroup.users)
                            {
                                selectedUser.Add(rrr);

                            }

                            grdCntrlUsersSelectedList.ItemsSource = null;
                            grdCntrlUsersSelectedList.ItemsSource = selectedUser;
                            foreach (var rrr in selectedUser)
                            {

                                if (allUsers.Contains(rrr))
                                {
                                    allUsers.Remove(rrr);

                                }
                            }
                            grdCntrlUsersList.ItemsSource = null;
                            grdCntrlUsersList.ItemsSource = allUsers;
                        }
                        
                    }
                    else
                    {
                        template = TaskGroupTemplate.Standard;
                        grdCntrlCompanies.Visibility = Visibility.Visible;
                        gridDepartment.Visibility = Visibility.Visible;
                        btnLeftMove.Visibility = Visibility.Collapsed;
                        btnRightMove.Visibility = Visibility.Collapsed;
                        grdCntrlUsersList.Visibility = Visibility.Collapsed;
                        grdCntrlUsersSelectedList.Visibility = Visibility.Collapsed;
                       

                        if (taskGroup.parentGroup != null)
                        {
                            chkHasParent.IsChecked = true;
                            lookupParentGroup.Text = taskGroup.parentGroup.GroupName;
                        }
                        else
                        {
                            loadcompanies();
                        }


                        foreach (Company _company in taskGroup.Companies)
                            grdCntrlCompanies.SelectItem(grdCntrlCompanies.FindRowByValue(grdCntrlCompanies.Columns.GetColumnByFieldName("Id"), _company.Id));

                        foreach (Department _dept in taskGroup.Departments)
                            gridDepartment.SelectItem(gridDepartment.FindRowByValue(gridDepartment.Columns.GetColumnByFieldName("Id"), _dept.Id));
                    }


                    txtItemTitle.Text = taskGroup.GroupName;



                    foreach (User _user in taskGroup.users)
                        grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));

                    selectedUsers = taskGroup.users;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message); 
            }




        }
        private void ColumnsVisibility()
        {
            var grid = lookupParentGroup.GetGridControl();

            grid.Columns["creationDate"].Visible = false;
            grid.Columns["parentGroup"].Visible = false;
            grid.Columns["GroupCreatorId"].Visible = false;
            grid.Columns["GroupCreator"].Visible = false;

            grid.Columns["creationDate"].ShowInColumnChooser = false;
            grid.Columns["parentGroup"].ShowInColumnChooser = false;
            grid.Columns["GroupCreatorId"].ShowInColumnChooser = false;
            grid.Columns["GroupCreator"].ShowInColumnChooser = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtItemTitle.Text))
                {
                    DXMessageBox.Show("Please Enter Group Title!");
                    txtItemTitle.Focus();
                    return;
                }
                if (chkHasParent.IsChecked == true)
                {

                    if (lookupParentGroup.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please Select Parent Group!");
                        lookupParentGroup.Focus();
                        return;
                    }
                }

                if (template == TaskGroupTemplate.Standard)
                {
                    if (grdCntrlCompanies.SelectedItems.Count == 0)
                    {
                        DXMessageBox.Show("Please select at least one Company!");
                        return;
                    }
                    if (gridDepartment.SelectedItems.Count == 0)
                    {
                        DXMessageBox.Show("Please select at least one Department!");
                        return;
                    }

                    if (grdCntrlCompanies.SelectedItems.Count != 0)
                    {
                        taskGroup.Companies = new List<Company>();
                        foreach (Company _company in grdCntrlCompanies.SelectedItems)
                        {
                            if (!taskGroup.Companies.Contains(_company))
                            {
                                taskGroup.Companies.Add(_company);
                            }
                        }
                    }
                    if (gridDepartment.SelectedItems.Count != 0)
                    {
                        taskGroup.Departments = new List<Department>();
                        foreach (Department _dept in gridDepartment.SelectedItems)
                        {
                            if (!taskGroup.Departments.Contains(_dept))
                            {
                                taskGroup.Departments.Add(_dept);
                            }
                        }
                    }
                }


                if (editFlag == true)
                {
                    if(template == ERP_BL.Enums.TaskGroupTemplate.Optional)
                    {
                        if (selectedUser.Count == 0)
                        {
                            DXMessageBox.Show("Please select Users!");
                            return;
                        }
                        if (selectedUser.Count != 0)
                        {
                            taskGroup.users = new List<User>();
                            foreach (var emp in selectedUser)
                            {
                                if (!taskGroup.users.Contains(emp))
                                {
                                    taskGroup.users.Add(emp);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (selectedUsers.Count == 0)
                        {
                            DXMessageBox.Show("Please select Users!");
                            return;
                        }

                        taskGroup.users = new List<User>();
                        foreach (User _user in selectedUsers)
                        {
                            if (!taskGroup.users.Contains(_user))
                            {
                                taskGroup.users.Add(_user);
                            }
                        }
                        //}
                    }


                }
                else
                {
                    if(selectedUser.Count == 0)
                    {
                        if (grdCntrlUsers.SelectedItems.Count == 0)
                        {
                            DXMessageBox.Show("Please select Users!");
                            return;
                        }

                        taskGroup.users = new List<User>();
                        foreach (User _user in grdCntrlUsers.SelectedItems)
                        {
                            if (!taskGroup.users.Contains(_user))
                            {
                                taskGroup.users.Add(_user);
                            }
                        }
                    }
                }

                if (selectedUser.Count == 0 && grdCntrlUsersList.Visibility == Visibility.Visible)
                {
                    DXMessageBox.Show("Please select Users!");
                    return;
                }
                if (selectedUser.Count != 0)
                {
                    taskGroup.users = new List<User>();
                    var userList = (grdCntrlUsersSelectedList.ItemsSource as List<User>) == null ? new List<User>() : grdCntrlUsersSelectedList.ItemsSource as List<User>;
                    foreach (User _user in userList)
                    {
                        if (!taskGroup.users.Contains(_user))
                            taskGroup.users.Add(_user);
                    }
                    //taskGroup.users = new List<User>();
                    //foreach (var emp in selectedUser)
                    //{
                    //    if (!taskGroup.users.Contains(emp))
                    //    {
                    //        taskGroup.users.Add(emp);
                    //    }
                    //}
                }

                taskGroup.GroupName = txtItemTitle.Text;

                if (chkHasParent.IsChecked == true)
                {

                    if (lookupParentGroup.SelectedIndex > -1)
                        taskGroup.parentId = (lookupParentGroup.SelectedItem as TaskGroups).Id;
                }
                else
                    taskGroup.parentId = null;

                if (editFlag == true)
                {
                    taskGroup.isBackground = true;
                    repo.UpdateTaskGroup(taskGroup);
                    DXMessageBox.Show("Updated Successfully!");
                }
                else if (editFlag == false)
                {
                    taskGroup.isBackground = true;
                    taskGroup.GroupCreatorId = SYSTEM_STATIC.currentUser.id;
                    taskGroup.creationDate = DateTime.Now;
                    repo.AddTaskGroup(taskGroup);
                    DXMessageBox.Show("Successfully Added!");
                }


                Window thisWindow = Window.GetWindow(this);
                thisWindow.Close();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void lookupParentGroup_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (chkHasParent.IsChecked == true)
            {
                if (lookupParentGroup.SelectedIndex > -1)
                {
                    if (template == TaskGroupTemplate.Standard)
                    {

                        parentTaskGroup = lookupParentGroup.SelectedItem as TaskGroups;
                        //var selectedGroup = lookupParentGroup.SelectedItem as TaskGroups;
                        var companies = parentTaskGroup.Companies;
                        //var departments = selectedGroup.Departments;
                        //var user = selectedGroup.users;


                        grdCntrlCompanies.ItemsSource = null;
                        grdCntrlCompanies.ItemsSource = companies;

                        //gridDepartment.ItemsSource = null;
                        //gridDepartment.ItemsSource = departments;

                        //grdCntrlUsers.ItemsSource = null;
                        //grdCntrlUsers.ItemsSource = user;
                    }
                    else
                    {
                        if(grdCntrlUsers.Visibility == Visibility.Collapsed)
                        {
                            parentTaskGroup = lookupParentGroup.SelectedItem as TaskGroups;
                            
                            var parentUser = parentTaskGroup.users;
                            allUsers = repo.getAllusers();
                            allUsers = allUsers.Intersect(parentUser).ToList();
                            
                            grdCntrlUsersList.ItemsSource = null;
                            grdCntrlUsersList.ItemsSource = allUsers;    
                        }
                        else
                        {
                            parentTaskGroup = lookupParentGroup.SelectedItem as TaskGroups;
                            var user = parentTaskGroup.users;
                            grdCntrlUsers.ItemsSource = null;
                            grdCntrlUsers.ItemsSource = user;
                        }
                     
                    }

                }
            }
        }

        private void btnLeftMove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdCntrlUsersList.SelectedItem as User;

                if (selectedItem != null)
                {
                    
                    //var rowhandle = gridUsersViewList.FocusedRowHandle;
                    //gridUsersViewList.DeleteRow(rowhandle);
                    //gridUsersViewList.AddNewRow();
                    allUsers.Remove(selectedItem);
                    //parentTaskGroup.users.Add(selectedItem);
                    if (!selectedUser.Contains(selectedItem))
                        selectedUser.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From User List Register!");
                }
                grdCntrlUsersList.RefreshData();
                grdCntrlUsersSelectedList.RefreshData();
                grdCntrlUsersList.SelectedItem = null;
                grdCntrlUsersSelectedList.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }

        private void btnRightMove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = grdCntrlUsersSelectedList.SelectedItem as User;

                if (selectedItem != null)
                {
                    if (!allUsers.Contains(selectedItem))
                        allUsers.Add(selectedItem);
                    selectedUser.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected User Register!");
                }
                grdCntrlUsersList.RefreshData();
                grdCntrlUsersSelectedList.RefreshData();
                grdCntrlUsersList.SelectedItem = null;
                grdCntrlUsersSelectedList.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }
    }
}
