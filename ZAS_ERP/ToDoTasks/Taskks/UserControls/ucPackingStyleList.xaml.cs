using ERP_BL.Databases;
using ERP_BL.ToDoTasks.Taskss;
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

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucPackingStyleList.xaml
    /// </summary>
    public partial class ucPackingStyleList : UserControl
    {
        TaskRepo taskRepo = new TaskRepo();
        public ucPackingStyleList()
        {
            InitializeComponent();
        }

        private void MbtnAddPackingStyle_Click(object sender, RoutedEventArgs e)
        {
            ucPackingStyleAdd ucAddPackingStyle = new ucPackingStyleAdd();
            Window win = new Window();
            ucAddPackingStyle.editFlag = false;
            win.Content = ucAddPackingStyle;
            win.Width = 400;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditPackingStyle_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlPackingStyleList.SelectedItem != null)
            {
                var selectedRow = grdCntrlPackingStyleList.SelectedItem as PackingStyle;
                ucPackingStyleAdd ucPackingStyle = new ucPackingStyleAdd();
                Window win = new Window();
                ucPackingStyle.packingStyleId = selectedRow.Id;
                ucPackingStyle.editFlag = true;
                win.Content = ucPackingStyle;
                win.Width = 400;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taskRepo = new TaskRepo();
            var listt = taskRepo.GetAllPackingStyles();
            grdCntrlPackingStyleList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            taskRepo = new TaskRepo();
            var listt = taskRepo.GetAllPackingStyles();
            grdCntrlPackingStyleList.ItemsSource = listt;
        }

    }
}
