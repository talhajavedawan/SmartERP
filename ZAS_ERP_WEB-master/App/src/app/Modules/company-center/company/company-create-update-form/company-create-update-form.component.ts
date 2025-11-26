import { Component, OnInit, ViewChild } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
} from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { HttpClient } from "@angular/common/http";
import { NzTableModule, NzTableComponent } from "ng-zorro-antd/table";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzTagComponent } from "ng-zorro-antd/tag";
import { NzIconModule } from "ng-zorro-antd/icon";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import {
  BusinessType,
  CompanyService,
  IndustryType,
} from "../../../../shared/services/company-center/company/company.service";
import { GroupService } from "../../../../shared/services/company-center/groups/group.service";

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
  groupId?: number | null;
  companyType?: number | null;
  isActive: boolean;
  isSubsidiary?: boolean;
  parentCompanyId?: number | null;

  address?: {
    zoneId?: number;
    countryId?: number;
    stateId?: number;
    cityId?: number;
    zipcode?: string;
    addressLine1?: string;
    addressLine2?: string | null;
    // 👇 these four lines must be here
    zone?: { id: number; name: string };
    country?: { id: number; name: string };
    state?: { id: number; name: string };
    city?: { id: number; name: string };
  };

  contact?: {
    phoneNumber?: string;
    email?: string;
    websiteUrl?: string;
  };

  businessType?: { id: number; businessTypeName: string };
  industryType?: { id: number; industryTypeName: string };
}

@Component({
  selector: "app-company-create-update-form",
  standalone: true,
  templateUrl: "./company-create-update-form.component.html",
  styleUrls: ["./company-create-update.css"], // ✅ Angular expects plural
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzTabsModule,
    NzTableModule,
    NzButtonModule,
    NzTagComponent,
    NzIconModule,
  ],
})
export class CompanyCreateUpdateFormComponent implements OnInit {
  @ViewChild("parentCompanyTable")
  parentCompanyTable!: NzTableComponent<TreeNode>;

  /** Tabs: 0 = Company, 1 = Address, 2 = Contact */
  selectedIndex = 0;

  zones: Location[] = [];
  countries: Location[] = [];
  states: Location[] = [];
  cities: Location[] = [];

  registrationForm!: FormGroup;
  submitted = false;

  mode: "create" | "update" = "create";
  companyId: number | null = null;
  isLoading = false;

  companyTypeOptions = [
    { id: 1, name: "Group Company" },
    { id: 2, name: "Individual Company" },
  ];

  showSubsidiaryFields = false;
  showParentCompanyGrid = false;
  showGroupDropdown = false;
  alert = { type: "" as "success" | "error" | "warning" | "", message: "" };

  selectedParentCompany: TreeNode | null = null;
  companies: TreeNode[] = [];
  companyTree: TreeNode[] = [];
  flattenedTree: TreeNode[] = [];

  businessTypes: BusinessType[] = [];
  industryTypes: IndustryType[] = [];
  groups: any[] = [];

  // alert = { type: "" as "success" | "error" | "", message: "" };

  constructor(
    private fb: FormBuilder,
    private companyService: CompanyService,
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private groupService: GroupService
  ) {}

  // ---------- Convenience getters for template ----------
  get f(): { [key: string]: AbstractControl } {
    return this.registrationForm?.controls ?? {};
  }

  ngOnInit(): void {
    this.initializeForm();
    this.loadBusinessTypes();
    this.loadIndustryTypes();
    this.loadCompanies();
    this.loadZones();

    // ❌ Removed stray `this.companyService.getAllCompanies()` that did nothing

    this.route.paramMap.subscribe((params) => {
      const id = params.get("id");
      if (id) {
        this.mode = "update";
        this.companyId = +id;
        this.loadCompany(this.companyId);
      }
    });
  }

  /** =========================== FORM INITIALIZATION ============================ */
  initializeForm() {
    // Common patterns (kept your intent, made a tad stricter/consistent)
    const numberOnly = /^[0-9]+$/;
    const ntnPattern = /^[0-9]{3,15}$/;
    const zipPattern = /^[0-9]{4,10}$/;
    const phonePattern = /^[0-9]{10,15}$/;
    // Basic URL pattern (http/https optional, domain required; path optional)
    const urlPattern =
      /^(https?:\/\/)?([A-Za-z0-9-]+\.)+[A-Za-z]{2,}(\/[^\s]*)?$/;

    this.registrationForm = this.fb.group({
      CompanyName: [
        "",
        Validators.compose([
          Validators.required,
          Validators.minLength(3),
          this.noOnlySpacesValidator,
        ]),
      ],
      IndustryTypeId: ["", Validators.required],
      BusinessTypeId: ["", Validators.required],
      CompanyType: ["", Validators.required],
      IsActive: [true],
      GroupId: [null],
      Ntn: [
        "",
        Validators.compose([
          Validators.required,
          Validators.pattern(ntnPattern),
        ]),
      ],

      // Address
      zoneId: ["", Validators.required],
      countryId: ["", Validators.required],
      stateId: ["", Validators.required],
      cityId: ["", Validators.required],
      zipCode: [
        "",
        Validators.compose([
          Validators.required,
          Validators.pattern(zipPattern),
        ]),
      ],
      addressLine1: [
        "",
        Validators.compose([Validators.required, this.noOnlySpacesValidator]),
      ],
      addressLine2: [""],

      // Contact
      phoneNumber: [
        "",
        Validators.compose([
          Validators.required,
          Validators.pattern(phonePattern),
        ]),
      ],
      Email: ["", Validators.compose([Validators.required, Validators.email])],
      WebsiteUrl: [""],

      // Relations
      IsSubsidiary: [false],
      ParentCompanyId: [null],
    });

    // Toggle group dropdown on company type
    this.registrationForm.get("CompanyType")?.valueChanges.subscribe((val) => {
      this.showGroupDropdown = Number(val) === 1;
      if (this.showGroupDropdown) {
        this.loadGroups();
      } else {
        this.registrationForm.patchValue({ GroupId: null });
      }
    });

    // Toggle parent company selection
    this.registrationForm
      .get("IsSubsidiary")
      ?.valueChanges.subscribe((val: boolean) => {
        this.showSubsidiaryFields = val;
        if (!val) {
          this.registrationForm.patchValue({ ParentCompanyId: null });
          this.selectedParentCompany = null;
        }
      });
  }

  // Prevents strings that are only whitespace
  private noOnlySpacesValidator(control: AbstractControl) {
    const value = (control.value ?? "") as string;
    if (typeof value === "string" && value.trim().length === 0) {
      return { whitespaceOnly: true };
    }
    return null;
  }

  onZoneChange(event: Event) {
    const select = event.target as HTMLSelectElement;
    const zoneId = Number(select.value);
    if (zoneId) {
      this.resetLocation("zone");
      this.loadCountries(zoneId);
    } else {
      this.countries = [];
      this.states = [];
      this.cities = [];
    }
  }

  onCountryChange(event: Event) {
    const select = event.target as HTMLSelectElement;
    const countryId = Number(select.value);
    if (countryId) {
      this.resetLocation("country");
      this.loadStates(countryId);
    } else {
      this.states = [];
      this.cities = [];
    }
  }

  onStateChange(event: Event) {
    const select = event.target as HTMLSelectElement;
    const stateId = Number(select.value);
    if (stateId) {
      this.resetLocation("state");
      this.loadCities(stateId);
    } else {
      this.cities = [];
    }
  }

  /** =========================== LOADERS ============================ */
  loadGroups() {
    this.groupService.getAllGroups().subscribe({
      next: (data) => (this.groups = Array.isArray(data) ? data : []),
      error: () => this.setAlert("error", "Failed to load groups"),
    });
  }

  loadZones() {
    this.companyService.getZones().subscribe({
      next: (data) => (this.zones = Array.isArray(data) ? data : []),
      error: () => (this.zones = []),
    });
  }

  loadCountries(zoneId: number) {
    this.companyService.getCountriesByZone(zoneId).subscribe({
      next: (data) => (this.countries = Array.isArray(data) ? data : []),
      error: () => (this.countries = []),
    });
  }

  loadStates(countryId: number) {
    this.companyService.getStatesByCountry(countryId).subscribe({
      next: (data) => (this.states = Array.isArray(data) ? data : []),
      error: () => (this.states = []),
    });
  }

  loadCities(stateId: number) {
    this.companyService.getCitiesByState(stateId).subscribe({
      next: (data) => (this.cities = Array.isArray(data) ? data : []),
      error: () => (this.cities = []),
    });
  }

  resetLocation(level: "zone" | "country" | "state") {
    if (level === "zone") {
      this.countries = [];
      this.states = [];
      this.cities = [];
      this.registrationForm.patchValue({
        countryId: "",
        stateId: "",
        cityId: "",
      });
    } else if (level === "country") {
      this.states = [];
      this.cities = [];
      this.registrationForm.patchValue({ stateId: "", cityId: "" });
    } else if (level === "state") {
      this.cities = [];
      this.registrationForm.patchValue({ cityId: "" });
    }
  }

  /** =========================== COMPANY LOGIC ============================ */
  loadBusinessTypes() {
    this.companyService.getAllBusinessTypes().subscribe({
      next: (data) => (this.businessTypes = Array.isArray(data) ? data : []),
      error: () => this.setAlert("error", "Failed to load business types"),
    });
  }

  loadIndustryTypes() {
    this.companyService.getAllIndustries().subscribe({
      next: (data) => (this.industryTypes = Array.isArray(data) ? data : []),
      error: () => this.setAlert("error", "Failed to load industry types"),
    });
  }

  buildCompanyTree(flatCompanies: Company[]): TreeNode[] {
    const map = new Map<number, TreeNode>();
    const roots: TreeNode[] = [];

    // Create all nodes
    flatCompanies.forEach((c) => {
      map.set(c.id, {
        ...c,
        children: [],
        isExpanded: false,
        level: 0,
      });
    });

    // Assign children & build hierarchy
    flatCompanies.forEach((c) => {
      const node = map.get(c.id)!;
      if (c.parentCompanyId && map.has(c.parentCompanyId)) {
        const parent = map.get(c.parentCompanyId)!;
        node.level = (parent.level ?? 0) + 1;
        parent.children!.push(node);
      } else {
        roots.push(node);
      }
    });

    return roots;
  }

  // keep references (no cloning)
  flattenTree(nodes: TreeNode[]): TreeNode[] {
    const flat: TreeNode[] = [];

    const walk = (items: TreeNode[], level = 0) => {
      for (const item of items) {
        item.level = level;
        flat.push(item);
        if (item.isExpanded && item.children?.length) {
          walk(item.children, level + 1);
        }
      }
    };

    walk(nodes);
    return flat;
  }

  toggleExpand(company: TreeNode): void {
    company.isExpanded = !company.isExpanded;
    this.flattenedTree = this.flattenTree(this.companyTree);
  }

  loadCompanies() {
    this.companyService.getAllCompanies().subscribe({
      next: (companies: any) => {
        const flatData: Company[] = Array.isArray(companies)
          ? companies
          : companies?.$values || [];

        this.companyTree = this.buildCompanyTree(flatData);
        this.flattenedTree = this.flattenTree(this.companyTree); // collapsed initially
      },
      error: () => this.setAlert("error", "Failed to load companies list"),
    });
  }

  loadCompany(id: number) {
    this.companyService.getCompanyById(id).subscribe({
      next: (company) => this.populateForm(company),
      error: () => this.setAlert("error", "Failed to load company"),
    });
  }

  populateForm(company: Company) {
    const address = company.address || {};
    const contact = company.contact || {};

    this.registrationForm.patchValue({
      CompanyName: company.companyName?.trim() || "",
      IndustryTypeId: company.industryTypeId || "",
      BusinessTypeId: company.businessTypeId || "",
      CompanyType: company.companyType ?? "",
      GroupId: company.groupId ?? null,
      Ntn: company.ntn ?? "",
      IsActive: !!company.isActive,

      // ✅ Address flattening
      zoneId: address.zoneId || address.zone?.id || "",
      countryId: address.countryId || address.country?.id || "",
      stateId: address.stateId || address.state?.id || "",
      cityId: address.cityId || address.city?.id || "",
      zipCode: address.zipcode || "",
      addressLine1: address.addressLine1 || "",
      addressLine2: address.addressLine2 || "",

      // ✅ Contact flattening
      phoneNumber: contact.phoneNumber || "",
      Email: contact.email || "",
      WebsiteUrl: contact.websiteUrl || "",

      IsSubsidiary: company.isSubsidiary || false,
      ParentCompanyId: company.parentCompanyId ?? null,
    });

    // ✅ UI toggles
    this.showGroupDropdown = Number(company.companyType) === 1;
    if (this.showGroupDropdown) this.loadGroups();
    this.showSubsidiaryFields = !!company.isSubsidiary;

    // ✅ Safe numeric extractions for dependent dropdowns
    const zoneId = address.zoneId ?? address.zone?.id ?? null;
    const countryId = address.countryId ?? address.country?.id ?? null;
    const stateId = address.stateId ?? address.state?.id ?? null;

    if (zoneId != null) {
      this.loadCountries(Number(zoneId));
    }

    if (countryId != null) {
      this.loadStates(Number(countryId));
    }

    if (stateId != null) {
      this.loadCities(Number(stateId));
    }

    // ✅ Repatch after async loads
    setTimeout(() => {
      this.registrationForm.patchValue({
        zoneId: address.zoneId || address.zone?.id || "",
        countryId: address.countryId || address.country?.id || "",
        stateId: address.stateId || address.state?.id || "",
        cityId: address.cityId || address.city?.id || "",
      });
    }, 500);
  }

  /** =========================== SUBMIT ============================ */
  submitForm() {
    this.submitted = true;

    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();

      // Try to scroll to the first invalid form control
      const firstInvalid: HTMLElement | null =
        document.querySelector("form .ng-invalid, .ant-form-item-has-error") ||
        document.querySelector(".ng-invalid");

      if (firstInvalid) {
        firstInvalid.scrollIntoView({ behavior: "smooth", block: "center" });
        // focusing helps screen readers & keyboard users
        (firstInvalid as any)?.focus?.();
      }

      this.setAlert(
        "error",
        "❌ Please correct highlighted fields before submitting."
      );
      return;
    }

    this.isLoading = true;
    const payload = this.buildPayload();

    if (this.mode === "create") this.createCompany(payload);
    else if (this.companyId) this.updateCompany(this.companyId, payload);
  }

  buildPayload() {
    const f: any = this.registrationForm.value;

    const toNum = (v: any): number => Number(v);
    const toNumOrNull = (v: any): number | null =>
      v === "" || v === null || v === undefined ? null : Number(v);

    return {
      companyName: String(f.CompanyName || "").trim(),
      industryTypeId: toNum(f.IndustryTypeId),
      businessTypeId: toNum(f.BusinessTypeId),
      companyType: toNum(f.CompanyType),
      groupId: toNumOrNull(f.GroupId),
      ntn: String(f.Ntn || ""),
      isActive: !!f.IsActive,
      isSubsidiary: !!f.IsSubsidiary,
      parentCompanyId: f.IsSubsidiary ? toNumOrNull(f.ParentCompanyId) : null,
      address: {
        zoneId: toNum(f.zoneId),
        countryId: toNum(f.countryId),
        stateId: toNum(f.stateId),
        cityId: toNum(f.cityId),
        zipcode: String(f.zipCode || ""),
        addressLine1: String(f.addressLine1 || "").trim(),
        addressLine2: (f.addressLine2 ?? "").toString().trim() || null,
      },
      contact: {
        phoneNumber: String(f.phoneNumber || ""),
        email: String(f.Email || "").toLowerCase(),
        websiteUrl: String(f.WebsiteUrl || ""),
      },
    };
  }

  createCompany(payload: any) {
    this.companyService.createCompany(payload).subscribe({
      next: () => this.handleSuccess(),
      error: (err) => this.handleError(err),
    });
  }

  updateCompany(id: number, payload: any) {
    this.companyService.updateCompany(id, payload).subscribe({
      next: () => this.handleSuccess(),
      error: (err) => this.handleError(err),
    });
  }

  /** =========================== HELPERS ============================ */
  clearParentCompany(): void {
    this.selectedParentCompany = null;
    this.registrationForm.patchValue({ ParentCompanyId: null });
  }

  toggleParentCompanyDropdown() {
    this.showParentCompanyGrid = !this.showParentCompanyGrid;
    if (this.showParentCompanyGrid) {
      // use already loaded companies
      this.flattenedTree = this.flattenedTree.length ? this.flattenedTree : [];
    }
  }

  /** show node only if all its ancestors are expanded */
  shouldShowNode(node: TreeNode): boolean {
    let current: any = node;
    const findParent = (id: number | null) =>
      this.companyTree.find((root) => root.id === id) ||
      this.findInTree(this.companyTree, id);

    while (current.parentCompanyId) {
      const parent = findParent(current.parentCompanyId);
      if (!parent?.isExpanded) return false;
      current = parent;
    }
    return true;
  }

  findInTree(nodes: TreeNode[], id: number | null): TreeNode | undefined {
    for (const node of nodes) {
      if (node.id === id) return node;
      if (node.children?.length) {
        const found = this.findInTree(node.children, id);
        if (found) return found;
      }
    }
    return undefined;
  }

  flattenNodes(nodes: TreeNode[]) {
    nodes.forEach((n) => {
      this.flattenedTree.push(n);
      if (n.isExpanded && n.children?.length) this.flattenNodes(n.children);
    });
  }

  handleSuccess() {
    this.isLoading = false;
    this.setAlert(
      "success",
      `✅ Company ${
        this.mode === "create" ? "created" : "updated"
      } successfully.`
    );
    setTimeout(() => this.router.navigate(["/company-list"]), 2500);
  }

  handleError(err?: any) {
    this.isLoading = false;
    this.setAlert(
      "error",
      err?.error?.message || "❌ Something went wrong. Please try again."
    );
  }

  // setAlert(type: "success" | "error", message: string) {
  //   this.alert = { type, message };
  //   window.scrollTo({ top: 0, behavior: "smooth" });
  //   setTimeout(
  //     () => (this.alert = { type: "", message: "" }),
  //     type === "success" ? 2500 : 5000
  //   );
  // }

  closePopup() {
    this.router.navigate(["/company-list"]);
  }

  selectParentCompany(company: any): void {
    this.selectedParentCompany = company;
    this.registrationForm.patchValue({ ParentCompanyId: company.id });
    this.showParentCompanyGrid = false;
  }

  setAlert(type: "success" | "error" | "warning", message: string) {
    this.alert = { type, message };
    window.scrollTo({ top: 0, behavior: "smooth" });
    setTimeout(
      () => (this.alert = { type: "", message: "" }),
      type === "success" ? 2500 : 5000
    );
  }
}
