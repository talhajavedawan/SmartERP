using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Databases;

namespace ERP_BL.Databases
{
    public class Report
    {
        
        //Composite key For defining user settings
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //public List<int> userIds { get; set; }
        //[ForeignKey("userIds")]
        public virtual List<User> Users { get; set; }
        [Required, Index(IsUnique = true), Column(TypeName = "VARCHAR")]

        public string ReportName { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        public string ReportDesign { get; set; }

        public string Type { get; set; }

        public DateTime lastModified { get; set; }
        public int? groupId { get; set; }
        [ForeignKey("groupId")]
        public virtual ReportGroup reportGroup { get; set; }
    }
    //public class  UserReports
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }
    //    public int userId { get; set; }
    //    [ForeignKey("userId")]
    //    public User user { get; set; }
    //    public int reportId { get; set; }
    //    [ForeignKey("reportId")]
    //    public Report Report { get; set; }
    //}
    public class ReportGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string group { get; set; }
        //public string discription { get; set; }
        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual ReportGroup parentGroup { get; set; }
        public bool isActive { get; set; } = true;
        public int? user_Id { get; set; }
        [ForeignKey("user_Id ")]
        public virtual User user { get; set; }
        public virtual List<Report> reports { get; set; }

    }
}
