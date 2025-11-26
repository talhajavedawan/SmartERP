using System.ComponentModel.DataAnnotations;

namespace ERP_BL.Entities
{
    public class VendorCreateDto
    {
        [Required(ErrorMessage = "Vendor name (Company name) is required.")]
        public string VendorName { get; set; } = string.Empty;

        public string? Ntn { get; set; }
        public int? IndustryTypeId { get; set; }
        public int? BusinessTypeId { get; set; }
        public String? RegistrationNumber { get; set; }
        public int? ContactPersonId { get; set; }
        //public string? ContactFirstName { get; set; }
        //public string? ContactLastName { get; set; }

        // Shipping and Billing Address Ids
        public int? ShippingAddressId { get; set; }
        public int? BillingAddressId { get; set; }

        // For Address Details, these can be expanded for more fields like AddressLine1, etc.
        public string? ShippingAddressLine1 { get; set; }
        public string? ShippingAddressLine2 { get; set; }
        public string? ShippingZipcode { get; set; }
        public int? ShippingCountryId { get; set; }
        public int? ShippingStateId { get; set; }
        public int? ShippingCityId { get; set; }
        public int? ShippingZoneId { get; set; }

        public string? BillingAddressLine1 { get; set; }
        public string? BillingAddressLine2 { get; set; }
        public string? BillingZipcode { get; set; }
        public int? BillingCountryId { get; set; }
        public int? BillingStateId { get; set; }
        public int? BillingCityId { get; set; }
        public int? BillingZoneId { get; set; }

        public int? ParentVendorId { get; set; }

        [Range(1, 5, ErrorMessage = "Ranking must be between 1 and 5.")]
        public int Ranking { get; set; } = 3;

        public bool RedList { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool IsSubsidiary { get; set; } = false;
        public int? VendorNatureId { get; set; }
        public int? CurrencyId { get; set; }
        public int ContactId { get; set; }
        public string? ContactEmail { get; set; }    
        public string? ContactPhone { get; set; }   
        public string? ContactWebsite { get; set; }
        public List<int>? ClientCompanyIds { get; set; }     
        public List<int>? DepartmentIds { get; set; }
    }
}
