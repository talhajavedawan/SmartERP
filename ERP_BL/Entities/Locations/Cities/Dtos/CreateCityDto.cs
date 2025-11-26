using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Locations.Cities.Dtos
{
  
   
        public class CreateCityDto
        {
            [Required]
            [MaxLength(150)]
            public string Name { get; set; }

            [Required]
            public int StateId { get; set; }
        }

    
}
