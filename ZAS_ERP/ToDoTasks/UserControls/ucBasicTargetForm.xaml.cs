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
    /// Interaction logic for ucBasicTargetForm.xaml
    /// </summary>
    public partial class ucBasicTargetForm : UserControl
    {
        ToDoTask step = new ToDoTask();
        public int stepId = 0;
        TaskGroups taskGroup = new TaskGroups();
        int taskGroupId = 0;
        List<ViewInfo> views = new List<ViewInfo>();
        bool addinfo = true;
        UsersRepo UsersRepo = new UsersRepo();
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public bool editFlag = false;
        TaskGroupTemplate template;
        public ucBasicTargetForm()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadStepData();
                LoadTargetData();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "On Load Data");
            }
        }

        private void LoadStepStatuses()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
            //BillRepo billRepo = new BillRepo();
            List<ToDoTaskStatus> taskStatuses = new List<ToDoTaskStatus>();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Statuses in Targets") != null)
                taskStatuses = taskRepo.GetAllStepStatuses();
            else
                taskStatuses = taskRepo.GetAllStepStatuses().Where(x => x.isActive == true).ToList();

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
            cmbStepStatus.ItemsSource = cmbitems;
        }

        private void LoadTargetTypes()
        {
            lookUpTargetType.ItemsSource = taskRepo.GetAllTargetTypes();
        }

        private void LoadStatuses()
        {
            List<cmbitem> cmbitems = new List<cmbitem>();
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

        private void LoadTargetGroups()
        {
            lookUpTargetGroup.ItemsSource = taskRepo.GetAllTargetGroups();
        }

        private void LoadStepData()
        {
            LoadStepStatuses();
            if (editFlag == true && stepId > 0)
            {

                step = taskRepo.GetTask(stepId);

                //Select Status
                var statusList = (cmbStepStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStepStatus.ItemsSource as List<cmbitem>;
                if (step.Status != null)
                {
                    int index = 0;
                    foreach (var _status in statusList)
                    {
                        if (_status.id == step.statusId)
                        {
                            cmbStepStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

              

                if (step.targetGroup != null)
                    lookUpTargetGroup.Text = step.targetGroup.GroupName;
                else
                    lookUpTargetGroup.SelectedIndex = -1;


                txtStepPoints.Text = step.TaskPoints.ToString();

                if (step.TargetYear != null)
                    datStepTargetYear.EditValue = step.TargetYear.Value;
                else
                    datStepTargetYear.EditValue = null;

                if (step.TargetMonth != null)
                    datStepTargetMonth.EditValue = step.TargetMonth.Value;
                else
                    datStepTargetMonth.EditValue = null;

               
            }
        }

        private void LoadTargetData()
        {
            
            LoadTargetTypes();
            LoadStatuses();
            LoadTargetGroups();


            if (editFlag == true && step != null)
            {
               
                if (step.taskGroupId != null && step.taskGroupId > 0)
                {
                    taskGroupId = step.taskGroupId.Value;
                    taskGroup = taskRepo.GetTaskGroup(taskGroupId);
                    LoadUsers();
                    if (taskGroup.Companies != null && taskGroup.Companies.Count > 0 && taskGroup.Departments != null && taskGroup.Departments.Count > 0)
                    {
                        txtCompanies.Text = String.Join(" | ", taskGroup.Companies.Select(x => x.CompanyName));
                        txtDepartments.Text = String.Join(" | ", taskGroup.Departments.Select(x => x.DeptName));

                        template = TaskGroupTemplate.Standard;
                    }
                    else
                        template = TaskGroupTemplate.Optional;

                    lblGroupName.Text = taskGroup.GroupName;
                }

                if (step.ParentTask.isVoid == true)
                    grdVoid.Visibility = Visibility.Visible;
                else
                    grdVoid.Visibility = Visibility.Collapsed;

                if (step.taskGroupId != null)
                    taskGroupId = step.taskGroupId.Value;

                txtTaskId.Text = step.ParentTask.Id.ToString();
                if (step.ParentTask.TaskName != null)
                    txtSelectedTaskName.Text = step.ParentTask.TaskName;
                else
                    txtSelectedTaskName.Text = "";

                if (step.ParentTask.TaskDescription != null)
                    txtTaskDescription.Text = step.ParentTask.TaskDescription;
                else
                    txtTaskDescription.Text = "";

                if (step.ParentTask.taskTargetType != null)
                    lookUpTargetType.Text = step.ParentTask.taskTargetType.TargetTypeName;
                else
                    lookUpTargetType.SelectedIndex = -1;

                if (step.ParentTask.targetGroup != null)
                    lookUpTargetGroup.Text = step.ParentTask.targetGroup.GroupName;
                else
                    lookUpTargetGroup.SelectedIndex = -1;

                //if (task.assignedTo != null)
                //    lookUpAssignedTo.Text = task.assignedTo.userName;
                //else
                //    lookUpAssignedTo.SelectedIndex = -1;


                //var grid = lookUpAssignedTo.GetGridControl();
                //foreach (var _user in task.assignedToUsers)
                //{
                //    grid.SelectItem(grid.FindRowByValue(grid.Columns.GetColumnByFieldName("id"), _user.id));
                //}

                if (step.ParentTask.creationDate != null)
                    datCreationDate.EditValue = step.ParentTask.creationDate;

                if (step.ParentTask.TargetYear != null)
                    datTargetYear.EditValue = step.ParentTask.TargetYear.Value;
                else
                    datTargetYear.EditValue = null;

                if (step.ParentTask.TargetMonth != null)
                    datTargetMonth.EditValue = step.ParentTask.TargetMonth.Value;
                else
                    datTargetMonth.EditValue = null;


                //Select Status
                var statusList = (cmbTaskStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbTaskStatus.ItemsSource as List<cmbitem>;
                if (step.ParentTask.Status != null)
                {
                    int index = 0;
                    foreach (var _status in statusList)
                    {
                        if (_status.id == step.ParentTask.statusId)
                        {
                            cmbTaskStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
            }
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
            if (gridTracker.Visibility == Visibility.Collapsed && step.Id != 0)
            {

                views = UsersRepo.getViwerInfo(step.Id, (int)TransactionItemType.TargetStep);
                grdUsers.ItemsSource = views;
                gridTracker.Visibility = Visibility.Visible;
            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        { }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            List<User> groupUsers = new List<User>();

            if (taskGroup != null && taskGroup.usersBulk != null && taskGroup.usersBulk.Count > 0)
                groupUsers = taskGroup.usersBulk;
            else if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count > 0)
                groupUsers = taskGroup.users;

            if (groupUsers != null && groupUsers.Count != 0)
            {
                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, groupUsers, TransactionItemType.TargetStep);
                inputBox.ShowDialog();
            }
            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (step != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && step.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + (step.taskTargetType == null? "N/A" : step.taskTargetType.TargetTypeName), step.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task with Target Type: " + (step.taskTargetType == null ? "N/A" : step.taskTargetType.TargetTypeName), step.Id, TransactionItemType.TargetStep, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Task with Target Type:" + (step.taskTargetType == null ? "N/A" : step.taskTargetType.TargetTypeName), step.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Task with Target Type:" + (step.taskTargetType == null ? "N/A" : step.taskTargetType.TargetTypeName), step.Id, TransactionItemType.TargetStep, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    procurementRepo.Add(step.Id, TransactionItemType.TargetStep, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    //frmInputBox.taggedUsers = new List<User>();
                    loadcomments();
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (step.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Sale Order first to add a comment!");
                }

            }
            loadcomments();
        }

        public void loadcomments()
        {
            try
            {
                if (step != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(step.Id, TransactionItemType.TargetStep);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void LoadUsers()
        {
            if (taskGroupId > 0)
            {
                var groupUsers = taskRepo.GetTaskGroup(taskGroupId).users;
                //lookUpAssignedTo.ItemsSource = groupUsers;
                //lookUpSupervisedBy.ItemsSource = groupUsers;
                //lookUpSalesHead.ItemsSource = groupUsers;

                foreach (var _user in groupUsers)
                {
                    if (_user.employee.person.Photo != null)
                        panelGroupImages.Children.Add(new ImageEdit()
                        {
                            Source = GetBitmapImageFromByteArray(_user.employee.person.Photo),
                            Height = 50,
                            Width = 50,
                            Stretch = Stretch.Fill,
                            Margin = new Thickness(5, 0, 0, 0),
                            ShowMenu = false,
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
                if (step.Id != 0)
                {
                    treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(step.Id, TransactionItemType.TargetStep);
                }
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (step.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Target") != null))
            {
                if (DXMessageBox.Show("This So is currently in the list of Void Targets! Do you want to remove it from Void?", "Remove Void Target", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    step.isVoid = false;
                    taskRepo.UpdateStep(step);

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
                        if (taskGroup != null && taskGroup.users != null && taskGroup.users.Count != 0)
                        {
                            winTagUsers win = new winTagUsers(taskGroup.users, taskGroup.Id, TransactionItemType.TargetStep);
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
                        Comment = "Task having Task Points: " + step.TaskPoints.ToString() + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Task UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(step.Id, TransactionItemType.TargetStep, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + step.taskTargetType.TargetTypeName, step.Id, TransactionItemType.TargetStep, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + step.taskTargetType.TargetTypeName, step.Id, TransactionItemType.TargetStep, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Target Target") != null)
            {
                if (DXMessageBox.Show("This So is not currently in the list of Void Targets! Do you want to move it to Void Targets?", "Add to Void Targets", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    step.isVoid = true;
                    taskRepo.UpdateStep(step);

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
                            winTagUsers win = new winTagUsers(taskGroup.users, taskGroup.Id, TransactionItemType.TargetStep);
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
                        Comment = "Task having Task Points: " + step.TaskPoints.ToString() + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Task Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(step.Id, TransactionItemType.TargetStep, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + step.taskTargetType.TargetTypeName, step.Id, TransactionItemType.TargetStep, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task with Target Type: " + step.taskTargetType.TargetTypeName, step.Id, TransactionItemType.TargetStep, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
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
                        var result = attachment.startDownload(str, TransactionItemType.TargetStep);
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
                if (stepId != 0)
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
                        destination += "Attachments\\TargetStep\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += stepId + "_" + TransactionItemType.TargetStep.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

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
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), stepId, TransactionItemType.TargetStep, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, step.Id, (int)TransactionItemType.TargetStep, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(stepId, TransactionItemType.TargetStep);
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

        private void BtnOpenTargetForm_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Target") != null)
            {
                if (step != null && step.ParentTask != null)
                {
                    ucFrmAddTask frmAddTask = new ucFrmAddTask();
                    frmAddTask.taskId = step.parentTaskId.Value;
                    frmAddTask.taskGroupId = step.taskGroupId.Value;
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
    }
}
