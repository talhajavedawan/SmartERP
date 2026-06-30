using ERP_BL.ChartofAccounts;
using ERP_BL.Enums;
using ERP_BL.Procurements.Inventories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string item { get; set; }
        [Column(TypeName = "VARCHAR")]
        [MaxLength(32)]
        [Index(IsUnique = true)]
        public string code { get; set; }
        //public string nature { get; set; }
        public int nature_Id { get; set; }
        [ForeignKey("nature_Id ")]
        public virtual ProductNature nature { get; set; }
        public int unitOfMeasureId { get; set; }
        [ForeignKey("unitOfMeasureId")]
        public virtual UnitOfMeasure unitOfMeasure { get; set; }
        public int categoryId { get; set; }
        [ForeignKey("categoryId")]
        public virtual ProductCategory category { get; set; }
        public string itemDescription { get; set; }
        public string ownDescription { get; set; }
        public bool isActive { get; set; } = true;
        public bool isTitle { get; set; } 
        public virtual List<Department> departments { get; set; }
        public int? user_Id { get; set; }
        [ForeignKey("user_Id ")]
        public virtual User user { get; set; }
        public ProductType productType { get; set; }
       
        public int? incomeAccount_id { get; set; }
        [ForeignKey("incomeAccount_id")]
        public virtual ChartofAccount incomeAccount { get; set; }
        public virtual ICollection<JournalTransaction> journalTransactions { get; set; }
        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual Product parent { get; set; }
        public int? cgsAccount_id { get; set; }
        [ForeignKey("cgsAccount_id")]
        public virtual ChartofAccount cgsAccount { get; set; }

        public int? company_id { get; set; }
        [ForeignKey("company_id")]
        public virtual Company company { get; set; }

        public virtual ICollection<Inventory> Inventories { get; set; }
        public int? cgsInvenAccount_id { get; set; }
        [ForeignKey("cgsInvenAccount_id")]
        public virtual ChartofAccount cgsInvenAccount { get; set; }

        public int? cAssetAccount_id { get; set; }
        [ForeignKey("cAssetAccount_id")]
        public virtual ChartofAccount cAssetAccount { get; set; }
        public virtual ICollection<BookerStatementItem> BookerInventories { get; set; }



        public Product()
        {
          
        }


    }
    public class ProductNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string nature { get; set; }

        public bool isActive { get; set; } = true;
        public int? user_Id { get; set; }
        [ForeignKey("user_Id ")]
        public virtual User user { get; set; }

    }
    public class UnitOfMeasure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string unitOfMeasure { get; set; }
        public bool isActive { get; set; } = true;
        public int? user_Id { get; set; }
        [ForeignKey("user_Id ")]
        public virtual User user { get; set; }

    }
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string category { get; set; }
        public string discription { get; set; }
        public int? parentId { get; set; }
        [ForeignKey("parentId")]
        public virtual ProductCategory parentCategory { get; set; }
        public bool isActive { get; set; } = true;
        public int? user_Id { get; set; }
        [ForeignKey("user_Id ")]
        public virtual User user { get; set; }
        public virtual List<Product> products { get; set; }

    }
}
