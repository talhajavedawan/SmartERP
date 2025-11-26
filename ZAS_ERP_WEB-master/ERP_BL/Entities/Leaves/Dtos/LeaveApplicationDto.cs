namespace ERP_BL.Entities.Leaves.Dtos
{
    public class LeaveApplicationDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }

        public DateTime ApplyDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsHalfDay { get; set; }
        public string LeaveDescription { get; set; } = string.Empty;

        public LeaveApplicationStatus Status { get; set; } = LeaveApplicationStatus.UnderApproval;
        public int? ApproverId { get; set; }

        // ✅ Convenience fields for UI (read-only display values)
        public string? EmployeeName { get; set; }    // from Employee.SystemDisplayName
        public string? ApproverName { get; set; }    // from Approver.SystemDisplayName
        public string? LeaveTypeName { get; set; }   // from LeaveType.LeaveTypeName
    }
}
