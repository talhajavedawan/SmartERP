using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Payments.ModelViews
{
    class TargetRewardPaymentModelView
    {
        public int Id { get; set; }
        public int TargetRewardId { get; set; }
        public DateTime TRCreationDate { get; set; }
        public string currency { get; set; }
        public string UserName { get; set; }
        public string RewardType { get; set; }
        public double RewardAmount { get; set; }
        public double AmountDue { get; set; }
        public double DebitedAmount { get; set; }
        public double Total { get; set; }
        public int? transactionHolderId { get; set; }
        public DateTime holderChangeDate { get; set; }
    }
}
