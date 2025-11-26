using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Core.Statuses.Dtos
{
    public class StatusDto
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public bool IsActive { get; set; }
        public string BackColor { get; set; }
        public string ForeColor { get; set; }
        public string TransactionItemType { get; set; } // string for enum name
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}
