import { Component, OnInit, OnDestroy, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { NzSelectModule } from "ng-zorro-antd/select";
import { NzTreeSelectModule } from "ng-zorro-antd/tree-select";
import { NzMessageService } from "ng-zorro-antd/message";
import { Subscription, forkJoin } from "rxjs";

import { VendorService } from "../../../../shared/services/vendor/vendor.service";
import { CompanyService } from "../../../../shared/services/company-center/company/company.service";
import { CurrencyService } from "../../../../shared/services/currency.service";
import { VendorNatureService } from "../../../../shared/services/vendor/vendor-nature.service";
import { DepartmentService } from "../../../../shared/services/department.service";
import {
  VendorCreateDto,
  VendorUpdateDto,
} from "../vendor.model";

interface TreeNode {
  title: string;
  key: string;
  parentId?: number | null;
  children: TreeNode[];
  expanded?: boolean;
  isLeaf: boolean;
}

@Component({
  selector: "app-frm-vendor",
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzTabsModule,
    NzSelectModule,
    NzTreeSelectModule,
  ],
  templateUrl: "./frm-vendor.component.html",
  styleUrls: ["./frm-vendor.component.css"],
})
export class FrmVendorComponent implements OnInit, OnDestroy {
  registrationForm!: FormGroup;
  isLoading = false;
  mode: "create" | "update" = "create";
  vendorId?: number;

  // Trees
  companyTree: TreeNode[] = [];
  departmentTree: TreeNode[] = [];
  parentVendorTree: TreeNode[] = [];

  selectedIndex = 0;

  // Base dropdowns
  zones: any[] = [];
  industryTypes: any[] = [];
  businessTypes: any[] = [];
  currencies: any[] = [];
  vendorNatures: any[] = [];

  // Cascading
  shippingCountries: any[] = [];
  shippingStates: any[] = [];
  shippingCities: any[] = [];
  billingCountries: any[] = [];
  billingStates: any[] = [];
  billingCities: any[] = [];

  alert: { type: string; message: string } = { type: "", message: "" };

  private subscriptions = new Subscription();

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private vendorService: VendorService,
    private companyService: CompanyService,
    private departmentService: DepartmentService,
    private currencyService: CurrencyService,
    private vendorNatureService: VendorNatureService,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.vendorId = Number(this.route.snapshot.paramMap.get("id"));
    this.buildForm();
    this.setupCascadingDropdowns();
    this.setupCompanyDepartmentFiltering();

    this.loadLookups(() => {
      if (this.vendorId && !isNaN(this.vendorId)) {
        this.mode = "update";
        this.loadVendor(this.vendorId);
        
      }
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  // =======================
  // FORM
  // =======================
  private buildForm(): void {
    this.registrationForm = this.fb.group({
      VendorName: ["", [Validators.required, Validators.minLength(3)]],
      Ntn: [""],
      RegistrationNumber: [""],
      IndustryTypeId: [null, Validators.required],
      BusinessTypeId: [null, Validators.required],
      VendorNatureId: [null, Validators.required],
      CurrencyId: [null, Validators.required],

      IsActive: [true],
      IsSubsidiary: [false],
      ParentVendorId: [null],
      RedList: [false],
      Ranking: [3, [Validators.required, Validators.min(1), Validators.max(5)]],

      ContactPhone: ["", [Validators.required, Validators.pattern("^[0-9]{7,15}$")]],
      ContactEmail: ["", [Validators.required, Validators.email]],
      ContactWebsite: [""],

      shippingZoneId: [null],
      shippingCountryId: [null],
      shippingStateId: [null],
      shippingCityId: [null],
      shippingZipcode: [""],
      shippingAddressLine1: ["", [Validators.required]],
      shippingAddressLine2: [""],

      billingZoneId: [null],
      billingCountryId: [null],
      billingStateId: [null],
      billingCityId: [null],
      billingZipcode: [""],
      billingAddressLine1: ["", [Validators.required]],
      billingAddressLine2: [""],

      clientCompanyIds: [[]],
      departmentIds: [[]],
    });
  }

  // =======================
 private buildTree(
  data: any[],
  labelKey: string,
  idKey: string,
  parentKey: string  // YE HAI
): TreeNode[] {
  if (!Array.isArray(data) || data.length === 0) return [];

  const map = new Map<string, TreeNode>();
  let hasHierarchy = false;

  data.forEach((item: any) => {
    const id = String(item[idKey]);
    map.set(id, {
      title: item[labelKey] || "Unknown",
      key: id,
      parentId: item[parentKey] ?? null,  // YE parentKey use karega
      children: [],
      expanded: false,
      isLeaf: false,
    });
  });

  const roots: TreeNode[] = [];
  map.forEach(node => {
    if (node.parentId != null && map.has(String(node.parentId))) {
      const parent = map.get(String(node.parentId))!;
      parent.children.push(node);
      parent.isLeaf = false;
      hasHierarchy = true;
    } else {
      roots.push(node);
    }
  });

  map.forEach(node => {
    if (node.children.length === 0) node.isLeaf = true;
  });


  
  return roots;
}

  // =======================
  // LOOKUPS (STATIC)
  // =======================
  private loadLookups(onComplete?: () => void): void {
    this.subscriptions.add(
      forkJoin({
        zones: this.companyService.getZones(),
        vendorNatures: this.vendorNatureService.getAllVendorNatures(),
        currencies: this.currencyService.getAllCurrencies(),
        industries: this.companyService.getAllIndustries(),
        businessTypes: this.companyService.getAllBusinessTypes(),
        companies: this.vendorService.getSelectableCompanies(),
        parentVendors: this.vendorService.getAllVendors("all"),
      }).subscribe({
        next: ({
          zones,
          vendorNatures,
          currencies,
          industries,
          businessTypes,
          companies,
          parentVendors,
        }) => {
          this.zones = zones || [];
          this.vendorNatures = vendorNatures?.data ?? [];
          this.currencies = currencies?.data ?? [];
          this.industryTypes = industries || [];
          this.businessTypes = businessTypes || [];

          // COMPANY TREE
          const companyList = (companies?.data || []).map((c: any) => ({
            id: c.id,
            title: c.companyName,
            parentCompanyId: c.parentCompanyId ?? null,
          }));
          this.companyTree = this.buildTree(companyList, "title", "id", "parentCompanyId");

  
          // PARENT VENDOR TREE
          const vendorList = Array.isArray(parentVendors) ? parentVendors : parentVendors?.data || [];
          this.parentVendorTree = this.buildTree(
            vendorList.map((v: any) => ({
              id: v.id,
              title: v.companyName ?? `Vendor #${v.id}`,
              parentVendorId: v.parentVendorId ?? null,
            })),
            "title",
            "id",
            "parentVendorId"
          );

          this.cdr.detectChanges();
          if (onComplete) onComplete();
        },
        error: () => {
          this.message.error("Failed to load data.");
          if (onComplete) onComplete();
        },
      })
    );
  }

  // =======================
  // DYNAMIC DEPARTMENT LOAD ON COMPANY SELECT
  // =======================
  private setupCompanyDepartmentFiltering(): void {
    const companyControl = this.registrationForm.get("clientCompanyIds");
    const deptControl = this.registrationForm.get("departmentIds");

    companyControl?.valueChanges.subscribe((selectedIds: string[]) => {
      if (!selectedIds || selectedIds.length === 0) {
        this.departmentTree = [];
        deptControl?.setValue([]);
        return;
      }

      this.isLoading = true;
      const requests = selectedIds.map(id => this.vendorService.getDepartmentsByCompany(Number(id)));

      this.subscriptions.add(
        forkJoin(requests).subscribe({
          next: (responses) => {
            const allDepts = responses
            
              .flatMap((res: any) => res?.data || [])
              .reduce((unique: any[], dept: any) => {
                if (!unique.some(d => d.id === dept.id)) unique.push(dept);
                return unique;
              }, []);
          const deptList = allDepts.map((d: any) => ({
  id: d.id,
  title: d.deptName,
  parentDepartmentId: d.parentDepartmentId ?? null,
}));
this.departmentTree = this.buildTree(deptList, "title", "id", "parentDepartmentId");
            const validDeptIds = (deptControl?.value || [])
              .filter((id: string) => allDepts.some((d: any) => String(d.id) === id))
              .map(String);

            deptControl?.setValue(validDeptIds, { emitEvent: false });

            this.isLoading = false;
            this.cdr.detectChanges();
          },
          error: () => {
            this.message.error("Failed to load departments.");
            this.isLoading = false;
          },
        })
      );
    });
  }

  // =======================
  // CASCADING DROPDOWNS
  // =======================
  private setupCascadingDropdowns(): void {
    const f = this.registrationForm;

    const resetAndLoad = (type: "shipping" | "billing", level: "zone" | "country" | "state", id?: number) => {
      if (!id) return;

      if (level === "zone") this.loadCountries(id, type);
      else if (level === "country") this.loadStates(id, type);
      else if (level === "state") this.loadCities(id, type);
    };

    // Shipping
    f.get("shippingZoneId")?.valueChanges.subscribe(id => {
      this.shippingCountries = this.shippingStates = this.shippingCities = [];
      f.patchValue({ shippingCountryId: null, shippingStateId: null, shippingCityId: null });
      resetAndLoad("shipping", "zone", id);
    });
    f.get("shippingCountryId")?.valueChanges.subscribe(id => {
      this.shippingStates = this.shippingCities = [];
      f.patchValue({ shippingStateId: null, shippingCityId: null });
      resetAndLoad("shipping", "country", id);
    });
    f.get("shippingStateId")?.valueChanges.subscribe(id => {
      this.shippingCities = [];
      f.patchValue({ shippingCityId: null });
      resetAndLoad("shipping", "state", id);
    });

    // Billing
    f.get("billingZoneId")?.valueChanges.subscribe(id => {
      this.billingCountries = this.billingStates = this.billingCities = [];
      f.patchValue({ billingCountryId: null, billingStateId: null, billingCityId: null });
      resetAndLoad("billing", "zone", id);
    });
    f.get("billingCountryId")?.valueChanges.subscribe(id => {
      this.billingStates = this.billingCities = [];
      f.patchValue({ billingStateId: null, billingCityId: null });
      resetAndLoad("billing", "country", id);
    });
    f.get("billingStateId")?.valueChanges.subscribe(id => {
      this.billingCities = [];
      f.patchValue({ billingCityId: null });
      resetAndLoad("billing", "state", id);
    });
  }

  private loadCountries(zoneId: number, type: "billing" | "shipping"): void {
    this.subscriptions.add(
      this.companyService.getCountriesByZone(zoneId).subscribe({
        next: data => {
          if (type === "billing") this.billingCountries = data || [];
          else this.shippingCountries = data || [];
        },
        error: () => this.message.error(`Failed to load countries for ${type}.`),
      })
    );
  }

  private loadStates(countryId: number, type: "billing" | "shipping"): void {
    this.subscriptions.add(
      this.companyService.getStatesByCountry(countryId).subscribe({
        next: data => {
          if (type === "billing") this.billingStates = data || [];
          else this.shippingStates = data || [];
        },
        error: () => this.message.error(`Failed to load states for ${type}.`),
      })
    );
  }

  private loadCities(stateId: number, type: "billing" | "shipping"): void {
    this.subscriptions.add(
      this.companyService.getCitiesByState(stateId).subscribe({
        next: data => {
          if (type === "billing") this.billingCities = data || [];
          else this.shippingCities = data || [];
        },
        error: () => this.message.error(`Failed to load cities for ${type}.`),
      })
    );
  }

  // =======================
  // LOAD VENDOR
  // =======================
private loadVendor(id: number): void {
  this.isLoading = true;
  this.subscriptions.add(
    this.vendorService.getVendorById(id).subscribe({
      next: (res: any) => {
        const vendor = res?.data ?? res;
        if (!vendor) {
          this.isLoading = false;
          return;
        }

        // 🧠 Smart and safe address parsing
        const parseAddress = (address: string | null): { line1: string; line2: string } => {
          if (!address) return { line1: "", line2: "" };
          const parts = address
            .split(",")
            .map(p => p.trim())
            .filter(p => p.length > 0);
          return {
            line1: parts[0] || "",
            line2: parts.slice(1).join(", ") || "",
          };
        };

        const ship = parseAddress(vendor.shippingAddress);
        const bill = parseAddress(vendor.billingAddress);

        this.registrationForm.patchValue({
          VendorName: vendor.companyName || "",
          Ntn: vendor.ntn || "",
          RegistrationNumber: vendor.registrationNumber || "",
          IndustryTypeId: vendor.industryTypeId ?? null,
          BusinessTypeId: vendor.businessTypeId ?? null,
          VendorNatureId: vendor.vendorNatureId ?? null,
          CurrencyId: vendor.currencyId ?? null,
          IsActive: vendor.isActive,
          IsSubsidiary: vendor.isSubsidiary,
          ParentVendorId: vendor.parentVendorId ? String(vendor.parentVendorId) : null,
          RedList: vendor.redList,
          Ranking: vendor.ranking,
          ContactPhone: vendor.contactPhone || "",
          ContactEmail: vendor.contactEmail || "",
          ContactWebsite: vendor.contactWebsiteUrl || "",
          shippingZipcode: vendor.shippingZipcode || "",
          billingZipcode: vendor.billingZipcode || "",
          clientCompanyIds: (vendor.clientCompanyIds || []).map(String),
          departmentIds: (vendor.departmentIds || []).map(String),

          // ✅ Address handling (always populate both)
          shippingAddressLine1: vendor.shippingAddressLine1 || ship.line1,
          shippingAddressLine2: vendor.shippingAddressLine2 || ship.line2,
          billingAddressLine1: vendor.billingAddressLine1 || bill.line1,
          billingAddressLine2: vendor.billingAddressLine2 || bill.line2,

          shippingZoneId: vendor.shippingZoneId ?? null,
          shippingCountryId: vendor.shippingCountryId ?? null,
          shippingStateId: vendor.shippingStateId ?? null,
          shippingCityId: vendor.shippingCityId ?? null,
          billingZoneId: vendor.billingZoneId ?? null,
          billingCountryId: vendor.billingCountryId ?? null,
          billingStateId: vendor.billingStateId ?? null,
          billingCityId: vendor.billingCityId ?? null,
        });

        // 🔧 Force detect changes right after patching
        this.cdr.detectChanges();

        // 🔁 Load departments for selected companies
        const companyIds = (vendor.clientCompanyIds || []).map(String);
        if (companyIds.length > 0) {
          this.loadDepartmentsForCompanies(companyIds);
        }

        // 🌍 Load cascading location data
        const requests: any[] = [];
        if (vendor.shippingZoneId)
          requests.push(this.companyService.getCountriesByZone(vendor.shippingZoneId));
        if (vendor.shippingCountryId)
          requests.push(this.companyService.getStatesByCountry(vendor.shippingCountryId));
        if (vendor.shippingStateId)
          requests.push(this.companyService.getCitiesByState(vendor.shippingStateId));
        if (vendor.billingZoneId)
          requests.push(this.companyService.getCountriesByZone(vendor.billingZoneId));
        if (vendor.billingCountryId)
          requests.push(this.companyService.getStatesByCountry(vendor.billingCountryId));
        if (vendor.billingStateId)
          requests.push(this.companyService.getCitiesByState(vendor.billingStateId));

        if (requests.length > 0) {
          forkJoin(requests).subscribe(results => {
            let i = 0;
            if (vendor.shippingZoneId) this.shippingCountries = results[i++] || [];
            if (vendor.shippingCountryId) this.shippingStates = results[i++] || [];
            if (vendor.shippingStateId) this.shippingCities = results[i++] || [];
            if (vendor.billingZoneId) this.billingCountries = results[i++] || [];
            if (vendor.billingCountryId) this.billingStates = results[i++] || [];
            if (vendor.billingStateId) this.billingCities = results[i++] || [];
            this.cdr.detectChanges();
          });
        }

        this.isLoading = false;
      },
      error: () => {
        this.message.error("Failed to load vendor.");
        this.isLoading = false;
      },
    })
  );
}


  private loadDepartmentsForCompanies(companyIds: string[]): void {
    if (!companyIds.length) return;

    const requests = companyIds.map(id => this.vendorService.getDepartmentsByCompany(Number(id)));

    this.subscriptions.add(
      forkJoin(requests).subscribe({
        next: (responses) => {
          const allDepts = responses.flatMap((res: any) => res?.data || []);
          const deptList = allDepts.map((d: any) => ({
            id: d.id,
            title: d.deptName,
      parentDepartmentId: d.parentDepartmentId ?? null,
          }));
     this.departmentTree = this.buildTree(deptList, "title", "id", "parentDepartmentId"); // YE BHI CHANGE
          this.cdr.detectChanges();
        },
        error: () => {
          this.message.error("Failed to load departments.");
        }
      })
    );
  }

  // =======================
  // SAVE
  // =======================
  saveVendor(): void {
    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();
      this.message.warning("Please fill all required fields.");
      return;
      
    }

    const f = this.registrationForm.value;
    const payload: VendorCreateDto = {
      VendorName: f.VendorName,
      Ntn: f.Ntn || null,
      RegistrationNumber: f.RegistrationNumber || null,
      IndustryTypeId: f.IndustryTypeId ?? null,
      BusinessTypeId: f.BusinessTypeId ?? null,
      VendorNatureId: f.VendorNatureId ?? null,
      CurrencyId: f.CurrencyId ?? null,
      IsActive: !!f.IsActive,
      IsSubsidiary: !!f.IsSubsidiary,
      ParentVendorId: f.ParentVendorId ? Number(f.ParentVendorId) : null,
      RedList: !!f.RedList,
      Ranking: +f.Ranking,
      ContactPhone: f.ContactPhone,
      ContactEmail: f.ContactEmail,
      ContactWebsite: f.ContactWebsite || null,
      ShippingAddressLine1: f.shippingAddressLine1,
      ShippingAddressLine2: f.shippingAddressLine2 || null,
      ShippingZipcode: f.shippingZipcode || null,
      ShippingZoneId: f.shippingZoneId ?? null,
      ShippingCountryId: f.shippingCountryId ?? null,
      ShippingStateId: f.shippingStateId ?? null,
      ShippingCityId: f.shippingCityId ?? null,
      BillingAddressLine1: f.billingAddressLine1,
      BillingAddressLine2: f.billingAddressLine2 || null,
      BillingZipcode: f.billingZipcode || null,
      BillingZoneId: f.billingZoneId ?? null,
      BillingCountryId: f.billingCountryId ?? null,
      BillingStateId: f.billingStateId ?? null,
      BillingCityId: f.billingCityId ?? null,
      ClientCompanyIds: (f.clientCompanyIds || []).map(Number),
      DepartmentIds: (f.departmentIds || []).map(Number),
    };

    this.isLoading = true;
    const save$ = this.vendorId
      ? this.vendorService.updateVendor(this.vendorId, { Id: this.vendorId, ...payload } as VendorUpdateDto)
      : this.vendorService.createVendor(payload);

    this.subscriptions.add(
      save$.subscribe({
        next: () => {
          this.message.success(this.vendorId ? "Vendor updated!" : "Vendor created!");
          this.router.navigate(["/lst-vendor"]);
        },
        error: () => {
          this.message.error("Failed to save vendor.");
          this.isLoading = false;
        },
      })
    );
  }

  closeModal(): void {
    this.router.navigate(["/lst-vendor"]);
  }

  // =======================
  // EXPAND/COLLAPSE HANDLER
  // =======================
  onExpandChange(event: any, type: 'company' | 'department' | 'vendor'): void {
    // ng-zorro handles expand state automatically
    // Optional: lazy load children here
  }

  // =======================
  // LABEL HELPER
  // =======================
  public getLabelById(id: string | number, type: "company" | "department" | "vendor"): string {
    const key = String(id);
    const findInTree = (tree: TreeNode[]): string | null => {
      for (const node of tree) {
        if (node.key === key) return node.title;
        if (node.children.length > 0) {
          const found: string | null = findInTree(node.children);
          if (found) return found;
        }
      }
      return null;
    };

    if (type === "company") return findInTree(this.companyTree) ?? "—";
    if (type === "department") return findInTree(this.departmentTree) ?? "—";
    if (type === "vendor") return findInTree(this.parentVendorTree) ?? "—";
    return "—";
  }

  showAlert(type: "success" | "error" | "warning", message: string): void {
    this.alert = { type, message };
    setTimeout(() => {
      this.alert = { type: "", message: "" };
    }, 5000);
  }




  copyBillingToShipping(): void {
  const f = this.registrationForm;

  f.patchValue({
    shippingAddressLine1: f.get('billingAddressLine1')?.value,
    shippingAddressLine2: f.get('billingAddressLine2')?.value,
    shippingZipcode: f.get('billingZipcode')?.value,

    shippingZoneId: f.get('billingZoneId')?.value,
    shippingCountryId: f.get('billingCountryId')?.value,
    shippingStateId: f.get('billingStateId')?.value,
    shippingCityId: f.get('billingCityId')?.value,
  });

  // Load dropdown lists (countries, states, cities)
  if (f.get('billingZoneId')?.value) {
    this.loadCountries(f.get('billingZoneId')?.value, "shipping");
  }

  if (f.get('billingCountryId')?.value) {
    this.loadStates(f.get('billingCountryId')?.value, "shipping");
  }

  if (f.get('billingStateId')?.value) {
    this.loadCities(f.get('billingStateId')?.value, "shipping");
  }

  this.message.success("Billing address copied to Shipping address!");
}

}