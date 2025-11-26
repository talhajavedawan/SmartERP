import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';

import { LeaveTypeService } from '../../../../shared/services/leaves/leave-type.service';
import { LeaveRequestService } from '../../../../shared/services/leaves/leave-request.service';
import { LeaveType, LeaveRequest } from '../../../location/model/leaveapp.model';

@Component({
  selector: 'app-apply-leave',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './apply-leave.component.html',
  styleUrls: ['./apply-leave.component.css']
})
export class ApplyLeaveComponent implements OnInit {
  /** ✅ Form model for new leave */
  newLeave: LeaveRequest = {
    employeeId: 1,
    leaveTypeId: 0,
    startDate: '',
    endDate: '',
    leaveDescription: '',
    isHalfDay: false,
    totalDays: 0
  };

  /** ✅ Dropdown data */
  leaveTypes: LeaveType[] = [];

  /** ✅ UI states */
  isSubmitting = false;
  errorMessage = '';

  constructor(
    private leaveTypeService: LeaveTypeService,
    private leaveRequestService: LeaveRequestService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadLeaveTypes();

    // Load employeeId from localStorage or fallback
    const storedId = localStorage.getItem('employeeId');
    if (storedId) {
      this.newLeave.employeeId = Number(storedId);
      console.log('✅ Loaded Employee ID:', this.newLeave.employeeId);
    } else {
      console.warn('⚠️ No employeeId found in localStorage. Using default.');
      this.newLeave.employeeId = 1;
    }
  }

  /** 🔹 Load all leave types from backend */
  loadLeaveTypes(): void {
    this.leaveTypeService.getAll().subscribe({
      next: (data) => {
        this.leaveTypes = (data || []).map((t: any) => ({
          leaveTypeId: t.id,
          leaveTypeName: t.leaveTypeName,
          description: t.description
        }));
        console.log('✅ Leave Types Loaded:', this.leaveTypes);
      },
      error: (err: HttpErrorResponse) => {
        console.error('❌ Error loading leave types:', err);
        this.errorMessage = 'Failed to load leave types.';
      }
    });
  }

  /** 🔹 Automatically calculate total leave days */
  calculateDays(): void {
    if (!this.newLeave.startDate || !this.newLeave.endDate) {
      this.newLeave.totalDays = 0;
      return;
    }

    const start = new Date(this.newLeave.startDate);
    const end = new Date(this.newLeave.endDate);

    // Validation: end date before start date
    if (end < start) {
      alert('⚠️ End date cannot be earlier than start date.');
      this.newLeave.totalDays = 0;
      return;
    }

    const diffDays = (end.getTime() - start.getTime()) / (1000 * 60 * 60 * 24);
    this.newLeave.totalDays = this.newLeave.isHalfDay ? 0.5 : diffDays + 1;

    console.log('📅 Calculated Days:', this.newLeave.totalDays);
  }

  /** 🔹 Submit leave request */
  submitLeave(form: any): void {
    if (form.invalid) {
      alert('⚠️ Please fill in all required fields.');
      return;
    }

    if (!this.newLeave.employeeId || this.newLeave.employeeId <= 0) {
      alert('⚠️ Employee ID is missing. Please log in again.');
      return;
    }

    if (!this.newLeave.leaveTypeId) {
      alert('⚠️ Please select a valid leave type.');
      return;
    }

    if (!this.newLeave.startDate || !this.newLeave.endDate) {
      alert('⚠️ Please select valid start and end dates.');
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    // ✅ Prepare backend-compatible payload
    const payload = {
      employeeId: this.newLeave.employeeId,
      leaveTypeId: this.newLeave.leaveTypeId,
      applyDate: new Date().toISOString(),
      startDate: new Date(this.newLeave.startDate).toISOString(),
      endDate: new Date(this.newLeave.endDate).toISOString(),
      leaveDescription: this.newLeave.leaveDescription?.trim() || 'No description provided.',
      isHalfDay: !!this.newLeave.isHalfDay
    };

    console.log('📦 Submitting Payload:', payload);

    // ✅ Submit to backend
    this.leaveRequestService.applyLeave(payload).subscribe({
      next: (response) => {
        console.log('✅ Leave applied successfully:', response);
        alert('✅ Leave applied successfully!');
        this.isSubmitting = false;
        this.router.navigate(['/leaves/history']);
      },
      error: (err: HttpErrorResponse) => {
        console.error('❌ Error applying leave:', err);
        const backendError =
          err.error?.error ||
          err.error?.detail ||
          err.error?.message ||
          err.message;

        this.errorMessage =
          backendError || 'Failed to submit leave request. Please try again.';
        alert('⚠️ Server error: ' + this.errorMessage);
        this.isSubmitting = false;
      }
    });
  }
}
