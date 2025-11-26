using ERP_BL.Entities.Core.Users;
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
    public class VendorNature
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "Vendor Nature Name can't exceed 150 characters.")]
        public string Name { get; set; }
        public bool ISActive { get; set; }=true;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }

        public int? LastModifiedById { get; set; }

        [ForeignKey("LastModifiedById")]
        public User? LastModifiedBy { get; set; }
    }
}
//