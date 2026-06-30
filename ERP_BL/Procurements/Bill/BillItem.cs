using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERP_BL.Procurements.Bill
{
   public class BillItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double AmountSOC { get; set; }
        public int? fieldId { get; set; }
        [ForeignKey("fieldId")]
        public virtual CostSheetField costSheetField { get; set; }
        public int? creditAccountId { get; set; }
        [ForeignKey("creditAccountId ")]
        public virtual ChartofAccount creditAccount { get; set; }
        public int? debitAccountId { get; set; }
        [ForeignKey("debitAccountId")]
        public virtual ChartofAccount debitAccount { get; set; }
        public double BillAmount { get; set; }
        //public int? bill_Id { get; set; }
        //[ForeignKey("bill_Id")]
        //public virtual ERP_BL.Databases.Bill bill { get; set; }
    }
}
