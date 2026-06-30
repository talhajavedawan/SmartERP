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

namespace ZAS_ERP.Procurementss.SaleInvoicess
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmSaleInvoiceStatusList : DXWindow
    {


        List<SaleInvoiceStatus> SaleInvoiceStatuss= new List<SaleInvoiceStatus>();
        SaleInvoiceRepo repo = new SaleInvoiceRepo();

        //SaleInvoiceStatus SaleInvoiceStatus = new SaleInvoiceStatus();
        public frmSaleInvoiceStatusList()
        {
            InitializeComponent();
            
            
        }

        private void winSaleInvoiceStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadSaleInvoiceStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSaleInvoiceStatus);


        }



        private void loadSaleInvoiceStatus()
        {
           
            if (MainWindow.currentUserid == 0)
                SaleInvoiceStatuss = repo.getAllSaleInvoiceStatus();
            
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Invoice Statuses") != null)
            {
                SaleInvoiceStatuss = repo.getAllSaleInvoiceStatus();
            }
            else
            {
                SaleInvoiceStatuss = repo.getAllActiveSaleInvoiceStatus();
            }
            //SaleInvoiceStatuss = repo.getAllActiveSaleInvoiceStatus();

            
            this.grdSaleInvoiceStatus.ItemsSource = SaleInvoiceStatuss;
            grdSaleInvoiceStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdSaleInvoiceStatus.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdSaleInvoiceStatus.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newSaleInvoiceStatus()
        {
            frmSaleInvoiceStatussAdd frmSaleInvoiceStatussadd = new frmSaleInvoiceStatussAdd();
            frmSaleInvoiceStatussadd.ShowDialog();
            
            loadSaleInvoiceStatus();
        }
        

        private void mbtnNewSaleInvoiceStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Invoice Status") != null)
            {
                newSaleInvoiceStatus();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Status!");
            }
                

        }

        private void mbtnEditSaleInvoiceStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Invoice Status") != null)
            {
                if (grdSaleInvoiceStatus.SelectedItem != null)
                {

                    SaleInvoicess.frmSaleInvoiceStatussAdd.StatusId = (grdSaleInvoiceStatus.SelectedItem as SaleInvoiceStatus).Id;

                    newSaleInvoiceStatus();
                }
                else
                {
                    MessageBox.Show("Please select a SaleInvoice Status to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Status!");
            }

            
        }



        private void grdSaleInvoiceStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Invoice Status") != null)
            {
                if (grdSaleInvoiceStatus.SelectedItem != null)
                {
                    SaleInvoicess.frmSaleInvoiceStatussAdd.StatusId = (grdSaleInvoiceStatus.SelectedItem as SaleInvoiceStatus).Id;
                    newSaleInvoiceStatus();
                }
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Status!");
            }
            
        }

        private void WinSaleInvoiceStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdSaleInvoiceStatus);
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loadSaleInvoiceStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSaleInvoiceStatus);
        }
        //int empid;



    }
}
