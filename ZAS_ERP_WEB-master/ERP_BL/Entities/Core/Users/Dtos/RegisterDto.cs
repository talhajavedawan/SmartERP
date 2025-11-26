using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Entities.Core.Users.Dtos
{//Register
  public class RegisterDto
  {
    public string UserName { get; set; }
    public string Password { get; set; }
    public int EmployeeId { get; set; }
  }
}
