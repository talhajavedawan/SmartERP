using ERP_BL.ChartofAccounts;
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

namespace ZAS_ERP.ChartofAccounts.UserControls
{
    /// <summary>
    /// Interaction logic for ucJournalVoucherStatusList.xaml
    /// </summary>
    public partial class ucJournalVoucherStatusList : UserControl
    {
        JournalVoucherRepo journalVoucherRepo = new JournalVoucherRepo();
        public ucJournalVoucherStatusList()
        {
            InitializeComponent();
        }

        private void GrdStatus_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }

        private void BtnNewStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add JV Status") != null)
            {
                ucJournalVoucherStatusAdd frm = new ucJournalVoucherStatusAdd();
                Window win = new Window();

                frm.editFlag = false;

                win.Content = frm;
                win.Width = 400;
                win.Height = 200;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.ShowDialog();
                //RefreshData();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add JV status!");
                return;
            }
        }

        private void BtnEditStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit JV Status") != null)
                {
                    ucJournalVoucherStatusAdd frm = new ucJournalVoucherStatusAdd();
                    Window win = new Window();

                    var selectedRow = grdStatus.SelectedItem as JournalVoucherStatus;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.editFlag = true;
                        frm.statusId = selectedRow.Id;

                        //frm.txtStatus.Text = selectedRow.Status;
                        //frm.chkisActive.IsChecked = selectedRow.isActive;
                        //frm.cpStatus.Color = (Color)color;

                        win.Content = frm;
                        win.Width = 400;
                        win.Height = 200;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.ResizeMode = ResizeMode.CanMinimize;
                        win.ShowDialog();
                        //RefreshData();

                    }


                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit JV status!");
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
            journalVoucherRepo = new JournalVoucherRepo();
            grdStatus.ItemsSource = journalVoucherRepo.GetAllJournalVoucherStatuses();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = journalVoucherRepo.GetAllJournalVoucherStatuses();
        }
    }
}
