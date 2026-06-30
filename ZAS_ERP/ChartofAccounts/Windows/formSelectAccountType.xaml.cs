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
using System.Windows.Shapes;
using ZAS_ERP.ChartofAccounts.UserControls;

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for formAccountAdd.xaml
    /// </summary>
    public partial class formSelectAccountType : DXWindow
    {
        public ucChartofAccountList myParent = null;
        public formSelectAccountType()
        {
            InitializeComponent();
        }
        private void BtnContinue_Click(object sender, RoutedEventArgs e)
        { 

            if(radioIncome.IsChecked==true)
            {
                formAccountName addNameForm = new formAccountName(radioIncome.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else 
                if(radioExpense.IsChecked==true)
            {
                formAccountName addNameForm = new formAccountName(radioExpense.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();  
            }
            else
                if (radioFixedAssets.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioFixedAssets.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioBank.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioBank.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioLoan.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioLoan.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioCreditCard.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioCreditCard.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioEquity.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioEquity.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioAccountReceivable.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioAccountReceivable.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioOtherCurrentAsset.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioOtherCurrentAsset.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioOtherAsset.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioOtherAsset.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioAccountsPayable.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioAccountsPayable.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioOtherCurrentLiability.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioOtherCurrentLiability.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioLongTermLiability.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioLongTermLiability.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioCostOfGoodsSold.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioCostOfGoodsSold.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioOtherIncome.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioOtherIncome.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                if (radioOtherExpense.IsChecked == true)
            {
                formAccountName addNameForm = new formAccountName(radioOtherExpense.Content.ToString());
                addNameForm.myParent = myParent;
                addNameForm.Show();
                this.Close();
            }
            else
                DXMessageBox.Show("Please select account type to continue", "Information!", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
