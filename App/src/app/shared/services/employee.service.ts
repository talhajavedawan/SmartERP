import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

import { environment } from "../../../environments/environment";
import { Department } from "./department.service";

export interface Person {
  id?: number;
  firstName: string;
  lastName: string;
  fatherName?: string;
  cnic?: string;
  dob?: string;
  bloodGroup?: string;
  gender?: string;
  maritalStatus?: string;
  passportNumber?: string;
  nationality?: string;
  religion?: string;
}

export interface Contact {
  id?: number;
  email?: string;
  phoneNumber?: string;
  linkedIn?: string;
  emergencyPhoneNumber?: string;
}

export interface Address {
  id?: number;
  addressLine1?: string;
  addressLine2?: string;
  zipcode?: string;
  countryId?: number;
  stateId?: number;
  cityId?: number;
  zoneId?: number;
}

export interface DepartmentCompany {
  id: number;
  companyName: string;
}

export interface Employee {
  id: number;
  systemDisplayName: string;
  jobTitle?: string;
  hireDate?: string;
  employmentType?: string;
  employeeStatus?: string;
  employeeStatusClass?: string;
  payGrade?: string;
  // Profile Picture Fields
  profilePicture?: string;
  profilePictureContentType?: string;
  profilePictureSize?: number;
  profilePictureFileName?: string;
  profilePictureUrl?: string;

  managerId?: number;
  hrManagerId?: number;
  hrManagerName?: string;
  managerName?: string;

  probationPeriodEndDate?: string;
  terminationDate?: string;
  joinDate?: string;

  createdByUserName?: string;
  lastModifiedByUserName?: string;
  createdDate?: string;
  lastModifiedDate?: string;

  isVoid?: boolean;
  isActive?: boolean;
  person?: Person;
  contact?: Contact;
  permanentAddress?: Address;
  temporaryAddress?: Address;
  companies?: DepartmentCompany[];
  departments?: Department[];
}

export interface EmployeeCreateUpdateDto {
  id: number;
  systemDisplayName: string;
  jobTitle?: string;
  hireDate?: string;
  employmentType?: string;
  employeeStatus?: string;
  employeeStatusClass?: string;
  managerId?: number;
  isActive?: boolean;
  payGrade?: string;
  hrManagerId?: number;
  probationPeriodEndDate?: string;
  terminationDate?: string;
  // Profile Picture
  profilePictureFile?: File;
  profilePicture?: string;
  profilePictureContentType?: string;
  profilePictureSize?: number;
  profilePictureFileName?: string;
  removeProfilePicture?: boolean;

  person?: Person;
  contact?: Contact;
  permanentAddress?: Address;
  temporaryAddress?: Address;
  CompanyIds?: number[];
  DepartmentIds?: number[];
}

export interface EnumItem {
  id: number;
  name: string;
}

@Injectable({
  providedIn: "root",
})
export class EmployeeService {
  private baseUrl = environment.apiBaseUrl;
  private apiUrl = `${this.baseUrl}/Employee`;
  private enumUrl = `${this.baseUrl}/enum`;

  constructor(private http: HttpClient) {}
  // get Image
  getProfilePictureUrl(employeeId: number): string | null {
    if (!employeeId) return null;
    const timestamp = Date.now();
    return `${this.baseUrl}/Employee/ProfilePicture/${employeeId}?t=${timestamp}`;
  }
  // Get all  employees
  getAllEmployees(status: string = "all"): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.apiUrl}/GetAll`, {
      params: { status },
    });
  }

  // Get employee by ID
  getEmployeeById(id: number): Observable<Employee> {
    return this.http.get<Employee>(`${this.apiUrl}/GetById/${id}`);
  }

  // For Add User → Only Active & Unassigned Employees
  getAvailableEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(`${this.apiUrl}/GetAvailableEmployees`);
  }

  // For Edit User → Active + Current Assigned Employee (even if inactive)
  getAvailableEmployeesForEdit(userId: number): Observable<Employee[]> {
    return this.http.get<Employee[]>(
      `${this.apiUrl}/GetAvailableEmployeesForEdit/${userId}`
    );
  }

  // Create new employee
  createEmployee(
    employee: EmployeeCreateUpdateDto
  ): Observable<{ message: string; employee: Employee }> {
    const formData = this.toFormData(employee);
    return this.http.post<{ message: string; employee: Employee }>(
      `${this.apiUrl}/Create`,
      formData
    );
  }

  // Update employee
  updateEmployee(
    id: number,
    employee: EmployeeCreateUpdateDto
  ): Observable<{ message: string }> {
    const formData = this.toFormData(employee);
    return this.http.put<{ message: string }>(
      `${this.apiUrl}/Update/${id}`,
      formData
    );
  }

  // Soft delete employee
  deleteEmployee(id: number): Observable<{ message: string }> {
    return this.http.delete<{ message: string }>(`${this.apiUrl}/Void/${id}`);
  }

  // Enum helpers
  getBloodGroups(): Observable<EnumItem[]> {
    return this.http.get<EnumItem[]>(`${this.enumUrl}/blood-groups`);
  }

  getGenders(): Observable<EnumItem[]> {
    return this.http.get<EnumItem[]>(`${this.enumUrl}/genders`);
  }

  getMaritalStatuses(): Observable<EnumItem[]> {
    return this.http.get<EnumItem[]>(`${this.enumUrl}/marital-statuses`);
  }

  // === ADD THIS PRIVATE METHOD ===
  private toFormData(dto: EmployeeCreateUpdateDto): FormData {
    const form = new FormData();

    // Main fields
    form.append("Id", (dto.id || 0).toString());
    form.append("SystemDisplayName", dto.systemDisplayName);
    if (dto.jobTitle) form.append("JobTitle", dto.jobTitle);
    if (dto.hireDate) form.append("HireDate", dto.hireDate);
    if (dto.employmentType) form.append("EmploymentType", dto.employmentType);
    if (dto.employeeStatus) form.append("EmployeeStatus", dto.employeeStatus);
    if (dto.employeeStatusClass)
      form.append("EmployeeStatusClass", dto.employeeStatusClass);
    if (dto.managerId) form.append("ManagerId", dto.managerId.toString());
    if (dto.hrManagerId) form.append("HRManagerId", dto.hrManagerId.toString());
    if (dto.payGrade) form.append("PayGrade", dto.payGrade);
    if (dto.probationPeriodEndDate)
      form.append("ProbationPeriodEndDate", dto.probationPeriodEndDate);
    if (dto.terminationDate)
      form.append("TerminationDate", dto.terminationDate);
    form.append("IsActive", (dto.isActive ?? true).toString());

    // Profile Picture
    if (dto.profilePictureFile) {
      form.append(
        "ProfilePictureFile",
        dto.profilePictureFile,
        dto.profilePictureFile.name
      );
    }
    if (dto.removeProfilePicture) {
      form.append("RemoveProfilePicture", "true");
    }

    // Collections
    (dto.CompanyIds || []).forEach((id) =>
      form.append("CompanyIds", id.toString())
    );
    (dto.DepartmentIds || []).forEach((id) =>
      form.append("DepartmentIds", id.toString())
    );

    // Nested objects
    this.appendObject(form, dto.person, "Person");
    this.appendObject(form, dto.contact, "Contact");
    this.appendObject(form, dto.permanentAddress, "PermanentAddress");
    this.appendObject(form, dto.temporaryAddress, "TemporaryAddress");

    return form;
  }

  private appendObject(form: FormData, obj: any, prefix: string) {
    if (!obj) return;
    for (const key in obj) {
      if (
        obj.hasOwnProperty(key) &&
        obj[key] !== null &&
        obj[key] !== undefined
      ) {
        const value = obj[key];
        const formKey = `${prefix}.${key}`;
        if (value instanceof Date) {
          form.append(formKey, value.toISOString());
        } else if (typeof value === "object" && !(value instanceof File)) {
          this.appendObject(form, value, formKey);
        } else {
          form.append(formKey, value.toString());
        }
      }
    }
  }
}
