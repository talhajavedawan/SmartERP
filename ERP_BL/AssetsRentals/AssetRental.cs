using ERP_BL.AssetsRentals.RentalContracts;
using ERP_BL.AssetsRentals.TenantRentals;
using ERP_BL.Countryy;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.StatusClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.AssetsRentals
{
    public class AssetRental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }
        public string AssetName { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public int? AssetNatureId { get; set; }
        [ForeignKey("AssetNatureId")]
        public virtual RentalAssetNature AssetNature { get; set; }

        public int? assetSubNatureId { get; set; }
        [ForeignKey("assetSubNatureId")]
        public virtual RentalAssetSubNature assetSubNature { get; set; }

        public int? assetBrandId { get; set; }
        [ForeignKey("assetBrandId")]
        public virtual AssetBrand assetBrand { get; set; }

        public int? assetModelId { get; set; }
        [ForeignKey("assetModelId")]
        public virtual AssetModel assetModel { get; set; }

        public int? assetNumberId { get; set; }
        [ForeignKey("assetNumberId")]
        public virtual AssetNumber assetNumber { get; set; }

        public string SerialNumber { get; set; }

        public int? assetTypeId { get; set; }
        [ForeignKey("assetTypeId")]
        public virtual AssetType assetType { get; set; }

        [InverseProperty("assetRental")]
        public virtual List<RentalContract> RentalContracts { get; set; }

        public bool VendorFromSystem { get; set; }

        public int? vendorId { get; set; }
        [ForeignKey("vendorId")]
        public virtual Vendor vendor { get; set; }

        public string Vendor { get; set; }
        public string AssetNumber { get; set; }
        public string Description { get; set; }
        public DateTime? PurchasingDate { get; set; }
        public double PurchasingCostManual { get; set; }
        public double PurchasingCost { get; set; }
        public double? ProgressiveCost { get; set; }
        public double? ProgressiveCostManual { get; set; }
        public double? TotalCostManual { get; set; }
        public double? TotalCost { get; set; }

        [InverseProperty("assetRental")]
        public virtual List<AdminBill> adminBills { get; set; }

        public int? assetHolderEmployeeId { get; set; }
        [ForeignKey("assetHolderEmployeeId")]
        public virtual ERP_BL.Databases.Employee assetHolderEmployee { get; set; }
        public string AssetHolder { get; set; }

        public int transactionGroupId { get; set; }
        public string SystemRef { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User Creator { get; set; }

        public bool isRentable { get; set; }
        public bool isSubsidary { get; set; }
        public bool isLeased { get; set; }

        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual AssetRental parentAsset { get; set; }

        public int? countryId { get; set; }
        [ForeignKey("countryId")]
        public virtual Country country { get; set; }

        public int? cityId { get; set; }
        [ForeignKey("cityId")]
        public virtual City city { get; set; }

        public int? assetRentalLocationId { get; set; }
        [ForeignKey("assetRentalLocationId")]
        public virtual AssetRentalLocation assetRentalLocation { get; set; }


        public int? assetRentalUnitId { get; set; }
        [ForeignKey("assetRentalUnitId")]
        public virtual AssetRentalUnit assetRentalUnit { get; set; }

        public int? addressId { get; set; }
        [ForeignKey("addressId")]
        public virtual Address address { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual AssetRentalStatus Status { get; set; }

        public string stage { get; set; }
        public bool isVoid { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }
        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool? isReApproved { get; set; }
        public DateTime? ReApprovalDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
    }

    public class AssetRentalStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<AssetRental> AssetRentals { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("assetRentalStatuses")]
        public virtual List<StatusClass> assetRentalStatusSubClasses { get; set; }
    }   

    public class RentalAssetNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NatureName { get; set; }
        public bool isActive { get; set; }
    }

    public class RentalAssetSubNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NatureName { get; set; }

        public int? assetNatureId { get; set; }
        [ForeignKey("assetNatureId")]
        public virtual RentalAssetNature assetNature { get; set; }

        public virtual List<AssetBrand> AssetBrands { get; set; }

        public virtual List<AssetModel> AssetModels { get; set; }

        public bool isActive { get; set; }
    }

    public class AssetType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool isActive { get; set; }
    }

    public class AssetBrand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string BrandName { get; set; }
        public int? assetNatureId { get; set; }
        [ForeignKey("assetNatureId")]
        public virtual RentalAssetNature assetNature { get; set; }

        [InverseProperty("AssetBrands")]
        public virtual List<RentalAssetSubNature> AssetSubNatures { get; set; }

        public virtual List<AssetModel> AssetModels { get; set; }

        public bool isActive { get; set; }
    }

    public class AssetModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ModelNumber { get; set; }

        public int? assetNatureId { get; set; }
        [ForeignKey("assetNatureId")]
        public virtual RentalAssetNature assetNature { get; set; }

        [InverseProperty("AssetModels")]
        public virtual List<RentalAssetSubNature> AssetSubNatures { get; set; }

        [InverseProperty("AssetModels")]
        public virtual List<AssetBrand> assetBrands { get; set; }

        public bool isActive { get; set; }
    }

    public class AssetNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Number { get; set; }
        public bool isActive { get; set; }
    }

    public class AssetRentalLocation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LocationTitle { get; set; }
        
        public int? cityId { get; set; }
        [ForeignKey("cityId")]
        public virtual City city { get; set; }

        public int? countryId { get; set; }
        [ForeignKey("countryId")]
        public virtual Country  country { get; set; }
    }
    
    public class AssetRentalUnit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UnitNo { get; set; }
        
        public int? cityId { get; set; }
        [ForeignKey("cityId")]
        public virtual City city { get; set; }

        public int? countryId { get; set; }
        [ForeignKey("countryId")]
        public virtual Country  country { get; set; }

        public int? assetRentalLocationId { get; set; }
        [ForeignKey("assetRentalLocationId")]
        public virtual AssetRentalLocation assetRentalLocation { get; set; }
    }
}
