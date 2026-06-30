using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.ChartofAccounts;
using ERP_BL.Procurements.InterBankTransfers;

namespace ERP_BL.Databases
{
    public class ProcurementProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double unitPrice { get; set; }
        public string caption1 { get; set; }
        public double value1 { get; set; }
        public string caption2 { get; set; }
        public double value2 { get; set; }
        public string caption3 { get; set; }
        public double value3 { get; set; }
        public double UnInvoicedQuantity { get; set; }
        public double UnInvoicedWeight { get; set; }
        public double InvoicedQuantity { get; set; }
        public double InvoicedWeight { get; set; }

        public double ReceivedQuantity { get; set; }
        public double ReceivedWeight { get; set; }
        public double DispatchedQuantity { get; set; }
        public double DispatchedWeight { get; set; }
        public string PackingDimensions { get; set; }
        public int? packingStyleId { get; set; }
        [ForeignKey("packingStyleId ")]
        public virtual PackingStyle packingStyle { get; set; }

        public double TotalInvoicedQuantity { get; set; }
        public double TotalInvoicedWeight { get; set; }
        public double totalCommission { get; set; }

        public double UnInvoicedSoAmount { get; set; }
        public double totalInvoicedSoAmount { get; set; }

        public double NowAmount { get; set; }
        public double AmountSOC { get; set; }


        public int product_Id { get; set; }
        [ForeignKey("product_Id ")]
        public virtual InquiryProduct inquiryProduct { get; set; }

        public int? fieldId { get; set; }
        [ForeignKey("fieldId")]
        public virtual CostSheetField costSheetField { get; set; }
        public int? creditAccountId { get; set; }
        [ForeignKey("creditAccountId ")]
        public virtual ChartofAccount creditAccount { get; set; }
        public int? debitAccountId { get; set; }
        [ForeignKey("debitAccountId")]
        public virtual ChartofAccount debitAccount { get; set; }

        public int priority { get; set; }

        [InverseProperty("products")]
        public int? interBankTransferId { get; set; }
        [ForeignKey("interBankTransferId")]
        public virtual InterBankTransfer interBankTransfer { get; set; }
    }


    public class PackingStyle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
