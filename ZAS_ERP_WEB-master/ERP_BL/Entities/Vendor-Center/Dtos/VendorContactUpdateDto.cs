using System;
namespace ERP_BL.Entities
{
    public class VendorContactUpdateDto
    {
        public int Id { get; set; }
        public string? Designation { get; set; }
        public bool IsPrimary { get; set; }
        public int VendorId { get; set; }
        public int? PersonId { get; set; }
        public int? ContactId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? WebsiteUrl { get; set; }
        public int? LastModifiedById { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}





