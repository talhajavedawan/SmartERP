using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Core.Statuses.Dtos
{
    public class StatusCreateDto
    {
        [Required, MaxLength(100)] public string StatusName { get; set; } = null!;
        [MaxLength(20)] public string? BackColor { get; set; }
        [MaxLength(20)] public string? ForeColor { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
