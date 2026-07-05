using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Enums;
using ERP_BL.Procurements.StatusClass;

namespace ERP_BL.Databases
{

    public class Inquiry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string referenceNo { get; set; } // customer side number
        public string SalesReferenceNo { get; set; }  // internal number
        public DateTime inquiryDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }

        public DateTime DeliveryDueDate { get; set; }
        public DateTime alertDate { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? TotalQuantity { get; set; }
        public DateTime lastSubmissionDate { get; set; }
        public string OwnDescription{ get; set; }
        public string Comments { get; set; }
        public bool isVoid { get; set; } = false;

        public int customerCompany_Id { get; set; }
        [ForeignKey("customerCompany_Id ")]
        public virtual CustomerCompany customerCompany { get; set; }
        public int dept_Id { get; set; }
        [InverseProperty("DepartmentInquiries")]
        [ForeignKey("dept_Id ")]
        public virtual Department department { get; set; }

        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("CompanyInquiries")]
        public virtual Company company { get; set; }
        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentInquiries")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? InterCompany_Id { get; set; }
        [ForeignKey("InterCompany_Id ")]
        [InverseProperty("InterCompanyInquiries")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }

        public DateTime? ApprovedDate { get; set; }
        public bool? isApproved { get; set; } = true;
        public bool? PendingForClosing { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public string stage { get; set; }

        public int user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }

        public int? allocation_Id { get; set; }
        [ForeignKey("allocation_Id")]
        public virtual Employee employee {get;set;}
        
        //public virtual List<ViewInfo> ViewsInfo { get; set; }
        public InquiryType inquirytype { get; set; }
        public virtual List<InquiryProduct>   products { get; set; }

        public virtual InquiryStatus inquiryStatus { get; set; }
        public string emailBody { get; set; }
        public string emailSubject { get; set; }
        public int? transactionHolderId { get; set; }
        [ForeignKey("transactionHolderId")]
        public Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }

        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
    }
   

    public class InquiryStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set;}
        public bool isApproved { get; set; }

        public bool? isActive { get; set; }
        public virtual List<Inquiry> inquiries { get; set; }

        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public bool isVoid { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("inquiryStatuses")]
        public virtual List<StatusClass> inquiryStatusSubClasses { get; set; }

    }

}
