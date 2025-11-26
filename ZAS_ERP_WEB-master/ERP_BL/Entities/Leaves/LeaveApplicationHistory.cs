using ERP_BL.Entities.HRM.Employees;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Leaves
{
    public class LeaveApplicationHistory   // make public
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("LeaveApplication")]
        public int LeaveApplicationId { get; set; }
        public LeaveApplication LeaveApplication { get; set; } = default!;

        public LeaveApplicationStatus Status { get; set; } = default!;
        public DateTime ChangedDate { get; set; }

        [ForeignKey("ChangedBy")]
        public int? ChangedById { get; set; }
        public Employee? ChangedBy { get; set; }

        [MaxLength(1000)]
        public string Remarks { get; set; } = default!;
    }
}
