import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";

// DTO for Employee in Department response
export interface DepartmentEmployee {
  id: number;
  systemDisplayName: string;
}

// DTO for Company in Department response
export interface DepartmentCompany {
  id: number;
  companyName: string;
}
export interface CompanyEmployeeDto {
  companyId: number;
  employeeId: number;
}
// Department interface aligned with backend response
export interface Department {
  id: number;
  deptName: string;
  deptCode: string;
  abbreviation?: string;
  isSubsidiary: boolean;
  parentDepartmentId?: number | null;
  createdByUserName?: string | null;
  createdDate: string;
  lastModifiedByUserName?: string | null;
  lastModifiedDate?: string | null;
  isActive: boolean;
  employees: DepartmentEmployee[];
  companies: DepartmentCompany[];
  children?: Department[];
}

// Request interface for creating/updating departments
export interface DepartmentRequest {
  department: Department;
  employeeIds?: number[];
  companyIds?: number[];
}

@Injectable({
  providedIn: "root",
})
export class DepartmentService {
  private baseUrl = `${environment.apiBaseUrl}/Department`;

  constructor(private http: HttpClient) {}
  // fetch employees by department
  getEmployeesByDepartment(
    departmentId: number
  ): Observable<DepartmentEmployee[]> {
    return this.http.get<DepartmentEmployee[]>(
      `${this.baseUrl}/${departmentId}/employees`
    );
  }
  // Get all departments
  getDepartments(status: string = "all"): Observable<Department[]> {
    return this.http.get<Department[]>(`${this.baseUrl}/GetAll`, {
      params: { status },
    });
  }

  // Get department by ID
  getDepartmentById(id: number): Observable<Department> {
    return this.http.get<Department>(`${this.baseUrl}/GetById/${id}`);
  }

  // Create new department
  createDepartment(
    department: Department,
    employeeIds?: number[],
    companyIds?: number[]
  ): Observable<Department> {
    const request: DepartmentRequest = {
      department,
      employeeIds,
      companyIds,
    };
    return this.http.post<Department>(`${this.baseUrl}/Create`, request);
  }

  // Update department
  updateDepartment(
    id: number,
    department: Department,
    employeeIds?: number[],
    companyIds?: number[]
  ): Observable<Department> {
    const request: DepartmentRequest = {
      department,
      employeeIds,
      companyIds,
    };
    return this.http.put<Department>(`${this.baseUrl}/Update/${id}`, request);
  }

  assignEmployeesToCompanies(
    companyIds: number[],
    employeeIds: number[]
  ): Observable<any> {
    return this.http.post(`${this.baseUrl}/assign-employees-to-companies`, {
      companyIds,
      employeeIds,
    });
  }
}
