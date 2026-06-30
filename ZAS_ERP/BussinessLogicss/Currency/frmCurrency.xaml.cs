using DevExpress.Xpf.Grid;
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
using System.Windows.Shapes;
using ERP_BL;

namespace ZAS_ERP.BussinessLogicss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmCurrency : Window
    {



        CurrencyRepo repo = new CurrencyRepo();
        List<Currency> currencies = new List<Currency>();
        public frmCurrency()
        {
            InitializeComponent();
        }

        private void winCurrency_Loaded(object sender, RoutedEventArgs e)
        {
            loadCurrencygrid();
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCurrency);
        }

        private void loadCurrencygrid()
        {
            currencies = repo.getAll();
            this.grdCurrency.ItemsSource = currencies;
            //grdemployee.Columns.GetColumnByFieldName("EmpId").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("person").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("address").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("contact").Visible = false;
            ////grdemployee.Columns.GetColumnByFieldName("Companies").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Desig").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Disability").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("DisDescription").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("MaritalStatus").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Status").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("JoinDate").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("BasicPay").Visible = false;
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "EmpId" });
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.FName" });
            //grdemployee.Columns.GetColumnByFieldName("person.FName").Header = "First Name";
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.LName" });
            //grdemployee.Columns.GetColumnByFieldName("person.LName").Header = "Last Name";
            
        }

        private void btnaddCurrency_Click(object sender, RoutedEventArgs e)
        {
            if(txtCurrencyName.Text!="" || txtAbrivation.Text !="")
            {
                
                Currency currency = new Currency();
                currency.Country =txtCountry.Text.Trim();
                currency.CurrencyName = txtCurrencyName.Text.Trim();
                currency.Abbrivation = txtAbrivation.Text.Trim();
                currency.Symbol = txtSymbol.Text.Trim();
                repo.Add(currency);
                
                MessageBox.Show(txtCurrencyName.Text+" Added Succesfully!");
                loadCurrencygrid();
            }
        }

        private void WinCurrency_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCurrency);
        }
        //int empid;



    }
}
