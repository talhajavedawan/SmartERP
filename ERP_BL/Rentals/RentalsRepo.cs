using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Rentals
{
    public class RentalsRepo
    {
        DBContextERP context = new DBContextERP();

        public List<RentalAssetStatus> GetAllCloseRentaltStatus()
        {
            var statusList = context.RentalAssetStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;
        }

        public List<RentalAssetMethod> GetAllRentalType()
        {
            return context.RentalAssetMethods
                .ToList();
        }
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="add Owner"></param>
        public void AddOwner(AssetOwner owner)
        {
            if (owner == null)
                throw new NullReferenceException("Object can not be null");
            context.assetOwners.Add(owner);
            context.SaveChanges();
        }

        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="add"></param>
        public void AddRentalAssets(RentalAssets rent)
        {

            if (rent == null)
                throw new NullReferenceException("Object can not be null");

            var units = rent.assetUnits;
            if (rent.assetUnits != null)
            {
                rent.assetUnits = new List<Asset>();
                foreach (var _unit in units)
                {
                    rent.assetUnits.Add(context.Assets.FirstOrDefault(x => x.Id == _unit.Id));
                }
            }

            context.RentalAssets.Add(rent);
            context.SaveChanges();


        }
        public void addRentalAmount(List<RentalReceiveAmount> rentalReceiveAmount)
        {
            if (rentalReceiveAmount == null)
                throw new NullReferenceException("Object can not be null");

            foreach (var rent in rentalReceiveAmount)
            {
                context.rentalReceiveAmounts.Add(rent);
            }

            context.SaveChanges();
        }
        ///// <summary>
        ///// Add New Method
        ///// </summary>
        ///// <param name="add"></param>
        //public void AddNotOwnedRentalAssets(RentalAssets notOwnedAsset)  
        //{
        //    if (notOwnedAsset == null)
        //        throw new NullReferenceException("Object can not be null");

        //    context.RentalAssets.Add(notOwnedAsset);
        //    context.SaveChanges();
        //}
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="add"></param>
        public void AddTenancyContract(TenancyContract tenancy)
        {
            if (tenancy == null)
                throw new NullReferenceException("Object can not be null");
            // var rentalAsset = context.Assets.FirstOrDefault(x=>x.Id == tenancy.AssetId);
            //// rentalAsset.isRented = true;
            // if (tenancy.isActive == true)
            //     rentalAsset.isRented = true;
            // else rentalAsset.isRented = false;

            var notOwnedUnits = tenancy.assetUnits;



            if (tenancy.assetUnits != null)
            {
                tenancy.assetUnits = new List<Asset>();
                foreach (var _unit in notOwnedUnits)
                {
                    var asset = context.Assets.FirstOrDefault(x => x.Id == _unit.Id);
                    asset.isRented = true;
                    tenancy.assetUnits.Add(asset);
                }
            }



            context.tenancyContracts.Add(tenancy);
            context.SaveChanges();
        }

        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="addTenant"></param>
        public void AddTenant(Tenant tenant)
        {
            if (tenant == null)
                throw new NullReferenceException("Object can not be null");
            context.tenants.Add(tenant);
            context.SaveChanges();

        }
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="addNotOwnedRA"></param>
        public void AddNotOwnedRA(Asset notOwned)
        {
            if (notOwned == null)
                throw new NullReferenceException("Object can not be null");
            context.Assets.Add(notOwned);
            context.SaveChanges();

        }

        //public List<RentalAssets> getParentRentalAssets()
        //{
        //    return context.RentalAssets.Where(x => x.AssetId == null).ToList();
        //}
        //public List<RentalAssets> getRentalAssetsByParentId(int ParentId)
        //{
        //    return context.RentalAssets.Where(x => x.AssetId == ParentId).ToList();
        //}
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="Get"></param>
        public List<Asset> getNotOwnedRentalAssets()
        {
            return context.Assets.Where(x => x.isOwned == false)

                .ToList();
        }
        public List<Asset> getNotOwnedRentalAssetss(int cmpID, int deptID, int NatureID)
        {
            return context.Assets
              .Where(x => x.companyId == cmpID && x.deptId == deptID && x.AssetNatureId == NatureID && x.isOwned == false)
              .ToList();
        }
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="Get"></param>
        public List<RentalAssets> getRentalAssets()
        {
            return context.RentalAssets
               .Where(x => x.AssetType == Enums.RentalAssetType.Owned)
                //.Include("NotOwnedasset")
                // .Include("NotOwnedassetNature")
                //.Include("Unit")
                .ToList();

            // return context.RentalAssets.Where(x => x.isSubsidary == true).FirstOrDefault();

            //*************get tenant name based on company and department********************
        }


        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="Get"></param>
        public List<RentalAssets> getAllNotOwnedRentalAssets()
        {
            return context.RentalAssets

                      .Where(x => x.AssetType == Enums.RentalAssetType.NotOwned)

                .ToList();
        }
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="Get"></param>
        public List<RentalAssets> getAllRentalAssets()
        {
            return context.RentalAssets

                .ToList();
        }

        public List<Tenant> GetTenantNameByCompDept(int compId, int deptId)
        {
            return context.tenants
                .Where(x => x.company.Id == compId && x.department.Id == deptId && x.isActive == true).ToList();
        }

        //*************Get Asset Name based on company department and isRented is false************************
        public List<RentalAssets> GetAssetNameByCompDept(int compId, int deptId, Enums.RentalAssetType assetType)
        {
            return context.RentalAssets
                .Where(x => x.company.Id == compId && x.department.Id == deptId && x.AssetType == assetType).ToList();
        }

        public List<Asset> GetFixedAssets(int AssetNatureId, int cmpID, int depId)
        {
            return context.Assets
                .Where(x => x.AssetNature.Id == AssetNatureId && x.OwnerCompany.Id == cmpID && x.managingDept.Id == depId && x.isRentable == true && x.parentId == null && x.isOwned == true)
                .ToList();
        }
        public List<Asset> GetFixedAssets(int parentId)
        {
            //return context.Assets.Include("OwnerCompany").Include("managingDept").Include("assetOwner").Include("CoassetOwner").Include("parentAsset").Include("AssetNature").Where(x => x.isOwned == false)
            return context.Assets
                .Where(x => x.parentId == parentId && x.isOwned == true)//notowned and isowned is true i make it false for testing
                .ToList();
        }
        public List<Asset> GetFixedAssetsNotOwned(int parentId)
        {
            //return context.Assets.Include("OwnerCompany").Include("managingDept").Include("assetOwner").Include("CoassetOwner").Include("parentAsset").Include("AssetNature").Where(x => x.isOwned == false)
            return context.Assets
                .Where(x => x.parentId == parentId && x.isOwned == false)//notowned and isowned is true i make it false for testing
                .ToList();
        }
        public List<Asset> GetFixedAssetsbyComDept(int cmpID, int depId)
        {
            return context.Assets
                .Where(x => x.OwnerCompany.Id == cmpID && x.managingDept.Id == depId && x.isRentable == true)
                .ToList();
        }
        public List<Asset> GetNotOwnedAsset(int AssetNatureId, int cmpID, int depId)
        {

            return context.Assets
               .Where(x => x.AssetNatureId == AssetNatureId && x.companyId == cmpID && x.deptId == depId && x.isRentable == true && x.parentId == null && x.isOwned == false)
               .ToList();
        }

        public List<Asset> getSubsidary(int id)
        {
            return context.Assets.Where(x => x.isSubsidary == true && x.parentId == id).ToList();
        }

        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetTenant"></param> 
        public List<Tenant> getAllTenant()
        {
            return context.tenants

                .ToList();
        }
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetOwner"></param> 
        public List<AssetOwner> getAllOwner()
        {
            return context.assetOwners
              .Where(x => x.isActive == true)
                .ToList();

        }


        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetTenancyContract"></param> 
        public List<TenancyContract> getAllTenancyContract()
        {
            return context.tenancyContracts

                .ToList();

            // return context.RentalAssets.Where(x => x.isSubsidary == true).FirstOrDefault();

        }
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetTenancyContract"></param> 
        public List<TenancyContract> getAllActiveTenancyContract()
        {
            return context.tenancyContracts
          .Where(x => x.isActive == true)
                .ToList();

            // return context.RentalAssets.Where(x => x.isSubsidary == true).FirstOrDefault();

        }

        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetTenancyContract"></param> 
        public List<TenancyContract> GetTenancyContracts(int AssetId, int? TcId)
        {
            return context.tenancyContracts

               .Where(x => x.AssetId == AssetId && x.Id == TcId && x.isActive == true)

                .ToList();

            // return context.RentalAssets.Where(x => x.isSubsidary == true).FirstOrDefault();

        }
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetTenancyContract"></param> 
        public List<TenancyContract> GetTenancyContractt(int cmpId, Enums.RentalAssetType assetType)
        {
            return context.tenancyContracts



               .Where(x => x.NotOwnedAssetId == cmpId && x.isActive == true && x.AssetType == assetType)

                .ToList();

            // return context.RentalAssets.Where(x => x.isSubsidary == true).FirstOrDefault();

        }
        public List<TenancyContract> GetTenancyContract(int cmpId, Enums.RentalAssetType assetType)
        {
            return context.tenancyContracts

               .Where(x => x.AssetId == cmpId && x.isActive == true && x.AssetType == assetType)

                .ToList();

            // return context.RentalAssets.Where(x => x.isSubsidary == true).FirstOrDefault();

        }


        public List<RentalReceiveAmount> getTenancyContract()
        {
            return context.rentalReceiveAmounts

                .ToList();

            // return context.RentalAssets.Where(x => x.isSubsidary == true).FirstOrDefault();

        }

        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetTenantName"></param> 
        public List<Tenant> getAllTenantName(int cmpId, int deptId)
        {
            return context.tenants

                .Where(x => x.company.Id == cmpId && x.department.Id == deptId && x.isActive == true)
                .ToList();

        }


        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetTenantAsset"></param> 
        public List<RentalAssets> getAllTenantAsset(int cmpId, int deptId, Enums.RentalAssetType assetType)
        {
            // var old = context.tenancyContracts.FirstOrDefault(x=>x.isActive==false);

            return context.RentalAssets


                    .Where(x => x.company.Id == cmpId && x.department.Id == deptId && x.AssetType == assetType)
                .ToList();

        }
        public List<RentalAssets> getAllTenantAssets(int cmpId)
        {
            // var old = context.tenancyContracts.FirstOrDefault(x=>x.isActive==false);

            return context.RentalAssets

            .Where(x => x.Id == cmpId)
                .ToList();

        }

        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetTenantAsset"></param> 
        public List<RentalAssets> getAllTenantUnitAsset(int ParentId)
        {
            // var old = context.tenancyContracts.FirstOrDefault(x=>x.isActive==false);

            return context.RentalAssets

           .Where(x => x.Id == ParentId)
                .ToList();

        }

        public List<Asset> getAllTenantAsset(int parentId)
        {
            // var old = context.tenancyContracts.FirstOrDefault(x=>x.isActive==false);

            return context.Assets



                    .Where(x => x.parentId == parentId)
                .ToList();

        }
        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="GetTenantAsset"></param> 
        public List<RentalAssets> getAllOwnerAsset(int cmpId, int deptId)
        {
            // var old = context.tenancyContracts.FirstOrDefault(x=>x.isActive==false);

            return context.RentalAssets



                   .Where(x => x.company.Id == cmpId && x.department.Id == deptId && x.AssetType == Enums.RentalAssetType.NotOwned)
                .ToList();

        }


        /// <summary>
        /// Get Tenant
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Tenant GetTenant(int Id)
        {
            return context.tenants.FirstOrDefault(x => x.Id == Id);
        }
        /// <summary>
        /// Get AssetOwner
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AssetOwner GetAssetOwner(int Id)
        {
            return context.assetOwners.FirstOrDefault(x => x.Id == Id);
        }
        /// <summary>
        /// Get Payee
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TenancyContract GetTenancyContract(int Id)
        {
            return
                context.tenancyContracts

                .FirstOrDefault(x => x.Id == Id);

        }
        /// <summary>
        /// update NotOwned  object details
        /// </summary>
        /// <param name="notOwnedRentalAsset">Role Object</param>
        public void Update(Asset notOwnedRentalAsset)
        {
            if (notOwnedRentalAsset == null)
                throw new NullReferenceException("Object can not be null");

            var AssetToUpdate = context.Assets.FirstOrDefault(x => x.Id == notOwnedRentalAsset.Id);
            AssetToUpdate = notOwnedRentalAsset;
            //AssetToUpdate.companyId = notOwnedRentalAsset.companyId;

            //AssetToUpdate.companyId = notOwnedRentalAsset.companyId;
            //AssetToUpdate.deptId = notOwnedRentalAsset.deptId;

            //AssetToUpdate.AssetNatureId = notOwnedRentalAsset.AssetNatureId;

            //AssetToUpdate.AssetName = notOwnedRentalAsset.AssetName;


            //AssetToUpdate.parentId = notOwnedRentalAsset.parentId;

            //AssetToUpdate.OwnerId = notOwnedRentalAsset.OwnerId;

            //AssetToUpdate.CoOwnerAssetId = notOwnedRentalAsset.CoOwnerAssetId;
            //AssetToUpdate.isRentable = notOwnedRentalAsset.isRentable;
            context.SaveChanges();

        }

        public void updateTenant(Tenant tenant)
        {
            if (tenant == null)
                throw new NullReferenceException("Object can not be null");
            var _tenant = context.tenants.FirstOrDefault(x => x.Id == tenant.Id);


            _tenant.companyId = tenant.companyId;
            _tenant.deptId = tenant.deptId;
            //_tenant.AssetNatureId = tenant.AssetNatureId;
            //_tenant.assetId = tenant.assetId;
            //_tenant.unitId = tenant.unitId;

            if (tenant.address != null)
            {
                _tenant.address = tenant.address;
            }

            _tenant.contact = tenant.contact;
            _tenant.CreationDate = tenant.CreationDate;

            _tenant.isActive = tenant.isActive;
            _tenant.person = tenant.person;
            _tenant.Profession = tenant.Profession;
            //_tenant.tenancyType = tenant.tenancyType;

            context.SaveChanges();

        }
        //public void UpdateRentalAssets(RentalAssets rentalAssets)
        //{
        //    if (rentalAssets == null)
        //        throw new NullReferenceException("Object can not be null");
        //    var _rentalAssets = context.RentalAssets.FirstOrDefault(x => x.Id == rentalAssets.Id);
        //    _rentalAssets.CreationDate = rentalAssets.CreationDate;

        //    _rentalAssets.notOwnedAssetId = rentalAssets.notOwnedAssetId;
        //    _rentalAssets.companyId = rentalAssets.companyId;
        //    _rentalAssets.deptId = rentalAssets.deptId;
        //    _rentalAssets.AssetNatureId = rentalAssets.AssetNatureId;
        //    _rentalAssets.assetId = rentalAssets.assetId;
        //    _rentalAssets.assetUnits = rentalAssets.assetUnits;
        //    //_rentalAssets.NotOwnedunit = rentalAssets.NotOwnedunit;
        //    context.SaveChanges();

        //}
        public void UpdateRentalAssets(RentalAssets rentalAssets)
        {
            if (rentalAssets == null)
                throw new NullReferenceException("Object can not be null");
            var _rentalAssets = context.RentalAssets.FirstOrDefault(x => x.assetId == rentalAssets.assetId);
            _rentalAssets.CreationDate = rentalAssets.CreationDate;
            _rentalAssets.companyId = rentalAssets.companyId;
            _rentalAssets.deptId = rentalAssets.deptId;
            _rentalAssets.AssetNatureId = rentalAssets.AssetNatureId;
            _rentalAssets.assetId = rentalAssets.assetId;
            _rentalAssets.UnitNames = rentalAssets.UnitNames;
            _rentalAssets.NotOwnedUnitNames = rentalAssets.NotOwnedUnitNames;



            if (rentalAssets.assetUnits.Count != 0)
            {
                _rentalAssets.assetUnits = new List<Asset>();
                foreach (var _asset in rentalAssets.assetUnits)
                {
                    _rentalAssets.assetUnits.Add(context.Assets.FirstOrDefault(x => x.Id == _asset.Id));
                }
            }

            else
                 if (_rentalAssets.assetUnits != null)
                _rentalAssets.assetUnits.Clear();

            context.SaveChanges();

        }
        public void UpdateTenancyContract(TenancyContract tenancyContract)
        {
            if (tenancyContract == null)
                throw new NullReferenceException("Object can not be null");

            var _tenancyContract = context.tenancyContracts

                .FirstOrDefault(x => x.Id == tenancyContract.Id);

            //var rentalAsset = context.Assets.FirstOrDefault(x => x.Id == tenancyContract.AssetId);
            // rentalAsset.isRented = true;
            //if (tenancyContract.isActive == true)
            //  rentalAsset.isRented = true;
            //else
            //  rentalAsset.isRented = false;

            _tenancyContract.AssetId = tenancyContract.AssetId;
            _tenancyContract.companyId = tenancyContract.companyId;
            _tenancyContract.deptId = tenancyContract.deptId;
            _tenancyContract.TenantId = tenancyContract.TenantId;
            _tenancyContract.ContractDateFrom = tenancyContract.ContractDateFrom;
            _tenancyContract.ContractDateTo = tenancyContract.ContractDateTo;
            _tenancyContract.TenancyContractDate = tenancyContract.TenancyContractDate;
            _tenancyContract.rentalBasis = tenancyContract.rentalBasis;
            _tenancyContract.RentalAmount = tenancyContract.RentalAmount;
            // _tenancyContract.assetUnits = tenancyContract.assetUnits;
            _tenancyContract.isActive = tenancyContract.isActive;

            foreach (var _assettt in tenancyContract.assetUnits)
            {
                var asset = context.Assets.FirstOrDefault(x => x.Id == _assettt.Id);
                if (tenancyContract.isActive == true)
                    asset.isRented = true;
                else
                    asset.isRented = false;
            }

            if (tenancyContract.assetUnits.Count != 0)
            {
                _tenancyContract.assetUnits.Clear();
                _tenancyContract.assetUnits = new List<Asset>();
                foreach (var _asset in tenancyContract.assetUnits)
                {
                    _tenancyContract.assetUnits.Add(context.Assets.FirstOrDefault(x => x.Id == _asset.Id));
                }
            }


            context.SaveChanges();

        }
        public void UpdateNotOwnedTenancyContract(TenancyContract tenancyContract)
        {
            if (tenancyContract == null)
                throw new NullReferenceException("Object can not be null");

            var _tenancyContract = context.tenancyContracts.FirstOrDefault(x => x.Id == tenancyContract.Id);

            var rentalAsset = context.Assets.FirstOrDefault(x => x.Id == tenancyContract.NotOwnedAssetId);
            // rentalAsset.isRented = true;
            if (tenancyContract.isActive == true)
                rentalAsset.isRented = true;
            else
                rentalAsset.isRented = false;

            _tenancyContract.NotOwnedAssetId = tenancyContract.NotOwnedAssetId;
            _tenancyContract.companyId = tenancyContract.companyId;
            _tenancyContract.deptId = tenancyContract.deptId;
            _tenancyContract.OwnerId = tenancyContract.OwnerId;
            _tenancyContract.ContractDateFrom = tenancyContract.ContractDateFrom;
            _tenancyContract.ContractDateTo = tenancyContract.ContractDateTo;
            _tenancyContract.TenancyContractDate = tenancyContract.TenancyContractDate;
            _tenancyContract.rentalBasis = tenancyContract.rentalBasis;
            _tenancyContract.RentalAmount = tenancyContract.RentalAmount;
            _tenancyContract.isActive = tenancyContract.isActive;
            context.SaveChanges();

        }

        public void UpdateTenancyContract(TenancyContract tenancyContract, int rentalAssetId)
        {
            if (tenancyContract == null)
                throw new NullReferenceException("Object can not be null");

            var _tenancyContract = context.tenancyContracts.FirstOrDefault(x => x.Id == tenancyContract.Id);

            var oldRentalAsset = context.Assets.FirstOrDefault(x => x.Id == rentalAssetId);
            oldRentalAsset.isRented = false;
            var rentalAsset = context.Assets.FirstOrDefault(x => x.Id == tenancyContract.AssetId);
            // rentalAsset.isRented = true;
            if (tenancyContract.isActive == true)
                rentalAsset.isRented = true;
            else
                rentalAsset.isRented = false;

            _tenancyContract.AssetId = tenancyContract.AssetId;
            _tenancyContract.companyId = tenancyContract.companyId;
            _tenancyContract.deptId = tenancyContract.deptId;
            _tenancyContract.TenantId = tenancyContract.TenantId;
            _tenancyContract.ContractDateFrom = tenancyContract.ContractDateFrom;
            _tenancyContract.ContractDateTo = tenancyContract.ContractDateTo;
            _tenancyContract.TenancyContractDate = tenancyContract.TenancyContractDate;
            _tenancyContract.rentalBasis = tenancyContract.rentalBasis;
            _tenancyContract.RentalAmount = tenancyContract.RentalAmount;
            _tenancyContract.isActive = tenancyContract.isActive;
            context.SaveChanges();

        }

        public void UpdateNotOwnedTenancyContract(TenancyContract tenancyContract, int rentalAssetId)
        {
            if (tenancyContract == null)
                throw new NullReferenceException("Object can not be null");

            var _tenancyContract = context.tenancyContracts.FirstOrDefault(x => x.Id == tenancyContract.Id);

            var oldRentalAsset = context.Assets.FirstOrDefault(x => x.Id == rentalAssetId);
            oldRentalAsset.isRented = false;
            var rentalAsset = context.Assets.FirstOrDefault(x => x.Id == tenancyContract.NotOwnedAssetId);
            // rentalAsset.isRented = true;
            if (tenancyContract.isActive == true)
                rentalAsset.isRented = true;
            else
                rentalAsset.isRented = false;

            _tenancyContract.NotOwnedAssetId = tenancyContract.NotOwnedAssetId;
            _tenancyContract.companyId = tenancyContract.companyId;
            _tenancyContract.deptId = tenancyContract.deptId;
            _tenancyContract.OwnerId = tenancyContract.OwnerId;
            _tenancyContract.ContractDateFrom = tenancyContract.ContractDateFrom;
            _tenancyContract.ContractDateTo = tenancyContract.ContractDateTo;
            _tenancyContract.TenancyContractDate = tenancyContract.TenancyContractDate;
            _tenancyContract.rentalBasis = tenancyContract.rentalBasis;
            _tenancyContract.RentalAmount = tenancyContract.RentalAmount;
            _tenancyContract.isActive = tenancyContract.isActive;
            context.SaveChanges();

        }

        /// <summary>
        /// Add New Method
        /// </summary>
        /// <param name="method"></param>
        public void AddRentalType(RentalAssetMethod method)
        {
            context.RentalAssetMethods.Add(method);
            context.SaveChanges();
        }
        /// <summary>
        /// Update Payment Method
        /// </summary>
        /// <param name="method"></param>
        public void UpdateRentalType(RentalAssetMethod method)
        {
            RentalAssetMethod _method = context.RentalAssetMethods.FirstOrDefault(x => x.Id == method.Id);

            _method.MethodName = method.MethodName;
            _method.isActive = method.isActive;
            context.SaveChanges();
        }
        /// <summary>
        /// Get Payment Methods
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public RentalAssetMethod GetRentalType(int methodId)
        {
            return
                context.RentalAssetMethods.FirstOrDefault(x => x.Id == methodId);
        }
        /// <summary>
        /// Get Payment Methods
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public RentalAssets GetRental(int rentalAssetId)
        {
            return


                context.RentalAssets

                .FirstOrDefault(x => x.Id == rentalAssetId);
        }
        public Asset GetAsset(int RA)
        {
            return context.Assets.FirstOrDefault(x => x.Id == RA && x.isRented == false);
        }

        public RentalAssets GetRentals(int rentalAssetId)
        {
            return


                context.RentalAssets

                .FirstOrDefault(x => x.Id == rentalAssetId);
        }

        /// <summary>
        /// Get Payment Methods
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>

        public List<Asset> GetNotOwnedAssetUnit(int parentId)
        {
            return context.Assets
                 .Where(x => x.parentId == parentId && x.isOwned == false)//owned
                 .ToList();
        }
        public Asset GetNotOwnedAsset(int NotOwnedrentalAssetId)
        {
            return
                context.Assets

                .FirstOrDefault(x => x.Id == NotOwnedrentalAssetId && x.isOwned == false);
        }
        /// <summary>
        /// Get Payment Methods
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public RentalAssets GetRentalByFixedAssetId(int FixedAssetId)
        {
            return
                context.RentalAssets
                .FirstOrDefault(x => x.assetId == FixedAssetId);
        }
        /// <summary>
        /// Get Payment Methods
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public RentalAssets GetNotOwnedByAssetId(int notOwnedId)
        {
            return
                context.RentalAssets
                .FirstOrDefault(x => x.assetId == notOwnedId);
        }
        public Asset GetAssetId(int AssetId)
        {
            return context.Assets.FirstOrDefault(x => x.Id == AssetId);


        }


        /// <summary>
        /// Get last Bill
        /// </summary>
        /// <returns></returns>
        public int GetLastRental()
        {
            var month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            var currentMonthYear = DateTime.Now.Year.ToString() + month.ToString();
            var listOfRentals = context.rentalReceiveAmounts.Where(x => x.SystemRefNo.Contains(currentMonthYear)).ToList();
            if (listOfRentals.Count > 0)
            {
                var lastId = listOfRentals.Max(x => x.transactionGroupId);
                return lastId;
            }
            else
                return 0;

        }

        public void UpdateREntalAssetStatus(RentalAssetStatus status)
        {
            if (status == null)
                throw new NullReferenceException("Object can not be null");
            var _status = context.RentalAssetStatuses.FirstOrDefault(x => x.Id == status.Id);

            _status.Status = status.Status;
            _status.isActive = status.isActive;
            _status.backcolor = status.backcolor;

            context.SaveChanges();

        }
        public void AddRentalStatusStatus(RentalAssetStatus status)
        {
            context.RentalAssetStatuses.Add(status);
            context.SaveChanges();

        }
        public List<RentalAssetStatus> GetAllRentalAssetStatus()
        {
            var statusList = context.RentalAssetStatuses.ToList();
            return statusList;

        }
        public RentalAssetStatus GetRAStatus(int statusId)
        {
            var status = context.RentalAssetStatuses.FirstOrDefault(x => x.Id == statusId);
            return status;

        }
        public List<RentalAssetStatus> GetAllOpenRentalAssetStatus()
        {
            var statusList = context.RentalAssetStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }
        public List<RentalReceiveAmount> GetAllRental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.rentalReceiveAmounts


            .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id))

                .ToList();
        }
        public List<RentalReceiveAmount> GetAllRentalByGroupId(int groupId)
        {
            return context.rentalReceiveAmounts

                .Where(x => x.transactionGroupId == groupId)
                .ToList();
        }

        public void UpdateRentalReceive(List<RentalReceiveAmount> Rentalamount)
        {

            foreach (var _loan in Rentalamount)
            {
                if (_loan.Id > 0)
                {
                    var loansToUpdate = context.rentalReceiveAmounts.FirstOrDefault(x => x.Id == _loan.Id);
                    loansToUpdate = _loan;
                }

                //context.rentalReceiveAmounts.Add(_loan);

            }

            context.SaveChanges();
        }

    }
}
