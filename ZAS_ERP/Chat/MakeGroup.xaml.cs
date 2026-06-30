using ERP_BL.ChatManager;
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

namespace ZAS_ERP.Chat
{
    /// <summary>
    /// Interaction logic for MakeGroup.xaml
    /// </summary>
    public partial class MakeGroup : Window
    {
        public MakeGroup()
        {
            InitializeComponent();
        }
//ALL objects used in this Window

        ChatManager cm = new ChatManager();

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllGroups();

        }


//groups loading

        public void LoadAllGroups()
        {
            try { 
            var allgroups = cm.GetAllGroups(false);
            List<ChatGroup> groups = new List<ChatGroup>();
            foreach(var group in allgroups)
            {
                groups.Add(new ChatGroup() { id = group.id, parentId = group.parentId, groupName = group.groupName, creationDate = group.creationDate, users = group.users });
            }
            gridGroups.ItemsSource = groups;
            }
            catch { }
        }

//groups selection changed 

        private void GridGroups_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            var selectedGroup = (ChatGroup)gridGroups.SelectedItem;
            LoadAllGroupUser(selectedGroup.id, selectedGroup.users);
        }

//load all group Users

        public void LoadAllGroupUser(int id, List<ChatUser> users)
        {  
            groupUsers.ItemsSource = users;
        }

//Add new click on contextmenu 

        private void BtnNewGroup_Click(object sender, RoutedEventArgs e)
        {
            frmAddGroup frmGroup = new frmAddGroup();
            frmGroup.ShowDialog();
        }
    }
}
   