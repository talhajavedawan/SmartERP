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
    /// Interaction logic for ucTargetTypeList.xaml
    /// </summary>
    public partial class ucTargetTypeList : UserControl
    {
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public ucTargetTypeList()
        {
            InitializeComponent();
        }

        private void MbtnAddTargetType_Click(object sender, RoutedEventArgs e)
        {
            ucFrmTargetType frmTargetType = new ucFrmTargetType();
            Window win = new Window();
            frmTargetType.editFlag = false;
            win.Content = frmTargetType;
            win.Width = 300;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditTargetType_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlTargetTypeList.SelectedItem != null)
            {
                var selectedRow = grdCntrlTargetTypeList.SelectedItem as TaskTargetType;
                ucFrmTargetType frmTargetType = new ucFrmTargetType();
                Window win = new Window();
                frmTargetType.typeId = selectedRow.Id;
                frmTargetType.editFlag = true;
                win.Content = frmTargetType;
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
            grdCntrlTargetTypeList.ItemsSource = taskRepo.GetAllTargetTypes();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taskRepo = new ToDoTaskRepo();
            grdCntrlTargetTypeList.ItemsSource = taskRepo.GetAllTargetTypes();
        }
    }
}
