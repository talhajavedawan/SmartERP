using ERP_BL.Entities.Leaves;
using ERP_BL.Entities.Leaves.Dtos;
using ERP_REPO.Repo.Leaves;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ERP_BL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class LeaveApplicationsController : ControllerBase
    {
        private readonly ILeaveRepo _leaveRepo;

        public LeaveApplicationsController(ILeaveRepo leaveRepo)
        {
            _leaveRepo = leaveRepo;
        }

        // ✅ HEALTH CHECK
        [HttpGet("ping")]
        public IActionResult Ping() => Ok(new { message = "Leave API is working!" });

        // ✅ GET ALL LEAVE TYPES
        [HttpGet("GetLeaveTypes")]
        public async Task<ActionResult<IEnumerable<LeaveTypeDto>>> GetLeaveTypes()
        {
            var types = await _leaveRepo.GetLeaveTypesAsync();
            return Ok(types);
        }

        // ✅ SIMPLE HISTORY (NO PAGING)
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<LeaveApplicationDto>>> GetByEmployee(int employeeId)
        {
            var leaves = await _leaveRepo.GetLeaveHistoryAsync(employeeId);
            return Ok(leaves);
        }

        // ✅ PAGED + FILTER + SORT
        [HttpGet("employee/{employeeId}/paged")]
        public async Task<ActionResult<PagedResult<LeaveApplicationDto>>> GetPaged(
            int employeeId,
            int page = 1,
            int pageSize = 10,
            string? status = null,
            string? search = null,
            string? sortField = null,
            string? sortOrder = null)
        {
            var result = await _leaveRepo.GetPagedLeavesAsync(
                employeeId, page, pageSize, status, search, sortField, sortOrder
            );
            return Ok(result);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveApplicationDto>> GetById(int id)
        {
            var leave = await _leaveRepo.GetLeaveByIdAsync(id);
            return leave == null ? NotFound() : Ok(leave);
        }

        // ✅ APPLY LEAVE
        [HttpPost("ApplyLeave")]
        public async Task<ActionResult> ApplyLeave([FromBody] LeaveApplicationDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid leave data.");

            try
            {
                // 🔹 Run unified validation
                var validationError = ValidateLeave(dto);
                if (validationError != null)
                    return BadRequest(validationError);

                // 🔹 Build entity
                var entity = new LeaveApplication
                {
                    EmployeeId = dto.EmployeeId,
                    LeaveTypeId = dto.LeaveTypeId,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    LeaveDescription = string.IsNullOrWhiteSpace(dto.LeaveDescription)
                        ? "No description provided."
                        : dto.LeaveDescription,
                    IsHalfDay = dto.IsHalfDay,
                    ApplyDate = dto.ApplyDate == default ? DateTime.UtcNow : dto.ApplyDate,
                    Status = LeaveApplicationStatus.UnderApproval,
                    ApproverId = dto.ApproverId // ⚠️ May be null — check your DB constraints
                };

                // 🔹 Repository call
                var created = await _leaveRepo.ApplyLeaveAsync(entity);
                var result = await _leaveRepo.GetLeaveByIdAsync(created.Id);

                return Ok(result);
            }
            catch (DbUpdateException dbEx)
            {
                var inner = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"❌ [DB ERROR] {inner}");
                return StatusCode(500, new { error = inner });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [SERVER ERROR] {ex}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ✅ STATUS UPDATE
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            if (request == null)
                return BadRequest("Invalid status update.");

            try
            {
                switch (request.Status)
                {
                    case LeaveApplicationStatus.Approved:
                    case LeaveApplicationStatus.Rejected:
                    case LeaveApplicationStatus.UnderApproval:
                        await _leaveRepo.UpdateLeaveStatusAsync(id, request.Status, request.ApproverId, request.Remarks);
                        break;

                    case LeaveApplicationStatus.Cancelled:
                        await _leaveRepo.CancelLeaveAsync(id, request.ApproverId, request.Remarks);
                        break;

                    case LeaveApplicationStatus.Void:
                        await _leaveRepo.VoidLeaveAsync(id, request.ApproverId, request.Remarks);
                        break;

                    default:
                        return BadRequest("Unsupported status type.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [STATUS UPDATE ERROR] {ex}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ✅ GET BALANCES
        [HttpGet("balances/{employeeId}")]
        public async Task<ActionResult<IEnumerable<EmployeeLeaveBalanceDto>>> GetBalances(int employeeId)
        {
            var balances = await _leaveRepo.GetBalancesAsync(employeeId);
            return Ok(balances);
        }

        // ✅ CREATE LEAVE TYPE
        [HttpPost("types")]
        public async Task<IActionResult> CreateLeaveType([FromBody] LeaveTypeDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid type.");

            await _leaveRepo.CreateLeaveTypeAsync(dto);
            return Ok();
        }

        // ✅ UPDATE LEAVE TYPE
        [HttpPut("types/{id}")]
        public async Task<IActionResult> UpdateLeaveType(int id, [FromBody] LeaveTypeDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid type.");

            await _leaveRepo.UpdateLeaveTypeAsync(id, dto);
            return Ok();
        }

        // ✅ DELETE LEAVE TYPE
        [HttpDelete("types/{id}")]
        public async Task<IActionResult> DeleteLeaveType(int id)
        {
            await _leaveRepo.DeleteLeaveTypeAsync(id);
            return NoContent();
        }

        // 🔸 Internal validator
        private string? ValidateLeave(LeaveApplicationDto dto)
        {
            if (dto.EmployeeId <= 0) return "Invalid employee ID.";
            if (dto.LeaveTypeId <= 0) return "Invalid leave type.";
            if (dto.StartDate == default || dto.EndDate == default) return "Start and end dates are required.";
            if (dto.EndDate < dto.StartDate) return "End date cannot be earlier than start date.";
            return null;
        }

        // 🔸 Request for status updates
        public class UpdateStatusRequest
        {
            public LeaveApplicationStatus Status { get; set; }
            public int ApproverId { get; set; }
            public string Remarks { get; set; } = string.Empty;
        }
    }
}
