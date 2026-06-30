using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.UserTasks
{
  public  class userTask
    {

        public TransactionItemType TransactionType { get; set; }
        public int PendingForApproval { get; set; }
        public int PendingForReapproval { get; set; }
        public int PendingForClosing { get; set; }
        public int Open { get; set; }
        public int Close { get; set; }
        public int Total { get; set; }
      

    }
}
