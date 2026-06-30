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

namespace ZAS_ERP.ToDoTasks.UserControls
{
    /// <summary>
    /// Interaction logic for ucTargetGroupList.xaml
    /// </summary>
    public partial class ucTargetGroupList : UserControl
    {
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public ucTargetGroupList()
        {
            InitializeComponent();
        }

        private void MbtnAddTargetGroup_Click(object sender, RoutedEventArgs e)
        {
            ucFrmTargetGroup frmTargetGroup = new ucFrmTargetGroup();
            Window win = new Window();
            frmTargetGroup.editFlag = false;
            win.Content = frmTargetGroup;
            win.Width = 300;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditTargetGroup_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlTargetGroupList.SelectedItem != null)
            {
                var selectedRow = grdCntrlTargetGroupList.SelectedItem as TargetGroup;
                ucFrmTargetGroup frmTargetGroup = new ucFrmTargetGroup();
                Window win = new Window();
                frmTargetGroup.GroupId = selectedRow.Id;
                frmTargetGroup.editFlag = true;
                win.Content = frmTargetGroup;
                win.Width = 300;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            taskRepo = new ToDoTaskRepo();
            grdCntrlTargetGroupList.ItemsSource = taskRepo.GetAllTargetGroups();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taskRepo = new ToDoTaskRepo();
            grdCntrlTargetGroupList.ItemsSource = taskRepo.GetAllTargetGroups();
        }
    }
}
