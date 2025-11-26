using ERP_BL.Entities.Core.PowerUsers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP_BL.Entities.Core.Users
{
    public class UserSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Foreign key to User
        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;



        // Foreign key to PowerUsers (nullable)
        public int? PowerUserId { get; set; }

        [ForeignKey(nameof(PowerUserId))]
        public virtual PowerUser? PowerUser { get; set; }


        // Key to identify which setting this is (e.g., "emplyeeGrid")
        //[Required]
        [MaxLength(200)]
        public string SettingKey { get; set; } = string.Empty;

        // JSON value that stores the grid layout
        //[Required]
        public string SettingValue { get; set; } = string.Empty;

        // Last modified timestamp
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }
}
