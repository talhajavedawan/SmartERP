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
    /// Interaction logic for frmDepartmentSelect.xaml
    /// </summary>
    public partial class frmDepartmentSelect : Window
    {
        DepartmentRepo depRepo = new DepartmentRepo();
        public Department department = new Department();

        public bool isManagerial = false;


        public frmDepartmentSelect()
        {
            InitializeComponent();

            //var depts = depRepo.GetDepartments();

            
        }

        private void BtnSelectDepart_Click(object sender, RoutedEventArgs e)
        {
            if (cmbDep.SelectedItem != null)
            {
                var _department = cmbDep.SelectedItem as Department;
                department = _department;
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (isManagerial == true)
            {
                var depts = depRepo.GetManagerialDepts();
                cmbDep.ItemsSource = depts;
            }
            else
            {
                var depts = depRepo.GetHRMDepartments();
                cmbDep.ItemsSource = depts;
            }
        }
    }
}
