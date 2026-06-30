using ERP_BL.BaseClasses;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.ToDoTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.BackgroundImages
{
    [Table("tabBackground")]
    public class BackgroundImages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Path { get; set; }
        public string EmployeeIds { get; set; }
        public string PopupText { get; set; }
        public bool isSharedAll { get; set; }
        public bool isUpdate { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User user { get; set; }
        public DateTime? UploadedTime { get; set; }
        public int? taskGroupId { get; set; }
        [ForeignKey("taskGroupId")]
        public virtual TaskGroups taskGroup { get; set; }

        public bool isGroup { get; set; }
    }
}
