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
    /// Interaction logic for ucFrmTargetGroup.xaml
    /// </summary>
    public partial class ucFrmTargetGroup : UserControl
    {
        TargetGroup targetGroup= new TargetGroup();
        public int GroupId = 0;
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public bool editFlag = false;
        public ucFrmTargetGroup()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lookUpTargetGroup.ItemsSource = taskRepo.GetAllTargetGroups();
            if (editFlag == true && GroupId > 0)
            {
                targetGroup = taskRepo.GetTargetGroup(GroupId);
                txtTargetGroup.Text = targetGroup.GroupName;

                if(targetGroup.targetGroup != null)
                    lookUpTargetGroup.Text = targetGroup.targetGroup.GroupName;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            targetGroup.GroupName = txtTargetGroup.Text;

            if(lookUpTargetGroup.SelectedIndex > -1)
            {
                var parent = lookUpTargetGroup.SelectedItem as TargetGroup;
                targetGroup.parentId = parent.Id;
            }
            else
                targetGroup.parentId = null;

            //taskRepo = new ToDoTaskRepo();
            if (editFlag == false)
            {
                taskRepo.AddTargetGroup(targetGroup);
                DXMessageBox.Show("Added Succesfully!");
            }
            else
            {
                taskRepo.UpdateTargetGroup(targetGroup);
                DXMessageBox.Show("Updated Succesfully!");
            }
            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
