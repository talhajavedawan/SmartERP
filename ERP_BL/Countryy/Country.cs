using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Countryy
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CountryName { get; set; }
        public string Abbriviation { get; set; }
        public string CountryCode { get; set; }

        public int  ResidentDays { get; set; }
    }

    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CityName { get; set; }
        public string Abbriviation { get; set; }
        public string PostalCode { get; set; }

        public int countryId { get; set; }
        [ForeignKey("countryId")]
        public virtual Country country { get; set; }
    }
}
