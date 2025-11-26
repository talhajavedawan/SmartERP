using ERP_BL.Entities.Locations.Countries;
using ERP_BL.Entities.Locations.Countries.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Currency
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Vendor Nature Name can't exceed 150 characters.")]
        public string Name { get; set; }   
        public string Symbol { get; set; }  
        public string Abbreviation { get; set; } 
        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Country  Country { get; set; }
    }
}