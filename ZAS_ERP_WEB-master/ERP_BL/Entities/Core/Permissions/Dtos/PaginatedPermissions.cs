using ERP_BL.Entities.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Core.Permissions.Dtos
{//dtos
  public class PaginatedPermissions
  {
    public List<Permission> Permissions { get; set; }
    public int TotalCount { get; set; }
  }
}
