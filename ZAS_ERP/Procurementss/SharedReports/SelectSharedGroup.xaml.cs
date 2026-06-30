using DevExpress.Xpf.Core;
using ERP_BL.Reports;
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

namespace ZAS_ERP.Procurementss.SharedReports
{
    /// <summary>
    /// Interaction logic for SelectSharedGroup.xaml
    /// </summary>
    public partial class SelectSharedGroup : DXWindow
    {
        List<int> deptIds = new List<int>();
        List<int> compIds = new List<int>();
        List<int> empIds = new List<int>();
        List<SharedGridGroup> groupsWithCompany = new List<SharedGridGroup>();
        List<SharedGridGroup> groupsWithDepartment = new List<SharedGridGroup>();
        List<SharedGridGroup> groupsWithEmployee = new List<SharedGridGroup>();
        GridReport gridReport = new GridReport();
        SharedReport sharedReport = new SharedReport();
        public SelectSharedGroup(GridReport _gridReport)
        {
            InitializeComponent();
            gridReport = _gridReport;
        }
        public SelectSharedGroup(SharedReport _sharedReport)
        {
            InitializeComponent();
            sharedReport = _sharedReport;
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            compIds = SYSTEM_STATIC.LoadCurrentUserCompanies().Select(x => x.Id).ToList();
            deptIds = SYSTEM_STATIC.LoadCurrentUserDepartments().Select(x => x.Id).ToList();

           
            if(sharedReport.Id!=0)
            {
                LoadGroups(sharedReport.settingkey);
                txtReportName.Text = sharedReport.reportName;

                lookupGroup.Text = sharedReport.SharedGridGroup.groupName;
            }
            else
                LoadGroups(gridReport.settingkey);
        }
        private void LoadGroups(string reportCategory)
        {
            GridReportRepo repo = new GridReportRepo();
            try
            {
                var pos = reportCategory.IndexOf("/");
                string _str = reportCategory;
                if (pos != -1)
                {
                     _str = reportCategory.Substring(0, pos);
                }
                else
                {

                    var allsahredGroups = repo.GetAllSharedGroups();
                    switch (_str)
                    {

                        case "Inquiries":

                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Inquiries").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Offers":

                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Offers").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Sale Orders":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Sale Orders").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Sale Register":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Sale Orders").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Purchase Orders":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Purchase Orders").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Purchase Register":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Purchase Orders").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Bills":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Bills").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Bill Register":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Bills").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Admin Bill":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Bills").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Admin Bill Register":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Admin Bills").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Sale Invoices":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Sale Invoices").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Sales Invoice Register":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Sale Invoices").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Purchase Invoices":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Purchase Invoices").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Purchase Invoice Register":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Purchase Invoices").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Inter-Bank Transfer Register":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Inter-Bank Transfers").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Payments":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Payments").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;
                        case "Step Register":
                            allsahredGroups = allsahredGroups.Where(x => x.isVoid != true && x.Parent != null).ToList();
                            if (allsahredGroups.Count != 0)
                            {
                                allsahredGroups = allsahredGroups.Where(x => x.Parent.groupName == "Todo Tasks").ToList();
                                foreach (var group in allsahredGroups)
                                {
                                    if (group.Companies.Count != 0)
                                    {
                                        foreach (var company in group.Companies)
                                        {
                                            if (compIds.Contains(company.Id))
                                            {
                                                groupsWithCompany.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithCompany.Add(group);
                                }
                                foreach (var group in groupsWithCompany)
                                {
                                    if (group.Departments.Count != 0)
                                    {
                                        foreach (var department in group.Departments)
                                        {
                                            if (deptIds.Contains(department.Id))
                                            {
                                                groupsWithDepartment.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithDepartment.Add(group);
                                }
                                foreach (var group in groupsWithDepartment)
                                {
                                    if (group.Employees.Count != 0)
                                    {
                                        empIds = group.Employees.Select(x => x.EmpId).Distinct().ToList();
                                        foreach (var department in group.Departments)
                                        {
                                            if (empIds.Contains(SYSTEM_STATIC.currentUser.employeeId))
                                            {
                                                groupsWithEmployee.Add(group);
                                                break;
                                            }
                                        }
                                    }
                                    else
                                        groupsWithEmployee.Add(group);
                                }
                                lookupGroup.ItemsSource = groupsWithEmployee;
                            }
                            break;

                    }
                }
                
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnreportGroupSave_Click(object sender, RoutedEventArgs e)
        {
            GridReportRepo repo = new GridReportRepo();
            if (sharedReport.Id != 0)
            {
                var dbReport = repo.GetSharedReport(sharedReport.Id);
                dbReport.reportName = txtReportName.Text;
                dbReport.lastModified = DateTime.Now;
                dbReport.sharedGroupId = (lookupGroup.SelectedItem as SharedGridGroup).Id;
                repo.RenameReport(dbReport);
                DXMessageBox.Show("Shared Report Renamed Successfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {


                if (string.IsNullOrEmpty(txtReportName.Text))
                {
                    DXMessageBox.Show("Report name cannot be null or empty", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    var dbReport = repo.GetSharedReportByName(txtReportName.Text);
                    if (dbReport == null)
                    {
                        SharedReport report = new SharedReport();
                        report.userId = gridReport.userId;
                        report.lastModified = gridReport.lastModified;
                        report.reportName = txtReportName.Text;
                        report.settingkey = gridReport.settingkey;
                        report.settingValue = gridReport.settingValue;
                        report.sharedGroupId = (lookupGroup.SelectedItem as SharedGridGroup).Id;
                        repo.AddSharedReport(report);
                        DXMessageBox.Show("Shared Report Added Successfully", "Congratulations", MessageBoxButton.OK, MessageBoxImage.Information);

                        this.Close();
                    }
                    else
                    {
                        DXMessageBox.Show("Report with name" + " " + txtReportName.Text + " Alredy Exists please give different name", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                }
            }
        }
    }
}
