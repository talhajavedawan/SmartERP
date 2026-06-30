using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
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

namespace ZAS_ERP.Userss
{
    /// <summary>
    /// Interaction logic for frmUsersRoles.xamllogin
    /// </summary>
    public partial class frmUsersRoles : DXWindow
    {
        List<User> users = new List<User>();
        List<Role> roles = new List<Role>();
        EmployeeRepo employeeRepo = new EmployeeRepo();
        UsersRepo rolesRepo = new UsersRepo();
        List<Department> departments = new List<Department>();
        List<Company> companies = new List<Company>();
        Role selectedRole = new Role();
        public frmUsersRoles()
        {
            //if (MainWindow.currentUserid == 0)
                InitializeComponent();
           
            SystemLog.LogInfo(this.GetType(), "Form Intialized");
            NLog.Logger logge = NLog.LogManager.GetCurrentClassLogger();
            logge.Info("Hello {0}", "Earth");

        }

        private void BtnRoleNew_Click(object sender, RoutedEventArgs e)
        {
            frmNewRole frmNewRole = new frmNewRole();
            frmNewRole.ShowDialog();
            loadRoles();
        }

        private void BtnRoleDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRoleEdit_Click(object sender, RoutedEventArgs e)
        {
           
            if (selectedRole.Id != 0)
            {
                rolesRepo = new UsersRepo();
                frmNewRole frmNewRole = new frmNewRole();

                frmNewRole.roleId = selectedRole.Id;
                frmNewRole.ShowDialog();

                loadRoles();

                // var rol = rolesRepo.get(selectedRow.Id);


            }

            //frmNewRole frmNewRole = new frmNewRole();
            //if(lstRoles.SelectedItem!=null)
            //frmNewRole.roleId = (lstRoles.SelectedItem as cmbitem).id;

            //frmNewRole.ShowDialog();
            //loadRoles();
        }

        private void BtnRoleViewPermissions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUserViewPermissions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnUserNew_Click(object sender, RoutedEventArgs e)
        {
            frmNewUser frmNewUser = new frmNewUser();
            frmNewUser.ShowDialog();
            loadUsers();
        }

        private void BtnUserEdit_Click(object sender, RoutedEventArgs e)
        {
            frmNewUser frmNewUser = new frmNewUser();
            if(grdUsers.SelectedItem!=null)
            frmNewUser.UserId= (grdUsers.SelectedItem as User).id;
            frmNewUser.ShowDialog();
            loadUsers();
        }

        private void BtnUserDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void LstUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (grdUsers.SelectedItem != null)
        //    {
        //        User user = users.Find(x => x.id == (lstUsers.SelectedItem as cmbitem).id);
        //        List<cmbitem> cmbitems = new List<cmbitem>();
        //        if (user.Roles != null)
        //            foreach (Role rolee in user.Roles)
        //            {

        //                cmbitems.Add(new cmbitem() { name = rolee.Name, id = rolee.Id, description = rolee.Description });

        //            }


        //        lstAssignedRoles.ItemsSource = cmbitems;

        //    }
        //}
        public void LoadColumn()
        {

            var selectedHeader = rolesRepo.getAllRoleField();

            if(selectedHeader.LastOrDefault(x => x.FieldName == "Company")!= null)
            grdRoles.Columns["Company"].Header = selectedHeader.LastOrDefault(x => x.FieldName == "Company").Header;

            if(selectedHeader.LastOrDefault(x => x.FieldName == "Department")!= null)
            grdRoles.Columns["Department"].Header = selectedHeader.LastOrDefault(x => x.FieldName == "Department").Header;

            if(selectedHeader.LastOrDefault(x => x.FieldName == "Name")!=null)
            grdRoles.Columns["Name"].Header = selectedHeader.LastOrDefault(x => x.FieldName == "Name").Header;

            if(selectedHeader.LastOrDefault(x => x.FieldName == "Description")!= null)
            grdRoles.Columns["Description"].Header = selectedHeader.LastOrDefault(x => x.FieldName == "Description").Header;

            if(selectedHeader.LastOrDefault(x => x.FieldName == "RoleName")!= null)
            grdRoles.Columns["RoleName"].Header = selectedHeader.LastOrDefault(x => x.FieldName == "RoleName").Header;
        }
        public void loadRoles()
        {

            rolesRepo = new UsersRepo();
            var rolesss = rolesRepo.getAll();
            var inActiveRoles = rolesRepo.getAllinActive();
            grdinActiveRoles.ItemsSource = inActiveRoles;
            grdRoles.ItemsSource = rolesss;
        }
        public void loadUsers()
        {
            users = rolesRepo.getAllusers();
            grdUsers.DataContext = users;
            grdUsers.ItemsSource = users;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Manage Users And Roles") != null || MainWindow.currentUserid == 0)
            {
                SystemLog.LogInfo(this.GetType(), "Window Loaded");
                loadRoles();
                loadUsers();
                loadUserMachines();
                loadRDCMachines();
        
                LoadColumn();
            }
            else
            {
                MessageBox.Show("You are not allowed to view this form");
                this.Close();
    }

}

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            SystemLog.LogInfo(this.GetType(), "Window UnLoaded");
        }

      

        private void GrdUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            frmNewUser frmNewUser = new frmNewUser();
            if (grdUsers.SelectedItem != null)
                frmNewUser.UserId = (grdUsers.SelectedItem as User).id;
            frmNewUser.ShowDialog();
            loadUsers();
        }

        private void GrdUsers_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            User usera = grdUsers.GetRow(e.ListSourceRowIndex) as User;/*users.Find(x => x.id == (grdUsers.SelectedItem as User).id);*/
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {
                if (e.GetListSourceFieldValue("employee.person.FName") != null)
                {


                    string fname = /*usera.employee.person.FName;*/ Convert.ToString(e.GetListSourceFieldValue("employee.person.FName"));
                    string lname = /*usera.employee.person.LName;//*/ Convert.ToString(e.GetListSourceFieldValue("employee.person.LName"));

                    //DateTime date;
                    
                    e.Value = fname+" "+lname;
                }
            }

            if (e.Column.FieldName == "Departments")

            {
                User user1 = grdUsers.GetRow(e.ListSourceRowIndex) as User;
                if(user1!=null)
                if (user1.employee.departments != null)
                {
                    departments = user1.employee.departments; //*/e.GetListSourceFieldValue("employee.departments") as List<Department>;
                    string depts = "";
                    if(departments!=null)
                    foreach (Department department in departments)
                    {
                        if (department.parentDepartment != null)
                        {
                            depts += department.parentDepartment.DeptName + " " + department.DeptName + " , ";
                        }
                        else
                            depts += department.DeptName + " , ";
                    }
                    e.Value = depts;
                }
            }
            if (e.Column.FieldName == "SupervisorUserName")

            {
                // if (e.GetListSourceFieldValue("employee.Supervisor.user.userName") != null)
                {
                    //string fname = usera.employee.Supervisor.user.userName;//Convert.ToString(e.GetListSourceFieldValue("usera.employee.Supervisor.user.userName"));
                    //string lname = Convert.ToString(e.GetListSourceFieldValue("userr.employee.person.LName"));

                    //DateTime date;

                    // e.Value = fname ;
                    // string s = "Test: FieldTwo";
                }
            }
            if (e.Column.FieldName == "SupervisorName")

            {
                if (e.GetListSourceFieldValue(e.ListSourceRowIndex, "employee.Supervisor.person.FName") != null)
                {
                    string fname = /*usera.employee.Supervisor.person.FName;// Convert.ToString(e.GetListSourceFieldValue(e.ListSourceRowIndex, "employee.Supervisor.person.FName"));
                    string lname = /*usera.employee.Supervisor.person.LName;//*/ Convert.ToString(e.GetListSourceFieldValue(e.ListSourceRowIndex, "employee.Supervisor.person.LName"));

                    

                    e.Value = fname;
                }
            }
            if (e.Column.FieldName == "Companies")

            {
                if (usera != null)
                {
                    var usea = grdUsers.GetRow(e.ListSourceRowIndex) as User;// users.Find(x => x.id == (grdUsers.SelectedItem as User).id);
                    companies = usea.employee.Companies;// e.GetListSourceFieldValue("employee.Companies") as List<Company>;
                    string comps = "";
                    foreach (Company company in companies)
                    {
                        comps += company.CompanyName + " , ";
                    }
                    e.Value = comps;
                }
            }
        }

        private void GrdUsers_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            if (grdUsers.SelectedItem != null)
            {
                User user = users.Find(x => x.id == (grdUsers.SelectedItem as User).id);
                if (user.Roles != null)
                {
                    //var v = rolesRepo.getUserRoles(user.id);
                    lstAssignedRoles.ItemsSource = user.Roles;
                    //lstAssignedRoles.ItemsSource = v;
                }
            }
        }

    
        private void BtnCopyRole_Click(object sender, RoutedEventArgs e)
        {
            frmNewRole frmNewRole = new frmNewRole();
            frmNewRole.roleCopy = true;
            if (grdRoles.SelectedItem != null)
                frmNewRole.roleId = (grdRoles.SelectedItem as Role).Id;

            frmNewRole.ShowDialog();
            loadRoles();
        }
        //User Machines Tab Item

        private void GrdMachines_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {

        }

        private void GrdMachines_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        public void loadUserMachines()
        {
            
            users = rolesRepo.getAllusers();
            grdMachines.DataContext = users;
            grdMachines.ItemsSource = users;

          
        }
        public void loadRDCMachines()
        {

            users = rolesRepo.getAllusers();
            grdRDCMachines.DataContext = users;
            grdRDCMachines.ItemsSource = users;


        }

        private void GrdMachines_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            //User user = grdMachines.GetRow(e.ListSourceRowIndex) as User;/*users.Find(x => x.id == (grdUsers.SelectedItem as User).id);*/
            //if (e.Column.FieldName == "Name" && e.IsGetData)

            //{
            //    if (e.GetListSourceFieldValue("employee.person.FName") != null)
            //    {


            //        string fname = /*usera.employee.person.FName;*/ Convert.ToString(e.GetListSourceFieldValue("employee.person.FName"));
            //        string lname = /*usera.employee.person.LName;//*/ Convert.ToString(e.GetListSourceFieldValue("employee.person.LName"));

            //        //DateTime date;

            //        e.Value = fname + " " + lname;
            //    }
            //}

            //if (e.Column.FieldName == "Departments")

            //{
            ////    User user1 = grdUsers.GetRow(e.ListSourceRowIndex) as User;
            ////    if (user1.employee.departments != null)
            ////    {
            ////        departments = user1.employee.departments; //*/e.GetListSourceFieldValue("employee.departments") as List<Department>;
            ////        string depts = "";
            ////        if (departments != null)
            ////            foreach (Department department in departments)
            ////            {
            ////                if (department.parentDepartment != null)
            ////                {
            ////                    depts += department.parentDepartment.DeptName + " " + department.DeptName + " , ";
            ////                }
            ////                else
            ////                    depts += department.DeptName + " , ";
            ////            }
            ////        e.Value = depts;
            ////    }
            //}
            //if (e.Column.FieldName == "SupervisorUserName")

            //{
            //    // if (e.GetListSourceFieldValue("employee.Supervisor.user.userName") != null)
            //    {
            //        //string fname = usera.employee.Supervisor.user.userName;//Convert.ToString(e.GetListSourceFieldValue("usera.employee.Supervisor.user.userName"));
            //        //string lname = Convert.ToString(e.GetListSourceFieldValue("userr.employee.person.LName"));

            //        //DateTime date;

            //        // e.Value = fname ;
            //        // string s = "Test: FieldTwo";
            //    }
            //}
            //if (e.Column.FieldName == "SupervisorName")

            //{
            //    if (e.GetListSourceFieldValue(e.ListSourceRowIndex, "employee.Supervisor.person.FName") != null)
            //    {
            //        string fname = /*usera.employee.Supervisor.person.FName;// Convert.ToString(e.GetListSourceFieldValue(e.ListSourceRowIndex, "employee.Supervisor.person.FName"));
            //        string lname = /*usera.employee.Supervisor.person.LName;//*/ Convert.ToString(e.GetListSourceFieldValue(e.ListSourceRowIndex, "employee.Supervisor.person.LName"));



            //        e.Value = fname;
            //    }
            //}
            //if (e.Column.FieldName == "Companies")

            //{
            //    //if (user != null)
            //    //{
            //    //    var usea = grdUsers.GetRow(e.ListSourceRowIndex) as User;// users.Find(x => x.id == (grdUsers.SelectedItem as User).id);
            //    //    companies = usea.employee.Companies;// e.GetListSourceFieldValue("employee.Companies") as List<Company>;
            //    //    string comps = "";
            //    //    foreach (Company company in companies)
            //    //    {
            //    //        comps += company.CompanyName + " , ";
            //    //    }
            //    //    e.Value = comps;
            //    //}
            //}
            //if (e.Column.FieldName == "Machine Identity")

            //{
            //    if (user != null)
            //    {
            //        var _user = grdUsers.GetRow(e.ListSourceRowIndex) as User;// users.Find(x => x.id == (grdUsers.SelectedItem as User).id);
            //        var machineKey = _user.machineKey;// e.GetListSourceFieldValue("employee.Companies") as List<Company>;
                
            //        e.Value = machineKey;
            //        e.Column.Width = 400;
            //    }
            //}

            //if (e.Column.FieldName == "Approve")

            //{

            //    //var a = grdMachines.GetCellValue(1, e.Column);

            //    var h  = grdMachines.GetRow(e.ListSourceRowIndex) as User;/*users.Find(x => x.id == (grdUsers.SelectedItem as User).id);*/


            //    //if (e.GetListSourceFieldValue("Machine Identity") == null)
            //    //{
                    
            //    //    var template = (Button)e.Column.CellTemplate.DataType;
            //    //    //template.IsEnabled = false;
            //    //}
              
            //    //var bt = template.Template;
            //    //var a = (Button)(((DataTemplate)this.Resources["declineTemp"]).DataTemplateKey);
            //    //a.IsEnabled
            //    // var x = ((Resources["declineTemp"])n.CellTemplate;
            //    //var bt = (Button)e.Column.CellTemplate;
            //    // var gf = ((EditGridCellData)bt.DataContext).RowData.Row;
            //    // colDecline.CellTemplate = (DataTemplate)this.Resources["declineTemp"];


            //    // var a =  (Button)e.Column.DataContext;
            //    //var b =  e.Column.CellTemplate;

            //    // var c = b.Template;


            //    // a.IsEnabled = false;
            //    //e.Column.CellTemplate = (DataTemplate)this.Resources["ApproveTemp"];
            //    //e.Column.Width = 70;
            //    //e.Column.Name = "Approve";

            //    if (user != null)
            //    {
            //        //Button bt = (Button)sender;
            //        //var machineKey = ((User)(((EditGridCellData)bt.DataContext).RowData.Row)).machineKey;
            //        //if (machineKey == null)
            //        //{
                        
            //        //}
            //        //e.Value = isApproved;
            //    }
            //}

        }
        private void btnApproveMachine_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            var user = (User)((EditGridCellData)bt.DataContext).RowData.Row;
            var machineKey = ((User)(((EditGridCellData)bt.DataContext).RowData.Row)).machineKey;
            if (machineKey != null)
            {
                user.isKeyApproved = true;
                rolesRepo.updateuser(user);
                DXMessageBox.Show("User key is Approved for " + user.userName + " ", "Key approved",MessageBoxButton.OK, MessageBoxImage.Exclamation );
            }
            else
            {
                DXMessageBox.Show("User key is not found for " + user.userName + " ", "Key Unapproved", MessageBoxButton.OK, MessageBoxImage.Stop);

            }

          // loadUserMachines();


        }
        private void btnDeclineMachine_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            var user = (User)((EditGridCellData)bt.DataContext).RowData.Row;
            var machineKey = ((User)(((EditGridCellData)bt.DataContext).RowData.Row)).machineKey;

            var choice =  DXMessageBox.Show("Do you want to Clear the machine key for user "+user.userName+" ?","Confirmation",MessageBoxButton.YesNoCancel, MessageBoxImage.Warning );
            switch (choice)
            {
                case MessageBoxResult.Yes :
                    if (user != null)
                    {
                        user.machineKey = null;
                        user.isKeyApproved = false;
                        rolesRepo.updateuser(user);
                        DXMessageBox.Show("Machine Key is cleared for " + user.userName + " ?", "Machine Key Cleared", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                 break;
                case MessageBoxResult.No:
                    break;
                   
            }
          //  loadUserMachines();
        }
        private void btnBlockMachine_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            var user = (User)((EditGridCellData)bt.DataContext).RowData.Row;

            var choice = DXMessageBox.Show("Do you want to Block user " + user.userName + " ?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            switch (choice)
            {
                case MessageBoxResult.Yes:
                    if (user != null)
                    {
                        //user.machineKey = null;
                        user.isKeyApproved = false;
                        user.isActive = false;
                        rolesRepo.updateuser(user);
                        DXMessageBox.Show("User " + user.userName + " is blocked", "User Blocked", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
            //foreach (var column in grdMachines.Columns)
            //{
            //    if (column.Name == "Approve")
            //    {
                    
            //        grdMachines.Columns[column.Name]
            //    }
            //    if (column.Name == "Clear")
            //    {

            //    }
            //    if (column.Name == "Block")
            //    {

            //    }
            //}
           // loadUserMachines();

        }
        private void GrdMachines_ColumnsPopulated(object sender, RoutedEventArgs e)
        {
            //GridColumn colApprove = new GridColumn()
            //{
            //    CellTemplate = (DataTemplate)this.Resources["ApproveTemp"],
            //    Width = 70,
            //    Header = "Approve",
            //    Name = "Approve"


            //};

            var colApprove = grdMachines.Columns["Approve"];
            colApprove.CellTemplate = (DataTemplate)this.Resources["ApproveTemp"];
            colApprove.Width = 70;
            colApprove.Name = "Approve";

            //GridColumn colDecline = new GridColumn()
            //{
            //    CellTemplate = (DataTemplate)this.Resources["declineTemp"],
            //    Width = 70,
            //    Header = "Clear",
            //    Name = "Clear"
            //};
            var colDecline = grdMachines.Columns["Clear"];
            colDecline.CellTemplate = (DataTemplate)this.Resources["declineTemp"];
            colDecline.Width = 70;
            colDecline.Name = "Clear";
          //  ((DataTemplate)this.Resources["declineTemp"]).

            //GridColumn colDelete = new GridColumn()
            //{
            //    CellTemplate = (DataTemplate)this.Resources["blockTemp"],
            //    Width = 70,
            //    Header = "Block",
            //    Name="Block"
            //};

            var colDelete = grdMachines.Columns["Block"];
            colDelete.CellTemplate = (DataTemplate)this.Resources["blockTemp"];
            colDelete.Width = 70;
            colDelete.Name = "Block";

            //grdMachines.Columns.Add(colApprove);
            //grdMachines.Columns.Add(colDecline);
            //grdMachines.Columns.Add(colDelete);


        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            loadUserMachines();
         
        }

        private void GrdRoles_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (grdRoles.SelectedItem != null)
            //if (grdUsers.SelectedItem != null)
            {

                selectedRole = (grdRoles.SelectedItem as Role);        
                if(selectedRole.Id != 0)
                {
                    txtRoleDescription.Text = selectedRole.Description;
                    List<cmbitem> cmbitems = new List<cmbitem>();
                    if (selectedRole.Users != null)
                        foreach (var user in selectedRole.Users)
                        {

                            cmbitems.Add(new cmbitem() { name = $"{user.userName} ( {user.employee.person.FName} {user.employee.person.LName})", id = user.id });

                        }


                    lstAssignedUsers.ItemsSource = cmbitems;
                }




            }

        }

        private void GrdRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //frmNewRole frmNewRole = new frmNewRole();
            //if (grdRoles.SelectedItem != null)
            //    frmNewRole.roleId = (grdRoles.SelectedItem as cmbitem).id;

            //frmNewRole.ShowDialog();
            //loadRoles();

        }

     
        private void TreeViews_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
        {
            var columnHeader = e.Column;

        }

        private void BtnAddField_Click(object sender, RoutedEventArgs e)
        {
            //frmAddnewRoles addnewRoles = new frmAddnewRoles();
            //addnewRoles.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //addnewRoles.ResizeMode = ResizeMode.CanMinimize;
            //addnewRoles.ShowDialog();

            LoadColumn();

        }

        private void grdRDCMachines_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {

        }

        private void grdRDCMachines_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e)
        {
            User user = grdRDCMachines.GetRow(e.ListSourceRowIndex) as User;
            if (e.Column.FieldName == "Name" && e.IsGetData)

            {
                if (e.GetListSourceFieldValue("employee.person.FName") != null)
                {


                    string fname =  Convert.ToString(e.GetListSourceFieldValue("employee.person.FName"));
                    string lname =  Convert.ToString(e.GetListSourceFieldValue("employee.person.LName"));

                    //DateTime date;

                    e.Value = fname + " " + lname;
                }
            }

            if (e.Column.FieldName == "Departments")

            {

            }
            if (e.Column.FieldName == "SupervisorUserName")

            {
              
            }
            if (e.Column.FieldName == "SupervisorName")

            {
                if (e.GetListSourceFieldValue(e.ListSourceRowIndex, "employee.Supervisor.person.FName") != null)
                {
                    string fname = Convert.ToString(e.GetListSourceFieldValue(e.ListSourceRowIndex, "employee.Supervisor.person.LName"));
                    e.Value = fname;
                }
            }
            if (e.Column.FieldName == "Companies")

            {

            }
            if (e.Column.FieldName == "Machine Identity")

            {
                if (user != null)
                {
                    var _user = grdUsers.GetRow(e.ListSourceRowIndex) as User;
                    var machineKey = _user.rdcMachineKey;
                    e.Value = machineKey;
                    e.Column.Width = 400;
                }
            }

            if (e.Column.FieldName == "Approve")
            {

                var h = grdMachines.GetRow(e.ListSourceRowIndex) as User;


                if (user != null)
                {
  
                }
            }
        }

        private void grdRDCMachines_ColumnsPopulated(object sender, RoutedEventArgs e)
        {
            var colApprove = grdRDCMachines.Columns["Approve"];
            colApprove.CellTemplate = (DataTemplate)this.Resources["ApproveRDCTemp"];
            colApprove.Width = 70;
            colApprove.Name = "Approve";


            var colDecline = grdRDCMachines.Columns["Clear"];
            colDecline.CellTemplate = (DataTemplate)this.Resources["declineRDCTemp"];
            colDecline.Width = 70;
            colDecline.Name = "Clear";


            var colDelete = grdRDCMachines.Columns["Block"];
            colDelete.CellTemplate = (DataTemplate)this.Resources["blockRDCTemp"];
            colDelete.Width = 70;
            colDelete.Name = "Block";
        }

        private void grdRDCMachines_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void mbtnRDCRefresh_Click(object sender, RoutedEventArgs e)
        {
            loadRDCMachines();
        }

        private void btnApproveRDCMachine_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            var user = (User)((EditGridCellData)bt.DataContext).RowData.Row;
            var machineKey = ((User)(((EditGridCellData)bt.DataContext).RowData.Row)).rdcMachineKey;
            if (machineKey != null)
            {
                user.isRDCKeyApproved = true;
                rolesRepo.updateuser(user);
                DXMessageBox.Show("User RDC key is Approved for " + user.userName + " ", "RDC Key approved", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                DXMessageBox.Show("User RDC key is not found for " + user.userName + " ", "RDC Key Unapproved", MessageBoxButton.OK, MessageBoxImage.Stop);

            }
        }

        private void btnDeclineRDCMachine_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            var user = (User)((EditGridCellData)bt.DataContext).RowData.Row;
            var machineKey = ((User)(((EditGridCellData)bt.DataContext).RowData.Row)).rdcMachineKey;

            var choice = DXMessageBox.Show("Do you want to Clear the machine key for user " + user.userName + " ?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            switch (choice)
            {
                case MessageBoxResult.Yes:
                    if (user != null)
                    {
                        user.rdcMachineKey = null;
                        user.isRDCKeyApproved = false;
                        rolesRepo.updateuser(user);
                        DXMessageBox.Show("Machine Key is cleared for " + user.userName + " ?", "Machine Key Cleared", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    break;
                case MessageBoxResult.No:
                    break;

            }
        }

        private void btnBlockRDCMachine_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            var user = (User)((EditGridCellData)bt.DataContext).RowData.Row;

            var choice = DXMessageBox.Show("Do you want to Block user " + user.userName + " ?", "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            switch (choice)
            {
                case MessageBoxResult.Yes:
                    if (user != null)
                    {
                        //user.machineKey = null;
                        user.isRDCKeyApproved = false;
                        user.isActive = false;
                        rolesRepo.updateuser(user);
                        DXMessageBox.Show("User " + user.userName + " is blocked", "User Blocked", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void lstAssignedRoles_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var role = lstAssignedRoles.SelectedItem as Role;
            if (role != null)
            {
                grdPermissions.ItemsSource = null;
                grdPermissions.ItemsSource = role.Permissions;
            }
        }

        private void GrdPermissions_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {

        }

        private void grdinActiveRoles_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void grdinActiveRoles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void treeinActiveViews_ColumnHeaderClick(object sender, ColumnHeaderClickEventArgs e)
        {

        }

        private void btninActiveRoleEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRole.Id != 0)
            {
                rolesRepo = new UsersRepo();
                frmNewRole frmNewRole = new frmNewRole();

                frmNewRole.roleId = (grdinActiveRoles.SelectedItem as Role).Id;
                frmNewRole.ShowDialog();

                loadRoles();

                // var rol = rolesRepo.get(selectedRow.Id);


            }
        }

        private void btninActiveCopyRole_Click(object sender, RoutedEventArgs e)
        {
            frmNewRole frmNewRole = new frmNewRole();
            frmNewRole.roleCopy = true;
            if (grdRoles.SelectedItem != null)
                frmNewRole.roleId = (grdinActiveRoles.SelectedItem as Role).Id;

            frmNewRole.ShowDialog();
            loadRoles();
        }
    }


    public class UserMachine
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string IsActive { get; set; }
        public string MachineIdentity { get; set; }
        public string IsKeyApproved { get; set; }
        public Button Approve { get; set; }
        public Button Clear { get; set; }
        public Button Block { get; set; }

    }
    public class UserRDCMachine
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string IsActive { get; set; }
        public string MachineIdentity { get; set; }
        public string IsKeyApproved { get; set; }
        public Button Approve { get; set; }
        public Button Clear { get; set; }
        public Button Block { get; set; }

    }

}
