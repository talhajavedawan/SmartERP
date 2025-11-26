import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { environment } from "../../../../../../environments/environment";

export interface IndustryType {
  id?: number;
  industryTypeName: string;
  isActive: boolean;

  createdDate?: string;
  createdByUserName?: string | null;
  lastModifiedDate?: string;
  lastModifiedByUserName?: string | null;
}
@Injectable({ providedIn: "root" })
export class IndustryService {
  private baseUrl = environment.apiBaseUrl;
  constructor(private http: HttpClient) {}

  getAllIndustries(status: string = "all"): Observable<IndustryType[]> {
    return this.http
      .get<IndustryType[]>(`${this.baseUrl}/Industry/GetAll`, {
        params: { status },
      })
      .pipe(catchError(this.handleError));
  }
  getIndustryById(id: number): Observable<IndustryType> {
    return this.http.get<IndustryType>(
      `${this.baseUrl}/Industry/GetById/${id}`
    );
  }

  createIndustry(industry: IndustryType): Observable<IndustryType> {
    return this.http.post<IndustryType>(
      `${this.baseUrl}/Industry/Create`,
      industry
    );
  }

  updateIndustry(id: number, industry: IndustryType): Observable<IndustryType> {
    return this.http.put<IndustryType>(
      `${this.baseUrl}/Industry/Update/${id}`,
      industry
    );
  }
  private handleError(error: any): Observable<never> {
    console.error("An error occurred:", error);
    return throwError(
      () => new Error("Something went wrong; please try again later.")
    );
  }
}
