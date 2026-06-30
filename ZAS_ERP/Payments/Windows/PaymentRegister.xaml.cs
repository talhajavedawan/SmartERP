using DevExpress.Xpf.Core;
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
using System.Windows.Shapes;
using ZAS_ERP.Payments.UserControls;
using ZAS_ERP.Procurementss.Payments.UserControls;

namespace ZAS_ERP.Payments.Windows
{
    /// <summary>
    /// Interaction logic for PaymentRegister.xaml
    /// </summary>
    public partial class PaymentRegister : Window
    {

        ucFrmPayments frmPaymentAdd = new ucFrmPayments();

        List<Payment> paymentList = new List<Payment>();
        List<cmbitem> TreeItems = new List<cmbitem>();
        PaymentRepo paymentsRepo = new PaymentRepo();

        ucPaymentList paymentRegister = new ucPaymentList();
        public string transctions;

        public PaymentRegister()
        {
            InitializeComponent();

           
        }

        private void AddPaymentBarItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Payment") != null)
            {
                Window moduleWin = new Window();

                ucFrmModuleSelect frmModuleSelect = new ucFrmModuleSelect();

                moduleWin.Content = frmModuleSelect;

                moduleWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                moduleWin.Width = 400;
                moduleWin.Height = 250;
                moduleWin.ResizeMode = ResizeMode.CanMinimize;
                moduleWin.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Payment!");
            }
        }

        private void GrdCollapse_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Collapsed;
            GridSplitter.Visibility = Visibility.Collapsed;
            faRightGrid.SetValue(Grid.ColumnProperty, 0);

            faRightGrid.SetValue(Grid.ColumnSpanProperty, 3);
        }

        private void GrdExpand_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            faLeftGrid.Visibility = Visibility.Visible;
            GridSplitter.Visibility = Visibility.Visible;

            faRightGrid.SetValue(Grid.ColumnProperty, 2);
            faRightGrid.SetValue(Grid.ColumnSpanProperty, 1);   
        }

        private void TreeViewPaymentStatus_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            paymentList = new List<Payment>();
            paymentsRepo = new PaymentRepo();
            var item = (cmbitem)treeViewPaymentsStatus.SelectedItem;

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
            {
                if (item.name == "Payments")
                {
                    paymentList = paymentsRepo.GetAllTransactionsOpenAndClosed(MainWindow.currentUserid);
                }
                else if (item.name == "Payments(Open)")
                {
                    paymentList = paymentsRepo.getAllActiveandUnapprovedTransactions(MainWindow.currentUserid);
                }
                else if (item.name == "Payments(Closed)")
                {
                    paymentList = paymentsRepo.getAllInActiveandUnapprovedReceipts(MainWindow.currentUserid);
                }
                else
                {
                    paymentList = paymentsRepo.getAllSaleReceiptsbyStatusId(MainWindow.currentUserid, item.id);
                }


                paymentRegister.grdPaymentRegister.ItemsSource = paymentList;
                paymentRegister.GridControlSetUserSettings();
                //bankTransferRegister.SetColumnsVisibility();
                paymentRegister.lblHeading.Text = item.name;
            }
            else
            {
                paymentRegister.grdPaymentRegister.ItemsSource = null;
                paymentRegister.lblHeading.Text = item.name;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                paymentList = new List<Payment>();
                //paymentList = paymentsRepo.GetAllOpenPayments(SYSTEM_STATIC.currentUser.id);
                faRightGrid.Children.Add(paymentRegister);

                //if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "List of Payments") != null)
                //{
                //    paymentRegister.grdPaymentRegister.ItemsSource = paymentList;
                //}

                cmbitem treeItem1 = new cmbitem() { name = "Payments" };
                cmbitem treeItema = new cmbitem() { name = "Payments(Open)" };
                ICollection<cmbitem> cmbItemsa = new List<cmbitem>();
                foreach (PaymentStatus status in paymentsRepo.GetAllOpenPaymentStatus().Where(x => x.isActive == true).ToList())
                {
                    cmbItemsa.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Payments" });
                }
                treeItema.Items = cmbItemsa;
                treeItem1.Items.Add(treeItema);
                cmbitem treeItemb = new cmbitem() { name = "Payments(Closed)" };

                ICollection<cmbitem> cmbItems = new List<cmbitem>();
                foreach (PaymentStatus status in paymentsRepo.GetAllClosePaymentStatus().Where(x => x.isActive == false).ToList())
                {
                    cmbItems.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Payments" });
                }
                treeItemb.Items = cmbItems;
                treeItem1.Items.Add(treeItemb);

                //}


                TreeItems.Add(treeItem1);
                treeViewPaymentsStatus.ItemsSource = TreeItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
