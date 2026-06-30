using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.ToDoTasks.UserControls
{
    /// <summary>
    /// Interaction logic for ucEnterListItemName.xaml
    /// </summary>
    public partial class ucEnterListItemTitle : UserControl
    {
        public int groupId = 0;
        public bool editFlag = false;
        public static string Title;
        ToDoTaskRepo taskRepo = new ToDoTaskRepo();
        TaskGroups taskGroup = new TaskGroups();

        
        public TaskGroupTemplate template;
        public bool isTitle = false;

        List<Company> allCompanies = new List<Company>();
        List<Company> selectedCompanies = new List<Company>();

        List<Department> allDepartments = new List<Department>();
        List<Department> selectedDepartments = new List<Department>();

        List<User> allUsers = new List<User>();
        List<User> selectedUsers = new List<User>();


        List<Company> allCompaniesBulk = new List<Company>();
        List<Company> selectedCompaniesBulk = new List<Company>();

        List<Department> allDepartmentsBulk = new List<Department>();
        List<Department> selectedDepartmentsBulk = new List<Department>();

        List<User> allUsersBulk = new List<User>();
        List<User> selectedUsersBulk = new List<User>();

        public ucEnterListItemTitle()
        {
            InitializeComponent();

        }

        public void LoadChartofAccounts()
        {
            ChartofAccountsRepo coaRepo = new ChartofAccountsRepo();
            lookupCGSAccounts.ItemsSource = coaRepo.getAllActive(SYSTEM_STATIC.currentUser.id);
            lookupPayableAccounts.ItemsSource = coaRepo.getAllActive(SYSTEM_STATIC.currentUser.id);
        }
        //public void LoadCompanies()
        //{
        //    lookupCompany.ItemsSource= SYSTEM_STATIC.LoadCurrentUserCompanies();
        //}
        //public void loadDepartments()
        //{
        //    lookupDepartment.ItemsSource = SYSTEM_STATIC.LoadCurrentUserDepartments();
        //}
        private void LoadCurrencies()
        {

            CurrencyRepo currencyRepo = new CurrencyRepo();
            List<Currency> currencies = currencyRepo.getAll();
            currencies = currencies.Where(x => x.isVoid != true).ToList();

            List<cmbitem> cmbitems = new List<cmbitem>();
            cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });
            foreach (Currency cur in currencies)
            {

                //cmbCurrency.Items.Add(new cmbitem() { name = cur.CurrencyName, id = cur.Id });
                cmbitems.Add(new cmbitem() { name = cur.CurrencyName + "(" + cur.Abbrivation + " " + cur.Symbol + ")", id = cur.Id });
            }

            cmbCurrency.ItemsSource = cmbitems;

        }

        private void LoadStatusCalculationTypes()
        {
            ToDoTaskRepo taskRepo = new ToDoTaskRepo();
            lookUpCalculationType.ItemsSource = taskRepo.GetAllStatusCalculationTypes();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChartofAccounts();
            //LoadCompanies();
            //loadDepartments();
            LoadCurrencies();
            LoadGroups();
            LoadStatusCalculationTypes();
            LoadTargetGroups();

            for (int i = 0; i <= (int)ERP_BL.Enums.TargetsTransactionType.VendorBill; i++)
            {
                cmbxModuleType.Items.Add(((ERP_BL.Enums.TargetsTransactionType)i).ToString());
            }

            if (editFlag == false)
            {
                
                if (template == TaskGroupTemplate.Standard)
                {
                    //tabCompDepts.Visibility = Visibility.Visible;
                    loadcompanies();
                }
                else
                { 
                    //tabCompDepts.Visibility = Visibility.Collapsed;
                    LoadUsers();
                    UIvisibility();
                }
                if(isTitle == true)
                {
                    LoadUsers();
                    //lblParentGroup.Visibility = Visibility.Collapsed;
                    //chkHasParent.Visibility = Visibility.Collapsed;
                    //lookupParentGroup.Visibility = Visibility.Collapsed;
                    grdCalculationType.Visibility = Visibility.Collapsed;
                    grdModuleType.Visibility = Visibility.Collapsed;
                    UIvisibility();
                }
                tabGroupDetails.Visibility = Visibility.Collapsed;
            }

            if(groupId > 0 && editFlag == true)
            {
                if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Group") == null)
                    btnSave.IsEnabled = false;

                taskGroup = taskRepo.GetTaskGroup(groupId);

                isTitle = taskGroup.isTitle;

                //Select Template
                for (int i = 0; i <= (int)ERP_BL.Enums.TargetsTransactionType.VendorBill; i++)
                {

                    if (((ERP_BL.Enums.TargetsTransactionType)i).ToString() == taskGroup.targetsTransactionType.ToString())
                    {
                        cmbxModuleType.SelectedIndex = i;
                        break;
                    }
                }

                if (!String.IsNullOrEmpty( taskGroup.settingValue))
                    gridDepartmentBulk.RestoreLayoutFromStream(SYSTEM_STATIC.ConvertToMemoryStream(taskGroup.settingValue));

                if (taskGroup.calculationType != null)
                    lookUpCalculationType.Text = taskGroup.calculationType.TypeName;
                else
                    lookUpCalculationType.SelectedIndex = -1;

                if (taskGroup.targetGroup != null)
                    lookUpTargetGroup.Text = taskGroup.targetGroup.GroupName;
                else
                    lookUpTargetGroup.SelectedIndex = -1;

                if (taskGroup.cgsAccount_Id != null)
                {
                    lookupCGSAccounts.Text = taskGroup.CGSAccount.accountName;
                }
                if (taskGroup.payableAccount_Id != null)
                {
                    lookupPayableAccounts.Text = taskGroup.PayableAccount.accountName;
                }
                //if(taskGroup.companyId!=null)
                //{
                //    lookupCompany.Text = taskGroup.Company.CompanyName;
                //}
                //if (taskGroup.dept_Id != null)
                //{
                //    lookupDepartment.Text = taskGroup.Department.DeptName;
                //}
                if (taskGroup.currency_Id != null)
                {
                    var currencySource = (List<cmbitem>)cmbCurrency.Items.SourceCollection;
                    cmbCurrency.SelectedItem = cmbCurrency.Items[cmbCurrency.Items.IndexOf(currencySource.Find(x => x.id == taskGroup.currency_Id))];
                }
                if(taskGroup.isTitle == true)
                {
                    //lblParentGroup.Visibility = Visibility.Collapsed;
                    //chkHasParent.Visibility = Visibility.Collapsed;
                    //lookupParentGroup.Visibility = Visibility.Collapsed;
                    grdCalculationType.Visibility = Visibility.Collapsed;
                    grdModuleType.Visibility = Visibility.Collapsed;
                    UIvisibility();
                }
                if (taskGroup.Companies.Count == 0 && taskGroup.Departments.Count == 0 && taskGroup.CompaniesBulk.Count == 0 && taskGroup.DepartmentsBulk.Count == 0)
                {
                    //if(taskGroup.creationDate.Day < 17)
                    //{
                    //    template = TaskGroupTemplate.Standard;
                    //    //tabCompDepts.Visibility = Visibility.Collapsed;
                    //    loadcompanies();
                    //}
                    //else
                    //{
                        template = TaskGroupTemplate.Optional;
                    //tabCompDepts.Visibility = Visibility.Collapsed;
                    //loadcompanies();
                    //}
                    UIvisibility();
                    
                    LoadUsers();
                    selectedUsers = taskGroup.users;
                    foreach (User _user in selectedUsers)
                    {
                        allUsers.Remove(_user);
                    }

                    selectedUsersBulk = taskGroup.usersBulk;
                    foreach (User _user in selectedUsersBulk)
                    {
                        allUsersBulk.Remove(_user);
                    }
                    grdCntrlUsers.ItemsSource = null;
                    grdCntrlSelectedUsers.ItemsSource = null;
                    grdCntrlUsers.ItemsSource = allUsers;
                    grdCntrlSelectedUsers.ItemsSource = selectedUsers;

                    grdCntrlUsersBulk.ItemsSource = null;
                    grdCntrlSelectedUsersBulk.ItemsSource = null;
                    grdCntrlUsersBulk.ItemsSource = allUsersBulk;
                    grdCntrlSelectedUsersBulk.ItemsSource = selectedUsersBulk;
                }
                else
                {
                    template = TaskGroupTemplate.Standard;
                    //tabCompDepts.Visibility = Visibility.Visible;
                    loadcompanies();

                    if ((taskGroup.Companies == null || taskGroup.Companies.Count == 0) && (taskGroup.Departments == null || taskGroup.Departments.Count == 0))
                    {
                        tabGroupDetails.IsEnabled = false;
                    }

                    if ((taskGroup.CompaniesBulk == null || taskGroup.CompaniesBulk.Count == 0) && (taskGroup.DepartmentsBulk == null || taskGroup.DepartmentsBulk.Count == 0))
                    {
                        tabGroupDetailsBulk.IsEnabled = false;
                    }

                    foreach (var _company in taskGroup.Companies)
                    {
                        var cmpny = allCompanies.FirstOrDefault(x => x.Id == _company.Id);
                        if (cmpny != null)
                        {
                            selectedCompanies.Add(cmpny);
                            allCompanies.Remove(cmpny);
                            allDepartments.AddRange(cmpny.departments);
                        }

                    }

                    allDepartments = allDepartments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    foreach (var _dept in taskGroup.Departments)
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



                    List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                    foreach (var _dept in selectedDepartments)
                    {
                        employees.AddRange((_dept as Department).employees.Where(x => x.isActive == true).ToList());
                    }
                    employees = employees.Distinct().ToList();

                    var empIds = employees.Where(x => x.Companies.Intersect(selectedCompanies) != null).Select(y => y.EmpId).ToList();
                    allUsers = taskRepo.getAllusersByEmpIds(empIds);
                    selectedUsers = taskGroup.users;
                    foreach (User _user in selectedUsers)
                    {
                        allUsers.Remove(_user);
                    }

                    gridDepartment.ItemsSource = allDepartments;
                    gridSelectedDepartments.ItemsSource = selectedDepartments;
                    gridSelectedCompanies.ItemsSource = selectedCompanies;
                    grdCntrlCompany.ItemsSource = allCompanies;
                    grdCntrlUsers.ItemsSource = allUsers;
                    grdCntrlSelectedUsers.ItemsSource = selectedUsers;

                    foreach (var _company in taskGroup.CompaniesBulk)
                    {
                        var cmpny = allCompaniesBulk.FirstOrDefault(x => x.Id == _company.Id);
                        if (cmpny != null)
                        {
                            selectedCompaniesBulk.Add(cmpny);
                            allCompaniesBulk.Remove(cmpny);
                            allDepartmentsBulk.AddRange(cmpny.departments.Where(x => x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());
                        }

                    }

                    switch ((TargetsTransactionType)cmbxModuleType.SelectedIndex)
                    {
                        case TargetsTransactionType.Inquiry:
                            allDepartmentsBulk = allDepartmentsBulk.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList();
                            break;
                        case TargetsTransactionType.Offer:
                            allDepartmentsBulk = allDepartmentsBulk.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList();
                            break;
                        case TargetsTransactionType.PurchaseInvoice:
                            allDepartmentsBulk = allDepartmentsBulk.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList();
                            break;
                        case TargetsTransactionType.PurchaseOrder:
                            allDepartmentsBulk = allDepartmentsBulk.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList();
                            break;
                        case TargetsTransactionType.SaleInvoice:
                            allDepartmentsBulk = allDepartmentsBulk.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList();
                            break;
                        case TargetsTransactionType.SaleOrder:
                            allDepartmentsBulk = allDepartmentsBulk.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList();
                            break;
                        case TargetsTransactionType.SaleReceipt:
                            allDepartmentsBulk = allDepartmentsBulk.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList();
                            break;
                        case TargetsTransactionType.VendorBill:
                            allDepartmentsBulk = allDepartmentsBulk.Where(x => x.IsVendorBillType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList();
                            break;
                    }

                    allDepartmentsBulk = allDepartmentsBulk.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    foreach (var _dept in taskGroup.DepartmentsBulk)
                    {

                        selectedDepartmentsBulk.Add(_dept);

                    }


                    List<ERP_BL.Databases.Employee> employeesBulk = new List<ERP_BL.Databases.Employee>();
                    foreach (var _dept in selectedDepartmentsBulk)
                    {
                        employeesBulk.AddRange((_dept as Department).employees.Where(x => x.isActive == true).ToList());
                    }
                    employeesBulk = employeesBulk.Distinct().ToList();

                    var empIdsBulk = employeesBulk.Where(x => x.Companies.Intersect(selectedCompaniesBulk) != null).Select(y => y.EmpId).ToList();
                    allUsersBulk = taskRepo.getAllusersByEmpIds(empIdsBulk);
                    selectedUsersBulk = taskGroup.usersBulk;
                    foreach (User _user in selectedUsersBulk)
                    {
                        allUsersBulk.Remove(_user);
                    }

                    
                    gridDepartmentBulk.ItemsSource = allDepartmentsBulk;
                    //gridSelectedDepartmentsBulk.ItemsSource = selectedDepartmentsBulk;
                    gridSelectedCompaniesBulk.ItemsSource = selectedCompaniesBulk;
                    grdCntrlCompanyBulk.ItemsSource = allCompaniesBulk;
                    grdCntrlUsersBulk.ItemsSource = allUsersBulk;
                    grdCntrlSelectedUsersBulk.ItemsSource = selectedUsersBulk;
                }

                
                txtItemTitle.Text = taskGroup.GroupName;

                if (taskGroup.parentGroup != null)
                {
                    chkHasParent.IsChecked = true;
                    lookupParentGroup.Text = taskGroup.parentGroup.GroupName;
                }


                

                //foreach (User _user in taskGroup.users)
                //    grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));


            }
        }

        private void UIvisibility()
        {
            layoutGrpDepts.Visibility = Visibility.Collapsed;
            layoutGrpCompanies.Visibility = Visibility.Collapsed;
            layoutGrpUsers.SetValue(Grid.RowProperty, 0);
            layoutGrpUsers.SetValue(Grid.RowSpanProperty, 2);

            tabBulkCompanies.Visibility = Visibility.Collapsed;
            tabBulkDepartments.Visibility = Visibility.Collapsed;
        }

        private void LoadGroups()
        {
            var groups = taskRepo.GetAllTaskGroups();
            lookupParentGroup.ItemsSource = groups;
        }


        private void LoadUsers()
        {
            var users = taskRepo.getAllusers();
            grdCntrlUsers.ItemsSource = users;
            allUsers = users;

            users = taskRepo.getAllusers();
            grdCntrlUsersBulk.ItemsSource = users;
            allUsersBulk = users;
        }

        private void LoadTargetGroups()
        {
            lookUpTargetGroup.ItemsSource = taskRepo.GetAllTargetGroups();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtItemTitle.Text))
                {
                    DXMessageBox.Show("Please Enter Group Title!");
                    txtItemTitle.Focus();
                    return;
                }
                if (chkHasParent.IsChecked == true)
                {

                    if (lookupParentGroup.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please Select Modules Type!");
                        lookupParentGroup.Focus();
                        return;
                    }
                }
                if (isTitle == false)
                {
                    if (lookUpCalculationType.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Calculation Type!");
                        lookUpCalculationType.Focus();
                        return;
                    }
                    if (lookUpTargetGroup.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Group Level!");
                        lookUpTargetGroup.Focus();
                        return;
                    }
                    if (cmbxModuleType.SelectedIndex < 0)
                    {
                        DXMessageBox.Show("Please select Group Level!");
                        lookUpTargetGroup.Focus();
                        return;
                    }
                    
                }
                
                
                if (isTitle == true)
                {
                    taskGroup.isTitle = isTitle;
                }

                //billNature.companyId = (lookupCompany.SelectedItem as Company).Id; 
                if (template == TaskGroupTemplate.Standard && isTitle ==false)
                {
                    if (selectedCompanies.Count == 0 && selectedCompaniesBulk.Count == 0)
                    {
                        DXMessageBox.Show("Please select at least one Company!");
                        return;
                    }
                    if (selectedDepartments.Count == 0 && selectedDepartmentsBulk.Count == 0)
                    {
                        DXMessageBox.Show("Please select at least one Department!");
                        return;
                    }

                    if (selectedCompanies.Count != 0)
                    {
                        taskGroup.Companies = new List<Company>();
                        foreach (Company _company in selectedCompanies)
                        {
                            if (!taskGroup.Companies.Contains(_company))
                            {
                                taskGroup.Companies.Add(_company);
                            }
                        }
                    }
                    if (selectedDepartments.Count != 0)
                    {
                        taskGroup.Departments = new List<Department>();
                        foreach (Department _dept in selectedDepartments)
                        {
                            if (!taskGroup.Departments.Contains(_dept))
                            {
                                taskGroup.Departments.Add(_dept);
                            }
                        }
                    }

                    if (selectedCompaniesBulk.Count != 0)
                    {
                        taskGroup.CompaniesBulk = new List<Company>();
                        foreach (Company _company in selectedCompaniesBulk)
                        {
                            if (!taskGroup.CompaniesBulk.Contains(_company))
                            {
                                taskGroup.CompaniesBulk.Add(_company);
                            }
                        }
                    }
                    if (selectedDepartmentsBulk.Count != 0)
                    {
                        taskGroup.DepartmentsBulk = new List<Department>();
                        foreach (Department _dept in selectedDepartmentsBulk)
                        {
                            if (!taskGroup.DepartmentsBulk.Contains(_dept))
                            {
                                taskGroup.DepartmentsBulk.Add(_dept);
                            }
                        }

                        System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                        gridDepartmentBulk.SaveLayoutToStream(memoryStream);
                        taskGroup.settingValue = SYSTEM_STATIC.ConvertToString(memoryStream).Trim();
                    }
                    else
                    {
                        taskGroup.settingValue = null;
                    }

                    taskGroup.targetsTransactionType = (ERP_BL.Enums.TargetsTransactionType)cmbxModuleType.SelectedIndex;
                }
                if (lookupCGSAccounts.SelectedIndex > -1)
                {
                    taskGroup.cgsAccount_Id = (lookupCGSAccounts.SelectedItem as ChartofAccount).Id;
                }
                if (lookupPayableAccounts.SelectedIndex > -1)
                {
                    taskGroup.payableAccount_Id = (lookupPayableAccounts.SelectedItem as ChartofAccount).Id;

                }
                //if(lookupCompany.SelectedIndex>-1)
                //{
                //    taskGroup.companyId = (lookupCompany.SelectedItem as Company).Id;
                //}
                //if (lookupDepartment.SelectedIndex > 0)
                //{
                //    taskGroup.dept_Id = (lookupDepartment.SelectedItem as Department).Id;
                //}
                if (cmbCurrency.SelectedIndex > -1)
                {
                    taskGroup.currency_Id = (cmbCurrency.SelectedItem as cmbitem).id;
                }


               
                if (selectedUsers.Count == 0 && selectedUsersBulk.Count == 0)
                {
                    DXMessageBox.Show("Please select Users!");
                    return;
                }

                taskGroup.users = new List<User>();
                foreach (User _user in selectedUsers)
                {
                    if (!taskGroup.users.Contains(_user))
                    {
                        taskGroup.users.Add(_user);
                    }
                }
                

                taskGroup.usersBulk = new List<User>();
                foreach (User _user in selectedUsersBulk)
                {
                    if (!taskGroup.usersBulk.Contains(_user))
                    {
                        taskGroup.usersBulk.Add(_user);
                    }
                }




                //if (grdCntrlUsers.SelectedItems.Count != 0)
                //{
                //    taskGroup.users = new List<User>();
                //    foreach (User _user in grdCntrlUsers.SelectedItems)
                //    {
                //        if (!taskGroup.users.Contains(_user))
                //        {
                //            taskGroup.users.Add(_user);
                //        }
                //    }
                //}

                taskGroup.GroupName = txtItemTitle.Text;
                if (isTitle == false)
                {
                    
                    taskGroup.CalculationType_Id = (lookUpCalculationType.SelectedItem as StatusCalculationType).Id;


                    taskGroup.targetGroup_Id = (lookUpTargetGroup.SelectedItem as TargetGroup).Id;
                }
             
                

                if (chkHasParent.IsChecked == true)
                {

                    if (lookupParentGroup.SelectedIndex > -1)
                        taskGroup.parentId = (lookupParentGroup.SelectedItem as TaskGroups).Id;
                }
                else
                    taskGroup.parentId = null;

                if (editFlag == true)
                {
                    taskRepo.UpdateTaskGroup(taskGroup);
                    DXMessageBox.Show("Updated Successfully!");
                }
                else if(editFlag == false)
                {
                    taskGroup.GroupCreatorId = SYSTEM_STATIC.currentUser.id;
                    taskGroup.creationDate = DateTime.Now;
                    taskRepo.AddTaskGroup(taskGroup);
                    DXMessageBox.Show("Successfully Added!");
                }

                
                    Window thisWindow = Window.GetWindow(this);
                    thisWindow.Close();
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message);
            }
        }

       
        private void LookUpDepartment_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {

        }

        private void LookUpDepartment_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        public void loadcompanies()
        {
            CompanyRepo companyRepo = new CompanyRepo();
            allCompanies = companyRepo.GetUserCompanies(SYSTEM_STATIC.currentUser.id);
            grdCntrlCompany.ItemsSource = allCompanies;

            companyRepo = new CompanyRepo();
            allCompaniesBulk = companyRepo.GetUserCompanies(SYSTEM_STATIC.currentUser.id);
            grdCntrlCompanyBulk.ItemsSource = allCompaniesBulk;
        }

        private void TabUsers_GotFocus(object sender, RoutedEventArgs e)
        {
            //if(template == TaskGroupTemplate.Standard)
            //{
            //    List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
            //    foreach (var _dept in gridDepartment.SelectedItems)
            //    {
            //        employees.AddRange((_dept as Department).employees.Where(x => x.isActive == true).ToList());
            //    }
            //    employees = employees.Distinct().ToList();

            //    List<Company> Companiess = new List<Company>();
            //    foreach (var _comp in grdCntrlCompanies.SelectedItems)
            //    {
            //        Companiess.Add(_comp as Company);
            //    }

            //    var empIds = employees.Where(x => x.Companies.Intersect(Companiess) != null).Select(y => y.EmpId).ToList();
            //    grdCntrlUsers.ItemsSource = taskRepo.getAllusersByEmpIds(empIds);

            //    foreach (User _user in selectedUsers)
            //    {
            //        grdCntrlUsers.SelectItem(grdCntrlUsers.FindRowByValue(grdCntrlUsers.Columns.GetColumnByFieldName("id"), _user.id));
            //    }
            //}   
        }

        private void TabCompDepts_GotFocus(object sender, RoutedEventArgs e)
        {
            selectedUsers = new List<User>();
            foreach (var _user in grdCntrlUsers.SelectedItems)
            {
                selectedUsers.Add(_user as User);
            }
        }

        private void ChkHasParent_Checked(object sender, RoutedEventArgs e)
        {
            lookupParentGroup.IsEnabled = true;
        }

        private void ChkHasParent_Unchecked(object sender, RoutedEventArgs e)
        {
            lookupParentGroup.IsEnabled = false;
        }

        private void LookupParentGroup_PopupOpened(object sender, RoutedEventArgs e)
        {
            ColumnsVisibility();
        }

        private void ColumnsVisibility()
        {
            var grid = lookupParentGroup.GetGridControl();

            grid.Columns["creationDate"].Visible = false;
            grid.Columns["parentGroup"].Visible = false;
            grid.Columns["GroupCreatorId"].Visible = false;
            grid.Columns["GroupCreator"].Visible = false;

            grid.Columns["creationDate"].ShowInColumnChooser = false;
            grid.Columns["parentGroup"].ShowInColumnChooser = false;
            grid.Columns["GroupCreatorId"].ShowInColumnChooser = false;
            grid.Columns["GroupCreator"].ShowInColumnChooser = false;
        }

        private void TabUsers_LostFocus(object sender, RoutedEventArgs e)
        {
            selectedUsers = new List<User>();
            foreach (var _user in grdCntrlUsers.SelectedItems)
            {
                selectedUsers.Add(_user as User);
            }
        }



        private void ImgLeftToRightComp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightComp.Width = 28;
        }

        private void ImgLeftToRightComp_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightComp.Width = 30;
            if (grdCntrlCompany.SelectedItem != null)
            {
                var company = grdCntrlCompany.SelectedItem as Company;
                selectedCompanies.Add(company);
                allCompanies.Remove(company);
                grdCntrlCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                grdCntrlCompany.ItemsSource = allCompanies;
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

                grdCntrlCompany.ItemsSource = null;
                gridSelectedCompanies.ItemsSource = null;

                grdCntrlCompany.ItemsSource = allCompanies;
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

                    if (template == TaskGroupTemplate.Standard)
                    {
                        List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                        foreach (var _dept in selectedDepartments)
                        {
                            employees.AddRange(_dept.employees.Where(x => x.isActive == true).ToList());
                        }
                        employees = employees.Distinct().ToList();

                        var empIds = employees.Where(x => x.Companies.Intersect(selectedCompanies) != null).Select(y => y.EmpId).ToList();
                        allUsers = taskRepo.getAllusersByEmpIds(empIds);

                        foreach (User _user in selectedUsers)
                        {
                            var user = allUsers.Find(x => x.id == _user.id);
                            if (user != null)
                            {
                                allUsers.Remove(user);
                            }
                        }

                        grdCntrlUsers.ItemsSource = null;
                        grdCntrlUsers.ItemsSource = allUsers;
                    }
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
            imgRightToLeftDept.Width = 30;

            if (gridSelectedDepartments.SelectedItem != null)
            {
                var department = gridSelectedDepartments.SelectedItem as Department;

                var userIds = selectedUsers.Select(y => y.id).ToList();
                var userss = department.employees.SelectMany(x=>x.EmployeeUsers).ToList();
                var uIds = userss.Select(x=>x.id).ToList();
                var commonUsers = userIds.Intersect(uIds).ToList();


                List<ERP_BL.Databases.Employee> employeess = new List<ERP_BL.Databases.Employee>();
                foreach (var _dept in selectedDepartments.Where(x=>x.Id != department.Id).ToList())
                {
                    employeess.AddRange(_dept.employees.Where(x => x.isActive == true).ToList());
                }
                employeess = employeess.Distinct().ToList();

                var empIdss = employeess.Where(x => x.Companies.Intersect(selectedCompanies) != null).Select(y => y.EmpId).ToList();
                var allUserIds = taskRepo.getAllusersByEmpIds(empIdss).Select(x=>x.id).ToList();
                commonUsers = commonUsers.Except(allUserIds).ToList();


                if (commonUsers != null && commonUsers.Count > 0)
                {
                    DXMessageBox.Show("Kindly remove the Users of this Department from Selected Users!");
                    return;
                }


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

                    if (template == TaskGroupTemplate.Standard)
                    {
                        List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                        foreach (var _dept in selectedDepartments)
                        {
                            employees.AddRange(_dept.employees.Where(x => x.isActive == true).ToList());
                        }
                        employees = employees.Distinct().ToList();

                        var empIds = employees.Where(x => x.Companies.Intersect(selectedCompanies) != null).Select(y => y.EmpId).ToList();
                        allUsers = taskRepo.getAllusersByEmpIds(empIds);

                        foreach (User _user in selectedUsers)
                        {
                            var user = allUsers.Find(x => x.id == _user.id);
                            if (user != null)
                            {
                                allUsers.Remove(user);
                            }
                        }
                    }
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
            imgRightToLeftDept.Width = 28;
        }

        private void ImgLeftToRightUsers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightUsers.Width = 28;
        }

        private void ImgLeftToRightUsers_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightUsers.Width = 30;
            if (grdCntrlUsers.SelectedItem != null)
            {
                var user = grdCntrlUsers.SelectedItem as User;
                var deptss = new List<Department>();
                foreach(var _dept in selectedDepartments)
                {
                    if((_dept.employees.SelectMany(x=>x.EmployeeUsers).ToList()).Find(y=>y.id == user.id) == null)
                    {
                        deptss.Add(_dept);
                        
                    }
                }

                if (deptss != null && deptss.Count > 0)
                {
                    if (DXMessageBox.Show("This user is not the part of: \n\n" + String.Join(" , ", deptss.Select(x => x.DeptName)) + ". \n\nDo you still want to continue?", "User Selection", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                    if (DXMessageBox.Show("This user is not the part of: \n\n " + String.Join(" , ", deptss.Select(x => x.DeptName)) + ". \n\nDo you still want to continue?", "User Selection", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                }


                selectedUsers.Add(user);
                allUsers.Remove(user);
                grdCntrlUsers.ItemsSource = null;
                grdCntrlSelectedUsers.ItemsSource = null;

                grdCntrlUsers.ItemsSource = allUsers;
                grdCntrlSelectedUsers.ItemsSource = selectedUsers;
            }
            else
            {
                DXMessageBox.Show("Please select User which you want to Insert to Selected Users!");
            }
        }

        private void ImgRightToLeftUsers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftUsers.Width = 28;
        }

        private void ImgRightToLeftUsers_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftUsers.Width = 30;
            if (grdCntrlSelectedUsers.SelectedItem != null)
            {
                var user = grdCntrlSelectedUsers.SelectedItem as User;

               
                selectedUsers.Remove(user);
                allUsers.Add(user);

                grdCntrlUsers.ItemsSource = null;
                grdCntrlSelectedUsers.ItemsSource = null;

                grdCntrlUsers.ItemsSource = allUsers;
                grdCntrlSelectedUsers.ItemsSource = selectedUsers;
            }
            else
            {
                DXMessageBox.Show("Please select User which you want to Insert to Selected Users!");
            }
        }

        private void ImgLeftToRightUsersBulk_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightUsersBulk.Width = 30;
            if (grdCntrlUsersBulk.SelectedItem != null)
            {
                var user = grdCntrlUsersBulk.SelectedItem as User;
                var deptss = new List<Department>();
                foreach (var _dept in selectedDepartmentsBulk)
                {
                    if ((_dept.employees.SelectMany(x => x.EmployeeUsers).ToList()).Find(y => y.id == user.id) == null)
                    {
                        deptss.Add(_dept);

                    }
                }

                if (deptss != null && deptss.Count > 0)
                {
                    if (DXMessageBox.Show("This user is not the part of: \n\n" + String.Join(" , ", deptss.Select(x => x.DeptName)) + ". \n\nDo you still want to continue?", "User Selection", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                    if (DXMessageBox.Show("This user is not the part of: \n\n " + String.Join(" , ", deptss.Select(x => x.DeptName)) + ". \n\nDo you still want to continue?", "User Selection", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                }


                selectedUsersBulk.Add(user);
                allUsersBulk.Remove(user);
                grdCntrlUsersBulk.ItemsSource = null;
                grdCntrlSelectedUsersBulk.ItemsSource = null;

                grdCntrlUsersBulk.ItemsSource = allUsersBulk;
                grdCntrlSelectedUsersBulk.ItemsSource = selectedUsersBulk;
            }
            else
            {
                DXMessageBox.Show("Please select User which you want to Insert to Selected Users!");
            }
        }

        private void ImgRightToLeftUsersBulk_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftUsersBulk.Width = 30;
            if (grdCntrlSelectedUsersBulk.SelectedItem != null)
            {
                var user = grdCntrlSelectedUsersBulk.SelectedItem as User;


                selectedUsersBulk.Remove(user);
                allUsersBulk.Add(user);

                grdCntrlUsersBulk.ItemsSource = null;
                grdCntrlSelectedUsersBulk.ItemsSource = null;

                grdCntrlUsersBulk.ItemsSource = allUsersBulk;
                grdCntrlSelectedUsersBulk.ItemsSource = selectedUsersBulk;
            }
            else
            {
                DXMessageBox.Show("Please select User which you want to Insert to Selected Users!");
            }
        }

        private void ImgRightToLeftUsersBulk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftUsersBulk.Width = 28;
        }

        private void ImgLeftToRightUsersBulk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightUsersBulk.Width = 28;
        }

        private void ImgLeftToRightCompBulk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCompBulk.Width = 28;
        }

        private void ImgRightToLeftCompBulk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCompBulk.Width = 28;
        }

        private void ImgLeftToRightCompBulk_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgLeftToRightCompBulk.Width = 30;

            if(cmbxModuleType.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Module Type!");
                return;
            }
            
            if (grdCntrlCompanyBulk.SelectedItem != null)
            {
                var company = grdCntrlCompanyBulk.SelectedItem as Company;
                selectedCompaniesBulk.Add(company);
                allCompaniesBulk.Remove(company);
                grdCntrlCompanyBulk.ItemsSource = null;
                gridSelectedCompaniesBulk.ItemsSource = null;

                grdCntrlCompanyBulk.ItemsSource = allCompaniesBulk;
                gridSelectedCompaniesBulk.ItemsSource = selectedCompaniesBulk;

                switch ((TargetsTransactionType)cmbxModuleType.SelectedIndex)
                {
                    case TargetsTransactionType.Inquiry:
                        allDepartmentsBulk.AddRange(company.departments.Where(x=>x.IsProcurementType == true && x.employees.FirstOrDefault(y=>y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());
                        break;
                    case TargetsTransactionType.Offer:
                        allDepartmentsBulk.AddRange(company.departments.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());
                        break;
                    case TargetsTransactionType.PurchaseInvoice:
                        allDepartmentsBulk.AddRange(company.departments.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());
                        break;
                    case TargetsTransactionType.PurchaseOrder:
                        allDepartmentsBulk.AddRange(company.departments.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());
                        break;
                    case TargetsTransactionType.SaleInvoice:
                        allDepartmentsBulk.AddRange(company.departments.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());
                        break;
                    case TargetsTransactionType.SaleOrder:
                        allDepartmentsBulk.AddRange(company.departments.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());
                        break;
                    case TargetsTransactionType.SaleReceipt:
                        allDepartmentsBulk.AddRange(company.departments.Where(x => x.IsProcurementType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());
                        break;
                    case TargetsTransactionType.VendorBill:
                        allDepartmentsBulk.AddRange(company.departments.Where(x => x.IsVendorBillType == true && x.employees.FirstOrDefault(y => y.EmpId == SYSTEM_STATIC.currentUser.employee.EmpId) != null).ToList());
                        break;
                }

               

                allDepartmentsBulk = allDepartmentsBulk.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                gridDepartmentBulk.ItemsSource = null;
                gridDepartmentBulk.ItemsSource = allDepartmentsBulk;
            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }

        private void ImgRightToLeftCompBulk_MouseUp(object sender, MouseButtonEventArgs e)
        {
            imgRightToLeftCompBulk.Width = 30;
            if (gridSelectedCompaniesBulk.SelectedItem != null)
            {
                var company = gridSelectedCompaniesBulk.SelectedItem as Company;

                if (company.departments.Intersect(selectedDepartmentsBulk).Count() > 0)
                {
                    DXMessageBox.Show("Kindly remove the Departments of this Company from Selected Departments!");
                    return;
                }
                selectedCompaniesBulk.Remove(company);
                allCompaniesBulk.Add(company);

                grdCntrlCompanyBulk.ItemsSource = null;
                gridSelectedCompaniesBulk.ItemsSource = null;

                grdCntrlCompanyBulk.ItemsSource = allCompaniesBulk;
                gridSelectedCompaniesBulk.ItemsSource = selectedCompaniesBulk;

                var depts = selectedDepartmentsBulk.Except(allDepartmentsBulk.Intersect(selectedDepartmentsBulk));
                allDepartmentsBulk = new List<Department>();
                foreach (var _cmpny in selectedCompaniesBulk)
                {

                    switch ((TargetsTransactionType)cmbxModuleType.SelectedIndex)
                    {
                        case TargetsTransactionType.Inquiry:
                            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                            break;
                        case TargetsTransactionType.Offer:
                            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                            break;
                        case TargetsTransactionType.PurchaseInvoice:
                            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                            break;
                        case TargetsTransactionType.PurchaseOrder:
                            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                            break;
                        case TargetsTransactionType.SaleInvoice:
                            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                            break;
                        case TargetsTransactionType.SaleOrder:
                            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                            break;
                        case TargetsTransactionType.SaleReceipt:
                            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsProcurementType == true).ToList());
                            break;
                        case TargetsTransactionType.VendorBill:
                            allDepartmentsBulk.AddRange(_cmpny.departments.Where(x => x.IsVendorBillType == true).ToList());
                            break;
                    }
                }


                allDepartmentsBulk = allDepartmentsBulk.Except(depts).ToList();

                allDepartmentsBulk = allDepartmentsBulk.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                gridDepartmentBulk.ItemsSource = null;
                gridDepartmentBulk.ItemsSource = allDepartmentsBulk;
                gridSelectedDepartmentsBulk.ItemsSource = null;
                gridSelectedDepartmentsBulk.ItemsSource = selectedDepartmentsBulk;

            }
            else
            {
                DXMessageBox.Show("Please select Company which you want to Insert to Selected Companies!");
            }
        }

        private void ImgLeftToRightDeptBulk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //imgLeftToRightDeptBulk.Width = 28;
        }

        private void ImgRightToLeftDeptBulk_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //imgRightToLeftDeptBulk.Width = 28;
        }

        private void ImgLeftToRightDeptBulk_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //imgLeftToRightDeptBulk.Width = 30;

            if (gridDepartmentBulk.SelectedItem != null)
            {
                var department = gridDepartmentBulk.SelectedItem as Department;
                if (allDepartmentsBulk.Find(x => x.ParentID == department.Id) == null)
                {

                    allDepartmentsBulk.Remove(department);
                    if (!selectedDepartmentsBulk.Contains(department))
                        selectedDepartmentsBulk.Add(department);

                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (allDepartmentsBulk.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!selectedDepartmentsBulk.Contains(parent))
                                selectedDepartmentsBulk.Add(parent);
                            var findParet = allDepartmentsBulk.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                allDepartmentsBulk.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }

                    gridDepartmentBulk.ItemsSource = null;
                    gridSelectedDepartmentsBulk.ItemsSource = null;

                    gridDepartmentBulk.ItemsSource = allDepartmentsBulk;
                    gridSelectedDepartmentsBulk.ItemsSource = selectedDepartmentsBulk;

                    if (template == TaskGroupTemplate.Standard)
                    {
                        List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                        foreach (var _dept in selectedDepartmentsBulk)
                        {
                            employees.AddRange(_dept.employees.Where(x => x.isActive == true).ToList());
                        }
                        employees = employees.Distinct().ToList();

                        var empIds = employees.Where(x => x.Companies.Intersect(selectedCompaniesBulk) != null).Select(y => y.EmpId).ToList();
                        allUsersBulk = taskRepo.getAllusersByEmpIds(empIds);

                        foreach (User _user in selectedUsersBulk)
                        {
                            var user = allUsersBulk.Find(x => x.id == _user.id);
                            if (user != null)
                            {
                                allUsersBulk.Remove(user);
                            }
                        }

                        grdCntrlUsersBulk.ItemsSource = null;
                        grdCntrlUsersBulk.ItemsSource = allUsersBulk;
                    }
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

        private void ImgRightToLeftDeptBulk_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //imgRightToLeftDeptBulk.Width = 30;

            if (gridSelectedDepartmentsBulk.SelectedItem != null)
            {
                var department = gridSelectedDepartmentsBulk.SelectedItem as Department;

                var userIds = selectedUsersBulk.Select(y => y.id).ToList();
                var userss = department.employees.SelectMany(x => x.EmployeeUsers).ToList();
                var uIds = userss.Select(x => x.id).ToList();
                var commonUsers = userIds.Intersect(uIds).ToList();


                List<ERP_BL.Databases.Employee> employeess = new List<ERP_BL.Databases.Employee>();
                foreach (var _dept in selectedDepartmentsBulk.Where(x => x.Id != department.Id).ToList())
                {
                    employeess.AddRange(_dept.employees.Where(x => x.isActive == true).ToList());
                }
                employeess = employeess.Distinct().ToList();

                var empIdss = employeess.Where(x => x.Companies.Intersect(selectedCompaniesBulk) != null).Select(y => y.EmpId).ToList();
                var allUserIds = taskRepo.getAllusersByEmpIds(empIdss).Select(x => x.id).ToList();
                commonUsers = commonUsers.Except(allUserIds).ToList();


                if (commonUsers != null && commonUsers.Count > 0)
                {
                    DXMessageBox.Show("Kindly remove the Users of this Department from Selected Users!");
                    return;
                }


                if (selectedDepartmentsBulk.Find(x => x.ParentID == department.Id) == null)
                {
                    selectedDepartmentsBulk.Remove(department);
                    if (!allDepartmentsBulk.Contains(department))
                        allDepartmentsBulk.Add(department);

                    var parent = department.parentDepartment;
                    while (parent != null)
                    {
                        if (selectedDepartmentsBulk.Find(x => x.Id == parent.Id) != null)
                        {
                            if (!allDepartmentsBulk.Contains(parent))
                                allDepartmentsBulk.Add(parent);
                            var findParet = selectedDepartmentsBulk.Find(x => x.parentDepartment == parent);
                            if (findParet == null)
                            {
                                selectedDepartmentsBulk.Remove(parent);
                            }
                        }
                        parent = parent.parentDepartment;
                    }

                    gridDepartmentBulk.ItemsSource = null;
                    gridSelectedDepartmentsBulk.ItemsSource = null;

                    gridDepartmentBulk.ItemsSource = allDepartmentsBulk;
                    gridSelectedDepartmentsBulk.ItemsSource = selectedDepartmentsBulk;

                    if (template == TaskGroupTemplate.Standard)
                    {
                        List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                        foreach (var _dept in selectedDepartmentsBulk)
                        {
                            employees.AddRange(_dept.employees.Where(x => x.isActive == true).ToList());
                        }
                        employees = employees.Distinct().ToList();

                        var empIds = employees.Where(x => x.Companies.Intersect(selectedCompaniesBulk) != null).Select(y => y.EmpId).ToList();
                        allUsersBulk = taskRepo.getAllusersByEmpIds(empIds);

                        foreach (User _user in selectedUsersBulk)
                        {
                            var user = allUsersBulk.Find(x => x.id == _user.id);
                            if (user != null)
                            {
                                allUsersBulk.Remove(user);
                            }
                        }
                    }
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

        string filterString;

        private void GridDepartmentBulk_FilterChanged(object sender, RoutedEventArgs e)
        {
            
                if (template == TaskGroupTemplate.Standard)
                {
                    List<ERP_BL.Databases.Employee> employees = new List<ERP_BL.Databases.Employee>();
                    List<Department> tempDepts = new List<Department>();

                var uIds = selectedUsersBulk.Select(x=>x.id).ToList();

                foreach (var _dept in gridDepartmentBulk.VisibleItems)
                {
                    var dept = _dept as Department;
                    tempDepts.Add(dept);
                    employees.AddRange(dept.employees.Where(x => x.isActive == true).ToList());
                }
                var selectedDeptIds = selectedDepartmentsBulk.Select(x=>x.Id).ToList();
                var filteredDeptIds = tempDepts.Select(x => x.Id).ToList();
                var removedDeptIds = selectedDeptIds.Except(filteredDeptIds).ToList();
                var removedDepts = selectedDepartmentsBulk.Where(x=>removedDeptIds.Contains( x.Id)).ToList();
                removedDepts.AddRange(tempDepts.Where(x=>removedDeptIds.Contains(x.Id) && !removedDepts.Select(y=>y.Id).ToList().Contains(x.Id)));

                var removedUids = removedDepts.SelectMany(x=>x.employees).SelectMany(x=>x.EmployeeUsers).Select(x=>x.id).ToList();
                var commonIds = uIds.Intersect(removedUids).ToList();
                if (commonIds != null && commonIds.Count() > 0) 
                {
                    DXMessageBox.Show("Kindly remove the Users of this Department from Selected Users!");
                    gridDepartmentBulk.FilterString = filterString;
                    return;
                }

                selectedDepartmentsBulk = tempDepts;
                filterString = gridDepartmentBulk.FilterString;


                    employees = employees.Distinct().ToList();
                    
                    var empIds = employees.Where(x => x.Companies.Intersect(selectedCompaniesBulk) != null).Select(y => y.EmpId).ToList();
                    allUsersBulk = taskRepo.getAllusersByEmpIds(empIds);

                        foreach (User _user in selectedUsersBulk)
                        {
                            var user = allUsersBulk.Find(x => x.id == _user.id);
                            if (user != null)
                            {
                            allUsersBulk.Remove(user);
                            }
                        }

                        grdCntrlUsersBulk.ItemsSource = null;
                        grdCntrlUsersBulk.ItemsSource = allUsersBulk;
                    }
              
          
        }

       

        private void GriddeptviewBulk_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "departmentLevel1" && e.IsGetData)
            {
                var row = e.Node.Content as Department;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row;
                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();
                    e.Value = deptList[0].DeptName;
                }
            }
            if (e.Column.FieldName == "departmentLevel2" && e.IsGetData)
            {
                var row = e.Node.Content as Department;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row;
                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }
                    }
                    deptList.Reverse();
                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[1].DeptName;
                            break;
                    }
                }
            }
            if (e.Column.FieldName == "departmentLevel3" && e.IsGetData)
            {
                var row = e.Node.Content as Department;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row;
                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and transverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }
                    }
                    deptList.Reverse();
                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[2].DeptName;
                            break;
                    }
                }
            }
            if (e.Column.FieldName == "departmentLevel4" && e.IsGetData)
            {
                var row = e.Node.Content as Department;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and tranverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }

                    }
                    deptList.Reverse();
                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[3].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[3].DeptName;
                            break;
                    }
                }
            }
            if (e.Column.FieldName == "departmentLevel5" && e.IsGetData)
            {
                var row = e.Node.Content as Department;
                if (row != null)
                {
                    var deptList = new List<Department>();
                    var node = row;

                    while (node != null)
                    {
                        if (node.ParentID != null)
                        {
                            if (node.ParentID != node.Id)
                            {
                                //this will add current node to department list and tranverse to its parent
                                deptList.Add(node);
                                node = node.parentDepartment;
                            }
                            else
                            {
                                //when node is parent to itself
                                deptList.Add(node);
                                break;
                            }
                        }
                        else
                        {
                            //parent with parent id is null
                            deptList.Add(node);
                            break;
                        }
                    }
                    deptList.Reverse();

                    switch (deptList.Count)
                    {
                        case 0:

                            break;
                        case 1:
                            e.Value = deptList[0].DeptName;
                            break;
                        case 2:
                            e.Value = deptList[1].DeptName;
                            break;
                        case 3:
                            e.Value = deptList[2].DeptName;
                            break;
                        case 4:
                            e.Value = deptList[3].DeptName;
                            break;
                        case 5:
                            e.Value = deptList[4].DeptName;
                            break;
                    }
                }
            }
        }
    }
}
