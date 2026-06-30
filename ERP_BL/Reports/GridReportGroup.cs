using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Reports
{
   public class GridReportGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string groupName { get; set; }      
        public bool isActive { get; set; } = true; 
        public virtual List<GridReport> reports { get; set; }
        public virtual List<ERP_BL.CashFlow.CashFlow> cashFlowReports { get; set; }
        public GridReportType gridReportType { get; set; }
        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual GridReportGroup parent { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User user { get; set; }

        public ReportTransactionType transactionType { get; set; }

    }
}
