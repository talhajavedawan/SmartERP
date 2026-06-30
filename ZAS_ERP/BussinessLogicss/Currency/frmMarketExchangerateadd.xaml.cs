using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ZAS_ERP.BussinessLogicss
{
    /// <summary>
    /// Interaction logic for frmExchangeRateadd.xaml
    /// </summary>
    public partial class frmMarketExchangeRateadd : Window
    {
        CurrencyRepo repo = new CurrencyRepo();
        List<MarketExchangeRate> exchangerates = new List<MarketExchangeRate>();
        MarketExchangeRate exchangerate = new MarketExchangeRate();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Currency Basecurrency = new Currency();
        Currency targetcurrency = new Currency();
        public static int exchangerateId;

        public frmMarketExchangeRateadd()
        {
            InitializeComponent();
            
        }
       
        public void loadcompanies()
        {
            //try
            //{
            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();

                this.lookupCompany.ItemsSource = cont.GetCompanies();
                return;

            }
            EmployeeRepo employeeRepo = new EmployeeRepo();
            ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
            empUser= employeeRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
           
        }
        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company != null)
            {
                string selecteddept = company.CompanyName;
                
                foreach (cmbitem cmbitem in cmbCurrency.Items)
                {
                    if (cmbitem.id == company.CurrencyId)
                        cmbbaseCurrency.SelectedItem = cmbitem;
                }

                
            }

        }
        private void cmbCurrency_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            
            if (cmbCurrency.SelectedItem as cmbitem != null)
            {
                int idd = (cmbCurrency.SelectedItem as cmbitem).id;
                targetcurrency= repo.get(idd);
                if (idd == 0)
                {
                    BussinessLogicss.frmCurrencyAdd frmCurrency = new BussinessLogicss.frmCurrencyAdd();
                    frmCurrency.ShowDialog();
                    loadCurrencies();
                }
                
            }
        }
        private void loadCurrencies()
        {

            
            List<Currency> currencies = repo.getAll();
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (Currency cur in currencies)
            {

                
                cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbCurrency.ItemsSource = cmbitems;
            if (cmbbaseCurrency.ItemsSource == null)
                cmbbaseCurrency.ItemsSource = cmbitems;

        }



        private void btnSaveExchangeRate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtexchangerate.Text == "")
                {
                    DXMessageBox.Show("Enter Exchange Rate");
                    txtexchangerate.Focus();
                    return;
                }
                else if (company.Id==0||company==null)
                {
                    DXMessageBox.Show("Select company first");
                    lookupCompany.Focus();
                    return;

                }
                
                else if (cmbbaseCurrency.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Select Base currency");
                    cmbbaseCurrency.Focus();
                    return;

                }
                else if (cmbCurrency.SelectedIndex == -1)
                {
                    DXMessageBox.Show("Select Target currency");
                    cmbCurrency.Focus();
                    return;

                }
                
                else //if (txtItemName.Text != "" && txtItemDiscription.Text != "" && txtItemCode.Text != "" && cmbItemNature.Text != "" && cmbUnitOfMeasure.Text != "")
                {
                    
                    exchangerate.exchangerate = Convert.ToDecimal(txtexchangerate.Text.Trim());
                    exchangerate.company_Id = company.Id;
                    exchangerate.effectiveTo = datEffectiveTo.DateTime;
                    exchangerate.effectiveFrom = datEffectiveFrom.DateTime;
                    exchangerate.target_currency_Id = (cmbCurrency.SelectedItem as cmbitem).id;
                    exchangerate.base_currency_Id = (cmbbaseCurrency.SelectedItem as cmbitem).id;



                    if (exchangerate.Id == 0)
                    {
                        if (MainWindow.currentUserid != 0)
                            exchangerate.Addedbyuser_Id = MainWindow.currentUserid;
                        exchangerate.AddedOn = System.DateTime.Now;
                        repo.Add(exchangerate);

                        DXMessageBox.Show( "Exchange Rate Added Succesfully!");
                        this.Close();

                    }
                    else
                    {
                        repo.update(exchangerate);
                        if (MainWindow.currentUserid != 0)
                            exchangerate.Editedbyuser_Id = MainWindow.currentUserid;
                        exchangerate.LastUpdated = System.DateTime.Now;
                        DXMessageBox.Show(" Exchange rate Updated Succesfully!");
                        this.Close();

                    }

                }
            }
            catch(Exception ex)
            {
                SystemLog.LogError(this.GetType(), "Exchange Rate add Error" + ex.ToString());
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void winExchangeRateadd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            exchangerateId = 0;
        }
       
        private void winExchangeRateadd_Loaded(object sender, RoutedEventArgs e)
        {
            loadcompanies();
            loadCurrencies();
            
            loaditeminfo();
        }

        public void loaditeminfo()
        {
            if (exchangerateId != 0)
            {
                exchangerate = repo.getMarketexchangerate(exchangerateId);
                datEffectiveFrom.DateTime = exchangerate.effectiveFrom;
                datEffectiveTo.DateTime = exchangerate.effectiveTo;

                txtexchangerate.Text = exchangerate.exchangerate.ToString();
                //txtItemName.Text = exchangerate.item;
                if (exchangerate.company_Id != null || exchangerate.company != null)
                {
                    company = exchangerate.company;

                    lookupCompany.Text = exchangerate.company.CompanyName;
                    //lookupCompany.SelectedItem = lookupCompany.GetItemByKeyValue(offer.company);

                    //loaddepartments();
                }
                if (exchangerate.target_currency_Id != 0 && exchangerate.Target_Currency != null)
                    foreach (cmbitem cmbitem in cmbCurrency.Items)
                    {
                        if (cmbitem.id == exchangerate.target_currency_Id)
                        {
                            cmbCurrency.SelectedItem = cmbitem;
                            break;
                        }

                    }
                if (exchangerate.base_currency_Id != 0 && exchangerate.Base_Currency != null)
                    foreach (cmbitem cmbitem in cmbbaseCurrency.Items)
                    {
                        if (cmbitem.id == exchangerate.base_currency_Id)
                        {
                            cmbbaseCurrency.SelectedItem = cmbitem;
                            break;
                        }

                    }
                
                //chkIsActive.IsChecked = exchangerate.isActive;
                

            }
        }
    }
}
