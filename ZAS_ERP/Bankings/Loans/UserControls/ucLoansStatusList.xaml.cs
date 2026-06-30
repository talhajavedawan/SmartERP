using DevExpress.Xpf.Core;
using ERP_BL.Bankings;
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

namespace ZAS_ERP.Bankings.Loans.UserControls
{
    /// <summary>
    /// Interaction logic for ucLoansStatusList.xaml
    /// </summary>
    public partial class ucLoansStatusList : UserControl
    {
        LoansRepo loansRepo = new LoansRepo();

        public ucLoansStatusList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = loansRepo.GetAllLoansStatuses();
        }

        private void BtnAddNewStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans Status") != null)
            {
                ucFrmLoansStatus frm = new ucFrmLoansStatus();
                DXWindow win = new DXWindow();

                frm.editFlag = false;
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
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add new status!");
                return;
            }
        }

        private void BtnEditStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Loans Status") != null)
                {
                    ucFrmLoansStatus frm = new ucFrmLoansStatus();
                    DXWindow win = new DXWindow();
                    var selectedRow = grdStatus.SelectedItem as LoansStatus;

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
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Loans status!");
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
            loansRepo = new LoansRepo();
            grdStatus.ItemsSource = loansRepo.GetAllLoansStatuses();
        }

        private void GrdStatus_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }

        
    }
}
