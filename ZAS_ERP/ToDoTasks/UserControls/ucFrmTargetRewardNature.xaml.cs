using DevExpress.Xpf.Core;
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
    /// Interaction logic for ucFrmTargetRewardNature.xaml
    /// </summary>
    public partial class ucFrmTargetRewardNature : UserControl
    {
        TargetRewardNature targetReward = new TargetRewardNature();
        public int rewardId = 0;
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public bool editFlag = false;
        public ucFrmTargetRewardNature()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && rewardId > 0)
            {
                targetReward = taskRepo.GetTargetRewardNature(rewardId);
                txtTargetReward.Text = targetReward.NatureName;
                chkIsActive.IsChecked = targetReward.isActive;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkIsActive.IsChecked == true)
                targetReward.isActive = true;
            else
                targetReward.isActive = false;

            targetReward.NatureName = txtTargetReward.Text;
            //taskRepo = new ToDoTaskRepo();
            if (editFlag == false)
            {
                taskRepo.AddTargetRewardNature(targetReward);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                taskRepo.UpdateTargetRewardNature(targetReward);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

    }
}
