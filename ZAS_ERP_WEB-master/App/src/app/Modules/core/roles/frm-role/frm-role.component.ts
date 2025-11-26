import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormControl,
} from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTreeSelectModule } from 'ng-zorro-antd/tree-select';
import { NzTreeModule, NzTreeNodeOptions } from 'ng-zorro-antd/tree';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';

import { RoleService, Role } from '../../../../shared/services/roles.service';
import {
  PermissionService,
  Permission,
} from '../../../../shared/services/permission.service';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzTagModule } from 'ng-zorro-antd/tag';

interface PermissionsTree {
  id: number;
  name: string;
  description: string;
  isActive: boolean;
  creationDate: string | null;
  parentPermissionId?: number | null;
  expand: boolean;
  level: number;
  children?: PermissionsTree[];
  indeterminate?: boolean;
}

@Component({
  selector: 'app-frm-role',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    NzModalModule,
    NzTableModule,
    NzTreeSelectModule,
    NzTreeModule,
    NzTagModule,
    NzTabsModule
  ],
  templateUrl: './frm-role.component.html',
  styleUrls: ['./frm-role.component.css'],
  providers: [DatePipe],
})
export class FrmRoleComponent implements OnInit {
  // ================= FORM + STATE =================
  roleForm: FormGroup;
  isEditMode = false;
  isLoading = false;
  currentEditId: number | null = null;

  // ================= ROLE TREE =================
  roles: Role[] = [];
  roleTreeNodes: NzTreeNodeOptions[] = [];

  // ================= PERMISSIONS =================
  permissions: Permission[] = [];
  permissionRows: PermissionsTree[] = [];
  filteredPermissionRows: PermissionsTree[] = [];
  permissionParentMap: Map<number, any> = new Map();

  selectedPermissionIds: number[] = [];
  checkedKeys: string[] = [];
  permissionSearch = '';
  selectedTabIndex = 0;
  constructor(
    private fb: FormBuilder,
    private datePipe: DatePipe,
    private roleService: RoleService,
    private permissionService: PermissionService,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.roleForm = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: [''],
      isSubsidiary: [false],
      parentRoleId: [null],
      isActive: [true],
      creationDate: [
        {
          value: this.datePipe.transform(new Date(), 'dd-MMM-yyyy hh:mm a'),
          disabled: true,
        },
      ],
      lastModified: [{ value: null, disabled: true }],
    });

    // Handle isSubsidiary logic
    this.roleForm.get('isSubsidiary')?.valueChanges.subscribe((val: boolean) => {
      const parentControl = this.roleForm.get('parentRoleId');
      if (val) {
        parentControl?.setValidators(Validators.required);
      } else {
        parentControl?.clearValidators();
        this.roleForm.patchValue({ parentRoleId: null });
      }
      parentControl?.updateValueAndValidity();
    });
  }

  // ================= INIT =================
  ngOnInit(): void {
    const id = this.route.snapshot.queryParamMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.currentEditId = +id;
    }

    this.loadRoles();
    this.loadPermissions();

    if (this.isEditMode && this.currentEditId) {
      this.loadRoleDetails(this.currentEditId);
    }
  }

  get nameControl(): FormControl {
    return this.roleForm.get('name') as FormControl;
  }

  // ================= LOAD ROLE TREE =================
  loadRoles(): void {
    this.roleService.getAll().subscribe({
      next: (data: Role[] | null) => {
        this.roles = data || [];
        this.roleTreeNodes = this.buildRoleTree(this.roles);
      },
      error: (error: HttpErrorResponse) =>
        this.message.error(error.error?.message || 'Failed to load roles'),
    });
  }

  private buildRoleTree(roles: Role[]): NzTreeNodeOptions[] {
    const map = new Map<number, NzTreeNodeOptions>();
    for (const role of roles) {
      map.set(role.id, {
        title: role.name,
        key: String(role.id),
        children: [],
        isLeaf: true,
      });
    }

    const tree: NzTreeNodeOptions[] = [];
    for (const role of roles) {
      const node = map.get(role.id)!;
      if (role.parentRoleId && map.has(role.parentRoleId)) {
        const parent = map.get(role.parentRoleId)!;
        parent.children!.push(node);
        parent.isLeaf = false;
      } else {
        tree.push(node);
      }
    }
    return tree;
  }

  // ================= LOAD ROLE DETAILS =================
  loadRoleDetails(id: number): void {
    this.isLoading = true;
    this.roleService.getById(id).subscribe({
      next: (role) => {
        this.isLoading = false;
        if (!role) {
          this.message.warning('Role not found');
          this.router.navigate(['/roles']);
          return;
        }

        this.roleForm.patchValue({
          name: role.name,
          description: role.description,
          isSubsidiary: !!role.parentRoleId,
          parentRoleId: role.parentRoleId,
          isActive: role.isActive,
          creationDate: this.datePipe.transform(role.creationDate, 'dd-MMM-yyyy hh:mm a'),
          lastModified: this.datePipe.transform(role.lastModified, 'dd-MMM-yyyy hh:mm a'),
        });

        this.selectedPermissionIds = (role.permissions || []).map((p) => p.id);
        this.checkedKeys = [...this.selectedPermissionKeys];
      },
      error: () => {
        this.isLoading = false;
        this.message.error('Failed to load role details');
      },
    });
  }

  // ================= LOAD PERMISSIONS =================
  loadPermissions(): void {
    this.permissionService.getAllPermission().subscribe({
      next: (res) => {
        this.permissions = res?.permissions ?? [];
        this.permissionRows = this.flattenPermissions(this.permissions);
        this.filteredPermissionRows = [...this.permissionRows];
        this.buildParentMap();
        this.checkedKeys = [...this.selectedPermissionKeys];
        this.cdr.detectChanges();
      },
      error: () => this.message.error('Failed to load permissions'),
    });
  }

  private flattenPermissions(
    permissions: Permission[],
    parentId: number | null = null,
    level: number = 0
  ): PermissionsTree[] {
    return permissions
      .filter((p) => p.parentPermissionId === parentId)
      .map((p) => ({
        ...p,
        level,
        expand: true,
        children: this.flattenPermissions(permissions, p.id, level + 1),
      }));
  }

  private buildParentMap() {
    const map = new Map<number, any>();
    const addToMap = (rows: any[]) => {
      for (const r of rows) {
        map.set(r.id, r);
        if (r.children) addToMap(r.children);
      }
    };
    addToMap(this.permissionRows);
    this.permissionParentMap = map;
  }

  get selectedPermissionKeys(): string[] {
    return (this.selectedPermissionIds || []).map((id) => String(id));
  }

  togglePermissionExpand(row: PermissionsTree) {
    row.expand = !row.expand;
  }

  // ================= SEARCH PERMISSIONS =================
  onPermissionSearch(search: string) {
    const filterTree = (rows: PermissionsTree[]): PermissionsTree[] => {
      return rows
        .map((row) => {
          const children = row.children ? filterTree(row.children) : [];
          const match = row.name.toLowerCase().includes(search.toLowerCase());
          if (match || children.length > 0) {
            return { ...row, children, expand: true };
          }
          return null;
        })
        .filter(Boolean) as PermissionsTree[];
    };

    if (search.trim()) {
      this.filteredPermissionRows = filterTree(this.permissionRows);
      this.cdr.detectChanges();
    } else {
      this.filteredPermissionRows = [...this.permissionRows];
    }
  }

  // ================= CHECKBOX SELECTION =================
  onPermissionCheck(event: any, id: number) {
    const isChecked = event.target.checked;
    this.updatePermissionSelection(id, isChecked);
  }

  updatePermissionSelection(id: number, isChecked: boolean) {
    const updateChildren = (node: PermissionsTree, check: boolean) => {
      if (check) {
        if (!this.selectedPermissionIds.includes(node.id))
          this.selectedPermissionIds.push(node.id);
      } else {
        this.selectedPermissionIds = this.selectedPermissionIds.filter((x) => x !== node.id);
      }
      node.children?.forEach((child: PermissionsTree) => updateChildren(child, check));
    };

    const updateParent = (node: PermissionsTree) => {
      if (!node.parentPermissionId) return;
      const parent = this.permissionParentMap.get(node.parentPermissionId);
      if (!parent) return;

      const allChecked =
        parent.children?.every((child: PermissionsTree) =>
          this.selectedPermissionIds.includes(child.id)
        ) ?? false;
      const someChecked =
        parent.children?.some((child: PermissionsTree) =>
          this.selectedPermissionIds.includes(child.id)
        ) ?? false;

      if (allChecked) {
        if (!this.selectedPermissionIds.includes(parent.id))
          this.selectedPermissionIds.push(parent.id);
      } else {
        this.selectedPermissionIds = this.selectedPermissionIds.filter((x) => x !== parent.id);
      }

      parent.indeterminate = !allChecked && someChecked;
      updateParent(parent);
    };

    const findNodeById = (nodes: PermissionsTree[], targetId: number): PermissionsTree | null => {
      for (const node of nodes) {
        if (node.id === targetId) return node;
        const found = node.children ? findNodeById(node.children, targetId) : null;
        if (found) return found;
      }
      return null;
    };

    const node = findNodeById(this.permissionRows, id);
    if (!node) return;

    updateChildren(node, isChecked);
    updateParent(node);
    this.buildParentMap();
  }

  // ================= SELECT / DESELECT ALL =================
  selectAllPermissions(): void {
    const collectIds = (nodes: PermissionsTree[]): number[] => {
      let ids: number[] = [];
      for (const node of nodes) {
        ids.push(node.id);
        if (node.children && node.children.length > 0) {
          ids = ids.concat(collectIds(node.children));
        }
      }
      return ids;
    };
    this.selectedPermissionIds = collectIds(this.permissionRows);
    this.checkedKeys = this.selectedPermissionIds.map((id) => String(id));
    this.cdr.detectChanges();
  }

  deselectAllPermissions(): void {
    this.selectedPermissionIds = [];
    this.checkedKeys = [];
    this.cdr.detectChanges();
  }

  // ================= SUBMIT + CANCEL =================
  onSubmit(): void {
    if (this.roleForm.invalid) {
      this.message.warning('Please fill required fields.');
      return;
    }

    this.isLoading = true;
    const { isSubsidiary, creationDate, lastModified, ...formValue } =
      this.roleForm.getRawValue();

    const payload: any = {
      ...formValue,
      id: this.isEditMode ? this.currentEditId : undefined,
      ParentRoleId:
        isSubsidiary && formValue.parentRoleId ? Number(formValue.parentRoleId) : null,
      PermissionIds: this.selectedPermissionIds,
      CreationDate: this.isEditMode
        ? new Date(creationDate!).toISOString()
        : new Date().toISOString(),
      LastModified: new Date().toISOString(),
      IsActive: formValue.isActive,
    };

    const request = this.isEditMode
      ? this.roleService.update(this.currentEditId!, payload)
      : this.roleService.create(payload);

    request.subscribe({
      next: (res) => {
        this.isLoading = false;
        this.message.success(
          res?.message ||
            (this.isEditMode ? 'Role updated successfully!' : 'Role created successfully!')
        );
        this.router.navigate(['/roles']);
      },
      error: (error: HttpErrorResponse) => {
        this.isLoading = false;
        this.message.error(
          error.error?.message ||
            (this.isEditMode ? 'Failed to update role' : 'Failed to create role')
        );
      },
    });
  }

  onCancel(): void {
    this.router.navigate(['/roles']);
  }


  isPermissionRowVisible(row: PermissionsTree): boolean {
  if (this.permissionSearch.trim()) {
    // when searching, show all matches + ancestors
    return true;
  }

  if (!row.parentPermissionId) return true; // root level always visible
  const parent = this.permissionParentMap.get(row.parentPermissionId);
  if (!parent) return true;

  return parent.expand && this.isPermissionRowVisible(parent);
}


  closePopup() {
      this.router.navigate(["/roles"]);
    }


}
