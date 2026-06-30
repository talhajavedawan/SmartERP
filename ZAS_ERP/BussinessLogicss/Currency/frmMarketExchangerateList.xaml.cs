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

using ERP_BL.Config;
using ERP_BL.Enums;
using DevExpress.Xpf.Core;

namespace ZAS_ERP.BussinessLogicss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmMarketExchangeRateList : Window
    {


        List<MarketExchangeRate> Currencies= new List<MarketExchangeRate>();
        CurrencyRepo repo = new CurrencyRepo();

        public frmMarketExchangeRateList()
        {
            InitializeComponent();
                       
        }

        private void winExchangeRateList_Loaded(object sender, RoutedEventArgs e)
        {
            loadExchangeRate();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdExchangeRate);
        }
        
  

        private void loadExchangeRate()
        {
           
            //if (MainWindow.currentUserid == 0)
                Currencies = repo.getAllMarketExchangeRates();
            
                       
            this.grdExchangeRate.ItemsSource = Currencies;

            //grdExchangeRate.Columns.GetColumnByFieldName("Addedbyuser").Visible = false;
            //grdExchangeRate.Columns.GetColumnByFieldName("Addedbyuser_Id").Visible = false;
            //grdExchangeRate.Columns.GetColumnByFieldName("Editedbyuser_Id").Visible = false;
            //grdExchangeRate.Columns.GetColumnByFieldName("Editedbyuser").Visible = false;
            //grdExchangeRate.Columns.GetColumnByFieldName("company").Visible = false;
            //grdExchangeRate.Columns.GetColumnByFieldName("company_Id").Visible = false;
            //grdExchangeRate.Columns.GetColumnByFieldName("maxVariationPercent").Visible = false;



        }

        public void newExchangeRate()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Exchange Rates") != null)
            {
                frmMarketExchangeRateadd frmExchangeRateadd = new frmMarketExchangeRateadd();
                frmExchangeRateadd.ShowDialog();
                loadExchangeRate();
            }
            else
            {
                DXMessageBox.Show("Permission Required to view Add Market Exchange Rates", "Permission Denied!");

            }
            
        }
        

        private void mbtnNewExchangeRate_Click(object sender, RoutedEventArgs e)
        {
            newExchangeRate();

        }

        private void mbtnEditExchangeRate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Exchange Rates") != null)
            {
                if (grdExchangeRate.SelectedItem != null)
                {
                    frmMarketExchangeRateadd.exchangerateId = (grdExchangeRate.SelectedItem as MarketExchangeRate).Id;
                    newExchangeRate();
                }
                else
                {
                    MessageBox.Show("Please select a ExchangeRate to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to edit Market Exchange Rates", "Permission Denied!");

            }
            
        }

        private void grdExchangeRate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdExchangeRate.SelectedItem != null)
            {

                frmMarketExchangeRateadd.exchangerateId = (grdExchangeRate.SelectedItem as MarketExchangeRate).Id;
                newExchangeRate();
            }
        }

        private void WinExchangeRateList_Unloaded(object sender, RoutedEventArgs e)
        {
            //SystemLogic.SaveUserSettingForCurrentWindow(grdExchangeRate);
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdExchangeRate);
        }
    }
}
