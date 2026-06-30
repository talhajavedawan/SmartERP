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
    /// Interaction logic for ucBankList.xaml
    /// </summary>
    public partial class ucBankList : UserControl
    {
        SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
        ucFrmAddBank frmBankAdd = new ucFrmAddBank();
        public ucBankList()
        {
            InitializeComponent();
        }

        
        private void MbtnAddBank_Click(object sender, RoutedEventArgs e)
        {
            frmBankAdd = new ucFrmAddBank();
                    
            Window win = new Window();

            win.ResizeMode = ResizeMode.CanMinimize;
            win.Height = 250;
            win.Width = 350;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmBankAdd.editFlag = false;
            win.Content = frmBankAdd;
            win.ShowDialog();
        }

        private void MbtnEditBank_Click(object sender, RoutedEventArgs e)
        {
            receiptRepo = new SalesReceiptRepo();
            Window win = new Window();
            var selectedRow = grdCntrlBankList.SelectedItem as MainBank;
            if (selectedRow != null)
            {
                frmBankAdd = new ucFrmAddBank();
                frmBankAdd.bankId = selectedRow.Id;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Height = 250;
                win.Width = 350;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmBankAdd.editFlag = true;
                win.Content = frmBankAdd;
                win.ShowDialog();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            receiptRepo = new SalesReceiptRepo();
            grdCntrlBankList.ItemsSource = receiptRepo.GetAllBanks();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            receiptRepo = new SalesReceiptRepo();
            grdCntrlBankList.ItemsSource = receiptRepo.GetAllBanks();
        }
    }
}
