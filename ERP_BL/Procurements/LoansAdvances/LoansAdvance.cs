using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.AdminBills;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.LoansAdvances
{
    public class LoansAdvance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public LoansAdvanceTemplate advanceTemplate { get; set; }
        public LoansAdvanceType loansAdvanceType { get; set; }

        public int transactionGroupId { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User Creator { get; set; }

        public string SystemRef { get; set; }
        public bool isEmployee { get; set; }

        public int? applicantTypeId { get; set; }
        [ForeignKey("applicantTypeId")]
        public virtual LoanApplicantType applicantType { get; set; }

        public int? applicantId { get; set; }
        [ForeignKey("applicantId")]
        public virtual LoanApplicant applicant { get; set; }

        public int? employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual ERP_BL.Databases.Employee ApplicantEmployee { get; set; }

        public int? vendorId { get; set; }
        [ForeignKey("vendorId")]
        public virtual Vendor vendor { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual LoansAdvanceStatus Status { get; set; }

        public double AppliedAmountOC { get; set; }
        public double LoanAmountOC { get; set; }
        public double MER { get; set; }
        public double LoanAmountMER { get; set; }

        public string Purpose { get; set; }

        public int LoanTenureDays { get; set; }
        public DateTime? LoanReturnDate { get; set; }

        [InverseProperty("loansAdvance")]
        public virtual List<Payment> Payments { get; set; }

        [InverseProperty("loansAdvance")]
        public virtual List<SalesReceipt> SalesReceipts { get; set; }

        [InverseProperty("loansAdvance")]
        public virtual List<AdminBill> AdminBills { get; set; }

        [InverseProperty("loansAdvance")]
        public virtual List<ERP_BL.Databases.Bill> bills { get; set; }

        public int? SaleInvoiceId { get; set; }
        [ForeignKey("SaleInvoiceId")]
        [InverseProperty("LoansAdvances")]
        public virtual SaleInvoice SaleInvoice { get; set; }

        public int? saleOrderId { get; set; }
        [ForeignKey("saleOrderId")]
        [InverseProperty("LoansAdvances")]
        public virtual SaleOrder saleOrder { get; set; }

        public int? purchaseOrderId { get; set; }
        [ForeignKey("purchaseOrderId")]
        [InverseProperty("LoansAdvances")]
        public virtual PurchaseOrder purchaseOrder { get; set; }

        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency currency { get; set; }

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

        public int? PettyCashRefId { get; set; }
        [ForeignKey("PettyCashRefId")]
        public virtual BillRefNumber PettyCashRef { get; set; } //It is used as a Petty Cash Ref No but previously created for Admin Bill on requirement

        public bool? isDeposit { get; set; }
        public virtual List<PettyCash> pettyCashes { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual ERP_BL.Procurements.StatusClass.StatusClass StatusClass { get; set; }
        public virtual List<ERP_BL.VATBook.VATBook> VATBooks { get; set; }

        public int? VATBookRefId { get; set; }
        [ForeignKey("VATBookRefId")]
        public virtual ERP_BL.VATBook.VATBookRefNumber VATBookRefNumber { get; set; }

        public bool isLinkable { get; set; }
    }

    public class LoanApplicantType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string TypeName { get; set; }
        public int? accountId { get; set; }
        [ForeignKey("accountId")]
        public virtual ChartofAccount account { get; set; }

        public bool LenderType { get; set; }
    }

    public class LoanApplicant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int? applicantTypeId { get; set; }
        [ForeignKey("applicantTypeId")]
        public virtual LoanApplicantType applicantType { get; set; }

        //[InverseProperty("loanApplicants")]
        public virtual List<Company> companies { get; set; }

        //[InverseProperty("loanApplicants")]
        public virtual List<Department> departments { get; set; }

        public bool LenderType { get; set; }
    }

    public class LoansAdvanceStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<LoansAdvance> payments { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("laStatuses")]
        public virtual List<ERP_BL.Procurements.StatusClass.StatusClass> laStatusSubClasses { get; set; }
    }
}
