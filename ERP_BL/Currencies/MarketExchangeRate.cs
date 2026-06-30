using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class MarketExchangeRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime effectiveFrom { get; set; }
        public DateTime effectiveTo { get; set; }
        public int? target_currency_Id { get; set; }
        [ForeignKey("target_currency_Id")]
        [InverseProperty("Target_CurrencyMarketExchangeRates")]
        public virtual Currency Target_Currency { get; set; }
        public int? base_currency_Id { get; set; }
        [ForeignKey("base_currency_Id")]
        [InverseProperty("Base_CurrencyMarketExchangeRates")]
        public virtual Currency Base_Currency { get; set; }
        public int? Addedbyuser_Id { get; set; }
        [ForeignKey("Addedbyuser_Id")]
        public virtual  User Addedbyuser { get; set; }
        public int? Editedbyuser_Id { get; set; }
        [ForeignKey("Editedbyuser_Id")]
        public virtual User Editedbyuser { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("marketExchangeRates")]
        public virtual Company company { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool? isApproved { get; set; }
        public decimal exchangerate { get; set; }
        public decimal? maxVariationPercent { get; set; }
    }
}
