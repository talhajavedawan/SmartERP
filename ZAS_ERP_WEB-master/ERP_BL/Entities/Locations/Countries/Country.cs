using ERP_BL.Entities.Locations.States;
using ERP_BL.Entities.Locations.Zones;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP_BL.Entities.Locations.Countries
{
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Country name is required")]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(3)]
        public string Iso3 { get; set; }

        [MaxLength(2)]
        public string Iso2 { get; set; }
        public int? PhoneCode { get; set; }
        //public string Currency { get; set; }

        // Navigation property for related regions
        public List<State> States { get; set; }

        public int? ZoneId { get; set; }

        [ForeignKey("ZoneId")]
        [JsonIgnore]
        public virtual Zone? Zone { get; set; }
        public bool IsVoid { get; set; }
        public Currency Currency { get; set; }
    }

}
