using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChartofAccounts
{
   public class ChartofAccountGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string referenceNo { get; set; }
        public virtual List<ERP_BL.Databases.Employee> Employees { get; set; }
        public virtual List<Company> Companies { get; set; }
        public virtual List<Department> Departments { get; set; }
        [InverseProperty("Groups")]
        public virtual List<ChartofAccount> ChartofAccounts { get; set; }

        public ChartofAccountGroup()
        {
            Companies = new List<Company>();
            Departments = new List<Department>();
            Employees = new List<ERP_BL.Databases.Employee>();
        }
    }
}
