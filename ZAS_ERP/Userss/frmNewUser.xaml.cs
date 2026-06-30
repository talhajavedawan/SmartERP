using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ZAS_ERP.Userss
{
    /// <summary>
    /// Interaction logic for frmNewUser.xaml
    /// </summary>
    public partial class frmNewUser : Window
    {
        public static int UserId;
        User user = new User();
        EmployeeRepo usersRepo = new EmployeeRepo();
        //List<Role> availableRoles = new List<Role>();
        //List<Role> assignedRoles = new List<Role>();
        List<Role> AllRoles = new List<Role>();
        UsersRepo rolesRepo = new UsersRepo();
        ERP_BL.Databases.Employee employee = new ERP_BL.Databases.Employee();
        //ObservableCollection<cmbitem> cmbAvailableitems { get; set; }
        //ObservableCollection<cmbitem> cmbAssigneditems { get; set; }
        public frmNewUser()
        {
            InitializeComponent();
            SystemLog.LogInfo(this.GetType(), "Form Intialized");
            //cmbAvailableitems = new ObservableCollection<cmbitem>();
            //cmbAssigneditems = new ObservableCollection<cmbitem>();
        }
        //private void OnNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        //{
        //    if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
        //        grdRoles.SelectItem(e.Node.RowHandle);
        //    else
        //        grdRoles.UnselectItem(e.Node.RowHandle);
        //}

        private void loadRoles()
        {
            AllRoles = rolesRepo.getAll();
            grdRoles.ItemsSource = AllRoles;
            ////if (UserId == 0)
            //{
            //    availableRoles = AllRoles;
            //    //List<cmbitem> cmbitems = new List<cmbitem>();



            //    foreach (Role rolee in availableRoles)
            //    {

            //        cmbAvailableitems.Add(new cmbitem() { name = rolee.Name, id = rolee.Id, description = rolee.Description });

            //    }
            //    //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

            //    lstAvailableRoles.ItemsSource = cmbAvailableitems;
            //    //lstAssignedRoles.Items.Add(new cmbitem() { name = "Customer and Inquiries", id = 0, description = "rolee. Description " });
            //}
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (TxtUserName.Text == "")
                {
                    MessageBox.Show("Enter UserName");
                    return;
                }

                else if (txtPassword.Password != txtConfirmPassword.Password)
                {
                    MessageBox.Show(" Password Mismatch");
                    SystemLog.LogInfo(this.GetType(), "Password Mismatched");
                    return;
                }
                else if (employee == null)
                {
                    MessageBox.Show("Select Employee");
                    return;
                }
                //else if (employee.user != null)
                //{
                //    MessageBox.Show("Select another Employee, This Employee already has a User ID");
                //    return;
                //}

                //if (TxtUserName.Text != "" || txtConfirmPassword.Password != "")
                {

                    if (chkisActive.IsChecked == true)
                        user.isActive = true;
                    else
                        user.isActive = false;
                    if (grdRoles.SelectedItems.Count != 0)
                    {
                        user.Roles = new List<Role>();
                        foreach (Role role in grdRoles.SelectedItems)
                        {

                            if (!user.Roles.Contains(role))
                            {
                                user.Roles.Add(role);
                            }
                        }

                    }
                    else if (grdRoles.SelectedItems.Count == 0)
                    {
                        user.Roles = new List<Role>();
                    }
                    if (user.id == 0)
                    {


                        if (txtConfirmPassword.Password == "")
                        {
                            MessageBox.Show("Enter Password");
                            return;
                        }
                        user.employeeId = employee.EmpId; ;
                        user.password = SYSTEM_STATIC.GenerateSHA512String(txtPassword.Password.Trim());
                        user.userName = TxtUserName.Text.Trim();
                        //user.Roles = assignedRoles;
                        rolesRepo.Adduser(user);
                        //cont.Users.Add(user);
                        //cont.SaveChanges();
                        MessageBox.Show("User Created Succesfully!");
                        SystemLog.LogInfo(this.GetType(), "New User Created ");
                    }
                    else
                    {

                        if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Manage Users And Roles") != null || user.id == MainWindow.currentUserid || MainWindow.currentUserid == 0)
                        {

                            //if(user.password.Length<=15&& user.password !=txtpassword.Text)
                            //{
                            //    MessageBox.Show("Password Mismatch! ReEnter");
                            //    return;
                            //}

                            //if (user.password != SystemLogic.GenerateSHA512String(txtPassword.Password))
                            //{
                            //    MessageBox.Show("Password Mismatch! ReEnter");
                            //    return;

                            //}
                            //SHA512 shaM = new SHA512Managed();
                            user.userName = TxtUserName.Text.Trim();
                            //user.password = shaM.ComputeHash(SystemLogic.ConvertToMemoryStream( txtnewpassword.Text.Trim())).ToString();
                            if (TxtUserName.Text != "" && txtConfirmPassword.Password != "")
                                user.password = SYSTEM_STATIC.GenerateSHA512String(txtPassword.Password);
                            if (lookupEmployee.SelectedIndex != -1)
                            {
                                var employee = lookupEmployee.SelectedItem as ERP_BL.Databases.Employee;
                                user.employeeId = employee.EmpId;
                            }
                            //MessageBox.Show(user.password);
                            //user.password = actualResult512;
                            rolesRepo.updateuser(user);
                            MessageBox.Show("User Updated Succesfully!");
                            SystemLog.LogInfo(this.GetType(), "User= " + user.userName + " Updated!");



                        }
                        else

                        {
                            MessageBox.Show("You are not authorized, to change this Users Password!");
                            return;

                        }

                    }
                    this.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                SystemLog.LogError(this.GetType(), ex.ToString());
            }
        }

        //private void LstAvailableRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (lstAvailableRoles.SelectedItem != null)
        //    {

        //        txtDescription.Text = (lstAvailableRoles.SelectedItem as cmbitem).description;
        //        btnAssign.IsEnabled = true;
        //        btnRemove.IsEnabled = false;
        //    }
        //}

        //private void LstAssignedRoles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (lstAssignedRoles.SelectedItem != null)
        //    {
        //        txtDescription.Text = (lstAssignedRoles.SelectedItem as cmbitem).description;
        //        btnRemove.IsEnabled = true;
        //        btnAssign.IsEnabled = false;
        //    }
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lookupEmployee.ItemsSource = usersRepo.GetActiveEmployeesForNewUser();
            loadRoles();
            if (UserId != 0)
            {
                user = rolesRepo.getuser(UserId);
                TxtUserName.Text = user.userName;

                if (user.employeeId != 0 || user.employee != null)
                {

                    employee = user.employee;
                    var empList = (lookupEmployee.ItemsSource as List<ERP_BL.Databases.Employee>) == null ? new List<ERP_BL.Databases.Employee>() : lookupEmployee.ItemsSource as List<ERP_BL.Databases.Employee>;
                    if (user.employee != null)
                    {
                        int index = 0;
                        foreach (var _emp in empList)
                        {
                            if (_emp.EmpId == user.employeeId)
                            {
                                lookupEmployee.SelectedIndex = index;
                                index = 0;
                                break;
                            }
                            index++;
                        }
                    }
                    //lookupEmployee.Text = user.employee.person.FName;

                }
                chkisActive.IsChecked = user.isActive;
                //List<cmbitem> cmbitemss = new List<cmbitem>();
                if (user.Roles != null)
                {
                    foreach (Role rolee in user.Roles)
                    {

                        grdRoles.SelectItem(grdRoles.FindRowByValue(grdRoles.Columns.GetColumnByFieldName("Id"), rolee.Id));
                    }
                    //assignedRoles = user.Roles;
                }//lstAssignedRoles.ItemsSource = cmbAvailableitems;


                //List<cmbitem> cmbitems = new List<cmbitem>();
                //foreach (Role rolee in availableRoles)
                //{

                //    cmbAvailableitems.Add(new cmbitem() { name = rolee.Name, id = rolee.Id, description = rolee.Description });

                //}
                //cmbitems.Add(new cmbitem() { name = "<-- Add New -->", id = 0 });

                //lstAvailableRoles.ItemsSource = cmbAssigneditems;



            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            UserId = 0;
        }

        //private void BtnAssign_Click(object sender, RoutedEventArgs e)
        //{
        //    Role toRemove = availableRoles.Find(x => x.Id == (lstAvailableRoles.SelectedItem as cmbitem).id);
        //    cmbitem cmbitem = new cmbitem() { name = toRemove.Name, id = toRemove.Id, description = toRemove.Description };
        //    if (assignedRoles != null)
        //        assignedRoles.Add(toRemove);
        //    else
        //    {
        //        assignedRoles = new List<Role>();
        //        assignedRoles.Add(toRemove);
        //    }

        //    ////List<cmbitem> cmbitems = new List<cmbitem>();
        //    ////cmbitems = lstAssignedRoles.ItemsSource as List<cmbitem>;
        //    ////lstAvailableRoles.ItemsSource = new List<cmbitem>();
        //    //List<cmbitem> cmbitems = new List<cmbitem>();
        //    //foreach (cmbitem cmbitemm in lstAvailableRoles.Items)
        //    //{

        //    //    if (cmbitem.id != cmbitemm.id)
        //    //        cmbitems.Add(cmbitemm);
        //    //    //lstAvailableRoles.ItemsSource = cmbAvailableitems;


        //    //}
        //    //lstAvailableRoles.Items.Clear();
        //    //lstAvailableRoles.ItemsSource = cmbitems;

        //        cmbAssigneditems.Add(cmbitem);
        //        //lstAssignedRoles.Items.Add(cmbitem);
        //        //lstAssignedRoles.ItemsSource = cmbAssigneditems;

        //    cmbAvailableitems.Remove(cmbitem);
        //    //lstAvailableRoles.ItemsSource = cmbAvailableitems;
        //    //lstAvailableRoles.Items.RemoveAt(lstAvailableRoles.Items.IndexOf(lstAvailableRoles.SelectedItem as cmbitem));
        //    availableRoles.Remove(toRemove);
        //}

        //private void BtnRemove_Click(object sender, RoutedEventArgs e)
        //{

        //    Role toRemove = assignedRoles.Find(x => x.Id == ((cmbitem)((ListBox)sender).SelectedItem).id);
        //    availableRoles.Add(toRemove);
        //    var cmbitem = (cmbitem)(lstAssignedRoles).SelectedItem;//new cmbitem() { name = toRemove.Name, id = toRemove.Id, description = toRemove.Description };
        //    List<cmbitem> cmbitems = new List<cmbitem>();
        //    //foreach (cmbitem cmbitemm in lstAssignedRoles.Items)
        //    //{

        //    //    if (cmbitem.id != cmbitemm.id)
        //    //        cmbitems.Add(cmbitemm);
        //    //    //lstAvailableRoles.ItemsSource = cmbAvailableitems;
        //    //    //lstAssignedRoles.ItemsSource = cmbAssigneditems;

        //    //}

        //    //lstAssignedRoles.Items.Remove(cmbitem);
        //    cmbAvailableitems.Add(cmbitem);
        //    cmbAssigneditems.Remove(cmbitem);
        //    //lstAvailableRoles.Items.Add(cmbitem);
        //    //lstAssignedRoles.ItemsSource = cmbitems;

        //    //lstAssignedRoles.Items.RemoveAt(lstAssignedRoles.Items.IndexOf(lstAssignedRoles.SelectedItem as cmbitem));
        //    assignedRoles.Remove(toRemove);
        //}

        private void LookupEmployee_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            employee = lookupEmployee.SelectedItem as ERP_BL.Databases.Employee;
        }

        private void GrdRoles_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)grdRoles.View;
            var node = view.GetNodeByRowHandle(e.ControllerRow);
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    if (node != null)
                        node.IsChecked = true;
                    break;
                case CollectionChangeAction.Remove:
                    if (node != null)
                        node.IsChecked = false;
                    break;
                case CollectionChangeAction.Refresh:
                    var selectedRows = grdRoles.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }

        private void TreeViews_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
            if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                grdRoles.SelectItem(e.Node.RowHandle);
            else
                grdRoles.UnselectItem(e.Node.RowHandle);
        }
    }
}
