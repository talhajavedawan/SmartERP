// ======================================================
// ✅ LEAVE STATUS (shared across all models)
// ======================================================
// Ordered by lifecycle: Pending → UnderApproval → Approved/Rejected → Cancelled/Void → ReApproval
export type LeaveStatus =
  | 'Pending'
  | 'UnderApproval'
  | 'Approved'
  | 'Rejected'
  | 'Cancelled'
  | 'Void'
  | 'ReApproval';

// ======================================================
// ✅ LEAVE TYPE
// ======================================================
export interface LeaveType {
  id?: number;                      // Matches backend LeaveType.Id
  leaveTypeId?: number;             // For legacy compatibility
  leaveTypeName: string;            // Backend property
  name?: string;                    // ✅ Added for UI compatibility (Angular templates use this)
  description?: string;
  maxDaysPerYear?: number;
  isPaid?: boolean;
}

// ======================================================
// ✅ LEAVE REQUEST (Main DTO - Matches LeaveApplicationDto)
// ======================================================
export interface LeaveRequest {
  /** ✅ Primary IDs */
  id?: number;                      // Backend primary key
  requestId?: number;               // UI alias for table rows

  /** ✅ Core fields */
  employeeId: number;
  leaveTypeId: number;

  /** ✅ Dates */
  startDate: string | Date;
  endDate: string | Date;
  applyDate?: string | Date;

  /** ✅ Details */
  leaveDescription: string;
  isHalfDay?: boolean;
  totalDays?: number;

  /** ✅ Status tracking */
  status?: LeaveStatus;

  /** ✅ Relational info */
  approverId?: number;
  approverName?: string;
  employeeName?: string;
  leaveTypeName?: string;

  /** ✅ Nested reference */
  leaveType?: LeaveType;

  /** ✅ UI aliases (for older components) */
  fromDate?: string | Date;        // mapped to startDate
  toDate?: string | Date;          // mapped to endDate
  reason?: string;                 // mapped to leaveDescription
  createdDate?: string | Date;
}

// ======================================================
// ✅ PAGED RESULT (for list/history API)
// ======================================================
export interface PagedResult<T> {
  totalCount: number;
  items: T[];
}

// ======================================================
// ✅ LEAVE BALANCE (EmployeeLeaveBalanceDto)
// ======================================================
export interface LeaveBalance {
  id?: number;
  employeeId?: number;
  leaveTypeId: number;
  leaveTypeName: string;
  allocatedDays?: number;
  carriedForwardDays?: number;
  usedDays: number;
  remainingDays: number;
  year?: number;
  employeeName?: string;
}

// ======================================================
// ✅ LEAVE APPROVAL (for approval tracking / audit)
// ======================================================
export interface LeaveApproval {
  approvalId: number;
  requestId: number;  // FK to LeaveRequest.id
  approverId: number;
  approvalDate: string | Date;
  comments?: string;
  status: LeaveStatus;
}
