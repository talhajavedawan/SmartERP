using ERP_BL.FilesAndDocs;
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

namespace ZAS_ERP.FilesAndDocss.TravellingRecords.UserControls
{
    /// <summary>
    /// Interaction logic for ucTravelingStatusList.xaml
    /// </summary>
    public partial class ucTravelingStatusList : UserControl
    {

        VisitingRecordRepo visitingRecordRepo = new VisitingRecordRepo();
        public ucTravelingStatusList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = visitingRecordRepo.GetAllTravelingStatuses();
        }

        private void BtnNewStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Traveling Status") != null)
            {
                ucFrmTravelingStatus frm = new ucFrmTravelingStatus();

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
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add Traveling status!");
                return;
            }
        }

        private void BtnEditStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Traveling Status") != null)
                {
                    ucFrmTravelingStatus frm = new ucFrmTravelingStatus();

                    var selectedRow = grdStatus.SelectedItem as TravelingStatus;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.saveEditFlag = true;
                        frm.status = visitingRecordRepo.GetTravelingStatus(selectedRow.Id);

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
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Traveling status!");
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
            grdStatus.ItemsSource = visitingRecordRepo.GetAllTravelingStatuses();
        }
    }
}
