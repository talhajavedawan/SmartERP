using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.HR;
using Microsoft.Win32;
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
using ZAS_ERP.Employee;

namespace ZAS_ERP.Leaves
{
    /// <summary>
    /// Interaction logic for frmLeaveApplication.xaml
    /// </summary>
    public partial class frmLeaveApplication : UserControl
    {
        List<ViewInfo> views = new List<ViewInfo>();

        public HrRepo hrRepo = new HrRepo();
        public double annualBalance = 0;
        public double casualBalance = 0;
        public double adjustmentBalance = 0;

        public double totalBalance = 0;
        public double appliedDays = 0;
        List<User> UsersForComments = new List<User>();
        //List<User> UsersForComments = new List<User>();
        NotificationsRepo notificationsRepo = new NotificationsRepo();

        UsersRepo UsersRepo = new UsersRepo();
        public int empIdInt;
        public bool isEdit = false;
        public bool isNotif = false;
        public int leaveIdNotif = 0;
        public int leaveId = 0;

        public bool isAdmin = false;
        public ERP_BL.Databases.Employee selectedEmployee = null;
        public LeaveApplication app = null;
        public EmployeeRepo empRepo = new EmployeeRepo();
        public int  index = -1;
        public bool isAppr = false;
        public LeaveType leaveType = new LeaveType();
        public bool isHalf = false;
        public double originalLeaveDays = 0;
        bool inputFormCloseFlag = false;


        public frmLeaveApplication()
        {
            InitializeComponent();
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {
            GenerateUsersForComments();
            //department = cmbxDepartments.SelectedItem as Department;
            //if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
            //{
            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersForComments, TransactionItemType.Leave);
            inputBox.ShowDialog();

            //}
            //else
            //{
            // frmInputBox inputBox = new frmInputBox();
            //inputBox.ShowDialog();
            //}

            if (leaveIdStr.Text != "")
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && leaveIdStr.Text != "")
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Leave #" + leaveIdStr.Text, Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Leave #" + leaveIdStr.Text, Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Leave #" + leaveIdStr.Text, Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " mentioned you in Leave #" + leaveIdStr.Text, Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    procurementRepo.Add(Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    MessageBox.Show("Comment Added!");
                    loadcomments();
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (leaveIdStr.Text == "")
                {
                    DXMessageBox.Show("Kindly save Leave first to add a comment!");
                }

            }
        }
        public void loadcomments()
        {
            try
            {
                if (leaveIdStr.Text != "")
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave);
                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }
        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //department = cmbxDepartments.SelectedItem as Department;
            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (Convert.ToInt32(leaveIdStr.Text) > 0)
                {
                    GenerateUsersForComments();
                    //        if (department != null && department.Id != 0 /*&& department.users!=null&& department.users.Count!=0*/)
                    //        {
                    //            frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByDepartment(department.Id), comment, TransactionItemType.FixedAssets);
                    //            inputBox.ShowDialog();

                    //        }
                    //        else
                    //        {
                    frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, UsersForComments, comment, TransactionItemType.Leave);
                    inputBox.ShowDialog();
                    // }

                    if (Convert.ToInt32(leaveIdStr.Text) > 0)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.comment != "" && leaveIdStr.Text != "")
                        {
                            if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Leave #" + leaveIdStr.Text, Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Leave #" + leaveIdStr.Text, Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.comment, user.id, "New Comment ", null);
                                }
                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForCC == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Fixed Leave #" + leaveIdStr.Text, Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Fixed Leave #" + leaveIdStr.Text, Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.comment, user.id, "New Comment ", null);
                                }
                            }

                            procurementRepo.Add(Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);

                            loadcomments();
                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (leaveIdStr.Text == "")
                        {
                            DXMessageBox.Show("Kindly save Leave first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);
                }
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
                grdAttach.Visibility = Visibility.Visible;
            }
        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var id = leaveIdStr.Text;
                if (!String.IsNullOrEmpty(id))
                {
                    var leave = hrRepo.GetLeaveApplication(Convert.ToInt32(id));

                    if (leave.isVoid == false)
                    {

                        var res = DXMessageBox.Show("Do you sure to want to mark this Leave void?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (MessageBoxResult.Yes == res)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Leave") != null || (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void Unapproved own Leave")!= null  && isAppr == false))
                            {
                                leave.isVoid = true;
                                //leave.isApproved = null;
                                
                                leave.leave.LeaveDays = 0;
                                hrRepo.UpdateLeaveApplication(leave);
                                DXMessageBox.Show("Leave is being marked as void successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                var win = Window.GetWindow(frmLeaveApp);
                                win.Close();
                            }
                            else
                            {
                                DXMessageBox.Show("You are not allowed to Mark the Leave void", "Unauthorize", MessageBoxButton.OK, MessageBoxImage.Stop);
                            }
                            return;
                        }
                    }

                    if (leave.isVoid == true)
                    {

                        var res = DXMessageBox.Show("Do you sure to want to Un-Mark this Leave from void?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (MessageBoxResult.Yes == res)
                        {
                            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Leave") != null || (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void Unapproved own Leave") != null && isAppr == false))
                            {
                                leave.isVoid = false;
                                if (!String.IsNullOrEmpty(txtTotalDays.Text))
                                {
                                    var days = Convert.ToDouble(txtTotalDays.Text);
                                    //days = days + 1;
                                    leave.leave.LeaveDays = 0-days;
                                }
                               
                                hrRepo.UpdateLeaveApplication(leave);
                                DXMessageBox.Show("Leave is being marked as Un-void successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                var win = Window.GetWindow(frmLeaveApp);
                                win.Close();
                                //this.Close();
                            }
                            else
                            {
                                DXMessageBox.Show("You are not allowed to Un-Mark the Leave void", "Unauthorize", MessageBoxButton.OK, MessageBoxImage.Stop);
                            }
                            return;
                        }
                    }

                }



            }

            catch (Exception ex) { MessageBox.Show(ex.Message); }



        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;

        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;

        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            //To be Changed.. 
            var empIdInt = Convert.ToInt32(leaveIdStr.Text);
            try
            {


                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);
                        var result = attachment.startDownload(str, TransactionItemType.Employee);
                        if (!string.IsNullOrEmpty(result.Item2))
                        {
                            System.Diagnostics.Process.Start(result.Item2);
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
            //To be change
            if (!String.IsNullOrEmpty(leaveIdStr.Text))
            {
                empIdInt = Convert.ToInt32(leaveIdStr.Text);
                string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
                string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
                if (cmbCategory.SelectedItem != null)
                {
                    if (leaveIdStr.Text != "")
                    {
                        try
                        {
                            int CategoryId = (cmbCategory.SelectedItem as ZAS_ERP.cmbitem).id;
                            OpenFileDialog fileDialog = new OpenFileDialog();
                            fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                            fileDialog.Multiselect = false;
                            string sourceFile = @"";
                            string exePath = System.Environment.GetCommandLineArgs()[0];
                            string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                            destination += "Attachments\\Employee\\ToUpload\\";
                            //string destination = @"D:\MovedFiles\new\";
                            System.IO.Directory.CreateDirectory(destination);
                            if (fileDialog.ShowDialog() == true) // Test result.
                            {
                                imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                                btnAttachNew.ToolTip = "Uploading";
                                btnAttachNew.IsEnabled = true;

                                btnAttachment.Content = "Uploading File . . .";
                                sourceFile = fileDialog.FileName;
                                destination += Convert.ToInt32(leaveIdStr.Text) + "_" + TransactionItemType.Employee.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);
                                if (sourceFile.Length < 74)
                                {
                                    System.IO.File.Move(sourceFile, destination);

                                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                                    {
                                        ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                        var result = attachment.startUploading(TransactionItemType.Employee);
                                        if (result.Item1)
                                        {
                                            AttachmentsRepo repo = new AttachmentsRepo();
                                            //Attachment attachmen= new Attachment();
                                            repo.Add(System.IO.Path.GetFileName(result.Item2), empIdInt, TransactionItemType.Employee, "None", MainWindow.currentUserid, "NA", destination, result.Item2, CategoryId);
                                            UsersRepo.Add(TransactionInfo.Attachment_Uploaded, empIdInt, 14, "Added a New attachment");

                                            this.Dispatcher.Invoke(() =>
                                            {
                                                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(empIdInt, TransactionItemType.Employee);
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


                                //MessageBox.Show("Attachment Uploaded");


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
        }
        public void loadOnLeaveData()
        {
            if (!String.IsNullOrEmpty(leaveIdStr.Text))
            {
                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(Convert.ToInt32(leaveIdStr.Text), TransactionItemType.Leave);
                cmbCategory.ItemsSource = SYSTEM_STATIC.GetActiveAttachmentCategories();

            }
         
        }
        public void GenerateUsersForComments()
        {
            try {
                frmDepartmentSelect frm = new frmDepartmentSelect();
                frm.ShowDialog();
                if (frm.department != null)
                {
                    var dept = frm.department;
                    var users = UsersRepo.getusersByDepartment(dept.Id);
                    if (users != null)
                    {
                        UsersForComments = users;

                        if (isAdmin != true && isNotif !=true)
                        {
                            UsersForComments.Add(SYSTEM_STATIC.currentUser);
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(leaveIdStr.Text))
                            {
                                var leaveId = Convert.ToInt32(leaveIdStr.Text);

                                var leave = hrRepo.GetLeaveApplication(leaveId);
                                if (leave.employee != null)
                                {
                                    var empId = (int)leave.employeeId;
                                   var user =  UsersRepo.getuserListByEmpId(empId);
                                    if (user.Count != 0)
                                    {
                                        foreach (var _user in user)
                                        {
                                            UsersForComments.Add(_user);

                                        }
                                    }
                                }
                                //var user = empRepo.GetUserFromEmployee(Convert.ToInt32(EmployeeIdStr.Text));
                                //if (user != null)
                                //{

                                //}
                            }
                        }
                    }
                }
                //frm.Dispose();
            }
            catch { }
            
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

        public void loadBasicInfo(User user)
        {
            if (user != null)
            {
                //Load Total allowed
                if (user.employee.Desig != null)
                {
                    allowedAnnual = user.employee.Desig.AnnualLeaveDays;
                    allowedCasual = user.employee.Desig.CasualLeaveDays;
                    totalLeaves = allowedAnnual + allowedCasual;

                    totalSick.Text = allowedCasual.ToString();
                    totalAnnual.Text = allowedAnnual.ToString();
                    total.Text = totalLeaves.ToString();
                }

                //Load Annual Balance
                try
                {
                    var annualLeaves = hrRepo.GetEmployeeAnnualLeaves(user.employeeId).Where(x=>x.isApproved != false).ToList();

                    if (annualLeaves.Count != 0)
                    {
                        annualBalance = allowedAnnual- annualLeaves.Sum(x => Math.Abs( x.LeaveDays));
                    }
                    else
                    {
                        //annualBalance = 0;
                        annualBalance = allowedAnnual;
                    }

                }
                catch
                {
                    annualBalance = 0;
                }

                var casualLeaves = hrRepo.GetEmployeeCasualLeaves(user.employeeId).Where(x => x.isApproved != false).ToList();
                if (casualLeaves.Count != 0)
                {
                    casualBalance = allowedCasual - casualLeaves.Sum(x => Math.Abs(x.LeaveDays));
                }
                else
                {
                    //casualBalance = 0;
                    casualBalance = allowedCasual;
                }

                var adjustedLeaves = hrRepo.GetEmployeeAdjustedLeaves(user.employeeId).Where(x => x.isApproved != false ).ToList();
                if (adjustedLeaves.Count != 0)
                {
                    adjustmentBalance = adjustedLeaves.Sum(x => Math.Abs(x.LeaveDays));
                }
                else
                {
                    adjustmentBalance = 0;
                }
                totalBalance = annualBalance + casualBalance + adjustmentBalance;

                openingSick.Text = casualBalance.ToString();
                openingAnnual.Text = annualBalance.ToString();
                openingAdj.Text = adjustmentBalance.ToString();
                openingTotal.Text = totalBalance.ToString();


                //Load Name and desination
                if (user.employee.person != null)
                {
                    var fName = user.employee.person.FName;
                    var lName = user.employee.person.LName;
                    if (user.employee.Desig != null)
                    {
                        var desig = user.employee.Desig.Title;
                        txtPosition.Text = desig;

                    }

                    txtFname.Text = fName;
                    txtLname.Text = lName;
                }
                dateApplied.DateTime = DateTime.Now;

            }
        }

        double allowedAnnual, allowedCasual, totalLeaves;

        public void loadLeaveInfo(ERP_BL.Databases.Employee emp, DateTime date, int Id)
        {
            try
            {
                if (emp != null)
                {
                    //Load total allowed
                    if (emp.Desig != null)
                    {
                        allowedAnnual = emp.Desig.AnnualLeaveDays;
                        allowedCasual = emp.Desig.CasualLeaveDays;
                        totalLeaves = allowedAnnual + allowedCasual;

                        totalSick.Text = allowedCasual.ToString();
                        totalAnnual.Text = allowedAnnual.ToString();
                        total.Text = totalLeaves.ToString();
                    }

                    //Load Balance
                    var annualLeaves = hrRepo.GetEmployeeAnnualLeaves(emp.EmpId, date, Id);
                     //annualLeaves.RemoveAt(annualLeaves.Count - 1);
                    if (annualLeaves.Count != 0)
                    {
                        annualBalance = allowedAnnual - annualLeaves.Sum(x => Math.Abs( x.LeaveDays));
                    }
                    else
                    {
                        //annualBalance = 0;
                        annualBalance = allowedAnnual;
                    }
                    
                    var casualLeaves = hrRepo.GetEmployeeCasualLeaves(emp.EmpId, date, Id);
                    if (casualLeaves.Count != 0)
                    {
                        casualBalance = allowedCasual - casualLeaves.Sum(x => Math.Abs( x.LeaveDays));
                    }
                    else
                    {
                        //casualBalance = 0;
                        casualBalance = allowedCasual;
                    }

                    var adjustedLeaves = hrRepo.GetEmployeeAdjustedLeaves(emp.EmpId, date, Id);
                    if (adjustedLeaves.Count != 0)
                    {
                        adjustmentBalance = adjustedLeaves.Sum(x => Math.Abs( x.LeaveDays));
                    }
                    else
                    {
                        adjustmentBalance = 0;
                    }
                    totalBalance = annualBalance + casualBalance + adjustmentBalance;

                    openingSick.Text = casualBalance.ToString();
                    openingAnnual.Text = annualBalance.ToString();
                    openingAdj.Text = adjustmentBalance.ToString();
                    openingTotal.Text = totalBalance.ToString();

                }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadFormInfo()
        {
            try
            {             
                User user=null;
                
                if (isEdit == false)
                {
                    if (isAdmin == false)
                    {
                        user = SYSTEM_STATIC.currentUser;
                        loadBasicInfo(user);
                    }
                }

                else if (isEdit == true)
                {
                    //Setting Visibility of Status
                    stckStatus.Visibility = Visibility.Visible;

                    if (leaveId != 0)
                    {
                        var leaveApp = hrRepo.GetLeaveApplication(leaveId);


                        if (leaveApp.isVoid == true)
                        {
                            grdVoid.Visibility = Visibility.Visible;
                            txtVoid.RenderTransform = new RotateTransform(-45);
                            //lblStage.Text = "Void";
                        }

                        if (leaveApp != null)
                        {
                            //loading employe leaves balance
                            if (leaveApp.employee != null)
                            {
                                loadLeaveInfo(leaveApp.employee, leaveApp.ApplyDate, leaveApp.leave.Id);

                            }

                            if (leaveApp.leave.LeaveType == ERP_BL.Enums.LeaveType.AnnualLeave)
                            {
                                radAnnual.IsChecked = true;
                                radCasual.IsEnabled = false;
                                radHalf.IsEnabled = false;
                                radAdjustment.IsEnabled = false;
                            }
                            else if (leaveApp.leave.LeaveType == ERP_BL.Enums.LeaveType.CasualLeave)
                            {
                                if (leaveApp.isHalf == false)
                                {
                                    radCasual.IsChecked = true;
                                    radHalf.IsEnabled = false;
                                    radAdjustment.IsEnabled = false;
                                    radAnnual.IsEnabled = false;
                                }
                                else if (leaveApp.isHalf == true)
                                {
                                    radHalf.IsChecked = true;
                                    radCasual.IsEnabled = false;
                                    radAdjustment.IsEnabled = false;
                                    radAnnual.IsEnabled = false;
                                    grdLeaveDate.Visibility = Visibility.Visible;
                                }
                            }
                            else if (leaveApp.leave.LeaveType == ERP_BL.Enums.LeaveType.Adjustment)
                            {
                                if (leaveApp.leave.LeaveDays < 1)
                                {
                                    radAdjustment.IsChecked = true;
                                    radCasual.IsEnabled = false;
                                    radHalf.IsEnabled = false;
                                    radAnnual.IsEnabled = false;
                                    chkIsHalf.IsChecked = true;
                                }
                                else
                                {
                                    radAdjustment.IsChecked = true;
                                    radCasual.IsEnabled = false;
                                    radHalf.IsEnabled = false;
                                    radAnnual.IsEnabled = false;
                                }
                                
                            }
                            else
                            {
                                DXMessageBox.Show("Unable to edit because it doesnot have any valid leave Type", "Un-Successfull", MessageBoxButton.OK, MessageBoxImage.Stop);
                                return;
                            }
                            isEdit = true;
                            leaveIdStr.Text = leaveApp.Id.ToString();
                            txtFname.Text = leaveApp.employee.person.FName;
                            txtLname.Text = leaveApp.employee.person.LName;
                            if (leaveApp.employee.Desig != null)
                                txtPosition.Text = leaveApp.employee.Desig.Title;

                            dateFrom.EditValue = leaveApp.StartDate;
                            dateTo.EditValue = leaveApp.EndDate;
                            dateApplied.EditValue = leaveApp.ApplyDate;
                            txtReason.Text = leaveApp.LeaveDes;
                            if (leaveApp.leave.LeaveDate != null)
                            {
                                dteLeaveDate.EditValue = (DateTime)leaveApp.leave.LeaveDate;
                            }

                            if (leaveApp.leave == null)
                            {
                                DXMessageBox.Show("Error in loading void leave", "Un-Successfull", MessageBoxButton.OK, MessageBoxImage.Stop);
                                return;
                            }
                            if (leaveApp.isVoid == true)
                            {
                                TimeSpan span = dateTo.DateTime.Subtract(dateFrom.DateTime);
                                var days1 = (int)span.Days;
                                if (days1 < 0)
                                {
                                    DXMessageBox.Show("Error in loading void leave", "Un-Successfull", MessageBoxButton.OK, MessageBoxImage.Stop);
                                    return;
                                }
                                else
                                {
                                    days1 = days1 + 1;
                                    txtTotalDays.Text = days1.ToString();

                                }
                            }
                            else
                            {
                                double days;
                                if (leaveApp.leave.LeaveDays > 0)
                                {
                                    days = leaveApp.leave.LeaveDays;
                                    originalLeaveDays = days;

                                }
                                else
                                {
                                    originalLeaveDays = leaveApp.leave.LeaveDays;
                                    days = 0 - leaveApp.leave.LeaveDays;


                                }
                                txtTotalDays.Text = days.ToString();
                            }

                            if (leaveApp.isHalf == true)
                            {
                                TimeFrom.EditValue = leaveApp.StartDate;
                                TimeTo.EditValue = leaveApp.EndDate;
                                double days = 0 - leaveApp.leave.LeaveDays;
                                //txtTotalTime.Text = days.ToString();

                            }

                            isAppr = (bool)leaveApp.isApproved;
                            leaveType = leaveApp.leave.LeaveType;
                            isHalf = (bool)leaveApp.isHalf;

                            if (leaveApp.leave.LeaveDate != null)
                            {
                                dteLeaveDate.DateTime = (DateTime)leaveApp.leave.LeaveDate;
                            }
                            if (leaveApp.stage != null)
                            {
                                if (leaveApp.isVoid == true)
                                {
                                    lblStage.Text = "Void";
                                }
                                else
                                {
                                    lblStage.Text = leaveApp.stage.ToString();
                                }

                                    
                            }
                        }

                        if (isAdmin == true)
                        {
                            //Load Leave Status
                            if (leaveApp.leaveStatus != null)
                            {
                                var status = leaveApp.leaveStatus;

                                for (int i = 0; i < cmbxLeaveStatus.Items.Count; i++)
                                {
                                    var _item = cmbxLeaveStatus.Items[i] as ZAS_ERP.cmbitem;
                                    //var item = _item as cmbitem;
                                    if (_item.name == leaveApp.leaveStatus.Status && _item.id == leaveApp.leaveStatus.Id)
                                    {
                                        index = i;
                                        break;
                                    }
                                }
                                cmbxLeaveStatus.SelectedIndex = index;
                            }

                            cmbEmployee.IsEnabled = false;
                        }
                        
                    }
                }


                //        if (isNotif != true)
                //    {
                //        user = SystemLogic.currentUser;
                //    }
                //    if (isNotif == true)
                //    {
                //        if (leaveIdNotif != 0)
                //        {
                //            var leave = hrRepo.GetLeaveApplication(leaveIdNotif);
                //            if (leave != null)
                //            {
                //                var emp = leave.employee;
                //                user = _usersRepo.getuserByEmpId(emp.EmpId);
                //            }
                //        }
                //    }
                   
                //}
                //else
                //{
                //    if (isEdit == true)
                //    {
                //        if (selectedEmployee != null)
                //        {
                //            //Load Total allowed
                //            if (selectedEmployee.Desig != null)
                //            {
                //                var allowedAnnual = selectedEmployee.Desig.AnnualLeaveDays;
                //                var allowedCasual = selectedEmployee.Desig.CasualLeaveDays;
                //                var totalLeaves = allowedAnnual + allowedCasual;

                //                totalSick.Text = allowedCasual.ToString();
                //                totalAnnual.Text = allowedAnnual.ToString();
                //                total.Text = totalLeaves.ToString();
                //            }

                //            //Load Annual Balance
                //            var annualLeaves = hrRepo.GetEmployeeAnnualLeaves(selectedEmployee.EmpId);

                //            if (annualLeaves.Count != 0)
                //            {
                //                annualBalance = annualLeaves.Sum(x => x.LeaveDays);
                //            }
                //            else
                //            { annualBalance = 0; }

                //            var casualLeaves = hrRepo.GetEmployeeCasualLeaves(selectedEmployee.EmpId);
                //            if (casualLeaves.Count != 0)
                //            {
                //                casualBalance = casualLeaves.Sum(x => x.LeaveDays);
                //            }
                //            else
                //            {
                //                casualBalance = 0;
                //            }
                //            totalBalance = annualBalance + casualBalance;

                //            openingSick.Text = casualBalance.ToString();
                //            openingAnnual.Text = annualBalance.ToString();
                //            openingTotal.Text = totalBalance.ToString();


                //            //Load Name and desination
                //            if (selectedEmployee.person != null)
                //            {
                //                var fName = selectedEmployee.person.FName;
                //                var lName = selectedEmployee.person.LName;
                //                if (selectedEmployee.Desig != null)
                //                {
                //                    var desig = selectedEmployee.Desig.Title;
                //                    txtPosition.Text = desig;

                //                }

                //                txtFname.Text = fName;
                //                txtLname.Text = lName;
                //            }

                //            //Load Leave Status
                //            if (app.leaveStatus != null)
                //            {
                //                var status = app.leaveStatus;

                //                for (int i = 0; i < cmbxLeaveStatus.Items.Count; i++)
                //                {
                //                    var _item = cmbxLeaveStatus.Items[i] as ZAS_ERP.cmbitem;
                //                    //var item = _item as cmbitem;
                //                    if (_item.name == app.leaveStatus.Status && _item.id == app.leaveStatus.Id)
                //                    {
                //                        index = i;
                //                        break;
                //                    }
                //                }
                //                cmbxLeaveStatus.SelectedIndex = index;
                //            }

                //            if (app != null)
                //            {
                //                if (app.leave != null)
                //                {
                //                    if (app.leave.LeaveType == ERP_BL.Enums.LeaveType.AnnualLeave)
                //                    {
                //                        radAnnual.IsChecked = true;
                //                    }
                //                    else if (app.leave.LeaveType == ERP_BL.Enums.LeaveType.CasualLeave)
                //                    {
                //                        if (app.isHalf == false)
                //                        { radCasual.IsChecked = true; }
                //                        else if (app.isHalf == true)
                //                        {
                //                            radHalf.IsChecked = true;
                //                        }
                //                    }
                //                    else if (app.leave.LeaveType == ERP_BL.Enums.LeaveType.HalfDay)
                //                    {
                //                        radHalf.IsChecked = true;
                //                    }
                //                    else
                //                    {
                //                        DXMessageBox.Show("Unable to edit because it doesnot have any valid leave Type", "Un-Successfull", MessageBoxButton.OK, MessageBoxImage.Stop);
                //                        return;
                //                    }
                //                    isEdit = true;
                //                    leaveIdStr.Text = app.Id.ToString();

                //                    dateFrom.EditValue = app.StartDate;
                //                    dateTo.EditValue = app.EndDate;
                //                    dateApplied.EditValue = app.ApplyDate;
                //                    txtReason.Text = app.LeaveDes;

                //                    if (app.isVoid == true)
                //                    {
                //                        TimeSpan span = dateTo.DateTime.Subtract(dateFrom.DateTime);
                //                        var days1 = (int)span.Days;
                //                        if (days1 < 0)
                //                        {
                //                            DXMessageBox.Show("Error in loading void leave", "Un-Successfull", MessageBoxButton.OK, MessageBoxImage.Stop);
                //                            return;
                //                        }
                //                        else
                //                        {
                //                            days1 = days1 + 1;
                //                            txtTotalDays.Text = days1.ToString();

                //                        }
                //                    }
                //                    else
                //                    {
                //                        double days;
                //                        if (app.leave.LeaveDays > 0)
                //                        {
                //                             days = app.leave.LeaveDays;
                //                        }
                //                        else
                //                        {
                //                             days = 0 - app.leave.LeaveDays;

                //                        }
                //                        txtTotalDays.Text = days.ToString();
                //                    }

                //                    if (app.isHalf == true)
                //                    {
                //                        TimeFrom.EditValue = app.StartDate;
                //                        TimeTo.EditValue = app.EndDate;
                //                        double days = 0 - app.leave.LeaveDays;
                //                        txtTotalTime.Text = days.ToString();

                //                    }
                //                }
                                
                //            }
                           


                //        }
                //    }
                  
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            if (isAdmin == true)
            {
                populateAdminFields();
            }
            if (isNotif == true)
            {
                btnApproveLeave.Visibility = Visibility.Visible;
            }
            loadFormInfo();
               
           loadOnLeaveData();

            //if (isNotif== true)
            //{
            //    if (leaveIdNotif != 0)
            //    {
            //        var leave = hrRepo.GetLeaveApplication(leaveIdNotif);
            //        if (leave != null)
            //        {
            //                if (leave.leave.LeaveType == ERP_BL.Enums.LeaveType.AnnualLeave)
            //                {
            //                    radAnnual.IsChecked = true;
            //                }
            //                else if (leave.leave.LeaveType == ERP_BL.Enums.LeaveType.CasualLeave)
            //                {
            //                    if (leave.isHalf == false)
            //                    { radCasual.IsChecked = true; }
            //                    else if (leave.isHalf == true)
            //                    {
            //                        radHalf.IsChecked = true;
            //                    }
            //                }
            //                else if (leave.leave.LeaveType == ERP_BL.Enums.LeaveType.HalfDay)
            //                {
            //                    radHalf.IsChecked = true;
            //                }
            //                else
            //                {
            //                    DXMessageBox.Show("Unable to edit because it doesnot have any valid leave Type", "Un-Successfull", MessageBoxButton.OK, MessageBoxImage.Stop);
            //                    return;
            //                }
            //                isEdit = true;
            //            leaveIdStr.Text = leave.Id.ToString();
            //            txtFname.Text = leave.employee.person.FName;
            //            txtLname.Text = leave.employee.person.LName;
            //            txtPosition.Text = leave.employee.Desig.Title;

            //            dateFrom.EditValue = leave.StartDate;
            //            dateTo.EditValue = leave.EndDate;
            //            dateApplied.EditValue = leave.ApplyDate;
            //            txtReason.Text = leave.LeaveDes;
                       
            //            if (leave.leave == null)
            //            {
            //                DXMessageBox.Show("Error in loading void leave", "Un-Successfull", MessageBoxButton.OK, MessageBoxImage.Stop);
            //                return;
            //            }
            //            if (leave.isVoid == true)
            //            {
            //                TimeSpan span = dateTo.DateTime.Subtract(dateFrom.DateTime);
            //                var days1 = (int)span.Days;
            //                if (days1 < 0)
            //                {
            //                    DXMessageBox.Show("Error in loading void leave", "Un-Successfull", MessageBoxButton.OK, MessageBoxImage.Stop);
            //                    return;
            //                }
            //                else
            //                {
            //                    days1 = days1 + 1;
            //                    txtTotalDays.Text = days1.ToString();

            //                }
            //            }
            //            else
            //            {
            //                double days = 0 - leave.leave.LeaveDays;
            //                txtTotalDays.Text = days.ToString();
            //            }

            //            if (leave.isHalf == true)
            //            {
            //                TimeFrom.EditValue = leave.StartDate;
            //                TimeTo.EditValue = leave.EndDate;
            //                double days = 0 - leave.leave.LeaveDays;
            //                txtTotalTime.Text = days.ToString();

            //            }

            //        }
            //    }
            
            //if (isEdit == true)
            //{
            //        if (isAdmin == true)
            //        {
            //            if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Edit Leave application (Admin)") != null)
            //            {
            //                btnApplyLeaves.IsEnabled = true;
            //            }
            //            else
            //            {
            //                btnApplyLeaves.IsEnabled = false;
            //            }
            //        }
            //        else if (isAdmin == false)
            //        {                        
            //           if (SystemLogic.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Leave") != null && isAppr == false)
            //            {
            //                btnApplyLeaves.IsEnabled = true;
            //            }
            //            else
            //            {
            //                btnApplyLeaves.IsEnabled = false;
            //            }
            //        }

            //        if (leaveType == LeaveType.CasualLeave)
            //        {
            //            if (isHalf == true)
            //            {
            //                radAnnual.IsEnabled = false;
            //                radCasual.IsEnabled = false;
            //                radAdjustment.IsEnabled = false;

            //            }
            //            else
            //            {
            //                radAnnual.IsEnabled = false;
            //                radHalf.IsEnabled = false;
            //                radAdjustment.IsEnabled = false;


            //            }
            //        }
            //        else if (leaveType == LeaveType.AnnualLeave)
            //        {
            //            if (originalLeaveDays != 0)
            //            {
            //                if (originalLeaveDays > 0)
            //                {
            //                    radCasual.IsEnabled = false;
            //                    radHalf.IsEnabled = false;
            //                    radAnnual.IsEnabled = false;
            //                    radAdjustment.IsEnabled = true;

            //                }
            //                else
            //                {
            //                    radCasual.IsEnabled = false;
            //                    radHalf.IsEnabled = false;
            //                    radAdjustment.IsEnabled = false;
            //                }
            //            }
                       
            //        }


            //        //lblCloseBal.Visibility = Visibility.Collapsed;
            //        //closingSick.Visibility = Visibility.Collapsed;
            //        //closingAnnual.Visibility = Visibility.Collapsed;
            //        //closingTotal.Visibility = Visibility.Collapsed;

            //        //lblOpBal.Content = "Balance after leaves";
            //        //lblOpBal.SetValue(Grid.RowProperty, 0);
            //        //lblOpBal.SetValue(Grid.ColumnProperty, 8);


            //        //openingSick.SetValue(Grid.RowProperty, 2);
            //        //openingSick.SetValue(Grid.ColumnProperty, 8);

            //        //openingAnnual.SetValue(Grid.RowProperty, 4);
            //        //openingAnnual.SetValue(Grid.ColumnProperty, 8);

            //        //openingTotal.SetValue(Grid.RowProperty, 6);
            //        //openingTotal.SetValue(Grid.ColumnProperty, 8);
            //    }

            //if (isAdmin == true)
            //{
            //    grdAdminControls.Visibility = Visibility.Visible;
            //}

            
            //}

            //Permissions
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add leave Adjustment(Admin)") != null)
            {
                radAdjustment.Visibility = Visibility.Visible;
            }
            else
            {
                radAdjustment.Visibility = Visibility.Collapsed;
            }

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Leave Adjustment") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New own Leave Adjustment") != null)
            {
                radAdjustment.Visibility = Visibility.Visible;
            }
            else
            {
                radAdjustment.Visibility = Visibility.Collapsed;
            }
            if (isAdmin == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Leave application (Admin)") != null)
                {
                    btnApplyLeaves.IsEnabled = true;
                }
                else
                {
                    btnApplyLeaves.IsEnabled = false;
                }
            }
            else if (isAdmin == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Leave") != null && isAppr == false)
                {
                    btnApplyLeaves.IsEnabled = true;
                }
                else
                {
                    btnApplyLeaves.IsEnabled = false;
                }
            }

        }

        public void populateAdminFields()
        {
            grdAdminControls.Visibility = Visibility.Visible;
            //Adding status list to combobox
            List<ZAS_ERP.cmbitem> leaveStatusLst = new List<ZAS_ERP.cmbitem>();
            var allLeaveStatus = hrRepo.GetAllLeaveStatus();
            if (allLeaveStatus != null)
            {
                Parallel.ForEach(allLeaveStatus, delegate (LeaveStatus status) // foreach (Employee status in EmployeeStatuses)
                {

                    leaveStatusLst.Add
                    (new ZAS_ERP.cmbitem()
                    {
                        name = status.Status,
                        id = status.Id,
                        bcolor = status.backcolor,
                        fcolor = "#FF000000"
                    });


                });
                cmbxLeaveStatus.ItemsSource = leaveStatusLst;
            }



            //Load employees in lookupedit
            EmployeeRepo repo = new EmployeeRepo();
            
            var empList = repo.GetAllEmployees();
            if (empList != null)
            {
                cmbEmployee.ItemsSource = empList;
            }


            btnApproveLeave.Visibility = Visibility.Visible;

        }

        private void BtnApplyLeaves_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int empId =0;
                if (isEdit == false)
                {
                    if (radAnnual.IsChecked == false && radCasual.IsChecked == false && radHalf.IsChecked == false && radAdjustment.IsChecked == false)
                    {
                        DXMessageBox.Show("Please Select Leave Type first","Please Select",MessageBoxButton.OK,MessageBoxImage.Information);
                        return;
                    }

                    if (dateFrom.DateTime == null)
                    {
                        DXMessageBox.Show("Please Select Start Date first", "Please Select", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    if (dateTo.DateTime == null)
                    {
                        DXMessageBox.Show("Please Select End Date first", "Please Select", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    // return;
                    LeaveApplication leave = new LeaveApplication();
                    Leave leaveObj = new Leave();

                    //leaveIdStr.Text = item.Id.ToString();
                    //if()
                    if (isAdmin == true)
                    {
                        if (cmbEmployee.SelectedItem != null)
                        {
                            var emp = cmbEmployee.SelectedItem as ERP_BL.Databases.Employee;
                            leave.employee = emp;
                            empId = emp.EmpId;
                        }
                        else
                        {
                            DXMessageBox.Show("Please select Employee first to add leave","Please Select",MessageBoxButton.OK,MessageBoxImage.Stop);
                            return;
                        }
                    }
                    else
                    {
                        var user = SYSTEM_STATIC.currentUser;
                        if (user != null)
                        {
                            var emp = user.employee;

                            leave.employee = emp;
                            empId = emp.EmpId;
                            //leave.initiator = emp;
                            //leaveObj.employee = emp;
                        }

                    }
                   

                    //leave.employee.person.FName = txtFname.Text;
                    //leave.employee.person.LName = txtLname.Text;
                    //leave.employee.Desig.Title = txtPosition.Text;




                    //txtTotalDays.Text = leave.leave.LeaveDays.ToString();


                    //var span = frm.dateTo.DateTime.Subtract(frm.dateFrom.DateTime);
                    //var days = span.Days;
                    //days = days + 1;
                    //frm.txtTotalDays.Text = days.ToString();

                    if (radAnnual.IsChecked == true)
                    {
                        leaveObj.LeaveType = LeaveType.AnnualLeave;
                    }
                    if (radCasual.IsChecked == true)
                    {
                        leaveObj.LeaveType = LeaveType.CasualLeave;
                    }
                    if (radHalf.IsChecked == true)
                    {
                        leaveObj.LeaveType = LeaveType.CasualLeave;
                    }
                    if (radAdjustment.IsChecked == true)
                    {
                        leaveObj.LeaveType = LeaveType.Adjustment;
                    }
                    


                    if (dateApplied != null)
                    {
                        leave.ApplyDate = dateApplied.DateTime;
                        leaveObj.ApplyDate = dateApplied.DateTime;
                    }
                    if (radHalf.IsChecked == true)
                    {
                        if (TimeFrom != null)
                        {
                            leave.StartDate = TimeFrom.DateTime;
                            leaveObj.DateFrom = TimeFrom.DateTime;
                        }
                        if (TimeTo != null)
                        {
                            leave.EndDate = TimeTo.DateTime;
                            leaveObj.DateTo = TimeTo.DateTime;
                        }

                        double day = 0.5;
                            var leaveDays = day;
                            leaveDays = 0 - leaveDays;
                            leaveObj.LeaveDays = leaveDays;
                        leave.isHalf = true;
                    }
                   else if (radAdjustment.IsChecked == true)
                    {
                        if (chkIsHalf.IsChecked != true)
                        {
                            if (dateFrom != null)
                            {
                                leave.StartDate = dateFrom.DateTime;
                                leaveObj.DateFrom = dateFrom.DateTime;
                            }
                            if (dateTo != null)
                            {
                                leave.EndDate = dateTo.DateTime;
                                leaveObj.DateTo = dateTo.DateTime;
                            }
                            if (!String.IsNullOrEmpty(txtTotalDays.Text))
                            {
                                var leaveDays = Convert.ToDouble(txtTotalDays.Text);
                                leaveDays = 0 + leaveDays;
                                leaveObj.LeaveDays = leaveDays;
                            }
                            
                        }
                        else
                        {
                            if (dateFrom != null)
                            {
                                leave.StartDate = dateFrom.DateTime;
                                leaveObj.DateFrom = dateFrom.DateTime;
                            }
                            if (dateTo != null)
                            {
                                leave.EndDate = dateTo.DateTime;
                                leaveObj.DateTo = dateTo.DateTime;
                            }
                            double day = 0.5;
                            var leaveDays = day;
                            leaveDays = 0 + leaveDays;
                            leaveObj.LeaveDays = leaveDays;
                            //leave.isHalf = true;

                        }
                        
                    }
                    else
                    {
                        if (dateFrom != null)
                        {
                            leave.StartDate = dateFrom.DateTime;
                            leaveObj.DateFrom = dateFrom.DateTime;
                        }
                        if (dateTo != null)
                        {
                            leave.EndDate = dateTo.DateTime;
                            leaveObj.DateTo = dateTo.DateTime;
                        }

                        if (!String.IsNullOrEmpty(txtTotalDays.Text))
                        {
                            var leaveDays = Convert.ToDouble(txtTotalDays.Text);
                            leaveDays = 0 - leaveDays;
                            leaveObj.LeaveDays = leaveDays;
                        }
                       
                    }
                   
                    leaveObj.employeeId = leave.employee.EmpId;
                    leaveObj.LeaveDes = txtReason.Text;
                    leave.leave = leaveObj;

                    leave.LeaveDes = txtReason.Text;

                    if (dteLeaveDate.DateTime != null)
                    {
                        leave.leave.LeaveDate = dteLeaveDate.DateTime;
                    }

                    if (empId != 0)
                    {
                        var empLeaves = hrRepo.GetPendingForApprovalLeave(empId);
                        //if (empLeaves.Count != 0)
                        //{
                        //    DXMessageBox.Show("You are not allowed to apply for leaves because your one or more leaves are already in pending for approval","Not Allowed",MessageBoxButton.OK,MessageBoxImage.Warning);
                        //    return;
                        //}
                    }
                    //if (Convert.ToDouble(total.Text) == 0)
                    if (String.IsNullOrEmpty(total.Text))

                        {
                            DXMessageBox.Show("You are not allowed to apply leaves because your allowed leaves are 0", "Not Allowed", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Leave") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add leave application (Admin)") != null)
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Leave without Approval") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Leave") != null)
                        {
                            leave.isApproved = true;
                            leaveObj.isApproved = true;
                            leave.ApprovedDate = DateTime.Now;
                            leave.stage = TransactionStage.Approved;
                        }
                        else
                        {
                            leave.isApproved = false;
                            leaveObj.isApproved = false;
                            leave.stage = TransactionStage.AwaitingApproval;

                        }

                        //if (cmbxLeaveStatus.SelectedItem != null)
                        //{
                        //    var item = cmbxLeaveStatus.SelectedItem as LeaveStatus;
                        //    leave.leaveStatus = item;
                        //}
                        if ((cmbxLeaveStatus.SelectedItem as ZAS_ERP.cmbitem) != null)
                        {

                            var status = hrRepo.GetLeaveStatus((cmbxLeaveStatus.SelectedItem as ZAS_ERP.cmbitem).id);
                            if (status != null)
                            {
                                leave.leaveStatus = status;
                            }
                        }

                        hrRepo.AddLeaveApplication(leave/*, leaveObj*/);
                        DXMessageBox.Show("Leave application added Successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);

                        var win = Window.GetWindow(frmLeaveApp);
                        win.Close();
                    }
                    else
                    {
                        DXMessageBox.Show("You are not allowed to apply for leave","",MessageBoxButton.OK,MessageBoxImage.Stop);
                    }


                  
                }
                if (isEdit == true)
                {
                    if (!String.IsNullOrEmpty(leaveIdStr.Text))
                    {
                        var leaveId = Convert.ToInt32(leaveIdStr.Text);
                        var leaveApp = hrRepo.GetLeaveApplication(leaveId);
                     

                        if (leaveApp.leave != null)
                        {
                            var leave = hrRepo.GetEmployeeLeave(leaveApp.leave.Id);
                            
                            if (radAnnual.IsChecked == true)
                            {
                                leave.LeaveType = LeaveType.AnnualLeave;
                            }
                            if (radCasual.IsChecked == true)
                            {
                                leave.LeaveType = LeaveType.CasualLeave;
                            }
                            if (radHalf.IsChecked == true)
                            {
                                leave.LeaveType = LeaveType.CasualLeave;
                                leaveApp.isHalf = true;

                                if (TimeFrom != null)
                                {
                                    leaveApp.StartDate = TimeFrom.DateTime;
                                    leave.DateFrom = TimeFrom.DateTime;
                                }
                                if (TimeTo != null)
                                {
                                    leaveApp.EndDate = TimeTo.DateTime;
                                    leave.DateTo = TimeTo.DateTime;
                                }
                            }

                            else
                            {
                                if (dateFrom != null)
                                {
                                    leaveApp.StartDate = dateFrom.DateTime;
                                    leave.DateFrom = dateFrom.DateTime;
                                }
                                if (dateTo != null)
                                {
                                    leaveApp.EndDate = dateTo.DateTime;
                                    leave.DateTo = dateTo.DateTime;
                                }

                            }

                            
                            if (dateApplied != null)
                            {
                                leaveApp.ApplyDate = dateApplied.DateTime;
                                leave.ApplyDate = dateApplied.DateTime;
                            }

                            if (!String.IsNullOrEmpty(txtTotalDays.Text))
                            {
                                var leaveDays = Convert.ToDouble(txtTotalDays.Text);
                                //leaveDays = leaveDays;
                                leave.LeaveDays = leaveDays;
                            }

                            leaveApp.LeaveDes = txtReason.Text;
                            leave.LeaveDes = txtReason.Text;
                            leaveApp.leave = leave;
                            if (dteLeaveDate.DateTime != null)
                            {
                                leaveApp.leave.LeaveDate = dteLeaveDate.DateTime;
                            }
                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Leave") != null 
                            || 
                            SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Leave application (Admin)") != null
                            ||
                            SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Leave") != null
                            )
                        {
                            if (leaveApp.isVoid == true || leaveApp.PendingForClosing == true || leaveApp.isApproved == false)
                            { }
                            else
                            {
                                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Leave without ReApproval") != null
                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for added Leave") != null)
                                {
                                    leaveApp.isReApproved = true;
                                    leaveApp.stage = TransactionStage.Approved;
                                    //if (leaveApp.PendingForClosing == true)
                                    //{
                                    //    leaveApp.PendingForClosing = true;
                                    //}
                                }
                                else
                                {
                                    // if (emp.employeeApproval.PendingForClosing == true || emp.employeeApproval.isReApproved == false || emp.employeeApproval.isApproved == false)
                                    //{
                                    //     DXMessageBox.Show("You don't have permission to edit employee in current state", "Un-Authorized", MessageBoxButton.OK, MessageBoxImage.Stop);
                                    //     return;
                                    //}
                                    //else
                                    //{
                                    //if (emp.employeeApproval.isApproved == false || emp.employeeApproval.PendingForClosing == true)
                                    //{
                                    //    DXMessageBox.Show("You don't have permission to edit employee in current state", "Un-Authorized", MessageBoxButton.OK, MessageBoxImage.Stop);
                                    //    return;
                                    //}
                                    leaveApp.isReApproved = false;
                                    leaveApp.stage = TransactionStage.AwaitingApproval;

                                    //}

                                }

                            }
                            //if (cmbxLeaveStatus.SelectedItem != null)
                            //{
                            //    var item = cmbxLeaveStatus.SelectedItem as LeaveStatus;
                            //    leaveApp.leaveStatus = item;
                            //}
                            if ((cmbxLeaveStatus.SelectedItem as ZAS_ERP.cmbitem) != null)
                            {

                                var status = hrRepo.GetLeaveStatus((cmbxLeaveStatus.SelectedItem as ZAS_ERP.cmbitem).id);
                                if (status != null)
                                {
                                    leaveApp.leaveStatus = status;
                                }
                            }

                            hrRepo.UpdateLeaveApplication(leaveApp);
                            DXMessageBox.Show("Leave application updated Successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);
                            var win = Window.GetWindow(frmLeaveApp);
                            win.Close();
                        }
                        else
                        {
                            DXMessageBox.Show("You don't have permission to Update Leave", "Un-authorized", MessageBoxButton.OK, MessageBoxImage.Stop);

                        }

                        
                    }

                  
                }
                //TimeSpan timespan = dateTo.DateTime.Subtract(dateFrom.DateTime);
                //var days = timespan.Days;
                //days = days + 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        private void RadCasual_Checked(object sender, RoutedEventArgs e)
        {
            dateFrom.EditValue = null;
            dateTo.EditValue = null;
            //dateApplied.EditValue = null;
            txtTotalDays.EditValue = null;
            newSick.Text = null;
            newAnnual.Text = null;
            newTotal.Text = null;
            closingSick.Text = null;
            closingAnnual.Text = null;
            closingTotal.Text = null;
            newAdj.Text = null;
            closingAdj.Text = null;

            if (grdDateFrom != null || grdDateTo != null || grdTotalDays != null)
            {
                grdDateFrom.Visibility = Visibility.Visible;
                grdDateTo.Visibility = Visibility.Visible;
                grdTotalDays.Visibility = Visibility.Visible;
            }

            if (grdTimeFrom != null || grdTimeTo != null || grdTotalTime != null)
            {
                grdTimeFrom.Visibility = Visibility.Collapsed;
                grdTimeTo.Visibility = Visibility.Collapsed;
                grdTotalTime.Visibility = Visibility.Collapsed;
            }

            chkIsHalf.IsChecked = false;
            grdLeaveDate.Visibility = Visibility.Collapsed;


        }

        private void RadAnnual_Checked(object sender, RoutedEventArgs e)
        {
            dateFrom.EditValue = null;
            dateTo.EditValue = null;
            //dateApplied.EditValue = null;
            txtTotalDays.EditValue = null;
            newSick.Text = null;
            newAnnual.Text = null;
            newTotal.Text = null;
            closingSick.Text = null;
            closingAnnual.Text = null;
            closingTotal.Text = null;
            newAdj.Text = null;
            closingAdj.Text = null;

            grdDateFrom.Visibility = Visibility.Visible;
            grdDateTo.Visibility = Visibility.Visible;
            grdTotalDays.Visibility = Visibility.Visible;

            grdTimeFrom.Visibility = Visibility.Collapsed;
            grdTimeTo.Visibility = Visibility.Collapsed;
            grdTotalTime.Visibility = Visibility.Collapsed;

            chkIsHalf.IsChecked = false;
            grdLeaveDate.Visibility = Visibility.Collapsed;

        }

        private void RadHalf_Checked(object sender, RoutedEventArgs e)
        {
            dateFrom.EditValue = null;
            dateTo.EditValue = null;
            //dateApplied.EditValue = null;
            txtTotalDays.EditValue = null;
            newSick.Text = null;
            newAnnual.Text = null;
            newTotal.Text = null;
            closingSick.Text = null;
            closingAnnual.Text = null;
            closingTotal.Text = null;
            newAdj.Text = null;
            closingAdj.Text = null;

            grdDateFrom.Visibility = Visibility.Collapsed;
            grdDateTo.Visibility = Visibility.Collapsed;
            grdTotalDays.Visibility = Visibility.Collapsed;

            grdTimeFrom.Visibility = Visibility.Visible;
            grdTimeTo.Visibility = Visibility.Visible;
            grdTotalTime.Visibility = Visibility.Visible;

            chkIsHalf.IsChecked = false;

            grdLeaveDate.Visibility = Visibility.Visible;

        }

        private void TimeTo_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                TimeSpan span = TimeTo.DateTime.Subtract(TimeFrom.DateTime);
                var hours = (int)span.Hours;
                if (hours < 0)
                {
                    TimeTo.Clear();
                    TimeTo.DateTime = TimeFrom.DateTime;
                    txtTotalTime.Text = "";
                    DXMessageBox.Show("Start Time cannot be lesser than End Time", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    //hours = hours + 1;
                    txtTotalTime.Text = span.ToString();

                    newSick.Text = "0.5";
                    newAnnual.Text = "0";
                    newAdj.Text = "0";
                    newTotal.Text = "0.5";

                    double openingSickInt = 0;
                    double newSickInt = 0;
                    double openingAnnualInt = 0;
                    double newAnnualInt = 0;
                    double openingTotalInt = 0;
                    double newTotalInt = 0;

                    double openingAdjInt = 0;
                    double newAdjInt = 0;


                    if (!String.IsNullOrEmpty(openingSick.Text))
                    { openingSickInt = Convert.ToDouble(openingSick.Text); }

                    if (!String.IsNullOrEmpty(newSick.Text))
                    { newSickInt = Convert.ToDouble(newSick.Text); }

                    if (!String.IsNullOrEmpty(openingAnnual.Text))
                    { openingAnnualInt = Convert.ToDouble(openingAnnual.Text); }

                    if (!String.IsNullOrEmpty(newAnnual.Text))
                    { newAnnualInt = Convert.ToDouble(newAnnual.Text); }

                    if (!String.IsNullOrEmpty(openingTotal.Text))
                    { openingTotalInt = Convert.ToDouble(openingTotal.Text); }

                    if (!String.IsNullOrEmpty(openingTotal.Text))
                    { newTotalInt = Convert.ToDouble(newTotal.Text); }

                    if (!String.IsNullOrEmpty(openingAdj.Text))
                    { openingAdjInt = Convert.ToDouble(openingAdj.Text); }

                    if (!String.IsNullOrEmpty(newAdj.Text))
                    { newAdjInt = Convert.ToDouble(newAdj.Text); }


                    var closingSickInt = openingSickInt - newSickInt;
                    var closingAnnualInt = openingAnnualInt - newAnnualInt;
                    var closingTotalInt = openingTotalInt - newTotalInt;
                    var closingAdjInt = openingAdjInt - newAdjInt;

                    //if (isEdit == false)
                    //{
                        closingSick.Text = closingSickInt.ToString();
                        closingAnnual.Text = closingAnnualInt.ToString();
                        closingTotal.Text = closingTotalInt.ToString();
                        closingAdjInt = openingAdjInt - newAdjInt;

                    //}



                }
            }
            catch (Exception ex)
            {

            }
           
        }

  

        private void DateFrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                if (radAnnual.IsChecked != true && radCasual.IsChecked != true && radHalf.IsChecked != true && radAdjustment.IsChecked != true)
                {
                    DXMessageBox.Show("Please Select the Leave type first","Please Select ",MessageBoxButton.OK,MessageBoxImage.Stop);
                    return;
                }
                dateTo.IsEnabled = true;
                if (dateTo.EditValue == null)
                    dateTo.DateTime = dateFrom.DateTime;
            }
            catch (Exception ex)
            {

            }
           


        }

        private void TimeFrom_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                TimeTo.IsEnabled = true;
                if (TimeTo.EditValue == null)
                    TimeTo.DateTime = TimeFrom.DateTime;
            }
            catch (Exception ex)
            {

            }
           

        }

        private void DateTo_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                TimeSpan span = dateTo.DateTime.Subtract(dateFrom.DateTime);
                double days = (int)span.Days;
                if (days < 0)
                {
                    dateTo.Clear();
                    txtTotalDays.Text = "";
                    DXMessageBox.Show("Date To cannot be lesser than Date From", "Wrong Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if (chkIsHalf.IsChecked != true)
                    {
                        days = days + 1;
                    }
                    else
                    {
                        days = 0.5;
                    }
                    txtTotalDays.Text = days.ToString();

                    if (radAnnual.IsChecked == true)
                    {
                        newAnnual.Text = days.ToString();
                        newSick.Text = "0";
                        newAdj.Text = "0";

                    }
                    if (radCasual.IsChecked == true)
                    {
                        newAnnual.Text = "0";
                        newSick.Text = days.ToString();
                        newAdj.Text = "0";

                    }
                    if (radAdjustment.IsChecked == true)
                    {
                        newAnnual.Text = "0";
                        newSick.Text = "0";
                        newAdj.Text = days.ToString();

                    }
                    newTotal.Text = days.ToString();

                    double openingSickInt = 0;
                    double newSickInt = 0;
                    double openingAnnualInt = 0;
                    double newAnnualInt = 0;

                    double openingAdjInt = 0;
                    double newAdjInt = 0;

                    double openingTotalInt = 0;
                    double newTotalInt = 0;



                    if (!String.IsNullOrEmpty(openingSick.Text))
                    { openingSickInt = Convert.ToDouble(openingSick.Text); }

                    if (!String.IsNullOrEmpty(newSick.Text))
                    { newSickInt = Convert.ToDouble(newSick.Text); }

                    if (!String.IsNullOrEmpty(openingAnnual.Text))
                    { openingAnnualInt = Convert.ToDouble(openingAnnual.Text); }

                    if (!String.IsNullOrEmpty(newAnnual.Text))
                    { newAnnualInt = Convert.ToDouble(newAnnual.Text); }

                    if (!String.IsNullOrEmpty(openingAdj.Text))
                    { openingAdjInt = Convert.ToDouble(openingAdj.Text); }

                    if (!String.IsNullOrEmpty(newAdj.Text))
                    { newAdjInt = Convert.ToDouble(newAdj.Text); }


                    if (!String.IsNullOrEmpty(openingTotal.Text))
                    { openingTotalInt = Convert.ToDouble(openingTotal.Text); }

                    if (!String.IsNullOrEmpty(openingTotal.Text))
                    { newTotalInt = Convert.ToDouble(newTotal.Text); }
                    double closingSickInt = 0;
                    double closingAnnualInt = 0;
                    double closingAdjInt = 0;

                    double closingTotalInt = 0;
                    if (radAdjustment.IsChecked != true)
                    {
                         closingSickInt = openingSickInt - newSickInt;
                         closingAnnualInt = openingAnnualInt - newAnnualInt;
                        closingAdjInt = openingAdjInt - newAdjInt;

                        closingTotalInt = openingTotalInt - newTotalInt;

                    }
                    else if (radAdjustment.IsChecked == true)
                    {
                         closingSickInt = openingSickInt + newSickInt;
                         closingAnnualInt = openingAnnualInt + newAnnualInt;
                        closingAdjInt = openingAdjInt + newAdjInt;

                        closingTotalInt = openingTotalInt + newTotalInt;

                    }
                    

                    //if (isEdit == false)
                    //{
                        closingSick.Text = closingSickInt.ToString();
                        closingAnnual.Text = closingAnnualInt.ToString();
                        closingTotal.Text = closingTotalInt.ToString();
                        closingAdj.Text = closingAdjInt.ToString();
                    //}
                    
                }
            

            }
            catch (Exception ex)
            {

            }
        }

        private void CmbEmployee_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                if (cmbEmployee.SelectedItem != null)
                {
                    dateFrom.EditValue = null;
                    dateTo.EditValue = null;
                    //dateApplied.EditValue = null;
                    txtTotalDays.EditValue = null;
                    newSick.Text = null;
                    newAnnual.Text = null;
                    newTotal.Text = null;
                    closingSick.Text = null;
                    closingAnnual.Text = null;
                    closingTotal.Text = null;

                    var emp = cmbEmployee.SelectedItem as ERP_BL.Databases.Employee;
                    var user = UsersRepo.getuserByEmpId(emp.EmpId);

                    loadBasicInfo(user);
                }
                //    if (emp == null)
                //    {
                //        return;
                //    }
                //    txtFname.Text = emp.person.FName;
                //    txtLname.Text = emp.person.LName;

                //    if (emp.Desig != null)
                //    {
                //        txtPosition.Text = emp.Desig.Title;
                //        var allowedAnnual = emp.Desig.AnnualLeaveDays;
                //        var allowedCasual = emp.Desig.CasualLeaveDays;
                //        var totalLeaves = allowedAnnual + allowedCasual;

                //        totalSick.Text = allowedCasual.ToString();
                //        totalAnnual.Text = allowedAnnual.ToString();
                //        total.Text = totalLeaves.ToString();
                //    }

                //    //Load Annual Balance
                //    var annualLeaves = hrRepo.GetEmployeeAnnualLeaves(emp.EmpId);

                //    if (annualLeaves != null)
                //    {
                //        annualBalance = annualLeaves.Sum(x => x.LeaveDays);
                //    }
                //    else
                //    { annualBalance = 0; }

                //    var casualLeaves = hrRepo.GetEmployeeCasualLeaves(emp.EmpId);
                //    if (casualLeaves != null)
                //    {
                //        casualBalance = casualLeaves.Sum(x => x.LeaveDays);
                //    }
                //    else
                //    {
                //        casualBalance = 0;
                //    }
                //    totalBalance = annualBalance + casualBalance;

                //    openingSick.Text = casualBalance.ToString();
                //    openingAnnual.Text = annualBalance.ToString();
                //    openingTotal.Text = totalBalance.ToString();

                //}
            }
            catch (Exception ex)
            {
            }
        }

        private void ColorEditStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RadAdjustment_Checked(object sender, RoutedEventArgs e)
        {
            dateFrom.EditValue = null;
            dateTo.EditValue = null;
            //dateApplied.EditValue = null;
            txtTotalDays.EditValue = null;
            newSick.Text = null;
            newAnnual.Text = null;
            newTotal.Text = null;
            closingSick.Text = null;
            closingAnnual.Text = null;
            closingTotal.Text = null;
            newAdj.Text = null;
            closingAdj.Text = null;


            grdDateFrom.Visibility = Visibility.Visible;
            grdDateTo.Visibility = Visibility.Visible;
            grdTotalDays.Visibility = Visibility.Visible;

            grdTimeFrom.Visibility = Visibility.Collapsed;
            grdTimeTo.Visibility = Visibility.Collapsed;
            grdTotalTime.Visibility = Visibility.Collapsed;

            chkIsHalf.Visibility = Visibility.Visible;
            grdLeaveDate.Visibility = Visibility.Collapsed;

        }

        private void RadAdjustment_Unchecked(object sender, RoutedEventArgs e)
        {
            chkIsHalf.Visibility = Visibility.Collapsed;
        }

        private void ChkIsHalf_Checked(object sender, RoutedEventArgs e)
        {
            dateFrom.EditValue = null;
            dateTo.EditValue = null;
            //dateApplied.EditValue = null;
            txtTotalDays.EditValue = null;
            newSick.Text = null;
            newAnnual.Text = null;
            newTotal.Text = null;
            closingSick.Text = null;
            closingAnnual.Text = null;
            closingTotal.Text = null;
            newAdj.Text = null;
            closingAdj.Text = null;
        }

        private void ChkIsHalf_Unchecked(object sender, RoutedEventArgs e)
        {
            dateFrom.EditValue = null;
            dateTo.EditValue = null;
            //dateApplied.EditValue = null;
            txtTotalDays.EditValue = null;
            newSick.Text = null;
            newAnnual.Text = null;
            newTotal.Text = null;
            closingSick.Text = null;
            closingAnnual.Text = null;
            closingTotal.Text = null;
            newAdj.Text = null;
            closingAdj.Text = null;
        }

        private void InputForm_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //change the event to avoid close form
            //receiptListFlag = 0;

            var approve = MessageBox.Show("Do you want to Approve?", "Approval", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (approve == MessageBoxResult.Yes)
            {
                inputFormCloseFlag = false;
            }
            else
            {
                inputFormCloseFlag = true;
            }

            e.Cancel = false;
        }
        private void BtnApproveLeave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inputFormCloseFlag = false;

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Leave without Approval") != null)
                {
                    LeaveApplication leave = new LeaveApplication();
                    HrRepo hrRepo = new HrRepo();
                    UsersRepo usersRepo = new UsersRepo();

                    //if (grdLeaveRegisters.GetFocusedRow() != null)
                    //{
                    //    //SalesReceipt receipt = new SalesReceipt();
                    if (!String.IsNullOrEmpty(leaveIdStr.Text))
                    {
                        var id = Convert.ToInt32(leaveIdStr.Text);
                        leave = hrRepo.GetLeaveApplication(id);
                    }
                    //var selectedRow = (LeaveApplication)grdLeaveRegisters.GetFocusedRow();
                    //if (selectedRow == null)
                    //{ return; }
                    //if (selectedRow.Id != 0)
                    //{
                    //    
                    //}
                    else
                    { return; }


                    if (leave.isApproved == false)
                    {
                        if (leave.isApproved != true)

                            if (
                                ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Leave without Approval") != null
                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Leave") != null)
                                ) ? true : false)
                            {
                                leave.isApproved = true;
                                leave.leave.isApproved = true;
                                leave.stage = TransactionStage.Approved;

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, leave.Id, 15, frmInputBox.comment);


                            }
                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Leave") != null)
                            {
                                if (leave.isApproved == null)
                                {
                                    leave.isApproved = false;
                                    leave.leave.isApproved = false;
                                    leave.stage = TransactionStage.AwaitingApproval;

                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Reviewed, leave.Id, 15, frmInputBox.comment);
                            }

                            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Leave") != null)
                            {
                                if (leave.isApproved == null)
                                {
                                    leave.isApproved = false;
                                    leave.leave.isApproved = false;
                                    leave.stage = TransactionStage.AwaitingApproval;


                                }

                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();

                                usersRepo.Add(TransactionInfo.Reviewed, leave.Id, 15, frmInputBox.comment);

                            }

                            else
                            {
                                leave.isApproved = false;
                                leave.leave.isApproved = false;
                                leave.stage = TransactionStage.AwaitingApproval;


                            }

                        if (inputFormCloseFlag == false)
                        {
                            hrRepo.UpdateLeaveApplication(leave);
                            MessageBox.Show("Leave is Approved (" + leave.Id + ")");
                            SystemLog.LogInfo(this.GetType(), "Leave is Approved (" + leave.Id + ")");
                        }
                    }
                    else if (leave.isReApproved == false)
                    {
                        if (leave.isReApproved != true)

                            if (
                                ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Leave without ReApproval") != null
                                || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "ReApprover for added Leave") != null)

                                ) ? true : false)
                            {
                                leave.isReApproved = true;
                                frmInputBox inputBox = new frmInputBox();
                                inputBox.Closing += InputForm_Window_Closing;
                                inputBox.ShowDialog();
                                usersRepo.Add(TransactionInfo.Approved_Adding, leave.Id, 15, frmInputBox.comment);
                                leave.stage = TransactionStage.Approved;


                            }


                            else
                            {
                                leave.isReApproved = false;
                                leave.stage = TransactionStage.AwaitingApproval;

                            }

                        if (inputFormCloseFlag == false)
                        {
                            hrRepo.UpdateLeaveApplication(leave);
                            MessageBox.Show("Leave is Re-Approved (" + leave.Id + ")");
                            SystemLog.LogInfo(this.GetType(), "Leave is Re-Approved (" + leave.Id + ")");
                        }


                    }
                    //}

                }
                else
                {
                    DXMessageBox.Show("You are not Allowed to Approve Leave Directly.", "Unauthorized Access", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
