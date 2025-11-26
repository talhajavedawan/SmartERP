namespace ERP_BL.Entities.Customer_Center
{
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? BusinessTypeName { get; set; }
        public string? IndustryTypeName { get; set; }

        // Contact Info
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }

        // Person
        public string? ContactPersonName { get; set; }

        // Addresses
        public string? BillingAddress { get; set; }
        public string? BillingZipcode { get; set; }
        public string? ShippingAddress { get; set; }
        public string? ShippingZipcode { get; set; }

        // Audit & Status
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public string? LastModifiedByName { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
