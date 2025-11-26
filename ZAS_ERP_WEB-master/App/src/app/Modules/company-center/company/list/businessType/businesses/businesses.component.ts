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
  BusinessService,
  BusinessType,
} from "../../../../../../shared/services/company-center/company/lists/business.service";
import { UserSettingService } from "../../../../../../shared/services/user-setting.service";
import { GridComponent } from "../../../../../../shared/components/grid/grid.component";
import { ColDef, ICellRendererParams } from "ag-grid-community";
import { formatPKTDate } from "../../../../../../shared/components/dateTime.util";

@Component({
  selector: "app-businesses",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzModalModule, GridComponent],
  templateUrl: "./businesses.component.html",
  styleUrls: ["../../../../../../../scss/lists.css"],
})
export class BusinessesComponent implements OnInit, OnDestroy {
  // ===== State =====
  searchControl = new FormControl("");
  businessTypes: BusinessType[] = [];
  filteredBusinessTypes: BusinessType[] = [];
  isLoading = false;
  selectedStatus: string = "all";
  readonly GRID_LAYOUT_KEY = "business-grid-layout";
  private subscriptions = new Subscription();

  // ===== Grid Columns =====
  columns: ColDef[] = [
    {
      field: "businessTypeName",
      headerName: "Business Type Name",
      minWidth: 200,
      hide: false,
      filter: "agTextColumnFilter",
    },
    {
      field: "isActive",
      headerName: "Status",
      minWidth: 120,
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
    private businessService: BusinessService,
    private message: NzMessageService,
    private userSettingService: UserSettingService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(DOCUMENT) private document: Document
  ) {}

  // ===== Lifecycle =====
  ngOnInit(): void {
    this.loadGridLayout();
    this.loadBusinessTypes();
    this.subscriptions.add(
      this.searchControl.valueChanges
        .pipe(debounceTime(300), distinctUntilChanged())
        .subscribe((val: string | null) => this.filterBusinessTypes(val ?? ""))
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
  loadBusinessTypes(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.businessService.getAllBusinessTypes(this.selectedStatus).subscribe({
        next: (data: BusinessType[]) => {
          this.businessTypes = data || [];
          this.filteredBusinessTypes = [...this.businessTypes];
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error("Error loading business types:", err);
          this.isLoading = false;
          this.message.error("Failed to load business types.");
        },
      })
    );
  }

  setFilterState(status: string): void {
    this.selectedStatus = status;
    this.loadBusinessTypes();
  }

  filterBusinessTypes(value: string = ""): void {
    const searchVal = value.trim().toLowerCase();
    this.filteredBusinessTypes = this.businessTypes.filter((b) =>
      (b.businessTypeName || "").toLowerCase().includes(searchVal)
    );
    this.cdr.detectChanges();
  }

  // ===== Row Click =====
  onRowClicked(event: any): void {
    const business = event?.data as BusinessType;
    this.selectBusiness(business);
  }

  // ===== Session Handling =====
  getSelectedBusiness(): BusinessType | null {
    const stored =
      this.document.defaultView?.sessionStorage.getItem("selectedBusiness");
    return stored ? JSON.parse(stored) : null;
  }

  selectBusiness(business: BusinessType): void {
    this.document.defaultView?.sessionStorage.setItem(
      "selectedBusiness",
      JSON.stringify(business)
    );
    this.cdr.detectChanges();
  }

  // ===== CRUD =====
  goToAddBusiness(): void {
    this.document.defaultView?.sessionStorage.removeItem("selectedBusiness");
    this.router.navigate(["/business-form"]);
  }

  editBusiness(business?: BusinessType | null): void {
    const target = business ?? this.getSelectedBusiness();
    if (!target || target.id === undefined) {
      this.message.warning("Please select a business type to update.");
      return;
    }
    this.document.defaultView?.sessionStorage.setItem(
      "selectedBusiness",
      JSON.stringify(target)
    );
    this.router.navigate(["/business-form"], { queryParams: { isEdit: true } });
  }

  deleteSelectedBusiness(): void {
    const selected = this.getSelectedBusiness();
    if (!selected || selected.id === undefined) {
      this.message.warning("Please select a business type to delete.");
      return;
    }
    this.message.info(`Deleting business type: ${selected.businessTypeName}`);
    // TODO: Implement delete API call
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

  refreshBusiness(): void {
    this.loadBusinessTypes();
    this.message.success("Business list refreshed successfully!");
  }

  // Handle row selection from GridComponent
  onRowSelected(business: BusinessType | null): void {
    if (business) {
      this.selectBusiness(business);
    } else {
      this.document.defaultView?.sessionStorage.removeItem("selectedBusiness");
    }
    this.cdr.detectChanges();
  }

  // Handle search from GridComponent
  onSearch(value: string): void {
    this.searchControl.setValue(value, { emitEvent: false });
    this.filterBusinessTypes(value);
  }
}
