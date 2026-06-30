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

namespace ZAS_ERP.Productss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmItemList : ThemedWindow
    {
        List<ProductCategory> productCategories = new List<ProductCategory>();
        ProductRepo repo = new ProductRepo();
        List<Product> products = new List<Product>();
        Product product = new Product();
        public frmItemList()
        {
            InitializeComponent();
        }
        private void winItemList_Loaded(object sender, RoutedEventArgs e)
        {
            loadItemgrid();
            //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdproducts);
        }
        private void loadItemgrid()
        {
            if (MainWindow.currentUserid == 0)
                products = repo.getAll();
            else
            {
                var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();
                var userDepartments= SYSTEM_STATIC.LoadCurrentUserDepartments();
                
                products = repo.getActiveProducts(userCompanies.Select(x => x.Id).ToList(), userDepartments.Select(x => x.Id).ToList());
                products = repo.getAllParentProducts();
            }

            this.grdproducts.ItemsSource = products;
            //grdproducts.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("categoryId").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("category").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("ownDescription").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("nature_Id").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("nature").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("unitOfMeasureId").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("unitOfMeasure").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("user_Id").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("user").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("accountReceivable_id").Visible = false;
            //grdproducts.Columns.GetColumnByFieldName("incomeAccount_id").Visible = false;



        }
        public void newItem()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add New Items") != null)
            {
                try
                {
                    frmItemadd frmItemadd = new frmItemadd();
                    frmItemadd.Show();

                    loadItemgrid();
                }
                catch (Exception ex) { }

            }
            else
            {
                DXMessageBox.Show("Permission required( Add New Items )to add new item!");
            }

        }
        private void grdproducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Productss.frmItemadd.productId = (grdproducts.SelectedItem as Product).Id;
            Productss.frmItemadd.duplicate = false;

            newItem();
        }

        private void WinItemList_Unloaded(object sender, RoutedEventArgs e)
        {
            //SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdproducts);
        }

     

      

        private void BarNewItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            newItem();
        }

        private void BarEditItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Items") != null)
            {
                if (grdproducts.SelectedItem != null)
                {

                    Productss.frmItemadd.productId = (grdproducts.SelectedItem as Product).Id;
                    Productss.frmItemadd.duplicate = false;

                    newItem();
                }
                else
                {
                    MessageBox.Show("Please select an Item to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required ( Edit Items ) to edit item!");
            }
        }

        private void BarInActiveList_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            var userCompanies = SYSTEM_STATIC.LoadCurrentUserCompanies();
            products = repo.getInActiveProducts(userCompanies.Select(x => x.Id).ToList());

            this.grdproducts.ItemsSource = products;
        }

        private void BarRefreshItems_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            repo = new ProductRepo();
            loadItemgrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdproducts);
        }

        private void barDuplicateItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Items") != null)
            {
                if (grdproducts.SelectedItem != null)
                {

                    Productss.frmItemadd.productId = (grdproducts.SelectedItem as Product).Id;
                    Productss.frmItemadd.duplicate = true;

                    newItem();
                }
                else
                {
                    MessageBox.Show("Please select an Item to Edit");
                }
            }
            else
            {
                DXMessageBox.Show("Permission required ( Edit Items ) to edit item!");
            }
        }
        private void barbyPassItems_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Bypass Item Authorities") != null)
            {
                products = repo.getAllParentProducts();
                this.grdproducts.ItemsSource = products;
            }
            else
            {
                DXMessageBox.Show("Permission required to ( Bypass Item Authorities ) !");

            }
        }
    }
}
