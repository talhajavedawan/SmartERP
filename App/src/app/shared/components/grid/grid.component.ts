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

// Register AG Grid modules
ModuleRegistry.registerModules([
  ClientSideRowModelModule,
  TextFilterModule,
  NumberFilterModule,
]);

@Component({
  selector: "app-grid",
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AgGridAngular,
    NzModalModule,
    DragDropModule,
  ],
  templateUrl: "./grid.component.html",
  styleUrls: ["./grid.component.css"],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GridComponent implements OnInit, OnChanges, OnDestroy {
  // ────────────────────── INPUTS ──────────────────────
  @Input() columnDefs: (ColDef | ColGroupDef)[] = [];
  @Input() rowData: any[] | null = [];
  @Input() domLayout: DomLayoutType = "autoHeight";
  @Input() title = "Grid";
  @Input() enableSearch = true;
  @Input() searchPlaceholder = "Search records...";
  @Input() showActions = true;
  @Input() layoutKey = "DefaultGrid";

  // ────────────────────── OUTPUTS ──────────────────────
  @Output() rowClicked = new EventEmitter<any>();
  @Output() onAdd = new EventEmitter<void>();
  @Output() onUpdate = new EventEmitter<any>();
  @Output() onRefresh = new EventEmitter<void>();
  @Output() onSearch = new EventEmitter<string>();
  @Output() onFilteredDataChange = new EventEmitter<any[]>();
  @Output() onRowSelected = new EventEmitter<any>();

  // ────────────────────── STATE ──────────────────────
  private gridApi!: GridApi;
  searchValue = new FormControl("");
  totalCount = 0;
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
    pagination: true,
    paginationPageSize: 25,
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
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["rowData"] && this.rowData) {
      this.originalRowData = [...this.rowData];
      this.totalCount = this.rowData.length;
      if (this.gridApi) this.applySearchFilter(this.searchValue.value || "");
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
    this.totalCount = this.rowData?.length || 0;
    this.loadSavedLayout();

    // Selection handling
    this.gridApi.addEventListener("selectionChanged", () => {
      this.selectedRows = this.gridApi.getSelectedRows();
      this.onRowSelected.emit(
        this.selectedRows.length === 1 ? this.selectedRows[0] : null
      );
      this.cdr.markForCheck();
    });

    // Row count update
    this.gridApi.addEventListener("modelUpdated", () => {
      this.totalCount = this.gridApi.getDisplayedRowCount();
      this.cdr.markForCheck();
    });
    this.gridApi.addEventListener("rowClicked", (event) => {
      if (event.node) {
        event.node.setSelected(true); // Force selection
      }
    });

    this.applySearchFilter(this.searchValue.value || "");
    this.gridApi.refreshHeader();
  }

  // ────────────────────── SEARCH ──────────────────────
  handleSearchInput(value: string): void {
    this.searchValue.setValue(value, { emitEvent: false });
    this.onSearch.emit(value);
    this.applySearchFilter(value);
  }

  private applySearchFilter(value: string): void {
    if (!this.gridApi || !this.originalRowData.length) {
      this.totalCount = this.rowData?.length || 0;
      this.onFilteredDataChange.emit(this.rowData || []);
      return;
    }

    const filtered = value
      ? this.originalRowData.filter((row) =>
          Object.values(row).some(
            (field) =>
              field &&
              typeof field === "string" &&
              field.toLowerCase().includes(value.toLowerCase())
          )
        )
      : this.originalRowData;

    this.gridApi.setGridOption("rowData", filtered);
    this.totalCount = filtered.length;
    this.onFilteredDataChange.emit(filtered);
    this.cdr.markForCheck();
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
    const newLayout = this.domLayout === "autoHeight" ? "normal" : "autoHeight";
    this.domLayout = newLayout;
    this.gridOptions.domLayout = newLayout;

    const gridContainer = document.querySelector(
      ".ag-grid-container"
    ) as HTMLElement;
    const agGrid = gridContainer?.querySelector(
      "ag-grid-angular"
    ) as HTMLElement;

    if (gridContainer && agGrid) {
      if (newLayout === "autoHeight") {
        // Let grid expand naturally
        gridContainer.style.height = "auto";
        agGrid.style.height = "auto";
      } else {
        // Fix container and grid height for "normal" mode
        gridContainer.style.height = "85vh";
        agGrid.style.height = "100%";
      }
    }

    this.gridApi?.refreshCells({ force: true });
    this.gridApi?.refreshHeader();
  }

  // ────────────────────── LAYOUT SAVE/LOAD ──────────────────────
  saveLayout(): void {
    if (!this.gridApi) return;

    this.modal.confirm({
      nzTitle: "Save Layout",
      nzContent:
        "Save current column order, width, visibility, and height mode?",
      nzOkText: "Save",
      nzCancelText: "Cancel",
      nzOnOk: () => {
        const state: ColumnState[] = this.gridApi.getColumnState();
        const layout = {
          columns: state,
          domLayout: this.domLayout, // store height preference
        };

        const json = JSON.stringify(layout);
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
        const layout = JSON.parse(json);

        // Restore column layout
        if (layout.columns) {
          this.gridApi.applyColumnState({
            state: layout.columns,
            applyOrder: true,
          });
        }

        // Restore domLayout (height mode)
        if (layout.domLayout) {
          this.domLayout = layout.domLayout as DomLayoutType;
          this.gridOptions.domLayout = this.domLayout;

          const gridContainer = document.querySelector(
            ".ag-grid-container"
          ) as HTMLElement;
          const agGrid = gridContainer?.querySelector(
            "ag-grid-angular"
          ) as HTMLElement;

          if (gridContainer && agGrid) {
            if (this.domLayout === "autoHeight") {
              gridContainer.style.height = "auto";
              agGrid.style.height = "auto";
            } else {
              gridContainer.style.height = "85vh";
              agGrid.style.height = "100%";
            }
          }

          // Ensure grid renders correctly after DOM update
          this.gridApi?.refreshCells({ force: true });
          this.gridApi?.refreshHeader();
        }
      } catch (err) {
        console.error("Failed to load layout:", err);
      }
    });
  }

  // ────────────────────── COLUMN MANAGER ──────────────────────
  openColumnVisibilityManager(): void {
    if (!this.gridApi) {
      this.message.warning("Grid not ready.");
      return;
    }

    const states = this.gridApi.getColumnState();
    this.columnManagerList = states.map((c) => {
      const colDef = this.columnDefs
        .flatMap((cd) => ("children" in cd ? cd.children : [cd as ColDef]))
        .find(
          (cd: any) => cd && (cd.field === c.colId || cd.colId === c.colId)
        );

      return {
        colId: c.colId,
        headerName: (colDef as any)?.headerName || c.colId,
        visible: !c.hide,
      };
    });

    this.columnManagerModal = this.modal.create({
      nzTitle: "Manage Columns",
      nzContent: this.columnManagerTemplate,
      nzWidth: 420,
      nzOkText: "Apply",
      nzCancelText: "Cancel",
      nzOnOk: () => this.applyColumnVisibilityChanges(),
    });
  }

  onColumnDrop(event: CdkDragDrop<any[]>): void {
    moveItemInArray(
      this.columnManagerList,
      event.previousIndex,
      event.currentIndex
    );
  }

  applyColumnVisibilityChanges(): void {
    if (!this.gridApi) return;

    this.columnManagerList.forEach((col) => {
      this.gridApi.setColumnsVisible([col.colId], col.visible);
    });

    const order = this.columnManagerList.map((c) => c.colId);
    this.gridApi.moveColumns(order, 0);
    this.gridApi.refreshHeader();
    this.gridApi.redrawRows();

    this.message.success("Columns updated!");
  }
}
