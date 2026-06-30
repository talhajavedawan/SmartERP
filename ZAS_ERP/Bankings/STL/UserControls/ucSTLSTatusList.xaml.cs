using DevExpress.Xpf.Core;
using ERP_BL.Procurements.InterBankTransfers;
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

namespace ZAS_ERP.Bankings.STL
{
    /// <summary>
    /// Interaction logic for ucSTLSTatusList.xaml
    /// </summary>
    public partial class ucSTLSTatusList : UserControl
    {
        STLRepo stlRepo = new STLRepo();

        public ucSTLSTatusList()
        {
            InitializeComponent();
        }

        private void BtnAddNewStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add STL Status") != null)
            {
                ucfrmSTLStatus frm = new ucfrmSTLStatus();
                DXWindow win = new DXWindow();

                frm.editFlag = false;
                win.Content = frm;
                win.Width = 400;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.ShowDialog();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new status!");
                return;
            }
        }

        private void BtnEditStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit STL Status") != null)
                {
                    ucfrmSTLStatus frm = new ucfrmSTLStatus();
                    DXWindow win = new DXWindow();
                    var selectedRow = grdStatus.SelectedItem as STLStatus;

                    if (selectedRow != null)
                    {
                        frm.editFlag = true;
                        frm.statusId = selectedRow.Id;
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
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit STL status!");
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
            stlRepo = new STLRepo();
            grdStatus.ItemsSource = stlRepo.GetAllSTLStatuses();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = stlRepo.GetAllSTLStatuses();
        }

        private void GrdStatus_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }
    }
}
