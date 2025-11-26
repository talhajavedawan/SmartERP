import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { NzMessageService } from "ng-zorro-antd/message";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { Subscription, Observable, forkJoin, map } from "rxjs";
import {
  EmployeeService,
  Employee,
  EnumItem,
  EmployeeCreateUpdateDto,
  DepartmentCompany,
} from "../../../../shared/services/employee.service";
import {
  CompanyService,
  CompanyList,
} from "../../../../shared/services/company-center/company/company.service";
import {
  DepartmentService,
  Department,
} from "../../../../shared/services/department.service";
import { LocationService } from "../../../../shared/services/location.service";
import {
  JobTitleService,
  JobTitle,
} from "../../../../shared/services/company-center/company/lists/jobTitle.service";
import {
  TransactionItemType,
  StatusService,
  Status,
} from "../../../../shared/services/status.service";
import { StatusClassService } from "../../../../shared/services/statusClass.service";
import { ProfilePictureService } from "../../../../shared/services/profile-picture.service";
import { environment } from "../../../../../environments/environment";
import { UserService } from "../../../../shared/services/User.service";
interface Location {
  id: number;
  name: string;
}

@Component({
  selector: "app-employee-form",
  templateUrl: "./employee-form.component.html",
  styleUrls: ["../../../../../scss/forms.css", "./employee-form.component.css"],
  imports: [CommonModule, ReactiveFormsModule, NzTabsModule],
  standalone: true,
})
export class EmployeeFormComponent implements OnInit, OnDestroy {
  employeeForm!: FormGroup;
  selectedTabIndex = 0;
  employeeId?: number;
  isLoading = false;
  currentUser: any = null;
  religions = [
    "Christianity",
    "Islam",
    "Hinduism",
    "Buddhism",
    "Sikhism",
    "Judaism",
    "Baháʼí Faith",
    "Jainism",
    "Shinto",
    "Taoism",
    "Zoroastrianism",
    "Confucianism",
    "Atheist",
    "Others",
  ];

  employmentTypes = ["Full-Time", "Part-Time", "Contract"];
  zones: Location[] = [];
  permanentCountries: Location[] = [];
  permanentStates: Location[] = [];
  permanentCities: Location[] = [];
  temporaryCountries: Location[] = [];
  temporaryStates: Location[] = [];
  temporaryCities: Location[] = [];
  genders: EnumItem[] = [];
  maritalStatuses: EnumItem[] = [];
  bloodGroups: EnumItem[] = [];

  allCompanies: CompanyList[] = [];
  filteredCompanies: CompanyList[] = [];
  selectedCompanies: DepartmentCompany[] = [];

  allDepartments: Department[] = [];
  filteredDepartments: Department[] = [];
  selectedDepartments: Department[] = [];

  companySearch = "";
  departmentSearch = "";

  nationalities: Location[] = [];
  allEmployees: Employee[] = [];
  filteredManagers: Employee[] = [];
  filteredHrManagers: Employee[] = [];
  managerSearch = "";
  hrManagerSearch = "";

  jobTitles: JobTitle[] = [];
  filteredJobTitles: JobTitle[] = [];
  jobTitleSearch = "";

  employeeStatus: Status[] = [];
  filteredEmployeeStatus: Status[] = [];
  employeeStatusSearch = "";

  statusClasses: any[] = [];
  filteredStatusClasses: any[] = [];

  profilePictureUrl: string | null = null; // for preview
  selectedProfileFile: File | null = null; // file from input
  removeProfilePictureFlag = false;
  private subscriptions = new Subscription();

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private companyService: CompanyService,
    private departmentService: DepartmentService,
    private locationService: LocationService,
    private jobTitleService: JobTitleService,
    private statusService: StatusService,
    private statusClassService: StatusClassService,
    private message: NzMessageService,
    private profilePictureService: ProfilePictureService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.employeeId = this.route.snapshot.paramMap.get("id")
      ? +this.route.snapshot.paramMap.get("id")!
      : undefined;

    this.buildForm();
    this.loadLookups();
    this.setupCascadingDropdowns();

    this.loadCurrentUser();

    if (this.employeeId) {
      this.loadEmployee(this.employeeId);
    }
    if (this.employeeId) {
      this.profilePictureUrl = this.employeeService.getProfilePictureUrl(
        this.employeeId
      );
    }
  }
  private setupStatusCascading(): void {
    this.employeeForm
      .get("employeeStatus")
      ?.valueChanges.subscribe((newValue) => {
        // Only trigger if value actually changed and is not empty
        if (
          newValue &&
          newValue !== this.employeeForm.get("employeeStatus")?.value
        ) {
          return; // Skip if same
        }
        this.onEmployeeStatusChange();
      });
  }
  private loadCurrentUser(): void {
    // Check if it's a Power User first
    const isPowerUser = localStorage.getItem("isPowerUser") === "true";

    if (isPowerUser) {
      this.currentUser = {
        userName: "Administrator",
        email: "Administrator2244@gmail.com",
        isPowerUser: true,
      };
      return;
    }
    // For regular users, get current user data
    this.userService.getCurrentUser().subscribe({
      next: (user) => {
        if (user && user.userName) {
          this.currentUser = user;
          console.log("👤 Current logged-in user:", user);
        }
      },
      error: (err) => {
        console.error("❌ Error loading current user:", err);
      },
    });
  }
  private isEditingOwnProfile(): boolean {
    if (!this.currentUser || !this.employeeId) return false;

    // For power users, they're not employees, so no self-update
    if (this.currentUser.isPowerUser) return false;

    // For regular users, check if employeeId matches user's employeeId
    // If user.employeeId is not available, assume employeeId = userId
    const userEmployeeId = this.currentUser.employeeId || this.currentUser.id;
    return userEmployeeId === this.employeeId;
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  private formatDate(date?: string): string | undefined {
    return date ? date.split("T")[0] : undefined;
  }

  private toISODate(date?: string): string | undefined {
    if (!date) return undefined;
    const d = new Date(date);
    return isNaN(d.getTime()) || d.getFullYear() <= 1
      ? undefined
      : d.toISOString();
  }

  loadCountries(zoneId: number, type: "permanent" | "temporary") {
    this.subscriptions.add(
      this.companyService.getCountriesByZone(zoneId).subscribe({
        next: (data) => {
          if (type === "permanent") this.permanentCountries = data || [];
          else this.temporaryCountries = data || [];
        },
        error: () =>
          this.message.error(`Failed to load countries for ${type} address.`),
      })
    );
  }

  loadStates(countryId: number, type: "permanent" | "temporary") {
    this.subscriptions.add(
      this.companyService.getStatesByCountry(countryId).subscribe({
        next: (data) => {
          if (type === "permanent") this.permanentStates = data || [];
          else this.temporaryStates = data || [];
        },
        error: () =>
          this.message.error(`Failed to load states for ${type} address.`),
      })
    );
  }

  loadCities(stateId: number, type: "permanent" | "temporary") {
    this.subscriptions.add(
      this.companyService.getCitiesByState(stateId).subscribe({
        next: (data) => {
          if (type === "permanent") this.permanentCities = data || [];
          else this.temporaryCities = data || [];
        },
        error: () =>
          this.message.error(`Failed to load cities for ${type} address.`),
      })
    );
  }

  private setupCascadingDropdowns(): void {
    this.employeeForm
      .get("PermanentZoneId")
      ?.valueChanges.subscribe((zoneId) => {
        this.permanentCountries = [];
        this.permanentStates = [];
        this.permanentCities = [];
        this.employeeForm.patchValue({
          PermanentCountryId: "",
          PermanentStateId: "",
          PermanentCityId: "",
        });
        if (zoneId) this.loadCountries(Number(zoneId), "permanent");
      });

    this.employeeForm
      .get("PermanentCountryId")
      ?.valueChanges.subscribe((countryId) => {
        this.permanentStates = [];
        this.permanentCities = [];
        this.employeeForm.patchValue({
          PermanentStateId: "",
          PermanentCityId: "",
        });
        if (countryId) this.loadStates(Number(countryId), "permanent");
      });

    this.employeeForm
      .get("PermanentStateId")
      ?.valueChanges.subscribe((stateId) => {
        this.permanentCities = [];
        this.employeeForm.patchValue({ PermanentCityId: "" });
        if (stateId) this.loadCities(Number(stateId), "permanent");
      });

    this.employeeForm
      .get("TemporaryZoneId")
      ?.valueChanges.subscribe((zoneId) => {
        this.temporaryCountries = [];
        this.temporaryStates = [];
        this.temporaryCities = [];
        this.employeeForm.patchValue({
          TemporaryCountryId: "",
          TemporaryStateId: "",
          TemporaryCityId: "",
        });
        if (zoneId) this.loadCountries(Number(zoneId), "temporary");
      });

    this.employeeForm
      .get("TemporaryCountryId")
      ?.valueChanges.subscribe((countryId) => {
        this.temporaryStates = [];
        this.temporaryCities = [];
        this.employeeForm.patchValue({
          TemporaryStateId: "",
          TemporaryCityId: "",
        });
        if (countryId) this.loadStates(Number(countryId), "temporary");
      });

    this.employeeForm
      .get("TemporaryStateId")
      ?.valueChanges.subscribe((stateId) => {
        this.temporaryCities = [];
        this.employeeForm.patchValue({ TemporaryCityId: "" });
        if (stateId) this.loadCities(Number(stateId), "temporary");
      });
  }

  buildForm() {
    this.employeeForm = this.fb.group({
      FirstName: [
        "",
        [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)],
      ],
      LastName: [
        "",
        [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)],
      ],
      FatherName: [
        "",
        [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)],
      ],
      CNIC: [
        "",
        [Validators.required, Validators.pattern(/^\d{5}-\d{7}-\d{1}$/)],
      ],
      Gender: ["", Validators.required],
      DateOfBirth: ["", Validators.required],
      MaritalStatus: ["", Validators.required],
      BloodGroup: ["", Validators.required],
      PassportNumber: [""],
      Religion: ["", Validators.required],
      Nationality: ["", Validators.required],
      Email: ["", [Validators.required, Validators.email]],
      PhoneNumber: [
        "",
        [Validators.required, Validators.pattern(/^\+?\d{10,15}$/)],
      ],
      EmergencyPhoneNumber: ["", Validators.pattern(/^\+?\d{10,15}$/)],
      LinkedIn: [""],
      PermanentAddressLine1: ["", Validators.required],
      PermanentAddressLine2: [""],
      PermanentZoneId: ["", Validators.required],
      PermanentCountryId: ["", Validators.required],
      PermanentStateId: ["", Validators.required],
      PermanentCityId: ["", Validators.required],
      PermanentZipcode: [
        "",
        [Validators.required, Validators.pattern(/^\d{4,10}$/)],
      ],
      TemporaryAddressLine1: [""],
      TemporaryAddressLine2: [""],
      TemporaryZoneId: [""],
      TemporaryCountryId: [""],
      TemporaryStateId: [""],
      TemporaryCityId: [""],
      TemporaryZipcode: ["", Validators.pattern(/^\d{4,10}$/)],
      systemDisplayName: ["", [Validators.required, Validators.maxLength(100)]],
      jobTitle: [""],
      managerId: [""],
      isActive: [true],
      employmentType: ["", [Validators.required, Validators.maxLength(50)]],
      employeeStatus: ["", Validators.required],
      employeeStatusClass: ["", Validators.required],
      payGrade: ["", Validators.maxLength(50)],
      hrManagerId: [""],
      hireDate: ["", Validators.required],
      probationPeriodEndDate: [""],
      terminationDate: [""],
    });
  }

  formatCnic(event: any) {
    let value = event.target.value.replace(/\D/g, "");
    if (value.length > 5)
      value = value.substring(0, 5) + "-" + value.substring(5);
    if (value.length > 13)
      value = value.substring(0, 13) + "-" + value.substring(13, 14);
    this.employeeForm.get("CNIC")?.setValue(value, { emitEvent: false });
  }

  loadLookups() {
    this.subscriptions.add(
      this.companyService.getZones().subscribe({
        next: (res) => (this.zones = res),
        error: () => this.message.error("Failed to load zones."),
      })
    );

    this.subscriptions.add(
      this.employeeService.getBloodGroups().subscribe({
        next: (res) => (this.bloodGroups = res),
        error: () => this.message.error("Failed to load blood groups."),
      })
    );

    this.subscriptions.add(
      this.employeeService.getGenders().subscribe({
        next: (res) => (this.genders = res),
        error: () => this.message.error("Failed to load genders."),
      })
    );

    this.subscriptions.add(
      this.employeeService.getMaritalStatuses().subscribe({
        next: (res) => (this.maritalStatuses = res),
        error: () => this.message.error("Failed to load marital statuses."),
      })
    );

    this.subscriptions.add(
      forkJoin({
        companies: this.companyService.getAllCompanies("active"),
        departments: this.departmentService.getDepartments(),
      })
        .pipe(
          map(({ companies, departments }) => {
            const filtered = (companies || []).filter(
              (c: any) => c.companyType === 1 || c.companyType === 2
            );
            return {
              companies: filtered,
              departments: departments || [],
            };
          })
        )
        .subscribe({
          next: ({ companies, departments }) => {
            this.allCompanies = companies;
            this.allDepartments = departments;
            this.resetCompanyAndDeptLists();
          },
          error: () =>
            this.message.error("Failed to load companies and departments."),
        })
    );

    this.subscriptions.add(
      this.locationService.getCountries().subscribe({
        next: (countries) => (this.nationalities = countries),
        error: () =>
          this.message.error("Failed to load countries for nationality."),
      })
    );

    this.subscriptions.add(
      this.employeeService.getAllEmployees("active").subscribe({
        next: (emps) => {
          this.allEmployees = emps;
          this.filterManagers();
          this.filterHrManagers();
        },
        error: () =>
          this.message.error("Failed to load employees for manager/HR."),
      })
    );

    this.subscriptions.add(
      this.jobTitleService.getAllJobTitles("all").subscribe({
        next: (titles) => {
          this.jobTitles = titles.filter((t) => t.isActive);
          this.filteredJobTitles = [...this.jobTitles];
        },
        error: () => this.message.error("Failed to load job titles."),
      })
    );

    this.subscriptions.add(
      this.statusService
        .getAllStatuses(TransactionItemType.Employee, "all")
        .subscribe({
          next: (statuses) => {
            this.employeeStatus = statuses.filter((s) => s.isActive);
            this.filteredEmployeeStatus = [...this.employeeStatus];
          },
          error: () => this.message.error("Failed to load employee statuses."),
        })
    );
    this.subscriptions.add(
      this.statusClassService
        .getAll(TransactionItemType.Employee, "all")
        .subscribe({
          next: (res) => {
            this.statusClasses = res.filter((sc) => sc.isActive);
            this.filteredStatusClasses = [];
            this.onEmployeeStatusChange(); // Re-filter if status is already selected
          },
          error: () => this.message.error("Failed to load status classes."),
        })
    );
  }

  onEmployeeStatusChange(event?: any): void {
    let selectedStatusName: string | null = null;

    if (event && event.target) {
      // From HTML (change) event
      selectedStatusName = event.target.value;
      if (!selectedStatusName) {
        this.filteredStatusClasses = [];
        this.employeeForm.get("employeeStatusClass")?.setValue("");
        return;
      }
    } else {
      // From valueChanges or initial load
      selectedStatusName = this.employeeForm.get("employeeStatus")?.value;
    }

    if (!selectedStatusName) {
      this.filteredStatusClasses = [];
      this.employeeForm.get("employeeStatusClass")?.setValue("");
      return;
    }

    // CRITICAL FIX: Only patch if the value is different (prevents loop)
    const currentStatus = this.employeeForm.get("employeeStatus")?.value;
    if (currentStatus !== selectedStatusName) {
      this.employeeForm
        .get("employeeStatus")
        ?.setValue(selectedStatusName, { emitEvent: false });
    }

    // Filter status classes
    this.filteredStatusClasses = this.statusClasses.filter(
      (sc) => sc.statusName === selectedStatusName
    );

    // Validate current class
    const currentClass = this.employeeForm.get("employeeStatusClass")?.value;
    const isValidClass = this.filteredStatusClasses.some(
      (sc) => sc.className === currentClass
    );

    if (currentClass && !isValidClass) {
      this.employeeForm.get("employeeStatusClass")?.setValue("");
    }
  }

  // Reset both lists when data is loaded or form is reset
  private resetCompanyAndDeptLists() {
    this.selectedCompanies = [];
    this.selectedDepartments = [];
    this.companySearch = "";
    this.departmentSearch = "";
    this.filterCompanies();
    this.filterDepartments();
  }

  loadEmployee(id: number) {
    this.isLoading = true;

    const emp$ = this.employeeService.getEmployeeById(id);
    const employees$ = this.employeeService.getAvailableEmployeesForEdit(id);

    this.subscriptions.add(
      forkJoin([emp$, employees$]).subscribe({
        next: ([emp, availableEmps]) => {
          this.filterManagers();
          this.filterHrManagers();

          const patchData: any = {
            FirstName: emp.person?.firstName || "",
            LastName: emp.person?.lastName || "",
            FatherName: emp.person?.fatherName || "",
            CNIC: emp.person?.cnic || "",
            Gender: emp.person?.gender || "",
            MaritalStatus: emp.person?.maritalStatus || "",
            BloodGroup: emp.person?.bloodGroup || "",
            DateOfBirth: this.formatDate(emp.person?.dob) || "",
            PassportNumber: emp.person?.passportNumber || "",
            Nationality: emp.person?.nationality || "",
            Religion: emp.person?.religion || "",
            Email: emp.contact?.email || "",
            PhoneNumber: emp.contact?.phoneNumber || "",
            EmergencyPhoneNumber: emp.contact?.emergencyPhoneNumber || "",
            LinkedIn: emp.contact?.linkedIn || "",
            PermanentAddressLine1: emp.permanentAddress?.addressLine1 || "",
            PermanentAddressLine2: emp.permanentAddress?.addressLine2 || "",
            PermanentZipcode: emp.permanentAddress?.zipcode || "",
            PermanentZoneId: emp.permanentAddress?.zoneId
              ? `${emp.permanentAddress.zoneId}`
              : "",
            PermanentCountryId: emp.permanentAddress?.countryId
              ? `${emp.permanentAddress.countryId}`
              : "",
            PermanentStateId: emp.permanentAddress?.stateId
              ? `${emp.permanentAddress.stateId}`
              : "",
            PermanentCityId: emp.permanentAddress?.cityId
              ? `${emp.permanentAddress.cityId}`
              : "",
            TemporaryAddressLine1: emp.temporaryAddress?.addressLine1 || "",
            TemporaryAddressLine2: emp.temporaryAddress?.addressLine2 || "",
            TemporaryZipcode: emp.temporaryAddress?.zipcode || "",
            TemporaryZoneId: emp.temporaryAddress?.zoneId
              ? `${emp.temporaryAddress.zoneId}`
              : "",
            TemporaryCountryId: emp.temporaryAddress?.countryId
              ? `${emp.temporaryAddress.countryId}`
              : "",
            TemporaryStateId: emp.temporaryAddress?.stateId
              ? `${emp.temporaryAddress.stateId}`
              : "",
            TemporaryCityId: emp.temporaryAddress?.cityId
              ? `${emp.temporaryAddress.cityId}`
              : "",
            systemDisplayName: emp.systemDisplayName || "",
            jobTitle: emp.jobTitle || "",
            managerId: emp.managerId || null,
            hrManagerId: emp.hrManagerId || null,
            isActive: emp.isActive ?? true,
            employmentType: emp.employmentType || "",
            employeeStatus: emp.employeeStatus || "",
            employeeStatusClass: emp.employeeStatusClass || "",
            payGrade: emp.payGrade || "",
            hireDate: this.formatDate(emp.hireDate) || "",
            probationPeriodEndDate:
              this.formatDate(emp.probationPeriodEndDate) || "",
            terminationDate: this.formatDate(emp.terminationDate) || "",
          };

          this.employeeForm.patchValue(patchData);
          this.onEmployeeStatusChange();
          this.selectedCompanies = (emp.companies || []).map((c: any) => ({
            id: c.id,
            companyName: c.companyName,
          }));
          this.selectedDepartments = emp.departments || [];

          this.filterCompanies();
          this.filterDepartments();
          this.profilePictureUrl = this.employeeService.getProfilePictureUrl(
            emp.id
          );
          // Address cascading
          const requests: Observable<any>[] = [];
          if (patchData.PermanentZoneId)
            requests.push(
              this.companyService.getCountriesByZone(
                Number(patchData.PermanentZoneId)
              )
            );
          if (patchData.PermanentCountryId)
            requests.push(
              this.companyService.getStatesByCountry(
                Number(patchData.PermanentCountryId)
              )
            );
          if (patchData.PermanentStateId)
            requests.push(
              this.companyService.getCitiesByState(
                Number(patchData.PermanentStateId)
              )
            );
          if (patchData.TemporaryZoneId)
            requests.push(
              this.companyService.getCountriesByZone(
                Number(patchData.TemporaryZoneId)
              )
            );
          if (patchData.TemporaryCountryId)
            requests.push(
              this.companyService.getStatesByCountry(
                Number(patchData.TemporaryCountryId)
              )
            );
          if (patchData.TemporaryStateId)
            requests.push(
              this.companyService.getCitiesByState(
                Number(patchData.TemporaryStateId)
              )
            );

          if (requests.length > 0) {
            this.subscriptions.add(
              forkJoin(requests).subscribe({
                next: (results) => {
                  let idx = 0;
                  if (patchData.PermanentZoneId)
                    this.permanentCountries = results[idx++] || [];
                  if (patchData.PermanentCountryId)
                    this.permanentStates = results[idx++] || [];
                  if (patchData.PermanentStateId)
                    this.permanentCities = results[idx++] || [];
                  if (patchData.TemporaryZoneId)
                    this.temporaryCountries = results[idx++] || [];
                  if (patchData.TemporaryCountryId)
                    this.temporaryStates = results[idx++] || [];
                  if (patchData.TemporaryStateId)
                    this.temporaryCities = results[idx++] || [];
                },
                error: () => this.message.error("Failed to load address data."),
              })
            );
          }

          this.isLoading = false;
        },
        error: () => {
          this.message.error("Failed to load employee data.");
          this.isLoading = false;
        },
      })
    );
  }
  saveEmployee() {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      this.message.warning("Please fill all required fields.");
      return;
    }

    const form = this.employeeForm.value;

    const payload: EmployeeCreateUpdateDto = {
      id: this.employeeId ?? 0,

      systemDisplayName: form.systemDisplayName,
      jobTitle: form.jobTitle,
      managerId: form.managerId ? Number(form.managerId) : undefined,
      hrManagerId: form.hrManagerId ? Number(form.hrManagerId) : undefined,
      payGrade: form.payGrade || undefined,
      employmentType: form.employmentType,
      employeeStatus: form.employeeStatus || undefined,
      employeeStatusClass: form.employeeStatusClass || undefined,
      isActive: form.isActive,

      profilePictureFile: this.selectedProfileFile || undefined,
      removeProfilePicture: this.removeProfilePictureFlag,
      hireDate: this.toISODate(form.hireDate),
      probationPeriodEndDate:
        this.toISODate(form.probationPeriodEndDate) || undefined,
      terminationDate: this.toISODate(form.terminationDate) || undefined,

      person: {
        firstName: form.FirstName,
        lastName: form.LastName,
        fatherName: form.FatherName,
        cnic: form.CNIC,
        dob: this.toISODate(form.DateOfBirth),
        bloodGroup: form.BloodGroup,
        gender: form.Gender,
        maritalStatus: form.MaritalStatus,
        passportNumber: form.PassportNumber || undefined,
        nationality: form.Nationality,
        religion: form.Religion,
      },

      contact: {
        email: form.Email,
        phoneNumber: form.PhoneNumber,
        linkedIn: form.LinkedIn || undefined,
        emergencyPhoneNumber: form.EmergencyPhoneNumber || undefined,
      },

      permanentAddress: {
        addressLine1: form.PermanentAddressLine1,
        addressLine2: form.PermanentAddressLine2 || undefined,
        zipcode: form.PermanentZipcode,
        zoneId: form.PermanentZoneId ? Number(form.PermanentZoneId) : undefined,
        countryId: form.PermanentCountryId
          ? Number(form.PermanentCountryId)
          : undefined,
        stateId: form.PermanentStateId
          ? Number(form.PermanentStateId)
          : undefined,
        cityId: form.PermanentCityId ? Number(form.PermanentCityId) : undefined,
      },

      temporaryAddress:
        form.TemporaryAddressLine1 ||
        form.TemporaryAddressLine2 ||
        form.TemporaryZipcode ||
        form.TemporaryZoneId ||
        form.TemporaryCountryId ||
        form.TemporaryStateId ||
        form.TemporaryCityId
          ? {
              addressLine1: form.TemporaryAddressLine1 || undefined,
              addressLine2: form.TemporaryAddressLine2 || undefined,
              zipcode: form.TemporaryZipcode || undefined,
              zoneId: form.TemporaryZoneId
                ? Number(form.TemporaryZoneId)
                : undefined,
              countryId: form.TemporaryCountryId
                ? Number(form.TemporaryCountryId)
                : undefined,
              stateId: form.TemporaryStateId
                ? Number(form.TemporaryStateId)
                : undefined,
              cityId: form.TemporaryCityId
                ? Number(form.TemporaryCityId)
                : undefined,
            }
          : undefined,

      CompanyIds: this.selectedCompanies.map((c) => c.id),
      DepartmentIds: this.selectedDepartments.map((d) => d.id),
    };

    this.isLoading = true;

    const save$ = this.employeeId
      ? this.employeeService.updateEmployee(this.employeeId, payload)
      : this.employeeService.createEmployee(payload);

    this.subscriptions.add(
      save$.subscribe({
        next: (res) => {
          this.isLoading = false;
          this.message.success(res.message || "Saved successfully!");

          // Only refresh header if editing own profile AND profile picture was updated
          if (
            this.isEditingOwnProfile() &&
            (this.selectedProfileFile || this.removeProfilePictureFlag)
          ) {
            this.refreshHeaderProfilePicture();
            console.log(
              "🔄 Updating header profile picture (editing own profile)"
            );
          } else {
            console.log(
              "ℹ️ Not updating header (editing different user or no profile picture change)"
            );
          }

          this.router.navigate(["/lst-employee"]);
        },
        error: (err) => {
          this.isLoading = false;
          console.error("API Error:", err);
          this.message.error(err.error?.message || "Failed to save");
        },
      })
    );
  }

  private refreshHeaderProfilePicture(): void {
    if (this.employeeId) {
      // Force refresh with cache busting
      const timestamp = new Date().getTime();
      const newUrl = `${environment.apiBaseUrl}/Employee/ProfilePicture/${this.employeeId}?t=${timestamp}`;

      // Update the shared service - this will automatically update the header
      this.profilePictureService.updateProfilePicture(newUrl);

      console.log("🔄 Profile picture updated in header via service");
    }
  }
  closeModal() {
    this.removeProfilePictureFlag = false;
    this.selectedProfileFile = null;
    this.resetCompanyAndDeptLists();
    this.router.navigate(["/lst-employee"]);
  }

  // === COMPANY & DEPARTMENT LOGIC ===

  filterCompanies() {
    const q = this.companySearch.trim().toLowerCase();
    const selectedIds = this.selectedCompanies.map((c) => c.id);

    this.filteredCompanies = this.allCompanies
      .filter(
        (c) =>
          !selectedIds.includes(c.id) &&
          (!q || c.companyName.toLowerCase().includes(q))
      )
      .sort((a, b) => a.companyName.localeCompare(b.companyName));
  }

  filterDepartments() {
    const q = this.departmentSearch.trim().toLowerCase();
    const selectedDeptIds = this.selectedDepartments.map((d) => d.id);
    const selectedCompanyIds = this.selectedCompanies.map((c) => c.id);

    let base = this.allDepartments.filter(
      (d) =>
        !selectedDeptIds.includes(d.id) &&
        (!q || d.deptName.toLowerCase().includes(q))
    );

    if (selectedCompanyIds.length > 0) {
      base = base.filter((d) =>
        d.companies?.some((dc) => selectedCompanyIds.includes(dc.id))
      );
    }

    this.filteredDepartments = base.sort((a, b) =>
      a.deptName.localeCompare(b.deptName)
    );
  }

  addCompany(company: CompanyList) {
    const dc: DepartmentCompany = {
      id: company.id,
      companyName: company.companyName,
    };

    if (!this.selectedCompanies.find((c) => c.id === dc.id)) {
      this.selectedCompanies.push(dc);
      this.filterCompanies();
      this.filterDepartments();
    }
  }

  removeCompany(company: DepartmentCompany, event?: MouseEvent) {
    if (event) event.stopPropagation();

    this.selectedCompanies = this.selectedCompanies.filter(
      (c) => c.id !== company.id
    );

    // Remove departments that no longer belong
    this.selectedDepartments = this.selectedDepartments.filter((d) =>
      this.selectedCompanies.some((c) =>
        d.companies?.some((dc) => dc.id === c.id)
      )
    );

    this.filterCompanies();
    this.filterDepartments();
  }

  addDepartment(dept: Department) {
    if (!this.selectedDepartments.find((d) => d.id === dept.id)) {
      this.selectedDepartments.push(dept);
      this.filterDepartments();
    }
  }

  removeDepartment(dept: Department, event?: MouseEvent) {
    if (event) event.stopPropagation();

    this.selectedDepartments = this.selectedDepartments.filter(
      (d) => d.id !== dept.id
    );

    this.filterDepartments();
  }

  // === OTHER FILTERS ===

  filterManagers() {
    const q = this.managerSearch.trim().toLowerCase();
    this.filteredManagers = this.allEmployees
      .filter(
        (e) =>
          e.id !== this.employeeId &&
          (e.systemDisplayName ?? "").toLowerCase().includes(q)
      )
      .sort((a, b) => a.systemDisplayName.localeCompare(b.systemDisplayName));
  }

  filterHrManagers() {
    const q = this.hrManagerSearch.trim().toLowerCase();
    this.filteredHrManagers = this.allEmployees
      .filter(
        (e) =>
          e.id !== this.employeeId &&
          (e.systemDisplayName ?? "").toLowerCase().includes(q)
      )
      .sort((a, b) => a.systemDisplayName.localeCompare(b.systemDisplayName));
  }

  filterJobTitles() {
    const q = this.jobTitleSearch.trim().toLowerCase();
    this.filteredJobTitles = q
      ? this.jobTitles.filter((t) => t.jobTitleName.toLowerCase().includes(q))
      : [...this.jobTitles];
  }

  filterEmployeeStatus(): void {
    const q = this.employeeStatusSearch.trim().toLowerCase();
    this.filteredEmployeeStatus = q
      ? this.employeeStatus.filter((s) =>
          s.statusName.toLowerCase().includes(q)
        )
      : [...this.employeeStatus];
  }
  getManagerName(managerId: number): string {
    if (!managerId) return "";
    const manager = this.allEmployees.find((e) => e.id === managerId);
    return manager?.systemDisplayName || "Unknown";
  }

  getHrManagerName(hrManagerId: number): string {
    if (!hrManagerId) return "";
    const hrManager = this.allEmployees.find((e) => e.id === hrManagerId);
    return hrManager?.systemDisplayName || "Unknown";
  }

  onProfilePictureChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    const maxSize = 5 * 1024 * 1024; // 5 MB
    if (file.size > maxSize) {
      this.message.error("Profile picture must be ≤ 5 MB");
      return;
    }

    const allowed = ["image/jpeg", "image/png", "image/gif", "image/webp"];
    if (!allowed.includes(file.type)) {
      this.message.error("Only JPG, PNG, GIF, WEBP allowed");
      return;
    }

    this.selectedProfileFile = file;
    this.removeProfilePictureFlag = false;
    // Preview
    const reader = new FileReader();
    reader.onload = (e) =>
      (this.profilePictureUrl = e.target?.result as string);
    reader.readAsDataURL(file);
  }

  removeProfilePicture(): void {
    this.selectedProfileFile = null;
    this.profilePictureUrl = null;
    this.removeProfilePictureFlag = true;
    this.employeeForm.markAsDirty();
  }
  getProfileImageSrc(): string {
    return this.profilePictureUrl || "assets/images/avatar.jpg";
  }

  onImageError(): void {
    this.profilePictureUrl = null; // Force default
  }
}
