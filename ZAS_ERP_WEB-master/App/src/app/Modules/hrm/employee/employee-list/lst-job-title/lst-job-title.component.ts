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
  JobTitleService,
  JobTitle,
} from "../../../../../shared/services/company-center/company/lists/jobTitle.service";
import { UserSettingService } from "../../../../../shared/services/user-setting.service";
import { GridComponent } from "../../../../../shared/components/grid/grid.component";
import { ColDef, ICellRendererParams } from "ag-grid-community";
import { formatPKTDate } from "../../../../../shared/components/dateTime.util";
@Component({
  selector: "app-lst-job-title",
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzModalModule, GridComponent],
  templateUrl: "./lst-job-title.component.html",
  styleUrls: ["../../../../../../scss/lists.css"],
})
export class LstJobTitleComponent implements OnInit, OnDestroy {
  // ===== Search & State =====
  searchControl = new FormControl("");
  jobTitles: JobTitle[] = [];
  filteredJobTitles: JobTitle[] = [];
  isLoading = false;
  selectedStatus: string = "all";
  readonly GRID_LAYOUT_KEY = "jobtitle-grid-layout";
  private subscriptions = new Subscription();

  // ===== Grid Config =====
  columns: ColDef[] = [
    {
      field: "jobTitleName",
      headerName: "Job Title",
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
    },
    {
      field: "createdDate",
      headerName: "Creation Date",
      minWidth: 180,
      hide: false,
      filter: "agDateColumnFilter",
      valueFormatter: (params: any) =>
        params.value ? formatPKTDate(params.value) : "—",
    },
    {
      field: "lastModifiedByUserName",
      headerName: "Last Modified By",
      minWidth: 150,
      hide: false,
      filter: "agTextColumnFilter",
    },
    {
      field: "lastModifiedDate",
      headerName: "Last Modified Date",
      minWidth: 180,
      hide: false,
      filter: "agDateColumnFilter",
      valueFormatter: (params: any) =>
        params.value ? formatPKTDate(params.value) : "—",
    },
  ];

  constructor(
    private jobTitleService: JobTitleService,
    private message: NzMessageService,
    private userSettingService: UserSettingService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    @Inject(DOCUMENT) private document: Document
  ) {}

  // ===== Lifecycle =====
  ngOnInit(): void {
    this.loadGridLayout();
    this.loadJobTitles();
    this.subscriptions.add(
      this.searchControl.valueChanges
        .pipe(debounceTime(300), distinctUntilChanged())
        .subscribe((val: string | null) => this.filterJobTitles(val ?? ""))
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
  loadJobTitles(): void {
    this.isLoading = true;
    this.subscriptions.add(
      this.jobTitleService.getAllJobTitles(this.selectedStatus).subscribe({
        next: (data: JobTitle[]) => {
          // NOTE: backend DTO should return createdByUserName/createdDate/lastModifiedByUserName/lastModifiedDate
          this.jobTitles = data || [];
          this.filteredJobTitles = [...this.jobTitles];
          this.isLoading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error("Error loading job titles:", err);
          this.isLoading = false;
          this.message.error("Failed to load job titles.");
        },
      })
    );
  }

  setFilterState(status: string): void {
    this.selectedStatus = status;
    this.loadJobTitles();
  }

  filterJobTitles(value: string = ""): void {
    const searchVal = value.trim().toLowerCase();
    this.filteredJobTitles = this.jobTitles.filter((i) =>
      (i.jobTitleName || "").toLowerCase().includes(searchVal)
    );
    this.cdr.detectChanges();
  }

  // ===== Row & CRUD =====
  onRowClicked(event: any): void {
    const jobTitle = event?.data as JobTitle;
    this.selectJobTitle(jobTitle);
  }

  goToAddJobTitle(): void {
    this.document.defaultView?.sessionStorage.removeItem("selectedJobTitle");
    this.router.navigate(["/Job-title-form"]);
  }

  editJobTitle(jobTitle?: JobTitle | null): void {
    const target = jobTitle ?? this.getSelectedJobTitle();
    if (!target || target.id === undefined) {
      this.message.warning("Please select a job title to update.");
      return;
    }
    this.document.defaultView?.sessionStorage.setItem(
      "selectedJobTitle",
      JSON.stringify(target)
    );
    this.router.navigate(["/Job-title-form"], {
      queryParams: { isEdit: true },
    });
  }

  deleteSelectedJobTitle(): void {
    const selected = this.getSelectedJobTitle();
    if (!selected || selected.id === undefined) {
      this.message.warning("Please select a job title to delete.");
      return;
    }
    this.message.info(`Deleting job title: ${selected.jobTitleName}`);
    // TODO: Implement delete API call
  }

  getSelectedJobTitle(): JobTitle | null {
    const stored =
      this.document.defaultView?.sessionStorage.getItem("selectedJobTitle");
    return stored ? JSON.parse(stored) : null;
  }

  selectJobTitle(jobTitle: JobTitle): void {
    this.document.defaultView?.sessionStorage.setItem(
      "selectedJobTitle",
      JSON.stringify(jobTitle)
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

  refreshJobTitles(): void {
    this.loadJobTitles();
    this.message.success("Job titles refreshed successfully!");
  }

  // ===== Row & Search from Grid =====
  onRowSelected(jobTitle: JobTitle | null): void {
    if (jobTitle) {
      this.selectJobTitle(jobTitle);
    } else {
      this.document.defaultView?.sessionStorage.removeItem("selectedJobTitle");
    }
    this.cdr.detectChanges();
  }

  onSearch(value: string): void {
    this.searchControl.setValue(value, { emitEvent: false });
    this.filterJobTitles(value);
  }
}
