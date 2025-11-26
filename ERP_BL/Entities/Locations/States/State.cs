using ERP_BL.Entities.Locations.Cities;
using ERP_BL.Entities.Locations.Countries;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP_BL.Entities.Locations.States

{
    [Index(nameof(Name), IsUnique = true)]
    public class State
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
  
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
 
        [JsonIgnore]
        public Country Country { get; set; }
        public string? StateCode { get; set; }
 
        public List<City> Cities { get; set; }
        public bool IsVoid { get; set; }
    }
}
