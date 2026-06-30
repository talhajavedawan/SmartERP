using DevExpress.Xpf.Core;
using ERP_BL.Enums;
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
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.AdminBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoan;
using ZAS_ERP.Procurementss.LoanAdvance.UserControls.VendorBillLoansAdvance;

namespace ZAS_ERP.Procurementss.LoanAdvance.UserControls
{
    /// <summary>
    /// Interaction logic for ucSelectLoansAdvanceType.xaml
    /// </summary>
    public partial class ucSelectLoansAdvanceType : UserControl
    {
        public LoansAdvanceTemplate advanceTemplate = new LoansAdvanceTemplate();
        public ucSelectLoansAdvanceType()
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

            if(advanceTemplate == LoansAdvanceTemplate.Advance)
            {
                if (cmbxModules.SelectedIndex == 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances") != null)
                    {
                        Window enterLoansAdvance = new Window();
                        ucFrmLoansAdvances frmLoansAdvances = new ucFrmLoansAdvances();

                        enterLoansAdvance.Content = frmLoansAdvances;
                        enterLoansAdvance.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        enterLoansAdvance.WindowState = WindowState.Maximized;
                        enterLoansAdvance.Show();

                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to Add Admin Bill Loans Advance!");
                    }
                }
                else if (cmbxModules.SelectedIndex == 1)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Bill Loans Advances") != null)
                    {
                        Window enterPaymentWin = new Window();
                        ucFrmBillLoansAdvance frmBillPayments = new ucFrmBillLoansAdvance();

                        enterPaymentWin.Content = frmBillPayments;
                        enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        enterPaymentWin.WindowState = WindowState.Maximized;
                        enterPaymentWin.Show();

                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to Add Vendor Bill Loans Advance!");
                    }

                }
            }
            else if(advanceTemplate== LoansAdvanceTemplate.Loan)
            {
                if (cmbxModules.SelectedIndex == 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Loans and Advances") != null)
                    {
                        Window enterLoansAdvance = new Window();
                        ucFrmAdminBillLoan frmLoansAdvances = new ucFrmAdminBillLoan();

                        enterLoansAdvance.Content = frmLoansAdvances;
                        enterLoansAdvance.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        enterLoansAdvance.WindowState = WindowState.Maximized;
                        enterLoansAdvance.Show();

                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to Add Admin Bill Loans Advance!");
                    }
                }
                else if (cmbxModules.SelectedIndex == 1)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Vendor Bill Loans Advances") != null)
                    {
                        Window enterPaymentWin = new Window();
                        ucFrmVendorBillLoan frmBillPayments = new ucFrmVendorBillLoan();

                        enterPaymentWin.Content = frmBillPayments;
                        enterPaymentWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        enterPaymentWin.WindowState = WindowState.Maximized;
                        enterPaymentWin.Show();

                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();
                    }
                    else
                    {
                        DXMessageBox.Show("Permission required to Add Vendor Bill Loans Advance!");
                    }
                }
            }
            


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.LoansAdvanceType.Vendor_Bill; i++)
            {
                cmbxModules.Items.Add(((ERP_BL.Enums.LoansAdvanceType)i).ToString());
            }
        }
    }
}
