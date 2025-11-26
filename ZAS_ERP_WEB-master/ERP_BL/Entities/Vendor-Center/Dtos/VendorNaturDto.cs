using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities
{
    public class VendorNatureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ISActive { get; set; } = true;
    }
}
