namespace ERP_BL.Entities.Leaves.Dtos
{
    public class LeaveApplicationHistoryDto
    {
        public int Id { get; set; }
        public int LeaveApplicationId { get; set; }
        public LeaveApplicationStatus Status { get; set; } = default!;
        public DateTime ChangedDate { get; set; }
        public int? ChangedById { get; set; }
        public string Remarks { get; set; } = default!;

        // ✅ For UI
        public string? ChangedByName { get; set; }
    }
}
