using DevExpress.Xpf.Core;
using ERP_BL.BackgroundImages;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.Memos;
using ERP_BL.ToDoTasks;
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
using ZAS_ERP.Memos.PerformanceReview;

namespace ZAS_ERP.Memos
{
    /// <summary>
    /// Interaction logic for ucFrmMemoAdd.xaml
    /// </summary>
    public partial class ucFrmMemoAdd : UserControl
    {
        MemoRepo memoRepo = new MemoRepo();
        Memo memo = new Memo();
        public int memoId = 0;
        public bool editFlag = false;
        public MemoType memoType;
        public ucFrmMemoAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
            LoadGroupMemos();
            LoadMemoTypes();

            if(editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
                lookupCreatedBy.Text = SYSTEM_STATIC.currentUser.employee.person.FullName;
                cmbxMemoType.SelectedIndex = (int)memoType;

                btnPerformanceReview.IsEnabled = false;
                switch (memoType)
                {
                    case MemoType.Linked:
                        grdCreatedFor.Visibility = Visibility.Visible;
                        grdMemoGroup.Visibility = Visibility.Collapsed;
                        break;

                    case MemoType.Non_Linked:
                        //grdCreatedFor.Visibility = Visibility.Collapsed;
                        //grdMemoGroup.Visibility = Visibility.Collapsed;
                        //grdCCusers.Visibility = Visibility.Collapsed;
                        break;

                    case MemoType.Group:
                        grdCreatedFor.Visibility = Visibility.Collapsed;
                        grdMemoGroup.Visibility = Visibility.Visible;
                        break;
                }

            }
            if(editFlag == true && memoId > 0)
            {
                memo = memoRepo.GetMemo(memoId);

                var review = memoRepo.GetPerformanceReviewByMemoId(memoId);
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Create Performance Review from Memo") == null || review != null)
                {
                    btnPerformanceReview.IsEnabled = false;
                }
                


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Memo") == null)
                    btnSave.IsEnabled = false;

                memoType = memo.memoType;

                cmbxMemoType.SelectedIndex = (int)memo.memoType;

                if (memo.CreationDate != null)
                    datCreationDate.EditValue = memo.CreationDate;

                txtMemoTopic.Text = memo.Subject;

                if (memo.createdBy != null)
                    lookupCreatedBy.Text = memo.createdBy.employee.person.FullName;

                switch (memo.memoType)
                {
                    case MemoType.Linked:
                        grdCreatedFor.Visibility = Visibility.Visible;
                        grdMemoGroup.Visibility = Visibility.Collapsed;
                        if (memo.createdFor != null)
                            lookupCreatedFor.Text = memo.createdFor.employee.person.FullName;
                        break;

                    case MemoType.Non_Linked:
                        grdCreatedFor.Visibility = Visibility.Visible;
                        grdMemoGroup.Visibility = Visibility.Collapsed;
                        if (memo.createdFor != null)
                            lookupCreatedFor.Text = memo.createdFor.employee.person.FullName;
                        break;

                    case MemoType.Group:
                        grdCreatedFor.Visibility = Visibility.Collapsed;
                        grdMemoGroup.Visibility = Visibility.Visible;
                        if (memo.taskGroup != null)
                            lookupMemoGroup.Text = memo.taskGroup.GroupName;
                        break;
                }

               
                var CCusersSource = cmbCCUsers.ItemsSource as List<User>;
                foreach (User cmbitem in CCusersSource)
                {
                    if (memo.CCUsersList.Find(x => x.id == cmbitem.id) != null)
                    {
                        cmbCCUsers.SelectedItems.Add(cmbitem);
                        //break;
                    }
                }
                
            }
        }

        private void LoadMemoTypes()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.MemoType.Group; i++)
            {
                cmbxMemoType.Items.Add(((ERP_BL.Enums.MemoType)i).ToString());
            }
        }
        private void LoadUsers()
        {
            UsersRepo usersRepo = new UsersRepo();

            var users = usersRepo.getAllActiveUsersForMemo();
            lookupCreatedBy.ItemsSource = users;
            lookupCreatedFor.ItemsSource = users;

            if (users != null && users.Count > 0)
            {
                foreach (var _user in users)
                {
                    var userList = (cmbCCUsers.ItemsSource as List<User>) == null ? new List<User>() : cmbCCUsers.ItemsSource as List<User>;
                    if (userList.Find(x => x.id == _user.id) == null)
                        userList.Add(_user);

                    cmbCCUsers.ItemsSource = null;
                    cmbCCUsers.ItemsSource = userList;
                }
            }
        }

        private void LoadGroupMemos()
        {
            BackgroundImagesRepo backgroundImagesRepo = new BackgroundImagesRepo();
            var TaskGroups = backgroundImagesRepo.GetAllTaskGroups();
            lookupMemoGroup.ItemsSource = TaskGroups;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(datCreationDate.DateTime == null)
            {
                DXMessageBox.Show("Please select Creation Date!");
                datCreationDate.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtMemoTopic.Text))
            {
                DXMessageBox.Show("Please Enter Topic!");
                txtMemoTopic.Focus();
                return;
            }
            if (lookupCreatedBy.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Created By User!");
                lookupCreatedBy.Focus();
                return;
            }


            memo.CreationDate = datCreationDate.DateTime;
            memo.Subject = txtMemoTopic.Text;
            memo.createdById = (lookupCreatedBy.SelectedItem as User).id;


            switch (memoType)
            {
                case MemoType.Linked:
                    if (lookupCreatedFor.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Created For User!");
                        lookupCreatedFor.Focus();
                        return;
                    }
                    memo.memoType = MemoType.Linked;
                    memo.createdForId = (lookupCreatedFor.SelectedItem as User).id;
                    memo.taskGroupId = null;
                    break;
                case MemoType.Non_Linked:
                    if (lookupCreatedFor.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Created For User!");
                        lookupCreatedFor.Focus();
                        return;
                    }
                    memo.memoType = MemoType.Non_Linked;
                    memo.createdForId = (lookupCreatedFor.SelectedItem as User).id;
                    memo.taskGroupId = null;
                    break;
                case MemoType.Group:
                    if (lookupMemoGroup.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Memo Group!");
                        lookupMemoGroup.Focus();
                        return;
                    }
                    memo.memoType = MemoType.Group;
                    memo.taskGroupId = (lookupMemoGroup.SelectedItem as TaskGroups).Id;
                    memo.createdForId = null;
                    break;
            }

            if (cmbCCUsers.SelectedItems.Count != 0)
            {
                if (cmbCCUsers.SelectedItems != null)
                {
                    var CCUsers = new List<User>();
                    foreach (User user in cmbCCUsers.SelectedItems)
                    {
                        CCUsers.Add(new User { id = user.id });

                    }
                    memo.CCUsersList = CCUsers;

                }
            }
            else
            {
                memo.CCUsersList = new List<User>();
            }

            if (editFlag == true)
            {
                memoRepo.UpdateMemo(memo);
                DXMessageBox.Show("Successfully Updated");
            }
            else if(editFlag == false)
            {
                memoRepo.AddMemo(memo);
                DXMessageBox.Show("Successfully Added");
            }

            Window win = Window.GetWindow(this);
            win.Close();
        }

        private void chkIsGroupMemo_Checked(object sender, RoutedEventArgs e)
        {
            grdMemoGroup.Visibility = Visibility.Visible;
            grdCreatedFor.Visibility = Visibility.Collapsed;
        }

        private void chkIsGroupMemo_Unchecked(object sender, RoutedEventArgs e)
        {
            grdMemoGroup.Visibility = Visibility.Collapsed;
            grdCreatedFor.Visibility = Visibility.Visible;
        }

        private void btnPerformanceReview_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                if(memoId > 0)
                {
                    var review = memoRepo.GetPerformanceReviewByMemoId(memoId);
                    if(review != null)
                    {
                        DXMessageBox.Show("Performance Review already created for this Memo!");
                        return;
                    }

                    PerformanceReviewWindow frmPerformanceReview = new PerformanceReviewWindow();
                    frmPerformanceReview.memoId = memoId;
                    frmPerformanceReview.editFlag = false;
                    frmPerformanceReview.reviewIdToEdit = 0;
                    //frmPerformanceReview.editFlag = false;
                    //frmPerformanceReview.performanceReviewId = 0;

                    //DXWindow window = new DXWindow();
                    //window.Width = 650;
                    //window.Height = 850;
                    //window.Content = frmPerformanceReview;
                    //window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //window.ResizeMode = ResizeMode.CanMinimize;

                    frmPerformanceReview.Show();
                }
            }
        }
    }
}
