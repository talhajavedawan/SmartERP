import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment'; // ✅ Import environment

/**
 * ✅ LEAVE TYPE MODEL
 * Matches your backend LeaveTypeDto
 */
export interface LeaveType {
  leaveTypeId: number;
  name: string;
  defaultAllowedDays: number;
  carryForwardAllowed: boolean;
}

/**
 * ✅ LEAVE TYPE SERVICE
 * Integrated with your backend LeaveApplicationsController
 */
@Injectable({ providedIn: 'root' })
export class LeaveTypeService {
  /** ✅ Base URL from environment — clean, not hardcoded */
  private baseUrl = `${environment.apiBaseUrl}/LeaveApplications`;

  constructor(private http: HttpClient) {}

  /** ✅ Get all leave types (GET /LeaveApplications/GetLeaveTypes) */
  getAll(): Observable<LeaveType[]> {
    return this.http.get<LeaveType[]>(`${this.baseUrl}/GetLeaveTypes`);
  }

  /** ✅ Create a new leave type (POST /LeaveApplications/types) */
  create(type: LeaveType): Observable<any> {
    return this.http.post(`${this.baseUrl}/types`, type);
  }

  /** ✅ Update an existing leave type (PUT /LeaveApplications/types/{id}) */
  update(id: number, type: LeaveType): Observable<any> {
    return this.http.put(`${this.baseUrl}/types/${id}`, type);
  }

  /** ✅ Delete a leave type (DELETE /LeaveApplications/types/{id}) */
  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/types/${id}`);
  }
}
