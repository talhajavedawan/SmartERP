import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, catchError, throwError } from "rxjs";
import { environment } from "../../../environments/environment";

// for listing and fetching customers
export interface CustomerDTO {
  id: number;
  companyName?: string;
  businessTypeName?: string;
  industryTypeName?: string;
  contactEmail?: string;
  contactPhone?: string;
  contactPersonName?: string;

  billingAddress?: string;
  billingZoneId?: number;
  billingCountryId?: number;
  billingStateId?: number;
  billingCityId?: number;
  billingZipCode?: number;

  shippingAddress?: string;
  shippingZoneId?: number;
  shippingCountryId?: number;
  shippingStateId?: number;
  shippingCityId?: number;
  shippingZipCode?: number;

  isActive: boolean;
  createdDate: Date;
  createdByName?: string;
  lastModifiedByName?: string;
  lastModified?: Date | null;
}

// for creating or updating a customer
export interface CustomerCreateDto {
  companyName: string;
  businessTypeId: number;
  industryTypeId: number;
  firstName: string;
  lastName: string;
  email?: string;
  phoneNumber?: string;
  billingAddressLine1: string;
  billingAddressLine2?: string;
  billingCityId?: number;
  billingStateId?: number;
  billingCountryId?: number;
  billingZoneId?: number;
  billingZipCode?: string;

  shippingAddressLine1: string;
  shippingAddressLine2?: string;
  shippingCityId?: number;
  shippingStateId?: number;
  shippingCountryId?: number;
  shippingZoneId?: number;
  shippingZipCode?: string;

  billingCityName: string;
  billingStateName: string;
  billingCountryName: string;
  shippingCityName: string;
  shippingStateName: string;
  shippingCountryName: string;

  isActive: boolean;

  createdDate?: Date | string;
  createdByName?: string;
  lastModified?: Date | string | null;
  lastModifiedByName?: string;
}

// -------------------- SERVICE --------------------
@Injectable({
  providedIn: "root",
})
export class CustomerService {
  private baseUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) {}

  // -------------------- CREATE --------------------
  createCustomer(dto: CustomerCreateDto): Observable<any> {
    return this.http
      .post(`${this.baseUrl}/Customer/create`, dto)
      .pipe(catchError(this.handleError));
  }

  // -------------------- UPDATE --------------------
  updateCustomer(id: number, dto: CustomerCreateDto): Observable<string> {
    return this.http
      .put(`${this.baseUrl}/Customer/update/${id}`, dto, {
        responseType: "text",
      })
      .pipe(catchError(this.handleError));
  }

  // -------------------- DELETE (Soft Delete) --------------------
  deleteCustomer(id: number): Observable<any> {
    return this.http
      .delete(`${this.baseUrl}/Customer/delete/${id}`)
      .pipe(catchError(this.handleError));
  }

  // -------------------- GET ALL --------------------
  getAllCustomers(status: string = "all"): Observable<CustomerDTO[]> {
    return this.http
      .get<CustomerDTO[]>(`${this.baseUrl}/Customer/GetAll`, {
        params: { status: status },
      })
      .pipe(catchError(this.handleError));
  }

  // -------------------- GET BY ID --------------------
  getCustomerById(id: number): Observable<CustomerCreateDto> {
    return this.http
      .get<CustomerCreateDto>(`${this.baseUrl}/Customer/GetById/${id}`)
      .pipe(catchError(this.handleError));
  }

  // -------------------- ERROR HANDLER --------------------
  private handleError(error: any) {
    console.error("CustomerService Error:", error);
    let errorMsg = "An unexpected error occurred.";
    if (error.error && typeof error.error === "string") errorMsg = error.error;
    else if (error.message) errorMsg = error.message;
    return throwError(() => new Error(errorMsg));
  }
}
