using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.CompanyCenter.Departments;
using ERP_BL.Entities.Core.Users;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities
{
    [Index(nameof(CompanyId), IsUnique = true)]
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("OwnedVendors")]
        [Required]
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        [StringLength(50, ErrorMessage = "Registration number cannot exceed 50 characters.")]
        public string? RegistrationNumber { get; set; }

        [StringLength(100, ErrorMessage = "Vendor nature cannot exceed 100 characters.")]
        public int? VendorNatureId { get; set; }
        [ForeignKey("VendorNatureId")]
        public VendorNature VendorNature { get; set; }

        public int? CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        public bool IsSubsidiary { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool RedList { get; set; } = false;

        [Range(1, 5, ErrorMessage = "Ranking must be between 1 and 5.")]
        public int Ranking { get; set; } = 3;

        public int? ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public virtual Address? ShippingAddress { get; set; }

        public int? BillingAddressId { get; set; }
        [ForeignKey("BillingAddressId")]
        public virtual Address? BillingAddress { get; set; }

        //public int? ContactId { get; set; }
        //[ForeignKey("ContactId")]
        //public virtual Contact? Contact { get; set; }

        public int? ParentVendorId { get; set; }
        [ForeignKey("ParentVendorId")]
        public virtual Vendor? ParentVendor { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }

        public int? CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }

        public int? LastModifiedById { get; set; }
        [ForeignKey("LastModifiedById")]
        public User? LastModifiedBy { get; set; }

        [InverseProperty("Vendors")]
        public ICollection<Company> ClientCompanies { get; set; } = new HashSet<Company>();

        [InverseProperty("Vendors")]
        public ICollection<Department> Departments { get; set; } = new HashSet<Department>();
    }
}
