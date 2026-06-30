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

namespace ZAS_ERP.Procurementss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmBillTypeList : Window
    {


        List<BillType> billTypes= new List<BillType>();
        BillRepo repo = new BillRepo();

        public frmBillTypeList()
        {
            InitializeComponent();
                       
        }

        private void winBillTypeList_Loaded(object sender, RoutedEventArgs e)
        {
            loadBillType();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdBillType);


        }



        private void loadBillType()
        {
           
            if (MainWindow.currentUserid == 0)
                billTypes = repo.getallbillType();
            else
                billTypes = repo.getActiveBillTypes();
                       
            this.grdBillType.ItemsSource = billTypes;
            grdBillType.Columns.GetColumnByFieldName("Id").Visible = false;
            grdBillType.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdBillType.Columns.GetColumnByFieldName("user").Visible = false;

        }

        public void newBillType()
        {
            frmBillTypeAdd frmBillTypeadd = new frmBillTypeAdd();
            frmBillTypeadd.ShowDialog();
            loadBillType();
        }
        

        private void mbtnNewBillType_Click(object sender, RoutedEventArgs e)
        {
            newBillType();

        }

        private void mbtnEditBillType_Click(object sender, RoutedEventArgs e)
        {
            if (grdBillType.SelectedItem != null)
            {
                frmBillTypeAdd.billTypeId = (grdBillType.SelectedItem as BillType).Id;
                newBillType();
            }
            else
            {
                MessageBox.Show("Please select a BillType to Edit");
            }
        }

        private void grdBillType_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmBillTypeAdd.billTypeId = (grdBillType.SelectedItem as BillType).Id;
            newBillType();
        }

        private void WinBillTypeList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdBillType);
        }
    }
}
