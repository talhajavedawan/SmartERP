using DevExpress.Data;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ZAS_ERP.Procurementss;

namespace ZAS_ERP.ToDoTasks.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddTask.xaml
    /// </summary>
    public partial class ucFrmAddTask : UserControl
    {
        TaskGroups taskGroup = new TaskGroups();
        ToDoTask task = new ToDoTask();
        public int taskId = 0;
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public int taskGroupId = 0;
        public bool editFlag = false;

        UsersRepo UsersRepo = new UsersRepo();
        bool addinfo = true;
        List<ViewInfo> views = new List<ViewInfo>();

        List<User> assignedToUsers = new List<User>();
        List<ToDoTask> steps = new List<ToDoTask>();
        List<cmbitem> SoFields = new List<cmbitem>();
        //public List<string> SOFields { get; set; }

        TaskGroupTemplate template;

        public ucFrmAddTask()
        {
            this.DataContext = this;
            InitializeComponent();
            //SOFields = SYSTEM_STATIC.GetSoFields().Select(x=>x.name).ToList();
        }

        private void SetColumnNames()
        {
            //var fieldName = String.Join(" | ", steps.Select(x => x.SOField).Distinct());
            //if (!String.IsNullOrEmpty(fieldName))
            //{
            //    grdCntrlSteps.Columns["SystemPoints"].Header = fieldName;
            //    grdCntrlSteps.Columns["AchievedPercentage"].Header = fieldName + " (%age)";
            //}

            //var fieldName1 = String.Join(" | ", steps.Select(x => x.SOField1).Distinct());
            //if (!String.IsNullOrEmpty(fieldName1))
            //{
            //    grdCntrlSteps.Columns["SystemPoints1"].Header = fieldName1;
            //    grdCntrlSteps.Columns["AchievedPercentage1"].Header = fieldName1 + " (%age)";
            //}

            //var fieldName2 = String.Join(" | ", steps.Select(x => x.SOField2).Distinct());
            //if (!String.IsNullOrEmpty(fieldName2))
            //{
            //    grdCntrlSteps.Columns["SystemPoints2"].Header = fieldName2;
            //    grdCntrlSteps.Columns["AchievedPercentage2"].Header = fieldName2 + " (%age)";
            //}
        }

        private void ShowSteps(int taskId)
        {
            //ToDoTaskRepo repo = new ToDoTaskRepo();
            steps = null;
            steps = taskRepo.GetAllStepsByParentId(taskId);
            grdCntrlSteps.ItemsSource = null;
            grdCntrlSteps.ItemsSource = steps.Where(x => x.isApproved != false && x.isReApproved != false && x.isVoid != true).ToList();

            SetColumnNames();
            if(steps != null && steps.Count > 0)
            {
                if(!String.IsNullOrEmpty(txtTaskPoints.Text) && Convert.ToDouble(txtTaskPoints.Text) > 0)
                {
                    if(template == TaskGroupTemplate.Standard)
                    {
                        var achvdPoints = steps.Where(x=>x.isVoid != true).Sum(x => x.AchievedPoints);
                        pbStatus.Value = Math.Round(achvdPoints / Convert.ToDouble(txtTaskPoints.Text) * 100, 2);
                    }
                    else if (template == TaskGroupTemplate.Optional)
                    {
                        var achvdPoints = steps.Where(x => x.isVoid != true).Sum(x => x.AchievedPoints);
                        pbStatus.Value = Math.Round(achvdPoints / Convert.ToDouble(txtTaskPoints.Text) * 100, 2);
                    }
                }
                
                //txtPercentageAchieved.Text = (Convert.ToDouble(steps.Sum(x => x.AchievedPoints)) / Convert.ToDouble(steps.Sum(x => x.TaskPoints)) * 100).ToString();
            }
                
            //List<cmbitem> cmbItems = new List<cmbitem>();
            //foreach (var _step in steps)
            //{
            //    cmbItems.Add(new cmbitem()
            //    {
            //        id = _step.Id,
            //        isActive = _step.isCompleted,
            //        name = _step.TaskName,
            //        description = _step.StepDueDate.Value.ToShortDateString()
            //    });
            //    //lstBoxSteps.Items.Add(_step.TaskName);
            //}
            //grdCntrlSteps.ItemsSource = cmbItems;
        }

        private void LoadUsers()
        {
            if(taskGroupId > 0)
            {
                var grp = taskRepo.GetTaskGroup(taskGroupId);
                List<User> groupUsers = new List<User>();

                if (grp.usersBulk != null && grp.usersBulk.Count > 0)
                    groupUsers = grp.usersBulk;
                else if(grp.users != null && grp.users.Count > 0)
                        groupUsers = grp.users;

                lookUpAssignedTo.ItemsSource = groupUsers;
                lookUpSupervisedBy.ItemsSource = groupUsers;
                lookUpSalesHead.ItemsSource = groupUsers;

                foreach (var _user in groupUsers)
                {
                    if (_user.employee.person.Photo != null)
                        panelGroupImages.Children.Add( new ImageEdit() {
                            Source = GetBitmapImageFromByteArray(_user.employee.person.Photo),
                            Height = 50,
                            Width = 50,
                            Stretch = Stretch.Fill,
                            Margin = new Thickness(5, 0 , 0, 0),
                            ShowMenu=false,
                            ToolTip = _user.employee.person.FName + " " + _user.employee.person.LName
                        });
                }
            }
        }

        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "On Load Data");
            }
        }

        private void LoadData()
        {
            CheckPermissions();
            LoadTargetTypes();
            LoadUsers();
            LoadStatuses();
            LoadTargetGroups();



            if (editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
                datCompletionDate.IsEnabled = false;

                if (taskGroupId > 0)
                {
                    taskGroup = taskRepo.GetTaskGroup(taskGroupId);
                    if (taskGroup.CompaniesBulk != null && taskGroup.CompaniesBulk.Count > 0 && taskGroup.DepartmentsBulk != null && taskGroup.DepartmentsBulk.Count > 0)
                    {
                        txtCompanies.Text = String.Join(" | ", taskGroup.CompaniesBulk.Select(x => x.CompanyName));
                        txtDepartments.Text = String.Join(" | ", taskGroup.DepartmentsBulk.Select(x => x.DeptName));

                        template = TaskGroupTemplate.Standard;
                    }else if (taskGroup.Companies != null && taskGroup.Companies.Count > 0 && taskGroup.Departments != null && taskGroup.Departments.Count > 0)
                    {
                        txtCompanies.Text = String.Join(" | ", taskGroup.Companies.Select(x => x.CompanyName));
                        txtDepartments.Text = String.Join(" | ", taskGroup.Departments.Select(x => x.DeptName));

                        template = TaskGroupTemplate.Standard;
                    }
                    else
                        template = TaskGroupTemplate.Optional;

                    lblGroupName.Text = taskGroup.GroupName;
                }
            }

            if (editFlag == true && taskId != 0)
            {
                SoFields = SYSTEM_STATIC.GetSoFields();
                cmbField.ItemsSource = SoFields;

                task = taskRepo.GetTask(taskId);

                if (task.taskGroupId != null && task.taskGroupId > 0)
                {
                    taskGroupId = task.taskGroupId.Value;
                    taskGroup = taskRepo.GetTaskGroup(taskGroupId);

                    if (taskGroup.CompaniesBulk != null && taskGroup.CompaniesBulk.Count > 0 && taskGroup.DepartmentsBulk != null && taskGroup.DepartmentsBulk.Count > 0)
                    {
                        txtCompanies.Text = String.Join(" | ", taskGroup.CompaniesBulk.Select(x => x.CompanyName));
                        txtDepartments.Text = String.Join(" | ", taskGroup.DepartmentsBulk.Select(x => x.DeptName));

                        template = TaskGroupTemplate.Standard;
                    }
                    else if(taskGroup.Companies != null && taskGroup.Companies.Count > 0 && taskGroup.Departments != null && taskGroup.Departments.Count > 0)
                    {
                        txtCompanies.Text = String.Join(" | ", taskGroup.Companies.Select(x => x.CompanyName));
                        txtDepartments.Text = String.Join(" | ", taskGroup.Departments.Select(x => x.DeptName));

                        template = TaskGroupTemplate.Standard;
                    }
                    else
                        template = TaskGroupTemplate.Optional;

                    var grp = taskGroup;
                    string groupName = "";
                    while (grp != null)
                    {
                        if (String.IsNullOrEmpty(groupName))
                            groupName = grp.GroupName;
                        else
                            groupName = grp.GroupName +"->"+groupName ;

                        grp = grp.parentGroup;
                    }

                    lblGroupName.Text = groupName;
                }

                if (task.isVoid == true)
                    grdVoid.Visibility = Visibility.Visible;
                else
                    grdVoid.Visibility = Visibility.Collapsed;

                if (task.isBasketed == true)
                    chkBasketed.IsChecked = true;
                else
                    chkBasketed.IsChecked = false;

                if (task.taskGroupId != null)
                    taskGroupId = task.taskGroupId.Value;

                txtTaskId.Text = task.Id.ToString();
                if (task.TaskName != null)
                    txtSelectedTaskName.Text = task.TaskName;
                else
                    txtSelectedTaskName.Text = "";

                if (task.TaskDescription != null)
                    txtTaskDescription.Text = task.TaskDescription;
                else
                    txtTaskDescription.Text = "";

                txtTaskPoints.Text = task.TaskPoints.ToString();

                ShowSteps(taskId);
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCntrlSteps);
                SetColumnNames();
                LoadCounters();

                if (task.taskTargetType != null)
                    lookUpTargetType.Text = task.taskTargetType.TargetTypeName;
                else
                    lookUpTargetType.SelectedIndex = -1;

                if (task.targetGroup != null)
                    lookUpTargetGroup.Text = task.targetGroup.GroupName;
                else if (task.taskGroup.targetGroup != null)
                    lookUpTargetGroup.Text = task.taskGroup.targetGroup.GroupName;

                //if (task.assignedTo != null)
                //    lookUpAssignedTo.Text = task.assignedTo.userName;
                //else
                //    lookUpAssignedTo.SelectedIndex = -1;

                assignedToUsers = task.assignedToUsers;
                lookUpAssignedTo.EditValue = String.Join(" | ", assignedToUsers.Select(x => x.userName));

                //var grid = lookUpAssignedTo.GetGridControl();
                //foreach (var _user in task.assignedToUsers)
                //{
                //    grid.SelectItem(grid.FindRowByValue(grid.Columns.GetColumnByFieldName("id"), _user.id));
                //}

                if (task.creationDate != null)
                    datCreationDate.EditValue = task.creationDate;

                if (task.StartDate != null)
                    datStartDate.EditValue = task.StartDate.Value;
                else
                    datStartDate.EditValue = null;

                if (task.TargetYear != null)
                    datTargetYear.EditValue = task.TargetYear.Value;
                else
                    datTargetYear.EditValue = null;

                if (task.TargetMonth != null)
                    datTargetMonth.EditValue = task.TargetMonth.Value;
                else
                    datTargetMonth.EditValue = null;

                if (task.TentativeClosingDate != null)
                    datTentativeCompletionDate.EditValue = task.TentativeClosingDate.Value;
                else
                    datTentativeCompletionDate.EditValue = null;

                if (task.ActualClosingDate != null)
                    datCompletionDate.EditValue = task.ActualClosingDate.Value;
                else
                    datCompletionDate.EditValue = null;

                if (task.supervisedBy != null)
                    lookUpSupervisedBy.EditValue = task.supervisedBy.id;

                if (task.salesHead != null)
                    lookUpSalesHead.EditValue = task.salesHead.id;

                //Select Status
                var statusList = (cmbTaskStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbTaskStatus.ItemsSource as List<cmbitem>;
                if (task.Status != null)
                {
                    int index = 0;
                    foreach (var _status in statusList)
                    {
                        if (_status.id == task.statusId)
                        {
                            cmbTaskStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (task.isImportant == true)
                    chkImportant.IsChecked = true;
                else
                    chkImportant.IsChecked = false;

                if (task.isCompleted == true)
                    chkCompleted.IsChecked = true;
                else
                    chkCompleted.IsChecked = false;
            }
        }

        private void BtnAddStep_Click(object sender, RoutedEventArgs e)
        {
            if(editFlag == false)
            {
                DXMessageBox.Show("Please Save this Target first to add steps!");
                return;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "Add New Step") != null)
            {
                ucFrmAddStep frmAddStep = new ucFrmAddStep();
                Window win = new Window();

                frmAddStep.txtParentId.Text = txtTaskId.Text;
                frmAddStep.taskGroupId = taskGroupId;
                frmAddStep.editFlag = false;

                win.Content = frmAddStep;
                win.Height = 450;
                win.Width = 500;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.ShowDialog();

                ShowSteps(Convert.ToInt32(txtTaskId.Text));
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new step!");
                return;
            }
            
        }

        private void MenuItemUpdateStep_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Step") != null)
            {
                ucFrmAddStep frmAddStep = new ucFrmAddStep();
                Window win = new Window();
                int stepId = 0;
                int focussedRow = 0;
                if (grdCntrlSteps.SelectedItem != null)
                {
                    frmAddStep.txtParentId.Text = txtTaskId.Text;
                    frmAddStep.taskGroupId = taskGroupId;
                    stepId = (grdCntrlSteps.SelectedItem as ToDoTask).Id;
                    frmAddStep.stepId = stepId;
                    frmAddStep.editFlag = true;
                    focussedRow = tblTasksListView.FocusedRowHandle;

                    win.Content = frmAddStep;
                    win.Height = 450;
                    win.Width = 500;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ResizeMode = ResizeMode.CanMinimize;
                    win.ShowDialog();

                    ShowSteps(Convert.ToInt32(txtTaskId.Text));

                    //Update Step Values Populate in Gridcontrol
                    //grdCntrlSteps.SelectItem(grdCntrlSteps.FindRowByValue(grdCntrlSteps.Columns.GetColumnByFieldName("Id"), stepId));
                    //var selectedItem = grdCntrlSteps.SelectedItem as GridRow;
                    

                    ToDoTaskRepo repo = new ToDoTaskRepo();
                    var _step = repo.GetTask(stepId);

                    grdCntrlSteps.SetCellValue(focussedRow, grdCntrlSteps.Columns["isCompleted"], _step.isCompleted);
                    grdCntrlSteps.SetCellValue(focussedRow, grdCntrlSteps.Columns["TaskName"], _step.TaskName);
                    grdCntrlSteps.SetCellValue(focussedRow, grdCntrlSteps.Columns["StepDueDate"], _step.StepDueDate);
                    grdCntrlSteps.SetCellValue(focussedRow, grdCntrlSteps.Columns["PointsUpdatedOn"], _step.PointsUpdatedOn);
                    grdCntrlSteps.SetCellValue(focussedRow, grdCntrlSteps.Columns["TaskPoints"], _step.TaskPoints);
                    grdCntrlSteps.SetCellValue(focussedRow, grdCntrlSteps.Columns["AchievedPoints"], _step.AchievedPoints);
                }
                
            }
            else
            {
                DXMessageBox.Show("Permission Required to View Step!");
            }

        }

        private void LoadTargetTypes()
        {
            lookUpTargetType.ItemsSource = taskRepo.GetAllTargetTypes();
        }

        private void CheckPermissions()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Target") == null)
                btnSaveTask.IsEnabled = false;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Step") == null)
                btnAddStep.IsEnabled = false;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Over Rule Group Level") == null)
                lookUpTargetGroup.IsEnabled = false;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Make a Copy of Targets") == null)
                btnMakeCopy.IsEnabled = false;
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Mark or UnMark Targets as Basketed") == null)
                chkBasketed.IsEnabled = false;
        }


        private void MenuItemDeleteStep_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Delete Step") != null)
            {
                if (DXMessageBox.Show("Do you really want to Delete this Step?", "Delete Step", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (grdCntrlSteps.SelectedItem != null)
                    {
                        taskRepo.DeleteStep((grdCntrlSteps.SelectedItem as ToDoTask).Id);
                        DXMessageBox.Show("Deleted Successfully!");
                        ShowSteps(Convert.ToInt32(txtTaskId.Text));
                    }
                }
            }
            else
                DXMessageBox.Show("Permission Required to Delete Step!");

        }

        private void BtnSelectUser_Click(object sender, RoutedEventArgs e)
        {
            //if (task != null && task.Id > 0)
            //{
            //    var taskGroup = taskRepo.GetTaskGroup(task.taskGroupId.Value);

            //    if (taskGroup.users != null)
            //    {
            //        ucUsersList usersList = new ucUsersList(taskGroup.users);
            //        Window win = new Window();
            //        win.Content = usersList;
            //        win.Height = 450;
            //        win.Width = 300;
            //        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //        win.ResizeMode = ResizeMode.CanMinimize;
            //        win.ShowDialog();

            //        var _user = usersList.grdCntrlUsers.SelectedItem as User;

            //        if (_user != null)
            //        {
            //            btnSelectUser.Visibility = Visibility.Collapsed;
            //            panelAssignedTo.Visibility = Visibility.Visible;

            //            txtUserId.Text = _user.id.ToString();
            //            if (_user.employee.person.FName.Length > 0)
            //            {
            //                txtInitials.Text = _user.employee.person.FName.Substring(0, 1);
            //            }
            //            if (_user.employee.person.LName.Length > 0)
            //            {
            //                txtInitials.Text = txtInitials.Text + _user.employee.person.LName.Substring(0, 1);
            //            }
            //            txtUserName.Text = _user.employee.person.FName + " " + _user.employee.person.LName;
            //        }
            //    }
            //}
        }

        private void TxtRemoveUser_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //txtUserId.Text = "0";
            //panelAssignedTo.Visibility = Visibility.Collapsed;
            //btnSelectUser.Visibility = Visibility.Visible;
        }

        private void LoadTargetGroups()
        {
            lookUpTargetGroup.ItemsSource = taskRepo.GetAllTargetGroups();
        }

        private void LoadStatuses()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            //BillRepo billRepo = new BillRepo();
            List<ToDoTaskStatus> taskStatuses = new List<ToDoTaskStatus>();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Statuses in Targets") != null)
                taskStatuses = taskRepo.GetAllTaskStatuses();
            else
                taskStatuses = taskRepo.GetAllTaskStatuses().Where(x => x.isActive == true).ToList();

            Parallel.ForEach(taskStatuses, delegate (ToDoTaskStatus status) // foreach (BillStatus status in BillStatuses)
            {
                cmbitems.Add(new cmbitem()
                {
                    name = status.Status,
                    id = status.Id,
                    bcolor = status.backcolor,
                    fcolor = "#FF000000"
                });
            });
            cmbTaskStatus.ItemsSource = cmbitems;
        }

        private void BtnSaveTask_Click(object sender, RoutedEventArgs e)
        {
            if (lookUpTargetType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Target Type!");
                lookUpTargetType.Focus();
                return;
            }

            if (assignedToUsers==null || assignedToUsers.Count == 0)
            {
                DXMessageBox.Show("Please select Assigned To User!");
                lookUpAssignedTo.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtTaskPoints.Text) || Convert.ToDouble(txtTaskPoints.Text) == 0)
            {
                DXMessageBox.Show("Please enter Target Points!");
                txtTaskPoints.Focus();
                return;
            }
            if (datStartDate.EditValue == null)
            {
                DXMessageBox.Show("Please select Start Date!");
                datStartDate.Focus();
                return;
            }
            if (datTentativeCompletionDate.EditValue == null)
            {
                DXMessageBox.Show("Please select Tentative Completion Date!");
                datTentativeCompletionDate.Focus();
                return;
            }

            if (datTargetYear.EditValue == null)
            {
                DXMessageBox.Show("Please select Target Year!");
                datTargetYear.Focus();
                return;
            }

            //if (datTargetMonth.EditValue == null)
            //{
            //    DXMessageBox.Show("Please select Target Year!");
            //    datTargetMonth.Focus();
            //    return;
            //}
            //if (datCompletionDate.EditValue == null)
            //{
            //    DXMessageBox.Show("Please select Completion Date!");
            //    datCompletionDate.Focus();
            //    return;
            //}

            if (cmbTaskStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Status!");
                cmbTaskStatus.Focus();
                return;
            }

            if (datTentativeCompletionDate.EditValue != null && datTentativeCompletionDate.DateTime < datStartDate.DateTime)
            {
                DXMessageBox.Show("Tentative Completion date should not be less than start date!");
                datTentativeCompletionDate.Focus();
                return;
            }

            if (datCompletionDate.EditValue != null && datCompletionDate.DateTime < datTentativeCompletionDate.DateTime)
            {
                DXMessageBox.Show("Completion date should not be less than Tentative date!");
                datCompletionDate.Focus();
                return;
            }


            task.targetTypeId = (lookUpTargetType.SelectedItem as TaskTargetType).Id;

            if(lookUpTargetGroup.SelectedIndex > -1)
            {
                task.targetGroupId = (lookUpTargetGroup.SelectedItem as TargetGroup).Id;
            }
            else
            {
                task.targetGroupId = null;
            }

            task.TaskName = txtSelectedTaskName.Text;
            task.TaskDescription = txtTaskDescription.Text;
            task.TaskPoints = Convert.ToDouble(txtTaskPoints.Text);
            


            if(taskGroupId>0)
                task.taskGroupId = taskGroupId;

            task.StartDate = datStartDate.DateTime;
            task.TentativeClosingDate = datTentativeCompletionDate.DateTime;

            task.TargetYear = new DateTime(datTargetYear.DateTime.Year, 1, 1);

            if (datTargetMonth.EditValue != null)
                task.TargetMonth = new DateTime(2022, datTargetMonth.DateTime.Month, 1);
            else
                task.TargetMonth = null;

            if (datTentativeCompletionDate.EditValue == null)
                task.ActualClosingDate = datCompletionDate.DateTime;
            else
                task.ActualClosingDate = null;

            //task.assignedToId = (lookUpAssignedTo.SelectedItem as User).id;
            //var grid = lookUpAssignedTo.GetGridControl();
            //var users = grid.SelectedItems as List<User>;
            //List<User> selectedUsers = new List<User>();
            task.assignedToUsers = new List<User>();
            foreach (var _user in assignedToUsers)
            {
                //var user = _user as User;
                if (!task.assignedToUsers.Contains(_user))
                    task.assignedToUsers.Add(_user);
                //selectedUsers.Add( user);
            }

            if(lookUpSupervisedBy.SelectedIndex > -1)
            {
                task.supervisedById = (lookUpSupervisedBy.SelectedItem as User).id;
            }

            if (lookUpSalesHead.SelectedIndex > -1)
            {
                task.salesHeadId = (lookUpSalesHead.SelectedItem as User).id;
            }

            //task.assignedToUsers = selectedUsers;

            task.statusId = (cmbTaskStatus.SelectedItem as cmbitem).id;

            if (chkImportant.IsChecked == true)
                task.isImportant = true;
            else
                task.isImportant = false;

            if (chkCompleted.IsChecked == true)
                task.isCompleted = true;
            else
                task.isCompleted = false;


            if(editFlag == true)
            {
                
                
                    var _task = taskRepo.GetTaskByGroupAndType(task.taskGroup.Id, task.targetTypeId.Value, task.TargetYear);
                    if(_task != null)
                    {
                        if(_task.Id != task.Id )
                        {
                            DXMessageBox.Show("Target having same Target Type and Target Year already exists!");
                            return;
                        }
                    }
                

                task.stepCount = steps.Count;
                var visibleItems = grdCntrlSteps.VisibleItems;
                int count = 0;
                //List<ToDoTask> toDoTasks = new List<ToDoTask>();
                double achievedPoints = 0;
                double systemPoints = 0;

                achievedPoints = steps.Where(x => x.isVoid != true).Sum(x=>x.AchievedPoints);
                systemPoints = steps.Where(x => x.isVoid != true).Sum(x => x.SystemPoints);
                count = steps.Where(x => x.isCompleted == true).Count();

                steps.ForEach(c => c.isBasketed = chkBasketed.IsChecked.Value);
                //foreach (ToDoTask _task in visibleItems)
                //{
                //    //_task.taskGroupId = taskGroupId;
                //    if (_task.isCompleted == true)
                //        count++;
                //    toDoTasks.Add(_task);
                //    achievedPoints = achievedPoints + _task.AchievedPoints;
                //    systemPoints = systemPoints + _task.SystemPoints;
                //}


                task.achievedStepsCount = count;
                //task.PercentageAchieved = Math.Round(pbStatus.Value, 2);
                //task.AchievedPoints = Math.Round(Convert.ToDouble(grdCntrlSteps.Columns["AchievedPoints"].TotalSummaries[0].Value));
                if(template == TaskGroupTemplate.Standard)
                    task.PercentageAchieved = Math.Round( (achievedPoints / Convert.ToDouble(txtTaskPoints.Text) *100),2);
                else
                    task.PercentageAchieved = Math.Round((achievedPoints / Convert.ToDouble(txtTaskPoints.Text) * 100), 2);

                task.AchievedPoints = Math.Round( achievedPoints,2);
                task.SystemPoints = Math.Round(systemPoints, 2);

                task.isBasketed = chkBasketed.IsChecked.Value;

                taskRepo.UpdateTask(task, steps);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                task.creationDate = datCreationDate.DateTime;
                task.isApproved = false;
                taskRepo.AddTask(task);
                DXMessageBox.Show("Added Successfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
            //panelTaskDetails.Visibility = Visibility.Collapsed;
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {
                // if (e.GetListSourceFieldValue("employee.person.FName") != null|| e.GetListSourceFieldValue("employee.person.LName") != null )
                {


                    string fname = viewInfo.User.employee.person.FName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.FName"));
                    string lname = viewInfo.User.employee.person.LName;// Convert.ToString(e.GetListSourceFieldValue("usera.employee.person.LName"));

                    //DateTime date;

                    e.Value = fname + " " + lname;
                }
            }
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Visible)
            {

                gridTracker.Visibility = Visibility.Collapsed;
            }
            else
            if (gridTracker.Visibility == Visibility.Collapsed && task.Id != 0)
            {

                views = UsersRepo.getViwerInfo(task.Id, (int)TransactionItemType.ToDo_Task);
                grdUsers.ItemsSource = views;
                gridTracker.Visibility = Visibility.Visible;
            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            if (task.isApproved == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Targets Without Approval") != null)
                {
                    //task.isReviewed = true;
                    task.isApproved = true;
                    task.stage = TransactionStage.Approved.ToString();
                    taskRepo.UpdateStep(task);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Target has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (taskGroup.users != null && taskGroup.users.Count > 0)
                        {
                            winTagUsers win = new winTagUsers(taskGroup.users, taskGroup.Id, TransactionItemType.ToDo_Task);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }


                        //if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        //{
                        //    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(purchaseOrder.department.Id, purchaseOrder.company.Id));
                        //    win.ShowDialog();
                        //    tagUsers = win.tagUsers;
                        //    ccUsers = win.ccUsers;
                        //}
                        //else
                        //{
                        //    winTagUsers win = new winTagUsers();
                        //    win.ShowDialog();
                        //}

                    }
                    
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task having Task Points: " + task.TaskPoints.ToString() + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "Task Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.ToDo_Task, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type:" + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, task.Id, (int)TransactionItemType.ToDo_Task, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
                //else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null))
                //{
                //    bill.isReviewed = true;
                //    bill.needReview = false;
                //    bill.stage = TransactionStage.AwaitingApproval.ToString();

                //    billRepo.update(bill);
                //    if (addinfo)
                //    {
                //        frmInputBox inputBox = new frmInputBox();
                //        inputBox.ShowDialog();
                //        _usersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                //        addinfo = false;
                //    }
                //}
                //if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null))
                //{
                //    bill.isReviewed = true;
                //    bill.needReview = true;
                //    bill.stage = TransactionStage.AwaitingSecondReview.ToString();

                //    billRepo.update(bill);
                //    if (addinfo)
                //    {
                //        frmInputBox inputBox = new frmInputBox();
                //        inputBox.ShowDialog();
                //        _usersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
                //        addinfo = false;
                //    }
                //}
            }
            else if (task.isApproved == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Targets Without Approval") != null)
                {
                    //task.isReviewed = true;
                    task.isApproved = false;
                    task.stage = TransactionStage.AwaitingApproval.ToString();
                    taskRepo.UpdateStep(task);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Target has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (taskGroup.users != null && taskGroup.users.Count > 0)
                        {
                            winTagUsers win = new winTagUsers(taskGroup.users, taskGroup.Id, TransactionItemType.ToDo_Task);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }


                        //if (purchaseOrder.department != null && purchaseOrder.department.Id != 0 && purchaseOrder.company?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        //{
                        //    winTagUsers win = new winTagUsers(userRepo.getusersByCompanyDepartment(purchaseOrder.department.Id, purchaseOrder.company.Id));
                        //    win.ShowDialog();
                        //    tagUsers = win.tagUsers;
                        //    ccUsers = win.ccUsers;
                        //}
                        //else
                        //{
                        //    winTagUsers win = new winTagUsers();
                        //    win.ShowDialog();
                        //}

                    }

                    
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task having Task Points: " + task.TaskPoints.ToString() + " has been UnApproved",
                        Timestamp = DateTime.Now,
                        Subject = "Task UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.ToDo_Task, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    if (addinfo)
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                        UsersRepo.Add(TransactionInfo.Approved_Adding, task.Id, (int)TransactionItemType.ToDo_Task, frmInputBox.comment);
                        addinfo = false;
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }

            //if (bill.PendingForClosing == true)
            //{
            //    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing Bill") != null)
            //    {
            //        bill.isReviewed = true;
            //        bill.PendingForClosing = false;
            //        bill.stage = TransactionStage.Closed.ToString();
            //        billRepo.update(bill);
            //        if (addinfo)
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //            _usersRepo.Add(TransactionInfo.Approved_Closing, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
            //            addinfo = false;
            //        }
            //        return;

            //    }
            //    else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Bill") != null))
            //    {
            //        bill.isReviewed = true;
            //        bill.needReview = false;
            //        bill.stage = TransactionStage.AwaitingApproval.ToString();

            //        billRepo.update(bill);
            //        if (addinfo)
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //            _usersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
            //            addinfo = false;
            //        }
            //    }
            //    if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Bill") != null))
            //    {
            //        bill.isReviewed = true;
            //        bill.needReview = true;
            //        bill.stage = TransactionStage.AwaitingSecondReview.ToString();

            //        billRepo.update(bill);
            //        if (addinfo)
            //        {
            //            frmInputBox inputBox = new frmInputBox();
            //            inputBox.ShowDialog();
            //            _usersRepo.Add(TransactionInfo.Reviewed, bill.Id, (int)TransactionItemType.Bill, frmInputBox.comment);
            //            addinfo = false;
            //        }
            //    }
            //}

            loadcomments();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

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

            if (task != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && task.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        var commentId = procurementRepo.AddCommentLinkNotification(task.Id, TransactionItemType.ToDo_Task, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                        if (commentId != null)
                        {
                            foreach (var user in frmInputBox.Comment.TaggedList)
                            {
                                if (frmInputBox.FlagForTag == true)
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                            }

                            foreach (var user in frmInputBox.Comment.CCUsersList)
                            {
                                if (frmInputBox.FlagForCC == true)
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                else
                                    notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                            }
                        }


                        
                    }

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (task.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                }

            }
            loadcomments();
        }


        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (task.Id != 0)
                {
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

                    if (task != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.commentAdded == true && task.Id != 0)
                        {

                            var commentId = procurementRepo.AddCommentLinkNotification(task.Id, TransactionItemType.ToDo_Task, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId, frmInputBox.FlagId);
                            if (commentId != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForTag(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, user.id, "New Comment ", null, commentId.Value);
                                }

                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId, commentId.Value);
                                    else
                                        notificationsRepo.AddNotificationForCC(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, frmInputBox.comment, 0, user.id, "New Comment ", null, commentId.Value);

                                }
                            }

                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (task.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Task first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
        }


        private void Resend_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Resend_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();
            ProcurementRepo procurementRepo = new ProcurementRepo();

            var comment = grdCommentss.SelectedItem as CommentLog;


            if (comment != null)
            {
                comment = procurementRepo.GetComment(comment.Id);
                if (comment.employee.EmployeeUsers.FirstOrDefault(x => x.id == SYSTEM_STATIC.currentUser.id) == null)
                {
                    DXMessageBox.Show("Only sender of this comment can change Flag!");
                    return;
                }
                if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, taskGroup.users, TransactionItemType.ToDo_Task);
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }
                else
                {
                    frmInputBox inputBox = new frmInputBox();
                    inputBox.editFlag = true;
                    frmInputBox.Comment = comment;
                    inputBox.ShowDialog();
                }

                if (frmInputBox.commentAdded == true && task.Id != 0)
                {
                    comment.FlagId = frmInputBox.FlagId;
                    procurementRepo.UpdateCommentLinkNotification(comment);
                    
                    //procurementRepo.Add(travelingRecord.Id, TransactionItemType.ToDo_Task, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");

                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (task.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Task first to add a comment!");
                }
            }
            loadcomments();
        }

        public void loadcomments()
        {
            try
            {
                if (task != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(task.Id, TransactionItemType.ToDo_Task);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
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

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {
                if (grdAttach.Visibility == Visibility.Visible)
                    grdAttach.Visibility = Visibility.Collapsed;
                else
                {
                    cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();
                    grdAttach.Visibility = Visibility.Visible;
                }
            
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                if (task.Id != 0)
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(task.Id, TransactionItemType.ToDo_Task);
                }
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (task.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Target") != null))
            {
                if (DXMessageBox.Show("This So is currently in the list of Void Targets! Do you want to remove it from Void?", "Remove Void Target", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    task.isVoid = false;
                    taskRepo.UpdateStep(task);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Target has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if( taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                        {
                            winTagUsers win = new winTagUsers(taskGroup.users, taskGroup.Id, TransactionItemType.ToDo_Task);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task having Task Points: " + task.TaskPoints.ToString() + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Task UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.ToDo_Task, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Target") != null)
            {
                if (DXMessageBox.Show("This So is not currently in the list of Void Targets! Do you want to move it to Void Targets?", "Add to Void Targets", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    task.isVoid = true;
                    taskRepo.UpdateStep(task);

                    grdVoid.Visibility = Visibility.Visible;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Target has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                        {
                            winTagUsers win = new winTagUsers(taskGroup.users, taskGroup.Id, TransactionItemType.ToDo_Task);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task having Task Points: " + task.TaskPoints.ToString() + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Task Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.ToDo_Task, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + task.taskTargetType.TargetTypeName, task.Id, TransactionItemType.ToDo_Task, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }

        private void GrdCntrlSteps_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            var step = grdCntrlSteps.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;
            if (e.IsGetData)
                switch (e.Column.FieldName)
                {
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
                    case "SalesBudgetedMarginPercent":
                        if (step.TaskPoints > 0)
                        {
                            decimal percentage = 0;
                            if (step.TaskPoints != 0)
                            {
                                if (template == TaskGroupTemplate.Standard)
                                    percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);
                                else
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
                                if (template == TaskGroupTemplate.Standard)
                                    percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.SoAmountSER)), 2);
                                else
                                    percentage = Math.Round((Convert.ToDecimal(step.SalesBudgetedMargin) * 100 / Convert.ToDecimal(step.SoAmountSER)), 2);
                            }
                            
                            e.Value = percentage;
                        }
                        break;

                    case "Companies":

                        if (step.taskGroup != null)
                        {
                            var companies = String.Join(" | ", step.taskGroup.Companies.Select(x => x.CompanyName));
                            e.Value = companies;
                        }
                        break;

                    case "Departments":

                        if (step.taskGroup != null)
                        {
                            var depts = String.Join(" | ", step.taskGroup.Departments.Select(x => x.DeptName));
                            e.Value = depts;
                        }
                        break;
                        //case "AchievedPercentage":
                        //    var step = grdCntrlSteps.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;

                        //    if(step.TaskPoints > 0)
                        //    {
                        //        decimal percentage = 0;
                        //        if (template == TaskGroupTemplate.Standard)
                        //            percentage = Math.Round((Convert.ToDecimal(step.AchievedPoints) * 100 / Convert.ToDecimal(step.TaskPoints) ), 2);
                        //        else
                        //            percentage = Math.Round((Convert.ToDecimal(step.AchievedPoints) * 100 / Convert.ToDecimal(step.TaskPoints)), 2);
                        //        e.Value = percentage;
                        //    }
                        //    break;
                        //case "AchievedPercentage1":
                        //    var step1 = grdCntrlSteps.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;

                        //    if (step1.TaskPoints > 0)
                        //    {
                        //        decimal percentage = 0;
                        //        if (template == TaskGroupTemplate.Standard)
                        //            percentage = Math.Round((Convert.ToDecimal(step1.SystemPoints1) * 100 / Convert.ToDecimal(step1.TaskPoints)), 2);
                        //        else
                        //            percentage = Math.Round((Convert.ToDecimal(step1.AchievedPoints) * 100 / Convert.ToDecimal(step1.TaskPoints)), 2);
                        //        e.Value = percentage;
                        //    }
                        //    break;
                        //case "AchievedPercentage2":
                        //    var step2 = grdCntrlSteps.GetRowByListIndex(e.ListSourceRowIndex) as ToDoTask;

                        //    if (step2.TaskPoints > 0)
                        //    {
                        //        decimal percentage = 0;
                        //        if (template == TaskGroupTemplate.Standard)
                        //            percentage = Math.Round((Convert.ToDecimal(step2.SystemPoints2) * 100 / Convert.ToDecimal(step2.TaskPoints)), 2);
                        //        else
                        //            percentage = Math.Round((Convert.ToDecimal(step2.AchievedPoints) * 100 / Convert.ToDecimal(step2.TaskPoints)), 2);
                        //        e.Value = percentage;
                        //    }
                        //    break;
                }
        }

        private void TxtTaskPoints_KeyUp(object sender, KeyEventArgs e)
        {
            if(editFlag == true)
            {
                var totalTaskPoints = Convert.ToDouble(txtTaskPoints.Text);
                if(totalTaskPoints > 0)
                {
                    if(template == TaskGroupTemplate.Standard)
                    {
                        var totalAchvdPoints = Convert.ToDouble(grdCntrlSteps.Columns["AchievedPoints"].TotalSummaries[0].Value);
                        pbStatus.Value = Math.Round(totalAchvdPoints / totalTaskPoints * 100, 2);
                    }
                    else if (template == TaskGroupTemplate.Optional)
                    {
                        var totalAchvdPoints = Convert.ToDouble(grdCntrlSteps.Columns["AchievedPoints"].TotalSummaries[0].Value);
                        pbStatus.Value = Math.Round(totalAchvdPoints / totalTaskPoints * 100, 2);
                    }
                }
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (task != null && task.Id != 0)
                UsersRepo.Add(TransactionInfo.viewed, task.Id, (int)TransactionItemType.ToDo_Task, "Viewed details of Sale Order");
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;
                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                        var result = attachment.startDownload(str, TransactionItemType.ToDo_Task);
                        if (!string.IsNullOrEmpty(result.Item2))
                        {
                            Process.Start(result.Item2);
                            this.Dispatcher.Invoke(() =>
                            {
                                grdProgressBar.Visibility = Visibility.Collapsed;
                            });
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                            });
                        }
                    });
                    thread.Start();
                    //grdProgressBar.Visibility = Visibility.Collapsed;
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());
            }
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (taskId != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\ToDo_Task\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += taskId + "_" + TransactionItemType.ToDo_Task.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Sale_Order);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), taskId, TransactionItemType.ToDo_Task, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, task.Id, (int)TransactionItemType.ToDo_Task, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(taskId, TransactionItemType.ToDo_Task);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });
                                    }
                                });
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {



                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void LookUpAssignedTo_PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            var grid = lookUpAssignedTo.GetGridControl();
            //var users = grid.SelectedItems as List<User>;
            string selectedUsers = "";
            assignedToUsers = new List<User>();
            foreach (var _user in grid.SelectedItems)
            {
                var user = _user as User;

                selectedUsers = selectedUsers + user.userName + " | ";
                assignedToUsers.Add(user);
            }

            lookUpAssignedTo.EditValue = selectedUsers;
        }

        private void TreeListView1_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            //var grid = lookUpAssignedTo.GetGridControl();
            //var row = grid.GetRow(e.Node.RowHandle) as Department;

            //if (e.Node.IsChecked == true)
            //    deptList.Add(row);
            //else
            //    deptList.Remove(row);
        }

        private void TreeViewAssignedToUsers_Loaded(object sender, RoutedEventArgs e)
        {
            var grid = lookUpAssignedTo.GetGridControl();
            foreach (var _user in assignedToUsers)
            {
                grid.SelectItem(grid.FindRowByValue(grid.Columns.GetColumnByFieldName("id"), _user.id));
            }
        }

        private void TblTasksListView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if(!String.IsNullOrEmpty( txtTaskPoints.Text) && Convert.ToDouble(txtTaskPoints.Text) > 0)
            {
                if(template == TaskGroupTemplate.Standard)
                    pbStatus.Value = Math.Round(Convert.ToDouble(grdCntrlSteps.Columns["AchievedPoints"].TotalSummaries[0].Value) / Convert.ToDouble(txtTaskPoints.Text) * 100, 2);
                else if (template == TaskGroupTemplate.Optional)
                    pbStatus.Value = Math.Round(Convert.ToDouble(grdCntrlSteps.Columns["AchievedPoints"].TotalSummaries[0].Value) / Convert.ToDouble(txtTaskPoints.Text) * 100, 2);
            }

            PopulateSOField();
        }
        //comment
        private void TblTasksListView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if(e.Column.FieldName == "AchievedPoints")
            {
                grdCntrlSteps.SetCellValue(e.RowHandle, grdCntrlSteps.Columns["PointsUpdatedOn"], DateTime.Now);

                if(SYSTEM_STATIC.AllowedPermissions.Find(x=>x.Name == "Add Steps Without ReApproval") == null)
                {
                    var selectedItem = grdCntrlSteps.SelectedItem as ToDoTask;
                    if (selectedItem.isApproved == true)
                        grdCntrlSteps.SetCellValue(e.RowHandle, grdCntrlSteps.Columns["isReApproved"], false);
                }
            }
        }

        private void CmbField_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(datFromDate.Text) || datFromDate.Text == null)
            {
                DXMessageBox.Show("Please select From Date!");
                datFromDate.Focus();
                return;
            }
            if (String.IsNullOrEmpty(datToDate.Text) || datToDate.Text == null)
            {
                DXMessageBox.Show("Please select To Date!");
                datToDate.Focus();
                return;
            }

            ToDoTaskRepo repo = new ToDoTaskRepo();

            var gridControl = sender as GridControl;

            //var soField = cmbField.SelectedItem as cmbitem;
            //var propertyName = soField.description;
            var toDate = datToDate.DateTime;
            TimeSpan ts = new TimeSpan(23, 59, 59);
            toDate = toDate.Date + ts;
            //DateTime newDt = new DateTime(toDate.Year, toDate.Month, toDate.Day+1);
            var saleOrders = repo.getDepartmentalSOByDates(taskGroup.Companies, taskGroup.Departments, datFromDate.DateTime, toDate);
            var rowHandle = tblTasksListView.FocusedRowHandle;

            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["margin"], saleOrders.Sum(x => x.margin));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["BudgetedMargininBase"], saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble( x.exchangeRate)));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SalesBudgetedMargin"], saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.marginExchangeRate)));


            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["RevisedMargin"], saleOrders.Sum(x => x.RevisedMargin));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["RevisedMargininBase"], saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.exchangeRate)));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SalesRevisedMargin"], saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.marginExchangeRate)));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ActualMargin"], saleOrders.Sum(x =>x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ActualMargininBase"], saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.exchangeRate)));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SalesActualMargin"], saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.marginExchangeRate)));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["totalFOBValue"], saleOrders.Sum(x => x.totalFOBValue));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["totalCFRValue"], saleOrders.Sum(x => x.totalCFRValue));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["totalBaseCFRValue"], saleOrders.Sum(x => x.totalBaseCFRValue));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SoAmountSER"], saleOrders.Sum(x => x.SoAmountSER));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SoAmountPER"], saleOrders.Sum(x => x.SoAmountPER));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["commisioninBase"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * x.exchangeRate));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["commision"], saleOrders.Sum(x => x.commision));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["commisioninSE"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * Convert.ToDouble(x.marginExchangeRate)));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["systemMarginOC"], saleOrders.Sum(x => Convert.ToDouble(x.SystemMargin)));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["systemMarginSE"], saleOrders.Sum(x => Convert.ToDouble(x.SystemMargin) * Convert.ToDouble(x.marginExchangeRate)));
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["systemMarginME"], saleOrders.Sum(x => Convert.ToDouble(x.SystemMargin) * Convert.ToDouble(x.exchangeRate)));

            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedFromDate"], datFromDate.DateTime);
            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedToDate"], datToDate.DateTime);

            //if(tblTasksListView.FocusedColumn.FieldName == "SystemPoints" || tblTasksListView.FocusedColumn.FieldName == "SystemPoints1" || tblTasksListView.FocusedColumn.FieldName == "SystemPoints2")
            //{
            //    if (grdCntrlSteps.SelectedItem != null)
            //    {
            //        if (String.IsNullOrEmpty(datFromDate.Text) || datFromDate.Text == null)
            //        {
            //            DXMessageBox.Show("Please select From Date!");
            //            datFromDate.Focus();
            //            return;
            //        }
            //        if (String.IsNullOrEmpty(datToDate.Text) || datToDate.Text == null)
            //        {
            //            DXMessageBox.Show("Please select To Date!");
            //            datToDate.Focus();
            //            return;
            //        }
            //        if (cmbField.SelectedIndex < 0)
            //        {
            //            DXMessageBox.Show("Please SO Field!");
            //            cmbField.Focus();
            //            return;
            //        }
            //        if (taskGroup.Companies == null || taskGroup.Companies.Count == 0)
            //        {
            //            DXMessageBox.Show("This group does not contain any Companies!");
            //            return;
            //        }
            //        if (taskGroup.Departments == null || taskGroup.Departments.Count == 0)
            //        {
            //            DXMessageBox.Show("This group does not contain any Departments!");
            //            return;
            //        }
            //        var soField = cmbField.SelectedItem as cmbitem;
            //        var propertyName = soField.description;
            //        var saleOrders = taskRepo.getDepartmentalSOByDates(taskGroup.Companies, taskGroup.Departments, datFromDate.DateTime, datToDate.DateTime);
            //        var systemPoints = saleOrders.Sum(x => Convert.ToDouble(x.GetPropertyValue(propertyName)));

            //        var rowHandle = tblTasksListView.FocusedRowHandle;
            //        var fieldName = tblTasksListView.FocusedColumn.FieldName;

            //        switch (fieldName)
            //        {
            //            case "SystemPoints":
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["GroupCompanies"], String.Join(" | ", taskGroup.Companies.Select(x => x.CompanyName)));
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["GroupDepartments"], String.Join(" | ", taskGroup.Departments.Select(x => x.DeptName)));
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedFromDate"], datFromDate.DateTime);
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedToDate"], datToDate.DateTime);
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SOField"], soField.name);
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns[fieldName], systemPoints);
            //                break;
            //            case "SystemPoints1":
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["GroupCompanies"], String.Join(" | ", taskGroup.Companies.Select(x => x.CompanyName)));
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["GroupDepartments"], String.Join(" | ", taskGroup.Departments.Select(x => x.DeptName)));
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedFromDate1"], datFromDate.DateTime);
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedToDate1"], datToDate.DateTime);
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SOField1"], soField.name);
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns[fieldName], systemPoints);
            //                break;
            //            case "SystemPoints2":
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["GroupCompanies"], String.Join(" | ", taskGroup.Companies.Select(x => x.CompanyName)));
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["GroupDepartments"], String.Join(" | ", taskGroup.Departments.Select(x => x.DeptName)));
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedFromDate2"], datFromDate.DateTime);
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedToDate2"], datToDate.DateTime);
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SOField2"], soField.name);
            //                grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns[fieldName], systemPoints);
            //                break;
            //        }

            //        tblTasksListView.FocusedRowHandle = -1;
            //    }
            //    else
            //    {
            //        DXMessageBox.Show("Please select an item from the Steps Register!");
            //        return;
            //    }
            //}
            //else
            //{
            //    DXMessageBox.Show("Please Select the right column for Loading Sale Order Data!");
            //}
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCntrlSteps);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if(editFlag == true)
            {

                taskRepo = new ToDoTaskRepo();

                if(panelGroupImages.Children != null && panelGroupImages.Children.Count > 0)
                    panelGroupImages.Children.Clear();
                LoadData();
                //ShowSteps(Convert.ToInt32(txtTaskId.Text));
                //LoadCounters();
                txtHeading.Text = "Approved Steps";
            }
        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = grdCntrlSteps.SelectedItem as ToDoTask;
            if(selectedItem != null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Steps Without Approval") != null && selectedItem.isApproved == false)
                {
                    if (DXMessageBox.Show("This Target Step is currently under Approval Targets! Do you want to remove it from Pending for Approvals?", "Approve Target Step", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        selectedItem.isApproved = true;
                        selectedItem.ApprovedDate = DateTime.Now;
                        taskRepo.UpdateStep(selectedItem);
                        DXMessageBox.Show("Target step has been Approved!");
                    }
                }
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Steps Without ReApproval") != null && selectedItem.isReApproved == false)
                {
                    if (DXMessageBox.Show("This Target Step is currently under ReApproval Targets! Do you want to remove it from Pending for ReApprovals?", "ReApprove Target Step", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        selectedItem.isReApproved = true;
                        selectedItem.ReApprovalDate = DateTime.Now;
                        taskRepo.UpdateStep(selectedItem);
                        DXMessageBox.Show("Target step has been Re-Approved!");
                    }
                }
                else
                {
                    DXMessageBox.Show("Permission required to Approve or ReApprove Targets!");
                    return;
                }
            }
            else
            {
                DXMessageBox.Show("Please select Target to Approve!");
            }
            
        }

        private void MbtnPendingForApproval_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Pending for Approval Steps") != null)
            {
                grdCntrlSteps.ItemsSource = steps.Where(x => x.isApproved == false);
                txtHeading.Text = "Pending For Approval Targets";
            }
            else
            {
                DXMessageBox.Show("Permission required to View Pending for Approval Steps!");
                return;
            }
        }

        private void LoadCounters()
        {
            mbtnApprovalsCount.Header = taskRepo.GetPendingForApprovalStepCount(Convert.ToInt32(txtTaskId.Text));
            mbtnReApprovalsCount.Header = taskRepo.GetPendingForReApprovalStepCount(Convert.ToInt32(txtTaskId.Text));
            mbtnVoidCounter.Header = taskRepo.GetVoidStepsCount(Convert.ToInt32(txtTaskId.Text));
        }

        private void MbtnReApproval_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Pending for ReApproval Steps") != null)
            {
                try
                {
                    grdCntrlSteps.ItemsSource = steps.Where(x => x.isReApproved == false);
                    txtHeading.Text = "Pending For Re-Approval Targets";
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

        private void TblTasksListView_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            //PopulateSOField();  
        }

        private void PopulateSOField()
        {
            var fieldName = tblTasksListView.FocusedColumn.FieldName;
            var row = grdCntrlSteps.GetFocusedRow() as ToDoTask;

            if (row != null)
            {

                if (row.searchedFromDate != null)
                    datFromDate.EditValue = row.searchedFromDate;
                else
                    datFromDate.EditValue = null;

                if (row.searchedToDate != null)
                    datToDate.EditValue = row.searchedToDate;
                else
                    datToDate.EditValue = null;

            }
            

            //switch (fieldName)
            //{
            //    case "SystemPoints":

            //        if (row != null)
            //        {
            //            if (row.searchedFromDate != null)
            //                datFromDate.EditValue = row.searchedFromDate;
            //            else
            //                datFromDate.EditValue = null;

            //            if (row.searchedToDate != null)
            //                datToDate.EditValue = row.searchedToDate;
            //            else
            //                datToDate.EditValue = null;

            //            if (!String.IsNullOrEmpty(row.SOField))
            //                cmbField.SelectedIndex = SoFields.FindIndex(x => x.name == row.SOField);
            //            else
            //                cmbField.Text = null;
            //        }
            //        break;
            //    case "SystemPoints1":
            //        if (row != null)
            //        {
            //            if (row.searchedFromDate1 != null)
            //                datFromDate.EditValue = row.searchedFromDate1;
            //            else
            //                datFromDate.EditValue = null;

            //            if (row.searchedToDate1 != null)
            //                datToDate.EditValue = row.searchedToDate1;
            //            else
            //                datToDate.EditValue = null;

            //            if (!String.IsNullOrEmpty(row.SOField1))
            //                cmbField.SelectedIndex = SoFields.FindIndex(x => x.name == row.SOField1);
            //            else
            //                cmbField.Text = null;
            //        }
            //        break;
            //    case "SystemPoints2":
            //        if (row != null)
            //        {
            //            if (row.searchedFromDate2 != null)
            //                datFromDate.EditValue = row.searchedFromDate2;
            //            else
            //                datFromDate.EditValue = null;

            //            if (row.searchedToDate2 != null)
            //                datToDate.EditValue = row.searchedToDate2;
            //            else
            //                datToDate.EditValue = null;

            //            if (!String.IsNullOrEmpty(row.SOField2))
            //                cmbField.SelectedIndex = SoFields.FindIndex(x => x.name == row.SOField2);
            //            else
            //                cmbField.Text = null;
            //        }
            //        break;
            //}
        }

        private void MenuItemVoidStep_Click(object sender, RoutedEventArgs e)
        {
            var step = grdCntrlSteps.SelectedItem as ToDoTask;
            if (step.isVoid != true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Step") != null))
            {
                if (DXMessageBox.Show("This Step is currently not in the list of Void Targets! Do you want to Mark this Step as Void?", "Void Step", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    step.isVoid = true;
                }
            }
        }

        private void MbtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View list of Void Steps") != null)
            {
                try
                {
                    grdCntrlSteps.ItemsSource = steps.Where(x => x.isVoid == true);
                    txtHeading.Text = "Void Steps";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Void Steps!");
                return;
            }
        }

        private void MbtnRefreshSystemPoints_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < grdCntrlSteps.VisibleRowCount; i++)
            {
                
                int rowHandle = grdCntrlSteps.GetRowHandleByVisibleIndex(i);
                var _row = grdCntrlSteps.GetRow(rowHandle);
                var _step = _row as ToDoTask;

                ToDoTaskRepo repo = new ToDoTaskRepo();
                if (_step != null && _step.Id > 0)
                {
                    List<SaleOrder> saleOrders = new List<SaleOrder>();
                    if (_step.searchedFromDate != null && _step.searchedToDate != null)
                    {
                        if(_step.taskGroup.CompaniesBulk != null && _step.taskGroup.CompaniesBulk.Count() > 0 && _step.taskGroup.DepartmentsBulk != null && _step.taskGroup.DepartmentsBulk.Count() > 0)
                        {
                            var toDate = _step.searchedToDate.Value;
                            TimeSpan ts = new TimeSpan(23, 59, 59);
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
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["margin"], /*saleOrders.Sum(x => x.margin)*/margin);

                            var MarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["BudgetedMargininBase"], MarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var MarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SalesBudgetedMargin"], MarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);

                            var revMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) /*+ (principalSO.Sum(x => x.RevisedMargin))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["RevisedMargin"], /*saleOrders.Sum(x => x.RevisedMargin)*/revMargin);

                            var revMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["RevisedMargininBase"], revMarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var revMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SalesRevisedMargin"], /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/revMarginSE);

                            var actualMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) /*+ (principalSO.Sum(x => x.ActualMargin))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ActualMargin"], /*saleOrders.Sum(x => x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin)))*/ actualMargin);

                            var actualMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ActualMargininBase"], actualMarginME/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var actualMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SalesActualMargin"], actualMarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);


                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["totalFOBValue"], saleOrders.Sum(x => x.totalFOBValue));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["totalCFRValue"], saleOrders.Sum(x => x.totalCFRValue));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["totalBaseCFRValue"], saleOrders.Sum(x => x.totalBaseCFRValue));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SoAmountSER"], saleOrders.Sum(x => x.SoAmountSER));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SoAmountPER"], saleOrders.Sum(x => x.SoAmountPER));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["commisioninBase"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * x.exchangeRate));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["commision"], saleOrders.Sum(x => x.commision));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["commisioninSE"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * Convert.ToDouble(x.marginExchangeRate)));

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["systemMarginOC"], saleOrders.Sum(x => Convert.ToDouble(x.SystemMargin)));
                            var systemMarginSE = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["systemMarginSE"], systemMarginSE);
                            var systemMarginME = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["systemMarginME"], systemMarginME);

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["netCommision"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision)));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["netCommisionSER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["netCommisionMER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)));

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["BMgrossProfitSE"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)) + Convert.ToDouble(MarginSE)));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["BMgrossProfitME"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)) + Convert.ToDouble(MarginME)));

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["TotalQuantity"], saleOrders.Sum(x => Convert.ToDouble(x.TotalQuantity)));

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["TotalOrdersCount"], TotalCount);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["UnderApprovalOrdersCount"], UnderApprovalCount);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ApprovedOrdersCount"], ApprovedCount);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["UnderClosingOrdersCount"], UnderClosingCount);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ClosedOrdersCount"], ClosedCount);

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedFromDate"], _step.searchedFromDate);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedToDate"], _step.searchedToDate);
                        }
                        else
                        {
                            var toDate = _step.searchedToDate.Value;
                            TimeSpan ts = new TimeSpan(23, 59, 59);
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
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["margin"], /*saleOrders.Sum(x => x.margin)*/margin);

                            var MarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["BudgetedMargininBase"], MarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var MarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalBudgetedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.margin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SalesBudgetedMargin"], MarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalBudgetedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);

                            var revMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin))) /*+ (principalSO.Sum(x => x.RevisedMargin))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["RevisedMargin"], /*saleOrders.Sum(x => x.RevisedMargin)*/revMargin);

                            var revMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["RevisedMargininBase"], revMarginME /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var revMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalRevisedMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.RevisedMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SalesRevisedMargin"], /*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalRevisedMargin))) * Convert.ToDouble(x.marginExchangeRate))*/revMarginSE);

                            var actualMargin = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin))) /*+ (principalSO.Sum(x => x.ActualMargin))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ActualMargin"], /*saleOrders.Sum(x => x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin)))*/ actualMargin);

                            var actualMarginME = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.exchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.exchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ActualMargininBase"], actualMarginME/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.exchangeRate))*/);

                            var actualMarginSE = supplyCCCSO.Sum(x => (Convert.ToDecimal(x.costCenterAmount) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => (Convert.ToDecimal(x.totalCFRValue) - (x.CostSheet == null ? 0 : x.CostSheet.TotalActualMargin)) * Convert.ToDecimal(x.marginExchangeRate)) /*+ (principalSO.Sum(x => x.ActualMargin * Convert.ToDecimal(x.marginExchangeRate)))*/;
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SalesActualMargin"], actualMarginSE/*saleOrders.Sum(x => (x.totalCFRValue - (x.CostSheet == null ? 0 : Convert.ToDouble(x.CostSheet.TotalActualMargin))) * Convert.ToDouble(x.marginExchangeRate))*/);


                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["totalFOBValue"], saleOrders.Sum(x => x.totalFOBValue));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["totalCFRValue"], saleOrders.Sum(x => x.totalCFRValue));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["totalBaseCFRValue"], saleOrders.Sum(x => x.totalBaseCFRValue));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SoAmountSER"], saleOrders.Sum(x => x.SoAmountSER));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["SoAmountPER"], saleOrders.Sum(x => x.SoAmountPER));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["commisioninBase"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * x.exchangeRate));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["commision"], saleOrders.Sum(x => x.commision));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["commisioninSE"], saleOrders.Sum(x => Convert.ToDouble(x.commision) * Convert.ToDouble(x.marginExchangeRate)));

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["systemMarginOC"], saleOrders.Sum(x => Convert.ToDouble(x.SystemMargin)));
                            var systemMarginSE = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccSER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.marginExchangeRate));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["systemMarginSE"], systemMarginSE);
                            var systemMarginME = supplyCCCSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.ccMER)) + nonPrincipalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate)) + principalSO.Sum(x => Convert.ToDecimal(x.SystemMargin) * Convert.ToDecimal(x.exchangeRate));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["systemMarginME"], systemMarginME);

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["netCommision"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision)));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["netCommisionSER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["netCommisionMER"], saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)));

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["BMgrossProfitSE"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.marginExchangeRate)) + Convert.ToDouble(MarginSE)));
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["BMgrossProfitME"], (saleOrders.Sum(x => Convert.ToDouble(x.netCommision) * Convert.ToDouble(x.exchangeRate)) + Convert.ToDouble(MarginME)));

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["TotalQuantity"], saleOrders.Sum(x => Convert.ToDouble(x.TotalQuantity)));

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["TotalOrdersCount"], TotalCount);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["UnderApprovalOrdersCount"], UnderApprovalCount);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ApprovedOrdersCount"], ApprovedCount);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["UnderClosingOrdersCount"], UnderClosingCount);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["ClosedOrdersCount"], ClosedCount);

                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedFromDate"], _step.searchedFromDate);
                            grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["searchedToDate"], _step.searchedToDate);
                        }
                    }

                    if(_step.calculationType != null)
                    {
                        var achieved = Convert.ToDouble(grdCntrlSteps.GetCellValue(rowHandle, _step.calculationType.AchievedField.SOFieldName));
                        var total = Convert.ToDouble(grdCntrlSteps.GetCellValue(rowHandle, _step.calculationType.TotalField.SOFieldName));
                        var result = Math.Round((achieved / total) * 100, 0);
                        grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["TargetAchievedPercenatage"], result);

                        var status = repo.GetTaskStatusByPercentage(result);
                        if (status != null)
                        {
                            _step.statusId = status.Id;
                        }
                    }
                    else if (_step.taskGroup != null && _step.taskGroup.calculationType != null)
                    {
                        var achieved = Convert.ToDouble(grdCntrlSteps.GetCellValue(rowHandle, _step.taskGroup.calculationType.AchievedField.SOFieldName));
                        var total = Convert.ToDouble(grdCntrlSteps.GetCellValue(rowHandle, _step.taskGroup.calculationType.TotalField.SOFieldName));
                        var result = Math.Round((achieved / total) * 100, 0);
                        grdCntrlSteps.SetCellValue(rowHandle, grdCntrlSteps.Columns["TargetAchievedPercenatage"], result);
                       
                        var status = repo.GetTaskStatusByPercentage(result);
                        if (status != null)
                        {
                            _step.statusId = status.Id;
                        }
                    }
                    taskRepo.UpdateStep(_step);
                }
            }
        }        

        double salesBMTotal1 = 0, salesBMTotal2 = 0, SOamountSERtotal = 0, totalTaskPoints = 0;

        private void BtnMakeCopy_Click(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Do you want to make the copy of this Target?", "Copy this Target", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (editFlag == true && task != null && task.Id > 0)
                {
                    //taskRepo = new ToDoTaskRepo();
                    task.stepCount = steps.Count;
                    var visibleItems = grdCntrlSteps.VisibleItems;
                    int count = 0;

                    double achievedPoints = 0;
                    double systemPoints = 0;

                    achievedPoints = steps.Where(x => x.isVoid != true).Sum(x => x.AchievedPoints);
                    systemPoints = steps.Where(x => x.isVoid != true).Sum(x => x.SystemPoints);
                    count = steps.Where(x => x.isCompleted == true).Count();

                    task.achievedStepsCount = count;
                    if (template == TaskGroupTemplate.Standard)
                        task.PercentageAchieved = Math.Round((achievedPoints / Convert.ToDouble(txtTaskPoints.Text) * 100), 2);
                    else
                        task.PercentageAchieved = Math.Round((achievedPoints / Convert.ToDouble(txtTaskPoints.Text) * 100), 2);

                    task.AchievedPoints = Math.Round(achievedPoints, 2);
                    task.SystemPoints = Math.Round(systemPoints, 2);

                    taskRepo.CopyTask(task, steps);

                    DXMessageBox.Show("Copy of this Target has been Saved!");

                    Window win = Window.GetWindow(this);
                    win.Close();
                }
            }
                
            
        }

        private void GrdCntrlSteps_CustomSummary(object sender, DevExpress.Data.CustomSummaryEventArgs e)
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
                            salesBMTotal1 += Convert.ToDouble(grdCntrlSteps.GetCellValue(e.RowHandle, grdCntrlSteps.Columns["SalesBudgetedMargin"]));
                            SOamountSERtotal += Convert.ToDouble(grdCntrlSteps.GetCellValue(e.RowHandle, grdCntrlSteps.Columns["SoAmountSER"]));

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
                            salesBMTotal2 += Convert.ToDouble(grdCntrlSteps.GetCellValue(e.RowHandle, grdCntrlSteps.Columns["SalesBudgetedMargin"]));
                            totalTaskPoints += Convert.ToDouble(grdCntrlSteps.GetCellValue(e.RowHandle, grdCntrlSteps.Columns["TaskPoints"]));

                            //Total = debitTotal - creditTotal;
                            break;
                        case CustomSummaryProcess.Finalize:

                            e.TotalValue = Math.Round((salesBMTotal2 / totalTaskPoints) * 100, 2);
                            break;
                    }
                }
            }
        }
    }
}
