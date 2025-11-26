import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { RoleService } from "./roles.service";



export interface ChangePasswordRequest  {
  oldPassword: string;
  newPassword: string;

}



@Injectable({
  providedIn: "root",
})
export class UserService {
  private baseUrl = environment.apiBaseUrl;
  private apiUser = `${this.baseUrl}/user`;
  private apiEmployee = `${this.baseUrl}/employee`;

  constructor(private http: HttpClient, private roleService: RoleService) {}

  getUsers(status: string = "all"): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUser}/getall`, {
      params: { status },
    });
  }

  getUserRoles(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUser}/GetUserRoles/${userId}`);
  }

  getUserById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUser}/getbyid/${id}`);
  }

  createUser(user: any): Observable<any> {
    return this.http.post<any>(`${this.apiUser}/create`, {
      ...user,
      roles: user.roles || [],
    });
  }

  updateUser(id: number, user: any): Observable<any> {
    return this.http.put<any>(`${this.apiUser}/Update/${id}`, user);
  }

  voidUser(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUser}/void/${id}`);
  }

  // --------------------- EMPLOYEE METHODS (NUMBER IDs) ---------------------
  getAvailableEmployees(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiEmployee}/getavailableemployees`);
  }

  getAvailableEmployeesForEdit(userId: number): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.apiEmployee}/getavailableemployeesforedit/${userId}`
    );
  }

  getEmployees(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiEmployee}/getall`);
  }

  getEmployeeById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiEmployee}/getbyid/${id}`);
  }

  createEmployee(employee: any): Observable<any> {
    return this.http.post<any>(`${this.apiEmployee}/create`, employee);
  }

  updateEmployee(id: number, employee: any): Observable<any> {
    return this.http.put<any>(`${this.apiEmployee}/update/${id}`, employee);
  }

  voidEmployee(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiEmployee}/void/${id}`);
  }


  getCurrentUser(): Observable<any> {
    return this.http.get<any>(`${this.apiUser}/GetCurrentUser`);
  }

changePassword(payload: ChangePasswordRequest): Observable<any> {
  return this.http.post<any>(`${this.apiUser}/ChangePassword`, payload);
}


}
