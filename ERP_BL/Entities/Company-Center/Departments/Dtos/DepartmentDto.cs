namespace ERP_BL.Entities.Company_Center.Departments.Dtos
{
    // DTO for Employee with limited fields
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string SystemDisplayName { get; set; }
    }

    // DTO for Company with limited fields
    public class CompanyDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
    }

    // DTO for Department response
    public class DepartmentResponseDto
    {
        public int Id { get; set; }
        public string DeptName { get; set; }
        public string DeptCode { get; set; }
        public string? Abbreviation { get; set; }
        public bool IsSubsidiary { get; set; }
        public int? ParentDepartmentId { get; set; }
        
        public bool IsActive { get; set; }


        public DateTime CreatedDate { get; set; }
        public string? CreatedByUserName { get; set; }
        public string? LastModifiedByUserName { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public List<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();
        public List<CompanyDto> Companies { get; set; } = new List<CompanyDto>();
    }
}
