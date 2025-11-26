import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable, throwError } from "rxjs";
import { environment } from "../../../../../../environments/environment";

// -------------------- Interface --------------------
export interface JobTitle {
  id?: number;
  jobTitleName: string;
  isActive: boolean;

  createdByUserName?: string;
  createdDate?: string;
  lastModifiedByUserName?: string;
  lastModifiedDate?: string;
}

// -------------------- Service --------------------
@Injectable({ providedIn: "root" })
export class JobTitleService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  // Get All Job Titles with optional active/inactive/all filter
  getAllJobTitles(status: string = "all"): Observable<JobTitle[]> {
    return this.http
      .get<JobTitle[]>(`${this.baseUrl}/JobTitle/GetAll`, {
        params: { status },
      })
      .pipe(catchError(this.handleError));
  }

  // Get single Job Title by ID
  getJobTitleById(id: number): Observable<JobTitle> {
    return this.http
      .get<JobTitle>(`${this.baseUrl}/JobTitle/GetById/${id}`)
      .pipe(catchError(this.handleError));
  }

  // Create new Job Title
  createJobTitle(jobTitle: JobTitle): Observable<JobTitle> {
    return this.http
      .post<JobTitle>(`${this.baseUrl}/JobTitle/Create`, jobTitle)
      .pipe(catchError(this.handleError));
  }

  // Update existing Job Title
  updateJobTitle(id: number, jobTitle: JobTitle): Observable<JobTitle> {
    return this.http
      .put<JobTitle>(`${this.baseUrl}/JobTitle/Update/${id}`, jobTitle)
      .pipe(catchError(this.handleError));
  }

  // -------------------- Error Handler --------------------
  private handleError(error: any): Observable<never> {
    console.error("An error occurred:", error);
    return throwError(
      () => new Error("Something went wrong; please try again later.")
    );
  }
}
