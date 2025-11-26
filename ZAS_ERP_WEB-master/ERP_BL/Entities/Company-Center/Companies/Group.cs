
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.Core.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP_BL.Entities.Company_Center.Companies
{
    [Index(nameof(GroupName), IsUnique = true)]
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Group name is required.")]
        [StringLength(100, ErrorMessage = "Group name cannot exceed 100 characters.")]
        public string GroupName { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
     

     
        public DateTime? LastModified { get; set; } = DateTime.UtcNow;
        public int? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public virtual User? CreatedBy { get; set; }
        public int? LastModifiedById { get; set; }
        [ForeignKey("LastModifiedById")]
        public virtual User? LastModifiedBy { get; set; }

        public bool IsActive { get; set; }
  

        public virtual ICollection<Company> Companies { get; set; } = new HashSet<Company>();
    }
}
