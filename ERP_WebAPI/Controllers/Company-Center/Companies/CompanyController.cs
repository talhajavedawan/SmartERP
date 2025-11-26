using ERP_BL.Data;
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.CompanyCenter.Departments;
using ERP_BL.Enums;
using ERP_REPO.Repo.CompanyCenter.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_WebAPI.Controllers.CompanyCenter.Companies
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepo _companyRepo;
        private readonly ApplicationDbContext _context;

        public CompanyController(ICompanyRepo companyRepo, ApplicationDbContext context)
        {
            _companyRepo = companyRepo ?? throw new ArgumentNullException(nameof(companyRepo));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] string status = "all", [FromQuery] string? companyType = null)
        {
            try
            {
                var companies = await _companyRepo.GetAllCompaniesAsync();

                if (companies == null || !companies.Any())
                    return NoContent();

                var filteredByStatus = status.ToLower() switch
                {
                    "active" => companies.Where(c => c.IsActive && !c.IsVoid),
                    "inactive" => companies.Where(c => !c.IsActive && !c.IsVoid),
                    _ => companies
                };

                IEnumerable<Company> filtered;

                if (!string.IsNullOrWhiteSpace(companyType))
                {
                    if (!Enum.TryParse<CompanyType>(companyType, true, out var parsedType))
                        return BadRequest(new { Message = $"Invalid company type '{companyType}'. Valid values: {string.Join(", ", Enum.GetNames<CompanyType>())}" });

                    filtered = filteredByStatus.Where(c => c.CompanyType == parsedType);
                }
                else
                {
                    filtered = filteredByStatus.Where(c =>
                        c.CompanyType == CompanyType.GroupCompany ||
                        c.CompanyType == CompanyType.IndividualCompany);
                }

                return Ok(filtered);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching companies.", Details = ex.Message });
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid company ID." });

            try
            {
                var company = await _companyRepo.GetCompanyByIdAsync(id);
                return company != null
                    ? Ok(company)
                    : NotFound(new { Message = $"Company with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving company.", Details = ex.Message });
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Company company)
        {
            if (company == null)
                return BadRequest(new { Message = "Company data cannot be null." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await ValidateRelationsAsync(company))
                return BadRequest(new { Message = "Invalid related entity reference." });

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var created = await _companyRepo.CreateCompanyAsync(company);
                await transaction.CommitAsync();

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "Database error while creating company.", Details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Company company)
        {
            if (id <= 0 || company == null)
                return BadRequest(new { Message = "Invalid input data." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await ValidateRelationsAsync(company))
                return BadRequest(new { Message = "Invalid related entity reference." });

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var updated = await _companyRepo.UpdateCompanyAsync(id, company);
                await transaction.CommitAsync();

                return updated != null
                    ? Ok(updated)
                    : NotFound(new { Message = $"Company with ID {id} not found or already voided." });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "Database error while updating company.", Details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "Unexpected error while updating company.", Details = ex.Message });
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Invalid company ID." });

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var deleted = await _companyRepo.DeleteCompanyAsync(id);
                await transaction.CommitAsync();

                return deleted
                    ? Ok(new { Message = "Company soft-deleted (voided) successfully." })
                    : NotFound(new { Message = $"Company with ID {id} not found or already voided." });
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "Database error while deleting company.", Details = dbEx.InnerException?.Message ?? dbEx.Message });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { Message = "Unexpected error while deleting company.", Details = ex.Message });
            }
        }

        [HttpGet("{companyId}/departments")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartmentsByCompany(int companyId)
        {
            if (companyId <= 0)
                return BadRequest(new { Message = "Invalid company ID." });

            var departments = await _context.Departments
                .Where(d => d.Companies.Any(c => c.Id == companyId))
                .ToListAsync();

            if (!departments.Any())
                return NotFound(new { Message = $"No departments found for company {companyId}." });

            return Ok(departments);
        }

        private async Task<bool> ValidateRelationsAsync(Company company)
        {
            if (company.BusinessTypeId.HasValue &&
                !await _context.BusinessTypes.AnyAsync(b => b.Id == company.BusinessTypeId))
                return false;

            if (company.IndustryTypeId.HasValue &&
                !await _context.IndustryTypes.AnyAsync(i => i.Id == company.IndustryTypeId))
                return false;

            if (company.ParentCompanyId.HasValue &&
                !await _context.Companies.AnyAsync(c => c.Id == company.ParentCompanyId))
                return false;

            return true;
        }
    }
}
