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
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP.BussinessLogicss
{
    /// <summary>
    /// Interaction logic for frmCurrencyAdd.xaml
    /// </summary>
    public partial class frmCurrencyAdd : Window
    {
        public frmCurrencyAdd()
        {
            InitializeComponent();
        }
        public static int currencyId;
        CurrencyRepo repo = new CurrencyRepo();
        Currency currency = new Currency();
        private void btnCurrencySave_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                currency.Country = txtCountry.Text.Trim();
                currency.CurrencyName = txtCurrencyName.Text.Trim();
                currency.Abbrivation = txtAbrivation.Text.Trim();
                currency.Symbol = txtSymbol.Text.Trim();
                if (currency.Id == 0)
                {
                    if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add  Currency") != null)
                    {
                        repo.Add(currency);
                        MessageBox.Show("New Currency (" + txtCurrencyName.Text + ") Added", "Congratulations");
                    }
                    else
                    {
                        MessageBox.Show("You don't have permission to Add Currency", "Warning");
                        return;
                    }
                }
                else
                {
                    repo.update(currency);

                    MessageBox.Show("Currency (" + txtCurrencyName.Text + ") updated", "Congratulations");
                }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winCurrencyAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            currencyId = 0;
        }

        private void winCurrencyAdd_Loaded(object sender, RoutedEventArgs e)
        {
            if (currencyId != 0)
            {
                currency = repo.get(currencyId);
                txtCurrencyName.Text = currency.CurrencyName;
                txtAbrivation.Text = currency.Abbrivation;
                txtCountry.Text = currency.Country;
                txtSymbol.Text = currency.Symbol;
                checkIsvoid.IsChecked = currency.isVoid == true ?  true : false;  
            }
        }

        private void CheckIsvoid_Checked(object sender, RoutedEventArgs e)
        {
            currency.isVoid = true;
        }

        private void CheckIsvoid_Unchecked(object sender, RoutedEventArgs e)
        {
            currency.isVoid = false;
        }
    }
}
