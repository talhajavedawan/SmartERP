using System;

namespace ERP_BL.Entities
{
    public class VendorGetDto
    {
        public int Id { get; set; }

        public string? CompanyName { get; set; }
        public string? Ntn { get; set; }
        public string? BusinessTypeName { get; set; }
        public string? IndustryTypeName { get; set; }

        public int? BusinessTypeId { get; set; }
        public int? IndustryTypeId { get; set; }
        public int ContactId { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactWebsiteUrl { get; set; } // 🔸 renamed for clarity
       // public string? ContactPersonName { get; set; }

        public string? ShippingAddress { get; set; }
        public string? ShippingZipcode { get; set; }

        public int? ShippingZoneId { get; set; }
        public int? ShippingCountryId { get; set; }
        public int? ShippingStateId { get; set; }
        public int? ShippingCityId { get; set; }

        public string? BillingAddress { get; set; }
        public string? BillingZipcode { get; set; }

        public int? BillingZoneId { get; set; }
        public int? BillingCountryId { get; set; }
        public int? BillingStateId { get; set; }
        public int? BillingCityId { get; set; }
        public string? RegistrationNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsSubsidiary { get; set; }
        public bool RedList { get; set; }
        public int Ranking { get; set; }
        public int? ParentVendorId { get; set; }
        public string? ParentVendorName { get; set; }

        public string? VendorNature { get; set; }
        public int? VendorNatureId { get; set; }

        public string? Currency { get; set; }
        public int? CurrencyId { get; set; }

        public DateTime CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public string? LastModifiedByName { get; set; }
        public DateTime? LastModified { get; set; }
        public List<int>? ClientCompanyIds { get; set; }
        public List<string>? ClientCompanyNames { get; set; }
        public List<int>? DepartmentIds { get; set; }
        public List<string>? DepartmentNames { get; set; }
    }
}
