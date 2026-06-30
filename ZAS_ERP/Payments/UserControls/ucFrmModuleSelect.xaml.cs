using DevExpress.Xpf.Core;
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
using ZAS_ERP.Payments.UserControls.LoansAdvancePayments;
using ZAS_ERP.Payments.UserControls.PurchaseInvoicePayments;
using ZAS_ERP.Payments.UserControls.TargetRewardPayments;
using ZAS_ERP.Payments.UserControls.VendorBillPayments;
using ZAS_ERP.Procurementss.Payments.UserControls;

namespace ZAS_ERP.Payments.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmModuleSelect.xaml
    /// </summary>
    public partial class ucFrmModuleSelect : UserControl
    {
        public ucFrmModuleSelect()
        {
            InitializeComponent();
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            if(cmbxModules.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Module!");
                cmbxModules.Focus();
                return;
            }

            if(cmbxModules.SelectedIndex == 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Admin Bill Payment") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmPayments frmBillPayments = new ucFrmPayments();

                    enterPaymentWin.Content = frmBillPayments;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Admin Bill Payment!");
                }
            }
            else if(cmbxModules.SelectedIndex == 1)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Bill Payment") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmBillPaymentAdd frmBillPayments = new ucFrmBillPaymentAdd();

                    enterPaymentWin.Content = frmBillPayments;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Vendor Bill Payment!");
                }
               
            }
            else if (cmbxModules.SelectedIndex == 2)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Purchase Invoice Payment") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmPInvoicePaymentAdd frmPIpaymentAdd = new ucFrmPInvoicePaymentAdd();

                    enterPaymentWin.Content = frmPIpaymentAdd;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Purchase Invoice Payment!");
                }
            }
            else if (cmbxModules.SelectedIndex == 3)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances Payment") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmLoansAdvancePaymentAdd frmLApaymentAdd = new ucFrmLoansAdvancePaymentAdd();

                    enterPaymentWin.Content = frmLApaymentAdd;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Loans and Advances Payment!");
                }
            }
            else if (cmbxModules.SelectedIndex == 4)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Target Rewards Payment") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmTargetRewardPayment frmTargetRewardPayment = new ucFrmTargetRewardPayment();

                    enterPaymentWin.Content = frmTargetRewardPayment;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add Target Rewards Payment!");
                }
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.PaymentTransactionType.Target_Reward; i++)
            {
                cmbxModules.Items.Add(((ERP_BL.Enums.PaymentTransactionType)i).ToString());
            }
        }
    }
}
