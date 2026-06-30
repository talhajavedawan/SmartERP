using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;
using ERP_BL.ChartofAccounts;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Payments;
using ERP_BL.ToDoTasks;
using ERP_BL.Reports;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.ToDoTasks.Taskss;
using ERP_BL.Procurements;
using ERP_BL.Documents;

namespace ERP_BL.Databases
{


    [Table("tabCompany")]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        //public IndustryTypes IndustryType { get; set; }
        public int? industryTypeId { get; set; }
        [ForeignKey("industryTypeId")]
        public virtual IndustryType industryType { get; set; }

        [InverseProperty("CashflowCompanies")]
        public List<ERP_BL.CashFlow.CashFlow> Cashflows { get; set; }  

        [InverseProperty("Companies")]
        public virtual List<Vendor> Vendors { get; set; }

        public BizTypes BizType { get; set; }
        public string EmployeerNo { get; set; }
        public string CustomerVAT { get; set; }
        public string SaleTaxRegistrationNumber { get; set; } 

        public DateTime openingDate { get; set; }

        public DateTime? closingDate { get; set; }
        //public CompanyBasic basicInfo = new CompanyBasic();
        public int addressId { get; set; }
        [ForeignKey("addressId")]
        public virtual Address address { get; set; }
        public int contactId { get; set; }
        [ForeignKey("contactId")]
        public virtual Contact contact { get; set; }

        public CompnayTypes compnayType { get; set; }

        [InverseProperty("Companies")]
        public virtual List<CustomerCompany> Customers { get; set; }

        public virtual List<Department> departments { get; set; }

        public virtual List<Employee> employees { get; set; }

        [InverseProperty("AdminBillCompanies")]
        public virtual List<Employee> AdminBillEmployees { get; set; }

        [InverseProperty("TaskCompanies")]
        public virtual List<Employee> TaskEmployees { get; set; }

        public virtual List<Employee> PettyCashEmployees { get; set; }

        [InverseProperty("CoreCompany")]
        public virtual List<Employee> CoreEmployees { get; set; }

        public bool IsSubsidary { get; set; }

        public int? ParentID { get; set; }

        [ForeignKey("ParentID")]
        public virtual Company parentCompany { get; set; }

        public int? CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public virtual Currency currency { get; set; }
        public Company()
        {
            departments = new List<Department>();
        }
        public bool isActive { get; set; } = true;

        public virtual ICollection<AdminBillNature> AdminBillNatures { get; set; }
        public virtual ICollection<VendorBillNature> VendorBillNatures { get; set; }

        [InverseProperty("Companies")]
        public virtual ICollection<TaskGroups> TaskGroups { get; set; }

        [InverseProperty("CompaniesBulk")]
        public virtual ICollection<TaskGroups> TaskGroupsBulk { get; set; }

        public virtual List<Asset> Assets { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<Inquiry> InterCompanyInquiries { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<Offer> InterCompanyOffers { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<ModuleContract> InterCompanyModuleContract { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<SaleOrder> InterCompanySaleOrders { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<SaleInvoice> InterCompanySaleInvoices { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<PurchaseOrder> InterCompanyPurchaseOrders { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<MemorandumSale> InterCompanyMemorandumSales { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<Bill> InterCompanyBills { get; set; }
        [InverseProperty("Company")]
        public virtual List<Inquiry> CompanyInquiries { get; set; }
        [InverseProperty("company")]
        public virtual List<Offer> CompanyOffers { get; set; }
        [InverseProperty("company")]
        public virtual List<ModuleContract> CompanyModuleContracts { get; set; }
        [InverseProperty("company")]
        public virtual List<SaleOrder> CompanySaleOrders { get; set; } 
        [InverseProperty("company")]
        public virtual List<AuditYearAdjustment> CompanyAuditYearAdjustments { get; set; } 

        [InverseProperty("company")]
        public virtual List<SaleInvoice> CompanySaleInvoices { get; set; }
        [InverseProperty("company")]
        public virtual List<PurchaseOrder> CompanyPurchaseOrders { get; set; }
        [InverseProperty("company")]
        public virtual List<Document> CompanyDocuments { get; set; }
        [InverseProperty("company")]
        public virtual List<MemorandumSale> CompanyMemorandumSales { get; set; }

        [InverseProperty("company")]
        public virtual List<Bill> CompanyBills { get; set; }

        [InverseProperty("loanAdvanceCompany")]
        public virtual List<Bill> LoanAdvanceCompanyBills { get; set; }

        [InverseProperty("company")]
        public virtual List<SalesExchangeRate> salesExchangeRates{ get; set; }
        [InverseProperty("company")]
        public virtual List<MarketExchangeRate> marketExchangeRates { get; set; }
        public virtual  ICollection<Bank> banks{ get; set; }

        [InverseProperty("Companies")]
        public virtual ICollection<Payee> payees { get; set; }
        public virtual ICollection<ChartofAccount> chartofAccounts { get; set; }
        [InverseProperty("company")]
        public virtual List<PurchaseInvoice> CompanyPurchaseInvoices { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<PurchaseInvoice> InterCompanyPurchaseInvoices { get; set; }
        [InverseProperty("InterCompany")]
        public virtual List<Payment> InterCompanyPayments { get; set; }
        public virtual ICollection<SharedGridGroup> SharedGridGroups { get; set; }

        [InverseProperty("companies")]
        public virtual List<DocumentType> DocumentTypes { get; set; }

        [InverseProperty("companies")]
        public virtual List<DocumentTemplate> DocumentTemplates { get; set; }

        [InverseProperty("companies")]
        public virtual ICollection<LoanApplicant> loanApplicants { get; set; }

        [InverseProperty("companies")]
        public virtual ICollection<TaskType> taskTypes { get; set; }
        public bool isLinkable { get; set; }
        [InverseProperty("Companies")]
        public virtual ICollection<ChartofAccountGroup> ChartofAccountGroups { get; set; }

    }

    [Table("tabpUser")]
    public class pUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string userName { get; set; }
        public virtual string PasswordStored
        {
            get;
            set;
        }

        [NotMapped]
        public string Password
        {
            get { return Decrypt(PasswordStored); }
            set { PasswordStored = Encrypt(value); }
        }

        private string Encrypt(string value)
        {
            return value;
            //throw new NotImplementedException();
        }

        private string Decrypt(string passwordStored)
        {
            return passwordStored;
            //throw new NotImplementedException();
        }

        public pUser()
        {
            userName = "Administrator";
        }

    }
}
