using AutoMapper;
using ERP_BL.Data;
using ERP_BL.Entities;
using ERP_REPO.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//
namespace ERP_WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class VendorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IVendorRepo _vendorRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<VendorController> _logger;

 
        public VendorController(
            IVendorRepo vendorRepo,
            IMapper mapper,
            ILogger<VendorController> logger,
            ApplicationDbContext context)
        {
            _context = context;
            _vendorRepo = vendorRepo;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet("GetAllVendor")]
 
        public async Task<IActionResult> GetAllVendor(
            [FromQuery] string status = "All",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? searchTerm = null)
        {
            try
            {

                var (vendors, totalCount) = await ((VendorService)_vendorRepo)
                    .GetAllForUserAsync(User, status, sortColumn, sortDirection, searchTerm, pageNumber, pageSize);

 
                var vendorDtos = _mapper.Map<IEnumerable<VendorGetDto>>(vendors);


                return Ok(new
                {
                    success = true,
                    totalCount,
                    pageNumber,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    data = vendorDtos
                });
            }
            catch (Exception ex)
            {
           
                _logger.LogError(ex, "Error in GetAllVendor");
                return StatusCode(500, new { error = "Failed to retrieve vendors", details = ex.Message });
            }
        }


    
        [HttpGet("GetVendorById/{id:int}")]
        public async Task<IActionResult> GetVendorById(int id)
        {
            try
            {
                var vendor = await _vendorRepo.GetByIdAsync(id,

                    includeProperties:
                        "Company,Company.Contact,Company.IndustryType,Company.BusinessType," +
                        "Currency,VendorNature,ParentVendor,ParentVendor.Company," +
                        "ClientCompanies,Departments," +
                        "ShippingAddress,ShippingAddress.Country,ShippingAddress.State,ShippingAddress.City,ShippingAddress.Zone," +
                        "BillingAddress,BillingAddress.Country,BillingAddress.State,BillingAddress.City,BillingAddress.Zone");

                if (vendor == null)
                 
                    return NotFound(new { message = $"Vendor with ID {id} not found." });

                var vendorDto = _mapper.Map<VendorGetDto>(vendor);
     
                return Ok(new { success = true, data = vendorDto });
            }
            catch (Exception ex)
            {
  
                _logger.LogError(ex, "Error in GetVendorById");
                return StatusCode(500, new { error = "Failed to retrieve vendor", details = ex.Message });
            }
        }

        [HttpPost("CreateVendor")]
        public async Task<IActionResult> CreateVendor([FromBody] VendorCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await using var transaction = await _vendorRepo.BeginTransactionAsync();

            try
            {
        
                bool exists = await _vendorRepo.ExistsAsync(v =>
                    v.Company.CompanyName.ToLower() == dto.VendorName.ToLower());

                if (exists)
                 
                    return Conflict(new { message = "Vendor with the same name already exists." });

                if (!dto.CurrencyId.HasValue || !await _context.Currencies.AnyAsync(c => c.Id == dto.CurrencyId.Value))
            
                    return BadRequest(new { message = "Invalid or missing CurrencyId." });

                if (!dto.VendorNatureId.HasValue || !await _context.VendorNatures.AnyAsync(v => v.Id == dto.VendorNatureId.Value))
  
                    return BadRequest(new { message = "Invalid or missing VendorNatureId." });

                if (dto.ParentVendorId.HasValue && !await _context.Vendors.AnyAsync(v => v.Id == dto.ParentVendorId.Value))
                  
                    return BadRequest(new { message = "Invalid ParentVendorId." });

                if (await _vendorRepo.IsCircularRelationAsync(0, dto.ParentVendorId))
                    return BadRequest(new { message = "Circular parent hierarchy detected." });

                var vendor = _mapper.Map<Vendor>(dto);

               
                if (dto.ClientCompanyIds != null && dto.ClientCompanyIds.Any())
                {
        
                    vendor.ClientCompanies = await _context.Companies
                        .Where(c => dto.ClientCompanyIds.Contains(c.Id))
                        .ToListAsync();
                }

        
                if (dto.DepartmentIds != null && dto.DepartmentIds.Any())
                {
          
                    vendor.Departments = await _context.Departments
                        .Where(d => dto.DepartmentIds.Contains(d.Id))
                        .ToListAsync();
                }


                await _vendorRepo.AddAsync(vendor, User);
                await _vendorRepo.SaveAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
              
                    success = true,
                    message = "Vendor created successfully.",
                    vendorId = vendor.Id
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
             
                _logger.LogError(ex, "Error creating vendor");
                return StatusCode(500, new { error = "Failed to create vendor", details = ex.Message });
            }
        }

     
        [HttpPut("UpdateVendor/{id:int}")]
        public async Task<IActionResult> UpdateVendor(int id, [FromBody] VendorUpdateDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch." });

            await using var transaction = await _vendorRepo.BeginTransactionAsync();

            try
            {

                if (dto.CurrencyId.HasValue && !await _context.Currencies.AnyAsync(c => c.Id == dto.CurrencyId.Value))
               
                    return BadRequest(new { message = "Invalid CurrencyId." });

                if (dto.VendorNatureId.HasValue && !await _context.VendorNatures.AnyAsync(v => v.Id == dto.VendorNatureId.Value))
              
                    return BadRequest(new { message = "Invalid VendorNatureId." });

                if (dto.ParentVendorId.HasValue && !await _context.Vendors.AnyAsync(v => v.Id == dto.ParentVendorId.Value))
                    
                    return BadRequest(new { message = "Invalid ParentVendorId." });

        
                if (await _vendorRepo.IsCircularRelationAsync(id, dto.ParentVendorId))
                    return BadRequest(new { message = "Circular parent hierarchy detected." });

              
                await ((VendorService)_vendorRepo).UpdateVendorAsync(dto, User);

                
                await transaction.CommitAsync();

              
                return Ok(new
                {
                    success = true,
                    message = "Vendor updated successfully.",
                    vendorId = id
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating vendor {Id}", id);
                return StatusCode(500, new { error = "Failed to update vendor", details = ex.Message });
            }
        }
    
        [HttpDelete("Deactivate/{id:int}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                var success = await _vendorRepo.DeactivateAsync(id);
                if (!success)
             
                    return NotFound(new { message = "Vendor not found or already inactive." });

                await _vendorRepo.SaveAsync();
         
                return Ok(new { success = true, message = "Vendor deactivated successfully." });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error in DeactivateVendor");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
        [HttpGet("GetSelectableCompanies")]
        public async Task<IActionResult> GetSelectableCompanies()
        {
            try
            {
                var companies = await _vendorRepo.GetSelectableCompaniesAsync();
                if (companies == null || !companies.Any())
                    return NotFound(new { message = "No companies found." });

                var result = companies.Select(c => new
                {
                    c.Id,
                    c.CompanyName,
                    ParentCompanyId = c.ParentCompanyId  
                });

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSelectableCompanies");
                return StatusCode(500, new { error = "Failed to retrieve companies.", details = ex.Message });
            }
        }

        [HttpGet("GetDepartmentsByCompany/{companyId:int}")]
        public async Task<IActionResult> GetDepartmentsByCompany(int companyId)
        {
            try
            {
                var departments = await _vendorRepo.GetDepartmentsByCompanyAsync(companyId);
                if (departments == null || !departments.Any())
                    return NotFound(new { message = $"No departments found for company ID {companyId}." });
                var result = departments.Select(d => new
                {
                    d.Id,
                    d.DeptName,
                    ParentDepartmentId = d.ParentDepartmentId  
                });

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDepartmentsByCompany");
                return StatusCode(500, new { error = "Failed to retrieve departments.", details = ex.Message });
            }
        }
        [HttpGet("GetDropdown")]
        public async Task<IActionResult> GetDropdown()
        {
            try
            {
                var (vendors, _) = await ((VendorService)_vendorRepo)
                    .GetAllForUserAsync(User, status: "active");

                if (!vendors.Any())
                    return NotFound(new { message = "No vendors found for your assigned company/department." });

                var result = vendors.Select(v => new
                {
                    v.Id,
                    CompanyName = v.Company.CompanyName
                });

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDropdown");
                return StatusCode(500, new { error = "Failed to retrieve vendors.", details = ex.Message });
            }
        }


    }
}
