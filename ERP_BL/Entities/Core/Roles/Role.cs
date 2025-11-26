using ERP_BL.Entities.Core.Permissions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP_BL.Entities.Core.Roles
{
    public class Role : IdentityRole<int>
    {
        //[ValidateNever]
        [MaxLength(500)]
        public string? Description { get; set; }    
        [ForeignKey("ParentRole")]
        public int? ParentRoleId { get; set; }
        public virtual Role? ParentRole { get; set; }
        [MaxLength(50)]
        public string? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;  // Default value
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
        public string? LastModifiedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsVoid { get; set; }
        //[ValidateNever]
        [InverseProperty("Roles")]
        public virtual ICollection<Permission> Permissions { get; set; } = new HashSet<Permission>();
    }
}
