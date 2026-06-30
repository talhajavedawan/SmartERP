using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChartofAccounts
{
    public class ChartofAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string accountName { get; set; }
        public bool isActive { get; set; } = true;
        public bool isActiveForTrialBalance { get; set; } = true;
        public COA_AccountType accountType { get; set; }
        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual ChartofAccount parent { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User user { get; set; }
        public string description { get; set; }
        public string bankAccountNo { get; set; }
        public string routingNo { get; set; }
        public string accociatedCompany { get; set; }
        public string creditCardNo { get; set; }
        public string accountNo { get; set; }
        public bool isOpeningBalance { get; set; }
        public DateTime? creationDate { get; set; }
        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        //public virtual List<JournalVoucher> journalList { get; set; }
        public virtual List<JournalTransaction> JournalTransactions { get; set; }

        public bool isDebitIncrease { get; set; }
        //public int? companyId { get; set; }
        //[ForeignKey("companyId")]
        //public virtual Company company { get; set; }
        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency Currency { get; set; }

        public string stage { get; set; }
        public bool isReApproved { get; set; }
        public DateTime? ReApprovalDate { get; set; }
        public DateTime? asOfDate { get; set; }
        public bool isVoid { get; set; }
        
        public virtual List<Department> Departments { get; set; }
        public virtual ICollection<Deduction> Deductions { get; set; }
        public virtual List<Company> Companies { get; set; }

        public virtual ICollection<AdminBillType> AdminBillTypes { get; set; }
        public virtual List<ERP_BL.Databases.Employee> Employees { get; set; }

        public DateTime? reconcilationDate { get; set; }
        public virtual List<Reconcilation> Reconcilations { get; set; }
        public double openingBalance { get; set; }
        [InverseProperty("ChartofAccounts")]
        public virtual List<ChartofAccountGroup> Groups { get; set; }

        public double manualBalanceOC { get; set; }
        public double manualBalancePKR { get; set; }
        public double BalanceOC { get; set; }
        public double BalancePKR { get; set; }


        public ChartofAccount()
        {
            Companies = new List<Company>();
            Departments = new List<Department>();
            Employees = new List<ERP_BL.Databases.Employee>();
        }
    }
}
