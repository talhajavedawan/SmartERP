// import { Injectable } from '@angular/core';
// import { HttpClient, HttpParams } from '@angular/common/http';
// import { Observable, throwError } from 'rxjs';
// import { catchError } from 'rxjs/operators';
// import { environment } from '../../../../environments/environment';

// // ✅ Import shared models
// import {
//   LeaveRequest,
//   PagedResult
// } from '../../../Modules/location/model/leaveapp.model';

// /** 🔹 DTO for applying leave (lightweight payload for backend POST) */
// type ApplyLeaveDto = Pick<
//   LeaveRequest,
//   'employeeId' | 'leaveTypeId' | 'startDate' | 'endDate' | 'leaveDescription' | 'isHalfDay'
// > & {
//   applyDate: string;
// };

// @Injectable({ providedIn: 'root' })
// export class LeaveRequestService {
//   /** ✅ Matches backend [Route("[controller]")] = 'LeaveApplications' */
//   private baseUrl = `${environment.apiBaseUrl}/LeaveApplications`;

//   constructor(private http: HttpClient) {}

//   // ======================================================
//   // ✅ 1. APPLY LEAVE — uses simplified DTO
//   // ======================================================
//   /** POST /LeaveApplications/ApplyLeave */
//   applyLeave(request: ApplyLeaveDto): Observable<any> {
//     // Dynamically pull employeeId if missing
//     if (!request.employeeId) {
//       const storedId = localStorage.getItem('employeeId');
//       if (storedId) {
//         request.employeeId = Number(storedId);
//         console.log(`🧠 Loaded employeeId=${request.employeeId} from localStorage`);
//       } else {
//         console.warn('⚠️ No employeeId found in localStorage or payload!');
//       }
//     }

//     console.log('📤 Posting leave to:', `${this.baseUrl}/ApplyLeave`);
//     console.log('📦 Payload:', request);

//     return this.http.post(`${this.baseUrl}/ApplyLeave`, request).pipe(
//       catchError((err) => {
//         console.error('❌ Error while applying leave:', err);
//         return throwError(() => err);
//       })
//     );
//   }

//   // ======================================================
//   // ✅ 2. GET LEAVE HISTORY BY EMPLOYEE
//   // ======================================================
//   /** GET /LeaveApplications/employee/{employeeId} */
//   getByEmployee(employeeId?: number): Observable<LeaveRequest[]> {
//     const id = employeeId || Number(localStorage.getItem('employeeId')) || 0;
//     if (!id) console.warn('⚠️ No employeeId found when calling getByEmployee.');

//     return this.http
//       .get<LeaveRequest[]>(`${this.baseUrl}/employee/${id}`)
//       .pipe(catchError((err) => throwError(() => err)));
//   }

//   // ======================================================
//   // ✅ 3. GET SINGLE LEAVE BY ID
//   // ======================================================
//   /** GET /LeaveApplications/{id} */
//   getById(id: number): Observable<LeaveRequest> {
//     return this.http
//       .get<LeaveRequest>(`${this.baseUrl}/${id}`)
//       .pipe(catchError((err) => throwError(() => err)));
//   }

//   // ======================================================
//   // ✅ 4. UPDATE STATUS
//   // ======================================================
//   /** PUT /LeaveApplications/{id}/status */
//   updateStatus(
//     requestId: number,
//     status: string,
//     approverId: number,
//     remarks: string = ''
//   ): Observable<any> {
//     const body = { status, approverId, remarks };
//     return this.http
//       .put(`${this.baseUrl}/${requestId}/status`, body)
//       .pipe(catchError((err) => throwError(() => err)));
//   }

//   // ======================================================
//   // ✅ 5. PAGED RESULTS
//   // ======================================================
//   /** GET /LeaveApplications/employee/{employeeId}/paged */
//   getPaged(
//     employeeId?: number,
//     page: number = 1,
//     pageSize: number = 10,
//     status?: string,
//     search?: string,
//     sortField?: string,
//     sortOrder?: string
//   ): Observable<PagedResult<LeaveRequest>> {
//     const id = employeeId || Number(localStorage.getItem('employeeId')) || 0;

//     let params = new HttpParams()
//       .set('page', page)
//       .set('pageSize', pageSize);

//     if (status) params = params.set('status', status);
//     if (search) params = params.set('search', search);
//     if (sortField) params = params.set('sortField', sortField);
//     if (sortOrder) params = params.set('sortOrder', sortOrder);

//     return this.http
//       .get<PagedResult<LeaveRequest>>(`${this.baseUrl}/employee/${id}/paged`, { params })
//       .pipe(catchError((err) => throwError(() => err)));
//   }

//   // ======================================================
//   // ✅ 6. BALANCES
//   // ======================================================
//   /** GET /LeaveApplications/balances/{employeeId} */
//   getBalances(employeeId?: number): Observable<any> {
//     const id = employeeId || Number(localStorage.getItem('employeeId')) || 0;
//     return this.http
//       .get(`${this.baseUrl}/balances/${id}`)
//       .pipe(catchError((err) => throwError(() => err)));
//   }
// }

import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../../environments/environment";

// 🧩 Model Interface (inside same file)
export interface LeaveRequest {
  id?: number;
  employeeId: number;
  leaveTypeId: number;
  applyDate: string;
  startDate: string;
  endDate: string;
  isHalfDay: boolean;
  leaveDescription: string;
  status?: string;
  approverId?: number;

  // Optional fields for display
  employeeName?: string;
  approverName?: string;
  leaveTypeName?: string;
}

// 🧠 Service
@Injectable({
  providedIn: "root",
})
export class LeaveRequestService {
  private readonly baseUrl = `${environment.apiBaseUrl}/LeaveApplications`;

  constructor(private http: HttpClient) {}

  /** ✅ Get all leaves by employee */
  getByEmployee(employeeId: number): Observable<LeaveRequest[]> {
    return this.http.get<LeaveRequest[]>(
      `${this.baseUrl}/employee/${employeeId}`
    );
  }

  /** ✅ Get paged leaves (optional filter) */
  getPagedLeaves(
    employeeId: number,
    page: number,
    pageSize: number,
    status?: string,
    search?: string,
    sortField?: string,
    sortOrder?: string
  ): Observable<any> {
    return this.http.get(`${this.baseUrl}/employee/${employeeId}/paged`, {
      params: {
        page,
        pageSize,
        status: status || "",
        search: search || "",
        sortField: sortField || "",
        sortOrder: sortOrder || "",
      },
    });
  }

  /** ✅ Apply for leave */
  applyLeave(data: LeaveRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}/ApplyLeave`, data);
  }

  /** ✅ Update leave status */
  updateStatus(
    leaveId: number,
    status: string,
    approverId: number,
    remarks: string
  ): Observable<any> {
    return this.http.put(`${this.baseUrl}/${leaveId}/status`, {
      status,
      approverId,
      remarks,
    });
  }

  /** ✅ Get balances */
  getBalances(employeeId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/balances/${employeeId}`);
  }

  /** ✅ Leave Types */
  getLeaveTypes(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/GetLeaveTypes`);
  }

  /** ✅ Create new leave type */
  createLeaveType(type: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/types`, type);
  }

  /** ✅ Update existing leave type */
  updateLeaveType(id: number, type: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/types/${id}`, type);
  }

  /** ✅ Delete leave type */
  deleteLeaveType(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/types/${id}`);
  }
}
