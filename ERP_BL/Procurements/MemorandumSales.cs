using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ERP_BL.Databases
{
    public class MemorandumSale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string referenceNo { get; set; } // customer side number
        public string CustomerReferenceNo { get; set; }  // internal number
        public string PrincipleReferenceNo { get; set; }  // internal number
        public DateTime? CustomerReferenceDate { get; set; }
        public DateTime? PrincipleReferenceDate { get; set; }

        public DateTime? ExpectedClosingDate { get; set; }
        public DateTime? memorandumSalesDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool ApplySaleRegister { get; set; }

        public float exchangeRate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }

        public string OwnDescription { get; set; }

        public bool isVoid { get; set; } = false;

        public string comments { get; set; }
        //public string deliveryTime { get; set; } // Valid time for Delivery
        public double totalFOBValue { get; set; }

        public double totalCFRValue { get; set; }
        //public double totalBaseFOBValue { get; set; }
        //public double totalBaseCFRValue { get; set; }
        //public double SoAmountSER { get; set; }


        public int customerCompany_Id { get; set; }
        [ForeignKey("customerCompany_Id ")]
        public virtual CustomerCompany customerCompany { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public string stage { get; set; }
        public int dept_Id { get; set; }
        [ForeignKey("dept_Id ")]
        [InverseProperty("DepartmentMemorandumSales")]
        public virtual Department department { get; set; }

        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }

        public int principal_Id { get; set; }
        [ForeignKey("principal_Id")]
        public virtual Principal principal { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("CompanyMemorandumSales")]
        public virtual Company company { get; set; }
        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentMemorandumSales")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? InterCompany_Id { get; set; }
        [ForeignKey("InterCompany_Id ")]
        [InverseProperty("InterCompanyMemorandumSales")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }



        public int? SaleOrder_Id { get; set; }
        [ForeignKey("SaleOrder_Id ")]
        [InverseProperty("MemorandumSales")]
        public virtual SaleOrder SaleOrder { get; set; }
        public int? Offer_Id { get; set; }
        [ForeignKey("Offer_Id")]
        public Offer Offer { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? TotalQuantity { get; set; }
        //public int currency_Id { get; set; }
        //[ForeignKey("currency_Id")]
        //public virtual Currency currency { get; set; }
        public bool isPercentTax { get; set; }
        public double salesTax { get; set; }
        public virtual ICollection<ProcurementProduct> products { get; set; }
        public virtual MemorandumSaleStatus memorandumSaleStatus { get; set; }
        public bool? PendingForClosing { get; set; }
        public bool? isApproved { get; set; } = true;
        public DateTime? ApprovedDate { get; set; }

        public int? TitleValue1Id { get; set; }
        [ForeignKey("TitleValue1Id")]
        public virtual Incoterm TitleValue1 { get; set; }
        public int? TitleValue2Id { get; set; }
        [ForeignKey("TitleValue2Id")]
        public virtual Incoterm TitleValue2 { get; set; }


    }

    public class MemorandumSaleStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<MemorandumSale> MemorandumSales { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
    }

}
