using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Utils
{
    
        public class DataGridItem
        {
            public int itemId { get; set; }
            public int inquiryitemId { get; set; }
            public int offeritemId { get; set; }

            public string Item_Name { get; set; }
            public string Item_Discription { get; set; }
            public string Own_Description { get; set; }
            public string UOM { get; set; }
            public double Quantity { get; set; }

            public double value1 { get; set; }
            public double value2 { get; set; }



        }
    
}
