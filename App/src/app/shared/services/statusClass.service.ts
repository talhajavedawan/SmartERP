import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";

export enum TransactionItemType {
  Undefined = 1,
  Employee = 2,
  Inquiry = 3,
  SaleOrder = 4,
}

export interface StatusClass {
  id: number;
  className: string;
  isApproved: boolean;
  isActive: boolean;
  backColor?: string | null;
  foreColor?: string | null;
  statusId: number;
  statusName: string;
  transactionItemType: string;
  creationDate?: string;
  createdBy?: string;
  modifiedDate?: string;
  lastModifiedBy?: string;
}

@Injectable({ providedIn: "root" })
export class StatusClassService {
  private readonly api = `${environment.apiBaseUrl}/StatusClass`;

  constructor(private http: HttpClient) {}

  // GET
  getAll(
    type?: TransactionItemType,
    status: "all" | "active" | "inactive" = "all"
  ): Observable<StatusClass[]> {
    let params = new HttpParams().set("status", status);
    if (type != null) params = params.set("type", type.toString());
    return this.http.get<StatusClass[]>(`${this.api}/GetAll`, { params });
  }

  // GET
  getById(id: number): Observable<StatusClass> {
    return this.http.get<StatusClass>(`${this.api}/GetById/${id}`);
  }

  // POST
  create(
    type: TransactionItemType,
    dto: Partial<StatusClass>
  ): Observable<StatusClass> {
    const typeName = TransactionItemType[type];
    return this.http.post<StatusClass>(`${this.api}/${typeName}/Create`, dto);
  }

  // PUT
  update(
    type: TransactionItemType,
    id: number,
    dto: Partial<StatusClass>
  ): Observable<StatusClass> {
    const typeName = TransactionItemType[type];
    return this.http.put<StatusClass>(
      `${this.api}/${typeName}/Update/${id}`,
      dto
    );
  }

  // PATCH
  deactivate(id: number): Observable<any> {
    return this.http.patch<any>(`${this.api}/${id}/Deactivate`, {});
  }
}
