using DevExpress.Xpf.Grid;
using ERP_BL.Databases;
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

namespace ZAS_ERP.Userss
{
    /// <summary>
    /// Interaction logic for frmNewRole.xaml
    /// </summary>
    public partial class frmNewRole : Window
    {
        public static int roleId;
        public static bool roleCopy;
       
       public Role role = new Role();
        UsersRepo rolesRepo = new UsersRepo();
        List<Permission> permissions = new List<Permission>();
        Role ParentRole = new Role();  

        public frmNewRole()
        {
            InitializeComponent();
            SystemLog.LogInfo(this.GetType(), "Form Intialized");
            viewTable.NodeCheckStateChanged += OnNodeCheckStateChanged;
            grdPermissions.SelectionChanged += OnGridSelectionChanged;

            roleCopy = false;
        }
        private void OnNodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        {
           
         
                if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
                    grdPermissions.SelectItem(e.Node.RowHandle);
                else
                    grdPermissions.UnselectItem(e.Node.RowHandle);
           
        
           
        }
        private void OnGridSelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var view = (TreeListView)grdPermissions.View;
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
                    var selectedRows = grdPermissions.GetSelectedRowHandles();
                    view.UncheckAllNodes();
                    foreach (var rowHandle in selectedRows)
                    {
                        node = view.GetNodeByRowHandle(rowHandle);
                        node.IsChecked = true;
                    }
                    break;
            }
        }
        public void getPermissions()
        {
          
            if (grdPermissions.SelectedItems.Count != 0)
            {
                role.Permissions = new List<Permission>();
                foreach (Permission permission in grdPermissions.SelectedItems)
                {

                    if (!role.Permissions.Contains(permission))
                    {
                        role.Permissions.Add(permission);
                    }
                }

            }
        }
        //public void getRoleField()
        //{
        //    if(grdRoleField.SelectedItems.Count != 0)
        //    {
        //        role.Fields = new List<RoleField>();
        //        foreach(RoleField roleField in grdRoleField.SelectedItems)
        //        {
        //            if (!role.Fields.Contains(roleField))
        //            {
        //                role.Fields.Add(roleField);
        //            }
        //        }
        //    }
        //}
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                //if (txtRoleName.Text == "")
                //{
                //    MessageBox.Show("Enter Role Name to Continue");
                //    txtRoleName.Focus();
                //    return;
                //}
                //else if (txtDescription.Text == "")
                //{
                //    MessageBox.Show("Enter Description for Role");
                //    txtDescription.Focus();
                //    return;
                //}
                //else if (txtCompany.Text == "")
                //{
                //    MessageBox.Show("Enter Company for Role");
                //    txtCompany.Focus();
                //    return;
                //}
                //else if (txtDepartment.Text == "")
                //{
                //    MessageBox.Show("Enter Department for Role");
                //    txtDepartment.Focus();
                //    return;
                //}

                if(chkHasParent.IsChecked == true && ParentRole.Id == 0)
                {
                    MessageBox.Show("Please Select Parent role first");
                    lookupParentRole.Focus();
                    return;
                }
                getPermissions();
               // getRoleField();
                role.Name = txtRoleName.Text.Trim();
                role.Company = txtCompany.Text;
                role.Department = txtDepartment.Text;
                role.RoleName = txtRoles.Text;
                if (ParentRole.Id != 0)
                    role.parentId = ParentRole.Id;

                if (chkisActive.IsChecked == true)
                {
                    role.isActive = true;
                }
                else
                {
                    role.isActive = false;
                }
                role.LastModified = System.DateTime.Now;
                role.Description = txtDescription.Text.Trim();
                //role.Permissions = permissions;
                if(role.Id == 0 || roleCopy == true)
                {
                    
                    role.Added = System.DateTime.Now;
                    rolesRepo.Add(role);
                    SystemLog.LogInfo(this.GetType(), "Added Role with Name= " + role.Name + " Id= " + role.Id);
                    MessageBox.Show("New Role Added");
                }
                else
                {
                    rolesRepo.Update(role);
                    SystemLog.LogInfo(this.GetType(), "Updated Role with Name= " + role.Name + " Id= " + role.Id);
                    MessageBox.Show("Role Updated");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                SystemLog.LogError(this.GetType(), ex.ToString());
                MessageBox.Show(ex.ToString());
                
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            


            this.Title = "Edit Role";
            loadpermissions();
            //loadRoleField();
            lookupParentRole.ItemsSource = rolesRepo.getAll();

            if (roleId != 0)
            {
                role = rolesRepo.get(roleId);
                if (roleCopy == true)
                {
                    role = new Role()
                    {
                        Company = role.Company , Department = role.Department,Description = role.Description, RoleName = role.RoleName,
                        Name = role.Name, parentId = role.parentId, Permissions = role.Permissions, isActive = role.isActive, Added = role.Added,
                        LastModified = role.LastModified,
   
                    };
                       
                    this.Title = "Copy Role";
                    txtDescription.Text = role.Description;
                    txtCompany.Text = role.Company;
                    txtDepartment.Text = role.Department;
                    txtRoles.Text = role.RoleName;
                    txtRoleName.Text = role.Name + "_Copy";
                    if(role.isActive==true)
                    chkisActive.IsChecked = true;
                    if (role.parentId != null)
                    {
                        ParentRole = rolesRepo.get((int)role.parentId);
                        lookupParentRole.Text = ParentRole.Name;
                        chkHasParent.IsChecked = true;
                    }

                    roleId = 0;
                    foreach (Permission permision in role.Permissions)
                        grdPermissions.SelectItem(grdPermissions.FindRowByValue(grdPermissions.Columns.GetColumnByFieldName("Id"), permision.Id));

                }
                else
                {
                    txtRoles.Text = role.RoleName;
                    txtDescription.Text = role.Description;
                    txtCompany.Text = role.Company;
                    txtDepartment.Text = role.Department;
                    txtRoleName.Text = role.Name;

                    if (role.parentId != null)
                    {
                        ParentRole = rolesRepo.get((int)role.parentId);
                        lookupParentRole.Text = ParentRole.Name;
                        chkHasParent.IsChecked = true;
                    }
                    if (role.isActive == true)
                        chkisActive.IsChecked = true;
                    foreach (Permission permision in role.Permissions)
                        grdPermissions.SelectItem(grdPermissions.FindRowByValue(grdPermissions.Columns.GetColumnByFieldName("Id"), permision.Id));
                }


            }
            else
            {
                chkisActive.IsChecked = true;
            }
        }
        //private void loadRoleField()
        //{
        //    grdRoleField.ItemsSource = rolesRepo.getAllRoleField();
        //}
        private void loadpermissions()
        {
            //rolesRepo.seeddb();
           grdPermissions.ItemsSource= rolesRepo.getAllPermissions();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            roleId = 0;
            roleCopy = false;
        }

        private void GrdPermissions_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            
        }

        private void LookupParentRole_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (lookupParentRole.SelectedIndex > -1)
            {
               
                if (lookupParentRole.SelectedItem != null)
                {
                    ParentRole = new Role();
                    ParentRole = lookupParentRole.SelectedItem as Role;
                }
            }
        }

        private void ChkHasParent_Checked(object sender, RoutedEventArgs e)
        {

            lookupParentRole.IsEnabled = true;

        }

        private void ChkHasParent_Unchecked(object sender, RoutedEventArgs e)

        {
            role.parentId = null;
            role.parentRole = null;
            lookupParentRole.IsEnabled = false;
        }

        private void TxtRoles_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtRoleName.Text = txtCompany.Text + "_" + txtDepartment.Text + "_" + txtRoles.Text;
        }

        private void TxtCompany_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtRoleName.Text = txtCompany.Text + "_" + txtDepartment.Text + "_" + txtRoles.Text;
        }

        private void TxtDepartment_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            txtRoleName.Text = txtCompany.Text + "_" + txtDepartment.Text + "_" + txtRoles.Text;

        }
        //private void TreeListView_NodeCheckStateChanged(object sender, DevExpress.Xpf.Grid.TreeList.TreeListNodeEventArgs e)
        //{
        //    if (e.Node.IsChecked.HasValue && e.Node.IsChecked.Value)
        //        grdRoleField.SelectItem(e.Node.RowHandle);
        //    else
        //        grdRoleField.UnselectItem(e.Node.RowHandle);

        //}

        //private void GrdRoleField_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        //{
        //    var view = (TreeListView)grdRoleField.View;
        //    var node = view.GetNodeByRowHandle(e.ControllerRow);
        //    switch (e.Action)
        //    {
        //        case CollectionChangeAction.Add:
        //            if (node != null)
        //                node.IsChecked = true;
        //            break;
        //        case CollectionChangeAction.Remove:
        //            if (node != null)
        //                node.IsChecked = false;
        //            break;
        //        case CollectionChangeAction.Refresh:
        //            var selectedRows = grdRoleField.GetSelectedRowHandles();
        //            view.UncheckAllNodes();
        //            foreach (var rowHandle in selectedRows)
        //            {
        //                node = view.GetNodeByRowHandle(rowHandle);
        //                node.IsChecked = true;
        //            }
        //            break;
        //    }

        //}
    }
}
