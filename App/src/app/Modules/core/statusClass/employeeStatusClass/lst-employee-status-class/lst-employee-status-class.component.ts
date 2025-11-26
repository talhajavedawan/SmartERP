import {
  Component,
  OnInit,
  OnDestroy,
  ChangeDetectorRef,
  Inject,
} from "@angular/core";
import { CommonModule, DOCUMENT } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { NzMessageService } from "ng-zorro-antd/message";
import { NzModalModule } from "ng-zorro-antd/modal";

import {
  StatusClassService,
  TransactionItemType,
  StatusClass,
} from "../../../../../shared/services/statusClass.service";
import { GridComponent } from "../../../../../shared/components/grid/grid.component";
import { ColDef, ICellRendererParams } from "ag-grid-community";
import { formatPKTDate } from "../../../../../shared/components/dateTime.util";

@Component({
  selector: "app-lst-employee-status-class",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzModalModule, GridComponent],
  templateUrl: "./lst-employee-status-class.component.html",
  styleUrls: ["../../../../../../scss/lists.css"],
})
export class LstEmployeeStatusClassComponent implements OnInit, OnDestroy {
  statusClasses: StatusClass[] = [];
  filteredStatusClasses: StatusClass[] = [];
  isLoading = false;
  selectedStatus: "all" | "active" | "inactive" = "all";

  readonly GRID_LAYOUT_KEY = "employee-status-class-grid-layout";
  private subscriptions = new Subscription();

  columns: ColDef[] = [
    {
      field: "className",
      headerName: "Class Name",
      minWidth: 220,
      filter: "agTextColumnFilter",
    },
    {
      field: "statusName",
      headerName: "Status",
      minWidth: 200,
      filter: "agTextColumnFilter",
    },
    {
      field: "foreColor",
      headerName: "Fore Color",
      minWidth: 130,
      cellStyle: {
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
      },
      cellRenderer: (params: ICellRendererParams) => {
        const color = params.value || "#000000";
        return `
        <div style="display:flex;align-items:center;justify-content:center;width:100%;">
          <span style="width:18px;height:18px;border-radius:50%;border:1px solid #ccc;background:${color};"></span>
        </div>
      `;
      },
    },
    {
      field: "isApproved",
      headerName: "Approval",
      minWidth: 120,
      cellRenderer: (params: ICellRendererParams) => {
        const approved = params.value === true;
        return `<span class="badge ${approved ? "bg-info" : "bg-secondary"}">
                ${approved ? "Approved" : "Pending"}
              </span>`;
      },
    },
    {
      field: "isActive",
      headerName: "Status",
      minWidth: 120,
      cellRenderer: (params: ICellRendererParams) => {
        const active = params.value === true;
        return `<span class="badge ${active ? "bg-success" : "bg-danger"}">
                ${active ? "Active" : "Inactive"}
              </span>`;
      },
    },
    {
      field: "createdBy",
      headerName: "Created By",
      minWidth: 150,
      valueFormatter: (p) => p.value || "—",
    },
    {
      field: "creationDate",
      headerName: "Created On",
      minWidth: 180,
      valueGetter: (p) => formatPKTDate(p.data?.creationDate),
    },
    {
      field: "lastModifiedBy",
      headerName: "Modified By",
      minWidth: 150,
      valueFormatter: (p) => p.value || "—",
    },
    {
      field: "modifiedDate",
      headerName: "Last Modified",
      minWidth: 180,
      valueGetter: (p) => formatPKTDate(p.data?.modifiedDate),
    },
  ];

  constructor(
    private statusClassService: StatusClassService,
    private message: NzMessageService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(DOCUMENT) private document: Document
  ) {}

  ngOnInit(): void {
    this.loadStatusClasses();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  // ────── LOAD DATA ──────
  loadStatusClasses(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.statusClassService
        .getAll(TransactionItemType.Employee, this.selectedStatus)
        .subscribe({
          next: (data: StatusClass[]) => {
            this.statusClasses = data || [];
            this.filteredStatusClasses = [...this.statusClasses];
            this.isLoading = false;
            this.cdr.detectChanges();
          },
          error: () => {
            this.isLoading = false;
            this.message.error("Failed to load employee status classes.");
            this.cdr.detectChanges();
          },
        })
    );
  }

  // ────── STATUS FILTER ──────
  setStatusFilter(status: "all" | "active" | "inactive"): void {
    this.selectedStatus = status;
    this.loadStatusClasses();
  }

  // ────── ROW SELECTION ──────
  onRowSelected(statusClass: StatusClass | null): void {
    if (statusClass) {
      this.selectStatusClass(statusClass);
    } else {
      this.document.defaultView?.sessionStorage.removeItem(
        "selectedStatusClass"
      );
    }
    this.cdr.detectChanges();
  }

  private selectStatusClass(statusClass: StatusClass): void {
    this.document.defaultView?.sessionStorage.setItem(
      "selectedStatusClass",
      JSON.stringify(statusClass)
    );
  }

  private getSelectedStatusClass(): StatusClass | null {
    const stored = this.document.defaultView?.sessionStorage.getItem(
      "selectedStatusClass"
    );
    return stored ? JSON.parse(stored) : null;
  }

  // ────── CRUD NAVIGATION ──────
  navigateToCreate(): void {
    this.document.defaultView?.sessionStorage.removeItem("selectedStatusClass");
    this.router.navigate(["/employeeStatusClass-form"]);
  }

  navigateToEdit(statusClass?: StatusClass | null): void {
    const target = statusClass ?? this.getSelectedStatusClass();
    if (!target?.id) {
      this.message.warning("Please select a status class to edit.");
      return;
    }

    this.document.defaultView?.sessionStorage.setItem(
      "selectedStatusClass",
      JSON.stringify(target)
    );
    this.router.navigate(["/employeeStatusClass-form"], {
      queryParams: { id: target.id },
    });
  }

  // ────── SEARCH + REFRESH ──────
  onSearch(term: string): void {
    const lower = term.trim().toLowerCase();
    this.filteredStatusClasses = this.statusClasses.filter((s) =>
      (s.className || "").toLowerCase().includes(lower)
    );
    this.cdr.detectChanges();
  }

  refreshStatusClasses(): void {
    this.loadStatusClasses();
    this.message.success("Status class list refreshed!");
  }
}
