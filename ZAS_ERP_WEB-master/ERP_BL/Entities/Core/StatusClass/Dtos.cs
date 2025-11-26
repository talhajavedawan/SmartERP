using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Core.StatusClass
{
    public class StatusClassCreateDto
    {
        [Required, MaxLength(100)]
        public string ClassName { get; set; } = null!;

        public bool IsApproved { get; set; } = false;
        public bool IsActive { get; set; } = true;

        [MaxLength(20)]
        public string? BackColor { get; set; }

        [MaxLength(20)]
        public string? ForeColor { get; set; }

        [Required]
        public int StatusId { get; set; }
    }
    public class StatusClassUpdateDto : StatusClassCreateDto
    {
        public int Id { get; set; }
    }
    public class StatusClassDto
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = null!;
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public string? BackColor { get; set; }
        public string? ForeColor { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public string TransactionItemType { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
