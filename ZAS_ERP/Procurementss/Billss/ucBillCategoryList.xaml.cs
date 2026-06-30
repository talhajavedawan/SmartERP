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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.Procurementss.Billss
{
    /// <summary>
    /// Interaction logic for ucBillCategoryList.xaml
    /// </summary>
    public partial class ucBillCategoryList : UserControl
    {//comment
        BillRepo billRepo = new BillRepo();
        public ucBillCategoryList()
        {
            InitializeComponent();
        }

        private void MbtnAddBillCategory_Click(object sender, RoutedEventArgs e)
        {
            ucFrmBillCategory frmBillCategory = new ucFrmBillCategory();
            Window win = new Window();
            frmBillCategory.editFlag = false;
            win.Content = frmBillCategory;
            win.Width = 300;
            win.Height = 250;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ResizeMode = ResizeMode.CanMinimize;
            win.Show();
        }

        private void MbtnEditBillCategory_Click(object sender, RoutedEventArgs e)
        {
            if (grdCntrlBillCategoryList.SelectedItem != null)
            {
                var selectedRow = grdCntrlBillCategoryList.SelectedItem as BillCategory;
                ucFrmBillCategory frmBillCategory = new ucFrmBillCategory();
                Window win = new Window();
                frmBillCategory.billCategory = selectedRow;
                frmBillCategory.editFlag = true;
                win.Content = frmBillCategory;
                win.Width = 300;
                win.Height = 250;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ResizeMode = ResizeMode.CanMinimize;
                win.Show();
            }
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            billRepo = new BillRepo();
            grdCntrlBillCategoryList.ItemsSource = billRepo.GetAllBillCategories();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billRepo = new BillRepo();
            grdCntrlBillCategoryList.ItemsSource = billRepo.GetAllBillCategories();
        }
        
    }
}
