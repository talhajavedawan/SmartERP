import {
  Component,
  OnInit,
  OnDestroy,
  ChangeDetectorRef,
  Inject,
  PLATFORM_ID,
} from "@angular/core";
import { isPlatformBrowser } from "@angular/common";
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormControl } from "@angular/forms";
import { NzTableModule } from "ng-zorro-antd/table";
import { NzMessageService } from "ng-zorro-antd/message";
import { NzModalService, NzModalModule } from "ng-zorro-antd/modal";
import { NzIconModule } from "ng-zorro-antd/icon";
import { NzToolTipModule } from "ng-zorro-antd/tooltip";
import { Subscription } from "rxjs";
import { Router } from "@angular/router";
import { UserService } from "../../../../shared/services/User.service";
import { RoleService, Role } from "../../../../shared/services/roles.service";
import {
  PermissionService,
  Permission,
} from "../../../../shared/services/permission.service";
import { NzTreeNodeOptions } from "ng-zorro-antd/core/tree";
import { ColDef, ICellRendererParams, IRowNode } from "ag-grid-community";
import { GridComponent } from "../../../../shared/components/grid/grid.component";

@Component({
  selector: "app-users",
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzTableModule,
    NzModalModule,
    NzIconModule,
    NzToolTipModule,
    GridComponent,
  ],
  templateUrl: "./user-information.component.html",
  styleUrls: [
    "./user-information.component.css",
    "../../../../../scss/lists.css",
  ],
})
export class UserFormComponent implements OnInit, OnDestroy {
  searchControl = new FormControl("");
  users: any[] = [];
  selectedUser: any = null;
  selectedUserRoles: Role[] = [];
  selectedRole: Role | null = null;
  selectedRolePermissions: Permission[] = [];
  filterState: "all" | "active" | "inactive" = "all";
  isLoading = false;
  isLoadingRoles = false;
  isLoadingPermissions = false;
  expandedUserRoleIds: Set<string> = new Set();
  expandedPermissionIds: Set<string> = new Set();
  private subscriptions = new Subscription();

  userColumnDefs: ColDef[] = [
    {
      field: "userName",
      headerName: "User Name",
      sortable: true,
      filter: "agTextColumnFilter",
      flex: 1,
    },
    {
      field: "employee.systemDisplayName",
      headerName: "System Name",
      sortable: true,
      filter: "agTextColumnFilter",
      flex: 1,
    },
    {
      field: "isActive",
      headerName: "Status",
      minWidth: 100,
      cellRenderer: (params: ICellRendererParams) => {
        return params.value
          ? `<span class="badge bg-success">Active</span>`
          : `<span class="badge bg-danger">Inactive</span>`;
      },
    },
  ];

  private _userRoleTreeNodes: NzTreeNodeOptions[] = [];
  private _permissionTreeNodes: NzTreeNodeOptions[] = [];

  get userRoleTreeNodes(): NzTreeNodeOptions[] {
    return this._userRoleTreeNodes;
  }
  set userRoleTreeNodes(nodes: NzTreeNodeOptions[]) {
    this._userRoleTreeNodes = nodes;
    this.cdr.detectChanges();
  }

  get permissionTreeNodes(): NzTreeNodeOptions[] {
    return this._permissionTreeNodes;
  }
  set permissionTreeNodes(nodes: NzTreeNodeOptions[]) {
    this._permissionTreeNodes = nodes;
    this.cdr.detectChanges();
  }

  get flatUserRoles(): any[] {
    const result: any[] = [];
    const traverse = (nodes: NzTreeNodeOptions[], level: number) => {
      for (const node of nodes) {
        const fullRole = this.selectedUserRoles.find(
          (r) => r.id.toString() === node.key
        );
        const row = {
          ...node,
          description: fullRole?.description,
          level,
          expand: this.expandedUserRoleIds.has(node.key),
          children: node.children || [],
          isParent: !!(node.children && node.children.length > 0),
        };
        result.push(row);
        if (row.expand && row.children.length) {
          traverse(row.children, level + 1);
        }
      }
    };
    traverse(this.userRoleTreeNodes, 0);
    return result;
  }

  get flatPermissions(): any[] {
    const result: any[] = [];
    const traverse = (nodes: NzTreeNodeOptions[], level: number) => {
      for (const node of nodes) {
        const fullPermission = this.selectedRolePermissions.find(
          (p) => p.id.toString() === node.key
        );
        const row = {
          ...node,
          description: fullPermission?.description,
          level,
          expand: this.expandedPermissionIds.has(node.key),
          children: node.children || [],
          isParent: !!(node.children && node.children.length > 0),
        };
        result.push(row);
        if (row.expand && row.children.length) {
          traverse(row.children, level + 1);
        }
      }
    };
    traverse(this.permissionTreeNodes, 0);
    return result;
  }

  constructor(
    private userService: UserService,
    private roleService: RoleService,
    private permissionService: PermissionService,
    private message: NzMessageService,
    private modal: NzModalService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

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

  ngOnInit(): void {
    this.loadUsers();
    this.selectedUser = this.getSessionItem("selectedUser");
    if (this.selectedUser) {
      this.loadUserRoles(this.selectedUser.id);
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.clearSelectionState();

    this.userService.getUsers(this.filterState).subscribe({
      next: (response: any) => {
        const arr = Array.isArray(response.data)
          ? response.data
          : response.data?.$values || [];
        this.users = arr;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.message.error(`Failed to load ${this.filterState} users.`);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  RefreshUsers(): void {
    this.loadUsers();
    this.message.success("Users Refreshed.");
  }

  setStatusFilter(status: "all" | "active" | "inactive"): void {
    this.filterState = status;
    this.loadUsers();
  }

  onUserRowClicked(event: {
    event: MouseEvent;
    node: IRowNode;
    data: any;
  }): void {
    this.selectUser(event.data);
  }

  selectUser(user: any): void {
    const isSame = this.selectedUser?.id === user.id;
    this.selectedUser = isSame ? null : user;
    this.setSessionItem("selectedUser", this.selectedUser);

    if (this.selectedUser) {
      this.loadUserRoles(this.selectedUser.id);
    } else {
      this.clearSelectionState();
    }
    this.cdr.detectChanges();
  }

  clearSelectionState(): void {
    this.selectedUserRoles = [];
    this.userRoleTreeNodes = [];
    this.selectedRole = null;
    this.selectedRolePermissions = [];
    this.permissionTreeNodes = [];
    this.expandedUserRoleIds.clear();
    this.expandedPermissionIds.clear();
    this.isLoadingRoles = false;
    this.isLoadingPermissions = false;
  }

  selectRole(row: any): void {
    const roleId = parseInt(row.key, 10);
    const role = this.selectedUserRoles.find((r) => r.id === roleId);
    const isSame = this.selectedRole?.id === role?.id;

    if (isSame) {
      this.selectedRole = null;
      this.selectedRolePermissions = [];
      this.permissionTreeNodes = [];
      this.expandedPermissionIds.clear();
      this.isLoadingPermissions = false;
    } else {
      this.selectedRole = role ?? null;
      if (this.selectedRole) {
        this.isLoadingPermissions = true;
        this.selectedRolePermissions = this.selectedRole.permissions ?? [];
        this.permissionTreeNodes = this.buildPermissionTree(
          this.selectedRolePermissions
        );
        this.isLoadingPermissions = false;
      }
    }
    this.cdr.detectChanges();
  }

  loadUserRoles(userId: number): void {
    this.isLoadingRoles = true;
    this.selectedRole = null;
    this.selectedRolePermissions = [];
    this.permissionTreeNodes = [];
    this.expandedPermissionIds.clear();

    this.subscriptions.add(
      this.userService.getUserRoles(userId).subscribe({
        next: (roles: Role[]) => {
          this.selectedUserRoles = roles;
          this.userRoleTreeNodes = this.buildAssignedRoleTree(roles);
          this.isLoadingRoles = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.message.error("Failed to load user roles.");
          this.selectedUserRoles = [];
          this.userRoleTreeNodes = [];
          this.isLoadingRoles = false;
          this.cdr.detectChanges();
        },
      })
    );
  }

  // FIXED: Removed `.filter()` that was removing child nodes
  buildAssignedRoleTree(roles: Role[]): NzTreeNodeOptions[] {
    const map = new Map<string, NzTreeNodeOptions>();
    roles.forEach((r) => {
      map.set(r.id.toString(), {
        title: r.name,
        key: r.id.toString(),
        value: r.id.toString(),
        isLeaf: true,
        children: [],
        description: r.description,
      });
    });

    const tree: NzTreeNodeOptions[] = [];
    roles.forEach((r) => {
      const node = map.get(r.id.toString())!;
      if (r.parentRoleId) {
        const parent = map.get(r.parentRoleId.toString());
        if (parent) {
          parent.children!.push(node);
          parent.isLeaf = false;
        } else {
          tree.push(node);
        }
      } else {
        tree.push(node);
      }
    });

    // No filtering — keep all root nodes
    return tree;
  }

  // FIXED: Same fix for permissions
  buildPermissionTree(permissions: Permission[]): NzTreeNodeOptions[] {
    const map = new Map<string, NzTreeNodeOptions>();
    permissions.forEach((p) => {
      map.set(p.id.toString(), {
        title: p.name,
        key: p.id.toString(),
        value: p.id.toString(),
        isLeaf: true,
        children: [],
        description: p.description,
      });
    });

    const tree: NzTreeNodeOptions[] = [];
    permissions.forEach((p) => {
      const node = map.get(p.id.toString())!;
      if (p.parentPermissionId) {
        const parent = map.get(p.parentPermissionId.toString());
        if (parent) {
          parent.children!.push(node);
          parent.isLeaf = false;
        } else {
          tree.push(node);
        }
      } else {
        tree.push(node);
      }
    });

    return tree; // No filtering
  }

  onUserRoleExpandChange(role: any, expanded: boolean): void {
    if (expanded) {
      this.expandedUserRoleIds.add(role.key);
    } else {
      this.expandedUserRoleIds.delete(role.key);
    }
    this.cdr.detectChanges();
  }

  onPermissionExpandChange(perm: any, expanded: boolean): void {
    if (expanded) {
      this.expandedPermissionIds.add(perm.key);
    } else {
      this.expandedPermissionIds.delete(perm.key);
    }
    this.cdr.detectChanges();
  }

  goToAddUser(): void {
    this.removeSessionItem("selectedUser");
    this.router.navigate(["/user-form"]);
  }

  editUser(user: any | null): void {
    const toEdit = user ?? this.selectedUser;
    if (!toEdit) {
      this.message.warning("Please select a user to update.");
      return;
    }
    this.setSessionItem("selectedUser", toEdit);
    this.router.navigate(["/user-form"], { queryParams: { isEdit: true } });
  }

  onUserSelectedFromCheckbox(user: any): void {
    if (user) this.selectUser(user);
    else {
      this.selectedUser = null;
      this.clearSelectionState();
    }
  }
}
