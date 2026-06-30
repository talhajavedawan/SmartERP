using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using ERP_BL.Payments;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.ChartofAccounts;
using ERP_BL.Procurements.StatusClass;

namespace ERP_BL.ToDoTasks
{
    public class ToDoTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime creationDate { get; set; }

        public int? TaskCreatorId { get; set; }
        [ForeignKey("TaskCreatorId")]
        public virtual ERP_BL.Databases.User TaskCreator { get; set; }

        public string TaskName { get; set; }
        public string TaskDescription { get; set; }

        public int? taskGroupId { get; set; }
        [ForeignKey("taskGroupId")]
        public virtual TaskGroups taskGroup { get; set; }

        public int? parentTaskId { get; set; }
        [ForeignKey("parentTaskId")]
        public virtual ToDoTask ParentTask { get; set; }

        public int? supervisedById { get; set; }
        [ForeignKey("supervisedById")]
        public virtual ERP_BL.Databases.User supervisedBy { get; set; }

        public int? salesHeadId { get; set; }
        [ForeignKey("salesHeadId")]
        public virtual ERP_BL.Databases.User salesHead { get; set; }

        [InverseProperty("ToDoTasks")]
        public virtual List<ERP_BL.Databases.User> assignedToUsers { get; set; }


        public int? targetGroupId { get; set; }
        [ForeignKey("targetGroupId")]
        public virtual TargetGroup targetGroup { get; set; }

        public int? targetTypeId { get; set; }
        [ForeignKey("targetTypeId")]
        public virtual TaskTargetType taskTargetType { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual ToDoTaskStatus Status { get; set; }

        public bool isImportant { get; set; }
        public bool isCompleted { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? TentativeClosingDate { get; set; }
        public DateTime? ActualClosingDate { get; set; }

        public double TaskPoints { get; set; }
        public double AchievedPoints { get; set; }
        public double SystemPoints { get; set; }
        public double PercentageAchieved { get; set; }
        public int stepCount { get; set; }
        public int achievedStepsCount { get; set; }

        public DateTime? PointsUpdatedOn { get; set; }
        public DateTime? StepDueDate { get; set; }

        public string stage { get; set; }

        public bool isVoid { get; set; }
        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public bool? isReApproved { get; set; }
        public DateTime? ReApprovalDate { get; set; }

        public string SOField { get; set; }
        public DateTime? searchedFromDate { get; set; }
        public DateTime? searchedToDate { get; set; }

        public string SOField1 { get; set; }
        public DateTime? searchedFromDate1 { get; set; }
        public DateTime? searchedToDate1 { get; set; }
        public double SystemPoints1 { get; set; }

        public string SOField2 { get; set; }
        public DateTime? searchedFromDate2 { get; set; }
        public DateTime? searchedToDate2 { get; set; }
        public double SystemPoints2 { get; set; }

        public string GroupCompanies { get; set; }
        public string GroupDepartments { get; set; }

        public DateTime? TargetYear { get; set; }
        public DateTime? TargetMonth { get; set; }

        public bool isClosed { get; set; }

        public double EstimatedGrossProfitSE { get; set; }

        public double margin { get; set; } //Budgeted Margin(O.C)
        public double ManualBudgetedMarginME { get; set; }
        public double BudgetedMargininBase { get; set; } //Budgeted Margin(M.E)
        public double ManualBudgetedMarginSE { get; set; }
        public double SalesBudgetedMargin { get; set; } //Budgeted Margin(S.E)

        public double RevisedMargin { get; set; } //Revised Margin(O.C)
        public double RevisedMargininBase { get; set; } //Revised Margin(M.E)
        public double SalesRevisedMargin { get; set; } //Revised Margin(S.E)

        public double ActualMargin { get; set; } //Actual Margin(O.C)
        public double ManualActualMarginME { get; set; }
        public double ActualMargininBase { get; set; } //Actual Margin(M.E)
        public double ManualActualMarginSE { get; set; }
        public double SalesActualMargin { get; set; } //Actual Margin(S.E)

        public double totalFOBValue { get; set; } //S.O FOB Amount(O.C)
        public double totalCFRValue { get; set; } //S.O Amount(O.C)
        public double totalBaseCFRValue { get; set; } //S.O Amount(M.E)
        public double SoAmountSER { get; set; } //S.O Amount(S.E)
        public double SoAmountPER { get; set; } //S.O Amount(PER)

        public double ManualCommissionME { get; set; }
        public double commisioninBase { get; set; } //Commision(M.E)
        public double commision { get; set; } //Commision(O.C)
        public double ManualCommissionSE { get; set; }
        public double commisioninSE { get; set; } //Commision(S.E)

        public double systemMarginOC { get; set; } //System Margin(OC)
        public double ManualSystemMarginSE { get; set; }
        public double systemMarginSE { get; set; } //System Margin(SE)
        public double ManualSystemMarginME { get; set; }
        public double systemMarginME { get; set; } //System Margin(ME)

        public double netCommision { get; set; } //Net Commission(OC)
        public double netCommisionSER { get; set; } //Net Commission(SER)
                                                    // public double netCommisionCMER { get; set; } //Net Commission(CMER)
        public double netCommisionMER { get; set; } //Net Commission(CMER)

        public double BMgrossProfitSE { get; set; } //[netCommisionSER] + [SalesBudgetedMargin]
        public double BMgrossProfitME { get; set; } //[netCommisionMER] + [BudgetedMargininBase]

        public double? TotalQuantity { get; set; } //Total Quantity

        public double TargetAchievedPercenatage { get; set; }

        public virtual List<TargetRewards> targetRewards { get; set; }
        //public virtual List<TargetRewards> targetFinanceRewards { get; set; }
        //public virtual List<TargetRewards> targetOtherRewards { get; set; }

        public int TotalOrdersCount { get; set; }
        public int UnderApprovalOrdersCount { get; set; }
        public int ApprovedOrdersCount { get; set; }
        public int UnderClosingOrdersCount { get; set; }
        public int ClosedOrdersCount { get; set; }

        public int? CalculationType_Id { get; set; }
        [ForeignKey("CalculationType_Id")]
        public virtual StatusCalculationType calculationType { get; set; }

        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }

        public bool isBasketed { get; set; }

        public object GetPropertyValue(string propertyName)
        {
            //returns value of property Name
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }
    }

    public class StatusCalculationType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string TypeName { get; set; }

        public int? AchievedField_Id { get; set; }
        [ForeignKey("AchievedField_Id")]
        public virtual SoCalculationFields AchievedField { get; set; }

        public int? TotalField_Id { get; set; }
        [ForeignKey("TotalField_Id")]
        public virtual SoCalculationFields TotalField { get; set; }
    }

    public class SoCalculationFields
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string DisplayName { get; set; }
        public string SOFieldName { get; set; }
    }


    public class TargetRewards
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual TargetRewardStatus Status { get; set; }

        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual ERP_BL.Databases.User user { get; set; }

        [InverseProperty("targetRewards")]
        public virtual List<Payment> Payments { get; set; }

        public double RewardAmount { get; set; }

        public int? toDoTask_Id { get; set; }
        [ForeignKey("toDoTask_Id")]
        [InverseProperty("targetRewards")]
        public virtual ToDoTask toDoTask { get; set; }

        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency currency { get; set; }

        public int? targetRewardNatureId { get; set; }
        [ForeignKey("targetRewardNatureId")]
        public virtual TargetRewardNature targetRewardNature { get; set; }

        public FunctionType functionType { get; set; }

        public bool isApplied { get; set; }
        public DateTime? AppliedDate { get; set; }

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

        public int? creator_Id { get; set; }
        [ForeignKey("creator_Id")]
        public virtual ERP_BL.Databases.User creator { get; set; }
        public virtual ICollection<JournalTransaction> journalTransactions { get; set; }
        public string financeRefNo { get; set; }

        public DateTime? GlPostingDate { get; set; }
        public double MER { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
    }

    public class TargetRewardNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string NatureName { get; set; }
        public bool isActive { get; set; }
    }

    public class TaskTargetType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string TargetTypeName { get; set; }
        public bool isActive { get; set; }
    }

    public class ToDoTaskTheme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ThemeColorCode { get; set; }
    }

    public class ToDoTaskStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<ToDoTask> payments { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool forStep { get; set; }
        public bool forTask { get; set; }
        public double MinPercentage { get; set; }
        public double MaxPercentage { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("todoTaskStatuses")]
        public virtual List<StatusClass> todoTaskStatusSubClasses { get; set; }
    }

    public class TargetRewardStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<TargetRewards> TargetRewards { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("targetRewardStatuses")]
        public virtual List<StatusClass> targetRewardStatusSubClasses { get; set; }

    }

    public class TargetGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string GroupName { get; set; }

        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual TargetGroup targetGroup { get; set; }

        //[InverseProperty("TargetGroups")]
        //public virtual ICollection<ToDoTask> ToDoTasks { get; set; }
    }
}
