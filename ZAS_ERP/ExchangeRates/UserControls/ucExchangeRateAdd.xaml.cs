using DevExpress.Xpf.Core;
using DevExpress.Xpf.Ribbon;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ExchangeRates;
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

namespace ZAS_ERP.ExchangeRates.UserControls
{
    /// <summary>
    /// Interaction logic for ucExchangeRateAdd.xaml
    /// </summary>
    public partial class ucExchangeRateAdd : DXRibbonWindow
    {
        List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
        UsersRepo userRepo = new UsersRepo();
        public bool editFlag = false;
        ExchangeRateGroupRepo exchangeRateGroupRepo = new ExchangeRateGroupRepo();
        ExchangeRateGroup rateGroup = new ExchangeRateGroup();
        public int groupId = 0;
        public ucExchangeRateAdd()
        {
            InitializeComponent();
            grdExchangeRates.ItemsSource = exchangeRates;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            getAlllExchangeRateTypes();
            cmbBaseCurrencies.ItemsSource = SYSTEM_STATIC.currencySources;
            cmbTransCurrencies.ItemsSource = SYSTEM_STATIC.currencySources;
            lookupCompanyGrid.ItemsSource = SYSTEM_STATIC.LoadCurrentUserCompanies();
            if(editFlag!=false && groupId!=0)
            {
                rateGroup= exchangeRateGroupRepo.Get(groupId);
                cmbExchangeTypes.SelectedItem = rateGroup.ExchangeType.ToString();
                var baseCurrencySource = (List<cmbitem>)cmbBaseCurrencies.Items.SourceCollection;
                cmbBaseCurrencies.SelectedItem = cmbBaseCurrencies.Items[cmbBaseCurrencies.Items.IndexOf(baseCurrencySource.Find(x => x.id == rateGroup.base_currency_Id))];
                var transactionCurrencySource = (List<cmbitem>)cmbTransCurrencies.Items.SourceCollection;
                cmbTransCurrencies.SelectedItem = cmbTransCurrencies.Items[cmbTransCurrencies.Items.IndexOf(transactionCurrencySource.Find(x => x.id == rateGroup.transaction_currency_Id))];
                datEffectiveFrom.EditValue = rateGroup.TargetYear;
                grdExchangeRates.ItemsSource = rateGroup.exchangeRates;
                lblHeading.Text = "Update Exchange Rates";
                this.Title= "Update Exchange Rates";
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Exchange Rates Base Currency") == null)
                {
                    cmbBaseCurrencies.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Exchange Rates Transaction Currency") == null)
                {
                    cmbTransCurrencies.IsEnabled = false;
                 
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Exchange Rates Target Year") == null)
                {
                    datEffectiveFrom.IsEnabled = false;
                }
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Exchange Rates Type") == null)
                {
                    cmbExchangeTypes.IsEnabled = false;
                 
                }

            }
            else
            {
                btnVoid.Visibility = Visibility.Collapsed;
            }

        }
        public void getAlllExchangeRateTypes()
        {
            List<string> list = new List<string>();
            list.Add(ExchangeRateType.SER.ToString());
            list.Add(ExchangeRateType.MER.ToString());
            cmbExchangeTypes.ItemsSource = list;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               


                if (editFlag == false)
                {
                    ExchangeRateGroup group = new ExchangeRateGroup();
                    group.Addedbyuser_Id = SYSTEM_STATIC.currentUser.id;
                    if (cmbExchangeTypes.SelectedItem.ToString() == "MER")
                        group.ExchangeType = ExchangeRateType.MER;
                    else { group.ExchangeType = ExchangeRateType.SER; }

                    var dbGroup = exchangeRateGroupRepo.GetGroupByTypeTargetYear(group.ExchangeType, Convert.ToInt32(datEffectiveFrom.EditValue), (cmbBaseCurrencies.SelectedItem as cmbitem).id, (cmbTransCurrencies.SelectedItem as cmbitem).id);
                    if (dbGroup != null)
                    {
                        DXMessageBox.Show("Exchange Rate Already exists, Please change target year and exchange rate type ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    else
                    {
                        group.AddedOn = DateTime.Now;
                        group.base_currency_Id = (cmbBaseCurrencies.SelectedItem as cmbitem).id;
                        group.transaction_currency_Id = (cmbTransCurrencies.SelectedItem as cmbitem).id;
                        group.isVoid = false;
                        group.LastUpdated = DateTime.Now;
                        group.TargetYear = Convert.ToInt32(datEffectiveFrom.EditValue);
                        group.exchangeRates = getRates();
                        exchangeRateGroupRepo.Add(group);
                        DXMessageBox.Show("Exchange Rate Added Successfully ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                   
                }
                else
                {
                    ExchangeRateGroup group = new ExchangeRateGroup();
                    group = exchangeRateGroupRepo.Get(groupId);
                    if (group != null)
                    {
                        group.Addedbyuser_Id = group.Addedbyuser_Id;
                        if (cmbExchangeTypes.SelectedItem.ToString() == "MER")
                            group.ExchangeType = ExchangeRateType.MER;
                        else { group.ExchangeType = ExchangeRateType.SER; }
                        group.AddedOn = group.AddedOn;
                        group.base_currency_Id = (cmbBaseCurrencies.SelectedItem as cmbitem).id;
                        group.transaction_currency_Id = (cmbTransCurrencies.SelectedItem as cmbitem).id;
                        group.isVoid = false;
                        group.LastUpdated = DateTime.Now;
                        group.Editedbyuser_Id = SYSTEM_STATIC.currentUser.id;
                        group.TargetYear = Convert.ToInt32(datEffectiveFrom.EditValue);
                        group.exchangeRates = getRates();
                        exchangeRateGroupRepo.Update(group);
                        DXMessageBox.Show("Exchange Rate Updated Successfully ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public List<ExchangeRate> getRates()
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            if (editFlag != true)
            {
                foreach (var rate in grdExchangeRates.ItemsSource as List<ExchangeRate>)
                {
                    if (rate.Id == 0)
                    {
                        if (rate.company != null)
                            exchangeRates.Add(new ExchangeRate()
                            {
                                company_Id = rate.company.Id,
                                rateJan = rate.rateJan,
                                rateFeb = rate.rateFeb,
                                rateMar = rate.rateMar,
                                rateApr = rate.rateApr,
                                rateMay = rate.rateMay,
                                rateJun = rate.rateJun,
                                rateJul = rate.rateJul,
                                rateAug = rate.rateAug,
                                rateSep = rate.rateSep,
                                rateOct = rate.rateOct,
                                rateNov = rate.rateNov,
                                rateDec = rate.rateDec,
                            });
                    }
                }
            }
            else
            {
                foreach (var rate in grdExchangeRates.ItemsSource as List<ExchangeRate>)
                {
                    if (rate.Id == 0)
                    {
                        if (rate.company != null)
                            exchangeRates.Add(new ExchangeRate()
                            {
                                company_Id = rate.company.Id,
                                rateJan = rate.rateJan,
                                rateFeb = rate.rateFeb,
                                rateMar = rate.rateMar,
                                rateApr = rate.rateApr,
                                rateMay = rate.rateMay,
                                rateJun = rate.rateJun,
                                rateJul = rate.rateJul,
                                rateAug = rate.rateAug,
                                rateSep = rate.rateSep,
                                rateOct = rate.rateOct,
                                rateNov = rate.rateNov,
                                rateDec = rate.rateDec,
                            });
                    }
                    else
                    {

                        var dbRate = exchangeRateGroupRepo.GetRate(rate.Id);
                             dbRate.company_Id = rate.company.Id;
                             dbRate.rateJan = rate.rateJan;
                             dbRate.rateFeb = rate.rateFeb;
                             dbRate.rateMar = rate.rateMar;
                             dbRate.rateApr = rate.rateApr;
                             dbRate.rateMay = rate.rateMay;
                             dbRate.rateJun = rate.rateJun;
                             dbRate.rateJul = rate.rateJul;
                             dbRate.rateAug = rate.rateAug;
                             dbRate.rateSep = rate.rateSep;
                             dbRate.rateOct = rate.rateOct;
                             dbRate.rateNov = rate.rateNov;
                             dbRate.rateDec = rate.rateDec;
                             exchangeRates.Add(dbRate);
                    }
                }
            }
            return exchangeRates;
        }

        private void BtnVoid_Click(object sender, RoutedEventArgs e)
        {
            if(groupId!=0 && editFlag==true)
            {
                ExchangeRateGroup group = new ExchangeRateGroup();
                group = exchangeRateGroupRepo.Get(groupId);
                if(group.isVoid==true)
                {
                    var info=DXMessageBox.Show("Exchange Rate is in void list do you want to un-void? ", "Congratulations", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if(info==MessageBoxResult.Yes)
                    {
                        group.isVoid = false;
                        exchangeRateGroupRepo.Update(group);
                        DXMessageBox.Show("Exchange Rate Updated Successfully ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
                else
                {
                    var info = DXMessageBox.Show("Are you sure? Do you want to void this Exchange Rate? ", "Congratulations", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (info == MessageBoxResult.Yes)
                    {
                        group.isVoid = true;
                        exchangeRateGroupRepo.Update(group);
                        DXMessageBox.Show("Exchange Rate Updated Successfully ", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
        }
    }
}
