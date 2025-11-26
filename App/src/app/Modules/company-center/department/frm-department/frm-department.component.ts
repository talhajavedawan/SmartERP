import { Component, OnInit, OnDestroy } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
} from "@angular/forms";
import { CommonModule } from "@angular/common";
import {
  DepartmentService,
  Department,
  DepartmentEmployee,
} from "../../../../shared/services/department.service";
import {
  CompanyService,
  CompanyList,
} from "../../../../shared/services/company-center/company/company.service";
import {
  EmployeeService,
  Employee,
} from "../../../../shared/services/employee.service";
import { Router, ActivatedRoute } from "@angular/router";
import { forkJoin, map, Subscription, tap } from "rxjs";
import { NzMessageService } from "ng-zorro-antd/message";
import { NzTreeSelectModule } from "ng-zorro-antd/tree-select";
import { NzTreeNodeOptions } from "ng-zorro-antd/tree";
import { NzTabsModule } from "ng-zorro-antd/tabs";
import { formatPKTDate } from "../../../../shared/components/dateTime.util";

// Custom validator: at least one company must be selected
function minSelectedCompanies(min: number): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const companies = control.value as CompanyList[];
    return companies && companies.length >= min
      ? null
      : { minCompanies: { required: min, actual: companies?.length || 0 } };
  };
}

@Component({
  selector: "app-frm-department",
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzTreeSelectModule,
    NzTabsModule,
  ],
  templateUrl: "./frm-department.component.html",
  styleUrls: [
    "../../../../../scss/forms.css",
    "./frm-department.component.css",
  ],
})
export class FrmDepartmentComponent implements OnInit, OnDestroy {
  departmentForm!: FormGroup;
  selectedTabIndex = 0;
  isLoading = false;
  departmentId?: number;
  departments: Department[] = [];
  departmentTreeNodes: NzTreeNodeOptions[] = [];
  showParentDropdown = false;

  // Companies & Employees for dual list
  allCompanies: CompanyList[] = [];
  filteredCompanies: CompanyList[] = [];
  selectedCompanies: CompanyList[] = [];
  allEmployees: Employee[] = [];
  filteredEmployees: Employee[] = [];
  selectedEmployees: Employee[] = [];
  companySearch: string = "";
  employeeSearch: string = "";
  private subscriptions: Subscription = new Subscription();

  // Store original department data for audit display
  originalDepartment: Department | null = null;
  formatPKTDate = formatPKTDate;
  private isInitialLoad: boolean = true;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private deptService: DepartmentService,
    private companyService: CompanyService,
    private employeeService: EmployeeService,
    private route: ActivatedRoute,
    private message: NzMessageService
  ) {}

  private buildDepartmentTree(departments: Department[]): NzTreeNodeOptions[] {
    const departmentMap = new Map<number, NzTreeNodeOptions>();
    const roots: NzTreeNodeOptions[] = [];

    for (const dept of departments) {
      departmentMap.set(dept.id, {
        title: dept.deptName,
        key: dept.id.toString(),
        children: [],
        isLeaf: true,
      });
    }

    for (const dept of departments) {
      const node = departmentMap.get(dept.id)!;
      if (
        dept.parentDepartmentId &&
        departmentMap.has(dept.parentDepartmentId)
      ) {
        const parentNode = departmentMap.get(dept.parentDepartmentId)!;
        parentNode.isLeaf = false;
        parentNode.children!.push(node);
      } else {
        roots.push(node);
      }
    }

    const clean = (nodes: NzTreeNodeOptions[]) => {
      for (const n of nodes) {
        if (n.children && n.children.length === 0) delete n.children;
        else if (n.children) clean(n.children);
      }
    };
    clean(roots);

    return roots;
  }

  ngOnInit(): void {
    this.departmentForm = this.fb.group({
      deptName: ["", [Validators.required, Validators.maxLength(100)]],
      deptCode: ["", [Validators.required, Validators.maxLength(50)]],
      abbreviation: ["", Validators.maxLength(20)],
      isActive: [true],
      isSubsidiary: [false],
      parentDepartmentId: [null],
      selectedCompanies: [[], [minSelectedCompanies(1)]],
    });

    // Sync selectedCompanies
    this.departmentForm
      .get("selectedCompanies")
      ?.setValue(this.selectedCompanies);
    this.departmentForm
      .get("selectedCompanies")
      ?.valueChanges.subscribe((value) => {
        this.selectedCompanies = value;
      });

    // React to subsidiary & parent changes
    this.subscriptions.add(
      this.departmentForm
        .get("isSubsidiary")
        ?.valueChanges.subscribe((value) => {
          this.showParentDropdown = value;
          if (!value) {
            this.departmentForm.patchValue({ parentDepartmentId: null });
            this.loadAllCompanies();
            this.loadAllEmployees();
          } else {
            const parentId =
              this.departmentForm.get("parentDepartmentId")?.value;
            if (parentId) {
              this.updateCompaniesForParent();
              this.updateEmployeesForParent();
            }
          }
        })
    );

    this.subscriptions.add(
      this.departmentForm
        .get("parentDepartmentId")
        ?.valueChanges.subscribe((parentId) => {
          if (this.departmentForm.get("isSubsidiary")?.value && parentId) {
            this.updateCompaniesForParent();
            this.updateEmployeesForParent();
          }
        })
    );

    this.departmentId =
      Number(this.route.snapshot.paramMap.get("id")) || undefined;

    const lists$ = forkJoin({
      departments: this.deptService.getDepartments(),
      companies: this.companyService.getAllCompanies("active"),
      employees: this.employeeService.getAllEmployees(),
    }).pipe(
      map((res: any) => ({
        departments: res.departments || [],
        companies: (res.companies || []).filter(
          (c: any) => c.companyType === 1 || c.companyType === 2
        ),
        employees: res.employees || [],
      }))
    );

    if (this.departmentId) {
      forkJoin({
        lists: lists$,
        department: this.deptService.getDepartmentById(this.departmentId),
      })
        .pipe(
          tap((res) => {
            const { departments, companies, employees } = res.lists;
            this.originalDepartment = res.department ?? null;
            this.handleDataLoad(
              departments,
              Array.isArray(companies) ? companies : companies ?? [],
              Array.isArray(employees) ? employees : employees ?? [],
              this.originalDepartment
            );
            this.isInitialLoad = false;
          })
        )
        .subscribe({
          error: (err) => {
            console.error("Error loading edit data:", err);
            this.isLoading = false;
            this.message.error("Failed to load department data.");
          },
        });
    } else {
      const state = window.history.state;
      const departmentFromState = state?.department as Department | undefined;

      if (departmentFromState) {
        this.departmentId = departmentFromState.id;
        this.originalDepartment = departmentFromState;
        lists$
          .pipe(
            tap((listsRes) => {
              this.handleDataLoad(
                listsRes.departments,
                Array.isArray(listsRes.companies)
                  ? listsRes.companies
                  : listsRes.companies ?? [],
                Array.isArray(listsRes.employees)
                  ? listsRes.employees
                  : listsRes.employees ?? [],
                departmentFromState
              );
              this.isInitialLoad = false;
            })
          )
          .subscribe({
            error: (err) => {
              console.error("Error loading lists from state:", err);
              this.isLoading = false;
              this.message.error("Failed to load department data.");
            },
          });
      } else {
        lists$
          .pipe(
            tap((listsRes) => {
              this.handleDataLoad(
                listsRes.departments,
                Array.isArray(listsRes.companies)
                  ? listsRes.companies
                  : listsRes.companies ?? [],
                Array.isArray(listsRes.employees)
                  ? listsRes.employees
                  : listsRes.employees ?? [],
                null
              );
              this.isInitialLoad = false;
            })
          )
          .subscribe({
            error: (err) => {
              console.error("Error loading lists:", err);
              this.isLoading = false;
              this.message.error("Failed to load lists.");
            },
          });
      }
    }
  }

  private loadAllCompanies() {
    this.companyService.getAllCompanies("active").subscribe({
      next: (companies) => {
        this.allCompanies = (companies || []).filter(
          (c: any) => c.companyType === 1 || c.companyType === 2
        );
        if (!this.departmentForm.get("isSubsidiary")?.value) {
          this.preserveSelectedCompanies();
        }
        this.filterCompanies();
      },
      error: () => this.message.error("Failed to load companies."),
    });
  }

  private loadAllEmployees() {
    this.employeeService.getAllEmployees().subscribe({
      next: (employees) => {
        this.allEmployees = Array.isArray(employees)
          ? employees
          : employees ?? [];
        if (!this.departmentForm.get("isSubsidiary")?.value) {
          this.preserveSelectedEmployees();
        }
        this.filterEmployees();
      },
      error: () => this.message.error("Failed to load employees."),
    });
  }

  private getDescendantIds(
    departments: Department[],
    parentId: number
  ): number[] {
    const childIds = departments
      .filter((d) => d.parentDepartmentId === parentId)
      .map((d) => d.id);
    let allDescendants: number[] = [...childIds];
    for (const childId of childIds) {
      allDescendants = allDescendants.concat(
        this.getDescendantIds(departments, childId)
      );
    }
    return allDescendants;
  }

  private updateCompaniesForParent() {
    const parentId = Number(
      this.departmentForm.get("parentDepartmentId")?.value
    );
    if (!parentId) {
      this.allCompanies = [];
      this.filteredCompanies = [];
      if (this.departmentForm.get("isSubsidiary")?.value) {
        this.selectedCompanies = [];
        this.departmentForm.get("selectedCompanies")?.setValue([]);
      }
      return;
    }

    this.deptService.getDepartmentById(parentId).subscribe({
      next: (parentDepartment) => {
        this.allCompanies = parentDepartment.companies ?? [];
        if (this.departmentForm.get("isSubsidiary")?.value) {
          this.selectedCompanies = this.selectedCompanies.filter((s) =>
            this.allCompanies.some((c) => c.id === s.id)
          );
          this.departmentForm
            .get("selectedCompanies")
            ?.setValue([...this.selectedCompanies]);
        }
        this.filterCompanies();
      },
      error: () => {
        this.message.error("Failed to load parent companies.");
        this.allCompanies = [];
        this.filteredCompanies = [];
        this.selectedCompanies = [];
        this.departmentForm.get("selectedCompanies")?.setValue([]);
      },
    });
  }

  private updateEmployeesForParent() {
    const parentId = Number(
      this.departmentForm.get("parentDepartmentId")?.value
    );
    if (!parentId) {
      this.allEmployees = [];
      this.filteredEmployees = [];
      if (this.departmentForm.get("isSubsidiary")?.value) {
        this.selectedEmployees = [];
      }
      return;
    }

    this.deptService.getEmployeesByDepartment(parentId).subscribe({
      next: (parentEmployees) => {
        this.allEmployees = parentEmployees ?? [];
        if (this.departmentForm.get("isSubsidiary")?.value) {
          this.selectedEmployees = this.selectedEmployees.filter((s) =>
            this.allEmployees.some((e) => e.id === s.id)
          );
        }
        this.filterEmployees();
      },
      error: () => {
        this.allEmployees = [];
        this.filteredEmployees = [];
        this.selectedEmployees = [];
      },
    });
  }

  private preserveSelectedCompanies() {
    this.selectedCompanies = this.selectedCompanies.filter((s) =>
      this.allCompanies.some((c) => c.id === s.id)
    );
    this.departmentForm
      .get("selectedCompanies")
      ?.setValue([...this.selectedCompanies]);
    this.filterCompanies();
  }

  private preserveSelectedEmployees() {
    this.selectedEmployees = this.selectedEmployees.filter((s) =>
      this.allEmployees.some((e) => e.id === s.id)
    );
    this.filterEmployees();
  }

  private handleDataLoad(
    departments: Department[],
    companies: CompanyList[],
    employees: Employee[],
    department: Department | null
  ) {
    let filteredDepartments = departments ?? [];
    if (this.departmentId) {
      const excludedIds = [
        this.departmentId,
        ...this.getDescendantIds(departments, this.departmentId),
      ];
      filteredDepartments = departments.filter(
        (d) => !excludedIds.includes(d.id)
      );
    }

    this.departments = filteredDepartments;
    this.departmentTreeNodes = this.buildDepartmentTree(this.departments);
    this.allCompanies = companies ?? [];
    this.allEmployees = employees ?? [];

    if (department) {
      this.populateFormAndSelections(department);
    } else {
      this.filteredCompanies = [...this.allCompanies];
      this.filteredEmployees = [...this.allEmployees];
    }
  }

  private populateFormAndSelections(department: Department) {
    this.showParentDropdown = department.isSubsidiary;

    this.departmentForm.patchValue({
      deptName: department.deptName,
      deptCode: department.deptCode,
      abbreviation: department.abbreviation || "",
      isActive: department.isActive,
      isSubsidiary: department.isSubsidiary,
      parentDepartmentId: department.parentDepartmentId?.toString() || null,
    });

    // ---- Companies ----
    const deptCompanyIds = (department.companies ?? []).map((c) => c.id);
    this.selectedCompanies = this.allCompanies.filter((c) =>
      deptCompanyIds.includes(c.id)
    );
    this.allCompanies = this.allCompanies.filter(
      (c) => !deptCompanyIds.includes(c.id)
    );

    // ---- Employees ----
    const deptEmpIds = (department.employees ?? []).map((de) => de.id);
    this.selectedEmployees = this.allEmployees.filter((e) =>
      deptEmpIds.includes(e.id)
    );
    this.allEmployees = this.allEmployees.filter(
      (e) => !deptEmpIds.includes(e.id)
    );

    this.departmentForm
      .get("selectedCompanies")
      ?.setValue([...this.selectedCompanies]);

    setTimeout(() => {
      const parentId = department.parentDepartmentId?.toString() || null;
      this.departmentForm.get("parentDepartmentId")?.setValue(parentId);
    }, 100);

    if (department.isSubsidiary && department.parentDepartmentId) {
      this.updateCompaniesForParent();
      this.updateEmployeesForParent();
    } else {
      this.filteredCompanies = [...this.allCompanies];
      this.filteredEmployees = [...this.allEmployees];
    }
    this.filterCompanies();
    this.filterEmployees();
  }

  filterCompanies() {
    const q = (this.companySearch ?? "").trim().toLowerCase();

    this.filteredCompanies = this.allCompanies.filter((c) =>
      (c.companyName ?? "").toLowerCase().includes(q)
    );
  }

  filterEmployees() {
    const q = (this.employeeSearch ?? "").trim().toLowerCase();

    this.filteredEmployees = this.allEmployees.filter((e) =>
      (e.systemDisplayName ?? "").toLowerCase().includes(q)
    );
  }

  addCompany(company: CompanyList) {
    if (this.selectedCompanies.some((c) => c.id === company.id)) return;
    this.allCompanies = this.allCompanies.filter((c) => c.id !== company.id);
    this.selectedCompanies.push(company);
    this.departmentForm
      .get("selectedCompanies")
      ?.setValue([...this.selectedCompanies]);
    this.filterCompanies();
  }

  removeCompany(company: CompanyList, event?: MouseEvent) {
    if (event) event.stopPropagation();
    this.selectedCompanies = this.selectedCompanies.filter(
      (c) => c.id !== company.id
    );
    if (!this.allCompanies.some((c) => c.id === company.id)) {
      this.allCompanies.push(company);
      this.allCompanies.sort((a, b) =>
        a.companyName.localeCompare(b.companyName)
      );
    }

    this.departmentForm
      .get("selectedCompanies")
      ?.setValue([...this.selectedCompanies]);
    this.filterCompanies();
  }

  addEmployee(employee: Employee) {
    if (this.selectedEmployees.some((e) => e.id === employee.id)) return;

    this.allEmployees = this.allEmployees.filter((e) => e.id !== employee.id);
    this.selectedEmployees.push(employee);
    this.filterEmployees();
  }

  removeEmployee(employee: Employee, event?: MouseEvent) {
    if (event) event.stopPropagation();

    this.selectedEmployees = this.selectedEmployees.filter(
      (e) => e.id !== employee.id
    );

    if (!this.allEmployees.some((e) => e.id === employee.id)) {
      this.allEmployees.push(employee);
      this.allEmployees.sort((a, b) =>
        (a.systemDisplayName ?? "").localeCompare(b.systemDisplayName ?? "")
      );
    }

    this.filterEmployees();
  }

  saveDepartment() {
    Object.keys(this.departmentForm.controls).forEach((field) => {
      this.departmentForm.get(field)?.markAsTouched({ onlySelf: true });
    });

    if (this.departmentForm.invalid) {
      const errors: string[] = [];
      if (this.departmentForm.get("deptName")?.invalid)
        errors.push("Department name is required.");
      if (this.departmentForm.get("deptCode")?.invalid)
        errors.push("Department code is required.");
      if (
        this.departmentForm.get("selectedCompanies")?.hasError("minCompanies")
      )
        errors.push("At least one company must be selected.");
      this.message.error(errors.join(" "));
      return;
    }

    const formData: Partial<Department> = {
      id: this.departmentId ?? 0,
      deptName: this.departmentForm.get("deptName")?.value?.trim(),
      deptCode: this.departmentForm.get("deptCode")?.value?.trim(),
      abbreviation:
        this.departmentForm.get("abbreviation")?.value?.trim() || null,
      isSubsidiary: this.departmentForm.get("isSubsidiary")?.value,
      parentDepartmentId: this.departmentForm.get("parentDepartmentId")?.value
        ? Number(this.departmentForm.get("parentDepartmentId")?.value)
        : undefined,
      isActive: this.departmentForm.get("isActive")?.value,
    };

    const companyIds = this.selectedCompanies.map((c) => c.id);
    const employeeIds = this.selectedEmployees
      .map((e) => e.id!)
      .filter((id): id is number => id !== undefined);

    this.isLoading = true;

    const save$ = this.departmentId
      ? this.deptService.updateDepartment(
          this.departmentId,
          formData as Department,
          employeeIds,
          companyIds
        )
      : this.deptService.createDepartment(
          formData as Department,
          employeeIds,
          companyIds
        );

    this.subscriptions.add(
      save$.subscribe({
        next: () => this.assignEmployeesToDepartmentCompanies(),
        error: (err) => {
          this.isLoading = false;
          const errorMessage = err.error?.errors
            ? Object.values(err.error.errors).flat().join("; ")
            : err.error?.message || "Failed to save department.";
          this.message.error(errorMessage);
        },
      })
    );
  }

  private assignEmployeesToDepartmentCompanies() {
    const companyIds = this.selectedCompanies.map((c) => c.id);
    const employeeIds = this.selectedEmployees
      .map((e) => e.id!)
      .filter(Boolean);

    if (companyIds.length === 0 || employeeIds.length === 0) {
      this.finalizeSave();
      return;
    }

    this.deptService
      .assignEmployeesToCompanies(companyIds, employeeIds)
      .subscribe({
        next: () => this.finalizeSave(),
        error: () => {
          this.message.warning(
            "Department saved, but failed to link employees to companies."
          );
          this.finalizeSave();
        },
      });
  }

  private finalizeSave() {
    this.isLoading = false;
    this.message.success("Department saved successfully!");
    this.router.navigate(["/lst-department"]);
  }

  toggleSubsidiary(event: any) {
    this.showParentDropdown = event.target.checked;
    if (!this.showParentDropdown) {
      this.departmentForm.patchValue({ parentDepartmentId: null });
      this.loadAllCompanies();
      this.loadAllEmployees();
    }
  }

  closeModal() {
    this.router.navigate(["/lst-department"]);
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
