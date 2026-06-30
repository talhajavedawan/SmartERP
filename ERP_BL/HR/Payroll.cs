using ERP_BL.Databases;
using ERP_BL.Procurements.LoansAdvances;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP_BL.HR
{
    public class EmploymentSalary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public int transactionGroupId { get; set; }

        public string SystemRef { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User creator { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? departmentId { get; set; }
        [ForeignKey("departmentId")]
        public virtual Department department { get; set; }

        public int? employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }

        public double BasicSalary { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        [InverseProperty("employmentSalary")]
        public virtual List<Payroll> Payrolls { get; set; }

        public bool isActive { get; set; }
        public string Description { get; set; }

        public bool? isApproved { get; set; }
        public string stage { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }

    public class Payroll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? SalaryMonth { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.User creator { get; set; }


        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? departmentId { get; set; }
        [ForeignKey("departmentId")]
        public virtual Department department { get; set; }

        public int? employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }

        [InverseProperty("Payrolls")]
        public int? employmentSalaryId { get; set; }
        [ForeignKey("employmentSalaryId")]
        public virtual EmploymentSalary employmentSalary { get; set; }

        public virtual List<SalaryAllowance> salaryAllowances { get; set; }
        public virtual List<SalaryBonus> salaryBonuses { get; set; }
        public virtual List<SalaryDeduction> salaryDeductions { get; set; }

        public double ProvidentFund { get; set; }
        public double ReturnedLoan { get; set; }

        public int? loansAdvanceId { get; set; }
        [ForeignKey("loansAdvanceId")]
        public virtual LoansAdvance loansAdvance { get; set; }
    }

    public class SalaryAllowance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? payrollId { get; set; }
        [ForeignKey("payrollId")]
        public virtual Payroll payroll { get; set; }

        public int? allowanceId { get; set; }
        [ForeignKey("allowanceId")]
        public virtual Allowance allowance { get; set; }
        public double Amount { get; set; }
    }

    public class Allowance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
    }


    public class SalaryBonus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? payrollId { get; set; }
        [ForeignKey("payrollId")]
        public virtual Payroll payroll { get; set; }

        public int? bonusId { get; set; }
        [ForeignKey("bonusId")]
        public virtual Bonus bonus { get; set; }
        public double Amount { get; set; }
    }

    public class Bonus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }
    }

    public class SalaryDeduction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? payrollId { get; set; }
        [ForeignKey("payrollId")]
        public virtual Payroll payroll { get; set; }

        public int? deductionId { get; set; }
        [ForeignKey("deductionId")]
        public virtual SalDeduction deduction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
    }

    public class SalDeduction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
