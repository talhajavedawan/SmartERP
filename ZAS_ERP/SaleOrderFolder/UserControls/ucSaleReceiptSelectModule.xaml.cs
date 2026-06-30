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
using ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits;
using ZAS_ERP.SaleOrderFolder.UserControls.DirectSaleReceipt;
using ZAS_ERP.SaleOrderFolder.UserControls.LoansAdvanceSaleReceipt;

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucSaleReceiptSelectModule.xaml
    /// </summary>
    public partial class ucSaleReceiptSelectModule : UserControl
    {
        public ucSaleReceiptSelectModule()
        {
            InitializeComponent();
        }

        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxModules.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Module!");
                cmbxModules.Focus();
                return;
            }

            if (cmbxModules.SelectedIndex == 0)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Receipt") != null)
                {
                    try
                    {
                        ucFrmSaleReceipt enterSaleReceiptObj = new ucFrmSaleReceipt();
                        enterSaleReceiptObj.enter_receipt_win.Content = enterSaleReceiptObj;
                        enterSaleReceiptObj.enter_receipt_win.Title = "Sale Receipt";
                        enterSaleReceiptObj.enter_receipt_win.ResizeMode = ResizeMode.CanResize;
                        enterSaleReceiptObj.enter_receipt_win.WindowState = WindowState.Maximized;
                        enterSaleReceiptObj.enter_receipt_win.Show();

                        enterSaleReceiptObj.enterReceiptWindowFlag = true;
                    }
                    catch
                    {

                    }
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Sale Receipt!");
                }
            }
            else if (cmbxModules.SelectedIndex == 1)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances Sale Receipts") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmLoansAdvanceSaleReceiptAdd frmLAsaleReceipts = new ucFrmLoansAdvanceSaleReceiptAdd();

                    enterPaymentWin.Content = frmLAsaleReceipts;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Sale Receipts!");
                }

            }
            else if (cmbxModules.SelectedIndex == 2)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Direct Receipts") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmDirectSaleReceipt frmDirectReceipts = new ucFrmDirectSaleReceipt();

                    enterPaymentWin.Content = frmDirectReceipts;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Direct Receipts!");
                }
            }
            else if (cmbxModules.SelectedIndex == 3)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Customer Credit Receipts") != null)
                {
                    Window enterPaymentWin = new Window();
                    ucFrmCustomerCreditReceipt frmDirectReceipts = new ucFrmCustomerCreditReceipt();

                    enterPaymentWin.Content = frmDirectReceipts;
                    enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    enterPaymentWin.WindowState = WindowState.Maximized;
                    enterPaymentWin.Show();

                    var myWindow = Window.GetWindow(this);
                    myWindow.Close();
                }
                else
                {
                    DXMessageBox.Show("Permission required to Add new Customer Credit Receipts!");
                }

            }


        }

        string[] modules = { "Sale Invoices", "Loans Advances", "Direct Receipt", "Customer Credits" };
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < modules.Length; i++)
            {
                cmbxModules.Items.Add(modules[i]);
            }
        }
    }
}
