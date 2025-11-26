using ERP_BL.Entities.Core.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.CompanyCenter.Companies.List
{
    [Index(nameof(BusinessTypeName), IsUnique = true)]
    public class BusinessType
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string BusinessTypeName { get; set; }
        public bool IsActive { get; set; }
        // -------------------- Audit Fields --------------------
        [ForeignKey(nameof(CreatedByUser))]
        public int? CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(LastModifiedByUser))]
        public int? LastModifiedByUserId { get; set; }
        public User? LastModifiedByUser { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
