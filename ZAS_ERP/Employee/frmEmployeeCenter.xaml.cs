using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZAS_ERP.Employee;

namespace ZAS_ERP.Employeess
{
    /// <summary>
    /// Interaction logic for frmemployeeMain.xaml
    /// </summary>
    public partial class frmEmployeeCenter : DXWindow
    {
        public static int editemp;
        public static int empid;
        public List<ZAS_ERP.cmbitem> TreeItems = new List<cmbitem>();
        public List<ERP_BL.Databases.Employee> itemSourceEmp = new List<ERP_BL.Databases.Employee>();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        bool isFirstEmpTab = true;
        bool isFirstRegTab = true;

        public string transctions;
        public frmEmployeeCenter()
        {
            InitializeComponent();
        }

        private void winemployeemain_Loaded(object sender, RoutedEventArgs e)
        {
            //Permissions
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View All Employee Register") != null)
            {

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View All Employee Personal Info") != null)
            {

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View All Employee Contact and Address") != null)
            {

            }
            //try
            //{
            //    registersGrid.Children.Clear();
            //    ucEmployeeInfo uc = new ucEmployeeInfo();
            //    registersGrid.Children.Add(uc);

            //}
            //catch { }

            if (registersTab.IsSelected == false)
            {
                loadingGif.Visibility = Visibility.Visible;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += LoadEmployees;
                worker.RunWorkerCompleted += LoademployeesCompleted;
                worker.RunWorkerAsync();
             //   loademployeegrid();
             //   SystemLogic.SetUserSettingOfCurrentWindow(grdemployee);
             //   isFirstEmpTab = false;
             ////   loadingGif.Visibility = Visibility.Hidden;
            }
        


            //GenerateTreeView();
            //AssignItemSource();

         //   GiveEmployeeApprovals();
        }
        private void LoadEmployees(object o, DoWorkEventArgs args)
        {
            Task.Delay(1000).Wait();  // Pretend to work
        }

        private void LoademployeesCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            loademployeegrid();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdemployee);
            isFirstEmpTab = false;
            loadingGif.Visibility = Visibility.Hidden;
        }

        public void GiveEmployeeApprovals()
        {
            var empList = employeeRepo.GetAllEmployees();
            foreach (var _item in empList)
            {
                if (_item.employeeApproval == null)
                {
                    _item.employeeApproval = new EmployeeApproval()
                    {
                        ApprovedDate = DateTime.Now,
                        isApproved = false,
                    };
                    employeeRepo.updateEmployee(_item);
                }
            }
        }
        public void GenerateTreeView()
        {
            //mployeeRepo repo = new EmployeeRepo();

            cmbitem treeItemEmp = new cmbitem() { name = "Employees", description = "Employees" };
            cmbitem treeItemEmpOpen = new cmbitem() { name = "Employees(Open)", description = "Employees" };
            ICollection<cmbitem> cmbItemsEmpOpen = new List<cmbitem>();
            foreach (EmployeeWorkingStatus status in employeeRepo.GetAllActiveEmployeeStatus().OrderBy(x => x.Status).ToList())
            {
                cmbItemsEmpOpen.Add(new cmbitem() { id = status.Id, name = status.Status, isActive = IsActive, bcolor = status.backcolor, description = "Employees" });
            }
            treeItemEmpOpen.Items = cmbItemsEmpOpen;
            treeItemEmp.Items.Add(treeItemEmpOpen);

            cmbitem treeItemFaClose = new cmbitem() { name = "Employees(Closed)", description = "Employees" };

            if ((SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Employees") != null))
            {
                ICollection<cmbitem> cmbItemsEmpClose = new List<cmbitem>();
                foreach (EmployeeWorkingStatus status in employeeRepo.GetAllInActiveEmployeeStatus().OrderBy(x => x.Status).ToList())
                {
                    cmbItemsEmpClose.Add(new cmbitem() { id = status.Id, name = status.Status, bcolor = status.backcolor, description = "Employees" });
                }
                treeItemFaClose.Items = cmbItemsEmpClose;
                treeItemEmp.Items.Add(treeItemFaClose);
            }
            TreeItems.Add(treeItemEmp);

            //Add Tree Items List as Item Source of TreeVIew
            empTreeView.ItemsSource = TreeItems;

        }


        public void AssignItemSource()
        {
            try
            {
                registersGrid.Children.Clear();
                //EmployeeRepo repo = new EmployeeRepo();

                itemSourceEmp = employeeRepo.GetAllEmployeesForRegister();

                ucEmployeeInfo uc = new ucEmployeeInfo();
                uc.grdEmployeeRegisters.ItemsSource = itemSourceEmp;
                SYSTEM_STATIC.SetUserSettingOfCurrentWindow(uc.grdEmployeeRegisters);

                registersGrid.Children.Add(uc);

                uc.grdEmployeeRegisters.Columns["person"].Visible = false;
                uc.grdEmployeeRegisters.Columns["address"].Visible = false;
                uc.grdEmployeeRegisters.Columns["contact"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Desig"].Visible = false;
                uc.grdEmployeeRegisters.Columns["EmpId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["empFunction"].Visible = false;
                uc.grdEmployeeRegisters.Columns["address2"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SalesTargetId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["DesignationTitle"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SupervisorId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SalesTarget"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Supervisor"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }


        private void BarSubItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            frmEmployeeAdd frmemployee = new frmEmployeeAdd();
            frmemployee.Owner = this;
            frmemployee.ShowDialog();
            loademployeegrid();
        }
        ERP_BL.Databases.Employee emp = new ERP_BL.Databases.Employee();
        private void grdemployee_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            // EmployeeRepo emprepo = new EmployeeRepo();
            List<ERP_BL.Databases.Company> employeeCompanies = new List<ERP_BL.Databases.Company>();

            EmployeeRepo emprepo = new EmployeeRepo();
            if (grdemployee.SelectedItem != null)
                emp = emprepo.GetEmployee((grdemployee.SelectedItem as ERP_BL.Databases.Employee).EmpId);
            if (emp != null && emp.departments != null)

                foreach (var compp in emp.Companies)
                {
                    List<Department> employeesDepts = new List<Department>();
                    foreach (Department dept in emp.departments)
                    {
                        foreach (Department depts in compp.departments)
                        {

                            if (dept.Id == depts.Id)
                            {
                                employeesDepts.Add(depts);
                                //detailGridDescriptor.ItemsSourcePath.Remove(i);
                            }

                        }
                    }
                    compp.departments = employeesDepts;
                    employeeCompanies.Add(compp);
                }




            gridCompany.ItemsSource = employeeCompanies;
            //gridCompanyDepartment.ItemsSource = emp.departments;
            //oademployeegrid();


        }


        private void btnNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            frmEmployeeAdd frmemployee = new frmEmployeeAdd();
            frmemployee.Owner = this;
            frmemployee.ShowDialog();
            loademployeegrid();
        }

        private void btnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            editEmployeeclick();
        }
        public void editEmployeeclick()
        {
            editemp = 1;
            if (grdemployee.GetFocusedRowCellValue(grdemployee.Columns.GetColumnByFieldName("EmpId")) != null)
            {
                empid = (int)grdemployee.GetFocusedRowCellValue(grdemployee.Columns.GetColumnByFieldName("EmpId"));

                //MessageBox.Show(empid.ToString());
            }
            frmEmployeeAdd frmemployee = new frmEmployeeAdd();
            frmemployee.Owner = this;
            frmemployee.ShowDialog();
            loademployeegrid();
        }
        private void loademployeegrid()
        {
            //EmployeeRepo emprepo = new EmployeeRepo();
            List<ERP_BL.Databases.Employee> employeelist = new List<ERP_BL.Databases.Employee>();
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View InActive Employees") != null || MainWindow.currentUserid == 0)
            {
                employeelist = employeeRepo.GetAllEmployeesForRegister();
            }
            else
            {
                employeelist = employeeRepo.GetActiveEmployeesForCenter();

            }
            this.grdemployee.ItemsSource = employeelist;

            //grdemployee.AutoGenerateColumns =AutoGenerateColumnsMode.AddNew;

            //grdemployee.Columns.GetColumnByFieldName("EmpId").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("person").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("address").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("contact").Visible = false;
            ////grdemployee.Columns.GetColumnByFieldName("Companies").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Desig").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Disability").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("DisDescription").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("MaritalStatus").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("Status").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("JoinDate").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("BasicPay").Visible = false;
            //grdemployee.Columns.GetColumnByFieldName("isActive").Visible = false;

            //if (grdemployee.Columns.Count == 17 && grdemployee.Columns.GetColumnByFieldName("EmpId").Visible == false)
            //{
            //    {
            //    grdemployee.Columns.Add(new GridColumn() { FieldName = "person.FName" });
            //    grdemployee.Columns.GetColumnByFieldName("person.FName").Header = "First Name";
            //    grdemployee.Columns.Add(new GridColumn() { FieldName = "person.LName" });
            //    grdemployee.Columns.GetColumnByFieldName("person.LName").Header = "Last Name";
            //}

            //}
            editemp = 0;
        }

        private void btnDeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.currentUserid == 0)
            {

                if (grdemployee.GetFocusedRowCellValue(grdemployee.Columns.GetColumnByFieldName("EmpId")) != null)
                {

                    empid = (int)grdemployee.GetFocusedRowCellValue(grdemployee.Columns.GetColumnByFieldName("EmpId"));

                    //MessageBox.Show(empid.ToString());
                }
                EmployeeRepo employeeRepo = new EmployeeRepo();
                employeeRepo.DeleteEmployee(empid);
            }
        }

        private void mbtneditemplyee_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            editEmployeeclick();
        }
        // List<Department> employeesDepts { get; set; }
        private void gridCompany_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //var comp = gridCompany.SelectedItem as ERP_BL.Databases.Company;

            //foreach (Department dept in emp.departments)
            //    foreach (Department depts in comp.departments)
            //    {
            //        int i = 0;
            //        if (dept.Id != depts.Id)
            //        {
            //            //employeesDepts.Add(depts);
            //            detailGridDescriptor.ItemsSourcePath.Remove(i);
            //        }
            //        i++;
            //    }
            //Binding path = new Binding("employeesDepts");
            //detailGridDescriptor.ItemsSourcePath= path;

        }

        private void Winemployeemain_Unloaded(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdemployee);
        }


        private void RegistersTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            registersGrid.Visibility = Visibility.Visible;
        }

        private void EmplyeesTab_MouseUp(object sender, MouseButtonEventArgs e)
        {
            registersGrid.Visibility = Visibility.Collapsed;
        }

        private void EmployeeRecordTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            EmployeeRepo repo = new EmployeeRepo();

            var selectedItem = (TreeViewItem)employeeRecordTree.SelectedItem;

            if (selectedItem.Name == "empRegister")
            {
                if (registersGrid != null)
                    registersGrid.Children.Clear();
                ucEmployeeInfo uc = new ucEmployeeInfo();
                var emp = repo.GetAllEmployeesForRegister();

                uc.grdEmployeeRegisters.ItemsSource = emp;


                uc.grdEmployeeRegisters.Columns["person"].Visible = false;
                uc.grdEmployeeRegisters.Columns["address"].Visible = false;
                uc.grdEmployeeRegisters.Columns["contact"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Desig"].Visible = false;
                uc.grdEmployeeRegisters.Columns["EmpId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["empFunction"].Visible = false;
                uc.grdEmployeeRegisters.Columns["address2"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SalesTargetId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["DesignationTitle"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SupervisorId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SalesTarget"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Supervisor"].Visible = false;



                // registersGrid.Visibility = Visibility;
                registersGrid.Children.Add(uc);
            }
            if (selectedItem.Name == "personalInfo")
            {
                registersGrid.Children.Clear();
                ucPersonalInfoGrid uc = new ucPersonalInfoGrid();
                var emp = repo.GetAllEmployeesForRegister();

                uc.grdEmployeeRegisters.ItemsSource = emp;
                uc.grdEmployeeRegisters.Columns["person"].Visible = false;
                uc.grdEmployeeRegisters.Columns["address"].Visible = false;
                uc.grdEmployeeRegisters.Columns["contact"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Desig"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Status"].Visible = false;
                uc.grdEmployeeRegisters.Columns["JoinDate"].Visible = false;
                uc.grdEmployeeRegisters.Columns["HireDate"].Visible = false;
                uc.grdEmployeeRegisters.Columns["BasicPay"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SupervisorId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Supervisor"].Visible = false;
                uc.grdEmployeeRegisters.Columns["DesignationTitle"].Visible = false;
                uc.grdEmployeeRegisters.Columns["JobDescription"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SalesTargetId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SalesTarget"].Visible = false;
                uc.grdEmployeeRegisters.Columns["EmpId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["address2"].Visible = false;
                //uc.grdEmployeeRegisters.Columns["Function"].Visible = false;
                uc.grdEmployeeRegisters.Columns["empFunction"].Visible = false;


                registersGrid.Children.Add(uc);

            }
            if (selectedItem.Name == "address")
            {
                registersGrid.Children.Clear();
                ucAddressInfo uc = new ucAddressInfo();
                var emp = repo.GetAllEmployeesForRegister();

                uc.grdEmployeeRegisters.ItemsSource = emp;

                uc.grdEmployeeRegisters.Columns["EmpId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["person"].Visible = false;
                uc.grdEmployeeRegisters.Columns["address"].Visible = false;
                uc.grdEmployeeRegisters.Columns["contact"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Desig"].Visible = false;
                uc.grdEmployeeRegisters.Columns["MaritalStatus"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Disability"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Status"].Visible = false;
                uc.grdEmployeeRegisters.Columns["HireDate"].Visible = false;
                uc.grdEmployeeRegisters.Columns["BasicPay"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SupervisorId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["Supervisor"].Visible = false;
                uc.grdEmployeeRegisters.Columns["isActive"].Visible = false;
                uc.grdEmployeeRegisters.Columns["DesignationTitle"].Visible = false;
                uc.grdEmployeeRegisters.Columns["JoinDate"].Visible = false;
                uc.grdEmployeeRegisters.Columns["DisDescription"].Visible = false;
                uc.grdEmployeeRegisters.Columns["JobDescription"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SalesTargetId"].Visible = false;
                uc.grdEmployeeRegisters.Columns["SalesTarget"].Visible = false;
                uc.grdEmployeeRegisters.Columns["address2"].Visible = false;
                uc.grdEmployeeRegisters.Columns["PassportNo"].Visible = false;
                uc.grdEmployeeRegisters.Columns["BloodGroup"].Visible = false;
                uc.grdEmployeeRegisters.Columns["empFunction"].Visible = false;
                uc.grdEmployeeRegisters.Columns["employeeApproval"].Visible = false;



                registersGrid.Children.Add(uc);


            }
            if (selectedItem.Name == "contact")
            {

            }
            if (selectedItem.Name == "employmentInfo")
            {

            }
            if (selectedItem.Name == "depts")
            {

            }
            if (selectedItem.Name == "sales")
            {

            }
        }

        private void GrdEmployeeRegisters_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            //if (e.Column.FieldName == "person.FName")
            //{
            //    if (e.GetListSourceFieldValue("person.FName") != null)
            //    {
            //        var field = (Employee)e.GetListSourceFieldValue("person.FName");
            //        //DateTime date;

            //        e.Value = field;
            //    }
            //}
        }

        private void SaveGriddata_Click(object sender, RoutedEventArgs e)
        {

            //EmployeeRepo repo = new EmployeeRepo();
            //var gridItems = grdEmployeeRegisters.VisibleItems;
            //foreach (var _item in gridItems)
            //{
            //ERP_BL.Databases.Employee emp = new ERP_BL.Databases.Employee();
            //    emp = (ERP_BL.Databases.Employee)_item;
            //    var newEmp = repo.GetEmployee(emp.EmpId);
            //    if (newEmp == null)
            //    { return; }
            //    newEmp.person.FName = emp.person.FName;
            //    newEmp.person.LName = emp.person.LName;
            //    newEmp.person.FatherName = emp.person.FatherName;
            //    newEmp.person.CNIC = emp.person.CNIC;
            //    newEmp.person.Gender = emp.person.Gender;
            //    newEmp.person.DOB = emp.person.DOB;
            //    newEmp.MaritalStatus = emp.MaritalStatus;
            //    newEmp.Disability = emp.Disability;
            //    newEmp.DisDescription = emp.DisDescription;
            //    newEmp.isActive = emp.isActive;


            //    repo.updateEmployee(newEmp);
            //}

        }

        private void BtnExpandGrid_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            leftGrid.Visibility = Visibility.Collapsed;
            GridSplitter.Visibility = Visibility.Collapsed;
            rightGrid.SetValue(Grid.ColumnProperty, 0);

            rightGrid.SetValue(Grid.ColumnSpanProperty, 3);

        }

        private void BtnCollapsedGrid_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            leftGrid.Visibility = Visibility.Visible;
            GridSplitter.Visibility = Visibility.Visible;

            rightGrid.SetValue(Grid.ColumnProperty, 2);
            rightGrid.SetValue(Grid.ColumnSpanProperty, 1);
        }


        private void EmpTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ucEmployeeInfo uc = new ucEmployeeInfo();

            EmployeeRepo empRepo = new EmployeeRepo();
            var item = (cmbitem)empTreeView.SelectedItem;


            if (item.description == "Employees")
            {
                if (item.name == "Employees")
                {
                    itemSourceEmp = empRepo.GetAllEmployeesForRegister();

                    if (itemSourceEmp == null)
                    {
                        return;
                    }

                    rightGrid.Children.Clear();
                    uc = new ucEmployeeInfo();
                    uc.lblfaRegister.Text = item.name;

                    uc.grdEmployeeRegisters.ItemsSource = itemSourceEmp;
                    uc.LoadCount();
                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(uc.grdEmployeeRegisters);
                    rightGrid.Children.Add(uc);

                }


                else if (item.name == "Employees(Open)")
                {

                    itemSourceEmp = empRepo.GetAllActiveEmployees();

                    List<ERP_BL.Databases.Employee> itemSourceOpenEmp = new List<ERP_BL.Databases.Employee>();

                    rightGrid.Children.Clear();
                    uc = new ucEmployeeInfo();
                    uc.lblfaRegister.Text = item.name;
                    uc.LoadCount();

                    //var openEmp = itemSourceOpenEmp;
                    //foreach (var _item in openEmp)
                    //{
                    //    if (_item.employeeStatus != null)
                    //    {
                    //        if (_item.AssetStatus.isActive == true)
                    //        {
                    //            itemSourceOpenEmp.Add(_item);

                    //        }
                    //    }
                    //}
                    uc.grdEmployeeRegisters.ItemsSource = itemSourceEmp;
                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(uc.grdEmployeeRegisters);
                    rightGrid.Children.Add(uc);


                }

                else if (item.name == "Employees(Closed)")
                {
                    itemSourceEmp = empRepo.GetAllInactiveEmployees();

                    List<ERP_BL.Databases.Employee> itemSourceOpenEmp = new List<ERP_BL.Databases.Employee>();

                    rightGrid.Children.Clear();
                    uc = new ucEmployeeInfo();
                    uc.lblfaRegister.Text = item.name;

                    //var openEmp = itemSourceOpenEmp;
                    //foreach (var _item in openEmp)
                    //{
                    //    if (_item.employeeStatus != null)
                    //    {
                    //        if (_item.AssetStatus.isActive == true)
                    //        {
                    //            itemSourceOpenEmp.Add(_item);

                    //        }
                    //    }
                    //}
                    uc.grdEmployeeRegisters.ItemsSource = itemSourceEmp;
                    uc.LoadCount();

                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(uc.grdEmployeeRegisters);
                    rightGrid.Children.Add(uc);

                }

                else
                {
                    itemSourceEmp = empRepo.GetAllEmployeesForRegister();

                    List<ERP_BL.Databases.Employee> itemSource = new List<ERP_BL.Databases.Employee>();

                    rightGrid.Children.Clear();
                    uc = new ucEmployeeInfo();
                    uc.lblfaRegister.Text = item.name;

                    var openFa = itemSourceEmp;
                    foreach (var _item in openFa)
                    {
                        if (_item.employeeStatus != null)
                        {
                            if (_item.employeeStatus.Status == item.name)
                            {
                                itemSource.Add(_item);

                            }
                        }
                    }
                    uc.grdEmployeeRegisters.ItemsSource = itemSource;
                    uc.LoadCount();

                    SYSTEM_STATIC.SetUserSettingOfCurrentWindow(uc.grdEmployeeRegisters);
                    rightGrid.Children.Add(uc);


                }
                try
                {
                    uc.grdEmployeeRegisters.Columns["EmpId"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["person"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["address"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["contact"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["Desig"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["MaritalStatus"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["Disability"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["Status"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["HireDate"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["BasicPay"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["SupervisorId"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["Supervisor"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["isActive"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["DesignationTitle"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["JoinDate"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["DisDescription"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["JobDescription"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["SalesTargetId"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["SalesTarget"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["address2"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["PassportNo"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["BloodGroup"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["empFunction"].Visible = false;
                    uc.grdEmployeeRegisters.Columns["employeeApproval"].Visible = false;
                }
                catch
                {
                }

            }
        }
        public void refreshGrid()
        {

            ucEmployeeInfo uc = new ucEmployeeInfo();
            itemSourceEmp = employeeRepo.GetAllActiveEmployees();

            List<ERP_BL.Databases.Employee> itemSourceOpenEmp = new List<ERP_BL.Databases.Employee>();

            rightGrid.Children.Clear();
            uc = new ucEmployeeInfo();
            uc.lblfaRegister.Text = "Employees(Open)";

            uc.grdEmployeeRegisters.ItemsSource = itemSourceEmp;
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(uc.grdEmployeeRegisters);
            rightGrid.Children.Add(uc);

        }

        private void Grdemployee_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {

        }

        private void DXTabControl_SelectionChanged(object sender, DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs e)
        {
            var selecetdTab = empTabControl.SelectedItem as DXTabItem;
            if (selecetdTab.Name == "registersTab")
            {
                if (isFirstRegTab == true)
                {
                    loadingGif.Visibility = Visibility.Visible;
                    GenerateTreeView();

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += LoadEmployeesReg;
                    worker.RunWorkerCompleted += LoademployeesRegCompleted;
                    worker.RunWorkerAsync();
                 //   AssignItemSource();
                 //   isFirstRegTab = false;
                 ////   loadingGif.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void LoadEmployeesReg(object o, DoWorkEventArgs args)
        {
            Task.Delay(1000).Wait();  // Pretend to work

            
        }

        private void LoademployeesRegCompleted(object o, RunWorkerCompletedEventArgs args)
        {
            AssignItemSource();
            isFirstRegTab = false;
            loadingGif.Visibility = Visibility.Hidden;

        }
    }
}
