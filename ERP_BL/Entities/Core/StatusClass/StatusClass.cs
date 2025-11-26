using ERP_BL.Entities.Core.Statuses;
using ERP_BL.Entities.Core.Users;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Core.StatusClass
{
    public class StatusClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string ClassName { get; set; } = null!;

        public bool IsApproved { get; set; } = false;
        public bool IsActive { get; set; } = true;

        [MaxLength(20)]
        public string? BackColor { get; set; }

        [MaxLength(20)]
        public string? ForeColor { get; set; }

        // Foreign key to Status
        public int StatusId { get; set; }

        [ForeignKey(nameof(StatusId))]
        public Status Status { get; set; } = null!;

        public TransactionItemType TransactionItemType { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public int? CreatedById { get; set; }
        [ForeignKey("CreatedById")] public User? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public int? LastModifiedById { get; set; }
        [ForeignKey("LastModifiedById")] public User? LastModifiedBy { get; set; }
    }
}
