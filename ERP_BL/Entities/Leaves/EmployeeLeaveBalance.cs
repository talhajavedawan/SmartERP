using ERP_BL.Entities.HRM.Employees; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Leaves
{
    public class EmployeeLeaveBalance   // make public
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

        [ForeignKey("LeaveType")]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; } = default!;

        public int Year { get; set; }
        public double AllocatedDays { get; set; }
        public double CarriedForwardDays { get; set; }
        public double UsedDays { get; set; }
        public double RemainingDays { get; set; }
    }
}
