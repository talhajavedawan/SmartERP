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

namespace ZAS_ERP
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmItem : Window
    {



        ProductRepo repo = new ProductRepo();
        List<Product> products = new List<Product>();
        Product product = new Product();
        public frmItem()
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

        private void winItem_Loaded(object sender, RoutedEventArgs e)
        {
            loadItemgrid();
            loadproductnature();
        }
        public void loadproductnature()
        {



            List<ProductNature> natures = new List<ProductNature>();
            natures = repo.getallnature();

            List<cmbitem> cmbitems = new List<cmbitem>();



            foreach (ProductNature nat in natures)
            {

                cmbitems.Add(new cmbitem() { name = nat.nature, id = nat.Id });


            }
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


            cmbNature.ItemsSource = cmbitems;
        }

        private void loadItemgrid()
        {
            products = repo.getAll();
            this.grdproducts.ItemsSource = products;
            grdproducts.Columns.GetColumnByFieldName("Id").Visible = false;
            txtItemName.Text = ""; txtDiscription.Text = ""; txtownDiscription.Text = ""; txtItemCode.Text = ""; cmbNature.Text = "";
            //grdemployee.Columns.GetColumnByFieldName("person").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("address").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("contact").Visible = false;
            ////grdemployee.Columns.GetColumnByFieldName("Companies").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Desig").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Disability").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("DisDescription").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("MaritalStatus").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Status").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("JoinDate").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("BasicPay").Visible = false;
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "EmpId" });
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.FName" });
            //grdemployee.Columns.GetColumnByFieldName("person.FName").Header = "First Name";
            //grdemployee.Columns.Add(new GridColumn() { FieldName = "person.LName" });
            //grdemployee.Columns.GetColumnByFieldName("person.LName").Header = "Last Name";

        }

        
        private void btnSaveItem_Click(object sender, RoutedEventArgs e)
        {
            if(txtItemName.Text == "")
            {
                MessageBox.Show("Enter Item Name");
                txtItemName.Focus();
            }
            else if (txtItemCode.Text == "")
            {
                MessageBox.Show("Enter Item Code");
                txtItemCode.Focus();
            }
            else if (txtDiscription.Text == "")
            {
                MessageBox.Show("Enter Item Discription");
                txtDiscription.Focus();
            }
            else if (cmbNature.Text == "")
            {
                MessageBox.Show("Select Item Nature");
                cmbNature.Focus();
            }
            else if(txtItemName.Text!="" && txtDiscription.Text !="" && txtownDiscription.Text != "" && txtItemCode.Text != "" && cmbNature.Text!="")
            {
                product.code = txtItemCode.Text.Trim();
                product.item =txtItemName.Text.Trim();
                product.itemDescription = txtDiscription.Text.Trim();
                product.ownDescription = txtownDiscription.Text.Trim();
                //product.nature = cmbNature.Text.Trim();

                product.nature = cmbNature.SelectedItem as ProductNature;
                product.nature.Id = (cmbNature.SelectedItem as ProductNature).Id;

                //product.UOM = txtUOM.Text.Trim();
                //product.quantity = string.IsNullOrEmpty(txtQty.Text.Trim()) ? 0 : Convert.ToDouble(txtQty.Text.Trim());
                repo.Add(product);
                
                MessageBox.Show(txtItemName.Text+" Added Succesfully!");
                loadItemgrid();
            }
        }
        //int empid;
        

      
    }
}
