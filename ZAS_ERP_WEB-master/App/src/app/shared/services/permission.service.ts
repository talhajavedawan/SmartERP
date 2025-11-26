import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
//import { environment } from '../../../enviroments/environment.prod';
//

export interface Permission {
  id: number;
  name: string;
  description: string;
  isActive: boolean;
  creationDate: string;
  parentPermissionId?: number | null;
  children?: Permission[];
  expand?: boolean;
  level?: number;
}

export interface PaginatedPermissions {
  permissions: Permission[];
  totalCount: number;
  page: number;
  pageSize: number;
}

@Injectable({
  providedIn: "root",
})
export class PermissionService {
  private baseUrl = environment.apiBaseUrl;
  private apiUrl = `${this.baseUrl}/permission`; // 👈 now dynamic

  constructor(private http: HttpClient) {}

  getAllPermission(
    page = 1,
    pageSize = 100,
    search: string | null = null
  ): Observable<PaginatedPermissions> {
    let params = new HttpParams()
      .set("page", page.toString())
      .set("pageSize", pageSize.toString());

    if (search) {
      params = params.set("search", search);
    }

    return this.http.get<PaginatedPermissions>(`${this.apiUrl}/getall`, {
      params,
    });
  }
}
