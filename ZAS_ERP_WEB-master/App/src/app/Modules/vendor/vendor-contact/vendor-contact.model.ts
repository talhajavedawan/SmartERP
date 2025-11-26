export interface VendorContactGet {
  id: number;
  designation?: string;
  isPrimary: boolean;
  vendorId: number;
  vendorName?: string;
  personId?: number;
  firstName?: string;
  lastName?: string;
  nationality?: string;
  religion?: string;
  contactId?: number;
  phoneNumber?: string;
  email?: string;
  websiteUrl?: string;
  creationDate: string;
  modifiedDate?: string;
  createdById?: number;
  createdByUserName?: string;
  lastModifiedById?: number;
  lastModifiedByUserName?: string;
}
//
export interface VendorContactCreate {
  designation?: string;
  isPrimary: boolean;
  vendorId: number;
  personId?: number;
  contactId?: number;
  firstName?: string;
  lastName?: string;
  nationality?: string;
  religion?: string;
  phoneNumber?: string;
  email?: string;
  websiteUrl?: string;
  createdById?: number;
}

export interface VendorContactUpdate extends VendorContactCreate {
  id: number;
}