using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.HR
{
    public class Leave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public LeaveType LeaveType { get; set; }
        public string LeaveDes { get; set; } //leave Description
        public double LeaveDays { get; set; }
        public double LeaveHours { get; set; }//Equivalent Hours
        public double daysCarried { get; set; }//Carried Days
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime? ApplyDate { get; set; }

        public int? employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }
        public bool? isApproved { get; set; }

        public DateTime? LeaveDate { get; set; }


    }
    public class LeaveApplication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime ApplyDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? leaveId { get; set; }
        [ForeignKey("leaveId")]
        public virtual Leave leave { get; set; }
        public int? employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }
        public virtual ERP_BL.Databases.Employee initiator { get; set; }
        public virtual ERP_BL.Databases.Employee approver { get; set; }
        public virtual LeaveStatus leaveStatus { get; set; }
        public LeaveCurrentStatus currentStatus { get; set; }
        public bool isHalf { get; set; }
        public string LeaveDes { get; set; } //leave Description
        public virtual EmployeeHRInfo HRinfo { get; set; }

        public TransactionStage stage { get; set; }
        public bool isVoid { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }
        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public bool? isReApproved { get; set; }
        public DateTime? ReApprovalDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }

    }
    public class LeaveStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<LeaveApplication> Leaves { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }

    }
}
