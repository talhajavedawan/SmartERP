using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Revaluation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int assetId { get; set; }
        [ForeignKey("assetId")]
        public virtual Asset asset { get; set; }
    
        public DateTime revaluationDate { get; set; }
	    public double amountMR { get; set; }
	    public double MER { get; set; }
	    public double amountMER { get; set; }
	    public int transGroupID { get; set; }

    }
}