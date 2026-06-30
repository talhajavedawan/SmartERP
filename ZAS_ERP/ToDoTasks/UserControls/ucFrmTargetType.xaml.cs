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
    /// Interaction logic for ucFrmTargetType.xaml
    /// </summary>
    public partial class ucFrmTargetType : UserControl
    {
        TaskTargetType targetType = new TaskTargetType();
        public int typeId = 0;
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public bool editFlag = false;
        public ucFrmTargetType()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true && typeId > 0)
            {
                targetType = taskRepo.GetTargetType(typeId);
                txtTargetType.Text = targetType.TargetTypeName;
                chkIsActive.IsChecked = targetType.isActive;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkIsActive.IsChecked == true)
                targetType.isActive = true;
            else
                targetType.isActive = false;

            targetType.TargetTypeName = txtTargetType.Text;
            //taskRepo = new ToDoTaskRepo();
            if (editFlag == false)
            {
                taskRepo.AddTargetType(targetType);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                taskRepo.UpdateTargetType(targetType);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
