using DevExpress.Xpf.Core;
using ERP_BL.Databases;
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

namespace ZAS_ERP.SaleOrderFolder.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmAddBank.xaml
    /// </summary>
    public partial class ucFrmAddBank : UserControl
    {
        SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
        MainBank bank = new MainBank();
        public int bankId = 0;
        public bool editFlag = false;

        public ucFrmAddBank()
        {
            InitializeComponent();
        }

        public void populatefIelds()
        {
            if (editFlag == true && bankId > 0)
            {
                bank = receiptRepo.GetMainBank(bankId);
                txtBankName.Text = bank.BankName;

                if (bank.isActive == true)
                    chkIsActive.IsChecked = true;
                else
                    chkIsActive.IsChecked = false;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtBankName.Text))
            {
                DXMessageBox.Show("Please enter Bank Name!");
                txtBankName.Focus();
                return;
            }

            bank.BankName = txtBankName.Text;
            if (chkIsActive.IsChecked == true)
                bank.isActive = true;
            else
                bank.isActive = false;


            if (editFlag == false && bankId == 0)
            {
                receiptRepo.AddMainBank(bank);
                DXMessageBox.Show("Successfully Added!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (editFlag == true && bankId != 0)
            {
                receiptRepo.UpdateMainBank(bank);
                DXMessageBox.Show("Updated Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            populatefIelds();
        }
    }
}
