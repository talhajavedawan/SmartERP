using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.AssetsRentals.TenantRentals
{
    public class TenantRentalRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Get last Rental Contract
        /// </summary>
        /// <returns></returns>
        public int GetLastTransactionId()
        {
            var rentalOrder = context.tenantRentals.OrderByDescending(q => q.Id).FirstOrDefault();

            if (rentalOrder == null)
                return 0;
            return rentalOrder.transactionGroupId;
        }


        /// <summary>
        /// Add new Asset
        /// </summary>
        /// <param name="tenantRental"></param>
        public void AddTenantRental(TenantRental tenantRental)
        {
            context.tenantRentals.Add(tenantRental);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset
        /// </summary>
        /// <param name="tenantRental"></param>
        public void UpdateTenantRental(TenantRental tenantRental)
        {
            var _tenantRental = context.tenantRentals.FirstOrDefault(x => x.Id == tenantRental.Id);
            _tenantRental = tenantRental;
            context.SaveChanges();
        }

        public List<TenantRental> GetActiveTenantRentalsByCompDept(int compId, int deptId)
        {
            return context.tenantRentals.Where(x => x.companyId == compId && x.deptId == deptId && x.Status.isActive == true).ToList();
        }

        public List<AssetRental> GetActiveAssetRentalsByCompDept(int compId, int deptId)
        {
            return context.assetRentals.Where(x => x.companyId == compId && x.deptId == deptId && x.Status.isActive == true && x.isRentable == true).ToList();
        }

        public ERP_BL.Databases.Employee GetEmployee(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }

        /// <summary>
        /// Get Asset by Id
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public TenantRental GetTenantRentall(int tenantId)
        {
            return context.tenantRentals.FirstOrDefault(x => x.Id == tenantId);
        }

        /// <summary>
        /// Get All Asset Rentals
        /// </summary>
        /// <returns></returns>
        public List<TenantRental> GetAllTenantRentals()
        {
            return context.tenantRentals.ToList();
        }

        public List<TenantRental> GetAllTenantsByCompDeptAsset(int compId, int deptId, int assetId)
        {
            return context.tenantRentals.Where(x => x.companyId == compId && x.deptId == deptId /*&& x.assetRentalId == assetId*/).ToList();
        }

        /// <summary>
        /// Add New Status
        /// </summary>
        /// <param name="rentalContractStatus"></param>
        public void AddTenantStatus(TenantRentalStatus tenantRentalStatus)
        {
            context.tenantRentalStatuses.Add(tenantRentalStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Status
        /// </summary>
        /// <param name="rentalContractStatus"></param>
        public void UpdateTenantRentalStatus(TenantRentalStatus tenantRentalStatus)
        {
            TenantRentalStatus _tenantRentalStatus = context.tenantRentalStatuses.FirstOrDefault(x => x.Id == tenantRentalStatus.Id);
            _tenantRentalStatus.Status = tenantRentalStatus.Status;
            _tenantRentalStatus.isActive = tenantRentalStatus.isActive;
            _tenantRentalStatus.forecolor = tenantRentalStatus.forecolor;
            _tenantRentalStatus.backcolor = tenantRentalStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public TenantRentalStatus GetTenantRentalStatus(int statusId)
        {
            return context.tenantRentalStatuses
                //.Include("payments")
                .FirstOrDefault(x => x.Id == statusId);
        }

        /// <summary>
        /// Get All Statuses
        /// </summary>
        /// <returns></returns>
        public List<TenantRentalStatus> GetAllTenantRentalStatuses()
        {
            return context.tenantRentalStatuses
                //.Include("payments")
                .ToList();
        }

        public List<TenantRentalStatus> GetAllTenantRentalClosedStatus()
        {
            var statusList = context.tenantRentalStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;
        }

        public List<TenantRentalStatus> GetAllTenantRentalOpenStatus()
        {
            var statusList = context.tenantRentalStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;
        }


        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<TenantRental> GetAllActiveTenantRentals(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.tenantRentals

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.Status.isActive == true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
           .ToList();
        }

        /// <summary>
        /// Get Count pending Inter-Bank Transfers by Departments
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

            return context.tenantRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending Inter-Bank Transfers by supervisor Id and <paramref name="StatusId"/>.
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

            return context.tenantRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Own Count pending Inter-Bank Transfers
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

            return context.tenantRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }



        /// <summary>
        /// Get Count pending Inter-Bank Transfers by Departments
        /// <returns></returns>
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.tenantRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending Inter-Bank Transfers by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.tenantRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Own Count pending Inter-Bank Transfers user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.tenantRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }

        /// <summary>
        /// Get all Void Inter-Bank Transfer for user count. 
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
            return context.tenantRentals
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.tenantRentals
    .Where(x => x.isVoid == true)
    .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfers for user count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tenantRentals
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all Inter-Bank Transfer  own count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.tenantRentals
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// Get Count Inter-Bank Transfer.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.tenantRentals

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfer .
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.tenantRentals
                .Where(x => x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending Inter-Bank Transfer by Departmental Count
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

            return context.tenantRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing Inter-Bank Transfer by supervisor Id and <paramref name="StatusId"/>.
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

            return context.tenantRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing Inter-Bank Transfers own <paramref name="StatusId"/>.
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

            return context.tenantRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.tenantRentals

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get All Pending For Approval Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<TenantRental> GetAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.tenantRentals

        .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
         .ToList();
        }


        /// <summary>
        /// Rental Contracts Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<TenantRental> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.tenantRentals

         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
             .ToList();

        }


        /// <summary>
        /// Get all pending for closing Tenant Rentals by Departmental
        /// </summary>
        /// <returns></returns>
        public List<TenantRental> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.tenantRentals
            .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.PendingForClosing == true && x.isVoid != true)
            .ToList();
        }


        /// <summary>
        /// Get All Tenant Rentals
        /// </summary>
        /// <returns></returns>
        public List<TenantRental> GetAllTenantRentals(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
            {
                companyIds.Add(comp.Id);

            }
            return context.tenantRentals

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isVoid != true)
           .ToList();


        }

        /// <summary>
        /// Get all Void Tenant Rental own.
        /// </summary>
        /// <returns></returns>
        public List<TenantRental> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.tenantRentals
         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
             .ToList();


        }

    }
}
