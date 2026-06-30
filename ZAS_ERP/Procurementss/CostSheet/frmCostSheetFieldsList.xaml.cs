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
    public partial class frmCostSheetFieldList : Window
    {


        List<CostSheetField> CostSheetFields = new List<CostSheetField>();
        SaleOrderRepo repo = new SaleOrderRepo();
        List<CostSheetField> Fields = new List<CostSheetField>();
        CostSheetField Field = new CostSheetField();
        public frmCostSheetFieldList()
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



        //    List<CostSheetFieldNature> natures = new List<CostSheetFieldNature>();
        //    natures = repo.getallnature();

        //    List<cmbitem> cmbitems = new List<cmbitem>();



        //    foreach (CostSheetFieldNature nat in natures)
        //    {

        //        cmbitems.Add(new cmbitem() { name = nat.nature, id = nat.Id });


        //    }
        //    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


        //    cmbNature.ItemsSource = cmbitems;
        //}
        public void loadCostSheetField()
        {
            if(MainWindow.currentUserid==0)
            CostSheetFields = repo.getallCostSheetField();
            else
                CostSheetFields = repo.getActiveCostSheetFields();

            this.grdCostSheetField.ItemsSource = CostSheetFields;
            //grdCostSheetField.Columns.GetColumnByFieldName("Id").Visible = false;
            //grdCostSheetField.Columns.GetColumnByFieldName("Type").Visible = false;

            //grdCostSheetField.Columns.GetColumnByFieldName("AddedbyUserId").Visible = false;
            //grdCostSheetField.Columns.GetColumnByFieldName("AddedbyUser").Visible = false;

            //grdCostSheetField.Columns.GetColumnByFieldName("parentCostSheetField").Visible = false;

        }

        //private void loadItemgrid()
        //{
        //    CostSheetFieldCostSheetField categor = grdCostSheetField.SelectedItem as CostSheetFieldCostSheetField;
            
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

        //    //grdCostSheetField.Columns.GetColumnByFieldName("parentCostSheetField").Visible = false;
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

        //        product.nature = cmbNature.SelectedItem as CostSheetFieldNature;
        //        product.nature.Id = (cmbNature.SelectedItem as CostSheetFieldNature).Id;

        //        //product.UOM = txtUOM.Text.Trim();
        //        //product.quantity = string.IsNullOrEmpty(txtQty.Text.Trim()) ? 0 : Convert.ToDouble(txtQty.Text.Trim());
        //        repo.Add(product);

        //        MessageBox.Show(txtItemName.Text+" Added Succesfully!");
        //        loadItemgrid();
        //    }
        //}
        public void newCostSheetField()
        {
            frmCostSheetFieldAdd productCostSheetFieldAdd = new frmCostSheetFieldAdd();
            productCostSheetFieldAdd.ShowDialog();
            loadCostSheetField();
        }
        public void newItem()
        {
            frmCostSheetFieldAdd frmItemadd = new frmCostSheetFieldAdd();
            frmItemadd.ShowDialog();
            loadCostSheetField();
            //loadItemgrid();
        }
        private void mbtnNewCostSheetField_Click(object sender, RoutedEventArgs e)
        {
            newCostSheetField();

        }

        private void mbtnEditCostSheetField_Click(object sender, RoutedEventArgs e)
        {
            frmCostSheetFieldAdd.costSheetFieldId = (grdCostSheetField.SelectedItem as CostSheetField).Id;
            newCostSheetField();
        }

        private void mbtnNewItem_Click(object sender, RoutedEventArgs e)
        {
            newItem();

        }

        //private void mbtnEditItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (grdproducts.SelectedItem != null)
        //    {

        //        CostSheetFieldss.frmItemadd.productId = (grdproducts.SelectedItem as CostSheetField).Id;
        //        newItem();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select an Item to Edit");
        //    }
        //}

        //private void grdCostSheetField_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        //{
        //    if (grdCostSheetField.SelectedItem != null)
        //        /*grdproducts.ItemsSource = (grdCostSheetField.SelectedItem as CostSheetFieldCostSheetField).products;*/
        //        loadItemgrid();
        //}

        private void grdCostSheetField_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmCostSheetFieldAdd.costSheetFieldId = (grdCostSheetField.SelectedItem as CostSheetField).Id;
            newCostSheetField();
        }

        private void winCostSheetFieldList_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void WinCostSheetFieldList_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void winCostSheetFieldsList_Loaded(object sender, RoutedEventArgs e)
        {
            loadCostSheetField();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdCostSheetField);
        }

        private void WinCostSheetFieldsList_Unloaded(object sender, RoutedEventArgs e)
        {

            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdCostSheetField);

        }

        //private void grdproducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    CostSheetFieldss.frmItemadd.productId = (grdproducts.SelectedItem as CostSheetField).Id;
        //    newItem();
        //}
        //int empid;



    }
}
