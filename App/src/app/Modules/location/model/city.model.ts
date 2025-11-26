export interface GetCityDto {
  id: number;
  name: string;
  stateId: number;
  stateName?: string;
}

export interface CreateCityDto {
  name: string;
  stateId: number;
}

export interface UpdateCityDto {
  id: number;
  name: string;
  stateId: number;
}

export interface CityState {
  id: number;
  name: string;
}

export interface PaginationParams {
  PageNumber: number;
  PageSize: number;
  Search?: string;
}

export interface PagedResult1<T> {
  items: T[];
  totalCount: number;
  currentPage: number;
  pageSize: number;
  totalPages: number;
}
export interface GetStateDto {
  id: number;
  name: string;
}