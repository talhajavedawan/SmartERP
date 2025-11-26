namespace ERP_BL.Entities.Leaves.Dtos
{
    public class EmployeeLeaveBalanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public int Year { get; set; }
        public double AllocatedDays { get; set; }
        public double CarriedForwardDays { get; set; }
        public double UsedDays { get; set; }
        public double RemainingDays { get; set; }

        // ✅ Extra convenience fields for UI
        public string EmployeeName { get; set; } = string.Empty;
        public string LeaveTypeName { get; set; } = string.Empty;
    }
}
