using ERP_BL.Entities.HRM.Employees;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Core.Users
{


    public class User : IdentityUser<int>
    {
        public bool IsActive { get; set; } = true;
        public bool IsLoggedIn { get; set; } = false;

        // Relationship with Employee
        public int? EmployeeId { get; set; }
        public bool IsVoid { get; set; } = false;
        [ValidateNever]
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }


    }

}

