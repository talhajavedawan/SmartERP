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
    /// Interaction logic for ucLotNumberList.xaml
    /// </summary>
    public partial class ucLotNumberList : UserControl
    {
        TaskRepo taskRepo = new TaskRepo();
        public ucLotNumberList()
        {
            InitializeComponent();
        }

        private void MbtnAddLOTno_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Bill Reference") != null)
            {
                ucFrmLotNumberAdd frmLotNumberAdd = new ucFrmLotNumberAdd();
                frmLotNumberAdd.editFlag = false;
                Window win = new Window();
                win.Content = frmLotNumberAdd;
                win.Height = 350;
                win.Width = 400;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add Bill Reference!");
            }

        }

        private void MbtnEditLOTno_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Bill Reference") != null)
            {
                var selectedRow = grdCntrlLOTnoList.SelectedItem as LotNumber;

                if (selectedRow != null)
                {
                    ucFrmLotNumberAdd frmLotNumberAdd = new ucFrmLotNumberAdd();
                    frmLotNumberAdd.editFlag = true;
                    frmLotNumberAdd.lotNoId = selectedRow.Id;
                    Window win = new Window();
                    win.Content = frmLotNumberAdd;
                    win.Height = 350;
                    win.Width = 400;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ResizeMode = ResizeMode.CanMinimize;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Edit Bill Reference!");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            taskRepo = new TaskRepo();
            grdCntrlLOTnoList.ItemsSource = taskRepo.GetAllLotNumber();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            taskRepo = new TaskRepo();
            grdCntrlLOTnoList.ItemsSource = taskRepo.GetAllLotNumber();
        }
    }
}
