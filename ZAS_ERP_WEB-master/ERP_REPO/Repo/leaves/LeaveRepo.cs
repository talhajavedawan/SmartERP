//using ERP_BL.Data;
//using ERP_BL.Entities.HRM.Employees;
//using ERP_BL.Entities.Leaves;
//using ERP_BL.Entities.Leaves.Dtos;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ERP_REPO.Repo.Leaves
//{
//    public class LeaveRepo : ILeaveRepo
//    {
//        private readonly ApplicationDbContext _context;

//        public LeaveRepo(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // ============================================
//        // ✅ Helper: Map Entity -> DTO
//        // ============================================
//        private static LeaveApplicationDto MapToDto(LeaveApplication leave) => new LeaveApplicationDto
//        {
//            Id = leave.Id,
//            EmployeeId = leave.EmployeeId,
//            LeaveTypeId = leave.LeaveTypeId,
//            ApplyDate = leave.ApplyDate,
//            StartDate = leave.StartDate,
//            EndDate = leave.EndDate,
//            IsHalfDay = leave.IsHalfDay,
//            LeaveDescription = leave.LeaveDescription,
//            Status = leave.Status,
//            ApproverId = leave.ApproverId,
//            EmployeeName = leave.Employee != null ? leave.Employee.SystemDisplayName : string.Empty,
//            ApproverName = leave.Approver != null ? leave.Approver.SystemDisplayName : string.Empty,
//            LeaveTypeName = leave.LeaveType != null ? leave.LeaveType.LeaveTypeName : string.Empty
//        };

//        // ============================================
//        // ✅ APPLY LEAVE — Fixed & Validated
//        // ============================================
//        public async Task<LeaveApplication> ApplyLeaveAsync(LeaveApplication leaveApp)
//        {
//            try
//            {
//                // 🔹 Validate employee
//                var employeeExists = await _context.Employees.AnyAsync(e => e.Id == leaveApp.EmployeeId);
//                if (!employeeExists)
//                    throw new Exception($"Employee with ID {leaveApp.EmployeeId} not found.");

//                // 🔹 Validate leave type
//                var leaveTypeExists = await _context.LeaveTypes.AnyAsync(t => t.Id == leaveApp.LeaveTypeId);
//                if (!leaveTypeExists)
//                    throw new Exception($"Leave type with ID {leaveApp.LeaveTypeId} not found.");

//                // 🔹 Validate dates
//                if (leaveApp.StartDate == default || leaveApp.EndDate == default)
//                    throw new Exception("Start and end dates are required.");
//                if (leaveApp.EndDate < leaveApp.StartDate)
//                    throw new Exception("End date cannot be earlier than start date.");

//                // 🔹 Apply defaults
//                leaveApp.ApplyDate = leaveApp.ApplyDate == default ? DateTime.UtcNow : leaveApp.ApplyDate;
//                leaveApp.Status = LeaveApplicationStatus.UnderApproval;
//                leaveApp.LeaveDescription = string.IsNullOrWhiteSpace(leaveApp.LeaveDescription)
//                    ? "No description provided."
//                    : leaveApp.LeaveDescription;

//                // ✅ Insert leave
//                _context.LeaveApplications.Add(leaveApp);
//                await _context.SaveChangesAsync();

//                // ✅ Insert history record
//                var history = new LeaveApplicationHistory
//                {
//                    LeaveApplicationId = leaveApp.Id,
//                    Status = leaveApp.Status,
//                    ChangedDate = DateTime.UtcNow,
//                    Remarks = "Leave Applied",
//                    ChangedById = leaveApp.EmployeeId
//                };

//                _context.LeaveApplicationHistories.Add(history);
//                await _context.SaveChangesAsync();

//                return leaveApp;
//            }
//            catch (DbUpdateException dbEx)
//            {
//                Console.WriteLine($"❌ Database error in ApplyLeaveAsync: {dbEx.InnerException?.Message ?? dbEx.Message}");
//                throw new Exception("Database error while saving leave. Please verify employee and leave type IDs.");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Error in ApplyLeaveAsync: {ex.Message}");
//                throw;
//            }
//        }

//        // ============================================
//        // ✅ GET LEAVE BY ID
//        // ============================================
//        public async Task<LeaveApplicationDto?> GetLeaveByIdAsync(int id)
//        {
//            var leave = await _context.LeaveApplications
//                .Include(l => l.Employee)
//                .Include(l => l.Approver)
//                .Include(l => l.LeaveType)
//                .FirstOrDefaultAsync(l => l.Id == id);

//            return leave == null ? null : MapToDto(leave);
//        }

//        // ============================================
//        // ✅ GET SIMPLE HISTORY (NO PAGING)
//        // ============================================
//        public async Task<IEnumerable<LeaveApplicationDto>> GetLeaveHistoryAsync(int employeeId)
//        {
//            var leaves = await _context.LeaveApplications
//                .Include(l => l.Employee)
//                .Include(l => l.Approver)
//                .Include(l => l.LeaveType)
//                .Where(l => l.EmployeeId == employeeId)
//                .OrderByDescending(l => l.ApplyDate)
//                .ToListAsync();

//            return leaves.Select(MapToDto);
//        }

//        // ============================================
//        // ✅ GET PAGED + FILTERED + SORTED
//        // ============================================
//        public async Task<PagedResult<LeaveApplicationDto>> GetPagedLeavesAsync(
//            int employeeId,
//            int page,
//            int pageSize,
//            string? status,
//            string? search,
//            string? sortField,
//            string? sortOrder)
//        {
//            var query = _context.LeaveApplications
//                .Include(l => l.Employee)
//                .Include(l => l.LeaveType)
//                .Where(l => l.EmployeeId == employeeId)
//                .AsQueryable();

//            // ✅ Filter by Status
//            if (!string.IsNullOrEmpty(status) && Enum.TryParse(status, out LeaveApplicationStatus parsedStatus))
//            {
//                query = query.Where(l => l.Status == parsedStatus);
//            }

//            // ✅ Search
//            if (!string.IsNullOrWhiteSpace(search))
//            {
//                search = search.ToLower();
//                query = query.Where(l =>
//                    l.LeaveDescription.ToLower().Contains(search) ||
//                    (l.LeaveType != null && l.LeaveType.LeaveTypeName.ToLower().Contains(search)) ||
//                    (l.Employee != null && l.Employee.SystemDisplayName.ToLower().Contains(search)));
//            }

//            // ✅ Sorting
//            query = (sortField, sortOrder) switch
//            {
//                ("status", "asc") => query.OrderBy(l => l.Status),
//                ("status", "desc") => query.OrderByDescending(l => l.Status),
//                ("applyDate", "asc") => query.OrderBy(l => l.ApplyDate),
//                ("applyDate", "desc") => query.OrderByDescending(l => l.ApplyDate),
//                ("startDate", "asc") => query.OrderBy(l => l.StartDate),
//                ("startDate", "desc") => query.OrderByDescending(l => l.StartDate),
//                ("endDate", "asc") => query.OrderBy(l => l.EndDate),
//                ("endDate", "desc") => query.OrderByDescending(l => l.EndDate),
//                _ => query.OrderByDescending(l => l.ApplyDate)
//            };

//            // ✅ Paging
//            var totalCount = await query.CountAsync();
//            var items = await query
//                .Skip((page - 1) * pageSize)
//                .Take(pageSize)
//                .ToListAsync();

//            return new PagedResult<LeaveApplicationDto>
//            {
//                TotalCount = totalCount,
//                Items = items.Select(MapToDto)
//            };
//        }

//        // ============================================
//        // ✅ UPDATE LEAVE STATUS
//        // ============================================
//        public async Task UpdateLeaveStatusAsync(int requestId, LeaveApplicationStatus status, int approverId, string remarks)
//        {
//            var leave = await _context.LeaveApplications
//                .Include(l => l.Employee)
//                .Include(l => l.LeaveType)
//                .FirstOrDefaultAsync(l => l.Id == requestId);

//            if (leave == null)
//                throw new Exception("Leave not found.");

//            leave.Status = status;
//            leave.ApproverId = approverId;

//            var history = new LeaveApplicationHistory
//            {
//                LeaveApplicationId = requestId,
//                Status = status,
//                ChangedDate = DateTime.UtcNow,
//                ChangedById = approverId,
//                Remarks = remarks
//            };

//            _context.LeaveApplicationHistories.Add(history);

//            // ✅ Update balances if approved
//            if (status == LeaveApplicationStatus.Approved)
//            {
//                var balance = await _context.EmployeeLeaveBalances
//                    .FirstOrDefaultAsync(b => b.EmployeeId == leave.EmployeeId && b.LeaveTypeId == leave.LeaveTypeId);

//                if (balance != null)
//                {
//                    double days = (leave.EndDate - leave.StartDate).TotalDays + 1;
//                    if (leave.IsHalfDay)
//                        days = 0.5;

//                    balance.UsedDays += days;
//                    balance.RemainingDays = (balance.AllocatedDays + balance.CarriedForwardDays) - balance.UsedDays;
//                }
//            }

//            await _context.SaveChangesAsync();
//        }

//        // ============================================
//        // ✅ CANCEL LEAVE (Employee)
//        // ============================================
//        public async Task CancelLeaveAsync(int requestId, int employeeId, string remarks)
//        {
//            var leave = await _context.LeaveApplications.FindAsync(requestId);
//            if (leave == null)
//                throw new Exception("Leave not found.");

//            leave.Status = LeaveApplicationStatus.Cancelled;

//            var history = new LeaveApplicationHistory
//            {
//                LeaveApplicationId = leave.Id,
//                Status = LeaveApplicationStatus.Cancelled,
//                ChangedDate = DateTime.UtcNow,
//                ChangedById = employeeId,
//                Remarks = remarks
//            };

//            _context.LeaveApplicationHistories.Add(history);
//            await _context.SaveChangesAsync();
//        }

//        // ============================================
//        // ✅ VOID LEAVE (Admin)
//        // ============================================
//        public async Task VoidLeaveAsync(int requestId, int adminId, string remarks)
//        {
//            var leave = await _context.LeaveApplications.FindAsync(requestId);
//            if (leave == null)
//                throw new Exception("Leave not found.");

//            leave.Status = LeaveApplicationStatus.Void;

//            var history = new LeaveApplicationHistory
//            {
//                LeaveApplicationId = leave.Id,
//                Status = LeaveApplicationStatus.Void,
//                ChangedDate = DateTime.UtcNow,
//                ChangedById = adminId,
//                Remarks = remarks
//            };

//            _context.LeaveApplicationHistories.Add(history);
//            await _context.SaveChangesAsync();
//        }

//        // ============================================
//        // ✅ GET EMPLOYEE LEAVE BALANCES
//        // ============================================
//        public async Task<IEnumerable<EmployeeLeaveBalanceDto>> GetBalancesAsync(int employeeId)
//        {
//            var balances = await _context.EmployeeLeaveBalances
//                .Include(b => b.Employee)
//                .Include(b => b.LeaveType)
//                .Where(b => b.EmployeeId == employeeId)
//                .ToListAsync();

//            return balances.Select(b => new EmployeeLeaveBalanceDto
//            {
//                Id = b.Id,
//                EmployeeId = b.EmployeeId,
//                LeaveTypeId = b.LeaveTypeId,
//                Year = b.Year,
//                AllocatedDays = b.AllocatedDays,
//                CarriedForwardDays = b.CarriedForwardDays,
//                UsedDays = b.UsedDays,
//                RemainingDays = b.RemainingDays,
//                EmployeeName = b.Employee != null ? b.Employee.SystemDisplayName : "",
//                LeaveTypeName = b.LeaveType != null ? b.LeaveType.LeaveTypeName : ""
//            });
//        }

//        // ============================================
//        // ✅ GET ALL LEAVE TYPES
//        // ============================================
//        public async Task<IEnumerable<LeaveTypeDto>> GetLeaveTypesAsync()
//        {
//            var types = await _context.LeaveTypes.ToListAsync();

//            return types.Select(t => new LeaveTypeDto
//            {
//                Id = t.Id,
//                LeaveTypeName = t.LeaveTypeName,
//                Description = t.Description,
//                MaxDaysPerYear = t.MaxDaysPerYear,
//                IsPaid = t.IsPaid
//            });
//        }

//        // ============================================
//        // ✅ CREATE LEAVE TYPE
//        // ============================================
//        public async Task CreateLeaveTypeAsync(LeaveTypeDto dto)
//        {
//            var entity = new LeaveType
//            {
//                LeaveTypeName = dto.LeaveTypeName,
//                Description = dto.Description,
//                MaxDaysPerYear = dto.MaxDaysPerYear,
//                IsPaid = dto.IsPaid
//            };

//            _context.LeaveTypes.Add(entity);
//            await _context.SaveChangesAsync();
//        }

//        // ============================================
//        // ✅ UPDATE LEAVE TYPE
//        // ============================================
//        public async Task UpdateLeaveTypeAsync(int id, LeaveTypeDto dto)
//        {
//            var entity = await _context.LeaveTypes.FindAsync(id);
//            if (entity == null)
//                throw new Exception("Leave type not found.");

//            entity.LeaveTypeName = dto.LeaveTypeName;
//            entity.Description = dto.Description;
//            entity.MaxDaysPerYear = dto.MaxDaysPerYear;
//            entity.IsPaid = dto.IsPaid;

//            await _context.SaveChangesAsync();
//        }

//        // ============================================
//        // ✅ DELETE LEAVE TYPE
//        // ============================================
//        public async Task DeleteLeaveTypeAsync(int id)
//        {
//            var entity = await _context.LeaveTypes.FindAsync(id);
//            if (entity == null)
//                throw new Exception("Leave type not found.");

//            _context.LeaveTypes.Remove(entity);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
