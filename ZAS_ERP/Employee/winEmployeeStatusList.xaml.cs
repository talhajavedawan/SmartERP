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

namespace ZAS_ERP.Employee
{
    /// <summary>
    /// Interaction logic for winEmployeeStatusList.xaml
    /// </summary>
    public partial class winEmployeeStatusList : Window
    {
        public winEmployeeStatusList()
        {
            InitializeComponent();

            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Employee Status List") != null)
            {
                loadEmployeeStatus();

            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Employee Status") != null)
            {
                btnNewEmployeeStatus.IsEnabled = true;
                mbtnNewEmployeeStatus.IsEnabled = true;
            }
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Edit Employee Status") != null)
            {
                btnEditEmployeeStatus.IsEnabled = true;
            }

        }
        public void loadEmployeeStatus()
        {
            EmpStatusListVM empStatusVM = new EmpStatusListVM();

            grdEmployeeStatus.ItemsSource = empStatusVM.empStatuses;
            grdEmployeeStatus.Columns["Id"].Visible = false;
            grdEmployeeStatus.Columns["isApproved"].Visible = false;
            grdEmployeeStatus.Columns["HierarchicalIndex"].Visible = false;

        }

        public void newEmployeeStatus()
        {
            frmEmployeeStatusAdd newEmpStatus = new frmEmployeeStatusAdd();
            newEmpStatus.isEdit = false;

            newEmpStatus.ShowDialog();

            loadEmployeeStatus();
        }

        private void WinEmployeeStatusList_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void WinEmployeeStatusList_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnNewEmployeeStatus_Click(object sender, RoutedEventArgs e)
        {
            newEmployeeStatus();
        }

        private void BtnEditEmployeeStatus_Click(object sender, RoutedEventArgs e)
        {
            if (grdEmployeeStatus.SelectedItem != null)
            {
                var item = (EmployeeStatusList)grdEmployeeStatus.SelectedItem;
                var itemId = item.Id.ToString();

                frmEmployeeStatusAdd newEmpStatus = new frmEmployeeStatusAdd();
                newEmpStatus.empStatusTxtbx.Text = item.StatusName;
                newEmpStatus.empStatusIsActive.IsChecked = item.isActive;
                newEmpStatus.empStatusColorEdit.Text = item.BgColor.ToString();
                newEmpStatus.empStatusId.Text = itemId;
                newEmpStatus.isEdit = true;


                newEmpStatus.ShowDialog();

                loadEmployeeStatus();

            }
        }
    }




    public class EmployeeStatusList
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public bool isApproved { get; set; }
        public bool isActive { get; set; }
        public Color BgColor { get; set; }
        public Color FgColor { get; set; }
        public int HierarchicalIndex { get; set; }
    }

    public class EmpStatusListVM
    {
        EmployeeRepo repo = new EmployeeRepo();
        public List<EmployeeStatusList> empStatuses = new List<EmployeeStatusList>();
        public EmpStatusListVM()
        {
            List<EmployeeStatusList> _empStatuses = new List<EmployeeStatusList>();
            var status = repo.GetAllEmployeeStatus();
            foreach (var _status in status)
            {
                EmployeeStatusList lst = new EmployeeStatusList();
                if (_status.backcolor != null)
                {
                    Color color = (Color)ColorConverter.ConvertFromString(_status.backcolor);

                    lst.BgColor = color;
                }
                if (_status.forecolor != null)
                {
                    Color color = (Color)ColorConverter.ConvertFromString(_status.forecolor);

                    lst.FgColor = color;
                }

                if (_status.HierarchicalIndex != 0)
                {
                    lst.HierarchicalIndex = _status.HierarchicalIndex;

                }

                if (_status.Id != 0)
                {
                    lst.Id = _status.Id;

                }

                lst.isActive = (bool)_status.isActive;
                lst.isApproved = (bool)_status.isApproved;

                if (_status.Status != null)
                {
                    lst.StatusName = _status.Status;
                }


                _empStatuses.Add(lst); ;

                //_assetStatuses.Add(_status);
            }
            empStatuses = _empStatuses;

        }
    }
}
