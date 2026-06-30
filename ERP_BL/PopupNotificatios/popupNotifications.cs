using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.PopupNotificatios
{
    [Table("tabPopupNotifications")]
    public class popupNotifications
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleColorCode { get; set; }
        public string popupText { get; set; }
        public string TextColorCode { get; set; }
        public string EmployeeIds { get; set; }
        public double FontSize { get; set; }
        public  string fontWeight { get; set; }
        public string Italic { get; set; }

        public double FontSizeHeading { get; set; }
        public string fontWeightHeading { get; set; }
        public string ItalicHeading { get; set; }

    } 
}
