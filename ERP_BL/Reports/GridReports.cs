using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERP_BL.Reports
{
    
   public class GridReport
    {
        //Composite key For defining user settings
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User Creater { get; set; }
        public int? group_Id { get; set; }
        [ForeignKey("group_Id")]
        public virtual GridReportGroup gridReportGroup { get; set; }
        [Key, Column(Order = 3, TypeName = "VARCHAR")]
        public string settingkey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string settingValue { get; set; }
        public DateTime lastModified { get; set; }
        //public virtual List<Department> departments { get; set; }
        public GridReportType gridReportType { get; set; }
        public string reportName { get; set; }
        public bool isFavourite { get; set; }

        public DateTime? from { get; set; }

        public DateTime? to { get; set; }

        public int? company_Id { get; set; }
        [ForeignKey("company_Id")]
        public virtual Company company { get; set; }

        public int? titleId { get; set; }
        [ForeignKey("titleId")]
        public virtual ReportTitle Title { get; set; }


    }
    public class ReportTitle
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string titleName { get; set; }
        public bool isActive { get; set; }
        public GridReportType gridReportType { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User User { get; set; }
    }
}
