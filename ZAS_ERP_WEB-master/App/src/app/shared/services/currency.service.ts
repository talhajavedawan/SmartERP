import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { CurrencyCreateDto, CurrencyGetDto, CurrencyUpdateDto } from '../../Modules/location/model/currency.model';

@Injectable({
  providedIn: 'root',
})
export class CurrencyService {
  private apiUrl = `${environment.apiBaseUrl}/Currency`;  

  constructor(private http: HttpClient) {}

getAllCurrencies(
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
  data: CurrencyGetDto[];
}> {
  let params = new HttpParams()
    .set('status', status)
    .set('pageNumber', pageNumber)
    .set('pageSize', pageSize)
    .set('sortDirection', sortDirection);

  if (sortColumn) params = params.set('sortColumn', sortColumn);
  if (searchTerm) params = params.set('searchTerm', searchTerm);

  const url = `${this.apiUrl}/GetAllCurrencies`;
  return this.http.get<{
    success: boolean;
    totalCount: number;
    pageNumber: number;
    pageSize: number;
    totalPages: number;
    data: CurrencyGetDto[];
  }>(url, { params });
}

  getCurrencyById(id: number): Observable<CurrencyGetDto> {
    const url = `${this.apiUrl}/GetCurrencyById/${id}`;
    return this.http.get<CurrencyGetDto>(url);
  }

  createCurrency(dto: CurrencyCreateDto): Observable<any> {
    const url = `${this.apiUrl}/CreateCurrency`;
    return this.http.post(url, dto, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }

  updateCurrency(id: number, dto: CurrencyUpdateDto): Observable<any> {
    const url = `${this.apiUrl}/UpdateCurrency/${id}`;
    return this.http.put(url, dto, {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    });
  }

  private handleError(error: any): Observable<never> {
    console.error(error); 
    throw new Error('Something went wrong!');
  }
}
