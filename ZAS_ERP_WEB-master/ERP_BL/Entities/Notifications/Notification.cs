using ERP_BL.Entities.Core.Users;
using ERP_BL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Notifications
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Info { get; set; }

        // Transaction details
        public int TransactionId { get; set; }

        public TransactionItemType TransactionType { get; set; } = TransactionItemType.Undefined;

        // User relationships
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public int? SendingUserId { get; set; }

        [ForeignKey("SendingUserId")]
        public virtual User? SendingUser { get; set; }

        public int? CcUserId { get; set; }

        [ForeignKey("CcUserId")]
        public virtual User? CcUser { get; set; }

        // Notification state
        public bool IsRead { get; set; } = false;

        public bool IsPending { get; set; } = false;

        public bool Glow { get; set; } = false;

        // Timestamps
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public DateTime? ReadTimestamp { get; set; }

        // Reference numbers
        [MaxLength(100)]
        public string? BillReferenceNo { get; set; }

        [MaxLength(100)]
        public string? PoReferenceNo { get; set; }

        // Flag relationship
        public int? FlagId { get; set; }

        [ForeignKey("FlagId")]
        public virtual NotificationFlag? NotificationFlag { get; set; }

        // Comment reference
        public int? CommentLogId { get; set; }
    }
}
