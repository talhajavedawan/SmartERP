using ERP_BL.Databases;
using ERP_BL.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.Budget
{
    public class BudgetCostSheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department Department { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public virtual List<BudgetCostField> budgetCostFields { get; set; }

        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }
        public bool? isApproved { get; set; } = true;
        public DateTime? ReApprovalDate { get; set; }
        public bool? isReApproved { get; set; } = true;
        public DateTime? ApprovedDate { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public string stage { get; set; }
        public bool isVoid { get; set; } = false;
        public virtual BudgetCostSheetStatus budgetCostSheetStatus { get; set; }
        public int? empId { get; set; }
        [ForeignKey("empId")]
        public virtual ERP_BL.Databases.Employee Employee { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public DateTime? ClosingDate { get; set; }

        public double TotalIncome { get; set; }
        public double TotalCGS { get; set; }
        public double TotalExpense { get; set; }
        public double TotalGrossProfit { get; set; }
        public double TotalNetProfit { get; set; }

        public double TotalRSBCIncome { get; set; }
        public double TotalRSBCCGS { get; set; }
        public double TotalRSBCExpense { get; set; }
        public double TotalRSBCGrossProfit { get; set; }
        public double TotalRSBCNetProfit { get; set; }
        public DateTime? CreationDate { get; set; }

        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency Currency { get; set; }
        public string refNo { get; set; }
        public DateTime? BudgetMonth { get; set; }


    }
    public class BudgetCostField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Head_Id { get; set; }
        [ForeignKey("Head_Id")]
        public virtual BudgetSheetHead BudgetSheettHead { get; set; }

        public double BudgetedCost { get; set; }
        public double bmPerc { get; set; }
        public double RSBC { get; set; }
        public double rsbcPerc { get; set; }
        public double AdjustmentCost { get; set; }
        public double addedRSBC { get; set; }
    }
    public class BudgetSheetHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string HeadName { get; set; }
        public bool isActive { get; set; }
        public int SortId { get; set; }
        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User Creator { get; set; }
        public bool isIncome { get; set; }
        public bool isCGS { get; set; }
        public bool isExpense { get; set; }


    }
    public class BudgetCostSheetStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<BudgetCostSheet> BudgetCostSheets { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }

    }
    public class BudgetSystemCostField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Head_Id { get; set; }
        [ForeignKey("Head_Id")]
        public virtual BudgetSheetHead BudgetSheettHead { get; set; }

        public double BudgetedCost { get; set; }
        public double RSBC { get; set; }
        public double addedSystemCost { get; set; }
    }
}
