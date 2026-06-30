using ERP_BL.AssetsRentals.RentalOrders;
using ERP_BL.AssetsRentals.TenantRentals;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.StatusClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.AssetsRentals.RentalContracts
{
    public class RentalContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public int? assetRentalId { get; set; }
        [ForeignKey("assetRentalId")]
        public virtual AssetRental assetRental { get; set; }

        public int? tenantRentalId { get; set; }
        [ForeignKey("tenantRentalId")]
        public virtual TenantRental tenantRental { get; set; }

        [InverseProperty("rentalContract")]
        public virtual List<RentalOrder> rentalOrders { get; set; }

        public int transactionGroupId { get; set; }
        public string SystemRef { get; set; }


        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User Creator { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual RentalContractStatus Status { get; set; }

        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency currency { get; set; }

        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

        public double RentAmount { get; set; }
        public double MER { get; set; }
        public double RentAmountMER { get; set; }
        public double NumberOfMonths { get; set; }
        public double TotalRentAmount { get; set; }

        public RentalBasis rentalBasis { get; set; }

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

    public class RentalContractStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<RentalContract> rentalContracts { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("rentalContractStatuses")]
        public virtual List<StatusClass> rentalContractStatusSubClasses { get; set; }
    }
}
