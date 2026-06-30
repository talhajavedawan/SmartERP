using ERP_BL.Databases;
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

namespace ZAS_ERP.Commentss
{
    /// <summary>
    /// Interaction logic for ucNotificationFlagList.xaml
    /// </summary>
    public partial class ucNotificationFlagList : UserControl
    {
        NotificationsRepo notificationRepo = new NotificationsRepo();
        public Window notificationFlagWin = new Window();

        public ucNotificationFlagList()
        {
            InitializeComponent();
        }

       

        private void BtnNewFlag_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Notifications Flag") != null)
            {
                ucFrmNotificationFlagAdd frm = new ucFrmNotificationFlagAdd();

                frm.saveEditFlag = false;

                frm.FrmNotificationFlagWin.Content = frm;
                frm.FrmNotificationFlagWin.Width = 400;
                frm.FrmNotificationFlagWin.Height = 350;
                frm.FrmNotificationFlagWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.FrmNotificationFlagWin.ResizeMode = ResizeMode.CanMinimize;
                frm.FrmNotificationFlagWin.ShowDialog();
                RefreshData();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add Notification Flags!");
                return;
            }
        }

        private void BtnEditFlag_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Notifications Flag") != null)
                {
                    ucFrmNotificationFlagAdd frm = new ucFrmNotificationFlagAdd();

                    var selectedRow = grdFlag.SelectedItem as NotificationFlag;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.saveEditFlag = true;
                        frm.flag = notificationRepo.GetNotificationFlag(selectedRow.Id);

                        frm.txtFlag.Text = selectedRow.Flag;
                        frm.chkisActive.IsChecked = selectedRow.isActive;
                        frm.cpFlag.Color = (Color)color;
                        frm.chkCanGlow.IsChecked = selectedRow.canGlow;

                        frm.FrmNotificationFlagWin.Content = frm;
                        frm.FrmNotificationFlagWin.Width = 400;
                        frm.FrmNotificationFlagWin.Height = 350;
                        frm.FrmNotificationFlagWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frm.FrmNotificationFlagWin.ResizeMode = ResizeMode.CanMinimize;
                        frm.FrmNotificationFlagWin.ShowDialog();
                        RefreshData();

                    }


                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Notification Flags!");
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdFlag.ItemsSource = notificationRepo.GetAllNotificationFlags();
            grdFlag.Columns["Id"].Visible = false;
        }

        private void RefreshData()
        {
            notificationRepo = new NotificationsRepo();
            grdFlag.ItemsSource = notificationRepo.GetAllNotificationFlags();
            grdFlag.Columns["Id"].Visible = false;
        }
    }
}
