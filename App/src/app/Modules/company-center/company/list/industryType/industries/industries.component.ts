import {
  Component,
  OnInit,
  OnDestroy,
  ChangeDetectorRef,
  Inject,
} from "@angular/core";
import { CommonModule, DOCUMENT } from "@angular/common";
import { FormControl, ReactiveFormsModule } from "@angular/forms";
import { Router } from "@angular/router";
import { Subscription, debounceTime, distinctUntilChanged } from "rxjs";
import { NzMessageService } from "ng-zorro-antd/message";
import { NzModalModule } from "ng-zorro-antd/modal";
import {
  IndustryService,
  IndustryType,
} from "../../../../../../shared/services/company-center/company/lists/industry.service";
import { UserSettingService } from "../../../../../../shared/services/user-setting.service";
import { GridComponent } from "../../../../../../shared/components/grid/grid.component";
import { ColDef, ICellRendererParams } from "ag-grid-community";
import { formatPKTDate } from "../../../../../../shared/components/dateTime.util";
@Component({
  selector: "app-industries",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzModalModule, GridComponent],
  templateUrl: "./industries.component.html",
  styleUrls: ["../../../../../../../scss/lists.css"],
})
export class IndustriesComponent implements OnInit, OnDestroy {
  // ===== Search & State =====
  searchControl = new FormControl("");
  industries: IndustryType[] = [];
  filteredIndustries: IndustryType[] = [];
  isLoading = false;
  selectedStatus: string = "all";
  readonly GRID_LAYOUT_KEY = "industry-grid-layout";
  private subscriptions = new Subscription();

  // ===== Grid Config =====
  columns: ColDef[] = [
    {
      field: "industryTypeName",
      headerName: "Industry Name",
      minWidth: 200,
      hide: false,
      filter: "agTextColumnFilter",
    },
    {
      field: "isActive",
      headerName: "Status",
      minWidth: 100,
      cellRenderer: (params: ICellRendererParams) => {
        const isActive = params.value;
        return `<span class="badge ${isActive ? "bg-success" : "bg-danger"}">${
          isActive ? "Active" : "Inactive"
        }</span>`;
      },
      hide: false,
    },
    {
      field: "createdByUserName",
      headerName: "Created By",
      minWidth: 150,
      hide: false,
      filter: "agTextColumnFilter",
      valueGetter: (params) => params.data?.createdByUserName || "-",
    },
    {
      field: "createdDate",
      headerName: "Created Date",
      minWidth: 180,
      hide: false,
      valueGetter: (params) => formatPKTDate(params.data?.createdDate),
      filter: "agDateColumnFilter",
    },
    {
      field: "lastModifiedByUserName",
      headerName: "Last Modified By",
      minWidth: 150,
      hide: false,
      filter: "agTextColumnFilter",
      valueGetter: (params) => params.data?.lastModifiedByUserName || "-",
    },
    {
      field: "lastModifiedDate",
      headerName: "Last Modified Date",
      minWidth: 180,
      hide: false,
      valueGetter: (params) => formatPKTDate(params.data?.lastModifiedDate),
      filter: "agDateColumnFilter",
    },
  ];

  constructor(
    private industryService: IndustryService, // Replaced CompanyService
    private message: NzMessageService,
    private userSettingService: UserSettingService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(DOCUMENT) private document: Document
  ) {}

  // ===== Lifecycle =====
  ngOnInit(): void {
    this.loadGridLayout();
    this.loadIndustries();
    this.subscriptions.add(
      this.searchControl.valueChanges
        .pipe(debounceTime(300), distinctUntilChanged())
        .subscribe((val: string | null) => this.filterIndustries(val ?? ""))
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }

  // ===== Load Grid Layout =====
  loadGridLayout(): void {
    this.subscriptions.add(
      this.userSettingService.getSetting(this.GRID_LAYOUT_KEY).subscribe({
        next: (layout) => {
          if (layout) {
            const savedLayout = JSON.parse(layout);
            this.columns = this.columns.map((col) => ({
              ...col,
              ...savedLayout.find((l: any) => l.field === col.field),
            }));
            this.cdr.detectChanges();
          }
        },
        error: (err) => {
          console.error("Error loading grid layout:", err);
          this.message.error("Failed to load grid layout.");
        },
      })
    );
  }

  // ===== Load & Filter =====
  loadIndustries(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.industryService.getAllIndustries(this.selectedStatus).subscribe({
        next: (data: IndustryType[]) => {
          this.industries = data || [];
          this.filteredIndustries = [...this.industries];
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error("Error loading industries:", err);
          this.isLoading = false;
          this.message.error("Failed to load industries.");
        },
      })
    );
  }

  setFilterState(status: string): void {
    this.selectedStatus = status;
    this.loadIndustries();
  }

  filterIndustries(value: string = ""): void {
    const searchVal = value.trim().toLowerCase();
    this.filteredIndustries = this.industries.filter((i) =>
      (i.industryTypeName || "").toLowerCase().includes(searchVal)
    );
    this.cdr.detectChanges();
  }

  // ===== Row & CRUD =====
  onRowClicked(event: any): void {
    const industry = event?.data as IndustryType;
    this.selectIndustry(industry);
  }

  goToAddIndustry(): void {
    this.document.defaultView?.sessionStorage.removeItem("selectedIndustry");
    this.router.navigate(["/industry-form"]);
  }

  editIndustry(industry?: IndustryType | null): void {
    const target = industry ?? this.getSelectedIndustry();
    if (!target || target.id === undefined) {
      this.message.warning("Please select an industry to update.");
      return;
    }
    this.document.defaultView?.sessionStorage.setItem(
      "selectedIndustry",
      JSON.stringify(target)
    );
    this.router.navigate(["/industry-form"], { queryParams: { isEdit: true } });
  }

  deleteSelectedIndustry(): void {
    const selected = this.getSelectedIndustry();
    if (!selected || selected.id === undefined) {
      this.message.warning("Please select an industry to delete.");
      return;
    }
    this.message.info(`Deleting industry: ${selected.industryTypeName}`);
    // TODO: Implement delete API call
  }

  getSelectedIndustry(): IndustryType | null {
    const stored =
      this.document.defaultView?.sessionStorage.getItem("selectedIndustry");
    return stored ? JSON.parse(stored) : null;
  }

  selectIndustry(industry: IndustryType): void {
    this.document.defaultView?.sessionStorage.setItem(
      "selectedIndustry",
      JSON.stringify(industry)
    );
    this.cdr.detectChanges();
  }

  // ===== Layout Sync =====
  onLayoutChanged(): void {
    const layout = JSON.stringify(
      this.columns.map(({ field, hide, width }) => ({ field, hide, width }))
    );
    this.subscriptions.add(
      this.userSettingService
        .saveSetting(this.GRID_LAYOUT_KEY, layout)
        .subscribe({
          next: () => this.message.success("Layout saved successfully!"),
          error: (err) => {
            console.error("Error saving grid layout:", err);
            this.message.error("Failed to save grid layout.");
          },
        })
    );
  }

  refreshIndustries(): void {
    this.loadIndustries();
    this.message.success("Industry list refreshed successfully!");
  }

  // Handle row selection from GridComponent
  onRowSelected(industry: IndustryType | null): void {
    if (industry) {
      this.selectIndustry(industry);
    } else {
      this.document.defaultView?.sessionStorage.removeItem("selectedIndustry");
    }
    this.cdr.detectChanges();
  }

  // Handle search from GridComponent
  onSearch(value: string): void {
    this.searchControl.setValue(value, { emitEvent: false });
    this.filterIndustries(value);
  }
}
