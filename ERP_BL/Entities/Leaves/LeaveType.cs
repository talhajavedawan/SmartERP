using System.ComponentModel.DataAnnotations;

namespace ERP_BL.Entities.Leaves
{
    public class LeaveType   // make public
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        // 🔹 Match the name used in DbContext
        public string LeaveTypeName { get; set; } = default!;

        [MaxLength(500)]
        public string Description { get; set; } = default!;

        public double MaxDaysPerYear { get; set; }
        public bool IsPaid { get; set; }

        // Navigation
        public ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();
        public ICollection<EmployeeLeaveBalance> EmployeeLeaveBalances { get; set; } = new List<EmployeeLeaveBalance>();
    }
}
