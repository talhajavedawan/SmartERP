using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Core.Statuses.Dtos
{
    public class StatusUpdateDto : StatusCreateDto
    {
        public int Id { get; set; }
    }
}
