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

namespace ZAS_ERP.Productss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmCategoriesList : Window
    {


        List<ProductCategory> productCategories = new List<ProductCategory>();
        ProductRepo repo = new ProductRepo();
        List<Product> products = new List<Product>();
        Product product = new Product();
        public frmCategoriesList()
        {
            InitializeComponent();
            //Config config = new Config();
            //List<string> gentype = config.getItemNature();
            //if (gentype.Count > 0)
            //{
            //    foreach (string IND in gentype)
            //    {
            //        cmbNature.Items.Add(IND);
            //    }
            //}
            
        }

        //public void loadproductnature()
        //{



        //    List<ProductNature> natures = new List<ProductNature>();
        //    natures = repo.getallnature();

        //    List<cmbitem> cmbitems = new List<cmbitem>();



        //    foreach (ProductNature nat in natures)
        //    {

        //        cmbitems.Add(new cmbitem() { name = nat.nature, id = nat.Id });


        //    }
        //    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


        //    cmbNature.ItemsSource = cmbitems;
        //}
        public void loadCategories()
        {
            if(MainWindow.currentUserid==0)
            productCategories = repo.getallProductCategory();
            else
                productCategories = repo.getActiveProductCategories();

            this.grdCategories.ItemsSource = productCategories;
            grdCategories.Columns.GetColumnByFieldName("Id").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("parentId").Visible = false;

            grdCategories.Columns.GetColumnByFieldName("user_Id").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("user").Visible = false;

            grdCategories.Columns.GetColumnByFieldName("parentCategory").Visible = false;

        }

        //private void loadItemgrid()
        //{
        //    ProductCategory categor = grdCategories.SelectedItem as ProductCategory;
            
        //    products = categor.products;
        //    // repo.getAll();
        //    this.grdproducts.ItemsSource = products;
        //    grdproducts.Columns.GetColumnByFieldName("Id").Visible = false;
        //    grdproducts.Columns.GetColumnByFieldName("categoryId").Visible = false;
        //    grdproducts.Columns.GetColumnByFieldName("category").Visible = false;
        //    grdproducts.Columns.GetColumnByFieldName("ownDescription").Visible = false;
        //    grdproducts.Columns.GetColumnByFieldName("nature_Id").Visible = false;
        //    grdproducts.Columns.GetColumnByFieldName("nature").Visible = false;
        //    grdproducts.Columns.GetColumnByFieldName("unitOfMeasureId").Visible = false;
        //    grdproducts.Columns.GetColumnByFieldName("unitOfMeasure").Visible = false;
        //    grdproducts.Columns.GetColumnByFieldName("user_Id").Visible = false;
        //    grdproducts.Columns.GetColumnByFieldName("user").Visible = false;

        //    //grdCategories.Columns.GetColumnByFieldName("parentCategory").Visible = false;
        //    //txtItemName.Text = ""; txtDiscription.Text = ""; txtownDiscription.Text = ""; txtItemCode.Text = ""; cmbNature.Text = "";
        //    //grdemployee.Columns.GetColumnByFieldName("person").Visible = false;
        //    //grdemployee.Columns.GetColumnByFieldName("address").Visible = false;
        //    //grdemployee.Columns.GetColumnByFieldName("contact").Visible = false;
        //    ////grdemployee.Columns.GetColumnByFieldName("Companies").Visible = false;
        //    //grdemployee.Columns.GetColumnByFieldName("Desig").Visible = false;
        //    //grdemployee.Columns.GetColumnByFieldName("Disability").Visible = false;
        //    //grdemployee.Columns.GetColumnByFieldName("DisDescription").Visible = false;
        //    //grdemployee.Columns.GetColumnByFieldName("MaritalStatus").Visible = false;
        //    //grdemployee.Columns.GetColumnByFieldName("Status").Visible = false;
        //    //grdemployee.Columns.GetColumnByFieldName("JoinDate").Visible = false;
        //    //grdemployee.Columns.GetColumnByFieldName("BasicPay").Visible = false;
        //    //grdemployee.Columns.Add(new GridColumn() { FieldName = "EmpId" });
        //    //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.FName" });
        //    //grdemployee.Columns.GetColumnByFieldName("person.FName").Header = "First Name";
        //    //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.LName" });
        //    //grdemployee.Columns.GetColumnByFieldName("person.LName").Header = "Last Name";

        //}


        //private void btnSaveItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if(txtItemName.Text == "")
        //    {
        //        MessageBox.Show("Enter Item Name");
        //        txtItemName.Focus();
        //    }
        //    else if (txtItemCode.Text == "")
        //    {
        //        MessageBox.Show("Enter Item Code");
        //        txtItemCode.Focus();
        //    }
        //    else if (txtDiscription.Text == "")
        //    {
        //        MessageBox.Show("Enter Item Discription");
        //        txtDiscription.Focus();
        //    }
        //    else if (cmbNature.Text == "")
        //    {
        //        MessageBox.Show("Select Item Nature");
        //        cmbNature.Focus();
        //    }
        //    else if(txtItemName.Text!="" && txtDiscription.Text !="" && txtownDiscription.Text != "" && txtItemCode.Text != "" && cmbNature.Text!="")
        //    {
        //        product.code = txtItemCode.Text.Trim();
        //        product.item =txtItemName.Text.Trim();
        //        product.itemDescription = txtDiscription.Text.Trim();
        //        product.ownDescription = txtownDiscription.Text.Trim();
        //        //product.nature = cmbNature.Text.Trim();

        //        product.nature = cmbNature.SelectedItem as ProductNature;
        //        product.nature.Id = (cmbNature.SelectedItem as ProductNature).Id;

        //        //product.UOM = txtUOM.Text.Trim();
        //        //product.quantity = string.IsNullOrEmpty(txtQty.Text.Trim()) ? 0 : Convert.ToDouble(txtQty.Text.Trim());
        //        repo.Add(product);

        //        MessageBox.Show(txtItemName.Text+" Added Succesfully!");
        //        loadItemgrid();
        //    }
        //}
        public void newCategory()
        {
            Productss.frmProductCategoryAdd productCategoryAdd = new Productss.frmProductCategoryAdd();
            productCategoryAdd.ShowDialog();
            loadCategories();
        }
        public void newItem()
        {
            frmItemadd frmItemadd = new frmItemadd();
            frmItemadd.ShowDialog();
            loadCategories();
            //loadItemgrid();
        }
        private void mbtnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            newCategory();

        }

        private void mbtnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            frmProductCategoryAdd.productCategoryId = (grdCategories.SelectedItem as ProductCategory).Id;
            newCategory();
        }

        private void mbtnNewItem_Click(object sender, RoutedEventArgs e)
        {
            newItem();

        }

        //private void mbtnEditItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (grdproducts.SelectedItem != null)
        //    {

        //        Productss.frmItemadd.productId = (grdproducts.SelectedItem as Product).Id;
        //        newItem();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select an Item to Edit");
        //    }
        //}

        //private void grdCategories_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        //{
        //    if (grdCategories.SelectedItem != null)
        //        /*grdproducts.ItemsSource = (grdCategories.SelectedItem as ProductCategory).products;*/
        //        loadItemgrid();
        //}

        private void grdCategories_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmProductCategoryAdd.productCategoryId = (grdCategories.SelectedItem as ProductCategory).Id;
            newCategory();
        }

        private void winCategoriesList_Loaded(object sender, RoutedEventArgs e)
        {
            loadCategories();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCategories);
        }

        private void WinCategoriesList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCategories);

        }

        //private void grdproducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    Productss.frmItemadd.productId = (grdproducts.SelectedItem as Product).Id;
        //    newItem();
        //}
        //int empid;



    }
}
