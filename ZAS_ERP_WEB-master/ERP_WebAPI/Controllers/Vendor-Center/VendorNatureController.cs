using ERP_BL.Entities;
using ERP_REPO.Repo;
using Microsoft.AspNetCore.Mvc;
namespace ERP_WebAPI.Controllers
{//
    [Route("[controller]")]
    [ApiController]
    public class VendorNatureController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;  
        private readonly ILogger<VendorNatureController> _logger;

        public VendorNatureController(IUnitOfWork unitOfWork, ILogger<VendorNatureController> logger)
        {
            _unitOfWork = unitOfWork; 
            _logger = logger;
        }

        [HttpGet("GetAllVendorNatures")]
        public async Task<IActionResult> GetAllVendorNatures(
     [FromQuery] string status = "All",
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10,
     [FromQuery] string? sortColumn = null,
     [FromQuery] string? sortDirection = "asc",
     [FromQuery] string? searchTerm = null)
        {
            try
            {
                var (vendorNatures, totalCount) = await _unitOfWork.VendorNatures.GetAllAsync(
                    status: status,
                    sortColumn: sortColumn,
                    sortDirection: sortDirection,
                    searchTerm: searchTerm,
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );

                return Ok(new
                {
                    success = true,
                    totalCount,
                    pageNumber,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    data = vendorNatures
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllVendorNatures");
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }


        [HttpGet("GetVendorNatureById/{id}")]
        public async Task<ActionResult<VendorNature>> GetVendorNatureById(int id)
        {
            try
            {
                var vendorNature = await _unitOfWork.VendorNatures.GetByIdAsync(id);

                if (vendorNature == null)
                {
                    return NotFound();
                }

                return Ok(vendorNature);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetVendorNatureById: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddVendorNature")]
        public async Task<ActionResult<VendorNature>> AddVendorNature([FromBody] VendorNature vendorNature)
        {
            if (vendorNature == null)
            {
                return BadRequest("Vendor Nature data is required");
            }

            try
            {
                var user = this.User;
                await _unitOfWork.VendorNatures.AddAsync(vendorNature, user);  

                await _unitOfWork.SaveAsync(); 

                return CreatedAtAction(nameof(GetAllVendorNatures), new { id = vendorNature.Id }, vendorNature);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddVendorNature: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("UpdateVendorNature/{id}")]
        public async Task<IActionResult> UpdateVendorNature(int id, [FromBody] VendorNature vendorNature)
        {
            if (id != vendorNature.Id)
            {
                return BadRequest("Vendor Nature ID mismatch");
            }

            try
            {
                var existingVendorNature = await _unitOfWork.VendorNatures.GetByIdAsync(id);

                if (existingVendorNature == null)
                {
                    return NotFound();
                }

                // Detach the existing entity if it's being tracked
                _unitOfWork.Detach(existingVendorNature);

                // Now update the entity
                var user = this.User;
                _unitOfWork.VendorNatures.Update(vendorNature, user);

                await _unitOfWork.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateVendorNature: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpDelete("DeactivateVendorNature/{id}")]
        public async Task<IActionResult> DeactivateVendorNature(int id)
        {
            try
            {
                var vendorNature = await _unitOfWork.VendorNatures.GetByIdAsync(id);

                if (vendorNature == null)
                {
                    return NotFound();
                }

                var user = this.User;
                await _unitOfWork.VendorNatures.DeactivateAsync(id);  
                await _unitOfWork.SaveAsync();  

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeactivateVendorNature: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
