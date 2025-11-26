import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable, pipe, throwError } from "rxjs";
import { environment } from "../../../../../../environments/environment";

export interface BusinessType {
  id?: number;
  businessTypeName: string;
  isActive: boolean;
  createdDate?: string;
  createdByUserName?: string | null;
  lastModifiedDate?: string;
  lastModifiedByUserName?: string | null;
}
@Injectable({ providedIn: "root" })
export class BusinessService {
  private baseUrl = environment.apiBaseUrl;
  constructor(private http: HttpClient) {}

  getAllBusinessTypes(status: string = "all"): Observable<BusinessType[]> {
    return this.http
      .get<BusinessType[]>(`${this.baseUrl}/Business/GetAll`, {
        params: { status },
      })
      .pipe(catchError(this.handleError));
  }
  getBusinessById(id: number): Observable<BusinessType> {
    return this.http.get<BusinessType>(
      `${this.baseUrl}/Business/GetById/${id}`
    );
  }
  createBusinessType(business: BusinessType): Observable<BusinessType> {
    return this.http.post<BusinessType>(
      `${this.baseUrl}/Business/Create`,
      business
    );
  }

  updateBusinessType(
    id: number,
    business: BusinessType
  ): Observable<BusinessType> {
    return this.http.put<BusinessType>(
      `${this.baseUrl}/Business/Update/${id}`,
      business
    );
  }
  private handleError(error: any): Observable<never> {
    console.error("An error occurred:", error);
    return throwError(
      () => new Error("Something went wrong; please try again later.")
    );
  }
}
