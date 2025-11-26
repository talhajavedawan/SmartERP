using ERP_BL.Data;
using ERP_BL.Entities.Core.PowerUsers;
using ERP_BL.Entities.HRM.Employees.List.JobTitle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERP_WebAPI.Controllers.HRM.Employees.List
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class JobTitleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JobTitleController(ApplicationDbContext context)
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
            // Get the username from claims (the logged-in user)
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return false;

            // Check if that username exists in the PowerUsers table
            return await _context.PowerUsers
                .AsNoTracking()
                .AnyAsync(p => p.UserName.ToLower() == username.ToLower());
        }

        // -------------------- GET: JobTitle/GetAll --------------------
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string status = "all")
        {
            try
            {
                var query = _context.JobTitles
                    .Include(j => j.CreatedByUser)
                    .Include(j => j.LastModifiedByUser)
                    .AsQueryable();

                status = status?.Trim().ToLower() ?? "all";

                query = status switch
                {
                    "active" => query.Where(j => j.IsActive),
                    "inactive" => query.Where(j => !j.IsActive),
                    _ => query
                };

                var jobTitles = await query
                    .AsNoTracking()
                    .Select(j => new
                    {
                        j.Id,
                        j.JobTitleName,
                        j.IsActive,
                        CreatedByUserName = j.CreatedByUser != null ? j.CreatedByUser.UserName : null,
                        j.CreatedDate,
                        LastModifiedByUserName = j.LastModifiedByUser != null ? j.LastModifiedByUser.UserName : null,
                        j.LastModifiedDate
                    })
                    .ToListAsync();

                return Ok(jobTitles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error fetching job titles: {ex.Message}" });
            }
        }

        // -------------------- GET: JobTitle/GetById/{id} --------------------
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var jobTitle = await _context.JobTitles
                    .Include(j => j.CreatedByUser)
                    .Include(j => j.LastModifiedByUser)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(j => j.Id == id);

                if (jobTitle == null)
                    return NotFound(new { Message = $"JobTitle with ID {id} not found." });

                return Ok(jobTitle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error fetching JobTitle {id}: {ex.Message}" });
            }
        }

        // -------------------- POST: JobTitle/Create --------------------
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] JobTitle jobTitle)
        {
            if (jobTitle == null)
                return BadRequest(new { Message = "JobTitle data cannot be null." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                int userId = GetLoggedInUserId();
                if (userId == 0)
                    return Unauthorized(new { Message = "User is not authenticated." });

                if (await IsPowerUserAsync())
                    return Forbid("PowerUser is not allowed to create Job Titles.");

                jobTitle.CreatedByUserId = userId;
                jobTitle.CreatedDate = DateTime.UtcNow;
                jobTitle.LastModifiedByUserId = null;
                jobTitle.LastModifiedDate = null;

                await _context.JobTitles.AddAsync(jobTitle);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = jobTitle.Id }, jobTitle);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { Message = "Database update failed.", Details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error creating job title.", Details = ex.Message });
            }
        }

        // -------------------- PUT: JobTitle/Update/{id} --------------------
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] JobTitle jobTitle)
        {
            if (jobTitle == null || jobTitle.Id != id)
                return BadRequest(new { Message = "Invalid JobTitle data." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existing = await _context.JobTitles.FindAsync(id);
                if (existing == null)
                    return NotFound(new { Message = $"JobTitle with ID {id} not found." });

                int userId = GetLoggedInUserId();
                if (userId == 0)
                    return Unauthorized(new { Message = "User is not authenticated." });

                if (await IsPowerUserAsync())
                    return Forbid("PowerUser is not allowed to update Job Titles.");

                existing.JobTitleName = jobTitle.JobTitleName;
                existing.IsActive = jobTitle.IsActive;
                existing.LastModifiedByUserId = userId;
                existing.LastModifiedDate = DateTime.UtcNow;

                _context.JobTitles.Update(existing);
                await _context.SaveChangesAsync();

                return Ok(existing);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "This record was modified by another user." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error updating JobTitle {id}: {ex.Message}" });
            }
        }
    }
}
