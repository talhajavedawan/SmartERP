export interface CreateZoneDto {
    name: string,
    countryIds: number[];
}


export interface Country{
    id: number,
    name: string;
}


export interface GetZone {
  id: number;
  name: string;
  countryIds: number[];
}

export interface UpdateZoneDto {
  name: string;
  countryIds: number[];
}