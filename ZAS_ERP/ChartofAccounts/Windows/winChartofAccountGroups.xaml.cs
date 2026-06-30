using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
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

namespace ZAS_ERP.ChartofAccounts.Windows
{
    /// <summary>
    /// Interaction logic for winChartofAccountGroups.xaml
    /// </summary>
    public partial class winChartofAccountGroups : DXWindow
    {
        List<Company> allCompanies = new List<Company>();
        List<Company> selectedCompanies = new List<Company>();
        List<Department> allDepartments = new List<Department>();
        List<Department> selectedDepartments = new List<Department>();
        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        List<ERP_BL.Databases.Employee> selectedEmployees = new List<ERP_BL.Databases.Employee>();
        List<cmbitem> allAccountTypes = new List<cmbitem>();
        List<cmbitem> selectedAccountTypes = new List<cmbitem>();
        ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
        ChartofAccountGroup group = new ChartofAccountGroup();
        public winChartofAccountGroups()
        {
            InitializeComponent();
        }
        public winChartofAccountGroups(ChartofAccountGroup _group)
        {
            InitializeComponent();
            group = _group;
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            allCompanies = coaRepo.GetEmployeeCompanies(SYSTEM_STATIC.currentUser.employeeId);
            if (group.Id != 0)
            {
                txtTitle.Text = group.Title;
                txtReference.Text = group.referenceNo;
                selectedCompanies = group.Companies;
                selectedDepartments = group.Departments;
                selectedEmployees = group.Employees;

                // Remove selectedCompanies from allCompanies and update allDepartments
                foreach (Company _Company in selectedCompanies)
                {
                    allCompanies = allCompanies.Where(c => c.Id != _Company.Id).ToList();

                    // Add departments from the selected company and remove duplicates
                    allDepartments.AddRange(_Company.departments);
                    allDepartments = allDepartments.GroupBy(d => d.Id).Select(d => d.First()).ToList();
                }

                // Remove selectedDepartments from allDepartments and update allEmployees
                foreach (Department _department in selectedDepartments)
                {
                    allDepartments = allDepartments.Where(d => d.Id != _department.Id).ToList();

                    // Add employees from the selected department and remove duplicates
                    allEmployees.AddRange(_department.employees);
                    allEmployees = allEmployees.GroupBy(f => f.EmpId).Select(f => f.First()).ToList();
                }

                // Remove selectedEmployees from allEmployees
                foreach (ERP_BL.Databases.Employee _employee in selectedEmployees)
                {
                    allEmployees = allEmployees.Where(x => x.EmpId != _employee.EmpId).ToList();
                }

                // Update grids
                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;
                gridAllDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = selectedDepartments;
                gridAllEmployee.ItemsSource = allEmployees;
                gridSelectedEmployee.ItemsSource = selectedEmployees;

            }
            else
            {
                gridAllCompany.ItemsSource = allCompanies;
            }
        }
        public List<Company> GetCompanies()
        {

            List<Company> companies = new List<Company>();
            var grdCompanies = gridSelectedCompanies.ItemsSource as List<Company>;
            CompanyRepo companyRepo = new CompanyRepo();
            foreach (Company company in grdCompanies)
            {
                companies.Add(company);
            }
            return companies;
        }

        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();
            var grdDepartments = gridSelectedDepartments.ItemsSource as List<Department>;
            foreach (Department department in grdDepartments)
            {
                departments.Add(department);
            }

            return departments;
        }
        public List<ERP_BL.Databases.Employee> GetEmployees()
        {
            List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            var grdEmployees = gridSelectedEmployee.ItemsSource as List<ERP_BL.Databases.Employee>;
            foreach (ERP_BL.Databases.Employee employee in grdEmployees)
            {
                employees.Add(employee);
            }
            return employees;
        }
        private void ImgLeftToRightComp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightComp.Width = 28;

        }

        private void ImgRightToLeftComp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftComp.Width = 28;
        }
        private void ImgLeftToRightDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightDept.Width = 28;

        }

        private void ImgRightToLeftDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftDept.Width = 28;
        }
        private void ImgRightToLeftComp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftComp.Width = 30;
            if (gridSelectedCompanies.SelectedItem != null)
            {
                var company = gridSelectedCompanies.SelectedItem as Company;

                if (company.departments.Intersect(selectedDepartments).Count() > 0)
                {
                    DXMessageBox.Show("Kindly remove the Departments of this Company from Selected Departments!");
                    return;
                }
                selectedCompanies.Remove(company);
                allCompanies.Add(company);

                gridAllCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;

                var depts = selectedDepartments.Except(allDepartments.Intersect(selectedDepartments));
                allDepartments = new List<Department>();
                foreach (var _cmpny in selectedCompanies)
                {
                    allDepartments.AddRange(_cmpny.departments);
                }


                allDepartments = allDepartments.Except(depts).ToList();

                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                gridAllDepartment.ItemsSource = null;
                gridAllDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = null;
                gridSelectedDepartments.ItemsSource = selectedDepartments;
            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }
        private void ImgLeftToRightDept_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightDept.Width = 30;

            if (gridAllDepartment.SelectedItem != null)
            {
                var department = gridAllDepartment.SelectedItem as Department;
                if (allDepartments.Find(x => x.ParentID == department.Id) == null)
                {

                    allDepartments.Remove(department);
                    if (!selectedDepartments.Contains(department))
                        selectedDepartments.Add(department);

                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (allDepartments.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedDepartments.Contains(parent))
                            {
                                selectedDepartments.Add(parent);
                                allEmployees.AddRange(parent.employees);
                            }
                            var findParet = allDepartments.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                allDepartments.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    allEmployees.AddRange(department.employees.Except(selectedEmployees));

                    allEmployees = allEmployees.GroupBy(x => x.EmpId).Select(y => y.First()).ToList();

                    gridAllDepartment.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;
                    gridAllEmployee.ItemsSource = allEmployees;
                    gridAllDepartment.ItemsSource = allDepartments;
                    gridSelectedDepartments.ItemsSource = selectedDepartments;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Department which you want to Insert to Selected Departments!");
            }
        }
        private void ImgRightToLeftDept_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftDept.Width = 28;

            if (gridSelectedDepartments.SelectedItem != null)
            {
                var department = gridSelectedDepartments.SelectedItem as Department;

                if (selectedDepartments.Find(x => x.ParentID == department.Id) == null)
                {
                    selectedDepartments.Remove(department);
                    if (!allDepartments.Contains(department))
                        allDepartments.Add(department);

                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (selectedDepartments.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!allDepartments.Contains(parent))
                                allDepartments.Add(parent);
                            var findParet = selectedDepartments.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                selectedDepartments.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }
                    gridAllDepartment.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;

                    gridAllDepartment.ItemsSource = allDepartments;
                    gridSelectedDepartments.ItemsSource = selectedDepartments;
                    var employees = selectedEmployees.Except(allEmployees.Intersect(selectedEmployees));
                    allEmployees = new List<ERP_BL.Databases.Employee>();
                    foreach (var _dept in selectedDepartments)
                    {
                        allEmployees.AddRange(_dept.employees);
                    }
                    allEmployees = allEmployees.Except(employees).ToList();
                    allEmployees = allEmployees.GroupBy(x => x.EmpId).Select(y => y.First()).ToList();
                    gridAllEmployee.ItemsSource = null;
                    gridAllEmployee.ItemsSource = allEmployees;
                    gridSelectedEmployee.ItemsSource = null;
                    gridSelectedEmployee.ItemsSource = selectedEmployees;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }

            }
            else
            {
                DXMessageBox.Show("Please select Department which you want to Remove from Selected Departments!");
            }
        }
        private void ImgLeftToRightComp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightComp.Width = 30;
            if (gridAllCompany.SelectedItem != null)
            {
                var company = gridAllCompany.SelectedItem as Company;
                selectedCompanies.Add(company);
                allCompanies.Remove(company);
                gridAllCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridAllCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;

                allDepartments.AddRange(company.departments.Except(selectedDepartments));

                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                gridAllDepartment.ItemsSource = null;
                gridAllDepartment.ItemsSource = allDepartments;
            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }

    
        private void imgLeftToRightEmp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightEmp.Width = 28;

        }
        private void imgRightToLeftEmp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftEmp.Width = 30;
            if (gridSelectedEmployee.SelectedItem != null)
            {
                var emp = gridSelectedEmployee.SelectedItem as ERP_BL.Databases.Employee;
                selectedEmployees.Remove(emp);
                allEmployees.Add(emp);
                gridAllEmployee.ItemsSource = null;
                gridSelectedEmployee.ItemsSource = null;
                gridAllEmployee.ItemsSource = allEmployees;
                gridSelectedEmployee.ItemsSource = selectedEmployees;
            }
            else
            {
                DXMessageBox.Show("Please select Employee which you want to Insert to All Employees!");
            }
        }


        private void imgRightToLeftEmp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftEmp.Width = 28;

        }
        private void imgLeftToRightEmp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightEmp.Width = 30;
            if (gridAllEmployee.SelectedItem != null)
            {
                var selectedEmployee = gridAllEmployee.SelectedItem as ERP_BL.Databases.Employee;
                selectedEmployees.Add(selectedEmployee);
                allEmployees.Remove(selectedEmployee);
                gridAllEmployee.ItemsSource = null;
                gridSelectedEmployee.ItemsSource = null;

                gridAllEmployee.ItemsSource = allEmployees;
                gridSelectedEmployee.ItemsSource = selectedEmployees;
            }
            else
            {
                DXMessageBox.Show("Please select Employee which you want to Insert to Selected Employees!");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (group.Id != 0)
                {
                    if (string.IsNullOrEmpty(txtTitle.Text))
                    {
                        DXMessageBox.Show("Please insert Group title");
                        return;
                    }
                    if (string.IsNullOrEmpty(txtReference.Text))
                    {
                        DXMessageBox.Show("Please insert reference number");
                        return;
                    }
                    if (gridSelectedCompanies.ItemsSource == null)
                    {
                        DXMessageBox.Show("Please Select Companies");
                        return;

                    }
                    if (gridSelectedDepartments.ItemsSource == null)
                    {
                        DXMessageBox.Show("Please Select Departments");
                        return;
                    }
                    if (gridSelectedEmployee.ItemsSource == null)
                    {
                        DXMessageBox.Show("Please Select Employees");
                        return;
                    }
                    group.Title = txtTitle.Text;
                    group.referenceNo = txtReference.Text;
                    group.Companies = new List<Company>();
                    group.Companies = GetSelectedCompanies();
                    group.Departments = new List<Department>();
                    group.Departments = GetSelectedDepartments();
                    group.Employees = new List<ERP_BL.Databases.Employee>();
                    group.Employees = GetSelectedEmployees();
                    group.Employees = group.Employees.GroupBy(x => x.EmpId).Select(y => y.FirstOrDefault()).ToList();
                    group.Departments = group.Departments.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
                    group.Companies = group.Companies.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
                    coaRepo.UpdateCoaGroup(group);
                    DXMessageBox.Show("Chart of Acount Group: " + group.Title +" "+ "Updated Sucessfully", MessageBoxImage.Information.ToString(), MessageBoxButton.OK);
                    this.Close();
                }
                else
                {
                    if(string.IsNullOrEmpty(txtTitle.Text))
                    {
                        DXMessageBox.Show("Please insert Group title");
                        return;
                    }
                    if (string.IsNullOrEmpty(txtReference.Text))
                    {
                        DXMessageBox.Show("Please insert reference number");
                        return;
                    }
                    if (gridSelectedCompanies.ItemsSource==null)
                    {
                        DXMessageBox.Show("Please Select Companies");
                        return;

                    }
                    if (gridSelectedDepartments.ItemsSource==null)
                    {
                        DXMessageBox.Show("Please Select Departments");
                        return;
                    }
                    if (gridSelectedEmployee.ItemsSource==null)
                    {
                        DXMessageBox.Show("Please Select Employees");
                        return;
                    }
                    group.Title = txtTitle.Text;
                    group.referenceNo = txtReference.Text;
                    group.Companies = GetSelectedCompanies();
                    group.Departments = GetSelectedDepartments();
                    group.Employees = GetSelectedEmployees();
                    coaRepo.AddCoaGroup(group);
                    DXMessageBox.Show("Chart of Acount Group: " + group.Title+ " " + "Added Sucessfully", MessageBoxImage.Information.ToString(), MessageBoxButton.OK);
                    this.Close();
                }
            }
            catch (Exception ex)
            {

            }

        }
        public List<Company> GetSelectedCompanies()
        {
            List<Company> companies = new List<Company>();
            var grdCompanies = gridSelectedCompanies.ItemsSource as List<Company>;
            foreach (Company company in grdCompanies)
            {
                companies.Add(company);
            }
            return companies;
        }

        public List<Department> GetSelectedDepartments()
        {
            List<Department> departments = new List<Department>();
            var grdDepartments = gridSelectedDepartments.ItemsSource as List<Department>;
            foreach (Department department in grdDepartments)
            {
                departments.Add(department);
            }
            return departments;
        }
        public List<ERP_BL.Databases.Employee> GetSelectedEmployees()
        {
            List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            var grdUsers = gridSelectedEmployee.ItemsSource as List<ERP_BL.Databases.Employee>;
            foreach (ERP_BL.Databases.Employee employee in grdUsers)
            {
                employees.Add(employee);
            }
            return employees;
        }

    }
}
