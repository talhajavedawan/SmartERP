using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.HR
{
    public class EmployeeHRInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       
        public virtual List<LeaveApplication> EmployeeLeaves { get; set; }
        //double annualL
        //public int? employeeId { get; set; }
        //[ForeignKey("employeeId")]
        //public virtual ERP_BL.Databases.Employee employee { get; set; }
    }
}
