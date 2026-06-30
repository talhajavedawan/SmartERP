using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERP_BL.HR
{
    public class HrRepo
    {
        DBContextERP context = new DBContextERP();

        //Fnx for Leave Status

        /// <summary>
        /// Add Leave status in DB
        /// </summary>
        /// <param name="leaveStatus"></param>
        public void AddLeaveStatus(LeaveStatus leaveStatus)
        {
           
            if (leaveStatus == null)
                throw new NullReferenceException("Object can not be null");

            context.leaveStatuses.Add(leaveStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Get all Leave statuses Lost
        /// </summary>
        /// <returns></returns>
        public List<LeaveStatus> GetAllLeaveStatus()
        {
            return context.leaveStatuses.ToList();
        }

        /// <summary>
        /// Get all Active Leave Status
        /// For "OPEN" Status
        ///  </summary>
        /// <returns></returns>
        public List<LeaveStatus> GetAllActiveLeaveStatus()
        {
            return context.leaveStatuses
                .Where(x => x.isActive == true)
                .ToList();
        }

        public LeaveStatus GetLeaveStatus(int id)
        {
            return context.leaveStatuses.FirstOrDefault(x=>x.Id == id);
        }
        /// <summary>
        /// Get all Inactive Leave Status.
        /// For "CLOSE" status
        /// </summary>
        /// <returns></returns>
        public List<LeaveStatus> GetAllInActiveLeaveStatus()
        {
            return context.leaveStatuses
                .Where(x => x.isActive == false)
                .ToList();
        }

        /// <summary>
        /// Get Leave Status based on Id.
        /// </summary>
        ///  <param name="leaveStatusId"></param>
        /// <returns>LeaveStatus</returns>
        public LeaveStatus GetEmployeeStatus(int leaveStatusId)
        {
            var status = context.leaveStatuses
                //.Include("Asset")
                .FirstOrDefault(x => x.Id == leaveStatusId);

            return status;
        }

        /// <summary>
        /// Updates Leave status
        /// </summary>
        /// <param name="leaveStatus"></param>
        public void UpdateLeaveStatus(LeaveStatus leaveStatus)
        {
            if (leaveStatus == null)
                throw new NullReferenceException("Object can not be null");

            var _LeaveStatus = context.leaveStatuses.FirstOrDefault(x => x.Id == leaveStatus.Id);
            if (_LeaveStatus == null)
                throw new Exception("Employee Status object not found");

            _LeaveStatus.backcolor = leaveStatus.backcolor;
            _LeaveStatus.forecolor = leaveStatus.forecolor;
            _LeaveStatus.HierarchicalIndex = leaveStatus.HierarchicalIndex;
            _LeaveStatus.isActive = leaveStatus.isActive;
            _LeaveStatus.isApproved = leaveStatus.isApproved;
            _LeaveStatus.Status = leaveStatus.Status;

            context.SaveChanges();

        }

        //Fnx related Leave Applications
        public void AddLeaveApplication(LeaveApplication app/*, Leave leave*/)
        {
            if (app == null)
                throw new NullReferenceException("Object can not be null");
            if (app.employee != null)
            {
                var emp = context.Employees


                .FirstOrDefault(x => x.EmpId == app.employee.EmpId);

                app.employee = emp;
            }

            //if (app.leaveStatus != null)
            //{
            //    var status = context.leaveStatuses.FirstOrDefault(x => x.Id == app.leaveStatus.Id);
            //    if (status != null)
            //    {
            //        app.leaveStatus = status;
            //    }
            //}


            //app.leave = leave;
            //if (app.leave != null)
            //{
            //    var leave = context.leaves.FirstOrDefault(x => x.Id == app.leave.Id);
            //    leave = app.leave;
            //}
            //Leave leave = new Leave();
            //leave = app.leave;
            context.leaveApplications.Add(app);
            context.SaveChanges();
        }

        public void UpdateLeaveApplication(LeaveApplication app)
        {
            LeaveApplication leaveApp = new LeaveApplication();
            leaveApp = app;
            context.SaveChanges();
        }

        public void UpdateLeave(Leave leave)
        {
            Leave leave2 = new Leave();
            leave2 = leave;
            context.SaveChanges();
        }
        public LeaveApplication GetLeaveApplication(int id)
        {
            var leaveApp = context.leaveApplications


                .FirstOrDefault(x => x.Id == id);
            return leaveApp;

        }
        public List<LeaveApplication> GetAllLeaveApplicationsForReg()
        {
            var leaveLists = context.leaveApplications
       
            .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplications()
        {
            var leaveLists = context.leaveApplications
  

                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false)
                .ToList();
                return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplications(int empId)
        {
            var leaveLists = context.leaveApplications
           

                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false && x.employee.EmpId == empId)
                //.Where(x => x.employee.EmpId == empId)
                .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplicationsDay(DateTime day)
        {
            var leaveLists = context.leaveApplications
   

                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false)
                .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplicationsMonth(DateTime month)
        {
            var leaveLists = context.leaveApplications
        

                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false && x.StartDate.Month == month.Month)
                //.Where(x=>x.StartDate.Month == month.Month)
                .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplicationsYear(DateTime year)
        {
            var leaveLists = context.leaveApplications
         

                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false && x.StartDate.Year == year.Year)
                               // .Where(x => x.StartDate.Year == year.Year)

                .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplicationsDate(DateTime date)
        {
            var leaveLists = context.leaveApplications
           

                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false)
                .Where(x => x.StartDate.Day == date.Day && x.StartDate.Month == date.Month && x.StartDate.Year == date.Year)

                .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplicationsDay(DateTime day,int EmpId)
        {
            var leaveLists = context.leaveApplications
  

                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false)
                .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplicationsMonth(DateTime month, int EmpId)
        {
            var leaveLists = context.leaveApplications
             
                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false && x.StartDate.Month == month.Month && x.employee.EmpId == EmpId)
                //.Where(x => x.StartDate.Month == month.Month)
                //.Where(x => x.employee.EmpId == EmpId)
                .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplicationsYear(DateTime year, int EmpId)
        {
            var leaveLists = context.leaveApplications
              

                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false
                        && x.StartDate.Year == year.Year
                        && x.employee.EmpId == EmpId)
                                //.Where(x => x.StartDate.Year == year.Year)
                                //.Where(x => x.employee.EmpId == EmpId)

                .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetAllLeaveApplicationsDate(DateTime date, int EmpId)
        {
            var leaveLists = context.leaveApplications
           

                .Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false
                        && x.StartDate.Day == date.Day && x.StartDate.Month == date.Month && x.StartDate.Year == date.Year
                        && x.employee.EmpId == EmpId)
                //.Where(x => x.StartDate.Day == date.Day && x.StartDate.Month == date.Month && x.StartDate.Year == date.Year)
                //.Where(x => x.employee.EmpId == EmpId)

                .ToList();
            return leaveLists;
        }


        public List<LeaveApplication> GetEmployeeLeaveApplications(int empId)
        {
            var leaveLists = context.leaveApplications


                .Where(x=>x.employee.EmpId == empId
                        && x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false
                        && x.StartDate.Year == DateTime.Now.Year)
                //.Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false)
                //.Where(x=> x.StartDate.Year == DateTime.Now.Year)
                .ToList();
            return leaveLists;
        }
        public List<LeaveApplication> GetEmployeeLeaveApplications(int empId, int year)
        {
            var leaveLists = context.leaveApplications
             
                .Where(x => x.employee.EmpId == empId
                && x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false
                && x.StartDate.Year == year)
                //.Where(x => x.isVoid != true && x.PendingForClosing != true && x.isApproved != false && x.isReApproved != false)
                //.Where(x => x.StartDate.Year == year)
                .ToList();
            return leaveLists;
        }
        //FnxRelatedLeaves
        public List<Leave> GetEmployeeLeaves(int empId)
        {
            var emp = context.Employees.FirstOrDefault(x=>x.EmpId == empId);

            var leaves = context.leaves
                .Where(x => x.employeeId == empId)
                .ToList();

            return leaves;

        }

        public List<Leave> GetTotalLeaves(int empId)
        {
            var leaves = context.leaves
                .Where(x => x.employeeId == empId)
                .ToList();

            return leaves;
        }

        public List<Leave> GetEmployeeCasualLeaves(int empId)
        {
            var casualLeaves = context.leaves
                   .Where(x => x.employeeId == empId && x.LeaveType == Enums.LeaveType.CasualLeave && x.LeaveDate.Value.Year == DateTime.Now.Year)
                   //.Where(x => x.LeaveType == Enums.LeaveType.CasualLeave)
              .ToList()
              ;
            return casualLeaves;
        }

        public List<Leave> GetEmployeeCasualLeaves(int empId, DateTime date, int Id)
        {
            var casualLeaves = context.leaves
                   .Where(x => x.employeeId == empId && x.LeaveType == Enums.LeaveType.CasualLeave && x.Id < Id && x.LeaveDate.Value.Year == date.Year)
              //.Where(x => x.LeaveType == Enums.LeaveType.CasualLeave)
              .ToList()
              ;
            return casualLeaves;
        }
        public List<Leave> GetEmployeeAnnualLeaves(int empId)
        {
            var annualLeaves = context.leaves
                 .Where(x => x.employeeId == empId && x.LeaveType == Enums.LeaveType.AnnualLeave && x.LeaveDate.Value.Year == DateTime.Now.Year).ToList();
                 //.Where(x => x.LeaveType == Enums.LeaveType.AnnualLeave);
            if (annualLeaves != null)
                return annualLeaves;
            else
            return null;
        }
        public List<Leave> GetEmployeeAnnualLeaves(int empId, DateTime date, int Id)
        {
            var annualLeaves = context.leaves
                 .Where(x => x.employeeId == empId && x.LeaveType == Enums.LeaveType.AnnualLeave && x.Id <Id && x.LeaveDate.Value.Year == date.Year/*x.ApplyDate < date *//*&& x.ApplyDate != date*/).ToList();
            //.Where(x => x.LeaveType == Enums.LeaveType.AnnualLeave);
            if (annualLeaves != null)
                return annualLeaves.ToList();
            else
            return null;
        }
        public List<Leave> GetEmployeeAdjustedLeaves(int empId)
        {
            var casualLeaves = context.leaves
                   .Where(x => x.employeeId == empId && x.LeaveType == Enums.LeaveType.Adjustment && x.LeaveDate.Value.Year == DateTime.Now.Year)
              //.Where(x => x.LeaveType == Enums.LeaveType.CasualLeave)
              .ToList()
              ;
            return casualLeaves;
        }

        public List<Leave> GetEmployeeAdjustedLeaves(int empId, DateTime date, int Id)
        {
            var casualLeaves = context.leaves
                   .Where(x => x.employeeId == empId && x.LeaveType == Enums.LeaveType.Adjustment && x.Id < Id && x.LeaveDate.Value.Year == date.Year)
              //.Where(x => x.LeaveType == Enums.LeaveType.CasualLeave)
              .ToList()
              ;
            return casualLeaves;
        }

        public Leave GetEmployeeLeave(int id)
        {
            var leave = context.leaves
      
                .FirstOrDefault(x=>x.Id == id)
                ;
            return leave;
        }

        public double GetEmployeeAllocatedAnnualLeaves(int empId, DateTime year, string desc)
        {
            var AnnualLeaves = context.leaves
                 .Where(x => x.employeeId == empId
                 && x.LeaveType == Enums.LeaveType.AnnualLeave
                 && x.LeaveDes == desc
                 && x.DateFrom.Year == year.Year
     
                 )
                 .ToList();



            if (AnnualLeaves.Count != 0)
            {
                var leave = AnnualLeaves.Last();
                return leave.LeaveDays;

            }

            else
                return 0;
        }

        public double GetEmployeeAllocatedCasualLeaves(int empId, DateTime year, string desc)
        {

            var casualLeaves = context.leaves
                 .Where(x => x.employeeId == empId
                 && x.LeaveType == Enums.LeaveType.CasualLeave
                 && x.LeaveDes == desc
                 && x.DateFrom.Year == year.Year
                 //&& (System.Convert.ToDateTime(x.ApplyDate).Year == year))
                 )
                 .ToList();
                 

            if (casualLeaves.Count != 0)
            {
                var leave = casualLeaves.Last();
                return leave.LeaveDays;
                
            }

            else
                return 0;
        }
        public void AddEmployeeLeave(Leave leave)
        {

            if (leave == null)
                throw new NullReferenceException("Object can not be null");
            if (leave.employee != null)
            {
                leave.employee = context.Employees

                    .FirstOrDefault(x => x.EmpId == leave.employee.EmpId)                   
                    ;             

            }
            // ((IObjectContextAdapter)context).ObjectContext.Detach(leave);
            context.leaves.Add(leave);
            context.SaveChanges();            
        }

        public List<LeaveApplication> GetPendingForApprovalLeave()
        {
            return context.leaveApplications
  

                .Where(x => x.isApproved == false && x.isVoid != true)
                .ToList();
        }

        public List<LeaveApplication> GetVoidLeave()
        {
            return context.leaveApplications
          
                .Where(x => x.isVoid == true)
                .ToList();
        }
        public List<LeaveApplication> GetPendingForReapprovalLeave()
        {
            return context.leaveApplications
           

                .Where(x => x.isReApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<LeaveApplication> GetAllLeavesRegister()
        {
            return context.leaveApplications
        
                                .Where(x=> x.isVoid != true)
                .ToList();
        }

        public List<LeaveApplication> GetPendingForApprovalLeave(int empId)
        {
            return context.leaveApplications
           

                .Where(x => x.isApproved == false && x.employee.EmpId == empId && x.isVoid!= true)
                .ToList();
        }

        public List<LeaveApplication> GetVoidLeave(int empId)
        {
            return context.leaveApplications
             

                .Where(x => x.isVoid == true && x.employee.EmpId == empId)
                .ToList();
        }
        public List<LeaveApplication> GetPendingForReapprovalLeave(int empId)
        {
            return context.leaveApplications
             

                .Where(x => x.isReApproved == false && x.employee.EmpId == empId && x.isVoid != true)
                .ToList();
        }
        public List<LeaveApplication> GetAllLeavesRegister(int empId)
        {
            return context.leaveApplications
               

                .Where(x => x.employee.EmpId == empId && x.isVoid != true)

                .ToList();
        }
        //Fnx related Count
        public int getVoidRegisterAdministratorCount()
        {
            return context.leaveApplications               
                .Where(x => x.isVoid == true)
                .Count();
        }
        public int getAllPendingForApprovalAdminCount()
        {
            return context.leaveApplications

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getRegisterAdministratorCount()
        {
            return context.leaveApplications
               
                .Where(x => x.isVoid != true)
                .Count();
        }
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.leaveApplications

                .Where(x => /*x.employeeApproval.isApproved == true &&*/ x.PendingForClosing == true /*&& x.employeeApproval.isVoid != true*/)
                .Count();
        }
        public int getAllPendingForReApprovalAdminCount()
        {
            return context.leaveApplications

                .Where(x => x.isReApproved == false && x.isVoid != true)
                .Count();
        }

        public int getAllLeavesForRegister()
        {
            return context.leaveApplications
                .Where(x=>x.isVoid != true)                               
                .Count();
        }

        public int getVoidRegisterOwnCount(int EmpId)
        {
            return context.leaveApplications
                .Where(x => x.isVoid == true &&  x.employeeId == EmpId)
                .Count();
        }
        public int getAllPendingForApprovalOwnCount(int EmpId)
        {
            return context.leaveApplications

                .Where(x => x.isApproved == false && x.employeeId == EmpId && x.isVoid != true)
                .Count();
        }
        public int getRegisterOwnCount(int EmpId)
        {
            return context.leaveApplications

                .Where(x => x.isVoid != true && x.employeeId == EmpId)
                .Count();
        }
        public int getAllPendingForClosingOwnCount(int EmpId)
        {
            return context.leaveApplications

                .Where(x => /*x.employeeApproval.isApproved == true &&*/ x.PendingForClosing == true /*&& x.employeeApproval.isVoid != true*/  && x.employeeId == EmpId)
                .Count();
        }
        public int getAllPendingForReApprovalOwnCount(int EmpId)
        {
            return context.leaveApplications

                .Where(x => x.isReApproved == false && x.employeeId == EmpId && x.isVoid != true)
                .Count();
        }
        public int getAllLeavesForRegister(int EmpId)
        {
            return context.leaveApplications
                 .Where(x =>  x.employeeId == EmpId)

                .Count();
        }
    }
}
