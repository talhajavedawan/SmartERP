using ERP_BL.Entities.Core.Permissions;
using ERP_BL.Entities.Core.Permissions.Dtos;
using ERP_REPO.Repo.Core.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace ERP_WebAPI.Controllers.Core.Permissions
{
  [ApiController]
  [Route("[controller]")]
    [Authorize]
    public class PermissionController : ControllerBase
  {
    private readonly IPermissionRepo _permissionRepo;

    public PermissionController(IPermissionRepo permissionRepo)
    {
      _permissionRepo = permissionRepo;
    }

    // ✅ Get All Permissions

    [HttpGet("GetAll")]
    public async Task<ActionResult<PaginatedPermissions>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? search = null)
    {
      try
      {
        var result = await _permissionRepo.GetAllPermissionsPaginatedAsync(page, pageSize, search);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error fetching permissions.", details = ex.Message });
      }
    }



    // ✅ Get Permission by Id
    [HttpGet("{id}")]
    public async Task<ActionResult<Permission>> GetById(int id)
    {
      try
      {
        var permission = await _permissionRepo.GetPermissionByIdAsync(id);
        if (permission == null)
          return NotFound(new { message = $"Permission with ID {id} not found." });

        return Ok(permission);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error fetching permission.", details = ex.Message });
      }
    }

    // ✅ Create Permission
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] Permission permission)
    {
      try
      {
        if (permission == null || string.IsNullOrWhiteSpace(permission.Name))
          return BadRequest(new { message = "Permission name is required." });

        var createdPermission = await _permissionRepo.AddPermissionAsync(permission);
        return CreatedAtAction(nameof(GetById), new { id = createdPermission.Id }, createdPermission);
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error creating permission.", details = ex.Message });
      }
    }

    // ✅ Update Permission
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Permission permission)
    {
      try
      {
        if (id != permission.Id)
          return BadRequest(new { message = "Permission ID mismatch." });

        var existing = await _permissionRepo.GetPermissionByIdAsync(id);
        if (existing == null)
          return NotFound(new { message = $"Permission with ID {id} not found." });

        var updated = await _permissionRepo.UpdatePermissionAsync(permission);
        return Ok(new { message = "Permission updated successfully.", updated });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error updating permission.", details = ex.Message });
      }
    }

    // ✅ Delete Permission
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
      try
      {
        var deleted = await _permissionRepo.DeletePermissionAsync(id);
        if (!deleted)
          return NotFound(new { message = $"Permission with ID {id} not found." });

        return NoContent();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { message = "Error deleting permission.", details = ex.Message });
      }
    }



  }
}
