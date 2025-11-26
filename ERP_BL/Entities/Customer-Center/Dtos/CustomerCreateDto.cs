using System.ComponentModel.DataAnnotations;

namespace ERP_BL.Entities.Customer_Center.Dtos
{
    public class CustomerCreateDto
    {
        // -------------------- COMPANY --------------------
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "BusinessTypeId must be a valid ID.")]
        public int BusinessTypeId { get; set; }
        public string? BusinessTypeName { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "IndustryTypeId must be a valid ID.")]
        public int IndustryTypeId { get; set; }
        public string? IndustryTypeName { get; set; }

        // -------------------- CONTACT PERSON --------------------
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        // -------------------- CONTACT --------------------
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        // -------------------- BILLING ADDRESS --------------------
        [Required]
        [StringLength(100)]
        public string BillingAddressLine1 { get; set; } = string.Empty;

        [StringLength(100)]
        public string? BillingAddressLine2 { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BillingCityId must be a valid ID.")]
        public int? BillingCityId { get; set; }

        public string? BillingCityName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BillingStateId must be a valid ID.")]
        public int? BillingStateId { get; set; }

        public string? BillingStateName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BillingCountryId must be a valid ID.")]
        public int? BillingCountryId { get; set; }

        public string? BillingCountryName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BillingZoneId must be a valid ID.")]
        public int? BillingZoneId { get; set; }

        public string? BillingZoneName { get; set; }

        [Required]
        [StringLength(20)]
        public string BillingZipCode { get; set; } = string.Empty;

        // -------------------- SHIPPING ADDRESS --------------------
        [StringLength(100)]
        public string? ShippingAddressLine1 { get; set; } // Made optional

        [StringLength(100)]
        public string? ShippingAddressLine2 { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ShippingCityId must be a valid ID.")]
        public int? ShippingCityId { get; set; }

        public string? ShippingCityName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ShippingStateId must be a valid ID.")]
        public int? ShippingStateId { get; set; }

        public string? ShippingStateName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ShippingCountryId must be a valid ID.")]
        public int? ShippingCountryId { get; set; }

        public string? ShippingCountryName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ShippingZoneId must be a valid ID.")]
        public int? ShippingZoneId { get; set; }

        public string? ShippingZoneName { get; set; }

        [StringLength(20)]
        public string? ShippingZipCode { get; set; } // Made optional

        // -------------------- GENERAL --------------------
        public bool IsActive { get; set; } = true;
        // -------------------- ADD THESE AUDIT/METADATA FIELDS --------------------
        public DateTime? CreatedDate { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedByName { get; set; }
    }
}