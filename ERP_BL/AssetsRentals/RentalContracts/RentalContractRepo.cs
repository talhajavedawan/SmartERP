using ERP_BL.AssetsRentals.TenantRentals;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.AssetsRentals.RentalContracts
{
    public class RentalContractRepo
    {
        DBContextERP context = new DBContextERP();

        public ERP_BL.Databases.Employee GetEmployee(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }

        /// <summary>
        /// Get last Rental Contract
        /// </summary>
        /// <returns></returns>
        public int GetLastTransactionId()
        {
            var rentalContract = context.rentalContracts.OrderByDescending(q => q.Id).FirstOrDefault();

            if (rentalContract == null)
                return 0;
            return rentalContract.transactionGroupId;
        }

        public List<AssetRental> GetActiveAssetRentalsByCompDept(int compId, int deptId)
        {
            return context.assetRentals.Where(x => x.companyId == compId && x.deptId == deptId && x.Status.isActive == true && x.isRentable == true).ToList();
        }

        public List<TenantRental> GetAllTenantsByCompDeptAsset(int compId, int deptId, int assetId)
        {
            return context.tenantRentals.Where(x => x.companyId == compId && x.deptId == deptId /*&& x.assetRentalId == assetId*/).ToList();
        }

        /// <summary>
        /// Add new Contract
        /// </summary>
        /// <param name="rentalContract"></param>
        public void AddRentalContract(RentalContract rentalContract)
        {
            //rentalContract.assetRental = context.assetRentals.FirstOrDefault(x=>x.Id == rentalContract.assetRentalId);
            //rentalContract.tenantRental = context.tenantRentals.FirstOrDefault(x => x.Id == rentalContract.tenantRentalId);
            context.rentalContracts.Add(rentalContract);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contract
        /// </summary>
        /// <param name="rentalContract"></param>
        public void UpdateRentalContract(RentalContract rentalContract)
        {
            var _rentalContract = context.rentalContracts.FirstOrDefault(x => x.Id == rentalContract.Id);
            _rentalContract = rentalContract;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Contract by Id
        /// </summary>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public RentalContract GetRentalContract(int contractId)
        {
            return context.rentalContracts.FirstOrDefault(x => x.Id == contractId);
        }

        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalContract> GetAllRentalContracts()
        {
            return context.rentalContracts.ToList();
        }


        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalContract> GetAllActiveRentalContracts(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.rentalContracts

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.Status.isActive == true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
           .ToList();


        }



        /// <summary>
        /// Add New Status
        /// </summary>
        /// <param name="rentalContractStatus"></param>
        public void AddRentalContractStatus(RentalContractStatus rentalContractStatus)
        {
            context.rentalContractStatuses.Add(rentalContractStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Status
        /// </summary>
        /// <param name="rentalContractStatus"></param>
        public void UpdateRentalContractStatus(RentalContractStatus rentalContractStatus)
        {
            RentalContractStatus _rentalContractStatus = context.rentalContractStatuses.FirstOrDefault(x => x.Id == rentalContractStatus.Id);
            _rentalContractStatus.Status = rentalContractStatus.Status;
            _rentalContractStatus.isActive = rentalContractStatus.isActive;
            _rentalContractStatus.forecolor = rentalContractStatus.forecolor;
            _rentalContractStatus.backcolor = rentalContractStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public RentalContractStatus GetRentalContractStatus(int statusId)
        {
            return context.rentalContractStatuses
                //.Include("payments")
                .FirstOrDefault(x => x.Id == statusId);
        }


        /// <summary>
        /// Get All Statuses
        /// </summary>
        /// <returns></returns>
        public List<RentalContractStatus> GetAllRentalContractStatuses()
        {
            return context.rentalContractStatuses
                //.Include("payments")
                .ToList();
        }

        public List<RentalContractStatus> GetAllClosedStatus()
        {
            var statusList = context.rentalContractStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }

        public List<RentalContractStatus> GetAllOpenStatus()
        {
            var statusList = context.rentalContractStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

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

            return context.rentalContracts

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

            return context.rentalContracts

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

            return context.rentalContracts

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
            return context.rentalContracts

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
            return context.rentalContracts

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
            return context.rentalContracts

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
            return context.rentalContracts
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.rentalContracts
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

            return context.rentalContracts
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
            return context.rentalContracts
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// Get Count Inter-Bank Transfer.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.rentalContracts

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfer .
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.rentalContracts
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

            return context.rentalContracts

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

            return context.rentalContracts

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

            return context.rentalContracts

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.rentalContracts

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get All Pending For Approval Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalContract> GetAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.rentalContracts

        .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
         .ToList();
        }


        /// <summary>
        /// Rental Contracts Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<RentalContract> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.rentalContracts

         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
             .ToList();

        }


        /// <summary>
        /// Get all pending for closing Rental Contracts by Departmental
        /// </summary>
        /// <returns></returns>
        public List<RentalContract> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.rentalContracts
            .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.PendingForClosing == true && x.isVoid != true)
            .ToList();
        }


        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalContract> GetAllRentalContracts(int uid)
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
            return context.rentalContracts

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isVoid != true)
           .ToList();


        }

        /// <summary>
        /// Get all Void Rental Contract own.
        /// </summary>
        /// <returns></returns>
        public List<RentalContract> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.rentalContracts
         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
             .ToList();


        }
    }
}
