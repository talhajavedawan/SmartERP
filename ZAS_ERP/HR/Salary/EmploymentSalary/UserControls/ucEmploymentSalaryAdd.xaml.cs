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
using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.HR;

namespace ZAS_ERP.HR.Salary.EmploymentSalary.UserControls
{
    /// <summary>
    /// Interaction logic for ucEmploymentSalaryAdd.xaml
    /// </summary>
    public partial class ucEmploymentSalaryAdd : UserControl
    {
        public bool editFlag = false;
        public int employmentSalaryId = 0;
        ERP_BL.HR.EmploymentSalary employmentSalary = new ERP_BL.HR.EmploymentSalary();
        PayrollRepo payrollRepo = new PayrollRepo();
        public ucEmploymentSalaryAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(editFlag == false)
            {
                datCreationDate.DateTime = DateTime.Now;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
            }
            LoadCompanies();

            if(editFlag == true && employmentSalaryId > 0)
            {
                btnSave.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.FirstOrDefault(x => x.Name == "Edit Employment Salary") != null) ? true : false;
                employmentSalary = payrollRepo.GetEmploymentSalary(employmentSalaryId);
                if (employmentSalary.CreationDate != null)
                    datCreationDate.DateTime = employmentSalary.CreationDate.Value;
                if (!String.IsNullOrEmpty(employmentSalary.SystemRef))
                    txtSystemRef.Text = employmentSalary.SystemRef;
                if (employmentSalary.company != null)
                    lookupCompany.EditValue = employmentSalary.company.Id;
                if (employmentSalary.department != null)
                    lookupDepartment.EditValue = employmentSalary.department.Id;
                if (employmentSalary.employee != null)
                    lookupEmployee.EditValue = employmentSalary.employee.EmpId;
                if (employmentSalary.creator != null)
                    txtCreator.Text = employmentSalary.creator.employee.person.FName +" "+ employmentSalary.creator.employee.person.LName;
                txtBasicSalary.Text = employmentSalary.BasicSalary.ToString();
                chkIsActive.IsChecked = employmentSalary.isActive;
                if (employmentSalary.FromDate != null)
                    datFromDate.DateTime = employmentSalary.FromDate.Value;
                if (employmentSalary.ToDate != null)
                    datToDate.DateTime = employmentSalary.ToDate.Value;
                txtDescription.Text = employmentSalary.Description;
            }
        }

        private void LoadCompanies()
        {
            lookupCompany.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var company = lookupCompany.SelectedItem as Company;
                var depts = company.departments;

                List<Department> departments = new List<Department>();

                foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments)
                {
                    if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                        departments.Add(_dept);
                }
                lookupDepartment.ItemsSource = departments;
            }
            catch (Exception ex)
            {

                DXMessageBox.Show(ex.Message + "Invalid Company");
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {

        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {

        }



        private void LookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }

        }

        private void LookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var department = lookupDepartment.SelectedItem as Department;

            if (department != null)
            {
                lookupEmployee.ItemsSource = department.employees.Where(x => x.isActive == true).ToList();
            }
        }

        private void LookupEmployee_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company first!");
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == false)
            {
                //datCreationDate.DateTime = DateTime.Now;
                GroupIdCalculation();
                txtSystemRef.Text = "Salary-" + intGroupId;
                lblLoansAdvancesRefNo.Text = " (Salary-" + intGroupId + ")";
                employmentSalary.transactionGroupId = intGroupId;
            }
            if (datCreationDate.DateTime == null)
            {
                DXMessageBox.Show("Please Select Creation Date!");
                datCreationDate.Focus();
                return;
            }
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Company!");
                lookupCompany.Focus();
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Department!");
                lookupDepartment.Focus();
                return;
            }
            if (lookupEmployee.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Employee!");
                lookupEmployee.Focus();
                return;
            }
            if(Convert.ToDouble( txtBasicSalary.Text) <= 0)
            {
                DXMessageBox.Show("Please Enter Basic Salary!");
                txtBasicSalary.Focus();
                return;
            }
            if (datFromDate.DateTime == null)
            {
                DXMessageBox.Show("Please Select From Date!");
                datFromDate.Focus();
                return;
            }
            if (datToDate.DateTime == null)
            {
                DXMessageBox.Show("Please Select To Date!");
                datToDate.Focus();
                return;
            }
            employmentSalary.CreationDate = datCreationDate.DateTime;
            employmentSalary.SystemRef = txtSystemRef.Text;
            employmentSalary.companyId = (lookupCompany.SelectedItem as Company).Id;
            employmentSalary.departmentId = (lookupDepartment.SelectedItem as Department).Id;
            employmentSalary.employeeId = (lookupEmployee.SelectedItem as ERP_BL.Databases.Employee).EmpId;
            employmentSalary.BasicSalary = Convert.ToDouble( txtBasicSalary.Text);
            employmentSalary.isActive = chkIsActive.IsChecked.Value;
            employmentSalary.FromDate = datFromDate.DateTime;
            employmentSalary.ToDate = datToDate.DateTime;
            employmentSalary.Description = txtDescription.Text;

            if (editFlag == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new added Employment Salary") != null && employmentSalary.isApproved != true) 
                {
                    if (DevExpress.Xpf.Core.DXMessageBox.Show("This Loans and Advances is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {

                        employmentSalary.stage =  TransactionStage.Approved.ToString();
                        employmentSalary.isApproved = true;
                        employmentSalary.ApprovedDate = System.DateTime.Now;
                    }
                }

                payrollRepo.UpdateEmploymentSalary(employmentSalary);
                DXMessageBox.Show("Updated Successfully!");
            }
            else
            {
                employmentSalary.creatorId = SYSTEM_STATIC.currentUser.id;
                employmentSalary.isApproved = false;
                payrollRepo.AddEmploymentSalary(employmentSalary);
                DXMessageBox.Show("Added Successfully!");
            }
            Window window = Window.GetWindow(this);
            window.Close();
        }


        int intGroupId;
        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = payrollRepo.GetLastTransactionId();
            if (lastPaymentId == 0)
            {
                year = DateTime.Now.Year.ToString();
                month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                id = "1";
                groupId = year + month + id;
            }
            else
            {
                // var last = history.Last();
                var lastId = lastPaymentId/*.transactionGroupId*/;
                string fullId = lastId.ToString();
                int length = fullId.Length;
                //length = length - 1;

                year = fullId.Substring(0, 4);
                if (year != DateTime.Now.Year.ToString())
                {
                    year = DateTime.Now.Year.ToString();
                }
                month = fullId.Substring(4, 2);

                var currentMonth = DateTime.Now.Month.ToString();
                if (currentMonth.Length == 1)
                {
                    currentMonth = "0" + currentMonth;
                }

                int idLen = length - 6;
                id = fullId.Substring(6, idLen);

                if (month != currentMonth)
                {
                    month = currentMonth;

                    id = "1";
                }
                else
                {
                    int intId = Convert.ToInt32(id);
                    intId = intId + 1;

                    id = intId.ToString();
                }
                groupId = year + month + id;
            }

            intGroupId = Convert.ToInt32(groupId);
        }
    }
}
