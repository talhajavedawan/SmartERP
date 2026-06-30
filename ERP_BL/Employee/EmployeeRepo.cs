using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class EmployeeRepo
    {
        DBContextERP context = new DBContextERP();

        public void addEmployee(Employee employee)
        {

            var fn = context.functions.FirstOrDefault(x => x.Id == employee.empFunction.Id);
            if (fn != null)
            {
                employee.empFunction = fn;
            }
            //if (employee.contact != null)
            //{
            //    var con = context.Contacts.FirstOrDefault(x => x.Id == employee.contact.Id);
            //    if (con != null)
            //    {
            //        employee.contact = con;
            //    }
            //}
            if (employee.Desig != null)
            {
                var desig = context.Designations.FirstOrDefault(x => x.DesigId == employee.Desig.DesigId);
                if (desig != null)
                {
                    employee.Desig = desig;
                }
            }

            if (employee.employeeStatus != null)
            {
                var status = context.employeeStatuses.FirstOrDefault(x => x.Id == employee.employeeStatus.Id);
                if (status != null)
                {
                    employee.employeeStatus = status;
                }

            }
            List<Department> depList = new List<Department>();
            if (employee.departments != null)
            {
                foreach (var emp in employee.departments)
                {
                    var dept = context.Departments.FirstOrDefault(x => x.Id == emp.Id);
                    depList.Add(dept);

                }
                employee.departments = depList;
            }

            List<Company> compList = new List<Company>();
            if (employee.Companies != null)
            {
                foreach (var cmp in employee.Companies)
                {
                    var company = context.Companies.FirstOrDefault(x => x.Id == cmp.Id);
                    compList.Add(company);

                }
                employee.Companies = compList;
            }

            List<Company> adminBillCompList = new List<Company>();
            if (employee.AdminBillCompanies != null)
            {
                foreach (var cmp in employee.AdminBillCompanies)
                {
                    var company = context.Companies.FirstOrDefault(x => x.Id == cmp.Id);
                    adminBillCompList.Add(company);

                }
                employee.AdminBillCompanies = adminBillCompList;
            }

            context.Employees.Add(employee);
            context.SaveChanges();


        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ERP_BL.Databases.Company> GetCompanies()
        {
            return context.Companies
                .Where(s => s.compnayType == Enums.CompnayTypes.Company)
                .ToList();
        }
        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetDepartments()
        {
            return context.Departments.ToList();
        }
        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetUserDepartments(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.departments;
        }

        public List<ERP_BL.Databases.Department> GetUserDepartmentsforUpdate(int loggedInId, int userId)
        {
            List<ERP_BL.Databases.Department> depList = new List<Department>();
            var user = context.Users
                .FirstOrDefault(x => x.id == loggedInId);

            var user2 = context.Employees
                .FirstOrDefault(x => x.EmpId == userId);
            if (user.employee.departments != null)
            {
                foreach (var dep in user.employee.departments)
                {
                    depList.Add(dep);
                }
            }

            if (user2.departments != null)
            {
                foreach (var dep in user2.departments)
                {
                    if (!depList.Contains(dep))
                    { depList.Add(dep); }
                }
            }

            return depList;
        }
        /// <summary>
        /// Get all Companies
        /// </summary>
        /// <returns>List of Companies Objects</returns>
        public List<ERP_BL.Databases.Company> GetUserCompanies(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.Companies;
        }

        public void UpdateMultipleEmployees(List<Employee> employees)
        {
            try
            {
                if (employees != null && employees.Count > 0)
                {
                    foreach (Employee _emp in employees)
                    {
                        if (_emp.EmpId > 0)
                        {
                            var employee = context.Employees.FirstOrDefault(x => x.EmpId == _emp.EmpId);

                            employee = _emp;
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        
        public void updateEmployee(Employee employee)
        {
            Employee empToUpdate = context.Employees.FirstOrDefault(x => x.EmpId == employee.EmpId);

            empToUpdate = employee;

            if (empToUpdate.empFunction != null)
            {
                var fn = context.functions.FirstOrDefault(x => x.Id == employee.empFunction.Id);
                if (fn != null)
                    empToUpdate.empFunction = fn;
            }

            if (empToUpdate.employeeStatus != null)
            {
                var status = context.employeeStatuses.FirstOrDefault(x => x.Id == employee.employeeStatus.Id);
                if (status != null)
                {
                    empToUpdate.employeeStatus = status;
                }
            }

            if (employee.Desig != null)
            {
                var des = context.Designations.FirstOrDefault(x => x.DesigId == employee.Desig.DesigId);
                if (des != null)
                {
                    empToUpdate.Desig = des;
                }
            }

            if (employee.employeeApproval != null)
            {
                var app = context.employeeApprovals.FirstOrDefault(x => x.Id == employee.employeeApproval.Id);
                if (app != null)
                {
                    empToUpdate.employeeApproval = app;
                }
            }

            var qualifications = context.qualifications.FirstOrDefault(x => x.employeeId == employee.EmpId);
            var experience = context.employeeWorkExperiences.Where(x => x.employeeId == employee.EmpId)
                .ToList();
            empToUpdate.WorkExperience = experience;

            List<Department> depList = new List<Department>();
            if (employee.departments != null)
            {
                foreach (var emp in employee.departments)
                {
                    var dept = context.Departments.FirstOrDefault(x => x.Id == emp.Id);
                    depList.Add(dept);

                }
                empToUpdate.departments = depList;
            }

            List<Company> compList = new List<Company>();
            if (employee.Companies != null)
            {
                foreach (var cmp in employee.Companies)
                {
                    var company = context.Companies.FirstOrDefault(x => x.Id == cmp.Id);
                    compList.Add(company);

                }
                empToUpdate.Companies = compList;
            }

            List<Company> adminBillCompList = new List<Company>();
            if (employee.AdminBillCompanies != null)
            {
                foreach (var cmp in employee.AdminBillCompanies)
                {
                    var company = context.Companies.FirstOrDefault(x => x.Id == cmp.Id);
                    adminBillCompList.Add(company);

                }
                empToUpdate.AdminBillCompanies = adminBillCompList;
            }

            List<Company> taskCompanies = new List<Company>();
            if (employee.TaskCompanies != null)
            {
                foreach (var cmp in employee.TaskCompanies)
                {
                    var company = context.Companies.FirstOrDefault(x => x.Id == cmp.Id);
                    taskCompanies.Add(company);

                }
                empToUpdate.TaskCompanies = taskCompanies;
            }

            List<Company> pettyCashCompanies = new List<Company>();
            if (employee.PettyCashCompanies != null)
            {
                foreach (var cmp in employee.PettyCashCompanies)
                {
                    var company = context.Companies.FirstOrDefault(x => x.Id == cmp.Id);
                    pettyCashCompanies.Add(company);

                }
                empToUpdate.PettyCashCompanies = pettyCashCompanies;
            }

            //Emergency Contacts
            if (employee.emergencyontact != null)
            {
                var emergencyContact = context.emergencyontacts
                    .FirstOrDefault(x => x.Id == employee.emergencyontact.Id);
                if (emergencyContact != null)
                {
                    empToUpdate.emergencyontact = emergencyContact;
                }
                else
                {
                    empToUpdate.emergencyontact = employee.emergencyontact;
                }
            }
            //foreach (var _image in empToUpdate.person.personPhotos.ToList())
            //{
            //    if (_image.Person_Id != 0)
            //    {
            //        context.personPhotos.RemoveRange(context.personPhotos.Where(x => x.Person_Id == employee.person.Id));
            //    }
            //}


            context.SaveChanges();
        }

        public void updateEmp(Employee emp)
        {

            Employee empToUpdate = context.Employees.FirstOrDefault(x => x.EmpId == emp.EmpId);
            if (empToUpdate != null)
            {
                if (emp.address != null)
                {
                    var address = context.Address.FirstOrDefault(x => x.Id == emp.address.Id);
                    empToUpdate.address = address;

                }

                if (emp.contact != null)
                {
                    var contact = context.Contacts.FirstOrDefault(x => x.Id == emp.contact.Id);
                    empToUpdate.contact = contact;

                }
                if (emp.Desig != null)
                {
                    var desig = context.Designations.FirstOrDefault(x => x.DesigId == emp.Desig.DesigId);
                    empToUpdate.Desig = desig;

                }
                if (emp.person != null)
                {
                    var person = context.Persons.FirstOrDefault(x => x.Id == emp.person.Id);
                    empToUpdate.person = person;

                }

                if (emp.Supervisor != null)
                {
                    var super = context.Employees.FirstOrDefault(x => x.EmpId == emp.Supervisor.EmpId);
                    empToUpdate.Supervisor = super;

                }

                if (emp.SalesTarget != null)
                {
                    var salesTarget = context.SalesTargets.FirstOrDefault(x => x.Id == emp.SalesTarget.Id);
                    empToUpdate.SalesTarget = salesTarget;

                }



            }


            context.SaveChanges();
        }

        public void updatePersonalInfo(int employeeID, Person person)
        {
            Employee empToUpdate = context.Employees.FirstOrDefault(x => x.EmpId == employeeID);
            empToUpdate.person = person;
            context.SaveChanges();
        }
        public void addDesignation(Designation designation)
        {
            if (designation == null)
                throw new NullReferenceException("Object can not be null");


            //company
            if (designation.company != null)
            {
                var comp = context.Companies.FirstOrDefault(x => x.Id == designation.company.Id);
                designation.company = comp;
            }

            //Department
            if (designation.department != null)
            {
                var dept = context.Departments.FirstOrDefault(x => x.Id == designation.department.Id);
                designation.department = dept;
            }

            //user
            if (designation.user != null)
            {
                var user = context.Users.FirstOrDefault(s => s.id == designation.user.id);
                designation.user = user;
            }

            //parent designation
            if (designation.parentDesignation != null)
            {
                var desig = context.Designations.FirstOrDefault(s => s.DesigId == designation.parentDesignation.DesigId);
                designation.parentDesignation = desig;
            }

            context.Designations.Add(designation);
            context.SaveChanges();
        }
        ///<summary>
        ///Get All Designations
        /// </summary>
        /// <param name="designationId">Desig Id</param>

        public List<Designation> getAllDesignation()
        {
            return context.Designations.ToList();

        }
        /// <summary>
        /// Get all active designations
        /// </summary>
        /// <returns></returns>
        public List<Designation> getAllActiveDesignation()
        {
            return context.Designations
                .Where(x => x.isActive == true)
                .ToList();

        }
        /// <summary>
        /// Gets only designations whose parent Id = null
        /// </summary>
        /// <returns></returns>
        public List<Designation> getParentDesignation()
        {
            return context.Designations
                .Where(x => x.ParentId == null)
                .ToList();

        }

        ///<summary>
        ///Get current User Settings
        /// </summary>
        /// <param name="designationId">Desig Id</param>

        public Designation getDesignation(int designationId)
        {
            return context.Designations.FirstOrDefault(x => x.DesigId == designationId);

        }
        /// <summary>
        /// Update designation
        /// </summary>
        /// <param name="designation">object of designation class</param>
        public void updateDesignation(Designation designation)
        {
            Designation empToUpdate = context.Designations.FirstOrDefault(x => x.DesigId == designation.DesigId);
            //Parent Designation
            if (designation.parentDesignation != null)
            {
                var parent = context.Designations.FirstOrDefault(x => x.DesigId == designation.parentDesignation.DesigId);
                empToUpdate.parentDesignation = parent;
            }
            else
            {
                empToUpdate.ParentId = null;

                empToUpdate.parentDesignation = null;
            }
            //Designation title
            empToUpdate.Title = designation.Title;

            //Designation is Active
            empToUpdate.isActive = designation.isActive;
            empToUpdate.AnnualLeaveDays = designation.AnnualLeaveDays;
            empToUpdate.CasualLeaveDays = designation.CasualLeaveDays;

            empToUpdate = designation;

            context.SaveChanges();
        }
        /// <summary>
        /// Update employee status 
        /// </summary>
        /// <param name="employeeID"> Employee Id</param>
        /// <param name="status">Object of Employee Status</param>
        public void updateEmpoyeeStatue(int employeeID, EmployeeStatus status)
        {
            Employee empToUpdate = context.Employees.FirstOrDefault(x => x.EmpId == employeeID);
            empToUpdate.Status = status;
            context.SaveChanges();
        }

        public void assignCompany(int employeeID, Company company)
        {
            Employee empToUpdate = context.Employees.FirstOrDefault(x => x.EmpId == employeeID);

            if (!empToUpdate.Companies.Contains(company))
            {
                empToUpdate.Companies.Add(company);
            }

            context.SaveChanges();
        }

        public void assignDepartment(int employeeID, Department department)
        {
            Employee empToUpdate = context.Employees.FirstOrDefault(x => x.EmpId == employeeID);

            if (!empToUpdate.departments.Contains(department))
            {
                empToUpdate.departments.Add(department);
            }

            context.SaveChanges();
        }

        public List<Employee> GetAllEmployees()
        {
            return context.Employees
                                .ToList();
        }
        public List<Employee> GetAllEmployeesForRegister()
        {
            return context.Employees
                                .Where(x => x.employeeApproval.isVoid != true && x.employeeApproval.PendingForClosing != true && x.employeeApproval.isApproved != false && x.employeeApproval.isReApproved != false)

                                .ToList();
        }

        public List<Employee> GetAllActiveEmployees()
        {
            return context.Employees
                .Where(x => x.employeeStatus.isActive == true && x.employeeApproval.isVoid != true && x.employeeApproval.PendingForClosing != true && x.employeeApproval.isApproved != false && x.employeeApproval.isReApproved != false)
                .ToList();
        }
        public List<Employee> GetAllInactiveEmployees()
        {
            return context.Employees
                  .Where(x => x.employeeStatus.isActive == false && x.employeeApproval.isVoid != true && x.employeeApproval.PendingForClosing != true && x.employeeApproval.isApproved != false && x.employeeApproval.isReApproved != false)
                  .ToList();
        }


        public List<Employee> GetEmployees()
        {
            return context.Employees
                                .ToList();
        }
        /// <summary>
        /// get all Inactive employees
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Employee> GetinActiveEmployees()
        {
            return context.Employees
                .Where(x => x.isActive == false).ToList();
        }
        /// <summary>
        /// get all active employees
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Employee> GetActiveEmployees()
        {
            return context.Employees
                .Where(x => x.isActive == true).ToList();
        }
        public List<Employee> GetActiveEmployeesForNewUser()
        {
            return context.Employees

                .Where(x => x.isActive == true && x.EmployeeUsers.Count == 0 || x.isActive == true && x.isMultiUser == true).ToList();
        }

        /// <summary>
        /// get all active employees
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Employee> GetActiveEmployeesForCenter()
        {
            return context.Employees
               .Where(x => x.isActive == true).ToList();
        }


        /// <summary>
        /// get all Pending for approval employees
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Employee> GetPendingForApprovalEmp()
        {
            return context.Employees

                .Where(x => x.employeeApproval.isApproved == false)
                .ToList();
        }


        /// <summary>
        /// get all Pending for Closing employees
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Employee> GetPendingForClosingEmp()
        {
            return context.Employees
                .Where(x => x.employeeApproval.PendingForClosing == true)
                .ToList();
        }
        /// <summary>
        /// Return Pending for reapproval employee list
        /// </summary>
        /// <returns></returns>
        public List<Employee> GetPendingForReapprovalEmp()
        {
            return context.Employees
                .Where(x => x.employeeApproval.isReApproved == false)
                .ToList();
        }

        /// <summary>
        /// Get list of void employees
        /// </summary>
        /// <returns>List(Employee) </returns>
        public List<Employee> GetVoidEmployees()
        {
            return context.Employees
               .Where(x => x.employeeApproval.isVoid == true)
                .ToList();
        }

        public User GetUserFromEmployee(int EmpId)
        {
            var user = context.Users.FirstOrDefault(x => x.employee.EmpId == EmpId);
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public List<Employee> GetTeamMembers(int empID)
        {
            Employee employee = GetEmployee(empID);

            return context.Employees
                .Where(x => x.SupervisorId == employee.EmpId || x.SupervisorId == employee.SupervisorId).ToList();
        }
        public List<User> GetTeamMembersbyUserId(int UserId, int empID)
        {
            Employee employee = GetEmployeeForUserProfile(empID);

            return context.Users

                .Where(x => x.employee.SupervisorId == employee.EmpId || x.employee.SupervisorId == employee.SupervisorId).ToList();
        }
        public Employee GetEmployee(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }

        public Employee GetEmployeeForForm(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }


        public Employee GetEmployeeForProfile(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }


        public Employee GetEmployeeForUserProfile(int empID)
        {
            return context.Employees
                //.Include("person")


                .FirstOrDefault(x => x.EmpId == empID);
        }

        public Employee GetEmployeeForPayments(int empID)
        {
            return context.Employees

                .FirstOrDefault(x => x.EmpId == empID);
        }


        public Employee GetEmployeeCompanies(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }


        public Employee GetEmployeeOnly(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }
        public List<CustomerCompany> GetAllCustomersByUserId(int UserId)
        {


            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == UserId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.customerCompanies
                .Where(x => x.departments.Any(y => deptIds.Contains(y.Id)) && x.Companies.Any(y => companyIds.Contains(y.Id)) && x.company.compnayType == CompnayTypes.CustomerCompany)
                .ToList();
        }
        public List<CustomerCompany> GetActiveCustomersByUserId(int UserId)
        {


            var user = context.Users.FirstOrDefault(x => x.id == UserId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.customerCompanies
                .Where(x => x.departments.Any(y => deptIds.Contains(y.Id)) && x.Companies.Any(y => companyIds.Contains(y.Id)) && x.isActive == true)
                .ToList();


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public void DeleteEmployee(int empID)
        {
            Employee empToUpdate = context.Employees
                .FirstOrDefault(x => x.EmpId == empID);


            context.Employees.Remove(empToUpdate);
            context.SaveChanges();
        }
        ///
        /// 
        /// <summary>
        /// get user based on employeeid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public User getuser(int userid)
        {
            return context.Users

                .FirstOrDefault(x => x.id == userid);

        }
        /// <summary>
        /// get user based on employeeid
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public User getemployeeuser(int empid)
        {
            return context.Users
                .FirstOrDefault(x => x.employeeId == empid);

        }
        /// <summary>
        /// Get all offers.
        /// </summary>
        /// <returns></returns>
        public List<CustomerCompany> getAll(Employee employee)
        {
            //var employee = GetEmployee(EmployeeId);
            //List<int> deptIds = new List<int>();
            List<CustomerCompany> Customers = new List<CustomerCompany>();
            foreach (var dpt in employee.departments)
            {   //deptIds.Add(dpt.Id);
                var customerCompany = context.customerCompanies
                 .FirstOrDefault(x => x.departments.Contains(dpt));
                Customers.Add(customerCompany);
            }
            return Customers;
        }
        /// <summary>
        /// add user to db
        /// </summary>
        /// <param name="user"></param>
        public void Adduser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
        /// <summary>
        /// add user to db
        /// </summary>
        /// <param name="user"></param>
        public void updateuser(User user)
        {
            User useer = new User();
            useer = user;
            context.SaveChanges();
        }
        ///<summary>
        ///Get current User Settings
        /// </summary>
        /// <param name="UserId"> User Id</param>

        public List<UserSettings> GetUserSettingsByUserId(int UserId)
        {
            return context.userSettings.Where(x => x.userId == UserId).ToList();

        }
        ///<summary>
        ///Get current User Settings
        /// </summary>
        /// <param name="UserId"  > User Id</param>
        /// <param name="settingKey">Setting Key</param>
        public UserSettings GetUserSettingsByUser(int UserId, string settingKey)
        {
            if (settingKey == "grdTargetsSaleOrder")
                return context.userSettings.FirstOrDefault(x => x.settingkey == settingKey);
            else
                return context.userSettings.FirstOrDefault(x => x.userId == UserId && x.settingkey == settingKey);
        }

        ///<summary>
        ///Get Settings
        /// </summary>
        /// <param name="UserId"  > User Id</param>
        /// <param name="settingKey">Setting Key</param>
        public UserSettings GetSettings(string settingKey)
        {
            return context.userSettings.FirstOrDefault(x => x.settingkey == settingKey);
        }

        ///<summary>
        ///Save User Settings Against User
        /// </summary>
        /// <param name="userSetting"> UserSetting Object</param>
        public void SaveUserSetting(UserSettings userSetting)
        {
            UserSettings userSet = new UserSettings();
            userSet = context.userSettings.FirstOrDefault(x => x.settingkey == userSetting.settingkey && x.userId == userSetting.userId);
            // Check if the Settings Already Exist in database for current element and user
            if (userSet != null)
            { //if true Update the value 
                userSet.settingValue = userSetting.settingValue;
                userSet.lastModified = System.DateTime.Now;
                context.SaveChanges();

            }
            else
            {
                //Create a new user Setting

                context.userSettings.Add(userSetting);
                context.SaveChanges();
            }

        }

        public void DeleteSetting(string settingKey)
        {
            var userSet = context.userSettings.FirstOrDefault(x => x.settingkey == settingKey);
            if (userSet != null)
            {
                context.userSettings.Remove(userSet);
                context.SaveChanges();
            }
        }

        ///<summary>
        ///Add User Settings Against User
        /// </summary>
        /// <param name="userSetting"> UserSetting Object</param>
        public void AddUserSetting(UserSettings userSetting)
        {

            context.userSettings.Add(userSetting);
            context.SaveChanges();


        }
        ///<summary>
        ///Update User Settings Against User
        /// </summary>
        /// <param name="userSetting"> UserSetting Object</param>
        public void UpdateUserSetting(UserSettings userSetting)
        {

            {

                UserSettings userSet = new UserSettings();
                userSet = userSetting;
                context.SaveChanges();
            }

        }

        //HR Info and Employee Function Related fnx


        /// <summary>
        /// add function to db
        /// </summary>
        /// <param name="fnx"></param>
        public void AddFunction(Function fnx)
        {
            context.functions.Add(fnx);
            context.SaveChanges();
        }
        /// <summary>
        /// Update function in db
        /// </summary>
        /// <param name="fnx"></param>
        public void UpdateFunction(Function fnx)
        {
            //Function function = new Function();
            //function = fnx;
            //context.SaveChanges();


            if (fnx == null)
                throw new NullReferenceException("Object can not be null");

            var _fnx = context.functions.FirstOrDefault(x => x.Id == fnx.Id);
            if (_fnx == null)
                throw new Exception("Asset Status object not found");

            _fnx.Title = fnx.Title;
            _fnx.IsActive = fnx.IsActive;
            _fnx.functionType = fnx.functionType;

            if (fnx.company != null)
            {
                var cmp = context.Companies.FirstOrDefault(x => x.Id == fnx.company.Id);
                if (cmp != null)
                {
                    _fnx.company = cmp;

                }
            }
            else
            {
                _fnx.company = null;
            }
            context.SaveChanges();



        }
        /// <summary>
        /// Retusn a function based on ID
        /// </summary>
        /// <param name="fnxId"></param>
        /// <returns></returns>
        public Function GetFunction(int fnxId)
        {
            var fnx = context.functions.FirstOrDefault(x => x.Id == fnxId);
            if (fnx != null)
            {
                return fnx;
            }
            else
            {
                return null;
            }

        }
        /// <summary>
        /// Return the list of functions from functions table
        /// </summary>
        /// <returns></returns>
        public List<Function> GetAllFunctions()
        {
            var fnxLst = context.functions.ToList();
            return fnxLst;
        }

        public void AddUserSignature(byte[] byteImg, User user)
        {
            User newUser = new User();

            newUser = user;
            if (user.employee != null)
            {
                if (user.employee.person != null)
                {
                    var per = context.Persons.FirstOrDefault(x => x.Id == user.employee.person.Id);
                    per.Signature = byteImg;
                    newUser.employee.person = per;

                }

            }

            context.SaveChanges();
        }
        public void ClearUserSignature(User user)
        {
            User newUser = new User();

            newUser = user;
            if (user.employee != null)
            {
                if (user.employee.person != null)
                {
                    var per = context.Persons.FirstOrDefault(x => x.Id == user.employee.person.Id);
                    per.Signature = null;
                    newUser.employee.person = per;

                }
                context.SaveChanges();

            }
        }
        public void AddUserImage(byte[] byteImg, User user)
        {
            User newUser = new User();

            newUser = user;
            if (user.employee != null)
            {
                if (user.employee.person != null)
                {
                    var per = context.Persons.FirstOrDefault(x => x.Id == user.employee.person.Id);
                    per.Photo = byteImg;
                    newUser.employee.person = per;

                }

            }

            context.SaveChanges();
        }

        public void ClearUserImage(User user)
        {


            User newUser = new User();

            newUser = user;
            if (user.employee != null)
            {
                if (user.employee.person != null)
                {
                    var per = context.Persons.FirstOrDefault(x => x.Id == user.employee.person.Id);
                    per.Photo = null;
                    newUser.employee.person = per;

                }
                context.SaveChanges();

            }
        }



        //Fnx for Employee Status


        /// <summary>
        /// Add Employee status in DB
        /// </summary>
        /// <param name="empStatus"></param>
        public void AddEmployeeStatus(EmployeeWorkingStatus empStatus)
        {
            if (empStatus == null)
                throw new NullReferenceException("Object can not be null");
            context.employeeStatuses.Add(empStatus);
            context.SaveChanges();

        }

        /// <summary>
        /// Get all employee statuses Lost
        /// </summary>
        /// <returns></returns>
        public List<EmployeeWorkingStatus> GetAllEmployeeStatus()
        {
            return context.employeeStatuses.ToList();
        }

        /// <summary>
        /// Get all Active Employee Status
        /// For "OPEN" Status
        ///  </summary>
        /// <returns></returns>
        public List<EmployeeWorkingStatus> GetAllActiveEmployeeStatus()
        {
            return context.employeeStatuses
                .Where(x => x.isActive == true)
                .ToList();
        }
        /// <summary>
        /// Get all Inactive Employee Status.
        /// For "CLOSE" status
        /// </summary>
        /// <returns></returns>
        public List<EmployeeWorkingStatus> GetAllInActiveEmployeeStatus()
        {
            return context.employeeStatuses
                .Where(x => x.isActive == false)
                .ToList();
        }

        /// <summary>
        /// Get Employee Status based on Id.
        /// </summary>
        ///  <param name="empStatusId"></param>
        /// <returns>AssetStatus</returns>
        public EmployeeWorkingStatus GetEmployeeStatus(int empStatusId)
        {
            var status = context.employeeStatuses
                //.Include("Asset")
                .FirstOrDefault(x => x.Id == empStatusId);

            return status;
        }

        /// <summary>
        /// Updates Employee status
        /// </summary>
        /// <param name="empStatus"></param>
        public void UpdateEmployeeStatus(EmployeeWorkingStatus empStatus)
        {
            if (empStatus == null)
                throw new NullReferenceException("Object can not be null");

            var _EmpStatus = context.employeeStatuses.FirstOrDefault(x => x.Id == empStatus.Id);
            if (_EmpStatus == null)
                throw new Exception("Employee Status object not found");

            _EmpStatus.backcolor = empStatus.backcolor;
            _EmpStatus.forecolor = empStatus.forecolor;
            _EmpStatus.HierarchicalIndex = empStatus.HierarchicalIndex;
            _EmpStatus.isActive = empStatus.isActive;
            _EmpStatus.isApproved = empStatus.isApproved;
            _EmpStatus.Status = empStatus.Status;

            context.SaveChanges();

        }


        //Functions for count


        public int getVoidRegisterAdministratorCount()
        {
            return context.Employees
                .Where(x => x.employeeApproval.isVoid == true)
                .Count();
        }
        public int getAllPendingForApprovalAdminCount()
        {
            return context.Employees

                .Where(x => x.employeeApproval.isApproved == false)
                .Count();
        }
        public int getRegisterAdministratorCount()
        {
            return context.Employees
                .Where(x => x.employeeApproval.isVoid != true)
                .Count();
        }
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.Employees

                .Where(x => /*x.employeeApproval.isApproved == true &&*/ x.employeeApproval.PendingForClosing == true /*&& x.employeeApproval.isVoid != true*/)
                .Count();
        }
        public int getAllPendingForReApprovalAdminCount()
        {
            return context.Employees

                .Where(x => x.employeeApproval.isReApproved == false)
                .Count();
        }


        public void AddExperience(EmployeeWorkExperience exp)
        {
            if (exp == null)
                throw new Exception("Error in adding experience");

            context.employeeWorkExperiences.Add(exp);
            context.SaveChanges();
        }

        public void UpdateExperience(EmployeeWorkExperience exp)
        {
            var exp2 = context.employeeWorkExperiences.FirstOrDefault(x => x.Id == exp.Id);
            if (exp2 != null)
            {
                exp2 = exp;
            }
            context.SaveChanges();
        }

        public EmployeeWorkExperience GetWorkExperience(int id)
        {
            var exp = context.employeeWorkExperiences.FirstOrDefault(x => x.Id == id);
            return exp;
        }
        public List<EmployeeWorkExperience> GetAllWork()
        {
            var expList = context.employeeWorkExperiences.ToList();
            return expList;
        }
        public void RemoveExperience(int id)
        {
            var exp = context.employeeWorkExperiences.Where(d => d.Id == id)
                      .First();
            context.employeeWorkExperiences.Remove(exp);
            context.SaveChanges();

        }
        public List<Employee> GetAllActiveEmployeesForBackgroundImage()
        {
            var emp = context.Employees

                    .Where(x => x.isActive == true && x.employeeStatus.isActive == true && x.employeeApproval.isVoid != true /* && x.employeeApproval.PendingForClosing != true && x.employeeApproval.isApproved != false && x.employeeApproval.isReApproved != false*/)
                    .ToList();
            return emp;
        }
        public List<EmployeeCoaCompanies> GetAllCOAEmployeesCompanies(int empId)
        {
            return context.employeeCoaCompanies
                .Where(x => x.EmpId == empId)
                    .ToList();
        }
        public void AddEmployeeCompanies(List<EmployeeCoaCompanies> coaCompanies)
        {
            context.employeeCoaCompanies
              .AddRange(coaCompanies);
            context.SaveChanges();
        }
        public void RemoveCoaCompanies( int empId)
        {
            List<EmployeeCoaCompanies> list = context.employeeCoaCompanies.Where(s => s.EmpId == empId ).ToList();
            context.employeeCoaCompanies.RemoveRange(list);
            context.SaveChanges();
        }
        public List<Company> GetUserCOACompanies(int empId)
        {
            List<Company> employeeCompanies = new List<Company>();


           var dbCompanies= context.employeeCoaCompanies
                .Where(x => x.EmpId == empId)
                    .ToList();

            foreach (var _comp in dbCompanies)
            {
                employeeCompanies.Add(_comp.Company);
            }
            return employeeCompanies;

        }
    }

}
