using ERP_BL.Data;
using ERP_BL.Entities.CompanyCenter.Companies.List;
using ERP_BL.Entities.Core.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERP_WebAPI.Controllers.CompanyCenter.Companies.List
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class IndustryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IndustryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------- Helper: Get Logged-In User ID --------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // -------------------- GET: Industry/GetAll --------------------
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string status = "all")
        {
            try
            {
                var query = _context.IndustryTypes
                    .Include(i => i.CreatedByUser)
                    .Include(i => i.LastModifiedByUser)
                    .AsQueryable();

                status = status?.Trim().ToLower() ?? "all";
                query = status switch
                {
                    "active" => query.Where(i => i.IsActive),
                    "inactive" => query.Where(i => !i.IsActive),
                    _ => query
                };

                var industries = await query
                    .AsNoTracking()
                    .Select(i => new
                    {
                        i.Id,
                        i.IndustryTypeName,
                        i.IsActive,
                        CreatedByUserName = i.CreatedByUser != null ? i.CreatedByUser.UserName : null,
                        i.CreatedDate,
                        LastModifiedByUserName = i.LastModifiedByUser != null ? i.LastModifiedByUser.UserName : null,
                        i.LastModifiedDate
                    })
                    .ToListAsync();

                return Ok(industries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching industries: {ex.Message}");
            }
        }

        // -------------------- GET: Industry/GetById/{id} --------------------
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var industry = await _context.IndustryTypes
                    .Include(i => i.CreatedByUser)
                    .Include(i => i.LastModifiedByUser)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (industry == null)
                    return NotFound(new { Message = $"IndustryType with ID {id} not found." });

                return Ok(new
                {
                    industry.Id,
                    industry.IndustryTypeName,
                    industry.IsActive,
                    CreatedByUserName = industry.CreatedByUser?.UserName,
                    industry.CreatedDate,
                    LastModifiedByUserName = industry.LastModifiedByUser?.UserName,
                    industry.LastModifiedDate
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching industry {id}: {ex.Message}");
            }
        }

        // -------------------- POST: Industry/Create --------------------
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] IndustryType industryType)
        {
            if (industryType == null)
                return BadRequest(new { Message = "IndustryType data cannot be null." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int userId = GetLoggedInUserId();
                if (userId == 0)
                    return Unauthorized(new { Message = "User is not authenticated." });

                industryType.CreatedByUserId = userId;
                industryType.CreatedDate = DateTime.UtcNow;
                industryType.LastModifiedByUserId = null;
                industryType.LastModifiedDate = null;

                await _context.IndustryTypes.AddAsync(industryType);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = industryType.Id }, industryType);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { Message = "Database update failed", Details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the industry", Details = ex.Message });
            }
        }

        // -------------------- PUT: Industry/Update/{id} --------------------
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] IndustryType industryType)
        {
            if (industryType == null || industryType.Id != id)
                return BadRequest(new { Message = "Invalid IndustryType data." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _context.IndustryTypes.FindAsync(id);
                if (existing == null)
                    return NotFound(new { Message = $"IndustryType with ID {id} not found." });

                int userId = GetLoggedInUserId();
                if (userId == 0)
                    return Unauthorized(new { Message = "User is not authenticated." });

                // Preserve creation data
                existing.IndustryTypeName = industryType.IndustryTypeName;
                existing.IsActive = industryType.IsActive;
                existing.LastModifiedByUserId = userId;
                existing.LastModifiedDate = DateTime.UtcNow;

                _context.IndustryTypes.Update(existing);
                await _context.SaveChangesAsync();

                return Ok(existing);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "The record you attempted to update was modified by another user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while updating industry {id}: {ex.Message}" });
            }
        }
    }
}