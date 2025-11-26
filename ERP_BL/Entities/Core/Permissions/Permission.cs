using ERP_BL.Entities.Core.Roles;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Core.Permissions
{
    [Index(nameof(Name), IsUnique = true)]
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [ForeignKey("ParentPermission")]
        public int? ParentPermissionId { get; set; }
        public virtual Permission? ParentPermission { get; set; }

        [MaxLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? LastModified { get; set; }
        [MaxLength(50)]
        public string? LastModifiedBy { get; set; }
        public bool IsActive { get; set; }


        //[validateNever]
        [InverseProperty("Permissions")]
        public virtual ICollection<Role> Roles { get; set; }
        public bool IsVoid { get; set; }
    }
}
