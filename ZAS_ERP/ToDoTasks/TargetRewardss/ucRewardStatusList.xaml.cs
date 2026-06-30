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

namespace ZAS_ERP.ToDoTasks.TargetRewardss
{
    /// <summary>
    /// Interaction logic for ucRewardStatusList.xaml
    /// </summary>
    public partial class ucRewardStatusList : UserControl
    {
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        public ucRewardStatusList()
        {
            InitializeComponent();
        }

        private void GrdStatus_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }

        private void BtnNewStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Target Reward Status") != null)
            {
                Window win = new Window();
                ucFrmRewardStatus frm = new ucFrmRewardStatus();

                frm.saveEditFlag = false;

                win.Content = frm;
                win.Width = 400;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.ShowDialog();
                //RefreshData();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Add Target Reward Status!");
                return;
            }
        }

        private void BtnEditStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Target Reward Status") != null)
                {
                    Window win = new Window();
                    ucFrmRewardStatus frm = new ucFrmRewardStatus();

                    var selectedRow = grdStatus.SelectedItem as TargetRewardStatus;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.saveEditFlag = true;
                        frm.status = taskRepo.GetTargetRewardStatus(selectedRow.Id);

                        //frm.txtStatus.Text = selectedRow.Status;
                        //frm.chkisActive.IsChecked = selectedRow.isActive;
                        //frm.cpStatus.Color = (Color)color;

                        win.Content = frm;
                        win.Width = 400;
                        win.Height = 250;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.ResizeMode = ResizeMode.CanMinimize;
                        win.ShowDialog();
                    }
                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to Edit Target Reward Status!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = taskRepo.GetAllTargetRewardStatuses();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = taskRepo.GetAllTargetRewardStatuses();
        }
    }
}
