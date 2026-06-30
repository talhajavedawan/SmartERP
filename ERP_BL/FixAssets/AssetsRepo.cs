using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.FixAssets
{
    public class AssetsRepo
    {
        DBContextERP context = new DBContextERP();

        public void AddAssetNature(AssetNature assetNature)
        {
            if (assetNature == null)
                throw new NullReferenceException("Object can not be null");

            context.AssetNatures.Add(assetNature);
            context.SaveChanges();
        }

        public void AddBuilding(Building building)
        {
            if (building == null)
                throw new NullReferenceException("Object can not be null");
            if (building.asset.OwnerCompany != null)
            {
                var ownerCompany = context.Companies.First(x => x.Id == building.asset.OwnerCompany.Id);
                building.asset.OwnerCompany = ownerCompany;
            }

            if (building.asset.managingDept != null)
            {
                var managingDept = context.Departments.First(x => x.Id == building.asset.managingDept.Id);
                building.asset.managingDept = managingDept;
            }

            if (building.asset.owner != null)
            {
                var owner = context.Employees.First(x => x.EmpId == building.asset.owner.EmpId);
                building.asset.owner = owner;
            }

            if (building.asset.CoOwner != null)
            {
                var CoOwner = context.Employees.First(x => x.EmpId == building.asset.CoOwner.EmpId);
                building.asset.CoOwner = CoOwner;
            }

            if (building.asset.purchaseInfo.currecncy != null)
            {
                var currency = context.currencies.First(x => x.Id == building.asset.purchaseInfo.currecncy.Id);
                building.asset.purchaseInfo.currecncy = currency;
            }

            if (building.asset.handler != null)
            {
                var handler = context.Users.First(x => x.employeeId == building.asset.handler.employeeId);
                building.asset.handler = handler;
            }
            if (building.asset.AssetNature != null)
            {
                var nature = context.AssetNatures.First(x => x.Id == building.asset.AssetNature.Id);
                building.asset.AssetNature = nature;
            }

            if (building.asset.parentAsset != null)
            {
                var parent = context.Assets.FirstOrDefault(x => x.Id == building.asset.parentAsset.Id);
                building.asset.parentAsset = parent;
            }

            if (building.asset.assetStatus != null)
            {
                var status = context.assetStatuses.FirstOrDefault(c => c.Id == building.asset.assetStatus.Id);
                building.asset.assetStatus = status;
            }

            if (building.OfficialAuths != null)
            {
                var offAuth = context.OfficialAuths.FirstOrDefault(x => x.Id == building.OfficialAuths.Id);
                building.OfficialAuths = offAuth;
            }


            context.Buildings.Add(building);
            context.SaveChanges();
        }

        public void AddAuthDoc(AuthDoc authDoc)
        {
            if (authDoc == null)
                throw new NullReferenceException("Object can not be null");
            else if (authDoc.authId == null)
                throw new NullReferenceException("Official Auth not mentioned");

            if (authDoc.OfficialAuth != null)
            {
                authDoc.OfficialAuth = context.OfficialAuths.FirstOrDefault(x => x.Id == authDoc.OfficialAuth.Id);
            }

            context.AuthDocs.Add(authDoc);
            context.SaveChanges();
        }



        public void AddLand(Land land)
        {
            if (land == null)
                throw new NullReferenceException("Object can not be null");

            if (land.asset.OwnerCompany != null)
            {
                var ownerCompany = context.Companies.First(x => x.Id == land.asset.OwnerCompany.Id);
                land.asset.OwnerCompany = ownerCompany;
            }

            if (land.asset.owner != null)
            {
                var owner = context.Employees.First(x => x.EmpId == land.asset.owner.EmpId);
                land.asset.owner = owner;
            }

            if (land.asset.CoOwner != null)
            {
                var CoOwner = context.Employees.First(x => x.EmpId == land.asset.CoOwner.EmpId);
                land.asset.CoOwner = CoOwner;
            }

            if (land.asset.purchaseInfo.currecncy != null)
            {
                var currency = context.currencies.First(x => x.Id == land.asset.purchaseInfo.currecncy.Id);
                land.asset.purchaseInfo.currecncy = currency;
            }

            context.Lands.Add(land);
            context.SaveChanges();
        }

        public void AddVehicleType(VehicleType type)
        {
            if (type == null)
                throw new NullReferenceException("Object can not be null");

            context.VehicleTypes.Add(type);
            context.SaveChanges();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new NullReferenceException("Object can not be null");
            if (vehicle.OfficialAuths == null)
                throw new NullReferenceException("Official Auth, can not be null");

            if (vehicle.asset.OwnerCompany != null)
            {
                var ownerCompany = context.Companies.First(x => x.Id == vehicle.asset.OwnerCompany.Id);
                vehicle.asset.OwnerCompany = ownerCompany;
            }

            if (vehicle.asset.owner != null)
            {
                var owner = context.Employees.First(x => x.EmpId == vehicle.asset.owner.EmpId);
                vehicle.asset.owner = owner;
            }

            if (vehicle.asset.CoOwner != null)
            {
                var CoOwner = context.Employees.First(x => x.EmpId == vehicle.asset.CoOwner.EmpId);
                vehicle.asset.CoOwner = CoOwner;
            }
            if (vehicle.asset.AssetNature != null)
            {
                var assetNature = context.AssetNatures.First(x => x.Id == vehicle.asset.AssetNature.Id);
                vehicle.asset.AssetNature = assetNature;
            }
            if (vehicle.asset.OwnerCompany != null)
            {
                var ownerCompany = context.Companies.First(x => x.Id == vehicle.asset.OwnerCompany.Id);
                vehicle.asset.OwnerCompany = ownerCompany;
            }

            if (vehicle.asset.managingDept != null)
            {
                var managingDepartment = context.Departments.First(x => x.Id == vehicle.asset.managingDept.Id);
                vehicle.asset.managingDept = managingDepartment;
            }

            if (vehicle.asset.handler != null)
            {
                var handler = context.Users.First(x => x.employeeId == vehicle.asset.handler.employeeId);
                vehicle.asset.handler = handler;
            }



            if (vehicle.asset.purchaseInfo.currecncy != null)
            {
                var currency = context.currencies.First(x => x.Id == vehicle.asset.purchaseInfo.currecncy.Id);
                vehicle.asset.purchaseInfo.currecncy = currency;
            }

            if (vehicle.manufacturer != null)
            {
                var _manufacturer = context.Manufacturers.First(x => x.Id == vehicle.manufacturer.Id);
                vehicle.manufacturer = _manufacturer;
            }

            if (vehicle.lesee != null)
            {
                var _lesee = context.Lesees.First(x => x.Id == vehicle.lesee.Id);
                vehicle.lesee = _lesee;
            }

            if (vehicle.OfficialAuths != null)
            {
                var _offAuth = context.OfficialAuths.First(x => x.Id == vehicle.OfficialAuths.Id);
                vehicle.OfficialAuths = _offAuth;
            }

            if (vehicle.currentStatus != null)
            {
                var status = context.VehicleCurrentStatuses.First(x => x.Id == vehicle.currentStatus.Id);
                vehicle.currentStatus = status;
            }

         



            context.Vehicles.Add(vehicle);
            context.SaveChanges();
        }

        public void AddVehicleConditionImages(ConditionImage conditionImage)
        {
            if (conditionImage == null)
                throw new NullReferenceException("Object can not be null");
            if (conditionImage.Vehicle == null)
                throw new Exception("Foreign key contraint voilation, Vehicle Object missing.");

            context.ConditionImages.Add(conditionImage);
            context.SaveChanges();
        }


        public void AddMortgagee(Mortgagee mortgagee)
        {
            if (mortgagee == null)
                throw new NullReferenceException("Object can not be null");

            context.Mortgagees.Add(mortgagee);
            context.SaveChanges();
        }

        public void AddLessee(Lesee lesee)
        {
            if (lesee == null)
                throw new NullReferenceException("Object can not be null");

            context.Lesees.Add(lesee);
            context.SaveChanges();
        }

        public void AddManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new NullReferenceException("Object can not be null");

            context.Manufacturers.Add(manufacturer);
            context.SaveChanges();
        }

        public void AddOfficialAuth(OfficialAuth officialAuth)
        {
            if (officialAuth == null)
                throw new NullReferenceException("Object can not be null");
            if (officialAuth.authDocs == null || officialAuth.authDocs.Count == 0)
                throw new NullReferenceException("No Authority Documents found");

            context.OfficialAuths.Add(officialAuth);
            context.SaveChanges();
        }

        public void AddRegion(Region region)
        {
            if (region == null)
                throw new NullReferenceException("Object can not be null");

            context.Regions.Add(region);
            context.SaveChanges();
        }

        public void AddRevaluation(Revaluation revaluation)
        {
            if (revaluation == null)
                throw new NullReferenceException("Object can not be null");
            if (revaluation.asset == null)
                throw new NullReferenceException("Asset for revaluation is missing");

            context.Revaluations.Add(revaluation);
            context.SaveChanges();
        }

        public void AddVehicleCurrentStatus(VehicleCurrentStatus currentStatus)
        {
            if (currentStatus == null)
                throw new NullReferenceException("Object can not be null");

            context.VehicleCurrentStatuses.Add(currentStatus);
            context.SaveChanges();
        }

        public void AddVehicleBuyingStatus(VehicleBuyingStatus vehicleBuyingStatus)
        {
            if (vehicleBuyingStatus == null)
                throw new NullReferenceException("Object can not be null");

            context.VehicleBuyingStatuses.Add(vehicleBuyingStatus);
            context.SaveChanges();
        }


        //Fixed Assets Status Related Functions

        public void AddAssetStatus(AssetStatus assetStatus)
        {
            if (assetStatus == null)
                throw new NullReferenceException("Object can not be null");
            context.assetStatuses.Add(assetStatus);
            context.SaveChanges();

        }


        public Revaluation GetRevaluation(int revId)
        {
            if (revId == 0)
                throw new NullReferenceException("Object can not be null");

            return context.Revaluations.First(x => x.Id == revId);
        }

        public Revaluation GetLastRevaluation()
        {
            var revs = context.Revaluations.ToList();
            if (revs == null || revs.Count == 0)
                return null;
            
            return revs.Last();
        }
        
        public List<Revaluation> GetRevaluationsHistory(Asset asset)
        {
            if (asset == null)
                throw new NullReferenceException("Object can not be null");

            return context.Revaluations.Where(x => x.asset.Id == asset.Id).ToList();
        }

        public List<Revaluation> GetRevaluationsHistory(int assetId)
        {
            if (assetId == 0)
                throw new NullReferenceException("Object can not be null");

            return context.Revaluations.Where(x => x.asset.Id == assetId).ToList();
        }

        public List<Revaluation> GetRevaluationsGroup(int transGroupID)
        {
            if (transGroupID == 0)
                throw new NullReferenceException("Object can not be null");

            return context.Revaluations.Where(x => x.transGroupID== transGroupID).ToList();
        }

        public Asset GetCommon(int Id)
        {
            return context.Assets.FirstOrDefault(x => x.Id == Id);
        }

        public List<Asset> GetAllCommon()
        {
            return context.Assets

                .Where(x=>x.isOwned == true)
                .ToList();
        }

        public List<Asset> GetAllPendingForApprovalAssets()
        {
            return context.Assets

                .Where(x => x.isApproved == false)
                .ToList();
        }
    
        public List<AuthDoc> GetAllOfficialDoc(int AuthId)
        {
            return context.AuthDocs.Where(x => x.authId == AuthId).ToList();
        }

        public Land GetLandAsset(int Id)
        {
            var landAsset =  context.Lands
                .FirstOrDefault(x => x.Id == Id);
            return landAsset;
        }

        public List<Land> GetAllLandAsset()
        {
           var landAssets =  context.Lands.ToList();
            return landAssets;
        }

        public Asset GetAsset(int Id)
        {
            var Asset = context.Assets
               
                .FirstOrDefault(x => x.Id == Id);
            return Asset;
        }

        public List<Asset> GetAllAsset()
        {
            var Assets = context.Assets.ToList();
            return Assets;
        }

        public List<Address> GetAllAddress()
        {
            return context.Address.Where(x => x.addressType == Enums.AddressTypes.assetAddress).ToList();
        }

        public AssetNature GetAssetNature(int id)
        {
            return context.AssetNatures.FirstOrDefault(x => x.Id == id);
        }

        public List<AssetNature> GetAllAssetNatures()
        {
            return context.AssetNatures.ToList();
        }

        public Building GetBuildingAsset(int id)
        {
            return context.Buildings

                .FirstOrDefault(x => x.Id == id);
        }

        public Building GetBuildingByAsset(int assetId)
        {
            var asset = context.Buildings

                .FirstOrDefault(x => x.asset.Id == assetId);

            return asset;
        }

        public List<Building> GetAllBuildingAsset()
        {
            return context.Buildings

                .ToList();
        }
        public Vehicle GetVehicleByAsset(int assetId)
        {
            var asset = context.Vehicles

                .FirstOrDefault(x => x.asset.Id == assetId);

            return asset;
        }
        public List<Vehicle> GetAllVehicleAsset()
        {
            return context.Vehicles
                .ToList();
        }

        public VehicleType GetAllVehicleTypes(int typeId)
        {
            return context.VehicleTypes.FirstOrDefault(x => x.Id == typeId);
        }

        public List<VehicleType> GetAllVehicleTypes()
        {
            return context.VehicleTypes.ToList();
        }
        public Vehicle GetAllVehicleAsset(int Id)
        {
            return context.Vehicles

                .FirstOrDefault(x => x.Id == Id);
        }

        public Land GetCompleteLandAsset(int Id)
        {
            return context.Lands

                .FirstOrDefault(x => x.Id == Id);
        }

        public List<Land> GetCompleteLandAsset()
        {
            return context.Lands

                .ToList();
        }

        public Building GetCompleteBuildingAsset(int Id)
        {
            return context.Buildings

                .FirstOrDefault(x => x.Id == Id);
        }

        public List<Building> GetCompleteBuildingAsset()
        {
            return context.Buildings
                .ToList();
        }

        public Vehicle GetCompleteVehiceAsset(int Id)
        {
            return context.Vehicles

                .FirstOrDefault(x => x.Id == Id);
        }

        public List<Vehicle> GetCompleteVehiceAsset()
        {
            return context.Vehicles

                .ToList();
        }

        public List<ConditionImage> GetConditionImages(int VehicleId)
        {
            return context.ConditionImages.Where(x => x.Id == VehicleId).ToList();
        }


        public Region GetRegion(int Id)
        {
            return context.Regions.FirstOrDefault(x => x.Id == Id);
        }

        public List<Region> GetAllRegion()
        {
            return context.Regions.ToList();
        }

        public Mortgagee GetMortgagee(int Id)
        {
            return context.Mortgagees.FirstOrDefault(x => x.Id == Id);
        }

        public List<Mortgagee> GetAllMortgagee()
        {
            return context.Mortgagees.ToList();
        }

        public Lesee GetLessee(int Id)
        {
            return context.Lesees.FirstOrDefault(x => x.Id == Id);
        }

        public List<Lesee> GetAllLessees()
        {
            return context.Lesees.ToList();
        }

        public Manufacturer GetManufacturer(int Id)
        {
            return context.Manufacturers.FirstOrDefault(x => x.Id == Id);
        }

        public List<Manufacturer> GetAllManufacturer()
        {
            return context.Manufacturers.ToList();
        }

        public OfficialAuth GetAllOfficialAuth(int Id)
        {
            return context.OfficialAuths.FirstOrDefault(x => x.Id == Id);
        }

        public List<OfficialAuth> GetAllOfficialAuth()
        {
            return context.OfficialAuths

                .ToList();
        }


        public OfficialAuth GetOfficialAuth(int Id)
        {
            if (Id == null)
                throw new NullReferenceException("Object can not be null");

            return context.OfficialAuths.FirstOrDefault(x => x.Id == Id);
        }

        public VehicleBuyingStatus GetVehicleBuyingStatus(int Id)
        {
            if (Id == null)
                throw new NullReferenceException("Object can not be null");

            return context.VehicleBuyingStatuses.FirstOrDefault(x => x.Id == Id);
        }

        public List<VehicleBuyingStatus> GetAllVehicleBuyingStatus()
        {
            return context.VehicleBuyingStatuses.ToList();
        }


        public VehicleCurrentStatus GetVehicleCurrentStatus(int Id)
        {
            if (Id == null)
                throw new NullReferenceException("Object can not be null");

            return context.VehicleCurrentStatuses.FirstOrDefault(x => x.Id == Id);
        }

        public List<VehicleCurrentStatus> GetAllVehicleCurrentStatus()
        {
            return context.VehicleCurrentStatuses.ToList();
        }


        //Fnx for Asset Status

        /// <summary>
        /// Get all Asset Statuses.
        /// </summary>
        public List<AssetStatus> GetAllAssetStatus()
        {
            return context.assetStatuses.ToList();
        }

        /// <summary>
        /// Get all Active Asset Status
        /// For "OPEN" Status
        ///  </summary>
        /// <returns></returns>
        public List<AssetStatus> GetAllActiveAssetStatus()
        {
            return context.assetStatuses
                //.Include("Asset")
                .Where(x => x.isActive == true)
                .ToList();
        }
        /// <summary>
        /// Get all Inactive Asset Status.
        /// For "CLOSE" status
        /// </summary>
        /// <returns></returns>
        public List<AssetStatus> GetAllInActiveAssetStatus()
        {
            return context.assetStatuses
                //.Include("Asset")
                .Where(x => x.isActive == false)
                .ToList();
        }

        /// <summary>
        /// Get Asset Status based on Id.
        /// </summary>
        ///  <param name="assetStatusId"></param>
        /// <returns>AssetStatus</returns>
        public AssetStatus GetAssetStatus(int assetStatusId)
        {
            var status =  context.assetStatuses
                //.Include("Asset")
                .FirstOrDefault(x => x.Id == assetStatusId);

            return status;
        }



        public void UpdateAsset(Asset asset)
        {
            AssetStatus assetStat = new AssetStatus();
            Asset asset1 = new Asset();
            if (asset.assetStatus != null)
            {
                var status = context.assetStatuses.FirstOrDefault(x => x.Id == asset.assetStatus.Id);
                assetStat = status;
            }
            asset1 = asset;
            //if (assetStat != null)
            //{ asset1.assetStatus = assetStat; }
            asset1.assetStatus = assetStat;
            context.SaveChanges();

        }

          
        public void UpdateBuilding(Building building)
        {
            
            if (building == null)
                throw new NullReferenceException("Object can not be null");
            var _building = context.Buildings.FirstOrDefault(x => x.Id == building.Id);
            if (building.OfficialAuths != null)
                _building.OfficialAuths = context.OfficialAuths.FirstOrDefault(x => x.Id == building.OfficialAuths.Id);

            _building.isMortgaged = building.isMortgaged;
            _building.measureUnit = building.measureUnit;
            _building.mortgagedValue = building.mortgagedValue;
            if (building.mortgagee != null)
                _building.mortgagee = context.Mortgagees.FirstOrDefault(x => x.Id == building.mortgagee.Id);

            var _asset = context.Assets.FirstOrDefault(x => x.Id == building.asset.Id);


            _asset.AssetName = building.asset.AssetName;
            _asset.AssetNature = context.AssetNatures.FirstOrDefault(x => x.Id == building.asset.AssetNature.Id);
            if(building.asset.CoOwner!= null)
            _asset.CoOwner = context.Employees.FirstOrDefault(x => x.EmpId == building.asset.CoOwner.EmpId);

            
            _asset.OwnerCompany = context.Companies.FirstOrDefault(x => x.Id == building.asset.OwnerCompany.Id);

            if (building.asset.address != null)
            {
                var _address = context.Address.FirstOrDefault(x => x.Id  == building.asset.address.Id);
                _address.City = building.asset.address.City;
                _address.Country = building.asset.address.Country;
                _address.Line1 = building.asset.address.Line1;
                _address.region = building.asset.address.region;
                _asset.address = _address;

            }
            if(building.asset.handler != null)
            _asset.handler = context.Users.FirstOrDefault(x => x.id == building.asset.handler.id);

            _asset.isInsured = building.asset.isInsured;
            _asset.isRentable = building.asset.isRentable;
            _asset.isSubsidary = building.asset.isSubsidary;

            if (building.asset.managingDept != null)
            _asset.managingDept = context.Departments.FirstOrDefault(x => x.Id == building.asset.managingDept.Id);
            _asset.mustInsured = building.asset.mustInsured;

            if(building.asset.owner != null)
            _asset.owner = context.Employees.FirstOrDefault(x => x.EmpId == building.asset.owner.EmpId);

            if (building.asset.parentAsset != null)
            {
                _asset.parentAsset = context.Assets.FirstOrDefault(x => x.Id == building.asset.parentAsset.Id);
            }

            
                _asset.rentalBasis = building.asset.rentalBasis;
            if (building.asset.assetStatus != null)
            {
                _asset.assetStatus = context.assetStatuses.FirstOrDefault(x=>x.Id == building.asset.assetStatus.Id);
            }
                
           
            _building.asset = _asset;
            

            //if (building.asset.purchaseInfo != null)
            //{
            //    var _purchaseInfo = context.PurchaseInfos.FirstOrDefault(x => x.Id == building.asset.purchaseInfo.Id);
            //    _purchaseInfo.acquireAt = building.asset.purchaseInfo.acquireAt;
            //    _purchaseInfo.currecncy = building.asset.purchaseInfo.currecncy;
            //    _purchaseInfo.FA_Amount = building.asset.purchaseInfo.FA_Amount;
            //    _purchaseInfo.FA_Amount_PER = building.asset.purchaseInfo.FA_Amount_PER;
            //    _purchaseInfo.PER = building.asset.purchaseInfo.PER;
            //}

            context.SaveChanges();
        }

        public void UpdateAssetNature(AssetNature assetNature)
        {
            if (assetNature == null)
                throw new NullReferenceException("Object can not be null");
            var _assetNature = context.AssetNatures.FirstOrDefault(x => x.Id == assetNature.Id);
            _assetNature.isActive = assetNature.isActive;
            _assetNature.isSubsdary = assetNature.isSubsdary;
            _assetNature.NatureName = assetNature.NatureName;
            _assetNature.parentNature = assetNature.parentNature;
            context.SaveChanges();
        }


        public void UpdateOfficalDoc(int AuthId, AuthDoc authDoc)
        {
            if (authDoc == null)
                throw new NullReferenceException("Object can not be null");
            if (authDoc == null)
                throw new NullReferenceException("Official Authority can not be null");

            var _authDoc = context.AuthDocs.FirstOrDefault(x => x.authId == AuthId && x.Id == authDoc.Id);

            _authDoc.DocName = authDoc.DocName;
            _authDoc.isAttached = authDoc.isAttached;
            _authDoc.uplaodLocation = authDoc.uplaodLocation;

            context.SaveChanges();
        }

        public void UpdateLand(Land land)
        {
            if (land == null)
                throw new NullReferenceException("Object can not be null");

            var _land = context.Lands.FirstOrDefault(x => x.Id == land.Id);
            _land.isMortgaged = land.isMortgaged;
            _land.measureUnit = land.measureUnit;
            _land.mortgagedValue = land.mortgagedValue;
            _land.mortgagee = land.mortgagee;
            _land.asset = land.asset;

            context.SaveChanges();
        }

        public void UpdateVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
                throw new NullReferenceException("Object can not be null");
            if (vehicle.OfficialAuths == null)
                throw new NullReferenceException("Official Auth, can not be null");

            var _vehicle = context.Vehicles.FirstOrDefault(x => x.Id == vehicle.Id);

            _vehicle.Chesis = vehicle.Chesis;
            _vehicle.EngineNo = vehicle.EngineNo;
            _vehicle.EngineReading = vehicle.EngineReading;
            _vehicle.isLeased = vehicle.isLeased;
            _vehicle.isNew = vehicle.isNew;
            _vehicle.leasingValue = vehicle.leasingValue;
            _vehicle.lesee = vehicle.lesee;
            _vehicle.LifeTimeToken = vehicle.LifeTimeToken;
            _vehicle.manufacturer = vehicle.manufacturer;
            _vehicle.Model = vehicle.Model;
            _vehicle.OfficialAuths = vehicle.OfficialAuths;
            _vehicle.tokenValidTill = vehicle.tokenValidTill;
            _vehicle.RegNo = vehicle.RegNo;

            var _asset = context.Assets.FirstOrDefault(x => x.Id == vehicle.asset.Id);
            if (_asset == null)
                throw new NullReferenceException("Asset, can not be null");

            _asset.AssetName = vehicle.asset.AssetName;
            _asset.AssetNature = context.AssetNatures.FirstOrDefault(x => x.Id == vehicle.asset.AssetNature.Id);
            if (vehicle.asset.CoOwner != null)
                _asset.CoOwner = context.Employees.FirstOrDefault(x => x.EmpId == vehicle.asset.CoOwner.EmpId);


            _asset.OwnerCompany = context.Companies.FirstOrDefault(x => x.Id == vehicle.asset.OwnerCompany.Id);

            if (vehicle.asset.address != null)
            {
                var _address = context.Address.FirstOrDefault(x => x.Id == vehicle.asset.address.Id);
                _address.City = vehicle.asset.address.City;
                _address.Country = vehicle.asset.address.Country;
                _address.Line1 = vehicle.asset.address.Line1;
                _address.region = vehicle.asset.address.region;
                _asset.address = _address;

            }
            if (vehicle.asset.handler != null)
                _asset.handler = context.Users.FirstOrDefault(x => x.id == vehicle.asset.handler.id);

            _asset.isInsured = vehicle.asset.isInsured;
            _asset.isRentable = vehicle.asset.isRentable;
            _asset.isSubsidary = vehicle.asset.isSubsidary;

            if (vehicle.asset.managingDept != null)
                _asset.managingDept = context.Departments.FirstOrDefault(x => x.Id == vehicle.asset.managingDept.Id);
            _asset.mustInsured = vehicle.asset.mustInsured;

            if (vehicle.asset.owner != null)
                _asset.owner = context.Employees.FirstOrDefault(x => x.EmpId == vehicle.asset.owner.EmpId);

            if (vehicle.asset.parentAsset != null)
            {
                _asset.parentAsset = context.Assets.FirstOrDefault(x => x.Id == vehicle.asset.assetStatus.Id);
            }


            if (vehicle.asset.assetStatus != null)
            {
                _asset.assetStatus = context.assetStatuses.FirstOrDefault(x => x.Id == vehicle.asset.assetStatus.Id);
            }

            _asset.rentalBasis = vehicle.asset.rentalBasis;

            _vehicle.asset = _asset;

            context.SaveChanges();
        }

        public void UpdateVehicleConditionImage(ConditionImage conditionImage)
        {
            if (conditionImage == null)
                throw new NullReferenceException("Object can not be null");
            var _image = context.ConditionImages.FirstOrDefault(x => x.Id == conditionImage.Id);

            _image.ImageName = conditionImage.ImageName;
            _image.location = conditionImage.location;
            _image.uploadDate = conditionImage.uploadDate;
            _image.Vehicle = conditionImage.Vehicle;

            context.SaveChanges();
        }

        public void UpdateMortgagee(Mortgagee mortgagee)
        {
            if (mortgagee == null)
                throw new NullReferenceException("Object can not be null");
            var _morgagee = context.Mortgagees.FirstOrDefault(x => x.Id == mortgagee.Id);

            _morgagee.isActive = mortgagee.isActive;
            _morgagee.Name = mortgagee.Name;

            context.SaveChanges();
        }

        public void UpdateLessee(Lesee lesee)
        {
            if (lesee == null)
                throw new NullReferenceException("Object can not be null");
            var _lessee = context.Lesees.FirstOrDefault(x => x.Id == lesee.Id);

            _lessee.LesseName = lesee.LesseName;
            _lessee.isActive = lesee.isActive;

            context.SaveChanges();
        }

        public void UpdateManufacturer(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                throw new NullReferenceException("Object can not be null");
            var _menufacturer = context.Manufacturers.FirstOrDefault(x => x.Id == manufacturer.Id);

            _menufacturer.ManufacturerName = manufacturer.ManufacturerName;
            _menufacturer.isActive = manufacturer.isActive;
            context.SaveChanges();
        }

        public void UpdateOfficialAuth(OfficialAuth officialAuth)
        {
            if (officialAuth == null)
                throw new NullReferenceException("Object can not be null");
            //if (officialAuth.authDocs == null || officialAuth.authDocs.Count == 0)
            //    throw new NullReferenceException("No Authority Documents found");

            var _OffAuth = context.OfficialAuths.FirstOrDefault(x => x.Id == officialAuth.Id);

            _OffAuth.AuthName = officialAuth.AuthName;
            _OffAuth.isActive = officialAuth.isActive;
            _OffAuth.region = officialAuth.region;
            context.SaveChanges();
        }

        public void UpdateVehicleType(VehicleType vehicleType)
        {
            if (vehicleType == null)
                throw new NullReferenceException("Object can not be null");

            var _vehicleType = context.VehicleTypes.FirstOrDefault(x => x.Id == vehicleType.Id);

            _vehicleType.TypeName = vehicleType.TypeName;
            _vehicleType.isActive = vehicleType.isActive;
            context.SaveChanges();
        }

        public void UpdateOfficialAuth(OfficialAuth officialAuth, List<AuthDoc> authDoc)
        {
            if (officialAuth == null)
                throw new NullReferenceException("Object can not be null");
            if (authDoc == null || authDoc.Count == 0)
                throw new NullReferenceException("No Authority Documents found");

            var _OffAuth = context.OfficialAuths.FirstOrDefault(x => x.Id == officialAuth.Id);

            _OffAuth.AuthName = officialAuth.AuthName;
            _OffAuth.isActive = officialAuth.isActive;
            _OffAuth.region = officialAuth.region;
            _OffAuth.authDocs = authDoc;
            context.SaveChanges();
        }

        public void UpdateRevaluation(Revaluation revaluation)
        {
            if (revaluation == null)
                throw new NullReferenceException("Object can not be null");
            if (revaluation.asset == null)
                throw new NullReferenceException("Asset for revaluation is missing");
            var _rev = context.Revaluations.FirstOrDefault(x => x.Id == revaluation.Id);
            if (_rev == null)
                throw new Exception("Revaluation object not found");

            _rev.MER = revaluation.MER;
            _rev.transGroupID = revaluation.transGroupID;
            _rev.amountMER = revaluation.amountMER;
            _rev.amountMR = revaluation.amountMR;
            _rev.asset = revaluation.asset;

            context.SaveChanges();
        }

        public void UpdateVehicleBuyingStatus(VehicleBuyingStatus vehicleBuyingStatus)
        {
            if (vehicleBuyingStatus == null)
                throw new NullReferenceException("Object can not be null");

            var _buyingStatus = context.VehicleBuyingStatuses.FirstOrDefault(x => x.Id == vehicleBuyingStatus.Id);
            if (_buyingStatus == null)
                throw new Exception("BuyingStatus object not found");

            _buyingStatus.buyingStatus = vehicleBuyingStatus.buyingStatus;
            _buyingStatus.isNew = vehicleBuyingStatus.isNew;

            context.SaveChanges();
        }

        public void UpdateVehicleCurrentStatus(VehicleCurrentStatus vehicleCurrentStatus)
        {
            if (vehicleCurrentStatus == null)
                throw new NullReferenceException("Object can not be null");

            var _CurrentStatus = context.VehicleCurrentStatuses.FirstOrDefault(x => x.Id == vehicleCurrentStatus.Id);
            if (_CurrentStatus == null)
                throw new Exception("Current Status object not found");

            _CurrentStatus.currentStatusName = vehicleCurrentStatus.currentStatusName;
            _CurrentStatus.isActive = vehicleCurrentStatus.isActive;

            context.SaveChanges();
        }
    /// <summary>
    /// Updates Assets status
    /// </summary>
    /// <param name="assetStatus"></param>
        public void UpdateAssetStatus(AssetStatus assetStatus)
        {
            if (assetStatus == null)
                throw new NullReferenceException("Object can not be null");

            var _AssetStatus = context.assetStatuses.FirstOrDefault(x => x.Id == assetStatus.Id);
            if (_AssetStatus == null)
                throw new Exception("Asset Status object not found");

            _AssetStatus.backcolor = assetStatus.backcolor;
            _AssetStatus.forecolor = assetStatus.forecolor;
            _AssetStatus.HierarchicalIndex = assetStatus.HierarchicalIndex;
            _AssetStatus.isActive = assetStatus.isActive;
            _AssetStatus.isApproved = assetStatus.isApproved;
            _AssetStatus.Status = assetStatus.Status;
      
            context.SaveChanges();

        }


        //Functions for count
        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.Assets
                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains(x.OwnerCompany.Id) && x.isApproved == false && x.isVoid != true)
                .Count();
        }

        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains(x.OwnerCompany.Id)
                && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId 
                || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId 
                || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                //.Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }

        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains(x.OwnerCompany.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                //.Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains(x.OwnerCompany.Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                //.Where(x => x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
        }

        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains(x.OwnerCompany.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                //.Where(x =>  x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains(x.OwnerCompany.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                //.Where(x => x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.Assets
   
                .Where(x => x.isVoid == true)
                .Count();
        }
        public int getVoidRegisterAdministratorCount()
        {
            return context.Assets
                //.Include("company")
                //.Include("department")
                //.Include("customer")
                //.Include("Currency")
                //.Include("collectionMethod")
                //.Include("receiptDeductions")
                //.Include("saleReceiptStatus")
                .Where(x => x.isVoid == true)
                .Count();
        }
        public int getRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.Assets
                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains((int)x.OwnerCompany.Id) && x.isVoid != true)
                .Count();
        }
        public int getRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.Assets
                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains((int)x.OwnerCompany.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForAdministratorCount()
        {
            return context.Assets

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getRegisterAdministratorCount()
        {
            return context.Assets
                .Where(x => x.isVoid != true)
                .Count();
        }
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            

            return context.Assets

                //.Where(x => (deptIds.Contains(x.dept_Id) && companyIds.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyIds.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains(x.OwnerCompany.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
               
                .Count();
        }
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyIds.Contains(x.OwnerCompany.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
     
                .Count();
        }

          public int getAllPendingForClosingAdministratorCount()
        {
            return context.Assets

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

    }
}
