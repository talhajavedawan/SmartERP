using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Locations.Zones.Dtos
{
    public class CreateZoneDTO
    {
        public string Name { get; set; }
        public List<int> CountryIds { get; set; }
    }
}
