import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { LeaveBalanceService, LeaveBalance } from '../../../../shared/services/leaves/leave-balance.service';

@Component({
  selector: 'app-leave-balance',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './leave-balance.component.html',
  styleUrls: ['./leave-balance.component.css'],
})
export class LeaveBalanceComponent implements OnInit {
  /** ✅ Data */
  balances: LeaveBalance[] = [];

  /** ✅ State */
  isLoading = false;
  errorMessage = '';

  /** ✅ Placeholder employee (replace with Auth later) */
  employeeId = 1;

  constructor(private balanceService: LeaveBalanceService) {}

  ngOnInit(): void {
    this.loadBalances();
  }

  /** ✅ Fetch balances from backend */
  loadBalances(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.balanceService.getByEmployee(this.employeeId).subscribe({
      next: (data: LeaveBalance[]) => {
        this.balances = data || [];
        this.isLoading = false;
      },
      error: (err: HttpErrorResponse) => {
        console.error('❌ Error fetching balances:', err);
        this.errorMessage = 'Failed to load leave balances. Please try again.';
        this.isLoading = false;
      },
    });
  }

  /** ✅ Calculate remaining total (for footer summary) */
  getTotalRemaining(): number {
    return this.balances.reduce((sum, b) => sum + (b.remainingDays || 0), 0);
  }

  /** ✅ Calculate total used days */
  getTotalUsed(): number {
    return this.balances.reduce((sum, b) => sum + (b.usedDays || 0), 0);
  }
}
