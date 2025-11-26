using ERP_BL.Entities.Core.StatusClass;
using ERP_BL.Enums;
using ERP_REPO.Repo.Core.StatusClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_WebAPI.Controllers.Core.StatusList
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class StatusClassController : ControllerBase
    {
        private readonly IStatusClassRepo _repo;
        public StatusClassController(IStatusClassRepo repo) => _repo = repo;

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<StatusClassDto>>> GetAll(
            [FromQuery] TransactionItemType? type = null,
            [FromQuery] string status = "all")
        {
            var result = await _repo.GetAllStatusClassesAsync(type, status);
            return result.Any() ? Ok(result) : NoContent();
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<StatusClassDto>> GetById(int id)
        {
            var data = await _repo.GetStatusClassByIdAsync(id);
            return data != null ? Ok(data) : NotFound();
        }

        [HttpPost("{type}/Create")]
        public async Task<ActionResult<StatusClassDto>> Create([FromRoute] TransactionItemType type, [FromBody] StatusClassCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _repo.AddStatusClassAsync(dto, type, User);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{type}/Update/{id}")]
        public async Task<ActionResult<StatusClassDto>> Update([FromRoute] TransactionItemType type, [FromRoute] int id, [FromBody] StatusClassUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch.");
            var updated = await _repo.UpdateStatusClassAsync(dto, type, User);
            return updated != null ? Ok(updated) : NotFound();
        }

        [HttpPatch("{id}/Deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var success = await _repo.DeactivateStatusClassAsync(id);
            return success ? Ok(new { Message = $"StatusClass {id} deactivated." }) : NotFound();
        }
    }
}
