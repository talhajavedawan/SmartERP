using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Tax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.Inventories
{
    public class InventoryAdjustment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? CreationDate { get; set; }

        public AdjustmentType AdjustmentType { get; set; }
        public int? creator_Id { get; set; }
        [ForeignKey("creator_Id")]
        public virtual ERP_BL.Databases.User Creator { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        public virtual Company Company { get; set; }

        public int? depId_Id { get; set; }
        [ForeignKey("depId_Id ")]
        public virtual Department Department { get; set; }
        public int currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency Currency { get; set; }

        public int chartofAccount_Id { get; set; }
        [ForeignKey("chartofAccount_Id")]
        public virtual ChartofAccount ChartofAccount { get; set; }
        public DateTime? AdjustmentDate { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual InventoryAdjustmentStatus AdjustmentStatus { get; set; }
        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }
        public bool? isApproved { get; set; } = true;
        public DateTime? ReApprovalDate { get; set; }
        public bool? isReApproved { get; set; } = true;
        public DateTime? ApprovedDate { get; set; }
        public bool isVoid { get; set; } = false;
        public string stage { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
    }
    public class InventoryAdjustmentStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public string backcolor { get; set; }
        public virtual List<InventoryAdjustment> InventoryAdjustments { get; set; }

        public string forecolor { get; set; }
    }
}
