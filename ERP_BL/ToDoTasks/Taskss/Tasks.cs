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

namespace ERP_BL.ToDoTasks.Taskss
{
    public class Tasks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public TaskTemplate? taskTemplate { get; set; }

        public DateTime? creationDate { get; set; }

        public int SystemId { get; set; }
        public int transactionId { get; set; }
        public TransactionItemType? transactionType { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public int? customerId { get; set; }
        [ForeignKey("customerId")]
        public virtual CustomerCompany CustomerCompany { get; set; }

        [InverseProperty("Tasks")]
        public virtual List<ERP_BL.Databases.User> AllowedUsers { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual TasksStatus Status { get; set; }

        public int? supervisedById { get; set; }
        [ForeignKey("supervisedById")]
        public virtual ERP_BL.Databases.User supervisedBy { get; set; }

        public int? assignedToId { get; set; }
        [ForeignKey("assignedToId")]
        public virtual ERP_BL.Databases.User assignedTo { get; set; }

        public int? assignedById { get; set; }
        [ForeignKey("assignedById")]
        public virtual ERP_BL.Databases.User assignedBy { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User creator { get; set; }

        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency currency { get; set; }

        public int? saleOrderId { get; set; }
        [ForeignKey("saleOrderId")]
        public virtual SaleOrder saleOrder { get; set; }

        public int? purchaseOrderId { get; set; }
        [ForeignKey("purchaseOrderId")]
        public virtual PurchaseOrder purchaseOrder { get; set; }

        public int? saleInvoiceId { get; set; }
        [ForeignKey("saleInvoiceId")]
        public virtual SaleInvoice saleInvoice { get; set; }

        public int? inquiryId { get; set; }
        [ForeignKey("inquiryId")]
        public virtual Inquiry inquiry { get; set; }

        public int? offerId { get; set; }
        [ForeignKey("offerId")]
        public virtual Offer offer { get; set; }

        public int? moduleContractId { get; set; }
        [ForeignKey("moduleContractId")]
        public virtual ERP_BL.Procurements.ModuleContract moduleContract { get; set; }
        public string SystemRef { get; set; }

        public string TaskRef { get; set; }

        public double ManualAmount { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? TentativeCompletionDate { get; set; }
        public DateTime? CompletionDate { get; set; }

        public bool isCompleted { get; set; }

        public string Description { get; set; }

        public virtual List<TaskTracking> TaskTrackings { get; set; }

        public virtual List<TaskEfficiency> TaskEfficiencies { get; set; }

        public int? taskTypeId { get; set; }
        [ForeignKey("taskTypeId")]
        public virtual TaskType taskType { get; set; }

        public int? vendorId { get; set; }
        [ForeignKey("vendorId")]
        public virtual Vendor vendor { get; set; }

        public int? employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual ERP_BL.Databases.Employee filerEmployee { get; set; }

        public double? taxableIncome { get; set; }
        public double? chargeableTax { get; set; }
        public string NTNno { get; set; }

        public DateTime? taxYear { get; set; }
        [Range(0, 4)]
        public int? TaxQuarter { get; set; }
        public string FilerName { get; set; }
        public double? depositedTax { get; set; }
        public string NoticeRefNo { get; set; }

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
        public string stage { get; set; }


        public bool OfficeSupportRequired { get; set; }
        public bool LositicSupportRequired { get; set; }
        public string LogisticAreaFrom { get; set; }
        public string LotNo { get; set; }
        public string LogisticAreaTo { get; set; }
        public bool inHouseLogistics { get; set; }
        public bool outsourceLogistics { get; set; }
        public DateTime? PlannedExecutionDate { get; set; }
        public DateTime? FinalExecutionDate { get; set; }

        public string TrackingNoIn { get; set; }
        public DateTime? ETDin { get; set; } //Estimated Time of Delivery (In)
        public DateTime? ETAin { get; set; }//Estimated Time of Arrival (In)
        public DateTime? ADDin { get; set; }

        public string TrackingNoOut { get; set; }
        public DateTime? ETDout { get; set; }//Estimated Time of Delivery (Out)
        public DateTime? ETAout { get; set; }//Estimated Time of Arrival (Out)
        public DateTime? ADDout { get; set; }

        public int? warehouseId { get; set; }
        [ForeignKey("warehouseId")]
        public virtual Warehouse warehouse { get; set; }

        public int? lotNumberId { get; set; }
        [ForeignKey("lotNumberId")]
        public virtual LotNumber lotNumber { get; set; }

        public bool ImportBillOfEntry { get; set; }
        public bool ExportBillOfEntry { get; set; }

        public int? checklistId { get; set; }
        [ForeignKey("checklistId")]
        public virtual Checklist checklist { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
        
        public virtual List<TaskComment> TaskComments { get; set; }
        public double InputTax { get; set; }
        public double OutputTax { get; set; }
        public double RefundAmountClaim { get; set; }
        public double CreditCarriedForward { get; set; }
        public double AccumulatedCredit { get; set; }
        public double AccumulatedDebit { get; set; }
        public double FEDpayable { get; set; }
        public double PLpayable { get; set; }
        public double SaleTaxPayable { get; set; }
        public double TotalAmountPaid { get; set; }
        public DateTime? TaxFrom { get; set; }//Estimated Time of Arrival (Out)
        public DateTime? TaxTo { get; set; }

    }

    public class Checklist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? creationDate { get; set; }
        public TransactionItemType TransactionType { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public ERP_BL.Databases.User creator { get; set; }

        public virtual ICollection<ProcurementProduct> products { get; set; }
    }

 public class TaskComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? goodReceiveNoteId { get; set; }
        [ForeignKey("goodReceiveNoteId")]
        public virtual GoodReceiveNote goodReceiveNote { get; set; }

        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User user { get; set; }

        public int? taskId { get; set; }
        [ForeignKey("taskId")]
        [InverseProperty("TaskComments")]
        public virtual Tasks task { get; set; }

        public string Comment { get; set; }
    }

    public class GoodReceiveNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Warehouse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string WarehouseName { get; set; }
        public bool isActive { get; set; }
    }

    //Used in multiple modules
    public class LotNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public string LotNo { get; set; }

        public bool isActive { get; set; }
    }

    public class TasksStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<Tasks> tasks { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("taskStatuses")]
        public virtual List<ERP_BL.Procurements.StatusClass.StatusClass> taskStatusSubClasses { get; set; }


        [InverseProperty("TaskStatusess")]
        public virtual List<TaskType> TaskTypes { get; set; }

    }

    public class TaskTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? tasksId { get; set; }
        [ForeignKey("tasksId")]
        public virtual Tasks tasks { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public int? UpdatedById { get; set; }
        [ForeignKey("UpdatedById")]
        public virtual ERP_BL.Databases.User UpdatedBy { get; set; }

        public string Description { get; set; }
    }

    public class TaskEfficiency
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? efficiencyPoints_Id { get; set; }
        [ForeignKey("efficiencyPoints_Id")]
        public virtual EfficiencyPoints efficiencyPoints { get; set; }

        public int? tasksId { get; set; }
        [ForeignKey("tasksId")]
        [InverseProperty("TaskEfficiencies")]
        public virtual Tasks tasks { get; set; }

        public double TotalPoints { get; set; }
        public double AchievedPoints { get; set; }
    }


    public class EfficiencyPoints
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string title { get; set; }
        public bool isActive { get; set; }

        public double Points { get; set; }
    }

    public class TaskType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string TypeName { get; set; }

        public bool isProcurementType { get; set; }

        public bool isSaleOrder { get; set; }
        public bool isPurchaseOrder { get; set; }
        public bool isSaleInvoice { get; set; }
        public bool isOffer { get; set; }
        public bool isInquiry { get; set; }

        [InverseProperty("TaskTypes")]
        public virtual List<TasksStatus> TaskStatusess { get; set; }

        //[InverseProperty("loanApplicants")]
        public virtual List<Company> companies { get; set; }

        //[InverseProperty("loanApplicants")]
        public virtual List<Department> departments { get; set; }
    }

}
