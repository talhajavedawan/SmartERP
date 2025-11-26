using ERP_BL.Entities.Core.Permissions.Dtos;
using ERP_BL.Entities.Core.Roles;
using ERP_BL.Entities.Core.Roles.Dtos;
using ERP_REPO.Repo.Core.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_WebAPI.Controllers.Core.Roles
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepo _roleRepo;

        public RoleController(IRoleRepo roleRepo)
        {
            _roleRepo = roleRepo;
        }

        // ✅ Get all roles
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
        {
            var roles = await _roleRepo.GetAllRolesAsync();

            var roleDtos = roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty,
                Description = r.Description,
                ParentRoleId = r.ParentRoleId,
                CreationDate = r.CreationDate,
                IsActive = r.IsActive,
                IsVoid = r.IsVoid,
                CreatedBy = r.CreatedBy,
                LastModifiedBy = r.LastModifiedBy,
                PermissionIds = r.Permissions.Select(p => p.Id).ToList(),
                Permissions = r.Permissions.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name ?? string.Empty,
                    Description = p.Description
                }).ToList()
            });

            return Ok(roleDtos);
        }

        // ✅ Get role by Id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RoleDto>> GetById(int id)
        {
            var role = await _roleRepo.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound(new { message = "Role not found." });

            var roleDto = new RoleDto
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty,
                Description = role.Description,
                ParentRoleId = role.ParentRoleId,
                IsActive = role.IsActive,
                IsVoid = role.IsVoid,
                CreatedBy = role.CreatedBy,
                LastModifiedBy = role.LastModifiedBy,
                PermissionIds = role.Permissions.Select(p => p.Id).ToList(),
                Permissions = role.Permissions.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Name = p.Name ?? string.Empty,
                    Description = p.Description
                }).ToList()
            };

            return Ok(roleDto);
        }

        // ✅ Create new role
        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create([FromBody] RoleDto roleDto)
        {
            if (string.IsNullOrWhiteSpace(roleDto.Name))
                return BadRequest(new { message = "Role name is required." });

            try
            {
                var role = new Role
                {
                    Name = roleDto.Name,
                    NormalizedName = roleDto.Name.ToUpper(),
                    Description = roleDto.Description,
                    ParentRoleId = roleDto.ParentRoleId,
                    IsActive = roleDto.IsActive,
                    IsVoid = roleDto.IsVoid,
                    CreatedBy = roleDto.CreatedBy,
                    LastModifiedBy = roleDto.LastModifiedBy
                };

                await _roleRepo.AddRoleAsync(role, roleDto.PermissionIds);

                roleDto.Id = role.Id; // return generated Id

                return CreatedAtAction(nameof(GetById), new { id = role.Id }, roleDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the role.", details = ex.Message });
            }
        }

        // ✅ Update role
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleDto roleDto)
        {
            if (id != roleDto.Id)
                return BadRequest(new { message = "Role ID mismatch." });

            try
            {
                var role = new Role
                {
                    Id = roleDto.Id,
                    Name = roleDto.Name,
                    NormalizedName = roleDto.Name.ToUpper(),
                    Description = roleDto.Description,
                    ParentRoleId = roleDto.ParentRoleId,
                    IsActive = roleDto.IsActive,
                    IsVoid = roleDto.IsVoid,
                    LastModifiedBy = roleDto.LastModifiedBy
                };

                await _roleRepo.UpdateRoleAsync(role, roleDto.PermissionIds);
                return Ok(new { message = "Role updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the role.", details = ex.Message });
            }
        }

        // ✅ Delete role
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _roleRepo.DeleteRoleAsync(id);
                return Ok(new { message = "Role deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the role.", details = ex.Message });
            }
        }
    }
}
