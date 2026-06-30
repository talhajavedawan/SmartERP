using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements
{
   public class AuditYearAdjustment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual ERP_BL.Databases.User user { get; set; }
        public InquiryType auditYearAdjustmentType { get; set; }

        public int company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("CompanyAuditYearAdjustments")]
        public virtual Company company { get; set; }

        public int dept_Id { get; set; }
        [ForeignKey("dept_Id ")]
        [InverseProperty("DepartmentAuditAdjustmnets")]
        public virtual Department department { get; set; }

        public int customerCompany_Id { get; set; }
        [ForeignKey("customerCompany_Id ")]
        [InverseProperty("CustomerAduitAdjustmnets")]
        public virtual CustomerCompany customerCompany { get; set; }

        public int? allocation_Id { get; set; }
        [ForeignKey("allocation_Id")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }


        //[InverseProperty("DepartmentAdusitAdjustmnets")]
        //public virtual List<Vendor> vendors { get; set; }
        public int principal_Id { get; set; }
        [ForeignKey("principal_Id")]
        public virtual Principal principal { get; set; }

        public DateTime? auditYear { get; set; }

        public double SOAmount { get; set; }

        //public int? baseCurrency_Id { get; set; }
        //[ForeignKey("baseCurrency_Id")]
        //public virtual Currency baseCurrency { get; set; }
        //public int? currency_Id { get; set; }
        //[ForeignKey("currency_Id")]
        //public virtual Currency currency { get; set; }

        public int? auditCurrency_Id { get; set; }
        [ForeignKey("auditCurrency_Id")]
        public virtual Currency auditCurrency { get; set; }

        public double comissionOC { get; set; }
        public double netComissionOC { get; set; } 
        public double comissionAudit { get; set; }
        public double netComissionAudit { get; set; }
        public double BudgetCost { get; set; }
        public double ActualCost { get; set; }
        public double SystemCost { get; set; }




        public double BudgetMargin { get; set; }
        public double ActualMargin { get; set; }
        public double SystemMargin { get; set; }
        public double BudgetCostAudit { get; set; }
        public double ActualCostAudit { get; set; }
        public double SystemCostAudit { get; set; }
        public double SOAmountAudit { get; set; }
        public double BudgetMarginAudit { get; set; }
        public double ActualMarginAudit { get; set; }
        public double SystemMarginAudit { get; set; }
        public double ExhangeRate { get; set; }
        public double revenue { get; set; }
        public double defferedIncome { get; set; }
        public double cgs { get; set; }
        public double accountReceivable { get; set; }
        public double accountPayable { get; set; }
        public double bank { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        public object GetPropertyValue(string propertyName)
        {
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }
    }
}
