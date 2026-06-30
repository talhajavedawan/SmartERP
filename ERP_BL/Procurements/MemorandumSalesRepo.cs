using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface IMemorandumSaleRepo
    {
        void Add(MemorandumSale memorandumSale);
        void Add(MemorandumSale memorandumSale, ProcurementProduct product);
        void addStatus(MemorandumSaleStatus status);
        void update(MemorandumSale memorandumSale);

        void updateStatus(MemorandumSale memorandumSale, MemorandumSaleStatus status);
        void updateStatus(int memorandumSaleId, MemorandumSaleStatus status);


        //void upadteItemType(Item Item, ItemType itemType);
        //void upadteItemType(int memorandumSaleItemId, ItemType itemType);


        List<MemorandumSale> getAll();
        List<MemorandumSale> getAll(User user);


        //List<ItemType> getAllItemTypes();
        MemorandumSale get(int memorandumSaleId);
    }
    public class MemorandumSaleRepo : IMemorandumSaleRepo
    {


        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Add MemorandumSale in database
        /// </summary>
        /// <param name="memorandumSale">MemorandumSale Object</param>
        public void Add(MemorandumSale memorandumSale)
        {
            context.memorandumSales.Add(memorandumSale);
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
        /// Add item to specified memorandumSale
        /// </summary>
        /// <param name="memorandumSale">MemorandumSale Object (main object in which Item is to be added)</param>
        /// <param name="memorandumSaleItem">MemorandumSaleItem Object (item to be added)</param>
        public void Add(MemorandumSale memorandumSale, ProcurementProduct product)
        {
            MemorandumSale memorandumSaletoUpdate = context.memorandumSales.FirstOrDefault(x => x.Id == memorandumSale.Id);
            memorandumSaletoUpdate.products.Add(product);
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
        /// Add MemorandumSaleStatus in database
        /// </summary>
        /// <param name="status">MemorandumSaleStatus Object</param>
        public void addStatus(MemorandumSaleStatus status)
        {
            context.memorandumSaleStatuses.Add(status);
            context.SaveChanges();
        }


        /// <summary>
        /// Update MemorandumSale
        /// </summary>
        /// <param name="memorandumSale"></param>
        public void update(MemorandumSale memorandumSale)
        {
            MemorandumSale memorandumSaletoUpdate = context.memorandumSales.FirstOrDefault(x => x.Id == memorandumSale.Id);
            memorandumSaletoUpdate = memorandumSale;
            context.SaveChanges();
        }



        /// <summary>
        /// Change inquiry status
        /// </summary>
        /// <param name="status"></param>
        public void updateStatus(MemorandumSaleStatus status)
        {
            MemorandumSaleStatus poStatus = context.memorandumSaleStatuses.FirstOrDefault(x => x.Id == status.Id);
            poStatus = status;
            context.SaveChanges();
        }
        /// <summary>
        /// Change memorandumSale status
        /// </summary>
        /// <param name="memorandumSale"></param>
        /// <param name="status"></param>
        public void updateStatus(MemorandumSale memorandumSale, MemorandumSaleStatus status)
        {
            MemorandumSale memorandumSaletoUpdate = context.memorandumSales.FirstOrDefault(x => x.Id == memorandumSale.Id);
            memorandumSaletoUpdate.memorandumSaleStatus = status;
            context.SaveChanges();
        }


        /// <summary>
        /// Change memorandumSale status based on MemorandumSale ID
        /// </summary>
        /// <param name="memorandumSaleId"></param>
        /// <param name="status"></param>
        public void updateStatus(int memorandumSaleId, MemorandumSaleStatus status)
        {
            MemorandumSale memorandumSaletoUpdate = context.memorandumSales.FirstOrDefault(x => x.Id == memorandumSaleId);
            memorandumSaletoUpdate.memorandumSaleStatus = status;
            context.SaveChanges();
        }

        /// <summary>
        /// Get all memorandumSales.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAll(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true)
                .ToList();
        }

        //#Void
        /// <summary>
        /// Get all Void MemorandumSales for user.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true)
                .ToList();
        }
        /// <summary>
        /// Get all Void MemorandumSales .
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getVoidRegisterAdministrator()
        {
                    return context.memorandumSales

                .Where(x => x.isVoid == true)
            .ToList();
        }


        /// <summary>
        /// Get all Active memorandumSales.
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

            return context.memorandumSales

            .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.memorandumSaleStatus.isActive == true && x.isApproved == true)
            .Count();
        }

        /// <summary>
        /// Get all Inactive memorandumSales.
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

            return context.memorandumSales

            .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.memorandumSaleStatus.isActive == false && x.isApproved == true)
            .Count();
        }

        /// <summary>
        /// Get all Void MemorandumSales .
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.memorandumSales

    .Where(x => x.isVoid == true)
    .Count();
        }
        /// <summary>
        /// Get all Void MemorandumSales for user count. 
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
            return context.memorandumSales
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true)
                .Count();
        }
        /// <summary>
        /// Get all Void MemorandumSales own.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }


        /// <summary>
        /// Get all Inactive memorandumSales.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.memorandumSaleStatus.isActive == false && x.isApproved == true)
                .ToList();
        }
        /// <summary>
        /// Get all Active memorandumSales.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales


                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.memorandumSaleStatus.isActive == true && x.isApproved == true)
                .ToList();
        }



        /// <summary>
        /// Get all pending memorandumSales by Departments <paramref name="UserId"/>.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales
  
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }
        /// <summary>
        /// Get Count pending memorandumSales by Departments
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

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get all pending memorandumSales by Departmental
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        /// <summary>
        /// Get all pending memorandumSales by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        /// <summary>
        /// Get all pending memorandumSales by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales
               
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        /// <summary>
        /// Get Own pending memorandumSales by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales
            
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }
        /// <summary>
        /// Get Count pending memorandumSales by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get Own Count pending memorandumSales
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get all pending memorandumSales by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        /// <summary>
        /// Get own pending memorandumSales by empoyee Id  <paramref name="employeeId"/>.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales
     
                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }
        /// <summary>
        /// Get Count pending for closing memorandumSales by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        /// <summary>
        /// Get Count pending for closing memorandumSales own <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        /// <summary>
        /// Get all memorandumSales count by Status Id.
        /// </summary>
        /// <returns></returns>
        public int getmemorandumSalesCountByStatusId(int uid, int statusId)
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

                    return _DbContext.memorandumSales

                        .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.memorandumSaleStatus.Id == statusId && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbContext.memorandumSales.Where(x => x.memorandumSaleStatus.Id == statusId && x.isApproved == true).Count();
            }
        }
        /// <summary>
        /// Get all memorandumSales count
        /// </summary>
        /// <returns></returns>
        public int getmemorandumSalesCount(int uid)
        {
            if (uid != 0)
            {
                var user = context.Users.FirstOrDefault(x => x.id == uid);
                List<int> deptIds = new List<int>();
                List<int> companyIds = new List<int>();
                foreach (var comp in user.employee.Companies)
                    companyIds.Add(comp.Id);
                foreach (var dpt in user.employee.departments)
                    deptIds.Add(dpt.Id);

                return context.memorandumSales

                    .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true)
                    .Count();
            }
            else
                return context.memorandumSales.Where(x => x.isApproved == true).Count();

        }
        /// <summary>
        /// Get all memorandumSales by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllPobyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.memorandumSales

                .Where(x => deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.memorandumSaleStatus.Id == StatusId && x.isApproved == true)
                .ToList();
        }
        /// <summary>
        /// Get all memorandumSales.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllPendingForAdministrator()
        {
            return context.memorandumSales

                .Where(x => x.isApproved == false)
                .ToList();
        }
        /// Get Count memorandumSales.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.memorandumSales

                .Where(x => x.isApproved == false)
                .Count();
        }
        /// <summary>
        /// Get Count memorandumSales.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.memorandumSales

                .Where(x => x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        /// <summary>
        /// Get all memorandumSales.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAllPendingForClosingAdministrator()
        {
            return context.memorandumSales
      
                .Where(x => x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }
        /// <summary>
        /// Get all memorandumSales.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSale> getAll()
        {
            return context.memorandumSales
         
                .ToList();
        }
        /// <summary>
        /// Get all MemorandumSales Status.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSaleStatus> getAllMemorandumSaleStatus()
        {
            return context.memorandumSaleStatuses.ToList();
        }
        /// <summary>
        /// Get all MemorandumSales Status.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSaleStatus> getAllActiveMemorandumSaleStatus()
        {
            List<MemorandumSaleStatus> statuses = new List<MemorandumSaleStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.memorandumSaleStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        /// <summary>
        /// Get all MemorandumSales Status.
        /// </summary>
        /// <returns></returns>
        public List<MemorandumSaleStatus> getAllInActiveMemorandumSaleStatus()
        {
            List<MemorandumSaleStatus> statuses = new List<MemorandumSaleStatus>();
            using (var _DbContext = new DBContextERP())
            {
                 statuses = _DbContext.memorandumSaleStatuses.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }

        /// <summary>
        /// Get all MemorandumSales Status.
        /// </summary>
        ///  <param name="memorandumSalestatusid"></param>
        /// <returns></returns>
        public MemorandumSaleStatus getstatus(int memorandumSalestatusid)
        {
            return context.memorandumSaleStatuses.FirstOrDefault(x => x.Id == memorandumSalestatusid);
        }




        /// <summary>
        /// Get all memorandumSales of specific User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<MemorandumSale> getAll(User user)
        {
            return context.memorandumSales.Where(x => x.user_Id == user.id).ToList();
        }



        ///// <summary>
        ///// Get all Item types
        ///// </summary>
        ///// <returns></returns>
        //public List<ItemType> getAllItemTypes()
        //{
        //    return  context.itemTypes.ToList();
        //}




        /// <summary>
        /// Get memorandumSale based on MemorandumSale ID.
        /// </summary>
        /// <param name="memorandumSaleId"></param>
        /// <returns></returns>
        public MemorandumSale get(int memorandumSaleId)
        {
            return context.memorandumSales
       

            .FirstOrDefault(x => x.Id == memorandumSaleId);
        }
        /// <summary>
        /// Get list of all CostSheetFields in DB
        /// </summary>
        /// <returns>List of CostSheetFields Objects</returns>
        public List<CostSheetField> getallCostSheetField()
        {
            return context.costSheetFields
                .ToList();

        }
        /// <summary>
        /// Get All Active CostSheetField
        /// </summary>
        /// <returns></returns>
        public List<CostSheetField> getActiveCostSheetFields()
        {
            return context.costSheetFields.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive CostSheetFields
        /// </summary>
        /// <returns></returns>
        public List<CostSheetField> getinActiveCostSheetFields()
        {
            return context.costSheetFields.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new CostSheetField
        /// </summary>
        /// <param name="costSheetField">CostSheetField Object</param>
        public void AddCostSheetField(CostSheetField costSheetField)
        {
            context.costSheetFields.Add(costSheetField);
            context.SaveChanges();
        }
        /// <summary>
        /// get CostSheetField by ID
        /// </summary>
        /// <param name="costSheetFieldid">CostSheetField ID</param>
        /// <returns></returns>
        public CostSheetField getCostSheetField(int costSheetFieldId)
        {
            return context.costSheetFields
                .FirstOrDefault(x => x.Id == costSheetFieldId);
        }
        /// <summarProductNature
        /// </summary>
        /// <param name="CostSheetField">CostSheetField  Object</param>
        public void UpdateCostSheetField(CostSheetField costSheetField)
        {
            CostSheetField prod = context.costSheetFields.FirstOrDefault(x => x.Id == costSheetField.Id);
            prod = costSheetField;
            context.SaveChanges();
        }
        /// <summary>
        /// Add new Comment
        /// </summary>
        /// <param name="viewInfo">viewInfo  Object</param>
        public void Add(CommentLog viewInfo)
        {
            context.CommentLogs.Add(viewInfo);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added Comment for userName= " + viewInfo.employee.person.FName + " Id= " + viewInfo.Id);

        }
        /// <summary>
        /// Add new Comment for transaction
        /// </summary>
        /// <param name="viewInfo">viewInfo  Object for Inquiry</param>
        public void Add(int Transactionid, TransactionItemType Transactiontype, string comment, int empid)
        {
            CommentLog viewInfo = new CommentLog();
            if (SystemLog.CurrentUserId != 0 && empid != 0)
            {
                viewInfo.employeeId = empid;
                viewInfo.TransactionId = Transactionid;
                viewInfo.TransactionType = Transactiontype;
                viewInfo.Timestamp = System.DateTime.Now;
                viewInfo.ReadTimestamp = System.DateTime.Now;

                //viewInfo.Info = Info.ToString();
                viewInfo.Comment = comment;
                context.CommentLogs.Add(viewInfo);

                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Added viewInfo  Id= " + viewInfo.Id);
            }
        }
        /// <summary>
        /// Get List of comments for currenttransaction
        /// </summary>
        /// 
        public List<CommentLog> getcommentslog(int TransactionId, TransactionItemType transactiontype)
        {
            return context.CommentLogs.Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();

        }
        /// <summary>
        /// Get List of comments for currenttransaction
        /// </summary>
        /// 
        public List<CommentLog> getcommentslogAsc(int TransactionId, TransactionItemType transactiontype)
        {
            return context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();

        }
        /// <summary>
        /// Get List of comments for currenttransaction
        /// </summary>
        /// 
        public List<CommentLog> getcommentslogDsc(int TransactionId, TransactionItemType transactiontype)
        {
            return context.CommentLogs.OrderByDescending(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();

        }
        
        /// <summary>
        /// Change MemorandumSale to Void
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="isVoid"></param>
        public void setMemorandumSaletoVoid(int Id, bool isVoid)
        {
            MemorandumSale item = context.memorandumSales.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            context.SaveChanges();
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
            return context.memorandumSales
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();

        }
        public int getAllActiveCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.memorandumSales
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.memorandumSaleStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
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
            return context.memorandumSales
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.memorandumSaleStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
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
            return context.memorandumSales

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
            return context.memorandumSales
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }
        public int getAllPendingForClosingDepartmentalCountByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }

            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();

            var count = context.memorandumSales
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
            return context.memorandumSales

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }
        public int getAllPendingforClosingCountownByCompany(List<Company> compId, int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> companyId = new List<int>();
            foreach (var v in compId)
            {
                companyId.Add(v.Id);
            }
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            return context.memorandumSales

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }


        public int getAllPendingForApprovalDepartmentalCountByCompanyDept(List<Company> compId, List<Department> deptId, int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
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
            return context.memorandumSales
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

            return context.memorandumSales
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.memorandumSaleStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
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
            return context.memorandumSales
            .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.memorandumSaleStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
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
            return context.memorandumSales

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
            return context.memorandumSales
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
            return context.memorandumSales

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
            return context.memorandumSales

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
            return context.memorandumSales

                .Where(x => (departmentId.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (departmentId.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();

        }

    }
}
