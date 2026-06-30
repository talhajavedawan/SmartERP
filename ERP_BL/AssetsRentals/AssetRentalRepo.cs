using ERP_BL.AssetsRentals.RentalContracts;
using ERP_BL.Countryy;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.AssetsRentals
{
    public class AssetRentalRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Get last Rental Contract
        /// </summary>
        /// <returns></returns>
        public int GetLastTransactionId()
        {
            var assetRental = context.assetRentals.OrderByDescending(q => q.Id).FirstOrDefault();

            if (assetRental == null)
                return 0;
            return assetRental.transactionGroupId;
        }


        /// <summary>
        /// Add new Asset
        /// </summary>
        /// <param name="assetRental"></param>
        public void AddAssetRental(AssetRental assetRental)
        {
            context.assetRentals.Add(assetRental);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset
        /// </summary>
        /// <param name="assetRental"></param>
        public void UpdateAssetRental(AssetRental assetRental)
        {
            var _assetRental = context.assetRentals.FirstOrDefault(x=>x.Id == assetRental.Id);
            _assetRental = assetRental;
            context.SaveChanges();
        }

        public ERP_BL.Databases.Employee GetEmployee(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }

        public List<AssetRental> GetActiveAssetRentalsByCompDept(int compId, int deptId)
        {
            return context.assetRentals.Where(x => x.companyId == compId && x.deptId == deptId && x.Status.isActive == true && x.isRentable == true).ToList();
        }

        public List<Vendor> GetActiveVendorsByCompDept(int compId, int deptId)
        {
            return context.Vendors.Where(x => x.Companies.FirstOrDefault(y=>y.Id == compId) != null && x.departments.FirstOrDefault(y => y.Id == deptId) != null && x.isActive == true).ToList();
        }

        /// <summary>
        /// Get Asset by Id
        /// </summary>
        /// <param name="assetId"></param>
        /// <returns></returns>
        public AssetRental GetAssetRental(int assetId)
        {
            return context.assetRentals.FirstOrDefault(x => x.Id == assetId);
        }

        /// <summary>
        /// Get Banks by Company and Main Bank
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public List<Bank> GetAllBranchesByCompanyBank(int companyId, int bankId)
        {
            return context.banks.Where(x=>x.companies.FirstOrDefault(y=>y.Id == companyId) != null && x.bankId == bankId).ToList();
        }

        /// <summary>
        /// Get Accounts by Company, Dept and Bank
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="deptId"></param>
        /// <param name="branchId"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public List<Account> GetAllAccountsByCompDeptBanks(int companyId, int deptId, int bankId, int branchId)
        {
            var accounts = context.accounts.Where(x => x.company.Id == companyId
            && x.departments.FirstOrDefault(y => y.Id == deptId) != null
            && x.mainBankId == bankId
            && x.bank.Id == branchId).ToList();
            return accounts;
        }

        /// <summary>
        /// Get All Banks
        /// </summary>
        /// <returns></returns>
        public List<MainBank> GetAllBanks()
        {
            return context.mainBanks.ToList();
        }

        /// <summary>
        /// Get All Collection Methods
        /// </summary>
        /// <returns></returns>
        public List<CollectionMethod> GetAllCollectionMethods()
        {
            return context.collectionMethods.ToList();
        }

        

       

        


       



        /// <summary>
        /// Add New Status
        /// </summary>
        /// <param name="rentalContractStatus"></param>
        public void AddAssetStatus(AssetRentalStatus assetRentalStatus)
        {
            context.assetRentalStatuses.Add(assetRentalStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Status
        /// </summary>
        /// <param name="rentalContractStatus"></param>
        public void UpdateAssetRentalStatus(AssetRentalStatus assetRentalStatus)
        {
            AssetRentalStatus _assetRentalStatus = context.assetRentalStatuses.FirstOrDefault(x => x.Id == assetRentalStatus.Id);
            _assetRentalStatus.Status = assetRentalStatus.Status;
            _assetRentalStatus.isActive = assetRentalStatus.isActive;
            _assetRentalStatus.forecolor = assetRentalStatus.forecolor;
            _assetRentalStatus.backcolor = assetRentalStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public AssetRentalStatus GetAssetRentalStatus(int statusId)
        {
            return context.assetRentalStatuses
                //.Include("payments")
                .FirstOrDefault(x => x.Id == statusId);
        }


        /// <summary>
        /// Get All Statuses
        /// </summary>
        /// <returns></returns>
        public List<AssetRentalStatus> GetAllAssetRentalStatuses()
        {
            return context.assetRentalStatuses
                //.Include("payments")
                .ToList();
        }

        public List<AssetRentalStatus> GetAllAssetRentalClosedStatus()
        {
            var statusList = context.assetRentalStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }

        public List<AssetRentalStatus> GetAllAssetRentalOpenStatus()
        {
            var statusList = context.assetRentalStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }

        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<AssetRental> GetAllActiveAssetRentals(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.assetRentals

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

            return context.assetRentals

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

            return context.assetRentals

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

            return context.assetRentals

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
            return context.assetRentals

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
            return context.assetRentals

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
            return context.assetRentals

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
            return context.assetRentals
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.assetRentals
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

            return context.assetRentals
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
            return context.assetRentals
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// Get Count Inter-Bank Transfer.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.assetRentals

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfer .
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.assetRentals
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

            return context.assetRentals

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

            return context.assetRentals

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

            return context.assetRentals

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.assetRentals

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get All Pending For Approval Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<AssetRental> GetAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.assetRentals

        .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
         .ToList();
        }


        /// <summary>
        /// Rental Contracts Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<AssetRental> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.assetRentals

         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
             .ToList();

        }


        /// <summary>
        /// Get all pending for closing Rental Contracts by Departmental
        /// </summary>
        /// <returns></returns>
        public List<AssetRental> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.assetRentals
            .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.PendingForClosing == true && x.isVoid != true)
            .ToList();
        }


        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<AssetRental> GetAllAssetRentals(int uid)
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
            return context.assetRentals

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isVoid != true)
           .ToList();


        }

        /// <summary>
        /// Get all Void Rental Contract own.
        /// </summary>
        /// <returns></returns>
        public List<AssetRental> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.assetRentals
         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
             .ToList();


        }

        /// <summary>
        /// Add Asset Type
        /// </summary>
        /// <param name="assetType"></param>
        public void AddAssetType(AssetType assetType)
        {
            context.assetTypes.Add(assetType);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset Type
        /// </summary>
        /// <param name="assetType"></param>
        public void UpdateAssetType(AssetType assetType)
        {
            var _assetType = context.assetTypes.FirstOrDefault(x=>x.Id == assetType.Id);
            _assetType = assetType;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Asset Type
        /// </summary>
        /// <param name="assetTypeId"></param>
        public AssetType GetAssetType(int assetTypeId)
        {
            return context.assetTypes.FirstOrDefault(x=>x.Id == assetTypeId);
        }

        /// <summary>
        /// Get All Asset Types
        /// </summary>
        /// <param name="assetTypeId"></param>
        public List<AssetType> GetAllAssetType()
        {
            return context.assetTypes.ToList();
        }


        /// <summary>
        /// Add Asset Number
        /// </summary>
        /// <param name="assetNumber"></param>
        public void AddAssetNumber(AssetNumber assetNumber)
        {
            context.assetNumbers.Add(assetNumber);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset Number
        /// </summary>
        /// <param name="assetNumber"></param>
        public void UpdateAssetNumber(AssetNumber assetNumber)
        {
            var _assetNumber = context.assetNumbers.FirstOrDefault(x => x.Id == assetNumber.Id);
            _assetNumber = assetNumber;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Asset Number
        /// </summary>
        /// <param name="assetNumberId"></param>
        public AssetNumber GetAssetNumber(int assetNumberId)
        {
            return context.assetNumbers.FirstOrDefault(x => x.Id == assetNumberId);
        }

        /// <summary>
        /// Get All Asset Numbers
        /// </summary>
        /// <param name="assetNumberId"></param>
        public List<AssetNumber> GetAllAssetNumber()
        {
            return context.assetNumbers.ToList();
        }


        /// <summary>
        /// Add Asset Nature
        /// </summary>
        /// <param name="assetNature"></param>
        public void AddAssetNature(RentalAssetNature assetNature)
        {
            context.rentalAssetNatures.Add(assetNature);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset Nature
        /// </summary>
        /// <param name="assetNature"></param>
        public void UpdateAssetNature(RentalAssetNature assetNature)
        {
            var _assetNature = context.rentalAssetNatures.FirstOrDefault(x => x.Id == assetNature.Id);
            _assetNature = assetNature;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Asset Nature
        /// </summary>
        /// <param name="assetNatureId"></param>
        public RentalAssetNature GetAssetNature(int assetNatureId)
        {
            return context.rentalAssetNatures.FirstOrDefault(x => x.Id == assetNatureId);
        }

        /// <summary>
        /// Get All Asset Natures
        /// </summary>
        /// <param name="assetNatureId"></param>
        public List<RentalAssetNature> GetAllAssetNature()
        {
            return context.rentalAssetNatures.ToList();
        }

        /// <summary>
        /// Add Asset Brand
        /// </summary>
        /// <param name="assetBrand"></param>
        public void AddAssetBrand(AssetBrand assetBrand)
        {
            context.assetBrands.Add(assetBrand);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset Brand
        /// </summary>
        /// <param name="assetBrand"></param>
        public void UpdateAssetBrand(AssetBrand assetBrand)
        {
            var _assetBrand = context.assetBrands.FirstOrDefault(x => x.Id == assetBrand.Id);
            _assetBrand = assetBrand;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Asset Brand
        /// </summary>
        /// <param name="assetBrandId"></param>
        public AssetBrand GetAssetBrand(int assetBrandId)
        {
            return context.assetBrands.FirstOrDefault(x => x.Id == assetBrandId);
        }

        /// <summary>
        /// Get All Asset Brands
        /// </summary>
        /// <param name="assetBrandId"></param>
        public List<AssetBrand> GetAllAssetBrand()
        {
            return context.assetBrands.ToList();
        }


        /// <summary>
        /// Add Asset SubNature
        /// </summary>
        /// <param name="rentalAssetSubNature"></param>
        public void AddRentalAssetSubNature(RentalAssetSubNature assetSubNature)
        {
            context.assetSubNatures.Add(assetSubNature);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset SubNature
        /// </summary>
        /// <param name="assetSubNature"></param>
        public void UpdateRentalAssetSubNature(RentalAssetSubNature assetSubNature)
        {
            var _assetSubNature = context.assetSubNatures.FirstOrDefault(x => x.Id == assetSubNature.Id);
            _assetSubNature = assetSubNature;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Asset SubNature
        /// </summary>
        /// <param name="assetSubNatureId"></param>
        public RentalAssetSubNature GetRentalAssetSubNature(int assetSubNatureId)
        {
            return context.assetSubNatures.FirstOrDefault(x => x.Id == assetSubNatureId);
        }

        /// <summary>
        /// Get All Asset SubNatures
        /// </summary>
        /// <param name="assetSubNatureId"></param>
        public List<RentalAssetSubNature> GetAllRentalAssetSubNature()
        {
            return context.assetSubNatures.ToList();
        }


        /// <summary>
        /// Add Asset Model
        /// </summary>
        /// <param name="assetModel"></param>
        public void AddAssetModel(AssetModel assetModel)
        {
            context.assetModels.Add(assetModel);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset Model
        /// </summary>
        /// <param name="assetModel"></param>
        public void UpdateAssetModel(AssetModel assetModel)
        {
            var _assetModel = context.assetModels.FirstOrDefault(x => x.Id == assetModel.Id);
            _assetModel = assetModel;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Asset Model
        /// </summary>
        /// <param name="assetModelId"></param>
        public AssetModel GetAssetModel(int assetModelId)
        {
            return context.assetModels.FirstOrDefault(x => x.Id == assetModelId);
        }

        /// <summary>
        /// Get All Asset Models
        /// </summary>
        /// <param name="assetModelId"></param>
        public List<AssetModel> GetAllAssetModel()
        {
            return context.assetModels.ToList();
        }

        /// <summary>
        /// Add Asset Type
        /// </summary>
        /// <param name="assetRentalLocation"></param>
        public void AddAssetRentalLocation(AssetRentalLocation assetRentalLocation)
        {
            context.assetRentalLocations.Add(assetRentalLocation);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset Type
        /// </summary>
        /// <param name="assetRentalLocation"></param>
        public void UpdateAssetRentalLocation(AssetRentalLocation assetRentalLocation)
        {
            var _assetRentalLocation = context.assetRentalLocations.FirstOrDefault(x => x.Id == assetRentalLocation.Id);
            _assetRentalLocation = assetRentalLocation;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Asset Type
        /// </summary>
        /// <param name="assetRentalLocationId"></param>
        public AssetRentalLocation GetAssetRentalLocation(int assetRentalLocationId)
        {
            return context.assetRentalLocations.FirstOrDefault(x => x.Id == assetRentalLocationId);
        }

        /// <summary>
        /// Get All Asset Types
        /// </summary>
        /// <param name="assetRentalLocationId"></param>
        public List<AssetRentalLocation> GetAllAssetRentalLocation()
        {
            return context.assetRentalLocations.ToList();
        }

        /// <summary>
        /// Get All Asset Types
        /// </summary>
        /// <param name="assetRentalLocationId"></param>
        public List<AssetRentalLocation> GetAllAssetRentalLocationByCountryCity(Countryy.Country country, City city)
        {
            return context.assetRentalLocations.Where(x=>x.countryId == country.Id && x.cityId == city.Id).ToList();
        }


        /// <summary>
        /// Add Asset Type
        /// </summary>
        /// <param name="assetRentalUnit"></param>
        public void AddAssetRentalUnit(AssetRentalUnit assetRentalUnit)
        {
            context.assetRentalUnits.Add(assetRentalUnit);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Asset Type
        /// </summary>
        /// <param name="assetRentalUnit"></param>
        public void UpdateAssetRentalUnit(AssetRentalUnit assetRentalUnit)
        {
            var _assetRentalUnit = context.assetRentalUnits.FirstOrDefault(x => x.Id == assetRentalUnit.Id);
            _assetRentalUnit = assetRentalUnit;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Asset Type
        /// </summary>
        /// <param name="assetRentalUnitId"></param>
        public AssetRentalUnit GetAssetRentalUnit(int assetRentalUnitId)
        {
            return context.assetRentalUnits.FirstOrDefault(x => x.Id == assetRentalUnitId);
        }

        /// <summary>
        /// Get All Asset Types
        /// </summary>
        /// <param name="assetRentalUnitId"></param>
        public List<AssetRentalUnit> GetAllAssetRentalUnit()
        {
            return context.assetRentalUnits.ToList();
        }

        /// <summary>
        /// Get All Asset Types
        /// </summary>
        /// <param name="assetRentalUnitId"></param>
        public List<AssetRentalUnit> GetAllAssetRentalUnitByCountryCityLocation(Countryy.Country country, City city, AssetRentalLocation location)
        {
            return context.assetRentalUnits.Where(x => x.countryId == country.Id && x.cityId == city.Id && x.assetRentalLocationId == location.Id).ToList();
        }

    }
}
