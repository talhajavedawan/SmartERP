namespace ERP_BL.Entities.Leaves
{
    public enum LeaveApplicationStatus
    {
        UnderApproval = 0,
        Approved = 1,
        Rejected = 2,

        // ✅ Add these to match your logic
        Cancelled = 3,   // Employee cancels their own request
        Void = 4         // Admin force-closes the request
    }
}
