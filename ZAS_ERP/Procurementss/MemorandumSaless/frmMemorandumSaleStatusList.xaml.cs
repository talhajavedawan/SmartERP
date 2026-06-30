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

namespace ZAS_ERP.Procurementss.MemorandumSaless
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmMemorandumSaleStatusList : Window
    {


        List<MemorandumSaleStatus> MemorandumSaleStatuss= new List<MemorandumSaleStatus>();
        MemorandumSaleRepo repo = new MemorandumSaleRepo();

        //MemorandumSaleStatus MemorandumSaleStatus = new MemorandumSaleStatus();
        public frmMemorandumSaleStatusList()
        {
            InitializeComponent();
            
            
        }

        private void winMemorandumSaleStatusList_Loaded(object sender, RoutedEventArgs e)
        {
            loadMemorandumSaleStatus();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdMemorandumSaleStatus);


        }



        private void loadMemorandumSaleStatus()
        {
           
            if (MainWindow.currentUserid == 0)
                MemorandumSaleStatuss = repo.getAllMemorandumSaleStatus();
            
                else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Memorandum Sale Statuses") != null)
            {
                MemorandumSaleStatuss = repo.getAllMemorandumSaleStatus();
            }
            else
            {
                MemorandumSaleStatuss = repo.getAllActiveMemorandumSaleStatus();
            }
            //MemorandumSaleStatuss = repo.getAllActiveMemorandumSaleStatus();

            
            this.grdMemorandumSaleStatus.ItemsSource = MemorandumSaleStatuss;
            grdMemorandumSaleStatus.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdMemorandumSaleStatus.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdMemorandumSaleStatus.Columns.GetColumnByFieldName("user").Visible = false;

            

        }

        public void newMemorandumSaleStatus()
        {
            frmMemorandumSaleStatussAdd frmMemorandumSaleStatussadd = new frmMemorandumSaleStatussAdd();
            frmMemorandumSaleStatussadd.ShowDialog();
            
            loadMemorandumSaleStatus();
        }
        

        private void mbtnNewMemorandumSaleStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Memorandum Sale Status") != null)
            {
                newMemorandumSaleStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to add new status!");
            }
        }

        private void mbtnEditMemorandumSaleStatus_Click(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Memorandum Sale Status") != null)
            {
                if (grdMemorandumSaleStatus.SelectedItem != null)
                {

                    MemorandumSaless.frmMemorandumSaleStatussAdd.StatusId = (grdMemorandumSaleStatus.SelectedItem as MemorandumSaleStatus).Id;

                    newMemorandumSaleStatus();
                }
                else
                {
                    MessageBox.Show("Please select a MemorandumSale Status to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to update existing status!");
            }
            
        }



        private void grdMemorandumSaleStatus_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Memorandum Sale Status") != null)
            {
                if (grdMemorandumSaleStatus.SelectedItem != null)
                    MemorandumSaless.frmMemorandumSaleStatussAdd.StatusId = (grdMemorandumSaleStatus.SelectedItem as MemorandumSaleStatus).Id;
                newMemorandumSaleStatus();
            }
            else
            {
                DXMessageBox.Show("Permission required to update existing status!");
            }
            
        }

        private void WinMemorandumSaleStatusList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdMemorandumSaleStatus);
        }
        //int empid;



    }
}
