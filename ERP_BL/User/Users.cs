using ERP_BL.Procurements;
using ERP_BL.Procurements.Memos;
using ERP_BL.ToDoTasks;
using ERP_BL.ToDoTasks.Taskss;
using ERP_BL.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public int employeeId { get; set; }
        [ForeignKey("employeeId")]
        public virtual Employee employee { get; set; }
        //public int userSettingId { get; set; }
        //[ForeignKey("userSettingId")]
        public virtual List<UserSettings> userSettings { get; set; }
        public virtual List<Report> userReports { get; set; }

        [InverseProperty("TaggedList")]

        public virtual List<CommentLog> TaggedComment { get; set; }
        [InverseProperty("TaggedRecomenndedList")]
        public virtual List<CommentLog> TaggedRecomededComment { get; set; }

        [InverseProperty("CCRecomenndedList")]
        public virtual List<CommentLog> CCRecomededComment { get; set; }
        [InverseProperty("CCUsersList")]

        public virtual List<CommentLog> CCComments { get; set; }

        [InverseProperty("CCUsersList")]
        public virtual List<Memo> CCMemos { get; set; }
        public virtual IEnumerable<Notification> ReceivedNotifications { get; set; }
        public virtual IEnumerable<Notification> SentNotifications { get; set; }

        public virtual List<Role> Roles { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(60)]
        [Index(IsUnique = true)]
        public string userName { get; set; }
        public string password { get; set; }
        public bool isActive { get; set; }
        public bool isLoggedIn { get; set; }
        public bool isKeyApproved { get; set; }
        public string machineKey { get; set; }

        public bool isRDCKeyApproved { get; set; }
        public string rdcMachineKey { get; set; }

        [InverseProperty("createdFor")]
        public virtual List<Memo> Memos { get; set; }
       
        //[InverseProperty("users")]
        public virtual ICollection<TaskGroups> taskGroups { get; set; }

        public virtual ICollection<TaskGroups> taskGroupsBulk { get; set; }

        [InverseProperty("assignedToUsers")]
        public virtual ICollection<ToDoTask> ToDoTasks { get; set; }

        public DateTime? LoginTime { get; set; }

        public virtual List<LoginUserDetails> loginUserDetails { get; set; }

        [InverseProperty("AllowedUsers")]
        public virtual ICollection<Tasks> Tasks { get; set; }
        public bool isBlink { get; set; }

        [InverseProperty("usersInFavor")]
        public virtual List<Poll> pollsInFavor { get; set; }

        [InverseProperty("usersNotInFavor")]
        public virtual List<Poll> pollsNotInFavor { get; set; }
    }
    public class UserSettings
    {
        //Composite key For defining user settings
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Key, Column(Order = 2)]

        [Required]
        public int userId { get; set; }
        [ForeignKey("userId")]
        public virtual User user { get; set; }

        [Key, Column(Order = 3, TypeName = "VARCHAR")]
        public string settingkey { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string settingValue { get; set; }
        public DateTime lastModified { get; set; }

    }
}
