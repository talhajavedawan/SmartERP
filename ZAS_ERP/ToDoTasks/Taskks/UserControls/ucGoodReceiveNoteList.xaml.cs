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
    /// Interaction logic for ucGoodReceiveNoteList.xaml
    /// </summary>
    public partial class ucGoodReceiveNoteList : UserControl
    {
        TaskRepo taskRepo = new TaskRepo();
        public ucGoodReceiveNoteList()
        {
            InitializeComponent();
        }


        private void MbtnAddReceiveNote_Click(object sender, RoutedEventArgs e)
        {
            ucGoodReceiveNoteAdd ucAddReceiveNote = new ucGoodReceiveNoteAdd();
            Window win = new Window();
            ucAddReceiveNote.editFlag = false;
            win.Content = ucAddReceiveNote;
            win.Width = 400;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditReceiveNote_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlReceiveNoteList.SelectedItem != null)
            {
                var selectedRow = grdCntrlReceiveNoteList.SelectedItem as GoodReceiveNote;
                ucGoodReceiveNoteAdd ucReceiveNote = new ucGoodReceiveNoteAdd();
                Window win = new Window();
                ucReceiveNote.packingStyleId = selectedRow.Id;
                ucReceiveNote.editFlag = true;
                win.Content = ucReceiveNote;
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
            var listt = taskRepo.GetAllGoodReceiveNotes();
            grdCntrlReceiveNoteList.ItemsSource = listt;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            taskRepo = new TaskRepo();
            var listt = taskRepo.GetAllGoodReceiveNotes();
            grdCntrlReceiveNoteList.ItemsSource = listt;
        }
    }
}
