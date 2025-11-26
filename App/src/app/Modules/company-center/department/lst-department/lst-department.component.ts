import { Component, OnInit, OnDestroy, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";
import { NzModalModule } from "ng-zorro-antd/modal";
import { NzMessageService } from "ng-zorro-antd/message";
import { Subscription } from "rxjs";
import { ColDef } from "ag-grid-community";
import {
  DepartmentService,
  Department,
} from "../../../../shared/services/department.service";
import { GridComponent } from "../../../../shared/components/grid/grid.component";
import { AfterViewInit, ElementRef, ViewChild } from "@angular/core";
import { formatPKTDate } from "../../../../shared/components/dateTime.util";
@Component({
  selector: "app-lst-department",
  standalone: true,
  imports: [CommonModule, NzModalModule, GridComponent],
  templateUrl: "./lst-department.component.html",
  styleUrls: [
    "../../../../../scss/lists.css",
    "./lst-department.component.css",
  ],
})
export class LstDepartmentComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  selectedStatus = "all";
  rowData: any[] = [];
  isLoading = false;
  selectedDepartment: Department | null = null;
  readonly GRID_LAYOUT_KEY = "department-grid-layout";
  private subscriptions = new Subscription();
  allExpanded = false;
  /** Full tree structure (never modified after load) */
  private fullTree: any[] = [];

  @ViewChild("leftPane") leftPane!: ElementRef<HTMLElement>;
  @ViewChild("rightPane") rightPane!: ElementRef<HTMLElement>;
  @ViewChild("hSplitter") hSplitter!: ElementRef<HTMLElement>;

  @ViewChild("companiesPane") companiesPane!: ElementRef<HTMLElement>;
  @ViewChild("employeesPane") employeesPane!: ElementRef<HTMLElement>;
  @ViewChild("vSplitter") vSplitter!: ElementRef<HTMLElement>;

  private isDraggingH = false;
  private isDraggingV = false;

  ngAfterViewInit(): void {
    this.initHorizontalSplitter();
    this.initVerticalSplitter();
  }
  private initHorizontalSplitter() {
    const splitter = this.hSplitter.nativeElement;
    const left = this.leftPane.nativeElement;
    const right = this.rightPane.nativeElement;

    splitter.addEventListener("mousedown", (e: MouseEvent) => {
      this.isDraggingH = true;
      e.preventDefault();
    });

    document.addEventListener("mousemove", (e: MouseEvent) => {
      if (!this.isDraggingH) return;
      const container = splitter.parentElement!;
      const rect = container.getBoundingClientRect();
      const offset = e.clientX - rect.left;
      const percent = (offset / rect.width) * 100;

      // keep between 20% and 80%
      const clamped = Math.min(Math.max(percent, 20), 80);
      left.style.flexBasis = `${clamped}%`;
      right.style.flexBasis = `${100 - clamped}%`;
    });

    document.addEventListener("mouseup", () => (this.isDraggingH = false));
  }

  private initVerticalSplitter() {
    const splitter = this.vSplitter.nativeElement;
    const top = this.companiesPane.nativeElement;
    const bottom = this.employeesPane.nativeElement;

    splitter.addEventListener("mousedown", (e: MouseEvent) => {
      this.isDraggingV = true;
      e.preventDefault();
    });

    document.addEventListener("mousemove", (e: MouseEvent) => {
      if (!this.isDraggingV) return;
      const container = splitter.parentElement!;
      const rect = container.getBoundingClientRect();
      const offset = e.clientY - rect.top;
      const percent = (offset / rect.height) * 100;

      const clamped = Math.min(Math.max(percent, 20), 80);
      top.style.flexBasis = `${clamped}%`;
      bottom.style.flexBasis = `${100 - clamped}%`;
    });

    document.addEventListener("mouseup", () => (this.isDraggingV = false));
  }

  columnDefs: ColDef[] = [
    {
      field: "deptName",
      headerName: "Department Name",
      width: 340,
      cellRenderer: this.getDepartmentNameCellRenderer.bind(this),
      filter: "agTextColumnFilter",
    },
    {
      field: "deptCode",
      headerName: "Code",
      width: 140,
      filter: "agTextColumnFilter",
    },
    {
      field: "abbreviation",
      headerName: "Abbr",
      width: 100,
      filter: "agTextColumnFilter",
    },
    {
      field: "isSubsidiary",
      headerName: "Type",
      width: 120,
      valueFormatter: (p) => (p.value ? "Subsidiary" : "Main"),
      cellRenderer: (p: { value: any }) =>
        p.value
          ? '<span class="badge bg-info">Sub</span>'
          : '<span class="badge bg-secondary">Main</span>',
    },
    {
      field: "isActive",
      headerName: "Status",
      width: 110,
      cellRenderer: (p: { value: any }) =>
        p.value
          ? '<span class="badge bg-success">Active</span>'
          : '<span class="badge bg-danger">Inactive</span>',
    },
    {
      field: "createdByUserName",
      headerName: "Created By",
      width: 150,
      valueGetter: (p) => p.data?.createdByUserName || "—",
      filter: "agTextColumnFilter",
    },
    {
      field: "createdDate",
      headerName: "Created",
      width: 160,
      valueGetter: (p) => formatPKTDate(p.data?.createdDate),
      filter: "agDateColumnFilter",
    },
    {
      field: "lastModifiedByUserName",
      headerName: "Last Modified By",
      width: 150,
      valueGetter: (p) => p.data?.lastModifiedByUserName || "—",
      filter: "agTextColumnFilter",
    },
    {
      field: "lastModifiedDate",
      headerName: "Modified",
      width: 160,
      valueGetter: (p) => formatPKTDate(p.data?.lastModifiedDate),
      filter: "agDateColumnFilter",
    },
  ];

  constructor(
    private departmentService: DepartmentService,
    private router: Router,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  // ── Load & Build Tree (ONCE) ──
  loadDepartments(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.departmentService.getDepartments(this.selectedStatus).subscribe({
        next: (data: Department[]) => {
          const map = new Map<number, any>();
          const roots: any[] = [];

          // 1. Clone + add UI fields
          data.forEach((d) => {
            const node = {
              ...d,
              level: 0,
              expand: false,
              children: [],
            };
            map.set(d.id, node);
            if (!d.parentDepartmentId || !d.isSubsidiary) {
              roots.push(node);
            }
          });

          // 2. Attach children + set level
          data.forEach((d) => {
            if (d.parentDepartmentId && d.isSubsidiary) {
              const parent = map.get(d.parentDepartmentId);
              const child = map.get(d.id);
              if (parent && child) {
                child.level = parent.level + 1;
                parent.children.push(child);
              }
            }
          });

          // 3. Save full tree
          this.fullTree = roots;

          // 4. Flatten initially (collapsed)
          this.rowData = this.flattenTree(this.fullTree);
          this.isLoading = false;
          this.cdr.markForCheck();
        },
        error: (err) => {
          console.error(err);
          this.isLoading = false;
          this.message.error("Failed to load departments.");
        },
      })
    );
  }

  /** Flatten tree based on expand state */
  private flattenTree(nodes: any[], result: any[] = []): any[] {
    nodes.forEach((node) => {
      result.push(node);
      if (node.expand && node.children.length > 0) {
        this.flattenTree(node.children, result);
      }
    });
    return result;
  }

  getDepartmentNameCellRenderer(params: any): string {
    const dept = params.data;
    const indent = dept.level * 22;
    const hasChildren = dept.children && dept.children.length > 0;
    const icon = hasChildren
      ? `<i class="bi ${
          dept.expand ? "bi-caret-down-fill" : "bi-caret-right-fill"
        } text-primary toggle-icon" data-id="${dept.id}"></i>`
      : "";

    return `
    <span style="display:inline-flex; align-items:center; gap:6px; padding-left:${indent}px;">
      ${icon}
      <span class="dept-name" data-id="${dept.id}">${dept.deptName}</span>
    </span>
  `;
  }

  toggleAllExpand(): void {
    this.allExpanded = !this.allExpanded;

    // walk the *immutable* fullTree and set every node.expand
    const walk = (nodes: any[]) => {
      nodes.forEach((n) => {
        n.expand = this.allExpanded; // <-- mutate the original node
        if (n.children?.length) walk(n.children);
      });
    };
    walk(this.fullTree);

    // rebuild the flat list
    this.rowData = this.flattenTree(this.fullTree);
    this.cdr.markForCheck();
  }

  private updateAllExpandedFlag(): void {
    const walk = (nodes: any[]): boolean => {
      return nodes.every((n) => {
        const childrenAll = n.children?.length ? walk(n.children) : true;
        return n.expand && childrenAll;
      });
    };
    this.allExpanded = this.fullTree.length ? walk(this.fullTree) : false;
  }
  onRowClicked(event: any): void {
    const dept = event.data;
    const clicked = event.event?.target as HTMLElement;

    if (clicked?.classList.contains("toggle-icon")) {
      // ---- single node toggle ------------------------------------------------
      dept.expand = !dept.expand;

      // keep the global flag correct (optional but nice UX)
      this.updateAllExpandedFlag();

      this.rowData = this.flattenTree(this.fullTree);
      this.cdr.markForCheck();
      return;
    }

    this.gridSelectRow(event);
    this.selectedDepartment = dept;
    this.cdr.markForCheck();
  }
  private gridSelectRow(event: any): void {
    if (event.node) {
      event.node.setSelected(true, true); // select and deselect others
    }
    const dept = event.data;
    sessionStorage.setItem("selectedDepartment", JSON.stringify(dept));
  }

  // ── Navigation ──
  navigateToCreateDepartment(): void {
    sessionStorage.removeItem("selectedDepartment");
    this.router.navigate(["/frm-department"]);
  }

  navigateToUpdateDepartment(dept: Department): void {
    if (!dept?.id) {
      this.message.warning("Please select a department.");
      return;
    }
    this.router.navigate([`/frm-department/${dept.id}`], {
      state: { department: dept },
    });
  }

  // ── Helpers ──
  private formatDate(date: string | Date): string {
    return date ? new Date(date).toLocaleDateString() : "";
  }

  refreshDepartments(): void {
    this.loadDepartments();
    this.message.success("Refreshed!");
  }

  setFilterState(status: string): void {
    this.selectedStatus =
      status === "active"
        ? "active"
        : status === "inactive"
        ? "inactive"
        : "all";
    this.loadDepartments();
  }
}
