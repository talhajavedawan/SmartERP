using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities
{
    public class CurrencyGetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Abbreviation { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
    }

}
