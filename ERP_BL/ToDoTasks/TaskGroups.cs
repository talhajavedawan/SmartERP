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

namespace ERP_BL.ToDoTasks
{
    public class TaskGroups
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual TaskGroups parentGroup { get; set; }

        public DateTime creationDate { get; set; }
        public string GroupName { get; set; }

        public int? GroupCreatorId { get; set; }
        [ForeignKey("GroupCreatorId")]
        public virtual ERP_BL.Databases.User GroupCreator { get; set; }

        [InverseProperty("taskGroups")]
        public virtual List<ERP_BL.Databases.User> users { get; set; }

        //[InverseProperty("ToDoTasks")]
        public virtual List<Company> Companies { get; set; }

        //[InverseProperty("ToDoTasks")]
        public virtual List<Department> Departments { get; set; }

        //public TaskGroupTemplate groupTemplate { get; set; }
        public bool isBackground { get; set; }

        public int? payableAccount_Id { get; set; }
        [ForeignKey("payableAccount_Id")]
        public virtual ChartofAccount PayableAccount { get; set; }

        public int? cgsAccount_Id { get; set; }
        [ForeignKey("cgsAccount_Id")]
        public virtual ChartofAccount CGSAccount { get; set; }

        //public int? companyId { get; set; }
        //[ForeignKey("companyId")]
        //public virtual Company Company { get; set; }

        //public int? dept_Id { get; set; }
        //[ForeignKey("dept_Id")]
        //public virtual Department Department { get; set; }

        public int? currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency Currency { get; set; }

        public int? CalculationType_Id { get; set; }
        [ForeignKey("CalculationType_Id")]
        public virtual StatusCalculationType calculationType { get; set; }

        public int? targetGroup_Id { get; set; }
        [ForeignKey("targetGroup_Id")]
        public virtual TargetGroup targetGroup { get; set; }

        [InverseProperty("taskGroupsBulk")]
        public virtual List<ERP_BL.Databases.User> usersBulk { get; set; }

        //[InverseProperty("ToDoTasks")]
        public virtual List<Company> CompaniesBulk { get; set; }

        //[InverseProperty("ToDoTasks")]
        public virtual List<Department> DepartmentsBulk { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        public string settingValue { get; set; }

        public TargetsTransactionType targetsTransactionType { get; set; }

        public bool isTitle { get; set; }
    }
}
