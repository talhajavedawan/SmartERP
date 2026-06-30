using DevExpress.Xpf.Core;
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

namespace ZAS_ERP.ToDoTasks.TargetRewardss
{
    /// <summary>
    /// Interaction logic for winSelectLedgerCompDept.xaml
    /// </summary>
    public partial class winSelectLedgerCompDept : DXWindow
    {

        public static Company company { get; set; }
        public static Department department { get; set; }
        public winSelectLedgerCompDept()
        {
            InitializeComponent();
        }
        public winSelectLedgerCompDept(Company _company, Department _department)
        {
            InitializeComponent();
            company = _company;
            department = _department;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lookupCompany.ItemsSource = SYSTEM_STATIC.LoadCurrentUserCompanies();
            if(company!=null)
            {
                lookupCompany.Text = company.CompanyName;
            }
            if(department!=null)
            {
                lookupDepartment.Text = department.DeptName;
            }
        }

        private void lookupCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var company = lookupCompany.SelectedItem as Company;
            if(company.departments.Count>0)
            {
                lookupDepartment.ItemsSource = company.departments;
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(lookupCompany.SelectedIndex==-1)
            {
                DXMessageBox.Show("Please Select Company", "Required Field", MessageBoxButton.OK,MessageBoxImage.Information);
                return;
            }
            else
            if (lookupDepartment.SelectedIndex == -1)
            {
                DXMessageBox.Show("Please Select Department", "Required Field", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                company = lookupCompany.SelectedItem as Company;
                department = lookupDepartment.SelectedItem as Department;
                this.Close();
            }
        }
    }
}
