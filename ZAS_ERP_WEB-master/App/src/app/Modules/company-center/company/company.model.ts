interface TreeNode extends Company {
  children?: TreeNode[];
  isExpanded?: boolean;
  hasChildren?: boolean;
  level?: number;
}

interface Location {
  id: number;
  name: string;
}

interface Company {
  id: number;
  companyName: string;
  ntn: string;
  businessTypeId?: number | null;
  industryTypeId?: number | null;
  isActive: boolean;
  isSubsidiary?: boolean;
  parentCompanyId?: number | null;
  address?: {
    region: string;
    country: string;
    zipcode: string;
    city: string;
    addressLine1: string;
    addressLine2?: string;
  };
  contact?: {
    phoneNumber: string;
    email: string;
    websiteUrl: string;
  };
  businessType?:
    | {
        id: number;
        businessTypeName: string;
      }
    | undefined;
  industryType?:
    | {
        id: number;
        industryTypeName: string;
      }
    | undefined;
}