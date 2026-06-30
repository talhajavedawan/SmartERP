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

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmSaleOrderStatusList : DXWindow
    {


        List<SaleOrderStatus> SaleOrderStatuss= new List<SaleOrderStatus>();
        SaleOrderRepo repo = new SaleOrderRepo();

        //SaleOrderStatus SaleOrderStatus = new SaleOrderStatus();
        public frmSaleOrderStatusList()
        {
            InitializeComponent();
            
            
        }

        private void winSaleOrderStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadSaleOrderStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSaleOrderStatus);


        }



        private void loadSaleOrderStatus()
        {
           
            if (MainWindow.currentUserid == 0)
                SaleOrderStatuss = repo.getAllSaleOrderStatus();
            
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Sale Order Statuses") != null)
            {
                SaleOrderStatuss = repo.getAllSaleOrderStatus();
            }
            else
            {
                SaleOrderStatuss = repo.getAllActiveSaleOrderStatus();
            }
            //SaleOrderStatuss = repo.getAllActiveSaleOrderStatus();

            
            this.grdSaleOrderStatus.ItemsSource = SaleOrderStatuss;
            grdSaleOrderStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdSaleOrderStatus.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdSaleOrderStatus.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newSaleOrderStatus()
        {
            frmSaleOrderStatussAdd frmSaleOrderStatussadd = new frmSaleOrderStatussAdd();
            frmSaleOrderStatussadd.ShowDialog();
            
            loadSaleOrderStatus();
        }
        

        private void mbtnNewSaleOrderStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Sale Order Status") != null)
            {
                newSaleOrderStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Status!");
            }
        }

        private void mbtnEditSaleOrderStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order Status") != null)
            {
                if (grdSaleOrderStatus.SelectedItem != null)
                {

                    SaleOrderss.frmSaleOrderStatussAdd.StatusId = (grdSaleOrderStatus.SelectedItem as SaleOrderStatus).Id;

                    newSaleOrderStatus();
                }
                else
                {
                    MessageBox.Show("Please select a SaleOrder Status to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to Update existing Status!");
            }
            
        }



        private void grdSaleOrderStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Sale Order Status") != null)
            {
                SaleOrderss.frmSaleOrderStatussAdd.StatusId = (grdSaleOrderStatus.SelectedItem as SaleOrderStatus).Id;
                newSaleOrderStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to Update existing Status!");
            }
            
        }

        private void WinSaleOrderStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdSaleOrderStatus);
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loadSaleOrderStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSaleOrderStatus);
        }
        //int empid;



    }
}
