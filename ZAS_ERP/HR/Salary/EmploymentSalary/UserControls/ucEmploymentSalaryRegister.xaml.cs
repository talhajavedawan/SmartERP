using DevExpress.Xpf.Core;
using ERP_BL.HR;
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

namespace ZAS_ERP.HR.Salary.EmploymentSalary.UserControls
{
    /// <summary>
    /// Interaction logic for ucEmploymentSalaryRegister.xaml
    /// </summary>
    public partial class ucEmploymentSalaryRegister : UserControl
    {
        PayrollRepo payrollRepo = new PayrollRepo();
        public ucEmploymentSalaryRegister()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            grdEmploymentSalary.ItemsSource = payrollRepo.GetAllEmploymentSalaries();
            SYSTEM_STATIC.SetUserSettingOfCurrentWindow(grdEmploymentSalary);
        }

        private void MbtnAddNew_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            AddNew();
        }

        private void MbtnUpdate_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            Update();
        }

        

        private void GrdEmploymentSalary_CustomUnboundColumnData(object sender, DevExpress.Xpf.Grid.GridColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Employee" & e.IsGetData)
            {
                var empSal = grdEmploymentSalary.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.HR.EmploymentSalary;
                string employeeName = "";

                if(empSal != null)
                {
                    if(empSal.employee != null)
                    {
                        if (empSal.employee.person != null)
                            employeeName = empSal.employee.person.FName + " " + empSal.employee.person.LName;
                    }
                }
                e.Value = employeeName;
            }
            if (e.Column.FieldName == "Creator" & e.IsGetData)
            {
                var empSal = grdEmploymentSalary.GetRowByListIndex(e.ListSourceRowIndex) as ERP_BL.HR.EmploymentSalary;
                string creator = "";

                if (empSal != null)
                {
                    if (empSal.creator != null)
                    {
                        if (empSal.creator.employee!= null && empSal.creator.employee.person != null)
                            creator = empSal.creator.employee.person.FName + " " + empSal.creator.employee.person.LName;
                    }
                }
                e.Value = creator;
            }
        }

        private void MbtnSaveLayout_Click(object sender, RoutedEventArgs e)
        {
            SYSTEM_STATIC.SaveUserSettingForCurrentWindow(grdEmploymentSalary);
        }

        private void MbtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            payrollRepo = new PayrollRepo();
            grdEmploymentSalary.ItemsSource = payrollRepo.GetAllEmploymentSalaries();
        }

        private void MbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void MbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNew();
        }

        private void AddNew()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "Add Employment Salary") != null)
            {
                ucEmploymentSalaryAdd employmentSalaryAdd = new ucEmploymentSalaryAdd();
                employmentSalaryAdd.employmentSalaryId = 0;
                employmentSalaryAdd.editFlag = false;
                Window win = new Window();
                win.Title = "Employment Salary";
                win.Height = 500;
                win.Width = 900;
                win.Content = employmentSalaryAdd;
                win.Show();
            }
            else
            {
                DXMessageBox.Show("Permission required to Add new Employment Salary Contract!");
            }
        }

        private void Update()
        {
            if (SYSTEM_STATIC.AllowedPermissions.Find(x => x.Name == "View Employment Salary") != null)
            {
                var selectedRow = grdEmploymentSalary.SelectedItem as ERP_BL.HR.EmploymentSalary;
                if (selectedRow != null)
                {
                    ucEmploymentSalaryAdd employmentSalaryAdd = new ucEmploymentSalaryAdd();
                    employmentSalaryAdd.employmentSalaryId = selectedRow.Id;
                    employmentSalaryAdd.editFlag = true;
                    Window win = new Window();
                    win.Title = "Employment Salary";
                    win.Height = 500;
                    win.Width = 900;
                    win.Content = employmentSalaryAdd;
                    win.Show();
                }
            }
            else
            {
                DXMessageBox.Show("Permission required to View Employment Salary Contract!");
            }

        }

        private void MbtnPrintPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnExportToReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnApprove_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnPending_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MbtnApprovedUnapproved_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
