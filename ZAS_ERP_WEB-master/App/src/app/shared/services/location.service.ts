import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { map, Observable } from "rxjs";
//import { environment } from '../../../enviroments/environment.prod';

import {
  Country,
  CountryUpdateZoneDTO,
  SelectCountryZoneDTO,
} from "../../Modules/location/model/country.model";

import {
  CreateZoneDto,
  GetZone,
  UpdateZoneDto,
} from "../../Modules/location/model/zone.model";

import {
  CreateCityDto,
  GetCityDto,
  GetStateDto,
  PagedResult1,
  UpdateCityDto,
} from "../../Modules/location/model/city.model";
import { environment } from "../../../environments/environment";

@Injectable({
  providedIn: "root",
})
export class LocationService {
  private baseUrl = environment.apiBaseUrl;

  private countryApi = `${this.baseUrl}/Country`;
  private stateApi = `${this.baseUrl}/State`;
  private cityApi = `${this.baseUrl}/City`;
  private zoneApi = `${this.baseUrl}/Zone`;

  constructor(private http: HttpClient) {}

  // ===============================
  // ✅ COUNTRY APIs
  // ===============================
  getAll(): Observable<Country[]> {
    return this.http.get<Country[]>(`${this.countryApi}/GetAll`);
  }

  getById(id: number): Observable<SelectCountryZoneDTO> {
    return this.http.get<SelectCountryZoneDTO>(
      `${this.countryApi}/GetById/${id}`
    );
  }

  updateCountryZone(model: CountryUpdateZoneDTO): Observable<void> {
    return this.http.put<void>(`${this.countryApi}/UpdateCountryZone`, model);
  }

  // ===============================
  // ✅ ZONE APIs
  // ===============================
  createZone(model: CreateZoneDto): Observable<any> {
    return this.http.post(`${this.zoneApi}/Create`, model);
  }

  getAllZones(): Observable<GetZone[]> {
    return this.http.get<GetZone[]>(`${this.zoneApi}/GetAll`);
  }

getZoneById(id: number): Observable<any> {
  return this.http.get<any>(`${this.zoneApi}/GetById/${id}`).pipe(
    map((res: any) => res?.data ?? res)
  );
}


  updateZone(id: number, model: UpdateZoneDto): Observable<any> {
    return this.http.put(`${this.zoneApi}/Update/${id}`, model);
  }

  deleteZone(id: number): Observable<any> {
    return this.http.delete(`${this.zoneApi}/Delete/${id}`);
  }

  // ===============================

  // ✅ STATE APIs
  // ===============================
  getAllStates(search = "", page = 1, pageSize = 20): Observable<any> {
    return this.http.get(`${this.stateApi}/GetAllStates`, {
      params: { search, page, pageSize },
    });
  }

  getStateById(id: number): Observable<any> {
    return this.http.get(`${this.stateApi}/GetStateById/${id}`);
  }

  createState(state: any): Observable<any> {
    return this.http.post(`${this.stateApi}/CreateState`, state);
  }

  updateState(id: number, state: any): Observable<any> {
    return this.http.put(`${this.stateApi}/UpdateState/${id}`, state);
  }

  deleteState(id: number): Observable<any> {
    return this.http.delete(`${this.stateApi}/DeleteState/${id}`);
  }

getCountries(): Observable<any[]> {
  return this.http.get<any[]>(`${this.countryApi}/GetAll`).pipe(
    map((res) =>
      (res || []).map((c: any) => ({
        id: c.id ?? c.Id,
        name: c.name ?? c.Name
      }))
    )
  );
}



  searchCities(search: string, page: number, size: number): Observable<any> {
    const params = {
      search,
      page: page.toString(),
      pageSize: size.toString(), // ✅ Correct key: pageSize not size
    };

    return this.http.get(`${this.stateApi}/cities/search`, { params });
  }

  // ===============================
  // ✅ CITY APIs
  // ===============================
  getCities(
    page = 1,
    pageSize = 20,
    search = ""
  ): Observable<PagedResult1<GetCityDto>> {
    let params = new HttpParams()

      .set("PageNumber", String(page))
      .set("PageSize", String(pageSize));

    if (search) params = params.set("Search", search);

    return this.http.get<PagedResult1<GetCityDto>>(
      `${this.cityApi}/GetAllCities`,
      { params }
    );
  }

  getStates(search = ""): Observable<GetStateDto[]> {
    let params = new HttpParams();

    if (search) params = params.set("search", search);
    return this.http.get<GetStateDto[]>(`${this.cityApi}/GetStatesDropdown`, {
      params,
    });
  }

  createCity(dto: CreateCityDto) {
    return this.http.post(`${this.cityApi}/CreateCity`, dto);
  }

  updateCity(id: number, dto: UpdateCityDto) {
    return this.http.put(`${this.cityApi}/UpdateCity/${id}`, dto);
  }

  deleteCity(id: number) {
    return this.http.delete(`${this.cityApi}/DeleteCity/${id}`);
  }
}
