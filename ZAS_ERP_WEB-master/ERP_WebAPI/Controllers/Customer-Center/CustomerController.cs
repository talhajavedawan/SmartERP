using ERP_BL.Data;
using ERP_BL.Entities.Customer_Center.Dtos;
using ERP_REPO.Repo.CustomerCenter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERP_WebAPI.Controllers.CompanyCenter
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerRepo _repository;

        public CustomerController(ApplicationDbContext context, ICustomerRepo repository)
        {
            _context = context;
            _repository = repository;
        }

        // -------------------- Helper to extract current user ID --------------------
        private int GetLoggedInUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                // Log for debugging
                Console.WriteLine("Error: NameIdentifier claim is missing or null");
                return 0;
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                // Log for debugging
                Console.WriteLine($"Error: NameIdentifier claim '{userIdClaim}' is not a valid integer");
                return 0;
            }

            return userId;
        }
        // -------------------- CREATE --------------------
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CustomerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = GetLoggedInUserId();
            if (userId == 0)
                return Unauthorized("Invalid or missing user token.");

            try
            {
                var customer = await _repository.CreateCustomerAsync(dto, userId);
                return Ok(new
                {
                    Message = "Customer created successfully",
                    CustomerId = customer.Id
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database error: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating customer: {ex.Message}");
            }
        }

        // -------------------- UPDATE --------------------
        [HttpPut("update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerCreateDto dto)
        {
            if (dto == null || id <= 0)
                return BadRequest("Invalid data.");

            int userId = GetLoggedInUserId();
            if (userId == 0)
                return Unauthorized("Invalid or missing user token.");

            try
            {
                var updated = await _repository.UpdateCustomerAsync(id, dto, userId);
                if (updated == null)
                    return NotFound($"Customer with ID {id} not found.");

                // Return a JSON object instead of a plain string
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error updating customer: {ex.Message}" });
            }
        }


        // -------------------- DELETE (Soft Delete) --------------------
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid customer ID.");

            int userId = GetLoggedInUserId();
            if (userId == 0)
                return Unauthorized("Invalid or missing user token.");

            try
            {
                var deleted = await _repository.DeleteCustomerAsync(id, userId);
                if (!deleted)
                    return NotFound($"Customer with ID {id} not found.");

                return Ok("Customer deleted (deactivated) successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting customer: {ex.Message}");
            }
        }

        // -------------------- GET ALL --------------------
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string status = "all")
        {
            try
            {
                var result = await _repository.GetAllCustomersAsync(status);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching customers: {ex.Message}");
            }
        }

        // -------------------- GET BY ID --------------------
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid customer ID.");

            try
            {
                var result = await _repository.GetCustomerByIdAsync(id);
                if (result == null)
                    return NotFound($"Customer with ID {id} not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching customer: {ex.Message}");
            }
        }
    }
}
