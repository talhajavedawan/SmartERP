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
    /// Interaction logic for ucEfficiencyPointsList.xaml
    /// </summary>
    public partial class ucEfficiencyPointsList : UserControl
    {
        TaskRepo taskRepo = new TaskRepo();
        public ucEfficiencyPointsList()
        {
            InitializeComponent();
        }

        private void GrdCntrlEfficiencyPointsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Deduction") != null)
                //{
                ucAddEfficiencyPointsHead ucEfficiencyPoints = new ucAddEfficiencyPointsHead();
                ucEfficiencyPoints.editFlag = false;
                ucEfficiencyPoints.efficiencyPointId = 0;
                Window win = new Window();
                    win.Title = "Add Deduction Form";
                win.Content = ucEfficiencyPoints;
                win.Height = 350;
                win.Width = 400;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();

                //}
                //else
                //{
                //    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new Deductions!");
                //    return;
                //}
            }
            catch
            {

            }
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var efficiencyPoints = grdCntrlEfficiencyPointsList.SelectedItem as EfficiencyPoints;
            ucAddEfficiencyPointsHead ucEfficiencyPoints = new ucAddEfficiencyPointsHead();
            ucEfficiencyPoints.editFlag = true;
            ucEfficiencyPoints.efficiencyPointId = efficiencyPoints.Id;
            Window win = new Window();
            win.Content = ucEfficiencyPoints;
            win.Height = 350;
            win.Width = 400;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlEfficiencyPointsList.ItemsSource = taskRepo.GetAllEfficiencyPoints();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taskRepo = new TaskRepo();
            grdCntrlEfficiencyPointsList.ItemsSource = taskRepo.GetAllEfficiencyPoints();
        }
    }
}
