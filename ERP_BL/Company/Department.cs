using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.ChartofAccounts;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.CreditCards;
using ERP_BL.Payments;
using ERP_BL.ToDoTasks;
using ERP_BL.Reports;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.ToDoTasks.Taskss;
using ERP_BL.Procurements;
using ERP_BL.Documents;

namespace ERP_BL.Databases
{

    [Table("tabDepartment")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DeptName { get; set; }
        [StringLength(20)]
        public string Code { get; set; }
        [StringLength(20)]
        public string Abbrivation { get; set; }
        public DateTime Timestamp { get; set; }
        //public int CompID { get; set; }
        //[ForeignKey("CompID")]

        // public ICollection<Company> company { get; set; }
        public int? userID { get; set; }
        [ForeignKey("userID")]
        public virtual User user { get; set; }
        public bool IsParent { get; set; }
        public bool IsSubsidary { get; set; }
        public bool IsAdminBillType { get; set; }
        public bool IsManagerial { get; set; }
        [InverseProperty("CashflowDepartments")]
        public List<ERP_BL.CashFlow.CashFlow> Cashflows { get; set; }
        public bool IsProcurementType { get; set; }
        public bool IsInquiryType { get; set; }
        public bool IsOfferType { get; set; }
        public bool IsModuleContractType { get; set; }
        public bool IsSaleOrderType { get; set; }
        public bool IsSaleInvoiceType { get; set; }
        public bool IsSaleReceiptType { get; set; }
        public bool IsPurchaseOrderType { get; set; }
        public bool IsPurchaseInvoiceType { get; set; }
        public bool IsPaymentType { get; set; }

        public bool IsInventoryType { get; set; }
        public bool IsVendorBillType { get; set; }
        public bool IsInterBankTransferType { get; set; }
        public bool IsInterCompTransferType { get; set; }
        public bool IsLoansAdvancesType { get; set; }
        public bool IsTaskType { get; set; }
        public bool IsTravelingRecordType { get; set; }
        public bool IsAssetType { get; set; }
        public bool IsRentalContractType { get; set; }
        public bool IsRentalOrderType { get; set; }
        public bool IsRentalInvoiceType { get; set; }
        public bool IsRentalReceiptType { get; set; }
        public bool IsDocumentType { get; set; }


        [InverseProperty("CoreDepartment")]
        public virtual List<Employee> CoreEmployees { get; set; }

        public int? ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual Department parentDepartment { get; set; }


        public int? LevelID { get; set; }
        [ForeignKey("LevelID")]
        public virtual DepartmentLevel departmentLevel { get; set; }
        public virtual ICollection<Department> subDepartments { get; set; }
        public virtual ICollection<Company> companies { get; set; }
        public virtual ICollection<Employee> employees { get; set; }
        [InverseProperty("departments")]
        public virtual ICollection<CustomerCompany> customers { get; set; }
        [InverseProperty("disableDepartments")]
        public virtual ICollection<CustomerCompany> disableCustomers { get; set; }
        //public virtual List<User> users { get; set; }

        public virtual List<Target> Targets { get; set; }



        public virtual List<Product> Products { get; set; }
        [InverseProperty("departments")]


        public virtual List<Vendor> Vendors { get; set; }
        [InverseProperty("")]
        public virtual List<Principal> Principals { get; set; }
        public bool isActive { get; set; } = true;

        public bool applyMERasSER { get; set; }
        public virtual ICollection<Account> accounts { get; set; }
        public virtual ICollection<Payee> payees { get; set; }
        public virtual ICollection<CreditCard> CreditCards { get; set; }

        public virtual List<Asset> Assets { get; set; }
        [InverseProperty("InterDepartment")]
        public virtual List<Inquiry> InterDepartmentInquiries { get; set; }
        [InverseProperty("InterDepartment")]
        public virtual List<Offer> InterDepartmentOffers { get; set; }

        [InverseProperty("InterDepartment")]
        public virtual List<ModuleContract> InterDepartmentModuleContracts { get; set; }
        [InverseProperty("InterDepartment")]
        public virtual List<SaleOrder> InterDepartmentSaleOrders { get; set; }
        [InverseProperty("InterDepartment")]

        public virtual List<SaleInvoice> InterDepartmentSaleInvoices { get; set; }
        [InverseProperty("InterDepartment")]


        public virtual List<PurchaseOrder> InterDepartmentPurchaseOrders { get; set; }
        [InverseProperty("InterDepartment")]


        public virtual List<MemorandumSale> InterDepartmentMemorandumSales { get; set; }
        [InverseProperty("InterDepartment")]
        public virtual List<Bill> InterDepartmentBills { get; set; }
        [InverseProperty("department")]
        public virtual List<Inquiry> DepartmentInquiries { get; set; }
        [InverseProperty("department")]
        public virtual List<Offer> DepartmentOffers { get; set; }

        [InverseProperty("department")]
        public virtual List<ModuleContract> DepartmentModuleContracts { get; set; }
        [InverseProperty("department")]
        public virtual List<SaleOrder> DepartmentSaleOrders { get; set; }
        [InverseProperty("department")]
        public virtual List<AuditYearAdjustment> DepartmentAuditAdjustmnets { get; set; }

        [InverseProperty("department")]
        public virtual List<SaleInvoice> DepartmentSaleInvoices { get; set; }
        [InverseProperty("department")]
        public virtual List<PurchaseOrder> DepartmentPurchaseOrders { get; set; }

        [InverseProperty("department")]
        public virtual List<Document> DepartmentDocuments { get; set; }

        [InverseProperty("department")]
        public virtual List<MemorandumSale> DepartmentMemorandumSales { get; set; }
        [InverseProperty("department")]
        public virtual List<Bill> DepartmentBills { get; set; }

        [InverseProperty("loanAdvanceDepartment")]
        public virtual List<Bill> loanAdvanceDepartmentBills { get; set; }
        public int? chartofAccountId { get; set; }
        [ForeignKey("chartofAccountId")]
        public virtual ChartofAccount ChartofAccount { get; set; }
        [InverseProperty("Departments")]
        public virtual ICollection<ChartofAccount> chartofAccounts { get; set; }
        [InverseProperty("InterDepartment")]
        public virtual List<PurchaseInvoice> InterDepartmentPurchaseInvoices { get; set; }
        [InverseProperty("department")]
        public virtual List<PurchaseInvoice> DepartmentPurchaseInvoices { get; set; }
        [InverseProperty("InterDepartment")]
        public virtual List<Payment> InterDepartmentPayments { get; set; }
        [InverseProperty("departments")]
        public virtual ICollection<Payment> payments { get; set; }
        public int? accountPayableId { get; set; }
        [ForeignKey("accountPayableId")]

        public virtual ChartofAccount AccountPayable { get; set; }

        [InverseProperty("Departments")]
        public virtual List<TaskGroups> TaskGroups { get; set; }

        [InverseProperty("DepartmentsBulk")]
        public virtual List<TaskGroups> TaskGroupsBulk { get; set; }

        [InverseProperty("departments")]
        public virtual List<DocumentType> DocumentTypes { get; set; }

        [InverseProperty("departments")]
        public virtual List<DocumentTemplate> DocumentTemplates { get; set; }

        public virtual ICollection<SharedGridGroup> SharedGridGroups { get; set; }

        [InverseProperty("departments")]
        public virtual ICollection<LoanApplicant> loanApplicants { get; set; }

        [InverseProperty("departments")]
        public virtual ICollection<TaskType> taskTypes { get; set; }

        [InverseProperty("Departments")]
        public virtual ICollection<SalesReceipt> SalesReceipts { get; set; }

        [InverseProperty("Departments")]
        public virtual ICollection<ChartofAccountGroup> ChartofAccountGroups { get; set; }

        public Department()
        { }
        public bool isLinkable { get; set; }

    }

    public class DepartmentLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }

        public int? ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual DepartmentLevel parentLevel { get; set; }
    }

}
