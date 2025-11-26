using ERP_BL.Entities.CompanyCenter.Departments;

namespace ERP_BL.Entities.Company_Center.Departments.Dtos
{
    public class DepartmentRequest
    {
        public Department Department { get; set; }
        public List<int>? EmployeeIds { get; set; }
        public List<int>? CompanyIds { get; set; }
    }


}
