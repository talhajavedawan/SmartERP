using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.CustomReports
{
   public class CustomReport
    {
        //used for grid data in custom reports
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        //Enums
        public TransactionItemType transactionItemType { get; set; }
        public CustomReportFields customReportField { get; set; }
        public AccountsType accountsType { get; set; }

        //foreign key
        public int? companyId { get; set; }        [ForeignKey("companyId")]        public virtual Company company { get; set; }        public int? deptId { get; set; }        [ForeignKey("deptId")]        public virtual Department department { get; set; }

        public int? chartOfAccountId { get; set; }        [ForeignKey("chartOfAccountId")]        public virtual ChartofAccount chartofAccounts { get; set; }
    }
   public class CustomReportGeoup
        {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        public string Title { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User Creator { get; set; }

        public int? updatorId { get; set; }
        [ForeignKey("updatorId")]
        public virtual ERP_BL.Databases.User UpdatedBy { get; set; }

        public virtual List<CustomReport> customReports { get; set; }

    }
}
