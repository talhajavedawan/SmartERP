using DevExpress.Xpf.Core;
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
    /// Interaction logic for ucGoodReceiveNoteAdd.xaml
    /// </summary>
    public partial class ucGoodReceiveNoteAdd : UserControl
    {
        public bool editFlag = false;
        TaskRepo taskRepo = new TaskRepo();
        GoodReceiveNote goodReceiveNote = new GoodReceiveNote();
        public int packingStyleId = 0;

        public ucGoodReceiveNoteAdd()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            if (String.IsNullOrEmpty(txtPackingStyle.Text))
            {
                DXMessageBox.Show("Please Enter Receive Note!");
                txtPackingStyle.Focus();
                return;
            }

            goodReceiveNote.Name = txtPackingStyle.Text;

            if (editFlag == false)
            {
                taskRepo.AddGoodReceiveNote(goodReceiveNote);
                DXMessageBox.Show("Added Succesfully!");
            }

            else
            {
                taskRepo.UpdateGoodReceiveNote(goodReceiveNote);
                DXMessageBox.Show("Updated Succesfully!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                goodReceiveNote = taskRepo.GetGoodReceiveNote(packingStyleId);

                txtPackingStyle.Text = goodReceiveNote.Name;
            }
        }

    }
}
