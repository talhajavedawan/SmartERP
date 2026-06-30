using DevExpress.Xpf.Core;
using ERP_BL.VATBook;
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

namespace ZAS_ERP.VATBook.UserControls
{
    /// <summary>
    /// Interaction logic for ucVATBookReferenceList.xaml
    /// </summary>
    public partial class ucVATBookReferenceList : UserControl
    {
        VATBookRepo vatBookRepo = new VATBookRepo();

        public ucVATBookReferenceList()
        {
            InitializeComponent();
        }

        private void MbtnAddReference_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add VAT Book Reference") != null)
            {
                ucFrmVATReferenceNo frmBillReferenceNo = new ucFrmVATReferenceNo();
                frmBillReferenceNo.editFlag = false;
                Window win = new Window();
                win.Content = frmBillReferenceNo;
                win.Height = 350;
                win.Width = 400;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add VAT Book Reference!");
            }

        }

        private void MbtnEditReference_Click(object sender, RoutedEventArgs e)
        {

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit VAT Book Reference") != null)
            {
                var selectedRow = grdCntrlReferenceList.SelectedItem as VATBookRefNumber;

                if (selectedRow != null)
                {
                    ucFrmVATReferenceNo frmBillReferenceNo = new ucFrmVATReferenceNo();
                    frmBillReferenceNo.editFlag = true;
                    frmBillReferenceNo.vatBookRefNumbers = vatBookRepo.GetVATBookReferenceNo(selectedRow.Id);
                    Window win = new Window();
                    win.Content = frmBillReferenceNo;
                    win.Height = 350;
                    win.Width = 400;
                    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    win.ResizeMode = ResizeMode.CanMinimize;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Edit VAT Book Reference!");
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            vatBookRepo = new VATBookRepo();
            grdCntrlReferenceList.ItemsSource = vatBookRepo.GetAllVATBookRefNo();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            vatBookRepo = new VATBookRepo();
            grdCntrlReferenceList.ItemsSource = vatBookRepo.GetAllVATBookRefNo();
        }
    }

}
