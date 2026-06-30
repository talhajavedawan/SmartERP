using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.Employeess
{
    /// <summary>
    /// Interaction logic for frmItemadd.xaml
    /// </summary>
    public partial class frmDesignationadd : Window
    {
        EmployeeRepo repo = new EmployeeRepo();
        ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        ERP_BL.Databases.Company company = new ERP_BL.Databases.Company();
        Department department = new Department();
        Designation designation = new Designation();
        Designation parentDesignation = new Designation();
        public static int designationId;

        public frmDesignationadd()
        {
            InitializeComponent();
        }
  



        private void btnSaveDesignation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtTitle.Text == "")
                {
                    MessageBox.Show("Enter Desig Title");
                    txtTitle.Focus();
                    return;
                }
                else if (company != null)
                {
                    MessageBox.Show("Select Company");
                    lookupCompany.Focus();
                    return;

                }
                else if (department == null)
                {
                    MessageBox.Show("Select Department");
                    lookupDepartment.Focus();
                    return;

                }
                
                {
                    designation.Title = txtTitle.Text.Trim();
                    designation.companyId = company.Id;
                    designation.departmentId = department.Id;
                    if (parentDesignation != null)
                        designation.parentDesignation = parentDesignation;
                    
                    if (MainWindow.currentUserid != 0)
                        designation.userId = MainWindow.currentUserid;
                    //product.categoryId = category.Id;
                    
                    if (designation.DesigId == 0)
                    {
                        repo.addDesignation(designation);

                        DXMessageBox.Show(txtTitle.Text + " Added Succesfully!");
                        this.Close();

                    }
                    else
                    {
                        repo.updateDesignation(designation);

                        MessageBox.Show(txtTitle.Text + " Updated Succesfully!");
                        this.Close();

                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void winDesignationAdd_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            designationId= 0;
        }

        private void winDesignationAdd_Loaded(object sender, RoutedEventArgs e)
        {
            loadcompanies();
            
            loaditeminfo();
        }

        public void loaddepartments()
        {
            if (MainWindow.currentUserid == 0)
            {
                DepartmentRepo departmentRepo = new DepartmentRepo();
                this.lookupDepartment.ItemsSource = departmentRepo.GetDepartments();
                return;
            }

            if (company != null)
                if (company.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    foreach (Department dep in company.departments)
                        foreach (Department empdep in employee.departments)
                            if (dep.Id == empdep.Id)
                            {
                                departments.Add(dep);
                            }
                    lookupDepartment.ItemsSource = departments;

                    if (departments.Count == 0)
                    {
                        MessageBox.Show("This company dosen't contain any department mapped with the current User");
                    }
                }
        }
        public void loadcompanies()
        {
            if (MainWindow.currentUserid == 0)
            {
                CompanyRepo cont = new CompanyRepo();

                this.lookupCompany.ItemsSource = cont.GetCompanies();
                return;
                //List<ERP_BL.Databases.Company> companies = new List<ERP_BL.Databases.Company>();
                //foreach (ERP_BL.Databases.Company comp in companylist)
                //    if (comp.compnayType == ERP_BL.Enums.CompnayTypes.Company)
                //        foreach (ERP_BL.Databases.Company empcomp in empUser.Companies)
                //            if (comp.Id == empcomp.Id)
                //                companies.Add(comp);
                //this.lookupCompany.ItemsSource = companies;
            }

           // var usernew = repo.getuser(MainWindow.currentUserid);
            employee = repo.GetEmployee(SYSTEM_STATIC.currentUser.employeeId);
            lookupCompany.ItemsSource = employee.Companies;

        }
        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            company = lookupCompany.SelectedItem as ERP_BL.Databases.Company;
            if (company != null)
            {
                string selecteddept = company.CompanyName;
                //lookupCompany.EditValue = selecteddept;

                loaddepartments();
            }

        }
        private void lookupDepartment_GotFocus(object sender, RoutedEventArgs e)
        {
            if (company.Id == 0)
            {
                MessageBox.Show("Select Company Frist");
                lookupCompany.Focus();
                return;
            }
        }

        private void lookupDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            department = lookupDepartment.SelectedItem as Department;
            if (department != null)
            {
                string selecteddept = department.DeptName + " (" + department.Code + ")";
                //lookupDepartment.EditValue = selecteddept;


                //lookupCustomer.ItemsSource = department.customers;
                

            }
        }
        public void loaditeminfo()
        {
            if (designationId != 0)
            {
                designation = repo.getDesignation(designationId);
                txtTitle.Text = designation.Title;
                //txtItemDiscription.Text = product.itemDescription;
                //txtItemName.Text = product.item;
                
                if (designation.companyId != 0 && designation.company != null)
                {
                    lookupCompany.Text = designation.company.CompanyName;

                    lookupCompany.SelectedItem = lookupCompany.GetItemByKeyValue(designation.company);

                    company = designation.company;
                }
                if (designation.departmentId != 0 && designation.department!= null)
                {
                    lookupDepartment.Text = designation.department.DeptName;

                    lookupDepartment.SelectedItem = lookupCompany.GetItemByKeyValue(designation.department);

                    department= designation.department;
                }
                if (designation.ParentId != 0 && designation.parentDesignation!= null)
                {
                    lookupParentDesignation.Text = designation.parentDesignation.Title;

                    lookupParentDesignation.SelectedItem = lookupCompany.GetItemByKeyValue(designation.parentDesignation);

                    parentDesignation = designation.parentDesignation;
                }
            }
        }


        private void LookupParentDesignation_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            parentDesignation = lookupParentDesignation.SelectedItem as Designation;
            if (parentDesignation != null)
            {
                string selecteddept = parentDesignation.Title ;
                lookupParentDesignation.EditValue = selecteddept;


                


            }
        }
    }
}
