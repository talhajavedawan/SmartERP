using DevExpress.Data;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ZAS_ERP.Reportss;

namespace ZAS_ERP.ToDoTasks.UserControls
{
    /// <summary>
    /// Interaction logic for ucTasksList.xaml
    /// </summary>
    public partial class ucTasksList : UserControl
    {
        public int OpenTasksCount { get; set; } = 0;

        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        List<TaskGroups> TaskGroups = new List<TaskGroups>();
        public ucTasksList()
        {
            //this.DataContext = this;
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CheckPermissions();

            LoadGroups();
            
            LoadCounters();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlToDoTaskList);
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlStepsForRegister);
        }

        private void LoadCounters()
        {
            var group = grdCntrlGroupList.SelectedItem as TaskGroups;
            if (group != null)
            {
                mbtnOpenedTasksCounter.Header = taskRepo.GetAllOpenTasksByGroupIdCount(group.Id);
                mbtnClosedTasksCounter.Header = taskRepo.GetAllClosedTasksByGroupIdCount(group.Id);
                mbtnGroupRegisterCounter.Header = taskRepo.GetGroupTasksRegisterByGroupIdCount(group.Id);
                mbtnTaskRegisterCounter.Header = taskRepo.GetTasksRegisterCount(SYSTEM_STATIC.currentUser.id);
                mbtnBasketedTargetsCount.Header = taskRepo.GetBasketedTasksCount(SYSTEM_STATIC.currentUser.id);
                mbtnVoidCounter.Header = taskRepo.GetVoidTasksCount(SYSTEM_STATIC.currentUser.id);
                mbtnPendingTasksCounter.Header = taskRepo.GetAllPendingForApprovalTasksByGroupIdCount(group.Id);
                mbtnStepsForApprovalCount.Header = taskRepo.GetPendingForApprovalStepCountForRegister(group.Id);
                mbtnStepsForReApprovalCount.Header = taskRepo.GetPendingForReApprovalStepCountForRegister(group.Id);
                mbtnGroupStepRegisterCount.Header = taskRepo.GetAllGroupStepsCount(group.Id);
                mbtnStepRegisterCount.Header = taskRepo.GetAllStepsCount(SYSTEM_STATIC.currentUser.id);
                mbtnBasketedStepsCount.Header = taskRepo.GetAllBasketedStepsCount(SYSTEM_STATIC.currentUser.id);
            }
        }

        private void CheckPermissions()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Delete Group") == null)
                mbtnDeleteNewGroup.IsEnabled = false;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Group") == null)
            {
                btnAddGroup.IsEnabled = false;
                mbtnAddNewGroup.IsEnabled = false;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Target") == null)
                mbtnAddNewTask.IsEnabled = false;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Groups") == null)
                grdCntrlGroupList.Visibility = Visibility.Collapsed;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Target") == null)
            {
                mbtnVoid.Visibility = Visibility.Collapsed;
                mbtnVoidCounter.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Group Target Register") == null)
            {
                mbtnGroupRegister.Visibility = Visibility.Collapsed;
                mbtnGroupRegisterCounter.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target Register") == null)
            {
                mbtnTaskRegister.Visibility = Visibility.Collapsed;
                mbtnTaskRegisterCounter.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Basketed Targets") == null)
            {
                mbtnBasketedTargets.Visibility = Visibility.Collapsed;
                mbtnBasketedTargetsCount.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Group Step Register") == null)
            {
                mbtnGroupStepRegister.Visibility = Visibility.Collapsed;
                mbtnGroupStepRegisterCount.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Step Register") == null)
            {
                mbtnStepRegister.Visibility = Visibility.Collapsed;
                mbtnStepRegisterCount.Visibility = Visibility.Collapsed;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View List of Basketed Steps") == null)
            {
                mbtnBasketedSteps.Visibility = Visibility.Collapsed;
                mbtnBasketedStepsCount.Visibility = Visibility.Collapsed;
            }
        }

        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            AddGroup();
        }

        private void AddGroup()
        {
            DXWindow win = new DXWindow();
            //ucEnterListItemTitle itemTitle = new ucEnterListItemTitle();
            //win.Title = "New Group";

            //win.Content = itemTitle;
            //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Height = 450;
            //win.Width = 250;
            //win.ShowDialog();

            ucFrmSelectTemplate frmTemplate = new ucFrmSelectTemplate();
            win.Title = "Templates";

            win.Content = frmTemplate;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Height = 250;
            win.Width = 400;
            win.ShowDialog();

            LoadGroups();
        }

        private void TxtSearch_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {

        }

        private void MenuItemDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlGroupList.SelectedItem != null)
            {
                var res = DXMessageBox.Show("Targets and Steps linked with this Group will also be deleted. Are you sure you want to Delete this Group? ", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Yes)
                {
                    var _item = grdCntrlGroupList.SelectedItem as TaskGroups;
                    var taskGroup = taskRepo.GetTaskGroup(_item.Id);
                    taskRepo.DeleteTaskGroup(taskGroup);
                    //tblGroupListView.DeleteRow(tblGroupListView.FocusedRowHandle); ;
                    DXMessageBox.Show("Deleted Successfully");
                    taskRepo = new ToDoTaskRepo();
                    LoadGroups();
                    LoadCounters();
                }
            }
            else
            {
                DXMessageBox.Show("Please select the Group first!");
            }
        }

        private void LoadGroupData()
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetAllOpenTasksByGroupId(group.Id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Open Targets";
                    grdCntrlToDoTaskList.ItemsSource = tasks;

                    grdCntrlToDoTaskList.Visibility = Visibility.Visible;
                    grdCntrlStepsForRegister.Visibility = Visibility.Collapsed;
                    panelCheckBoxes.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void BtnSaveTask_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LstBoxToDoTaskList_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnThemeColor_Click(object sender, RoutedEventArgs e)
        {
            if (grdThemeColor.Visibility == Visibility.Collapsed)
                grdThemeColor.Visibility = Visibility.Visible;
            else
                grdThemeColor.Visibility = Visibility.Collapsed;
        }

        private void BtnThemeColorSave_Click(object sender, RoutedEventArgs e)
        {
            ToDoTaskTheme taskTheme = new ToDoTaskTheme();
            //taskTheme.ThemeColorCode = lstBoxToDoTaskList.Background.ToString();
            taskRepo.AddThemeColor(taskTheme);

            DXMessageBox.Show("Theme Applied Successfully!");
            grdThemeColor.Visibility = Visibility.Collapsed;
        }

        private void ClrPcker_Background_ColorChanged(object sender, RoutedEventArgs e)
        {
            var red = ClrPcker_Background.Color.R;
            var green = ClrPcker_Background.Color.G;
            var blue = ClrPcker_Background.Color.B;
            //lstBoxToDoTaskList.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(red, green, blue));
        }

        private void MenuItemTaskDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuGroupUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateGroup();

            int selectedRow = tblGroupListView.FocusedRowHandle;
            //TaskGroups = new List<TaskGroups>();
            LoadGroups();
            //tblGroupListView.FocusedRowHandle = selectedRow;
        }

        private void LoadGroups()
        {
            taskRepo = new ToDoTaskRepo();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "By Pass Authority of Target Groups") == null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Groups") != null)
                {
                    TaskGroups = taskRepo.GetAllTaskGroupsByUser(SYSTEM_STATIC.currentUser.id);
                    grdCntrlGroupList.ItemsSource = TaskGroups;
                }
            }
            else
            {
                TaskGroups = taskRepo.GetAllTaskGroups();
                grdCntrlGroupList.ItemsSource = TaskGroups;
            }
            grdCntrlGroupList.RefreshData();
        }

        private void UpdateGroup()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Group") != null)
            {
                var item = grdCntrlGroupList.SelectedItem;
                var _item = item as TaskGroups;


                var titleCheck =(bool)grdCntrlGroupList.GetFocusedRowCellValue(grdCntrlGroupList.Columns["isTitlee"]);
                if (_item != null && _item.Id > 0)
                {
                    var taskGroup = taskRepo.GetTaskGroup(_item.Id);
                    ucEnterListItemTitle itemTitle = new ucEnterListItemTitle();
                    itemTitle.editFlag = true;
                    itemTitle.groupId = taskGroup.Id;
                  
                    DXWindow win = new DXWindow();
                    if (titleCheck == true)
                        win.Title = "Update Title";
                    else
                        win.Title = "Update Group";
                    win.Content = itemTitle;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //win.Height = 450;
                    //win.Width = 250;
                    win.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Group details!");
            }
        }

        private void MenuItemAddTask_Click(object sender, RoutedEventArgs e)
        {
            ucFrmAddTask frmAddTask = new ucFrmAddTask();
            frmAddTask.editFlag = false;
            frmAddTask.taskGroupId = (grdCntrlGroupList.SelectedItem as TaskGroups).Id;

            Window win = new Window();
            win.Content = frmAddTask;
            win.WindowState = WindowState.Maximized;
            win.Show();
        }

        private void GrdCntrlGroupList_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            LoadGroupData();
            LoadCounters();
        }

        private void MbtnSaveLayout_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if(grdCntrlStepsForRegister.Visibility == Visibility.Visible)
                SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlToDoTaskList);
            else if (grdCntrlToDoTaskList.Visibility == Visibility.Visible)
                SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlStepsForRegister);
        }

        private void MbtnUpdate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            UpdateTask();
        }

        private void MbtnDelete_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }

        private void MbtnRefresh_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            taskRepo = new ToDoTaskRepo();
            LoadGroupData();
            LoadCounters();
        }

        private void MbtnAddNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddNewTask();
        }

        private void MbtnUpdateTask_Click(object sender, RoutedEventArgs e)
        {
            UpdateTask();
        }

        private void AddNewTask()
        {
            if(grdCntrlGroupList.SelectedItem == null)
            {
                DXMessageBox.Show("Please select a Group to Add New Task!");
                return;
            }

            var groupId = (grdCntrlGroupList.SelectedItem as TaskGroups).Id;
            var check = taskRepo.CheckParent(groupId);

            if(check == false)
            {
                ucFrmAddTask frmAddTask = new ucFrmAddTask();
                frmAddTask.editFlag = false;
                frmAddTask.taskGroupId = groupId;

                Window win = new Window();
                win.Content = frmAddTask;
                win.Title = "Targets";
                win.WindowState = WindowState.Maximized;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Targets cannot be added in Parent Groups!");
            }
        }

        private void UpdateTask()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target") != null)
            {
                var task = grdCntrlToDoTaskList.SelectedItem as ToDoTask;
                if (task != null)
                {
                    ucFrmAddTask frmAddTask = new ucFrmAddTask();
                    frmAddTask.taskId = task.Id;
                    frmAddTask.taskGroupId = task.taskGroupId.Value;
                    frmAddTask.editFlag = true;

                    Window win = new Window();
                    win.Content = frmAddTask;
                    win.Title = "Targets";
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("You do not have permission to View Target!");
                return;
            }
        }

        private void MbtnAddTask_Click(object sender, RoutedEventArgs e)
        {
            AddNewTask();
        }

        private void AccordionItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetAllTasksByGroupIdAndUserId(group.Id, SYSTEM_STATIC.currentUser.id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Assigned To Me";
                    grdCntrlToDoTaskList.ItemsSource = tasks;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GrdCntrlToDoTaskList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateTask();
        }

        private void MbtnGroupRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetAllGroupTasks(group.Id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Group Target Register (" + group.GroupName+")";
                    grdCntrlToDoTaskList.ItemsSource = tasks;

                    grdCntrlStepsForRegister.Visibility = Visibility.Collapsed;
                    panelCheckBoxes.Visibility = Visibility.Collapsed;
                    grdCntrlToDoTaskList.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnTaskRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetAllTasks(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Task Register";
                    grdCntrlToDoTaskList.ItemsSource = tasks;
                    //grdCntrlToDoTaskList.ItemsSource = tasks;

                    grdCntrlStepsForRegister.Visibility = Visibility.Collapsed;
                    panelCheckBoxes.Visibility = Visibility.Collapsed;
                    grdCntrlToDoTaskList.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                taskRepo = new ToDoTaskRepo();
                //lstBoxToDoTaskList.Items.Clear();
                var tasks = taskRepo.GetAllVoidTasks(SYSTEM_STATIC.currentUser.id);
                lblHeading.Caption = "Void Targets";
                lblTaskHeading.Text = "Void Targets";
                grdCntrlToDoTaskList.ItemsSource = tasks;

                grdCntrlStepsForRegister.Visibility = Visibility.Collapsed;
                panelCheckBoxes.Visibility = Visibility.Collapsed;
                grdCntrlToDoTaskList.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {
            if(SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "View list of Pending for Approval Targets") != null)
            {
                try
                {
                    var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                    if (group != null)
                    {
                        taskRepo = new ToDoTaskRepo();
                        //lstBoxToDoTaskList.Items.Clear();
                        var tasks = taskRepo.GetAllPendingForApprovalTasksByGroupId(group.Id);
                        lblHeading.Caption = group.GroupName;
                        lblTaskHeading.Text = "Pending For Approval Targets";
                        grdCntrlToDoTaskList.ItemsSource = tasks;
                        grdCntrlStepsForRegister.Visibility = Visibility.Collapsed;
                        panelCheckBoxes.Visibility = Visibility.Collapsed;
                        grdCntrlToDoTaskList.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Pending for Approval Targets!");
                return;
            }
        }

        private void MbtnReApprovals_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnDriectClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnPendingClosing_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grdCntrlToDoTaskList.SelectedItem as ToDoTask;
            if(selectedItem != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Targets Without Approval") != null && selectedItem.isApproved == false)
                {
                    selectedItem.isApproved = true;
                    taskRepo.UpdateStep(selectedItem);
                    DXMessageBox.Show("Target has been approved!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Target to Approve!");
                return;
            }
        }

        private void MbtnOpenedTasks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetAllOpenTasksByGroupId(group.Id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Open Targets";
                    grdCntrlToDoTaskList.ItemsSource = tasks;
                    grdCntrlStepsForRegister.Visibility = Visibility.Collapsed;
                    panelCheckBoxes.Visibility = Visibility.Collapsed;
                    grdCntrlToDoTaskList.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnClosedTaskss_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetAllClosedTasksByGroupId(group.Id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Closed Targets";
                    grdCntrlToDoTaskList.ItemsSource = tasks;

                    grdCntrlStepsForRegister.Visibility = Visibility.Collapsed;
                    panelCheckBoxes.Visibility = Visibility.Collapsed;
                    grdCntrlToDoTaskList.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnExpand_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            gridGroupList.Visibility = Visibility.Collapsed;
        }

        private void MbtnCollapse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            gridGroupList.Visibility = Visibility.Visible;
        }

        private void GrdCntrlToDoTaskList_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "TargettYear" && e.IsGetData)
            {
                var target = grdCntrlToDoTaskList.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
                if (target != null && target.TargetYear != null)
                {
                    e.Value = target.TargetYear.Value.Year;
                }
            }

            if (e.Column.FieldName == "TargettMonth" && e.IsGetData)
            {
                var target = grdCntrlToDoTaskList.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
                if (target != null && target.TargetMonth != null)
                {
                    string fullMonthName = target.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("en"));
                    e.Value = fullMonthName;
                }
            } if (e.Column.FieldName == "assignedTo" && e.IsGetData)
            {
                var pymnt = grdCntrlToDoTaskList.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
                string deptNames = "";

                if (pymnt.assignedToUsers != null && pymnt.assignedToUsers.Count > 0)
                {
                    deptNames = String.Join(" | ", pymnt.assignedToUsers.Select(x => x.userName));
                }
                e.Value = deptNames;
            }


            

            if (e.Column.FieldName == "GroupLevel1" && e.IsGetData)
            {
                var row = grdCntrlToDoTaskList.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
                if (row != null)
                {
                    if (row.targetGroup == null)
                    {
                        if (row.taskGroup != null && row.taskGroup.targetGroup != null)
                        {
                            var groupList = new List<TargetGroup>();
                            var node = row.taskGroup.targetGroup;

                            while (node != null)
                            {
                                if (node.parentId != null)
                                {
                                    if (node.parentId != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        groupList.Add(node);
                                        node = node.targetGroup;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        groupList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    groupList.Add(node);
                                    break;
                                }

                            }
                            groupList.Reverse();
                            if (groupList.Count > 0)
                                e.Value = groupList[0].GroupName;
                        }

                    }
                    else
                    {
                        var groupList = new List<TargetGroup>();
                        var node = row.targetGroup;

                        while (node != null)
                        {
                            if (node.parentId != null)
                            {
                                if (node.parentId != node.Id)
                                {
                                    //this will add current node to department list and transverse to its parent
                                    groupList.Add(node);
                                    node = node.targetGroup;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    groupList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
                                groupList.Add(node);
                                break;
                            }

                        }
                        groupList.Reverse();
                        if (groupList.Count > 0)
                            e.Value = groupList[0].GroupName;
                    }

                }
        }
            if (e.Column.FieldName == "GroupLevel2" && e.IsGetData)
            {
                var row = grdCntrlToDoTaskList.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
                if (row != null)
                {
                    if (row.targetGroup == null)
                    {
                        if (row.taskGroup != null && row.taskGroup.targetGroup != null)
                        {
                            var groupList = new List<TargetGroup>();
                            var node = row.taskGroup.targetGroup;

                            while (node != null)
                            {
                                if (node.parentId != null)
                                {
                                    if (node.parentId != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        groupList.Add(node);
                                        node = node.targetGroup;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        groupList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    groupList.Add(node);
                                    break;
                                }

                            }

                            groupList.Reverse();

                            switch (groupList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = groupList[0].GroupName;
                                    break;
                                case 2:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 3:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 4:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 5:
                                    e.Value = groupList[1].GroupName;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        var groupList = new List<TargetGroup>();
                        var node = row.targetGroup;

                        while (node != null)
                        {
                            if (node.parentId != null)
                            {
                                if (node.parentId != node.Id)
                                {
                                    //this will add current node to department list and transverse to its parent
                                    groupList.Add(node);
                                    node = node.targetGroup;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    groupList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
                                groupList.Add(node);
                                break;
                            }

                        }

                        groupList.Reverse();

                        switch (groupList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = groupList[0].GroupName;
                                break;
                            case 2:
                                e.Value = groupList[1].GroupName;
                                break;
                            case 3:
                                e.Value = groupList[1].GroupName;
                                break;
                            case 4:
                                e.Value = groupList[1].GroupName;
                                break;
                            case 5:
                                e.Value = groupList[1].GroupName;
                                break;
                        }
                    }

                }
            }

            if (e.Column.FieldName == "GroupLevel3" && e.IsGetData)
            {
                var row = grdCntrlToDoTaskList.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;

                if (row != null)
                {
                    if (row.targetGroup == null)
                    {
                        if (row.taskGroup != null && row.taskGroup.targetGroup != null)
                        {
                            var groupList = new List<TargetGroup>();
                            var node = row.taskGroup.targetGroup;

                            while (node != null)
                            {
                                if (node.parentId != null)
                                {
                                    if (node.parentId != node.Id)
                                    {
                                        //this will add current node to department list and transverse to its parent
                                        groupList.Add(node);
                                        node = node.targetGroup;
                                    }
                                    else
                                    {
                                        //when node is parent to itself
                                        groupList.Add(node);
                                        break;
                                    }
                                }
                                else
                                {
                                    //parent with parent id is null
                                    groupList.Add(node);
                                    break;
                                }

                            }
                            groupList.Reverse();

                            switch (groupList.Count)
                            {
                                case 0:

                                    break;
                                case 1:
                                    e.Value = groupList[0].GroupName;
                                    break;
                                case 2:
                                    e.Value = groupList[1].GroupName;
                                    break;
                                case 3:
                                    e.Value = groupList[2].GroupName;
                                    break;
                                case 4:
                                    e.Value = groupList[2].GroupName;
                                    break;
                                case 5:
                                    e.Value = groupList[2].GroupName;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        var groupList = new List<TargetGroup>();
                        var node = row.targetGroup;

                        while (node != null)
                        {
                            if (node.parentId != null)
                            {
                                if (node.parentId != node.Id)
                                {
                                    //this will add current node to department list and transverse to its parent
                                    groupList.Add(node);
                                    node = node.targetGroup;
                                }
                                else
                                {
                                    //when node is parent to itself
                                    groupList.Add(node);
                                    break;
                                }
                            }
                            else
                            {
                                //parent with parent id is null
                                groupList.Add(node);
                                break;
                            }

                        }
                        groupList.Reverse();

                        switch (groupList.Count)
                        {
                            case 0:

                                break;
                            case 1:
                                e.Value = groupList[0].GroupName;
                                break;
                            case 2:
                                e.Value = groupList[1].GroupName;
                                break;
                            case 3:
                                e.Value = groupList[2].GroupName;
                                break;
                            case 4:
                                e.Value = groupList[2].GroupName;
                                break;
                            case 5:
                                e.Value = groupList[2].GroupName;
                                break;
                        }
                    }

                }
            }
        }

        private void MbtnStepsForApproval_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Pending for Approval Steps") != null)
            {
                try
                {
                    var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                    if (group != null)
                    {
                        taskRepo = new ToDoTaskRepo();
                        //lstBoxToDoTaskList.Items.Clear();
                        var tasks = taskRepo.GetAllPendingForApprovalStepsByGroupId(group.Id);
                        lblHeading.Caption = group.GroupName;
                        lblTaskHeading.Text = "Pending For Approval Steps";
                        grdCntrlStepsForRegister.ItemsSource = tasks;
                        grdCntrlStepsForRegister.Visibility = Visibility.Visible;
                        panelCheckBoxes.Visibility = Visibility.Visible;
                        grdCntrlToDoTaskList.Visibility = Visibility.Collapsed;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Pending for Approval Steps!");
                return;
            }
        }

        private void MbtnStepsForReApprovals_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Pending for ReApproval Steps") != null)
            {
                try
                {
                    var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                    if (group != null)
                    {
                        taskRepo = new ToDoTaskRepo();
                        //lstBoxToDoTaskList.Items.Clear();
                        var tasks = taskRepo.GetAllPendingForReApprovalStepsByGroupId(group.Id);
                        lblHeading.Caption = group.GroupName;
                        lblTaskHeading.Text = "Pending For Re-Approval Steps";
                        grdCntrlStepsForRegister.ItemsSource = tasks;
                        grdCntrlStepsForRegister.Visibility = Visibility.Visible;
                        panelCheckBoxes.Visibility = Visibility.Visible;
                        grdCntrlToDoTaskList.Visibility = Visibility.Collapsed;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Pending for Re-Approval Steps!");
                return;
            }
        }

        private void MenuItemUpdateStep_Click(object sender, RoutedEventArgs e)
        {
            UpdateStep();
        }

        private void GrdCntrlSteps_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var step = grdCntrlStepsForRegister.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
            if (e.IsGetData)
                switch (e.Column.FieldName)
                {
                    case "Stage":
                    //var todo = grdCntrlStepsForRegister.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
                    //if (todo.isVoid == true)
                    //{
                    //    e.Value = "Void";
                    //}
                    //else if (todo.isReApproved == false)
                    //{
                    //    e.Value = "Under Approval";
                    //}
                    //else if (todo.isApproved == true && todo.stage == "Closed")
                    //{
                    //    e.Value = "Closed";
                    //}
                    //else if (todo.isApproved == true && todo.Status.isActive == false )
                    //{
                    //    e.Value = "Closed";
                    //}

                    //else if (todo.isApproved == true)
                    //{
                    //    e.Value = "Approved";
                    //}
                    //else if (todo.isApproved == false)
                    //{
                    //    e.Value = "Under Approval";
                    //}
                    //break;
                    case "CalculationTypee":
                        if (step != null && step.calculationType != null)
                        {
                            e.Value = step.calculationType.TypeName;
                        }
                        else if (step != null && step.taskGroup != null && step.taskGroup.calculationType != null)
                        {
                            e.Value = step.taskGroup.calculationType.TypeName;
                        }
                        break;
                    case "StepYear":
                        if (step != null && step.TargetYear != null)
                        {
                            e.Value = step.TargetYear.Value.Year;
                        }
                        break;

                    case "StepMonth":
                        if (step != null && step.TargetMonth != null)
                        {
                            e.Value = step.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("en"));
                        }
                        break;
                    case "TargettYear":
                        if (step != null && step.ParentTask != null && step.ParentTask.TargetYear != null)
                        {
                            e.Value = step.ParentTask.TargetYear.Value.Year;
                        }
                        break;

                    case "TargettMonth":
                        if (step != null && step.ParentTask != null && step.ParentTask.TargetMonth != null)
                        {
                            e.Value = step.ParentTask.TargetMonth.Value.ToString("MMMM", System.Globalization.CultureInfo.CreateSpecificCulture("en"));
                        }
                        break;
                    case "SalesBudgetedMarginPercent":
                        if (step.TaskPoints > 0)
                        {
                            decimal percentage = 0;
                            if (step.TaskPoints != 0)
                            {
                                percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);
                            }
                            e.Value = percentage;
                        }
                        break;
                    case "SoAmountSERpercent":
                        if (step.TaskPoints > 0)
                        {
                            decimal percentage = 0;
                            if (step.SoAmountSER != 0)
                            {
                                percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.SoAmountSER)), 2);

                            }

                            e.Value = percentage;
                        }
                        break;

                    case "Companies":

                        if (step.taskGroup != null)
                        {
                            string companies = "";
                            if (step.taskGroup.Companies != null && step.taskGroup.Companies.Count > 0)
                                companies = String.Join(" | ", step.taskGroup.Companies.Select(x => x.CompanyName));
                            else if (step.taskGroup.CompaniesBulk != null && step.taskGroup.CompaniesBulk.Count > 0)
                                companies = String.Join(" | ", step.taskGroup.CompaniesBulk.Select(x => x.CompanyName));
                            e.Value = companies;
                        }
                        break;

                    case "Departments":

                        if (step.taskGroup != null)
                        {
                            string depts = "";
                            if (step.taskGroup.Departments != null && step.taskGroup.Departments.Count > 0)
                                depts = String.Join(" | ", step.taskGroup.Departments.Select(x => x.DeptName));
                            else if (step.taskGroup.DepartmentsBulk != null && step.taskGroup.DepartmentsBulk.Count > 0)
                                depts = String.Join(" | ", step.taskGroup.DepartmentsBulk.Select(x => x.DeptName));
                            e.Value = depts;
                        }
                        break;
                    case "Group":


                        e.Value = step.taskGroup.GroupName;

                        break;

                    case "StepLevel1":

                        if (step != null)
                        {
                            if (step.targetGroup == null)
                            {
                                if (step.taskGroup != null && step.taskGroup.targetGroup != null)
                                {
                                    var groupList = new List<TargetGroup>();
                                    var node = step.taskGroup.targetGroup;

                                    while (node != null)
                                    {
                                        if (node.parentId != null)
                                        {
                                            if (node.parentId != node.Id)
                                            {
                                                //this will add current node to department list and transverse to its parent
                                                groupList.Add(node);
                                                node = node.targetGroup;
                                            }
                                            else
                                            {
                                                //when node is parent to itself
                                                groupList.Add(node);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //parent with parent id is null
                                            groupList.Add(node);
                                            break;
                                        }

                                    }
                                    groupList.Reverse();
                                    if (groupList.Count > 0)
                                        e.Value = groupList[0].GroupName;
                                }

                            }
                            else
                            {
                                var groupList = new List<TargetGroup>();
                                var node = step.targetGroup;

                                while (node != null)
                                {
                                    if (node.parentId != null)
                                    {
                                        if (node.parentId != node.Id)
                                        {
                                            //this will add current node to department list and transverse to its parent
                                            groupList.Add(node);
                                            node = node.targetGroup;
                                        }
                                        else
                                        {
                                            //when node is parent to itself
                                            groupList.Add(node);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //parent with parent id is null
                                        groupList.Add(node);
                                        break;
                                    }

                                }
                                groupList.Reverse();
                                if (groupList.Count > 0)
                                    e.Value = groupList[0].GroupName;
                            }

                        }
                        break;
                    case "StepLevel2":

                        if (step != null)
                        {
                            if (step.targetGroup == null)
                            {
                                if (step.taskGroup != null && step.taskGroup.targetGroup != null)
                                {
                                    var groupList = new List<TargetGroup>();
                                    var node = step.taskGroup.targetGroup;

                                    while (node != null)
                                    {
                                        if (node.parentId != null)
                                        {
                                            if (node.parentId != node.Id)
                                            {
                                                //this will add current node to department list and transverse to its parent
                                                groupList.Add(node);
                                                node = node.targetGroup;
                                            }
                                            else
                                            {
                                                //when node is parent to itself
                                                groupList.Add(node);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //parent with parent id is null
                                            groupList.Add(node);
                                            break;
                                        }

                                    }

                                    groupList.Reverse();

                                    switch (groupList.Count)
                                    {
                                        case 0:

                                            break;
                                        case 1:
                                            e.Value = groupList[0].GroupName;
                                            break;
                                        case 2:
                                            e.Value = groupList[1].GroupName;
                                            break;
                                        case 3:
                                            e.Value = groupList[1].GroupName;
                                            break;
                                        case 4:
                                            e.Value = groupList[1].GroupName;
                                            break;
                                        case 5:
                                            e.Value = groupList[1].GroupName;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                var groupList = new List<TargetGroup>();
                                var node = step.targetGroup;

                                while (node != null)
                                {
                                    if (node.parentId != null)
                                    {
                                        if (node.parentId != node.Id)
                                        {
                                            //this will add current node to department list and transverse to its parent
                                            groupList.Add(node);
                                            node = node.targetGroup;
                                        }
                                        else
                                        {
                                            //when node is parent to itself
                                            groupList.Add(node);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //parent with parent id is null
                                        groupList.Add(node);
                                        break;
                                    }

                                }

                                groupList.Reverse();

                                switch (groupList.Count)
                                {
                                    case 0:

                                        break;
                                    case 1:
                                        e.Value = groupList[0].GroupName;
                                        break;
                                    case 2:
                                        e.Value = groupList[1].GroupName;
                                        break;
                                    case 3:
                                        e.Value = groupList[1].GroupName;
                                        break;
                                    case 4:
                                        e.Value = groupList[1].GroupName;
                                        break;
                                    case 5:
                                        e.Value = groupList[1].GroupName;
                                        break;
                                }
                            }

                        }
                        break;

                    case "StepLevel3":

                        if (step != null)
                        {
                            if (step.targetGroup == null)
                            {
                                if (step.taskGroup != null && step.taskGroup.targetGroup != null)
                                {
                                    var groupList = new List<TargetGroup>();
                                    var node = step.taskGroup.targetGroup;

                                    while (node != null)
                                    {
                                        if (node.parentId != null)
                                        {
                                            if (node.parentId != node.Id)
                                            {
                                                //this will add current node to department list and transverse to its parent
                                                groupList.Add(node);
                                                node = node.targetGroup;
                                            }
                                            else
                                            {
                                                //when node is parent to itself
                                                groupList.Add(node);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //parent with parent id is null
                                            groupList.Add(node);
                                            break;
                                        }

                                    }
                                    groupList.Reverse();

                                    switch (groupList.Count)
                                    {
                                        case 0:

                                            break;
                                        case 1:
                                            e.Value = groupList[0].GroupName;
                                            break;
                                        case 2:
                                            e.Value = groupList[1].GroupName;
                                            break;
                                        case 3:
                                            e.Value = groupList[2].GroupName;
                                            break;
                                        case 4:
                                            e.Value = groupList[2].GroupName;
                                            break;
                                        case 5:
                                            e.Value = groupList[2].GroupName;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                var groupList = new List<TargetGroup>();
                                var node = step.targetGroup;

                                while (node != null)
                                {
                                    if (node.parentId != null)
                                    {
                                        if (node.parentId != node.Id)
                                        {
                                            //this will add current node to department list and transverse to its parent
                                            groupList.Add(node);
                                            node = node.targetGroup;
                                        }
                                        else
                                        {
                                            //when node is parent to itself
                                            groupList.Add(node);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //parent with parent id is null
                                        groupList.Add(node);
                                        break;
                                    }

                                }
                                groupList.Reverse();

                                switch (groupList.Count)
                                {
                                    case 0:

                                        break;
                                    case 1:
                                        e.Value = groupList[0].GroupName;
                                        break;
                                    case 2:
                                        e.Value = groupList[1].GroupName;
                                        break;
                                    case 3:
                                        e.Value = groupList[2].GroupName;
                                        break;
                                    case 4:
                                        e.Value = groupList[2].GroupName;
                                        break;
                                    case 5:
                                        e.Value = groupList[2].GroupName;
                                        break;
                                }
                            }

                        }
                        break;

                    case "StepLevel4":

                        if (step != null)
                        {
                            if (step.targetGroup == null)
                            {
                                if (step.taskGroup != null && step.taskGroup.targetGroup != null)
                                {
                                    var groupList = new List<TargetGroup>();
                                    var node = step.taskGroup.targetGroup;

                                    while (node != null)
                                    {
                                        if (node.parentId != null)
                                        {
                                            if (node.parentId != node.Id)
                                            {
                                                //this will add current node to department list and transverse to its parent
                                                groupList.Add(node);
                                                node = node.targetGroup;
                                            }
                                            else
                                            {
                                                //when node is parent to itself
                                                groupList.Add(node);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //parent with parent id is null
                                            groupList.Add(node);
                                            break;
                                        }

                                    }
                                    groupList.Reverse();

                                    switch (groupList.Count)
                                    {
                                        case 0:

                                            break;
                                        case 1:
                                            e.Value = groupList[0].GroupName;
                                            break;
                                        case 2:
                                            e.Value = groupList[1].GroupName;
                                            break;
                                        case 3:
                                            e.Value = groupList[2].GroupName;
                                            break;
                                        case 4:
                                            e.Value = groupList[3].GroupName;
                                            break;
                                        case 5:
                                            e.Value = groupList[3].GroupName;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                var groupList = new List<TargetGroup>();
                                var node = step.targetGroup;

                                while (node != null)
                                {
                                    if (node.parentId != null)
                                    {
                                        if (node.parentId != node.Id)
                                        {
                                            //this will add current node to department list and transverse to its parent
                                            groupList.Add(node);
                                            node = node.targetGroup;
                                        }
                                        else
                                        {
                                            //when node is parent to itself
                                            groupList.Add(node);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //parent with parent id is null
                                        groupList.Add(node);
                                        break;
                                    }

                                }
                                groupList.Reverse();

                                switch (groupList.Count)
                                {
                                    case 0:

                                        break;
                                    case 1:
                                        e.Value = groupList[0].GroupName;
                                        break;
                                    case 2:
                                        e.Value = groupList[1].GroupName;
                                        break;
                                    case 3:
                                        e.Value = groupList[2].GroupName;
                                        break;
                                    case 4:
                                        e.Value = groupList[3].GroupName;
                                        break;
                                    case 5:
                                        e.Value = groupList[3].GroupName;
                                        break;
                                }
                            }

                        }
                        break;

                    case "StepLevel5":

                        if (step != null)
                        {
                            if (step.targetGroup == null)
                            {
                                if (step.taskGroup != null && step.taskGroup.targetGroup != null)
                                {
                                    var groupList = new List<TargetGroup>();
                                    var node = step.taskGroup.targetGroup;

                                    while (node != null)
                                    {
                                        if (node.parentId != null)
                                        {
                                            if (node.parentId != node.Id)
                                            {
                                                //this will add current node to department list and transverse to its parent
                                                groupList.Add(node);
                                                node = node.targetGroup;
                                            }
                                            else
                                            {
                                                //when node is parent to itself
                                                groupList.Add(node);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //parent with parent id is null
                                            groupList.Add(node);
                                            break;
                                        }

                                    }
                                    groupList.Reverse();

                                    switch (groupList.Count)
                                    {
                                        case 0:

                                            break;
                                        case 1:
                                            e.Value = groupList[0].GroupName;
                                            break;
                                        case 2:
                                            e.Value = groupList[1].GroupName;
                                            break;
                                        case 3:
                                            e.Value = groupList[2].GroupName;
                                            break;
                                        case 4:
                                            e.Value = groupList[3].GroupName;
                                            break;
                                        case 5:
                                            e.Value = groupList[4].GroupName;
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                var groupList = new List<TargetGroup>();
                                var node = step.targetGroup;

                                while (node != null)
                                {
                                    if (node.parentId != null)
                                    {
                                        if (node.parentId != node.Id)
                                        {
                                            //this will add current node to department list and transverse to its parent
                                            groupList.Add(node);
                                            node = node.targetGroup;
                                        }
                                        else
                                        {
                                            //when node is parent to itself
                                            groupList.Add(node);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        //parent with parent id is null
                                        groupList.Add(node);
                                        break;
                                    }

                                }
                                groupList.Reverse();

                                switch (groupList.Count)
                                {
                                    case 0:

                                        break;
                                    case 1:
                                        e.Value = groupList[0].GroupName;
                                        break;
                                    case 2:
                                        e.Value = groupList[1].GroupName;
                                        break;
                                    case 3:
                                        e.Value = groupList[2].GroupName;
                                        break;
                                    case 4:
                                        e.Value = groupList[3].GroupName;
                                        break;
                                    case 5:
                                        e.Value = groupList[4].GroupName;
                                        break;
                                }
                            }

                        }
                        break;
                }
        }

        private void GrdCntrlStepsForRegister_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateStep();
        }

        private void UpdateStep()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target") != null)
            {
                var task = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
                if (task != null)
                {
                    ucFrmAddTask frmAddTask = new ucFrmAddTask();
                    frmAddTask.taskId = task.parentTaskId.Value;
                    frmAddTask.taskGroupId = task.taskGroupId.Value;
                    frmAddTask.editFlag = true;

                    Window win = new Window();
                    win.Content = frmAddTask;
                    win.Title = "Target";
                    win.WindowState = WindowState.Maximized;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("You do not have permission to View Task!");
                return;
            }
        }
        //public static string ConvertToString(MemoryStream memoryStream)
        //{
        //    //StreamWriter writer = new StreamWriter(memoryStream);
        //    //writer.Write(Layoutstream);
        //    //writer.Flush();
        //    string Layoutstream = "";
        //    memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
        //    StreamReader reader = new StreamReader(memoryStream);
        //    return Layoutstream = reader.ReadToEnd();


        //}
        //public static MemoryStream ConvertToMemoryStream(string memoryStream)
        //{
        //    byte[] byteArray = Encoding.ASCII.GetBytes(memoryStream);
        //    MemoryStream stream = new MemoryStream(byteArray);
        //    //MemoryStream Stream = new MemoryStream();
        //    //Stream.Position = 0;
        //    //StreamReader reader = new StreamReader(memoryStream);
        //    //memoryStream = reader.ReadToEnd();
        //    return stream;
        //}
        private void MbtnExport_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {



            //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            //grdCntrlGroupList.SaveLayoutToStream(memoryStream);
            //var settings = ConvertToString(memoryStream).Trim();
            //grdCntrlGroupList.RestoreLayoutFromStream(ConvertToMemoryStream(settings));





            var inputfromUser = DXMessageBox.Show("Are you sure? \nDo you want to create Report? ", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Export Target Reports") != null && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Targets") != null && lblTaskHeading.Text== "Step Register")
            {
                if (inputfromUser == System.Windows.MessageBoxResult.Yes)
                {
                    Reportss.frmSetReportName setReportName = new Reportss.frmSetReportName(lblTaskHeading.Text);
                    setReportName.ShowDialog();
                    var report = setReportName.report;
                    if (report != null && report.gridReportGroup != null && report.gridReportType != null && report.reportName != null && report.userId != null)
                    {
                        var reportGroup = report.gridReportGroup;
                        var reportType = report.gridReportType;
                        var reportName = report.reportName;
                        ReportLogic.SaveGridReport(grdCntrlStepsForRegister, reportName, reportType, reportGroup, lblTaskHeading.Text, report.titleId);
                        DXMessageBox.Show("( " + reportName + " ) is exported Successfully!", "Congratulation!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("You don't have permission to Export Target Reports!", "Permission Required", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void MbtnGroupStepRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetAllGroupSteps(group.Id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Group Steps Register (" + group.GroupName + ")";
                    grdCntrlStepsForRegister.ItemsSource = tasks;

                    grdCntrlStepsForRegister.Visibility = Visibility.Visible;
                    panelCheckBoxes.Visibility = Visibility.Visible;
                    grdCntrlToDoTaskList.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnStepRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetAllSteps(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Step Register";
                    grdCntrlStepsForRegister.ItemsSource = tasks;
                    //grdCntrlToDoTaskList.ItemsSource = tasks;

                    grdCntrlStepsForRegister.Visibility = Visibility.Visible;
                    panelCheckBoxes.Visibility = Visibility.Visible;
                    grdCntrlToDoTaskList.Visibility = Visibility.Collapsed;

                    RefreshSystemPoints();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshSystemPoints()
        {
            grdCntrlStepsForRegister.ShowLoadingPanel = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += OnDoWork;
            worker.RunWorkerCompleted += OnRunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        private void MbtnRefreshSystemPoints_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            RefreshSystemPoints();

            //grdProgressBar.Visibility = Visibility.Collapsed;
            //TimeSpan ts = new TimeSpan(23, 59, 59);
            //Dispatcher.BeginInvoke(new Action(() =>
            //{
                
            //    for (int i = 0; i < grdCntrlStepsForRegister.VisibleRowCount; i++)
            //    {
            //        var _row = grdCntrlStepsForRegister.GetRow(i);
            //        int rowHandle = grdCntrlStepsForRegister.GetRowHandleByVisibleIndex(i);
            //        var _step = _row as ToDoTask;

            //        ToDoTaskRepo repo = new ToDoTaskRepo();
            //        if (_step.searchedFromDate != null && _step.searchedToDate != null /*&&  !String.IsNullOrEmpty(_step.SOField)*/)
            //        {
            //            var toDate = _step.searchedToDate.Value;
                        
            //            toDate = toDate.Date + ts;

            //            var saleOrders = repo.getDepartmentalSOByDates(_step.taskGroup.Companies, _step.taskGroup.Departments, (DateTime)_step.searchedFromDate, toDate);

            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["margin"], saleOrders.Sum(x => x.margin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BudgetedMargininBase"], saleOrders.Sum(x => x.BudgetedMargininBase));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"], saleOrders.Sum(x => x.SalesBudgetedMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargin"], saleOrders.Sum(x => x.RevisedMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargininBase"], saleOrders.Sum(x => x.RevisedMargininBase));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesRevisedMargin"], saleOrders.Sum(x => x.SalesRevisedMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargin"], saleOrders.Sum(x => x.ActualMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargininBase"], saleOrders.Sum(x => x.ActualMargininBase));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesActualMargin"], saleOrders.Sum(x => x.SalesActualMargin));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalFOBValue"], saleOrders.Sum(x => x.totalFOBValue));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalCFRValue"], saleOrders.Sum(x => x.totalCFRValue));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalBaseCFRValue"], saleOrders.Sum(x => x.totalBaseCFRValue));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountSER"], saleOrders.Sum(x => x.SoAmountSER));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountPER"], saleOrders.Sum(x => x.SoAmountPER));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninBase"], saleOrders.Sum(x => x.commisioninBase));
            //            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commision"], saleOrders.Sum(x => x.commision));

            //            //grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["searchedFromDate"], _step.searchedFromDate);
            //            //grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["searchedToDate"], _step.searchedToDate);
                    
            //        }
            //        taskRepo.UpdateStep(_step);
            //    }
                
            //}), DispatcherPriority.Render);
            //grdProgressBar.Visibility = Visibility.Collapsed;

           
        }

        private void OnDoWork(object o, DoWorkEventArgs args)
        {
            Task.Delay(4000).Wait();  // Pretend to work
        }
        private void OnRunWorkerCompleted(object o, RunWorkerCompletedEventArgs args)
        {

            TimeSpan ts = new TimeSpan(23, 59, 59);
            //Dispatcher.BeginInvoke(new Action(() =>
            //{

            for (int i = 0; i < grdCntrlStepsForRegister.VisibleRowCount; i++)
            {
                //var _row = grdCntrlStepsForRegister.GetRow(i);
                int rowHandle = grdCntrlStepsForRegister.GetRowHandleByVisibleIndex(i);
                var _row = grdCntrlStepsForRegister.GetRow(rowHandle);
                var _step = _row as ToDoTask;

                ToDoTaskRepo repo = new ToDoTaskRepo();
                if (rowHandle >= 0 && _step != null && _step.Id > 0)
                {
                    List<SaleOrder> saleOrders = new List<SaleOrder>();
                    if (_step.searchedFromDate != null && _step.searchedToDate != null /*&&  !String.IsNullOrEmpty(_step.SOField)*/)
                    {
                        if (_step.taskGroup.CompaniesBulk != null && _step.taskGroup.CompaniesBulk.Count() > 0 && _step.taskGroup.DepartmentsBulk != null && _step.taskGroup.DepartmentsBulk.Count() > 0)
                        {
                            var toDate = _step.searchedToDate.Value;

                            toDate = toDate.Date + ts;

                            saleOrders = repo.getDepartmentalSOByDates(_step.taskGroup.CompaniesBulk, _step.taskGroup.DepartmentsBulk, (DateTime)_step.searchedFromDate, toDate);

                            var TotalCount = saleOrders.Count();
                            var UnderApprovalCount = saleOrders.Where(x => x.isApproved == false && x.isVoid != true).Count();
                            var ApprovedCount = saleOrders.Where(x => x.saleOrderStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true).Count();
                            var UnderClosingCount = saleOrders.Where(x => x.PendingForClosing == true && x.isVoid != true).Count();
                            var ClosedCount = saleOrders.Where(x => (x.isApproved == true && x.stage == "Closed") || (x.isApproved == true && x.saleOrderStatus.isActive == false && x.PendingForClosing != true)).Count();

                            var principalSO = saleOrders.Where(x => x.saleOrdertype == InquiryType.Principal).ToList();
                            var supplyCCCSO = saleOrders.Where(x => x.saleOrdertype == InquiryType.SupplyCCC).ToList();
                            var nonPrincipalSO = saleOrders.Where(x => x.saleOrdertype != InquiryType.Principal && x.saleOrdertype != InquiryType.SupplyCCC).ToList();

                            var margin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin))) /*+ (principalSO.Sum(x => x.margin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["margin"], /*saleOrders.Sum(x => x.margin)*/margin);

                            var MarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BudgetedMargininBase"], MarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var MarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"], MarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);

                            var revMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) /*+ (principalSO.Sum(x => x.RevisedMargin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargin"], /*saleOrders.Sum(x => x.RevisedMargin)*/revMargin);

                            var revMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargininBase"], revMarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var revMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesRevisedMargin"], /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/revMarginSE);

                            var actualMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) /*+ (principalSO.Sum(x => x.ActualMargin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargin"], /*saleOrders.Sum(x => x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin)))*/ actualMargin);

                            var actualMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargininBase"], actualMarginME/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var actualMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesActualMargin"], actualMarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);


                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalFOBValue"], saleOrders.Sum(x => x.totalFOBValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalCFRValue"], saleOrders.Sum(x => x.totalCFRValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalBaseCFRValue"], saleOrders.Sum(x => x.totalBaseCFRValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountSER"], saleOrders.Sum(x => x.SoAmountSER));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountPER"], saleOrders.Sum(x => x.SoAmountPER));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninBase"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * x.exchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commision"], saleOrders.Sum(x => x.commision));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninSE"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * Convert.ToDouble(x.marginExchangeRate)));

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginOC"], saleOrders.Sum(x => Convert.ToDouble(x.SystemMargin)));
                            var systemMarginSE = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginSE"], systemMarginSE);
                            var systemMarginME = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginME"], systemMarginME);

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommision"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision)));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommisionSER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommisionMER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)));

                            var BMGrossProfitSE = saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)) + Convert.ToDouble(MarginSE);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitSE"], BMGrossProfitSE);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitME"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)) + Convert.ToDouble(MarginME)));

                            var EstimatedGrossProfitSE = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["EstimatedGrossProfitSE"]));

                            if (BMGrossProfitSE > EstimatedGrossProfitSE)
                                grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["EstimatedGrossProfitSE"], BMGrossProfitSE);

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TotalQuantity"], saleOrders.Sum(x => Convert.ToDouble(x.TotalQuantity)));

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TotalOrdersCount"], TotalCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["UnderApprovalOrdersCount"], UnderApprovalCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ApprovedOrdersCount"], ApprovedCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["UnderClosingOrdersCount"], UnderClosingCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ClosedOrdersCount"], ClosedCount);
                        }
                        else
                        {
                            var toDate = _step.searchedToDate.Value;

                            toDate = toDate.Date + ts;

                            saleOrders = repo.getDepartmentalSOByDates(_step.taskGroup.Companies, _step.taskGroup.Departments, (DateTime)_step.searchedFromDate, toDate);

                            var TotalCount = saleOrders.Count();
                            var UnderApprovalCount = saleOrders.Where(x => x.isApproved == false && x.isVoid != true).Count();
                            var ApprovedCount = saleOrders.Where(x => x.saleOrderStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true).Count();
                            var UnderClosingCount = saleOrders.Where(x => x.PendingForClosing == true && x.isVoid != true).Count();
                            var ClosedCount = saleOrders.Where(x => (x.isApproved == true && x.stage == "Closed") || (x.isApproved == true && x.saleOrderStatus.isActive == false && x.PendingForClosing != true)).Count();

                            var principalSO = saleOrders.Where(x => x.saleOrdertype == InquiryType.Principal).ToList();
                            var supplyCCCSO = saleOrders.Where(x => x.saleOrdertype == InquiryType.SupplyCCC).ToList();
                            var nonPrincipalSO = saleOrders.Where(x => x.saleOrdertype != InquiryType.Principal && x.saleOrdertype != InquiryType.SupplyCCC).ToList();

                            var margin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin))) /*+ (principalSO.Sum(x => x.margin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["margin"], /*saleOrders.Sum(x => x.margin)*/margin);

                            var MarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BudgetedMargininBase"], MarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var MarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"], MarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);

                            var revMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) /*+ (principalSO.Sum(x => x.RevisedMargin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargin"], /*saleOrders.Sum(x => x.RevisedMargin)*/revMargin);

                            var revMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["RevisedMargininBase"], revMarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var revMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesRevisedMargin"], /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/revMarginSE);

                            var actualMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) /*+ (principalSO.Sum(x => x.ActualMargin))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargin"], /*saleOrders.Sum(x => x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin)))*/ actualMargin);

                            var actualMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ActualMargininBase"], actualMarginME/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var actualMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SalesActualMargin"], actualMarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);


                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalFOBValue"], saleOrders.Sum(x => x.totalFOBValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalCFRValue"], saleOrders.Sum(x => x.totalCFRValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["totalBaseCFRValue"], saleOrders.Sum(x => x.totalBaseCFRValue));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountSER"], saleOrders.Sum(x => x.SoAmountSER));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["SoAmountPER"], saleOrders.Sum(x => x.SoAmountPER));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninBase"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * x.exchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commision"], saleOrders.Sum(x => x.commision));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["commisioninSE"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * Convert.ToDouble(x.marginExchangeRate)));

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginOC"], saleOrders.Sum(x => Convert.ToDouble(x.SystemMargin)));
                            var systemMarginSE = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginSE"], systemMarginSE);
                            var systemMarginME = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["systemMarginME"], systemMarginME);

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommision"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision)));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommisionSER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)));
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["netCommisionMER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)));

                            //grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitSE"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)) + Convert.ToDouble(MarginSE)));
                            //grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitME"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)) + Convert.ToDouble(MarginME)));

                            var BMGrossProfitSE = saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)) + Convert.ToDouble(MarginSE);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitSE"], BMGrossProfitSE);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["BMgrossProfitME"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)) + Convert.ToDouble(MarginME)));

                            var EstimatedGrossProfitSE = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["EstimatedGrossProfitSE"]));

                            if (BMGrossProfitSE > EstimatedGrossProfitSE)
                                grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["EstimatedGrossProfitSE"], BMGrossProfitSE);

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TotalQuantity"], saleOrders.Sum(x => Convert.ToDouble(x.TotalQuantity)));

                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TotalOrdersCount"], TotalCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["UnderApprovalOrdersCount"], UnderApprovalCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ApprovedOrdersCount"], ApprovedCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["UnderClosingOrdersCount"], UnderClosingCount);
                            grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["ClosedOrdersCount"], ClosedCount);
                        }


                    }


                    if (_step.calculationType != null)
                    {
                        var achieved = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, _step.calculationType.AchievedField.SOFieldName));
                        var total = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, _step.calculationType.TotalField.SOFieldName));
                        var result = Math.Round((achieved / total) * 100, 0);

                        grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TargetAchievedPercenatage"], result);
                        //var result1 = Math.Round((Convert.ToDouble(_step.GetPropertyValue(_step.calculationType.AchievedField.SOFieldName)) / Convert.ToDouble(_step.GetPropertyValue(_step.calculationType.TotalField.SOFieldName))) * 100, 2);
                        var status = repo.GetTaskStatusByPercentage(result);
                        if (status != null)
                        {
                            _step.statusId = status.Id;
                        }

                    }
                    else if (_step.taskGroup != null && _step.taskGroup.calculationType != null)
                    {
                        var achieved = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, _step.taskGroup.calculationType.AchievedField.SOFieldName));
                        var total = Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(rowHandle, _step.taskGroup.calculationType.TotalField.SOFieldName));
                        var result = Math.Round((achieved / total) * 100, 0);

                        grdCntrlStepsForRegister.SetCellValue(rowHandle, grdCntrlStepsForRegister.Columns["TargetAchievedPercenatage"], result);
                        //var result1 = Math.Round((Convert.ToDouble(_step.GetPropertyValue(_step.calculationType.AchievedField.SOFieldName)) / Convert.ToDouble(_step.GetPropertyValue(_step.calculationType.TotalField.SOFieldName))) * 100, 2);
                        var status = repo.GetTaskStatusByPercentage(result);
                        if (status != null)
                        {
                            _step.statusId = status.Id;
                        }

                    }
                    taskRepo.UpdateStep(_step);
                }

            }

            //}), DispatcherPriority.Render);
            //grdProgressBar.Visibility = Visibility.Collapsed;
            grdCntrlStepsForRegister.ShowLoadingPanel = false;
        }

        double salesBMTotal1 = 0, salesBMTotal2 = 0, SOamountSERtotal = 0, totalTaskPoints = 0;

        private void ChkClosedSO_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tblStepsListView.FormatConditions)
            {
                if (c.FieldName == "ClosedOrdersCount")
                    c.IsEnabled = true;
            }
        }

        private void ChkClosedSO_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tblStepsListView.FormatConditions)
            {
                if (c.FieldName == "ClosedOrdersCount")
                    c.IsEnabled = false;
            }
        }

        private void ChkApprovedSO_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tblStepsListView.FormatConditions)
            {
                if (c.FieldName == "OpenOrdersCount")
                    c.IsEnabled = false;
            }
        }

        private void ChkApprovedSO_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var c in tblStepsListView.FormatConditions)
            {
                if(c.FieldName == "OpenOrdersCount")
                    c.IsEnabled = true;
            }
                
        }

        private void GrdCntrlStepsForRegister_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.IsTotalSummary)
            {
                GridControl gridControl = sender as GridControl;

                GridSummaryItem item = e.Item as GridSummaryItem;
                if (item.FieldName == "SoAmountSERpercent")
                {
                    switch (e.SummaryProcess)
                    {
                        case CustomSummaryProcess.Start:
                            salesBMTotal1 = 0;
                            SOamountSERtotal = 0;
                            break;
                        case CustomSummaryProcess.Calculate:
                            salesBMTotal1 += Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(e.RowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"]));
                            SOamountSERtotal += Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(e.RowHandle, grdCntrlStepsForRegister.Columns["SoAmountSER"]));

                            //Total = debitTotal - creditTotal;
                            break;
                        case CustomSummaryProcess.Finalize:

                            e.TotalValue = Math.Round((salesBMTotal1 / SOamountSERtotal) * 100, 2);
                            break;

                    }
                }
                else if (item.FieldName == "SalesBudgetedMarginPercent")
                {
                    switch (e.SummaryProcess)
                    {
                        case CustomSummaryProcess.Start:
                            salesBMTotal2 = 0;
                            totalTaskPoints = 0;
                            break;
                        case CustomSummaryProcess.Calculate:
                            salesBMTotal2 += Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(e.RowHandle, grdCntrlStepsForRegister.Columns["SalesBudgetedMargin"]));
                            totalTaskPoints += Convert.ToDouble(grdCntrlStepsForRegister.GetCellValue(e.RowHandle, grdCntrlStepsForRegister.Columns["TaskPoints"]));

                            //Total = debitTotal - creditTotal;
                            break;
                        case CustomSummaryProcess.Finalize:

                            e.TotalValue = Math.Round((salesBMTotal2 / totalTaskPoints) * 100, 2);
                            break;
                    }
                }
            }
        }


        public void loadcomments()
        {
            try
            {
                ToDoTask target = new ToDoTask();
                if (grdCntrlStepsForRegister.Visibility == Visibility.Visible)
                {
                    target = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
                    if (target != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        List<CommentLog> comments = new List<CommentLog>();
                        comments = procurementRepo.getcommentslogAsc(target.Id, TransactionItemType.TargetStep);
                        grdCommentss.ItemsSource = comments;
                    }
                }
                else if (grdCntrlToDoTaskList.Visibility == Visibility.Visible)
                {
                    target = grdCntrlToDoTaskList.SelectedItem as ToDoTask;
                    if (target != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        List<CommentLog> comments = new List<CommentLog>();
                        comments = procurementRepo.getcommentslogAsc(target.Id, TransactionItemType.ToDo_Task);
                        grdCommentss.ItemsSource = comments;
                    }
                }
               
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            ToDoTask target = new ToDoTask();
            if (grdCntrlStepsForRegister.Visibility == Visibility.Visible)
            {
                target = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
                if (target != null)
                {
                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    if (target != null)
                    {
                        var taskGroup = target.taskGroup;

                        List<User> groupUsers = new List<User>();

                        if (taskGroup.usersBulk != null && taskGroup.usersBulk.Count > 0)
                            groupUsers = taskGroup.usersBulk;
                        else if (taskGroup.users != null && taskGroup.users.Count > 0)
                            groupUsers = taskGroup.users;

                        if (taskGroup != null && groupUsers != null && groupUsers.Count != 0)
                        {
                            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, groupUsers, TransactionItemType.TargetStep);
                            inputBox.ShowDialog();
                        }
                        else
                        {
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                        }


                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.comment != "" && target.Id != 0)
                        {
                            if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", null);
                                }
                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", null);
                                }
                            }

                            procurementRepo.Add(target.Id, TransactionItemType.TargetStep, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            //frmInputBox.taggedUsers = new List<User>();
                            loadcomments();
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (target.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Target Reward first to add a comment!");
                        }

                    }
                    loadcomments();
                }
                else
                {
                    DXMessageBox.Show("Please select any row to Add Comment!");
                }
            }
            else if (grdCntrlToDoTaskList.Visibility == Visibility.Visible)
            {
                target = grdCntrlToDoTaskList.SelectedItem as ToDoTask;
                if (target != null)
                {
                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    if (target != null)
                    {
                        var taskGroup = target.taskGroup;
                        List<User> groupUsers = new List<User>();

                        if (taskGroup.usersBulk != null && taskGroup.usersBulk.Count > 0)
                            groupUsers = taskGroup.usersBulk;
                        else if (taskGroup.users != null && taskGroup.users.Count > 0)
                            groupUsers = taskGroup.users;

                        if (taskGroup != null && groupUsers != null && groupUsers.Count != 0)
                        {
                            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, groupUsers, TransactionItemType.ToDo_Task);
                            inputBox.ShowDialog();
                        }
                        else
                        {
                            frmInputBox inputBox = new frmInputBox();
                            inputBox.ShowDialog();
                        }


                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.comment != "" && target.Id != 0)
                        {
                            if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, user.id, "New Comment ", null);
                                }
                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, 0, user.id, "New Comment ", null);
                                }
                            }

                            procurementRepo.Add(target.Id, TransactionItemType.ToDo_Task, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            //frmInputBox.taggedUsers = new List<User>();
                            loadcomments();
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (target.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Target Reward first to add a comment!");
                        }

                    }
                    loadcomments();
                }
                else
                {
                    DXMessageBox.Show("Please select any row to Add Comment!");
                }
            }
                

           
        }

        private void BtnDateFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddNewTitle_Click(object sender, RoutedEventArgs e)
        {
            DXWindow win = new DXWindow();
            ucEnterListItemTitle itemTitle = new ucEnterListItemTitle();
            itemTitle.isTitle = true;
            win.Title = "New Group";

            win.Content = itemTitle;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Height = 450;
            //win.Width = 250;
            win.Show();

            //var myWindow = Window.GetWindow(this);
            //myWindow.Close();
        }

        private void MenuItemLinkedSaleorders_Click(object sender, RoutedEventArgs e)
        {
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            TimeSpan ts = new TimeSpan(23, 59, 59);
            var selectedItem = grdCntrlStepsForRegister.SelectedItem as ToDoTask;

            ToDoTaskRepo repo = new ToDoTaskRepo();
            if (selectedItem != null && selectedItem.Id > 0)
            {
                if (selectedItem.searchedFromDate != null && selectedItem.searchedToDate != null)
                {
                    if (selectedItem.taskGroup.CompaniesBulk != null && selectedItem.taskGroup.CompaniesBulk.Count() > 0 && selectedItem.taskGroup.DepartmentsBulk != null && selectedItem.taskGroup.DepartmentsBulk.Count() > 0)
                    {
                        var toDate = selectedItem.searchedToDate.Value;

                        toDate = toDate.Date + ts;

                        saleOrders = repo.getDepartmentalSOByDates(selectedItem.taskGroup.CompaniesBulk, selectedItem.taskGroup.DepartmentsBulk, (DateTime)selectedItem.searchedFromDate, toDate);
                    }
                    else
                    {
                        var toDate = selectedItem.searchedToDate.Value;

                        toDate = toDate.Date + ts;
                        saleOrders = repo.getDepartmentalSOByDates(selectedItem.taskGroup.Companies, selectedItem.taskGroup.Departments, (DateTime)selectedItem.searchedFromDate, toDate);
                    }
                }
            }

            ucTargetsSaleOrderList targetsSaleOrderList = new ucTargetsSaleOrderList();
            targetsSaleOrderList.saleOrders = saleOrders;
            Window window = new Window();
            window.Content = targetsSaleOrderList;
            window.WindowState = WindowState.Maximized;
            window.Show();
        }

        private void MenuItemVisibleTargetsSaleorders_Click(object sender, RoutedEventArgs e)
        {
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            TimeSpan ts = new TimeSpan(23, 59, 59);
            for (int i = 0; i < grdCntrlStepsForRegister.VisibleRowCount; i++)
            {
                //var _row = grdCntrlStepsForRegister.GetRow(i);
                int rowHandle = grdCntrlStepsForRegister.GetRowHandleByVisibleIndex(i);
                var _row = grdCntrlStepsForRegister.GetRow(rowHandle);
                var _step = _row as ToDoTask;

                ToDoTaskRepo repo = new ToDoTaskRepo();
                if (rowHandle >= 0 && _step != null && _step.Id > 0)
                {
                    if (_step.searchedFromDate != null && _step.searchedToDate != null /*&&  !String.IsNullOrEmpty(_step.SOField)*/)
                    {
                        if (_step.taskGroup.CompaniesBulk != null && _step.taskGroup.CompaniesBulk.Count() > 0 && _step.taskGroup.DepartmentsBulk != null && _step.taskGroup.DepartmentsBulk.Count() > 0)
                        {
                            var toDate = _step.searchedToDate.Value;

                            toDate = toDate.Date + ts;

                            saleOrders = repo.getDepartmentalSOByDates(_step.taskGroup.CompaniesBulk, _step.taskGroup.DepartmentsBulk, (DateTime)_step.searchedFromDate, toDate);
                        }
                        else
                        {
                            var toDate = _step.searchedToDate.Value;

                            toDate = toDate.Date + ts;
                            saleOrders.AddRange(repo.getDepartmentalSOByDates(_step.taskGroup.Companies, _step.taskGroup.Departments, (DateTime)_step.searchedFromDate, toDate));
                        }
                    }
                }
            }

            saleOrders = saleOrders.Distinct().ToList();

            ucTargetsSaleOrderList targetsSaleOrderList = new ucTargetsSaleOrderList();
            targetsSaleOrderList.saleOrders = saleOrders;
            Window window = new Window();
            window.Content = targetsSaleOrderList;
            window.WindowState = WindowState.Maximized;
            window.Show();
        }

        private void TblGroupListView_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "isTitlee" && e.IsGetData)
                {
                    //var grp = grdCntrlGroupList.GetRow(e.Node.RowHandle) as TaskGroups;

                    //if (grp.isTitle == true || ((grp.Companies == null || grp.Companies.Count == 0) && (grp.Departments == null || grp.Departments.Count == 0) && (grp.CompaniesBulk == null || grp.CompaniesBulk.Count == 0) && (grp.DepartmentsBulk == null || grp.DepartmentsBulk.Count == 0)))
                    //    e.Value = true;
                    //else
                    //    e.Value = false;


                    if (e.Node.HasChildren == false)
                    {
                        e.Value = false;
                    }
                    else
                        e.Value = true;
                }
            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message);
            }
            //if( e.Node.HasChildren == false)
            //{
            //    TreeListView grdCntrl = sender as TreeListView;


            //}
        }

        private void BtnRefreshGroups_Click(object sender, RoutedEventArgs e)
        {
            LoadGroups();
        }

        private void ChkGroupLevel1_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel1"].Visible = true;
        }

        private void ChkGroupLevel2_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel2"].Visible = true;
            chkGroupLevel1.IsChecked = true;
        }

        private void ChkGroupLevel3_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel3"].Visible = true;
            chkGroupLevel1.IsChecked = true;
            chkGroupLevel2.IsChecked = true;
        }

        private void ChkGroupLevel4_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel4"].Visible = true;
            chkGroupLevel1.IsChecked = true;
            chkGroupLevel2.IsChecked = true;
            chkGroupLevel3.IsChecked = true;
        }

        private void ChkGroupLevel5_Checked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel5"].Visible = true;
            chkGroupLevel1.IsChecked = true;
            chkGroupLevel2.IsChecked = true;
            chkGroupLevel3.IsChecked = true;
            chkGroupLevel4.IsChecked = true;
        }

        private void ChkGroupLevel1_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel1"].Visible = false;
            chkGroupLevel2.IsChecked = false;
            chkGroupLevel3.IsChecked = false;
            chkGroupLevel4.IsChecked = false;
            chkGroupLevel5.IsChecked = false;
        }

        private void ChkGroupLevel2_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel2"].Visible = false;
            chkGroupLevel3.IsChecked = false;
            chkGroupLevel4.IsChecked = false;
            chkGroupLevel5.IsChecked = false;
        }

        private void ChkGroupLevel3_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel3"].Visible = false;
            chkGroupLevel4.IsChecked = false;
            chkGroupLevel5.IsChecked = false;
        }

        private void ChkGroupLevel4_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel4"].Visible = false;
            chkGroupLevel5.IsChecked = false;
        }

        private void ChkGroupLevel5_Unchecked(object sender, RoutedEventArgs e)
        {
            grdCntrlStepsForRegister.Columns["StepLevel5"].Visible = false;
        }

        private void MbtnBasketedTargets_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetBasketedTasks(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Basketed Targets";
                    grdCntrlToDoTaskList.ItemsSource = tasks;
                    //grdCntrlToDoTaskList.ItemsSource = tasks;

                    grdCntrlStepsForRegister.Visibility = Visibility.Collapsed;
                    panelCheckBoxes.Visibility = Visibility.Collapsed;
                    grdCntrlToDoTaskList.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MbtnBasketedSteps_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var group = grdCntrlGroupList.SelectedItem as TaskGroups;
                if (group != null)
                {
                    taskRepo = new ToDoTaskRepo();
                    //lstBoxToDoTaskList.Items.Clear();
                    var tasks = taskRepo.GetAllBasketedSteps(SYSTEM_STATIC.currentUser.id);
                    lblHeading.Caption = group.GroupName;
                    lblTaskHeading.Text = "Basketed Steps";
                    grdCntrlStepsForRegister.ItemsSource = tasks;
                    //grdCntrlToDoTaskList.ItemsSource = tasks;

                    grdCntrlStepsForRegister.Visibility = Visibility.Visible;
                    panelCheckBoxes.Visibility = Visibility.Visible;
                    grdCntrlToDoTaskList.Visibility = Visibility.Collapsed;

                    RefreshSystemPoints();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void grdCntrlStepsForRegister_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var task = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
            task.EstimatedGrossProfitSE = Convert.ToDouble(txtEstimatedGP.Text);

            taskRepo.UpdateStep(task);

            DXMessageBox.Show("Updated Estimated Gross Profit (SE)");
        }

        private void tblStepsListView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            var task = grdCntrlStepsForRegister.GetFocusedRow() as ToDoTask;
            txtEstimatedGP.Text = task.EstimatedGrossProfitSE.ToString();
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ToDoTask target = new ToDoTask();
            if(grdCntrlStepsForRegister.Visibility == Visibility.Visible)
            {
                target = grdCntrlStepsForRegister.SelectedItem as ToDoTask;
                if (target != null)
                {
                    NotificationsRepo notificationsRepo = new NotificationsRepo();

                    if (grdCommentss.SelectedItem != null)
                    {
                        var comment = grdCommentss.SelectedItem as CommentLog;
                        if (target != null)
                        {
                            var taskGroup = target.taskGroup;
                            if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                            {
                                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.users, comment, TransactionItemType.TargetStep);
                                inputBox.ShowDialog();
                            }
                            else
                            {
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                            }

                            if (target != null)
                            {
                                ProcurementRepo procurementRepo = new ProcurementRepo();

                                if (frmInputBox.commentAdded == true && target.Id != 0)
                                {

                                    var commentId = procurementRepo.AddCommentLinkNotification(target.Id, TransactionItemType.TargetStep, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                                    if (commentId != null)
                                    {
                                        foreach (var user in frmInputBox.Comment.TaggedList)
                                        {
                                            if (frmInputBox.FlagForTag == true)
                                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                            else
                                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                        }

                                        foreach (var user in frmInputBox.Comment.CCUsersList)
                                        {
                                            if (frmInputBox.FlagForCC == true)
                                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                            else
                                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                        }
                                    }


                                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                                }
                                else if (target.Id == 0)
                                {
                                    DXMessageBox.Show("Kindly save Target first to add a comment!");
                                }
                            }
                            loadcomments();
                        }
                        else
                        {
                            DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                        }
                    }
                }
            }
            else if(grdCntrlToDoTaskList.Visibility == Visibility.Visible)
            {
                target = grdCntrlToDoTaskList.SelectedItem as ToDoTask;
                if (target != null)
                {
                    NotificationsRepo notificationsRepo = new NotificationsRepo();

                    if (grdCommentss.SelectedItem != null)
                    {
                        var comment = grdCommentss.SelectedItem as CommentLog;
                        if (target != null)
                        {
                            var taskGroup = target.taskGroup;
                            if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                            {
                                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.users, comment, TransactionItemType.ToDo_Task);
                                inputBox.ShowDialog();
                            }
                            else
                            {
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.ShowDialog();
                            }

                            if (target != null)
                            {
                                ProcurementRepo procurementRepo = new ProcurementRepo();

                                if (frmInputBox.commentAdded == true && target.Id != 0)
                                {

                                    var commentId = procurementRepo.AddCommentLinkNotification(target.Id, TransactionItemType.ToDo_Task, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                                    if (commentId != null)
                                    {
                                        foreach (var user in frmInputBox.Comment.TaggedList)
                                        {
                                            if (frmInputBox.FlagForTag == true)
                                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                            else
                                                notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                        }

                                        foreach (var user in frmInputBox.Comment.CCUsersList)
                                        {
                                            if (frmInputBox.FlagForCC == true)
                                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                            else
                                                notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Target with Target Points: " + target.TaskPoints, target.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                        }
                                    }


                                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                                }
                                else if (target.Id == 0)
                                {
                                    DXMessageBox.Show("Kindly save Target first to add a comment!");
                                }
                            }
                            loadcomments();
                        }
                        else
                        {
                            DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                        }
                    }
                }
            }
                           
        }
    }
}
