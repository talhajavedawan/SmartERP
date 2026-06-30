using ERP_BL.AssetsRentals.RentalContracts;
using ERP_BL.Databases;
using ERP_BL.Procurements.StatusClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.AssetsRentals.TenantRentals
{
    public class TenantRental
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string TenantName { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public int? assetRentalId { get; set; }
        [ForeignKey("assetRentalId")]
        public virtual AssetRental assetRental { get; set; }

        [InverseProperty("tenantRental")]
        public virtual List<RentalContract> RentalContracts { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? contactId { get; set; }
        [ForeignKey("contactId")]
        public virtual Contact contact { get; set; }

        public int? personId { get; set; }
        [ForeignKey("personId")]
        public virtual Person person { get; set; }

        public int? addressId { get; set; }
        [ForeignKey("addressId")]
        public virtual Address address { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual TenantRentalStatus Status { get; set; }

        public int transactionGroupId { get; set; }
        public string SystemRef { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User Creator { get; set; }

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

    public class TenantRentalStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<TenantRental> TenantRentals { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("tenantRentalStatuses")]
        public virtual List<StatusClass> tenantRentalStatusSubClasses { get; set; }
    }
}
