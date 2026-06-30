using ERP_BL.Payments;
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

namespace ZAS_ERP.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucPaymentStatusList.xaml
    /// </summary>
    public partial class ucPaymentStatusList : UserControl
    {
        PaymentRepo paymentsRepo = new PaymentRepo();
        public ucPaymentStatusList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = paymentsRepo.GetAllPaymentStatuses();
        }

        private void BtnNewPaymentStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment Status") != null)
            {
                ucFrmPaymentStatus frm = new ucFrmPaymentStatus();

                frm.saveEditFlag = false;

                frm.frmPaymentStatusWin.Content = frm;
                frm.frmPaymentStatusWin.Width = 400;
                frm.frmPaymentStatusWin.Height = 250;
                frm.frmPaymentStatusWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.frmPaymentStatusWin.ResizeMode = ResizeMode.CanMinimize;
                frm.frmPaymentStatusWin.ShowDialog();
                //RefreshData();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add Payment status!");
                return;
            }
        }

        private void BtnEditPaymentStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Payment Status") != null)
                {
                    ucFrmPaymentStatus frm = new ucFrmPaymentStatus();

                    var selectedRow = grdStatus.SelectedItem as PaymentStatus;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.saveEditFlag = true;
                        frm.status = paymentsRepo.GetPaymentStatus(selectedRow.Id);

                        //frm.txtStatus.Text = selectedRow.Status;
                        //frm.chkisActive.IsChecked = selectedRow.isActive;
                        //frm.cpStatus.Color = (Color)color;

                        frm.frmPaymentStatusWin.Content = frm;
                        frm.frmPaymentStatusWin.Width = 400;
                        frm.frmPaymentStatusWin.Height = 250;
                        frm.frmPaymentStatusWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frm.frmPaymentStatusWin.ResizeMode = ResizeMode.CanMinimize;
                        frm.frmPaymentStatusWin.ShowDialog();
                        //RefreshData();

                    }


                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Payment status!");
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
            grdStatus.ItemsSource = paymentsRepo.GetAllPaymentStatuses();
        }
    }
}
