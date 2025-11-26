using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Locations.Countries;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ERP_BL.Entities.Locations.Zones
{
    [Index(nameof(Name), IsUnique = true)]
    public class Zone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        [ValidateNever]
        public virtual ICollection<Country> Countries { get; set; }
        public bool IsVoid { get; set; }
        public ICollection<Address> Address { get; set; }
    }
}
