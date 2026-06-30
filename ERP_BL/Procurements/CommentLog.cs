using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ERP_BL.Databases
{
    public class CommentLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual Employee employee { get; set; }
        public int? AssigneeId { get; set; }
        [ForeignKey("AssigneeId")]
        public virtual Employee Assignee { get; set; }
        [InverseProperty("TaggedComment")]
        public virtual List<User> TaggedList { get; set; } 
        [InverseProperty("TaggedRecomededComment")]
        public virtual List<User> TaggedRecomenndedList { get; set; }
        [InverseProperty("CCRecomededComment")]
        public virtual List<User> CCRecomenndedList { get; set; }
        [InverseProperty("CCComments")]
        public virtual List<User> CCUsersList { get; set; }
        public int? ReplyCommentId { get; set; }
        [ForeignKey("ReplyCommentId")]
        public virtual CommentLog ReplyComment { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual CommentCategory Category{ get; set; }
        public DateTime Timestamp { get; set; }
        //public string Info { get; set; }
        public string Comment { get; set; }
        public string Subject { get; set; }
        public bool isReply { get; set; }
        public bool isRead { get; set; }
        public DateTime? ReadTimestamp { get; set; }

        public TransactionItemType TransactionType { get; set; }
        public int TransactionId { get; set; }
        public string billSystemRef { get; set; }
        public string poSystemRef { get; set; }
        public int? FlagId { get; set; }
        [ForeignKey("FlagId")]
        public virtual NotificationFlag notificationFlag { get; set; }

        public int? managerId { get; set; }
        [ForeignKey("managerId")]
        public virtual Employee manager { get; set; }
        public int? salesPersonId { get; set; }
        [ForeignKey("salesPersonId")]
        public virtual Employee salesPerson { get; set; }
        public int? financePersonId { get; set; }
        [ForeignKey("financePersonId")]
        public virtual Employee financePerson { get; set; }


    }
    public class CommentCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string category { get; set; }
        public string discription { get; set; }

        public bool isActive { get; set; } = true;
        public int? user_Id { get; set; }
        [ForeignKey("user_Id ")]
        public virtual User user { get; set; }
        public virtual List<TransactionItem> TransactionTypes { get; set; }

    }

    public class TransactionItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual TransactionItemType TransactionType { get; set; }
    }
}


