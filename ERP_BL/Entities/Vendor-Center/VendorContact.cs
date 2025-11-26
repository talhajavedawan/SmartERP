using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Base.Persons;
using ERP_BL.Entities.Core.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities
{
    public class VendorContact
    {
        public int Id { get; set; }
        public string? Designation { get; set; }
        public bool IsPrimary { get; set; }
        public int VendorId { get; set; }
        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }
        public int? PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person? Person { get; set; }
        public int? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact? Contact { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }

        public int? LastModifiedById { get; set; }

        [ForeignKey("LastModifiedById")]
        public User? LastModifiedBy { get; set; }
        public bool IsActive    { get; set; }= true;
    }

}
//ven