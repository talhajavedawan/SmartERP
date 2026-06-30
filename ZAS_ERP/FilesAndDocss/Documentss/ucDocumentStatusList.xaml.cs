using ERP_BL.Documents;
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

namespace ZAS_ERP.FilesAndDocss.Documentss
{
    /// <summary>
    /// Interaction logic for ucDocumentStatusList.xaml
    /// </summary>
    public partial class ucDocumentStatusList : UserControl
    {
        DocumentRepo documentRepo = new DocumentRepo();
        public ucDocumentStatusList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = documentRepo.GetAllDocumentStatuses();
        }

        private void BtnNewStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Document Status") != null)
            {
                ucFrmDocumentStatus frm = new ucFrmDocumentStatus();

                frm.saveEditFlag = false;

                Window win = new Window();
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
                DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to add Document status!");
                return;
            }
        }

        private void BtnEditStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Document Status") != null)
                {
                    ucFrmDocumentStatus frm = new ucFrmDocumentStatus();

                    var selectedRow = grdStatus.SelectedItem as DocumentStatus;
                    if (selectedRow != null)
                    {
                        object color = ColorConverter.ConvertFromString(selectedRow.backcolor);

                        frm.saveEditFlag = true;
                        frm.status = documentRepo.GetDocumentStatus(selectedRow.Id);

                        //frm.txtStatus.Text = selectedRow.Status;
                        //frm.chkisActive.IsChecked = selectedRow.isActive;
                        //frm.cpStatus.Color = (Color)color;
                        Window win = new Window();

                        win.Content = frm;
                        win.Width = 400;
                        win.Height = 250;
                        win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        win.ResizeMode = ResizeMode.CanMinimize;
                        win.ShowDialog();
                        //RefreshData();

                    }


                }
                else
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("Permission required to edit Document status!");
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
            grdStatus.ItemsSource = documentRepo.GetAllDocumentStatuses();
        }

    }
}
