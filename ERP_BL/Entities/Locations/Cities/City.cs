using ERP_BL.Entities.Locations.States;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ERP_BL.Entities.Locations.Cities
{
    [Index(nameof(Name), IsUnique = true)]
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        // Foreign Key
        [ForeignKey(nameof(State))]
        public int StateId { get; set; }

        // Navigation Property
        [JsonIgnore]
        public State State { get; set; }
        public bool IsVoid { get; set; }
    }
}
