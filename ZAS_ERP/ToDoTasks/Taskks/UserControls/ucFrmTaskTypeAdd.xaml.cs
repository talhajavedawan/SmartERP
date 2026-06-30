using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.ToDoTasks.Taskss;
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

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls
{
    /// <summary>
    /// Interaction logic for ucFrmTaskTypeAdd.xaml
    /// </summary>
    public partial class ucFrmTaskTypeAdd : UserControl
    {
        TaskRepo repo = new TaskRepo();
        TaskType taskType = new TaskType();
        public int taskTypeId = 0;
        public bool editFlag = false;

        List<Company> allCompanies = new List<Company>();
        List<Company> selectedCompanies = new List<Company>();

        List<Department> allDepartments = new List<Department>();
        List<Department> selectedDepartments = new List<Department>();
        User loginUser = new User();
        public bool isProcurementType = false;
        public ucFrmTaskTypeAdd()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdStatus.ItemsSource = repo.GetAllTaskStatusesForRegister();

            CompanyRepo companyRepo = new CompanyRepo();
            allCompanies = companyRepo.GetUserCompanies(SYSTEM_STATIC.currentUser.id);

            if(isProcurementType == true)
            {
                gridCompDept.Visibility = Visibility.Collapsed;
                gridProcurement.Visibility = Visibility.Visible;
            }
            else
            {
                gridCompDept.Visibility = Visibility.Visible;
                gridProcurement.Visibility = Visibility.Collapsed;
            }
            

            populatefIelds();
        }

        public void populatefIelds()
        {
            if (editFlag == true && taskTypeId > 0)
            {
                taskType = repo.GetTaskType(taskTypeId);
                txtTaskType.Text = taskType.TypeName;

                if (taskType.TaskStatusess != null)
                {
                    foreach (TasksStatus _status in taskType.TaskStatusess)
                        grdStatus.SelectItem(grdStatus.FindRow(_status));
                }

                if(taskType.isProcurementType == false)
                {
                    foreach (var _company in taskType.companies)
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
                    foreach (var _dept in taskType.departments)
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
                else
                {
                    if (taskType.isSaleOrder == true)
                        chkSaleOrder.IsChecked = true;

                    if (taskType.isPurchaseOrder == true)
                        chkPurchaseOrder.IsChecked = true;

                    if (taskType.isSaleInvoice == true)
                        chkSaleInvoice.IsChecked = true;

                    if (taskType.isOffer == true)
                        chkOffer.IsChecked = true;

                    if (taskType.isInquiry == true)
                        chkInquiry.IsChecked = true;                           
                }
            }
            gridCompany.ItemsSource = allCompanies;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtTaskType.Text))
            {
                DXMessageBox.Show("Please enter Applicant Name!");
                txtTaskType.Focus();
                return;
            }

            taskType.TypeName = txtTaskType.Text;
            taskType.isProcurementType = isProcurementType;
            if (isProcurementType == false)
            {
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

                taskType.companies = selectedCompanies;
                taskType.departments = selectedDepartments;
            }
            else
            {
                if (chkSaleOrder.IsChecked == true)
                {
                    taskType.isSaleOrder = true;
                }
                else
                {
                    taskType.isSaleOrder = false;
                }

                if (chkPurchaseOrder.IsChecked == true)
                {
                    taskType.isPurchaseOrder = true;
                }
                else 
                {
                    taskType.isPurchaseOrder = false;
                }

                if (chkSaleInvoice.IsChecked == true)
                {
                    taskType.isSaleInvoice = true;
                }
                else 
                {
                    taskType.isSaleInvoice = false;
                }

                if (chkOffer.IsChecked == true)
                {
                    taskType.isOffer = true;
                }
                else 
                {
                    taskType.isOffer = false;
                }

                if (chkInquiry.IsChecked == true)
                {
                    taskType.isInquiry = true;
                }
                else 
                {
                    taskType.isInquiry = false;
                }
            }


            if (grdStatus.SelectedItems.Count > 0)
            {
                taskType.TaskStatusess = new List<TasksStatus>();
                foreach (TasksStatus _status in grdStatus.SelectedItems)
                {
                    if (taskType.TaskStatusess == null)
                        taskType.TaskStatusess = new List<TasksStatus>();

                    if (taskType.TaskStatusess.Find(x => x.Id == _status.Id) == null)
                    {
                        taskType.TaskStatusess.Add(_status);
                    }
                }
            }
            else
            {
                taskType.TaskStatusess = null;
            }
                

            if (editFlag == false && taskTypeId == 0)
            {
                repo.AddTaskType(taskType);
                DXMessageBox.Show("Successfully Added!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (editFlag == true && taskTypeId != 0)
            {
                repo.UpdateTaskType(taskType);
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
