using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace ZAS_ERP.FixedAssets.Classes
{
   public  class GroupAdjustmentRecord
    {
        public int RevId { get; set; }
        public double GroupId { get; set; }
        public double PrevGroupId { get; set; }
        public double NextGroupId { get; set; }

    }
}
