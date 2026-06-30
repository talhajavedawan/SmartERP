using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
   public class PurchaseInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //public int assetId { get; set; }
        //[ForeignKey("assetId")]
        //public virtual Asset asset { get; set; }

        public DateTime acquireAt { get; set; }
        //public int currencyId { get; set; }
        //[ForeignKey("currencyId")]
        public virtual Currency currecncy { get; set; }
        public double FA_Amount { get; set; }
        public double PER { get; set; } // Purchase Exchange rate
        public double FA_Amount_PER { get; set; } // (FA_Amout * PER)
    }
}
