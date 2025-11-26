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
  StatusService,
  TransactionItemType,
  Status,
} from "../../../../../shared/services/status.service";
import { GridComponent } from "../../../../../shared/components/grid/grid.component";
import { ColDef, ICellRendererParams } from "ag-grid-community";
import { formatPKTDate } from "../../../../../shared/components/dateTime.util";
@Component({
  selector: "app-lst-employee-status",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzModalModule, GridComponent],
  templateUrl: "./lst-employee-status.component.html",
  styleUrls: ["../../../../../../scss/lists.css"],
})
export class LstEmployeeStatusComponent implements OnInit, OnDestroy {
  statuses: Status[] = [];
  filteredStatuses: Status[] = [];
  isLoading = false;
  selectedStatus: "all" | "active" | "inactive" = "all";

  readonly GRID_LAYOUT_KEY = "employee-status-grid-layout";
  private subscriptions = new Subscription();
  columns: ColDef[] = [
    {
      field: "statusName",
      headerName: "Status Name",
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
    private statusService: StatusService,
    private message: NzMessageService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(DOCUMENT) private document: Document
  ) {}

  ngOnInit(): void {
    this.loadStatuses();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  // ────── LOAD DATA ──────
  loadStatuses(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.statusService
        .getAllStatuses(TransactionItemType.Employee, this.selectedStatus)
        .subscribe({
          next: (data: Status[]) => {
            this.statuses = data || [];
            this.filteredStatuses = [...this.statuses];
            this.isLoading = false;
            this.cdr.detectChanges();
          },
          error: () => {
            this.isLoading = false;
            this.message.error("Failed to load employee statuses.");
            this.cdr.detectChanges();
          },
        })
    );
  }

  // ────── STATUS FILTER ──────
  setStatusFilter(status: "all" | "active" | "inactive"): void {
    this.selectedStatus = status;
    this.loadStatuses();
  }

  // ────── ROW SELECTION ──────
  onRowSelected(status: Status | null): void {
    if (status) {
      this.selectStatus(status);
    } else {
      this.document.defaultView?.sessionStorage.removeItem("selectedStatus");
    }
    this.cdr.detectChanges();
  }

  private selectStatus(status: Status): void {
    this.document.defaultView?.sessionStorage.setItem(
      "selectedStatus",
      JSON.stringify(status)
    );
  }

  private getSelectedStatus(): Status | null {
    const stored =
      this.document.defaultView?.sessionStorage.getItem("selectedStatus");
    return stored ? JSON.parse(stored) : null;
  }

  // ────── CRUD NAVIGATION ──────
  navigateToCreate(): void {
    this.document.defaultView?.sessionStorage.removeItem("selectedStatus");
    this.router.navigate(["/employeeStatus-form"]);
  }

  navigateToEdit(status?: Status | null): void {
    const target = status ?? this.getSelectedStatus();
    if (!target?.id) {
      this.message.warning("Please select a status to edit.");
      return;
    }

    this.document.defaultView?.sessionStorage.setItem(
      "selectedStatus",
      JSON.stringify(target)
    );
    this.router.navigate(["/employeeStatus-form"], {
      queryParams: { id: target.id },
    });
  }

  // ────── SEARCH + REFRESH ──────
  onSearch(term: string): void {
    const lower = term.trim().toLowerCase();
    this.filteredStatuses = this.statuses.filter((s) =>
      (s.statusName || "").toLowerCase().includes(lower)
    );
    this.cdr.detectChanges();
  }

  refreshStatuses(): void {
    this.loadStatuses();
    this.message.success("Status list refreshed!");
  }
}
