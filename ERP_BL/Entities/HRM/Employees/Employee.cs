using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Base.Persons;
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.CompanyCenter.Departments;
using ERP_BL.Entities.Core.Users;
using ERP_BL.Entities.Leaves;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.HRM.Employees
{
    [Index(nameof(SystemDisplayName), IsUnique = true)]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // ---------------- REQUIRED FIELDS ----------------
        //system display name
        [Required(ErrorMessage = "System Display Name is required.")]
        [StringLength(100)]
        public string SystemDisplayName { get; set; } = null!;


        // job title
        public string? JobTitle { get; set; }

        // Dates
        [DataType(DataType.Date)]
        public DateTime? ProbationPeriodEndDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }

        //Hire date
        [Required(ErrorMessage = "Hire Date is required.")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        // Employment type
        [Required(ErrorMessage = "Employment Type is required.")]
        [StringLength(50)]
        public string EmploymentType { get; set; } = null!;
        // Employement status
        [Required(ErrorMessage = "Employee Status is required.")]
        [StringLength(50)]
        public string EmployeeStatus { get; set; } = null!;

        [Required(ErrorMessage = "Employee Status Class is required.")]
        [StringLength(50)]
        public string EmployeeStatusClass { get; set; } = null!;

        // Profile Picture Fields
        public byte[]? ProfilePicture { get; set; }

        [StringLength(100)]
        public string? ProfilePictureContentType { get; set; }

        public long? ProfilePictureSize { get; set; }

        [StringLength(255)]
        public string? ProfilePictureFileName { get; set; }

        // Manager & HR Manager as EmployeeId (FK)
        public int? ManagerId { get; set; }
        [ForeignKey(nameof(ManagerId))]
        public virtual Employee? Manager { get; set; }

        public int? HRManagerId { get; set; }
        [ForeignKey(nameof(HRManagerId))]
        public virtual Employee? HRManager { get; set; }

      

        // ---------------- Foreign keys ----------------
        [ForeignKey(nameof(Contact))]
        public int? ContactId { get; set; }
        public virtual Contact? Contact { get; set; }

        
        public int? PersonId { get; set; }
        [ForeignKey(nameof(PersonId))]
        public virtual Person? Person { get; set; }

        public int? PermanentAddressId { get; set; }
        [ForeignKey(nameof(PermanentAddressId))]
        public virtual Address? PermanentAddress { get; set; }

        [StringLength(50)]
        public string? PayGrade { get; set; }


        // Secondary address
        public int? TemporaryAddressId { get; set; }
        [ForeignKey(nameof(TemporaryAddressId))]
        public virtual Address? TemporaryAddress { get; set; }

        // ---------------- METADATA ----------------
        [ForeignKey(nameof(CreatedByUser))]
        public int? CreatedByUserId { get; set; }
        public virtual User? CreatedByUser { get; set; }

        [ForeignKey(nameof(LastModifiedByUser))]
        public int? LastModifiedByUserId { get; set; }
        public virtual User? LastModifiedByUser { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedDate { get; set; }

        // Status
        public bool IsActive { get; set; } = true;

        // Navigation
        public virtual ICollection<LeaveApplication> LeaveApplications { get; set; } = new HashSet<LeaveApplication>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        [InverseProperty("Employees")]
        public virtual ICollection<Department> Departments { get; set; } = new HashSet<Department>();

        [ValidateNever]
        public virtual ICollection<Company> Companies { get; set; } = new HashSet<Company>();

    }
}