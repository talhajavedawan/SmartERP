using ERP_BL.Entities.HRM.Employees;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Leaves
{
    public class LeaveApplication   // make public
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

        [ForeignKey("LeaveType")]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; } = default!;

        public DateTime ApplyDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsHalfDay { get; set; }

        [MaxLength(500)]
        public string LeaveDescription { get; set; } = default!;

        public LeaveApplicationStatus Status { get; set; } = default!;

        [ForeignKey("Approver")]
        public int? ApproverId { get; set; }
        public Employee? Approver { get; set; }

        // 🔹 Add Histories collection
        public ICollection<LeaveApplicationHistory> Histories { get; set; } = new List<LeaveApplicationHistory>();
    }
}
