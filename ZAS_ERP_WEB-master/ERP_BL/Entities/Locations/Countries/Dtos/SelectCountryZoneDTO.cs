using ERP_BL.Entities.Locations.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Locations.Countries.Dtos
{
    public class SelectCountryZoneDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iso2 { get; set; }
        public string Iso3 { get; set; }
        public List<Zone> Zones { get; set; }
    }
}
