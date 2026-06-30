using ERP_BL.Databases;
using ERP_BL.HR;
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

namespace ZAS_ERP.HR.Leaves
{
    /// <summary>
    /// Interaction logic for winLeaveSummary.xaml
    /// </summary>
    public partial class winLeaveSummary : Window
    {
        EmployeeRepo employeeRepo = new EmployeeRepo();
        HrRepo hrRepo = new HrRepo();
        int cmbYear = DateTime.Now.Year;
        public winLeaveSummary()
        {
            InitializeComponent();
        }

        public void populateFields()
        {
            //Load years in year combobox
            var currentYear = DateTime.Now.Year;
            for (int i = 1990; i <= currentYear; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
                cmbYears.Items.Add(item);
                cmbYears.SelectedItem = item;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            populateFields();
            var employees = employeeRepo.GetAllEmployees();
            grdLeavesSummary.ItemsSource = employees;
            var cmb = (ComboBoxItem)cmbYears.SelectedItem;
            var item = cmb.Content;
            var year = item.ToString();
            lblHeading.Text = year + " Leaves Summary";
            cmbYear = Convert.ToInt32(item);
        }

        private void CmbYears_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmb = (ComboBoxItem)cmbYears.SelectedItem;
            var item = cmb.Content;
            var year = item.ToString();
            lblHeading.Text = year + " Leaves Summary";
            cmbYear = Convert.ToInt32(item);

            var employees = employeeRepo.GetAllEmployees();
            grdLeavesSummary.ItemsSource = employees;
        }

        private void GrdLeavesSummary_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void GrdLeavesSummary_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                var employee = grdLeavesSummary.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.Databases.Employee;
                



                if (e.Column.FieldName == "casualBalance" && e.IsGetData)
                {
                    double casualBalance = 0;
                    DateTime dt3 = new DateTime(cmbYear, 01, 01, 00, 00, 00);

                    var empId = (int)e.GetListSourceFieldValue("EmpId");

                    try
                    {
                        var casualLeaves = hrRepo.GetEmployeeCasualLeaves(empId).Where(x => x.isApproved != false && x.DateFrom.Year == dt3.Year).ToList();
                        //if (casualLeaves.Count != 0)
                        //{
                            casualBalance = Math.Abs(employee.Desig != null ? employee.Desig.CasualLeaveDays : 0) - Math.Abs( casualLeaves.Sum(x => Math.Abs( x.LeaveDays)));
                        //}
                        //else
                        //{
                          //  casualBalance = 0;
                        //}
                    }
                    catch
                    {
                        casualBalance = 0;
                    }
                    e.Value = casualBalance;
                }

                if (e.Column.FieldName == "annualBalance" && e.IsGetData)
                {
                    DateTime dt3 = new DateTime(cmbYear, 01, 01, 00, 00, 00);

                    var empId = (int)e.GetListSourceFieldValue("EmpId");

                    double annualBalance = 0;

                    //Load Annual Balance
                    try
                    {
                        var annualLeaves = hrRepo.GetEmployeeAnnualLeaves(empId).Where(x => x.isApproved != false && x.DateFrom.Year == dt3.Year).ToList();

                        //if (annualLeaves.Count != 0)
                        //{
                            annualBalance = Math.Abs(employee.Desig != null ?  employee.Desig.AnnualLeaveDays : 0) - Math.Abs(annualLeaves.Sum(x => Math.Abs(x.LeaveDays)));
                            //annualBalance = annualLeaves.Sum(x => x.LeaveDays);
                        //}
                        //else
                        //{ annualBalance = 0; }

                    }
                    catch
                    { annualBalance = 0; }
                    e.Value = annualBalance;
                }

                if (e.Column.FieldName == "adjustedLeaves" && e.IsGetData)
                {

                    DateTime dt3 = new DateTime(cmbYear, 01, 01, 00, 00, 00);

                    var empId = (int)e.GetListSourceFieldValue("EmpId");

                    double adjustmentBalance = 0;
                    try
                    {
                        var adjustedLeaves = hrRepo.GetEmployeeAdjustedLeaves(empId).Where(x => x.isApproved != false && x.DateFrom.Year == dt3.Year).ToList();
                        if (adjustedLeaves.Count != 0)
                        {
                            adjustmentBalance = adjustedLeaves.Sum(x => Math.Abs(x.LeaveDays));
                        }
                        else
                        {
                            adjustmentBalance = 0;
                        }
                    }
                    catch
                    {
                        adjustmentBalance = 0;

                    }
                    e.Value = adjustmentBalance;

                }

                if (e.Column.FieldName == "allocatedCasual" && e.IsGetData)
                {
                    DateTime dt3 = new DateTime(cmbYear, 01, 01, 00, 00, 00);

                    if (cmbYears.SelectedItem != null)
                    {
                        var cmb = (ComboBoxItem)cmbYears.SelectedItem;
                        var item = cmb.Content;
                        var year = Convert.ToInt32(item);

                        var empId = (int)e.GetListSourceFieldValue("EmpId");

                        var allocatedCasual = employee.Desig?.CasualLeaveDays;// hrRepo.GetEmployeeAllocatedCasualLeaves(empId, dt3, "Allocation");
                        e.Value = Convert.ToInt32(allocatedCasual);

                    }
                }
                if (e.Column.FieldName == "allocatedAnnual" && e.IsGetData)
                {
                    DateTime dt3 = new DateTime(cmbYear, 01, 01, 00, 00, 00);

                    if (cmbYears.SelectedItem != null)
                    {
                        var cmb = (ComboBoxItem)cmbYears.SelectedItem;
                        var item = cmb.Content;
                        var year = Convert.ToInt32(item);
                        var empId = (int)e.GetListSourceFieldValue("EmpId");

                        var allocatedAnual = employee.Desig?.AnnualLeaveDays; //hrRepo.GetEmployeeAllocatedAnnualLeaves(empId, dt3, "Allocation");
                        e.Value = Convert.ToInt32(allocatedAnual);
                    }
                }

                if (e.Column.FieldName == "utilizedCasual" && e.IsGetData)
                {
                    var empId = (int)e.GetListSourceFieldValue("EmpId");
                    var allocCasual = (int)e.GetListSourceFieldValue("allocatedCasual");
                    var balCasual = (double)e.GetListSourceFieldValue("casualBalance");

                    if(balCasual < 0)
                    {
                        var utilCasual = Math.Abs(allocCasual) + Math.Abs(balCasual);
                        e.Value = utilCasual;
                    }
                    else
                    {
                        var utilCasual = Math.Abs(allocCasual) - Math.Abs(balCasual);
                        e.Value = utilCasual;
                    }
                }
                if (e.Column.FieldName == "utilizaedAnnual" && e.IsGetData)
                {
                    var empId = (int)e.GetListSourceFieldValue("EmpId");
                    var allocAnnual = (int)e.GetListSourceFieldValue("allocatedAnnual");
                    var balAnnual = (double)e.GetListSourceFieldValue("annualBalance");

                    //var utilCasual = Math.Abs(allocAnnual) - Math.Abs(balAnnual);
                    //e.Value = utilCasual;

                    if (balAnnual < 0)
                    {
                        var utilCasual = Math.Abs(allocAnnual) + Math.Abs(balAnnual);
                        e.Value = utilCasual;
                    }
                    else
                    {
                        var utilCasual = Math.Abs(allocAnnual) - Math.Abs(balAnnual);
                        e.Value = utilCasual;
                    }
                }
                if (e.Column.FieldName == "totalBalance" && e.IsGetData)
                {

                    var balCasual = (double)e.GetListSourceFieldValue("casualBalance");
                    var balAnnual = (double)e.GetListSourceFieldValue("annualBalance");
                    var balAdjustment = (double)e.GetListSourceFieldValue("adjustedLeaves");

                    var totalBal = balCasual + balAnnual + balAdjustment;
                    e.Value = totalBal;
                }
            }
        }

        private void BtnAllocateLeave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditAllocation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddAllocationWin win = new AddAllocationWin();
                var selectedItem = grdLeavesSummary.SelectedItem as ERP_BL.Databases.Employee;
                if (selectedItem != null)
                {
                    win.txtYear.Text = DateTime.Now.Year.ToString();
                    win.txtId.Text = selectedItem.EmpId.ToString();
                    win.txtName.Text = selectedItem.person.FName + " " + selectedItem.person.LName;
                                   
                        var year = DateTime.Now;                        

                        var allocatedCasual = hrRepo.GetEmployeeAllocatedCasualLeaves(selectedItem.EmpId, year, "Allocation");

                       var allocatedAnual = hrRepo.GetEmployeeAllocatedAnnualLeaves(selectedItem.EmpId, year, "Allocation");

                    win.txtannual.Text = allocatedAnual.ToString();
                    win.txtCasual.Text = allocatedCasual.ToString();
                }
                win.Show();
                var employees = employeeRepo.GetAllEmployees();
                grdLeavesSummary.ItemsSource = employees;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }

    public class LeaveSummary
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int AllowedCasual { get; set; }
        public int AllowedAnnual { get; set; }
        public int AllcatedCasual { get; set; }
        public int AllocatedAnnual { get; set; }
        public int UtilizedCasual { get; set; }
        public int UtilizedAnnual { get; set; }
        public int CasualBalance { get; set; }
        public int AnnualBalance { get; set; }
        public int AdjustedLaeves { get; set; }

    }
    public class LeavesSummaryVM
    {

        public List<LeaveSummary> leaveSummary { get; private set; }

        public LeavesSummaryVM()
        {
            List<LeaveSummary> leaveSummaryLst = new List<LeaveSummary>();


        }
    }
}
