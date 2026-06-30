using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Reports
{
   public class SharedGridGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string groupName { get; set; }
        public virtual List<ERP_BL.Databases.Employee> Employees { get; set; }
        public virtual List<Department> Departments { get; set; }
        public virtual List<Company> Companies { get; set; }
        public virtual List<SharedReport> SharedReports { get; set; }
        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual SharedGridGroup Parent { get; set; }
        public bool isVoid { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User User { get; set; }
        public SharedGridGroup()
        {
            Companies = new List<Company>();
            Departments = new List<Department>();
            Employees = new List<ERP_BL.Databases.Employee>();
        }
    }
    public class SharedReport
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User Creater { get; set; }
        [Key, Column(Order = 3, TypeName = "VARCHAR")]
        public string settingkey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string settingValue { get; set; }
        public DateTime lastModified { get; set; }
        public string reportName { get; set; }
        public int? sharedGroupId { get; set; }
        [ForeignKey("sharedGroupId")]
        public virtual SharedGridGroup SharedGridGroup { get; set; }
    }
}
