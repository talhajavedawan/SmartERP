using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Reports;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;

namespace ERP_BL.CashFlow
{
    public class CashFlow
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string reportName { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User user { get; set; }
        public int? groupId { get; set; }
        [ForeignKey("groupId")]
        public GridReportGroup gridReportGroup { get; set; }
        [InverseProperty("Cashflows")]
        public virtual ICollection<Company> CashFlowCompanies { get; set; }

        [InverseProperty("Cashflows")]

        public virtual ICollection<Department> CashflowDepartments { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string saleOrderSettingKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string purchaseOrderSettingKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string saleinvoiceSettingKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string vendorBillSettingKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string adminBillSettingKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string stlSettingKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string paymentSettingKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string bankSettingKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string loanSettingKey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string advanceSettingKey { get; set; }
        public GridReportType gridReportType { get; set; }
        public int? titleId { get; set; }
        [ForeignKey("titleId")]
        public virtual ReportTitle Title { get; set; }


    }
}
