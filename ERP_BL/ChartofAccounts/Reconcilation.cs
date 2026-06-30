using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChartofAccounts
{
   public class Reconcilation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? chartofAccountId { get; set; }
        [ForeignKey("chartofAccountId")]
        public virtual ChartofAccount chartofAccount { get; set; }
        public DateTime reconcilationDate { get; set; }
        public double reconcilationAmount { get; set; }
    }
}
