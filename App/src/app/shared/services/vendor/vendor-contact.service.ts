import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../../environments/environment";
import {
  VendorContactCreate,
  VendorContactGet,
  VendorContactUpdate,
} from "../../../Modules/vendor/vendor-contact/vendor-contact.model";

@Injectable({
  providedIn: "root",
})
export class VendorContactService {
  private baseUrl = environment.apiBaseUrl;
  private apiVendorContact = `${this.baseUrl}/VendorContact`;

  constructor(private http: HttpClient) {}
  getAllPaged(
    pageNumber: number = 1,
    pageSize: number = 10,
    searchTerm: string = "",
    sortColumn: string = "",
    sortDirection: string = "asc",
    status: string = "All"
  ): Observable<any> {
    let params = new HttpParams()
      .set("pageNumber", pageNumber)
      .set("pageSize", pageSize)
      .set("status", status);

    if (searchTerm) params = params.set("searchTerm", searchTerm);
    if (sortColumn) params = params.set("sortColumn", sortColumn);
    if (sortDirection) params = params.set("sortDirection", sortDirection);

    return this.http.get<any>(`${this.apiVendorContact}/GetAll`, { params });
  }

  getById(id: number): Observable<VendorContactGet> {
    return this.http.get<VendorContactGet>(
      `${this.apiVendorContact}/GetById/${id}`
    );
  }

  // getByVendorId(vendorId: number): Observable<VendorContactGet[]> {
  //   return this.http.get<VendorContactGet[]>(
  //     `${this.apiVendorContact}/GetByVendor/${vendorId}`
  //   );
  // }

  create(contact: VendorContactCreate): Observable<any> {
    return this.http.post(`${this.apiVendorContact}/Create`, contact);
  }

  update(id: number, contact: VendorContactUpdate): Observable<any> {
    return this.http.put(`${this.apiVendorContact}/Update/${id}`, contact);
  }

  deactivate(id: number): Observable<any> {
    return this.http.delete(`${this.apiVendorContact}/Deactivate/${id}`);
  }

  getByVendor(vendorId: number): Observable<any> {
    return this.http.get<any>(
      `${this.apiVendorContact}/GetByVendor/${vendorId}`
    );
  }

}
