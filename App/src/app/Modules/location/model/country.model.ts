export interface Country {
  flag: any;
  id: number;
  name: string;
  iso2: string;
  iso3: string;
  zoneId?: number;
  zoneName?: string;
  phoneCode?: number;
}
 


export interface Zone {
  id: number;
  name: string;
}

export interface SelectCountryZoneDTO {
  id: number;
  name: string;
  iso2: string;
  iso3: string;
  zones: Zone[];
    zoneId: number; 
}
export interface CountryUpdateZoneDTO {
  countryId: number;
  zoneId: number;
}