import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

// ✅ Services & Models
import { LeaveRequestService } from '../../../../shared/services/leaves/leave-request.service';
import { LeaveRequest, LeaveStatus } from '../../../location/model/leaveapp.model';

// ✅ NG-ZORRO UI imports
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzSpinModule } from 'ng-zorro-antd/spin';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';

@Component({
  selector: 'app-leave-history',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NzTableModule,
    NzSelectModule,
    NzBadgeModule,
    NzSpinModule,
    NzPaginationModule,
    NzDropDownModule,
    NzButtonModule,
    NzInputModule
  ],
  templateUrl: './leave-history.component.html',
  styleUrls: ['./leave-history.component.css']
})
export class LeaveHistoryComponent implements OnInit {
  // ✅ Data
  leaves: LeaveRequest[] = [];
  isLoading = false;

  // ✅ Filters
  searchText = '';
  statusFilter: LeaveStatus | null = null;
  statusOptions = [
    { label: 'Approved', value: 'Approved' },
    { label: 'Rejected', value: 'Rejected' },
    { label: 'Cancelled', value: 'Cancelled' },
    { label: 'UnderApproval', value: 'UnderApproval' }
  ];

  // ✅ Pagination
  page = 1;
  pageSize = 10;
  totalCount = 0;

  // ✅ Context
  employeeId = 1; // TODO: Replace with AuthService value
  selectedRow: LeaveRequest | null = null;
  canApprove = false; // Toggle by user role

  constructor(private leaveService: LeaveRequestService) {}

  ngOnInit(): void {
    this.loadLeaves();
  }

  /** ✅ Load paged & filtered leave requests */
  loadLeaves(): void {
    this.isLoading = true;

    this.leaveService
      .getPaged(
        this.employeeId, // ✅ employeeId must come first
        this.page,
        this.pageSize,
        this.statusFilter ?? undefined,
        this.searchText || undefined
      )
      .subscribe({
        next: (result: any) => {
          console.log('✅ Leaves fetched:', result);

          // 🩵 Normalize the backend result to match LeaveRequest model
          this.leaves = (result.items || []).map((l: any) => ({
            requestId: l.id,
            employeeId: l.employeeId,
            leaveTypeId: l.leaveTypeId,
            fromDate: l.startDate,
            toDate: l.endDate,
            reason: l.leaveDescription,
            isHalfDay: l.isHalfDay,
            status: l.status,
            createdDate: l.applyDate,
            approverId: l.approverId,
            approverName: l.approverName,
            leaveType: {
              leaveTypeId: l.leaveTypeId,
              name: l.leaveTypeName
            }
          }));

          this.totalCount = result.totalCount || 0;
          this.isLoading = false;
        },
        error: (err: HttpErrorResponse) => {
          console.error('❌ Error fetching leaves:', err);
          this.isLoading = false;
        }
      });
  }

  /** ✅ Refresh list manually (e.g. after new leave) */
  refreshLeaves(): void {
    console.log('🔄 Refreshing leaves...');
    this.page = 1;
    this.loadLeaves();
  }

  /** ✅ Handle page change */
  onPageChange(newPage: number): void {
    this.page = newPage;
    this.loadLeaves();
  }

  /** ✅ Filter by status */
  onFilterStatus(status: LeaveStatus | null): void {
    this.statusFilter = status;
    this.page = 1;
    this.loadLeaves();
  }

  /** ✅ Search by text */
  onSearch(text: string): void {
    this.searchText = text.trim();
    this.page = 1;
    this.loadLeaves();
  }

  /** ✅ Sorting handler (optional backend sort) */
  handleSort(sortEvent: any): void {
    const sortBy = sortEvent.key;
    const sortDir = sortEvent.value;
    console.log('Sorting by:', sortBy, sortDir);
  }

  /** ✅ Dropdown menu actions */
  onMenu(action: string): void {
    if (!this.selectedRow) return;

    switch (action) {
      case 'approve':
        this.updateStatus('Approved');
        break;
      case 'reject':
        this.updateStatus('Rejected');
        break;
      case 'copy':
        navigator.clipboard.writeText(JSON.stringify(this.selectedRow, null, 2));
        alert('Copied to clipboard!');
        break;
      case 'open':
        alert(`Viewing details for Request ID: ${this.selectedRow.requestId}`);
        break;
      case 'edit':
        alert('Edit functionality coming soon...');
        break;
    }
  }

  /** ✅ Approve / Reject leave */
  updateStatus(status: LeaveStatus): void {
    if (!this.selectedRow?.requestId) {
      alert('Invalid leave record — missing Request ID.');
      return;
    }

    const requestId = this.selectedRow.requestId;

    this.leaveService
      .updateStatus(requestId, status, 1, 'Updated from UI')
      .subscribe({
        next: () => {
          alert(`Leave ${status} successfully!`);
          this.loadLeaves(); // 🔁 Refresh table
        },
        error: (err: HttpErrorResponse) => {
          console.error('❌ Error updating status:', err);
          alert('Failed to update leave status.');
        }
      });
  }
}
