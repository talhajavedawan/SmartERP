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

namespace ZAS_ERP.Procurementss.Billss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmBillStatusList : Window
    {


        List<BillStatus> BillStatuss= new List<BillStatus>();
        BillRepo repo = new BillRepo();

        //BillStatus BillStatus = new BillStatus();
        public frmBillStatusList()
        {
            InitializeComponent();
            
            
        }

        private void winBillStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadBillStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdBillStatus);


        }



        private void loadBillStatus()
        {
           
            if (MainWindow.currentUserid == 0)
                BillStatuss = repo.getAllBillStatus();
            
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Bill Statuses") != null)
            {
                BillStatuss = repo.getAllBillStatus();
            }
            else
            {
                BillStatuss = repo.getAllActiveBillStatus();
            }
            //BillStatuss = repo.getAllActiveBillStatus();

            
            this.grdBillStatus.ItemsSource = BillStatuss;
            grdBillStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdBillStatus.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdBillStatus.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newBillStatus()
        {
            frmBillStatussAdd frmBillStatussadd = new frmBillStatussAdd();
            frmBillStatussadd.ShowDialog();
            
            loadBillStatus();
        }
        

        private void mbtnNewBillStatus_Click(object sender, RoutedEventArgs e)
        {
            newBillStatus();

        }

        private void mbtnEditBillStatus_Click(object sender, RoutedEventArgs e)
        {
            if (grdBillStatus.SelectedItem != null)
            {

                Billss.frmBillStatussAdd.StatusId = (grdBillStatus.SelectedItem as BillStatus).Id;
                
                newBillStatus();
            }
            else
            {
                MessageBox.Show("Please select a Bill Status to Edit");
            }
        }



        private void grdBillStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Billss.frmBillStatussAdd.StatusId = (grdBillStatus.SelectedItem as BillStatus).Id;
            newBillStatus();
        }

        private void WinBillStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdBillStatus);
        }
        //int empid;



    }
}
