import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment'; // ✅ Import environment
import { LeaveRequest } from '../../../Modules/location/model/leaveapp.model';

@Injectable({ providedIn: 'root' })
export class LeaveHistoryService {
  /** ✅ Base URL matches your [Route("[controller]")] in backend */
  private baseUrl = `${environment.apiBaseUrl}/LeaveApplications`;

  constructor(private http: HttpClient) {}

  /** ✅ Get all leaves for an employee (GET /LeaveApplications/employee/{employeeId}) */
  getByEmployee(employeeId: number): Observable<LeaveRequest[]> {
    return this.http.get<LeaveRequest[]>(`${this.baseUrl}/employee/${employeeId}`);
  }

  /** ✅ Get a single leave detail (GET /LeaveApplications/{id}) */
  getById(id: number): Observable<LeaveRequest> {
    return this.http.get<LeaveRequest>(`${this.baseUrl}/${id}`);
  }
}
