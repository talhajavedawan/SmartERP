using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.Locations.Cities;
using ERP_BL.Entities.Locations.Countries;
using ERP_BL.Entities.Locations.States;
using ERP_BL.Entities.Locations.Zones;
using ERP_BL.Enums;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Base.Addresses
{

    public class Address
    {

     [Key]
     [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
     public int Id { get; set; }
     [StringLength(100)]
     public string? AddressLine1 { get; set; }
     [StringLength(100)]
     public string? AddressLine2 { get; set; }

        [StringLength(20)]
        public string? Zipcode { get; set; }

    [ForeignKey("CountryId")]
    public int? CountryId { get; set; }
    public Country? Country { get; set; }


     [ForeignKey("StateId")]
    public int? StateId { get; set; }
    public State? State { get; set; }


     [ForeignKey("CityId")]

        public int? CityId { get; set; }
        public City? City { get; set; }

     [ForeignKey("ZoneId")]
    public int? ZoneId { get; set; }
    public Zone? Zone { get; set; }

     public Enums.Enums AddressType { get; set; }
     
    }
}
