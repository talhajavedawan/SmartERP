using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Databases
{
    public class SalesExchangeRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AddedOn { get; set; }
        private int _targetYear { get; set; }
        public int targetYear { 
            get { return _targetYear; } 
            set 
            {
                _targetYear = value;
                if(_targetYear!=0)
                { 
                    effectiveFrom = new DateTime(_targetYear, 1, 1) ;
                    effectiveTo = new DateTime(_targetYear, 12, 31,23,59,59);
                }
            } 
        }
        public DateTime effectiveFrom { get; set; }
        public DateTime effectiveTo { get; set; }
        public int? target_currency_Id { get; set; }
        [ForeignKey("target_currency_Id")]
        [InverseProperty("Target_CurrencysalesExchangeRates")]
        public virtual Currency Target_Currency { get; set; }
        public int? base_currency_Id { get; set; }
        [ForeignKey("base_currency_Id")]
        [InverseProperty("Base_CurrencysalesExchangeRates")]
        public virtual Currency Base_Currency { get; set; }
        public int? Addedbyuser_Id { get; set; }
        [ForeignKey("Addedbyuser_Id")]
        public virtual User Addedbyuser { get; set; }
        public int? Editedbyuser_Id { get; set; }
        [ForeignKey("Editedbyuser_Id")]
        public virtual User Editedbyuser { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("salesExchangeRates")]
        public virtual Company company { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool? isApproved { get; set; }
        public decimal exchangerate { get; set; }

    }
}
