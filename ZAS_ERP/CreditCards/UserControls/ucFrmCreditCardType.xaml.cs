using DevExpress.Xpf.Core;
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
    /// Interaction logic for ucFrmCreditCardTyoe.xaml
    /// </summary>
    public partial class ucFrmCreditCardType : UserControl
    {
        public bool editFlag = false;
        public CreditCardType cardType = new CreditCardType();
        CreditCardRepo cardRepo = new CreditCardRepo();

        public ucFrmCreditCardType()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(editFlag == true)
            {
                txtType.Text = cardType.Type;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtType.Text))
            {
                DXMessageBox.Show("Please enter Type!");
                txtType.Focus();
                return;
            }

            cardType.Type = txtType.Text;

            if(editFlag == false)
            {
                cardRepo.addCreditCardType(cardType);
                DXMessageBox.Show("Successfully Added!");
            }
            else if(editFlag == true)
            {
                cardRepo.updateCreditCardType(cardType);
                DXMessageBox.Show("Successfully Added!");
            }

            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }
    }
}
