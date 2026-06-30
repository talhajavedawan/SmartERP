using DevExpress.Xpf.Core;
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
    /// Interaction logic for ucPackingStyleAdd.xaml
    /// </summary>
    public partial class ucPackingStyleAdd : UserControl
    {
        public bool editFlag = false;
        TaskRepo taskRepo = new TaskRepo();
        PackingStyle packingStyle = new PackingStyle();
        public int packingStyleId = 0;

        public ucPackingStyleAdd()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(txtPackingStyle.Text))
            {
                DXMessageBox.Show("Please Enter Packing Style!");
                txtPackingStyle.Focus();
                return;
            }

            packingStyle.Name = txtPackingStyle.Text;

            if (editFlag == false)
            {
                taskRepo.AddPackingStyle(packingStyle);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                taskRepo.UpdatePackingStyle(packingStyle);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                packingStyle = taskRepo.GetPackingStyle(packingStyleId);

                txtPackingStyle.Text = packingStyle.Name;
            }
        }
    }
}
