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

namespace ERP_BL.Procurements.Memos
{
    public class Memo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public MemoType memoType { get; set; }
        public string Subject { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? createdById { get; set; }
        [ForeignKey("createdById")]
        public virtual ERP_BL.Databases.User createdBy { get; set; }

        public int? createdForId { get; set; }
        [ForeignKey("createdForId")]
        public virtual ERP_BL.Databases.User createdFor { get; set; }

        [InverseProperty("CCMemos")]
        public virtual List<ERP_BL.Databases.User> CCUsersList { get; set; }

        public int? taskGroupId { get; set; }
        [ForeignKey("taskGroupId")]
        public virtual TaskGroups taskGroup { get; set; }

        public bool isVoid { get; set; }

        //[InverseProperty("memo")]
        //public virtual List<MessageLog> messageLogs { get; set; }
    }

    public class PerformanceIndicatorDefinition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // e.g., "Quality of Work"

        public string Description { get; set; }

        public int DisplayOrder { get; set; } // To control hierarchy

        public int Weightage { get; set; } // e.g., 0.10 for 10%

        public bool IsActive { get; set; } = true;

        public bool IsAdminType { get; set; }

        [InverseProperty("IndicatorDefinition")]
        public virtual ICollection<PerformanceIndicatorRating> Ratings { get; set; }
    }

    public class PerformanceIndicatorRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ReviewId { get; set; }
        [ForeignKey("ReviewId")]
        public virtual EmployeePerformanceReview Review { get; set; }

        public int IndicatorDefinitionId { get; set; }
        [ForeignKey("IndicatorDefinitionId")]
        public virtual PerformanceIndicatorDefinition IndicatorDefinition { get; set; }

        public int SelfRating { get; set; }
        public int SupervisorLevelOneRating { get; set; }
        public int SupervisorLevelTwoRating { get; set; }
        public int AdminRating { get; set; }
        public int ManagementRating { get; set; }

        public string Comment { get; set; }
    }


    public class EmployeePerformanceReview
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? PerformanceYear { get; set; }

        public virtual List<ERP_BL.Databases.User> HiddenChatUsers { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual ERP_BL.Databases.User Employee { get; set; }

        public int? DeptId { get; set; }
        [ForeignKey("DeptId")]
        public virtual Department EmpDepartment { get; set; }

        public int? SupervisorLevelOneId { get; set; }
        [ForeignKey("SupervisorLevelOneId")]
        public virtual ERP_BL.Databases.User SupervisorLevelOne { get; set; }

        public DateTime? SupervisorLevelOneReviewDate { get; set; }

        public int? SupervisorLevelTwoId { get; set; }
        [ForeignKey("SupervisorLevelTwoId")]
        public virtual ERP_BL.Databases.User SupervisorLevelTwo { get; set; }

        public DateTime? SupervisorLevelTwoReviewDate { get; set; }

        public int? AdminId { get; set; }
        [ForeignKey("AdminId")]
        public virtual ERP_BL.Databases.User Admin { get; set; }

        public int? ManagementId { get; set; }
        [ForeignKey("ManagementId")]
        public virtual ERP_BL.Databases.User Management { get; set; }

        public DateTime? AdminReviewDate { get; set; }

        public int? MemoId { get; set; }
        [ForeignKey("MemoId")]
        public virtual Memo Memo { get; set; }

        public string PreviousYearGoals { get; set; }
        public string NextYearGoals { get; set; }

        [InverseProperty("Review")]
        public virtual ICollection<PerformanceIndicatorRating> Ratings { get; set; }

        public PerformanceReviewerStage performanceReviewerStage { get; set; }
        public PerformanceReviewType performanceReviewType { get; set; }
    }

}
