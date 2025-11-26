import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { VendorNature } from '../../../Modules/vendor/vendor-nature/vendor-nature.model'; 
import { environment } from '../../../../environments/environment'; 

@Injectable({
  providedIn: 'root',
})
export class VendorNatureService {
  private readonly baseUrl = `${environment.apiBaseUrl}/VendorNature`;  

  constructor(private http: HttpClient) {}

getAllVendorNatures(
  status: string = 'All',
  pageNumber: number = 1,
  pageSize: number = 10,
  sortColumn?: string,
  sortDirection: 'asc' | 'desc' = 'asc',
  searchTerm?: string
): Observable<{
  success: boolean;
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  data: VendorNature[];
}> {
  let params = new HttpParams()
    .set('status', status)
    .set('pageNumber', pageNumber)
    .set('pageSize', pageSize)
    .set('sortDirection', sortDirection);

  if (sortColumn) params = params.set('sortColumn', sortColumn);
  if (searchTerm) params = params.set('searchTerm', searchTerm);

  const url = `${this.baseUrl}/GetAllVendorNatures`;
  return this.http.get<{
    success: boolean;
    totalCount: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
    data: VendorNature[];
  }>(url, { params });
}

  getVendorNatureById(id: number): Observable<VendorNature> {
    return this.http.get<VendorNature>(`${this.baseUrl}/GetVendorNatureById/${id}`);
  }

  createVendorNature(vendorNature: VendorNature): Observable<VendorNature> {
    return this.http.post<VendorNature>(`${this.baseUrl}/AddVendorNature`, vendorNature);
  }

  updateVendorNature(id: number, vendorNature: VendorNature): Observable<VendorNature> {
    return this.http.put<VendorNature>(`${this.baseUrl}/UpdateVendorNature/${id}`, vendorNature);
  }

  deactivateVendorNature(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/DeactivateVendorNature/${id}`);
  }
  
}
