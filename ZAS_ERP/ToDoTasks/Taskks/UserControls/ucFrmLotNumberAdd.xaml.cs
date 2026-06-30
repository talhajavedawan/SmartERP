using DevExpress.Xpf.Core;
using ERP_BL.Databases;
using ERP_BL.ToDoTasks.Taskss;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZAS_ERP.ToDoTasks.Taskks
{
    /// <summary>
    /// Interaction logic for ucFrmLotNumberAdd.xaml
    /// </summary>
    public partial class ucFrmLotNumberAdd : UserControl
    {

        public bool editFlag = false;
        public LotNumber lotNumber = new LotNumber();
        public TaskRepo taskRepo = new TaskRepo();

        public int lotNoId = 0;

        public ucFrmLotNumberAdd()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxCompany.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Company!");
                cmbxCompany.Focus();
                return;
            }
            if (cmbxDepartment.SelectedIndex < 0)
            {
                DXMessageBox.Show("Please select Department!");
                cmbxDepartment.Focus();
                return;
            }
            if (String.IsNullOrEmpty(txtLOTno.Text))
            {
                DXMessageBox.Show("Please enter Bill Ref Number!");
                txtLOTno.Focus();
                return;
            }
            lotNumber.LotNo = txtLOTno.Text;
            lotNumber.deptId = (cmbxDepartment.SelectedItem as Department).Id;
            lotNumber.companyId = (cmbxCompany.SelectedItem as Company).Id;
            lotNumber.isActive = chkIsActive.IsChecked.Value;

            if (editFlag == false)
            {
                taskRepo.AddLOTnumber(lotNumber);
                DXMessageBox.Show("Added Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
            else if (editFlag == true)
            {
                taskRepo.UpdateLotNumber(lotNumber);
                DXMessageBox.Show("Updated Successfully!");
                var myWindow = Window.GetWindow(this);
                myWindow.Close();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            loadCompanies();

            if (editFlag == true)
            {
                lotNumber = taskRepo.GetLotNumber(lotNoId);
                //Select Company
                var companyList = (cmbxCompany.ItemsSource as List<Company>) == null ? new List<Company>() : cmbxCompany.ItemsSource as List<Company>;
                if (lotNumber.company != null)
                {
                    int index = 0;
                    foreach (var _company in companyList)
                    {
                        if (_company.Id == lotNumber.companyId)
                        {
                            cmbxCompany.SelectedIndex = index;
                            index = 0;
                            break;
                        }
                        index++;
                    }
                }

                if (lotNumber.department != null)
                {
                    cmbxDepartment.Text = lotNumber.department.DeptName;
                }

                if (lotNumber.LotNo != null)
                {
                    txtLOTno.Text = lotNumber.LotNo;
                }

                chkIsActive.IsChecked = lotNumber.isActive;
            }
        }

        private void loadCompanies()
        {
            CompanyRepo companyRepo = new CompanyRepo();
            cmbxCompany.ItemsSource = companyRepo.GetActiveCompanies();
        }

        private void CmbxCompany_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            var company = cmbxCompany.SelectedItem as ERP_BL.Databases.Company;

            if (company != null)
                if (company.departments != null)
                {
                    List<Department> departments = new List<Department>();
                    foreach (var _dept in SYSTEM_STATIC.currentUser.employee.departments)
                    {
                        if (_dept.companies.FirstOrDefault(x => x.Id == company.Id) != null)
                            departments.Add(_dept);
                    }

                    cmbxDepartment.ItemsSource = departments;
                }
        }

        public void loaddepartments()
        {
            
        }
    }
}
