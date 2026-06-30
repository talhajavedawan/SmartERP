using DevExpress.Xpf.Grid;
using ERP_BL.ChatManager;
using ERP_BL.Databases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

namespace ZAS_ERP.Chat
{
    /// <summary>
    /// Interaction logic for frmAddGroup.xaml
    /// </summary>
    public partial class frmAddGroup : Window
    {
        public frmAddGroup()
        {
            InitializeComponent();
        }

//Objects to use making grouops

        UsersRepo up = new UsersRepo();
        List<ChatUser> listUsers = new List<ChatUser>();
        ChatManager cm = new ChatManager();
        public int parentID;

//All UI button clicks (maximize,minimize,adjustments,close)  

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            //this.WindowState = WindowState.Minimized;
            this.Close();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    this.DragMove();
                }
        }
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                //MaximizeButton.Content = "1";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                //MaximizeButton.Content = "2";
            }

        }


//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


//loading All users
        public void loadUsers()
        {

            var users = up.getAllActiveUsersForChat();
            List<ChatUser> chatUser = new List<ChatUser>();
             foreach(var user in users)
            {
                ChatUser us = new ChatUser()
                {
                    userName = user.userName,
                    employeeId = user.employeeId
                };

                chatUser.Add(us);
            }

            allUsers.ItemsSource = chatUser;
        }

 //Window loaded 

        private void WindowGroupadd_Loaded(object sender, RoutedEventArgs e)
        {
            loadUsers();
            
        }

//Save button

        private void BtnGrouptSave_Click(object sender, RoutedEventArgs e)
        {
        
       
            listUsers.Clear();
            if (allUsers.SelectedItems.Count != 0)
            {
                foreach (var item in allUsers.SelectedItems)
                {
                    listUsers.Add((ChatUser)item);
                }
                CreatGroup();

            }
            else
                MessageBox.Show("Please select users");    
        }

//Push and creat group towards DB 

        public void CreatGroup()
        {
            var count = ((ICollection)lookupParentGroup.ItemsSource).Count;

            List<ChatUser> groupUsers = new List<ChatUser>();
            foreach (var user in listUsers)
            {
                ChatUser _user = new ChatUser()
                {
                    employeeId = user.employeeId,
                    userName = user.userName,
                };
                groupUsers.Add(_user);
            }
            ChatGroup group = new ChatGroup()
            {
                groupName = txtGroupname.Text,
                id = count,
                users = groupUsers,
                creationDate = Convert.ToDateTime(dateGroupCreation.EditValue),
                parentId = parentID
            };
            cm.CreateNewGroup(group, false);

            MessageBox.Show("Group created succussfully");
            this.Close();
        }

//window closed event 

        private void WindowGroupadd_Closed(object sender, EventArgs e)
        {
            MakeGroup obj = new MakeGroup();
            obj.LoadAllGroups();
        }

//Check subsidary  

        private void chkisSubsidaiary_Checked(object sender, RoutedEventArgs e)
        {
            if (lookupParentGroup.IsVisible == false)
            {
                lookupParentGroup.Visibility = Visibility.Visible;
                lblParentGroup.Visibility = Visibility.Visible;
                loadGroupdata();
            }
        }

// uncheck subsidary 

        private void chkisSubsidaiary_Unchecked(object sender, RoutedEventArgs e)
        {
            if (lookupParentGroup.IsVisible == true)
                lookupParentGroup.Visibility = Visibility.Collapsed;
            if (lblParentGroup.IsVisible == true)
                lblParentGroup.Visibility = Visibility.Collapsed;
            loadUsers();
        }

//load all users exists in every group 

        public void loadGroupUsers(int id,List< ChatUser> users)
        {
            allUsers.ItemsSource = users;
        }

// load groups in lookup edit popup

        public void loadGroupdata()
        {
            var groups = cm.GetAllGroups(false);
            lookupParentGroup.ItemsSource = groups;
        }

//lookupEdit selectionChanged 

        private void LookupParentGroup_PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            try
            {
                var group = (ChatGroup)e.EditValue;
                loadGroupUsers(group.id,group.users);
                parentID = group.id;
            }
            catch
            {
            }
        }
        private void DateGroupCreation_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGroupname.Text))
            {
                MessageBox.Show("Please enter Group name");
                txtGroupname.Focus();
            }

        }

        private void WindowGroupadd_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
