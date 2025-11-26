using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Company_Center.Companies;
using ERP_BL.Entities.CompanyCenter.Companies.List;
using ERP_BL.Entities.CompanyCenter.Customers;
using ERP_BL.Entities.CompanyCenter.Departments;
using ERP_BL.Entities.Core.Users;
using ERP_BL.Entities.HRM.Employees;
using ERP_BL.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace ERP_BL.Entities.CompanyCenter.Companies
{
    [Index(nameof(CompanyName), IsUnique = true)]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Company Name is required.")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed 100 characters.")]
        public string CompanyName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? OpeningDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ClosingDate { get; set; }
        [StringLength(15, ErrorMessage = "NTN cannot exceed 15 characters.")]
        public string? Ntn { get; set; }
        public bool IsVoid { get; set; }
        public CompanyType CompanyType { get; set; }
        public int? GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group? Group { get; set; }
        public int? BusinessTypeId { get; set; }
        [ForeignKey("BusinessTypeId")]
        [ValidateNever]
        public virtual BusinessType? BusinessType { get; set; }
        public int? IndustryTypeId { get; set; }
        [ForeignKey("IndustryTypeId")]
        [ValidateNever]
        public virtual IndustryType? IndustryType { get; set; }
        public bool IsActive { get; set; } = true;
        public int? AddressId { get; set; }
        [ForeignKey("AddressId")]
        [ValidateNever]
        public virtual Address? Address { get; set; }
        public int? ContactId { get; set; }
        [ForeignKey("ContactId")]
        [ValidateNever]
        public virtual Contact? Contact { get; set; }


        public bool IsSubsidiary { get; set; }
        public int? ParentCompanyId { get; set; }
        [ForeignKey("ParentCompanyId")]
        public virtual Company? ParentCompany { get; set; }
        [InverseProperty(nameof(Employee.Companies))]
        [ValidateNever]
        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        [InverseProperty(nameof(ParentCompany))]
        [ValidateNever]
        public virtual ICollection<Company> ChildCompanies { get; set; }
        [InverseProperty("Companies")]
        public virtual ICollection<Department> Departments { get; set; } = new HashSet<Department>();
        public int? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public  User? CreatedByUser { get; set; }
        public int? LastModifiedById { get; set; }

        [ForeignKey("LastModifiedById")]
        public virtual User? LastModifiedByUser { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastModified { get; set; }
        [InverseProperty("ClientCompanies")]
        public  ICollection<Vendor> Vendors { get; set; } = new HashSet<Vendor>();
        [InverseProperty("Company")]
        public ICollection<Vendor> OwnedVendors { get; set; } = new HashSet<Vendor>();

    }

}