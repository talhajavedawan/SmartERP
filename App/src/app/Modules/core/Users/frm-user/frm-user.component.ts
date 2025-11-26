import {
  Component,
  OnInit,
  OnDestroy,
  ChangeDetectorRef,
  Inject,
  PLATFORM_ID,
  AfterViewInit,
} from "@angular/core";
import { isPlatformBrowser } from "@angular/common";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
  AbstractControl,
  ValidationErrors,
} from "@angular/forms";
import { CommonModule } from "@angular/common";
import { NzMessageService } from "ng-zorro-antd/message";
import { Router, ActivatedRoute } from "@angular/router";
import { Subscription } from "rxjs";
import { UserService } from "../../../../shared/services/User.service";
import { RoleService, Role } from "../../../../shared/services/roles.service";
import { NzTableModule } from "ng-zorro-antd/table";
import { NzIconModule } from "ng-zorro-antd/icon";
import { NzSelectModule } from "ng-zorro-antd/select";
import { NzDropDownModule } from "ng-zorro-antd/dropdown";
import { NzTreeNodeOptions } from "ng-zorro-antd/core/tree";

declare var bootstrap: any;

@Component({
  selector: "app-frm-user",
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzTableModule,
    NzIconModule,
    NzSelectModule,
    NzDropDownModule,
  ],
  templateUrl: "./frm-user.component.html",
  styleUrls: ["../../../../../scss/forms.css"],
})
export class FrmUserComponent implements OnInit, OnDestroy, AfterViewInit {
  randomId: string = "";
  userForm: FormGroup;
  isLoading = false;
  isEdit = false;
  user: any | null = null;
  employees: any[] = [];
  roles: Role[] = [];
  roleTreeNodes: NzTreeNodeOptions[] = [];
  expandedRoleIds: Set<string> = new Set();
  passwordVisible = false;
  confirmPasswordVisible = false;
  dropdownOpen = false;
  selectedEmployee: any = null;
  private subscriptions = new Subscription();

  // Safe sessionStorage helpers
  private setSessionItem(key: string, value: any): void {
    if (isPlatformBrowser(this.platformId)) {
      sessionStorage.setItem(key, JSON.stringify(value));
    }
  }

  private getSessionItem<T>(key: string): T | null {
    if (isPlatformBrowser(this.platformId)) {
      const raw = sessionStorage.getItem(key);
      return raw ? JSON.parse(raw) : null;
    }
    return null;
  }

  private removeSessionItem(key: string): void {
    if (isPlatformBrowser(this.platformId)) {
      sessionStorage.removeItem(key);
    }
  }

  constructor(
    private fb: FormBuilder,
    private message: NzMessageService,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private roleService: RoleService,
    private cdr: ChangeDetectorRef,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
    this.userForm = this.fb.group(
      {
        userName: ["", [Validators.required, Validators.maxLength(100)]],
        email: ["", [Validators.required, Validators.email]],
        password: [""],
        confirmPassword: [""],
        employeeId: [null],
        roles: [[], Validators.required],
        isActive: [true],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  /* --------------------------------------------------- */
  /*  FLAT ROLES                                         */
  /* --------------------------------------------------- */
  get flatRoles(): any[] {
    const result: any[] = [];
    const traverse = (nodes: NzTreeNodeOptions[], level: number) => {
      for (const node of nodes) {
        const row = {
          ...node,
          level,
          expand: this.expandedRoleIds.has(node.key),
          children: node.children || [],
          isParent: node.children && node.children.length > 0,
        };
        result.push(row);
        if (row.expand && row.children.length) {
          traverse(row.children, level + 1);
        }
      }
    };
    traverse(this.roleTreeNodes, 0);
    return result;
  }

  /* --------------------------------------------------- */
  /*  LIFECYCLE                                          */
  /* --------------------------------------------------- */
  ngOnInit(): void {
    this.randomId = Math.random().toString(36).substring(2, 15);
    this.subscriptions.add(
      this.route.queryParams.subscribe((params) => {
        this.isEdit = params["isEdit"] === "true";
        const storedUser = this.getSessionItem("selectedUser");

        // Reset form state
        this.resetFormState();

        if (this.isEdit && storedUser) {
          this.user = storedUser;

          this.userForm.patchValue({
            userName: this.user.userName,
            email: this.user.email,
            employeeId: this.user.employeeId,
            isActive: this.user.isActive,
          });

          this.clearPasswordValidators();
          this.loadAvailableEmployeesForEdit(this.user.id);
          this.loadUserRolesForEdit(this.user.id);
        } else {
          this.setPasswordValidators();
          this.loadAvailableEmployees();
        }

        this.loadRoles();
      })
    );

    // Safe password change handler in edit mode
    if (this.isEdit) {
      let isUpdating = false;
      const passwordControl = this.userForm.get("password");

      this.subscriptions.add(
        passwordControl!.valueChanges.subscribe((value) => {
          if (isUpdating) return;
          isUpdating = true;

          try {
            const trimmed = (value || "").toString().trim();
            if (trimmed !== "") {
              this.setPasswordValidators();
            } else {
              this.clearPasswordValidators();
            }
          } finally {
            isUpdating = false;
          }

          passwordControl?.updateValueAndValidity({ emitEvent: false });
        })
      );
    }
  }

  ngAfterViewInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const tooltipTriggerList = Array.from(
        document.querySelectorAll('[data-bs-toggle="tooltip"]')
      );
      tooltipTriggerList.forEach(
        (tooltipEl) => new bootstrap.Tooltip(tooltipEl)
      );
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  /* --------------------------------------------------- */
  /*  FORM VALIDATORS                                    */
  /* --------------------------------------------------- */
  passwordMatchValidator = (form: FormGroup): ValidationErrors | null => {
    const p = form.get("password")?.value || "";
    const c = form.get("confirmPassword")?.value || "";
    return p && c && p !== c ? { mismatch: true } : null;
  };

  private setPasswordValidators(): void {
    const passwordCtrl = this.userForm.get("password");
    const confirmCtrl = this.userForm.get("confirmPassword");

    if (!passwordCtrl || !confirmCtrl) return;

    passwordCtrl.setValidators([
      Validators.required,
      Validators.minLength(6),
      this.passwordStrengthValidator.bind(this),
    ]);
    confirmCtrl.setValidators([Validators.required]);

    passwordCtrl.updateValueAndValidity({ emitEvent: false });
    confirmCtrl.updateValueAndValidity({ emitEvent: false });
  }

  private clearPasswordValidators(): void {
    const passwordCtrl = this.userForm.get("password");
    const confirmCtrl = this.userForm.get("confirmPassword");

    if (!passwordCtrl || !confirmCtrl) return;

    passwordCtrl.clearValidators();
    confirmCtrl.clearValidators();

    passwordCtrl.updateValueAndValidity({ emitEvent: false });
    confirmCtrl.updateValueAndValidity({ emitEvent: false });
  }

  private passwordStrengthValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const value = control.value || "";
    if (!value) return null;

    const hasUpperCase = /[A-Z]/.test(value);
    const hasLowerCase = /[a-z]/.test(value);
    const hasDigit = /\d/.test(value);
    const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);

    const isValid =
      hasUpperCase &&
      hasLowerCase &&
      hasDigit &&
      hasSpecialChar &&
      value.length >= 6;

    return isValid ? null : { passwordStrength: true };
  }

  /* --------------------------------------------------- */
  /*  ROLES                                              */
  /* --------------------------------------------------- */
  private loadRoles(): void {
    this.subscriptions.add(
      this.roleService.getAll().subscribe({
        next: (roles) => {
          this.roles = roles;
          this.roleTreeNodes = this.buildRoleTree(roles);
          this.cdr.detectChanges();
        },
        error: () => this.message.error("Failed to load roles."),
      })
    );
  }

  private buildRoleTree(roles: Role[]): NzTreeNodeOptions[] {
    const map = new Map<string, NzTreeNodeOptions>();
    roles.forEach((r) => {
      map.set(r.id.toString(), {
        title: r.name,
        key: r.id.toString(),
        value: r.id.toString(),
        isLeaf: !r.children?.length,
        children: [],
        description: r.description,
      });
    });

    const tree: NzTreeNodeOptions[] = [];
    roles.forEach((r) => {
      if (r.parentRoleId) {
        const parent = map.get(r.parentRoleId.toString());
        if (parent) {
          parent.children!.push(map.get(r.id.toString())!);
          parent.isLeaf = false;
        } else {
          tree.push(map.get(r.id.toString())!);
        }
      } else {
        tree.push(map.get(r.id.toString())!);
      }
    });
    return tree;
  }

  private loadUserRolesForEdit(userId: number): void {
    this.subscriptions.add(
      this.userService.getUserRoles(userId).subscribe({
        next: (userRoles: Role[]) => {
          const ids = userRoles.map((r) => r.id.toString());
          this.userForm.patchValue({ roles: ids });
          this.cdr.detectChanges();
        },
        error: () => this.message.error("Failed to load user roles for edit."),
      })
    );
  }

  /* --------------------------------------------------- */
  /*  EMPLOYEE                                           */
  /* --------------------------------------------------- */
  private loadAvailableEmployees(): void {
    this.subscriptions.add(
      this.userService.getAvailableEmployees().subscribe({
        next: (emps) => {
          this.employees = emps;
          this.cdr.detectChanges();
        },
        error: () => this.message.error("Failed to load employees."),
      })
    );
  }

  private loadAvailableEmployeesForEdit(userId: number): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.userService.getAvailableEmployeesForEdit(userId).subscribe({
        next: (emps) => {
          this.employees = emps;
          const curId = this.userForm.get("employeeId")?.value;
          this.selectedEmployee =
            this.employees.find((e) => e.id === curId) ?? null;
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.message.error("Failed to load employees for edit.");
          this.isLoading = false;
        },
      })
    );
  }

  selectEmployee(emp: any): void {
    this.selectedEmployee = emp;
    this.userForm.get("employeeId")?.setValue(emp.id);
    this.userForm.get("employeeId")?.markAsTouched();
    this.dropdownOpen = false;
    this.cdr.detectChanges();
  }

  onExpandChange(role: any, expanded: boolean): void {
    expanded
      ? this.expandedRoleIds.add(role.key)
      : this.expandedRoleIds.delete(role.key);
    this.cdr.detectChanges();
  }

  /* --------------------------------------------------- */
  /*  SUBMIT – FULLY SAFE                                */
  /* --------------------------------------------------- */
  onSubmit(): void {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      this.message.error("Please fill in all required fields correctly.");
      return;
    }

    this.isLoading = true;
    this.userForm.disable();

    const fv = this.userForm.getRawValue(); // Get values even if disabled

    // Safe trim
    const userName = (fv.userName || "").toString().trim();
    if (!userName) {
      this.finalizeSubmit(false, "User Name is required.");
      return;
    }

    const payload: any = {
      id: this.isEdit && this.user ? this.user.id : undefined,
      userName,
      email: (fv.email || "").toString().trim(),
      employeeId: fv.employeeId,
      roles: (fv.roles || []).map((s: string) => parseInt(s, 10)),
      isActive: fv.isActive,
    };

    // Only send password if filled and trimmed
    const password = (fv.password || "").toString().trim();
    if (password) {
      payload.password = password;
    }

    // Username uniqueness check
    this.subscriptions.add(
      this.userService.getUsers().subscribe({
        next: (response: any) => {
          const usersArray: any[] =
            response?.data?.$values ?? response?.data ?? response ?? [];

          const usernameExists = usersArray.some(
            (u: any) =>
              u.userName?.toLowerCase() === userName.toLowerCase() &&
              u.id !== payload.id
          );

          if (usernameExists) {
            this.finalizeSubmit(false, "Username already exists.");
            return;
          }

          const validRoles = payload.roles.filter((id: number) =>
            this.roles.some((r) => r.id === id)
          );
          if (validRoles.length !== payload.roles.length) {
            this.finalizeSubmit(false, "Invalid role selected.");
            return;
          }
          payload.roles = validRoles;

          const save$ =
            this.isEdit && payload.id
              ? this.userService.updateUser(payload.id, payload)
              : this.userService.createUser(payload);

          save$.subscribe({
            next: () => {
              this.finalizeSubmit(
                true,
                this.isEdit ? "User updated!" : "User created!"
              );

              const savedEmp = this.employees.find(
                (e) => e.id === payload.employeeId
              );
              if (savedEmp) this.selectedEmployee = savedEmp;

              this.removeSessionItem("selectedUser");
              this.router.navigate(["/users"]);
            },
            error: () => {
              this.finalizeSubmit(
                false,
                this.isEdit ? "Update failed." : "Create failed."
              );
            },
          });
        },
        error: () => {
          this.finalizeSubmit(false, "Validation failed.");
        },
      })
    );
  }

  private finalizeSubmit(success: boolean, msg: string): void {
    this.isLoading = false;
    this.userForm.enable();
    if (success) {
      this.message.success(msg);
    } else {
      this.message.error(msg);
    }
  }

  /* --------------------------------------------------- */
  /*  UTILITIES                                          */
  /* --------------------------------------------------- */
  closeModal(): void {
    this.resetForm();
    this.removeSessionItem("selectedUser");
    this.router.navigate(["/users"]);
  }

  private resetFormState(): void {
    this.isLoading = false;
    this.expandedRoleIds.clear();
    this.selectedEmployee = null;
    this.dropdownOpen = false;
  }

  resetForm(): void {
    this.userForm.reset({
      userName: "",
      password: "",
      confirmPassword: "",
      employeeId: null,
      roles: [],
      isActive: true,
    });
    this.resetFormState();
  }

  togglePasswordVisibility(field: "password" | "confirmPassword"): void {
    if (field === "password") this.passwordVisible = !this.passwordVisible;
    else this.confirmPasswordVisible = !this.confirmPasswordVisible;
  }

  formatPKTDate(date: string | Date | undefined | null): string {
    if (!date || isNaN(new Date(date).getTime())) return "—";
    const d = new Date(date);
    return new Intl.DateTimeFormat("en-PK", {
      dateStyle: "medium",
      timeStyle: "medium",
      timeZone: "Asia/Karachi",
    }).format(d);
  }
  /* --------------------------------------------------- */
  /*  ROLE CHECKBOX – AUTO-SELECT CHILDREN               */
  /* --------------------------------------------------- */
  onRoleCheckChange(event: Event, roleKey: string): void {
    const checked = (event.target as HTMLInputElement).checked;
    const cur = (this.userForm.get("roles")?.value as string[]) || [];

    let updated: string[] = [];

    if (checked) {
      // 1. Add the clicked role
      updated = [...new Set([...cur, roleKey])];

      // 2. If it is a parent → add ALL its descendants
      const node = this.findNodeByKey(this.roleTreeNodes, roleKey);
      if (node && node.children && node.children.length) {
        const descendants = this.getAllDescendantKeys(node);
        updated = [...new Set([...updated, ...descendants])];
      }
    } else {
      // 1. Remove the clicked role
      updated = cur.filter((id) => id !== roleKey);

      // 2. If it is a child maybe un-check the parent
      const parentNode = this.findParentNode(this.roleTreeNodes, roleKey);
      if (parentNode) {
        // If no sibling is checkedun-check the parent
        const siblingKeys = parentNode.children!.map((c) => c.key);
        const anySiblingChecked = siblingKeys.some((k) => updated.includes(k));
        if (!anySiblingChecked) {
          updated = updated.filter((id) => id !== parentNode.key);
        }
      }
    }

    this.userForm.patchValue({ roles: updated });
    this.cdr.detectChanges();
  }

  /* --------------------------------------------------- */
  /*  HELPERS – TREE TRAVERSAL                           */
  /* --------------------------------------------------- */
  private findNodeByKey(
    nodes: NzTreeNodeOptions[],
    key: string
  ): NzTreeNodeOptions | null {
    for (const n of nodes) {
      if (n.key === key) return n;
      if (n.children?.length) {
        const found = this.findNodeByKey(n.children, key);
        if (found) return found;
      }
    }
    return null;
  }

  private findParentNode(
    nodes: NzTreeNodeOptions[],
    childKey: string
  ): NzTreeNodeOptions | null {
    for (const n of nodes) {
      if (n.children?.some((c) => c.key === childKey)) return n;
      if (n.children?.length) {
        const found = this.findParentNode(n.children, childKey);
        if (found) return found;
      }
    }
    return null;
  }

  private getAllDescendantKeys(node: NzTreeNodeOptions): string[] {
    const keys: string[] = [];
    const walk = (n: NzTreeNodeOptions) => {
      if (n.children?.length) {
        n.children.forEach((c) => {
          keys.push(c.key);
          walk(c);
        });
      }
    };
    walk(node);
    return keys;
  }
}
