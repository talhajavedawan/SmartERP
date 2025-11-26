export interface CurrencyGetDto {
  id: number;
  name: string;
  symbol: string;
  abbreviation: string;
  countryId: number | null;
  countryName: string | null; 
}


export interface CurrencyCreateDto {
  name: string;
  symbol: string;
  abbreviation: string;
  countryId: number | null;
  currencyName: string | null;
}

export interface CurrencyUpdateDto {
  id: number;
  name: string;
  symbol: string;
  abbreviation: string;
  countryId: number | null;
   currencyName: string | null;
}
export interface Country {
  id: number;
  name: string;
}
