using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucBillStatusList.xaml
    /// </summary>
    public partial class ucBillStatusList : UserControl
    {
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        public ucBillStatusList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = billsRepo.GetAllBillStatuses();
        }

        private void BtnNewBillStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer Status") != null)
            {
                ucFrmBillStatus frm = new ucFrmBillStatus();

                frm.saveEditFlag = false;

                frm.FrmBillStatusWin.Content = frm;
                frm.FrmBillStatusWin.Width = 400;
                frm.FrmBillStatusWin.Height = 250;
                frm.FrmBillStatusWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.FrmBillStatusWin.ResizeMode = ResizeMode.CanMinimize;
                frm.FrmBillStatusWin.ShowDialog();
                //RefreshData();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add Inter-Bank Transfer status!");
                return;
            }
        }

        private void BtnEditBillStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inter-Bank Transfer Status") != null)
                {
                    ucFrmBillStatus frm = new ucFrmBillStatus();

                    var selectedRow = grdStatus.SelectedItem as AdminBillStatus;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.saveEditFlag = true;
                        frm.status = billsRepo.GetBillStatus(selectedRow.Id);

                        //frm.txtStatus.Text = selectedRow.Status;
                        //frm.chkisActive.IsChecked = selectedRow.isActive;
                        //frm.cpStatus.Color = (Color)color;

                        frm.FrmBillStatusWin.Content = frm;
                        frm.FrmBillStatusWin.Width = 400;
                        frm.FrmBillStatusWin.Height = 250;
                        frm.FrmBillStatusWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frm.FrmBillStatusWin.ResizeMode = ResizeMode.CanMinimize;
                        frm.FrmBillStatusWin.ShowDialog();
                        //RefreshData();

                    }


                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Inter-Bank Transfer status!");
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GrdStatus_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = billsRepo.GetAllBillStatuses();
        }
    }
}
