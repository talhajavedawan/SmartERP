import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpErrorResponse } from "@angular/common/http";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzMessageService } from "ng-zorro-antd/message";
import { Router, RouterModule } from "@angular/router";
import { VendorNatureService } from "../../../../shared/services/vendor/vendor-nature.service";
import { GridComponent } from "../../../../shared/components/grid/grid.component";
import { VendorNature } from "../vendor-nature.model";
import { NzPaginationModule } from "ng-zorro-antd/pagination";

@Component({
  selector: "app-lst-vendor-nature",
  standalone: true,
  templateUrl: "./lst-vendor-nature.component.html",
  styleUrls: ["./lst-vendor-nature.component.css"],
  imports: [
    CommonModule,
    NzButtonModule,
    RouterModule,
    GridComponent,
    NzPaginationModule,
  ],
})
export class LstVendorNatureComponent implements OnInit {
  /** ===================== STATE ===================== */
  vendorNatures: VendorNature[] = [];
  filteredVendorNatures: VendorNature[] = [];
  selectedVendorNature: VendorNature | null = null;

  isLoading = false;
  selectedStatus: "all" | "active" | "inactive" = "all";

  /** ===================== PAGINATION ===================== */
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;

  /** ===================== SORTING + SEARCH ===================== */
  sortColumn = "name";
  sortDirection: "asc" | "desc" = "asc";
  searchTerm = "";

  /** ===================== GRID CONFIG ===================== */
  columnDefs = [
    {
      headerName: "Vendor Nature Name",
      field: "name",
      flex: 2,
    },
    {
      headerName: "Status",
      field: "isActive",
      flex: 1,
      cellRenderer: (params: any) => {
        const active = !!params.value;
        const badgeClass = active ? "badge bg-success" : "badge bg-danger";
        const label = active ? "Active" : "Inactive";
        return `<span class="${badgeClass}">${label}</span>`;
      },
    },
  ];

  constructor(
    private vendorNatureService: VendorNatureService,
    private message: NzMessageService,
    private cdr: ChangeDetectorRef,
    private router: Router
  ) {}

  /** ===================== INIT ===================== */
  ngOnInit(): void {
    this.loadVendorNatures();
  }

  /** ===================== API CALL ===================== */
  private loadVendorNatures(): void {
    this.isLoading = true;

    this.vendorNatureService
      .getAllVendorNatures(
        this.selectedStatus,
        this.pageNumber,
        this.pageSize,
        this.sortColumn,
        this.sortDirection,
        this.searchTerm
      )
      .subscribe({
        next: (res) => {
          const data = res?.data ?? [];
          this.vendorNatures = data.map((v) => ({
            id: v.id,
            name: v.name ?? "—",
            isActive: v.isActive ?? false,
          }));

          this.totalCount = res?.totalCount ?? this.vendorNatures.length;
          this.applyStatusFilter();

          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err: HttpErrorResponse) => {
          console.error("Error loading vendor natures:", err);
          this.message.error("❌ Failed to load vendor natures.");
          this.isLoading = false;
        },
      });
  }

  /** ===================== STATUS FILTER ===================== */
  setStatusFilter(status: "all" | "active" | "inactive"): void {
    if (this.selectedStatus === status) return;
    this.selectedStatus = status;
    this.pageNumber = 1;
    this.loadVendorNatures();
  }

  private applyStatusFilter(): void {
    if (this.selectedStatus === "active") {
      this.filteredVendorNatures = this.vendorNatures.filter((v) => v.isActive);
    } else if (this.selectedStatus === "inactive") {
      this.filteredVendorNatures = this.vendorNatures.filter((v) => !v.isActive);
    } else {
      this.filteredVendorNatures = [...this.vendorNatures];
    }
  }

  /** ===================== GRID ACTIONS ===================== */
  onAdd(): void {
    this.router.navigate(["/frm-vendor-nature"]);
  }

  onUpdate(row: VendorNature): void {
    if (!row?.id) {
      this.message.warning("Please select a vendor nature to edit.");
      return;
    }
    this.router.navigate(["/frm-vendor-nature"], { queryParams: { id: row.id } });
  }

  onRefresh(): void {
    this.loadVendorNatures();
    this.message.success("✅ Vendor Nature list refreshed successfully!");
  }

  onSearch(value: string): void {
    this.searchTerm = value.trim();
    this.pageNumber = 1;
    this.loadVendorNatures();
  }

  onRowClicked(event: any): void {
    this.selectedVendorNature = event?.data ?? null;
  }

  /** ===================== SORTING ===================== */
  onSortChange(event: any): void {
    const sortModel = event?.api?.getSortModel?.() ?? [];
    if (sortModel.length > 0) {
      this.sortColumn = sortModel[0].colId;
      this.sortDirection = sortModel[0].sort ?? "asc";
    } else {
      this.sortColumn = "name";
      this.sortDirection = "asc";
    }
    this.loadVendorNatures();
  }

  /** ===================== PAGINATION ===================== */
  onPageChange(page: number): void {
    this.pageNumber = page;
    this.loadVendorNatures();
  }

  totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}
