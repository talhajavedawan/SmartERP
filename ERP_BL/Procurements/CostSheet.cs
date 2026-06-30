using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;
using ERP_BL.ChartofAccounts;

namespace ERP_BL.Databases

{
    public class CostSheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required]
        //public int UserId { get; set; }
        //[ForeignKey("UserId")]
        public virtual List<FieldValue> FieldValues{  get; set; }
        public virtual List<CostFieldHistory> CostFieldHistories{ get; set; }
        public virtual List<CostSheetBillField> CostSheetBillFields { get; set; }
        public virtual List<CostSheetPOField> CostSheetPOFields { get; set; }
        public virtual List<CostSheetSOField> CostSheetSOFields { get; set; }
        public virtual List<CostSheetSaleReceiptField> CostSheetSaleReceiptFields { get; set; }
        public virtual List<CostSheetPaymentField> CostSheetPaymentFields { get; set; }
        public virtual List<CostSheetSIField> CostSheetSIFields { get; set; }
        public virtual List<CostSheetOfferField> CostSheetOfferFields { get; set; }


        public DateTime Timestamp { get; set; }
        public string Info { get; set; }
        //public int SaleOrderId { get; set; }
        //[ForeignKey("SaleOrderId")]
        //public virtual SaleOrder SaleOrder { get; set; }
        public int TransactionType { get; set; }
        public int TransactionId { get; set; }
        public decimal TotalBudgetedMargin { get; set; }
        public decimal TotalActualMargin { get; set; }
        public decimal TotalRevisedMargin { get; set; }
        public string paymenttermWithSupplier { get; set; }
        public DateTime? PODeliveryDate { get; set; }

        public string maker { get; set; }
        public string origin { get; set; }
        public int?  SupplierWarrantyId { get; set; }
        [ForeignKey("SupplierWarrantyId ")]
        public virtual Warranty SupplierWarranty { get; set; }
        public string packing { get; set; }

        public int? paymentterm_Id { get; set; }
        [ForeignKey("paymentterm_Id")]
        public virtual PaymentTerm paymentTerm { get; set; }
        public int? incoterm_Id { get; set; }
        [ForeignKey("incoterm_Id")]
        public virtual Incoterm incoterm { get; set; }
    }


    public class CostSheetField
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            [Required]
            public int AddedbyUserId { get; set; }
            [ForeignKey("AddedbyUserId")]
            public virtual User AddedbyUser { get; set; }

            public DateTime Timestamp { get; set; }
            //public decimal value { get; set; }
            public string Title { get; set; }
            public int SortId { get; set; }
            public int Type { get; set; }
            public bool isActive { get; set; }
            //public int? creditCoaId { get; set; }
            //[ForeignKey("creditCoaId")]
            //public virtual ChartofAccount creditAccount { get; set; }
            //public int? debitCoaId { get; set; }
            //[ForeignKey("debitCoaId")]
            //public virtual ChartofAccount debitAccount { get; set; }
            public string Maker { get; set; }
            public string Origin { get; set; }
            public string Packing { get; set; }
 

        public ICollection<JournalTransaction> journalTransactions { get; set; }

    }

    public class FieldValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }

        
        public decimal Value { get; set; }
        public int Type { get; set; }
        public string stringValue { get; set; }
        public DateTime? dateValue { get; set; }

        public bool isdgGood { get; set; } = false;
        public bool isPacking { get; set; } = false;
        public bool drawingRequired { get; set; } = false;
        public bool isExportLicense { get; set; } = false;
        public decimal adjSCost { get; set; } 





    }
    public class CostFieldHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }
        [Required]
        public int CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public virtual CostSheet CostSheet{ get; set; }

        public decimal Value { get; set; }
        public CostFieldType FieldType { get; set; }

        public DateTime timeStamp { get; set; }

    }
    public class CostSheetBillField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }
        [Required]
        public int CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public virtual CostSheet CostSheet { get; set; }

        public int Bill_Id { get; set; }
      

        public decimal Value { get; set; }
        public CostFieldType FieldType { get; set; }

        public DateTime timeStamp { get; set; }
    }
    public class CostSheetPOField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }
        [Required]
        public int CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public virtual CostSheet CostSheet { get; set; }

        public int PO_Id { get; set; }


        public decimal Value { get; set; }
        public CostFieldType FieldType { get; set; }

        public DateTime timeStamp { get; set; }
    }
    public class CostSheetSOField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }
        [Required]
        public int CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public virtual CostSheet CostSheet { get; set; }

        public int SO_Id { get; set; }


        public decimal Value { get; set; }
        public CostFieldType FieldType { get; set; }

        public DateTime timeStamp { get; set; }
    }
    public class PQDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ShippingTerm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CostSheetSaleReceiptField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }
        [Required]
        public int CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public virtual CostSheet CostSheet { get; set; }

        public int Receipt_Id { get; set; }


        public decimal Value { get; set; }
        public CostFieldType FieldType { get; set; }

        public DateTime timeStamp { get; set; }
    }

    public class CostSheetPaymentField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }
        [Required]
        public int CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public virtual CostSheet CostSheet { get; set; }

        public int Payment_Id { get; set; }


        public decimal Value { get; set; }
        public CostFieldType FieldType { get; set; }

        public DateTime timeStamp { get; set; }
    }
    public class CostSheetSIField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }
        [Required]
        public int CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public virtual CostSheet CostSheet { get; set; }

        public int SI_Id { get; set; }


        public decimal Value { get; set; }
        public CostFieldType FieldType { get; set; }

        public DateTime timeStamp { get; set; }
    }
    public class CostSheetOfferField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual CostSheetField Field { get; set; }
        [Required]
        public int CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public virtual CostSheet CostSheet { get; set; }

        public int Offer_Id { get; set; }


        public decimal Value { get; set; }
        public CostFieldType FieldType { get; set; }

        public DateTime timeStamp { get; set; }
    }

}


