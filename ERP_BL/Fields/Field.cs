using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Fields
{
    public class Field
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Identity { get; set; }
        public string Tag { get; set; }
        public string ElementName { get; set; }
        //public string elementType { get; set; }
        //public TransactionItemType transactionType { get; set; }

        public int? moduleField_Id { get; set; }
        [ForeignKey("moduleField_Id")]
        public virtual ModuleFields moduleField { get; set; }

        //public int? templateField_Id { get; set; }
        //[ForeignKey("templateField_Id")]
        //public TemplateFields templateField { get; set; }

        public DateTime? LastModified { get; set; }
    }

    public class ModuleFields
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TransactionItemType transactionType { get; set; }

        public int? templateId { get; set; }
        [ForeignKey("templateId")]
        public virtual Template template { get; set; }

        public virtual List<Field> fields { get; set; }

    }
    public class Template
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public TransactionItemType transactionType { get; set; }
        public COA_AccountType? Coa_AccountType { get; set; }

    }
}
