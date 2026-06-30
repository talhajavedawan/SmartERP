using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Databases

{
    public class CommissionSummarySheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Info { get; set; }
        public virtual List<SummaryFieldValue> FieldValues { get; set; }
        //public int SaleOrderId { get; set; }
        //[ForeignKey("SaleOrderId")]
        //public virtual SaleOrder SaleOrder { get; set; }
        public int? OfferCurrencyId { get; set; }
        [ForeignKey("OfferCurrencyId")]
        public virtual Currency OfferCurrency { get; set; }
        public double totalOfferFOB { get; set; }

        public double totalOfferCFR { get; set; }
        public string OfferDeliveryTerm { get; set; }
        public DateTime? OfferDeliveryDate { get; set; }
        public string OfferPacking { get; set; }
        public double OfferCommission { get; set; }
        public double SOCommission { get; set; }

        public int TransactionType { get; set; }
        public int TransactionId { get; set; }
        public string Offerpaymentterm { get; set; }
        public bool? Offertranshipment { get; set; }
        public double netOfferCommission { get; set; }
        public double netSOCommission { get; set; }
    }





    public class SummarySheetField
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

    }

    public class SummaryFieldValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey("FieldId")]
        public virtual SummarySheetField Field { get; set; }


        public decimal Value { get; set; }
        public int Type { get; set; }

    }


}


