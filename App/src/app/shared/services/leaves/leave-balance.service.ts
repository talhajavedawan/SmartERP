import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment'; // ✅ Import environment

/**
 * ✅ Leave Balance Model
 * Matches your backend EmployeeLeaveBalanceDto
 */
export interface LeaveBalance {
  leaveTypeId: number;
  leaveTypeName: string;
  maxDaysPerYear: number;
  usedDays: number;
  remainingDays: number;
  year?: number;
}

/**
 * ✅ Leave Balance Service
 * Integrated with your backend LeaveApplicationsController
 */
@Injectable({ providedIn: 'root' })
export class LeaveBalanceService {
  /** ✅ Matches [Route("[controller]")] in backend: LeaveApplicationsController */
  private baseUrl = `${environment.apiBaseUrl}/LeaveApplications`;

  constructor(private http: HttpClient) {}

  /** ✅ Get balances for an employee
   * GET /LeaveApplications/balances/{employeeId}
   */
  getByEmployee(employeeId: number): Observable<LeaveBalance[]> {
    return this.http.get<LeaveBalance[]>(`${this.baseUrl}/balances/${employeeId}`);
  }
}
