using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Databases
{
    public class PaymentTerm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string term { get; set; }
        public int daysofMonthDue { get; set; }
        public float discountPercent { get; set; }
        public int discountDays { get; set; }
        public int discountonDayofMonth { get; set; }
        public int minimumDaytoPay { get; set; }
        public int netDueDays { get; set; }
        public string type { get; set; }
        public bool isApproved { get; set; }

        public bool isActive { get; set; } = true;

        public int user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }

        public DateTime addedDate { get; set; }
        [InverseProperty("SoPaymentTerm")]

        public virtual List<PurchaseOrder> SOPaymentTermforPO { get; set; }
        [InverseProperty("POPaymentTerm")]
        public virtual List<PurchaseOrder> POPaymentTerm { get; set; }

        public bool isSaleOrderType { get; set; }
        public bool isPurchaseOrderType { get; set; }
        public bool isSaleInvoiceType { get; set; }
        public bool isPurchaseInvoiceType { get; set; }
        public bool isPaymentType { get; set; }
        public bool isVnedorBillType { get; set; }
        public bool isOfferType { get; set; }
        public bool isCostSheetType { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual PaymentTerm parentTerm { get; set; }
    }
}
