using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;
using System.ComponentModel;

namespace ERP_BL.Databases
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int? CcUserId { get; set; }
        [ForeignKey("CcUserId")]
        public virtual User CcUser { get; set; }

        public int? SendingUserId { get; set; }
        [ForeignKey("SendingUserId")]
        public virtual User SendingUser { get; set; }
        public DateTime Timestamp { get; set; }
        public string Title { get; set; }

        public string Info { get; set; }
        public bool isRead{ get; set; }
        public DateTime ReadTimestamp { get; set; }
        public string Description{ get; set; }
        public int NotificationType { get; set; }

        public TransactionItemType TransactionType { get; set; }
        public int TransactionId { get; set; }
        public string BillReferenceNo { get; set; }
        public string PoReferenceNo { get; set; }

        public int? FlagId { get; set; }
        [ForeignKey("FlagId")]
        public virtual NotificationFlag notificationFlag { get; set; }

        public bool Glow { get; set; }

        public int? commentLogId { get; set; }
        public bool isPending { get; set; }
    }


    public class NotificationFlag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Flag { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<Notification> notifications { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool canGlow { get; set; }
    }
}
