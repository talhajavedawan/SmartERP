using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Permission
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Added { get; set; }
        public DateTime LastModified { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Permission ParentPermission { get; set; }
        //public int? RoleId { get; set; }
        //[ForeignKey("RoleId")]
        public virtual List<Role> Roles { get; set; }
    }
    public class AccessLevel
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

    }
}

