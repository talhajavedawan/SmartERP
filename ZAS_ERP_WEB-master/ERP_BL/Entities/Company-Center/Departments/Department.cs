using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.Core.Users;
using ERP_BL.Entities.HRM.Employees;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.CompanyCenter.Departments
{
    [Index(nameof(DeptName), IsUnique = true)]
    public class Department
    {
        public List<int> companyIds;
        public List<int> employeeIds;

        //ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Department Name
        [Required(ErrorMessage = "Department Name is required.")]
        [StringLength(100, ErrorMessage = "Department Name cannot exceed 100 characters.")]
        public string DeptName { get; set; }

        // Department Code
        [Required(ErrorMessage = "Department Code is required.")]
        [StringLength(50, ErrorMessage = "Department Code cannot exceed 50 characters.")]
        public string DeptCode { get; set; }

        // Abbreviation
        [StringLength(20, ErrorMessage = "Abbreviation cannot exceed 20 characters.")]
        public string? Abbreviation { get; set; }

        // Child relationship
        public bool IsSubsidiary { get; set; } = false;

        // parent for hierarchy
        public int? ParentDepartmentId { get; set; }
        [ForeignKey("ParentDepartmentId")]
        public virtual Department? ParentDepartment { get; set; }

        // for children of a given parent
        [InverseProperty("ParentDepartment")]
        public virtual ICollection<Department> ChildDepartments { get; set; } = new HashSet<Department>();


        // Many-many relation with Employees and Company

        [InverseProperty("Departments")]
        public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

        [InverseProperty("Departments")]
        public virtual ICollection<Company> Companies { get; set; } = new HashSet<Company>();
        public bool IsActive { get; set; } = true;


        //Audit fields
        [ForeignKey(nameof(CreatedByUser))]
        public int? CreatedByUserId { get; set; }
        public User? CreatedByUser { get; set; }


        [ForeignKey(nameof(LastModifiedByUser))]
        public int? LastModifiedByUserId { get; set; }
        public User? LastModifiedByUser { get; set; }


        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedDate { get; set; }


        [InverseProperty("Departments")]
        public  ICollection<Vendor> Vendors { get; set; } = new HashSet<Vendor>();
       // [InverseProperty("Departments")]
        //public  ICollection<Customer> Customers { get; set; } = new HashSet<Customer>();


    }
}
