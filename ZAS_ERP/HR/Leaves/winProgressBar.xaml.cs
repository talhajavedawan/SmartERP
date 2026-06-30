using ERP_BL.Databases;
using ERP_BL.HR;
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

namespace ZAS_ERP.HR.Leaves
{
    /// <summary>
    /// Interaction logic for winProgressBar.xaml
    /// </summary>
    public partial class winProgressBar : Window
    {
        public HrRepo hrRepo = new HrRepo();
        public EmployeeRepo empRepo = new EmployeeRepo();

        public winProgressBar()
        {
            EmployeeRepo empRepo = new EmployeeRepo();

            var lstEmp = empRepo.GetAllEmployees();
            var empCount = lstEmp.Count();

            InitializeComponent();

            pbCalculationProgress.Value = 0;
            //lbResults.Items.Clear();

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(empCount);
        }
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                
                var lstEmp = empRepo.GetAllEmployees();
                var empCount = lstEmp.Count();

                int result = 0;
                int max = (int)e.Argument;

                int i = 0;
                var currentYear = DateTime.Now.Year;

                foreach (var _emp in lstEmp)
                {
                    double annualBalance = 0;
                    double casualBalance = 0;
                    double adjustmentBalance = 0;
                    int progressPercentage = Convert.ToInt32(((double)i / max) * 100);

                    var empLeaveList = hrRepo.GetEmployeeLeaves(_emp.EmpId).Where(x=>x.LeaveDes == "Allocation").ToList();

                    if (empLeaveList.Count == 0)
                    {
                        Leave annLeave = new Leave();
                        annLeave.LeaveType = ERP_BL.Enums.LeaveType.AnnualLeave;
                        if (_emp.Desig != null)
                        {
                            if (_emp.Desig.AnnualLeaveDays != 0)
                                annLeave.LeaveDays = _emp.Desig.AnnualLeaveDays;
                            annLeave.LeaveDes = "Allocation";
                            //leave.employeeId = _emp.EmpId;
                            annLeave.employee = _emp;
                            annLeave.DateFrom = DateTime.Now;
                            annLeave.DateTo = DateTime.Now;
                            annLeave.ApplyDate = DateTime.Now;

                            hrRepo.AddEmployeeLeave(annLeave);

                        }
                        Leave casLeave = new Leave();
                        casLeave.LeaveType = ERP_BL.Enums.LeaveType.CasualLeave;
                        if (_emp.Desig != null)
                        {
                            if (_emp.Desig.CasualLeaveDays != 0)
                            {
                                casLeave.LeaveDays = _emp.Desig.CasualLeaveDays;
                                casLeave.LeaveDes = "Allocation";
                                //leave.employeeId = _emp.EmpId;
                                casLeave.employee = _emp;
                                casLeave.DateFrom = DateTime.Now;
                                casLeave.DateTo = DateTime.Now;
                                casLeave.ApplyDate = DateTime.Now;

                                hrRepo.AddEmployeeLeave(casLeave);
                            }

                        }
                    }
                    else
                    {
                        var lastLeave = empLeaveList.Last();
                        var lastLeaveYear = (DateTime)lastLeave.ApplyDate;
                        if (lastLeaveYear.Year == DateTime.Now.Year)
                        {

                        }
                        else
                        {
                            //Calculate current Annual Leaves Balance
                            var annualLeaves1 = hrRepo.GetEmployeeAnnualLeaves(_emp.EmpId);

                            if (annualLeaves1 != null)
                            {
                                annualBalance = annualLeaves1.Sum(x => x.LeaveDays);
                            }
                            else
                            {
                                annualBalance = 0;
                            }

                            //Calculate current Adjustment Leaves Balance
                            var adjustmentLeaves = hrRepo.GetEmployeeAdjustedLeaves(_emp.EmpId);

                            if (adjustmentLeaves != null)
                            {
                                adjustmentBalance = adjustmentLeaves.Sum(x => x.LeaveDays);
                            }
                            else
                            {
                                adjustmentBalance = 0;
                            }

                            //Calculate current casual Leaves Balance
                            var casualLeaves = hrRepo.GetEmployeeCasualLeaves(_emp.EmpId);
                            if (casualLeaves != null)
                            {
                                casualBalance = casualLeaves.Sum(x => x.LeaveDays);
                            }
                            else
                            {
                                casualBalance = 0;
                            }

                            var totalBalance = annualBalance + adjustmentBalance;

                            //Create oppsite entry to make annual balance zero
                            if (annualBalance != 0)
                            {
                                var days = 0 - annualBalance;
                                Leave annLeave = new Leave()
                                {
                                    LeaveType = ERP_BL.Enums.LeaveType.AnnualLeave,
                                    LeaveDes = "AnnualAdjustment",
                                    ApplyDate = (DateTime)DateTime.Now,
                                    DateFrom = (DateTime)DateTime.Now,
                                    DateTo = (DateTime)DateTime.Now,
                                    LeaveDays = days,
                                    employee = _emp,
                                    employeeId = _emp.EmpId
                                };
                                hrRepo.AddEmployeeLeave(annLeave);
                            }

                            //Create oppsite entry to make Casual balance zero
                            if (casualBalance != 0)
                            {
                                var days = 0 - casualBalance;
                                Leave casLeave = new Leave()
                                {
                                    LeaveType = ERP_BL.Enums.LeaveType.CasualLeave,
                                    LeaveDes = "CasualAdjustment",
                                    ApplyDate = (DateTime)DateTime.Now,
                                    DateFrom = (DateTime)DateTime.Now,
                                    DateTo = (DateTime)DateTime.Now,
                                    LeaveDays = days,
                                    employee = _emp,
                                    employeeId = _emp.EmpId
                                };
                                hrRepo.AddEmployeeLeave(casLeave);
                            }


                            //Create oppsite entry to make Adjusted leaves balance zero
                            if (adjustmentBalance != 0)
                            {
                                var days = 0 - adjustmentBalance;
                                Leave adjLeave = new Leave()
                                {
                                    LeaveType = ERP_BL.Enums.LeaveType.Adjustment,
                                    LeaveDes = "AdjustedLeavesAdjustment",
                                    ApplyDate = (DateTime)DateTime.Now,
                                    DateFrom = (DateTime)DateTime.Now,
                                    DateTo = (DateTime)DateTime.Now,
                                    LeaveDays = days,
                                    employee = _emp,
                                    employeeId = _emp.EmpId
                                };
                                hrRepo.AddEmployeeLeave(adjLeave);
                            }

                            //else if(totalBalance == 0)
                            //{

                            //}

                            Leave leave = new Leave();
                            leave.LeaveType = ERP_BL.Enums.LeaveType.AnnualLeave;
                            if (_emp.Desig != null)
                            {
                                if (_emp.Desig.AnnualLeaveDays != 0)
                                    leave.LeaveDays = _emp.Desig.AnnualLeaveDays;
                                leave.LeaveDes = "Allocation";
                                //leave.employeeId = _emp.EmpId;
                                leave.employee = _emp;
                                leave.DateFrom = DateTime.Now;
                                leave.DateTo = DateTime.Now;
                                leave.ApplyDate = DateTime.Now;

                                hrRepo.AddEmployeeLeave(leave);

                            }
                            Leave leave2 = new Leave();
                            leave2.LeaveType = ERP_BL.Enums.LeaveType.CasualLeave;
                            if (_emp.Desig != null)
                            {
                                if (_emp.Desig.CasualLeaveDays != 0)
                                {
                                    leave2.LeaveDays = _emp.Desig.CasualLeaveDays;
                                    leave2.LeaveDes = "Allocation";
                                    //leave.employeeId = _emp.EmpId;
                                    leave2.employee = _emp;
                                    leave2.DateFrom = DateTime.Now;
                                    leave2.DateTo = DateTime.Now;
                                    leave2.ApplyDate = DateTime.Now;

                                    hrRepo.AddEmployeeLeave(leave2);
                                }

                            }


                        }

                    }

                 
                    result++;
                    (sender as BackgroundWorker).ReportProgress(progressPercentage, i);

                    e.Result = result;
                    i++;

                    //}
                    //catch
                    //{
                    //    (sender as BackgroundWorker).ReportProgress(progressPercentage);
                    //    System.Threading.Thread.Sleep(1);
                    //}

                }



                //int max = (int)e.Argument;
                //int result = 0;
                //for (int i = 0; i < max; i++)
                //{
                //    int progressPercentage = Convert.ToInt32(((double)i / max) * 100);
                //    if (i % 42 == 0)
                //    {
                //        result++;
                //        (sender as BackgroundWorker).ReportProgress(progressPercentage, i);
                //    }
                //    else
                //        (sender as BackgroundWorker).ReportProgress(progressPercentage);
                //    System.Threading.Thread.Sleep(1);

                //}
                //e.Result = result;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        public void AddEmployeeLeave(Leave leave, ERP_BL.Enums.LeaveType leaveType, ERP_BL.Databases.Employee emp)
        {

            leave.LeaveType = leaveType;
            if (emp.Desig != null)
            {
                if (emp.Desig.CasualLeaveDays != 0)
                {
                    leave.LeaveDays = emp.Desig.CasualLeaveDays;
                    leave.LeaveDes = "Allocation";
                    //leave.employeeId = _emp.EmpId;
                    leave.employee = emp;
                    leave.DateFrom = DateTime.Now;
                    leave.DateTo = DateTime.Now;
                    leave.ApplyDate = DateTime.Now;

                    hrRepo.AddEmployeeLeave(leave);
                }
            }
}
void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbCalculationProgress.Value = e.ProgressPercentage;
            //if (e.UserState != null)
                //lbResults.Items.Add(e.UserState);
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("All employees updated Successfully: " + e.Result);
            this.Close();
        }
    }
}
