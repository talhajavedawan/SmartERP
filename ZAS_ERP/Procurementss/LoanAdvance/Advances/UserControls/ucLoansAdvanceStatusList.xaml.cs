using ERP_BL.Procurements.LoansAdvances;
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

namespace ZAS_ERP.Procurementss.LoanAdvance.UserControls
{
    /// <summary>
    /// Interaction logic for ucLoansAdvanceStatusList.xaml
    /// </summary>
    public partial class ucLoansAdvanceStatusList : UserControl
    {
        AdvanceRepo repo = new AdvanceRepo();
        public ucLoansAdvanceStatusList()
        {
            InitializeComponent();
        }

        private void BtnNewLoansAdvanceStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances Status") != null)
            {
                Window win = new Window();
                ucFrmLoansAdvanceStatus frm = new ucFrmLoansAdvanceStatus();

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
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add Loans Advance status!");
                return;
            }
        }

        private void BtnEditLoansAdvanceStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Loans and Advances Status") != null)
                {
                    Window win = new Window();
                    ucFrmLoansAdvanceStatus frm = new ucFrmLoansAdvanceStatus();

                    var selectedRow = grdStatus.SelectedItem as LoansAdvanceStatus;
                    if (selectedRow != null)
                    {
                        //object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.saveEditFlag = true;
                        frm.status = repo.GetLoansAdvanceStatus(selectedRow.Id);

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
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Loans Advance status!");
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
            grdStatus.ItemsSource = repo.GetAllloansAdvanceStatuses();
        }

        private void GrdStatus_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = repo.GetAllloansAdvanceStatuses();
        }
    }
}
