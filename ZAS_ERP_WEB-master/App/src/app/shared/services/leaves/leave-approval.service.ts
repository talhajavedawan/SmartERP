import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment'; // ✅ Import environment

@Injectable({ providedIn: 'root' })
export class LeaveApprovalService {
  /** ✅ Matches your [Route("[controller]")] → LeaveApplicationsController */
  private baseUrl = `${environment.apiBaseUrl}/LeaveApplications`;

  constructor(private http: HttpClient) {}

  /** ✅ Approve Leave (PUT /LeaveApplications/{id}/status) */
  approve(requestId: number, approverId: number, remarks: string): Observable<any> {
    const body = { status: 'Approved', approverId, remarks };
    return this.http.put(`${this.baseUrl}/${requestId}/status`, body);
  }

  /** ✅ Reject Leave (PUT /LeaveApplications/{id}/status) */
  reject(requestId: number, approverId: number, remarks: string): Observable<any> {
    const body = { status: 'Rejected', approverId, remarks };
    return this.http.put(`${this.baseUrl}/${requestId}/status`, body);
  }

  /** ✅ Update Leave Status (Cancel / Void / UnderApproval) */
  updateStatus(requestId: number, status: string, approverId: number, remarks: string = ''): Observable<any> {
    const body = { status, approverId, remarks };
    return this.http.put(`${this.baseUrl}/${requestId}/status`, body);
  }
}
