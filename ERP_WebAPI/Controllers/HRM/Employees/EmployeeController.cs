using ERP_BL.Entities.HRM.Employees.Dtos;
using ERP_REPO.Repo.HRM.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("[controller]")]
[ApiController]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepo _repo;
    private readonly IWebHostEnvironment _environment;

    public EmployeeController(IEmployeeRepo repo, IWebHostEnvironment environment)
    {
        _repo = repo;
        _environment = environment;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] string status = "all")
        => Ok(await _repo.GetAllEmployeesAsync(status));

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(int id)
        => await _repo.GetEmployeeByIdAsync(id) is { } emp ? Ok(emp) : NotFound();

    [HttpGet("ProfilePicture/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProfilePicture(int id)
    {
        var pic = await _repo.GetEmployeeProfilePictureAsync(id);

        if (pic.HasValue && pic.Value.Data != null)
        {
            var (data, contentType) = pic.Value;
            return File(data, contentType ?? "application/octet-stream");
        }

        return NotFound();
    }


    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] EmployeeCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await ProcessProfilePictureFile(dto); // now works
            var employee = await _repo.CreateEmployeeAsync(dto);
            var response = await _repo.GetEmployeeByIdAsync(employee.Id);

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, new
            {
                message = "Employee created successfully.",
                employee = response
            });
        }
        catch (UnauthorizedAccessException) { return Unauthorized("User not authenticated."); }
        catch (Exception ex) { return StatusCode(500, new { error = ex.Message }); }
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] EmployeeUpdateDto dto)
    {
        if (!ModelState.IsValid || id != dto.Id)
            return BadRequest("Invalid data or ID mismatch.");

        await ProcessProfilePictureFile(dto);
        var success = await _repo.UpdateEmployeeAsync(dto);

        return success
            ? Ok(new { message = "Employee updated successfully." })
            : NotFound();
    }


    [HttpGet("GetAvailableEmployees")]
    public async Task<IActionResult> GetAvailable()
        => Ok(await _repo.GetAvailableEmployeesAsync());

    [HttpGet("GetAvailableEmployeesForEdit/{userId}")]
    public async Task<IActionResult> GetAvailableForEdit(int userId)
        => Ok(await _repo.GetAvailableEmployeesForEditAsync(userId));

    // DELETE: api/employees/5/profile-picture
    [HttpDelete("{id}/profile-picture")]
    public async Task<IActionResult> DeleteProfilePicture(int id)
    {
        try
        {
            var success = await _repo.RemoveEmployeeProfilePictureAsync(id);
            if (!success)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting profile picture: {ex.Message}");
        }
    }

    // Helper method to process profile picture file
    private async Task ProcessProfilePictureFile<T>(T dto) where T : class
    {
        var fileProp = dto.GetType().GetProperty("ProfilePictureFile")?.GetValue(dto) as IFormFile;
        if (fileProp == null || fileProp.Length == 0) return;

        // --- Validation ---
        if (fileProp.Length > 5 * 1024 * 1024)
            throw new InvalidOperationException("Profile picture must be ≤ 5 MB.");

        var ext = Path.GetExtension(fileProp.FileName).ToLowerInvariant();
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        if (!allowed.Contains(ext))
            throw new InvalidOperationException("Only JPG, PNG, GIF, WEBP allowed.");

        // --- Read file ---
        await using var ms = new MemoryStream();
        await fileProp.CopyToAsync(ms);

        // --- Set DTO properties ---
        var byteArrayProp = dto.GetType().GetProperty("ProfilePicture");
        var contentTypeProp = dto.GetType().GetProperty("ProfilePictureContentType");
        var sizeProp = dto.GetType().GetProperty("ProfilePictureSize");
        var fileNameProp = dto.GetType().GetProperty("ProfilePictureFileName");

        byteArrayProp?.SetValue(dto, ms.ToArray());
        contentTypeProp?.SetValue(dto, fileProp.ContentType);
        sizeProp?.SetValue(dto, fileProp.Length);
        fileNameProp?.SetValue(dto, fileProp.FileName);
    }
}