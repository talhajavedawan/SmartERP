using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Base.Persons;
using System.Collections.Generic;

namespace ERP_BL.Entities.HRM.Employees.Dtos
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = null!;
    }

    public class DepartmentDto
    {
        public int Id { get; set; }
        public string DeptName { get; set; } = null!;
    }
    // GetAll

    public class EmployeeResponseDto
    {
        public int Id { get; set; }
        public string SystemDisplayName { get; set; } = null!;
        public string? JobTitle { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? ProbationPeriodEndDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string EmploymentType { get; set; } = null!;
        public string? EmployeeStatus { get; set; }
        public string? EmployeeStatusClass { get; set; }
        public bool IsActive { get; set; }


        // Profile Picture
        public string? ProfilePictureUrl { get; set; }

        // Manager & HR Manager
        public int? ManagerId { get; set; }
        public string? ManagerName { get; set; }
        public int? HRManagerId { get; set; }
        public string? HRManagerName { get; set; }

        public string? PayGrade { get; set; }
        

        // Related
        public Person? Person { get; set; }
        public Contact? Contact { get; set; }
        public Address? PermanentAddress { get; set; }
        public Address? TemporaryAddress { get; set; }

        public ICollection<CompanyDto> Companies { get; set; } = new HashSet<CompanyDto>();
        public ICollection<DepartmentDto> Departments { get; set; } = new HashSet<DepartmentDto>();

        // Audit
        public int? CreatedByUserId { get; set; }
        public string? CreatedByUserName { get; set; }
        public int? LastModifiedByUserId { get; set; }
        public string? LastModifiedByUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    // GetById
    public class EmployeeDetailResponseDto : EmployeeResponseDto
    {
        // ---- picture binary (only here) ----
        public byte[]? ProfilePicture { get; set; }
        public string? ProfilePictureContentType { get; set; }
        public long? ProfilePictureSize { get; set; }
        public string? ProfilePictureFileName { get; set; }

        // Override URL to compute from binary (optional, but handy)
        public new string? ProfilePictureUrl => ProfilePicture != null
            ? $"/api/employees/{Id}/profile-picture"
            : null;
    }
}