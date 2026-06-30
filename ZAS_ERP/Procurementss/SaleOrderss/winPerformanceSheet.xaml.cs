using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Procurements.Budget;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

namespace ZAS_ERP.Procurementss.SaleOrderss
{
    /// <summary>
    /// Interaction logic for winPerformanceSheet.xaml
    /// </summary>
    public partial class winPerformanceSheet : DXWindow
    {

        BudgetCostCenterRepo repo = new BudgetCostCenterRepo();
        SaleOrder _saleOrder = new SaleOrder();
        public static PerformanceSheet efficencySheet = new PerformanceSheet();
        Department department = new Department();
        List<PerformanceSheetViewModel> modelList = new List<PerformanceSheetViewModel>();
        //List<PerformanceSheetViewModel> modelList = new List<PerformanceSheetViewModel>();

        public winPerformanceSheet()
        {
            InitializeComponent();
        }
        public winPerformanceSheet(SaleOrder saleOrder)
        {
            InitializeComponent();
            _saleOrder = saleOrder;
            grdEfficiency.ItemsSource = modelList;
        }



        private void lookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Performance Sheet") == null)
            {
                grdEfficiency.Columns.GetColumnByFieldName("totalPoints").ReadOnly = true;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Performance Sheet Initial Points") == null)
            {
                grdEfficiency.Columns.GetColumnByFieldName("initialPoints").ReadOnly = true;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Performance Sheet Revised Points") == null)
            {
                grdEfficiency.Columns.GetColumnByFieldName("revisedPoints").ReadOnly = true;
            }
            LoadDepartments();

            LoadSheetData();


        }
        public void LoadSheetData()
        {
            try
            {
                if (_saleOrder.PerformanceSheet_Id != null)
                {
                    efficencySheet = _saleOrder.PerformanceSheet;
                    lookupCustomer.Text = efficencySheet.customer.company.CompanyName;
                    lookupDepartment.Text = efficencySheet.department.DeptName;
                    txtSoNumber.Text = efficencySheet.soNumber;

                    var empSource = (List<cmbitem>)cmbAllocatedTo.Items.SourceCollection;
                    cmbAllocatedTo.SelectedItem = cmbAllocatedTo.Items[cmbAllocatedTo.Items.IndexOf(empSource.Find(x => x.id == efficencySheet.supervoisedId))];


                    var empStaffLevelOneSource = (List<cmbitem>)cmbStaffLevelone.Items.SourceCollection;
                    cmbStaffLevelone.SelectedItem = cmbStaffLevelone.Items[cmbStaffLevelone.Items.IndexOf(empStaffLevelOneSource.Find(x => x.id == efficencySheet.staffLevelOneId))];


                    var empStaffLevelTwoSource = (List<cmbitem>)cmbStaffLevetwo.Items.SourceCollection;
                    cmbStaffLevetwo.SelectedItem = cmbStaffLevetwo.Items[cmbStaffLevetwo.Items.IndexOf(empStaffLevelTwoSource.Find(x => x.id == efficencySheet.staffLevelTwoId))];
                    foreach (var field in efficencySheet.performanceSheetFields)
                    {
                        PerformanceSheetViewModel model = new PerformanceSheetViewModel();
                        {
                            model.Head_Id = field.Head_Id;
                            model.PerformanceSheetHead = field.PerformanceSheetHead;
                            model.initialPoints = field.point;
                            model.revisedPoints = field.revisedPoint;
                            model.totalPoints = field.PerformanceSheetHead.totalPoints;
                        }
                        modelList.Add(model);
                    }
                    grdEfficiency.ItemsSource = modelList;
                    txtAveragePoints.Text = efficencySheet.totalAveragePoints.ToString();
                    txtTotalFinalPoints.Text = efficencySheet.totalPoints.ToString();
                    txtPointsPerc.Text = efficencySheet.totalPointsPerc.ToString();
                    txtTotalPoints.Text = modelList.Sum(x=>x.totalPoints).ToString();

                }
                else
                {
                    txtSoNumber.Text = _saleOrder.SalesReferenceNo;
                    lookupDepartment.Text = _saleOrder.department.DeptName;
                    lookupCustomer.Text = _saleOrder.customerCompany.company.CompanyName;
                    var heads = repo.getActivePerformanceSheetHeads();
                    foreach (var field in heads)
                    {
                        PerformanceSheetViewModel model = new PerformanceSheetViewModel();

                        var head = repo.getPerformanceSheetHead((int)field.Id);
                        model.Head_Id = head.Id;
                        model.PerformanceSheetHead = head;
                        model.initialPoints = 0;
                        model.revisedPoints = 0;
                        model.totalPoints = field.totalPoints;

                        modelList.Add(model);
                    }
                    grdEfficiency.ItemsSource = modelList;
                }
            }
            catch (Exception)
            {


            }
        }
        public void LoadDepartments()
        {
            lookupDepartment.ItemsSource = SYSTEM_STATIC.LoadCurrentUserDepartments();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                List<PerfomarmanceSheetField> fieldValues = new List<PerfomarmanceSheetField>();
                //efficencySheet = new PerformanceSheet();

                efficencySheet.soNumber = txtSoNumber.Text;
                efficencySheet.deptId = (lookupDepartment.SelectedItem as Department).Id;
                efficencySheet.customerId = (lookupCustomer.SelectedItem as CustomerCompany).Id;
                efficencySheet.supervoisedId = (cmbAllocatedTo.SelectedItem as cmbitem).id;
                efficencySheet.staffLevelOneId = (cmbStaffLevelone.SelectedItem as cmbitem).id;
                efficencySheet.staffLevelTwoId = (cmbStaffLevetwo.SelectedItem as cmbitem).id;
                efficencySheet.totalPoints = Convert.ToDouble(txtTotalFinalPoints.Text);
                efficencySheet.totalPointsPerc = Convert.ToDouble(txtPointsPerc.Text);
                efficencySheet.totalAveragePoints = Convert.ToDouble(txtAveragePoints.Text);
                foreach (var value in grdEfficiency.ItemsSource as List<PerformanceSheetViewModel>)
                {
                    PerfomarmanceSheetField field = new PerfomarmanceSheetField();
                    field.Head_Id = value.PerformanceSheetHead.Id;
                    if (efficencySheet.Id == 0 || value.Id == 0)
                    {

                    }
                    else
                    {
                        field.PerformanceSheetHead = value.PerformanceSheetHead;
                    }
                    field.point = value.initialPoints;
                    field.revisedPoint = value.revisedPoints;
                    fieldValues.Add(field);
                }

                efficencySheet.performanceSheetFields = fieldValues;
                this.Close();
            }
            catch (Exception)
            {

            }
        }

        private void lookupDepartment_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            loademployees();
            if (department.Id != 0)
            {
                lookupCustomer.ItemsSource = department.customers;
            }
        }
        public void loademployees()
        {
            ICollection<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            employees = department.employees;
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (ERP_BL.Databases.Employee employee in employees)
            {
                cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            }
            cmbAllocatedTo.ItemsSource = cmbitems;
            cmbStaffLevelone.ItemsSource = cmbitems;
            cmbStaffLevetwo.ItemsSource = cmbitems;
        }

        private void view_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            try
            {
                double suminitalPoints = 0,sumtotalPoints=0;
                var col = e.Column.Name;
               
                if (grdEfficiency.ItemsSource != null)
                    foreach (var item in grdEfficiency.ItemsSource as List<PerformanceSheetViewModel>)
                    {

                        if(item.initialPoints>item.totalPoints)
                        {
                            DXMessageBox.Show("Supervisor Point value" + " " + item.initialPoints + " " + "greater than total point");
                            item.initialPoints = 0;
                            break;
                        }
                        item.revisedPoints = item.initialPoints;

                        if (item.revisedPoints <= item.initialPoints)
                        {
                            suminitalPoints += item.initialPoints;
                        }
                        else
                        {
                            suminitalPoints += item.revisedPoints;
                        }

                        sumtotalPoints += item.PerformanceSheetHead.totalPoints;
                        if (e.Column.FieldName != "revisedPoints" && item.revisedPoints==0)
                        {
                            item.revisedPoints = item.initialPoints;
                        }
                    }
                var averagePonts = suminitalPoints / grdEfficiency.VisibleRowCount;
                txtTotalPoints.Text = sumtotalPoints.ToString();
                txtTotalFinalPoints.Text = suminitalPoints.ToString();

                var percentage = suminitalPoints / sumtotalPoints * 100;
                txtAveragePoints.Text = averagePonts.ToString();
                txtPointsPerc.Text = percentage.ToString();
            }

            catch (global::System.Exception)
            {

            }

        }
        public class PerformanceSheetViewModel
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }
            public int? Head_Id { get; set; }
            [ForeignKey("Head_Id")]
            public virtual PerformanceSheetHead PerformanceSheetHead { get; set; }
            public double initialPoints { get; set; }
            public double revisedPoints { get; set; }
            public double totalPoints { get; set; }

        }
    }
}
