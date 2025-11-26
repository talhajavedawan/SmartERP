using ERP_BL.Entities.Locations.Cities.Dtos;
using ERP_BL.Entities.Locations.Countries.Dtos;
using ERP_BL.Entities.Locations.States.Dtos;
using ERP_BL.Entities.Locations.Zones.Dtos;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Base.Addresses.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? Zipcode { get; set; }
        public CountryDto? Country { get; set; }
        public StateAddressDto? State { get; set; }
        public CityDto? City { get; set; }
        public ZoneDto? Zone { get; set; }
        public Enums.Enums AddressType { get; set; } = Enums.Enums.Company;
    }
}
