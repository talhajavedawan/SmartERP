// DepartmentRepo.cs
using ERP_BL.Data;
using ERP_BL.Entities.CompanyCenter.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ERP_REPO.Repo.CompanyCenter.Departments
{
    public interface IDepartmentRepo
    {
        Task<IEnumerable<Department>> GetAllDepartmentsAsync(string status = "all");
        Task<Department?> GetDepartmentByIdAsync(int id);
        Task<Department> CreateDepartmentAsync(Department department, List<int>? employeeIds = null, List<int>? companyIds = null);
        Task<Department?> UpdateDepartmentAsync(Department department, List<int>? employeeIds = null, List<int>? companyIds = null);
        Task<bool> DeleteDepartmentAsync(int id);
        Task AssignEmployeesToCompaniesAsync(List<int> companyIds, List<int> employeeIds);
    }

    public class DepartmentRepo : IDepartmentRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartmentRepo(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // GetAllDepartments
        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(string status = "all")
        {
            var query = _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Companies)
                .Include(d => d.CreatedByUser)
                .Include(d => d.LastModifiedByUser)
                .AsQueryable();

            status = status?.Trim().ToLower() ?? "all";
            query = status switch
            {
                "active" => query.Where(d => d.IsActive),
                "inactive" => query.Where(d => !d.IsActive),
                _ => query
            };

            return await query.AsNoTracking().ToListAsync();
        }

        // Get Department by ID
        public async Task<Department?> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Companies)
                .Include(d => d.CreatedByUser)
                .Include(d => d.LastModifiedByUser)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        // Create Department
        public async Task<Department> CreateDepartmentAsync(
            Department department,
            List<int>? employeeIds = null,
            List<int>? companyIds = null)
        {
            // Check duplicate
            bool exists = await _context.Departments
                .AnyAsync(d => d.DeptName == department.DeptName);
            if (exists)
                throw new InvalidOperationException($"Department with name '{department.DeptName}' already exists.");

            int userId = GetCurrentUserId();
            if (userId == 0)
                throw new UnauthorizedAccessException("User is not authenticated.");

            // SET AUDIT FIELDS HERE
            department.CreatedByUserId = userId;
            department.CreatedDate = DateTime.UtcNow;
            department.LastModifiedByUserId = null;
            department.LastModifiedDate = null;

            // Attach employees
            if (employeeIds?.Any() == true)
            {
                var employees = await _context.Employees
                    .Where(e => employeeIds.Contains(e.Id))
                    .ToListAsync();
                department.Employees = employees;
            }

            // Attach companies
            if (companyIds?.Any() == true)
            {
                var companies = await _context.Companies
                    .Where(c => companyIds.Contains(c.Id))
                    .ToListAsync();
                department.Companies = companies;
            }

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        // Update Department
        public async Task<Department?> UpdateDepartmentAsync(
            Department department,
            List<int>? employeeIds = null,
            List<int>? companyIds = null)
        {
            var existingDepartment = await _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Companies)
                .FirstOrDefaultAsync(d => d.Id == department.Id);

            if (existingDepartment == null)
                return null;

            int userId = GetCurrentUserId();
            if (userId == 0)
                throw new UnauthorizedAccessException("User is not authenticated.");

            // UPDATE SCALAR FIELDS
            existingDepartment.DeptName = department.DeptName;
            existingDepartment.DeptCode = department.DeptCode;
            existingDepartment.Abbreviation = department.Abbreviation;
            existingDepartment.IsSubsidiary = department.IsSubsidiary;
            existingDepartment.ParentDepartmentId = department.ParentDepartmentId;
            existingDepartment.IsActive = department.IsActive;

            // SET AUDIT FIELDS HERE
            existingDepartment.LastModifiedByUserId = userId;
            existingDepartment.LastModifiedDate = DateTime.UtcNow;

            // Update employees
            if (employeeIds != null)
            {
                existingDepartment.Employees.Clear();
                if (employeeIds.Any())
                {
                    var employees = await _context.Employees
                        .Where(e => employeeIds.Contains(e.Id))
                        .ToListAsync();
                    foreach (var emp in employees)
                        existingDepartment.Employees.Add(emp);
                }
            }

            // Update companies
            if (companyIds != null)
            {
                existingDepartment.Companies.Clear();
                if (companyIds.Any())
                {
                    var companies = await _context.Companies
                        .Where(c => companyIds.Contains(c.Id))
                        .ToListAsync();
                    foreach (var comp in companies)
                        existingDepartment.Companies.Add(comp);
                }
            }

            await _context.SaveChangesAsync();
            return existingDepartment;
        }

        // Soft Delete
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return false;

            department.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task AssignEmployeesToCompaniesAsync(List<int> companyIds, List<int> employeeIds)
        {
            if (!companyIds.Any() || !employeeIds.Any()) return;

            var companies = await _context.Companies
                .Include(c => c.Employees)
                .Where(c => companyIds.Contains(c.Id))
                .ToListAsync();

            var employees = await _context.Employees
                .Where(e => employeeIds.Contains(e.Id))
                .ToListAsync();

            foreach (var company in companies)
            {
                foreach (var employee in employees)
                {
                    if (!company.Employees.Any(e => e.Id == employee.Id))
                        company.Employees.Add(employee);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}