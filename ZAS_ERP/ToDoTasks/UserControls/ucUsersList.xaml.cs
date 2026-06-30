using ERP_BL.Databases;
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

namespace ZAS_ERP.ToDoTasks.UserControls
{
    /// <summary>
    /// Interaction logic for ucUsersList.xaml
    /// </summary>
    public partial class ucUsersList : UserControl
    {
        List<User> groupUsers = new List<User>();
        public ucUsersList()
        {
            InitializeComponent();
        }

        public ucUsersList(List<User> users)
        {
            InitializeComponent();
            groupUsers = users;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlUsers.ItemsSource = groupUsers;
        }
    }
}
