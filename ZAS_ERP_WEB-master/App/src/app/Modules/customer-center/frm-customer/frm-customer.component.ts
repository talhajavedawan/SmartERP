import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { NzMessageService } from "ng-zorro-antd/message";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { Subscription, Observable, forkJoin } from "rxjs";
import {
  CustomerService,
  CustomerDTO,
  CustomerCreateDto,
} from "../../../shared/services/customer.service";
import { CompanyService } from "../../../shared/services/company-center/company/company.service";
import {
  IndustryService,
  IndustryType,
} from "../../../shared/services/company-center/company/lists/industry.service";
import {
  BusinessService,
  BusinessType,
} from "../../../shared/services/company-center/company/lists/business.service";
interface Location {
  id: number;
  name: string;
}
@Component({
  selector: "app-frm-customer",
  templateUrl: "./frm-customer.component.html",
  styleUrls: ["../../../../scss/forms.css"],
  imports: [CommonModule, ReactiveFormsModule, NzTabsModule],
  standalone: true,
})
export class FrmCustomerComponent implements OnInit, OnDestroy {
  customerForm!: FormGroup;
  selectedTabIndex: number = 0;
  customerId?: number;
  isLoading = false;

  zones: Location[] = [];
  billingCountries: Location[] = [];
  billingStates: Location[] = [];
  billingCities: Location[] = [];
  shippingCountries: Location[] = [];
  shippingStates: Location[] = [];
  shippingCities: Location[] = [];
  industries: IndustryType[] = [];
  businessTypes: BusinessType[] = [];

  private subscriptions = new Subscription();

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private customerService: CustomerService,
    private companyService: CompanyService,
    private businessService: BusinessService,
    private industryService: IndustryService,
    private message: NzMessageService
  ) {}

  ngOnInit(): void {
    this.customerId = this.route.snapshot.paramMap.get("id")
      ? +this.route.snapshot.paramMap.get("id")!
      : undefined;

    this.buildForm();
    this.loadLookups();
    this.setupCascadingDropdowns();

    if (this.customerId) {
      this.loadCustomer(this.customerId);
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  private buildForm(): void {
    this.customerForm = this.fb.group({
      companyName: ["", [Validators.required, Validators.maxLength(100)]],
      businessTypeId: ["", Validators.required],
      industryTypeId: ["", Validators.required],
      firstName: [
        "",
        [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)],
      ],
      lastName: [
        "",
        [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)],
      ],
      email: ["", [Validators.email]],
      phoneNumber: ["", Validators.pattern(/^\+?\d{10,15}$/)],
      billingAddressLine1: ["", Validators.required],
      billingAddressLine2: [""],
      billingZipCode: ["", [Validators.required, Validators.pattern(/^\d+$/)]],
      billingZoneId: ["", Validators.required],
      billingCountryId: ["", Validators.required],
      billingStateId: ["", Validators.required],
      billingCityId: ["", Validators.required],
      billingCityName: [""],
      billingStateName: [""],
      billingCountryName: [""],
      shippingAddressLine1: ["", Validators.required],
      shippingAddressLine2: [""],
      shippingZipCode: ["", [Validators.required, Validators.pattern(/^\d+$/)]],
      shippingZoneId: ["", Validators.required],
      shippingCountryId: ["", Validators.required],
      shippingStateId: ["", Validators.required],
      shippingCityId: ["", Validators.required],
      shippingCityName: [""],
      shippingStateName: [""],
      shippingCountryName: [""],
      isActive: [true],
      createdDate: [""],
      createdByName: [""],
      lastModified: [""],
      lastModifiedByName: [""],
    });
  }

  private loadLookups(): void {
    this.subscriptions.add(
      forkJoin({
        zones: this.companyService.getZones(),
        industries: this.industryService.getAllIndustries("active"),
        businessTypes: this.businessService.getAllBusinessTypes("active"),
      }).subscribe({
        next: ({ zones, industries, businessTypes }) => {
          this.zones = zones || [];
          this.industries = industries || [];
          this.businessTypes = businessTypes || [];
        },
        error: () => this.message.error("Failed to load lookup data."),
      })
    );
  }

  private setupCascadingDropdowns(): void {
    this.customerForm.get("billingZoneId")?.valueChanges.subscribe((zoneId) => {
      this.billingCountries = [];
      this.billingStates = [];
      this.billingCities = [];
      this.customerForm.patchValue({
        billingCountryId: "",
        billingStateId: "",
        billingCityId: "",
      });
      if (zoneId) this.loadCountries(Number(zoneId), "billing");
    });

    this.customerForm
      .get("billingCountryId")
      ?.valueChanges.subscribe((countryId) => {
        this.billingStates = [];
        this.billingCities = [];
        this.customerForm.patchValue({
          billingStateId: "",
          billingCityId: "",
        });
        if (countryId) this.loadStates(Number(countryId), "billing");
      });

    this.customerForm
      .get("billingStateId")
      ?.valueChanges.subscribe((stateId) => {
        this.billingCities = [];
        this.customerForm.patchValue({ billingCityId: "" });
        if (stateId) this.loadCities(Number(stateId), "billing");
      });

    this.customerForm
      .get("shippingZoneId")
      ?.valueChanges.subscribe((zoneId) => {
        this.shippingCountries = [];
        this.shippingStates = [];
        this.shippingCities = [];
        this.customerForm.patchValue({
          shippingCountryId: "",
          shippingStateId: "",
          shippingCityId: "",
        });
        if (zoneId) this.loadCountries(Number(zoneId), "shipping");
      });

    this.customerForm
      .get("shippingCountryId")
      ?.valueChanges.subscribe((countryId) => {
        this.shippingStates = [];
        this.shippingCities = [];
        this.customerForm.patchValue({
          shippingStateId: "",
          shippingCityId: "",
        });
        if (countryId) this.loadStates(Number(countryId), "shipping");
      });

    this.customerForm
      .get("shippingStateId")
      ?.valueChanges.subscribe((stateId) => {
        this.shippingCities = [];
        this.customerForm.patchValue({ shippingCityId: "" });
        if (stateId) this.loadCities(Number(stateId), "shipping");
      });
  }

  private loadCountries(zoneId: number, type: "billing" | "shipping"): void {
    this.subscriptions.add(
      this.companyService.getCountriesByZone(zoneId).subscribe({
        next: (data) => {
          if (type === "billing") this.billingCountries = data || [];
          else this.shippingCountries = data || [];
        },
        error: () =>
          this.message.error(`Failed to load countries for ${type} address.`),
      })
    );
  }

  private loadStates(countryId: number, type: "billing" | "shipping"): void {
    this.subscriptions.add(
      this.companyService.getStatesByCountry(countryId).subscribe({
        next: (data) => {
          if (type === "billing") this.billingStates = data || [];
          else this.shippingStates = data || [];
        },
        error: () =>
          this.message.error(`Failed to load states for ${type} address.`),
      })
    );
  }

  private loadCities(stateId: number, type: "billing" | "shipping"): void {
    this.subscriptions.add(
      this.companyService.getCitiesByState(stateId).subscribe({
        next: (data) => {
          if (type === "billing") this.billingCities = data || [];
          else this.shippingCities = data || [];
        },
        error: () =>
          this.message.error(`Failed to load cities for ${type} address.`),
      })
    );
  }

  formatPKTDate(date: string | Date | undefined | null): string {
    if (!date || isNaN(new Date(date).getTime())) {
      return "—";
    }
    try {
      const formatted = new Intl.DateTimeFormat("en-PK", {
        dateStyle: "medium",
        timeStyle: "medium",
        timeZone: "Asia/Karachi",
      }).format(new Date(date));
      const testDate = new Date("2025-10-15T08:00:00Z");
      const testFormatted = new Intl.DateTimeFormat("en-PK", {
        timeStyle: "medium",
        timeZone: "Asia/Karachi",
      }).format(testDate);
      const isPKT = testFormatted.includes("13:00:00");
      if (isPKT) {
        return formatted;
      }
    } catch (error) {}
    const utcDate = new Date(date);
    const pktDate = new Date(utcDate.getTime() + 5 * 60 * 60 * 1000);
    return new Intl.DateTimeFormat("en-PK", {
      dateStyle: "medium",
      timeStyle: "medium",
    }).format(pktDate);
  }

  private loadCustomer(id: number): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.customerService.getCustomerById(id).subscribe({
        next: (customer: CustomerCreateDto) => {
          const patchData = {
            companyName: customer.companyName || "",
            businessTypeId: customer.businessTypeId
              ? `${customer.businessTypeId}`
              : "",
            industryTypeId: customer.industryTypeId
              ? `${customer.industryTypeId}`
              : "",
            firstName: customer.firstName || "",
            lastName: customer.lastName || "",
            email: customer.email || "",
            phoneNumber: customer.phoneNumber || "",
            billingAddressLine1: customer.billingAddressLine1 || "",
            billingAddressLine2: customer.billingAddressLine2 || "",
            billingZipCode: customer.billingZipCode || "",
            billingZoneId: customer.billingZoneId
              ? `${customer.billingZoneId}`
              : "",
            billingCountryId: customer.billingCountryId
              ? `${customer.billingCountryId}`
              : "",
            billingStateId: customer.billingStateId
              ? `${customer.billingStateId}`
              : "",
            billingCityId: customer.billingCityId
              ? `${customer.billingCityId}`
              : "",
            billingCityName: customer.billingCityName || "",
            billingStateName: customer.billingStateName || "",
            billingCountryName: customer.billingCountryName || "",
            shippingAddressLine1: customer.shippingAddressLine1 || "",
            shippingAddressLine2: customer.shippingAddressLine2 || "",
            shippingZipCode: customer.shippingZipCode || "",
            shippingZoneId: customer.shippingZoneId
              ? `${customer.shippingZoneId}`
              : "",
            shippingCountryId: customer.shippingCountryId
              ? `${customer.shippingCountryId}`
              : "",
            shippingStateId: customer.shippingStateId
              ? `${customer.shippingStateId}`
              : "",
            shippingCityId: customer.shippingCityId
              ? `${customer.shippingCityId}`
              : "",
            shippingCityName: customer.shippingCityName || "",
            shippingStateName: customer.shippingStateName || "",
            shippingCountryName: customer.shippingCountryName || "",
            isActive: customer.isActive ?? true,
            createdDate: customer.createdDate || "",
            createdByName: customer.createdByName || "",
            lastModified: customer.lastModified || "",
            lastModifiedByName: customer.lastModifiedByName || "",
          };

          this.customerForm.patchValue(patchData);

          const requests: Observable<any>[] = [];
          if (customer.billingZoneId) {
            requests.push(
              this.companyService.getCountriesByZone(customer.billingZoneId)
            );
          }
          if (customer.billingCountryId) {
            requests.push(
              this.companyService.getStatesByCountry(customer.billingCountryId)
            );
          }
          if (customer.billingStateId) {
            requests.push(
              this.companyService.getCitiesByState(customer.billingStateId)
            );
          }
          if (customer.shippingZoneId) {
            requests.push(
              this.companyService.getCountriesByZone(customer.shippingZoneId)
            );
          }
          if (customer.shippingCountryId) {
            requests.push(
              this.companyService.getStatesByCountry(customer.shippingCountryId)
            );
          }
          if (customer.shippingStateId) {
            requests.push(
              this.companyService.getCitiesByState(customer.shippingStateId)
            );
          }

          if (requests.length > 0) {
            this.subscriptions.add(
              forkJoin(requests).subscribe({
                next: (results) => {
                  let idx = 0;
                  if (customer.billingZoneId)
                    this.billingCountries = results[idx++] || [];
                  if (customer.billingCountryId)
                    this.billingStates = results[idx++] || [];
                  if (customer.billingStateId)
                    this.billingCities = results[idx++] || [];
                  if (customer.shippingZoneId)
                    this.shippingCountries = results[idx++] || [];
                  if (customer.shippingCountryId)
                    this.shippingStates = results[idx++] || [];
                  if (customer.shippingStateId)
                    this.shippingCities = results[idx++] || [];
                },
                error: () => this.message.error("Failed to load address data."),
              })
            );
          }
          this.isLoading = false;
        },
        error: () => {
          this.message.error("Failed to load customer data.");
          this.isLoading = false;
        },
      })
    );
  }

  saveCustomer(): void {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      this.message.warning("Please fill all required fields.");
      return;
    }

    const form = this.customerForm.value;
    const payload: CustomerCreateDto = {
      companyName: form.companyName,
      businessTypeId: Number(form.businessTypeId),
      industryTypeId: Number(form.industryTypeId),
      firstName: form.firstName,
      lastName: form.lastName,
      email: form.email || undefined,
      phoneNumber: form.phoneNumber || undefined,
      billingAddressLine1: form.billingAddressLine1,
      billingAddressLine2: form.billingAddressLine2 || undefined,
      billingCityId: form.billingCityId
        ? Number(form.billingCityId)
        : undefined,
      billingStateId: form.billingStateId
        ? Number(form.billingStateId)
        : undefined,
      billingCountryId: form.billingCountryId
        ? Number(form.billingCountryId)
        : undefined,
      billingZoneId: form.billingZoneId
        ? Number(form.billingZoneId)
        : undefined,
      billingZipCode: form.billingZipCode,
      billingCityName: form.billingCityName || "",
      billingStateName: form.billingStateName || "",
      billingCountryName: form.billingCountryName || "",
      shippingAddressLine1: form.shippingAddressLine1,
      shippingAddressLine2: form.shippingAddressLine2 || undefined,
      shippingCityId: form.shippingCityId
        ? Number(form.shippingCityId)
        : undefined,
      shippingStateId: form.shippingStateId
        ? Number(form.shippingStateId)
        : undefined,
      shippingCountryId: form.shippingCountryId
        ? Number(form.shippingCountryId)
        : undefined,
      shippingZoneId: form.shippingZoneId
        ? Number(form.shippingZoneId)
        : undefined,
      shippingZipCode: form.shippingZipCode,
      shippingCityName: form.shippingCityName || "",
      shippingStateName: form.shippingStateName || "",
      shippingCountryName: form.shippingCountryName || "",
      isActive: form.isActive,
      createdDate: form.createdDate || new Date().toISOString(),
      createdByName: form.createdByName || "-",
      lastModified: form.lastModified || new Date().toISOString(),
      lastModifiedByName: form.lastModifiedByName || "-",
    };

    this.isLoading = true;
    const save$ = this.customerId
      ? this.customerService.updateCustomer(this.customerId, payload)
      : this.customerService.createCustomer(payload);

    this.subscriptions.add(
      save$.subscribe({
        next: () => {
          this.isLoading = false;
          this.message.success(
            this.customerId
              ? "Customer updated successfully!"
              : "Customer created successfully!"
          );
          this.router.navigate(["/lst-customer"]);
        },
        error: (err) => {
          this.isLoading = false;
          console.error("Save Error:", err);
          //this.message.error(err.message || "Failed to save customer.");
          this.message.error("Failed to Save record!");
        },
      })
    );
  }

  closeModal(): void {
    this.router.navigate(["/lst-customer"]);
  }
}
