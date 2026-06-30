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

namespace ZAS_ERP.Procurementss.PurchaseOrderss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmPurchaseOrderStatusList : DXWindow
    {


        List<PurchaseOrderStatus> PurchaseOrderStatuss= new List<PurchaseOrderStatus>();
        PurchaseOrderRepo repo = new PurchaseOrderRepo();

        //PurchaseOrderStatus PurchaseOrderStatus = new PurchaseOrderStatus();
        public frmPurchaseOrderStatusList()
        {
            InitializeComponent();
            
            
        }

        private void winPurchaseOrderStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadPurchaseOrderStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPurchaseOrderStatus);


        }



        private void loadPurchaseOrderStatus()
        {
           
            if (MainWindow.currentUserid == 0)
                PurchaseOrderStatuss = repo.getAllPurchaseOrderStatus();
            
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Order Statuses") != null)
            {
                PurchaseOrderStatuss = repo.getAllPurchaseOrderStatus();
            }
            else
            {
                PurchaseOrderStatuss = repo.getAllActivePurchaseOrderStatus();
            }
            //PurchaseOrderStatuss = repo.getAllActivePurchaseOrderStatus();

            
            this.grdPurchaseOrderStatus.ItemsSource = PurchaseOrderStatuss;
            grdPurchaseOrderStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdPurchaseOrderStatus.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdPurchaseOrderStatus.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newPurchaseOrderStatus()
        {
            frmPurchaseOrderStatussAdd frmPurchaseOrderStatussadd = new frmPurchaseOrderStatussAdd();
            frmPurchaseOrderStatussadd.ShowDialog();
            
            loadPurchaseOrderStatus();
        }
        

        private void mbtnNewPurchaseOrderStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Purchase Order Statuses") != null)
            {
                newPurchaseOrderStatus();
            }
            else
            {
                DXMessageBox.Show("Permission Required to Add new Status!");
            }
            

        }

        private void mbtnEditPurchaseOrderStatus_Click(object sender, RoutedEventArgs e)
        {
            if (grdPurchaseOrderStatus.SelectedItem != null)
            {

                PurchaseOrderss.frmPurchaseOrderStatussAdd.StatusId = (grdPurchaseOrderStatus.SelectedItem as PurchaseOrderStatus).Id;
                
                newPurchaseOrderStatus();
            }
            else
            {
                MessageBox.Show("Please select a PurchaseOrder Status to Edit");
            }
        }



        private void grdPurchaseOrderStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PurchaseOrderss.frmPurchaseOrderStatussAdd.StatusId = (grdPurchaseOrderStatus.SelectedItem as PurchaseOrderStatus).Id;
            newPurchaseOrderStatus();
        }

        private void WinPurchaseOrderStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdPurchaseOrderStatus);
        }

        private void mbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loadPurchaseOrderStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdPurchaseOrderStatus);

        }
        //int empid;



    }
}
