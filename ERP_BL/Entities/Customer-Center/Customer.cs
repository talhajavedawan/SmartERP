using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Base.Persons;
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.Core.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.CompanyCenter.Customers
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // -------------------- COMPANY RELATIONSHIP --------------------
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }

        // -------------------- CONTACT PERSON --------------------
        public int? ContactPersonId { get; set; }
        [ForeignKey("ContactPersonId")]
        public virtual Person? ContactPerson { get; set; }

        // -------------------- CONTACT INFORMATION --------------------
        public int? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact? Contact { get; set; }

        // -------------------- BILLING ADDRESS --------------------
        public int? BillingAddressId { get; set; }
        [ForeignKey("BillingAddressId")]
        public virtual Address? BillingAddress { get; set; }

        // -------------------- SHIPPING ADDRESS --------------------
        public int? ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public virtual Address? ShippingAddress { get; set; }

        // -------------------- STATUS / AUDIT FIELDS --------------------
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastModified { get; set; }

        public int? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public virtual User? CreatedBy { get; set; }

        public int? LastModifiedById { get; set; }

        [ForeignKey("LastModifiedById")]
        public virtual User? LastModifiedBy { get; set; }
    }
}
