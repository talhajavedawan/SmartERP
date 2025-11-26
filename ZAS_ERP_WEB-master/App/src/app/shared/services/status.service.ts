import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { Observable } from "rxjs";

export enum TransactionItemType {
  Undefined = 1,
  Employee = 2,
  Inquiry = 3,
  SaleOrder = 4,
}

export interface Status {
  id: number;
  statusName: string;
  isActive: boolean;
  backColor?: string | null;
  foreColor?: string | null;
  transactionItemType: string;
  creationDate?: string;
  createdBy?: string;
  modifiedDate?: string;
  lastModifiedBy?: string;
}

@Injectable({ providedIn: "root" })
export class StatusService {
  private readonly api = `${environment.apiBaseUrl}/Status`;

  constructor(private http: HttpClient) {}

  // GET /Status/GetAllStatuses?type=2&status=active
  getAllStatuses(
    type?: TransactionItemType,
    status: "all" | "active" | "inactive" = "all"
  ): Observable<Status[]> {
    let params = new HttpParams().set("status", status);
    if (type != null) params = params.set("type", type.toString());
    return this.http.get<Status[]>(`${this.api}/GetAllStatuses`, { params });
  }

  getStatusById(id: number): Observable<Status> {
    return this.http.get<Status>(`${this.api}/GetStatusById/${id}`);
  }

  // POST /Status/Employee/Create
  createStatus(
    type: TransactionItemType,
    dto: Partial<Status>
  ): Observable<Status> {
    const typeName = TransactionItemType[type];
    return this.http.post<Status>(`${this.api}/${typeName}/Create`, dto);
  }

  // PUT /Status/Employee/Update/5
  updateStatus(
    type: TransactionItemType,
    id: number,
    dto: Partial<Status>
  ): Observable<Status> {
    const typeName = TransactionItemType[type];
    return this.http.put<Status>(`${this.api}/${typeName}/Update/${id}`, dto);
  }

  // PATCH /Status/5/Deactivate
  deactivateStatus(id: number): Observable<any> {
    return this.http.patch<any>(`${this.api}/${id}/Deactivate`, {});
  }
}
