using ERP_BL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class DepartmentRepo
    {
        DBContextERP context = new DBContextERP();
        public DepartmentRepo()
        {
            SystemLog.LogInfo(this.GetType(), "Instance Created! ");

        }
        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetDepartments()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived the List of All Departments! ");

            return context.Departments.Include("AccountPayable").ToList();
        }
        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetActiveDepartments()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived the List of All Departments! ");

            return context.Departments.Include("ChartofAccount").Where(x=>x.isActive==true).ToList();
        }
        public List<ERP_BL.Databases.Department> GetInActiveDepartments()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived the List of All Departments! ");

            return context.Departments.Where(x => x.isActive == false).ToList();
        }

        public List<ERP_BL.Databases.Department> GetHRMDepartments()
        {
            //List<Department> departments = new List<Department>();

            // SystemLog.LogInfo(this.GetType(), "Retrived the List of All Departments! ");
            return context
                .Departments
                .Where(x => x.Code.EndsWith("--MGMT") == true)
                .ToList();

            //var deps = context.Departments
            //    .ToList();
            //foreach (var _dep in deps)
            //{
            //    //var code = _dep.Code;
            //    //var subCode = code.EndsWith("--MGMT");
            //    if (_dep.Code.EndsWith("--MGMT"))
            //    {

            //    }
            //}
        }

        public List<ERP_BL.Databases.Department> GetManagerialDepts()
        {
            //List<Department> departments = new List<Department>();

            // SystemLog.LogInfo(this.GetType(), "Retrived the List of All Departments! ");
            return context
                .Departments
                .Where(x => x.IsManagerial == true)
                .ToList();

            //var deps = context.Departments
            //    .ToList();
            //foreach (var _dep in deps)
            //{
            //    //var code = _dep.Code;
            //    //var subCode = code.EndsWith("--MGMT");
            //    if (_dep.Code.EndsWith("--MGMT"))
            //    {

            //    }
            //}
        }


        /// <summary>
        /// Get single Department against specified ID
        /// </summary>
        /// <param name="DeptID">Department ID</param>
        /// <returns>Single Department Object</returns>
        public ERP_BL.Databases.Department GetDepartment(int DeptID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Department Having Id= "+DeptID);

            // var departments= context.Departments.ToList();
            return context.Departments.Include("employees").Include("companies").Include("Principals").Include("customers").Include("Vendors")
                .FirstOrDefault(x => x.Id == DeptID) ;
        }


        /// <summary>
        /// Get all Departments Ordered by Name
        /// </summary>
        /// <returns>List of Department Objects</returns>
        public List<ERP_BL.Databases.Department> GetCompaniesOrderByName()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of Departments ordered By Department Name ");

            return context.Departments.OrderBy(x => x.DeptName).ToList();
        }

        /// <summary>
        /// Add Subsidary Department based to parent Department object
        /// </summary>
        /// <param name="parentDept">Department Object as Parent Department</param>
        /// <param name="subsidaryDept">Department Object as Subsidary Department</param>
        public void Add(Department parentDept, Department subsidaryDept)
        {
            //using (var context = new DBContextERP(Connections.connection, false))
            {
                subsidaryDept.parentDepartment = parentDept;
                context.Departments.Add(subsidaryDept);
                context.SaveChanges();
            }
            SystemLog.LogInfo(this.GetType(), "Added Subsidory Department Name= " + subsidaryDept.DeptName + " Id =" + subsidaryDept.Id + " Of Parent Department Id= " + parentDept.Id +parentDept.DeptName);

        }

        /// <summary>
        /// Add Subsidary Department based to parent Department object
        /// </summary>
        /// <param name="parentDept">Department Object as Parent Department</param>
        /// <param name="subsidaryDept">Department Object as Subsidary Department</param>
        public void Add(int parentDeptId, Department subsidaryDept)
        {
            //using (var context = new DBContextERP(Connections.connection, false))
            {
                Department parentDept = context.Departments.FirstOrDefault(x => x.Id == parentDeptId);
                subsidaryDept.parentDepartment = parentDept;
                context.Departments.Add(subsidaryDept);
                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Added Subsidory Department Name= " + subsidaryDept.DeptName + " Id =" + subsidaryDept.Id +" Of Parent Department Id= "+parentDeptId);

            }
        }

        /// <summary>
        /// get Department matching to ID
        /// </summary>
        /// <param name="departmentID">Department ID</param>
        /// <returns></returns>
        public Department get(int departmentID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Department Having Id= " + departmentID);

            return context.Departments.FirstOrDefault(x => x.Id == departmentID);
        }

        /// <summary>
        /// Get all subsidaries related to Department
        /// </summary>
        /// <param name="DeptID"> Department ID </param>
        /// <returns></returns>
        public List<Department> getSubsidaries(int DeptID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of Subsidaries Department of Department Having Id= " + DeptID);

            return context.Departments
                .Where(x => x.ParentID == DeptID).ToList();
        }

        /// <summary>
        /// Get all subsidaries related to Department
        /// </summary>
        /// <param name="parentDepartment"> Department object </param>
        /// <returns></returns>
        public List<Department> getSubsidaries(Department parentDepartment)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of Subsidaries Department of Department Having Id= " + parentDepartment.Id);

            return context.Departments.Where(x => x.ParentID == parentDepartment.Id).ToList();
        }



        /// <summary>
        /// Add new Department
        /// </summary>
        /// <param name="department">Object of Department</param>
        public void addDepartment(Department department)
        {
            //using (var context = new DBContextERP(Connections.connection, false))
            {
                context.Departments.Add(department);
                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Added New Department Name= " + department.DeptName+ " Id =" +department.Id );

            }
        }

        /// <summary>
        /// Update Department details
        /// </summary>
        /// <param name="department">Department Object</param>
        public void updateDepartment(Department department)
        {
           // using (var context = new DBContextERP(Connections.connection, false))
            {
                Department deptToUpdate = context.Departments.FirstOrDefault(x => x.Id == department.Id);
                //deptToUpdate.Abbrivation = department.Abbrivation;
                //deptToUpdate.Code= department.Code;
                //deptToUpdate.DeptName= department.DeptName;
                deptToUpdate = department;
                //deptToUpdate.Timestamp= department.Timestamp;
                //deptToUpdate.parentDepartment = department.parentDepartment;
                //heelooooo there
                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Updated New Department Name= " + department.DeptName + " Id =" + department.Id);

            }
        }
        /// <summary>
        /// Get list of all TargetTypes in DB
        /// </summary>
        /// <returns>List of TargetTypes Objects</returns>
        public List<TargetType> getallTargetType()
        {
            return context.TargetTypes.ToList();

        }
        /// <summary>
        /// Get All Active TargetType
        /// </summary>
        /// <returns></returns>
        public List<TargetType> getActiveTargetTypes()
        {
            return context.TargetTypes.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive TargetTypes
        /// </summary>
        /// <returns></returns>
        public List<TargetType> getinActiveTargetTypes()
        {
            return context.TargetTypes.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new TargetType
        /// </summary>
        /// <param name="targetType">TargetType Object</param>
        public void AddTargetType(TargetType targetType)
        {
            context.TargetTypes.Add(targetType);
            context.SaveChanges();
        }
        /// <summary>
        /// get TargetType by ID
        /// </summary>
        /// <param name="targetTypeid">TargetType ID</param>
        /// <returns></returns>
        public TargetType getTargetType(int targetTypeId)
        {
            return context.TargetTypes
                .FirstOrDefault(x => x.Id == targetTypeId);
        }
        /// <summarProductNature
        /// </summary>
        /// <param name="TargetType">TargetType  Object</param>
        public void UpdateTargetType(TargetType targetType)
        {
            TargetType prod = context.TargetTypes.FirstOrDefault(x => x.Id == targetType.Id);
            prod = targetType;
            context.SaveChanges();
        }

        /// <summary>
        /// Get list of all Targets in DB
        /// </summary>
        /// <returns>List of Targets Objects</returns>
        public List<Target> getallTarget()
        {
            return context.Targets.Include("Company").Include("Department").Include("Type").Include("TargetAwards").ToList();

        }
        /// <summary>
        /// Get All Active Target
        /// </summary>
        /// <returns></returns>
        public List<Target> getActiveTargets()
        {
            return context.Targets.Include("Company").Include("Department").Include("Type").Include("TargetAwards").Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All Active Target for employee Departments
        /// </summary>
        /// <returns></returns>
        public List<Target> getActiveTargetsForUserandYear(int uid, int year)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.Targets.Include("Company").Include("Department").Include("Type").Include("TargetAwards").Where(x => deptIds.Contains((int)x.departmentId) && x.isActive == true && x.Year==year).ToList();

        }
        /// <summary>
        /// Get All Active Target for employee Departments
        /// </summary>
        /// <returns></returns>
        public List<Target> getActiveTargetsForDepartment(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.Targets.Include("Company").Include("Department").Include("Type").Include("TargetAwards").Where(x => deptIds.Contains((int)x.departmentId ) && x.isActive == true).ToList();

        }

        public object getAllCompanies()
        {
            return context.Companies.Where(s => s.compnayType == Enums.CompnayTypes.Company).ToList();
        }

        /// <summary>
        /// Get All  Target for Department
        /// </summary>
        /// <returns></returns>
        public List<Target> getTargets(int DepartmentId, int companyId, int Year)
        {
            return context.Targets.Include("Company").Include("Department").Include("Type").Include("TargetAwards").Where(x => x.isActive == true && x.departmentId==DepartmentId && x.Year == Year && x.companyId ==companyId).ToList();

        }
        /// <summary>
        /// Get All inActive Targets
        /// </summary>
        /// <returns></returns>
        public List<Target> getinActiveTargets()
        {
            return context.Targets.Include("Company").Include("Department").Include("Type").Include("TargetAwards").Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new Target
        /// </summary>
        /// <param name="target">Target Object</param>
        public void AddTarget(Target target)
        {
            context.Targets.Add(target);
            context.SaveChanges();
        }
        /// <summary>
        /// get Target by ID
        /// </summary>
        /// <param name="targetid">Target ID</param>
        /// <returns></returns>
        public Target getTarget(int targetId)
        {
            return context.Targets.Include("Company").Include("Department").Include("Type").Include("TargetAwards")
                .FirstOrDefault(x => x.Id == targetId);
        }

        /// <summary>
        /// get list of Targets by Department ID
        /// </summary>
        /// <param name="departmentid">department ID</param>
        /// <returns></returns>
        public List<Target> getTargetsByDepartmentId(int departmentId)
        {
            return context.Targets
                .Where(x => x.departmentId == departmentId).ToList();
        }
        /// <summarProductNature
        /// </summary>
        /// <param name="Target">Target  Object</param>
        public void UpdateTarget(Target target)
        {
            Target prod = context.Targets.FirstOrDefault(x => x.Id == target.Id);
            prod = target;
            context.SaveChanges();
        }

        /// get list of transactionFrequncies as a list of string
        /// 
        public List<string> GetTargetFrequencies()
        {
            return Enum.GetNames(typeof(Enums.TargetFrequency)).ToList();
        }

        public object GetAllCustomers()
        {
            return context.customerCompanies.Include("company").Include("contactPerson").ToList();
        }

        public object GetAllEmployees()
        {
            return context.Employees.Include("person").ToList();
        }
        public List<Vendor> GetAllVendors()
        {
            return context.Vendors.Include("company").Include("contactPerson").ToList();
        }

        public List<Principal> GetAllPrinciple()
        {
            return context.Principals.Include("company").Include("contactPerson").ToList();
        }
        public Department getForCoa(int departmentID)
        {

            return context.Departments.Include("ChartofAccount").FirstOrDefault(x => x.Id == departmentID);
        }
        public CustomerCompany getParent(int id)
        {
            return (CustomerCompany)context.customerCompanies.Where(x => x.ParentID == id).FirstOrDefault();
        }
        public List< CustomerCompany> getParents(int id)
        {
            return context.customerCompanies.Where(x => x.Id == id).ToList();
        }
        public List<ERP_BL.Databases.Department> GetUserDepartments(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.departments;
        }
        public Department getParentDepartment(int id)
        {
            return context.Departments.Where(x => x.ParentID == id).FirstOrDefault();
        }  
        public Department GetByName()
        {
            return  context.Departments.FirstOrDefault(x => x.DeptName == "VDummy");

        }
        public List<Department> GetAllLinkAble()
        {
            return context.Departments.Where(x => x.isLinkable == true && x.isActive==true).ToList();
        }

        public List<ERP_BL.Databases.DepartmentLevel> getAllDepartmentLevels()
        {
            return context.DepartmentLevels.ToList();
        }
        public void UpdateDepartmentLevel(DepartmentLevel _level)
        {
            var level = context.DepartmentLevels.FirstOrDefault(x => x.Id == _level.Id);
            level.Title = _level.Title;
            level.ParentID = _level.ParentID;
            context.SaveChanges();

        }
        public void AddDepartmentLevel(DepartmentLevel _level)
        {
            context.DepartmentLevels.Add(_level);
            context.SaveChanges();
            
        }
    }
}
