using DevExpress.Xpf.Core;
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
    /// Interaction logic for ucPayeeList.xaml
    /// </summary>
    public partial class ucPayeeList : UserControl
    {
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        public ucPayeeList()
        {
            InitializeComponent();
        }

        private void MbtnAddPayee_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payee") != null)
            {
                ucFrmPayeeName frmPayeeName = new ucFrmPayeeName();
                frmPayeeName.addPayeeWindow.ResizeMode = ResizeMode.CanMinimize;
                frmPayeeName.addPayeeWindow.Height = 550;
                frmPayeeName.addPayeeWindow.Width = 600;
                frmPayeeName.addPayeeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmPayeeName.editFlag = false;
                frmPayeeName.addPayeeWindow.Content = frmPayeeName;
                frmPayeeName.addPayeeWindow.ShowDialog();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Payee!");
            }

            
        }

        private void MbtnEditPayee_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Payee") != null)
            {
                var selectedRow = (Payee)grdCntrlPayee.SelectedItem;

                if (selectedRow != null)
                {
                    ucFrmPayeeName frmPayeeName = new ucFrmPayeeName();
                    frmPayeeName.addPayeeWindow.ResizeMode = ResizeMode.CanMinimize;
                    frmPayeeName.addPayeeWindow.Height = 550;
                    frmPayeeName.addPayeeWindow.Width = 600;
                    frmPayeeName.addPayeeWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    frmPayeeName.editFlag = true;
                    frmPayeeName.payee = billsRepo.GetPayee(selectedRow.Id);
                    frmPayeeName.addPayeeWindow.Content = frmPayeeName;
                    frmPayeeName.addPayeeWindow.ShowDialog();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Edit Payee!");
            }

            
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            billsRepo = new AdminBillsRepo();
            grdCntrlPayee.ItemsSource = billsRepo.GetAllPayees();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billsRepo = new AdminBillsRepo();
            grdCntrlPayee.ItemsSource = billsRepo.GetAllPayees();
        }
    }
}
