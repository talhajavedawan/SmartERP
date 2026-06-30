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

namespace ZAS_ERP.Employeess
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmEmployeesList : Window
    {


        List<ERP_BL.Databases.Employee> Employees = new List<ERP_BL.Databases.Employee>();
        EmployeeRepo repo = new EmployeeRepo();
        List<ERP_BL.Databases.Employee> products = new List<ERP_BL.Databases.Employee>();
        ERP_BL.Databases.Employee product = new ERP_BL.Databases.Employee();
        public frmEmployeesList()
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



        //    List<EmployeeNature> natures = new List<EmployeeNature>();
        //    natures = repo.getallnature();

        //    List<cmbitem> cmbitems = new List<cmbitem>();



        //    foreach (EmployeeNature nat in natures)
        //    {

        //        cmbitems.Add(new cmbitem() { name = nat.nature, id = nat.Id });


        //    }
        //    cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });


        //    cmbNature.ItemsSource = cmbitems;
        //}
        public void loadEmployees()
        {
            if(MainWindow.currentUserid==0)
            Employees = repo.GetActiveEmployeesForCenter();
            else
                Employees = repo.GetActiveEmployeesForCenter();

            this.grdEmployees.ItemsSource = Employees;
            grdEmployees.Columns.GetColumnByFieldName("EmpId").Visible = false;
            grdEmployees.Columns.GetColumnByFieldName("SupervisorId").Visible = false;
            grdEmployees.Columns.GetColumnByFieldName("Supervisor").Visible = false;
            //grdEmployees.Columns.GetColumnByFieldName("userId").Visible = false;
            //grdEmployees.Columns.GetColumnByFieldName("user").Visible = false;
            grdEmployees.Columns.GetColumnByFieldName("contact").Visible = false;
            grdEmployees.Columns.GetColumnByFieldName("person").Visible = false;
            grdEmployees.Columns.GetColumnByFieldName("address").Visible = false;
            grdEmployees.Columns.GetColumnByFieldName("Desig").Visible = false;
            //grdEmployees.Columns.GetColumnByFieldName("parentEmploye").Visible = false;

        }

        //private void loadItemgrid()
        //{
        //    Employee categor = grdEmployees.SelectedItem as Employee;
            
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

        //    //grdEmployees.Columns.GetColumnByFieldName("parentEmploye").Visible = false;
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

        //        product.nature = cmbNature.SelectedItem as EmployeeNature;
        //        product.nature.Id = (cmbNature.SelectedItem as EmployeeNature).Id;

        //        //product.UOM = txtUOM.Text.Trim();
        //        //product.quantity = string.IsNullOrEmpty(txtQty.Text.Trim()) ? 0 : Convert.ToDouble(txtQty.Text.Trim());
        //        repo.Add(product);

        //        MessageBox.Show(txtItemName.Text+" Added Succesfully!");
        //        loadItemgrid();
        //    }
        //}
        public void newEmploye()
        {
            Employeess.frmEmployeeAdd productEmployeAdd = new Employeess.frmEmployeeAdd();
            productEmployeAdd.ShowDialog();
            loadEmployees();
        }
        //public void newItem()
        //{
        //    frmItemadd frmItemadd = new frmItemadd();
        //    frmItemadd.ShowDialog();
        //    loadEmployees();
        //    //loadItemgrid();
        //}
        private void mbtnNewEmploye_Click(object sender, RoutedEventArgs e)
        {
            newEmploye();

        }

        private void mbtnEditEmploye_Click(object sender, RoutedEventArgs e)
        {
            frmEmployeeCenter.empid = (grdEmployees.SelectedItem as ERP_BL.Databases.Employee).EmpId;
            frmEmployeeCenter.editemp = 1;
            newEmploye();
        }

        //private void mbtnNewItem_Click(object sender, RoutedEventArgs e)
        //{
        //    newItem();

        //}

        //private void mbtnEditItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (grdproducts.SelectedItem != null)
        //    {

        //        Employeess.frmItemadd.productId = (grdproducts.SelectedItem as Employee).Id;
        //        newItem();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select an Item to Edit");
        //    }
        //}

        //private void grdEmployees_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        //{
        //    if (grdEmployees.SelectedItem != null)
        //        /*grdproducts.ItemsSource = (grdEmployees.SelectedItem as Employee).products;*/
        //        loadItemgrid();
        //}

        private void grdEmployees_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmEmployeeCenter.empid = (grdEmployees.SelectedItem as ERP_BL.Databases.Employee).EmpId;
            frmEmployeeCenter.editemp = 1;

            newEmploye();
        }

        private void winEmployeesList_Loaded(object sender, RoutedEventArgs e)
        {
            loadEmployees();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdEmployees);
        }

        private void WinEmployeesList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdEmployees);

        }

        //private void grdproducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    Employeess.frmItemadd.productId = (grdproducts.SelectedItem as Employee).Id;
        //    newItem();
        //}
        //int empid;



    }
}
