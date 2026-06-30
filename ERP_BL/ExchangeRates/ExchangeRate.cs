using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ExchangeRates
{
    public class ExchangeRate
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double rateJan { get; set; }
        public double rateFeb { get; set; }
        public double rateMar { get; set; }
        public double rateApr { get; set; }
        public double rateMay { get; set; }
        public double rateJun { get; set; }
        public double rateJul { get; set; }
        public double rateAug { get; set; }
        public double rateSep { get; set; }
        public double rateOct { get; set; }
        public double rateNov { get; set; }
        public double rateDec { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        public virtual Company company { get; set; }
    }
}
