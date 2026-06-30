using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.ChartofAccounts.ViewModels
{
    public class CountersModel
    {
        public static int _ApprovalCount;
        public int ApprovalCount { get; set; }
        public static int _ReApprovalCount;
        public int ReApprovalCount { get; set; }
        public static int _VoidCount;
        public int VoidCount { get; set; }
        public int ClosingCount { get; set; }
        public static int _ApproveunapprovedCount;
        public int ApproveunapprovedCount { get; set; }



        public static CountersModel GetCountersDetails()
        {
            var header = new CountersModel()
            {
                ApprovalCount = _ApprovalCount,
                ReApprovalCount = _ReApprovalCount,
                VoidCount = _VoidCount,
                ApproveunapprovedCount = _ApproveunapprovedCount,

            };
            return header;
        }
    }
}
