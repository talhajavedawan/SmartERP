// src/app/Modules/vendor/vendor-center/vendor.model.ts

export interface VendorGetDto {
  id: number;
  ntn: string;
  companyName?: string;
  businessTypeName?: string;
  industryTypeName?: string;
  registrationNumber?: string | null;
  contactEmail?: string;
  contactPhone?: string;
  contactWebsiteUrl?: string;

  billingAddress?: string;
  billingZipcode?: string;
  shippingAddress?: string;
  shippingZipcode?: string;

  isActive: boolean;
  createdDate: string;
  createdByName?: string;
  lastModifiedByName?: string;
  lastModified?: string | null;

  redList: boolean;
  ranking: number;
  parentVendorId?: number;
  parentVendorName?: string;
  isSubsidiary: boolean;
  vendorNature?: string;
  currency?: string;
  industryTypeId?: number;
  businessTypeId?: number;
  vendorNatureId?: number;
  currencyId?: number;
  // Address IDs (needed for edit mode cascading)
  shippingZoneId?: number;
  shippingCountryId?: number;
  shippingStateId?: number;
  shippingCityId?: number;
  clientCompanyIds?: number[];
  clientCompanyNames?: string[];

  billingZoneId?: number;
  billingCountryId?: number;
  billingStateId?: number;
  billingCityId?: number;

  departmentIds?: number[];
  departmentNames?: string[];
}

export interface VendorCreateDto {
  VendorName: string;
  Ntn?: string | null;
  IndustryTypeId?: number | null;
  BusinessTypeId?: number | null;
  RegistrationNumber?: string | null;
  ContactPersonId?: number | null;

  ShippingAddressId?: number | null;
  BillingAddressId?: number | null;

  ShippingAddressLine1: string;
  ShippingAddressLine2?: string | null;
  ShippingZipcode: string;
  ShippingCountryId?: number | null;
  ShippingStateId?: number | null;
  ShippingCityId?: number | null;
  ShippingZoneId?: number | null;

  BillingAddressLine1: string;
  BillingAddressLine2?: string | null;
  BillingZipcode: string;
  BillingCountryId?: number | null;
  BillingStateId?: number | null;
  BillingCityId?: number | null;
  BillingZoneId?: number | null;

  ParentVendorId?: number | null;

  Ranking: number;
  RedList: boolean;
  IsActive: boolean;
  IsSubsidiary: boolean;
  VendorNatureId?: number | null;
  CurrencyId?: number | null;

  ContactEmail: string;
  ContactPhone: string;
  ContactWebsite?: string | null;
    ClientCompanyIds?: number[];
  DepartmentIds?: number[];
}

export interface VendorUpdateDto extends VendorCreateDto {
  Id: number;
}


export interface AddressDto {
  id?: number;
  addressLine1?: string;
  addressLine2?: string;
  zipcode?: string;
  country?: CountryDto;
  state?: StateDto;
  city?: CityDto;
  zone?: ZoneDto;
  addressType?: number;
}

export interface CountryDto {
  id: number;
  name: string;
  iso2?: string;
  iso3?: string;
  phoneCode?: number;
  zoneId?: number;
}

export interface StateDto {
  id: number;
  name: string;
  stateCode?: string;
}

export interface CityDto {
  id: number;
  name: string;
}

export interface ZoneDto {
  id: number;
  name: string;
}


export interface ContactDto {
  id?: number;
  email?: string;
  phoneNumber?: string;      // ← API uses `phoneNumber`
  websiteUrl?: string;
  emergencyPhoneNumber?: string;
  whatsAppNumber?: string;
  fax?: string;
  linkedIn?: string;
}


export interface VendorNode extends VendorGetDto {
  isExpanded?: boolean;
  level?: number;
  children?: VendorNode[];
}

export interface VendorGetDto {

  vendorNatureId?: number;
  vendorNatureName?: string;  // ← Fixed typo

}