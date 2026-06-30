using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Procurements.StatusClass;

namespace ERP_BL.Procurements
{
    public class ModuleContract
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string OfferReferenceNo { get; set; } 
        public string SalesReferenceNo { get; set; }  
        public string ModuleContractReferenceNo { get; set; } 
        public string commisionRefrenceNo { get; set; }  
        public DateTime? ModuleContractDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public DateTime? OfferDate { get; set; }
        public DateTime? ModuleContractValidityDate { get; set; }
        public DateTime? deliveryDate { get; set; }
        public string maker { get; set; }
        public string origin { get; set; }
        public string OwnDescription { get; set; }
        public bool isVoid { get; set; } = false;
        public DateTime? responseDate { get; set; }
        public float exchngeRate { get; set; }
        public DateTime? bidOpenDate { get; set; }
        public DateTime? alertDate { get; set; }
        public DateTime? closingDate { get; set; }
        public decimal? commision { get; set; }
        public decimal marginExchangeRate { get; set; } 
        public decimal? margin { get; set; } 
        public string comments { get; set; }    
       
        public double totalFOBValue { get; set; }
        public double totalCFRValue { get; set; }
        public double totalBaseFOBValue { get; set; }
        public double totalBaseCFRValue { get; set; }
        public bool isPercentTax { get; set; }
        public double salesTax { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? TotalQuantity { get; set; }
        public int customerCompany_Id { get; set; }
        [ForeignKey("customerCompany_Id ")]
        public virtual CustomerCompany customerCompany { get; set; }
        public int dept_Id { get; set; }
        [InverseProperty("DepartmentModuleContracts")]
        [ForeignKey("dept_Id ")]
        public virtual Department department { get; set; }
        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual ERP_BL.Databases.User user { get; set; }
        public int allocation_Id { get; set; }
        [ForeignKey("allocation_Id")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }
        [InverseProperty("ModuleContracts")]
        public virtual List<Vendor> vendors { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("CompanyModuleContracts")]
        public virtual Company company { get; set; }
        public InquiryType ModuleContracttype { get; set; }
        public int? principal_Id { get; set; }
        [ForeignKey("principal_Id")]
        public virtual Principal principal { get; set; }
        public int? offer_Id { get; set; }
        [ForeignKey("offer_Id ")]
        public virtual Offer offer { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public string stage { get; set; }
        public int? bid_Id { get; set; }
        [ForeignKey("bid_Id ")]
        public virtual Bid bid { get; set; }
        public int? currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency currency { get; set; }
        public virtual List<ProcurementProduct> products { get; set; }
        public virtual ModuleContractStatus ModuleContractStatus { get; set; }
        public int paymentterm_Id { get; set; }
        [ForeignKey("paymentterm_Id")]
        public virtual PaymentTerm paymentTerm { get; set; }
        public int incoterm_Id { get; set; }
        [ForeignKey("incoterm_Id")]
        public virtual Incoterm incoterm { get; set; }
        public bool? PendingForClosing { get; set; }
        public bool? isApproved { get; set; } = true;
        public DateTime? ApprovedDate { get; set; }
        public int? TitleValue1Id { get; set; }
        [ForeignKey("TitleValue1Id")]
        public virtual Incoterm TitleValue1 { get; set; }
        public int? TitleValue2Id { get; set; }
        [ForeignKey("TitleValue2Id")]
        public virtual Incoterm TitleValue2 { get; set; }
        public virtual List<MemorandumSale> MemorandumSales { get; set; }

        public virtual List<ComparativeStatement> comparativeStatements { get; set; }
        public virtual List<BookerStatementItem> BookerStatementItems { get; set; }
        public string uniqueNumber { get; set; }

        public int? transactionHolderId { get; set; }
        [ForeignKey("transactionHolderId")]
        public ERP_BL.Databases.Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual ERP_BL.Procurements.StatusClass.StatusClass StatusClass { get; set; }
        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentModuleContracts")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? InterCompany_Id { get; set; }
        [ForeignKey("InterCompany_Id ")]
        [InverseProperty("InterCompanyModuleContract")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }
        public double contractValue { get; set; }

        public double budgetCost { get; set; }

        public double budgetMargin { get; set; }

    }

    public class ModuleContractStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<ModuleContract> ModuleContracts { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("moduleContractStatuses")]
        public virtual List<ERP_BL.Procurements.StatusClass.StatusClass> moduleContractStatusSubClasses { get; set; }

    }
}
