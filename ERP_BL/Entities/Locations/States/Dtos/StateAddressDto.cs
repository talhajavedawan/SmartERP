using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Locations.States.Dtos
{
    public class StateAddressDto
    {
       
            public int Id { get; set; }
            public string Name { get; set; }
            public string? StateCode { get; set; }
            public int CountryId { get; set; }
        
    }
}
