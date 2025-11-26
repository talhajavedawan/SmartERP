import { Component, OnInit, ChangeDetectorRef } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpErrorResponse } from "@angular/common/http";
import { NzButtonModule } from "ng-zorro-antd/button";
import { NzMessageService } from "ng-zorro-antd/message";
import { Router, RouterModule } from "@angular/router";
import { CurrencyService } from "../../../../shared/services/currency.service";
import { GridComponent } from "../../../../shared/components/grid/grid.component";
import { CurrencyGetDto } from "../../model/currency.model";
import { NzPaginationModule } from "ng-zorro-antd/pagination";

@Component({
  selector: "app-lst-currency",
  standalone: true,
  templateUrl: "./lst-currency.component.html",
  styleUrls: ["./lst-currency.component.css"],
  imports: [
    CommonModule,
    RouterModule,
    NzButtonModule,
    GridComponent,
    NzPaginationModule,
  ],
})
export class LstCurrencyComponent implements OnInit {
  /** ===================== STATE ===================== */
  currencies: CurrencyGetDto[] = [];
  selectedCurrency: CurrencyGetDto | null = null;

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
    { headerName: "Currency Name", field: "name", flex: 2 },
    { headerName: "Symbol", field: "symbol", flex: 1 },
    { headerName: "Abbreviation", field: "abbreviation", flex: 1 },
    { headerName: "Country", field: "countryName", flex: 2 },
  ];

  constructor(
    private currencyService: CurrencyService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private message: NzMessageService
  ) {}

  /** ===================== INIT ===================== */
  ngOnInit(): void {
    this.loadCurrencies();
  }

  /** ===================== API CALL ===================== */
  private loadCurrencies(): void {
    this.isLoading = true;

    this.currencyService
      .getAllCurrencies(
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
       this.currencies = data.map((c) => ({
  id: c.id,
  name: c.name ?? "—",
  symbol: c.symbol ?? "-",
  abbreviation: c.abbreviation ?? "-",
  countryId: c.countryId ?? null,
  countryName: c.countryName ?? "-",
}));


          this.totalCount = res?.totalCount ?? this.currencies.length;
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err: HttpErrorResponse) => {
          console.error("Error fetching currencies:", err);
          this.message.error("❌ Failed to load currencies.");
          this.isLoading = false;
        },
      });
  }

  /** ===================== STATUS FILTER ===================== */
  setStatusFilter(status: "all" | "active" | "inactive"): void {
    if (this.selectedStatus === status) return;
    this.selectedStatus = status;
    this.pageNumber = 1;
    this.loadCurrencies();
  }

  /** ===================== GRID ACTIONS ===================== */
  onAdd(): void {
    this.router.navigate(["/frmCurrency"]);
  }

  onUpdate(row: CurrencyGetDto): void {
    if (!row?.id) {
      this.message.warning("Please select a currency to edit.");
      return;
    }
    this.router.navigate([`/frmCurrency/${row.id}`]);
  }

  onRefresh(): void {
    this.loadCurrencies();
    this.message.success("✅ Currency list refreshed successfully!");
  }

  onSearch(value: string): void {
    this.searchTerm = value.trim();
    this.pageNumber = 1;
    this.loadCurrencies();
  }

  onRowClicked(event: any): void {
    this.selectedCurrency = event?.data ?? null;
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
    this.loadCurrencies();
  }

  /** ===================== PAGINATION ===================== */
  onPageChange(page: number): void {
    this.pageNumber = page;
    this.loadCurrencies();
  }

  totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}
