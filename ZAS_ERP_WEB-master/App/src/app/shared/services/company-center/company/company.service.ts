import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { catchError, map, Observable, of } from "rxjs";
import { environment } from "../../../../../environments/environment";
export interface IndustryType {
  id?: number;
  industryTypeName: string;
  isActive: boolean;

  createdBy: string;
  creationDate: string;
  lastModifiedBy?: string;
  lastModified?: string;
}
export interface CompanyType {
  id: number;
  name: string;
}

export interface BusinessType {
  id?: number;
  businessTypeName: string;
  isActive: boolean;

  createdBy?: string;

  creationDate?: string;
  lastModifiedBy?: string;
  lastModified?: string;
}

export interface CreateZone {
  id: number;
  zoneName: string;
}
export interface CompanyList {
  id: number;
  companyName: string;
}
export interface DepartmentEmployee {
  id: number;
  systemDisplayName: string;
}

export interface Department {
  id: number;
  deptName: string;
  deptCode: string;
  abbreviation?: string;
  isSubsidiary: boolean;
  parentDepartmentId?: number;
  creationDate: string;
  modificationDate?: string;
  isActive: boolean;
  employees: DepartmentEmployee[];
  companies: CompanyList[]; // Use CompanyList instead of DepartmentCompany
}

export interface DepartmentRequest {
  department: Department;
  employeeIds?: number[];
  companyIds?: number[];
}
@Injectable({ providedIn: "root" })
export class CompanyService {
  private baseUrl = environment.apiBaseUrl;

  registrationData = {
    companyDetails: {},
    address: {},
    contact: {},
  };
  message: any;

  constructor(private http: HttpClient) {}

  // ---------------- Industry CRUD ----------------
  getAllIndustries(): Observable<IndustryType[]> {
    return this.http.get<IndustryType[]>(`${this.baseUrl}/Industry/GetAll`);
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

  // ---------------- Business CRUD ----------------
  getAllBusinessTypes(): Observable<BusinessType[]> {
    return this.http.get<BusinessType[]>(`${this.baseUrl}/Business/GetAll`);
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

  // ---------------- Company CRUD ----------------

  createCompany(payload: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/Company/Create`, payload);
  }

  updateCompany(companyId: number, payload: any): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/Company/Update/${companyId}`,
      payload
    );
  }

  deleteCompany(companyId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Company/Delete/${companyId}`);
  }

  getCompanyById(companyId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/Company/GetById/${companyId}`);
  }

  // getAllCompanies(status: string = "all"): Observable<CompanyList[]> {
  //   return this.http.get<CompanyList[]>(
  //     `${this.baseUrl}/Company/GetAll?status=${status}`
  //   );
  // }

  // getAllCompanies(status: string = "all"): Observable<CompanyList[]> {
  //   const validStatus = ["all", "active", "inactive"].includes(
  //     status.toLowerCase()
  //   )
  //     ? status.toLowerCase()
  //     : "all";

  //   return this.http
  //     .get<CompanyList[]>(
  //       `${this.baseUrl}/Company/GetAll?status=${validStatus}`
  //     )
  //     .pipe(
  //       map((companies) => (Array.isArray(companies) ? companies : [])),
  //       catchError((err) => {
  //         console.error("Failed to fetch companies:", err);
  //         this.message?.error?.("Failed to load companies.");
  //         return of([]);
  //       })
  //     );
  // }

  getAllCompanies(status: string = "all"): Observable<CompanyList[]> {
    const validStatus = ["all", "active", "inactive"].includes(
      status.toLowerCase()
    )
      ? status.toLowerCase()
      : "all";

    return this.http
      .get<any>(`${this.baseUrl}/Company/GetAll?status=${validStatus}`)
      .pipe(
        map((response) => {
          const companies = Array.isArray(response)
            ? response
            : response?.$values || [];
          return Array.isArray(companies) ? companies : [];
        }),
        catchError((err) => {
          console.error("Failed to fetch companies:", err);
          this.message?.error?.("Failed to load companies.");
          return of([]);
        })
      );
  }

  getBusinessTypes(): Observable<any> {
    return this.http.get(`${this.baseUrl}/Company/GetBusinessTypes`);
  }

  // ---------------- Location APIs ----------------
  getZones(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Location/zones`);
  }

  getCountriesByZone(zoneId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Location/countries/${zoneId}`);
  }

  getStatesByCountry(countryId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Location/states/${countryId}`);
  }

  getCitiesByState(stateId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Location/cities/${stateId}`);
  }
  // ---------------- Company Type APIs ----------------

  // Reset registration data
  reset() {
    this.registrationData = {
      companyDetails: {},
      address: {},
      contact: {},
    };
  }
}
