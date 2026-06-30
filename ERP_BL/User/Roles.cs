using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string RoleName { get; set; }
        public int? parentId { get; set; }        [ForeignKey("parentId")]        public virtual Role parentRole { get; set; }
        public DateTime Added { get; set; }
        public DateTime LastModified { get; set; }
        public bool isActive { get; set; }
        public virtual List<Permission> Permissions { get; set; }
        public virtual List<User> Users { get; set; }
    }
    public class RoleField 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Header { get; set; }
        public string FieldName { get; set; }
    }
}
