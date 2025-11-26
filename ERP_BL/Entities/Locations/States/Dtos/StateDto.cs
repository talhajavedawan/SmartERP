namespace ERP_BL.Entities.Locations.States.Dtos
{
    
        public class StateDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int CountryId { get; set; }
            public string? StateCode { get; set; }
            public List<int> CityIds { get; set; } = new();
        }

    }
