import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';
import { VendorGetDto, VendorUpdateDto } from '../../../Modules/vendor/vendor-center/vendor.model';

@Injectable({
  providedIn: 'root'
})
export class VendorService {
  private readonly baseUrl = `${environment.apiBaseUrl}/Vendor`;

  constructor(private http: HttpClient) {}

  getAllVendors(
    status: string = 'All',
    pageNumber: number = 1,
    pageSize: number = 10,
    sortColumn: string = '',
    sortDirection: string = 'asc',
    searchTerm: string = ''
  ): Observable<any> {
    const params = new HttpParams()
      .set('status', status)
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('sortColumn', sortColumn)
      .set('sortDirection', sortDirection)
      .set('searchTerm', searchTerm);

    return this.http
      .get<any>(`${this.baseUrl}/GetAllVendor`, { params })
      .pipe(catchError(this.handleError));
  }

getVendorById(id: number): Observable<any> {
  return this.http
    .get<any>(`${this.baseUrl}/GetVendorById/${id}`)
    .pipe(catchError(this.handleError));
}

  createVendor(vendor: any) {
    return this.http.post(`${this.baseUrl}/CreateVendor`, vendor).pipe(
      catchError((error) => {
        let userMessage = 'An unexpected error occurred.';

        if (error.status === 409 && error.error?.message) {
          userMessage = error.error.message;
        }

        return throwError(() => ({
          status: error.status,
          message: userMessage
        }));
      })
    );
  }

  updateVendor(id: number, vendor: VendorUpdateDto): Observable<any> {
    return this.http
      .put(`${this.baseUrl}/UpdateVendor/${id}`, vendor)
      .pipe(catchError(this.handleError));
  }

  deactivateVendor(id: number): Observable<any> {
    return this.http
      .delete(`${this.baseUrl}/Deactivate/${id}`)
      .pipe(catchError(this.handleError));
  }

  getSelectableCompanies(): Observable<any> {
    return this.http
      .get<any>(`${this.baseUrl}/GetSelectableCompanies`)
      .pipe(catchError(this.handleError));
  }

  getDepartmentsByCompany(companyId: number): Observable<any> {
    return this.http
      .get<any>(`${this.baseUrl}/GetDepartmentsByCompany/${companyId}`)
      .pipe(catchError(this.handleError));
  }

  getVendorDropdown() {
  return this.http.get<any>(`${this.baseUrl}/GetDropdown`);
}

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Client Error: ${error.error.message}`;
    } else {
      switch (error.status) {
        case 404:
          errorMessage = `Error: Resource not found (404).`;
          break;
        case 500:
          errorMessage = `Error: Server issue (500). Please try again later.`;
          break;
        case 409:
          errorMessage = `Conflict error (409): ${error.error?.message || 'Conflict occurred during the operation'}`;
          break;
        default:
          errorMessage = `Server returned code ${error.status}, message: ${error.error?.message || error.message}`;
          break;
      }
    }

    console.error('VendorService Error:', errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
