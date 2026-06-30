using DevExpress.Xpf.Core;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for winCOAAdminPanel.xaml
    /// </summary>
    public partial class winCOAAdminPanel : DXWindow
    {
        List<Company> allCompanies = new List<Company>();
        List<Company> selectedCompanies = new List<Company>(); 
        List<Department> allDepartments = new List<Department>();
        List<Department> selectedDepartments = new List<Department>();
        List<ERP_BL.Databases.Employee> allEmployees = new List<ERP_BL.Databases.Employee>();
        List<ERP_BL.Databases.Employee> selectedEmployees = new List<ERP_BL.Databases.Employee>();
        List<ChartofAccount> allChartofAccounts = new List<ChartofAccount>();
        List<ChartofAccount> selectedChartofAccounts = new List<ChartofAccount>();

        List<cmbitem> allAccountTypes = new List<cmbitem>();
        List<cmbitem> selectedAccountTypes = new List<cmbitem>();
        ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
        CoaLogicClass ModuleLogic = new CoaLogicClass();
        User user = new User();
        UsersRepo rolesRepo = new UsersRepo();

        public winCOAAdminPanel()
        {
            InitializeComponent();
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

                gridCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;

                var depts = selectedDepartments.Except(allDepartments.Intersect(selectedDepartments));
                allDepartments = new List<Department>();
                foreach (var _cmpny in selectedCompanies)
                {
                    allDepartments.AddRange(_cmpny.departments);
                }


                allDepartments = allDepartments.Except(depts).ToList();

                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                gridDepartment.ItemsSource = null;
                gridDepartment.ItemsSource = allDepartments;
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

            if (gridDepartment.SelectedItem != null)
            {
                var department = gridDepartment.SelectedItem as Department;
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

                    gridDepartment.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;
                    gridAllEmployee.ItemsSource = allEmployees;
                    gridDepartment.ItemsSource = allDepartments;
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
                    gridDepartment.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;

                    gridDepartment.ItemsSource = allDepartments;
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
                    gridSelectedEmployees.ItemsSource = null;
                    gridSelectedEmployees.ItemsSource = selectedEmployees;
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
            if (gridCompany.SelectedItem != null)
            {
                var company = gridCompany.SelectedItem as Company;
                selectedCompanies.Add(company);
                allCompanies.Remove(company);
                gridCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                gridCompany.ItemsSource = allCompanies;
                gridSelectedCompanies.ItemsSource = selectedCompanies;

                allDepartments.AddRange(company.departments.Except(selectedDepartments));

                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                gridDepartment.ItemsSource = null;
                gridDepartment.ItemsSource = allDepartments;
            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

 
            foreach (COA_AccountType name in Enum.GetValues(typeof(COA_AccountType)))
            {
                cmbitem item = new cmbitem();
                item.id =Convert.ToInt32(name);
                item.name =name.ToString();
                allAccountTypes.Add(item);
            }
            gridAccountTypes.ItemsSource = allAccountTypes;

            //var userCompanies=coaRepo.GetEmployeeCompanies(SYSTEM_STATIC.currentUser.employeeId);
            allCompanies = SYSTEM_STATIC.currentUser.employee.Companies;
            //allCompanies = userCompanies;
            gridCompany.ItemsSource = allCompanies;
        }

        private void imgLeftToRightType_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightType.Width = 30;
            if (gridAccountTypes.SelectedItem != null)
            {
                var type = gridAccountTypes.SelectedItem as cmbitem;
                selectedAccountTypes.Add(type);
                allAccountTypes.Remove(type);
                gridAccountTypes.ItemsSource = null;
                gridSelectedAccountTypes.ItemsSource = null;
                gridAccountTypes.ItemsSource = allAccountTypes;
                gridSelectedAccountTypes.ItemsSource = selectedAccountTypes;
                allChartofAccounts.AddRange(coaRepo.getAccountsByType(SYSTEM_STATIC.currentUser.employeeId, (COA_AccountType)Enum.Parse(typeof(COA_AccountType), type.name)));


                //allDepartments.AddRange(company.departments.Except(selectedDepartments));

                allChartofAccounts = allChartofAccounts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                gridAllChartofAccounts.ItemsSource = allChartofAccounts;

            }
            else
            {
                DXMessageBox.Show("Please select Account Type which you want to Insert to Selected Account Types!");
            }
        }

        private void imgLeftToRightType_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightType.Width = 28;
        }

        private void imgRightToLeftType_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftType.Width = 28;
        }

        private void imgRightToLeftType_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftType.Width = 30;
            if (gridSelectedAccountTypes.SelectedItem != null)
            {
                var type = gridSelectedAccountTypes.SelectedItem as cmbitem;
                selectedAccountTypes.Remove(type);
                allAccountTypes.Add(type);
                gridAccountTypes.ItemsSource = null;
                gridSelectedAccountTypes.ItemsSource = null;

                gridAccountTypes.ItemsSource = allAccountTypes;
                gridSelectedAccountTypes.ItemsSource = selectedAccountTypes;

                var COAs = selectedChartofAccounts.Except(allChartofAccounts.Intersect(selectedChartofAccounts));
                allChartofAccounts = new List<ChartofAccount>();



                foreach (var _type in selectedAccountTypes)
                {
                    allChartofAccounts.AddRange(coaRepo.getAccountsByType(SYSTEM_STATIC.currentUser.employeeId, (COA_AccountType)Enum.Parse(typeof(COA_AccountType), _type.name)));
                }


                allChartofAccounts = allChartofAccounts.Except(COAs).ToList();

                allChartofAccounts = allChartofAccounts.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                gridAllChartofAccounts.ItemsSource = null;
                gridAllChartofAccounts.ItemsSource = allChartofAccounts;
                gridSelectedChartofAccounts.ItemsSource = null;
                gridSelectedChartofAccounts.ItemsSource = selectedChartofAccounts;
            }
            else
            {
                DXMessageBox.Show("Please select Account type which you want to Insert to All Account Types!");
            }
        }

        private void imgLeftToRightEmp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightEmp.Width = 28;

        }

        private void imgRightToLeftEmp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftEmp.Width = 30;
            if (gridSelectedEmployees.SelectedItem != null)
            {
                var emp = gridSelectedEmployees.SelectedItem as ERP_BL.Databases.Employee;
                selectedEmployees.Remove(emp);
                allEmployees.Add(emp);
                gridAllEmployee.ItemsSource = null;
                gridSelectedEmployees.ItemsSource = null;
                gridAllEmployee.ItemsSource = allEmployees;
                gridSelectedEmployees.ItemsSource = selectedEmployees;
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
                gridSelectedEmployees.ItemsSource = null;

                gridAllEmployee.ItemsSource = allEmployees;
                gridSelectedEmployees.ItemsSource = selectedEmployees;
            }
            else
            {
                DXMessageBox.Show("Please select Employee which you want to Insert to Selected Employees!");
            }
        }

        private void imgLeftToRightCOA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCOA.Width = 28;

        }

        private void imgRightToLeftCOA_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCOA.Width = 30;

        }

        private void imgLeftToRightCOA_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCOA.Width = 30;

            if (gridAllChartofAccounts.SelectedItem != null)
            {
                var coa = gridAllChartofAccounts.SelectedItem as ChartofAccount;
                if (allChartofAccounts.Find(x => x.parentId == coa.Id) == null)
                {

                    allChartofAccounts.Remove(coa);
                    if (!selectedChartofAccounts.Contains(coa))
                        selectedChartofAccounts.Add(coa);

                    var parent = coa.parent;
                    while (parent != null)
                    {
                        if (allChartofAccounts.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedChartofAccounts.Contains(parent))
                            {
                                selectedChartofAccounts.Add(parent);
                            }
                            var findParet = allChartofAccounts.Find(x => x.parent == parent);
                            if (findParet == null)
                            {
                                allChartofAccounts.Remove(parent);
                            }
                        }
                        parent = parent.parent;
                    }
                    gridAllChartofAccounts.ItemsSource = null;
                    gridSelectedChartofAccounts.ItemsSource = null;
                    gridAllChartofAccounts.ItemsSource = allChartofAccounts;
                    gridSelectedChartofAccounts.ItemsSource = selectedChartofAccounts;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }
            }
            else
            {
                DXMessageBox.Show("Please select Chart of Account which you want to Insert to Selected Chart of Account!");
            }
        }

        private void imgRightToLeftCOA_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCOA.Width = 28;

            if (gridSelectedChartofAccounts.SelectedItem != null)
            {
                var coa = gridSelectedChartofAccounts.SelectedItem as ChartofAccount;

                if (selectedChartofAccounts.Find(x => x.parentId == coa.Id) == null)
                {
                    selectedChartofAccounts.Remove(coa);
                    if (!allChartofAccounts.Contains(coa))
                        allChartofAccounts.Add(coa);

                    var parent = coa.parent;
                    while (parent != null)
                    {
                        if (selectedChartofAccounts.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!allChartofAccounts.Contains(parent))
                                allChartofAccounts.Add(parent);
                            var findParet = selectedChartofAccounts.Find(x => x.parent == parent);
                            if (findParet == null)
                            {
                                selectedChartofAccounts.Remove(parent);
                            }
                        }
                        parent = parent.parent;
                    }
                    gridAllChartofAccounts.ItemsSource = null;
                    gridSelectedChartofAccounts.ItemsSource = null;

                    gridAllChartofAccounts.ItemsSource = allChartofAccounts;
                    gridSelectedChartofAccounts.ItemsSource = selectedChartofAccounts;
                }
                else
                {
                    DXMessageBox.Show("Parent cannot be Move!");
                }

            }
            else
            {
                DXMessageBox.Show("Please select Chart of Account which you want to Remove from Selected Chart of Account !");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ChartofAccount> coas = new List<ChartofAccount>();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                });
                foreach (var chartofAccount in selectedChartofAccounts)
                {
                    //var chartofAccount = coaRepo.get(_chartofAccount.Id);
                    //chartofAccount.Companies.Clear();
                    chartofAccount.Companies.AddRange(GetCompanies());
                    //chartofAccount.Departments.Clear();
                    chartofAccount.Departments.AddRange(GetDepartments());
                    //chartofAccount.Employees.Clear();
                    chartofAccount.Employees.AddRange(GetEmployees());

                    chartofAccount.Companies.GroupBy(x => x).Select(d => d.First()).ToList();
                    chartofAccount.Departments.GroupBy(x => x).Select(d => d.First()).ToList();
                    chartofAccount.Employees.GroupBy(x => x).Select(d => d.First()).ToList();


                    if (chartofAccount.Id != 0 && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Chart of Account") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Unapproved Chart of Account") != null))
                    {
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without Approval") != null && chartofAccount.isApproved != true)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Chart of Account is in Pending State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                chartofAccount.stage = TransactionStage.Approved.ToString();
                                chartofAccount.isApproved = true;
                                chartofAccount.ApprovedDate = System.DateTime.Now;
                            }
                        }
                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Chart of Account without ReApproval") != null && chartofAccount.isReApproved != true)
                        {
                            if (DevExpress.Xpf.Core.DXMessageBox.Show("This Chart of Account is in Re-Approval State! Do you want to Approve it?", "Approval Required", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                chartofAccount.stage = TransactionStage.Approved.ToString();
                                chartofAccount.isReApproved = true;
                                chartofAccount.ReApprovalDate = System.DateTime.Now;
                            }
                        }
                        coas.Add(chartofAccount);
                    }
                }
                coaRepo.Update(coas);
                user = null;
                if (user == null)
                {
                    user = rolesRepo.getuserbyUsername(SYSTEM_STATIC.currentUser.userName);
                }
                if (SYSTEM_STATIC.currentUser != null)
                {
                    BackgroundWorker worker = new BackgroundWorker();
                    user = rolesRepo.getuserbyUsername(SYSTEM_STATIC.currentUser.userName);
                    SYSTEM_STATIC.PopulateTransactionPanel();
                    SYSTEM_STATIC.currentUser = user;
                    worker.DoWork += BgWorker_SystemStatic;
                    if (!worker.IsBusy)
                        worker.RunWorkerAsync();
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                });
                DXMessageBox.Show("Chart of Accounts are Updated Successfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception)
            {
            }
        }
        private void BgWorker_SystemStatic(object sender, DoWorkEventArgs e)
        {

            if (user != null)
            {
                user.Roles = new List<Role>();
                user.Roles = rolesRepo.getUserRoles(user.id).Distinct().ToList();
                
                SYSTEM_STATIC.AllowedPermissions = new List<Permission>();
                SYSTEM_STATIC.currentUserRoles = new List<Role>();

                //var depts = SYSTEM_STATIC.currentUser.employee.Companies[1].departments.Where(x=>x.DeptName.StartsWith("PRI")).ToList();
                if (user.Roles != null)
                    foreach (Role role in user.Roles)
                    {
                        SYSTEM_STATIC.currentUserRoles.Add(role);
                        if (role.Permissions != null)
                            SYSTEM_STATIC.AllowedPermissions.AddRange(role.Permissions.Distinct().ToList());
                        else
                            SystemLog.LogInfo(this.GetType(), "Role Name = " + role.Name + " had Permissions = null");
                    }
            }
            SYSTEM_STATIC.isLoadingPermissions = false;
            SYSTEM_STATIC.LoadOutlookEmails();

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

            DepartmentRepo departmentRepo = new DepartmentRepo();
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
            EmployeeRepo employeeRepo = new EmployeeRepo();
            var grdEmployees = gridSelectedEmployees.ItemsSource as List<ERP_BL.Databases.Employee>;
            foreach (ERP_BL.Databases.Employee employee in grdEmployees)
            {
                employees.Add(employee);
            }
            return employees;
        }

        private void DXWindow_Closing(object sender, CancelEventArgs e)
        {
            //EmployeeRepo employeeRepo = new EmployeeRepo();
            //CompanyRepo companyRepo = new CompanyRepo();
            //var employee =  employeeRepo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            //foreach (var _company in selectedCompanies)
            //{
            //    var userCompany = companyRepo.GetCompany(_company.Id);
            //    if (!userCompany.employees.Contains(employee))
            //    {
            //        userCompany.employees.Add(employee);
            //    }
            //    companyRepo.updateCompany(userCompany);
            //}
            //user = SYSTEM_STATIC.currentUser;
            //if (user == null)
            //{
            //    user = rolesRepo.getuserbyUsername(SYSTEM_STATIC.currentUser.userName);
            //    if (user == null)
            //        MessageBox.Show("Invalid UserName, Please Enter a Valid UserName", "Invalid credentials", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //if (SYSTEM_STATIC.currentUser != null)
            //{
            //    BackgroundWorker worker = new BackgroundWorker();
            //    user = rolesRepo.getuserbyUsername(SYSTEM_STATIC.currentUser.userName);
            //    SYSTEM_STATIC.PopulateTransactionPanel();
            //    SYSTEM_STATIC.currentUser = user;
            //    worker.DoWork += BgWorker_SystemStatic;
            //    if (!worker.IsBusy)
            //        worker.RunWorkerAsync();
            //}
        }
    }
}
