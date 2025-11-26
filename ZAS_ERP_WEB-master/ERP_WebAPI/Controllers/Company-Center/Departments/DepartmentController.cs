// ERP_WebAPI/Controllers/CompanyCenter/Departments/DepartmentController.cs

using ERP_BL.Data;
using ERP_BL.Entities.Company_Center.Departments.Dtos;
using ERP_BL.Entities.CompanyCenter.Departments;
using ERP_REPO.Repo.CompanyCenter.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERP_WebAPI.Controllers.CompanyCenter.Departments
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class DepartmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDepartmentRepo _departmentRepo;

        public DepartmentController(ApplicationDbContext context, IDepartmentRepo departmentRepo)
        {
            _context = context;
            _departmentRepo = departmentRepo;
        }

        private async Task<bool> IsPowerUserAsync()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username)) return false;

            return await _context.PowerUsers
                .AsNoTracking()
                .AnyAsync(p => p.UserName.ToLower() == username.ToLower());
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<DepartmentResponseDto>> GetById(int id)
        {
            var department = await _departmentRepo.GetDepartmentByIdAsync(id);
            return department == null ? NotFound() : Ok(MapToDepartmentResponseDto(department));
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> GetAll(string status = "all")
        {
            var departments = await _departmentRepo.GetAllDepartmentsAsync(status);
            return Ok(departments.Select(MapToDepartmentResponseDto));
        }

        [HttpPost("Create")]
        public async Task<ActionResult<DepartmentResponseDto>> Create([FromBody] DepartmentRequest request)
        {
            if (await IsPowerUserAsync())
                return Forbid("PowerUser is not allowed to create Departments.");

            try
            {
                var created = await _departmentRepo.CreateDepartmentAsync(
                    request.Department,
                    request.EmployeeIds,
                    request.CompanyIds
                );

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDepartmentResponseDto(created));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<DepartmentResponseDto>> Update(int id, [FromBody] DepartmentRequest request)
        {
            if (id != request.Department.Id)
                return BadRequest("Department ID mismatch");

            if (await IsPowerUserAsync())
                return Forbid("PowerUser is not allowed to update Departments.");

            try
            {
                var updated = await _departmentRepo.UpdateDepartmentAsync(
                    request.Department,
                    request.EmployeeIds,
                    request.CompanyIds
                );

                return updated == null ? NotFound() : Ok(MapToDepartmentResponseDto(updated));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
        }

        [HttpGet("{departmentId}/employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesByDepartment(int departmentId)
        {
            var employees = await _context.Employees
                .Where(e => e.Departments.Any(d => d.Id == departmentId))
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    SystemDisplayName = e.SystemDisplayName
                })
                .ToListAsync();

            return employees.Count == 0
                ? NotFound($"No employees found for department {departmentId}")
                : Ok(employees);
        }

        [HttpPost("assign-employees-to-companies")]
        public async Task<IActionResult> AssignEmployeesToCompanies([FromBody] AssignEmployeesToCompaniesRequest request)
        {
            if (request?.CompanyIds == null || request.EmployeeIds == null)
                return BadRequest("CompanyIds and EmployeeIds are required.");

            await _departmentRepo.AssignEmployeesToCompaniesAsync(request.CompanyIds, request.EmployeeIds);
            return Ok(new { message = "Employees assigned to companies successfully." });
        }

        private static DepartmentResponseDto MapToDepartmentResponseDto(Department department)
        {
            return new DepartmentResponseDto
            {
                Id = department.Id,
                DeptName = department.DeptName,
                DeptCode = department.DeptCode,
                Abbreviation = department.Abbreviation,
                IsSubsidiary = department.IsSubsidiary,
                ParentDepartmentId = department.ParentDepartmentId,
                CreatedDate = department.CreatedDate,
                CreatedByUserName = department.CreatedByUser?.UserName,
                LastModifiedByUserName = department.LastModifiedByUser?.UserName,
                LastModifiedDate = department.LastModifiedDate,
                IsActive = department.IsActive,
                Employees = department.Employees?.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    SystemDisplayName = e.SystemDisplayName
                }).ToList() ?? new List<EmployeeDto>(),
                Companies = department.Companies?.Select(c => new CompanyDto
                {
                    Id = c.Id,
                    CompanyName = c.CompanyName
                }).ToList() ?? new List<CompanyDto>()
            };
        }

        public class AssignEmployeesToCompaniesRequest
        {
            public List<int> CompanyIds { get; set; } = new();
            public List<int> EmployeeIds { get; set; } = new();
        }
    }
}