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
using ZAS_ERP.HR;
using DevExpress.Xpf.Core;

namespace ZAS_ERP.Employeess
{
    /// <summary>
    /// Interaction logic for frmUserssCenter.xaml
    /// </summary>
    public partial class frmDesignationsList : Window
    {


        List<Designation> designationDesignations = new List<Designation>();
        EmployeeRepo repo = new EmployeeRepo();
        List<Designation> designations = new List<Designation>();
        Designation designation = new Designation();
        public frmDesignationsList()
        {
            InitializeComponent();
  
        }

       
       
        public void loadDesignations()
        {
            try
            {
                EmployeeRepo employeeRepo = new EmployeeRepo();
                var lst = employeeRepo.getAllDesignation();
                grdDesig.ItemsSource = lst;

                grdDesig.Columns.GetColumnByFieldName("parentDesignation").Visible = false;
                //grdDesignations.Columns.GetColumnByFieldName("parentId").Visible = false;
                grdDesig.Columns.GetColumnByFieldName("companyId").Visible = false;
                grdDesig.Columns.GetColumnByFieldName("company").Visible = false;
                grdDesig.Columns.GetColumnByFieldName("departmentId").Visible = false;

                grdDesig.Columns.GetColumnByFieldName("department").Visible = false;
                grdDesig.Columns.GetColumnByFieldName("user").Visible = false;

                grdDesig.Columns.GetColumnByFieldName("userId").Visible = false;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        //private void loadItemgrid()
        //{
        //    DesignationDesignation categor = grdDesignations.SelectedItem as DesignationDesignation;
            
        //    designations = categor.designations;
        //    // repo.getAll();
        //    this.grddesignations.ItemsSource = designations;
        //    grddesignations.Columns.GetColumnByFieldName("Id").Visible = false;
        //    grddesignations.Columns.GetColumnByFieldName("categoryId").Visible = false;
        //    grddesignations.Columns.GetColumnByFieldName("category").Visible = false;
        //    grddesignations.Columns.GetColumnByFieldName("ownDescription").Visible = false;
        //    grddesignations.Columns.GetColumnByFieldName("nature_Id").Visible = false;
        //    grddesignations.Columns.GetColumnByFieldName("nature").Visible = false;
        //    grddesignations.Columns.GetColumnByFieldName("unitOfMeasureId").Visible = false;
        //    grddesignations.Columns.GetColumnByFieldName("unitOfMeasure").Visible = false;
        //    grddesignations.Columns.GetColumnByFieldName("user_Id").Visible = false;
        //    grddesignations.Columns.GetColumnByFieldName("user").Visible = false;

        //    //grdDesignations.Columns.GetColumnByFieldName("parentDesignation").Visible = false;
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
        //        designation.code = txtItemCode.Text.Trim();
        //        designation.item =txtItemName.Text.Trim();
        //        designation.itemDescription = txtDiscription.Text.Trim();
        //        designation.ownDescription = txtownDiscription.Text.Trim();
        //        //designation.nature = cmbNature.Text.Trim();

        //        designation.nature = cmbNature.SelectedItem as DesignationNature;
        //        designation.nature.Id = (cmbNature.SelectedItem as DesignationNature).Id;

        //        //designation.UOM = txtUOM.Text.Trim();
        //        //designation.quantity = string.IsNullOrEmpty(txtQty.Text.Trim()) ? 0 : Convert.ToDouble(txtQty.Text.Trim());
        //        repo.Add(designation);

        //        MessageBox.Show(txtItemName.Text+" Added Succesfully!");
        //        loadItemgrid();
        //    }
        //}
        public void addDesignation()
        {
            //  Employeess.frmDesignationadd designationDesignationAdd = new Employeess.frmDesignationadd();

            try
            {
                frmAddDesignation designationDesignationAdd = new frmAddDesignation();
                designationDesignationAdd.isEdit = false;
                designationDesignationAdd.ShowDialog();
                loadDesignations();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
          
        }
       
        private void mbtnNewDesignation_Click(object sender, RoutedEventArgs e)
        {
            addDesignation();

        }

        public void editDesig()
        {
            try
            {
                frmAddDesignation frm = new frmAddDesignation();
                frm.isEdit = true;
                var grdRow = (Designation)grdDesig.GetFocusedRow();
                if (grdRow != null)
                {
                    //Setting Title
                    if (!String.IsNullOrEmpty(grdRow.Title))
                    {
                        frm.desgnationTxtbx.Text = grdRow.Title;
                    }
                    //Setting parent
                    if (grdRow.parentDesignation != null)
                    {
                        frm.desigIsSubsidary.IsChecked = true;
                        frm.cmbxDesignations.IsEnabled = true;
                        frm.cmbxDesignations.Text = grdRow.parentDesignation.Title;
                    }
                    else
                    {
                        frm.desigIsSubsidary.IsChecked = false;
                        frm.cmbxDesignations.IsEnabled = false;

                    }

                    //Setting isActive
                    frm.chckisActive.IsChecked = (bool)grdRow.isActive;

                    frm.designationId.Text = grdRow.DesigId.ToString();
                    frm.txtAnnuaLeaves.Text = grdRow.AnnualLeaveDays.ToString() ;
                    frm.txtCasualLeaves.Text = grdRow.CasualLeaveDays.ToString();


                    frm.ShowDialog();
                    loadDesignations();
                }
                //frmDesignationadd.designationId = (grdDesignations.SelectedItem as Designation).DesigId;
                //newDesignation();

                else
                {
                    DXMessageBox.Show("Please select the row first","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
           

        }

        private void mbtnEditDesignation_Click(object sender, RoutedEventArgs e)
        {
            editDesig();
        }

        private void mbtnNewItem_Click(object sender, RoutedEventArgs e)
        {
           // newItem();

        }

        //private void mbtnEditItem_Click(object sender, RoutedEventArgs e)
        //{
        //    if (grddesignations.SelectedItem != null)
        //    {

        //        Designationss.frmDesignationadd.designationId = (grddesignations.SelectedItem as Desig).Id;
        //        newItem();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please select an Item to Edit");
        //    }
        //}

        //private void grdDesignations_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        //{
        //    if (grdDesignations.SelectedItem != null)
        //        /*grddesignations.ItemsSource = (grdDesignations.SelectedItem as DesignationDesignation).designations;*/
        //        loadItemgrid();
        //}

        private void grdDesignations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmDesignationadd.designationId = (grdDesignations.SelectedItem as Designation).DesigId;
            addDesignation();
        }

        private void winDesignationsList_Loaded(object sender, RoutedEventArgs e)
        {
            loadDesignations();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdDesig);
        }

        private void WinDesignationsList_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdDesig);

        }

    }
}
