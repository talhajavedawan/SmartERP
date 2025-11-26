import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnChanges,
  OnDestroy,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  SimpleChanges,
  TemplateRef,
  ViewChild,
} from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule, FormControl } from "@angular/forms";
import { AgGridAngular } from "ag-grid-angular";
import {
  ColDef,
  GridApi,
  GridOptions,
  GridReadyEvent,
  DomLayoutType,
  ColGroupDef,
  ColumnState,
  ClientSideRowModelModule,
  TextFilterModule,
  NumberFilterModule,
  ModuleRegistry,
} from "ag-grid-community";
import { UserSettingService } from "../../../shared/services/user-setting.service";
import { NzMessageService } from "ng-zorro-antd/message";
import { NzModalService, NzModalModule, NzModalRef } from "ng-zorro-antd/modal";
import {
  DragDropModule,
  CdkDragDrop,
  moveItemInArray,
} from "@angular/cdk/drag-drop";
import { NzPaginationModule } from "ng-zorro-antd/pagination";
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
import { CeilPipe } from "../pipes/ceil.pipe";
import { NzToolTipModule } from "ng-zorro-antd/tooltip";

// Register AG Grid modules
ModuleRegistry.registerModules([
  ClientSideRowModelModule,
  TextFilterModule,
  NumberFilterModule,
]);

@Component({
  selector: "app-grid-layout",
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AgGridAngular,
    NzModalModule,
    DragDropModule,
    NzPaginationModule,
    CeilPipe,
    NzToolTipModule 
  ],
  templateUrl: "./grid-layout.component.html",
  styleUrls: ["./grid-layout.component.css"],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GridLayoutComponent implements OnInit, OnChanges, OnDestroy {
  // ────────────────────── INPUTS ──────────────────────
  @Input() columnDefs: (ColDef | ColGroupDef)[] = [];
  @Input() rowData: any[] | null = [];
  @Input() domLayout: DomLayoutType = "autoHeight";
  @Input() title = "Grid";
  @Input() enableSearch = true;
  @Input() searchPlaceholder = "Search records...";
  @Input() showActions = true;
  @Input() layoutKey = "DefaultGrid";
public currentDomLayout: DomLayoutType = 'normal';
  // ────────────────────── OUTPUTS ──────────────────────
  @Input() showHeader: boolean = true;

  @Output() rowClicked = new EventEmitter<any>();
  @Output() onAdd = new EventEmitter<void>();
  @Output() onUpdate = new EventEmitter<any>();
  @Output() onRefresh = new EventEmitter<void>();
  @Output() onSearch = new EventEmitter<string>();
  @Output() onFilteredDataChange = new EventEmitter<any[]>();
  @Output() onRowSelected = new EventEmitter<any>();
  @Output() onPageChange = new EventEmitter<number>();
  @Output() onPageSizeChange = new EventEmitter<number>();
  @Output() onSortChange = new EventEmitter<{
    sortColumn: string;
    sortDirection: string;
  }>();

  @Input() pageNumber = 1;
  @Input() pageSize = 10;
  @Input() totalCount = 0;

  // ────────────────────── STATE ──────────────────────
  private gridApi!: GridApi;
  searchValue = new FormControl("");
  selectedRows: any[] = [];
  private originalRowData: any[] = [];
  isRefreshing = false;

  gridOptions: GridOptions = {
    animateRows: true,
    rowSelection: "multiple",
    suppressRowClickSelection: false,
    defaultColDef: {
      resizable: true,
      sortable: true,
      filter: true,
      minWidth: 50,
    },
    pagination: false,
  };

  @ViewChild("columnManagerTemplate", { static: true })
  columnManagerTemplate!: TemplateRef<any>;
  columnManagerList: { colId: string; headerName: string; visible: boolean }[] =
    [];
  private columnManagerModal?: NzModalRef;

  constructor(
    private cdr: ChangeDetectorRef,
    private userSettingService: UserSettingService,
    private modal: NzModalService,
    private message: NzMessageService
  ) {}

  // ────────────────────── LIFECYCLE ──────────────────────
  ngOnInit(): void {
    this.setupColumns();

    // ✅ Debounced server-side search
    this.searchValue.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe((value) => {
        const trimmed = (value || "").trim();
        this.onSearch.emit(trimmed);
        console.log("🔎 Server-side search triggered:", trimmed);
      });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["rowData"] && this.rowData) {
      this.originalRowData = [...this.rowData];
      // this.totalCount = this.rowData.length;
      this.cdr.markForCheck();
    }
  }

  ngOnDestroy(): void {}

  // ────────────────────── SETUP HELPERS ──────────────────────
  private setupColumns(): void {
    const selectionCol: ColDef = {
      headerName: "",
      checkboxSelection: true,
      headerCheckboxSelection: true,
      pinned: "left",
      width: 40,
      sortable: false,
      filter: false,
      resizable: false,
    };

    this.columnDefs = [
      selectionCol,
      ...this.columnDefs.map((col: ColDef | ColGroupDef) => {
        if ("children" in col) {
          return {
            ...col,
            children: col.children.map((child: ColDef) => ({
              ...child,
              minWidth: 50,
              filter: child.filter || "agTextColumnFilter",
            })),
          };
        }
        return {
          ...col,
          minWidth: 50,
          filter: col.filter || "agTextColumnFilter",
        };
      }),
    ];
  }

  // ────────────────────── GRID READY ──────────────────────
  onGridReady(params: GridReadyEvent): void {
    this.gridApi = params.api;
    this.originalRowData = [...(this.rowData || [])];
    // this.totalCount = this.rowData?.length || 0;
    this.loadSavedLayout();

    this.gridApi.addEventListener("selectionChanged", () => {
      this.selectedRows = this.gridApi.getSelectedRows();
      this.onRowSelected.emit(
        this.selectedRows.length === 1 ? this.selectedRows[0] : null
      );
      this.cdr.markForCheck();
    });

    this.gridApi.addEventListener("modelUpdated", () => {
      //this.totalCount = this.gridApi.getDisplayedRowCount();
      this.cdr.markForCheck();
    });

    this.gridApi.addEventListener("rowClicked", (event) => {
      if (event.node) event.node.setSelected(true);
    });

    this.gridApi.refreshHeader();
  }

  // ────────────────────── ACTIONS ──────────────────────
  triggerRefresh(): void {
    if (this.isRefreshing) return;
    this.isRefreshing = true;
    this.onRefresh.emit();
    setTimeout(() => {
      this.isRefreshing = false;
      this.cdr.markForCheck();
    }, 1500);
  }

  get canEdit(): boolean {
    return this.selectedRows.length === 1;
  }

  get canExport(): boolean {
    return this.selectedRows.length > 0;
  }

  updateSelectedRow(): void {
    if (!this.canEdit) {
      this.message.warning("Please select exactly ONE row to update.");
      return;
    }
    this.onUpdate.emit(this.selectedRows[0]);
  }

  exportSelectedRows(): void {
    if (!this.canExport) {
      this.message.warning("Please select rows to export.");
      return;
    }
    this.gridApi.exportDataAsCsv({ onlySelected: true });
  }

toggleHeight(): void {
  // Toggle between normal (fixed height) and autoHeight
  this.currentDomLayout = this.currentDomLayout === 'normal' ? 'autoHeight' : 'normal';

  // Ye hai asli working method ab AG Grid v28+ mein
  this.gridOptions.domLayout = this.currentDomLayout;

  if (this.gridApi) {
    // Grid ko batao layout change hua hai
    this.gridApi.setGridOption('domLayout', this.currentDomLayout);

    // AutoHeight ke liye row heights reset karo
    if (this.currentDomLayout === 'autoHeight') {
      this.gridApi.resetRowHeights();
    }

    // Fixed height mein columns fit kar do (perfect look)
    if (this.currentDomLayout === 'normal') {
      setTimeout(() => {
        this.gridApi.sizeColumnsToFit();
      }, 100);
    }

    // Force refresh
    this.gridApi.refreshCells();
    this.cdr.markForCheck();
  }
}

  // ────────────────────── LAYOUT SAVE/LOAD ──────────────────────
  saveLayout(): void {
    if (!this.gridApi) return;

    this.modal.confirm({
      nzTitle: "Save Layout",
      nzContent: "Save current column order, width, and visibility?",
      nzOkText: "Save",
      nzCancelText: "Cancel",
      nzOnOk: () => {
        const state: ColumnState[] = this.gridApi.getColumnState();
        const json = JSON.stringify(state);
        this.userSettingService.saveSetting(this.layoutKey, json).subscribe({
          next: () => this.message.success("Layout saved!"),
          error: () => this.message.error("Failed to save layout."),
        });
      },
    });
  }

  loadSavedLayout(): void {
    if (!this.gridApi) return;
    this.userSettingService.getSetting(this.layoutKey).subscribe((json) => {
      if (!json) return;
      try {
        const state: ColumnState[] = JSON.parse(json);
        this.gridApi.applyColumnState({ state, applyOrder: true });
      } catch (err) {
        console.error("Failed to load layout:", err);
      }
    });
  }

  // ────────────────────── COLUMN MANAGER ──────────────────────
// ────────────────────── COLUMN MANAGER ──────────────────────
openColumnVisibilityManager(): void {
  if (!this.gridApi) {
    this.message.warning("Grid not ready.");
    return;
  }

  // 🔹 Get ALL columns from AG Grid API (includes visible + hidden)
  const allColumns = this.gridApi.getColumnDefs();

  // 🔹 Build list for modal (colId, headerName, visibility)
  this.columnManagerList = (allColumns || [])
    .filter((col: any) => !!col.field || !!col.colId) // ignore group or blank defs
    .map((col: any) => ({
      colId: col.colId || col.field,
      headerName: col.headerName || col.field || "(Unnamed)",
      visible: this.gridApi.getColumnDef(col.colId || col.field)?.hide !== true,
    }));

  // 🔹 Open Modal
  this.columnManagerModal = this.modal.create({
    nzTitle: "Manage Columns",
    nzContent: this.columnManagerTemplate,
    nzWidth: 420,
    nzOkText: "Apply",
    nzCancelText: "Cancel",
    nzOnOk: () => this.applyColumnVisibilityChanges(),
  });
}

// 🔹 Drag & Drop Handler (for reordering)
onColumnDrop(event: CdkDragDrop<any[]>): void {
  moveItemInArray(
    this.columnManagerList,
    event.previousIndex,
    event.currentIndex
  );
}

// 🔹 Apply Visibility & Order changes to Grid
applyColumnVisibilityChanges(): void {
  if (!this.gridApi) return;

  // Update visibility
  this.columnManagerList.forEach((col) => {
    this.gridApi.setColumnsVisible([col.colId], col.visible);
  });

  // Update order
  const order = this.columnManagerList.map((c) => c.colId);
  this.gridApi.moveColumns(order, 0);

  this.gridApi.refreshHeader();
  this.gridApi.redrawRows();
  this.message.success("Columns updated!");
}


  onSortChanged(): void {
    if (!this.gridApi) return;
    const sortModel = (this.gridApi as any).getSortModel();
    if (!sortModel || sortModel.length === 0) {
      this.onSortChange.emit({ sortColumn: "Id", sortDirection: "asc" });
      return;
    }
    const sortColumn = sortModel[0].colId;
    const sortDirection = sortModel[0].sort;
    this.onSortChange.emit({ sortColumn, sortDirection });
  }

  showTotal(total: number): string {
  return `Total ${total} records`;
}

}
