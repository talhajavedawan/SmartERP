using ERP_BL.ChartofAccounts;
using ERP_BL.Enums;
using ERP_BL.Procurements.StatusClass;
using ERP_BL.Tax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Offer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string InquiryReferenceNo { get; set; } // customer side number
        public string SalesReferenceNo { get; set; }  // internal number
        public string offerReferenceNo { get; set; } //our Reference Number
        public string commisionRefrenceNo { get; set; }  // Commission number
        public DateTime? OfferDate { get; set; }
        public DateTime? CreationDate { get; set; }
        //public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public DateTime? InquiryDate { get; set; }
        public DateTime? offerValidityDate { get; set; }
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
        public decimal marginExchangeRate { get; set; } // Commision Exchange Rate Amount
        public decimal? margin { get; set; } // Commision Amount
        public string comments { get; set; }    // Comments
        /*public string deliveryTime { get; set; }*/ // Valid time for Delivery
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
        [InverseProperty("DepartmentOffers")]
        [ForeignKey("dept_Id ")]
        public virtual Department department { get; set; }
        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }
        public int allocation_Id { get; set; }
        [ForeignKey("allocation_Id")]
        public virtual Employee employee { get; set; }
        //public int vendor_Id { get; set; }
        [InverseProperty("Offers")]
        public virtual List<Vendor> vendors { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("CompanyOffers")]
        public virtual Company company { get; set; }
        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentOffers")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? InterCompany_Id { get; set; }
        [ForeignKey("InterCompany_Id ")]
        [InverseProperty("InterCompanyOffers")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }
        public InquiryType offertype { get; set; }
        public int? principal_Id { get; set; }
        [ForeignKey("principal_Id")]
        public virtual Principal principal { get; set; }
        public int? inquiry_Id { get; set; }
        [ForeignKey("inquiry_Id ")]
        public virtual Inquiry inquiry { get; set; }
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
        public virtual OfferStatus offerStatus { get; set; }
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
        public Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
        public int? CostSheet_Id { get; set; }
        [ForeignKey("CostSheet_Id ")]
        public virtual CostSheet CostSheet { get; set; }
        public int? costCenterCurrencyId { get; set; }
        [ForeignKey("costCenterCurrencyId")]
        public virtual Currency CostCenterCurrency { get; set; }
        public double SER { get; set; }
        public double MER { get; set; }
        public double totalOfferAmount { get; set; }
        public double totalOfferAmountSER { get; set; }
        public double totalOfferAmountMER { get; set; }
        public double totalbudgetCost { get; set; }
        public double budgetMarginSER { get; set; }
        public double budgetMarginMER { get; set; }

        
        
    }

    public class OfferStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<Offer> Offers { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("offerStatuses")]
        public virtual List<StatusClass> offerStatusSubClasses { get; set; }

    }
    public class ComparativeStatement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //[Required]
        public int? offerId { get; set; }
        [ForeignKey("offerId")]
        public virtual Offer offer { get; set; }
        public int? VendorId { get; set; }
        [ForeignKey("VendorId")]
        public virtual Vendor vendor { get; set; }
        public double offerValue { get; set; }
        public int? incoTerm_Id { get; set; }
        [ForeignKey("incoTerm_Id")]
        public virtual Incoterm incoTerm { get; set; }

        public int? offerCurrencyId { get; set; }
        [ForeignKey("offerCurrencyId")]
        public virtual Currency offerCurrency { get; set; }
        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual User creator { get; set; }
        public virtual List<ComparativeStatementItem> comparativeStatementItems { get; set; }
        public string vendorName { get; set; }

        public int? vendorIncoTermId { get; set; }
        [ForeignKey("vendorIncoTermId")]
        public virtual Incoterm itemIncoTerm { get; set; }
        public int? productId { get; set; }
        [ForeignKey("productId")]
        public virtual ProcurementProduct product { get; set; }
        public bool isSelect { get; set; }

    }
    public class ComparativeStatementItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }


        public int? costSheetFieldId { get; set; }
        [ForeignKey("costSheetFieldId")]
        public virtual CostSheetField costSheetField { get; set; }
        public int? convertedCurrencyId { get; set; }
        [ForeignKey("convertedCurrencyId")]
        public virtual Currency convertedCurrency { get; set; }
        public int? itemCurrencyId { get; set; }
        [ForeignKey("itemCurrencyId")]
        public virtual Currency itemCurrency { get; set; }
        public int? comparativeStatementId { get; set; }
        [ForeignKey("comparativeStatementId")]
        public virtual ComparativeStatement ComparativeStatement { get; set; }

        public double MER { get; set; }
        public double amountOC { get; set; }
        public double amountMER { get; set; }
        public string ownDescription { get; set; }
    }


    //public class BookerStatement
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }
    //    public string bookerName { get; set; }
    //    public int? offerId { get; set; }
    //    [ForeignKey("offerId")]
    //    public virtual Offer offer { get; set; }
    //    public double bookerValue { get; set; }
    //    public int? creatorId { get; set; }
    //    [ForeignKey("creatorId")]
    //    public virtual User creator { get; set; }
    //    public virtual List<BookerStatementItem> BookerStatementItems { get; set; }
    //}
    public class BookerStatementItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? offerId { get; set; }
        [ForeignKey("offerId")]
        public virtual Offer offer { get; set; }

        public int? ModuleContractId { get; set; }
        [ForeignKey("ModuleContractId")]
        public virtual ERP_BL.Procurements.ModuleContract ModuleContract { get; set; }

        public int product_Id { get; set; }
        [ForeignKey("product_Id ")]
        public virtual Product product { get; set; }

        public double unit { get; set; }
        public double siQuantity { get; set; }
        public double quantity { get; set; }
        public double siWeight { get; set; }
        public double weight { get; set; }
        public double amount { get; set; }
        public double siAmount { get; set; }

        public double amountGST { get; set; }
        public double siAmountGST { get; set; }
        public double passOnValue { get; set; }
        public double siPassOnValue { get; set; }

        public int? taxNameId { get; set; }
        [ForeignKey("taxNameId")]
        public virtual TaxName TaxName { get; set; }
        public int? focSamplingId { get; set; }
        [ForeignKey("focSamplingId")]
        public virtual FOCSampling FOCSampling { get; set; }
        public double focValue { get; set; }
        public double siFocValue { get; set; }

        public int? claimDiscountId { get; set; }
        [ForeignKey("claimDiscountId")]
        public virtual ClaimDiscount ClaimDiscount { get; set; }
        public double claimDiscountValue { get; set; }
        public double siClaimDiscountValue { get; set; }

        public double netAmount { get; set; }
        public double siNetAmount { get; set; }

        public int? saleOrderId { get; set; }
        [ForeignKey("saleOrderId")]
        public virtual SaleOrder saleOrder { get; set; }

        public int? passOnId { get; set; }
        [ForeignKey("passOnId")]
        public virtual PassOn PassOn { get; set; }
        public int? saleInvoiceId { get; set; }
        [ForeignKey("saleInvoiceId")]
        public virtual SaleInvoice saleInvoice { get; set; }
        public int? purchaseOrderId { get; set; }
        [ForeignKey("purchaseOrderId")]
        public virtual PurchaseOrder purchaseOrder { get; set; }

        public int? purchaseInvoiceId { get; set; }
        [ForeignKey("purchaseInvoiceId")]
        public virtual PurchaseInvoice purchaseInvoice { get; set; }
    }
    public class ClaimDiscount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string discountName { get; set; }
        public bool isActive { get; set; }
        public int? chartofAccountId { get; set; }
        [ForeignKey("chartofAccountId")]
        public virtual ChartofAccount ChartofAccount { get; set; }


    }
    public class FOCSampling
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string samplingtName { get; set; }
        public bool isActive { get; set; }
        public int? chartofAccountId { get; set; }
        [ForeignKey("chartofAccountId")]
        public virtual ChartofAccount ChartofAccount { get; set; }


    }
    public class PassOn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        public string passOnName { get; set; }
        public bool isActive { get; set; }
        public int? chartofAccountId { get; set; }
        [ForeignKey("chartofAccountId")]
        public virtual ChartofAccount ChartofAccount { get; set; }

    }
    public class UniqueNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UniqueName { get; set; }
        public bool isActive { get; set; }
        public DateTime From { get; set; }

        public DateTime To { get; set; }

    }

}
