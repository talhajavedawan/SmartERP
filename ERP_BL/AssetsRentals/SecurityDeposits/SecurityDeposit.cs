using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.AssetsRentals.SecurityDeposits
{
    public class SecurityDeposit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? mainBankId { get; set; }
        [ForeignKey("mainBankId")]
        public MainBank mainBank { get; set; }

        public int? bankId { get; set; }
        [ForeignKey("bankId")]
        public Bank bank { get; set; }

        public int? accountId { get; set; }
        [ForeignKey("accountId")]
        public Account account { get; set; }

        public double Amount { get; set; }

        public int? collectionMethodId { get; set; }
        [ForeignKey("collectionMethodId")]
        public CollectionMethod collectionMethod { get; set; }

        public string RefNo { get; set; }
    }

}
