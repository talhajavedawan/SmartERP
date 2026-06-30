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
    public partial class frmSalesExchangeRateList : Window
    {


        List<SalesExchangeRate> Currencies= new List<SalesExchangeRate>();
        CurrencyRepo repo = new CurrencyRepo();

        public frmSalesExchangeRateList()
        {
            InitializeComponent();
                       
        }

        private void winSalesExchangeRateList_Loaded(object sender, RoutedEventArgs e)
        {
            loadSalesExchangeRate();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSalesExchangeRate);
        }
        
  

        private void loadSalesExchangeRate()
        {
           
            //if (MainWindow.currentUserid == 0)
                Currencies = repo.getAllsalesExchangeRates();
            
                       
            this.grdSalesExchangeRate.ItemsSource = Currencies;
           // grdSalesExchangeRate.Columns.GetColumnByFieldName("Id").Visible = false;
           // //grdSalesExchangeRate.Columns.GetColumnByFieldName("currency_Id").Visible = false;
           // //grdSalesExchangeRate.Columns.GetColumnByFieldName("Basecurrency_Id").Visible = false;
           //// grdSalesExchangeRate.Columns.GetColumnByFieldName("currency").Visible = false;
           // //grdSalesExchangeRate.Columns.GetColumnByFieldName("Basecurrency").Visible = false;
           // grdSalesExchangeRate.Columns.GetColumnByFieldName("Addedbyuser").Visible = false;
           // grdSalesExchangeRate.Columns.GetColumnByFieldName("Addedbyuser_Id").Visible = false;
           // grdSalesExchangeRate.Columns.GetColumnByFieldName("Editedbyuser_Id").Visible = false;
           // grdSalesExchangeRate.Columns.GetColumnByFieldName("Editedbyuser").Visible = false;
           // grdSalesExchangeRate.Columns.GetColumnByFieldName("company").Visible = false;
           // grdSalesExchangeRate.Columns.GetColumnByFieldName("company_Id").Visible = false;
            //grdSalesExchangeRate.Columns.GetColumnByFieldName("maxVariationPercent").Visible = false;



        }

        public void newSalesExchangeRate()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sales Exchange Rates") != null)
            {
                frmSalesExchangeRateadd frmSalesExchangeRateadd = new frmSalesExchangeRateadd();
                frmSalesExchangeRateadd.ShowDialog();
                loadSalesExchangeRate();
            }
            else
            {
                DXMessageBox.Show("Permission Required to view Add Sales Exchange Rates", "Permission Denied!");

            }
           
        }
        

        private void mbtnNewSalesExchangeRate_Click(object sender, RoutedEventArgs e)
        {
            newSalesExchangeRate();

        }

        private void mbtnEditSalesExchangeRate_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sales Exchange Rates") != null)
            {
                if (grdSalesExchangeRate.SelectedItem != null)
                {
                    frmSalesExchangeRateadd.exchangerateId = (grdSalesExchangeRate.SelectedItem as SalesExchangeRate).Id;
                    newSalesExchangeRate();
                }
                else
                {
                    DXMessageBox.Show("Please select a SalesExchangeRate to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Edit Sales Exchange Rates", "Permission Denied!");

            }
            
        }

        private void grdSalesExchangeRate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdSalesExchangeRate.SelectedItem != null)
            {
                frmSalesExchangeRateadd.exchangerateId = (grdSalesExchangeRate.SelectedItem as SalesExchangeRate).Id;
                newSalesExchangeRate();
            }
            
        }

        private void WinSalesExchangeRateList_Unloaded(object sender, RoutedEventArgs e)
        {
            //SystemLogic.SaveUserSettingForCurrentWindow(grdSalesExchangeRate);
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdSalesExchangeRate);
        }
    }
}
