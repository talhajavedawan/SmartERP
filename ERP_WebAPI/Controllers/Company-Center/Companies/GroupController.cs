using ERP_REPO.Repo.Company_Center.Companies;
using Microsoft.AspNetCore.Mvc;
using ERP_BL.Entities.Company_Center.Companies;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace ERP_WebAPI.Controllers.Company_Center.Companies
{
    [Route("[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepo _repo;

        public GroupController(IGroupRepo repo)
        {
            _repo = repo;
        }

        // ✅ Get all groups with status filter
        [HttpGet("GetAllGroups")]
        public async Task<ActionResult<IEnumerable<Group>>> GetAllGroups([FromQuery] string status = "all")
        {
            try
            {
                var groups = await _repo.GetAllGroupsAsync(status);

                if (groups == null || !groups.Any())
                    return NoContent();

                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching groups: {ex.Message}");
            }
        }

        // ✅ Get single group by ID
        [HttpGet("GetGroupById/{id}")]
        public async Task<ActionResult<Group>> GetGroupById(int id)
        {
            try
            {
                var group = await _repo.GetGroupByIdAsync(id);
                if (group == null)
                    return NotFound(new { Message = $"Group with ID {id} not found." });

                return Ok(group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching group {id}: {ex.Message}");
            }
        }

        // ✅ Create a new group
        [HttpPost("CreateGroup")]
        public async Task<ActionResult<Group>> CreateGroup([FromBody] Group group)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var created = await _repo.AddGroupAsync(group, User);
                return CreatedAtAction(nameof(GetGroupById), new { id = created.Id }, created);
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new { Message = "Database update failed", Details = dbEx.Message });
            }
            catch (InvalidOperationException opEx)
            {
                return BadRequest(new { Message = opEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the group", Details = ex.Message });
            }
        }

        // ✅ Update existing group
        [HttpPut("UpdateGroup/{id}")]
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] Group group)
        {
            if (id != group.Id)
                return BadRequest(new { Message = "Group ID mismatch." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _repo.UpdateGroupAsync(id, group, User);

                if (updated == null)
                    return NotFound(new { Message = $"Group with ID {id} not found." });

                return Ok(updated);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "The record you attempted to update was modified by another user." });
            }
            catch (InvalidOperationException opEx)
            {
                return BadRequest(new { Message = opEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while updating group {id}: {ex.Message}" });
            }
        }

        // ✅ Deactivate group
        [HttpPatch("{id}/deactivate")]
        public async Task<IActionResult> DeactivateGroup(int id)
        {
            try
            {
                var success = await _repo.DeactivateGroupAsync(id);
                if (!success)
                    return NotFound(new { Message = $"Group with ID {id} not found." });

                return Ok(new { Message = $"Group {id} has been deactivated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"An error occurred while deactivating group {id}: {ex.Message}" });
            }
        }
    }
}
