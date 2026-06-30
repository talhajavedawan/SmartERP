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

namespace ZAS_ERP.Attachmentss
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmAttachmentCategoriesList : Window
    {


        List<AttachmentCategory> attachmentCategories = new List<AttachmentCategory>();
        AttachmentsRepo repo = new AttachmentsRepo();
        List<Attachment> products = new List<Attachment>();
        Attachment product = new Attachment();
        public frmAttachmentCategoriesList()
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



        //    List<AttachmentNature> natures = new List<AttachmentNature>();
        //    natures = repo.getallnature();

        //    List<cmbitem> cmbitems = new List<cmbitem>();



        //    foreach (AttachmentNature nat in natures)
        //    {

        //        cmbitems.Add(new cmbitem() { name = nat.nature, id = nat.Id });


        //    }
        //    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


        //    cmbNature.ItemsSource = cmbitems;
        //}
        public void loadCategories()
        {
            if(MainWindow.currentUserid==0)
            attachmentCategories = repo.getallAttachmentCategories();
            else
                attachmentCategories = repo.getActiveAttachmentCategories();

            this.grdCategories.ItemsSource = attachmentCategories;
            grdCategories.Columns.GetColumnByFieldName("Id").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("ParentId").Visible = false;

            grdCategories.Columns.GetColumnByFieldName("userId").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("user").Visible = false;

            grdCategories.Columns.GetColumnByFieldName("parentCategory").Visible = false;

            grdCategories.Columns.GetColumnByFieldName("Inquiry").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("Offer").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("PO").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("SO").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("SI").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("SR").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("PI").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("Payment").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("VBill").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("ABill").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("IBT").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("ICBT").Visible = false;
            grdCategories.Columns.GetColumnByFieldName("MS").Visible = false;

    }

        //private void loadItemgrid()
        //{
        //    AttachmentCategory categor = grdCategories.SelectedItem as AttachmentCategory;
            
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

        //        product.nature = cmbNature.SelectedItem as AttachmentNature;
        //        product.nature.Id = (cmbNature.SelectedItem as AttachmentNature).Id;

        //        //product.UOM = txtUOM.Text.Trim();
        //        //product.quantity = string.IsNullOrEmpty(txtQty.Text.Trim()) ? 0 : Convert.ToDouble(txtQty.Text.Trim());
        //        repo.Add(product);

        //        MessageBox.Show(txtItemName.Text+" Added Succesfully!");
        //        loadItemgrid();
        //    }
        //}
        public void newCategory()
        {
            Attachmentss.frmAttachmentCategoryAdd productCategoryAdd = new Attachmentss.frmAttachmentCategoryAdd();
            productCategoryAdd.ShowDialog();
            loadCategories();
        }
        
        private void mbtnNewCategory_Click(object sender, RoutedEventArgs e)
        {
            newCategory();

        }

        private void mbtnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            frmAttachmentCategoryAdd.attachmentCategoryId = (grdCategories.SelectedItem as AttachmentCategory).Id;
            newCategory();
        }

        private void mbtnNewItem_Click(object sender, RoutedEventArgs e)
        {
           

        }

        //private void mbtnEditItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (grdproducts.SelectedItem != null)
        //    {

        //        Attachmentss.frmItemadd.productId = (grdproducts.SelectedItem as Attachment).Id;
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
        //        /*grdproducts.ItemsSource = (grdCategories.SelectedItem as AttachmentCategory).products;*/
        //        loadItemgrid();
        //}

        private void grdCategories_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmAttachmentCategoryAdd.attachmentCategoryId = (grdCategories.SelectedItem as AttachmentCategory).Id;
            newCategory();
        }

        private void winAttachmentCategoriesList_Loaded(object sender, RoutedEventArgs e)
        {
            loadCategories();
            //SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCategories);
        }

        private void WinAttachmentCategoriesList_Unloaded(object sender, RoutedEventArgs e)
        {
            //SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCategories);

        }

        //private void grdproducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    Attachmentss.frmItemadd.productId = (grdproducts.SelectedItem as Attachment).Id;
        //    newItem();
        //}
        //int empid;



    }
}
