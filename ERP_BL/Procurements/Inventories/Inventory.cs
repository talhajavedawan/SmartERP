using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.Inventories
{
   public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? creationDate { get; set; }
        public int? prodId { get; set; }
        [ForeignKey("prodId")]
        public virtual Product Product { get; set; }
        public double Quantity { get; set; }
        public double Weight { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double UnitRate { get; set; }
        public double MER { get; set; }

        public string transactionRefno { get; set; }
        public InventoryTransactionsType TransactionsType { get; set; }
        public TransactionItemType moduleType { get; set; }
        public double Balance { get; set; }
        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency Currency { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User creator { get; set; }
        public int? companyId { get; set; }
        [ForeignKey("companyId ")]
        public virtual Company company { get; set; }
        public int? deptId { get; set; }
        [ForeignKey("deptId ")]
        public virtual Department department { get; set; }
        public int? PurchaseInvoiceId { get; set; }
        [ForeignKey("PurchaseInvoiceId")]
        public virtual PurchaseInvoice PurchaseInvoice { get; set; }
        public int? SaleInviceId { get; set; }
        [ForeignKey("SaleInviceId")]
        public virtual SaleInvoice SaleInvoice { get; set; }
        public double AverageCost { get; set; }
        public double AmountOC { get; set; }
        public double AmountMER { get; set; }
        public int? bookerItemId { get; set; }
        [ForeignKey("bookerItemId")]
        public virtual BookerStatementItem BookerStatementItem { get; set; }

        public bool isAdjusted { get; set; }

        public int? adjustment_Id { get; set; }
        [ForeignKey("adjustment_Id")]
        public virtual InventoryAdjustment InventoryAdjustment { get; set; }


    }
}
