import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { Permission } from "./permission.service";

// ✅ Role Interface
export interface Role {
  id: number;
  name: string;
  description?: string;
  parentRoleId?: number;
  parentRole?: Role;
  createdBy?: string;
  creationDate?: string | null;
  lastModifiedBy?: string;
  lastModified?: string | null;
  isActive: boolean;
  isVoid: boolean;
  permissions?: Permission[];
  children?: Role[];
}

// ✅ Standard API Response Interface
export interface ApiResponse {
  message: string;
  roleId?: number; // present only when creating a new role
}

@Injectable({
  providedIn: "root",
})
export class RoleService {
  private baseUrl = environment.apiBaseUrl;
  private apiUrl = `${this.baseUrl}/role`;

  constructor(private http: HttpClient) {}

  // ✅ Get all roles
  getAll(): Observable<Role[]> {
    return this.http.get<Role[]>(`${this.apiUrl}/getall`);
  }

  // ✅ Get a role by numeric ID
  getById(id: number): Observable<Role> {
    return this.http.get<Role>(`${this.apiUrl}/${id}`);
  }

  // ✅ Create a new role
  create(role: Role): Observable<ApiResponse> {
    return this.http.post<ApiResponse>(this.apiUrl, role);
  }

  // ✅ Update an existing role
  update(id: number, role: Role): Observable<ApiResponse> {
    return this.http.put<ApiResponse>(`${this.apiUrl}/${id}`, role);
  }

  // ✅ Delete a role by ID
  delete(id: number): Observable<ApiResponse> {
    return this.http.delete<ApiResponse>(`${this.apiUrl}/${id}`);
  }
}
