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
    public partial class frmSummarySheetFieldList : Window
    {


        List<SummarySheetField> SummarySheetFields = new List<SummarySheetField>();
        SaleOrderRepo repo = new SaleOrderRepo();
        List<SummarySheetField> Fields = new List<SummarySheetField>();
        SummarySheetField Field = new SummarySheetField();
        public frmSummarySheetFieldList()
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



        //    List<SummarySheetFieldNature> natures = new List<SummarySheetFieldNature>();
        //    natures = repo.getallnature();

        //    List<cmbitem> cmbitems = new List<cmbitem>();



        //    foreach (SummarySheetFieldNature nat in natures)
        //    {

        //        cmbitems.Add(new cmbitem() { name = nat.nature, id = nat.Id });


        //    }
        //    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


        //    cmbNature.ItemsSource = cmbitems;
        //}
        public void loadSummarySheetField()
        {
            if(MainWindow.currentUserid==0)
            SummarySheetFields = repo.getallSummarySheetField();
            else
                SummarySheetFields = repo.getActiveSummarySheetFields();

            this.grdSummarySheetField.ItemsSource = SummarySheetFields;
            grdSummarySheetField.Columns.GetColumnByFieldName("Id").Visible = false;
            grdSummarySheetField.Columns.GetColumnByFieldName("Type").Visible = false;

            grdSummarySheetField.Columns.GetColumnByFieldName("AddedbyUserId").Visible = false;
            grdSummarySheetField.Columns.GetColumnByFieldName("AddedbyUser").Visible = false;

            //grdSummarySheetField.Columns.GetColumnByFieldName("parentSummarySheetField").Visible = false;

        }

        //private void loadItemgrid()
        //{
        //    SummarySheetFieldSummarySheetField categor = grdSummarySheetField.SelectedItem as SummarySheetFieldSummarySheetField;
            
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

        //    //grdSummarySheetField.Columns.GetColumnByFieldName("parentSummarySheetField").Visible = false;
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

        //        product.nature = cmbNature.SelectedItem as SummarySheetFieldNature;
        //        product.nature.Id = (cmbNature.SelectedItem as SummarySheetFieldNature).Id;

        //        //product.UOM = txtUOM.Text.Trim();
        //        //product.quantity = string.IsNullOrEmpty(txtQty.Text.Trim()) ? 0 : Convert.ToDouble(txtQty.Text.Trim());
        //        repo.Add(product);

        //        MessageBox.Show(txtItemName.Text+" Added Succesfully!");
        //        loadItemgrid();
        //    }
        //}
        public void newSummarySheetField()
        {
            frmSummarySheetFieldAdd productSummarySheetFieldAdd = new frmSummarySheetFieldAdd();
            productSummarySheetFieldAdd.ShowDialog();
            loadSummarySheetField();
        }
        public void newItem()
        {
            frmSummarySheetFieldAdd frmItemadd = new frmSummarySheetFieldAdd();
            frmItemadd.ShowDialog();
            loadSummarySheetField();
            //loadItemgrid();
        }
        private void mbtnNewSummarySheetField_Click(object sender, RoutedEventArgs e)
        {
            newSummarySheetField();

        }

        private void mbtnEditSummarySheetField_Click(object sender, RoutedEventArgs e)
        {
            frmSummarySheetFieldAdd.summarySheetFieldId = (grdSummarySheetField.SelectedItem as SummarySheetField).Id;
            newSummarySheetField();
        }

        private void mbtnNewItem_Click(object sender, RoutedEventArgs e)
        {
            newItem();

        }

        //private void mbtnEditItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (grdproducts.SelectedItem != null)
        //    {

        //        SummarySheetFieldss.frmItemadd.productId = (grdproducts.SelectedItem as SummarySheetField).Id;
        //        newItem();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select an Item to Edit");
        //    }
        //}

        //private void grdSummarySheetField_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        //{
        //    if (grdSummarySheetField.SelectedItem != null)
        //        /*grdproducts.ItemsSource = (grdSummarySheetField.SelectedItem as SummarySheetFieldSummarySheetField).products;*/
        //        loadItemgrid();
        //}

        private void grdSummarySheetField_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmSummarySheetFieldAdd.summarySheetFieldId = (grdSummarySheetField.SelectedItem as SummarySheetField).Id;
            newSummarySheetField();
        }

        private void winSummarySheetFieldList_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void WinSummarySheetFieldList_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void winSummarySheetFieldsList_Loaded(object sender, RoutedEventArgs e)
        {
            loadSummarySheetField();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdSummarySheetField);
        }

        private void WinSummarySheetFieldsList_Unloaded(object sender, RoutedEventArgs e)
        {

            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdSummarySheetField);

        }

        //private void grdproducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    SummarySheetFieldss.frmItemadd.productId = (grdproducts.SelectedItem as SummarySheetField).Id;
        //    newItem();
        //}
        //int empid;



    }
}
