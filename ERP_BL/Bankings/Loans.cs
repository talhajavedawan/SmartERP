using ERP_BL.Databases;
using ERP_BL.Procurements.StatusClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Bankings
{
    //comment
    [Table("tabLoan")]
    public class Loans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime? LimitDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ExtensionDate { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public int? bankId { get; set; }
        [ForeignKey("bankId")]
        public virtual Bank bank { get; set; }

        public int? accountId { get; set; }
        [ForeignKey("accountId")]
        public virtual Account account { get; set; }

        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency currency { get; set; }

        public int? MainLimitNatureId { get; set; }
        [ForeignKey("MainLimitNatureId")]
        public virtual FacilityNature MainLimitFacilityNature { get; set; }

        public int? SubLimitNatureId { get; set; }
        [ForeignKey("SubLimitNatureId")]
        public virtual FacilityNature SubLimitFacilityNature { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual LoansStatus Status { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.Employee Creator { get; set; }

        public double MainLimitAmount { get; set; }
        public double SubLimitAmount { get; set; }

        public int transactionGroupId { get; set; }

        public string SystemRefNo { get; set; }
        public string FinanceRefNo { get; set; }

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
        public virtual ERP_BL.Databases.User user { get; set; }

        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
    }

    public class LoansStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<Loans> loans { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("loanStatuses")]
        public virtual List<StatusClass> loanStatusSubClasses { get; set; }
    }


    public class FacilityNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NatureName { get; set; }
        public bool isActive { get; set; }
    }
}
