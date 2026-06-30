using ERP_BL.Procurements.AdminBills;
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

namespace ZAS_ERP.Procurementss.AdminBillss.UserControls
{
    /// <summary>
    /// Interaction logic for ucPayeeCategoryList.xaml
    /// </summary>
    public partial class ucPayeeCategoryList : UserControl
    {
        AdminBillsRepo billsRepo = new AdminBillsRepo();
        ucFrmPayeeCategory frmPayeeCategory = new ucFrmPayeeCategory();
        public ucPayeeCategoryList()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            billsRepo = new AdminBillsRepo();
            grdCntrlCategoryList.ItemsSource = billsRepo.GetAllPayeeCategories();
        }

        private void MbtnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            frmPayeeCategory = new ucFrmPayeeCategory();
            frmPayeeCategory.addCategoryWindow.ResizeMode = ResizeMode.CanMinimize;
            frmPayeeCategory.addCategoryWindow.Height = 250;
            frmPayeeCategory.addCategoryWindow.Width = 350;
            frmPayeeCategory.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frmPayeeCategory.editFlag = false;
            frmPayeeCategory.addCategoryWindow.Content = frmPayeeCategory;
            frmPayeeCategory.addCategoryWindow.ShowDialog();
        }

        private void MbtnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            billsRepo = new AdminBillsRepo();
            var selectedRow = grdCntrlCategoryList.SelectedItem as PayeeCategory;
            if(selectedRow != null)
            {
                frmPayeeCategory = new ucFrmPayeeCategory();
                frmPayeeCategory.payeeCategory = billsRepo.GetPayeeCategory(selectedRow.Id);
                frmPayeeCategory.addCategoryWindow.ResizeMode = ResizeMode.CanMinimize;
                frmPayeeCategory.addCategoryWindow.Height = 250;
                frmPayeeCategory.addCategoryWindow.Width = 350;
                frmPayeeCategory.addCategoryWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                frmPayeeCategory.editFlag = true;
                frmPayeeCategory.addCategoryWindow.Content = frmPayeeCategory;
                frmPayeeCategory.addCategoryWindow.ShowDialog();
            }
            
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            billsRepo = new AdminBillsRepo();
            grdCntrlCategoryList.ItemsSource = billsRepo.GetAllPayeeCategories();
        }
    }
}
