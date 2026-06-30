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
    public partial class frmCurrencyList : Window
    {


        List<Currency> Currencies= new List<Currency>();
        CurrencyRepo repo = new CurrencyRepo();

        public frmCurrencyList()
        {
            InitializeComponent();
                       
        }

        private void winCurrencyList_Loaded(object sender, RoutedEventArgs e)
        {
            loadCurrency();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCurrency);
        }
        
  

        private void loadCurrency()
        {
            Currencies = null;
                 //if (MainWindow.currentUserid == 0)
                 Currencies = repo.getAll();
            
                       
            this.grdCurrency.ItemsSource = Currencies;
            grdCurrency.Columns.GetColumnByFieldName("Id").Visible = false;
            

        }

        public void newCurrency()
        {
            frmCurrencyAdd frmCurrencyadd = new frmCurrencyAdd();
            frmCurrencyadd.ShowDialog();
            loadCurrency();
        }
        

        private void mbtnNewCurrency_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Currency") != null)
            {
                newCurrency();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new currency!");
            }

        }

        private void mbtnEditCurrency_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Currency") != null)
            {
                if (grdCurrency.SelectedItem != null)
                {
                    frmCurrencyAdd.currencyId = (grdCurrency.SelectedItem as Currency).Id;
                    newCurrency();
                    repo = new CurrencyRepo();
                    loadCurrency();

                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCurrency);

                }
                else
                {
                    MessageBox.Show("Please select a Currency to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Update existing currency!");
            }


        }

        private void grdCurrency_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmCurrencyAdd.currencyId = (grdCurrency.SelectedItem as Currency).Id;
            newCurrency();
        }

        private void WinCurrencyList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCurrency);
        }

       
    }
}
