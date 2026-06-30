using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.User
{
    public class Poll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public PollingType pollingType { get; set; }

        public string Title { get; set; }

        public int? taskGroupId { get; set; }
        [ForeignKey("taskGroupId")]
        public virtual TaskGroups taskGroup { get; set; }

        public int? initiatedById { get; set; }
        [ForeignKey("initiatedById")]
        public virtual ERP_BL.Databases.User initiatedBy { get; set; }

        public DateTime? ValidUntil { get; set; }

        [InverseProperty("pollsInFavor")]
        public virtual List< ERP_BL.Databases.User> usersInFavor { get; set; }

        [InverseProperty("pollsNotInFavor")]
        public virtual List<ERP_BL.Databases.User> usersNotInFavor { get; set; }
    }

    //public class UsersPolls
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }

    //    public int? UserId { get; set; }
    //    [ForeignKey("UserId")]
    //    public virtual ERP_BL.Databases.User user { get; set; }

    //    public int? PollId { get; set; }
    //    [ForeignKey("PollId")]
    //    public virtual Poll poll { get; set; }

    //    public bool? InFavor { get; set; }
    //}
}
