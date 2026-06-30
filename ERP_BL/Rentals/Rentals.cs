using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Rentals
{
    public class RentalAssets
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }//Should be hide

        public RentalAssetType AssetType { get; set; }

        public int? AssetNatureId { get; set; }
        [ForeignKey("AssetNatureId")]
        public virtual AssetNature assetNature { get; set; }


        public string UnitNames { get; set; }
        public string NotOwnedUnitNames { get; set; }


        public virtual List<Asset> assetUnits { get; set; }

        public int? assetId { get; set; }
        [ForeignKey("assetId")]
        public virtual Asset asset { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }


    }
    public class Tenant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Profession { get; set; }
        public DateTime? CreationDate { get; set; }
        public virtual Contact contact { get; set; }
        public virtual Person person { get; set; }
        public virtual Address address { get; set; }

        public bool isActive { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }
    }

    public class RentalAssetStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<RentalReceiveAmount> rentalAssets { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }

    }
    public class RentalAssetMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MethodName { get; set; }
        public bool isActive { get; set; }
    }


    public class TenancyContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ContractDateFrom { get; set; }
        public DateTime? ContractDateTo { get; set; }



        public DateTime? TenancyContractDate { get; set; }
        public string ContractReferenceNo { get; set; }
        public double RentalAmount { get; set; }
        public RentalBasis rentalBasis { get; set; }
        public RentalAssetType AssetType { get; set; }
        public bool isActive { get; set; }

        [InverseProperty("tenancyContracts")]
        public List<Asset> assetUnits { get; set; }

        public string UnitNames { get; set; }

        public int? AssetId { get; set; }
        [ForeignKey("AssetId")]
        public virtual RentalAssets AssetsName { get; set; }

        public int? NotOwnedAssetId { get; set; }
        [ForeignKey("NotOwnedAssetId")]
        public virtual RentalAssets NotOwnedAssetsName { get; set; }

        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual AssetOwner owner { get; set; }

        public int? TenantId { get; set; }
        [ForeignKey("TenantId")]
        public virtual Tenant tenant { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }


    }

    public class AssetOwner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime creationDate { get; set; }
        public virtual Person owner { get; set; }
        public virtual Contact contact { get; set; }
        public virtual Address address { get; set; }
        public bool isActive { get; set; }
        public Gender gender { get; set; }

    }


    //public class NotOwnedRentalAsset
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }
    //    public DateTime creationDate { get; set; }
    //    public bool isRentable { get; set; }
    //    public string AssetName { get; set; }

    //    public int? companyId { get; set; }
    //    [ForeignKey("companyId")]
    //    public Company company { get; set; }

    //    public int? deptId { get; set; }
    //    [ForeignKey("deptId")]
    //    public Department department { get; set; }

    //    public int? OwnerId { get; set; }
    //    [ForeignKey("OwnerId")]
    //    public AssetOwner assetOwner { get; set; }

    //    public int? CoOwnerId { get; set; }
    //    [ForeignKey("CoOwnerId")]
    //    public AssetOwner CoassetOwner { get; set; } 

    //    public int? parentId { get; set; } 
    //    [ForeignKey("parentId")] 
    //    public virtual NotOwnedRentalAsset ParentNotOwnedRentalAsset { get; set; }  

    //    public int? AssetNatureId { get; set; }
    //    [ForeignKey("AssetNatureId")]
    //    public AssetNature assetNature { get; set; }

    //    [InverseProperty("NotOwnedAssetUnits")]
    //    public ICollection<TenancyContract> tenancyContracts { get; set; }
    //    public bool isRented { get; set; }

    //}
    public class RentalReceiveAmount //used for main form class
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime creationDate { get; set; }
        public double receiveAmount { get; set; }
        public RentalBasis rentalBasis { get; set; } //used for rental period
        public RentalAssetType AssetType { get; set; }
        public string FinanceRefNo { get; set; }
        public string SystemRefNo { get; set; }
        public int transactionGroupId { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        public virtual Company company { get; set; }

        public int? dept_Id { get; set; }
        [ForeignKey("dept_Id")]
        public virtual Department department { get; set; }

        public int? tenancyContractId { get; set; }
        [ForeignKey("tenancyContractId")]
        public virtual TenancyContract tenancyContract { get; set; }
        public string Subsidary { get; set; }
        public string TenantName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int? RentalassetId { get; set; }
        [ForeignKey("RentalassetId")]
        public virtual RentalAssets rentalAssetName { get; set; }

        public int? tenantId { get; set; }
        [ForeignKey("tenantId")]
        public Tenant tenantName { get; set; }


        public int? ownerId { get; set; }
        [ForeignKey("ownerId")]
        public virtual AssetOwner ownerName { get; set; }

        //public int? notOwnedAssetId { get; set; }
        //[ForeignKey("notOwnedAssetId")]
        //public NotOwnedRentalAsset notOwnedRentalAssetName { get; set; } 


        public DateTime? rentalDate { get; set; }
        public double rentalAmount { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual RentalAssetStatus rentalAssetStatus { get; set; }


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


        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.Employee Creator { get; set; }

    }


}
