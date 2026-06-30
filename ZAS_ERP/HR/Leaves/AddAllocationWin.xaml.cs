using DevExpress.Xpf.Core;
using ERP_BL.Databases;
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
using System.Windows.Shapes;

namespace ZAS_ERP.HR.Leaves
{
    /// <summary>
    /// Interaction logic for AddAllocationWin.xaml
    /// </summary>
    public partial class AddAllocationWin : Window
    {
        HrRepo hrRepo = new HrRepo();
        EmployeeRepo empRepo = new EmployeeRepo();
        public AddAllocationWin()
        {
            InitializeComponent();
        }

        private void BtnAllocate_Click(object sender, RoutedEventArgs e)
        {
            ERP_BL.Databases.Employee employee = null;
            try
            {
                if (String.IsNullOrEmpty(txtCasual.Text) || String.IsNullOrEmpty(txtannual.Text))
                {
                    DXMessageBox.Show("Fields Cannot be null","Null Fields",MessageBoxButton.OK,MessageBoxImage.Exclamation);
                    return;
                }

                int empId = 0;
                if (!String.IsNullOrEmpty(txtId.Text))
                {
                    empId = Convert.ToInt32(txtId.Text);
                }
                if(empId!=0)
                {
                    employee = empRepo.GetEmployee(empId);
                       var casualLeave = hrRepo.GetEmployeeLeaves(empId).FirstOrDefault(x => x.LeaveDes == "Allocation" && x.DateFrom.Year == DateTime.Now.Year &&x.LeaveType== ERP_BL.Enums.LeaveType.CasualLeave);
                    var annualLeave = hrRepo.GetEmployeeLeaves(empId).FirstOrDefault(x => x.LeaveDes == "Allocation" && x.DateFrom.Year == DateTime.Now.Year && x.LeaveType == ERP_BL.Enums.LeaveType.AnnualLeave);
                    if (casualLeave == null)
                    {
                        Leave leave = new Leave();
                        leave.LeaveType = ERP_BL.Enums.LeaveType.CasualLeave;
                        //if (employee.Desig != null)
                        //{
                        //    if (employee.Desig.AnnualLeaveDays != 0)
                            leave.LeaveDays = Convert.ToInt32(txtCasual.Text);
                            leave.LeaveDes = "Allocation";
                            //leave.employeeId = _emp.EmpId;
                            leave.employee = employee;
                            leave.DateFrom = DateTime.Now;
                            leave.DateTo = DateTime.Now;
                            leave.ApplyDate = DateTime.Now;

                            hrRepo.AddEmployeeLeave(leave);

                    }

                    else
                    {
                        casualLeave.LeaveDays = Convert.ToInt32(txtCasual.Text);
                        hrRepo.UpdateLeave(casualLeave);

                    }
                    if (annualLeave == null)
                    {
                        Leave leave = new Leave();
                        leave.LeaveType = ERP_BL.Enums.LeaveType.AnnualLeave;
                        //if (employee.Desig != null)
                        //{
                        //    if (employee.Desig.AnnualLeaveDays != 0)
                        leave.LeaveDays = Convert.ToInt32(txtannual.Text);
                        leave.LeaveDes = "Allocation";
                        //leave.employeeId = _emp.EmpId;
                        leave.employee = employee;
                        leave.DateFrom = DateTime.Now;
                        leave.DateTo = DateTime.Now;
                        leave.ApplyDate = DateTime.Now;

                        hrRepo.AddEmployeeLeave(leave);

                    }
                    else
                    {
                        annualLeave.LeaveDays = Convert.ToInt32(txtannual.Text);
                        hrRepo.UpdateLeave(annualLeave);

                    }
                    DXMessageBox.Show("Leave allocated Successfully", "Successfull", MessageBoxButton.OK, MessageBoxImage.Information);

                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();

        }
    }
}
