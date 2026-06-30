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
    /// Interaction logic for ucStatusCalculationTypeList.xaml
    /// </summary>
    public partial class ucStatusCalculationTypeList : UserControl
    {

        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public ucStatusCalculationTypeList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            taskRepo = new ToDoTaskRepo();
            grdCntrlStatusCalculationTypeList.ItemsSource = taskRepo.GetAllStatusCalculationTypes();
        }

        private void MbtnAddStatusCalculationType_Click(object sender, RoutedEventArgs e)
        {
            ucFrmStatusCalculationType frmTargetType = new ucFrmStatusCalculationType();
            Window win = new Window();
            frmTargetType.editFlag = false;
            win.Content = frmTargetType;
            win.Width = 500;
            win.Height = 350;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditStatusCalculationType_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlStatusCalculationTypeList.SelectedItem != null)
            {
                var selectedRow = grdCntrlStatusCalculationTypeList.SelectedItem as StatusCalculationType;
                ucFrmStatusCalculationType frmTargetType = new ucFrmStatusCalculationType();
                Window win = new Window();
                frmTargetType.typeId = selectedRow.Id;
                frmTargetType.editFlag = true;
                win.Content = frmTargetType;
                win.Width = 500;
                win.Height = 350;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taskRepo = new ToDoTaskRepo();
            grdCntrlStatusCalculationTypeList.ItemsSource = taskRepo.GetAllStatusCalculationTypes();
        }        
    }
}
