using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks.Taskss;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using ZAS_ERP.Procurementss;

namespace ZAS_ERP.ToDoTasks.Taskks.UserControls.TaxTasks
{
    /// <summary>
    /// Interaction logic for ucTaxTaskAdd.xaml
    /// </summary>
    public partial class ucTaxTaskAdd : UserControl
    {
        public bool editFlag = false;
        public int taskId = 0;
        Tasks task = new Tasks();
        TaskRepo taskRepo = new TaskRepo();

        UsersRepo UsersRepo = new UsersRepo();
        List<ViewInfo> views = new List<ViewInfo>();

        public TasksStatus checkStatus = new TasksStatus();
        TasksStatus oldStatus = new TasksStatus();
        static TasksStatus statusChanged = new TasksStatus();
        List<User> AllAllowedUser = new List<User>();
        List<User> SelectedAllowedUser = new List<User>();

        List<TaskEfficiency> taskEfficiencies = new List<TaskEfficiency>();
        public ucTaxTaskAdd()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTaskTemplate();
            LoadCompanies();
            //LoadTaskStatuses();
            SetValuesforPaymentyearandQuarter();
            //cmbxTaskTemplate.IsReadOnly = true;
            if (editFlag == false && taskId == 0)
            {
                btnSaveTracking.IsEnabled = false;
                txtCreator.Text = SYSTEM_STATIC.currentUser.employee.person.FName + " " + SYSTEM_STATIC.currentUser.employee.person.LName;
                datCreationDate.DateTime = DateTime.Now;

                grdCntrlUsersSelected.ItemsSource = SelectedAllowedUser;
            }

            if (editFlag == true && taskId > 0)
            {
                task = taskRepo.GetTask(taskId);

                btnSaveTracking.IsEnabled = (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Tracking Details") != null) ? true : false;


                if (task.isApproved != true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit User Task Under Approval") != null)
                {
                    btnSave.IsEnabled = true;
                }
                else if (task.isApproved == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Tax Tasks") != null)
                {
                    btnSave.IsEnabled = true;
                }
                else
                {
                    btnSave.IsEnabled = false;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Efficiency Points in Tasks") != null)
                    btnAddEfficiencyPoints.Visibility = Visibility.Visible;

                taskEfficiencies = task.TaskEfficiencies;
                if (taskEfficiencies != null)
                {
                    var achievedPoints = taskEfficiencies.Sum(x => x.AchievedPoints);
                    var totalPoints = taskEfficiencies.Where(x => x.TotalPoints >= 0).Sum(x => x.TotalPoints);
                    txtEfficiencyPoints.Text = achievedPoints + "/" + totalPoints;
                }

                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can Add Users in Tasks (Not creator of the Task)") != null || task.creatorId == SYSTEM_STATIC.currentUser.id)
                {
                    tabUsers.Visibility = Visibility.Visible;
                }
                else
                {
                    tabUsers.Visibility = Visibility.Collapsed;
                }

                //if (task.isVoid == true)
                //{
                //    grdVoid.Visibility = Visibility.Visible;
                //    txtVoid.RenderTransform = new RotateTransform(-45);
                //}
                //else if(task.Status.isActive == true)
                //{
                //    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                //}
                //else if (task.Status.isActive == false)
                //{
                //    grdApproved.Background = Brushes.DeepSkyBlue;
                //    grdClosed.Background = Brushes.DeepSkyBlue;
                //}

                if (task.isVoid == true)
                {
                    grdVoid.Visibility = Visibility.Visible;
                    txtVoid.RenderTransform = new RotateTransform(-45);
                    //lblStage.Text = "Void";
                }
                else if (task.isReApproved == false)
                {
                    //lblStage.Text = "Under Re-Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (task.isApproved != false && task.stage == "Closed")
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (task.isApproved != false && task.Status.isActive == false && task.PendingForClosing != true)
                {
                    //lblStage.Text = "Approved and Closed";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.DeepSkyBlue;
                }
                else if (task.isApproved != false && task.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (task.isApproved != false)
                {
                    //lblStage.Text = "Approved";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (task.isApproved == false)
                {
                    //lblStage.Text = "Under Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.LightGray;
                    grdUnderClosing.Background = Brushes.LightGray;
                    grdClosed.Background = Brushes.LightGray;
                }
                else if (task.PendingForClosing == true)
                {
                    //lblStage.Text = "Under Closing Approval";

                    grdUnderApproval.Background = Brushes.DeepSkyBlue;
                    grdApproved.Background = Brushes.DeepSkyBlue;
                    grdUnderClosing.Background = Brushes.DeepSkyBlue;
                    grdClosed.Background = Brushes.LightGray;
                }


                grdTaskTracking.ItemsSource = task.TaskTrackings;

                lblRefNo.Text = task.SystemRef;

                //if (task.isCompleted == true)
                //{
                //    var curDate = task.StartDate.Value; new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                //    DateTime startDate = new DateTime(curDate.Year, curDate.Month, curDate.Day, 0, 0, 0);
                //    var date = task.CompletionDate.Value;
                //    DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                //    var timeSpan = startDate.Subtract(targetDate);


                //    txtDays.Text = "Completed In";
                //    txtDays.FontSize = 18;
                //    txtDaysRemaining.Text = System.Math.Abs(timeSpan.Days).ToString() + " Days";
                //    txtDaysRemaining.FontSize = 20;
                //    txtDaysRemaining.VerticalAlignment = VerticalAlignment.Center;
                //    txtDaysIndicator.Background = Brushes.DeepSkyBlue;

                //}
                //else
                //{
                //    DateTime currDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                //    var date = task.TentativeCompletionDate.Value;
                //    DateTime targetDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                //    var timeSpan = currDate.Subtract(targetDate);

                //    if (timeSpan.Days <= 0)
                //    {
                //        txtDays.Text = System.Math.Abs(timeSpan.Days).ToString();
                //        txtDaysRemaining.Text = "Days Remaining";
                //        txtDaysIndicator.Background = Brushes.DeepSkyBlue;
                //    }
                //    else
                //    {
                //        txtDays.Text = System.Math.Abs(timeSpan.Days).ToString();
                //        txtDaysRemaining.Text = "Days Passed";
                //        txtDaysIndicator.Background = Brushes.Red;
                //    }
                //}




                if (task.creationDate != null)
                    datCreationDate.DateTime = task.creationDate.Value;
                if (task.taskTemplate != null)
                    cmbxTaskTemplate.Text = task.taskTemplate.Value.ToString();


                if (task.TaxFrom != null)
                    datFrom.EditValue = task.TaxFrom.Value;
                if (task.TaxTo != null)
                    datTo.EditValue = task.TaxTo.Value;


                var companyList = (lookupCompany.ItemsSource as List<Company>) == null ? new List<Company>() : lookupCompany.ItemsSource as List<Company>;
                if (task.companyId != null)
                {
                    if (companyList.Find(x => x.Id == task.companyId) == null)
                    {
                        companyList.Add(task.company);
                        lookupCompany.ItemsSource = null;
                        lookupCompany.ItemsSource = companyList;
                    }


                    lookupCompany.EditValue = task.companyId;
                }


                var deptList = (lookupDepartment.ItemsSource as List<Department>) == null ? new List<Department>() : lookupDepartment.ItemsSource as List<Department>;
                if (task.deptId != null)
                {
                    if (deptList.Find(x => x.Id == task.deptId) == null)
                    {
                        deptList.Add(task.department);
                        lookupDepartment.ItemsSource = null;
                        lookupDepartment.ItemsSource = deptList;
                    }

                    lookupDepartment.EditValue = task.deptId;
                }

                var vendorList = (lookupVendor.ItemsSource as List<Vendor>) == null ? new List<Vendor>() : lookupVendor.ItemsSource as List<Vendor>;
                if (task.vendorId != null)
                {
                    if (vendorList.Find(x => x.Id == task.vendorId) == null)
                    {
                        vendorList.Add(task.vendor);
                        lookupVendor.ItemsSource = null;
                        lookupVendor.ItemsSource = vendorList;
                    }


                    lookupVendor.EditValue = task.vendorId;
                }

                var employeeList = (cmbEmployee.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbEmployee.ItemsSource as List<cmbitem>;
                if (task.employeeId != null)
                {
                    int index = 0;
                    foreach (var _emp in employeeList)
                    {
                        if (_emp.id == task.employeeId)
                        {
                            chkIsFilerEmployee.IsChecked = true;
                            cmbEmployee.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    chkIsFilerEmployee.IsChecked = false;
                    txtFilerName.Text = task.FilerName;
                }

                txtSystemRef.Text = task.SystemRef;
                txtTaskRef.Text = task.TaskRef;
                //txtEfficiencyPoints.Text = task.EfficiencyPoints.ToString();
                if (task.creator != null)
                    txtCreator.Text = task.creator.employee.person.FName + " " + task.creator.employee.person.LName;
                if (task.supervisedBy != null)
                    lookUpSupervisedBy.EditValue = task.supervisedBy.id;
                if (task.assignedBy != null)
                    lookUpAssignedBy.EditValue = task.assignedBy.id;
                if (task.assignedTo != null)
                    lookUpAssignedTo.EditValue = task.assignedTo.id;
                if (task.taskType != null)
                    lookupTaskType.EditValue = task.taskType.Id;

                //Select Status
                var statusList = (cmbStatus.ItemsSource as List<cmbitem>) == null ? new List<cmbitem>() : cmbStatus.ItemsSource as List<cmbitem>;
                if (task.Status != null)
                {
                    checkStatus = task.Status;
                    int index = 0;
                    foreach (var _status in statusList)
                    {

                        if (_status.id == task.statusId)
                        {
                            cmbStatus.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (task.taxableIncome != null)
                    txtTaxableIncome.Text = task.taxableIncome.ToString();
                if (task.chargeableTax != null)
                    txtTaxChargeable.Text = task.chargeableTax.ToString();
                txtNTNnumber.Text = task.NTNno;
                txtNoticeRef.Text = task.NoticeRefNo;
                if (task.taxYear != null)
                    datTaxYear.EditValue = task.taxYear;

                if (task.TaxQuarter == null)
                    SetValuesforPaymentyearandQuarter();
                else
                    spnQuarter.Text = task.TaxQuarter.ToString();

                if (task.depositedTax != null)
                    txtDepositedTax.Text = task.depositedTax.ToString();



                txtInputTax.Text = task.InputTax.ToString();
                txtOutputTax.Text = task.OutputTax.ToString();
                txtRefundAmount.Text = task.RefundAmountClaim.ToString();
                txtCreditCarriedForward.Text = task.CreditCarriedForward.ToString();
                txtAccumulatedCredit.Text = task.AccumulatedCredit.ToString();
                txtAccumulatedDebit.Text = task.AccumulatedDebit.ToString();
                txtFEDpayable.Text = task.FEDpayable.ToString();
                txtPLpayable.Text = task.PLpayable.ToString();
                txtSaleTaxPayable.Text = task.SaleTaxPayable.ToString();
                txtTotalAmountPaid.Text = task.TotalAmountPaid.ToString();



                foreach (var _selectedUser in task.AllowedUsers)
                {
                    SelectedAllowedUser.Add(_selectedUser);

                }
                grdCntrlUsersSelected.ItemsSource = null;
                grdCntrlUsersSelected.ItemsSource = SelectedAllowedUser;

                foreach (var rrr in SelectedAllowedUser)
                {
                    if (AllAllowedUser.Contains(rrr))
                    {
                        AllAllowedUser.Remove(rrr);
                    }
                }
                grdCntrlUsers.ItemsSource = null;
                grdCntrlUsers.ItemsSource = AllAllowedUser;


                txtDescription.Text = task.Description;
            }
        }

        private void LoadTaskStatuses()
        {
            var taskType = lookupTaskType.SelectedItem as TaskType;

            if(taskType != null)
            {
                List<cmbitem> cmbitems = new List<cmbitem>();
                //BillRepo billRepo = new BillRepo();
                List<TasksStatus> tasksStatuses = new List<TasksStatus>();


                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive User Tasks Statuses") != null)
                {
                    //tasksStatuses = taskRepo.GetAllTaskStatuses();
                    tasksStatuses = taskType.TaskStatusess;
                }
                else
                {
                    if (taskType.TaskStatusess.Count > 0)
                        //tasksStatuses = taskRepo.GetAllTaskStatuses().Where(x => x.isActive == true).ToList();
                        tasksStatuses = taskType.TaskStatusess.Where(x => x.isActive == true).ToList();
                }

                

                Parallel.ForEach(tasksStatuses, delegate (TasksStatus status) // foreach (BillStatus status in BillStatuses)
                {
                    cmbitems.Add(new cmbitem()
                    {
                        name = status.Status,
                        id = status.Id,
                        bcolor = status.backcolor,
                        fcolor = "#FF000000"
                    });
                });

                if (editFlag == true && task.Status != null)
                {
                    if (tasksStatuses.Count == 0 || tasksStatuses.FirstOrDefault(x => x.Id == task.statusId) == null)
                        cmbitems.Add(new cmbitem()
                        {
                            name = task.Status.Status,
                            id = task.Status.Id,
                            bcolor = task.Status.backcolor,
                            fcolor = "#FF000000"
                        });
                }

                cmbStatus.ItemsSource = cmbitems;
            }
        }

        private void LoadTaskTypes()
        {
            var _item = (TaskTemplate)cmbxTaskTemplate.SelectedIndex;

                var compId = (lookupCompany.SelectedItem as Company).Id;
                var deptId = (lookupDepartment.SelectedItem as Department).Id;

                var taskTypes = taskRepo.GetTaskTypesCompDept(compId, deptId);
                lookupTaskType.ItemsSource = taskTypes;

           
        }

        private void LoadTaskTemplate()
        {
            for (int i = 0; i <= (int)ERP_BL.Enums.TaskTemplate.Tax_Record; i++)
            {
                    cmbxTaskTemplate.Items.Add(((ERP_BL.Enums.TaskTemplate)i).ToString());
            }
        }

        private void LoadCompanies()
        {
            lookupCompany.ItemsSource = SYSTEM_STATIC.currentUser.employee.Companies;
        }

        private void SetValuesforPaymentyearandQuarter()
        {
            if (System.DateTime.Now.Month < 4)
            {
                spnQuarter.Text = "1";

            }
            else if (System.DateTime.Now.Month >= 4 && System.DateTime.Now.Month <= 6)
            {
                spnQuarter.Text = "2";

            }
            else if (System.DateTime.Now.Month >= 7 && System.DateTime.Now.Month <= 9)
            {
                spnQuarter.Text = "3";

            }
            else if (System.DateTime.Now.Month <= 12)
            {
                spnQuarter.Text = "4";
            }
        }

        private void LookupAssignedTo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void LookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                var company = lookupCompany.SelectedItem as Company;
                var depts = company.departments;
                List<Department> departments = new List<Department>();
                foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments.Where(x => x.IsTaskType == true).ToList())
                {
                    if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                        departments.Add(_dept);
                }
                //Only Allowed departments to Employee will show in Dropdown
                lookupDepartment.ItemsSource = departments;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message + "Invalid Company");
            }
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
            var company = lookupCompany.SelectedItem as Company;

            var users = taskRepo.getusersByCompanyDepartment(department.Id, company.Id);
            AllAllowedUser = taskRepo.getusersByCompanyDepartment(department.Id, company.Id);

            lookUpAssignedTo.ItemsSource = users;
            lookUpSupervisedBy.ItemsSource = users;
            lookUpAssignedBy.ItemsSource = users;
            grdCntrlUsers.ItemsSource = AllAllowedUser;

            LoadVendors();
            loademployees();

            if (cmbxTaskTemplate.SelectedIndex == 0)
                LoadTaskTypes();
        }

        public void loademployees()
        {
            var department = lookupDepartment.SelectedItem as Department;
            var company = lookupCompany.SelectedItem as Company;
            List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            employees = department.employees.Where(x => x.Companies.Find(y => y.Id == company.Id) != null).ToList(); ;
            List<cmbitem> cmbitems = new List<cmbitem>();
            foreach (ERP_BL.Databases.Employee employee in employees)
            {
                cmbitems.Add(new cmbitem() { name = employee.person.FName + " " + employee.person.LName, id = employee.EmpId });
            }
            cmbEmployee.ItemsSource = cmbitems;
        }

        private void LoadVendors()
        {
            var department = lookupDepartment.SelectedItem as Department;

            if (department.Id != 0)
            {
                VendorRepo vendorRepo = new VendorRepo();
                var vendors = vendorRepo.getAllActivebyDepartment(SYSTEM_STATIC.currentUser.employee);
                if (vendors == null || vendors.Count == 0)
                {
                    //DevExpress.Xpf.Core.DXMessageBox.Show("Please select a different Department and Company! No customer is mapped to this department or Company.", "Select another Department or Company", MessageBoxButton.OK, MessageBoxImage.Information);
                    //return;
                }
                else
                {
                    lookupVendor.ItemsSource = vendors.Where(x=>x.departments.Find(y=>y.Id == department.Id) != null).ToList();
                    return;
                }
            }
            else
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Please select a Department First!", "Select Department to Load Customer Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        int intGroupId;
        private void GroupIdCalculation()
        {
            string groupId;
            string year;
            string month;
            string id;

            var lastPaymentId = taskRepo.GetLastTransactionId();
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == false)
            {
                //datCreationDate.DateTime = DateTime.Now;
                GroupIdCalculation();
                txtSystemRef.Text = "Task-" + intGroupId;
                lblRefNo.Text = " (Task-" + intGroupId + ")";
                task.SystemId = intGroupId;
            }

            if (datCreationDate.DateTime == null)
            {
                DXMessageBox.Show("Please Enter Creation Date!");
                datCreationDate.Focus();
                return;
            }
            if (cmbxTaskTemplate.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Task Template");
                cmbxTaskTemplate.Focus();
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
            if (lookupVendor.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Vendor!");
                lookupVendor.Focus();
                return;
            }
            if (cmbStatus.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Status!");
                cmbStatus.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtSystemRef.Text))
            {
                DXMessageBox.Show("Please Enter System Ref!");
                txtSystemRef.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtTaskRef.Text))
            {
                DXMessageBox.Show("Please Enter Task Ref!");
                txtTaskRef.Focus();
                return;
            }
            if (lookUpSupervisedBy.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Supervisor!");
                lookUpSupervisedBy.Focus();
                return;
            }
            if (lookUpAssignedBy.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Assignee!");
                lookUpAssignedBy.Focus();
                return;
            }
            if (lookUpAssignedTo.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Assigned To!");
                lookUpAssignedTo.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtTaxableIncome.Text))
            {
                DXMessageBox.Show("Please enter Taxable Income!");
                txtTaxableIncome.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtTaxChargeable.Text))
            {
                DXMessageBox.Show("Please enter Tax Chargeable!");
                txtTaxChargeable.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtNTNnumber.Text))
            {
                DXMessageBox.Show("Please enter NTN #!");
                txtNTNnumber.Focus();
                return;
            }
            if (datTaxYear.DateTime == null)
            {
                DXMessageBox.Show("Please enter Tax Year!");
                datTaxYear.Focus();
                return;
            }
            if(chkIsFilerEmployee.IsChecked == true)
            {
                if(cmbEmployee.SelectedIndex < 0)
                {
                    DXMessageBox.Show("Please select Filer!");
                    cmbEmployee.Focus();
                    return;
                }
            }
            else
            {
                if (String.IsNullOrEmpty(txtFilerName.Text))
                {
                    DXMessageBox.Show("Please enter Filer Name!");
                    txtFilerName.Focus();
                    return;
                }
            }
            
            if (String.IsNullOrEmpty(txtDepositedTax.Text))
            {
                DXMessageBox.Show("Please enter Deposited Tax!");
                txtDepositedTax.Focus();
                return;
            }

            if (String.IsNullOrEmpty(txtDescription.Text))
            {
                DXMessageBox.Show("Please Enter Description!");
                txtDescription.Focus();
                return;
            }

            task.creationDate = datCreationDate.DateTime;

            task.TaxFrom = datFrom.DateTime;
            task.TaxTo = datTo.DateTime;

            task.taskTemplate = (TaskTemplate)cmbxTaskTemplate.SelectedIndex;

            //if (transactionType != null)
            //    task.transactionType = transactionType.Value;
            //else
            //    task.transactionType = null;

            task.companyId = (lookupCompany.SelectedItem as Company).Id;
            task.deptId = (lookupDepartment.SelectedItem as Department).Id;
            task.vendorId = (lookupVendor.SelectedItem as Vendor).Id;
            task.supervisedById = (lookUpSupervisedBy.SelectedItem as User).id;
            task.assignedToId = (lookUpAssignedTo.SelectedItem as User).id;
            task.assignedById = (lookUpAssignedBy.SelectedItem as User).id;

            if (lookupTaskType.SelectedIndex > -1)
                task.taskTypeId = (lookupTaskType.SelectedItem as TaskType).Id;

           

            task.SystemRef = txtSystemRef.Text;
            task.TaskRef = txtTaskRef.Text;

            task.taxableIncome = Convert.ToDouble(txtTaxableIncome.Text);
            task.chargeableTax = Convert.ToDouble(txtTaxChargeable.Text);
            task.NTNno = txtNTNnumber.Text;
            task.NoticeRefNo = txtNoticeRef.Text;
            task.taxYear = datTaxYear.DateTime;
            task.TaxQuarter = Convert.ToInt32(spnQuarter.Text);


            task.InputTax = Convert.ToDouble(txtInputTax.Text);
            task.OutputTax = Convert.ToDouble(txtOutputTax.Text);
            task.RefundAmountClaim = Convert.ToDouble(txtRefundAmount.Text);
            task.CreditCarriedForward = Convert.ToDouble(txtCreditCarriedForward.Text);
            task.AccumulatedCredit = Convert.ToDouble(txtAccumulatedCredit.Text);
            task.AccumulatedDebit = Convert.ToDouble(txtAccumulatedDebit.Text);
            task.FEDpayable = Convert.ToDouble(txtFEDpayable.Text);
            task.PLpayable = Convert.ToDouble(txtPLpayable.Text);
            task.SaleTaxPayable = Convert.ToDouble(txtSaleTaxPayable.Text);
            task.TotalAmountPaid = Convert.ToDouble(txtTotalAmountPaid.Text);

            if (chkIsFilerEmployee.IsChecked == true)
                task.employeeId = (cmbEmployee.SelectedItem as cmbitem).id;
            else
                task.FilerName = txtFilerName.Text;

            task.depositedTax = Convert.ToDouble(txtDepositedTax.Text);

            task.Description = txtDescription.Text;
            task.statusId = (cmbStatus.SelectedItem as cmbitem).id;
            //task.EfficiencyPoints = Convert.ToDouble(txtEfficiencyPoints.Text);

            if (SelectedAllowedUser.Count != 0)
            {
                task.AllowedUsers = new List<User>();
                foreach (var _user in SelectedAllowedUser)
                {
                    if (!task.AllowedUsers.Contains(_user))
                    {
                        task.AllowedUsers.Add(_user);
                    }
                }
            }



            //task.AllowedUsers = new List<User>();
            //foreach(User _user in grdCntrlUsers.SelectedItems)
            //{
            //    if (!task.AllowedUsers.Contains(_user))
            //        task.AllowedUsers.Add(_user);
            //}

            if (editFlag == false)
            {
                task.isApproved = false;
                task.creatorId = SYSTEM_STATIC.currentUser.id;
                taskRepo.AddTask(task, SYSTEM_STATIC.currentUser.id);
                DXMessageBox.Show("Successfully Added!");
            }
            else
            {
                task.TaskEfficiencies = new List<TaskEfficiency>();
                if (taskEfficiencies != null)
                {
                    foreach (var _eff in taskEfficiencies)
                    {
                        task.TaskEfficiencies.Add(new TaskEfficiency
                        {
                            Id = 0,
                            efficiencyPoints_Id = _eff.efficiencyPoints_Id,
                            tasksId = task.Id,
                            AchievedPoints = _eff.AchievedPoints,
                            TotalPoints = _eff.TotalPoints
                        });
                    }
                }
                taskRepo.UpdateTask(task);


                if (checkStatus.Id != task.Status.Id)
                {
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res = MessageBox.Show("Status of Task has been changed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        if (lookupDepartment.SelectedItem as Department != null && (lookupDepartment.SelectedItem as Department).Id != 0 && (lookupCompany.SelectedItem as Company)?.Id != 0/*&& department.users!=null&& department.users.Count!=0*/)
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);

                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string oldStat = checkStatus.Status;
                    string newStat = task.Status.Status;
                    string symbolCurr = "";
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog();

                    comment.Comment = "Status of Task having System Ref # " + task.SystemRef + " (" + symbolCurr + ")\n "
               + "\nhas been changed \nFrom: " + oldStat + " \nTo: " + newStat;
                    comment.Timestamp = DateTime.Now;
                    comment.Subject = "Status Changed";                                        comment.TaggedList = tagUsers;
                                        comment.CCUsersList = ccUsers;
                                        comment.TaggedRecomenndedList = tagUsersRecommendation;
                                        comment.CCRecomenndedList = ccUsersRecommendation;


                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating notification
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {

                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {


                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Loans Advance with System Ref #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }



                }
                if (checkStatus != null && checkStatus.Id != 0)
                {
                    //saleOrderRepo.Add(saleOrderid, 3, "Status Changed from (" + checkStatus.Status + ") to (" + saleOrder.saleOrderStatus.Status + ")");
                    UsersRepo.Add(TransactionInfo.Status_Changed, task.Id, 3, "Status Changed from (" + checkStatus.Status + ") to (" + task.Status.Status + ")");
                }
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
                UsersRepo.Add(TransactionInfo.Edited, task.Id, (int)TransactionItemType.Tasks, frmInputBox.comment);

                DXMessageBox.Show("Successfully Updated!");
            }
            Window myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        private void BtnInfo_Click(object sender, RoutedEventArgs e)
        {
            if (gridTracker.Visibility == Visibility.Collapsed)
            {
                views = UsersRepo.getViwerInfo(taskId, (int)TransactionItemType.Tasks);
                grdUsers.ItemsSource = views;
                gridTracker.Visibility = Visibility.Visible;
            }
            else
            {
                gridTracker.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnMarkReviewed_Click(object sender, RoutedEventArgs e)
        {
            if (task.isApproved == false)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new Added Task") != null)
                {
                    task.isReviewed = true;
                    task.isApproved = true;
                    task.stage = TransactionStage.Approved.ToString();
                    taskRepo.UpdateTask(task);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Vendor Bill has been Approved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }
                    }

                    string symbolCurr = "";
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task having value: " + task.ManualAmount.ToString() + "(" + symbolCurr + ") " + " has been Approved",
                        Timestamp = DateTime.Now,
                        Subject = "Task Approved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
                else if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 2 Task") != null))
                {
                    task.isReviewed = true;
                    task.needReview = false;
                    task.stage = TransactionStage.AwaitingApproval.ToString();

                    taskRepo.UpdateTask(task);
                }
                if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Reviewer Level 1 Task") != null))
                {
                    task.isReviewed = true;
                    task.needReview = true;
                    task.stage = TransactionStage.AwaitingSecondReview.ToString();

                    taskRepo.UpdateTask(task);
                }
            }


            else if (task.isApproved == true || task.isApproved == null)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for new Added Task") != null)
                {
                    task.isReviewed = true;
                    task.isApproved = false;
                    task.stage = TransactionStage.AwaitingApproval.ToString();
                    taskRepo.UpdateTask(task);

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Task has been UnApproved, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {

                        if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }
                    }

                    string symbolCurr = "";
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task having value: " + task.ManualAmount.ToString() + "(" + symbolCurr + ") " + " has been UnApproved",
                        Timestamp = DateTime.Now,
                        Subject = "Task UnApproved",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                    Window myWindow = Window.GetWindow(this);
                    myWindow.Close();
                    return;
                }
            }

            loadcomments();
        }

        public ucTaxTaskAdd(TasksStatus tasksStatus)
        {
            statusChanged = tasksStatus;
        }
        ProcurementRepo procurementRepo = new ProcurementRepo();
        NotificationsRepo notificationsRepo = new NotificationsRepo();
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            UsersRepo = new UsersRepo();

            if (editFlag == true && taskId > 0)
            {
                string previous_status;
                taskRepo = new TaskRepo();
                task = taskRepo.GetTask(taskId);
                if (task != null)
                {
                    if (task.isApproved == false)
                    {
                        DXMessageBox.Show("This task cannot be Closed because it is Underl Approval Stage!");
                        return;
                    }
                    previous_status = task.Status.Status;
                    var deparment = task.department;
                    var company = task.company;

                    statusChanged = null;
                    ucFrmTasksDirectClose ucFrmDirectClose = new ucFrmTasksDirectClose();
                    if (task.Status != null)
                    {
                        ucFrmDirectClose.statusName.Text = task.Status.Status;

                        var brush = new BrushConverter();
                        ucFrmDirectClose.statusColor.Background = (Brush)brush.ConvertFrom(task.Status.backcolor);
                    }

                    ucFrmDirectClose.directCloseWin.Content = ucFrmDirectClose;
                    ucFrmDirectClose.frmFlag = true;
                    ucFrmDirectClose.template = "TaxRecord";
                    ucFrmDirectClose.directCloseWin.Width = 450;
                    ucFrmDirectClose.directCloseWin.Height = 650;
                    ucFrmDirectClose.directCloseWin.ResizeMode = ResizeMode.NoResize;
                    ucFrmDirectClose.directCloseWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ucFrmDirectClose.directCloseWin.ShowDialog();

                    if (statusChanged != null)
                    {
                        //Asking for Tag
                        List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                        var res = MessageBox.Show("Task has been Closed, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (res == MessageBoxResult.Yes)
                        {
                            if (deparment != null && company?.Id != 0)
                            {
                                //var userss = _usersRepo.getusersByCompanyDepartment(deparment.Id, company.Id);

                                winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
                                //frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, _usersRepo.getusersByCompanyDepartment(department.Id, company.Id), TransactionItemType.Sale_Invoice);
                                win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                            }
                            else
                            {
                                winTagUsers win = new winTagUsers();
                                win.ShowDialog();

                            }
                        }

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Approver for Closing User Task") != null || SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Close User Tasks without Approval") != null)
                        {

                            task.PendingForClosing = false;
                            task.stage = TransactionStage.Closed.ToString();
                            task.statusId = statusChanged.Id;
                            task.LastStatusChangeDate = System.DateTime.Now;
                            task.ClosingDate = System.DateTime.Now;



                            taskRepo.UpdateTask(task);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Approved_Closing, statusChanged.Id, (int)TransactionItemType.Tasks, frmInputBox.comment);


                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Task having system ref #: " + task.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };

                            procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }


                            //Load_Receipts();
                        }
                        else
                        {

                            task.PendingForClosing = true;
                            task.stage = TransactionStage.AwaitingApproval.ToString();
                            task.statusId = statusChanged.Id;
                            task.LastStatusChangeDate = System.DateTime.Now;
                            task.ClosingDate = System.DateTime.Now;


                            taskRepo.UpdateTask(task);
                            frmInputBox inputBox = new frmInputBox("While direct closing Status Changed from (" + previous_status + ") to (" + statusChanged.Status + ")");
                            inputBox.ShowDialog();
                            UsersRepo.Add(TransactionInfo.Closed, statusChanged.Id, (int)TransactionItemType.Payments, frmInputBox.comment);

                            string oldStat = previous_status;
                            string newStat = statusChanged.Status;
                            CommentLog comment = new CommentLog()
                            {
                                Comment = "Status of Task having system ref #: " + task.SystemRef + " has been changed \nFrom: " + oldStat + " \nTo: " + newStat,
                                Timestamp = DateTime.Now,
                                Subject = "Status Changed",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                            };

                            procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                            //Creating notification
                            if (tagUsers.Count != 0)
                            {
                                foreach (var user in tagUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                                }
                            }

                            if (ccUsers.Count != 0)
                            {
                                foreach (var user in ccUsers)
                                {
                                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " tagged you in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                                }
                            }


                        }
                        DXMessageBox.Show("Status has been changed to InActive from " + previous_status + " to " + statusChanged.Status);
                        var myWindow = Window.GetWindow(this);
                        myWindow.Close();

                    }
                }


            }
        }

        private void BtnAddComment_Click(object sender, RoutedEventArgs e)
        {

            var department = lookupDepartment.SelectedItem as Department;
            var company = lookupCompany.SelectedItem as Company;

            NotificationsRepo notificationsRepo = new NotificationsRepo();
            if (department != null && department.Id != 0 && company?.Id != 0)
            {

                frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, task.AllowedUsers, TransactionItemType.Bill);
                inputBox.ShowDialog();

            }

            else
            {
                frmInputBox inputBox = new frmInputBox();
                inputBox.ShowDialog();
            }

            if (task != null)
            {
                ProcurementRepo procurementRepo = new ProcurementRepo();

                if (frmInputBox.comment != "" && task.Id != 0)
                {
                    if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                    {
                        foreach (var user in frmInputBox.Comment.TaggedList)
                        {
                            if (frmInputBox.FlagForTag == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", null);
                        }
                        foreach (var user in frmInputBox.Comment.CCUsersList)
                        {
                            if (frmInputBox.FlagForCC == true)
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                            else
                                notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", null);
                        }
                    }

                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                    loadcomments();
                    MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                    if (window1 != null) { window1.UrgentNotificationGlow(); }
                }
                else if (task.Id == 0)
                {
                    DXMessageBox.Show("Kindly save Bill first to add a comment!");
                }

            }
        }

        public void loadcomments()
        {
            try
            {
                if (task != null)
                {
                    ProcurementRepo procurementRepo = new ProcurementRepo();

                    List<CommentLog> comments = new List<CommentLog>();
                    comments = procurementRepo.getcommentslogAsc(task.Id, TransactionItemType.Tasks);

                    grdCommentss.ItemsSource = comments;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
            }
        }

        private void BtnCommentLog_Click(object sender, RoutedEventArgs e)
        {
            if (grdComments.Visibility == Visibility.Collapsed)
            {
                grdComments.Visibility = Visibility.Visible;
                loadcomments();
            }
            else
                grdComments.Visibility = Visibility.Collapsed;
        }

        private void BtnAttachNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttachmentList_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttachments.Visibility == Visibility.Visible)
                grdAttachments.Visibility = Visibility.Collapsed;
            else
            {
                treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(task.Id, TransactionItemType.Tasks);
                grdAttachments.Visibility = Visibility.Visible;
            }
        }

        private void Btnexpand_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 1200;
        }

        private void BtnCollapse_Click(object sender, RoutedEventArgs e)
        {
            gridTracker.Width = 500;
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            ViewInfo viewInfo = views.Find(x => x.Id == (grdUsers.SelectedItem as ViewInfo).Id);
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {

                string fname = viewInfo.User.employee.person.FName;
                string lname = viewInfo.User.employee.person.LName;
                e.Value = fname + " " + lname;

            }
        }

        private void BtnDownloadAttachment_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                Button thisButton = (Button)sender;
                string str = thisButton.Tag.ToString();
                if (!string.IsNullOrEmpty(str))
                {
                    grdProgressBar.Visibility = Visibility.Visible;
                    System.Threading.Thread thread = new System.Threading.Thread(() =>
                    {
                        //Button thisButton = (Button)sender;

                        ERP_BL.Attach attachment = new ERP_BL.Attach(str);

                        if (str.Contains("Sale_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Offer"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Offer);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Invoice"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Invoice);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Purchase_Order"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Purchase_Order);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Sale_Receipt"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Sale_Receipt);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Payments"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Payments);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Bill"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Bill);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }
                        else
                        if (str.Contains("Tasks"))
                        {
                            var result = attachment.startDownload(str, TransactionItemType.Tasks);
                            if (!string.IsNullOrEmpty(result.Item2))
                            {
                                Process.Start(result.Item2);
                                this.Dispatcher.Invoke(() =>
                                {
                                    grdProgressBar.Visibility = Visibility.Collapsed;
                                });
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    DXMessageBox.Show("Error in Downloading File", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                                });
                            }
                        }

                    });
                    thread.Start();
                    //grdProgressBar.Visibility = Visibility.Collapsed;
                }
                else
                    return;



            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());


            }
        }

        private void BtnTrackingWindow_Click(object sender, RoutedEventArgs e)
        {
            if (taskId != 0)
            {
                frmTrackingWindow trackingWindow = new frmTrackingWindow(taskId, TransactionItemType.Tasks);
                trackingWindow.ShowDialog();
            }
        }

        private void BtnSetVoid_Click(object sender, RoutedEventArgs e)
        {
            if (task.isVoid == true && (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Unmark Void User Tasks") != null))
            {
                if (DXMessageBox.Show("This Task is currently in the list of Void Tasks s! Do you want to remove it from Void?", "Remove Void Tasks", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    task.isVoid = false;
                    taskRepo.UpdateTask(task);

                    grdVoid.Visibility = Visibility.Collapsed;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Task has been UnVoided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task (Amount OC) having value: " + task.ManualAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as Unvoid",
                        Timestamp = DateTime.Now,
                        Subject = "Task UnVoided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }
            else if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Mark as Void User Tasks") != null && task.isVoid != true)
            {
                if (DXMessageBox.Show("This Task is not currently in the list of Void Tasks s! Do you want to move it to Void Tasks?", "Add to Void Tasks", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    task.isVoid = true;
                    taskRepo.UpdateTask(task);

                    grdVoid.Visibility = Visibility.Visible;

                    NotificationsRepo notificationsRepo = new NotificationsRepo();
                    ProcurementRepo procurementRepo = new ProcurementRepo();
                    UsersRepo userRepo = new UsersRepo();
                    //Asking for Tag
                    List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

                    var res1 = MessageBox.Show("Task has been Voided, Do you want to notify other users by tagging?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (res1 == MessageBoxResult.Yes)
                    {
                        if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                        {
                            winTagUsers win = new winTagUsers(task.AllowedUsers, task.Id, TransactionItemType.Tasks);
                            win.ShowDialog();
tagUsers = win.tagUsers;
ccUsers = win.ccUsers;
tagUsersRecommendation = win.tagRecommendationUsers;
ccUsersRecommendation = win.ccRecommendationUsers;
                        }
                        else
                        {
                            winTagUsers win = new winTagUsers();
                            win.ShowDialog();
                        }

                    }

                    string symbolCurr = "";
                    if (task.currency != null)
                    {
                        symbolCurr = task.currency.Abbrivation.ToString();
                    }
                    CommentLog comment = new CommentLog()
                    {
                        Comment = "Task (Amount OC) having value: " + task.ManualAmount.ToString() + "(" + symbolCurr + ") " + " has been marked as void",
                        Timestamp = DateTime.Now,
                        Subject = "Task Voided",TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
                    };
                    procurementRepo.Add(task.Id, TransactionItemType.Tasks, comment, SYSTEM_STATIC.currentUser.employeeId);
                    //Creating Comments
                    if (tagUsers.Count != 0)
                    {
                        foreach (var user in tagUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, user.id, "New Comment ", null);

                        }
                    }

                    if (ccUsers.Count != 0)
                    {
                        foreach (var user in ccUsers)
                        {
                            notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, comment.Comment, 0, user.id, "New Comment ", null);
                        }
                    }
                }
            }

            loadcomments();
            var thisWindow = Window.GetWindow(this);
            thisWindow.Close();
        }

        private void BtnAttachNew1_Click(object sender, RoutedEventArgs e)
        {
            if (grdAttach1.Visibility == Visibility.Visible)
                grdAttach1.Visibility = Visibility.Collapsed;
            else
            {
                cmbCategory1.ItemsSource = SYSTEM_STATIC.GetActiveTaskAttachmentCategories();
                grdAttach1.Visibility = Visibility.Visible;
            }
        }


        private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NotificationsRepo notificationsRepo = new NotificationsRepo();

            if (grdCommentss.SelectedItem != null)
            {
                var comment = grdCommentss.SelectedItem as CommentLog;
                if (task.Id != 0)
                {
                    if (task.department != null && task.department.Id != 0 && task.company?.Id != 0)
                    {
                        frmInputBox inputBox = new frmInputBox("Write a comment in a below box and press save", "Add Comment ", true, task.AllowedUsers, comment, TransactionItemType.Tasks);
                        inputBox.ShowDialog();
                    }
                    else
                    {
                        frmInputBox inputBox = new frmInputBox();
                        inputBox.ShowDialog();
                    }

                    if (task != null)
                    {
                        ProcurementRepo procurementRepo = new ProcurementRepo();

                        if (frmInputBox.comment != "" && task.Id != 0)
                        {
                            if (frmInputBox.Comment.TaggedList != null || frmInputBox.Comment.CCUsersList != null)
                            {
                                foreach (var user in frmInputBox.Comment.TaggedList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, user.id, "New Comment ", null);

                                }
                                foreach (var user in frmInputBox.Comment.CCUsersList)
                                {
                                    if (frmInputBox.FlagForTag == true)
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", frmInputBox.FlagId);
                                    else
                                        notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, frmInputBox.comment, 0, user.id, "New Comment ", null);
                                }
                            }
                            procurementRepo.Add(task.Id, TransactionItemType.Tasks, frmInputBox.Comment, SYSTEM_STATIC.currentUser.employeeId);
                            loadcomments();

                            MainWindow window1 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                            if (window1 != null) { window1.UrgentNotificationGlow(); }
                        }
                        else if (task.Id == 0)
                        {
                            DXMessageBox.Show("Kindly save Task first to add a comment!");
                        }

                    }
                }
                else
                {
                    DXMessageBox.Show($"Select a comment to add reply!", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Hand);

                }
            }
        }

        private void BtnAttachmentList1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (taskId != 0)
            {
                UsersRepo.Add(TransactionInfo.viewed, taskId, (int)TransactionItemType.Tasks, "Viewed details of Task");
            }
        }

        private void cmbCurrency_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TxtDescription_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GrdTaskTracking_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.IsGetData)
            {
                string userName = "";
                var row = grdTaskTracking.GetRowByListIndex(e.ListSourceRowIndex) as TaskTracking;
                switch (e.Column.FieldName)
                {
                    case "UpdatedByy":
                        if (row.UpdatedBy != null && row.UpdatedBy.employee != null && row.UpdatedBy.employee.person != null)
                        {
                            userName = row.UpdatedBy.employee.person.FName + " " + row.UpdatedBy.employee.person.LName;
                        }
                        e.Value = userName;
                        break;

                }
            }
        }

        private void BtnAttachment1_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory1.SelectedItem != null)
            {
                if (taskId != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory1.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\Tasks\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += taskId + "_" + TransactionItemType.Tasks.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Tasks);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), taskId, TransactionItemType.Tasks, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, task.Id, (int)TransactionItemType.Tasks, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Tasks);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            System.IO.File.Move(destination, sourceFile);
                                            DXMessageBox.Show("Error while Uploading Attachment, Try Again", "Try again");
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });
                                    }
                                });
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;
                            }


                            //DXMessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        DXMessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {



                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void BtnAttachment_Click(object sender, RoutedEventArgs e)
        {
            string imageAttach = "/ZAS_ERP;component/images/clipattachment.png";
            string imageLoading = "/ZAS_ERP;component/images/LoadingColorDot.gif";
            if (cmbCategory.SelectedItem != null)
            {
                if (taskId != 0)
                {
                    try
                    {

                        int CategoryId = (cmbCategory.SelectedItem as cmbitem).id;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "Image Files|*.jpg;*.jpeg; *.png; *.pdf;";

                        fileDialog.Multiselect = false;
                        string sourceFile = @"";
                        string exePath = System.Environment.GetCommandLineArgs()[0];
                        string destination = exePath.Replace(System.IO.Path.GetFileName(exePath), "");
                        destination += "Attachments\\Tasks\\ToUpload\\";
                        //string destination = @"D:\MovedFiles\new\";
                        System.IO.Directory.CreateDirectory(destination);
                        if (fileDialog.ShowDialog() == true) // Test result.
                        {
                            imgAttachNew.Source = new BitmapImage(new Uri(imageLoading, UriKind.RelativeOrAbsolute));
                            btnAttachNew.ToolTip = "Uploading";
                            btnAttachNew.IsEnabled = true;

                            btnAttachment.Content = "Uploading File . . .";
                            sourceFile = fileDialog.FileName;
                            destination += taskId + "_" + TransactionItemType.Tasks.ToString() + "_" + System.IO.Path.GetFileName(fileDialog.FileName);

                            if (sourceFile.Length < 74)
                            {
                                System.IO.File.Move(sourceFile, destination);

                                System.Threading.Thread thread = new System.Threading.Thread(() =>
                                {
                                    ERP_BL.Attach attachment = new ERP_BL.Attach(destination);
                                    var result = attachment.startUploading(TransactionItemType.Tasks);
                                    if (result.Item1)
                                    {
                                        AttachmentsRepo repo = new AttachmentsRepo();
                                        //Attachment attachmen= new Attachment();
                                        repo.Add(System.IO.Path.GetFileName(result.Item2), taskId, TransactionItemType.Tasks, "None", MainWindow.currentUserid, "PehliAttachment", destination, result.Item2, CategoryId);
                                        UsersRepo.Add(TransactionInfo.Attachment_Uploaded, task.Id, (int)TransactionItemType.Tasks, "Added a new attachment");

                                        this.Dispatcher.Invoke(() =>
                                        {
                                            //treeViewAttachments.ItemsSource = SYSTEM_STATIC.GetAttachmentsListByCategory(OrderId, TransactionItemType.Tasks);
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                        });



                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            System.IO.File.Move(destination, sourceFile);
                                            DXMessageBox.Show("Error while Uploading Attachment, Try Again", "Try again");
                                            imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                                            btnAttachNew.ToolTip = "Attach";
                                        });
                                    }
                                            btnAttachNew.IsEnabled = true;
                                            btnAttachment.Content = "Select";
                                });
                                thread.Start();
                            }
                            else
                            {
                                MessageBox.Show("Invalid File name size");
                                return;
                            }


                            //DXMessageBox.Show("Attachment Uploaded");


                        }


                    }
                    catch (Exception ex)
                    {
                        DXMessageBox.Show(ex.ToString());
                        imgAttachNew.Source = new BitmapImage(new Uri(imageAttach, UriKind.RelativeOrAbsolute));

                        btnAttachNew.ToolTip = "Attach";
                        btnAttachNew.IsEnabled = true;
                    }
                    finally
                    {



                    }
                }
                else
                    return;
            }
            else
            {
                DXMessageBox.Show("Select a Attachment Category in which you want to upload an attachment.", "Select Category", MessageBoxButton.OK, MessageBoxImage.Stop);
            }

        }

        private void LookUpAssignedTo_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var user = lookUpAssignedTo.SelectedItem as User;
            if (user != null)
            {
                if (user.employee != null)
                {
                    if (user.employee.person != null)
                    {
                        var byteImg = user.employee.person.Photo;
                        if (byteImg != null)
                        {
                            var image = GetBitmapImageFromByteArray(byteImg);
                            imgAssignedTo.ImageSource = image;
                        }
                    }
                }
            }
        }

        public BitmapImage GetBitmapImageFromByteArray(byte[] bytesArr)
        {
            try
            {
                MemoryStream stream = new MemoryStream();
                stream.Write(bytesArr, 0, bytesArr.Length);
                stream.Position = 0;
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                BitmapImage returnImage = new BitmapImage();
                returnImage.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                returnImage.StreamSource = ms;
                returnImage.EndInit();

                return returnImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        private void LookupCustomer_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void LookupCustomer_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        

        private void TabUsers_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TabUsers_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveTracking_Click(object sender, RoutedEventArgs e)
        {

            Window win = new Window();
            ucFrmTaskTracking frmTaskTracking = new ucFrmTaskTracking();
            win.Content = frmTaskTracking;
            win.ResizeMode = ResizeMode.NoResize;
            win.Width = 400;
            win.Height = 300;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();

            TaskTracking tracking = new TaskTracking();
            tracking.Description = frmTaskTracking.txtDetails.Text;
            tracking.UpdateDateTime = DateTime.Now;
            tracking.UpdatedById = SYSTEM_STATIC.currentUser.id;

            if (task.TaskTrackings != null)
                task.TaskTrackings.Add(tracking);

            taskRepo.UpdateTask(task);

            DXMessageBox.Show("Tracking Added Successfully!");

            List<User> tagUsers = new List<User>();
List<User> ccUsers = new List<User>(); 
List<User> tagUsersRecommendation = new List<User>();
List<User> ccUsersRecommendation = new List<User>();

            if (task.assignedTo.id == SYSTEM_STATIC.currentUser.id)
            {
                tagUsers.Add(task.assignedBy);
                ccUsers.Add(task.supervisedBy);
            }
            else if (task.assignedBy.id == SYSTEM_STATIC.currentUser.id)
            {
                tagUsers.Add(task.assignedTo);
                ccUsers.Add(task.supervisedBy);
            }
            else if (task.supervisedBy.id == SYSTEM_STATIC.currentUser.id)
            {
                tagUsers.Add(task.assignedTo);
                ccUsers.Add(task.assignedBy);
            }
            else
            {
                tagUsers.Add(task.assignedTo);
                ccUsers.Add(task.assignedBy);
                ccUsers.Add(task.supervisedBy);
            }

            CommentLog Comment = new CommentLog
            {
                Subject = "Tracking Added",
                Comment = "Task having System Ref # " + task.SystemRef + " has been Updated\n" + tracking.Description,TaggedList = tagUsers,
CCUsersList = ccUsers,
TaggedRecomenndedList = tagUsersRecommendation,
CCRecomenndedList = ccUsersRecommendation
            };

            if (Comment.TaggedList != null || Comment.CCUsersList != null)
            {
                foreach (var user in Comment.TaggedList)
                {
                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, Comment.Comment, user.id, "New Comment ", null);
                }
                foreach (var user in Comment.CCUsersList)
                {
                    notificationsRepo.Add(SYSTEM_STATIC.currentUser.userName + " in Task #" + task.SystemRef, task.Id, TransactionItemType.Tasks, Comment.Comment, 0, user.id, "New Comment ", null);
                }
            }



            procurementRepo.Add(task.Id, TransactionItemType.Tasks, Comment, SYSTEM_STATIC.currentUser.employeeId);
        }

        private void imgLeftToRight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Height = 25;
            imgLeftToRight.Width = 25;
        }

        private void imgLeftToRight_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRight.Height = 32;
            imgLeftToRight.Width = 32;
            try
            {
                var selectedItem = grdCntrlUsers.SelectedItem as User;

                if (selectedItem != null)
                {

                    AllAllowedUser.Remove(selectedItem);
                    if (!SelectedAllowedUser.Contains(selectedItem))
                        SelectedAllowedUser.Add(selectedItem);

                }
                else
                {
                    DXMessageBox.Show("Please Select First From Employee List Register!");
                }
                grdCntrlUsersSelected.RefreshData();
                grdCntrlUsers.RefreshData();
                grdCntrlUsers.SelectedItem = null;
                grdCntrlUsersSelected.SelectedItem = null;
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

        private void btnLeftMoveCustomer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 32;
            imgRightToLeft.Height = 32;

            try
            {
                var selectedItem = grdCntrlUsersSelected.SelectedItem as User;

                if (selectedItem != null)
                {
                    if (!AllAllowedUser.Contains(selectedItem))
                        AllAllowedUser.Add(selectedItem);
                    SelectedAllowedUser.Remove(selectedItem);
                }
                else
                {
                    DXMessageBox.Show("Please Select First From Selected Employee Register!");
                }
                grdCntrlUsersSelected.RefreshData();
                grdCntrlUsers.RefreshData();
                grdCntrlUsers.SelectedItem = null;
                grdCntrlUsersSelected.SelectedItem = null;
            }
            catch (Exception)
            {


            }
        }

        private void btnLeftMoveCustomer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeft.Width = 25;
            imgRightToLeft.Height = 25;
        }

        private void GrdCntrlUsers_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var row = grdCntrlUsers.GetRowByListIndex(e.ListSourceRowIndex) as User;

            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlUsersSelected_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var row = grdCntrlUsersSelected.GetRowByListIndex(e.ListSourceRowIndex) as User;

            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlSupervisedBy_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var gridControl = lookUpSupervisedBy.GetGridControl();
            var row = gridControl.GetRowByListIndex(e.ListSourceRowIndex) as User;

            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlAssignedBy_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var gridControl = lookUpAssignedBy.GetGridControl();
            var row = gridControl.GetRowByListIndex(e.ListSourceRowIndex) as User;

            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlAssignedTo_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            var gridControl = lookUpAssignedTo.GetGridControl();
            var row = gridControl.GetRowByListIndex(e.ListSourceRowIndex) as User;

            if (row.employee.isTaskType == true && SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Can View Non-Departmental Users in Tasks") == null)
            {
                e.Visible = false;
            }
            e.Handled = !e.Visible ? true : false;
        }

        private void GrdCntrlUsersSelected_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {

        }

        private void BtnAddEfficiencyPoints_Click(object sender, RoutedEventArgs e)
        {
            if (editFlag == true)
            {
                try
                {
                    ucFrmEfficiencyPoints ucFrmEfficiency = new ucFrmEfficiencyPoints(task.Id);
                    ucFrmEfficiency.ShowDialog();
                    taskEfficiencies = ucFrmEfficiency.finalTaskEfficiencies;
                    var achievedPoints = taskEfficiencies.Sum(x => x.AchievedPoints);
                    var totalPoints = taskEfficiencies.Where(x => x.TotalPoints >= 0).Sum(x => x.TotalPoints);
                    txtEfficiencyPoints.Text = achievedPoints + "/" + totalPoints;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                DXMessageBox.Show("Please Save this Task first!");
                return;
            }

        }

        private void lookupTaskType_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Company first!");
                lookupCompany.Focus();
                return;
            }
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please Select Department first!");
                lookupDepartment.Focus();
                return;
            }
        }

        private void CmbxTaskTemplate_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //if (cmbxTaskTemplate.SelectedIndex > 0)
            //{
               // LoadTaskTypes();
            //}

        }

        private void LookupVendor_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void ChkIsFilerEmployee_Checked(object sender, RoutedEventArgs e)
        {
            txtFilerName.Visibility = Visibility.Collapsed;
            cmbEmployee.Visibility = Visibility.Visible;
        }

        private void ChkIsFilerEmployee_Unchecked(object sender, RoutedEventArgs e)
        {
            txtFilerName.Visibility = Visibility.Visible;
            cmbEmployee.Visibility = Visibility.Collapsed;
        }

        private void cmbEmployee_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lookupDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department first!");
                return;
            }
        }

        private void LookupTaskType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            LoadTaskStatuses();


        }
    }

    public class TaskEfficiencyPoints
    {
        public int Id { get; set; }

        public double TotalPoints { get; set; }
        public double AchievedPoints { get; set; }

        public List<TaskEfficiency> taskEfficiencies = new List<TaskEfficiency>();
    }
}
