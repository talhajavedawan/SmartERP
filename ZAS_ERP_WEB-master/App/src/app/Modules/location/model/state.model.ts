export interface StateDto {
  id?: number;
  name: string;
  countryId: number;
  stateCode?: string;
  cityIds: number[];
}

export interface CountryState {
  id: number;
  name: string;
}

export interface CityState {
  id: number;
  name: string;
  countryId: number;
}

export interface PagedResult<T> {
  totalCount: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  data: T[];
}
