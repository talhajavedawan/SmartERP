using ERP_BL.BackgroundImages;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ZAS_ERP.BackgroundUpload
{
    /// <summary>
    /// Interaction logic for frmSelectGroups.xaml
    /// </summary>
    public partial class frmSelectGroups : Window
    {
       public BackgroundImagesRepo taskRepoGroup = new BackgroundImagesRepo(); 
        List<TaskGroups> TaskGroups = new List<TaskGroups>();
        public List<User> groupUser = new List<User>();
        public int groupId;
        public bool isCancle;
        public frmSelectGroups()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadGroups(); //commit added
            }
            catch (Exception)
            {

                throw;
            }
           
        }
        private void LoadGroups()
        {


            TaskGroups = taskRepoGroup.GetAllTaskGroups();
            grdCntrlGroupList.ItemsSource = TaskGroups;

        }
        private void tblGroupListView_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.TreeList.TreeListUnboundColumnDataEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "dept" && e.IsGetData)
                {
                    var dept = grdCntrlGroupList.GetRow(e.Node.RowHandle) as TaskGroups;
                    string deptNames = "";

                    if (dept.Departments != null && dept.Departments.Count > 0)
                    {
                        deptNames = String.Join(" | ", dept.Departments.Select(x => x.DeptName));
                    }
                    e.Value = deptNames;
                }

                if (e.Column.FieldName == "comp" && e.IsGetData)
                {
                    var cmpy = grdCntrlGroupList.GetRow(e.Node.RowHandle) as TaskGroups;
                    string cmpyNames = "";

                    if (cmpy.Companies != null && cmpy.Companies.Count > 0)
                    {
                        cmpyNames = String.Join(" | ", cmpy.Companies.Select(x => x.CompanyName));
                    }
                    e.Value = cmpyNames;
                }
                if (e.Column.FieldName == "emp" && e.IsGetData)
                {
                    var user = grdCntrlGroupList.GetRow(e.Node.RowHandle) as TaskGroups;
                    string userNames = "";

                    if (user.users != null && user.users.Count > 0)
                    {
                        userNames = String.Join(" | ", user.users.Select(x => x.userName));
                    }
                    e.Value = userNames;
                }
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroup = grdCntrlGroupList.SelectedItem as TaskGroups;
            if (selectedGroup != null)
            {
                foreach (var _user in selectedGroup.users)
                {
                    groupUser.Add(_user);
                }
                groupId = selectedGroup.Id;
                isCancle = true;
                this.Close();
            }
        }
    }
}
