using AutoMapper;
using ERP_BL.Entities;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Base.Persons;

namespace ERP_REPO.Repo
{//
    public class VendorContactProfile : Profile
    {
        public VendorContactProfile()
        {
            // ✅ GET mapping
            CreateMap<VendorContact, VendorContactGetDto>()
                // Core Info
                .ForMember(dest => dest.VendorName, opt => opt.MapFrom(src => src.Vendor.Company.CompanyName))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src.Designation))
                .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive)) // ✅ Added
                                                                                           // Person Info
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person != null ? src.Person.FirstName : null))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person != null ? src.Person.LastName : null))
                .ForMember(dest => dest.Nationality, opt => opt.MapFrom(src => src.Person != null ? src.Person.Nationality : null))
                .ForMember(dest => dest.Religion, opt => opt.MapFrom(src => src.Person != null ? src.Person.Religion : null))
                // Contact Info
                .ForMember(dest => dest.ContactId, opt => opt.MapFrom(src => src.ContactId))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Contact != null ? src.Contact.PhoneNumber : null))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Contact != null ? src.Contact.Email : null))
                .ForMember(dest => dest.WebsiteUrl, opt => opt.MapFrom(src => src.Contact != null ? src.Contact.WebsiteUrl : null))
                // Audit Info
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => src.ModifiedDate))
                .ForMember(dest => dest.CreatedById, opt => opt.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.UserName : null))
                .ForMember(dest => dest.LastModifiedById, opt => opt.MapFrom(src => src.LastModifiedById))
                .ForMember(dest => dest.LastModifiedByUserName, opt => opt.MapFrom(src => src.LastModifiedBy != null ? src.LastModifiedBy.UserName : null))
                .ReverseMap();

            // ✅ CREATE mapping
            CreateMap<VendorContactCreateDto, VendorContact>()
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => new Person
                {
                    FirstName = src.FirstName,
                    LastName = src.LastName,
                    Nationality = src.Nationality,
                    Religion = src.Religion,
                }))
                .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => new Contact
                {
                    Email = src.Email,
                    PhoneNumber = src.PhoneNumber,
                    WebsiteUrl = src.WebsiteUrl,
                }))
                .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src.Designation))
                .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true)) // ✅ new contacts default Active
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.PersonId, opt => opt.Ignore())
                .ForMember(dest => dest.ContactId, opt => opt.Ignore());

                CreateMap<VendorContactUpdateDto, VendorContact>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src.Designation))
                 .ForMember(dest => dest.IsPrimary, opt => opt.MapFrom(src => src.IsPrimary))
                 .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                  .ForMember(dest => dest.VendorId, opt => opt.MapFrom(src => src.VendorId))
                 .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                 .ForMember(dest => dest.LastModifiedById, opt => opt.MapFrom(src => src.LastModifiedById))
                 .ForMember(dest => dest.Person, opt => opt.MapFrom(src => new Person
                 {
                      Id = src.PersonId ?? 0,
                         FirstName = src.FirstName,
                           LastName = src.LastName,
                          Nationality = src.Nationality,
                              Religion = src.Religion
                  }))
                   .ForMember(dest => dest.Contact, opt => opt.MapFrom(src => new Contact
                   {
                      Id = src.ContactId ?? 0,
                      Email = src.Email,
                       PhoneNumber = src.PhoneNumber,
                       WebsiteUrl = src.WebsiteUrl
                  }))
                     .ForMember(dest => dest.Vendor, opt => opt.Ignore())
                    .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                    .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore());

        }
    }
}