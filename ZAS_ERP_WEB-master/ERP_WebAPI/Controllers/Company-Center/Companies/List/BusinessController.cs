using ERP_BL.Data;
using ERP_BL.Entities.Core.PowerUsers;
using ERP_BL.Entities.CompanyCenter.Companies.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERP_WebAPI.Controllers.CompanyCenter.Companies.List
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BusinessController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BusinessController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------- Helper: Get Logged-In User ID --------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // -------------------- Helper: Identify if Current User is a PowerUser --------------------
        private async Task<bool> IsPowerUserAsync()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return false;

            return await _context.PowerUsers
                .AsNoTracking()
                .AnyAsync(p => p.UserName.ToLower() == username.ToLower());
        }

        // -------------------- GET: Business/GetAll --------------------
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string status = "all")
        {
            try
            {
                var query = _context.BusinessTypes
                    .Include(b => b.CreatedByUser)
                    .Include(b => b.LastModifiedByUser)
                    .AsQueryable();

                status = status?.Trim().ToLower() ?? "all";
                query = status switch
                {
                    "active" => query.Where(b => b.IsActive),
                    "inactive" => query.Where(b => !b.IsActive),
                    _ => query
                };

                var businesses = await query
                    .AsNoTracking()
                    .Select(b => new
                    {
                        b.Id,
                        b.BusinessTypeName,
                        b.IsActive,
                        CreatedByUserName = b.CreatedByUser != null ? b.CreatedByUser.UserName : null,
                        b.CreatedDate,
                        LastModifiedByUserName = b.LastModifiedByUser != null ? b.LastModifiedByUser.UserName : null,
                        b.LastModifiedDate
                    })
                    .ToListAsync();

                return Ok(businesses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error fetching Business Types: {ex.Message}" });
            }
        }

        // -------------------- GET: Business/GetById/{id} --------------------
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var business = await _context.BusinessTypes
                    .Include(b => b.CreatedByUser)
                    .Include(b => b.LastModifiedByUser)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (business == null)
                    return NotFound(new { Message = $"BusinessType with ID {id} not found." });

                return Ok(business);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error fetching BusinessType {id}: {ex.Message}" });
            }
        }

        // -------------------- POST: Business/Create --------------------
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BusinessType businessType)
        {
            if (businessType == null)
                return BadRequest(new { Message = "BusinessType data cannot be null." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int userId = GetLoggedInUserId();
                if (userId == 0)
                    return Unauthorized(new { Message = "User is not authenticated." });

                // Restrict PowerUser from creating
                if (await IsPowerUserAsync())
                    return Forbid("PowerUser is not allowed to create Business Types.");

                businessType.CreatedByUserId = userId;
                businessType.CreatedDate = DateTime.UtcNow;
                businessType.LastModifiedByUserId = null;
                businessType.LastModifiedDate = null;

                await _context.BusinessTypes.AddAsync(businessType);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = businessType.Id }, businessType);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { Message = "Database update failed.", Details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating BusinessType.", Details = ex.Message });
            }
        }

        // -------------------- PUT: Business/Update/{id} --------------------
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BusinessType businessType)
        {
            if (businessType == null || businessType.Id != id)
                return BadRequest(new { Message = "Invalid BusinessType data." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _context.BusinessTypes.FindAsync(id);
                if (existing == null)
                    return NotFound(new { Message = $"BusinessType with ID {id} not found." });

                int userId = GetLoggedInUserId();
                if (userId == 0)
                    return Unauthorized(new { Message = "User is not authenticated." });

                // Restrict PowerUser from updating
                if (await IsPowerUserAsync())
                    return Forbid("PowerUser is not allowed to update Business Types.");

                // Preserve creation data
                existing.BusinessTypeName = businessType.BusinessTypeName;
                existing.IsActive = businessType.IsActive;
                existing.LastModifiedByUserId = userId;
                existing.LastModifiedDate = DateTime.UtcNow;

                _context.BusinessTypes.Update(existing);
                await _context.SaveChangesAsync();

                return Ok(existing);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "This record was modified by another user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error updating BusinessType {id}: {ex.Message}" });
            }
        }
    }
}
