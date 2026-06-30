using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ExchangeRates
{
   public  class ExchangeRateGroup
    {
        [Key]
        public int Id { get; set; }
        public ExchangeRateType ExchangeType { get; set; }
        public int TargetYear { get; set; }
        public DateTime AddedOn { get; set; }
        public int? transaction_currency_Id { get; set; }
        [ForeignKey("transaction_currency_Id")]
        public virtual Currency Transaction_Currency { get; set; }
        public int? base_currency_Id { get; set; }
        [ForeignKey("base_currency_Id")]
        public virtual Currency Base_Currency { get; set; }
        public int? Addedbyuser_Id { get; set; }
        [ForeignKey("Addedbyuser_Id")]
        public virtual ERP_BL.Databases.User Addedbyuser { get; set; }
        public int? Editedbyuser_Id { get; set; }
        [ForeignKey("Editedbyuser_Id")]
        public virtual ERP_BL.Databases.User Editedbyuser { get; set; }
        public DateTime? LastUpdated { get; set; }
        public virtual List<ExchangeRate> exchangeRates { get; set; }
        public bool isVoid { get; set; }


    }
}
