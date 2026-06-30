using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.Procurements.LoansAdvances;
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

namespace ZAS_ERP.Procurementss.LoanAdvance.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmLender.xaml
    /// </summary>
    public partial class ucFrmLender : UserControl
    {
        AdvanceRepo repo = new AdvanceRepo();
        LoanApplicant applicant = new LoanApplicant();
        public int applicantId = 0;
        public bool editFlag = false;

        List<Company> allCompanies = new List<Company>();
        List<Company> selectedCompanies = new List<Company>();

        List<Department> allDepartments = new List<Department>();
        List<Department> selectedDepartments = new List<Department>();
        User loginUser = new User();
        public ucFrmLender()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CompanyRepo companyRepo = new CompanyRepo();
            allCompanies = companyRepo.GetUserCompanies(SYSTEM_STATIC.currentUser.id);

            //allCompanies = loginUser.employee.Companies;

            lookupApplicantType.ItemsSource = repo.GetAllLenderTypes();

            populatefIelds();
        }

        public void populatefIelds()
        {
            if (editFlag == true && applicantId > 0)
            {
                applicant = repo.GetApplicant(applicantId);
                txtApplicantName.Text = applicant.Name;

                if (applicant.applicantType != null)
                    lookupApplicantType.EditValue = applicant.applicantTypeId;


                foreach (var _company in applicant.companies)
                {
                    var cmpny = allCompanies.FirstOrDefault(x => x.Id == _company.Id);
                    if (_company != null)
                    {
                        selectedCompanies.Add(cmpny);
                        allCompanies.Remove(cmpny);
                        allDepartments.AddRange(cmpny.departments);
                    }

                }

                allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                foreach (var _dept in applicant.departments)
                {
                    var dept = allDepartments.FirstOrDefault(x => x.Id == _dept.Id);
                    if (dept != null)
                    {
                        selectedDepartments.Add(dept);
                        if (allDepartments.Find(x => x.ParentID == dept.Id) == null)
                        {
                            allDepartments.Remove(dept);
                        }
                    }

                }

                gridDepartment.ItemsSource = allDepartments;
                gridSelectedDepartments.ItemsSource = selectedDepartments;
                gridSelectedCompanies.ItemsSource = selectedCompanies;
            }

            gridCompany.ItemsSource = allCompanies;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtApplicantName.Text))
            {
                DXMessageBox.Show("Please enter Applicant Name!");
                txtApplicantName.Focus();
                return;
            }
            if (lookupApplicantType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Applicant Type!");
                lookupApplicantType.Focus();
                return;
            }
            if (gridSelectedCompanies.ItemsSource == null || gridSelectedCompanies.VisibleItems.Count == 0)
            {
                DXMessageBox.Show("Please select Companies!");
                gridSelectedCompanies.Focus();
                return;
            }
            if (gridSelectedDepartments.ItemsSource == null || gridSelectedDepartments.VisibleItems.Count == 0)
            {
                DXMessageBox.Show("Please select Departments!");
                gridSelectedDepartments.Focus();
                return;
            }

            applicant.LenderType = true;
            applicant.Name = txtApplicantName.Text;
            applicant.applicantTypeId = (lookupApplicantType.SelectedItem as LoanApplicantType).Id;


            applicant.companies = gridSelectedCompanies.ItemsSource as List<Company>;
            applicant.departments = gridSelectedDepartments.ItemsSource as List<Department>;

            if (editFlag == false && applicantId == 0)
            {
                repo.AddApplicant(applicant);
                DXMessageBox.Show("Successfully Added!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (editFlag == true && applicantId != 0)
            {
                repo.UpdateApplicant(applicant);
                DXMessageBox.Show("Updated Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }



        private void ImgLeftToRightComp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightComp.Width = 28;
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

        private void ImgRightToLeftComp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftComp.Width = 28;
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

        private void ImgLeftToRightDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightDept.Width = 28;
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
                                selectedDepartments.Add(parent);
                            var findParet = allDepartments.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                allDepartments.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }

                    gridDepartment.ItemsSource = null;
                    gridSelectedDepartments.ItemsSource = null;

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

        private void ImgRightToLeftDept_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftDept.Width = 30;
        }

    }
}
