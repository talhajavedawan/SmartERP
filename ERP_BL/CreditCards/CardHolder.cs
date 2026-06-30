using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.CreditCards
{
    public class CardHolder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isActive { get; set; }
    }

    public class CreditCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public CardHolderType cardHolderType { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company company { get; set; }

        public virtual List<Department> departments { get; set; }

        public int? BankID { get; set; }
        [ForeignKey("BankID")]
        public virtual Bank bank { get; set; }

        public int? PrimaryCardHolderId { get; set; }
        [ForeignKey("PrimaryCardHolderId")]
        public virtual CardHolder PrimaryCardHolder { get; set; }

        public int? SecondaryCardHolderId { get; set; }
        [ForeignKey("SecondaryCardHolderId")]
        public virtual CardHolder SecondaryCardHolder { get; set; }

        public int? CardUserID { get; set; }
        [ForeignKey("CardUserID")]
        public virtual CardHolder CardUser { get; set; }

        //public bool hasPrimaryCard { get; set; }

        public int? PrimaryCardNoId { get; set; }
        [ForeignKey("PrimaryCardNoId")]
        public virtual CreditCard PrimaryCardNo { get; set; }

        public string CardNumber { get; set; }
        //public string SecondaryCardNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int CVV { get; set; }

        public int? creditCardTypeId { get; set; }
        [ForeignKey("creditCardTypeId")]
        public virtual CreditCardType creditCardType { get; set; }

        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency currency { get; set; }

        public double LimitAmount { get; set; }

        public bool isActive { get; set; }
    }

    public class CreditCardType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Type { get; set; }
    }
}
