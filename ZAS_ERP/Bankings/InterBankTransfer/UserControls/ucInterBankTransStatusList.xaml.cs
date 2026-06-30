using ERP_BL.Databases;
using ERP_BL.Procurements;
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

namespace ZAS_ERP.Bankings.UserControls
{
    /// <summary>
    /// Interaction logic for ucInterBankTransStatusList.xaml
    /// </summary>
    public partial class ucInterBankTransStatusList : UserControl
    {
        InterBankTransRepo bankTransRepo = new InterBankTransRepo();
        public Window interBankTransStatusWin = new Window();

        public ucInterBankTransStatusList()
        {
            InitializeComponent();
        }

        private void BtnNewStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Inter-Bank Transfer Status") != null)
            {
                ucFrmInterBankTransStatus frm = new ucFrmInterBankTransStatus();

                frm.saveEditFlag = false;

                frm.FrmBankTransStatusWin.Content = frm;
                frm.FrmBankTransStatusWin.Width = 400;
                frm.FrmBankTransStatusWin.Height = 250;
                frm.FrmBankTransStatusWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frm.FrmBankTransStatusWin.ResizeMode = ResizeMode.CanMinimize;
                frm.FrmBankTransStatusWin.ShowDialog();
                RefreshData();
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add Inter-Bank Transfer status!");
                return;
            }
                

        }

        private void BtnEditStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Inter-Bank Transfer Status") != null)
                {
                    ucFrmInterBankTransStatus frm = new ucFrmInterBankTransStatus();

                    var selectedRow = grdStatus.SelectedItem as InterBankTransferStatus;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.saveEditFlag = true;
                        frm.status = bankTransRepo.GetInterBankTransStatus(selectedRow.Id);

                        frm.txtStatus.Text = selectedRow.Status;
                        frm.chkisActive.IsChecked = selectedRow.isActive;
                        frm.cpStatus.Color = (Color)color;

                        frm.FrmBankTransStatusWin.Content = frm;
                        frm.FrmBankTransStatusWin.Width = 400;
                        frm.FrmBankTransStatusWin.Height = 250;
                        frm.FrmBankTransStatusWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        frm.FrmBankTransStatusWin.ResizeMode = ResizeMode.CanMinimize;
                        frm.FrmBankTransStatusWin.ShowDialog();
                        RefreshData();

                    }


                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Inter-Bank Transfer status!");
                    return;
                }
            
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = bankTransRepo.GetAllInterBankTransStatus();
            grdStatus.Columns["Id"].Visible = false;
        }

        private void RefreshData()
        {
            grdStatus.ItemsSource = bankTransRepo.GetAllInterBankTransStatus();
            grdStatus.Columns["Id"].Visible = false;
        }

        private void SetColumnsVisibility()
        {
            grdStatus.Columns["Id"].Visible = false;
        }
    }
}
