using ERP_BL.Entities.Base.Addresses.Dtos;
using ERP_BL.Entities.Base.Contacts.Dtos;
using ERP_BL.Entities.Company_Center.Companies.Dtos;
using ERP_BL.Enums;

namespace ERP_BL.Entities.CompanyCenter.Companies.Dtos
{
    public class CompanyGetDto
    {

        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string? Ntn { get; set; }
        public bool IsActive { get; set; }
        public bool IsSubsidiary { get; set; }
        public CompanyType CompanyType { get; set; }

        public DateTime? OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastModified { get; set; }

        public int? CreatedById { get; set; }
        public string? CreatedByName { get; set; }
        public int? LastModifiedById { get; set; }
        public string? LastModifiedByName { get; set; }

        public GroupDto? Group { get; set; }
        public BusinessTypeDto? BusinessType { get; set; }
        public IndustryTypeDto? IndustryType { get; set; }
        public AddressDto? Address { get; set; }
        public ContactDto? Contact { get; set; }

        public string? ParentCompanyName { get; set; }
    }
}
