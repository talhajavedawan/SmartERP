using ERP_BL.Entities.Core.Statuses.Dtos;
using ERP_BL.Enums;
using ERP_REPO.Repo.Core.Statuses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_WebAPI.Controllers.Core.Statuses
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepo _repo;

        public StatusController(IStatusRepo repo) => _repo = repo;

        // GET: /Status/GetAllStatuses?type=Employee&status=active
        [HttpGet("GetAllStatuses")]
        public async Task<ActionResult<IEnumerable<StatusDto>>> GetAllStatuses(
            [FromQuery] TransactionItemType? type = null,
            [FromQuery] string status = "all")
        {
            var result = await _repo.GetAllStatusesAsync(type, status);
            return result?.Any() == true ? Ok(result) : NoContent();
        }

        // GET: /Status/GetStatusById/5
        [HttpGet("GetStatusById/{id}")]
        public async Task<ActionResult<StatusDto>> GetStatusById(int id)
        {
            var status = await _repo.GetStatusByIdAsync(id);
            return status != null
                ? Ok(status)
                : NotFound(new { Message = $"Status with ID {id} not found." });
        }

        // POST: /Status/Employee/Create
        [HttpPost("{type}/Create")]
        public async Task<ActionResult<StatusDto>> CreateStatus(
            [FromRoute] TransactionItemType type,
            [FromBody] StatusCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _repo.AddStatusAsync(dto, type, User);
            return CreatedAtAction(nameof(GetStatusById), new { id = created.Id }, created);
        }

        // PUT: /Status/Employee/Update/5
        [HttpPut("{type}/Update/{id}")]
        public async Task<ActionResult<StatusDto>> UpdateStatus(
            [FromRoute] TransactionItemType type,
            [FromRoute] int id,
            [FromBody] StatusUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { Message = "ID mismatch." });

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _repo.UpdateStatusAsync(dto, type, User);
            return updated != null
                ? Ok(updated)
                : NotFound(new { Message = $"Status with ID {id} not found." });
        }

        // PATCH: /Status/5/Deactivate
        [HttpPatch("{id}/Deactivate")]
        public async Task<IActionResult> DeactivateStatus(int id)
        {
            var success = await _repo.DeactivateStatusAsync(id);
            return success
                ? Ok(new { Message = $"Status {id} has been deactivated." })
                : NotFound(new { Message = $"Status with ID {id} not found." });
        }
    }
}