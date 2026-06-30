using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.Enums;

namespace ERP_BL.Databases
{
    public class OfficialAuth
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AuthName { get; set; }
        public virtual Region region { get; set; }
        public bool isActive { get; set; }
        public List<AuthDoc> authDocs { get; set; }
        public OfficialAuthType officialAuthtype { get; set; }
    }

    public class AuthDoc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string DocName { get; set; }
        public bool isMust { get; set; }
        public string palcedAt { get; set; }
        public bool isAttached { get; set; }
        public string uplaodLocation { get; set; }
        public int authId { get; set; }
        [ForeignKey("authId")]
        public virtual OfficialAuth OfficialAuth{ get; set; }
    }

    public class Region
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}
