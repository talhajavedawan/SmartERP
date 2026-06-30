using ERP_BL.CreditCards;
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

namespace ZAS_ERP.CreditCards.UserControls
{
    /// <summary>
    /// Interaction logic for ucCreditCardTypeList.xaml
    /// </summary>
    public partial class ucCreditCardTypeList : UserControl
    {
        CreditCardRepo cardRepo = new CreditCardRepo();
        ucFrmCreditCardType frmCreditCardType = new ucFrmCreditCardType();
        public ucCreditCardTypeList()
        {
            InitializeComponent();
        }

        private void MbtnAddNew_Click(object sender, RoutedEventArgs e)
        {
            frmCreditCardType = new ucFrmCreditCardType();
            frmCreditCardType.editFlag = false;
            Window window = new Window();
            window.Height = 320;
            window.Width = 400;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = frmCreditCardType;
            window.Show();
        }

        private void MbtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            frmCreditCardType = new ucFrmCreditCardType();

            if (grdCntrlCreditCardType.SelectedItem != null)
            {
                frmCreditCardType.editFlag = true;
                frmCreditCardType.cardType = grdCntrlCreditCardType.SelectedItem as CreditCardType;
            }
            Window window = new Window();
            window.Height = 320;
            window.Width = 400;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = frmCreditCardType;
            window.Show();
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            grdCntrlCreditCardType.ItemsSource = cardRepo.GetAllCreditCardTypes();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdCntrlCreditCardType.ItemsSource = cardRepo.GetAllCreditCardTypes();
        }
    }
}
