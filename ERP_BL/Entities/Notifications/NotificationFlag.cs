using System.ComponentModel.DataAnnotations;

namespace ERP_BL.Entities.Notifications
{
    public class NotificationFlag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Flag { get; set; } = string.Empty;

        [MaxLength(50)]
        public string BackColor { get; set; } = "#FFFFFF";

        public bool IsActive { get; set; } = true;

        public bool CanGlow { get; set; } = false;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
