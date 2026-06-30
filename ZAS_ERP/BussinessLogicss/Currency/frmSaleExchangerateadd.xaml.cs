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
    /// Interaction logic for frmSalesExchangeRateadd.xaml
    /// </summary>
    public partial class frmSalesExchangeRateadd : Window
    {
        CurrencyRepo repo = new CurrencyRepo();
        List<SalesExchangeRate> exchangerates = new List<SalesExchangeRate>();
        SalesExchangeRate exchangerate = new SalesExchangeRate();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Currency Basecurrency = new Currency();
        Currency targetcurrency = new Currency();
        public static int exchangerateId;

        public frmSalesExchangeRateadd()
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
            var usernew = employeeRepo.getuser(MainWindow.currentUserid);
            ERP_BL.Databases.Employee empUser = new ERP_BL.Databases.Employee();
            empUser= employeeRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = empUser.Companies;
            //List<ERP_BL.Databases.Company> companylist = new List<ERP_BL.Databases.Company>();
            //    companylist = cont.GetCompanies();
            //    List<ERP_BL.Databases.Company> companies = new List<ERP_BL.Databases.Company>();
            //    foreach (ERP_BL.Databases.Company comp in companylist)
            //        if (comp.compnayType == ERP_BL.Enums.CompnayTypes.Company)
            //            companies.Add(comp);
            //    this.lookupCompany.ItemsSource = companies;
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
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
            // cmbCurrency.DisplayMemberPath = (cmbCurrency.SelectedItem as cmbitem).name;
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

                //cmbCurrency.Items.Add(new cmbitem() { name = cur.CurrencyName, id = cur.Id });
                cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            cmbCurrency.ItemsSource = cmbitems;
            if (cmbbaseCurrency.ItemsSource == null)
                cmbbaseCurrency.ItemsSource = cmbitems;

        }



        private void btnSaveSalesExchangeRate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtexchangerate.Text == "")
                {
                    MessageBox.Show("Enter Sales Exchange Rate");
                    txtexchangerate.Focus();
                    return;
                }
                else if (company.Id==0||company==null)
                {
                    MessageBox.Show("Select company first");
                    lookupCompany.Focus();
                    return;

                }
                
                else if (cmbbaseCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Base currency");
                    cmbbaseCurrency.Focus();
                    return;

                }
                else if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Target currency");
                    cmbCurrency.Focus();
                    return;

                }
                
                else //if (txtItemName.Text != "" && txtItemDiscription.Text != "" && txtItemCode.Text != "" && cmbItemNature.Text != "" && cmbUnitOfMeasure.Text != "")
                {
                    
                    exchangerate.exchangerate = Convert.ToDecimal(txtexchangerate.Text.Trim());
                    exchangerate.company_Id = company.Id;
                    exchangerate.targetYear =Convert.ToInt32( datEffectiveFrom.Value);
                    exchangerate.target_currency_Id = (cmbCurrency.SelectedItem as cmbitem).id;
                    exchangerate.base_currency_Id = (cmbbaseCurrency.SelectedItem as cmbitem).id;



                    if (exchangerate.Id == 0)
                    {
                        if (MainWindow.currentUserid != 0)
                            exchangerate.Addedbyuser_Id = MainWindow.currentUserid;
                        exchangerate.AddedOn = System.DateTime.Now;
                        repo.Add(exchangerate);

                        MessageBox.Show( "Sales Exchange Rate Added Succesfully!");
                        this.Close();

                    }
                    else
                    {
                        if (MainWindow.currentUserid != 0)
                            exchangerate.Editedbyuser_Id = MainWindow.currentUserid;
                        exchangerate.LastUpdated = System.DateTime.Now;
                        repo.update(exchangerate);
                        
                        MessageBox.Show("Sales Exchange rate Updated Succesfully!");
                        this.Close();

                    }

                }
            }
            catch(Exception ex)
            {
                SystemLog.LogError(this.GetType(), "Sales Exchange Rate add Error" + ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void winSalesExchangeRateadd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            exchangerateId = 0;
        }
       
        private void winSalesExchangeRateadd_Loaded(object sender, RoutedEventArgs e)
        {
            loadcompanies();
            loadCurrencies();
            
            loaditeminfo();
        }

        public void loaditeminfo()
        {
            if (exchangerateId != 0)
            {
                exchangerate = repo.getsalesExchangeRate(exchangerateId);
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
                datEffectiveFrom.Value=exchangerate.targetYear;

                //chkIsActive.IsChecked = exchangerate.isActive;


            }
        }
    }
}
