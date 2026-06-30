using ERP_BL.ChartofAccounts;
using ERP_BL.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Tax
{
    public class TaxType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TypeName { get; set; }
        public bool isActive { get; set; }
    }

    public class TaxName
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? taxTypeId { get; set; }
        [ForeignKey("taxTypeId")]
        public virtual TaxType taxType { get; set; }

        public double percentage { get; set; }

        public int? COA_Id { get; set; }
        [ForeignKey("COA_Id")]
        public virtual ChartofAccount chartofAccount { get; set; }

        public bool isAdjusted { get; set; }
        public bool isManual { get; set; }
    }
}
