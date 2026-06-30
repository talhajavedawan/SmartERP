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
    /// Interaction logic for ucTargetRewardNatureList.xaml
    /// </summary>
    public partial class ucTargetRewardNatureList : UserControl
    {
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public ucTargetRewardNatureList()
        {
            InitializeComponent();
        }

        private void MbtnAddRewardNature_Click(object sender, RoutedEventArgs e)
        {
            ucFrmTargetRewardNature frmTargetRewardNature = new ucFrmTargetRewardNature();
            Window win = new Window();
            frmTargetRewardNature.editFlag = false;
            win.Content = frmTargetRewardNature;
            win.Width = 300;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditRewardNature_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlTargetRewardNatureList.SelectedItem != null)
            {
                var selectedRow = grdCntrlTargetRewardNatureList.SelectedItem as TargetRewardNature;
                ucFrmTargetRewardNature frmTargetRewardNature = new ucFrmTargetRewardNature();
                Window win = new Window();
                frmTargetRewardNature.rewardId = selectedRow.Id;
                frmTargetRewardNature.editFlag = true;
                win.Content = frmTargetRewardNature;
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
            grdCntrlTargetRewardNatureList.ItemsSource = taskRepo.GetAllTargetRewardNatures();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taskRepo = new ToDoTaskRepo();
            grdCntrlTargetRewardNatureList.ItemsSource = taskRepo.GetAllTargetRewardNatures();
        }

    }
}
