import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { CommonModule, DatePipe } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { NzMessageService } from "ng-zorro-antd/message";
import { NzModalService, NzModalModule } from "ng-zorro-antd/modal";
import { AgGridModule } from "ag-grid-angular"; 
import { AgGridLayoutComponent } from "../../../shared/components/grids-layout/grids-layout.component";

import { RoleService, Role } from "../../../shared/services/roles.service";
import {
  PermissionService,
  Permission,
} from "../../../shared/services/permission.service";

@Component({
  selector: "app-roles",
  standalone: true,
  templateUrl: "./roles.component.html",
  styleUrls: ["../../../../scss/global.css", "roles.component.css"],
  imports: [
    CommonModule,
    FormsModule,
    NzModalModule,
    AgGridLayoutComponent,
    AgGridModule, // ✅ required for <ag-grid-angular>
  ],
  providers: [DatePipe],
})
export class RolesComponent implements OnInit {
  /** ========== STATE ========== */
  roles: any[] = [];
  displayRoles: any[] = [];
  selectedRoleId: number | null = null;

  permissions: any[] = [];
  selectedRolePermissions: any[] = [];
  displayPermissions: any[] = [];

  permissionsLoading = false;
  searchValue = "";

  /** ========== AG GRID CONFIG ========== */
  roleColumnDefs = [
    {
      field: "name",
      headerName: "Role Name",
      width: 240,
      visible: true,
      cellRenderer: this.roleRenderer.bind(this),
    },
    { field: "description", headerName: "Description", width: 250, visible: true },
{
  headerName: "Status",
  field: "isActive",
  width: 120,
  visible: true,
  cellRenderer: (params: any) => {
    const isActive =
      params.value === true || params.value === "Active" || params.value === 1;

    const badgeClass = isActive
      ? "badge rounded-pill bg-success"
      : "badge rounded-pill bg-secondary";

    const label = isActive ? "Active" : "Inactive";
    return `<span class="${badgeClass}">${label}</span>`;
  },
},

    {
      field: "creationDate",
      headerName: "Created On",
      width: 180,
      visible: true,
      valueFormatter: (p: any) =>
        p.value ? new Date(p.value).toLocaleString() : "",
    },
  ];

  permissionColumnDefs = [
    {
      field: "name",
      headerName: "Permission Name",
      width: 170,
      visible: true,
      cellRenderer: this.permissionRenderer.bind(this),
    },
    {
      field: "description",
      headerName: "Description",
      width:200,
      visible: true,
    },
 
  ];

  constructor(
    private roleService: RoleService,
    private permissionService: PermissionService,
    private message: NzMessageService,
    private modal: NzModalService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  /** ================= INIT ================= */
  ngOnInit(): void {
    this.loadRoles();
  }

  /** ================= LOAD ROLES ================= */
  loadRoles(): void {
    this.roleService.getAll().subscribe({
      next: (data) => {
        const res: any = data;
        const roles = Array.isArray(res) ? res : res?.$values || [];

        this.roles = roles.map((r: Role) => ({
          ...r,
          expand: false,
          level: 0,
          children: [],
        }));

        this.buildRoleHierarchy("parentRoleId");
        this.displayRoles = this.flattenTree(
          this.getRootNodes(this.roles, "parentRoleId")
        );
      },
      error: () => this.message.error("❌ Failed to load roles."),
    });
  }

  /** ================== TREE HELPERS ================== */
  buildRoleHierarchy(parentKey: string): void {
    const map = new Map<number, any>();
    this.roles.forEach((r) => {
      r.children = [];
      r.level = 0;
      map.set(r.id, r);
    });

    this.roles.forEach((r) => {
      if (r[parentKey]) {
        const parent = map.get(r[parentKey]);
        if (parent) {
          r.level = parent.level + 1;
          parent.children.push(r);
        }
      }
    });
  }

  getRootNodes(list: any[], parentKey: string): any[] {
    return list.filter((x) => !x[parentKey]);
  }

  flattenTree(nodes: any[]): any[] {
    const result: any[] = [];
    const traverse = (n: any) => {
      result.push(n);
      if (n.expand && n.children) {
        n.children.forEach((c: any) => traverse(c));
      }
    };
    nodes.forEach(traverse);
    return result;
  }

  /** ================== ROLE RENDERER ================== */
  roleRenderer(params: any) {
    const role = params.data;
    const indent = role.level * 20;
    const hasChildren = role.children && role.children.length > 0;
    const icon = hasChildren ? (role.expand ? "▼" : "▶") : "";
    return `
      <span style="padding-left:${indent}px; cursor:pointer;" class="role-node">
        <span style="color:#007bff; font-weight:500;">${icon}</span>
        <span style="margin-left:5px;">${role.name}</span>
      </span>
    `;
  }

  /** ================== PERMISSION RENDERER ================== */
  permissionRenderer(params: any) {
    const perm = params.data;
    const indent = perm.level * 20;
    const hasChildren = perm.children && perm.children.length > 0;
    const icon = hasChildren ? (perm.expand ? "▼" : "▶") : "";
    return `
      <span style="padding-left:${indent}px; cursor:pointer;" class="perm-node">
        <span style="color:#198754; font-weight:500;">${icon}</span>
        <span style="margin-left:5px;">${perm.name}</span>
      </span>
    `;
  }

  /** ================== ROLE CLICK ================== */
  onRoleRowClicked(event: any): void {
    const node = event.data;
    const el = event.event.target as HTMLElement;

    const isExpandClick =
      el.innerText === "▶" ||
      el.innerText === "▼" ||
      el.classList.contains("role-node");

    if (isExpandClick) {
      node.expand = !node.expand;
      this.displayRoles = this.flattenTree(
        this.getRootNodes(this.roles, "parentRoleId")
      );
      this.cdr.detectChanges();
      return;
    }

    this.selectedRoleId = node.id;
    this.loadAssignedPermissions(node);
  }

  /** ================== LOAD ASSIGNED PERMISSIONS ================== */
  loadAssignedPermissions(role: Role): void {
    if (!role) return;
    this.permissionsLoading = true;

    this.permissionService.getAllPermission().subscribe({
      next: (res) => {
        const allPermissions = res?.permissions ?? [];
        const permissionHierarchy = this.buildHierarchy(
          allPermissions,
          "parentPermissionId"
        );
        const assignedIds = (role.permissions || []).map((p: any) => p.id);
        const filtered = this.filterAssigned(permissionHierarchy, assignedIds);

        this.selectedRolePermissions = filtered;
        this.displayPermissions = this.flattenTree(
          this.getRootNodes(filtered, "parentPermissionId")
        );
        this.permissionsLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.permissionsLoading = false;
        this.message.error("Failed to load permissions");
      },
    });
  }

  /** ================== PERMISSION TREE HELPERS ================== */
  buildHierarchy(list: any[], parentKey: string): any[] {
    const map = new Map<number, any>();
    const roots: any[] = [];

    list.forEach((p) => {
      p.children = [];
      p.expand = false;
      p.level = 0;
      map.set(p.id, p);
    });

    list.forEach((p) => {
      if (p[parentKey]) {
        const parent = map.get(p[parentKey]);
        if (parent) {
          p.level = parent.level + 1;
          parent.children.push(p);
        }
      } else roots.push(p);
    });

    return roots;
  }

  filterAssigned(fullTree: Permission[], assignedIds: number[]): Permission[] {
    return fullTree
      .map((p) => {
        const children = this.filterAssigned(p.children ?? [], assignedIds);
        if (assignedIds.includes(p.id) || children.length > 0) {
          return { ...p, expand: false, children };
        }
        return null;
      })
      .filter(Boolean) as Permission[];
  }

  /** ================== PERMISSION CLICK ================== */
  onPermissionRowClicked(event: any): void {
    const node = event.data;
    const el = event.event.target as HTMLElement;

    const isExpandClick =
      el.innerText === "▶" ||
      el.innerText === "▼" ||
      el.classList.contains("perm-node");

    if (isExpandClick) {
      node.expand = !node.expand;
      this.displayPermissions = this.flattenTree(
        this.getRootNodes(this.selectedRolePermissions, "parentPermissionId")
      );
      this.cdr.detectChanges();
    }
  }

  /** ================== CRUD ================== */
  showAddModal(): void {
    this.router.navigate(["/frmRoles"]);
  }

  showEditModal(): void {
    if (!this.selectedRoleId) {
      this.message.error("Please select a role to edit");
      return;
    }
    this.router.navigate(["/frmRoles"], {
      queryParams: { id: this.selectedRoleId },
    });
  }

  deleteRole(): void {
    if (!this.selectedRoleId) {
      this.message.error("Please select a role to delete");
      return;
    }

    this.modal.confirm({
      nzTitle: "Delete Role?",
      nzContent: "Are you sure you want to delete this role?",
      nzOkText: "Yes, delete it",
      nzOkDanger: true,
      nzOnOk: () => {
        this.roleService.delete(this.selectedRoleId!).subscribe({
          next: () => {
            this.message.success("Role deleted successfully!");
            this.selectedRoleId = null;
            this.loadRoles();
          },
          error: () => this.message.error("Failed to delete role."),
        });
      },
    });
  }

    refreshRoles(): void {
  this.loadRoles(); // simply reload data
  this.message.success('✅ Role list refreshed successfully!');
}
}
