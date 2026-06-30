using ERP_BL.Procurements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface IModuleContractRepo
    {
        void Add(ModuleContract ModuleContract);
        void Add(ModuleContract ModuleContract, ProcurementProduct product);
        void addStatus(ModuleContractStatus status);
        void update(ModuleContract ModuleContract);
        void update(ModuleContract ModuleContract, Employee employee);
        void updateStatus(ModuleContract ModuleContract, ModuleContractStatus status);
        void updateStatus(int ModuleContractId, ModuleContractStatus status);


        //void upadteItemType(Item Item, ItemType itemType);
        //void upadteItemType(int ModuleContractItemId, ItemType itemType);


        List<ModuleContract> getAll();
        List<ModuleContract> getAll(User user);
        List<ModuleContract> getAll(Employee employee);

        //List<ItemType> getAllItemTypes();
        ModuleContract get(int ModuleContractId);
    }
    public class ModuleContractRepo : IModuleContractRepo
    {


        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Add ModuleContract in database
        /// </summary>
        /// <param name="ModuleContract">ModuleContract Object</param>
        public void Add(ModuleContract ModuleContract)
        {

            context.ModuleContracts.Add(ModuleContract);
            context.SaveChanges();
        }
        ///// <summary>
        ///// Add ItemType in database
        ///// </summary>
        ///// <param name="itemType">ItemType Object</param>
        //public void additemtype(ItemType itemType)
        //{
        //    context.itemTypes.Add(itemType);
        //    context.SaveChanges();
        //} 
        /// <summary>
        /// Add item to specified ModuleContract
        /// </summary>
        /// <param name="ModuleContract">ModuleContract Object (main object in which Item is to be added)</param>
        /// <param name="ModuleContractItem">ModuleContractItem Object (item to be added)</param>
        public void Add(ModuleContract ModuleContract, ProcurementProduct product)
        {
            ModuleContract ModuleContracttoUpdate = context.ModuleContracts.FirstOrDefault(x => x.Id == ModuleContract.Id);
            ModuleContracttoUpdate.products.Add(product);
            context.SaveChanges();
        }
        /// <summary>
        /// get vendorsCompany matching to ID
        /// </summary>
        /// <param name="vendorsCompID">Vendor Company ID</param>
        /// <returns></returns>
        public Vendor getVendor(int vendorsCompID)
        {
            return context.Vendors

                .FirstOrDefault(x => x.Id == vendorsCompID);
        }
        /// <summary>
        /// Add ModuleContractStatus in database
        /// </summary>
        /// <param name="status">ModuleContractStatus Object</param>
        public void addStatus(ModuleContractStatus status)
        {
            context.ModuleContractStatuses.Add(status);
            context.SaveChanges();
        }


        /// <summary>
        /// Update ModuleContract
        /// </summary>
        /// <param name="ModuleContract"></param>
        public void update(ModuleContract ModuleContract)
        {
            ModuleContract ModuleContracttoUpdate = context.ModuleContracts.FirstOrDefault(x => x.Id == ModuleContract.Id);
            //foreach (var prod in ModuleContracttoUpdate.products)
            //{
            //    if (prod.Id != 0)
            //    {
            //        context.inquiryProducts.Remove(context.inquiryProducts.FirstOrDefault(x => x.Id == prod.product_Id));
            //        context.procurementProducts.Remove(context.procurementProducts.FirstOrDefault(x => x.Id == prod.Id));
            //    }
            //}
            ModuleContracttoUpdate = ModuleContract;
            context.SaveChanges();
        }
        public void updateFromGrid(ModuleContract ModuleContract)
        {
            ModuleContract ModuleContracttoUpdate = context.ModuleContracts.FirstOrDefault(x => x.Id == ModuleContract.Id);
            ModuleContracttoUpdate = ModuleContract;
            ModuleContracttoUpdate.ModuleContractStatus = context.ModuleContractStatuses.FirstOrDefault(x => x.Id == ModuleContract.ModuleContractStatus.Id);
            context.SaveChanges();
        }

        /// <summary>
        /// Update ModuleContract and allocate it to new employee
        /// </summary>
        /// <param name="ModuleContract">ModuleContract to be updated</param>
        /// <param name="employee">New Employee as replacement</param>
        public void update(ModuleContract ModuleContract, Employee employee)
        {
            ModuleContract ModuleContracttoUpdate = context.ModuleContracts.FirstOrDefault(x => x.Id == ModuleContract.Id);
            ModuleContracttoUpdate.employee = employee;
            context.SaveChanges();
        }

        /// <summary>
        /// Change ModuleContract status
        /// </summary>
        /// <param name="ModuleContract"></param>
        /// <param name="status"></param>
        public void updateStatus(ModuleContract ModuleContract, ModuleContractStatus status)
        {
            ModuleContract ModuleContracttoUpdate = context.ModuleContracts.FirstOrDefault(x => x.Id == ModuleContract.Id);
            ModuleContracttoUpdate.ModuleContractStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Get list of all Products in DB
        /// </summary>
        /// <returns>List of Products Objects</returns>
        public List<Product> getAllProducts()
        {
            return context.Products.ToList();

        }


        /// <summary>
        /// Change ModuleContract status based on ModuleContract ID
        /// </summary>
        /// <param name="ModuleContractId"></param>
        /// <param name="status"></param>
        public void updateStatus(int ModuleContractId, ModuleContractStatus status)
        {
            ModuleContract ModuleContracttoUpdate = context.ModuleContracts.FirstOrDefault(x => x.Id == ModuleContractId);
            ModuleContracttoUpdate.ModuleContractStatus = status;
            context.SaveChanges();
        }


        /// <summary>
        /// Change ModuleContract to Void
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="isVoid"></param>
        public void setModuleContracttoVoid(int Id, bool isVoid)
        {
            ModuleContract item = context.ModuleContracts.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            context.SaveChanges();
        }

        /// <summary>
        /// Get all ModuleContracts.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAll(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .ToList();

        }
        public List<ModuleContract> getAllFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate & (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate & (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }
        public List<ModuleContract> getAllByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }

        /// <summary>
        /// Get all ModuleContracts.
        /// </summary>
        /// <returns></returns>
        public int getAllActiveCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

            .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }



        /// <summary>
        /// Get all Closed ModuleContracts.
        /// </summary>
        /// <returns></returns>
        public int getAllInActiveCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

            .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }


        //#Void
        /// <summary>
        /// Get all Void ModuleContracts for user.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .ToList();
        }
        /// <summary>
        /// Get all Void ModuleContracts .
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getVoidRegisterAdministrator()
        {
            return context.ModuleContracts

        .Where(x => x.isVoid == true)
    .ToList();
        }
        /// <summary>
        /// Get all Void ModuleContracts .
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.ModuleContracts

    .Where(x => x.isVoid == true)
    .Count();
        }
        /// <summary>
        /// Get all Void ModuleContracts for user count. 
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.ModuleContracts
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true)
                .Count();
        }
        /// <summary>
        /// Get all Void ModuleContracts own.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }


        /// <summary>
        /// Get all pending ModuleContracts by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<ModuleContract> getAllPendingForApprovalDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        public List<ModuleContract> getAllPendingForApprovalDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        /// <summary>
        /// Get Count pending ModuleContracts by Departments
        /// <returns></returns>
        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
        }


        /// <summary>
        /// Get all pending ModuleContracts by Departmental
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<ModuleContract> getAllPendingForClosingDepartmentalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        public List<ModuleContract> getAllPendingForClosingDepartmentalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        /// <summary>
        /// Get all pending ModuleContracts by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }


        /// <summary>
        /// Get all ModuleContracts Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllPendingforApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        public List<ModuleContract> getAllPendingforApprovalFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        public List<ModuleContract> getAllPendingforApprovalByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);

            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        /// <summary>
        /// Get all ModuleContracts Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllPendingforApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        public List<ModuleContract> getAllPendingforApprovalOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        public List<ModuleContract> getAllPendingforApprovalOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();

        }
        /// <summary>
        /// Get Count ModuleContracts Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();

        }




        /// <summary>
        /// Get Count ModuleContracts Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();

        }



        /// <summary>
        /// Get all ModuleContracts.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllPendingsforAdministrator()
        {
            return context.ModuleContracts

                .Where(x => x.isApproved == false).ToList();
        }
        public List<ModuleContract> getAllPendingsforAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == false).ToList();
        }
        public List<ModuleContract> getAllPendingsforAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == false).ToList();
        }
        /// <summary>
        /// Get Count of ModuleContracts pending for approval.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingsforApprovalAdministratorCount()
        {
            return context.ModuleContracts

                .Where(x => x.isApproved == false).Count();
        }
        /// <summary>
        /// Get all ModuleContracts Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllPendingforClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        public List<ModuleContract> getAllPendingforClosingFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= previousMonthDate && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        public List<ModuleContract> getAllPendingforClosingByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.EmpId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.CreationDate >= from && x.CreationDate <= to && x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        /// <summary>
        /// Get all ModuleContracts Pending for Approval.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllPendingforClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        public List<ModuleContract> getAllPendingforClosingOwnFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        public List<ModuleContract> getAllPendingforClosingOwnByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();

        }
        /// <summary>
        /// Get Count ModuleContracts Pending for Approval .
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }

        /// <summary>
        /// Get Count ModuleContracts Pending for Approval .
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforClosingCountown(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }



        /// <summary>
        /// Get all pending for Closing ModuleContracts.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllPendingforClosingAdministrator()
        {
            return context.ModuleContracts

                .Where(x => x.isApproved == true && x.PendingForClosing == true).ToList();
        }
        public List<ModuleContract> getAllPendingforClosingAdministratorFirst()
        {
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && x.isApproved == true && x.PendingForClosing == true).ToList();
        }
        public List<ModuleContract> getAllPendingforClosingAdministratorByDateRange(DateTime from, DateTime to)
        {
            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && x.isApproved == true && x.PendingForClosing == true).ToList();
        }
        /// <summary>
        /// Get Count pending for Closing ModuleContracts.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingforClosingAdministratorCount()
        {
            return context.ModuleContracts

                .Where(x => x.isApproved == true && x.PendingForClosing == true).Count();
        }
        /// <summary>
        /// Get all Closed ModuleContracts.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }
        public List<ModuleContract> getAllInActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }
        public List<ModuleContract> getAllInActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }
        /// <summary>
        /// Get all ModuleContracts.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }
        public List<ModuleContract> getAllActiveFirst(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }
        public List<ModuleContract> getAllActiveByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts

                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
                .ToList();

        }
        /// <summary>
        /// Get all ModuleContracts count by Status Id.
        /// </summary>
        /// <returns></returns>
        public int getModuleContractsCountByStatusId(int uid, int statusId)
        {
            using (var _DbContext = new DBContextERP())
            {
                if (uid != 0)
                {
                    var user = _DbContext.Users.FirstOrDefault(x => x.id == uid);
                    List<int> deptIds = new List<int>();
                    List<int> companyIds = new List<int>();
                    //foreach (var comp in user.employee.Companies)
                    for (int i = 0; i < user.employee.Companies.Count(); i++)
                        companyIds.Add(user.employee.Companies[i].Id);

                    //foreach (var dpt in user.employee.departments)
                    for (int j = 0; j < user.employee.departments.Count; j++)
                        deptIds.Add(user.employee.departments[j].Id);

                    return _DbContext.ModuleContracts
                        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.Id == statusId && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbContext.ModuleContracts.Where(x => x.ModuleContractStatus.Id == statusId && x.isApproved == true).Count();
            }
        }
        /// <summary>
        /// Get all ModuleContracts count 
        /// </summary>
        /// <returns></returns>
        public int getModuleContractsCount(int uid)
        {
            using (var _DbContext = new DBContextERP())
            {
                if (uid != 0)
                {
                    var user = _DbContext.Users.FirstOrDefault(x => x.id == uid);
                    List<int> deptIds = new List<int>();
                    List<int> companyIds = new List<int>();
                    foreach (var comp in user.employee.Companies)
                        companyIds.Add(comp.Id);
                    foreach (var dpt in user.employee.departments)
                        deptIds.Add(dpt.Id);
                    return context.ModuleContracts

                        .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbContext.ModuleContracts.Where(x => x.isApproved == true).Count();
            }
        }
        /// <summary>
        /// Get all ModuleContracts by Status Id.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAllModuleContractsByStatusId(int uid, int statusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.Id == statusId && x.isApproved == true)
                .ToList();

        }
        public List<ModuleContract> getAllModuleContractsByStatusIdFirst(int uid, int statusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            var previousMonthDate = DateTime.Today.AddMonths(-1);
            return context.ModuleContracts

                .Where(x => x.CreationDate >= previousMonthDate && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= previousMonthDate && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.Id == statusId && x.isApproved == true)
                .ToList();

        }
        public List<ModuleContract> getAllModuleContractsByStatusIdByDateRange(DateTime from, DateTime to, int uid, int statusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.ModuleContracts


                .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.Id == statusId && x.isApproved == true)
                .ToList();

        }
        /// <summary>
        /// 
        /// Get all ModuleContracts.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContract> getAll()
        {
            return context.ModuleContracts

                .ToList();
        }
        /// <summary>
        /// Get all ModuleContracts Status.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContractStatus> getAllModuleContractStatus()
        {
            return context.ModuleContractStatuses.ToList();
        }
        /// <summary>
        /// Get all Active ModuleContracts Status.
        /// </summary>
        /// <returns></returns>
        public List<ModuleContractStatus> getAllActiveStatus()
        {
            List<ModuleContractStatus> statuses = new List<ModuleContractStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.ModuleContractStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;

        }/// <summary>
         /// Get all Inactive ModuleContracts Status.
         /// </summary>
         /// <returns></returns>
        public List<ModuleContractStatus> getAllInactiveStatus()
        {
            List<ModuleContractStatus> statuses = new List<ModuleContractStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.ModuleContractStatuses.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }
        /// <summary>
        /// Get all ModuleContracts Status.
        /// </summary>
        ///  <param name="ModuleContractstatusid"></param>
        /// <returns></returns>
        public ModuleContractStatus getstatus(int ModuleContractstatusid)
        {
            return context.ModuleContractStatuses.FirstOrDefault(x => x.Id == ModuleContractstatusid);
        }


        /// <summary>
        /// Get all ModuleContracts of specific Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public List<ModuleContract> getAll(Employee employee)
        {
            return context.ModuleContracts.Where(x => x.allocation_Id == employee.EmpId).ToList();
        }

        /// <summary>
        /// Get all ModuleContracts of specific User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<ModuleContract> getAll(User user)
        {
            return context.ModuleContracts.Where(x => x.user_Id == user.id).ToList();
        }
        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="status"></param>
        public void updateStatus(ModuleContractStatus status)
        {
            ModuleContractStatus ModuleContractStatus = context.ModuleContractStatuses.FirstOrDefault(x => x.Id == status.Id);
            ModuleContractStatus = status;
            context.SaveChanges();
        }

        //}

        /// <summary>
        /// Get ModuleContract based on ModuleContract ID.
        /// </summary>
        /// <param name="ModuleContractId"></param>
        /// <returns></returns>
        public ModuleContract get(int ModuleContractId)
        {
            return context.ModuleContracts




            .FirstOrDefault(x => x.Id == ModuleContractId);
        }
        public List<ModuleContract> getAllByOfferId(int offerId)
        {
            return context.ModuleContracts
                .Where(x => x.offer_Id == offerId)
                .ToList();
        }

        public int getAllPendingForApprovalDepartmentalCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.ModuleContracts
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();

        }
        public int getAllActiveCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.ModuleContracts
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }
        public int getAllInActiveCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.ModuleContracts
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }
        public int getAllPendingforApprovalCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingforApprovalCountOwnByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.ModuleContracts
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForClosingDepartmentalCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }

            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();

            var count = context.ModuleContracts
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
            return count;
        }
        public int getAllPendingforClosingCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        public int getAllPendingforClosingCountownByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.ModuleContracts

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }


        public int getAllPendingForApprovalDepartmentalCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.ModuleContracts
                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
        }
        public int getAllActiveCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            // var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }

            return context.ModuleContracts
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }
        public int getAllInActiveCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            //var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.ModuleContracts
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.ModuleContractStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();

        }
        public int getAllPendingforApprovalCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.ModuleContracts

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingforApprovalCountOwnByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.ModuleContracts
                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForClosingDepartmentalCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.ModuleContracts

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        public int getAllPendingforClosingCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.ModuleContracts

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        public int getAllPendingforClosingCountownByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> departmentId = new List<int>();
            foreach (var _dept in deptId)
            {
                departmentId.Add(_dept.Id);
            }
            return context.ModuleContracts

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }
        public void AddCompatativeStatement(ComparativeStatement statement)
        {
            context.comparativeStatements.Add(statement);
            context.SaveChanges();
        }
        public void UpdateCompatativeStatement(ComparativeStatement statement)
        {
            var dbStatement = context.comparativeStatements.FirstOrDefault(x => x.Id == statement.Id);
            dbStatement = statement;
            context.SaveChanges();
        }
        public ComparativeStatement GetCompatativeStatementById(int id)
        {
            return context.comparativeStatements
               .FirstOrDefault(x => x.Id == id);

        }
        public ComparativeStatementItem GetCompatativeStatementItemById(int id)
        {
            return context.comparativeStatementItems.FirstOrDefault(x => x.Id == id);

        }
        public ModuleContract GetForComparativeStatement(int ModuleContractId)
        {
            return context.ModuleContracts

            .FirstOrDefault(x => x.Id == ModuleContractId);
        }
        public FOCSampling GetFOCSampling(int _Id)
        {
            return context.fOCSamplings

            .FirstOrDefault(x => x.Id == _Id);
        }
        public PassOn GetPassOn(int _Id)
        {
            return context.passOns

            .FirstOrDefault(x => x.Id == _Id);
        }
        public ClaimDiscount GetClaimDiscount(int _Id)
        {
            return context.claimDiscounts

            .FirstOrDefault(x => x.Id == _Id);
        }
        public ClaimDiscount GetClaimDiscountByName(string _name)
        {
            return context.claimDiscounts

            .FirstOrDefault(x => x.discountName == _name);
        }
        public void AddFOCSampling(FOCSampling sampl)
        {
            context.fOCSamplings

           .Add(sampl);
            context.SaveChanges();
        }
        public void AddPassOn(PassOn passOn)
        {
            context.passOns

           .Add(passOn);
            context.SaveChanges();
        }
        public void AddClaimDiscount(ClaimDiscount claim)
        {
            context.claimDiscounts
           .Add(claim);
            context.SaveChanges();
        }
        public void UpdateFOCSampling(FOCSampling sampl)
        {

            var dbSamp = context.fOCSamplings.FirstOrDefault(x => x.Id == sampl.Id);
            dbSamp = sampl;
            context.SaveChanges();
        }
        public void UpdatePassOn(PassOn passOn)
        {

            var dbSamp = context.passOns.FirstOrDefault(x => x.Id == passOn.Id);
            dbSamp = passOn;
            context.SaveChanges();
        }
        public void UpdateClaimDiscount(ClaimDiscount claim)
        {
            var dbClaim = context.claimDiscounts.FirstOrDefault(x => x.Id == claim.Id);
            dbClaim = claim;
            context.SaveChanges();
        }
        public List<FOCSampling> GetAllActiveFOCSamplings()
        {
            return context.fOCSamplings.Where(x => x.isActive == true).ToList();
        }
        public List<ClaimDiscount> GetAllActiveClaimDiscounts()
        {
            return context.claimDiscounts.Where(x => x.isActive == true).ToList();

        }
        public List<FOCSampling> GetAllFOCSamplings()
        {
            return context.fOCSamplings/*.Where(x => x.isActive == true)*/.ToList();
        }
        public List<ClaimDiscount> GetAllClaimDiscounts()
        {
            return context.claimDiscounts/*.Where(x => x.isActive == true)*/.ToList();

        }
        public List<PassOn> GetAllPassOns()
        {
            return context.passOns/*.Where(x => x.isActive == true)*/.ToList();

        }

        public BookerStatementItem GetBookerStatement(int bookerId)
        {
            return context.bookerStatementItems.FirstOrDefault(x => x.Id == bookerId);

        }
        public FOCSampling GetFOCSamplingByName(string _name)
        {
            return context.fOCSamplings

            .FirstOrDefault(x => x.samplingtName == _name);
        }
        public PassOn GetPassOnByName(string _name)
        {
            return context.passOns

            .FirstOrDefault(x => x.passOnName == _name);
        }
        public void AddUniqueNUmber(UniqueNumber uniqueNumber)
        {
            context.uniqueNumbers

           .Add(uniqueNumber);
            context.SaveChanges();
        }
        public void UpdateUniqueNumber(UniqueNumber uniqueNumber)
        {

            var dbSamp = context.uniqueNumbers.FirstOrDefault(x => x.Id == uniqueNumber.Id);
            dbSamp = uniqueNumber;
            context.SaveChanges();
        }
        public UniqueNumber GetUniqueNumber(int _Id)
        {
            return context.uniqueNumbers

            .FirstOrDefault(x => x.Id == _Id);
        }
        public List<UniqueNumber> GetAllUniqueNumbers()
        {
            return context.uniqueNumbers/*.Where(x => x.isActive == true)*/.ToList();
        }
        public List<UniqueNumber> GetAllActiveUniqueNumbers()
        {
            return context.uniqueNumbers.Where(x => x.isActive == true).ToList();
        }
        public List<ModuleContract> getFirstModuleContractRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.ModuleContracts

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)

                .ToList();
        }
        public List<ModuleContract> getModuleContractRegisterAdministrator()
        {
            return context.ModuleContracts

                    .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && x.isVoid != true)
                    .ToList();
        }
        public int getModuleContractRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.ModuleContracts
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
                .Count();
        }
        public int getModuleContractRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.ModuleContracts
                .Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }
        public List<ModuleContract> ModuleContractRegisterByDateRange(DateTime from, DateTime to, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.ModuleContracts
        .OrderByDescending(x => x.CreationDate)
            .Where(x => x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) ||
            x.CreationDate >= from && x.CreationDate <= to && (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true)
            .ToList();


        }
    }
}
