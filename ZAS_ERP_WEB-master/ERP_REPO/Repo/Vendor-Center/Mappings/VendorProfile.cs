using AutoMapper;
using ERP_BL.Entities;
using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Enums;
using System;
using System.Linq;

namespace ERP_REPO.Repo
{
    public class VendorProfile : Profile
    {
        public VendorProfile()
        {
            // =======================
            //   ENTITY → GET DTO
            // =======================
            CreateMap<Vendor, VendorGetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.LastModified, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.CompanyName))
                .ForMember(dest => dest.Ntn, opt => opt.MapFrom(src => src.Company.Ntn))
                .ForMember(dest => dest.BusinessTypeName, opt => opt.MapFrom(src => src.Company.BusinessType != null ? src.Company.BusinessType.BusinessTypeName : null))
                .ForMember(dest => dest.IndustryTypeName, opt => opt.MapFrom(src => src.Company.IndustryType != null ? src.Company.IndustryType.IndustryTypeName : null))
                .ForMember(dest => dest.BusinessTypeId, opt => opt.MapFrom(src => src.Company.BusinessTypeId))
                .ForMember(dest => dest.IndustryTypeId, opt => opt.MapFrom(src => src.Company.IndustryTypeId))

                // ✅ Company.Contact info
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.Company.Contact != null ? src.Company.Contact.Email : null))
                .ForMember(dest => dest.ContactPhone, opt => opt.MapFrom(src => src.Company.Contact != null ? src.Company.Contact.PhoneNumber : null))
                .ForMember(dest => dest.ContactWebsiteUrl, opt => opt.MapFrom(src => src.Company.Contact != null ? src.Company.Contact.WebsiteUrl : null))

                // ✅ Registration Number
                .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationNumber))

                // ✅ Billing Address
                .ForMember(dest => dest.BillingAddress, opt => opt.MapFrom(src =>
                    src.BillingAddress != null
                        ? (src.BillingAddress.AddressLine1 + ", " +
                           (src.BillingAddress.AddressLine2 ?? "") + ", " +
                           (src.BillingAddress.City != null ? src.BillingAddress.City.Name : "")).Trim(',', ' ')
                        : string.Empty))
                .ForMember(dest => dest.BillingZipcode, opt => opt.MapFrom(src => src.BillingAddress != null ? src.BillingAddress.Zipcode : null))

                // ✅ Shipping Address
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src =>
                    src.ShippingAddress != null
                        ? (src.ShippingAddress.AddressLine1 + ", " +
                           (src.ShippingAddress.AddressLine2 ?? "") + ", " +
                           (src.ShippingAddress.City != null ? src.ShippingAddress.City.Name : "")).Trim(',', ' ')
                        : string.Empty))
                .ForMember(dest => dest.ShippingZipcode, opt => opt.MapFrom(src => src.ShippingAddress != null ? src.ShippingAddress.Zipcode : null))

                .ForMember(dest => dest.ShippingZoneId, opt => opt.MapFrom(src => src.ShippingAddress != null ? src.ShippingAddress.ZoneId : null))
                .ForMember(dest => dest.ShippingCountryId, opt => opt.MapFrom(src => src.ShippingAddress != null ? src.ShippingAddress.CountryId : null))
                .ForMember(dest => dest.ShippingStateId, opt => opt.MapFrom(src => src.ShippingAddress != null ? src.ShippingAddress.StateId : null))
                .ForMember(dest => dest.ShippingCityId, opt => opt.MapFrom(src => src.ShippingAddress != null ? src.ShippingAddress.CityId : null))
                .ForMember(dest => dest.BillingZoneId, opt => opt.MapFrom(src => src.BillingAddress != null ? src.BillingAddress.ZoneId : null))
                .ForMember(dest => dest.BillingCountryId, opt => opt.MapFrom(src => src.BillingAddress != null ? src.BillingAddress.CountryId : null))
                .ForMember(dest => dest.BillingStateId, opt => opt.MapFrom(src => src.BillingAddress != null ? src.BillingAddress.StateId : null))
                .ForMember(dest => dest.BillingCityId, opt => opt.MapFrom(src => src.BillingAddress != null ? src.BillingAddress.CityId : null))

                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.UserName : null))
                .ForMember(dest => dest.LastModifiedByName, opt => opt.MapFrom(src => src.LastModifiedBy != null ? src.LastModifiedBy.UserName : null))
                .ForMember(dest => dest.RedList, opt => opt.MapFrom(src => src.RedList))
                .ForMember(dest => dest.Ranking, opt => opt.MapFrom(src => src.Ranking))
                .ForMember(dest => dest.ParentVendorId, opt => opt.MapFrom(src => src.ParentVendorId))
                .ForMember(dest => dest.ParentVendorName, opt => opt.MapFrom(src => src.ParentVendor != null ? src.ParentVendor.Company.CompanyName : null))
                .ForMember(dest => dest.IsSubsidiary, opt => opt.MapFrom(src => src.IsSubsidiary))
                .ForMember(dest => dest.VendorNature, opt => opt.MapFrom(src => src.VendorNature != null ? src.VendorNature.Name : null))
                .ForMember(dest => dest.VendorNatureId, opt => opt.MapFrom(src => src.VendorNatureId))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency != null ? src.Currency.Abbreviation : null))
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.CurrencyId))
                .ForMember(dest => dest.ClientCompanyIds, opt => opt.MapFrom(src => src.ClientCompanies.Select(c => c.Id)))
                .ForMember(dest => dest.ClientCompanyNames, opt => opt.MapFrom(src => src.ClientCompanies.Select(c => c.CompanyName)))
                .ForMember(dest => dest.DepartmentIds, opt => opt.MapFrom(src => src.Departments.Select(d => d.Id)))
                .ForMember(dest => dest.DepartmentNames, opt => opt.MapFrom(src => src.Departments.Select(d => d.DeptName)));

            // =======================
            //   CREATE DTO → ENTITY
            // =======================
            CreateMap<VendorCreateDto, Vendor>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsSubsidiary, opt => opt.MapFrom(src => src.IsSubsidiary))
                .ForMember(dest => dest.RedList, opt => opt.MapFrom(src => src.RedList))
                .ForMember(dest => dest.Ranking, opt => opt.MapFrom(src => src.Ranking))
                .ForMember(dest => dest.ParentVendorId, opt => opt.MapFrom(src => src.ParentVendorId))
                .ForMember(dest => dest.VendorNatureId, opt => opt.MapFrom(src => src.VendorNatureId))
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.CurrencyId))
                .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationNumber))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => new Company
                {
                    CompanyName = src.VendorName != null ? src.VendorName.Trim() : string.Empty,
                    Ntn = src.Ntn != null ? src.Ntn.Trim() : null,
                    BusinessTypeId = src.BusinessTypeId,
                    IndustryTypeId = src.IndustryTypeId,
                    CreationDate = DateTime.UtcNow,
                    CompanyType = CompanyType.VendorCompany,
                    ContactId = src.ContactId,
                    Contact = new Contact
                    {
                        Email = src.ContactEmail != null ? src.ContactEmail.Trim() : null,
                        PhoneNumber = src.ContactPhone != null ? src.ContactPhone.Trim() : null,
                        WebsiteUrl = src.ContactWebsite != null ? src.ContactWebsite.Trim() : null
                    }
                }))
                .ForMember(dest => dest.BillingAddress, opt => opt.MapFrom(src => new Address
                {
                    AddressLine1 = src.BillingAddressLine1 != null ? src.BillingAddressLine1.Trim() : null,
                    AddressLine2 = src.BillingAddressLine2 != null ? src.BillingAddressLine2.Trim() : null,
                    Zipcode = src.BillingZipcode != null ? src.BillingZipcode.Trim() : null,
                    AddressType = Enums.Billing,
                    CountryId = src.BillingCountryId,
                    StateId = src.BillingStateId,
                    CityId = src.BillingCityId,
                    ZoneId = src.BillingZoneId
                }))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => new Address
                {
                    AddressLine1 = src.ShippingAddressLine1 != null ? src.ShippingAddressLine1.Trim() : null,
                    AddressLine2 = src.ShippingAddressLine2 != null ? src.ShippingAddressLine2.Trim() : null,
                    Zipcode = src.ShippingZipcode != null ? src.ShippingZipcode.Trim() : null,
                    AddressType = Enums.Shipping,
                    CountryId = src.ShippingCountryId,
                    StateId = src.ShippingStateId,
                    CityId = src.ShippingCityId,
                    ZoneId = src.ShippingZoneId
                }))
                .ForMember(dest => dest.ClientCompanies, opt => opt.Ignore())
                .ForMember(dest => dest.Departments, opt => opt.Ignore());

            // =======================
            //   UPDATE DTO → ENTITY
            // =======================
            CreateMap<VendorUpdateDto, Vendor>()
                .ForMember(dest => dest.Company, opt => opt.Ignore())
                .ForMember(dest => dest.BillingAddress, opt => opt.Ignore())
                .ForMember(dest => dest.ShippingAddress, opt => opt.Ignore())
                .ForMember(dest => dest.Ranking, opt => opt.MapFrom(src => src.Ranking))
                .ForMember(dest => dest.RedList, opt => opt.MapFrom(src => src.RedList))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.IsSubsidiary, opt => opt.MapFrom(src => src.IsSubsidiary))
                .ForMember(dest => dest.ParentVendorId, opt => opt.MapFrom(src => src.ParentVendorId))
                .ForMember(dest => dest.VendorNatureId, opt => opt.MapFrom(src => src.VendorNatureId))
                .ForMember(dest => dest.CurrencyId, opt => opt.MapFrom(src => src.CurrencyId))
                .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationNumber))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForPath(dest => dest.Company.CompanyName, opt => opt.MapFrom(src => src.VendorName != null ? src.VendorName.Trim() : null))
                .ForPath(dest => dest.Company.Ntn, opt => opt.MapFrom(src => src.Ntn != null ? src.Ntn.Trim() : null))
                .ForPath(dest => dest.Company.IndustryTypeId, opt => opt.MapFrom(src => src.IndustryTypeId))
                .ForPath(dest => dest.Company.BusinessTypeId, opt => opt.MapFrom(src => src.BusinessTypeId))
                .ForPath(dest => dest.Company.LastModified, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForPath(dest => dest.Company.Contact.Email, opt => opt.MapFrom(src => src.ContactEmail != null ? src.ContactEmail.Trim() : null))
                .ForPath(dest => dest.Company.Contact.PhoneNumber, opt => opt.MapFrom(src => src.ContactPhone != null ? src.ContactPhone.Trim() : null))
                .ForPath(dest => dest.Company.Contact.WebsiteUrl, opt => opt.MapFrom(src => src.ContactWebsite != null ? src.ContactWebsite.Trim() : null))
                .ForPath(dest => dest.BillingAddress.AddressLine1, opt => opt.MapFrom(src => src.BillingAddressLine1 != null ? src.BillingAddressLine1.Trim() : null))
                .ForPath(dest => dest.BillingAddress.AddressLine2, opt => opt.MapFrom(src => src.BillingAddressLine2 != null ? src.BillingAddressLine2.Trim() : null))
                .ForPath(dest => dest.BillingAddress.Zipcode, opt => opt.MapFrom(src => src.BillingZipcode != null ? src.BillingZipcode.Trim() : null))
                .ForPath(dest => dest.BillingAddress.CountryId, opt => opt.MapFrom(src => src.BillingCountryId))
                .ForPath(dest => dest.BillingAddress.StateId, opt => opt.MapFrom(src => src.BillingStateId))
                .ForPath(dest => dest.BillingAddress.CityId, opt => opt.MapFrom(src => src.BillingCityId))
                .ForPath(dest => dest.BillingAddress.ZoneId, opt => opt.MapFrom(src => src.BillingZoneId))
                .ForPath(dest => dest.ShippingAddress.AddressLine1, opt => opt.MapFrom(src => src.ShippingAddressLine1 != null ? src.ShippingAddressLine1.Trim() : null))
                .ForPath(dest => dest.ShippingAddress.AddressLine2, opt => opt.MapFrom(src => src.ShippingAddressLine2 != null ? src.ShippingAddressLine2.Trim() : null))
                .ForPath(dest => dest.ShippingAddress.Zipcode, opt => opt.MapFrom(src => src.ShippingZipcode != null ? src.ShippingZipcode.Trim() : null))
                .ForPath(dest => dest.ShippingAddress.CountryId, opt => opt.MapFrom(src => src.ShippingCountryId))
                .ForPath(dest => dest.ShippingAddress.StateId, opt => opt.MapFrom(src => src.ShippingStateId))
                .ForPath(dest => dest.ShippingAddress.CityId, opt => opt.MapFrom(src => src.ShippingCityId))
                .ForPath(dest => dest.ShippingAddress.ZoneId, opt => opt.MapFrom(src => src.ShippingZoneId));
        }
    }
}//