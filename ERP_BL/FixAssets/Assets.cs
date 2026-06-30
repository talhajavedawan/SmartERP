using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Rentals;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Asset
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AssetName { get; set; }

        public int? AssetNatureId { get; set; }
        [ForeignKey("AssetNatureId")]
        public AssetNature AssetNature { get; set; }
        public bool isRentable { get; set; }
        public RentalBasis rentalBasis { get; set; }

        public bool isSubsidary { get; set; }

        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual Asset parentAsset { get; set; }

        public int companyId { get; set; }
        [ForeignKey("companyId ")]
        public virtual Company OwnerCompany { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department managingDept { get; set; }

        public virtual Employee owner { get; set; }
        public int? coOwnerID { get; set; }
        [ForeignKey("coOwnerID")]
        public virtual Employee CoOwner { get; set; }
        public virtual User handler { get; set; }

        public virtual Designation designation { get; set; }
        public bool mustInsured { get; set; }
        public bool isInsured { get; set; }
        public virtual Address address { get; set; }

        public virtual PurchaseInfo purchaseInfo { get; set; }

        public virtual List<Revaluation> revaluations { get; set; }
        public virtual AssetStatus assetStatus { get; set; }

        [InverseProperty("assetUnits")]
        public ICollection<TenancyContract> tenancyContracts { get; set; }
        public bool isRented { get; set; }

        public bool isOwned { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual AssetOwner assetOwner { get; set; }

        public int? CoOwnerAssetId { get; set; }
        [ForeignKey("CoOwnerAssetId")]
        public virtual AssetOwner CoassetOwner { get; set; }



        //New Migrations
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



        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }


        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }


    }

    public class AssetNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NatureName { get; set; }
        public bool isSubsdary { get; set; }

        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual AssetNature parentNature { get; set; }
        public bool isActive { get; set; }

    }

    public class AssetStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<Asset> Assets { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
    }
}
