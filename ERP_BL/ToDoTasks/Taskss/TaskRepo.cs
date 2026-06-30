using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ToDoTasks.Taskss
{
    public class TaskRepo
    {
        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Get last Payment
        /// </summary>
        /// <returns></returns>
        public int GetLastTransactionId()
        {
            var task = context.tasks.OrderByDescending(q => q.Id).FirstOrDefault();
            if (task == null)
                return 0;
            return task.SystemId;
        }

        /// <summary>
        /// Add New Task
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Tasks task, int userId)
        {
            task.TaskTrackings = new List<TaskTracking>();
            task.TaskTrackings.Add(new TaskTracking() {
            UpdateDateTime=DateTime.Now,
            UpdatedById=userId,
            Description = "Added New Task!"});
            context.tasks.Add(task);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Task
        /// </summary>
        /// <param name="task"></param>
        public void UpdateTask(Tasks task)
        {
            var _task = context.tasks.FirstOrDefault(x=>x.Id == task.Id);
            _task = task;

            context.SaveChanges();
        }

        /// <summary>
        /// Get All Users by Department and Company Id
        /// </summary>
        /// <param name="departmentID">Department Id</param>
        /// <param name="companyId">Company Id</param>
        /// <returns></returns>
        public List<ERP_BL.Databases.User> getusersByCompanyDepartment(int departmentID, int companyId)
        {

            var u = context.Users
                .Where(x => (x.employee.departments.FirstOrDefault(y => y.Id == departmentID) != null && x.employee.Companies.FirstOrDefault(z => companyId == z.Id) != null || (x.employee.isTaskType == true && x.employee.TaskCompanies.FirstOrDefault(a=>a.Id == companyId) != null)) && x.isActive != false).ToList();
            return u;

        }

        /// <summary>
        /// Get Task
        /// </summary>
        /// <param name="task"></param>
        public Tasks GetTask(int taskId)
        {
            return context.tasks.Include("TaskEfficiencies.efficiencyPoints").FirstOrDefault(x=>x.Id == taskId);
        }

        /// <summary>
        /// Get SO Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetSOTask(int SoId, bool tracking)
        {
            return context.tasks.Where(x => x.saleOrderId == SoId && x.isVoid != true).ToList();
        }
        public Tasks GetSOTask(int SoId)
        {
            return context.tasks.FirstOrDefault(x => x.saleOrderId == SoId && x.isVoid != true);
        }

        /// <summary>
        /// Get SO Task
        /// </summary>
        /// <param name="task"></param>
        public Tasks GetPOTask(int PoId)
        {
            return context.tasks.FirstOrDefault(x => x.purchaseOrderId == PoId && x.isVoid != true);
        } 
        public List<Tasks> GetPOTask(int PoId, bool tracking)
        {
            return context.tasks.Where(x => x.purchaseOrderId == PoId && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get SO Task
        /// </summary>
        /// <param name="task"></param>
        public Tasks GetSITask(int SiId)
        {
            return context.tasks.FirstOrDefault(x => x.saleInvoiceId == SiId && x.isVoid != true);
        }  
        public List<Tasks> GetSITask(int SiId, bool tracking)
        {
            return context.tasks.Where(x => x.saleInvoiceId == SiId && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get Inquiry Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetInquiryTask(int inquiryId)
        {
            return context.tasks.Where(x => x.inquiryId == inquiryId && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get Offer Task
        /// </summary>
        /// <param name="task"></param>
        public Tasks GetOfferTask(int offerId)
        {
            return context.tasks.FirstOrDefault(x => x.offerId == offerId && x.isVoid != true);
        }
        public Tasks GetModuleContractTask(int modulesContractId)
        {
            return context.tasks.FirstOrDefault(x => x.moduleContractId == modulesContractId && x.isVoid != true);
        }
        public List<Tasks> GetOfferTask(int offerId, bool tracking)
        {
            return context.tasks.Where(x => x.offerId == offerId && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllTasksAllowedUsers(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x=> (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid)  && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllTasksDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllTasks()
        {
            

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllOpenTasksAllowedUsers(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => (x.AllowedUsers.FirstOrDefault(y=>y.id == uid) != null || x.creatorId == uid)  && x.isVoid != true && x.Status.isActive == true && x.PendingForClosing != true && x.isApproved != false)
                .ToList();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllOpenTasksDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true && x.Status.isActive == true && x.PendingForClosing != true && x.isApproved != false)
                .ToList();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllOpenTasks()
        {
           
            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x =>x.isVoid != true && x.Status.isActive == true && x.PendingForClosing != true && x.isApproved != false)
                .ToList();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllPendingForApprovalTasksAllowedUsers(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid) && x.isVoid != true && x.isApproved == false && x.Status.isActive == true && x.PendingForClosing != true)
                .ToList();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllPendingForApprovalTasksDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true && x.isApproved == false && x.Status.isActive == true && x.PendingForClosing != true)
                .ToList();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllPendingForApprovalTasks()
        {

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => x.isVoid != true && x.Status.isActive == true && x.isApproved == false && x.PendingForClosing != true)
                .ToList();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllPendingForApprovalTasksAllowedUsersCount(int uid)
        {
            return context.tasks
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid) && x.isVoid != true && x.isApproved == false && x.Status.isActive == true && x.PendingForClosing != true)
                .Count();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllPendingForApprovalTasksDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true && x.isApproved == false && x.Status.isActive == true && x.PendingForClosing != true)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllPendingForClosingTasksCount()
        {

            return context.tasks
                .Where(x => x.isVoid != true && x.PendingForClosing == true && x.Status.isActive == false)
                .Count();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllPendingForApprovalTasksCount()
        {

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => x.isVoid != true && x.Status.isActive == true && x.isApproved == false && x.PendingForClosing != true)
                .Count();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllClosedTasksAllowedUsers(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid)  && x.isVoid != true && x.Status.isActive == false && x.PendingForClosing != true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllClosedTasksDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true && x.Status.isActive == false && x.PendingForClosing != true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllClosedTasks()
        {
           

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => x.isVoid != true && x.Status.isActive == false && x.PendingForClosing != true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllOpenTasksAllowedUsersCount(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid) && x.isVoid != true && x.Status.isActive == true && x.PendingForClosing != true && x.isApproved != false)
                .Count();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllOpenTasksDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true && x.Status.isActive == true && x.PendingForClosing != true && x.isApproved != false)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllOpenTasksCount()
        {
            return context.tasks
                .Where(x => x.isVoid != true && x.Status.isActive == true && x.PendingForClosing != true && x.isApproved != false)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllClosedTasksAllowedUsersCount(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid)  && x.isVoid != true && x.Status.isActive == false && x.PendingForClosing != true)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllClosedTasksDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true && x.Status.isActive == false && x.PendingForClosing != true)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllClosedTasksCount()
        {
            return context.tasks
                .Where(x => x.isVoid != true && x.Status.isActive == false && x.PendingForClosing != true)
                .Count();
        }

        /// <summary>
        /// Get All Closed Tasks Count
        /// </summary>
        /// <param name="task"></param>
        public int GetAllPendingForClosingTasksAllowedUsersCount(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid) && x.isVoid != true && x.PendingForClosing == true && x.Status.isActive == false)
                .Count();
        }

        /// <summary>
        /// Get All Closed Tasks Count
        /// </summary>
        /// <param name="task"></param>
        public int GetAllPendingForClosingTasksDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true && x.PendingForClosing == true && x.Status.isActive == false)
                .Count();
        }



        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllTasksAllowedUsersCount(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid)  && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllTasksDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllTasksCount()
        {
           
            return context.tasks
                .Where(x => x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllVoidTasksAllowedUsersCount(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid) && x.isVoid == true)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllVoidTasksDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid == true)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllVoidTasksCount()
        {

            return context.tasks
                .Where(x => x.isVoid == true)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public int GetAllAssignedToMeTasksCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => x.assignedTo.id == uid && ((x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid) && deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllPendingForClosingTasksAllowedUsers(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid) && x.isVoid != true && x.PendingForClosing == true && x.Status.isActive == false)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllPendingForClosingTasksDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true && x.PendingForClosing == true && x.Status.isActive == false)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllPendingForClosingTasks()
        {

            return context.tasks
                .Where(x => x.isVoid != true && x.PendingForClosing == true && x.Status.isActive == false)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllAssignedToMeTasks(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => x.assignedTo.id == uid && ((x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid) && deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllVoidTasksAllowedUsers(int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> companyIds = new List<int>();
            //companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            //deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            //foreach (var comp in user.employee.Companies)
            //    companyIds.Add(comp.Id);
            //foreach (var dpt in user.employee.departments)
            //    deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => (x.AllowedUsers.FirstOrDefault(y => y.id == uid) != null || x.creatorId == uid)  && x.isVoid == true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllVoidTasksDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => (deptIds.Contains(x.deptId.Value) && companyIds.Contains((int)x.companyId)) && x.isVoid == true)
                .ToList();
        }

        /// <summary>
        /// Get All Task
        /// </summary>
        /// <param name="task"></param>
        public List<Tasks> GetAllVoidTasks()
        {
           
            return context.tasks
                //.Include("supervisedBy")
                //.Include("assignedTo")
                //.Include("creator")
                //.Include("supervisedBy")
                .Where(x => x.isVoid == true)
                .ToList();
        }

        public SaleOrder GetSaleOrder(int saleOrderId)
        {
            return context.saleOrders.FirstOrDefault(x=>x.Id == saleOrderId);
        }

        public PurchaseOrder GetPurchaseOrder(int poId)
        {
            return context.purchaseOrders.FirstOrDefault(x => x.Id == poId);
        }

        public SaleInvoice GetSaleInvoice(int saleInvoiceId)
        {
            return context.saleInvoices.FirstOrDefault(x => x.Id == saleInvoiceId);
        }

        public Inquiry GetInquiry(int inquiryId)
        {
            return context.inquiries.FirstOrDefault(x => x.Id == inquiryId);
        }

        public Offer GetOffer(int offerId)
        {
            return context.offers.FirstOrDefault(x => x.Id == offerId);
        }


        /// <summary>
        /// Get All Loans Statuses
        /// </summary>
        /// <returns></returns>
        public List<TasksStatus> GetAllTaskStatusesForRegister()
        {
            return context.taskStatuses
                .ToList();
        }

        /// <summary>
        /// Get A Loans Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public TasksStatus GetTaskStatus(int statusId)
        {
            return context.taskStatuses
                .FirstOrDefault(x => x.Id == statusId);
        }

        /// <summary>
        /// Get All Payment Statuses
        /// </summary>
        /// <returns></returns>
        public List<TasksStatus> GetAllTaskStatuses()
        {
            return context.taskStatuses
                //.Include("payments")
                .ToList();
        }

        /// <summary>
        /// Get All Active Statuses
        /// </summary>
        /// <returns></returns>
        public List<TasksStatus> GetAllActiveTaskStatuses()
        {
            return context.taskStatuses.Where(x => x.isActive == true)
                //.Include("payments")
                .ToList();
        }

        public List<TasksStatus> GetAllCloseTasksStatus()
        {
            var statusList = context.taskStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }

        /// <summary>
        /// Add New Loans Status
        /// </summary>
        /// <param name="loansStatus"></param>
        public void AddTaskStatus(TasksStatus tasksStatus)
        {
            context.taskStatuses.Add(tasksStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Task Status
        /// </summary>
        /// <param name="loansStatus"></param>
        public void UpdateTaskStatus(TasksStatus taskStatus)
        {
            TasksStatus _tasksStatus = context.taskStatuses.FirstOrDefault(x => x.Id == taskStatus.Id);
            _tasksStatus = taskStatus;
            context.SaveChanges();
        }

        public List<EfficiencyPoints> GetAllEfficiencyPoints()
        {
            return context.efficiencyPoints.ToList();
        }

        public EfficiencyPoints GetEfficiencyPointsById(int id)
        {
            return context.efficiencyPoints.FirstOrDefault(x => x.Id == id);
        }

        public void AddEfficiencyPoints(EfficiencyPoints efficiencyPoints)
        {
            context.efficiencyPoints.Add(efficiencyPoints);
            context.SaveChanges();
        }

        public void UpdateEfficiencyPoints(EfficiencyPoints efficiencyPoints)
        {
            var _efficiencyPoints = context.efficiencyPoints.FirstOrDefault(x=>x.Id == efficiencyPoints.Id);
            context.SaveChanges();
        }

        /// <summary>
        /// Add Task Type
        /// </summary>
        /// <param name="applicantType"></param>
        public void AddTaskType(TaskType type)
        {
            if(type.isProcurementType == false)
            {
                var compIds = type.companies.Select(x => x.Id);
                var deptIds = type.departments.Select(x => x.Id);

                type.companies = new List<Company>();
                foreach (var _id in compIds)
                {
                    type.companies.Add(context.Companies.FirstOrDefault(x => x.Id == _id));
                }

                type.departments = new List<Department>();
                foreach (var _id in deptIds)
                {
                    type.departments.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
                }
            }
            

            context.taskTypes.Add(type);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Task Type
        /// </summary>
        /// <param name="applicant"></param>
        public void UpdateTaskType(TaskType type)
        {
            
            if (type.isProcurementType == false)
            {
                var compIds = type.companies.Select(x => x.Id);
                var deptIds = type.departments.Select(x => x.Id);

                var _type = context.taskTypes.FirstOrDefault(x => x.Id == type.Id);
                _type = type;


                _type.companies = new List<Company>();
                foreach (var _id in compIds)
                {
                    _type.companies.Add(context.Companies.FirstOrDefault(x => x.Id == _id));
                }

                _type.departments = new List<Department>();
                foreach (var _id in deptIds)
                {
                    _type.departments.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
                }
            }
            var typee = context.taskTypes.FirstOrDefault(x => x.Id == type.Id);
            typee = type;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Task Type
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public TaskType GetTaskType(int typeId)
        {
            return context.taskTypes
                .FirstOrDefault(x => x.Id == typeId);
        }

        /// <summary>
        /// Get All Task Type
        /// </summary>
        /// <returns></returns>
        public List<TaskType> GetAllTaskTypes()
        {
            return context.taskTypes
                .ToList();
        }

        /// <summary>
        /// Get All Task Types By Company and Department
        /// </summary>
        /// <returns></returns>
        public List<TaskType> GetTaskTypesCompDept( int compId, int deptId)
        {
            return context.taskTypes
                .Where(x => x.companies.FirstOrDefault(y => y.Id == compId) != null && x.departments.FirstOrDefault(z => z.Id == deptId) != null)
                .ToList();
        }

        /// <summary>
        /// Get Procurement Task Types
        /// </summary>
        /// <returns></returns>
        public List<TaskType> GetProcurementTaskTypes(TransactionItemType type)
        {
            switch (type)
            {
                case TransactionItemType.Sale_Order:
                    return context.taskTypes.Where(x=>x.isSaleOrder == true).ToList();
                case TransactionItemType.Purchase_Order:
                    return context.taskTypes.Where(x => x.isPurchaseOrder == true).ToList();
                case TransactionItemType.Sale_Invoice:
                    return context.taskTypes.Where(x => x.isSaleInvoice == true).ToList();
                case TransactionItemType.Offer:
                    return context.taskTypes.Where(x => x.isOffer == true).ToList();
                case TransactionItemType.Inquiry:
                    return context.taskTypes.Where(x => x.isInquiry == true).ToList();
                default:
                    return null;                       
            }
        }

        /// <summary>
        /// Add Warehouse
        /// </summary>
        /// <param name="vehicleOwner"></param>
        public void AddWarehouse(Warehouse warehouse)
        {
            context.warehouses.Add(warehouse);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Warehouse
        /// </summary>
        /// <param name="vehicleOwner"></param>
        public void UpdateWarehouse(Warehouse warehouse)
        {
            var _warehouse = context.warehouses.FirstOrDefault(x => x.Id == warehouse.Id);
            _warehouse = warehouse;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Warehouse
        /// </summary>
        /// <param name="natureId"></param>
        /// <returns></returns>
        public Warehouse GetWarehouse(int warehouseId)
        {
            return context.warehouses

                .FirstOrDefault(x => x.Id == warehouseId);
        }

        /// <summary>
        /// Get All Rented Vehicle Owner
        /// </summary>
        /// <returns></returns>
        public List<Warehouse> GetAllWarehouse()
        {
            return context.warehouses

                .ToList();
        }

        /// <summary>
        /// Add Packing Style
        /// </summary>
        /// <param name="packingStyle"></param>
        public void AddPackingStyle(PackingStyle packingStyle)
        {
            context.packingStyles.Add(packingStyle);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Packing Style
        /// </summary>
        /// <param name="packingStyle"></param>
        public void UpdatePackingStyle(PackingStyle packingStyle)
        {
            var _packingStyle = context.packingStyles.FirstOrDefault(x => x.Id == packingStyle.Id);
            _packingStyle = packingStyle;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Packing Style
        /// </summary>
        /// <param name="packingStyleId"></param>
        /// <returns></returns>
        public PackingStyle GetPackingStyle(int packingStyleId)
        {
            return context.packingStyles

                .FirstOrDefault(x => x.Id == packingStyleId);
        }

        /// <summary>
        /// Get All Packing Styles
        /// </summary>
        /// <returns></returns>
        public List<PackingStyle> GetAllPackingStyles()
        {
            return context.packingStyles

                .ToList();
        }


        /// <summary>
        /// Add Good Receive Note
        /// </summary>
        /// <param name="goodReceiveNote"></param>
        public void AddGoodReceiveNote(GoodReceiveNote goodReceiveNote)
        {
            context.goodReceiveNotes.Add(goodReceiveNote);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Good Receive Note
        /// </summary>
        /// <param name="goodReceiveNote"></param>
        public void UpdateGoodReceiveNote(GoodReceiveNote goodReceiveNote)
        {
            var _GoodReceiveNote = context.goodReceiveNotes.FirstOrDefault(x => x.Id == goodReceiveNote.Id);
            _GoodReceiveNote = goodReceiveNote;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Good Receive Note
        /// </summary>
        /// <param name="goodReceiveNoteId"></param>
        /// <returns></returns>
        public GoodReceiveNote GetGoodReceiveNote(int goodReceiveNoteId)
        {
            return context.goodReceiveNotes

                .FirstOrDefault(x => x.Id == goodReceiveNoteId);
        }

        /// <summary>
        /// Get All Good Receive Notes
        /// </summary>
        /// <returns></returns>
        public List<GoodReceiveNote> GetAllGoodReceiveNotes()
        {
            return context.goodReceiveNotes.ToList();
        }

        public List<Product> getAllDepartmentProducts(int deptIdId)
        {
            var department = context.Departments.FirstOrDefault(x => x.Id == deptIdId);

            return department.Products.Where(x => x.isActive == true).Distinct().ToList();//context.Products.Where(x => x.departments.Where(y=> deptIds.Contains(y.Id))!=null).ToList();

        }

        /// <summary>
        /// Add New LOT Number
        /// </summary>
        /// <param name="lotNo"></param>
        public void AddLOTnumber(LotNumber lotNo)
        {
            context.lotNumbers.Add(lotNo);
            context.SaveChanges();
        }


        /// <summary>
        /// Update Lot Number
        /// </summary>
        /// <param name="lotNo"></param>
        public void UpdateLotNumber(LotNumber lotNo)
        {
            LotNumber _lotNumber = context.lotNumbers.FirstOrDefault(x => x.Id == lotNo.Id);

            _lotNumber = lotNo;
            context.SaveChanges();
        }



        /// <summary>
        /// Get Lot Number
        /// </summary>
        /// <param name="lotId"></param>
        /// <returns></returns>
        public LotNumber GetLotNumber(int lotId)
        {
            return
                context.lotNumbers

                .FirstOrDefault(x => x.Id == lotId);
        }


        /// <summary>
        /// Get All Active Lot Number
        /// </summary>
        /// <returns></returns>
        public List<LotNumber> GetAllLotNumberForCompDept(int compId, int deptId)
        {
            return
            context.lotNumbers

            .Where(x => x.companyId == compId && x.deptId == deptId && x.isActive == true)
            .ToList();
        }


        /// <summary>
        /// Get All Lot Number
        /// </summary>
        /// <returns></returns>
        public List<LotNumber> GetAllLotNumber()
        {
            return
            context.lotNumbers.ToList();
        }
    }
}
