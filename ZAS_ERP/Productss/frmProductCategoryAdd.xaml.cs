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
using ERP_BL;
using ERP_BL.Databases;

namespace ZAS_ERP.Productss
{
    /// <summary>
    /// Interaction logic for frmProductCategoryAdd.xaml
    /// </summary>
    public partial class frmProductCategoryAdd : Window
    {
        public frmProductCategoryAdd()
        {
            InitializeComponent();
        }
        public static int productCategoryId;
        ProductRepo repo = new ProductRepo();
        ProductCategory productCategory = new ProductCategory();
        ProductCategory parentCategory = new ProductCategory();

        private void btnproductCategorySave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtproductCategory.Text == "")
                {
                    MessageBox.Show("Please enter category name", "Required Fields", MessageBoxButton.OK, MessageBoxImage.Stop);
                    txtproductCategory.Focus();
                    return;
                }

                productCategory.discription = txtDiscription.Text.Trim();
            productCategory.category = txtproductCategory.Text.Trim();
                if(parentCategory.Id!=0)
                productCategory.parentId = parentCategory.Id;
            if (chkisactive.IsChecked == true)
                productCategory.isActive = true;
            else
                productCategory.isActive = false;
            if (productCategory.Id == 0)
            {
                    if (MainWindow.currentUserid != 0)
                        productCategory.user_Id = MainWindow.currentUserid;
                    else
                        productCategory.user_Id = null;
                    repo.AddProductCategory(productCategory);

                MessageBox.Show("New Product Category (" + txtproductCategory.Text + ") Added", "Congratulations");
            }
            else
            {
                repo.UpdateProductCategory(productCategory);

                MessageBox.Show("Product Category (" + txtproductCategory.Text + ") updated", "Congratulations");
            }
            this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void loadCategoris()
        {


            List<ProductCategory> categories = new List<ProductCategory>();
            categories = repo.getActiveProductCategories();
            lookupCategory.ItemsSource = categories;

        }
        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lookupCategory_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            parentCategory = lookupCategory.SelectedItem as ProductCategory;
            if (parentCategory != null)
            {
                string selectedcust = parentCategory.category;
                lookupCategory.EditValue = selectedcust;


            }
        }
        private void winProductCategoryAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            productCategoryId = 0;
        }

        private void winProductCategoryAdd_Loaded(object sender, RoutedEventArgs e)
        {
            loadCategoris();
            if (productCategoryId != 0)
            {
                this.Title = "Edit Product Category";
                productCategory = repo.getProductCategory(productCategoryId);
                txtproductCategory.Text = productCategory.category;
                txtDiscription.Text = productCategory.discription;
                chkisactive.IsChecked = productCategory.isActive;
                if (productCategory.parentCategory != null && productCategory.parentId != 0)
                {
                    parentCategory = productCategory.parentCategory;

                    lookupCategory.SelectedItem = lookupCategory.GetItemByKeyValue(parentCategory);
                }
            }
        }
    }
}
