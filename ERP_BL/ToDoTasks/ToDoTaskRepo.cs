using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ToDoTasks
{
    public class ToDoTaskRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        public List<ERP_BL.Databases.User> getAllusers()
        {
            return context.Users

                .ToList();
        }

        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        public List<ERP_BL.Databases.User> getAllusersByEmpIds(List<int> empIds)
        {
            return context.Users

                .Where(x => empIds.Contains(x.employeeId))
                .ToList();
        }

        /// <summary>
        /// Get All Tasks by Group Id
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllTasksByGroupIdAndUserId(int taskGroupId, int userId)
        {
            return context.toDoTasks

                .Where(x => x.taskGroupId == taskGroupId && x.assignedToUsers.FirstOrDefault(y => y.id == userId) != null && x.parentTaskId == null && x.isVoid != true && x.isBasketed != true)
                .ToList();
        }


        /// <summary>
        /// Get All Tasks Open Tasks
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllOpenTasksByGroupId(int taskGroupId)
        {
            return context.toDoTasks

                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId == null && x.Status.isActive == true && x.isApproved != false && x.isBasketed != true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get All Pending for Approval Tasks
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllPendingForApprovalTasksByGroupId(int taskGroupId)
        {
            return context.toDoTasks

                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId == null && x.isApproved == false && x.isVoid != true && x.isBasketed != true)
                .ToList();
        }

        /// <summary>
        /// Get All Pending for Approval Steps
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllPendingForApprovalStepsByGroupId(int taskGroupId)
        {
            return context.toDoTasks

                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId != null && x.isApproved == false && x.isVoid != true && x.isBasketed != true)
                .ToList();
        }


        /// <summary>
        /// Get All Closed Steps
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllClosedSteps(int uId)
        {
            return context.toDoTasks

                .Where(x => /*x.taskGroupId == taskGroupId && */x.parentTaskId != null && x.ParentTask.Status.isActive == false && x.isBasketed != true && x.isVoid != true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList();
        }

        /// <summary>
        /// Get All Steps Open
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllOpenSteps(int uId)
        {
            return context.toDoTasks

                .Where(x => /*x.taskGroupId == taskGroupId &&*/ x.parentTaskId != null && x.ParentTask.Status.isActive == true && x.isApproved != false && x.isBasketed != true && x.isVoid != true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList();
        }

        /// <summary>
        /// Get All Steps Open
        /// </summary>
        /// <returns></returns>
        public int GetAllOpenStepsCount(int uId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId != null && x.ParentTask.Status.isActive == true && x.isApproved != false && x.isBasketed != true && x.isVoid != true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .Count();
        }

        /// <summary>
        /// Get All Closed Steps count
        /// </summary>
        /// <returns></returns>
        public int GetAllClosedStepsCount(int uId)
        {
            return context.toDoTasks

                .Where(x => x.parentTaskId != null && x.ParentTask.Status.isActive == false && x.isBasketed != true && x.isVoid != true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .Count();
        }

        /// <summary>
        /// Get All Pending for Approval Steps
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllPendingForReApprovalStepsByGroupId(int taskGroupId)
        {
            return context.toDoTasks

                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId != null && x.isReApproved == false && x.isVoid != true && x.isBasketed != true)
                .ToList();
        }

        /// <summary>
        /// Get Count of Group Task Register
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllGroupTasks(int taskGroupId)
        {
            return context.toDoTasks

                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId == null && x.isVoid != true && x.isBasketed != true)
                .ToList();
        }

        /// <summary>
        /// Get Count of Steps Register
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllGroupSteps(int taskGroupId)
        {
            return context.toDoTasks

                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId != null && x.isVoid != true && x.isBasketed != true)
                .ToList();
        }

        /// <summary>
        /// Get Count of Group Task Register
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllTasks(int uId)
        {
            return context.toDoTasks

                .Where(x => x.parentTaskId == null && x.isVoid != true && x.isBasketed != true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList();
        }


        /// <summary>
        /// Get All steps
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllSteps(int uId)
        {
            return context.toDoTasks

                .Where(x => x.parentTaskId != null && x.isVoid != true && x.isBasketed != true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList();
        }

        /// <summary>
        /// Get All Basketed Steps
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllBasketedSteps(int uId)
        {
            return context.toDoTasks

               .Where(x => x.parentTaskId != null && x.isVoid != true && x.isBasketed == true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList();
        }

        /// <summary>
        /// Get All Basketed Steps Count
        /// </summary>
        /// <returns></returns>
        public int GetAllBasketedStepsCount(int uId)
        {
            return context.toDoTasks
               .Where(x => x.parentTaskId != null && x.isVoid != true && x.isBasketed == true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .Count();
        }

        /// <summary>
        /// Get All steps
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllStepsFirst(int uId)
        {
            return context.toDoTasks

                .Where(x => x.parentTaskId != null && x.isVoid != true && x.isBasketed != true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList();
        }


        /// <summary>
        /// Get Count of Steps Register
        /// </summary>
        /// <returns></returns>
        public int GetAllGroupStepsCount(int taskGroupId)
        {
            var count = context.toDoTasks
                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId != null && x.isVoid != true && x.isBasketed != true)
                .Count();
            return count;
        }

        /// <summary>
        /// Get All steps Count
        /// </summary>
        /// <returns></returns>
        public int GetAllStepsCount(int uId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId != null && x.isVoid != true && x.isBasketed != true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .Count();
        }

        /// <summary>
        /// Get All Tasks Open Tasks
        /// </summary>
        /// <returns></returns>
        public int GetAllOpenTasksByGroupIdCount(int taskGroupId)
        {
            var count = context.toDoTasks
                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId == null && x.Status.isActive == true && x.isApproved != false && x.isVoid != true && x.isBasketed != true)
                .ToList().Count();
            return count;
        }

        /// <summary>
        /// Get All Tasks Closed Tasks
        /// </summary>
        /// <returns></returns>
        public int GetAllClosedTasksByGroupIdCount(int taskGroupId)
        {
            return context.toDoTasks
                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId == null && x.Status.isActive == false && x.isVoid != true && x.isBasketed != true)
                .ToList().Count();
        }

        /// <summary>
        /// Get All Pending for Approval Tasks Count
        /// </summary>
        /// <returns></returns>
        public int GetAllPendingForApprovalTasksByGroupIdCount(int taskGroupId)
        {
            return context.toDoTasks
                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId == null && x.isApproved == false && x.isVoid != true && x.isBasketed != true)
                .Count();
        }

        /// <summary>
        /// Get Count of Group Task Register
        /// </summary>
        /// <returns></returns>
        public int GetGroupTasksRegisterByGroupIdCount(int taskGroupId)
        {
            return context.toDoTasks
                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId == null && x.isVoid != true && x.isBasketed != true)
                .ToList().Count();
        }

        /// <summary>
        /// Get Count of Group Task Register
        /// </summary>
        /// <returns></returns>
        public int GetTasksRegisterCount(int uId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId == null && x.isVoid != true && x.isBasketed != true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList().Count();
        }

        /// <summary>
        /// Get All Basketed Tasks
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetBasketedTasks(int uId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId == null && x.isVoid != true && x.isBasketed == true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList();
        }

        /// <summary>
        /// Get All Basketed Tasks Count
        /// </summary>
        /// <returns></returns>
        public int GetBasketedTasksCount(int uId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId == null && x.isVoid != true && x.isBasketed == true && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList().Count();
        }


        /// <summary>
        /// Get Count of Void Tasks
        /// </summary>
        /// <returns></returns>
        public int GetVoidTasksCount(int uId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId == null && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid == true)
                .ToList().Count();
        }


        /// <summary>
        /// Get All Pending for Approval Tasks Count
        /// </summary>
        /// <returns></returns>
        public int GetPendingForApprovalStepCountForRegister(int taskGroupId)
        {
            return context.toDoTasks
                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId != null && x.isApproved == false && x.isVoid != true && x.isBasketed != true)
                .Count();
        }


        /// <summary>
        /// Get All Pending for ReApproval Tasks Count
        /// </summary>
        /// <returns></returns>
        public int GetPendingForReApprovalStepCountForRegister(int taskGroupId)
        {
            return context.toDoTasks
                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId != null && x.isReApproved == false && x.isVoid != true && x.isBasketed != true)
                .Count();
        }


        /// <summary>
        /// Get Step Count
        /// </summary>
        /// <returns></returns>
        public int GetPendingForApprovalStepCount(int taskId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId == taskId && x.isApproved == false && x.isBasketed != true)
                .ToList().Count();
        }

        /// <summary>
        /// Get Step Count
        /// </summary>
        /// <returns></returns>
        public int GetPendingForReApprovalStepCount(int taskId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId == taskId && x.isReApproved == false && x.isBasketed != true)
                .ToList().Count();
        }

        /// <summary>
        /// Get Void Step Count
        /// </summary>
        /// <returns></returns>
        public int GetVoidStepsCount(int taskId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId == taskId && x.isVoid == true)
                .ToList().Count();
        }


        /// <summary>
        /// Get All Tasks Closed Tasks
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllClosedTasksByGroupId(int taskGroupId)
        {
            return context.toDoTasks

                .Where(x => x.taskGroupId == taskGroupId && x.parentTaskId == null && x.Status.isActive == false && x.isVoid != true && x.isBasketed != true)
                .ToList();
        }

        /// <summary>
        /// Get All Tasks Closed Tasks
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllVoidTasks(int uId)
        {
            return context.toDoTasks

                .Where(x => x.parentTaskId == null && (x.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid == true)
                .ToList();
        }

        /// <summary>
        /// Get Approved Steps by Parent Id
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllStepsByParentId(int parentId)
        {
            return context.toDoTasks

                .Where(x => x.parentTaskId == parentId)
                .ToList();
        }


        /// <summary>
        /// Get All Steps Under Approval
        /// </summary>
        /// <returns></returns>
        public List<ToDoTask> GetAllPendingStepsByParentId(int parentId)
        {
            return context.toDoTasks
                .Where(x => x.parentTaskId == parentId && x.isApproved == false)
                .ToList();
        }

        /// <summary>
        /// Get All Tasks by Group Id
        /// </summary>
        /// <returns></returns>
        public double SumOfCompletedSteps(int taskId)
        {
            var tasks = context.toDoTasks
                .Where(x => x.parentTaskId == taskId && x.isCompleted == true).ToList();
            if (tasks.Count > 0)
            {
                var sum = tasks.Sum(x => x.TaskPoints);
                return sum;
            }
            else
                return 0;

        }

        /// <summary>
        /// Get All Tasks by Group Id
        /// </summary>
        /// <returns></returns>
        public double CalculateStepPoints(int taskId)
        {
            var task = context.toDoTasks.FirstOrDefault(x => x.Id == taskId);
            var tasks = context.toDoTasks.Where(x => x.parentTaskId == taskId).ToList();
            if (tasks.Count > 0)
            {
                var sum = tasks.Sum(x => x.TaskPoints);
                return task.TaskPoints - sum;
            }
            else
            {
                return task.TaskPoints;
            }
        }


        public void AddThemeColor(ToDoTaskTheme toDoTaskTheme)
        {
            var removeThemes = context.toDoTaskThemes.ToList();
            context.toDoTaskThemes.RemoveRange(removeThemes);

            context.toDoTaskThemes.Add(toDoTaskTheme);
            context.SaveChanges();
        }

        public ToDoTaskTheme getTaskTheme()
        {
            var theme = context.toDoTaskThemes.ToList().LastOrDefault();
            if (theme == null)
            {
                return null;
            }
            else
            {
                return theme;
            }
        }


        /// <summary>
        /// Delete Step
        /// </summary>
        /// <returns></returns>
        public void DeleteStep(int stepId)
        {
            var step = context.toDoTasks.FirstOrDefault(x => x.Id == stepId);
            context.toDoTasks.Remove(step);
            context.SaveChanges();
        }

        /// <summary>
        /// Add new Task
        /// </summary>
        /// <returns></returns>
        public void AddTask(ToDoTask task)
        {
            context.toDoTasks.Add(task);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Task
        /// </summary>
        /// <returns></returns>
        public void UpdateTask(ToDoTask task, List<ToDoTask> toDoTasks)
        {
            ToDoTask _task = context.toDoTasks.FirstOrDefault(x => x.Id == task.Id);
            _task = task;

            if (toDoTasks != null && toDoTasks.Count > 0)
                foreach (var _item in toDoTasks)
                {
                    ToDoTask _toDoTask = context.toDoTasks.FirstOrDefault(x => x.Id == _item.Id);
                    _toDoTask = _item;
                }

            context.SaveChanges();
        }

        /// <summary>
        /// Make a Copy of Task
        /// </summary>
        /// <returns></returns>
        public void CopyTask(ToDoTask task, List<ToDoTask> toDoTasks)
        {
            task.creationDate = DateTime.Now;
            task.isApproved = false;
            task.ApprovedDate = null;
            task.isReApproved = null;
            task.ReApprovalDate = null;
            task.isBasketed = true;

            context.toDoTasks.Add(task);

            if (toDoTasks != null && toDoTasks.Count > 0)
                foreach (var _item in toDoTasks)
                {
                    _item.isBasketed = true;
                    _item.parentTaskId = task.Id;
                    context.toDoTasks.Add(_item);
                }

            context.SaveChanges();
        }

        /// <summary>
        /// Update Step
        /// </summary>
        /// <returns></returns>
        public void UpdateStep(ToDoTask task)
        {
            ToDoTask _task = context.toDoTasks.FirstOrDefault(x => x.Id == task.Id);
            _task = task;
            context.SaveChanges();
        }


        /// <summary>
        /// Get To Do Task by Task Id
        /// </summary>
        /// <returns></returns>
        public ToDoTask GetTask(int taskId)
        {
            return context.toDoTasks

                .FirstOrDefault(x => x.Id == taskId);
        }

        /// <summary>
        /// Get To Do Task by Task Id
        /// </summary>
        /// <returns></returns>
        public ToDoTask GetTaskByGroupAndType(int groupId, int typeId, DateTime? targetYear)
        {
            return context.toDoTasks

                .FirstOrDefault(x => x.taskGroupId == groupId && x.targetTypeId == typeId && x.TargetYear == targetYear);
        }

        /// <summary>
        /// Add new group
        /// </summary>
        /// <returns></returns>
        public void AddTaskGroup(TaskGroups taskGroup)
        {
            var userIds = taskGroup.users.Select(x => x.id);
            if (taskGroup.isTitle == false)
            {
                var compIds = taskGroup.Companies?.Select(x => x.Id).ToList();
                var deptIds = taskGroup.Departments?.Select(x => x.Id).ToList();


                taskGroup.Companies = new List<Company>();
                if (compIds != null)
                    foreach (var _id in compIds)
                    {
                        taskGroup.Companies.Add(context.Companies.FirstOrDefault(x => x.Id == _id));
                    }

                taskGroup.Departments = new List<Department>();
                if (deptIds != null)
                    foreach (var _id in deptIds)
                    {
                        taskGroup.Departments.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
                    }

            }


            taskGroup.users = new List<ERP_BL.Databases.User>();
            foreach (var _id in userIds)
            {
                taskGroup.users.Add(context.Users.FirstOrDefault(x => x.id == _id));
            }

            var userIdsBulk = taskGroup.usersBulk.Select(x => x.id);

            if (taskGroup.isTitle == false)
            {
                var compIdsBulk = taskGroup.CompaniesBulk?.Select(x => x.Id).ToList();
                var deptIdsBulk = taskGroup.DepartmentsBulk?.Select(x => x.Id).ToList();


                taskGroup.CompaniesBulk = new List<Company>();
                if (compIdsBulk != null)
                    foreach (var _id in compIdsBulk)
                    {
                        taskGroup.CompaniesBulk.Add(context.Companies.FirstOrDefault(x => x.Id == _id));
                    }

                taskGroup.DepartmentsBulk = new List<Department>();
                if (deptIdsBulk != null)
                    foreach (var _id in deptIdsBulk)
                    {
                        taskGroup.DepartmentsBulk.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
                    }
            }

            taskGroup.usersBulk = new List<ERP_BL.Databases.User>();
            foreach (var _id in userIdsBulk)
            {
                taskGroup.usersBulk.Add(context.Users.FirstOrDefault(x => x.id == _id));
            }

            context.taskGroups.Add(taskGroup);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Group
        /// </summary>
        /// <returns></returns>
        public void UpdateTaskGroup(TaskGroups taskGroup)
        {
            var userIds = taskGroup.users.Select(x => x.id);
            var userIdsBulk = taskGroup.usersBulk.Select(x => x.id);


            TaskGroups _taskGroup = context.taskGroups.FirstOrDefault(x => x.Id == taskGroup.Id);
            _taskGroup = taskGroup;

            if (taskGroup.isTitle == false)
            {
                var compIds = taskGroup.Companies.Select(x => x.Id);
                var deptIds = taskGroup.Departments.Select(x => x.Id);
                _taskGroup.Companies = new List<Company>();
                foreach (var _id in compIds)
                {
                    _taskGroup.Companies.Add(context.Companies.FirstOrDefault(x => x.Id == _id));
                }

                _taskGroup.Departments = new List<Department>();
                foreach (var _id in deptIds)
                {
                    _taskGroup.Departments.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
                }
            }


            _taskGroup.users = new List<ERP_BL.Databases.User>();
            foreach (var _id in userIds)
            {
                _taskGroup.users.Add(context.Users.FirstOrDefault(x => x.id == _id));
            }

            if (taskGroup.isTitle == false)
            {
                var compIdsBulk = taskGroup.CompaniesBulk.Select(x => x.Id);
                var deptIdsBulk = taskGroup.DepartmentsBulk.Select(x => x.Id);
                _taskGroup.CompaniesBulk = new List<Company>();
                foreach (var _id in compIdsBulk)
                {
                    _taskGroup.CompaniesBulk.Add(context.Companies.FirstOrDefault(x => x.Id == _id));
                }

                _taskGroup.DepartmentsBulk = new List<Department>();
                foreach (var _id in deptIdsBulk)
                {
                    _taskGroup.DepartmentsBulk.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
                }
            }


            _taskGroup.usersBulk = new List<ERP_BL.Databases.User>();
            foreach (var _id in userIdsBulk)
            {
                _taskGroup.usersBulk.Add(context.Users.FirstOrDefault(x => x.id == _id));
            }


            context.SaveChanges();
        }


        /// <summary>
        /// Delete Group
        /// </summary>
        /// <returns></returns>
        public void DeleteTaskGroup(TaskGroups TaskGroup)
        {
            TaskGroups _taskGroup = context.taskGroups.Remove(TaskGroup);
            var taskss = context.toDoTasks.Where(x => x.taskGroupId == TaskGroup.Id);

            context.toDoTasks.RemoveRange(taskss);
            context.SaveChanges();
        }

        /// <summary>
        /// Get Group by Id
        /// </summary>
        /// <returns></returns>
        public TaskGroups GetTaskGroup(int groupId)
        {
            return context.taskGroups

                .FirstOrDefault(x => x.Id == groupId);
        }

        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        public List<TaskGroups> GetAllTaskGroups()
        {
            return context.taskGroups
              .Where(x => x.isBackground == false)
                .ToList();
        }

        public List<TaskGroups> GetAllBackgroundTaskGroups()
        {
            return context.taskGroups
                .Where(x => x.isBackground == true)
                .ToList();
        }

        /// <summary>
        /// Get All groups by Creator Id
        /// </summary>
        /// <returns></returns>
        public List<TaskGroups> GetAllTaskGroupsByUser(int userId)
        {
            var groups = context.taskGroups

                .Where(x => (/*x.usersBulk != null && x.usersBulk.Count > 0 &&*/ x.usersBulk.FirstOrDefault(y => y.id == userId) != null || /*x.usersBulk == null  &&*/ x.users.FirstOrDefault(y => y.id == userId) != null) && x.isBackground == false)
                .ToList();
            if (groups.Count > 0)
            {
                return groups;
            }
            else
            {
                return null;
            }

        }


        public void AddTargetType(TaskTargetType targetType)
        {
            context.taskTargetTypes.Add(targetType);
            context.SaveChanges();
        }

        public void UpdateTargetType(TaskTargetType TargetType)
        {
            var _TargetType = context.taskTargetTypes.FirstOrDefault(x => x.Id == TargetType.Id);
            _TargetType = TargetType;
            context.SaveChanges();
        }

        public TaskTargetType GetTargetType(int typeId)
        {
            return context.taskTargetTypes
                .FirstOrDefault(x => x.Id == typeId);
        }

        public List<TaskTargetType> GetAllTargetTypes()
        {
            return context.taskTargetTypes.ToList();
        }


        public void AddTargetRewardNature(TargetRewardNature rewardNature)
        {
            context.targetRewardNatures.Add(rewardNature);
            context.SaveChanges();
        }

        public void UpdateTargetRewardNature(TargetRewardNature rewardNature)
        {
            var _rewardNature = context.targetRewardNatures.FirstOrDefault(x => x.Id == rewardNature.Id);
            _rewardNature = rewardNature;
            context.SaveChanges();
        }

        public TargetRewardNature GetTargetRewardNature(int rewardNatureId)
        {
            return context.targetRewardNatures
                .FirstOrDefault(x => x.Id == rewardNatureId);
        }

        public List<TargetRewardNature> GetAllTargetRewardNatures()
        {
            return context.targetRewardNatures.ToList();
        }


        public void AddTargetGroup(TargetGroup targetGroup)
        {
            context.targetGroups.Add(targetGroup);
            context.SaveChanges();
        }

        public void UpdateTargetGroup(TargetGroup TargetGroup)
        {
            var _TargetGroup = context.targetGroups.FirstOrDefault(x => x.Id == TargetGroup.Id);
            _TargetGroup = TargetGroup;
            context.SaveChanges();
        }

        public TargetGroup GetTargetGroup(int groupId)
        {
            return context.targetGroups
                .FirstOrDefault(x => x.Id == groupId);
        }

        public List<TargetGroup> GetAllTargetGroups()
        {
            return context.targetGroups.ToList();
        }

        /// <summary>
        /// Get All Loans Statuses
        /// </summary>
        /// <returns></returns>
        public List<ToDoTaskStatus> GetAllTaskStatusesForRegister()
        {
            return context.toDoTaskStatuses
                .ToList();
        }

        /// <summary>
        /// Get All Loans Statuses
        /// </summary>
        /// <returns></returns>
        public List<ToDoTaskStatus> GetAllTaskStatuses()
        {
            return context.toDoTaskStatuses
                .Where(x => x.forTask == true)
                .ToList();
        }

        /// <summary>
        /// Get All Loans Statuses
        /// </summary>
        /// <returns></returns>
        public List<ToDoTaskStatus> GetAllTaskActiveStatuses()
        {
            return context.toDoTaskStatuses
                .Where(x => x.forTask == true && x.isActive == true)
                .ToList();
        }

        /// <summary>
        /// Get All Loans Statuses
        /// </summary>
        /// <returns></returns>
        public List<ToDoTaskStatus> GetAllStepStatuses()
        {
            return context.toDoTaskStatuses
                .Where(x => x.forStep == true)
                .ToList();
        }

        /// <summary>
        /// Get A Loans Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public ToDoTaskStatus GetTaskStatus(int statusId)
        {
            return context.toDoTaskStatuses
                .FirstOrDefault(x => x.Id == statusId);
        }

        /// <summary>
        /// Get A Loans Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public ToDoTaskStatus GetTaskStatusByPercentage(double percentage)
        {
            return context.toDoTaskStatuses
                .FirstOrDefault(x => x.MinPercentage <= percentage && x.MaxPercentage >= percentage && x.forStep == true);
        }

        /// <summary>
        /// Add New Loans Status
        /// </summary>
        /// <param name="loansStatus"></param>
        public void AddTaskStatus(ToDoTaskStatus loansStatus)
        {
            context.toDoTaskStatuses.Add(loansStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Loans Status
        /// </summary>
        /// <param name="loansStatus"></param>
        public void UpdateTaskStatus(ToDoTaskStatus loansStatus)
        {
            ToDoTaskStatus _loansStatus = context.toDoTaskStatuses.FirstOrDefault(x => x.Id == loansStatus.Id);
            _loansStatus = loansStatus;
            context.SaveChanges();
        }


        public ERP_BL.Databases.Employee GetEmployeeForTasks(int empID)
        {
            return context.Employees
                //.Include("person")


                //.Include("Companies")
                //.Include("departments.customers")
                //.Include("Companies.departments")
                //.Include("Companies.departments.parentDepartment")
                //.Include("departments")
                //.Include("departments.Vendors")                
                //.Include("departments.parentDepartment")
                .FirstOrDefault(x => x.EmpId == empID);
        }

        public bool CheckParent(int id)
        {
            var task = context.taskGroups.FirstOrDefault(x => x.parentId == id && x.isBackground == false);
            if (task == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Get all Active saleOrders except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<SaleOrder> getDepartmentalSOByDates(List<Company> companies, List<Department> departments, DateTime fromDate, DateTime toDate)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            //List<int> deptIds = new List<int>();
            //List<int> compIds = new List<int>();

            //foreach (var dpt in departments)
            var deptIds = departments.Select(x => x.Id).ToList();

            //foreach (var comp in companies)
            var compIds = companies.Select(x => x.Id).ToList();

            return context.saleOrders
                .Where(x => deptIds.Contains(x.dept_Id) && compIds.Contains(x.company_Id.Value) && x.CreationDate >= fromDate && x.CreationDate <= toDate /*(x.CreationDate.Value.Day >= fromDate.Day && x.CreationDate.Value.Month >= fromDate.Month && x.CreationDate.Value.Year >= fromDate.Year )*/
                /*&& (x.CreationDate.Value.Day <= toDate.Day && x.CreationDate.Value.Month <= toDate.Month && x.CreationDate.Value.Year <= toDate.Year)*/ && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get All Status Calculation Types
        /// </summary>
        /// <returns></returns>
        public List<StatusCalculationType> GetAllStatusCalculationTypes()
        {
            return context.statusCalculationTypes
                .ToList();
        }

        /// <summary>
        /// Get A Status Calculation Type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public StatusCalculationType GetStatusCalculationType(int typeId)
        {
            return context.statusCalculationTypes
                .FirstOrDefault(x => x.Id == typeId);
        }

        /// <summary>
        /// Add New Status Calculation Type
        /// </summary>
        /// <param name="statusCalculationType"></param>
        public void AddStatusCalculationType(StatusCalculationType statusCalculationType)
        {
            context.statusCalculationTypes.Add(statusCalculationType);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Status Calculation Type
        /// </summary>
        /// <param name="statusCalculationType"></param>
        public void UpdateStatusCalculationType(StatusCalculationType statusCalculationType)
        {
            StatusCalculationType _statusCalculationType = context.statusCalculationTypes.FirstOrDefault(x => x.Id == statusCalculationType.Id);
            _statusCalculationType = statusCalculationType;
            context.SaveChanges();
        }

        public bool CheckStatusPercentage(int[] percentages, int statusId)
        {
            var statuses = context.toDoTaskStatuses.ToList().Where(x => x.forStep == true && x.Id != statusId).ToList();

            foreach (var _status in statuses)
            {
                var intersection = Enumerable.Range(Convert.ToInt16(_status.MinPercentage), Convert.ToInt16((_status.MaxPercentage - _status.MinPercentage) + 1)).ToArray().Intersect(percentages).ToArray();
                if (intersection != null && intersection.Count() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public List<TargetRewards> GetAllRewards(int uId)
        {
            return context.targetRewards.Where(x =>
           (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true).ToList();
        }

        public List<TargetRewards> GetAllUnAppliedRewards(int uId)
        {
            return context.targetRewards.Where(x =>
           (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true && x.isApplied == false).ToList();
        }

        public List<TargetRewards> GetAllSalesRewards(int uId)
        {
            return context.targetRewards.Where(x =>
           (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true && x.functionType == Enums.FunctionType.Sales && x.isApplied == false).ToList();
        }

        public List<TargetRewards> GetAllFinanceRewards(int uId)
        {
            return context.targetRewards.Where(x =>
           (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true && x.functionType == Enums.FunctionType.Finance && x.isApplied == false).ToList();
        }

        public List<TargetRewards> GetAllOtherRewards(int uId)
        {
            return context.targetRewards.Where(x =>
           (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true && x.functionType == Enums.FunctionType.Other && x.isApplied == false).ToList();
        }

        public List<TargetRewards> GetAllAchievedRewards(int uId)
        {
            return context.targetRewards.Where(x =>
            x.toDoTask != null && x.isApplied == false && x.toDoTask.Status != null && x.toDoTask.Status.MinPercentage == 100 && (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true)
            .ToList();
        }

        public List<TargetRewards> GetAllAppliedRewards(int uId)
        {
            return context.targetRewards.Where(x =>
           (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true && x.isApplied == true).ToList();
        }

        public List<TargetRewards> GetAllAppliedSalesRewards(int uId)
        {
            return context.targetRewards.Where(x =>
           (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true && x.functionType == Enums.FunctionType.Sales && x.isApplied == true).ToList();
        }

        public List<TargetRewards> GetAllAppliedFinanceRewards(int uId)
        {
            return context.targetRewards.Where(x =>
           (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true && x.functionType == Enums.FunctionType.Finance && x.isApplied == true).ToList();
        }

        public List<TargetRewards> GetAllAppliedOtherRewards(int uId)
        {
            return context.targetRewards.Where(x =>
           (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true && x.functionType == Enums.FunctionType.Other && x.isApplied == true).ToList();
        }

        public List<TargetRewards> GetAllAppliedAchievedRewards(int uId)
        {
            return context.targetRewards.Where(x =>
            x.toDoTask != null && x.isApplied == true && x.toDoTask.Status != null && x.toDoTask.Status.MinPercentage == 100 && (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid != true)
            .ToList();
        }

        public List<TargetRewards> GetAllVoidRewards(int uId)
        {
            return context.targetRewards.Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid == true)
            .ToList();
        }

        public List<TargetRewards> GetRewardsOnGroupCurrency(int taskGroupId, int currId)
        {
            var TODO = context.targetRewards.Where(x => x.currencyId == currId)
            .ToList();


            return context.targetRewards.Where(x =>
            x.toDoTask != null && x.toDoTask.taskGroup.Id == taskGroupId && x.currencyId == currId)
            .ToList();
        }

        public TargetRewards GetTargetReward(int id)
        {
            return context.targetRewards.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateTargetReward(TargetRewards targetReward)
        {
            var _targetReward = context.targetRewards.FirstOrDefault(x => x.Id == targetReward.Id);
            _targetReward = targetReward;
            context.SaveChanges();
        }

        /// <summary>
        /// Get all currencies as list
        /// </summary>
        /// <returns></returns>
        public List<Currency> getAllCurrencies()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Currencies");

            return context.currencies.Where(x => x.isVoid != true).ToList();
        }



        /// <summary>
        /// Add New Target Reward Status
        /// </summary>
        /// <param name="loansAdvanceStatus"></param>
        public void AddTargetRewardStatus(TargetRewardStatus targetRewardStatus)
        {
            context.targetRewardStatuses.Add(targetRewardStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Target Reward Status
        /// </summary>
        /// <param name="targetRewardStatus"></param>
        public void UpdateTargetRewardStatus(TargetRewardStatus targetRewardStatus)
        {
            TargetRewardStatus _targetRewardStatus = context.targetRewardStatuses.FirstOrDefault(x => x.Id == targetRewardStatus.Id);
            _targetRewardStatus.Status = targetRewardStatus.Status;
            _targetRewardStatus.isActive = targetRewardStatus.isActive;
            _targetRewardStatus.forecolor = targetRewardStatus.forecolor;
            _targetRewardStatus.backcolor = targetRewardStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A Target Reward Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public TargetRewardStatus GetTargetRewardStatus(int statusId)
        {
            return context.targetRewardStatuses
                //.Include("payments")
                .FirstOrDefault(x => x.Id == statusId);
        }


        /// <summary>
        /// Get All Target Reward Statuses
        /// </summary>
        /// <returns></returns>
        public List<TargetRewardStatus> GetAllTargetRewardStatuses()
        {
            return context.targetRewardStatuses
                //.Include("payments")
                .ToList();
        }

        public List<TargetRewardStatus> GetAllClosedStatus()
        {
            var statusList = context.targetRewardStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }

        public List<TargetRewardStatus> GetAllOpenStatus()
        {
            var statusList = context.targetRewardStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }

        /// <summary>
        /// Get Count of Target Reward Register
        /// </summary>
        /// <returns></returns>
        public int GetRewardsRegisterCount(int uId)
        {
            return context.targetRewards
                .Where(x => x.isVoid != true && (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList().Count();
        }


        /// <summary>
        /// Get Count of Void Rewards
        /// </summary>
        /// <returns></returns>
        public int GetVoidRewardsCount(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid == true)
                .ToList().Count();
        }

        /// <summary>
        /// Get All Pending for Approval Rewards Count
        /// </summary>
        /// <returns></returns>
        public int GetPendingForApprovalRewardsCount(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get All Pending for ReApproval Rewards Count
        /// </summary>
        /// <returns></returns>
        public int GetPendingForReApprovalRewardsCount(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isReApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count pending for closing Rewards <paramref name="uid"/>.
        /// </summary>
        /// <returns></returns>
        public int GetPendingForClosingCount(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isApproved == false && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count of Target Reward Register
        /// </summary>
        /// <returns></returns>
        public List<TargetRewards> GetAllRewardsForRegister(int uId)
        {
            return context.targetRewards
                .Where(x => x.isVoid != true && (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null))
                .ToList();
        }


        /// <summary>
        /// Get Count of Void Rewards
        /// </summary>
        /// <returns></returns>
        public List<TargetRewards> GetVoidRewards(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isVoid == true)
                .ToList();
        }

        /// <summary>
        /// Get All Pending for Approval Rewards Count
        /// </summary>
        /// <returns></returns>
        public List<TargetRewards> GetPendingForApprovalRewards(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isApproved == false && x.isVoid != true).ToList();
        }


        /// <summary>
        /// Get All Pending for ReApproval Rewards Count
        /// </summary>
        /// <returns></returns>
        public List<TargetRewards> GetPendingForReApprovalRewards(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.isReApproved == false && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get Count pending for closing Rewards <paramref name="uid"/>.
        /// </summary>
        /// <returns></returns>
        public List<TargetRewards> GetPendingForClosing(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.Status.isActive == false && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get Closed Rewards <paramref name="uid"/>.
        /// </summary>
        /// <returns></returns>
        public List<TargetRewards> GetAllClosedRewards(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.Status.isActive == false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true).ToList();
        }

        /// <summary>
        /// Get Open Rewards <paramref name="uid"/>.
        /// </summary>
        /// <returns></returns>
        public List<TargetRewards> GetAllOpenRewards(int uId)
        {
            return context.targetRewards
                .Where(x => (x.toDoTask.taskGroup.usersBulk.FirstOrDefault(y => y.id == uId) != null || x.toDoTask.taskGroup.users.FirstOrDefault(y => y.id == uId) != null) && x.Status.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true).ToList();
        }
    }
}
