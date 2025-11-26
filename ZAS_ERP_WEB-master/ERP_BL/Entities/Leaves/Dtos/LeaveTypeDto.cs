namespace ERP_BL.Entities.Leaves.Dtos
{
    public class LeaveTypeDto
    {
        public int Id { get; set; }
        public string LeaveTypeName { get; set; } = string.Empty;   // ✅ REQUIRED
        public string Description { get; set; } = string.Empty;
        public double MaxDaysPerYear { get; set; }
        public bool IsPaid { get; set; }
    }
}
