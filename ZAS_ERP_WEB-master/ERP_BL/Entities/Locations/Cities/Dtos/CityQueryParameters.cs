using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Locations.Cities.Dtos
{
    public class CityQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20; // Limit to avoid heavy load
        public string? Search { get; set; } // Optional filter
    }
}
