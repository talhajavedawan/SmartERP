using System;
namespace ERP_BL.Entities
{
    public class VendorContactCreateDto
    {
        public int VendorId { get; set; }
        public string Designation { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int? PersonId { get; set; }
        public int? ContactId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Nationality { get; set; }
        public string? Religion { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? WebsiteUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public int? CreatedById { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
