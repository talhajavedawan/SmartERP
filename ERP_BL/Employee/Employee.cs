using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;
using ERP_BL.HR;
using ERP_BL.ChartofAccounts;
using ERP_BL.Reports;
using ERP_BL.Payments;

namespace ERP_BL.Databases
{
    public class Designation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesigId { get; set; }

        public string Title { get; set; }

        public bool isActive { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Designation parentDesignation { get; set; }

        public virtual List<Designation> SubOrdinates { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? departmentId { get; set; }
        [ForeignKey("departmentId")]
        public virtual Department department { get; set; }

        public double AnnualLeaveDays { get; set; }
        public double AnnualLeaveHours { get; set; }//Equivalent Hours
        public double CasualLeaveDays { get; set; }
        public double CasualLeaveHours { get; set; }//Equivalent Hours

        public int? userId { get; set; }
        [ForeignKey("userId")]
        public User user { get; set; }
    }


    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }
        //[NotMapped]
        public virtual Person person { get; set; }

        public int? coreCompanyId { get; set; }
        [ForeignKey("coreCompanyId")]
        public virtual Company CoreCompany { get; set; }

        public int? coreDeptId { get; set; }
        [ForeignKey("coreDeptId")]
        public virtual Department CoreDepartment { get; set; }

        public virtual List<Company> Companies { get; set; }
        public virtual List<Company> AdminBillCompanies { get; set; }
        public virtual List<Company> TaskCompanies { get; set; }

        [InverseProperty("PettyCashEmployees")]
        public virtual List<Company> PettyCashCompanies { get; set; }

        public virtual List<Department> departments { get; set; }
        public virtual List<SaleOrder> SaleOrders { get; set; }
        [InverseProperty("TransactionHolder")]
        public virtual List<SaleOrder> TransactionHolderSaleOrders { get; set; }
        [InverseProperty("TransactionHolder")]
        public virtual List<SaleInvoice> TransactionHolderSaleInvoices { get; set; }
        [InverseProperty("TransactionHolder")]
        public virtual List<SalesReceipt> TransactionHolderSaleReceipts { get; set; }

        [InverseProperty("TransactionHolder")]
        public virtual List<PurchaseOrder> TransactionHolderPurchaseOrders { get; set; }

        [InverseProperty("TransactionHolder")]
        public virtual List<PurchaseInvoice> TransactionHolderPurchaseInvoices { get; set; }
        [InverseProperty("TransactionHolder")]
        public virtual List<Payment> TransactionHolderPayments { get; set; }

        [InverseProperty("TransactionHolder")]
        public virtual List<Bill> TransactionHolderBills { get; set; }

        [InverseProperty("TransactionHolder")]
        public virtual List<Offer> TransactionHolderOffers { get; set; }

        [InverseProperty("TransactionHolder")]
        public virtual List<Inquiry> TransactionHolderInquiries { get; set; }
        public virtual Address address { get; set; }
        //[NotMapped]
        public virtual Contact contact { get; set; }
        //[NotMapped]
        //public int? DesigId { get; set; }
        //[ForeignKey("DesigId")]
        //public Desig Desig { get; set; }
        public virtual Designation Desig { get; set; }
        public string MaritalStatus { get; set; }
        public bool Disability { get; set; }
        public string DisDescription { get; set; }
        public bool isActive { get; set; }
        public virtual EmployeeStatus Status { get; set; }
        [Required]
        public DateTime? JoinDate { get; set; }
        public DateTime? HireDate { get; set; }

        public bool AllowOpenTransactions { get; set; }
        public DateTime? RetrievalDate { get; set; }
        public DateTime? InquiryDataRetrievalDate { get; set; }
        public DateTime? OfferDataRetrievalDate { get; set; }
        public DateTime? SODataRetrievalDate { get; set; }
        public DateTime? MemorandumSaleDataRetrievalDate { get; set; }
        public DateTime? SIDataRetrievalDate { get; set; }
        public DateTime? PODataRetrievalDate { get; set; }
        public DateTime? PIDataRetrievalDate { get; set; }
        public DateTime? VendorBillDataRetrievalDate { get; set; }
        public DateTime? SRDataRetrievalDate { get; set; }
        public DateTime? FixedAssetsDataRetrievalDate { get; set; }
        public DateTime? IBTDataRetrievalDate { get; set; }
        public DateTime? AdminBillDataRetrievalDate { get; set; }
        public DateTime? PaymentDataRetrievalDate { get; set; }
        public DateTime? LoansAdvancesDataRetrievalDate { get; set; }


        //public virtual User user { get; set; }
        [Required]
        public double BasicPay { get; set; }

        //[ForeignKey("Desig")]
        public int? SupervisorId { get; set; }

        [ForeignKey("SupervisorId")]
        
        public virtual Employee Supervisor { get; set; }
      

        public string DesignationTitle { get; set; }
        public string JobDescription { get; set; }
        public int? SalesTargetId { get; set; }

        [ForeignKey("SalesTargetId")]
        public virtual SalesTarget SalesTarget { get; set; }
        [InverseProperty("employee")]
        public virtual List<CommentLog> SentComments { get; set; }
        [InverseProperty("Assignee")]
       
     
        public virtual List<CommentLog> AssignedComments { get; set; }
	   
        public virtual List<Asset> Assets { get; set; }

        [InverseProperty("employee")]
        public virtual List<Qualification> Qualifications { get; set; }
        public virtual Address address2 { get; set; }

        //public int? empFunctionId { get; set; }
        //[ForeignKey("empFunctionId")]
        public virtual Function empFunction { get; set; }
        public string EmployeeId { get; set; } //Employee Id for company administration

      
      
        public virtual EmployeeWorkingStatus employeeStatus { get; set; }
        public virtual EmployeeApproval employeeApproval { get; set; }

        [InverseProperty("employee")]
        public virtual List<EmployeeWorkExperience> WorkExperience { get; set; }

        public virtual Emergencyontact emergencyontact { get; set; }

        public int? HrInfoId { get; set; }
        [ForeignKey("HrInfoId")]
        public virtual EmployeeHRInfo HRInfo { get; set; }
        //[InverseProperty("employee")]
        //public List<LeaveApplication> leaves { get; set; }
        public string empDescription { get; set; } //Employee description
        [InverseProperty("Employees")]
        public virtual ICollection<ChartofAccount> chartofAccounts { get; set; }

        public bool isAdminBillType { get; set; }
    
        public virtual ICollection<SharedGridGroup> SharedGridGroups { get; set; }

        public bool isTaskType { get; set; }

        public virtual ICollection<User> EmployeeUsers { get; set; }


        public bool isMultiUser { get; set; }

        public int? receivableAccountId { get; set; }
        [ForeignKey("receivableAccountId")]
        public virtual ChartofAccount receivableAccount { get; set; }

        [InverseProperty("Employees")]
        public virtual ICollection<ChartofAccountGroup> ChartofAccountGroups { get; set; }
    }
    public class SalesTarget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        public double TargetAmount { get; set; }
        public int? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency currency { get; set; }
        public bool? isAchieved { get; set; }
    }
    public class Function
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }

        //public int? ParentId { get; set; }

        //public virtual Function parentFunction { get; set; }

        //public virtual List<Function> SubOrdinates { get; set; }

        public bool? IsActive { get; set; }

        //public int? companyId { get; set; }
        //[ForeignKey("companyId")]

        public virtual Company company { get; set; }

        public FunctionType? functionType { get; set; }

        //public int? departmentId { get; set; }
        //[ForeignKey("departmentId")]
        //public virtual Department department { get; set; }

        //public int? userId { get; set; }
        //[ForeignKey("userId")]
        //public User user { get; set; }

        //public int? empId { get; set; }
        //[ForeignKey("empId")]
        //  public ERP_BL.Databases.Employee employee { get; set; }



        //  public HrInfo hrInfo { get; set; }
    }
    public class Qualification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //public DegreeType DegreeType { get; set; }
        public string DegreeType { get; set; }
        public string DegreeTitle { get; set; }
        public string Specialization { get; set; }
        public double MarksObtained { get; set; }
        public double MarksTotal { get; set; }
        public double MarksPercentage { get; set; }
        public string Division { get; set; }
        public DateTime? StartYear { get; set; }
        public DateTime? PassingYear { get; set; }
        public string Institute { get; set; }
        public string Location { get; set; }
        public bool IsLatest { get; set; }
        public bool IsDistinction { get; set; }
        public string DistDetails { get; set; }
        public bool IsValid { get; set; } //ForCertificates
        public DateTime? validTill { get; set; }//ForCertificates
       
        public string Score { get; set; }//ForCertificates
        public bool IsCompleted { get; set; }
        public int? employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual Employee employee { get; set; }

    }
    public class EmployeeWorkingStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<Employee> Employees { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }

    }

    public class EmployeeApproval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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

    public class EmployeeWorkExperience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Company { get; set; }
        public string JobDescription { get; set; }
        public string JobTitle { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string employerAddress { get; set; }
        public string employerContact { get; set; }
        public int? employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual Employee employee { get; set; }
        public bool isLatest { get; set; }
    }

    public class Emergencyontact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }
        public string contact { get; set; }
        public string Address { get; set; }
    }
    public class EmployeeCoaCompanies
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? compId { get; set; }
        [ForeignKey ("compId")]
        public virtual Company Company { get; set; }

        public int? EmpId { get; set; }
        [ForeignKey("EmpId")]
        public virtual Employee Emoployee { get; set; }

    }

}
