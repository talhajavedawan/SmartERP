using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.InterBankTransfers
{
   public class STL 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public double paymentGroupId { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        public virtual Company company { get; set; }
        public int? dept_Id { get; set; }
        [ForeignKey("dept_Id")]
        public virtual Department department { get; set; }

        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual ERP_BL.Databases.User user { get; set; }
        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual STLStatus stlStatus { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public DateTime? stlPaymentDate { get; set; }
        public DateTime? creditTenure { get; set; }
        public DateTime? extendedCreditTenure { get; set; }
        public double creditTenureNo { get; set; }
        public double extendedCreditTenureNo { get; set; }
        public double paymentDueDays { get; set; }
        public double stlUtilizedDays { get; set; }
        public double paymentAmountOC { get; set; }
        public double interestPercent { get; set; }
        public int? interestAmountCurrency_Id { get; set; }
        [ForeignKey("interestAmountCurrency_Id")]
        public virtual Currency interestAmountCurrency { get; set; }
        public string stage { get; set; }
        public string salesReferenceNo { get; set; }
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
       
        public int? paymentBank_Id { get; set; }
        [ForeignKey("paymentBank_Id")]
        public virtual Bank paymentBank { get; set; }

        public int? paymentCurrency_Id { get; set; }
        [ForeignKey("paymentCurrency_Id")]
        public virtual Currency paymentCurrency { get; set; }

        public int? paymentAccount_Id { get; set; }
        [ForeignKey("paymentAccount_Id")]
        public virtual Account paymentAccount { get; set; }
        public double settlmentAmount { get; set; }
        public double settlmentBalance { get; set; }

        public double marginReversal { get; set; }

        public int? stlBank_Id { get; set; }
        [ForeignKey("stlBank_Id")]
        public virtual Bank stlBank { get; set; }
        public int? stlCurrency_Id { get; set; }
        [ForeignKey("stlCurrency_Id")]
        public virtual Currency stlCurrency { get; set; }

        public double stlPaymentAmountOC { get; set; }
        public int? stlAccount_Id { get; set; }
        [ForeignKey("stlAccount_Id")]
        public virtual Account stlAccount { get; set; }
        public int? cashMarginBank_Id { get; set; }
        [ForeignKey("cashMarginBank_Id")]
        public virtual Bank cashMarginbank { get; set; }

        public int? cashMarginCurrency_Id { get; set; }
        [ForeignKey("cashMarginCurrency_Id")]
        public virtual Currency cashMarginCurrency { get; set; }
        public double paymentAmountSTL { get; set; }
        public double paymentSTLER { get; set; }
        public double paymentAmountSTLMER { get; set; }
        public int? cashMarginDrAccount_Id { get; set; }
        [ForeignKey("cashMarginDrAccount_Id")]
        public virtual Account cashMarginDrAccount { get; set; }

        public int? cashMarginCrAccount_Id { get; set; }
        [ForeignKey("cashMarginDrAccount_Id")]
        public virtual Account cashMarginCrAccount { get; set; }

        public double cashMarginPercent { get; set; }
        public double cashMarginAmount { get; set; }
        public string stlRef { get; set; }
        public DateTime? paymentMaturityDate { get; set; }
        public DateTime? GLPostingDate { get; set; }
        public double InterestAmount { get; set; }
        public double InterestAmountCD { get; set; }
        public DateTime? paymentDate { get; set; }
        public int? interest_Id { get; set; }
        [ForeignKey("interest_Id")]
        public virtual STLInterest interest { get; set; }

        public int? cashMarginPerc_Id { get; set; }
        [ForeignKey("cashMarginPerc_Id")]
        public virtual MarginPercentage cashMarginPerc { get; set; }

        public double SER { get; set; }
        public double MER { get; set; }
        public double STLAmountSER { get; set; }
        public double STLAmountMER { get; set; }
        public int? customer_Id { get; set; }
        [ForeignKey("customer_Id")]
        public virtual CustomerCompany customer { get; set; }

        public int? vendor_Id { get; set; }
        [ForeignKey("vendor_Id")]
        public virtual Vendor vendor { get; set; }

        public virtual List<JournalTransaction> journalTransactions { get; set; }
        public virtual List<STLSettlement> Settlements { get; set; }
        public virtual List<ReversalSettlement> ReversalSettlements { get; set; }
        public virtual List<InterBankTransfer> InterBankTransfers { get; set; }
        public virtual List<InterCompanyBankTransfer> InterCompanyBankTransfers { get; set; }

        public string stlRemarks { get; set; }


    }
    public class STLStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<STL> stls { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }


    }
    public class STLInterestType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool isActive { get; set; }
    }

    public class STLInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? interestTypeId { get; set; }
        [ForeignKey("interestTypeId")]
        public virtual STLInterestType interestType { get; set; }

        public double percentage { get; set; }

        public int? COA_Id { get; set; }
        [ForeignKey("COA_Id")]
        public virtual ChartofAccount chartofAccount { get; set; }

        public bool isAdjusted { get; set; }
        public bool isManual { get; set; }
    }
    public class MarginPercentage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? marginpercentageTypeId { get; set; }
        [ForeignKey("marginpercentageTypeId")]
        public virtual MarginPercentageType marginpercentageType { get; set; }

        public double percentage { get; set; }

        public int? COA_Id { get; set; }
        [ForeignKey("COA_Id")]
        public virtual ChartofAccount chartofAccount { get; set; }

        public bool isAdjusted { get; set; }
        public bool isManual { get; set; }
    }
    public class MarginPercentageType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool isActive { get; set; }
    }
    public class STLSettlement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string refNO { get; set; }
        public double settlementAmount { get; set; }

        public int? stl_Id { get; set; }
        [ForeignKey("stl_Id")]
        public virtual STL STL { get; set; }
    }
    public class ReversalSettlement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string refNO { get; set; }

        public double reversalSettlementAmount { get; set; }

        public int? stl_Id { get; set; }
        [ForeignKey("stl_Id")]
        public virtual STL STL { get; set; }
    }
}
